using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;
using Nebula.DataAccessLibrary.ScriptGenerators;

namespace Nebula.DataAccessLibrary.TableCreaters
{
    
    public class NpgTableCreator : ITableCreator
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IDbConnection _connection;
        public NpgTableCreator(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = connectionFactory.Connection;
        }

        public bool IsTableExists(string tableName)
        {
            bool exists;
            try
            {
                var query = $@"
                        SELECT EXISTS (
                           SELECT 1
                           FROM   information_schema.tables 
                           WHERE  table_schema = 'public'
                           AND    table_name = '{tableName}'
                           );";

                int result = _connection.Query<int>(query).FirstOrDefault();

                if (result == 1)
                    exists = true;
                else
                {
                    exists = false;
                }
            }
            catch (Exception)
            {
                exists = false;
            }
            finally
            {
                _connection.Close();
            }

            return exists;
        }

        private bool IsTableExists(Type type)
        {
            string tableName = type.Name;
            return IsTableExists(tableName);
        }

        public void CreateTable(Type type)
        {
            var scriptGenerator = ScriptGeneratorFactory.CreateInstance(type, typeof(PostgreScriptGenerator));
            var script = scriptGenerator.CreateTableScript();
            _connection.Execute(script);
        }

        private void UpdateTable(Type type)
        {
            IEnumerable<TableSchemaInformation> columns = TableColumns(type);
            var types = PostgreScriptGenerator.StaticDataMapper().Keys.ToList();

            var props = type.GetProperties().Where(v => types.Contains(v.PropertyType)).ToList();
            FindDifferenceColums(type,columns,props);
        }

        private static bool IsNullableConverter(string isNullable)
        {
            return (isNullable == "YES") ;
        }

        public void FindDifferenceColums(Type type,IEnumerable<TableSchemaInformation> dbColumns, IEnumerable<PropertyInfo> entityProps)
        {
            StringBuilder addBuilder = new StringBuilder();
            StringBuilder removeBuilder = new StringBuilder();
            var dbColumsList = dbColumns as TableSchemaInformation[] ?? dbColumns.ToArray();
            
            var dbColumnNames = dbColumsList.Select(v => v.column_name).ToList();
            var dbColumnTypes = dbColumsList.Select(v => v.data_type).ToList();
            var propertyInfos = entityProps as PropertyInfo[] ?? entityProps.ToArray();
            var removeProps = dbColumnNames.Except(propertyInfos.Select(v => v.Name)).ToList();
            var addProps = propertyInfos.Select(v => v.Name).Except(dbColumnNames).ToList();
            foreach (var dbColumn in dbColumsList)
            {
                var entityProp = propertyInfos.FirstOrDefault(v => v.Name == dbColumn.column_name);
                if (entityProp == null) continue;
                DbColumnType propType = PostgreScriptGenerator.DataTypeMapper()[entityProp.PropertyType];
                if (propType.Type == dbColumn.data_type && propType.IsNullable == IsNullableConverter(dbColumn.is_nullable)) continue;
                removeProps.Add(dbColumn.column_name);
                addProps.Add(entityProp.Name);
            }
            foreach (var propString in addProps)
            {
                var prop = entityProps.FirstOrDefault(v => v.Name == propString);
                string script = "ALTER TABLE \"" + type.Name + "\" ADD COLUMN IF NOT EXISTS \"" + propString + "\" " + PostgreScriptGenerator.StaticDataMapper()[prop.PropertyType];

                if (!DataHelper.IsNullableType(prop.PropertyType))
                {
                    script += GenerateDefaultValue(type,prop);
                }

                script += ";";
                addBuilder.AppendLine(script);
            }
            foreach (var propString in removeProps)
            {
                string script = "ALTER TABLE \"" + type.Name + "\" DROP COLUMN \"" + propString + "\";";
                removeBuilder.AppendLine(script);
            }

            _connection.Execute(removeBuilder.ToString() + addBuilder);

        }

        private string GenerateDefaultValue(Type type,PropertyInfo prop)
        {
            string script = "";
            if (prop.PropertyType == typeof(bool))
            {
                bool value = DataHelper.GetDefaultValue<bool>(type, prop.Name);
                script += " DEFAULT " + value + " ";
            }
            else if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(DateTime))
            {
                string value = DataHelper.GetDefaultValue<string>(type, prop.Name);
                script += " DEFAULT \'" + value + "\' ";
            }
            else if (prop.PropertyType == typeof(int))
            {
                int value = DataHelper.GetDefaultValue<int>(type, prop.Name);
                script += " DEFAULT " + value + " ";
            }
            else if (prop.PropertyType == typeof(float))
            {
                float value = DataHelper.GetDefaultValue<float>(type, prop.Name);
                script += " DEFAULT " + value + " ";
            }

            return script;
        }

        public void CreateAllTable<TImlement>()
        {
            IEnumerable<Type> types = ReflectionUtility.FindSubClassesOf<TImlement>();
            foreach (var type in types)
            {
                if (!IsTableExists(type))
                {
                    CreateTable(type);
                }
                else
                {
                    UpdateTable(type);
                }
            }
        }

        public IEnumerable<TableSchemaInformation> TableColumns(Type type)
        {
            ScriptGeneratorBase scriptGenerator = ScriptGeneratorFactory.CreateInstance(type, typeof(PostgreScriptGenerator));
            string script = scriptGenerator.CreateGetTableColumnsScript();
            IEnumerable<TableSchemaInformation> columns = _connection.Query<TableSchemaInformation>(script);
            return columns;
        }
    }


    public class TableSchemaInformation
    {
        public string column_name { get; set; }
        public string data_type { get; set; }
        public string is_nullable { get; set; }
    }
}
