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
        private List<Player> players { get; set; }
        private int currentPlayer { get; set; }

        private CurrentAction _additionaStatus;

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
        }

        private int currentMove { get; set; }

        private bool isGameEnded
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
        }

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
            Status = CurrentAction.None;
            log = _log;
            LoseParams = GameControllerHelper.GetLoseParams();
            WinParams = GameControllerHelper.GetWinParams();
            players = new List<Player>();
           
            if (server == null)
                host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url));
            else
                host = server;

            //Устанавливаем соответствие между методом и статусом
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
            eventHandlers.Add(CurrentAction.PlayerMustDropCard, PlayerMustDropCard);
            
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

    

        #region public methods

        public void ChangeMaxCard(int NewMaxCard)
        {
            MaxCard = NewMaxCard;
        }

        public List<Card> GetAIUsedCard()
        {
            List<Card> returnVal = new List<Card>();
            var result = logCard.Where(x=>x.player.type == TypePlayer.AI && x.move == currentMove);

            foreach (var item in result)
            {
                returnVal.Add(item.card);
            }

            return returnVal;
        }

        public void SendGameNotification(Dictionary<string, object> information)
        {
           if (information == null || information.Count == 0  || !information.ContainsKey("CurrentAction"))
                log.Error("Нет информации о событие");

            if (eventHandlers.ContainsKey(Status))
                eventHandlers[Status](information);
            else
                log.Error("Cтатус " + Status + " не описан в коде");
        }

       

        public Dictionary<Attributes, int> GetPlayerParams(SelectPlayer selectPlayer = SelectPlayer.None)
        {
            
            if (selectPlayer == SelectPlayer.None)
                selectPlayer = (SelectPlayer)currentPlayer;

            int i = (int) selectPlayer;

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


        public List<Card> GetPlayersCard(SelectPlayer selectPlayer = SelectPlayer.None)
        {
            if (selectPlayer == SelectPlayer.None)
                selectPlayer = (SelectPlayer)currentPlayer;

            int i = (int)selectPlayer;

            if (players[i] != null)
            {
                log.Info("GetPlayerParams type: " + players[i].type);
                return players[i].Cards;
            }
          
            return new List<Card>();
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


        #endregion


        #region private methods

        /// <summary>
        /// Получает карту из стека карт
        /// </summary>
        /// <returns></returns>
        private List<Card> GetCard()
        {

            if (Status != CurrentAction.GetPlayerCard && Status != CurrentAction.GetAICard)
            {
                log.Error("Нельзя получить карты при текущем статусе");
                return null;
            }
            
            
            List<Card> serverCards = new List<Card>();
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


            List<Card> returnVal = new List<Card>();
            int playerCountCard = 0;

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
        private bool UseCard(int id)
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

    

        private bool PassMove(int id)
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
            if (Status == CurrentAction.EndHumanMove)
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
            }

            log.Info("----===== Ход компьютера закончился =====----");
        }

#endregion



        #region Methods switch

        private void PlayerMustDropCard(Dictionary<string, object> info)
        {
            if (info["CurrentAction"].ToString() == "PassStroke")
            {
                if (PassMove((int)info["ID"]))
                {
                    if (players[currentPlayer].type == TypePlayer.Human)
                    {
                        Status = CurrentAction.GetPlayerCard;
                        info["CurrentAction"] = CurrentAction.WaitHumanMove.ToString();
                        SendGameNotification(info);
                    }
                    else
                    {
                        additionaStatus = CurrentAction.None;
                        Status = CurrentAction.GetAICard;
                    }

                    players[currentPlayer].UpdateParams();
                    
                }
            }
        }

        private void EndGame(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "StartGame")
            {
                Status = CurrentAction.None;
                SendGameNotification(information);
            }
        }

        private void EndAIMove(Dictionary<string, object> information = null)
        {
            currentPlayer = currentPlayer == 1 ? 0 : 1;
            currentMove ++;
            Status = CurrentAction.WaitHumanMove;
        }

        private void AIUseCardAnimation(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "AIMoveIsAnimated")
            {
                Status = CurrentAction.AIMoveIsAnimated;
                information["CurrentAction"] = CurrentAction.UpdateStatAI.ToString();
                SendGameNotification(information);
            }
        }

        private void AIMoveIsAnimated(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "UpdateStatAI")
            {
                players[currentPlayer].UpdateParams();
                Status = CurrentAction.UpdateStatAI;
            }
        }

        private void EndHumanMove(Dictionary<string, object> information = null)
        {
            Status = CurrentAction.GetPlayerCard;
            SendGameNotification(new Dictionary<string, object>() {{ "CurrentAction", CurrentAction.EndHumanMove }});

            currentPlayer = currentPlayer == 1 ? 0 : 1;
            currentMove++;

            if (players[currentPlayer].type == TypePlayer.AI)
            {
                MakeMoveAI();
                Status = CurrentAction.AIUseCardAnimation;
            }
        }



        private void UpdateStat(Dictionary<string, object> information)
        {
            Winner = GameControllerHelper.CheckPlayerParams(players, WinParams, LoseParams);
            if (Winner.Length > 0)
            {
                Status = CurrentAction.EndGame;
                return;
            }

            if (information["CurrentAction"].ToString() == "EndHumanMove" || information["CurrentAction"].ToString() == "EndAIMove")
            {
                Status = information["CurrentAction"].ToString() == "EndHumanMove" ? CurrentAction.EndHumanMove : CurrentAction.EndAIMove;
                SendGameNotification(information);
            }
        }

        private void HumanUseCard(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() != "AnimateHumanMove")
                return;

            if (additionaStatus == CurrentAction.PlayerMustDropCard)
            {             

                Status = CurrentAction.GetPlayerCard;
                information["CurrentAction"] = CurrentAction.WaitHumanMove.ToString();
                SendGameNotification(information);
            }
            else
            {
                Status = CurrentAction.UpdateStatHuman;
                players[currentPlayer].UpdateParams();
            }
        }

        private void PassStroke(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() != "AnimateHumanMove")
                return;

            if (additionaStatus == CurrentAction.PlayerMustDropCard)
            {
                additionaStatus = CurrentAction.None;

                Status = CurrentAction.GetPlayerCard;
                information["CurrentAction"] = CurrentAction.WaitHumanMove.ToString();
                SendGameNotification(information);
            }
            else
                Status = players[currentPlayer].type == TypePlayer.Human
                    ? CurrentAction.UpdateStatHuman
                    : CurrentAction.UpdateStatAI;

            players[currentPlayer].UpdateParams();
        }

        private void WaitHumanMove(Dictionary<string, object> information)
        {
            CurrentAction action;
            if (information.TryGetValue("CurrentAction", out object val2))
            {
                action = (CurrentAction)Enum.Parse(typeof(CurrentAction), val2.ToString(), true);
            }
            else
            {
                log.Info($"information: {string.Join(";", information.Select(x => $"{x.Key}:{x.Value}").ToArray())}");
                throw new ArgumentException("Can't find CurrentAction");
            }

            int ID;
            if (action != CurrentAction.AIMoveIsAnimated && information.TryGetValue("ID", out object val))
            {
                ID = (int)val;
            }
            else
            {
                log.Info($"information: {string.Join(";", information.Select(x => $"{x.Key}:{x.Value}").ToArray())}");
                throw new ArgumentException("Can't find ID");
            }      
            
            if (additionaStatus == CurrentAction.PlayerMustDropCard && PassMove(ID))
            {
                players[currentPlayer].UpdateParams();
                Status = CurrentAction.GetPlayerCard;
                information["CurrentAction"] = CurrentAction.PassStroke.ToString();
                SendGameNotification(information);
                return;
            }

            if (action == CurrentAction.HumanUseCard && UseCard(ID))
            {
                if (Status != CurrentAction.PlayAgain)
                    Status = CurrentAction.HumanUseCard;
                else
                {
                    players[currentPlayer].UpdateParams();

                    Status = CurrentAction.GetPlayerCard;
                    information["CurrentAction"] = CurrentAction.WaitHumanMove.ToString();
                    SendGameNotification(information);
                }
            }

            if (action == CurrentAction.PassStroke && PassMove(ID))
                Status = CurrentAction.PassStroke;
        }

        private void GetPlayerCard(Dictionary<string, object> information)
        {
            var type = GameControllerHelper.ConvertObjToEnum<CurrentAction>(information["CurrentAction"]);
            GetCard();
            Status = type;
            
        }

        private void StartGame(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "AIUseCardAnimation")
            {
                Status = CurrentAction.EndHumanMove;
                MakeMoveAI();
                Status = CurrentAction.AIUseCardAnimation;
            }
            else if (information["CurrentAction"].ToString() == "GetPlayerCard")
            {
        
                Status = CurrentAction.GetPlayerCard;
                information["CurrentAction"] = CurrentAction.WaitHumanMove.ToString();
                SendGameNotification(information);
            }
        }

        private void None(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "StartGame")
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


                Status = CurrentAction.StartGame;
                additionaStatus = CurrentAction.None;

                if (information.ContainsKey("currentPlayer"))
                {
                    var op = GameControllerHelper.ConvertObjToEnum<TypePlayer>(information["currentPlayer"]);

                    if (op != players[currentPlayer].type)
                        currentPlayer = currentPlayer == 1 ? 0 : 1;
                }
                else
                {
                    Random rnd = new Random();
                    currentPlayer = rnd.Next(0, 2);
                }

                log.Info("Game is started. CurrentPlayer: " + currentPlayer);
                currentMove = 0;

                Dictionary<string, object> notify = new Dictionary<string, object>();

                if (players[currentPlayer].type == TypePlayer.AI)
                    notify.Add("CurrentAction", CurrentAction.AIUseCardAnimation);
                else
                    notify.Add("CurrentAction", CurrentAction.GetPlayerCard);

                if (information.ContainsKey("CardTricksters"))
                    specialHand = (List<int>)information["CardTricksters"];


                SendGameNotification(notify);
            }
        }

        #endregion

    }
}
