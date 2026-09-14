<%@ Page Title="Special Extension Preview Form" Language="C#" AutoEventWireup="true"  CodeFile="SpecialExtensionPreview.aspx.cs"
    Inherits="SpecialExtensionPreview" Debug="true" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
     
    <style type="text/css">
        .DivImage
        {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 168px;
        }
        .InnerTable
        {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 100%;
            height: 100%;
        }
        .PhotoImage
        {
            position: relative;
            right: 3px;
            top: 5px;
            z-index: -1;
        }
        .BarCodeImage
        {
            position: relative;
            right: 5px;
            top: -1px;
            z-index: 0;
        }

       /* .normal td {
    padding: 8px;
    vertical-align: middle;
}

input[type="file"] {
    width: 95%;
}*/
    </style>
     <script src="../Script/jquery-3.7.1.min.js" type="text/javascript"></script>
<%-- <script type="text/javascript">
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


     function ValidateForm() {
         if (!isBlank("ImgUpload", " Upload Image"))
             return false;
         if (!isvalidImageFile("ImgUpload", "Photo"))
             return false;
         if (!isBlank("ImgUploadSignature", "Upload Signature"))
             return false;
         if (!isvalidImageFile("ImgUploadSignature", "Signature"))
             return false;
     }
 </script>--%>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server">
 </asp:ScriptManager>   
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" id="Tblpreview"
        runat="server">
        <tr>
            <td>
                <uc1:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
                    <tr>
    <td align="center">
        <strong style="text-align: center" class="headfont">SPECIAL EXTENSION FORM  
     </strong>
    </td>
</tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr style="height: 45px;">
            <td align="center" style="border-bottom: 1px solid #000000;" valign="middle">
                <asp:Label ID="Lblhead" runat="server" Style="font-size: 22px;"></asp:Label>
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 10px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
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
        <tr>
            <td align="center">
                <table style="width: 100%;" class="preview" cellpadding="1" cellspacing="0">
                    <tr class="normal" style="font: bold 18px arial;">
                        <td align="center" width="25%">
                            Application Number
                        </td>
                        <td align="center" width="25%">
                            Application Date &amp; Time
                        </td>
                        <td align="center">
                            For Office Use Only&nbsp;
                        </td>
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
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblAppNumber" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblAppDataTime" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            valign="middle">
                            <table cellpadding="0" cellspacing="0" style="height: 100%;" width="100%">
                                <tr id="TrDemandnote" runat="server">
                                    <td align="left" style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 1px solid #000000;"
                                        valign="middle">
                                        &nbsp;
                                        <asp:Label ID="Label95" runat="server" Text="Demand Note Number"></asp:Label>
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 0px;
                                        width: 50%" align="left">
                                        &nbsp;
                                        <asp:Label ID="LblDemandNoteID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="TrDemandnote1" runat="server">
                                    <td align="left" style="border-bottom: 1px solid #000000; width: 50%; border-left: 0px;
                                        border-right: 1px solid #000000;" valign="middle">
                                        &nbsp;
                                        <asp:Label ID="Label102" runat="server" Text="Demand Note Date"></asp:Label>
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; width: 50%;" align="left">
                                        &nbsp;<asp:Label ID="LblDemandNoteDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td align="left" style="border-bottom: 1px solid #000000; width: 50%; border-left: 0px;
                                        border-right: 1px solid #000000;" valign="middle">
                                        &nbsp; Registration No.
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; width: 50%;" align="left">
                                        &nbsp;<asp:Label ID="lblRegistrationNo" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="left" style="border-bottom: 0px; border-left: 0px; border-right: 1px solid #000000;"
                                        valign="middle">
                                        &nbsp;&nbsp;Batch No.
                                    </td>
                                    <td style="border-bottom: 0px; width: 50%;" align="left">
                                        &nbsp;<asp:Label ID="lblBatchNo" runat="server"></asp:Label>
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
                        <td align="left" width="35%">
                            1. Registration Details / पंजीयन का विवरण
                        </td>
                        <td align="left" width="15%">
                        </td>
                        <td align="left" width="25%">
                            &nbsp;
                        </td>
                        <td align="left" width="25%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Registration for Course / पाठ्यक्रम के लिए पंजीयन"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
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
                        <td rowspan="2" id="TdPreRegLevel2" runat="server">
                            <asp:Label ID="Label58" runat="server" Text="Applied As / किसके रूप में आवेदन किया"></asp:Label>
                        </td>
                        <td rowspan="2" id="Tdpreregister" runat="server">
                            <asp:Label ID="lblapplicantTypeName" Text="" runat="server"></asp:Label>
                        </td>
                       <%-- <td>
                            <asp:Label ID="Lblprecourse" runat="server" Text=" Course /  पाठ्यक्रम"></asp:Label>
                        </td>--%>
                        <td class="rightBorder">
                            <asp:Label ID="LblPreDoeaccCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPreRegLevel1" runat="server">
                        <td>
                            <asp:Label ID="Label68" runat="server" Text="Registration Number / पंजीयन संख्या"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="lblRegistrationNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr class="normal" id="trexam" runat="server" visible="false">
     <td>
         <asp:Label ID="Label10" runat="server" Text="Exam Cycle / परीक्षा चक्र"></asp:Label>
     </td>
     <td class="rightBorder">
         <asp:Label ID="lblExamcycle" runat="server"></asp:Label>
     </td>
                           <td>
      <asp:Label ID="Label13" runat="server" Text="Fee Amt. (Rs/-) / शुल्क राशि "></asp:Label>
  </td>
  <td class="rightBorder">
      <asp:Label ID="lblFeeDetail" runat="server"></asp:Label>
  </td>
 </tr>
                                        <tr class="normal" id="trFRegDate" runat="server">
    <td>
        <asp:Label ID="Label3" runat="server" Text="Registration Date / पंजीयन दिनांक"></asp:Label>
    </td>
    <td class="rightBorder">
        <asp:Label ID="LblregDate" runat="server"></asp:Label>
    </td>
                          <td>
     <asp:Label ID="Label16" runat="server" Text="Valid Upto Date / मान्य होने की अंतिम तिथि"></asp:Label>
 </td>
 <td class="rightBorder">
     <asp:Label ID="LblValidityDate" runat="server"></asp:Label>
 </td>
