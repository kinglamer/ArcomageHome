using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arcomage.Entity;

namespace Arcomage.Server
{
    public class CardTest
    {
        public int id { get; set; }
        public string name { get; set; }

        public List<CardParamsTest> Paramses { get; set; } 
    }

     public class CardParamsTest //student
    {
       
     

        public Specifications key { get; set; }
        public int value { get; set; }

        //  public virtual List<Card> cards { get; set; }

      
    }
}