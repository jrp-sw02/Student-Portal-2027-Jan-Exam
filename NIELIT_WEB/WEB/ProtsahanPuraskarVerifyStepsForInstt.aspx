<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProtsahanPuraskarVerifyStepsForInstt.aspx.cs"
    Inherits="ProtsahanPuraskarVerifyStepsForInstt" Debug="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .sample3
        {
            width: 98%;
        }
        input[disabled="disabled"]
        {
            color: Gray;
        }
        .auto-style1 {
            height: 5px;
            width: 240px;
        }
    </style>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    
</head>
<body style="background-color: White;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%; text-align: left;">
        <table align="center" class="sample3" width="98%" cellpadding="3" cellspacing="0">
           
            <tr class="gdalternate1">
                <td>
                    <strong style="font-size: large;" class="header">Protsahan Puraskar Verification Steps
                       </strong>
                    <%--<strong style="font-size: large;" class="header">Step By Step Instructions For Filling
                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>Form</strong>--%>
                    <%--<td class="header" width="70%">
                    Step By Step Process For Application Form:-
                </td>--%>
                </td>
                <td valign="top" align="right" width="30%">
                    
                </td>
            </tr>
        
            <tr>
                <td colspan="2">
                    <iframe runat="server" id="ifrmAboutUs" width="100%" height="350px" frameborder='0'
                        marginheight='0' marginwidth='0' scrolling="yes"></iframe>
                </td>
            </tr>
            
            
            
                    </table>
               <table>
            
        
                         <tr>
                 <td colspan="2" class="auto-style1">
                </td>
                
            </tr>
                    </table>
    </div>
    </form>
</body>
</html>
