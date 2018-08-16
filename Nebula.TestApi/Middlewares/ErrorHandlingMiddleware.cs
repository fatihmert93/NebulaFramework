using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using System.Threading.Tasks;
using Nebula.TestApi.Models;

namespace Nebula.TestApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IExceptionManager _exceptionManager;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
            _exceptionManager = DependencyService.GetService<IExceptionManager>();
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                _exceptionManager.Handle(ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is UnauthorizedAccessException) code = HttpStatusCode.OK;
            else if (exception is BadHttpRequestException) code = HttpStatusCode.OK;


            var responseModel = new ResponseModel
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