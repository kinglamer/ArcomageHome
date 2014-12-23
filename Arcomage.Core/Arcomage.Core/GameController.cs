using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.ServiceModel.Security.Tokens;
using System.Text;
using Arcomage.Common;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;

namespace Arcomage.Core
{
    public enum TypePlayer
    {
        AI, Human
    }

    public enum SelectPlayer
    {
        First = 0,
        Second,
        None
    }

    public enum CurrentAction
    {
        None, //Произошел первый запуск
        StartGame, //Начало игры
        GetPlayerCard, //Получить карты для игрока
        WaitHumanMove, //Ожидание хода игрока
        HumanUseCard, //Игрок использовал карту
        HumanCanPlayAgain, //Игрок может сходить еще раз
        AnimateHumanMove, //Анимация использования карты игрока
        UpdateStatHuman, //Обновление статистики игрока
        UpdateStatAI, //Обновление статистики компьютера
        EndHumanMove, //Завершение хода игрока
        PlayerMustDropCard, //Статус, что игрок обязан сбросить карту
        PlayAgain, //Флаг того, что нужно сыграть еще одну карту
        AIUseCard, //Флаг того, что компьютер завершил использование всех карт (появилось в следствие того, что есть карты, которые заставляют брать еще карту)
        AIMoveIsAnimated, //Анимация стола противника
        AIUseCardAnimation, //Анимация использование хода противника
        EndAIMove, //Завершение хода противника
        EndGame, //Завершение игры
        PassStroke, //пропуск хода
        GetAICard //компьютер берет следующую добавочную карту
    }

   
    public delegate void EventMethod(Dictionary<string, object> info);

    public class GameController
    {

        #region Variables 

        private readonly int MaxCard;
        private List<IPlayer> players { get; set; }
        private int currentPlayer { get; set; }

      
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
        protected readonly IStartParams startParams;

        private readonly Dictionary<Specifications, int> WinParams;
        private readonly Dictionary<Specifications, int> LoseParams;
        private const string url = "http://arcomage.somee.com/ArcoServer.svc?wsdl"; //"http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        public string Winner { get;private set; }
        private IArcoServer host;

        private Dictionary<CurrentAction, EventMethod> eventHandlers;
        private Dictionary<int, Action> specialCardHandlers;


        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private Queue<Card> QCard = new Queue<Card>();

        public List<GameCardLog> logCard = new List<GameCardLog>();

        #endregion


        public GameController(ILog _log, IArcoServer server = null, IStartParams _startParams = null)
        {
            if (_startParams == null)
                _startParams = new GameStartParams();
            startParams = _startParams;

            MaxCard = startParams.MaxPlayerCard;
            Status = CurrentAction.None;
            log = _log;
            LoseParams = GameControllerHelper.GetLoseParams();
            WinParams = GameControllerHelper.GetWinParams();
            players = new List<IPlayer>();
           
            if (server == null)
            {
                host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url));
            }
            else
            {
                host = server;
            }

            //Устанавливаем соответствие между методом и статусом
            eventHandlers = new Dictionary<CurrentAction, EventMethod>();
            eventHandlers.Add(CurrentAction.None,None);
            eventHandlers.Add(CurrentAction.StartGame, StartGame);
            eventHandlers.Add(CurrentAction.GetPlayerCard, GetPlayerCard);
            eventHandlers.Add(CurrentAction.WaitHumanMove, WaitHumanMove);
            eventHandlers.Add(CurrentAction.PassStroke, PassStroke);
            eventHandlers.Add(CurrentAction.HumanUseCard, HumanUseCard);
            eventHandlers.Add(CurrentAction.UpdateStatHuman, UpdateStatHuman);
            eventHandlers.Add(CurrentAction.UpdateStatAI, UpdateStatAI);
            eventHandlers.Add(CurrentAction.EndHumanMove, EndHumanMove);
            eventHandlers.Add(CurrentAction.AIMoveIsAnimated, AIMoveIsAnimated);
            eventHandlers.Add(CurrentAction.AIUseCardAnimation, AIUseCardAnimation);
            eventHandlers.Add(CurrentAction.EndAIMove, EndAIMove);
            eventHandlers.Add(CurrentAction.EndGame, EndGame);
            eventHandlers.Add(CurrentAction.PlayerMustDropCard, PlayerMustDropCard);

