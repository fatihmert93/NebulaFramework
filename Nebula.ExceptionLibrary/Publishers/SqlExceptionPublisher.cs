using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;
using System;

namespace Nebula.ExceptionLibrary.Publishers
{
    internal class SqlExceptionPublisher : ExceptionPublisher
    {
        private readonly ILogRepository _logRepository;

        public SqlExceptionPublisher()
        {
            _logRepository = DependencyService.GetService<ILogRepository>();
        }

        internal override void Publish(Exception e)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.LogType = "error";
            logEntity.Message = e.Message;
            logEntity.JsonMessage = ConverterUtility.WriteFromObject(e);
            _logRepository.Create(logEntity);
        }
    }
}
