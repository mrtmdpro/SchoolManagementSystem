using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolManagementSystem.Models.CommonFn;

namespace SchoolManagementSystem.Teacher
{
    public partial class StudentAttendance : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["staff"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                GetClass();
                btnMarkAttendance.Visible = false;
            }
        }

        protected void Timer1_OnTick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var dt = fn.Fetch(
                @"SELECT StudentId, StudentName AS Name, StudentPhone AS Phone, StudentRollNo AS RollNo FROM Students WHERE ClassId = '"+ ddlClass.SelectedValue +"'");
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (dt.Rows.Count > 0)
            {
                btnMarkAttendance.Visible = true;
            }
            else
            {
                btnMarkAttendance.Visible = false;
            }
        }

        protected void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            bool isTrue = false;
            foreach (GridViewRow row in GridView1.Rows)
            {
                string rollNo = row.Cells[4].Text.Trim();
                int studentId = Convert.ToInt32(row.Cells[1].Text);

                RadioButton rb1 = (row.Cells[0].FindControl("RadioButton1") as RadioButton);
                RadioButton rb2 = (row.Cells[0].FindControl("RadioButton2") as RadioButton);
                int status = 0;
                if (rb1.Checked)
                {
                    status = 1;
                }
                else if (rb2.Checked)
                {
                    status = 0;
                }

                fn.Query(@"INSERT INTO StudentAttendances VALUES ('" + studentId + "','" + ddlClass.SelectedValue + "','"+ ddlSubject.SelectedValue +"','"+ rollNo +"','" + status + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "')");
                isTrue = true;
            }
            if (isTrue)
            {
                lblMsg.Text = "Inserted successfully!";
                lblMsg.CssClass = "alert alert-success";
            }
            else
            {
                lblMsg.Text = "Insertion failed.";
                lblMsg.CssClass = "alert alert-warning";
            }
        }
    }
}