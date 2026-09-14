<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlinePuraskarApplicationForm.aspx.cs" Inherits="OnlinePuraskarApplicationForm"
    MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Online Puraskar Application Form" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ValidateForm() {
            if (!isBlank("<%=txtPHCertNo.ClientID  %>", "Physically handicapped Certificate No."))
                return false;
            if (!isBlankDate("<%=txtPHCertDate.ClientID %>", "Physically handicapped Certificate Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlank("<%=txtAadharNumber.ClientID  %>", "Aadhaar No."))
                return false;
            if (!isNumber("<%=txtAadharNumber.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isBlank("<%=txtAnnualIncome.ClientID  %>", "Annual Income"))
                return false;
            if (!isNumber("<%=txtAnnualIncome.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isBlank("<%=txtAccountNo.ClientID  %>", "Account No."))
                return false;
            if (!isNumber("<%=txtAccountNo.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isBlank("<%=txtAccountHolderName.ClientID  %>", "Account Holder Name"))
                return false;
            if (!isBlank("<%=txtAccountType.ClientID  %>", "Account Type"))
                return false;
            if (!isBlank("<%=txtBankName.ClientID  %>", "Bank Name"))
                return false;
            if (!isBlank("<%=txtBankAddress.ClientID  %>", "Bank Address"))
                return false;
            if (!isBlank("<%=txtBankIFSC.ClientID  %>", "Bank IFSC Code"))
                return false;
            if (!isBlank("<%=txtCasteCertNo.ClientID  %>", "Caste Certificate No."))
                return false;
            if (!isBlankDate("<%=txtCasteCertDate.ClientID %>", "Caste Certificate Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlank("<%=txtIncomeCertNo.ClientID  %>", "Income Certificate No."))
                return false;
            if (!isBlankDate("<%=txtIncomeCertDate.ClientID %>", "Income Certificate Date", "dd-MMM-yyyy")) 
                return false;

            return true;
        }
    </script>
    <script type="text/javascript">
        function check_Number(id) {
            if (!isBlank("<%=txtmobileverificationcode.ClientID  %>", "Verification Code"))
                return false;
            if (!isNumber("<%=txtmobileverificationcode.ClientID %>", "Numeric characters are  allowed"))
                return false;

            if (!isBlank("<%=txtEmailverificationcode.ClientID  %>", "Verification Code"))
                return false;
            if (!isNumber("<%=txtEmailverificationcode.ClientID %>", "Numeric characters are  allowed"))
                return false;
        }
        </script>
    <div id="OnlinePuraskarAppForm" runat="server" visible="true">
        <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">             
            <tr>
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">PURASKAR APPLICATION FORM                        
                    </strong>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;
                     <asp:Label ID="Label7" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>          

            <tr class="head1">
                <td align="left" colspan="3">Applicant's Personal Details
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Name
                </td>
                <td width="44%">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">Parent / Guardian Details
                </td>
                <td width="44%">
                    <asp:Label ID="lblParent_GuardianDetails" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Gender
                </td>
                <td width="44%">
                    <asp:Label ID="lblGender" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">Date of birth
                </td>
                <td width="44%">
                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Caste
                </td>
                <td width="44%">
                    <asp:Label ID="LblCaste" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="TrFatherName" runat="server">
                <td width="36%">Email-Id
                </td>
                <td width="44%">
                    <asp:Label ID="LblEmailId" runat="server"></asp:Label>
                    <asp:Button ID="btnsendOTPEmail" runat="server" OnClick="btnsendOTPEmail_Click" Text="Send OTP" />
                    <asp:Label ID="lblConfiramationEmail" runat="server" EnableTheming="False" Visible="False" ForeColor="Green" ></asp:Label>
                </td>
            </tr>
             <tr class="gdalternate1" id="TrEmailVerification" runat="server" visible="false">
                <td width="36%">Verification Code
                </td>
                <td width="44%">
                    <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanel6" UpdateMode="Conditional" runat="server">
                          <ContentTemplate>
                             <asp:TextBox id="txtEmailverificationcode" runat="server"   MaxLength="10"></asp:TextBox> 
                             <asp:Button ID="btnEmailVerify" runat="server"  OnClick="btnEmailVerify_Click" OnClientClick="return check_Number();" Style="height: 26px"  Visible="false" Text="Verify" />
                             <asp:Label ID="lblerrorEmail" runat="server" EnableTheming="False" Visible="False" ForeColor="Red" ></asp:Label>  
                          </ContentTemplate>
                         <Triggers>
                                 <asp:PostBackTrigger ControlID="btnEmailVerify" />            
                         </Triggers>
                     </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdrow1" id="TrMotherName" runat="server">
                <td width="36%">Mobile Number
                </td>
                <td width="44%">
                    <asp:Label ID="LblMobileNumber" runat="server"></asp:Label>
                    <asp:Button ID="btnsendOTP" runat="server" OnClick="btnsendOTP_Click" Text="Send OTP" />
                    <asp:Label ID="lblConfiramation" runat="server" EnableTheming="False" Visible="False" ForeColor="Green" ></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="TrMobileVerification" runat="server" visible="false">
                <td width="36%">Verification Code
                </td>
                <td width="44%">
                    <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional" runat="server">
                          <ContentTemplate>
                             <asp:TextBox id="txtmobileverificationcode" runat="server"   MaxLength="10"></asp:TextBox> 
                             <asp:Button ID="btnMobileVerify" runat="server"  OnClick="btnMobileVerify_Click" OnClientClick="return check_Number();" Style="height: 26px"  Visible="false" Text="Verify" />
                             <asp:Label ID="lblerror" runat="server" EnableTheming="False" Visible="False" ForeColor="Red" ></asp:Label>  
                          </ContentTemplate>
                         <Triggers>
                                 <asp:PostBackTrigger ControlID="btnMobileVerify" />            
                         </Triggers>
                     </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdalternate1" id="trgender" runat="server">
                <td width="36%">
                    <asp:Label ID="Label8" runat="server" Text="Institute Details"></asp:Label>
                </td>
                <td width="44%" colspan="2">
                    <asp:Label ID="LblInstituteDetails" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trdob" runat="server">
                <td width="36%">
                    <asp:Label ID="Label12" runat="server" Text="Is-Handicapped "></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <%--<asp:Label ID="LblIsHandicapped" runat="server"></asp:Label>--%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlPWD" runat="server" SkinID="ddl250" Enabled="false">
                                <asp:ListItem Value="0">---Select One---</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label1" runat="server" EnableTheming="True" Text="Required " Visible="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <div id="PHCertDetailsInputView" runat="server" visible="false">
                <tr class="gdalternate1">
                    <td width="36%">
                        <asp:Label ID="LblPHCertNo" runat="server" Text="Physically handicapped Certificate No"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">

                        <asp:TextBox ID="txtPHCertNo" runat="server"
                            Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                            oncut="return false;"></asp:TextBox>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="36%">
                        <asp:Label ID="lblPHCertDate" runat="server" EnableTheming="True" Text="Physically handicapped Certificate Date "></asp:Label>
                    </td>
                    <td colspan="2" width="44%">                       
                        <asp:TextBox ID="txtPHCertDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtPHCertDate">
                        </asp:CalendarExtender>
                        <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="36%">
                        <asp:Label ID="LblPHCertUpload" runat="server" EnableTheming="True" Text="Physically handicapped Certificate Upload"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">                       
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>                                
                                <asp:FileUpload ID="PHCertUpload" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="360px" /><br />
                                ( PDF file with size upto 100 KB )   
                                <asp:Label ID="lblPHCertiUpload" runat="server" Visible="false"></asp:Label>
                                <asp:RegularExpressionValidator ID="regPHCertUpload" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="PHCertUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </div>
            <tr class="gdrow1">
                <td width="36%">
                    <asp:Label ID="lblRegnNo1" runat="server" EnableTheming="True" Text="Regn No"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:Label ID="LblRegnNo" runat="server"></asp:Label>
                </td>
            </tr>           
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblAadhaarNumber" runat="server" Text="Aadhaar Number"></asp:Label>
                </td>
                <td colspan="2" width="44%">                   
                    <asp:TextBox ID="txtAadharNumber" runat="server"
                        Width="280px" MaxLength="12" onpaste="return false;" oncopy="return false;"
                       onkeypress="checkNumber(this,12,0,event);" oncut="return false;" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">
                    <asp:Label ID="LblAnnualIncome" runat="server" EnableTheming="True" Text="Annual Income"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:TextBox ID="txtAnnualIncome" runat="server"
                        Width="280px" MaxLength="6" onpaste="return false;" oncopy="return false;"
                       onkeypress="checkNumber(this,6,0,event);" oncut="return false;"></asp:TextBox>

                    

                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="Label5" runat="server" Text="Exam"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:Label ID="LblExam" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td align="left" colspan="3">Aadhaar Linked Bank Details mapped with NPCI for DBT  (Direct Benefit Transfer)
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">
                    <asp:Label ID="LblAccountNo" runat="server" EnableTheming="True" Text="Account No"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:TextBox ID="txtAccountNo" runat="server"
                        Width="280px" MaxLength="30" onpaste="return false;" oncopy="return false;"
                      onkeypress="checkNumber(this,30,0,event);"  oncut="return false;"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblAccountHolderName" runat="server" Text="Account Holder Name"></asp:Label>
                </td>
                <td colspan="2" width="44%">

                    <asp:TextBox ID="txtAccountHolderName" runat="server"
                        Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                        oncut="return false;"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">
                    <asp:Label ID="LblAccountType" runat="server" EnableTheming="True" Text="Account Type"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:TextBox ID="txtAccountType" runat="server"
                        Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                        oncut="return false;"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblBankName" runat="server" Text="Bank Name"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:TextBox ID="txtBankName" runat="server"
                        Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                        oncut="return false;"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">
                    <asp:Label ID="LblBankAddress" runat="server" EnableTheming="True" Text="Bank Address"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:TextBox ID="txtBankAddress" runat="server"
                        Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                        oncut="return false;"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblBankIFSC" runat="server" Text="Bank IFSC"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:TextBox ID="txtBankIFSC" runat="server"
                        Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                        oncut="return false;"></asp:TextBox>
                </td>
            </tr>            
            <div id="CasteCertDetailsInputView" runat="server" visible="false">
                <tr class="head1">
                    <td align="left" colspan="3">Caste Details
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertNo" runat="server" Text="Caste Cert No"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">

                        <asp:TextBox ID="txtCasteCertNo" runat="server"
                            Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                            oncut="return false;"></asp:TextBox>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertDate" runat="server" EnableTheming="True" Text="Caste Cert Date "></asp:Label>
                    </td>
                    <td colspan="2" width="44%">                      
                        <asp:TextBox ID="txtCasteCertDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtCasteCertDate">
                        </asp:CalendarExtender>
                        <img id="img1" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertUpload" runat="server" EnableTheming="True" Text="Caste Cert Upload"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>

                                <asp:FileUpload ID="CasteCertFileUpload" runat="server" onkeypress="return false;" TabIndex="45" /><br />
                                ( PDF file with size upto 100 KB  )  
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                ControlToValidate="CasteCertFileUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                Display="Dynamic" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </div>
            <tr class="head1">
                <td align="left" colspan="3">Income Details
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertNo" runat="server" Text="Income Certificate No"></asp:Label>
                </td>
                <td colspan="2" width="44%">

                    <asp:TextBox ID="txtIncomeCertNo" runat="server"
                        Width="280px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                        oncut="return false;"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertDate" runat="server" EnableTheming="True" Text="Income Certificate Date &lt;br/&gt;&lt;font color=&quot;red&quot;&gt; Certificate should not be older than three years&lt;/font&gt;"></asp:Label>
                </td>
                <td colspan="2" width="44%">                    
                    <asp:TextBox ID="txtIncomeCertDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtIncomeCertDate">
                    </asp:CalendarExtender>
                    <img id="img3" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertUpload" runat="server" EnableTheming="True" Text="Income Certificate File&lt;br/&gt;&lt;font color=&quot;red&quot;&gt;Income Certificate must be issued from SDM/SDO/BDO/Tehsildar in whose jusrisdiction you reside.&lt;/font&gt;"></asp:Label>
                    <br />
                </td>
                <td colspan="2" width="44%">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>

                            <asp:FileUpload ID="IncomeCerFileUpload" runat="server" onkeypress="return false;" TabIndex="45" /><br />
                            ( PDF file with size upto 100 KB  ) 
            <asp:RegularExpressionValidator ID="IncomeCerFileU" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                ControlToValidate="IncomeCerFileUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                Display="Dynamic" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
            <tr class="head1">
                <td align="left" colspan="3">Modules Details
                </td>
            </tr>
            <tr class="gdrow1">
                <td colspan="2">
                    <div id="paperviewcount" runat="server" visible="false">

                        <table class="sample3" style="width: 90%; text-align: left" border="0" cellpadding="3"
                            cellspacing="1">

                            <tr class="gdrow1">
                                <td width="25%">

                                    <asp:Label ID="lblpaperaap" runat="server" Text="Papers Appeared" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="15%">
                                    <%-- <asp:Label ID="lblpaperaap" runat="server"></asp:Label>--%>
                                    <asp:TextBox ID="txtpaperaap" runat="server"
                                        Width="90px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                                        oncut="return false;" Font-Bold="True"></asp:TextBox>
                                </td>
                                <td width="25%">

                                    <asp:Label ID="Label6" runat="server" Text="Papers Passed" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="15%">
                                    <%--<asp:Label ID="lblpaperpass" runat="server"></asp:Label>--%>
                                    <asp:TextBox ID="txtpaperpass" runat="server"
                                        Width="90px" MaxLength="100" onpaste="return false;" oncopy="return false;"
                                        oncut="return false;" Font-Bold="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="GVPaper" CssClass="table table-bordered table-condensed" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="ID" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                        CellPadding="4"
                        EmptyDataText="No records has been added." Width="50%">

                        <Columns>
                            <asp:BoundField DataField="SL" HeaderText="SL No." />
                            <%--<asp:TemplateField HeaderText="Paper Appered" >
                            <ItemTemplate >
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Appred") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>--%>

                            <%-- <asp:TemplateField HeaderText="Paper Passed" >
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkPassed" runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>--%>
                            <asp:BoundField DataField="Appred" HeaderText="Paper Appered" ItemStyle-Width="450px" />
                            <asp:TemplateField HeaderText="Paper Passed">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkPassed" runat="server" Checked='<%# Bind("IsSelected") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>


            <tr class="head1">
                                <td colspan="3" id="tddeclarartion" runat="server">Declaration
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td align="left" valign="top" colspan="3">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="style107"></td>
                                            <td style="text-align: justify;">
                                                <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="42" Text="<font color='RED'>*</font>" />
                                                I,
                                            <%--<asp:Label ID="Label2" runat="server" Text=""></asp:Label>,
                                            <asp:Label ID="Lblsalutation" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="LblDecMName" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="LblDecFname" runat="server" Text=""></asp:Label>--%>
                                                hereby declare that I have submitted the complete examination fees
                                                 through Online Process while applying for the aforesaid examination through Online Process.
                                            
                                            
                                           
                                            </td>
                                        </tr>


        </table>
<table>
 <tr class="head1">
                                <td colspan="3" id="td1" runat="server">Disclaimer
                                </td>
                            </tr>
                            <tr>
                                <td class="style107"></td>
                                            <td style="text-align: justify;">
                                                In any case, during verification, if found that, caste certificate and Income certificate not issued by the competent department, your claim will be summarily rejected and your claim will not be settled.
                                            
                                            
                                           
                                            </td>
                                        </tr>
</table>
        <div style="text-align: right; margin-top: 10px; height: 80px">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave" runat="server" OnClick="SaveRecord" OnClientClick="return ValidateForm();" Style="height: 26px"
                        Text="Save" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="divAadhar" runat="server" visible="false">
         <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">
             <tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">
                        <asp:Label Width="99%" EnableTheming="False" ID="Aadhar"
            runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Medium"></asp:Label>                       
                    </strong>
                </td>
            </tr>
             </table>
        
        </div>
    <div id="divSumbitMsg" runat="server" visible="false">
         <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">
             <tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">
                        <asp:Label Width="99%" EnableTheming="False" ID="LblSubmitMessage"
            runat="server" ForeColor="Green" Font-Bold="True" Font-Size="Medium"></asp:Label>                       
                    </strong>
                </td>
            </tr>
             </table>
        
    </div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" align="center" class="nav" cellspacing="0" cellpadding="0"
        id="tblNavLinks" width="97%" visible="false">
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">               
                <asp:HyperLink ID="hl1" runat="server" Target="_blank">Print Puraskar Application Form</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>