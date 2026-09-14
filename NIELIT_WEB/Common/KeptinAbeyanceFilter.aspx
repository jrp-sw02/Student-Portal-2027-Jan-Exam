<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KeptinAbeyanceFilter.aspx.cs"
    Inherits="Common_KeptinAbeyanceFilter" MasterPageFile="~/MasterPages/MyInfo.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Kept in Abeyance Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function OpenWindow() 
        {
            var DateFrom = 0;
            var DateTo = 0;
            var CourseId;

            // Registration Date

            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "Registration From Date", "dd-MMM-yyyy"))
                return false;
            else 
            {
                if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid Registration From Date", "dd-MMM-yyyy"))
                    return false;
                else
                {
                    DateFrom = document.getElementById("<%=txtDateFrom.ClientID %>").value;
                }
            }
            if (!isBlankDate("<%= txtToDate.ClientID %>", "Registration To Date", "dd-MMM-yyyy"))
                return false;
            else 
            {
                if (!isDate("<%=txtToDate.ClientID %>", "Invalid Registration To Date", "dd-MMM-yyyy"))
                    return false;
                else
                {
                    DateTo = document.getElementById('<%= txtToDate.ClientID %>').value;
                }
            }

            if (!CompareDates(DateFrom, DateTo, " Application From date should be less than Application To Date", true))
                return false;
            
            if (document.getElementById('<%=ddlCourseName.ClientID %>') && document.getElementById('<%=ddlCourseName.ClientID %>').value == "0")
                CourseId = 0;
            else
                CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;
           
            //View report
            window.open("../HO/Rpt/KeptinAbeyanceReport.aspx?CourseId=" + CourseId + "&DateFrom=" + DateFrom + "&DateTo=" + DateTo, 'report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
            return false;

        }
    </script>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;&lt;/b&gt;"
                    Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Application Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 33%;" valign="top">
                <asp:DropDownList ID="ddlCourseName" runat="server" SkinID="ddl250">
                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtDateFrom" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                <asp:CalendarExtender ID="calendar1" TargetControlID="txtDateFrom" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom" runat="server">
                </asp:CalendarExtender>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtToDate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img id="imgTo" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgTo" runat="server">
                </asp:CalendarExtender>
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
