<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DownloadSettlementFile.aspx.cs" Inherits="DownloadSettlementFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download Settlememt File"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            var curdate = '<%= DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")%>'; 
            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "Date of Online Transaction", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid Date of Online Transaction", "dd-MMM-yyyy"))
                return false;
            var FromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            if (!CompareDates(FromDate, curdate, " Date of Online Transaction should be less than todays Date", true))
                return false;
            return true;
        }
    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div class="box" id="DivSearch" runat="server">
        <table class="sample3" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr class="gdrow1">
                <td style="width: 20%;" valign="top">
                    <asp:Label ID="lblDateFrom" runat="server" SkinID="CaptionLabel" Text="Date of Online Transaction &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td style="width: 25%;" valign="top">
                    <asp:TextBox ID="txtDateFrom" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                    <img runat="server" id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px;
                        height: 22px; vertical-align: top;" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                        Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                    </asp:CalendarExtender>
                </td>
                <td align="center" style="width: 33%;" align="center">
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return ValidateLogin();"
                        OnClick="btnShow_Click" />
                </td>
            </tr>
        </table>
    </div>
    <table id="tblShow" visible="false" runat="server" border="0" cellpadding="3" cellspacing="1"
        class="sample3" style="width: 100%; text-align: left">
        <tr class="head1">
            <td width="60%">
                Online Transaction Status
            </td>
            <td width="40%">
                Number of Applications
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                Total Number of Transactions
            </td>
            <td>
                <asp:Label runat="server" ID="lbltotal" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                Failed Transactions
            </td>
            <td>
                <asp:Label runat="server" ID="lblFailed" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                Successfull Transactions
            </td>
            <td>
                <asp:Label runat="server" ID="lblSuccessfull" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                Success and Demand Note Paid
            </td>
            <td>
                <asp:Label runat="server" ID="lblPaymentRecvd" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                Success but Demand Note not Paid(Refundable)
            </td>
            <td>
                <asp:Label runat="server" ID="lblPending" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                Success, Demand Note Paid and not Verified/ not Reconciled
            </td>
            <td>
                <asp:Label runat="server" ID="lblPaidNotVerified" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                Success, Demand Note Paid and Verified/Reconciled
            </td>
            <td>
                <asp:Label runat="server" ID="lblPaidVerified" Text=""></asp:Label>
            </td>
        </tr>
        <tr runat="server" visible="false" id="trMsg">
            <td colspan="2">
                <asp:Label ID="lblMsg" runat="server" CssClass="error" EnableTheming="false" Width="99%"></asp:Label>
            </td>
        </tr>
    </table>
    <div id="divDownload" runat="server" style="text-align: right; margin-top: 10px;
        margin-bottom: 6px;">
        <asp:Button ID="btnDownload" runat="server" Text="Download Settlement File" Visible="False"
            OnClick="btnDownload_Click" />
        <asp:Button ID="btnRefund" runat="server" Text="Download Refund File" Visible="False"
            OnClick="btnRefund_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="false" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
