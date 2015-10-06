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
            return new Dictionary<Attributes, int>
            {
                {Attributes.Tower, 50},
                {Attributes.Animals, 150},
                {Attributes.Rocks, 150},
                {Attributes.Diamonds, 150}
            };;
        }

        public static Dictionary<Attributes, int> GetLoseParams()
        {
            return new Dictionary<Attributes, int> {{Attributes.Tower, 0}};
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
                int ememyindex = i == 1 ? 0 : 1;

                if (IsPlayerWin(players[i].PlayerParams, winParams) || IsPlayerLose(players[ememyindex].PlayerParams, loseParams))
                {
                    returnVal = players[i].playerName;
                    break;
                }
            }
            return returnVal;
        }

        private static bool IsPlayerWin(Dictionary<Attributes, int> playerStatistic, Dictionary<Attributes, int> winParams)
        {
            return winParams.Any(item => playerStatistic[item.Key] >= item.Value);
        }

        private static bool IsPlayerLose(Dictionary<Attributes, int> playerStatistic, Dictionary<Attributes, int> loseParams)
        {
            return loseParams.Any(item => playerStatistic[item.Key] <= item.Value);
        }
    }
}
