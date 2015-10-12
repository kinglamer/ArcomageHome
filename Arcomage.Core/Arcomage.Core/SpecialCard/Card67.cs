using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card67 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //  if Tower > Enemy Wall 8 Damage to Enemy Tower else 8 Damage

            if (playerUsed.PlayerParams[Attributes.Tower] > enemy.PlayerParams[Attributes.Wall])
            {
                enemy.PlayerParams[Attributes.Tower] -= 8;
                if (enemy.PlayerParams[Attributes.Tower] < 0)
                    enemy.PlayerParams[Attributes.Tower] = 0;
            }
            else
            {
                int remaingDamage = enemy.PlayerParams[Attributes.Wall] - 8;
                if (remaingDamage < 0)
                    enemy.PlayerParams[Attributes.Wall] = 0;

                enemy.PlayerParams[Attributes.Tower] += remaingDamage;
                if (enemy.PlayerParams[Attributes.Tower] < 0)
                    enemy.PlayerParams[Attributes.Tower] = 0;
            }



        }
    }
}
