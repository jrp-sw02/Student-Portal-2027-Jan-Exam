<%@ Page Title="Faculty Course Module Mapping" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="FacultyCourse.aspx.cs" Inherits="HO_FacultyCourse"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Faculty - Course - Module Mapping"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter">
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
                                        <asp:Label ID="lblFilterCourse" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlFilterCourse" Width="100%" runat="server"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlFilterCourse_SelectedIndexChanged">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Faculty Name or Faculty Code "
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb3" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (!isSelected("<%=ddlFaculty.ClientID %>", "Faculty Name")) return false;
            if (!isSelected("<%=ddlCourse.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlModule.ClientID %>", "Module Name"))
                return false;
            if (!isBlank("<%=txtEffectiveFrom.ClientID %>", "Effective from cannot be left blank ")) return false;
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
                            class="ActionPopup" style="width: 100px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <%--<asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                     runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                     OnClick="PerformPopupAction"></asp:LinkButton>--%>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID"
                            AllowSorting="True"
                            OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound"
                            AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <%-- Serial Number --%>
                                <asp:BoundField DataField="SNo" HeaderText="#" ReadOnly="true">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>

                                <%-- Faculty Name --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="FacultyCourse.aspx?Key={0}"
                                    DataTextField="FacultyDisplay"
                                    HeaderText="Faculty Name - Code"
                                    SortExpression="FacultyDisplay">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                
                                <%-- Course Name --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="FacultyCourse.aspx?Key={0}"
                                    DataTextField="CourseName"
                                    HeaderText="Course Name"
                                    SortExpression="CourseName">
                                    <HeaderStyle Width="39%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>


                                <%-- Effective From --%>
                                <asp:BoundField DataField="StartDate" HeaderText="Effective From"
                                    SortExpression="StartDate" DataFormatString="{0:yyyy-MM-dd}">
                                    <HeaderStyle Width="13%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <%-- Effective To --%>
                                <asp:BoundField DataField="EndDate" HeaderText="Effective To"
                                    SortExpression="EndDate" DataFormatString="{0:yyyy-MM-dd}">
                                    <HeaderStyle Width="13%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

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
            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" EnableViewState="false" />
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblFaculty" runat="server" Text="Faculty Name - Code *"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFaculty" runat="server" Width="250px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course Name *"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="true" Width="250px"
                            OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                        </asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblModule" runat="server" Text="Module Name *"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModule" runat="server" Width="250px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEffectiveFrom" runat="server" Text="Effective From *"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEffectiveFrom" runat="server" Width="150px"></asp:TextBox>
                        <asp:CalendarExtender ID="calFrom" runat="server" TargetControlID="txtEffectiveFrom" Format="yyyy-MM-dd"></asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEffectiveTo" runat="server" Text="Effective To *"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEffectiveTo" runat="server" Width="150px"></asp:TextBox>
                        <asp:CalendarExtender ID="calTo" runat="server" TargetControlID="txtEffectiveTo" Format="yyyy-MM-dd"></asp:CalendarExtender>
                    </td>
                </tr>
            </table>

            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return ValidateFormFields();" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
