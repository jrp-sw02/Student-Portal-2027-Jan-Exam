<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="CandidateCountforExamCheck.aspx.cs" Inherits="CandidateCountforExamCheck1" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Candidate Count for Exam
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server"> 

    <script type="text/javascript" language="javascript">

        function OpenWindow() {

           

            //Course Category
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Course Category"))
                return false;
            if (!isSelected("<%=ddlcourse.ClientID %>", "Data Download Sequence Number"))
                return false;
           
        }
    </script>
     <style type="text/css">
         .PromptCSS {
             color: Blue;
             font-size: small;
             font-style: italic;
             font-weight: bold;
             font-family: CourierNew;
             height: 20px;
             margin-left: 100px;
         }
         .auto-style1
         {
             height: 23px;
         }
     </style> 

    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td class="auto-style1">
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Exam Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlExamYear" runat="server" Height="22px" SkinID="ddl250"
                              Enabled="true" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged" AutoPostBack ="true"  >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>             
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                       <asp:DropDownList ID="ddlcourse" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDataDownloadedSequence" EventName="SelectedIndexChanged" />
                    </Triggers>--%>
                </asp:UpdatePanel>

            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                       <asp:DropDownList ID="ddlExamName" runat="server" Height="22px" SkinID="ddl250"
                            AutoPostBack="True" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDataDownloadedSequence" EventName="SelectedIndexChanged" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </td>
        </tr      
        <tr>
           
        </tr>
    </table>  
    <div style="text-align: right; margin-top: 10px">        
               
        <asp:Button ID="btn_result" runat="server" Text="Show Result" Width="140px" OnClick="btn_result_Click"   />                      
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" />

    </div>

    <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>                        
                        <asp:GridView ID="gvMain" runat="server"  AutoGenerateColumns="false" AllowPaging =" true" >
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

                                <asp:BoundField DataField="Date_of_Exam" HeaderText="Date of Exam" SortExpression="BrowserName" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="total_count" HeaderText="Total Count" SortExpression="ClientIP" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Course_Name" HeaderText="Course ID" SortExpression="SourceIP" ItemStyle-HorizontalAlign="Left" />
                                                                 
                            </Columns>
                            <PagerSettings Visible="true" />
                        </asp:GridView>                    
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        
                        <uc3:pagingbar ID="PagingBar1" runat="server"  
                            OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>


</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
