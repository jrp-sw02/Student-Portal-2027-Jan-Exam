<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ConfigurationManagement.aspx.cs" Inherits="ConfigurationManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Configuration Management"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        var dtgp = "<%= gvMain.ClientID %>"
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <table width="100%">
            <tr>
            <td align="left">
                <asp:RadioButtonList ID="rblConfigurationtype" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="rblConfigurationtype_SelectedIndexChanged" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="1">Payment Mode Configuration</asp:ListItem>
                    <asp:ListItem Value="2">Notification Message Configuration</asp:ListItem>
                    <asp:listitem value="3">Fields Updatable Configuration</asp:listitem>
                </asp:RadioButtonList>
            </td>
            <td align="right">
                <asp:Button ID="btnSave" runat="server" Text="Update" OnClick="btnSave_Click" />
            </td>    
            </tr>
            </table>
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                        OnRowDataBound="gvMain_RowDataBound" Width="100%">
                                        <%--onselectedindexchanged="gvMain_SelectedIndexChanged"--%>
                                        <Columns>
                                            <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                                <HeaderStyle Width="2%" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Payment Option" DataField="Name">
                                                <HeaderStyle Width="30%" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--<asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,Appno" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                                DataTextField="" HeaderText="Payment Option" SortExpression=""
                                                Target="_self">
                                                <HeaderStyle Width="12%" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:HyperLinkField>--%>
                                            <asp:TemplateField HeaderStyle-Width="22%" HeaderText="">
                                                <HeaderTemplate>
                                                    <%--<asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />--%>
                                                    <asp:Label ID="lblShow" runat="server" Text="Update"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkupdate" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="22%" HeaderText="">
                                                <HeaderTemplate>
                                                    <%--<asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />--%>
                                                    <asp:Label ID="lblShow" runat="server" Text="Show"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkShow" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="23%" HeaderText="">
                                                <HeaderTemplate>
                                                    <%--<asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />--%>
                                                    <asp:Label ID="lblShow" runat="server" Text="SMS"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chksms" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="23%" HeaderText="">
                                                <HeaderTemplate>
                                                    <%--<asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />--%>
                                                    <asp:Label ID="lblShow" runat="server" Text="E-Mail"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkEmail" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
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
            <div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
