<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scholarship.aspx.cs" Inherits="HO_Scholarship"
    MasterPageFile="~/MasterPages/main.master" Debug="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Aadhar & Bank Acc. Details"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode"  runat="server"
        Visible="false" />
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
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            OnClientClick="return validfilter();" Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Width="100%" runat="server" Text="Date From"></asp:Label>
                                        <asp:TextBox ID="txtflFromDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="imgdatefrom" TargetControlID="txtflFromDate">
                                        </asp:CalendarExtender>
                                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Date To"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="img1" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
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
  <%--  <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Candidate Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />--%>
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <asp:updatepanel enableviewstate="true" id="upBread" updatemode="Conditional" runat="server">
        <contenttemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </contenttemplate>
    </asp:updatepanel>
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
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
        function ValidateForm() {

            //         if (!TestTextBox())
            //             return false;
            if (!TestCheckBox())
                return false;

            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }

        function validfilter() {
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

        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    <asp:multiview id="mltvTab" runat="server" activeviewindex="0">
        <asp:view id="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:updatepanel enableviewstate="true" id="uPnlGrid" updatemode="Conditional" runat="server">
                    <contenttemplate>
                    <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbaadhar" 
                                        runat="server" Text="View Scanned Copy of Aadhar Card" SkinID="lnkbtnAction"
                                        CommandName="AH" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbincome"
                                        runat="server" Text="View Scanned Copy of Income Certificate of Parents"
                                        SkinID="lnkbtnAction" CommandName="IN" OnClick="PerformPopupAction"></asp:LinkButton>
                                   <asp:LinkButton ID="lbcastcertificate" 
                                        runat="server" Text="View Scanned Copy of Caste Certificate"
                                        SkinID="lnkbtnAction" CommandName="CT" OnClick="PerformPopupAction"></asp:LinkButton>
                                <asp:LinkButton ID="lbphcertificate"
                                        runat="server" Text="View Scanned Copy of Physically Handicapped Certificate" 
                                        SkinID="lnkbtnAction" CommandName="PH" OnClick="PerformPopupAction"></asp:LinkButton>
                                <asp:LinkButton ID="lbankaccount"
                                        runat="server" Text="View Scanned Copy of Bank Account Pass Book" 
                                        SkinID="lnkbtnAction" CommandName="BK" OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:Label Width="99%" EnableTheming="False" CssClass="error" ID="lblError1" runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                 <asp:HyperLinkField DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="Date" HeaderText="Date"
                                    SortExpression="Date" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Center" Width="12%" />
                                </asp:HyperLinkField>
                                  <asp:HyperLinkField  DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="regno"
                                    HeaderText="Reg.No" SortExpression="regno" Target="_self">
                                    <ItemStyle Width="10%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField  DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="name"
                                    HeaderText="Name" SortExpression="name" Target="_self">
                                    <ItemStyle Width="25%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField  DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="dob"
                                    HeaderText="DOB" SortExpression="dob" Target="_self"  DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle Width="12%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField  DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="bname"
                                    HeaderText="BankName" SortExpression="bname" Target="_self">
                                    <ItemStyle Width="20%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="accno"
                                    HeaderText="Acc/No." SortExpression="accno" Target="_self">
                                    <ItemStyle Width="18%" />
                                </asp:HyperLinkField>
                                <%-- <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>--%>
                               <%-- <asp:TemplateField HeaderText="">
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode" runat="server" />
                    </contenttemplate>
                </asp:updatepanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:updatepanel rendermode="Inline" id="uPnlNavigation" updatemode="Conditional"
                    runat="server">
                    <contenttemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </contenttemplate>
                </asp:updatepanel>
            </div>
        </asp:view>
        <asp:view id="New" runat="server">
            <div align="right">
            </div>
            <table class="sample2" runat="server" id="Table1" cellpadding="2" cellspacing="0"
                width="100%">
                <tr class="heading">
                    <td valign="top" colspan="3">
                        <asp:label id="lblServiceType" runat="server" skinid="CaptionLabel" text="Candidate Details">
                        </asp:label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Label3" runat="server" skinid="CaptionLabel" text="Name" width="100%">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="Label4" runat="server" skinid="CaptionLabel" text="DOB"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label id="lblState" runat="server" skinid="CaptionLabel" text="E-Mail"
                            width="100%"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" id="tdName" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tddob" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdemail" runat="server">
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:label  runat="server" skinid="CaptionLabel" text="Mobile Number" width="100%">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label runat="server" skinid="CaptionLabel" text="Annual Income of Parents" width="100%"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:label runat="server" skinid="CaptionLabel" text="Aadhar Card Number" width="100%">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" id="tdmobile" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdincome" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdaadharcard" runat="server">
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top" id="tdhfame" runat="server">
                        <asp:label runat="server" skinid="CaptionLabel" text="Father Name" width="100%"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top" id="tdhmame" runat="server">
                        <asp:label runat="server" skinid="CaptionLabel" text="Mother Name" width="100%">
                        </asp:label>
                    </td>
                    <td style="width: 33%;" valign="top" id="tdhgame" runat="server">
                        <asp:label runat="server" skinid="CaptionLabel" text="Guardian Name" width="100%"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" id="tdfname" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdmname" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdgname" runat="server">
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top"  runat="server">
                        <asp:label runat="server" skinid="CaptionLabel" text="Gender" width="100%"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top"  runat="server">
                        <asp:label runat="server" skinid="CaptionLabel" text="Category" width="100%"></asp:label>
                    </td>
                    <td style="width: 33%;" valign="top"  runat="server">
                        <asp:label runat="server" skinid="CaptionLabel" text="Is_Physically_Handicapped" width="100%"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top" id="tdgender" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdcategory" runat="server">
                    </td>
                    <td style="width: 33%;" valign="top" id="tdphhandicap" runat="server">
                    </td>
                </tr>
                <tr class="heading">
                    <td valign="top" colspan="3">
                        <asp:label runat="server" skinid="CaptionLabel" text="Course Registration Details">
                        </asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3" id="tdregdetails" runat="server">
                    </td>
                </tr>
                <tr class="heading">
                    <td valign="top" colspan="3">
                        <asp:label runat="server" skinid="CaptionLabel" text="Bank Details"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3" id="tdbank" runat="server">
                    </td>
                </tr>
                <tr class="heading">
                    <td valign="top" colspan="3">
                        <asp:label runat="server" skinid="CaptionLabel" text="Document Details"></asp:label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3" id="tddoc" runat="server">
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnCancel" runat="server" text="Cancel" visible="False" onclick="btnCancel_Click" />
            </div>
        </asp:view>
    </asp:multiview>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
