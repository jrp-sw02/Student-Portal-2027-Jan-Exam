<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="NielitStatisticsReportFilter.aspx.cs" Inherits="Admin_NielitStatisticsReportFilter" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    Nielit Statistics Report Filter 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <script type="text/javascript" language="javascript">
       
        function OpenWindow() {
            

         if (!isBlankDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
             return false;
         var datefrom;
         if (!isDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
             return false;
         else
             datefrom = document.getElementById('<%=txttDateFrom.ClientID %>').value;

         if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
             return false;
         var dateto;
         if (!isDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
             return false;
         else
             dateto = document.getElementById('<%=txtDateto.ClientID %>').value;

                        
            if (!CompareDates(datefrom, dateto, "DateFrom should be less then or equal to DateTo", true))
                return false;
                                   

            document.forms[0].target = "_blank";
        
        
     }
 </script>
     <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
               <tr id="TrExamHead" runat="server">
            <td width="33%">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
             <td width="33%">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Visible="false"  Text="Project Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                    <Triggers>
                      
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
            </td>  
            <td width="33%">
                <asp:DropDownList ID="ddlProjectName" runat="server" SkinID="ddl250" Width="185px" TabIndex="27"   Visible="false" 
                                  Style="display: inline;" >
                                            <asp:ListItem Value="0">--Select Project Name--</asp:ListItem>
                                        </asp:DropDownList>
            </td>                 
        </tr>
       
       
        </table> 
<div style="text-align: right; margin-top: 10px">

    <asp:Button ID="btnView" runat="server" Text="Generate Report" 
    OnClick="btnView_Click" OnClientClick="return OpenWindow();" />
<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
 <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"> </asp:Label>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

