using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Admin
{
    public partial class ExpenseDetails : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                GetExpenseDetails();
                //GridView1.UseAccessibleHeader = true;
                //GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                //GridView1.HeaderRow.CssClass = "fixedHeader";
            }
        }

        private void GetExpenseDetails()
        {
            DataTable dt = fn.Fetch(@"SELECT Row_NUMBER() Over(Order by(Select 1)) AS [Sr.No], E.ExpenseId, E.ClassId, C.ClassName, S.SubjectId, 
                                           S.SubjectName, E.ChargeAmount FROM Expenses AS E INNER JOIN Classes C ON E.ClassId = C.ClassId INNER JOIN Subjects S ON E.SubjectId = S.SubjectId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.TableSection = TableRowSection.TableHeader;
            //}
        }
    }
}