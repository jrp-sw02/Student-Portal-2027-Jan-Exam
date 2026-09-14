<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="Studentstatistics.aspx.cs" Inherits="Admin_Studentstatistics" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NIELIT Student Statistics"></asp:Label>
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
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=DDlApplicationStatus.ClientID %>", "Status"))
                return false;
            if (!isSelected("<%=DdlReportType.ClientID %>", "Report Type"))
                return false;
            if (!isBlankDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
           
            return true;
        }
    </script>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td width="33%">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                 <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlCourseName_SelectedIndexChanged">
                            <asp:ListItem Value="00">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td width="33%">
              <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DDlApplicationStatus" runat="server" Height="22px" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Applied</asp:ListItem>
                            <asp:ListItem Value="2">Appeared</asp:ListItem>
                            <asp:ListItem Value="3">Passed</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                      <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td width="33%">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Report Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
            </td>
            <td width="33%">
                <asp:DropDownList ID="DdlReportType" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    <asp:ListItem Value="1">State Wise</asp:ListItem>
                    <asp:ListItem Value="2">Month Wise</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="Download Data" OnClientClick="return OpenWindow();"
            OnClick="btnView_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
