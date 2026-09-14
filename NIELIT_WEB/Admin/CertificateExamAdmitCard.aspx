<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CertificateExamAdmitCard.aspx.cs" Inherits="Admin_CertificateExamAdmitCard" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master"  Async="true" AutoEventWireup="true"
    CodeFile="CertificateExamAdmitCard.aspx.cs" Inherits="Admin_CertificateExamAdmitCard" Debug="true"
    Culture="auto" UICulture="auto" %>

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

    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="New" runat="server">
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
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Upload MS-Access File (.ACCDB)" meta:resourcekey="Label4Resource1"></asp:Label>
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
                        <asp:FileUpload ID="fuInstitituteList" Width="485px" runat="server" />
                    </td>
                    <tr>
                        <td colspan="3" style="text-align: right">
                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload File" />
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                        </td>
                    </tr>
                    <tr class="even">
                        <td colspan="3" valign="top">
                            <asp:Label ID="lblCount" runat="server" meta:resourcekey="Label4Resource1" SkinID="CaptionLabel" Text=""></asp:Label>
                        </td>
                    </tr>
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
                    OnClick="SendSMSBtn_Click" />

                        <asp:Button ID="SendEmailBtn" runat="server" Text="Send Email"
                    OnClick="SendEmailBtn_Click" />
 </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:Content>
