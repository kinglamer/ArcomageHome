﻿
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

            /*    var result = DatabaseHelper.GetCards();

              if (result.Count == 0)
              {
                  return "Empty";
              }
            //  result.Randomize();

           List<Card> returnVal = new List<Card>();

              for (int i = 0; i < 5 || i < result.Count; i++)
              {
                  returnVal.Add(result[i]);
              }*/



            CardTest card = new CardTest();
            card.id = 1;
            card.name = "Test";

            card.Paramses.Add(new CardParamsTest() { key = Specifications.CostAnimals, value = 10});


            return JsonConvert.SerializeObject(card); 

        }

   
    }
}
