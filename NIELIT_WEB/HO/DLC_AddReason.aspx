<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="DLC_AddReason.aspx.cs" Inherits="Admin_DLC_AddReason" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Add new Reason"></asp:Label>
&nbsp;(Only for DLC Courses)
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <div>
         <strong>
         <asp:Label ID="Label2" runat="server" Text="New Application Status Entry (Rejection)"></asp:Label>
&nbsp;
      </strong>
      <br />
         <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%">
             <tr class =" gdalternate1" runat="server"  id ="trStatusName">
                <td  width="15%">
                    <asp:Label ID="lblStatusName" runat ="server" Text ="Status Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtStatusName" runat="server" Width="95%"  Enabled ="false"></asp:TextBox>                

                </td>
            </tr>

            <tr class ="gdrow1" runat ="server" id="trStatusDescription">  
                <td width="15%">
                    <asp:Label ID="lblStatusDescription" runat="server" Text =" Status Description "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID ="txtStatusDescription" runat ="server" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  />
                </td>
            </tr>

        </table>
    </div>
    <br />
    <div>
        <asp:Label ID ="lblReason" Text ="Note : Application Status to reject a candidate already available in the system. Please enter a status which in not present in the below table (if required)." runat ="server" style="font-weight: 700"></asp:Label>
        </br>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
                <asp:GridView ID="grdReason" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                            <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="Description" HeaderText="Status Description" SortExpression="BrowserName" ItemStyle-HorizontalAlign="Left"/>

                    </Columns>

                </asp:GridView>
               
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

