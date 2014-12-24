﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Entity;

namespace Arcomage.Tests.MoqStartParams
{
    class TestStartParams4 : IStartParams
    {
        public TestStartParams4()
        {
            MaxPlayerCard = 102;
        }

        public int MaxPlayerCard { get; set; }



        public Dictionary<Specifications, int> GenerateDefault()
        {
            Dictionary<Specifications, int> returnVal = new Dictionary<Specifications, int>();
           
               
                returnVal.Add(Specifications.PlayerWall, 5);
                returnVal.Add(Specifications.PlayerTower, 10);

                returnVal.Add(Specifications.PlayerMenagerie, 1);
                returnVal.Add(Specifications.PlayerColliery, 1);
                returnVal.Add(Specifications.PlayerDiamondMines, 1);

                returnVal.Add(Specifications.PlayerRocks, 5);
                returnVal.Add(Specifications.PlayerDiamonds, 5);
                returnVal.Add(Specifications.PlayerAnimals, 5);
             
            
          
           
            return returnVal;
        }
    }
}
