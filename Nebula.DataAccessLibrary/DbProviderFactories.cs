using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Nebula.CoreLibrary.Utilities;
using Westwind.Utilities;
using Westwind.Utilities.Properties;

namespace Nebula.DataAccessLibrary
{
    public enum DataAccessProviderTypes
    {
        SqlServer,
        SqLite,
        MySql,
        PostgreSql
    }

    public class DbProviderFactories
    {
        private static DbProviderFactory GetDbProviderFactory(string dbProviderFactoryTypeName, string assemblyName)
        {
            var instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypeName, "Instance");
            if (instance == null)
            {
                var a = ReflectionUtils.LoadAssembly(assemblyName);
                if (a != null)
                    instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypeName, "Instance");
            }

            if (instance == null)
                throw new InvalidOperationException(string.Format(Resources.UnableToRetrieveDbProviderFactoryForm, dbProviderFactoryTypeName));

            return instance as DbProviderFactory;
        }


        public static DbProviderFactory GetDbProviderFactory(DataAccessProviderTypes type)
        {
            switch (type)
            {
                case DataAccessProviderTypes.SqlServer:
                    return SqlClientFactory.Instance;
                case DataAccessProviderTypes.SqLite:
                    return GetDbProviderFactory("Microsoft.Data.Sqlite.SqliteFactory", "Microsoft.Data.Sqlite");
                case DataAccessProviderTypes.MySql:
                    return GetDbProviderFactory("MySql.Data.MySqlClient.MySqlClientFactory", "MySql.Data");
                case DataAccessProviderTypes.PostgreSql:
                    return GetDbProviderFactory("Npgsql.NpgsqlFactory", "Npgsql");
                default:
                    throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, type.ToString()));
            }
        }

        public static DbProviderFactory GetDbProviderFactory(string providerName)
        {
            var providername = providerName.ToLower();

            if (providerName == "system.data.sqlclient")
                return GetDbProviderFactory(DataAccessProviderTypes.SqlServer);
            if (providerName == "system.data.sqlite" || providerName == "microsoft.data.sqlite")
                return GetDbProviderFactory(DataAccessProviderTypes.SqLite);
            if (providerName == "mysql.data.mysqlclient" || providername == "mysql.data")
                return GetDbProviderFactory(DataAccessProviderTypes.MySql);
            if (providerName == "npgsql")
                return GetDbProviderFactory(DataAccessProviderTypes.PostgreSql);

            throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, providerName));
        }


    }
}
