<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ExamCentres.aspx.cs" Inherits="ExamCentres"  Debug="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Centres"></asp:Label>
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="States"></asp:Label>
                                        <asp:DropDownList ID="ddlflstates" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlficoursecategory" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlficoursecategory_SelectedIndexChanged">
                                            <asp:ListItem>--Select All--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlflcourse" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--<asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Centre Type"></asp:Label>
                                        <asp:DropDownList ID="ddlflexcentretype" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>--%>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Centre Name"
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
            if (!isSelected("<%=ddlCourseCategory.ClientID %>", "Course Category"))
                return false;
//            if (!isSelected("<%=ddlCourse.ClientID %>", "Course"))
//                return false;
            if (!isSelected("<%=ddlExamCentreType.ClientID %>", "Exam Centre Type"))
                return false;
            if (!isSelected("<%=ddlState.ClientID %>", "State"))
                return false;
            if (!isBlank("<%=txtCode.ClientID %>", "Exam Centre Code"))
                return false;
            if (!IsValidMinMaxLenght("<%=txtCode.ClientID %>", 3, 5, "Exam Centre Code length should be between 3 and 5 characters"))
                return false;
            if (!isBlank("<%=txtCenterName.ClientID %>", "Exam Centre Name"))
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
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <%--<asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>--%>
                                    <asp:LinkButton ID="lbdisable" OnClientClick="return ConfirmAction('Are you sure you want to change the status of the selected exam center!');"
                                        runat="server" Text="Change Status" ToolTip="click to change status of this record" 
                                        SkinID="lnkbtnAction" onclick="lbdisable_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID,CourseCategoryID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CategoryID={1}" DataTextField="Name"
                                    HeaderText="Centre Name" SortExpression="Name" Target="_self">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseCategoryID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CategoryID={1}" DataTextField="Code"
                                    HeaderText="Code" SortExpression="Code" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseCategoryID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CategoryID={1}" DataTextField="State"
                                    HeaderText="State" SortExpression="State" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,CourseCategoryID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CategoryID={1}" DataTextField="Course"
                                    HeaderText="Course" SortExpression="Course" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Type" SortExpression="CentreType" Visible="false">
                                    <ItemTemplate>
                                        <span>
                                            <%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmExamCenterType)Eval("CentreType"))%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,CourseId" DataNavigateUrlFormatString="?Key={0}&CategoryID={1}"
                                    DataTextField="IsActive" HeaderText="Active Status" SortExpression="IsActive"
                                    Target="_self">
                                </asp:HyperLinkField>
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
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course "></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                       
                         <asp:Label ID="lblState" runat="server" SkinID="CaptionLabel" Text="State &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlCourseCategory" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourse" runat="server" SkinID="ddl250" AutoPostBack="True">
                                    <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                       
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" 
                                    SkinID="ddl250" onselectedindexchanged="ddlState_SelectedIndexChanged">
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>

                 <tr>
                   <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="District &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td id ="tdRc" runat="server" visible="true" >

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblRc" runat="server" SkinID="CaptionLabel" Text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                    <td>

                    </td>
                </tr>
                <tr>
                     <td style="width: 33%;" valign="top">
                       
                         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" 
                                    SkinID="ddl250"  OnSelectedIndexChanged ="ddlDistrict_SelectedIndexChanged" >
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                         <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlRC" runat="server" AutoPostBack="true" Visible ="false" 
                                    SkinID="ddl250" OnSelectedIndexChanged="ddlRC_SelectedIndexChanged" >
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>

                    </td>
                </tr>

                <tr>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="lblexamcentre" runat="server" SkinID="CaptionLabel" Text="Exam Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblDistrict0" runat="server" SkinID="CaptionLabel" Text="Exam Centre Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                         <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Exam Centre Type &lt;b class='mandatory'&gt;*&lt;/b&gt;" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtCenterName" runat="server" SkinID="txt248" ToolTip="Exam Centre"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCode" runat="server" MaxLength="5" SkinID="txt248" style="text-transform:uppercase;"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlExamCentreType" runat="server" SkinID="ddl250" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>

               

            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
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
            <td>
                <asp:HyperLink ID="hlExamMenu" runat="server" Target="_self">Exam Venues</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
