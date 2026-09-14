<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="CertificateExamCandidateListReport.aspx.cs" Inherits="CAND_CertificateExamCandidateListReport"
    Debug="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 87px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="Server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="Server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
<div align="center">
        <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    </div><br />
    <br />
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <table>
        <tr>
            <td colspan="3">
                <strong><b><u>Declaration:</u></b></strong>
                <p>
                    I do hereby certify that the information furnished above is true and spelled correctly
                    to the best of my knowledge and belief and the same is entered in the database.<br />
                    I further abide that no alteratioin shall be sought from our end.</p>
            </td>
        </tr>
        <tr>
            <td width="20%">
                Sign of the forwarding authority
            </td>
            <td width="1%">
            :
            </td>
            <td width="79%">
                ____________________
            </td>
        </tr>
        <tr>
            <td width="20%">
                Name:
            </td>
             <td width="1%">
            :
            </td>
            <td width="79%">
            ____________________
            </td>
        </tr>
        <tr>
            <td width="20%">
                Designation:
            </td>
             <td width="1%">
            :
            </td>
            <td width="79%">
                ____________________
            </td>
        </tr>
        <tr>
            <td width="20%">
                <asp:Label ID="lblBccnoLable" runat="server" Text="Label"></asp:Label>:
            </td>
             <td width="1%">
            :
            </td>
            <td width="79%">
                <asp:Label ID="lblBccno" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="20%">
                Institute Name:
            </td>
             <td width="1%">
            :
            </td>
            <td width="79%">
            <asp:Label ID="lblInstituteName" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="20%">
                Seal of the Institute
            </td>
             <td width="1%">
            :
            </td>
            <td width="79%">
            </td>
        </tr>
    </table>
</asp:Content>
