<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CurrentRegistrationStatus.aspx.cs" Inherits="CAND_CurrentRegistrationStatus"
    Debug="false" %>

<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="My Current Course Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <style type="text/css">
        .modalPopup {
            border: 3px solid #31597C;
            background-color: #E6F0F0;
            padding-top: 0px;
            padding-right: 0px;
            width: 330px;
            height: 165px;
            top: 50px;
            left: 320px;
            position: relative;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function ValidateForm() {
            if (!isSelected("<%=DDLRegForCourse.ClientID %>", "Course"))
                return false;
            if (document.getElementById("<%=TxtExperienceInYears.ClientID %>")) {

                if (document.getElementById("<%=HfExperience.ClientID %>").value != "0") {
                    if (!isBlank("<%=TxtExperienceInYears.ClientID %>", "Experience in Year"))
                        return false;
                    if (!isNumber("<%=TxtExperienceInYears.ClientID %>"))
                        return false;
                    if (Number(document.getElementById("TxtExperienceInYears").value) >= 10) {
                        callErrorMsg("TxtExperienceInYears", "Experience should be less than 10 years.");
                        return false;
                    }
                    if (Number(document.getElementById("<%=TxtExperienceInYears.ClientID %>").value) < Number(document.getElementById("<%=HfExperience.ClientID %>").value)) {
                        callErrorMsg("<%=TxtExperienceInYears.ClientID %>", "Minimum " + document.getElementById("<%=HfExperience.ClientID %>").value + " years of experience required");
                        return false;
                    }
                }
                else {
                    if (!isBlank("<%=TxtExperienceInYears.ClientID %>", "Experience in Year"))
                        return false;
                    if (!isNumber("<%=TxtExperienceInYears.ClientID %>"))
                        return false;
                    if (Number(document.getElementById("TxtExperienceInYears").value) >= 10) {
                        callErrorMsg("TxtExperienceInYears", "Experience should be less than 10 years.");
                        return false;
                    }
                }
            }
            if (!isSelected("<%=DDLeducode.ClientID %>", "Highest Education"))
                return false;
            if (!isBlank("<%=TxtYearOfPassing2.ClientID %> ", "Year of Passing"))
                return false;
            if (!isNumber("<%=TxtYearOfPassing2.ClientID %> "))
                return false;
            if (!isSelected("<%=ddlReligion.ClientID %> ", "Religion"))
                return false;
            if (!isBlank("<%=txtBodyMark.ClientID %> ", "Body Mark"))
                return false;
            // APAAR Validations start
            if (!isBlank("txtapaar", "Apaar cannot be left blank"))
                return false;

            if (!isNumber("txtapaar", "Apaar can only contain Number"))
                return false;

            if (!isSelected("ddlConsentRelation", "Consent Relation"))
                return false;

            if (!isBlank("txtproviderName", "Captcha Code"))
                return false;

            if (!isSelected("ddlAuthMode", "Consent Relation"))
                return false;

            if (!isBlank("txtAuthenticationIdNo", "Authentication ID No"))
                return false;

            if (!isBlank("txtConsentDate", "Consent Date"))
                return false;

            if (!isBlank("txtConsentTime", "Consent Time"))
                return false;

            if (!isBlank("txtConsentPlace", "Consent Place"))
                return false;
            // APAAR Validations end
            var checkbox = document.getElementById("<%= chk1.ClientID %>")
            if (document.getElementById("<%= chk1.ClientID %>").checked == false) {
                alert("Please Select Declaration");
                return false;
            }
            return true;

        }
    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div class="summary_block">
        <span id="lblCourseNameSt" runat="server">Course Status: </span>
        <table align="center" width="100%" cellpadding="2" cellspacing="1">
            <tr>
                <td width="40%">Registration No.
                </td>
                <td width="60%" id="lblRegNumber" runat="server"></td>
            </tr>
            <tr>
                <td>Registration Status.
                </td>
                <td id="lblRegStatus" runat="server"></td>
            </tr>
            <tr>
                <td>Registration Date.
                </td>
                <td id="lblRegDate" runat="server"></td>
            </tr>
            <tr>
                <td>Registration Commencement Date.
                </td>
                <td id="lblRegCommencementDate" runat="server"></td>
            </tr>
            <tr>
                <td>Valid Upto Date.
                </td>
                <td id="lblRegValidUptoDate" runat="server"></td>
            </tr>
            <tr>
                <td>Candidate Type.
                </td>
                <td id="lblCandidateType" runat="server"></td>
            </tr>
        </table>
    </div>
    <div id="div_O" class="summary_block" runat="server">
        <span id="summeryHeading" runat="server">Modules Summary</span>
        <table align="center" width="100%" cellpadding="2" cellspacing="1">
            <tr>
                <td width="40%">
                    <b>Module Status</b>
                </td>
                <td align="center" width="30%" id="td1">
                    <b>Theory(Comp. + Elect. + Bridge)</b>
                </td>
                <td align="center" width="15%" id="td2">
                    <b>Practical</b>
                </td>
                <td align="center" width="15%" id="td3">
                    <b>Project</b>
                </td>
            </tr>
            <tr>
                <td>Total number of modules to be passed
                </td>
                <td align="center" id="tdTotalTheoryModules" runat="server"></td>
                <%--<td align="center" id="tdTotalElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdTotalPracticalModules" runat="server"></td>
                <td align="center" id="tdTotalProjectModules" runat="server"></td>
            </tr>
            <tr>
                <td>Total number of modules attempted till date
                </td>
                <td align="center" id="tdAttemptedTheoryModules" runat="server"></td>
                <%--<td align="center" id="tdAttemptedElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdAttemptedPracticalModules" runat="server"></td>
                <td align="center" id="tdAttemptedProjectModules" runat="server"></td>
            </tr>
            <tr>
                <td>Total number of modules passed till date
                </td>
                <td align="center" id="tdPassedTheoryModules" runat="server"></td>
                <%--<td align="center" id="tdPassedElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdPassedPracticalModules" runat="server"></td>
                <td align="center" id="tdPassedProjectModules" runat="server"></td>
            </tr>
            <tr>
                <td>Total number of modules remaining to pass
                </td>
                <td align="center" id="tdRemainingTheorygModules" runat="server"></td>
                <%--<td align="center" id="tdRemainingElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdRemainingPracticalModules" runat="server"></td>
                <td align="center" id="tdRemainingProjectModules" runat="server"></td>
            </tr>
        </table>
    </div>
    <table style="width: 100%;" id="tblMessage" runat="server">
        <tr>
            <td class="box">
                <asp:Label Width="99%" EnableTheming="false" ID="lblMessage" CssClass="error" Visible="false"
                    runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="box">
        <table style="width: 100%;" class="sample3" border="0" cellpadding="3" cellspacing="0"
            runat="server" id="tblRegProcess" visible="false">
            <tr class="gdalternate1">
                <td width="3%" valign="top">&nbsp;
                </td>
                <td valign="top" width="40%">
                    <asp:Label ID="Label2" runat="server" Text="Select Registration Process  "></asp:Label>
                </td>
                <td width="57%">
                    <asp:DropDownList ID="ddlNewRegistration" runat="server" Width="375px" OnSelectedIndexChanged="ddlNewRegistration_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Value="Z"> --Select One--</asp:ListItem>
                        <asp:ListItem Value="C"> Cancel the Existing Registration</asp:ListItem>
                        <asp:ListItem Value="R">ReRegister in the same level</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table style="width: 100%;" class="sample3" border="0" cellpadding="3" cellspacing="1"
                runat="server" id="tblRegistration" visible="false">
                <tr>
                    <td align="center" colspan="3">
                        <asp:Label ID="lblErrorMsg" runat="server" EnableTheming="False" CssClass="error"
                            Visible="False" Width="99%"></asp:Label>
                    </td>
                </tr>
                <tr class="head1">
                    <td align="left" colspan="3">1. Registration Details
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="3%" valign="top">1.1
                    </td>
                    <td valign="top" width="40%">
                        <asp:Label ID="Label1" runat="server" Text="Registration sought for  &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td width="57%">
                        <asp:DropDownList ID="DDLRegForCourse" runat="server" Width="375px" AutoPostBack="true"
                            OnSelectedIndexChanged="DDLRegForCourse_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:HiddenField runat="server" ID="hfCourse" />
                    </td>
                </tr>
                <tr class="gdrow1" id="Tr13" runat="server">
                    <td valign="top" width="3%">1.2
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label58" runat="server" Text="Previous Registration Details 
                                                    &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="lblPreviousCourse" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1" id="TrApplicantType" runat="server">
                    <td width="3%" valign="top">1.3
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label89" runat="server" Text="Applied As <font color='RED'>*</font>"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RdoUndergngDOEACC" runat="server" RepeatDirection="Horizontal"
                            TabIndex="6" Width="182px" AutoPostBack="True" OnSelectedIndexChanged="RdoUndergngDOEACC_SelectedIndexChanged">
                            <asp:ListItem Value="D" Selected="True">Direct</asp:ListItem>
                            <asp:ListItem Value="I">Institute</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <%--    <tr class="gdrow1" id="TrExperience" runat="server">
                    <td valign="top">
                        1.3.1
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label63" runat="server" Text="  If direct, experience in years "></asp:Label>
                        <asp:HiddenField ID="HfExperience" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="TxtExperienceInYears" runat="server" Width="93px" onkeypress="checkNumber(this,4,0,event);"
                            MaxLength="2" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr class="gdrow1" id="TrLastCenterAccno" runat="server" visible="false">
                    <td valign="top">1.3.1
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label9" runat="server" Text="Select State of Accredited Institute  <font color='RED'>*</font>"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="DdlAccState" runat="server" Width="375px" AutoPostBack="True"
                            OnSelectedIndexChanged="DdlAccState_SelectedIndexChanged" TabIndex="7">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem>Rajasthan</asp:ListItem>
                            <asp:ListItem>Delhi</asp:ListItem>
                            <asp:ListItem>Bihar</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="gdalternate1" id="TrLastCenterInstiName" runat="server" visible="false">
                    <td valign="top">1.3.2
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label62" runat="server" Text="Select District of Accredited Institute  <font color='RED'>*</font>"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlAccDistrict" runat="server" TabIndex="8" Width="375px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DdlAccDistrict_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="TrAccCentre" runat="server" visible="false" class="gdalternate1">
                    <td align="left" style="width: 3%" valign="top">1.3.2
                    </td>
                    <td align="left">
                        <asp:Label ID="Label12" runat="server" Text="Select Centre Name of Accredited Institute <font color='RED'>*</font>"></asp:Label>
                    </td>
                    <td align="left" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlAccCentre" runat="server" Width="420px" AutoPostBack="True"
                                    TabIndex="9">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr class="gdrow1" id="trPaymentSource" runat="server">
                    <td>1.4
                    </td>
                    <td>Applicable Registration Fee Will Be Paid By?
                    </td>
                    <td style="padding-left: 0px;" align="left">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPaymentOption" runat="server" Width="400px" Enabled="false">
                                    <asp:ListItem Value="1" Text="Candidate directly to NIELIT (Using Available Payment Methods)"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Accredited Centre"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RdoUndergngDOEACC" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr class="gdalternate1" id="TrExamName" runat="server">
                    <td valign="top">1.5
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label90" runat="server" Text="Exam Name&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td style="margin-left: 40px">
                        <%--<asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>--%>
                        <asp:Label ID="LblExamName" runat="server" BackColor="White" Width="275px" BorderStyle="Solid"
                            BorderWidth="1px" Height="22px"></asp:Label>
                        <asp:Label ID="lblFeeDetail" runat="server" Font-Size="Small"></asp:Label>
                        <asp:ImageButton ID="ImgBtnPopupFee" runat="server" ImageUrl="~/images/DisablePopup.PNG"
                            Enabled="False" TabIndex="10" />
                        <asp:ModalPopupExtender Drag="true" ID="ModalPopupExtender2" runat="server" DropShadow="true"
                            CancelControlID="ImgCancle1" TargetControlID="ImgBtnPopupFee" PopupControlID="InfoDiv">
                        </asp:ModalPopupExtender>
                        <div id="InfoDiv" runat="server" class="modalPopup">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
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
                <tr class="head1">
                    <td colspan="3">2. Educational / Qualification Details
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td valign="top">2.1
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label10" runat="server" Text="Previous Qualification Details &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblPrevQual" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1" valign="top" id="cls1" runat="server">
                    <td>2.2
                    </td>
                    <td>
                        <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLeducode" runat="server" Width="375px" SkinID="25" TabIndex="46"
                            AutoPostBack="true" OnSelectedIndexChanged="DDLeducode_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="gdrow1" id="cls2" runat="server" valign="top">
                    <td>2.3
                    </td>
                    <td>
                        <asp:Label ID="Label82" runat="server" Text="Year of Passing &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtYearOfPassing2" Width="93px" runat="server" onkeypress="checkNumber(this,4,0,event);"
                            MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
                <tr class="gdalternate1" id="TrExperience" runat="server">
                    <td valign="top">2.4
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label63" runat="server" Text="  If direct, experience in years "></asp:Label>
                        <%--   <asp:HiddenField ID="HfExperience" runat="server" />--%>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="TxtExperienceInYears" runat="server" Width="93px" onkeypress="checkNumber(this,1,2,event);"
                                    MaxLength="4" TabIndex="2"></asp:TextBox>
                                <asp:Label ID="lblExperienceInYears" runat="server" Visible="False" EnableTheming="False"></asp:Label>
                                <asp:HiddenField ID="HfExperience" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="DDLeducode" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <%-- <asp:TextBox ID="TxtExperienceInYears" runat="server" Width="93px" onkeypress="checkNumber(this,4,0,event);"
                            MaxLength="2" TabIndex="2"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr class="head1">
                    <td colspan="3">3. Personal Details
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td>3.1
                    </td>
                    <td>
                        <asp:Label ID="Label73" runat="server" Text="Religion &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReligion" runat="server" Width="375px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td>3.2
                    </td>
                    <td>
                        <asp:Label ID="Label30" runat="server" Text="Visible Distinguishing Mark<font color='RED'>*</font>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBodyMark" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <!--Added for apaar-->
                <tr class="head1">
                    <td colspan="3">4. Identification Details
                    </td>
                </tr>
                <tr class="gdalternate1" id="Apaartr" runat="server" visible="true">
                    <td>4.1</td>
                    <td>
                        <asp:Label ID="Label15" runat="server" Text="Apaar&nbsp; ID<font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtapaar" runat="server" MaxLength="12" TabIndex="49" Width="520px"
                            AutoPostBack="true" OnTextChanged="txtapaar_TextChanged"
                            onpaste="return false;" oncopy="return false;" oncut="return false;">
                        </asp:TextBox>

                        <a href="#" onclick="window.open('https://www.abc.gov.in')">Click here to generate Apaar Id</a>

                    </td>
                </tr>
                <tr class="gdalternate1" id="trname" runat="server" visible="true">
                    <td>4.2</td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Name<font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppName" Enabled="false" runat="server" TabIndex="49" Width="520px">
                        </asp:TextBox>



                    </td>
                </tr>
                <tr class="gdalternate1" id="trDob" runat="server" visible="true">
                    <td>4.3</td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Date of Birth<font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDob" Enabled="false" runat="server" MaxLength="12" TabIndex="49" Width="520px">
                        </asp:TextBox>



                    </td>
                </tr>
                <tr class="gdalternate1" id="trgender" runat="server" visible="true">
                    <td>4.4</td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Gender<font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGender" Enabled="false" runat="server" TabIndex="49" Width="520px">
                        </asp:TextBox>



                    </td>
                </tr>
                <!--Apaar End-->
                <%-- added for apaar validation start--%>
                <tr class="head1">
                    <td colspan="3">5. Apaar Consent
                    </td>
                </tr>
                <tr class="gdrow1" id="TrProviderPresent" runat="server" visible="true">
                    <td>5.1</td>
                    <td>
                        <asp:Label ID="lblProviderPresent" runat="server"
                            Text="Is Provider Present / क्या प्रदाता उपस्थित है <font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>

                        <asp:TextBox ID="txtIsProviderPresent" runat="server"
                            Text="True" Width="520px" ReadOnly="true">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr class="gdalternate1" id="TrConsentRelation" runat="server" visible="true">
                    <td>5.2</td>
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
                    <td>5.3</td>

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
                    <td>5.4</td>
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
                    <td>5.5</td>
                    <td>
                        <asp:Label ID="lblAuthId" runat="server"
                            Text="Authentication ID No / प्रमाणीकरण आईडी संख्या <font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAuthenticationIdNo" runat="server"
                            MaxLength="50" Width="520px"
                            AutoPostBack="true"
                            OnTextChanged="txtAuthenticationIdNo_TextChanged"
                            onkeypress="return isNumberKey(event);"
                            onpaste="return false;" oncopy="return false;" oncut="return false;">
                        </asp:TextBox>
                    </td>
                </tr>



                <tr class="gdalternate1" id="TrConsentDate" runat="server" visible="true">
                    <td>5.6</td>
                    <td>
                        <asp:Label ID="lblConsentDate" runat="server"
                            Text="Consent Date / सहमति दिनांक <font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtConsentDate" runat="server"
                            Width="520px" ReadOnly="true">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr class="gdrow1" id="TrConsentTime" runat="server" visible="true">
                    <td>5.7</td>
                    <td>
                        <asp:Label ID="lblConsentTime" runat="server"
                            Text="Consent Time / सहमति समय <font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtConsentTime" runat="server"
                            Width="520px" ReadOnly="true">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr class="gdalternate1" id="TrConsentPlace" runat="server" visible="true">
                    <td>5.8</td>
                    <td>
                        <asp:Label ID="lblConsentPlace" runat="server"
                            Text="Consent Place (alphabets, one space and apostrophe)/ सहमति स्थान <font color='RED'>*</font>">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtConsentPlace" runat="server"
                            Width="520px" MaxLength="100">
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
                                        runat="server">
                                    </asp:Label>

                                    <br />

                                </td>

                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%-- added for apaar validation end--%>

                <tr class="head1">
                    <td colspan="3">Declaration
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td align="left" valign="top" colspan="3">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: justify;" class="odd1">
                                    <asp:CheckBox ID="chk1" runat="server" TabIndex="40" Text="<font color='RED'>*</font>"
                                        align="left" />
                                    I,hereby declare that, I agree to abide by the rules and regulations of NIELIT and
                                    also to the decision of the Examination authority, regarding my admission to the
                                    examination. I have noted that the Examination Authority has the right to withhold
                                    my result ever after my appearing in the Examination in addition to any other action
                                    as may be deemed fit in the event of any of the statements made above being found
                                    incorrect.
                                </td>
                            </tr>
                            <tr id="trConfirm" runat="server" visible="false">
                                <td style="text-align: justify;" class="odd1">
                                    <asp:CheckBox ID="ChkConfirmCancel" runat="server" TabIndex="40" Text="<font color='RED'>*</font>"
                                        align="left" />
                                    I,hereby declare that, I agree to cancel my current level registration.
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlNewRegistration" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <div style="text-align: right; margin-top: 10px" runat="server" id="divSave" visible="false">
        <asp:Button ID="btnSave" runat="server" Text="Submit" TabIndex="53" Width="100px"
            OnClick="btnSave_Click" Style="height: 26px" />
    </div>
    <div id="Lblnote" runat="server" visible="false" class="error" style="width: 99%;">
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="SideLink2" runat="server" Visible="false" />
    <div>
        <uc2:SideLink ID="SideLink1" runat="server" />
    </div>
</asp:Content>
