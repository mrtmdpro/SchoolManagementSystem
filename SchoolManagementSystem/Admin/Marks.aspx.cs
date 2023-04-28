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
    public partial class Marks : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
                GetMarks();
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

        private void GetMarks()
        {
            DataTable dt = fn.Fetch(@"SELECT Row_NUMBER() Over(Order by(Select 1)) AS [Sr.No], E.ExamId, E.ClassId, C.ClassName, S.SubjectId, 
                                           S.SubjectName, E.RollNo, E.TotalMarks, E.OutOfMarks FROM Exams AS E INNER JOIN Classes C ON E.ClassId = C.ClassId INNER JOIN Subjects S ON E.SubjectId = S.SubjectId");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            try
            {
                string classId = ddlClass.SelectedItem.Value;
                string subjectId = ddlSubject.SelectedItem.Value;
                string rollNo = txtRoll.Text.Trim();
                string studMarks = txtStudMarks.Text.Trim();
                string outOfMarks = txtOutOfMarks.Text.Trim();

                DataTable dttb1 =
                    fn.Fetch("SELECT StudentId FROM Students WHERE ClassId = '" + classId +
                             "' AND StudentRollNo = '" + rollNo + "'");
                if (dttb1.Rows.Count > 0)
                {
                    DataTable dt =
                        fn.Fetch("SELECT * FROM Exams WHERE ClassId = '" + classId +
                                 "' AND SubjectId = '" + subjectId + "' AND RollNo = '" + rollNo + "'");
                    if (dt.Rows.Count == 0)
                    {
                        string query = "INSERT INTO Exams Values('" + classId + "','" + subjectId + "', '" + rollNo + "', '" + studMarks + "', '" + outOfMarks + "')";
                        fn.Query(query);
                        lblMsg.Text = "Inserted successful!";
                        lblMsg.CssClass = "alert alert-success";
                        ddlClass.SelectedIndex = 0;
                        ddlSubject.SelectedIndex = 0;
                        txtRoll.Text = string.Empty;
                        txtStudMarks.Text = string.Empty;
                        txtOutOfMarks.Text = string.Empty;
                        GetMarks();
                    }
                    else
                    {
                        lblMsg.Text = "Entered <b>Marks</b> already existed.";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Text = "Entered roll number <b>"+rollNo+"</b> does not exist for selected class.";
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
            GetMarks();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetMarks();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetMarks();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var row = GridView1.Rows[e.RowIndex];
                var examId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string classId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGV")).SelectedValue;
                string subjId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGV")).SelectedValue;
                var totalMarks = (row.FindControl("txtStudMarksGV") as TextBox).Text.Trim();
                var outOfMarks = (row.FindControl("txtOutOfMarksGV") as TextBox).Text.Trim();
                var rollNo = (row.FindControl("txtRollNoGV") as TextBox).Text.Trim();
                fn.Query(@"UPDATE Exams SET ClassId = '" + classId + "', SubjectId = '" + subjId + "', RollNo = '" + rollNo + "', TotalMarks = '"+totalMarks+"', OutOfMarks = '"+outOfMarks+"' WHERE ExamId = '" + examId + "'");
                lblMsg.Text = "Record updated successful.";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetMarks();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClassGV");
                    DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlSubjectGV");
                    DataTable dt = fn.Fetch("SELECT * FROM Subjects WHERE ClassId = '" + ddlClass.SelectedValue + "'");
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectId";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, "Select subject");
                    string selectedSubject = DataBinder.Eval(e.Row.DataItem, "SubjectName").ToString();
                    ddlSubject.Items.FindByText(selectedSubject).Selected = true;
                }
            }
        }

        protected void ddlClassGV_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlClassSelected = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlClassSelected.NamingContainer;
            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlSubjectGV = (DropDownList)row.FindControl("ddlSubjectGV");
                    DataTable dt = fn.Fetch("SELECT * FROM Subjects WHERE ClassId = '" +
                                            ddlClassSelected.SelectedValue + "'");
                    ddlSubjectGV.DataSource = dt;
                    ddlSubjectGV.DataTextField = "SubjectName";
                    ddlSubjectGV.DataValueField = "SubjectId";
                    ddlSubjectGV.DataBind();
                }
            }
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
    }
}