<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="LoadsProcess.aspx.cs" Inherits="LoadsProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function abc() {
            alert("Registration Number Allocated");
            return false;
        }
    </script>
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
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Batch Processing"></asp:Label>
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
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUserType" Width="100%" runat="server" Text="User Type"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchUserType" Width="100%" runat="server">
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
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <ul class="crumbs">
        <li class="first"><a href="LoadsProcess.aspx" style="z-index: 9;"><span></span>Batch
            Processing</a></li>
        <li id="liedit" runat="server" visible="false"><a href="#" style="z-index: 8;"><span
            id="rlink" runat="server"></span></a></li>
        <%--<li><a href="admcertiexamdetail.aspx" style="z-index:7;"><span></span>Exam Details</a></li>
    <li><a href="admcertiexamdetail.aspx?key=January" style="z-index:6;"><span></span>January</a></li>
    <li><a href="admexamdetail.aspx?key=January 2010" style="z-index:5;"><span></span>Exam Session</a></li>
    <li><a href="#" style="z-index:4;"><span></span>Time Table</a></li>--%>
    </ul>
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
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="UserID,OrganizationID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" Visible="False">
                            <Columns>
                                <asp:BoundField HeaderText="#" />
                                <asp:HyperLinkField DataNavigateUrlFields="UserID,OrganizationID" DataNavigateUrlFormatString="?Key={0}&OrgId={1}"
                                    DataTextField="LoginID" HeaderText="Login ID" SortExpression="LoginID" Target="_self" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:TemplateField HeaderText="User Type" SortExpression="UserType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" Text='<%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.URM.UserType)Eval("UserType"))  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pwd Expiry Date" SortExpression="PasswordExpiryDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("PasswordExpiryDate")).ToString("dd-MMM-yyyy")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Login Status" SortExpression="HasLoginAccess">
                                    <ItemTemplate>
                                        <asp:Label ID="lbStatus" runat="server" Text='<%# Convert.ToBoolean(Eval("HasLoginAccess").ToString())==true? "Enabled": "Disabled" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <table width="100%">
                            <tr>
                                <td colspan="6">
                                    <table class="gdbody" cellspacing="1" cellpadding="4" id="cphContents_gvMain" width="100%">
                                        <tr class="gdheader">
                                            <th scope="col">
                                                Batch No.
                                            </th>
                                            <th scope="col">
                                                Course Name
                                            </th>
                                            <th scope="col" style="width: 20%;">
                                                Application Received
                                            </th>
                                            <th scope="col" class="style2">
                                                &nbsp;Date of Receiveing
                                            </th>
                                            <th class="style2" scope="col">
                                                Status
                                            </th>
                                        </tr>
                                        <tr class="gdrow">
                                            <td>
                                                <a href="LoadsProcess.aspx?key=R/BTH/101">R/BTH/101</a>
                                            </td>
                                            <td>
                                                O Level
                                            </td>
                                            <td align="right">
                                                50
                                            </td>
                                            <td align="center">
                                                01-Jan-2012
                                            </td>
                                            <td>
                                                Pending
                                            </td>
                                        </tr>
                                        <tr class="gdalternate">
                                            <td>
                                                <a href="LoadsProcess.aspx?key=R/BTH/102">R/BTH/102</a>
                                            </td>
                                            <td>
                                                A Level
                                            </td>
                                            <td align="right">
                                                45
                                            </td>
                                            <td align="center">
                                                05-Jan-2012
                                            </td>
                                            <td>
                                                Pending
                                            </td>
                                        </tr>
                                        <tr class="gdrow">
                                            <td>
                                                <a href="LoadsProcess.aspx?key=R/BTH/103">R/BTH/103</a>
                                            </td>
                                            <td>
                                                B Level
                                            </td>
                                            <td align="right">
                                                44
                                            </td>
                                            <td align="center">
                                                07-Jan-2012
                                            </td>
                                            <td>
                                                Processed
                                            </td>
                                        </tr>
                                        <tr class="gdalternate">
                                            <td>
                                                <a href="LoadsProcess.aspx?key=R/BTH/104">R/BTH/104</a>
                                            </td>
                                            <td>
                                                C Level
                                            </td>
                                            <td align="right">
                                                23
                                            </td>
                                            <td align="center">
                                                10-Jan-2012
                                            </td>
                                            <td>
                                                Processed
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
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
            <%--<table class="sample2" cellpadding="0" cellspacing="1" width="100%">
                <tr class="heading">
                    <td colspan="6">
                        Batch Detail
                    </td>
                </tr>
                <tr>
                    <td>
                        Batch No.
                    </td>
                    <td>
                        :&nbsp;
                    </td>
                    <td>
                        R/BTH/101
                    </td>
                    <td>
                        Course Name
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        O Level
                    </td>
                </tr>
                <tr>
                    <td>
                        Application Received
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        50
                    </td>
                    <td>
                        Status
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        Processing
                    </td>
                </tr>
            </table>--%>
            <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
                <tr class="heading">
                    <td colspan="3">
                        Batch Detail
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        Batch No. : R/BTH/101
                    </td>
                    <td style="width: 33%;" valign="top">
                        Date Of Receiveing :&nbsp; 01-Jan-2012
                    </td>
                    <td style="width: 33%;" valign="top">
                        Course : O Level
                    </td>
                </tr>
            </table>
            <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
                <tr class="heading">
                    <td colspan="2">
                        Batch Processing Details
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%;" valign="top">
                        Application Received :
                        <asp:LinkButton ID="hlkreceived" runat="server" onclick="hlkreceived_Click">50</asp:LinkButton>
                    </td>
                    <td style="width: 50%;" valign="top">
                        Application Processed :
                        <asp:LinkButton ID="hlkprocess" runat="server" onclick="hlkprocess_Click">00</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%;" valign="top">
                        Application Duplicate :
                        <asp:LinkButton ID="hlkduplicate" runat="server" onclick="hlkduplicate_Click">00</asp:LinkButton>
                    </td>
                    <td style="width: 50%;" valign="top">
                        Application Rejected&nbsp;&nbsp; :
                        <asp:LinkButton ID="hlkreject" runat="server" onclick="hlkreject_Click">00</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <table class="sample2" cellpadding="0" cellspacing="1" width="100%" id="tblhead"
                runat="server" visible="false">
                <tr class="heading">
                    <td colspan="3">
                        Application Detail
                    </td>
                </tr>
            </table>
            <table class="gdbody" cellspacing="1" cellpadding="4" id="tbldata" width="100%" runat="server"
                visible="false">
                <tr class="gdheader">
                    <th scope="col">
                        #
                    </th>
                    <th scope="col">
                        Application No
                    </th>
                    <th scope="col">
                        Candidate Name
                    </th>
                    <th scope="col">
                        Application Date
                    </th>
                    <th scope="col">
                        Fathers&#39;s Name</th>
                    <th class="style1" scope="col">
                        D.O.B.</th>
                    <th scope="col">
                        &nbsp;Accrediated Center
                    </th>
                    <th scope="col">
                        &nbsp;
                    </th>
                </tr>
                <tr class="gdrow">
                    <td align="right">
                        1
                    </td>
                    <td align="right">
                       <a href="hostudentpreview.aspx?no=1001">1001</a> 
                    </td>
                    <td>
                        Kuldeep
                    </td>
                    <td align="center">
                        15-Jan-2012
                    </td>
                    <td>
                        Mr. Ganesh Jani</td>
                    <td class="style1">
                        05-Jan-1985
                    </td>
                    <td>
                        Aishwarya College Udaipur
                    </td>
                    <td>
                        <input id="Checkbox7" type="checkbox" />
                    </td>
                </tr>
                <tr class="gdalternate">
                    <td align="right">
                        2
                    </td>
                    <td align="right">
                        <a href="hostudentpreview.aspx?no=1001">1002</a>
                    </td>
                    <td>
                        Punit
                    </td>
                    <td align="center">
                        16-Jan-2012
                    </td>
                    <td>
                        Mr. Paresh Babel</td>
                    <td class="style1">
                        04-Jan-1982
                    </td>
                    <td>
                        Krishna College Udaipur
                    </td>
                    <td>
                        <input id="Checkbox8" type="checkbox" />
                    </td>
                </tr>
                <tr class="gdrow">
                    <td align="right">
                        3
                    </td>
                    <td align="right">
                        <a href="hostudentpreview.aspx?no=1001">1003</a>
                    </td>
                    <td>
                        Suresh
                    </td>
                    <td align="center">
                        17-Jan-2012
                    </td>
                    <td>
                        Mr. Pritesh Jain</td>
                    <td class="style1">
                        02-Jan-1984
                    </td>
                    <td>
                        Akc Udaipur
                    </td>
                    <td>
                        <input id="Checkbox9" type="checkbox" />
                    </td>
                </tr>
                <tr class="gdalternate">
                    <td align="right">
                        4
                    </td>
                    <td align="right">
                        <a href="hostudentpreview.aspx?no=1001">1004</a>
                    </td>
                    <td>
                        Sumit
                    </td>
                    <td align="center">
                        18-Jan-2012
                    </td>
                    <td align="left">
                        Mr. Ganesh</td>
                    <td align="left" class="style1">
                        01-Jan-1985
                    </td>
                    <td align="left">
                        Akc Udaipur
                    </td>
                    <td>
                        <input id="Checkbox10" type="checkbox" />
                    </td>
                </tr>
                <tr class="gdrow" id="tr1" runat="server" visible="false">
                    <td align="right">
                        5</td>
                    <td align="right">
                        <a href="hostudentpreview.aspx?no=1001">1005</a>
                    </td>
                    <td>
                        Rajesh</td>
                    <td align="center">
                        18-Jan-2012
                    </td>
                    <td align="left">
                        Mr. Pritesh Jain</td>
                    <td align="left" class="style1">
                        05-Jan-1985
                    </td>
                    <td align="left">
                        Akc Udaipur
                    </td>
                    <td>
                        <input id="Checkbox11" type="checkbox" />
                    </td>
                </tr>
                <tr class="gdalternate" id="tr2" runat="server" visible="false">
                    <td align="right">
                        6</td>
                    <td align="right">
                        <a href="hostudentpreview.aspx?no=1001">1006</a>
                    </td>
                    <td>
                        Ganesh</td>
                    <td align="center">
                        18-Jan-2012
                    </td>
                    <td align="left">
                        Mr. Ganesh</td>
                    <td align="left" class="style1">
                        04-Jan-1982
                    </td>
                    <td align="left">
                        Akc Udaipur
                    </td>
                    <td>
                        <input id="Checkbox12" type="checkbox" />
                    </td>
                </tr>
                <tr class="gdrow" id="tr3" runat="server" visible="false">
                    <td align="right">
                        7</td>
                    <td align="right">
                        <a href="hostudentpreview.aspx?no=1001">1007</a>
                    </td>
                    <td>
                        Pritesh</td>
                    <td align="center">
                        18-Jan-2012
                    </td>
                    <td align="left">
                        Mr. Pritesh Jain</td>
                    <td align="left" class="style1">
                        05-Jan-1983
                    </td>
                    <td align="left">
                        Akc Udaipur
                    </td>
                    <td>
                        <input id="Checkbox13" type="checkbox" />
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnfduplicate" runat="server" Text="Find Duplicate" />
                <asp:Button ID="btnVerify" runat="server" Text="Verify" />
                <asp:Button ID="btnReject" runat="server" Text="Reject" />
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" class="nav" cellspacing="0" cellpadding="0"
        id="tblNavLinks" width="97%" style="width: 97%;">
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="../Sticker.aspx" target="_blank">Print</a>
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
        .style1
        {
            width: 93px;
        }
    </style>
</asp:Content>
