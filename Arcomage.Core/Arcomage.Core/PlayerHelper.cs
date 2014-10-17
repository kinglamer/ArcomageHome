using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Arcomage.Common;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;

namespace Arcomage.Core
{
    public class PlayerHelper
    {
        protected readonly ILog log;
        public readonly int MaxCard;
        public int CountCard { get; private set; }

        private string playerName { get; set; }
        private PlayerHelper enemy { get; set; }

        private Dictionary<Specifications, int> playerStatistic { get; set; }


        private const string url = "http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private Queue<Card> QCard = new Queue<Card>();

        /// <summary>
        /// Список текущих карт игрока
        /// </summary>
        private List<Card> playCards = new List<Card>();
        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public PlayerHelper(ILog _log, string _playerName)
        {

            playerName = _playerName;

                log = _log;
            
                MaxCard = 5;
                CountCard = 0;
                
                playerStatistic = GenerateDefault(); // new Dictionary<Specifications, int>();
        }



        public void SetTheEnemy(PlayerHelper newEnemy)
        {
            if (enemy == null)
            enemy = newEnemy;
        }

        public int GetPlayerStat(Specifications sp)
        {
            if (playerStatistic.ContainsKey(sp))
                return playerStatistic[sp];
            else
            {
                return -1;
            }
        }


        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <returns>если карту не удалось использовать возвращается false</returns>
        public bool UseCard(int id)
        {
            int index = playCards.FindIndex(x => x.id == id);

            var costCard = playCards[index].cardParams.Where(x => x.key == Specifications.CostDiamonds ||
                                                                  x.key == Specifications.CostAnimals ||
                                                                  x.key == Specifications.CostRocks).ToList();


            if (isCanUsed(costCard))
            {
                log.Info("Player: " + playerName + " use card: " + playCards[index].name);
                ApplyCardParamsToPlayer(playCards[index].cardParams);

                playCards.RemoveAt(index);
                CountCard--;
                return true;
            }

            return false;
        }

        public Card ReturnCard(int id)
        {
            return playCards.First(x => x.id == id);
        }


        /// <summary>
        /// Расчет прироста ресурсов игрока от его шахт
        /// </summary>
        public void CalculateMove()
        {
            PlusValue(Specifications.PlayerDiamonds, playerStatistic[Specifications.PlayerDiamondMines]);
            PlusValue(Specifications.PlayerAnimals, playerStatistic[Specifications.PlayerMenagerie]);
            PlusValue(Specifications.PlayerRocks, playerStatistic[Specifications.PlayerColliery]);
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
                       // log.Info(playerStatistic[Specifications.PlayerDiamonds] + " > " + item.value);
                        if (playerStatistic[Specifications.PlayerDiamonds] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                    case Specifications.CostAnimals:
                       // log.Info(playerStatistic[Specifications.PlayerAnimals] + " > " + item.value);
                        if (playerStatistic[Specifications.PlayerAnimals] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                    case Specifications.CostRocks:
                       // log.Info(playerStatistic[Specifications.PlayerRocks] + " > " + item.value);
                        if (playerStatistic[Specifications.PlayerRocks] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                }
            }

           // log.Info("isCanUsed: " + returnVal);
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
                            PlusValue(item.key, item.value);
                            break;
                        case Specifications.EnemyTower:
                        case Specifications.EnemyWall:
                        case Specifications.EnemyDiamondMines:
                        case Specifications.EnemyMenagerie:
                        case Specifications.EnemyColliery:
                        case Specifications.EnemyDiamonds:
                        case Specifications.EnemyAnimals:
                        case Specifications.EnemyRocks:
                            enemy.ApplyCardParamFromEnemy(item);
                            break;
                        case Specifications.CostDiamonds:
                            ChangePlayerResourses(Specifications.PlayerDiamonds, item.value);
                            break;
                        case Specifications.CostAnimals:
                            ChangePlayerResourses(Specifications.PlayerAnimals, item.value);
                            break;
                        case Specifications.CostRocks:
                            ChangePlayerResourses(Specifications.PlayerRocks, item.value);
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

        private void ChangePlayerResourses(Specifications spec, int value)
        {
            if (playerStatistic[spec] - value <= 0)
            {
                playerStatistic[spec] = 0;
            }
            else
            {
                playerStatistic[spec] -= value;
            }

        }

        private void PlusValue(Specifications specifications, int value)
        {
            if (playerStatistic[specifications] + value <= 0)
            {
                playerStatistic[specifications] = 0;
            }
            else
            {
                playerStatistic[specifications] += value;
            }
        }

        /// <summary>
        /// Метод для изменения параметров противника
        /// </summary>
        /// <param name="item"></param>
        public void ApplyCardParamFromEnemy(CardParams item)
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

           // log.Info("playerName:" + playerName + ": " + playerStatistic[resutl]);

            PlusValue(resutl, item.value);
            
            
          //  log.Info("playerName:" + playerName + ": " + playerStatistic[resutl]);
            
        }

        /// <summary>
        /// Генерация стандартных значений для игрока:
        /// стена, башня, шахты, ресурсы
        /// </summary>
        /// <returns></returns>
        private Dictionary<Specifications, int> GenerateDefault()
        {
            Dictionary<Specifications, int> returnVal = new Dictionary<Specifications, int>();
            returnVal.Add(Specifications.PlayerWall,5);
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
        /// Получает карту из стека карт
        /// </summary>
        /// <returns></returns>
        public Card GetCard()
        {
            if (QCard.Count == 0)
            {
                ArcoServerClient host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url));
                
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

            var returnVal = QCard.Dequeue();
            playCards.Add(returnVal);

            CountCard++;
            return returnVal;
        }

    }
}
