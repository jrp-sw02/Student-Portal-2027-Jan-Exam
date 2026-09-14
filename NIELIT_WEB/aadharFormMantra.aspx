<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aadharFormMantra.aspx.cs" Inherits="aadharFormMantra" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="Scripts/jquery.min.js"></script>
    <script src="assets/js/jquery.min.js"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
           // alert('ready');
            CaptureMantra();
        });
        function CaptureMantra() {
            var url = "https://127.0.0.1:8005/rd/capture"
            //https://127.0.0.1:8005/rd/capture";
         //   alert (url);
            // var url = "http://localhost:11101/capture";
           /* var PIDOPTS = '"<PidOptions><Opts fCount=\"1\" fType=\"0\" iCount=\"0\"
            pCount=\"0\" format=\"0\" pidVer=\"2.0\" timeout=\"20000\" otp=\"\"
            posh=\"LEFT_INDEX\" env=\"S\" wadh=\"\" /> <Demo></Demo> <CustOpts> <Param
            name=\"Param1\" value=\"\" /> </CustOpts> </PidOptions>"';*/
            var PIDOPTS = '<PidOptions ver=\"1.0\">' + '<Opts fCount=\"1\" fType=\"2\" iCount=\"0\" iType=\"\" pCount=\"0\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"20000\" otp=\"\" wadh=\"\" posh=\"\"/>' + '</PidOptions>';
           // alert(PIDOPTS);
            //var PIDOPTS = '<PidOptions ver=\"2.0\"><Opts fCount=\"1\" fType=\"0\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"20000\" otp=\"\" wadh=\"\" env=\"P\" posh=\"\"/></PidOptions>';

            var xhr;
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return version number
            {
                //IE browser
                xhr = new ActiveXObject("Microsoft.XMLHTTP");
            } else {
                //other browser
                xhr = new XMLHttpRequest();
            }

            xhr.open('CAPTURE', url, true);

            xhr.setRequestHeader("Content-Type", "text/xml");
            xhr.setRequestHeader("Accept", "text/xml");

            xhr.onreadystatechange = function () {

                if (xhr.readyState == 4) {
                    var status = xhr.status;
                   //  alert(status);
                    if (status == 200) {
                        var x = 1;


                        x = xhr.responseText;
                        // alert('response'+x);
                        var bytes = [];

                        for (var i = 0; i < x.length; ++i) {
                            bytes.push(x.charCodeAt(i));
                        }
                        var finalResponse = arrayBufferToBase64(bytes);
                        //    x = x.substring(1, 25);
                        // var bytes = [];

                        //for (var i = 0; i < x.length; ++i) {
                        //    bytes.push(x.charCodeAt(i));
                        //         }
                        //// var bytes = x.getBytes(Charsets.UTF_8);
                        //var finalResponse = arrayBufferToBase64(bytes);
                        //// alert('final' + getCheckedRadio());
                        // alert("final" + finalResponse);


                        callService(finalResponse);


                    } else {

                        console.log(xhr.response);

                    }

                }
            };
            xhr.send(PIDOPTS);
        }
      function Capture() {

          var url = "http://127.0.0.1:11100/capture";

          // var url = "http://localhost:11100/capture";
            var PIDOPTS = '<PidOptions ver=\"1.0\">' + '<Opts fCount=\"1\" fType=\"2\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"20000\" otp=\"\" wadh=\"\" posh=\"\"/>' + '</PidOptions>';

            //var PIDOPTS = '<PidOptions ver=\"2.0\"><Opts fCount=\"1\" fType=\"0\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"20000\" otp=\"\" wadh=\"\" env=\"P\" posh=\"\"/></PidOptions>';
          
            var xhr;
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return version number
            {
                //IE browser
                xhr = new ActiveXObject("Microsoft.XMLHTTP");
            } else {
                //other browser
                xhr = new XMLHttpRequest();
            }

            xhr.open('CAPTURE', url,true);
          
            xhr.setRequestHeader("Content-Type", "text/xml");
            xhr.setRequestHeader("Accept", "text/xml");

            xhr.onreadystatechange = function () {
               
                if (xhr.readyState == 4) {
                    var status = xhr.status;
                   // alert(status);
                    if (status == 200) {
                        var x = 1;
                        
                       
                        x = xhr.responseText;
                       // alert('response'+x);
                         var bytes = [];

                        for (var i = 0; i < x.length; ++i) {
                            bytes.push(x.charCodeAt(i));
                                 }
                        var finalResponse = arrayBufferToBase64(bytes);
                    //    x = x.substring(1, 25);
                       // var bytes = [];

                        //for (var i = 0; i < x.length; ++i) {
                        //    bytes.push(x.charCodeAt(i));
                        //         }
                        //// var bytes = x.getBytes(Charsets.UTF_8);
                        //var finalResponse = arrayBufferToBase64(bytes);
                        //// alert('final' + getCheckedRadio());
                       // alert("final" + finalResponse);
                      
                       
                        callService(finalResponse);
                       
                       
                    } else {

                        console.log(xhr.response);

                    }
                   
                }
               
            };

            xhr.send(PIDOPTS);
      
        }

        function callService(x) {
          
            //Getting data from controls
            // alert('x'+document.getElementById("rdbtnlstgender"));
            
            var body = {
                "name": document.getElementById("txtAppName").value,
                "dob": formatDate(document.getElementById("txtDob").value),
                "gender": getCheckedRadio().toLowerCase(),
                "biometricDeviceResponse": x,
                "aadhaarNumber": document.getElementById("txtaadhar").value,
                "id": document.getElementById("HStudentID").value
            };
           // alert('x1 JSon' + x);
      //{
      //    "name": document.getElementById("txtAppName").value,
      //    //dob = "27/11/1974",
      //    "dob": formatDate(document.getElementById("txtDob").value),
      //    //Convert.ToDateTime(txtDob.Text).ToString("yyyy-MM-dd"),
      //    "gender": getCheckedRadio(),
      //    //document.getElementById("rdbtnlstgender").value,
      //    //rdbtnlstgender.SelectedValue,
      //    "biometricDeviceResponse": x,
      //    "aadhaarNumber": document.getElementById("txtaadhar").value
      //    //txtaadhar.Text
      //    //name = "Santosh Bhardwaj",
      //    ////dob = "27/11/1974",
      //    //dob = "1974-11-27",
      //    //     gender="F",
      //    //     biometricDeviceResponse=finalResponse ,
      //    //aadhaarNumber = "206927461266"
      //};
            var jsonData = JSON.stringify(body);
           // alert('j' + jsonData);
               // obj.name = jsonData;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Services/BiometricService.asmx/DBEntity",
                    data: jsonData,

                    dataType: "json",
                    success: function (Result) {
                        Result = Result.d;
                       alert(Result);
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                });


            
        }
        function formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth()+1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2)
                month = '0' + month;
            if (day.length < 2)
                day = '0' + day;
           // alert( [year, month, day].join('-'));
            return [year, month, day].join('-');
        }

        function getCheckedRadio() {
            var radioButtons = document.getElementsByName("rdbtnlstgender");
                 for (var x = 0; x < radioButtons.length; x ++) {
                       if (radioButtons[x].checked) {
                        //   alert("You checked " + radioButtons[x].id);
                           return (radioButtons[x].value);
                            }
                      }
               }
        function arrayBufferToBase64(buffer) {
            var binary = '';
            var bytes = new Uint8Array(buffer);
            var len = bytes.byteLength;
            var i;
            for ( i = 0; i < len; i++) 
            {
                binary += String.fromCharCode(bytes[i]);
            }
            //  return (window.btoa(unescape(encodeURIComponent(binary))));
            return (window.btoa(binary));
        }
             </script>
            
        <%-- <script type="text/javascript">
        $(document).ready(function () {
            var obj = {};
          //  alert('aaa Gyaa !!');

            obj.name = $.trim($("[id*=TextBox1]").val());
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Services/BiometricService.asmx/DBEntity",
                data: JSON.stringify(obj),

                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                },
                error: function (Result) {
                    alert("Error");
                }
            });


        });
        </script>--%>
      
