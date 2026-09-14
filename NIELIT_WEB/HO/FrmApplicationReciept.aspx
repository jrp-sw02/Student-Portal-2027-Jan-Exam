<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="FrmApplicationReciept.aspx.cs" Inherits="FrmApplicationReciept" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Certificate Exam Applications"></asp:Label>
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
                                            Text="" OnClientClick="return validatefilter()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursecategory" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlcourse" Width="100%" runat="server" OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:DropDownList ID="ddlexamcycle" Width="100%" runat="server" OnSelectedIndexChanged="ddlexamcycle_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:DropDownList ID="ddlexamyear" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlexamyear_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:DropDownList ID="ddlexamname" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by application number or candidate name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function validatefilter() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlexamcycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlexamyear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlexamname.ClientID %>", "Exam Name"))
                return false;
        }

        function ValidateLogin() {

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
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbverify" OnClientClick="return ConfirmAction('Are you sure you want to verify this record!');"
                                        runat="server" Text="Verify" ToolTip="click to verify this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction" CommandName="verify"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" OnSorting="gvMain_Sorting" PageSize ="30" OnRowDataBound="gvMain_RowDataBound"
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <ItemStyle HorizontalAlign="Left" Width="2%" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="2%" DataNavigateUrlFields="ID,examID,categoryid,courseid,examyear,examcycle,paymentStatus"
                                    DataTextField="examName" HeaderText="Exam Name" SortExpression="examName" Target="_self"
                                    DataNavigateUrlFormatString="?Key={0}&examID={1}&categoryid={2}&courseid={3}&examyear={4}&examcycle={5}&paymentStatus={6}">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="2%" DataNavigateUrlFields="ID,examID,categoryid,courseid,examyear,examcycle,paymentStatus"
                                    DataTextField="description" HeaderText="Application Status" SortExpression="description"
                                    Target="_self" DataNavigateUrlFormatString="?Key={0}&examID={1}&categoryid={2}&courseid={3}&examyear={4}&examcycle={5}&paymentStatus={6}">
                                    <HeaderStyle Width="50%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,examID,categoryid,courseid,examyear,examcycle,paymentStatus"
                                    DataTextField="NumberOfApp" HeaderText="Application Received" SortExpression="NumberOfApp"
                                    Target="_self" DataNavigateUrlFormatString="?Key={0}&examID={1}&categoryid={2}&courseid={3}&examyear={4}&examcycle={5}&paymentStatus={6}">
                                    <HeaderStyle Width="18%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
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
            <br />
            <div>
                <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain1" runat="server" AutoGenerateColumns="False" Width="100%"
                            HeaderStyle-Font-Size="12px">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <HeaderStyle Width="1%"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="examName" HeaderText="Exam Name">
                                    <HeaderStyle Width="5%" />
                                      <ItemStyle HorizontalAlign="left" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="?CourseId={0}&ExamId={1}&Handicapped=true"
                                    DataTextField="Handicapped" HeaderText="Handicapped" Target="_self">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="?CourseId={0}&ExamId={1}&SingleGuardian=true"
                                    DataTextField="SingleGuardian" HeaderText="SingleGuardian" Target="_self">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <asp:UpdatePanel EnableViewState="true" ID="Updatepanellist" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvdetail" runat="server" OnSorting="gvdetail_Sorting" DataKeyNames="ID"
                        OnRowDataBound="gvdetail_RowDataBound" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                                <HeaderStyle Width="1%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Appno,apptypeID,key,examID" DataTextField="Appno"
                                HeaderText="Application No." SortExpression="Appno" Target="_self" DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}">
                                <HeaderStyle Width="7%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Appno,apptypeID,key,examID" DataTextField="Appdate"
                                HeaderText="App.Date" SortExpression="Appdate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}"
                                DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}">
                                <HeaderStyle Width="16%" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Appno,apptypeID,key,examID" DataTextField="Name"
                                HeaderText="Candidate Name" SortExpression="Name" Target="_self" DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}">
                                <HeaderStyle Width="25%" />
                                <ItemStyle HorizontalAlign="left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField HeaderStyle-Width="27%" DataNavigateUrlFields="InstituteId" DataTextField="Institute"
                                HeaderText="Institute" SortExpression="Institute" Target="_self" DataNavigateUrlFormatString="InstituteDetails.aspx?InstituteId={0}">
                                <HeaderStyle Width="27%" />
                                <ItemStyle HorizontalAlign="left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID,Appno,apptypeID,key,examID"
                                DataTextField="RegionalCenter" HeaderText="Regional Center" SortExpression="RegionalCenter" Target="_self"
                                DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}">
                                <HeaderStyle Width="5%" />
                                <ItemStyle HorizontalAlign="left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField HeaderStyle-Width="7%" DataNavigateUrlFields="ID,Appno,apptypeID,key,examID"
                                DataTextField="Contact" HeaderText="Contact" Target="_self"
                                DataNavigateUrlFormatString="ApplicationDetails.aspx?ID={0}&Appno={1}&ApplicationTypeID={2}&key={3}&examID={4}">
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="left" />
                            </asp:HyperLinkField>
                            <asp:TemplateField HeaderStyle-Width="1%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:Image ToolTip="Action" ClientIDMode="Static" onclick="PerformAction(this,'popup')"
                                        runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                        Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                <HeaderStyle Width="1%" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChangedOld" />
                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="Btnprint" runat="server" Text="Print" OnClick="Btnprint_Click" />
                <asp:Button ID="Button1" runat="server" Text="Back" OnClick="btnCancel_Click1" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
