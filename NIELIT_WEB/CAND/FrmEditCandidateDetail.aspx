<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmEditCandidateDetail.aspx.cs" Inherits="CAND_FrmEditCandidateDetail" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="LblHeading" runat="server" Text="All Change Requests"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateForm() {

            if (!isSelected("<%= DdlAllDetail.ClientID %>", "Request Type Detail"))
                return false;
            if (document.getElementById("<%= TxtGuardianName.ClientID %>") && document.getElementById("<%= TxtFatherName.ClientID %>") && document.getElementById("<%= TxtMotherName.ClientID %>")) {
                var GuardianName = document.getElementById("<%= TxtGuardianName.ClientID %>").value != "" ? document.getElementById("<%= TxtGuardianName.ClientID %>").value : "";
                var FatherName = document.getElementById("<%= TxtFatherName.ClientID %>").value;
                var MotherName = document.getElementById("<%= TxtMotherName.ClientID %>").value;
                if ((trim(GuardianName, " ") == "" && trim(FatherName, " ") == "" && trim(MotherName, " ") == "") || (trim(GuardianName, " ") != "" && trim(FatherName, " ") != "" && trim(MotherName, " ") != "")) {
                    callErrorMsg("<%= TxtGuardianName.ClientID %>", "Please enter either Guardian Name OR Father Name and Mother Name.");
                    return false;
                }

                else if (trim(FatherName, " ") != "" && trim(MotherName, " ") == "") {
                    callErrorMsg("<%= TxtMotherName.ClientID %>", "Please enter Mother Name).");
                    return false;
                }
                else if (trim(MotherName, " ") != "" && trim(FatherName, " ") == "") {
                    callErrorMsg("<%= TxtFatherName.ClientID %>", "Please enter Father Name).");
                    return false;
                }

            }
            if (!isNumber("<%=TxtSTDcode.ClientID %>"))
                return false;

            if (!isNumber("<%=TxtPhoneNo.ClientID %>"))
                return false;
            if (!isValidTeliphone("<%=TxtSTDcode.ClientID %>", "<%=TxtPhoneNo.ClientID %>"))
                return false;
            if (!isBlankNumber("<%=TxtMobile.ClientID %>", "Mobile Number"))
                return false;
            if (!isNumber("<%=TxtMobile.ClientID %>"))
                return false;
            if (!chekMobNo("<%=TxtMobile.ClientID %>"))
                return false;
            if (!isBlank("<%=TxtEmail.ClientID %>", "Email Id"))
                return false;
            if (!isValidEmail("<%=TxtEmail.ClientID %>", "Invalid E-Mail ID"))
                return false;

            if (!isBlank("<%=TxtAddressLine1.ClientID %>", "House Number"))
                return false;
            if (!isBlank("<%=TxtCity.ClientID %>", "City"))
                return false;
            if (!isSelected("<%=ddlCorState.ClientID %>", "State"))
                return false;
            if (!isSelected("<%=ddldistrict.ClientID %>", "District"))
                return false;
            if (!isBlankNumber("<%=TxtPincode.ClientID %>", "Pin Code"))
                return false;
            if (!isNumber("<%=TxtPincode.ClientID %>"))
                return false;
            if (!IsValidMinMaxLenght("<%=TxtPincode.ClientID %>", 6, 6, "Invalid Pin Code"))
                return false;




            // amit_apaar_api_changes_may_2026_start
            if (!isBlank("<%=txtapaar.ClientID %>", "Apaar ID"))
                return false;


            if (!isNumber("<%=txtapaar.ClientID %>", "Apaar ID"))
                return false;

            if (!IsValidMinLength("<%=txtapaar.ClientID %>", "Apaar ID", 12))
                return false;

            if (!isSelected("<%=ddlConsentRelation.ClientID %>", "Consent Relation"))
                return false;


            if (!isSelected("<%=ddlAuthMode.ClientID %>", "Authentication Mode"))
                return false;

            if (!isBlank("<%=txtAuthenticationIdNo.ClientID %>", "Authentication ID No"))
                return false;

            if (!IsValidMinLength("<%=txtAuthenticationIdNo.ClientID %>", "Authentication ID No", 6))
                return false;
            function IsValidMinLength(ctrl, msg, minLen) {

                var obj = document.getElementById(ctrl);

                if (!obj)
                    return true;

                var value = trim(obj.value, "");
                var upperValue = value.toUpperCase();

                var invalidValues = ["NONE", "N/A", "EMPTY"];

                // Check minimum length
                if (value.length < minLen) {
                    CallDiv(ctrl, msg + " must contain at least " + minLen + " characters.");
                    obj.focus();
                    return false;
                }

                // Check invalid values
                for (var i = 0; i < invalidValues.length; i++) {
                    if (upperValue == invalidValues[i]) {
                        CallDiv(ctrl, msg + " contains an invalid value.");
                        obj.value = "";
                        obj.focus();
                        return false;
                    }
                }

                return true;
            }

            if (!isBlank("<%=txtConsentPlace.ClientID %>", "Consent Place"))
                return false;

            if (!ischecked("<%=chkApaarDeclaration.ClientID %>", "Apaar Declaration"))
                return false;

            if (!ischecked("<%=chkWarning.ClientID %>", "Final Information Declaration"))
                return false;
            // amit_apaar_api_changes_may_2026_end



            return true;

        }
        function blankFnameMname() {
            document.getElementById("<%= TxtFatherName.ClientID %>").value = "";
            document.getElementById("<%= TxtMotherName.ClientID %>").value = "";
        }
        function blankGuardianName() {
            document.getElementById("<%= TxtGuardianName.ClientID %>").value = "";
        }
    </script>
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="800px" style="margin-top: 10px">
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
                    Width="99%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
                    <asp:View ID="ViewAllDetails" runat="server">
                        <div class="sample3">
                            <table id="tbl0" runat="server" style="text-align: left" border="0" class="box" cellpadding="3"
                                cellspacing="0" width="100%">
                                <tr class="head1">
                                    <td colspan="2">Change Details
                                    </td>
                                </tr>
                                <tr class="gdalternate1" style="height: 5px;">
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td width="20%">Change Request Type <b class='mandatory'>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DdlAllDetail" runat="server" Width="350px">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                            <%--<asp:ListItem Value="1">Change Personal Details</asp:ListItem>--%>
                                            <asp:ListItem Value="1">Change Personal details</asp:ListItem>
                                            <asp:ListItem Value="2">Change Contact Details</asp:ListItem>
                                            <asp:ListItem Value="3">Change Corespondence Address Details</asp:ListItem>
                                            <%--<asp:ListItem Value="4">Change Permanent Address Details</asp:ListItem>
                                            <asp:ListItem Value="5">Change Educational/Qualification Details</asp:ListItem>
                                            <asp:ListItem Value="6">Change Accredited Centre Details</asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td></td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center; margin-top: 10px">
                                <asp:Button ID="BtnAllContinue" runat="server" OnClick="BtnAllContinue_Click" OnClientClick="return ValidateForm();"
                                    Text="Continue" Height="26px" />
                                <asp:Button ID="BtnAllCancel" runat="server" Text="Cancel" OnClick="BtnAllCancel_Click" />
                                <asp:HiddenField ID="HfRequestTypeID" runat="server" />
                                <asp:HiddenField ID="HfRequestID" runat="server" />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="ViewPersonalDetail" runat="server">
                        <table id="Tbl1ViewMode" runat="server" style="text-align: left" class="sample3"
                            border="0" cellpadding="3" cellspacing="1" width="100%">
                            <tr class="head1">
                                <td colspan="3">Personal Details
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrName" runat="server">
                                <td width="30%">
                                    <asp:Label ID="Label7" runat="server" Text="Full Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblAppName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrFatherName" runat="server">
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Father's Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblFatherName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrMotherName" runat="server">
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Mother's Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblMotherName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrGardianName" runat="server">
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Guardian&#39;s Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label17" runat="server" Text="Gender &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="LblGender" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label19" runat="server" Text="Dob &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDob" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label19" runat="server" Text="Marital Status"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblMaritalStatus" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label23" runat="server" Text="Date of Birth "></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblDob" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label24" runat="server" Text="Cast Category"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblCategory" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                        </table>
                        <table visible="false" id="Tbl1EditMode" runat="server" style="text-align: left"
                            class="sample3" border="0" cellpadding="3" cellspacing="1" width="100%">

                            <tr class="head1">
                                <td colspan="2">Candidate's Personal Details
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrName1" runat="server">

                                <td width="30%">Full Name <b class='mandatory'>*</b>
                                </td>
                                <td>
                                    <asp:Label ID="LblCandNameE" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Gender <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdbtnlstgender" runat="server" RepeatDirection="Horizontal"
                                        TabIndex="5" Width="300px">
                                        <asp:ListItem Value="male">Male / पुरुष </asp:ListItem>
                                        <asp:ListItem Value="female">Female / महिला </asp:ListItem>
                                        <asp:ListItem Value="Trans">Others</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td colspan="2">Guardian's Name /Parents' Detail <b class='mandatory'>*</b>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>
                                    <asp:Label ID="Label42" runat="server" Text="Guardian's Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtGuardianName" OnTextChanged="txtGuardianName_TextChanged" AutoPostBack="true" runat="server" MaxLength="30" Width="282px" oncopy="return false;"
                                        onkeypress="blankFnameMname()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td colspan="2" align="center">
                                    <i><b>Please enter either Guardian Name OR (Father Name and Mother Name).</b></i>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrFatherName1" runat="server">
                                <td>Father's Name
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFatherName" OnTextChanged="txtFatherName_TextChanged" AutoPostBack="true" runat="server" Width="282px" MaxLength="30" oncopy="return false;"
                                        onkeypress="blankGuardianName()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrMotherName1" runat="server">
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Mother's Name "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtMotherName" OnTextChanged="txtMotherName_TextChanged" AutoPostBack="true" runat="server" Width="282px" MaxLength="30" oncopy="return false;"
                                        onkeypress="blankGuardianName()"></asp:TextBox>
                                </td>
                            </tr>
                            <%-- // amit_apaar_api_changes_may_2026_start --%>
                            <tr class="gdalternate1" id="Apaartr" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="Label26" runat="server" Text="Apaar&nbsp; ID / अपार आईडी  <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtapaar" runat="server" MaxLength="12" TabIndex="49" Width="282px" OnTextChanged="txtapaar_TextChanged"
                                        onpaste="return false;" oncopy="return false;" oncut="return false;" AutoPostBack="true" onkeypress="checkNumber(this,15,0,event);">
                                    </asp:TextBox>
                                    <br />
                                    <a href="#" onclick="window.open('https://www.abc.gov.in')">Click here to generate Apaar Id</a>                            </td>
                            </tr>
                            <tr class="gdrow1" id="TrProviderPresent" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="lblProviderPresent" runat="server"
                                        Text="Is Provider Present / क्या प्रदाता उपस्थित है <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIsProviderPresent" runat="server"
                                        Text="True" Width="282px" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrConsentRelation" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="lblConsentRelation" runat="server"
                                        Text="Consent Relation / सहमति संबंध <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlConsentRelation" runat="server"
                                        Width="282px" AutoPostBack="true"
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


                                <td>
                                    <asp:Label ID="Label27" runat="server"
                                        Text="Provider Name / प्रदाता का नाम <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>

                                <td>

                                    <asp:TextBox ID="txtproviderName" runat="server"
                                        MaxLength="100" Width="282px">
                                    </asp:TextBox>


                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrAuthMode" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="lblAuthMode" runat="server"
                                        Text="Authentication Mode / प्रमाणीकरण मोड <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAuthMode" runat="server" Width="282px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlAuthMode_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Label ID="lblauthidrules" Text="" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrAuthId" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="lblAuthId" runat="server"
                                        Text="Authentication ID No / प्रमाणीकरण आईडी संख्या <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updAuth">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtAuthenticationIdNo" runat="server"
                                                MaxLength="50" Width="282px"
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

                                <td>
                                    <asp:Label ID="lblConsentDate" runat="server"
                                        Text="Consent Date / सहमति दिनांक <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentDate" runat="server"
                                        Width="282px" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdrow1" id="TrConsentTime" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="lblConsentTime" runat="server"
                                        Text="Consent Time / सहमति समय <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentTime" Enabled="false" runat="server"
                                        Width="282px">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrConsentPlace" runat="server" visible="true">

                                <td>
                                    <asp:Label ID="lblConsentPlace" runat="server"
                                        Text="Consent Place / सहमति स्थान <font color='RED'>*</font>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsentPlace" MaxLength="30" onpaste="return false" runat="server"
                                        Width="282px">
                                    </asp:TextBox>
                                    <br />
                                    <small>Maximum 30 characters. Only alphabets and hyphens (-) are allowed. Do not enter a full address.
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
                                                    ClientIDMode="Static"
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
                                                    <%--          // amit_apaar_api_changes_may_2026_end--%>  
                            <caption>
                                m
                            </caption>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewContactDetail" runat="server">
                        <table id="Tbl2ViewMode" runat="server" border="0" cellpadding="3" cellspacing="1"
                            class="sample3" style="text-align: left" width="100%">
                            <tr class="head1">
                                <td colspan="2">
                                    <asp:Label ID="LblContactViewHead" runat="server" Text="Change Contact Details "></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label1" runat="server" Text="Phone Number with STD Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblPhoneNo" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label2" runat="server" Text="Mobile Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblMobile" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label8" runat="server" EnableTheming="True" Text="Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblEmail" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table visible="false" id="Tbl2EditMode" runat="server" style="text-align: left"
                            class="sample3" border="0" cellpadding="3" cellspacing="1" width="100%">
                            <tr class="head1">
                                <td colspan="2">
                                    <asp:Label ID="LblContactEditHead" runat="server" Text="Change Contact Details"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td colspan="2">Please Update the details, you wish to change.
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label18" runat="server" Text="Phone Number with STD Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtSTDcode" runat="server" MaxLength="5" onkeypress="checkNumber(this,4,0,event);"
                                        onpaste="return false" ToolTip="Enter STD Code here" Width="65px"></asp:TextBox>
                                    <asp:TextBox ID="TxtPhoneNo" ToolTip="Enter phone number here" runat="server" Width="170px"
                                        onkeypress="checkNumber(this,8,0,event);" MaxLength="8"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label40" runat="server" Text="Mobile Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtMobile" runat="server" Width="250px" onkeypress="checkNumber(this,10,0,event);"
                                        MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email Address <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEmail" runat="server" Width="250px" MaxLength="130"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewCorAddressDetail" runat="server">
                        <table id="Tbl3ViewMode" runat="server" style="text-align: left" class="sample3"
                            border="0" cellpadding="3" cellspacing="1" width="100%">
                            <tr class="head1">
                                <td colspan="2">
                                    <asp:Label ID="LblCorAddressViewHead" runat="server" Text="Change Correspondence Address Details "></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label9" runat="server" Text="Address Line1 &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblAddressLine1" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label11" runat="server" Text="Address Line2"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblAddressLine2" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label12" runat="server" Text="Address Line3"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblAddressLine3" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label16" runat="server" Text="City &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblCity" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label14" runat="server" Text="District &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblDistrict" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label13" runat="server" Text="State &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblState" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label15" runat="server" Text="Pin Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblPincode" runat="server">NA</asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table visible="false" id="Tbl3EditMode" runat="server" style="text-align: left"
                            class="sample3" border="0" cellpadding="3" cellspacing="1" width="100%">
                            <tr class="head1">
                                <td colspan="2">
                                    <asp:Label ID="LblCorAddressEditHead" runat="server" Text="Change Correspondence Address Detail "></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td colspan="2">Please Update the details, you wish to change.
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label32" runat="server" Text="Address Line1 <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAddressLine1" runat="server" Width="250px" MaxLength="60"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label20" runat="server" Text="Address Line2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAddressLine2" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label22" runat="server" Text="Address Line3"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAddressLine3" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">City <b class='mandatory'>*</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCity" runat="server" MaxLength="30" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td width="30%">
                                    <asp:Label ID="Label33" runat="server" Text="State <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCorState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
                                        TabIndex="29" Width="263px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td width="30%">
                                    <asp:Label ID="Label34" runat="server" Text="District <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddldistrict" runat="server" TabIndex="25" Width="263px">
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
                                <td width="30%">
                                    <asp:Label ID="Label39" runat="server" Text="Pin Code <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPincode" runat="server" Width="105px" onkeypress="checkNumber(this,6,0,event);"
                                        MaxLength="6"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>
    <asp:MultiView ID="mvAction" Visible="false" ActiveViewIndex="0" runat="server">
        <asp:View ID="viewConfirm" runat="server">
            <div class="sample3" id="DivChangeConfirm" runat="server">
                <table width="100%" class="box" style="margin-top: 20px" cellpadding="3" cellspacing="0">
                    <tr class="gdalternate1" style="height: 5px;">
                        <td width="73%" style="color: #FF0000" align="left"></td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">Are you sure you want to change your
                            <asp:Label ID="LblReqType" runat="server"></asp:Label>
                            &nbsp;details?
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">If you wish to continue. Please click on Continue to update else on Cancel to go
                            back to profile.
                        </td>
                    </tr>
                    <tr class="gdalternate1" style="height: 5px;">
                        <td align="left" style="color: #FF0000" width="73%"></td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="BtnOTPcontinue" runat="server" OnClick="BtnOTPcontinue_Click" Text="Continue" />
                <asp:Button ID="LnkBtnNo" runat="server" OnClick="LnkBtnNo_Click" Text="Cancel" />
            </div>
        </asp:View>
        <asp:View ID="viewFinal" runat="server">
            <div style="text-align: center; margin-top: 10px;">
                 <%--// amit_apaar_api_changes_may_2026_start--%>
                <div style="display: inline-block; padding: 10px 15px; border: 1px solid #f5c6cb; background-color: #fff3cd; border-radius: 5px; text-align: left;">

                    <asp:CheckBox ID="chkWarning"
                        runat="server"
                       />

                    <span style="color: red; font-weight: bold;">*</span>

                   
                    <span style="font-size: 14px; color: #856404; font-family:Arial;">
                        <strong>Declaration:</strong>
                        I have verified all the information entered above.
            After clicking <strong>Update</strong>, I understand that I may not be allowed to modify these details again.
        </span>

                </div>

                <br />
                <br />
                 <%--// amit_apaar_api_changes_may_2026_end--%>

                <asp:Button ID="BtnUpdate" runat="server" Text="Update"
                    OnClick="BtnUpdate_Click"
                    OnClientClick="return ValidateForm();" />

                <asp:Button ID="BtnCancelUpdate" runat="server" Text="Cancel"
                    Width="55px"
                    OnClick="BtnCancelUpdate_Click" />

            </div>
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="ViewUpdateMessage" runat="server">
            <div class="sample3">
                <table cellpadding="3" cellspacing="0" class="box" style="margin-top: 20px" width="100%">
                    <tr class="head1" style="height: 5px;">
                        <td align="left" style="color: #FFFFFF" width="73%">
                            <asp:Label ID="LblUpdateHeading" runat="server" Text=""></asp:Label>
                            &nbsp;is successfully updated.
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            <asp:Label ID="LblWelcome" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">You have successfully changed your&nbsp;
                            <asp:Label ID="LblDetail" runat="server"></asp:Label>
                            .
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" id="TdContactUpdateMsg" runat="server" visible="false" style="color: #800000; font-size: 17px;"
                            width="73%">but yet not verified ,&nbsp; so please verify these details from Profile page.
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">Please go&nbsp;
                            <asp:LinkButton ID="LnkBtnBacktoProfile1" runat="server" OnClick="LnkBtnBacktoProfile1_Click"> back to dashboard</asp:LinkButton>
                            to see the profile status.
                        </td>
                    </tr>
                    <tr class="gdalternate1" style="height: 5px;">
                        <td align="right" style="color: #FF0000" width="73%">
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LnkBtnBacktoProfile2">Back to dashboard</asp:LinkButton>
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
