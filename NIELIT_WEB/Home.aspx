 
<%@ Page Language="C#" MasterPageFile="MasterPages/main.master" AutoEventWireup="true" ValidateRequest ="true" CodeFile="Home.aspx.cs" Inherits="Home" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Script/Hash.js" type="text/javascript"></script>
    <script src="Script/SHA256.js"></script>

    <script language="javascript" type="text/javascript">    


        // loader code home page by amit
        function showLoader() {
            var loader = document.getElementById("loadingMessage");

            if (loader) {
                loader.style.display = "inline-flex";
            }

            return true;
        }
        // loader code home page by amit



        function ValidateLoginFields() {
            if (document.getElementById("<%=ddlUserType.ClientID %>").value == 0) {
                document.getElementById("<%=ddlUserType.ClientID %>").focus();
                alert("Please Select User Type");
                return false;
            }
            if (document.getElementById("<%=txtUserName.ClientID %>").value.length === 0) {
                document.getElementById("<%=txtUserName.ClientID %>").focus();
                alert("Please Enter User Id");
                return false;
            }
            if (document.getElementById("<%=txtPwd.ClientID %>").value.length === 0) {
                document.getElementById("<%=txtPwd.ClientID %>").focus();
                alert("Please Enter Password");
                return false;
            }
            if (document.getElementById("<%=txtcode.ClientID %>").value.length === 0) {
                document.getElementById("<%=txtcode.ClientID %>").focus();
                alert("Please Enter Captcha code");
                return false;
            }

            //var pwd = HashPwd(document.getElementById("<%=txtPwd.ClientID %>").value).toUpperCase();
            //var salt = randomString(10, '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ');
            //document.getElementById("<%=hfKey.ClientID %>").value = String(salt);
            //var mixpwd = String(salt).concat(String(pwd));           
            //document.getElementById("<%=txtPwd.ClientID %>").value = HashPwd(mixpwd);    
            
            var pwd = SHA256(document.getElementById("<%=txtPwd.ClientID %>").value).toUpperCase();
            var salt = randomString(10, '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ');
            document.getElementById("<%=hfKey.ClientID %>").value = String(salt);
            var mixpwd = String(salt).concat(String(pwd));           
            document.getElementById("<%=txtPwd.ClientID %>").value = SHA256(mixpwd);    

            return true;
        }

        function randomString(length, chars) {
        var result = '';
        for (var i = length; i > 0; --i) result += chars[Math.floor(Math.random() * chars.length)];
        return result;
        }

        function VerifyOtpFields() {

            <%if(txtOtp.ClientID !=null){%>               
              if (document.getElementById("<%=txtOtp.ClientID %>").value.length === 0) {
                document.getElementById("<%=txtOtp.ClientID %>").focus();
                alert("Please Enter OTP code");
                return false;
            }
            <%} %>            
            return true;
        }

        function forgot() {
            window.location.href = "ForgotPassword.aspx?Uname=" + document.getElementById('<%=txtUserName.ClientID %>').value;
        }

    </script>
    
     <%--  // loader code home page by amit--%>
    <style type="text/css">
 .smallSpinner {
    display: inline-block;
    width: 14px;
    height: 14px;
    border: 2px solid #ddd;
    border-top: 2px solid #333;
    border-radius: 50%;
    animation: spin 0.8s linear infinite;
    vertical-align: middle;
    margin-right: 5px;
}

@keyframes spin {
    100% {
        transform: rotate(360deg);
    }
}

