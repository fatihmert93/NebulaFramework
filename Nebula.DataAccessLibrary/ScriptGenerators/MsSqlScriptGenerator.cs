using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.DataAccessLibrary.ScriptGenerators
{
    public class MsSqlScriptGenerator : ScriptGeneratorBase
    {
        public MsSqlScriptGenerator(Type t) : base(t)
        {
        }

        protected override Dictionary<Type, string> DataMapper()
        {
            Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
            dataMapper.Add(typeof(int), "INT");
            dataMapper.Add(typeof(string), "NVARCHAR(MAX)");
            dataMapper.Add(typeof(bool), "BIT");
            dataMapper.Add(typeof(DateTime), "DATE");
            dataMapper.Add(typeof(float), "FLOAT");
            dataMapper.Add(typeof(decimal), "DECIMAL(18,0)");
            dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER");
            return dataMapper;
        }

        public override string CreateGetTableColumnsScript()
        {
            throw new NotImplementedException();
        }
    }
}
