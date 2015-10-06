using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.Common;
using Arcomage.Core.Interfaces;
using Arcomage.Core.SpecialCard;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Newtonsoft.Json;


namespace Arcomage.Core
{
    public class GameController
    {
        public Player CurrentPlayer { get; set; }
        public Player EnemyPlayer { get; set; }
        public bool IsGameEnded { get; set; }
        public string Winner { get; private set; }

        private int _currentMove;
        private int _maxCard;

        protected readonly ILog Log;
        private readonly Dictionary<Attributes, int> _winParams;
        private readonly Dictionary<Attributes, int> _loseParams;
        private const string Url = "http://arcomage.somee.com/ArcoServer.svc?wsdl"; //"http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        readonly List<Card> _serverCards = new List<Card>();
        private readonly Dictionary<int, Card> _specialCardHandlers;

        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private readonly Queue<Card> _qCard = new Queue<Card>();

        public List<GameCardLog> LogCard = new List<GameCardLog>();

        private readonly List<Player> _players;



        public GameController(ILog log, IArcoServer server = null)
        {
            IArcoServer host;
            IsGameEnded = true;
            _maxCard = 6;
            Log = log;
            _loseParams = GameControllerHelper.GetLoseParams();
            _winParams = GameControllerHelper.GetWinParams();
            _players = new List<Player>();
           
            if (server == null)
                host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(Url));
            else
                host = server;

            _serverCards = JsonConvert.DeserializeObject<List<Card>>(host.GetRandomCard());
           
            _specialCardHandlers = new Dictionary<int, Card>();
            _specialCardHandlers.Add(5,new Card5());
            _specialCardHandlers.Add(8,new Card8());
            _specialCardHandlers.Add(12, new Card12());
            _specialCardHandlers.Add(31, new Card31());
            _specialCardHandlers.Add(32, new Card32());
            _specialCardHandlers.Add(34,new Card34());
            _specialCardHandlers.Add(39,new Card39());
            _specialCardHandlers.Add(48,new Card48());
            _specialCardHandlers.Add(64,new Card64());
            _specialCardHandlers.Add(67,new Card67());
            _specialCardHandlers.Add(73,new Card73());
            _specialCardHandlers.Add(87,new Card87());
            _specialCardHandlers.Add(89,new Card89());
            _specialCardHandlers.Add(90,new Card90());
            _specialCardHandlers.Add(91,new Card91());
            _specialCardHandlers.Add(98,new Card98());
        }

        public void ChangeMaxCard(int newMaxCard)
        {
            _maxCard = newMaxCard;
        }

