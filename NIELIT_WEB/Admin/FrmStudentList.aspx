<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmStudentList.aspx.cs" Inherits="FrmStudentList" %>

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
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="4">
                <div id="Div_Default" runat="server">
                    <table cellspacing="1" cellpadding="4" border="0" style="width: 100%;">
                        <tr class="gdheader">
                            <th>
                                #
                            </th>
                            <th>
                                Candidate Name
                            </th>
                            <th>
                                Application No.
                            </th>
                            <th class="style1">
                                Father&#39;s Name
                            </th>
                            <th class="style2">
                                D.O.B.
                            </th>
                            <th>
                                &nbsp;Application Date
                            </th>
                            <th>
                                &nbsp;Status
                            </th>
                            <th>
                                &nbsp;
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                1
                            </td>
                            <td>
                                <a href="studentpreview.aspx?&course=<%=Request.QueryString["course"].ToString() %>&v=<%=Request.QueryString["sts"].ToString() %>">
                                    Saroj Kitawat</a>
                            </td>
                            <td align="right">
                                11123
                            </td>
                            <td align="left" class="style1">
                                Rajendra Kitawat
                            </td>
                            <td align="right" class="style2">
                                12-Dec-1985
                            </td>
                            <td align="right">
                                09-Jan-2012
                            </td>
                            <td>
                                Paid &amp; To BeVerified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
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
                                11523
                            </td>
                            <td align="left" class="style1">
                                Surendra Jain
                            </td>
                            <td align="right" class="style2">
                                02-Jan-1986
                            </td>
                            <td align="right">
                                12-Feb-2002
                            </td>
                            <td>
                                Not Paid &amp; To BeVerified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox2" runat="server" />
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
                                12312
                            </td>
                            <td align="left" class="style1">
                                Basenti Lal Babel
                            </td>
                            <td align="right" class="style2">
                                05-Dec-1983
                            </td>
                            <td align="right">
                                15-Nov-2011
                            </td>
                            <td>
                                Paid &amp; Verified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox3" runat="server" />
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
                                14434
                            </td>
                            <td align="left" class="style1">
                                Ganpal Lal Sharma
                            </td>
                            <td align="right" class="style2">
                                12-Dec-1998
                            </td>
                            <td align="right">
                                10-Jan-2012
                            </td>
                            <td>
                                Not Paid &amp; Verified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox4" runat="server" />
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
                                18534
                            </td>
                            <td align="left" class="style1">
                                Rajendra Kitawat
                            </td>
                            <td align="right" class="style2">
                                02-Jan-1986
                            </td>
                            <td align="right">
                                02-Aug-2002
                            </td>
                            <td>
                                Paid &amp; To BeVerified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox5" runat="server" />
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
                                11223
                            </td>
                            <td align="left" class="style1">
                                Rajendra Kitawat
                            </td>
                            <td align="right" class="style2">
                                05-Dec-1983
                            </td>
                            <td align="right">
                                08-Jul-2005
                            </td>
                            <td>
                                Not Paid &amp; To BeVerified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox6" runat="server" />
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
                                17744
                            </td>
                            <td align="left" class="style1">
                                Surendra Jain
                            </td>
                            <td align="right" class="style2">
                                05-Dec-1983
                            </td>
                            <td align="right">
                                02-Feb-2008
                            </td>
                            <td>
                                Paid &amp; Verified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox7" runat="server" />
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
                                32466
                            </td>
                            <td align="left" class="style1">
                                Surendra Jain
                            </td>
                            <td align="right" class="style2">
                                02-Dec-1983
                            </td>
                            <td align="right">
                                04-May-1980
                            </td>
                            <td>
                                Not Paid &amp; Verified
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox8" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="DivForword" runat="server">
                    <table cellspacing="1" cellpadding="4" border="0" style="width: 100%;">
                        <tr class="gdheader">
                            <th>
                                #
                            </th>
                            <th>
                                Candidate Name
                            </th>
                            <th>
                                Application No.
                            </th>
                            <th class="style1">
                                Father&#39;s Name
                            </th>
                            <th class="style3">
                                D.O.B.
                            </th>
                            <th>
                                &nbsp;Application Date
                            </th>
                            <th>
                                &nbsp;Status
                            </th>
                            <th>
                                &nbsp;
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                1
                            </td>
                            <td>
                                <a href="studentpreview.aspx">Saroj Kitawat</a>
                            </td>
                            <td align="right">
                                11113
                            </td>
                            <td align="left" class="style1">
                                Rajendra Kitawat
                            </td>
                            <td align="right" class="style3">
                                12-Dec-1985
                            </td>
                            <td align="right">
                                09-Jan-1990
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox1" runat="server" />
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
                                11545
                            </td>
                            <td align="left">
                                Surendra Jain
                            </td>
                            <td align="right">
                                02-Jan-1986
                            </td>
                            <td align="right">
                                12-Feb-2002
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox2" runat="server" />
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
                                12376
                            </td>
                            <td align="left">
                                Basenti Lal Babel
                            </td>
                            <td align="right">
                                05-Dec-1983
                            </td>
                            <td align="right">
                                15-Nov-2011
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox3" runat="server" />
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
                                14435
                            </td>
                            <td align="left">
                                Ganpal Lal Sharma
                            </td>
                            <td align="right">
                                12-Dec-1998
                            </td>
                            <td align="right">
                                10-Jan-2012
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox4" runat="server" />
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
                                18576
                            </td>
                            <td align="left">
                                Rajendra Kitawat
                            </td>
                            <td align="right">
                                02-Jan-1986
                            </td>
                            <td align="right">
                                02-Aug-2002
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox5" runat="server" />
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
                                112434
                            </td>
                            <td align="left">
                                Rajendra Kitawat
                            </td>
                            <td align="right">
                                05-Dec-1983
                            </td>
                            <td align="right">
                                08-Jul-2005
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox6" runat="server" />
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
                                17734
                            </td>
                            <td align="left">
                                Surendra Jain
                            </td>
                            <td align="right">
                                05-Dec-1983
                            </td>
                            <td align="right">
                                02-Feb-2008
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox7" runat="server" />
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
                                32455
                            </td>
                            <td align="left">
                                Surendra Jain
                            </td>
                            <td align="right">
                                02-Dec-1983
                            </td>
                            <td align="right">
                                04-May-1980
                            </td>
                            <td>
                                Paid
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkBox8" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<div id="Div_Exam" runat="server">
                    <table cellspacing="1" cellpadding="4" border="0" style="width: 100%;">
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
                                Date
                            </th>
                            <th>
                                Modules
                            </th>
                            <th>
                                Payment status
                            </th>
                            <th>
                                Exam form status
                            </th>
                            <th>
                                &nbsp;
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                1
                            </td>
                            <td align="left">
                                <a href="frmmodulelistforexam.aspx?status=for Examination&course=O level(Computer S/w)&exam=July 2012&R=R/AC/111">R/AC/111</a>
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
                            <td>
                                Pending
                            </td>
                            <td>
                                Recieved
                            </td>
                            <td>
                                <input id="Checkbox7" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                2
                            </td>
                            <td align="left">
                                <a href="frmmodulelistforexam.aspx?status=for Examination&course=O level(Computer S/w)&exam=July 2012&R=R/AC/115">R/AC/115</a>
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
                            <td>
                                Paid
                            </td>
                            <td>
                                Varified
                            </td>
                            <td>
                                <input id="Checkbox8" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                3.
                            </td>
                            <td align="left">
                                <a href="frmmodulelistforexam.aspx?status=for Examination&course=O level(Computer S/w)&exam=July 2012&R=R/AC/123">R/AC/123</a>
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
                            <td>
                                Pending
                            </td>
                            <td>
                                Dispatched
                            </td>
                            <td>
                                <input id="Checkbox9" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                4
                            </td>
                            <td align="left">
                                <a href="frmmodulelistforexam.aspx?status=for Examination&course=O level(Computer S/w)&exam=July 2012&R=R/AC/144">R/AC/144</a>
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
                            <td>
                                Paid
                            </td>
                            <td>
                                Recieved
                            </td>
                            <td>
                                <input id="Checkbox10" type="checkbox" />
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                5
                            </td>
                            <td align="left">
                                <a href="frmmodulelistforexam.aspx?status=for Examination&course=O level(Computer S/w)&exam=July 2012&R= R/AC/185">R/AC/185</a>
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
                            <td>
                                Paid
                            </td>
                            <td>
                                Recieved
                            </td>
                            <td>
                                <input id="Checkbox11" type="checkbox" />
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <div id="DivTotReg" runat="server">
                    <table cellspacing="1" cellpadding="4" border="0" style="width: 100%;">
                        <tr class="gdheader">
                            <th>
                                #
                            </th>
                            <th>
                                Application&nbsp; No.
                            </th>
                            <th>
                                &nbsp;Application&nbsp; Date
                            </th>
                            <th>
                                Candidate Name
                            </th>
                            <th>
                                Father&#39;s Name
                            </th>
                            <th>
                                D.O.B.
                            </th>
                            <th>
                                Current Status
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                1
                            </td>
                            <td align="right">
                                11156
                            </td>
                            <td align="right">
                                09-Jan-1990
                            </td>
                            <td>
                                Saroj Kitawat
                            </td>
                            <td align="left">
                                Rajendra Kitawat
                            </td>
                            <td align="right">
                                12-Dec-1985
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                2
                            </td>
                            <td align="right">
                                11533
                            </td>
                            <td align="right">
                                12-Feb-2002
                            </td>
                            <td>
                                Vishwaraj
                            </td>
                            <td align="left">
                                Surendra Jain
                            </td>
                            <td align="right">
                                02-Jan-1986
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                3.
                            </td>
                            <td align="right">
                                12333
                            </td>
                            <td align="right">
                                15-Nov-2011
                            </td>
                            <td>
                                Puneet
                            </td>
                            <td align="left">
                                Basenti Lal Babel
                            </td>
                            <td align="right">
                                05-Dec-1983
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                4
                            </td>
                            <td align="right">
                                14445
                            </td>
                            <td align="right">
                                10-Jan-2012
                            </td>
                            <td>
                                Khushboo
                            </td>
                            <td align="left">
                                Ganpal Lal Sharma
                            </td>
                            <td align="right">
                                12-Dec-1998
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                5
                            </td>
                            <td align="right">
                                18533
                            </td>
                            <td align="right">
                                02-Aug-2002
                            </td>
                            <td>
                                Neetu
                            </td>
                            <td align="left">
                                Rajendra Kitawat
                            </td>
                            <td align="right">
                                02-Jan-1986
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                6
                            </td>
                            <td align="right">
                                11222
                            </td>
                            <td align="right">
                                08-Jul-2005
                            </td>
                            <td>
                                Kuldeep
                            </td>
                            <td align="left">
                                Rajendra Kitawat
                            </td>
                            <td align="right">
                                05-Dec-1983
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdrow">
                            <td align="right">
                                7
                            </td>
                            <td align="right">
                                17734
                            </td>
                            <td align="right">
                                02-Feb-2008
                            </td>
                            <td>
                                Priyanka
                            </td>
                            <td align="left">
                                Surendra Jain
                            </td>
                            <td align="right">
                                05-Dec-1983
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td align="right">
                                8
                            </td>
                            <td align="right">
                                32477
                            </td>
                            <td align="right">
                                04-May-1980
                            </td>
                            <td>
                                Nitesh Garg
                            </td>
                            <td align="left">
                                Surendra Jain
                            </td>
                            <td align="right">
                                02-Dec-1983
                            </td>
                            <td>
                                Awaiting Registration
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="text-align: right; margin-top: 10px">
                    <asp:Button ID="btnvermark" runat="server" Text="Verify & Marks For Payment" Width="173px"
                        OnClick="btnVerify_Click" Visible="False" />
                    <asp:Button ID="btnVerify" runat="server" Text="Verify" Width="87px" OnClick="btnVerify_Click" />
                    <asp:Button ID="btnReject" runat="server" Text="Reject" Width="86px" />
                    <%--<asp:Button ID="btnok" runat="server" Text="Ok" Width="86px" />--%>
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
