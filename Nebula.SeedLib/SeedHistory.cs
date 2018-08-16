using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.SeedLib
{
    public class SeedHistory : EntityBase
    {
        public string SeedName { get; set; }

    }

    public class SeedHistoryItem : EntityBase
    {
        public Guid SeedHistoryId { get; set; }
        public string SerializedObject { get; set; }
        public string ClassName { get; set; }
        public string ObjectId { get; set; }
    }
}
