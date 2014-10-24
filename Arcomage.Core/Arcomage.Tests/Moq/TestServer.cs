using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

namespace Arcomage.Tests.Moq
{
    class TestServer : IArcoServer
    {
        public string GetRandomCard()
        {
            List<Card> returnVal = new List<Card>();

            var paramsM = new List<CardParams>();
            paramsM.Add(new CardParams() {key = Specifications.EnemyTower, value = -50});
            paramsM.Add(new CardParams() { key = Specifications.CostAnimals, value = 0 });

            returnVal.Add(new Card()
            {
                id = 1,
                cardParams = paramsM
            });






            return JsonConvert.SerializeObject(returnVal);
        }
    }
}
