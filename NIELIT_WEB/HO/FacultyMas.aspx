 <%@ Page Title="Faculty Data Form" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="FacultyMas.aspx.cs" Inherits="HO_FacultyMas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Faculty Data"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter">
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
                                        <asp:Label ID="lblFilterCentre" Width="100%" runat="server" Text="Centre"></asp:Label>
                                        <asp:DropDownList ID="ddlFilterCentre" Width="100%" runat="server"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlFilterCentre_SelectedIndexChanged">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Faculty Name or Faculty Code "
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb2" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isSelected("<%=ddlCentre.ClientID %>", "Centre Name"))
                return false;
            if (!isBlank("<%=txtFacultyName.ClientID %>", "Faculty Name")) return false;
            if (!isSelected("<%=ddlGender.ClientID %>", "Gender"))
                return false;
            if (!isValidEmail("<%=txtEmail.ClientID %>", "Please enter valid Email")) return false;
            if (!isBlankNumber("<%=txtMobile.ClientID %>", "Mobile Number"))
                return false;
            if (!isBlank("<%=txtAddress1.ClientID %>", "Address Line-1"))
                return false;
            if (!isSelected("<%=ddlState.ClientID %>", "State Name"))
                return false;
            if (!isSelected("<%=ddlDistrict.ClientID %>", "District Name"))
                return false;
            if (!isBlank("<%=txtCity.ClientID %>", "City Name")) return false;
            if (!isBlankNumber("<%=txtPin.ClientID %>", "Pin Code"))
                return false;
            if (!isSelected("<%=ddlFacultyType.ClientID %>", "Faculty Type"))
                return false;
            if (!isBlankNumber("<%=txtExperience.ClientID %>", "Experience"))
                return false;
            if (!isBlank("<%=txtQualification.ClientID %>", "Highest Qualification")) return false;
            if (!isBlank("<%=txtFacultyCode.ClientID %>", "Highest Qualification")) return false;

            if (!isBlank("<%=txtEffectiveFrom.ClientID %>", "Effective from cannot be left blank ")) return false;
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
                                    <%--<asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                     runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                     OnClick="PerformPopupAction"></asp:LinkButton>--%>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID"
                            AllowSorting="true"
                            OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound"
                            AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <%-- Serial Number --%>
                                <asp:BoundField DataField="SNo" HeaderText="#" ReadOnly="true">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>

                                <%-- Faculty Name --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="FacultyMas.aspx?Key={0}"
                                    DataTextField="facultyName"
                                    HeaderText="Faculty Name"
                                    SortExpression="facultyName">
                                    <HeaderStyle Width="45%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="FacultyMas.aspx?Key={0}"
                                    DataTextField="facultyEmail"
                                    HeaderText="Faculty Email"
                                    SortExpression="facultyEmail">
                                    <HeaderStyle Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="FacultyMas.aspx?Key={0}"
                                    DataTextField="FacultyHighestQualification"
                                    HeaderText="Highest Qualification"
                                    SortExpression="FacultyHighestQualification">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

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
            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" EnableViewState="false" />
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblCentre" runat="server" Text="Centre Name *" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblFacultyName" runat="server" Text="Faculty Name *" SkinID="CaptionLabel"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblGender" runat="server" Text="Gender *" SkinID="CaptionLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:DropDownList ID="ddlCentre" runat="server" SkinID="ddl250"></asp:DropDownList>

                    </td>
                    <td>
                        <asp:TextBox ID="txtFacultyName" runat="server" SkinID="txt248" MaxLength="100"></asp:TextBox>

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGender" runat="server" SkinID="ddl250" MaxLength="10">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                        </asp:DropDownList>

                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblEmail" runat="server" Text="Email *" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblMobile" runat="server" Text="Mobile *" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblAddress1" runat="server" Text="Address Line 1 *" SkinID="CaptionLabel"></asp:Label></td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" SkinID="txt248" MaxLength="100"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" SkinID="txt248" MaxLength="10" oninput="this.value = this.value.replace(/[^0-9]/g, '');"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress1" runat="server" SkinID="txt248" MaxLength="200"></asp:TextBox>

                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblAddress2" runat="server" Text="Address Line 2" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblState" runat="server" Text="State *" SkinID="CaptionLabel"></asp:Label></td>

                    <td>
                        <asp:Label ID="lblDistrict" runat="server" Text="District *" SkinID="CaptionLabel"></asp:Label></td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:TextBox ID="txtAddress2" runat="server" SkinID="txt248" MaxLength="200"></asp:TextBox></td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" SkinID="ddl250" MaxLength="50" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                        </asp:DropDownList>

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDistrict" runat="server" SkinID="ddl250" MaxLength="50"></asp:DropDownList>

                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblCity" runat="server" Text="City *" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblPin" runat="server" Text="Pin Code *" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblFacultyType" runat="server" Text="Faculty Type *" SkinID="CaptionLabel"></asp:Label></td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" SkinID="txt248" MaxLength="50"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtPin" runat="server" SkinID="txt248" MaxLength="6" oninput="this.value = this.value.replace(/[^0-9]/g, '');"></asp:TextBox>

                    </td>
                     

                    <td>
                        <asp:DropDownList ID="ddlFacultyType" runat="server" SkinID="ddl250" MaxLength="50">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Contractual" Value="Contractual"></asp:ListItem>
                            <asp:ListItem Text="Regular" Value="Regular"></asp:ListItem>
                            <asp:ListItem Text="Outsourced" Value="Outsourced"></asp:ListItem>
                        </asp:DropDownList>

                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblPrevCompany" runat="server" Text="Previous Company" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblExperience" runat="server" Text="Experience (Years)" SkinID="CaptionLabel"></asp:Label></td>
                    <td>

                        <asp:Label ID="lblQualification" runat="server" Text="Highest Qualification" SkinID="CaptionLabel"></asp:Label></td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:TextBox ID="txtPrevCompany" runat="server" SkinID="txt248" MaxLength="100"></asp:TextBox></td>
                     <%--<td>
                        <asp:TextBox ID="txtExperience" runat="server" SkinID="txt248" MaxLength="50"></asp:TextBox></td>--%>


                    <td>
                        <asp:TextBox ID="txtExperience" runat="server" SkinID="txt248" MaxLength="2" oninput="this.value = this.value.replace(/[^0-9]/g, '');"></asp:TextBox>

                    </td>

                    <td>

                        <asp:TextBox ID="txtQualification" runat="server" SkinID="txt248" MaxLength="100"></asp:TextBox></td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblSkills" runat="server" Text="Skills" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblFacultyCode" runat="server" Text="Faculty Code *" SkinID="CaptionLabel"></asp:Label></td>
                    <td>
                        <asp:Label ID="lblEffectiveFrom" runat="server" Text="Effective From * (dd-mm-yyyy)" SkinID="CaptionLabel"></asp:Label></td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:TextBox ID="txtSkills" runat="server" SkinID="txt248" MaxLength="200"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtFacultyCode" runat="server" SkinID="txt248" MaxLength="50"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtEffectiveFrom" runat="server" SkinID="txt248"></asp:TextBox>
                        <asp:CalendarExtender ID="calEffectiveFrom" runat="server" TargetControlID="txtEffectiveFrom" Format="yyyy-MM-dd"></asp:CalendarExtender>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblEffectiveTo" runat="server" Text="Effective To (dd-mm-yyyy)" SkinID="CaptionLabel"></asp:Label></td>
                    <td colspan="2"></td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:TextBox ID="txtEffectiveTo" runat="server" SkinID="txt248"></asp:TextBox>
                        <asp:CalendarExtender ID="calEffectiveTo" runat="server" TargetControlID="txtEffectiveTo" Format="yyyy-MM-dd"></asp:CalendarExtender>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>

            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return ValidateFormFields();" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
