<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AdminCourses.aspx.cs" Inherits="Admin_AdminCourses" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Courses"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="false" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
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
                                        <asp:Label ID="lblUserType" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcname" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by course name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() 
        {

            if (document.getElementById("<%=ddlapptype.ClientID %>").disabled == false) 
            {
                if (!isSelected("<%=ddlapptype.ClientID %>", "Candidate Type"))
                    return false;
            }
  
            return true;

        }
        function Validate(msg)
         {
                if (ConfirmAction(msg))
                    return true;
                else
                    return false;
            }
        
    </script>
    <style type="text/css">  
          .PromptCSS  
        {  
            color:Blue;  
            font-size:10pt;
            font-style:italic;  
            font-weight:bold;  
            font-family:Courier New;  
            border:solid 1px Pink;  
            vertical-align:bottom;
            height:12px;
            padding-bottom:0px;
            }   
    </style>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#"><HeaderStyle Width="2%" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" HeaderText="Course" DataNavigateUrlFields="ID,appid,CourseID,CandId,StatusId,RegID"
                                    DataNavigateUrlFormatString="?key={0}&key1={1}&CourseID={2}&CandId={3}&StatusId={4}&RegID={5}"
                                    DataTextField="Course" SortExpression="Course" Target="_self"><HeaderStyle Width="15%" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Registration Date" HeaderStyle-Width="20%" DataTextField="regdate"
                                    DataNavigateUrlFields="ID,appid,CourseID,CandId,StatusId,RegID" DataNavigateUrlFormatString="?key={0}&key1={1}&CourseID={2}&CandId={3}&StatusId={4}&RegID={5}"
                                    SortExpression="regdate" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Right" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Valid Upto" HeaderStyle-Width="20%" DataTextField="validdate"
                                    DataNavigateUrlFields="ID,appid,CourseID,CandId,StatusId,RegID" DataNavigateUrlFormatString="?key={0}&key1={1}&CourseID={2}&CandId={3}&StatusId={4}&RegID={5}"
                                    SortExpression="validdate" DataTextFormatString="{0:dd-MMM-yyyy}"><HeaderStyle Width="20%" /><ItemStyle HorizontalAlign="Right" /></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Status." HeaderStyle-Width="20%" DataTextField="status"
                                    DataNavigateUrlFields="ID,appid,CourseID,CandId,StatusId,RegID" DataNavigateUrlFormatString="?key={0}&key1={1}&CourseID={2}&CandId={3}&StatusId={4}&RegID={5}"
                                    SortExpression="status"><HeaderStyle Width="20%" /></asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
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
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Course Name. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Candidate Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lbtxtcname" runat="server" SkinID="txt248"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lbname" runat="server" Text="" SkinID="txt248"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlstatus" runat="server" SkinID="ddl250"
                            onselectedindexchanged="ddlstatus_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList> 
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lblregdate" runat="server" SkinID="CaptionLabel" Text="Registration Date.&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lbcommendate" runat="server" SkinID="CaptionLabel" Text="Commencement Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Lbl_effDate" runat="server" Text="Valid Upto.&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="lbtxtregdate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="lbcommencementDate" runat="server" SkinID="txt210" Visible="false" MaxLength="11"></asp:TextBox>
                        <asp:TextBox ID="lbregno" runat="server" SkinID="txt248" Visible="false" MaxLength="16"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="lbtxtvaliddate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Candidate Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Institute Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr  class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlapptype" runat="server" EnableTheming="True" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlapptype_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td  valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlinstitute" runat="server" EnableTheming="True" SkinID="ddl504">
                                </asp:DropDownList>
                                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlinstitute"
                                    PromptText="Type institute name to search from the institute list"
                                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS"></asp:ListSearchExtender>
                            </ContentTemplate>
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlapptype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div style="margin-top: 10px" id="divinfo" visible="false" runat="server" class="error">
                <table cellpadding="3" cellspacing="0" width="100%">
                    <tr class="gdalternate1">
                        <td valign="top">
                            <asp:Label ID="lbfilter" runat="server" Text="" Style="text-align: right;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="SaveRecord" OnClientClick="return ValidateFormFields();" />
                <asp:Button ID="btnapplicanttype" runat="server" Text="Change Applicant Type" OnClick="btnapplicanttype_Click"
                    OnClientClick="return Validate('Are you sure you want to change the applicant type of the candidate!')" />
                <asp:Button ID="btninstitute" runat="server" Text="Change Institute" OnClick="btninstitute_Click"
                    OnClientClick="return Validate('Are you sure you want to change the institute of the candidate!')" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="Btnback" runat="server" Text="Cancel" onclick="Btnback_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc5:SideLink ID="SideLink1" runat="server" />
</asp:Content>
