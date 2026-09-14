<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Dashboard/Dashboard.master"
    CodeFile="RepositoryMaster.aspx.cs" Inherits="Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 651px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    Repository of NIELIT Services
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
    <%--<asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">


  <marquee behavior="scroll" loop="-1" width="40%"> 
  <span class=sub-heading> <asp:Label  ID="LblRptSubHeader" runat="server" Text = "NIELIT Online Services">
  </asp:Label>
    </span></marquee>
  &nbsp;
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <div id="div1" runat="server" style="width: 100%;">
        <table style="border: 1px solid black; width: 100%; font-family: Arial;">
            <tr>
                <td style="color:Navy;">
                    <b>Chart</b>
                    <asp:DropDownList ID="ddlChartList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlChartList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="color:Navy;">
                    <b>&nbsp;3D</b>
                    <asp:CheckBox ID="ThreeDChkBox" AutoPostBack="true" runat="server" Checked = "true" OnCheckedChanged="ThreeDChkBox_CheckedChanged" />
                </td>
                <td style="color:Navy;">
                    <b>&nbsp;Sort By</b>
                    <asp:DropDownList ID="ddlSortBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged">
                    <asp:ListItem Text = "Subject Name" Value = "Subject_Type"></asp:ListItem>
                    <asp:ListItem Text = "Subject Status Count" Value = "Subject_Status_Count" Selected = "True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="color:Navy;">
                    <b>&nbsp;Sort Direction</b>
                    <asp:DropDownList ID="ddlSortDirection" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlSortDirection_SelectedIndexChanged" 
                        Width="102px">
                    <asp:ListItem Text = "Ascending" Value = "ASC" Selected ="True"></asp:ListItem>
                    <asp:ListItem Text = "Descending" Value = "DESC"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="border: 1px solid black; font-family: Arial;" width="700px">
            <tr>
                <td class="style1">
                        <asp:Chart ID="Chart1" runat="server" Width = "780px" Height = "670px" BackColor="Lavender"
                        EnableViewState="True" ImageType="png" Palette="SemiTransparent"
                        Style="margin-top: 0px">
                        <Titles>
                        <asp:Title Font="Times New Roman, 12pt, style=Bold, Italic" Text = "NIELIT Online Services"></asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series Name="Services" ChartArea="ChartArea1" ChartType="Bar" IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="True" Name="Default"
                                LegendStyle= "Table" />
                        </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisX Title="Online Services" Interval="1">
                                </AxisX>
                                <AxisY Title="Count of Online Services">
                                </AxisY>
                                <Area3DStyle Enable3D="true" />
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
