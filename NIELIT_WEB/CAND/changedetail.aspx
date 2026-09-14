<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master"
    CodeFile="changedetail.aspx.cs" Inherits="changedetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<style type="text/css">
        .style2
        {
            width: 162px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Change Detail "></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <ul class="crumbs">
        <li class="first"><a href="myprofile.aspx" style="z-index: 9;"><span></span>Profile</a></li>
        <li><a href="#" style="z-index: 8;">Name Change</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
<br />
    <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
        <tr class="heading">
            <td colspan="3">
                Existing Information
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
                <asp:TextBox runat="server" ID="TextBox4" SkinID="txt248" Enabled="false">Shanti Devi Babel</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                D.O.B.(dd/mm/yyyy)
            </td>
            <td style="width: 33%;" valign="top">
                Contact No.
            </td>
            <td style="width: 33%;" valign="top">
                Email
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:TextBox runat="server" ID="txtdob" SkinID="txt248" Enabled="false">12/07/1985</asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox runat="server" ID="txtcontacts" SkinID="txt248" Enabled="false">9828042637</asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox runat="server" ID="txtemail" SkinID="txt248" Enabled="false">pbabel@gmail.com</asp:TextBox>
            </td>
        </tr>
        <tr class="heading">
            <td colspan="3">
                Update Information
            </td>
        </tr>
        <tr>
            <td valign="top">
                Candidate Name
            </td>
            <td valign="top">
                Father&#39;s Name
            </td>
            <td valign="top">
                Mother&#39;s Name
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtname" SkinID="txt248" runat="server"></asp:TextBox>
            </td>
            <td valign="top" style="height: 25%;">
                <asp:TextBox ID="txtfathers" runat="server" SkinID="txt248"></asp:TextBox>
            </td>
            <td style="height: 25%;" valign="top">
                <asp:TextBox ID="txtmothers" runat="server" SkinID="txt248"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top">
                D.O.B.(dd/mm/yyyy)
            </td>
            <td valign="top">
                Contact No.
            </td>
            <td valign="top">
                Email
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtdobs" SkinID="txt210" runat="server"></asp:TextBox>
            </td>
            <td valign="top" style="height: 25%;">
                <asp:TextBox ID="txtcontactss" runat="server" SkinID="txt248"></asp:TextBox>
            </td>
            <td style="height: 25%;" valign="top">
                <asp:TextBox ID="txtemails" runat="server" SkinID="txt248"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnsave" runat="server" Text="Apply" />
        <asp:Button ID="btncancel" runat="server" Text="Back" OnClientClick = " window.history.back();return false; " />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
