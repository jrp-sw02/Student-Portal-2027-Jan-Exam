<%@ Page Title="NIELIT" Language="C#" MasterPageFile="~/MasterPages/main.master"
    AutoEventWireup="true" CodeFile="verifycoursecompletionaffidavitdateacc.aspx.cs"
    Inherits="verifycoursecompletionaffidavitdateacc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Verification of ACC Course Completion - Affidavit Dates
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphAddNew" runat="Server">
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
                                        <asp:Label ID="Label8" Width="100%" runat="server" Text="Institute Id"></asp:Label>
                                        <asp:DropDownList ID="ddlInstitute" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Year"></asp:Label>
                                        <asp:DropDownList ID="ddlYear" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Registration Cycle"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlRegnCycle" Width="100%" runat="server">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlYear" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Verification Status"></asp:Label>
                                        <asp:DropDownList ID="ddlVerified" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="All"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Certificate Issued"></asp:Label>
                                        <asp:DropDownList ID="ddlCertificateIssued" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="All"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="N"></asp:ListItem>
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
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {
            if (!isSelected("<%=ddlInstitute.ClientID %>", "Institute"))
                return false;
            if (!isSelected("<%=ddlYear.ClientID %>", "Regn Year"))
                return false;
            if (!isSelected("<%=ddlRegnCycle.ClientID %>", "Regn Cycle"))
                return false;
        }

        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=gbapplicant.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }

        function Selectchildcheckboxes(header) {
            var ck = header;
            var count = 0;
            var gvcheck = document.getElementById("<%=gbapplicant.ClientID %>");
            var headerchk = document.getElementById(header);
            var rowcount = gvcheck.rows.length;
            //By using this for loop we will count how many checkboxes has checked
            for (i = 1; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                if (inputs[0].checked) {
                    count++;
                }
            }
            //Condition to check all the checkboxes selected or not
            if (count == rowcount - 1) {
                headerchk.checked = true;
            }
            else {
                headerchk.checked = false;
            }
        }
    </script>
    <div>
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label Width="99%" EnableTheming="false" ID="LabelDetail" runat="server" Text=""></asp:Label>
                <div id="divgrid" runat="server">
                    <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                        <tr>
                            <td>
                                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">
                                <asp:Button ID="btnsubmit" Visible="false" Style="float: right; margin-left: 3px;"
                                    runat="server" OnClick="btnsubmit_Click" OnClientClick="return Validate_Checkbox('Are you sure you want to Validate selected applications!')"
                                    Text="Confirm" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gbapplicant" runat="server" AutoGenerateColumns="False" DataKeyNames="RegistrationNo, CourseId, IsVerified, CertificateIssued"
                        OnRowDataBound="gbapplicant_RowDataBound" Width="100%" HeaderStyle-Font-Size="12px"  
                        HeaderStyle-Height="25px" RowStyle-Height="25px" OnRowEditing="gbapplicant_RowEditing"
                        OnRowUpdating="gbapplicant_RowUpdating" OnRowCancelingEdit="gbapplicant_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="2%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderStyle-Width="3%" DataField="RegistrationNo" HeaderText="Regn.No"
                                ReadOnly="true"></asp:BoundField>
                            <asp:BoundField HeaderStyle-Width="12%" DataField="Name" HeaderText="Name" ReadOnly="true">
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Start Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Date1" runat="server" Text='<%#Eval("SessionStart") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox MaxLength="11" ID="txtAfdStart" runat="server" Text='<%#Eval("SessionStart") %>'
                                        SkinID="txtDate" Width="80px">
                                    </asp:TextBox>
                                    <img id="imgjoin1" alt="calander" runat="server" src="../images/calendaricon.jpg"
                                        style="width: 20px; height: 20px; vertical-align: top;" />
                                    <asp:CalendarExtender ID="ceDOjoin1" TargetControlID="txtAfdStart" PopupPosition="BottomLeft"
                                        Format="dd-MMM-yyyy" PopupButtonID="imgjoin1" runat="server">
                                    </asp:CalendarExtender>
                                </EditItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="End Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Date2" runat="server" Text='<%#Eval("SessionEnd") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox MaxLength="11" ID="txtAfdEnd" runat="server" Text='<%#Eval("SessionEnd") %>'
                                        SkinID="txtDate" Width="80px">
                                    </asp:TextBox>
                                    <img id="imgjoin2" alt="calander" runat="server" src="../images/calendaricon.jpg"
                                        style="width: 20px; height: 20px; vertical-align: top;" />
                                    <asp:CalendarExtender ID="ceDOjoin2" TargetControlID="txtAfdEnd" PopupPosition="BottomLeft"
                                        Format="dd-MMM-yyyy" PopupButtonID="imgjoin2" runat="server">
                                    </asp:CalendarExtender>
                                </EditItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="9%" HeaderText="Status">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" Style="color: Navy;
                                        font-weight: bold;" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btn_Update" runat="server" Text="Update" CommandName="Update"
                                        Style="color: Navy; font-weight: bold;" />
                                    <asp:LinkButton ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"
                                        Style="color: Navy; font-weight: bold;" />
                                </EditItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="1%" HeaderText="">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkheader" runat="server" onclick="javascript:SelectheaderCheckboxes(this)" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkchild" runat="server" onclick="javascript:Selectchildcheckboxes(chkheader)" /></ItemTemplate>
                                <HeaderStyle Width="1%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>                        
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divNavigation" runat="server">
            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
