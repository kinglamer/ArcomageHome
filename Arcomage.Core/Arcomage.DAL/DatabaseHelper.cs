using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Entity;
using Arcomage.Entity.Cards;

namespace Arcomage.DAL
{
    public class DatabaseHelper
    {
        public static void SaveCard(List<CardParams> cardParam, List<CardParams> result, Card item)
        {
            using (var db = new CardContext())
            {
                cardParam.AddRange(result);

                //  item.cardParams = cardParam;

                db.Cards.Add(item);
                db.SaveChanges();

                db.CardParamses.AddRange(cardParam);
                db.SaveChanges();
            }
        }

        public static List<Card> GetCardsForSeriz()
        {
            using (CardContext db = new CardContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                return db.Cards.Include(x=>x.cardParams).ToList();
            }

        }

        public static List<Card> GetCards()
        {
            List<Card> cards;
            using (var db = new CardContext())
            {
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.LazyLoadingEnabled = true;

                cards = db.Cards.ToList();
            }

            foreach (var item in cards)
            {
                item.Init();
            }

            return cards;
        }

        public static void DeleteCard(string Id)
        {
            using (var db = new CardContext())
            {
                var myCard = new Card {id = Convert.ToInt32(Id)};
                db.Cards.Attach(myCard);

                db.Entry(myCard).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public static void UpdateCard(IDictionary<string, object> newValues)
        {
            using (var db = new CardContext())
            {
                int id = Convert.ToInt32(newValues["id"]);
                Card myCard = db.Cards.FirstOrDefault(x => x.id == id);

                myCard.name = newValues["name"].ToString();
               
                if (newValues["description"] != null)
                myCard.description = newValues["description"].ToString();

              

                foreach (var item in newValues)
                {
                    if (item.Key != "name" && item.Key != "id" && item.Key != "description" && item.Key.Length > 0)
                    {
                        Specifications spec = (Specifications) Enum.Parse(typeof (Specifications), item.Key);


                        if (myCard.cardParams.Any(x => x.key == spec))
                        {
                            if (item.Value != null)
                            {
                                myCard.cardParams.First(x => x.key == spec).value = Convert.ToInt32(item.Value);
                            }
                            else
                            {
                                db.Entry(myCard.cardParams.First(x => x.key == spec)).State = EntityState.Deleted;
                            }
                        }
                        else
                        {
                            if (item.Value != null)
                            {
                                db.CardParamses.Add(new CardParams()
                                {
                                    card = myCard,
                                    key = spec,
                                    value = Convert.ToInt32(item.Value)
                                });
                            }
                        }
                    }
                }

                db.Entry(myCard).State = EntityState.Modified;


                db.SaveChanges();
            }
        }
    }
}
