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

            List<Card> cards = DatabaseHelper.GetCardsForSeriz();

                foreach (var card in cards)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["name"] = card.name;

                    newRow["id"] = card.id;

                    newRow["description"] = card.description;

                    foreach (var parm in card.cardParams)
                    {
                        newRow[parm.key.ToString()] = parm.value;
                    }

                    dt.Rows.Add(newRow);
                }
           

            gvTable.DataSource = dt;
            gvTable.DataBind();
        }

        private static void MakeTableStruct()
        {
            dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            dt.Columns.Add("description");

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
                string Id = gvTable.Rows[e.RowIndex].Cells[1].Text;

                DatabaseHelper.DeleteCard(Id);
                GetData();
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


            CheckBox chkBx = (CheckBox)row.FindControl("cbGetCard");
            if (chkBx != null && chkBx.Checked)
                values.Add("GetCard", "1");
            else
            {
                values.Add("GetCard", null);
            }

            return values;
        }

        protected void gvTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
                GridViewRow row = gvTable.Rows[e.RowIndex];
                var newValues = GetValues(row);
   
                DatabaseHelper.UpdateCard(newValues);

                gvTable.EditIndex = -1;
                GetData();

            
        }
    }
}