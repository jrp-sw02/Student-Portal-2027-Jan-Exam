<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlineCourseAdmitCard_Ver1.aspx.cs" Inherits="CAND_OnlineCourseAdmitCard_Ver1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="956px">
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
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; xpadding-bottom: 20px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr class="normal">
            <td colspan="3" align="center" style="padding: 5px; font-weight: bold;">
               <span>CANDIDATE ADMIT CARD </span>
            </td>
        </tr>
        <tr>
                        <td colspan="3" align="center">
                            VALID FOR
                            <asp:Label ID="Lbename" runat="server" Text=""></asp:Label>&nbsp;THEORY EXAMINATION ONLY
                        </td>
                    </tr>
       <br/>
        <tr>
            <td colspan="3" style="border: 0px;">
                <table width="100%" cellspacing="0" cellpadding="4" border="1px" style="font-size: 16px;">
                    <tr style="text-align:center;background-color:#8fbcdb"><td colspan="3">1. PERSONAL DETAILS</td></tr>
                    <tr>
                        <td class="auto-style1">
                            Level</td>
                        <td style="width: 44%">
                            <asp:Label ID="Lblevel" runat="server" Text="" Font-Size="Large"></asp:Label>
                        </td>
                        <td style="width: 20%; padding: 8px 8px 8px 8px;" align="center" rowspan="7">
                            <%--<img id="imgcandphoto" runat="server" style="height:200px; width: 235px" />--%>
                            <asp:image id="ImgCandidatePhoto" width="235px" height="200px" runat="server" /><br/>
                            <span> <img id="imgPhotoBarcode" runat="server"/> </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="Label2" runat="server" Text="Registration Number"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LbRegNo" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trmothername" runat="server">
                        <td class="auto-style1">
                            <asp:Label ID="Label3" runat="server" Text="Roll Number"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LbRollNo" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trfathername" runat="server">
                        <td class="auto-style1">
                            <asp:Label ID="Label4" runat="server" Text="Candidate Name"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="LbCname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trguardian" runat="server">
                        <td class="auto-style1">
                            <asp:Label ID="Label1" runat="server" Text="Father Name/Guardian Name"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="Lbfgname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="Label5" runat="server" Text="Date Of Birth"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbldob" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Exam Center :
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblECenter" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td style="width: 36%">
                            
                        </td>
                        <td style="width: 44%">
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td colspan="3" align="center">
                            VALID FOR
                            <asp:Label ID="Lbename" runat="server" Text=""></asp:Label>&nbsp;EXAMINATION ONLY
                        </td>
                    </tr>--%>
                    <tr style="text-align:center;background-color:#8fbcdb;font-size:16px;">
                        <td colspan="3" align="center">
                          2. EXAMINATION DETAILS
                        </td>
                    </tr>
                   <%-- <tr>
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
                    </tr>--%>
                    
                  <tr>
                        <td colspan="3" align="left" style="padding: 0px; border: 1px; vertical-align: top;
                            font-size: 16px;">
                       
                            <div id="divReportData" runat="server" style="width: 100%;">
                            </div>
                        </td>
                    </tr>
 <tr>
                           
                            <td colspan="10" valign="top" style="padding-left: 5px;font-weight:bold">* No Request regarding Change of Examination Centre OR Schedule will be entertained.</td>
                        </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES</strong></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: justify;">
                            1.	Candidates are required to follow the rules regulations and guidelines of NIELIT; Local Authorities, State Government and the Government of India as issued from time to time.<br />
                            2.	No candidate will be permitted to enter the Examination Centre after Reporting Time.<br />
                            3.	Candidate shall ensure to sit on the seat allocated as per the seating plan as soon as possible after entering the examination centre and not move around.<br />
                            4.	Applicants must sign the attendance sheet at the Exam Centre as they did when uploading their signatures to the Student Online Portal for course registration. Only matching signature-roll number pairs on the physical sheet will validate attendance. NIELIT may verify signatures and cancel exams for inconsistencies<br />
                            5.	Candidates must bring their Original Photo Identity Proof along with Admit Card and Registration allocation-cum-Identity Card. Without these, they won't be allowed to take the exam. Valid photo IDs include Voter ID, Passport, PAN card, Driving License, Aadhaar card, School/College/ITI/Polytechnic Student ID with photo, Govt.-issued photo ID, PSU/Autonomous body ID, Bank passbook with attested photo, Bank-issued credit cards with photo, Gazetted Officer/Tehsildar-issued photo ID <br />
                            6.	Candidates will be allowed to leave the examination hall only after completion of sixty minutes from the time of commencement of the examination.<br />
                            7.	Candidates are advised not to bring any valuable items to the examination centre as arrangements for the safekeeping of such items cannot be assured.<br />
                            8.	No mobile phones or any other electronic device will be allowed in the examination hall. If any such items are found in the possession of the candidate, they will be confiscated. Pen and water in a transparent bottle for personal use is allowed. No other item other than that specifically mentioned here shall be permissible inside the examination Centre. <br />
                            9.	No candidate shall give or receive aid from any other applicant or source during the examination. No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind is allowed in the examination hall. Such cases will be treated as unfair means cases and are liable for strict action.<br />
                            10.	Smoking or Chewing Tobacco or use of Alcohol/Intoxicating substances is strictly prohibited at the Examination Centre. Candidates found doing so during the course of the Examination, shall be liable, to be expelled from the Examination Centre by the Examination Superintendent/Examiner/Observer.<br />
                            11.	Candidates breaking Exam Centre rules, carrying knives or weapons, or using unfair means that disrupt the exam's integrity will be expelled. Their results will be withheld, and appropriate action will be taken as per the SoP. The SoP will make the final decision on penalties.<br />
                            12.	Candidates should follow the instructions of the Invigilator/Centre Superintendent/ Observers of the Examination Centre concerned. Candidates must thoroughly read and follow the instructions which will appear on their respective Exam Panel before starting the examination.<br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
