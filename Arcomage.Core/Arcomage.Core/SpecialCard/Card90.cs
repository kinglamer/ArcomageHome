using System;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Players;

namespace Arcomage.Core.SpecialCard
{
    class Card90 : Card
    {

        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);

            //if Magic > Enemy Magic 12 Damage else 8 Damage

            int value = playerUsed.PlayerParams[Attributes.Diamonds] > enemy.PlayerParams[Attributes.Diamonds] ? 12 : 8;

            CardAttributes item = new CardAttributes();
            item.attributes = Attributes.DirectDamage;
            item.value = value;

            ApplyDirectDamage(item, enemy);

        }
    }
}
