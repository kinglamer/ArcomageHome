using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card91 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            // if Wall > Enemy Wall 6 Damage to Enemy Tower else 6 Damage 

            if (playerUsed.PlayerParams[Attributes.Wall] > enemy.PlayerParams[Attributes.Wall])
            {
                enemy.PlayerParams[Attributes.Tower] -= 6;
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
