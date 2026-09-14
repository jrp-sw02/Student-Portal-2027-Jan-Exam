<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="ConsolidatedPaymentApplicationFilter.aspx.cs" Inherits="HO_ConsolidatedPaymentApplicationFilter" Debug="false"%>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Consolidated Payment Application Report
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
            var CourseCatId;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            else
                CourseCatId = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;
            //Application Type
            var TypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            else
                TypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;
            //Exam Name which name as May, 2020
            var examName;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            else
                var ddlReport = document.getElementById("<%=ddlExamName.ClientID%>");
            var examName = ddlReport.options[ddlReport.selectedIndex].text;
                //examName = document.getElementById('<%=ddlExamName.ClientID %>').selectedIndex.text;
            //Regional Centre
            var RcentreID = 0;
            RcentreID = document.getElementById('<%=ddlRegionalCentre.ClientID %>').value;

            //Category
            var Category = 0;
            Category = document.getElementById('<%=ddlCastCategory.ClientID %>').value;

            //Occupation
            var Occupation = 0;
            Occupation = document.getElementById('<%=ddlOccupation.ClientID %>').value;
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
            //window.open("../HO/Rpt/ConsolidatedPaymentApplicationReport.aspx?applicantTypeID=" + applicantTypeID + "&InstituteID=" + instituteid +  "&Category=" + Category + "&Occupation=" + Occupation +  "&RegID=" + RcentreID + "&paymentStatusId=0");
            window.open("../HO/Rpt/ConsolidatedPaymentApplicationReport.aspx?applicantTypeID=" + applicantTypeID + "&TypeId=" + TypeId + "&ExamName=" + examName + "&CourseCatId=" + CourseCatId + "&InstituteID=" + instituteid + "&Category=" + Category + "&Occupation=" + Occupation + "&RegID=" + RcentreID + "&paymentStatusId=0");

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
                 <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>             
                <%-- <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
      <%--          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
               <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                       <%-- <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>              
                <%--<asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>--%>
                 <asp:Label ID="Label19" runat="server" SkinID="CaptionLabel" Text="Regional Centre&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>               
                 <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Cast Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>               
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Occupation&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>               
                 <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
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
        
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
