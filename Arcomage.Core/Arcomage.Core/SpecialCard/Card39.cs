using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;

namespace Arcomage.Core.SpecialCard
{
    class Card39 : Card
    {

        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);
           // throw new NotImplementedException();
            //Draw 1 card Discard 1 card Play again
           // additionaStatus = CurrentAction.PlayerMustDropCard;
            discard = true;
           //playAgain = true;
        }

    }
}
