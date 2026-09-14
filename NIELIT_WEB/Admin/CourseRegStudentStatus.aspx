<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CourseRegStudentStatus.aspx.cs" Inherits="Admin_CourseRegStudentStatus"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Application Status"></asp:Label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" runat="server" Visible="False" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
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
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        var dtgp = "<%= gbbatch.ClientID %>"
        var dtgp1 = "<%= gvMain.ClientID %>"



        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=gvMain.ClientID %>");
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
            var gvcheck = document.getElementById("<%=gvMain.ClientID %>");
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
    </script>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
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
                            <asp:Button ID="btnVerify" runat="server" Text="Mark As Verified" Visible="False"
                                OnClientClick="return Validate_Checkbox('Are you sure you want to verify selected applications!')"
                                OnClick="btnVerify_Click" />
                            <asp:Button ID="btnMNotVerify" runat="server" Text="Mark As Not Verified" Visible="False"
                                OnClientClick="return Validate_Checkbox('Are you sure you want to cancel/revert verification of selected applications!')"
                                OnClick="btnMNotVerify_Click" />
                            <asp:Button ID="btnCPayment" runat="server" Text="Make Payment" Visible="False" OnClientClick="return Validate_Checkbox('Are you sure you want to make payment of all applications!')"
                                OnClick="btnCPayment_Click" />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" Visible="true" OnClientClick="return Validate_Checkbox('Are you sure you want to reject selected applications!')"
                                OnClick="btnReject_Click" />
                            <asp:Button ID="btnShowDemandNote" Visible="false" runat="server" Text="Show Demand Note"
                                OnClick="btnShowDemandNote_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gbbatch" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                Width="100%" OnRowDataBound="gbbatch_RowDataBound" OnSorting="gbbatch_Sorting">
                                <Columns>
                                    <asp:BoundField HeaderText="#">
                                        <HeaderStyle Width="1%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID,CourseCategoryID,ServiceID,ApplTypeID,ApplicationStatusID"
                                        DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&CourseCategoryID={2}&ServiceID={3}&ApplTypeID={4}&Status={5}"
                                        DataTextField="Appno" HeaderText="Appl. No." SortExpression="Appno" Target="_self">
                                        <HeaderStyle Width="12%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID,CourseCategoryID,ServiceID,ApplTypeID,ApplicationStatusID"
                                        DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&CourseCategoryID={2}&ServiceID={3}&ApplTypeID={4}&Status={5}"
                                        DataTextField="Appdate" DataTextFormatString="{0:dd-MMM-yyyy}" HeaderText="Appl. Date"
                                        SortExpression="Appdate" Target="_self">
                                        <HeaderStyle Width="18%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID,CourseCategoryID,ServiceID,ApplTypeID,ApplicationStatusID"
                                        DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&CourseCategoryID={2}&ServiceID={3}&ApplTypeID={4}&Status={5}"
                                        DataTextField="Name" HeaderText="Name" SortExpression="Name" Target="_self">
                                        <HeaderStyle Width="19%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID,CourseCategoryID,ServiceID,ApplTypeID,ApplicationStatusID"
                                        DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&CourseCategoryID={2}&ServiceID={3}&ApplTypeID={4}&Status={5}"
                                        DataTextField="PaymentMode" HeaderStyle-Width="40%" HeaderText="Pay. Mode" SortExpression="PaymentMode"
                                        Target="_self">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID,CourseCategoryID,ServiceID,ApplTypeID,ApplicationStatusID"
                                        DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&CourseCategoryID={2}&ServiceID={3}&ApplTypeID={4}&Status={5}"
                                        DataTextField="PaymentStatus" HeaderStyle-Width="40%" HeaderText="Pay. Status"
                                        SortExpression="PaymentStatus" Target="_self">
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:HyperLinkField>
                                    <asp:HyperLinkField DataNavigateUrlFields="ID,CourseID,CourseCategoryID,ServiceID,ApplTypeID,ApplicationStatusID"
                                        DataNavigateUrlFormatString="?ApplID={0}&CourseID={1}&CourseCategoryID={2}&ServiceID={3}&ApplTypeID={4}&Status={5}"
                                        DataTextField="PaymentDetails" HeaderStyle-Width="40%" HeaderText="Payment Details"
                                        SortExpression="PaymentDetails" Target="_self">
                                        <HeaderStyle Width="19%" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:HyperLinkField>
                                    <asp:TemplateField HeaderText="Course Duration">
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFmonth" runat="server">
                                                            <asp:ListItem Text="Month" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFyear" runat="server" Style="width: 30px;" MaxLength="4" onkeypress="checkNumber(this,10,0,event)"></asp:TextBox>
                                                        <asp:TextBoxWatermarkExtender WatermarkText="Year" WatermarkCssClass="watermark"
                                                            TargetControlID="txtFyear" ID="TextBoxWatermarkExtender2" runat="server">
                                                        </asp:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        To
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTmonth" runat="server">
                                                            <asp:ListItem Text="Month" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTyear" runat="server" Style="width: 30px;" MaxLength="4" onkeypress="checkNumber(this,4,0,event)"></asp:TextBox>
                                                        <asp:TextBoxWatermarkExtender WatermarkCssClass="watermark" WatermarkText="Year"
                                                            TargetControlID="txtTyear" ID="TextBoxWatermarkExtender1" runat="server">
                                                        </asp:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <%--Added_UP_Project_02_09_2024--%>

                                    <asp:TemplateField HeaderText="Batch" runat="server" >
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBatch" runat="server" Visible="false"></asp:TextBox>
                                            <asp:DropDownList ID="ddlBatchId" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Project" runat="server">
                                        <ItemTemplate>
                                            <%--<asp:DropDownList ID="ddlProjectId" runat="server" Enabled="true" AutoPostBack="true">          
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtProject" runat="server" Visible="false"></asp:TextBox>
                                            <asp:DropDownList Width="300px" ID="ddlProjectId" runat="server" AutoPostBack="true"  Visible="false"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <%--Added_UP_Project_02_09_2024--%>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>


                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
            </div>
            <div id="divNavigation" runat="server">
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </div>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSubmits" runat="server" Text="Dispatch" Visible="False" OnClientClick="return Validate_Checkbox('Are you sure you want to dispatch selected applications  to the concerned centre!')"
                            OnClick="btnSubmits_Click" />
                        <asp:Button ID="btnShowAppl" runat="server" Text="Show All Applications" OnClick="btnShowAppl_Click"
                            Visible="false" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                OnRowDataBound="gvMain_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                        <HeaderStyle Width="2%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Demand Note No" DataField="DemandNo" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Date" DataField="DemandDate" DataFormatString="{0:dd-MMM-yyyy}"
                        HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Payment Mode" DataField="PaymentMode" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Payment Status" DataField="PaymentStatus" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="No of Applications" DataField="applCount" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Amount" DataField="Amount" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="3%" HeaderText="">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkheader" runat="server" onclick="javascript:SelectheaderCheckboxes(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkchild" runat="server" onclick="javascript:Selectchildcheckboxes()" />
                        </ItemTemplate>
                        <HeaderStyle Width="3%" />
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Visible="False" />
            </asp:GridView>
            <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged1"
                Visible="false" />
        </asp:View>
        <asp:View ID="New" runat="server">
            <div>
                <uc4:CourseApplication ID="CourseApplication1" runat="server" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
