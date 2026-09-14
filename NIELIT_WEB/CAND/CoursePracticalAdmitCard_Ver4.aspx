<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CoursePracticalAdmitCard_Ver4.aspx.cs"
    Inherits="CAND_CoursePracticalAdmitCard" Debug="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            text-align: center;
        }
        p.MsoNormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:8.0pt;
	margin-left:0in;
	line-height:107%;
	font-size:11.0pt;
	font-family:"Calibri",sans-serif;
	}
        .auto-style14
        {
            height: 22px;
        }
        .auto-style17
        {
            height: 49px;
        }
        .auto-style19
        {
            height: 36px;
        }
        .auto-style20
        {
            height: 35px;
        }
        .auto-style24
        {
            height: 33px;
        }
        .auto-style32
        {
            height: 210px;
        }
        .auto-style35
        {
            height: 31px;
        }
        .auto-style41
        {
            height: 75px;
        }
        .auto-style42
        {
            text-align: center;
            width: 290px;
        }
        .auto-style43
        {
            width: 290px;
        }
        .auto-style44
        {
            height: 21px;
        }
        .auto-style45
        {
            height: 16px;
        }
        .auto-style46
        {
            height: 55px;
        }
        .auto-style47
        {
            height: 43px;
        }
        .auto-style48
        {
            height: 61px;
        }
        .auto-style49
        {
            height: 46px;
        }
        .auto-style51
        {
            height: 47px;
        }
        .auto-style52
        {
            height: 45px;
        }
        .auto-style53
        {
            height: 28px;
        }
        .auto-style54
        {
            height: 34px;
        }
        .auto-style55
        {
            height: 107px;
        }
        .auto-style56
        {
            height: 77px;
        }
        .auto-style57
        {
            height: 25px;
        }
        .auto-style58
        {
            height: 65px;
        }
        .auto-style59
        {
            height: 27px;
        }
        .auto-style60
        {
            height: 17px;
        }
        .auto-style62
        {
            height: 48px;
        }
        .auto-style63
        {
            width: 1%;
            height: 8px;
        }
        .auto-style67
        {
            width: 1%;
            height: 21px;
        }
        .auto-style68
        {
            height: 81px;
        }
        .auto-style69
        {
            height: 66px;
        }
        .auto-style70
        {
            height: 14px;
        }
        .auto-style71
        {
            height: 80px;
        }
        .auto-style77
        {
            height: 40px;
        }
        .auto-style79
        {
            height: 19px;
        }
        .auto-style84
        {
            text-decoration: underline;
        }
        p.Default
	{margin-bottom:.0001pt;
	text-autospace:none;
	font-size:12.0pt;
	font-family:"Arial",sans-serif;
	color:black;
	        margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
        }
        .auto-style89
        {
            height: 8px;
        }
        .auto-style90
        {
            height: 20px;
        }
        .auto-style91
        {
            width: 250px;
        }
        .auto-style94
        {
            width: 2%;
        }
        .auto-style95
        {
            height: 66px;
            width: 2%;
        }
        .auto-style96
        {
            height: 22px;
            width: 2%;
        }
        .auto-style97
        {
            height: 20px;
            width: 2%;
        }
        .auto-style112
        {
            width: 2%;
            height: 8px;
        }
        .auto-style113
        {
            text-align: justify;
            height: 57px;
            width: 513px;
        }
        .auto-style114
        {
            width: 300px;
        }
        .auto-style115
        {
            height: 17px;
            width: 300px;
        }
        .auto-style116
        {
            width: 300px;
            text-align: center;
        }
        .auto-style117
        {
            text-decoration: underline;
            text-align: center;
        }
        .auto-style118
        {
            width: 403px;
        }
        .auto-style119
        {
            width: 114px;
        }
        .auto-style120
        {
            font-weight: bold;
        }
        .auto-style122
        {
            width: 156px;
        }
        .auto-style123
        {
            width: 76px;
        }
        </style>
