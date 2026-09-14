<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NormalHeader.ascx.cs"
    Inherits="UserControl_NormaltHeader" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td height="68px">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" valign="top" class="logo">
                        <img runat="server" id="imgLogo" src="~/App_Themes/Blue/Images/Logo.jpg"  alt="NIELIT" />
                    </td>
                    <td align="center" class="heading">
                        <div id="tdHeaderBig" runat="server"></div>
                        <div id="tdHeaderSmall" runat="server" class="heading_small"></div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center" height="5px" class="menu">
        </td>
    </tr>
</table>