</head>
<body>
    <form id="form1" runat="server" style="background-color: #FFFFFF">
         <%--<asp:ScriptManager ID="ScriptManager1" runat="server"/>--%>
         <div>     
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" style="margin-bottom:20px">          
            <tr>
                <td>
                    <%-- <Scripts>
                <asp:ScriptReference Name="AjaxControlToolkit.Common.Common.js" Assembly="AjaxControlToolkit" />
                <asp:ScriptReference Name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" Assembly="AjaxControlToolkit" />
            </Scripts>--%>    
                               
                       <asp:HiddenField ID="hidPID" runat="server" />
                  
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center" colspan ="3">
                    <strong style="text-align: center">AADHAAR AUTHENTICATION FORM </strong>
                </td>
                </tr>
                 <tr>
                <td align="center">
                  <%--  <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>--%>
                            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                Width="99%"></asp:Label>
                            <asp:Label ID="messages" runat="server" Visible="true"
                                Width="99%"></asp:Label>
                    <asp:HiddenField ID="HStudentID" runat="server" />
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
          
            
                        <tr>
                            <td colspan="3">
                                1.
                                <asp:Label ID="Label69" runat="server" Text="Applicant's Personal Details /आवेदक का व्यक्तिगत विवरण">
                                </asp:Label>
                            </td>

                        </tr>
                        <tr class="gdrow1">
                            <td>
                                1.1
                            </td>
                            <td>
                                <asp:Label ID="Label70" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList Width="95px" runat="server" ID="ddlSalutaionName"  TabIndex="7" Visible="False">
                                    <asp:ListItem Value="0" Selected="True" Text="--Select--">
                                    </asp:ListItem>
                                    <asp:ListItem Value="Mr." Text="Mr./श्री">
                                    </asp:ListItem>
                                    <asp:ListItem Value="Ms." Text="Ms./सुश्री">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtAppName" runat="server" MaxLength="60" Width="430px" TabIndex="8"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" 
                                    autocomplete="off" Enabled="False"></asp:TextBox>
                                <br />
                                ( Full name as per the highest / latest qualification certificate or legal certificate
                                )
                                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>  --%>
                                <asp:TextBox ID="TextBox2" runat="server"  MaxLength="90000" Visible="False" ></asp:TextBox>
                          <%--  </ContentTemplate> 
                                     </asp:UpdatePanel>
                              --%>
                                <%--style="display:none;" --%>
                            </td>
                        </tr>
            <%--<input type ="hidden" id ="hidPID" name="hidPID"  />--%>
                        <tr class="gdrow1" id="trgender" runat="server">
                            <td>
                                1.2
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Gender / लिंग<font color='RED'>*</font>">
                                </asp:Label>
                            </td>
                            <td>
                              <%--  <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:RadioButtonList ID="rdbtnlstgender" runat="server" RepeatDirection="Horizontal"
                                            TabIndex="14" Width="300px" Enabled="False">
                                            <asp:ListItem Value="M">Male / पुरुष
                                            </asp:ListItem>
                                            <asp:ListItem Value="F">Female / महिला
                                            </asp:ListItem>
                                        </asp:RadioButtonList>
                                   <%-- </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlSalutaionName" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr class="trgdalternate1calendar" id="trdob" runat="server">
                            <td>
                                1.3
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक <font color='RED'>*</font> (dd-Mon-yyyy)">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox MaxLength="11" ID="txtDob" runat="server" SkinID="txtDate" Width="99px"
                                    TabIndex="15" onpaste="return false;" oncopy="return false;" oncut="return false;" Enabled="False"
                                    ></asp:TextBox>
                              
                                <br />
                                ( As per high school certificate in 'dd-Mon-yyyy' format. i.e. '01-Jan-1990' )
                                <%--<asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>--%>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                2. Identification Details / पहचान की सूचना
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="Aadhaartr" runat="server" >
                            <td>
                                2.1
                            </td>
                            <td>
                                <asp:Label ID="Label49" runat="server" Text="Aadhaar Card Number / आधार कार्ड संख्या">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtaadhar" runat="server" MaxLength="15" TabIndex="49" Width="520px" 
                                    onpaste="return false;" oncopy="return false;" oncut="return false;" onkeypress="checkNumber(this,15,0,event);" Enabled="False"  ></asp:TextBox>
                            </td>
                        </tr>
                                            
                    </table>                    
               
                   
    </div>
    <div style="text-align: center">
    <asp:TextBox ID="TextBox1" runat="server" Text="Shivesh" Visible="False"></asp:TextBox>
            <button id="Button2" type="button" onclick="CaptureMantra()" runat ="server" visible="False">Capture</button>
                    <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClientClick="ValidateFormFields();"/>--%>
    </div>
    </form>
</body>
</html>

