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
    public partial class MarksDetailsUserControl : System.Web.UI.UserControl
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
                GetMarks();
            }
        }

        private void GetMarks()
        {
            DataTable dt = fn.Fetch(@"SELECT Row_NUMBER() Over(Order by(Select 1)) AS [Sr.No], E.ExamId, E.ClassId, C.ClassName, E.SubjectId, S.SubjectName, E.RollNo, E.TotalMarks, 
                                            E.OutOfMarks FROM Exams AS E INNER JOIN Classes AS C ON C.ClassId = E.ClassId INNER JOIN Subjects AS S ON S.SubjectId = E.SubjectId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
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

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            try
            {
                string classId = ddlClass.SelectedValue;
                if (classId == "Select Class")
                {
                    lblMsg.Text = "Select a class before proceeding.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                string rollNo = txtRoll.Text.Trim();
                DataTable dt = fn.Fetch(@"SELECT Row_NUMBER() Over(Order by(Select 1)) AS [Sr.No], E.ExamId, E.ClassId, C.ClassName, E.SubjectId, S.SubjectName, E.RollNo, E.TotalMarks, 
                                            E.OutOfMarks FROM Exams AS E INNER JOIN Classes AS C ON C.ClassId = E.ClassId INNER JOIN Subjects AS S ON S.SubjectId = E.SubjectId WHERE E.ClassId = '" + classId + "' AND E.RollNo = '" + rollNo + "' ");
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
        }
    }
}