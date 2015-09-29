using System.Collections.Generic;
using Arcomage.Entity;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Tests.MoqStartParams
{
    internal class TestStartParams : IStartParams
    {

        public int MaxPlayerCard { get; set; }
        public Dictionary<Attributes, int> DefaultParams { get; set; }

        public TestStartParams(int countGenerate = 0)
        {
            MaxPlayerCard = 20;
  
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
