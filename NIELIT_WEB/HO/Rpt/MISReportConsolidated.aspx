
<%@ Page Title="MIS Student Consolidated Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
     CodeFile="MISReportConsolidated.aspx.cs" Inherits="HO_Rpt_MISReportConsolidated" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function printwindow() {
        window.print();
        return false;
    }
</script>
    <style>
    #cpReportData table tr {
        height: 60px; /* Adjust spacing between rows */
    }
    #cpReportData table td {
        padding: 5px 10px; /* Adds spacing inside each cell */
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    MIS Project based Students Details
</asp:Content>
<asp:Content ID="Content3"  ContentPlaceHolderID="cpButtons" runat="server">
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
&nbsp;
</asp:Content>
<asp:Content ID="Content5"  ContentPlaceHolderID="cpReportDate" runat="server">
   Report Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <table style="height: 91px; width: 95%;border:1px solid #999;">
        <tr>
            <td style="width:350px; text-align:right; border:1px solid #999; padding:2px; background-color:#ded9d9; border-radius:2px;">
  
            <!-- Project Dropdown -->
            <asp:Label ID="lblProject" runat="server" Text="Select Project: "></asp:Label>
                    
            </td>
        <td>
            <asp:DropDownList ID="ddlProjects" runat="server" Width="500px"  AutoPostBack="true" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged">
            </asp:DropDownList>
            </td>
            </tr>
            <tr>
            <td style="width:300px; text-align:right; border:1px solid #999; padding:2px; background-color:#ded9d9; border-radius:2px;">
            <!-- Centre CheckBoxList -->
            <asp:Label ID="lblCentres" runat="server" Text="Select Centres: "></asp:Label>
                </td>
                <td>
            <asp:CheckBoxList ID="chkCentres" runat="server" RepeatColumns="3"></asp:CheckBoxList>
           <asp:Button ID="btnToggleSelect" runat="server" 
            Text="Select All" 
            OnClick="btnToggleSelect_Click" />
                    </td>
                </tr>
        <tr><td>
            <asp:Label ID="lblNoCentres" runat="server" ForeColor="Red" Font-Italic="true" Visible="false"></asp:Label>
            </td></tr>
         <tr>
            <td style="width:300px; text-align:right; border:1px solid #999; padding:2px; background-color:#ded9d9; border-radius:2px;">
            <!-- Date Range -->
            <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
                </td><td>
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>
                    </td></tr>
         <tr><td style="width:300px; text-align:right; border:1px solid #999; padding:2px; background-color:#ded9d9; border-radius:2px;">
            <asp:Label ID="lblEndDate" runat="server" Text=" End Date: "></asp:Label>
             </td><td>
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
   </td></tr>
        <tr>
            <td colspan="2" style="text-align:center;">
            <!-- Buttons -->
           <%-- <asp:Button ID="btnShowdata" runat="server" Text="Show Data" OnClick="btnShowData_Click" />--%>
                <%-- <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />--%>
           
            <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" OnClick="btnExportExcel_Click"  />
            <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" OnClick="btnExportPDF_Click"  />
                 <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                </td>
            </tr>
        </table>
            <br />
             <br />

            <!-- GridView -->
            <asp:Panel ID="pnlGrid" runat="server"  Height="400px" Width="100%">
            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" 
                          BorderColor="Black" BorderWidth="1px">
            <Columns>
    <asp:BoundField DataField="Centre" HeaderText="Centre" />
    <asp:BoundField DataField="Course" HeaderText="Course" />
    <asp:BoundField DataField="Batch" HeaderText="Batch" />
    <asp:BoundField DataField="Registration No" HeaderText="Registration No" />
    <asp:BoundField DataField="Online Reference No" HeaderText="Online Reference No" />
    <asp:BoundField DataField="Final submitted" HeaderText="Final Submitted" />
    <asp:BoundField DataField="Full Name" HeaderText="Full Name" />
    <asp:BoundField DataField="Father Name" HeaderText="Father Name" />
    <asp:BoundField DataField="Mother Name" HeaderText="Mother Name" />
    <asp:BoundField DataField="Guardian Name" HeaderText="Guardian Name" />
    <asp:BoundField DataField="Gender" HeaderText="Gender" />
   <%-- <asp:BoundField DataField="Marital_Status" HeaderText="Marital Status" />--%>
    <asp:BoundField DataField="Date Of Birth" HeaderText="Date of Birth" DataFormatString="{0:dd-MM-yyyy}" />
    <asp:BoundField DataField="Cast Category" HeaderText="Caste Category" />
    <%--<asp:BoundField DataField="Religion" HeaderText="Religion" />--%>
    <asp:BoundField DataField="Handicapped" HeaderText="Handicapped" />
    <%--<asp:BoundField DataField="ExServicemane" HeaderText="Ex-Serviceman" />--%>
    <%--<asp:BoundField DataField="Mark" HeaderText="Body Mark" />--%>
    <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
    <%-- <asp:BoundField DataField="Std" HeaderText="STD" />--%>
    <asp:BoundField DataField="Phone" HeaderText="Phone" />
    <asp:BoundField DataField="Email" HeaderText="Email" />

    <asp:BoundField DataField="Correspondence Address" HeaderText="Correspondence Address" />
    <%-- <asp:BoundField DataField="Permanent_Address" HeaderText="Permanent Address" />--%>

    <%--  <asp:BoundField DataField="Institute Verified" HeaderText="Verified By Institute" />--%>
    <%-- <asp:BoundField DataField="Aadhar Verified" HeaderText="Aadhar Verified" />--%>
    <asp:BoundField DataField="Project Student" HeaderText="Project Student" />
    <asp:BoundField DataField="Project ID" HeaderText="Project ID" />
    <asp:BoundField DataField="Course Completed" HeaderText="Course Completed" />
    <asp:BoundField DataField="Certificate Status" HeaderText="Certificate Status" />
    <%--  <asp:BoundField DataField="PlacementStatus" HeaderText="Placement Status" />--%>

    <%--<asp:BoundField DataField="companyNameID" HeaderText="Company Name ID" />
    <asp:BoundField DataField="Company_FullAddress" HeaderText="Company Address" />--%>

    <%--<asp:BoundField DataField="EWS" HeaderText="EWS" />
    <asp:BoundField DataField="Semester" HeaderText="Semester ID" />--%>
    <%--<asp:BoundField DataField="Lateral Entry" HeaderText="Lateral Entry" />--%>
    <%-- <asp:BoundField DataField="Caste Certificate" HeaderText="Caste Certificate" />
    <asp:BoundField DataField="Dropout" HeaderText="Dropout" />--%>
</Columns>
                <EmptyDataTemplate>
        <div style="text-align:center; color:red; font-style:italic;">
            No data available.
        </div>
    </EmptyDataTemplate>
            </asp:GridView>
                </asp:Panel>
            <br /></asp:Content>
