using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card98 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //if Wall > Enemy Wall 3 Damage else 2 Damage 
            int damage = 2;
            if (playerUsed.PlayerParams[Attributes.Wall] > enemy.PlayerParams[Attributes.Wall])
            {
                damage = 3;
            }

            int remaingDamage = enemy.PlayerParams[Attributes.Wall] - damage;
            if (remaingDamage < 0)
            {
                enemy.PlayerParams[Attributes.Wall] = 0;

                enemy.PlayerParams[Attributes.Tower] += remaingDamage;
                if (enemy.PlayerParams[Attributes.Tower] < 0)
                    enemy.PlayerParams[Attributes.Tower] = 0;
            }
            else
            {
                enemy.PlayerParams[Attributes.Wall] -= damage;
            }

          
           

        }
    }
}
