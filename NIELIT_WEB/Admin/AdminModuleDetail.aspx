<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="AdminModuleDetail.aspx.cs" Inherits="Admin_AdminModuleDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
<ul class="crumbs">
	    <li class="first"><a href="AdminRegstud.aspx" style="z-index:9;"><span></span>Registered Students</a></li>
	    <li><a href="AdminRegstud.aspx?key1=<%=Session["regno"].ToString() %>&name=<%=Session["name"].ToString() %>" style="z-index:8;"><%=Session["regno"].ToString() %></a></li>
	    <li id="licourse" runat="server"><a href="AdminCourses.aspx" style="z-index:7;"> Courses:</a></li>
        <li id="liLevel" runat="server"><a href="AdminCourses.aspx" style="z-index:6;">  <asp:Label  ID="Label2" runat="server" Text=""></asp:Label></a></li>
        <li><a href="#" style="z-index:5;">Module detail</a></li>
    </ul>



</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<div class="summary_block">
     <span id="Span1" runat="server">Module Status</span>
        <table width="100%">
            
            <tr>
                <td colspan="6">
                <table class="gdbody" cellspacing="1" cellpadding="4" id="Table2" style="width:100%;">
        <tr class="gdheader">
            <th scope="col" style="width: 3%;">
                #
            </th>
            <th scope="col" style="width: 20%;">
                Module </th>
            <th scope="col" style="width: 20%;">
                No Of Attempted</th>
            <th scope="col" style="width: 20%;">
                Last Exam</th>
            <th scope="col" style="width: 20%;">
                Date of Exam
            </th>
            <th scope="col" style="width: 15%;">
                 Result</th>
            <th scope="col" style="width: 15%;">
                 Grade</th>
        </tr>
        <tr class="gdrow">
            <td>
                1
            </td>
            <%--<td>
                <a href="AdminExamHistory.aspx"> M1</a></td>--%>
                <td>
                M1
                </td>
            <td align="center">
                4</td>
            <td>
                Jan-2012</td>
            <td>
                15-Jan-2012
            </td>
            <td>
                Pass</td>
            <td>
                A</td>
        </tr>
        <tr class="gdalternate">
            <td>
                2
            </td>
            <%--<td>
                <a href="AdminExamHistory.aspx">M2</a></td>--%>
                <td>
                M2
                </td>
            <td align="center">
                3</td>
            <td>
                Jan-2012</td>
            <td>
                17-Jan-2012</td>
            <td>
                Fail
            </td>
            <td>
                F</td>
        </tr>
        <tr class="gdrow">
            <td>
                3</td>
            <%--<td>
                <a href="AdminExamHistory.aspx">M3</a></td>--%>
                <td>
                M3
                </td>
            <td align="center">
                1</td>
            <td>
                Jan-2012</td>
            <td>
                18-Jani-2012</td>
            <td>
                Persuing</td>
            <td>
                -</td>
        </tr>
        <tr class="gdalternate">
            <td>
                4</td>
            <%--<td>
                <a href="AdminExamHistory.aspx">M4</a></td>--%>
                <td>
                M4
                </td>
            <td align="center">
                2</td>
            <td>
                July-2012</td>
            <td>
                12-July-2012</td>
            <td>
                Pass</td>
            <td>
                B</td>
        </tr>
        <tr class="gdrow">
            <td>
                5</td>
            <td>
                M5</td>
            <td>
               </td>
            <td>
                -</td>
            <td>
                -</td>
            <td>
                Not Attempted</td>
            <td>
                </td>
        </tr>
        <tr class="gdalternate">
            <td>
                6</td>
            <td>
                Project</td>
            <td>
               </td>
            <td>
                -</td>
            <td>
                -</td>
            <td>
                Not Attempted</td>
            <td>
               </td>
        </tr>
    </table>
                </td>
               
            </tr>
        </table>
       
    
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

