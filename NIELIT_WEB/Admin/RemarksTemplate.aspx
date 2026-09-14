<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemarksTemplate.aspx.cs"
    Inherits="Admin_RemarksTemplate" MasterPageFile="~/MasterPages/main.master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Remarks"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <asp:panel runat="server" id="pnlFilter" visible="false">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:updatepanel enableviewstate="true" rendermode="Inline" id="filterPnal_upnlFilter"
                        updatemode="Conditional" runat="server">
                        <contenttemplate>
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
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler1" Width="100%" runat="server" Text="Remrks"></asp:Label>
                                        <asp:DropDownList ID="ddlremarks" Width="100%" runat="server">
                                            <asp:ListItem>--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </contenttemplate>
                    </asp:updatepanel>
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
    </asp:panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by  Cast Category Name or Code"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isBlank("<%=Txtremarks.ClientID %>", "Remarks"))
                return false;
            if (!isBlank("<%=Txtdisplayorder.ClientID %>", "Display Order"))
                return false;
            if (!isNumber("<%=Txtdisplayorder.ClientID %>", "Display Order"))
                return false;
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
    <asp:multiview id="mltvTab" runat="server" activeviewindex="0">
        <asp:view id="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:updatepanel enableviewstate="true" id="uPnlGrid" updatemode="Conditional" runat="server">
                    <contenttemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 100px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">                                  
                                    <HeaderStyle Width="2%" />                                    
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="remarkname" HeaderText="Remarks" SortExpression="remarkname"
                                    Target="_self" />
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </contenttemplate>
                </asp:updatepanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:updatepanel rendermode="Inline" id="uPnlNavigation" updatemode="Conditional"
                    runat="server">
                    <contenttemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </contenttemplate>
                </asp:updatepanel>
            </div>
        </asp:view>
        <asp:view id="New" runat="server">
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="lblCategory" runat="server" skinid="CaptionLabel" text="Remarks Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            width="100%"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:textbox id="Txtremarks" runat="server" skinid="txt756" maxlength="230" 
                            onkeypress="return isNumberKey(event);"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top" colspan="3">
                        <asp:label id="lblDownloadType" runat="server" skinid="CaptionLabel" text="Display Order &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top" colspan="3">
                        <asp:textbox id="Txtdisplayorder" runat="server" skinid="txt756" maxlength="2" onkeypress="checkNumber(this,2,0,event);">
                        </asp:textbox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnSave" onclientclick="return ValidateLogin();" runat="server" text="Save"
                    onclick="SaveRecord" />
                <asp:button id="btnCancel" runat="server" text="Cancel" onclick="btnCancel_Click" /></div>
        </asp:view>
    </asp:multiview>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
