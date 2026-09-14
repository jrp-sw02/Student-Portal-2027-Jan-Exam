<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Forgot Password: Recovery Wizard"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
<uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
        if (window.top.location.href.indexOf('Index.aspx') < 0)
            window.top.location.href = 'Index.aspx';
    </script>
    <h3 id="wizhead" runat="server">
        Please follow the following steps in order to recover forgotten password:-
    </h3>
    <asp:Label ID="lblError" runat="server" Text="Label"></asp:Label>
    <table border="0" width="100%">
        <tr>
            <td>
                <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" Width="100%" DisplaySideBar="False"
                    OnNextButtonClick="Wizard1_NextButtonClick" OnFinishButtonClick="Wizard1_FinishButtonClick"
                    OnPreviousButtonClick="Wizard1_PreviousButtonClick">
                    <WizardSteps>
                        <asp:WizardStep ID="first" runat="server" Title="User Name Verification">
                            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Step 1: User Name Verification
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td colspan="2">
                                      <asp:Label ID="lbl1" Width="70%" runat="server" ForeColor="Red"></asp:Label><a style="float:right;" href="Home.aspx">Back
                                          to Login Page</a>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 30%">
                                        Select your User Type
                                    </td>
                                    <td style="width: 70%">
                                        :
                                        <asp:DropDownList ID="ddlUserType" Width="60%" runat="server" AutoPostBack = "true"
                                            onpaste="return false;" oncopy="return false;" 
                                            OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">
                                         <asp:ListItem Value = '0' Text = 'Select User Type' Selected = "true"></asp:ListItem>
                                         <asp:ListItem Value = '3' Text = 'Candidate'></asp:ListItem>
                                         <asp:ListItem Value = '4' Text = 'Institute'></asp:ListItem>
                                         <asp:ListItem Value = '9' Text = 'Others'></asp:ListItem>
                                       </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 30%">
                                        Enter Your User Name
                                    </td>
                                    <td style="width: 70%">
                                        :
                                        <asp:TextBox ID="Txtname" Width="60%" runat="server" Enabled = "false" onpaste="return false;" oncopy="return false;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep ID="second" runat="server" Title="Email Verification">
                            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Step 2: Email & Mobile Number Verification
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td colspan="2">
                                        <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="User Name"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                        <asp:Label ID="lblname" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 30%">
                                        Enter Your Registered Email Address
                                    </td>
                                    <td style="width: 70%">
                                        :
                                        <asp:TextBox ID="txtemail" Width="60%" runat="server" onpaste="return false;" oncopy="return false;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" runat="server" visible="false">
                                    <td style="width: 30%" align="center" colspan="2">
                                        or
                                    </td>
                                </tr>
                                <tr class="gdrow1" runat="server" visible="false">
                                    <td style="width: 30%">
                                        Enter Your Registered Mobile Number
                                    </td>
                                    <td style="width: 70%">
                                        :
                                        <asp:TextBox ID="txtmobile" Width="60%" runat="server" MaxLength="10" onpaste="return false;"
                                            oncopy="return false;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep ID="Third" runat="server" Title="Final Step: Security Question Verification">
                            <table id="lastframe" runat="server" cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2">
                                        Final Step: Security Question Verification
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td colspan="2">
                                        <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td>
                                        <asp:Label ID="Label6" runat="server"  Text="User Name"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                        <asp:Label ID="lblname1" runat="server" Text="Label"></asp:Label><br />
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td>
                                        <asp:Label ID="Label5" runat="server"  Text="Email-ID"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                        <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="width: 30%">
                                        Your Security Question
                                    </td>
                                    <td style="width: 70%">
                                        :
                                        <asp:TextBox ID="txtques" Enabled="false" Text="" Width="60%" runat="server" onpaste="return false;"
                                            oncopy="return false;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td style="width: 30%">
                                        Enter Your Security Answer
                                    </td>
                                    <td style="width: 70%">
                                        :
                                        <asp:TextBox ID="txtsec" Width="60%" runat="server" onpaste="return false;" oncopy="return false;"></asp:TextBox>
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
                            Forgot password recovery wizard successfully completed.
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td colspan="2">
                            Dear <span id="userid" runat="server"></span><br />
                            <br />
                            A Temporary Password has been sent to your registerd email address.<br />
                            <br />
                            Please login with your UserId and Temporary Password and change your password 
                            after login.
                            <br />
                            <br />
                            Note: If you have not received email, 
                                  Please <asp:LinkButton ID="lnkresend" runat="server" 
                                onclick="lnkresend_Click">click here to resend email.</asp:LinkButton><br />
                                  <br />
                            Thanks<br />
                            NIELIT Team
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td colspan="2" align="right">
                            <a href="Home.aspx">Back to Login Page</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
