<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="SamplePageHO.aspx.cs" Inherits="SamplePageHO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Contact Detail"></asp:Label>
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
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search By E-Mail Address."
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function Validate() {
            if (!isBlankNumber("<%=Txt_Mobno.ClientID %>", "Mobile No."))
                return false;
            if (!isValidEmail("<%=Txt_MailId.ClientID%>", "Invalid E-Mail ID"))
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
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" HeaderText="Phone No." DataNavigateUrlFields="ID,appid,PhoneNo"
                                    DataNavigateUrlFormatString="?key={0}&key1={1}&PhoneNo={2}" DataTextField="PhoneNo"
                                    SortExpression="PhoneNo" Target="_self">
                                    <HeaderStyle Width="10%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Mobile No." DataTextField="Mobno" DataNavigateUrlFields="ID,appid,PhoneNo"
                                    DataNavigateUrlFormatString="?key={0}&key1={1}&PhoneNo={2}" SortExpression="Mobno">
                                    <HeaderStyle Width="8%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Email" DataTextField="email" DataNavigateUrlFields="ID,appid,PhoneNo"
                                    DataNavigateUrlFormatString="?key={0}&key1={1}&PhoneNo={2}" SortExpression="email">
                                    <HeaderStyle Width="25%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Effective Date" DataTextField="effdate" DataNavigateUrlFields="ID,appid,PhoneNo"
                                    DataNavigateUrlFormatString="?key={0}&key1={1}&PhoneNo={2}" SortExpression="effdate"
                                    DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="12%" />
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
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Phone No."></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption0" runat="server" SkinID="CaptionLabel" Text="Mobile No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lbl_effDate" runat="server" Text="Effective Date" SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPhoneno" runat="server" SkinID="txt248" ToolTip="Phone No." MaxLength="6"
                            onkeypress="checkNumber(this,6,0,event);"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="Txt_Mobno" runat="server" SkinID="txt248" MaxLength="10" onkeypress="checkNumber(this,10,0,event);"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="Txt_EffDate" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Email &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" colspan="3">
                        <asp:TextBox ID="Txt_MailId" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return Validate();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
            <div id="DivHistory" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="UPanelHistory" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <table cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <strong>Candidate Contact Detail History :-</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblError2" runat="server" CssClass="error" EnableTheming="False" Visible="False"
                                        Width="99%"></asp:Label>
                                    <asp:GridView ID="GridViewOld" runat="server" OnSorting="GridViewOld_Sorting" AutoGenerateColumns="False"
                                        OnRowDataBound="GridViewOld_RowDataBound" Width="100%">
                                        <Columns>
                                            <asp:BoundField HeaderText="#">
                                                <ItemStyle Width="4%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Phone No." DataField="PhoneNo" SortExpression="PhoneNo">
                                                <ItemStyle Width="12%" HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Mobile No." DataField="Mobno" SortExpression="Mobno">
                                                <ItemStyle Width="8%" HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Email" DataField="email" SortExpression="email">
                                                <ItemStyle Width="25%" HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Effective From Date" SortExpression="effdate">
                                                <ItemTemplate>
                                                    <%# Eval("effdate", "{0:dd-MMM-yyyy}")%></ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="16%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Visible="False" />
                                    </asp:GridView>
                                    <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChangedOld" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
