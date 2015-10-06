using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Core.Interfaces;
using Arcomage.Entity;

namespace Arcomage.Core
{
    internal static class GameControllerHelper
    {
        public static Dictionary<Attributes, int> GetWinParams()
        {
            Dictionary<Attributes, int> WinParams = new Dictionary<Attributes, int>();
            WinParams.Add(Attributes.Tower, 50);
            WinParams.Add(Attributes.Animals, 150);
            WinParams.Add(Attributes.Rocks, 150);
            WinParams.Add(Attributes.Diamonds, 150);

            return WinParams;
        }

        public static Dictionary<Attributes, int> GetLoseParams()
        {
            Dictionary<Attributes, int> LoseParams = new Dictionary<Attributes, int>();
            LoseParams.Add(Attributes.Tower, 0);

            return LoseParams;
        }

        public static T ConvertObjToEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }
      


        /// <summary>
        /// Метод для определения имени победителя
        /// </summary>
        /// <param name="players"></param>
        /// <param name="winParams"></param>
        /// <param name="loseParams"></param>
        public static string CheckPlayerParams(List<Player> players, Dictionary<Attributes, int> winParams, Dictionary<Attributes, int> loseParams)
        {
            string returnVal = String.Empty;

            for (int i = 0; i < players.Count; i ++)
            {
                int Ememyindex = i == 1 ? 0 : 1;

                if (IsPlayerWin(players[i].PlayerParams, winParams) || IsPlayerLose(players[Ememyindex].PlayerParams, loseParams))
                {
                    returnVal = players[i].playerName;
                    break;
                }
            }
            return returnVal;
        }

        private static bool IsPlayerWin(Dictionary<Attributes, int> playerStatistic, Dictionary<Attributes, int> winParams)
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

        private static bool IsPlayerLose(Dictionary<Attributes, int> playerStatistic, Dictionary<Attributes, int> loseParams)
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
