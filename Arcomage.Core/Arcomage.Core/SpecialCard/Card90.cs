using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;

namespace Arcomage.Core.SpecialCard
{
    class Card90 : Card
    {

        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //if Magic > Enemy Magic 12 Damage else 8 Damage

            int damage = 8;
            if (playerUsed.PlayerParams[Attributes.Diamonds] > enemy.PlayerParams[Attributes.Diamonds])
            {
                damage = 12;
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
