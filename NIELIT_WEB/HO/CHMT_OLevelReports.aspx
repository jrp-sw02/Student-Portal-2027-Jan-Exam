<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="CHMT_OLevelReports.aspx.cs" Inherits="HO_CHMT_OLevelReports"  Debug="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .error
{
    background-color: #EACFCE; 
    padding-top:4px; 
    padding-bottom:4px; 
    padding-left:4px; 
    color: Red;
    border: 1px solid maroon; 
    font-size: 11pt; 
   
}
       .head1
{
	font:bold 13px arial;
	background-color :  #31597C;
	color :#ffffff;
	line-height: 20px; 
	padding-left:2px;
	
}
/*.sample3 Tr.head1 td
{
	padding-left:3px;
}*/
.gdalternate1
{
	font:normal 12px arial;
	background-color : #E6F0F0;
	color :#000000;
	line-height: 18px; 
}
/*/*.sample3 Tr.gdalternate1 td
{
	padding-left:3px;
}*/
.gdrow1
{
	background-color:#c9d7e2;
	font:normal 12px arial;
	color: #000000;
	line-height: 18px; 
}
/*.sample3 Tr.gdrow1 td
{
	padding-left:3px;
}*/
       .auto-style1
       {
           height: 20px;
       }
    </style>
    <script language ="javascript"  type="text/javascript">
        function printwindow() {
            window.focus();
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">

    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
      <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" Height="22px"  />
    &nbsp; <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <div>
         <asp:Label id ="lblError" runat ="server" CssClass="error" ForeColor="#FF3300"  Width="100%" Visible="false"> </asp:Label>
        <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
            width="100%">

           
            <%--  <tr >
                <td class="auto-style1" colspan="2">
                    
                </td>
            </tr>--%>
            
             <tr class="odd">
                <td colspan="1" class="auto-style2">
                    <label id="Label2" runat="server">Report</label>
                </td>
                <td>
                  
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>

                            <asp:DropDownList ID="ddlReport" runat="server" Height="26px" Enabled="true" Width="449px" AutoPostBack="true"  OnSelectedIndexChanged="ddlReport_SelectedIndexChanged"  >

                                 <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                 <asp:ListItem Value="1">Report- CHM-T Compiled Result</asp:ListItem>                             
                                 <asp:ListItem Value="2">Report- CHM-T Finalized Result (Module Wise)</asp:ListItem>
                                 <asp:ListItem Value="3">Report- CHM-T Finalized Result (Centre Wise)</asp:ListItem>
                                 <asp:ListItem Value="4">Report- CHM-T Finalized Result</asp:ListItem>
                                 <asp:ListItem Value="5">Report- CHM-T Employability Skill Result</asp:ListItem>
                                 <asp:ListItem Value="6">Report- CHM-T Employability Skill Summary Report</asp:ListItem>

                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlReport" EventName="SelectedIndexChanged" />
                            <%--<asp:AsyncPostBackTrigger ControlID="rbtnTheoryMarks" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbtnPracticalMarks" EventName="CheckedChanged" />--%>
                        </Triggers>
                       
                    </asp:UpdatePanel>

                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2"
                        runat="server"
                        ControlToValidate="ddlReport"
                        InitialValue="0"
                        ErrorMessage="Please select the  report."
                        Display="Dynamic"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
                         
                          <tr class="odd">
                <td colspan="1" class="auto-style2">
                    <label id="Label1" runat="server">Exam </label>
                </td>
                <td>
                  
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <asp:DropDownList ID="ddlExam" runat="server" Height="25px" Enabled="true" Width="209px" AutoPostBack="true"  onchange="handleClick()"  >
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlExam" EventName="SelectedIndexChanged" />
                            <%--<asp:AsyncPostBackTrigger ControlID="rbtnTheoryMarks" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbtnPracticalMarks" EventName="CheckedChanged" />--%>
                        </Triggers>
                       
                    </asp:UpdatePanel>

                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1"
                        runat="server"
                        ControlToValidate="ddlExam"
                        InitialValue="0"
                        ErrorMessage="Please select a value from the list."
                        Display="Dynamic"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
           


            <tr>
                <td>

                </td>
                <td>
                     <asp:Button  runat="server" Text="View" ID ="btnView" Width="150px" OnClick="btnView_Click"/>
                </td>
            </tr>

            

            <%--<tr >
                <td colspan="2">
                    <asp:LinkButton ID="btnDownloadPDF" runat="server" OnClick="btnDownloadPDF_Click" Visible  ="true">Download Compiled Result Report</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btnCentreWise" runat="server"  Visible  ="true" OnClick="btnCentreWise_Click">Download Compiled Result Centre-Wise Report</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btnModuleWise" runat="server" Visible  ="true" OnClick="btnModuleWise_Click">Download Compiled Result Module -Wise Report</asp:LinkButton>
                </td>
            </tr>--%>
        </table>
    </div>
    <div id ="divReportData" runat ="server" style ="width : 100%"></div>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

