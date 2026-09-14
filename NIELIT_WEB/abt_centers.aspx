<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/FullInfo.master" AutoEventWireup="true"
    CodeFile="abt_centers.aspx.cs" Inherits="abt_centers" %>

<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>

<%--<%@ Register Src="~/UserControl/SideLink.ascx" TagName="sidelink" TagPrefix="uc3" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Regional Centres"></asp:Label>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">

            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                OnRowDataBound="gvMain_RowDataBound"  Width="100%"
                AllowSorting="True">
                <Columns>
                    <asp:BoundField HeaderText="#">
                        <HeaderStyle Width="2%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Organisation" DataField="organization">
                     <HeaderStyle Width="15%" />
                     </asp:BoundField>
                    <asp:BoundField  HeaderText="Address" DataField="address">
                      <HeaderStyle Width="30%"/>
                     </asp:BoundField>
                    <asp:HyperLinkField Target="_blank" DataTextField="website" HeaderText="Website"
                        DataNavigateUrlFields="website" DataNavigateUrlFormatString="{0}">
                        <ItemStyle Width="15%" Wrap="true"/>
                     </asp:HyperLinkField>
                    <asp:BoundField HeaderText="Contact No" DataField="contactno" >
                     <HeaderStyle Width="10%" />
                     </asp:BoundField>
                    <asp:BoundField HeaderText="Email" DataField="email">
                    <HeaderStyle Width="15%" />
                     </asp:BoundField>
					   <asp:TemplateField HeaderText ="Email"                    >
                       <ItemTemplate>
                           <asp:Label ID="chEmail" runat="server"></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Visible="False" />
            </asp:GridView>

</asp:Content>
<%--<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>--%>
<%--<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc3:sidelink ID="Sidelink" runat="server" />
</asp:Content>--%>
