using Newtonsoft.Json;

namespace Arcomage.Entity.Cards
{
    public class CardParams 
    {
        public int id { get; set; }

        public Specifications key { get; set; }
        public int value { get; set; }

        [JsonIgnore]
        public Card card { get; set; }

        public CardParams()
        {
            card = new Card();
        }
    }
}