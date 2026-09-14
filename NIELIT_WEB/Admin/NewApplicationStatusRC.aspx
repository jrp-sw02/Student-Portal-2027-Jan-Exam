<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="NewApplicationStatusRC.aspx.cs" Inherits="Admin_NewApplicationStatusRC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
 <asp:Label ID="lblHeading" runat="server" Text="New Application Status"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div id="divReportData" runat="server" style="width: 100%;">
        <div class="summary_block1" id="divDLC" runat="server">
            <span>Digital Literacy Courses > New Applications > RC </span>
            <br />
             <asp:Label ID="LblRptSubHeader" runat="server" Font-Size ="10px"></asp:Label>
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>                   
                    <asp:GridView ID="DlcNewApplicationGrid" runat="server" DataKeyNames="RcId" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="DlcNewApplicationGrid_RowDataBound" HeaderStyle-Font-Size="12px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>      
                            <asp:BoundField DataField="RcName" HeaderText="RC Name">
                                <HeaderStyle Width="5%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId,RcId" DataNavigateUrlFormatString="NewApplicationStatusInstitute.aspx?CourseId={0}&ExamId={1}&RcId={2}"
                                DataTextField="FinalSubmitted" HeaderText="Submitted" Target="_self">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId,RcId" DataNavigateUrlFormatString="NewApplicationStatusInstitute.aspx?CourseId={0}&ExamId={1}&RcId={2}"
                                DataTextField="Direct" HeaderText="Direct" Target="_self">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId,RcId" DataNavigateUrlFormatString="NewApplicationStatusInstitute.aspx?CourseId={0}&ExamId={1}&RcId={2}"
                                DataTextField="ViaInstitute" HeaderText="Institute: Not-Verified | Verified | Paid"
                                Target="_self">
                                <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId,RcId" DataNavigateUrlFormatString="NewApplicationStatusInstitute.aspx?CourseId={0}&ExamId={1}&RcId={2}"
                                DataTextField="FeePaid" HeaderText="All Paid" Target="_self">
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId,RcId" DataNavigateUrlFormatString="NewApplicationDisability.aspx?CourseId={0}&ExamId={1}&RcId={2}"
                                DataTextField="Disability" HeaderText="Disability" Target="_self">
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