</tr>
                                                            <tr class="normal" id="trRegType" runat="server">
    <td>
        <asp:Label ID="Label14" runat="server" Text="Registration Type / पंजीयन प्रकार"></asp:Label>
    </td>
    <td class="rightBorder">
        <asp:Label ID="lblRegistrationType" runat="server"></asp:Label>
    </td>
                         <%-- <td>
     <asp:Label ID="Label22" runat="server" Text="Valid Upto Date :"></asp:Label>
 </td>
 <td class="rightBorder">
     <asp:Label ID="Label24" runat="server"></asp:Label>
 </td>--%>
</tr>

                    <tr class="head1">
                        <td colspan="2">
                            2.
                            <asp:Label ID="Label69" runat="server" Text="Applicant's Details / आवेदक का विवरण"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label70" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम"></asp:Label>
                             
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblAppName" runat="server"></asp:Label>
                           
                        </td>
                    </tr>
                    <tr class="normal" id="TrFatherName" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label26" runat="server" Text="Father's Name / पिता का नाम "></asp:Label>
                            
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblFName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrMotherName" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label27" runat="server" Text="Mother's Name / माता का नाम "></asp:Label>
                             
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblMName" runat="server"></asp:Label>
                        </td>
                    </tr>
                                      <tr class="normal" id="trguardian" runat="server" visible="false">
   <%-- <td width="3%">2.2.1
    </td>--%>
    <td>
        <asp:Label ID="LblGuardian" runat="server" Text="Guardian's Name / संरक्षक का नाम ">
        </asp:Label>
    </td>
                                          <td class="rightBorder" colspan="3">
    <asp:Label ID="lblGuardianName" runat="server"></asp:Label>

