<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VirtualAcademyPaymentStatusReportBatchWise.aspx.cs"
    Inherits="Common_VirtualAcademyPaymentStatusReportBatchWise" MasterPageFile="~/MasterPages/main.master" Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function validateFormFields() {

            if (!isSelected("<%=ddlNielitCente.ClientID %>", "Centre Name"))
                return false;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlBatchName.ClientID %>", "Batch Name"))
                return false;

            if (!isSelected("<%=ddlNielitCente.ClientID %>", "Centre Name"))
                var CentreID = 0;
            else
                var CentreID = document.getElementById('<%=ddlNielitCente.ClientID %>').value;

            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                var CourseCategory = 0;
            else
                var CourseCategory = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;


            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                var CourseId = 0;
            else
                var CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            if (!isSelected("<%=ddlBatchName.ClientID %>", "Batch Name"))
                var BatchId = 0;
            else
                var BatchId = document.getElementById('<%=ddlBatchName.ClientID %>').value;


            <%--
            if (document.getElementById('<%=ddlCourseName.ClientID %>').value == "0") {
                var CourseId = 0;
            }
            else {
                var CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;
            }

            var BatchId = 0;
            if (document.getElementById('<%=ddlBatchName.ClientID %>').value == "0") {
                var BatchId = 0;
            }
            else {
                var BatchId = document.getElementById('<%=ddlBatchName.ClientID %>').value;
            }
        --%>


            var url = "../HO/Rpt/VirtualAcademyPaymentStatusReportBatchWiseRpt.aspx?CourseCategory=" + CourseCategory + "&CentreID=" + CentreID + "&CourseId=" + CourseId + "&BatchId=" + BatchId;
            window.open(url);
            return false;
        }
    </script>
    <style type="text/css">
        .style1 {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text=" Virtual Academy Batch Wise Payment Status Report" runat="server"></asp:Label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="LabelCentre" runat="server" SkinID="CaptionLabel" Text="NIELIT Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LabelCourseCat" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
              <td>
                <asp:Label ID="LabelCourse" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
                        <td class="style1">
                            <asp:DropDownList ID="ddlNielitCente" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="NielitCente_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
            </td>
            <td class="style1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry"/>
                    </Triggers>
                </asp:UpdatePanel>
            </td>

        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelBatch" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlBatchName" runat="server" Height="22px" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName"/>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return validateFormFields()"/>
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click1" /></div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
