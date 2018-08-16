using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public class LogEntity : EntityBase
    {
        public string Message { get; set; }
        public string LogType { get; set; }
        public string JsonMessage { get; set; }
    }
}
