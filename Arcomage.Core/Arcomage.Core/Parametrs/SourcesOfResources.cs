
using Arcomage.BL;

namespace Arcomage.Core.Parametrs
{
    /// <summary>
    /// Класс отвечающий за источники ресурсов игрока
    /// </summary>
    public class SourcesOfResources : ISourcesOfResources
    {
        /// <summary>
        /// Брилиантовый прииск
        /// </summary>
        public int DiamondMines { get; set; }

        /// <summary>
        /// Зверинец
        /// </summary>
        public int Menagerie { get; set; }

        /// <summary>
        /// Каменоломня
        /// </summary>
        public int Colliery { get; set; }
    }

}
