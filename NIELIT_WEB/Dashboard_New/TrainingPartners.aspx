<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" EnableEventValidation = "false"  CodeFile="TrainingPartners.aspx.cs" Inherits="DashBoard1_TrainingPartners" %>

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
  <%--  <script type="text/javascript" src="Scripts/js/jquery.min.js" ></script> 
     <script src="assets/js/jquery.min.js"></script>
     <script src="assets/js/highcharts.js"></script>
    <script src="assets/js/exporting.js"></script>--%>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>   
    <script src="asset1/js/jquery.min.js"></script>
    <script src="asset1/js/highcharts.js"></script>
    <script src="asset1/js/exporting.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=GridView1] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
        </script>

      <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/NielitCentre",
                data: "{}",
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChart(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        });
        function DreawChart(series) {
            var date = new Date();
            var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                     "Aug", "Sep", "Oct", "Nov", "Dec"];
            var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();


            $('#divNielitCentreGraph').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 1, //null,
                    plotShadow: false,
                    backgroundColor: {
                        linearGradient: [0, 0, 500, 500],
                        stops: [
                    [0, 'rgb(255, 255, 255)'],
                    [1, 'rgb(200, 200, 255)']
                        ]
                    }
                },
                title: {
                    //text: 'NIELIT Students Registered '
                    text: 'NIELIT Centres  Till ' + valDate + ''
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: 'NIELIT Centres',
                    data: series
                }]
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row">
            <div id="divNielitCentreGraph">

            </div>
        </div>
        <div class="row">
       
            <div class="col-sm-1 col-sm-1 "></div> 
           

        <div class="col-sm-10 col-sm-10 ">                            
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
               width="80%"     BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"   
                                 Caption="&lt;center&gt;&lt;font size=&quot;4&quot; color=&amp;quot#00008B&quot;&gt;&lt;b&gt;NIELIT TRAINING PARTNERS COUNT&lt;br/&gt;&lt;/b&gt;&lt;/font&gt;&lt;/centre&gt;"   
                                  Font-Size ="Medium" EmptyDataText="No data found"
                    CellPadding="4" DataKeyNames="id" CellSpacing="2" ForeColor="#003153" OnRowCreated="GridView1_RowCreated" 
                          OnRowDataBound="OnRowDataBound"   OnSelectedIndexChanged = "OnSelectedIndexChanged" PageSize="15" >  
                    <Columns>  
                        <asp:TemplateField HeaderText = "S. No." ItemStyle-Width="100">
                     <ItemTemplate>
                           <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                     </ItemTemplate>

<ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
                         <asp:BoundField DataField="TP" HeaderText="Training Partner" ItemStyle-Width="150" />
                        <asp:BoundField DataField="countTP" HeaderText="Count" ItemStyle-Width="150"  ItemStyle-HorizontalAlign ="Center"  HeaderStyle-HorizontalAlign ="Center" />                      
                    </Columns>  
                    <FooterStyle BackColor="#CCCCCC" />  
                    <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White"   Height="35px" HorizontalAlign="Center"   />  
                    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Center"  CssClass ="gvwCasesPager"/>  
                    <RowStyle BackColor="White"  Height="30px" Font-Size="12px"  ForeColor="#003153" />                                  
                    <alternatingrowstyle Height="30px" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />  
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />  
                    <SortedAscendingHeaderStyle BackColor="#808080" />  
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />  
                    <SortedDescendingHeaderStyle BackColor="#383838" />  
                </asp:GridView>                  
            <div class="row"></div>
                            <asp:Button ID="btnBack" runat="server" CssClass ="btn btn-info" Text="Back" OnClick="btnBack_Click" />
          </div>
            <div  class="col-sm-1 col-sm-1 "></div>       
            </div> 
            <div class="row">
                <div  class="col-sm-12 col-sm-12 ">
             <asp:Label ID="lblcentrenamelist" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Salmon"></asp:Label></td></tr>
                    </div>
                </div>
            <div class="row">
                <div  class="col-sm-1 col-sm-1 "></div>
                    
                <div  class="col-sm-10 col-sm-10 ">
                        <asp:GridView ID="gvpartnerlist" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                       BackColor="#CCCCCC" BorderColor="#999999"  BorderStyle="Solid" BorderWidth="1px" OnSelectedIndexChanged = "gvpartnerlist_OnSelectedIndexChanged"  HeaderStyle-Font-Size="14px" 
                       OnRowDataBound ="OnRowDataBound1"    OnRowCommand ="gvpartnerlist_RowCommand"     OnRowCreated="gvpartnerlist_RowCreated" OnPageIndexChanging ="gvpartnerlist_PageIndexChanging"  AutoGenerateColumns="false" Caption="&lt;b&gt;&lt;center&gt;NIELIT CENTRES / STUDY / EXT. CENTRES&lt;/centre&gt;&lt;/b&gt;" AllowPaging="True" DataKeyNames="instituteID" PageSize="15">
                            <Columns>
         <asp:TemplateField HeaderText = "S. No." ItemStyle-Width="25">
                     <ItemTemplate>
                           <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server"  />
                     </ItemTemplate>
                            <ItemStyle Width="50px"></ItemStyle>
                        </asp:TemplateField>
                            <asp:BoundField DataField="name" HeaderText="Centre Name" ItemStyle-Width="450" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="State" HeaderText="State" ItemStyle-Width="450" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="LinkedToCentre" HeaderText="Linked to Centre Name" ItemStyle-Width="450" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText = "Courses" ItemStyle-Width="25">
                     <ItemTemplate>
                           <asp:LinkButton ID="lnkCourses" CssClass ="btn btn-primary" Text ="Check Courses" runat ="server" CommandName ="CheckCourses" CommandArgument ='<%# Eval("instituteID")%>'  ></asp:LinkButton>
                     </ItemTemplate>
                            <ItemStyle Width="200px"></ItemStyle>
                        </asp:TemplateField>
                            </Columns>
                             <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White"   Height="35px" HorizontalAlign="Left"   /> 
                            <RowStyle BackColor="White"  Height="30px" Font-Size="12px"  ForeColor="#003153" />
                            <alternatingrowstyle Height="30px" />
                            <PagerStyle CssClass ="gvwCasesPager" />
                        </asp:GridView>
                    </div> 
                  
                <div  class="col-sm-1 col-sm-1"></div> 
                       </div>
    </div>
</asp:Content>

