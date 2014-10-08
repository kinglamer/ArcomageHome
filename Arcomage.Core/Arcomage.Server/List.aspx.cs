using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arcomage.DAL;
using Arcomage.Entity;

namespace Arcomage.Server
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            using (var db =new CardContext())
            {

                dt.Columns.Add("id");
                dt.Columns.Add("name");
             
                foreach (Specifications suit in (Specifications[])Enum.GetValues(typeof(Specifications)))
                {
                    dt.Columns.Add(suit.ToString());
                }

                var cards = db.Cards.ToList();

                foreach (var card in cards)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["name"] = card.name;

                    newRow["id"] = card.id;

                    foreach (var parm in card.cardParams)
                    {
                        newRow[parm.key.ToString()] = parm.value;
                    }

                    dt.Rows.Add(newRow);
                }




             
                //  
            }

            gvTable.DataSource = dt;
            gvTable.DataBind();
        }
    }
}