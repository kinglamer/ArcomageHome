using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity.Cards;

namespace Arcomage.Entity.Players
{
    public class HumanPlayer : Player, ICardObserver
    {
        protected readonly ICardPicker _cardPicker;
        protected Card ChoosenCard { get; private set; }

        public HumanPlayer(string playerName, TypePlayer type, Dictionary<Attributes, int> gameParams, ICardPicker cardPicker)
            : base(playerName, type, gameParams)
        {
            _cardPicker = cardPicker;
            _cardPicker.AddObserver(this);
            ChoosenCard = null;
        }


        public void Update(Card card)
        {
            ChoosenCard = card;
        }

        public override Card ChooseCard()
        {
            var rVal = ChoosenCard ?? Cards[0];
            ChoosenCard = null;
            return rVal;
        }
    }
}
