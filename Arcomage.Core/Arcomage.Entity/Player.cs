using System.Collections.Generic;
using Arcomage.Entity.Cards;

namespace Arcomage.Entity
{
    public class Player
    {
        public string playerName { get; private set; }

        public TypePlayer type { get; private set; }

        public Dictionary<Attributes, int> PlayerParams { get; set; }

        public List<Card> Cards { get; set; }

        public List<GameAction> gameActions { get; set; }

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public Player(string playerName, TypePlayer type, Dictionary<Attributes, int> gameParams)
        {
            this.playerName = playerName;
            this.type = type;

            PlayerParams = gameParams;
            Cards = new List<Card>();
            gameActions = new List<GameAction>();
        }

        public void UpdateParams()
        {
            PlayerParams[Attributes.Diamonds] += PlayerParams[Attributes.DiamondMines];
            PlayerParams[Attributes.Rocks] += PlayerParams[Attributes.Colliery];
            PlayerParams[Attributes.Animals] += PlayerParams[Attributes.Menagerie];
        }

        public virtual Card ChooseCard()
        {
            if (Cards.Count > 0)
                return Cards[0];

            return new Card();
        }
    }
}
