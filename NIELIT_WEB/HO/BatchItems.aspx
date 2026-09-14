<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="BatchItems.aspx.cs" Inherits="BatchItems" Debug = "true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/DuplicateRecords.ascx" TagName="DuplicateRecords"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Batch Applications"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Application Status"></asp:Label>
                                        <asp:DropDownList ID="ddlStatus" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        var dtga = "<%= gbbatch.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtga, Sender, CheckBoxName)
        }
        //        var dtga = "<%= gbbatch.ClientID %>"
        //        function CheckAll(Sender, CheckBoxName) {
        //            CheckUncheckAll(dtga, Sender, CheckBoxName)
        //        }

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


        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
//        function PerformAction(obj, tableid) {
//            document.getElementById("<%=hfActionID1.ClientID %>").value = obj.id.split("_")[1];
//            ShowHideMenu(obj, tableid);
//        }
    </script>
    <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblErrorMsg" runat="server" EnableTheming="False" CssClass="error"
                                Visible="False" Width="99%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="bottom">
                            <%--<asp:TextBox Style="float: left" ID="txtappno" runat="server" Width="250px" MaxLength="30"
                                Visible="False"></asp:TextBox>--%>
                            <asp:Button ID="btnsubmit" Style="float: right; margin-left: 3px;"
                                runat="server" OnClick="btnsubmit_Click" OnClientClick="return Validate_Checkbox('Are you sure you want to scan selected applications!')"
                                Text="Add Applicants" />
                            <%--<asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtappno"
                                WatermarkText="Enter Application No./Scan BarCode">
                            </asp:TextBoxWatermarkExtender>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gbapplicant" runat="server" AutoGenerateColumns="False" DataKeyNames="Appno"
                         OnRowDataBound="gbapplicant_RowDataBound" Width="100%" HeaderStyle-Font-Size="11px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Status,ApplTypeID,Appno" DataNavigateUrlFormatString="?ID={0}&Status={1}&ApplTypeID={2}&Appno={3}"
                                DataTextField="Appno" HeaderText="Appl.No." Target="_self">
                                <HeaderStyle Width="9%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Status,ApplTypeID,Appno" DataNavigateUrlFormatString="?ID={0}&Status={1}&ApplTypeID={2}&Appno={3}"
                                DataTextField="Appdate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Appl.Date"
                                Target="_self">
                                <HeaderStyle Width="13%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Status,ApplTypeID,Appno" DataNavigateUrlFormatString="?ID={0}&Status={1}&ApplTypeID={2}&Appno={3}"
                                DataTextField="Name" HeaderText="CandidateName" Target="_self">
                                <HeaderStyle Width="17%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Status,ApplTypeID,Appno" DataNavigateUrlFormatString="?ID={0}&Status={1}&ApplTypeID={2}&Appno={3}"
                                DataTextField="PaymentMode" HeaderText="PaymentMode" Target="_self">
                                <HeaderStyle Width="16%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Status,ApplTypeID,Appno" DataNavigateUrlFormatString="?ID={0}&Status={1}&ApplTypeID={2}&Appno={3}"
                                DataTextField="PaymentDetails" HeaderText="PaymentDetails" Target="_self">
                                <HeaderStyle Width="19%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,Status,ApplTypeID,Appno" DataNavigateUrlFormatString="?ID={0}&Status={1}&ApplTypeID={2}&Appno={3}"
                                DataTextField="PaymentStatus" HeaderText="PaymentStatus" Target="_self">
                                <HeaderStyle Width="18%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:TemplateField HeaderStyle-Width="1%" HeaderText="">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkheader" runat="server"  onclick="javascript:SelectheaderCheckboxes(this)" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkchild" runat="server" onclick="javascript:Selectchildcheckboxes(chkheader)" /></ItemTemplate>
                                <HeaderStyle Width="1%" />
                            </asp:TemplateField>
                        </Columns>
                       <%-- <EmptyDataTemplate>No Applicant to Add in the Batch</EmptyDataTemplate>--%>
                    </asp:GridView>
                    <asp:HiddenField ID="HiddenField2" runat="server" Value="" />
                    <asp:HiddenField ID="hfActionID1" runat="server" Value="" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="divNavigation1" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged2" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />
            <br />
            <table width="100%">
                <tr>
                    <td align="right" valign="bottom">
                        <asp:Button ID="btnMasNotReject" runat="server" Text="Mark As Not Rejected" OnClientClick="return Validate_Checkbox()"
                            Visible="false" OnClick="btnMasNotReject_Click" />
                        <asp:Button ID="btnMprocessed" runat="server" Text="Mark As Processed" OnClientClick="return Validate_Checkbox()"
                            Visible="false" OnClick="btnMprocessed_Click" />
                        <asp:Button ID="btnVerify" runat="server" Text="Verify Application Form" Visible="false"
                            OnClientClick="return Validate_Checkbox('Are you sure you want to verify selected applications!')"
                            OnClick="btnVerify_Click" />
                        <asp:Button ID="btnVerifyDD" Visible="false" runat="server" Text="Verify Payment Details"
                            OnClientClick="return Validate_Checkbox('Are you sure you want to verify demand draft / NEFT transaction details of selected applications!')"
                            OnClick="btnVerifyDD_Click" />
                        <asp:Button Visible="false" ID="btnKeepInAbeyance" runat="server" Text="Keep In Abeyance"
                            OnClientClick="return Validate_Checkbox('Are you sure you want to Keep In Abeyance selected applications!')"
                            OnClick="btnKeepInAbeyance_Click" />
                        <asp:Button Visible="false" ID="btnReject" runat="server" Text="Reject" OnClientClick="return Validate_Checkbox('Are you sure you want to reject selected applications!')"
                            OnClick="btnReject_Click" />
                        <asp:Button Visible="false" ID="btnDelete" runat="server" Text="Delete" OnClientClick="return Validate_Checkbox('Are you sure you want to delete selected applications!')"
                            OnClick="btnDelete_Click" />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gbbatch" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                        OnRowDataBound="gbbatch_RowDataBound" Width="100%" HeaderStyle-Font-Size="11px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="Appno" HeaderText="Appl.No." Target="_self">
                                <HeaderStyle Width="9%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="Appdate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Appl.Date"
                                Target="_self">
                                <HeaderStyle Width="13%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="Name" HeaderText="CandidateName" Target="_self">
                                <HeaderStyle Width="17%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="PaymentMode" HeaderText="PaymentMode" Target="_self">
                                <HeaderStyle Width="16%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="PaymentDetails" HeaderText="PaymentDetails" Target="_self">
                                <HeaderStyle Width="19%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="PaymentStatus" HeaderText="PaymentStatus" Target="_self">
                                <HeaderStyle Width="18%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,BatchID,status,ApplTypeID,AppID" DataNavigateUrlFormatString="?batchItemID={0}&BatchID={1}&status={2}&ApplTypeID={3}&Appno={4}"
                                DataTextField="dupcount" HeaderText="Duplicates" Target="_self">
                                <HeaderStyle Width="1%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:TemplateField HeaderStyle-Width="1%" HeaderText="">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hllnkapp">Link</asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle Width="1%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="1%" HeaderText="">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView"  />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView"  />
                                </ItemTemplate>
                                <HeaderStyle Width="1%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                    <asp:HiddenField ID="hfActionID" runat="server" Value="" />
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
        </asp:View>

        <asp:View ID="New" runat="server">            
            <div>
                <uc4:CourseApplication ID="candidateDetail" runat="server" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="UpnlShow" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="3" cellspacing="1" class="sample3" style="width: 100%;
                text-align: left" id="tblShow" runat="server">
                <tr class="head1">
                    <td width="80%">
                        Application Status
                    </td>
                    <td width="20%">
                        Count
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td>
                        Received But Pending For Payment Verification
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hlkpendingDDVerify" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td>
                        Application Received
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hlkreceived" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td>
                        Application Rejected
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hlkreject" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td>
                        Application Kept In Abeyance
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hlKeptInAbeyance" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td>
                        Application Duplicate
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hlkduplicate" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td>
                        Application Verified
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hlkprocess" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td>
                        Total Application
                    </td>
                    <td>
                        <asp:Label runat="server" ID="hktotal" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divduplicaterecords" runat="server" visible="false">
        <uc6:DuplicateRecords ID="DuplicateRecords1" runat="server" />
    </div>
</asp:Content>
