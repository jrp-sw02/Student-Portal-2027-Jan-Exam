<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="DownloadCertificateData.aspx.cs" Inherits="Admin_DownloadCertificateData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download Certificate Data"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <asp:UpdatePanel EnableViewState="true" ID="upBread" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {

            if (document.getElementById("<%=ddlRc.ClientID %>").disabled == false) {
                if (!isSelected("<%=ddlRc.ClientID %>", "Regional Centre"))
                    return false;
            }
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Application Type"))
                return false;
            if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                return false;
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
            if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                return false;
        }
        function OpenWindow() {
        }
    </script>
    <asp:Label ID="Lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Regional Centre &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:DropDownList ID="ddlRc" runat="server" Height="22px" SkinID="ddl250">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCourseCategry" runat="server" AutoPostBack="True" Height="22px"
                    OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>--%>
                <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" Height="22px"
                    OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
                <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Exam Cycle &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>--%>
                <asp:DropDownList ID="ddlAppType" runat="server" AutoPostBack="True" Height="22px"
                    OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>--%>
                <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="True" Height="22px"
                    OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged" SkinID="ddl250">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>--%>
                <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%-- </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td>
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="even">
            <td>
                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlExamName_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:DropDownList>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamYear" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div id="divBatchDetails" runat="server" visible="false">
        <asp:Label runat="server" ID="lblApplicationsDetails" CssClass="error" Text="" Visible="False"
            Width="100%" EnableTheming="False"></asp:Label>
    </div>
    <div style="text-align: right; margin-top: 10px" runat="server" id="divDownload">

        <asp:Button ID="btnDownload" runat="server" Text="Download Applications" OnClick="btnDownload_Click"
            OnClientClick="return validatefilter();" ClientIDMode="Static" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
