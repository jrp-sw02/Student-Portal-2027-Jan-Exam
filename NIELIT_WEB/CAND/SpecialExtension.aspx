<%@ Page Title="Special Extension Registration Form" Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="SpecialExtension.aspx.cs"
    Inherits="SpecialExtension" Debug="true" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
     <script language="javascript" type="text/javascript">
         function ValidateForm() {
             if (!isNumber("txtapaar"))
                 return false;
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

             if (!isBlank("txtConsentDate", "Consent Place"))
                 return false;

             if (!isBlank("txtConsentTime", "Consent Place"))
                 return false;

             if (!isBlank("txtConsentPlace", "Consent Place"))
                 return false;

             if (!isNumber("txtapaar", "Apaar can only contain Number"))
                 return false;
             if (!isBlankNumber("txtMobile", "Mobile Number"))
                 return false;
             if (!isNumber("txtMobile"))
                 return false;
             if (!chekMobNo("txtMobile"))
                 return false;
             if (!isBlank("txtEmail", "Email Id"))
                 return false;
             if (!isValidEmail("txtEmail", "Invalid E-Mail ID"))
                 return false;
             if (!isBlank("txtCorAddressLine1", "correspondence AddressLine 1"))
                 return false;
             if (!isBlank("txtCorAddressLine2", "correspondence AddressLine 2"))
                 return false;
             if (!isBlank("txtCorAddressLine3", "correspondence AddressLine 3"))
                 return false;
             if (!isBlank("txtCorCity", "correspondence City Name"))
                 return false;            
             if (!isBlankNumber("txtPincode", "Pin Code"))
                 return false;
             if (!isNumber("txtPincode"))
                 return false;
             if (!IsValidMinMaxLenght("txtPincode", 6, 6, "Invalid Pin Code"))
                 return false;
             if (!isBlank("ImgUpload", " Upload Image"))
                 return false;
             if (!isvalidImageFile("ImgUpload", "Photo"))
                 return false;
             if (!isBlank("ImgUploadSignature", "Upload Signature"))
                 return false;
             if (!isvalidImageFile("ImgUploadSignature", "Signature"))
                 return false;
             if (!ischecked("chkdisclamier", "Declaration"))
                 return false;
         }

         function isNumberKey(evt) {
             var charCode = evt.which ? evt.which : evt.keyCode;

             // Allow Backspace, Delete, Tab
             if (charCode == 8 || charCode == 46 || charCode == 9)
                 return true;

             // Allow only 0-9
             if (charCode < 48 || charCode > 57)
                 return false;

             return true;
         }

         function AllowOnlyNumbers(evt) {
             var charCode = evt.which ? evt.which : evt.keyCode;

             // Allow Backspace, Tab, Delete, Left Arrow, Right Arrow
             if (charCode == 8 || charCode == 9 || charCode == 46 || charCode == 37 || charCode == 39)
                 return true;

             // Allow only digits 0-9
             if (charCode >= 48 && charCode <= 57)
                 return true;

             return false;
         }
     </script>
    <style type="text/css">
        .DivImage {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 168px;
        }

        .InnerTable {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 100%;
            height: 100%;
        }

        .PhotoImage {
            position: relative;
            right: 3px;
            top: 5px;
            z-index: -1;
        }

        .BarCodeImage {
            position: relative;
            right: 5px;
            top: -1px;
            z-index: 0;
        }

        .normal td {
            padding: 8px;
            vertical-align: middle;
        }

        input[type="file"] {
            width: 95%;
        }

        .addressBox {
            width: 100%;
            box-sizing: border-box;
        }

        .labelCell {
            white-space: nowrap;
        }
    </style>
    <script src="../Script/jquery-3.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
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

        function clearEduError() {
            document.getElementById("lblEduStatus").innerHTML = "";
        }

        function clearExpCertiError() {
            document.getElementById("lblExpCertiFormStatus").innerHTML = "";
        }

        function clearMedicalStatusError() {
            document.getElementById("lblMedicalStatus").innerHTML = "";
        }

        function clearIdCardStatusError() {
            document.getElementById("lblIDCardStatus").innerHTML = "";
        }

        //function ValidateForm() {
        //    if (!isBlank("ImgUpload", " Upload Image"))
        //        return false;
        //    if (!isvalidImageFile("ImgUpload", "Photo"))
        //        return false;
        //    if (!isBlank("ImgUploadSignature", "Upload Signature"))
        //        return false;
        //    if (!isvalidImageFile("ImgUploadSignature", "Signature"))
        //        return false;
        //}
    </script>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="500px" id="Tblpreview"
            runat="server">
            <tr>
                <td>
                    <uc1:NormalHeader ID="NormalHeader1" runat="server" />
                </td>
            </tr>
             <tr>
     <td align="center">
         <strong id="lblMainHeading" runat="server" style="text-align: center" class="headfont">SPECIAL EXTENSION REGISTRATION FORM  
         <%--<asp:Label ID="Label16" runat="server" Text=""></asp:Label>--%></strong>
     </td>
 </tr>
                       <tr id="trlblnote" runat="server" visible="false">
    <td colspan="4"
        style="background-color:#fff8cc;
               border:1px solid #f0ad4e;
               padding:8px;
               font-weight:bold;
               color:#856404;">
        ✏ Note: Fields highlighted in color are editable. Please verify and update the information using link before submitting the application.
    </td>
</tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
                </td>
            </tr>
           <%-- <tr style="height: 45px;">
                <td align="center" style="border-bottom: 1px solid #000000;" valign="middle">
                    <asp:Label ID="Lblhead" runat="server" Style="font-size: 22px;"></asp:Label>--%>
                    <%--<asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 10px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />--%>
              <%--  </td>
            </tr>--%>
            <%--  <tr>
     <td align="center">
         <asp:UpdatePanel ID="UpdatePanel3" runat="server">
             <ContentTemplate>
                 <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                     Width="99%"></asp:Label>
             </ContentTemplate>
         </asp:UpdatePanel>
     </td>
 </tr>--%>
            <tr id="trpreview" runat="server" visible="false">
                <td align="center">
                    <table style="width: 100%;" class="preview" cellpadding="1" cellspacing="0">
                        <tr class="normal" style="font: bold 18px arial;">
                            <td align="center" width="25%">Registration Date
                            </td>
                            <td align="center" width="25%">Valid Upto Date 
                            </td>
                            <td align="center">Registration Type&nbsp;
                            </td>
                           <%-- <td style="border-bottom: 1px solid #000000; border-right: 1px solid #000000;" align="center"
                                valign="middle" rowspan="2" width="18%">
                              
                                <img id="imgPhotoBarcode" runat="server" class="BarCodeImage" />
                               
                            </td>--%>
                             <td style="border-bottom: 1px solid #000000; border-right: 1px solid #000000;" align="center"
     valign="middle" rowspan="2" width="18%">
     <div class="DivImage">

         <asp:Image ID="ImgApplicantPhoto" CssClass="PhotoImage" ImageUrl="../images/photo.jpg"
             runat="server" Height="100px" Width="97px" />
         <br />
         <asp:Image ID="ImgApplicantSign" CssClass="PhotoImage" ImageUrl="../images/photo.jpg"
             runat="server" Height="30px" Width="100%" />
         <img id="imgPhotoBarcode" runat="server" class="BarCodeImage" />

     </div>
 </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; padding:10px;"
                                align="center">
                                <asp:Label ID="LblregDate" runat="server" Font-Size="Larger"></asp:Label>
                            </td>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; padding:10px;"
                                align="center">
                                <asp:Label ID="LblValidityDate" runat="server" Font-Size="Larger"></asp:Label>
                            </td>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; padding:10px;"
                                valign="middle">
                                <table cellpadding="0" cellspacing="0" style="height: 100%;" width="100%">
                                  
                                    <tr>
                                        <td align="left" style="border-bottom: 0px; border-left: 0px; border-right: 1px solid #000000;"
                                            valign="middle">&nbsp;&nbsp;Registration Type
                                        </td>
                                        <td style="border-bottom: 0px; width: 50%;" align="left">&nbsp;<asp:Label ID="lblRegistrationType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
          
            <tr>
                <td align="left" valign="top">
                    <table style="width: 100%;" class="preview" border="0" cellspacing="0" cellpadding="2">
                        <tr class="head1">
                            <td align="left" width="35%">1. Registration Details / पंजीयन का विवरण
                            </td>
                            <td align="left" width="15%"></td>
                            <td align="left" width="25%">&nbsp;
                            </td>
                            <td align="left" width="25%">&nbsp;
                            </td>
                        </tr>
                      <%--  <tr class="normal">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Registration for Course / पाठ्यक्रम के लिए पंजीयन"></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblCourse" runat="server"></asp:Label>
                            </td>
                        </tr>--%>
                        <%-- <tr class="normal">
                        <td rowspan="2" id="Tdundergoing" runat="server">
                            <asp:Label ID="Label89" runat="server" Text="Whether Already Registered With NIELIT, IF YES, Level"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Lblundergoing" Text="" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblAcc" runat="server" Text=" Accreditation no. of the institute / प्रत्यायन संख्या"></asp:Label>
                            <asp:Label ID="LblExp" runat="server" Text="Experience in years / वर्षों का अनुभव"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblAccNo" Text="" runat="server"></asp:Label><br />
                            <asp:Label ID="LblExperience" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                        <%--  <tr class="normal" id="TrLastCenterInstiName" runat="server">
                        <td>
                            <asp:Label ID="Label62" runat="server" Text="Name of the institute / संस्थान का नाम"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblInstitute" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                        <tr class="normal" id="TrPreRegLevel2" runat="server">
                            <%--<td rowspan="2" id="TdPreRegLevel2" runat="server">
                                <asp:Label ID="Label58" runat="server" Text="Whether Already Registered With NIELIT, IF YES, Level."></asp:Label>
                            </td>
                            <td rowspan="2" id="Tdpreregister" runat="server">
                                <asp:Label ID="Lblpreregister" Text="" runat="server"></asp:Label>
                            </td>--%>
                             <td rowspan="2">
     <asp:Label ID="lblRegFCourse" runat="server" Text="Registration for Course / पाठ्यक्रम के लिए पंजीयन"></asp:Label>
 </td>
 <td rowspan="2">
     <asp:Label ID="LblCourse" runat="server"></asp:Label>
 </td>
                            <%-- <td>
                            <asp:Label ID="Lblprecourse" runat="server" Text=" Course /  पाठ्यक्रम"></asp:Label>
                        </td>--%>
                            <td class="rightBorder">
                                <asp:Label ID="LblPreDoeaccCourse" runat="server"></asp:Label>
                            </td>
                        </tr>
                          <tr class="normal" id="Tr2" runat="server">
      <td>
          <asp:Label ID="Label14" runat="server" Text="Registration Number / पंजीयन संख्या"></asp:Label>
      </td>
      <td class="rightBorder">
          <asp:Label ID="lblRegistrationNo" runat="server"></asp:Label>
      </td>
  </tr>
                                                                       <tr class="normal" id="trreupappliname" runat="server" visible="false">                           
                            <td rowspan="2">
    <asp:Label ID="Label13" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम"></asp:Label>
