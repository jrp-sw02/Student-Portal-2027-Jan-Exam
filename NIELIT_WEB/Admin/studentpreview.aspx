<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="studentpreview.aspx.cs" Inherits="studentpreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Registered Students"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <ul class="crumbs">
        <li class="first"><a id="a1" runat="server" href="acc_reg_info.aspx" style="z-index: 9;">
            <span></span>Course Registration Status</a></li>
        <li><a id="a3" runat="server" href="#"
            style="z-index: 8;"><%=Request.QueryString["course"].ToString() %>: Student List</a></li>
        <li><a id="a2" runat="server" href="#" style="z-index: 7;"><span></span>Registered Student</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
<div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnprevious" runat="server" Text="Previous" />
        <asp:Button ID="btnverify" runat="server" Text="Verify" />
        <asp:Button ID="btnskip" runat="server" Text="Skip" />
        <asp:Button ID="btnreject" runat="server" Text="Reject" />
        <asp:Button ID="btnnext" runat="server" Text="Next" />
</div>
    <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
        cellspacing="1">
        <tr class="head1">
            <td align="left" colspan="2">
                Registration Details
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="38%">
                Registration No.
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Reg/A/123"></asp:Label>
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
                Mr.
                Amit Garg
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="38%">
                <asp:Label ID="Label7" runat="server" Text="Mother's Name"></asp:Label>
            </td>
            <td>
                Mrs.
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
        <tr class="gdrow1">
            <td width="38%">
                Accrediated Center</td>
            <td>
                Aishwarya College Udaipur</td>
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
        <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
