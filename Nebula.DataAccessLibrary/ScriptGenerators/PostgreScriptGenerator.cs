using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.DataAccessLibrary.ScriptGenerators
{
    public class DbColumnType
    {
        public DbColumnType(string type, bool isNullable)
        {
            Type = type;
            IsNullable = isNullable;
        }


        public string Type { get; set; }
        public bool IsNullable { get; set; }
    }

    public class PostgreScriptGenerator : ScriptGeneratorBase
    {
        public PostgreScriptGenerator(Type t) : base(t)
        {
        }
        
        public static Dictionary<Type, string> StaticDataMapper()
        {
            Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
            dataMapper.Add(typeof(int), "integer not null");
            dataMapper.Add(typeof(int?),"integer null");
            dataMapper.Add(typeof(string), "text");
            dataMapper.Add(typeof(bool), "boolean not null");
            dataMapper.Add(typeof(bool?),"boolean null");
            dataMapper.Add(typeof(DateTime), "timestamp not null");
            dataMapper.Add(typeof(DateTime?),"timestamp null");
            dataMapper.Add(typeof(float), "real not null");
            dataMapper.Add(typeof(float?),"real null");
            dataMapper.Add(typeof(decimal), "numeric not null");
            dataMapper.Add(typeof(decimal?),"numeric null");
            dataMapper.Add(typeof(long),"bigint not null");
            dataMapper.Add(typeof(long?),"bigint null");
            dataMapper.Add(typeof(byte[]),"bytea");
            dataMapper.Add(typeof(TimeSpan),"interval not null");
            dataMapper.Add(typeof(TimeSpan?),"interval null");
            dataMapper.Add(typeof(Guid), "uuid not null");
            dataMapper.Add(typeof(Guid?),"uuid null");
            return dataMapper;
        }

        public static Dictionary<Type, DbColumnType> DataTypeMapper()
        {
            Dictionary<Type, DbColumnType> dataMapper = new Dictionary<Type, DbColumnType>();
            dataMapper.Add(typeof(int), new DbColumnType("integer", false));
            dataMapper.Add(typeof(int?), new DbColumnType("integer", true));
            dataMapper.Add(typeof(string), new DbColumnType("text", true));
            dataMapper.Add(typeof(bool), new DbColumnType("boolean", false));
            dataMapper.Add(typeof(bool?), new DbColumnType("boolean", true));
            dataMapper.Add(typeof(DateTime), new DbColumnType("timestamp", false));
            dataMapper.Add(typeof(DateTime?), new DbColumnType("timestamp", true));
            dataMapper.Add(typeof(float), new DbColumnType("real", false));
            dataMapper.Add(typeof(float?), new DbColumnType("real", true));
            dataMapper.Add(typeof(decimal), new DbColumnType("numeric", false));
            dataMapper.Add(typeof(decimal?), new DbColumnType("numeric", true));
            dataMapper.Add(typeof(long), new DbColumnType("bigint", false));
            dataMapper.Add(typeof(long?), new DbColumnType("bigint", true));
            dataMapper.Add(typeof(byte[]), new DbColumnType("bytea", false));
            dataMapper.Add(typeof(TimeSpan), new DbColumnType("interval", false));
            dataMapper.Add(typeof(TimeSpan?), new DbColumnType("interval", true));
            dataMapper.Add(typeof(Guid), new DbColumnType("uuid", false));
            dataMapper.Add(typeof(Guid?), new DbColumnType("uuid", true));
            return dataMapper;
        }



        protected override Dictionary<Type, string> DataMapper()
        {
            Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
            dataMapper.Add(typeof(int), "integer not null");
            dataMapper.Add(typeof(int?),"integer null");
            dataMapper.Add(typeof(string), "text");
            dataMapper.Add(typeof(bool), "boolean not null");
            dataMapper.Add(typeof(bool?),"boolean null");
            dataMapper.Add(typeof(DateTime), "timestamp not null");
            dataMapper.Add(typeof(DateTime?),"timestamp null");
            dataMapper.Add(typeof(float), "real not null");
            dataMapper.Add(typeof(float?),"real null");
            dataMapper.Add(typeof(decimal), "numeric not null");
            dataMapper.Add(typeof(decimal?),"numeric null");
            dataMapper.Add(typeof(long),"bigint not null");
            dataMapper.Add(typeof(long?),"bigint null");
            dataMapper.Add(typeof(byte[]),"bytea");
            dataMapper.Add(typeof(TimeSpan),"interval not null");
            dataMapper.Add(typeof(TimeSpan?),"interval null");
            dataMapper.Add(typeof(Guid), "uuid not null");
            dataMapper.Add(typeof(Guid?),"uuid null");
            return dataMapper;
        }

        public override string CreateTableScript()
        {
            System.Text.StringBuilder script = new StringBuilder();

            ClassName = "\"" + ClassName + "\"";
            script.Append(@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");
            script.AppendLine("CREATE TABLE " + this.ClassName);
            script.AppendLine("(");
            for (int i = 0; i < this.Fields.Count; i++)
            {
                KeyValuePair<String, Type> field = this.Fields[i];
                

                if (DataMapper().ContainsKey(field.Value))
                {
                    string fieldKey = "\"" + field.Key + "\"";
                    if (IdentityColumnCheck(field.Key))
                    {
                        identityColumn = fieldKey;
                        script.Append("\t " + fieldKey + " " + DataMapper()[field.Value] + " PRIMARY KEY NOT NULL UNIQUE DEFAULT uuid_generate_v4()");
                    }
                    else
                    {
                        script.Append("\t " + fieldKey + " " + DataMapper()[field.Value]);
                    }
                }


                if (i != this.Fields.Count - 1)
                {
                    script.Append(",");
                }

                script.Append(Environment.NewLine);
            }

            script.AppendLine(")");

            string strScript = script.ToString();
            return script.ToString();
        }

        public override string CreateGetTableColumnsScript()
        {
            string script = @"SELECT column_name,data_type,is_nullable
            FROM information_schema.columns
            WHERE table_schema = 'public'
            AND table_name = '" + this.ClassName + "'; ";

            return script;
        }

    }
}