</td>
<td rowspan="2">   
     <asp:Label ID="lblreupappliname" runat="server"></asp:Label>
</td>
                         
                           <td class="rightBorder">
                               <asp:Label ID="Label16" runat="server"></asp:Label>
                           </td>
                       </tr>
                         <tr class="normal" id="trreupapplidob" runat="server" visible="false">
     <td>
         <asp:Label ID="Label20" runat="server" Text="Date of Birth / जन्म दिनांक  (dd/mm/yyyy)"></asp:Label> 
     </td>
     <td class="rightBorder">        
         <asp:Label ID="lblreupapplidob" runat="server"></asp:Label>
     </td>
 </tr>
                        <tr class="normal" id="TrPreRegLevel1" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label68" runat="server" Text="Exam Cycle / परीक्षा चक्र &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                 <br />
 (Subject to Change *)
                            </td>
                            <td class="rightBorder" colspan="3">
                              <asp:Label ID="LblExamName" runat="server" Width="375px" BorderStyle="Solid"
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
                        <%--  <tr class="normal">
      <td>
          <asp:Label ID="Label10" runat="server" Text="Registration for Course / पाठ्यक्रम के लिए पंजीयन"></asp:Label>
      </td>
      <td class="rightBorder" colspan="3">
          <asp:Label ID="Label13" runat="server"></asp:Label>
      </td>
  </tr>--%>
                        <tr class="head1" id="trapplidet" runat="server" visible="false">
                            <td colspan="2">2.
                            <asp:Label ID="Label69" runat="server" Text="Applicant's Details / आवेदक का विवरण"></asp:Label>
                            </td>
                            <td></td>
                            <td class="rightBorder"></td>
                        </tr>
                        <tr class="normal" id="trapplifn" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label70" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम"></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblAppName" runat="server"></asp:Label>

                            </td>
                        </tr>
                        <tr class="normal" id="TrFatherName" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label26" runat="server" Text="Father's Name / पिता का नाम "></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblFName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrMotherName" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label27" runat="server" Text="Mother's Name / माता का नाम "></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblMName" runat="server"></asp:Label>
                            </td>
                        </tr>

                          <tr class="normal" id="trguardian" runat="server" visible="false">
     <%-- <td width="3%">2.2.1
      </td>--%>
      <td class="labelCell">
          <asp:Label ID="LblGuardian" runat="server" Text="Guardian's Name / संरक्षक का नाम ">
          </asp:Label>
      </td>
      <td class="rightBorder" colspan="3">
          <asp:TextBox ID="LblGuardianName" runat="server" MaxLength="60" TabIndex="9" Width="529px"
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

                        <tr class="normal" id="trgender" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label4" runat="server" Text="Gender / लिंग"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblGender" runat="server"></asp:Label>
                            </td>
                            <td class="labelCell">
                                <asp:Label ID="Label5" runat="server" Text="Marital Status / वैवाहिक स्थिति"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblMaritalStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal"  id="trdobct" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक  (dd/mm/yyyy)"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblDob" runat="server"></asp:Label>
                            </td>
                            <td class="labelCell">
                                <asp:Label ID="Label7" runat="server" Text="Category / वर्ग"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblCategory" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="trctsr" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label71" runat="server" Text="Handicapped / विकलांग"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblHandicapped" runat="server"></asp:Label>
                            </td>
                            <td class="labelCell">
                                <asp:Label ID="Label72" runat="server" Text="Ex-Serviceman / पूर्व सेवाकर्मी"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblExService" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal"  id="trrelg" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label73" runat="server" Text="Religion / धर्म"></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblReligion" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1" id="trcnt" runat="server" visible="false">
                            <td colspan="2">3. Contact Details / संपर्क विवरण
                            </td>
                             <td colspan="2" style="text-align: right;">
      <asp:Label ID="lblContactUpdateInfo" runat="server"
          Text="⚠️ Updation in Mobile Number and Email can be done through the following link: ">
      </asp:Label>

      <asp:HyperLink ID="lnkContactUpdate" runat="server"
          NavigateUrl="~/Cand/CandContactUpdate.aspx"
          Text="Update Contact Details" />
  </td>
                        </tr>
                        <tr class="normal" id="trphmob" runat="server" visible="false">
                            <td class="labelCell">
                                <asp:Label ID="Label18" runat="server" Text="Phone with STD code / दूरभाष एस टी डी कोड सहित "></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="LblSTD" runat="server"></asp:Label>
    -
    <asp:Label ID="LblPhone" runat="server"></asp:Label>
    <br />
                               <%-- <asp:Label ID="LblLandLine" runat="server"></asp:Label>
                                <br />--%>
                            </td>
                            <td class="labelCell" style="background-color:#8fbcdb">
                                <asp:Label ID="Label40" runat="server" Text="Mobile Number / मोबाइल नंबर" Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td class="rightBorder" style="background-color:#8fbcdb">
                                <asp:Label ID="lblMobile" runat="server"  oncopy="return false" 
                                    onkeypress="checkNumber(this,10,0,event);" oninput="this.value=this.value.replace(/[^0-9]/g,'');" onpaste="return false"
                                    TabIndex="24" Width="437px"></asp:Label>
                                 <%--<asp:Label runat="server" ID="lblmobile" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr class="normal" id="tremail" runat="server" visible="false">
                            <td style="background-color:#8fbcdb">
                                <asp:Label ID="Label21" runat="server" Text="Email Address / ईमेल पता " Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td class="rightBorder" colspan="3" style="background-color:#8fbcdb">
                                <asp:Label ID="lblEmail" runat="server" CssClass="addressBox" Width="100%"></asp:Label>
                                 <%--<asp:Label runat="server" ID="lblEmail" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                                <br />
                            </td>
                        </tr>
                        <tr class="head1" id="trper" runat="server" visible="false">
                            <td colspan="2">4. Permanent Address Details / स्थायी पता विवरण
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="normal" id="trperadd1" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label83" runat="server" Text="Address Line1/पता पंक्ति 1 "></asp:Label>
                               
                            </td>
                            <td colspan="3" class="rightBorder">
                                <asp:Label ID="lblPerAddressLine1" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="trperadd2" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label96" runat="server" Text="Address Line2/पता पंक्ति 2 "></asp:Label>
                               
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="lblPerAddressLine2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="trperadd3" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label85" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                                
                            </td>
                            <td colspan="3" class="rightBorder">
                                <asp:Label ID="lblPerAddressLine3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="trpercity" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label97" runat="server" Text="City Name/शहर का नाम"></asp:Label>

                            </td>
                            <td>
                                <asp:Label ID="lblPerCity" runat="server" Enabled="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label78" runat="server" Text="District / जिला">
                                </asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlPdistrict" runat="server" Enabled="false" TabIndex="31" Width="449px">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlPState" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        
                        <tr class="normal" id="trperstate" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label79" runat="server" Text="State / राज्य "></asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPState" runat="server" Enabled="false" AutoPostBack="True" OnSelectedIndexChanged="ddlPState_SelectedIndexChanged"
                                    TabIndex="30" Width="375px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Pin Code / पिन  कोड  "></asp:Label>
                                
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblPerPinCode" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1" id="trpercorr" runat="server" visible="false">
                            <td colspan="2">5.
                            <asp:Label ID="Label74" runat="server" Text="Correspondence Details / पत्राचार की सूचना"></asp:Label>
                            </td>
                           <td colspan="2" style="text-align: right;">
     <asp:Label ID="Label2" runat="server"
         Text="⚠️ Updation in Correspondence Address can be done through the following link: ">
     </asp:Label>

     <asp:HyperLink ID="hypcandcontact" runat="server"
         NavigateUrl="~/Cand/CandidateUpdateRequest.aspx"
         Text="Update Address Details" />
 </td>
                        </tr>
                        <tr class="normal" id="trcorradd1" runat="server" visible="false">
                            <td class="labelCell" style="background-color:#8fbcdb">
                                <asp:Label ID="Label98" runat="server" Text="Address Line1/पता पंक्ति 1 " Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>

                            </td>
                            <td colspan="3" class="rightBorder" style="background-color:#8fbcdb">
                                <asp:Label ID="lblCorAddressLine1" runat="server" CssClass="addressBox"  Width="99%"></asp:Label>
                                <%--<asp:Label runat="server" ID="lblcoraddline1" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>

                            </td>
                        </tr>
                        <tr class="normal" id="trcorradd2" runat="server" visible="false">
                            <td class="labelCell" style="background-color:#8fbcdb">
                                <asp:Label ID="Label99" runat="server" Text="Address Line2/पता पंक्ति 2 " Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td colspan="3" class="rightBorder" style="background-color:#8fbcdb">
                                <asp:Label ID="lblCorAddressLine2" runat="server" CssClass="addressBox" Width="99%" ></asp:Label>
                                <%--<asp:Label runat="server" ID="lblcoraddline2" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr class="normal" id="trcorradd3" runat="server" visible="false">
                            <td class="labelCell" style="background-color:#8fbcdb">
                                <asp:Label ID="Label100" runat="server" Text="Address Line3/पता पंक्ति 3" Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td colspan="3" class="rightBorder" style="background-color:#8fbcdb">
                                <asp:Label ID="lblCorAddressLine3" runat="server" CssClass="addressBox" Width="99%" ></asp:Label>
                                <%--<asp:Label runat="server" ID="lblcoraddline3" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr class="normal" id="trcorrcity" runat="server" visible="false">
                            <td class="labelCell" style="background-color: #8fbcdb">
                                <asp:Label ID="Label101" runat="server" Text="City Name/शहर का नाम" Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td style="background-color:#8fbcdb">
                                <asp:Label ID="lblCorCity" runat="server" CssClass="addressBox" Width="97%" ></asp:Label>
                                <%--<asp:Label runat="server" ID="lblcorcity" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                            </td>
                            <td style="background-color: #8fbcdb">
                                <asp:Label ID="Label34" runat="server" Font-Bold="true" Text="District / जिला  <font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td class="rightBorder" style="background-color:#8fbcdb">
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddldistrict" runat="server" TabIndex="39" Width="449px" Enabled="false">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCorState" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="normal" id="trcorrstate" runat="server" visible="false">
                            <td style="background-color:#8fbcdb">
                                <asp:Label ID="Label33" runat="server" Text="State / राज्य " Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td style="background-color:#8fbcdb">

                                <%--<asp:textbox ID="txtState" runat="server"></asp:textbox>--%>
                                <asp:DropDownList ID="ddlCorState" runat="server" Enabled="false"  AutoPostBack="True"  OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
                                    TabIndex="38" Width="375px">
                                </asp:DropDownList>

                            </td>

                            <td style="background-color:#8fbcdb">
                                <asp:Label ID="Label39" runat="server" Text="Pin Code / पिन  कोड  "  Font-Bold="true"></asp:Label>
                                <font color='RED'>*</font>
                            </td>
                            <td class="rightBorder" style="background-color:#8fbcdb">
                                <asp:Label ID="lblPincode" runat="server" oncopy="return false" oncut="return false"  
                                    onkeypress="checkNumber(this,6,0,event);" oninput="this.value=this.value.replace(/[^0-9]/g,'');" onpaste="return false" TabIndex="32"
                                    Width="437px" MaxLength="6"></asp:Label>
                                <%--<asp:Label runat="server" ID="lblPincode" Visible="False" Font-Bold="False" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                            </td>
                        </tr>
                          <tr id="TrApplicantType" runat="server" visible="false">
     
      <td class="labelCell">
          <asp:Label ID="Label89" runat="server" Font-Bold="true" Text="Applied As / किसके रूप में आवेदन किया<font color='RED'>*</font>">
          </asp:Label>
      </td>
      <td style="border:1px solid #000; padding:4px;">
          <asp:RadioButtonList ID="RdoUndergngDOEACC" runat="server" RepeatDirection="Horizontal"
              TabIndex="2"  AutoPostBack="True" OnSelectedIndexChanged="RdoUndergngDOEACC_SelectedIndexChanged">
              <asp:ListItem Value="D" Selected="True">Direct Candidate</asp:ListItem>
              <asp:ListItem Value="I">Through Institute</asp:ListItem>
          </asp:RadioButtonList>
          <asp:Label ID="lblApplicantType" runat="server" Visible="False"></asp:Label>
      </td>
                                                            <td colspan="2" style="text-align: right;">
    <asp:Label ID="Label1" runat="server" Font-Bold="true"
        Text="⚠️ Updation for Direct to Institute or vice-versa can be done through the following link: ">
    </asp:Label>

    <asp:HyperLink ID="hypcandupdatereg" runat="server"
        NavigateUrl="~/Cand/CandidateUpdateRegistrationType.aspx"
        Text="Update Institute/Direct Candidate Details" />
