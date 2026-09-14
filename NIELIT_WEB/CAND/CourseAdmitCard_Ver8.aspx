<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CourseAdmitCard_Ver8.aspx.cs" Debug="true"
    Inherits="CourseAdmitCard" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style4
        {
            text-decoration: underline;
        }

        .style7
        {
            text-decoration: underline;
            font-size: small;
            font-style: italic;
        }
        .newStyle1
        {
            font-family: "Times New Roman", Times, serif;
        }
        .newStyle2
        {
            font-family: "times New Roman", Times, serif;
        }
        .auto-style5
        {
            font-family: "times New Roman", Times, serif;
            width: 487px;
        }
        .auto-style6
        {
            width: 1%;
        }
        .newStyle3
        {
            font-family: "times New Roman", Times, serif;
        }
        .newStyle4
        {
            font-family: "times New Roman", Times, serif;
        }
        .newStyle5
        {
            font-family: "times New Roman", Times, serif;
        }
        .auto-style7
        {
            height: 46px;
        }
        .auto-style8
        {
            width: 1%;
            height: 37px;
        }
        .auto-style9
        {
            height: 37px;
        }
        .auto-style10
        {
            width: 1%;
            height: 39px;
        }
        .auto-style11
        {
            height: 39px;
        }
        .auto-style12
        {
            width: 1%;
            height: 18px;
        }
        .auto-style13
        {
            height: 18px;
        }
        .auto-style14
        {
            height: 18px;
            text-decoration: underline;
        }
        .auto-style15
        {
            height: 29px;
        }
        .auto-style16
        {
            height: 18px;
            font-weight: bold;
        }
        </style>
</head>
<body style="background-color: #FFFFFF;">
    <form id="form1" runat="server" style="margin-top: 10px;">
        <table align="center" border="0" cellspacing="0" style="font-size: 14px; font-family: Arial;"
            width="958px">
            <tr class="normal">
                <td style="border: 0;"></td>
                <td style="border: 0;"></td>
                <td style="border: 0;"></td>
            </tr>
            <tr>
                <td colspan="3">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="center" valign="top" class="logo">
                                <img runat="server" id="imgLogo" src="~/App_Themes/Blue/Images/Logo.jpg" alt="NIELIT" />
                            </td>
                            <td align="center" class="heading">
                                <div id="tdHeaderBig" runat="server">
                                </div>
                                <div id="tdHeaderSmall" runat="server" class="heading_small">
                                </div>
                                <div id="tdheaderaddress" runat="server" style="font: normal 11px  verdana; color: #000000; padding: 2px 0 0 2px; width: 100%; text-align: center; margin-right: 45px;">
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" valign="middle" colspan="3">
                    <asp:imagebutton id="BtnPrint" runat="server" style="float: right; padding: 0;" onclientclick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                        imageurl="~/images/print.gif" tooltip="Print Form" />
                </td>
            </tr>
            <tr class="normal">
                <td colspan="3" align="center" style="padding: 5px;">
                    <asp:label id="Lbename" width="45%" font-bold="true" font-size="13pt" style="text-align: center;" runat="server" text=""></asp:label>
                    <span style="width: 110px; float: right; font-size: 12px;">Date: <%=DateTime.Now.ToString("dd-MMM-yyyy") %></span>
                </td>
                
            </tr>
            <tr class="normal">
                <td colspan="3" align="center" style="padding: 5px;">
                     <span style="width: 110px; float: right; font-size: 12px;">Version : 1.8</span>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="border: 0px;">
                    <table width="100%" cellspacing="0" cellpadding="3" border="1px">
                        <tr>
                            <td width="8%" class="auto-style15">
                                <asp:label id="Label70" runat="server" text="ROLL NO" font-bold="true"></asp:label>
                            </td>
                            <td width="10%" class="auto-style15">
                                <asp:label id="Lblrno" font-size="Large" font-bold="true" runat="server" text=""></asp:label>
                            </td>
                            <td width="4%" class="auto-style15"></td>
                            <td width="8%" class="auto-style15">
                                <asp:label id="Label5" runat="server" text="REG NO" font-bold="true"></asp:label>
                            </td>
                            <td width="10%" class="auto-style15">
                                <asp:label id="Lbregno" font-size="Large" font-bold="true" runat="server" text=""></asp:label>
                            </td>
                            <td colspan="2" valign="top" class="auto-style15">
                                <asp:label id="Label1" runat="server" text="Office Reference No:" style="font-weight: 700"></asp:label>
                            </td>
                            <td colspan="2" class="auto-style15">
                                <asp:label id="lboffrefno" runat="server" text=""></asp:label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" colspan="5">
                                <asp:label id="Lbcname" runat="server" text=""></asp:label>
                                <br />
                                <asp:label id="lbstudaddress" runat="server" text=""></asp:label>
                            </td>
                            <td width="15%">
                                <strong>LEVEL</strong>
                            </td>
                            <td width="15%">
                                <asp:label id="Lblevel" runat="server" text=""></asp:label>
                            </td>
                            <td width="15%">
                                <strong>MEDIUM </strong>
                            </td>
                            <td width="15%">
                                <asp:label id="Lblmedium" runat="server" text=""></asp:label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="padding-left: 4px; height: 60px;">
                                <strong>Venue Code :- </strong>
                                <asp:label id="Lbccode" runat="server" text=""></asp:label>
                                <br />
                                <strong>Address :- </strong>
                                <asp:label id="lblexamaddress" runat="server" text=""></asp:label>
                            </td>
                            <td  colspan="2" rowspan ="4" align="center" height="100px">
                                 <asp:image id="ImgCandidatePhoto" width="200px" height="220px" runat="server" style="text-align: center;"
                                    imagealign="Middle" />
                                <br />
                               <%-- <asp:image id="ImgCandidatesignature" width="200px" height="35px"
                                    runat="server" style="text-align: center;" imagealign="Middle" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" style="font-size: 12px; font-weight: bold; padding: 1px;"
                                valign="top" align="center">PAPER CODE(S) / MODULES APPEARING FOR
                            </td>
                           
                        </tr>
                        <tr>
                            <td style="font-size: 11px; font-weight: bold; font-family: Sans-Serif;" colspan="7" align="left">On the basis of your online Examination Application form you have been provisionally
                                permitted to appear in the following module(s) / paper(s)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" align="left" style="padding: 0px; border: 0px; vertical-align: top;">
                                <div id="divReportData" runat="server" style="width: 100%;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                           
                            <td colspan="10" valign="top" style="padding-left: 5px;font-weight:bold">* No Request regarding Change of Examination Centre OR Schedule will be entertained.</td>
                        </tr>
                        <tr>
                           
                            <td colspan="10" valign="top" style="padding-left: 5px;">“It is mandatory to bring this Admit Card along with your valid original “Registration Allocation-cum-Identity
                            Card” issued by NIELIT to the Examination Hall” at the venue, the address of which
                            is given above additionally along with any other valid original<%--In case of non-receipt of your “Registration Allocation-cum-Identity
                            Card’, you are requested to carry an alternate--%> Photo Identity Card, such as Driving
                            License, Passport, PAN Card, etc., to the Examination Centre,as proof of your identity.NIELIT reserves the right to cancel the examination ( partially or fully ) of candidate ,without assigning any reason therof .<br />
                            </td>
                        </tr>
                   
                        <tr>
                            <td colspan="9" style="color: black; font-size: 12px; padding-left: 4px;"
                                align="left">
                                <u><strong>Note:-</strong>
                                    Date(s) for Practical Examination shall be notified separately.
                                Also Admit card for Practical Examination shall be issued to all eligible applicants
                                separately.
                            
                           <%-- Practical Examination shall commence from 2nd Saturday of
                            Feb/Aug--%>
                                    <asp:label id="Lbexamyear" runat="server" text="" visible="false" style="color: #000000;"></asp:label>
                                    <%-- &nbsp;and admit card for the same shall be issued Online to all eligible application separately.--%></u>
                            </td>
                        </tr>
                           <tr>
                          <td colspan="7" style="color: black; font-size: 12px; padding-left: 4px;">
                              *This is electronically generated admit card ,hence does not require signature.
                          </td>
                           <td valign="top" colspan="3">
                             Controller of Examinations
                            </td>
                      </tr>
                    </table>
                </td>
            </tr>
            <tr class="normal">
                <td colspan="3" align="center">
                    <strong>
                        <br />
                  रा.इ.सू.प्रौ.सं. की परीक्षा में परीक्षार्थियों द्वारा अनुपालन हेतु अनुदेश  / INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 0px; font-size: 11px;">
                    <table width="100%" cellpadding="1" cellspacing="0" border="0">
                        <tr>
                            <td style="text-align: left; width: 50%; vertical-align: top;">
                                <table style="height: 2326px">
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;
                                        </td>
                                        <td style="text-align: left; width: 1%; vertical-align: top;">1.</td>
                                        <td style="vertical-align: top; text-align: justify;" width="48%">
                                            &nbsp;<span class="style4">सामान्य 
                                            <br />
                                            </span>यह निर्देश आपको रा.इ.सू.प्रौ.सं. की परीक्षाओं को ठीक से लिखने में मदद करेंगे। दिये गए निर्देशों को ध्यानपूर्वक पढ़ें और पालन की जाने वाली प्रक्रियाओं को समझें । </strong> 
