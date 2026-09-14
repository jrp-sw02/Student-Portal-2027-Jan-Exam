<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ExamCenterFilter.aspx.cs" Inherits="ExamCenterFilter" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Exam Centre Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
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
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;

            //Exam Year
            var ExamYear;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            else
                ExamYear = document.getElementById('<%=ddlExamYear.ClientID %>').value;

            //Exam Name
            var ExamId;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            else
                ExamId = document.getElementById('<%=ddlExamName.ClientID %>').value;

            //State Name
            var stateID;
            if (!isSelected("<%=ddlState.ClientID %>", "State Name"))
                return false;
            else
                stateID = document.getElementById('<%=ddlState.ClientID %>').value;

            var examCenter;
            if (!isSelected("<%=ddlExamCenter.ClientID %>", "Exam Center"))
                return false;
            else
                examCenter = document.getElementById('<%=ddlExamCenter.ClientID %>').value;
            var examVenue=0;
            var hfCourseExamApp = document.getElementById('<%=hf1.ClientID %>').value;
//            if (TypeId == hfCourseExamApp)
//            {
//               if (!isSelected("<%=ddlExamVenue.ClientID %>", "Exam Venue"))
//                    return false;
//               else
//                   examVenue = document.getElementById('<%=ddlExamVenue.ClientID %>').value;
//            }
            //View report
            window.open("../HO/Rpt/ExamCenterReport.aspx?ExamYear=" + ExamYear + "&ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId + "&stateID=" + stateID + "&ExamCenter=" + examCenter);
            return false;
        }
    </script>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
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
                        <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" 
                            onselectedindexchanged="ddlExamName_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="State&lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <%--<asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="District&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Centre&lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblExamVenue" runat="server" SkinID="CaptionLabel" Text="Exam Venue&lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Visible="False"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="even">
            <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlState" runat="server" SkinID="ddl250" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                <%--<asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlDistrict" runat="server" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCenter" runat="server" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlExamCenter_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamVenue" runat="server" SkinID="ddl250" Visible="false">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCenter" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hf1" runat="server" />
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
