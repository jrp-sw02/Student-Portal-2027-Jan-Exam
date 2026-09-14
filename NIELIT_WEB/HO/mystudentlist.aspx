<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="MystudentList.aspx.cs" Inherits="MystudentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="List Of Candidates"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <ul class="crumbs">
        <li class="first"><a id="a1" runat="server" href="#" style="z-index: 9;"><span></span>
        </a></li>
        <li><a href="#" style="z-index: 8;">
            <%=Request.QueryString["course"].ToString() %>: Student List</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:HiddenField ID="hidvalue" runat="server" />
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="4">
                <div id="Div_Course" runat="server">
                    <table cellspacing="1" cellpadding="4" border="0" id="cphContents_gvMain" style="width: 100%;">
                        <tr class="gdheader">
                            <th>
                                #
                            </th>
                            <th>
                                Student Name
                            </th>
                            <th>
                                Application No.
                            </th>
                            <th>
                                Date
                            </th>
                            <th>
                                Payment status
                            </th>
                            <th>
                                Registration status
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                1
                            </td>
                            <td>
                                <a href="NielitRegistration.aspx">Saroj Kitawat</a>
                            </td>
                            <td align="right">
                                111
                            </td>
                            <td align="right">
                                09-Jan-1990
                            </td>
                            <td>
                                Pending
                            </td>
                            <td>
                                Applied
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                2
                            </td>
                            <td>
                                Vishwaraj
                            </td>
                            <td align="right">
                                115
                            </td>
                            <td align="right">
                                12-Feb-2002
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                Aknowledged
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                3.
                            </td>
                            <td>
                                Puneet
                            </td>
                            <td align="right">
                                123
                            </td>
                            <td align="right">
                                15-Nov-2011
                            </td>
                            <td>
                                Pending
                            </td>
                            <td>
                                Pending
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                4
                            </td>
                            <td>
                                Khushboo
                            </td>
                            <td align="right">
                                144
                            </td>
                            <td align="right">
                                10-Jan-2012
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                Applied
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                5
                            </td>
                            <td>
                                Neetu
                            </td>
                            <td align="right">
                                185
                            </td>
                            <td align="right">
                                02-Aug-2002
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                Aknowledged
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                6
                            </td>
                            <td>
                                Kuldeep
                            </td>
                            <td align="right">
                                112
                            </td>
                            <td align="right">
                                08-Jul-2005
                            </td>
                            <td>
                                Pending
                            </td>
                            <td>
                                Pending
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                7
                            </td>
                            <td>
                                Priyanka
                            </td>
                            <td align="right">
                                177
                            </td>
                            <td align="right">
                                02-Feb-2008
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                Varified
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                8
                            </td>
                            <td>
                                Nitesh Garg
                            </td>
                            <td align="right">
                                324
                            </td>
                            <td align="right">
                                04-May-1980
                            </td>
                            <td>
                                Pending
                            </td>
                            <td>
                                Pending
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Exam" runat="server">
                    <table cellspacing="1" cellpadding="4" border="0" id="Table1" style="width: 100%;">
                        <tr class="gdheader">
                            <th>
                                #
                            </th>
                            <th>
                                Registration No.
                            </th>
                            <th>
                                Student Name
                            </th>
                            <th>
                                Application Date
                            </th>
                            <th>
                                Modules
                            </th>
                            <th>
                                Amount
                            </th>
                            <th>
                                Status
                            </th>
                            <th id="chk" runat="server">
                                &nbsp;
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                1
                            </td>
                            <td align="left">
                                <a href="frmmodulelistforexam.aspx?status=For Examination&exam=July 2012&course=<%=Request.QueryString["course"].ToString() %>&R=R/AC/111&v=<%=Request.QueryString["sts"].ToString()%>">
                                    R/AC/111</a>
                            </td>
                            <td>
                                Saroj Kitawat
                            </td>
                            <td align="right">
                                09-Jan-1990
                            </td>
                            <td align="right">
                                3
                            </td>
                            <td align="right">
                                700
                            </td>
                            <td id="status1" runat="server">
                                Paid &amp; To Be Verified
                            </td>
                            <td id="ch1" runat="server">
                                <input id="Checkbox7" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                2
                            </td>
                            <td align="left">
                                <a id="rno1" runat="server" href="frmmodulelistforexam.aspx?status=for Examination&course=O 

level(Computer S/w)&exam=July 2012&R=R/AC/115">R/AC/115</a>
                            </td>
                            <td>
                                Vishwaraj
                            </td>
                            <td align="right">
                                12-Feb-2002
                            </td>
                            <td align="right">
                                5
                            </td>
                            <td align="right">
                                1200
                            </td>
                            <td id="status2" runat="server">
                                Not Paid &amp; To Be Verified
                            </td>
                            <td id="ch2" runat="server">
                                <input id="Checkbox8" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                3.
                            </td>
                            <td align="left">
                                <a id="rno2" runat="server" href="frmmodulelistforexam.aspx?status=for Examination&course=O 

level(Computer S/w)&exam=July 2012&R=R/AC/123">R/AC/123</a>
                            </td>
                            <td>
                                Puneet
                            </td>
                            <td align="right">
                                15-Nov-2011
                            </td>
                            <td align="right">
                                2
                            </td>
                            <td align="right">
                                300
                            </td>
                            <td id="status3" runat="server">
                                Paid &amp; To Be Verified
                            </td>
                            <td id="ch3" runat="server">
                                <input id="Checkbox9" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                4
                            </td>
                            <td align="left">
                                <a id="rno3" runat="server" href="frmmodulelistforexam.aspx?status=for Examination&course=O 

level(Computer S/w)&exam=July 2012&R=R/AC/144">R/AC/144</a>
                            </td>
                            <td>
                                Khushboo
                            </td>
                            <td align="right">
                                10-Jan-2012
                            </td>
                            <td align="right">
                                4
                            </td>
                            <td align="right">
                                1000
                            </td>
                            <td id="status4" runat="server">
                                Not Paid &amp; To Be Verified
                            </td>
                            <td id="ch4" runat="server">
                                <input id="Checkbox10" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                5
                            </td>
                            <td align="left">
                                <a id="rno4" runat="server" href="frmmodulelistforexam.aspx?status=for Examination&course=O level(Computer S/w)&exam=July 2012&R= R/AC/185">
                                    R/AC/185</a>
                            </td>
                            <td>
                                Neetu
                            </td>
                            <td align="right">
                                02-Aug-2002
                            </td>
                            <td align="right">
                                3
                            </td>
                            <td align="right">
                                300
                            </td>
                            <td id="status5" runat="server">
                                Paid &amp; To Be Verified
                            </td>
                            <td id="ch5" runat="server">
                                <input id="Checkbox11" type="checkbox" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="text-align: right; margin-top: 10px">
                    <asp:Button ID="btnok" runat="server" Text="Ok" Width="86px" OnClick="btnok_Click" />
                    <asp:Button ID="btncancel" runat="server" Text="Back" OnClick="btncancel_Click" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
