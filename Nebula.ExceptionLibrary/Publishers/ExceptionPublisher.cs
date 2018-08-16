using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary.Publishers
{
    internal abstract class ExceptionPublisher
    {
        internal abstract void Publish(Exception e);
    }
}
