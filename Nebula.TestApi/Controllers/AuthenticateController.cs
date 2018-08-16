using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Extensions;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.Membership;
using Nebula.Membership.Visitors;
using Nebula.TestConsole;

namespace Nebula.TestApi.Controllers
{

    public class OrdinoApprove : IMembershipVisitor
    {
        public void Visit(IMembershipManager membershipManager)
        {
            Console.WriteLine("ordino onaylandı");
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateController : Controller
    {
        private readonly IMembershipManager _membershipManager;
        private readonly IRepository<SampleEntity> _sampleRepository;

        public AuthenticateController(IRepository<SampleEntity> sampleRepository)
        {
            _sampleRepository = sampleRepository;
            _membershipManager = DependencyService.GetService<IMembershipManager>();
        }

        public IActionResult Login([FromBody]LoginModel model)
        {
            try
            {
                

                string token = _membershipManager.SignIn(model.Email, model.Password);
                
                _membershipManager.Accept(new OrdinoApprove());

                _membershipManager.UserQuery().AsNoTracking();
                


                return Ok(token);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        public IActionResult Update([FromBody] User model)
        {
            
            var user = _membershipManager.UserFind(model.Id);
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = model.Password.HashString();

            user.UpdatedBy = model.UpdatedBy;
            user.DeletedBy = model.DeletedBy;
            
            _membershipManager.UserUpdate(user);
            return Ok(user);
        }
        
        public IActionResult Delete([FromBody] SampleEntity model)
        {
            
            return Ok(model);
        }


        [NebulaAuthorize("UserManagement")]
        public IActionResult Verify()
        {
            return Ok();
        }
    }
}