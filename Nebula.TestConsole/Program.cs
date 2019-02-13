
using Nebula.ClientLibrary;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Amazon.Util.Internal.PlatformServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Aspects.DynamicProxy;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership;
using Nebula.Membership.Visitors;
using Nebula.SeedLib;
using ApplicationSettings = Nebula.CoreLibrary.ApplicationSettings;
using IContainer = Nebula.CoreLibrary.IOC.IContainer;


namespace Nebula.TestConsole
{

    public interface ISampleBusiness
    {


        List<LogEntity> Sample(string prm);
    }

    
    public class Business : ISampleBusiness
    {
        private readonly ILogRepository _logRepository;
        private ILogManager _logManager;
        public Business(ILogRepository logRepository,ILogManager logManager)
        {
            _logRepository = logRepository;
            _logManager = logManager;
        }

        [CacheRemoveAspect]
        public void Insert()
        {

        }

        [CacheAspect()]
        public List<LogEntity> Sample(string prm)
        {
            //LogEntity logEntity = new LogEntity();
            //logEntity.Message = "business çalıştı";
            //logEntity.LogType = "Log";
            //_logRepository.Create(logEntity);
            //_logManager.Write("business çalıştı");
            var logs = _logRepository.Query().ToList();
            return logs;
        }
    }


    public class Student
    {
        public int StudentId { get; set; }

        public string Name { get; set; }

        public int FacultyId { get; set; }
        public int DepartmentId { get; set; }

    }

    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }


    public class Faculty
    {
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
    }


    public class SspBootstrapper : ClientBootstrapper
    {
        public override void DependencyResolving(IContainer container)
        {
            base.DependencyResolving(container);
            container.Register<ISampleBusiness, Business>();
        }
    }

    public class SampleEntity : EntityBase
    {
        public int Sample { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime Nullsuz { get; set; }
        public float Para { get; set; }
        public string Emrah { get; set; }
        [DefaultValue(false)]
        public bool? DenemeBool { get; set; }

        public bool Deneme2 { get; set; }
        
    }
    
    public class SampleEntity1 : EntityBase
    {
        public int Sample { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime Nullsuz { get; set; }
        public float Para { get; set; }
        public string Emrah { get; set; }
        [DefaultValue(false)]
        public bool? DenemeBool { get; set; }

        public bool Deneme2 { get; set; }
        public string Fatih { get; set; }
    }

    public class OrdinoApprove : IMembershipVisitor
    {
        public void Visit(IMembershipManager membershipManager)
        {
            Console.WriteLine("Ordino onaylandı.");
        }
    }

    static class Program
    {


        static void Main(string[] args)
        {
            ApplicationSettings.ConnectionString =
                "server=superdoctor.cnc1ajzvqewo.eu-central-1.rds.amazonaws.com;database=superdoctor;user id=spadmin;password=spr31415SD;";
            SspBootstrapper sspBootstrapper = new SspBootstrapper();
            sspBootstrapper.Start();

            ITableCreator tableCreator = DependencyService.GetService<ITableCreator>();
            tableCreator.CreateAllTable<EntityBase>();


            
//            IRepository<SampleEntity> sRepository = DependencyService.GetService<IRepository<SampleEntity>>();
//            
//            var sample = new SampleEntity();
//            sample.Emrah = "emrah1";
//            sample.IsDeleted = true;
//            sRepository.Create(sample);
//            
//            var samle = new SampleEntity();
//            samle.Emrah = "emrah2";
//            samle.IsDeleted = false;
//            sRepository.Create(samle);
//
//            var list = sRepository.Query().ToList();

            //dynamic repository = DependencyService.GetService(typeof(IRepository));

            string temp = "";

            //IMembershipManager membershipManager = DependencyService.GetService<IMembershipManager>();


            //ISeedManager seedManager = DependencyService.GetService<ISeedManager>();
            //seedManager.SeedType = SeedType.Dev;
            //seedManager.ExecuteAll();

            ////RelationalDatabaseCreator databaseCreator =
            ////    (RelationalDatabaseCreator)_context.Database.GetService<IDatabaseCreator>();
            ////  databaseCreator.CreateTables();

            ////DbContextFactory<NebulaNpgEntityContext>.Instance.CurrentContext.Database.Migrate();
            ////string script = DbContextFactory<NebulaNpgEntityContext>.Instance.CurrentContext.Database.GenerateCreateScript();

            //var role = new Role();
            //role.Key = "UserManagement";

            //UserGroup userGroup = new UserGroup();
            //userGroup.Name = "Administrator";

            //Company company = new Company();
            //company.Name = "Medyanet";




            //IMembershipManager membershipManager = DependencyService.GetService<IMembershipManager>();

            //var users = membershipManager.UserQuery().Include(v => v.UserGroup).Include(v => v.Company).Include(v => v.Manager).ToList();
            //string str = "";
            //membershipManager.CreateRole(role);

            //UserGroupRole ugr = new UserGroupRole();
            //ICollection<UserGroupRole> ugrlist = new List<UserGroupRole>();
            //ugrlist.Add(ugr);
            //ugr.RoleId = role.Id;
            //userGroup.UserGroupRoles = ugrlist;
            //membershipManager.CreateUserGroup(userGroup);
            //membershipManager.CreateCompany(company);


            //var user = new Users()
            //{
            //    Email = "fatihmert93@gmail.com",
            //    Name = "Fatih",
            //    Lastname = "Mert",
            //    Password = "12345",
            //    UserGroupId = userGroup.Id,
            //    Username = "fatihmert",
            //    CompanyId = company.Id
            //};

            //membershipManager.CreateUser(user);

            //var users = membershipManager.GetAllUsers().Include(v => v.UserGroup).Include(v => v.Company).Include(v => v.Manager).ToList();







            ////var logss = sampleBusiness.Sample("abc");

            ////var logs2 = sampleBusiness.Sample("abc");

            ////var logs3 = sampleBusiness.Sample("abcd");

            ////var logs4 = sampleBusiness.Sample("abc");

            //LogEntity logEntity = new LogEntity()
            //{
            //    JsonMessage = "mesage",
            //    LogType = "error",
            //    Message = "error message",
            //    CreateDate = DateTime.Now,
            //    UpdateDate = DateTime.Now
            //};




            //Console.ReadKey();
        }
    }
}
