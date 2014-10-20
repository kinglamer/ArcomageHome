using System.ComponentModel.DataAnnotations;

namespace Arcomage.Entity
{
    public class CardParams //student
    {
       
        public int id { get; set; }

        public Specifications key { get; set; }
        public int value { get; set; }

        //  public virtual List<Card> cards { get; set; }

       // [JsonIgnore]
        public Card card { get; set; }

        public CardParams()
        {
            card = new Card();
        }
    }
}