<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPages/MyInfo.master" CodeFile="CentreWiseBatchDetailsFilter.aspx.cs" Inherits="Common_CentreWiseBatchDetailsFilter" %>


<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Centre Wise Batch Details Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            //Project Name
            var centreId
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreId = document.getElementById('<%=ddlCentreName.ClientID %>').value;
            var reportId
            if (!isSelected("<%=ddlReportType.ClientID %>", "Report Type"))
                return false;
            else
                reportId = document.getElementById('<%=ddlReportType.ClientID %>').value;
            if (reportId == "C") {
                var courseId
                if (!isSelected("<%=ddlCourse.ClientID %>", "Course Name"))
                    return false;
                else
                    courseId = document.getElementById('<%=ddlCourse.ClientID %>').value;

                var batchId
                if (!isSelected("<%=ddlBatch.ClientID %>", "Batch Name"))
                    return false;
                else
                    batchId = document.getElementById('<%=ddlBatch.ClientID %>').value;
                window.open("../HO/Rpt/CentreWiseBatchDetails.aspx?centreId=" + centreId + "&reportId=" + reportId + "&courseId=" + courseId + "&batchId=" + batchId);
                return false;
            }
            else if (reportId == "D") {
                // Batch Dates
                if (!isBlankDate("<%=txtBatchFrom.ClientID %>", "Batch Start From", "dd-MMM-yyyy"))
                    return false;
                if (!isDate("<%=txtBatchFrom.ClientID %>", "Batch Start From", "dd-MMM-yyyy"))
                    return false;
                if (!isBlankDate("<%=txtBatchto.ClientID %>", "Batch Start To", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtBatchto.ClientID %>", "Batch Start To", "dd-MMM-yyyy"))
                return false;
            var batchFromDate = document.getElementById('<%=txtBatchFrom.ClientID %>').value;
                var batchToDate = document.getElementById('<%=txtBatchto.ClientID %>').value;
                if (!CompareDates(batchFromDate, batchToDate, " Batch Start From Date should be less than Batch Start To Date", true))
                    return false;

                window.open("../HO/Rpt/CentreWiseBatchDetails.aspx?centreId=" + centreId + "&reportId=" + reportId + "&batchFromDate=" + batchFromDate + "&batchToDate=" + batchToDate);
                return false;
            }
           

           




            //View report
          
        }
    </script>
    <style type="text/css">
        .PromptCSS {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: CourierNew;
            height: 20px;
            margin-left: 100px;
        }
    </style>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblCentreName" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblReport" runat="server" SkinID="CaptionLabel" Text="Report Display From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="false">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
             
                        <asp:DropDownList ID="ddlReportType" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="C">Batches of a course</asp:ListItem>
                            <asp:ListItem Value="D">Batches within dates</asp:ListItem>
                        </asp:DropDownList>
                   
            </td>
          
        </tr>
        <tr id="Trcourse" runat="server" visible="false">
            <td>
                <asp:Label ID="lblcourse" runat="server" SkinID="CaptionLabel" Text="Course Name"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblbatch" runat="server" SkinID="CaptionLabel" Text="Batch Name"></asp:Label>
            </td>
            
        </tr>
        <tr id="TrcourseInput" runat="server" class="even" visible="false">
            <td>
                
                        <asp:DropDownList ID="ddlCourse" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>                            
                        </asp:DropDownList>
                   
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlBatch" runat="server" Height="22px" SkinID="ddl250" >
                            <asp:ListItem Value="99999">--ALL--</asp:ListItem>                            
                        </asp:DropDownList>
                    </ContentTemplate>

                </asp:UpdatePanel>
            </td>           
        </tr>

        <tr id="trBatchdate" runat="server" visible="false">
            <td>
                <asp:Label ID="lblbatchfrom" runat="server" SkinID="CaptionLabel" Text="Batch Starting Date From"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblbatchto" runat="server" SkinID="CaptionLabel" Text="Batch Starting Date To"></asp:Label>
            </td>
            
        </tr>

        <tr id="trbatchfrom" runat="server" visible="false">
            
            <td width="33%">
                <asp:TextBox ID="txtBatchFrom" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBatchFrom"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtBatchto" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgto" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBatchto"
                    Format="dd-MMM-yyyy" PopupButtonID="imgto">
                </asp:CalendarExtender>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();"  />
        <asp:Button ID="btnReset" runat="server" Text="Reset"  OnClick="btnReset_Click" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   
                    <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
