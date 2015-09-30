using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcomage.Entity;
using Arcomage.Entity.Cards;

namespace Arcomage.Core.SpecialCard
{
    class Card12 : Card
    {
        public override void Apply(Player playerUsed, Player enemy)
        {
            base.Apply(playerUsed, enemy);
            //If player wall = 0,+6 wall,else +3 wall

            int value = playerUsed.PlayerParams[Attributes.Wall];
            int newValue = 3;

            if (value == 0)
                newValue = 6;

            playerUsed.PlayerParams[Attributes.Wall] += newValue;
        }


    }
}
