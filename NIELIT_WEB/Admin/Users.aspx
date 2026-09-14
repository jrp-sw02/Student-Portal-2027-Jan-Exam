<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="Users.aspx.cs" Inherits="Admin_Users" Debug ="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Users"></asp:Label>
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
                                            Text="" OnClientClick="return ValidateFilter()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUserType" Width="100%" runat="server" Text="User Type"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchUserType" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Login Status"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchLoginStatus" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                            <asp:ListItem Value="1">Enabled</asp:ListItem>
                                            <asp:ListItem Value="2">Disabled</asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel ID="urtr" UpdateMode="Always"
        runat="server">
        <ContentTemplate>
                <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        var entityName = "";
        function OnEntitySelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            if (results.ID == "0")
                return false;
            document.getElementById("<% =hfEntityID.ClientID %>").value = results.ID;
            entityName = results.Name;
            __doPostBack("<% =hfEntityID.ClientID %>", "");
        }
        function ValidateFilter() {
            if (!isSelected("<%=ddlSearchUserType.ClientID %>", "User type"))
                return false;
        }
        function ValidateLogin() {
            if (!isSelected("<%=ddlUserType.ClientID %>", "User type"))
                return false;

            if (!isBlank("<%=txtUserId.ClientID %>", "User ID"))
                return false;
            if (!isBlank("<%=txtUserName.ClientID %>", "User Name"))
                return false;
            if (trim(document.getElementById("<%=txtEmail.ClientID %>").value, " ") == "" || trim(document.getElementById("<%=txtMobileNumber.ClientID %>").value, " ") == "") {
                alert("Email address or mobile number not found of this user. Please update user's profile first");
                return false;
            }
            if (!isBlank("<%=txtEmail.ClientID %>", "E-mail"))
                return false;
            if (!isValidEmail("<%=txtEmail.ClientID %>", "Not A Valid Email Address"))
                return false;
            if (!isBlankNumber("<%=txtMobileNumber.ClientID %>", "Mobile Number"))
                return false;
            if (!isNumber("<%=txtMobileNumber.ClientID %>"))
                return false;
            if (!chekMobNo("<%=txtMobileNumber.ClientID %>"))
                return false;
            if (!isSelected("<%=Ddlrolename.ClientID %>", "Default Role Name"))
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
                                    <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                        runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                        CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                        runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                        SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="UserID,OrganizationID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="#" >
                                <ItemStyle Width="2%" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="UserID,OrganizationID" DataNavigateUrlFormatString="?Key={0}&OrgId={1}"
                                    DataTextField="LoginID" HeaderText="Login ID" SortExpression="LoginID" 
                                    Target="_self" >
                                <ItemStyle Width="15%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="UserID,OrganizationID" DataNavigateUrlFormatString="?Key={0}&OrgId={1}"
                                    DataTextField="UserName" HeaderText="User Name" SortExpression="UserName" 
                                    Target="_self" >
                                <ItemStyle Width="32%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="UserID,OrganizationID" DataNavigateUrlFormatString="?Key={0}&OrgId={1}"
                                    DataTextField="enmUserType" HeaderText="User Type" SortExpression="enmUserType"
                                    Target="_self" >
                                <ItemStyle Width="13%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="UserID,OrganizationID" DataNavigateUrlFormatString="?Key={0}&OrgId={1}"
                                    DataTextField="LastLoginDateTime" DataTextFormatString="{0:dd-MMM-yyyy hh:mm tt}"
                                    HeaderText="Last Login Date" SortExpression="LastLoginDateTime" 
                                    Target="_self" >
                                <ItemStyle Width="20%" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderText="Login Status" SortExpression="HasLoginAccess">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lbStatus" runat="server" Text='<%# Eval("HasLoginAccess") ==true? "Enabled": "Disabled" %>'></asp:Label>--%>
                                        <asp:Label ID="lbStatus" runat="server" Text='<%# Eval("HasLoginAccess") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfLoginID" runat="server" Value="" />
                        <asp:HiddenField ID="hfName" runat="server" Value="" />
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
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="User Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" Text="Entity Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                                    Width="100%"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlUserType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="True" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEntity" runat="server" MaxLength="200" SkinID="txt502"></asp:TextBox>
                                <asp:AutoCompleteExtender OnClientItemSelected="OnEntitySelected" FirstRowSelected="true"
                                    ID="acUserType" UseContextKey="true" TargetControlID="txtEntity" runat="server"
                                    ServiceMethod="GetUserType" MinimumPrefixLength="3" CompletionInterval="0" EnableCaching="false"
                                    CompletionSetCount="10">
                                </asp:AutoCompleteExtender>
                                <asp:HiddenField ID="hfEntityID" runat="server" Value="0" OnValueChanged="hfEntityID_ValueChanged" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlUserType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="User ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="User Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Passwrord Expiry Days"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUserId" runat="server" MaxLength="20" SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlExpiryDays" runat="server" SkinID="ddl250">
                            <asp:ListItem Text="Password Never Expires" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="10 Days" Value="10"></asp:ListItem>
                            <asp:ListItem Text="15 Days" Value="15"></asp:ListItem>
                            <asp:ListItem Text="20 Days" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30 Days" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Login Status"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="MobileNumber &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" SkinID="ddl250">
                            <asp:ListItem Selected="True" Text="Disabled" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Enabled" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEmail" Enabled="false" runat="server" oncopy="return false;"
                                    onpaste="return false;" MaxLength="100" SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtMobileNumber" Enabled="false"  runat="server"  MaxLength="10"
                                  oncopy="return false;" onpaste="return false;" SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Default Role &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
 <td style="width: 33%;" valign="top">
                                      <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" 
                            
                            Text="RegionalCentre User Email Address"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">           
                                   <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" 
                            
                            Text="RegionalCentre User MobileNumber"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
               </tr>
               <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                        <asp:DropDownList ID="Ddlrolename" runat="server" SkinID="ddl250" 
                                    onselectedindexchanged="Ddlrolename_SelectedIndexChanged" AutoPostback="true">
                        </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlUserType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
               <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEmailRegCentre" runat="server" oncopy="return false;"
                                    onpaste="return false;" MaxLength="100" SkinID="txt248" Visible="False"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                   </td>
                   <td style="width: 33%;" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtMobileNumberRegCentre"  runat="server"  MaxLength="10"
                                  oncopy="return false;" onpaste="return false;" SkinID="txt248" 
                                    Visible="False"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="hfEntityID" EventName="ValueChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                   </td>
                </tr>
                <tr >
                    <td colspan="3">
                        <asp:updatepanel id="UpdatePanel7" runat="server">
                            <contenttemplate>
                        <table class="sample2" cellpadding="0" cellspacing="0" id="tblShow" runat="server"
                            visible="false">
                            <tr class="heading">
                                <td>
                                    HO Course-Wise Mapping Details
                                </td>
                            </tr>
                            <%--<tr>
                                <td valign="top">
                                    <asp:label id="lblbcc" runat="server" skinid="CaptionLabel" text="Courses &lt;b class='mandatory'&gt;&lt;/b&gt;"
                                        width="100%"></asp:label>
                                </td>
                            </tr>--%>
                            <tr class="even">
                                <td>
                                <div style="width:750px; height:68px; overflow :scroll;">
                                <asp:checkboxlist id="chkcourselist" runat="server" repeatcolumns="2" repeatdirection="Horizontal"
                                        width="100%" cellpadding="0" cellspacing="0" enabletheming="false" 
                                        font-size="8pt">
                                    </asp:checkboxlist>
                                </div>
                                </td>
                            </tr>
                        </table>
                         </contenttemplate>
                            <triggers>
                                <asp:AsyncPostBackTrigger ControlID="Ddlrolename" EventName="SelectedIndexChanged" />
                            </triggers>
                        </asp:updatepanel>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top:8px">
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
                <asp:HyperLink ID="hlUserLog" runat="server" Target="_self">View User Login Log</asp:HyperLink>
                
            </td>
        </tr>
        <tr id="truserRole" runat="server">
            <td>
                <asp:HyperLink ID="hlUserRole" runat="server" Target="_self">User Role</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
