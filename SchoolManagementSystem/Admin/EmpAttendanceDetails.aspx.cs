using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem
{
    public partial class EmpAttendanceDetails : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTeacher();
            }
        }

        private void GetTeacher()
        {
            DataTable dt = fn.Fetch("SELECT * FROM Teachers");
            ddlTeacher.DataSource = dt;
            ddlTeacher.DataTextField = "TeacherName";
            ddlTeacher.DataValueField = "TeacherId";
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, "Select Teacher");
        }

        protected void btnCheckAttendance_OnClick(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtMonth.Text);
            DataTable dt = fn.Fetch(@"SELECT Row_Number() OVER(ORDER BY(SELECT 1)) AS [Sr.No], T.TeacherName, TA.Status, TA.Date FROM TeacherAttendances AS TA INNER JOIN Teachers 
                                        AS T ON T.TeacherId = TA.TeacherId WHERE DATEPART(yy, Date) = '" + date.Year + "' AND DATEPART(M, Date) = '" + date.Month + "' AND TA.TeacherId = '" + ddlTeacher.SelectedValue + "'");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}