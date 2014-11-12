using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Common;
using Arcomage.Entity;

namespace Arcomage.Core
{
    public interface IPlayer
    {
      
     

        string playerName { get; set; }

        TypePlayer type { get; set; }

        Dictionary<Specifications, int> Statistic { get; set; }

        List<Card> Cards { get; set; }

       
    }
}