</td>
   <%-- <td class="rightBorder" colspan="3">
        <asp:TextBox ID="lblGuardianName" runat="server" MaxLength="60" TabIndex="9" Width="529px"
            onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
        <br />

        <span style="color: red;">( Full Name As Per Educational/Legal Certificate )</span>--%>

        <%-- Added --%>
       <%-- <div id="divgurdian" runat="server" style="display: none; position: absolute;" class="modalPopup5">
            Candidate submitting Guardian Name needs to submit the affidavit in prescribed format
        ( <a id="link" runat="server" target="_blank" style="text-decoration: none; color: black; font-weight: bold;">Click here to download the Format </a>) to NIELIT HQ, Delhi
         for further processing of the form before the last date of filling the  form ,
         failing which, the form will not be processed and no fee will be refunded. 
         If successful in examination, the certificate will be issued with Guardian Name only.
        <div align="right">
            <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
        </div>--%>

   <%-- </td>--%>
</tr>

                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Gender / लिंग"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="LblGender" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Marital Status / वैवाहिक स्थिति"></asp:Label>
                             
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblMaritalStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक  (dd/mm/yyyy)"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="LblDob" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Category / वर्ग"></asp:Label>
                            
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblCategory" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label71" runat="server" Text="Handicapped / विकलांग"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="LblHandicapped" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label72" runat="server" Text="Ex-Serviceman / पूर्व सेवाकर्मी"></asp:Label>
                             
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblExService" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label73" runat="server" Text="Religion / धर्म"></asp:Label>
                            
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblReligion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            3. Contact Details / संपर्क विवरण
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label18" runat="server" Text="Phone with STD code / दूरभाष एस टी डी कोड सहित "></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="LblLandLine" runat="server"></asp:Label>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="Label40" runat="server" Text="Mobile Number / मोबाइल नंबर"></asp:Label>
                            
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="lblMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="Email Address / ईमेल पता "></asp:Label>
                             
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            4. Permanent Address Details / स्थायी पता विवरण
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label83" runat="server" Text="Address Line1/पता पंक्ति 1 "></asp:Label>
                             
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblPerAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td class="normal">
                            <asp:Label ID="Label96" runat="server" Text="Address Line2/पता पंक्ति 2 "></asp:Label>
                            
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblPerAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label85" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                            
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblPerAddressLine3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label97" runat="server" Text="City Name/शहर का नाम"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:Label ID="lblPerCity" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label78" runat="server" Text="District / जिला  "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPerDistrict" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label79" runat="server" Text="State / राज्य "></asp:Label>
                             
                        </td>
                        <td>
                            <%--<asp:DropDownList ID="ddlPState" runat="server" Enabled="false" AutoPostBack="True" OnSelectedIndexChanged="ddlPState_SelectedIndexChanged"
     TabIndex="30" Width="375px">
 </asp:DropDownList>--%>
                             <asp:Label ID="lblPState" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Pin Code / पिन  कोड  "></asp:Label>
                            
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPerPinCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            5.
                            <asp:Label ID="Label74" runat="server" Text="Correspondence Details / पत्राचार की सूचना"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label98" runat="server" Text="Address Line1/पता पंक्ति 1 "></asp:Label>
                             
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblCorAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label99" runat="server" Text="Address Line2/पता पंक्ति 2 "></asp:Label>
                             
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblCorAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label100" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                             
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblCorAddressLine3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label101" runat="server" Text="City Name/शहर का नाम"></asp:Label>
                             
                        </td>
                        <td>
                            <asp:Label ID="lblCorCity" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label34" runat="server" Text="District / जिला  "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblDistrict" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label33" runat="server" Text="State / राज्य "></asp:Label>
                             
                        </td>
                        <td>
                          
                            <%--<asp:textbox ID="txtState" runat="server"></asp:textbox>--%>
                           <%-- <asp:DropDownList ID="ddlCorState" runat="server" Enabled="false"  AutoPostBack="True" OnSelectedIndexChanged="ddlCorState_SelectedIndexChanged"
     TabIndex="38" Width="375px">
 </asp:DropDownList>--%>
                             <asp:Label ID="lblCorState" runat="server"></asp:Label>
         
                        </td>
                        
                        <td>
                            <asp:Label ID="Label39" runat="server" Text="Pin Code / पिन  कोड  "></asp:Label>
                             
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="lblPincode" runat="server"></asp:Label>
                        </td>
                    </tr>
                                        <%--<tr class="normal">
    <td colspan="2">
        5.
        <asp:Label ID="lblapplicantType" runat="server" Text="Applied As / किसके रूप में आवेदन किया" Font-Bold="true"></asp:Label>
    </td>
    <td>
         <asp:Label ID="lblapplicantTypeName" Text="" runat="server"></asp:Label>
    </td>
    <td>
    </td>
