<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="OTPprocess.aspx.cs" Inherits="CAND_OTPprocess" %>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" ></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateForm() {

            if (!isBlankNumber("<%=TxtOTP.ClientID %>", "OTP"))
                return false;
            if (!isNumber("<%=TxtOTP.ClientID %>"))
                return false;

        }


    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
         <div class="sample3">
        <table class="box" cellpadding="3" cellspacing="0" width="100%" style="margin-top: 20px">
            <tr class="gdalternate1">
                <td colspan="3" align="left" style="color: #800000; font-size: 17px; line-height:1.5pc;">
                    OTP (One Time Password) has been sent to your registered
                    <asp:Label ID="LblOTP" runat="server"></asp:Label><br />
                    Please enter OTP in below textbox and click on Validate button to complete the verification
                    process.
                    <br />
                    If you didn't receive OTP please click on Resend button to get OTP again.
                </td>
            </tr>
            <tr class="gdalternate1">
                <td colspan="3" style="height: 25px;">
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" width="14%" style="color: #800000; font-size: 17px;">
                    Enter OTP
                </td>
                <td align="left" width="25%">
                    <asp:TextBox ID="TxtOTP" runat="server" onkeypress="checkNumber(this,6,0,event);"  Width="198px" MaxLength="6"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:Label ID="lblRefNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" style="height: 25px;">
                </td>
                <td align="left" width="25%" style="height: 25px;">
                </td>
                <td align="left" style="height: 25px;">
                </td>
            </tr>
            <tr class="gdalternate1" runat="server" id="trmessage">
                <td align="left" style="height: 25px;" colspan="3">
                    <asp:Label ID="lbmessage" runat="server" Text="" style="color:Red; font-size:12pt; padding:2px 2px 2px 2px;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="BtnValidate" runat="server" OnClick="BtnValidate_Click" Text="Validate"
            OnClientClick="return ValidateForm();" Width="69px" />
        <asp:Button ID="BtnResend" runat="server" Text="Resend" OnClick="BtnResend_Click" />
        <asp:Button ID="BtnValidateCancel" runat="server" Text="Cancel" OnClick="BtnValidateCancel_Click" />
    </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
        <div class="sample3">
        <table class="box" cellpadding="3" cellspacing="0" width="100%" style="margin-top: 20px">
        <tr class="gdalternate1">
        <td colspan="3" align="left" style="color: #800000; font-size: 17px;" id="Tdmsg1" runat ="server">
        You have Successfully verified your <asp:Label ID="LblOTPMsg" runat="server" ></asp:Label> .<br /> 
            Now you will receive all the messages from Nielit here after on this
            <asp:Label ID="LblOTPCaption" runat="server"></asp:Label>
            &nbsp;.<br/> If in future you want to change your contact detail,please
            <a href="myprofile.aspx">go to the Profile page</a>.<br /><br />
        Thank You,<br />
        NIELIT Team

        </td>
        </tr>
        </table>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
