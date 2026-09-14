<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PuraskarFeeReimbursementInstalment.aspx.cs"
    Inherits="Admin_PuraskarFeeReimbursementInstalment" MasterPageFile="~/MasterPages/main.master"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Puraskar Fee Reimbursement Installments"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcourseName" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Project Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (!isSelected("<%=ddlcourses.ClientID  %>", "Course Name"))
                return false;
            if (!isBlank("<%=txtinstalmentsNumber.ClientID  %>", "Installments Number"))
                return false;
            if (!isBlank("<%=txtpaperCount.ClientID  %>", "Paper Count"))
                return false;
            if (!isBlank("<%=txtinstalmentAmount.ClientID  %>", "Installments Amount"))
                return false;
            if (!isBlankDate("<%=txteffectivefromDate.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;

            if (!isNumber("txtinstalmentsNumber"))
                return false;
            if (!isNumber("txtpaperCount"))
                return false;
            if (!isNumber("txtinstalmentAmount"))
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

    <script type="text/javascript" language="javascript">
        function numeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && ((charCode >= 48 && charCode <= 57) || charCode == 46))
                return true;
            else {
                alert('Please Enter Numeric values.');
                return false;
            }
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">

                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblerrorG" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="courseName"
                                    HeaderText="Course Name" SortExpression="courseName" Target="_self"></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="InstalmentsNumber"
                                    HeaderText="Installments Number" SortExpression="InstalmentsNumber" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="PaperCount"
                                    HeaderText="Paper Count" SortExpression="PaperCount" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="InstalmentAmt"
                                    HeaderText="Installments Amount" SortExpression="InstalmentAmt" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="effectiveFrom"
                                    HeaderText="Effective From Date" SortExpression="effectiveFrom" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="effectiveTo"
                                    HeaderText="Effective End Date" SortExpression="effectiveTo" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode" runat="server" />
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
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">

                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Installments Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">

                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Paper Count &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourses" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcourses_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtinstalmentsNumber" runat="server" onkeypress="return numeric(event)" onpaste="return false;" SkinID="txt248" MaxLength="1"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtpaperCount" SkinID="txt248" runat="server" onkeypress="return numeric(event)" onpaste="return false;" Width="200px" MaxLength="2"></asp:TextBox>

                    </td>
                </tr>

                <tr>
                     <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Installments Amount &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                     </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Effective From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Effective End Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                   
                </tr>
                <tr class="even">
                     <td style="width: 33%;" valign="top">
                         <asp:TextBox ID="txtinstalmentAmount" SkinID="txt248" runat="server" onkeypress="return numeric(event)" onpaste="return false;" Width="200px" MaxLength="5"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txteffectivefromDate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px"></asp:TextBox>
                        <img id="img2" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txteffectivefromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txteffectiveEndDate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px"></asp:TextBox>
                        <img id="img3" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txteffectiveEndDate">
                        </asp:CalendarExtender>

                    </td>                   
                </tr>

                <tr id="lblerrordisplay" runat="server" visible="false">
                    <td colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False" Width="99%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>