</td>
  </tr>
                        <tr class="head1" id="TrInstitutedetails" runat="server" visible="false">
                            <td colspan="2">
         <%--<asp:Label ID="Label2" runat="server" Text="Institute Details "></asp:Label>--%>
                                 <asp:Label ID="LblSectionInstituteNo" runat="server"></asp:Label>
                                 <asp:Label ID="Label10" runat="server" Text="Institute Details / संस्थान का विवरण"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <tr class="normal" id="TrStateCenterAccno" runat="server" visible="false">
                                    <td valign="top">
                                        <asp:Label ID="Lbl2" runat="server"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="Label9" runat="server" Text="Select State of Accredited Institute / मान्यता प्राप्त संस्थान के राज्य का चयन करें <font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>

                                    <td align="left" style="width: 3%" colspan="2" class="rightBorder">
                                        <asp:DropDownList ID="DdlAccState" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlAccState_SelectedIndexChanged" TabIndex="3">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>

                                <tr class="normal" id="TrCenterNameAccno" runat="server" visible="false">
                                    <td align="left" style="width: 3%" valign="top">
                                        <asp:Label ID="Lbl3" runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label12" runat="server" Text="Select Centre Name of Accredited Institute / मान्यता प्राप्त संस्थान के केंद्र के नाम का चयन करें<font color='RED'>*</font>">
                                        </asp:Label>
                                    </td>
                                    <td align="left" style="width: 3%" colspan="2" class="rightBorder">
                                        <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>

                                        <asp:DropDownList ID="DdlAccCentre" runat="server" Width="100%" AutoPostBack="True"
                                            TabIndex="5">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- </ContentTemplate>--%>
                                        <%-- <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                </Triggers>--%>
           
                                    </td>
                                </tr>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <tr class="head1" id="trIDenti" runat="server" visible="false">
                            <td colspan="2">
        <%--<asp:Label ID="Label3" runat="server" Text="Identification Details / पहचान की सूचना"></asp:Label>--%>
                            <asp:Label ID="LblSectionIdentificationNo"
            runat="server"></asp:Label>
        Identification Details / पहचान की सूचना
                                </td>
                            <td></td>
                            <td></td>
                        </tr>

                        <%-- For ApaarId --%>
                                   <%-- <tr id="trapaar" runat="server" >
