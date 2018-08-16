using System;

namespace Nebula.ExceptionCore
{
    public class NebulaException : Exception
    {
        public NebulaException()
        {

        }

        public NebulaException(string errorMessage) : base(errorMessage)
        {

        }
        public string ErrorCode { get; set; }
    }
}
