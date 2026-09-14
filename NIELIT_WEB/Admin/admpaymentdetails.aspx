<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="admpaymentdetails.aspx.cs" Inherits="admpaymentdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
 <asp:Label ID="lblHeading" runat="server" Text="Payment Details"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
<ul class="crumbs">
 <li class="first"><a href="adminaccrediatedcenter.aspx" style="z-index:9;"><span></span>Accredited Centers</a></li>
 <li><a href="adminaccrediatedcenter.aspx?name="+ <%=Request.QueryString["name"] %>" style="z-index:8;">Aishwarya College</a></li>
 <li><a href="accrediationdetails.aspx" style="z-index:7;">Accrediation Details</a></li>
 <li><a href="accrediationdetails.aspx?key=ACR101&adate=01/01/2012&vdate=01/01/2017&course=O" style="z-index:6;"><span id="Span1" runat="server">ACC101</span></a></li>
 <li><a href="#" style="z-index:5;">Payment Details</a></li>
</ul>
<%--<a href="adminaccrediatedcenter.aspx">Accredited Centers</a>:<a href="adminaccrediatedcenter.aspx?name="+ <%=Request.QueryString["name"] %>>Aishwarya College</a>>><a href="accrediationdetails.aspx">Accrediation Details</a>:<a href="accrediationdetails.aspx?key=ACR101&adate=01/01/2012&vdate=01/01/2017&course=O"><span id="aclink" runat="server">ACC101</span></a>>><a href="#">Payment Details</a>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
 <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td align="center" valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                   
                    <tr>
                        <td valign="top" style="min-height:400px; text-align:left">
                            <h2 align="center">
                                &nbsp;PAYMENT DETAILS
                            </h2>
                            <div id="d1" runat="server">
                                <table id="Table2" width="95%" align="center" class="sample" runat="server" style="background-color:#ffffff;">
                                    <tr>
                                        <td colspan="2" class="odd1">
                                            Payment Detail</td>
                                    </tr>
                                    <tr>
                                        <td class="odd" width="30%" >
                                            Payment Receipt No. and Date 
                                        </td>
                                        <td class="odd" width="70%" >
                                            12345556 Dated 15-Nov-2012
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="even" width="30%" >
                                            Payment Description
                                        </td>
                                        <td class="even" width="70%" >
                                            Accrediation Fee For O Level
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="odd" width="30%" >
                                            Amount (in rupees)
                                        </td>
                                        <td class="odd" width="70%" >
                                            12000/- (Rs. Five Hundred Only)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="odd" width="30%" >
                                            Payment Mode</td>
                                        <td class="odd" width="70%" >
                                            ONLINE</td>
                                    </tr>
                                    </table>
                            </div>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

