<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="DigitalIndiaStudent.aspx.cs" Inherits="DigitalIndiaStudent" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Digital India Student Registration</title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function setDeclarartion() {
            if (document.getElementById("txtAppName")) {
                if (document.getElementById("txtAppName").value != "") {
                    document.getElementById("lblname").innerText = document.getElementById("txtAppName").value
                }
                else {
                    document.getElementById("lblname").innerText = "";
                }
            }
        }

//        function Validate() {
//            if (!isBlank("RegnRollNumber", "Registration/Roll NUmber"))
//                return false;
//            if (!isSpecialCharacter("RegnRollNumber", "Special characters are not allowed"))
//                return false;
//            if (!isBlank("txtAppName", "Applicant Name"))
//                return false;
//            if (!CheckNumberPresent("txtAppName", "Numeric characters are not allowed"))
//                return false;
//            if (!isSpecialCharacter("txtAppName", "Special characters are not allowed"))
//                return false;
//            if (!isBlankDate("txtDob", "Date Of Birth", "dd-MMM-yyyy"))
//                return false;
//            if (!isDate("txtDob", "Invalid date of birth", "dd-MMM-yyyy"))
//                return false;
//            return true;
//        }


        function ValidateForm() {
            if (!isBlank("txtAppName", "Applicant Name"))
                return false;
            if (!CheckNumberPresent("txtAppName", "Numeric characters are not allowed"))
                return false;
            if (!isSpecialCharacter("txtAppName", "Special characters are not allowed"))
                return false;
            if (!isBlankDate("txtDob", "Date Of Birth", "dd-MMM-yyyy"))
                return false;
            if (!isDate("txtDob", "Invalid date of birth", "dd-MMM-yyyy"))
                return false;
            if (!isBlankNumber("txtCorMobileNo", "Mobile Number"))
                return false;
            if (!isNumber("txtCorMobileNo"))
                return false;
            if (!chekMobNo("txtCorMobileNo"))
                return false;
            if (!isBlank("txtEmailId", "Email Id"))
                return false;
            if (!isValidEmail("txtEmailId", "Invalid E-Mail ID"))
                return false;
            if (!isBlank("TxtPerAddressLine1", "Permanent AddressLine 1"))
                return false;
            if (!isBlank("TxtPerAddressLine2", "Permanent AddressLine 2"))
                return false;
            if (!isBlank("TxtPerCity", "Permanent City Name"))
                return false;
            if (!isSelected("ddlPState", "State"))
                return false;
            if (!isSelected("ddlPdistrict", "District"))
                return false;
            if (!isBlankNumber("TxtPpincode", "Pin Code"))
                return false;
            if (!isNumber("TxtPpincode"))
                return false;
            if (!IsValidMinMaxLenght("TxtPpincode", 6, 6, "Invalid Pin Code"))
                return false;
            if (!isBlank("Qualificationtxt", "Highest Education"))
                return false;
            if (!isSelected("ddlCardType", "ID Card Type"))
                return false;
            if (!isBlank("CardNumbertxt", "Id Card Number"))
                return false;
            return true;
        }

    </script>
    <style type="text/css">
        .modalPopup
        {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 165px;
            top: -620px;
            left: 320px;
            position: relative;
        }
        .modalPopup1
        {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 165px;
            top: -10px;
            left: 320px;
            position: relative;
        }
    </style>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <strong style="text-align: center" class="headfont">REGISTRATION APPLICATION FORM FOR
                        DIGITAL INDIA WEEK </strong>
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblmandatory0" runat="server" Text="<font color='red'>*</font> अनिवार्य फिल्डस (Mandatory fields)">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                Width="99%"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table style="width: 100%;" class="sample3" border="0" cellpadding="3" cellspacing="1">
                        <tr class="head1">
                            <td align="left" colspan="3">
                                1. Registration Details / पंजीयन का विवरण
                            </td>
                        </tr>
                    </table>
                    <table id="TblFormDetail" runat="server" class="sample3" style="width: 100%; text-align: left"
                        border="0" cellpadding="3" cellspacing="1">
                        <tr id="TrAccCentre" runat="server" class="gdrow1">
                            <td align="left" style="width: 3%" valign="top">
                                1.1
                            </td>
                            <td align="left">
                                <asp:Label ID="Label12" runat="server" Text="Accredited Institute / मान्यता प्राप्त संस्थान<font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="AccrInstitute" runat="server" MaxLength="100" Width="520px" Enabled="false"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                <asp:Label ID="lblinstcode" runat="server" Text="" Visible="false"> </asp:Label>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="TrApplicantType" runat="server">
                            <td width="3%" valign="top">
                                1.2
                            </td>
                            <td width="40%" valign="top">
                                <asp:Label ID="Label89" runat="server" Text="Applied As / किसके रूप में आवेदन किया<font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td width="57%">
                                <asp:RadioButtonList ID="RdoAppliedAs" runat="server" RepeatDirection="Horizontal"
                                    Width="312px" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="RdoAppliedAs_SelectedIndexChanged">
                                    <asp:ListItem Value="NEW" Selected="True">New Candidate</asp:ListItem>
                                    <asp:ListItem Value="REG">Registered Candidate</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="gdrow1" id="ApplicantTypeTr" runat="server" visible="false">
                            <td valign="top">
                                1.3
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label9" runat="server" Text="Applicant Type<font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:RadioButtonList ID="RdoApplicantType" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="312px" AutoPostBack="True" OnSelectedIndexChanged="RdoApplicantType_SelectedIndexChanged">
                                    <asp:ListItem Value="OABCMAT">O/A/B/C/MATO</asp:ListItem>
                                    <asp:ListItem Value="CCCBCC">CCC/BCC</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="galternate1" id="RegnRollTr" runat="server" visible="false">
                            <td valign="top">
                                1.4
                            </td>
                            <td valign="top">
                                <asp:Label ID="LblRegnRoll" runat="server" Text="Registration Number<font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:TextBox ID="RegnRollNumber" runat="server" TabIndex="3" MaxLength="60" Width="520px" onpaste="return false;"
                                    oncopy="return false;" oncut="return false;" onkeyup="setDeclarartion();" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                2.
                                <asp:Label ID="Label69" runat="server" Text="Applicant's Personal Details /आवेदक का व्यक्तिगत विवरण">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                2.1
                            </td>
                            <td>
                                <asp:Label ID="Label70" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppName" runat="server" MaxLength="100" Width="520px" TabIndex="4" onpaste="return false;"
                                    oncopy="return false;" oncut="return false;" onkeyup="setDeclarartion();" autocomplete="off"></asp:TextBox>
                                <br />
                                ( Full name as per the highest / latest qualification certificate or legal certificate
                                )
                            </td>
                        </tr>
                        <tr class="trgdalternate1calendar" id="trdob" runat="server">
                            <td>
                                2.2
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक <font color='RED'>*</font> (dd-Mon-yyyy)">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox MaxLength="11" ID="txtDob" runat="server" TabIndex="5" SkinID="txtDate" Width="99px"
                                     onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                                    height: 22px; vertical-align: top;" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <%--<asp:Button ID="btnValidate" runat="server" Text="Validate" Visible="false" 
                                    OnClientClick="return Validate();" onclick="btnValidate_Click" />--%>
                                <br />
                                ( As per high school certificate in 'dd-Mon-yyyy' format. i.e. '01-Jan-1990' )
                                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                2.3
                            </td>
                            <td>
                                <asp:Label ID="Label40" runat="server" Text="Mobile Number / मोबाइल नंबर &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorMobileCode" runat="server" Enabled="false" Text="+91" Width="65px"
                                     onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                <asp:TextBox ID="txtCorMobileNo" runat="server" MaxLength="10" TabIndex="6" oncopy="return false"
                                    oncut="return false" onkeypress="checkNumber(this,10,0,event);" onpaste="return false"
                                     Width="280px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td>
                                2.4
                            </td>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Email Address / ईमेल पता&lt;font color='RED'&gt;*&lt;/font&gt; ">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailId" runat="server" MaxLength="150" TabIndex="7"  Width="520px"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox><br />
                                (e.g.abc@yahoo.com)
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                3. Correspondence Address Details / पता विवरण
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                3.1
                            </td>
                            <td>
                                <asp:Label ID="Label83" runat="server" Text="Address Line1/पता पंक्ति 1 <b class='mandatory'>*</b>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPerAddressLine1" runat="server" Width="520px" TabIndex="8"  MaxLength="50"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td>
                                3.2
                            </td>
                            <td>
                                <asp:Label ID="Label84" runat="server" Text="Address Line2/पता पंक्ति 2 <b class='mandatory'>*</b>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPerAddressLine2" runat="server" Width="520px" TabIndex="9"  MaxLength="50"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                3.3
                            </td>
                            <td>
                                <asp:Label ID="Label85" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPerAddressLine3" runat="server" Width="520px" TabIndex="10"  MaxLength="50"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                3.4
                            </td>
                            <td>
                                <asp:Label ID="LblCity" runat="server" Text="City Name / शहर का नाम <b class='mandatory'>*</b>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPerCity" runat="server" Width="520px" MaxLength="50" TabIndex="11" onpaste="return false;"
                                    oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                3.5
                            </td>
                            <td>
                                <asp:Label ID="Label79" runat="server" Text="State / राज्य <font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlPState" runat="server" TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlPState_SelectedIndexChanged"
                                            Width="375px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td>
                                3.6
                            </td>
                            <td>
                                <asp:Label ID="Label78" runat="server" Text="District / जिला  <font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlPdistrict" runat="server" TabIndex="13" Width="375px">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlPState" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                3.7
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Pin Code / पिन  कोड  <font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPpincode" runat="server" TabIndex="14" oncopy="return false" oncut="return false"
                                    onkeypress="checkNumber(this,6,0,event);" onpaste="return false" 
                                    Width="93px" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                4. Educational / Qualification Details / शैक्षिक / योग्यता का विवरण
                            </td>
                        </tr>
                        <tr class="gdrow1" valign="top" id="cls1" runat="server">
                            <td>
                                4.1
                            </td>
                            <td>
                                <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification / उच्चतम शैक्षिक योग्यता &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Qualificationtxt" runat="server" TabIndex="15" Width="520px" MaxLength="100"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td>
                                4.2
                            </td>
                            <td>
                                <asp:Label ID="Label49" runat="server" Text="ID Card Type &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="16" AutoPostBack="true" Width="375px">
                                            <%--OnSelectedIndexChanged="ddlCardType_SelectedIndexChanged">--%>
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            <asp:ListItem Value="Aad">Aadhaar Card</asp:ListItem>
                                            <asp:ListItem Value="Pan">PAN Card</asp:ListItem>
                                            <asp:ListItem Value="Vot">Voter Card</asp:ListItem>
                                            <asp:ListItem Value="Pas">Passport</asp:ListItem>
                                            <asp:ListItem Value="Rat">Ration Card</asp:ListItem>
                                            <asp:ListItem Value="Ins">Institute ID Card</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td>
                                4.3
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Selected ID Card Number / कार्ड संख्या &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td>
                                <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:TextBox ID="CardNumbertxt" runat="server" Width="520px" TabIndex="17" MaxLength="50"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                  <%--  </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <br />
                                <asp:Button ID="btnSave" runat="server" Text="Submit" TabIndex="20" OnClick="btnSave_Click"
                                    OnClientClick="return ValidateForm();" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    </form>
</body>
</html>