        public void AddPlayer(TypePlayer tp, string name, Dictionary<Attributes, int> startParams = null,
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
                ? new Player(name, tp, startParams)
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

        public void StartGame(int currentPlayer = -1)
        {
            if (_players.Count == 0)
            {
                Log.Error(string.Format("Добавьте игроков в игру. Сейчас их {0}", _players.Count));
                return;
            }

            if (!IsGameEnded)
            {
                Log.Info("Игра еще не закончилась");
                return;
            }

            int enemy;
            if (currentPlayer != -1)
            {
                CurrentPlayer = _players[currentPlayer];
                enemy = currentPlayer == 1 ? 0 : 1;
            }
            else
            {
                Random rnd = new Random();
                int indexPlayer = rnd.Next(0, 2);
                enemy = indexPlayer == 1 ? 0 : 1;
                CurrentPlayer = _players[indexPlayer];
            }

            EnemyPlayer = _players[enemy];
            IsGameEnded = false;

            Log.Info(string.Format("Game is started. CurrentPlayer: {0}", CurrentPlayer));

            _currentMove = 0;

            SetPlayerCards(CurrentPlayer);
            SetPlayerCards(EnemyPlayer);
        }


        /// <summary>
        /// Устанавливаем карты для игрока
        /// </summary>
        /// <returns></returns>
        private void SetPlayerCards(Player player)
        {
            if (_qCard.Count < _maxCard)
            {
                _serverCards.Randomize(); //TODO: стоил ли перемешивать список карт, каждый раз перед добавлением в стэк?
                foreach (var item in _serverCards)
                {
                    _qCard.Enqueue(item);
                }
            }

            while (player.Cards.Count < _maxCard)
            {
                if (_qCard.Count == 0)
                    break;

                var newCard = _qCard.Dequeue();
                newCard.description = ParseDescription.Parse(newCard.description);
                player.Cards.Add(newCard);
            }
        }

        public List<Card> GetAiUsedCard()
        {
            List<Card> returnVal = new List<Card>();
            var result = LogCard.Where(x=>x.Player.type == TypePlayer.AI && x.Move == _currentMove);

            foreach (var item in result)
            {
                returnVal.Add(item.Card);
            }

            return returnVal;
        }

        
        /// <summary>
        /// Проверка хватает ли ресурсов для использования карты
        /// </summary>
        public bool CanUseCard(Price price)
        {
            if (CurrentPlayer.PlayerParams[price.attributes] >= price.value)
                return true;

            return false;
        }

        public bool CanUseCard(int id)
        {
            int index;
            return CanUseCard(GetCardById(id, out index).price);
        }




        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <param name="dropCard">Флаг, что карта сбрасывается</param>
        public void MakePlayerMove(int id, bool dropCard = false)
        {
            if (CurrentPlayer.gameActions.Contains(GameAction.EndMove))
                return;

            int index;
            var card = GetCardById(id, out index);

            if (CanUseCard(card.price) && !dropCard)
            {

                if (CurrentPlayer.gameActions.Contains(GameAction.MakeMoveAgain))
                    CurrentPlayer.gameActions.Remove(GameAction.MakeMoveAgain);

                if (!_specialCardHandlers.ContainsKey(id))
                    card.Apply(CurrentPlayer, EnemyPlayer);
                else
                {
                    _specialCardHandlers[id].copyParams(card);
                    _specialCardHandlers[id].Apply(CurrentPlayer, EnemyPlayer);

                    if (_specialCardHandlers[id].discard)
                        CurrentPlayer.gameActions.Add(GameAction.DropCard);
                }

                if (card.playAgain)
                    CurrentPlayer.gameActions.Add(GameAction.MakeMoveAgain);



                LogCard.Add(new GameCardLog(CurrentPlayer, GameAction.MakeMove, CurrentPlayer.Cards[index], _currentMove));
                Debug.Print(CurrentPlayer.playerName + " use " + CurrentPlayer.Cards[index].id);
                CurrentPlayer.Cards.RemoveAt(index);
            }
            else
            {
                if (id == 40 && dropCard)
                {
                    Log.Info("Эту карту нельзя сбросить!");
                    return;
                }

                try
                {
                    CurrentPlayer.gameActions.Remove(GameAction.DropCard);
                    LogCard.Add(new GameCardLog(CurrentPlayer, GameAction.DropCard, CurrentPlayer.Cards[index], _currentMove));
                    Debug.Print(CurrentPlayer.playerName + " drop " + CurrentPlayer.Cards[index].id);
                    CurrentPlayer.Cards.RemoveAt(index);
                }
                catch
                {
                    Log.Error(string.Format("Player: {0} can't pass card {1}", CurrentPlayer.playerName, CurrentPlayer.Cards[index].name));
                }
            }

            if (CurrentPlayer.gameActions.Count == 0)
                CurrentPlayer.gameActions.Add(GameAction.EndMove);
        }


        public void NextPlayerTurn()
        {
            if (!CurrentPlayer.gameActions.Contains(GameAction.EndMove) || IsGameEnded)
                return;

            SetPlayerCards(CurrentPlayer);

            CurrentPlayer.UpdateParams();
            CurrentPlayer.gameActions.Clear();

            Winner = GameControllerHelper.CheckPlayerParams(_players, _winParams, _loseParams);
            if (Winner.Length > 0)
            {
                Log.Info("Победил: " + Winner);
                IsGameEnded = true;
                return;
            }

            Player temp = CurrentPlayer;
            CurrentPlayer = EnemyPlayer;
            EnemyPlayer = temp;
        }

        private Card GetCardById(int id, out int index)
        {
            index = CurrentPlayer.Cards.FindIndex(x => x.id == id);
            return CurrentPlayer.Cards[index];
        }
    
    }
}
