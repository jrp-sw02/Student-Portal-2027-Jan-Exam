<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="DownloadConsolidatedMarksheet.aspx.cs" Inherits="ReportPgae"  Debug="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function printwindow() {
        window.print();
        return false;
    }
</script>
    <style type="text/css">
        .error {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    Consolidated Marksheet
</asp:Content>
<asp:Content ID="Content3"  ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" 
        ToolTip="Export to PDF file" ID="ibExport" ImageUrl="~/images/pdf.jpg" 
        runat="server" onclick="imgPDF_Click" Visible="True" height="30%" width="15%"/>
    <%--<asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="30%" width="15%" />--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
        <%--<asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>--%>
&nbsp;
</asp:Content>
<asp:Content ID="Content5"  ContentPlaceHolderID="cpReportDate" runat="server">
   Report Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
<div id="divReportData" runat="server" style="width:100%;">
 <%--<asp:gridview id ="grdv" runat="server"></asp:gridview>--%>

<%--<td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtAddress2" runat="server" MaxLength="20" SkinID="txt248"></asp:TextBox>
 </td>--%>

      <tr class="gdrow1">
                <td width="40%">
                    Registration No.  
                </td>
                <td>
                    <asp:TextBox ID="regisId" runat="server" Width="139px" MaxLength="30" ></asp:TextBox>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;"></span>
                    <asp:Button ID="Button1" runat="server" OnClick="chkBtn_Click" Text="Check" />
                    <td>
                        <asp:label id="Lblerror" runat="server" text align =" centre" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>

                    </td>
                    <%--<asp:label id="Lblerror" runat="server" text align =" centre" enabletheming="false" cssclass="error" width="100%"
        visible="false"></asp:label>--%>
                </td>
              <%--<td> 
              <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
              visible="false"></asp:label>
              </td>--%>
            </tr>

    <tr >
        <td text align ="centre">
             <%--<asp:label id="Lblerror" runat="server" text align =" centre" enabletheming="false" cssclass="error" width="100%"
        visible="false"></asp:label>--%>
            <%--<asp:label id="Lblerror" runat="server" align = "center" enabletheming="False" cssclass="error" width="200px" 
                style =" margin-left: 100px; margin-right : 100px; "
              visible="False"></asp:label>--%>
        </td>
    </tr>

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <%--<asp:Button ID="chkBtn" runat="server" OnClick="chkBtn_Click" OnClientClick=" return ValidateFormFields();" Text="Check" />--%>
    <%--<asp:Button ID="chkBtn" runat="server" Text="Check" />--%>

</div>
</asp:Content>


