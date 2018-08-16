using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary.Dapper.PostgreSql
{
    public class NpgConnectionFactory : ConnectionFactoryBase
    {

        protected override DbProviderFactory GetProviderFactory()
        {
            var factory = DbProviderFactories.GetDbProviderFactory(DataAccessProviderTypes.PostgreSql);
            return factory;
        }
    }
}
