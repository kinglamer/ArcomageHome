using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
        UpdateStat, //Обновление статистики игроков
        EndHumanMove, //Завершение хода игрока
        AIMoveAnimation, //Анимация стола противника
        AIUseCardAnimation, //Анимация использование хода противника
        EndAIMove, //Завершение хода противника
        EndGame //Завершение игры
    }

   

    public class GameController
    {
        public readonly int MaxCard;
        private List<IPlayer> players { get; set; }
        private int currentPlayer { get; set; }

        private CurrentAction status  { get; set; }

        protected readonly ILog log;
        private readonly Dictionary<Specifications, int> WinParams;
        private readonly Dictionary<Specifications, int> LoseParams;
        private const string url = "http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        public CurrentAction currentAction { get; private set; }
      
        private IArcoServer host;

        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private Queue<Card> QCard = new Queue<Card>();


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

        public bool GameNotification(Dictionary<string, object> information)
        {
            if (information == null || information.Count == 0 )
            {
                log.Error("Нет информации о событие");
                return false;
            }

            switch (status)
            {
                case CurrentAction.None:
                    log.Info("Для статуса " + CurrentAction.None + " нет уведомлений");
                    return true;
                case CurrentAction.StartGame:
                    break;
                case CurrentAction.GetPlayerCard:
                    break;
                case CurrentAction.WaitHumanMove:
                    break;
                case CurrentAction.HumanUseCard:
                    break;
                case CurrentAction.HumanCanPlayAgain:
                    break;
                case CurrentAction.AnimateHumanMove:
                    break;
                case CurrentAction.UpdateStat:
                    break;
                case CurrentAction.EndHumanMove:
                    break;
                case CurrentAction.AIMoveAnimation:
                    break;
                case CurrentAction.AIUseCardAnimation:
                    break;
                case CurrentAction.EndAIMove:
                    break;
                case CurrentAction.EndGame:
                    break;
                default:
                    log.Error("Cтатус " + status + " не описан в коде");
                    break;
            }

            return true;
        }

        public Dictionary<Specifications, int> GetParamsPlayer(SelectPlayer selectPlayer = SelectPlayer.None)
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

            if (tp == TypePlayer.AI)
            {
                 currentPlayer = players.Count - 1;
                  GetCard();
            }
        }


        public void StartGame()
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


            status = CurrentAction.StartGame;

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
        public bool UseCard(int id)
        {
            if (isGameEnded())
            {
                log.Info("Игра уже закончилась");
                return false;
            }

            int index = players[currentPlayer].Cards.FindIndex(x => x.id == id);

            var costCard = players[currentPlayer].Cards[index].cardParams.Where(x => x.key == Specifications.CostDiamonds ||
                                                                  x.key == Specifications.CostAnimals ||
                                                                  x.key == Specifications.CostRocks).ToList();


            if (isCanUsed(costCard))
            {
                log.Info("Player: " + players[currentPlayer].playerName + " use card: " + players[currentPlayer].Cards[index].name);
                ApplyCardParamsToPlayer(players[currentPlayer].Cards[index].cardParams);

                players[currentPlayer].Cards.RemoveAt(index);
                return true;
            }
            else
            {
                log.Info("Player: " + players[currentPlayer].playerName + " can't use card: " + players[currentPlayer].Cards[index].name);
            }


            return false;
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

            while (players[currentPlayer].Cards.Count <= MaxCard)
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
        private bool isCanUsed(ICollection<CardParams> cardParams)
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

            if (!returnVal)
                log.Error("У карты не указана стоимость");
            return returnVal;
        }

        public bool PassMove(int id)
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

        /// <summary>
        /// Расчет прироста ресурсов игрока от его шахт. А так же выполнения хода за компьютер
        /// </summary>
        public CurrentAction EndMove()
        {
            if (status == CurrentAction.GetPlayerCard)
            {
                return status;
            }

            if (WhoWin().Length > 0)
            {
                log.Info("EndMove - уже есть победитель.");
                status = CurrentAction.EndGame;
                return status;
            }

            PlusValue(Specifications.PlayerDiamonds, players[currentPlayer].Statistic[Specifications.PlayerDiamondMines], currentPlayer);
            PlusValue(Specifications.PlayerAnimals, players[currentPlayer].Statistic[Specifications.PlayerMenagerie], currentPlayer);
            PlusValue(Specifications.PlayerRocks, players[currentPlayer].Statistic[Specifications.PlayerColliery], currentPlayer);

            currentPlayer = currentPlayer == 1 ? 0 : 1;

            if (players[currentPlayer].type == TypePlayer.AI)
            {
                MakeMoveAI();
            }

            return status;
        }

        private void MakeMoveAI()
        {
            log.Info("Ход компьютера");

            foreach (var item in players[currentPlayer].Cards)
            {
                if (UseCard(item.id))
                {
                    GetCard();
                    break;
                }
            }

          
            while (status != CurrentAction.None)
            {
                var result = EndMove();

                if (result == CurrentAction.GetPlayerCard)
                {
                    GetCard();
                }

            }

            currentPlayer = currentPlayer == 1 ? 0 : 1;
            log.Info("Ход компьютера закончился.");
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
        /// <returns></returns>
        public string WhoWin()
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
