using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Newtonsoft.Json;

namespace Arcomage.Tests.Moq
{
    class TestServer2 : IArcoServer
    {
        public string GetRandomCard()
        {
            List<Card> returnVal = new List<Card>();

            var paramsM6 = new List<CardParams>();
            paramsM6.Add(new CardParams() { key = Specifications.PlayerDiamonds, value = 11 });
            paramsM6.Add(new CardParams() { key = Specifications.CostAnimals, value = 0 });

            returnVal.Add(new Card()
            {
                id = 6,
                name = "Check Change Diamonds",
                cardParams = paramsM6
            });

            foreach (var item in returnVal)
            {
                item.Init();
            }


            return JsonConvert.SerializeObject(returnVal);
        }
    }
}
