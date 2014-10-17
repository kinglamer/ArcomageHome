using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcomage.Core
{
    public class AIHelper
    {
        public static bool MakeMove(PlayerHelper ph)
        {
            //Использует первую подходящую карту
            for(int i =0 ; i < 5 ; i++)
            {
                var card = ph.GetCard();
                bool haveUseCard = ph.UseCard(card.id);

                if (haveUseCard)
                    return true;
                
            }

            return false;

        }
    }
}
