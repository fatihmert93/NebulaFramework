using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface ITableCreator
    {
        bool IsTableExists(string tableName);
        void CreateTable(Type type);
        void CreateAllTable<TImlement>();
    }
}
