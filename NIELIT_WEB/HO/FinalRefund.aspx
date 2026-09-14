<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinalRefund.aspx.cs" Inherits="HO_FinalRefund"
    MasterPageFile="~/MasterPages/main.master" Debug="false" %>


<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Upload Refund File"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" runat="server" Visible="true" OnBtnMode_Click="ToggleViewMode_Changed" />
    <asp:panel runat="server" id="pnlFilter" visible="true">
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
                                            Text="" onclick="btnReset_Click" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClientClick="return ValidateFilter()" 
                                            onclick="btnFilter_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldate" Width="70%" runat="server" Text="Date Type"></asp:Label>
                                        <asp:DropDownList ID="ddldatetype" Width="70%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                            <asp:ListItem Value="1">NIELIT Refund Date</asp:ListItem>
                                            <asp:ListItem Value="2">Bill Desk Refund Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Date From"></asp:Label>
                                         <asp:TextBox ID="txtflFromDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                         <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                             PopupButtonID="imgdatefrom" TargetControlID="txtflFromDate"></asp:CalendarExtender>
                                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Date To"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                         <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                                             PopupButtonID="img1" TargetControlID="txtToDate"></asp:CalendarExtender>
                                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
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
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateFilter() {
            if (!isSelected("<%=ddldatetype.ClientID %>", "Date Type"))
                return false;
            if (!isBlankDate("<%=txtflFromDate.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtflFromDate.ClientID %>", "Invalid From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtToDate.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtToDate.ClientID %>", "Invalid To Date", "dd-MMM-yyyy"))
                return false;
            var frdate = document.getElementById("<%=txtflFromDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtToDate.ClientID %>").value;
            if (!CompareDates(frdate, todate, "From date should be less then To date", true)) {
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
       
    </script>
    <asp:multiview id="mltvTab" runat="server" activeviewindex="0">
        <asp:view id="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:updatepanel enableviewstate="true" id="uPnlGrid" updatemode="Conditional" runat="server">
                    <contenttemplate>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server">
                        </asp:Label>
                        <asp:Panel ID="pnlMain" Visible="false" runat="server">
                        <table width="99%" cellspacing ="0" cellpadding ="0" id="tblheader">
                            <tr>
                                <td width="33%">
                                <asp:Label ID="lblOne" Text="" runat="server" ></asp:Label>
                                </td>
                                 <td width="33%" align ="right" >
                                     <asp:Label ID="lbltxttotal" runat="server" Font-Bold="true" Text="Total Refund Amount:" Visible="false"/>
                                </td>
                                 <td width="10%" align="left" >
                                     <asp:Label ID="lblTotal" runat="server" Visible="false" />
                                </td>
                                <td width="10%" align="right" >
                                    <%--<asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
                                    ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  Visible="false"/>--%>
                                    &nbsp;
                                   <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
                                    ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" Visible ="false" OnClick ="Export"  />
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                        <div id="divGridMain" runat="server" >
                         <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" 
                            AutoGenerateColumns="False"  onrowdatabound="gvMain_RowDataBound" 
                            onsorting="gvMain_Sorting" AllowSorting ="true">
                             <AlternatingRowStyle Font-Size="11px" Height="15px" />
                            <Columns>
                            <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                              <HeaderStyle Width="2%" />
                              <ItemStyle HorizontalAlign="Right" />
                               </asp:BoundField>
                               <asp:TemplateField HeaderText="TransactionID" SortExpression ="transid">
                               <ItemTemplate>
                               <asp:Label ID="lblbname" runat="server" Text='<%# Eval("transid") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="12%" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="TransactionDate" SortExpression ="createdon">
                               <ItemTemplate>
                               <asp:Label ID="lbltrans" runat="server" Text='<%# Eval("createdon") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="11%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ReferenceNo." SortExpression ="bdreferenceNo">
                               <ItemTemplate>
                               <asp:Label ID="lblrefno" runat="server" Text='<%# Eval("bdreferenceNo") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="15%" />
                               </asp:TemplateField>
                              <%-- <asp:TemplateField HeaderText="ProductID" SortExpression ="pcode">
                               <ItemTemplate>
                               <asp:Label ID="lblName" runat="server" Text='<%# Eval("pcode") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="15%" />
                               </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="RefundDate" SortExpression ="refundDate">
                               <ItemTemplate>
                               <asp:Label ID="lblnrefund" runat="server" Text='<%# Eval("refundDate") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="11%" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="RefundID" SortExpression ="refundid">
                               <ItemTemplate>
                               <asp:Label ID="lblpmode" runat="server" Text='<%# Eval("refundid") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="8%" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="BillDeskRefundDate" SortExpression ="billdeskdate">
                               <ItemTemplate>
                               <asp:Label ID="lblbrefund" runat="server" Text='<%# Eval("billdeskdate") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="11%" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" SortExpression ="Amount">
                               <ItemTemplate>
                               <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="5%" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="DemandDetails" SortExpression ="dddetails">
                               <ItemTemplate>
                               <asp:Label ID="lbldddetails" runat="server" Text='<%# Eval("dddetails") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="14%" />
                               </asp:TemplateField>
                            </Columns>
                            <HeaderStyle Font-Size="12px" Height="15px" />
                            <PagerSettings Visible="False" />
                            <RowStyle Font-Size="11px" Font-Underline="False" Height="15px" />
                        </asp:GridView>
                        </div>
                    </contenttemplate>
                    <triggers>
                    <asp:PostBackTrigger ControlID="ibExport"  />
                </triggers>
                </asp:updatepanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:updatepanel rendermode="Inline" id="uPnlNavigation" updatemode="Conditional"
                    runat="server">
                    <contenttemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" class="Pagination" OnPageIndexChanged="PageIndexChanged" />
                    </contenttemplate>
                </asp:updatepanel>
            </div>
        </asp:view>
        <asp:view id="New" runat="server">
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td style="width: 20%;" valign="top">
                <asp:label id="Label8" runat="server" skinid="CaptionLabel" text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td style="width: 80%;" valign="top">
                <asp:label id="Label11" runat="server" skinid="CaptionLabel" text="Upload MS-Excel File (.xls/xlsx) &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 20%;" valign="top">
                <asp:dropdownlist id="ddlpaymentmode" runat="server" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td style="width: 80%;" valign="top">
                <asp:fileupload id="flUpload" runat="server" width="485px" />
                <asp:hiddenfield id="flpath" runat="server" />
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
                    <asp:label id="lblTotalRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Validate Records
                </td>
                <td>
                    <asp:label id="lblValidateRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%">
                    Not Validate
                </td>
                <td>
                    <asp:label id="lblNotValidate" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%" valign="top">
                    Failed Records Details
                </td>
                <td>
                    <asp:label id="lblFailedRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%" colspan="2">
                    <asp:label id="Label12" runat="server" text="Only Validate records can be uploaded"
                        forecolor="Red"></asp:label>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: right; margin-top: 10px">
        <asp:button id="btnSave" runat="server" text="Upload" onclick="btnSave_Click"
            onclientclick="return ValidateFormFields()" />
        <asp:button id="btnCancel" runat="server" text="Reset" onclick="btnCancel_Click" />
    </div>
        </asp:view>
    </asp:multiview>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
