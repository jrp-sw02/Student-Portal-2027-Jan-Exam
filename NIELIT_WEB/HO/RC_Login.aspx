
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RC_Login.aspx.cs"
    Inherits="RC_Login" MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    RC Login
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">

    <asp:Label ID="lblError" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"></asp:Label>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Username &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Password &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtUsername" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" SkinID="txt248" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
    </div>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>