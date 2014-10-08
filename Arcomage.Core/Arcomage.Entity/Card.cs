using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcomage.Entity
{

    public class CardParams //student
    {
        [Key]
        public int id { get; set; }

        public Specifications key { get; set; }
        public int value { get; set; }

      //  public virtual List<Card> cards { get; set; }

        public Card card { get; set; }

        public CardParams()
        {
            card = new Card();
        }
    }

    public class Card //Standard
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }

        public virtual ICollection<CardParams> cardParams { get; set; }

        public Card()
        {
            cardParams = new Collection<CardParams>();
        }
    }


    public enum Specifications 
    {
        NotSet = 0,
        PlayerTower,
        PlayerWall,
        PlayerDiamondMines,
        PlayerMenagerie,
        PlayerColliery,
        PlayerDiamonds,
        PlayerAnimals,
        PlayerRocks,

        EnemyTower,
        EnemyWall,
        EnemyDiamondMines,
        EnemyMenagerie,
        EnemyColliery,
        EnemyDiamonds,
        EnemyAnimals,
        EnemyRocks,

        CostDiamonds,
        CostAnimals,
        CostRocks
    }
}
