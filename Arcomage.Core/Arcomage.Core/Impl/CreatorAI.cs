using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Core.Interfaces;

namespace Arcomage.Core.Impl
{
    class CreatorAi : IPlayersCreator
    {
        public Player FactoryMethod(string playerName)
        {
            Player player = new Player(playerName, TypePlayer.AI, new GameStartParams());

            return player;
        }
    }
}
