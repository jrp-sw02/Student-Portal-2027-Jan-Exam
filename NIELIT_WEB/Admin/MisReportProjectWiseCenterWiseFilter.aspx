<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="MisReportProjectWiseCenterWiseFilter.aspx.cs" Inherits="Admin_MisReportProjectWiseCenterWiseFilter" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
     MIS Report Details Of Students  ProjectWise CenterWise Filter 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <style type="text/css">
        .PromptCSS {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: Courier New;
            border: none;
            height: 20px;
            padding-left: 90px;
        }
    </style>
    <script type="text/javascript" language="javascript">
       
        function OpenWindow() {
            if (!isSelected("<%=ddlStatus.ClientID %>", "Status"))
                return false;
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

            if (!CompareDates(datefrom, dateto, "DateFrom should be less then  DateTo", true))
                return false;
            

            document.forms[0].target = "_blank";
        
        
     }
 </script>
      <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td style="width: 36%;" valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Project Name &lt;b class='mandatory'&gt;&lt;/b&gt;"
                    Width="100%"></asp:Label>
            </td>
            <td colspan="2" style="width: 64%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td style="width: 36%;" valign="top">
                <asp:DropDownList ID="ddlProjectName" runat="server" SkinID="ddl250" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlProjectName_SelectedIndexChanged" EnableTheming="True">
                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlProjectName"
                    PromptText="     type Project Name"
                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                </asp:ListSearchExtender>
            </td>
            <td style="width: 64%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentre" runat="server" AutoPostBack="True" Height="22px" Style="width: 470px;"
                            OnSelectedIndexChanged="ddlCentre_SelectedIndexChanged">
                            <asp:ListItem Value="0">--ALL--</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlCentre"
                                     PromptText="At first click on dropdown and type Centre Name"
                                     PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                                 </asp:ListSearchExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>

        </tr>
        <tr>

            <td width="33%">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Status  &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                    Width="100%"></asp:Label>
            </td>

        </tr>
        <tr>
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210" MaxLength="11" onpaste="return false;"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210" MaxLength="11" onpaste="return false;"
                            ToolTip="Date To" AutoPostBack="True" OnTextChanged="txtDateto_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtDateto">
                        </asp:CalendarExtender>
                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="99">ALL</asp:ListItem>
                            <asp:ListItem Value="1">Trained/Undergoing training </asp:ListItem>
                            <asp:ListItem Value="2">Certified </asp:ListItem>
                            <asp:ListItem Value="3">Placed</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>

        </tr>
    </table>
<div style="text-align: right; margin-top: 10px">
    <asp:Button ID="btnView" runat="server" Text="Generate Report" 
    OnClick="btnView_Click" />
    <%--OnClientClick="return OpenWindow();--%>

<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
 <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"> </asp:Label>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

