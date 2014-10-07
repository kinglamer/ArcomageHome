using Arcomage.Core;
using Arcomage.Core.Parametrs;
using Arcomage.DAL;
using Arcomage.Server.ServiceHelper;
using Newtonsoft.Json;

namespace Arcomage.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ArcoServer" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ArcoServer.svc or ArcoServer.svc.cs at the Solution Explorer and start debugging.
    public class ArcoServer : IArcoServer
    {
        public string GetRandomCard()
        {
            var parametrs = new CardParametrs();
            parametrs.name = "test";
            if (CardCache.cache.Count == 0)
            {
                GetRandomCardFromDB();
            }
       

            parametrs = CardCache.cache.Dequeue();


            return JsonConvert.SerializeObject(parametrs); ;

        }

        private void GetRandomCardFromDB()
        {
            using (var db = new CardContext())
            {
               var item = new CardParametrs();

                item.name = "test";
                item.playerBuildings.Tower = 5;

                db.ArcomageCards.Add(item);

                CardCache.cache.Enqueue(item);
                db.SaveChanges(); 
            }

        }
    }
}
