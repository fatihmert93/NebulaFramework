using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary.ExpressionBuilders
{
    public class SqlScriptBuilder<T> where T : EntityBase, new()
    {
        private Type _type;
        private readonly string _typeName;
        private readonly List<WherePart> _whereParts;
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        private readonly WhereBuilder _whereBuilder;


        public SqlScriptBuilder()
        {
            _type = typeof(T);
            _typeName = _type.Name;
            _whereParts = new List<WherePart>();
            _whereBuilder = new WhereBuilder();
        }

        public SqlScriptBuilder<T> Where(Expression<Func<T, bool>> predict)
        {
            WherePart wherePart = _whereBuilder.ToSql(predict);
            _whereParts.Add(wherePart);
            return this;
        }

        public string ToSql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM " + _typeName);
            if (_whereParts.Count != 0)
            {
                builder.Append(" WHERE ");
                string[] whereScripts = _whereParts.Select(v => v.Sql).ToArray();
                
                string where = string.Join(" || ", whereScripts);
                builder.Append(where);
            }

            return builder.ToString();
        }
    }
}
