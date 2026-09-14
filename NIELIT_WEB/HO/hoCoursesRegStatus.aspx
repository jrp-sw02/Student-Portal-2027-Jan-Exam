<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="hoCoursesRegStatus.aspx.cs" Inherits="hoCoursesRegStatus"  Debug="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Batches"></asp:Label>
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
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseCategoryFilter" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCourseCategoryFilter_SelectedIndexChanged">
                                            <asp:ListItem>--Select All--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseFilter" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCourseFilter_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Application Type"></asp:Label>
                                        <asp:DropDownList ID="ddlApplicationTypeFilter" Width="100%" runat="server" OnSelectedIndexChanged="ddlApplicationTypeFilter_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Batch Status"></asp:Label>
                                        <asp:DropDownList ID="ddlStatusFilter" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:DropDownList ID="ddlExamNameFilter" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by batch number"
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
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course"))
                return false;
            if (!isSelected("<%=ddlApplicationType.ClientID %>", "Batch Type"))
                return false;

            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlApplicantType.ClientID %>", "Applicant Type"))
                return false;
            if (!isBlank("<%=txtbatchno.ClientID %>", "Batch Number"))
                return false;

            if (!isBlankDate("<%=txtbatchdate.ClientID %>", "Batch Date"))
                return false;
        }

        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
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
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Number" HeaderText="Batch Number" SortExpression="Number" Target="_self">
                                    <HeaderStyle Width="20%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CreatedOn" HeaderText="Date" DataTextFormatString="{0:dd-MMM-yyyy}"
                                    SortExpression="CreatedOn" Target="_self">
                                    <HeaderStyle Width="12%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Course" HeaderText="Course" SortExpression="Course" Target="_self">
                                    <HeaderStyle Width="13%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ApplicationType" HeaderText="Application Type" SortExpression="ApplicationType"
                                    Target="_self">
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ExamName" HeaderText="Exam Name" SortExpression="ExamName" Target="_self">
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Status" SortExpression="StatusID">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl" runat="server"><%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmBatchStatus)Convert.ToInt32(Eval("StatusID")))%></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfCcategory" runat="server" Value="" />
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
            <table class="sample2" cellpadding="0" cellspacing="1" width="100%" id="tblEditInfo"
                runat="server">
                <tr class="heading">
                    <td colspan="3">
                        Batch Detail
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" Text="Course Category&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" AutoPostBack="True" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlApplicationType" runat="server" Height="22px" SkinID="ddl250"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlrequesttype_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseName" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" Text="Applicant Type"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" Text="Batch Number"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseName" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                                <asp:AsyncPostBackTrigger ControlID="ddlApplicationType" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlApplicantType" AutoPostBack="true" runat="server" Height="22px"
                            SkinID="ddl250" OnSelectedIndexChanged="ddlApplicantType_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatchno" runat="server" SkinID="txt248" ToolTip="Batch No" MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseName" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                                <asp:AsyncPostBackTrigger ControlID="ddlApplicationType" />
                                <asp:AsyncPostBackTrigger ControlID="ddlApplicantType" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" Text="Batch Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <%--  <asp:Label ID="Label11" runat="server" Text="Batch Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtbatchdate" runat="server" OnKeyPress="return false" SkinID="txt210"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="ca1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgdate"
                            TargetControlID="txtbatchdate">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                    <td valign="top" colspan="2">
                    </td>
                    <%--<td style="width: 33%;" valign="top">
                    </td>--%>
                </tr>
            </table>
            <div id="btns" runat="server" style="text-align: right; margin-top: 10px; margin-bottom: 6px;">
                <asp:Button ID="btnscan" runat="server" OnClick="btnscan_Click" Text="Start Scanning" style="width: 130px" />
                <asp:Button ID="btnSave" runat="server" OnClick="SaveRecord" OnClientClick="return ValidateFormFields()"
                    Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
            </div>
            <div id="divViewInfo" runat="server" visible="false">
                <table class="sample2" cellpadding="0" cellspacing="0" width="100%">
                    <tr class="heading">
                        <td colspan="4">
                            Batch Details
                            <asp:Label ID="lblmsgg" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="line-height: 1.3pc;" colspan="4" valign="top">
                            Batch Number:
                            <asp:Label ID="lblbno" runat="server"></asp:Label>
                            Dated :
                            <asp:Label ID="bthdate" runat="server"></asp:Label>
                            Applicant Type :
                            <asp:Label ID="lblApplicantType" runat="server"></asp:Label>
                            <br />
                            Application Type :
                            <asp:Label ID="lblbthtype" runat="server"></asp:Label>
                            Course :
                            <asp:Label ID="lblcourse" runat="server"></asp:Label>
                            <asp:Label ID="lblcategory" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="heading">
                        <td colspan="4">
                            Batch Processing Details
                            <asp:LinkButton ToolTip="Click here to scan new applications" ID="lnkNewScan" runat="server"
                                CommandArgument="10" Style="float: right; color: #ffffff" OnClick="hlkreceived_Click">New Scan</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="3" cellspacing="1" class="sample3" style="width: 100%;
                    text-align: left">
                    <tr class="head1">
                        <td width="70%">
                            Application Status
                        </td>
                        <td width="30%">
                            Number of Applications
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            Received But Pending For Payment Verification
                        </td>
                        <td>
                            <asp:LinkButton ID="hlkpendingDDVerify" runat="server" CommandArgument="12" OnClick="hlkreceived_Click">00</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            Application Received
                        </td>
                        <td>
                            <asp:LinkButton ID="hlkreceived" runat="server" CommandArgument="10" OnClick="hlkreceived_Click">00</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            Application Rejected
                        </td>
                        <td>
                            <asp:LinkButton ID="hlkreject" runat="server" CommandArgument="15" OnClick="hlkreceived_Click">00</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            Application Kept In Abeyance
                        </td>
                        <td>
                            <asp:LinkButton ID="hlKeptInAbeyance" runat="server" CommandArgument="14" 
                                OnClick="hlkreceived_Click" >00</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            Application Duplicate
                        </td>
                        <td>
                            <asp:LinkButton ID="hlkduplicate" runat="server" CommandArgument="13" OnClick="hlkreceived_Click">00</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            Application Verified
                        </td>
                        <td>
                            <asp:LinkButton ID="hlkprocess" runat="server" CommandArgument="5" OnClick="hlkreceived_Click">00</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            Total Application
                        </td>
                        <td>
                            <asp:LinkButton ID="hktotal" runat="server" CommandArgument="0" OnClick="hlkreceived_Click">00</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Div1" runat="server" style="text-align: right; margin-top: 10px; margin-bottom: 6px;">
                <asp:Button ID="BtnMrkAsVerified" runat="server" Text="Mark Batch As Completed" OnClick="BtnMrkAsVerified_Click"
                    Visible="False" OnClientClick="return confirm('Are you sure you want to mark this batch as complete?');" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" class="nav" cellspacing="0" cellpadding="0"
        id="tblNavLinks" width="97%" style="width: 97%;">
        <tr>
            <td>
                <div id="header">
                    Navigation Links</div>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#" target="_self">Navgation Link1</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link2</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link3</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link4</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link5</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link6</a>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Navgation Link7 </a>
            </td>
        </tr>
    </table>
</asp:Content>
