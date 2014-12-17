using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Entity;

namespace Arcomage.Tests.MoqStartParams
{
    class TestStartParams3 : IStartParams
    {

        public TestStartParams3()
        {
            MaxPlayerCard = 20;
            countGenerate = 0;
        }

        public int MaxPlayerCard { get; set; }


        private int countGenerate; 

        public Dictionary<Specifications, int> GenerateDefault()
        {
            Dictionary<Specifications, int> returnVal = new Dictionary<Specifications, int>();
            if (countGenerate == 0)
            {
               
                returnVal.Add(Specifications.PlayerWall, 1);
                returnVal.Add(Specifications.PlayerTower, 5);

                returnVal.Add(Specifications.PlayerMenagerie, 1);
                returnVal.Add(Specifications.PlayerColliery, 1);
                returnVal.Add(Specifications.PlayerDiamondMines, 5);

                returnVal.Add(Specifications.PlayerRocks, 5);
                returnVal.Add(Specifications.PlayerDiamonds, 5);
                returnVal.Add(Specifications.PlayerAnimals, 5);
                countGenerate++;
            }
            else
            {
                returnVal.Add(Specifications.PlayerWall, 0);
                returnVal.Add(Specifications.PlayerTower, 12);

                returnVal.Add(Specifications.PlayerMenagerie, 2);
                returnVal.Add(Specifications.PlayerColliery, 3);
                returnVal.Add(Specifications.PlayerDiamondMines, 4);

                returnVal.Add(Specifications.PlayerRocks, 5);
                returnVal.Add(Specifications.PlayerDiamonds, 5);
                returnVal.Add(Specifications.PlayerAnimals, 5);
            }
           
            return returnVal;
        }
    }
}