<td colspan="4">
                         <table  width="100%" border="0" cellspacing="0" cellpadding="2">--%>
                        <tbody id="trapaar" runat="server" visible="false">
                        <tr  class="normal" >
                            <td valign="top"> <asp:Label ID="LblIdentification1" runat="server"></asp:Label></td>
                            <td valign="top">
                                <asp:Label ID="Label8" runat="server" Text="Apaar&nbsp; ID / अपार आईडी  <font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td align="left" style="width:3%" colspan="2" class="rightBorder" runat="server" id="tdapaartext">

                                <asp:TextBox ID="txtapaar" runat="server" MaxLength="12" TabIndex="49"  AutoPostBack="true" Width="610px"
                                    onpaste="return false;" onkeypress="return AllowOnlyNumbers(event);"  oninput="this.value=this.value.replace(/[^0-9]/g,'');" oncopy="return false;" oncut="return false;" TextChanged="txtapaar_TextChanged" OnTextChanged="txtapaar_TextChanged">
                                </asp:TextBox>
                                <br/>
                                 <%-- </td>
                                  <td class="rightBorder">--%>
        <a href="#" onclick="window.open('https://www.abc.gov.in');return false;">
            Click here to generate Apaar Id
        </a>
    </td>
                                <%--<a href="#" onclick="window.open('https://www.abc.gov.in')">Click here to generate Apaar Id</a>--%>

                          
                        </tr>
                    

                        <tr class="normal" id="TrProviderPresent" runat="server">
    <td valign="top"> <asp:Label ID="LblIdentification11" runat="server"></asp:Label></td>

    <td valign="top">
        <asp:Label ID="lblProviderPresent" runat="server"
            Text="Is Provider Present / क्या प्रदाता उपस्थित है ">
        </asp:Label>
        <font color="red">*</font>
    </td>

    <td align="left" style="width:3%" colspan="2" class="rightBorder">
        <asp:TextBox ID="txtIsProviderPresent" runat="server" Width="610px"
            Text="True" ReadOnly="true">
        </asp:TextBox>
    </td>
                             
