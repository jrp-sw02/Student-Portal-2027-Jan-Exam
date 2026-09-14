<%@ Page Title="DataforNCVET" Language="C#" AutoEventWireup="true" CodeFile="DataforNCVET.aspx.cs" Inherits="HO_Rpt_DataforNCVET"
    MasterPageFile="~/MasterPages/main.master" %>

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

        function showLoader() {
            var loader = document.getElementById("loadingMessage");

            if (loader) {
                loader.style.display = "inline-flex";
            }

            return true;
        }


        function ValidateFormFields() {

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
 Candidate / Training Marks Data
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="server">

    <table border="0" width="100%" cellpadding="2" cellspacing="0">
        <tr>

            <td colspan="2" align="center">
                <span class="lblWarningNew"
                   >Confidentiality Notice:This report contains official candidate data and may include sensitive information. The data shall be used, handled, stored, and shared strictly in accordance with applicable Government rules, regulations, policies, and guidelines. Unauthorized use or disclosure is strictly prohibited.
                </span>
            </td>

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

                <asp:Label ID="Label3" runat="server" Text=" Choose Type of Data"></asp:Label>

            </td>

            <td>
                <asp:DropDownList ID="ddlTypeofData" runat="server" Width="300px" AutoPostBack="true">
                    <asp:ListItem Value="0">NSQF Marks Data</asp:ListItem>
                    <asp:ListItem Value="1">CCC Candidate Wise Detail</asp:ListItem>
                    <asp:ListItem Value="2">NSQF Candidate Wise Detail</asp:ListItem>
                    <asp:ListItem Value="3">OABC Candidate Wise Detail</asp:ListItem>
                    <asp:ListItem Value="4">OABC Marks Data</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        <tr class="gdalternate1">
            <td>
                <asp:Label ID="lblyear" runat="server" Text="Start Date&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
            </td>

            <td>

                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
                <asp:TextBox ID="txtStartDate" runat="server" placeholder="DD-Mon-YYYY"></asp:TextBox>
                <img id="imgDob" alt="" runat="server" src="~/images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtStartDate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="lblmonth" runat="server" Text="End Date&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
            </td>

            <td>

                <asp:TextBox ID="txtEndDate" runat="server" placeholder="DD-Mon-YYYY"></asp:TextBox>
                <img alt="" id="imgDob2" runat="server" src="~/images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtEndDate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgDob2" runat="server">
                </asp:CalendarExtender>
               
            </td>
        </tr>

        <%--PDF generation block--%>

        <%--if (!ValidateFormFields()) return false; return showLoader();--%>

        <tr>
            <td colspan="2" align="center" style="padding-top: 10px;">
                <asp:Button ID="btnShowData" runat="server" Text="Show Data" OnClientClick="return ValidateFormFields()" OnClick="btnShowData_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Data" OnClick="btnClear_Click" />

                <asp:Button ID="btnDownload" Visible="false" runat="server" Text="Download Excel" OnClick="btnDownload_Click" />

                   <div id="loadingMessage" style="display: none; margin-left: 8px; vertical-align: middle; white-space: nowrap;"">
                       <span class="smallSpinner"></span>
                       <strong style="font-family: Arial, sans-serif; font-size: 13px">Please Wait...</strong>
                   </div>

              
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

