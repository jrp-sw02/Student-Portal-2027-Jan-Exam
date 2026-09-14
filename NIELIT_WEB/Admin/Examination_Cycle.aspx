<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="Examination_Cycle.aspx.cs" Inherits="Examination_Cycle" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Examination Cycle"></asp:Label>
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
                                            Text="Select"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCategory" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourse" runat="server" Width="100%">
                                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Exam Schedule"></asp:Label>
                                        <asp:DropDownList ID="ddlExamSchedule" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Cycle Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {

            if (!isSelected("<%=ddlCourseCategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isBlank("<%=txtExamName.ClientID %>", "Exam Cycle Name"))
                return false;
            if (!isSelected("<%=ddlMonthCycle.ClientID %>", "Exam Schedule"))
                return false;
            if (!isSelected("<%=ddlStartMonth.ClientID %>", "Starting Month"))
                return false;
            return true;

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
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?key={0}"
                                    DataTextField="name" HeaderText="Exam Cycle Name" SortExpression="name" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?key={0}"
                                    DataTextField="CourseName" HeaderText="Course Name" SortExpression="CourseName"
                                    Target="_self" />
                                <asp:TemplateField HeaderText="Exam Schedule" SortExpression="MonthCycle">
                                    <ItemTemplate>
                                        <%--  <asp:Label ID="Label1" runat="server" Text='<%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmExamSchedule)Eval("MonthCycle")) %>'></asp:Label>--%>
                                        <a href="Examination_Cycle.aspx?key=<%#Eval("ID")%>"><span></span>
                                            <%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmExamSchedule)Eval("MonthCycle")) %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
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
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCategory" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCourse" runat="server" SkinID="CaptionLabel" Text="Course Name&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblExamName" runat="server" SkinID="CaptionLabel" Text="Exam Cycle Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlCourseCategory" runat="server" SkinID="ddl250" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCourseCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourseName" runat="server" SkinID="ddl250">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtExamName" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblMonthCycle" runat="server" SkinID="CaptionLabel" Text="Exam Schedule &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblStartMonth0" runat="server" SkinID="CaptionLabel" Text="Starting Month&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                        
                    </td>
                    <td style="width: 33%;" valign="top">
                        <%--<asp:Label ID="lblStartMonth" runat="server" SkinID="CaptionLabel" Text="Starting Week&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlMonthCycle" runat="server" SkinID="ddl250">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlStartMonth" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="January" Value="1"></asp:ListItem>
                            <asp:ListItem Text="february" Value="2"></asp:ListItem>
                            <asp:ListItem Text="March" Value="3"></asp:ListItem>
                            <asp:ListItem Text="April" Value="4"></asp:ListItem>
                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                            <asp:ListItem Text="June" Value="6"></asp:ListItem>
                            <asp:ListItem Text="July" Value="7"></asp:ListItem>
                            <asp:ListItem Text="August" Value="8"></asp:ListItem>
                            <asp:ListItem Text="September" Value="9"></asp:ListItem>
                            <asp:ListItem Text="October" Value="10"></asp:ListItem>
                            <asp:ListItem Text="November" Value="11"></asp:ListItem>
                            <asp:ListItem Text="December" Value="12"></asp:ListItem>
                        </asp:DropDownList>
                        
                    </td>
                    <td style="width: 33%;" valign="top">
                        
                    </td>
                </tr>
                <%-- <tr>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" 
                            Text="Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" 
                            Text="Passwrord Expiry Days"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" 
                            Text="Login Status"></asp:Label>
                    </td>
                </tr>--%>
                <%--  <tr class="even" >
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width:33%;border:1px solid #A8A8A8;" valign="top">
                        <asp:DropDownList ID="ddlExpiryDays" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="Password Never Expires" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="10 Days" Value="10"></asp:ListItem>
                            <asp:ListItem Text="15 Days" Value="15"></asp:ListItem>
                            <asp:ListItem Text="20 Days" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30 Days" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top">
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
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <asp:HyperLink ID="h2Exams" runat="server" Target="_self">Exams</asp:HyperLink>
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
