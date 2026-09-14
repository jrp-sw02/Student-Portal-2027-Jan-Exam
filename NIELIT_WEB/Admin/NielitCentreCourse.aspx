<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="NielitCentreCourse.aspx.cs" Inherits="Admin_NielitCentreCourse"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Courses"></asp:Label>
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
                                        <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseCategoryName" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCourseCategoryName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourses" Width="100%" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlCourses_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategoryName" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Verified Status" Visible="true"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourseVerifiedStatus" runat="server" Width="100%" Visible="true">
                                                    <asp:ListItem Value="99">--All--</asp:ListItem>
                                                    <asp:ListItem Value="1">YES</asp:ListItem>
                                                    <asp:ListItem Value="0">NO</asp:ListItem>
                                                    <asp:ListItem Value="-1">Pending</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlCourseName" runat="server" Width="100%" Visible="False">
                                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCourses" EventName="SelectedIndexChanged" />
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by course name"
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
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlNielitTrgSpecialization.ClientID %>", "NIELIT Training Specialization"))
                return false;
            if (!isBlank("<%=txtcoursename.ClientID  %>", "Course Name"))
                return false;
            if (!isBlank("<%=txtcoursecode.ClientID  %>", "Course Code "))
                return false;
            if (!isBlank("<%=txtdisplay.ClientID  %>", "Display Order"))
                return false;
            if (!isSelected("<%=ddlwhetherShortTerm.ClientID  %>", "Whether ShortTerm"))
                return false;
            if (!isBlank("<%=TxtRemarks.ClientID  %>", "Remarks"))
                return false;
            if (!isSelected("<%=ddlShowOnWeb.ClientID  %>", "Show on Web"))
                return false;

            return true;

        }

        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1]
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
                                </td>
                            </tr>
                        </table>
                         <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblErrMsg" Visible="false"
                runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CategoryName" HeaderText="CourseCategory Name" SortExpression="CategoryName" Target="_self">
                                    <HeaderStyle Width="15%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Course Name" SortExpression="Name" Target="_self">
                                    <HeaderStyle Width="8%" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Code" HeaderText="CourseCode" SortExpression="Code" Target="_self">
                                    <HeaderStyle Width="8%" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="13%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="NielitTrgSplID" HeaderText="NielitTrgSplID" SortExpression="NielitTrgSplID"
                                    Target="_self">
                                    <HeaderStyle Width="13%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="8%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="DisplayOrder" HeaderText="Display Order" SortExpression="DisplayOrder"
                                    Target="_self">
                                    <HeaderStyle Width="8%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="18%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="WhetherShortTerm" HeaderText="WhetherShortTerm" SortExpression="WhetherShortTerm"
                                    Target="_self">
                                    <HeaderStyle Width="18%" />
                                </asp:HyperLinkField>
                                 
                     
                                <asp:HyperLinkField HeaderStyle-Width="9%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Verified" HeaderText="Verified" SortExpression="Verified"
                                    Target="_self">
                                    <HeaderStyle Width="9%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="22%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="VerifiedOn" HeaderText="VerifiedOn" SortExpression="VerifiedOn"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="22%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="9%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="VerifiedStatus" HeaderText="VerifiedStatus" SortExpression="Verified"
                                    Target="_self">
                                    <HeaderStyle Width="9%" />
                                </asp:HyperLinkField>
                                 <asp:TemplateField Visible="false" HeaderText="lblid">

                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Visible="false" ID="lblIDC" Text='<%# Eval("IDC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                            
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccID" runat="server" />
                        <asp:HiddenField ID="hddnielitId" runat="server" />  
                        <asp:HiddenField ID="HDDFORIDC" runat="server" />                       
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" CurrentPageSize="200" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="NIELIT Training Specialization &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">

                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">

                    <td valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlNielitTrgSpecialization" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlNielitTrgSpecialization_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlNielitTrgSpecialization" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">

                        <asp:TextBox ID="txtcoursename" runat="server" MaxLength="150" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" Text="Course Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Display Order &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="LblwhetherShortTerm" runat="server" SkinID="CaptionLabel" Text="whetherShortTerm &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtcoursecode" runat="server" MaxLength="15" SkinID="txt248" ToolTip="Code No."></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdisplay" runat="server" Enabled="False" MaxLength="2" onkeypress="checkNumber(this,2,0,event);" SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlwhetherShortTerm" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <tr>
                        <td style="width: 33%;" valign="top" colspan="3">
                            <asp:Label ID="LbRemarks" runat="server" SkinID="CaptionLabel" Text="Remarks &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                        </td>                       
                    </tr>
                    <tr class="even">
                        <td style="width: 33%;" valign="top" colspan="3">

                            <asp:TextBox ID="TxtRemarks" runat="server" BackColor="White" MaxLength="150" SkinID="txt756"></asp:TextBox>
                        </td>                      
                    </tr>
                                        <tr  id="Rview2" runat="server">
                        
                        <td style="width: 33%;" valign="top">
                            <asp:Label ID="lblverifiedStatus" runat="server" SkinID="CaptionLabel" Text="Verified Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>

                        </td>
                        <td style="width: 33%;" valign="top">
                            <asp:Label ID="lblverificationMessage" runat="server" SkinID="CaptionLabel" Text="Verification Message &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>

                        </td>
			<td style="width: 33%;" valign="top" >
                            <asp:Label ID="lblRequestFrom" runat="server" SkinID="CaptionLabel" Text="Request From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>

                        </td>
                    </tr>
                     <tr class="even"  id="Rview3" runat="server">                        
                        <td style="width: 33%;" valign="top">                           
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlverifiedStatus" runat="server" SkinID="ddl250" Enabled="false" >                                   
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td style="width: 33%;" valign="top">
                            <asp:TextBox ID="TxtverificationMessage" runat="server" BackColor="White" MaxLength="500" SkinID="txt502" Enabled="false"></asp:TextBox>
                        </td>
 <td style="width: 33%;" valign="top" >
                            <asp:TextBox ID="TxtRequestFrom" runat="server" BackColor="White" MaxLength="150" SkinID="txt248" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Rview" runat="server">
                        <td style="width: 33%;" valign="top">
                            <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Display On Web"></asp:Label>
                        </td>
                        <td style="width: 33%;" valign="top">
                            <asp:Label ID="LbIsActive" runat="server" SkinID="CaptionLabel" Text="IsActive "></asp:Label>
                        </td>
                        <td style="width: 33%;" valign="top">
                            
                            <asp:Label ID="LbIsVerified" runat="server" SkinID="CaptionLabel" Text="IsVerified " Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Rview1" runat="server" class="even">
                        <td style="width: 33%;" valign="top">
                            <asp:DropDownList ID="ddlShowOnWeb" runat="server" SkinID="ddl250">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 33%;" valign="top">
                             <asp:DropDownList ID="ddlIsActive" runat="server" SkinID="ddl250">
                                <asp:ListItem>---Select One---</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">No</asp:ListItem>
                            </asp:DropDownList>
                           
                        </td>
                        <td style="width: 33%;" valign="top">
                             <asp:DropDownList ID="ddlIsVerified" runat="server" SkinID="ddl250" Visible="false">
                                <asp:ListItem>---Select One---</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">No</asp:ListItem>
                            </asp:DropDownList>
                           
                        </td>
                    </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecordNielitCourse" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnback" runat="server" Text="Back" TabIndex="54" OnClick="btnback_Click" Visible="False" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>