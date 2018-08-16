using Nebula.CoreLibrary.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.LogLibrary
{
    public class TextLogger : ILogManager
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Write(string message, string logType)
        {
            Console.WriteLine(message);
        }

        public void WriteError(string message, Exception ex)
        {
            Console.WriteLine($"{message} - {ex.Message}");
        }
    }
}
