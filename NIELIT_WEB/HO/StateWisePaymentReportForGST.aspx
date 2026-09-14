<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="StateWisePaymentReportForGST.aspx.cs" Inherits="HO_StateWisePaymentReportForGST" Debug ="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="State wise payment report for GST" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function validateFormFields() {
            if (!isSelected("<%=ddlpaymentmode.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlDateRange.ClientID %>", "Date Range"))
                return false;
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;

            return true;
        }

    </script>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Date Range &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Start Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDateRange" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="even">
            <td class="style1">
      
                <asp:DropDownList ID="ddlpaymentmode" runat="server" AutoPostBack="true" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlpaymentmode_SelectedIndexChanged">
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
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlDateRange" runat="server" Height="22px" SkinID="ddl250"
                    OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    <asp:ListItem Value="1">From Beginning</asp:ListItem>
                    <asp:ListItem Value="2">From Start Date</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style1">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210"
                            Enabled="false" ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" runat="server" visible="false" alt="Calender" src="../images/calendaricon.jpg" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDateRange" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="even">
            <td class="style1">
                <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style1">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td class="style1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" Enabled="false">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Registration</asp:ListItem>
                            <asp:ListItem Value="2">Examination</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="Download" OnClientClick="return validateFormFields()"
            OnClick="btnView_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
