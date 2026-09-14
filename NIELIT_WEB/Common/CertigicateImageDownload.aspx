<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="CertigicateImageDownload.aspx.cs" Inherits="Common_CertigicateImageDownload" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    O/A/B/C Certificate Image Download
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function Validate() {

            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Level"))
                return false;
            if (!isSelected("<%=ddlPhaseNumber.ClientID %>", "Phase Number"))
                return false;

            return true;
        }
    </script>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td style="width: 33%;">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;">
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Phase Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text=" Download"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 33%;">
                <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 33%;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlPhaseNumber" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlPhaseNumber_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="text-align: right; vertical-align: bottom;">
             <asp:Button ID="btnphoto" runat="server" Text="Download Images" OnClientClick="return Validate();"
                    OnClick="btnphoto_Click" Visible="true" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblTotalCand" runat="server" SkinID="CaptionLabel" Text="Total Number of Candidate :"></asp:Label>
                        <asp:Label ID="lblPhotoCand" runat="server" SkinID="CaptionLabel" Text="Available Photo : "></asp:Label>
                        <asp:Label ID="lblNoPhotoCand" runat="server" SkinID="CaptionLabel" Text="Missing Photo : "></asp:Label>
                        <asp:Label ID="lblMissingCand" runat="server" SkinID="CaptionLabel" Visible="false" Text="Missing Photo Registration Number : "></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPhaseNumber" EventName="SelectedIndexChanged" />                       
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td >
               
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
