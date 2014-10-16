using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcomage.Common
{
    public interface ILog
    {
        void Info(string text);

        void Error(string text);
    }
}
