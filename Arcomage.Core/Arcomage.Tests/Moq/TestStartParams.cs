using System.Collections.Generic;
using Arcomage.Entity;

namespace Arcomage.Tests.Moq
{
    internal class TestStartParams
    {

        public static Dictionary<Attributes, int> GetParams(int typeParams = 0)
        {
            var defaultParams = new Dictionary<Attributes, int>();
            switch (typeParams)
            {
                case 0:
                    defaultParams = new Dictionary<Attributes, int>
                    {
                        {Attributes.Wall, 0},
                        {Attributes.Tower, 5},
                        {Attributes.Menagerie, 3},
                        {Attributes.Colliery, 1},
                        {Attributes.DiamondMines, 1},
                        {Attributes.Rocks, 5},
                        {Attributes.Diamonds, 5},
                        {Attributes.Animals, 5}
                    };
                    break;
                case 1:
                    defaultParams = new Dictionary<Attributes, int>
                    {
                        {Attributes.Wall, 5},
                        {Attributes.Tower, 12},
                        {Attributes.Menagerie, 1},
                        {Attributes.Colliery, 3},
                        {Attributes.DiamondMines, 4},
                        {Attributes.Rocks, 5},
                        {Attributes.Diamonds, 5},
                        {Attributes.Animals, 5}
                    };
                    break;
                case 2:
                    defaultParams = new Dictionary<Attributes, int>
                    {
                        {Attributes.Wall, 5},
                        {Attributes.Tower, 10},
                        {Attributes.Menagerie, 1},
                        {Attributes.Colliery, 1},
                        {Attributes.DiamondMines, 1},
                        {Attributes.Rocks, 5},
                        {Attributes.Diamonds, 5},
                        {Attributes.Animals, 5}
                    };
                    break;
                case 3:
                    defaultParams = new Dictionary<Attributes, int>
                    {
                        {Attributes.Wall, 1},
                        {Attributes.Tower, 5},
                        {Attributes.Menagerie, 1},
                        {Attributes.Colliery, 1},
                        {Attributes.DiamondMines, 5},
                        {Attributes.Rocks, 5},
                        {Attributes.Diamonds, 5},
                        {Attributes.Animals, 5}
                    };
                    break;
                case 4:
                    defaultParams = new Dictionary<Attributes, int>
                    {
                        {Attributes.Wall, 0},
                        {Attributes.Tower, 12},
                        {Attributes.Menagerie, 2},
                        {Attributes.Colliery, 3},
                        {Attributes.DiamondMines, 4},
                        {Attributes.Rocks, 5},
                        {Attributes.Diamonds, 5},
                        {Attributes.Animals, 5}
                    };
                    break;
            }
            return defaultParams;

        }
    }

}
