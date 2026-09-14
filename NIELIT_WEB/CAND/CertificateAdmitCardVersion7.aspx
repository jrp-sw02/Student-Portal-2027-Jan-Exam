<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CertificateAdmitCardVersion7.aspx.cs"
    Inherits="CertificateAdmitCardVersion6" Debug="true" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css" media="print">
        
    </style>
    <style type="text/css">

 p.MsoNormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:8.0pt;
	margin-left:0in;
	line-height:107%;
	font-size:11.0pt;
	font-family:"Calibri",sans-serif;
	}
        .auto-style1
        {
            width: 28%;
        }
    </style>
</head>
<body style="margin: 70px 40px 70px 40px; background-color: #FFFFFF; -webkit-print-color-adjust: exact;">
    <form id="form1" runat="server" style="margin-top: 10px;">
        `<table align="center" border="0" cellpadding="0" cellspacing="0" width="956px">
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
                        <td align="center" valign="top" class="logo">
                            <img runat="server" id="imgLogo" src="~/App_Themes/Blue/Images/Logo.jpg" alt="NIELIT" />
                        </td>
                        <td align="center" class="heading">
                            <div id="tdHeaderBig" runat="server">
                            </div>
                            <div id="tdHeaderSmall" runat="server" class="heading_small">
                            </div>
                            <div id="tdheaderaddress" runat="server" style="font: normal 11px  verdana; color: #000000;
                                padding: 2px 0 0 2px; width: 100%; text-align: center; margin-right: 45px;">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle" colspan="3">
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 20px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr class="normal">
            <td colspan="3" align="center" style="padding: 5px; font-weight: bold;">
                <span>NIELIT '<asp:Label ID="Lblctype" runat="server" Text=""></asp:Label>' EXAMINATION
                </span>
                <br />
                <span>CANDIDATE ADMIT CARD </span>
            </td>
        </tr>
        <tr>
                        <td colspan="3" align="center">
                            VALID FOR
                            <asp:Label ID="Lbename" runat="server" Text=""></asp:Label>&nbsp;EXAMINATION ONLY
                        </td>
                    </tr>
        <tr class="normal">
            <td colspan="3" align="center" style="font-weight: bold;">
                Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="border: 0px;">
                <table width="100%" cellspacing="0" cellpadding="4" border="1px" style="font-size: 16px;">
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="Label70" runat="server" Text="ROLL NO" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lblrno" runat="server" Text="" Font-Size="Large" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 20%; padding: 8px 8px 8px 8px;" align="center" rowspan="13">
                            <img id="imgcandphoto" runat="server" style="height: 347px; width: 235px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="Label2" runat="server" Text="NAME"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbcname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trmothername" runat="server">
                        <td class="auto-style1">
                            <asp:Label ID="Label3" runat="server" Text="MOTHER'S NAME"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trfathername" runat="server">
                        <td class="auto-style1">
                            <asp:Label ID="Label4" runat="server" Text="FATHER'S NAME"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trguardian" runat="server" visible="false">
                        <td class="auto-style1">
                            <asp:Label ID="Label1" runat="server" Text="GUARDIAN NAME"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="Lbgname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 36%">
                           GENDER : 
                        </td>
                        <td style="width: 44%">
                       <asp:Label ID="LblGender" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="Label5" runat="server" Text="EXAM CENTER CODE"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lblexamcode" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            PWD :
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblDisability" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                     
                    <%--<tr>
                        <td colspan="3" align="center">
                            VALID FOR
                            <asp:Label ID="Lbename" runat="server" Text=""></asp:Label>&nbsp;EXAMINATION ONLY
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" align="center">
                            <b><u>BATCH SCHEDULE</u></b>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            EXAM CENTRE CODE:
                            <asp:Label ID="Lbccode" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="1">
                            EXAM DATE :
                            <asp:Label ID="Lbedate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="5" valign="top" class="auto-style1">
                            EXAM CENTRE ADDRESS:
                            <%--  ADDRESS:--%>
                            <br />
                            <asp:Label ID="lblexamaddress" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="1">
                            BATCH :
                            <asp:Label ID="Lblbatchno" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            REPORTING TIME :
                            <asp:Label ID="Lbreporting" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            <b>GATE CLOSING TIME :
                                <asp:Label ID="Lbclosing" runat="server" Text=""></asp:Label><br />
                                No candidate will be allowed to enter the examination center after the gate closing
                                time. </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            EXAM START TIME:
                            <asp:Label ID="Lbreptime" runat="server" Text=""></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            EXAM DURATION :
                            <asp:Label ID="lbminutes" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="3" align="center">
                            <p class="MsoNormal" style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                                <b><span lang="EN-IN" style="font-size:12.0pt;
font-family:&quot;Times New Roman&quot;,serif;mso-fareast-font-family:&quot;Times New Roman&quot;;
mso-ansi-language:EN-IN">IMPORTANT: CANDIDATES ARE ADVISED TO PUT THEIR SIGNATURE ON THE ATTENDANCE SHEET, FAILING WHICH, THEIR CANDIDATURE SHALL NOT BE CONSIDERED FOR EXAM. CANDIDATE MUST CARRY A VALID ORIGINAL PHOTO IDENTITY CARD ALONG WITH THE ADMIT CARD ISSUED BY NIELIT FAILING WHICH THE CANDIDATES WILL NOT BE ALLOWED TO APPEAR IN THE EXAMINATION. <o:p></o:p></span></b>
                            </p>
                            <b>&nbsp;<br />
                                <span>महत्वपूर्ण: अभ्यर्थियों को उपस्थिति-पत्र में अपने हस्ताक्षर करने की सलाह दी जाती है। ऐसा न करने पर, परीक्षा के लिए उनकी अभ्यर्थिता पर विचार नहीं किया जाएगा। अभ्यर्थी को रा.इ.सू.प्रौ.सं. द्धारा जारी प्रवेश-पत्र के साथ एक मूल वैध फोटो पहचान पत्र लाना अनिवार्य है अन्यथा उन्हें परीक्षा में बैठने की अनुमति नहीं दी जायेगी।  </span>
                                <%--<span>Note:Please use Login Id in place of Roll number for starting your exam.</span></b>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION </strong>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: justify;">
                            a)	Candidates are required to follow the rules, regulations and guidelines of NIELIT; Local Authorities, State Government and Government of India especially related to COVID-19 as issued from time to time.<br />
                            b)  Candidates are required to report at the examination centre strictly as per the reporting time allotted to the candidate to maintain staggered entry.<br />
                            c)	Candidates shall be admitted to the Examination Centre maximum 60 minutes before the commencement of Examination.<br />
                            d)	Candidates shall not be permitted to enter the Examination Centre after Gate Closing Time.<br />
                            e) 	Candidates are expected to take seat in the Examination Hall as soon as possible after entering the examination centre and not move around to maintain social distancing. <br />
                            f)Candidates entry will not be permitted inside the examination hall after the time of commencement of examination.<br />
                            <%--<asp:Label ID="lblmintime" runat="server" Text="mintime"></asp:Label>--%>                           
                            g)	Before the commencement of the Examination, it is essential and mandatory for all candidates to sign on the attendance sheet.<br />
                            h)	Candidate must ensure that s/he appends her/his signature on the attendance sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while submitting Online Examination Application Form for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.<br />

                            i)	Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination. The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadhar card, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic, Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />
                            j)	Candidates will be allowed to leave the examination hall only after completion of sixty minutes from the time of commencement of examination.<br />
                            k)	Candidates should carry admit card issued by NIELIT, an original photo identity card, pen, soap/hand-sanitiser of up to 50 ml in transparent bottle, face mask, gloves and water in transparent bottle for personal use. No other item other than specifically mentioned here shall be permissible inside the examination centre.<br />
                            l)  Candidates are advised not to bring any valuable items to the examination centre as arrangement for safe keeping of such items cannot be assured.<br />
                            m)	No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.</br> 
                            n)	No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.</br>

                            o)	Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.</br>
                            p)	No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.</br>
                            q)	No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.</br>
                            r)	Candidate should not use abusive/derogatory language orally or otherwise/ threatening/ using violence towards/against anyone including Examination Staff.</br>
                            s)	No candidate shall give or receive aid from any other applicant or source during the administration of the examination.</br>
                            t)	Smoking or Chewing Tobacco or use of Alcohol/Intoxicating Substance is strictly prohibited at the Examination Centre and inside Examination Hall. Candidates found doing so during the course of the Examination, shall be liable, to be expelled from the Examination Centre by the Examination Superintendent. A candidate, if found smoking or chewing tobacco or under the influence of intoxicated drinks/drugs/alcohol shall not be allowed to enter the examination hall and if found appearing in the examination shall be expelled from the examination hall immediately by the Examination Superintendent.</br>
                            u)	In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, candidate shall be expelled from the examination centre and his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision as per SOP of the Examination in imposing penalty for the offence committed by the candidate shall be final and binding on him/her. </br>
                            v)	<strong> Candidates are advised to strictly adhere to the time schedule and instructions. </strong> 

                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
