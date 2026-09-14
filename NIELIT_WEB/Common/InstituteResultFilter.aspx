<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InstituteResultFilter.aspx.cs"
    Inherits="Common_InstituteResultFilter" MasterPageFile="~/MasterPages/MyInfo.master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Institute Result Report
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <style type="text/css">  
        .PromptCSS  
        {  
            color:Blue;  
            font-size:medium;  
            font-style:italic;  
            font-weight:bold;  
            font-family:Courier New;  
            border:solid 1px Pink;  
            height:20px;  
            }  
    </style>
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
            window.open("../Common/InstituteResultReport.aspx?ExamId=" + ExamId + "&CourseId=" + CourseId + "&TypeId=" + TypeId, 'Institute Result Report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
            return false;
        }
    </script>
    <asp:label id="lblerror" runat="server"></asp:label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:label id="Label5" runat="server" skinid="CaptionLabel" text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label6" runat="server" skinid="CaptionLabel" text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label4" runat="server" skinid="CaptionLabel" text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:updatepanel id="UpdatePanel6" runat="server">
                    <contenttemplate>
                <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                    OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                    </contenttemplate>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel3" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:label id="Label8" runat="server" skinid="CaptionLabel" text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label9" runat="server" skinid="CaptionLabel" text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>
                <asp:updatepanel id="UpdatePanel5" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel4" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:button id="btnView" runat="server" text="View" onclientclick="return OpenWindow();" />
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />
    </div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>