</head>
<body style="background-color: #FFFFFF;">
    <form id="form1" runat="server" style="margin-top: 10px;">
    <table align="center" border="0" cellspacing="0" style="font-size:16px; font-family: Arial;"
        width="955px">
        <tr class="normal">
            <td style="border: 0;">
            </td>
            <td style="border: 0;">
            </td>
            <td style="border: 0;">
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                       <td align="left" valign="top" class="logo">
                           <img runat="server" id="img1" src="~/App_Themes/Blue/Images/log-2.png" alt="NIELIT" />
                                                    
                       </td>
                        <td align="center" class="heading">
                            <div id="tdHeaderBig" runat="server">
                            </div>
                            <div id="tdHeaderSmall" runat="server" class="heading_small">
                            </div>
                            <div id="tdheaderaddress" runat="server" style="font: normal 11px  verdana; color: #000000;
                                padding: 2px 0 0 2px; width: 100%; text-align: center; margin-right: 45px;">
                            </div>
                             <td align="right" valign="top" class="logo">
                            <img runat="server" id="imgLogo" src="~/App_Themes/Blue/Images/Logo.jpg" alt="NIELIT" />
                        </td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle" colspan="3">
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding: 0;" OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr class="normal">
            <td colspan="3" align="center" style="padding: 5px;">
                <asp:Label ID="Lbename" Width="45%" Font-Bold="true" Font-Size="13pt" Style="text-align: right;"
                    runat="server" Text="प्रायोगिक परीक्षा / PRACTICAL EXAMINATION "></asp:Label>
                <br />
                <asp:Label ID="Label1" Font-Bold="true" Font-Size="13pt" Style="margin-left: 100px;"
                    runat="server" Text="प्रवेश-पत्र / ADMIT CARD Valid for July 2022 Examination Only"></asp:Label>

                <span style="width: 110px; float: right; font-size: 12px;">Date:<%=DateTime.Now.ToString("dd-MMM-yyyy") %>
                </span>
            </td>
           
             
        </tr>
        <tr>
            <td colspan="3" align="center" style="padding: 5px;">
                <span style="width: 110px; float: right; font-size: 10px;">Ver:  1.2
                </span>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="border: 0px;">
                <table width="100%" cellspacing="0" cellpadding="3" border="0" style="font-size:17px;">
                    <tr>
                        <td colspan="1" valign="top" width="30%">
                            <asp:Label ID="Label2" runat="server" Text="कार्यालय संदर्भ / Office Reference:-"
                                Style="font-weight: 700"></asp:Label>
                            &nbsp;<asp:Label ID="lboffrefno" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="2" width="70%">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="top">
                            <asp:Label ID="Label5" runat="server" Text="सेवा में / To," Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="Lbcname" runat="server" Text=""></asp:Label><br />
                            <asp:Label ID="lbstudaddress" runat="server" Text=""></asp:Label>
                        </td>
                        <td rowspan="1" align="right" height="100px">
                            <asp:Image ID="ImgCandidatePhoto_prac" Width="200px" Height="220px" runat="server" Style="text-align: center;"
                                ImageAlign="Middle" /><br /><asp:Image ID="ImgCandidatesignature_prac" Width="200px" Height="35px"
                                    runat="server" Style="text-align: center; margin-left: 0px;" ImageAlign="Middle" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" valign="top" style="text-align: justify;">
                            आपके परीक्षा-प्रपत्र के आधार पर, नीचे दी गई तारीख, समय, एवं स्थान में आयोजित की
                            जाने वाली निम्नलिखित प्रायोगिक परीक्षा मॉड्यूल में आपको उपस्थित होने की अंनतिम रूप
                            से अनुमति दी गई है:
                            <br />
                            On the basis of your examination form, you have been provisionally permitted to
                            appear in the following Practical Examination Module(s) scheduled to be held on
                            the Date, Time and Venue as indicated below:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" valign="top" width="30%">
                            <asp:Label ID="Label3" runat="server" Text="नाम / Name:-" Style="font-weight: 700"></asp:Label>
                        </td>
                        <td colspan="2" width="70%">
                            <asp:Label ID="Lbcname1" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" valign="top" width="40%">
                            <asp:Label ID="Label4" runat="server" Text="पंजीकरण संख्या / Registration Number :-"
                                Style="font-weight: 700"></asp:Label>
                        </td>
                        <td colspan="2" width="60%">
                            <asp:Label ID="Lbregno" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" valign="top" width="40%">
                            <asp:Label ID="Label6" runat="server" Text="स्थान / Venue:-" Style="font-weight: 700"></asp:Label>
                        </td>
                        <td colspan="2" valign="top" width="60%">
                            <asp:Label ID="Lbinstname" runat="server" Text=""></asp:Label><br />
                            <asp:Label ID="Lbinstaddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" style="padding: 0px; border: 1px; vertical-align: top;
                            font-size: 16px;">
                            <br />
                            <div id="divReportData" runat="server" style="width: 100%;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: justify;" valign="top">
                            <br />
                            आपको रा.इ.सू.प्रौ.सं. द्वारा जारी प्रवेश-पत्र एवं पहचान-पत्र के साथ ऊपर उल्लिखित
                            दिनांक एवं समय–स्लॉट को प्रायोगिक परीक्षा स्थान में परीक्षा प्रारंभ होने के कम से
                            कम आधे घंटे पहले रिपोर्ट करना आवश्यक है। रा.इ.सू.प्रौ.सं.प्रायोगिक परीक्षाओं के
                            पृष्ठ की दूसरी ओर प्रायोगिक परीक्षा का पाठ्यविवरण तथा अभ्यर्थियों द्वारा अनुपालन
                            किए जाने वाले अनुदेश उपलब्ध कराए गए है और उम्मीदवार पर बाध्यकारी हैं।
                            <br />
                            You are required to report at the Practical Examination Venue on the Date(s) and
                            Time-Slot as mentioned above along with this Admit Card and the Identity Card issued
                            by the NIELIT at least ONE HOUR before commencement of Practical Exams. Syllabus
                            of Practical Examination and Instructions to be followed by the candidates at NIELIT
                            Practical Examinations are provided overleaf and are  binding on the candidate.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" valign="top" align="right">
                            <asp:Image ID="Imgofficesignature" runat="server" Style="text-align: center; padding-bottom: 2px;
                                padding-top: 2px;" ImageAlign="Middle" Width="100px" Height="30px" ImageUrl="~/images/COESign.jpg" />
                            <br />
                            परीक्षा नियंत्रक 
                            <br />
                            Controller Of Examinations
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" valign="top" align="right">
                            <br />
                            <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
   


        
        <tr>
            <td colspan="3" align="center" valign="top" style="border-bottom: 1px solid #000000;">
                <strong>
               
                रा.इ.सू.प्रौ.सं. प्रायोगिक परीक्षाओं में अभ्यर्थियों द्वारा अनुपालन किए जाने
                    वाले अनुदेश
                    <br />
                    Instructions to be followed by the candidates at NIELIT Practical Examinations
                </strong>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left" valign="top" width="100%" style="font-size: 12px; text-align: justify;">
                अभ्यर्थी परीक्षा के दौरान अनुशासन एवं आचार-व्यवहार से संबंधित निम्नलिखित अनुदेशों
                का कड़ाई से अनुपालन करें। / Candidates must strictly follow the following instructions
                relating to discipline and conduct during practical exam:
            </td>
        </tr>
        <tr>
            <td colspan="3" style="padding: 0px; font-size: 12px;">
                <table width="100%" cellpadding="1" cellspacing="0" border="0">
                    <tr>
                        <td style="text-align: left; width: 100%; vertical-align: top; border-bottom: 1px solid #000000;">
                            <table style="height: 2406px">
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style63">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style63">
                                        1.</td>
                                    <td style="vertical-align: top; text-align: justify;" width="48%" class="auto-style89" >
                                       	<span class="style1"><strong>सामान्‍य 
                                        </strong></span> 
                                        <br />                                      
                                    </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style67">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style112">
                                        1.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;" width="48%" class="auto-style44">
                                        <span class="style1"><strong>General</strong></span>
                                        <br />
                                       
                                    </td>

                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style41">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style41">
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style41">
                                       a) दिये गये निर्देश आपको रा.इ.सू.प्रौ.सं. की परीक्षाओं को ठीक से लिखने में मदद करेंगे। इनको ध्यान से पढ़ें और पालन की जाने वाली प्रक्रियाओं को समझें। <strong>कृपया परीक्षा के दौरान उपस्थिति मय मूल्‍यांकन पत्रक पर उचित स्थान पर अपने हस्ताक्षर करें, अन्यथा आपकी कार्य-  पुस्तिका का मूल्यांकन नहीं किया जाएगा। </strong>
                                    </td>
                                     <td style="text-align: left; vertical-align: top;" class="auto-style68">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style112">
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style68">
                                       a) These instructions will help you to write NIELIT practical examinations properly. Read them carefully and understand the procedures to be followed.
