<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="NielitRegistration.aspx.cs" Inherits="NielitRegistration" Debug="True" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <%-- <script type="text/javascript" src="https://code.jquery.com/jquery-1.8.2.js"> </script>--%>
    <script src="../Script/jquery-3.7.1.min.js" type="text/javascript"></script>
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
        //Added by Amit start
        function udiseValidation(input) {
            input.value = input.value.toUpperCase().replace(/[^A-Za-z0-9\-]/g, '');
        }

        function classValidation(input) {
            input.value = input.value.replace(/[^0-9]/g, '').substring(0, 2);
        }

        function rnvalidation(input) {
            input.value = input.value.replace(/[^A-Za-z0-9\-]/g, '');
        }

        //Added by Amit end}

        //-------------- added_by_amit_audit_april_2026_start image upload code ----------------------/
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("UploadPhoto").click();
            }
        }
        function imageSignpreview(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("UploadSignature").click();
            }
        }
        function imageThumbpreview(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("UploadThumb").click();
            }
        }
        //--------------  added_by_amit_audit_april_2026_end image upload code ----------------------/


        /*
        function imagePhotopreview(FileUpload) {
            var fildr = new FileReader();
            fildr.onload = function (e) {
                $('#photoPreview').attr('src', e.target.result);
            }
            fildr.readAsDataURL(FileUpload.files[0]);
        }
        function imageSignpreview(FileUpload) {
            var fildr = new FileReader();
            fildr.onload = function (e) {
                $('#signPreview').attr('src', e.target.result);
            }
            fildr.readAsDataURL(FileUpload.files[0]);
        }
        function imageThumbpreview(FileUpload) {
            var fildr = new FileReader();
            fildr.onload = function (e) {
                $('#thumbPreview').attr('src', e.target.result);
            }
            fildr.readAsDataURL(FileUpload.files[0]);
                }*/
    </script>
    <script type="text/javascript">
        //function preventBack() { window.history.forward(); }
        //setTimeout("preventBack()", 0);
        //window.onunload = function () { null };
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
        function validatePreviousRegistration() {
            if (!isSelected("DDL_PreviousLevel", "Course"))
                return false;
            if (!isBlankNumber("Txt_Regno", "Registration Number"))
                return false;
            if (!isNumber("Txt_Regno"))
                return false;
            return true;
        }
        function ValidateForm() {

            if (!isSelected("DDL_PreviousLevel", "Course"))
                return false;
            if (!isBlankNumber("Txt_Regno", "Registration Number"))
                return false;
            if (!isNumber("Txt_Regno"))
                return false;
            if (!isSelected("DDLRegForCourse", "Course"))
                return false;

            if (!isSelected("DdlAccState", " State of Accredited Institute"))
                return false;
            if (!isSelected("DdlAccDistrict", " District of Accredited Institute"))
                return false;
            if (!isSelected("DdlAccCentre", " Centre of Accredited Institute"))
                return false;
            //            if (!isSelected("DDL_ExamName", "Exam Name"))
            //                return false;
            if (!isSelected("ddlSalutaionName", "Salutation"))
                return false;
            if (!isBlank("txtAppName", "Applicant Name"))
                return false;
            if (!CheckNumberPresent("txtAppName", "Numeric characters are not allowed"))
                return false;
            if (!isSpecialCharacterName("txtAppName", "Special characters are not allowed"))
                return false;
            if (!isvalidateRadioButtonList("Rdoownertype", "   "))
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

                //Added 22-Feb-2019
                if (!isBlank("txtAffidavitNo", "Affidavit No."))
                    return false;
                if (!isSpecialCharacterAffidavit("txtAffidavitNo", "Special characters are not allowed"))
                    return false;
                if (!isBlankDate("txtAffidavitDate", "Affidavit Date", "dd-MMM-yyyy"))
                    return false;
                if (!isDate("txtAffidavitDate", "Invalid Affidavit date", "dd-MMM-yyyy"))
                    return false;
                if (!isBlank("fileAffidavit", " Upload Affidavit"))
                    return false;
                //

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

            //if (!isvalidateRadioButtonList("rdbtnlstgender", "Gender"))
            //    return false;
            if (!isSelected("ddl_gender", "Gender")) //jksah
                return false;
            if (!isBlankDate("txtDob", "Date Of Birth", "dd-MMM-yyyy"))
                return false;
            if (!isDate("txtDob", "Invalid date of birth", "dd-MMM-yyyy"))
                return false;
            //date of birth validation                        
            var Dobdate = document.getElementById("txtDob").value;
            var result = Date.today().addYears(-10).compareTo(Date.parse(Dobdate));
            if (result == -1) {
                callErrorMsg("txtDob", "Not Applicable to apply for exam");
                return false;
            }
            if (!isSelected("ddlMStatus", "Marital status"))
                return false;
            if (!isSelected("ddlCategory", "Cast Category"))
                return false;
            if (!isSelected("ddlReligion", "Religion"))
                return false;
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

            if (document.getElementById("chkSame").checked == false) {
                if (!isBlank("TxtCorAddressLine1", "correspondence AddressLine 1"))
                    return false;
                if (!isBlank("TxtCorAddressLine2", "correspondence AddressLine 2"))
                    return false;
                if (!isBlank("TxtCorCity", "correspondence City Name"))
                    return false;
                if (!isSelected("ddlCorState", "State"))
                    return false;
                if (!isSelected("ddldistrict", "District"))
                    return false;
                if (!isBlankNumber("txtCorPinCode", "Pin Code"))
                    return false;
                if (!isNumber("txtCorPinCode"))
                    return false;
                if (!IsValidMinMaxLenght("txtCorPinCode", 6, 6, "Invalid Pin Code"))
                    return false;
            }

            if (!isSelected("DDLeducode", "Highest Education"))
                return false;
            if (!isBlank("TxtEduIfOther", "If Other"))
                return false;
            if (!isBlank("TxtYearOfPassing2", "Year of Passing"))
                return false;
            if (!isNumber("TxtYearOfPassing2"))
                return false;
            //if (!isValidPassingYear("TxtYearOfPassing2", "txtDob"))
            //  return false;
            if (document.getElementById("TxtExperienceInYears")) {

                if (document.getElementById("HfExperience").value != "0") {
                    if (!isBlank("TxtExperienceInYears", "Experience in Year"))
                        return false;
                    if (!isNumber("TxtExperienceInYears"))
                        return false;
                    if (Number(document.getElementById("TxtExperienceInYears").value) >= 10) {
                        callErrorMsg("TxtExperienceInYears", "Experience should be less than 10 years.");
                        return false;
                    }
                    if (Number(document.getElementById("TxtExperienceInYears").value) < Number(document.getElementById("HfExperience").value)) {
                        callErrorMsg("TxtExperienceInYears", "Minimum " + document.getElementById("HfExperience").value + " years of experience required");
                        return false;
                    }
                }
                else {
                    if (!isBlank("TxtExperienceInYears", "Experience in Year"))
                        return false;
                    if (!isNumber("TxtExperienceInYears"))
                        return false;
                    if (Number(document.getElementById("TxtExperienceInYears").value) >= 10) {
                        callErrorMsg("TxtExperienceInYears", "Experience should be less than 10 years.");
                        return false;
                    }
                }
            }
            if (UIDtr != null) {
                if (!isSelected('UidTypeDdl', "ID Card Type"))
                    return false;
                if (!isBlank('UidNumberTxt', "ID Card Number"))
                    return false;
            }

            if (!isBlank("ImgUpload", " Upload Image"))
                return false;
            if (!isvalidImageFile("ImgUpload", "Photo"))
                return false;
            if (!isBlank("ImgUploadSignature", "Upload Signature"))
                return false;
            if (!isvalidImageFile("ImgUploadSignature", "Signature"))
                return false;
            if (!isBlank("ImgUploadThumb", "Upload Thumb Impression"))
                return false;
            if (!isvalidImageFile("ImgUploadThumb", "Thumb"))
                return false;
            if (!isBlank("txtBodyMark", "Body Mark"))
                return false;
            if (!isBlank("txtcode", "Captcha Code"))
                return false;
            if (!ischecked("chkdisclamier", "Declaration"))
                return false;
            if (!isNumber("txtapaar"))
                return false;
            //amit_apaar_may_2026_start

            if (!isBlank("txtapaar"))
                return false;

            if (!isNumber("txtapaar"))
                return false;

            if (!isSelected("ddlConsentRelation", "Consent Relation"))
                return false;

            if (!isBlank("txtproviderName", "Captcha Code"))
                return false;

            if (!isSelected("ddlAuthMode", "Consent Relation"))
                return false;

            if (!isBlank("txtAuthenticationIdNo", "Authentication ID No"))
                return false;

            if (!isBlank("txtConsentDate", "Consent Place"))
                return false;

            if (!isBlank("txtConsentTime", "Consent Place"))
                return false;

            if (!isBlank("txtConsentPlace", "Consent Place"))
                return false;

            //amit_apaar_may_2026_end
            //Added_30_12_2024 , Added by Amit start


            var selectedRadio = document.querySelector(`input[name="radProject"][value="1"]`);

            if (selectedRadio.checked) {

                if (document.getElementById("txtUDISECode").style.display !== "none") {
                    //if (!isBlank("txtUDISECode", "Field 1"))
                    if (!isBlank("txtUDISECode", "" + document.getElementById("Label14").innerText))
                        return false;
                }

                if (!isSelected("DdlProject", "Project of Student"))
                    return false;

                if (document.getElementById("txtField2").style.display !== "none") {
                    //if (!isBlank("txtField2", "Field 2"))
                    if (!isBlank("txtField2", "" + document.getElementById("lblField2").innerText))
                        return false;
                }
                if (document.getElementById("txtField3").style.display !== "none") {
                    //if (!isBlank("txtField3", "Field 3"))
                    if (!isBlank("txtField3", "" + document.getElementById("lblField3").innerText))
                        return false;
                }
                if (document.getElementById("txtField4").style.display !== "none") {
                    //if (!isBlank("txtField4", "Field 4"))
                    if (!isBlank("txtField4", "" + document.getElementById("lblField4").innerText))
                        return false;
                }
                if (document.getElementById("txtField5").style.display !== "none") {
                    //if (!isBlank("txtField5", "Field 5"))
                    if (!isBlank("txtField5", "" + document.getElementById("lblField5").innerText))
                        return false;
                }
            }

            //Added by amit end
            //var letters = /^[A-Za-z]+$/;

            //if (!letters.test(document.getElementById("txtAppName").value)) {
            //    alert("Valid characters: English Alphabets Only");
            //    document.getElementById('txtAppName').focus();
            //    return false;
            //}
            //return true;

        }
        function blankFnameMname() {
            document.getElementById("txtFatherName").value = "";
            document.getElementById("txtMotherName").value = "";
        }
        function blankGuardianName() {
            document.getElementById("TxtGuardianName").value = "";
        }
        //function onlyAlphabets(e, t) {
        //    alert('alpha')
        //    if (!((e.charCode > 64 && e.charCode < 91) || (e.charCode > 96 && e.charCode < 123) || e.charCode == 32))
        //        alert('Valid characters: English Alphabets Only')
        //    return;
        //}
    </script>
    <style type="text/css">
        .modalPopup {
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

        .modalPopup1 {
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
        /*Added 25 April 2019*/
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
    </style>
</head>
<body style="background-color: white;" oncontextmenu="return false;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" style="margin-bottom: 20px">
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>


                <tr>
                    <td align="center">
                        <strong style="text-align: center" class="headfont">REGISTRATION APPLICATION FORM FOR
                        COURSE –
                        <asp:Label ID="Lblhead" runat="server" Text=""></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
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
                                <td align="left" colspan="3">1. Registration Details / पंजीयन का विवरण
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrPreMIS" runat="server">
                                <td width="3%" valign="top">1.0
                                </td>
                                <td width="40%" valign="top">Have you got MIS online reference number for enrollment
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
                                    <asp:Label ID="Label25" runat="server" Text="If yes Enter Previous Reference No. / यदि हाँ पिछले संदर्भ नंबर दर्ज करें<font color='RED'>*</font>">
                                    </asp:Label>
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
                                                <asp:ImageButton ID="ImgBtnReset" runat="server" ImageUrl="~/images/reset_btn.jpg"
                                                    OnClick="ImgBtnReset_Click" Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>



                            <tr class="gdalternate1">
                                <td width="3%" valign="top" style="padding-right: 13px;">1.1
                                </td>
                                <td valign="top" width="40%">
                                    <asp:Label ID="Label1" runat="server" Text="Registration sought for / के लिए पंजीयन &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td width="57%">
                                    <asp:DropDownList ID="DDLRegForCourse" runat="server" Width="375px" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table id="TblFormDetail" runat="server" class="sample3" style="width: 100%; text-align: left"
                            border="0" cellpadding="3" cellspacing="1">
                            <tr class="gdalternate1" id="TrApplicantType" runat="server">
                                <td width="3%" valign="top">1.2
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label89" runat="server" Text="Applied As / किसके रूप में आवेदन किया<font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td width="57%">
                                    <asp:RadioButtonList ID="RdoUndergngDOEACC" runat="server" RepeatDirection="Horizontal"
                                        TabIndex="2" Width="312px" AutoPostBack="True" OnSelectedIndexChanged="RdoUndergngDOEACC_SelectedIndexChanged"
                                        Style="height: 27px">
                                        <asp:ListItem Value="D" Selected="True">Direct Candidate</asp:ListItem>
                                        <asp:ListItem Value="I">Through Institute</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Label ID="lblApplicantType" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <%--  Added by  Amit--%>
                            <tr class="gdalternate1" id="TrUPBoard" visible="false" runat="server">
                                <td width="3%" valign="top">
                                    <asp:Label ID="lblproject" runat="server" Text="1.2.0"></asp:Label>
                                </td>
                                <td width="40%" valign="top">
                                    <asp:Label ID="Label16" runat="server" Text="Whether student belongs to Project"></asp:Label>
                                </td>
                                <td width="57%">
                                    <asp:RadioButtonList ID="radProject" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" Width="364px" OnSelectedIndexChanged="radProject_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <%-- Added by  Amit--%>
                            <%--
                         ## LIVE BSB CODE
                        <tr class="gdrow1" id="trBSB" runat="server" visible ="false">
                            <td> <asp:Label ID="Label13" runat="server">1.2.1</asp:Label></td>
                            <td valign="top">
                                <asp:Label ID="Label14" runat="server" Text="Enter U-DISE Code">
                                </asp:Label>
                            </td>
                            <td align="left" style="width: 50%">
                               <asp:TextBox ID="txtUDISECode" runat ="server"></asp:TextBox>
                            </td>
                            </tr>
                            --%>
                            <tr class="gdrow1" id="TrLastCenterAccno" runat="server">
                                <td valign="top">
                                    <asp:Label ID="Lbl2" runat="server">1.2.1</asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:Label ID="Label9" runat="server" Text="Select State of Accredited Institute / मान्यता प्राप्त संस्थान के राज्य का चयन करें <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td align="left" style="width: 50%">
                                    <asp:DropDownList ID="DdlAccState" runat="server" Width="375px" OnSelectedIndexChanged="DdlAccState_SelectedIndexChanged"
                                        AutoPostBack="True" TabIndex="3">
                                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <%-- <tr class="gdalternate1" id="TrLastCenterInstiName" runat="server">
                            <td valign="top">
                                1.2.2
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label62" runat="server" Text="Select District of Accredited Institute / मान्यता प्राप्त संस्थान के जिले का चयन करें <font color='RED'>*</font>"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DdlAccDistrict" runat="server" TabIndex="8" Width="375px" OnSelectedIndexChanged="DdlAccDistrict_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                            <tr id="TrAccCentre" runat="server" visible="false" class="gdalternate1">
                                <td align="left" style="width: 3%" valign="top">
                                    <asp:Label ID="Lbl3" runat="server">1.2.2</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label12" runat="server" Text="Select Centre Name of Accredited Institute / मान्यता प्राप्त संस्थान के केंद्र के नाम का चयन करें<font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td align="left" valign="top">
                                  <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:DropDownList ID="DdlAccCentre" runat="server" Width="526px" AutoPostBack="True"
                                                TabIndex="5" OnSelectedIndexChanged="DdlAccCentre_SelectedIndexChanged">
                                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            </asp:DropDownList>
                                       <%-- </ContentTemplate>--%>
                                        <%--<Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                                        </Triggers>--%>
                                    <%--</asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <%-- Added by Amit start --%>
                            <tr class="gdalternate1" id="trUPProject" runat="server" visible="false">
                                <td valign="top">
                                    <asp:Label ID="Label23" runat="server" Text="1.2.3"></asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:Label ID="Label26" runat="server" Text="Select Project&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left" style="width: 50%">
                                    <asp:DropDownList ID="DdlProject" runat="server" Width="375px" OnSelectedIndexChanged="DdlProject_SelectedIndexChanged"
                                        AutoPostBack="True" TabIndex="3">
                                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trBSB" runat="server" visible="false">
                                <td>
                                    <%-- <asp:Label ID="Label13" runat="server">1.2.4</asp:Label></td>--%>
                                    <asp:Label ID="Label13" runat="server" Text="1.2.4"></asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:Label ID="Label14" runat="server" Text="Enter U-DISE Code&lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <%--added by amit start,  bsb--%>
                                <td align="left" style="width: 50%">
                                    <asp:TextBox ID="txtUDISECode" oncopy="return false;"
                                        oncut="return false;" onpaste="return false;" oninput="udiseValidation(this)" runat="server"></asp:TextBox>
                                    <!-- <asp:DropDownList ID="ddlUDISECode" runat="server" Visible="false">
                                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    </asp:DropDownList>-->
                                </td>
                                <%--added by amit end,  bsb--%>
                            </tr>
                            <tr class="gdrow1" id="trUPField2" runat="server" visible="false">
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblSrField2" runat="server" Text="1.2.5"></asp:Label>
                                </td>
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblField2" runat="server" Text="Field 2&lt;font color='RED'&gt;*&lt;/font&gt;" EnableHtmlEncode="false"></asp:Label>
                                </td>

                                <td style="margin-left: 40px" class="auto-style1">
                                    <asp:TextBox ID="txtField2" onpaste="return false;"
                                        oninput="classValidation(this)"
                                        oncopy="return false;"
                                        oncut="return false;" runat="server" />
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trUPField3" runat="server" visible="false">
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblSrField3" runat="server" Text="1.2.6"></asp:Label>
                                </td>
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblField3" runat="server" Text="Field 3&lt;font color='RED'&gt;*&lt;/font&gt;" EnableHtmlEncode="false"></asp:Label>
                                </td>
                                <td style="margin-left: 40px" class="auto-style1">
                                    <asp:TextBox ID="txtField3" onpaste="return false;"
                                        oninput="rnvalidation(this)"
                                        oncopy="return false;"
                                        oncut="return false;" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trUPField4" runat="server" visible="false">
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblSrField4" runat="server" Text="1.2.7"></asp:Label>
                                </td>
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblField4" runat="server" Text="Field 4&lt;font color='RED'&gt;*&lt;/font&gt;" EnableHtmlEncode="false"></asp:Label>
                                </td>
                                <td style="margin-left: 40px" class="auto-style1">
                                    <asp:TextBox ID="txtField4" onpaste="return false;"
                                        oncopy="return false;"
                                        oncut="return false;" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trUPField5" runat="server" visible="false">
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblSrField5" runat="server" Text="1.2.8"></asp:Label>
                                </td>
                                <td valign="top" class="auto-style1">
                                    <asp:Label ID="lblField5" runat="server" Text="Field 5&lt;font color='RED'&gt;*&lt;/font&gt;" EnableHtmlEncode="false"></asp:Label>
                                </td>
                                <td style="margin-left: 40px" class="auto-style1">
                                    <asp:TextBox ID="txtField5" onpaste="return false;"
                                        oncopy="return false;"
                                        oncut="return false;" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <%-- Added by Amit End --%>
                            <tr class="gdrow1">
                                <td valign="top">
                                    <asp:Label ID="Lbl4" runat="server">1.3</asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:Label ID="Label90" runat="server" Text="Exam Cycle / परीक्षा चक्र &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                    <br />
                                    (Subject to Change *)
                                </td>
                                <td style="margin-left: 40px">
                                    <asp:Label ID="LblExamName" runat="server" BackColor="#e6ffe6" Width="375px" BorderStyle="Solid"
                                        BorderWidth="1px" Height="22px" TabIndex="5"></asp:Label>
                                    <asp:Label ID="lblFeeDetail" runat="server" Font-Size="Small"></asp:Label>
                                    <asp:ImageButton ID="ImgBtnPopupFee" runat="server" ImageUrl="~/images/DisablePopup.PNG"
                                        Enabled="False" TabIndex="10" />
                                    <asp:ModalPopupExtender Drag="true" ID="ModalPopupExtender2" runat="server" DropShadow="true"
                                        CancelControlID="ImgCancle1" TargetControlID="ImgBtnPopupFee" PopupControlID="InfoDiv">
                                    </asp:ModalPopupExtender>
                                    <div id="InfoDiv" runat="server" class="modalPopup">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                &nbsp;
                                                </td>
                                                <td>
                                                </td>
                                                <td>
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
                                                                <td style="width: 60%">Fee Name
                                                                </td>
                                                                <td>Fee Amt. (Rs/-)
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
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="Regfee" runat="server">
                                <td align="left" style="width: 3%" valign="top">1.4
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label8" runat="server" Text="Registration Fee Will Be Paid By? / पंजीकरण शुल्क किसके द्वारा भुगतान किया जाएगा?<font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:DropDownList ID="ddlPaymentOption" runat="server" Width="400px" TabIndex="6"
                                        Enabled="false">
                                        <asp:ListItem Value="1" Text="Candidate directly to NIELIT (Using Available Payment Methods)">
                                        </asp:ListItem>
                                        <asp:ListItem Value="2" Text="NIELIT Center/Accredited Institute">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">2.
                                <asp:Label ID="Label69" runat="server" Text="Applicant's Personal Details /आवेदक का व्यक्तिगत विवरण">
                                </asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>2.1
                                </td>
                                <td>
                                    <asp:Label ID="Label70" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="95px" runat="server" ID="ddlSalutaionName" OnSelectedIndexChanged="ddlSalutaionName_SelectedIndexChanged"
                                        AutoPostBack="True" TabIndex="7">
                                        <asp:ListItem Value="0" Selected="True" Text="--Select--">
                                        </asp:ListItem>
                                        <asp:ListItem Value="Mr." Text="Mr./श्री">
                                        </asp:ListItem>
                                        <asp:ListItem Value="Ms." Text="Ms./सुश्री">
                                        </asp:ListItem>
                                        <asp:ListItem Value="Others" Text="Others/अन्य">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtAppName" runat="server" MaxLength="60" Width="430px" TabIndex="8"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;" onkeyup="setDeclarartion();"
                                        autocomplete="off" AutoPostBack="true" OnTextChanged="txtAppName_TextChanged"></asp:TextBox>
                                    <br />
                                    ( Full name as per the highest / latest qualification certificate or legal certificate
                                )
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">2.2
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="Care Of /  देखभाल <font color='RED'>*</font>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="Rdoownertype" runat="server" RepeatDirection="Horizontal"
                                        Width="300px" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="Rdoownertype_SelectedIndexChanged">
                                        <asp:ListItem Value="P" Selected="true"> Parents / माता पिता </asp:ListItem>
                                        <asp:ListItem Value="G"> Guardian / संरक्षक    </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trguardian" runat="server" visible="false">
                                <td width="3%">2.2.1
                                </td>
                                <td>
                                    <asp:Label ID="LblGuardian" runat="server" Text="Guardian's Name / संरक्षक का नाम <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtGuardianName" runat="server" MaxLength="60" TabIndex="9" Width="529px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />

                                    <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>

                                    <%-- Added --%>
                                    <div id="divgurdian" runat="server" style="display: none; position: absolute;" class="modalPopup5">
                                        Candidate submitting Guardian Name needs to submit the affidavit in prescribed format
                                    ( <a id="link" runat="server" target="_blank" style="text-decoration: none; color: black; font-weight: bold;">Click here to download the Format </a>) to NIELIT HQ, Delhi
                                     for further processing of the form before the last date of filling the  form ,
                                     failing which, the form will not be processed and no fee will be refunded. 
                                     If successful in examination, the certificate will be issued with Guardian Name only.
                                    <div align="right">
                                        <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                    </div>
                                    </div>

                                </td>
                            </tr>

                            <%-- Added --%>
                            <tr class="gdalternate1" id="trAffidavitNo" runat="server" visible="false">
                                <td width="3%">2.2.2
                                </td>
                                <td>
                                    <asp:Label ID="lblAffidavitNo" runat="server" Text="Affdavit No. / शपथ पत्र संख्या <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAffidavitNo" runat="server" MaxLength="60" TabIndex="11" Width="430px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trAffidavitDate" runat="server" visible="false">
                                <td width="3%">2.2.3
                                </td>
                                <td>
                                    <asp:Label ID="lblAffidavitDate" runat="server" Text="Affidavit Date / शपथ पत्र दिनांक <font color='RED'>*</font>">
                                    </asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtAffidavitDate" runat="server" MaxLength="60" TabIndex="13" Width="430px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <img id="imgAffidavitDate" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />

                                    <asp:CalendarExtender ID="calAffidavitDate" TargetControlID="txtAffidavitDate" PopupPosition="BottomLeft"
                                        Format="dd-MMM-yyyy" PopupButtonID="imgAffidavitDate" runat="server">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trAffidavit" runat="server" visible="false">
                                <td width="3%">2.2.4
                                </td>
                                <td>
                                    <asp:Label ID="lblAffidavit" runat="server" Text="Affdavit / शपथ पत्र <font color='RED'>*</font>">
                                    </asp:Label>

                                </td>
                                <td>
                                    <asp:FileUpload ID="fileAffidavit" runat="server" onkeypress="return false;" TabIndex="47"
                                        Width="360px" /><br />
                                    ( PDF file with size upto 100 KB )   
                                <asp:Label ID="lblAffidavitFile" runat="server" Visible="false"></asp:Label>
                                    <asp:RegularExpressionValidator ID="regAffidavit" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                        ControlToValidate="fileAffidavit" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                        Display="Dynamic" />
                                </td>
                            </tr>


                            <tr class="gdalternate1" id="trfather" runat="server" visible="false">
                                <td width="3%">2.2.1
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Father's Name / पिता का नाम <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="96px" runat="server" ID="DropDownList1" Enabled="False"
                                        TabIndex="10">
                                        <asp:ListItem Value="1" Text="Mr./श्री">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtFatherName" runat="server" MaxLength="60" TabIndex="11" Width="430px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />

                                    <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trmother" runat="server" visible="false">
                                <td width="3%">2.2.2
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Mother's Name / माता का नाम <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="96px" runat="server" ID="DropDownList2" Enabled="False"
                                        TabIndex="12">
                                        <asp:ListItem Value="2" Text="Mrs./श्रीमती">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtMotherName" runat="server" MaxLength="60" TabIndex="13" Width="430px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />

                                    <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trgender" runat="server">
                                <td>2.3
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Gender / लिंग<font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <%--  <asp:RadioButtonList ID="rdbtnlstgender" runat="server" RepeatDirection="Horizontal"
                                            TabIndex="14" Width="300px">
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
                            <tr class="trgdalternate1calendar" id="trdob" runat="server">
                                <td>2.4
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक <font color='RED'>*</font> (dd-Mon-yyyy)">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="11" ID="txtDob" runat="server" SkinID="txtDate" Width="99px"
                                        TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                        AutoPostBack="true" OnTextChanged="txtDob_TextChanged"></asp:TextBox>
                                    <img id="imgDob" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />
                                    <br />
                                    ( As per high school certificate in 'dd-Mon-yyyy' format. i.e. '01-Jan-1990' )
                                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trmaritalstatus" runat="server">
                                <td>2.5
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Marital Status / वैवाहिक स्थिति<font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMStatus" runat="server" Width="375px" TabIndex="16">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trcategory" runat="server">
                                <td>2.6
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Category / वर्ग<font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" Width="375px" TabIndex="17">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trhandicapped" runat="server">
                                <td>2.7
                                </td>
                                <td>
                                    <asp:Label ID="Label71" runat="server" Text="Handicapped / दिव्यांग &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="Rdhandicapped" runat="server" RepeatDirection="Horizontal"
                                        TabIndex="18">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं </asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हाँ  </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trexserviceman" runat="server">
                                <td>2.8
                                </td>
                                <td>
                                    <asp:Label ID="Label72" runat="server" Text="Ex-Serviceman / पूर्व सेवाकर्मी &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="Rdexserviceman" runat="server" RepeatDirection="Horizontal"
                                        TabIndex="19">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं </asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हाँ </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="trews" runat="server">
                                <td>2.9
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="EWS / आर्थिक रूप से कमजोर वर्ग &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RdisEWS" runat="server" RepeatDirection="Horizontal"
                                        TabIndex="19">
                                        <asp:ListItem Value="N" Selected="True">No / नहीं </asp:ListItem>
                                        <asp:ListItem Value="Y">Yes / हाँ </asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="trreligion" runat="server">
                                <td>2.10
                                </td>
                                <td>
                                    <asp:Label ID="Label73" runat="server" Text="Religion / धर्म &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReligion" runat="server" Width="375px" TabIndex="20">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">3. Contact Details / संपर्क विवरण
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>3.1
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Text="Phone with STD code / दूरभाष एस टी डी कोड सहित">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtSTDcode" runat="server" onpaste="return false" Width="65px" MaxLength="5"
                                        TabIndex="21" onkeypress="checkNumber(this,4,0,event);" oncopy="return false;"
                                        oncut="return false;"></asp:TextBox>
                                    <asp:TextBox ID="txtCorPhoneNo" runat="server" onkeypress="checkNumber(this,10,0,event);"
                                        TabIndex="22" Width="280px" MaxLength="8" onpaste="return false;" oncopy="return false;"
                                        oncut="return false;"></asp:TextBox><br />
                                    (एसटीडी कोड) (दूरभाष संख्या)
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>3.2
                                </td>
                                <td>
                                    <asp:Label ID="Label40" runat="server" Text="Mobile Number / मोबाइल नंबर &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorMobileCode" runat="server" Enabled="false" Text="+91" Width="65px"
                                        TabIndex="23" onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <asp:TextBox ID="txtCorMobileNo" runat="server" MaxLength="10" oncopy="return false"
                                        onkeypress="checkNumber(this,10,0,event);" onpaste="return false"
                                        TabIndex="24" Width="280px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>3.3
                                </td>
                                <td>
                                    <asp:Label ID="Label21" runat="server" Text="Email Address / ईमेल पता&lt;font color='RED'&gt;*&lt;/font&gt; ">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="150" TabIndex="25" Width="520px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox><br />
                                    (e.g.abc@yahoo.com)
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">4. Permanent Address Details / स्थायी पता विवरण
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>4.1
                                </td>
                                <td>
                                    <asp:Label ID="Label83" runat="server" Text="Address Line1/पता पंक्ति 1 <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPerAddressLine1" runat="server" Width="520px" TabIndex="26" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>4.2
                                </td>
                                <td>
                                    <asp:Label ID="Label84" runat="server" Text="Address Line2/पता पंक्ति 2 <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPerAddressLine2" runat="server" Width="520px" TabIndex="27" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>4.3
                                </td>
                                <td>
                                    <asp:Label ID="Label85" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPerAddressLine3" runat="server" Width="520px" TabIndex="28" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="3%">4.4
                                </td>
                                <td>
                                    <asp:Label ID="LblCity" runat="server" Text="City Name / शहर का नाम <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPerCity" runat="server" Width="520px" TabIndex="29" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>4.5
                                </td>
                                <td>
                                    <asp:Label ID="Label79" runat="server" Text="State / राज्य <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPState_SelectedIndexChanged"
                                        TabIndex="30" Width="375px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>4.6
                                </td>
                                <td>
                                    <asp:Label ID="Label78" runat="server" Text="District / जिला  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPdistrict" runat="server" TabIndex="31" Width="375px">
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
                                <td>4.7
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Pin Code / पिन  कोड  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPpincode" runat="server" oncopy="return false" oncut="return false"
                                        onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="32"
                                        Width="93px" MaxLength="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">5.
                                <asp:Label ID="Label74" runat="server" Text="Correspondence Details / पत्राचार की सूचना">
                                </asp:Label>
                                    <asp:CheckBox Style="float: right;" runat="server" Text="Same as Permanent Address / स्थायी पता जैसा&nbsp;&nbsp;&nbsp;"
                                        ID="chkSame" OnCheckedChanged="chkSame_CheckedChanged" AutoPostBack="True" TabIndex="33" />
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>5.1
                                </td>
                                <td>
                                    <asp:Label ID="Label86" runat="server" Text="Address Lin1/पता पंक्ति 1 <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorAddressLine1" runat="server" Width="520px" TabIndex="34" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>5.2
                                </td>
                                <td>
                                    <asp:Label ID="Label87" runat="server" Text="Address Line2/पता पंक्ति 2 <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorAddressLine2" runat="server" Width="520px" TabIndex="35" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>5.3
                                </td>
                                <td>
                                    <asp:Label ID="Label88" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorAddressLine3" runat="server" Width="520px" TabIndex="36" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>5.4
                                </td>
                                <td>
                                    <asp:Label ID="LblCity0" runat="server" Text="City Name /शहर का नाम <b class='mandatory'>*</b>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCorCity" runat="server" Width="520px" TabIndex="37" MaxLength="30"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>5.5
                                </td>
                                <td>
                                    <asp:Label ID="Label33" runat="server" Text="State / राज्य <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCorState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
                                        TabIndex="38" Width="375px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>5.6
                                </td>
                                <td>
                                    <asp:Label ID="Label34" runat="server" Text="District / जिला  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddldistrict" runat="server" TabIndex="39" Width="375px">
                                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCorState" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>5.7
                                </td>
                                <td>
                                    <asp:Label ID="Label39" runat="server" Text="Pin Code / पिन  कोड  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorPinCode" runat="server" oncopy="return false" oncut="return false"
                                        onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="40"
                                        Width="93px" MaxLength="6">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">6. Educational / Qualification Details / शैक्षिक / योग्यता का विवरण
                                </td>
                            </tr>
                            <tr class="gdalternate1" valign="top" id="cls1" runat="server">
                                <td>6.1
                                </td>
                                <td>
                                    <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification / उच्चतम शैक्षिक योग्यता &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDLeducode" runat="server" Width="530px" SkinID="25" TabIndex="41"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDLeducode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrIfother" runat="server" valign="top" visible="False">
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label81" runat="server" Text="If others specify / यदि अन्य, तो निर्दिष्ट करें ">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEduIfOther" Width="520px" runat="server" MaxLength="50" TabIndex="42"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                    <br />

                                </td>
                            </tr>
                            <tr class="gdrow1" id="cls2" runat="server" valign="top">
                                <td>6.2
                                </td>
                                <td>
                                    <asp:Label ID="Label82" runat="server" Text="Year of Passing / उत्तीर्ण वर्ष &lt;font color='RED'&gt;*&lt;/font&gt;">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtYearOfPassing2" Width="93px" runat="server" onkeypress="checkNumber(this,4,0,event);"
                                        MaxLength="4" TabIndex="43" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrExperience" runat="server">
                                <td valign="top">6.3
                                </td>
                                <td valign="top">
                                    <asp:Label ID="Label63" runat="server" Text="  If direct, experience in years / यदि  डायरेक्ट, वर्षों का अनुभव">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TxtExperienceInYears" runat="server" Width="93px" onkeypress="checkNumber(this,1,2,event);"
                                                MaxLength="4" TabIndex="44" onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                            <asp:Label ID="lblExperienceInYears" runat="server" Visible="False" EnableTheming="False"></asp:Label>
                                            <asp:HiddenField ID="HfExperience" runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DDLeducode" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">7. Identification Details / पहचान की सूचना
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="Aadhaartr" runat="server" visible="false">
                                <td>7.1
                                </td>
                                <td>
                                    <asp:Label ID="Label49" runat="server" Text="Aadhar Card Number / आधार कार्ड संख्या">
                                    </asp:Label>
                                </td>
                                <td>
                                    <%-- // added_by_amit_audit_april_2026_start --%>
                                    <asp:TextBox ID="txtaadhar" runat="server" MaxLength="12" TabIndex="49" Width="250px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                   <asp:UpdatePanel>
								   <ContentTemplate>
                                            <asp:Button ID="btnSaveAadhar" runat="server" Text="Save Aadhaar" EnableTheming="false" OnClick="btnSaveAadhar_Click" />
                                            <asp:Button ID="btnClearAadhar" runat="server" Text="Clear Aadhaar" EnableTheming="false" OnClick="btnClearAadhar_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <asp:HiddenField ID="hfaadhaar" Value="" runat="server" />
                                    <%--  added_by_amit_audit_april_2026_end --%>
                                    <%--
                                <asp:TextBox ID="txtaadhar" runat="server" MaxLength="12" TabIndex="49" Width="520px"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" onkeypress="checkNumber(this,12,0,event);">
                                </asp:TextBox>
                                    --%>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="UIDtr" runat="server" visible="false">
                                <td>7.1
                                </td>
                                <td>
                                    <asp:Label ID="UidTypeLbl" runat="server" Text="ID Card Type / पहचान पत्र  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="UidTypeDdl" runat="server" Width="200px">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Aadhaar Card</asp:ListItem>
                                        <asp:ListItem Value="2">PAN Card</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp &nbsp &nbsp
                                <asp:TextBox ID="UidNumberTxt" runat="server" MaxLength="12" placeholder="ID Card Number"
                                    Width="300px" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="Apaartr" runat="server" visible="true">
                                <td>7.2</td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="Apaar&nbsp; ID / अपार आईडी  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                      
                                    <asp:TextBox ID="txtapaar" runat="server" MaxLength="12" TabIndex="49" Width="520px" AutoPostBack="true"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;" TextChanged="txtapaar_TextChanged" OnTextChanged="txtapaar_TextChanged">
                                    </asp:TextBox>
                                          
                                    <a href="#" onclick="window.open('https://www.abc.gov.in')">Click here to generate Apaar Id</a>

                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrProviderPresent" runat="server" visible="true">
                                <td>7.2.1</td>
                                <td>
                                    <asp:Label ID="lblProviderPresent" runat="server"
                                        Text="Is Provider Present / क्या प्रदाता उपस्थित है <font color='RED'>*</font>">
                                    </asp:Label>
                                    </>
                                      <td>
                                          <asp:TextBox ID="txtIsProviderPresent" runat="server"
                                              Text="True" Width="520px" ReadOnly="true">
                                          </asp:TextBox>
                                      </td>
                            </tr>

                            <tr class="gdalternate1" id="TrConsentRelation" runat="server" visible="true">
                                <td>7.2.2</td>
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
                            <tr class="gdrow1" id="Tr1" runat="server" visible="true">
                                <td>7.2.3</td>

                                <td>
                                    <asp:Label ID="Label17" runat="server"
                                        Text="Provider Name / प्रदाता का नाम <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>

                                <td>
                                    <asp:TextBox ID="txtproviderName" runat="server"
                                        MaxLength="100" Width="520px">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrAuthMode" runat="server" visible="true">
                                <td>7.2.4</td>
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
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrAuthId" runat="server" visible="true">
                                <td>7.2.5</td>
                                <td>
                                    <asp:Label ID="lblAuthId" runat="server"
                                        Text="Authentication ID No / प्रमाणीकरण आईडी संख्या <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthenticationIdNo" runat="server"
                                        MaxLength="50" Width="520px"
                                        onkeypress="return isNumberKey(event);"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                </td>
                            </tr>



                            <tr class="gdalternate1" id="TrConsentDate" runat="server" visible="true">
                                <td>7.2.6</td>
                                <td>
                                    <asp:Label ID="lblConsentDate" runat="server"
                                        Text="Consent Date / सहमति दिनांक <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentDate" runat="server"
                                        Width="520px"  ReadOnly="true">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrConsentTime" runat="server" visible="true">
                                <td>7.2.7</td>
                                <td>
                                    <asp:Label ID="lblConsentTime" runat="server"
                                        Text="Consent Time / सहमति समय <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentTime" runat="server"
                                        Width="520px" TextMode="Time" ReadOnly="true">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrConsentPlace" runat="server" visible="true">
                                <td>7.2.8</td>
                                <td>
                                    <asp:Label ID="lblConsentPlace" runat="server"
                                        Text="Consent Place (alphabets/one space/apostrophe)/ सहमति स्थान <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentPlace" runat="server" MaxLength="50"
                                        Width="520px">
                                    </asp:TextBox>
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

                                                <asp:Label ID="lblApaarDeclaration"
                                                    runat="server"
                                                    >
                                                </asp:Label>

                                                <br />

                                            </td>

                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                            <%-- added_by_amit_audit_april_2026_start --%>
                            <tr class="gdrow1">
                                <td align="left" style="width: 3%" valign="top">7.3
                                </td>
                                <td align="left" valign="top">
                                    <%-- image upload --%>
                                    <asp:Label ID="ImagePathPh" runat="server" Text="" Style="display: none"></asp:Label>
                                    <asp:Label ID="lblphoto" runat="server" Text="Upload Photo / फोटो अपलोड करें <font color='RED'>*</font>">
                                    </asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="ImgUpload" runat="server" onkeypress="return false;" TabIndex="45"
                                                Width="360px" onchange="UploadFile(this);" />
                                            <asp:Button ID="UploadPhoto" ClientIDMode="Static" runat="server" Text="Upload Photo" OnClick="UploadPhoto_Click" Style="display: none" />
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
                                    <%--<img src="" runat="server" id="photoPreview" alt="Photo" width="110" height="130" />--%>
                                    <asp:Image runat="server" ID="photoPreview" alt="Photo" Width="110" Height="130"></asp:Image>
                                    <asp:Label runat="server" ID="lblphotoShow" Visible="False" Font-Bold="False" Font-Size="10pt"
                                        ForeColor="Red"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                <td height="80">7.4
                                </td>
                                <td>
                                    <%-- image upload code --%>
                                    <asp:Label ID="ImagePathSig" runat="server" Text="" Style="display: none"></asp:Label>
                                    <asp:Label ID="lblSign" runat="server" Text="Upload Signature / हस्ताक्षर अपलोड करें <font color='RED'>*</font>">
                                    </asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="ImgUploadSignature" runat="server" onkeypress="return false;"
                                                TabIndex="46" Width="360px" AutoPostBack="true" onchange="imageSignpreview(this);" />
                                            <asp:Button ID="UploadSignature" ClientIDMode="Static" runat="server" Text="Upload Signature" OnClick="UploadSignature_Click" Style="display: none" />
                                            <br />

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
                                <td>
                                    <asp:Image runat="server" ID="signPreview" alt="Signature" Width="200" Height="80"></asp:Image>
                                    <asp:Label runat="server" Text="" ID="lblsignShow" Visible="false" Font-Bold="False"
                                        Font-Size="10pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td height="90">7.5
                                </td>
                                <td>
                                    <%-- image upload code --%>
                                    <asp:Label ID="ImagePathLt" runat="server" Text="" Style="display: none"></asp:Label>

                                    <asp:Label ID="lblThumb" runat="server" Text="Upload left hand thumb impression / बांए हाथ के अंगूठे का निशान अपलोड करें<font color='RED'>*</font>">
                                    </asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>

                                            <asp:FileUpload ID="ImgUploadThumb" runat="server" onkeypress="return false;" TabIndex="47"
                                                Width="360px" AutoPostBack="true" onchange="imageThumbpreview(this);" /><br />

                                            <asp:Button ID="UploadThumb" ClientIDMode="Static" runat="server" Text="Upload Thumb" OnClick="UploadThumb_Click" Style="display: none" />
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
                            <%--  added_by_amit_audit_april_2026_end --%>
                            <%--
                        <tr class="gdrow1">
                            <td height="130">
                                7.3
                            </td>
                            <td height="130">
                                <asp:Label ID="Label27" runat="server" Text="Upload Photo / फोटो अपलोड करें <font color='RED'>*</font>">
                                </asp:Label><br />
                                <asp:FileUpload ID="ImgUpload" runat="server" onkeypress="return false;" TabIndex="45"
                                    Width="360px" AutoPostBack="true" onchange="imagePhotopreview(this);" /><br />
                                ( JPG,JPEG,GIF,PNG image with size upto 50 KB )                           
                            </td>
                            <td>
                                <img src="" runat="server" id="photoPreview" alt="Photo" width="110" height="130" />
                                <asp:Label runat="server" ID="lblphotoShow" Visible="False" Font-Bold="False" Font-Size="10pt"
                                    ForeColor="Red"></asp:Label>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.JPG|.PNG|.GIF|.JPEG|.png|.jpg|.gif|.jpeg)$"
                                    ControlToValidate="ImgUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid Image file."
                                    Display="Dynamic" />
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td height="80">
                                7.4
                            </td>
                            <td>
                                <asp:Label ID="Label28" runat="server" Text="Upload Signature / हस्ताक्षर अपलोड करें <font color='RED'>*</font>">
                                </asp:Label><br />
                                <asp:FileUpload ID="ImgUploadSignature" runat="server" onkeypress="return false;"
                                    TabIndex="46" Width="360px" AutoPostBack="true" onchange="imageSignpreview(this);" /><br />
                                ( JPG,JPEG,GIF,PNG image with size upto 50 KB )                          
                            </td>
                            <td>
                                <img id="signPreview" src="" style="height: 80px; width: 200px" alt="Signature" runat="server" />
                                <asp:Label runat="server" Text="" ID="lblsignShow" Visible="false" Font-Bold="False"
                                    Font-Size="10pt" ForeColor="Red"></asp:Label>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.JPG|.PNG|.GIF|.JPEG|.png|.jpg|.gif|.jpeg)$"
                                    ControlToValidate="ImgUploadSignature" runat="server" ForeColor="Red" ErrorMessage="Please select a valid Image file."
                                    Display="Dynamic" />
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td height="90">
                                7.5
                            </td>
                            <td>
                                <asp:Label ID="Label29" runat="server" Text="Upload left hand thumb impression / बांए हाथ के अंगूठे का निशान अपलोड करें<font color='RED'>*</font>">
                                </asp:Label><br />
                                <asp:FileUpload ID="ImgUploadThumb" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="360px" AutoPostBack="true" onchange="imageThumbpreview(this);" /><br />
                                ( JPG,JPEG,GIF,PNG image with size upto 50 KB )                        
                            </td>
                            <td>
                                <img src="" id="thumbPreview" style="height: 90px; width: 150px;" alt="Left Thumb Impression"
                                    runat="server" />
                                <asp:Label runat="server" Text="" ID="lblthumbshow" Visible="false" ffont-bold="False"
                                    Font-Size="10pt" ForeColor="Red"></asp:Label>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.JPG|.PNG|.GIF|.JPEG|.png|.jpg|.gif|.jpeg)$"
                                    ControlToValidate="ImgUploadThumb" runat="server" ForeColor="Red" ErrorMessage="Please select a valid Image file."
                                    Display="Dynamic" />
                            </td>
                        </tr>
                            --%>
                            <tr class="gdalternate1">
                                <td>7.6
                                </td>
                                <td>
                                    <asp:Label ID="Label30" runat="server" Text="Visible Distinguishing Mark / स्पष्ट पहचान चिन्ह <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBodyMark" runat="server" MaxLength="50" TabIndex="48" Width="520px"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td colspan="3">
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 45%; margin-left: 364px;" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" style="vertical-align: middle">
                                                        <span style="font-size: 12px; font-weight: bold; margin-left: -75px;">Please enter the
                                                        following number you see into the textbox below.</span>
                                                        <img src="" id="imgcap" runat="server" alt="Capture Code" width="150" height="55"
                                                            style="margin-left: 18px;" />
                                                        <asp:ImageButton ID="ImgBtnRefresh" ImageUrl="~/images/refresh.jpg" runat="server"
                                                            CausesValidation="false" Width="30px" Style="vertical-align: top; padding-top: 0px; border: none;"
                                                            OnClick="ImgBtnRefresh_Click" TabIndex="50" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtcode" runat="server" Width="140px" MaxLength="6" TabIndex="51"
                                                            autocomplete="off" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                                            Style="margin-left: 17px;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">8. Declaration / घोषणा (यदि हाँ तो बॉक्स (<asp:CheckBox ID="CheckBox1" runat="server"
                                    Enabled="false" />) पर क्लिक (<asp:CheckBox ID="CheckBox2" runat="server" Enabled="false"
                                        Checked="true" />) करें)
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td align="left" valign="top" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td></td>
                                            <td valign="top">
                                                <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="52" />
                                            </td>
                                            <td id="tddeclaration1" runat="server" style="display: none; text-align: justify;">
                                                <font color='RED'>*</font><asp:Label ID="lblDeclar" runat="server" Text="This is to certify that all the information 
                                            submitted by me in this Online Registration Application Form is true and correct to the best of my knowledge and belief.
                                             If any information, furnished by me in this Online Registration Application Form, submitted by me is found to be incorrect,
                                              fake, misleading or illegal at any point of time, I will be solely responsible for any financial, social or
                                               legal action that may be taken against me by NIELIT at any point of time. / यह प्रमाणित किया जाता है कि मेरे द्वारा ऑनलाइन पंजीकरण आवेदन प्रपत्र में दी गई सूचना मेरे ज्ञान विश्वास से सत्य व सही है। ऑनलाइन पंजीकरण आवेदन प्रपत्र में यदि मेरे द्वारा प्रदत्त सूचना किसी भी समय त्रुटिपूर्ण, जाली, भ्रामक व अवैध पायी जाती है तो नाइलिट द्वारा मेरे विरुद्ध किसी भी प्रकार की  वित्तीय, सामाजिक व वैधानिक कार्यवाई की जा सकती है जिसके लिए मैं पूर्णरूप से उत्तरदाई रहूँगा।"></asp:Label>
                                                <br />
                                            </td>
                                            <td id="tddeclaration2" runat="server" style="display: none; text-align: justify;">
                                                <font color='RED'>*</font>I, Shri/Ms.
                                            <asp:Label ID="LblGuard" runat="server" Text=""></asp:Label>
                                                (name of the parent/Guardian), father/mother/guardian of Sh./Ms.
                                            <asp:Label ID="lblname" runat="server" Text=""></asp:Label>
                                                (Name of the student) have read and understood the eligibility criteria of the course
                                            <asp:Label runat="server" Text="" ID="lbldeccoursecode" Style="font-weight: bold;"></asp:Label>
                                                (name of the course) as mentioned in the syllabus of the course and I undertake
                                            that my son/daughter/ward is eligible to register himself/herself for this course.
                                            If it is discovered at any point of time that my son/daughter or ward was not eligible
                                            to register in the course name his/her registration for the course will automatically
                                            be treated as null and void ab initio and I will have no claim whatsoever.<br />
                                                This is further to certify that all the information submitted by him/her in this
                                            Online Registration Application Form is true and correct to the best of my knowledge
                                            and belief. If any information, furnished in this Online Registration Application
                                            Form, submitted by him/her is found to be incorrect, fake, misleading or illegal
                                            at any point of time, I will be solely responsible for any financial, social or
                                            legal action that may be taken by NIELIT at any point of time./ मैं, श्री/सुश्री
                                            <asp:Label ID="LblHGuard" runat="server" Text=""></asp:Label>(माता-पिता/अभिभावक
                                            का नाम), श्री/सुश्री<asp:Label ID="LblHName" runat="server" Text=""></asp:Label>(छात्र/छात्रा
                                            का नाम) का पिता/माता/अभिभावक ने पाठ्यक्रम की विषय-वस्तु में उल्लिखित पाठ्यक्रम
                                            <asp:Label runat="server" Text="" ID="LblHCourseCode" Style="font-weight: bold;"></asp:Label>(पाठ्यक्रम
                                            का नाम) की पात्रता के मानदंड को भलीभाँति पढ़ एवं समझ लिया है एवं मैं वचन देता/देती
                                            हूँ कि मेरा/मेरी पुत्र/पुत्री/प्रतिपाल्य इस पाठ्यक्रम में पंजीकरण हेतु पात्र है।
                                            यदि किसी भी समय ऐसा पाया जाता है कि मेरा/मेरी पुत्र/पुत्री/प्रतिपाल्य (पाठ्यक्रम
                                            का नाम) में पंजीकरण हेतु पात्र नहीं था/थी तो उक्त पाठ्यक्रम हेतु उसका पंजकरण प्रारम्भ
                                            से ही अमान्य समझा जाएगा एवं किसी भी स्थिति में कोई भी दावा नहीं करूंगा।<br />
                                                यह प्रमाणित किया जाता है कि उनके द्वारा ऑनलाइन पंजीकरण आवेदन प्रपत्र में दी गई सूचना
                                            मेरे ज्ञान विश्वास से सत्य व सही है। ऑनलाइन पंजीकरण आवेदन प्रपत्र में यदि उनके द्वारा
                                            प्रदत्त सूचना किसी भी समय त्रुटिपूर्ण, जाली, भ्रामक व अवैध पायी जाती है तो नाइलिट
                                            द्वारा किसी भी प्रकार की वित्तीय, सामाजिक व वैधानिक कार्यवाई की जा सकती है जिसके
                                            लिए मैं पूर्णरूप से उत्तरदाई रहूँगा।
                                            <br />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trnote" runat="server" class="gdrow1" visible="false">
                                <td colspan="3">
                           <%--         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>--%>
                                            <strong style="font-size: 14px; padding-left: 2px; text-align: justify; color: Red;">Note: Registration does not guarantee admission. It will depend on the availability
                                            of the seats, minimum batch size and availability of slot/resources with the concerned
                                            Centre. Registration
                                            <asp:Label ID="Labelfee" runat="server" Text=""></asp:Label>
                                                will be payable at opted
                                            <asp:Label ID="LabelCenter" runat="server" Text="NIELIT Centre"></asp:Label>. Please
                                            visit
                                            <asp:Label ID="LabelCenters" runat="server" Text="NIELIT Centre"></asp:Label>
                                                for further details</strong>
                                        <%--</ContentTemplate>--%>
                  <%--                      <Triggers>

                                            <asp:AsyncPostBackTrigger ControlID="DdlAccCentre" EventName="SelectedIndexChanged" />
                                        </Triggers>--%>
                                    <%--</asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Proceed" TabIndex="53" OnClick="btnSave_Click"
                                        OnClientClick="return ValidateForm();" />
                                    <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" OnClick="btnback_Click" />
                                </td>
                            </tr>
                        </table>
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
                <asp:Label ID="Lblmsg" runat="server" Text=" Please ensure that you are already enrolled with the selected Accreditation Institute for the level applied for"></asp:Label>
            </p>
            <asp:Button ID="CancleBtn" runat="server" Style="text-align: right;" Text="Close" />
        </asp:Panel>
    </form>
</body>
</html>
