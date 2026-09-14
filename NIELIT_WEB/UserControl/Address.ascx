<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Address.ascx.cs" Inherits="UserControl_Address"%>
<script language="javascript" type="text/javascript">
</script>
<table class="sample2" width="100%" cellpadding="2" cellspacing="0">
    <tr>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Address Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                Width="100%"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="lbl1" runat="server" SkinID="CaptionLabel" Text="Address Line 1 &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                Width="100%"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Address Line 2 &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                Width="100%"></asp:Label>
        </td>
    </tr>
    <tr class="even">
        <td style="width: 33%;" valign="top">
            <asp:DropDownList ID="ddlAddrType" runat="server" SkinID="ddl250">
                <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Address Line 3"
                Width="100%"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="State &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="District "></asp:Label>
        </td>
    </tr>
    <tr class="even">
        <td style="width: 33%;" valign="top">
            <asp:TextBox ID="txtAddress3" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <asp:DropDownList ID="ddlState" runat="server" SkinID="ddl250" AutoPostBack="true"
                OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
            </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlDistrict" runat="server" SkinID="ddl250">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    </asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="City &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Pin &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text=""></asp:Label>
        </td>
    </tr>
    <tr class="even">
        <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
            <asp:TextBox ID="txtCity" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>
        </td>
        <td style="width: 33%;" valign="top">
            <asp:TextBox ID="txtPin" runat="server" MaxLength="6" SkinID="txt248"></asp:TextBox>
        </td>
        <td style="width: 33%;" valign="top">
        </td>
    </tr>
</table>
