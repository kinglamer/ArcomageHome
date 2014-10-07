

using Arcomage.BL;

namespace Arcomage.Core.Parametrs
{
    /// <summary>
    /// Класс отвечающий за строения игрока
    /// </summary>
    public class Buildings : IBuildings
    {
        public int Wall { get; set; }
        public int Tower { get; set; }
    }
}