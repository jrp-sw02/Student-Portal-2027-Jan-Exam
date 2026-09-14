<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="perfoCriterias.aspx.cs" Inherits="perfoCriterias" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Performance Criteria"></asp:Label>
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseCategoryFilter" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                              <%--  <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Parent Type"></asp:Label>
                                        <asp:DropDownList ID="ddlFParentType" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Location name or Location Type"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {
            if (!isSelected("<%=ddlCourseCategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCityType.ClientID %>", "City Type"))
                return false;
            if (!isBlank("<%=txtMinCand.ClientID %>", "Minimum Candidates"))
                return false;
            if (!isBlank("<%=txtPassPercent.ClientID %>", "Pass Percent"))
                return false;
            if (!isBlank("<%=txtExamCount.ClientID %>", "Exam count"))
                return false;
            if (!isBlank("<%=txtEffectiveFrom.ClientID %>", "Effective From"))
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
        function CheckSelected() {
            if (!isSelected("<%=ddlCourseCategory.ClientID %>", "Course Category"))
                return false;
           
            }
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="false" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="3%" HeaderText="#" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="courseCat" HeaderText="Course Category" SortExpression="courseCat" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="cityType" HeaderText="City Type" SortExpression="cityType"
                                    Target="_self" />
                               
                                 <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="minCand" HeaderText="Minimum Candidates" SortExpression="minCand" 
                                    Target="_self" />
                                 <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="passPercent" HeaderText="Pass Percentage" SortExpression="passPercent"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="examCount" HeaderText="Exam Count" SortExpression="examCount"
                                    Target="_self" />
                                 <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="effFrom" HeaderText="Effective From" SortExpression="effFrom" DataTextFormatString="{0:dd-MMM-yyyy}"
                                    Target="_self" />
                                 <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="effTo" HeaderText="Effective To" SortExpression="effTo" DataTextFormatString="{0:dd-MMM-yyyy}"
                                    Target="_self" />
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
        
            <table class="sample2" cellpadding="0" cellspacing="1">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCourseCategory" runat="server" Text="Course Category<b class='mandatory'>*</b>"
                            SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCityType" runat="server" Text="City Type <b class='mandatory'>*</b>"
                            SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="lblMinCand" runat="server" SkinID="CaptionLabel" Text="Minimum Candidates &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    
                </tr>
                <tr class="even">
                   
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlCourseCategory" runat="server" SkinID="ddl250"
                            TabIndex="1">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlCityType" runat="server" SkinID="ddl250"
                            TabIndex="1">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtMinCand" runat="server" SkinID="txt248" MaxLength="10" TabIndex="3" onkeypress="checkNumber(this,5,0,event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblPassPercent" runat="server" Text="Pass Percentage *" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblExamCount" runat="server" Text="Exam Count" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                         <asp:Label ID="lblEffectiveFrom" runat="server" Text="Effective From(dd-Mon-yyyy) &lt;b class='mandatory'&gt;*&lt;/b&gt;" SkinID="CaptionLabel"></asp:Label>
                           
                    </td>
                   
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        
                                <asp:TextBox SkinID="txt248" ID="txtPassPercent"  onkeypress="checkNumber(this,5,0,event);"
                                    runat="server"  TabIndex="4"></asp:TextBox>
                        </td>      
                    <td style="width: 33%;" valign="top">
                        
                                <asp:TextBox SkinID="txt248" ID="txtExamCount"  onkeypress="checkNumber(this,5,0,event);"
                                    runat="server"  TabIndex="5"></asp:TextBox>
                        </td> 
                     <td style="width: 33%;" valign="top">
                         <asp:TextBox ID="txtEffectiveFrom" runat="server" Width="200px" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                        <img id="imgEFrm" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                       <%-- <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Name of the candidate
                                            at the time of registration.)</span>--%>
                                        <asp:CalendarExtender ID="ceEFrm" TargetControlID="txtEffectiveFrom" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" runat="server">
                                        </asp:CalendarExtender>
                                
                        </td> 
                                        </tr>
                                       
                                       
                                    </table>
                                
                   
                <%--  <tr>
                    <td style="width: 33%;" valign="top">
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>--%>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" TabIndex="7" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" 
                    TabIndex="8" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <%--<table runat="server" visible="false" align="center" class="sidetable" cellspacing="1"
        cellpadding="8" id="tblNavLinks" style="width: 95%;">
        <tr>
            <td style="border: 1px solid #2c5070; color: #ffffff; font-weight: bold; background-color: #31597C;"
                width="100%">
                Navigation Links
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
    </table>--%>
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
