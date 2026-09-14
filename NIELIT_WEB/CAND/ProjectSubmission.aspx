<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectSubmission.aspx.cs" Inherits="CAND_ProjectSubmission"  MasterPageFile="~/MasterPages/main.master"%>


<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="My Current Course Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div class="summary_block">
        <span id="lblCourseNameSt" runat="server">Course Status: </span>
        <table align="center" width="100%" cellpadding="2" cellspacing="1">
            <tr>
                <td width="40%">
                    Registration No.
                </td>
                <td width="60%" id="lblRegNumber" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Registration Status.
                </td>
                <td id="lblRegStatus" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Registration Date.
                </td>
                <td id="lblRegDate" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Registration Commencement Date.
                </td>
                <td id="lblRegCommencementDate" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Valid Upto Date.
                </td>
                <td id="lblRegValidUptoDate" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Candidate Type.
                </td>
                <td id="lblCandidateType" runat="server">
                </td>
            </tr>
        </table>
    </div>
    <div id="div_O" class="summary_block" runat="server">
        <span id="summeryHeading" runat="server">Modules Summary</span>
        <table align="center" width="100%" cellpadding="2" cellspacing="1">
            <tr>
                <td width="40%">
                    <b>Module Status</b>
                </td>
                <td align="center" width="30%" id="td1">
                    <b>Theory(Comp. + Elect. + Bridge)</b>
                </td>
                <td align="center" width="15%" id="td2">
                    <b>Practical</b>
                </td>
                <td align="center" width="15%" id="td3">
                    <b>Project</b>
                </td>
            </tr>
            <tr>
                <td>
                    Total number of modules to be passed
                </td>
                <td align="center" id="tdTotalTheoryModules" runat="server">
                </td>
                <%--<td align="center" id="tdTotalElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdTotalPracticalModules" runat="server">
                </td>
                <td align="center" id="tdTotalProjectModules" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Total number of modules attempted till date
                </td>
                <td align="center" id="tdAttemptedTheoryModules" runat="server">
                </td>
                <%--<td align="center" id="tdAttemptedElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdAttemptedPracticalModules" runat="server">
                </td>
                <td align="center" id="tdAttemptedProjectModules" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Total number of modules passed till date
                </td>
                <td align="center" id="tdPassedTheoryModules" runat="server">
                </td>
                <%--<td align="center" id="tdPassedElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdPassedPracticalModules" runat="server">
                </td>
                <td align="center" id="tdPassedProjectModules" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    Total number of modules remaining to pass
                </td>
                <td align="center" id="tdRemainingTheorygModules" runat="server">
                </td>
                <%--<td align="center" id="tdRemainingElectiveModules" runat="server">
                </td>--%>
                <td align="center" id="tdRemainingPracticalModules" runat="server">
                </td>
                <td align="center" id="tdRemainingProjectModules" runat="server">
                </td>
            </tr>
        </table>
    </div>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblMessage" Visible="false"
        runat="server"></asp:Label>
    <table align="center" width="100%" cellpadding="2" cellspacing="1">
        <tr>
            <td width="40%">
                Select Level/Course
            </td>
            <td align="center" width="2%" id="td4">
                :
            </td>
            <td align="center" width="58%" id="td6">
                <asp:DropDownList Width="100%" ID="ddlCourse" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="SideLink2" runat="server" Visible="false" />
    <div>
        <uc2:SideLink ID="SideLink1" runat="server" />
    </div>
</asp:Content>
