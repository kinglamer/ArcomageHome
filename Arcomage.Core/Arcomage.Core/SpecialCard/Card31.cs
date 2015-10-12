using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card31 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);
            //  Player(s) w/lowest Wall are -1 Dungeon -2 Tower

            if(playerUsed.PlayerParams[Attributes.Wall] != enemy.PlayerParams[Attributes.Wall])
            {
                Player target = playerUsed.PlayerParams[Attributes.Wall] > enemy.PlayerParams[Attributes.Wall] ? enemy : playerUsed;
                target.PlayerParams[Attributes.Tower] -= 2;
                target.PlayerParams[Attributes.Menagerie] -= 1;
            }
 
        }
      
    }
}