<strong>PLEASE APPEND YOUR SIGNATURES AT APPROPRIATE PLACE IN THE ATTENDANCE-cum-EVALUATION SHEET DURING THE EXAMINATION, OTHERWISE YOUR WORK SHEET WILL NOT BE EVALUTATED. </strong>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style47">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style47">
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style47">
                                       b) अभ्यर्थियों को परीक्षा-कक्ष/प्रयोगशाला से बाहर प्रश्‍न-पत्र मय कार्य-शीट ले जाने / लेने की अनुमति नहीं दी जाएगी। इसे परीक्षा कक्ष/प्रयोगशाला को छोड़ने से पहले प्रायोगिक परीक्षक को वाप‍िस करें। 
                                    </td>

                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        &nbsp;</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        b)Candidates are not allowed to carry/take the Question Paper cum Work-Sheet out of examination hall/lab. The same must be returned to the Practical Examiner before leaving the exam hall/lab.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style46">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style46">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style46">
                                        c)प्रत्येक परीक्षा की समय-अवधि तीन घंटा है। अधिकतम अंक 100 है जिसमें से 80 अंको का प्रायोगिक अभ्यास तथा 20 अंकों की मौखिक परीक्षा है।</td>

                                    <td style="text-align: left; vertical-align: top;" class="auto-style69">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style95">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style69">
                                      c) The duration of each exam is three hours. Maximum Marks are 100, out of which 80 Marks are assigned to Practical Exercises and 20 Marks to Viva Voce.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style89">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style89">
                                        2.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style89">
                                        <strong>रोल नंबर</strong></td>

                                    <td style="text-align: left; vertical-align: top;" class="auto-style14">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style96">
                                        2.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style14">
                                        <strong>ROLL NUMBER</strong></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style71">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style71">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style71">
                                        प्रायोगिक परीक्षा में आपका पंजीकरण नंबर ही आपका रोल नंबर है। आपका रोल नम्‍बर, नाम और विषय, जिसमें आपको इस परीक्षा में शामिल होना है, आपके प्रवेश पत्र पर मुद्रित हैं। अपना रोल नंबर (पंजीकरण नम्‍बर) कार्य पुस्तिका में निर्धारित स्थान पर लिखें। कार्य-पुस्तिका में किसी भी भाग पर आपका नाम नहीं लिखा होना चाहिए। प्रवेश पत्र में कोई भी विसंगति होने पर, तुरंत रा.इ. सू.प्रौ.सं. से ईमेल द्वारा prexam@nielit.gov.in पर संपर्क करें।</td>
                                
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        &nbsp;</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Your Registration Number is your Roll Number in case of Practical examination. Your Roll number, Name & Subject that you are to appear in this examination are printed on your admit card. Write your Roll Number (Regn. No.) in the space provided on the Examination Work-Sheet . No additional sheets shall be provided. Your name should NOT appear in any part of the Examination Work Sheet, in case of any discrepancy in the Admit Card,contact NIELIT immediately by email at prexam@nielit.gov.in.</td>
                                
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style14">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style14">
                                        3.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style14">
                                        <strong>अनुसूची</strong></td>

                                    <td style="text-align: left; vertical-align: top;" class="auto-style90">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style97">
                                        3.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style90">
                                        <span class="style1"><strong>SCHEDULE</strong></span> </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style32">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style32">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style32" >
                                        परीक्षा हॉल में कार्य-पुस्तिकाओं / प्रश्न पत्रों के वितरण और संग्रह और अन्य संबंधित गतिविधियों की अनुसूची नीचे दी गई है:<br />
                                        <br />
                                        <table  border="1" cellpadding="0" cellspacing="0" style="height: 247px"  >
                                            <tr >
                                                <td class="auto-style42"  >
                                                    <strong>क्रियाकलाप
                                                
                                                </strong>
                                                
                                                </td>
                                                <td class="style2" >
                                                    <strong><span class="style1">पू</span>र्वान्हसत्र
                                                        <br />
                                                        (9:30-12:30)<span class="style1">
                                                </span></strong>
                                                </td>
                                                <td class="style2" >
                                                    <strong><span class="style1">दोपहर सत्र
                                                        </span>
                                                        <br class="style1" />
                                                        <span class="style1">(1400-1700)</span></strong>
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style43" >
                                                    प्रवेश का समय
                                                </td>
                                                <td >
                                                    08:30 IST 
                                                </td>
                                                <td >
                                                    13:00 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style43" >
                                                    अंततम समय जिसके बाद परीक्षार्थी को परीक्षा केंद्र में प्रवेश की अनुमति नहीं है (गेट बंद होने का समय)
                                                </td>
                                                <td>
                                                     09:15 IST 
                                                </td>
                                                <td >
                                                     13:45 IST  
                                                </td>
                                            </tr>
                                            <tr  >
                                                <td class="auto-style43"  >
                                                     बैठने की व्यवस्था के अनुसार परीक्षार्थी का बैठना 
                                                </td>
                                                <td  >
                                                    
                                                         09:15 IST  
                                                </td>
                                                <td  >
                                                    13:45 IST 
                                                 </td>
                                            </tr>
                                            <tr  >
                                                <td class="auto-style43"  >
                                                     कार्य पुस्तिका का वितरण 
                                                </td>
                                                <td >                                                                                                           
                                                        09:25 IST                                                  
                                                </td>
                                                <td >
                                                   13:55 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style43" >
                                                   
                                                        प्रश्न पत्र वितरण
                                                </td>
                                                <td >
                                                    
                                                       09:25 IST
                                                </td>
                                                <td>
                                                    13:55 IST
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style43" >
                                                    प्रायोगिक परीक्षा आरम्भ होना
                                                </td>
                                                <td>
                             09:30 IST 
                                                </td>
                                                <td >
                             14:00 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style43" >
                                                   परीक्षा हॉल जल्दी छोड़ने की अनुमति हेतु शीघ्रतम समय
                                                </td>
                                                <td >
                                                    12:00 IST 
                                                </td>
                                                <td >
                                  16:30 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style43" >
                                                    परीक्षा पूर्ण होना
                                                    
                                                </td>
                                                <td >
                                                    12:30 IST 
                                                </td>
                                                <td >
                                                    17:00 IST 
                                                </td>
                                            </tr>
                                        </table>                                                                                                                    
                                    </td>

                                    <td style="text-align: left; vertical-align: top;" class="auto-style13">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style98">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style13" >
                                        The Schedule for distribution and collection of Work Sheets/Question papers and other related activities in the examination hall is as given below:-<br />
                                        <br />
                                        <table border="1" cellpadding="0" cellspacing="0" style="width: 435px; height: 245px"  >
                                            <tr >
                                                <td class="auto-style116" >
                                                    <strong>ACTIVITY
                                                </strong>
                                                </td>
                                                <td class="style2">
                                                    <strong style="text-align: center">Forenoon<br />
                                                        Session<br />
                                                        (9:30-12:30)
                                                </strong>
                                                </td>
                                                <td class="style2"  >
                                                    <strong style="text-align: center">Afternoon<br />
                                                        Session<br />
                                                        (1400-1700)                                                   
                                                    </strong>                                                   
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style114" >
                                                    Entry Time 
                                                </td>
                                                <td class="auto-style88" >
                                                    08:30 IST 
                                                </td>
                                                <td >
                                                    13:00 IST                                                     
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style114" >
                                                     Latest time after which a candidate is not permitted to enter the examination Centre( Gate Closing Time )
                                                </td>
                                                <td class="auto-style88" >
                                                    09:15 IST 
                                                </td>
                                                <td >
                                                   13:45 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style114" >
                                                    Seating of candidate as per seating plan 
                                                </td>
                                                <td class="auto-style88" >
                                                    09:15 IST 
                                                </td>
                                                <td >
                                                    13:45 IST
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style114" >
                                                    Distribution of Work Sheets.
                                                </td>
                                                <td class="auto-style88" >
                                                    09:25 IST 
                                                </td>
                                                <td >
                                                    13:55 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style114" >
                                                    Distribution of Question Papers
                                                </td>
                                                <td class="auto-style88" >
                                                    09:25 IST
                                                <td >
                                                    13:55 IST
                                                </td>
                                            </tr>
                                            <tr  >
                                                <td class="auto-style114" >
                                                    Commencement of Practical Examination
                            
                                                </td>
                                                <td class="auto-style88" >
                                                    09:30 IST 
                                                </td>
                                                <td >
                                                    14:00 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style114" >
                                                    Earliest time a candidate is permitted to leave the Examination Hall 
                                                </td>
                                                <td class="auto-style88" >
                                                    12:00 IST 
                                                </td>
                                                <td >
                                                    16:30 IST 
                                                </td>
                                            </tr>
                                            <tr >
                                                <td class="auto-style115" >
                                                    Completion of Examination 
                                                </td>
                                                <td class="auto-style60" >
                                                    12:30 IST
                                                </td>
                                                <td class="auto-style60" >
                                                    17:00 IST                                                   
                                                </td>
                                            </tr>
                                        </table>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style45">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style45">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style45">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style45">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style45">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style45">
                                        </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style44">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style44">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style44">
                                        <strong><span class="auto-style84">हॉल अनुशासन</span> </strong> </td>

                                     <td style="text-align: left; vertical-align: top;" class="auto-style14">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style96">
                                        </td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style84">
                                        <strong>Hall Discipline</strong></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style62">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style62">
                                        4.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style62">
                                        परीक्षा अधीक्षक / प्रेक्षक / फ्लाइंग स्क्वायड के पास परीक्षा हॉल से उम्मीदवार को निष्कासित करने की पूर्ण शक्तियाँ हैं, यदि, उनकी राय में, उम्मीदवार ने अनुचित साधनों को अपनाया है या हॉल अनुशासन में गड़बड़ी की है।</td>
                                
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        4.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        The Examination Superintendent/Observer/Flying Squad has absolute powers to expel a candidate from the examination hall,if ,in their opinion,the candidate has adopted unfair means,or has disturbed the hall discipline.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style49">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style49">
                                        5.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style49">
                                        परीक्षा हॉल के अन्दर किसी भी तरह के इलेक्ट्रॉनिक्स डिवाइस, स्मार्ट वॉच, पेजर्स, मोबाइल फ़ोन,आदि ले जाने की अनुमति नहीं है। यदि किसी परीक्षार्थी के पास ऐसी वस्तुएं पायी जाती हैं तो उन्हें जब्त किया जा सकता है।</td>
                                                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        5.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Any kind of electronics devices Smart Watches, Pagers, Mobile Phones etc. are not permitted inside the examination hall, if any such items are found in possession of the candidate, they may be confiscated.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style47">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style47">
                                        6.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style47">
                                        अनुपूरक शीट प्रदान नहीं किए जायेंगे। आपको केवल कार्य- पुस्तिका में दिए गए पृष्ठों में ही उत्तर देना होगा। इसलिए, उत्तर देते समय संक्षिप्त रहें। स्पष्ट और सुपाठ्य लिखें और अनुक्रम बनाए रखें।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style74">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style99">
                                        6.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style74">
                                       Extra/Supplementary sheets shall not be provided. You have to answer only in the pages provided in work sheets so be concise while answering. Write clearly and legibly and maintain the sequence.</td>

                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style70">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style70">
                                        7.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style70">
                                        केवल नीले / काले बॉल प्वाइंट पेन की अनुमति है।</td>

                                    <td style="text-align: left; vertical-align: top;" class="auto-style60">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style100">
                                        7.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style60">
                                        Only Blue/Black Ball Point pens are allowed.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        8.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style17">
                                        उम्मीदवारों को यह सुनिश्चित करना चाहिए कि, वे परीक्षा में उपस्थित होने के प्रमाण के रूप में अपने रोल नंबर (पंजीकरण नम्‍बर), नाम के सामने ही उपस्थिति मय मूल्‍यांकन पत्रक पर हस्ताक्षर करें। जो परीक्षार्थी उपस्थिति मय मूल्‍यांकन  पत्रक पर हस्ताक्षर करने में विफल रहते हैं, उनकी कार्य-पुस्तिकाओं का मूल्यांकन नहीं किया जा सकेगा।</td>
                                <td style="text-align: left; vertical-align: top;" class="auto-style78">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style102">
                                        8.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style78">
                                       Candidates should ensure that they sign on the attendance-cum-evaluation sheet only against their roll number, name, as a proof of having attended the examination. The Work Sheets of those candidates who fail to sign the attendance-cum-evaluation sheet,may not be evaluated.</td>
                                     </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style77">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style77">
                                        9.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style77">
                                        परीक्षार्थियों को किसी अन्य प्रायोगिक मॉड्यूल / पेपर के लिए उपस्थित होने की अनुमति नहीं है, सिवाय उसके जिसका उल्लेख उसके एडमिट कार्ड या NIELIT द्वारा जारी किए गए एक विशेष अनुमति पत्र के माध्यम से किया गया है।</td>
                                <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style103">
                                        9.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style17">
                                       Candidates are not permitted to appear for any other practical module/paper,other than those ,which are mentioned in his/her Admit Card or by way of a special permission letter issued by NIELIT.</td>
                                
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style19">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style19">
                                        10.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style19">
                                        प्रायोगिक परीक्षा की डेट शीट रा.इ.सू.प्रौ.सं. की वेबसाइट पर उपलब्ध है। कृपया अद्यतन सूचना के लिए https://www.nielit.gov.in को देखें।</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        10.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Date sheet for Practical Examination is available at the NIELIT Website. Please check the website at https://www.nielit.gov.in REGULARLY FOR LATEST UPDATES.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        11.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style17">
                                        परीक्षार्थियों से अपेक्षा है कि वे रा.इ.सू.प्रौ.सं., स्थानीय प्राधिकारियों, राज्य सरकार और भारत सरकार द्वारा विशेषतः कोविड-19 के संबंध में समय –समय पार जारी मौखिक एवं लिखित नियमों, विनियमों और दिशानिर्देशों का पालन करें ।</td>
                                   <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        11.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Candidates are required to follow the instruction (verbal or written) rules, regulations and guidelines of NIELIT; Local Authorities, State Government and Government of India especially related to COVID-19 as issued from time to time.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style17">
                                        12.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style17">
                                        कोविड-19 से संबंधित स्व-घोषणा एंट्री-एग्जिट सूची में प्रदान की गई है और परीक्षार्थियों को परीक्षा केंद्र में प्रवेश करने से पहले और परीक्षा केंद्र से बाहर निकलते समय एंट्री-एग्जिट सूची पर अनिवार्य रूप से घोषित एवं हस्ताक्षर करना होगा।</td>
                               <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        12.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Self-declaration related to COVID-19 is provided in the Entry/Exit list and candidates shall have to mandatorily declare and sign the Entry/Exit list before entry into and while exiting the examination centre.</td>
                                    
                                     </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style24">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style24">
                                        13.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style24">
                                        प्रवेश के समय भीड़ न हो, इसलिए परीक्षार्थियों को परीक्षा केंद्र पर रिपोर्ट करने के लिए आवंटित समय के अनुसार रिपोर्ट करना आवश्यक है।</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        13.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidates are required to report at the examination centre strictly as per the reporting time allotted to the candidate to maintain staggered entry.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style59">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style59">
                                        14.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style59">
                                        परीक्षार्थियों को परीक्षा शुरू होने से अधिकतम 60 मिनट पहले परीक्षा केंद्र में प्रवेश दिया जाएगा।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style24">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style104">
                                        14.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style24">
                                        Candidates shall be admitted to the Examination Centre maximum 60 minutes before the commencement of Examination.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style60">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style60">
                                        15.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style60">
                                        परीक्षार्थियों को गेट बंद करने के समय के बाद परीक्षा केंद्र में प्रवेश करने की अनुमति नहीं दी जाएगी।</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        15.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Candidates shall not be permitted to enter the Examination Centre after Gate closing time.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style20">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style20">
                                        16.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style20">
                                        परीक्षार्थियों को परीक्षा केंद्र में प्रवेश करने के बाद जल्द से जल्द परीक्षा हॉल में अपनी सीट पर बैठ जाना चाहिए और आपसी दूरी को बनाए रखने के लिए इधर –उधर न घूमें।</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        16.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Candidates are expected to take seat in the Examination Hall as soon as possible after entering the examination centre and not move around to maintain social distancing.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style59">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style59">
                                        17.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style59">
                                        परीक्षा आरम्भ होने के बाद परीक्षार्थियों को परीक्षा हॉल में प्रवेश की अनुमति नहीं दी जायेगी।</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        17.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidates entry will not be permitted inside the examination hall after the time of commencement of examination.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style48">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style48">
                                        18.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style48">
                                        परीक्षार्थियों को यह सुनिश्चित करना होगा कि वह उस पाठ्यक्रम के लिए पंजीकरण करते समय छात्र ऑनलाइन पोर्टल पर उसके द्वारा अपलोड किए गए हस्ताक्षर के अनुसार ही परीक्षा केंद्र में उपस्थिति मय मूल्‍यांकन  पत्रक पर अपना हस्ताक्षर करें। रा.इ.सू.प्रौ.सं.अपने विवेक से किसी भी / सभी परीक्षार्थियों के रिकॉर्ड की जांच कर सकता है और यदि उपरोक्त दो दस्तावेजों पर परीक्षार्थी के हस्ताक्षर मेल नहीं होते हैं तो उस परीक्षार्थी की परीक्षा रद्द की जा सकती है।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style36">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style105">
                                        18.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style36">
                                       Candidate must ensure that he/she appends his/her signature on the attendance cum evaluation sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while registering for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style79">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style79">
                                        19.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style79">
                                        परीक्षार्थियों को पंजीकरण सह-पहचान पत्र एवं प्रवेश पत्र के साथ अपना मूल फोटो पहचान प्रमाण साथ ले जाना चाहिए, जिसके न होने पर, परीक्षार्थियों को परीक्षा में शामिल होने की अनुमति नहीं दी जाएगी। मान्य फोटो पहचान प्रमाण - मतदाता पहचान पत्र, पासपोर्ट, पैन कार्ड, स्थायी लैमिनेटेड ड्राइविंग लाइसेंस, आधार कार्ड, मान्यता प्राप्त स्कूल / कॉलेज / आईटीआई / पॉलिटेक्निक द्वारा जारी किए गए फोटो के साथ छात्र पहचान पत्र, केंद्रीय / राज्य द्वारा जारी क्रम संख्या के साथ फोटो पहचान पत्र, पीएसयू / स्वायत्त निकायों द्वारा जारी किए गए फोटो पहचान पत्र, बैंक अधिकारी / प्रबंधक द्वारा सत्यापित ग्राहक की तस्वीर और हस्ताक्षर के साथ राष्ट्रीयकृत बैंक पासबुक, लैमिनेटेड फोटोग्राफ के साथ बैंकों द्वारा जारी किए गए क्रेडिट कार्ड, लेटरहेड पर राजपत्रित अधिकारी या तहसीलदार द्वारा जारी किए गए फोटो पहचान पत्र।</td>
                                   <td style="text-align: left; vertical-align: top;" class="auto-style38">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style106">
                                        19.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style38">
                                        Candidates must carry their Original Photo Identity Proof along with Admit Card and  Registration allocation-cum-Identity Card failing which; the candidates will not be allowed to appear in the examination. The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadhar card, Student identity card with photograph issued by recognized School/College/ITI/Polytechnic, Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letter head.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style58">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style58">
                                        20.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style58">
                                        उम्मीदवारों को रा.इ.सू.प्रौ.सं. द्वारा जारी एडमिट कार्ड, पंजीकरण आवंटन-सह-पहचान पत्र, एक मूल फोटो पहचान पत्र, पेन, निजी उपयोग के लिए साबुन / हाथ-सेनिटाईजर (पारदर्शी बोतल में 50 मिलीलीटर तक ), फेस मास्क, दस्ताने और पारदर्शी बोतल में पानी साथ लाना चाहिए। यहाँ लिखित मदों से अलावा अन्य कोई भी वस्तु परीक्षा केंद्र के अंदर लाने की अनुमति नहीं होगी।</td>
                                   <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        20.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidates should carry admit card issued by NIELIT, Registration allocation-cum-Identity Card , an original photo identity card, pen, soap/hand-sanitiser of up to 50 ml in transparent bottle, face mask, gloves and water in transparent bottle for personal use. No other item other than specifically mentioned here shall be permissible inside the examination centre.</td>
                                     </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style20">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style20">
                                        21.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style20">
                                        उम्मीदवारों को सलाह दी जाती है कि वे अपने साथ परीक्षा केंद्र में मूल्यवान वस्तुओं को न लायें क्योंकि ऐसी वस्तुओं को सुरक्षित रखने की व्यवस्था सुनिश्चित नहीं की जा सकती है।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style53">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style107">
                                        21.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style53">
                                        Candidates are advised not to bring any valuable items to the examination centre as arrangement for safe keeping of such items cannot be assured.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style54">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style54">
                                        22.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style54">
                                        परीक्षार्थियों के वाहनों की पार्किंग की सुविधा सुनिश्चित / प्रदान नहीं की जा सकती है। इसके लिये परीक्षार्थियों को अपनी स्‍वयं की व्‍यवस्‍था करनी होगी। </td>
                                     <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        22.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        The facility of parking of vehicles of the candidates can not be ensured / provided. Candidates have to make their own arrangements for the same.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style51">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style51">
                                        23.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style51">
                                        आपको पॉकेटबुक, हैंडबैग, किताबें, नोट्स, लिखित या मुद्रित सामग्री, सीडी या किसी भी प्रकार के डेटा आदि को साथ लाना वर्जित है। यदि कोई ऐसी वस्तु उम्मीदवार के कब्जे में पाई जाती है, तो उन्हें जब्त कर लिया जाएगा।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style49">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style108">
                                        23.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style49">
                                        You are prohibited from bringing pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind etc. If any such items are found in possession of the candidate, they will be confiscated.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style52">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style52">
                                        24.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style52">
                                        परीक्षा हॉल में प्रवेश करने के बाद और परीक्षा सत्र शुरू होने से पहले उम्मीदवार को स्मृति से कोई भी नोट बनाने की अनुमति नहीं होगी। यदि कोई उम्मीदवार ऐसी किसी भी वस्तु के साथ पाया जाता है, तो उन्हें जब्त कर लिया जाएगा</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        24.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style53">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style53">
                                        25.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style53">
                                        आपको किसी भी चाकू (ब्लेड के साथ किसी भी पॉकेटनाइफ या मल्टीटूल ब्लेड्स सहित), या किसी भी तरह के किसी भी हथियार को परीक्षा केंद्र में लाना वर्जित है। ।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style83">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style109">
                                        25.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style83">
                                        You are prohibited from bringing any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style47">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style47">
                                        26.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style47">
                                        कोई भी अभ्यर्थी लगातार ध्वनि या कोई ऐसी गतिविधि नहीं करेगा, जो किसी अन्य उम्मीदवार की एकाग्रता को बाधित कर सकती है और न ही कोई उम्मीदवार किसी भी ऐसी गतिविधि में संलग्न होगा, जिससे शांति भंग हो सकती है।</td>
                                   <td style="text-align: left; vertical-align: top;" class="auto-style82">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style110">
                                        26.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style82">
                                        No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style49">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style49">
                                        27.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style49">
                                       उम्मीदवार को परीक्षा स्टाफ सहित किसी के भी खिलाफ मौखिक रूप से या अन्यथा गाली-गलौज / अपमानजनक / असंसदीय-भाषा का उपयोग और धमकी देने वाला / हिंसा का व्यवहार नहीं करना चाहिए।</td>
                               <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        27.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Candidate should not use abusive/derogatory/unparliamentry language orally or otherwise/ threatening/ using violence towards/against anyone including Examination Staff.</td>
                                     </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style35">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style35">
                                        28.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style35">
                                        कोई भी उम्मीदवार परीक्षा संचालन के दौरान किसी अन्य उम्‍मीदवार  या स्रोत से न तो सहायता लेगा और न ही देगा।  </td>
                                     <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        28.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       No candidate shall give or receive aid from any other candidate or source during the administration of the examination.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style55">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style55">
                                        29.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style55">
                                        परीक्षा केंद्र पर और परीक्षा हॉल के अंदर धूम्रपान या चबाने वाला तंबाकू या अल्कोहल / नशीले पदार्थों का सेवन सख्त वर्जित है। परीक्षा के दौरान ऐसा करने वाले अभ्यर्थी, परीक्षा केंद्र अधीक्षक द्वारा परीक्षा केंद्र से निष्कासित किए जाने के लिए स्‍वयं उत्तरदायी होंगे। कोई अभ्यर्थी, यदि धूम्रपान या चबाने वाले तंबाकू के नशे में या नशीले पेय / ड्रग्स / पदार्थ / शराब के प्रभाव में पाया जाता है, तो उसे परीक्षा हॉल में प्रवेश करने की अनुमति नहीं दी जाएगी और यदि वह परीक्षा में उपस्थित पाया गया तो उसे परीक्षा अधीक्षक द्वारा परीक्षा हॉल से तुरंत निष्कासित किया जाएगा। </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        29.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       Smoking or Chewing Tobacco or use of Alcohol/Intoxicating Substance is strictly prohibited at the Examination Centre or inside Examination Hall. Candidates found doing so during the course of the Examination, shall be liable, to be expelled from the Examination Centre by the Examination Superintendent. A candidate, if found smoking or chewing tobacco or under the influence of intoxicated drinks/drugs/substance/alcohol shall not be allowed to enter the examination centre and if found appearing in the examination shall be expelled from the examination centre immediately by the Examination Superintendent.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style56">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style56">
                                        30.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style56">
                                        यदि कोई भी अभ्यर्थी हॉल अनुशासन का उल्लंघन करता हुआ पाया जाता है और / या अनुचित साधनों का उपयोग करता पाया जाता है, जो प्रत्यक्ष / अप्रत्यक्ष रूप से परीक्षा की पुनीतता को भंग करता है, तो ऐसे उम्मीदवार को परीक्षा केंद्र से निष्कासित कर दिया जाएगा और उसके परीक्षा परिणाम को रोक दिया जाएगा और एसओपी के तहत उपयुक्त समझे जाने वाली कार्रवाई की जाएगी। उम्मीदवार द्वारा किए गए अपराध के लिए दण्‍ड लगाने के बारे में परीक्षा के एसओपी के अनुसार निर्णय अंतिम और उस पर बाध्यकारी होगा।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style37">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style111">
                                        30.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style37">
                                        In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, candidate shall be expelled from the examination centre and his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision as per SOP of the Examination in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.</td>
                                    
                                      </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style57">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style57">
                                        31.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style57">
                                        उम्मीदवारों को समय-सारणी और निर्देशों का सख्ती से पालन करने की सलाह दी जाती है।</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style44">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style92">
                                        31.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style44">
                                       Candidates shall have to strictly adhere to the time schedule and instructions.</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style24">
                                        </td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style24">
                                        32.</td>
                                    <td style="vertical-align: top; text-align: justify;" class="auto-style24">
                                        यदि हिंदी संस्करण में कोई भी त्रुटि/ विसंगति पाई जाती है, तो उस अवस्था में अंग्रेजी संस्करण ही मान्य होगा |</td>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;</td>
                                    <td style="text-align: left; vertical-align: top;" class="auto-style94">
                                        32.</td>
                                    <td style="vertical-align: top; text-align: justify;">
                                       In case of any discrepancy found in Hindi Language , English version will be treated as final.</td>
                                </tr>
                                
                            </table>

                        </td>
                        <td style="text-align: left; width: 50%; vertical-align: top; border-bottom: 1px solid #000000;">
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 50%; vertical-align: top; border-bottom: 1px solid #000000;">
                            &nbsp;</td>
                        <td style="text-align: left; width: 50%; vertical-align: top; border-bottom: 1px solid #000000;">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <br />
                <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <br />
                 <span style="text-align:center;font-weight:bold; font-size:12pt; text-decoration:underline;">प्रायोगिक परीक्षा का पाठ्यविवरण / Syllabus of Practical Examinations</span>
                <br />
                <span style="text-align:left; font-weight: bold; font-size:11pt;">प्रश्नपत्र में निम्नलिखित पर आधारित तीन प्रश्न शामिल होंगे / The question paper shall consist of three questions based on the following: </span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="5" cellspacing="0" border="1px" align="center" style="font-size:14px;">
                    <tr id="trolevel" runat="server" valign="top">
                        <td colspan="2" >
                            <span style="font-size: 14px; padding-left: 385px;" class="style1"><strong>`ओ` स्तर
                                / O-Level 
                            <br />
                            </strong></span>
                            <br />
                         </td>
                    </tr>
                    <tr id="trolevel1" runat="server" valign="top">
                        <td class="auto-style117" >
                            <strong>Revision –IV (O-PR-R4)</strong></td>
                        <td class="auto-style117" >
                            <strong>Revision –V</strong></td>
                    </tr>
                    <tr>
                       
                        <td colspan="1" width="45%" valign="top">

                            M1-R4: IT Tools and Business Systems <br />
