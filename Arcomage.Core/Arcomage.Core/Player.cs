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

    public class Player : IPlayer
    {
       
        public int CountCard { get; private set; }

        public string playerName { get; set; }

        public Dictionary<Specifications, int> playerStatistic { get; set; }


   



        /// <summary>
        /// Список текущих карт игрока
        /// </summary>
        public List<Card> playCards { get; set; }

        /// <summary>
        /// Установка дефолтных значений
        /// </summary>
        public Player(string _playerName)
        {
            
            playerName = _playerName;

            playCards = new List<Card>();
            CountCard = 0;

        }


        public Card ReturnCard(int id)
        {
            return playCards.First(x => x.id == id);
        }


 

     

    }
}
