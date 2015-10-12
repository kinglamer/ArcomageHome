using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card32 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //   +6 Recruits +6 Wall . if dungeon < enemy dungeon +1 Dungeon
            
            if (playerUsed.PlayerParams[Attributes.Menagerie] < enemy.PlayerParams[Attributes.Menagerie])
                playerUsed.PlayerParams[Attributes.Menagerie] += 1;

            playerUsed.PlayerParams[Attributes.Animals] += 6;
            playerUsed.PlayerParams[Attributes.Wall] += 6;
        }
    }
}
