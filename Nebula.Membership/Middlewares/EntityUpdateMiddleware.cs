using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Nebula.CoreLibrary.Extensions;
using Nebula.Security.Bearer.Helpers;

namespace Nebula.Membership.Middlewares
{
    public class EntityUpdateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionManager _exceptionManager;

        public EntityUpdateMiddleware(RequestDelegate next)
        {
            this._next = next;
            _exceptionManager = DependencyService.GetService<IExceptionManager>();
            
        }

        public async Task Invoke(HttpContext context) {
            var request = context.Request;
            var create = false;
            var update = false;
            var delete = false;

            if (request.Path.Value.ToLower().Contains("create"))
            {
                create = true;
                update = false;
                delete = false;
            }
            else if (request.Path.Value.ToLower().Contains("update"))
            {
                create = false;
                update = true;
                delete = false;
            }
            else if (request.Path.Value.ToLower().Contains("delete"))
            {
                create = false;
                update = false;
                delete = true;
            }
            
            if (create || update || delete) {
                //get the request body and put it back for the downstream items to read
                var stream = request.Body;// currently holds the original stream                    
                var originalContent = new StreamReader(stream).ReadToEnd();
                var notModified = true;
                try {
                    dynamic dataSource = JsonConvert.DeserializeObject<dynamic>(originalContent);
                    if (dataSource != null)
                    {
                        
                        var user = FindUserByContext(context);
                        if (user == null)
                        {
                            return;
                        }
                        
                        if (create)
                        {
                            dataSource.CreatedBy = user.Id;
                        }
                        else if (update)
                        {
                            dataSource.UpdatedBy = user.Id;
                        }
                        else if (delete)
                        {
                            dataSource.DeletedBy = user.Id;
                        }
                        
                        var json = JsonConvert.SerializeObject(dataSource);
                        //replace request stream to downstream handlers
                        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                        stream = await requestContent.ReadAsStreamAsync();//modified stream
                        notModified = false;
                    }
                } catch {
                    //No-op or log error
                }
                if (notModified) {
                    //put original data back for the downstream to read
                    var requestData = Encoding.UTF8.GetBytes(originalContent);
                    stream = new MemoryStream(requestData);
                }

                request.Body = stream;
            }
            await _next.Invoke(context);
        }

        private User FindUserByContext(HttpContext context)
        {
            var request = context.Request;
            var token = request.Headers["Authorization"].ToString();
            token = token.Replace("Bearer ", "");
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.IssuerSigningKey = JwtSecurityKey.Create("fiver-secret-key");
            validationParameters.ValidAudience = "fiver.Security.Bearer";
            validationParameters.ValidIssuer = "fiver.Security.Bearer";
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);
            Guid userid = principal.FindFirst("UserId").Value.ConvertGuid();
            IMembershipManager userService = DependencyService.GetService<IMembershipManager>();
            var user = userService.UserFind(userid);
            return user;
        }
        
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is UnauthorizedAccessException) code = HttpStatusCode.OK;
            else if (exception is BadHttpRequestException) code = HttpStatusCode.OK;


            var responseModel = new 
            {
                Status = false,
                ErrorMessage = exception.Message
            };
            var serializerSettings =
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var result = JsonConvert.SerializeObject(responseModel, serializerSettings);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}