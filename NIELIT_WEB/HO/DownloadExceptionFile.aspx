<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownloadExceptionFile.aspx.cs" Inherits="HO_DownloadExceptionFile"  MasterPageFile="~/MasterPages/main.master" Debug="false"%>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download Refund File"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" runat="server" Visible="false" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClientClick="return ValidateFilter()" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                </script>
            </div>
        </div>
    </asp:Panel>
    <uc1:SearchBar ID="ucSearchBar" SearchTextToolTip="Search by Roll Number" runat="server"
        AutoCompleteFirstRowSelected="True" AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function printStatus() {
            var WindowObject = window.open('', 'PrintWindow', 'width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(document.getElementById('divprint').innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            // window.print();
        }
        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
        function ValidateFormFields() {

            if (!isSelected("<%=ddlpaymentmode.ClientID %>", "Payment Mode"))
                return false;
            if (!isBlank("<%=flUpload.ClientID %>", "Browse File Upload"))
                return false;
        }
       
    </script>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td style="width: 20%;" valign="top">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 80%;" valign="top">
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.xls/xlsx) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 20%;" valign="top">
                <asp:DropDownList ID="ddlpaymentmode" runat="server" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 80%;" valign="top">
                <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                <asp:HiddenField ID="flpath" runat="server" />
            </td>
        </tr>
    </table>
    <div id="divValidateData" runat="server" visible="false" height="150px" width="600px"
        style="overflow: scroll;">
        <table class="sample3" id="tblValidateData" style="width: 100%; text-align: left"
            border="0" cellpadding="3" cellspacing="1">
            <tr class="head1">
                <td align="left" colspan="2">
                    Validate Data
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="20%">
                    Total Records
                </td>
                <td width="80%">
                    <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Validate Records
                </td>
                <td>
                    <asp:Label ID="lblValidateRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%">
                    Not Validate
                </td>
                <td>
                    <asp:Label ID="lblNotValidate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%" valign="top">
                    Failed Records Details
                </td>
                <td>
                    <asp:Label ID="lblFailedRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%" colspan="2">
                    <asp:Label ID="Label12" runat="server" Text="Only Validate records can be uploaded"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnValidate" runat="server" Text="Validate Data" OnClick="btnValidate_Click"
            OnClientClick="return ValidateFormFields()" Visible="false" />
        <asp:Button ID="btnSave" runat="server" Text="Download Refund File" OnClick="btnSave_Click" OnClientClick="return ValidateFormFields()" />
        <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
