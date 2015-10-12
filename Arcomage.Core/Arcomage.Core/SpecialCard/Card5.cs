using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card5 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            int value = 1;
            if (playerUsed.PlayerParams[Attributes.Colliery] < enemy.PlayerParams[Attributes.Colliery])
                value = 2;

            playerUsed.PlayerParams[Attributes.Colliery] += value;
        }

    }
}
