﻿using System;
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
    class TestServer4 : IArcoServer
    {
        public string GetRandomCard()
        {
            List<Card> returnVal = new List<Card>();

            var paramsM = new List<CardParams>();
            paramsM.Add(new CardParams() { key = Specifications.EnemyTower, value = -50 });
            paramsM.Add(new CardParams() { key = Specifications.CostAnimals, value = 100 });

            returnVal.Add(new Card()
            {
                id = 1,
                name = "Card1",
                cardParams = paramsM
            });
            

            var paramsM2 = new List<CardParams>();
            paramsM2.Add(new CardParams() { key = Specifications.PlayerTower, value = -8 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerWall, value = -4 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerDiamondMines, value = 2 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerMenagerie, value = 3 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerColliery, value = 4 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerDiamonds, value = 11 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerAnimals, value = 12 });
            paramsM2.Add(new CardParams() { key = Specifications.PlayerRocks, value = 13 });

            paramsM2.Add(new CardParams() { key = Specifications.EnemyTower, value = -8 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyWall, value = -4 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyDiamondMines, value = 2 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyMenagerie, value = 3 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyColliery, value = 4 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyDiamonds, value = 11 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyAnimals, value = 12 });
            paramsM2.Add(new CardParams() { key = Specifications.EnemyRocks, value = 13 });

            paramsM2.Add(new CardParams() { key = Specifications.CostAnimals, value = 100 });

            returnVal.Add(new Card()
            {
                id = 2,
                name = "Card2",
                cardParams = paramsM2
            });


            var paramsM3 = new List<CardParams>();
            paramsM3.Add(new CardParams() { key = Specifications.EnemyDirectDamage, value = -50 });
            paramsM3.Add(new CardParams() { key = Specifications.CostAnimals, value = 100 });

            returnVal.Add(new Card()
            {
                id = 3,
                name = "Card3",
                cardParams = paramsM3
            });

            var paramsM4 = new List<CardParams>();
            paramsM4.Add(new CardParams() { key = Specifications.PlayerDirectDamage, value = -50 });
            paramsM4.Add(new CardParams() { key = Specifications.CostAnimals, value = 100 });

            returnVal.Add(new Card()
            {
                id = 4,
                name = "Card4",
                cardParams = paramsM4
            });



            var paramsM5 = new List<CardParams>();
            paramsM5.Add(new CardParams() { key = Specifications.PlayAgain, value = -50 });
            paramsM5.Add(new CardParams() { key = Specifications.CostAnimals, value = 0 });

            returnVal.Add(new Card()
            {
                id = 55,
                name = "CardGetCard",
                cardParams = paramsM5
            });


            var paramsM6 = new List<CardParams>();
            paramsM6.Add(new CardParams() { key = Specifications.PlayerDiamonds, value = 11 });
            paramsM6.Add(new CardParams() { key = Specifications.CostAnimals, value = 0 });

            returnVal.Add(new Card()
            {
                id = 6,
                name = "CardCostAnimalsZero",
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
