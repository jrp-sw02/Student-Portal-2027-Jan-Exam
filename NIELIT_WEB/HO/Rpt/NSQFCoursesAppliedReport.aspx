<%@ Page Title="NSQF Courses Accr Applied Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="NSQFCoursesAppliedReport.aspx.cs" Inherits="NSQFCoursesAppliedReport" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
        function validateform() {
            
            if (!isBlank("txtFrom", "Date From"))
                return false;
            
            return true;
        }
    </script>
    <style type="text/css">
.custom {
	font-family: Courier;
	color: red;
	font-size:20px;
}
</style>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  visible="false"/>&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to excel file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" visible="false" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="30%" width="15%" />
  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
            
          <tr  >
            <td>
                <asp:Label ID="lblbatchfrom" runat="server" SkinID="CaptionLabel" Text="Date From"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblbatchto" runat="server" SkinID="CaptionLabel" Text=" Date To"></asp:Label>
            </td>
            
        </tr>

        <tr  >
            
            <td width="33%">
                <asp:TextBox ID="txtFrom" runat="server" Width="200px" MaxLength="11" AutoPostBack="True" ></asp:TextBox>
                <img id="imgFrom" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFrom"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtto" runat="server" Width="200px" MaxLength="11" ></asp:TextBox>
                <img id="imgto1" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtto"
                    Format="dd-MMM-yyyy" PopupButtonID="imgto">
                </asp:CalendarExtender>
            </td>
        </tr>
                    </table>
     <div style="text-align: right; margin-top: 10px">
          
                <asp:Button ID="btnGenerate" runat="server" Text="Show Report" 
                    OnClick="btnGenerate_Click" />
        
        
            </div>        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    NSQF Courses Accr Applied Report</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
                                 </ContentTemplate></asp:UpdatePanel>
                           <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblheading" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="NSQF Courses Accr Applied Report" Visible="false"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label5" Visible="false"
                    runat="server"></asp:Label><br />
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="700px" ShowHeader="true" >
                    <RowStyle Height="50px" />
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="SL">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>                      
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ID" HeaderText="Request ID" SortExpression="ID" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="Appdate" HeaderText="Application Date" SortExpression="Appdate" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="Accreditation_Number" HeaderText="Accr No." SortExpression="ID" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                          <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="InstName" HeaderText="Institute Name" SortExpression="ID" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                        
                           <asp:HyperLinkField HeaderStyle-Width="25%"
                            DataTextField="InstAddress" HeaderText="Institute Address" SortExpression="ID"
                            Target="_self" >
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                      
                        <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="CourseName" HeaderText="Course Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                       
                    </Columns>
                    <SelectedRowStyle BackColor="#87CEFA" ForeColor="Maroon" Font-Size="10" />
                    <PagerSettings Visible="False" />
                </asp:GridView>

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
                        
</asp:Content>
