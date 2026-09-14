<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="RegistrationPolicy.aspx.cs" Inherits="Admin_RegistrationPolicy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Registration Policy"></asp:Label>
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
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
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBreadCrumb" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {

            if (!isBlank("<%=txtregperiod.ClientID  %>", "Registration Period"))
                return false;
            if (!isNumber("<%=txtregperiod.ClientID  %>", "Registration Period"))
                return false;
            if (!isSelected("<%=ddlregchances.ClientID  %>", "Re-Registration Chances"))
                return false;
            if (!isBlankDate("<%=Txtregpolicyeffectivedate.ClientID %>", "Registration Policy Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=Txtregpolicyeffectivedate.ClientID %>", "Invalid Registration Policy Date", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("<%=ddllanguage.ClientID  %>", "Language"))
                return false;
            if (document.getElementById("<%=txtvalidity.ClientID %>").disabled == false) {
                if (!isBlank("<%=txtvalidity.ClientID  %>", "Re-Registration Period"))
                    return false;
                if (!isNumber("<%=txtvalidity.ClientID  %>", "Re-Registration Period"))
                    return false;
            }
            if (document.getElementById("<%=txtreggape.ClientID %>").disabled == false) {
                if (!isBlank("<%=txtreggape.ClientID  %>", "Re-Registration Gape"))
                    return false;
                if (!isNumber("<%=txtreggape.ClientID  %>", "Re-Registration Gape"))
                    return false;
            }
            return true;
        }

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
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" EnableTheming="false"
                            CssClass="error" Width="99%" Visible="false"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" 
                            Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="18%" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="?Key={0}&ID={1}"
                                    DataTextField="regperiod" HeaderText="Reg.Period(Papers)" SortExpression="regperiod"
                                    Target="_self">
                                    <HeaderStyle Width="18%" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="18%" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="?Key={0}&ID={1}"
                                    DataTextField="reregchance" HeaderText="Re-Reg.Chance" SortExpression="reregchance"
                                    Target="_self">
                                    <HeaderStyle Width="18%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="?Key={0}&ID={1}"
                                    DataTextField="reregperiod" HeaderText="Re-Reg.Period(Papers)" SortExpression="reregperiod"
                                    Target="_self">
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="23%" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="?Key={0}&ID={1}"
                                    DataTextField="rereggap" HeaderText="Re-Reg.Gap(In Months)" SortExpression="rereggap"
                                    Target="_self">
                                    <HeaderStyle Width="23%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="?Key={0}&ID={1}"
                                    DataTextField="EffectiveDateFrom" HeaderText="Effective Date" SortExpression="EffectiveDateFrom"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="2%" />
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
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%" id="tbreregistration"
                runat="server" >
                <tr>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Registration Period(Papers) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Re-Registration Chance &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Registration Policy Effective Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txtregperiod" runat="server" MaxLength="2" SkinID="txt248" onkeypress="checkNumber(this,2,0,event);"></asp:TextBox>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlregchances" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlregchances_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="Txtregpolicyeffectivedate" runat="server" SkinID="txt210"></asp:TextBox>
                        <asp:CalendarExtender ID="Txtregpolicyeffectivedate_CalendarExtender" runat="server"
                            Format="dd-MMM-yyyy" PopupButtonID="img1" TargetControlID="Txtregpolicyeffectivedate">
                        </asp:CalendarExtender>
                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Allowed Language &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                        <asp:Label ID="Lbreregperiod" runat="server" SkinID="CaptionLabel" Text="Re-Registration Period(Papers) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlregchances" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td id="tdlbcoursename" runat="server" valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                        <asp:Label ID="Lbrereggap" runat="server" SkinID="CaptionLabel" Text="Re-Registration Gap(In Month) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlregchances" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                       
                </tr>
                <tr class="even">
                    <td>
                        <asp:DropDownList ID="ddllanguage" runat="server" SkinID="ddl250">
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtvalidity" runat="server" MaxLength="2" SkinID="txt248" onkeypress="checkNumber(this,2,0,event);"></asp:TextBox>
                         </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID ="ddlregchances" EventName="SelectedIndexChanged" /></Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtreggape" runat="server" MaxLength="2" SkinID="txt248" onkeypress="checkNumber(this,2,0,event);"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID ="ddlregchances" EventName="SelectedIndexChanged" /></Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return ValidateLogin();"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
