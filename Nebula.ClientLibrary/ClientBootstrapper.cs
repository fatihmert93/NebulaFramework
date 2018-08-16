
using Nebula.CacheLibrary.Microsoft;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.ExceptionLibrary;
using Nebula.LogLibrary;
using Nebula.DataAccessLibrary.Dapper.PostgreSql;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.DataAccessLibrary.EntityFramework.PostgreSql;
using Nebula.DataAccessLibrary.TableCreaters;
using Nebula.FileServiceLibrary;
using Nebula.MailLibrary;
using Nebula.Membership;
using Nebula.Membership.Repositories;
using Nebula.Membership.Repositories.EntityFramework.Postgresql;
using Nebula.Membership.Repositories.Postgresql;
using Nebula.SeedLib;
using Nebula.SystemSettings;
using Nebula.SystemSettings.Repositories;
using Nebula.SystemSettings.Repositories.EntityFramework.Postgresql;

namespace Nebula.ClientLibrary
{
    public class ClientBootstrapper : Bootstrapper
    {
        public override void DependencyResolving(IContainer container)
        {
            base.DependencyResolving(container);

            container.Register<ICacheManager, MemoryCacheManager>();
            container.Register<IExceptionManager, ExceptionManager>();

            container.RegisterTransient<IDbContextFactory, DbContextFactory>();
            container.Register<IConnectionFactory,NpgConnectionFactory>();
            container.Register<ILogRepository, EFNpgLogRepository>();
            container.Register<ILogManager, SqlLogger>();
            container.Register<IMailService,MailService>();
            container.Register<IFileUploader,AwsS3FileUploader>();

            container.Register<ITableCreator,NpgTableCreator>();
            container.Register<IUserRepository, EFNpgUserRepository>();
            container.Register<IUserGroupRepository, EFNpgUserGroupRepository>();
            container.Register<IUserGroupRoleRepository, EFNpgUserGroupRoleRepository>();
            container.Register<IRoleRepository, EFNpgRoleRepository>();
            container.Register<ICompanyRepository, EFNpgCompanyRepository>();

            container.Register(typeof(IRepository<>),typeof(EFRepository<>));
            container.Register<IRepository, EFRepository>();

            // membership
            container.Register<IUserService, UserService>();
            container.Register<IUserGroupService, UserGroupService>();
            container.Register<ICompanyService,CompanyService>();
            container.Register<IMembershipManager, MembershipManager>();
            container.Register<ISeedManager,SeedManager>();


            // system settings
            container.Register<ISystemSettingsRepository,EFNpgSystemSettingsRepository>();
            container.Register<ISettingsManager,SettingsManager>();
        }
    }
}
