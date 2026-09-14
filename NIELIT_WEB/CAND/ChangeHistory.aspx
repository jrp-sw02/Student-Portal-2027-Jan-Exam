<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ChangeHistory.aspx.cs" Inherits="ChangeHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/Address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="History Detail"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <%-- <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />--%>
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Request Type"></asp:Label>
                                        <asp:DropDownList ID="ddlFilter1" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                            <asp:ListItem Value="2">Contact Details</asp:ListItem>
                                            <asp:ListItem Value="3">Corespondence Address Details</asp:ListItem>
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
  <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Cycle Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
 
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

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
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                       
                        <asp:GridView ID="gvMain" runat="server"  OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" Visible="true" 
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#" />
                                <asp:HyperLinkField HeaderText="Request Type" SortExpression="RequestType" 
                                    Target="_self" DataTextField="RequestType" />
                                <asp:BoundField DataField="RequestNo" HeaderText="Request No." 
                                    SortExpression="RequestNo" />
                                <asp:TemplateField HeaderText="Request Date" SortExpression="UserType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Convert.ToDateTime(Eval("RequestDate")).ToString("dd-MMM-yyyy")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmRequestStatus) Eval("Status")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                            </Columns>
                            <PagerSettings Visible="true" />
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
            <div id="d1" runat="server" visible="false">
                <table class="sample2" cellpadding="0" cellspacing="1" width="100%" runat="server">
                    <tr class="heading">
                        <td colspan="3">
                            Personal Detail
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 33%;" valign="top">
                            Candidate Name
                        </td>
                        <td style="width: 33%;" valign="top">
                            Father&#39;s Name
                        </td>
                        <td style="width: 33%;" valign="top">
                            Mother&#39;s Name
                        </td>
                    </tr>
                    <tr class="even">
                        <td style="width: 33%;" valign="top">
                            <asp:TextBox runat="server" ID="txtcandname" SkinID="txt248" Enabled="false">Punit Babel</asp:TextBox>
                        </td>
                        <td style="width: 33%;" valign="top">
                            <asp:TextBox runat="server" ID="txtfather" SkinID="txt248" Enabled="false">Mr. Basantilal Babel</asp:TextBox>
                        </td>
                        <td style="width: 33%;" valign="top">
                            <asp:TextBox runat="server" ID="txtmother" SkinID="txt248" Enabled="false">Shati Devi Babel</asp:TextBox>
                        </td>
                    </tr>
                    <tr class="heading">
                        <td colspan="3">
                            Correspondence Details
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Mobile No.
                        </td>
                        <td valign="top">
                            Telephone No.
                        </td>
                        <td valign="top">
                            Email
                        </td>
                    </tr>
                    <tr class="even">
                        <td style="width: 33%;" valign="top">
                            <asp:TextBox ID="txtcontact" SkinID="txt248" runat="server">9828043637</asp:TextBox>
                        </td>
                        <td valign="top" style="height: 25%;">
                            <asp:TextBox ID="txttelephone" runat="server" SkinID="txt248">0292-243456</asp:TextBox>
                        </td>
                        <td style="height: 25%;" valign="top">
                            <asp:TextBox ID="txtemail" runat="server" SkinID="txt248">pbabel@gmail.com</asp:TextBox>
                        </td>
                    </tr>
                </table>
                <uc1:Address ID="Address1" runat="server" />
            </div>
            <table class="sample2" cellpadding="0" cellspacing="1" width="100%" runat="server"
                id="t2" visible="false">
                <tr class="heading">
                    <td colspan="3">
                        Existing Information
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        Candidate Name
                    </td>
                    <td style="width: 33%;" valign="top">
                        Father&#39;s Name
                    </td>
                    <td style="width: 33%;" valign="top">
                        Mother&#39;s Name
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox runat="server" ID="TextBox1" SkinID="txt248" Enabled="false">Punit Babel</asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox runat="server" ID="TextBox2" SkinID="txt248" Enabled="false">Mr. Basantilal Babel</asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox runat="server" ID="TextBox4" SkinID="txt248" Enabled="false">Shanti Devi Babel</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        D.O.B.(dd/mm/yyyy)
                    </td>
                    <td style="width: 33%;" valign="top">
                        Contact No.
                    </td>
                    <td style="width: 33%;" valign="top">
                        Email
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox runat="server" ID="txtdob" SkinID="txt248" Enabled="false">12/07/1985</asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox runat="server" ID="txtcontacts" SkinID="txt248" Enabled="false">9828042637</asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox runat="server" ID="TextBox3" SkinID="txt248" Enabled="false">pbabel@gmail.com</asp:TextBox>
                    </td>
                </tr>
                <tr class="heading">
                    <td colspan="3">
                        Update Information
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Candidate Name
                    </td>
                    <td valign="top">
                        Father&#39;s Name
                    </td>
                    <td valign="top">
                        Mother&#39;s Name
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtname" SkinID="txt248" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtfathers" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtmothers" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        D.O.B.(dd/mm/yyyy)
                    </td>
                    <td valign="top">
                        Contact No.
                    </td>
                    <td valign="top">
                        Email
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtdobs" SkinID="txt210" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtcontactss" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtemails" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <div style="height: 6px;">
    </div>
    <uc2:SideLink ID="Sidelink" runat="server" />
    <uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
