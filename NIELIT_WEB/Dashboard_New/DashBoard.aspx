<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard1_HomePage" %>

<%--<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style type="text/css">
        /* INSET STYLE */
        .box-icon1 {
            background: linear-gradient(to bottom, #5b912a 0%,#466f20 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon2 {
            background: linear-gradient(to bottom, #862a91 0%,#67206f 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon3 {
            background: #F36D21;
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon4 {
            background: linear-gradient(to bottom, #c67e30 0%,#986125 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon5 {
            background: linear-gradient(to bottom, #912a2a 0%,#712121 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon6 {
            background: linear-gradient(to bottom, #5b912a 0%,#466f20 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon7 {
            background: linear-gradient(to bottom, #1b973e 0%,#157530 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .box-icon8 {
            background: linear-gradient(to bottom, #ea2c2b 0%,#b52221 100%);
            border-radius: 10px;
            box-sizing: border-box;
        }

        .text-left {
            text-align: left;
            padding: 10px;
        }

        .text-right {
            text-align: right;
        }

        .icon-capt {
            background: #393a3c none repeat scroll 0 0;
            color: #ffffff;
            padding: 5px;
        }
    </style>

    <script type="text/javascript" src="engine1/wowslider.js"></script>
    <script type="text/javascript" src="engine1/script.js"></script>

    <script type="text/javascript" src="Scripts/js/jquery.min.js"></script>
    <script type="text/javascript">
        if (document.layers) {
            //Capture the MouseDown event.
            document.captureEvents(Event.MOUSEDOWN);

            //Disable the OnMouseDown event handler.
            $(document).mousedown(function () {
                return false;
            });
        }
        else {
            //Disable the OnMouseUp event handler.
            $(document).mouseup(function (e) {
                if (e != null && e.type == "mouseup") {
                    //Check the Mouse Button which is clicked.
                    if (e.which == 2 || e.which == 3) {
                        //If the Button is middle or right then disable.
                        return false;
                    }
                }
            });
        }


    </script>
    <script type="text/javascript">
        function confirm_alert(node) {
            return confirm("This is external link, Are you sure you want to continue?.");
        }

    </script>

    <style type="text/css">
        #boxes img {
            display: none;
        }

        #boxes .active {
            display: block !important;
        }

        nivo-slider.img {
            height: 450px;
            width: 490px; /*maintain aspect ratio*/
            max-width: 500px;
        }

        #content-wrapper .page-desc p {
            text-align: left;
        }

        table.fixed {
            table-layout: fixed;
        }

            table.fixed td {
                overflow: hidden;
            }

        .top-image {
            margin-bottom: 50px;
        }

        .ulc,
        .lic {
        }

        .btnc {
            border-radius: 8px;
        }

        .tales {
            width: 100%;
        }

        .carousel-inner {
            width: 100%;
            max-height: 200px !important;
        }

        .button {
            background-color: #3B3C36; /* Green */
            border: none;
            color: white;
            padding: 5px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 26px;
            margin: 4px 4px;
            cursor: pointer;
        }

        .icon-capt {
            background: #393a3c none repeat scroll 0 0;
            color: #ffffff;
            padding: 10px;
        }

        .text-right {
            text-align: right;
        }

        .button:hover {
            /*background-color: #FD6C9E;*/ /* Green */
            color: #256D7B;
        }
        /*.socialLink {
            position: fixed;
            width: 38px;
            right: 0;
            top: 35%;
            z-index: 1000;
        }*/
    </style>
    <script type="text/javascript" src="Scripts/js/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#boxes img:first").addClass('active');
            setInterval(function () {
                InitializeSlider();
            }, 3000);

            function InitializeSlider() {
                if ($("#boxes img").hasClass('active')) {
                    var nextImg;
                    if (($("#boxes .active").index() + 1) == $("#boxes img").length) {
                        nextImg = $("#boxes img:first");
                    } else {
                        nextImg = $("#boxes .active").next();
                    }
                    $("#boxes .active").hide("slow").removeClass("active");
                    nextImg.addClass('active');
                } else {
                    $("#boxes img:first").addClass('active');
                }
            }
        });
    </script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="Styles/nivo-slider.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="Styles/nivo-slider.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/Demo.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <script type="text/javascript">
        $(window).load(function () {
            $('#nivo-slider').nivoSlider();
        });         
    </script>
    <script type="text/javascript">
        function ValidateRange() {
            document.forms[0].target = "_blank";
            return true;
        }
    </script>
    
    <div class="container count-container">

        <div class="row" >
            
            <div class="col-xs-12 col-sm-12 training-section-right">
                <div class="row" style="margin: 10px;" >
                    <div class="col-md-3" style="margin-bottom: 10px;">
                        <div class="trainboxinner box-icon1">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="lblNielitPresence" runat="server" ForeColor="White" Font-Size="Large" Text="NIELIT's Presence<br/><font size=2> (Centres / Extension / Study Centres)</font>"></asp:Label>
                                <%--<asp:Label ID="lblCentre" runat="server" ForeColor="White" Font-Size="Small" Text=" (Centres / Extension / Study Centres)</font>"></asp:Label>--%>
                                <br />
                                <br />
                                <span class="badge badge-inverse" ">
                                <asp:Label ID="lblNielitPresenceCount" Font-Size ="Medium"  runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label></span>
                            </div>
                            <div class="redirect-btn text-right clearfix">
                                <%--<asp:Button ID="btnNielitPresence" class=" button btnc text-right" runat="server" OnClick="btnNielitPresence_Click" Text=">" />--%>
                                <asp:ImageButton ID="imgNielitPresence" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnNielitPresence_Click"/>
                            </div>
                            <%--<div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>--%>
                        </div>

                    </div>
                    <div class="col-md-3" style="margin-bottom: 10px;">
                        <div class="trainboxinner box-icon2">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="lblTraining" runat="server" ForeColor="White" Font-Size="Large" Text="Training Partners <br/><font size=2>(Centres/ Ext. Centres / Study Centres / Accredited Centres)</font>"></asp:Label>
                                <br /> 
                                <span class="badge badge-inverse"">
                                <asp:Label ID="lblTrainingCount" Font-Size ="Medium" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </span>
                            </div>
                            <div class="redirect-btn text-right clearfix">
                                <asp:ImageButton ID="imgTraining" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnTraining_Click"/>
                                <%--<asp:Button ID="btnTraining" class=" button btnc text-right" runat="server" OnClick="btnTraining_Click" Text=">" />--%>
                            </div>
                            <%--<div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>--%>
                        </div>
                    </div>
                    <div class="col-md-3" style="margin-bottom: 10px;">
                        <div class="trainboxinner box-icon3">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="lblITSkilling" runat="server" ForeColor="White" Font-Size="Large" Text="Skilled Candidates <br/><font size=2> (DLC/OABC/Short Term/Non NSQF)</font>"></asp:Label><br /><br />
                                <span class="badge badge-inverse"">
                                <asp:Label ID="lblITSkillingCount" runat="server" Text="" Font-Bold="True" Font-Size ="Medium" ForeColor="White"></asp:Label>
                                    </span>
                            </div>
                            <div class="redirect-btn text-right clearfix">
                                <asp:ImageButton ID="imgITSkilling" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnITSkilling_Click"/>
                                <%--<asp:Button ID="btnITSkilling" class=" button btnc text-right"   runat="server" OnClick="btnITSkilling_Click" Text=">" />--%>
                                
                            </div>
                           <%-- <div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>--%>
                        </div>
                    </div>
                    <div class="col-md-3" style="margin-bottom: 10px;">
                        <div class="trainboxinner box-icon4">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="lblNSQFCourses" runat="server" ForeColor="White" Font-Size="Large" Text="NSQF Courses<br/><font size=2> (Candidates Applied)</font>"></asp:Label>
                                <br />
                                <br />
                                <span class="badge badge-inverse"">
                                <asp:Label ID="lblNSQFCoursesCount"  Font-Size ="Large" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </span>
                            </div>
                            <div class="redirect-btn text-right clearfix">
                                <%--<asp:Button ID="btnlblNSQFCourses" class=" button btnc text-right" runat="server" OnClick="btnlblNSQFCourses_Click" Text=">" />--%>
                                
                                <asp:ImageButton ID="imgNSQFCourses" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnlblNSQFCourses_Click"/>
                                
                                
                            </div>
                            <%--<div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>--%>
                        </div>

                    </div>
                </div>
                <%--<div class="col-md-3" style="margin-bottom: 10px; top: 0px; left: 0px;">
                        <div class="trainboxinner box-icon5">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="Label1" runat="server" ForeColor="White" Font-Size="Large" Text="Courses<br/><font size=2> (Candidates Registered)</font>"></asp:Label>
                                <br />
                                <br />
                                <span class="badge badge-inverse"">
                                <asp:Label ID="Label2"  Font-Size ="Large" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </span>
                            </div>
                            <div class="redirect-btn text-right clearfix">                                                                
                                <asp:ImageButton ID="btnCandListYear" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnCandListYear_Click"/>                                                                
                            </div>                            
                        </div>
                    </div>--%>
                    
                    <div class="col-md-3" style="margin-bottom: 10px; visibility :hidden;">
                        <div class="trainboxinner box-icon5">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="lblPlacements" runat="server" ForeColor="White" Font-Size="Large" Text="Placements"></asp:Label>
                                <div class="row"></div>
                                 <div class="row"><div class="col-md-3" style="margin-bottom: 10px;">

                                                  </div> 

                                 </div>
                                 <div class="row">
                                      <div class="col-md-3" style="margin-bottom: 10px;">
                                <span class="badge badge-inverse"">
                                <asp:Label ID="lblPlacementsCount" Font-Size ="Large" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </span>
                                          </div>
                                
                            </div>
                                </div>
                            <div class="redirect-btn text-right clearfix">
                                <%--<asp:Button ID="btnlblPlacements" class=" button btnc text-right" runat="server" OnClick="btnlblPlacements_Click" Text=">" />--%>
                                <asp:ImageButton ID="imgBtnPlacements" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnlblPlacements_Click" />
                            </div>
                            <%--<div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>--%>
                        </div>

                    </div>
                    <div class="col-md-3" style="margin-bottom: 10px;visibility :hidden;">
                        <div class="trainboxinner box-icon6">
                            <div class="icon-container text-left clearfix ">
                                <asp:Label ID="lblDigiLocker" runat="server" ForeColor="White" Font-Size="Large" Text="Digi Locker<br/><font size=2> (NIELIT Certificates Linked)</font>"></asp:Label>
                                <div class="row"></div>
                                 <div class="row"></div>
                                <span class="badge badge-inverse"">
                                <asp:Label ID="lblDigiLockerCount" runat="server" Font-Size ="Large" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </span>
                            </div>
                            <div class="redirect-btn text-right clearfix">
                                <%--<asp:Button ID="btnDigiLocker" class=" button btnc text-right" runat="server" OnClick="btnDigiLocker_Click" Text=">" />--%>
                                 <asp:ImageButton ID="imgBtnDigiLocker" runat ="server" ImageUrl ="~/Images/download1.jpeg" OnClick="btnDigiLocker_Click" />
                            </div>
                            <%--<div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>--%>
                        </div>

                    </div>
                   
                    </div>
                <%--<div class="row" style="margin: 5px">
                            <div class="col-xs-4">
                                <div class="trainboxinner box-icon3">
                                    <div class="icon-container text-left clearfix ">
                                        
                                        <asp:Label ID="Label11" runat="server" ForeColor="White" Font-Size="Large" Text="Nielit Centers"></asp:Label>
                                        <asp:Label ID="Label12" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </div>
                                    <div class="redirect-btn text-right clearfix">
                                        <button id="Button12" class=" button btnc text-right" onserverclick="btnnielitregionalcenter_Click">></button>
                                        </div>
                                    <div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>
                                </div>

                            </div>
                            <div class="col-xs-4">
                                <div class="trainboxinner box-icon3">
                                    <div class="icon-container text-left clearfix ">
                                        <asp:Label ID="Label13" runat="server" ForeColor="White" Font-Size="Large" Text="Nielit Centers"></asp:Label>
                                        <asp:Label ID="Label14" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </div>
                                    <div class="redirect-btn text-right clearfix">
                                        <button id="Button13" class=" button btnc text-right" onserverclick="btnnielitregionalcenter_Click">></button>
                                        </div>
                                    <div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>
                                </div>

                            </div>
                            <div class="col-xs-4">
                                <div class="trainboxinner box-icon3">
                                    <div class="icon-container text-left clearfix ">
                                        <asp:Label ID="Label15" runat="server" ForeColor="White" Font-Size="Large" Text="Nielit Centers"></asp:Label>
                                        <asp:Label ID="Label16" runat="server" Text="" Font-Bold="True" ForeColor="White"></asp:Label>
                                    </div>
                                    <div class="redirect-btn text-right clearfix">
                                        <button id="Button14" class=" button btnc text-right" onserverclick="btnnielitregionalcenter_Click">></button>
                                        </div>
                                    <div class="icon-capt">Create channel for physical delivery of the digital literacy training</div>
                                </div>
                            </div>
                        </div>--%>

                <%--
                        <div class="row" style="padding: 15px;">
                            <div class="col-md-5" style="background-color: #A52A2A; border-radius: 5px; margin: 5px">
                                <asp:Label ID="lbnc1" runat="server" ForeColor="White" Font-Size="Large" Text="Nielit Centers"></asp:Label>
                                <br />

                                <asp:Label ID="lbncData1" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                                <br />

                                <button id="Button4" class="button btnc text-right" style="float: right" runat="server" onserverclick="btnnielitregionalcenter_Click">></button>
                                <br />
                                <br />
                                <br />
                                <div class="icon-capt">Create channel for physical delivery of the digital literacy training </div>
                               </div>
                            <div class="col-md-5" style="background-color: #228B22; border-radius: 5px; margin: 10px">

                                <asp:Label ID="lbscst" runat="server" ForeColor="White" Font-Size="Large" Text="SC / ST Candidates"></asp:Label>

                                <br />

                                <asp:Label ID="lbscstdata" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                                <br />

                                <button id="Button5" class="button btnc" style="float: right" runat="server" onserverclick="Button1_Click">> </button>
                                <br />
                                <br />
                                <br />
                                <div class="icon-capt">Monitor physical delivery of digital literacy program </div>

                                </div>
                        </div>
                        <div class="row" style="padding: 15px;">

                            <div class="col-md-5" style="background-color: #CC7722; border-radius: 5px; margin: 5px">
                                <asp:Label ID="lbl1" runat="server" ForeColor="White" Font-Size="Large" Text="Training Partners">                                
                                </asp:Label>
                                <br />

                                <asp:Label ID="lblTrPtn" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                                <br />

                                <button id="btntrainingparteners" style="float: right" class="button btnc" runat="server" onserverclick="btntrainingparteners_Click">></button>
                                <br />
                                <br />
                                <br />
                                <div class="container my-auto" style="background-color: black; text-align: justify; width: 100%; color: white">Create channel for physical delivery of the digital literacy training </div>

                                </div>
                            <div class="col-md-5" style="background-color: #FD6C9E; border-radius: 5px; margin: 5px">
                                <asp:Label ID="lbl2" runat="server" ForeColor="White" Font-Size="Large" Text="Registered Candidates"></asp:Label>

                                <br />

                                <asp:Label ID="lblRegCan" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                                <br />

                                <button id="Button1" class="button btnc" style="float: right" runat="server" onserverclick="Button1_Click">> </button>
                                <br />
                                <br />
                                <br />
                                <div class="container my-auto" style="background-color: black; text-align: justify; width: 100%; color: white">Monitor physical delivery of digital literacy program </div>

                                </div>

                        </div>
                        <div class="row" style="padding: 15px;">
                            <div class="col-md-5" style="background-color: #40826D; border-radius: 5px; margin: 10px">
                                <asp:Label ID="lbl3" runat="server" ForeColor="White" Font-Size="Large" Text="Training Completed"></asp:Label>

                                <br />

                                <asp:Label ID="lblTrCom" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                                <br />

                                <button id="Button2" class="button btnc" style="float: right" runat="server" onserverclick="Button2_Click">> </button>
                                <br />
                                <br />
                                <br />
                                <div class="container my-auto" style="background-color: black; text-align: justify; width: 100%; color: white">Ensure physical delivery of the digital literacy training </div>

                                </div>
                            <div class="col-md-5" style="background-color: #007FFF; border-radius: 5px; margin: 10px">
                                <asp:Label ID="lbl4" runat="server" ForeColor="White" Font-Size="Large" Text="Certified Students"></asp:Label>

                                <br />

                                <asp:Label ID="lblCerStu" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                                <br />

                                <button id="Button3" class="button btnc" style="float: right" runat="server" onserverclick="Button3_Click">> </button>
                                <br />
                                <br />
                                <br />
                                <div class="container my-auto" style="background-color: black; text-align: justify; width: 100%; color: white">
                                    Beneficiary of the digital
                        <br />
                                    literacy training
                                </div>

                                </div>
                        </div>
                --%>
            </div>


        </div>
</asp:Content>

