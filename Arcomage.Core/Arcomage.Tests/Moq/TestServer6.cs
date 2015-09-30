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
    class TestServer6 : IArcoServer
    {
        public string GetRandomCard()
        {
            List<Card> returnVal = new List<Card>();

            var cardParams = new CardParams() {key = Specifications.CostAnimals, value = 0};

            returnVal.Add(new Card()
            {
                id = 5,
                name = "Mother Lode",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 8,
                name = "Copping the Tech",
                cardParams = new List<CardParams>() { cardParams }
            });
         

            returnVal.Add(new Card()
            {
                id = 12,
                name = "Foundations",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 73,
                name = "Elven Scout",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 1,
                name = "PASS MF",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 31,
                name = "Flood Water",
                cardParams = new List<CardParams>() { cardParams }
            });
            
            returnVal.Add(new Card()
            {
                id = 32,
                name = "Barracks",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 34,
                name = "Shift",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 39,
                name = "Prism",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 40,
                name = "Lodestone",
                cardParams = new List<CardParams>() { cardParams }
            });


            returnVal.Add(new Card()
            {
                id = 48,
                name = "Parity",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 64,
                name = "Bag of Baubles",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 67,
                name = "Lighting Shard",
                cardParams = new List<CardParams>() { cardParams }
            });
            

            returnVal.Add(new Card()
            {
                id = 87,
                name = "Spizzer",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 89,
                name = "Corrosion Cloud",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 90,
                name = "Unicorn",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 91,
                name = "Elven Archer",
                cardParams = new List<CardParams>() { cardParams }
            });

            returnVal.Add(new Card()
            {
                id = 98,
                name = "Spearman",
                cardParams = new List<CardParams>() { cardParams }
            });


            foreach (var item in returnVal)
            {
                item.Init();
            }



            return JsonConvert.SerializeObject(returnVal);
        }
    }
}
