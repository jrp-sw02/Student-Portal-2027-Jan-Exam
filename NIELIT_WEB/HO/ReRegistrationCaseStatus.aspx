<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ReRegistrationCaseStatus.aspx.cs" Inherits="HO_ReRegistrationCaseStatus" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Re Registration Case Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <%--<uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />--%>
    <asp:Panel runat="server" ID="pnlFilter">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <%-- Filter Panel content starts here --%>
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
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Applicant Type"></asp:Label>
                                        <asp:DropDownList ID="ddlfilterapplicanttype" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Course Level"></asp:Label>
                                        <asp:DropDownList ID="ddlfilterCourseLevel" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Width="100%" runat="server" Text="Registration Type"></asp:Label>
                                        <asp:DropDownList ID="ddlfilterRegistrationType" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- Filter Panel content ends here --%>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Education Qualification Name or Code"
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
 <%--           if (!isBlank("<%=txtCode.ClientID %>", "Code"))
                return false;
            if (!isBlank("<%=txtEdu.ClientID %>", "Education Qualification"))
                return false;
            if (!isBlank("<%=txtDisplayOrder.ClientID %>", "Display Order"))
                return false;
            if (!isNumber("<%=txtDisplayOrder.ClientID %>", "Display Order"))
                return false;
            if (!isSelected("<%=ddlQualificationLevel.ClientID %>", "Qualification Level"))
                return false;--%>
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

                                <asp:BoundField HeaderText="#">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="Registration_No"
                                    DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Name"
                                    HeaderText="Candidate Name"
                                    SortExpression="Name"
                                    Target="_self">
                                    <HeaderStyle Width="30%" HorizontalAlign="Center" />
                                    <ItemStyle Width="30%" HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="Registration_No"
                                    DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Registration_No"
                                    HeaderText="Registration Num"
                                    SortExpression="Registration_No"
                                    Target="_self">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="Registration_No"
                                    DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Valid_Upto_Date"
                                    HeaderText="Valid Upto Date"
                                    SortExpression="Valid_Upto_Date"
                                    Target="_self">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="Registration_No"
                                    DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="SplRegType"
                                    HeaderText="Spl Reg Type"
                                    SortExpression="MercyRegType"
                                    Target="_self">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField
                                    DataNavigateUrlFields="Registration_No"
                                    DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ApplicantType"
                                    HeaderText="Applicant Type"
                                    SortExpression="ApplicantType"
                                    Target="_self">
                                    <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                </asp:HyperLinkField>

                            </Columns>
                            <%--                            <PagerSettings Visible="False" />--%>
                        </asp:GridView>
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

        <%-- VIEW 2 --%>

                <asp:View ID="New" runat="server">
            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Green" EnableViewState="false" />

            <br />
            <asp:Label ID="LabelNote" runat="server" ForeColor="black" Style="font-size: 14px;">
            <b>Please Read Carefully:</b><br />
            &bull; If any document requires further review, the entire request should be placed On Hold. In such cases, do not verify or reject any document. Click the 'Hold Request' button.<br />
            &bull; If one or more documents are found to be incorrect and are rejected, carefully review the request before clicking the Final Submit button.
            </asp:Label>

            <br />
            <br />


            <br />

            <asp:GridView ID="gvDocuments"
                runat="server"
                AutoGenerateColumns="False"
                Width="100%"
                DataKeyNames="candidate_id"
                OnRowCommand="gvDocuments_RowCommand">
                <Columns>
                    <asp:BoundField
                        DataField="Doc_Type"
                        HeaderText="Document Type" />

                

                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkDocument"
                                runat="server"
                                Text="View"
                                NavigateUrl='<%# "~/handlers/ViewMercyDoc.ashx?id=" + Eval("candidate_id") %>'
                                Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Verification">
                        <ItemTemplate>

                            <asp:Button ID="btnVerify"
                                runat="server"
                                Text='<%# Convert.ToBoolean(Eval("docsVerified")) ? "Verified" : "Verify" %>'
                                CommandName="Verify"
                                CommandArgument='<%# Eval("ID") %>'
                                Enabled='<%# !Convert.ToBoolean(Eval("docsVerified")) %>' />

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Rejection">
                        <ItemTemplate>

                            <asp:Button ID="btnReject"
                                runat="server"
                                Text='<%# Convert.ToBoolean(Eval("docsRejected")) ? "Rejected" : "Reject" %>'
                                CommandName="Reject"
                                CommandArgument='<%# Eval("ID") %>'
                                Enabled='<%# !Convert.ToBoolean(Eval("docsRejected")) %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="Hold">
                        <ItemTemplate>
                            <asp:Button ID="btnHold"
                                runat="server"
                                Text='<%# Convert.ToBoolean(Eval("docsHold")) ? "On Hold" : "Hold" %>' 
                                CommandName="Hold" 
                                CommandArgument='<%# Eval("ID") %>' Enabled='<%# !Convert.ToBoolean(Eval("docsHold")) %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remark">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemark"
                                runat="server"
                                Text='<%# (Convert.ToInt32(Eval("requestStatusID")) == 13 || Convert.ToInt32(Eval("requestStatusID")) == 10) ? Eval("Remark") : "" %>'
                                Width="150px"
                                TextMode="MultiLine"
                                Rows="2"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <br />

            <asp:Label ID="LabelReject" runat="server" ForeColor="Red" Style="font-size: 14px;">
            <b>Note:</b><br />
            &bull; In case any document is found to be incorrect or invalid, please enter the reason for rejection in the <b>Remark</b> field and then click the <b>Reject</b> button to reject that document.<br />
            &bull; Please ensure that the remarks entered are clear, specific, and appropriate, as they will be communicated to the candidate for necessary corrective action.
            </asp:Label>

            <br />


            <h3 style="color: #006699; margin-bottom: 8px;">Verified/Rejected Records </h3>

            <asp:GridView ID="gvVerifiedDocuments"
                runat="server"
                AutoGenerateColumns="False"
                Width="100%"
                DataKeyNames="candidate_id"
                EmptyDataText="No verified documents found.">

                <Columns>

                    <asp:BoundField
                        DataField="Doc_type"
                        HeaderText="Document Type" /> 

                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkVerifiedDocument"
                                runat="server"
                                Text="👁"
                                NavigateUrl='<%# "../Handlers/ViewMercyDoc.ashx?id=" + Eval("candidate_id") %>'
                                Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus"
                                runat="server"
                                Text="Verified"
                                ForeColor="Green"
                                Font-Bold="true" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus"
                                runat="server"
                                Text='<%# Eval("VerificationStatus") %>'
                                ForeColor='<%# Eval("VerificationStatus").ToString() == "Rejected" ? System.Drawing.Color.Red : System.Drawing.Color.Green %>'
                                Font-Bold="true" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField
                        DataField="docsVerifiedOn"
                        HeaderText="Verified On"
                        DataFormatString="{0:dd-MMM-yyyy HH:mm}" />

                </Columns>

            </asp:GridView>
            <br />
<%--            <asp:Button ID="btnBack"
                runat="server"
                Text="Back"
                OnClick="btnBack_Click" />--%>
            &nbsp;&nbsp;
            <asp:Button ID="btnFinalSubmit"
                Visible="false"
                runat="server"
                Text="Final Submit"
                OnClick="btnFinalSubmit_Click"
                OnClientClick="return confirm('No further changes can be made once Final Submit is done.\n\nClick OK to proceed or Cancel to stay on this page.');"/>
            <%--&nbsp;&nbsp;
            <asp:Button ID="BtnKeepOnHold"
                runat="server"
                Text="Hold Request"
                OnClick="BtnKeepOnHold_Click" />--%>
        </asp:View>

    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
