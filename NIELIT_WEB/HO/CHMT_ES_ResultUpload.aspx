<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="CHMT_ES_ResultUpload.aspx.cs" Inherits="HO_CHMT_ES_ResultUpload" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    CHM -T O Level ES Result Upload
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

 <style>
    .gdheader {
    background: #c9d7e2;
    font:normal 12px arial;
	    color: #000000;
    height: 20px;
    text-align: center;
    line-height: 18px;
}
 </style>

    <div>
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Width="100%" style="margin-bottom: 0px"  ></asp:Label>
        <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
            width="100%">
            
            <tr>
               <td colspan="1" class="auto-style2">
                    <asp:RadioButton ID="rbtnESMarks" runat="server" Text="ES Marks" GroupName="MarksType"  AutoPostBack="true" OnCheckedChanged="rbtnESMarks_CheckedChanged"  />

                </td>
                <td>
                    <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                    <asp:HiddenField ID="flpath" runat="server" />
                </td>
            </tr>

           
             <tr>
               <td colspan="1" class="auto-style2">
                   

                </td>
                <td>
                      <asp:Button ID="btnUpload" runat="server" Text="Upload"  Width="210px" Height="26px" OnClick="btnUpload_Click" />
                </td>
            </tr>
            <%-- <tr>
               <td colspan="1" class="auto-style2">
                   

                </td>
                <td>
                      <asp:Button ID="btnTransfer" runat="server" Text="Validate Data"  Width="210px" Height="26px" OnClick="btnTransfer_Click" />
                </td>
            </tr>--%>
            <tr>
               <td colspan="1" class="auto-style2">
                   

                </td>
                <td>
                      <asp:Button ID="btnFTransfer" runat="server" Text="Validate and Finalize Result"  Width="210px" Height="26px" OnClick="btnFTransfer_Click" />
                </td>
            </tr>
            </table>
        </div>
    <%--AlternatingRowStyle-BackColor="#C9D7E2"--%> 
     <div id="divGrid">
        <asp:GridView ID="grdMismatch" runat="server" Visible="false" AutoGenerateColumns="false" CssClass="gdalternate"  
                        Width="100%" Caption="<b><center><span style='font-size: 24px;'>Uploaded Marks with  Error Log</centre></b>" OnRowDataBound="grdMismatch_RowDataBound"  style="margin: 0 auto;" PageSize="400" >
            <AlternatingRowStyle CssClass="gdrow" />  <HeaderStyle CssClass="gdheader"  /> 
            <Columns>
                <asp:BoundField DataField="SrNo" HeaderText="SrNo." SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="registration_no" HeaderText="Registration No" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                 <asp:BoundField DataField="module_name" HeaderText="Module Name" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="absent_flag" HeaderText="Attendance" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="marks" HeaderText="Marks Obtained" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="exam_date" HeaderText="Exam Date" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="100px" />
                <asp:BoundField DataField="Error_Message" HeaderText="Error" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
               <%-- <asp:BoundField DataField="moduleCode" HeaderText="Module Code" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="levelCode" HeaderText="Level Code" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />--%>
            </Columns>
        </asp:GridView>
         <asp:GridView ID="grdError" runat="server" Visible="false" AutoGenerateColumns="false"  CssClass="gdalternate"  
                        Width="100%" Caption="<b><center><span style='font-size: 24px;'>Validation and Finalization Error Log</centre></b>" OnRowDataBound="grdError_RowDataBound"  Style="margin: 0 auto;" AllowPaging="false" >
             <AlternatingRowStyle CssClass="gdrow" /> <HeaderStyle CssClass="gdheader"  /> 
             <Columns>
                 <asp:BoundField DataField="SrNo" HeaderText="SrNo." SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="30px"  />
                 <asp:BoundField DataField="reg" HeaderText="Registration No" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="70px"  />
                 <asp:BoundField DataField="Error" HeaderText="Result" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width ="300px" />
             </Columns>
             <PagerSettings Visible="False" />
         </asp:GridView>
         <div id="divNavigation" runat="server">
                 <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                     <ContentTemplate>
                         <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                     </ContentTemplate>
                     </asp:UpdatePanel>
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

