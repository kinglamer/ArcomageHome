using System.Diagnostics;
using Arcomage.Core.Interfaces;

namespace Arcomage.Tests.Moq
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
