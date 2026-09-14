<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchBCCCCCandidate.aspx.cs"
    Inherits="Common_SearchBCCCCCandidate" MasterPageFile="~/MasterPages/main.master"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Search DLC/IRDA-Exam Candidate"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var globalvar = "0";
        function ValidateFormFields() {
            var rblitems = document.getElementById("<%=Rdsearchby.ClientID%>");
            if (rblitems.getElementsByTagName("input").item != "Appl.No.") {
                if (!isBlank("<%=Txtfilter.ClientID %>", "This Field"))
                    return false;
                if (!isSelected("<%=ddlcname.ClientID %>", "Course"))
                    return false;
                if (!isSelected("<%=ddlapptype.ClientID %>", "Applicant Type"))
                    return false;
                if (!isSelected("<%=ddlGender.ClientID %>", "Gender"))
                    return false;

                if (document.getElementById("<%=ddlapptype.ClientID %>").value == 2) {
                    if (document.getElementById("<%=TrIns1.ClientID %>").style.display != 'none' && document.getElementById("<%=TrIns2.ClientID %>").style.display != 'none') {
                        if (document.getElementById("<%=ChkAllInstitute.ClientID %>").checked == false) {
                            var TxtInstituteName = document.getElementById('<%=TxtInstituteName.ClientID %>').value
                            var hfInstituteName = document.getElementById("<%=hfInstituteName.ClientID %>").value
                            if (document.getElementById('<%=HfInstitute.ClientID %>').value == "0") {
                                callErrorMsg('<%=TxtInstituteName.ClientID %>', "Please enter Institute Name");
                                return false;
                            }
                            else {
                                if (document.getElementById('<%=TxtInstituteName.ClientID %>').value != "") {
                                    if (hfInstituteName != TxtInstituteName) {
                                        callErrorMsg('<%=TxtInstituteName.ClientID %>', "Please enter Institute Name again");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                var DateFrom = document.getElementById("<%=txtDateFrom.ClientID %>").value;
                var DateTo = document.getElementById('<%= txtDateto.ClientID %>').value;
                var DOB = document.getElementById('<%= txtdate.ClientID %>').value;
                var filter = document.getElementById('<%=Txtfilter.ClientID %>').value;
                if (trim(DateFrom, "") == "" && trim(DateTo, "") == "" && trim(DOB, "") == "" && trim(filter, "") == "") {
                    alert("Please select atleast one filter criteria to search the candidate ie. select either application date or date of birth or enter any keyword!");
                    return false;
                }
                else {
                    if (trim(DOB, "") != "") {
                        if (!isDate("<%= txtdate.ClientID %>", "Invalid Date of Birth"))
                            return false;
                    }
                    if (trim(DateFrom, "") != "" && trim(DateTo, "") != "") {
                        if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid Application From Date", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtDateto.ClientID %>", "Invalid Application To Date", "dd-MMM-yyyy"))
                            return false;
                        if (!CompareDates(DateFrom, DateTo, " Application From date should be less than Application To Date", true))
                            return false;
                    }
                    if (trim(DateFrom, "") != "") {
                        if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid Application From Date", "dd-MMM-yyyy"))
                            return false;
                    }
                    if (trim(DateFrom, "") == "" && trim(DateTo, "") != "") {
                        alert("Please select first Application From Date)");
                        document.getElementById("<%=txtDateFrom.ClientID %>").focus();
                        return false;
                    }
                }
            }

            else {
                if (!isBlank("<%=Txtfilter.ClientID %>", "This Field"))
                    return false;

                return true;
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

        function OnInstituteSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            if (results.ID == "0")
                return false;
            document.getElementById("<%=HfInstitute.ClientID %>").value = results.ID;
            document.getElementById("<%=hfInstituteName.ClientID %>").value = results.Name;
            globalvar = results.Name;
        }
    </script>
    <div id="divfilter" runat="server">
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td width="33%" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Search By:-"></asp:Label>
                            <asp:RadioButtonList ID="Rdsearchby" runat="server" RepeatDirection="Horizontal"
                                Style="vertical-align: top; border: 0" BorderStyle="None" RepeatLayout="Flow"
                                AutoPostBack="true" OnSelectedIndexChanged="Rdsearchby_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Appl.No.</asp:ListItem>
                                <asp:ListItem Value="1">Name.</asp:ListItem>
                                <asp:ListItem Value="2">Father Name.</asp:ListItem>
                                <asp:ListItem Value="3">Mother Name.</asp:ListItem>
                                <asp:ListItem Value="4">Guardian Name.</asp:ListItem>
                                <asp:ListItem Value="5">Roll Number</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td width="33%" colspan="3">
                    <asp:TextBox ID="Txtfilter" runat="server" MaxLength="50" SkinID="txt756" onkeypress="return isNumberKey(event);"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table id="TblOptions" runat="server" visible="false" class="sample2" width="100%"
                    border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td width="33%">
                            <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Name"></asp:Label>
                        </td>
                        <td width="33%">
                            <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Applicant Type"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Applicant Gender"></asp:Label>
                        </td>
                    </tr>
                    <tr class="even">
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlcname" runat="server" Height="22px" SkinID="ddl250" AutoPostBack="True"
                                        AppendDataBoundItems="false" OnSelectedIndexChanged="ddlcname_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlapptype" runat="server" Height="22px" SkinID="ddl250" AppendDataBoundItems="false"
                                OnSelectedIndexChanged="ddlapptype_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGender" runat="server" Height="22px" SkinID="ddl250" AppendDataBoundItems="false"
                                OnSelectedIndexChanged="ddlapptype_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                <asp:ListItem Value="A">-All-</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                                <asp:ListItem Value="F">Female</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TrIns1" runat="server" visible="false">
                        <td colspan="3">
                            Institute Name
                            <asp:CheckBox ID="ChkAllInstitute" runat="server" AutoPostBack="True" OnCheckedChanged="ChkAllInstitute_CheckedChanged"
                                Text="All Institutes" />
                        </td>
                    </tr>
                    <tr class="even" id="TrIns2" runat="server" visible="false">
                        <td colspan="3">
                            <asp:TextBox ID="TxtInstituteName" runat="server" SkinID="txt756" Width="700px"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="aceSearch" ServiceMethod="GetBCCCCCInstitutes" FirstRowSelected="true"
                                OnClientItemSelected="OnInstituteSelected" ServicePath="~/WS/Common.asmx" TargetControlID="TxtInstituteName"
                                UseContextKey="true" ContextKey="0" runat="server" MinimumPrefixLength="1" CompletionInterval="0"
                                EnableCaching="false" CompletionSetCount="10">
                            </asp:AutoCompleteExtender>
                            <asp:HiddenField ID="HfInstitute" runat="server" Value="0" />
                            <asp:HiddenField ID="hfInstituteName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="33%">
                            <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Application From Date"></asp:Label>
                        </td>
                        <td width="33%">
                            <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application To Date"></asp:Label>
                        </td>
                        <td width="33%">
                            <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Date of Birth(dd-Mon-yyyy)"></asp:Label>
                        </td>
                    </tr>
                    <tr class="even">
                        <td width="33%">
                            <asp:TextBox ID="txtDateFrom" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                            <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                vertical-align: top;" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                                Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                            </asp:CalendarExtender>
                        </td>
                        <td width="33%">
                            <asp:TextBox ID="txtDateto" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                            <img id="imgto" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                vertical-align: top;" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateto"
                                Format="dd-MMM-yyyy" PopupButtonID="imgto">
                            </asp:CalendarExtender>
                        </td>
                        <td width="33%">
                            <asp:TextBox ID="txtdate" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                            <img id="imgdate" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                vertical-align: top;" />
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtdate"
                                Format="dd-MMM-yyyy" PopupButtonID="imgdate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="BtnView" runat="server" Text="View" OnClick="BtnView_Click" OnClientClick="return ValidateFormFields();" />
            <asp:Button ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" /></div>
    </div>
    <asp:Label Width="99%" Style="background-color: #EACFCE; padding-top: 4px; padding-bottom: 4px;
        padding-left: 4px; color: Red; border: 1px solid maroon; font-size: 11pt; font-variant: normal;"
        ID="Lblerror" Visible="false" runat="server" Text=""></asp:Label>
    <div id="divGrid" runat="server" visible="false">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="1%" HeaderText="#">
                            <HeaderStyle Width="1%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:HyperLinkField HeaderText="Course" DataTextField="CourseName" SortExpression="CourseName"
                            DataNavigateUrlFields="ApplID,CourseID,Applno,regdate,regtodate,filcriteria,AptpID,couID,dob,index"
                            DataNavigateUrlFormatString="../Admin/AdminRegstudbcc.aspx?ApplID={0}&CourseID={1}&Applno={2}&Src=search&regdate={3:dd-MMM-yyyy}&regtodate={4:dd-MMM-yyyy}&filcriteria={5}&AptpID={6}&couID={7}&dob={8:dd-MMM-yyyy}&index={9}">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Appl.No." DataNavigateUrlFields="ApplID,CourseID,Applno,regdate,regtodate,filcriteria,AptpID,couID,dob,index"
                            DataNavigateUrlFormatString="../Admin/AdminRegstudbcc.aspx?ApplID={0}&CourseID={1}&Applno={2}&Src=search&regdate={3:dd-MMM-yyyy}&regtodate={4:dd-MMM-yyyy}&filcriteria={5}&AptpID={6}&couID={7}&dob={8:dd-MMM-yyyy}&index={9}"
                            DataTextField="Applno" SortExpression="Applno" Target="_self">
                            <HeaderStyle Width="8%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Appl.Date" DataTextField="date" DataNavigateUrlFields="ApplID,CourseID,Applno,regdate,regtodate,filcriteria,AptpID,couID,dob,index"
                            DataNavigateUrlFormatString="../Admin/AdminRegstudbcc.aspx?ApplID={0}&CourseID={1}&Applno={2}&Src=search&regdate={3:dd-MMM-yyyy}&regtodate={4:dd-MMM-yyyy}&filcriteria={5}&AptpID={6}&couID={7}&dob={8:dd-MMM-yyyy}&index={9}"
                            SortExpression="date" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Student Name" DataTextField="StudentName" DataNavigateUrlFields="ApplID,CourseID,Applno,regdate,regtodate,filcriteria,AptpID,couID,dob,index"
                            DataNavigateUrlFormatString="../Admin/AdminRegstudbcc.aspx?ApplID={0}&CourseID={1}&Applno={2}&Src=search&regdate={3:dd-MMM-yyyy}&regtodate={4:dd-MMM-yyyy}&filcriteria={5}&AptpID={6}&couID={7}&dob={8:dd-MMM-yyyy}&index={9}"
                            SortExpression="StudentName">
                            <HeaderStyle Width="35%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Father Name" DataTextField="FatherName" DataNavigateUrlFields="ApplID,CourseID,Applno,regdate,regtodate,filcriteria,AptpID,couID,dob,index"
                            DataNavigateUrlFormatString="../Admin/AdminRegstudbcc.aspx?ApplID={0}&CourseID={1}&Applno={2}&Src=search&regdate={3:dd-MMM-yyyy}&regtodate={4:dd-MMM-yyyy}&filcriteria={5}&AptpID={6}&couID={7}&dob={8:dd-MMM-yyyy}&index={9}"
                            SortExpression="FatherName">
                            <HeaderStyle Width="33%" />
                        </asp:HyperLinkField>
                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>
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
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
