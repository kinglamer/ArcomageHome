using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.Common;
using Arcomage.Core.Interfaces;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Core.SpecialCard;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Interfaces;
using Newtonsoft.Json;


namespace Arcomage.Core
{

   
    public delegate void EventMethod(Dictionary<string, object> info);

    public class GameController
    {

        #region Variables 

        private int MaxCard;
        public List<Player> players { get; private set; }

        public Player CurrentPlayer { get; set; }
        public Player EnemyPlayer { get; set; }

      //  private int currentPlayer { get; set; }

       /* private CurrentAction _additionaStatus;

        public CurrentAction additionaStatus
        {
            get { return _additionaStatus; }
            set
            {
                if (log != null)
                    log.Info("Дополнительный Статус " + additionaStatus + " изменен на " + value);
                _additionaStatus = value;

            }
        }

        private CurrentAction _status;
        public CurrentAction Status {
            get {   return _status;}
            set
            {
                if (log != null)
                log.Info("Статус " + Status + " изменен на " + value);
                _status = value;
                
            }
        }*/

        private int currentMove { get; set; }

        public bool isGameEnded;

       /* private bool isGameEnded
        {
            get
            {
                if (Status == CurrentAction.None || Status == CurrentAction.EndGame)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }*/

        protected readonly ILog log;
        private readonly Dictionary<Attributes, int> WinParams;
        private readonly Dictionary<Attributes, int> LoseParams;
        private const string url = "http://arcomage.somee.com/ArcoServer.svc?wsdl"; //"http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        List<Card> serverCards = new List<Card>();

        public string Winner { get;private set; }
        private IArcoServer host;

        private Dictionary<CurrentAction, EventMethod> eventHandlers;
        private Dictionary<int, Card> specialCardHandlers;

        //очередная подтасовка, что игрок начинал ход с определенным набором карт
        private List<int> specialHand = new List<int>(); 
        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private Queue<Card> QCard = new Queue<Card>();

        public List<GameCardLog> logCard = new List<GameCardLog>();

        #endregion


        public GameController(ILog _log, IArcoServer server = null)
        {
            isGameEnded = true;
            MaxCard = 6;
           // Status = CurrentAction.None;
            log = _log;
            LoseParams = GameControllerHelper.GetLoseParams();
            WinParams = GameControllerHelper.GetWinParams();
            players = new List<Player>();
           
            if (server == null)
                host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url));
            else
                host = server;

            serverCards = JsonConvert.DeserializeObject<List<Card>>(host.GetRandomCard());

          /*  //Устанавливаем соответствие между методом и статусом
            eventHandlers = new Dictionary<CurrentAction, EventMethod>();
            eventHandlers.Add(CurrentAction.None,None);
            eventHandlers.Add(CurrentAction.StartGame, StartGame);
            eventHandlers.Add(CurrentAction.GetPlayerCard, GetPlayerCard);
            eventHandlers.Add(CurrentAction.WaitHumanMove, WaitHumanMove);
            eventHandlers.Add(CurrentAction.PassStroke, PassStroke);
            eventHandlers.Add(CurrentAction.HumanUseCard, HumanUseCard);
            eventHandlers.Add(CurrentAction.UpdateStatHuman, UpdateStat);
            eventHandlers.Add(CurrentAction.UpdateStatAI, UpdateStat);
            eventHandlers.Add(CurrentAction.EndHumanMove, EndHumanMove);
            eventHandlers.Add(CurrentAction.AIMoveIsAnimated, AIMoveIsAnimated);
            eventHandlers.Add(CurrentAction.AIUseCardAnimation, AIUseCardAnimation);
            eventHandlers.Add(CurrentAction.EndAIMove, EndAIMove);
            eventHandlers.Add(CurrentAction.EndGame, EndGame);
            eventHandlers.Add(CurrentAction.PlayerMustDropCard, PlayerMustDropCard);*/
            
