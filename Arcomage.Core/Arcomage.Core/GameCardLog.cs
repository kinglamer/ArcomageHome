using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core
{
    public enum GameEvent
    {
        None,
        Used,
        Droped
    }

    public class GameCardLog
    {
        //Игрок, который работал над картой
        public GameCardLog(IPlayer player, GameEvent gameEvent, Card card, int move)
        {
            this.player = player;
            this.gameEvent = gameEvent;
            this.card = card;
            this.move = move;
        }

        public IPlayer player { get; set; }

        //Событие произведенное над картой
        public GameEvent gameEvent { get; set; }

        //Сама карта
        public Card card { get; set; }

        public int move { get; set; }



    }


}
