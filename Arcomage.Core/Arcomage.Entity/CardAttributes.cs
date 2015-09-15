using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcomage.Entity
{
    public enum Attributes{
        Tower,
        Wall,
        DiamondMines,
        Menagerie,
        Colliery,
        Diamonds,
        Animals,
        Rocks,
        DirectDamage
     }

    public enum Target {
        Player,
        Enemy
       }



    public class CardAttributes
    {
        public Attributes attributes { get; set; }    
        public Target target { get; set; }
        public int value { get; set; }
    }


}
