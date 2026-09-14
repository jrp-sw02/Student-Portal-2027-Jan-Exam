<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="GeographicalLocation.aspx.cs" Inherits="GeographicalLocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Geographical Location"></asp:Label>
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Location Type"></asp:Label>
                                        <asp:DropDownList ID="ddlFLocType" Width="100%" runat="server">
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
            if (!isSelected("<%=ddlLocationType.ClientID %>", "Location Type"))
                return false;
            if (!isBlank("<%=txtName.ClientID %>", "Name"))
                return false;
            if (!isBlank("<%=txtCode.ClientID %>", "Code"))
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
            if (!isSelected("<%=ddlLocationType.ClientID %>", "Location Type"))
                return false;
            if (document.getElementById("<%=ddlLocationType.ClientID %>").value == "1") {
                alert("Selected Location type does not have any parent location name");
                return false;
            }
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
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction" CommandName="Delete"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="false" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="3%" HeaderText="#" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="name" HeaderText="Location Name" SortExpression="name" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="locationType" HeaderText="Location Type" SortExpression="locationType"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="parentType" HeaderText="Parent Type" SortExpression="parentType"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="parentTypeName" HeaderText="Parent Name" SortExpression="parentTypeName"
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
                        <asp:Label ID="lblLocationType" runat="server" Text="Location Type <b class='mandatory'>*</b>"
                            SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="lblName" runat="server" SkinID="CaptionLabel" Text="Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCodeName" runat="server" Text="Name *" SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlLocationType" runat="server" AutoPostBack="True" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlLocationType_SelectedIndexChanged" TabIndex="1">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="100" SkinID="txt248" TabIndex="2" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtCode" runat="server" SkinID="txt248" MaxLength="10" TabIndex="3" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblParentLocName" runat="server" Text="Parent Location Name" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                            <ContentTemplate>
                                <asp:Label ID="lblTypeName" runat="server" Text="Type Name" SkinID="CaptionLabel"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlLocationType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <asp:TextBox SkinID="txt210" ID="txtParent" Style="cursor: pointer" onkeypress="return false;"
                                    onkeydown="return false;" runat="server"  TabIndex="4"></asp:TextBox>
                                <asp:HiddenField ID="hfParentId" Value="" runat="server" />
                                <asp:Button Width="0px" Height="0px" ID="btnPopup" runat="server" Style="display: none;" />
                                <asp:ImageButton ID="imgPopup" OnClientClick="return CheckSelected();" ClientIDMode="Static"
                                    ToolTip="Search Parent Location Name" ImageUrl="~/App_Themes/Blue/Images/search_button_02.png"
                                    ImageAlign="Middle" Style="margin-left: 3px; border: none 0px; height: 17px;"
                                    runat="server" OnClick="imgPopup_Click" />
                                <asp:ModalPopupExtender OnOkScript="CheckChecked();" ClientIDMode="Static" TargetControlID="btnPopup"
                                    BackgroundCssClass="modalBackground" ID="ModalPopupExtender1" runat="server"
                                    CancelControlID="btnCancel" OkControlID="btnOk" PopupControlID="pnlObjects" RepositionMode="RepositionOnWindowScroll"
                                    ViewStateMode="Enabled">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="pnlObjects" runat="server" widht="700px" Height="350px" ScrollBars="Auto"
                                    BorderStyle="Solid" BorderWidth="1px" BorderColor="Navy" BackColor="White">
                                    <table cellpadding="1" cellspacing="0" width="600px" class="sample3">
                                        <tr class="head1">
                                            <td colspan="2" align="left" valign="top" style="color: White">
                                                Parent Object Browser: Please select parent location name from the list below.
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td width="50%" align="left">
                                            </td>
                                            <td width="50%" align="left">
                                            </td>
                                        </tr>--%>
                                        <tr class="sample2">
                                            <td colspan="2" align="center" valign="top">
                                                <div style="overflow: auto; height: 290px; width: 95%; text-align: left;">
                                                    <asp:TreeView ID="tvParents" Width="100%" runat="server" 
                                                        OnSelectedNodeChanged="tvParents_SelectedNodeChanged" TabIndex="5">
                                                    </asp:TreeView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr class="sample2">
                                            <td colspan="2" align="right" valign="top">
                                                <asp:Button ID="btnOk" Style="display: none;" runat="server" Text="Select" /><asp:Button
                                                    ID="btnCancel" runat="server" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlLocationType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtTypeName" runat="server" MaxLength="100" SkinID="txt248" 
                            TabIndex="6" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
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
                <asp:Button ID="Button1" runat="server" Text="Cancel" OnClick="btnCancel_Click" 
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
