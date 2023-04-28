<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="MarkDetails.aspx.cs" Inherits="SchoolManagementSystem.Admin.MarkDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div style="background-image:url('../Images/WhiteBG.jpg'); width:100%; height: 1080px; background-repeat: no-repeat; background-size: cover; background-attachment: fixed;">
        <div class="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h3 class="text-center">Marks details</h3>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="ddlClass">Class</label>
                    <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Class is required."
                                                ControlToValidate="ddlClass" Display="Dynamic" ForeColor="Red" InitialValue="Select class" SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-md-6">
                    <label for="txtRoll">Student Roll Number</label>
                    <asp:TextBox ID="txtRoll" runat="server" CssClass="form-control" placeholder="Enter student's roll number" required>
                    </asp:TextBox>
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#5558C9" Text="Geet Marks" OnClick="btnAdd_OnClick"/>
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5">
                <div class="col-md-12">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="No record to display."
                                  AutoGenerateColumns="False" AllowPaging="True" PageSize="8" OnPageIndexChanging="GridView1_OnPageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>

                            <asp:BoundField DataField="ExamId" HeaderText="ExamId">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>

                            <asp:BoundField DataField="ClassName" HeaderText="Class">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="SubjectName" HeaderText="Subject">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="RollNo" HeaderText="Rol number">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="TotalMarks" HeaderText="Total marks">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="OutOfMarks" HeaderText="Out of marks">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>

                        </Columns>
                        <HeaderStyle BackColor="#5558C9" ForeColor="White"></HeaderStyle>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
