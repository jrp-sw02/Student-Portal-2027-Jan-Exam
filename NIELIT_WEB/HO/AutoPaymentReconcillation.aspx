<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoPaymentReconcillation.aspx.cs"
    Inherits="HO_AutoPaymentReconcillation" MasterPageFile="~/MasterPages/main.master"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Auto Payment Reconcillation"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" runat="server" Visible="false" />
    <asp:panel runat="server" id="pnlFilter" visible="false">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:updatepanel enableviewstate="true" rendermode="Inline" id="filterPnal_upnlFilter"
                        updatemode="Conditional" runat="server">
                        <contenttemplate>
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
                        </contenttemplate>
                    </asp:updatepanel>
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
    </asp:panel>
    <uc1:SearchBar ID="ucSearchBar" SearchTextToolTip="Search by Roll Number" runat="server"
        AutoCompleteFirstRowSelected="True" AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
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
            <td style="width: 30%;" valign="top">
                <asp:label id="Label8" runat="server" skinid="CaptionLabel" text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td style="width: 70%;" valign="top">
                <asp:label id="Label11" runat="server" skinid="CaptionLabel" text="Upload MS-Excel File (.xls/xlsx) &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 30%;" valign="top">
                <asp:dropdownlist id="ddlpaymentmode" runat="server" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td style="width: 70%;" valign="top">
                <asp:fileupload id="flUpload" runat="server" width="485px" />
                <asp:hiddenfield id="flpath" runat="server" />
            </td>
        </tr>
    </table>
    <%--  <div id="divValidateData" runat="server" visible="false" height="150px" width="600px"
        style="overflow: scroll;">
        <table class="sample3" id="tblValidateData" style="width: 100%; text-align: left"
            border="0" cellpadding="2" cellspacing="1">
            <tr class="head1">
                <td align="left" colspan="2" width="100%">
                    Validate Data
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="30%">
                    Total Records
                </td>
                <td width="70%">
                    <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Settled Records
                </td>
                <td>
                    <asp:Label ID="lblValidateRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Not Validate
                </td>
                <td>
                    <asp:Label ID="lblNotValidate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top">
                    Failed Records Details
                </td>
                <td>
                 <asp:Label ID="lblFailedRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td colspan="2">
                    <asp:Label ID="Label12" runat="server" Text="Only Validate records can be uploaded"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>--%>
    <div id="divValidateData" runat="server" visible="false" height="150px" width="600px"
        style="overflow: scroll;">
        <table class="sample3" id="tblValidateData" style="width: 100%; text-align: left"
            border="0" cellpadding="2" cellspacing="1">
            <tr class="head1">
                <td align="left" colspan="2" width="100%">
                    Validate Data
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="30%">
                    Total Records
                </td>
                <td width="70%">
                    <asp:label id="lblTotalRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Settled Records
                </td>
                <td>
                    <asp:label id="lblSettledRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Manually Settled Records
                </td>
                <td>
                    <asp:label id="lblManSet" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top">
                    Refund Records
                </td>
                <td>
                    <asp:label id="lblRefundRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td valign="top">
                    Failed Records
                </td>
                <td>
                    <asp:label id="lblFailRecordsCount" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top">
                    Failed Records Details
                </td>
                <td>
                    <asp:label id="lblFailedRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td colspan="2">
                    <asp:label id="Label12" runat="server" text="Failure may be due to Unavailable/Already Reconcilled/Invalid Records."
                        forecolor="Red"></asp:label>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px">

        <%--<asp:button id="btnValidate" runat="server" text="Validate Data" onclick="btnValidate_Click"
            onclientclick="return ValidateFormFields()" visible="False" />--%>

        <asp:button id="btnSave" runat="server" text="Upload" onclick="btnSave_Click" onclientclick="return ValidateFormFields()" />
        <asp:button id="btndownload" runat="server" text="Download Refund File" 
            visible="false" onclick="btndownload_Click" />
        <asp:button id="btnCancel" runat="server" text="Reset" onclick="btnCancel_Click" />
    </div>
    <asp:hiddenfield runat="server" id="hffilename" />
    <asp:hiddenfield runat="server" id="hffilepath" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
