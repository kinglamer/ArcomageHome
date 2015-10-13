using System.Collections.Generic;
using System.Linq;
using Arcomage.Entity.Cards;

namespace Arcomage.Entity.Players
{
    public class AiPlayer : Player
    {
        public AiPlayer(string playerName, TypePlayer type, Dictionary<Attributes, int> gameParams)
            : base(playerName, type, gameParams, null)
        {
        }

        public override Card ChooseCard()
        {
            Card card = Cards.FirstOrDefault(item => PlayerParams[item.price.attributes] >= item.price.value);

            if (card == null)
            {
                if (Cards.Count > 0)
                    card = Cards[0];
              
                if (gameActions.Contains(GameAction.PlayCard))
                    gameActions.Remove(GameAction.PlayCard);

                gameActions.Add(GameAction.DropCard);
            }

            return card;
        }
    }
}
