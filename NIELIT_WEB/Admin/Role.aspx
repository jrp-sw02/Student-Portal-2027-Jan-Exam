<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="Role.aspx.cs" Inherits="Admin_Role" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Roles"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" runat="server"
        Visible="false" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                </script>
            </div>
        </div>
    </asp:Panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Role Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBreadCrumb" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        var dtgp = "<%= gvMain.ClientID %>"
        function validate(Sender, Mode) {
            var Number = /^[0-9,.]/;
            var arrId = Sender.id.split("_")
            if (Mode == 'e') {
                if (!isBlank("cphContents_gvMain_Txtrolename1_" + arrId[3], "Role Name"))
                    return false;
                if (!isSelected("cphContents_gvMain_ddlusertype2_" + arrId[3], "User Type"))
                    return false;
            }
            else {
                if(!isBlank("cphContents_gvMain_Txtrolename","Role Name"))
                    return false;
                if (!isSelected("cphContents_gvMain_ddlusertype1", "User Type"))
                    return false;
            }
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    <div id="divGrid" runat="server">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                    class="ActionPopup" style="width: 132px; height: 40px;">
                    <tr>
                        <td align="left">
                            <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                            <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="false" OnRowCancelingEdit="gvMain_RowCancelingEdit"
                    OnRowEditing="gvMain_RowEditing" OnRowUpdating="gvMain_RowUpdating" Width="100%"
                    OnRowCommand="gvMain_RowCommand" OnRowDeleting="gvMain_RowDeleting" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Role Name" SortExpression="name">
                            <ItemTemplate>
                                <asp:Label ID="lblrolname" runat="server" Text='<%# Eval("name")  %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="Txtrolename" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                            </FooterTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="Txtrolename1" runat="server" Text='<%#  Eval("name") %>' MaxLength="50"
                                    Width="300px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle Width="45%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Type" SortExpression="usertype">
                            <ItemTemplate>
                                <asp:Label ID="lblusertype" runat="server" Text='<%# Eval("usertype")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlusertype1" runat="server" Width="340px">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlusertype2" runat="server" Width="340px">
                                </asp:DropDownList>
                                <asp:Label ID="lbQLevel" runat="server" Visible="false" Text='<%#  Eval("usertype") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemStyle Width="45%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="&lt;img title='Edit' src='../Images/edit.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbkUpdate" runat="server" CommandName="Update" OnClientClick="return validate(this,'e')"
                                    CausesValidation="True" Text="&lt;img title='Update' src='../Images/save.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                    Text="&lt;img title='Cancel' src='../Images/cancel.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="true" OnClientClick="return validate(this,'n')"
                                    CommandName="Add" Text="&lt;img title='Click to add new Role ' src='../Images/addwebpart.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="6%" />
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="&lt;img title='Click to delete this record' src='../Images/delete.gif' style='border-width:0px;' /&gt;"
                                    OnClientClick="return ConfirmAction('Are you sure you want to delete this record');"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%-- <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" />--%>
                        <%--  <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" >
                                    <HeaderStyle Width="15%" />
                                </asp:CommandField>--%>
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
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
