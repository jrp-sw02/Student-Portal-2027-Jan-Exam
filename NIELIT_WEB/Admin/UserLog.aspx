<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="UserLog.aspx.cs" Inherits="UserLog"  Debug="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="User Log"></asp:Label>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">

    <%--<uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />--%>
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
                                            Text="Select Status"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Browser Type"></asp:Label>
                                        <asp:DropDownList ID="ddlBrowser" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>                                        
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Login Status"></asp:Label>
                                        <asp:DropDownList ID="ddlLog" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFrom" Width="100%" runat="server" Text="Login Date From"></asp:Label>
                                        <asp:TextBox ID="txtDateFrom" runat="server"  ></asp:TextBox>
                                        <asp:ImageButton ID="imgFrom" ImageUrl="~/App_Themes/Blue/Images/calendar4.gif" runat="server" ImageAlign="AbsMiddle"  />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom" 
                                        Format="dd-MMM-yyyy" PopupButtonID="imgFrom"></cc1:CalendarExtender>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td> 
                                        <asp:Label ID ="lblTo"  runat="server" Width="100%" Text="Login Date To"></asp:Label><br />
                                        <asp:TextBox ID="txtToDate" runat="server"  ></asp:TextBox>
                                        <asp:ImageButton ID="imgTo" runat="server" ImageUrl="~/App_Themes/Blue/Images/calendar4.gif" ImageAlign="AbsMiddle" />
                                         <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                         Format="dd-MMM-yyyy" PopupButtonID="imgTo"></cc1:CalendarExtender>
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
   <%-- <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />--%>
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
 <%--<ul class="crumbs">
    <li class="first"><a href="Users.aspx" style="z-index:9;"><span></span>Users</a></li>
    <li class="first"><a href="Users.aspx?key=<%=  hfUserID.Value  %>" style="z-index:8;"><span></span><%= Convert.ToString(Request.QueryString["name"])%></a></li>
   <li class="first"><a href="UserLog.aspx" style="z-index:7"><span></span>User Log</a></li>
 </ul>--%>
<uc4:breadcrumb id="BreadCrumb1" runat="server" />
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
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                    OnClick="lbDelteteOne_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" OnSorting="gvMain_Sorting" AutoGenerateColumns="false" 
                               DataKeyNames="ID,OrganizationID" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField   HeaderText="#" ItemStyle-HorizontalAlign="Right" DataField="ID" />
                              
                                <asp:TemplateField HeaderText="Login Time" SortExpression="LoginTime" ItemStyle-HorizontalAlign="Left">
                                   <ItemTemplate>
                                         <%# Convert.ToDateTime(Eval("LoginTime")).ToString("dd-MMM-yyyy hh:mm:ss")%>
                                   </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BrowserName" HeaderText="Browser" SortExpression="BrowserName" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ClientIP" HeaderText="Client IP" SortExpression="ClientIP" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="SourceIP" HeaderText="Source IP" SortExpression="SourceIP" ItemStyle-HorizontalAlign="Left" />
                                
                                 <asp:TemplateField HeaderText="Logout Time" SortExpression="LogoutTime" ItemStyle-HorizontalAlign="Left">
                                     <ItemTemplate>
                                         <%# Convert.ToDateTime(Eval("LogoutTime")).ToString("dd-MMM-yyyy hh:mm:ss")%>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                              
                                <asp:TemplateField HeaderText="Status" SortExpression="LoginResponseValue">
                                    <ItemTemplate>
                                        <%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.URM.AuthenticationResponse)Eval("LoginResponseValue"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ToolTip="Action" ClientIDMode="Static" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderStyle-Width="3%" HeaderText="">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfUserID" runat="server" Value="" />
                        <asp:HiddenField ID="hfName"  runat="server" Value=""/>
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
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
