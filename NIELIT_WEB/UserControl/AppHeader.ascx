<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AppHeader.ascx.cs" Inherits="UserControl_AppHeader" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script language="javascript" type="text/javascript">
    
</script>
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
                        <a id="hlSetting" runat="server" href="" target="_top">
                            <%--<div class="setting" title="settings">
                                
                            </div>--%>
                            <div id="loginContainer">
                          <a href="#" id="loginButton"><span></span><em></em></a>
                          <div style="clear: both">
                          </div>
                          <div id="loginBox" align="left">
                              <div id="loginForm">
                              <fieldset id="body">
                                  <fieldset style="border-bottom: 1px dashed #cccccc;">
                                      <img src="Images/change_theme.png" width="16" height="16" align="right" />
                                      <label for="email" class="hyperlink">
                                          Themes</label>
                                      <img src="Images/themes_button.jpg" width="106" height="28" />
                                  </fieldset>
                              </fieldset>
                              </div>
                          </div>
                      </div>
                        </a>
                    </td>
                    <td width="169">
                        <div id="loginContainer2">
                            <a href="#" id="loginButton2">
                                <label>
                                    Bhuvnesh Jain</label>&nbsp;&nbsp;<span></span></a>
                            <div id="loginBox2">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="loginForm2">
                                    <tr>
                                        <td class="loginuser2">
                                            <ul>
                                                <li>
                                                    <a herf="#"><img src="Images/change_password.png" align="right" width="16" height="16" vspace="4"
                                                        style="float: left;" />Change Password</a></li>
                                                <li>
                                                    <a herf="#"><img src="Images/change_password.png" align="right" width="16" height="16" vspace="4"
                                                        style="float: left;" />Logout</a></li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td width="5">
                        &nbsp;
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
                    <td align="left" width="559px">
                        <div class="menu">
                            <ul>
                                <li class="home"><a target="ifHome" href="Modules.aspx">Operations</a></li>
                                <li><a href="Modules.aspx">Analysis</a></li>
                                <li><a href="Modules.aspx">Settings</a></li>
                            </ul>
                        </div>
                    </td>
                    <td width="331px" align="right" class="menu">
                    
                        <asp:DropDownList ID="ddlModules" Width="331px"  runat="server" 
                            AutoPostBack="True" onselectedindexchanged="ddlModules_SelectedIndexChanged">
                        </asp:DropDownList>
                        <script type="text/javascript">
                            $(document).ready(function () {

                                try {
                                    oHandler = $("#<% =ddlModules.ClientID %>").msDropDown({ mainCSS: 'dd2' }).data("dd");
                                    $("#ver").html($.msDropDown.version);
                                } catch (e) {
                                    alert("Error: " + e.message);
                                }
                            })
                
                        </script>
                    </td>
                    <td class="right_block">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
