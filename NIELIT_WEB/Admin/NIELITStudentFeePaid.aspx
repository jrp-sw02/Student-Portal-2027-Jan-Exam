<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BatchCourseCentre.aspx.cs" Inherits="Admin_BatchCourseCentre" %>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NIELITStudentFeePaid.aspx.cs"
    Inherits="Admin_NIELITStudentFeePaid" MasterPageFile="~/MasterPages/main.master"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NIELIT Student Fee Paid"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">

    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" ID="filterPnal_upnlFilter" RenderMode="Inline"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>

                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                                    Text="Filter Panel"></asp:Label>
                                                <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                                    Text="" OnClick="ResetFilterPanel" />
                                                <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                                    Text="" OnClick="AllyFilter" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Label5" Width="100%" runat="server" Text="Batch Name"></asp:Label>
                                                <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlbatchname_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                            </div>
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
            if (!isSelected("<%=ddlBatch.ClientID  %>", "Batch Name"))
                return false;

            if (!isSelected("<%=ddlfeetypemasid.ClientID  %>", "Fee Type "))
                return false;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
    </script>
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
                            <asp:ListItem Value="1">Accredited Centres</asp:ListItem>
                            <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>
                            <asp:ListItem Value="2">NIELIT Centre</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Sub Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="lblBatch" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;" Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Fee Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlSubcentreName" Width="100%" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlSubcentreName_SelectedIndexChanged" SkinID="ddl250">
                            <asp:ListItem Value="99" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" SkinID="ddl250">
                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlfeetypemasid" Width="100%" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnShowRecord" runat="server" Text="ShowRecord" OnClick="ShowRecord" OnClientClick="return ValidateFormFields();" />
    </div>
    <div id="msg">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="false"></asp:Label>
                <asp:HiddenField ID="hcentreID" runat="server" /><asp:HiddenField ID="HSubcentreID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divGrid" runat="server" visible="false">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div style="text-align: right; margin-top: 10px">
                    <%-- <asp:Button ID="btnShowRecord"  runat="server" Text="ShowRecord"  OnClick="ShowRecord" />--%>
                </div>
                <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                    class="ActionPopup" style="width: 132px; height: 40px;">
                    <tr>
                        <td align="left"></td>
                    </tr>
                </table>
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="600px" ShowHeader="true">
                    <RowStyle Height="40px" />
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="SL">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <%-- <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                            DataTextField="ID" HeaderText="ID" SortExpression="ID" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>--%>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ID" HeaderText="ID" SortExpression="ID" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <%--<asp:HyperLinkField HeaderStyle-Width="16%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                            DataTextField="Name" HeaderText="Candidates Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>--%>
                        <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="Name" HeaderText="Candidates Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                        <%-- <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                            DataTextField="FatherName" HeaderText="FatherName" SortExpression="BatchCode" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>--%>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="FatherName" HeaderText="FatherName" SortExpression="BatchCode" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                        <%-- <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                            DataTextField="DoB" HeaderText="DOB" SortExpression="DOB"
                            Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>--%>
                        <asp:HyperLinkField HeaderStyle-Width="25%"
                            DataTextField="DoB" HeaderText="DOB" SortExpression="DOB"
                            Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="AmountPaid" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("AmountPaid") %>' Width="110px"></asp:Label>

                                <asp:TextBox ID="txtAmountPaid" runat="server" Text='<%# Eval("AmountPaid") %>'
                                    Width="70px" Visible="false" MaxLength="7"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PaymentDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lblpaymentDate" runat="server" Text='<%# String.Format("{0:dd-MMM-yyyy}", Eval("PaymentDate")) %>' Width="150px"></asp:Label>
                                <asp:TextBox ID="txtPaymentDate" runat="server" Text='<%#  Eval("PaymentDate","{0:dd-MMM-yyyy}")  %>'
                                    SkinID="txtDate" Width="100px" Visible="false" MaxLength="11"></asp:TextBox>
                                <img id="img3" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" visible="false" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtPaymentDate">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="lblRemark_s" runat="server" Text='<%# Bind("Remark_s") %>' Width="110px"></asp:Label>
                                <asp:TextBox ID="txtRemark_s" runat="server" Text='<%# Eval("Remark_s") %>'
                                    TextMode="MultiLine" Width="110px" Visible="false" MaxLength="500"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="FeetypeMasIDD" HeaderText="FeeDescription" SortExpression="FeetypeMasIDD" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                        <asp:TemplateField Visible="false" HeaderText="lblIdVisFalse">
                            <ItemTemplate>
                                <asp:Label runat="server" Visible="true" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                            </ItemTemplate>
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
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="text-align: right; margin-top: 10px; height: 480px">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
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
