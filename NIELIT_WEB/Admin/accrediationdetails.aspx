<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="accrediationdetails.aspx.cs" Inherits="accrediationdetails"  Debug="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Accrediation Details"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                            Text="" OnClientClick="return validatefilter()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlcategry" Width="100%" runat="server" 
                                            onselectedindexchanged="ddlcategry_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlcour" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Status"></asp:Label>
                                        <asp:DropDownList ID="ddlsts" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Accrediation Number."
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">

    <%--<ul class="crumbs">
        <li class="first"><a href="adminaccrediatedcenter.aspx" style="z-index: 9;"><span></span>
            Accredited Centers</a></li>
        <li><a href="adminaccrediatedcenter.aspx?key=R101&name=Aishwarya College&contact=9001107701"
            style="z-index: 8;">Aishwarya College</a></li>
        <li><a href="accrediationdetails.aspx" style="z-index: 7;">Accrediation Details</a></li>
        <li id="l1" runat="server" visible="false"><a href="#" style="z-index: 6;"><span
            id="aclink" runat="server"></span></a></li>
    </ul>--%>
    <%--<a href="adminaccrediatedcenter.aspx">Accredited Centers</a>:<a href="adminaccrediatedcenter.aspx?key=R101&name=Aishwarya College&contact=9001107701">Aishwarya College</a>>><a href="accrediationdetails.aspx">Accrediation Details</a>:<a href="#"><span id="aclink" runat="server"></span></a>--%>
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function validatefilter() {
            if (!isSelected("<%=ddlcategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcour.ClientID %>", "Course"))
                return false;
        }
        function ValidateFormFields() {

            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlaccfor.ClientID %>", "Accredition For"))
                return false;
            if (!isBlank("<%=txtaccno.ClientID %>", "Accrediation Number"))
                return false;
            if (!isBlankDate("<%=txteffectivefrom.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txteffectiveto.ClientID %>", "Effective To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txteffectivefrom.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txteffectiveto.ClientID %>", "Effective To Date", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("<%=ddlstatus.ClientID %>", "Accredition For"))
                return false;


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
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#"><HeaderStyle Width="5%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="AccreditationNumber" HeaderText="Accrediation Number" SortExpression="AccreditationNumber"
                                    Target="_self"><HeaderStyle Width="20%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="EffectiveFromDate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective From Date" SortExpression="EffectiveFromDate "
                                    Target="_self"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Right" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="EffectiveToDate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective To Date" SortExpression="EffectiveToDate"
                                    Target="_self"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Right" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Course" SortExpression="Name" Target="_self"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Left" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="SNAME" HeaderText="Status" SortExpression="SNAME" Target="_self"><HeaderStyle Width="40%" /><ItemStyle HorizontalAlign="Left" /></asp:HyperLinkField>
                    <%--  <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Address" HeaderText="Address" SortExpression="Address" Target="_self" />--%>
                                 <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}" DataTextField="Temp_Blocked" HeaderText="Blocked" SortExpression="Temp_Blocked" Target="_self">
                                     <HeaderStyle Width="40%" />
                                     <ItemStyle HorizontalAlign="Left" /></asp:HyperLinkField>
                                <%--  <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Address" HeaderText="Address" SortExpression="Address" Target="_self" />--%>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccreID" runat="server" />
                        <asp:HiddenField ID="hfAccName" runat="server" />
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
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Accrediation No &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers><asp:AsyncPostBackTrigger ControlID="ddlaccfor" EventName="SelectedIndexChanged" /></Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlaccfor" runat="server" SkinID="ddl250">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers><asp:AsyncPostBackTrigger ControlID="ddlaccfor" EventName="SelectedIndexChanged" /></Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtaccno" runat="server" SkinID="txt248" ToolTip="Accredition Number" MaxLength="25"></asp:TextBox>
                    </td>
                    <%-- <td style="width: 33%;" valign="top">
                        
                    </td>--%>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" Text="Effective From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Effective To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txteffectivefrom" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdatefrom" TargetControlID="txteffectivefrom"></asp:CalendarExtender>
                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txteffectiveto" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                        <asp:CalendarExtender ID="txteffectivedate_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdateto" TargetControlID="txteffectiveto"></asp:CalendarExtender>
                        <img id="imgdateto" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlstatus" runat="server" SkinID="ddl250" AutoPostBack="True"  OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
				  <%-- Added 20 June 2019 --%>
                <tr id="withdrawal" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblWithdrawlDate" runat="server" SkinID="CaptionLabel" Text="Withdrawl Date &lt;b class='mandatory'&gt;&lt;/b&gt;"
                           Visible ="False"   Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr id="withdrawal1" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtWithdrawldate" runat="server" SkinID="txt210" MaxLength="11" Visible ="false" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtWithdrawldate_Calendarextender" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdatewithdrawl" TargetControlID="txtWithdrawldate"></asp:CalendarExtender>
                        <img id="imgdatewithdrawl" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <%--till--%>

                 <%-- Added 20 May2020 --%>
                <tr id="blocking" runat ="server">

                    <td style="width: 33%;" valign="top">
                        <asp:CheckBox ID="chkBlocked" runat="server"  Text="Whether Temporarily Blocked" AutoPostBack="true" OnCheckedChanged="chkBlocked_CheckedChanged"
                           Visible ="False"></asp:CheckBox> 
                        <br />
			<asp:Label ID="lblBlockDate" runat="server" SkinID="CaptionLabel" Text="Temporary Block Date &lt;b class='mandatory'&gt;&lt;/b&gt;"
                           Visible ="False"   Width="100%"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtBlockDate" runat="server" SkinID="txt210" MaxLength="11" Visible ="false" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtBlockdate_Calendarextender" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdateblock" TargetControlID="txtBlockdate"></asp:CalendarExtender>
                        <img id="imgdateblock" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <%--till--%>

            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" class="nav" cellspacing="0" cellpadding="0"
        id="tblNavLinks" width="97%" style="width: 97%;">
       <%-- <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="admpaymentdetails.aspx">Payment Detail</a>
            </td>
        </tr>--%>
    </table>
</asp:Content>
