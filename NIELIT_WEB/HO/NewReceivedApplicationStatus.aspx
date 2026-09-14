<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="NewReceivedApplicationStatus.aspx.cs" Inherits="HO_NewReceivedApplicationStatus" %>

<%@ Register src="../UserControl/PagingBar.ascx" tagname="PagingBar" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
 <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        
                        <asp:GridView ID="gvMain" runat="server" OnSorting="gvMain_Sorting" OnRowDataBound="gvMain_RowDataBound"
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="#" />                               
                                  <asp:TemplateField  HeaderText="Application No." SortExpression="ApplicationNumber">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl="#" ID="HyperLink1" runat="server"><%#Eval("ApplicationNumber")%></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateField>   
                                 <asp:TemplateField  HeaderText="Application Date" SortExpression="ApplicationDate">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl="#" ID="HyperLink2" runat="server"><%# Convert.ToDateTime(Eval("ApplicationDate")).ToString("dd-MMM-yyyy")%></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateField>   
                                <asp:TemplateField HeaderText="Candidate Name" SortExpression="CandidateName">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl="#" ID="HyperLink3" runat="server"><%#Eval("CandidateName")%></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateField>                              
                                <asp:TemplateField HeaderText="Father Name" SortExpression="FatherName">
                                <ItemTemplate>
                                 <asp:HyperLink NavigateUrl="#" ID="HyperLink4" runat="server"><%#Eval("FatherName")%></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mother Name" SortExpression="MotherName">
                                <ItemTemplate>
                                 <asp:HyperLink NavigateUrl="#" ID="HyperLink5" runat="server"><%#Eval("MotherName")%></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Date Of Birth" SortExpression="DOB">
                                <ItemTemplate>
                                 <asp:HyperLink NavigateUrl="#" ID="HyperLink6" runat="server"><%# Convert.ToDateTime(Eval("DOB")).ToString("dd-MMM-yyyy")%></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc1:PagingBar ID="PagingBar1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

