using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card48 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);
            // All players magic equals the highest player's magic 
            
            if (playerUsed.PlayerParams[Attributes.DiamondMines] < enemy.PlayerParams[Attributes.DiamondMines])
                playerUsed.PlayerParams[Attributes.DiamondMines] = enemy.PlayerParams[Attributes.DiamondMines];
            else
            {
                enemy.PlayerParams[Attributes.DiamondMines] = playerUsed.PlayerParams[Attributes.DiamondMines];
            }

        }
    }
}
