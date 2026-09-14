<%@ Page Title="Protsahan Puraskar Students Installments" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="rptFinanceCandDetailsWithInstallmentsDetailsPuraskar.aspx.cs" Inherits="rptFinanceCandDetailsWithInstallmentsDetailsPuraskar" %>

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
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course Name"))
                return false;
           
            
            return true;
        }
    </script>
    <style type="text/css">
        .custom {
            font-family: Courier;
            color: red;
            font-size: 20px;
        }
    </style>
</asp:Content>


<asp:content id="Content3" contentplaceholderid="cpButtons" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  visible="false"/>&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to excel file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" visible="false" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="30%" width="15%" />
  
</asp:content>
<asp:content id="Content4" contentplaceholderid="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" AutoPostBack="True" 
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged" Enabled="false" EnableTheming="True">
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourse" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                    <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                 <asp:DropDownList ID="ddlExamCycle" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                                    <asp:ListItem>--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
        <tr>
           
            <td align="left" valign="top" >
                                <asp:Label ID="Label1" runat="server" Text="Session / Examination &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                                </asp:Label>
                            </td>          
         
           
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
             <asp:DropDownList ID="ddlSessionExam" runat="server" SkinID="ddl250"  TabIndex="27"  AutoPostBack ="true"
                                  Style="display: inline;" OnSelectedIndexChanged ="ddlSessionExam_SelectedIndexChanged">
                                            <asp:ListItem Value="0">--Select Exam Name--</asp:ListItem>
                                        </asp:DropDownList>
                                
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>                           
                        </asp:UpdatePanel>
                </td>

           
           
        </tr>
                    </table>
     <div style="text-align: center; margin-top: 10px">
          <asp:Button ID="btnShowReport" runat="server" Text="Show Report " OnClientClick="return Validate();"
                    OnClick="btnShowReport_Click" /> &nbsp;&nbsp;&nbsp;
               
            </div>        
</asp:content>
<asp:content id="Content2" contentplaceholderid="cpReportHeader" runat="Server">
    Protsahan Puraskar Installments Details Report (Finance)</asp:content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate> 
    <asp:Label ID="lblheading" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar Installments Details Report (Finance)" Visible="false"></asp:Label>     
    
                                <br /><br />
     <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
           <asp:HiddenField ID="hddbtnR" runat="server" />
                                 </ContentTemplate></asp:UpdatePanel>
                           <div id="divGrid" runat="server" visible="true">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblerrorStatistics" Visible="false"
        runat="server"></asp:Label>
                <br />              
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label5" Visible="false"
                    runat="server"></asp:Label><br />  
                            </ContentTemplate>
        </asp:UpdatePanel>
    </div>       
</asp:content>