<%@ Page Language="C#" Title="Online Student Information and Enrollment System, NIELIT"
    AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="Script/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="Script/jquery.dd.js" type="text/javascript"></script>
    <script src="Script/login.js" type="text/javascript"></script>
    <script src="Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript">
        if (top.location != self.location) {
            top.location = self.location.href
        }
        window.onbeforeunload = function doUnload(e) {
            if ((window.event.clientX < 0) || (window.event.clientY < 0)) {
                __doPostBack("lbLogOut", "");
            }
        }
        function ShowAlert(eventArgument, context) {
            if (eventArgument != '') {
                alert(eventArgument);
                __doPostBack("lbLogOut", "");
            }
        }
        function AutoCheck() {
            startTime();
            setInterval(function () { CheckLog() }, 10000);
        }

        function openWindow() {
            window.open('Admin/AdminChangePasswd.aspx?src=profile', '_blank', 'top=200,left=300,height=280,width=555,location=no,menubar=no,status=no,toolbar=no,scrollbars=no,resizable=no');
            return false;
        }


    </script>
    <!-- Code 001 Start NEWS FLASH SCRIPT -->
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
            document.getElementById('txt').innerHTML = month + " " + Day + ", " + Year + "<br/>" + time;
            var t = setTimeout(startTime, 500);
        }

    </script>

        <style type="text/css">
            
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

            </style>
    <!-- Code 001 End NEWS FLASH SCRIPT -->
</head>
<body onload="AutoCheck();">
    <div id="showimage" style="position: absolute; width: 250px; left: 400px; top: 100px;
        z-index: 5000;">
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
                                    <img src="images/cancel.gif" alt="Close" width="16px" height="14px"
                                        border="0" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <iframe name="ifImportant" onload="autoResizeF(this)" id="Iframe1" src="ImportantNotificationnew.aspx"
                                    width="100%" frameborder='0' marginheight='0' marginwidth='0' scrolling="no">
                                </iframe>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="1000px" style="margin-bottom: 2px; min-height: 600px;" border="0" align="center"
        cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" valign="top">
                <table width="1000px" border="0" cellspacing="0" cellpadding="0" align="center">
                    <tr>
                        <td height="68px">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="center" valign="top" class="logo">
                                    </td>
                                    <td align="center" class="heading">
                                        <div id="tdHeaderBig" runat="server">
                                        </div>
                                        <div id="tdHeaderSmall" runat="server" class="heading_small">
                                        </div>
                                    </td>
                                    <td width="254" valign="top" bgcolor="#8FBCDB" class="login_header">
                                        <table width="100%" border="0" cellpadding="4" cellspacing="4" bgcolor="#8FBCDB">
                                            <tr>
                                                <td style="font-size: 12px;" align="right">
                                                </td>
                                                <td colspan="4">
                                                    <div id="loginContainer2">
                                                        <a title="Click here to change password or logout" href="#" id="loginButton2">
                                                            <label>
                                                                hi,
                                                                <%=GetUserName()%>&nbsp;&nbsp;</label><span>&nbsp;&nbsp;</span></a>
                                                        <div id="loginBox2" align="left">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" id="loginForm2">
                                                                <tr>
                                                                    <td class="loginuser2">
                                                                        <ul>
                                                                            <li><a title="Change Password" href="#" onclick="openWindow();">
                                                                                <img src="Images/change_password.png" align="right" border="0" width="16" height="16"
                                                                                    vspace="4" style="float: left;" />
                                                                                Change Password</a></li>
                                                                            <li>
                                                                                <asp:LinkButton ToolTip="Logout" ID="lbLogOut" runat="server" OnClick="lbLogOut_Click">
                                                                                    <img border="0" src="Images/logout.png" align="right" width="16" height="16"   vspace="4"
                                                                                    style="float: left;" />Logout</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="24px" align="right">
                                                    &nbsp;
                                                </td>
                                                <td style="font-size: 12px;" height="24px" colspan="2" align="right">
                                                    <label id="txt"> </label>
                                                </td>
                                                <td height="24px" style="font-size: 10pt; font-weight: normal;" align="right">
                                                    (<%=Session["UserType"].ToString()%>
                                                    User)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="1000px" class="menu">
                            <asp:Menu Orientation="Horizontal" ID="Menu1" runat="server">
                            </asp:Menu>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <iframe name="ifHome" onload="autoResize(this);" id="ifHome" src="frmdashboard.aspx"
                                width="100%" style="height: 400px;" frameborder='0' marginheight='0' marginwidth='0'
                                scrolling="no"></iframe>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="footer" onclick="window.open('http://www.nielit.gov.in')">
                            Designed &amp; *Developed By NIELIT-HQ IT Team
                        </td>
                    </tr>
                    <%--<tr>
                        <td align="center" class="footer">
                        </td>
                    </tr>--%>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
