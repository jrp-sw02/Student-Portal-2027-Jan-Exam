<%@ Page Title="" Language="C#"  MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="FrmModuleHistory.aspx.cs" Inherits="FrmModuleHistory" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
 <asp:Label ID="lblHeading" runat="server" Text="Module Details"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <div style="background-color: #c7dded; width: 100%; padding: 10px 10px 4px 10px;
        color: #666666; margin-top: 8px; min-height: 20px;">
        <span style="font: normal 18px arial; color: #003366;">Module Summary</span>
    </div>
    <div id="divGrid" runat="server" width="100%">
        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
            Width="100%" OnRowDataBound="gvMain_RowDataBound" AllowPaging="false" PageSize="50">
            <Columns>
                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                    <HeaderStyle Width="2%" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:HyperLinkField HeaderStyle-Width="13%" HeaderText="Exam Name" DataTextField="exname"
                    Target="_self" DataNavigateUrlFields="CourseID,examID" DataNavigateUrlFormatString="frmexamhistory.aspx?CourseID={0}&ExamId={1}">
                    <HeaderStyle Width="15%" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderStyle-Width="13%" HeaderText="Code" DataTextField="Code"
                    Target="_self" DataNavigateUrlFields="CourseID,examID" DataNavigateUrlFormatString="frmexamhistory.aspx?CourseID={0}&ExamId={1}">
                    <HeaderStyle Width="10%" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderStyle-Width="33%" HeaderText="Module Name" DataTextField="name"
                    Target="_self" DataNavigateUrlFields="CourseID,examID" DataNavigateUrlFormatString="frmexamhistory.aspx?CourseID={0}&ExamId={1}">
                    <HeaderStyle Width="50%" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderText="Result" HeaderStyle-Width="13%" DataTextField="Result"
                    DataNavigateUrlFields="CourseID,examID" DataNavigateUrlFormatString="frmexamhistory.aspx?CourseID={0}&ExamId={1}">
                    <HeaderStyle Width="13%" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderText="Grade" HeaderStyle-Width="13%" DataTextField="Grade"
                    DataNavigateUrlFields="CourseID,examID" DataNavigateUrlFormatString="frmexamhistory.aspx?CourseID={0}&ExamId={1}">
                    <HeaderStyle Width="10%" />
                </asp:HyperLinkField>
            </Columns>
            <PagerSettings Visible="False" />
        </asp:GridView>
        <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
 <uc2:SideLink ID="Sidelink" runat="server" />
  <uc3:sidelink ID="Sidelink1" runat="server" />   
</asp:Content>

