<%@ Page Title="MONTHLY PROGRESS REPORT" Language="C#" AutoEventWireup="true" CodeFile="MPRreport.aspx.cs" Inherits="HO_Rpt_MPRreport"
    MasterPageFile="~/MasterPages/main.master" Debug="True" %>

<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table-container {
            overflow-x: auto; /* Enables horizontal scroll if table exceeds container width */ /* Optional: Prevents vertical scroll if not needed */
            width: 800px; /* Full width of parent (adapts to page) */
            max-width: 100%; /* Ensures it doesn't exceed page width */
            height: auto; /* Remove fixed height; let it grow naturally */
            /* padding-top: 20px; */ /* Uncomment if needed */
        }

            .table-container th {
                font-family: Arial, sans-serif;
                font-size: 12px;
                font-weight: bold;
                background: linear-gradient(to bottom, #E0F0FC, #6b849a);
            }

            .table-container tr {
                font-family: Arial, sans-serif;
                font-size: 12px;
            }

            .table-container td, .table-container th {
                white-space: nowrap;
                padding: 5px;
            }

        .error {
            background-color: #EACFCE;
            padding: 4px;
            color: Red;
            border: 1px solid maroon;
            font-size: 11pt;
        }

        .gdheader {
            background: #1e1cdc;
            font: normal 12px arial;
            color: #ffffff;
            height: 20px;
            text-align: center;
            line-height: 18px;
        }

        .head1 {
            font: bold 13px arial;
            background-color: black;
            color: #ffffff;
            line-height: 20px;
            padding-left: 2px;
        }

        .gdalternate1 {
            font: normal 12px arial;
            background-color: #E6F0F0;
            color: #000000;
            line-height: 18px;
        }

        .gdrow1 {
            background-color: #c9d7e2;
            font: normal 12px arial;
            color: #000000;
            line-height: 18px;
        }

        input[type="date"] {
            padding: 4px;
            font-family: Arial;
            width: 126px;
        }

        /*for pdf generation*/
        .pdf-header {
            display: flex;
            align-items: center;
            width: 100%;
        }

            .pdf-header img {
                width: 80px;
                height: 80px;
                margin-right: 10px;
            }

        .pdf-header-text {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 10pt;
            font-weight: bold;
            color: #000000;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isSelected("<%=ddlproject.ClientID %>", "Project"))
                return false

            <%--if (!isSelected("<%=ddlinstitute.ClientID %>", "Institute"))
                return false;--%>

            if (!isSelected("<%=txtStartDate.ClientID %>", "Start Date"))
                return false;

            if (!isSelected("<%=txtEndDate.ClientID %>", "End Date"))
                return false;

            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="server">
    MIS Monthly Progress Report
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="server">
</asp:Content>


<%--<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>

</asp:Content>--%>


<%--<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>--%>


<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="server">

    <table border="0" width="100%" cellpadding="2" cellspacing="0">
        <tr>

            <%-- Drop down for selecting project --%>
            <td colspan="2" align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                            Width="98%"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>

                <asp:Label ID="lblproject" runat="server" Text=" Project&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>

            </td>

            <td>
                <asp:DropDownList ID="ddlproject" runat="server" Width="300px" AutoPostBack="true">
                    <asp:ListItem Value="0">-- ALL --</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td>

                <asp:Label ID="Label2" runat="server" Text=" Gender"></asp:Label>

            </td>

            <td>
                <asp:DropDownList ID="ddlgender" runat="server" Width="300px" AutoPostBack="true">
                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>

                <asp:Label ID="Label3" runat="server" Text=" Category"></asp:Label>

            </td>

            <td>
                <asp:DropDownList ID="ddlCategory" runat="server" Width="300px" AutoPostBack="true">
                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <tr class="gdalternate1">
            <td>
                <asp:Label ID="lblyear" runat="server" Text="Start Date&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
            </td>

            <td>
                <%--              <asp:DropDownList ID="ddlyear" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" >
                    <asp:ListItem Value="0">--Select Year--</asp:ListItem>
                </asp:DropDownList>--%>
                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
                <asp:TextBox ID="txtStartDate" runat="server" placeholder="DD-Mon-YYYY"></asp:TextBox>
                <img id="imgDob" runat="server" src="~/images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtStartDate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                </asp:CalendarExtender>

                <%--<img id="imgDob1" runat="server" src="../../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="cedate1" TargetControlID="txtdate1" PopupPosition="BottomLeft"
                    Format="yyyy-MMM-dd" PopupButtonID="imgDob1" runat="server">
                </asp:CalendarExtender>--%>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="lblmonth" runat="server" Text="End Date&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
            </td>

            <td>
                <%--               <asp:DropDownList ID="ddlmonth" runat="server" Width="300px" AutoPostBack="true">
      <asp:ListItem Value="0">--Select Month--</asp:ListItem>
  </asp:DropDownList>--%>
                <asp:TextBox ID="txtEndDate" runat="server" placeholder="DD-Mon-YYYY"></asp:TextBox>
                <img id="imgDob2" runat="server" src="~/images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtEndDate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgDob2" runat="server">
                </asp:CalendarExtender>
                <%--    <img id="imgDob2" runat="server" src="../../images/calendaricon.jpg" alt="Calandar" style="width: 20px; height: 22px; vertical-align: top;" />

                <asp:CalendarExtender ID="cedate2" TargetControlID="txtdate2" PopupPosition="BottomLeft"
                    Format="yyyy-MMM-dd" PopupButtonID="imgDob2" runat="server">
                </asp:CalendarExtender>--%>
            </td>
        </tr>


        <%--<asp:Label ID="Label2" runat="server" Text=""></asp:Label>--%>
        <%--PDF generation block--%>

        <tr>
            <td colspan="2" align="center" style="padding-top: 10px;">
                <asp:Button ID="btnShowData" runat="server" Text="Show Data" OnClientClick="return ValidateFormFields()" OnClick="btnShowData_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Data" OnClick="btnClear_Click" />
                <asp:Button ID="btnDownload" Visible="false" runat="server" Text="Download Excel" OnClick="btnDownload_Click" />
                <%--<asp:Button ID="btnDownloadpdf" Visible="false" runat="server" Text="Download Pdf" OnClick="btnDownloadpdf_Click" />--%>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <div id="divpdf" class="text-center" runat="server" visible="false">
                </div>
            </td>
        </tr>
    </table>

    <div class="table-container">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                <%--  Width="100%" --%>
                <asp:GridView ID="gvMain" runat="server"
                    CssClass="my-grid"
                    Width="100%"
                    Height="100%"
                    AutoGenerateColumns="True"
                    Visible="False"
                    OnRowDataBound="gvMain_RowDataBound"
                    EnableTheming="false">

                  <Columns>

        <asp:TemplateField HeaderText="Sr. No.">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" 
                    Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle Width="5%" />
            <ItemStyle Width="3%" HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
                    <PagerSettings Visible="true" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" Visible="false" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


</asp:Content>

