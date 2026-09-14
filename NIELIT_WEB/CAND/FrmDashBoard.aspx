<%@ Page Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmdashBoard.aspx.cs" Inherits="FrmdashBoard" Debug="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="My Dashboard"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <div class="summary_block" id="divpersonal" runat="server" visible="false">
        <span>
            Personal Detail Notification
        </span>
        <ul>
            <li>Please Update Your Personal Detail by Clicking on the Update Detail link.</li>
        </ul>
        <div>
        <asp:LinkButton  ID="Lnkpersonal" runat="server" >Update Detail</asp:LinkButton>
        </div>
    </div>
    <div class="summary_block" id="divcontact" runat="server" visible="false">
        <span>Contact Detail Notification </span>
        <ul>
            <li>Please Update Your Contact Detail by Clicking on the Update Detail link.</li>
        </ul>
        <div>
            <asp:LinkButton ID="Lnlcontact" runat="server" onclick="Lnlcontact_Click">Update Detail</asp:LinkButton>
        </div>
    </div>
    <div class="summary_block" id="div1" runat="server" visible="false">
        <span>Correspondence Address Detail Notification </span>
        <ul>
            <li>Please Update Your Correspondence Address Detail by Clicking on the Update Detail link.</li>
        </ul>
        <div>
            <asp:LinkButton ID="Lnladdress" runat="server">Update Detail</asp:LinkButton>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
