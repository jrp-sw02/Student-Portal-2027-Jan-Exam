<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="admexamdetail.aspx.cs" Inherits="admexamdetail" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Time Table"></asp:Label>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" 
        runat="server" Visible="False" />
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Module Type"></asp:Label>
                                        <asp:DropDownList ID="ddlFlterModuletype" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Exam Session" 
                                            Visible="False"></asp:Label>
                                        <asp:DropDownList ID="ddlFilterExamSession" Width="100%" runat="server" 
                                            Visible="False">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
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

        function ValidateLogin() {
            if (!isSelected("<%=ddlModuleNType.ClientID %>", "Module Type"))
                return false;
            if (!isSelected("<%=ddlModuleName.ClientID %>", "Module Name"))
                return false;
            if (!isSelected("<%=ddlExamSession.ClientID %>", "Exam Session"))
                return false;

            if (!isBlankDate("<%=txtDateOfExam.ClientID %>", "Date Of Exam", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateOfExam.ClientID %>", "Invalid Date Of Exam", "dd-MMM-yyyy"))
                return false;
            return true;

        }
        var dtgp = "<%= gvMain.ClientID %>"
        function validate(Sender, Mode) {
            var Number = /^[0-9,.]/;
            var arrId = Sender.id.split("_")
            if (Mode == 'e') {
                if (document.getElementById("cphContents_gvMain_ddlModuleType_" + arrId[3]).value == "0") {
                    alert("Select Module Type");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_ddlmName_" + arrId[3]).value == "0") {
                    alert("Select Module Name");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_ddleSession_" + arrId[3]).value == "0") {
                    alert("Select Exam Session");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_txteDate_" + arrId[3]).value == "") {
                    alert("Enter Exam Date");
                    return false;
                }

            }
            else {
                if (document.getElementById("cphContents_gvMain_ddlModuleType1").value == "0") {
                    alert("Select Module Type");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_ddlmName1").value == "0") {
                    alert("Select Module Name");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_ddleSession1").value == "0") {
                    alert("Select Exam Session");
                    return false;
                }
                if (document.getElementById("cphContents_gvMain_txteDate1").value == "") {
                    alert("Enter Exam Date");
                    return false;
                }
            }
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
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <div align="right">
                    <asp:Label ID="lblPublish" runat="server" Text="Enter Publishing Date" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtPublish" runat="server" Visible="false" Width="80px"></asp:TextBox>
                    <asp:CalendarExtender
                        ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtPublish"
                        TargetControlID="txtPublish">
                    </asp:CalendarExtender>
                    <asp:Button ID="btnPublish" runat="server" Text="Publish" visible="false" 
                        onclick="btnPublish_Click" />
                    <asp:Button ID="btnSaveList" runat="server" Text="Save" OnClick="btnSaveList_Click" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" 
                       />
                </div>
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError1" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,ExamSession,TimeTableID"
                            Width="100%" OnRowDataBound="gvMain_RowDataBound" OnSorting="gvMain_Sorting">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <HeaderStyle Width="1%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="ModuleTypeID" HeaderText="Module Type ID"><HeaderStyle Width="10%"  /></asp:BoundField>
                                <asp:BoundField DataField="ModuleID" HeaderText="Module ID"><HeaderStyle Width="45%" /></asp:BoundField>--%>
                                
                                <asp:BoundField DataField="ShortName" HeaderText="S.Name">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ModuleName" HeaderText="Module Name">
                                    <HeaderStyle Width="45%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ModuleType" HeaderText="Type">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Exam Session">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlExamSession" runat="server" Width="80px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtExamDate" Text='<%#  Bind("ExamDate","{0:dd-MMM-yyyy}") %>' runat="server" Width="75px"></asp:TextBox>
                                        
                                        <asp:CalendarExtender
                                            ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtExamDate"
                                            TargetControlID="txtExamDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtExamToDate" Text='<%#  Bind("ExamToDate","{0:dd-MMM-yyyy}") %>' runat="server" Width="75px"></asp:TextBox>
                                        
                                        <asp:CalendarExtender
                                            ID="Calendarextender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtExamToDate"
                                            TargetControlID="txtExamToDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
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
                        <asp:Label ID="lblAppType" runat="server" SkinID="CaptionLabel" Text="Module Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblQuallevel" runat="server" SkinID="CaptionLabel" Text="Module Name&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlModuleNType" runat="server" SkinID="ddl250" Height="22px"
                            AutoPostBack="True">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlModuleName" runat="server" Height="22px" Width="500px">
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlModuleNType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lbExperience" runat="server" SkinID="CaptionLabel" Text="Exam Name&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td valign="top" width="250px">
                        <asp:Label ID="lblEffectiveFromDt" runat="server" SkinID="CaptionLabel" Text="Date of Exam&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" width="250px">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlExamSession" runat="server" Height="22px" SkinID="ddl250">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtDateOfExam" runat="server" SkinID="txt210"></asp:TextBox>
                        <img id="imgDateFrom" alt="Calender" src="../images/calendaricon.jpg" />
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgDateFrom" TargetControlID="txtDateOfExam">
                        </asp:CalendarExtender>
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
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="certificateexamdetail.aspx" target="_self">Time Table</a>
            </td>
        </tr>
    </table>
    <style type="text/css">
        .sidetable a
        {
            color: #000000;
            text-decoration: none;
        }
        .sidetable a:hover
        {
            color: #3366CC;
            text-decoration: underline;
        }
    </style>
</asp:Content>