</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">2.</td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            <span class="style4">&nbsp;विवरण </span> 
                                            <br />
आपका अनुक्रमांक, नाम और इस परीक्षा के जिन विषयों में ,आपको सम्मिलित होना है ,आपके प्रवेश पत्र पर मुद्रित हैं।  प्रवेश पत्र में कोई भी विसंगति होने पर, तुरंत रा.इ.सू.प्रौ.सं. से संपर्क करें। 
  </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">3.</td>
                                        <td style="vertical-align: top; text-align: justify;" class="style4">
                                            अनुसूची और कक्ष में अनुशासन   </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">3.1</td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            परीक्षा केंद्र पर गतिविधियों की अनुसूची नीचे दी गई है: -
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center; font-size: 10px;">
                                            <table border="1" cellspacing="0" cellpadding="0" width="100%" style="border-collapse: collapse; line-height: 14px; height: 339px; border-left-style: none; border-left-color: inherit; border-left-width: 0;">
                                                <tr>
                                                    <td style="text-align: center;" width="60%" class="auto-style7">
                                                       <strong> क्रियाकलाप </strong> 
                                                    </td>
                                                    <td style="text-align: center;" width="20%" class="auto-style7">
                                                        <strong>पूर्वान्ह सत्र <br />
                                                            
                                                            (9:30-12:30)</strong>
                                                    </td>
                                                    <td style="text-align: center;" width="20%" class="auto-style7">
                                                        <strong>दोपहर सत्र <br />
                                                            
                                                            (1400-1700)</strong>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">प्रवेश का समय 
                                                    </td>
                                                    <td>08:30 IST
                                                    </td>
                                                    <td>13:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">गेट बंद होने का समय (विलंबतम समय जिसके बाद उम्मीदवार को परीक्षा केंद्र में प्रवेश करने की अनुमति नहीं है)
                                                    </td>
                                                    <td>09:15 IST
                                                    </td>
                                                    <td>13:45 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">बैठने की व्यवस्था के अनुसार परीक्षार्थी का बैठना  
                                                    </td>
                                                    <td>09:15 IST
                                                    </td>
                                                    <td>13:45 IST</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">प्रश्न पत्रों और भाग 1 ओएम्आर शीट का वितरण (ए स्तर {A5-10.x-R5}, 'बी' स्तर के पहले 10 विषय) और उत्तर पुस्तिका (बी स्तर और सी स्तर के शेष विषय के विषय के लिए लागू)। </td>
                                                    <td>09:25 IST</td>
                                                    <td>13:55 IST</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">परीक्षा आरम्भ होना 
                                                    </td>
                                                    <td>09:30 IST
                                                    </td>
                                                    <td>14:00 IST
                                                    </td>
                                                </tr>

                                                <%-- <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    The latest time after which a candidate is not permitted to enter the Examination
                                                    Hall
                                                </td>
                                                <td>
                                                    10:00 IST
                                                </td>
                                                <td>
                                                    14:30 IST
                                                </td>
                                            </tr>--%>

                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">परीक्षार्थी द्वारा भाग 1 उत्तर पत्रक (ओएम्आर शीट) प्रस्तुत करने का अंततम समय , जो भी लागू हो|
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">भाग- II की उत्तर पुस्तिकाओं का वितरण (ए स्तर के अंतिम 6 विषयों के लिए लागू तथा 'बी' स्तर के पहला 10 विषयों)
                                                    </td>
                                                    <td>10:30 IST*
                                                        <br />
                                                    </td>
                                                    <td>15:00 IST*
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">उपस्थिति पत्रक में विवरण की रिकॉर्डिंग 
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">निरीक्षक द्वारा एडमिट कार्ड पर हस्ताक्षर किया जाना 
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">अभ्यर्थी के परीक्षा कक्ष जल्दी छोड़ने की  अनुमति हेतु समय  (बिना प्रश्न पत्र के)
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">अभ्यर्थी के परीक्षा कक्ष  जल्दी छोड़ने की अनुमति हेतु समय(प्रश्न पत्र के साथ) 
                                                    </td>
                                                    <td>11:30 IST
                                                    </td>
                                                    <td>16:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">परीक्षा का समापन  (ओ/ए1-ए4)
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>16:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">परीक्षा का समापन  (ए5-ए10.5 & बी/सी)
                                                    </td>
                                                    <td>12:30 IST
                                                    </td>
                                                    <td>17:00 IST
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">*</td>
                                        <td style="vertical-align: top; text-align: justify;">जैसे ही उम्मीदवार भाग I पूरा करता है, वह भाग- I उत्तर पत्रक निरीक्षक को सौंपने के बाद ही निरीक्षक से भाग II के लिए उत्तर पुस्तिका प्राप्त कर सकता/सकती है।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;">3.2</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षा अधीक्षक / प्रेक्षक / उड़न  के पास परीक्षा हॉल से एक उम्मीदवार को निष्कासित करने की पूर्ण शक्तियाँ हैं, यदि, उनकी राय में, उम्मीदवार ने अनुचित साधनों को अपनाया है या हॉल अनुशासन में गड़बड़ी की है।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">3.3</td>
                                        <td style="vertical-align: top; text-align: justify;">पॉकेटबुक, हैंडबैग, किताबें, नोट्स, लिखित / हस्तलिखित या मुद्रित सामग्री, सीडी, नोटबुक, इलेक्ट्रॉनिक्स उपकरण, स्मार्ट घड़ी, सेलुलर / मोबाइल फोन, कैलकुलेटर, पेजर, संचार उपकरण आदि या कोई अन्य समान इलेक्ट्रॉनिक गैजेट / सामग्री या किसी प्रकार का डेटा आदि  परीक्षा केंद्र के अंदर ले जाने की अनुमति नहीं है।परीक्षा केंद्र के अंदर यदि आपके कब्जे में ऐसी कोई वस्तु (वस्तुओं) पायी जाती  हैं, तो उन्हें जब्त कर लिया जा सकता है और उन्हें कदाचार / अनुचित साधन माना जाएगा और आपकी परीक्षा रद्द करने सहित आपके खिलाफ उचित कार्रवाई की जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style8"></td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style9">3.4</td>
                                        <td style="vertical-align: top; text-align: justify;" class="auto-style9">आपको प्रश्न पत्र पर अपनी अनुक्रमांक संख्या, ओएमआर शीट संख्या और उत्तर पुस्तिका संख्या लिखना होगा। प्रश्न पत्र पर और कुछ न लिखें।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style10"></td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style11">3.5</td>
                                        <td style="vertical-align: top; text-align: justify;" class="auto-style11">उत्तर पुस्तिका के भीतर अपना उत्तर पूरा करने के लिए उम्मीदवार को केवल एक उत्तर पुस्तिका प्रदान की जाएगी। अतिरिक्त/अनुपूरक शीट/अतिरिक्त उत्तर पुस्तिका उपलब्ध नहीं कराई जाएगी।