            specialCardHandlers = new Dictionary<int, Card>();
            specialCardHandlers.Add(5,new Card5());
            specialCardHandlers.Add(8,new Card8());
            specialCardHandlers.Add(12, new Card12());
            specialCardHandlers.Add(31, new Card31());
            specialCardHandlers.Add(32, new Card32());
            specialCardHandlers.Add(34,new Card34());
            specialCardHandlers.Add(39,new Card39());
            specialCardHandlers.Add(48,new Card48());
            specialCardHandlers.Add(64,new Card64());
            specialCardHandlers.Add(67,new Card67());
            specialCardHandlers.Add(73,new Card73());
            specialCardHandlers.Add(87,new Card87());
            specialCardHandlers.Add(89,new Card89());
            specialCardHandlers.Add(90,new Card90());
            specialCardHandlers.Add(91,new Card91());
            specialCardHandlers.Add(98,new Card98());
          
        }

        public void ChangeMaxCard(int newMaxCard)
        {
            MaxCard = newMaxCard;
        }

        public void AddPlayer(TypePlayer tp, string name, IStartParams startParams = null, IEnumerable<int> cardTricksters = null)
        {
            if (players.Count == 2)
            {
                log.Error("Достигнуто максимальное количество игроков");
                return;
            }

            if (startParams == null)
                startParams = new GameStartParams();

            Player newPlayer = tp == TypePlayer.Human ? new Player(name, tp, startParams) : new AiPlayer(name, tp, startParams);
           
            if (cardTricksters != null)
                foreach (var item in cardTricksters)
                {
                    if (newPlayer.Cards.Count == MaxCard)
                        break;

                    var newCard = serverCards.FirstOrDefault(x => x.id == item);
                    if (newCard != null)
                    {
                        newCard.description = ParseDescription.Parse(newCard.description);
                        newPlayer.Cards.Add(newCard);
                    }
                }

            players.Add(newPlayer);
        }

        public void StartGame(int currentPlayer = -1)
        {
            if (players.Count == 0)
            {
                log.Error("Добавьте игроков в игру. Сейчас их " + players.Count);
                return;
            }

            if (!isGameEnded)
            {
                log.Info("Игра еще не закончилась");
                return;
            }

            int enemy;
            if (currentPlayer != -1)
            {
                CurrentPlayer = players[currentPlayer];
                enemy = currentPlayer == 1 ? 0 : 1;
            }
            else
            {
                Random rnd = new Random();
                int indexPlayer = rnd.Next(0, 2);
                enemy = indexPlayer == 1 ? 0 : 1;
                CurrentPlayer = players[indexPlayer];
            }

            EnemyPlayer = players[enemy];
            isGameEnded = false;

            log.Info("Game is started. CurrentPlayer: " + CurrentPlayer);

            currentMove = 0;
   
 

            SetPlayerCards(CurrentPlayer);
            SetPlayerCards(EnemyPlayer);

            if(CurrentPlayer.type == TypePlayer.AI)
                MakeMoveAi();
        }

        public List<Card> GetAiUsedCard()
        {
            List<Card> returnVal = new List<Card>();
            var result = logCard.Where(x=>x.player.type == TypePlayer.AI && x.move == currentMove);

            foreach (var item in result)
            {
                returnVal.Add(item.card);
            }

            return returnVal;
        }

        
        /// <summary>
        /// Проверка хватает ли ресурсов для использования карты
        /// </summary>
        public bool IsCanUseCard(Price price)
        {
            if (CurrentPlayer.PlayerParams[price.attributes] >= price.value)
                return true;

            return false;
        }

        public bool IsCanUseCard(int id)
        {
            int index;
            return IsCanUseCard(GetCardById(id, out index).price);
        }





        /// <summary>
        /// Устанавливаем карты для игрока
        /// </summary>
        /// <returns></returns>
        private void SetPlayerCards(Player player)
        {
            if (QCard.Count < MaxCard)
            {
                serverCards.Randomize(); //TODO: стоил ли перемешивать список карт, каждый раз перед добавлением в стэк?
                foreach (var item in serverCards)
                {
                    QCard.Enqueue(item);
                }
            }

            while (player.Cards.Count < MaxCard)
            {
                if (QCard.Count == 0)
                    break;

                var newCard = QCard.Dequeue();
                newCard.description = ParseDescription.Parse(newCard.description);
                player.Cards.Add(newCard);
            }
        }

        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <param name="dropCard">Флаг, что карта сбрасывается</param>
        public void MakePlayerMove(int id, bool dropCard = false)
        {
            if (CurrentPlayer.gameActions.Contains(GameAction.Succes))
                return;

            int index;
            var card = GetCardById(id, out index);
            
            if (IsCanUseCard(card.price) && !dropCard)
            {

                if (CurrentPlayer.gameActions.Contains(GameAction.PlayAgain))
                    CurrentPlayer.gameActions.Remove(GameAction.PlayAgain);

                if (!specialCardHandlers.ContainsKey(id))
                    card.Apply(CurrentPlayer, EnemyPlayer);
                else 
                {
                    specialCardHandlers[id].copyParams(card);
                    specialCardHandlers[id].Apply(CurrentPlayer, EnemyPlayer);

                    if (specialCardHandlers[id].discard)
                        CurrentPlayer.gameActions.Add(GameAction.DropCard);
                }

                if (card.playAgain)
                    CurrentPlayer.gameActions.Add(GameAction.PlayAgain);



                logCard.Add(new GameCardLog(CurrentPlayer, GameEvent.Used, CurrentPlayer.Cards[index], currentMove));
                Debug.Print(CurrentPlayer.playerName + " use " + CurrentPlayer.Cards[index].id);
                CurrentPlayer.Cards.RemoveAt(index);
            }
            else
            {
                if (id == 40 && dropCard)
                {
                    log.Info("Эту карту нельзя сбросить!");
                    return;
                }

                try
                {
                    CurrentPlayer.gameActions.Remove(GameAction.DropCard);
                    logCard.Add(new GameCardLog(CurrentPlayer, GameEvent.Droped, CurrentPlayer.Cards[index], currentMove));
                    Debug.Print(CurrentPlayer.playerName + " drop " + CurrentPlayer.Cards[index].id);
                    CurrentPlayer.Cards.RemoveAt(index);  
                }
                catch
                {
                    log.Error("Player: " + CurrentPlayer.playerName + " can't pass card " + CurrentPlayer.Cards[index].name);
                }
            }

            if (CurrentPlayer.gameActions.Count == 0)
                CurrentPlayer.gameActions.Add(GameAction.Succes);
        }


        public void NextPlayerTurn()
        {
            if (!CurrentPlayer.gameActions.Contains(GameAction.Succes) || isGameEnded)
                return;

                SetPlayerCards(CurrentPlayer);

                CurrentPlayer.UpdateParams();
                CurrentPlayer.gameActions.Clear();

                Winner = GameControllerHelper.CheckPlayerParams(players, WinParams, LoseParams);
            if (Winner.Length > 0)
            {
                isGameEnded = true;
                return;
            }

            SetPlayerCards(EnemyPlayer);

                Player temp = CurrentPlayer;
                CurrentPlayer = EnemyPlayer;
                EnemyPlayer = temp;


                if (CurrentPlayer.type == TypePlayer.AI)
                    MakeMoveAi();
        }

        private Card GetCardById(int id, out int index)
        {
            index = CurrentPlayer.Cards.FindIndex(x => x.id == id);
            return CurrentPlayer.Cards[index];
        }
        

        private void MakeMoveAi()
        {
            log.Info("----===== Ход компьютера =====----");

            MakePlayerMove(CurrentPlayer.ChooseCard().id);

            GameAction[] copyAction = new GameAction[CurrentPlayer.gameActions.Count] ;
            CurrentPlayer.gameActions.CopyTo(copyAction);

            foreach (var gameAction in copyAction)
            {
                switch (gameAction)
                {
                    case GameAction.DropCard: //TODO: зашить, что сначала идет сброс
                        MakePlayerMove(CurrentPlayer.Cards.First().id, true);
                        break;
                    case GameAction.PlayAgain:
                        MakePlayerMove(CurrentPlayer.ChooseCard().id);
                        break;
                }
            }

            NextPlayerTurn();

            log.Info("----===== Ход компьютера закончился =====----");
        }




    
    }
}
