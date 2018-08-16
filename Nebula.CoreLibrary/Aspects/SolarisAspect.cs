using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Aspects
{
    public class NebulaAspect : Attribute
    {
        public virtual void Before(AspectExecutingContext context)
        {

        }

        public virtual void After(AspectExecutedContext context)
        {

        }
    }
}
