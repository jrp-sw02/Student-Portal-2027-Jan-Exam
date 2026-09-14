<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="NielitCentres.aspx.cs" Inherits="DashBoard1_NIELITCentres" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .tdm {
            position: relative;
            color: orangered;
            background-color: #F5F5DC;
            font-family: Arial;
            font-size: 12px;
            width: 100%;
            height: 30px;
            white-space: nowrap;
            padding-left: 55px;
        }

        .Grid, .Grid th, .Grid td {
            border: 1px solid #fff000;
        }

        .RowStyle {
            height: 50px;
        }

        .AlternateRowStyle {
            height: 50px;
        }

        .normal {
            background-color: white;
        }

        .tdcur {
            cursor: pointer;
        }

        .hover_row {
            background-color: #50C878;
        }
    </style>

    <%--<script src="https://code.jquery.com/jquery-2.2.4.js" integrity="sha256-iT6Q9iMJYuQiMWNd9lDyBUStIq/8PuOW33aOqmvFpqI=" crossorigin="anonymous"></script>--%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <%--<script type="text/javascript" src="Script1/jquery.min.js"></script>--%>
    <script src="asset1/js/jquery.min.js"></script>
    <script src="asset1/js/highcharts.js"></script>
    <script src="asset1/js/exporting.js"></script>
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
    <script type="text/javascript">
        //$(function () {
        //    $("[id*=GridView1] td").hover(function () {
        //        $("td", $(this).closest("tr")).addClass("hover_row");
        //    }, function () {
        //        $("td", $(this).closest("tr")).removeClass("hover_row");
        //    });
        //});
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row">
            <div id="divNielitCentreGraph">

            </div>
        </div>
        <div class="row">
        <%-- <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
            <tr>
                         <td class="tdm">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/HomePage.aspx" style="text-decoration:none" Font-Bold="True" Font-Size="Medium" ForeColor="#FF9900">Home / </asp:HyperLink>--%>
            <div class="col-sm-1 col-sm-1 "></div> 
           

        <div class="col-sm-10 col-sm-10 ">
            <div class="text-center">

                <%--<asp:Label ID="Label2" runat="server" Text="  NIELIT Centres list" Font-Bold="True" ForeColor="#DC143C"></asp:Label>
             </div> --%>

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                  width="100%" Caption="<center><b><font size=4 color=black>NIELIT CENTRES LIST</font></b></center>"
                    DataKeyNames="instituteID" ForeColor="Black"
                  OnrowCommand="gvChildGrid_RowCommand"   OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" PageSize="100" GridLines="Both" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100" HeaderStyle-HorizontalAlign ="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>

                            <%--<ItemStyle Width="100px"></ItemStyle>--%>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="100" HeaderStyle-HorizontalAlign ="Left" Visible ="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblInstID" Text='<%# Eval("instituteID")%>' runat="server" />
                            </ItemTemplate>

                            <%--<ItemStyle Width="100px"></ItemStyle>--%>

                             <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        
                                <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderStyle-HorizontalAlign ="Left"  HeaderText ="State Name">  
                            <ItemTemplate>  
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%# Eval("Name")%>' CommandName="CentreDet" CommandArgument ='<%#Eval("instituteID") %>'/> 

                            </ItemTemplate>  
                          
                        </asp:TemplateField> --%>
                          <asp:BoundField DataField="Name" HeaderText="State Name" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign ="Left"  HeaderText ="NIELIT Centres">  
                            <ItemTemplate>  
                                <asp:LinkButton ID="lnkInstName" runat="server" Text='<%# Eval("instname")%>' CommandName="CentreDet" CommandArgument ='<%#Eval("instituteID") %>'/> 

                            </ItemTemplate>  
                          
                        </asp:TemplateField> 

                      
                       <%-- <asp:BoundField DataField="instname" HeaderText="NIELIT Centres" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>--%>

                        <asp:TemplateField>  
                            <ItemTemplate>  
                       
                <asp:GridView ID="gvChildGrid" Width ="100%" runat="server" AutoGenerateColumns="False" CellPadding="12" CellSpacing ="12"  ForeColor="#333333" GridLines="None"  ShowHeader="False" OnRowCommand ="gvChildGrid_RowCommand">  
                    <AlternatingRowStyle BackColor="White" />
                   <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign ="Left" >  
                            <ItemTemplate>  
                                
                                <asp:LinkButton ID="lblSubName" runat="server" Text='<%# " * "+  String.Format("{0:d}", Eval("instname"))%>' CommandName="CentreDet" CommandArgument ='<%#Eval("instituteID") %>'/> 

                            </ItemTemplate>  
                          <ItemStyle HorizontalAlign ="Left" />
                        </asp:TemplateField>  
                       
                      </Columns> 
                    
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Left" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                    
                    </asp:GridView>   
                   
    </ItemTemplate>  
    <HeaderTemplate>
            <asp:Label ID="lblExtCentre" runat ="server" Text =" Ext. /Study Centres"></asp:Label>
</HeaderTemplate>
           
</asp:TemplateField>  




                    </Columns>
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" Height="35px" HorizontalAlign="Left" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Left" />
                    <RowStyle Height="30px" Font-Size="12px" />
                    <AlternatingRowStyle Height="30px" BackColor="PaleGoldenrod" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>

            </div>
        </div>
        <div class="col-sm-12 col-sm-12 ">
            <asp:Button ID="btnBack" runat="server" Text="<< Back" OnClick="btnBack_Click" />
            <br />
            <asp:Label ID="lblstatenamelist" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="#FA8072"></asp:Label>
        </div>
       
    </div>
        </div>
</asp:Content>

