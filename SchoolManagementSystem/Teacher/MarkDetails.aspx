<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="MarkDetails.aspx.cs" Inherits="SchoolManagementSystem.Teacher.MarksDetails" %>

<%@ Register src="~/MarksDetailsUserControl.ascx" tagPrefix="uc" tagName="MarkDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <%--<h1>New mark details</h1>--%>
    <uc:MarkDetails runat="server" ID="MarkDetails1" />

</asp:Content>
