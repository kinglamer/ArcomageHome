using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Common;
using Arcomage.Entity;

namespace Arcomage.Core
{
    interface IPlayer
    {
      
     
        int CountCard { get;   }

        string playerName { get; set; }

        Dictionary<Specifications, int> playerStatistic { get; set; }

        List<Card> playCards { get; set; }

        Card ReturnCard(int id);
    }
}
