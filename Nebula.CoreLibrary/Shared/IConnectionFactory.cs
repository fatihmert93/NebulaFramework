using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface IConnectionFactory : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
