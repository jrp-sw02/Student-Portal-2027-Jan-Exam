<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ModuleExemption.aspx.cs" Inherits="ModuleExemption" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Modules Exemption"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
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
                                        <asp:Button runat="server" ID="btn1" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Filter1"></asp:Label>
                                        <asp:DropDownList ID="ddlFilter1" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        //        function document.documentElement.onclick() 
        //        { 
        //         alert(event.srcElement.id);
        //        }
        function ValidateLogin() {

            return true;

        }


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
            <table cellpadding="2" cellspacing="0" width="100%">
                <tr runat="server" visible="false" id="trMsg">
                    <td valign="top" colspan="3">
                        <asp:Label ID="lblMsg" runat="server" CssClass="error" EnableTheming="false" Width="99%"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="sample2" cellpadding="2" cellspacing="0" runat="server" id="tblShow">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblNewRevNo" runat="server" SkinID="CaptionLabel" Text="Current Course Revision Number"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblOldRevNo" runat="server" SkinID="CaptionLabel" Text="Lower Course Revision Number"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlCurrentCourseRevNo" runat="server" SkinID="ddl250" Height="22px" 
                         AutoPostBack="true" 
                            onselectedindexchanged="ddlCurrentCourseRevNo_SelectedIndexChanged">
                            <asp:ListItem Text="--Select One--" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLowerCourseRevNo" runat="server" 
                                    Height="22px" SkinID="ddl250" >
                                    <asp:ListItem Text="--Select One--" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCurrentCourseRevNo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top" align="center">
                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return ValidateLogin();"
                            OnClick="btnShow_Click" />
                    </td>
                </tr>
            </table>
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnRowDataBound="gvMain_RowDataBound"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="oldModuleName" HeaderText="Module Name">
                                    <HeaderStyle Width="42%" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Module Name">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlNew" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="42%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Allow Parity" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign ="Center" />
                                </asp:TemplateField>
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
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged"
                            Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="text-align: right; margin-top: 10px;" id="divSave" runat="server" visible="false">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"  />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="sidetable" cellspacing="1"
        cellpadding="8" id="tblNavLinks" style="width: 95%;">
        <tr>
            <td style="border: 1px solid #2c5070; color: #ffffff; font-weight: bold; background-color: #31597C;"
                width="100%">
                Navigation Links
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#" target="_self">Navgation Link1</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link2</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link3</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link4</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link5</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link6</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link7 </a>
            </td>
        </tr>
    </table>
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
