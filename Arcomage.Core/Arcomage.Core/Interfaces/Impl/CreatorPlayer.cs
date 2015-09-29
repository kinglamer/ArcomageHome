using Arcomage.Entity;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Core.Interfaces.Impl
{
    class CreatorPlayer : IPlayersCreator
    {

        public Player FactoryMethod(string playerName, IStartParams startParams)
        {
            Player player = new Player(playerName, TypePlayer.Human, startParams);

            return player;
        }
    }
}
