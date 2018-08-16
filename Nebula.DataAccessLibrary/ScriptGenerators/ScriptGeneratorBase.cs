using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nebula.DataAccessLibrary.ScriptGenerators
{
    public abstract class ScriptGeneratorBase
    {
        protected List<KeyValuePair<String, Type>> _fieldInfo = new List<KeyValuePair<String, Type>>();
        protected string _className = String.Empty;

        protected abstract Dictionary<Type, String> DataMapper();

        public abstract string CreateGetTableColumnsScript();

        public List<KeyValuePair<String, Type>> Fields
        {
            get { return this._fieldInfo; }
            set { this._fieldInfo = value; }
        }

        public virtual string ClassName
        {
            get { return this._className; }
            set { this._className = value; }
        }

        protected string identityColumn;

        protected ScriptGeneratorBase(Type t)
        {
            this._className = t.Name;

            List<PropertyInfo> properties = new List<PropertyInfo>();

            var types = DataMapper().Keys.ToList();

            var tempProperties = t.GetProperties().Where(v => types.Contains(v.PropertyType)).ToList();
            foreach (var property in tempProperties)
            {
                object[] attributes = property.GetCustomAttributes(typeof(NotMappedAttribute), true);
                if (attributes.Length == 0)
                {
                    properties.Add(property);
                }
            }

            foreach (PropertyInfo p in properties)
            {
                KeyValuePair<String, Type> field = new KeyValuePair<String, Type>(p.Name, p.PropertyType);

                this.Fields.Add(field);
            }
        }

        public virtual string CreateTableScript()
        {
            System.Text.StringBuilder script = new StringBuilder();

            script.AppendLine("CREATE TABLE " + this.ClassName);
            script.AppendLine("(");
            for (int i = 0; i < this.Fields.Count; i++)
            {
                KeyValuePair<String, Type> field = this.Fields[i];

                if (DataMapper().ContainsKey(field.Value))
                {
                    script.Append("\t " + field.Key + " " + DataMapper()[field.Value]);
                    if (IdentityColumnCheck(field.Key))
                    {
                        identityColumn = field.Key;
                    }
                }
                else
                {
                    // Complex Type? 
                    script.Append("\t " + field.Key + " INT");
                }


                if (i != this.Fields.Count - 1)
                {
                    script.Append(",");
                }
                else
                {
                    script.Append(",");
                    if (!string.IsNullOrEmpty(identityColumn))
                    {
                        script.Append("\t primary key(" + identityColumn + ")");
                    }
                }

                script.Append(Environment.NewLine);
            }

            script.AppendLine(")");

            string strScript = script.ToString();
            return script.ToString();
        }

        protected bool IdentityColumnCheck(string key)
        {
            string lower = key.ToLower();
            return (lower.Equals("id") || lower.Equals("ıd")) ;
        }
    }
}