<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master"
    CodeFile="NielitCentrePeriodWiseStudentFilter.aspx.cs" Inherits="NielitCentrePeriodWiseStudentFilter" %>

<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Nielit Centre PeriodWise Students Report 
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
            var centreId
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            if (!isSelected("<%=ddlReportType.ClientID %>", "Report (Year) Type"))
                return false;
            if (!isBlankDate("<%=txtBatchFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtBatchto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;

            var centreID;
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreID = document.getElementById('<%=ddlCentreName.ClientID %>').value;

            var TypeYear
            if (!isSelected("<%=ddlReportType.ClientID %>", "Report Type"))
                return false;
            else
                TypeYear = document.getElementById('<%=ddlReportType.ClientID %>').value;

            var FromDate;
            if (!isDate("<%=txtBatchFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            else
                FromDate = document.getElementById('<%=txtBatchFrom.ClientID %>').value;

            var ToDate;
            if (!isDate("<%=txtBatchto.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            else
                ToDate = document.getElementById('<%=txtBatchto.ClientID %>').value;

            var rdSearchby = "";
            var rb = document.getElementById("<%=Rdsearchby.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    rdSearchby = radio[i].value;
                    break;
                }
            }

            var categoryId = 0, genderId = 0;

            if (rdSearchby == "N") {

                var urlN = "NielitCentrePeriodWiseStudentRep.aspx?centreID=" + centreID + "&TypeYear=" + rdSearchby + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&categoryId=" + categoryId + "&genderId=" + genderId;
                window.open(urlN);
                return false;
            }
            else if (rdSearchby == "C") {

                if (!isSelected("<%=ddlCastCategory.ClientID %>", "Cast Category"))
                    return false;
                else
                    categoryId = document.getElementById('<%=ddlCastCategory.ClientID %>').value;

                var urlC = "NielitCentrePeriodWiseStudentRep.aspx?centreID=" + centreID + "&TypeYear=" + rdSearchby + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&categoryId=" + categoryId + "&genderId=" + genderId;
                window.open(urlC);
                return false;
            }
            else if (rdSearchby == "G") {

                if (!isSelected("<%=ddlGender.ClientID %>", "Gender"))
                    return false;
                else
                    genderId = document.getElementById('<%=ddlGender.ClientID %>').value;

                var urlG = "NielitCentrePeriodWiseStudentRep.aspx?centreID=" + centreID + "&TypeYear=" + rdSearchby + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&categoryId=" + categoryId + "&genderId=" + genderId;
                window.open(urlG);
                return false;
            }
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
                <asp:Label ID="lblReport" runat="server" SkinID="CaptionLabel" Text="Year Type (Report Display On) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>

        </tr>
        <tr class="even">
            <%--<td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" runat="server" Height="30px" SkinID="ddl250" 
                            AutoPostBack="false" Width="125px">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>--%>
            <td style="width: 34%;" valign="top">
                <asp:DropDownList ID="ddlCentreName" runat="server" Style="width: 270px;" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCentreName_SelectedIndexChanged" EnableTheming="True">
                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlCentreName"
                    PromptText="  Select Centre Name"
                    PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                </asp:ListSearchExtender>
            </td>



            <td>

                <asp:DropDownList ID="ddlReportType" runat="server" Height="21px" SkinID="ddl250"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" Width="182px">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    <asp:ListItem Value="C">Calendar Year</asp:ListItem>
                    <asp:ListItem Value="F">Financial Year</asp:ListItem>
                </asp:DropDownList>

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
                <asp:TextBox ID="txtBatchFrom" runat="server" Width="200px" MaxLength="11" AutoPostBack="True" OnTextChanged="txtBatchFrom_TextChanged"></asp:TextBox>
                <img id="imgFrom" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBatchFrom"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtBatchto" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgto1" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBatchto"
                    Format="dd-MMM-yyyy" PopupButtonID="imgto">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="20%" valign="top">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Search By:- &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>

                <asp:RadioButtonList ID="Rdsearchby" runat="server" RepeatDirection="Horizontal"
                    TabIndex="2" Width="412px" AutoPostBack="True" Style="height: 27px" Font-Bold="True" OnSelectedIndexChanged="Rdsearchby_SelectedIndexChanged">
                    <asp:ListItem Value="N" Selected="True">None</asp:ListItem>
                    <asp:ListItem Value="C">Category</asp:ListItem>
                    <asp:ListItem Value="G">Gender</asp:ListItem>
                </asp:RadioButtonList>


            </td>
        </tr>
        <tr id="trCategory" runat="server" visible="false">

            <td width="33%">
                <asp:Label ID="lblCategory" Text="Category" runat="server"></asp:Label>
            </td>
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCastCategory" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true" Width="295px">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trGender" runat="server" visible="false">

            <td width="33%">
                <asp:Label ID="lblGender" Text="Gender" runat="server"></asp:Label>
            </td>
            <td width="33%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlGender" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="true" Width="295px">
                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>



    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
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
