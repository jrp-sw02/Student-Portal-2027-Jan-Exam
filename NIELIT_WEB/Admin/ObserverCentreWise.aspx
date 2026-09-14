<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="ObserverCentreWise.aspx.cs" Inherits="Admin_ObserverCentreWise" %>

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

  <%--  <table  cellpadding="0" cellspacing="0" width ="100%">

        <tr>
            <td>
                <asp:label ID ="lblExamDate"  runat ="server">
                    Exam Date 
                </asp:label>
            </td>
            <td>
                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlExamDate" runat="server" Height="31px" SkinID="ddl250"
                            AutoPostBack="True" Width="292px" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>                 
                </asp:UpdatePanel>

            </td>
        </tr>

        <tr>
            <td>
                <asp:label ID ="Label1"  runat ="server">
                    Exam Reporting Time
                </asp:label>
            </td>
            <td>
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlReportingTime" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" Width="293px" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>                 
                </asp:UpdatePanel>

            </td>
        </tr>
    </table>--%>
    <table  border="1" cellpadding="0" cellspacing="0" width ="100%" >
        
        <tr>
             <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                  <%--  <asp:GridView runat="server" ID="grdCandidateData"
                        CssClass=""
                        AutoGenerateColumns="false"
                        ShowFooter="true">
                    </asp:GridView>--%>
                    <%--<columns>--%>

                      <%-- <asp:BoundField ItemStyle-Width="60%" DataField="Name" HeaderText="Accredited Centre" />
                        <asp:BoundField DataField="Registration_Number" HeaderText="REGISTRATION NO" >
                            <ItemStyle Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Candidate_Name" HeaderText="CANDIDATE NAME"  >
                            <ItemStyle Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Module_Short_Name" HeaderText="CODE" >
                            <ItemStyle Font-Size="Small" />
                        </asp:BoundField>--%>
                                              
                    <%--</columns>--%>
                    <asp:Label ID="Label1" runat="server" >* To mark absent award -1 Marks</asp:Label>
                    <asp:GridView ID="grdCandidateData" runat="server"  AutoGenerateColumns="False"
                                Width="100%" AllowPaging="false" EnableTheming="false" BorderColor="Black">
                                <Columns>

                                  
                                 <%--   <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>--%>
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
                                   
                                    <asp:TemplateField HeaderText  ="Observer Marks (Out of 40)">
                                        <ItemTemplate>                                            
                                            <asp:TextBox  runat="server" ID="txtObserver40"></asp:TextBox>
                                            <%--<asp:LinkButton ID="LinkButton1" Text="Edit" runat="server" CommandName="Edit" />--%>     
                                            <asp:LinkButton ID="lnkOUpdate" Text="Update" runat="server"  OnClick="OnUpdate"   />
                                           <asp:LinkButton ID="lnkOCancel" Text="Cancel" runat="server"  />                                                                               
                                        </ItemTemplate>

                                         <EditItemTemplate>
                                           
            
                                   </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    
                                   <%--  <asp:TemplateField HeaderText  ="Observer Marks (Out of 20)">
                                        <ItemTemplate>                                            
                                            <asp:TextBox  runat="server" ID="txtObserver20"></asp:TextBox>
                                            
                                            <asp:LinkButton ID="lnkO2Update" Text="Update" runat="server"  OnClick="OnUpdate" />
                                           <asp:LinkButton ID="LnkO2Cancel" Text="Cancel" runat="server"  />                                                                             
                                        </ItemTemplate>

                                         <EditItemTemplate>
                                           
            
                                   </EditItemTemplate>
                                    </asp:TemplateField>
                                   
      --%>
                                   <%-- <asp:BoundField HeaderStyle-Width="15%" HeaderText="Result" HeaderStyle-BorderColor="Black"
                                        DataField="Result" ItemStyle-BorderColor="Black" />
                                    <asp:BoundField HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HeaderText="Grade"
                                        HeaderStyle-BorderColor="Black" DataField="Grade" ItemStyle-BorderColor="Black" />--%>
                                </Columns>
                                <PagerSettings Visible="False" />
                            </asp:GridView>

                </ContentTemplate> 
            </asp:UpdatePanel> 
        </tr>
           
       
    </table>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

