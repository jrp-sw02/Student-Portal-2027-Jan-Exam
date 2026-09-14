<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormStatus.ascx.cs" Inherits="UserControl_FormStatus" %>
<script language="javascript" type="text/javascript">
    function printStatus() {
        var WindowObject = window.open('PrintForm.aspx', 'PrintWindow', 'width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes');
        WindowObject.document.getElementById("divresult").innerHTML = (document.getElementById("divStatus").innerHTML);
                            WindowObject.document.close();
                            WindowObject.focus();
                            WindowObject.print();
        window.print();
    }
</script>
<div align="right"><asp:Label ID="Lbldate" runat="server" Text="" Style="font-weight:bold;font-size: large;"></asp:Label></div>
<div style="width: 100%; vertical-align: top;" id="divfull" runat="server">
    <div id="divStatus" runat="server">
        <table align="center" border="0" cellpadding="3" class="sample3" width="100%">
            <tr class="head1">
                <th colspan="3" align="center">
                    <asp:Label ID="Lblcourse" runat="server" Text="" Style="text-align: center;"></asp:Label>
                </th>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Applicant Name
                </td>
                <td>
                    <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
                </td>
                <td width="10%" rowspan="4" align="center" valign="top">
                    <img id="imgcandphoto" runat="server" style="height: 111px; width: 90px" />
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Date of Birth
                </td>
                <td>
                    <asp:Label ID="LblDOB" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Father's Name
                </td>
                <td>
                    <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%">
                    Mother's Name
                </td>
                <td >
                    <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    Application Details:-
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Application No
                </td>
                <td colspan="2">
                    <asp:Label ID="LblAppno" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Application Date
                </td>
                <td colspan="2">
                    <asp:Label ID="LblAppdate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Course Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblccname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trexcycle" runat="server">
                <td>
                    Exam cycle
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblecycle" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Current Status
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblss" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px; width: 100%;">
        <asp:Button ID="BtnBack" runat="server" Text="Back" OnClick="BtnBack_Click" />
    </div>
</div>
