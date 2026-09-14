<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebHeader.ascx.cs" Inherits="UserControl_WebHeader" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="95" rowspan="2" align="center" valign="middle" class="logo" title="HRMS  Portal Government of Goa">
            <img src="images/logo_02.png" width="63" height="84" />
        </td>
        <td width="332" class="header" title="HRMS  Portal Government of Goa">
            HRMS Portal Government of Goa
        </td>
        <td align="right" valign="bottom" class="right_top">
            <table width="374" border="0" align="right" cellpadding="2" cellspacing="2">
                <tr>
                    <td width="160" align="right" valign="middle">
                        <a id="hlHome" runat="server" href="" target="_top">
                            <div class="home" title="home">
                                
                            </div>
                        </a>
                    </td>
                    <td width="40" align="left" valign="middle">
                        <a href="Home.aspx">
                            <div class="setting" title="settings">
                            </div>
                        </a>
                    </td>
                    <td width="174" align="left" class="login_user">
                        Hi....
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <!--<img src="../App_Themes/Blue/Images/menu_right.jpg" width="8" height="47" align="right" />-->
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left">
                        <div class="menu">
                            <ul>
                                <li class="home"><a href="Home.aspx" >Home</a></li>
                                <li><a href="Modules.aspx" >Know Goa</a></li>
                                <li><a href="Home.aspx">Government</a></li>
                                <li><a href="Home.aspx">Citizen Departments</a></li>
                                <li><a href="Home.aspx">E-services & Forms</a></li>
                                <li><a href="Home.aspx">Notice & Tender</a></li>
                                <li><a href="Home.aspx">Photo Gallery</a></li>
                                <li><a href="Home.aspx">Contact Us</a></li>
                            </ul>
                        </div>
                    </td>
                    <td class="right_block">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
