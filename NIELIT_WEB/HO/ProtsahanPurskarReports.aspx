<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ProtsahanPurskarReports.aspx.cs" Inherits="ProtsahanPurskarReports" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download Protsahan Puruskar Reports"></asp:Label>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">   
  
        <table class="sample3" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr >
                <td style="width: 20%;" valign="top" colspan="3">
                        <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblheading2" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puruskar Reports" Visible="true"></asp:Label><br />
                
                 <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
                

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
                        </td>              
            </tr>
            <tr class="gdalternate1">
                <td style="width: 20%;" valign="top" >
                    <asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Name"></asp:Label>

                </td>
                <td style="width: 20%;" valign="top" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExam" Width="100%" runat="server" Enabled="false" >   
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                </td>
            </tr>
        </table>
     <div id="divReportData" runat="server" style="overflow:scroll;width: 1000px;height:1000px;" visible="false">
    </div>
   
    <div id="divRefundFileDownload" runat="server" style="text-align: right; margin-top: 10px;
        margin-bottom: 6px;">       
         
        <asp:Button ID="btnDownload3" runat="server" Text="DBT Bank Report"
            OnClick="btnDownload3_Click" /> &nbsp;&nbsp;&nbsp;
         <asp:Button ID="btnDownload1" runat="server" Text="Master List Report"
            OnClick="btnDownload1_Click" />

        &nbsp;&nbsp;&nbsp;
         <asp:Button ID="btnDownload2" runat="server" Text="Recommended Report"
            OnClick="btnDownload2_Click" />
    </div>
    
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
