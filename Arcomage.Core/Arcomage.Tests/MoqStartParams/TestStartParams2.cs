﻿using System;
using System.Collections.Generic;

using Arcomage.Entity;
using Arcomage.Entity.Interfaces;

namespace Arcomage.Tests.MoqStartParams
{
    class TestStartParams2 : IStartParams
    {
        public TestStartParams2()
        {
            MaxPlayerCard = 20;

            DefaultParams = new Dictionary<Attributes, int>();


            DefaultParams.Add(Attributes.Wall, 5);
            DefaultParams.Add(Attributes.Tower, 10);

            DefaultParams.Add(Attributes.Menagerie, 1);
            DefaultParams.Add(Attributes.Colliery, 1);
            DefaultParams.Add(Attributes.DiamondMines, 1);

            DefaultParams.Add(Attributes.Rocks, 5);
            DefaultParams.Add(Attributes.Diamonds, 5);
            DefaultParams.Add(Attributes.Animals, 5);
        }

        public int MaxPlayerCard { get; set; }
        public Dictionary<Attributes, int> DefaultParams { get; set; }



       
    }
}
