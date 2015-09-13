using System.Collections.Generic;
using Arcomage.Entity;

namespace Arcomage.Core.Interfaces
{
    public interface IStartParams
    {
        int MaxPlayerCard { get; set; }


        /// <summary>
        /// Генерация стандартных значений для игрока:
        /// стена, башня, шахты, ресурсы
        /// </summary>
        /// <returns></returns>
        Dictionary<Specifications, int> GenerateDefault();
    }
}
