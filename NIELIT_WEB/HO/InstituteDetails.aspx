<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="InstituteDetails.aspx.cs" Inherits="HO_InstituteDetails" Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Institute Detail"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
        cellspacing="1">
        <tr class="head1">
            <td align="left" colspan="2">
                Institute Details
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Name
            </td>
            <td width="44%">
                <asp:Label ID="lblName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Address
            </td>
            <td width="44%">
                <asp:Label ID="lblAddress" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Contact Person
            </td>
            <td width="44%">
                <asp:Label ID="LblContactPerson" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Email-Id
            </td>
            <td width="44%">
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Mobile Number
            </td>
            <td width="44%">
                <asp:Label ID="lblMobile" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Land-line Number
            </td>
            <td width="44%">
                <asp:Label ID="lblLandline" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Fax Number
            </td>
            <td width="44%">
                <asp:Label ID="LblFax" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Accreditation/Facilitation
            </td>
            <td width="44%">
                <asp:Label ID="LblAccr" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

