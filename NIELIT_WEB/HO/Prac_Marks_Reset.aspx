<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="Prac_Marks_Reset.aspx.cs" Inherits="HO_Prac_Marks_Reset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
   Practical Marks Reset
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <script type="text/javascript" language="javascript">
        function ValidateForm() {

            if (!isSelected("<%= ddlMarksType.ClientID %>", "Marks Type"))
                return false;          
            if (!isBlank("<%= txtRegNo.ClientID %>", "Registration Number"))
                return false;
            if (!isBlank("<%= txtBatchNo.ClientID %>", "Batch Number"))
                return false;
            if (!IsValidMinMaxLenght("txtBatchNo", 3, 3, "Invalid Pin Code"))
                return false;
            if (!isBlank("<%= txtRemarks.ClientID %>", "Remarks"))
                return false;
            

            return true;

        }

       
    </script>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
        Width="99%"></asp:Label>
                             </ContentTemplate>
                    </asp:UpdatePanel>
    <div class="box" id= "DivSearch" runat="server">
        <table class="sample3" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr class="gdrow1">
                <td >
                    <asp:Label ID="lblMarksType" runat="server" SkinID="CaptionLabel" Text="Marks Type&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td colspan ="5">
                    <asp:DropDownList ID="ddlMarksType" runat="server" Height="22px" Width="220px"  AutoPostBack ="true"  SkinID="ddl250">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        <asp:ListItem Value="1">Examination Marks</asp:ListItem>
                        <asp:ListItem Value="2">Observation Marks</asp:ListItem>
                        <asp:ListItem Value="3">Viva Marks</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr class="gdrow1">
               
               
                <td>
                    <asp:Label ID="lblRegNo" runat="server" Text="Registration  Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRegNo" runat="server" Width="150px" MaxLength="11" onkeypress="checkNumber(this,11,0,event);"
                        TabIndex="1"></asp:TextBox>
                </td>
               
            </tr>
            <tr class="gdrow1">
               
               
                <td>
                    <asp:Label ID="lblbatchNo" runat="server" Text="Batch  Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBatchNo" runat="server" Width="150px" MaxLength="3" onkeypress="checkNumber(this,3,0,event);"
                        TabIndex="1"></asp:TextBox>
                </td>
                
            </tr>
             <tr class="gdrow1">
               
               
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Remaks &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                   
                               <asp:TextBox ID="txtRemarks" runat="server" Width="520px" TabIndex="34" MaxLength="30" TextMode="MultiLine"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                </td>
                
            </tr>
            <tr class="gdrow1">
                <td></td>
                <td valign="top">
                    <%--<asp:ImageButton ID="ImgBtnReset" runat="server" ImageUrl="~/images/reset_btn.jpg"
                       Visible="True" TabIndex="3" />--%>
                     <asp:button id="btnCancel" runat="server" text="Reset" OnClientClick="ValidateForm()" OnClick="btnCancel_Click"  />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

