using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card98 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //if Wall > Enemy Wall 3 Damage else 2 Damage 

            int value = playerUsed.PlayerParams[Attributes.Wall] > enemy.PlayerParams[Attributes.Wall] ? 3 : 2;

            CardAttributes item = new CardAttributes();
            item.attributes = Attributes.DirectDamage;
            item.value = value;

            ApplyDirectDamage(item, enemy);
        }
    }
}
