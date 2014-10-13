using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
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

        //TODO: refactor. Вынести в дал операции с бд
        private static DataTable dt { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetData();
            }
          
        }

        private void GetData()
        {
            MakeTableStruct();

            using (var db = new CardContext())
            {
                
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
            }

            gvTable.DataSource = dt;
            gvTable.DataBind();
        }

        private static void MakeTableStruct()
        {
            dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            foreach (Specifications suit in (Specifications[]) Enum.GetValues(typeof (Specifications)))
            {
                dt.Columns.Add(suit.ToString());
            }
        }

        protected void gvTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTable.EditIndex = e.NewEditIndex;
            GetData();
        }

        protected void gvTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTable.EditIndex = -1;
            GetData();
        }

        protected void gvTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (var db = new CardContext())
            {

                string Id = gvTable.Rows[e.RowIndex].Cells[1].Text;

                var myCard = new Card { id = Convert.ToInt32(Id) };
                db.Cards.Attach(myCard);

                db.Entry(myCard).State = EntityState.Deleted;
                db.SaveChanges();


                GetData();
                
            }
        }

     

        private IDictionary<string, object> GetValues(GridViewRow row)
        {
            IOrderedDictionary dictionary = new OrderedDictionary();

            foreach (Control control in row.Controls)
            {
                DataControlFieldCell cell = control as DataControlFieldCell;

                if ((cell != null) && cell.Visible)
                {
                    cell.ContainingField.ExtractValuesFromCell(dictionary, cell, row.RowState, true);
                }
            }

            IDictionary<string, object> values = new Dictionary<string, object>();

            foreach (DictionaryEntry de in dictionary)
            {
                values[de.Key.ToString()] = de.Value;
            }

            return values;
        }

        protected void gvTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var db = new CardContext())
            {
                GridViewRow row = gvTable.Rows[e.RowIndex];
                var newValues = GetValues(row);
                int id = Convert.ToInt32(newValues["id"]);
                var myCard = db.Cards.FirstOrDefault(x => x.id == id );

                myCard.name = newValues["name"].ToString();

                foreach (var item in newValues)
                {
                    if (item.Key != "name" && item.Key != "id" )
                    {
                        Specifications spec = (Specifications)Enum.Parse(typeof(Specifications), item.Key);
                     

                        if (myCard.cardParams.Any(x => x.key == spec))
                        {
                         
                            if (item.Value != null)
                            {
                                myCard.cardParams.First(x => x.key == spec).value = Convert.ToInt32(item.Value);
                            }
                            else
                            {
                               /* var removedParams = new CardParams()
                                {
                                    id = myCard.cardParams.First(x => x.key == spec)
                                     
                                };*/

                              //  db.CardParamses.Attach(removedParams);
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

                gvTable.EditIndex = -1;
                GetData();

            }
        }

    
    }
}