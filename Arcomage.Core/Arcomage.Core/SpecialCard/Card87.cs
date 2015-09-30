using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card87 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            // if Enemy Wall = 0 10 Damage else 6 Damage

            if (enemy.PlayerParams[Attributes.Wall] == 0)
            {
                enemy.PlayerParams[Attributes.Tower] -= 10;
                if (enemy.PlayerParams[Attributes.Tower] < 0)
                    enemy.PlayerParams[Attributes.Tower] = 0;
            }
            else
            {
                int remaingDamage = enemy.PlayerParams[Attributes.Wall] - 6;
                if (remaingDamage < 0)
                    enemy.PlayerParams[Attributes.Wall] = 0;

                enemy.PlayerParams[Attributes.Tower] += remaingDamage;
                if (enemy.PlayerParams[Attributes.Tower] < 0)
                    enemy.PlayerParams[Attributes.Tower] = 0;
            }

        }
    }
}
