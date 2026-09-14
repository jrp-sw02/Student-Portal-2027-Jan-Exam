<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="rptStateSkillDevStat.aspx.cs" Inherits="rptStateSkillDevStat" Debug="false" %>

<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="State Wise Skill Development Report" Font-Bold="True"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            
            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
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
       
        <tr id="TrExamHead" runat="server">
            <td width="33%">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
           
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txtDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
                    </ContentTemplate>
                    
                </asp:UpdatePanel>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../../images/calendaricon.jpg" />
            </td>
            
        </tr>
    </table>
    <div style="text-align: center; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="Download Data" OnClientClick="return OpenWindow();"
            OnClick="btnView_Click" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
