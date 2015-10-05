using System;
using System.Collections.Generic;
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
        private int currentPlayer { get; set; }

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

        public List<Card> GetPlayersCard(SelectPlayer selectPlayer = SelectPlayer.None)
        {
            int i = (int)(selectPlayer == SelectPlayer.None ? (SelectPlayer)currentPlayer : selectPlayer);

            if (players[i] != null)
            {
                log.Info("GetPlayerParams type: " + players[i].type);
                return players[i].Cards;
            }

            return new List<Card>();
        }
       

        public Dictionary<Attributes, int> GetPlayerParams(SelectPlayer selectPlayer = SelectPlayer.None)
        {

            int i = (int)(selectPlayer == SelectPlayer.None ? (SelectPlayer)currentPlayer : selectPlayer);

            if (players[i] != null)
            {
                log.Info("GetPlayerParams type: " + players[i].type);
                return players[i].PlayerParams;
            }
           

            Dictionary<Attributes, int> DefaultParams = new Dictionary<Attributes, int>();
                 DefaultParams = new Dictionary<Attributes, int>();
                 DefaultParams.Add(Attributes.Wall, 5);
                 DefaultParams.Add(Attributes.Tower, 10);

                 DefaultParams.Add(Attributes.Menagerie, 1);
                 DefaultParams.Add(Attributes.Colliery, 1);
                 DefaultParams.Add(Attributes.DiamondMines, 1);

                 DefaultParams.Add(Attributes.Rocks, 5);
                 DefaultParams.Add(Attributes.Diamonds, 5);
                 DefaultParams.Add(Attributes.Animals, 5);

            return DefaultParams;
            
        }


        public void AddPlayer(TypePlayer tp, string name, IStartParams startParams = null)
        {

            if (Status == CurrentAction.StartGame)
            {
                log.Error("Невозможно добавить игроков во время игры");
                return;
            }

            if (players.Count == 2)
            {
                log.Error("Достигнуто максимальное количество игроков");
                return;
            }

            if (startParams == null)
                startParams = new GameStartParams();

            players.Add(new Player(name, tp, startParams));
        }




        
        /// <summary>
        /// Проверка хватает ли ресурсов для использования карты
        /// </summary>
        public bool IsCanUseCard(Price price)
        {
            if (players[currentPlayer].PlayerParams[price.attributes] >= price.value)
                return true;

            return false;
        }

        public bool IsCanUseCard(int id)
        {
            int index;
            return IsCanUseCard(GetCardById(id, out index).price);
        }





        /// <summary>
        /// Получает карту из стека карт
        /// </summary>
        /// <returns></returns>
        private List<Card> GetCard()
        {
            List<Card> returnVal = new List<Card>();
            List<Card> serverCards = new List<Card>();
            int playerCountCard = 0;

            if (Status != CurrentAction.GetPlayerCard && Status != CurrentAction.GetAICard)
            {
                log.Error("Нельзя получить карты при текущем статусе");
                return null;
            }
            
            
            if (QCard.Count < MaxCard)
            {
                string cardFromServer = host.GetRandomCard();
                serverCards = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);

                if (serverCards.Count == 0)
                    return null;

                foreach (var item in serverCards)
                {
                    QCard.Enqueue(item);
                }
            }
            

            //если хоти смухлевать с картами для игрока
            if (specialHand.Count > 0)
            {
                foreach (var item in specialHand)
                {
                   if(playerCountCard == MaxCard)
                       break;

                    var newCard = serverCards.FirstOrDefault(x => x.id == item);
                    if (newCard != null)
                    {
                        newCard.description = ParseDescription.Parse(newCard.description);
                        returnVal.Add(newCard);
                        playerCountCard++;
                    }
                }

                players[currentPlayer].Cards.AddRange(returnVal);
                specialHand = new List<int>();
                return returnVal;
            }

     

            if (players[currentPlayer].Cards != null)
                playerCountCard = players[currentPlayer].Cards.Count;

            while (playerCountCard < MaxCard)
            {
                if (QCard.Count == 0)
                    break;

                var newCard = QCard.Dequeue();
                newCard.description = ParseDescription.Parse(newCard.description);
                returnVal.Add(newCard);
                playerCountCard++;
            }

            players[currentPlayer].Cards.AddRange(returnVal);
            return returnVal;
        }

        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <returns>если карту не удалось использовать возвращается false</returns>
        public bool UseCard(int id)
        {
            int index;
            var card = GetCardById(id, out index);

            if (IsCanUseCard(card.price))
            {

                int Enemyindex = currentPlayer == 1 ? 0 : 1;
                if (!specialCardHandlers.ContainsKey(id))
                    card.Apply(players[currentPlayer], players[Enemyindex]);
                else 
                {
                    specialCardHandlers[id].copyParams(card);
                    specialCardHandlers[id].Apply(players[currentPlayer], players[Enemyindex]);

                    if (specialCardHandlers[id].discard)
                        additionaStatus = CurrentAction.PlayerMustDropCard;
                }

                if (card.playAgain)
                    Status = CurrentAction.PlayAgain;

                logCard.Add(new GameCardLog(players[currentPlayer], GameEvent.Used,players[currentPlayer].Cards[index],currentMove));

                players[currentPlayer].Cards.RemoveAt(index);
                return true;
            }
            
            return false;
        }

       

        private Card GetCardById(int id, out int index)
        {
            index = players[currentPlayer].Cards.FindIndex(x => x.id == id);
            return players[currentPlayer].Cards[index];
        }

    

        public bool PassMove(int id)
        {
            if (id == 40)
            {
                log.Info("Эту карту нельзя сбросить!");
                return false;
            }

            int index = players[currentPlayer].Cards.FindIndex(x => x.id == id);
            
            try
            {
                logCard.Add(new GameCardLog(players[currentPlayer], GameEvent.Droped, players[currentPlayer].Cards[index], currentMove));
                players[currentPlayer].Cards.RemoveAt(index);               
                return true;
            }
            catch
            {
                log.Error("Player: " + players[currentPlayer].playerName + " can't pass card " + players[currentPlayer].Cards[index].name);
            }


            return false;
        }


        private void MakeMoveAI()
        {
            log.Info("----===== Ход компьютера =====----");

            //TODO: вставка гавнокода, будет исправлено при доработке AI
           /* if (Status == CurrentAction.EndHumanMove)
            {
                Status = CurrentAction.GetAICard;
                GetCard();

                Status = CurrentAction.EndHumanMove;
            }
          

            while (Status != CurrentAction.AIUseCard)
            {
                if (Status == CurrentAction.GetAICard)
                {
                    GetCard();
                    Status = CurrentAction.AIUseCard;
                }

                if (Status == CurrentAction.PlayAgain)
                {
                    Status = CurrentAction.GetAICard;
                    GetCard();
                    players[currentPlayer].UpdateParams();
                    Status = CurrentAction.AIUseCard;
                }

                foreach (var item in players[currentPlayer].Cards)
                {
                    if (UseCard(item.id))
                    {

                        if (Status == CurrentAction.EndHumanMove)
                            Status = CurrentAction.AIUseCard;

                        if (additionaStatus == CurrentAction.PlayerMustDropCard)
                        {
                            if (PassMove(players[currentPlayer].Cards.First().id))
                            {
                                additionaStatus = CurrentAction.None;
                                Status = CurrentAction.GetAICard;
                                players[currentPlayer].UpdateParams();
                            }

                            if (Status == CurrentAction.GetAICard)
                            {
                                GetCard();
                            }
                        }

                        break;
                    }
                }


                if (Status == CurrentAction.EndHumanMove)
                    break;
             
            }

            //TODO: еще один костыль
            if (Status == CurrentAction.AIUseCard)
            {
                Status = CurrentAction.GetAICard;
                 GetCard();
                 Status = CurrentAction.AIUseCard;
            }

               //если компьютер не использовал карту и ему не нужно брать еще одну, тогда пропускаем ход
            if (Status != CurrentAction.AIUseCard)
            {
                PassMove(players[currentPlayer].Cards.First().id);
            }*/

            log.Info("----===== Ход компьютера закончился =====----");
        }




    
    }
}
