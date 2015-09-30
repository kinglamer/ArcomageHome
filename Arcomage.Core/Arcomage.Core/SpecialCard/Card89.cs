using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card89 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //if Enemy Wall > 0 10 Damage else 7 Damage
            int damage = 7;
            if (enemy.PlayerParams[Attributes.Wall] > 0)
            {
                damage = 10;
            }

            int remaingDamage = enemy.PlayerParams[Attributes.Wall] - damage;
            if (remaingDamage < 0)
                enemy.PlayerParams[Attributes.Wall] = 0;

            enemy.PlayerParams[Attributes.Tower] += remaingDamage;
            if (enemy.PlayerParams[Attributes.Tower] < 0)
                enemy.PlayerParams[Attributes.Tower] = 0;

        }
    }
}
