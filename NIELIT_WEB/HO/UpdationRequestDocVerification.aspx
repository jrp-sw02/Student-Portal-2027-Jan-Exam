<%@ Page Title="Document Verification for Candidate Updation Request" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="UpdationRequestDocVerification.aspx.cs" Inherits="HO_UpdationRequestDocVerification"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Document Verification for Candidate Updation Request"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <%--<asp:Panel runat="server" ID="pnlFilter">
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
    </asp:Panel>--%>
    <%--<uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Updation Detail "
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb2" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">

    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
          <%--  <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 50%;" valign="top">
                        <asp:Label ID="LabelCourseName"
                            runat="server"
                            SkinID="CaptionLabel"
                            Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:Label>
                    </td>

                    <td style="width: 50%;" valign="top">
                        <asp:Label ID="LabelAnnexure"
                            runat="server"
                            SkinID="CaptionLabel"
                            Text="Annexure &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:Label>
                    </td>
                </tr>

                <tr class="even">
                    <td valign="top">
                        <asp:DropDownList ID="ddlCourse"
                            runat="server"
                            Width="220px"
                            Height="22px">
                        </asp:DropDownList>
                    </td>

                    <td valign="top">
                        <asp:DropDownList ID="ddlAnnexure"
                            runat="server"
                            Width="220px"
                            Height="22px">

                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Annexure 1</asp:ListItem>
                            <asp:ListItem Value="2">Annexure 2</asp:ListItem>
                            <asp:ListItem Value="3">Annexure 3</asp:ListItem>
                            <asp:ListItem Value="4">Annexure 4</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                </tr>
            </table>--%>

  <%--          <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnView" runat="server"
                    Text="Show Data"
                    OnClick="btnView_Click" />

                &nbsp;&nbsp;&nbsp;

        <asp:Button ID="btnReset"
            runat="server"
            Text="Reset"
            OnClick="btnReset_Click" />
            </div>--%>
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
                        <asp:GridView ID="gvMain" runat="server" Width="100%"
                            AutoGenerateColumns="False"
                            DataKeyNames="requestNo"
                            AllowSorting="True"
                            OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound"
                            EmptyDataText="No record found.">
                            <Columns>

                                <%-- Serial Number --%>
                                <asp:BoundField DataField="SNo" HeaderText="#" ReadOnly="true">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle Width="5%" />
                                </asp:BoundField>

                                <%-- Candidate ID --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="requestNo"
                                    DataNavigateUrlFormatString="UpdationRequestDocVerification.aspx?Key={0}&Back=1"
                                    DataTextField="CandID"
                                    HeaderText="Candidate ID"
                                    SortExpression="CandID">
                                    <HeaderStyle Width="8%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <%-- Request Number --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="requestNo"
                                    DataNavigateUrlFormatString="UpdationRequestDocVerification.aspx?Key={0}&Back=1"
                                    DataTextField="requestNo"
                                    HeaderText="Request Number"
                                    SortExpression="requestNo">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <%-- Update Field --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="requestNo"
                                    DataNavigateUrlFormatString="UpdationRequestDocVerification.aspx?Key={0}&Back=1"
                                    DataTextField="updateFieldID"
                                    HeaderText="Request Details"
                                    SortExpression="updateFieldID">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <%-- Update Value --%>
                                <asp:HyperLinkField
                                    DataNavigateUrlFields="requestNo"
                                    DataNavigateUrlFormatString="UpdationRequestDocVerification.aspx?Key={0}&Back=1"
                                    DataTextField="newValue"
                                    HeaderText="Requested Updation"
                                    SortExpression="newValue">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                                <%-- Requested date --%>
                                <asp:BoundField DataField="enterDate" HeaderText="Request Date"
                                    SortExpression="enterDate" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="12%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>


                                <asp:HyperLinkField
                                    HeaderText="Status"
                                    DataTextField="Status"
                                    DataNavigateUrlFields="requestNo"
                                    DataNavigateUrlFormatString="UpdationRequestDocVerification.aspx?Key={0}&Back=1">
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID"
                            runat="server" />
                        <asp:HiddenField ID="hfcode"
                            runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="divNavigation" runat="server">
                <asp:UpdatePanel ID="uPnlNavigation" RenderMode="Inline"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:PagingBar
                            ID="PagingBar1"
                            runat="server"
                            OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>

        <!-- VIEW 2  -->

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

            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td><b>Candidate ID :</b></td>
                    <td>
                        <asp:Label ID="lblCandidateID" runat="server" />
                    </td>

                    <td><b>Request No :</b></td>
                    <td>
                        <asp:Label ID="lblRequestNo" runat="server" />
                    </td>
					<td><b>Annexure Number :</b></td>
                    <td>
                        <asp:Label ID="lblAnnexureNumber" runat="server" />
                    </td>               
                     </tr>
            </table>

            <br />

            <asp:GridView ID="gvDocuments"
                runat="server"
                AutoGenerateColumns="False"
                Width="100%"
                DataKeyNames="ID"
                OnRowCommand="gvDocuments_RowCommand">
                <Columns>
                    <asp:BoundField
                        DataField="UpdateField"
                        HeaderText="Request Details" />
                    <asp:BoundField
                        DataField="UpdateValue"
                        HeaderText="Requested Updation" />

                    <%--<asp:BoundField
                        DataField="FileName"
                        HeaderText="Document Name" />

                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkDocument"
                                runat="server"
                                Text="View Document"
                                NavigateUrl='<%# Eval("FilePath") %>'
                                Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkDocument"
                                runat="server"
                                Text='<%# Eval("FileName") %>'
                                NavigateUrl='<%# Eval("FilePath") %>'
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
                DataKeyNames="ID"
                EmptyDataText="No verified documents found.">

                <Columns>

                    <asp:BoundField
                        DataField="UpdateField"
                        HeaderText="Request Details" />

                    <asp:BoundField
                        DataField="FileName"
                        HeaderText="Document Name" />

                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkVerifiedDocument"
                                runat="server"
                                Text="View Document"
                                NavigateUrl='<%# Eval("FilePath") %>'
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
            <asp:Button ID="btnBack"
                runat="server"
                Text="Back"
                OnClick="btnBack_Click" />
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






























