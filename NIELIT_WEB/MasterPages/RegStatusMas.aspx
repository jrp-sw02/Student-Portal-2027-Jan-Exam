<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="RegStatusMas.aspx.cs" Inherits="HO_RegStatusMas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Reg Status Master"></asp:Label>
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
                                            Text="Select Reg Status Name"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRegStatusName" Width="100%" runat="server" Text="Reg Status Name"></asp:Label>
                                        <asp:DropDownList ID="ddlRegName" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Reg Status Name"
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
            
            if (!isBlank("<%=txtRegSt.ClientID%>", "Reg Status Name"))
                return false;
           <%-- if (isNumber("<%=txtRegSt.ClientID %>", "characters are  allowed"))
                return false;--%>
            if (!isBlank("<%=txtRegDesc.ClientID%>", "Reg Discription"))
                return false;
           <%-- if (isNumber("<%=txtRegDesc.ClientID %>", "characters are  allowed"))
                return false;--%>
           
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
                <asp:UpdatePanel ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <%-- <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
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
                        </table>--%>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                            OnSorting="gvMain_Sorting" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="#" ItemStyle-HorizontalAlign="Right" DataField="ID" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?key={0}"
                                    DataTextField="reqStatusName" HeaderText="Reg Status Name" SortExpression="reqStatusName"
                                    Target="_self" />
                               
                                <asp:TemplateField HeaderText="Status Desc" SortExpression="reqStatusDesc" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a href="Address.aspx?key=<%#Eval("ID")%>"><span></span>
                                            <%# DataBinder.Eval(Container.DataItem, "reqStatusDesc")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Enter Date" SortExpression="enterDate" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <a href="Address.aspx?key=<%#Eval("ID")%>"><span></span>
                                            <%# Convert.ToDateTime(Eval("enterDate")).ToString("dd-MMM-yyyy")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
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
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblRegStatus" runat="server" SkinID="CaptionLabel" Text="Reg Status Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblRegDscp" runat="server" SkinID="CaptionLabel" Text="Reg Description &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label></td>
                    <td style="width: 33%;" valign="top">
                        Enter Date</td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                 <ContentTemplate>
                        <asp:TextBox ID="txtRegSt" runat="server" MaxLength="50"  SkinID="txt210"></asp:TextBox>
                         </ContentTemplate></asp:UpdatePanel>
                    </td>
                    
                    <td style="width: 33%;" valign="top">
                         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                 <ContentTemplate>
                         <asp:TextBox ID="txtRegDesc" runat="server" MaxLength="50" SkinID="txt248"></asp:TextBox>
                                  </ContentTemplate>   </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEntDt" Enabled="false" runat="server" SkinID="txt210"></asp:TextBox>
                         
                    </td>
                </tr>
               
                
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="sidetable" cellspacing="1"
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
    </table>
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
