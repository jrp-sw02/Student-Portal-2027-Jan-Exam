<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ForwardExam.aspx.cs" Inherits="HO_Default" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Forward Applications"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
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
                                            Text="" OnClientClick="return validatefilter()" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReg" runat="server" Width="100%" Text="Regional Centre"></asp:Label>
                                        <asp:DropDownList ID="ddlRc" runat="server" Width="100%">
                                            <asp:ListItem Value="0">--All--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" Width="100%" runat="server" Text="Course Category"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursecategory" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlcourse" Width="100%" runat="server" OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:DropDownList ID="ddlexamcycle" Width="100%" runat="server" OnSelectedIndexChanged="ddlexamcycle_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:DropDownList ID="ddlexamyear" Width="100%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlexamyear_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:DropDownList ID="ddlexamname" Width="100%" runat="server" OnSelectedIndexChanged="ddlexamname_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPaymentStatus" Width="100%" runat="server" Text="Exam Center"></asp:Label>
                                        <asp:DropDownList ID="ddlexamcenter" Width="100%" runat="server">
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlexamcycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlexamyear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlexamname.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlexamcenter.ClientID %>", "Exam Center"))
                return false;
        }

        function ValidateLogin() {
            return true;
        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function validateExam() {
            if (!isSelected("<%=ddlNewExam.ClientID %>", "Forward to Exam"))
                return false;
            if (!isSelected("<%=ddlRemarks.ClientID %>", "Remarks"))
                return false;

            if (!Validate_Checkbox('Are you sure you want to forward selected applications!'))
                return false;
        }
    </script>
    <div id="divGrid" runat="server">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <div id="NewExam" runat="server" >
                    <table class="sample2"  border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td style="width: 26%;" valign="middle">
                                <asp:Label ID="lblExaminationCycle" Text="Destination Exam Cycle" runat="server" SkinID="CaptionLabel"
                                    Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td style="width: 25%;" valign="top">
                                <asp:DropDownList ID="ddlExaminationCycle" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlExaminationCycle_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 26%;" valign="middle">
                                <asp:Label ID="lblNewExam" Text="Forward to Exam" runat="server" SkinID="CaptionLabel"
                                    Style="font-weight: bold;"></asp:Label>
                            </td>

                            <td style="width: 25%;" valign="top">
                                <asp:DropDownList ID="ddlNewExam" runat="server" SkinID="ddl250">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 25%;" valign="middle">
                                <asp:Label ID="lblRemarks" Text="Remarks" runat="server" SkinID="CaptionLabel" Style="font-weight: bold;"></asp:Label>
                            </td>
                            <td style="width: 25%;" valign="top">
                                <asp:DropDownList ID="ddlRemarks" runat="server" SkinID="ddl250">
                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <strong>Note: Candidate having Result Grade 'ABS' or 'N/A' (with Roll Number) not allowed to carry forward without result processing.</strong>
                            </td>
                            <td style="width: 33%;" valign="top" align="right" colspan="3">
                                <asp:Button ID="btnNewExam" Text="Forward to Exam" runat="server" CssClass="btnNormal"
                                    OnClick="btnNewExam_Click" OnClientClick="return validateExam()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                    OnRowDataBound="gvMain_RowDataBound">
                    <AlternatingRowStyle Font-Size="11px" Height="15px" />
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                            <HeaderStyle Width="1%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="App.No." DataField="AppNo">
                            <ItemStyle Width="8%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="RollNo." DataField="RollNumber" NullDisplayText="N/A">
                            <ItemStyle Width="8%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblApplicationDate" runat="server" Text='<%# Eval("AppDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="11%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" DataField="CandidateName">
                            <ItemStyle Width="18%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="DOB">
                            <ItemTemplate>
                                <asp:Label ID="lblDob" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="11%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Father Name" DataField="FatherName">
                            <ItemStyle Width="18%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Result-Grade" DataField="ResultGrade">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                            </HeaderTemplate>
                            <HeaderStyle Width="1%" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle Font-Size="12px" Height="15px" />
                    <PagerSettings Visible="False" />
                    <RowStyle Font-Size="11px" Font-Underline="False" Height="15px" />
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
</asp:Content>
<%--<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>--%>
<%--<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>--%>
