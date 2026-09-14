<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NielitCentreBatch.aspx.cs"
    Inherits="Admin_NielitCentreBatch" MasterPageFile="~/MasterPages/main.master"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Batch Course Centres"></asp:Label>
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
                                  <tr>
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
                                </tr>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Batch Name"
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

            if (!isSelected("<%=ddlCourse.ClientID  %>", "Course"))
                return false;
            if (!isBlankDate("<%=txtstartDate.ClientID %>", "Start Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtendDate.ClientID %>", "End Date", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("<%=ddlbatchSession.ClientID  %>", "Batch Session"))
                return false;
            if (!isBlank("<%=txtbatchDurationTheoryHours.ClientID  %>", "Batch Duration Theory Hours"))
                return false;
            if (!isBlank("<%=txtbatchDurationPracticalHours.ClientID  %>", "Batch Duration Practical Hours"))
                return false;
            if (!isNumber("<%=txtbatchDurationTheoryHours.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isNumber("<%=txtbatchDurationPracticalHours.ClientID %>", "Numeric characters are  allowed"))
                return false;
            if (!isSelected("<%=ddlCorporate.ClientID  %>", "Corporate"))
                return false;
            if (!isBlank("<%=txtFName.ClientID  %>", "Faculty  Name"))
                return false;
            if (!isBlank("<%=txtFEmail.ClientID  %>", "Faculty Email"))
                return false;
            if (!IsValidMinMaxLenght("<%=txtbatchDurationPracticalHours.ClientID %>", 1, 6, "Invalid Practical Hours"))
                return false;
            if (!IsValidMinMaxLenght("<%=txtbatchDurationTheoryHours.ClientID %>", 1, 6, "Invalid Theory Hours"))
                return false;

            var frdate = document.getElementById("<%=txtstartDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtendDate.ClientID %>").value;

            if (!CompareDates(frdate, todate, "Start date should be less then End date", true))
                return false;

            var e2;

            e2 = document.getElementById("<%=txtFEmail.ClientID %>").value;
                if (e2 != "") {
                    if (!isValidEmail("<%=txtFEmail.ClientID %>", "Not A Valid Email Address"))
                        return false;
                }
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
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="CourseName"
                                    HeaderText="CourseName" SortExpression="CourseName" Target="_self">
                                <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="Name"
                                    HeaderText="BatchName" SortExpression="Name" Target="_self">
                                <HeaderStyle Width="20%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="BatchCode"
                                    HeaderText="Batch Code" SortExpression="Code" Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="learningModeName"
                                    HeaderText="LearningMode Name" SortExpression="learningModeName" Target="_self">
                                <HeaderStyle Width="15%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="startDate"
                                    HeaderText="Start Date" SortExpression="State" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="endDate"
                                    HeaderText="End Date" SortExpression="Course" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CourseId={1}" DataTextField="IsVerified"
                                    HeaderText="Is Verified" SortExpression="Code" Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,CourseId"
                                    DataNavigateUrlFormatString="?Key={0}&CategoryID={1}" DataTextField="Course"
                                    HeaderText="CourseID" Visible="false" SortExpression="Course" Target="_self">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
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
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td colspan="2" style="width: 66%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institutes"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">&nbsp;</td>
                </tr>
                <tr class="even">
                    <td colspan="2" style="width: 66%;" valign="top">
                        <asp:TextBox Style="width: 501px;" ID="txtInstitute" runat="server" Enabled="false" SkinID="txt248" Width="100%" ToolTip="Institute"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top"></td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">

                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                                    Style="height: 27px" Font-Bold="True">
                                    <asp:ListItem Value="1">Accredited Centre</asp:ListItem>
                                    <asp:ListItem Value="0">Non Accredited Centre</asp:ListItem>
                                    <asp:ListItem Value="2">Nielit Centre</asp:ListItem>
                                </asp:RadioButtonList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblState0" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="20%"></asp:Label>
                                <asp:Label ID="lblerrorddlcouse" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
                <tr class="even">
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="True" Width="760px" SkinID="ddl760" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Sub Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Batch Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSubcentreName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlSubcentreName_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtName" runat="server" SkinID="txt248" Style="text-transform: uppercase;" ToolTip="Name" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtBatchCode" runat="server" SkinID="txt248" Style="text-transform: uppercase;" MaxLength="50" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Start Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="End Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Batch Session &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtstartDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" AutoPostBack="True" OnTextChanged="txtstartDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtstartDate">
                                </asp:CalendarExtender>
                                <img id="img2" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtendDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtendDate">
                                </asp:CalendarExtender>
                                <img id="img3" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlbatchSession" runat="server" SkinID="ddl250">
                            <%--<asp:ListItem Value="0" Text="--Select One--"></asp:ListItem> 
                            <asp:ListItem Value="M" Text="Morning Session"></asp:ListItem>
                            <asp:ListItem Value="A" Text="Afternoon Session"></asp:ListItem> 
                            <asp:ListItem Value="E" Text="Evening Session"></asp:ListItem>  --%>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" MaxLength="3" SkinID="CaptionLabel" Text="Batch Duration Theory Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;(per Day)"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label20" runat="server" MaxLength="3" SkinID="CaptionLabel" Text="Batch Duration Practical Hours &lt;b class='mandatory'&gt;*&lt;/b&gt;(per Day)"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Whether Corporate &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtbatchDurationTheoryHours" runat="server" onkeypress="checkNumber(this,2,0,event);" MaxLength="3" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtbatchDurationPracticalHours" runat="server" onkeypress="checkNumber(this,2,0,event);" MaxLength="3" onpaste="return false;" SkinID="txt248" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCorporate" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlCorporate_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label19" runat="server" SkinID="CaptionLabel" Text="Organisation Trained &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Course Coordinator Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text="Course Coordinator Email &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtOrgTrained" runat="server" MaxLength="100" SkinID="txt248" onpaste="return false;" Width="200px"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtFName" runat="server" MaxLength="50" SkinID="txt248" onpaste="return false;" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFEmail" Width="200px" MaxLength="50" runat="server" SkinID="txt248" Style="text-transform: uppercase;"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="Rview1" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblShowOnWeb" runat="server" SkinID="CaptionLabel" Visible="false" Text="Show On Web &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblIsVerifieds" runat="server" Visible="false" SkinID="CaptionLabel" Text="Is Verified &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblIsActive" runat="server" Visible="false" SkinID="CaptionLabel" Text="Is Active &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr id="Rview" runat="server" class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlShowOnWeb" runat="server" SkinID="ddl250" Visible="false">
                            <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlVerifieds" runat="server" SkinID="ddl250" Visible="false">
                            <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlActive" runat="server" SkinID="ddl250" Visible="false">
                            <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>


              <%--  Added for IsSemBased On--%>

                <tr id="trsembased" runat="server" visible="false">
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Whether Semester/Year Based" Width="64%"></asp:Label>
                                <asp:Label ID="lblwhetherSembased" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
                <tr  id="trsembased1" runat="server" class="even" visible="false">
                    <td colspan="3" style="width: 66%;" valign="top">
                       <%-- <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                            <ContentTemplate>--%>
                                <asp:DropDownList ID="ddlIsSemBased" runat="server" AutoPostBack="True" Width="760px" SkinID="ddl760"  OnSelectedIndexChanged="ddlIsSemBased_SelectedIndexChanged">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                           <%-- </ContentTemplate>
                            <Triggers> 
                            </Triggers>  
                        </asp:UpdatePanel>--%>
                    </td>
                   <%-- <caption>
                    </caption>--%>
                </tr>

                <tr id="trsemyearBased" runat="server" visible="false">
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label23" runat="server" SkinID="CaptionLabel" Text="Semester/Year Based" Width="64%"></asp:Label>
                                <asp:Label ID="lblyearSem" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
                 <tr id="trsemyearBased1" runat="server" visible="false" class="even">
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSemYearBased" runat="server" AutoPostBack="True" Width="760px" SkinID="ddl760">
                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Semester" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Year" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers> 
                            </Triggers>  
                        </asp:UpdatePanel>
                    </td>
                  
                </tr>

 	<tr id="truniversity" runat="server" visible="false">
                    <td style="width: 10%;" valign="top" colspan="2">
                        <asp:Label ID="Label16" runat="server" SkinID="CaptionLabel" Text="University Name &lt;b class=''&gt;&lt;/b&gt;"></asp:Label>
                        <asp:Label ID="lbluniversity" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                    </td>

         <td style="width: 100%;"  valign="top" colspan="1">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" Text="Registration Duration(in Years) &lt;b class=''&gt;&lt;/b&gt;"></asp:Label>
                        <asp:Label ID="lblRegDuration" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                    </td>
                    
                </tr>
                <tr id="truniversity1" runat="server" class="even" visible="false">
                    <td style="width: 100%;"   valign="top" colspan="2">
                        <asp:TextBox ID="txtUniversity" runat="server" MaxLength="100" SkinID="txt248" onpaste="return false;" Width="100%"></asp:TextBox>
                    </td>

                    <td style="width: 100%;"  valign="top" colspan="1">
                        <asp:TextBox ID="txtRegDuration" runat="server" MaxLength="15" SkinID="txt248" onpaste="return false;" Width="100%"></asp:TextBox>
                    </td>
                </tr>

                   <%--  end for IsSemBased On--%>

                                     <!-- start by rka -->
                 <tr>
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Learning Mode" Width="20%"></asp:Label>
                                <asp:Label ID="Label7" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="60%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
                <tr class="even">
                    <td colspan="3" style="width: 66%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLearningMode" runat="server" AutoPostBack="True" Width="760px" SkinID="ddl760">
                                    <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers> 
                            </Triggers>  
                        </asp:UpdatePanel>
                    </td>
                </tr>
 			<tr>
                    <td style="width: 100%;" colspan="3" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Remarks &lt;b class=''&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    
                </tr>
                <tr class="even">
                    <td style="width: 100%;" colspan="3"  valign="top">
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="1000" SkinID="txt248" onpaste="return false;" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <!-- end by rka -->
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
                     <asp:HiddenField ID="AFLINSTID" runat="server" />
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
