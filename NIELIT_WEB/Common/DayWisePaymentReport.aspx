<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DayWisePaymentReport.aspx.cs"
    Inherits="Common_DayWisePaymentReport" MasterPageFile="~/MasterPages/main.master" Debug="True" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function validateFormFields() {

            var curdate = new Date().format("dd-MMM-yyyy");
            if (!isSelected("<%=ddlpaymentmode.ClientID %>", "Payment Mode Type"))
                return false;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            if (!isBlankDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            if (document.getElementById('<%=ddldatetype.ClientID %>')) {
                if (!isSelected("<%=ddldatetype.ClientID %>", "Date Type"))
                    return false;
            }

            var TransTypeId;
            if (!isSelected("<%=ddltytype.ClientID %>", "Payment Mode Type"))
                TransTypeId = 0;
            else
                TransTypeId = document.getElementById('<%=ddltytype.ClientID %>').value;
            var PayModeId = document.getElementById('<%=ddlpaymentmode.ClientID %>').value;
            var CourseCatId;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                CourseCatId = 0;
            else
                CourseCatId = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;
            var CourseId;
            if (document.getElementById('<%=ddlCourseName.ClientID %>').value == "0") {
                CourseId = 0;
            }
            else {
                CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;
            }
            var PayFromDate = document.getElementById('<%=txttDateFrom.ClientID %>').value;
            if (!CompareDates(PayFromDate, curdate, "Payment from date should be less than todays Date", true))
                return false;
            var PayToDate = document.getElementById('<%=txtDateto.ClientID %>').value;
            if (!CompareDates(PayToDate, curdate, "Payment To Date should be less than todays Date", true))
                return false;

            var AppTypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                AppTypeId = 0;
            else
                AppTypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;
            var PayStatusId;
            if (!isSelected("<%=ddlpaymentstatus.ClientID %>", "Payment Status"))
                PayStatusId = 0;
            else
                PayStatusId = document.getElementById('<%=ddlpaymentstatus.ClientID %>').value;
            if (!CompareDates(PayFromDate, PayToDate, "Payment from date should be less than Payment To Date", true))
                return false;


            // added by amit start
            var Gateway = '0';

            var gatewayElement = document.getElementById('<%=ddlgateway.ClientID %>');
            if (gatewayElement) {
                Gateway = gatewayElement.value;
            }
            // added by amit end

            var datetype = 0;
            if (document.getElementById('<%=ddldatetype.ClientID %>')) {
                if (!isSelected("<%=ddldatetype.ClientID %>", "Date Type"))
                    datetype = 0;
                else
                    datetype = document.getElementById('<%=ddldatetype.ClientID %>').value;
            }

            if (AppTypeId == '4' ) {
                var url = "../HO/Rpt/DayWisePaymentModuleCertificate.aspx?PayModeId=" + PayModeId + "&TransTypeId=" + TransTypeId + "&CourseId=" + CourseId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&PayStatusId=" + PayStatusId + "&Datetype=" + datetype + "&Gateway=" + Gateway;;
            }
	        else
	        {
                    if (AppTypeId == '5') {
                        var url = "../HO/Rpt/DayWisePaymentModuleCertificate.aspx?PayModeId=" + PayModeId + "&TransTypeId=" + TransTypeId + "&CourseId=" + CourseId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&PayStatusId=" + PayStatusId + "&Datetype=" + datetype + "&Gateway=" + Gateway;;
                    }
                    else {
                        // window.open("../HO/Rpt/PaymentRecieptReport.aspx?PayModeId=" + PayModeId + "&TransTypeId=" + TransTypeId + "&CourseCatId=" + CourseCatId + "&CourseId=" + CourseId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&PayStatusId=" + PayStatusId);
                        var url = "../HO/Rpt/DayWisePayment.aspx?PayModeId=" + PayModeId + "&TransTypeId=" + TransTypeId + "&CourseCatId=" + CourseCatId + "&CourseId=" + CourseId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&PayStatusId=" + PayStatusId + "&Datetype=" + datetype + "&Gateway=" + Gateway;
                    }
            }

            window.open(url);
            return false;
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Day Wise Payment Report" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Transaction Type"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td class="style1">
      
                <%-- AutoPostBack="true" is added by amit  --%>
                <asp:DropDownList 
                    ID="ddlpaymentmode" runat="server" 
                    AutoPostBack="true" 
                    Height="22px" SkinID="ddl250"
                    OnSelectedIndexChanged="ddlpaymentmode_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                
                <%-- added by amit start --%>
                 <asp:DropDownList 
                    ID="ddlgateway" runat="server" 
                    AutoPostBack="true" 
                    Visible="false"
                    Height="22px" SkinID="ddl250"
                    OnSelectedIndexChanged="ddlGateway_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%-- added by amit end --%>
          
            </td>
            <td class="style1">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddltytype" runat="server" Height="22px" SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddltytype" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td class="style1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course Name "></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Payment Status"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:DropDownList ID="ddlpaymentstatus" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--All--</asp:ListItem>
                    <asp:ListItem Value="S">Success</asp:ListItem>
                    <asp:ListItem Value="F">Failed</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Payment From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Payment To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Lbdatetype" runat="server" SkinID="CaptionLabel" Text="Date Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
            </td>
            <td>
                <asp:DropDownList ID="ddldatetype" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    <asp:ListItem Value="P">Payment Date</asp:ListItem>
                    <asp:ListItem Value="SV">Settled/Verified Date</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return validateFormFields()" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click1" /></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
