<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="DataDownloadSeqGenerationNSQF.aspx.cs" Inherits="Admin_DataDownloadSeqGenerationNSQF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1
        {
            width: 334px;
        }
        .auto-style2
        {
            width: 334px;
            height: 25px;
        }
        .auto-style3
        {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">

    Data Download Sequence Generation NSQF

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
     <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Data Downloaded Sequence No &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>

               <%--  <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlSeqNo" runat="server" Height="20px" SkinID="ddl250" AutoPostBack="true" 
                              Enabled="true" Width="201px"   >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtSeqNo" runat="server" Width="195px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <asp:TextBox ID="txtSeqNo" runat="server" Enabled="false" Width="191px"></asp:TextBox>
              
            
        </tr>
      <%-- <tr class="even">
            <td class="auto-style2">
                 <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Data Downloaded Sequence Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            </td>
            <td class="auto-style3">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlDataDownloadedDate" runat="server" Height="22px" SkinID="ddl250"
                             AutoPostBack="True" Width="186px" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="TextBox1" runat="server" Width="195px"></asp:TextBox>
                    </ContentTemplate>                 
                </asp:UpdatePanel>
            </td>
            
        </tr>--%>
       <tr>
                            <td colspan="1">
                                Captcha Code
                            </td>
           <td>
                        <asp:TextBox ID="txtcode" runat="server" Width="195px"></asp:TextBox>
           </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <img src="" id="imgcap" runat="server" alt="Capture Code" width="155" height="35" />
                                        <asp:ImageButton ID="ImgBtnRefresh" ImageUrl="~/images/refresh.jpg" runat="server"
                                            CausesValidation="false" Width="25px" Height="35" Style="vertical-align: top;
                                            padding-top: 0px; border: none;"  />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
        <tr >
            <td colspan="1"></td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Generate & Freeze" Width="148px" OnClick="btnSubmit_Click" />
            </td>
        </tr>
     
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

