<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationBatchProcessingFilter.aspx.cs"
    Inherits="Common_RegistrationBatchProcessingFilter" MasterPageFile="~/MasterPages/MyInfo.master"
    Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Registration Processing Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            var curdate = new Date().format("dd-MMM-yyyy");
            //Course Category
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;

            //Course Name
            var CourseId;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            else
                CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            //Application Type
            var TypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            else
                TypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;

            //Exam cycle
            if (document.getElementById('<%=ddlExamCycle.ClientID %>')) {
                if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                    return false;

            }

            //Exam Name
            var ExamId;
            if (document.getElementById('<%=ddlExamName.ClientID %>')) {
                if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                    return false;
                else
                    ExamId = document.getElementById('<%=ddlExamName.ClientID %>').value;
            }
            else
                ExamId = "0";

            //Gender
            var Gender = 0;
            Gender = document.getElementById('<%=ddlGender.ClientID %>').value;

            // ApplicantType
            var applicantTypeID;
            var instituteid = 0;
            if (document.getElementById('<%=ddlapplicationtype.ClientID %>').value == "0") {
                applicantTypeID = 0;
            }
            else {
                if (document.getElementById("<%=ddlapplicationtype.ClientID %>").disabled == false) {
                    applicantTypeID = document.getElementById('<%=ddlapplicationtype.ClientID %>').value;
                    if (applicantTypeID == "2") {
                        if (document.getElementById('<%=Ddlinstitutes.ClientID %>').value == "0") {
                            instituteid = 0;
                        }
                        else {
                            instituteid = document.getElementById('<%=Ddlinstitutes.ClientID %>').value;
                        }
                    }
                }
                else {
                    applicantTypeID = document.getElementById('<%=ddlapplicationtype.ClientID %>').value;
                }
            }

            //Category
            var Category = 0;
            Category = document.getElementById('<%=ddlCastCategory.ClientID %>').value;

          
            //Candidate State
            var CStateID = 0;
            CStateID = document.getElementById('<%=ddlCandidateState.ClientID %>').value;

            // Registration Dates
            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "Registration/Re-Registration From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateFrom.ClientID %>", "Registration/Re-Registration From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtDateto.ClientID %>", "Registration/Re-Registration To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateto.ClientID %>", "Registration/Re-Registration To Date", "dd-MMM-yyyy"))
                return false;
            var regFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            var regToDate = document.getElementById('<%=txtDateto.ClientID %>').value;
            if (!CompareDates(regFromDate, regToDate, " Registration/Re-Registration From date should be less than Registration/Re-Registration To Date", true))
                return false;


            //Registration Type
            var RegtypeID = 0;
