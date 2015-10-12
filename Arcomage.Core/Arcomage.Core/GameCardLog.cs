using Arcomage.Core.Interfaces;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core
{
    public class GameCardLog
    {
        public GameCardLog(Player player, GameAction gameAction, Card card, int moveIndex)
        {
            Player = player;
            GameAction = gameAction;
            Card = card;
            MoveIndex = moveIndex;
        }

        public Player Player { get; set; }

        //Событие произведенное над картой
        public GameAction GameAction { get; set; }

        public Card Card { get; set; }

        public int MoveIndex { get; set; }

    }


}
