<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="ChangeExamCentreChoice_Notification.aspx.cs" Inherits="CAND_ChangeExamCentreChoice_Notification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1
        {
            font-size: x-large;
        }
        .auto-style2
        {
            text-align: right;
            font-size: large;
        }
        .auto-style3
        {
            text-align: center;
            font-size: x-large;
            text-decoration: underline;
        }
        .auto-style4
        {
            text-align: justify;
        }
        .auto-style5
        {
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
    <div id="div1" runat="server" style="width:100%;">    
 <table style="width:100%;" id="Table1" runat="server"> 
        <tr>
            <td width="100%" align="center" class="auto-style7" >
                <span class="auto-style1"><strong style="text-align: justify;">National Institute of Electronics and Information Technology
                <br />
                New Delhi</strong></span><strong style="font-size: large; text-align: justify;">
            </strong>
            </td>

        </tr>
        <tr>
            <td width="100%" align="center" class="auto-style7" >
            </td>

        </tr>
        <tr>
            <td width="100%" class="auto-style2" >
                <strong>September 11, 2020</strong></td>

        </tr>
        <tr>
            <td width="100%" class="auto-style3" >
                <strong>IMPORTANT ANNOUNCEMENT</strong></td>

        </tr>
        <tr>
            <td width="100%" class="auto-style3" >
                </td>

        </tr>
     <tr>
            <td width="100%" class="auto-style4" >
                <span class="auto-style5">Owing to the ongoing COVID-19 pandemic and keeping in view the larger interest of the candidates and as a very special case under extraordinary situation, it has been decided by NIELIT to provide an opportunity to the candidates ofDLC Examinations(BCC/CCC/CCCP/ECC) who have applied for appearing in the examinations of DLC for the examination cycles earlier scheduled to be conducted from 1st Saturday of the respective month viz. April, May, June, August and September 2020 to submit their request to opt-out/ change of examination centre preference in respect of examination scheduled to be conducted from September 29, 2020 onwards. </span>
                <br class="auto-style5" />
                <br class="auto-style5" />
                <span class="auto-style5">Candidates who don’t submit orfailed to submit request to opt-out from September 2020 examinations of DLC or change of examination center, it shall deemed to be understood that such candidates do not have any dissent in appearing in the examinations of DLC scheduled to be conducted from September 29, 2020 onwards. NIELIT shall be conducting the examinations of 
                DLC as per the schedule available at the website of NIELIT PAN India by adhering to the precautions and guidelines related to COVID-19 issued by Government of India from time to time which includes but not limited to thermal scanning, sanitization protocols, wearing of masks/face covers, maintaining social distancing at all times during the conduct of online examinations of DLC. </span>
                <br class="auto-style5" />
                <br class="auto-style5" />
                <span class="auto-style5">Candidates can visit the
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/CAND/ChangeExamCentreChoice.aspx" Text="Exam Center Change Facility" Target="_blank" ForeColor="#0000CC"></asp:HyperLink>
&nbsp;</span><URL><span class="auto-style5">from September 11, 2020 to September 15, 2020 and avail the above-mentioned facility for the examinations commencing from September 29, 2020 onwards. The allocation of the examination center shall however be subject to availability of seats and other feasibility conditions. Although all possible efforts shall be made by NIELIT to allocate examination center as per the preference opted by the candidate, however NIELIT reserves the right to allocate any other examination center. In case of any dispute arising out of the situation the decision of DG NIELIT shall be final and 
                binding. </span>
                <br class="auto-style5" />
                <br class="auto-style5" />
                <span class="auto-style5">All the candidates and stakeholders are advised to keep themselves updated at the website of NIELIT for latest updates.</span></td>
        </tr>

     <tr>
            <td width="100%" class="auto-style9" >
                </td>
        </tr>

     <tr>
            <td width="100%" class="auto-style2" >
                <strong>Sd/-
                <br />
                (Anurag Shah)
                <br />
                Controller of Examinations</strong></td>
        </tr>

        </table>
        </div>
</asp:Content>

