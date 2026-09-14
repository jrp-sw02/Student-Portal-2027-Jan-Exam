<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SemesterMaster.aspx.cs"
    Inherits="SemesterMaster" MasterPageFile="~/MasterPages/main.master"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Semester Master"></asp:Label>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
           <%-- //Filter--%>
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseName" Width="100%" runat="server"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Batch Name"></asp:Label>
                                        <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlbatchname_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <%-- <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Verified Status" Visible="true"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlBatchVerifiedStatus" runat="server" Width="100%" Visible="true">
                                                    <asp:ListItem Value="99">--All--</asp:ListItem>
                                                    <asp:ListItem Value="1">YES</asp:ListItem>
                                                    <asp:ListItem Value="0">NO</asp:ListItem>                                                   
                                                </asp:DropDownList>                                                
                                            </ContentTemplate>                                            
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>--%>
                                <tr>
                                   <td></td>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Batch code"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
          
            if (!isSelected("<%=ddlCourse.ClientID  %>", "Course Name"))
                return false;

            if (!isSelected("<%=ddlbatchSession.ClientID  %>", "Batch Name"))
                return false;

            if (!isBlank("<%=txtSems.ClientID %>", "No. Of Sem"))
                return false;
            
            
           
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
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                    <%--<asp:LinkButton ID="lbdisable" OnClientClick="return ConfirmAction('Are you sure you want to change the status of the selected exam center!');"
                                        runat="server" Text="Change Status" ToolTip="click to change status of this record" 
                                        SkinID="lnkbtnAction" onclick="lbdisable_Click"></asp:LinkButton>--%>
                                </td>
                            </tr>
                        </table>

                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="Id" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                                         
                                <asp:HyperLinkField HeaderStyle-Width="50%" DataNavigateUrlFields="Id,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="CourseName"
                                    HeaderText="Course Name" SortExpression="CourseName" Target="_self">
                                <HeaderStyle Width="50%" />
                                      <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="Id,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="BatchCode"
                                    HeaderText="Batch Code" SortExpression="BatchCode" Target="_self">
                                <HeaderStyle Width="20%"/>
                                      <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="Id,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="NoOfSems"
                                    HeaderText="No of Sems" SortExpression="NoOfSems" Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="Id,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="CourseId"
                                    HeaderText="CourseID" Visible="false" SortExpression="CourseId" Target="_self">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:TemplateField Visible="false" HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
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
                                </asp:TemplateField>
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
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
      <%--  //Insert--%>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="33%">
                
              <tr>
                    <td style="width: 33%;" valign="top">
                         <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblcourse0" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="20%"></asp:Label>
                                <asp:Label ID="lblerrorddlcouse" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Batch &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                       <asp:Label ID="lblerrorddlbatch" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>

                    </td>
                    <td style="width: 33%;" valign="top">
                         <asp:Label ID="lblSems" runat="server" SkinID="CaptionLabel" Text="No of Sems &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>                     

                    </td>
                </tr>
                  
              <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="True" Width="200px" SkinID="ddl760" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                   <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                       <asp:DropDownList ID="ddlbatchSession"  runat="server" SkinID="ddl250">
                            <%--<asp:ListItem Value="0" Text="--All--"></asp:ListItem>--%>
                        </asp:DropDownList>
                                </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                   <td style="width: 33%;" valign="top">
                       <asp:TextBox ID="txtSems" runat="server" MaxLength="100" SkinID="txt248" TabIndex="2" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                   </tr>

               <tr id="trsem" runat="server" visible="false">
                    <td></td>
                    <td></td>
                    <td>
  <asp:Label ID="lblerrorsem" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="100%" visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
           
                
            
            <div style="text-align: right; margin-top: 10px">

                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="NIELITCentreId" runat="server" />
                    <asp:HiddenField ID="HNANFL" runat="server" />
                    <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>
