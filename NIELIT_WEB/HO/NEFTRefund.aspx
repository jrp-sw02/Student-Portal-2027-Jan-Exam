<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NEFTRefund.aspx.cs" Inherits="HO_NEFTRefund"
    MasterPageFile="~/MasterPages/main.master" Debug="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="NEFT/RTGS Refund"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="true" />
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
                                            Text="" OnClientClick="return validatefilter()" 
                                            onclick="btnFilter_Click" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lblStatus" runat="server" Text="Refund Mode"></asp:Label>
                                        <asp:DropDownList ID="ddlRefundMode" Width="70%" runat="server" 
                                            AutoPostBack="True" >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                            <asp:ListItem Value="3">Cheque</asp:ListItem>
                                            <asp:ListItem Value="6">NEFT</asp:ListItem>
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
        function validatefilter() {
            if (!isSelected("<%=ddlRefundMode.ClientID %>", "Refund Mode"))
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

        function ValidateFormFields1() {

            if (!isBlank("<%=txtutrnumber.ClientID %>", "UTR Number"))
                return false;
            if (!isBlank("<%=txtdate.ClientID %>", "Transaction Date"))
                return false;
            if (!isDate("<%=txtdate.ClientID %>", "Invalid Transaction Date"))
                return false;
            if (!isBlank("<%=txtamount.ClientID %>", "Credit Amount"))
                return false;
            if (!isNumber("<%=txtamount.ClientID %>", "Invalid Credit Amount"))
                return false;
        }

        function ValidateFormFields2() {

            if (!isBlank("<%=txtcname.ClientID %>", "Name"))
                return false;
            if (!isBlank("<%=txtcrefundamount.ClientID %>", "Refund Amount"))
                return false;
            if (!isNumber("<%=txtcrefundamount.ClientID %>", "Invalid Refund Amount"))
                return false;
            if (!isBlank("<%=txtcrefunddate.ClientID %>", "Refund Date"))
                return false;
            if (!isDate("<%=txtcrefunddate.ClientID %>", "Invalid Refund Date"))
                return false;
            if (!isBlank("<%=txtcheqnumber.ClientID %>", "Cheque Number"))
                return false;
            if (!isNumber("<%=txtcheqnumber.ClientID %>", "Invalid Cheque Number"))
                return false;
            if (!isBlank("<%=txtcrefundreason.ClientID %>", "Refund Reason"))
                return false;
        }

        function ValidateFormFields3() {

            if (!isBlank("<%=txtnrefamount.ClientID %>", "Refund Amount"))
                return false;
            if (!isNumber("<%=txtnrefamount.ClientID %>", " Invalid Refund Amount"))
                return false;
            if (!isBlank("<%=txtneftrefunddate.ClientID %>", "Refund Date"))
                return false;
            if (!isDate("<%=txtneftrefunddate.ClientID %>", "Invalid Refund Date"))
                return false;
            if (!isBlank("<%=txtnaccnumber.ClientID %>", "A/C Number"))
                return false;
            if (!isBlank("<%=txtnaccholdername.ClientID %>", "A/C Holder Name"))
                return false;
            if (!isBlank("<%=txtnacctype.ClientID %>", "A/C Type"))
                return false;
            if (!isBlank("<%=txtnifsccode.ClientID %>", "IFSC Code"))
                return false;
            if (!isBlank("<%=txtnrefundreason.ClientID %>", "Refund Reason"))
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
                                     <asp:Label ID="lbltxttotal" runat="server" Font-Bold="true" Text="Total Amount :" Visible="false"/>
                                </td>
                                 <td width="10%" align="left" >
                                     <asp:Label ID="lblTotal" runat="server" Visible="false" />
                                </td>
                                <td width="10%" align="right" >
                                   <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
                                    ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" Visible ="false" OnClick ="Export"  />
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                        <div id="divGridMain" runat="server" >
                         <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" 
                            AutoGenerateColumns="False"  onrowdatabound="gvMain_RowDataBound" 
                            onsorting="gvMain_Sorting" AllowSorting ="True" Font-Bold="False" 
                                Font-Size="10px" CellPadding="0" >
                            <Columns>
                             <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                              <HeaderStyle Width="1%" />
                              <ItemStyle HorizontalAlign="Right" />
                               </asp:BoundField>
                              <asp:TemplateField HeaderText="ChequeNo." SortExpression ="Chequeno">
                               <ItemTemplate>
                               <asp:Label ID="lblChequeno" runat="server" Text='<%# Eval("Chequeno") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="12%" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="A/CNo." SortExpression ="accno">
                               <ItemTemplate>
                               <asp:Label ID="lblAccNo" runat="server" Text='<%# Eval("accno") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="8%" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="IFSCCode" SortExpression ="IFSC">
                               <ItemTemplate>
                               <asp:Label ID="lblSenderIFSC" runat="server" Text='<%# Eval("IFSC") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="8%" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Name" SortExpression ="cname">
                               <ItemTemplate>
                               <asp:Label ID="lblName" runat="server" Text='<%# Eval("cname") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="14%" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/CHolderName" SortExpression ="accholderName">
                               <ItemTemplate>
                               <asp:Label ID="lblaccName" runat="server" Text='<%# Eval("accholderName") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="18%" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/CType" SortExpression ="acctype">
                               <ItemTemplate>
                               <asp:Label ID="lblacctype" runat="server" Text='<%# Eval("acctype") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="9%" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Date" SortExpression ="refundDate">
                               <ItemTemplate>
                               <asp:Label ID="lblTran" runat="server" Text='<%# Eval("refundDate") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="10%" />
                                   <ItemStyle HorizontalAlign="Center" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" SortExpression ="Amount">
                               <ItemTemplate>
                               <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                               </ItemTemplate>
                               <HeaderStyle Width="8%" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason" SortExpression ="reason">
                               <ItemTemplate>
                               <asp:Label ID="lblreason" runat="server" Text='<%# Eval("reason") %>'></asp:Label>
                               </ItemTemplate>
                                  <HeaderStyle Width="40%" />
                                    <ItemStyle HorizontalAlign="Left" />
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="UTRNo" SortExpression ="utrno">
                               <ItemTemplate>
                               <asp:Label ID="lblutrno" runat="server" Text='<%# Eval("utrno") %>'></asp:Label>
                               </ItemTemplate>
                                  <HeaderStyle Width="10%" />
                               </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
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
            <asp:radiobuttonlist runat="server" id="rblServiceType" repeatdirection="Horizontal"
                autopostback="True" 
                onselectedindexchanged="rblServiceType_SelectedIndexChanged" visible="false">
                <asp:listitem value="1" selected="true">Single Entry</asp:listitem>
                <asp:listitem value="2">Multiple Entry</asp:listitem>
            </asp:radiobuttonlist>
        <div id="divmultiple" runat="server" visible="false">
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
                            Validate Records
                        </td>
                        <td>
                            <asp:label id="lblValidateRecords" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td valign="top">
                            Not Validate Records Details
                        </td>
                        <td>
                            <asp:label id="lblFailedRecords" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td colspan="2">
                            <asp:label id="Label12" runat="server" text="The unsaved records may be due to data discrepancies or duplicacy."
                                forecolor="Red"></asp:label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnSave" runat="server" text="Upload Data" onclick="btnSave_Click"
                    onclientclick="return ValidateFormFields()" />
                <asp:button id="btnCancel" runat="server" text="Reset" onclick="btnCancel_Click" />
            </div>
        </div>
        <div id="divsingle" runat="server" visible="false">
            <table class="sample2" id="Table1" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td>
                        <asp:label id="Lbutrnumber" runat="server" skinid="CaptionLabel" text="UTR Number &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td>
                        <asp:label  runat="server" skinid="CaptionLabel" text="Transaction Date &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td>
                        <asp:label runat="server" skinid="CaptionLabel" text="Credit Amount &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:textbox runat="server" id="txtutrnumber" maxlength="20" skinid="txt248"></asp:textbox>
                    </td>
                    <td>
                        <asp:textbox runat="server" id="txtdate" maxlength="11" SkinID="txtDate"></asp:textbox>
                        <asp:calendarextender id="Calendarextender3" runat="server" format="dd-MMM-yyyy"
                            popupbuttonid="img3" targetcontrolid="txtdate">
                        </asp:calendarextender>
                        <img id="img3" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                    <td>
                        <asp:textbox runat="server" id="txtamount" maxlength="8" onkeypress="checkNumber(this,8,0,event)"
                            skinid="txt248"></asp:textbox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnSubmit" runat="server" text="Next" 
                    onclientclick="return ValidateFormFields1()" onclick="btnSubmit_Click" />
                <asp:button id="btnSCancel" runat="server" text="Cancel" 
                    onclick="btnSCancel_Click" />
            </div>
        </div> 
        <div id="divmoepayment" runat="server" visible="false" class="box">
        Mode of Payment
            <asp:radiobuttonlist runat="server" id="rblmodeofpayment" repeatdirection="Horizontal"
                autopostback="True" onselectedindexchanged="rblmodeofpayment_SelectedIndexChanged">
                <asp:listitem value="1">Cheque</asp:listitem>
                <asp:listitem value="2">NEFT</asp:listitem>
            </asp:radiobuttonlist>
        </div>
        <div id="divcheque" runat="server" visible="false">
            <table class="sample2" id="Table2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Lbname" runat="server" skinid="CaptionLabel" text="Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="lbamount" runat="server" skinid="CaptionLabel" text="Refund Amount &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label runat="server" skinid="CaptionLabel" text="Refund Date &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtcname" maxlength="50" skinid="txt248"></asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtcrefundamount" maxlength="8" skinid="txt248" onkeypress="checkNumber(this,8,0,event)"></asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtcrefunddate" maxlength="11" SkinID="txtDate"></asp:textbox>
                        <asp:calendarextender id="Calendarextender4" runat="server" format="dd-MMM-yyyy"
                            popupbuttonid="img5" targetcontrolid="txtcrefunddate">
                        </asp:calendarextender>
                        <img id="img5" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Lbchequeno" runat="server" skinid="CaptionLabel" text="Cheque Number &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:label id="lbreason" runat="server" skinid="CaptionLabel" text="Refund Reason &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtcheqnumber" maxlength="8" skinid="txt248" onkeypress="checkNumber(this,8,0,event)"></asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:textbox runat="server" id="txtcrefundreason" maxlength="75"
                            skinid="txt502"></asp:textbox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnchequeSubmit" runat="server" text="Submit" 
                 onclientclick="return ValidateFormFields2()" onclick="btnchequeSubmit_Click" />
                <asp:button id="btnchequeCancel" runat="server" text="Back" 
                    onclick="btnchequeCancel_Click" />
            </div>
        </div>
        <div id="divneft" runat="server" visible="false">
            <table class="sample2" id="Table3" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Lbrefamount" runat="server" skinid="CaptionLabel" text="Refund Amount &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label runat="server" skinid="CaptionLabel" text="Refund Date &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="lbaccnumber" runat="server" skinid="CaptionLabel" text="A/C Number &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtnrefamount" maxlength="8" skinid="txt248" onkeypress="checkNumber(this,8,0,event)"></asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtneftrefunddate" maxlength="11" 
                            SkinID="txtDate"></asp:textbox>
                        <asp:calendarextender id="Calendarextender5" runat="server" format="dd-MMM-yyyy"
                            popupbuttonid="img8" targetcontrolid="txtneftrefunddate">
                        </asp:calendarextender>
                        <img id="img8" alt="Calender" src="../images/calendaricon.jpg" />
                       
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtnaccnumber" maxlength="16" skinid="txt248"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Lbachname" runat="server" skinid="CaptionLabel" text="Account Holder Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="lbacctype" runat="server" skinid="CaptionLabel" text="Account Type (Saving/Current) &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="lbifsccode" runat="server" skinid="CaptionLabel" text="IFSC Code &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtnaccholdername" maxlength="50" skinid="txt248"></asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtnacctype" maxlength="50" skinid="txt248"></asp:textbox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:textbox runat="server" id="txtnifsccode" maxlength="12" skinid="txt248"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:label id="Lnrefundreason" runat="server" skinid="CaptionLabel" text="Refund Reason &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td></td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:textbox runat="server" id="txtnrefundreason" maxlength="75"
                            skinid="txt502"></asp:textbox>
                    </td>
                    <td></td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnneftSubmit" runat="server" text="Submit" 
                    onclientclick="return ValidateFormFields3()" onclick="btnneftSubmit_Click"
                    />
                <asp:button id="btnneftCancel" runat="server" text="Back" 
                    onclick="btnneftCancel_Click" />
            </div>
        </div>
        </asp:view>
    </asp:multiview>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
