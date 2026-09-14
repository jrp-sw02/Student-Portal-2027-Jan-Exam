<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/FullInfo.master" AutoEventWireup="true"
    CodeFile="allCourses.aspx.cs" Inherits="allCourses" Debug="true" %>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Courses"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
<uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:Label Width="99%"  CssClass="error" EnableTheming="false"
        ID="Lblerror" Visible="false" runat="server" Text="" Style="background-color: #EACFCE; color: Red;
        border: 1px solid maroon; font-size: 11pt; font-variant: normal;padding: 5px 5px 5px 5px;"></asp:Label>
    <asp:Panel ID="pnlCourses" runat="server" Width="100%">
    </asp:Panel>
</asp:Content>
