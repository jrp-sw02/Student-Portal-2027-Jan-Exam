<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExamData.aspx.cs" Inherits="Common_ExamData"
    MasterPageFile="~/MasterPages/MyInfo.master" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Exam Data Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            //Table name
            if (!isSelected("<%=ddltablename.ClientID %>", "Table Name"))
                return false;
            //Course Name
            var CourseId;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isBlankDate("<%=txtexamdate.ClientID %>", "Exam Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtexamdate.ClientID %>", "Invalid Exam Date", "dd-MMM-yyyy"))
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
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text=" Table Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text=" Exam Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:DropDownList ID="ddltablename" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    <asp:ListItem Value="1">E_Fm</asp:ListItem>
                    <asp:ListItem Value="2">E_Fm_Sub</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtexamdate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                <asp:CalendarExtender ID="calendar1" TargetControlID="txtexamdate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom" runat="server">
                </asp:CalendarExtender>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="Generate Report" 
            OnClientClick="return OpenWindow();" onclick="btnView_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
