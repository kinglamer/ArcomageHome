using System.Collections.Generic;
using Arcomage.Core.Interfaces;
using Arcomage.Entity;

namespace Arcomage.Core.Impl
{
    internal class AI : IPlayer
    {
        public AI(string name, TypePlayer _type)
        {
            playerName = name;
            type = _type;
            Cards = new List<Card>();

        }

        public string playerName { get; set; }
        public TypePlayer type { get; set; }
        public Dictionary<Specifications, int> Statistic { get; set; }
        public List<Card> Cards { get; set; }
        
    }
}
