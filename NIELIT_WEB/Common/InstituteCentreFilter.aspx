<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InstituteCentreFilter.aspx.cs"
    Inherits="Common_InstituteCentreFilter" MasterPageFile="~/MasterPages/MyInfo.master"
     Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Institute Performance Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <style type="text/css">
        .PromptCSS
        {
            color: Blue;
            font-size: medium;
            font-style: italic;
            font-weight: bold;
            font-family: Courier New;
            border: solid 1px Pink;
            height: 20px;
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

            //Application Type
            var TypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            
            if (document.getElementById('<%= DateWiseTable.ClientID %>').value != "") {
                if (!isBlank('<%= txtDateFrom.ClientID %>', "From Date"))
                    return false;
                if (!isBlank('<%= txtDateto.ClientID %>', "To Date"))
                   return false;
                if (!isSelected("<%=ddlinstitutename.ClientID %>", "Institute Name"))
                    return false;
                document.forms[0].target = "_blank";                  
            }                 
            if (document.getElementById('<%= ExamWiseTable.ClientID %>').value != "") {
                if (!isSelected('<%= ddlExamCycle.ClientID %>', "Exam Cycle"))
                    return false;
                if (!isSelected('<%= ddlExamName.ClientID %>', "Exam Name"))
                    return false;
                if (!isSelected("<%=ddlinstitutename.ClientID %>", "Institute Name"))
                    return false;
                document.forms[0].target = "_blank";                 
            }      
        }
    </script>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td width="33%" colspan="3">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Search By:-"></asp:Label>
                        <asp:RadioButtonList ID="Rdsearchby" runat="server" RepeatDirection="Horizontal"
                            Style="vertical-align: top; border: 0" BorderStyle="None" RepeatLayout="Flow"
                            AutoPostBack="true" OnSelectedIndexChanged="Rdsearchby_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">Exam-Wise</asp:ListItem>
                            <asp:ListItem Value="1">Date-Wise</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
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
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
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
    </table>
    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
        <ContentTemplate>
            <table id="ExamWiseTable" runat="server" visible="true" class="sample2" width="100%"
                border="0" cellpadding="2" cellspacing="0">
                <tr id="TrExamHead" runat="server">
                    <td>
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td width="33%">
                        &nbsp;
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
                    <td width="33%">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
        <ContentTemplate>
            <table id="DateWiseTable" runat="server" class="sample2" visible="false" width="100%"
                border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td width="33%">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td width="33%">
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table1" class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr id="Tr1" runat="server">
            <td colspan="3">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institute Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlinstitutename" runat="server" Height="22px" Style="width: 760px;">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlinstitutename"
                            PromptText="At first click on Institute name dropdown and type Institute Name"
                            PromptPosition="Bottom" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                        </asp:ListSearchExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="Summary Report" OnClientClick="return OpenWindow();"
            OnClick="btnView_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
