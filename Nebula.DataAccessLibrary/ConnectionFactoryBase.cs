using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary
{
    public abstract class ConnectionFactoryBase : IConnectionFactory
    {
        private string _connectionString;

        protected abstract DbProviderFactory GetProviderFactory();

        protected virtual string GetConnectionString()
        {
            if (ConfigurationManager.AppSettings["connectionString"] != null)
                _connectionString = ConfigurationManager.AppSettings["connectionString"];
            else if (!string.IsNullOrEmpty(ApplicationSettings.ConnectionString))
                _connectionString = ApplicationSettings.ConnectionString;
            else if (ApplicationSettings.AppSettings["connectionString"] != null)
                _connectionString = ApplicationSettings.AppSettings["connectionString"].ToString();
            return _connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            this._connectionString = connectionString;
        }


        public IDbConnection Connection
        {
            get
            {
                var factory = GetProviderFactory();
                //var factory = DbProviderFactories.GetDbProviderFactory(DataAccessProviderTypes.PostgreSql);
                var connection = factory.CreateConnection();
                if (connection == null) throw new Exception("There is available connection factory!");
                connection.ConnectionString = GetConnectionString();
                connection.Open();
                return connection;
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                Connection.Close();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposedValue = true;
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ConnectionFactory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
