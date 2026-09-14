<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPages/main.master" CodeFile="NielitCentreStudentMapping.aspx.cs" Inherits="Admin_NielitCentreStudentMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Nielit Centre Student"></asp:Label>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            ;

            $("[id*=txtDob]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendaricon.jpg',
                dateFormat: 'dd-M-yy'
            });
            $("[id*=txtcertificateIssueDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendaricon.jpg',
                dateFormat: 'dd-M-yy'
            });
            $("[id*=txtPlacementDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendaricon.jpg',
                dateFormat: 'dd-M-yy'
            });
            //if ($('#ddlwhetherCertificateIssued').val() == "1") {


            //}
        });
    </script>
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

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
        }
    </script>
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

            if (!isvalidateRadioButtonList("RdoAffInstOrNonAffInst", "Choose  Institute"))
                return false;

            var list = document.getElementById("RdoAffInstOrNonAffInst");
            var listItemArray = list.getElementsByTagName("input");
            var Itemvalue = "";
            for (var i = 0; i < listItemArray.length; i++) {
                var listItem = listItemArray[i];
                if (listItem.checked) {
                    Itemvalue = listItem.value;
                }
            }


            if (!isSelected("DDL_PreviousLevel", "Course"))
                return false;
            if (!isBlankNumber("Txt_Regno", "Registration Number"))
                return false;
            if (!isNumber("Txt_Regno"))
                return false;
            if (!isSelected("DDLRegForCourse", "Course"))
                return false;

            if (!isBlank("txtcode", "Captcha Code"))
                return false;


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
    </style>

    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="New" runat="server">
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>

            <div>
                <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" style="margin-bottom: 20px">
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <strong style="text-align: center" class="headfont">STUDENT MAPPING FORM
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
                                    <asp:Label ID="lblerror" runat="server" EnableTheming="False" TabIndex="-1" CssClass="error" Visible="False"
                                        Width="99%"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table style="width: 100%;" class="sample3" border="0" cellpadding="3" cellspacing="1">
                                <tr class="head1">
                                    <td align="left" colspan="4">1. Registration Details / पंजीयन का विवरण
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" colspan="4">
                                        <asp:TextBox Style="width: 501px;" ID="txtInstitute" runat="server" Enabled="false" Width="150px" ToolTip="Institute"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trregno" runat="server">
                                    <td width="3%">1.0
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrefno" runat="server" Text="Online Reference Number<font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtrefno" runat="server" MaxLength="50" Width="281px"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                    </td>

                                    <td align="left">
                                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trremarks" runat="server" visible="false">
                                    <td width="3%">1.0.1
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRemarks" runat="server" Text="Reason for Editing <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" Width="529px"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />

                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="tralready" runat="server" visible="false">
                                    <td width="3%" valign="top">1.0.2
                                    </td>
                                    <td width="40%" valign="top">Whether already registered                                
                               
                                <font color='RED'>*</font>
                                    </td>
                                    <td style="width: 57%;" valign="top" colspan="2">

                                        <asp:RadioButtonList ID="RadioButtonListCentre" runat="server" RepeatDirection="Horizontal"
                                            Width="172px" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListCentre_SelectedIndexChanged">
                                            <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        </asp:RadioButtonList>

                                    </td>
                                    <td></td>
                                </tr>

                                <tr class="gdalternate1" id="Trappno" runat="server" visible="false">
                                    <td width="3%" valign="top">1.0.2.1
                                    </td>
                                    <td width="40%" valign="top">Course Type                              
                               
                                <font color='RED'>*</font>
                                    </td>
                                    <td style="width: 57%;" valign="top" colspan="2">

                                        <asp:RadioButtonList ID="rdbCourseType" runat="server" RepeatDirection="Horizontal"
                                            Width="476px" AutoPostBack="True" OnSelectedIndexChanged="rdbCourseType_SelectedIndexChanged">
                                            <asp:ListItem Value="R">Registration No.(Other Course)</asp:ListItem>
                                            <asp:ListItem Value="A">Application No.(DLC)</asp:ListItem>
                                        </asp:RadioButtonList>

                                    </td>
                                    <td></td>
                                </tr>
                                <tr class="gdrow1" id="tr4" runat="server" visible="false">
                                    <td width="3%">1.0.2.2
                                    </td>
                                    <td>
                                        <asp:Label ID="lblregistrationNo" runat="server" Text="Registration No. <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtRegNo" runat="server" MaxLength="200" Width="529px"></asp:TextBox>
                                        <br />

                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="trcenter" runat="server" visible="false">

                                    <td width="3%" valign="top" style="padding-right: 13px;"></td>

                                    <%--<td width="97%" valign="top" >
                                Nielit Centre:

                                <asp:Label ID="lblNielitCentre" runat="server" Text="">
                                </asp:Label>--%>

                                    <%--</td>--%>

                                    <td width="40%" valign="top">
                                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 57%;" valign="top" colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                                                    TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                                                    Style="height: 27px" Font-Bold="True">
                                                    <asp:ListItem Value="1">Accredited Centre</asp:ListItem>
                                                    <asp:ListItem Value="0">Non Accredited Centre</asp:ListItem>
                                                    <asp:ListItem Value="2">Nielit Centre</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>

                                </tr>



                                <tr class="gdalternate1" id="trcourse" runat="server" visible="false">

                                    <td width="3%" valign="top" style="padding-right: 13px;">1.1
                                    </td>

                                    <td valign="top" width="30%">Center:
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCenter" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>

                                        <%--<asp:DropDownList ID="ddlCenter" runat="server" Width="281px" TabIndex="1">
                                </asp:DropDownList>--%>

                                    </td>

                                    <td valign="top" width="20%">Course:
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCourse" runat="server" Width="281px" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" TabIndex="1">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                    </td>

                                    <td valign="top" width="37%">Batch:
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlBatch" runat="server" Width="337px" TabIndex="1" Enabled="false">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                    </td>

                                </tr>

                                <tr class="gdalternate1" id="trproject" runat="server" visible="false">

                                    <td width="3%" valign="top" style="padding-right: 13px;">1.2
                                    </td>

                                    <td valign="top" width="40%">Poject Student:
                                <asp:DropDownList ID="ddlWheatherPojectStu" runat="server" SkinID="ddl250"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlWheatherPojectStu_SelectedIndexChanged" Enabled="false">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>

                                    </td>

                                    <td valign="top" width="57%" colspan="2">
                                        <asp:Label ID="LblProjectname" runat="server" SkinID="CaptionLabel" Text="Project Name :"></asp:Label>
                                        <asp:DropDownList ID="ddlProcname" runat="server" Enabled="False" SkinID="ddl250" OnSelectedIndexChanged="ddlProcname_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>



                                    </td>


                                </tr>

                                <%--<tr class="gdalternate1">
                            <td width="3%" valign="top" style="padding-right: 13px;">
                                1.1
                            </td>
                            <td valign="top" width="40%">
                                <asp:Label ID="Label1" runat="server" Text="Registration sought for / के लिए पंजीयन &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td width="57%">
                                <asp:DropDownList ID="DDLRegForCourse" runat="server" Width="375px" TabIndex="1">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                            </table>
                            <table id="TblFormDetail" runat="server" class="sample3" style="width: 100%; text-align: left"
                                border="0" cellpadding="3" cellspacing="1" visible="false">
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
                                            AutoPostBack="True" Enabled="false" TabIndex="7">
                                            <asp:ListItem Value="0" Selected="True" Text="--Select--">
                                            </asp:ListItem>
                                            <asp:ListItem Value="Mr." Text="Mr./श्री">
                                            </asp:ListItem>
                                            <asp:ListItem Value="Ms." Text="Ms./सुश्री">
                                            </asp:ListItem>
                                            <asp:ListItem Value="X" Text="Others/अन्य">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtAppName" Enabled="false" runat="server" MaxLength="60" Width="430px" TabIndex="8"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;" onblur="return onlyAlphabets(event,this);" onkeyup="setDeclarartion();"
                                            autocomplete="off"></asp:TextBox>
                                        <br />
                                        ( Full name as per the highest / latest qualification certificate or legal certificate
                                )
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td width="3%">2.2
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Care Of /  देखभाल <font color='RED'>*</font>"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="Rdoownertype" runat="server" RepeatDirection="Horizontal"
                                            Width="300px" Enabled="false" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="Rdoownertype_SelectedIndexChanged">
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
                                        <asp:TextBox ID="TxtGuardianName" Enabled="False" runat="server" MaxLength="60" TabIndex="9" Width="529px"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />

                                        <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trfather" runat="server" visible="false">
                                    <td width="3%">2.2.1
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Father's Name / पिता का नाम <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Width="96px" runat="server" ID="DropDownList1" Enabled="False"
                                            TabIndex="10">
                                            <asp:ListItem Value="1" Text="Mr./श्री">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtFatherName" Enabled="False" runat="server" MaxLength="60" TabIndex="11" Width="430px"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />

                                        <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trmother" runat="server" visible="false">
                                    <td width="3%">2.2.2
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Mother's Name / माता का नाम <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Width="96px" runat="server" ID="DropDownList2" Enabled="False"
                                            TabIndex="12">
                                            <asp:ListItem Value="2" Text="Mrs./श्रीमती">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtMotherName" Enabled="False" runat="server" MaxLength="60" TabIndex="13" Width="430px"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />

                                        <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trgender" runat="server">
                                    <td>2.3
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Gender / लिंग<font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" Enabled="False">
                                            <ContentTemplate>
                                                <%-- <asp:RadioButtonList ID="rdbtnlstgender" runat="server" RepeatDirection="Horizontal"
                                            TabIndex="14" Width="300px">
                                            <asp:ListItem Value="Male">Male / पुरुष
                                            </asp:ListItem>
                                            <asp:ListItem Value="Female">Female / महिला
                                            </asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                                <asp:DropDownList ID="ddl_gender" runat="server" Enabled="false" Width="375px" />
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
                                        <asp:TextBox MaxLength="11" Enabled="False" ID="txtDob" runat="server" SkinID="txtDate" Width="99px"
                                            TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                            AutoPostBack="true" OnTextChanged="txtDob_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblDOB" runat="server" Visible="False"></asp:Label>
                                        <%--<asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>

                                <img id="imgDob" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                <br />--%>
                                        <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtstartDate">
                                </asp:CalendarExtender>--%>

                                        <%--<img id="imgDob" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px;
                                    height: 22px; vertical-align: top;" />--%>
                                        <br />
                                        <span>( As per high school certificate in 'dd-Mon-yyyy' format. i.e. '01-Jan-1990' )</span>
                                        <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtstartDate">
                                </asp:CalendarExtender>--%>

                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trmaritalstatus" runat="server">
                                    <td>2.5
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Marital Status / वैवाहिक स्थिति<font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Enabled="False" ID="ddlMStatus" runat="server" Width="375px" TabIndex="16">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trcategory" runat="server">
                                    <td>2.6
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="Category / वर्ग<font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCategory" Enabled="False" runat="server" Width="375px" TabIndex="17">
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
                                        <asp:RadioButtonList ID="Rdhandicapped" Enabled="False" runat="server" RepeatDirection="Horizontal"
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
                                        <asp:RadioButtonList ID="Rdexserviceman" Enabled="False" runat="server" RepeatDirection="Horizontal"
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
                                        <asp:Label ID="Label14" runat="server" Text="EWS / आर्थिक रूप से कमजोर वर्ग &lt;font color='RED'&gt;*&lt;/font&gt;">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RdisEWS" runat="server" RepeatDirection="Horizontal"
                                            TabIndex="19" Enabled="false">
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
                                        <asp:DropDownList ID="ddlReligion" Enabled="False" runat="server" Width="375px" TabIndex="20">
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
                                        <asp:TextBox ID="TxtSTDcode" Enabled="False" runat="server" onpaste="return false" Width="65px" MaxLength="5"
                                            TabIndex="21" onkeypress="checkNumber(this,4,0,event);" oncopy="return false;"
                                            oncut="return false;"></asp:TextBox>
                                        <asp:TextBox ID="txtCorPhoneNo" Enabled="False" runat="server" onkeypress="checkNumber(this,10,0,event);"
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
                                        <asp:TextBox ID="txtCorMobileCode" Enabled="False" runat="server" Text="+91" Width="65px"
                                            TabIndex="23" onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <asp:TextBox ID="txtCorMobileNo" Enabled="False" runat="server" MaxLength="10" oncopy="return false"
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
                                        <asp:TextBox Enabled="False" ID="txtEmailId" runat="server" MaxLength="150" TabIndex="25" Width="520px"
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
                                        <asp:TextBox Enabled="False" ID="TxtPerAddressLine1" runat="server" Width="520px" TabIndex="26" MaxLength="30"
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
                                        <asp:TextBox Enabled="False" ID="TxtPerAddressLine2" runat="server" Width="520px" TabIndex="27" MaxLength="30"
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
                                        <asp:TextBox Enabled="False" ID="TxtPerAddressLine3" runat="server" Width="520px" TabIndex="28" MaxLength="30"
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
                                        <asp:TextBox Enabled="False" ID="TxtPerCity" runat="server" Width="520px" TabIndex="29" MaxLength="30"
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
                                        <asp:DropDownList Enabled="False" ID="ddlPState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPState_SelectedIndexChanged"
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
                                                <asp:DropDownList Enabled="False" ID="ddlPdistrict" runat="server" TabIndex="31" Width="375px">
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
                                        <asp:TextBox Enabled="False" ID="TxtPpincode" runat="server" oncopy="return false" oncut="return false"
                                            onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="32"
                                            Width="93px" MaxLength="6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="head1">
                                    <td colspan="3">5.
                                <asp:Label ID="Label74" runat="server" Text="Correspondence Details / पत्राचार की सूचना">
                                </asp:Label>
                                        <asp:CheckBox Enabled="False" Style="float: right;" runat="server" Text="Same as Permanent Address / स्थायी पता जैसा&nbsp;&nbsp;&nbsp;"
                                            ID="chkSame" OnCheckedChanged="chkSame_CheckedChanged" AutoPostBack="True" TabIndex="33" />

                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td>5.1
                                    </td>
                                    <td>
                                        <asp:Label ID="Label86" runat="server" Text="Address Line1/पता पंक्ति 1 <b class='mandatory'>*</b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox Enabled="False" ID="TxtCorAddressLine1" runat="server" Width="520px" TabIndex="34" MaxLength="30"
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
                                        <asp:TextBox Enabled="False" ID="TxtCorAddressLine2" runat="server" Width="520px" TabIndex="35" MaxLength="30"
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
                                        <asp:TextBox Enabled="False" ID="TxtCorAddressLine3" runat="server" Width="520px" TabIndex="36" MaxLength="30"
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
                                        <asp:TextBox Enabled="False" ID="TxtCorCity" runat="server" Width="520px" TabIndex="37" MaxLength="30"
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
                                        <asp:DropDownList Enabled="False" ID="ddlCorState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
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
                                        <%--<asp:DropDownList ID="ddldistrict" runat="server" AutoPostBack="True" TabIndex="39" Width="375px">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>--%>

                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList Enabled="False" ID="ddlcordistrict" runat="server" TabIndex="31" Width="375px">
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
                                        <asp:TextBox Enabled="False" ID="txtCorPinCode" runat="server" oncopy="return false" oncut="return false"
                                            onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="40"
                                            Width="93px" MaxLength="6">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="head1" id="HeadAadhaartr" runat="server">
                                    <td colspan="3">6. Identification Details / पहचान की सूचना
                                    </td>
                                </tr>
                                <%--<tr class="gdalternate1" id="Aadhaartr" runat="server" >
                            <td>
                                7.1
                            </td>
                            <td>
                                <asp:Label ID="Label49" runat="server" Text="Aadhar Card Number / आधार कार्ड संख्या">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtaadhar" runat="server" MaxLength="15" TabIndex="49" Width="520px"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" onkeypress="checkNumber(this,15,0,event);">
                                </asp:TextBox>
                            </td>
                        </tr>--%>
                                <tr class="gdalternate1" id="UIDtr" runat="server">
                                    <td>6.1
                                    </td>
                                    <td>
                                        <asp:Label ID="UidTypeLbl" runat="server" Text="ID Card Type / पहचान पत्र  <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Enabled="False" ID="UidTypeDdl" runat="server" Width="200px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Aadhaar Card</asp:ListItem>
                                            <asp:ListItem Value="2">PAN Card</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp &nbsp &nbsp
                                <asp:TextBox Enabled="False" ID="UidNumberTxt" runat="server" MaxLength="12" placeholder="ID Card Number"
                                    Width="300px" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr class="gdrow1">
                            <td height="130">
                                7.2
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
                        </tr>--%>
                                <%--<tr class="gdalternate1">
                            <td height="80">
                                7.3
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
                        </tr>--%>
                                <%--<tr class="gdrow1">
                            <td height="90">
                                7.4
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
                        </tr>--%>
                                <tr class="head1" id="trCompanyDetails" runat="server" visible="false">
                                    <td colspan="3">7. Company Details / संस्थान के विवरण
                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="trwhetherCourseComplete" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl81" runat="server" Text="7.1">
                                        </asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblwhetherCourseComplete" runat="server" Text="Whether Course Complete <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Enabled="False" ID="ddlCourseComplete" runat="server" Width="375px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="trwhetherCertificateIssued" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl82" runat="server" Text="7.2">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblwhetherCertificateIssued" runat="server" Text="Whether Certificate Issued <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Enabled="False" ID="ddlwhetherCertificateIssued" runat="server" Width="375px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="trcertificateIssueDate" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl83" runat="server" Text="7.3">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcertificateIssueDate" runat="server" Text="Certificate Issue Date <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>


                                        <asp:TextBox Enabled="False" MaxLength="11" ID="txtcertificateIssueDate" Visible="false" runat="server" SkinID="txtDate" Width="99px"
                                            TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                            AutoPostBack="true"></asp:TextBox>
                                        <%--<asp:Calendar ID="certificateIssueDate_Calender" Enabled="false" OnSelectionChanged="certificateIssueDate_Calender_SelectionChanged" TargetControlID="txtcertificateIssueDate" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="img1" runat="server">
                                        </asp:Calendar>--%>
                                    
                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="trWhetherPlaced" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl84" runat="server" Text="7.4">
                                        </asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblWhetherPlaced" runat="server" Text="Whether Placed <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Enabled="False" ID="ddlWhetherPlaced" AutoPostBack="true" OnSelectedIndexChanged="ddlWhetherPlaced_SelectedIndexChanged" runat="server" Width="375px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr class="gdalternate1" id="trWhetherPlacedDate" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblPlacedDate1" runat="server" Text="7.5">
                                        </asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblPlacedDate" runat="server" Text="Placement Date <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:TextBox Enabled="False" MaxLength="11" ID="txtPlacementDate" runat="server" SkinID="txtDate" Width="99px"
                                            TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                            AutoPostBack="true"></asp:TextBox>

                                        <%--<asp:Calendar ID="CalendarPlacementDate" OnSelectionChanged="CalendarPlacementDate_SelectionChanged" TargetControlID="txtPlacementDate" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="img1" runat="server"></asp:Calendar>--%>
                                        <%-- </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>--%>

                                        <asp:Button ID="btn" runat="server" SkinID="txtDate" Text="Add Placements Details" Visible="false" OnClick="btn_Click" />
                                    </td>
                                </tr>

                                <tr class="head1" id="trCompanyHead" runat="server" visible="false">
                                    <td colspan="3">
                                        <asp:Label ID="lbl9" runat="server" Text="8. Company Address Details / स्थायी पता विवरण">
                                        </asp:Label>
                                    </td>
                                </tr>

                                <tr class="gdrow1" id="trCompanyName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl91" runat="server" Text=" ">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCompanyName" runat="server" Text="Company Name <b class='mandatory'></b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:TextBox ID="txtCompanyName" runat="server" Width="520px" TabIndex="26" MaxLength="30"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>--%>

                                        <asp:DropDownList Enabled="False" ID="ddlCompanyName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged"
                                            TabIndex="30" Width="375px">
                                        </asp:DropDownList>


                                        <br />
                                    </td>
                                </tr>

                                <tr class="gdrow1" id="trPersonDetails" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="8.1">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Person Name <b class='mandatory'></b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox Enabled="False" ID="txtPersonName" runat="server" Width="520px" TabIndex="26" MaxLength="30"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>

                                <tr class="gdrow1" id="tr2" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="8.1.1">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMob" runat="server" Text="Mobile Number <b class='mandatory'></b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox Enabled="False" ID="txtMob" runat="server" Width="520px" TabIndex="26" MaxLength="30"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>



                                <tr class="gdrow1" id="trCmpAdd1" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl92" runat="server" Text="8.2">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmpAdd1" runat="server" Text="Address Line1/पता पंक्ति 1 <b class='mandatory'></b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCmpAdd1" runat="server" Width="520px" TabIndex="26" MaxLength="30"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;" Enabled="False"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trCmpAdd2" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl93" runat="server" Text="8.3">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmpAdd2" runat="server" Text="Address Line2/पता पंक्ति 2 <b class='mandatory'></b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCmpAdd2" runat="server" Width="520px" TabIndex="27" MaxLength="30"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;" Enabled="False"></asp:TextBox>
                                        <br />

                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trCmpAdd3" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl94" runat="server" Text="8.4">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmpAdd3" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCmpAdd3" runat="server" Width="520px" TabIndex="28" MaxLength="30"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;" Enabled="False"></asp:TextBox>
                                        <br />

                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trCmyCityName" runat="server" visible="false">
                                    <td width="3%">
                                        <asp:Label ID="lbl95" runat="server" Text="8.5">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmyCityName" runat="server" Text="City Name / शहर का नाम <b class='mandatory'></b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCmyCityName" runat="server" Width="520px" TabIndex="29" MaxLength="30"
                                            onpaste="return false;" oncopy="return false;" oncut="return false;" Enabled="False"></asp:TextBox>
                                        <br />

                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trCmpState" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl96" runat="server" Text="8.6">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmpState" runat="server" Text="State / राज्य <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCmpState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCmpState_SelectedIndexChanged"
                                            TabIndex="30" Width="375px" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trCmpDistrict" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl97" runat="server" Text="8.7">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmpDistrict" runat="server" Text="District / जिला  <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList Enabled="False" ID="ddlCmpDistrict" runat="server" TabIndex="31" Width="375px">
                                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCmpState" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trCmpPinCode" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl98" runat="server" Text="8.8">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCmpPinCode" runat="server" Text="Pin Code / पिन  कोड  <font color='RED'></font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCmpPinCode" runat="server" oncopy="return false" oncut="return false"
                                            onkeypress="checkNumber(this,6,0,event);" onpaste="return false" TabIndex="32"
                                            Width="93px" MaxLength="6" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr class="head1">
                                    <td colspan="3">
                                        <asp:Label ID="lblAffidiavit" Visible="false" runat="server" Text="9. Affidavit / शपथ पत्र">
                                        </asp:Label>
                                    </td>
                                </tr>

                                <tr class="gdrow1" id="trAffidiate" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbl101" Visible="false" runat="server" Text="9.1">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbllblAffidiavitNo" runat="server" Visible="false" Text="Affidiavit No <b class='mandatory'>*</b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox Enabled="False" ID="txtAffidiavitNo" runat="server" Width="520px" TabIndex="26" MaxLength="30"
                                            onpaste="return false;" Visible="false" oncopy="return false;" oncut="return false;"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>

                                <tr class="gdrow1">
                                    <td>
                                        <asp:Label ID="lbl102" Visible="false" runat="server" Text="9.2">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbllblAffidiavitDate" Visible="false" runat="server" Text="Affidiavit Date <b class='mandatory'>*</b>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox Enabled="False" MaxLength="11" ID="txtAffidiavitDate" Visible="false" runat="server" SkinID="txtAffidiavitDate" Width="99px"
                                            TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                            AutoPostBack="true"></asp:TextBox>

                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtAffidiavitDate" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="img2" runat="server">
                                        </asp:CalendarExtender>

                                        <img id="img2" runat="server" visible="false" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />
                                        <br />
                                    </td>
                                </tr>

                                <tr class="gdalternate1">
                                    <td>
                                        <asp:Label ID="lbl103" Visible="false" runat="server" Text="9.3">
                                        </asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblAffidiavitVerified" Visible="false" runat="server" Text="Affidiavit Verified <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList Enabled="False" ID="ddlAffidiavitVerified" Visible="false" runat="server" Width="375px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>


                                <tr id="Tr1" class="gdrow1" runat="server" visible="true">
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
                                <%--<tr class="head1">
                            <td colspan="3">
                                8. Declaration / घोषणा (यदि हाँ तो बॉक्स (<asp:CheckBox ID="CheckBox1" runat="server"
                                    Enabled="false" />) पर क्लिक (<asp:CheckBox ID="CheckBox2" runat="server" Enabled="false"
                                        Checked="true" />) करें)
                            </td>
                        </tr>--%>
                                <tr class="gdalternate1">
                                    <td align="left" valign="top" colspan="3">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td></td>
                                                <td valign="top">
                                                    <asp:CheckBox ID="chkdisclamier" runat="server" Visible="false" TabIndex="52" />
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
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <strong style="font-size: 14px; padding-left: 2px; text-align: justify; color: Red;">Note: Registration does not guarantee admission. It will depend on the availability
                                            of the seats, minimum batch size and availability of slot/resources with the concerned
                                            Centre. Registration
                                            <asp:Label ID="Labelfee" runat="server" Text=""></asp:Label>
                                                    will be payable at opted
                                            <asp:Label ID="LabelCenter" runat="server" Text="NIELIT Centre"></asp:Label>. Please
                                            visit
                                            <asp:Label ID="LabelCenters" runat="server" Text="NIELIT Centre"></asp:Label>
                                                    for further details</strong>
                                            </ContentTemplate>
                                            <Triggers>
                                                <%--<asp:AsyncPostBackTrigger ControlID="DdlAccCentre" EventName="SelectedIndexChanged" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" Text="Update" TabIndex="53" OnClick="btnSave_Click"
                                            OnClientClick="return ValidateForm();" />
                                        <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" OnClick="btnback_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>


            </div>

            <%--<div style="text-align: right; margin-top: 10px">
                <asp:Button ID="Button1" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>--%>

            <asp:HiddenField ID="HiddenField2" runat="server" />
            <%--<asp:ModalPopupExtender ID="AlertModalPopUp" runat="server" PopupControlID="PopUpPanel"
        PopupDragHandleControlID="PopupHeader" TargetControlID="HiddenField2" OkControlID="CancleBtn"
        X="300" Y="300">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PopUpPanel" runat="server" BorderColor="#990000" BackColor="AliceBlue"
        BorderStyle="Solid" Height="160px" Style="width: 500px" CssClass="modalPopup"
        align="center">
        <h2>
            Alert!</h2>
        <p>
            <asp:Label ID="Lblmsg" runat="server" Text=" Please ensure that you are already enrolled with the selected Accreditation Institute for the level applied for"></asp:Label>
        </p>
        <asp:Button ID="CancleBtn" runat="server" Style="text-align: right;" Text="Close" />
    </asp:Panel>--%>
        </asp:View>
    </asp:MultiView>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td>
                <%--<asp:HyperLink ID="hlExamMenu" runat="server" Target="_self">Exam Venues</asp:HyperLink>--%>
            </td>
        </tr>
    </table>
</asp:Content>

