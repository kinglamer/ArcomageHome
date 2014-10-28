﻿using System;
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

    public class GameController
    {
        public readonly int MaxCard;
        public List<IPlayer> players { get; set; }
        public int currentPlayer { get; set; }



        protected readonly ILog log;
        private readonly Dictionary<Specifications, int> WinParams;
        private readonly Dictionary<Specifications, int> LoseParams;
        private const string url = "http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        private bool isGameEnd { get; set; }
        private IArcoServer host;
        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private Queue<Card> QCard = new Queue<Card>();


        public GameController(ILog _log, IArcoServer server = null)
        {
            MaxCard = 5;
            isGameEnd = true;
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

        public void AddPlayer(TypePlayer tp, string name)
        {
            if (players.Count == 2)
            {
                log.Error("Достигнуто максимальное количество игроков");
                return;
            }

            IPlayer newPlayer;
            switch (tp)
            {
                case TypePlayer.AI:
                    newPlayer = new AI(name);
                    break;
                case TypePlayer.Human:
                    newPlayer = new Player(name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("tp");
            }

            newPlayer.Statistic = GenerateDefault();
            players.Add(newPlayer);
        }


        public void StartGame()
        {
            if (players.Count == 0)
            {
                log.Error("Добавьте игрок в игру. Сейчас их " + players.Count);
                return;
            }

            if (isGameEnd)
            {
                isGameEnd = false;

                Random rnd = new Random();
                currentPlayer = rnd.Next(0, 1);
                
            }
            else
            {
                log.Info("Игра уже итак запущена");
                return;
            }


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
            if (isGameEnd)
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


            return false;
        }

        /// <summary>
        /// Получает карту из стека карт
        /// </summary>
        /// <returns></returns>
        public Card GetCard()
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

            var returnVal = QCard.Dequeue();
            players[currentPlayer].Cards.Add(returnVal);

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
                        // log.Info(Statistic[Specifications.PlayerDiamonds] + " > " + item.value);
                        if (players[currentPlayer].Statistic[Specifications.PlayerDiamonds] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                    case Specifications.CostAnimals:
                        // log.Info(Statistic[Specifications.PlayerAnimals] + " > " + item.value);
                        if (players[currentPlayer].Statistic[Specifications.PlayerAnimals] >= item.value)
                        {
                            returnVal = true;
                        }
                        break;
                    case Specifications.CostRocks:
                        // log.Info(Statistic[Specifications.PlayerRocks] + " > " + item.value);
                        if (players[currentPlayer].Statistic[Specifications.PlayerRocks] >= item.value)
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
        /// Расчет прироста ресурсов игрока от его шахт
        /// </summary>
        public void EndMove()
        {
            PlusValue(Specifications.PlayerDiamonds, players[currentPlayer].Statistic[Specifications.PlayerDiamondMines], currentPlayer);
            PlusValue(Specifications.PlayerAnimals, players[currentPlayer].Statistic[Specifications.PlayerMenagerie], currentPlayer);
            PlusValue(Specifications.PlayerRocks, players[currentPlayer].Statistic[Specifications.PlayerColliery], currentPlayer);

            currentPlayer = currentPlayer == 1 ? 0 : 1;
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
            if (players[index].Statistic[specifications] + value <= 0)
            {
                players[index].Statistic[specifications] = 0;
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
                int Ememyindex = currentPlayer == 1 ? 0 : 1;

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
                // log.Info("item.Key " + item.Key);
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
                // log.Info("item.Key " + item.Key);
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