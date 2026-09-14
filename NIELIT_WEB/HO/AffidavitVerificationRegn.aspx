<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AffidavitVerificationRegn.aspx.cs" Inherits="HO_AffidavitVerificationRegn" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Afidavit Verification Process
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
        function ValidateForm() {

            if (!isSelected("<%= ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isBlank("<%= txtAppNo.ClientID %>", "Application Number"))
                return false;
           
            return true;

        }

    </script>
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
        Width="99%"></asp:Label>
    <div class="box" id= "DivSearch" runat="server">
        <table class="sample3" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr class="gdrow1">
                <td>
                    <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" Width="220px">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblAppNo" runat="server" Text="Application Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAppNo" runat="server" Width="150px" MaxLength="30"  onpaste="return false;" oncopy="return false;" oncut="return false;"
                        TabIndex="1"></asp:TextBox>
                </td>
                <td valign="top">
                    &nbsp;
                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/images/search_btn.jpg"
                        ToolTip="Serch" OnClick="ImgBtnSearch_Click" OnClientClick="return ValidateForm();"
                        Style="height: 22px" TabIndex="2" />
                </td>
                <td valign="top">
                    <asp:ImageButton ID="ImgBtnReset" runat="server" ImageUrl="~/images/reset_btn.jpg"
                        OnClick="ImgBtnReset_Click" Visible="False" TabIndex="3" />
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlCandidate" runat="server" Visible="False">
                
                <table border="0" cellpadding="2" cellspacing="1" class="sample3" style="width: 100%;">
                    <tr class="head1">
                        <td align="left" width="25%">
                            1. Candidate Details
                        </td>
                        <td align="left" width="25%">
                            &nbsp;</td>
                        <td align="right" width="25%">
                            <asp:Button ID="BtnBack" runat="server" onclick="BtnBack_Click" Text="Back" 
                                Visible="False" TabIndex="4" />
                        </td>
                        <td align="left" width="25%">
                            &nbsp;</td>
                      
                    </tr>
                    <%--<tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Roll Number "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRollNum" runat="server"></asp:Label>
                        </td>
                       
                  
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Current Course"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCourse" runat="server"></asp:Label>
                        </td>
                        
                    </tr>--%>

                     <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="lblExamMonth" runat="server" Text="Exam Month "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblExamMonthV" runat="server"></asp:Label>
                        </td>
                       
                  
                        <td>
                            <asp:Label ID="lblExamyear" runat="server" Text="Exam year"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblExamYearV" runat="server"></asp:Label>
                        </td>
                        
                    </tr>
                    <%--<tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label80" runat="server" Text="Locked On"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblLockedOn" runat="server"></asp:Label>
                        </td>
                        
                        <td>
                            <asp:Label ID="Label81" runat="server" Text="Verified On"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblVerifiedOn" runat="server"></asp:Label>
&nbsp;
                            <asp:LinkButton ID="btnVerify" runat="server" Visible ="false">Verify 
                            Now</asp:LinkButton>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>--%>
                   <%-- <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label82" runat="server" Text="Synced On"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblSyncedOn" runat="server"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>--%>
                    <tr class="head1">
                        <td  >
                            2.
                            <asp:Label ID="Label24" runat="server" Text="Personal Details "></asp:Label>
                        </td>
                       
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            <asp:Label ID="Label25" runat="server" Text="Name of Candidate "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblAppName" runat="server"></asp:Label>
                        </td>
                      <td id="trGuardian" runat="server" class="gdalternate1" visible="false">
                            <asp:Label ID="Label28" runat="server" Text="Guardian's Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblGName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trParent" runat="server" class="gdrow1"> 
                        <td>
                            <asp:Label ID="Label26" runat="server" Text="Father's Name "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblFName" runat="server"></asp:Label>
                        </td>
      
                   
                        <td>
                            <asp:Label ID="Label27" runat="server" Text="Mother's Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblMName" runat="server"></asp:Label>
                        </td>
      
                    </tr>
                    <%--<tr id="trGuardian" runat="server" class="gdalternate1" visible="false">
                        <td>
                            <asp:Label ID="Label28" runat="server" Text="Guardian's Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblGName" runat="server"></asp:Label>
                        </td>
      
                    </tr>--%>
                      <tr id="trAffidavit" runat="server" class="gdalternate1" visible="false">
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Affidavit No."></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAffidavitNo" runat="server"></asp:Label>
                        </td>
                       
                    
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Affidavit Date"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAffidavitDate" runat="server"></asp:Label>
                        </td>
                       
                    </tr>
                    <tr id="trShowAffidavit" runat ="server" class="gdalternate1" visible ="false" >
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Affidavit"></asp:Label>
                        </td>
                        <td>
                            <asp:LinkButton id="lnkAffidavit" runat="server" Text="Click to check the affidavit" OnClick="lnkAffidavit_Click" > </asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Affidavit Verified On"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAffidavitVerifiedDate" runat="server"></asp:Label>
&nbsp;
                            <asp:LinkButton ID="lnkAffidavitVerified" runat="server" onclick="btnVerifyAffidavit_Click" OnClientClick ="return confirm('Are you sure you want to verify');">Verify Affidavit
                            Now</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdrow1" id="Trdob" runat="server">
                        <td>
                            <asp:Label ID="Label30" runat="server" Text="Date of Birth"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblDob" runat="server"></asp:Label>
                        </td>
      
                    </tr>
                    <tr class="head1">
                        <td  >
                            3. Contact Details
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            <asp:Label ID="Label35" runat="server" Text="Phone No. with STD"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="LblPhone" runat="server"></asp:Label>
                        </td>
      
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Mobile No."></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMobile" runat="server"></asp:Label>
                        </td>
      
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            <asp:Label ID="Label37" runat="server" Text="Email Address"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblEmail" runat="server"></asp:Label>
                        </td>
      
                    </tr>
                    <tr class="head1">
                        <td >
                            4. Address Details
                        </td>
      
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label75" runat="server" Text="Address1"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblAdd1" runat="server"></asp:Label>
                        </td>
      
                  
                        <td>
                            <asp:Label ID="Label77" runat="server" Text="Address2"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblAdd2" runat="server"></asp:Label>
                        </td>
                    
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Address3"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAdd3" runat="server"></asp:Label>
                        </td>
                    
                   
                        <td>
                            <asp:Label ID="Label76" runat="server" Text="City"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblCity" runat="server"></asp:Label>
                        </td>
                    
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label78" runat="server" Text="District"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblDistrict" runat="server"></asp:Label>
                        </td>
                    
                    
                        <td>
                            <asp:Label ID="Label79" runat="server" Text="State"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblState" runat="server"></asp:Label>
                        </td>
                    
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label38" runat="server" Text="Pin Code"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblPinCode" runat="server"></asp:Label>
                        </td>
                    
                    </tr>
                </table>
                <table border="0" cellpadding="3" cellspacing="0" class="box" 
                    style="text-align: left" width="100%" runat="server" id="tabphoto">
                    <tr >
                        <td align="center" width="30%">
                            <asp:Image ID="ImgCandidatePhoto" runat="server" Height="135px" 
                                ImageUrl="../images/photo.jpg" />
                        </td>
                        <td align="center" valign="bottom" width="30%">
                            <img id="imgSignature" runat="server" src="../images/sign.jpg" style="height: 27px;
                                width: 111px" />
                        </td>
                        <td align="center" valign="bottom" width="30%">
                            <img id="imgThumb" runat="server" src="~/images/thumb.jpg" style="height: 27px; width: 111px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Photograph
                        </td>
                        <td align="center">
                            Signature
                        </td>
                        <td align="center">
                            Left Thumb Impression
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
