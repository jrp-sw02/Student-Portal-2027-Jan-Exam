<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Title="Student Information and Enrollment System, NIELIT"
    Inherits="Index" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Home</title>
    <link rel="icon"  href="~/favicon.ico" />
    <%-- <link rel="stylesheet" href="/css/news/thumbnailviewer.css" type="text/c ss" />
    <script src="/js/news/thumbnailviewer.js" type="text/
        javascript">
    </script>  
    <script src="Script/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="Script/jquery.dd.js" type="text/javascript"></script>--%>
    <script type="text/javascript" language="JavaScript">   

        document.onkeydown = function (e) {
            if (event.keyCode == 123) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
                return false;
            } 
            if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
                return false;
            }
        }

        if (top.location != self.location) {
            top.location = self.location.href
        }

        function checktop() {
            startTime();
            if (window.top.location.href.toLowerCase().indexOf("mainpage.aspx") >= 0) // this doesn't work, any suggestions.
            {
                window.top.location.href = "index.aspx";
            }
        }

    </script>

    <%--    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-39855250-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>--%>

    <script src="Script/GlobalFunction.js" type="text/javascript"></script>
    <style type="text/css">
        a:link {
            color: #F26100;
            text-decoration: none;
        }

        a:visited {
            color: #F26100;
        }

        a:hover {
            color: #ff0000;
            text-decoration: underline;
            font-weight: bold;
        }

        a:active {
            color: #F26100;
        }

        .auto-style1 {
            cursor: pointer;
            height: 19px;
        }

        .auto-style2 {
            cursor: hand;
            height: 19px;
        }

        /*added by amit SHINE NOTIFICATION for notification bar*/

        #dragbar {
            position: relative;
            overflow: hidden;
        }

            #dragbar::after {
                content: '';
                position: absolute;
                top: -40px;
                left: -120px;
                width: 60px;
                height: 150px;
                background: linear-gradient( to right, rgba(255,255,255,0), rgba(255,255,255,.15), rgba(255,255,255,.6), rgba(255,255,255,.15), rgba(255,255,255,0) );
                transform: rotate(25deg);
                animation: notifShine 5s linear infinite;
                pointer-events: none;
            }

        @keyframes notifShine {
            from {
                left: -120px;
            }

            to {
                left: 100%;
            }
        }


        /*added by amit SHINE NOTIFICATION for notification bar*/
    </style>
    <script type="text/javascript">
        /******************************************
        * Popup Box- By Jim Silver @ jimsilver47@yahoo.com
        * Visit http://www.dynamicdrive.com/ for full source code
        * This notice must stay intact for use
        ******************************************/

        var ns4 = document.layers
        var ie4 = document.all
        var ns6 = document.getElementById && !document.all

        //drag drop function for NS 4////
        /////////////////////////////////

        var dragswitch = 0
        var nsx
        var nsy
        var nstemp

        function drag_dropns(name) {
            if (!ns4)
                return
            temp = eval(name)
            temp.captureEvents(Event.MOUSEDOWN | Event.MOUSEUP)
            temp.onmousedown = gons
            temp.onmousemove = dragns
            temp.onmouseup = stopns
        }


        function gons(e) {
            temp.captureEvents(Event.MOUSEMOVE)
            nsx = e.x
            nsy = e.y
        }

        function dragns(e) {
            if (dragswitch == 1) {
                temp.moveBy(e.x - nsx, e.y - nsy)
                return false
            }
        }


        function stopns() {
            temp.releaseEvents(Event.MOUSEMOVE)
        }


        //drag drop function for ie4+ and NS6////
        /////////////////////////////////

        function drag_drop(e) {
            if (ie4 && dragapproved) {
                crossobj.style.left = tempx + event.clientX - offsetx
                crossobj.style.top = tempy + event.clientY - offsety
                return false
            }
            else if (ns6 && dragapproved) {
                crossobj.style.left = tempx + e.clientX - offsetx + "px"
                crossobj.style.top = tempy + e.clientY - offsety + "px"
                return false
            }
        }

        function initializedrag(e) {
            crossobj = ns6 ? document.getElementById("showimage") : document.all.showimage
            var firedobj = ns6 ? e.target : event.srcElement
            var topelement = ns6 ? "html" : document.compatMode != "BackCompat" ? "documentElement" : "body"
            while (firedobj.tagName != topelement.toUpperCase() && firedobj.id != "dragbar") {
                firedobj = ns6 ? firedobj.parentNode : firedobj.parentElement
            }

            if (firedobj.id == "dragbar") {
                offsetx = ie4 ? event.clientX : e.clientX
                offsety = ie4 ? event.clientY : e.clientY

                tempx = parseInt(crossobj.style.left)
                tempy = parseInt(crossobj.style.top)

                dragapproved = true
                document.onmousemove = drag_drop
            }
        }

        document.onmouseup = new Function("dragapproved=false")

        ////drag drop functions end here//////

        function hidebox() {
            crossobj = ns6 ? document.getElementById("showimage") : document.all.showimage
            if (ie4 || ns6)
                crossobj.style.visibility = "hidden"
            else if (ns4)
                document.showimage.visibility = "hide"
        }

        function startTime() {
            var today = new Date();
            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var month = monthNames[today.getMonth()];
            var Day = today.getDate();
            var Year = today.getFullYear();
            var time = today.toLocaleString('en-IN', { hour: 'numeric', minute: 'numeric', second: 'numeric', hour12: true });
            document.getElementById('txt').innerHTML = month + " " + Day + ", " + Year + "; " + time;
            var t = setTimeout(startTime, 500);
        }




    </script>
