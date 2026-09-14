<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="Revisionlist.aspx.cs" Inherits="Admin_Revisionlist" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Course Revisions"></asp:Label>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"  Visible="false"/>
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
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBreadCrumb" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            if (!isSelected("<%=ddlAppType.ClientID %>", "Applicant Type"))
                return false;
            if (!isSelected("<%=ddlQualLevel.ClientID %>", "Qualification Level"))
                return false;
            if (!isBlank("<%=txtExperienceYrs.ClientID %>", "Experience in Years"))
                return false;
            if (!isNumber("<%=txtExperienceYrs.ClientID %>"))
                return false;
            if (!isBlankDate("<%=txtEffectiveDtFrom.ClientID %>", "Effective Date From", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtEffectiveDtFrom.ClientID %>", "Invalid Effective Date From", "dd-MMM-yyyy"))
                return false;
            return true;

        }

        var dtgp = "<%= gvMain.ClientID %>"
        function validate(Sender, Mode) {
            var Number = /^[0-9,.]/;
            var arrId = Sender.id.split("_")
            var curdate = new Date().format("dd-MMM-yyyy");
            if (Mode == 'n') {
                if (document.getElementById("cphContents_gvMain_Txtrevno").value == "") {
                    alert("Enter Revision no");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_Txtcname").value == "") {
                    alert("Enter Course Name");
                    return false;
                }
                if (!isBlank("cphContents_gvMain_txteffdate", "Date of Introduction")) {
                    return false;
                }
                if (!isDate("cphContents_gvMain_txteffdate", "Invalid Effective Date From", "dd-MMM-yyyy")){
                   return false;
                }
            }
        }
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
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
                                    <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                        runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                        CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                        runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                        SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="false" Width="100%"
                            OnRowCommand="gvMain_RowCommand"  ShowFooter="true">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Revision No" SortExpression="revno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrevno" runat="server" Text='<%# Eval("revno")  %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="Txtrevno" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                    </FooterTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Course Name" SortExpression="cname">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcname" runat="server" Text='<%# Eval("cname")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="Txtcname" runat="server" Width="180px" MaxLength="40"></asp:TextBox>
                                    </FooterTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date of Introduction" SortExpression="EffectiveDateFrom">
                                    <ItemTemplate>
                                        <asp:Label ID="lbleffective" runat="server" Text='<%# Convert.ToDateTime(Eval("EffectiveDateFrom")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txteffdate" runat="server" MaxLength="11" Width="139px"></asp:TextBox>
                                        <img id="imgDob" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                        <asp:CalendarExtender ID="ceDOB" TargetControlID="txteffdate" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                        </asp:CalendarExtender>
                                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="true" OnClientClick="return validate(this,'n')"
                                            CommandName="Add" Text="&lt;img title='Click to add new revision no' src='../Images/addwebpart.gif' style='border-width:0px;vertical-align:middle;' /&gt;"></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
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
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="lblAppType" runat="server" SkinID="CaptionLabel" Text="Applicant Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblQuallevel" runat="server" SkinID="CaptionLabel" Text="Qualification Level&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlAppType" runat="server" SkinID="ddl250" Height="22px">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:DropDownList ID="ddlQualLevel" runat="server" Height="22px" Width="500px">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lbExperience" runat="server" SkinID="CaptionLabel" Text="Experience in Years&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td valign="top" width="250px">
                        <asp:Label ID="lblEffectiveFromDt" runat="server" SkinID="CaptionLabel" Text="Effective Date From&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" width="250px">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtExperienceYrs" runat="server" MaxLength="1" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEffectiveDtFrom" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="imgDateFrom" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgDateFrom" TargetControlID="txtEffectiveDtFrom">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <%-- <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>--%>
                    </td>
                </tr>
                <%-- <tr>--%>
                <%--     <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Passwrord Expiry Days"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Login Status"></asp:Label>
                    </td>
                </tr>--%>
                <%-- <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:DropDownList ID="ddlExpiryDays" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="Password Never Expires" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="10 Days" Value="10"></asp:ListItem>
                            <asp:ListItem Text="15 Days" Value="15"></asp:ListItem>
                            <asp:ListItem Text="20 Days" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30 Days" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlStatus" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="Disabled" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="Enabled" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>--%>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

