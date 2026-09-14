<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CertificateExamApplicationResultUpdate.aspx.cs" Inherits="HO_CertificateExamApplicationResultUpdate"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Result Update ABS to @"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isBlank("<%=flUpload.ClientID %>", "Browse File Upload"))
                return false;
        }
       
    </script>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td style="width: 100%;" valign="top">
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.xls/xlsx) &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 100%;" valign="top">
                <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                <asp:HiddenField ID="flpath" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <td>
                List of Invalidated Records (Application Number) : <br/>
                <asp:Label ID="InValidLabel" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnValidate" runat="server" Text="Validate Data" OnClick="btnValidate_Click"
            OnClientClick="return ValidateFormFields()" />
        <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