</head>

<body onload="checktop();" oncontextmenu="return false;">
    <div id="showimage" style="position: absolute; width: 250px; left: 400px; top: 100px; z-index: 1;">
      <table border="0"
            width="500"
            cellspacing="0"
            cellpadding="2"
            style="background: #FFD400; border: 2px solid #FFD400; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 6px rgba(0,0,0,.15);">
            <tr>
                <td width="100%">
                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                       <tr>
                     
                            <td id="dragbar"
                                width="100%"
                                onmousedown="initializedrag(event)"
                                style="background: #FFD400; color: #111; text-align: center; font-family: Verdana,Arial,sans-serif; font-weight: bold; padding: 8px; border-bottom: 1px solid #e6c000; cursor: move;">Important Notification
                            </td>
                         
                            <td style="cursor: hand">
                                <a href="#" onclick="hidebox();return false">
                                    <img alt="close" src="images/cancel.gif" width="16px" height="14px"
                                        border="0" /></a>
                            </td>
                        </tr>
                        
                           <td colspan="2">
                                <iframe name="ifImportant" onload="autoResizeF(this);" id="Iframe1" src="./ImportantNotificationnew.aspx"
                                    width="100%" frameborder='0' marginheight='0' marginwidth='0' scrolling="no"></iframe>
                            </td>
                          </table>
                         </td>
                        </tr>

                 </table>
    </div>
    <form id="form1" runat="server">
        <table  border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
            <tr>
                <td align="center" valign="top">
                    <table width="1000" border="0" cellspacing="0" cellpadding="0" align="center">
                        <tr>
                            <td valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="68px" colspan="4">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="center" valign="top" class="logo"></td>
                                                    <td align="center" class="heading">
                                                        <div id="tdHeaderBig" runat="server">
                                                        </div>
                                                        <div id="tdHeaderSmall" runat="server" class="heading_small">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" height="32px" class="menu" valign="middle">
                                            <label id="txt" style="color: White; text-align: left; margin-left: 5px;">
                                            </label>
                                        </td>
                                        <td colspan="3" align="right" height="32px" class="menu">
                                            <a href="https://www.facebook.com/NIELITIndia/" onclick="return confirm('This link will take you to an external web site.')"
                                                target="_blank">
                                                <img alt="social" src="https://student.nielit.gov.in/images/face.png" style="width: 27px; height: 27px;" /></a>&nbsp; <a href="https://twitter.com/NIELITIndia" onclick="return confirm('This link will take you to an external web site.')"
                                                    target="_blank">
                                                    <img alt="social" src="https://student.nielit.gov.in/images/tw.png" style="width: 25px; height: 25px;" /></a>&nbsp; <a href="Dashboard/RepositoryMaster.aspx" target="_blank">
                                                        <img alt="Dashboard" src="https://student.nielit.gov.in/images/Dashboard.png" style="width: 27px; height: 27px;" /></a>&nbsp; <a href="Index.aspx" target="_top">
                                                            <img alt="Home" src="https://student.nielit.gov.in/images/homepage.png" style="width: 28px; height: 28px;" /></a>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <iframe name="ifHome" onload="autoResize(this)" id="ifHome" src="Home.aspx" width="100%"
                                    height="400px" frameborder='0' marginheight='0' marginwidth='0' scrolling="no"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="footer" onclick="window.open('http://www.nielit.gov.in')">Designed &amp; *Developed By NIELIT-HQ IT Team
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </form>
</body>

</html>
