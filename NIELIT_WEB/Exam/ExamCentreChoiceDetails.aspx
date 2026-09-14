<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ExamCentreChoiceDetails.aspx.cs" Inherits="ExamCentreChoiceDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Generate / view Exam Centre Choice"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
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
                                            Text="" OnClick="AllyFilter" OnClientClick="return ValidateFilter()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlflCourse" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlflCourse_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExamCycle" Width="100%" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlflExamCycle_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflCourse" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExamYear" Width="100%" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlflExamYear_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflExamCycle" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExam" Width="100%" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlflExam_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflExamYear" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Label13" Width="100%" runat="server" Text="Regional Centre"></asp:Label>
                                                <asp:DropDownList ID="ddlRegionalCentre" Width="100%" runat="server">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflExam" EventName="SelectedIndexChanged" />
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
        function Validate() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlRc.ClientID %>", "Regional Centre"))
                return false;

        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnRowDataBound="gvMain_RowDataBound"
                            AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="5%" DataField="StateName" HeaderText="State">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="5%" DataField="CityName" HeaderText="City">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="5%" DataField="CityCode" HeaderText="City-Code">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="10%" DataField="FirstChoice" HeaderText="First Choice">
                                    <HeaderStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-Width="10%" DataField="SecondChoice" HeaderText="Second Choice">
                                    <HeaderStyle Width="10%" />
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
                        <uc3:PagingBar ID="PagingBar1" runat="server" CurrentPageSize="20" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>               
            </div>
             <div style="text-align: right; margin-top: 10px">
                    <asp:Button ID="btnDownload" runat="server" Text="Download" Visible="true" OnClick="btnDownload_Click" /></div>
                     <br /><br /><br /><br /><br /> <br /><br /><br /><br /><br /> <br /><br />
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourse" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                    <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamCycle" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                                    <asp:ListItem>--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33px;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="--Select One--">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged">
                                    <asp:ListItem>--Select One-</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlRc" runat="server" Height="22px" SkinID="ddl250">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClientClick="return Validate();"
                    OnClick="btnGenerate_Click" />
            </div>
             <br /><br /><br /><br /><br /> <br /><br /><br /><br /><br /> 
        </asp:View>         
    </asp:MultiView>
    <br /><br /><br /><br /><br /> <br /><br /><br /> <br /><br /><br />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
