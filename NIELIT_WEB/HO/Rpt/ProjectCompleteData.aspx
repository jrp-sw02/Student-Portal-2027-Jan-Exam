<%@ Page Title="Project Based Student Report" Language="C#"
    AutoEventWireup="true" CodeFile="ProjectCompleteData.aspx.cs"
    Inherits="HO_Rpt_ProjectCompleteData"
    MasterPageFile="~/MasterPages/MyInfo.master"
    Debug="true" %>

<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.table-container {
            overflow-x: auto;
            width: 800px;
            height: 100%;*/
        /*padding-top: 20px;*/
        /*}

        .table-container th {
            font-family: Arial, sans-serif;
            font-size: 12px;
            font-weight: bold;
            background: linear-gradient(to bottom, #E0F0FC, #6b849a);*/
        /* For older browsers */
        /*background: linear-gradient(to bottom, #E0F0FC, #6b849a);
            background: linear-gradient(to bottom, #E0F0FC, #6b849a);
            background: linear-gradient(to bottom, #E0F0FC, #6b849a);
        }

        .table-container tr {
            font-family: Arial, sans-serif;
            font-size: 12px;
        }

        .table-container table {
            width: 100% !important;
            overflow-x: auto;
        }*/


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

            /*.table-container table {
    width: auto;*/ /* Let table width be determined by content (enables natural scrolling) */
            /*min-width: 100%;*/ /* Ensures it at least fills the container */
            /*table-layout: auto;*/ /* Auto layout for natural column sizing; change to 'fixed' if setting explicit widths */
            /*border-collapse: collapse;*/ /* Optional: Cleaner borders */
            /*}*/

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
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isSelected("<%=ddlproject.ClientID %>", "Project"))
                return false;

        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="server">
    Project Complete Data Report
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

            <%--label error--%>
            <td colspan="2" align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                            Width="98%"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <%-- Drop down for selecting project --%>
        <tr class="gdrow1">
            <td>
                <div>
                    <asp:Label ID="lblproject" runat="server" Text="Select Project&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                </div>
            </td>

            <td>
                <asp:DropDownList ID="ddlproject" runat="server" Width="300px"
                    AutoPostBack="true">
                    <asp:ListItem Value="0">--Select Project--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>


        <%--PDF generation block--%>

        <tr>
            <td colspan="2" align="center" style="padding-top: 10px;">
                <asp:Button ID="btnShowData" runat="server" Text="Show Data" OnClientClick="return ValidateFormFields()" OnClick="btnShowData_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Data" OnClick="btnClear_Click" />
                <asp:Button ID="btnDownload" Visible="false" runat="server" Text="Download Excel" OnClick="btnDownload_Click" />
                <asp:Button ID="btnDownloadpdf" Visible="false" runat="server" Text="Download Pdf" OnClick="btnDownloadpdf_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="loadingtxt" runat ="server" ForeColor="Red">0</asp:Label>
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
                    AutoGenerateColumns="False"
                    Visible="False"
                    OnRowDataBound="gvMain_RowDataBound"
                    EnableTheming="false">

                    <Columns>

                        <asp:TemplateField HeaderText="Sr. No.">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server"
                                    Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="NIELIT Centre" HeaderText="NIELIT Centre" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Online Reference Number" HeaderText="Online Reference Number" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Registration Number" HeaderText="Registration Number" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Batch Name" HeaderText="Batch Name" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Candidate Name" HeaderText="Candidate Name" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Father Name" HeaderText="Father Name" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Mother Name" HeaderText="Mother Name" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Caste Category" HeaderText="Caste Category" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="EWS" HeaderText="EWS" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Handicapped" HeaderText="Handicapped" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Ex ServiceMan" HeaderText="Ex-Servicemen" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Date of Birth"
                            DataFormatString="{0:dd-MMM-yyyy}"
                            HtmlEncode="false"
                            HeaderText="Date of Birth" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Course" HeaderText="Course Enrolled" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Batch Start Date"
                            DataFormatString="{0:dd-MMM-yyyy}"
                            HtmlEncode="false"
                            HeaderText="Batch Start Date" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Batch End Date"
                            DataFormatString="{0:dd-MMM-yyyy}"
                            HtmlEncode="false"
                            HeaderText="Batch End Date" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="District" HeaderText="District" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Training Partner" HeaderText="Training Partner" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Candidate Mobile" HeaderText="Candidate Mobile" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Candidate Email" HeaderText="Candidate Email" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Correspondence Address" HeaderText="Correspondence Address" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="C_District" HeaderText="Correspondence District" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="C_State" HeaderText="Correspondence State" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="C_Pin Code" HeaderText="Correspondence Pin Code" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Permanent Address" HeaderText="Permanent Address" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="P_District" HeaderText="Permanent District" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="P_State" HeaderText="Permanent State" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="P_Pin Code" HeaderText="Permanent Pin Code" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Course Complete" HeaderText="Course Completion Status" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Certficate Issued" HeaderText="Certificate Issued" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />
                        <asp:BoundField DataField="Drop Out" HeaderText="Dropout Status" NullDisplayText="&nbsp;&nbsp;-&nbsp;&nbsp;" />

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
