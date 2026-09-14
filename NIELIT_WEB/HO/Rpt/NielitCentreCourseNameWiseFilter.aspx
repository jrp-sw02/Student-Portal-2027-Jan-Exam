<%@ Page Title="NIELIT Center CourseWise Students Details Report" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" 
    AutoEventWireup="true" CodeFile="NielitCentreCourseNameWiseFilter.aspx.cs" Inherits="NielitCentreCourseNameWiseFilter" %>

<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    NIELIT Center Course Name Wise Students
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function OpenWindow()
        {
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            if (!isSelected("<%=ddlCourseCat.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isBlankDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;

            if (document.getElementById('<%=ddlCentreName.ClientID %>')) {
                if (!isSelected("<%=ddlCentreName.ClientID %>", "Please Select Centre Name"))
                                return false;
            }

            var startDate;
            if (!isDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            else
                startDate = document.getElementById('<%=txttDateFrom.ClientID %>').value;
            
            var endDate;
            if (!isDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            else
                endDate = document.getElementById('<%=txtDateto.ClientID %>').value;

            if (!CompareDates(startDate, endDate, "DateFrom should be less then  DateTo", true))
                return false;

            var centreID;

                if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                    return false;
            else
                    centreID = document.getElementById('<%=ddlCentreName.ClientID %>').value;

            var courseCat;
            if (!isSelected("<%=ddlCourseCat.ClientID %>", "Course Category"))
                return false;
            else
                courseCat = document.getElementById('<%=ddlCourseCat.ClientID %>').value;

            var courseID;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            else
                courseID = document.getElementById('<%=ddlCourseName.ClientID %>').value;
          
            var url = "NielitCentreCourseNameWiseRep.aspx?startDate=" + startDate + "&endDate=" + endDate + "&courseCat=" + courseCat + "&centreID=" + centreID + "&courseID=" + courseID;
            window.open(url);

            return false;

        }
 </script>
    
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


      <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td style="width: 34%;" valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                    Width="100%"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Catogory &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>

        <tr class="even">
            <td style="width: 34%;" valign="top">

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                <asp:DropDownList ID="ddlCentreName" runat="server" Style="width: 270px;" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCentreName_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--"</asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlCentreName"
                    PromptText="  Select Centre Name"
                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                </asp:ListSearchExtender>
                                </ContentTemplate>
                        </asp:UpdatePanel>


            </td>

            <td style="width: 33%;" valign="top" >

                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                <asp:DropDownList ID="ddlCourseCat" runat="server" Style="width: 270px;" AutoPostBack="True"  
                    OnSelectedIndexChanged="ddlCourseCat_SelectedIndexChanged" EnableTheming="True">
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlCourseCat"
                                     PromptText="  Select Course Category"
                                     PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                                 </asp:ListSearchExtender>
                                </ContentTemplate>
                        </asp:UpdatePanel>
            </td>

            <td style="width: 33%;" valign="top" >
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" Height="22px" Style="width: 270px;">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlCourseName"
                                     PromptText="  Select Course Name"
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
                        <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210" MaxLength="11" onpaste="return false;"
                            ToolTip="Date To" AutoPostBack="True" ></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtDateto">
                        </asp:CalendarExtender>
                        <img id="img1" alt="Calender" src="../../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

<div style="text-align: right; margin-top: 10px">
    <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>

 <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"> </asp:Label>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

