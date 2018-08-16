using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nebula.ClientLibrary;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.Dapper.PostgreSql;
using Nebula.DataAccessLibrary.TableCreaters;
using Nebula.Membership;
using Nebula.Membership.Repositories;
using Nebula.Membership.Repositories.Postgresql;

namespace Nebula.TestApi
{
    public class ApiBootstrapper : ClientBootstrapper
    {
        public override void DependencyResolving(IContainer container)
        {
            base.DependencyResolving(container);

        }
    }
}
