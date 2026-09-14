<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateUpdateRequestDocUpload.aspx.cs" Inherits="CAND_CandidateUpdateRequestDocUpload" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">    <title></title>
<head id="Head1" runat="server">
    <title></title>
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
        .auto-style1 {
            width: 483px;
        }
    </style>
 
     <script type="text/javascript">
         function preventBack() { window.history.forward(); }
         setTimeout("preventBack()", 0);
         window.onunload = function () { null };
    </script>
    <script type="text/Javascript" language ="javascript" >
        function ValidateDocUpload() {
            //if (trHandicapped.gdalternate1) {
           // if($('#trHandicapped').is(':visible'))
            if ($("#trHandicapped").is(":visible")) {
                //It's visible

                var inputElement = document.getElementById('fileHandicapped');
                var files = inputElement.files;
                if (files.length == 0) {
                    alert("Please choose a file first...");
                    return false;
                }
            }
            if ($("#trMarital").is(":visible")) {
                var inputElementMarital = document.getElementById('fileMarital');
                var filesMarital = inputElementMarital.files;
                if (filesMarital.length == 0) {
                    alert("Please choose a file Marital File");
                    return false;
                }
            }
            if ($("#trMobile").is(":visible")) {
                var inputElementMob = document.getElementById('FileMobile');
                var filesMob = inputElementMob.files;
                if (filesMob.length == 0) {
                    alert("Please choose a file first...");
                    return false;
                }
            }
            if ($("#trEmail").is(":visible")) {
                var inputElementEmail = document.getElementById('FileEmail');
                var filesEmail = inputElementEmail.files;
                if (filesEmail.length == 0) {
                    alert("Please choose a file first...");
                    return false;
                }
            }
           // }
            //if (!isBlank("fileHandicapped", "Upload Certificate"))
            //    return false;
            //        alert("Pease Upload Certificate")
            //        return false;
           // if (document.getElementById("trHandicapped").value == true)
                //if(document.getElementById("trHandicapped").disabled == false)
                //if (trHandicapped == true)
            // {
           
                //if (!isBlank("fileHandicapped","Upload Docs"))
                //{

                //    //if (!isBlank("fileHandicapped.FileName", "Upload Certificate"))
                //        alert("Pease Upload Certificate")
                //        return false;
                //}
                //}
              
            //if (trMarital.Visible == true) {
            //    if (fileMarital.FileName == "")
            //    {
            //        if(!isBlank("fileMarital.FileName", "Upload Certificate"))                   
            //        return false;
            //    }
            //}
            //if (trCasteCerti.Visible == true) {                
            //    if (FileCaste.FileName == "") {
            //        if (!isBlank("FileCaste.FileName", "Upload Certificate"))
            //            return false;
            //    }
            //}
            //if (trMobile.Visible == true) {
            //    if (FileMobile.FileName == "") {
            //        if (!isBlank("FileMobile.FileName", "Upload Certificate"))
            //            return false;
            //    }
            //}
            //if (trEmail.Visible == true) {
            //    if (FileEmail.FileName == "") {
            //        if (!isBlank("FileEmail.FileName", "Upload Certificate"))
            //            return false;
            //    }
            //}

            //if (trCorAddress.Visible == true) {
            //    if (FileCorAddress.FileName == "") {
            //        if (!isBlank("FileCorAddress.FileName", "Upload Certificate"))
            //            return false;
            //    }
            //}
            //if (trHighEducation.Visible == true) {
            //    if (FileHighEducation.FileName == "") {
            //        if (!isBlank("FileHighEducation.FileName", "Upload Certificate"))
            //            return false;
            //    }
            //}
           

           

        }

        function confirm_meth() {
            if (confirm("Do you want to continue!Click 'YES'") == true) {
                document.writeln("<b>You had click on 'YES' Button</b>");
            }
            else {
                document.writeln("<b>You had clic on 'NO' Button</b>");
            }
        }
</script> 
   
