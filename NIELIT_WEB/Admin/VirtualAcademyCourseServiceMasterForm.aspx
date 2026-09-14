<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="VirtualAcademyCourseServiceMasterForm.aspx.cs" Inherits="Admin_VirtualAcademyCourseServiceMasterForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Virtual Academy Course Service"></asp:Label>
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
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursecategoryF" Width="100%" runat="server" AutoPostBack="True"
                                             OnSelectedIndexChanged="ddlcoursecategoryF_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                      
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td> 
                                        <asp:Label ID="lblcourseNameF" Width="100%" runat="server" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                        <asp:DropDownList ID="ddlCourseNameF" Width="100%" runat="server"  AutoPostBack="True">                                          
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                                </ContentTemplate>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Activity Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (document.getElementById("<%=Ddlccat.ClientID %>").disabled == false) {
                if (!isSelected("<%=Ddlccat.ClientID %>", "Course Category"))
                    return false;
            }
            if (document.getElementById("<%=Ddlcname.ClientID %>").disabled == false) {
                if (!isSelected("<%=Ddlcname.ClientID %>", "Course Name"))
                    return false;
            }
            if (document.getElementById("<%= ddlServiceID.ClientID %>").disabled == false) {
                if (!isSelected("<%= ddlServiceID.ClientID %>", "Course Service ID"))
                    return false;
            }
            if (!isBlank("<%=txtServiceId.ClientID  %>", "Registration Service ID"))
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
                        </table>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" 
                            Width="100%">
                            <Columns>
                               <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID" DataNavigateUrlFormatString="?Key={1}&CourseID={0}"
                                    DataTextField="coursecatName" HeaderText="Course Category Name" SortExpression="Name"
                                    Target="_self" >
                                <ItemStyle Width="13%" HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID" DataNavigateUrlFormatString="?Key={1}&CourseID={0}"
                                    DataTextField="CourseName" HeaderText="Course Name" SortExpression="CourseName" Target="_self" >
                                     <ItemStyle Width="13%" HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID" DataNavigateUrlFormatString="?Key={1}&CourseID={0}"
                                    DataTextField="Registration_ServiceID" HeaderText="Registration ServiceID" SortExpression="Registration_ServiceID" Target="_self">
                                     <ItemStyle Width="13%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                             
                              <%-- <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID" DataNavigateUrlFormatString="?Key={1}&CourseID={0}"
                                    DataTextField="Examination_ServiceID" HeaderText="Examination ServiceID" SortExpression="Examination_ServiceID" Target="_self">
                                     <ItemStyle Width="13%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>--%>
                                <%-- <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif" Visible="false">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>--%>
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
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Service ID&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="Ddlccat" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="Ddlccat_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="Ddlcname" runat="server" SkinID="ddl250" 
                                    AutoPostBack="True" OnSelectedIndexChanged="Ddlcname_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Ddlccat" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlServiceID" runat="server" SkinID="ddl250" 
                                    AutoPostBack="true"  OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">Registration</asp:ListItem>
                                    <asp:ListItem Value="2">Examination</asp:ListItem>
                                   <%-- <asp:ListItem Value="3">Certificate</asp:ListItem>
                                    <asp:ListItem Value="4">Project</asp:ListItem>--%>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Ddlcname" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
               <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>--%>
                <tr id="serviceid"  >
                    <td colspan="3" valign="top" style="width: 33%;">  
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>         
                        <asp:Label ID="lblservice" runat="server" SkinID="CaptionLabel" Text="Registration Service ID&lt;b class='mandatory'&gt;*&lt;/b&gt;" ></asp:Label>                                
                          </ContentTemplate>   </asp:UpdatePanel>
                    </td>
                    <%--<td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Reg Start Date&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>

                     <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Effective Date&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>--%>
                </tr>
                <tr id="serviceid1" runat="server" class="even" >
                    <td colspan="3" valign="top" style="width: 33%;" >
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate> 
                        <asp:TextBox ID="txtServiceId" runat="server" Width="100%" MaxLength="20"  SkinID="txt248"
                                                    onpaste="return false;" oncopy="return false;" oncut="return false;" BackColor="#ffffcc"  style="text-transform:uppercase; ">
                                                </asp:TextBox>
                                </ContentTemplate>   </asp:UpdatePanel>
                    </td> 
                </tr>
                   <%--  </ContentTemplate> </asp:UpdatePanel>  --%>        
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
