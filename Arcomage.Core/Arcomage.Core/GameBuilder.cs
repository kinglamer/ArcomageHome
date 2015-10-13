using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.Common;
using Arcomage.Core.Interfaces;
using Arcomage.Core.SpecialCard;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;
using Newtonsoft.Json;

namespace Arcomage.Core
{
    public class GameBuilder
    {
        private int _maxCard;
        private readonly List<Player> _players;
        private readonly Dictionary<int, Card> _specialCardHandlers;
        readonly List<Card> _serverCards = new List<Card>();

        protected readonly ILog Log;
        private const string Url = "http://arcomage.somee.com/ArcoServer.svc?wsdl"; //"http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        public GameBuilder(ILog log, IArcoServer server = null)
        {
            Log = log;
            IArcoServer host;
            _maxCard = 6;
            Log = log;
            _players = new List<Player>();

            if (server == null)
                host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(Url));
            else
                host = server;

            _serverCards = JsonConvert.DeserializeObject<List<Card>>(host.GetRandomCard());

            _specialCardHandlers = new Dictionary<int, Card>();
            _specialCardHandlers.Add(5, new Card5());
            _specialCardHandlers.Add(8, new Card8());
            _specialCardHandlers.Add(12, new Card12());
            _specialCardHandlers.Add(31, new Card31());
            _specialCardHandlers.Add(32, new Card32());
            _specialCardHandlers.Add(34, new Card34());
            _specialCardHandlers.Add(39, new Card39());
            _specialCardHandlers.Add(48, new Card48());
            _specialCardHandlers.Add(64, new Card64());
            _specialCardHandlers.Add(67, new Card67());
            _specialCardHandlers.Add(73, new Card73());
            _specialCardHandlers.Add(87, new Card87());
            _specialCardHandlers.Add(89, new Card89());
            _specialCardHandlers.Add(90, new Card90());
            _specialCardHandlers.Add(91, new Card91());
            _specialCardHandlers.Add(98, new Card98());
        }

        public void ChangeMaxCard(int newMaxCard)
        {
            _maxCard = newMaxCard;


        }

        public void AddPlayer(TypePlayer tp, string name,ICardPicker cardPicker = null, Dictionary<Attributes, int> startParams = null,
            IEnumerable<int> cardTricksters = null)
        {
            if (_players.Count == 2)
            {
                Log.Error("Достигнуто максимальное количество игроков");
                return;
            }

            if (startParams == null)
            {
                startParams = new Dictionary<Attributes, int>
                {
                    {Attributes.Wall, 5},
                    {Attributes.Tower, 10},
                    {Attributes.Menagerie, 1},
                    {Attributes.Colliery, 1},
                    {Attributes.DiamondMines, 1},
                    {Attributes.Rocks, 5},
                    {Attributes.Diamonds, 5},
                    {Attributes.Animals, 5}
                };
            }


            Player newPlayer = tp == TypePlayer.Human
                ? new Player(name, tp, startParams, cardPicker)
                : new AiPlayer(name, tp, startParams);


            if (cardTricksters != null) //добавление кастомных стартовых карт для игрока
                foreach (var item in cardTricksters)
                {
                    if (newPlayer.Cards.Count == _maxCard)
                        break;

                    var newCard = _serverCards.FirstOrDefault(x => x.id == item);
                    if (newCard != null)
                    {
                        newCard.description = ParseDescription.Parse(newCard.description);
                        newPlayer.Cards.Add(newCard);
                    }
                }

            _players.Add(newPlayer);
        }

        public GameModel StartGame(int currentPlayer = -1)
        {
            if (_players.Count == 0)
            {
                Log.Error(string.Format("Добавьте игроков в игру. Сейчас их {0}", _players.Count));
                return null;
            }

            if (currentPlayer == -1)
            {
                Random rnd = new Random();
                currentPlayer = rnd.Next(0, 2);
            }

            Log.Info(string.Format("Game is started. CurrentPlayer: {0}", currentPlayer));
            return new GameModel(Log, _players,_serverCards, _specialCardHandlers, currentPlayer, currentPlayer == 1 ? 0 : 1, _maxCard);
        }

    }
}
