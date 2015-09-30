using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core.Interfaces;

namespace Arcomage.Tests
{
    class LogTest : ILog
    {
        public void Info(string text)
        {
            Debug.Print(text);
        }

        public void Error(string text)
        {
            Debug.Print("ERROR:" + text);
        }
    }
}
