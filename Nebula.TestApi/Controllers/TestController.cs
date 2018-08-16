using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Edm.Csdl;
using Microsoft.EntityFrameworkCore;
using Nebula.CoreLibrary.Shared;
using Nebula.Membership;
using Nebula.Membership.Repositories;
using SQLitePCL;

namespace Nebula.TestApi.Controllers
{

    public class DenemeEntity : EntityBase
    {
        public string Name { get; set; }
    }
    
    
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        
        public TestController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        
        
        
        
        // GET
        public IActionResult Index()
        {
            var company = new Company();
            company.CommercialName = "datasoft";
            company.Name = "datasoft";
            
            _companyRepository.Create(company);
            
            
            
            return Ok();
        }

//        public async Task<List<User>> GetUser()
//        {
//            var user = _concext.Set<User>().ToListAsync();
//            var userCount = _concext.Set<User>().CountAsync();
//            await Task.WhenAll(user, userCount);
//            return await user;
//        }
    }
}