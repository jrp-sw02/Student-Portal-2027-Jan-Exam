<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DuplicateRecords.ascx.cs"
    Inherits="UserControl_DuplicateRecords"  Debug="false"%>
<table border="0" cellpadding="3" cellspacing="1" class="sample3" style="width: 100%;
    text-align: left" id="tblShow" runat="server">
    <tr class="head1">
        <td align="left" colspan="3">
            Duplicate Records Details:-
        </td>
    </tr>
    <tr class="gdrow1" id="trrecords" runat="server">
        <td width="100%" valign="top" style="color: Red; font-weight: bold;">
            Duplicate Records Matching This Application :-
        </td>
    </tr>
    <tr class="gdalternate1" id="trrecords1" runat="server">
        <td width="100%" id="tdduplicaterecords" runat="server">
        </td>
    </tr>
    <tr class="gdrow1" id="trmsg" runat="server" visible="false">
        <td colspan="2" style="text-align:left;" width="100%">
         <asp:Label ID="lblmsg" EnableTheming="false" CssClass="error" runat="server" Text="No, duplicate record found matching this application."
             Width="98%" Style="font-weight: bold;"></asp:Label>
        </td>
    </tr>
</table>