</tr>

                         <tr class="normal" id="TrConsentRelation" runat="server">
     <td valign="top"> <asp:Label ID="LblIdentification12" runat="server"></asp:Label></td>
     <td valign="top">
         <asp:Label ID="lblConsentRelation" runat="server"
             Text="Consent Relation / सहमति संबंध <font color='RED'>*</font>">
         </asp:Label>
     </td>
     <td align="left" style="width:3%" colspan="2" class="rightBorder">
         <asp:DropDownList ID="ddlConsentRelation" runat="server"
              AutoPostBack="true" Width="100%"
                 OnSelectedIndexChanged="ddlConsentRelation_SelectedIndexChanged">

             <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
             <asp:ListItem Text="Self" Value="1"></asp:ListItem>
             <asp:ListItem Text="Guardian" Value="2"></asp:ListItem>
             <asp:ListItem Text="Father" Value="3"></asp:ListItem>
             <asp:ListItem Text="Mother" Value="4"></asp:ListItem>

         </asp:DropDownList>
     </td>
                            
 </tr>

                        <tr class="normal" id="Tr1" runat="server">
      <td valign="top"><asp:Label ID="LblIdentification13" runat="server"></asp:Label></td>

      <td valign="top">
          <asp:Label ID="Label17" runat="server"
              Text="Provider Name / प्रदाता का नाम <font color='RED'>*</font>">
          </asp:Label>
      </td>

      <td align="left" style="width:3%" colspan="2" class="rightBorder">
          <asp:TextBox ID="txtproviderName" runat="server" Width="610px"
              MaxLength="100" >
          </asp:TextBox>
      </td>
                           
  </tr>
                         <tr class="normal" id="TrAuthMode" runat="server">
       <td valign="top"><asp:Label ID="LblIdentification14" runat="server"></asp:Label></td>
       <td valign="top">
           <asp:Label ID="lblAuthMode" runat="server"
               Text="Authentication Mode / प्रमाणीकरण मोड <font color='RED'>*</font>">
           </asp:Label>
       </td>
       <td align="left" style="width:3%" colspan="2" class="rightBorder">
           <asp:DropDownList ID="ddlAuthMode" runat="server" Width="100%"
               AutoPostBack="true" OnSelectedIndexChanged="ddlAuthMode_SelectedIndexChanged">
               <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
           </asp:DropDownList>
       </td>
                            
   </tr>
                         <tr class="normal" id="TrAuthId" runat="server">
       <td valign="top"><asp:Label ID="LblIdentification15" runat="server"></asp:Label></td>
       <td valign="top">
           <asp:Label ID="lblAuthId" runat="server"
               Text="Authentication ID No / प्रमाणीकरण आईडी संख्या <font color='RED'>*</font>">
           </asp:Label>
       </td>
       <td align="left" style="width:3%" colspan="2" class="rightBorder">
           <asp:TextBox ID="txtAuthenticationIdNo" runat="server"
               MaxLength="50" Width="610px"
               AutoPostBack="true"
                OnTextChanged="txtAuthenticationIdNo_TextChanged"
               onkeypress="return isNumberKey(event);"
               onpaste="return false;" oncopy="return false;" oncut="return false;">
           </asp:TextBox>
       </td>
                            
   </tr>
                        <tr class="normal" id="TrConsentDate" runat="server">
      <td valign="top"> <asp:Label ID="LblIdentification16" runat="server"></asp:Label></td>
      <td valign="top">
          <asp:Label ID="lblConsentDate" runat="server"
              Text="Consent Date / सहमति दिनांक <font color='RED'>*</font>">
          </asp:Label>
      </td>
      <td align="left" style="width:3%" colspan="2" class="rightBorder">
          <asp:TextBox ID="txtConsentDate" runat="server" Width="610px"
                ReadOnly="true">
          </asp:TextBox>
      </td>
                           
  </tr>

                         <tr class="normal" id="TrConsentTime" runat="server">
      <td valign="top"><asp:Label ID="LblIdentification17" runat="server"></asp:Label></td>
      <td valign="top">
          <asp:Label ID="lblConsentTime" runat="server"
              Text="Consent Time / सहमति समय <font color='RED'>*</font>">
          </asp:Label>
      </td>
      <td align="left" style="width:3%" colspan="2" class="rightBorder">
          <asp:TextBox ID="txtConsentTime" runat="server" Width="610px"
               TextMode="Time" ReadOnly="true">
          </asp:TextBox>
      </td>
                            
  </tr>

                        <tr class="normal" id="TrConsentPlace" runat="server">
      <td valign="top"><asp:Label ID="LblIdentification18" runat="server"></asp:Label></td>
      <td valign="top">
          <asp:Label ID="lblConsentPlace" runat="server"
              Text="Consent Place / सहमति स्थान <font color='RED'>*</font>">
          </asp:Label>
      </td>
      <td align="left" style="width:3%" colspan="2" class="rightBorder">
          <asp:TextBox ID="txtConsentPlace" runat="server" Width="610px"
              MaxLength="100">
          </asp:TextBox>
      </td>
                           
  </tr>

