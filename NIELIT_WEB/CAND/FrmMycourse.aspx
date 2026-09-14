<%@ Page Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmMycourse.aspx.cs" Inherits="FrmMycourse" Debug="false" %>

<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="My Courses"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
        <ItemTemplate>
            <div class="Course_block">
                <div>
                    <%#DataBinder.Eval(Container, "DataItem.Cname")%>
                    <asp:Label ID="Lbcid" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.cid")%>'
                        Visible="false"></asp:Label>
                    <asp:Label ID="lbRegNo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.Regno")%>'
                        Visible="false"></asp:Label>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td width="40%">
                            Registration No.
                        </td>
                        <td width="60%">
                            :<%# DataBinder.Eval(Container, "DataItem.Regno")%></td>
                    </tr>
                    <tr>
                        <td>
                            Registration Status.
                        </td>
                        <td>
                            :<%# DataBinder.Eval(Container, "DataItem.RegStatus")%></td>
                    </tr>
                    <tr>
                        <td>
                            Registration Date.
                        </td>
                        <td>
                            :<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.RegDate")).ToString("dd-MMM-yyyy")%></td>
                    </tr>
                    <tr>
                        <td>
                            Registration Commencement Date.
                        </td>
                        <td>
                            :<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.RegComDate")).ToString("dd-MMM-yyyy")%></td>
                    </tr>
                    <tr>
                        <td>
                            Valid Upto Date.
                        </td>
                        <td>
                            :<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.Validity")).ToString("dd-MMM-yyyy")%></td>
                    </tr>
                    <tr>
                        <td>
                            Candidate Type.
                        </td>
                        <td>
                            :<%# DataBinder.Eval(Container, "DataItem.candtype")%></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <asp:LinkButton ID="btnViewDetail2" runat="server" Text="View Detail"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
        <SeparatorTemplate>
        </SeparatorTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="SideLink2" runat="server" Visible="false" />
    <div>
        <uc2:SideLink ID="SideLink1" runat="server" />
    </div>
</asp:Content>
