using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.Interfaces;
using Arcomage.Entity;

namespace Arcomage.Core
{
    public class NewPlayer
    {
        public NewPlayer(TypePlayer type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public TypePlayer type { get; private set; }
        public string name { get; private set; }

        public Dictionary<Attributes, int> StartInts { get; set; }
        public List<int> cardTricksters { get; set; }

    }

    public class GameController
    {
        
        private GameModel gameModel = null;

        public void CreateGame(ILog log, IArcoServer server,NewPlayer player1, NewPlayer player2)
        {
            GameBuilder gameBuilder = new GameBuilder(log, server);

            gameBuilder.AddPlayer(player1.type, player1.name, null, player1.StartInts, player1.cardTricksters);
            gameBuilder.AddPlayer(player2.type, player2.name, null, player2.StartInts, player2.cardTricksters);

            gameModel = gameBuilder.StartGame();
        }
    }
}
