<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransactionDetails.aspx.cs" Inherits="HO_TransactionDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script language="javascript" type="text/javascript">
    function printwindow() {
        window.print();
        return false;
    }
</script>
<body style="background-color: White;">
    <form id="form1" runat="server">
    <div style="text-align:center">
       
    <table style="text-align: left" class="sample3" border="0" cellpadding="3" cellspacing="1"
        width="900px" align="center">
        <tr>
        <td colspan="2">
        <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
            ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" Style="float: right;" />&nbsp;
        </td>
        </tr>
        <tr class="head1">
            <td colspan="2" align="center">
                <asp:Label ID="lblTransactionDetail" runat="server" Text="Transaction Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrName" runat="server">
            <td style="width: 25%">
                <asp:Label ID="lblTransactionNoHead" runat="server" Text="Transaction Number"></asp:Label>
            </td>
            <td >
                <asp:Label ID="lblTransactionNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="TrFatherName" runat="server">
            <td style="width: 25%">
                <asp:Label ID="LblTransactionDateHead" runat="server" Text="Transaction Date"></asp:Label>
            </td>
            <td >
                <asp:Label ID="lblTransactionDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" >
            <td style="width: 25%">
                <asp:Label ID="lblTransaction" runat="server" Text="Response Status"></asp:Label>
&nbsp;</td>
            <td >
                <asp:Label ID="lblResponseStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 25%">
                <asp:Label ID="Label4" runat="server" Text="Transaction Amount"></asp:Label>
            </td>
            <td align="left" >
                <asp:Label ID="lblTransactionAmt" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 25%">
                DemandNote Number
            </td>
            <td >
                <asp:Label ID="lblDemandNoteNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 25%">
                DemandNote Date
            </td>
            <td >
                <asp:Label ID="lblDemandNoteDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 25%">
                DemandNote Amount</td>
            <td >
                <asp:Label ID="lblDemandNoteAmt" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 25%">
                Application Type</td>
            <td >
                <asp:Label ID="lblApplicationType" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 25%">
                Application Number</td>
            <td >
                <asp:Label ID="lblApplicationNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 25%">
                Application Date</td>
            <td >
                <asp:Label ID="lblApplicationDate" runat="server"></asp:Label>
            </td>
        </tr>
        
        <tr class="gdalternate1">
            <td style="width: 25%">
                Application Status</td>
            <td >
                <asp:Label ID="lblApplicationStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 25%">
                Fee Type
            </td>
            <td >
                <asp:Label ID="lblFeeType" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" >
            <td style="width: 25%">
                Payment Status</td>
            <td >
                <asp:Label ID="lblDemandNoteStatus" runat="server"></asp:Label>
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
