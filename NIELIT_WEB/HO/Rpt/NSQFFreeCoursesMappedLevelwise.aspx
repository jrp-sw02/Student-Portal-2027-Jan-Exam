<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="NSQFFreeCoursesMappedLevelwise.aspx.cs" Inherits="HO_Rpt_NSQFFreeCoursesMappedLevelwise" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    NSQF Free Courses Mapped Level-Wise  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        Visible="false" height="30%" width="15%" OnClick="imgPDF_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
     <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
      <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
          <tr>
            <td colspan ="2">
                <asp:Label ID="lblLevel" runat="server"  Text="Level"></asp:Label>
            </td>
                      
        </tr>
          <tr>
              <td colspan="1">
                   <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlCourse" runat="server" Height="22px" SkinID="ddl250"
                              Enabled="true" Width="170px"  >
                            <asp:ListItem Value="0">-- All --</asp:ListItem>
                          <%--<asp:ListItem Value="-1">-- ALL --</asp:ListItem>--%>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
              </td>
              <td colspan="1">
                   <asp:Button  ID ="btnShowDetails" runat ="server"  Text =" Show Details " Width="185px" OnClick="btnShowDetails_Click" />
              </td>
          </tr>
          <tr>
              <td colspan ="2">

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
                    Text=" NSQF Free Courses Mapped Level-Wise" Visible="false"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label5" Visible="false"
                    runat="server"></asp:Label><br />
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server"  AutoGenerateColumns="False" Width="1016px" ShowHeader="true" >
                    <RowStyle Height="50px" />
                    <Columns>
                         <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left">
                       <ItemTemplate>
                       <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <HeaderStyle CssClass="table_04" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle CssClass="table_02" HorizontalAlign="Left"></ItemStyle>
        </asp:TemplateField>      
                        <asp:HyperLinkField HeaderStyle-Width="8%"
                            DataTextField="courseName" HeaderText="Course Name" >
                            <HeaderStyle Width="8%" />
                        </asp:HyperLinkField>               
                        <asp:HyperLinkField HeaderStyle-Width="18%"
                            DataTextField="mappedCourse" HeaderText="Mapped Course Name" >
                            <HeaderStyle Width="18%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="11%"
                            DataTextField="effectiveFrom" HeaderText="Effective From Date" >
                            <HeaderStyle Width="11%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="effectiveTo" HeaderText="Effective To Date" >
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                          <asp:HyperLinkField HeaderStyle-Width="13%"
                            DataTextField="mappingApprovalDate" HeaderText="Mapping Approval Date" >
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>

                         <asp:HyperLinkField HeaderStyle-Width="13%"
                            DataTextField="eFileNo" HeaderText="eFile No." >
                            <HeaderStyle Width="13%" />
                        </asp:HyperLinkField>
                        
                           <asp:HyperLinkField HeaderStyle-Width="11%"
                            DataTextField="isReplacement" HeaderText="Whether Replaced" >
                            <HeaderStyle Width="11%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="18%"
                            DataTextField="replacedCourse" HeaderText="Replaced Course Name" >
                            <HeaderStyle Width="18%" />
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
                <uc3:PagingBar ID="PagingBar1" runat="server"  OnPageIndexChanged="PageIndexChanged" Visible="true" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