M2-R4: Internet Technology and Web Design <br />
M3-R4: Programming & Problem Solving through ‘C’ Language <br /><br />

तथा / AND <br /><br />
सैद्धांतिक परीक्षा में अभ्यर्थी द्वारा चुने गए मॉड्यूल पर आधारित निम्नलिखित विकल्पों से केवल एक प्रश्न /<br /><br />
any one question from the following choices based on the module opted by the candidate in the theory exam <br /><br />

                            <span class="auto-style84"><strong>M4.1-R4:</strong></span> Application of .NET Technology / <span class="auto-style84"><strong>M4.2-R4</strong></span>: Introduction to Multimedia / <span class="auto-style84"><strong>M4.3-R4:</strong></span> Introduction to ICT Resources 


                            <br />
                            <br />
                        </td>
                        <td width="55%" valign="top">
                            <%--M1-R5: Information Technology Tools and Network Basics<br />
M2-R5: Web Designing & Publishing<br />
M3-R5: Programming and Problem Solving through Python <br />
M4-R5 Internet of Things and its Applications --%>
                            <table  border="1" cellpadding="0" cellspacing="0" style="height: 84px; width: 504px" >
                                <tr>
                                    <td class="auto-style119">
                                       O-PR-I-R5&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    </td>
                                    <td>
                                        <br />
                                    </td>
                                    <td class="auto-style118">
                                         &nbsp;&nbsp;
                                         M1-R5 Information Technology Tools and Network Basics
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="auto-style119">
                                       O-PR-II-R5&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    </td>
                                     <td>
                                         <br />
                                     </td>
                                    <td class="auto-style118">
                                        &nbsp;&nbsp;
                                        M2-R5 Web Designing & Publishing
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="auto-style119">
                                        O-PR-III-R5&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    </td>
                                     <td>
                                         <br />
                                     </td>
                                    <td class="auto-style118">
                                        &nbsp;&nbsp;
                                        M3-R5 Programming and Problem Solving through Python
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="auto-style119">
                                       O-PR-IV-R5&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    </td>
                                     <td>
                                         <br />
                                     </td>
                                    <td class="auto-style118">
                                        &nbsp;&nbsp;
                                        M4-R5 Internet of Things and its Applications
                                    </td>
                                </tr>
                            </table>                          
                        </td>
                    </tr>
                    <tr id="tralevel" runat="server">
                        <td colspan="1" width="45%" valign="top">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="style1" style="font-size: 14px; padding-left:10px;"><strong style="text-align: center">`ए` स्तर / A-Level <br />
                            <br />
                            <br />
                           
                            </strong>
                           
                            <table border="1" cellpadding="0" cellspacing="0" >
                                <tr >
                                    <td class="auto-style122" >
                                        <b>Revision –IV
                                        ( A-PR-I-R4 / B-PR-I-R4)</b></td>
                                    <td >
                                        
                                        </td>
                                    <td >
                                        <b>Revision -V&nbsp;</b></td>
                                </tr>
                                
                                <tr >
                                    <td class="auto-style122" >A1-R4/B1.1-R4: IT Tools and Business Systems<br /><br />
                                        A2-R4/B1.2-R4: Internet Technology and Web Design  <br /><br />
                                        A3-R4/B1.3-R4: Programming & Problem Solving through ‘C’ Language <br /><br />                                       
                                        A4-R4/B1.4-R4: Computer System Architecture
                                    </td>
                                    <td >
                                       
                                    </td>
                                    <td >
                                        <table  border="1" cellpadding="0" cellspacing="0" >
                                            <tr>

                                                <td class="auto-style123">
                                                    A-PR-I-R5
                                                </td>
                                                <td>
                                                    A1-R5 Information Technology Tools and Network Basics
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="auto-style123">
                                                    A-PR-II-R5
                                                </td>
                                                <td>
                                                   A2-R5 Web Designing and Publishing
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="auto-style123">
                                                    A-PR-III-R5
                                                </td>
                                                <td>
                                                    A3-R5 Programming and Problem Solving Through Python
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="auto-style123">
                                                    A-PR-IV-R5
                                                </td>
                                                <td>
                                                    A4-R5 Internet of Things and Its Applications
                                                </td>
                                            </tr>



                                            </table>
                                       
                                       
                                    </td>
                                </tr>
                            </table>
                            
            
                        </td>
                        <td width="55%" valign="top">
                            <div class="auto-style113">
                            <span class="style1" style="font-size: 14px; padding-left:55px;"><strong>`ए` स्तर / A-Level ( ) &  ‘बी’ स्‍तर / B-Level (B-PR-II-R4)</strong></span><br />
                                <br />
                            <br />
                            </div>
                            <table border="1" cellpadding="0" cellspacing="0">
                                <tr >
                                    <td class="auto-style91" >
                                        <b>Revision –IV&nbsp; (</b><span class="auto-style120" style="font-size: 14px; ">A-PR-II-R4)</span></td>
                                    <td>
                                        
                                    </td>
                                    <td ><b>Revision -V
                                        (A-PR-V-R5 )</b></td>
                                </tr>
                                <tr>
                                    <td class="auto-style91" >
                                        <br />
                                        A5-R4/B1.5-R4: Structured System Analysis & Design   <br /><br /> 
                                          
