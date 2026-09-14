<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="PaymntTransactionReciept.aspx.cs" Inherits="HO_HOPaymntRecieptReport"
    %>

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
            if (!isNumber("<%=Txttransno.ClientID %>", "Transaction No"))
                return false;

            if (document.getElementById('<%=ddldatetype.ClientID %>')) {
                if (!isSelected("<%=ddldatetype.ClientID %>", "Date Type"))
                    return false;
            }

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
            var transactionno;
            if (document.getElementById('<%=Txttransno.ClientID %>').value == "") {
                transactionno = 0;
            }
            else {
                transactionno = document.getElementById('<%=Txttransno.ClientID %>').value;
            }
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

            var manualsettled = 0;
            if (document.getElementById('<%=ddlsettled.ClientID %>')) {
                if (!isSelected("<%=ddlsettled.ClientID %>", "Is Settled"))
                    manualsettled = 0;
                else
                    manualsettled = document.getElementById('<%=ddlsettled.ClientID %>').value;
            }

            var regcenterid = 0;
            if (document.getElementById('<%=ddlregcenter.ClientID %>')) {
                //                if (!isSelected("<%=ddlregcenter.ClientID %>", "Regional Center"))
                //                    regcenterid = 0;
                //                else
                regcenterid = document.getElementById('<%=ddlregcenter.ClientID %>').value;
            }

            var TransTypeId;
            if (!isSelected("<%=ddltytype.ClientID %>", "Transaction Type"))
                TransTypeId = 0;
            else
                TransTypeId = document.getElementById('<%=ddltytype.ClientID %>').value;

            if (!CompareDates(PayFromDate, PayToDate, "Payment from date should be less than Payment To Date", true))
                return false;

            var ExamID;
            ExamID = document.getElementById('<%=ddlexamname.ClientID %>').value;

            var datetype = 0;
            if (document.getElementById('<%=ddldatetype.ClientID %>')) {
                if (!isSelected("<%=ddldatetype.ClientID %>", "Date Type"))
                    datetype = 0;
                else
                    datetype = document.getElementById('<%=ddldatetype.ClientID %>').value;
            }

            // added by amit start
            var Gateway = 0;
            var gatewayElement = document.getElementById('<%=ddlgateway.ClientID %>');
            if (gatewayElement) {
                Gateway = gatewayElement.value;
            }
            // added by amit end

            // window.open("../HO/Rpt/PaymentRecieptReport.aspx?PayModeId=" + PayModeId + "&TransTypeId=" + TransTypeId + "&CourseCatId=" + CourseCatId + "&CourseId=" + CourseId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&PayStatusId=" + PayStatusId);
            var url = "../HO/Rpt/PaymentRecieptReport.aspx?PayModeId=" + PayModeId + "&TransTypeId=" + TransTypeId + "&CourseCatId=" + CourseCatId + "&CourseId=" + CourseId + "&PayFromDate=" + PayFromDate + "&PayToDate=" + PayToDate + "&AppTypeId=" + AppTypeId + "&PayStatusId=" + PayStatusId + "&Transno=" + transactionno + "&ManualSettled=" + manualsettled + "&Regcentreid=" + regcenterid + "&ExamID=" + ExamID + "&Datetype=" + datetype + "&Gateway="+Gateway;
            window.open(url);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Payment Transaction Detail" runat="server"></asp:Label>
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
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Category "></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>--%>
                <asp:DropDownList ID="ddlpaymentmode" runat="server" Height="22px" SkinID="ddl250"
                    OnSelectedIndexChanged="ddlpaymentmode_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>

                    
                <%-- added by amit start --%>
                 <asp:DropDownList 
                    ID="ddlgateway" runat="server" 
                    AutoPostBack="true" 
                    Visible="false"
                    Height="22px" SkinID="ddl250"
                   >
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%-- added by amit end --%>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlpaymentmode" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
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
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
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
                <asp:Label ID="Labelexamname" runat="server" SkinID="CaptionLabel" Text="Exam Name"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel8" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlexamname" runat="server" skinid="ddl250" AutoPostBack="True">
                    <asp:listitem value="0">All</asp:listitem>
                </asp:DropDownList>
                 </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
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
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Transaction Number"></asp:Label>
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
                <asp:TextBox ID="Txttransno" runat="server" SkinID="txt248" onkeypress="checkNumber(this,8,0,event)"
                    MaxLength="8"></asp:TextBox>
            </td>
        </tr>
        <tr id="trregcentre" runat="server">
            <td>
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Regional Center"></asp:Label>
            </td>
            <td>
                <asp:label id="Label9" runat="server" skinid="CaptionLabel" text="Payment Status"></asp:label>
            </td>
            <td>
                <asp:label id="Lbldatetype" runat="server" skinid="CaptionLabel" text="Date Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr class="even" id="trregcentre1" runat="server">
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlregcenter" runat="server" Height="22px" SkinID="ddl250">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:dropdownlist id="ddlpaymentstatus" runat="server" height="22px" skinid="ddl250"
                    onselectedindexchanged="ddlpaymentstatus_SelectedIndexChanged" autopostback="true">
                    <asp:listitem value="0">--All--</asp:listitem>
                    <asp:listitem value="S">Success</asp:listitem>
                    <asp:listitem value="F">Failed</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td>
                <asp:dropdownlist id="ddldatetype" runat="server" height="22px" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                    <asp:listitem value="P">Payment Date</asp:listitem>
                    <asp:listitem value="SV">Settled/Verified Date</asp:listitem>
                </asp:dropdownlist>
            </td>
        </tr>
        <tr id="trsettled" runat="server">
            <td>
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Is Settled"></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr class="even" id="trsettled1" runat="server">
            <td>
                <asp:DropDownList ID="ddlsettled" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="F">False</asp:ListItem>
                    <asp:ListItem Value="T">True</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td>
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
