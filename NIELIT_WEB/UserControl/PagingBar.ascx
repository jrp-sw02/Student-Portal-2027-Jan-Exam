<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagingBar.ascx.cs" Inherits="UserControl_PagingBar" %>
<script language="javascript" type="text/javascript">
    function ValidateNumber(ctl, maxValue) {
        if (parseInt(ctl.value) > 0 && parseInt(ctl.value) <= maxValue) {

        }
        else if (parseInt(ctl.value) <= 0 || ctl.value == '') {
            ctl.value = "1"
        }
        else if (parseInt(ctl.value) > maxValue) {
            ctl.value = maxValue

        }
        else
            return false;
        __doPostBack(ctl.id, "");
        return false;
    }
</script>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <progresstemplate>
  <%--<img id="imgL" style=" position:absolute; left:250px; top:150px;" src="~/images/loading.gif" />--%>
    <asp:Image runat="server" ImageUrl="~/images/loading.gif" Style="position:absolute; left:250px; top:150px;" />
</progresstemplate>
</asp:UpdateProgress>
<table align="center" border="0" cellpadding="0" cellspacing="0" class="navigator"
    width="100%">
    <tr>
        <td align="left" width="5%">
            &nbsp;<asp:ImageButton runat="server" ID="btnRefresh" BorderStyle="None" ImageUrl="~/images/refresh_btn.jpg"
                ImageAlign="AbsMiddle" OnClientClick="this.src='../images/indicator.gif';" Width="23px"
                Height="23px" ToolTip="Refresh" OnClick="btnRefresh_Click" Style="margin: 0px" />
        </td>
        <td width="18%" align="center">
            Records Per Page
        </td>
        <td width="5%">
            <asp:DropDownList ID="ddlRecordNo" Enabled="false" Width="50px" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlRecordNo_SelectedIndexChanged">
                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                <asp:ListItem Value="10" Selected="True" Text="10"></asp:ListItem>
                <asp:ListItem Value="15" Text="15"></asp:ListItem>
                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                <asp:ListItem Value="100" Text="100"></asp:ListItem>
                <asp:ListItem Value="200" Text="200"></asp:ListItem>
                <asp:ListItem Value="0" Text="All"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td align="center" width="25%">
            Total Records :
            <asp:Label ID="lblTotRecords" runat="server" Text="0"></asp:Label>
        </td>
        <td align="right" valign="middle" width="8%">
            <asp:Button ID="btnFirst" TabIndex="1" runat="server" Text="First" CommandArgument="F"
                Enabled="False" OnClick="btnNavigate_Click" SkinID="btnNavigator" />
        </td>
        <td align="right" valign="middle" width="8%">
            <asp:Button ID="btnPrev" TabIndex="2" runat="server" Text="Prev" CommandArgument="P"
                Enabled="False" OnClick="btnNavigate_Click" SkinID="btnNavigator" />
        </td>
        <td align="center" valign="middle" width="7%">
            <asp:TextBox ID="txtPageNumber" TabIndex="0" onchange="return ValidateNumber(this,txtTotPage.value)"
                onkeypress="checkNumber(this,(txtTotPage.value.length+1),0)" Text="1" Width="30px"
                runat="server" Style="text-align: center;" OnTextChanged="txtPageNumber_TextChanged"></asp:TextBox>
        </td>
        <td align="center" valign="middle" width="3%">
            of
        </td>
        <td align="center" valign="middle" width="7%">
            <asp:TextBox ID="txtTotPage" ClientIDMode="Static" Text="0" ReadOnly="true" Width="30px"
                runat="server" Style="text-align: center;"></asp:TextBox>
        </td>
        <td align="left" valign="middle" width="8%">
            <asp:Button ID="btnNext" Enabled="false" TabIndex="3" runat="server" Text="Next"
                CommandArgument="N" OnClick="btnNavigate_Click" SkinID="btnNavigator" />
        </td>
        <td align="left" valign="middle" width="8%">
            <asp:Button ID="btnLast" Enabled="false" TabIndex="4" runat="server" Text="Last"
                CommandArgument="L" OnClick="btnNavigate_Click" SkinID="btnNavigator" />
        </td>
    </tr>
</table>
