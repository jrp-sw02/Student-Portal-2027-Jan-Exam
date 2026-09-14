<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="RegionalCentrePaymentReportFilter.aspx.cs" Inherits="Common_RegionalCentrePaymentReport" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
Regional Centre Payment Report 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
 <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
 <script type="text/javascript" language="javascript">

     function OpenWindow() {
         //Regional Centre ID
         var RegionalCentreID;
         RegionalCentreID = document.getElementById('<%=ddlRc.ClientID %>').value;

         //Course Category
         var CourseCategory;
         if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
             return false;
         else
             CourseCategory = document.getElementById('<%=ddlCourseCategry.ClientID %>').value;
        
         //Application Type
         var TypeId;
         if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
             return false;
         else
             TypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;

         //Exam Name
         var ExamId;
             if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                 return false;
             else
                 ExamId = document.getElementById('<%=ddlExamName.ClientID %>').value;

        //Report Type
        var ReportTypeId;
        if (!isSelected("<%=ddlreporttype.ClientID %>", "Report Type"))
            return false;
        else
            ReportTypeId = document.getElementById('<%=ddlreporttype.ClientID %>').value;


        window.open("../HO/Rpt/RegionalCentrePaymentReport.aspx?RegionalCentreID=" + RegionalCentreID + "&CourseCategory=" + CourseCategory + "&ExamId=" + ExamId + "&TypeId=" + TypeId + "&ReportTypeId=" + ReportTypeId);
         return false;
     }
 </script>
     <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:DropDownList ID="ddlRc" runat="server" Height="22px" SkinID="ddl250">
                    <asp:ListItem Value="0">--All--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCourseCategry" runat="server" AutoPostBack="True" Height="22px"
                    OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:label id="Label19" runat="server" skinid="CaptionLabel" text="Report Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" 
                            AutoPostBack="true">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td> 
            <td colspan="2">
                <asp:updatepanel id="UpdatePanel51" runat="server">
                    <contenttemplate>
                <asp:dropdownlist id="ddlreporttype" runat="server" skinid="ddl504" autopostback="true">
                    <asp:listitem value="1">Actual Exam (Excluding Forward Candidates)</asp:listitem>
                    <asp:listitem value="2">Paid For Exam(Including Forward Candidates)</asp:listitem>
                </asp:dropdownlist>
                 </contenttemplate>
                </asp:updatepanel>
            </td>
        </tr>
        </table> 
<div style="text-align: right; margin-top: 10px">
<asp:Button ID="btnView" runat="server" Text="Generate Report" 
    OnClientClick="return OpenWindow();"/>
<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
 <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"> </asp:Label>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

