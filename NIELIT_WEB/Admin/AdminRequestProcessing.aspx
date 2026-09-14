<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AdminRequestProcessing.aspx.cs" Inherits="AdminRequestProcessing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Student Request Processing"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" 
        runat="server" Visible="False" />
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Filter1"></asp:Label>
                                        <asp:DropDownList ID="ddlFilter1" Width="100%" runat="server">
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
     <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Cycle Name"
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
                        <asp:GridView ID="gvMain" runat="server" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField   HeaderText="#" >
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField
                                DataNavigateUrlFields="RequestType"  DataTextField="RequestType" HeaderText="Request Type" SortExpression="RequestType"
                                    Target="_self" DataNavigateUrlFormatString="?key={0}">
                                <ItemStyle Width="200px" />
                                </asp:HyperLinkField>
                                <asp:BoundField DataField="RequestNo" HeaderText="Request No." 
                                    SortExpression="RequestNo" >
                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Request Date" SortExpression="RequestDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestDate" runat="server" 
                                            Text='<%# Convert.ToDateTime(Eval("RequestDate")).ToString("dd-MMM-yyyy")  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" 
                                            Text='<%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmRequestStatus) Eval("Status")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="true" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server"  
                            OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server"><%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing ="0">
                <tr>
                    <td colspan="2" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" 
                            Text="Request Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" 
                            Text="Request Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td colspan="2" valign="top"  >
                        <asp:TextBox ID="txtDept" runat="server" SkinID="txt502" 
                            ToolTip="Department Name"></asp:TextBox> 
                       
                    </td>
                    <td style="width:33%;" valign="top">
                         <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="True" 
                                    onchange="SetNames(this);" onfocusin="CheckSelectedDept();" 
                                      SkinID="ddl250" Height="22px">
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" 
                            Text="Employee Name &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="100%"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" 
                            Text="User ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" 
                            Text="User Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width:33%;" valign="top">
                       <asp:DropDownList ID="ddlEntity" runat="server" onchange="SetNames(this);" 
                                    SkinID="ddl250">
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:TextBox ID="txtUserId" runat="server" MaxLength="20" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" 
                            Text="Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" 
                            Text="Passwrord Expiry Days"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" 
                            Text="Request Status"></asp:Label>
                    </td>
                </tr>
                <tr class="even" >
                    <td style="width:33%;" valign="top" >
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width:33%;border:1px solid #A8A8A8;" valign="top">
                        <asp:DropDownList ID="ddlExpiryDays" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="Password Never Expires" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="10 Days" Value="10"></asp:ListItem>
                            <asp:ListItem Text="15 Days" Value="15"></asp:ListItem>
                            <asp:ListItem Text="20 Days" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30 Days" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:DropDownList ID="ddlStatus" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="Disabled" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="Enabled" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table> 
            <div style="text-align:right; margin-top:10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" 
                            Text="Save" onclick="SaveRecord" />
                        <asp:Button ID="btnCancel" runat="server" 
                            Text="Cancel" onclick="btnCancel_Click" /></div>
        </asp:View>

    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false"   align="center" class="sidetable" cellspacing="1" cellpadding="8" id="tblNavLinks"
        style="width: 95%;">
        <tr>
            <td style="border: 1px solid #2c5070; color:#ffffff; font-weight:bold; background-color: #31597C;" width="100%">
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
    </table>
    <style type="text/css">
        .sidetable a
        {
        	color:#000000;
        	text-decoration:none;
        
        } 
        .sidetable a:hover
        {
        	color:#3366CC;
            text-decoration:underline;
        }   
        
     
    </style>
</asp:Content>