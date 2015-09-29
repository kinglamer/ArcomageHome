using Arcomage.Entity;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Core.Interfaces.Impl
{
    class CreatorAi : IPlayersCreator
    {
        public Player FactoryMethod(string playerName, IStartParams startParams)
        {
            Player player = new Player(playerName, TypePlayer.AI, startParams);

            return player;
        }
    }
}
