<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master"  AutoEventWireup="true" CodeFile="PaymentReconciliationFilter.aspx.cs" Inherits="HO_PaymentReconciliation" Debug="false"%>

<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>

<%@ Register src="../UserControl/PagingBar.ascx" tagname="PagingBar" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
Payment Reconciliation
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<script type="text/javascript" language="javascript">
    function isValidForm() {
        if (!isSelected("<%=ddlPaymentMode.ClientID %>", "Payment Mode Type"))
            return false;
        var curdate = new Date().format("dd-MMM-yyyy");
        if (!isBlankDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
            return false;
        if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid From Date", "dd-MMM-yyyy"))
            return false;
        if (!isBlankDate("<%=txtToDate.ClientID %>", "To Date", "dd-MMM-yyyy"))
            return false;
        if (!isDate("<%=txtToDate.ClientID %>", "Invalid To Date", "dd-MMM-yyyy"))
            return false;
        var PayFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
        if (!CompareDates(PayFromDate, curdate, "Payment from date should be less than todays Date", true))
            return false;
        var PayToDate = document.getElementById('<%=txtToDate.ClientID %>').value;
        if (!CompareDates(PayToDate, curdate, "Payment To Date should be less than todays Date", true))
            return false;
        if (!CompareDates(PayFromDate, PayToDate, "Payment from date should be less than Payment To Date", true))
            return false;
        if (!isSelected("<%=ddlApplicationType.ClientID %>", "Application type"))
            return false;
        if (!isSelected("<%=ddlTransactionStatus.ClientID %>", "Transaction Status"))
            return false;    
           
        return true;      
    
    }

    function CheckSelection() {
        if (!isValidForm())
            return false;
        if (document.getElementById('<%=BtnShowDetail.ClientID %>').value == "Verify") {
            if (!TestCheckBox())
                return false;
        }
    }

    function validateFormFields() {
        if (isValidForm()) {
            var PayFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            var PayToDate = document.getElementById('<%=txtToDate.ClientID %>').value;
            var AppTypeId = 0;
            if (document.getElementById('<%=ddlApplicationType.ClientID %>').value != "0") {
                AppTypeId = document.getElementById('<%=ddlApplicationType.ClientID %>').value;
            }
            var TransactionStatus = 0;
            if (document.getElementById('<%=ddlTransactionStatus.ClientID %>').value != "0") {

                TransactionStatus = document.getElementById('<%=ddlTransactionStatus.ClientID %>').value;
            }
            var PayModeId = document.getElementById('<%=ddlPaymentMode.ClientID %>').value;

            window.open("../HO/Rpt/PaymentReconciliationReport.aspx?PayModeId=" + PayModeId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&TransactionStatus=" + TransactionStatus);
            return false;
        }
        return false;
    }

    function TestCheckBox() {
        var TargetBaseControl = document.getElementById('<%= gvMain.ClientID %>');
        if (TargetBaseControl != null) {
            //get target child control.
            var TargetChildControl = "chk";
            //get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked)
                    return true;
        }

        alert('Select at least one checkbox!');
        return false;



    }

    var dtgp = "<%= gvMain.ClientID %>"
    function CheckAll(Sender, CheckBoxName) {
        CheckUncheckAll(dtgp, Sender, CheckBoxName)
    }
    function PerformAction(obj, tableid) {
        document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
        ShowHideMenu(obj, tableid);
    }
    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
<table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" 
                     Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" 
                     Text="Transaction  From Date&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" 
                     Text="Transaction To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
        </tr>
        <tr class="even">
             <td style="width: 33%;" valign="top">
                <asp:DropDownList ID="ddlPaymentMode" runat="server" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
             <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtDateFrom" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img runat="server" id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtToDate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img runat="server" id="imgTo" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                     <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                    Format="dd-MMM-yyyy" PopupButtonID="imgTo">
                </asp:CalendarExtender>
                    </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" 
                    Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" 
                    Text="Transaction Status&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr class="even" >
            <td>
                <asp:DropDownList ID="ddlApplicationType" runat="server" SkinID="ddl250"
                    Width="250px">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                  
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlTransactionStatus" runat="server" SkinID="ddl250"
                    Width="250px">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                  
                    <asp:ListItem Value="SUV">Success And Updated(Verified)</asp:ListItem>
                    <asp:ListItem Value="SUNV">Success And Updated(Not Verified)</asp:ListItem>
                  
                    <asp:ListItem Value="SNU">Success and Not Updated</asp:ListItem>
                    <asp:ListItem Value="F">Failed</asp:ListItem>
                  
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="BtnShowDetail" runat="server" onclick="BtnShowDetail_Click"  OnClientClick="return CheckSelection();"
                Text="Show Detail" />
              
        <asp:Button ID="btnView" runat="server" Text="Print Report" 
                OnClientClick="return validateFormFields()" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" 
            onclick="btnReset_Click" /></div>
            <div id="Div_GridPnl" runat ="server" visible ="false">
             <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False"  OnRowDataBound="gvMain_RowDataBound" 
                        DataKeyNames="demandNoteID"
                        >
                            <Columns>
                                <asp:BoundField HeaderText="#" />
                                <asp:HyperLinkField 
                                    DataTextField="TransactionNumber" HeaderText="Transaction No." SortExpression="TransactionNumber" 
                                    DataNavigateUrlFields="demandNoteID,TranID,PayModeId,AppTypeId" DataNavigateUrlFormatString="~/HO/TransactionDetails.aspx?demandNoteID={0}&TranID={1}&PayModeId={2}&AppTypeId={3}"
                                    Target="_blank" />
                                <asp:BoundField HeaderText="Transaction Date" DataField="TransactionDate" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="DemandNoteNo./Date">                                  
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("DemandNoteDetail") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Application No" DataField="ApplicationNumber" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="TransactionAmt" DataField="TransactionAmount" DataFormatString="{0:#0.00}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                  <asp:TemplateField  HeaderText="">
                                        <HeaderStyle  />
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk"  SkinID="CheckAllInGridView" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chk"  SkinID="CheckAllInGridView" />
                                        </HeaderTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                          </ContentTemplate>
                </asp:UpdatePanel>
                        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            
            </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

