using System.Collections.Generic;
using Arcomage.Core;

namespace Arcomage.Server.ServiceHelper
{
    public class CardCache
    {
        public static Queue<CardParametrs> cache { get; set; }

        static CardCache()
        {
            cache = new Queue<CardParametrs>();
        }
    }
}