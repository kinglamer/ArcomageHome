
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Arcomage.Common;
using Arcomage.DAL;
using Arcomage.Entity;
using Newtonsoft.Json;

namespace Arcomage.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ArcoServer" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ArcoServer.svc or ArcoServer.svc.cs at the Solution Explorer and start debugging.
    public class ArcoServer : IArcoServer
    {
        public string GetRandomCard()
        {

              var result = DatabaseHelper.GetCardsForSeriz();

              if (result.Count == 0)
              {
                  return "Empty";
              }

              if (result.Count > 0)
                    result.Randomize();

              List<Card> returnVal = new List<Card>();

              for (int i = 0; i < 5 || i < result.Count; i++)
              {
                  returnVal.Add(result[i]);
              }





            return JsonConvert.SerializeObject(result); 

        }

   
    }
}
