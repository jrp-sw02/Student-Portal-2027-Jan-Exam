<%@ Page Title="UP Districtwise Schoolwise Registered Candidate Report" Language="C#"
    AutoEventWireup="true" CodeFile="UPdistrictwiseschoolwisecandidate.aspx.cs"
    Inherits="HO_Rpt_UPdistrictwiseschoolwisecandidate"
    MasterPageFile="~/MasterPages/MyInfo.master"%>

<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
    </style>
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (!isSelected("<%=ddlexamcycle.ClientID %>", "Exam Cycle"))
                return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="server">
    UP Districtwise Schoolwise Registered Candidate Report
</asp:Content>


<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="server">
</asp:Content>--%>


<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="server">

    <table width="100%" border="0" cellpadding="2" cellspacing="0">
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

        <tr class="gdalternate1">
            <td>
                <div style="float: left">
                    <asp:Label ID="lblcourse" runat="server" Text="Course ID&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                </div>
            </td>
            <td>
                <asp:DropDownList ID="ddlcourse" runat="server" Width="300px"
                    AutoPostBack="true">
                    <asp:ListItem Value="0">O Level</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        

        <tr class="gdrow1">
            <td>
                <div style="float: left">
                    <asp:Label ID="lblexamcycle" runat="server" Text="Exam Cycle&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                </div>
            </td>
            <td>
                <asp:DropDownList ID="ddlexamcycle" runat="server" Width="300px"
                    AutoPostBack="true">
                    <asp:ListItem Value="0">--Select Exam Cycle--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

                <tr class="gdalternate1">
            <td>
                <asp:Label ID="lblinstitute" runat="server" Text="Select Institute&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlinstitute" runat="server" Width="300px" AutoPostBack="true">
                    <asp:ListItem Value="0">--All Schools--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>


        <tr class="gdrow1">
            <td>
                <asp:Label ID="lblDatefrom" runat="server" Text="As on Date"></asp:Label>
            </td>

            <td>
                <asp:Label ID="txttodaydate" runat="server" Text=""></asp:Label>
            </td>
        </tr>


        <tr>
            <td colspan="2" align="center" style="padding-top: 10px;">
                <asp:Button ID="btnShowData" runat="server" Text="Show Data" OnClientClick="return ValidateFormFields()" OnClick="btnShowData_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Data" OnClick="btnClear_Click" />
                <asp:Button ID="btnDownload" Visible="false" runat="server" Text="Download Excel" OnClick="btnDownload_Click" />
                <asp:Button ID="btnDownloadpdf" Visible="false" runat="server" Text="Download Pdf" OnClick="btnDownloadpdf_Click" />
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
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                <asp:GridView ID="gvMain" runat="server"
                    Width="100%"
                    CssClass="my-grid"
                    Height="100%"
                    AutoGenerateColumns="False"
                    Visible="False"
                    >

                    <Columns>

                        <asp:TemplateField HeaderText="Sr. No.">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="District" HeaderText="District" />
                        <asp:BoundField DataField="SchoolName" HeaderText="School Name" NullDisplayText="&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;" />
                        <asp:BoundField DataField="TotalRegistrations" HeaderText="Total Registrations" NullDisplayText="&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;" />
                     

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
