<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DemandNoteStatusheadOffice.aspx.cs" Inherits="DemandNoteStatusheadOffice" Debug="True"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>


<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Demand Note Status"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />
    <asp:panel runat="server" id="pnlFilter" visible="true">
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
                                            Text="" OnClientClick="return ValidateLogin()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseCategoryFilter" Width="100%" runat="server" 
                                            AutoPostBack="True" onselectedindexchanged="ddlCourseCategoryFilter_SelectedIndexChanged"
                                            >
                                            <asp:ListItem>--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseFilter" Width="100%" runat="server" 
                                            AutoPostBack="True" onselectedindexchanged="ddlCourseFilter_SelectedIndexChanged"
                                            >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblApplicationType" Width="100%" runat="server" Text="Application Type"></asp:Label>
                                        <asp:DropDownList ID="ddlAppType" Width="100%" runat="server" 
                                            AutoPostBack="True" onselectedindexchanged="ddlAppType_SelectedIndexChanged">
                                            <asp:ListItem>--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="excycle" visible="false">
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:DropDownList ID="ddlExamCycle" Width="100%" runat="server" 
                                            AutoPostBack="true" 
                                            onselectedindexchanged="ddlExamCycle_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:DropDownList ID="ddlExamYear" Width="100%" runat="server" 
                                            AutoPostBack="true" onselectedindexchanged="ddlExamYear_SelectedIndexChanged"
                                            >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter4" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:DropDownList ID="ddlExamName" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Status"></asp:Label>
                                        <asp:DropDownList ID="ddlpStatus" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Payment Mode"></asp:Label>
                                        <asp:DropDownList ID="ddlPaymentMode" Width="100%" runat="server">
                                            <asp:ListItem>--All--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <%--<tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Payment Status"></asp:Label>
                                        <asp:DropDownList ID="ddlPaymentStatus" Width="100%" runat="server">
                                            <asp:ListItem>--All--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                            </table>
                        </contenttemplate>
                        <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ucSearchBar" EventName="LnkBtnGO" />
                        </triggers>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Demand Note Number/Candidate Name/Institute Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" AutoCompleteContextKey="1" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <asp:updatepanel enableviewstate="true" id="upBread" updatemode="Conditional" runat="server">
        <contenttemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </contenttemplate>
    </asp:updatepanel>
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
        function ValidateLogin() {
            if (!isSelected("<%=ddlCourseCategoryFilter.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseFilter.ClientID %>", "Course"))
                return false;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            var ec = document.getElementById("<%=ddlAppType.ClientID %>").value;
            if (ec == 2) {
                if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycel"))
                    return false;
                if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                    return false;
            }
            else {
                if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                    return false;
            }
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            return true;

        }
        function OpenWindow() {
            var demandNoteID = "";
            if (document.getElementById('<%=hfDemandNoteID.ClientID %>').value != "") {
                demandNoteID = document.getElementById('<%=hfDemandNoteID.ClientID %>').value;
                window.open("../HO/Rpt/CandidateDetailReport.aspx?DemandNoteId=" + demandNoteID);
                return false;
            }

        }
    </script>
    <asp:multiview id="mltvTab" runat="server" activeviewindex="0">
        <asp:view id="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:updatepanel enableviewstate="true" id="uPnlGrid" updatemode="Conditional" runat="server">
                    <contenttemplate>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="DemandNo" HeaderText="Number" SortExpression="DemandNo" Target="_self">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                 <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                     DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                     DataTextField="DemandDate" HeaderText="Date" SortExpression="DemandDate" Target="_self"
                                     DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="Dtype" HeaderText="Payee Type" SortExpression="Dtype" Target="_self"
                                    DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="pName" HeaderText="Payee Name" SortExpression="pName" Target="_self"
                                    DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="PaymentMode" HeaderText="Payment Mode" SortExpression="PaymentMode"
                                    Target="_self" >
                                    <ItemStyle Width="14%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="PaymentStatus" HeaderText="Payment Status" SortExpression="PaymentStatus"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="Amount" HeaderText="Amount" SortExpression="Amount" Target="_self">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>


                                <asp:HyperLinkField
                                    DataTextField ="PaymentGateway"
                                    DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    HeaderText="Payment Gateway"
                                    SortExpression="PaymentGateway"
                                    Target="_self"
                                    >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                           <%--     <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,Coursecat,courseIDs,ExamYears,ExamNames,Dtype,PaymentStatusID,AppId"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&Coursecat={2}&courseIDs={3}&ExamYears={4}&ExamNames={5}&Dtype={6}&PaymentStatus={7}&AppId={8}"
                                    DataTextField="PaymentGateway" HeaderText="Payment Gateway" SortExpression="Amount" Target="_self">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>--%>
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
            <div id="btnPay" style="text-align: right;">
                <asp:button visible="false" id="btnpaynow" runat="server" text="Pay Now" onclick="btnpaynow_Click" />
                <asp:button id="btnPrint" runat="server" text="Print" visible="false" onclientclick="return OpenWindow();" />
                <asp:hiddenfield id="hfDemandNoteID" runat="server" />
                <asp:hiddenfield id="hfAppID" runat="server" />
            </div>
            <table class="sample3" id="tblprint" style="width: 100%; text-align: left" border="0"
                cellpadding="3" cellspacing="1">
                <tr class="head1">
                    <td align="left" colspan="2">
                        Demand Note Details
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Demand Note Number
                    </td>
                    <td>
                        <asp:label id="lblDemandNoteNo" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Demand Note Date
                    </td>
                    <td>
                        <asp:label id="lblDemandNoteDate" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Amount
                    </td>
                    <td>
                        <asp:label id="lblAmount" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Demand Note Type
                    </td>
                    <td>
                        <asp:label id="lblDemandNoteType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Application Type
                    </td>
                    <td>
                        <asp:label id="lblApplType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Fee Type
                    </td>
                    <td>
                        <asp:label id="lblFeeType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Payment Mode
                    </td>
                    <td>
                        <asp:label id="lblPaymentMode" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Payment Status
                    </td>
                    <td>
                        <asp:label id="lblPaymentStatus" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="head1" runat="server" id="lbltd" visible="false">
                    <td align="left" colspan="2">
                        Transaction Details
                    </td>
                </tr>
                <tr class="gdrow1" runat="server" id="lbldno" visible="false">
                    <td width="40%" id="ddno" runat="server">
                        Demand Draft Number
                    </td>
                    <td>
                        <asp:label id="lblddno" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1" runat="server" id="lblddate" visible="false">
                    <td width="40%" id="dddate" runat="server">
                        Demand Draft Date
                    </td>
                    <td>
                        <asp:label id="lbldddate" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1" runat="server" id="lblbname" visible="false">
                    <td width="40%" id="ddbank" runat="server">
                        Bank Name
                    </td>
                    <td>
                        <asp:label id="lblBank" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1" runat="server" id="lblVdate" visible="false">
                    <td width="40%" id="ddveri" runat="server">
                        Date of Demand Draft Verification
                    </td>
                    <td>
                        <asp:label id="LblVarificationDate" runat="server"></asp:label>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnSave" visible="false" onclientclick="return ValidateLogin();"
                    runat="server" text="Save" onclick="SaveRecord" />
                <asp:button id="btnCancel" visible="false" runat="server" text="Cancel" onclick="btnCancel_Click" /></div>
        </asp:view>
    </asp:multiview>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
