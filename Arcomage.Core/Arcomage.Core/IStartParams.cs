using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core
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
