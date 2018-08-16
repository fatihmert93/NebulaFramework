using System;
using Nebula.CoreLibrary.Shared;

namespace Nebula.SystemSettings
{
    public class SettingsEntity : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

    }
}
