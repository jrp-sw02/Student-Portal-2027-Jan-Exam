<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DownloadPurskarCandListForAmountReleasedToBank.aspx.cs" Inherits="DownloadPurskarCandListForAmountReleasedToBank" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download Purskar Candidates List to be sent to Bank for Amount Release"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

        }
        var dtgp = "<%= gvMain.ClientID %>"
         function CheckAll(Sender, CheckBoxName) {
             CheckUncheckAll(dtgp, Sender, CheckBoxName)
         }
       
    </script>   
  
        <table class="sample3" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr >
                <td style="width: 20%;" valign="top" colspan="3">
                        <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblheading2" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar Candidates List to be sent to Bank for Amount Release" Visible="true"></asp:Label><br />
                
                 <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="Regno" OnSorting="gvMain_Sorting"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="600px" ShowHeader="true" PageSize="100" >
                    <RowStyle Height="40px" />
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="SL" DataField="sl" SortExpression="sl">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>    
                        <asp:BoundField HeaderStyle-Width="16%"
                             DataField ="onlinerefno" HeaderText="Ref Number" SortExpression="onlinerefno">
                            <HeaderStyle Width="16%" />
                        </asp:BoundField>                     
                          <asp:BoundField HeaderStyle-Width="16%"
                            DataField="Regno" HeaderText="Registration Number" SortExpression="Regno" >
                            <HeaderStyle Width="16%" />
                        </asp:BoundField>                       
                        <asp:BoundField HeaderStyle-Width="16%"
                            DataField="Name" HeaderText="Candidates Name" SortExpression="Name" >
                            <HeaderStyle Width="16%" />
                        </asp:BoundField>                       
                       <asp:BoundField HeaderText="Level" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" DataField="Level">
                         
                        </asp:BoundField>

                        <asp:BoundField HeaderStyle-Width="15%" 
                            DataField="Exams" HeaderText="Exam" SortExpression="Exams" >
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="15%"
                            DataField="FatherName" HeaderText="FatherName" SortExpression="FatherName" >
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                          <asp:TemplateField HeaderText="Aadhaar Number" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10px">
                            <ItemTemplate>
                                <asp:Label ID="lblaadhaar" runat="server" Width="80px"></asp:Label>
                                <asp:Label ID="lblad" runat="server" Visible ="false" Text ='<%# Bind("AadharNumber") %>'></asp:Label>

</ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="15%"
                            DataField="BankName" HeaderText="Bank Name" SortExpression="BankName">
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                         <asp:BoundField HeaderStyle-Width="15%"
                            DataField="AccountNumber" HeaderText="Account Number" SortExpression="AccountNumber" >
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                         <asp:BoundField HeaderStyle-Width="15%"
                            DataField="AmountToBeReleased" HeaderText="AmountToBeReleased" SortExpression="AmountToBeReleased" >
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                         <%--<asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="onlinerefno" HeaderText="Ref Number" SortExpression="onlinerefno" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                     
                          <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="Regno" HeaderText="Registration Number" SortExpression="Regno" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                       
                        <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="Name" HeaderText="Candidates Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                       
                       <asp:TemplateField HeaderText="Level" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                            <ItemTemplate>
                                <asp:Label ID="lblcode" runat="server" Text='<%# Bind("Level") %>' Width="20px"></asp:Label>

</ItemTemplate>
                        </asp:TemplateField>

                        <asp:HyperLinkField HeaderStyle-Width="15%" 
                            DataTextField="Exams" HeaderText="Exam" SortExpression="Exams" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="FatherName" HeaderText="FatherName" SortExpression="FatherName" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                          <asp:TemplateField HeaderText="Aadhaar Number" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10px">
                            <ItemTemplate>
                                <asp:Label ID="lblaadhaar" runat="server" Width="80px"></asp:Label>
                                <asp:Label ID="lblad" runat="server" Visible ="false" Text ='<%# Bind("AadharNumber") %>'></asp:Label>

</ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="BankName" HeaderText="Bank Name" SortExpression="BankName" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="AccountNumber" HeaderText="Account Number" SortExpression="AccountNumber" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="AmountToBeReleased" HeaderText="AmountToBeReleased" SortExpression="AmountToBeReleased" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>--%>
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

                        </td>              
            </tr>
        </table>
    <%--</div>--%>
   
    <div id="divRefundFileDownload" runat="server" style="text-align: right; margin-top: 10px;
        margin-bottom: 6px;">
        <asp:Button ID="btnDownload" runat="server" Text="Download File" Visible="false"
            OnClick="btnDownload_Click" />   
          <asp:Button ID="btnDownloadWithLockCell" runat="server" Text="Download Excel File " Visible="true"
            OnClick="btnDownloadWithLockCell_Click" />
        <asp:Button ID="btnPdfDownload" runat="server" Text="Download PDF File" Visible="true"
            OnClick="btnPdfDownload_Click" /> 
        <asp:Button ID="btnDownload3" runat="server" Text="Download Letter"
            OnClick="btnDownload3_Click" />
    </div>
     <div id="divReportData" runat="server" style="overflow:scroll;width: 1000px;height:1000px;" visible="false">
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