            specialCardHandlers = new Dictionary<int, Action>();
            specialCardHandlers.Add(5, Card5);
            specialCardHandlers.Add(8, Card8);
            specialCardHandlers.Add(12, Card12);
            specialCardHandlers.Add(31, Card31);
            specialCardHandlers.Add(32, Card32);
            specialCardHandlers.Add(34, Card34);
            specialCardHandlers.Add(39, CardWithDiscard);
            specialCardHandlers.Add(48, Card48);
            specialCardHandlers.Add(64, Card64);
            specialCardHandlers.Add(67, Card67);
            specialCardHandlers.Add(73, CardWithDiscard);
            specialCardHandlers.Add(87, Card87);
            specialCardHandlers.Add(89, Card89);
            specialCardHandlers.Add(90, Card90);
            specialCardHandlers.Add(91, Card91);
            specialCardHandlers.Add(98, Card98);

          
        }

    

        #region public methods


        public List<Card> GetAIUsedCard()
        {
            List<Card> returnVal = new List<Card>();
           
             

                var result = logCard.Where(x => x.gameEvent == GameEvent.Used && x.player.type == TypePlayer.AI).FirstOrDefault();

                if (result != null)
                    returnVal.Add(result.card);

            

            return returnVal;
        }

        public void SendGameNotification(Dictionary<string, object> information)
        {
            if (information == null || information.Count == 0  || !information.ContainsKey("CurrentAction"))
            {
                log.Error("Нет информации о событие");
            }

            if (eventHandlers.ContainsKey(Status))
            {
                eventHandlers[Status](information);
            }
            else
            {
                log.Error("Cтатус " + Status + " не описан в коде");
            }
        }

       

        public Dictionary<Specifications, int> GetPlayerParams(SelectPlayer selectPlayer = SelectPlayer.None)
        {
            
            if (selectPlayer == SelectPlayer.None)
            {
                selectPlayer = (SelectPlayer)currentPlayer;
            }

            int i = (int) selectPlayer;

            if (players[i] != null)
            {
                log.Info("GetPlayerParams type: " + players[i].type);
                return players[i].Statistic;
            }
            else
            {
                return startParams.GenerateDefault();
            }
        }


        public void AddPlayer(TypePlayer tp, string name)
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

            IPlayer newPlayer;
            switch (tp)
            {
                case TypePlayer.AI:
                    newPlayer = new AI(name, tp);
                    break;
                case TypePlayer.Human:
                    newPlayer = new Player(name, tp);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("tp");
            }

            newPlayer.Statistic = startParams.GenerateDefault();
            players.Add(newPlayer);
        
        }

        /// <summary>
        /// Получает карту из стека карт
        /// </summary>
        /// <returns></returns>
        public List<Card> GetCard()
        {

            if (Status != CurrentAction.GetPlayerCard && Status != CurrentAction.GetAICard)
            {
                log.Error("Нельзя получить карты при текущем статусе");
                return null;
            }

            if (QCard.Count < MaxCard)
            {


                string cardFromServer = host.GetRandomCard();

                List<Card> newParametrs = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);

                if (newParametrs.Count == 0)
                    return null;

                foreach (var item in newParametrs)
                {
                    QCard.Enqueue(item);
                }
            }

            log.Info("Стэк карт равен " + QCard.Count);

            List<Card> returnVal = new List<Card>();

            int playerCountCard = 0;

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

            log.Info("Player: " + players[currentPlayer].playerName + " GET " + returnVal.Count + " cards");
            players[currentPlayer].Cards.AddRange(returnVal);

            return returnVal;
        }


        #region Can Use Card 
        /// <summary>
        /// Проверка хватает ли ресурсов для использования карты
        /// </summary>
        public bool IsCanUseCard(ICollection<CardParams> cardParams)
        {
            bool returnVal = false;
            foreach (var item in cardParams)
            {
                switch (item.key)
                {
                    case Specifications.CostDiamonds:
                        if (players[currentPlayer].Statistic[Specifications.PlayerDiamonds] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                    case Specifications.CostAnimals:
                        if (players[currentPlayer].Statistic[Specifications.PlayerAnimals] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                    case Specifications.CostRocks:
                        if (players[currentPlayer].Statistic[Specifications.PlayerRocks] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                }
            }

            if (cardParams.Count == 0)
                log.Error("У карты не указана стоимость");



            return returnVal;
        }

        public bool IsCanUseCard(int id)
        {
            int index;
            List<CardParams> costCard = GetCardById(id, out index);

            return IsCanUseCard(costCard);
        }

        #endregion

        #endregion


        #region private methods
        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <returns>если карту не удалось использовать возвращается false</returns>
        private bool UseCard(int id)
        {
            if (isGameEnded)
            {
                log.Info("Игра уже закончилась");
                return false;
            }

            int index;
            List<CardParams> costCard = GetCardById(id, out index);


            if (IsCanUseCard(costCard))
            {
                log.Info("Player: " + players[currentPlayer].playerName + " use card: " + players[currentPlayer].Cards[index].name);

                //если карта не имеет специального обработчика, тогда используем как обычно
                if (!specialCardHandlers.ContainsKey(id))
                {
                  
                    ApplyCardParamsToPlayer(players[currentPlayer].Cards[index].cardParams);
                }
                else //иначе вызываем специальный обработчик и потом отнимаем стоимость карты
                {
                    specialCardHandlers[id].Invoke();
                    ApplyCardParamsToPlayer(costCard);

                }

            
               

                logCard.Add(new GameCardLog() {
                    card = players[currentPlayer].Cards[index],
                    gameEvent = GameEvent.Used,
                    player = players[currentPlayer]});

                players[currentPlayer].Cards.RemoveAt(index);
                return true;
            }
            else
            {
                log.Info("Player: " + players[currentPlayer].playerName + " can't use card: " + players[currentPlayer].Cards[index].name);
            }


            return false;
        }

       

        private List<CardParams> GetCardById(int id, out int index)
        {
            index = players[currentPlayer].Cards.FindIndex(x => x.id == id);

            List<CardParams> costCard = players[currentPlayer].Cards[index].cardParams.Where(x => x.key == Specifications.CostDiamonds ||
                                                                                 x.key == Specifications.CostAnimals ||
                                                                                 x.key == Specifications.CostRocks).ToList();
            return costCard;
        }


    

        private bool PassMove(int id)
        {
            if (isGameEnded)
            {
                log.Info("Игра уже закончилась");
                return false;
            }

            if (id == 40 )
            {
                log.Info("Эту карту нельзя сбросить!");
                return false;
            }

            int index = players[currentPlayer].Cards.FindIndex(x => x.id == id);



            try
            {
                logCard.Add(new GameCardLog()
                {
                    card = players[currentPlayer].Cards[index],
                    gameEvent = GameEvent.Droped,
                    player = players[currentPlayer]
                });

                log.Info("Player: " + players[currentPlayer].playerName + " DROP CARD " + players[currentPlayer].Cards[index].name);
                players[currentPlayer].Cards.RemoveAt(index);               
                return true;
            }
            catch
            {
                log.Error("Player: " + players[currentPlayer].playerName + " can't pass card " + players[currentPlayer].Cards[index].name);
            }


            return false;
        }



        private void UpdateStatistic()
        {
            var playerParams = players[currentPlayer].Statistic;

            log.Info("UpdateStatistic for " + players[currentPlayer].type);
            GameControllerHelper.PlusValue(Specifications.PlayerDiamonds, playerParams[Specifications.PlayerDiamondMines], players[currentPlayer]);
            GameControllerHelper.PlusValue(Specifications.PlayerAnimals, playerParams[Specifications.PlayerMenagerie], players[currentPlayer]);
            GameControllerHelper.PlusValue(Specifications.PlayerRocks, playerParams[Specifications.PlayerColliery], players[currentPlayer]);
        }

        private void MakeMoveAI()
        {
            log.Info("----===== Ход компьютера =====----");

            //вставка гавнокода, будет исправлено при доработке AI
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
                    UpdateStatistic();
                    Status = CurrentAction.AIUseCard;
                }

                foreach (var item in players[currentPlayer].Cards)
                {
                    if (UseCard(item.id))
                    {

                        if (Status == CurrentAction.EndHumanMove)
                            Status = CurrentAction.AIUseCard;

                        if (Status == CurrentAction.PlayerMustDropCard)
                        {
                            Dictionary<string, object> notify = new Dictionary<string, object>();
                            notify.Add("CurrentAction", CurrentAction.PassStroke);
                            notify.Add("ID", players[currentPlayer].Cards.First().id);
                            SendGameNotification(notify);
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

            if (Status == CurrentAction.AIUseCard)
            {
                 GetCard();
            }

               //если компьютер не использовал карту и ему не нужно брать еще одну, тогда пропускаем ход
            if (Status != CurrentAction.AIUseCard)
            {
                PassMove(players[currentPlayer].Cards.First().id);
            }

            log.Info("----===== Ход компьютера закончился =====----");
        }

#endregion


        #region apply params 

        /// <summary>
        /// Применения параметров карты к игроку
        /// </summary>
        private void ApplyCardParamsToPlayer(ICollection<CardParams> cardParams)
        {



            foreach (var item in cardParams)
            {
                try
                {
                 
                    switch (item.key)
                    {
                        case Specifications.PlayerTower:
                        case Specifications.PlayerWall:
                        case Specifications.PlayerDiamondMines:
                        case Specifications.PlayerMenagerie:
                        case Specifications.PlayerColliery:
                        case Specifications.PlayerDiamonds:
                        case Specifications.PlayerAnimals:
                        case Specifications.PlayerRocks:
                      /*      log.Info(string.Format("BEFORE player: {3} type: {0} val: {1} currentVal: {2}", item.key, item.value,
                     players[currentPlayer].Statistic.ContainsKey(item.key) ? players[currentPlayer].Statistic[item.key].ToString() : "Ключа нет"
                     , players[currentPlayer].type));*/

                            GameControllerHelper.PlusValue(item.key, item.value, players[currentPlayer]);

                        /*    log.Info(string.Format("AFTER player: {3} type: {0} val: {1} currentVal: {2}", item.key, item.value,
                   players[currentPlayer].Statistic.ContainsKey(item.key) ? players[currentPlayer].Statistic[item.key].ToString() : "Ключа нет"
                   , players[currentPlayer].type));*/

                            break;
                        case Specifications.EnemyTower:
                        case Specifications.EnemyWall:
                        case Specifications.EnemyDiamondMines:
                        case Specifications.EnemyMenagerie:
                        case Specifications.EnemyColliery:
                        case Specifications.EnemyDiamonds:
                        case Specifications.EnemyAnimals:
                        case Specifications.EnemyRocks:
                            ApplyCardParamFromEnemy(item);
                            break;
                        case Specifications.CostDiamonds:
                            GameControllerHelper.MinusValue(Specifications.PlayerDiamonds, item.value, players[currentPlayer]);
                            break;
                        case Specifications.CostAnimals:
                            GameControllerHelper.MinusValue(Specifications.PlayerAnimals, item.value, players[currentPlayer]);
                            break;
                        case Specifications.CostRocks:
                            GameControllerHelper.MinusValue(Specifications.PlayerRocks, item.value,  players[currentPlayer]);
                            break;
                        case Specifications.EnemyDirectDamage:
                            int Ememyindex = currentPlayer == 1 ? 0 : 1;
                            ApplyDirectDamage(item, Ememyindex);
                            break;
                        case Specifications.PlayerDirectDamage:
                            ApplyDirectDamage(item, currentPlayer);
                            break;
                        case Specifications.PlayAgain:
                            if (Status != CurrentAction.EndHumanMove || Status != CurrentAction.EndAIMove || Status != CurrentAction.EndGame)
                                Status = CurrentAction.PlayAgain;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                  
                }
                catch (Exception ex)
                {
                    log.Error("Ex: " + ex + "\n Additional Info: " + item.key);
                }
            }


        }

        private void ApplyDirectDamage(CardParams item, int player)
        {
            int value = item.value;
            
            if (item.value > 0)
                value = -item.value;

            int tempVal = players[player].Statistic[Specifications.PlayerWall] + value;

            if (tempVal < 0)
            {
                GameControllerHelper.PlusValue(Specifications.PlayerWall, -players[player].Statistic[Specifications.PlayerWall], players[player]);

                GameControllerHelper.PlusValue(Specifications.PlayerTower, tempVal, players[player]);
            }
            else
            {
                GameControllerHelper.PlusValue(Specifications.PlayerWall, value, players[player]);
            }
        }

        /// <summary>
        /// Метод для изменения параметров противника
        /// </summary>
        /// <param name="item"></param>
        private void ApplyCardParamFromEnemy(CardParams item)
        {

            Specifications resutl = Specifications.NotSet;
            switch (item.key)
            {
                case Specifications.EnemyTower:
                    resutl = Specifications.PlayerTower;
                    break;
                case Specifications.EnemyWall:
                    resutl = Specifications.PlayerWall;
                    break;
                case Specifications.EnemyDiamondMines:
                    resutl = Specifications.PlayerDiamondMines;
                    break;
                case Specifications.EnemyMenagerie:
                    resutl = Specifications.PlayerMenagerie;
                    break;
                case Specifications.EnemyColliery:
                    resutl = Specifications.PlayerColliery;
                    break;
                case Specifications.EnemyDiamonds:
                    resutl = Specifications.PlayerDiamonds;
                    break;
                case Specifications.EnemyAnimals:
                    resutl = Specifications.PlayerAnimals;
                    break;
                case Specifications.EnemyRocks:
                    resutl = Specifications.PlayerRocks;
                    break;
            }

            // log.Info("playerName:" + playerName + ": " + Statistic[resutl]);


            int index = currentPlayer == 1 ? 0 : 1;

         /*   log.Info(string.Format("BEFORE player: {3} type: {0} val: {1} currentVal: {2}", item.key, item.value,
                      players[index].Statistic.ContainsKey(resutl) ? players[index].Statistic[resutl].ToString() : "Ключа нет"
                      , players[index].type));*/

            GameControllerHelper.PlusValue(resutl, item.value, players[index]);

        /*    log.Info(string.Format("BEFORE player: {3} type: {0} val: {1} currentVal: {2}", item.key, item.value,
                      players[index].Statistic.ContainsKey(resutl) ? players[index].Statistic[resutl].ToString() : "Ключа нет"
                      , players[index].type));*/

            //  log.Info("playerName:" + playerName + ": " + Statistic[resutl]);

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
                        Status = CurrentAction.GetPlayerCard;
                    else
                        Status = CurrentAction.GetAICard;

                    UpdateStatistic();
                    
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
                UpdateStatistic();
                Status = CurrentAction.UpdateStatAI;
            }
        }

        private void EndHumanMove(Dictionary<string, object> information = null)
        {
            currentPlayer = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].type == TypePlayer.AI)
            {
                MakeMoveAI();
                Status = CurrentAction.AIUseCardAnimation;
            }
        }

        private void UpdateStatAI(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "EndAIMove")
            {
                Winner = GameControllerHelper.CheckPlayerParams(players, WinParams, LoseParams);
                if (Winner.Length > 0)
                {
                    Status = CurrentAction.EndGame;
                }
                else
                {
                    Status = CurrentAction.EndAIMove;
                    SendGameNotification(information);
                }
            }
        }

        private void UpdateStatHuman(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "EndHumanMove")
            {
                Winner = GameControllerHelper.CheckPlayerParams(players, WinParams, LoseParams);
                if (Winner.Length > 0)
                {
                    Status = CurrentAction.EndGame;
                }
                else
                {
                    Status = CurrentAction.EndHumanMove;
                    SendGameNotification(information);
                }
            }
        }

        private void HumanUseCard(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "AnimateHumanMove")
            {
                UpdateStatistic();
                Status = CurrentAction.UpdateStatHuman;
            }
        }

        private void PassStroke(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "AnimateHumanMove")
            {
                if (players[currentPlayer].type == TypePlayer.Human)
                {
                    Status = CurrentAction.UpdateStatHuman;
                }
                else
                {
                    Status = CurrentAction.UpdateStatAI;
                }
                UpdateStatistic();
            }
        }

        private void WaitHumanMove(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == "HumanUseCard")
            {
                if (UseCard((int)information["ID"]))
                {
                    if (Status != CurrentAction.PlayAgain)
                    {

                        if (Status != CurrentAction.PlayerMustDropCard)
                        {
                          
                            Status = CurrentAction.HumanUseCard;
                         
                        }
                    }
                    else
                    {
                        Status = CurrentAction.WaitHumanMove;
                        UpdateStatistic();
                    }
                }
            }
            else if (information["CurrentAction"].ToString() == "PassStroke")
            {
                if (PassMove((int)information["ID"]))
                {
                    Status = CurrentAction.PassStroke;
                }
            }
        }

        private void GetPlayerCard(Dictionary<string, object> information)
        {
            if (information["CurrentAction"].ToString() == ("WaitHumanMove"))
            {
                Status = CurrentAction.WaitHumanMove;
            }
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

                if (information.ContainsKey("currentPlayer"))
                {
                    var op = GameControllerHelper.ConvertObjToEnum<TypePlayer>(information["currentPlayer"]);

                    if (op != players[currentPlayer].type)
                    {
                        currentPlayer = currentPlayer == 1 ? 0 : 1;
                    }
                }
                else
                {
                    Random rnd = new Random();
                    currentPlayer = rnd.Next(0, 2);
                }
                log.Info("Game is started. CurrentPlayer: " + currentPlayer);


                Dictionary<string, object> notify = new Dictionary<string, object>();
                if (players[currentPlayer].type == TypePlayer.AI)
                {
                    notify.Add("CurrentAction", CurrentAction.AIUseCardAnimation);
                }
                else
                {
                    notify.Add("CurrentAction", CurrentAction.GetPlayerCard);
                }

                SendGameNotification(notify);
            }
        }

        #endregion


        #region Special Card Methods

        private void IfElseWithValue(Specifications spec, int value1, int value2)
        {
            List<CardParams> result = new List<CardParams>();
            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] < players[index].Statistic[spec])
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = spec,
                    value = value1
                }); 
               
            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = spec,
                    value = value2
                }); 
            }

            ApplyCardParamsToPlayer(result);
        }

        private void IfElseWithValue(Specifications spec, int value1, int value2, int equalVal)
        {
            List<CardParams> result = new List<CardParams>();

            if (players[currentPlayer].Statistic[spec] == equalVal)
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = spec,
                    value = value1
                });

            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = spec,
                    value = value2
                });
            }

            ApplyCardParamsToPlayer(result);
        }

        private void Card5( )
        {
            IfElseWithValue(Specifications.PlayerColliery,2,1);
        }

      

        private void Card8()
        {
            //If quarry < enemy quarry, quarry = enemy quarry

            Specifications spec = Specifications.PlayerColliery;
        

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] < players[index].Statistic[spec])
                players[currentPlayer].Statistic[spec] = players[index].Statistic[spec];
          
        }

        private void Card12()
        {
            //If wall = 0,+6 wall,else +3 wall
            IfElseWithValue(Specifications.PlayerWall, 6, 3,0);

        }

        private void Card31()
        {
            Specifications spec = Specifications.PlayerWall;

            //  Player(s) w/lowest Wall are -1 Dungeon -2 Tower

            List<CardParams> result = new List<CardParams>();

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] < players[index].Statistic[spec])
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.PlayerMenagerie,
                    value = -1
                });

                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.PlayerTower,
                    value = -2
                });
            }
            else if (players[currentPlayer].Statistic[spec] > players[index].Statistic[spec])
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyMenagerie,
                    value = -1
                });

                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyTower,
                    value = -2
                });

            }

            ApplyCardParamsToPlayer(result);
        }


        private void Card32()
        {
         //   +6 Recruits +6 Wall . if dungeon < enemy dungeon +1 Dungeon

            Specifications spec = Specifications.PlayerMenagerie;

            List<CardParams> result = new List<CardParams>();

            result.Add(new CardParams()
            {
                card = null,
                id = 0,
                key = Specifications.PlayerAnimals,
                value = 6
            });

            result.Add(new CardParams()
            {
                card = null,
                id = 0,
                key = Specifications.PlayerWall,
                value = 6
            });

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] < players[index].Statistic[spec])
            {

                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.PlayerMenagerie,
                    value = 1
                });
            }

            ApplyCardParamsToPlayer(result);
        }

        private void Card34()
        {
          //  Switch your Wall with enemy Wall
            Specifications spec = Specifications.PlayerWall;
            int index = currentPlayer == 1 ? 0 : 1;
            players[currentPlayer].Statistic[spec] = players[index].Statistic[spec];
        }


        private void Card48()
        {
           // All players magic equals the highest player's magic 
            Specifications spec = Specifications.PlayerDiamondMines;
            int index = currentPlayer == 1 ? 0 : 1;

            if (players[currentPlayer].Statistic[spec] < players[index].Statistic[spec])
            {
                players[currentPlayer].Statistic[spec] = players[index].Statistic[spec];
            }
            else 
            {
                players[index].Statistic[spec] = players[currentPlayer].Statistic[spec];
            }
           
        }

        private void Card64()
        {
           // if Tower < Enemy Tower +2 Tower else +1 Tower
            IfElseWithValue(Specifications.PlayerTower, 2, 1);
        }

        private void Card67()
        {
          //  if Tower > Enemy Wall 8 Damage to Enemy Tower else 8 Damage

            int index = currentPlayer == 1 ? 0 : 1;
            List<CardParams> result = new List<CardParams>();
            if (players[currentPlayer].Statistic[Specifications.PlayerTower] > players[index].Statistic[Specifications.PlayerWall])
            {
            
               result.Add(new CardParams()
               {
                   card = null,
                   id =0,
                   key = Specifications.EnemyTower,
                   value = -8
               }); 
              
            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = -8
                }); 
            }

             ApplyCardParamsToPlayer(result);

        }

        private void Card87()
        {
           // if Enemy Wall = 0 10 Damage else 6 Damage
            Specifications spec = Specifications.PlayerWall;
            List<CardParams> result = new List<CardParams>();

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[index].Statistic[spec] == 0)
            {
                 result.Add(new CardParams()
               {
                   card = null,
                   id =0,
                   key = Specifications.EnemyDirectDamage,
                   value = 10
               }); 
            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 6
                }); 
            }

            ApplyCardParamsToPlayer(result);

        }

        private void Card89()
        {
            //if Enemy Wall > 0 10 Damage else 7 Damage

            Specifications spec = Specifications.PlayerWall;
            List<CardParams> result = new List<CardParams>();

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[index].Statistic[spec] > 0)
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 10
                });
            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 7
                });
            }

            ApplyCardParamsToPlayer(result);
        }

        private void Card90()
        {
            //if Magic > Enemy Magic 12 Damage else 8 Damage

            Specifications spec = Specifications.PlayerDiamondMines;
            List<CardParams> result = new List<CardParams>();
         
            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] > players[index].Statistic[spec])
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 12
                });
            }
            else 
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 8
                });
            }

            ApplyCardParamsToPlayer(result);
        }

        private void Card91()
        {
           // if Wall > Enemy Wall 6 Damage to Enemy Tower else 6 Damage 

            Specifications spec = Specifications.PlayerWall;
            List<CardParams> result = new List<CardParams>();

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] > players[index].Statistic[spec])
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyTower,
                    value = -6
                });
            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 6
                });
            }

            ApplyCardParamsToPlayer(result);

        }


        private void Card98()
        {
            //if Wall > Enemy Wall 3 Damage else 2 Damage

            Specifications spec = Specifications.PlayerWall;
            List<CardParams> result = new List<CardParams>();

            int index = currentPlayer == 1 ? 0 : 1;
            if (players[currentPlayer].Statistic[spec] > players[index].Statistic[spec])
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 3
                });
            }
            else
            {
                result.Add(new CardParams()
                {
                    card = null,
                    id = 0,
                    key = Specifications.EnemyDirectDamage,
                    value = 2
                });
            }

            ApplyCardParamsToPlayer(result);
        }


        private void CardWithDiscard()
        {
            //Draw 1 card Discard 1 card Play again
            Status = CurrentAction.PlayerMustDropCard;
        }

        #endregion



    }
}
