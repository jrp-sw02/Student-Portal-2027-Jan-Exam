<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="repCourseModules.aspx.cs" Inherits="HO_Rpt_repCourseModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
        .error
{
    background-color: #EACFCE; 
    padding-top:4px; 
    padding-bottom:4px; 
    padding-left:4px; 
    color: Red;
    border: 1px solid maroon; 
    font-size: 11pt; 
   
}
       .head1
{
	font:bold 13px arial;
	background-color :  #31597C;
	color :#ffffff;
	line-height: 20px; 
	padding-left:2px;
	
}
/*.sample3 Tr.head1 td
{
	padding-left:3px;
}*/
.gdalternate1
{
	font:normal 12px arial;
	background-color : #E6F0F0;
	color :#000000;
	line-height: 18px; 
}
/*/*.sample3 Tr.gdalternate1 td
{
	padding-left:3px;
}*/
.gdrow1
{
	background-color:#c9d7e2;
	font:normal 12px arial;
	color: #000000;
	line-height: 18px; 
}
/*.sample3 Tr.gdrow1 td
{
	padding-left:3px;
}*/
       .auto-style1
       {
           height: 20px;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
     Course Modules Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" Height="22px"  />
    &nbsp; <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
   
         </script>


    <asp:Label id ="lblError" runat ="server" CssClass="error" ForeColor="#FF3300"  Width="100%" Visible="false"> </asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="100%">
         <tr>
 <td style="width: 40%;" valign="top">
         <asp:Label ID="lblFiler3" Width="100%" runat="server" Text="Course Category Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>    

 </td>
            <td width="40%">
                <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr>
             <td style="width: 40%;" valign="top" >
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategoryName" Width="100%" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCourseCategoryName_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td width="40%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourses" Width="100%" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategoryName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
    <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" OnClick="btnView_Click" />
<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
     <div id ="divReportData" runat ="server" style ="width : 100%"></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

