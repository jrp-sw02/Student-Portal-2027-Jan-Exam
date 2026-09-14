<%@ Page Title="Statewise NSQF TPs  as on date" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="StateWiseNSQFTPReportFilter.aspx.cs" Inherits="StateWiseNSQFTPReportFilter" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    State Wise NSQF Short Term Courses TP Statistics As on Date
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            var DateFrom = 0;
            var DateTo = 0;
            var CourseId;

            if (!isBlankDate("<%=txtAsOnDate.ClientID %>", "As On Date", "dd-MMM-yyyy"))
                return false;
             DateFrom = document.getElementById('<%=txtAsOnDate.ClientID %>').value;
            //View report 
            var url = "../HO/Rpt/StateWiseNSQFTP.aspx?asOnDate=" + DateFrom ;
            window.open(url);
            return false;
        }
    </script>

    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="As On Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>            
       
            <td>
                <asp:TextBox ID="txtAsOnDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" 
                     ReadOnly="false" ToolTip="Date From" ></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="imgdate" TargetControlID="txtAsOnDate">
                </asp:CalendarExtender>
                <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
            </td>

        </tr>
    </table>
  
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
