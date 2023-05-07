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
    public partial class StudentAttendanceUC : System.Web.UI.UserControl
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["admin"] == null)
            //{
            //    Response.Redirect("../Login.aspx");
            //}
            if (!IsPostBack)
            {
                GetClass();
            }
        }

        private void GetClass()
        {
            DataTable dt = fn.Fetch("SELECT * FROM Classes");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "ClassName";
            ddlClass.DataValueField = "ClassId";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classId = ddlClass.SelectedValue;
            DataTable dt = fn.Fetch("SELECT * FROM Subjects WHERE ClassId = '" + classId + "'");
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "SubjectName";
            ddlSubject.DataValueField = "SubjectId";
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, "Select Subject");
        }

        protected void btnCheckAttendance_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DateTime date = Convert.ToDateTime(txtMonth.Text);
            if (ddlSubject.SelectedValue == "Select Subject")
            {
                dt = fn.Fetch(@"SELECT Row_Number() OVER(ORDER BY(SELECT 1)) AS [Sr.No], S.StudentName, SA.Status, SA.Date FROM StudentAttendances AS SA INNER JOIN Students 
                               AS S ON S.StudentRollNo = SA.RollNo WHERE SA.ClassId = '" + ddlClass.SelectedValue + "' AND SA.RollNo = '" + txtRollNo.Text.Trim() 
                              + "' AND DATEPART(yy, Date) = '" + date.Year + "' AND DATEPART(M, Date) = '" + date.Month + "' AND SA.Status = 1");
            }
            else
            {
                dt = fn.Fetch(@"SELECT Row_Number() OVER(ORDER BY(SELECT 1)) AS [Sr.No], S.StudentName, SA.Status, SA.Date FROM StudentAttendances AS SA INNER JOIN Students 
                               AS S ON S.StudentRollNo = SA.RollNo WHERE SA.ClassId = '" + ddlClass.SelectedValue + "' AND SA.RollNo = '" + txtRollNo.Text.Trim() 
                              + "' AND SA.SubjectId = '"+ ddlSubject.SelectedValue +"' AND DATEPART(yy, Date) = '" + date.Year + "' AND DATEPART(M, Date) = '" + date.Month + "' AND SA.Status = 1");
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}