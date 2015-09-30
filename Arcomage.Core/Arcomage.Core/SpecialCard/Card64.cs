using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card64 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            // if Tower < Enemy Tower +2 Tower else +1 Tower

            int value = 1;
            if (playerUsed.PlayerParams[Attributes.Tower] < enemy.PlayerParams[Attributes.Tower])
                value = 2;

            playerUsed.PlayerParams[Attributes.Tower] += value;
        }
    }
}
