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
    public partial class EmployeeAttendance : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Attendance();
            }
        }

        private void Attendance()
        {
            DataTable dt = fn.Fetch(@"SELECT TeacherId, TeacherName AS Name, TeacherPhone AS Phone, TeacherEmail AS Email FROM Teachers");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Timer1_OnTick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        protected void btnMarkAttendance_OnClick(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                int teacherId = Convert.ToInt32(row.Cells[1].Text);

                RadioButton rb1 = (row.Cells[0].FindControl("RadioButton1") as RadioButton);
                RadioButton rb2 = (row.Cells[0].FindControl("RadioButton2") as RadioButton);
                int status = 0;
                if (rb1.Checked)
                {
                    status = 1;
                }else if (rb2.Checked)
                {
                    status = 0;
                }

                fn.Query(@"INSERT INTO TeacherAttendances VALUES ('"+ teacherId +"','"+ status +"','"+ DateTime.Now.ToString("yyyy/MM/dd") +"')");
                lblMsg.Text = "Updated successfully!";
                lblMsg.CssClass = "alert alert-success";
            }
        }
    }
}