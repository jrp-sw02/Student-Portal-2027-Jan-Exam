<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="RegStudSummary.aspx.cs" Inherits="Admin_RegStudSummary" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Registered Candidates"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                </script>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
<uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                         <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="False" Width="100%" 
                            onrowdatabound="gvMain_RowDataBound" AllowSorting="True">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField  HeaderText="Course Name" HeaderStyle-Width="12%" DataField="Name" SortExpression="Name"/>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminRegstud.aspx?CourseID={0}&Status=1"
                                    DataTextField="PPursuing" HeaderText="Registered" SortExpression="PPursuing" Target="_self" >
                                   <ItemStyle HorizontalAlign="Right" />
                                 </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminRegstud.aspx?CourseID={0}&Status=4"
                                    DataTextField="PCompleted" HeaderText="Completed" SortExpression="PCompleted" Target="_self">
                                 <ItemStyle HorizontalAlign="Right" />
                                  </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminRegstud.aspx?CourseID={0}&Status=5"
                                    DataTextField="PExpired" HeaderText="Expired" SortExpression="PExpired" Target="_self" >
                                    <ItemStyle HorizontalAlign="Right" />
                                 </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminRegstud.aspx?CourseID={0}&Status=6"
                                    DataTextField="PCancelled" HeaderText="Cancelled" SortExpression="PCancelled" Target="_self">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="13%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminRegstud.aspx?CourseID={0}&Status=2"
                                    DataTextField="PreRegistered" HeaderText="Re-Registered" SortExpression="PreRegistered" Target="_self">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="13%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminRegstud.aspx?CourseID={0}&Status=3"
                                    DataTextField="Ppractpending" HeaderText="Project Pending" SortExpression="Ppractpending"
                                    Target="_self">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
