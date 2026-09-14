<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="RegisteredUser.aspx.cs" Inherits="RegisteredUser"   Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 40%;
        }
        .style2
        {
            width: 30%;
        }
        .Weak
        {
            color: Blue;
            font-size: 12pt;
            padding-left: 170px;
            vertical-align: bottom;
        }
        .Poor
        {
            color: Red;
            font-size: 12pt;
            padding-left: 170px;
            vertical-align: middle;
        }
        .Strong
        {
            color: Green;
            font-size: 12pt;
            padding-left: 170px;
            vertical-align: middle;
        }
        .Good
        {
            color: Gray;
            font-size: 12pt;
            padding-left: 170px;
            vertical-align: middle;
        }
        .Excellent
        {
            color: Maroon;
            font-size: 12pt;
            padding-left: 170px;
            vertical-align: middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="User Registration:For Existing Registered Candidate"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
        if (window.top.location.href.indexOf('Index.aspx') < 0)
            window.top.location.href = 'Index.aspx';

        function PasswordPolicy() {
            var password = document.getElementById("<%=Txtpassword.ClientID %>").value;
            var regExp = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%&*()]).{8,}/;
            var validPassword = regExp.test(password);

            if (validPassword != false) {
                return true;
            }
            else {
                document.getElementById("<%=Txtpassword.ClientID %>").value = "";
                alert('Password should be at least 8 characters long with lowercase, uppercase, numeric character,and special symbol characters');
                return false;
            }
        }
    </script>
    <table border="0" width="100%">
        <tr>
            <td>
                <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" Width="100%" DisplaySideBar="False"
                    OnNextButtonClick="Wizard1_NextButtonClick" OnFinishButtonClick="Wizard1_FinishButtonClick"
                    OnPreviousButtonClick="Wizard1_PreviousButtonClick">
                    <WizardSteps>
                        <asp:WizardStep ID="first" runat="server" Title="Registered Candidate Verification">
                            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Step 1: Registered Candidate Verification
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <asp:Label ID="lblError" runat="server" Text="Please enter your current course details to start new user registration process"
                                            CssClass="error" EnableTheming="false" Width="100%" Style="padding-left: 1px;
                                            margin-left: -1px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td colspan="2">
                                        <asp:Label ID="lbl1" Width="70%" runat="server" ForeColor="Red"></asp:Label><a style="float: right;"
                                            href="Home.aspx">Back to Home Page</a>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td class="style2">
                                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Current Course &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:DropDownList ID="Ddlcname" runat="server" Height="22px" Width="210px">
                                        </asp:DropDownList>
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Current
                                            Course in which you are Registered.)</span>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td class="style2">
                                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Registration Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="Txtregno" Width="200px" runat="server" MaxLength="10" onkeypress="checkNumber(this,10,0,event);"
                                            onpaste="return false;"></asp:TextBox>
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Registration Number
                                            of above selected course.)</span>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td class="style2">
                                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Name of Candidate &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="Txtname" MaxLength="60" Width="200px" runat="server" onpaste="return false;"></asp:TextBox>
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Name of the candidate
                                            at the time of registration.)</span>
                                    </td>
                                </tr>
                                <tr class="trgdalternate1calendar">
                                    <td class="style2">
                                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Date of Birth (dd-Mon-yyyy) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtDOB" runat="server" Width="200px" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                        <img id="imgDob" src="images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Date of Birth
                                            of the candidate)</span>
                                        <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDOB" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td valign="top" class="style2">
                                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Captcha Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcode" runat="server" Width="200px" MaxLength="6" onkeypress="checkNumber(this,6,0,event)"></asp:TextBox>
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Enter captcha
                                            code shown in image below)</span>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td valign="top" class="style2">
                                    </td>
                                    <td align="left" valign="top" style="padding: 0">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" style="margin: 0">
                                                    <tr>
                                                        <td width="42%" align="left">
                                                            <img id="imgcap" runat="server" alt="Capture Code" width="150" height="40" src="" />
                                                            <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                                                Width="30px" ToolTip="Click to get new captcha code" OnClick="ImgBtnRefresh_Click"
                                                                Style="vertical-align: baseline;" />
                                                        </td>
                                                        <td width="58%">
                                                            <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Click here to
                                                                obtain new captcha code if are not able to see the captcha code in image.)</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="HfCaptcha" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep ID="second" runat="server" Title="Email Verification">
                            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Step 2: Contact Detail Verification For Registered Candidate
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="Lblerror2" runat="server" Text="Please enter/update your contact details to proceed new user registration process.<br>Note: Please enter a valid and active email address and mobile number. "
                                            CssClass="error" EnableTheming="false" Width="100%" Style="padding-left: 1px;
                                            margin-left: -1px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td colspan="2">
                                        <asp:Label ID="Lblcontact" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td>
                                        <asp:Label ID="Lbcandname" runat="server" Text="Candidate Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblname" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Enter Your Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="txtemail" Width="200px" runat="server" MaxLength="100" onpaste="return false;"></asp:TextBox>
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Email for account
                                            activation will be sent to this email address)</span>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" runat="server">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Enter Your Mobile Number&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="txtmobile" Width="200px" runat="server" MaxLength="10" onkeypress="checkNumber(this,10,10,event);"
                                            onpaste="return false;"></asp:TextBox><span style="text-align: left; vertical-align: top;
                                                font-size: 8pt;"> (SMS will be sent to this mobile number.)</span>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep ID="Third" runat="server" Title="Email Verification">
                            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Step 3: Email Verification For Registered Candidate
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblErrorEmail" runat="server" Text=""
                                            CssClass="error" EnableTheming="false" Width="100%" Style="padding-left: 1px;
                                            margin-left: -1px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td colspan="2">
                                        <asp:Label ID="lblcc" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="Candidate Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcname" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text="Email Address &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:Label ID="lblcEmail" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr id="Tr3" class="gdalternate1" runat="server">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Mobile Number&lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:Label ID="lblCMobile" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr id="Tr4" class="gdalternate1" runat="server">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Enter OTP Number&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="txtCOTPNumber" Width="200px" runat="server" MaxLength="10" onkeypress="checkNumber(this,10,10,event);"
                                            onpaste="return false;"></asp:TextBox><span id="lblRefno" runat="server" style="text-align: left; vertical-align: top;
                                                font-size: 8pt;"></span>
                                    </td>
                                    
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep ID="Fourth" runat="server" Title="User Creation For Existing Registered Candidate">
                            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Step 3:New User Creation For Existing Registered Candidate
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="Lberror3" runat="server" Text="Please enter your login details to complete new user registration process"
                                            CssClass="error" EnableTheming="false" Width="100%" Style="padding-left: 1px;
                                            margin-left: -1px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td colspan="2">
                                        <asp:Label ID="Lbuser" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td>
                                        <asp:Label ID="Lbcandidatename" runat="server" Text="Candidate Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Lblcname1" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 25%">
                                        User Name/Id
                                    </td>
                                    <td style="width: 75%">
                                        <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr id="Tr1" class="gdalternate1" runat="server">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Create New Password &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="Txtpassword" Width="200px" runat="server" onblur="PasswordPolicy();" TextMode="Password" MaxLength="50"
                                            EnableViewState="true" onpaste="return false;"></asp:TextBox><span style="text-align: left;
                                                vertical-align: top; font-size: 8pt;">(Please enter your password here.)<br />
                                                <asp:Label ID="Label12" runat="server" Style="padding-left: 215px; font-size: 8pt;"
                                                    Text="(Minimum 6 characters)"></asp:Label></span>&nbsp;
                                        <asp:PasswordStrength runat="server" ID="PasswordStrength1" TargetControlID="Txtpassword"
                                            DisplayPosition="BelowRight" MinimumSymbolCharacters="1" MinimumNumericCharacters="1"
                                            MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
                                            RequiresUpperAndLowerCaseCharacters="true" TextStrengthDescriptions="Poor; Weak; Good; Strong; Excellent"
                                            StrengthIndicatorType="Text" HelpHandlePosition="AboveLeft" StrengthStyles="Poor;Weak;Good;Strong;Excellent">
                                        </asp:PasswordStrength>
                                    </td>
                                </tr>
                                <tr id="Tr2" class="gdrow1" runat="server">
                                    <td style="width: 25%">
                                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Confirm New Password &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="Txtconfirmpassword" Width="200px" runat="server" TextMode="Password"
                                            MaxLength="50" EnableViewState="true" onpaste="return false;"></asp:TextBox><span
                                                style="text-align: left; vertical-align: top; font-size: 8pt;"> (Please re-enter
                                                your password here.)</span>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="TrQues" runat="server">
                                    <td width="200px">
                                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Security Questions &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label><span
                                            class="style1"></span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSecurityQues" runat="server" Width="210px">
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
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Will be used at
                                            the time of recovery of forgotten password.)</span>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="TrAns" runat="server">
                                    <td width="200px">
                                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Security Answer &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label><span
                                            class="style1"></span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtSecurityAns" runat="server" Width="200px" MaxLength="50" onpaste="return false;"></asp:TextBox>
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Will be used at
                                            the time of recovery of forgotten password.)</span>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblResult" runat="server" visible="false" cellpadding="4" border="0" width="100%"
                    class="sample3">
                    <tr class="head1">
                        <td colspan="2">
                            New User Registration Wizard successfully completed.
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td colspan="2">
                            Dear <span id="userid" runat="server"></span>
                            <br />
                            <br />
                            You have successfully completed New User Registration Process.Your User ID and Password
                            have been sent to your email address.<br />
                            <br />
                            Please activate your account by clicking on the link provided in the mail sent to
                            you. Without activation you can not access Online Student Information and Enrollment
                            System of NIELIT.
                            <br />
                            <br />
                            Thanks<br />
                            NIELIT
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td colspan="2" align="right">
                            <a href="Home.aspx">Back to Home Page</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
