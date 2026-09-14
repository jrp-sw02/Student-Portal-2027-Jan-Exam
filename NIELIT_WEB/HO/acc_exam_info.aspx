<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="acc_exam_info.aspx.cs" Inherits="HO_acc_exam_info" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Application Status"></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">

 
<ul class="crumbs">
<li class="first"><a href="../FrmDashBoard.aspx" style="z-index:9;"><span></span>Home</a></li>
	 <li ><a href="#" style="z-index:8;">Exam Application Status</a></li>   
</ul> 
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
<div>
  <table cellspacing="1" cellpadding="4" border="0" id="cphContents_gvMain" 
        style="width: 100%;">
        <tr class="gdheader">
            <th>
                #
            </th>
            <th>
                Course Name</th>
            <th >
                Session</th>
            <th>
                 Pending Verification
            </th>
            <th>
                 Verified But Not Paid</th>
            <th>
                 Pending 
                 For Payment</th>
            <th>
                 Pending For Approval</th>
        </tr>
        <tr class="gdrow">
            <td>
                1
            </td>
            <td>
                O level(Computer S/w)</td>
            <td>
                 July 2012</td>
            <td align="right">
                 <a href="MystudentList.aspx?status=For Pending Verification&course=Pending For Verification&exam=July 2012&st=0&sts=j">40</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Verified But Not Paid&course=Verified But Not Paid&exam=July 2012&st=3&sts=a">10</a>
            </td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p">20</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Approvals&course=Pending For Approval&exam=July 2012&st=2&sts=f">90</a></td>
        </tr>
        <tr class="gdalternate">
            <td>
                2
            </td>
            <td>
                Olevel(Computer S/w)</td>
            <td>
                January 2012</td>
            <td align="right">
               <a href="MystudentList.aspx?status=For Pending Verification&course=Pending For Verification&exam=July 2012&st=0&sts=j">190</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Verified But Not Paid&course=Verified But Not Paid&exam=July 2012&st=3&sts=a">20</a>
            </td>
            <td align="right">
               <a href="MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p">50</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Approval&course=Pending For Approval&exam=July 2012&st=2&sts=f">130</a></td>
           </tr>
        <tr class="gdrow">
            <td>
                3.</td>
            <td>
               A level(Computer S/w)</td>
            <td>
                July 2011</td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Verification&course=Pending For Verification&exam=July 2012&st=0&sts=j">33</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Verified But Not Paid&course=Verified But Not Paid&exam=July 2012&st=3&sts=a">25</a>
            </td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p">21</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Approval&course=Pending For Approval&exam=July 2012&st=2&sts=f">77</a></td>
        </tr>
        <tr class="gdalternate">
            <td>
                4</td>
            <td>
               A level(Computer S/w)</td>
            <td>
                January 2011</td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Verification&course=Pending For Verificatio&exam=July 2012&st=0&sts=j">45</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Verified But Not Paid&course=Verified But Not Paid&exam=July 2012&st=3&sts=a">12</a>
            </td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p">23</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Approval&course=Pending For Approval&exam=July 2012&st=2&sts=f">13</a></td>
        </tr>
        <tr class="gdrow">
            <td>
                5</td>
            <td>
               B level(Computer S/w)</td>
            <td>
                July 2011</td>
            <td align="right">
                 <a href="MystudentList.aspx?status=For Pending Verification&course=Pending For Verificatio&exam=July 2012&st=0&sts=j">40</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Payment&course=Verified But Not Paid&exam=July 2012&st=3&sts=a">15</a>
            </td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p">20</a></td>
            <td align="right">
                <a href="MystudentList.aspx?status=For Pending Approval&course=Pending For Approval&exam=July 2012&st=2&sts=f">90</a></td>
        </tr>
        </table>
</div>
    <link href="CSS/puda.css" rel="stylesheet" type="text/css" />
    <link href="CSS/login.css" rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    
      

</asp:Content>
