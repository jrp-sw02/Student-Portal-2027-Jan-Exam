<%@ Page Title="" Language="C#" Debug="false" MasterPageFile="~/MasterPages/FullInfo.master" AutoEventWireup="true"
    CodeFile="AdminChangePasswd.aspx.cs" Inherits="Admin_AdminChangePasswd" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            color: #FF3300;
        }
        .style2
        {
            color: #FF0000;
        }
        .Weak
        {
            color:Blue;
            font-size:10pt;
        }
        .Poor
        {
            color:Red;
            font-size:10pt;
        }
        .Strong
        {
            color:Green;
            font-size:10pt;
        }
        .Good
        {
            color:Gray;
            font-size:10pt;
        }
        .Excellent
        {
            color:Maroon;
            font-size:10pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <%--<uc1:NormalHeader ID="NormalHeader1" runat="server" />--%>
    <asp:Label ID="lblHeading" runat="server" Text="Change Password"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/SHA256.js"></script>
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            
            if (!isBlank("<%=Txt_OldPsswd.ClientID %>", "Current password"))
                return false;
            if (!isBlank("<%=Txt_NewPsswd.ClientID %>", "New password"))
                return false;
            if (!isBlank("<%=Txt_ConfirmPsswd.ClientID %>", "Confirm new password"))
                return false;
            if (!isSelected("<%=ddlSecurityQues.ClientID %>", "Security Questions"))
                return false;
          if (!isBlank("<%=TxtSecurityAns.ClientID %>", "Security answer")) {
              return false;
          }
          
            //Password Policy 
            // at least one upper case letter (A – Z).
            // at least one lower case letter(a-z).
            // At least one digit (0 – 9) .
            // at least one special Characters of !@#$%&*()
            var userName = document.getElementById("<%=TxtUserName.ClientID %>").value;
            var password = document.getElementById("<%=Txt_NewPsswd.ClientID %>").value;
            var regExp = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%&*()]).{8,}/;
            var validPassword = regExp.test(password);
            //var Flag = aContainsB(password, userName);//contains(password, userName);

            //alert(validPassword)
            //alert('Flag' +Flag)
            
            if (validPassword != false) {
                document.getElementById("<%=MD5Curr.ClientID %>").value = document.getElementById("<%=Txt_OldPsswd.ClientID %>").value; 
                var salt = randomString(10, '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ');
                var cpwd = SHA256(document.getElementById("<%=Txt_OldPsswd.ClientID %>").value).toUpperCase();

                document.getElementById("<%=tuntCurr.ClientID %>").value = String(cpwd);
                var cmixpwd = String(salt).concat(String(cpwd));
                document.getElementById("<%=Txt_OldPsswd.ClientID %>").value = SHA256(cmixpwd);

                var newpwd = SHA256(document.getElementById("<%=Txt_NewPsswd.ClientID %>").value).toUpperCase();
                document.getElementById("<%=tuntNew.ClientID %>").value = String(newpwd);
                var newmixpwd = String(salt).concat(String(newpwd));
                document.getElementById("<%=Txt_NewPsswd.ClientID %>").value = SHA256(newmixpwd);

                var conpwd = SHA256(document.getElementById("<%=Txt_ConfirmPsswd.ClientID %>").value).toUpperCase();
                document.getElementById("<%=tuntConfirm.ClientID %>").value = String(conpwd);
                var conmixpwd = String(salt).concat(String(conpwd));
                document.getElementById("<%=Txt_ConfirmPsswd.ClientID %>").value = SHA256(conmixpwd);
            }
            else {
                document.getElementById("<%=Txt_OldPsswd.ClientID %>").value = "";
                document.getElementById("<%=Txt_NewPsswd.ClientID %>").value = "";
                document.getElementById("<%=Txt_ConfirmPsswd.ClientID %>").value = "";
                alert('Password should be at least 8 characters long with lowercase, uppercase, numeric character,and special symbol characters');
                return false;
            }
          return true;
        }

        function aContainsB(a, b) {
            return a.indexOf(b) >= 0;
        }

        function randomString(length, chars) {
            var result = '';
            for (var i = length; i > 0; --i) result += chars[Math.floor(Math.random() * chars.length)];
            return result;
        }
    </script>
    <div id="div2" runat="server" width="100%">
        <table style="width: 550px;">
            <tr>
                <td colspan="3">
                    &nbsp;<asp:Label ID="LblTitle" runat="server" Font-Bold="False" ForeColor="#003399"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="1"
                        cellspacing="0">
                        <tr class="head1">
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td align="left" width="2%">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/change_password.png" />
                                        </td>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="LblHead" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="100%" colspan="2">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td width="30%">
                                User Name<span class="style1">*</span>
                            </td>
                            <td width="70%">
                                <asp:TextBox ID="TxtUserName" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td >
                                Current Password<span class="style2">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_OldPsswd" runat="server" TextMode="Password" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td >
                                New Password<span class="style1">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_NewPsswd" runat="server" TextMode="Password" Width="250px"></asp:TextBox>
                                <asp:PasswordStrength runat="server" ID="PasswordStrength1" TargetControlID="Txt_NewPsswd"
                                    DisplayPosition="BelowRight" MinimumSymbolCharacters="1" MinimumNumericCharacters="1"
                                    MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
                                    RequiresUpperAndLowerCaseCharacters="true" TextStrengthDescriptions="Poor; Weak; Good; Strong; Excellent"
                                    StrengthIndicatorType="Text" HelpHandlePosition="AboveLeft" StrengthStyles="Poor;Weak;Good;Strong;Excellent">
                                </asp:PasswordStrength>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td >
                                Confirm New Password<span class="style1">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_ConfirmPsswd" runat="server" TextMode="Password" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdrow1" id="TrQues" runat="server">
                            <td >
                                Security Questions<span class="style1">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSecurityQues" runat="server" Width="263px">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    <asp:ListItem>What is your name?</asp:ListItem>
                                    <asp:ListItem>Your First School Name</asp:ListItem>
                                    <asp:ListItem>Your Pet Name</asp:ListItem>
                                    <asp:ListItem>Your Mobile Number</asp:ListItem>
                                    <asp:ListItem>Your First College Name</asp:ListItem>
                                    <asp:ListItem>Your Ideal Name</asp:ListItem>
                                    <asp:ListItem>Your Favourite Teacher</asp:ListItem>
                                    <asp:ListItem>Your Favourite Color</asp:ListItem>
                                    <asp:ListItem>Your Favourite Actor</asp:ListItem>
                                    <asp:ListItem>Your Favourite Movie</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="TrAns" runat="server">
                            <td >
                                Security Answer<span class="style1">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtSecurityAns" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <div style="text-align: center; width: 100%; margin-top: 10px;" align="center">
                        <asp:Button ID="btnChangePsswd" runat="server" Text="Change Password" OnClick="btnChangePsswd_Click"
                            OnClientClick="return ValidateLogin();" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                    </div>
                </td>
                <td>
                    <asp:HiddenField ID="HfSrc" runat="server" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div id="div1" runat="server" width="100%" visible="false">
        <table border="0" cellpadding="3" class="sample3" width="100%">
            <tr class="head1">
                <td colspan="2">
                    <asp:Label ID="LblMsgHead" runat="server" Text="Password change successfully completed on first login. "></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr class="gdrow1">
                <td colspan="2">
                    <asp:Label ID="LblWelcome" runat="server"></asp:Label>
                    <br />
                    <br />
                    Your Password has been successfully changed.
                    <br />
                    Please go back to <a href="../Home.aspx">Login page to Login</a> again.
                    <br />
                    Thanks<br />
                    NIELIT Team
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="right" colspan="2">
                    <a href="../Home.aspx">Back to Login Page</a>
                </td>
            </tr>
        </table>
    </div>

    <asp:HiddenField ID="tuntCurr" runat="server" Value="0" />
    <asp:HiddenField ID="tuntNew" runat="server" Value="0" />
    <asp:HiddenField ID="tuntConfirm" runat="server" Value="0" />
    <asp:HiddenField ID="MD5Curr" runat="server" Value="0" />
</asp:Content>
