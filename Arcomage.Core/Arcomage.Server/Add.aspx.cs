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
            
            lbInfo.Text = "";
            lbError.Text = "";

            
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

                if (tbDes.Content.Length > 0)
                {
                    item.description = tbDes.Content;
                }
                else
                {
                    lbError.Text = "Необходимо заполнить описание карты";
                    return;
                }

                List<CardParams>  cardParam = new List<CardParams>();

                var dicCost = GetDicCost();


                var result = GetCardParams(item, dicCost);

                if (result.Count > 1 || result.Count == 0)
                {
                    lbError.Text = "Необходимо заполнить стоимость карты. У карты может быть только одна стоимость.";
                    return;
                }

                cardParam.AddRange(result);

                var dicParam = GetDicParam();

                result = GetCardParams(item, dicParam);

                if (result.Count > 3 || result.Count == 0)
                {
                    lbError.Text = "Необходимо заполнить хотя бы один параметр карты. Параметров карт не должо быть больше 3";
                    return;
                }

                DatabaseHelper.SaveCard(cardParam, result, item);

                lbError.Text = "";
                lbInfo.Text = "Карта добавлена";
            
        }

        private Dictionary<Specifications, string> GetDicCost()
        {
            Dictionary<Specifications, string> dicCost = new Dictionary<Specifications, string>();
            dicCost.Add(Specifications.CostAnimals, tbCostAnimals.Text);
            dicCost.Add(Specifications.CostDiamonds, tbCostDiamonds.Text);
            dicCost.Add(Specifications.CostRocks, tbCostRocks.Text);
            return dicCost;
        }

        private Dictionary<Specifications, string> GetDicParam()
        {
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

            dicParam.Add(Specifications.DirectDamage, tbDirectDamage.Text);

            if (cbGetNewCard.Checked)
                dicParam.Add(Specifications.GetCard, Convert.ToInt16(cbGetNewCard.Checked).ToString());

            return dicParam;
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