<%--                         <tr class="normal">
     <td>
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
 </tr>--%>
                            <tr class="normal">
    <td colspan="4" class="rightBorder">

        <asp:CheckBox ID="chkApaarDeclaration"
            runat="server"
            TabIndex="53" />

        <font color="red">*</font>

        <asp:Label ID="lblApaarDeclaration"
            runat="server">
        </asp:Label>

    </td>
</tr>
                            </tbody>
      <%-- </table>  
    </td>
                                        </tr>--%>
                        <%-- added for apaar validation end--%>

                     <%--   <tr class="normal" id="tridentif2" runat="server" visible="false">
                            <td align="left" style="width: 3%" valign="top">
                                 <asp:Label ID="LblIdentification2" runat="server"></asp:Label>
                            </td>
                            <td align="left" valign="top">
                              
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
                            <td style="width: 3%" colspan="2" class="rightBorder">
                                 <div style="display:flex;
                justify-content:space-between;
                align-items:flex-start;">
                            
                                <asp:Image runat="server" ID="photoPreview" alt="Photo" Width="110" Height="130"></asp:Image>
                                <asp:Label runat="server" ID="lblphotoShow" Visible="False" Font-Bold="False" Font-Size="10pt"
                                    ForeColor="Red"></asp:Label>

                                <asp:Button ID="btnNotification" runat="server" Text="Help/Tip to Resize Image" TabIndex="43" Width="157px" OnClick="btnNotification_Click1" />
                                     </div>
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
                                 
                                </div>
                                </div>

                            </td>
                        </tr>--%>

<%--                        <tr class="normal" id="tridentf3" runat="server" visible="false">
                            <td height="80">
                                 <asp:Label ID="LblIdentification3" runat="server"></asp:Label>
                            </td>
                            <td>
                             
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
                            <td style="width: 3%" colspan="2" class="rightBorder">
                                <asp:Image runat="server" ID="signPreview" alt="Signature" Width="200" Height="80"></asp:Image>
                                <asp:Label runat="server" Text="" ID="lblsignShow" Visible="false" Font-Bold="False"
                                    Font-Size="10pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr class="head1" id="trsecdoc" runat="server" visible="false">

                            <td colspan="3">
                                 <asp:Label ID="LblSectionDocumentsNo"
            runat="server">
        </asp:Label>
         <asp:Label ID="lblDocuments" runat="server" Text="Documents / दस्तावेज़" Font-Bold="true">
         </asp:Label>
                                <asp:Label runat="server" Text="" ID="lbldocument" EnableTheming="false" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                       <%-- <tr class="normal" id="trsecdoc1" runat="server" visible="false">
                            <td style="width: 5%;">
                                <asp:Label ID="lblDocNo1" runat="server" Text="8.1"></asp:Label>
                            </td>
                           
                            <td colspan="2" style="width: 65%;">
                                <asp:Label ID="lblEduquacertiForm" runat="server"
                                    Text="Upload Highest Education Form (PDF only) <font color='RED'>*</font> :"></asp:Label>

                            </td>
                            <td class="rightBorder" style="border: 1px solid black;">

                                <asp:FileUpload ID="fuEduquacertiForm" runat="server" onchange="clearEduError();" />
                                <asp:Label ID="lblEduStatus" runat="server" ClientIDMode="Static" />

                            </td>
                        </tr>--%>

                       <%-- <tr class="normal" id="trexpcerti" runat="server" visible="false">
                            <td><asp:Label ID="lblDocNo2" runat="server" Text="8.2"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblexpcertiForm" runat="server"
                                    Text="Upload Experience Certificate (PDF only)  <font color='RED'>*</font> :"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:FileUpload ID="fuexpcertiForm" runat="server" onchange="clearExpCertiError();" />
                                <asp:Label ID="lblExpCertiFormStatus" runat="server" ClientIDMode="Static" />
                            </td>

                        </tr>--%>
                        <tr class="normal"  id="trsecdoc1" runat="server" visible="false">
                            <td style="width: 5%;">
                                <asp:Label ID="lblDocNo1" runat="server" Text="8.1"></asp:Label>
                            </td>
                            <td colspan="2" style="width: 65%;">
                                <asp:Label ID="lblmedical" runat="server"
                                    Text="Upload PROOF OF MEDICAL ISSUE/AFFIDAVIT(PDF only)  <font color='RED'>*</font> :"></asp:Label>
                            </td>
                            <td class="rightBorder" style="border: 1px solid black;">
                                <asp:FileUpload ID="Fumedical" runat="server" onchange="clearMedicalStatusError();" />
                                <asp:Label ID="lblMedicalStatus" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>
                        <%-- ID Card --%>
                       <%--  <tr class="normal" id="trsecdoc4" runat="server" visible="false">
     <td>
         <asp:Label ID="lblDocNo4" runat="server" Text="8.4"></asp:Label>
     </td>
     <td colspan="2">
         <asp:Label ID="lblIdCard" runat="server"
             Text="Upload ID Card(PDF only)  <font color='RED'>*</font> :"></asp:Label>
     </td>
     <td class="rightBorder">
         <asp:FileUpload ID="FuIdCard" runat="server" onchange="clearIdCardStatusError();" />
         <asp:Label ID="lblIDCardStatus" runat="server" ClientIDMode="Static" />
     </td>
 </tr>--%>

                        <%--   <tr class="normal">
    <td style="width:5%;">8.1</td>

    <td colspan="2" style="width:65%;">
        <asp:Label ID="Label8" runat="server" 
            Text="Upload Education Form (PDF only):"></asp:Label>
    </td>

    <td style="width:30%;" class="rightBorder">
        <asp:FileUpload ID="FileUpload1" runat="server" />
    </td>
