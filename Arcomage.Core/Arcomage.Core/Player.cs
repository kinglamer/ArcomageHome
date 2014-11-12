using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Arcomage.Common;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;

namespace Arcomage.Core
{

    internal class Player : IPlayer
    {
       

        public string playerName { get; set; }
        public TypePlayer type { get; set; }

        public Dictionary<Specifications, int> Statistic { get; set; }



        /// <summary>
        /// Список текущих карт игрока
        /// </summary>
        public List<Card> Cards { get; set; }

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public Player(string _playerName, TypePlayer _type)
        {
            
            playerName = _playerName;
            type = _type;
            Cards = new List<Card>();

        }




 

     

    }
}
