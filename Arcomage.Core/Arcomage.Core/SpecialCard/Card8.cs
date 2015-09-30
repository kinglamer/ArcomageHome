using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card8 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //If quarry < enemy quarry, quarry = enemy quarry
            
            if (playerUsed.PlayerParams[Attributes.Colliery] < enemy.PlayerParams[Attributes.Colliery])
                playerUsed.PlayerParams[Attributes.Colliery] = enemy.PlayerParams[Attributes.Colliery];

        }
    }
}
