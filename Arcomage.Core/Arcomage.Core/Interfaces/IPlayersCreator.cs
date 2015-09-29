using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Core.Impl;
using Arcomage.Entity;

namespace Arcomage.Core.Interfaces
{
   
    interface IPlayersCreator
    {
        Player FactoryMethod(string playerName);
    }
}
