using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core
{
    class AI : IPlayer
    {
        public AI(string name)
        {
            throw new NotImplementedException();
        }

        public int CountCard { get; private set; }
        public string playerName { get; set; }
        public Dictionary<Specifications, int> playerStatistic { get; set; }
        public List<Card> playCards { get; set; }
        public Card ReturnCard(int id)
        {
            throw new NotImplementedException();
        }
    }
}