A6-R4/B2.1-R4: Data Structure through C++ 
 
                                        <br /><br/>
                                         A7-R4/B2.2-R4: Introduction to Data Base Management Systems <br />
                                        <br />
                                        A8-R4/B2.3-R4: Basics of OS, Unix & Shell Programming 
                                        <br />
                                        <br />
                                        
A9-R4/B2.4-R4: Data Communication and Network Technologies 

                                        
                                        <br />
                                        <br />
                                        <span class="auto-style84"><strong>Elective (Any one from the following to be chosen)</strong></span><br />
                                        <br />
                                        <strong><span class="auto-style84">A10.1-R4/B2.5.1-R4:</span> Introduction to Object Oriented Programming Java. <br />
Any one question from the following choices based on the modules opted by the candidate in the theory examinations</strong>. <br /><br />

                                        <strong><span class="auto-style84">A10.2-R4/B2.5.2-R4: </span>Software Testing and Quality Management </strong> <br />
                            

                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td>
                                         &nbsp;A5-R5 Data Structure Through Object Oriented Programming Language <br /> <br />A6-R5: Computer Organization and Operating System <br /> <br />
                                        A7-R5: Database Technologies 
                                        
                                        <br />
                                        <br />
                                        
                                        
                                    </td>
                                </tr>
                                
                            </table>
                            

                        </td>
                    </tr>
                    <tr id="trblevel" runat="server">
                        <td colspan="1" width="45%" valign="top">
                            <span style="font-size: 14px; padding-left: 115px; font-weight: 700; text-decoration: underline;">
                                `बी` स्तर / B-Level (B-PR-III-R4)</span>
                            <br />
                            B3.3-R4: Software Engineering & CASE Tools.
                            <br />
                            B3.4-R4: Operating Systems.
                            <br />
                            B3.5-R4: Visual Programming.
                        </td>
                        <td width="55%" valign="top">
                            <span style="font-size: 14px; padding-left: 175px;" class="style1"><strong>`बी` स्तर
                                / B-Level (B-PR-IV-R4)</strong></span>
                            <br />
                            B4.3-R4: Object Oriented DBMS.
                            <br />
                            B4.4-R4: Computer Graphics & Multimedia
                            <br />
                            B4.5-R4: Internet Technologies and Web Services.
                        </td>
                    </tr>
                    <tr id="trclevel1" runat="server">
                        <td colspan="1" width="45%" valign="top">
                            <span style="font-size: 14px; padding-left: 115px;" class="style1"><strong>`सी` स्तर
                                / C-Level (C-PR-I-R4)</strong></span>
                            <br />
                            C1-R4 : Advanced Computer Graphics.
                            <br />
                            C3-R4 : Mathematical Methods for Computing.
                        </td>
                        <td width="55%" valign="top">
                            <span style="font-size: 14px; padding-left: 175px;" class="style1"><strong>`सी` स्तर
                                / C-Level (C-PR-II-R4)</strong></span>
                            <br />
                            C2-R4 : Advanced Computer Networks.
                            <br />
                            C5-R4 : Data Warehousing and Data Mining.
                        </td>
                    </tr>
                    <tr id="trclevel2" runat="server">
                        <td colspan="1" width="45%" valign="top">
                            <span style="font-size: 14px; padding-left: 115px;" class="style1"><strong>`सी` स्तर
                                / C-Level (C-PR-III-R4)</strong></span>
                            <br />
                            C6-R4 : Multimedia Systems.
                            <br />
                            C7-R4 : Digital Image Processing and Computer Vision.
                        </td>
                        <td width="55%" valign="top">
                            <span style="font-size: 14px; padding-left: 175px;" class="style1">`<strong>सी` स्तर
                                / C-Level (C-PR-IV-R4)</strong></span>
                            <br />
                            C8-R4 : Information Security.
                            <br />
                            C9-R4 : Soft Computing.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
