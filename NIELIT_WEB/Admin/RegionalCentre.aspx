<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="RegionalCentre.aspx.cs" Inherits="RegionalCentre" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Regional Centres"></asp:Label>
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
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Regional Centre"></asp:Label>
                                        <asp:DropDownList ID="ddlflregcen" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--Select All--"></asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Centre Name"
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

            if (!isBlank("<%=txtname.ClientID %>", "Regional Centre Name"))
                return false;
            if (!isBlank("<%=txtcode.ClientID %>", "Regional Centre Code"))
                return false;
            if (!isBlank("<%=txtweb.ClientID %>", "Website Address"))
                return false;
            if (!isBlank("<%=txtaddress.ClientID %>", "Address"))
                return false;
            if (!isBlank("<%=txtemail.ClientID %>", "Email"))
                return false;
            if (!isBlankNumber("<%=txtcontact.ClientID %>", "Contact Number"))
                return false;
            var email;
            email = document.getElementById("<%=txtregemail.ClientID %>").value;
            if (email != "") {
                if (!isBlank("<%=txtregemail.ClientID %>", "Registered Email Address"))
                    return false;
                if (!isValidEmail("<%=txtregemail.ClientID%>", "Not Valid Registered Email Address"))
                    return false;
            }
            var mobile;
            mobile = document.getElementById("<%=txtregmobileno.ClientID %>").value;
            if (mobile != "") {
                if (!isBlankNumber("<%=txtregmobileno.ClientID %>", "Registered Mobile No."))
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
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,Name" DataNavigateUrlFormatString="?Key={0}&Name={1}"
                                    DataTextField="Name" HeaderText="Regional Centre Name" SortExpression="Name"
                                    Target="_self">
                                    <HeaderStyle Width="20%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Code" HeaderText="Code" SortExpression="Code" Target="_self">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,Name" DataNavigateUrlFormatString="?Key={0}&Name={1}"
                                    DataTextField="email" HeaderText="E-Mail Address" SortExpression="email" Target="_self">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,Name" DataNavigateUrlFormatString="?Key={0}&Name={1}"
                                    DataTextField="contactno" HeaderText="Mobile No" SortExpression="contactno" Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <%-- <asp:HyperLinkField HeaderStyle-Width="40%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="SNAME" HeaderText="Status" SortExpression="SNAME" Target="_self"><HeaderStyle Width="40%" /><ItemStyle HorizontalAlign="Left" /></asp:HyperLinkField>--%>
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
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr class="heading">
                    <td colspan="3">
                        Regional Centre Details
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Web Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txtname" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtcode" runat="server" SkinID="txt248" MaxLength="10"></asp:TextBox>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:TextBox ID="txtweb" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblState" runat="server" SkinID="CaptionLabel" Text="Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblDistrict" runat="server" SkinID="CaptionLabel" Text="Email Addresses &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblDistrict0" runat="server" SkinID="CaptionLabel" Text="Contact Numbers &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtaddress" runat="server" SkinID="txt248" ToolTip="Regional Centre"
                            Height="60px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtemail" runat="server" SkinID="txt248" Height="60px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtcontact" runat="server" SkinID="txt248" Height="60px" TextMode="MultiLine"
                            onkeypress="checkNumber(this,50,0,event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 66%;" valign="top" colspan="2">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Registered Email Address&lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Registered Mobile No&lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <%-- <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Contact Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>--%>
                </tr>
                <tr class="even">
                    <td style="width: 66%;" valign="top" colspan="2">
                        <asp:TextBox ID="txtregemail" runat="server" SkinID="txt502" MaxLength="200"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtregmobileno" runat="server" SkinID="txt248" MaxLength="10" onkeypress="checkNumber(this,10,0,event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 66%;" valign="top" colspan="2">
                        <asp:Label ID="lblBankAccount" runat="server" SkinID="CaptionLabel" Text="Bank Account Name"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblBankBranch" runat="server" SkinID="CaptionLabel" Text="Bank Branch Name "></asp:Label>
                    </td>
                    <%-- <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Contact Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>--%>
                </tr>
                <tr class="even">
                    <td style="width: 66%;" valign="top" colspan="2">
                        <asp:TextBox ID="txtBankAccount" runat="server" SkinID="txt502" MaxLength="200"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtBankBranch" runat="server" SkinID="txt248" MaxLength="200"></asp:TextBox>
                    </td>
                    <%--<td style="width: 33%;" valign="top">
                        <asp:TextBox ID="TextBox3" runat="server" SkinID="txt248" Height="60px" TextMode="MultiLine"
                            onkeypress="checkNumber(this,50,0,event)"></asp:TextBox>
                    </td>--%>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
            <br />
            <table class="sample2" cellpadding="2" cellspacing="0" id="tblShow" runat="server"
                visible="false">
                <tr class="heading">
                    <td>
                        State-Course Mapping Details
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblCourse" runat="server" SkinID="CaptionLabel" Text="Short Term Course List &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:RadioButtonList ID="RblAllCourse" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="1" CellSpacing="0" EnableTheming="False" AutoPostBack ="true"
                            Font-Size="8pt" onselectedindexchanged="RblAllCourse_SelectedIndexChanged">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblStateList" runat="server" SkinID="CaptionLabel" Text="State List &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:CheckBoxList ID="chkStatelist" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="1" CellSpacing="0" EnableTheming="False" Font-Size="10pt">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <%--<tr>
                    <td align="right">
                        <asp:Button ID="btnbcc" runat="server" Text="Update List" OnClick="btnbcc_Click" />
                    </td>
                </tr>--%>
               <%-- <tr>
                    <td valign="top">
                        <asp:Label ID="lblCourseList" runat="server" SkinID="CaptionLabel" Text="Short Term Course List &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:CheckBoxList ID="chkCourseList" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0" Font-Size="8pt">
                        </asp:CheckBoxList>
                    </td>
                </tr>--%>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnCourseUpdate" runat="server" Text="Update List" Enabled="false" OnClick="btnCourseUpdate_Click" />
                    </td>
                </tr>
                <%--<tr>
                    <td valign="top">
                        <asp:Label ID="lblbcc1" runat="server" SkinID="CaptionLabel" Text="BCC Certification &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top">
                        <asp:CheckBoxList ID="chkbcc1statelist" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"
                            Width="100%" CellPadding="1" CellSpacing="0" EnableTheming="False" Font-Size="8pt">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnbcc1" runat="server" Text="Update List" OnClick="btnbcc1_Click" />
                    </td>
                </tr>--%>
            </table>
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
    </style>
</asp:Content>
