<%@ Page Title="Payment Receipt" Debug="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/FullInfo.master"
    CodeFile="CSCResponse.aspx.cs" Inherits="CSCResponse" %>

<%@ Register Src="UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            document.getElementById("<%=btnHome.ClientID %>").style.visibility = "hidden";
            document.getElementById("<%=imPrint.ClientID %>").style.visibility = "hidden";
            window.print();
            document.getElementById("<%=btnHome.ClientID %>").style.visibility = "visible";
            document.getElementById("<%=imPrint.ClientID %>").style.visibility = "visible";
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <uc1:NormalHeader ID="NormalHeader1" runat="server" />
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td width="75%" style="vertical-align: bottom;">
                <asp:Label runat="server" ID="Label1" Text="CSC SPV Payment Receipt of NIELIT Student Service"
                    Style="font-size: 20px; font-weight: bold;"></asp:Label>
            </td>
            <td width="25%">
                <img alt="" class="style1" src="images/csc.jpg" align="right" />
            </td>
        </tr>
    </table>
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" Style="float: right;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div id="divResponse" runat="server" visible="false">
        <table id="tblResponse" width="100%" align="center" class="sample" runat="server"
            style="background-color: #ffffff;">
            <tr>
                <td colspan="2" class="odd1">
                    Demand Note / Payment Details:-
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    Demand Note Number and Date
                </td>
                <td id="tdDemandNumber" runat="server" class="odd" width="70%">
                </td>
            </tr>
            <tr>
                <td class="even" width="30%" style="vertical-align: top;">
                    Payment Description
                </td>
                <td class="even" id="tdPaymentDescription" style="line-height: 1.6" runat="server"
                    width="70%">
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    Payment Mode
                </td>
                <td class="odd" id="tdMode" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="even" width="30%">
                    Transaction Reference Number
                </td>
                <td class="even" id="tdRefNo" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    Transaction Date
                </td>
                <td class="odd" id="tdRefDate" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="even" width="30%">
                    Transaction Amount (in rupees)
                </td>
                <td class="even" id="tdAmount" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    CSC SPV Processing Charges (in rupees)
                </td>
                <td class="odd" id="tdcscamount" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="even" width="30%">
                    Total Payable Amount (in rupees)
                </td>
                <td class="even" id="tdtotalamount" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    Transaction Status
                </td>
                <td class="odd" id="tdStatus" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="odd1">
                    Payee's Detail:-
                </td>
            </tr>
            <tr>
                <td id="tdNameCaption" runat="server" class="odd" width="30%">
                    Candidate Name
                </td>
                <td class="odd" id="tdPayeeName" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td id="tdFnameCaption" runat="server" class="even" width="30%">
                    Father's Name
                </td>
                <td class="even" id="tdPayeeFatherName" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td id="tdMNameCaption" runat="server" class="odd" width="30%">
                    Mother's Name
                </td>
                <td class="odd" id="tdPayeeMotherName" runat="server" width="70%">
                </td>
            </tr>
            <tr id="trextra" runat="server">
                <td id="tdDobCaption" runat="server" class="even" width="30%">
                    &nbsp;
                </td>
                <td class="even" id="tdPayeeDOB" runat="server" width="70%">
                </td>
            </tr>
            <tr runat="server" id="trExam" visible="false">
                <td colspan="2" class="odd" align="left">
                    <b>Note:- <i>Please Attach the payment receipt alongwith your Form and send to the NIELIT
                        Centre as indicated in the application form before last date.</i></b>
                </td>
            </tr>
            <tr runat="server" id="trReg" visible="false">
                <td colspan="2" class="odd" align="left">
                    <b>Note:- <i>Please Attach the payment receipt alongwith your Form and send to the NIELIT
                        Head Office as indicated in the application form before last date.</i></b>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="even" align="right">
                    <asp:Button ID="btnHome" runat="server" Text="Go to Home Page" class="even" OnClick="btnHome_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
