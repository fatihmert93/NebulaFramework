using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface ILogManager
    {
        void Write(string message);
        void Write(string message, string logType);

        void WriteError(string message, Exception ex);
    }
}
