using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Core.Interfaces
{
   
    interface IPlayersCreator
    {
        Player FactoryMethod(string playerName, IStartParams startParams);
    }
}
