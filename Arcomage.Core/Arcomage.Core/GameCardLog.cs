using Arcomage.Core.Interfaces;
using Arcomage.Entity;
using Arcomage.Entity.Cards;

namespace Arcomage.Core
{


    public class GameCardLog
    {
        //Игрок, который работал над картой
        public GameCardLog(Player player, GameEvent gameEvent, Card card, int move)
        {
            this.player = player;
            this.gameEvent = gameEvent;
            this.card = card;
            this.move = move;
        }

        public Player player { get; set; }

        //Событие произведенное над картой
        public GameEvent gameEvent { get; set; }

        //Сама карта
        public Card card { get; set; }

        public int move { get; set; }



    }


}
