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

        public Price price;
        public List<CardAttributes> cardAttributes;
        public bool playAgain = false;

        public Card()
        {
            cardParams = new Collection<CardParams>();
        }


        //TODO: избавиться в дальнейшем от данной переинициализации
        public void Init()
        {
            if (cardParams != null && cardParams.Count > 0 && cardAttributes == null)
            {
                cardAttributes = new List<CardAttributes>();
               
                foreach (var item in cardParams)
                {
                    switch (item.key)
                    {
                        case Specifications.PlayerTower:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Tower, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerWall:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Wall, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerDiamondMines:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.DiamondMines, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerMenagerie:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Menagerie, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerColliery:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Colliery, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerDiamonds:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Diamonds, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerAnimals:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Animals, target = Target.Player, value = item.value });
                            break;
                        case Specifications.PlayerRocks:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Rocks, target = Target.Player, value = item.value });
                            break;

                        case Specifications.EnemyTower:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Tower, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyWall:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Wall, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyDiamondMines:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.DiamondMines, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyMenagerie:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Menagerie, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyColliery:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Colliery, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyDiamonds:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Diamonds, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyAnimals:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Animals, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.EnemyRocks:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.Rocks, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.CostDiamonds:
                            price = new Price()
                            {
                                attributes = Attributes.Diamonds,
                                value = item.value
                            };
                            break;
                        case Specifications.CostAnimals:
                            price = new Price()
                            {
                                attributes = Attributes.Animals,
                                value = item.value
                            };
                            break;
                        case Specifications.CostRocks:
                            price = new Price()
                            {
                                attributes = Attributes.Rocks,
                                value = item.value
                            };
                            break;
                        case Specifications.PlayAgain:
                            playAgain = true;
                            break;
                        case Specifications.EnemyDirectDamage:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.DirectDamage, target = Target.Enemy, value = item.value });
                            break;
                        case Specifications.PlayerDirectDamage:
                            cardAttributes.Add(new CardAttributes() { attributes = Attributes.DirectDamage, target = Target.Player, value = item.value });
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

     
        }
    }
}
