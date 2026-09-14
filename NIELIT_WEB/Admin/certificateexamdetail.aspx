<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="certificateexamdetail.aspx.cs" Inherits="certificateexamdetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Time Table"></asp:Label>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
<ul class="crumbs">
	<li class="first"><a href="admincertiexams.aspx" style="z-index:9;"><span></span>Certification Exams</a></li>
	<li><a href="admincertiexams.aspx?key=S101&category=S/W&Course=BCC" style="z-index:8;"><span></span>BCC(S/W)</a></li>
    <li><a href="admcertiexamdetail.aspx" style="z-index:7;"><span></span>Exam Details</a></li>
    <li><a href="admcertiexamdetail.aspx?key=January" style="z-index:6;"><span></span>January</a></li>
    <li><a href="admexamdetail.aspx?key=January 2010" style="z-index:5;"><span></span>Exam Session</a></li>
    <li><a href="#" style="z-index:4;"><span></span>Time Table</a></li>

</ul>  
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
                                    <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                        runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                        CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                        runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                        SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="UserID,OrganizationID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" Visible="False">
                            <Columns>
                                <asp:BoundField   HeaderText="#" />
                                <asp:HyperLinkField DataNavigateUrlFields="UserID,OrganizationID" DataNavigateUrlFormatString="?Key={0}&OrgId={1}"
                                    DataTextField="LoginID" HeaderText="Login ID" SortExpression="LoginID"
                                    Target="_self" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:TemplateField HeaderText="User Type" SortExpression="UserType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" Text='<%# EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.URM.UserType)Eval("UserType"))  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pwd Expiry Date" SortExpression="PasswordExpiryDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("PasswordExpiryDate")).ToString("dd-MMM-yyyy")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Login Status" SortExpression="HasLoginAccess">
                                    <ItemTemplate>
                                        <asp:Label ID="lbStatus" runat="server" Text='<%# Convert.ToBoolean(Eval("HasLoginAccess").ToString())==true? "Enabled": "Disabled" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
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
                        <table class="gdbody" cellspacing="1" cellpadding="4" id="cphContents_gvMain" style="width: 99%;">
                                <tr class="gdheader">
                                    <td align="left" class="style1">
                                        #</td>
                                    <td align="left">
                                        <b>Exam</b></td>
                                    <td align="left">
                                        <b>&nbsp;Date of Exam </b></td>
                                </tr>
                                <tr class="gdrow">
                                    <td align="left" class="style1">
                                        1</td>
                                    <td align="left">
                                        Exam1
                                        </td>
                                    <td align="left">
                                        01/01/2009</td>
                                </tr>
                                <tr class="gdalternate">
                                    <td align="left" class="style1">
                                        2</td>
                                    <td align="left">
                                         Exam2
                                    </td>
                                    <td align="left">
                                        02/01/2009</td>
                                </tr>
                                <tr class="gdrow">
                                    <td align="left" class="style1">
                                        3
                                        </td>
                                    <td align="left">
                                         Exam3
                                    </td>
                                    <td align="left">
                                        03/01/2009</td>
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
                        <uc3:PagingBar ID="PagingBar1" runat="server"  
                            OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server"><%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing ="0">
                <tr>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption" runat="server" SkinID="CaptionLabel" 
                            Text="Category &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="100%"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" 
                            Text="Certificate Course &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" 
                            Text="Exam Month &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width:33%;" valign="top">
                        <asp:DropDownList ID="ddlEntity1" runat="server" onchange="SetNames(this);" 
                            SkinID="ddl250">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                            <asp:ListItem>H/W</asp:ListItem>
                            <asp:ListItem>S/W</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:DropDownList ID="ddlEntity0" runat="server" onchange="SetNames(this);" 
                            SkinID="ddl250">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                            <asp:ListItem>BCC</asp:ListItem>
                            <asp:ListItem>CCC</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:DropDownList ID="ddlEntity2" runat="server" onchange="SetNames(this);" 
                            SkinID="ddl250">
                            <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                            <asp:ListItem>January</asp:ListItem>
                            <asp:ListItem>February</asp:ListItem>
                            <asp:ListItem>March</asp:ListItem>
                            <asp:ListItem>Aprial</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width:33%;" valign="top">
                        Exam Year</td>
                    <td style="width:33%;" valign="top">
                        Exam Name</td>
                    <td style="width:33%;" valign="top">
                        Date</td>
                </tr>
                <tr class="even">
                    <td style="width:33%;" valign="top">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" SkinID="txt210"></asp:TextBox>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:TextBox ID="txtEmail0" runat="server" MaxLength="100" SkinID="txt210"></asp:TextBox>
                    </td>
                    <td style="width:33%;" valign="top">
                        <asp:TextBox ID="txtEmail1" runat="server" MaxLength="100" SkinID="txt210"></asp:TextBox>
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