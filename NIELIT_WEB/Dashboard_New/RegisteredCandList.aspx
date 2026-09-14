<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="RegisteredCandList.aspx.cs" Inherits="DashBoard1_RegisteredCandList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .tdm
        {
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

        .Grid, .Grid th, .Grid td
        {
            border: 1px solid #fff000;
        }

        .RowStyle
        {
            height: 50px;
        }

        .AlternateRowStyle
        {
            height: 50px;
        }

        .normal
        {
            background-color: white;
        }

        .tdcur
        {
            cursor: pointer;
        }

        .hover_row
        {
            background-color: #50C878;
        }

        .error-message
        {
            color: red;
        }






        .form-group
        {
            max-width: 1150px;
            margin: 0 auto;
            padding: 0 px;
            background-color: white;
            /*border-radius: 5px;
        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);*/
        }

        h1
        {
            font-size: 5px;
            margin-bottom: 10px;
        }

        .dashboardheader
        {
            background: #F82605;
            font: bold 15px verdana;
            color: #000066;
            vertical-align: top;
           /*padding: 1000px 7px 2px 4px;*/
           padding-left:2000px;
            margin-left:0px;
            width: 1165px;
        }
        /* label {
        display: block;
        margin-bottom: 5px;
        font-weight: bold;
    }

    .form-control {
        width: 100%;
        padding: 8px;
        font-size: 16px;
        border: 1px solid #ccc;
        border-radius: 4px;
        box-sizing: border-box;
        margin-bottom: 10px;
    }

    .btn {
        display: inline-block;
        padding: 10px 20px;
        font-size: 16px;
        cursor: pointer;
        background-color: #007bff;
        color: #fff;
        border: none;
        border-radius: 4px;
        transition: background-color 0.3s;
    }

    .btn:hover {
        background-color: #0056b3;
    }

    .error-message {
        color: #dc3545;
        font-size: 14px;
    }

    .success {
        color: #28a745;
    }*/
        .auto-style1
        {
            height: 38px;
        }
    </style>
    <%--<script type="text/javascript" src="Scripts/js/jquery.min.js"></script>
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/js/highcharts.js"></script>
    <script src="assets/js/exporting.js"></script>--%>

    <%--<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>
    <script src="Script1/jquery-3.7.1.min.js" ></script>
    <script src="asset1/js/jquery.min.js"></script>
    <script src="asset1/js/highcharts.js"></script>
    <script src="asset1/js/exporting.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            //alert('switch');
            // debugger;
            var txt = $('#<%= hdnSelected.ClientID %>').val();
            var sID = $('#<%= hdnStateID.ClientID %>').val();
            var cID = $('#<%= hndCourseID.ClientID %>').val();
            //var cID = '1';
            switch (txt) {
                case "0":
                    GridViewBind();
                    break;

                case "STATE":
                    StateGridGraph(sID);
                    callAjax(txt);
                  //  $("#divFilter").hide();
                    break;
                    //  break;

                case "CATEGORY":
                    CourseGridGraph(sID, cID);
                  //  $("#divFilter").hide();
                    callAjax(txt);
                    break;

                default:
                    //StateGridGraph();
                    //callAjax(txt);
                    break;

            }


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
        function validateDropDowns(sender, args) {
            var ddlFromMonth = document.getElementById('<%= ddlFromMonth.ClientID %>');
    var ddlToMonth = document.getElementById('<%= ddlToMonth.ClientID %>');
    var ddlFromYear = document.getElementById('<%= ddlFromYear.ClientID %>');
    var ddlToYear = document.getElementById('<%= ddlToYear.ClientID %>');
    var customValidator = document.getElementById('<%= customValidator.ClientID %>');
    var cmpToYear = document.getElementById('<%= cmpToYear.ClientID %>');

    // Comparison validation for dates
    var fromDate = new Date(parseInt(ddlFromYear.value), parseInt(ddlFromMonth.value), 1);
    var toDate = new Date(parseInt(ddlToYear.value), parseInt(ddlToMonth.value), 1);

    if (ddlFromMonth.value != "0" && ddlToMonth.value != "0" && ddlFromYear.value != "0" && ddlToYear.value != "0") {
        if (fromDate >= toDate) {
            customValidator.innerHTML = "Please check the dates.";
            customValidator.style.color = "Red";
            customValidator.style.display = "inline"; // Show the validation message
            args.IsValid = false; // Set validation to false
        } else {
            customValidator.style.display = "none"; // Hide the validation message
            args.IsValid = true; // Set validation to true
        }
    } else {
        customValidator.innerHTML = "Please select exam month or exam year";
        customValidator.style.color = "Red";
        customValidator.style.display = "inline"; // Show the validation message
        args.IsValid = false; // Set validation to false
    }
}


        function GridViewBind() {

            debugger;
            var isCheckedSC = document.getElementById("<%= chk_sc.ClientID %>").checked;
            var isCheckedST = document.getElementById("<%= chk_st.ClientID %>").checked;
            var isCheckedOBC = document.getElementById("<%= chk_OBC.ClientID %>").checked;
            var isCheckedPH = document.getElementById("<%= chk_PH.ClientID %>").checked;
            var genderFilter = document.getElementById("<%= ddlGender.ClientID %>").value;
            var fromMonthFilter = document.getElementById("<%= ddlFromMonth.ClientID%>").value;
            var toMonthFilter = document.getElementById("<%= ddlToMonth.ClientID %>").value;            
            var fromYearFilter = document.getElementById("<%= ddlFromYear.ClientID%>").value;
            var toYearFilter = document.getElementById("<%= ddlToYear.ClientID %>").value;

            var obj = '';

            var obj1 = {};
            obj1.pSC = isCheckedSC ? 'Y' : null;
            obj1.pST = isCheckedST ? 'Y' : null;
            obj1.pOBC = isCheckedOBC ? 'Y' : null;
            obj1.pPH = isCheckedPH ? 'Y' : null;
            //obj1.pG = genderFilter ? genderFilter : null;
            //obj1.pFY =  0 ? null : fromYearFilter;
            //obj1.pTY = 0 ? null : toYearFilter;
            obj1.pG = genderFilter === '0' ? null : genderFilter;
            obj1.pFM = fromMonthFilter === '0' ? null : fromMonthFilter;
            obj1.pTM = toMonthFilter === '0' ? null : toMonthFilter;
            obj1.pFY = fromYearFilter === '0' ? null : fromYearFilter;
            obj1.pTY = toYearFilter === '0' ? null : toYearFilter;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/RegisterCandParam",
                // data: "{}",
                data: JSON.stringify(obj1),
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


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/CourseCompleted",
                data: JSON.stringify(obj1),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChart1(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });

            function DreawChart(series) {
                var date = new Date();
                //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                //            "Aug", "Sep", "Oct", "Nov", "Dec"];
                //var valDate = date.getDate() - 1 + " " + months[date.getMonth()] + " " + date.getFullYear();

                debugger;
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
                        // text: 'NIELIT Students Registered '+  obj1.pSC

                        text  : 'NIELIT Students Registered'
                          

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
              //  var date = new Date();
                //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                  //          "Aug", "Sep", "Oct", "Nov", "Dec"];
                //var valDate = date.getDate() - 1 + " " + months[date.getMonth()] + " " + date.getFullYear();

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
                        text: 'NIELIT Students Certified '//Till ' + valDate + ''
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

            //var obj = {};
            //obj.pCentre = sID;
            debugger;
            var isCheckedSC = document.getElementById("<%=HdnfSc.ClientID%>").value;
            var isCheckedST = document.getElementById("<%=HdnfST.ClientID %>").value;
            var isCheckedOBC = document.getElementById("<%=HdnfOBC.ClientID %>").value;
            var isCheckedPH = document.getElementById("<%= HdnfPH.ClientID %>").value;
            var genderFilter = document.getElementById("<%= ddlGender.ClientID %>").value;
            var fromMonthFilter = document.getElementById("<%= ddlFromMonth.ClientID%>").value;
            var toMonthFilter = document.getElementById("<%= ddlToMonth.ClientID %>").value;
            var fromYearFilter = document.getElementById("<%= ddlFromYear.ClientID%>").value;
            var toYearFilter = document.getElementById("<%= ddlToYear.ClientID %>").value;

            var obj = '';

            var obj1 = {};
            obj1.pStateID = sID;
            obj1.pSC = isCheckedST ? 'Y' : (isCheckedSC === "" ? null : null);
            obj1.pST = isCheckedST ? 'Y' : null;
            obj1.pOBC = isCheckedOBC ? 'Y' : null;
            obj1.pPH = isCheckedPH ? 'Y' : null;
            obj1.pG = genderFilter === '0' ? null : genderFilter;
            obj1.pFY = fromYearFilter === '0' ? null : fromYearFilter;
            obj1.pTY = toYearFilter === '0' ? null : toYearFilter;
            obj1.pFM = fromMonthFilter === '0' ? null : fromMonthFilter;
            obj1.pTM = toMonthFilter === '0' ? null : toMonthFilter;



            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/StateWiseNSQFCounts",
                data: JSON.stringify(obj1),
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


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/StateWiseNSQFCountsCertified",
                data: JSON.stringify(obj1),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChartState1(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });



            function DreawChartState(series) {
                //var date = new Date();
                //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                //        "Aug", "Sep", "Oct", "Nov", "Dec"];
                //var valDate = date.getDate() - 1 + " " + months[date.getMonth()] + " " + date.getFullYear();

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
                        text: 'Candidates Registered Course Category-Wise '//Till ' + valDate + ''
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


            function DreawChartState1(series) {
                //var date = new Date();
                //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                //        "Aug", "Sep", "Oct", "Nov", "Dec"];
                //var valDate = date.getDate() - 1 + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").show();
                //$("#divRegGraph").hide();
                //$("#divCourseCertifiedGraph").hide();

                $('#div1').highcharts({
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
                        text: 'Candidates Certified Course Category-Wise'// Till  ' + valDate + ''
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
                        name: 'NIELIT Certified Candidates',
                        data: series
                    }]
                });
            }


        }
        function CourseGridGraph(sID, cID) {
            //var obj = {};
            //obj.pCentre = sID;
            //obj.pCategory = cID;

            debugger;
            var isCheckedSC = document.getElementById("<%=HdnfSc.ClientID%>").value;
            var isCheckedST = document.getElementById("<%=HdnfST.ClientID %>").value;
            var isCheckedOBC = document.getElementById("<%=HdnfOBC.ClientID %>").value;
            var isCheckedPH = document.getElementById("<%= HdnfPH.ClientID %>").value;
            var genderFilter = document.getElementById("<%= ddlGender.ClientID %>").value;
            var fromMonthFilter = document.getElementById("<%= ddlFromMonth.ClientID%>").value;
            var toMonthFilter = document.getElementById("<%= ddlToMonth.ClientID %>").value;
            var fromYearFilter = document.getElementById("<%= ddlFromYear.ClientID%>").value;
            var toYearFilter = document.getElementById("<%= ddlToYear.ClientID %>").value;

            var obj = '';

            var obj1 = {};
            obj1.pStateID = sID;
            obj1.pCourseCategoryID = cID;
            obj1.pSC = isCheckedST ? 'Y' : (isCheckedSC === "" ? null : null);
            obj1.pST = isCheckedST ? 'Y' : null;
            obj1.pOBC = isCheckedOBC ? 'Y' : null;
            obj1.pPH = isCheckedPH ? 'Y' : null;
            obj1.pG = genderFilter === '0' ? null : genderFilter;
            obj1.pFY = fromYearFilter === '0' ? null : fromYearFilter;
            obj1.pTY = toYearFilter === '0' ? null : toYearFilter;
            obj1.pFM = fromMonthFilter === '0' ? null : fromMonthFilter;
            obj1.pTM = toMonthFilter === '0' ? null : toMonthFilter;


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/getStateWiseCourseCategoryWiseNSQFCountsRegGrid",
                data: JSON.stringify(obj1),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChartCourse(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });



            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/Graph.asmx/getStateWiseCourseCategoryWiseNSQFCountsCertGrid",
                data: JSON.stringify(obj1),
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        data.push(serie);
                    }

                    DreawChartCourse1(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });


            function DreawChartCourse(series) {
                //var date = new Date();
                //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                //        "Aug", "Sep", "Oct", "Nov", "Dec"];
                //var valDate = date.getDate() - 1 + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").show();
                //$("#divRegGraph").hide();
                //$("#divCourseCertifiedGraph").hide();

                $('#divRegCourse').highcharts({
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
                        text: 'Candidates Registered Course-Wise'// Till ' + valDate + ''
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


            function DreawChartCourse1(series) {
                //var date = new Date();
                //var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul",
                //        "Aug", "Sep", "Oct", "Nov", "Dec"];
                //var valDate = date.getDate() - 1 + " " + months[date.getMonth()] + " " + date.getFullYear();

                //$("#divNSQFGraph").show();
                //$("#divRegGraph").hide();
                //$("#divCourseCertifiedGraph").hide();

                $('#divCertCourse').highcharts({
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
                        text: 'Candidates Certified Course-Wise'// Till ' + valDate + ''
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
                        name: 'NIELIT Certified Candidates',
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
                $("#div1").show();
                $("#divRegGraph").hide();
                $("#divCourseCertifiedGraph").hide();
                $("#divRegCourse").hide();
                $("#divCertCourse").hide();

            }


            if (condition === 'CATEGORY') {

                $("#divRegCourse").show();
                $("#divCertCourse").show();
                $("#divStateGraph").hide();
                $("#div1").hide();
                $("#divRegGraph").hide();
                $("#divCourseCertifiedGraph").hide();

            }


        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--   <div class="col-sm-1">
                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-info" Text="Home" OnClick="Button1_Click"  />

                        </div>--%>
    <asp:HiddenField ID="HdnfSc" runat="server" />
    <asp:HiddenField ID="HdnfST" runat="server" />
    <asp:HiddenField ID="HdnfOBC" runat="server" />
    <asp:HiddenField ID="HdnfPH" runat="server" />
    <%--<asp:HiddenField ID="currenttxtValue" runat="server" />--%>
    <div class="container">

         <%-- <h1 class="dashboardheader">Select Filters</h1>--%>
        <div class="row">
        
         <div class="form-group" id ="divFilter" >  

              <div id="updatpanel1">
           <%-- <div style="overflow: auto; height: 800px;">
                <center>--%>
                  <div style="text-align: center;">
                       <asp:Image ID="Image1"  runat="server" ImageUrl="~/images/NIELIT.jpg" Height="100px" Width="370px" />
                  </div>
                 
                  <div class="container">
    <div class="row">
        <div class="col-md-14"> 
            <%--<div  class="text-center bg-success text-white p-3 rounded" > 
                 <strong>Caste Category and Specially Abled wise Breakup</strong>
            </div>--%>
           
        </div>
        
    </div>


                    <div id="tblCat" visible="true" class="row" runat="server">

                    <table class="table table-bordered table-responsive">
                    <tbody>
                        <tr align="center" class="success">
                            <%--<th style="text-align:center;" class="auto-style1"></th>--%>
                            <th style="text-align:center;" class="auto-style1">SC</th>
                            <th style="text-align:center;" class="auto-style1">ST</th>
                            <th style="text-align:center;" class="auto-style1">OBC</th>
                            <th style="text-align:center;" class="auto-style1">SPECIALLY ABLED</th>
                         </tr>
                        <tr align="center" class="success">
                           <%--<td>
                               Caste Category
                           </td>--%>
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
                        </tr>

                    </tbody>
                    </table>
                    </div>
                    <%--</div>--%>
                  </div>




         
            <label for="ddlGender"  class="success">Gender: </label>
            <asp:DropDownList ID="ddlGender" runat="server" Width="100px"  class="success">
                 <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                <asp:ListItem Text="Female" Value="2"></asp:ListItem>
                <asp:ListItem Text="Transgender" Value="3"></asp:ListItem>
               
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <label for="ddlFromYear">From :</label>

             <asp:DropDownList ID="ddlFromMonth" runat="server" Height="20px" Width="110px" >
              <asp:ListItem Text="--Select Month--" Value="0"></asp:ListItem>
                 <asp:ListItem Text="January" Value="1"></asp:ListItem>
                 <asp:ListItem Text="February" Value="2"></asp:ListItem>
                 <asp:ListItem Text="March" Value="3"></asp:ListItem>
                 <asp:ListItem Text="April" Value="4"></asp:ListItem>
                 <asp:ListItem Text="May" Value="5"></asp:ListItem>
                 <asp:ListItem Text="June" Value="6"></asp:ListItem>
                 <asp:ListItem Text="July" Value="7"></asp:ListItem>
                  <asp:ListItem Text="August" Value="8"></asp:ListItem>
                  <asp:ListItem Text="September" Value="9"></asp:ListItem>
                  <asp:ListItem Text="October" Value="10"></asp:ListItem>
                  <asp:ListItem Text="November" Value="11"></asp:ListItem>
                  <asp:ListItem Text="December" Value="12"></asp:ListItem>
            </asp:DropDownList>
            
            <asp:DropDownList ID="ddlFromYear" runat="server" Height="20px" Width="110px" >
              <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <label for="ddlToYear">To :</label>
             <asp:DropDownList ID="ddlToMonth" runat="server" Height="20px" Width="110px" >
              <asp:ListItem Text="--Select Month--" Value="0"></asp:ListItem>
               <asp:ListItem Text="January" Value="1"></asp:ListItem>
                 <asp:ListItem Text="February" Value="2"></asp:ListItem>
                 <asp:ListItem Text="March" Value="3"></asp:ListItem>
                 <asp:ListItem Text="April" Value="4"></asp:ListItem>
                 <asp:ListItem Text="May" Value="5"></asp:ListItem>
                 <asp:ListItem Text="June" Value="6"></asp:ListItem>
                 <asp:ListItem Text="July" Value="7"></asp:ListItem>
                  <asp:ListItem Text="August" Value="8"></asp:ListItem>
                  <asp:ListItem Text="September" Value="9"></asp:ListItem>
                  <asp:ListItem Text="October" Value="10"></asp:ListItem>
                  <asp:ListItem Text="November" Value="11"></asp:ListItem>
                  <asp:ListItem Text="December" Value="12"></asp:ListItem>


            </asp:DropDownList>
             
            <asp:DropDownList ID="ddlToYear" runat="server" Width="110px" Height="20px">
               <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>  
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               
            <asp:Button ID="btnApplyFilters" runat="server" Text="Apply Filters" Width="140px" OnClick="btnApplyFilters_Click" ValidationGroup="YearValidation" CssClass="btn btn-info" />
                <asp:CompareValidator ID="cmpToYear" runat="server"
                   ControlToValidate="ddlToYear"
                   ControlToCompare="ddlFromYear"
                   Operator="GreaterThanEqual"
                   OperatorDefaultValue="0"
                   Type="Integer"
                   InitialValue="0"
                   ErrorMessage=""
                   Display="Dynamic"
                   ValidationGroup="YearValidation"
                   CssClass="error-message">
               </asp:CompareValidator>
                  <asp:CustomValidator ID="customValidator" runat="server" ErrorMessage="Please select exam month or exam year" ValidationGroup="YearValidation" ClientValidationFunction="validateDropDowns"></asp:CustomValidator>
              

        </div>
            </div>

        <div class="row">

            <div id="divRegGraph" class="col-md-6">
            </div>

            <div id="divCourseCertifiedGraph" class="col-md-6">
            </div>

            <div id="divStateGraph" style="display: none" class="col-md-6">
            </div>

            <div id="div1" style="display: none" class="col-md-6">
            </div>

            <div id="divRegCourse" style="display: none" class="col-md-6">
            </div>


            <div id="divCertCourse" style="display: none" class="col-md-6">
            </div>

        </div>


      
                    <div class="row">
                        <div class="col-sm-1">
                            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-info" Text="Back" OnClick="btnBack_Click" />

                        </div>

                        <div class="col-sm-10 col-sm-10 ">

                             
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                                    BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="id" CellSpacing="2" ForeColor="#003153" OnRowCreated="GridView1_RowCreated"
                                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" AllowPaging="True" PageSize="45" >
                                    <Columns>
                                     <%--   <asp:TemplateField HeaderText="S. No." ItemStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server"                                                                                                                                                    
                                                     />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>--%>
                                          <asp:BoundField DataField="S. No." HeaderText="S No." ItemStyle-Width="150" />
                                        <asp:BoundField DataField="Name" HeaderText="State Name" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="Registered" HeaderText="Students Registered" ItemStyle-Width="150" />
                                         <asp:TemplateField  ItemStyle-Width="100" Visible="false"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblSC" Text='<%# Eval("RegSC") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadSC" Text="SC"   runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-Width="100" Visible="false"    >
                                            <ItemTemplate>
                                                <asp:Label ID="lblST" Text='<%# Eval("RegST") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadST" Text="ST"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-Width="100" Visible="false"    >
                                            <ItemTemplate>
                                                <asp:Label ID="lblOBC" Text='<%# Eval("RegOBC") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadOBC" Text="OBC"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-Width="100"  Visible="false"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblPH" Text='<%# Eval("RegPH") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadPH" Text="PH"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <%--<asp:BoundField DataField="RegSC" Visible="false" HeaderText="SC" ItemStyle-Width="150" />--%>
                                        <%--<asp:BoundField DataField="RegST"  HeaderText="ST" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="RegOBC"  HeaderText="OBC" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="RegPH"  HeaderText="PH" ItemStyle-Width="150" />--%>

                                        <asp:BoundField DataField="CourseCompleted" Visible="false" HeaderText="Course Completed" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="CertificateIssue"  HeaderText="Students Certified" ItemStyle-Width="150" />
                                        
                                        <asp:TemplateField  ItemStyle-Width="100"  Visible="false"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertSC" Text='<%# Eval("CertSC") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadCertSC" Text="SC"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-Width="100"  Visible="false"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertST" Text='<%# Eval("CertST") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadCertST" Text="ST"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-Width="100"  Visible="false"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertOBC" Text='<%# Eval("CertOBC") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadCertOBC" Text="OBC"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-Width="100"  Visible="false"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblCertPH" Text='<%# Eval("CertPH") %>'   runat="server" />
                                            </ItemTemplate>
                                             <HeaderTemplate >
                                                  <asp:Label ID="lblheadCertPH" Text="PH"  runat="server" />
                                             </HeaderTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>

                                        <%--<asp:BoundField DataField="CertSC" Visible="false" HeaderText="SC" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="CertST" Visible="false" HeaderText="ST" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="CertOBC" Visible="false" HeaderText="OBC" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="CertPH" Visible="false" HeaderText="PH" ItemStyle-Width="150" />--%>

                                        <%--<asp:BoundField DataField="SCRegistered" HeaderText="SC Registered" ItemStyle-Width="150" />.
                                        <asp:BoundField DataField="SCCourseCompleted" HeaderText="SC CourseCompleted" ItemStyle-Width="150" />
                                        <asp:BoundField DataField="STRegistered" HeaderText="ST Registered" ItemStyle-Width="150" />--%>
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
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"  DataKeyNames="Course_Category_id" HeaderStyle-Font-Size="14px"
                                    BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellSpacing="2" ForeColor="#003153"
                                    CellPadding="4" width="100%" 
                    PageSize="100" GridLines="Both"  OnSelectedIndexChanged="GridView3_SelectedIndexChanged" OnRowCreated="GridView3_RowCreated" OnRowDataBound="OnRowDataBound1">
                    <Columns>
      <%--  <asp:TemplateField HeaderText="S No." ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Width="100px"></ItemStyle>
        </asp:TemplateField>--%>
                              <asp:BoundField DataField="S. No." HeaderText="S No." ItemStyle-Width="150" />

        <asp:BoundField DataField="Course_Category_Name" HeaderText="Course Category Name" ItemStyle-Width="150">
            <ItemStyle Width="150px"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Registered_Total" HeaderText="Registered Count" ItemStyle-Width="150">
            <ItemStyle Width="150px"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Certified_Total" HeaderText="Certified Count" ItemStyle-Width="150">
            <ItemStyle Width="150px"></ItemStyle>
        </asp:BoundField>
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
            </div>
            <div class="col-sm-1 col-sm-1"></div>            
        </div>

                    <div class="row">
            <div class="col-sm-1 col-sm-1 "></div>

            <div class="col-sm-10 col-sm-10 ">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="14px"
                  width="100%" 
                    ForeColor="Black"
                    PageSize="1000" GridLines="Both" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px">
                    <Columns>
   <%--     <asp:TemplateField HeaderText="S No." ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Width="100px"></ItemStyle>
        </asp:TemplateField>--%>
                              <asp:BoundField DataField="S. No." HeaderText="S No." ItemStyle-Width="150" />

        <asp:BoundField DataField="Course_Name" HeaderText="Course Name" ItemStyle-Width="150">
            <ItemStyle Width="150px"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Registered_Total" HeaderText="Registered Count" ItemStyle-Width="150">
            <ItemStyle Width="150px"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Certified_Total" HeaderText="Certified Count" ItemStyle-Width="150">
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
    <input type="hidden" id="hdnSelected" name="hdnSelected" runat="server" />
    <input type="hidden" id="hdnStateID" name="hdnSelected" runat="server" />
    <input type="hidden" id="hndCourseID" name="hdnSelected" runat="server" enableviewstate="true" />
</asp:Content>

