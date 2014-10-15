using System.Collections.Generic;
using System.ServiceModel;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;

namespace Arcomage.Core
{
    public class PlayerHelper
    {

        public readonly int MaxCard;
        public int CountCard = 0;

        private Dictionary<Specifications, int> playerStatistic { get; set; }


        private const string url = "http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        private Queue<Card> QCard = new Queue<Card>();

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public PlayerHelper()
        {
            MaxCard = 5;
            CountCard = 0;
            playerStatistic = GenerateDefault(); // new Dictionary<Specifications, int>();
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
        /// Генерация стандартных значений для игрока:
        /// стена, башня, шахты, ресурсы
        /// </summary>
        /// <returns></returns>
        private Dictionary<Specifications, int> GenerateDefault()
        {
            Dictionary<Specifications, int> returnVal = new Dictionary<Specifications, int>();
            returnVal.Add(Specifications.PlayerWall,5);
            returnVal.Add(Specifications.PlayerTower, 10);

            returnVal.Add(Specifications.PlayerMenagerie, 5);
            returnVal.Add(Specifications.PlayerColliery, 5);
            returnVal.Add(Specifications.PlayerDiamondMines, 5);

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

            return QCard.Dequeue();
        }

    }
}
