<%@ Page Language="C#"  MasterPageFile="~/MasterPages/main.master" CodeFile="BookletGenerate.aspx.cs" Inherits="Admin_BookletGenerate" AutoEventWireup="false"  Debug="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
   
   
    <table class="sample2" cellpadding="2" cellspacing="0">
        <tr class="gdalternate1">
            <td colspan="4" style="text-align: left">
                <asp:Label ID="Label1" runat="server" Width="750px" Font-Bold="True" 
                    Text="Generate Data for Website"></asp:Label>
            </td>
        </tr>
      
        <tr class="gdalternate1">
            <td colspan="4" align="center" style="height: 33px" valign="middle">
                <asp:Button ID="btnUpload" runat="server" Text="Generate Booklet for O/A/B/C"  Font-Bold="True" OnClick="btnUpload_Click"></asp:Button>
            </td>
              </tr>
              <tr class="gdalternate1">
            <td align="center" colspan="4" style="height: 33px; text-align: left;" valign="middle">
                <asp:Label ID="Lblmessage" runat="server"></asp:Label></td>
        </tr>
      
    </table>
            
        
</asp:Content>

