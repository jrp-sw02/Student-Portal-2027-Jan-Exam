<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="adminaccaffiliation.aspx.cs" Inherits="Admin_adminaccaffiliation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
        .style2
        {
            width: 111px;
        }
        .style3
        {
            width: 115px;
        }
        .style4
        {
            width: 183px;
        }
        .style5
        {
            width: 90px;
        }
        .style6
        {
            width: 164px;
        }
        .style7
        {
            width: 186px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
<asp:Label ID="lblHeading" runat="server" Text="Accredited Centre Affiliation Details"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
<a href="#">Accreadiation Center Affiliation Details</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

<div id="div1" runat="server">
                            <table class="gdbody" cellspacing="1" cellpadding="4" id="cphContents_gvMain" style="width: 99%;">
                                <tr class="gdheader">
                                    <td align="center" class="style6">
                                        <b>&nbsp;Acc. No.</b></td>
                                    <td align="left" class="style7">
                                        <b>Institute Name</b>
                                    </td>
                                    <td align="left" class="style1">
                                        <b>Address</b>
                                    </td>
                                    <td align="center" class="style2">
                                        <b>Contact No</b>
                                    </td>
                                    <td align="left" class="style3">
                                        <b>Course Level</b>
                                    </td>
                                    <td align="left" class="style4">
                                        <b>Affiliation Date</b>
                                    </td>
                                    <td align="left" class="style5">
                                        <b>Valid Upto</b></td>
                                </tr>
                                <tr class="gdrow">
                                    <td align="center" class="style6">
                                        ACR101
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R101&name=Aishwarya College&contact=9001107701&b=O,A,B">Aishwarya College</a>
                                    </td>
                                    <td align="left" class="style1">
                                        12 abcd&nbsp; xyzcolony
                                    </td>
                                    <td align="center" class="style2">
                                        900107701
                                    </td>
                                    <td align="left" class="style3">
                                        O,A,B
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                                <tr class="gdalternate">
                                    <td align="center" class="style6">
                                        ACR102
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R102&name=AKC&contact=9001107701&b=A,B">AKC</a>
                                    </td>
                                    <td align="left" class="style1">
                                        1k23 cvxbvui3ndbadfbasj
                                    </td>
                                    <td align="center" class="style2">
                                        123123344
                                    </td>
                                    <td align="left" class="style3">
                                        A,B
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                                <tr class="gdrow">
                                    <td align="center" class="style6">
                                        ACR103
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R103&name=Krishna College&contact=9001107701&b=BCC,CCC,O">Krishna College</a>
                                    </td>
                                    <td align="left" class="style1">
                                        CEDTI Complex, University
                                    </td>
                                    <td align="center" class="style2">
                                        312324355
                                    </td>
                                    <td align="left" class="style3">
                                        BCC,CCC,O
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                                <tr class="gdalternate">
                                    <td align="center" class="style6">
                                        ACR104
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R104&name=AKC&contact=9001107701&b=O,A,B">AKC</a>
                                    </td>
                                    <td align="left" class="style1">
                                        1k23 cvxbvui3ndbadfbasj
                                    </td>
                                    <td align="center" class="style2">
                                        123123344
                                    </td>
                                    <td align="left" class="style3">
                                        O.A.B
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                                <tr class="gdrow">
                                    <td align="center" class="style6">
                                        ACR105
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R105&name=Krishna College&contact=9001107701&b=O,A,B&b=BCC,O">Krishna College</a>
                                    </td>
                                    <td align="left" class="style1">
                                        CEDTI Complex, University
                                    </td>
                                    <td align="center" class="style2">
                                        312324355
                                    </td>
                                    <td align="left" class="style3">
                                        BCC,O
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                                <tr class="gdalternate">
                                    <td align="center" class="style6">
                                        ACR106
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R106&name=AKC&contact=9001107701&b=O,A,B&b=O,A">AKC</a>
                                    </td>
                                    <td align="left" class="style1">
                                        1k23 cvxbvui3ndbadfbasj
                                    </td>
                                    <td align="center" class="style2">
                                        123123344
                                    </td>
                                    <td align="left" class="style3">
                                        0,A
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                                <tr class="gdrow">
                                    <td align="center" class="style6">
                                        ACR107
                                    </td>
                                    <td align="left" class="style7">
                                        <a href="adminaccrediatedcenter.aspx?key=R107&name=Krishna College&contact=9001107701&b=CCC,O">Krishna College</a>
                                    </td>
                                    <td align="left" class="style1">
                                        CEDTI Complex, University
                                    </td>
                                    <td align="center" class="style2">
                                        312324355
                                    </td>
                                    <td align="left" class="style3">
                                        CCC,O
                                    </td>
                                    <td align="left" class="style4">
                                        01/01/2012</td>
                                    <td align="left" class="style5">
                                        01/01/2017</td>
                                </tr>
                            </table>
                        </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

