<%@ Page Language="C#"MasterPageFile="~/MasterPages/DashBoard.master"  AutoEventWireup="true" CodeFile="NIELITCentreCourses.aspx.cs" Inherits="NIELITCentreCourses" %>

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
        .gvwCasesPager a
            {
                margin-left:5px;
                margin-right:5px;
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
    <script>
        // Disable right-click context menu
        window.addEventListener('contextmenu', function (e) {
            e.preventDefault();
        });

        // Disable keyboard shortcuts for developer tools
        window.addEventListener('keydown', function (e) {
            if (e.keyCode === 123 || (e.ctrlKey && e.shiftKey && e.keyCode === 73)) {
                e.preventDefault();
            }
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
      <div class="row">
    <div class="col-sm-1 col-sm-1 ">
         <asp:Button ID="btnBack" CssClass ="btn btn-info" runat="server" Text="Back" OnClick="btnBack_Click" Visible="False" />
    </div>
             
     <div class="col-sm-3 col-sm-3 "></div>  
   <div class="col-sm-4 col-sm-4 ">
       <div class="text-center" >

        <asp:Label ID="lblCentre" runat ="server" Visible ="false" Font-Size="Large"    Font-Bold ="true"></asp:Label>
       </div>
    </div>
   <div class="col-sm-4 col-sm-4 "></div>
        </div> 
   <div class="row">
       <div class="col-sm-1 col-sm-1 "></div>
    <div class="col-sm-10 col-sm-10 ">
       <asp:GridView ID="grdCentreCourse" Width ="100%"  runat ="server" AutoGenerateColumns="False" CellPadding="5" CellSpacing="5" DataKeyNames="wID"  Caption="&lt;center&gt;&lt;font size=&quot;3&quot; color=&amp;quot#00008B&quot;&gt;&lt;b&gt;Courses Details&lt;br/&gt;&lt;/b&gt;&lt;/font&gt;&lt;/centre&gt;"    Font-Size ="Medium" EmptyDataText="No data found" AllowPaging="True" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" BorderStyle="None" PageSize="15"  OnPageIndexChanging ="grdCentreCourse_PageIndexChanging" >
        <Columns>
            <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
          <ItemStyle Width ="3%" HorizontalAlign ="Center"  />
            <HeaderStyle HorizontalAlign="Left" Width="3%"></HeaderStyle>
          
        </asp:TemplateField>
             <asp:TemplateField HeaderText="Name of Course" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:Label  runat="server" ID="name" Text='<%# Eval("cName")%>'></asp:Label>
               
            </ItemTemplate>
            
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                  <ItemStyle HorizontalAlign="Left"  Width ="10%"/>
            
        </asp:TemplateField>
          
           
            
            
        </Columns>
           <FooterStyle BackColor="White" ForeColor="#000066" />
           <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" CssClass="gvwCasesPager"/>
           <RowStyle ForeColor="#000066" />
           <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
           <SortedAscendingCellStyle BackColor="#F1F1F1" />
           <SortedAscendingHeaderStyle BackColor="#007DBB" />
           <SortedDescendingCellStyle BackColor="#CAC9C9" />
           <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
        <asp:SqlDataSource ID="dsDashboard" runat="server" ConnectionString="<%$ ConnectionStrings:EConnectContext %>" SelectCommand="getNIELITCentreCourses" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter Name="pInstID" QueryStringField="pInstituteID" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
        <div class="col-sm-1 col-sm-1 "></div>
       </div> 
       
        </div>
</asp:Content> 