</head>

   <body style="background-color: #ffffff;">
    <form id="form1" runat="server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" id="Tblpreview"
        runat="server">
        <tr>
            <td>
                <uc1:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
         <tr>
            <td align="center">
                <asp:Label ID="Lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lblmsg" runat="server" EnableTheming ="false" ForeColor ="#990000"></asp:Label>
            </td>
        </tr>
         
        <tr style="height: 45px;">
            <td align="center" style="border-bottom: 1px solid #000000;" valign="middle">
                <asp:Label ID="Lblhead" runat="server" Style="font-size: 22px;"></asp:Label>
                
            </td>
        </tr>
        <tr>
            <td align="center">
                 <table style="width: 100%;" class="preview" cellpadding="1" cellspacing="0">
                     <tr class="gdalternate1" id="trHandicapped" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblHandicapped" runat="server" Text="Self-Attested copy of Handicapped Certificate.<font color='RED'>*</font>">
                                </asp:Label><br />
                                  ( PDF file with size upto 100 KB )               
                            </td>
                            <td><asp:FileUpload ID="fileHandicapped" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <%--<asp:LinkButton id="LinkDownloadHandi" Text="Download" OnClick="LinkDownloadHandi_Click" runat="server"/>--%>
                                 <asp:button id="UploadDisabiliy" runat="server" text="Upload" OnClick="UploadDisabiliy_Click"/>
                                <br /><asp:Label ID="showHandicapped" runat="server"  ForeColor="Green"></asp:Label><br />
                                &nbsp;<asp:Label ID="lblHandicappedFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regHandicapped" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="fileHandicapped" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                          <tr class="gdalternate1" id="trMarital" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblMarital" runat="server" Text="Application along with Self-Attested Copy of marriage Certificate.<font color='RED'>*</font>">
                                </asp:Label><br />
                                ( PDF file with size upto 100 KB )                                                 
                            </td>
                            <td><asp:FileUpload ID="fileMarital" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadMaritalCert" runat="server" OnClick="UploadMaritalCert_Click" text="Upload" />
                                <br /><asp:Label ID="showMarital" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblMaritalFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regMarital" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="fileMarital" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                      <tr class="gdalternate1" id="trCasteCerti" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblCasteCerti" runat="server" Text="Self-Attested copy of Caste Certificate.<font color='RED'>*</font>">
                                </asp:Label><br/>
                                 <font color='BLUE'>( In case of General Category, Please Upload Undertaking ) </font>  
                                <br />
                                  ( PDF file with size upto 100 KB )                 
                            </td>
                            <td><asp:FileUpload ID="FileCaste" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"/>
                                <asp:Button ID="UploadCasteCerti" runat="server" OnClick="UploadCasteCerti_Click" text="Upload" />
                                 <br /><asp:Label ID="showcaste" runat="server"  ForeColor="Green"></asp:Label><br/>
                                 
                                <asp:Label ID="lblCasteCertFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regCaste" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileCaste" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                      <tr class="gdalternate1" id="trMobile" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblMobile" runat="server" Text="Application or Email along with Self-Attested copy of registration card registered email address<font color='RED'>*</font>">
                                </asp:Label>
                                 <br />
                                ( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileMobile" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadMobile" runat="server" OnClick="UploadMobile_Click" text="Upload" />
                                 <br /><asp:Label ID="showMobile" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblMobileFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regMobile" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileMobile" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>

                     <tr class="gdalternate1" id="trEmail" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblEmail" runat="server" Text="Application with Self-Attested copy of Registration Card.<font color='RED'>*</font>">
                                </asp:Label>
                                   <br />( PDF file with size upto 100 KB )              
                            </td>
                            <td><asp:FileUpload ID="FileEmail" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadEmail" runat="server" OnClick="UploadEmail_Click" text="Upload" />
                                  <br /><asp:Label ID="showEmail" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblEmailFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regEmail" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileEmail" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>

                     <tr class="gdalternate1" id="trCorAddress" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblCorAddress" runat="server" Text="Application along with Self-Attested copy of registration card along with any document containing the desired address<font color='RED'>*</font>">
                                </asp:Label>
                                  <br />( PDF file with size upto 100 KB )               
                            </td>
                            <td><asp:FileUpload ID="FileCorAddress" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadCorAddress" runat="server" OnClick="UploadCorAddress_Click" text="Upload" />
                                 <br /><asp:Label ID="showAddress" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblCorAddressFile" runat="server" ></asp:Label>
                                <asp:RegularExpressionValidator ID="regAddress" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileCorAddress" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>

                     <tr class="gdalternate1" id="trHighEducation" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblHighEducation" runat="server" Text="Application along with the Self-Attested copy of related educational documents of the candidate. <font color='RED'>*</font>">
                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileHighEducation" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadHighEdu" runat="server" OnClick="UploadHighEdu_Click" text="Upload" />
                               <br /><asp:Label ID="showHighEdu" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblHighEducationFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regHighEducation" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileHighEducation" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                          <tr class="gdalternate1" id="trCandidate" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblCname" runat="server" Text="Self-Attested copy of highest qualification Certificate of the Candidate. <font color='RED'>*</font>">
                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileCnameCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadCnameCerti" runat="server" text="Upload" OnClick="UploadCnameCerti_Click" />
                               <br /><asp:Label ID="showCnameCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblCnameFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regCname" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileCnameCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>             
                  <tr class="gdalternate1" id="trFather" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblFather" runat="server" Text="Self-Attested Copy of the documents (10th/HSC certificate or any other Govt.<br /> issued photo card of the candidate) where
                                   the Father’s name is clearly mentioned.<font color='RED'>*</font>">
                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileFatherCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadFatherCerti" runat="server" text="Upload" OnClick="UploadFatherCerti_Click" />
                               <br /><asp:Label ID="showFatherCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblFatherFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regFather" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileFatherCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                      <tr class="gdalternate1" id="trMother" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblMother" runat="server" Text="Self-Attested copy of the documents (10th/HSC certificate or any other Govt.<br/> issued photo card of the candidate) where
                                 the Mother’s name is clearly mentioned<br /> or Self-Attested copy of the Mother’s Aadhar Card.<font color='RED'>*</font>">

                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileMotherCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadMotherCerti" runat="server" text="Upload" OnClick="UploadMotherCerti_Click"  />
                               <br /><asp:Label ID="showMotherCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblMotherFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regMother" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileMotherCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                      <tr class="gdalternate1" id="trDob" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblDob" runat="server" Text="Self-Attested copy of 10th Certificate of the Candidate.<font color='RED'>*</font>">

                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileDobCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadDobCerti" runat="server" text="Upload" OnClick="UploadDobCerti_Click"  />
                               <br /><asp:Label ID="showDobCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblDobFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regDob" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileDobCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                     <tr class="gdalternate1" id="trGender" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblGender" runat="server" Text="Self-Attested copy of 10th Certificate of the Candidate.<font color='RED'>*</font>">

                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileGenderCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadGenderCerti" runat="server" text="Upload" OnClick="UploadGenderCerti_Click"  />
                               <br /><asp:Label ID="showGenderCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblGenderFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regGender" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileGenderCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                      <tr class="gdalternate1" id="trPerAddress" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblPerAddress" runat="server" Text="Application along with Self-Attested copy of Registration ICard issued by <br />  NIELIT along with any document containing the address.<font color='RED'>*</font>">

                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FilePerAddressCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadPerAddressCerti" runat="server" text="Upload" OnClick="UploadPerAddressCerti_Click"  />
                               <br /><asp:Label ID="showPerAddressCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblPerAddressFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regPerAddress" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FilePerAddressCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                     <tr class="gdalternate1" id="trRegType" runat="server" visible="false">
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblRegType" runat="server" Text="NOC issued by current institute.<font color='RED'>*</font>">

                                </asp:Label>
                                 <br />( PDF file with size upto 100 KB )                
                            </td>
                            <td><asp:FileUpload ID="FileRegTypeCert" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="250px"  />
                                <asp:Button ID="UploadRegTypeCerti" runat="server" text="Upload" OnClick="UploadRegTypeCerti_Click"  />
                               <br /><asp:Label ID="showRegTypeCert" runat="server"  ForeColor="Green"></asp:Label><br/>
                                &nbsp;<asp:Label ID="lblRegTypeFile" runat="server"></asp:Label>
                                <asp:RegularExpressionValidator ID="regRegType" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="FileRegTypeCert" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </td> 
                        </tr>
                      <tr>
                            <td align="center" colspan="3">
                                <br />
                                <asp:Button ID="btnSave" runat="server" Text="Final Submit" TabIndex="53" OnClick="btnSave_Click"
                                    />
                                <%--<asp:Button ID="btnSave" runat="server" Text="Final Submit" TabIndex="53" OnClientClick="return ValidateDocUpload();" OnClick="btnSave_Click"
                                    />--%>
                              <%--  <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" OnClick="btnback_Click" />--%>
                            </td>
                        </tr>   
                     
                     </table>
                 <asp:HiddenField ID="HiddenField2" runat="server" />
                </td>
            </tr>
      
     
       </table>
           </form>
</body>
</html>