</tr>--%>
                     <tr class="head1" visible="false" id="TrInstitutedetails" runat="server">
     <td colspan="2"> 
         <asp:Label ID="LblSectionInstituteNo" runat="server"></asp:Label>
<asp:Label ID="Label2" runat="server" Text="Institute Details / संस्थान का विवरण"></asp:Label>
         <%--<asp:Label ID="Label2" runat="server" Text="Institute Details "></asp:Label>--%>
     </td>
     <td>
     </td>
     <td>
     </td>
 </tr>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
                     <tr class="normal" visible="false" id="TrStateCenterAccno" runat="server">
     <td valign="top">
         <asp:Label ID="Lbl2" runat="server"></asp:Label>
     </td>
     <td valign="top">
         <asp:Label ID="Label9" runat="server" Text="Institute Name / संस्थान नाम <font color='RED'>*</font>">
         </asp:Label>
     </td>

     <td align="left"  style="width: 3%" colspan="2" class="rightBorder">
        <%-- <asp:DropDownList ID="DdlAccState" runat="server" Width="526px" AutoPostBack="True" OnSelectedIndexChanged="DdlAccState_SelectedIndexChanged" TabIndex="3">
             <asp:ListItem Value="0">--Select One--</asp:ListItem>
         </asp:DropDownList>--%>
         <asp:Label ID="lblInstName" runat="server" Text=" Institute Name / संस्थान नाम"></asp:Label>
     </td>
                        
 </tr>

                        <tr class="normal" visible="false" id="TrCenterNameAccno" runat="server">
        <td align="left" style="width: 3%" valign="top">
            <asp:Label ID="Lbl3" runat="server"></asp:Label>
        </td>
        <td align="left">
            <asp:Label ID="Label12" runat="server" Text="Accreditation no. of the institute / प्रत्यायन संख्या<font color='RED'>*</font>">
            </asp:Label>
        </td>
        <td align="left" style="width: 3%" colspan="2" class="rightBorder">
           <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
               
                   <%-- <asp:DropDownList ID="DdlAccCentre" runat="server" Width="526px" AutoPostBack="True"
                        TabIndex="5" OnSelectedIndexChanged="DdlAccCentre_SelectedIndexChanged">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    </asp:DropDownList>--%>
             <asp:Label ID="LblAccNo" runat="server" Text=" Accreditation no. of the institute / प्रत्यायन संख्या"></asp:Label>
               <%-- </ContentTemplate>--%>
               <%-- <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DdlAccState" EventName="SelectedIndexChanged" />
                </Triggers>--%>
           
        </td>
    </tr>
          </ContentTemplate>
              </asp:UpdatePanel>
                

                                        <tr class="head1">
    <td colspan="2">
                       <asp:Label ID="LblSectionIdentificationNo"
runat="server"></asp:Label>
       Identification Details / पहचान की सूचना
    </td>
    <td>
    </td>
    <td>
    </td>
