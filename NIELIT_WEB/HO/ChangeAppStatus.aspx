<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ChangeAppStatus.aspx.cs" Inherits="ChangeAppStatus"  Debug="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Change Application Status"></asp:Label>
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
                                            OnClientClick="return ValidateForm();" Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Date From"></asp:Label>
                                        <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="11" Width="200px"></asp:TextBox>
                                        <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Date To"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" Width="200px"></asp:TextBox>
                                        <img id="imgTo" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgTo">
                                        </asp:CalendarExtender>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Candidate Name or Registration Number"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel ID="UpnlBreadCrumb" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function TestCheckBox() {
            var TargetBaseControl = document.getElementById('<%= gvMain1.ClientID %>');
            if (TargetBaseControl != null) {
                //get target child control.
                var TargetChildControl = "chk";
                //get all the control of the type INPUT in the base control.
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                for (var n = 0; n < Inputs.length; ++n)
                    if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked)
                        return true;
            }

            alert('Select at least one checkbox!');
            return false;

        }

        function validateGrid() {
            if (!TestCheckBox())
                return false;

            if (confirm('Are you sure you want to mark this applications as not verified?') == false)
                return false;

            return true;

        }

        function validateGrid1() {
            if (!TestCheckBox())
                return false;

            if (confirm('Are you sure you want to mark this applications as verified?') == false)
                return false;

            return true;

        }
        function ValidateForm() {
            var DateFrom = 0;
            var DateTo = 0;
            DateFrom = document.getElementById("<%=txtDateFrom.ClientID %>").value;
            DateTo = document.getElementById('<%= txtToDate.ClientID %>').value;


            if (DateFrom != "" && DateTo == "") {
                if (!isBlankDate("<%=txtToDate.ClientID %>", "To Date", "dd-MMM-yyyy"))
                    return false;
            }
            if (DateTo != "" && DateFrom == "") {
                if (!isBlankDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                    return false;
            }
            if (DateFrom != "" && DateTo != "") {
                DateFrom = document.getElementById("<%=txtDateFrom.ClientID %>").value;
                DateTo = document.getElementById('<%= txtToDate.ClientID %>').value;
                if (!CompareDates(DateFrom, DateTo, "From date should be less than To Date", true))
                    return false;
            }
            return true;

        }


        var dtgp = "<%= gvMain1.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
        
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGridSummery" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSyncApplData" runat="server" 
                                Text="Sync All Verification Application Data" onclick="btnSyncApplData_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvMain_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="#" />
                            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                            <asp:HyperLinkField DataTextField="LockedCount" DataNavigateUrlFields="CourseID,StatusDateFrom,StatusDateTo"
                                DataNavigateUrlFormatString="?courseID={0}&StatusFromDate={1}&StatusToDate={2}&status=LockedOn"
                                HeaderText="Locked" Target="_self" />
                            <asp:HyperLinkField DataTextField="VerifiedCount" DataNavigateUrlFields="CourseID,StatusDateFrom,StatusDateTo"
                                DataNavigateUrlFormatString="?courseID={0}&StatusFromDate={1}&StatusToDate={2}&status=VerifiedOn"
                                HeaderText="Verified" Target="_self" />
                            <asp:HyperLinkField DataTextField="SyncedCount" DataNavigateUrlFields="CourseID,StatusDateFrom,StatusDateTo"
                                DataNavigateUrlFormatString="?courseID={0}&StatusFromDate={1}&StatusToDate={2}&status=SyncedOn"
                                HeaderText="Synced" Target="_self" />
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="List" runat="server">
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="BtnRevertVerify" runat="server" Text="Mark as not verified(Locked)"
                            OnClientClick="return validateGrid();" OnClick="BtnRevertVerify_Click" />
                        <asp:Button ID="BtnVerify" runat="server" OnClick="BtnVerify_Click" OnClientClick="return validateGrid1();"
                            Text="Verify Detail" ToolTip="Click here to verify Candidate detail" />
                    </td>
                </tr>
            </table>
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain1" runat="server" DataKeyNames="CandidateID" AutoGenerateColumns="False"
                            OnRowDataBound="gvMain1_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="#" />
                                <asp:HyperLinkField DataTextField="CourseName" DataNavigateUrlFields="CourseID,RegNo,StatusDateFrom,StatusDateTo,StatusHead"
                                    DataNavigateUrlFormatString="ChangeApplicationVerification.aspx?courseID={0}&Regno={1}&StatusFromDate={2}&StatusToDate={3}&status={4}"
                                    HeaderText="Course Name" SortExpression="CourseName" Target="_self" />
                                <asp:HyperLinkField DataTextField="CandidateName" DataNavigateUrlFields="CourseID,RegNo,StatusDateFrom,StatusDateTo,StatusHead"
                                    DataNavigateUrlFormatString="ChangeApplicationVerification.aspx?courseID={0}&Regno={1}&StatusFromDate={2}&StatusToDate={3}&status={4}"
                                    HeaderText="Candidate Name" Target="_self" />
                                <asp:HyperLinkField DataTextField="FatherName" DataNavigateUrlFields="CourseID,RegNo,StatusDateFrom,StatusDateTo,StatusHead"
                                    DataNavigateUrlFormatString="ChangeApplicationVerification.aspx?courseID={0}&Regno={1}&StatusFromDate={2}&StatusToDate={3}&status={4}"
                                    HeaderText="Father/Guardian Name" Target="_self" />
                                <asp:HyperLinkField DataTextField="RegNo" DataNavigateUrlFields="CourseID,RegNo,StatusDateFrom,StatusDateTo,StatusHead"
                                    DataNavigateUrlFormatString="ChangeApplicationVerification.aspx?courseID={0}&Regno={1}&StatusFromDate={2}&StatusToDate={3}&status={4}"
                                    HeaderText="Registration No." Target="_self" />
                                <asp:HyperLinkField DataTextField="StatusField" DataNavigateUrlFields="CourseID,RegNo,StatusDateFrom,StatusDateTo,StatusHead"
                                    DataNavigateUrlFormatString="ChangeApplicationVerification.aspx?courseID={0}&Regno={1}&StatusFromDate={2}&StatusToDate={3}&status={4}"
                                    HeaderText="" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy hh:mm}" />
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged1" />
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                </asp:UpdatePanel>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
