<%@ Page Title="Nielit Centre Payment Report For Virtual Academy Course" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="VANielitCentrePaymentReportForGst.aspx.cs" Inherits="VANielitCentrePaymentReportForGst" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Nielit Centre Payment Report For Virtual Academy Course
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            var DateFrom = 0;
            var DateTo = 0;
            var CourseId;

                if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;

            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;

            if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;

            var PayFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            var PayToDate = document.getElementById('<%=txtDateto.ClientID %>').value;

            if (!CompareDates(PayFromDate, PayToDate, "From date should be less than To Date", true))
                return false;

            var centreID;            
            //
            if (document.getElementById('<%=ddlCentreName.ClientID %>').value != "") {
                centreID = document.getElementById('<%=ddlCentreName.ClientID %>').value;
            }
            //

            var dateFrom, dateTo;
            if (document.getElementById('<%=txtDateFrom.ClientID %>').value != "") {
                dateFrom = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            }

            if (document.getElementById('<%=txtDateto.ClientID %>').value != "") {
                dateTo = document.getElementById('<%=txtDateto.ClientID %>').value;
            }

            //View report 
            var url = "VANielitCentrePaymentReportForGstRep.aspx?centreId=" + centreID + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;
            window.open(url);
            return false;
        }
    </script>

    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">

        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="NIELIT Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Payment Settled Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Payment Settled Date To&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>

        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCentreName_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="11" onpaste="return false;" Width="200px" 
                    AutoPostBack="True" ReadOnly="false" ToolTip="Date From" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="imgdate" TargetControlID="txtDateFrom">
                </asp:CalendarExtender>
                <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
            </td>

            <td>
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" Width="200px" ReadOnly="true" 
                    ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img11" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../../images/calendaricon.jpg" />
            </td>
        </tr>

    </table>
  <%--  <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="NIELITCentreId" runat="server" />
            <asp:HiddenField ID="HNANFL" runat="server" />
            <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
