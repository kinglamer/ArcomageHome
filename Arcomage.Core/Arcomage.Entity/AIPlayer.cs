using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Entity
{
    public class AiPlayer : Player
    {
        public AiPlayer(string playerName, TypePlayer type, IStartParams gameParams) : base(playerName, type, gameParams)
        {
        }

        public override Card ChooseCard()
        {
            Card card = Cards.FirstOrDefault(item => PlayerParams[item.price.attributes] >= item.price.value);

            if (card == null)
            {
                card = Cards[0];
                gameActions.Add(GameAction.DropCard);
            }

            return card;
        }
    }
}
