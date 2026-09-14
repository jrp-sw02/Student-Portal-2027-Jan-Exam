<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DuplicateRecordsFilter.aspx.cs"
    Inherits="Common_DuplicateRecordsFilter" MasterPageFile="~/MasterPages/main.master"  Debug="false"%>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function validateFormFields() {

            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcoursename.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            if (!isSelected("<%=ddlexamname.ClientID %>", "Exam Name"))
                return false;
            if (!isSelected("<%=ddlbatchnumber.ClientID %>", "Batch Number"))
                return false;

            var apptypeID;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                apptypeID = 0;
            else
                apptypeID = document.getElementById('<%=ddlAppType.ClientID %>').value;

            var CourseCatId;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                CourseCatId = 0;
            else
                CourseCatId = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;

            var batchId;
            if (!isSelected("<%=ddlbatchnumber.ClientID %>", "Batch Number"))
                batchId = 0;
            else
                batchId = document.getElementById('<%=ddlbatchnumber.ClientID %>').value;

            var examid;
            if (!isSelected("<%=ddlexamname.ClientID %>", "Exam Name"))
                examid = 0;
            else
                examid = document.getElementById('<%=ddlexamname.ClientID %>').value;

            var courseid;
            if (!isSelected("<%=ddlcoursename.ClientID %>", "Course Name"))
                courseid = 0;
            else
                courseid = document.getElementById('<%=ddlcoursename.ClientID %>').value;


            window.open("../HO/Rpt/DuplicateCandidateReport.aspx?CourseCatId=" + CourseCatId + "&AppTypeId=" + apptypeID + "&BatchId=" + batchId + "&Examid=" + examid + "&CourseID=" + courseid, 'report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Duplicate Candidate Details" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt; "></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlcoursename" runat="server" Height="22px" 
                            SkinID="ddl250" onselectedindexchanged="ddlcoursename_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" 
                            onselectedindexchanged="ddlAppType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlcoursename" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Batch Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlexamname" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlexamname_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlbatchnumber" runat="server" Height="22px" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlexamname" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px;">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return validateFormFields()" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click1" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