आपको केवल उत्तर पुस्तिका में दिए गए पृष्ठों में ही उत्तर देना होगा। इसलिए, उत्तर देते समय संक्षिप्त रहें। स्पष्ट और सुपाठ्य लिखें और अनुक्रम बनाए रखें।
</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">3.6</td>
                                        <td style="vertical-align: top; text-align: justify;"> यदि उत्तर पुस्तिका में संलग्न एक से अधिक उत्तर पुस्तिका/अतिरिक्त पत्रक किसी रोल नंबर के सापेक्ष पाए जाते हैं, तो उत्तर पुस्तिका को मान्य नहीं माना जाएगा और मॉड्यूल में शून्य (0) अंक प्रदान किया जाएगा ।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">3.7</td>
                                        <td style="vertical-align: top; text-align: justify;"> केवल नीले / काले बॉल प्वाइंट पेन की अनुमति है। उत्तर पुस्तिका में केवल काले या नीले बॉल प्वाइंट पेन का ही प्रयोग करें। फ़्लोचार्ट/आरेख, यदि कोई हो, को छोड़कर, ओएमआर शीट या उत्तर पुस्तिका पर कहीं भी उत्तर लिखने/किसी भी जानकारी को भरने के लिए पेंसिल का उपयोग न करें।</td>
                                    </tr>
                                    <%--<tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="vertical-align: top; text-align: justify;">&nbsp;</td>
                                    </tr>--%>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style12"></td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style13">4.</td>
                                        <td style="vertical-align: top; text-align: justify;" class="auto-style14">ओएमआर पत्रक में  उत्तर चिन्हित  करने के लिए अनुदेश </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">4.1</td>
                                        <td style="vertical-align: top; text-align: justify;">वस्तुनिष्ठ प्रकार के प्रश्नों का उत्तर लिखते समय अनुमान न लगाएं। वस्तुनिष्ठ प्रकार के प्रश्नों में निम्नलिखित में से एक या अधिक शामिल हो सकते हैं: क) बहुविकल्प, ख) सही /गलत, ग)कॉलम मिलान , घ) रिक्त स्थान भरें। यदि आपकी राय में एक से अधिक सही उत्तर दिए गए हैं तो सबसे उपयुक्त विकल्प चुनें। </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">4.2</td>
                                        <td style="vertical-align: top; text-align: justify;">प्रथम भाग में एक से अधिक विकल्प वाले प्रश्नों के उत्तर प्रश्न पत्र के साथ दी गई भाग 1 ओएमआर शीट में उचित बॉक्स को शेडिंग करके मार्क किये जाने हैं। </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">4.3</td>
                                        <td style="vertical-align: top; text-align: justify;">मार्क काला होना चाहिए और सर्किल को पूर्णतः भरना चाहिए।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            <asp:Image ID="Image2" runat="server" imagealign="Right" imageurl="~/images/admitformHindi.jpg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">4.4</td>
                                        <td style="vertical-align: top; text-align: justify;">उम्मीदवार को सावधानीपूर्वक विचार करने के बाद ही अपने उत्तर को चिह्नित करना चाहिए, क्योंकि एक बार चिह्नित होने के बाद उत्तर को बदलना संभव नहीं है। उम्मीदवार को केवल एक सर्किल में शेड करना चाहिए। यदि एक से अधिक सर्किल में शेड किया जाता है, तो उसे गलत उत्तर माना जाएगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">4.5</td>
                                        <td style="vertical-align: top; text-align: justify;">काटना / मिटाना / व्हाइट फ्लुइड / पेंसिल उपयोग करने की अनुमति नहीं है। यदि ऐसा पाया जाता है तो इसे अनुचित साधनों का उपयोग माना जाएगा और मॉड्यूल में शून्य (0) अंक प्रदान किया जाएगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">5.</td>
                                        <td style="vertical-align: top; text-align: justify;">आपको यह ध्यान रखना चाहिए कि, असाधारण परिस्थितियों में यह संभव है कि विभिन्न मॉड्यूल के लिए परीक्षा केंद्र अलग-अलग स्थानों पर हो। यह प्रवेश पत्र पर इंगित किया जाएगा या नाइलिट के नियंत्रण से बाहर की स्थिति में, केवल नाइलिट की वेबसाइट (www.nielit.gov.in) पर घोषित/प्रकाशित किया जाएगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">6.</td>
                                        <td style="vertical-align: top; text-align: justify;">उम्मीदवारों को यह सुनिश्चित करना चाहिए कि, वे परीक्षा में उपस्थित होने के प्रमाण के रूप में अपने अनुक्रमांक, नाम के सामने ही उपस्थिति पत्रक पर हस्ताक्षर करें। यदि आप उपस्थिति पत्रक पर हस्ताक्षर करने में विफल रहते हैं तो आपकी उत्तर पुस्तिका का मूल्यांकन नहीं किया जाएगा और आपको मॉड्यूल में "अनुपस्थित" के रूप में चिह्नित किया जाएगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">7.</td>
                                        <td style="vertical-align: top; text-align: justify;">प्रैक्टिकल परीक्षा के लिए अलग से सूचना  ऑनलाइन प्रकाशित किया जाएगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">8.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों को किसी अन्य मॉड्यूल / पेपर के लिए उपस्थित होने की अनुमति नहीं है, सिवाय उसके जिसका उल्लेख उसके एडमिट कार्ड या नाइलिट्ट  द्वारा जारी किए गए एक विशेष अनुमति पत्र के माध्यम से किया गया है।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">9.</td>
                                        <td style="vertical-align: top; text-align: justify;"><strong>परीक्षा की डेट शीट रा.इ.सू.प्रौ.सं. की वेबसाइट पर उपलब्ध है। कृपया अद्यतन सूचना के लिए https://www.nielit.gov.in पर वेबसाइट को देखें।</strong></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">10.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों से अपेक्षा है कि वे रा.इ.सू.प्रौ.सं., स्थानीय प्राधिकारियों, राज्य सरकार और भारत सरकार द्वारा विशेषतः कोविड-19 के संबंध में समय –समय पार जारी मौखिक एवं लिखित नियमों, विनियमों और दिशानिर्देशों का पालन करें ।  </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">11</td>
                                        <td style="vertical-align: top; text-align: justify;">कोविड-19 से संबंधित स्व-घोषणा एंट्री-एग्जिट सूची में प्रदान की गई है और परीक्षार्थियों को परीक्षा केंद्र में प्रवेश करने से पहले और परीक्षा केंद्र से बाहर निकलते समय एंट्री-एग्जिट सूची पर अनिवार्य रूप से घोषित एवं हस्ताक्षर करना होगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">12.</td>
                                        <td style="vertical-align: top; text-align: justify;">प्रवेश के समय भीड़ न हो, इसलिए परीक्षार्थियों को परीक्षा केंद्र पर रिपोर्ट करने के लिए आवंटित समय के अनुसार रिपोर्ट करना आवश्यक है।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">13.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों को परीक्षा शुरू होने से अधिकतम 60 मिनट पहले परीक्षा केंद्र में प्रवेश दिया जाएगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">14.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों को गेट बंद करने के समय के बाद परीक्षा केंद्र में प्रवेश करने की अनुमति नहीं दी जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">15.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों को परीक्षा केंद्र में प्रवेश करने के बाद जल्द से जल्द परीक्षा हॉल में अपनी सीट पर बैठ जाना चाहिए और आपसी दूरी को बनाए रखने के लिए इधर –उधर न घूमें।  </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">16.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षा आरम्भ होने के बाद परीक्षार्थियों को परीक्षा हॉल में प्रवेश की अनुमति नहीं दी जायेगी। </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">17.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों को यह सुनिश्चित करना होगा कि वह उस पाठ्यक्रम के लिए पंजीकरण करते समय छात्र ऑनलाइन पोर्टल पर उनके द्वारा अपलोड किए गए हस्ताक्षर के अनुसार ही परीक्षा केंद्र में उपस्थिति पत्रक पर अपना हस्ताक्षर करें। नाइलिट अपने विवेकाधिकार से आपके रिकॉर्ड की जांच कर सकता है और यदि उपर्युक्त दो दस्तावेजों पर हस्ताक्षर में कोई बेमेल पाया जाता है, तो आपकी परीक्षा रद्द होने  की संभावना है ।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">18.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षार्थियों को पंजीकरण सह-पहचान पत्र एवं प्रवेश पत्र के साथ अपना मूल फोटो पहचान प्रमाण साथ ले जाना चाहिए, जिसके न होने पर, परीक्षार्थियों को परीक्षा में शामिल होने की अनुमति नहीं दी जाएगी। मान्य फोटो पहचान प्रमाण - मतदाता पहचान पत्र, पासपोर्ट, पैन कार्ड, स्थायी लैमिनेटेड ड्राइविंग लाइसेंस, आधार कार्ड, मान्यता प्राप्त स्कूल / कॉलेज / आईटीआई / पॉलिटेक्निक द्वारा जारी किए गए फोटो के साथ छात्र पहचान पत्र, केंद्रीय / राज्य द्वारा जारी क्रम संख्या के साथ फोटो पहचान पत्र, पीएसयू / स्वायत्त निकायों द्वारा जारी किए गए फोटो पहचान पत्र, बैंक अधिकारी / प्रबंधक द्वारा सत्यापित ग्राहक की तस्वीर और हस्ताक्षर के साथ राष्ट्रीयकृत बैंक पासबुक, लैमिनेटेड फोटोग्राफ के साथ बैंकों द्वारा जारी किए गए क्रेडिट कार्ड, लेटरहेड पर राजपत्रित अधिकारी या तहसीलदार द्वारा जारी किए गए फोटो पहचान पत्र हैं ।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">19.</td>
                                        <td style="vertical-align: top; text-align: justify;">उम्मीदवारों को रा.इ.सू.प्रौ.सं. द्वारा जारी एडमिट कार्ड, पंजीकरण आवंटन-सह-पहचान पत्र, एक मूल फोटो पहचान पत्र, पेन, निजी उपयोग के लिए साबुन / हाथ-सेनिटाईजार (पारदर्शी बोतल में 50 मिलीलीटर तक ), फेस मास्क, दस्ताने और पारदर्शी बोतल में पानी साथ लाना चाहिए। यहाँ लिखित मदों से अलावा अन्य कोई भी वस्तु परीक्षा केंद्र के अंदर लाने की अनुमति नहीं होगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">20.</td>
                                        <td style="vertical-align: top; text-align: justify;">उम्मीदवारों को सलाह दी जाती है कि वे अपने साथ परीक्षा केंद्र में मूल्यवान वस्तुओं को न लायें क्योंकि ऐसी वस्तुओं को सुरक्षित रखने की व्यवस्था सुनिश्चित नहीं की जा सकती है।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">21.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षा हॉल में प्रवेश करने के बाद और परीक्षा सत्र शुरू होने से पहले उम्मीदवार को स्मृति से कोई भी नोट बनाने की अनुमति नहीं होगी। यदि आपके पास ऐसी कोई वस्तु पाई जाती है, तो उसे जब्त कर लिया जाएगा और इसे कदाचार/अनुचित साधन माना जाएगा और आपकी परीक्षा रद्द करने सहित आपके खिलाफ उचित कार्रवाई की जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">22.</td>
                                        <td style="vertical-align: top; text-align: justify;">आपको किसी भी चाकू (ब्लेड के साथ किसी भी पॉकेटनाइफ या मल्टीटूल ब्लेड्स सहित), या किसी भी तरह के किसी भी हथियार को परीक्षा केंद्र में लाने से प्रतिबंधित किया जाता है। यदि आपके पास ऐसी कोई वस्तु पाई जाती है, तो उसे जब्त कर लिया जाएगा और इसे कदाचार/अनुचित साधन माना जाएगा और आपकी परीक्षा रद्द करने सहित आपके खिलाफ उचित कार्रवाई की जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">23.</td>
                                        <td style="vertical-align: top; text-align: justify;">कोई भी अभ्यर्थी ध्वनि या अन्य गतिविधि नहीं करेगा, जो किसी अन्य अभ्यर्थी  की एकाग्रता को बाधित करता है और न ही कोई अभ्यर्थी  किसी भी ऐसी गतिविधि में संलग्न होगा, जिससे शांति भंग हो सकती है।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">24.</td>
                                        <td style="vertical-align: top; text-align: justify;">उम्मीदवार को परीक्षा स्टाफ सहित किसी के भी खिलाफ मौखिक रूप से या अन्यथा गाली-गलौज / अपमानजनक भाषा का उपयोग और धमकी देने वाला / हिंसा का व्यवहार नहीं करना चाहिए। यदि आप इस तरह की गतिविधि में लिप्त पाए जाते हैं, तो इसे कदाचार / अनुचित साधन माना जाएगा और आपकी परीक्षा रद्द करने सहित आपके खिलाफ उचित कार्रवाई की जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">25.</td>
                                        <td style="vertical-align: top; text-align: justify;">कोई भी उम्मीदवार परीक्षा संचालन के दौरान किसी अन्य आवेदक या स्रोत को  सहायता नहीं देगा और ना ही प्राप्त  करेगा। यदि आप इस तरह की गतिविधि में लिप्त पाए जाते हैं, तो इसे कदाचार / अनुचित साधन माना जाएगा और आपकी परीक्षा रद्द करने सहित आपके खिलाफ उचित कार्रवाई की जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">26.</td>
                                        <td style="vertical-align: top; text-align: justify;">परीक्षा केंद्र पर और परीक्षा हॉल के अंदर धूम्रपान या चबाने वाला तंबाकू या शराब/ नशीले पदार्थों का सेवन सख्त वर्जित है। परीक्षा के दौरान ऐसा करने वाले अभ्यर्थी, परीक्षा केंद्र अधीक्षक द्वारा परीक्षा केंद्र से निष्कासित किए जाने के लिए उत्तरदायी होंगे। एक अभ्यर्थी, यदि धूम्रपान या चबाने वाले तंबाकू के नशे में या नशीले पेय / ड्रग्स / पदार्थ / शराब के प्रभाव में पाया जाता है, तो उसे परीक्षा हॉल में प्रवेश करने की अनुमति नहीं दी जाएगी और यदि वह परीक्षा में उपस्थित पाया गया तो उसे परीक्षा अधीक्षक द्वारा परीक्षा हॉल से तुरंत निष्कासित किया जाएगा। यदि आप इस तरह की गतिविधि में लिप्त पाए जाते हैं, तो इसे कदाचार / अनुचित साधन माना जाएगा और आपकी परीक्षा रद्द करने सहित आपके खिलाफ उचित कार्रवाई की जाएगी।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">27.</td>
                                        <td style="vertical-align: top; text-align: justify;">यदि कोई भी अभ्यर्थी हॉल अनुशासन का उल्लंघन करता हुआ पाया जाता है और / या अनुचित साधनों का उपयोग करता पाया जाता है, जो प्रत्यक्ष / अप्रत्यक्ष रूप से परीक्षा की शुचिता को भंग करता है, तो ऐसे उम्मीदवार को परीक्षा केंद्र से निष्कासित कर दिया जाएगा और उसके परीक्षा परिणाम को रोक दिया जाएगा और एसओपी के तहत उपयुक्त कार्रवाई की जाएगी। उम्मीदवार द्वारा किए गए अपराध के लिए जुर्माना लगाने के बारे में परीक्षा के एसओपी के अनुसार निर्णय अंतिम और उस पर बाध्यकारी होगा।</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;">28.</td>
                                        <td style="vertical-align: top; text-align: justify;"><strong>अभ्यर्थियों को समय-समय पर नाइलिट द्वारा जारी समय-सारिणी  और निर्देशों का कड़ाई से पालन करने की सलाह दी जाती है।</strong></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style12"></td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style13"><strong>29</strong>.</td>
                                        <td style="vertical-align: top; text-align: justify;" class="auto-style13"><strong>यदि हिंदी संस्करण में कोई भी त्रुटि/ विसंगति पाई जाती है, तो उस अवस्था में अंग्रेजी संस्करण ही मान्य होगा |</strong></td>
                                    </tr>
                                   <!-- <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style12">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style16">30</td>
                                        <td style="vertical-align: top; text-align: justify;" class="auto-style16">अपूर्ण/बेमेल बिंदु VI पर कोई जानकारी नहीं दी गई है, तो ओएमआर शीट का मूल्यांकन नहीं किया जा सकता है और शून्य अंक दिए जाएंगे।</td>
                                    </tr>-->
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style12">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align: left; width: 50%; vertical-align: top;">
                                <table style="height: 2323px">
                                    <tr>
                                        <td style="text-align: left; width: 1%; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">1.
                                        </td>
                                        <td style="vertical-align: top; text-align: justify;" width="48%">
                                            <strong><u>GENERAL</u></strong><br />
                                           These instructions will help you to write NIELIT examinations properly. Read these instructions that follow carefully and understand the procedures to be observed. </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">2.
                                        </td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            <strong><u>PARTICULARS</u></strong><br />
                                            Your Roll number, Name & Subjects, that you are to appear, in this examination are printed on this admit card. In case of any discrepancy in the Admit Card, contact NIELIT immediately. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.
                                        </td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            <strong><u>SCHEDULE AND HALL DISCIPLINE </u></strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.1
                                        </td>
                                        <td style="vertical-align: top; text-align: justify;">The Schedule of activities at the examination center is as given below:- 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" colspan="2">
                                            <table border="1" cellspacing="0" cellpadding="0" width="100%" style="border-collapse: collapse; line-height: 14px; border-left: 0;">
                                                <tr>
                                                    <td style="text-align: center;" width="60%" class="auto-style7">
                                                        <strong>ACTIVITY</strong>
                                                    </td>
                                                    <td style="text-align: center;" width="20%" class="auto-style7">
                                                        <strong>Forenoon<br />
                                                            Session<br />
                                                            (9:30-12:30)</strong>
                                                    </td>
                                                    <td style="text-align: center;" width="20%" class="auto-style7">
                                                        <strong>Afternoon<br />
                                                            Session<br />
                                                            (1400-1700)</strong>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Entry Time
                                                    </td>
                                                    <td>08:30 IST
                                                    </td>
                                                    <td>13:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Gate Closing Time (Latest time after which a candidate is not permitted to enter the examination Centre)</td>
                                                    <td>09:15 IST
                                                    </td>
                                                    <td>13:45 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Seating of candidate as per seating plan 
                                                    </td>
                                                    <td>09:15 IST
                                                    </td>
                                                    <td>13:45 IST</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Distributions of Question Papers and Part I OMR Sheet (A Level {A5-10.x}, first 10 subjects of ‘B’ Level) and Answer book (applicable to remaining subject of B level & C level). </td>
                                                    <td>09:25 IST</td>
                                                    <td>13:55 IST</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Commencement of Examination
                                                    </td>
                                                    <td>09:30 IST
                                                    </td>
                                                    <td>14:00 IST
                                                    </td>
                                                </tr>

                                                <%-- <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    The latest time after which a candidate is not permitted to enter the Examination
                                                    Hall
                                                </td>
                                                <td>
                                                    10:00 IST
                                                </td>
                                                <td>
                                                    14:30 IST
                                                </td>
                                            </tr>--%>

                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Latest time for submission of Part 1 Answer Sheet(OMR Sheet) by the candidates, whatever applicable.
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Distribution of Answer Books for Part-II (applicable for last 6 subjects of A level and first 10 subjects of 'B' level)
                                                    </td>
                                                    <td>10:30 IST*
                                                        <br />
                                                    </td>
                                                    <td>15:00 IST*
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Recording of particulars in the Attendance Sheet
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Signing on Admit Card by the Invigilator
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Earliest time a candidate is permitted to leave the Examination Hall(without Question
                                                    Paper)
                                                    </td>
                                                    <td>10:30 IST
                                                    </td>
                                                    <td>15:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Earliest time a candidate is permitted to leave the Examination Hall( with Question
                                                    Paper)
                                                    </td>
                                                    <td>11:30 IST
                                                    </td>
                                                    <td>16:00 IST
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Completion of Examintion
                                                        (O/A1-A4)</td>
                                                    <td>&nbsp;</td>
                                                    <td>16:00 IST
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td style="text-align: left; padding-left: 5px;">Completion of Examintion (A5-A10.5 & B/C)</td>
                                                    <td>12:30 IST</td>
                                                    <td>17:00 IST
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">*
                                        </td>
                                        <td style="vertical-align: top; text-align: justify;">As soon as the candidate completes Part I, he/she can collect the answer book for Part II from the Invigilator only after handing over the Part-I Answer Sheet. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.2
                                        </td>
                                        <td style="vertical-align: top; text-align: justify;">The Examination Superintendent/ Observer/ Flying Squad has absolute powers to expel you from the examination hall, if, in their opinion, you have adopted unfair means, or have disturbed the hall discipline. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.3</td>
                                        <td style="vertical-align: top; text-align: justify;">Pocketbooks, handbags, books, notes, written / handwritten or printed material, CDs, notebooks, electronics devices, smart watches, cellular/ Mobile phones, calculators, Pagers, communication device etc. or any other similar electronic gadgets/material or data of any kind etc. are not permissible inside the Examination Centre. If you are found in possession of any such item(s) inside the Examination Centre, they may be confiscated and will amount to misconduct / Unfair Means and appropriate action will be taken against you including cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.4</td>
                                        <td style="vertical-align: top; text-align: justify;">You must write your Roll Number, OMR Sheet Number and Answer Booklet Number on Question Paper. DO NOT WRITE ANYTHING ELSE ON QUESTION PAPER.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.5</td>
                                        <td style="vertical-align: top; text-align: justify;">Only one Answer Book will be provided to the candidate to complete their Answer within Answer Book. Extra/ Supplementary sheets/ additional Answer Book shall not be provided. You have to answer only in the pages provided in the answer book, so be concise while answering. Write clearly and legibly and maintain the sequence.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.6</td>
                                        <td style="vertical-align: top; text-align: justify;">If more than one answer book/Extra sheets attached to the answer book is found against any roll number, the answer book will be treated as not valid and zero (0) mark shall be awarded in the module.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">3.7</td>
                                        <td style="vertical-align: top; text-align: justify;">Only Blue/Black Ball Point pens are allowed. Use ONLY BLACK OR BLUE BALL POINT PEN in the Answer Book. DO NOT use pencil to write answers/fill any information anywhere on the OMR Sheet or Answer Book, except to draw flowcharts/diagrams, if any.</td>
                                    </tr>
                                    <%--<tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="vertical-align: top; text-align: justify;">&nbsp;</td>
                                    </tr>--%>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">4</td>
                                        <td style="vertical-align: top; text-align: justify; text-decoration: underline;">INSTRUCTION FOR MARKING THE ANSWERS on OMR SHEET</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">4.1</td>
                                        <td style="vertical-align: top; text-align: justify;">While answering objective type of questions avoid guesswork. Objective type of questions may consist of one or more of the following: a) Multiple Choice b) True/False c) Matching Columns d) Fill in the Blanks. Choose the most appropriate option if in your opinion more than one correct option is available in the options.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">4.2</td>
                                        <td style="vertical-align: top; text-align: justify;">The answers to the multiple choice questions, in the Part1 are to be marked by shading, the appropriate circle against the question number on OMR Sheet, which is supplied with Question Paper. 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;
                                        </td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">4.3</td>
                                        <td style="vertical-align: top; text-align: justify;">Shading to mark the answer should be DARK and should completely fill the circle.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            <asp:Image ID="Image1" runat="server" ImageAlign="Right" ImageUrl="~/images/admitform.jpeg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">4.4</td>
                                        <td style="vertical-align: top; text-align: justify;">You must mark your response after careful consideration, as it is not possible to change the response once it is marked. You must shade only one circle. If more than one circle is shaded, it will be not be evaluated and zero marks shall be awarded. </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">4.5</td>
                                        <td style="vertical-align: top; text-align: justify;">Cutting/Erasing/Overwriting/Correction or Use of White Fluid/Pencil is strictly NOT permitted. If found this will be treated as use of Unfair Means and zero (0) mark shall be awarded in the module.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">5.</td>
                                        <td style="vertical-align: top; text-align: justify;">You must take note that, in exceptional circumstances it is possible that for different modules the examination center may be in different locations. This will be indicated on the ADMIT CARD or in situation beyond control of NIELIT, shall be announced/published on the NIELIT website (www.nielit.gov.in) only. </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">6.</td>
                                        <td style="vertical-align: top; text-align: justify;">You should ensure that, you sign on the attendance sheet against your roll number & name only as a proof of having attended the examination. If you fail to sign the attendance sheet your answer book will not be evaluated and you shall be marked “Absent” in the module.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">7.</td>
                                        <td style="vertical-align: top; text-align: justify;">For Practical Examination separate communication will be published online.  </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">8.</td>
                                        <td style="vertical-align: top; text-align: justify;">You are not permitted to appear for any other module other than those, which are mentioned in your Admit Card or by way of a special permission letter issued by NIELIT to you. </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">9.</td>
                                        <td style="vertical-align: top; text-align: justify;">
                                            <strong>DATESHEET FOR EXAMINATION IS AVAILABLE AT THE NIELIT WEBSITE. PLEASE CHECK THE WEBSITE AT https://www.nielit.gov.in REGULARLY FOR LATEST UPDATE. </strong></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">10.</td>
                                        <td style="vertical-align: top; text-align: justify;">You are required to follow the instructions (verbal or written), rules, regulations and guidelines of NIELIT; Local Authorities, State Government and Government of India especially related to COVID-19 as issued from time to time.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">11.</td>
                                        <td style="vertical-align: top; text-align: justify;">Self-declaration related to COVID-19 is provided in the Entry-Exit list and you shall have to mandatorily declare and sign the Entry-Exit list before entry into and while exiting the Examination Centre.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">12.</td>
                                        <td style="vertical-align: top; text-align: justify;">You are required to report at the Examination Centre strictly as per the reporting time allotted to you to maintain staggered entry.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">13.</td>
                                        <td style="vertical-align: top; text-align: justify;">You shall be admitted to the Examination Centre maximum 60 minutes before the commencement of Examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">14.</td>
                                        <td style="vertical-align: top; text-align: justify;">You shall not be permitted to enter the Examination Centre after Gate Closing Time.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">15.</td>
                                        <td style="vertical-align: top; text-align: justify;">You are expected to take seat in the Examination Hall as soon as possible after entering the examination center and not move around to maintain social distancing.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">16.</td>
                                        <td style="vertical-align: top; text-align: justify;">You will not be permitted to enter inside the examination hall after the time of commencement of examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">17.</td>
                                        <td style="vertical-align: top; text-align: justify;">You must ensure that you append your signature on the attendance sheet at the examination center in the same manner as uploaded by you at the Student Online Portal while registering for the particular course. NIELIT at its own discretion may check your record and if any mismatch in the signatures on the above said two documents is found, there is a possibility of cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">18.</td>
                                        <td style="vertical-align: top; text-align: justify;">You must carry your Original Photo Identity Proof along with Admit Card and Registration allocation-cum-Identity Card failing which; you will not be allowed to appear in the examination. The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent Laminated Driving License, Aadhaar card, Student identity card with photograph issued by recognized School/ College/ ITI/ Polytechnic, Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/ Autonomous bodies, Nationalized bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">19.</td>
                                        <td style="vertical-align: top; text-align: justify;">You should carry admit card issued by NIELIT, Registration allocation-cum-Identity Card, an original photo identity card, pen, soap/hand-sanitizer of up to 50 ml in transparent bottle, face mask, gloves and water in transparent bottle for personal use. No other item other than specifically mentioned here shall be permissible inside the examination Centre.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">20.</td>
                                        <td style="vertical-align: top; text-align: justify;">You are advised not to bring any valuable items to the examination Centre as arrangement for safe keeping of such items cannot be assured.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">21.</td>
                                        <td style="vertical-align: top; text-align: justify;">You will not be allowed to make any notes from memory once you enter the examination hall and prior to the start of an examination session. If you are found with any such items, they will be confiscated and will amount to misconduct / Unfair Means and appropriate action will be taken against you including cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">22.</td>
                                        <td style="vertical-align: top; text-align: justify;">You are prohibited from bringing any weapon of any kind including any knife (including any pocketknife or multi-tool with blades), or into the examination Centre. If you are found with any such items, they will be confiscated and will amount to misconduct / Unfair Means and appropriate action will be taken against you including cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">23.</td>
                                        <td style="vertical-align: top; text-align: justify;">You shall not create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">24.</td>
                                        <td style="vertical-align: top; text-align: justify;">You should not use abusive/derogatory language orally or otherwise/ threatening/ using violence towards/against anyone including Examination Staff. If you are found indulged in such activity, it will amount to misconduct / Unfair Means and appropriate action will be taken against you including cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">25.</td>
                                        <td style="vertical-align: top; text-align: justify;">You shall not give or receive aid from any other applicant or source during the administration of the examination. If you are found indulged in such activity, it will amount to misconduct / Unfair Means and appropriate action will be taken against you including cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">26.</td>
                                        <td style="vertical-align: top; text-align: justify;">Smoking or Chewing Tobacco or use of Alcohol/Intoxicating Substance is strictly prohibited at the Examination Centre or inside Examination Hall. If you are  found doing so during the course of the Examination, you shall be liable, to be expelled from the Examination Centre by the Examination Superintendent. If  you are found smoking or chewing tobacco or under the influence of intoxicated drinks/drugs/substance/alcohol shall not be allowed to enter the examination centre and if found appearing in the examination you shall be expelled from the examination centre immediately by the Examination Superintendent. If you are found indulged in such activity, it will amount to misconduct and appropriate action will be taken against you including cancellation of your examination.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">27.</td>
                                        <td style="vertical-align: top; text-align: justify;">In case you are found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, you shall be expelled from the examination centre and your examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision as per SOP of the Examination in imposing penalty for the offence committed by you shall be final and binding on you.</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">28.</td>
                                        <td style="vertical-align: top; text-align: justify;"><strong>You shall strictly adhere to the  schedule and instructions as issued by NIELIT from time to time. </strong> </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6"><strong>29</strong>.</td>
                                        <td style="vertical-align: top; text-align: justify; font-weight: 700;">In case of any discrepancy found in Hindi Language, English version will be treated as final.</td>
                                    </tr>
                                    <!--<tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6"><strong>30.</strong></td>
                                        <td style="vertical-align: top; text-align: justify; font-weight: 700;">Incomplete/Mismatch No information provided at point VI , then OMR sheet may not be evaluated and Zero marks will be awarded.</td>
                                    </tr>-->
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">&nbsp;</td>
                                        <td style="text-align: left; vertical-align: top;" class="auto-style6">&nbsp;</td>
                                        <td style="vertical-align: top; text-align: justify; font-weight: 700;">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 50%; vertical-align: top;">
                                &nbsp;</td>
                            <td style="text-align: left; width: 50%; vertical-align: top;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table align="center" border="0" cellspacing="0" style="font-size: 14px; font-family: Arial;"
            width="958px">
            <tr>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            
        </table>

    </form>
</body>
</html>