</tr>
           <%-- Apaar Details --%>               
                 <tr>
     <td align="left" valign="top">
         <table style="width: 288%;" class="preview" border="0" cellspacing="0" id="tblapaar" runat="server" visible="false" cellpadding="2">
              <tr class="normal">
                   <td style="width:340px;">
                       <asp:Label ID="LblIdentification1" runat="server"></asp:Label>
 </td>
     <td style="width:270px;">
         <asp:Label ID="Label8" runat="server" Text="Apaar&nbsp; ID / अपार आईडी"></asp:Label>
          
     </td>
     <td class="rightBorder" colspan="3">
         <asp:Label ID="lblApaarId" runat="server"></asp:Label>
     </td>
 </tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification2" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="lblProviderPresent" runat="server" Text="Is Provider Present / क्या प्रदाता उपस्थित है"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblIsProviderPresent" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification3" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="lblConsentRelation" runat="server" Text="Consent Relation / सहमति संबंध"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblIsConsentRelation" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification4" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="Label17" runat="server" Text="Provider Name / प्रदाता का नाम"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblproviderName" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification5" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="Label20" runat="server" Text="Authentication Mode / प्रमाणीकरण मोड"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblAuthMode" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification6" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="Label23" runat="server" Text="Authentication ID No / प्रमाणीकरण आईडी संख्या"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblAuthId" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification7" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="Label25" runat="server" Text="Consent Date / सहमति दिनांक"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblConsentDate" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification8" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="Label29" runat="server" Text="Consent Time / सहमति समय"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblConsentTime" runat="server"></asp:Label>
    </td>
</tr>
                          <tr class="normal">
                               <td><asp:Label ID="LblIdentification9" runat="server"></asp:Label>
 </td>
    <td>
        <asp:Label ID="Label31" runat="server" Text="Consent Place / सहमति स्थान"></asp:Label>
         
    </td>
    <td class="rightBorder" colspan="3">
        <asp:Label ID="lblConsentPlace" runat="server"></asp:Label>
    </td>
</tr>
             </table>
         </td>
                     </tr>
                    <%-- <tr>
      <td>
          <table style="width: 100%; padding-top: 10px;" cellpadding="1" cellspacing="0">
              <tr>
                 
                  <td align="center" colspan="2" valign="middle" width="20%">
                      <img id="imgBarCode" runat="server" />
                  </td>
                  <td align="center" width="40%">
                      <img src="../images/sign.jpg" id="imgSignature" style="height: 35px; width: 168px"
                          runat="server" />
                  </td>
              </tr>
              <tr>
                  
                  <td>
                      &nbsp;
                  </td>
                  <td align="center">
                      Signature of Applicant/ हस्ताक्षर
                  </td>
              </tr>
          </table>
      </td>
  </tr>--%>
                     <tr class="head1">
                         
     <td colspan="3">
                                 <asp:Label ID="LblSectionDocumentsNo"
    runat="server">
</asp:Label>
         <asp:Label ID="Label15" runat="server" Text="Documents / दस्तावेज़" Font-Bold="true">
         </asp:Label>

     </td>
 </tr>

                    <%-- <tr class="normal">
     <td style="width:5%;">
          <asp:Label ID="lblDocNo1" runat="server" Text="8.1"></asp:Label>
     </td>
                      
     <td colspan="2" style="width:65%;">
         <asp:Label ID="lblEduquacertiForm" runat="server" 
             Text="Highest Education Form :"></asp:Label>
          
     </td>
     <td style="width:30%; border: 1px solid black;" class="rightBorder">
          
         <asp:Label ID="lblEduStatus" runat="server" />
         
     </td>
 </tr>--%>

<%-- <tr class="normal" id="trexpcerti" runat="server" visible="false">
     <td>
         <asp:Label ID="lblDocNo2" runat="server" Text="8.2"></asp:Label>
</td>
     <td colspan="2">
         <asp:Label ID="lblexpcertiForm" runat="server" 
             Text="Experience Certificate :"></asp:Label>
     </td>
     <td class="rightBorder">
       
         <asp:Label ID="lblExpCertiFormStatus" runat="server" />
     </td>
 </tr>--%>
