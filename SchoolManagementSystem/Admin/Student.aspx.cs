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
    public partial class Student : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
                GetStudents();
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

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlGender.SelectedValue != "0")
                {
                    string rollNo = txtRollNo.Text.Trim();
                    DataTable dt = fn.Fetch("SELECT * FROM Students WHERE ClassId = '"+ ddlClass.SelectedValue + "' AND StudentRollNo = '" + rollNo + "'");
                    if (dt.Rows.Count == 0)
                    {
                        string query = "INSERT INTO Students VALUES ('" + txtName.Text.Trim() + "', '" + txtDoB.Text.Trim() + "', '" + ddlGender.SelectedValue + "', '" +
                                       txtMobile.Text.Trim() + "', '" + txtEmail.Text.Trim() + "', '" + txtRollNo.Text.Trim() + "', '" + txtAddress.Text.Trim() + "', '" + ddlClass.SelectedValue +"')";
                        fn.Query(query);
                        lblMsg.Text = "Inserted successful!";
                        lblMsg.CssClass = "alert alert-success";
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtDoB.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtEmail.Text = string.Empty;
                        txtRollNo.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        ddlClass.SelectedIndex = 0;
                        GetStudents();
                    }
                    else
                    {
                        lblMsg.Text = "Entered Roll No. (<b>'" + rollNo + "'</b>) already existed for the selected class.";
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

        private void GetStudents()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [Sr.No], S.StudentId, S.StudentName, S.StudentDOB, 
                                        S.StudentGender, S.StudentPhone, S.StudentEmail, S.StudentRollNo, S.StudentAddress, C.ClassId, C.ClassName FROM Students S INNER JOIN
                                        CLASSES C ON C.ClassId = S.ClassId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetStudents();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetStudents();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetStudents();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var row = GridView1.Rows[e.RowIndex];
                var studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string studentName = (row.FindControl("txtName") as TextBox).Text;
                string studentPhone = (row.FindControl("txtMobile") as TextBox).Text;
                string studentEmail = (row.FindControl("txtEmail") as TextBox).Text;
                string studentRollNo = (row.FindControl("txtRollNo") as TextBox).Text;
                string studentAddress = (row.FindControl("txtAddress") as TextBox).Text;
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[5].FindControl("ddlClass")).SelectedValue;
                fn.Query("UPDATE Students SET StudentName = '" + studentName.Trim() + "', StudentPhone = '" + studentPhone.Trim() + "', StudentEmail = '" + studentEmail.Trim() + "', StudentRollNo = '" + studentRollNo.Trim() + "', StudentAddress = '" + studentAddress.Trim() + "', ClassId = '"+classId+"' WHERE StudentId = '" + studentId + "'");
                lblMsg.Text = "Student updated successful.";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetStudents();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClass");
                DataTable dt = fn.Fetch("SELECT * FROM Classes");
                ddlClass.DataSource = dt;
                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassId";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0, "Select Class");
                string selectedClass = DataBinder.Eval(e.Row.DataItem, "ClassName").ToString();
                ddlClass.Items.FindByText(selectedClass).Selected = true;

            }
        }
    }
}