<%@ Page Title="Protsahan Puraskar Students Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="rptExamCandidateForPuraskar.aspx.cs" Inherits="rptExamCandidateForPuraskar" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
        function validateform() {
            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
           

            if (!isBlank("txtYears", "Years required"))
                return false;
            if (!isNumber("txtYears"))
                return false;

            return true;
        }
    </script>
    <style type="text/css">
.custom {
	font-family: Courier;
	color: red;
	font-size:20px;
}
</style>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  visible="false"/>&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to excel file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" visible="false" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="30%" width="15%" />
  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl250" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged" Enabled="true" EnableTheming="True">
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourse" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                    <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamCycle" runat="server" SkinID="ddl250" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                                    <asp:ListItem>--Select One--</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
        <tr>
           
            <td align="left" valign="top" >
                                <asp:Label ID="Label1" runat="server" Text="Session / Examination">
                                </asp:Label>
                            </td>
           
          <td align="left" valign="top" >
                                <asp:Label ID="Label2" runat="server" Text="Exam Verification Status">
                                </asp:Label>
                            </td>
           
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
            <asp:DropDownList ID="ddlSessionExam" runat="server" SkinID="ddl250" Width="185px" TabIndex="27"  AutoPostBack ="true"
                                  Style="display: inline;" OnSelectedIndexChanged ="ddlSessionExam_SelectedIndexChanged">
                                            <asp:ListItem Value="0">--Select Exam Name--</asp:ListItem>
                                        </asp:DropDownList>
                                
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                </td>

            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
            <asp:DropDownList ID="ddlVerificationStatus" runat="server" SkinID="ddl250" Width="185px" TabIndex="27"  
                                  Style="display: inline;" >
                                          
                                <asp:ListItem Value="0" Selected="True" >---All---</asp:ListItem>
                                <asp:ListItem Value="1">Verified</asp:ListItem>
                                <asp:ListItem Value="2" >Not Verified</asp:ListItem>
                               <asp:ListItem Value="3" >Pending</asp:ListItem>
                      
           </asp:DropDownList>
                                
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                </td>
           
        </tr>
                    </table>
     <div style="text-align: center; margin-top: 10px">
          <asp:Button ID="btnProcess" runat="server" Text="Process" OnClientClick="return Validate();"
                    OnClick="btnProcess_Click" /> &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnGenerate" runat="server" Text="Show Report" 
                    OnClick="btnGenerate_Click" />
        
        
            </div>        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    Protsahan Puraskar Students Report (Exam)</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
                                 </ContentTemplate></asp:UpdatePanel>
                           <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblheading" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar Students Report (Exam)" Visible="false"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label5" Visible="false"
                    runat="server"></asp:Label><br />
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="RegnNo" OnSorting="gvMain_Sorting"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="700px" ShowHeader="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                    <RowStyle Height="50px" />
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="SL">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>                      
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ExamCycle" HeaderText="Exam Cycle" SortExpression="ExamCycle" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                          <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="RegnNo" HeaderText="Regn No" SortExpression="RegnNo" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                           <asp:HyperLinkField HeaderStyle-Width="25%"
                            DataTextField="RegnDate" HeaderText="Regn Date" SortExpression="RegnDate"
                            Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                      
                        <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="Name" HeaderText="Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="20%"
                            DataTextField="ParentGuardianName" HeaderText="Parent/Guardian Name" SortExpression="ParentGuardianName" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="DoB" HeaderText="DOB" SortExpression="DOB"
                            Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="Gender" HeaderText="Gender" SortExpression="Gender" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="10%"
                            DataTextField="Caste" HeaderText="Caste" SortExpression="Caste" Target="_self">
                            <HeaderStyle Width="10%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="isHandicapped" HeaderText="Physically Handicapped" SortExpression="isHandicapped" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>                       
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ExamAppeared" HeaderText="ExamAppeared" SortExpression="ExamAppeared" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField> 
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ExamPassed" HeaderText="ExamPassed" SortExpression="ExamPassed" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="VerificationStatus" HeaderText="Verifcation Status" SortExpression="VerificationStatus" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="rejectionReason" HeaderText="RejectionReason (if rejected)" SortExpression="rejectionReason" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="ApplicationStatus" HeaderText="Application Status" SortExpression="ApplicationStatus" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>                      
                           <asp:TemplateField Visible="false" HeaderText="lblIdVisFalse">
                            <ItemTemplate>
                                <%-- <asp:Label runat="server" Visible="true" ID="lblExamid" Text='<%# Eval("Examid") %>'></asp:Label>--%>
                                <asp:Label runat="server" Visible="true" ID="lblRegno" Text='<%# Eval("RegnNo") %>'></asp:Label>
                                <asp:Label runat="server" Visible="true" ID="lblcandidateid" Text='<%# Eval("candidateid") %>'></asp:Label>
                               
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:ButtonField  Text="View" CommandName="Select" ItemStyle-Width="30" />
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