<tr class="normal">
         <td style="width:5%;">
             <asp:Label ID="lblDocNo1" runat="server" Text="8.1"></asp:Label>
</td>
    <td colspan="2" style="width:65%;">
        <asp:Label ID="lblmedical" runat="server" 
            Text="PROOF OF MEDICAL ISSUE/AFFIDAVIT :"></asp:Label>
    </td>
    <td class="rightBorder" style="width:30%; border: 1px solid black;">
      
        <asp:Label ID="lblMedicalStatus" runat="server" />
    </td>
</tr>
                         <%-- <tr class="normal">
         <td>
              <asp:Label ID="lblDocNo4" runat="server" Text="8.4"></asp:Label>
</td>
    <td colspan="2">
        <asp:Label ID="lblIdCard" runat="server" 
            Text="ID Card :"></asp:Label>
    </td>
    <td class="rightBorder">
       
        <asp:Label ID="lblIdCardstatus" runat="server" />
    </td>
</tr>       --%>      
 <%--    <tr class="head1" id="TrPaymentDetail" runat="server">
      <td align="left" valign="top" colspan="4">
          8. Payment Detail
      </td>
  </tr>
  <tr class="normal" id="TrPaymentMode" runat="server">
      <td align="left" valign="top">
          Payment Mode:
      </td>
      <td>
          <asp:Label ID="LblPaymentMode" runat="server"></asp:Label>
      </td>
      <td>
          <asp:Label ID="LblFeeType" runat="server"></asp:Label>
      </td>
      <td class="rightBorder">
          <asp:Label ID="LblFeeAmount" runat="server"></asp:Label>
      </td>
  </tr>
  <tr class="normal" id="TrPaymentModeInstruction" runat="server">
      <td align="left" valign="top" colspan="4" class="rightBorder">
          <asp:Label ID="LblPaymentDescription" runat="server"></asp:Label>
      </td>
  </tr>
  <tr class="normal" id="TrPaymentModeTransactionNo" runat="server">
      <td align="left" valign="top" colspan="3">
          Please enter the received Transaction no.<asp:Label ID="LblPaymentSrc" runat="server"></asp:Label>
          प्राप्त ट्रांजेक्शन संख्या दर्ज करें
      </td>
      <td class="rightBorder">
           Transaction No.
           <asp:Label ID="lblTransactionNumber" runat="server"></asp:Label>
      </td>
  </tr>
  <tr class="normal" id="TrPayment_not" runat="server">
      <td align="left" valign="top" colspan="4" class="rightBorder">
          &nbsp;<strong>Note</strong> *&nbsp; <%--Please don&#39;t send this form without making
          payment./कृपया इस फार्म को भुगतान के बिना नहीं भेजें--%>
        <%--  Please pay the registration fee online after online submission of registration application form/पंजीकरण आवेदन फार्म ऑनलाइन जमा करने के बाद ऑनलाइन पंजीकरण शुल्क का भुगतान करें
      </td>
  </tr>  --%>
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
   
 
    </table>
    <div style="text-align: center; margin-top: 10px;" id="divfooter" runat="server">
        <asp:Button ID="Btnsubmit" runat="server" Text="Final Submit" OnClick="Btnsubmit_Click" Width="100px" OnClientClick="return confirm('Are you sure you want to final submit the form? Please verify all the details carefully. Once final submitted, the data cannot be modified.If you have filled the application through an institute, please coordinate with the institute for verification of your form.');"  />
         <%--<asp:Button ID="btnSave" runat="server" Text="Proceed" TabIndex="53" OnClick="btnSave_Click"/>--%>
        <%--<asp:Button ID="btnUpload" runat="server" Text="Upload Documents" />--%>
        <asp:Button ID="Btnback" runat="server" Text="Back" Width="50px" OnClick="Btnback_Click" />
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
    </form>
</body>
</html>
