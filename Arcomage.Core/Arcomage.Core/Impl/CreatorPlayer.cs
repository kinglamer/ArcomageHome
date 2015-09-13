using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Core.Interfaces;

namespace Arcomage.Core.Impl
{
    class CreatorPlayer : IPlayersCreator
    {

        public Player FactoryMethod(string playerName)
        {
            Player player = new Player(playerName, TypePlayer.Human, new GameStartParams());

            return player;
        }
    }
}
