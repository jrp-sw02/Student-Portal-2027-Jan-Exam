<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="showAffidavit.aspx.cs" Inherits="HO_showAffidavit" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Afidavit Verification Process
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
        Width="99%"></asp:Label>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlCandidate" runat="server" Visible="False">
               
                <table border="0" cellpadding="3" cellspacing="0" class="box" 
                    style="text-align: left" width="100%" runat="server" id="tabphoto">
                    <tr >
                        <td align="center" width="30%">
                           <%-- <asp:Image ID="ImgCandidateAffidavit" runat="server" Height="135px" 
                               />--%>
                            
                                <div id="pdfViewer" style="height: 100%; width: 100%;" runat ="server"></div>
                           
                        </td>

                        
                    </tr>
                    <tr>
                        <td align="center" width="25%">
                            <asp:Button ID="BtnBack" runat="server" onclick="BtnBack_Click" Text="Back" 
                                Visible="False" TabIndex="4" />
                           
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
