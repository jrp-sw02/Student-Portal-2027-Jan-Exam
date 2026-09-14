<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CourseExamDataupload.aspx.cs"
    Inherits="CourseExamDataupload" MasterPageFile="~/MasterPages/main.master" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Upload Admit Card Data"></asp:Label>
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
                                            Text="" OnClick="AllyFilter" OnClientClick="return ValidateFilter()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" Width="100%" runat="server" Text="Course"></asp:Label>
                                        <asp:DropDownList ID="ddlflCourse" Width="100%" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlflCourse_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Exam Cycle"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExamCycle" Width="100%" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlflExamCycle_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflCourse" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Exam Year"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExamYear" Width="100%" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlflExamYear_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflExamCycle" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Name"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExam" Width="100%" runat="server">
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlflExamYear" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label18" Width="100%" runat="server" Text="Module Type"></asp:Label>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlmodulefilter" Width="100%" runat="server">
                                                    <asp:ListItem Value="0" Text="All"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Only Theory"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Only Practical"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Both Theory and Practical"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
    <asp:UpdatePanel EnableViewState="true" ID="upbreadsearch" UpdateMode="Conditional"
        runat="server">
        <ContentTemplate>
            <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Candidate Name or Registration Number or Roll Number"
                OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
                AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
                AutoCompleteCompletionSetCount="10" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function printStatus() {
            var WindowObject = window.open('', 'PrintWindow', 'width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(document.getElementById('divprint').innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            // window.print();
        }
        function Validate(msg) {
            if (ConfirmAction(msg))
                return true;
            else
                return false;
        }
        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
        function ValidateFilter() {

            if (!isSelected("<%=ddlflCourse.ClientID %>", "Course"))
                return false;
            if (!isSelected("<%=ddlflExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlflExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlflExam.ClientID %>", "Exam"))
                return false;

        }

        function ValidateFormFields() {


            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=Ddlmoduletype.ClientID %>", "Module Type"))
                return false;
            if (!isBlank("<%=flUpload.ClientID %>", "Browse File Upload"))
                return false;
            document.getElementById("imgProgress").style.display = "block";
            //            document.getElementById("<%=btnValidate.ClientID %>").disabled = true;
            //            return true;
        }

        function ValidateFormFields2() {
            if (!isSelected("<%=ddlexamcentre.ClientID %>", "Exam Centre"))
                return false;
            if (!isSelected("<%=ddlexamvenue.ClientID %>", "Exam Venue"))
                return false;
            if (!isBlank("<%=Txtrollno.ClientID %>", "Roll Number"))
                return false;
            if (!isNumber("<%=Txtrollno.ClientID %>", "Roll Number"))
                return false;
        }

        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
        function SampleFile() {
            window.open("../Download/Result.xls");
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
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseID,ExamID"
                                    DataNavigateUrlFormatString="?Key={0}&CourseID={1}&ExamID={2}" DataTextField="RegNo"
                                    HeaderText="Reg.No." SortExpression="RegNo" Target="_self"></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID,CourseID,ExamID"
                                    DataNavigateUrlFormatString="?Key={0}&CourseID={1}&ExamID={2}" DataTextField="appNo"
                                    HeaderText="Application No." SortExpression="appNo" Target="_self"></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,CourseID,ExamID"
                                    DataNavigateUrlFormatString="?Key={0}&CourseID={1}&ExamID={2}" DataTextField="appDate"
                                    HeaderText="Application Date" SortExpression="appDate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID,CourseID,ExamID"
                                    DataNavigateUrlFormatString="?Key={0}&CourseID={1}&ExamID={2}" DataTextField="Name"
                                    HeaderText="Candidate Name" SortExpression="Name" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID,CourseID,ExamID"
                                    DataNavigateUrlFormatString="?Key={0}&CourseID={1}&ExamID={2}" DataTextField="Father"
                                    HeaderText="Father/Guardian Name" SortExpression="Father" Target="_self"></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="7%" DataNavigateUrlFields="ID,CourseID,ExamID"
                                    DataNavigateUrlFormatString="?Key={0}&CourseID={1}&ExamID={2}" DataTextField="Rollno"
                                    HeaderText="Roll No" SortExpression="Rollno" Target="_self"></asp:HyperLinkField>
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
            <img id="imgProgress" style="position: absolute; left: 270px; top: 250px; display: none;"
                src="../images/loading.gif" />
            <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>--%>
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourse" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                    <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamCycle" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                                    <asp:ListItem>--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" style="width: 33%;">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Module Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33px;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="--Select One--">--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250">
                                    <asp:ListItem>--Select One-</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="Ddlmoduletype" runat="server" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One-</asp:ListItem>
                            <asp:ListItem Value="1">Theory</asp:ListItem>
                            <asp:ListItem Value="2">OnLine Theory</asp:ListItem>
                             <asp:ListItem Value="3">Practical</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.XLS/XLSX) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
                <tr class="even">
                    <td colspan="2">
                        <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                        <asp:HiddenField ID="flpath" runat="server" />
                    </td>
                    <td style="width: 33%;" valign="top">
                    </td>
                </tr>
            </table>
            <div id="divprint" runat="server">
                <table class="sample3" id="tblprint" style="width: 100%; text-align: left" border="0"
                    cellpadding="3" cellspacing="1">
                    <tr class="head1">
                        <td align="left" colspan="2">
                            Admit Card Details
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="40%">
                            Course Name
                        </td>
                        <td>
                            <asp:Label ID="lblCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="40%">
                            Exam Name
                        </td>
                        <td>
                            <asp:Label ID="lblExamDetail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="40%">
                            Application Number
                        </td>
                        <td>
                            <asp:Label ID="lblAppNumber" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="40%">
                            Application Date
                        </td>
                        <td>
                            <asp:Label ID="lblAppDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="40%">
                            Candidate Name
                        </td>
                        <td>
                            <asp:Label ID="lblCandidate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" runat="server" id="TrFatherName">
                        <td width="40%">
                            Father Name
                        </td>
                        <td>
                            <asp:Label ID="lblFather" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" runat="server" id="TrMotherName">
                        <td width="40%">
                            Mother Name
                        </td>
                        <td>
                            <asp:Label ID="lblMother" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" runat="server" id="TrGardianName">
                        <td width="40%">
                            Guardian Name
                        </td>
                        <td>
                            <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" id="trchangedetail" runat="server">
                        <td align="right" colspan="2">
                            <asp:LinkButton ID="Lnkcentrechange" runat="server" OnClick="Lnkcentrechange_Click"> Change Admit Card Details</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="head1" id="trtheory1" runat="server" visible="false">
                        <td align="left" colspan="2">
                            Theory Exam Admit Card Details
                        </td>
                    </tr>
                    <tr class="gdalternate1" id="trtheory2" runat="server" visible="false">
                        <td width="40%">
                            Roll Number
                        </td>
                        <td>
                            <asp:Label ID="lblRollno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" id="trtheory3" runat="server" visible="false">
                        <td width="38%">
                            Exam Centre Name
                        </td>
                        <td width="38%">
                            <asp:Label ID="lblExamCentreName" runat="server" Text="NA (As not Applied for Theory Examination)"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" id="trtheory4" runat="server" visible="false">
                        <td width="38%">
                            Exam Venue Name
                        </td>
                        <td width="38%">
                            <asp:Label ID="lblexamvenuecode" runat="server" Text="NA (As not Applied for Theory Examination)"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" id="trtheory5" runat="server" visible="false">
                        <td width="38%" valign="top">
                            Exam Venue Address
                        </td>
                        <td width="38%">
                            <asp:Label ID="lblAddress" runat="server" Text="NA (As not Applied for Theory Examination)"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1" id="trprac" runat="server" visible="false">
                        <td align="left" colspan="2">
                            Practical Exam Admit Card Details
                        </td>
                    </tr>
                    <tr class="gdrow1" id="trprac1" runat="server" visible="false">
                        <td width="38%" valign="top">
                            Institute Name
                        </td>
                        <td>
                            <asp:Label ID="lbpracticalinsname" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" id="trprac2" runat="server" visible="false">
                        <td width="38%" valign="top">
                            Institute Address
                        </td>
                        <td width="38%">
                            <asp:Label ID="Lbpracinstaddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divValidateData" runat="server" visible="false" height="150px" width="600px"
                style="overflow: scroll;">
                <table class="sample3" id="tblValidateData" style="width: 100%; text-align: left"
                    border="0" cellpadding="3" cellspacing="1">
                    <tr class="head1">
                        <td align="left" colspan="2">
                            Validated and Updated Data
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="40%" valign="top">
                            Total Records
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" style="display:none">
                        <td width="40%" valign="top">
                            Validate/Valid Records
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblValidateRecords" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="40%" valign="top">
                            Not Validated/Invalid Records
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblNotValidate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="40%" valign="top">
                            Invalid Records Details (Registration-Module Id)
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblFailed" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="40%" valign="top">
                            Updated Records Details
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblUpdated" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" style="display:none">
                        <td width="40%" colspan="2">
                            <asp:Label ID="Label12" runat="server" Text="Only Validated records can be uploaded successfully."
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnValidate" runat="server" Text="Validate/Save Data" OnClick="btnValidate_Click"
                    OnClientClick="return ValidateFormFields()" />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return ValidateFormFields()" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
            <div style="margin-top: 10px" id="divchangedata" visible="false" runat="server">
                <div style="margin-top: 10px" id="divradio" class="error">
                    Please select one option for which you want to change the details:-
                    <asp:RadioButtonList ID="rblOption" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                        OnSelectedIndexChanged="rblOption_SelectedIndexChanged">
                        <asp:ListItem Text="Theory Exam Admit Card" Value="T"></asp:ListItem>
                        <asp:ListItem Text="Practical Exam Admit Card" Value="P"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <table cellpadding="1" cellspacing="0" width="100%" class="sample3" border="0" id="tblTheoryData"
                    visible="false" runat="server">
                    <tr class="head1">
                        <td align="left" colspan="2">
                            Theory Exam Admit Card Change Details
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="20%">
                            Exam Centre:-
                        </td>
                        <td width="80%">
                            <asp:DropDownList ID="ddlexamcentre" runat="server" Width="607px" OnSelectedIndexChanged="ddlexamcentre_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="20%">
                            Exam Venue:-
                        </td>
                        <td width="80%">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlexamvenue" runat="server" Width="607px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlexamcentre" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="20%">
                            Roll Number:-
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="Txtrollno" runat="server" MaxLength="6" Width="595px" onkeypress="checkNumber(this,6,0,event);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="0" width="100%" class="sample3" border="0" id="tblPracticalData"
                    visible="false" runat="server">
                    <tr class="head1">
                        <td align="left" colspan="2">
                            Practical Exam Admit Card Change Details
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td width="20%">
                            Institute Name:-
                        </td>
                        <td width="80%">
                            <asp:DropDownList ID="ddlinstitutename" runat="server" Width="607px" OnSelectedIndexChanged="ddlinstitutename_SelectedIndexChanged1"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trinstitutehead" runat="server" visible="false" class="head1">
                        <td align="left" colspan="2">
                            New Institute Details
                        </td>
                    </tr>
                    <tr id="trinstitute1" runat="server" visible="false" class="gdrow1">
                        <td width="20%">
                            <asp:Label ID="Label14" runat="server" Text="Institute Name <font color='RED'>*</font>"></asp:Label>
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="Txtinstname" runat="server" Width="595px" MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trinstitute2" runat="server" visible="false" class="gdalternate1">
                        <td width="20%">
                            <asp:Label ID="Label15" runat="server" Text="Institute Address <font color='RED'>*</font>"></asp:Label>
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="Txtinstadd" runat="server" Width="595px" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trinstitute5" runat="server" visible="false" class="gdrow1">
                        <td>
                            <asp:Label ID="LblCity" runat="server" Text=" Institute City <font color='RED'>*</font>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtinstCity" runat="server" Width="245px" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trinstitute3" runat="server" visible="false" class="gdalternate1">
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="Institute State <font color='RED'>*</font>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged"
                                Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trinstitute6" runat="server" visible="false" class="gdrow1">
                        <td>
                            <asp:Label ID="Label17" runat="server" Text="Institute District <font color='RED'>*</font>"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddldistrict" runat="server" Width="250px">
                                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlstate" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trinstitute4" runat="server" visible="false" class="gdalternate1">
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Pin Code <font color='RED'>*</font>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txtinstpincode" runat="server" oncopy="return false" oncut="return false"
                                onkeypress="checkNumber(this,6,0,event);" onpaste="return false" Width="93px"
                                MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td width="20%">
                            Practical Office Ref.No:-
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="Txtpracofficerefno" runat="server" MaxLength="15" Width="245px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class= "trgdalternate1calendar">
                        <td colspan="2">
                            <asp:GridView ID="gvpracexam" runat="server" OnRowDataBound="gvpracexam_RowDataBound"
                                AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:BoundField HeaderText="#" HeaderStyle-Width="2%"></asp:BoundField>
                                    <asp:BoundField DataField="modulename" HeaderText="LEVEL / PR-CODE" HeaderStyle-Width="15%">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="BATCH" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtbatchno" runat="server" MaxLength="2" Text='<%# Eval("batch") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="DATE" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtpracexamdate" runat="server" Width="120px" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("practicaldate")).ToString("dd-MMM-yyyy")  %>'></asp:TextBox>
                                            <asp:CalendarExtender ID="ceDOB" TargetControlID="txtpracexamdate" 
                                                Format="dd-MMM-yyyy" PopupButtonID="txtpracexamdate" runat="server" PopupPosition="TopRight">
                                            </asp:CalendarExtender>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="TIME-SLOT" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtreptime" MaxLength="20" runat="server" Text='<%# Eval("reptime") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Visible="False" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right; margin-top: 10px">
                    <asp:Button ID="BtnradioCancel" runat="server" Text="Cancel" OnClick="BtnradioCancel_Click" />
                </div>
            </div>
            <div style="text-align: right; margin-top: 10px" id="divdata" runat="server" visible="false">
                <asp:Button ID="Btnupdate" runat="server" Text="Update" OnClick="Btnupdate_Click"
                    OnClientClick="return Validate('Are you sure you want to change the Theory / Practical Exam Admit Card Details of the Candidate!');" />
                <asp:Button ID="Bcancel" runat="server" Text="Cancel" OnClick="Bcancel_Click" />
            </div>
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
                <asp:LinkButton ID="hlAppDetail" runat="server">Application Detail</asp:LinkButton>
            </td>
        </tr>
        <tr id="trtheory" runat="server" visible="false">
            <td>
                <asp:LinkButton ID="hldownloadadmitcard" runat="server">Download Admit Card</asp:LinkButton>
            </td>
        </tr>
        <tr id="trprac4" runat="server" visible="false">
            <td>
                <asp:LinkButton ID="hldownloadpracticaladmitcard" runat="server">Download Practical Admit Card</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
