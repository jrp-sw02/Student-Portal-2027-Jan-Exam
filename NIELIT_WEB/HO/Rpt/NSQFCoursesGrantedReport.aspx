<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="NSQFCoursesGrantedReport.aspx.cs" Inherits="HO_Rpt_NSQFCoursesGrantedtoInstitute" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">

    NSQF Free Courses Granted 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">

     <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  visible="false"/>&nbsp;


 <%--   <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to excel file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" visible="false" /> &nbsp;--%>


    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="false" height="30%" width="15%" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">

    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
        
         <tr>
            <td>
                <asp:Label ID="lblLevel" runat="server"  Text="Level"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAccrNO" runat="server"  Text="Accrediation Number"></asp:Label>
            </td>            
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlCourse" runat="server" Height="22px" SkinID="ddl250"
                              Enabled="true" Width="170px"  >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtAccrNo" runat="server" Width="229px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>

            </td>
            <td>
                <asp:Button  ID ="btnShowDetails" runat ="server"  Text =" Show Details " Width="185px" OnClick="btnShowDetails_Click"/>
            </td>
             
        </tr>

        <tr>
            <td colspan ="2">
                <asp:GridView ID="grdInstituteDetails" runat="server" AutoGenerateColumns="false" CellPadding="6">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                       <ItemTemplate>
                       <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <HeaderStyle CssClass="table_04" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle CssClass="table_02" HorizontalAlign="Left"></ItemStyle>
        </asp:TemplateField>
                         <asp:BoundField DataField="InstituteName" HeaderText="Institute Name" />  
                         <asp:BoundField DataField="InstituteAddress" HeaderText="Institute Address" />                           
                         <asp:BoundField DataField="AccrediationValidity" HeaderText="Validity" />                           
                         <asp:BoundField DataField="InstituteStatus" HeaderText="Status" />  
                    </Columns>
                </asp:GridView>
            </td>
          
        </tr>
        <tr>
            <td colspan ="2">

            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnViewReport" runat="server" Text="View Report" Width="136px" Visible=" false" OnClick="btnViewReport_Click" />
            </td>
        </tr>

    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div id="divReportData" runat="server" style="width: 100%;">

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divGrid" runat="server" visible="true">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                 <br />
                <asp:Label ID="lblheading" runat="server" ForeColor="blue" Font-Bold="true"
                    Text=" NSQF Free Courses Granted Report" Visible="false"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label5" Visible="false"
                    runat="server"></asp:Label><br />
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server"  AutoGenerateColumns="False" Width="700px" ShowHeader="true" >
                    <RowStyle Height="50px" />
                    <Columns>
                         <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                       <ItemTemplate>
                       <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <HeaderStyle CssClass="table_04" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle CssClass="table_02" HorizontalAlign="Left"></ItemStyle>
        </asp:TemplateField>                     
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ID" HeaderText="Course Name" >
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="grantDate" HeaderText="Grant Date" >
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="isActive" HeaderText="Active" >
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                          <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="eFileNo" HeaderText="eFile No" >
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                        
                           <asp:HyperLinkField HeaderStyle-Width="25%"
                            DataTextField="approvalDate" HeaderText="Approval Date" >
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                      
                       <%-- <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="CourseName" HeaderText="Course Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField> --%>                      
                    </Columns>
                    <SelectedRowStyle BackColor="#87CEFA" ForeColor="Maroon" Font-Size="10" />
                    <PagerSettings Visible="False" />
                </asp:GridView>

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />


            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

