using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface IDistributedCacheManager : ICacheManager
    {
        string ConnectionString { get; set; }
        string InstanceName { get; set; }
    }
}
