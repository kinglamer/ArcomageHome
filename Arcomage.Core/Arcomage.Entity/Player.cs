using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Entity
{
  

    public class Player
    {

        public string playerName { get; private set; }

        public TypePlayer type { get; private set; }

        public Dictionary<Attributes, int> PlayerParams { get; set; }

        public List<Card> Cards { get; set; }

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public Player(string playerName, TypePlayer type, IStartParams gameParams)
        {
            this.playerName = playerName;
            this.type = type;

            PlayerParams = new Dictionary<Attributes, int>(gameParams.DefaultParams);
            Cards = new List<Card>();
        }

        public void UpdateParams()
        {
            PlayerParams[Attributes.Diamonds] += PlayerParams[Attributes.DiamondMines];
            PlayerParams[Attributes.Rocks] += PlayerParams[Attributes.Colliery];
            PlayerParams[Attributes.Animals] += PlayerParams[Attributes.Menagerie];
        }

    }
}
