using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true)]
    public class NotTableMapped : Attribute
    {
    }
}
