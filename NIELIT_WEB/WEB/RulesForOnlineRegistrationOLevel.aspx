<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RulesForOnlineRegistrationOLevel.aspx.cs"
    Inherits="RulesForOnlineRegistrationOLevel" Debug="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .sample3
        {
            width: 98%;
        }
        input[disabled="disabled"]
        {
            color: Gray;
        }
        .auto-style1 {
            height: 5px;
        }
    </style>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (!ischecked("chk1", "Declaration"))
                return false;

        }
    </script>
</head>
<body style="background-color: White;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%; text-align: left;">
        <table align="center" class="sample3" width="98%" cellpadding="3" cellspacing="0">
            <tr>
                <td align="center" colspan="2">
                    <br />
                    &nbsp;<asp:Label ID="Label1" align="Center" runat="server" Text="" Style="font-size: 16pt;
                        font-weight: bold;"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    <strong style="font-size: large;" class="header">Step By Step Instructions For Filling
                        <asp:Label ID="Lblctype" runat="server" Text=""></asp:Label>Form</strong>
                    <%--<strong style="font-size: large;" class="header">Step By Step Instructions For Filling
                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>Form</strong>--%>
                    <%--<td class="header" width="70%">
                    Step By Step Process For Application Form:-
                </td>--%>
                </td>
                <td valign="top" align="right" width="30%">
                    <table>
                        <tr>
                            <td>
                                <img src="../images/pdf.jpg" style="height: 21px; width: 31px;" />
                            </td>
                            <td>
                                <a id="link" runat="server" target="_blank" style="vertical-align: top;">Download Brochure</a>
                            </td>
                        </tr>
                        <tr id="trsyllabus" runat="server" visible="false">
                            <td>
                                <img src="../images/pdf.jpg" style="height: 21px; width: 31px;" />
                            </td>
                            <td>
                                <a id="link1" runat="server" target="_blank" style="vertical-align: top;">Download Syllabus</a>
                            </td>
                        </tr>
                        <tr id="trinstructions" runat="server" visible="false">
                            <td>
                                <img src="../images/pdf.jpg" style="height: 21px; width: 31px;" />
                            </td>
                            <td>
                                <a id="link2" runat="server" target="_blank" style="vertical-align: top;"><span id="sptext" runat="server"></span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <tr class="gdalternate1">
                        <td colspan="5" style="height: 300px; border: 1px solid black; vertical-align: top;">
                            <strong style="font-size: large;" class="header">Step By Step Instructions For Filling
                                <asp:Label ID="Lblctype" runat="server" Text=""></asp:Label>Form</strong>
                                <span>&nbsp;&nbsp; <a href="RulesForOnlineRegistration.aspx?ID=<%=Request.QueryString["id"]%>&type=H">Show in Hindi</a></span> 
                                <span>&nbsp;&nbsp; <a href="RulesForOnlineRegistration.aspx?ID=<%=Request.QueryString["id"]%>&Type=E">Show in English</a></span>
                            <ul id="ulcertificate" runat="server" visible="false" style="text-align: left;">
                                <li>Please read the instructions and procedures carefully before you start filling the
                                    form. </li>
                                <li>Candidates of Certificate Exam have to apply online only.</li>
                                <li>Please ensure your eligibility as per the criteria laid down for Certification Exams.</li>
                                <li>Candidates are allowed to submit only one application form.</li>
                                <li>Multiple applications of a candidate are liable to be rejected.</li>
                                <li>The name in the application form must be the same as registered in class XIIth/Qualifying
                                    Examination.</li>
                                <li>If you are Direct Candidate then send the printed and duly signed application along
                                    with DEMAND DRAFT (if payment selected DRAFT) or Online Payment Slip (if payment
                                    made Online) at the address of the NIELIT Regional Center of your State and if you
                                    are Institutional Candidate then send the printed and duly signed application along
                                    with DEMAND DRAFT at the address of the Institute for which you have applied or
                                    pay fees at your Institute from where you have applied. </li>
                                <li>All correspondence related to Certification Exams should be addressed to the NIELIT
                                    at New Delhi.</li>
                                <li>The application no. printed on the computer generated Confirmation Page must be
                                    mentioned in all such correspondences. It is therefore essential to note down the
                                    application number printed on the Confirmation Page.</li>
                                <li>Application fee once paid is non-refundable.</li>
                            </ul>
                            <ul id="ulcertificateh" runat="server" visible="false" style="text-align: left;">
                                <li>इससे पहले कि आप फार्म भरना शुरू करे निर्देशों और प्रक्रियाओं को ध्यान से पढ़ ले.</li>
                                <li>सर्टिफिकेट परीक्षा के उम्मीदवारों को केवल ऑनलाइन आवेदन करना है.</li>
                                <li>अपनी पात्रता सुनिश्चित करने के प्रति के रूप में प्रमाण पत्र परीक्षा के लिए मापदंड
                                    निर्धारित है.</li>
                                <li>एक उम्मीदवार के एकाधिक अनुप्रयोगों को अस्वीकार किया जा सकता हैं.</li>
                                <li>आवेदन प्रपत्र में नाम XIIth क्वालिफाइंग परीक्षा में पंजीकृत नाम के समान होना चाहिए.</li>
                                <li>यदि आप प्रत्यक्ष उम्मीदवार हैं तो मुद्रित और विधिवत् हस्ताक्षरित आवेदन डिमांड ड्राफ्ट
                                    के साथ भेजें(यदि भुगतान ड्राफ्ट चयनित) या ऑनलाइन भुगतान पर्ची (यदि भुगतान ऑनलाइन
                                    बनाया है) को अपने राज्य के NIELIT क्षेत्रीय केंद्र के पते पर और यदि अगर आप संस्थागत
                                    उम्मीदवार हैं तो डिमांड ड्राफ्ट के साथ मुद्रित और विधिवत् हस्ताक्षरित आवेदन भेजें
                                    अपने संस्थान के पते पर जिसके लिए आपने आवेदन किया है या फीस का भुगतान करे संस्थान
                                    से जहां से आपने आवेदन किया है. </li>
                                <li>प्रमाण पत्र परीक्षा से संबंधित सभी पत्राचार NIELIT को संबोधित किया जाना चाहिए नई
                                    दिल्ली में.</li>
                                <li>आवेदन संख्या.कंप्यूटर जनित पुष्टिकरण पृष्ठ पर मुद्रित किया जाना चाहिए ऐसे सभी पत्राचार
                                    के लिए. इसलिए यह जरूरी है कि आप कंप्यूटर जनित आवेदन संख्या लिख ले.</li>
                                <li>एक बार भुगतान किया आवेदन शुल्क अप्रतिदेय है.</li>
                            </ul>
                            <ul id="ulcourse" runat="server" visible="false" style="text-align: left;">
                                <li>Please read the instructions and procedures carefully before you start filling the
                                    form. </li>
                                <li>Candidates of Course Registration have to apply online only.</li>
                                <li>Please ensure your eligibility as per the criteria laid down for Course Registration.</li>
                                <li>Candidates are allowed to submit only one application form.</li>
                                <li>Multiple applications of a candidate are liable to be rejected.</li>
                                <li>The name in the application form must be the same as registered in class XIIth/Qualifying
                                    Examination.</li>
                                <li>If you are Direct Candidate then send the printed and duly signed application along
                                    with DEMAND DRAFT (if payment selected DRAFT) or Online Payment Slip (if payment
                                    made Online) at the address of the NIELIT Regional Center of your State and if you
                                    are Institutional Candidate then send the printed and duly signed application along
                                    with DEMAND DRAFT at the address of the Institute for which you have applied or
                                    pay fees at your Institute from where you have applied. </li>
                                <li>All correspondence related to Course Registration should be addressed to the NIELIT
                                    at New Delhi. </li>
                                <li>The application no. printed on the computer generated Confirmation Page must be
                                    mentioned in all such correspondences. It is therefore essential to note down the
                                    application number printed on the Confirmation Page.</li>
                                <li>Application fee once paid is non-refundable.</li>
                                <li>An email will be received at the email address provided by you. Open the email and
                                    click on the link to activate your account.</li>
                                <li>Login using the user id and temporary password sent to you.</li>
                                <li>Click on the link Apply Online to Apply for Examination of the course registered
                                    by you.</li>
                                <li>After First Login it is mandatory to change your temporary password.</li>
                                <li>Candidates should invariably inform the NIELIT in case there is a change of address,
                                    as communications to the candidates will be to their addresses given in the Registration
                                    form.</li>
                                <li>Essentially a candidate can register at only one Level at a time and appearing for
                                    an examination, at any other Level, is not permitted.</li>
                                <li>The Registration form duly filled in should be sent to the NIELIT with a photograph
                                    affixed thereon attested by a Gazetted Officer/Panchayat/Bank Officer/Centre Manager
                                    of the institute where the candidate has undergone the accredited course.</li>
                                <li>An attested photocopy of the certificate of highest educational qualification attained
                                    by the candidate should be attached.</li>
                            </ul>
                            <ul id="ulcourseh" runat="server" visible="false" style="text-align: left;">
                                <li>इससे पहले कि आप फार्म भरना शुरू करे निर्देशों और प्रक्रियाओं को ध्यान से पढ़ ले.</li>
                                <li>कोर्स पंजीकरण के उम्मीदवारों को केवल ऑनलाइन आवेदन करना है.</li>
                                <li>अपनी पात्रता सुनिश्चित करने के प्रति के रूप में कोर्स पंजीकरण के लिए मापदंड निर्धारित
                                    है</li>
                                <li>एक उम्मीदवार के एकाधिक अनुप्रयोगों को अस्वीकार किया जा सकता हैं.</li>
                                <li>आवेदन प्रपत्र में नाम XIIth क्वालिफाइंग परीक्षा में पंजीकृत नाम के समान होना चाहिए.</li>
                                <li>यदि आप प्रत्यक्ष उम्मीदवार हैं तो मुद्रित और विधिवत् हस्ताक्षरित आवेदन डिमांड ड्राफ्ट
                                    के साथ भेजें(यदि भुगतान ड्राफ्ट चयनित) या ऑनलाइन भुगतान पर्ची (यदि भुगतान ऑनलाइन
                                    बनाया है) को अपने राज्य के NIELIT क्षेत्रीय केंद्र के पते पर और यदि अगर आप संस्थागत
                                    उम्मीदवार हैं तो डिमांड ड्राफ्ट के साथ मुद्रित और विधिवत् हस्ताक्षरित आवेदन भेजें
                                    अपने संस्थान के पते पर जिसके लिए आपने आवेदन किया है या फीस का भुगतान करे संस्थान
                                    से जहां से आपने आवेदन किया है. </li>
                                <li>कोर्स पंजीकरण से संबंधित सभी पत्राचार NIELIT को संबोधित किया जाना चाहिए नईदिल्ली
                                    में.</li>
                                <li>आवेदन संख्या.कंप्यूटर जनित पुष्टिकरण पृष्ठ पर मुद्रित किया जाना चाहिए ऐसे सभी पत्राचार
                                    के लिए. इसलिए यह जरूरी है कि आप कंप्यूटर जनित आवेदन संख्या लिख ले.</li>
                                <li>एक बार भुगतान किया पंजीकरण शुल्क अप्रतिदेय है.</li>
                                <li>आपके द्वारा प्रदान की गयी ई-मेल पते पर एक ईमेल भेजा जाएगा. ईमेल खोलें और लिंक पर
                                    क्लिक करके अपने खाते को सक्रिय करे.</li>
                                <li>आवेदक प्रयोक्ता आईडी और अस्थायी पासवर्ड का उपयोग करके लॉगिन करे.</li>
                                <li>ऑनलाइन आवेदन लिंक पर क्लिक करके आवेदक उस परीक्षा के लिए आवेदन करने जिस कोर्स के
                                    लिए उसने पंजीकरण किया हैं.</li>
                                <li>प्रथम लॉगइन करने के बाद आप अपने अस्थायी पासवर्ड बदलने के लिए अनिवार्य है.</li>
                                <li>उम्मीदवारों को सदा ही NIELIT को सूचित करना चाहिए यदि उनके पते में परिवर्तन है, उम्मीदवारों
                                    के लिए संचार के रूप में उनके पंजीकरण फार्म में दिए गए पते का उपयोग किया जाएगा.</li>
                                <li>मूलतः एक उम्मीदवार एक बार में केवल एक ही कोर्स स्तर पर पंजीकरण कर सकते है और किसी
                                    अन्य स्तर कि परीक्षा देने कि अनुमति नहीं है.</li>
                                <li>पंजीकरण में विधिवत भरे फार्म और उस पर चिपकी तस्वीर एक राजपत्रित अधिकारी /पंचायत
                                    अधिकारी / बैंक अधिकारी / संस्थान के प्रबंधक जहां से उम्मीदवार ने मान्यता प्राप्त
                                    पाठ्यक्रम किया है/ से अनुप्रमाणित करके NIELIT को भेजी जानी चाहिए.</li>
                                <li>उच्चतम शैक्षिक उम्मीदवार के द्वारा प्राप्त योग्यता के प्रमाण पत्र की एक अनुप्रमाणित
                                    प्रतिलिपि संलग्न की जानी चाहिए.</li>
                            </ul>
                        </td>
                    </tr>
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <tr>
                <td colspan="2">
                    <iframe runat="server" id="ifrmAboutUs" width="100%" height="350px" frameborder='0'
                        marginheight='0' marginwidth='0' scrolling="yes"></iframe>
                </td>
            </tr>
            <tr class="gdrow1" id="trnotification" runat="server" visible="false">
                <td colspan="4" style="font-size: 14px; text-align: justify; text-transform: inherit;">
                    <strong>Note:-</strong> If you are not already registered with NIELIT for O level,
                    please click on the agree and proceed button to apply for the course registration,otherwise
                    login into the online student and information system of NIELIT using your Login-ID
                    and password .If you
                    do not have&nbsp; Login-ID and password to login into the&nbsp; online student and
                    information system of NIELIT but you are registered with NIELIT then&nbsp; register
                    yourself with the online student and information system of NIELIT.
                </td>
            </tr>
            <tr class="gdalternate1">
                <td colspan="4">
                    Declaration / घोषणा
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="left" valign="top" colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: justify;" class="odd1">
                                <asp:CheckBox ID="chk1" runat="server" TabIndex="40" Text="<font color='RED'>*</font>"
                                    align="left" />
                                <span id="spancourse" runat="server" visible="false">
                                I, hereby declare that, I agree to abide by the rules and regulations of NIELIT
                                and also to the decision of the Registration authority, regarding my eligibility
                                for registration at the desired level. I have noted that the Registration Authority
                                has the right to withhold my Registration application or cancel the allotted Registration
                                No. in addition to any other action as may be deemed fit in the event of any of
                                the statements made above being found incorrect.(मैं एतद्धारा घोषणा करता/ करती हूँ
                                कि मुझे वांछनीय स्तर में पंजीकरण हेतु मेरी योग्यता के संबंध में रा.इ.सू.प्रौ.सं.
                                के नियम एवं विनियम तथा पंजीकरण प्राधिकारी का निर्णय भी मान्य है। मुझे सूचित है कि
                                पंजीकरण प्राधिकारी को मेरा पंजीकरण आवेदन रोकने अथवा रद्द करने का अधिकार है। इसके
                                अतिरिक्त उपरोक्त निर्दिष्ट कथन सही न पाए जाने पर किसी भी प्रकार की कार्रवाई करने
                                का अधिकार होगा।)</span> 
                                <span id="spancertificate" runat="server" visible="false">I, hereby declare that, I
                                    agree to abide by the rules and regulations of NIELIT and also to the decision of
                                    the Examination authority, regarding my eligibility for filling the exam form of
                                    <asp:label runat="server" text="" id="lbldeccoursecode"></asp:label>.
                                     I declare that the particulars filled in the exam form are true to the best
                                    of my knowledge & belief. I have noted that the Examination Authority has the right
                                    to withhold my examination application or result, in addition to any other action
                                    as may be deemed fit in the event of any of the statement(s) made by me in the exam
                                    form/above being found incorrect.(मैं एतद्धारा घोषणा करता/करती हूँ कि मुझे 
                                    <asp:label runat="server" text="" id="lblhdeccoursecode"></asp:label>
                                     में परीक्षा आवेदन हेतु मेरी योग्यता के संबंध में रा.इ.सू.प्रौ.सं. के नियम एवं विनियम
                                    तथा परीक्षा प्राधिकारी का निर्णय मान्य है। मैं घोषणा करता/ करती हूँ कि परीक्षा फार्म
                                    में मेरे द्वारा भरी गई जानकारी मेरे ज्ञान और विश्वास के अनुसार सही हैं. मुझे सूचित
                                    है कि परीक्षा प्राधिकारी / रा.इ.सू.प्रौ.सं को मेरा परीक्षा आवेदन और परिणाम रोकने
                                    अथवा रद्द करने का अधिकार है। इसके अतिरिक्त मेरे द्वारा परीक्षा आवेदन फार्म में भरी
                                    जानकारी / उपरोक्त निर्दिष्ट कथन सही न पाए जाने पर मेरे ऊपर किसी भी प्रकार की कार्रवाई
                                    करने का अधिकार रा.इ.सू.प्रौ.सं को होगा।)
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style1">
                </td>
            </tr>
            <tr>
                            
                      
                            <td align="left" class="login_txt">
                                 Enter Captcha
                                <asp:TextBox ID="txtcode" runat="server" Width="175px" MaxLength="6" autocomplete="off"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;"></asp:TextBox>
                            </td>
                        </tr>
            
                        <tr>
                            <td class="login_txt">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate> Captcha Code
                                        <img src="" id="imgcap" runat="server" alt="Capture Code" width="155" height="35" />
                                        <asp:ImageButton ID="ImgBtnRefresh" ImageUrl="~/images/refresh.jpg" runat="server"
                                            CausesValidation="false" Width="25px" Height="35" Style="vertical-align: top;
                                            padding-top: 0px; border: none;" OnClick="ImgBtnRefresh_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
             <tr>
                        <td>
                            <asp:Label ID="lblError" runat="server" ForeColor="#CC0000"></asp:Label>
                           
                        </td>
                    </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="I Agree & Proceed" TabIndex="42" class="odd"
                        OnClick="btnSave_Click" OnClientClick="return validate();" />
                    <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="41" class="odd" OnClick="btnback_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px;">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
