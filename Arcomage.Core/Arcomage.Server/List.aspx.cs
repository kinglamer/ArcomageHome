using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arcomage.DAL;

namespace Arcomage.Server
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            using (var db = new CardContext())
            {
               var data = db.ArcomageCards.Select(p => new 
               {p.id,
                PlayerTower =  p.playerBuildings.Tower,
                PlayerWall = p.playerBuildings.Wall,

                EnemyTower = p.enemyBuildings.Tower,
                EnemyWall = p.enemyBuildings.Wall,

                PlayerResDiamonds = p.playerResources.Diamonds,
                PlayerResAnimals = p.playerResources.Animals,
                PlayerResRocks = p.playerResources.Rocks,

                EnemyResDiamonds = p.enemyResources.Diamonds,
                EnemyResAnimals = p.enemyResources.Animals,
                EnemyResRocks = p.enemyResources.Rocks,

                PlayerSourDiamonds = p.playerSources.DiamondMines,
                PlayerSourAnimals = p.playerSources.Menagerie,
                PlayerSourRocks = p.playerSources.Colliery,

                EnemySourDiamonds = p.enemySources.DiamondMines,
                EnemySourAnimals = p.enemySources.Menagerie,
                EnemySourRocks = p.enemySources.Colliery,

                 CostDiamonds =  p.cardCost.Diamonds,
                 CostAnimals=  p.cardCost.Animals, 
                 CostRocks =  p.cardCost.Rocks});

                gvTable.DataSource = data.ToList();
                gvTable.DataBind();
            }
        }
    }
}