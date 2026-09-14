<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentAcknowledgementReceipt.aspx.cs" Inherits="PaymentAcknowledgementReceipt" %>

<%--<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>    
    <style type="text/css">
        .auto-style5
        {
            text-align: right; 
            font-size:medium;
            border:1px solid black;
        }       
        .auto-style6
        {
            height: 50px;
        }     
        /*table, th, td {
  border: 1px solid black;*/
}
        .auto-style7
        {
            height: 21px;
        }
         .auto-style8
        {
            width: 50%;
            text-align: left; 
            font-size:medium;
            border:1px solid black;
        }
        </style>
</head>
<body style="background-color: #ffffff" oncontextmenu="return false;">
    <form id="form2" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <td align="center">
                       
                        <asp:ImageMap ID="Imagelogo" runat="server" ImageUrl="~/images/NIELIT.jpg" Height="95px" Width="254px">
                        </asp:ImageMap>
                       
                    </td>
                </tr>          
                
                <tr>
                    <td align="center" style="border:1px solid black";>
                        <h3>
                            <strong style="text-align: center; font-size:x-large";>                                
                               Acknowledgement Receipt for Online Payment
                            </strong>
                        </h3>
                    </td>
                </tr>
                
                <tr>
                    <td align="center" valign="top" >
                        <table id="tblMain" runat="server" class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
                            cellspacing="1">
                           
                            <tr>
                                <td colspan="2" class="auto-style5" style="color:red;font-weight:800;text-align:center">
                                    *Note: This receipt is valid only upto 3 months from the date of final payment made.</td>
                            </tr>

                            <tr>                                
                                <td class="auto-style5">Candidate Name
                                </td>
                                <td class="auto-style8">
                                <asp:Label ID="lblCandidateName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <tr>                               
                                <td class="auto-style5">Candidate Registration Number
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblRegNumber" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                           
                            <tr>                                
                                <td class="auto-style5">Application Number
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblapplicationnumber" runat="server" Text=""></asp:Label>
                                </td>
                            </tr> 
                                                       
                            <tr>                                
                                <td class="auto-style5">Demand Note Number</td>
                                <td class="auto-style8">
                                    <asp:Label ID="lbldemandnote" runat="server" Text=""></asp:Label></td>
                            </tr> 
                                                       
                            <tr>                                
                                <td class="auto-style5">Transaction Reference Number
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblreferencenumber" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>  

                            <tr>                                
                                <td class="auto-style5">Transaction Number
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lbltransactionnumber" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>  

                            <tr>                                
                                <td class="auto-style5">Amount Paid
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblamount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>  

                            <tr>                                
                                <td class="auto-style5">Amount Paid For Project</td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblprojectname" runat="server" Text=""></asp:Label></td>
                            </tr>  

                            <tr>                                
                                <td class="auto-style5">Payment Mode
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblpaymentmode" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>  

                            <tr>                                
                                <td class="auto-style5">Payment Date and Time
                                </td>
                                <td class="auto-style8">
                                    <asp:Label ID="lblpaymentdate" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>  

                             <tr>                                
                                <td class="auto-style5">Transaction Status
                                </td>
                                <td class="auto-style8" style="color:darkgreen; font-weight:900">
                                    <asp:Label ID="lblstatus" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>  
                           
                            <tr>
                    <td align="center" valign="top" colspan="2" class="auto-style8">
                        <table id="Table1" runat="server" class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
                            cellspacing="1">
                             <tr>                                
                                <td colspan="2" align="center" valign="middle" style="font-weight: 700">***Instruction for candidates***</td>
                            </tr>  
                           
                             <tr>                                
                                <td colspan="2" style="font-weight: 700">1)Kindly attach this receipt along with your project form and send to the NIELIT HQ. If failing so, project will not be consider.</td>
                            </tr>  
                           
                             <tr>                                
                                <td colspan="2" style="font-weight: 700">2)This receipt is valid only upto 3 months from the date of final payment made.</td>
                            </tr>  
                           
                             <tr>                                
                                <td colspan="2" style="font-weight: 700">3)If any kind of discrepency or data tempering found with these receipt, in that case candidate can be debarred.</td>
                            </tr>  
                           
                             <tr>                                
                                <td colspan="2" style="font-weight: 700">
                                    4)For any query regarding the receit/projet fee, Kindly contact to the NIELIT HQ.</td>                                
                            </tr>  
                            </table>
                        </td>
                                </tr>
                           
                                    </table>                                    
                                </td>                               
                            </tr>                            
                                       
                    <tr>
                        <td align="center" class="auto-style6">
                              <asp:Button ID="btn_print" runat="server" Text="Print" 
                              OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;" Height="26px" Width="75px" />
                        </td>
                    </tr>

                <tr>
                        <td align="center">                            
                        </td>
                    </tr>
                
                <tr>
                        <td align="center" class="auto-style7">                            
                            </td>
                    </tr>
                
            </table>
        </div>
    </form>
</body>
</html>

