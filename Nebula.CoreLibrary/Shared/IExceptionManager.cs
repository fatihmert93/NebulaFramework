using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface IExceptionManager
    {
        void Handle(Exception e);

        void Wrap();
    }
}
