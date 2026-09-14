<%@ Page Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ExamDetail.aspx.cs" Inherits="ExamDetail" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sidetable a
        {
            color: #000000;
            text-decoration: none;
        }
        .sidetable a:hover
        {
            color: #3366CC;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Detail"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
<ul class="crumbs">
 <li class="first"><a href="frmDashboard.aspx" style="z-index:9;"><span></span>Home</a></li>
 <li><a href="frmMycourse.aspx" style="z-index:8;">My Course
 <li><a href="#" style="z-index:7;" id="l1" runat="server" visible="false"><span id="Span1" runat="server"></span></a></li>
 <%--<li><a href="#" style="z-index:6;">Exam Details</a></li>--%>
</ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <div class="summary_block">
        <span  runat="server">Exam Details of  Level </span>
        <table width="100%">
            <tr>
                <td width="15%">
                    Registration No.
                </td>
                <td width="2%">
                    :
                </td>
                <td>
                    1234
                </td>
                <td width="15%">
                    Registration Date
                </td>
                <td width="2%">
                    :
                </td>
                <td>
                    01/01/2012
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <table class="gdbody" cellspacing="1" cellpadding="4" id="cphContents_gvMain" style="width: 97%;">
                        <tr class="gdheader">
                            <th scope="col" style="width: 2%;">
                                #
                            </th>
                            <th scope="col" style="width: 13%;">
                                Name of Exam
                            </th>
                            <th scope="col" style="width: 20%;">
                                Date of Exam
                            </th>
                            <th scope="col" style="width: 25%;">
                                Module Attempted
                            </th>
                            <th scope="col" style="width: 15%;">
                                Result
                            </th>
                        </tr>
                        <tr class="gdrow">
                            <td>
                                1
                            </td>
                            <td>
                                <asp:LinkButton ID="LnkExam1" runat="server" Text="Jan 2012" OnClick="LnkExam1_Click"></asp:LinkButton>
                            </td>
                            <td>
                                15-Jan-2012 to 15-Feb-2012
                            </td>
                            <td>
                                M1, M2
                            </td>
                            <td>
                                Pending
                            </td>
                        </tr>
                        <tr class="gdalternate">
                            <td>
                                2
                            </td>
                            <td>
                                <asp:LinkButton ID="LnkExam2" runat="server" Text="July 2011" OnClick="LnkExam2_Click"></asp:LinkButton>
                            </td>
                            <td>
                                15-Jul-2012 to 15-Aug-2012
                            </td>
                            <td>
                                M1, M2, M3, M4, M5
                            </td>
                            <td>
                                Declared
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <%--<table style="width:100%;" class="search_result">
        <tr>
            <td width="600">
                </td>
            <td align="right">
                &nbsp;</td>
            <td align="left" width="100">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="600" align="right">
                <h3>
                    <strong>Registration No</strong></h3>
            </td>
            <td align="right">
                &nbsp;</td>
            <td align="left" width="100">
                12345</td>
        </tr>
        <tr>
            <td colspan="3">
                <div visible="false" id="div_O" style="border: 1px solid #003300;" runat="server">
                <table width="100%">
                <tr>
                <td >
                    <table widh="100%">
                        <tr>
                            <td align="left" class="heading2" colspan="2" style="width: 720px" width="600">
                                O Level
                            </td>
                        </tr>
                    </table>
                    </td>
                </tr>
                 <tr>
                <td >
                  <table width="100%" cellpadding="0" cellspacing="0">
                  <tr>
                  <th bgcolor="#CAE4FF" width="100">Exam Name</th>
                  <th bgcolor="#CAE4FF" class="style4">Module Attempted</th>
                  <th bgcolor="#CAE4FF" class="style2">Exam Date</th>
                  <th bgcolor="#CAE4FF" width="100">Cleared</th>
                  <th bgcolor="#CAE4FF">NotCleared</th>
                  </tr>
                  <tr>
                  <td align="center" class="style1" width="100">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="frmexamhistory.aspx">July 2011</a></td>
                  <td align="center" class="style4">M1,M3,M4</td>
                  <td align="center" class="style2">7-07-2010</td>
                  <td align="center" class="style1" width="100">1</td>
                  <td align="center" class="style1">2</td>
                  </tr>
                  <tr>
                  <td align="center" width="100"><a href="frmexamhistory.aspx">Jan 2012</a></td>
                  <td align="center" class="style4">M2,M3</td>
                  <td align="center" class="style2">08-01-2011</td>
                  <td align="center" width="100">0</td>
                  <td align="center">2</td>
                  </tr>
                  <tr>
                  <td align="center" width="100"><a href="frmexamhistory.aspx">July 2012</a></td>
                  <td align="center" class="style4">M4,M3,M2</td>
                  <td align="center" class="style2">12-01-2011</td>
                  <td align="center" width="100">2</td>
                  <td align="center">1</td>
                  </tr>
                  <tr>
                  <td align="center" width="100">:<br />
                      :</td>
                  <td align="center" class="style4">:<br />
                      :</td>
                  <td align="center" class="style2">:<br />
                      :</td>
                  <td align="center" width="100">:<br />
                      :</td>
                  <td align="center">:<br />
                      :</td>
                  </tr>
                  <tr>
                  <td align="center" width="100">&nbsp;</td>
                  <td align="center" class="style4">&nbsp;</td>
                  <td align="center" class="style2">
                      &nbsp;</td>
                  <td align="center" colspan="2" width="100">
                      &nbsp;</td>
                  </tr>
                  </table>
                  
                   </td>
                </tr>
                 </table>
                </div></td>
        </tr>
        <tr>
            <td colspan="3">
            <div id="div_A" visible="false" style="border: 1px solid #003300;" runat="server">
            <table widh="100%">
                <tr>
                    <td class="heading2" width="600" align="left" colspan="2" style="width: 720px">
                        A Level
                    </td>
                </tr>
            </table>
                
                <table width="100%">
                 <tr>
                <td >
                  <table width="100%" cellpadding="0" cellspacing="0">
                  <tr>
                  <th bgcolor="#CAE4FF" width="100">Exam Name</th>
                  <th bgcolor="#CAE4FF" class="style3">Module Attempted</th>
                  <th bgcolor="#CAE4FF" width="140">Exam Date</th>
                  <th bgcolor="#CAE4FF" width="100">Cleared</th>
                  <th bgcolor="#CAE4FF">NotCleared</th>
                  </tr>
                  <tr>
                  <td align="center" class="style1" width="100"><a href="#">July 2011</a></td>
                  <td align="center" class="style3">M1,M3,M4</td>
                  <td align="center" class="style1" width="140">7-07-2010</td>
                  <td align="center" class="style1" width="100">1</td>
                  <td align="center" class="style1">2</td>
                  </tr>
                  <tr>
                  <td align="center" width="100"><a href="#">Jan 2012</a></td>
                  <td align="center" class="style3">M2,M3</td>
                  <td align="center" width="140">08-01-2011</td>
                  <td align="center" width="100">0</td>
                  <td align="center">2</td>
                  </tr>
                  <tr>
                  <td align="center" width="100"><a href="#">July 2012</a></td>
                  <td align="center" class="style3">M4,M3,M2</td>
                  <td align="center" width="140">12-01-2011</td>
                  <td align="center" width="100">2</td>
                  <td align="center">1</td>
                  </tr>
                  <tr>
                  <td align="center" width="100">:<br />
                      :</td>
                  <td align="center" class="style3">:<br />
                      :</td>
                  <td align="center" width="140">:<br />
                      :</td>
                  <td align="center" width="100">:<br />
                      :</td>
                  <td align="center">:<br />
                      :</td>
                  </tr>
                  <tr>
                  <td align="center" width="100">&nbsp;</td>
                  <td align="center" class="style3">&nbsp;</td>
                  <td align="center" width="140">
                      &nbsp;</td>
                  <td align="center" colspan="2" width="100">
                      &nbsp;</td>
                  </tr>
                  </table>
                  
                   </td>
                </tr>
                 </table>
                </div> </td>
        </tr>
    </table>--%>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
  <uc2:SideLink ID="Sidelink" runat="server" />
  <uc3:sidelink ID="Sidelink1" runat="server" />  
</asp:Content>
