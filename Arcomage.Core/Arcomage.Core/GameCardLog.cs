using Arcomage.Core.Interfaces;
using Arcomage.Entity;
using Arcomage.Entity.Cards;

namespace Arcomage.Core
{
    public class GameCardLog
    {
        public GameCardLog(Player player, GameAction gameAction, Card card, int move)
        {
            Player = player;
            GameAction = gameAction;
            Card = card;
            Move = move;
        }

        public Player Player { get; set; }

        //Событие произведенное над картой
        public GameAction GameAction { get; set; }

        public Card Card { get; set; }

        public int Move { get; set; }

    }


}
