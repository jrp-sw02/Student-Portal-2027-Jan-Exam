<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResultSheet_Download.aspx.cs" Inherits="CAND_ResultSheet_Download"  MasterPageFile="~/MasterPages/MyInfo.master"  Debug="false"%>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Download Result Sheet" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
<table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Level &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Year &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                     <td style="width: 25%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Month &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                       <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddlLevel" runat="server" SkinID="ddl250" AutoPostBack="True" onselectedindexchanged="ddlLevel_SelectedIndexChanged"
                           >
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                               </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlyear" runat="server" SkinID="ddl250" 
                                    AutoPostBack="True" onselectedindexchanged="ddlyear_SelectedIndexChanged"  >
                                    <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
                      <td style="width: 33%;" valign="top">
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddlmonth" runat="server" SkinID="ddl250" 
                              AutoPostBack="False" >
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                          </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
                    
                </tr>
                <tr>
                   <td colspan=3 align="center"> <asp:Button ID="Button1" runat="server" 
                           Text="Download" onclick="Button1_Click" /></td></tr>
                           <tr><td colspan="3"></td></tr>
                           <tr><td colspan="3">
                          
                     <%--          <asp:Label ID="LblResultSheetMessage" runat="server"   ForeColor="Red" ></asp:Label>--%>
                                <asp:Label Width="99%" EnableTheming="False"  ID="LblResultSheetMessage" Visible="False"
                            runat="server" ForeColor="Red"></asp:Label>
                       
                           </td></tr>
                
             
            </table>
            </asp:Content>


