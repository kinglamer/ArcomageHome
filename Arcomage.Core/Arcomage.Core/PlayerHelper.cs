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
        private const string url = "http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";

        private Queue<Card> QCard = new Queue<Card>();

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public PlayerHelper()
        {
            MaxCard = 5;
            CountCard = 0;
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
