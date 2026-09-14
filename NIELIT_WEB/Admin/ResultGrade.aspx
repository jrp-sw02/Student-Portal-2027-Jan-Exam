<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="ResultGrade.aspx.cs" Inherits="Admin_ResultGrade" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Result Grade"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlfilterccategory" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Width="100%" runat="server" Text="Version"></asp:Label>
                                        <asp:DropDownList ID="ddlflversion" Width="100%" runat="server" AutoPostBack="True">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Result Code"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<script language="javascript" type="text/javascript">
    function ValidateFormFields() {
        if (!isBlank("<%=txtCode.ClientID %>", "Code"))
            return false;
        if (!isBlankDate("<%=txtEffectiveFromDate.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
            return false;
        if (!isDate("<%=txtEffectiveFromDate.ClientID %>", "Effective From Date", "dd-MMM-yyyy"))
            return false;
        if (!isSelected("<%=ddlStatus.ClientID %>", "Status"))
            return false;
//        if (!isBlankNumber("<%=txtPercentageFrom.ClientID %>", "Percentage From"))
//            return false;
//        if (!isNumber("<%=txtPercentageFrom.ClientID %>"))
//            return false;
//        if (!isBlankNumber("<%=txtPassedTo.ClientID %>", "Percentage To"))
//            return false;
//        if (!isNumber("<%=txtPassedTo.ClientID %>"))
//            return false;
        var perfrom = document.getElementById("<%=txtPercentageFrom.ClientID %>").value;
        var perto = document.getElementById("<%=txtPassedTo.ClientID %>").value;
        if (perfrom!="" && perto !="")
        {
            if (!isNumber("<%=txtPercentageFrom.ClientID %>"))
                return false;
            if (!isNumber("<%=txtPassedTo.ClientID %>"))
               return false;
            if (parseInt(perfrom) > parseInt(perto))
            {
                 alert("Percentage from value should be less than percentage to value");
                 return false;
            }
         }
         if (perfrom != "" && perto == "") {
             alert("Percentage to cannot be blank");
             return false;
         }
         if (perfrom == "" && perto != "") {
             alert("Percentage from cannot be blank");
             return false;
         }
        if (!isBlank("<%=txtDescription.ClientID %>", "Description"))
            return false;
        if (!isSelected("<%=ddlccat.ClientID %>", "Course Category"))
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
                            class="ActionPopup" style="width: 100px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <%-- <asp:LinkButton ID="lbResetGrid" OnClientClick="return ConfirmAction('Are you sure you want to reset password of selected user!');"
                                        runat="server" Text="Reset Password" ToolTip="click to reset password" SkinID="lnkbtnAction"
                                        CommandName="Reset" OnClick="PerformPopupAction"></asp:LinkButton>
                                    <asp:LinkButton ID="lbChnageStatus" OnClientClick="return ConfirmAction('Are you sure you want to change login status of selected user!');"
                                        runat="server" Text="Change Login Status" ToolTip="click to Change Login Status"
                                        SkinID="lnkbtnAction" CommandName="ChangeStatus" OnClick="PerformPopupAction"></asp:LinkButton>--%>
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction" CommandName="Delete"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="3%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="22%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ccat" HeaderText="Course Category" SortExpression="ccat" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="12%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Code" HeaderText="Code" SortExpression="Code" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="60%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Description" HeaderText="Grade Description" SortExpression="Description"
                                    Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
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
        <asp:View ID="New" runat="server">
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblState" runat="server" SkinID="CaptionLabel" Text="Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" >
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Effective From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" >
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" 
                            Text="Is Passed &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="8" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEffectiveFromDate" runat="server" SkinID="txt210" ToolTip="Effective From Date"
                            MaxLength="11"></asp:TextBox>
                        <img id="img1" alt="Calendar" src="../images/calendaricon.jpg" style="width: 20px;
                            height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtEffectiveFromDate" PopupPosition="BottomLeft"></asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                    <asp:DropDownList ID="ddlStatus" runat="server" SkinID="ddl250">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="2">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Percentage From &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" >
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" 
                            Text="Percentage To &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" >
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" 
                            Text="Description &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPercentageFrom" runat="server" MaxLength="3" SkinID="txt248"
                            onkeypress="checkNumber(this,3,0,event)" Height="25"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPassedTo" runat="server" SkinID="txt248" MaxLength="3"  onkeypress="checkNumber(this,3,0,event)"
                            Height="25"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtDescription" runat="server" SkinID="txt248" MaxLength="50" TextMode="MultiLine"
                            Height="25" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td>
                        <asp:label id="Label17" runat="server" skinid="CaptionLabel" text="Version No. &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            width="100%"></asp:label>
                     </td>
                    <td></td>
                </tr>
                <tr class="even">
                <td>
                 <asp:DropDownList ID="ddlccat" runat="server" SkinID="ddl250">
                 <asp:ListItem Value="0">--Select One--</asp:ListItem>
                 </asp:DropDownList>
                </td>
                <td>
                    <asp:dropdownlist id="ddlversion" runat="server" skinid="ddl250">
                    </asp:dropdownlist>
                   </td>
                <td>
                </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="Btnsaveas" runat="server" Text="Save As New " 
                    OnClientClick="return ValidateFormFields();" onclick="Btnsaveas_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
               </div>
            <div id="DivHistory" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="UPanelHistory" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <table cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <strong>Result Grade History :</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblError2" runat="server" CssClass="error" EnableTheming="False" Visible="False"
                                        Width="99%"></asp:Label>
                                    <asp:GridView ID="GridViewOld" runat="server" OnSorting="GridViewOld_Sorting" AutoGenerateColumns="False"
                                        OnRowDataBound="GridViewOld_RowDataBound" Width="100%">
                                        <Columns>
                                            <asp:BoundField HeaderText="#"><ItemStyle Width="4%" /></asp:BoundField>
                                            <asp:BoundField HeaderText="Version" DataField="version" SortExpression="version"><ItemStyle Width="8%" HorizontalAlign="left" /></asp:BoundField>
                                            <asp:TemplateField HeaderText="Effective From Date" SortExpression="MaxEffectiveFromDate" ><ItemTemplate>
                                                <%# Eval("MaxEffectiveFromDate", "{0:dd-MMM-yyyy}") %></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="16%" /></asp:TemplateField>
                                            <asp:BoundField HeaderText="Course Category" DataField="ccategory" SortExpression="category">
                                                <ItemStyle Width="15%" HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Grade" DataField="grade" SortExpression="grade"><ItemStyle Width="8%" HorizontalAlign="left" /></asp:BoundField>
                                            <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description"><ItemStyle Width="60%" HorizontalAlign="left" /></asp:BoundField>
                                        </Columns>
                                        <PagerSettings Visible="False" />
                                    </asp:GridView>
                                    <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChangedOld" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

