using Arcomage.DAL;
using Arcomage.Entity;
using System;
using System.Collections.Generic;

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
                var item = new Card();

                if (tbName.Text.Length > 0)
                {

                    item.name = tbName.Text;
                }
                else
                {
                    lbError.Text = "Необходимо заполнить название карты";
                    return;
                }

                List<CardParams>  cardParam = new List<CardParams>();

                Dictionary<Specifications, string> dicCost = new Dictionary<Specifications, string>();
                dicCost.Add(Specifications.CostAnimals,tbCostAnimals.Text);
                dicCost.Add(Specifications.CostDiamonds,tbCostDiamonds.Text);
                dicCost.Add(Specifications.CostRocks,tbCostRocks.Text);


                var result = GetCardParams(item, dicCost);

                if (result.Count > 1 || result.Count == 0)
                {
                    lbError.Text = "Необходимо заполнить стоимость карты. У карты может быть только одна стоимость.";
                    return;
                }

                cardParam.AddRange(result);

                Dictionary<Specifications, string> dicParam = new Dictionary<Specifications, string>();
                dicParam.Add(Specifications.PlayerTower, tbTowerPlayer.Text);
                dicParam.Add(Specifications.PlayerWall, tbWallPlayer.Text);

                dicParam.Add(Specifications.EnemyTower, tbTowerEnemy.Text);
                dicParam.Add(Specifications.EnemyWall, tbWallEnemy.Text);

                dicParam.Add(Specifications.PlayerAnimals, tbAnimalsPlayer.Text);
                dicParam.Add(Specifications.PlayerDiamonds, tbDiamondsPlayer.Text);
                dicParam.Add(Specifications.PlayerRocks, tbRocksPlayer.Text);

                dicParam.Add(Specifications.EnemyAnimals, tbAnimalsEnemy.Text);
                dicParam.Add(Specifications.EnemyDiamonds, tbDiamondsEnemy.Text);
                dicParam.Add(Specifications.EnemyRocks, tbRocksEnemy.Text);

                dicParam.Add(Specifications.PlayerMenagerie, tbMenageriePlayer.Text);
                dicParam.Add(Specifications.PlayerDiamondMines, tbDiamondMinesPlayer.Text);
                dicParam.Add(Specifications.PlayerColliery, tbCollieryPlayer.Text);

                dicParam.Add(Specifications.EnemyMenagerie, tbMenagerieEnemy.Text);
                dicParam.Add(Specifications.EnemyDiamondMines, tbDiamondMinesEnemy.Text);
                dicParam.Add(Specifications.EnemyColliery, tbCollieryEnemy.Text);

                result = GetCardParams(item, dicParam);

                if (result.Count > 3 || result.Count == 0)
                {
                    lbError.Text = "Необходимо заполнить хотя бы один параметр карты. Параметров карт не должо быть больше 3";
                    return;
                }

                cardParam.AddRange(result);

                db.Cards.Add(item);
                db.SaveChanges();

                db.CardParamses.AddRange(cardParam);
                db.SaveChanges();

            }
        }



        private List<CardParams> GetCardParams(Card card, Dictionary<Specifications, string> elem)
        {

            List<CardParams> returnVal = new List<CardParams>();

            foreach (var item in elem)
            {
                var result = AddToList(card, item.Value, item.Key);

                if (result != null)
                    returnVal.Add(result);
            }
          

            return returnVal;
        }

        private CardParams AddToList(Card item, string value, Specifications key)
        {
            if (value.Length > 0)
            {
                CardParams newItem = new CardParams()
                {
                    card = item,
                    key = key,
                    value = Convert.ToInt32(value)
                };
                return newItem;
            }
            else
            {
                return null;
            }
        }
    }
}