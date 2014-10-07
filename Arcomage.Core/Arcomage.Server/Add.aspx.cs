using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arcomage.Core;
using Arcomage.Core.Parametrs;
using Arcomage.DAL;
using Arcomage.Server.ServiceHelper;

namespace Arcomage.Server
{
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            using (var db = new CardContext())
            {
                var item = new CardParametrs();

                if (tbName.Text.Length > 0)
                {
                    
                    item.name = tbName.Text;
                }
                else
                {
                    lbError.Text = "Необходимо заполнить название карты";
                    return;
                }

                Resources res;
                if (!GetCostCard(out res))
                {
                    lbError.Text = "Необходимо заполнить стоимость карты. У карты может быть только одна стоимость.";
                    return;
                }
                else
                {
                    item.cardCost = res;
                }


                Buildings bP ;
                SourcesOfResources srP;
                Resources rP;

                Buildings bE ;
                SourcesOfResources srE;
                Resources rE;



                if (!GetCardParams(out bP, out srP, out rP, out bE, out srE, out rE))
                {
                    lbError.Text = "Необходимо заполнить хотя бы один параметр карты. Параметров карт не должо быть больше 3";
                    return;
                }
                else
                {
                    item.playerBuildings = bP;
                    item.playerSources = srP;
                    item.playerResources = rP;
                    item.enemyBuildings = bE;
                    item.enemySources = srE;
                    item.enemyResources = rE;

                }

                

                db.ArcomageCards.Add(item);

                db.SaveChanges();
            }
        }

        private bool GetCardParams(out Buildings bP, out SourcesOfResources srP, out Resources rP, out Buildings bE, out SourcesOfResources srE, out Resources rE)
        {
            int cntParams = 0;

            bP = new Buildings();
            srP =new SourcesOfResources();
            rP =new Resources();

            bE = new Buildings();
            srE = new SourcesOfResources();
            rE = new Resources();


            cntParams = CntBuilding(bP, cntParams, tbTowerPlayer.Text, tbWallPlayer.Text);
            cntParams = CntBuilding(bE, cntParams, tbTowerEnemy.Text, tbWallEnemy.Text);

            cntParams = CntResourses(rP, cntParams, tbAnimalsPlayer.Text, tbDiamondsPlayer.Text, tbRocksPlayer.Text);
            cntParams = CntResourses(rE, cntParams, tbAnimalsEnemy.Text, tbDiamondsEnemy.Text, tbRocksEnemy.Text);

            cntParams = CntSource(srP, cntParams, tbMenageriePlayer.Text, tbDiamondMinesPlayer.Text, tbCollieryPlayer.Text);
            cntParams = CntSource(srE, cntParams, tbMenagerieEnemy.Text, tbDiamondMinesEnemy.Text, tbCollieryEnemy.Text);

            

            if (cntParams == 0 || cntParams > 3)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        private int CntBuilding(Buildings bP, int cntParams, string Tower, string Wall)
        {
            if (Tower.Length > 0)
            {
                bP.Tower = Convert.ToInt32(Tower);
                cntParams++;
            }

            if (Wall.Length > 0)
            {
                bP.Wall = Convert.ToInt32(Wall);
                cntParams++;
            }
            return cntParams;
        }


        private bool GetCostCard(out Resources cardCost)
        {
            int cntBoxes = 0;
            cardCost =new Resources();

            cntBoxes = CntResourses(cardCost, cntBoxes, tbCostAnimals.Text, tbCostDiamonds.Text, tbCostRocks.Text);

            if (cntBoxes == 0 || cntBoxes > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int CntResourses(Resources cardCost, int cntBoxes, string animals, string dimonds, string rocks)
        {
            if (animals.Length > 0)
            {
                cardCost.Animals = Convert.ToInt32(animals);
                cntBoxes++;
            }

            if (dimonds.Length > 0)
            {
                cardCost.Diamonds = Convert.ToInt32(dimonds);
                cntBoxes++;
            }

            if (rocks.Length > 0)
            {
                cardCost.Rocks = Convert.ToInt32(rocks);
                cntBoxes++;
            }
            return cntBoxes;
        }

        private int CntSource(SourcesOfResources sourse, int cntBoxes, string animals, string dimonds, string rocks)
        {
            if (animals.Length > 0)
            {
                sourse.Menagerie = Convert.ToInt32(animals);
                cntBoxes++;
            }

            if (dimonds.Length > 0)
            {
                sourse.DiamondMines = Convert.ToInt32(dimonds);
                cntBoxes++;
            }

            if (rocks.Length > 0)
            {
                sourse.Colliery = Convert.ToInt32(rocks);
                cntBoxes++;
            }
            return cntBoxes;
        }
    }
}