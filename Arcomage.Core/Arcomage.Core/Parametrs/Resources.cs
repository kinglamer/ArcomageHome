

using Arcomage.BL;

namespace Arcomage.Core.Parametrs
{
    /// <summary>
    /// Количество ресурсов определенного вида
    /// </summary>
    public class Resources : IResources
    {
        /// <summary>
        /// Брилианты
        /// </summary>
        public int Diamonds { get; set; }

        /// <summary>
        /// Звери
        /// </summary>
        public int Animals { get; set; }

        /// <summary>
        /// Камени
        /// </summary>
        public int Rocks { get; set; }
    }
}
