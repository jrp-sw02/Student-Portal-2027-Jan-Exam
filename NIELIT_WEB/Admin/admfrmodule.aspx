<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="admfrmodule.aspx.cs" Inherits="admfrmodule" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Modules"></asp:Label>
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Module Type"></asp:Label>
                                        <asp:DropDownList ID="ddlfiltermoduletype" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Selection Type"></asp:Label>
                                        <asp:DropDownList ID="ddlfilterselection" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Revision Number"></asp:Label>
                                        <asp:DropDownList ID="ddlfilterRevisionNo" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search By Module Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function changetext() {

            if (document.getElementById('<%=txtmoduleno.ClientID %>').value == "") {
                document.getElementById('<%=ddlselectiontype.ClientID %>').value = "0";
                 document.getElementById('<%=txtElectiveGroup.ClientID %>').value = "";
                document.getElementById('<%=ddlselectiontype.ClientID %>').disabled = true;
            }
            else {
                
                document.getElementById('<%=ddlselectiontype.ClientID %>').disabled = false;
            }
            }
       
        function ValidateFormFileds() {

            if (!isSelected("<%=ddlmoduletype.ClientID %>", "Module Type Name"))
                return false;
            if (!isBlank("<%=txtmodulecode.ClientID %>", "Module Name"))
                return false;
            if (!isNumber("<%=txtmodulecode.ClientID %>", "Module Name"))
                return false;
            if (!isBlank("<%=txtname.ClientID %>", "Module Name"))
                return false;
            if (!isBlank("<%=txtsubname.ClientID %>", "Module Sub Name"))
                return false;
            if(document.getElementById("<%=ddlmoduletype.ClientID %>").value ==<% =Convert.ToInt32(EConnect.enmModuleType.Project) %>)
            
            if (document.getElementById("<%=ddlProjectNo.ClientID %>").disabled ==false) 
            {
                if (!isSelected("<%=ddlProjectNo.ClientID %>", "Project Number"))
                    return false;
            }
            if (document.getElementById("<%=ddlSynopsis.ClientID %>").disabled ==false) 
            {
              if (!isSelected("<%=ddlSynopsis.ClientID %>", "Synopsis"))
                return false;
            }
            if (!isBlankNumber("<%=txtmoduleno.ClientID %>", "Module Number"))
                return false;
            if (!isNumber("<%=txtmoduleno.ClientID %>"))
                return false;
            if (!isSelected("<%=ddlselectiontype.ClientID %>", "Selection Type"))
                return false;
            var electiveGroup=document.getElementById("<%=txtElectiveGroup.ClientID %>").value;
            if(electiveGroup!="")
            {
                 if (!isNumber("<%=txtElectiveGroup.ClientID %>"))
                return false;
                if (!isBlankNumber("<%=txtsubmoduleno.ClientID %>", "Elective Module Number"))
                return false;
                 if (!isNumber("<%=txtsubmoduleno.ClientID %>"))
                return false;
                if (!isBlankNumber("<%=txtallowedmodules.ClientID %>", "Allowed No of Modules"))
                return false;
                if (!isNumber("<%=txtallowedmodules.ClientID %>", "Allowed No of Modules"))
                return false;
            }
            if (!isBlankNumber("<%=txtSubCodeNumber.ClientID %>", "Module Sub Code"))
                return false;
            if (!isBlankDate("<%=txteffectivefrom.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txteffectivefrom.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=ddlExamMode.ClientID %>", "Select Exam Mode"))
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
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                 <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID,CourseID,RevNo"
                                     DataNavigateUrlFormatString="?Key={0}&CourseId={1}&Revision={2}" DataTextField="SName"
                                     HeaderText="Short Name" SortExpression="SName" Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID,CourseID,RevNo"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}&Revision={2}" DataTextField="Name"
                                    HeaderText="Name" SortExpression="Name" Target="_self">
                                    <HeaderStyle Width="55%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID,CourseID,RevNo"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}&Revision={2}" DataTextField="Code"
                                    HeaderText="Code" SortExpression="Code" Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID,CourseID,RevNo"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}&Revision={2}" DataTextField="MType"
                                    HeaderText="Type" SortExpression="MType" Target="_self">
                                    <HeaderStyle Width="10%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID,CourseID,RevNo"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}&Revision={2}" DataTextField="EC"
                                    HeaderText="Elec/Comp" SortExpression="EC" Target="_self">
                                    <HeaderStyle Width="10%" HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID,CourseID,RevNo"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}&Revision={2}" DataTextField="ExamMode"
                                    HeaderText="Exam Mode" SortExpression="ExamMode" Target="_self">
                                    <HeaderStyle Width="10%" HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccName" runat="server" Value="" />
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
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Revision No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Module Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label33" runat="server" SkinID="CaptionLabel" Text="Module Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlrevisionno" runat="server" AutoPostBack="False" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:DropDownList ID="ddlmoduletype" runat="server" AutoPostBack="True" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlmoduletype_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" style="width 33%">
                        <asp:TextBox ID="txtmodulecode" SkinID="txt248" runat="server" onkeypress="checkNumber(this,15,0,event)"
                            MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        Module Name
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3">
                        <asp:TextBox ID="txtname" runat="server" MaxLength="150" SkinID="txt756"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label29" runat="server" SkinID="CaptionLabel" Text="Short Module Name&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label40" runat="server" SkinID="CaptionLabel" Text="Project Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label39" runat="server" SkinID="CaptionLabel" Text="Synopsis Required &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtsubname" runat="server" MaxLength="10" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                            <ContentTemplate>
                                <asp:DropDownList Enabled="false"  ID="ddlProjectNo" runat="server" AutoPostBack="false" 
                                    SkinID="ddl250"  >
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlmoduletype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" >
                            <ContentTemplate>
                                <asp:DropDownList Enabled="false"  ID="ddlSynopsis" runat="server"  SkinID="ddl250">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                    <asp:ListItem Value="False">No</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlmoduletype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label27" runat="server" SkinID="CaptionLabel" Text="Module Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label36" runat="server" SkinID="CaptionLabel" Text="Selection Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label34" runat="server" SkinID="CaptionLabel" Text="Elective Group &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlselectiontype" runat="server" AutoPostBack="True" SkinID="ddl250"
                                    OnSelectedIndexChanged="ddlselectiontype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlselectiontype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                        <asp:TextBox ID="txtmoduleno" runat="server" MaxLength="4" onkeyup="changetext()"
                            SkinID="txt248" onkeypress="checkNumber(this,4,0,event)"></asp:TextBox>
                    </td>
                    <td>
                       
                            <asp:DropDownList ID="ddlselectiontype" runat="server" AutoPostBack="True" SkinID="ddl250"
                                OnSelectedIndexChanged="ddlselectiontype_SelectedIndexChanged">
                            </asp:DropDownList>
                           
                            
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtElectiveGroup" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event)"
                                    SkinID="txt248" AutoPostBack="True" OnTextChanged="txtElectiveGroup_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlselectiontype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label38" runat="server" SkinID="CaptionLabel" Text="Elective Module No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label41" runat="server" SkinID="CaptionLabel" Text="Module Sub Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Allowed No of Modules. &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                       
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtsubmoduleno" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event)"
                                    SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlselectiontype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top" id="txteffectiveto">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSubCodeNumber" runat="server" MaxLength="4" onkeypress="checkNumber(this,4,0,event)"
                                    SkinID="txt248">
                                </asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlselectiontype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtallowedmodules" runat="server" MaxLength="2" onkeypress="checkNumber(this,2,0,event)"
                                    SkinID="txt248"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlselectiontype" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label37" runat="server" SkinID="CaptionLabel" Text="Effective From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                   <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Select Exam Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txteffectivefrom" runat="server" MaxLength="100" SkinID="txt210"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdatefrom" TargetControlID="txteffectivefrom">
                        </asp:CalendarExtender>
                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                    <td>
                   <asp:DropDownList ID="ddlExamMode" runat="server" AutoPostBack="False" SkinID="ddl250">
                       <asp:ListItem Value="0">--Select One--</asp:ListItem>
                       <asp:ListItem Value="1"> Online </asp:ListItem>
                       <asp:ListItem Value="2"> Offline </asp:ListItem>
                        </asp:DropDownList>
                </td>
                <td></td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFileds();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
             <table class="sample2" cellpadding="2" cellspacing="0" id="tblPrEligibility" runat="server" visible="false">
                <tr class="heading">
                    <td colspan="2">
                        Module Eligibility
                    </td>
                </tr>
                <tr class="even">
                    <td width="60%" valign="top">
                            <asp:Label ID="Label3" runat="server" Text="Compulsary Papers"></asp:Label>
                            <asp:CheckBoxList ID="chkModulelist" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="1" CellSpacing="0" EnableTheming="False" 
                            Font-Size="8pt">
                        </asp:CheckBoxList>
                    </td>
                    <td width="4%" valign="top" align="left">
                     <asp:Label ID="Label4" runat="server" Text="Elective Papers"></asp:Label>
                        <asp:TreeView ShowLines="false" ID="trvElective" Width="300px" runat="server" 
                            ShowCheckBoxes="Root" BorderWidth="0px" EnableTheming="True" Font-Size="10pt" 
                            NodeIndent="10" NodeWrap="True">
                            <LeafNodeStyle Font-Size="8pt" />
                            <NodeStyle Font-Size="10pt" />
                        </asp:TreeView>
                     <%--<asp:CheckBoxList ID="chkModuleElectiveList" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="1" CellSpacing="0" EnableTheming="False" 
                            Font-Size="8pt">
                        </asp:CheckBoxList>--%>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:Button ID="btnPrList" runat="server" Text="Update List" onclick="btnPrList_Click" 
                            />
                    </td>
                </tr>
                </table>
            
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
     <uc5:SideLink ID="SideLink1" runat="server" />
</asp:Content>
