<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CSCDayWisePaymentReport.aspx.cs" Inherits="Common_CSCDayWisePaymentReport" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function validateFormFields() {

            if (!isDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;

            var PayStatusId;
            if (!isSelected("<%=ddlpaymentstatus.ClientID %>", "Payment Status"))
                PayStatusId = 0;
            else
                PayStatusId = document.getElementById('<%=ddlpaymentstatus.ClientID %>').value;
            var curdate = new Date().format("dd-MMM-yyyy");
            var PayFromDate = document.getElementById('<%=txttDateFrom.ClientID %>').value;
            if (!CompareDates(PayFromDate, curdate, "Payment from date should be less than todays Date", true))
                return false;
            var PayToDate = document.getElementById('<%=txtDateto.ClientID %>').value;
            if (!CompareDates(PayToDate, curdate, "Payment To Date should be less than todays Date", true))
                return false;

            var url = "../HO/Rpt/CSCDayWisePayment.aspx?PayModeId=5&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&PayStatusId=" + PayStatusId;
            window.open(url);
            return false;
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="CSC Day Wise Payment Report" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Payment Status"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr class="even">
            <td class="style1">
                <asp:TextBox ID="Txtpaymentmode" runat="server" Height="22px" SkinID="ddl250" ReadOnly="true"
                    Text="CSC-SPV">               
                </asp:TextBox>
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlpaymentstatus" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--All--</asp:ListItem>
                    <asp:ListItem Value="S">Success</asp:ListItem>
                    <asp:ListItem Value="F">Failed</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Payment From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Payment To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date From"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                </asp:CalendarExtender>
                <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
            </td>
            <td>
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return validateFormFields()" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
