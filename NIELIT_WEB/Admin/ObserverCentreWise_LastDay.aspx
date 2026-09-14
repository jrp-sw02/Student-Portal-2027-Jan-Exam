<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="ObserverCentreWise_LastDay.aspx.cs" Inherits="Admin_ObserverCentreWise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
    <asp:Label  runat ="server"> <strong>
        Marks Entry Screen
                                 </strong> </asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true" >  
</asp:ScriptManager>--%> 

  <table  border="1"  class="sample2" cellpadding="0" cellspacing="0" width ="100%" >
         <tr>
             <td colspan="2">
                  <asp:Label ID="Label2" runat="server"  width ="100%" Style="background-color: #EACFCE; color: Red; font-size: 11pt; font-variant: normal; ">* To mark absent award (-1) </asp:Label>
             </td>
            
         </tr>
         <tr>
             <td>
                 <asp:Label ID="Label3" runat="server">Batch :</asp:Label> 
             </td>
             <td>
                 <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                     <ContentTemplate>
                         <asp:DropDownList ID="ddlBatch" runat="server"  Width="151px" AutoPostBack ="true"  OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"></asp:DropDownList>
                     </ContentTemplate>
                 </asp:UpdatePanel>
             </td>
         </tr>       
         <tr id ="trCandidatesCount"  runat ="server" visible ="true" >
             <td id="tdlbl" class ="auto-style1" runat ="server" visible="true">
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                     <ContentTemplate>
                         <asp:Label ID="Label4" runat="server">Total Candidates in the the selected batch :</asp:Label>
                     </ContentTemplate>
                 </asp:UpdatePanel>
                       
             </td>   
             <td id="tdCandidatesCount" runat ="server" visible="true">
                 <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                     <ContentTemplate>
                         <asp:TextBox ID="txtCount" runat="server" Width="72px" Enabled="false"></asp:TextBox>
                     </ContentTemplate>
                 </asp:UpdatePanel>
             </td>         
         </tr>
     </table>
    <div style="overflow-y: scroll;height: 400px ;width: auto;">
         <table  border="1" cellpadding="0" cellspacing="0" width ="100%" >
        
        <tr>
             <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                 
                    <asp:Label ID="lblInfo" runat="server"  Visible ="false"  width ="100%" Style="background-color: #EACFCE; color: Red; font-size: 11pt; font-variant: normal; ">  </asp:Label>


                    <asp:GridView ID="grdCandidateData" runat="server"  AutoGenerateColumns="False" CssClass="gdrow"
                                Width="100%" AllowPaging="true" EnableTheming="false" BorderColor="Black" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PageSize="10"
                            OnPageIndexChanging="grdCandidateData_PageIndexChanging" PagerStyle-HorizontalAlign="Center"  OnRowDataBound="grdCandidateData_RowDataBound" AlternatingRowStyle-BackColor="#C9D7E2" PagerStyle-ForeColor="#0066FF"  >
                                <Columns>
                                      <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="table_04" HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle CssClass="table_02" HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                     <asp:BoundField HeaderStyle-Width="8%" HeaderStyle-BorderColor="Black" HeaderText="Centre Code"
                                        DataField="Center_Code" ItemStyle-BorderColor="Black" />
                                    <asp:BoundField HeaderStyle-Width="8%" HeaderStyle-BorderColor="Black" HeaderText="Registration Number"
                                        DataField="Registration_no" ItemStyle-BorderColor="Black" />
                                    <asp:BoundField HeaderStyle-Width="58%" HeaderStyle-BorderColor="Black" HeaderText="Candidate Name"
                                        DataField="Candidate_Name" ItemStyle-BorderColor="Black" />
                                    <asp:BoundField HeaderStyle-Width="12%" HeaderText="Module Name" HeaderStyle-BorderColor="Black"
                                        DataField="module_Short_Name" ItemStyle-BorderColor="Black" />
                                       <asp:BoundField HeaderStyle-Width="12%" HeaderText="Module ID" HeaderStyle-BorderColor="Black" Visible="true"
                                        DataField="module_id" ItemStyle-BorderColor="Black" />                                   
                                    <asp:TemplateField HeaderText="Observer Marks (Out of 40)">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtObserver40"></asp:TextBox>                                         
                                            <asp:LinkButton ID="lnkOUpdate" Text="Submit" runat="server" OnClick="OnUpdate" ForeColor="Blue" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>                                                                   
                                </Columns>
                                <PagerSettings Visible="true" />
                            </asp:GridView>
                </ContentTemplate> 
            </asp:UpdatePanel> 
        </tr>
           
       
    </table>

    </div>
   
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

