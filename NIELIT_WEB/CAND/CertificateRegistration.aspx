<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="CertificateRegistration.aspx.cs" Inherits="Certificate" Debug="True" %>

<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Examination Form</title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/jquery-3.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
    <script type="text/javascript">
        document.onkeydown = function (e) {
            if (event.keyCode == 123) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
                return false;
            }
        }


        // it removes consecutive spaces from input textboxes
        function sanitizeSpace() {
            const inputField = document.querySelector('#txtConsentPlace');
            inputField.addEventListener('input', (e) => {
                e.target.value = e.target.value.replace(/ +/g, ' ');
            });
        }

        //-------------- added on Sept-2021 by vishal----------------------/
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%#UploadPhoto.ClientID %>").click();
            }
        }
        function imageSignpreview(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%#UploadSignature.ClientID %>").click();
            }
        }
        function imageThumbpreview(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%#UploadThumb.ClientID %>").click();
            }
        }
        //-------------- added on Sept-2021 by vishal----------------------/

    </script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function setDeclarartion() {

            if (document.getElementById("ddlSalutaionName").value != "0") {
                if ((document.getElementById("txtFatherName")) && (document.getElementById("txtMotherName"))) {
                    if ((document.getElementById("txtFatherName").value != "") && (document.getElementById("txtMotherName").value != "")) {
                        if (document.getElementById("ddlSalutaionName").value == "Mr.") {
                            document.getElementById("LblName").innerText = document.getElementById("txtAppName").value;
                            document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                            document.getElementById("Lblsalutation").innerText = " son of ";
                            document.getElementById("LblDecMName").innerText = " Smt " + document.getElementById("txtMotherName").value;
                            document.getElementById("LblDechmName").innerText = "श्रीमती " + document.getElementById("txtMotherName").value;
                            document.getElementById("LblDechfName").innerText = " और श्री " + document.getElementById("txtFatherName").value;
                            document.getElementById("LblDecFname").innerText = " and Shri " + document.getElementById("txtFatherName").value;
                            document.getElementById("Lblhsalutation").innerText = " का पुत्र ";
                            document.getElementById("Lblhdectype").innerText = " करता ";
                            document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                        }
                        else {
                            if (document.getElementById("ddlSalutaionName").value == "Ms.") {
                                document.getElementById("LblName").innerText = document.getElementById("txtAppName").value;
                                document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                                document.getElementById("Lblsalutation").innerText = " daughter of ";
                                document.getElementById("LblDecMName").innerText = " Smt " + document.getElementById("txtMotherName").value;
                                document.getElementById("LblDechmName").innerText = "श्रीमती " + document.getElementById("txtMotherName").value;
                                document.getElementById("LblDechfName").innerText = " और श्री " + document.getElementById("txtFatherName").value;
                                document.getElementById("LblDecFname").innerText = " and Shri " + document.getElementById("txtFatherName").value;
                                document.getElementById("Lblhsalutation").innerText = " की पुत्री ";
                                document.getElementById("Lblhdectype").innerText = " करती ";
                                document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                            }
                            else {
                                document.getElementById("LblName").innerText = document.getElementById("txtAppName").value;
                                document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                                document.getElementById("Lblsalutation").innerText = " child of ";
                                document.getElementById("LblDecMName").innerText = " Smt " + document.getElementById("txtMotherName").value;
                                document.getElementById("LblDechmName").innerText = "श्रीमती " + document.getElementById("txtMotherName").value;
                                document.getElementById("LblDechfName").innerText = " और श्री " + document.getElementById("txtFatherName").value;
                                document.getElementById("LblDecFname").innerText = " and Shri " + document.getElementById("txtFatherName").value;
                                document.getElementById("Lblhsalutation").innerText = " की संतान ";
                                document.getElementById("Lblhdectype").innerText = " करता ";
                                ocument.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                            }
                        }
                    }
                }

                else {
                    if (document.getElementById("ddlSalutaionName").value == "Mr.") {
                        document.getElementById("Lblsalutation").innerText = "in care of";
                        document.getElementById("LblName").innerText = document.getElementById("txtAppName").value;
                        document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                        document.getElementById("LblDecMName").innerText = document.getElementById("TxtGuardianName").value;
                        document.getElementById("LblDechmName").innerText = "अभिभावक " + document.getElementById("TxtGuardianName").value;;
                        document.getElementById("LblDecFname").innerText = "";
                        document.getElementById("Lblhsalutation").innerText = "की देखभाल में";
                        document.getElementById("Lblhdectype").innerText = " करता ";

                        document.getElementById("LblhDecmname").innerText = "";
                        document.getElementById("Lblhdecfathername").innerText = "";
                    }
                    else {
                        if (document.getElementById("ddlSalutaionName").value == "Ms.") {
                            document.getElementById("LblName").innerText = document.getElementById("txtAppName").value;
                            document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                            document.getElementById("Lblsalutation").innerText = " daughter of ";
                            document.getElementById("LblDecMName").innerText = " Smt " + document.getElementById("txtMotherName").value;
                            document.getElementById("LblDechmName").innerText = "श्रीमती " + document.getElementById("txtMotherName").value;
                            document.getElementById("LblDechfName").innerText = " और श्री " + document.getElementById("txtFatherName").value;
                            document.getElementById("LblDecFname").innerText = " and Shri " + document.getElementById("txtFatherName").value;
                            document.getElementById("Lblhsalutation").innerText = " की पुत्री ";
                            document.getElementById("Lblhdectype").innerText = " करती ";
                            document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                        }
                        else {
                            document.getElementById("Lblsalutation").innerText = "in care of";
                            document.getElementById("LblName").innerText = document.getElementById("txtAppName").value;
                            document.getElementById("LblhName").innerText = document.getElementById("txtAppName").value;
                            document.getElementById("LblDecMName").innerText = document.getElementById("TxtGuardianName").value;;
                            document.getElementById("LblDechmName").innerText = "अभिभावक " + document.getElementById("TxtGuardianName").value;;
                            document.getElementById("LblDecFname").innerText = "";
                            document.getElementById("Lblhsalutation").innerText = "की देखभाल में";
                            document.getElementById("Lblhdectype").innerText = " करती ";
                        }
                    }
                }
            }
            else {
                document.getElementById("Lblsalutation").innerText = "";
                document.getElementById("LblDecMName").innerText = "";
                document.getElementById("LblDecFname").innerText = "";
                document.getElementById("Lblhdectype").innerText = " करता/करती ";
                document.getElementById("Lblhsalutation").innerText = "";
            }
        }
        function isValidPassingYear(PassingYearctrlId, DobctrlId) {
            if (document.getElementById(PassingYearctrlId)) {
                var PassingYear = document.getElementById(PassingYearctrlId).value;
                var Dobdate = document.getElementById(DobctrlId).value;
                var dobYear = Dobdate.split("-");
                var msg = "Not Valid Passing Year";
                var d = new Date();
                if (d.getFullYear() < PassingYear || (PassingYear) <= (Number(dobYear[2]) + 4)) {
                    CallDiv(PassingYearctrlId, msg);
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        function ValidateForm() {

            if (!isSelected("DdlLastExamMonthYear", "Last Exam Month Year"))
                return false;
            if (!isBlank("TxtRno", "Roll Number"))
                return false;
            if (!isSelected("ddlSalutaionName", "Salutation"))
                return false;
            if (!isBlank("txtAppName", "Applicant Name"))
                return false;
            if (!CheckNumberPresent("txtAppName", "Numeric characters are not allowed"))
                return false;
            // if (!isSpecialCharacterName("txtAppName", "Special characters are not allowed"))
            //   return false;
            if (!isvalidateRadioButtonList("Rdoownertype", "Care Of"))
                return false;

            var list = document.getElementById("Rdoownertype");
            var listItemArray = list.getElementsByTagName("input");
            var Itemvalue = "";
            for (var i = 0; i < listItemArray.length; i++) {
                var listItem = listItemArray[i];
                if (listItem.checked) {
                    Itemvalue = listItem.value;
                }
            }

            if (Itemvalue == "G") {
                if (!isBlank("TxtGuardianName", "Guardian Name"))
                    return false;
                if (!CheckNumberPresent("TxtGuardianName", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("TxtGuardianName", "Special characters are not allowed"))
                    return false;
            }

            else if (Itemvalue == "P") {
                if (!isBlank("txtFatherName", "Father Name"))
                    return false;
                if (!CheckNumberPresent("txtFatherName", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("txtFatherName", "Special characters are not allowed"))
                    return false;
                if (!isBlank("txtMotherName", "Mother Name"))
                    return false;
                if (!CheckNumberPresent("txtMotherName", "Numeric characters are not allowed"))
                    return false;
                if (!isSpecialCharacter("txtMotherName", "Special characters are not allowed"))
                    return false;
            }
            if (!isSelected("ddl_gender", "Gender"))
                return false;

            //if (!isvalidateRadioButtonList("RdoGender", "Gender"))
            //    return false;
            if (!isBlankDate("txtDob", "Date Of Birth", "dd-MMM-yyyy"))
                return false;
            if (!isDate("txtDob", "Invalid date of birth", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("ddlCategory", "Cast Category"))
                return false;
            if (!isSelected("ddlOccupation", "Occupation"))
                return false;
            if (document.getElementById("ddlOccupation").value == "5") //gujgovt
            {
                if (!isBlank("Txtdept", "Department Name"))
                    return false;
                if (!CheckNumberPresent("Txtdept", "Numeric characters are not allowed"))
                    return false;
                if (!isBlank("Txtempcode", "Employee code"))
                    return false;
                if (!isBlank("txtdesg", "Designation"))
                    return false;
                if (!CheckNumberPresent("txtdesg", "Numeric characters are not allowed"))
                    return false;
                if (!isBlank("txtpostcity", "Place of Posting"))
                    return false;
                if (!CheckNumberPresent("txtpostcity", "Numeric characters are not allowed"))
                    return false;
                if (!isBlankDate("txtDojoin", "Date Of Joining", "dd-MMM-yyyy"))
                    return false;
                if (!isDate("txtDojoin", "Invalid date of Joining", "dd-MMM-yyyy"))
                    return false;
                if (!isBlankDate("txtDoretment", "Date Of Retirement", "dd-MMM-yyyy"))
                    return false;
                if (!isDate("txtDoretment", "Invalid date of Retirement", "dd-MMM-yyyy"))
                    return false;

            }

            //            if (document.getElementById("RdDisability").value == "Y")          
            //            {
            if (!isSelected("DdlDisabilityType", "Disability Type"))
                return false;
            if (!isBlank("TxtDisabilityPercent", "Disability Percent"))
                return false;
            if (!isNumber("TxtDisabilityPercent"))
                return false;
            //            }

            if (document.getElementById("TxtSTDcode").value != "" && document.getElementById("txtCorPhoneNo").value != "") {
                if (!isNumber("TxtSTDcode"))
                    return false;
                if (!isNumber("txtCorPhoneNo"))
                    return false;
                if (!isValidTeliphone("TxtSTDcode", "txtCorPhoneNo"))
                    return false;
            }
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
            if (!isBlank("TxtAddressLine1", "Address Line1"))
                return false;
            if (!isBlank("TxtAddressLine2", "Address Line2"))
                return false;
            if (!isBlank("TxtCity", "City"))
                return false;
            if (!isSelected("ddlCorState", "State"))
                return false;
            if (!isSelected("Ddldistrict", "District"))
                return false;
            if (!isBlankNumber("txtPinCode", "Pin Code"))
                return false;
            if (!isNumber("txtPinCode"))
                return false;
            if (!IsValidMinMaxLenght("txtPinCode", 6, 6, "Invalid Pin Code"))
                return false;
            if (!isSelected("DDLeducode", "Highest Education"))
                return false;
            if (!isBlank("TxtYearOfPassing", "Year of Passing"))
                return false;
            if (!isValidPassingYear("TxtYearOfPassing", "txtDob"))
                return false;
            if (!isBlank("TxtEduIfOther", "If Other"))
                return false;
            if (!isSelected("DdlAccState", "Accredited State"))
                return false;
            if (!isSelected("DdlAccDistrict", "Accredited District"))
                return false;
            if (!isSelected("DdlAccCentre", "Accredited Centre"))
                return false;
            if (!isSelected("DdlExamCycle", "Exam Cycle"))
                return false;
            if (!isSelected("DdlApplied4Exam", "Applied for Exam"))
                return false;
            if (!isSelected("DdlExamCentre1", "Exam Centre 1"))
                return false;
            if (!isSelected("DdlExamCentre2", "Exam Centre 2"))
                return false;



             // amit_apaar_api_changes_may_2026_start

            if (!isBlank("txtapaar", "Apaar ID"))
                return false;

            if (!isNumber("txtapaar", "Apaar ID"))
                return false;

            if (!IsValidMinLength("txtapaar", "Apaar ID", 12))
                return false;

            if (!isSelected("ddlConsentRelation", "Consent Relation"))
                return false;

            if (!isBlank("txtproviderName", "Captcha Code"))
                return false;

            if (!isSelected("ddlAuthMode", "Authentication Mode"))
                return false;

            if (!isBlank("txtAuthenticationIdNo", "Authentication ID No"))
                return false;

            if (!IsValidMinLength("txtAuthenticationIdNo", "Authentication ID No", 6))
                return false;

            function IsValidMinLength(ctrl, msg, minLen) {

                var element = document.getElementById(ctrl);

                if (!element)
                    return true;

                var value = trim(element.value, "");
                var upperValue = value.toUpperCase();

                var invalidValues = ["NONE", "N/A", "EMPTY"];

                if (value.length < minLen) {
                    CallDiv(ctrl, msg + " must contain at least " + minLen + " characters");
                    element.focus();
                    return false;
                }

                if (invalidValues.indexOf(upperValue) !== -1) {
                    CallDiv(ctrl, msg + " contains an invalid value.");
                    element.value = "";
                    element.focus();
                    return false;
                }

                return true;
            }


            if (!isBlank("txtConsentDate", "Consent Date"))
                return false;

            if (!isBlank("txtConsentTime", "Consent Time"))
                return false;

            if (!isBlank("txtConsentPlace", "Consent Place"))
                return false;

            if (!ischecked("chkdisclamier", "Declaration"))
                return false;
                 

            if (!ischecked("chkApaarDeclaration", "Apaar Declaratoin"))
                return false;


            // amit_apaar_api_changes_may_2026_end



            if (document.getElementById("ChkMarksheetCopy")) {
                if (!ischecked("ChkMarksheetCopy", "Copy of Marksheet"))
                    return false;
            }

            if (!isBlank("txtcode", "Captcha Code"))
                return false;

            return true;
        }

        function blankFnameMname() {
            document.getElementById("txtFatherName").value = "";
            document.getElementById("txtMotherName").value = "";
        }
        function blankGuardianName() {
            document.getElementById("TxtGuardianName").value = "";
        }

    </script>
    <style type="text/css">
        .modalPopup {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 170px;
            top: 325px;
            left: 320px;
            position: relative;
        }


        .modalPopup5 {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 2px;
            padding-right: 2px;
            padding-left: 2px;
            padding-bottom: 2px;
            width: 443px;
            height: auto;
            top: 257px;
            left: 440px;
            position: relative;
        }

        .modalPopup6 {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 2px;
            padding-right: 2px;
            padding-left: 2px;
            padding-bottom: 2px;
            width: 443px;
            height: auto;
            top: 457px;
            left: 340px;
            position: relative;
        }

        .modalPopup1 {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 170px;
            top: 80px;
            left: 320px;
            position: relative;
        }

        .PromptCSS {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: Courier New;
            border: solid 1px Pink;
            height: 20px;
        }

        disabled {
            position: relative;
            color: grey;
        }

        .disabled:after {
            position: absolute;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            content: ' ';
        }
    </style>
</head>
<body style="background-color: #ffffff; margin-bottom: 10;" oncontextmenu="return false;">
    <form id="form2" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
                <tr>
                    <td>
                        <uc1:NormalHeader ID="NormalHeader1" Visible="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <h3>
                            <strong style="text-align: center" class="headfont">Examination Application Form:
                            <asp:Label ID="Lblhead" runat="server" Text=""></asp:Label>
                            </strong>
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblmandatory0" runat="server" Text="<font color='red'>*</font> अनिवार्य फिल्डस (Mandatory fields)"></asp:Label>
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
                    <td align="center" valign="top">
                        <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
                            cellspacing="1">
                            <tr class="head1">
                                <td colspan="3">1. Registration Details / पंजीकरण के विवरण
                                </td>
                            </tr>
                            <%-- Added for OnlineRef ID --%>
                            <tr class="gdalternate1" id="TrPreMIS" runat="server">
                                <td width="3%" valign="top">1.0
                                </td>
                                <td width="40%" valign="top">Have you got online reference number for enrollment
                                <asp:Label ID="Label24" runat="server" Text=""></asp:Label>
                                    with NIELIT/Accreditation/Extension Center&nbsp; &nbsp; <span id="Span1" runat="server"></span><font color='RED'>*</font>
                                </td>
                                <td width="60%" align="left">
                                    <asp:RadioButtonList ID="RadioButtonListMIS" runat="server" RepeatDirection="Horizontal"
                                        Width="182px" OnSelectedIndexChanged="RadioButtonListMIS_SelectedIndexChanged"
                                        AutoPostBack="True" TabIndex="1">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं</asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हां</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TRMIS" visible="false" runat="server">
                                <td width="3%" valign="middle">1.0.1
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label25" runat="server" Text="If Yes, Please enter Online Reference No. / यदि हाँ,कृपया ऑन्-लाइन् संदर्भ नंबर दर्ज करें<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td width="60%" align="left">
                                    <table cellpadding="0" cellspacing="0" style="width: 64%">
                                        <tr>
                                            <td style="padding-left: 0" width="59%">
                                                <asp:TextBox ID="txtMIS" runat="server" Width="302px" MaxLength="100" TabIndex="2"
                                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImageButtonMIS" runat="server" ImageUrl="~/images/search_btn.jpg"
                                                    ToolTip="Serch" Height="22px" OnClick="ImageButtonMIS_Click" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/reset_btn.jpg"
                                                    OnClick="ImgBtnReset_Click" Visible="False" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                            <%-- End OnlineRefID --%>
                            <tr class="gdalternate1">
                                <td width="3%" valign="top">1.1
                                </td>
                                <td width="40%" valign="top">Whether Applied Previously in
                                <asp:Label ID="LblCourseinEnglish" runat="server" Text=""></asp:Label>
                                    Examination&nbsp; /&nbsp; क्या आपने पहले <span id="spnCourseinHindi" runat="server"></span>परीक्षा के लिए आवेदन किया है" <font color='RED'>*</font>
                                </td>
                                <td width="60%" align="left">
                                    <asp:RadioButtonList ID="RdoAlreadyAppeared4Exam" runat="server" RepeatDirection="Horizontal"
                                        Width="182px" OnSelectedIndexChanged="RdoAlreadyAppeared4Exam_SelectedIndexChanged"
                                        AutoPostBack="True" TabIndex="1">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं</asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हां</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrPreExamRno" visible="false" runat="server">
                                <td width="3%" valign="middle">1.1.2
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label68" runat="server" Text="If yes Enter Previous Roll No. / यदि हाँ पिछले रोल नंबर दर्ज करें<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td width="60%" align="left">
                                    <table cellpadding="0" cellspacing="0" style="width: 64%">
                                        <tr>
                                            <td style="padding-left: 0" width="59%">
                                                <asp:TextBox ID="TxtRno" runat="server" Width="302px" MaxLength="15" TabIndex="2"
                                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/images/search_btn.jpg"
                                                    ToolTip="Serch" Height="22px" OnClick="ImgBtnSearch_Click" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImgBtnReset" runat="server" ImageUrl="~/images/reset_btn.jpg"
                                                    OnClick="ImgBtnReset_Click" Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table id="TblFormDetail" runat="server" class="sample3" style="width: 100%; text-align: left"
                            border="0" cellpadding="3" cellspacing="1">
                            <tr class="head1">
                                <td colspan="3">2.
                                <asp:Label ID="Label69" runat="server" Text="Applicant's Personal Details /आवेदक का व्यक्तिगत विवरण"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">2.1
                                </td>
                                <td>
                                    <asp:Label ID="Label70" runat="server" Text="Applicant's  Full Name / आवेदक का पूरा नाम <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="96px" runat="server" ID="ddlSalutaionName" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSalutaionName_SelectedIndexChanged" TabIndex="3" onclick="setDeclarartion();">
                                        <asp:ListItem Value="0" Selected="True" Text="--Select--">
                                        </asp:ListItem>
                                        <asp:ListItem Value="Mr." Text="Mr./श्री">
                                        </asp:ListItem>
                                        <asp:ListItem Value="Ms." Text="Ms./सुश्री">
                                        </asp:ListItem>
                                        <asp:ListItem Value="X" Text="Others/अन्य">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtAppName" runat="server" Width="430px" AutoPostBack="true" OnTextChanged="txtAppName_TextChanged" TabIndex="4" MaxLength="60"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <small style="color:red">Please fill carefully. Cannot be edited after Submitting</small>

                                    <br />

                                    ( Full name as per the highest / latest qualification certificate or legal certificate
                                )
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%">2.2
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="Care Of /  देखभाल <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="Rdoownertype" runat="server" RepeatDirection="Horizontal"
                                        Width="300px" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="Rdoownertype_SelectedIndexChanged">
                                        <asp:ListItem Value="P" Selected="true"> Parents / माता पिता  &nbsp;&nbsp;&nbsp; or/या</asp:ListItem>
                                        <asp:ListItem Value="G"> Guardian / संरक्षक    </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trguardian" runat="server" visible="false">
                                <td width="3%">2.2.1
                                </td>
                                <td>
                                    <asp:Label ID="Label82" runat="server" Text="Guardian's Name / संरक्षक का नाम <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel>
                                        <ContentTemplate>
                                            <asp:TextBox ID="TxtGuardianName" OnTextChanged="txtGuardianName_TextChanged" AutoPostBack="true" runat="server" MaxLength="60" TabIndex="5" Width="529px"
                                                autocomplete="off" onkeyup="setDeclarartion();" onpaste="return false;" oncopy="return false;"
                                                oncut="return false;">

                                            </asp:TextBox>
                                            <small style="color:red">Please fill carefully. Cannot be edited after Submitting</small>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />

                                    <span style="color: red;">( Full Name As Per Educational/Legal Certificate ) </span>
                                    <%--<div id="divgurdian" runat="server" style="display: none; position: absolute;" class="modalPopup5">
                                    Candidate submitting Guardian Name needs to submit the affidavit in prescribed format
                                    ( <a id="link" runat="server" target="_blank" style="text-decoration: none; color: black;
                                        font-weight: bold;">Click here to download the Format </a>) along with the examination
                                    form to the respective Regional Centre for further processing of examination form
                                    before the last date of filling of exam form , failing which, the examination form
                                    will not be processed and no fee will be refund. If successful in examination, the
                                    certificate will be issued with Guardian Name only.
                                    <div align="right">
                                        <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                    </div>
                                </div>--%>

                                    <div id="divgurdian" runat="server" style="display: none; position: absolute;" class="modalPopup5">
                                        Candidate submitting Guardian Name needs to submit the affidavit in prescribed format
                                    ( <a id="link" runat="server" target="_blank" style="text-decoration: none; color: black; font-weight: bold;">Click here to download the Format </a>) along with the examination form to 
                                    the respective Regional Centre for further processing of examination form up to seven(07) days time 
                                    after cut-off date of submission of OEAF, failing which, the examination form will not be processed 
                                    and no fee will be refund. If successful in examination, the certificate will be issued with 
                                    Guardian Name only.
                                    <div align="right">
                                        <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" />
                                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />--%>
                                    </div>
                                    </div>

                                </td>
                            </tr>
                            <tr class="gdrow1" id="trfather" runat="server" visible="false">
                                <td width="3%">2.2.1
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Father's Name / पिता का नाम <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="96px" runat="server" ID="DropDownList1" Enabled="False"
                                        TabIndex="6">
                                        <asp:ListItem Value="1" Text="Mr./श्री">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:UpdatePanel>
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtFatherName" OnTextChanged="txtFatherName_TextChanged" AutoPostBack="true" runat="server" MaxLength="60" TabIndex="7" Width="430px"
                                                autocomplete="off" onkeyup="setDeclarartion();" onpaste="return false;" oncopy="return false;"
                                                oncut="return false;">
                                            </asp:TextBox>
                                                <small style="color:red">Please fill carefully. Cannot be edited after Submitting</small>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <br />

                                    <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trmother" runat="server" visible="false">
                                <td width="3%">2.2.2
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Mother's Name / माता का नाम <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="96px" runat="server" ID="DropDownList2" Enabled="False"
                                        TabIndex="8">
                                        <asp:ListItem Value="2" Text="Mrs./श्रीमती">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:UpdatePanel>
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtMotherName" OnTextChanged="txtMotherName_TextChanged" AutoPostBack="true" runat="server" MaxLength="60" TabIndex="9" Width="430px"
                                                autocomplete="off" onkeyup="setDeclarartion();" onpaste="return false;" oncopy="return false;"
                                                oncut="return false;">
                                            </asp:TextBox>
                                                <small style="color:red">Please fill carefully. Cannot be edited after Submitting</small>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />

                                    <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trgender" runat="server">
                                <td width="3%">2.3
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Gender / लिंग<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <%--<asp:RadioButtonList ID="RdoGender" runat="server" RepeatDirection="Horizontal" TabIndex="10"
                                            Width="300px">
                                            <asp:ListItem Value="Male">Male / पुरुष
                                            </asp:ListItem>
                                            <asp:ListItem Value="Female">Female / महिला
                                            </asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                            <asp:DropDownList ID="ddl_gender" runat="server" Width="375px" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSalutaionName" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="trgdrow1calendar" id="trdob" runat="server">
                                <td width="3%">2.4
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक <font color='RED'>*</font> (dd-Mon-yyyy)"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="11" ID="txtDob" runat="server" SkinID="txtDate" Width="99px" AutoPostBack="true"
                                        TabIndex="11" onpaste="return false;" oncopy="return false;" oncut="return false;" OnTextChanged="txtDob_TextChanged">
                                    </asp:TextBox>

                                    <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                                    <br />
                                    ( As per high school certificate in 'dd-Mon-yyyy' format. i.e. '01-Jan-1990' )
                                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trcategory" runat="server">
                                <td width="3%">2.5
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Category / वर्ग<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" Width="375px" TabIndex="12">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="troccupation" runat="server">
                                <td width="3%">2.6
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Occupation / व्यवसाय <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlOccupation" runat="server" Width="375px" TabIndex="13">
                                                <%--OnSelectedIndexChanged="ddlOccupation_SelectedIndexChanged" AutoPostBack="true">--%>
                                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr id="Troccupation1" runat="server" visible="false" class="gdalternate1">
                                <td width="3%">2.6.1
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbdepartment" runat="server" Text="Department / विभाग <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="Txtdept" Width="364px" runat="server" MaxLength="75" onpaste="return false;"
                                        oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr id="Troccupation2" runat="server" visible="false" class="gdrow1">
                                <td width="3%">2.6.2
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbempcode" runat="server" Text="Employee Code / कर्मचारी कोड <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="Txtempcode" Width="364px" runat="server" MaxLength="30" onpaste="return false;"
                                        oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr id="Troccupation3" runat="server" visible="false" class="gdalternate1">
                                <td width="3%">2.6.3
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbldesg" runat="server" Text="Designation / पद <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtdesg" Width="364px" runat="server" MaxLength="75" onpaste="return false;"
                                        oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr id="Troccupation4" runat="server" visible="false" class="gdrow1">
                                <td width="3%">2.6.4
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblplace" runat="server" Text="Place of Posting(City) / पोस्टिंग की जगह (शहर) <font color='RED'>*</font> "></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtpostcity" Width="364px" runat="server" MaxLength="50" onpaste="return false;"
                                        oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr id="Troccupation5" runat="server" visible="false" class="trgdalternate1calendar">
                                <td width="3%">2.6.5
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbldjoin" runat="server" Text="Date of Joining / शामिल होने की तारीख <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox MaxLength="11" ID="txtDojoin" runat="server" SkinID="txtDate" Width="99px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <img id="imgjoin" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                                    <br />
                                    ( As per the format. i.e. '01-Jan-1990' )
                                <asp:CalendarExtender ID="ceDOjoin" TargetControlID="txtDojoin" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgjoin" runat="server">
                                </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr id="Troccupation6" runat="server" visible="false" class="trgdrow1calendar">
                                <td width="3%">2.6.6
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbldretment" runat="server" Text="Date of Retirement / सेवानिवृत्ति की तारीख <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox MaxLength="11" ID="txtDoretment" runat="server" SkinID="txtDate" Width="99px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <img id="imgretment" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                                    <br />
                                    ( As per the format. i.e. '01-Jan-1990' )
                                <asp:CalendarExtender ID="ceDOretment" TargetControlID="txtDoretment" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgretment" runat="server">
                                </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trdisability" runat="server">
                                <td width="3%">2.7
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label8" runat="server" Text="Disability / दिव्यांगता<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RdDisability" runat="server" OnSelectedIndexChanged="RdDisability_SelectedIndexChanged" AutoPostBack="True" RepeatDirection="Horizontal"
                                        TabIndex="18">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं </asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हाँ  </asp:ListItem>
                                    </asp:RadioButtonList>
                                    <div id="divDisability" runat="server" style="display: none; position: absolute;" class="modalPopup6">
                                        The eligible candidates who desire to use the services of scribe should submit 
                                     an application at the respective Regional Centre along with the supporting documents 
                                     as stated in the guidelines at <a
                                         id="link1" runat="server" target="_blank" style="text-decoration: none; color: red; font-weight: bold;"
                                         href="http://www.nielit.gov.in/sites/default/files/headquarter/pdf/SOP_OnlineExaminations_IT_Literacy_NIELIT_Scribe-Amended_12022019.pdf">this Link</a>
                                        and obtain requisite permission at least 15 days 
                                     before the commencement of the examination cycle.
                                    <div align="right">
                                        <asp:Button ID="Button1" runat="server" Text="OK" OnClick="btnOKD_Click" />
                                        <%--<asp:Button ID="Button2" runat="server" Text="Cancel" OnClick="btnCancelD_Click" />--%>
                                    </div>
                                    </div>

                                </td>
                            </tr>
                            <tr class="gdrow1" id="trDisability1" runat="server" visible="false">
                                <td width="3%">2.7.1
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label9" runat="server" Text="Type of Disability / दिव्यांगता प्रकार<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="DdlDisabilityType" runat="server" Width="185px">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trDisability2" runat="server" visible="false">
                                <td width="3%">2.7.2
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label18" runat="server" Text="Percentage of Disability / दिव्यांगता प्रतिशत<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtDisabilityPercent" Width="93px" runat="server" MaxLength="3" onkeypress="checkNumber(this,3,0,event);"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="tr1" runat="server">
                                <td width="3%">2.8
                                </td>
                                <td>
                                    <asp:Label ID="Label23" runat="server" Text="EWS / आर्थिक रूप से कमजोर वर्ग &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RdisEWS" runat="server" RepeatDirection="Horizontal" TabIndex="19">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं </asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हाँ </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>

                            <tr class="head1">
                                <td colspan="3">3.
                                <asp:Label ID="Label15" runat="server" Text="Contact Details / संपर्क विवरण"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">3.1
                                </td>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Phone with STD Code / दूरभाष एस टी डी कोड सहित"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtSTDcode" runat="server" onpaste="return false" Width="65px" MaxLength="5"
                                        TabIndex="14" onkeypress="checkNumber(this,4,0,event);" oncopy="return false;"
                                        oncut="return false;">
                                    </asp:TextBox>
                                    <asp:TextBox ID="txtCorPhoneNo" runat="server" onkeypress="checkNumber(this,10,0,event);"
                                        TabIndex="15" Width="280px" MaxLength="8" onpaste="return false;" oncopy="return false;"
                                        oncut="return false;">
                                    </asp:TextBox><br />
                                    (एसटीडी कोड) (दूरभाष संख्या)
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%">3.2
                                </td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="Mobile / मोबाइल <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorMobileCode" runat="server" Enabled="false" Text="+91" Width="65px"
                                        TabIndex="16" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <asp:TextBox ID="txtCorMobileNo" runat="server" MaxLength="10" oncopy="return false"
                                        oncut="return false" onkeypress="checkNumber(this,10,0,event);" onpaste="return false"
                                        TabIndex="17" Width="280px">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">3.3
                                </td>
                                <td>
                                    <asp:Label ID="Label19" runat="server" EnableTheming="True" Text="Email / ईमेल पता <font color='RED'>*</font> "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="150" TabIndex="18" Width="530px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox><br />
                                    (e.g.abc@yahoo.com)
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">4.
                                <asp:Label ID="Label74" runat="server" Text="Address Details / पता विवरण "></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">4.1
                                </td>
                                <td>
                                    <asp:Label ID="Label32" runat="server" Text="Address Line1/पता पंक्ति 1 <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAddressLine1" runat="server" Width="530px" TabIndex="19" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%" class="style1">4.2
                                </td>
                                <td class="style1">
                                    <asp:Label ID="Label20" runat="server" Text="Address Line2/पता पंक्ति 2 <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="TxtAddressLine2" runat="server" Width="530px" TabIndex="20" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">4.3
                                </td>
                                <td>
                                    <asp:Label ID="Label22" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAddressLine3" runat="server" Width="530px" TabIndex="21" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%">4.4
                                </td>
                                <td>
                                    <asp:Label ID="LblCity" runat="server" Text="City Name / शहर का नाम <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCity" runat="server" Width="530px" TabIndex="22" MaxLength="50"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">4.5
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="State / राज्य <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCorState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
                                        Width="375px" TabIndex="23">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="3%">4.6
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="District / जिला  <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="Ddldistrict" runat="server" Width="375px" TabIndex="24">
                                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCorState" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">4.7
                                </td>
                                <td>
                                    <asp:Label ID="Label39" runat="server" Text="Pin Code / पिन  कोड  <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPinCode" runat="server" MaxLength="6" oncopy="return false" oncut="return false"
                                        onpaste="return false" TabIndex="25" Width="93px" onkeypress="checkNumber(this,6,0,event);">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">5. Educational / Qualification Details / शैक्षिक / योग्यता का विवरण
                                </td>
                            </tr>
                            <tr class="gdalternate1" valign="top">
                                <td width="3%">5.1
                                </td>
                                <td>
                                    <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification / उच्चतम शैक्षिक योग्यता<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDLeducode" runat="server" Width="540px" SkinID="25" TabIndex="26">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1" valign="top">
                                <td width="3%">5.2
                                </td>
                                <td>
                                    <asp:Label ID="Label65" runat="server" Text="Year of  Passing / उत्तीर्ण वर्ष &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtYearOfPassing" runat="server" MaxLength="4" Width="93px" TabIndex="27"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TrIfOther" runat="server" visible="false" class="gdalternate1">
                                <td width="3%">5.2
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label81" runat="server" Text="If others specify / यदि अन्य, तो निर्दिष्ट करें "></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtEduIfOther" Width="530px" runat="server" MaxLength="50" TabIndex="28"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">6. Examination Details / परीक्षा विवरण&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td align="left" style="width: 3%" valign="top">6.1
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="Label11" runat="server" Text="Applied As / किसके रूप में आवेदन किया<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:RadioButtonList ID="RdoAppliedAs" runat="server" RepeatDirection="Horizontal"
                                        Height="16px" OnSelectedIndexChanged="RdoAppliedAs_SelectedIndexChanged" AutoPostBack="True"
                                        TabIndex="29">
                                        <asp:ListItem Value="D" Selected="True">Direct / सीधा </asp:ListItem>
                                        <asp:ListItem Value="I">Institute / संस्थान</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Label ID="lblApplicantType" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr id="TrAccState" runat="server" visible="false">
                                <td align="left" style="width: 3%" valign="top">6.1.1
                                </td>
                                <td align="left" style="width: 50%">
                                    <asp:Label ID="Lbstate" runat="server" Text="Select State of Accredited Institute  / मान्यता प्राप्त संस्थान के राज्य का चयन करें<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left" style="width: 50%" valign="top">
                                    <asp:DropDownList ID="DdlAccState" runat="server" Width="375px" OnSelectedIndexChanged="DdlAccState_SelectedIndexChanged"
                                        AutoPostBack="True" TabIndex="30">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrAccCentre" runat="server" visible="false">
                                <td align="left" style="width: 3%" valign="top">6.1.2
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label21" runat="server" Text="Select Centre Name of Accredited Institute / मान्यता प्राप्त संस्थान के केंद्र के नाम का चयन करें<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DdlAccCentre" runat="server" Width="555px" TabIndex="31" Height="22px">
                                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DdlAccCentre"
                                                PromptText="Type accredited institute name to search from the institute list"
                                                PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                                            </asp:ListSearchExtender>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td align="left" style="width: 3%" valign="top">6.2
                                </td>
                                <td align="left" valign="top" width="45%">
                                    <asp:Label ID="Label1" runat="server" Text="Applied for Examination / किस परीक्षा के लिए आवेदन किया<font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DdlExamCycle" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlExamCycle_SelectedIndexChanged"
                                                TabIndex="32" Width="185px">
                                                <asp:ListItem Value="0">--Select Exam Cycle--</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblExamDate" runat="server" Visible="false" Text="Exam Date:"></asp:Label>

                                            <asp:DropDownList ID="DdlApplied4Exam" runat="server" Width="185px" TabIndex="27"
                                                OnSelectedIndexChanged="DdlApplied4Exam_SelectedIndexChanged" AutoPostBack="True"
                                                Style="display: inline;">
                                                <asp:ListItem Value="0">--Select Exam Name--</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblFeeDetail" runat="server" Font-Size="Small"></asp:Label>
                                            <asp:ImageButton ID="ImgBtnPopupFee" runat="server" ImageUrl="~/images/DisablePopup.PNG"
                                                Enabled="False" />
                                            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" DropShadow="true"
                                                CancelControlID="ImgCancle1" TargetControlID="ImgBtnPopupFee" PopupControlID="InfoDiv">
                                            </asp:ModalPopupExtender>
                                            <div id="InfoDiv" runat="server" class="modalPopup">
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="5%">&nbsp;
                                                        </td>
                                                        <td width="90%"></td>
                                                        <td width="5%">
                                                            <asp:ImageButton ID="ImgCancle1" runat="server" ImageUrl="~/images/cancel.gif" Style="float: right;"
                                                                ToolTip="click to close" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">&nbsp;
                                                        </td>
                                                        <td align="center">
                                                            <div class="box">
                                                                <table style="width: 100%;" class="sample3" cellpadding="1" cellspacing="1">
                                                                    <tr class="head1">
                                                                        <td style="width: 65%">Fee Name
                                                                        </td>
                                                                        <td style="width: 35%">Fee Amt.(Rs.)
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="gdrow1" align="left">
                                                                        <td align="left">
                                                                            <asp:Label ID="LblFeeTypeName" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="padding-right: 5px;">
                                                                            <asp:Label ID="LblNormalFee" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="gdalternate1">
                                                                        <td align="left">Late Fee
                                                                        </td>
                                                                        <td align="right" style="padding-right: 5px;">
                                                                            <asp:Label ID="LblLateFee" runat="server" Text="0.00"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="gdrow1">
                                                                        <td align="left">CSC SPV Processing Charges
                                                                        </td>
                                                                        <td align="right" style="padding-right: 5px;">
                                                                            <asp:Label ID="lblProcessingFee" runat="server" Text="0.00"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="gdalternate1">
                                                                        <td align="left">Total Fee
                                                                        </td>
                                                                        <td align="right" style="padding-right: 5px;">
                                                                            <asp:Label ID="LblTotalFee" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="gdrow1">
                                                                        <td colspan="2" style="color: Red;" align="right">
                                                                            <asp:Label ID="LblAmountInWords" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td valign="top">&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrExamCentre1" runat="server">
                                <td align="left" style="width: 3%" valign="top">6.3
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="Label12" runat="server" Text="Examination Location 1 / परीक्षा केंद्र 1&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DdlExamCentreState1" runat="server" Width="185px" AutoPostBack="True"
                                                OnSelectedIndexChanged="DdlExamCentreState1_SelectedIndexChanged" TabIndex="33">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlExamCentre1" runat="server" Width="185px" AutoPostBack="True"
                                                OnSelectedIndexChanged="DdlExamCentre1_SelectedIndexChanged" TabIndex="34">
                                                <asp:ListItem Value="0">--Select Location--</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlExamCentre2" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrExamCentre2" runat="server">
                                <td align="left" style="width: 3%" valign="top">6.4
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="Label13" runat="server" Text="Examination Location 2 / परीक्षा केंद्र 2&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:UpdatePanel ID="Update" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DdlExamCentreState2" runat="server" AutoPostBack="True" Width="185px"
                                                OnSelectedIndexChanged="DdlExamCentreState2_SelectedIndexChanged" TabIndex="35">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlExamCentre2" runat="server" Width="185px" TabIndex="36">
                                                <asp:ListItem Value="0">--Select Location--</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlExamCentre1" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">7. Identification Details / पहचान की सूचना&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="Aadhaartr" runat="server">
                                <td align="left" style="width: 3%" valign="top">7.1
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="Label49" runat="server" Text="Aadhar Card Number / आधार कार्ड संख्या"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtaadhar" runat="server" MaxLength="12" TabIndex="39" Width="300px"
                                        ToolTip="12 digit Number" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                        onkeypress="checkNumber(this,12,0,event);">
                                    </asp:TextBox>
                                    <asp:UpdatePanel>
                                        <ContentTemplate>
                                            <asp:Button ID="btnSaveAadhar" runat="server" Text="Save Aadhaar" EnableTheming="false" OnClick="btnSaveAadhar_Click" />
                                            <asp:Button ID="btnClearAadhar" runat="server" Text="Clear Aadhaar" EnableTheming="false" OnClick="btnClearAadhar_Click" />
                                        </ContentTemplate>
                                       
                                    </asp:UpdatePanel>
                                     <asp:HiddenField ID="hfaadhaar" Value="" runat="server" />
                                </td>
                      
                            </tr>

                            <tr class="gdalternate1" id="UIDtr" runat="server" visible="false">
                                <td>7.1
                                </td>
                                <td>
                                    <asp:Label ID="UidTypeLbl" runat="server" Text="ID Card Type / पहचान पत्र  <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="UidTypeDdl" runat="server" Width="200px" Enabled="false">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Aadhaar Card</asp:ListItem>
                                        <asp:ListItem Value="2">PAN Card</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp &nbsp &nbsp
                                <asp:TextBox ID="UidNumberTxt" runat="server" MaxLength="50" ReadOnly="true" placeholder="ID Card Number"
                                    Width="300px" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="Apaartr" runat="server" visible="true">
                                <td>7.2</td>
                                <td>
                                    <asp:Label ID="Label26" runat="server" Text="Apaar&nbsp; ID / अपार आईडी  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtapaar" runat="server" MaxLength="12" TabIndex="49" Width="520px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;" AutoPostBack="true" Enabled="false" OnTextChanged="txtapaar_TextChanged" onkeypress="checkNumber(this,15,0,event);">
                                    </asp:TextBox>
                                    <a href="#" onclick="window.open('https://www.abc.gov.in')">Click here to generate Apaar Id</a>                            </td>
                            </tr>
                            <tr class="gdrow1" id="TrProviderPresent" runat="server" visible="true">
                                <td>7.2.1</td>
                                <td>
                                    <asp:Label ID="lblProviderPresent" runat="server"
                                        Text="Is Provider Present / क्या प्रदाता उपस्थित है <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIsProviderPresent" runat="server"
                                        Text="True" Width="520px" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrConsentRelation" runat="server" visible="true">
                                <td>7.2.3</td>
                                <td>
                                    <asp:Label ID="lblConsentRelation" runat="server"
                                        Text="Consent Relation / सहमति संबंध <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlConsentRelation" runat="server"
                                        Width="520px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlConsentRelation_SelectedIndexChanged">

                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Self" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Guardian" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Father" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Mother" Value="4"></asp:ListItem>

                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="Tr2" runat="server" visible="true">
                                <td>7.2.4</td>

                                <td>
                                    <asp:Label ID="Label27" runat="server"
                                        Text="Provider Name / प्रदाता का नाम <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>

                                <td>

                                    <asp:TextBox ID="txtproviderName" runat="server"
                                        MaxLength="100" Width="520px" Enabled="false">
                                    </asp:TextBox>


                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrAuthMode" runat="server" visible="true">
                                <td>7.2.5</td>
                                <td>
                                    <asp:Label ID="lblAuthMode" runat="server"
                                        Text="Authentication Mode / प्रमाणीकरण मोड <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAuthMode" runat="server" Width="520px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlAuthMode_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblauthidrules" Text="" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrAuthId" runat="server" visible="true">
                                <td>7.2.6</td>
                                <td>
                                    <asp:Label ID="lblAuthId" runat="server"
                                        Text="Authentication ID No / प्रमाणीकरण आईडी संख्या <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updAuth">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtAuthenticationIdNo" runat="server"
                                                MaxLength="50" Width="520px"
                                                onkeypress="return isNumberKey(event);"
                                                onpaste="return false;" oncopy="return false;" oncut="return false;">
                                            </asp:TextBox>
                                            <br />
                                            <small runat="server" id="authidrule">Rule for entering Authentication ID</small>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlAuthMode" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>


                            <tr class="gdalternate1" id="TrConsentDate" runat="server" visible="true">
                                <td>7.2.7</td>
                                <td>
                                    <asp:Label ID="lblConsentDate" runat="server"
                                        Text="Consent Date / सहमति दिनांक <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentDate" runat="server"
                                        Width="520px" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrConsentTime" runat="server" visible="true">
                                <td>7.2.8</td>
                                <td>
                                    <asp:Label ID="lblConsentTime" runat="server"
                                        Text="Consent Time / सहमति समय <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentTime" Enabled="false" runat="server"
                                        Width="520px">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrConsentPlace" runat="server" visible="true">
                                <td>7.2.9</td>
                                <td>
                                    <asp:Label ID="lblConsentPlace" runat="server"
                                        Text="Consent Place / सहमति स्थान <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentPlace" MaxLength="30" oninput="sanitizeSpace(this);" onpaste="return false" runat="server"
                                        Width="520px">
                                    </asp:TextBox>
                                    <br />
                                    <small>Do not enter a full address. Maximum 30 characters. Only Two words, alphabets and ' allowed.
                                    </small>
                                </td>
                            </tr>

                            <tr class="gdalternate1">
                                <td align="left" valign="top" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td></td>

                                            <td valign="top">
                                                <asp:CheckBox ID="chkApaarDeclaration"
                                                    runat="server"
                                                    TabIndex="53" />
                                            </td>

                                            <td id="tdApaarDeclaration"
                                                runat="server"
                                                style="text-align: justify;">

                                                <font color='RED'>*</font>


                                                <asp:Label ID="lblApaarDeclaration" Text=" I, hereby voluntarily give my consent to NIELIT to use APAAR ID of [Applicant Name] with APAAR ID as for validation of personal details.I understand that the APAAR ID may be used and shared only for limited, authorized purposes, and that the information provided by me shall be kept confidential.The information w.r.t authentication document no. provided by me, is correct and valid to the best of my knowledge."
                                                    runat="server">
                                                </asp:Label>



                                                <br />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr class="gdrow1">
                                <td align="left" style="width: 3%" valign="top">7.3
                                </td>

                                <td align="left" valign="top">
                                    <asp:Label ID="ImagePathPh" runat="server" Text="" Style="display: none"></asp:Label>
                                    <asp:Label ID="lblphoto" runat="server" Text="Upload Photo / फोटो अपलोड करें <font color='RED'>*</font>"></asp:Label>
                                    <br />
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="ImgUpload" runat="server" onkeypress="return false;" TabIndex="37" Width="220px" />
                                            <asp:Button ID="UploadPhoto" runat="server" Text="Upload Photo" OnClick="UploadPhoto_Click" Style="display: none" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="UploadPhoto" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:Panel Font-Size="11px" ID="InstructionsForImagesPhoto" runat="server">
                                        (Only JPEG/JPG image allowed with the size 5 KB to 50 KB, Dimension should be 132X170 pixels (width 132 pixels & height 170 pixels) and DPI 96 to 300)<br />
                                        (i) The colour photos taken professionally (not on a mobile phone) during the last six months with white background should be used.<br />
                                        (ii) The facial features of the person should be clearly visible and no goggles to be used and no part of the face should be covered.
                                    </asp:Panel>
                                </td>
                                <td align="left" valign="top">
                                    <asp:Image runat="server" ID="photoPreview" alt="Photo" Width="110" Height="130"></asp:Image>
                                    <asp:Label runat="server" ID="lblphotoShow" Visible="False" Font-Bold="False" Font-Size="10pt"
                                        ForeColor="Red"></asp:Label>
                                    <br />


                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    
                                      <asp:Button ID="btnNotification" runat="server" Text="Help/Tip to Resize Image" TabIndex="43" Width="157px" OnClick="btnNotification_Click1" />
                                    <div id="divPopup" runat="server" class="modalPopup2">
                                        1.  Click on the “
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/window.png" Height="19px" Width="23px" />
                                        ” key and search for paint<br />
                                        2.Open ‘MS Paint’ in your computer<br />
                                        3.Press ‘Ctrl+O’ and browse to the scanned images of your Photograph/Signature/LTI.<br />
                                        4.Now, press ‘Ctrl+W’ to open “Resize and Skew” window<br />
                                        <asp:Image ID="imgPhoto" runat="server" ImageUrl="~/images/resize.jpg" />
                                        <br />
                                        5.Switch to Pixels instead of Percentage<br />
                                        6.Uncheck the "Maintain aspect ratio" checkbox.<br />
                                        7.Enter the specified pixels mentioned in notification for Photograph/Signature/LTI<br />
                                        8.Go to File tab and click on “Save As”<br />
                                        9.Choose ‘JPG/JPEG’ format and save the image in the desired location on your desktop.

                                                      <div align="center">
                                                          <asp:Button ID="Button3" runat="server" Text="OK" OnClick="btnOKD_Click" Style="font-size: small" />
                                                          <%--<asp:Button ID="Button4" runat="server" Text="Cancel" OnClick="btnCancelD_Click" />--%>
                                                      </div>
                                    </div>


                                </td>
                            </tr>

                            <tr class="gdalternate1">
                                <td align="left" style="width: 3%" valign="top">7.4
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="ImagePathSig" runat="server" Text="" Style="display: none"></asp:Label>
                                    <asp:Label ID="lblsign" runat="server" Text="Upload Signature / हस्ताक्षर अपलोड करें <font color='RED'>*</font>"></asp:Label><br />
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="ImgUploadSignature" runat="server" onkeypress="return false;" TabIndex="37" Width="220px" />
                                            <asp:Button ID="UploadSignature" runat="server" Text="Upload Signature" OnClick="UploadSignature_Click" Style="display: none" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="UploadSignature" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:Panel Font-Size="11px" ID="InstructionsForImagesSig" runat="server">
                                        (Only JPEG/JPG image allowed with the size 5 KB to 20 KB, Dimension should be 170X132 pixels (width 170 pixels & height 132 pixels) and DPI 96 to 200)<br />
                                        (i) Signature should be taken on white paper using black/blue pen.<br />
                                        (ii) Image should not be blurred or smudged.
                                    </asp:Panel>
                                </td>
                                <td align="left" valign="top">
                                    <asp:Image runat="server" ID="signPreview" alt="Signature" Width="200" Height="80"></asp:Image>
                                    <asp:Label runat="server" Text="" ID="lblsignShow" Visible="false" Font-Bold="False"
                                        Font-Size="10pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>

                            <tr class="gdrow1">
                                <td align="left" style="width: 3%" valign="top">7.5
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="ImagePathLt" runat="server" Text="" Style="display: none"></asp:Label>
                                    <asp:Label ID="lblThumb" runat="server" Text="Upload left thumb impression / बांए हाथ के अंगूठे का निशान अपलोड करें<font color='RED'>*</font>"></asp:Label><br />
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="ImgUploadThumb" runat="server" onkeypress="return false;" TabIndex="38" Width="220px" />
                                            <asp:Button ID="UploadThumb" runat="server" Text="Upload Thumb" OnClick="UploadThumb_Click" Style="display: none" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="UploadThumb" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:Panel Font-Size="11px" ID="InstructionsForImagesLt" runat="server">
                                        (Only JPEG/JPG image allowed with the size 5 KB to 20 KB, Dimension should be 170X132 pixels (width 170 pixels & height 132 pixels) and DPI 96 to 200)<br />
                                        (i) Left thumb impression should be taken on white paper using black/blue ink.<br />
                                        (ii) Image should not be blurred or smudged.
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Image runat="server" ID="thumbPreview" alt="Left Thumb Impression" Width="150" Height="90"></asp:Image>
                                    <asp:Label runat="server" Text="" ID="lblthumbshow" Visible="false" ffont-bold="False"
                                        Font-Size="10pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>

                            <tr class="gdalternate1">
                                <td colspan="3">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 45%; margin-left: 364px;" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" style="width: 90%" valign="middle">
                                                        <span style="font-size: 12px; font-weight: bold; margin-left: -75px;">Please enter the
                                                        following number you see into the textbox below.</span>
                                                        <img id="imgcap" runat="server" alt="Capture Code" width="150" height="55" src=""
                                                            style="margin-left: 18px;" />
                                                        <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.jpg"
                                                            Width="30px" Style="vertical-align: top; padding-top: 0px; border: none;" ToolTip="Click to renew captcha."
                                                            OnClick="ImgBtnRefresh_Click" />
                                                    </td>
                                                    <td align="left" style="width: 10%">
                                                        <asp:HiddenField ID="HfCaptcha" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtcode" runat="server" MaxLength="6" TabIndex="40" Width="140px"
                                                            onkeyup="setDeclarartion();" autocomplete="off" onpaste="return false;" oncopy="return false;"
                                                            oncut="return false;" Style="margin-left: 17px;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="head1" id="trenclosure" runat="server" visible="false">
                                <td id="tdencheading" runat="server" colspan="3">8. Enclosures / भेजें
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trenclosure1" runat="server" visible="false">
                                <td align="left" valign="top" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="style107"></td>
                                            <td>
                                                <asp:CheckBox ID="ChkMarksheetCopy" runat="server" TabIndex="41" Text="<font color='RED'>*</font>"
                                                    Enabled="False" Checked="True" />
                                                Attested copy of Mark sheet of Highest Qualification Obtained by the Candidate(अभ्यर्थी
                                            की उच्चतम शैक्षिक योग्यता प्रमाणपत्र की सत्यापित प्रतिलिपि)
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3" id="tddeclarartion" runat="server">8. Declaration / घोषणा
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td align="left" valign="top" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="style107"></td>
                                            <td style="text-align: justify;">
                                                <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="42" Text="<font color='RED'>*</font>" />
                                                I
                                            <asp:Label ID="LblName" runat="server" Text=""></asp:Label>,
                                            <asp:Label ID="Lblsalutation" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="LblDecMName" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="LblDecFname" runat="server" Text=""></asp:Label>
                                                hereby declare that the particulars submitted by me in the online examination application
                                            form of
                                            <asp:Label runat="server" Text="" ID="lbldeccoursecode"></asp:Label>
                                                are true to the best of my knowledge and belief. I agree to abide by the rules and
                                            regulations of NIELIT and also to the decision of NIELIT regarding my admission
                                            for the examination. I have noted that, NIELIT has the right to withhold my result
                                            even after my appearing in the examination in addition to any other action as may
                                            be deemed fit in the event of any of the statements/particulars made above being
                                            found incorrect. I have noted that, I might be required to appear in the examination
                                            at any other examination center not specified under examination center choice column
                                            (6.3,6.4). I further understand that, in the event of non-conduction /non-evaluation
                                            of my examination due to any reason whatsoever, I will not held NIELIT responsible
                                            for any damages.
                                            <br />
                                                मैं
                                            <asp:Label ID="LblhName" runat="server" Text=""></asp:Label>,<asp:Label ID="LblDechmName"
                                                runat="server" Text=""></asp:Label>
                                                <asp:Label ID="LblDechfName" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="Lblhsalutation" runat="server" Text=""></asp:Label>
                                                यह घोषणा
                                            <asp:Label ID="Lblhdectype" runat="server" Text="करता/ करती"></asp:Label>
                                                &nbsp;हूँ कि
                                            <asp:Label runat="server" Text="" ID="lblhdeccoursecode"></asp:Label>
                                                परीक्षा हेतु ऑनलाइन आवेदन फार्म में मेरे द्वारा वर्णित समस्त जानकारी मेरे ज्ञान
                                            और विश्वास से सत्य है। मैं रा.इ.सू.प्रौ.सं. के नियमों व विनियमों तथा मेरी परीक्षा
                                            हेतु प्रवेश से संबंधित रा.इ.सू.प्रौ.सं. के निर्णयों से भी सहमत हूँ। मुझे ज्ञात है
                                            कि रा.इ.सू.प्रौ.सं. को उपर्युक्त उल्लिखित किसी भी प्रकार का कथन/ विवरण असत्य पाये
                                            जाने पर परीक्षा में मेरी उपस्थिति के पश्चात भी मेरा परिणाम रोके जाने के अतिरिक्त
                                            अन्य कोई कार्रवाई जो उचित समझी जाए, अधिकार है। मुझे ज्ञात है कि मेरी परीक्षा केंद्र
                                            चयन कॉलम(6.3,6.4) के अंतर्गत किसी भी अन्य परीक्षा केंद्र में करायी जा सकती है। आगे,
                                            मैं यह भी समझता हूँ कि किसी भी प्रकार के कारणवश मेरी परीक्षा के अनिर्धारण/अवमूल्यांकन
                                            की स्थिति में, मेरी किसी प्रकार की क्षति के लिए रा.इ.सू.प्रौ.सं.उत्तरदायी नहीं होगा।
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Proceed" TabIndex="43" OnClick="btnSave_Click"
                                        OnClientClick="return ValidateForm();" Style="height: 26px" />
                                    <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="44" OnClick="btnback_Click" />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:ModalPopupExtender ID="AlertModalPopUp" runat="server" PopupControlID="PopUpPanel"
            PopupDragHandleControlID="PopupHeader" TargetControlID="HiddenField2" OkControlID="CancleBtn"
            X="300" Y="300">
        </asp:ModalPopupExtender>
        <asp:Panel ID="PopUpPanel" runat="server" BorderColor="#990000" BackColor="AliceBlue"
            BorderStyle="Solid" Height="160px" Style="width: 500px" CssClass="modalPopup"
            align="center">
            <h2>Alert!</h2>
            <p>
                <asp:Label ID="Lblmsg" runat="server" Text=" Please complete your application in one go to avoid any problem."></asp:Label>
            </p>
            <asp:Button ID="CancleBtn" runat="server" Style="text-align: right;" Text="Close" />
        </asp:Panel>
    </form>
</body>
</html>
