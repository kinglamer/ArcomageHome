using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Core.Interfaces;
using Arcomage.Entity;

namespace Arcomage.Tests.MoqStartParams
{
    internal class TestStartParams : IStartParams
    {

        private int countGenerate;
        public int MaxPlayerCard { get; set; }
        public Dictionary<Attributes, int> DefaultParams { get; set; }

        public TestStartParams()
        {
            MaxPlayerCard = 20;
            countGenerate = 0;

            DefaultParams = new Dictionary<Attributes, int>();
            if (countGenerate == 0)
            {

                DefaultParams.Add(Attributes.Wall, 0);
                DefaultParams.Add(Attributes.Tower, 5);

                DefaultParams.Add(Attributes.Menagerie, 3);
                DefaultParams.Add(Attributes.Colliery, 1);
                DefaultParams.Add(Attributes.DiamondMines, 1);

                DefaultParams.Add(Attributes.Rocks, 5);
                DefaultParams.Add(Attributes.Diamonds, 5);
                DefaultParams.Add(Attributes.Animals, 5);
                countGenerate++;
            }
            else
            {
                DefaultParams.Add(Attributes.Wall, 5);
                DefaultParams.Add(Attributes.Tower, 12);

                DefaultParams.Add(Attributes.Menagerie, 1);
                DefaultParams.Add(Attributes.Colliery, 3);
                DefaultParams.Add(Attributes.DiamondMines, 4);

                DefaultParams.Add(Attributes.Rocks, 5);
                DefaultParams.Add(Attributes.Diamonds, 5);
                DefaultParams.Add(Attributes.Animals, 5);
            }

        }
    }






}
