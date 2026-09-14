<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="NewApplicationStatusInstitute.aspx.cs" Inherits="Admin_NewApplicationStatusInstitute" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="New Application Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div id="divReportData" runat="server" style="width: 100%;">
        <div class="summary_block1" id="divDLC" runat="server">
            <span>Digital Literacy Courses > New Applications > Institute </span>
            <br />
            <asp:Label ID="LblRptSubHeader" runat="server" Font-Size="10px"></asp:Label>
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:GridView ID="DlcNewApplicationGrid" runat="server" DataKeyNames="InstituteId" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="DlcNewApplicationGrid_RowDataBound" HeaderStyle-Font-Size="12px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="InstituteName" HeaderText="Institute Name">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>                           
                            <asp:BoundField DataField="FinalSubmitted" HeaderText="Submitted">
                                <HeaderStyle Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:BoundField DataField="InstituteNotVerified" HeaderText="Not-Verified">
                                <HeaderStyle Width="6%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" ForeColor="Maroon" />
                            </asp:BoundField>
                            <asp:BoundField DataField="InstituteVerified" HeaderText="Verified">
                                <HeaderStyle Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:BoundField DataField="InstitutePaid" HeaderText="Paid">
                                <HeaderStyle Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Disability" HeaderText="Disability">
                                <HeaderStyle Width="4%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
