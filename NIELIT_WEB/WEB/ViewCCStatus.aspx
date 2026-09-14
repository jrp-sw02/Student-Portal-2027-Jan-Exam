<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCCStatus.aspx.cs" Inherits="ViewCCStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <style type="text/css">
        .style1
        {
            width: 32%;
        }
    </style>
</head>
<body style="background-color:White;">
    <form id="form1" runat="server">
    <div>
    <br />
    <h3 align="center">
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label> Status</h3>
     <table id="Table2" width="95%" align="center" class="sample3" runat="server" style="background-color:#ffffff;">
                                    <tr class="head1">
                                        <td colspan="2">
                                            Applicant's Detail:-
                                        </td>
                                    </tr>
                                    <tr class="gdalternate1">
                                        <td >
                                            Name 
                                        </td>
                                        <td  width="70%" >
                                            Ramesh Jain
                                        </td>
                                    </tr>
                                    <tr class="gdrow1">
                                        <td>
                                            Address 
                                        </td>
                                        <td width="70%" >
                                            Udaipur, Rajasthan
                                        </td>
                                    </tr>
                                    <tr class="head1">
                                        <td colspan="2">
                                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label> Detail:-
                                        </td>
                                    </tr>
                                    <tr class="gdalternate1">
                                        <td >
                                            Registration No and Date 
                                        </td>
                                        <td  width="70%" >
                                            12345556 Dated 15-Nov-2012
                                        </td>
                                    </tr>
                                    <tr class="gdrow1">
                                        <td  >
                                            Registration For <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td  width="70%" >
                                            <asp:Label ID="Label4" runat="server" Text="" style="font-size:small; font-weight:normal;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="gdalternate1">
                                        <td >
                                            Accredited Center
                                        </td>
                                        <td  width="70%" >
                                            Udaipur
                                        </td>
                                    </tr>
                                  
                                     
                                    <tr class="gdrow1">
                                        <td>
                                            Status
                                        </td>
                                        <td  width="70%" >
                                            Completed/Pursuing
                                        </td>
                                    </tr>
                                    </table>
                                    <div style="text-align: center; margin-top: 10px">
                                   <asp:Button ID="Button2" runat="server" Text="Print"  onClientClick=" window.print();"/>
                                   <asp:Button ID="Button3" runat="server" Text="Close" onclientclick="window.close(); return false" />
                                   </div>
                                   <br />
                               
    </div>
    </form>
</body>
</html>
