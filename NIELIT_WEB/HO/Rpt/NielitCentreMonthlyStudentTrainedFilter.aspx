<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="NielitCentreMonthlyStudentTrainedFilter.aspx.cs" Inherits="HO_NielitCentreMonthlyStudentTrainedFilter" Debug="TRUE" %>

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
    Nielit Centre Student Trained
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


            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;

            if (!isBlankDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;


            if ((document.getElementById('<%=ddlSubcentreName.ClientID %>').value == "0") && (document.getElementById('<%=ddlCentreName.ClientID %>').value == "0")) {
                if (!isSelectedDrp("<%=ddlCentreName.ClientID %>", "Please Select Centre Name or Sub Centre Name"))
                    return false;

            }

            var PayFromDate = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            var PayToDate = document.getElementById('<%=txtDateto.ClientID %>').value;

            var year = new Date(PayFromDate).getFullYear();
            var month = new Date(PayFromDate).getMonth();
            var day = new Date(PayFromDate).getDate();
            var date1 = new Date(year + 1, month, day);
            var x = date1.format("dd-MMM-yyyy");

            if (!CompareDates(PayFromDate, PayToDate, "To Date should be more than From Date", true))
                return false;

            if (!CompareDates(PayToDate, x, "Date should be within an year", true))
                return false;

            var FromMonth = new Date(PayFromDate).getMonth();
            var ToMonth = new Date(PayToDate).getMonth();

            if (FromMonth == ToMonth) {
                alert("The Month should not be same in Batch End Date From and Batch End Date To.!");
                return false;
            }


            // Intitute name or sub centre name
            var centreType;


            var InstId;
            if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value == "0") {
                if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value != "0") {
                    InstId = document.getElementById('<%=ddlSubcentreName.ClientID %>').value;
                    centreType = "S";
                }

                if (document.getElementById('<%=ddlCentreName.ClientID %>').value != "0") {
                    InstId = document.getElementById('<%=ddlCentreName.ClientID %>').value;
                    centreType = "C";
                }
            }
            else {
                if (document.getElementById('<%=ddlSubcentreName.ClientID %>').value != "0") {
                    InstId = document.getElementById('<%=ddlSubcentreName.ClientID %>').value;
                    centreType = "S";
                }

            }

            var dateFrom, dateTo;
            if (document.getElementById('<%=txtDateFrom.ClientID %>').value != "") {
                dateFrom = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            }

            if (document.getElementById('<%=txtDateto.ClientID %>').value != "") {
                dateTo = document.getElementById('<%=txtDateto.ClientID %>').value;
            }

            //View report
            // window.open("NielitCentreStudentsTrainedRep.aspx?centreId=" + InstId + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&centrytype=" & centreType, 'report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);

            window.open("NielitCentreMonthlyStudentsTrainedRep.aspx?centreId=" + InstId + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&centreType=" + centreType, 'report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
            return false;

        }
    </script>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <%--<tr>
                    <td colspan="2" style="width: 66%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institutes"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">&nbsp;</td>
                </tr>--%>
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                            TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                            Style="height: 27px" Font-Bold="True">
                            <asp:ListItem Value="1">Accredited Centres</asp:ListItem>
                            <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>
                            <asp:ListItem Value="2">NIELIT Centre</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="NIELIT Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>



            <td style="width: 33%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" Width="100%" runat="server" SkinID="ddl250" AutoPostBack="True" OnSelectedIndexChanged="ddlCentreName_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>



        <tr>
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Sub Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlSubcentreName" Width="100%" runat="server" SkinID="ddl250" Enabled="false">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Batch End Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td class="auto-style1" colspan="2">
                <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Batch End Date To&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>

        </tr>
        <tr class="even">
            <td>
                <%--<asp:TextBox ID="txtDateFrom1" runat="server" OnKeyPress="return false" SkinID="txt210"
                    ToolTip="Date From"></asp:TextBox>--%>

                <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" ToolTip="Date From"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="imgdate" TargetControlID="txtDateFrom">
                </asp:CalendarExtender>
                <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" Width="200px" ReadOnly="false" ToolTip="Date To"></asp:TextBox>
                <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                    PopupButtonID="img1" TargetControlID="txtDateto">
                </asp:CalendarExtender>
                <img id="img1" alt="Calender" src="../../images/calendaricon.jpg" />
            </td>

        </tr>

    </table>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="NIELITCentreId" runat="server" />
            <asp:HiddenField ID="HNANFL" runat="server" />
            <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />

    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
