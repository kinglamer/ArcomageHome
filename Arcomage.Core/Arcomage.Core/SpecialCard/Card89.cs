using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card89 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //if Enemy Wall > 0 10 Damage else 7 Damage

            int value = enemy.PlayerParams[Attributes.Wall] > 0 ? 10 : 7;

            CardAttributes item = new CardAttributes();
            item.attributes = Attributes.DirectDamage;
            item.value = value;

            ApplyDirectDamage(item, enemy);

        }
    }
}
