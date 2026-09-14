<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="FrmCourseExamApplications.aspx.cs" Inherits="FrmCourseExamApplications"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Course Exam Applications"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="false" />
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlFlCourseCategory" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlFlCourseCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseName" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:DropDownList ID="ddlExamYear" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter4" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:DropDownList ID="ddlExamName" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnFilter" />
                            <asp:PostBackTrigger ControlID="btnReset" />
                        </Triggers>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Application number or candidate name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {

            return true;

        }


        var dtgp = "<%= gvdetail.ClientID %>"
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
                <%-- <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>--%>
                <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
                    Width="99%"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" OnSorting="gvMain_Sorting" DataKeyNames="ID"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                        <asp:HyperLinkField HeaderStyle-Width="2%" DataNavigateUrlFields="ID,examID,categoryid,courseid,examyear,paymentStatus"
                            DataTextField="examName" HeaderText="Exam Name" SortExpression="examName" Target="_self"
                            DataNavigateUrlFormatString="?Key={0}&examID={1}&categoryid={2}&courseid={3}&examyear={4}&paymentStatus={5}"><HeaderStyle Width="25%" /><ItemStyle HorizontalAlign="left" /></asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="2%" DataNavigateUrlFields="ID,examID,categoryid,courseid,examyear,paymentStatus"
                            DataTextField="description" HeaderText="Application Status" SortExpression="description"
                            Target="_self" DataNavigateUrlFormatString="?Key={0}&examID={1}&categoryid={2}&courseid={3}&examyear={4}&paymentStatus={5}"><HeaderStyle Width="50%" /><ItemStyle HorizontalAlign="left" /></asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,examID,categoryid,courseid,examyear,paymentStatus"
                            DataTextField="NumberOfApp" HeaderText="Application Received" SortExpression="NumberOfApp"
                            Target="_self" DataNavigateUrlFormatString="?Key={0}&examID={1}&categoryid={2}&courseid={3}&examyear={4}&paymentStatus={5}"><HeaderStyle Width="15%" /><ItemStyle HorizontalAlign="Right" /></asp:HyperLinkField>
                        <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif"
                            Visible="false"><ItemTemplate><asp:Image ToolTip="Action" ClientIDMode="Static" onclick="PerformAction(this,'popup')"
                                    runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                    Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate><HeaderStyle Width="3%" /></asp:TemplateField>
                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
            <div id="divNavigation" runat="server">
                <%-- <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>--%>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <%-- <asp:UpdatePanel EnableViewState="true" ID="Updatepanellist" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>--%>
            <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                <tr>
                    <td align="right" valign="bottom">
                        <asp:Button Visible="false" ID="btnreceive" runat="server" Text="Mark as Received"
                            OnClientClick="return Validate_Checkbox('Are you sure you want to mark the selected applications as received by NIELIT!')"
                            OnClick="btnreceive_Click" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lbnorecord" runat="server" CssClass="error" EnableTheming="false"
                Visible="false" Width="99%"></asp:Label>
            <asp:GridView ID="gvdetail" runat="server" OnSorting="gvdetail_Sorting" DataKeyNames="ID"
                OnRowDataBound="gvdetail_RowDataBound" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="ID,Appno,apptypeID,key,examID" DataTextField="Appno"
                        HeaderText="Application No." SortExpression="Appno" Target="_self" DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}"><HeaderStyle Width="10%" /><ItemStyle HorizontalAlign="Right" /></asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ID,Appno,apptypeID,key,examID" DataTextField="Appdate"
                        HeaderText="Application Date." SortExpression="Appdate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"
                        DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}"><HeaderStyle Width="15%" /><ItemStyle HorizontalAlign="center" /></asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ID,Appno,apptypeID,key,examID" DataTextField="Name"
                        HeaderText="Candidate Name" SortExpression="Name" Target="_self" DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}"><HeaderStyle Width="30%" /><ItemStyle HorizontalAlign="left" /></asp:HyperLinkField>
                    <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,Appno,apptypeID,key,examID"
                        DataTextField="fathername" HeaderText="Father Name" SortExpression="fathername"
                        Target="_self" DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}"><HeaderStyle Width="25%" /><ItemStyle HorizontalAlign="left" /></asp:HyperLinkField>
                    <asp:TemplateField HeaderStyle-Width="3%" HeaderText=""><HeaderTemplate><asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" /></HeaderTemplate><ItemTemplate><asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" /></ItemTemplate><HeaderStyle Width="3%" /></asp:TemplateField>
                </Columns>
                <PagerSettings Visible="False" />
            </asp:GridView>
            <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChangedOld" />
            <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
            <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnPrint" runat="server" Text="Print" />
                <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click1" />
            </div>
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <%--<asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click"
        Style="float: right;" Visible="false" />--%>
    <table id="tbdata" runat="server" cellspacing="0" cellpadding="1" width="100%" visible="false"
        class="sample3" border="1">
        <%--<tr>
            <td colspan="2" align="right" style="padding-right:0px; margin-right:0px;">
                <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_Click" Visible="false"
                    Text="Generate Report" BorderColor="#666699" BorderStyle="Solid" BackColor="Transparent"/>
            </td>
        </tr>--%>
        <tr class="gdrow1">
            <td colspan="2" align="center">
                Application Status Till Date
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="60%">
                Total Applications :-
            </td>
            <td width="40%" align="right">
                <asp:Label ID="lbtot" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="60%">
                Exported :-
            </td>
            <td align="right">
                <asp:Label ID="lbexported" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>
                Pending to Export:-
            </td>
            <td align="right">
                <asp:Label ID="lbnotexported" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                Pending to Receive:-
            </td>
            <td align="right">
                <asp:Label ID="Lbpendrecieve" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td colspan="2" align="center">
                <asp:Button ID="btnExport" runat="server" Text="Export Pending Applications" OnClientClick="return  ConfirmAction('Are you sure you want to export pending to export applications!');"
                    OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
