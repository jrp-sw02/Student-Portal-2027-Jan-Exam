<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CertificateAdmitCardVersion1.aspx.cs" Inherits="CertificateAdmitCardVersion1" %>


<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css" media="print">
        
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
        <tr class="normal">
            <td colspan="3" align="center" style="font-weight: bold;">
                Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="border: 0px;">
                <table width="100%" cellspacing="0" cellpadding="4" border="1px" style="font-size: 16px;">
                    <tr>
                        <td style="width: 36%">
                            <asp:Label ID="Label70" runat="server" Text="ROLL NO" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lblrno" runat="server" Text="" Font-Size="Large" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 20%; padding: 8px 8px 8px 8px;" align="center" rowspan="6">
                            <img id="imgcandphoto" runat="server" style="height: 150px; width: 120px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 36%">
                            <asp:Label ID="Label2" runat="server" Text="NAME"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbcname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trmothername" runat="server">
                        <td style="width: 36%">
                            <asp:Label ID="Label3" runat="server" Text="MOTHER'S NAME"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trfathername" runat="server">
                        <td style="width: 36%">
                            <asp:Label ID="Label4" runat="server" Text="FATHER'S NAME"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trguardian" runat="server" visible="false">
                        <td style="width: 36%">
                            <asp:Label ID="Label1" runat="server" Text="GUARDIAN NAME"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="Lbgname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 36%">
                            <asp:Label ID="Label5" runat="server" Text="EXAM CENTER CODE"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lblexamcode" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 36%;">
                            FOR OFFICE USE ONLY :
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lblbatchcode" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td style="width: 36%">
                            
                        </td>
                        <td style="width: 44%">
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="3" align="center">
                            VALID FOR
                            <asp:Label ID="Lbename" runat="server" Text=""></asp:Label>&nbsp;EXAMINATION ONLY
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <b><u>BATCH SCHEDULE</u></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            EXAM CENTRE CODE:
                            <asp:Label ID="Lbccode" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="2">
                            EXAM DATE :
                            <asp:Label ID="Lbedate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="3" valign="top">
                            EXAM CENTRE ADDRESS:
                            <%--  ADDRESS:--%>
                            <br />
                            <asp:Label ID="lblexamaddress" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="2">
                            BATCH :
                            <asp:Label ID="Lblbatchno" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            EXAM TIME:
                            <asp:Label ID="Lbreptime" runat="server" Text=""></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            EXAM DURATION : 
                            <asp:Label ID="lbminutes" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="3" align="center">
                            <b><span>IMPORTANT:CANDIDATES ARE ADVISED TO PUT THEIR SIGNATURE AND LEFT THUMB IMPRESSION(LTI)
                                ON THE ATTENDANCE SHEET, FAILING WHICH, THEIR CANDIDATURE SHALL NOT BE CONSIDERED
                                FOR EXAM. CANDIDATE MUST CARRY A VALID PHOTO IDENTITY CARD ALONG WITH THE ADMIT
                                CARD ISSUED BY NIELIT. </span>
                                <br />
                                <span>Note:Please use Login Id in place of Roll number for starting your exam.</span></b>
                        </td>
                    </tr>--%>
                   <tr>
                        <td colspan="3" align="center">
                            <b><span>IMPORTANT:CANDIDATES ARE ADVISED TO PUT THEIR SIGNATURE AND LEFT THUMB IMPRESSION(LTI)
                                ON THE ATTENDANCE SHEET, FAILING WHICH, THEIR CANDIDATURE SHALL NOT BE CONSIDERED
                                FOR EXAM. CANDIDATE MUST CARRY A VALID PHOTO IDENTITY CARD ALONG WITH THE ADMIT
                                CARD ISSUED BY NIELIT FAILING WHICH THE CANDIDATES WILL NOT BE ALLOWED TO APPEAR
                                IN THE EXAMINATION.</span>
                                <br />
                                <span>महत्वपूर्ण: अभ्यर्थियों को उपस्थिति-पत्र में अपने हस्ताक्षर व बाएँ हाथ के अंगूठे
                                    की छाप दिये जाने की सलाह दी जाती है। ऐसा न करने पर, परीक्षा के लिए उनकी अभ्यर्थिता
                                    पर विचार नहीं किया जाएगा। अभ्यर्थी को रा.इ.सू.प्रौ.सं. द्धारा जारी प्रवेश-पत्र के
                                    साथ एक वैध फोटो पहचान पत्र लाना अनिवार्य है अन्यथा उन्हें परीक्षा में बैठने की अनुमति
                                    नहीं दी जायेगी।
                                </span>
                                <%--<span>Note:Please use Login Id in place of Roll number for starting your exam.</span></b>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <strong>
                             INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION
                             </strong>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="3" style="text-align:justify;">
                        a) Candidates are required to report at the examination centre one hour prior to 
                        the scheduled examination time alloted to the candidate.<br />
                        b) Candidates shall be admitted to the Examination hall only 15 minutes before 
                        the commencement of Examination and candidates will not be permitted to enter 
                        the examination hall after expiry of 30 minutes from the time of commencement of 
                        examination.<br />
                        c) Before the commencement of the Examination, it is
                        essential and mandatory for all candidates to sign and put Left Thumb Impression
                        on the attendance sheet.<br />
                        d) Candidates must carry their Original Photo Identity Proof failing
                        which; the candidates will not be allowed to appear in the examination.The valid
                        photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadhar
                        card, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,
                        Photo identity card having serial number issued by Central/State Government, Photo
                        identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph
                        with attested customer photograph and signature by bank official/manager, Credit
                        cards issued by banks with laminated photograph, Certificate of Identity having
                        photo issued by Gazetted Officer or Tehsildar on letterhead. <br />
                        e) Candidates will be allowed to leave the examination hall only after 
                        completion of 
                        <asp:Label ID="lblmintime" runat="server" Text="mintime"></asp:Label> &nbsp;minutes from the time of commencement of examination.<br />
                        f) Candidates should carry only their admit card issued by NIELIT, an original photo identity
                        card and pen at the examination centre.<br />
                        g) No cell phones or any electronic device will be allowed in the examination hall.
                        If any such items are found in possession of the candidate, they will be confiscated.<br />
                        h) No pocketbooks, handbags, books, notes, written or printed material, CDs or data
                        of any kind will be allowed in the examination hall. If any such items are found
                        in possession of the candidate, they will be confiscated.<br />
                        i) Candidate will not be allowed to make any notes from memory once he/she enter
                        the examination hall and prior to the start of an examination session. If any candidate
                        is found with any such items, they will be confiscated.<br />
                        j) No candidate can bring any knife (including any pocketknife or multi-tool with
                        blades), or any weapon of any kind into the examination centre.<br />
                        k) No candidate shall create a continuing distraction by sound or movement which
                        tends to disrupt the concentration of another candidate nor shall any candidate
                        engage in any activity which reasonably may be considered to be a breach of the
                        peace.<br />
                        l) Candidate should not use abusive/derogatory language orally against the Exam
                        Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators
                        or Exam Supdt.<br />
                        m) No candidate shall give or receive aid from any other applicant or source during
                        the administration of the examination.<br />
                        n) In case any candidate is found breaching of the hall discipline and / or found
                        having used unfair means which directly / indirectly disturbs the sanctity of the
                        examination, his/her examination result shall be withheld, and action as deemed
                        fit under the SOP will be taken. The decision of the Examination Board in imposing
                        penalty for the offence committed by the candidate shall be final and binding on
                        him/her. </br>
			o)<strong>Candidates are advised to strictly adhere to the time schedule and instructions.</strong> <br />
                    </td>
                    </tr>
                    <%-- <tr>
                        <td colspan="3" align="left" style="text-align: justify;">
                            Candidates are advised to check the spellings of their name, mother’s name and father’s
                            name. Errors, if any, should be reported to the regional centre along with the supporting
                            document(s) in the format below. Please note that, the examination superintendent
                            at the examination centre duly verifies your correction request. No request for
                            corrections would be entertained after the conduct of the examination of the candidate.
                            NIELIT reserves the right to reject the application for correction request without
                            assigning any reason thereof.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            No request for correction would be entertained after the conduct of Examination
                            of that candidate.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Please correct the details as given below:-
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            For Office Use Only
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td style="width: 36%">
                            <asp:Label ID="Label9" runat="server" Text="ROLL NO" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbrollno" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 10%; padding: 2px 2px 2px 2px;" align="center" rowspan="7" valign="top">
                            <p>
                                <img src="../images/pic1.jpg" style="height: 120px; width: 100px;" />
                            </p>
                            <p>
                                (Signature)<br />
                                <asp:Label ID="Lbcname1" runat="server" Text="CandidateName"></asp:Label>
                            </p>
                            <p>
                                (Signature)<br />
                                (Exam Superintendent)</p>
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td style="width: 36%">
                            EXAM CENTER CODE
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="Lbexcode" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td style="width: 36%">
                            <asp:Label ID="Label10" runat="server" Text="NAME"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <hr style="color: Black; vertical-align: middle;" />
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td style="width: 36%">
                            <asp:Label ID="Label6" runat="server" Text="MOTHER'S NAME"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <hr style="color: Black; vertical-align: middle;" />
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td style="width: 36%">
                            <asp:Label ID="Label7" runat="server" Text="FATHER'S NAME"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <hr style="color: Black; vertical-align: middle;" />
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td colspan="2">
                            I hereby certify that the details given above are true and correct.
                        </td>
                    </tr>
                    <tr style="height: 39px;">
                        <td colspan="2">
                            I have checked all details and found that above mentioned information is true &amp;
                            correct.
                        </td>
                    </tr>--%>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>