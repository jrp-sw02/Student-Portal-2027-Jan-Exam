<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/FullInfo.master" AutoEventWireup="true" CodeFile="DownloadApplicationsPendingCount.aspx.cs"
     Inherits="Admin_DownloadApplicationsPendingCount" Debug ="true" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    List of Pending Candidates
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
    </br>  
    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <asp:button id="btnPendingCount" runat="server" text="Click here to view pending list" 
           visible="true" OnClick="btnPendingCount_Click"  />
    </br>
   <table width="100%" style ="align-content:center">
       <td>
           <asp:UpdatePanel  ID="Updatepanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvPendingCount" runat="server" AutoGenerateColumns="false" AllowPaging=" true" Visible ="true" Width="100%" >
                                    <Columns>                                
                                <%--<asp:TemplateField>
                                  <HeaderTemplate>
                                           #
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <%# Container.DataItemIndex + 1 %>
                                  </ItemTemplate>
                                </asp:TemplateField>--%>
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

                                <asp:BoundField DataField="Number" HeaderText="Application Number" SortExpression="BrowserName" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Name" HeaderText="Candidate Name" SortExpression="ClientIP" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Applied_Exam_Name" HeaderText="Exam Name" SortExpression="SourceIP" ItemStyle-HorizontalAlign="Left" />
                                <%--<asp:BoundField DataField="Pending_Count" HeaderText="Pending Count" SortExpression="SourceIP" ItemStyle-HorizontalAlign="Left" />--%>
                                                                 
                            </Columns>
                                    <PagerSettings Visible="true" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>

       </td>
   </table>
         
    <div id="divNavigation" runat="server">
        <%-- <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>                        
                        <uc3:pagingbar ID="PagingBar1" runat="server"  
                            OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server"
                    OnPageIndexChanged="PageIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContents" Runat="Server">
</asp:Content>

