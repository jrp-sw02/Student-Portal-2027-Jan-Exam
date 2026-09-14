<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="CHMTResultPublish.aspx.cs" Inherits="HO_CHMTResultPublish" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    CHM-T O Level Publish Result
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <div>
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
        <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
            width="100%">
           
            <tr>
               <td class="auto-style1">
                   <label id="Label1" runat="server"> Exam Name</label>
               </td>
               <td>
                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   <asp:DropDownList ID="ddlExam" runat="server" Height="25px"
                            Enabled="true" Width="209px"  >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <%--<asp:ListItem Value="9293">CHM-T O Level Exam</asp:ListItem>--%>
                        </asp:DropDownList>
               </td>              
           </tr>

             <tr>
               <td class="auto-style1">&nbsp;</td>
               <td>
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button ID="btnFreeze" runat="server" Text="Publish Result"  Width="210px" Height="26px" OnClick="btnFreeze_Click" />
               </td>
           </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

