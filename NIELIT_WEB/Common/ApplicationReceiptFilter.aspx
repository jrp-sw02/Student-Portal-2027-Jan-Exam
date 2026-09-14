<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ApplicationReceiptFilter.aspx.cs" Inherits="HO_ApplicationReceiptFilter" Debug="false"%>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Application Receipt Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
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
            //Gender
            var Gender = 0;
            Gender = document.getElementById('<%=ddlGender.ClientID %>').value;

            //Category
            var Category = 0;
            Category = document.getElementById('<%=ddlCastCategory.ClientID %>').value;

            //Occupation
            var Occupation = 0;
            Occupation = document.getElementById('<%=ddlOccupation.ClientID %>').value;

            //Candidate State
            var CStateID = 0;
            CStateID = document.getElementById('<%=ddlCandidateState.ClientID %>').value;

            //Regional Centre
            var RcentreID = 0;
            RcentreID = document.getElementById('<%=ddlRegionalCentre.ClientID %>').value;

            //Exam Centre 1
            var Examchoice1 = 0;
            if (document.getElementById("<%=ddlExamCentre1.ClientID %>").disabled == false) {
                Examchoice1 = document.getElementById('<%=ddlExamCentre1.ClientID %>').value;
            }

            //Ex State 1
            var ExState1 = 0;
            if (document.getElementById("<%=ddlExamState1.ClientID %>").disabled == false) {
                ExState1 = document.getElementById('<%=ddlExamState1.ClientID %>').value;
            }

            //Exam Centre 2
            var Examchoice2 = 0;
            if (document.getElementById("<%=ddlExamCentre2.ClientID %>").disabled == false) {
                Examchoice2 = document.getElementById('<%=ddlExamCentre2.ClientID %>').value;
            }

            //Ex State 2
            var ExState2 = 0;
            if (document.getElementById("<%=ddlExamState2.ClientID %>").disabled == false) {
                ExState2 = document.getElementById('<%=ddlExamState2.ClientID %>').value;
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

            //Application Status
            var StatusId;
            if (document.getElementById('<%=ddlStatus.ClientID %>').value == "0") {
                StatusId = 0;
            }
            else {
                StatusId = document.getElementById('<%=ddlStatus.ClientID %>').value;
            }
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
            //View report
            window.open("../HO/Rpt/ApplicationReceiptReport.aspx?StatusId=" + StatusId + "&ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId + "&applicantTypeID=" + applicantTypeID + "&InstituteID=" + instituteid + "&Gender=" + Gender + "&Category=" + Category + "&Occupation=" + Occupation + "&CStateID=" + CStateID + "&ExamCentre1=" + Examchoice1 + "&Exam1StateID=" + ExState1 + "&ExamCentre2=" + Examchoice2 + "&Exam2StateID=" + ExState2 + "&RegID=" + RcentreID + "&paymentStatusId=0");
            return false;
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
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Gender &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Applicant Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institute Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
                <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Occupation&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Candidate State&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
            <td>
                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlOccupation" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
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
        </tr>
        <tr id="Tr5" runat="server">
            <td>
                <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Exam Centre Choice 1&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Exam Centre State 1&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                &nbsp;
                <asp:Label ID="Label19" runat="server" SkinID="CaptionLabel" Text="Regional Centre&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr id="Tr6" runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCentre1" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlExamCentre1_SelectedIndexChanged">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamState1" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCentre1" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlRegionalCentre" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="Tr7" runat="server">
            <td>
                <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Exam Centre Choice 2&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text="Exam Centre State 2&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr id="Tr8" runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCentre2" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlExamCentre2_SelectedIndexChanged">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamState2" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCentre2" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                &nbsp;
            </td>
        </tr>
        <tr class="even">
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server" Height="22px" Width="760px">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
