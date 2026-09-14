<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="FrmOnline.aspx.cs" Inherits="FrmOnline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style4
        {
            width: 305px;
        }
        .style5
        {
            width: 305px;
            height: 27px;
        }
        .style6
        {
            height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<center>
    <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
    
   <td class="style5">
   </td>
   <td class="style6">
   </td>
    </tr>
        <tr>
            <td class="style4" >
               <h4 align="right"> Select Bank</h4>
            </td>
            <td align="left">
                <asp:DropDownList ID="DropDownList1" runat="server" Height="25px" Width="222px">
                    <asp:ListItem Value="0">--select one--</asp:ListItem>
                    <asp:ListItem>ICICI Bank</asp:ListItem>
                    <asp:ListItem>SBI Bank</asp:ListItem>
                    <asp:ListItem>Axis Bank</asp:ListItem>
                    <asp:ListItem>HDFC Bank</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style4" >
              </td>
            <td>
               </td>
        </tr>
        <tr>
            <td class="style4" >
               </td>
            <td>
               </td>
        </tr>
        <tr>
            <td class="style4" >
            </td>
            <td align="left" >
                <asp:Button ID="BtnOk" runat="server" Text="OK" />
                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" />
            </td>
        </tr>
        <tr>
            <td class="style4" >
               </td>
            <td >
                </td>
        </tr>
    </table>
    </center>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

