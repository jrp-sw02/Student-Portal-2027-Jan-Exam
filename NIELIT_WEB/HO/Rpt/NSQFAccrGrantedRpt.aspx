<%@ Page Title="NSQF Accreditation Applied Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="NSQFAccrGrantedRpt.aspx.cs" Inherits="NSQFAccrGrantedRpt" %>

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
            if (!isBlankDate("<%=txtPaymentFromDate.ClientID %>", " From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txPaymentToDate.ClientID %>", " To Date", "dd-MMM-yyyy"))
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
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" visible="true" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="30%" width="15%" />
  
</asp:content>

<asp:content id="Content4" contentplaceholderid="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr id="nc" runat="server" visible="false">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Payment Status"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr id="ncc" runat="server" visible="false" class="even">
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
                                <asp:DropDownList ID="ddlPaymentStatus" runat="server" SkinID="ddl250" Width="185px" TabIndex="27"  
                                  Style="display: inline;" >
                                          
                                <asp:ListItem Value="2" Selected="True" >---All---</asp:ListItem>
                                <asp:ListItem Value="1">Success</asp:ListItem>
                                <asp:ListItem Value="0" >Failed</asp:ListItem> 
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
                                <asp:Label ID="Label6" runat="server" Text="Choose one of the options  &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                                </asp:Label>
                            </td>
           
          <td align="left" valign="top" colspan="2" >
                              
               <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:RadioButtonList ID="RdoNSQFCourseRptChoice" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="580px" AutoPostBack="True" OnSelectedIndexChanged="RdoNSQFCourseRptChoice_SelectedIndexChanged"
                                    Style="height: 27px" Font-Bold="True">
                                    <asp:ListItem Value="1">City Wise</asp:ListItem>
                                    <asp:ListItem Value="2">Level Wise</asp:ListItem>
                                    <asp:ListItem Value="3">State Wise</asp:ListItem>
                                     <asp:ListItem Value="4">NSQF Course Wise</asp:ListItem>
                                     <asp:ListItem Value="5">Within a Period</asp:ListItem>
                                </asp:RadioButtonList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RdoNSQFCourseRptChoice" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                            </td>
           
        </tr>
         <tr id="dc2" runat="server" visible="true">
             <td align="left" valign="top" >
                                <asp:Label ID="Label7" runat="server" Text="NSQF Course  &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                                </asp:Label>
                            </td>   
            <td align="left" valign="top" >
                                <asp:Label ID="Label1" runat="server" Text="From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                                </asp:Label>
                            </td>
           
          <td align="left" valign="top" >
                                <asp:Label ID="Label2" runat="server" Text=" To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                                </asp:Label>
                            </td>
           
        </tr>
        <tr id="dc1" runat="server" visible="true">
             <td >
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250" Enabled="false" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
            
                            </ContentTemplate>                           
                        </asp:UpdatePanel>
                </td>   
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
             <asp:TextBox ID="txtPaymentFromDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" Enabled="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtPaymentFromDate">
                                </asp:CalendarExtender>                                
                                <img id="img2" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>                           
                        </asp:UpdatePanel>
                </td>

            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
            <asp:TextBox ID="txPaymentToDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" Enabled="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txPaymentToDate">
                                </asp:CalendarExtender>
                                <img id="img3" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                                
                            </ContentTemplate>                          
                        </asp:UpdatePanel>
                </td>
           
        </tr>        
        
                    </table>
     <div style="text-align: center; margin-top: 10px">
         <asp:Button ID="btnReportChoice" runat="server" Text="Show Report " OnClientClick="return Validate();"
                    OnClick="btnReportChoice_Click"  /> &nbsp;&nbsp;&nbsp;
          <asp:Button ID="btnShowStatistics" runat="server" Text="Show Statistics " OnClientClick="return Validate();"
                    OnClick="btnShowStatistics_Click" visible="false" /> &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnShowCandidateDetails" runat="server" Text="Show Candidate details" OnClientClick="return Validate();"
                    OnClick="btnShowCandidateDetails_Click" visible="false" />
            </div>        
</asp:content>
<asp:content id="Content2" contentplaceholderid="cpReportHeader" runat="Server">
    NSQF Accreditation Grant Report</asp:content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate> 
    <asp:Label ID="lblheading" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="NSQF Accreditation Grant Report" Visible="false"></asp:Label>     
     <asp:Label ID="lblheadingCandDetails" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="NSQF Accreditation Grant Report" Visible="false"></asp:Label><br /><br />
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