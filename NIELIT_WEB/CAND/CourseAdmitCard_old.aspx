<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CourseAdmitCard.aspx.cs"
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
    </style>
</head>
<body style="background-color: #FFFFFF;">
    <form id="form1" runat="server" style="margin-top: 10px;">
    <table align="center" border="0" cellspacing="0" style="font-size: 14px; font-family: Arial;"
        width="958px">
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
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding: 0;" OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr class="normal">
            <td colspan="3" align="center" style="padding: 5px;">
                <asp:Label ID="Lbename" Width="45%" Font-Bold="true" Font-Size="13pt" Style="text-align: right;"
                    runat="server" Text=""></asp:Label>
                <span style="width: 110px; float: right; font-size: 12px;">Date:
                    <%=DateTime.Now.ToString("dd-MMM-yyyy") %></span>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="border: 0px;">
                <table width="100%" cellspacing="0" cellpadding="3" border="1px">
                    <tr>
                        <td width="8%">
                            <asp:Label ID="Label70" runat="server" Text="ROLL NO" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="10%">
                            <asp:Label ID="Lblrno" Font-Size="Large" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="4%">
                        </td>
                        <td width="8%">
                            <asp:Label ID="Label5" runat="server" Text="REG NO" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="10%">
                            <asp:Label ID="Lbregno" Font-Size="Large" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="2" valign="top">
                            <asp:Label ID="Label1" runat="server" Text="Office Reference No:" Style="font-weight: 700"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lboffrefno" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2" colspan="5">
                            <asp:Label ID="Lbcname" runat="server" Text=""></asp:Label><br />
                            <asp:Label ID="lbstudaddress" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="15%">
                            <strong>LEVEL</strong>
                        </td>
                        <td width="15%">
                            <asp:Label ID="Lblevel" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="15%">
                            <strong>MEDIUM </strong>
                        </td>
                        <td width="15%">
                            <asp:Label ID="Lblmedium" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" style="padding-left: 4px; height: 60px;">
                            <strong>Centre Code :- </strong>
                            <asp:Label ID="Lbccode" runat="server" Text=""></asp:Label><br />
                            <strong>Address :- </strong>
                            <asp:Label ID="lblexamaddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="font-size: 12px; font-weight: bold; padding: 1px;" valign="top"
                            align="center">
                            PAPER CODE(S) / MODULES APPEARING FOR
                        </td>
                        <td rowspan="3" align="center" height="100px">
                            <asp:Image ID="ImgCandidatePhoto" Width="100px" Height="120px" runat="server" Style="text-align: center;"
                                ImageAlign="Middle" /><br />
                            <asp:Image ID="ImgCandidatesignature" Width="100px" Height="35px" runat="server"
                                Style="text-align: center;" ImageAlign="Middle" />
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 11px; font-weight: bold; font-family: Sans-Serif;" colspan="8"
                            align="left">
                            On the basis of your online Examination Application form you have been provisionally
                            permitted to appear in the following module(s) / paper(s)
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="left" style="padding: 0px; border: 0px; vertical-align: top;">
                            <div id="divReportData" runat="server" style="width: 100%;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="top" align="center">
                            <asp:Image ID="Imgofficesignature" runat="server" Style="text-align: center; padding-bottom: 2px;
                                padding-top: 2px;" ImageAlign="Middle" Width="100px" Height="30px" ImageUrl="~/images/ExamDirectorSignature.jpg" />
                            <br />
                            Asst.Director (Exam)
                        </td>
                        <td rowspan="2" colspan="7" valign="top" style="padding-left: 5px;">
                            “Please bring this Admit Card along with your “Registration Allocation-cum-Identity
                            Card” issued by NIELIT to the Examination Hall” at the venue, the address of which
                            is given above along with any other original<%--In case of non-receipt of your “Registration Allocation-cum-Identity
                            Card’, you are requested to carry an alternate--%>
                            Photo Identity Card, such as Driving License, Passport, PAN Card, etc., to the Examination
                            Centre,as proof of your identity. <span class="style7"><strong>Books,Papers,Brochures,Mobile
                                Phones and/or other Electronic Gadgets are stictly prohibited in the Examination
                                Hall.</strong></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="top" align="center">
                            <asp:Label ID="lbpubdate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9" style="color: black; font-size: 12px; padding-left: 4px;" align="left">
                            <u><strong>Note:-</strong> Date(s) for Practical Examination shall be notified separately.
                                Also Admit card for Practical Examination shall be issued to all eligible applicants
                                separately.</u>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="normal">
            <td colspan="3" align="center">
                <strong>
                    <br />
                    INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="padding: 0px; font-size: 11px;">
                <table width="100%" cellpadding="1" cellspacing="0" border="0">
                    <tr>
                        <td style="text-align: left; width: 50%; vertical-align: top;">
                            <table>
                                <tr>
                                    <td style="text-align: left; width: 1%; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; width: 1%; vertical-align: top;">
                                        1.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;" width="48%">
                                        <strong><u>GENERAL</u></strong><br />
                                        These instructions will help you to write NIELIT examinations properly. Read the
                                        paragraphs that follow carefully and understand the procedures to be observed. <strong>
                                            PLEASE APPEND YOUR SIGNATURES AT APPROPRIATE PLACE IN THE ATTENDANCE SHEET DURING
                                            THE EXAMINATION OTHERWISE YOUR ANSWER BOOKS WILL NOT BE EVALUTATED.</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        2.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        <strong><u>ROLL NUMBER</u></strong><br />
                                        Your Roll number,Name &amp; Subjects,that you are to appear,in this examination
                                        are printed on your admit card. Write your Roll Number in the space provided on
                                        all Answer Books. Write Anser Book number in the additional sheet if taken. Your
                                        name should NOT appear in any part of the Answer book, in case of any discrepancy
                                        in the Admit Card,contact NIELIT immediately.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        <strong><u>SCHEDULE AND HALL DISCIPLINE</u></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.1
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        The Schedule for distribution and collection of Answer Books/Question papers and
                                        other related activities in the examination hall is as given below:-
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center; font-size: 10px;">
                                        <table border="1" cellspacing="0" cellpadding="0" width="100%" style="border-collapse: collapse;
                                            line-height: 14px; border-left: 0;">
                                            <tr>
                                                <td style="text-align: center;" width="60%">
                                                    <strong>ACTIVITY</strong>
                                                </td>
                                                <td style="text-align: center;" width="20%">
                                                    <strong>Forenoon<br />
                                                        Session<br />
                                                        (9:30-12:30)</strong>
                                                </td>
                                                <td style="text-align: center;" width="20%">
                                                    <strong>Afternoon<br />
                                                        Session<br />
                                                        (1400-1700)</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Seating of candidate as per seating plan
                                                </td>
                                                <td>
                                                    09:15 IST
                                                </td>
                                                <td>
                                                    13:45 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Distribution of Question Papers
                                                </td>
                                                <td>
                                                    09:25 IST
                                                </td>
                                                <td>
                                                    13:55 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Commencement of Examination
                                                </td>
                                                <td>
                                                    09:30 IST
                                                </td>
                                                <td>
                                                    14:00 IST
                                                </td>
                                            </tr>
                                            <tr>
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
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Lates time for submission of Part 1 Answer Sheet(OMR Sheet) by the candidates
                                                </td>
                                                <td>
                                                    10:30 IST
                                                </td>
                                                <td>
                                                    15:00 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Distribution of Answer Books.<br />
                                                    6.1 Part-II Answer book(applicable for all subject of O level,all subject of A level,
                                                    first 10 subject of &#39;B&#39; level)<br />
                                                    6.2 Answer book(applicable for remaining subject of B level &amp; C level).
                                                </td>
                                                <td>
                                                    10:30 IST*
                                                    <br />
                                                    9:25 IST
                                                </td>
                                                <td>
                                                    15:00 IST*
                                                    <br />
                                                    13:55 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Recording of particulars in the Attendance Sheet
                                                </td>
                                                <td>
                                                    10:30 IST
                                                </td>
                                                <td>
                                                    15:00 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Signing on Admit Card by the Invigilator
                                                </td>
                                                <td>
                                                    10:30 IST
                                                </td>
                                                <td>
                                                    15:00 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Earliest time a candidate is permitted to leave the Examination Hall(without Question
                                                    Paper)
                                                </td>
                                                <td>
                                                    10:30 IST
                                                </td>
                                                <td>
                                                    15:00 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Earliest time a candidate is permitted to leave the Examination Hall( with Question
                                                    Paper)
                                                </td>
                                                <td>
                                                    11:30 IST
                                                </td>
                                                <td>
                                                    16:00 IST
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; padding-left: 5px;">
                                                    Completion of Examintion
                                                </td>
                                                <td>
                                                    12:30 IST
                                                </td>
                                                <td>
                                                    17:00 IST
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        *
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        as soon as the candidate completes Part I, he/she can collect the answer book for
                                        Part II from the Invigilator after handing over the Part-I Answer Sheet.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.2
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        The Examination Superintendent has absolute powers to expel a candidate from the
                                        examination hall,if ,in his opinion,the candidate has adopted unfair means,or has
                                        disturbed the hall discipline.
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: left; width: 50%; vertical-align: top;">
                            <table>
                                <tr>
                                    <td style="text-align: left; width: 1%; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; width: 1%; vertical-align: top;">
                                        3.3
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;" width="48%">
                                        You are prohibited from bringing text books,notes and programmable calculators in
                                        the examination hall. Pagers &amp; Mobile phones are not permitted inside the examintation
                                        hall.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.4
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        For certain papers booklets are supplied. DO NOT WRITE,SCRIBBLE OR IN ANY WAY,DEFACE
                                        THESE,RETURN THEM TO THE INVIGILATOR ALONG WITH ANSWER BOOKS.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.5
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidate must write his/her Roll Number on Question Paper. DO NOT WRITE ANYTHING
                                        ELSE ON QUESTION PAPER.<
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.6
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Supplementary sheets should not be used for Rough Work. Please ensure that supplementary/addtional
                                        sheets are tagged/stapled inside answer book cover to prevent them from being separated/lost.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        3.7
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Be concise while answering. Write clearly and legibly and maintain the sequence.
                                        Ball Point pens are allowed.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        4.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        <strong><u>INSTRUCTION FOR MARKING THE ANSWERS</u></strong><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        4.1
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        While ansering objective type of questions avoid guesswork. Objective type of questions
                                        may consist of one or more of the following: a) Multiple Choice b) True/False c)
                                        Matching Columns d) Fill in the Blanks.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        4.2
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        The answers to the multiple choice questions,,in the first part,are to be marked
                                        by shading, the appropriate box in the Part 1 OMR answer sheets,which is supplied
                                        with Question Paper.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        4.3
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Mark should be DARK and should completely fill the circle.<br />
                                        <asp:Image ID="Image1" ImageAlign="Right" runat="server" ImageUrl="~/images/admitform.jpeg" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        4.4
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        The candidate must mark his/her response after careful consideration,as it is not
                                        possible to change the response once it is marked. The candidate must shade only
                                        one circle. If more than one cirlce is shaded,it will be treated as wrong answer.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        4.4
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        <strong>Cutting/Erasing/Use of White Fluid/Pencil is NOT permitted.</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        5.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidates must take note that,it is possible that the examintation centre may be
                                        in two locations. This will be indicated on the ADMIT CARD or announced/published
                                        in print/electronic media and the NIELIT website.(www.nielit.in).
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        6.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidates should ensure that,they sign on the attendance sheet only against their
                                        name,as a proof of having attended the examination. The answer script of those candidates,who
                                        fail to sign the attendance sheet,will not be evaluated.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        7.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        For Practical Examination separate communication will be published online.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        8.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        Candidates are not permitted to appear for any other module/paper,other than those
                                        ,which are mentioned in his/her Admit Card or by way of a special permission letter
                                        issued by NIELIT.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        9.
                                    </td>
                                    <td style="vertical-align: top; text-align: justify;">
                                        <strong>DATESHEET FOR
                                            <asp:Label ID="lbename1" runat="server"></asp:Label>
                                            EXAMINATION IS AVAILABLE AT THE NIELIT WEBSITE. PLEASE CHECK THE WEBSITE AT http://www.nielit.in
                                            REGULARLY FOR LATEST UPDATION.</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
