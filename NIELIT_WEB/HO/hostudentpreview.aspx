<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="hostudentpreview.aspx.cs" Inherits="HO_hostudentpreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Applicant Students"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <ul class="crumbs">
        <li class="first"><a href="LoadsProcess.aspx" style="z-index: 9;"><span></span>Batch
            Processing</a></li>
        <li><a href="LoadsProcess.aspx?key=R/BTH/101" style="z-index: 8;"><span></span>R/BTH/101</a></li>
        <li id="liedit" runat="server"><a href="#" style="z-index: 7;"><%=Request.QueryString["no"].ToString() %></a></li>      
    </ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    
    <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
        <tr class="heading">
            <td colspan="3">
                Batch Detail
            </td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                Batch No. : R/BTH/101
            </td>
            <td style="width: 33%;" valign="top">
                Date Of Receiveing :&nbsp; 01-Jan-2012
            </td>
            <td style="width: 33%;" valign="top">
                Course : O Level
            </td>
        </tr>
    </table>
    <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
        cellspacing="1">
        <tr class="head1">
            <td align="left" colspan="2">
                Application Details
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="38%">
                Application No.
            </td>
            <td>
                1001
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="38%">
                <asp:Label ID="Label70" runat="server" Text=" Name "></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Sunil Kumar"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="38%">
                <asp:Label ID="Label3" runat="server" Text="Father's Name"></asp:Label>
            </td>
            <td>
                Amit Garg
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="38%">
                <asp:Label ID="Label7" runat="server" Text="Mother's Name"></asp:Label>
            </td>
            <td>
                Amita Garg
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="38%">
                <asp:Label ID="Label8" runat="server" Text="Gender"></asp:Label>
            </td>
            <td width="38%">
                Male
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="38%">
                <asp:Label ID="Label9" runat="server" Text="Marital Status"></asp:Label>
            </td>
            <td>
                Married
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="38%">
                <asp:Label ID="Label12" runat="server" Text="Date of Birth "></asp:Label>
            </td>
            <td>
                21-05-1980
            </td>
        </tr>
        <tr class="head1">
            <td colspan="2">
                <asp:Label ID="Label74" runat="server" Text="Correspondence Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="25%">
                Address
            </td>
            <td>
                23, New vidhaya Nagar Hiran Magri Sec-11
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="25%">
                <asp:Label ID="Label18" runat="server" Text="Phone with STD Code"></asp:Label>
            </td>
            <td width="62%">
                0294456677
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="25%">
                <asp:Label ID="Label40" runat="server" Text="Mobile"></asp:Label>
            </td>
            <td>
                +91345435646
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="25%">
                <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email "></asp:Label>
            </td>
            <td>
                niteshgarg@e-connectsolutions.com
            </td>
        </tr>
        <tr class="head1">
            <td colspan="2">
                Permanent Address Details
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="25%">
                Address:
            </td>
            <td>
                23, New vidhaya Nagar Hiran Magri Sec-11
            </td>
        </tr>
        <tr class="head1">
            <td colspan="2">
                Educational/Qualification Details
            </td>
        </tr>
        <tr valign="top" class="gdrow1">
            <td width="25%">
                <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification"></asp:Label>
            </td>
            <td>
                MCA
            </td>
        </tr>
        <tr class="gdalternate1" valign="top">
            <td width="25%">
                Year of Passing
            </td>
            <td>
                2008
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="25%" align="left">
                &nbsp;DOEACC Course
            </td>
            <td width="62%">
                O Level
            </td>
        </tr>
        <tr class="gdalternate1">
            <td align="left" width="25%">
                Year of Passing
            </td>
            <td width="62%">
                2009
            </td>
        </tr>
        <tr class="gdrow1">
            <td align="left" width="25%">
                &nbsp;
            </td>
            <td width="62%">
                &nbsp;
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
     <asp:Button ID="btnprevious" runat="server" Text="Previous" />
        <%--<asp:Button ID="btnverify" runat="server" Text="Verify" />
        <asp:Button ID="btnskip" runat="server" Text="Skip" />
        <asp:Button ID="btnreject" runat="server" Text="Reject" />--%>
        <asp:Button ID="btnnext" runat="server" Text="Next" />
        <asp:Button ID="btnCancel" runat="server" Text="Back" 
            onclick="btnCancel_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
