<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CertificateCourse.aspx.cs" Inherits="Admin_CertificateCourse" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Courses"></asp:Label>
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
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Types"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseType" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCourses" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCourses_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourseName" runat="server" Width="100%">
                                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCourses" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
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
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {

            if (!isBlank("<%=Txtrevisionno.ClientID  %>", "Revision Number"))
                return false;
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlctype.ClientID %>", "Course Type"))
                return false;
            if (!isBlank("<%=txtcoursename.ClientID  %>", "Course Name"))
                return false;
            if (!isBlank("<%=txtcoursecode.ClientID  %>", "Course Code "))
                return false;
            if (!isBlankDate("<%=txteffectivedate.ClientID %>", "Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txteffectivedate.ClientID %>", "Invalid Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlank("<%=txtdisplay.ClientID  %>", "Display Order"))
                return false;
            if (!isNumber("<%=txtdisplay.ClientID  %>", "Display Order"))
                return false;
            if (!isSelected("<%=ddlShowOnWeb.ClientID  %>", "Show on Web"))
                return false;

            if (!isBlank("<%=txtexamserviceID.ClientID  %>", "Examination ServiceID"))
                return false;

            if (document.getElementById("<%=txtregserviceID.ClientID  %>"))
            {
                if(!isBlank("<%=txtregserviceID.ClientID  %>", "Registration ServiceID"))
                return false;
            }
           
            if (!isBlank("<%=txtregperiod.ClientID  %>", "Registration Period"))
                return false;
            if (!isNumber("<%=txtregperiod.ClientID  %>", "Registration Period"))
                return false;
            if (!isSelected("<%=ddlregchances.ClientID  %>", "Re-Registration Chances"))
                return false;
            if (!isBlankDate("<%=Txtregpolicyeffectivedate.ClientID %>", "Registration Policy Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=Txtregpolicyeffectivedate.ClientID %>", "Invalid Registration Policy Date", "dd-MMM-yyyy"))
                return false;
            if (document.getElementById("<%=txtvalidity.ClientID %>").disabled == false) {
                if (!isBlank("<%=txtvalidity.ClientID  %>", "Re-Registration Period"))
                    return false;
                if (!isNumber("<%=txtvalidity.ClientID  %>", "Re-Registration Period"))
                    return false;
            }
            if (document.getElementById("<%=txtreggape.ClientID %>").disabled == false) 
            {
                if (!isBlank("<%=txtreggape.ClientID  %>", "Re-Registration Gape"))
                    return false;
                if (!isNumber("<%=txtreggape.ClientID  %>", "Re-Registration Gape"))
                    return false;
            }
            if (!isSelected("<%=ddllanguage.ClientID  %>", "Language"))
                return false;
            return true;

        }


        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1]
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
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="32%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?CourseId={0}"
                                    DataTextField="Name" HeaderText="Course Name" SortExpression="Name" Target="_self">
                                    <HeaderStyle Width="32%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?CourseId={0}"
                                    DataTextField="CategoryName" HeaderText="Category" SortExpression="CategoryName"
                                    Target="_self">
                                    <HeaderStyle Width="8%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?CourseId={0}"
                                    DataTextField="Code" HeaderText="Code" SortExpression="Code" Target="_self">
                                    <HeaderStyle Width="8%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="13%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?CourseId={0}"
                                    DataTextField="courseType" HeaderText="Course Type" SortExpression="courseType"
                                    Target="_self">
                                    <HeaderStyle Width="13%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="11%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?CourseId={0}"
                                    DataTextField="revno" HeaderText="Current Revision" SortExpression="revno" Target="_self">
                                    <HeaderStyle Width="11%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccID" runat="server" />
                        <asp:HiddenField ID="hfName" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" CurrentPageSize="200" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" Text="Revision No &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Course Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:TextBox ID="Txtrevisionno" runat="server" SkinID="txt248" MaxLength="3" onkeypress="checkNumber(this,3,0,event);"
                            Text="1"></asp:TextBox>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlctype" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlctype_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
 				<!-- Added for sector and specialization-->
                 <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Sector &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" >
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Training Specialization &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                       <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" Text="Whether Future Skill &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                   
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:DropDownList ID="ddlNielitSectorID" runat="server" Height="22px" Width="210px" SkinID="ddl250" OnSelectedIndexChanged="ddlNIELITSector_SelectedIndexChanged"
                                             AutoPostBack="True">
                                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                    </td>
                    <td valign="top" >
                        <asp:DropDownList ID="ddlNielitTrgSpecialization" runat="server" Height="22px" SkinID="ddl250"    >
                                </asp:DropDownList>
                    </td>
                   <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlWhetherFuture" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Course Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Date Of Introduction &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtcoursename" runat="server" MaxLength="140" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtcoursecode" MaxLength="20" runat="server" SkinID="txt248" ToolTip="Code No."></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txteffectivedate" runat="server" SkinID="txt210"></asp:TextBox>
                        <asp:CalendarExtender ID="txteffectivedate_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txteffectivedate">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Display Order &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Display On Web &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lbprevcoursename" runat="server" SkinID="CaptionLabel" Text="Lower Course Name "></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtdisplay" runat="server" MaxLength="3" SkinID="txt248" onkeypress="checkNumber(this,3,0,event);"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlShowOnWeb" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlcourse" runat="server" SkinID="ddl250">
                            <asp:ListItem>---Select One---</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                     <td style="width: 33%;" valign="top">
                        Applicable For
                    </td>
                     <td style="width: 33%;" valign="top">
                         <asp:label id="lblexam" runat="server" skinid="CaptionLabel" text="Examination SeviceID (For Payment)">
                         </asp:label>
                     </td>
                     <td style="width: 33%;" valign="top">
                        <asp:label id="lblreg" runat="server" skinid="CaptionLabel" text="Registration SeviceID (For Payment)"></asp:Label>
                     </td>
                </tr>
                <tr class="even"><td valign="top">
                        <asp:dropdownlist runat="server" ID="ddlapplicanttype" skinid="ddl250">
                            <asp:listitem value="0" selected="true">Both</asp:listitem>
                        </asp:dropdownlist>
                    </td>
                    <td valign="top">
                        <asp:textbox id="txtexamserviceID" maxlength="20" runat="server" skinid="txt248"
                            tooltip="Examination SeviceID.">
                        </asp:textbox>
                    </td>
                    <td valign="top">
                        <asp:textbox id="txtregserviceID" maxlength="20" runat="server" skinid="txt248" 
                        tooltip="Registration SeviceID.">
                        </asp:textbox>
                    </td>
                </tr>
            </table>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%" id="tbreregistration"
                runat="server" visible="false">
                <tr style="background-color: #31597C; color: Blue; line-height: 20px; padding-left: 2px;">
                    <td valign="top" style="width: 100%;" colspan="3">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Registration Policy"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Registration Period(in months) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Re-Registration Chance &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Registration Policy Effective Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txtregperiod" runat="server" MaxLength="2" SkinID="txt248" onkeypress="checkNumber(this,2,0,event);"></asp:TextBox>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlregchances" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlregchances_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="Txtregpolicyeffectivedate" runat="server" SkinID="txt210"></asp:TextBox>
                        <asp:CalendarExtender ID="Txtregpolicyeffectivedate_CalendarExtender" runat="server"
                            Format="dd-MMM-yyyy" PopupButtonID="img1" TargetControlID="Txtregpolicyeffectivedate">
                        </asp:CalendarExtender>
                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Allowed Language &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Lbreregperiod" runat="server" SkinID="CaptionLabel" Text="Re-Registration Period(in months) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlregchances" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="width: 33%;" id="tdlbcoursename" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Lbrereggap" runat="server" SkinID="CaptionLabel" Text="Re-Registration Gap(In Month) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlregchances" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddllanguage" runat="server" SkinID="ddl250">
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtvalidity" runat="server" MaxLength="2" SkinID="txt248" onkeypress="checkNumber(this,2,0,event);"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlregchances" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtreggape" runat="server" MaxLength="2" SkinID="txt248" onkeypress="checkNumber(this,2,0,event);"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlregchances" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
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
            <td>
                <asp:HyperLink ID="hlCourseLevelDuration" runat="server" Target="_self">Course Level Duration</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlmodule" runat="server" Target="_self">Modules</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlModuleParity" runat="server" Target="_self">Modules Parity</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlModuleExemption" runat="server" Target="_self">Modules Exemption</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="h2Exams" runat="server" Target="_self">Exams</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <%--<a  href="ExamCentres.aspx?CourseId1=<%=  hfAccID.Value %> &Cate=<%= 1 %> &name1=<%=  hfName.Value  %>" target="_self">Exam Centres</a>--%>
                <asp:HyperLink ID="h1cexam" runat="server" Target="_self">Exam Centres</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlDownload" runat="server" Target="_self">Upload Document</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlQualification" runat="server" Target="_self">Qualification Eligibility</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="h1FeeDetail" runat="server" Target="_self">Fee Detail</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlrevision" runat="server" Target="_self">Show Revisions</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlregpolicy" runat="server" Target="_self">Registration Policy</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="hlTimeTablePattern" runat="server" Target="_self">Time Table Pattern</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
