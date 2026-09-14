<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    Debug="false" CodeFile="RejectedRegistrationApplications.aspx.cs" Inherits="RejectedRegistrationApplications"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Kept In Abeyance Applications"></asp:Label>
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
                                            Text="Select"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            OnClientClick="return ValidateForm();" Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Candidate Type"></asp:Label>
                                        <asp:DropDownList ID="ddlCandidateType" Width="240px" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter2" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlCourse" Width="240px" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" Width="100%" runat="server" Text="Application Status"></asp:Label>
                                        <asp:DropDownList ID="ddlApplStatus" runat="server" Width="240px" OnSelectedIndexChanged="ddlApplStatus_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="K">Kept In Abeyance</asp:ListItem>
                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
                                            <asp:ListItem Value="V">Verified</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilter3" Width="100%" runat="server" Text="Batch Number"></asp:Label>
                                        <asp:DropDownList ID="ddlBatchNumber" Width="240px" runat="server" OnSelectedIndexChanged="ddlBatchNumber_SelectedIndexChanged">
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
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function TestCheckBox() {
            var TargetBaseControl = document.getElementById('<%= gvMain.ClientID %>');
            if (TargetBaseControl != null) {
                //get target child control.
                var TargetChildControl = "chk";
                //get all the control of the type INPUT in the base control.
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                for (var n = 0; n < Inputs.length; ++n)
                    if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked)
                        return true;
        }
        alert('Select at least one checkbox!');
        return false;
    }



    function ValidateForm() {

        if (!isSelected("<%=ddlCandidateType.ClientID %>", "Candidate Type"))
            return false;
        if (!isSelected("<%=ddlCourse.ClientID %>", "Course Name"))
            return false;
        if (!isSelected("<%=ddlBatchNumber.ClientID %>", "Batch Number"))
            return false;
        return true;

    }
    function Rejected() {
        if (!isBlank("<%=txtReason.ClientID %>", "Rejected Reason"))
            return false;
        if (document.getElementById("<%=txtReason.ClientID %>").value != "") {
            if (!confirm("Are you sure you want to Reject this Application!")) {
                return false;
            }
            else
            { return true; }
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
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
                        Width="99%"></asp:Label>
                    <div id="divGrid" runat="server">
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplTypeID" DataNavigateUrlFormatString="?batchItemID={0}&ApplTypeID={1}"
                                    DataTextField="AppNo" HeaderText="Application No." SortExpression="AppNo" Target="_self">
                                    <ItemStyle HorizontalAlign="Right" Width="15%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplTypeID" DataNavigateUrlFormatString="?batchItemID={0}&ApplTypeID={1}"
                                    DataTextField="AppDate" HeaderText="Application Date" SortExpression="AppDate"
                                    Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplTypeID" DataNavigateUrlFormatString="?batchItemID={0}&ApplTypeID={1}"
                                    DataTextField="CandidateName" HeaderText="Candidate Name" SortExpression="CandidateName"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplTypeID" DataNavigateUrlFormatString="?batchItemID={0}&ApplTypeID={1}"
                                    DataTextField="Course" HeaderText="Course" SortExpression="Course" Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID,ApplTypeID" DataNavigateUrlFormatString="?batchItemID={0}&ApplTypeID={1}"
                                    DataTextField="CandidateType" HeaderText="Candidate Type" SortExpression="CandidateType"
                                    Target="_self" />
                                <%--<asp:TemplateField HeaderStyle-Width="3%" HeaderText="">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                    </div>
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
            <br />
        </asp:View>
        <asp:View ID="View1" runat="server">
        </asp:View>
        <asp:View ID="New" runat="server">
            <div class="box" style="margin-bottom: 0px;">
                <uc4:CourseApplication ID="candidateDetail" runat="server" style="margin-top: 0px;" />
            </div>
            <asp:Label ID="lblMsg" runat="server" Style="margin-top: 20px" ForeColor="Red"></asp:Label>
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <div id="divDefiencyDetail" runat="server" style="text-align: right; margin-top: 10px"
                class="box">
                <table style="width: 100%;" class="sample3" cellpadding="2" cellspacing="1">
                    <tr class="head1">
                        <td align="left">
                            Kept In Abeyance Detail
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td align="left" style="font-weight: bold; font-size: 12px;">
                            Deficiency Codes<font color='RED'>*</font>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left">
                            <asp:CheckBoxList ID="chklistCodes" runat="server">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td align="left" style="font-weight: bold; font-size: 12px;">
                            Deficiency Description(Maximum 500 Characters)
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left">
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" Width="800px" Height="90px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="gdrow1" runat="server" visible="false" id="trRejectedReason1">
                        <td align="left" style="font-weight: bold; font-size: 12px;">
                            Rejected Reason(Maximum 500 Characters)<font color='RED'>*</font>
                        </td>
                    </tr>
                    <tr class="gdalternate1" runat="server" visible="false" id="trRejectedReason2" >
                        <td align="left">
                            <asp:TextBox ID="txtReason" runat="server" MaxLength="500" Width="800px" Height="90px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Div1" runat="server" style="text-align: right; margin-top: 10px; margin-bottom: 6px;">
                <asp:Button ID="btnVerify" runat="server" Text="Verify" Visible="false" OnClick="btnVerify_Click"
                    OnClientClick="return Validate_Checkbox('Are you sure you want to Verify this Application!')" />
                <asp:Button ID="btnReject" runat="server" Text="Reject" Visible="false" OnClick="btnReject_Click"
                    OnClientClick="return Rejected()" />
                <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
