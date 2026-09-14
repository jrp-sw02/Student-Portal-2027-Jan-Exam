<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="CertificatePreview.aspx.cs"
    Inherits="CertificatePreview" Title="Certificate Examination Form" Debug="True" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }
    </script>
    <style type="text/css">
        
    </style>
</head>
<body style="background-color: #ffffff; margin-bottom: 0px;">
    <form id="form1" runat="server">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
            <tr>
                <td colspan="3">
                    <uc2:NormalHeader ID="NormalHeader1" runat="server" Visible="False" />
                </td>
            </tr>
            <tr>
                <td align="center" style="border-bottom: 1px solid #000000;" valign="middle">
                    <asp:Label ID="Lblhead" runat="server" Style="font-size: 22px;" Text=""></asp:Label>
                    <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 20px;"
                        OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                        ImageUrl="~/images/print.gif" ToolTip="Print Form" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table style="width: 100%;" class="preview" cellpadding="1" cellspacing="0">
                        <tr class="normal" style="font: bold 18px arial;">
                            <td align="center" width="170px">Application Number
                            </td>
                            <td align="center" width="220px">Application Date &amp; Time
                            </td>
                            <td align="center" width="270px">For Office Use Only
                            </td>
                            <td style="border-bottom: 1px solid #000000; height: 111px; border-right: 1px solid #000000;"
                                align="center" valign="middle" rowspan="2" width="210px">
                                <%--<div class="DivImage1">--%>
                                <asp:Image ID="ImgApplicantPhoto" CssClass="PhotoImage1" ImageUrl="../images/photo.jpg"
                                    runat="server" Height="100px" Width="97px" />
                                <img id="imgPhotoBarcode" runat="server" class="BarCodeImage1" />
                                <%-- </div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 117px;"
                                align="center">
                                <asp:Label ID="LblAppNumber" runat="server" Font-Size="Larger"></asp:Label>
                            </td>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 117px;"
                                align="center">
                                <asp:Label ID="LblAppDataTime" runat="server" Font-Size="Larger"></asp:Label>
                            </td>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 117px;">
                                <table cellpadding="0" cellspacing="0" style="height: 117px;" width="100%">
                                    <tr id="TrDemandnote" runat="server">
                                        <td align="left" style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 1px solid #000000;"
                                            valign="middle">&nbsp;
                                        <asp:Label ID="Label95" runat="server" Text="Demand Note Number"></asp:Label>
                                        </td>
                                        <td style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 0px; width: 50%"
                                            align="left">&nbsp;
                                        <asp:Label ID="LblDemandNoteID" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="TrDemandnote1" runat="server">
                                        <td align="left" style="border-bottom: 1px solid #000000; width: 50%; border-left: 0px; border-right: 1px solid #000000;"
                                            valign="middle">&nbsp;
                                        <asp:Label ID="Label102" runat="server" Text="Demand Note Date"></asp:Label>
                                        </td>
                                        <td style="border-bottom: 1px solid #000000; width: 50%;" align="left">&nbsp;<asp:Label ID="LblDemandNoteDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 1px solid #000000;"
                                            valign="middle">&nbsp;
                                        </td>
                                        <td style="border-bottom: 1px solid #000000;">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="border-bottom: 0px; border-left: 0px; border-right: 1px solid #000000;"
                                            valign="middle">&nbsp;
                                        </td>
                                        <td style="border-bottom: 0px;">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" height="20px">
                    <asp:Label ID="lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table style="width: 100%;" class="preview" border="0" cellspacing="0" cellpadding="2">
                        <tr class="head1">
                            <td align="left" width="35%">1. Examination Details / परीक्षा का विवरण
                            </td>
                            <td align="left" width="15%"></td>
                            <td align="left" width="25%">&nbsp;
                            </td>
                            <td align="left" width="25%">&nbsp;
                            </td>
                        </tr>
                        <tr id="Tr1" class="normal" runat="server">
                            <td>
                                <asp:Label ID="Label89" runat="server" Text="Applied for Examination / किस परीक्षा के लिए आवेदन किया"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblExamCycle" runat="server"></asp:Label>
                            </td>
                            <td class="rightBorder"></td>
                        </tr>
                        <tr class="normal" id="TrAccState" runat="server">
                            <td id="TdAppliedAs" runat="server" rowspan="2" valign="top">
                                <asp:Label ID="Label90" runat="server" Text="Applied As / किसके रूप में आवेदन किया"></asp:Label>
                            </td>
                            <td rowspan="2" id="TrLblAppliedAs" runat="server" valign="top">
                                <asp:Label ID="LblAppliedAs" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblAccstateHead" runat="server" Text="State of Approved Institute  / मान्यता प्राप्त संस्थान के राज्य का नाम"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblAccState" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrAccCentre" runat="server">
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Centre Name of Approved Institute / मान्यता प्राप्त संस्थान के केंद्र का नाम"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblAccCentre" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="Tr2" class="normal" runat="server">
                            <td>
                                <asp:Label ID="Label93" runat="server" Text="Examination Center1 / परीक्षा केंद्र 1"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblExamCentre1" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label94" runat="server" Text="Examination Center2 / परीक्षा केंद्र 2"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblExamCentre2" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td rowspan="2" id="TdAlreadyAppeared" runat="server">Appeared in Previous
                            <asp:Label ID="LblCourseinEnglish" runat="server" Text=""></asp:Label>
                                &nbsp;Exam / पिछली <span id="spnCourseinHindi" runat="server"></span>परीक्षा
                            </td>
                            <td rowspan="2" id="TdLblAlreadyAppeared" runat="server">
                                <asp:Label ID="LblIsPreExamined" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Lblexam" runat="server" Text="Last Exam Name /अंतिम परीक्षा का नाम"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <br />
                                <asp:Label ID="LblLastMonthYear" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrPreRollno" runat="server">
                            <td>
                                <asp:Label ID="Label86" runat="server" Text="Previous Roll No. / पिछले रोल नंबर "></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblPreRno" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="2">2.
                            <asp:Label ID="Label24" runat="server" Text="Applicant's Personal Details / आवेदक का व्यक्तिगत विवरण"></asp:Label>
                            </td>
                            <td></td>
                            <td class="rightBorder"></td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label25" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम"></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblAppName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrFatherName" runat="server">
                            <td>
                                <asp:Label ID="Label26" runat="server" Text="Father's Name / पिता का नाम "></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblFName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrMotherName" runat="server">
                            <td>
                                <asp:Label ID="Label27" runat="server" Text="Mother's Name / माता का नाम "></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblMName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrGardianName" runat="server">
                            <td>
                                <asp:Label ID="Label82" runat="server" Text="Guardian's Name / संरक्षक का नाम "></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label28" runat="server" Text="Gender / लिंग"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblGender" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label31" runat="server" Text="Category / वर्ग"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblCategory" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="TrDisability" runat="server" visible="false" class="normal">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Disability / दिव्यांगता"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblDisability" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Disability Type / दिव्यांगता प्रकार"></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblDisabilityType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label30" runat="server" Text="Date of Birth / जन्म दिनांक  (dd-MMM-yyyy)"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblDob" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label88" runat="server" Text="Occupation / व्यवसाय "></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblOccuption" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="labelad" runat="server" Text="Aadhar Card Number / आधार कार्ड संख्या">
                                </asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="lbladhar" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labelapaar" runat="server" Text="Apaar ID / अपार आईडी">
                                </asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="lblApaar" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="Troccupation1" runat="server" visible="false" class="normal">
                            <td>Department / विभाग
                            </td>
                            <td>
                                <asp:Label ID="lbdepartment" runat="server"></asp:Label>
                            </td>
                            <td>Employee Code / कर्मचारी कोड
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="lbempcode" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="Troccupation2" runat="server" visible="false" class="normal">
                            <td>Designation / पद
                            </td>
                            <td>
                                <asp:Label ID="lbldesg" runat="server"></asp:Label>
                            </td>
                            <td>Place of Posting / पोस्टिंग की जगह
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="lblplace" runat="server">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr id="Troccupation3" runat="server" visible="false" class="normal">
                            <td>Date of Joining / शामिल होने की तारीख
                            </td>
                            <td>
                                <asp:Label ID="lbldjoin" runat="server">
                                </asp:Label>
                            </td>
                            <td>Date of Retirement / सेवानिवृत्ति की तारीख
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="lbldretment" runat="server">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="2">3. Contact Details / संपर्क विवरण
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label35" runat="server" Text="Phone with STD code / दूरभाष एस टी डी कोड सहित "></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblLandLine" runat="server"></asp:Label>
                                <br />
                            </td>
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
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblEmail" runat="server"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="2">4. Address Details /&nbsp; पता विवरण
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label32" runat="server" Text="Address Line1/पता पंक्ति 1 "></asp:Label>
                            </td>
                            <td colspan="3" class="rightBorder">
                                <asp:Label ID="LblAddressLine1" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td class="normal">
                                <asp:Label ID="Label20" runat="server" Text="Address Line2/पता पंक्ति 2"></asp:Label>
                            </td>
                            <td class="rightBorder" colspan="3">
                                <asp:Label ID="LblAddressLine2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label22" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                            </td>
                            <td colspan="3" class="rightBorder">
                                <asp:Label ID="LblAddressLine3" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label96" runat="server" Text="City Name/शहर का नाम"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblCity" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label78" runat="server" Text="District / जिला  "></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblDistrict" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:Label ID="Label79" runat="server" Text="State / राज्य "></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblState" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label38" runat="server" Text="Pin Code / पिन  कोड  "></asp:Label>
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblPinCode" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="2">5. Educational / Qualification Details / शैक्षिक / योग्यता का विवरण
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="normal" runat="server">
                            <td>
                                <asp:Label ID="Label49" runat="server" Text="Highest Educational Qualification / उच्चतम शैक्षिक योग्यता"></asp:Label>
                            </td>
                            <td colspan="3" class="rightBorder">
                                <asp:Label ID="LblHeighEducation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" runat="server">
                            <td>
                                <asp:Label ID="Label65" runat="server" Text="Year of  Passing / उत्तीर्ण वर्ष"></asp:Label>
                            </td>
                            <td colspan="3" class="rightBorder">
                                <asp:Label ID="LblYearofPassing" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="head1" id="trattestedheder" runat="server">
                            <td colspan="2">6.Enclosures / भेजें
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="normal" id="trattested" runat="server">
                            <td colspan="4" class="rightBorder">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                                Attested copies of educational qualifications of the candidate (अभ्यर्थी की शैक्षिक
                            योग्यता प्रमाणपत्र की सत्यापित प्रतिलिपि )
                            </td>
                        </tr>
                        <%-- <tr class="normal">
                        <td colspan="4" class="rightBorder">
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                            Attested copy of age proof certificate of the candidate ( अभ्यर्थी के आयु प्रमाणपत्र
                            की सत्यापित प्रतिलिपि )
                        </td>
                    </tr>--%>
                        <tr class="normal" id="TrDD" runat="server">
                            <td colspan="4" class="rightBorder">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                                Please attach Demand Draft with the application form and write Application Number,
                            Name, Course applied for at the reverse side of the draft.(कृपया आवेदन पत्र के साथ
                            डिमांड ड्राफ्ट संलग्न करें, इसके पीछे की ओर निम्नलिखित विवरण लिखें: आवेदन संख्या,
                            अभ्यर्थी का नाम और पाठ्यक्रम जिसके लिए आवेदन किया )
                            </td>
                        </tr>
                        <tr class="head1">
                            <td id="tddeclaration" runat="server">7. Declaration / घोषणा
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="4" class="rightBorder" style="text-align: justify; border-left: 1px solid black; border-bottom: 1px solid black;">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                                I<asp:Label ID="Lblsalutation" runat="server" Text=""></asp:Label>
                                <asp:Label ID="LblDecMName" runat="server" Text=""></asp:Label>
                                <asp:Label ID="LblDecFname" runat="server" Text=""></asp:Label>
                                hereby declare, that, the particulars submitted by me in the online examination 
                             application form of&nbsp;<asp:Label ID="lbldeccoursecode" runat="server" Text=""></asp:Label>
                                are true to the best of my knowledge and belief. I agree to abide by the rules and regulations of NIELIT 
                             and also to the decision of NIELIT regarding my admission for the examination. I have noted that, NIELIT 
                             has the right to withhold my result even after my appearing in the examination in addition to any other 
                             action as may be deemed fit in the event of any of the statements/particulars made above being found 
                             incorrect. I have noted that, I might be required to appear in the examination at any other examination 
                             centre not specified under examination centre choice column (1). I further understand that, in 
                             the event of non-conduction / non-evaluation of my examination due to any reason whatsoever, 
                             I will not held NIELIT responsible for any damages.
                            <br />
                                मैं,<asp:Label ID="LblhDecmname" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Lblhdecfathername" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Lblhsalutation" runat="server" Text=""></asp:Label>
                                यह घोषणा<asp:Label ID="Lblhdectype" runat="server" Text=""></asp:Label>हूँ कि 
                            <asp:Label ID="lblhdeccoursecode" runat="server" Text=""></asp:Label>
                                परीक्षा हेतु ऑनलाइन आवेदन फार्म में मेरे द्वारा वर्णित समस्त जानकारी मेरे ज्ञान और
                            विश्वास से सत्य है। मैं रा.इ.सू.प्रौ.सं. के नियमों व विनियमों तथा मेरी परीक्षा हेतु
                            प्रवेश से संबंधित रा.इ.सू.प्रौ.सं. के निर्णयों से भी सहमत हूँ। मुझे ज्ञात है कि
                            रा.इ.सू.प्रौ.सं. को उपर्युक्त उल्लिखित किसी भी प्रकार का कथन/ विवरण असत्य पाये जाने
                            पर परीक्षा में मेरी उपस्थिति के पश्चात भी मेरा परिणाम रोके जाने के अतिरिक्त अन्य
                            कोई कार्रवाई जो उचित समझी जाए, अधिकार है। मुझे ज्ञात है कि मेरी परीक्षा केंद्र चयन
                            कॉलम(1) के अंतर्गत किसी भी अन्य परीक्षा केंद्र में करायी जा सकती है। आगे,
                            मैं यह भी समझता हूँ कि किसी भी प्रकार के कारणवश मेरी परीक्षा के अनिर्धारण/अवमूल्यांकन
                            की स्थिति में, मेरी किसी प्रकार की क्षति के लिए रा.इ.सू.प्रौ.सं.उत्तरदायी नहीं होगा।
                            </td>
                        </tr>
                        <tr class="head1" id="TrPaymentDetail" runat="server">
                            <td align="left" valign="top" colspan="4" id="tdpdetail" runat="server">8. Payment Detail
                            </td>
                        </tr>
                        <tr class="normal" id="TrPaymentModeTransactionNo" runat="server">
                            <td align="left" valign="top">Fee Details:
                            </td>
                            <td align="left" valign="top" colspan="3" class="rightBorder">
                                <asp:Label ID="LblFeeType" runat="server"></asp:Label>
                                <asp:Label ID="LblFeeAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrPaymentMode" runat="server">
                            <td align="left" valign="top">Payment Mode:
                            </td>
                            <td>
                                <asp:Label ID="LblPaymentMode" runat="server"></asp:Label>
                            </td>
                            <td align="left" valign="top">Payment Transaction No./भुगतान संख्या
                            </td>
                            <td class="rightBorder">
                                <asp:Label ID="LblPaymentSrc" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrPaymentModeInstruction" runat="server">
                            <td align="left" valign="top" colspan="4" class="rightBorder">
                                <asp:Label ID="LblPaymentDescription" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrPayment_not" runat="server">
                            <td align="left" valign="top" colspan="4" class="rightBorder">&nbsp;<strong>Note</strong> * Please don't send this form without making payment./कृपया
                            इस फार्म को भुगतान के बिना नहीं भेजें
                            </td>
                        </tr>
                        <tr class="normal" id="TrInstitute" runat="server">
                            <td align="left" valign="top" colspan="4" class="rightBorder" id="TdInstituteInfo"
                                runat="server"></td>
                        </tr>
                        <tr class="normal" id="TrPaymentMsg" runat="server">
                            <td align="left" valign="top" colspan="4" class="rightBorder">&nbsp;After making payment/depositing fees please submit this form with required
                            attachments to the following Address:
                            <br />
                                <asp:Label ID="LblRegionalAddressE" runat="server"></asp:Label>
                                शुल्क जमा करने के बाद आवश्यक संलग्नकों के साथ यह प्रपत्र निम्न पते पर जमा करें:
                            <br />
                                <asp:Label ID="LblRegionalAddressH" runat="server"></asp:Label>
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
                                <img src="../images/thumb.jpg" id="imgThumbImpression" style="height: 50px; width: 168px;"
                                    runat="server" />
                            </td>
                            <td align="center" colspan="2" valign="middle" width="20%">
                                <img id="imgBarCode" runat="server" />
                            </td>
                            <td align="center" width="40%">
                                <img src="../images/sign.jpg" id="imgSignature" style="height: 35px; width: 168px"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Left hand thumb impression / बाएं हाथ के अंगूठे का निशान
                            </td>
                            <td>&nbsp;
                            </td>
                            <td align="center">Signature of Applicant/ हस्ताक्षर
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="text-align: center;">
            <asp:Button ID="Btnsubmit" runat="server" Text="Final Submit" OnClick="Btnsubmit_Click" />
            <asp:Button ID="BtnBack" runat="server" Text="Back" Width="50px"
                OnClick="BtnBack_Click" Style="height: 26px" />
        </div>
        <br />
    </form>
</body>
</html>
