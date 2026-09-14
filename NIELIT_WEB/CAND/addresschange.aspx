<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="addresschange.aspx.cs" Inherits="addresschange" %>

<%@ Register Src="../UserControl/Address.ascx" TagName="Address" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Address Change"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <ul class="crumbs">
        <li class="first"><a href="myprofile.aspx" style="z-index: 9;"><span></span>Profile</a></li>
        <li><a href="#" style="z-index: 8;">Address Change</a></li>
    </ul>
    <%--<a href="myprofile.aspx">Profile</a>>><a href="#">Address Change</a>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <br />
    <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
        <tr class="heading">
            <td colspan="3">
                Personal Detail
            </td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                Candidate Name
            </td>
            <td style="width: 33%;" valign="top">
                Father&#39;s Name
            </td>
            <td style="width: 33%;" valign="top">
                Mother&#39;s Name
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:TextBox runat="server" ID="txtcandname" SkinID="txt248" Enabled="false">Punit Babel</asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox runat="server" ID="txtfather" SkinID="txt248" Enabled="false">Mr. Basantilal Babel</asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox runat="server" ID="txtmother" SkinID="txt248" Enabled="false">Shati Devi Babel</asp:TextBox>
            </td>
        </tr>
        <tr class="heading">
            <td colspan="3">
                Correspondence Details
            </td>
        </tr>
        <tr>
            <td valign="top">
                Mobile No.
            </td>
            <td valign="top">
                Telephone No.
            </td>
            <td valign="top">
                Email
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtcontact" SkinID="txt248" runat="server">9828043637</asp:TextBox>
            </td>
            <td valign="top" style="height: 25%;">
                <asp:TextBox ID="txttelephone" runat="server" SkinID="txt248">0292-243456</asp:TextBox>
            </td>
            <td style="height: 25%;" valign="top">
                <asp:TextBox ID="txtemail" runat="server" SkinID="txt248">pbabel@gmail.com</asp:TextBox>
            </td>
        </tr>
    </table>
    <uc1:Address ID="Address1" runat="server" />
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnsave" runat="server" Text="Apply" />
        <asp:Button ID="btncancel" runat="server" Text="Back" OnClientClick ="window.history.back();return false;" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
