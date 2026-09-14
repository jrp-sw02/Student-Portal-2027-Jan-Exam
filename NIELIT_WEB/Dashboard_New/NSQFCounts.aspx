<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="NSQFCounts.aspx.cs" Inherits="DashBoard1_NSQFCounts" %>

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

        .gvwCasesPager a {
            margin-left: 5px;
            margin-right: 5px;
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



        //$("[id*=GridView1] td").click(function () {
        //    var title;
        //    title = $(this).text();
        //    alert('123  ' + title);

        //});
        //alert('123  ' + title);
        //function callTitle() {
        //    //alert('claaaa')
        //    var title;
        //    //debugger;

        //    //alert('tuntU1 ' + title);
        //    //return title;
        //}

        $(document).ready(function () {
            var title;
            var txt = $('#<%= hdnSelected.ClientID %>').val();

            switch(txt)
            {
            case "0":
                GridViewBind();
                break;
             case "NIELIT Centres":
                ElseGridClick();
                callAjax(txt);
                break;
            case "Accredited Institutes":
                ifGridClick();
                callAjax(txt);
                break;
                case "STATE":
                StateGridGraph();
                callAjax(txt);
                break;

                default:
                    alert('default');
                    //StateGridGraph();
                    //callAjax(txt);
                    break;
                
            }
           
            $("[id*=GridView1] td").click(function () {
                var obj = {};
                title = $(this).text();
                $("#hdntitle").val(title);


                //var txt = $('#<%= lblGrand.ClientID %>').text();
                //alert('txt  ' + txt)
                if (title != null) {
                    callAjax(title);
                }

            });

            $("[id*=GridView1] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });

                
            
            });
        

    </script>

    <script type="text/javascript">
        
        function GridViewBind() {
           
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/getCandidatesNSQF",
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

            function DreawChart(series) {
                var date = new Date();
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
                var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

                $("#divNSQFGraph").show();
                //$("#divRegGraph").hide();
                //$("#divCourseCertifiedGraph").hide();
                
                $('#divNSQFGraph').highcharts({
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
                        text: 'NSQF Candidates Till ' + valDate + ''
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

        function ElseGridClick() {
            
            var obj = {};
            obj.pCentre = 1;
                
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/NSQFCentreCounts",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChartElseReg(data);
                    DreawChartElseCert(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });

            function DreawChartElseCert(series) {
                var date = new Date();
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
                var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").hide();
                //$("#divRegGraph").show();
                //$("#divCourseCertifiedGraph").show();
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
                        //text: 'NIELIT Students Registered '
                        text: 'NSQF Candidates Till ' + valDate + ''
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

            function DreawChartElseReg(series) {
                var date = new Date();
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
                var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").hide();
                //$("#divRegGraph").show();
                //$("#divCourseCertifiedGraph").show();
                $('#divRegGraph').highcharts({
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
                        text: 'NSQF Candidates Till ' + valDate + ''
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

        function ifGridClick() {
            //alert('I');
            var obj = {};
            obj.pCentre = 0;


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/NSQFCentreCounts",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChartifReg(data);
                    DreawChartifCert(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });

            function DreawChartifReg(series) {
                var date = new Date();
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
                var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

                
                $('#divRegGraph').highcharts({
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
                        text: 'NSQF Candidates Till ' + valDate + ''
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

            function DreawChartifCert(series) {
                var date = new Date();
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                        "Aug", "Sep", "Oct", "Nov", "Dec"];
                var valDate = date.getDate() + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").hide();
                //$("#divRegGraph").show();
                //$("#divCourseCertifiedGraph").show();
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
                        //text: 'NIELIT Students Registered '
                        text: 'NSQF Candidates Till ' + valDate + ''
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

        function StateGridGraph() {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/EmergingTrends",
                data: "{}",
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
                        text: 'NSQF Candidates Till ' + valDate + ''
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

            if (condition === 'NIELIT Centres') {


                $("#divNSQFGraph").hide();
                $("#divRegGraph").show();
                $("#divCourseCertifiedGraph").show();

            }
            else if (condition === 'Accredited Institutes') {

                //ElseGridClick();
                $("#divNSQFGraph").hide();
                $("#divRegGraph").show();
                $("#divCourseCertifiedGraph").show();

            }
            else {

                $("#divNSQFGraph").hide();
                $("#divRegGraph").hide();
                $("#divCourseCertifiedGraph").hide();
                $("#divStateGraph").show();
            }

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row"> 
            


            <div id="divRegGraph" style="display: none"   class="col-md-6">
                
            </div>

            <div id="divCourseCertifiedGraph" style="display: none"  class="col-md-6">
                
            </div>
            <div id="divNSQFGraph"       class="col-md-12">
            </div>
            
            <div id="divStateGraph"  style="display: none"    class="col-md-12">
            </div>

        </div>

        <div id="tblCat" visible="false" class="row" runat="server">
            <table class="table table-bordered table-responsive">
                <tbody>
                    <tr align="center" class="success">
                        <th style="text-align: center;">SC Wise Breakup</th>
                        <th style="text-align: center;">ST Wise Breakup</th>
                        <th style="text-align: center;">OBC Wise Breakup</th>
                        <th style="text-align: center;">PH Wise Breakup</th>
                    </tr>
                    <tr align="center" class="success">
                        <td style="text-align: center;">
                            <asp:CheckBox ID="chk_sc" AutoPostBack="true" runat="server" OnCheckedChanged="chk_sc_CheckedChanged"></asp:CheckBox>
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox ID="chk_st" runat="server" AutoPostBack="true" OnCheckedChanged="chk_st_CheckedChanged"></asp:CheckBox>
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox ID="chk_OBC" runat="server" AutoPostBack="true" OnCheckedChanged="chk_OBC_CheckedChanged"></asp:CheckBox>
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox ID="chk_PH" runat="server" AutoPostBack="true" OnCheckedChanged="chk_PH_CheckedChanged"></asp:CheckBox>
                        </td>
                    </tr>

                </tbody>
            </table>
        </div>

        <div class="row">

            <div class="col-sm-1 col-sm-1 "></div>


            <div class="col-sm-10 col-sm-10 ">

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                    Width="80%" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                    Caption="&lt;center&gt;&lt;font size=&quot;4&quot; color=&amp;quot#00008B&quot;&gt;&lt;b&gt;NIELIT NSQF CANDIDATES COUNT&lt;br/&gt;&lt;/b&gt;&lt;/font&gt;&lt;/centre&gt;"
                    Font-Size="Medium" EmptyDataText="No data found"
                    CellPadding="4" DataKeyNames="Centre" CellSpacing="2" ForeColor="#003153" OnRowCreated="GridView1_RowCreated"
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" PageSize="15">
                    <Columns>
                        <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>

                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Centre" HeaderText="Training Partner" ItemStyle-Width="150" />
                        <asp:BoundField DataField="Count" HeaderText="Count" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White" Height="35px" HorizontalAlign="Center" />
                    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Center" CssClass="gvwCasesPager" />
                    <RowStyle BackColor="White" Height="30px" Font-Size="12px" ForeColor="#003153" />
                    <AlternatingRowStyle Height="30px" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#808080" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
                <div class="row"></div>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-info" Text="Back" OnClick="btnBack_Click" />
            </div>
            <div class="col-sm-1 col-sm-1 "></div>
        </div>
        <%--<div class="row">
            <div class="col-sm-12 col-sm-12 ">
                <asp:Label ID="lblcentrenamelist" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Salmon"></asp:Label>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-sm-1 col-sm-1 "></div>

            <div class="col-sm-10 col-sm-10 ">
                <asp:GridView ID="gvpartnerlist" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                    BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" OnSelectedIndexChanged="gvpartnerlist_OnSelectedIndexChanged" HeaderStyle-Font-Size="14px"
                    OnRowDataBound="OnRowDataBound1" OnRowCommand="gvpartnerlist_RowCommand" OnRowCreated="gvpartnerlist_RowCreated" OnPageIndexChanging="gvpartnerlist_PageIndexChanging" AutoGenerateColumns="false" Caption="&lt;b&gt;&lt;center&gt;NSQF CANDIDATES COUNT&lt;/centre&gt;&lt;/b&gt;" AllowPaging="True" DataKeyNames="ID" PageSize="15">
                    <Columns>
                        <asp:TemplateField HeaderText="S. No." ItemStyle-Width="25">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="50px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="name" HeaderText="State Name" ItemStyle-Width="450" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Registered" HeaderText="Candidates Registered" ItemStyle-Width="450" ItemStyle-HorizontalAlign="Left" />

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSC" Text='<%# Eval("RegSC") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadSC" Text="SC" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblST" Text='<%# Eval("RegST") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadST" Text="ST" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOBC" Text='<%# Eval("RegOBC") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadOBC" Text="OBC" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPH" Text='<%# Eval("RegPH") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadPH" Text="PH" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>


                        <asp:BoundField DataField="CertificateIssue" HeaderText="Candidates Certified" ItemStyle-Width="450" ItemStyle-HorizontalAlign="Left" />

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCertSC" Text='<%# Eval("CertSC") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadCertSC" Text="SC" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCertST" Text='<%# Eval("CertST") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadCertST" Text="ST" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCertOBC" Text='<%# Eval("CertOBC") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadCertOBC" Text="OBC" runat="server" />
                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="100" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCertPH" Text='<%# Eval("CertPH") %>' runat="server" />

                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblheadCertPH" Text="PH" runat="server" />

                            </HeaderTemplate>
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>


                    </Columns>
                    <HeaderStyle BackColor="#003153" Font-Bold="True" ForeColor="White" Height="35px" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" Height="30px" Font-Size="12px" ForeColor="#003153" />
                    <AlternatingRowStyle Height="30px" />
                    <PagerStyle CssClass="gvwCasesPager" />
                </asp:GridView>
            </div>

            <div class="col-sm-1 col-sm-1"></div>
            <asp:Label ID="lblGrand" runat="server" Visible="false" Text="0"></asp:Label>
            <asp:Label ID="lblCenter" runat="server" Visible="false" Text="1"></asp:Label>
        </div>
        <div class="row">
            <div class="col-sm-1 col-sm-1 "></div>

            <div class="col-sm-10 col-sm-10 ">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                  width="100%" Caption="<center><b><font size=4 color=black>EMERGING TRENDS COURSES LIST</font></b></center>"
                    DataKeyNames="ID" ForeColor="Black"
                    PageSize="100" GridLines="Both" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100" HeaderStyle-HorizontalAlign ="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>

                            <%--<ItemStyle Width="100px"></ItemStyle>--%>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="50" HeaderStyle-HorizontalAlign ="Left" Visible ="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblInstID" Text='<%# Eval("ID")%>' runat="server" />
                            </ItemTemplate>

                            <%--<ItemStyle Width="100px"></ItemStyle>--%>

                             <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        
                                <ItemStyle Width="100px"></ItemStyle>
                        </asp:TemplateField>
  
                          <asp:BoundField DataField="Course" HeaderText="Course Name" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                             <asp:BoundField DataField="Count" HeaderText="CandidateCount" ItemStyle-Width="150" >
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

    </div>

      <input type="hidden" id="hdnSelected" name="hdnSelected" runat="server" enableviewstate="true"/>
      
</asp:Content>

