<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="NIELITCentreInst.aspx.cs" Inherits="DashBoard1_NIELITCentreInst" %>

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
        .gvwCasesPager a
            {
                margin-left:5px;
                margin-right:5px;
            }

    </style>
    <script type="text/javascript" src="Scripts/js/jquery.min.js"></script>
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/js/highcharts.js"></script>
    <script src="assets/js/exporting.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/NielitAccrInst",
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


            $('#divNielitAccrInst').highcharts({
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
                    text: 'NIELIT Accredited institutes  Till ' + valDate + ''
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
                    name: 'Accredited Institutes',
                    data: series
                }]
            });
        }

    </script>  
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="container">
         <div class="row">
            <div id="divNielitAccrInst">

            </div>
        </div>
        <div class="row">

            <div class="col-sm-1 col-sm-1 "></div>


            <div class="col-sm-10 col-sm-10 ">
                <div class="text-center">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                        Width="100%" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                        Caption="<b><center>NIELIT ACCREDITED INSTITUTES</centre></b>" CellPadding="4" DataKeyNames="id" CellSpacing="2" ForeColor="#003153" OnRowCreated="GridView1_RowCreated"
                        OnPageIndexChanging="OnPaging" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" AllowCustomPaging="False" PageSize="20" AllowPaging="True">
                        <Columns>
                            <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                </ItemTemplate>

                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="State Name" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Center" HeaderText="Institute Count" ItemStyle-Width="150" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White" Height="35px" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left"  CssClass ="gvwCasesPager"/>
                        <RowStyle BackColor="White" Height="30px" Font-Size="12px" ForeColor="#003153" />
                        <AlternatingRowStyle Height="30px" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>
                </div>
              
                        <asp:Button ID="btnBack"  runat="server" CssClass ="btn btn-info"  Text="Back" OnClick="btnBack_Click" />
                   
            </div>
        </div>

        <div class="row">
            <div class="col-sm-3 col-sm-3"></div>
            <div class="col-sm-6 col-sm-6">
                <asp:Label ID="lblstatenamelist" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="#3AC0F2"></asp:Label>
            </div>
            <div class="col-sm-3 col-sm-3"></div> 

        </div>
        <div class="row">

            <div class="col-sm-1 col-sm-1 "></div>


            <div class="col-sm-10 col-sm-10 ">
                <div class="text-center">

                    <asp:GridView ID="gvpartnerlist" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        Width="100%" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" HeaderStyle-Font-Size="14px"
                        OnPageIndexChanging="OnPaging1" OnRowCreated="gvpartnerlist_RowCreated" AutoGenerateColumns="false" PageSize="20" AllowPaging="True">
                        <Columns>
                            <asp:TemplateField HeaderText="S. No." ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="50px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="INSTITUTE NAME" ItemStyle-Width="450" />
                            <asp:BoundField DataField="City_Name" HeaderText="CITY NAME" ItemStyle-Width="450" />
                        </Columns>
                        <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White" Height="35px" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" Height="30px" Font-Size="12px" ForeColor="#003153" />
                        <AlternatingRowStyle Height="30px" />
                        <PagerStyle  HorizontalAlign ="Center" CssClass="gvwCasesPager"  />
                        
                    </asp:GridView>

                </div>
            </div>
            <div class="col-sm-1 col-sm-1 "></div>
        </div>

    </div>
</asp:Content>

