using System.Collections.Generic;
using Arcomage.Entity;

namespace Arcomage.Core.Interfaces
{
    public interface IPlayer
    {
      
     

        string playerName { get; set; }

        TypePlayer type { get; set; }

        Dictionary<Specifications, int> Statistic { get; set; }

        List<Card> Cards { get; set; }

       
    }
}
