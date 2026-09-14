<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdmitCardUpload_DLC.aspx.cs" Inherits="Admin_CertificateExamAdmitCard" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master"  Async="true" AutoEventWireup="true"
    CodeFile="AdmitCardUpload_DLC.aspx.cs" Inherits="Admin_AdmitCardUpload_DLC" Debug="true" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/usercontrol/sidelink.ascx" TagPrefix="uc" TagName="SideLink" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Upload Admit Card Data" meta:resourcekey="lblHeadingResource1"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {

            return true;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }

        function ValidateFormFields() {


            if (!isSelected("<%=ddlcoursecategory.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Course"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
            
            if (!isBlank("<%=flUpload.ClientID %>", "Browse File Upload"))
                return false;
        }
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="New" runat="server">
            <div style="width:100%">
                <div style="width:33%;float:left">
                    <asp:DropDownList ID="ddlcoursecategory" runat="server" SkinID="ddl253" Width="100%" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                            <asp:ListItem>-- Select Course Category --</asp:ListItem>
                        </asp:DropDownList>
                </div>
               <div style="width:33%;float:left">
                   <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourse" runat="server" Width="100%" SkinID="ddl251" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                    <asp:ListItem> -- Select Course Name -- </asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
               </div>
                <div style="width:33%;float:left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamCycle" runat="server" Width="100%" SkinID="ddl252" AutoPostBack="True" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                                    <asp:ListItem>-- Select Exam Cycle --</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcourse" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                </div>
            </div><br/><hr/>
            <div style="width:100%">
               <div style="width:33%;float:left">
                  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem>-- Select Exam Year --</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
               </div>
                <div style="width:33%;float:left">
                  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true">
                                    <asp:ListItem>-- Select Exam Name --</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                </div>
            </div><br/>
            <table class="sample2" cellpadding="2" cellspacing="0">
                 
                <tr class="even">
                    <td colspan="3">
                        <table width="100%">
                            <tr>
                                <td>
                                    <a href="#" target="_blank">
                                        <img src="../images/Export_Exl.jpg" /></a>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Style="display: inline;"
                                        Text="Please upload the data in given format" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <%--<td valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Category"
                            meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Course Name" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>--%>
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.XLS/.XLSX)" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                  <%--  <td valign="top">
                        <asp:DropDownList ID="ddlCourseCategory" runat="server" SkinID="ddl250" meta:resourcekey="ddlActivityGroupNewResource1"
                            OnSelectedIndexChanged="ddlCourseCategory_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="--Select One--" Value="0" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top">                    
                        <asp:DropDownList ID="ddlCourse" runat="server" SkinID="ddl250" meta:resourcekey="ddlActivityGroupNewResource1">
                            <asp:ListItem Text="--Select One--" Value="0" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                    </tr>
                    <tr>
                  
                    <td>
                        <asp:FileUpload ID="flUpload" Width="485px" runat="server" />
                    </td>
                   </tr>
                    <tr>
                        <td colspan="3" style="text-align: right">
                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload File" OnClientClick="return ValidateFormFields()" />
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                        </td>
                    </tr>
                    <tr class="even">
                        <td colspan="3" valign="top">
                            <asp:Label ID="lblCount" runat="server" meta:resourcekey="Label4Resource1" SkinID="CaptionLabel" Text=""></asp:Label>
                        </td>
                    </tr>
               
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
    <uc:SideLink runat="server" ID="ucSideLink" />
</asp:Content>



<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">

    <table>
     <tr>

            <td>
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanelSMS" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="SMSLbl" Visible="true"
                    runat="server">SMS/Email to be sent : </asp:Label><br />
                 
                
                <asp:Button ID="SendSMSBtn" runat="server" Text="Send SMS"
                    OnClick="SendSMSBtn_Click"  OnClientClick="return ValidateFormFields()"/>

                        <asp:Button ID="SendEmailBtn" runat="server" Visible="true" Text="Send Email"
                    OnClick="SendEmailBtn_Click" OnClientClick="return ValidateFormFields()" />
                 </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:Content>
