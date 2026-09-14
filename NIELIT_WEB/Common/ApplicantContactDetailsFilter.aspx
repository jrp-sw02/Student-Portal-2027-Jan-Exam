<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplicantContactDetailsFilter.aspx.cs"
    Inherits="Common_ApplicantContactDetailsFilter" MasterPageFile="~/MasterPages/MyInfo.master"  Debug="false"%>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Applicant Contact Details Report
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
            //moduletype
            var module = 0;
            module = document.getElementById('<%=ddlmoduletype.ClientID %>').value;


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

            //View report
            window.open("../HO/Rpt/ApplicantContactDetailReport.aspx?ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId + "&moduletypeID=" + module);
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
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Module Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
                        <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250">
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
                <asp:DropDownList ID="ddlmoduletype" runat="server" Height="22px" SkinID="ddl250" AutoPostBack="true">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Theory</asp:ListItem>
                    <asp:ListItem Value="2">Practical</asp:ListItem>
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
