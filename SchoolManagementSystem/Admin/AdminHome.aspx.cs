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
    public partial class AdminHome : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            else
            {
                studentCount();
                teacherCount();
                subjectCount();
                classCount();
            }
        }

        void studentCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) FROM Students");
            Session["student"] = dt.Rows[0][0];
        }
        void teacherCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) FROM Teachers");
            Session["teacher"] = dt.Rows[0][0];
        }
        void subjectCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) FROM Subjects");
            Session["subject"] = dt.Rows[0][0];
        }
        void classCount()
        {
            DataTable dt = fn.Fetch("SELECT COUNT(*) FROM Classes");
            Session["class"] = dt.Rows[0][0];
        }
    }
}