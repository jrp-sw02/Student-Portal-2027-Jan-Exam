<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="Upload_excel_to-database.aspx.cs" Inherits="Upload_excel_to_database" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1
        {
            width: 218px;
        }
        .auto-style2
        {
            width: 218px;
            height: 69px;
        }
        .auto-style3
        {
            height: 69px;
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
  
        <table>   
           <tr> 
               <td>
                   <br />
               </td>
           </tr>
            <tr> 
                <td class="auto-style1">
                    
                       <asp:Label ID="Label1" runat="server" Text="Select File" Font-Bold="True"></asp:Label>
                       
                    </td>
                <td>
         <asp:FileUpload ID="FileUpload1" runat="server" style="margin-left: 0px" OnClick="callme()" AutoPostBack="true" />
<%--                     <asp:FileUpload ID="FileUpload1"  onchange="this.form.submit()"   runat="server"/>--%>

<%--                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Invalid file type" ValidationExpression="/(.xlsx|.xls)$/"></asp:RegularExpressionValidator>--%>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Select a valid file."></asp:RequiredFieldValidator>
                    <asp:Label ID="Label5" runat="server"></asp:Label>

                    </td>
                </tr>
           
            <tr>
                
                <td class="auto-style2">

            <asp:Label ID="Label2" runat="server" 
                Text="Enter Sheet Name:" Font-Bold="True"></asp:Label>

                    </td>
                <td class="auto-style3">
            <asp:TextBox ID="txtSheetname" runat="server" style="margin-left: 0px" 
                Width="205px" Height="22px"  AutoPostBack="false"></asp:TextBox>

                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtSheetname"  ErrorMessage="Sheet name cannot be empty."></asp:RequiredFieldValidator>
                    </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Upload File" OnClick="Button1_Click"  />
                    </td>
                <td>
                    <asp:Label ID="Label4" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        
     

  
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

