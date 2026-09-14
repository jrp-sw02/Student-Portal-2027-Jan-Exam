<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCourseResult.aspx.cs"
    Inherits="WEB_ViewCourseResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body style="height: 566px; background-color: #FFFFFF;">
    <form id="form1" runat="server">
    <table align="center" border="0" cellpadding="2" cellspacing="0" width="977px" class="preview"
        style="font-size: 14px;">
        <tr>
            <td colspan="2">
                <uc2:NormalHeader ID="NormalHeader2" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center" style="border-bottom: 1px solid #000000;" valign="middle" colspan="2">
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 20px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr class="normal">
            <td colspan="3" align="center" class="rightBorder" style="padding: 5px;">
                <asp:Label ID="Lblcourse" runat="server" Text="" Font-Bold="true" Font-Size="14pt"></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="2" style="font-size: 16px; padding-top: 10px;">
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
            <td colspan="2" style="font-size: 16px; padding-top: 10px;">
                Result Details
            </td>
        </tr>
        <tr class="normal">
            <td width="40%">
                Roll Number
            </td>
            <td class="rightBorder">
                <asp:Label ID="LblRollno" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="normal">
            <td width="40%">
                Course Name
            </td>
            <td class="rightBorder">
                <asp:Label ID="Lblccname" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="normal">
            <td width="40%">
                Exam Name
            </td>
            <td class="rightBorder">
                <asp:Label ID="Lblexam" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="normal">
            <td width="40%">
              Result Published Date
            </td>
            <td class="rightBorder">
                <asp:Label ID="LblResultDate" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="normal">
            <td width="40%">
                Remark
            </td>
            <td class="rightBorder">
                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="2" style="font-size: 16px; padding-top: 10px;">
                Exam Details
            </td>
        </tr>
    </table>
    <div>
        <table align="center" cellpadding="0" cellspacing="0" width="977px" class="preview"
            style="font-size: 14px;">
            <tr>
                <td>
                    <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="977px" AllowPaging="false" EnableTheming="false" BorderColor="Black">
                        <Columns>
                            <asp:BoundField HeaderStyle-Width="8%" HeaderStyle-BorderColor="Black" HeaderText="Code"
                                DataField="Code" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="60%" HeaderStyle-BorderColor="Black" HeaderText="Module Name"
                                DataField="name" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" HeaderText="Theory Marks" HeaderStyle-BorderColor="Black"
                                DataField="TheoryMarks" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" HeaderText="Practical Marks" HeaderStyle-BorderColor="Black"
                                DataField="PracticalMarks" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" HeaderText="Weighted Marks (Wherever Applicable)" HeaderStyle-BorderColor="Black"
                                DataField="WeightedMarks" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" HeaderText="Result" HeaderStyle-BorderColor="Black"
                                DataField="Result" ItemStyle-BorderColor="Black" />
                            <asp:BoundField HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderText="Grade Legend"
                                HeaderStyle-BorderColor="Black" DataField="Grade" ItemStyle-BorderColor="Black" />
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="2" style="font-size: 16px; padding-top: 10px;">
                    Grade Legends
                </td>
            </tr>
            <tr class="head1">
                <td width="100%" id="tdLegends" runat="server" style="font-size: 12px; padding-top: 0px;">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
