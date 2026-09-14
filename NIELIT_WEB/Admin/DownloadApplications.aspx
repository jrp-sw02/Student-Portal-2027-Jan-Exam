<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DownloadApplications.aspx.cs" Inherits="Admin_DownloadApplications"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Download Applications"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" runat="server" Visible="false" OnBtnMode_Click="ToggleViewMode_Changed" />
    <asp:panel runat="server" id="pnlFilter" visible="false">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:updatepanel enableviewstate="true" rendermode="Inline" id="filterPnal_upnlFilter"
                        updatemode="Conditional" runat="server">
                        <contenttemplate>
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btn1" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClientClick="return validatefilter()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </contenttemplate>
                    </asp:updatepanel>
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
    </asp:panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Candidate Name or Father Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <asp:updatepanel enableviewstate="true" id="upBread" updatemode="Conditional" runat="server">
        <contenttemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </contenttemplate>
    </asp:updatepanel>
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {

            //if (document.getElementById("<%=ddlRc.ClientID %>").disabled == false) {
               // if (!isSelected("<%=ddlRc.ClientID %>", "Regional Centre"))
                  //  return false;
          //  }
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
        }
        function OpenWindow() {
        }
    </script>
    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:label id="Label1" runat="server" skinid="CaptionLabel" text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label2" runat="server" skinid="CaptionLabel" text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label3" runat="server" skinid="CaptionLabel" text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:dropdownlist id="ddlRc" runat="server" height="22px" skinid="ddl250" OnSelectedIndexChanged="ddlRc_SelectedIndexChanged">
                    <asp:listitem value="0">--All--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td>
                <asp:dropdownlist id="ddlCourseCategry" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlCourseCategry_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>--%>
                <asp:dropdownlist id="ddlCourseName" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlCourseName_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:label id="Label4" runat="server" skinid="CaptionLabel" text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label5" runat="server" skinid="CaptionLabel" text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label6" runat="server" skinid="CaptionLabel" text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>--%>
                <asp:dropdownlist id="ddlAppType" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlAppType_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>--%>
                <asp:dropdownlist id="ddlExamCycle" runat="server" autopostback="True" height="22px"
                    onselectedindexchanged="ddlExamCycle_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>--%>
                <asp:dropdownlist id="ddlExamYear" runat="server" skinid="ddl250" autopostback="true"
                    onselectedindexchanged="ddlExamYear_SelectedIndexChanged">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                <%-- </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td>
                <asp:label id="Label7" runat="server" skinid="CaptionLabel" text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="even">
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                <asp:dropdownlist id="ddlExamName" runat="server" skinid="ddl250" autopostback="true"
                    onselectedindexchanged="ddlExamName_SelectedIndexChanged">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <%--  <asp:UpdatePanel ID="Upddata" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <div id="divBatchDetails" runat="server" visible="false">
        <asp:label runat="server" id="lblApplicationsDetails" cssclass="error" text="" visible="False"
            width="100%" enabletheming="False"></asp:label>
        <table id="tblGrid" runat="server" class="sample2" width="100%" border="0" cellpadding="2"
            cellspacing="0">
            <tr id="TrBatchDetailHeader" runat="server">
                <td colspan="3">
                    <asp:label id="lblBatchDetailHeader" runat="server" skinid="CaptionLabel" text="Batch Details ">
                    </asp:label>
                </td>
            </tr>
            <tr id="trBatchDetail" runat="server" class="even">
                <td id="tdImages" runat="server" colspan="3">
                    <div id="divGrid" runat="server">
                        <asp:updatepanel enableviewstate="true" id="uPnlGrid" updatemode="Conditional" runat="server">
                            <contenttemplate>
                                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblErrorGrid" Visible="false"
                                            runat="server"></asp:Label>
                                        <asp:GridView ID="gvMain" runat="server" OnSorting="gvMain_Sorting" OnRowDataBound="gvMain_RowDataBound"
                                            AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                                    <HeaderStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="Batch Number" DataField="BatchNumber"
                                                    SortExpression="BatchNumber">
                                                    <HeaderStyle Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="Number Of Applications" DataField="applCount"
                                                    SortExpression="applCount">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Batch Status" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmBatchStatus)Eval("Status"))%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Visible="False" />
                                        </asp:GridView>
                                     
                                    </contenttemplate>
                        </asp:updatepanel>
                    </div>
                    <div id="divNavigation" runat="server">
                        <asp:updatepanel rendermode="Inline" id="uPnlNavigation" updatemode="Conditional"
                            runat="server">
                            <contenttemplate>
                                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                                    </contenttemplate>
                        </asp:updatepanel>
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div id ="divPC" runat = "server">
                        <asp:UpdatePanel  ID="Updatepanel1"  runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvPendingCount" runat="server" AutoGenerateColumns="false" AllowPaging=" true" Visible ="true">
                                    <Columns>                                
                                <%--<asp:TemplateField>
                                  <HeaderTemplate>
                                           #
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <%# Container.DataItemIndex + 1 %>
                                  </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="#">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Applied_Exam_Name" HeaderText="Exam Name" SortExpression="BrowserName" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Regional_Center_Name" HeaderText="Regional Centre Name" SortExpression="ClientIP" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Course_Name" HeaderText="Course Name" SortExpression="SourceIP" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Pending_Count" HeaderText="Pending Count" SortExpression="SourceIP" ItemStyle-HorizontalAlign="Left" />
                                                                 
                            </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </td>
            </tr>
            <%-- <tr>
                        <td colspan="3" align="right">--%>
            <%--  </td>
                    </tr>--%>
        </table>
    </div>
    <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <div style="text-align: right; margin-top: 10px" runat="server" id="divDownload">
         <asp:button id="btnPendingCount" runat="server" text="View Pending Count" onclientclick="return OpenWindow();"
           visible="false" OnClick="btnPendingCount_Click" />
        <asp:button id="btnView" runat="server" text="View Applications" onclientclick="return OpenWindow();"
            onclick="btnView_Click" visible="false" />
        <asp:button id="btnDownload" runat="server" text="Download Applications" onclick="btnDownload_Click"
            onclientclick="return validatefilter();" clientidmode="Static" />
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />
    </div>
    <asp:hiddenfield id="hfActionID" runat="server" value="" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