//            if (!isSelected("<%=ddlregtype.ClientID %>", "Registration Type"))
//                return false;
//            else
                RegtypeID = document.getElementById('<%=ddlregtype.ClientID %>').value;


            //Application Status
            var ApplicationStatusId = 0;
            if (document.getElementById('<%=ddlApplStatus.ClientID %>').value == "V") {
                ApplicationStatusId = 18; //RegistrationNumberAlloted
            }
            else if (document.getElementById('<%=ddlApplStatus.ClientID %>').value == "K") {
                ApplicationStatusId = 14; //KeptInAbeyance
            }
            else if (document.getElementById('<%=ddlApplStatus.ClientID %>').value == "R") {
                ApplicationStatusId = 15; //ApplicationRejectedWithReason
            }

            //ReportFormat
            var repformat = 0;
            if (!isSelected("<%=ddlreportformat.ClientID %>", "Report Format"))
                return false;
            else
                repformat = document.getElementById('<%=ddlreportformat.ClientID %>').value;


            //BatchNumber
            var batchid;
            batchid = document.getElementById('<%=ddlbatchnumber.ClientID %>').value;

            //View report
            window.open("../HO/Rpt/RegistrationProcessingReport.aspx?ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId + "&applicantTypeID=" + applicantTypeID + "&InstituteID=" + instituteid + "&Gender=" + Gender + "&Category=" + Category + "&CStateID=" + CStateID + "&RegTypeID=" + RegtypeID + "&regFromDate=" + regFromDate + "&regToDate=" + regToDate + "&ApplicationStatusId=" + ApplicationStatusId + "&ReptFormat=" + repformat + "&batchno=" + batchid);
            return false;
        }

        function OpenWindow1() {
            var curdate = new Date().format("dd-MMM-yyyy");
            //Course Category
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;

            //Course Name
            var CourseId;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            else
                CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            //Application Type
            var TypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            else
                TypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;

            //Exam cycle
            if (document.getElementById('<%=ddlExamCycle.ClientID %>')) {
                if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                    return false;

            }

            //Exam Name
            var ExamId;
            if (document.getElementById('<%=ddlExamName.ClientID %>')) {
                if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                    return false;
                else
                    ExamId = document.getElementById('<%=ddlExamName.ClientID %>').value;
            }
            else
                ExamId = "0";

            //Gender
            var Gender = 0;
            Gender = document.getElementById('<%=ddlGender.ClientID %>').value;

            // ApplicantType
            var applicantTypeID;
            var instituteid = 0;
            if (document.getElementById('<%=ddlapplicationtype.ClientID %>').value == "0") {
                applicantTypeID = 0;
            }
            else {
                if (document.getElementById("<%=ddlapplicationtype.ClientID %>").disabled == false) {
                    applicantTypeID = document.getElementById('<%=ddlapplicationtype.ClientID %>').value;
                    if (applicantTypeID == "2") {
                        if (document.getElementById('<%=Ddlinstitutes.ClientID %>').value == "0") {
                            instituteid = 0;
                        }
                        else {
                            instituteid = document.getElementById('<%=Ddlinstitutes.ClientID %>').value;
                        }
                    }
                }
                else {
                    applicantTypeID = document.getElementById('<%=ddlapplicationtype.ClientID %>').value;
                }
            }

            //Category
            var Category = 0;
            Category = document.getElementById('<%=ddlCastCategory.ClientID %>').value;

            

            //Candidate State
            var CStateID = 0;
            CStateID = document.getElementById('<%=ddlCandidateState.ClientID %>').value;

            // Registration Dates
            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "Registration/Re-Registration From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateFrom.ClientID %>", "Registration/Re-Registration From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtDateto.ClientID %>", "Registration/Re-Registration To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateto.ClientID %>", "Registration/Re-Registration To Date", "dd-MMM-yyyy"))
                return false;
            var regFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            var regToDate = document.getElementById('<%=txtDateto.ClientID %>').value;
            if (!CompareDates(regFromDate, regToDate, " Registration/Re-Registration From date should be less than Registration/Re-Registration To Date", true))
                return false;


            //Registration Type
            var RegtypeID = 0;
            if (!isSelected("<%=ddlregtype.ClientID %>", "Registration Type"))
                return false;
            else
                RegtypeID = document.getElementById('<%=ddlregtype.ClientID %>').value;

            //Application Status
            var ApplicationStatusId = 0;
            if (document.getElementById('<%=ddlApplStatus.ClientID %>').value == "V") {
                ApplicationStatusId = 18; //RegistrationNumberAlloted
            }
            else if (document.getElementById('<%=ddlApplStatus.ClientID %>').value == "K") {
                ApplicationStatusId = 14; //KeptInAbeyance
            }
            else if (document.getElementById('<%=ddlApplStatus.ClientID %>').value == "R") {
                ApplicationStatusId = 15; //ApplicationRejectedWithReason
            }

            //ReportFormat
            var repformat = 0;
            if (!isSelected("<%=ddlreportformat.ClientID %>", "Report Format"))
                return false;
            else
                repformat = document.getElementById('<%=ddlreportformat.ClientID %>').value;
        }

        function SetDropdownvalue() {
            document.getElementById('<%=ddlregtype.ClientID %>').value = "0"
        }
    </script>
    <style type="text/css">
        .PromptCSS
        {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: CourierNew;
            height: 20px;
            margin-left: 100px;
        }
    </style>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Gender"></asp:Label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlGender" runat="server" Height="22px" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Applicant Type"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institute Name"></asp:Label>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanelapp" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlapplicationtype" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlapplicationtype_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="Ddlinstitutes" runat="server" Height="22px" SkinID="ddl504">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="Ddlinstitutes"
                            PromptText="Type accredited institute name to search from the institute list"
                            PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                        </asp:ListSearchExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlapplicationtype" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="Tr3" runat="server">
            <td>
                <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Category"></asp:Label>
            </td>
            <td colspan="2">
                <asp:label id="Label19" runat="server" skinid="CaptionLabel" text="Registration Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
           
        </tr>
        <tr id="Tr4" runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCastCategory" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td colspan="2">
                <asp:updatepanel id="UpdatePanel10" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlregtype" runat="server" Height="22px" SkinID="ddl504" 
                            AutoPostBack="True" >
                        </asp:DropDownList>
                    </contenttemplate>
                </asp:updatepanel>
            </td>
        </tr>
        <tr id="Tr5" runat="server">
            <td>
                <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Candidate State"></asp:Label>
            </td>
            <td>
                <asp:label id="Label13" runat="server" skinid="CaptionLabel" text=" Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label14" runat="server" skinid="CaptionLabel" text=" Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCandidateState" runat="server" Height="22px" SkinID="ddl250">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="200px" MaxLength="11"  
                             AutoPostBack="True" 
                            ontextchanged="txtDateFrom_TextChanged"></asp:TextBox>
                        <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                            vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                            Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                        </asp:CalendarExtender>
                    </contenttemplate>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel9" runat="server">
                    <contenttemplate>
                        <asp:TextBox ID="txtDateto" runat="server" Width="200px" MaxLength="11" 
                             AutoPostBack="True" 
                            ontextchanged="txtDateto_TextChanged"></asp:TextBox>
                        <img id="imgto" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                            vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateto"
                            Format="dd-MMM-yyyy" PopupButtonID="imgto">
                        </asp:CalendarExtender>
                    </contenttemplate>
                </asp:updatepanel>
            </td>
        </tr>
        <tr id="Tr6" runat="server">
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Application Status"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Report Format"></asp:Label>
            </td>
            <td>
                <asp:label id="Label31" runat="server" skinid="CaptionLabel" text="Batch No">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:DropDownList ID="ddlApplStatus" runat="server" Height="22px" 
                    SkinID="ddl250" onselectedindexchanged="ddlApplStatus_SelectedIndexChanged1" AutoPostBack="true">
                    <asp:ListItem Value="V">Registration Number Allotted</asp:ListItem>
                    <asp:ListItem Value="K">Kept In Abeyance</asp:ListItem>
                    <asp:ListItem Value="R">Rejected</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlreportformat" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="2">Registration Card Data File Format</asp:ListItem>
                    <asp:ListItem Value="1">Registration Card Dispatch File Format</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel18" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlbatchnumber" runat="server" height="22px" skinid="ddl250">
                    <asp:listitem value="0">All</asp:listitem>
                </asp:dropdownlist>
                </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtDateFrom" EventName="TextChanged" />
                         <asp:AsyncPostBackTrigger ControlID="txtDateto" EventName="TextChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnphoto" runat="server" Text="Download Images" OnClientClick="return OpenWindow1();"
            OnClick="btnphoto_Click" Visible="true" />
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
