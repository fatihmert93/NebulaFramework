using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.LogLibrary
{
    public class SqlLogger : ILogManager
    {
        private readonly ILogRepository _logRepository;
        public SqlLogger(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void Write(string message)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.LogType = "info";
            logEntity.Message = message;
            logEntity.JsonMessage = string.Empty;
            _logRepository.Create(logEntity);
        }

        public void Write(string message, string logType)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.LogType = logType;
            logEntity.Message = message;
            logEntity.JsonMessage = string.Empty;
            _logRepository.Create(logEntity);
        }

        public void WriteError(string message, Exception ex)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.LogType = "error";
            logEntity.Message = ex.Message;
            logEntity.JsonMessage = ConverterUtility.WriteFromObject(ex);
            _logRepository.Create(logEntity);
        }
    }
}
