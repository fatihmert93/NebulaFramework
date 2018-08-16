using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Nebula.DataAccessLibrary.Dapper.Sql
{
    public class SqlConnectionFactory : ConnectionFactoryBase
    {
        protected override DbProviderFactory GetProviderFactory()
        {
            var factory = DbProviderFactories.GetDbProviderFactory(DataAccessProviderTypes.SqlServer);
            return factory;
        }
    }
}
