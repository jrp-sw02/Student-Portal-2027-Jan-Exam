<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CandidateFeedbackView.aspx.cs" Inherits="Admin_CandidateFeedbackView"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Feedback/Suggestions"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="false" />
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
                                            OnClientClick="return validfilter();" Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="User Type"></asp:Label>
                                        <asp:DropDownList ID="ddluserType" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Width="100%" runat="server" Text="Date From"></asp:Label>
                                        <asp:TextBox ID="txtflFromDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="imgdatefrom" TargetControlID="txtflFromDate">
                                        </asp:CalendarExtender>
                                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Date To"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="img1" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
                                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Marked_Read"></asp:Label>
                                        <asp:DropDownList ID="ddlreadonstatus" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by User Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function TestCheckBox() {
            var TargetBaseControl = document.getElementById('<%= gvMain.ClientID %>');
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
        function ValidateForm() {

            //         if (!TestTextBox())
            //             return false;
            if (!TestCheckBox())
                return false;

            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }

        function validfilter() {
            if (!isSelected("<%=ddluserType.ClientID %>", "User Type"))
                return false;
            if (!isBlankDate("<%=txtflFromDate.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtflFromDate.ClientID %>", "Invalid From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtToDate.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtToDate.ClientID %>", "Invalid To Date", "dd-MMM-yyyy"))
                return false;
            var frdate = document.getElementById("<%=txtflFromDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtToDate.ClientID %>").value;
            if (!CompareDates(frdate, todate, "From date should be less then To date", true)) {
                return false;
            }
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label Width="99%" EnableTheming="False" CssClass="error" ID="lblError1" runat="server"></asp:Label>
                        <div align="right">
                            <asp:Button ID="btnPrint" runat="server" Text="Print" />
                            <asp:Button ID="BtnMark" runat="server" Text="Mark As Read" OnClientClick="return Validate_Checkbox('Are you sure you want to mark the selected applications as read!')"
                                OnClick="BtnMark_Click" />
                        </div>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID,UserTypeID"
                                    DataNavigateUrlFormatString="?Key={0}&UserTypeID={1}" DataTextField="userName"
                                    HeaderText="User Name" SortExpression="userName" Target="_self">
                                    <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="12%" DataNavigateUrlFields="ID,UserTypeID"
                                    DataNavigateUrlFormatString="?Key={0}&UserTypeID={1}" DataTextField="usertype"
                                    HeaderText="User Type" SortExpression="usertype" Target="_self">
                                    <HeaderStyle Width="12%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,UserTypeID"
                                    DataNavigateUrlFormatString="?Key={0}&UserTypeID={1}" DataTextField="feedback"
                                    HeaderText="Feedback Recieved" SortExpression="feedback" Target="_self">
                                    <HeaderStyle Width="15%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="18%" DataNavigateUrlFields="ID,UserTypeID"
                                    DataNavigateUrlFormatString="?Key={0}&UserTypeID={1}" DataTextField="suggestions"
                                    HeaderText="Suggestions Recieved" SortExpression="suggestions" Target="_self">
                                    <HeaderStyle Width="18%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="12%" DataNavigateUrlFields="ID,UserTypeID"
                                    DataNavigateUrlFormatString="?Key={0}&UserTypeID={1}" DataTextField="Date" HeaderText="Date"
                                    SortExpression="Date" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="12%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode" runat="server" />
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
            <div align="right">
            <asp:Label ID="lblMark" runat="server" Text="Label"></asp:Label>
                <asp:Button ID="btnMarkOnUpdate" runat="server" Text="Mark As Read" OnClick="btnMarkOnUpdate_Click"  OnClientClick="return ConfirmAction('Are you sure you want to take this action!');"  />
            </div>
            <table class="sample2" runat="server" id="Table1" cellpadding="2" cellspacing="0"
                width="100%">
                <tr class="heading">
                    <td valign="top" colspan="3">
                        <asp:Label ID="lblServiceType" runat="server" SkinID="CaptionLabel" Text="User Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="User Name" Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="User Type"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblState" runat="server" SkinID="CaptionLabel" Text="Feedback/Suggestion Date"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" id="tdUserName" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdUserType" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdSuggDate" runat="server">
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="lblexamcentre" runat="server" SkinID="CaptionLabel" Text="Feedback"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3" id="tdFeedback" runat="server">
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Suggestion"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3" id="tdsuggestion" runat="server">
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="False" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
