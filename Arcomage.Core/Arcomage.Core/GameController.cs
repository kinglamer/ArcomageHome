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
        AIMoveIsAnimated, //Анимация стола противника
        AIUseCardAnimation, //Анимация использование хода противника
        EndAIMove, //Завершение хода противника
        EndGame, //Завершение игры
        PassStroke //пропуск хода
    }

   

    public class GameController
    {
        public readonly int MaxCard;
        private List<IPlayer> players { get; set; }
        private int currentPlayer { get; set; }

        public CurrentAction status { get; private set; }

        protected readonly ILog log;
        private readonly Dictionary<Specifications, int> WinParams;
        private readonly Dictionary<Specifications, int> LoseParams;
        private const string url = "http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

      
        private IArcoServer host;

        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private Queue<Card> QCard = new Queue<Card>();

        private List<Card> usedCards = new List<Card>();
        private Queue<int> AIUsedCard = new Queue<int>();

        public GameController(ILog _log, IArcoServer server = null)
        {
            MaxCard = 5;
            status = CurrentAction.None;
            log = _log;
            LoseParams = new Dictionary<Specifications, int>();
            LoseParams.Add(Specifications.PlayerTower, 0);

            WinParams = new Dictionary<Specifications, int>();
            WinParams.Add(Specifications.PlayerTower, 50);
            WinParams.Add(Specifications.PlayerAnimals, 150);
            WinParams.Add(Specifications.PlayerRocks, 150);
            WinParams.Add(Specifications.PlayerDiamonds, 150);

            players = new List<IPlayer>();

            if (server == null)
            {
                host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url));
            }
            else
            {
                host = server;
            }
        }

        public bool SendGameNotification(Dictionary<string, object> information)
        {
            bool returnVal = false;
            if (information == null || information.Count == 0  || !information.ContainsKey("CurrentAction"))
            {
                log.Error("Нет информации о событие");
                return false;
            }

         


            switch (status)
            {
                case CurrentAction.None:
                   
                        if (information["CurrentAction"].ToString() == "StartGame")
                        {
                            StartGame();
                            status = CurrentAction.StartGame;

                            if (information.ContainsKey("currentPlayer"))
                            {

                                var op = ConvertObjToEnum<TypePlayer>(information["currentPlayer"]);

                                if (op != players[currentPlayer].type)
                                {
                                    currentPlayer = currentPlayer == 1 ? 0 : 1;
                                }
                            }


                            Dictionary<string, object> notify = new Dictionary<string, object>();
                            if (players[currentPlayer].type == TypePlayer.AI)
                            {
                                notify.Add("CurrentAction", CurrentAction.AIMoveIsAnimated);
                            }
                            else
                            {
                                notify.Add("CurrentAction", CurrentAction.GetPlayerCard);
                            }

                            SendGameNotification(notify);
                        }
                    
                    returnVal = true;
                    break;
                case CurrentAction.StartGame:
                    if (information["CurrentAction"].ToString() == "AIUseCardAnimation")
                    {
                        status = CurrentAction.AIUseCardAnimation;
                        returnVal = true;
                    }
                    else if (information["CurrentAction"].ToString() == "GetPlayerCard")
                    {
                        status = CurrentAction.GetPlayerCard;
                        returnVal = true;
                    }
                   
                    break;
                case CurrentAction.GetPlayerCard:
                    if (information["CurrentAction"].ToString() == ("WaitHumanMove"))
                    {
                          status = CurrentAction.WaitHumanMove;
                          returnVal = true;
                    }
                  
                    break;
                case CurrentAction.WaitHumanMove:
                    if (information["CurrentAction"].ToString() == "HumanUseCard")
                    {
                        if (UseCard((int) information["ID"]))
                        {
                            Winner = CheckPlayerParams();
                            if (Winner.Length > 0)
                            {
                                status = CurrentAction.EndGame;
                            }
                            else if (status != CurrentAction.GetPlayerCard)
                                status = CurrentAction.HumanUseCard;
                        }

                        returnVal = true;
                    }
                    else if (information["CurrentAction"].ToString() == "PassStroke")
                    {
                        if (PassMove((int) information["ID"]))
                        {
                             status = CurrentAction.PassStroke;
                        }

                        returnVal = true;
                    }
                    break;
                case CurrentAction.PassStroke:
                    if (information["CurrentAction"].ToString() == "AnimateHumanMove")
                    {
                        if (players[currentPlayer].type == TypePlayer.Human)
                        {
                            status = CurrentAction.UpdateStatHuman;
                        }
                        else
                        {
                            status = CurrentAction.UpdateStatAI;
                        }
                        UpdateStatistic();

                        returnVal = true;
                    }
                    
                    break;
                case CurrentAction.HumanUseCard:
                    returnVal = true;
                    break;
                case CurrentAction.HumanCanPlayAgain:
                    returnVal = true;
                    break;
                case CurrentAction.AnimateHumanMove:
                    returnVal = true;
                    break;
                case CurrentAction.UpdateStatHuman:
                    if (information["CurrentAction"].ToString() == "EndHumanMove")
                    {
                        Winner = CheckPlayerParams();
                        if (Winner.Length > 0)
                        {
                            status = CurrentAction.EndGame;
                        }
                        else
                        {
                            status = CurrentAction.EndHumanMove;
                            SendGameNotification(information);
                        }

                        returnVal = true;
                    }
                    break;
                case CurrentAction.UpdateStatAI:
                    if (information["CurrentAction"].ToString() == "EndAIMove")
                    {
                        Winner = CheckPlayerParams();
                        if (Winner.Length > 0)
                        {
                            status = CurrentAction.EndGame;
                        }
                        else
                        {
                            status = CurrentAction.EndAIMove;
                            SendGameNotification(information);
                        }

                        returnVal = true;
                    }
                    break;
                case CurrentAction.EndHumanMove:
                        currentPlayer = currentPlayer == 1 ? 0 : 1;
                        if (players[currentPlayer].type == TypePlayer.AI)
                        {
                            MakeMoveAI();
                            status = CurrentAction.AIUseCardAnimation;
                        }
                    returnVal = true;
                    break;
                case CurrentAction.AIMoveIsAnimated:
                    if (information["CurrentAction"].ToString() == "AIMoveIsAnimated")
                    {
                        UpdateStatistic();
                        status = CurrentAction.UpdateStatAI;
                        returnVal = true;
                    }
                    break;
                case CurrentAction.AIUseCardAnimation:
                    if (information["CurrentAction"].ToString() == "AIMoveIsAnimated")
                    {
                        status = CurrentAction.AIMoveIsAnimated;
                        SendGameNotification(information);
                        returnVal = true;
                    }
                    break;
                case CurrentAction.EndAIMove:
                    currentPlayer = currentPlayer == 1 ? 0 : 1;
                    status = CurrentAction.WaitHumanMove;
                    returnVal = true;
                    break;
                case CurrentAction.EndGame:
                    if (information["CurrentAction"].ToString() == "StartGame")
                    {
                       
                        status = CurrentAction.None;
                        SendGameNotification(information);
                        returnVal = true;
                    }
                    break;
                default:
                    log.Error("Cтатус " + status + " не описан в коде");
                    returnVal = false;
                    break;
            }

            return returnVal;
        }
        public T ConvertObjToEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
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
                return players[i].Statistic;
            }
            else
            {
                return GenerateDefault();
            }
        }


        public void AddPlayer(TypePlayer tp, string name)
        {

            if (status == CurrentAction.StartGame)
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

            newPlayer.Statistic = GenerateDefault();
            players.Add(newPlayer);
        
        }


        private void StartGame()
        {
            if (players.Count == 0)
            {
                log.Error("Добавьте игроков в игру. Сейчас их " + players.Count);
                return;
            }

            if (!isGameEnded())
            {
                log.Info("Игра еще не закончилась");
                return;
            }

            Random rnd = new Random();
            currentPlayer =  rnd.Next(0, 2);
            log.Info("Game is started. CurrentPlayer: " + currentPlayer);
        }


        /// <summary>
        /// Генерация стандартных значений для игрока:
        /// стена, башня, шахты, ресурсы
        /// </summary>
        /// <returns></returns>
        private Dictionary<Specifications, int> GenerateDefault()
        {
            Dictionary<Specifications, int> returnVal = new Dictionary<Specifications, int>();
            returnVal.Add(Specifications.PlayerWall, 5);
            returnVal.Add(Specifications.PlayerTower, 10);

            returnVal.Add(Specifications.PlayerMenagerie, 1);
            returnVal.Add(Specifications.PlayerColliery, 1);
            returnVal.Add(Specifications.PlayerDiamondMines, 1);

            returnVal.Add(Specifications.PlayerRocks, 5);
            returnVal.Add(Specifications.PlayerDiamonds, 5);
            returnVal.Add(Specifications.PlayerAnimals, 5);
            return returnVal;
        }


        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <returns>если карту не удалось использовать возвращается false</returns>
        private bool UseCard(int id)
        {
            if (isGameEnded())
            {
                log.Info("Игра уже закончилась");
                return false;
            }

            int index;
            List<CardParams> costCard = GetCardById(id, out index);


            if (IsCanUseCard(costCard))
            {
                log.Info("Player: " + players[currentPlayer].playerName + " use card: " + players[currentPlayer].Cards[index].name);
                ApplyCardParamsToPlayer(players[currentPlayer].Cards[index].cardParams);

                usedCards.Add(players[currentPlayer].Cards[index]);
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

        private bool isGameEnded()
        {
            if (status == CurrentAction.None || status == CurrentAction.EndGame)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Получает карту из стека карт
        /// </summary>
        /// <returns></returns>
        public List<Card> GetCard()
        {
            if (QCard.Count == 0)
            {


                string cardFromServer = host.GetRandomCard();

                List<Card> newParametrs = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);

                if (newParametrs.Count == 0)
                    return null;

                foreach (var item in newParametrs)
                {
                    if (QCard.Count == MaxCard)
                    {
                        break;
                    }
                    QCard.Enqueue(item);

                }
            }

            List<Card> returnVal = new List<Card>();

            while (returnVal.Count < MaxCard)
            {
                var newCard = QCard.Dequeue();
                newCard.description = ParseDescription.Parse(newCard.description);
                returnVal.Add(newCard);
            }
            

            players[currentPlayer].Cards.AddRange(returnVal);
            
            return returnVal;
        }

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

            bool returnVal = false;
            foreach (var item in costCard)
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

            if (!returnVal)
                log.Error("У карты не указана стоимость");
            return returnVal;
        }

        private bool PassMove(int id)
        {
            if (isGameEnded())
            {
                log.Info("Игра уже закончилась");
                return false;
            }

            int index = players[currentPlayer].Cards.FindIndex(x => x.id == id);

            try
            {
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
            PlusValue(Specifications.PlayerDiamonds, players[currentPlayer].Statistic[Specifications.PlayerDiamondMines],
                currentPlayer);
            PlusValue(Specifications.PlayerAnimals, players[currentPlayer].Statistic[Specifications.PlayerMenagerie],
                currentPlayer);
            PlusValue(Specifications.PlayerRocks, players[currentPlayer].Statistic[Specifications.PlayerColliery], currentPlayer);
        }

        private void MakeMoveAI()
        {
            log.Info("Ход компьютера");

            GetCard();

            foreach (var item in players[currentPlayer].Cards)
            {
                if (UseCard(item.id))
                {
                    AIUsedCard.Enqueue(item.id);
                    GetCard();
                    break;
                }
            }

          
         /*   while (status != CurrentAction.None)
            {
                var result = EndMove();

                if (result == CurrentAction.GetPlayerCard)
                {
                    GetCard();
                }

            }*/

          

           
            log.Info("Ход компьютера закончился.");
        }


        public List<Card> GetAIUsedCard()
        {
             List<Card> returnVal = new List<Card>();
            while (AIUsedCard.Count > 0)
            {
                int id = AIUsedCard.Dequeue();

                var card = usedCards.Where(x=>x.id == id).FirstOrDefault();

                if (card != null)
                    returnVal.Add(card);

            }

            return returnVal;
        }

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
                            PlusValue(item.key, item.value, currentPlayer);
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
                            MinusValue(Specifications.PlayerDiamonds, item.value);
                            break;
                        case Specifications.CostAnimals:
                            MinusValue(Specifications.PlayerAnimals, item.value);
                            break;
                        case Specifications.CostRocks:
                            MinusValue(Specifications.PlayerRocks, item.value);
                            break;
                        case Specifications.EnemyDirectDamage:
                            int Ememyindex = currentPlayer == 1 ? 0 : 1;
                            ApplyDirectDamage(item, Ememyindex);
                            break;
                        case Specifications.PlayerDirectDamage:
                            ApplyDirectDamage(item, currentPlayer);
                            break;
                        case Specifications.GetCard:
                            status = CurrentAction.GetPlayerCard;
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
            int tempVal = players[player].Statistic[Specifications.PlayerWall] + item.value;

            if (tempVal < 0)
            {
                PlusValue(Specifications.PlayerWall,
                    - players[player].Statistic[Specifications.PlayerWall], player);

                PlusValue(Specifications.PlayerTower, tempVal, player);
            }
            else
            {
                PlusValue(Specifications.PlayerWall, item.value, player);
            }
        }

        private void MinusValue(Specifications spec, int value)
        {
            if (players[currentPlayer].Statistic[spec] - value <= 0)
            {
                players[currentPlayer].Statistic[spec] = 0;
            }
            else
            {
                players[currentPlayer].Statistic[spec] -= value;
            }

        }

        private void PlusValue(Specifications specifications, int value, int index)
        {
            int minValue = 0;
            if (specifications == Specifications.PlayerDiamondMines || specifications == Specifications.PlayerColliery ||
                specifications == Specifications.PlayerMenagerie)
            {
                minValue = 1;
            }

            if (players[index].Statistic[specifications] + value <= minValue)
            {
                players[index].Statistic[specifications] = minValue;
            }
            else
            {
                players[index].Statistic[specifications] += value;
            }
        }

        /// <summary>
        /// Метод для определения имени победителя
        /// </summary>
 

        public string Winner { get; set; }

        private string CheckPlayerParams()
        {
            string returnVal = string.Empty;

            for (int i = 0; i < players.Count; i ++)
            {
                int Ememyindex = i == 1 ? 0 : 1;

                if (IsPlayerWin(players[i].Statistic) || IsPlayerLose(players[Ememyindex].Statistic))
                {
                    returnVal = players[i].playerName;
                    break;
                }
            }
            return returnVal;
        }

        private bool IsPlayerWin(Dictionary<Specifications, int> playerStatistic)
        {
            foreach (var item in WinParams)
            {
                if (playerStatistic[item.Key] >= item.Value)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPlayerLose(Dictionary<Specifications, int> playerStatistic)
        {
            foreach (var item in LoseParams)
            {
                if (playerStatistic[item.Key] <= item.Value)
                {
                    return true;
                }
            }

            return false;
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
            PlusValue(resutl, item.value, index);


            //  log.Info("playerName:" + playerName + ": " + Statistic[resutl]);

        }
    }
}
