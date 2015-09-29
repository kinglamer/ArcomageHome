using System.Collections.Generic;

namespace Arcomage.Entity.Interfaces
{
    public interface IStartParams
    {
        int MaxPlayerCard { get; set; }


        /// <summary>
        /// стандартные значений для игрока:
        /// стена, башня, шахты, ресурсы
        /// </summary>
        /// <returns></returns>
        Dictionary<Attributes, int> DefaultParams { get; set; }
    }
}