</style>
    <%--// loader code home page by amit--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text=" Welcome to Student Information and Enrollment System"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <font color="blue" size="3">&nbsp;NIELIT Help Desk No. 011-44446771 [Mon-Sat From 9:00AM-5:30PM]. Board No. 011-44446777</font>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" class="department_header">
                            <img alt="Arrow" src="App_Themes/Blue/Images/arrow1.png" width="30" height="20" /><font
                                size="3">Exam Notification</font>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt2">
                            <marquee behavior="scroll" direction="down" scrollamount="4"  onmouseover="this.stop();"
                                style="height: 300px;" onmouseout="this.start();">                         
                            <p style="font-weight:bold; text-decoration:underline; padding-left:4px;">Upcoming Exam Notification</p>
                            <ul id="examNotification" runat="server">
                            </ul></marquee>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" class="department_header">
                            <img alt="Arrow" src="App_Themes/Blue/Images/arrow1.png" width="30" height="20" /><font
                                size="3">Admit Card Notification</font>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt2">
                            <marquee behavior="scroll" direction="down" scrollamount="3" onmouseover="this.stop();"
                                style="height: 200px;" onmouseout="this.start();">
                                 <p style="font-weight:bold; text-decoration:underline; padding-left:4px;">Admit Card Notification</p>                            
                            <ul id="admitCardNotification" runat="server">
                            </ul></marquee>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" class="department_header">
                            <img alt="Arrow" src="App_Themes/Blue/Images/arrow1.png" width="30" height="20" /><font
                                size="3">Practical Admit Card Notification</font>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt2">
                            <marquee behavior="scroll" direction="down" scrollamount="3" onmouseover="this.stop();"
                                style="height: 200px;" onmouseout="this.start();">
                                 <p style="font-weight:bold; text-decoration:underline; padding-left:4px;">Practical Examination Admit Card Notification</p>                           
                            <ul id="practicaladmitCardNotification" runat="server">
                            </ul></marquee>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" class="department_header">
                            <img alt="Arrow" src="App_Themes/Blue/Images/arrow1.png" width="30" height="20" /><font
                                size="3">Result Notification</font>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt2">
                            <marquee behavior="scroll" direction="down" scrollamount="3" onmouseover="this.stop();"
                                style="height: 200px;" onmouseout="this.start();">
                                 <p style="font-weight:bold; text-decoration:underline; padding-left:4px;">Result Notification</p>                          
                            <ul id="resultNotification" runat="server">
                            </ul></marquee>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" class="department_header">
                            <img alt="Arrow" src="App_Themes/Blue/Images/arrow1.png" width="30" height="20" /><font
                                size="3">Other Notification</font>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt2">
                            <marquee behavior="scroll" direction="down" scrollamount="3" onmouseover="this.stop();"
                                style="height: 200px;" onmouseout="this.start();">
                    <ul id="otherNotification" runat="server">
                    </ul></marquee>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="bottom" height="2px">
            </td>
        </tr>
        <tr>
            <td valign="bottom" class="txt2" style="padding-left: 40px; padding-bottom: 2px;
                padding-top: 2px;">
                <iframe name="ifHome" id="ifHome" src="WebImages/steps.jpg" width="100%" height="500px"
                    frameborder='0' marginheight='0' marginwidth='0' scrolling="no"></iframe>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <a id="TOP"></a>
    <table width="95%" style="margin-top: 5px; height: 100%;" border="0" align="center"
        cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" valign="middle" class="department_header">
                Login
            </td>
        </tr>
        <tr>
            <td class="txt2">
                <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                    <tr>
                        <td align="left" height="5px">
                            Currently LoggedIn Users:
                            <asp:Label ID="LblActiveLoggedIn" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblError" runat="server" ForeColor="#CC0000"></asp:Label>
                            <asp:LinkButton ID="Lnkemail" runat="server" Visible="false" OnClick="Lnkemail_Click">Resend Email</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="LoginPnl" runat="server" DefaultButton="btnLogin">
                                 
                    <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                        <tr>
                            <td class="login_caption">
                                User Type
                            </td>
                        </tr>
                        <tr>
                            <td class="login_txt">
                                <asp:DropDownList ID="ddlUserType" Width="187px" autocomplete="off" runat="server"
                                    ToolTip="User Type" onpaste="return false;">
                                    <asp:ListItem Value='0' Text='Select User Type'></asp:ListItem>
                            	<asp:ListItem Value='3' Text='Candidate'></asp:ListItem>
                                    <asp:ListItem Value='4' Text='Institute'></asp:ListItem>
                                    <asp:ListItem Value='9' Text='Others'></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="login_caption">
                                User ID
                            </td>
                        </tr>
                        <tr>
                            <td class="login_txt">
                                <asp:TextBox ID="txtUserName" Width="175px" autocomplete="off" runat="server" Enabled="true"
                                    ToolTip="User Name"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="login_caption">
                                Password
                            </td>
                        </tr>
                        <tr>
                            <td class="login_txt">
                                <asp:TextBox ID="txtPwd" Width="175px" autocomplete="off" TextMode="Password" runat="server"
                                    Enabled="true" ToolTip="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="login_caption" style="width: 90%" valign="middle">
                                Captcha
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="login_txt">
                                <asp:TextBox ID="txtcode" runat="server" Width="175px" MaxLength="6" autocomplete="off"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="login_caption">
                                Captcha Code
                            </td>
                        </tr>
                        <tr>
                            <td class="login_txt">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <img src="" id="imgcap" runat="server" alt="Capture Code" width="155" height="35" />
                                        <asp:ImageButton ID="ImgBtnRefresh" ImageUrl="~/images/refresh.jpg" runat="server"
                                            CausesValidation="false" Width="25px" Height="35" Style="vertical-align: top;
                                            padding-top: 0px; border: none;" OnClick="ImgBtnRefresh_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--if (!ValidateLoginFields()) return false;--%>

                            <td align="left" style="padding:20px 10px;">
                                <asp:Button ToolTip="Login" ID="btnLogin" runat="server" Text="Login" Width="60px"
                                    Enabled="true" OnClick="btnLogin_Click"  OnClientClick="if (!ValidateLoginFields()) return false; return showLoader();" />

                                <span id="loadingMessage" style="display: none; margin-left: 8px; vertical-align: middle; white-space: nowrap;"">
                                    <span class="smallSpinner"></span>
                                    <strong>Please Wait...</strong>
                                </span>
                            </td>
                        </tr>
                      
                    </table>
       
 
                </asp:Panel>
                <asp:Panel ID="VeriifyPnl" runat="server" DefaultButton="btnverify" Visible="false">
                    <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                        <tr id="trOtp" runat="server" visible="false">
                            <td class="login_caption">
                                <b>OTP Code</b>
                            </td>
                        </tr>
                        <tr id="trOtpCode" runat="server" visible="false">
                            <td class="login_txt">
                                <asp:TextBox ID="txtOtp" Width="175px" MaxLength="6" autocomplete="off" runat="server"
                                    Enabled="true" ToolTip="OTP"></asp:TextBox>
                                <br />
                                Your otp-code has been sent to your registered mobile & email-Id.<br />
                                <asp:Label ID="lblOtpRefNo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 13px; text-align: right;">
                                <asp:Button ToolTip="Verify" ID="btnVerify" runat="server" Text="Verify" Width="60px"
                                    Enabled="true" OnClick="btnLVerify_Click" OnClientClick="return VerifyOtpFields();" />&nbsp;&nbsp;
                                <asp:Button ToolTip="Cancel" ID="btnCancel" runat="server" Text="Cancel" Width="60px"
                                    Enabled="true" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 10%">
                <asp:HiddenField ID="HfCaptcha" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="forgot">
                <a onclick="forgot();" href="#" title="forgot Password ?">Forgot Password ?</a>
            </td>
        </tr>
        <tr>
            <td class="forgot" width="100%">
                <a href="RegisteredUser.aspx" title="New user registration for existing registered candidate with NIELIT">
                    New User ?(Registered Candidate)</a>
            </td>
        </tr>
        <tr>
            <td style="height: 3px;">
                <asp:HiddenField ID="hfVal" runat="server" Value="0" />
                <asp:HiddenField ID="hfKey" runat="server" Value="0" />
                <asp:HiddenField ID="tuntP" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td>
                <uc2:SideLink ID="Sidelink" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <input onclick="window.open('https://certificate.nielit.gov.in/')"
                    class='btnNormal' type='button' style='border-style: None; height: 54px; width: 212px;
                    background-image: url(images/Get_Certificate.jpg); padding: 0 0 13px 50px; text-align: left;'
                    value='Download Certificate' />
            </td>
        </tr>
        <tr>
            <td align="center">
                <input onclick="window.open('https://www.nielit.gov.in/content/frequently-asked-questions-faqs-0')"
                    class='btnNormal' type='button' style='border-style: None; height: 54px; width: 212px;
                    background-image: url(images/faq_btn1.jpg); padding: 0 0 13px 50px; text-align: left;'
                    value='FAQ' />
            </td>
        </tr>
        <tr>
            <td style="height: 3px">
            </td>
        </tr>
        <tr align="center">
            <td align="center" valign="middle">
                <div style="padding-top: 6px; margin-left: 0px; margin-right: 0px;
                    border: 3px groove #9999FF; font-family: Arial, Helvetica, sans-serif; font-size: 11px;
                    font-weight: bold; color: #000066; line-height: 1.1pc; padding-left: 1px; text-align: left;">
                    For any query/support please write us on for :-                                 
                    <ul><li>O/A/B/C Registration: <b style="color:Maroon";>regn[at]nielit[dot]gov[dot]in/<br/> 011-25308321</b></li>
                    <li>O/A/B/C Theory Exams: <b style="color:Maroon";>exam[at]nielit[dot]gov[dot]in/<br/> 011-25308394</b></li>
                    <li>O/A/B/C Practical Exams: <b style="color:Maroon";>prexam[at]nielit[dot]gov[dot]in/<br/> 011-25308393​</b></li>
                    <li>O/A/B/C Projects: <b style="color:Maroon";>projects[at]nielit[dot]gov[dot]in/<br/> 011-25308393</b></li>
                    <li>Certificates (including provisional, verification, transcripts etc): <b style="color:Maroon";>certificate[at]nielit[dot]gov[dot]in/<br/> 011-25308392​</b></li>
                    <li>Digital Literacy Exams: (ACC/BCC/CCC/CCCP/ECC): <b style="color:Maroon";>ccc[at]nielit[dot]gov[dot]in</b></li>
                    <li>O/A/B/C Accreditation: <b style="color:Maroon";>accr[at]nielit[dot]gov[dot]in/<br/> 011-25308351</b></li>
                    <li>Digital Literacy Facillitation: <b style="color:Maroon";>ccc.accr[at]nielit[dot]gov[dot]in/<br/> 011-25308350</b></li>  
                    </ul>            
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 8px">
            </td>
        </tr>
        <tr align="center">
            <td align="center" valign="middle">
                <div style="padding: 3px; margin-left: 4px; margin-right: 4px; border: 3px groove #9999FF;
                    font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold;
                    color: #000066; line-height: 1.6pc;">
                    You are Visitor Number<br />
                    <span id="lblCounter" runat="server">1000</span>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" style="position: absolute; bottom: 5px;">
                <a href="#Top">GO TO TOP</a>
            </td>
        </tr>
    </table>
</asp:Content>
