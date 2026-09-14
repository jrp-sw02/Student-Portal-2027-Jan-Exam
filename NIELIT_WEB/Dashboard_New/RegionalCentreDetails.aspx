<%@ Page Language="C#"MasterPageFile="~/MasterPages/DashBoard.master"  AutoEventWireup="true" CodeFile="RegionalCentreDetails.aspx.cs" Inherits="RegionalCentreDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
         .tdm
         {
             position: relative;
             color: orangered;
             background-color: #F5F5DC ;
             font-family: Arial;
             font-size: 12px;
             width: 100%;
             height: 30px;
             white-space: nowrap;
             padding-left:55px;
         }
          .Grid, .Grid th, .Grid td
           {
              border:1px solid #fff000;
              }            
         .RowStyle {
            height: 50px;
                  }
                  .AlternateRowStyle {
                 height: 50px;
                }
                   .normal

      {

          background-color:white;

      }
      
       .tdcur
        {
            cursor: pointer;
        }
        .hover_row
        {
            background-color: #50C878;
        }

         </style>
    <script type="text/javascript" src="Scripts/js/jquery.min.js" ></script> 
    <script type="text/javascript">
        $(function () {
            $("[id*=GridView1] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
   
     <div class="container">
        <div class="row">
        <%-- <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
            <tr>
                         <td class="tdm">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/HomePage.aspx" style="text-decoration:none" Font-Bold="True" Font-Size="Medium" ForeColor="#FF9900">Home / </asp:HyperLink>--%>
            <div class="col-sm-1 col-sm-1 "></div> 
           

        <div class="col-sm-10 col-sm-10 ">
            <div class="text-center">
    <asp:GridView ID="grdCentre" Width ="100%" runat ="server" AutoGenerateColumns="False" CellPadding="2" DataKeyNames="ID" Caption="&lt;center&gt;&lt;font size=&quot;4&quot; color=&quot;#D2B48C&quot;&gt;&lt;b&gt;NIELIT CENTRE / STUDY / EXT. CENTRE DETAILS&lt;br/&gt;&lt;/b&gt;&lt;/font&gt;&lt;/centre&gt;"    Font-Size ="Medium" EmptyDataText="No data found" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" ForeColor="Black" GridLines="Both"  >
        <AlternatingRowStyle BackColor="PaleGoldenrod" />
        <Columns>
            <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
          <ItemStyle Width ="3%" />
            <HeaderStyle HorizontalAlign="Left" Width="3%"></HeaderStyle>
          
        </asp:TemplateField>
             <asp:TemplateField HeaderText="Name" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:Label Font-Bold ="true"  runat="server" ID="name" Text='<%# Eval("name")%>'></asp:Label>
               
            </ItemTemplate>
            
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                  <ItemStyle HorizontalAlign="Left"  Width ="10%"/>
            
        </asp:TemplateField>
           <asp:TemplateField HeaderText="Address" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:Label runat="server" ID="address" Text='<%# string.Concat(Eval("Address1") ," ",Eval("Address2")," ",Eval("City_Name"),"<br/>",Eval("District")," ",Eval("State"),"-",Eval("Pin_Code"))%>'></asp:Label>
              </ItemTemplate>

                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                <ItemStyle HorizontalAlign="Left" Width ="15%" />
           </asp:TemplateField>

           
           
            <asp:TemplateField HeaderText="Name and Designation<br/> of Contact Person" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                
                <asp:Label runat="server" ID="T201415" Text='<%# string.Concat(Eval("Contact_Person_Name"),"<br/>",Eval("Contact_Person_Post"))%>' ></asp:Label>
               
            </ItemTemplate>
            
<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
            <ItemStyle HorizontalAlign="Left" Width ="10%" />
        </asp:TemplateField>
           <asp:TemplateField HeaderText="Contact No. / Email ID" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:Label runat="server" ID="contactEmail" Text='<%# string.Concat(Eval("Std_No"),"-",Eval("Phone1"),",<br/>",Eval("Mobile"),"<br/>",Eval("email1"))%>'></asp:Label>
              </ItemTemplate>
            
<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                 <ItemStyle HorizontalAlign="Left" Width ="10%" />
            
        </asp:TemplateField>
           
            
            
        </Columns>
        <FooterStyle BackColor="Tan" />
        <HeaderStyle BackColor="Tan" Font-Bold="True" />
        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="Tan" ForeColor="GhostWhite" />
        <SortedAscendingCellStyle BackColor="#FAFAE7" />
        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
        <SortedDescendingCellStyle BackColor="#E1DB9C" />
        <SortedDescendingHeaderStyle BackColor="#C2A47B" />
        </asp:GridView>
        <asp:SqlDataSource ID="dsDashboard" runat="server" ConnectionString="<%$ ConnectionStrings:EConnectContext %>" SelectCommand="getNIELITCentreDetails" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter Name="pInstituteId" QueryStringField="pInstituteID" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <asp:Button ID="btnBack" runat="server" CssClass ="btn btn-info" Text="Back"   Font-Bold ="true" OnClick="btnBack_Click" />
        </div>
            </div> 
            </div> 
         </div>
    
</asp:Content> 