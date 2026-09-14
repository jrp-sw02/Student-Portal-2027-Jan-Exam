<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DuplicateApaarCheck.aspx.cs" Inherits="DuplicateApaarCheck" Debug="True"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>


<asp:Content id="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="CCC Duplicate Apaar Candidate List"></asp:label>
</asp:Content>


<asp:Content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
<%--    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />--%>
    <asp:Panel runat="server" id="pnlFilter" visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel enableviewstate="true" rendermode="Inline" id="filterPnal_upnlFilter"
                        updatemode="Conditional" runat="server">
                        <ContentTemplate>
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClientClick="return ValidateLogin()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                         
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseFilter" Width="100%" runat="server" 
                                            AutoPostBack="True" onselectedindexchanged="ddlCourseFilter_SelectedIndexChanged"
                                            >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblexamcycle" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:DropDownList ID="ddlExamCycle" Width="100%" runat="server" 
                                            AutoPostBack="True" onselectedindexchanged="ddlExamCycle_SelectedIndexChanged">
                                            <asp:ListItem>--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                          
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:DropDownList ID="ddlExamYear" Width="100%" runat="server" 
                                            AutoPostBack="true" onselectedindexchanged="ddlExamYear_SelectedIndexChanged"
                                            >
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
                        <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ucSearchBar" EventName="LnkBtnGO" />
                        </triggers>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Number, Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10"  />

    <%--AutoCompleteContextKey="1"--%>
</asp:Content>
<asp:Content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <asp:UpdatePanel enableviewstate="true" id="upBread" updatemode="Conditional" runat="server">
        <contenttemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
        function ValidateLogin() {
            if (!isSelected("<%=ddlCourseFilter.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year "))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            return true;

        }

        document.addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
                return false;
            }
        });
       
    </script>

   
     <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
      runat="server"></asp:Label>

      <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblMessage" Visible="True"
    runat="server"></asp:Label>


            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                      <%--  <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>--%>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Number" HeaderText="Number" />
                                <asp:BoundField DataField="Name" HeaderText="Name" NullDisplayText="&nbsp;&nbsp;&nbsp;-" />
                                <asp:BoundField DataField="Father_Name" HeaderText="Father_Name" NullDisplayText="&nbsp;&nbsp;&nbsp;-" />
                                <asp:BoundField DataField="Dob" HeaderText="Dob" DataFormatString="{0:dd-MMM-yyyy}" NullDisplayText="&nbsp;&nbsp;&nbsp;-" />
                                <asp:BoundField DataField="Gender" HeaderText="Gender" NullDisplayText="&nbsp;&nbsp;&nbsp;-" />
                                <asp:BoundField DataField="Exam" HeaderText="Exam" NullDisplayText="&nbsp;&nbsp;&nbsp;-" />
                                <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" NullDisplayText="&nbsp;&nbsp;&nbsp;-" />
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
                        <uc3:PagingBar ID="PagingBar1" Visible="false" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
</asp:Content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
