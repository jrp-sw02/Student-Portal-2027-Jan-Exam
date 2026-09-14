<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewResult.aspx.cs" Inherits="ViewResult"%>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .summary_block
        {
            height: 566px;
        }
    </style>
 
</head>
<body style="height: 566px; background-color:#FFFFFF;" >
    <form id="form1" runat="server">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" class="preview"
            style="font-size: 14px;">
            <tr>
                <td colspan="3">
                    <uc2:NormalHeader ID="NormalHeader2" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center" style="border-bottom: 1px solid #000000;" valign="middle" colspan="3">
                    <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 20px;"
                        OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                        ImageUrl="~/images/print.gif" ToolTip="Print Form" />
                </td>
            </tr>
            <tr class="normal">
                <td colspan="3" align="center" class="rightBorder" style="padding: 5px;">
                    <asp:Label ID="Lblcourse" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3" style="font-size: 16px; padding-top: 10px;">
                    Candidate's Personal Details
                </td>
            </tr>
            <tr class="normal">
                <td width="40%">
                    Candidate Name
                </td>
                <td class="rightBorder">
                    <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal" id="trfathername" runat="server">
                <td width="40%">
                    Father's Name
                </td>
                <td class="rightBorder">
                    <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal" id="trmothername" runat="server">
                <td width="40%">
                    Mother's Name
                </td>
                <td class="rightBorder">
                    <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal" runat="server" visble="false" id="trgname">
                    <td width="40%">
                        Guardian Name
                    </td>
                    <td class="rightBorder">
                        <asp:Label ID="Lgname" runat="server" Text=""></asp:Label>
                    </td>
             </tr>
            <tr class="head1">
                <td colspan="3" style="font-size: 16px; padding-top: 10px;">
                    Result Details
                </td>
            </tr>
            <tr class="normal">
                <td>
                    Roll Number
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="LblRollno" runat="server" Text=""></asp:Label>
                </td>
            </tr>
           <%-- <tr class="normal">
                <td>
                    Course Name
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lblccname" runat="server" Text=""></asp:Label>
                </td>
            </tr>--%>
            <tr class="normal">
                <td>
                    Exam Name
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lblexam" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td>
                    Date of Exam
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lblexamdate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td>
                    Exam Centre Name
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lblexamcentre" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td>
                    CCC No.
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lblcc" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td>
                    Institute Name
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lbliname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td valign="top">
                    Result
                </td>
                <td colspan="2" class="rightBorder">
                    <asp:Label ID="Lblresult" runat="server" Text=""></asp:Label>
                </td>
            </tr>
             <tr class="normal">
                <td valign="top">
                    Result Published Date
                </td>
                <td colspan="2">
                    <asp:Label ID="LblResultDate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td style="font-size: 16px; padding-top: 10px;" colspan="3">
                    Grade Legends
                </td>
            </tr>
            <tr>
                <td width="100%" id="tdLegends" runat="server" style="font-size: 12px; padding-top: 0px;"
                    colspan="2" class="rightBorder">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
