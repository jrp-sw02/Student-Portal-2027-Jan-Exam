<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProfileCandStatus.aspx.cs"
    Inherits="CAND_ProfileCandStatus" %>
<%@ Register src="../UserControl/NormalHeader.ascx" tagname="NormalHeader" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #Table2
        {
            height: 231px;
        }
    </style>
</head>
<body style="background-color: #FFFFFF;">
    <form id="form2" runat="server">
    <div>
      <uc2:NormalHeader ID="NormalHeader1" runat="server" />
        <br />
        <h3 align="center">
            &nbsp APPLICATION STATUS FOR
            <asp:Label ID="Label2" runat="server" Text="" style="text-transform:uppercase;"></asp:Label></h3>
        <table id="Table2" width="95%" align="center" class="sample3" runat="server">
            <tr class="head1">
                <td colspan="2">
                    Application Detail:-
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="30%">
                    Application No and Date
                </td>
                <td width="70%">
                    9856325 Dated 15-Nov-2012
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="30%">
                    Change Type
                </td>
                <td width="70%">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="30%">
                    Status
                </td>
                <td width="70%">
                    Pending
                </td>
            </tr>
            <tr class="head1">
                <td colspan="2">
                    Applicant's Detail:-
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="30%">
                    Name
                </td>
                <td width="70%">
                    Ramesh Jain
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="30%">
                    Address
                </td>
                <td width="70%">
                    Udaipur, Rajasthan
                </td>
            </tr>
        </table>
        <div style="text-align: center; margin-top: 10px">
            <asp:Button ID="Button2" runat="server" Text="Print" OnClientClick=" window.print();" />
            <asp:Button ID="Button3" runat="server" Text="Close" OnClientClick="window.close(); return false" />
        </div>
        <br />
    </div>
    </form>
</body>
</html>
