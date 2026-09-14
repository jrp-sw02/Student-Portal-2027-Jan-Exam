<%@ Page Title="DLC Bulk Institute Performance Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="instBulkPerfoReportDLC.aspx.cs" Inherits="instBulkPerfoReportDLC" Debug="false" %>

<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
        function validateform() {
            if (!isBlankDate("<%=txtValidUptoStart.ClientID %>", "Valid upto Start Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtValidUptoStart.ClientID %>", "Invalid  Valid upto Start Date", "dd-MMM-yyyy"))
                return false;

            if (!isBlankDate("<%=txtValidUptoEnd.ClientID %>", "Valid upto End Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtValidUptoEnd.ClientID %>", "Invalid  Valid upto End Date", "dd-MMM-yyyy"))
                return false;

            if (!isBlank("txtYears", "Years required"))
                return false;
            if (!isNumber("txtYears"))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    DLC Bulk Institute Performance Report 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to excel file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server"
        OnClick="imgPDF_Click" Visible="True" Height="30%" Width="15%" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblValidUptoStart" runat="server" Text="<b>Institutes Valid Upto start date :</b>"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                            <asp:ScriptManager ID="ScriptManager2" runat="server" />


                        <asp:TextBox ID="txtValidUptoStart" runat="server"></asp:TextBox>


                        <asp:CalendarExtender ID="txtExpirydate_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txtValidUptoStart">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>

            <td>
                <asp:Label ID="lblValidUptoEnd" runat="server" Text="<b>Institutes Valid Upto end date :</b>"></asp:Label>

                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                                <asp:ScriptManager ID="ScriptManager3" runat="server" />

                            <asp:TextBox ID="txtValidUptoEnd" runat="server"></asp:TextBox>


                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                PopupButtonID="img1" TargetControlID="txtValidUptoEnd">
                            </asp:CalendarExtender>
                            <img id="img1" alt="Calender" src="../../images/calendaricon.jpg" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
        </tr>
        <tr>

            <td>
                <asp:Label ID="lblYears" runat="server" Text="<b>No. of years to check for :</b>"></asp:Label>

                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>

                            <asp:TextBox ID="txtYears" runat="server" onkeypress="checkNumber(this,3,0,event);" MaxLength="2"></asp:TextBox>

                        </ContentTemplate>

                    </asp:UpdatePanel>
                </td>

                <td>
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return validateform();"
                        OnClick="btnShow_Click" Visible="True" />

                </td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div id="divReportData" runat="server" style="overflow: scroll; width: 1000px; height: 1000px;">
    </div>
</asp:Content>
