<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="RegisteredCandList1.aspx.cs" Inherits="DashBoard1_RegisteredCandList" %>

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
        .chart-container {
    width: 100%; /* Set the width to cover the full page */
    height: 50%; /* Set the height to cover the full viewport height */
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
    <%--<script type="text/javascript" src="Scripts/js/jquery.min.js"></script>
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/js/highcharts.js"></script>
    <script src="assets/js/exporting.js"></script>--%>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>   
    <script src="asset1/js/jquery.min.js"></script>
    <script src="asset1/js/highcharts.js"></script>
    <script src="asset1/js/exporting.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var txt = $('#<%= hdnSelected.ClientID %>').val();
            var sID = $('#<%= hdnStateID.ClientID %>').val();
            switch (txt) {
                case "0":
                    GridViewBind();
                    break;

                case "STATE":
                    StateGridGraph(sID);


                    callAjax(txt);
                    break;

                default:
                    //StateGridGraph();
                    //callAjax(txt);
                    break;

            }


            GridViewBind();
        });

    </script>  
    <script type="text/javascript">
        $(function () {
            $("[id*=GridView1] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });


        function GridViewBind() {
           
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/RegisterCandYearWise",
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

            //$.ajax({
            //    type: "POST",
            //    contentType: "application/json; charset=utf-8",
            //    url: "Services/Graph.asmx/CourseCompleted",
            //    data: "{}",
            //    dataType: "json",
            //    success: function (Result) {
            //        Result = Result.d;
            //        var data = [];

            //        for (var i in Result) {
            //            var serie = new Array(Result[i].Name, Result[i].Value);
            //            data.push(serie);
            //        }

            //        DreawChart1(data);
            //    },
            //    error: function (Result) {
            //        alert("Error");
            //    }
            //});

        function DreawChart(series) {
            var date = new Date();
            var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
            //var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

            var valDate = date.getFullYear();
            

            $('#divRegGraph').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 2, //null,
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
                    text: 'NIELIT Students Registered  in Year ' + valDate + ''
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
                    name: 'NIELIT Registered Candidates',
                    data: series
                }]
            });
        }

        function DreawChart1(series) {
            var date = new Date();
            var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
            var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

            $('#divCourseCertifiedGraph').highcharts({
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
                    text: 'NIELIT Students Certified Till ' + valDate + ''
                    //text: 'NIELIT Course Completed '
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
                    name: 'NIELIT Students Certified',
                    data: series
                }]
            });
        }

        }

        function StateGridGraph(sID) {

            var obj = {};
            obj.pCentre = sID;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/StateWiseNSQFCounts",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChartState(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });

            function DreawChartState(series) {
                var date = new Date();
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
                var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").show();
                //$("#divRegGraph").hide();
                //$("#divCourseCertifiedGraph").hide();

                $('#divStateGraph').highcharts({
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
                        text: 'CANDIDATES NIELIT CENTRES Till ' + valDate + ''
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
                        name: 'NIELIT Registered Candidates',
                        data: series
                    }]
                });
            }

        }

        function callAjax(condition) {

            if (condition === '0') {

                $("#divRegGraph").show();
                $("#divCourseCertifiedGraph").show();

            }
            if (condition === 'STATE') {

                $("#divStateGraph").show();
                $("#divRegGraph").hide();
                $("#divCourseCertifiedGraph").hide();

            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row">
            <div id="divRegGraph" class="chart-container">
            </div>

            <div id="divCourseCertifiedGraph" class="col-md-6">
            </div>

            <div id="divStateGraph"  style="display: none"    class="col-md-12">
            </div>

        </div>
        

        <div id="updatpanel1">
            <div style="overflow: auto; height: 800px;">
                <center>
                    <div id="tblCat" visible="false" class="row" runat="server">
                    <table class="table table-bordered table-responsive">
                    <tbody>
                        <%--<tr align="center" class="success">
                            <th style="text-align:center;">SC Wise Breakup</th>
                            <th style="text-align:center;">ST Wise Breakup</th>--%>>
                            <th style="text-align:center;"> REGISTERED CANDIDATES YEARWISE</th>
                            <%--<th style="text-align:center;">PH Wise Breakup</th>
                         </tr>
                        <tr align="center" class="success">
                            <td style="text-align:center;">
                                <asp:CheckBox ID="chk_sc" AutoPostBack="true" runat="server" OnCheckedChanged="chk_sc_CheckedChanged"></asp:CheckBox>
                            </td>
                            <td style="text-align:center;">
                                <asp:CheckBox ID="chk_st" runat="server" AutoPostBack="true" OnCheckedChanged="chk_st_CheckedChanged"></asp:CheckBox>
                            </td>
                            <td style="text-align:center;">
                                <asp:CheckBox ID="chk_OBC" runat="server" AutoPostBack="true" OnCheckedChanged="chk_OBC_CheckedChanged"></asp:CheckBox>
                            </td>
                            <td style="text-align:center;">
                                <asp:CheckBox ID="chk_PH" runat="server" AutoPostBack="true" OnCheckedChanged="chk_PH_CheckedChanged"></asp:CheckBox>
                            </td>
                        </tr>--%>

                    </tbody>
                    </table>
                    </div>
                    <div class="row">
                        <div class="col-sm-1 col-sm-1 ">
                            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-info" Text="Back" OnClick="btnBack_Click" />

                        </div>

                        <div class="col-sm-10 col-sm-10 ">

                             
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                                    BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="id" CellSpacing="2" ForeColor="#003153" OnRowCreated="GridView1_RowCreated"
                             Caption="&lt;center&gt;&lt;font size=&quot;4&quot; color=&amp;quot#00008B&quot;&gt;&lt;b&gt; CANDIDATES REGISTERED YEARWISE COUNT&lt;br/&gt;&lt;/b&gt;&lt;/font&gt;&lt;/centre&gt;"
                                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" AllowPaging="True" PageSize="15"  OnPageIndexChanging="GridView1_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                            </ItemTemplate>

                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="State Name" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="Registered" HeaderText="Students Registered" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="_year" HeaderText="Year" ItemStyle-Width="150" />
                                         
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White" Height="35px" HorizontalAlign="Left" />
                                    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                                    <RowStyle BackColor="White" Height="30px" Font-Size="12px" ForeColor="#003153" />
                                    <AlternatingRowStyle Height="30px" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#808080" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                              
                                </asp:GridView>
                            <div class="row" ></div>
                           
                        </div>

                        <div class="col-sm-1 col-sm-1 "></div>

                    </div>
                    
                    <div class="row">
            <div class="col-sm-1 col-sm-1 "></div>

            <div class="col-sm-10 col-sm-10 ">
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                  width="100%" 
                    DataKeyNames="Course" ForeColor="Black"
                    PageSize="100" GridLines="Both" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100" HeaderStyle-HorizontalAlign ="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>


                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Course" ItemStyle-Width="50" HeaderStyle-HorizontalAlign ="Left" Visible ="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblInstID" Text='<%# Eval("Course")%>' runat="server" />
                            </ItemTemplate>

                            <%--<ItemStyle Width="100px"></ItemStyle>--%>

                             <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        
                                <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
  
                          <asp:BoundField DataField="Course" HeaderText="Course Name" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                             <asp:BoundField DataField="Total" HeaderText="CandidateCount" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                          
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
            <div class="col-sm-1 col-sm-1"></div>            
        </div>
                </center>
            </div>
        </div>        
    </div>
    <input type="hidden" id="hdnSelected" name="hdnSelected" runat="server" enableviewstate="true"/>
    <input type="hidden" id="hdnStateID" name="hdnSelected" runat="server" enableviewstate="true"/>
</asp:Content>

