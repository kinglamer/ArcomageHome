using System;
using System.Collections.Generic;
using Arcomage.Entity.Cards;

namespace Arcomage.Entity.Players
{
    public class Player: ICardObserver
    {
        public string PlayerName { get; private set; }

        public TypePlayer Type { get; private set; }

        public Dictionary<Attributes, int> PlayerParams { get; set; }

        public List<Card> Cards { get; set; }

        private Card ChoosenCard { get; set; }

        public List<GameAction> gameActions { get; set; }

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public Player(string playerName, TypePlayer type, Dictionary<Attributes, int> gameParams, ICardPicker cardPicker)
        {
            PlayerName = playerName;
            Type = type;

            PlayerParams = gameParams;
            Cards = new List<Card>();
            gameActions = new List<GameAction>();

            if (cardPicker != null)
            {
                cardPicker.AddObserver(this);
            }
            ChoosenCard = null;
        }

        public void UpdateParams()
        {
            PlayerParams[Attributes.Diamonds] += PlayerParams[Attributes.DiamondMines];
            PlayerParams[Attributes.Rocks] += PlayerParams[Attributes.Colliery];
            PlayerParams[Attributes.Animals] += PlayerParams[Attributes.Menagerie];
        }

        public virtual Card ChooseCard()
        {
            var rVal = ChoosenCard;
            ChoosenCard = null;
            return rVal;
        }

        public void Update(Card card, GameAction gameAction)
        {
            if (!gameActions.Contains(gameAction))
            gameActions.Add(gameAction);
            ChoosenCard = card;
        }
    }
}
