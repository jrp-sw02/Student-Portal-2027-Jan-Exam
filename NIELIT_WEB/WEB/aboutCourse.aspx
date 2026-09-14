<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="aboutCourse.aspx.cs" Inherits="aboutCourse" %>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>
<%@ Register src="../UserControl/SideLink.ascx" tagname="SideLink" tagprefix="uc2" %>
<%@ Register src="../UserControl/SideLink.ascx" tagname="SideLink" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script src="../Script/jquery-1.7.1.js" type="text/javascript"></script>
<script src="../Script/jquery.dd.js" type="text/javascript"></script>
<script src="../Script/GlobalFunction.js" type="text/javascript"></script>
<style type="text/css">
    .sidetable a
    {
        color: #000000;
        text-decoration: none;
    }
    .sidetable a:hover
    {
        color: #3366CC;
        text-decoration: underline;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblheading1" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
<uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <div id="pdescription" class="summary_block" runat="server">
        <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
        <iframe runat="server" id="ifrmAboutUs" width="750px"
            style="height: 400px;" frameborder='0' marginheight='0' marginwidth='0' scrolling="no"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
<uc2:SideLink ID="Sidelink" runat="server" />
<uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
