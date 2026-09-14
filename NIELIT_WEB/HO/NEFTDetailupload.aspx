<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NEFTDetailupload.aspx.cs"
    Inherits="HO_NEFTDetailupload" MasterPageFile="~/MasterPages/main.master" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NEFT/RTGS Bank Detail Upload"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="true" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
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
                                            Text="" OnClick="btnReset_Click" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClientClick="return validatefilter()" OnClick="btnFilter_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStatus" Width="70%" runat="server" Text="Status"></asp:Label>
                                        <asp:DropDownList ID="ddlStatus" Width="70%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                            <asp:ListItem Value="1">Verified</asp:ListItem>
                                            <asp:ListItem Value="2">Not Verified</asp:ListItem>
                                            <asp:ListItem Value="3">Refunded</asp:ListItem>
                                            <asp:ListItem Value="4">All Records</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Transaction Date From"></asp:Label>
                                        <asp:TextBox ID="txtflFromDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="imgdatefrom" TargetControlID="txtflFromDate">
                                        </asp:CalendarExtender>
                                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Transaction Date To"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="img1" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
                                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
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
    <asp:UpdatePanel EnableViewState="true" ID="upbreadsearch" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc1:SearchBar ID="ucSearchBar" SearchTextToolTip="Search by Utr Number" runat="server"
                OnLnkBtnGO="SearchBar_ApplySearch" AutoCompleteFirstRowSelected="True" AutoCompleteMinimumPrefixLength="1"
                AutoCompleteServiceMethod="GetSearchText" AutoCompleteCompletionSetCount="10"
                Visible="true" OnLnkBtnResetSearch="SearchBar_Reset" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {
            if (!isSelected("<%=ddlStatus.ClientID %>", "Status"))
                return false;
            if (!isBlankDate("<%=txtflFromDate.ClientID %>", "Transaction From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtflFromDate.ClientID %>", "Invalid Transaction From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtToDate.ClientID %>", "Transaction To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtToDate.ClientID %>", "Invalid Transaction To Date", "dd-MMM-yyyy"))
                return false;
            var frdate = document.getElementById("<%=txtflFromDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtToDate.ClientID %>").value;
            if (!CompareDates(frdate, todate, "Transaction From date should be less then Transaction To date", true)) {
                return false;
            }
        }
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
        function printwindow() {

            var prtGrid = document.getElementById('<%=gvMain.ClientID %>');
            var prtwin = window.open('', 'NEFT/RTGS Details', '');
            prtwin.document.write(prtGrid.outerHTML);
            prtwin.document.close();
            prtwin.focus();
            prtwin.print();
            prtwin.close();
        }
       
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server">
                        </asp:Label>
                        <asp:Panel ID="pnlMain" Visible="false" runat="server">
                            <table width="99%" cellspacing="0" cellpadding="0" id="tblheader">
                                <tr>
                                    <td width="50%" style="font-weight:bold; font-size:12px;">
                                        <asp:Label ID="lblOne" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td width="20%" align="right" style="font-size:12px;">
                                        <asp:Label ID="lbltxttotal" runat="server" Font-Bold="true" Text="Total Amount :"
                                            Visible="false" />
                                    </td>
                                    <td width="20%" align="left" style="font-size:12px;" >
                                        <asp:Label ID="lblTotal" runat="server" Visible="false" />
                                    </td>
                                    <td width="10%" align="right">
                                        <%--<asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
                                    ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  Visible="false"/>--%>
                                        &nbsp;
                                        <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
                                            ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" Visible="false"
                                            OnClick="Export" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div id="divGridMain" runat="server">
                            <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                                OnRowDataBound="gvMain_RowDataBound" OnSorting="gvMain_Sorting" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                                        <HeaderStyle Width="1%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="UTRNo." SortExpression="UtrNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUtrNo" runat="server" Text='<%# Eval("UtrNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SenderIFSC" SortExpression="SenderIFSC">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSenderIFSC" runat="server" Text='<%# Eval("SenderIFSC") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SenderName" SortExpression="SenderName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("SenderName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tran.Date" SortExpression="TranDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTran" runat="server" Text='<%# Eval("TranDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UploadedDate" SortExpression="uploadeddate">
                                        <ItemTemplate>
                                            <asp:Label ID="lbluploadeddate" runat="server" Text='<%# Eval("uploadeddate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="13%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DemandNoteStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Visible="False" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ibExport" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" class="Pagination" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 30%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 70%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.xls/xlsx) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 30%;" valign="top">
                        <asp:DropDownList ID="ddlpaymentmode" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70%;" valign="top">
                        <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                        <asp:HiddenField ID="flpath" runat="server" />
                    </td>
                </tr>
            </table>
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
                            <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            Validate Records
                        </td>
                        <td>
                            <asp:Label ID="lblValidateRecords" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td valign="top">
                            Not Validate Records Details
                        </td>
                        <td>
                            <asp:Label ID="lblFailedRecords" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td colspan="2">
                            <asp:Label ID="Label12" runat="server" Text="The unsaved records may be due to data discrepancies or duplicacy."
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" Text="Upload Data" OnClick="btnSave_Click"
                    OnClientClick="return ValidateFormFields()" />
                <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
