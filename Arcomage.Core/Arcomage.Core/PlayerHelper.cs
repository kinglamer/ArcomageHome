using Arcomage.Core.Parametrs;

namespace Arcomage.Core
{
    public class PlayerHelper
    {

        public readonly int MaxCard;
        public int CountCard = 0;

        public Buildings build = new Buildings();
        public SourcesOfResources sources = new SourcesOfResources();
        public Resources resources = new Resources();


        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public PlayerHelper()
        {
            MaxCard = 5;
            CountCard = 0;
            build.Tower = 10;
            build.Wall = 5;

            resources.Animals = 10;
            resources.Diamonds = 10;
            resources.Rocks = 10;
        }

    }
}