</tr>

<tr class="normal">
    <td>8.2</td>

    <td colspan="2">
        <asp:Label ID="Label10" runat="server" 
            Text="Upload Experience Certificate (PDF only):"></asp:Label>
    </td>

    <td class="rightBorder">
        <asp:FileUpload ID="FileUpload2" runat="server" />
    </td>
</tr>

<tr class="normal">
    <td>8.3</td>

    <td colspan="2">
        <asp:Label ID="Label13" runat="server" 
            Text="Upload proof of medical issue/affidavit stating reason for failure to complete course (PDF only):">
        </asp:Label>
    </td>

    <td class="rightBorder">
        <asp:FileUpload ID="FileUpload3" runat="server" />
    </td>
</tr> --%>
                    </table>
                </td>
            </tr>
             <tr class="gdalternate1">
     <td align="left" valign="top" colspan="3">
         <table style="width: 100%;">
             <tr>
                 <td></td>
                 <td valign="top">
                     <asp:CheckBox ID="chkdisclamier" runat="server" Visible="false" TabIndex="52" />
                 </td>
                 <td id="tddeclaration1" runat="server" style="display: none; text-align: justify;">
                     <font color='RED'>*</font><asp:Label ID="lblDeclar" runat="server" Text="I hereby declare that all the information submitted by me in this Online Registration Application Form is true and correct to the best of my knowledge and belief. I agree to abide by all the rules and regulations of NIELIT and accept the decision of the Registration Authority regarding my eligibility for registration at the desired level.

I understand and acknowledge that NIELIT and the Registration Authority reserve the right to withhold, reject, or cancel my registration application or allotted Registration Number, and to take any other action deemed appropriate, if any information or declaration furnished by me is found to be incorrect, false, fake, misleading, or illegal at any point of time. I shall be solely responsible for any financial, social, or legal consequences arising from such incorrect or misleading information.

I further understand and agree that the fee paid by me for this registration process is **non-refundable**, and I shall not be entitled to claim a refund of the fee under any circumstances.
/मैं एतद्द्वारा घोषणा करता/करती हूँ कि मेरे द्वारा इस ऑनलाइन पंजीकरण आवेदन प्रपत्र में प्रस्तुत की गई सभी सूचनाएँ मेरे ज्ञान एवं विश्वास के अनुसार सत्य एवं सही हैं। मैं नाइलिट के सभी नियमों एवं विनियमों का पालन करने तथा वांछित स्तर पर पंजीकरण हेतु मेरी पात्रता के संबंध में पंजीकरण प्राधिकारी द्वारा लिए गए निर्णय को स्वीकार करने के लिए सहमत हूँ।

मैं समझता/समझती हूँ कि यदि मेरे द्वारा प्रस्तुत की गई कोई भी सूचना अथवा घोषणा किसी भी समय गलत, असत्य, जाली, भ्रामक या अवैध पाई जाती है, तो नाइलिट एवं पंजीकरण प्राधिकारी को मेरे पंजीकरण आवेदन को रोकने, अस्वीकार करने अथवा आवंटित पंजीकरण संख्या को रद्द करने तथा उचित समझी जाने वाली अन्य कार्रवाई करने का अधिकार होगा। ऐसी गलत अथवा भ्रामक सूचना के कारण होने वाले किसी भी वित्तीय, सामाजिक या वैधानिक परिणाम के लिए मैं स्वयं पूर्ण रूप से उत्तरदायी रहूँगा/रहूँगी।

मैं यह भी समझता/समझती हूँ और सहमत हूँ कि इस पंजीकरण प्रक्रिया हेतु मेरे द्वारा जमा की गई फीस **अप्रतिदेय (Non-Refundable)** है तथा किसी भी परिस्थिति में मुझे उक्त फीस की वापसी का दावा करने का अधिकार नहीं होगा।"></asp:Label>
                     <br />
                 </td>               
                 <td></td>
             </tr>
         </table>
     </td>
 </tr>

        </table>
        <div style="text-align: center; margin-top: 10px;" id="divfooter" runat="server">
            <asp:Button ID="Btnsubmit" runat="server" Text="Submit" Width="100px" Visible="false" OnClick="Btnsubmit_Click" />
            <asp:Button ID="Btnupdate" runat="server" Text="Update" Width="100px" Visible="false" OnClick="Btnupdate_Click" />
            <%--<asp:Button ID="btnSave" runat="server" Text="Proceed" TabIndex="53" OnClick="btnSave_Click"/>--%>
            <%--<asp:Button ID="btnUpload" runat="server" Text="Upload Documents" />--%>
            <%--<asp:Button ID="Btnback" runat="server" Text="Back" Width="50px" />--%>
        </div>
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:ModalPopupExtender ID="AlertModalPopUp" runat="server" PopupControlID="PopUpPanel"
            PopupDragHandleControlID="PopupHeader" TargetControlID="HiddenField2" OkControlID="CancleBtn"
            X="300" Y="300">
        </asp:ModalPopupExtender>
        <br />
        <%-- <asp:HiddenField ID="courseid" runat="server" />
    <asp:HiddenField ID="appid" runat="server" />--%>
        <asp:HiddenField ID="hdnCandidateID" Value="" runat="server" />
        <asp:HiddenField ID="hdnMaritalStatusID" runat="server" />
<asp:HiddenField ID="hdnCategoryID" runat="server" />
<asp:HiddenField ID="hdnExServiceManID" runat="server" />
<asp:HiddenField ID="hdnIsHandicapedID" runat="server" />
<asp:HiddenField ID="hdnReligionID" runat="server" />
    </form>
</body>
</html>
