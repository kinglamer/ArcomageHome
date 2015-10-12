using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card34 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //  Switch your Wall with enemy Wall

            int playerWall = playerUsed.PlayerParams[Attributes.Wall];
            playerUsed.PlayerParams[Attributes.Wall] = enemy.PlayerParams[Attributes.Wall];
            enemy.PlayerParams[Attributes.Wall] = playerWall;

        }
    }
}
