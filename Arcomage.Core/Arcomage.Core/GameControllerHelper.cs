using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Core.Interfaces;
using Arcomage.Entity;

namespace Arcomage.Core
{
    internal class GameControllerHelper
    {
        public static Dictionary<Specifications, int> GetWinParams()
        {
            Dictionary<Specifications, int> WinParams = new Dictionary<Specifications, int>();
            WinParams.Add(Specifications.PlayerTower, 50);
            WinParams.Add(Specifications.PlayerAnimals, 150);
            WinParams.Add(Specifications.PlayerRocks, 150);
            WinParams.Add(Specifications.PlayerDiamonds, 150);

            return WinParams;
        }

        public static Dictionary<Specifications, int> GetLoseParams()
        {
            Dictionary<Specifications, int> LoseParams = new Dictionary<Specifications, int>();
            LoseParams.Add(Specifications.PlayerTower, 0);

            return LoseParams;
        }

        public static T ConvertObjToEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }


      

        public static void MinusValue(Specifications spec, int value, IPlayer playerParam)
        {
            if (playerParam.Statistic[spec] - value <= 0)
            {
                playerParam.Statistic[spec] = 0;
            }
            else
            {
                playerParam.Statistic[spec] -= value;
            }

        }

        public static void PlusValue(Specifications specifications, int value, IPlayer playerParam)
        {
            int minValue = 0;
            if (specifications == Specifications.PlayerDiamondMines || specifications == Specifications.PlayerColliery ||
                specifications == Specifications.PlayerMenagerie)
            {
                minValue = 1;
            }

            if (playerParam.Statistic[specifications] + value <= minValue)
            {
                playerParam.Statistic[specifications] = minValue;
            }
            else
            {
                playerParam.Statistic[specifications] += value;
            }
        }

        /// <summary>
        /// Метод для определения имени победителя
        /// </summary>
        /// <param name="players"></param>
        /// <param name="winParams"></param>
        /// <param name="loseParams"></param>
        public static string CheckPlayerParams(List<IPlayer> players, Dictionary<Specifications, int> winParams, Dictionary<Specifications, int> loseParams)
        {
            string returnVal = String.Empty;

            for (int i = 0; i < players.Count; i ++)
            {
                int Ememyindex = i == 1 ? 0 : 1;

                if (IsPlayerWin(players[i].Statistic, winParams) || IsPlayerLose(players[Ememyindex].Statistic, loseParams))
                {
                    returnVal = players[i].playerName;
                    break;
                }
            }
            return returnVal;
        }

        private static bool IsPlayerWin(Dictionary<Specifications, int> playerStatistic, Dictionary<Specifications, int> winParams)
        {
            foreach (var item in winParams)
            {
                if (playerStatistic[item.Key] >= item.Value)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsPlayerLose(Dictionary<Specifications, int> playerStatistic, Dictionary<Specifications, int> loseParams)
        {
            foreach (var item in loseParams)
            {
                if (playerStatistic[item.Key] <= item.Value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
