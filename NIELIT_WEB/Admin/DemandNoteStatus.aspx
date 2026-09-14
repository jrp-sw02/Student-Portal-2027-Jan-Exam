<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DemandNoteStatus.aspx.cs" Inherits="DemandNoteStatus" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Demand Note Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />
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
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblApplicationType" Width="100%" runat="server" Text="Application Type"></asp:Label>
                                        <asp:DropDownList ID="ddlAppType" Width="100%" runat="server">
                                            <asp:ListItem>--All--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Payment Mode"></asp:Label>
                                        <asp:DropDownList ID="ddlPaymentMode" Width="100%" runat="server">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Payment Status"></asp:Label>
                                        <asp:DropDownList ID="ddlPaymentStatus" Width="100%" runat="server">
                                            <asp:ListItem>--All-</asp:ListItem>
                                        </asp:DropDownList>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by course name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
      <script type="text/javascript" language="javascript">

          function OpenWindow() {
               var demandNoteID="";
                if (document.getElementById('<%=hfDemandNoteID.ClientID %>').value != "")
                {
                    demandNoteID = document.getElementById('<%=hfDemandNoteID.ClientID %>').value;
                    window.open("../HO/Rpt/CandidateDetailReport.aspx?DemandNoteId=" + demandNoteID);
                     return false;
                }

             }
             function OpenWindow1() {
                 var demandNoteID = "";
                 if (document.getElementById('<%=hfDemandNoteID.ClientID %>').value != "") {
                     demandNoteID = document.getElementById('<%=hfDemandNoteID.ClientID %>').value;
                     window.open("../HO/Rpt/CertificateExamCandidateListReport.aspx?DemandNoteId=" + demandNoteID);
                     return false;
                 }

             }
             function Validate(msg) {
                 if (ConfirmAction(msg))
                     return true;
                 else
                     return false;
             }
      </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,PaystatusID,PayMode"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&PstsID={2}&PModeID={3}" DataTextField="DemandNo"
                                    HeaderText="Demand Number" SortExpression="DemandNo" Target="_self">
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,PaystatusID,PayMode"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&PstsID={2}&PModeID={3}" DataTextField="DemandDate"
                                    HeaderText="Date" SortExpression="DemandDate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,PaystatusID,PayMode"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&PstsID={2}&PModeID={3}" DataTextField="PaymentMode"
                                    HeaderText="Payment Mode" SortExpression="PaymentMode" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,PaystatusID,PayMode"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&PstsID={2}&PModeID={3}" DataTextField="PaymentStatus"
                                    HeaderText="Payment Status" SortExpression="PaymentStatus" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplicationTypeID,PaystatusID,PayMode"
                                    DataNavigateUrlFormatString="?Key={0}&TypeID={1}&PstsID={2}&PModeID={3}" DataTextField="Amount"
                                    HeaderText="Amount" SortExpression="Amount" Target="_self">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                     
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <div id="btnPay" style="text-align: right;">
                <asp:Button Visible="false" ID="btncancelDemandNote" runat="server" Text="Cancel Demand Note"
                    OnClick="btncancelDemandNote_Click" OnClientClick="return Validate('Are you sure you want to cancel this DemandNote!')" />
                <asp:Button Visible="false" ID="btnpaynow" runat="server" Text="Pay Now" OnClick="btnpaynow_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="Print Payment Detail List" Visible="false" OnClientClick="return OpenWindow();"     />
                <asp:Button ID="btnDispetchPrint" runat="server" 
                    Text="Print Dispatch Detail List" Visible="false" 
                    OnClientClick="return OpenWindow1();"     />
                   <asp:HiddenField ID="hfDemandNoteID" runat="server"  />
            </div>
            <table class="sample3" id="tblprint" style="width: 100%; text-align: left" border="0"
                cellpadding="3" cellspacing="1">
                <tr class="head1">
                    <td align="left" colspan="2">
                        Demand Note Details
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Demand Note Number
                    </td>
                    <td>
                        <asp:Label ID="lblDemandNoteNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Demand Note Date
                    </td>
                    <td>
                        <asp:Label ID="lblDemandNoteDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Amount
                    </td>
                    <td>
                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Demand Note Type
                    </td>
                    <td>
                        <asp:Label ID="lblDemandNoteType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Application Type
                    </td>
                    <td>
                        <asp:Label ID="lblApplType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Fee Type
                    </td>
                    <td>
                        <asp:Label ID="lblFeeType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Payment Mode
                    </td>
                    <td>
                        <asp:Label ID="lblPaymentMode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Payment Status
                    </td>
                    <td>
                        <asp:Label ID="lblPaymentStatus" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="head1" runat="server" id="lbltd" visible="false">
                    <td align="left" colspan="2">
                        Transaction Details
                    </td>
                </tr>
                <tr class="gdrow1" runat="server" id="lbldno" visible="false">
                    <td width="40%" id="ddno" runat="server">
                        Demand Draft Number
                    </td>
                    <td>
                        <asp:Label ID="lblddno" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1" runat="server" id="lblddate" visible="false">
                    <td width="40%" id="dddate" runat="server">
                        Demand Draft Date
                    </td>
                    <td>
                        <asp:Label ID="lbldddate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1" runat="server" id="lblbname" visible="false">
                    <td width="40%" id="ddbankname" runat="server">
                        Bank Name
                    </td>
                    <td>
                        <asp:Label ID="lblBank" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1" runat="server" id="lblVdate" visible="false">
                    <td width="40%" id="ddverification" runat="server">
                        Date of Demand Draft Verification
                    </td>
                    <td>
                        <asp:Label ID="LblVarificationDate" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" Visible="false" OnClientClick="return ValidateLogin();"
                    runat="server" Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" Visible="false" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
