<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="FeeDetail.aspx.cs" Inherits="FeeDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Fee Detail"></asp:Label>
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
                                            Text="" OnClick="ApplyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseCategory" Width="100%" runat="server" OnSelectedIndexChanged="ddlCourseCategory_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--ALL--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlCourseName" runat="server" Width="100%" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                                    <asp:ListItem Text="--ALL--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategory" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Fee Type"></asp:Label>
                                        <asp:DropDownList ID="ddlFeeType" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--ALL--"></asp:ListItem>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateForm() {
            if (!isSelected("<%=ddlCourseCategoryE.ClientID %>", "Course Category"))
                return false;

            if (!isSelected("<%=ddlCourseNameE.ClientID %>", "Course"))
                return false;

            if (!isBlankDate("<%= txtEffectiveFromDate.ClientID%>", "Effective From Date", "dd-MMM-yyyy"))
                return false;

            if (!isDate("<%= txtEffectiveFromDate.ClientID%>", "Invalid Effective From Date", "dd-MMM-yyyy"))
                return false;

            if (!isSelected("<%=ddlFeeTypeE.ClientID %>", "Fee Type"))
                return false;

            if (!isNumber("<%=TxtFeeAmount.ClientID %>", "Fee Amount"))
                return false;
            return true;


            //            if (!isBlankNumber("<%=TxtFeeAmount.ClientID %>", "Fee Amount"))
            //                return false;

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
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <ItemStyle Width="3%" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CourseName" HeaderText="Course Name" SortExpression="CourseName"
                                    Target="_self">
                                    <ItemStyle Width="12%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="FeeType" HeaderText="Fee Type" SortExpression="FeeType" Target="_self">
                                    <ItemStyle Width="38%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="FeeAmount" HeaderText="Fee Amount" SortExpression="FeeAmount"
                                    Target="_self">
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="MaxEffectiveFromDate" HeaderText="Effective From Date" SortExpression="MaxEffectiveFromDate"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle Width="16%" HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
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
            <asp:Label ID="LblError" runat="server" ForeColor="#FF6600"></asp:Label>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Effective From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" class="style1">
                        <asp:DropDownList ID="ddlCourseCategoryE" runat="server" AutoPostBack="True" Height="22px"
                            SkinID="ddl250" OnSelectedIndexChanged="ddlCourseCategoryE_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourseNameE" runat="server" Height="22px" OnSelectedIndexChanged="ddlCourseNameE_SelectedIndexChanged"
                                    SkinID="ddl250" AutoPostBack="True">
                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseCategoryE" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEffectiveFromDate" runat="server" MaxLength="11" SkinID="txt210"
                                    TabIndex="10" Width="99px"></asp:TextBox>
                                <asp:CalendarExtender ID="ceDOB" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEffDate"
                                    PopupPosition="BottomLeft" TargetControlID="txtEffectiveFromDate">
                                </asp:CalendarExtender>
                                <img id="imgEffDate" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                    vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Fee Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        &nbsp;
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" Text="Fee Amount &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFeeTypeE" runat="server" Height="22px" SkinID="ddl504">
                                    <asp:ListItem Text="--Select One--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCourseNameE" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" class="style2">
                        <asp:TextBox ID="TxtFeeAmount" runat="server" onkeypress="checkNumber(this,5,0,event);"
                            Style="text-align: right" MaxLength="5" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateForm();" runat="server" Text="Save"
                    OnClick="SaveRecord" Style="height: 26px" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
            <div id="DivHistory" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="UPanelHistory" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <table cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <strong>Fee Detail History :</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblError2" runat="server" CssClass="error" EnableTheming="False" Visible="False"
                                        Width="99%"></asp:Label>
                                    <asp:GridView ID="GridViewOld" runat="server" OnSorting="GridViewOld_Sorting" AutoGenerateColumns="False"
                                        OnRowDataBound="GridViewOld_RowDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="#">
                                                <ItemStyle Width="3%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Effective From Date" SortExpression="MaxEffectiveFromDate">
                                                <ItemTemplate>
                                                    <%# Eval("MaxEffectiveFromDate", "{0:dd-MMM-yyyy}") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="16%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Type" SortExpression="FeeType">
                                                <ItemTemplate>
                                                    <%# Eval("FeeType") %>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Fee Amount" DataField="FeeAmt" SortExpression="FeeAmt">
                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            </asp:BoundField>
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
        
        
        .style1
        {
            height: 24px;
        }
        .style2
        {
            width: 33%;
            height: 24px;
        }
    </style>
</asp:Content>
