<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="FrmCSC.aspx.cs" Inherits="FrmCSC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
 <center>
    <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
    <td class="style1">
    
    </td>
    </tr>
        <tr>
            <td>
               <h4> Your Application Receipt Number Is : 12232334</h4>
            </td>
            <td align="right">
            <a href="Download/PrintReceipt.pdf" target ="_blank">Print Receipt</a>
            </td>
        </tr>
        <tr>
            <td>
              <a href="#"><h2> Please go to nearest CSC Center and make your Payment</h2></a></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
            </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
        </tr>
    </table>
    </center>
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

