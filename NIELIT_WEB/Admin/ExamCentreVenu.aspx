<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ExamCentreVenu.aspx.cs" Inherits="Admin_ExamCentreVenu" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Centres Venues"></asp:Label>
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Active Status"></asp:Label>
                                        <asp:DropDownList ID="ddlflActiveStatus" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="2">InActive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="City"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflCity" Width="100%" runat="server">
                                                    <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflstates" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>--%>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Exam Venue Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isBlank("<%=txtVenueName.ClientID %>", "Venu Name"))
                return false;
            if (!isBlank("<%=Txtvenuecode.ClientID %>", "Venu Code"))
                return false;
            if (!isBlank("<%=txtAddress1.ClientID %>", "Address-1"))
                return false;
            if (!isBlank("<%=txtCity.ClientID %>", "City Name"))
                return false;
            if (!isSelected("<%=ddlActiveStatus.ClientID %>", "Active Status"))
                return false;
            if (document.getElementById("<%=txtStdCode.ClientID %>").value != "") {
                if (!isBlankNumber("<%=txtStdCode.ClientID %>", "STD Code"))
                    return false;
                if (!IsValidMinMaxLenght("<%=txtStdCode.ClientID %>", 3, 5, "STD Code"))
                    return false;
            }
            if (document.getElementById("<%=txtPhoneNumber.ClientID %>").value != "") {
                if (!isBlankNumber("<%=txtStdCode.ClientID %>", "STD Number"))
                    return false;
                if (!isNumber("<%=txtStdCode.ClientID %>"))
                    return false;
                if (!isBlankNumber("<%=txtPhoneNumber.ClientID %>", "Phone Number"))
                    return false;
                if (!isNumber("<%=txtPhoneNumber.ClientID %>", "Phone Number"))
                    return false;
                if (!isValidTeliphone("<%=txtStdCode.ClientID %>", "<%=txtPhoneNumber.ClientID %>"))
                    return false;
            }
            if (document.getElementById("<%=txtFaxNumber.ClientID %>").value != "") {
                if (!isBlankNumber("<%=txtStdCode.ClientID %>", "STD Number"))
                    return false;
                if (!isNumber("<%=txtStdCode.ClientID %>"))
                    return false;
                if (!isBlankNumber("<%=txtFaxNumber.ClientID %>", "FAX Number"))
                    return false;
                if (!isNumber("<%=txtFaxNumber.ClientID %>", "FAX Number"))
                    return false;
                if (!isValidTeliphone("<%=txtStdCode.ClientID %>", "<%=txtFaxNumber.ClientID %>"))
                    return false;
            }
            if (document.getElementById("<%=txtMobileNumber.ClientID %>").value != "") {
                if (!isBlankNumber("<%=txtMobileNumber.ClientID %>", "Mobile Number"))
                    return false;
                if (!IsValidMinMaxLenght("<%=txtMobileNumber.ClientID %>", 10, 10, "Invalid Mobile Number"))
                    return false;
            }
            if (document.getElementById("<%=txtEmail.ClientID %>").value != "") {
                if (!isBlank("<%=txtEmail.ClientID %>", "E-mail"))
                    return false;
                if (!isValidEmail("<%=txtEmail.ClientID %>", "Not A Valid Email Address"))
                    return false;
            }

            if (document.getElementById("<%=txtPinCode.ClientID %>").value != "") {
                if (!isBlankNumber("<%=txtPinCode.ClientID %>", "Pin Code"))
                    return false;
                if (!isNumber("<%=txtPinCode.ClientID %>"))
                    return false;
                if (!IsValidMinMaxLenght("<%=txtPinCode.ClientID %>", 6, 6, "Pin Code"))
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
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" Width="5%" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name" HeaderText="Venue Name" SortExpression="Name" Target="_self">
                                    <ItemStyle Width="25%" HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Code" HeaderText="Code" SortExpression="Code" Target="_self">
                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="City" HeaderText="Location" SortExpression="City" Target="_self">
                                    <ItemStyle Width="35%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="IsActive" HeaderText="Active Status" SortExpression="IsActive"
                                    Target="_self">
                                    <ItemStyle Width="25%" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode" runat="server" />
                        <asp:HiddenField ID="hfExamCentreID" runat="server" />
                        <asp:HiddenField ID="hfStateID" runat="server" />
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
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Exam Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Venue Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtExamCentre" runat="server" SkinID="txt248" ToolTip="Exam Centre"></asp:TextBox>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:TextBox ID="Txtvenuecode" runat="server" SkinID="txt502" ToolTip="Venue Code" MaxLength="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Venue Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3">
                        <asp:TextBox ID="txtVenueName" runat="server" SkinID="txt756" ToolTip="Exam Venu Name"
                            MaxLength="80"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3" class="style1">
                        <asp:Label ID="Label16" runat="server" SkinID="CaptionLabel" Text="Address-1 &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3">
                        <asp:TextBox ID="txtAddress1" runat="server" SkinID="txt756" ToolTip="Address-1"
                            MaxLength="80"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="3">
                        <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text="Address-2 &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td valign="top" colspan="3">
                        <asp:TextBox ID="txtAddress2" runat="server" SkinID="txt756" ToolTip="Address-2"
                            MaxLength="80"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="City &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="State &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Pincode &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtCity" runat="server" SkinID="txt248" ToolTip="City" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtState" runat="server" SkinID="txt248" ToolTip="State" MaxLength="40"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPinCode" runat="server" SkinID="txt248" ToolTip="Pin Code" MaxLength="6"
                            onkeypress="checkNumber(this,6,0,event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="STD Code &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Phone Number &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Fax Number &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtStdCode" runat="server" SkinID="txt248" ToolTip="STD Number"
                            MaxLength="5" onkeypress="checkNumber(this,4,0,event)"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtPhoneNumber" runat="server" SkinID="txt248" ToolTip="Phone Number"
                            MaxLength="8" onkeypress="checkNumber(this,8,0,event)"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtFaxNumber" runat="server" SkinID="txt248" ToolTip="Fax Number"
                            MaxLength="8" onkeypress="checkNumber(this,8,0,event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblState1" runat="server" SkinID="CaptionLabel" Text="Mobile Number &lt;b class='mandatory'&gt;&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label20" runat="server" SkinID="CaptionLabel" Text="Email-Id &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Active Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtMobileNumber" runat="server" SkinID="txt248" ToolTip="Mobile Number"
                            onkeypress="checkNumber(this,10,0,event)" MaxLength="10"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtEmail" runat="server" SkinID="txt248" ToolTip="Email ID" MaxLength="100"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlActiveStatus" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="2">InActive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td>
                <asp:HyperLink ID="hlExamMenu" runat="server" Target="_self">Exam Venues</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
