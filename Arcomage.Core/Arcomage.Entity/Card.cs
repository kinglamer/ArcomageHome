using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;

namespace Arcomage.Entity
{
    public class Card //Standard
    {
        public int id { get; set; }
        public string name { get; set; }

        public string description { get; set; }

        public virtual ICollection<CardParams> cardParams { get; set; }

        public Card()
        {
            cardParams = new Collection<CardParams>();
        }
    }
}
