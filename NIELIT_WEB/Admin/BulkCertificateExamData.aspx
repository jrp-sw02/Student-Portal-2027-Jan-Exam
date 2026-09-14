<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BulkCertificateExamData.aspx.cs"
    Inherits="BulkCertificateExamData" MasterPageFile="~/MasterPages/main.master"
    Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download Exam Data"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function validatefilter() {
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlRC.ClientID %>", "Regional Center"))
                return false;
            if (!isSelected("<%=ddlExamDate.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlExamBatch.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlExamCenter.ClientID %>", "Exam Name"))
                return false;

            var CourseCatId = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;
            var CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;
            var ExamName = document.getElementById('<%=ddlExamName.ClientID %>').value;
            var RcName = document.getElementById('<%=ddlRC.ClientID %>').value;
            var ExamDate = document.getElementById('<%=ddlExamDate.ClientID %>').value;
            var ExamBatch = document.getElementById('<%=ddlExamBatch.ClientID %>').value;
            var ExamCenter = document.getElementById('<%=ddlExamCenter.ClientID %>').value;

            var url = "../HO/Rpt/StudentExamData.aspx?CourseCatId=" + CourseCatId + "&CourseId=" + CourseId + "&ExamId=" + ExamName + "&RcId=" + RcName + "&ExamDate=" + ExamDate + "&ExamBatch=" + ExamBatch + "&ExamCenter=" + ExamCenter;
            window.open(url);
            return false;
        }
    </script>
    <style type="text/css">
        .trAdmitCard
        {
            text-align: left;
            padding-left: 5px;
            border: 1 solid #000000;
        }
    </style>
    <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="LblcourseCategory" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Lblcourse" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:DropDownList ID="ddlCourseCategry" runat="server" AutoPostBack="True" Height="22px"
                    OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" Height="22px"
                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True" Height="22px"
                            OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lblexamcycle" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Lblexamyear" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="True" Height="22px"
                            OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Regional Center &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Exam Date &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Exam Batch &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlRC" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlRC_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRC" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamDate" runat="server" SkinID="ddl250" AutoPostBack="true"
                            DataTextFormatString="{0:dd-MM-yyyy}" OnSelectedIndexChanged="ddlExamDate_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamDate" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamBatch" runat="server" SkinID="ddl250" 
                            AutoPostBack="true" onselectedindexchanged="ddlExamBatch_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlexambatch" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Exam Center &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCenter" runat="server" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRC" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
               
            </td>
            <td>
               
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
        <ContentTemplate>
            <asp:Label runat="server" Text="" ID="lblcount" CssClass="error" EnableTheming="False"
                Width="99%" Visible="false"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button runat="server" Text="View" ID="btnView" OnClientClick="return validatefilter();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
    </div>
    <asp:Panel runat="server" Style="display: none;" ID="pnladmitcard">
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
