<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="QualificationEligiblity.aspx.cs" Inherits="QualificationEligiblityForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Qualification Eligibility"></asp:Label>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Applicant Type"></asp:Label>
                                        <asp:DropDownList ID="ddlAtype" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Qualification Levels"></asp:Label>
                                        <asp:DropDownList ID="ddlQlevels" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Effective From Date"></asp:Label>
                                        <asp:DropDownList ID="ddlEffDtFrom" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
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
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBreadCrumb" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        //                function document.documentElement.onclick() {

        //                    alert(event.srcElement.id);
        //                }
        function ValidateLogin() {
            if (!isSelected("<%=ddlAppType.ClientID %>", "Applicant Type"))
                return false;
            if (!isSelected("<%=ddlQualLevel.ClientID %>", "Qualification Level"))
                return false;
            if (!isBlank("<%=txtExperienceYrs.ClientID %>", "Experience in Years"))
                return false;
            if (!isNumber("<%=txtExperienceYrs.ClientID %>"))
                return false;
            return true;

        }

        var dtgp = "<%= gvMain.ClientID %>"
        function validate(Sender, Mode) {
            var Number = /^[0-9,.]/;
            var arrId = Sender.id.split("_")
            if (Mode == 'e') {
                if (document.getElementById("cphContents_gvMain_ddlApplicantType_" + arrId[3]).value == "0") {
                    alert("Select Applicant Type");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_ddlQLevel_" + arrId[3]).value == "0") {
                    alert("Select Qualification Level");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_txtExperience_" + arrId[3]).value == "") {
                    alert("Enter Experience");
                    return false;
                }
                if ((!(document.getElementById("cphContents_gvMain_txtExperience_" + arrId[3]).value).match(Number))) {
                    alert("Experience must be in digits")
                    document.getElementById("cphContents_gvMain_txtExperience_" + arrId[3]).value = "";
                    document.getElementById("cphContents_gvMain_txtExperience_" + arrId[3]).focus();
                    return false;
                }
            }
            else {
                if (document.getElementById("cphContents_gvMain_ddlApplicantType1").value == "0") {
                    alert("Select Applicant Type");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_ddlQLevel1").value == "0") {
                    alert("Select Qualification Level");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_txtExp").value == "") {
                    alert("Enter Experience");
                    document.getElementById("cphContents_gvMain_txtExp").focus();
                    return false;
                }
                if ((!(document.getElementById("cphContents_gvMain_txtExp").value).match(Number))) {
                    alert("Experience must be in digits")
                    document.getElementById("cphContents_gvMain_txtExp").value = "";
                    document.getElementById("cphContents_gvMain_txtExp").focus();
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
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="false" OnRowCancelingEdit="gvMain_RowCancelingEdit"
                            OnRowEditing="gvMain_RowEditing" OnRowUpdating="gvMain_RowUpdating" Width="100%"
                            OnRowCommand="gvMain_RowCommand" OnRowDeleting="gvMain_RowDeleting">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Applicant Type" SortExpression="ApplicantType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApplicantType" runat="server" Text='<%# Eval("ApplicantType")  %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlApplicantType1" AutoPostBack="true" runat="server" Width="180px">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlApplicantType" AutoPostBack="true" runat="server" Width="99%">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAType" runat="server" Visible="false" Text='<%#  Eval("ApplicantType") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemStyle Width="15%" />
                                    <%-- <FooterStyle Width="25%" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qualification Levels" SortExpression="qualificationLevel">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQLevel" runat="server" Text='<%# Eval("qualificationLevel")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlQLevel1" runat="server" Width="300px">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlQLevel" runat="server" Width="99%">
                                        </asp:DropDownList>
                                        <asp:Label ID="lbQLevel" runat="server" Visible="false" Text='<%#  Eval("qualificationLevel") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemStyle Width="45%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Experience" SortExpression="experience">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExperience" runat="server" Text='<%# Eval("experience") %>'>
                                        </asp:Label>&nbsp;&nbsp;<span style="font-size: small">Years</span>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtExp" runat="server" MaxLength="3" Width="20px"></asp:TextBox>&nbsp;&nbsp;<span
                                            style="font-size: small">Years</span>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtExperience" Width="30%" MaxLength="3" runat="server" Text='<%# Eval("experience") %>'></asp:TextBox>&nbsp;&nbsp;<span
                                            style="font-size: small">Years</span>
                                    </EditItemTemplate>
                                    <ItemStyle Width="20%" />
                                    <%-- <FooterStyle Width="50%" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="&lt;img title='Edit' src='../Images/edit.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lbkUpdate" runat="server" CommandName="Update" OnClientClick="return validate(this,'e')"
                                            CausesValidation="True" Text="&lt;img title='Update' src='../Images/save.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                            Text="&lt;img title='Cancel' src='../Images/cancel.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="true" OnClientClick="return validate(this,'n')"
                                            CommandName="Add" Text="&lt;img title='Click to add new Qualification Eligibility Details ' src='../Images/addwebpart.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                         <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                             Text="&lt;img title='Click to delete this record' src='../Images/delete.gif' style='border-width:0px;' /&gt;"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                               <%-- <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" />--%>
                              <%--  <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" >
                                    <HeaderStyle Width="15%" />
                                </asp:CommandField>--%>
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
                       <%-- <asp:Label ID="lblEffectiveFromDt" runat="server" SkinID="CaptionLabel" Text="Effective Date From&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%>
                    </td>
                    <td valign="top" width="250px">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtExperienceYrs" runat="server" MaxLength="4" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <%--<asp:TextBox ID="txtEffectiveDtFrom" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="imgDateFrom" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgDateFrom" TargetControlID="txtEffectiveDtFrom">
                        </asp:CalendarExtender>--%>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <%-- <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>--%>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
