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
    public partial class Teacher : System.Web.UI.Page
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
                GetTeachers();
            }
        }

        private void GetTeachers()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [Sr.No], TeacherId, TeacherName, TeacherDOB, 
                                        TeacherGender, TeacherPhone, TeacherEmail, TeacherAddress, TeacherPassword FROM Teachers");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGender.SelectedValue != "0")
                {
                    string email = txtEmail.Text.Trim();
                    DataTable dt = fn.Fetch("SELECT * FROM Teachers WHERE TeacherEmail = '" + email + "'");
                    if (dt.Rows.Count == 0)
                    {
                        string hashedPassword = CommonFnx.ComputeHash(txtPassword.Text.Trim());
                        string query = "INSERT INTO Teachers VALUES ('" + txtName.Text.Trim() + "', '"+txtDoB.Text.Trim() +"', '"+ ddlGender.SelectedValue + "', '"+ txtMobile.Text.Trim() +"', '"+ txtEmail.Text.Trim() +"', '"+ txtAddress.Text.Trim() +"', '"+ hashedPassword.Trim() +"')";
                        fn.Query(query);
                        lblMsg.Text = "Inserted successful!";
                        lblMsg.CssClass = "alert alert-success";
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtDoB.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtEmail.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                        GetTeachers();
                    }
                    else
                    {
                        lblMsg.Text = "Entered email (<b>'" + email + "'</b>) already existed.";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Text = "Gender is required.";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetTeachers();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetTeachers();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                var TeacherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                fn.Query("DELETE FROM Teachers WHERE TeacherId = '" + TeacherId + "'");
                lblMsg.Text = "Teacher deleted successful.";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeachers();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetTeachers();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var row = GridView1.Rows[e.RowIndex];
                var teacherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string teacherName = (row.FindControl("txtName") as TextBox).Text;
                string teacherPhone = (row.FindControl("txtMobile") as TextBox).Text;
                string teacherPassword = (row.FindControl("txtPassword") as TextBox).Text;
                string hasedPassword = CommonFnx.ComputeHash(teacherPassword);
                string teacherAddress = (row.FindControl("txtAddress") as TextBox).Text;
                fn.Query("UPDATE Subjects SET TeacherName = '" + teacherName.Trim() + "', TeacherPhone = '" + teacherPhone.Trim() + "', TeacherAddress = '"+ teacherAddress.Trim() +"', TeacherPassword = '"+ hasedPassword.Trim() +"' WHERE TeacherId = '" + teacherId + "'");
                lblMsg.Text = "Teacher updated successful.";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeachers();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

    }
}