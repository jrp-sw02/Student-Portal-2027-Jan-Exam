<%@ Page Title="Course Examination Form" Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="ExamFormPreview.aspx.cs"
    Inherits="ExamFormPreview" Debug="true" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" id="Tblpreview"
        runat="server">
        <tr>
            <td>
                <uc1:NormalHeader Visible="false" ID="NormalHeader1" runat="server" />
            </td>
        </tr>
        <tr style="height: 45px;">
            <td align="center" style="border-bottom: 1px solid #000000; font-size:20px;" valign="middle">
                <asp:Label ID="lbllevel" runat="server" Text=""></asp:Label>
                ONLINE EXAMINATION FORM -&nbsp;
                <asp:Label ID="lblExamName" runat="server"></asp:Label>
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 10px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr  >
            <td align="center">
            <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
                Visible="false"> </asp:Label>
                </td> 
        </tr>
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
                            <table>
                                <tr>
                                    <td style="border-bottom: 0px; border-left: 0px; border-top: 0px;" align="center">
                                        <img src="../images/photo.jpg" id="ImgApplicantPhoto" style="height: 111px; width: 113px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: 0px; border-left: 0px; border-top: 0px;" align="center">
                                        <img id="imgPhotoBarcode" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblAppNumber" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblAppDataTime" runat="server" Font-Size="Larger">15-Jan-2013</asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            valign="middle">
                            <table cellpadding="0" cellspacing="0" style="height: 100%;" width="100%">
                                <tr id="TrDemandnote" runat="server">
                                    <td align="left" style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 1px solid #000000;"
                                        valign="middle">
                                        &nbsp;
                                        <asp:Label ID="Label95" runat="server" Text="Demand Note No."></asp:Label>
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 0px;
                                        width: 50%" align="left">
                                        &nbsp;<asp:Label ID="lblDemandNoteNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: 1px solid #000000; width: 40%; border-left: 0px;
                                        border-right: 1px solid #000000;" valign="middle">
                                        &nbsp;
                                        <asp:Label ID="Label96" runat="server" Text="Demand Note Date"></asp:Label>
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; width: 60%;" align="left">
                                        &nbsp;
                                        <asp:Label ID="lblDemandNoteDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: 0px; border-left: 0px; border-right: 1px solid #000000;"
                                        valign="middle">
                                        &nbsp; Batch No.
                                    </td>
                                    <td style="border-bottom: 0px;">
                                        &nbsp;
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
                        <td colspan="2">
                            1.
                            <asp:Label ID="Label69" runat="server" Text="Applicant's Personal Details / आवेदक का व्यक्तिगत विवरण"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label70" runat="server" Text="Applicant's Full Name / आवेदक का पूरा नाम"></asp:Label>
                        </td>
                        <td width="30%" class="rightBorder">
                            <asp:Label ID="LblAppName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Father's/Guardian's Name / पिता/अभिभावक का नाम "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblFName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Mother's Name / माता का नाम "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblMName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label30" runat="server" Text="Date of Birth / जन्म दिनांक  (dd/mm/yyyy)"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblDob" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr class="head1">
                        <td colspan="2">
                            2.
                            <asp:Label ID="Label14" runat="server" Text="Registration Details / पंजीयन का विवरण"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td width="21%">
                            <asp:Label ID="Label15" runat="server" Text="Registration No. / पंजीकरण संख्या"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="Lblregno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label17" runat="server" Text="Course Name / कोर्स का नाम "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="Lblcname" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label19" runat="server" Text="Candidate Type / उम्मीदवार का प्रकार "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="Lblctype1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            3. Contact Details / संपर्क विवरण
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label35" runat="server" Text="Phone with STD code / दूरभाष एस टी डी कोड सहित "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblLandLine" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label36" runat="server" Text="Mobile Number / मोबाइल नंबर"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label37" runat="server" Text="Email Address / ईमेल पता "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblEmail" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            4.
                            <asp:Label ID="Lbladdress" runat="server" Text="Correspondence Address / पत्राचार का पता "></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td valign="top" runat="server">
                            <asp:Label ID="Lbladd" runat="server" Text="Address / पता "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="Lblfulladd" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            5.
                            <asp:Label ID="Label1" runat="server" Text="Exam Details / परीक्षा का विवरण"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tryp">
                        <td valign="top" id="Tdqualified" runat="server">
                            <asp:Label ID="Label84" runat="server" Text="Exam Name / परीक्षा का नाम"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Tdlblqualified" runat="server" class="rightBorder">
                            <asp:Label ID="Lblexam" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="TrOfflineExam1" visible ="false">
                        <td valign="top" id="Td1" runat="server">
                            <asp:Label ID="Label4" runat="server" Text=" Exam Centre First Choice / परीक्षा केंद्र पहला विकल्प"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td2" runat="server" class="rightBorder">
                            <asp:Label ID="lblCentre1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="TrOfflineExam2" visible ="false">
                        <td valign="top" id="Td3" runat="server">
                            <asp:Label ID="Label6" runat="server" Text=" Exam Centre Second Choice / परीक्षा केंद्र दूसरा विकल्प"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td4" runat="server" class="rightBorder">
                            <asp:Label ID="lblCentre2" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <%--Added By Reena --%>
                      <tr class="normal" runat="server" id="TrOnlineExam1" visible="false">
                        <td valign="top" id="Td18" runat="server">
                            <asp:Label ID="Label16" runat="server" Text="Online Exam Centre First Choice / परीक्षा केंद्र पहला विकल्प"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td23" runat="server" class="rightBorder">
                            <asp:Label ID="lblOnlCentre1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="TrOnlineExam2" visible="false">
                        <td valign="top" id="Td24" runat="server">
                            <asp:Label ID="Label21" runat="server" Text="Online Exam Centre Second Choice / परीक्षा केंद्र दूसरा विकल्प"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td25" runat="server" class="rightBorder">
                            <asp:Label ID="lblOnlCentre2" runat="server"></asp:Label>
                        </td>
                    </tr>
                      <tr class="normal" runat="server" id="TrPracExam1" visible="false">
                        <td valign="top" id="Td12" runat="server">
                            <asp:Label ID="Label9" runat="server" Text="Practical Exam Centre First Choice / परीक्षा केंद्र पहला विकल्प"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td13" runat="server" class="rightBorder">
                            <asp:Label ID="lblPracCentre1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="TrPracExam2" visible="false">
                        <td valign="top" id="Td14" runat="server">
                            <asp:Label ID="Label18" runat="server" Text="Practical Exam Centre Second Choice / परीक्षा केंद्र दूसरा विकल्प"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td17" runat="server" class="rightBorder">
                            <asp:Label ID="lblPracCentre2" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <%-- End Added By Reena --%>
                    <tr class="normal" >
                        <td valign="top"  >
                            Improvement/इम्प्रूवमेंट</td>
                        <td align="left" valign="top"   class="rightBorder">
                            <asp:Label ID="lblImprovement" runat="server"></asp:Label></td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr3">
                        <td valign="top" id="Td5" runat="server">
                            <asp:Label ID="Label8" runat="server" Text="Modules Appearing / मॉड्यूल देने"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td6" runat="server" class="rightBorder">
                            <asp:Label ID="lblTheoryModules" Text="NA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr4">
                        <td valign="top" id="Td7" runat="server">
                            <asp:Label ID="Label10" runat="server" Text="Practicals Appearing / प्रैक्टिकल देने"></asp:Label>
                        </td>
                        <td align="left" valign="top" id="Td8" runat="server" class="rightBorder">
                            <asp:Label ID="lblPracticals" Text="NA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            6.
                            <asp:Label ID="Label11" runat="server" Text="Fee Details / शुल्क विवरण"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr5">
                        <td valign="top" id="Td9" runat="server">
                            <asp:Label ID="Label12" runat="server" Text="">No. of Modules X Fee/Module / मॉड्यूल की संख्या X शुल्क/मॉड्यूल  
                                                                 </asp:Label>
                        </td>
                        <td align="right" valign="top" id="Td10" runat="server" class="rightBorder">
                            <asp:Label ID="lblTheoryCount" Style="float: left;" Text="" runat="server"></asp:Label>
                            <asp:Label ID="lblTheoryFee" Text="" runat="server"> </asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr7">
                        <td valign="top" id="Td15" runat="server">
                            <asp:Label ID="Label13" runat="server" Text="">No. of Practicals X Fee/Practical / प्रैक्टिकल की संख्या X शुल्क/प्रैक्टिकल</asp:Label>
                        </td>
                        <td align="right" valign="top" id="Td16" runat="server" class="rightBorder">
                            <asp:Label ID="lblPractialCount" style="float:left;" Text="" runat="server"></asp:Label>
                            <asp:Label ID="lblPracticalFee" Text="" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr6">
                        <td valign="top" id="Td11" runat="server">
                            <asp:Label ID="Label5" runat="server" Text="Exam Form Processing Fees / परीक्षा फार्म का प्रसंस्करण शुल्क"></asp:Label>
                        </td>
                        <td align="right" valign="top" id="tdProcessingFee" runat="server" class="rightBorder">
                            <asp:Label ID="Label7" Text="0.00" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr9">
                        <td valign="top" id="Td19" runat="server">
                            <asp:Label ID="Label22" runat="server" Text="Late Fees(if applicable) / विलंब शुल्क (यदि लागू हो)"></asp:Label>
                        </td>
                        <td align="right" valign="top" id="Td20" runat="server" class="rightBorder">
                            <asp:Label ID="Lbllatefee" Text="0.00 (Not Applicable)" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tr10">
                        <td valign="top" id="Td21" runat="server">
                            <asp:Label ID="Label24" runat="server" Text="Total Fees / कुल शुल्क "></asp:Label>
                        </td>
                        <td align="right" valign="top" id="Td22" runat="server" class="rightBorder">
                            <asp:Label style="float:left;" ID="lblFeeInWords" Text="" runat="server"></asp:Label>
                            <asp:Label ID="lblTotalFee" Text="" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1" id="TrPaymentDetail" runat="server">
                        <td align="left" valign="top" colspan="2">
                            8. Payment Detail
                        </td>
                    </tr>
                    <tr class="normal" id="Tr8" runat="server">
                        <td align="left" valign="top">
                            Applying This Exam
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="lblAppliedAs" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="Tr11" runat="server">
                        <td align="left" valign="top">
                            Application Examination Fee will be Deposited by
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="lblFeeSource" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPaymentMode" runat="server">
                        <td align="left" valign="top">
                            Payment Mode:
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPaymentMode" runat="server"></asp:Label><asp:Label ID="LblFeeType"
                                runat="server"></asp:Label><asp:Label ID="LblFeeAmount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPaymentModeTransactionNo" runat="server">
                        <td align="left" valign="top">
                            Transaction no. of payment receipt<asp:Label ID="LblPaymentSrc" runat="server"></asp:Label>
                            प्राप्त ट्रांजेक्शन संख्या दर्ज करें
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPaymentDescription" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="trPaymentNoteDirect" runat="server">
                        <td align="left" valign="top" colspan="2" class="rightBorder">
                            &nbsp;<strong>Note *&nbsp;: Please don&#39;t send this form without making payment./कृपया इस फार्म को भुगतान के बिना नहीं भेजें
                                </strong>
                        </td>
                    </tr>
                    <tr class="normal" id="trPaymentNoteInstitute" runat="server">
                        <td align="left" valign="top" colspan="2" class="rightBorder">
                            &nbsp;<strong>Note *: Please pay examination fee to the institute mentioned above in
                                the form. /कृपया परीक्षा शुल्क का भुगतान ऊपर वर्णित संस्थान को करें |</strong>
                        </td>
                    </tr>
                    <tr class="normal" id="trPaymentNoteInstituteSelf" runat="server">
                        <td align="left" valign="top" colspan="2" class="rightBorder">
                            &nbsp;<strong>Note *: Please pay examination fee through available payment options and
                                after making payment submit this form for institute verification to the institute
                                mentioned above in the form. /कृपया भुगतान के लिए उपलब्ध विकल्पों में से किसी एक
                                का चयन करके परीक्षा शुल्क का भुगतान कर ऊपर वर्णित संस्थान को सत्यापन के लिए परीक्षा
                                फार्म प्रस्तुत करें |</strong>
                        </td>
                    </tr>
                    <tr class="normal" id="tr12" runat="server">
                        <td align="left" valign="top" colspan="2" class="rightBorder">
                            &nbsp;<strong>Note *: Last date of payment of examination fee / परीक्षा शुल्क का भुगतान
                                की अंतिम तारीख: </strong>
                            <asp:Label ID="lblLastDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            7. Declaration / घोषणा
                        </td>
                    </tr>
                    <tr class="normal">
                        <td align="left" valign="top" colspan="2" class="rightBorder" style="text-align: justify;">
                            I
                            <asp:Label ID="lblName" Font-Bold="true" runat="server" Text=""></asp:Label>
                            registered
                            <asp:Label ID="lblCandidateType" Font-Bold="true" Text="NA" runat="server"></asp:Label>
                            hereby declare that, all the particular stated in the application, are true to the
                            best of my knowledge and belief. I hereby certify that I have applied the aforesaid
                            checks before submitting the Online Examination Form. I have read and understood
                            all the instructions available on the site. I agree to abide by the rules and regulations
                            of the NIELIT and also to the decision of the Examination Authority, on any issue
                            related to my admission to the Examination. I have noted that the Examination Authority
                            has the right to withhold my result ever after appearing in the Examination in addition
                            to any other action as may be deemed fit in the event of any of the statements made
                            above being found incorrect or my candidature being found ineligible at a later
                            date. I have specially gone through the eligibility criteria laid down by NIELIT
                            for appearing in different examinations and I confirm that I fullfill the eligibility
                            for the Theory & Practical modules, I have applied for.
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; padding-top: 10px;" cellpadding="1" cellspacing="0">
                    <tr>
                        <td align="center" width="40%">
                            <img src="../images/thumb.jpg" id="imgThumbImpression" style="height: 38px; width: 168px"
                                runat="server" />
                        </td>
                        <td align="center" colspan="2" valign="middle" width="20%">
                            <img id="imgBarCode" runat="server" />
                        </td>
                        <td align="center" width="40%">
                            <img src="../images/sign.jpg" id="imgSignature" style="height: 31px; width: 148px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Left hand thumb impression / बाएं हाथ के अंगूठे का निशान
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center">
                            Signature of Applicant/ हस्ताक्षर
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="text-align: center; margin-top: 10px;" id="divfooter" runat="server">
        <asp:Button ID="Btnsubmit" runat="server" Text="Final Submit" Width="100px" OnClick="Btnsubmit_Click" />
       <%-- <asp:Button ID="Btnsubmit12" runat="server" visible="false" Text="Final Submit" Width="100px" OnClick="Btnsubmit12_Click" />--%>
        <asp:Button ID="btnback" runat="server" Text="Back" Width="100px" OnClick="Btnback_Click" style="height: 26px" />
    </div>
    <br />
    <asp:HiddenField ID="courseid" runat="server" />
    <asp:HiddenField ID="appid" runat="server" />
    </form>
</body>
</html>
