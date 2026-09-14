//function md(e) {
//    try { if (event.button == 2 || event.button == 3) return false; }
//    catch (e) { if (e.which == 3) return false; }
//}
//document.oncontextmenu = function () { return false; }
//document.ondragstart = function () { return false; }
//document.onmousedown = md;  

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 33 || charCode == 60 || charCode == 62)
        return false;

    return true;
}


//document.onmousedown = md;
 var val = navigator.userAgent.toLowerCase();
 if (val.indexOf("firefox") > -1) {
     document.onkeypress = CheckInvalidCharacterOnPaste();

     //CheckInvalidCharacters 
 }
document.onkeyup = CheckInvalidCharacterOnPaste();

 function CheckInvalidCharacterOnPaste(e) {
     try
     {
         if (event.srcElement.type == "text" || event.srcElement.type == "textarea")
         {
                 var txt = event.srcElement.value;                 
                     if (txt.indexOf('!') >= 0)
                         event.srcElement.value = txt.replace(/!/g, "").replace(/>/g, "").replace(/</g, "").replace(/&/g, "").replace(/#/g, "");
                     else if (txt.indexOf('<') >= 0)
                         event.srcElement.value = txt.replace(/!/g, "").replace(/>/g, "").replace(/</g, "").replace(/&/g, "").replace(/#/g, "");
                     else if (txt.indexOf('>') >= 0)
                         event.srcElement.value = txt.replace(/!/g, "").replace(/>/g, "").replace(/</g, "").replace(/&/g, "").replace(/#/g, "");
//                     else if (txt.indexOf('&') >= 0)
//                         event.srcElement.value = txt.replace(/!/g, "").replace(/>/g, "").replace(/</g, "").replace(/&/g, "").replace(/#/g, "");
//                     else if (txt.indexOf('#') >= 0)
//                         event.srcElement.value = txt.replace(/!/g, "").replace(/>/g, "").replace(/</g, "").replace(/&/g, "").replace(/#/g, "");    
        }
     }
     catch(err)
     {
//       throw err;
     }                 
   }
   document.onkeyup = CheckInvalidCharacterOnPaste;
   document.onmouseout = CheckInvalidCharacterOnPaste;
function chekMobNo(ctl) {
    if (document.getElementById(ctl)) {
        var flag = false;
        var msg = "Please Enter Valid Mobile Number. Mobile Number should be of 10 digits"
        if (trim(document.getElementById(ctl).value, " ") != "") {
            if (trim(document.getElementById(ctl).value, " ").length != 10)
                flag = false;
            else if (trim(document.getElementById(ctl).value, " ").length == 10 && trim(document.getElementById(ctl).value, " ").indexOf("0") == 0)
                flag = false;
            else
                flag = true;
        }
        if (flag == false) {
            CallDiv(ctl, msg)
            if (document.getElementById(ctl).disabled == false)
                document.getElementById(ctl).focus();
            return false;
        }
        else
            return true;
    }
    return true;
}


function isValidPassingYear(PassingYearctrlId, DobctrlId) {
    if (document.getElementById(PassingYearctrlId)) {
        var PassingYear = document.getElementById(PassingYearctrlId).value;
        var Dobdate = document.getElementById(DobctrlId).value;
        var dobYear = Dobdate.split("-");
        var msg =  "Invalid Passing Year";
        var d = new Date();
        if (d.getFullYear() < PassingYear || (PassingYear) < (Number(dobYear[2]) + 10)) {
            CallDiv(PassingYearctrlId, msg);
            return false;
        }
        else
        return true;
    }
    return true;

}


function isvalidImageFile(ctl, str) {

    if (document.getElementById(ctl)) {

        var fileName = new String();
        var fileExtension = new String();  
        fileName = document.getElementById(ctl).value;         
        fileExtension = fileName.substr(fileName.length - 3, 3);             
        var validFileExtensions = new Array("jpg", "gif", "jpeg", "png");
        var flag = false;          
        for (var index = 0; index < validFileExtensions.length; index++) {
            if (fileExtension.toLowerCase() == validFileExtensions[index].toString().toLowerCase()) {
                flag = true;
            }
        }
        if (flag == false) {
            var msg = 'Invalid ' + str + '.Only jpg, gif, jpeg ,png extensions are allowed.';
            CallDiv(ctl, msg)
            return false;
        }
        else
            return true;
    }
        return true;
    
}


function isValidTeliphone(STDctrlId, PhoneNoCtrlId) {

    if (document.getElementById(STDctrlId) && document.getElementById(PhoneNoCtrlId)) {
        var flag1 = false;
        var flag2 = false;
        var msg = "Please Enter Valid Std Code (starting with 0) and Phone Number. Phone Number should be of 11 digits with std code"
        var StdLength = trim(document.getElementById(STDctrlId).value, " ").length;
        var PhoneNoLength = trim(document.getElementById(PhoneNoCtrlId).value, " ").length;
        var totallength = StdLength + PhoneNoLength;
        if (trim(document.getElementById(STDctrlId).value, " ") != "" && trim(document.getElementById(PhoneNoCtrlId).value, " ") != "") {
            if (StdLength != 0 && PhoneNoLength != 0) {
                if (totallength > 11 || totallength < 11) {

                    CallDiv(PhoneNoCtrlId, msg)
                    if (document.getElementById(PhoneNoCtrlId).disabled == false)
                        document.getElementById(PhoneNoCtrlId).focus();
                    return false;
                }
                else if (trim(document.getElementById(STDctrlId).value, " ").indexOf("0") != 0 || (trim(document.getElementById(PhoneNoCtrlId).value, " ").indexOf("0")) == 0) {
                    CallDiv(PhoneNoCtrlId, msg)
                    if (document.getElementById(PhoneNoCtrlId).disabled == false)
                        document.getElementById(PhoneNoCtrlId).focus();
                    return false;
                }
                else
                    return true;
            }
        }
    }
    return true;

}

//isValidDob(ctrl)
//{ 
// var Dobdate = document.getElementById(ctrl).value;
//           var result = Date.today().addYears(-10).compareTo(Date.parse(Dobdate));
//           if (result == -1 || result == 0) {
//               alert("helloo");
//               CallDiv(ctrl, "Invalid Date");               
//                alert(Date.today().addYears(-1).compareTo(Date.parse(Dobdate)));
//                alert(Date.parse(Dobdate));
//                return false;
//            }

//}

function callErrorMsg(ctrlId, msg) {
    CallDiv(ctrlId, msg);    
 }

function isNumber(ctrlId) {
    if (document.getElementById(ctrlId)) {
        if (trim(document.getElementById(ctrlId).value, " ") != "") {
            if (isNaN(trim(document.getElementById(ctrlId).value, " "))) {
                var msg = "Invalid Number";
                CallDiv(ctrlId, msg)
                if (document.getElementById(ctrlId).disabled == false)
                    document.getElementById(ctrlId).focus();
                return false;
            }
            else
                return true;
        }
    }
    return true;
}


function ischecked(ctl, msg) {

    if (document.getElementById(ctl).checked == false) {
        alert("Please Select  " + msg);
        if (document.getElementById(ctl).disabled == false)
            document.getElementById(ctl).focus();
        return false;
    }
    else {
       
        return true;
    }
    return true;
}

function Validate_Checkbox(msg) {
    var chks = document.getElementsByTagName('input');
    var hasChecked = false;
    for (var i = 0; i < chks.length; i++) {
        if (chks[i].checked) {
            hasChecked = true;
            break;
        }
    }
    if (hasChecked == false) {
        alert("Please select at least one record");

        return false;
    }
    else {
        if (ConfirmAction(msg))
            return true;
        else
            return false;
    }
}

function isvalidateRadioButtonList(ctrl, msg) {
    var listItemArray = document.getElementsByName(ctrl);
    var isItemChecked = false;
    for (var i = 0; i < listItemArray.length; i++) {
        var listItem = listItemArray[i];
        if (listItem.checked) {           
            isItemChecked = true;
        }
    }
    if (isItemChecked == false) {
        CallDiv(ctrl, "Please select " + msg)
        return false;       
    }
    return true;
}


function isBlank(ctrl, msg) {
    if (document.getElementById(ctrl)) {
        if (trim(document.getElementById(ctrl).value, " ") == "") {
            CallDiv(ctrl, msg + " can not be left blank")
            document.getElementById(ctrl).value = ""
            if (document.getElementById(ctrl).disabled == false)
                document.getElementById(ctrl).focus();
            return false;
        }
    }
    return true;
}

function isBlankNumber(ctrl, msg) {
    if (document.getElementById(ctrl)) {
        if (trim(document.getElementById(ctrl).value, " ") == "" || parseFloat(trim(document.getElementById(ctrl).value, " ")) == 0 || trim(document.getElementById(ctrl).value, " ") == ".") {
            CallDiv(ctrl, msg + " can not be left blank");
            document.getElementById(ctrl).value = ""
            if (document.getElementById(ctrl).disabled == false)
                document.getElementById(ctrl).focus();
            return false;
        }
    }
    return true;
}

function isSelected(ctrl, msg) {
    if (document.getElementById(ctrl) && document.getElementById(ctrl).value == "0") {
        CallDiv(ctrl, "Please Select " + msg);
        document.getElementById(ctrl).focus();
        return false;
    }
    return true;
}

function trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

var divTag = document.createElement("div");
var imgTag = document.createElement('div');
function findPos(obj) {
    var curleft = curtop = 0;
    if (obj.offsetParent) {
        curleft = obj.offsetLeft
        curtop = obj.offsetTop
        while (obj = obj.offsetParent) {
            curleft += obj.offsetLeft
            curtop += obj.offsetTop
        }
    }
    return [curleft, curtop];
}
function CallDiv(ctrl, msg) {
//    var imgHtml = "<img src='App_Themes/Blue/Images/tip.png' border='0' style='top:-10; left:-10;'/>";
    //    divTag.innerHTML = imgHtml;
    var pos = findPos(document.getElementById(ctrl));
   
    imgTag.id = "imgTag";
    imgTag.className = "error_tip";
    imgTag.style.left = (pos[0] + 3)  + "px";
    imgTag.style.visibility = "visible";
    imgTag.style.width = "20px"
    imgTag.style.top = (pos[1]+11) + "px";
       
    
    divTag.id = "divDyn";
    divTag.innerHTML = msg;
    divTag.style.left = pos[0] + "px";
    divTag.style.visibility = "visible";
    divTag.style.width = "199px";
    // IE
    if (document.getElementById(ctrl).currentStyle)
    { 
        //if (document.getElementById(ctrl).currentStyle["width"].match(/\d/g).join("") < 200)
            divTag.style.width = (document.getElementById(ctrl).currentStyle["width"].match(/\d/g).join("")-5)+"px";
    }
    else if (document.defaultView && document.defaultView.getComputedStyle) // other browsers
    {//alert(document.defaultView.getComputedStyle(document.getElementById(ctrl), null).getPropertyValue("width"))
        if(document.defaultView.getComputedStyle(document.getElementById(ctrl), null).getPropertyValue("width").match(/\d/g).join("") < 200)
            divTag.style.width = (document.defaultView.getComputedStyle(document.getElementById(ctrl), null).getPropertyValue("width").match(/\d/g).join("")-4)+"px"
    }
    divTag.style.top = pos[1] + 25 + "px";
    divTag.className = "error_div";
    
    document.body.appendChild(imgTag);
    document.body.appendChild(divTag);

    setTimeout("HideCtrl()", 100000);
    if (document.getElementById(ctrl).addEventListener) {
        document.getElementById(ctrl).addEventListener("keydown", HideCtrl, false);
        document.getElementById(ctrl).addEventListener("change", HideCtrl, false);
        document.getElementById(ctrl).addEventListener("focusout", HideCtrl, false);
    }
    else if (document.getElementById(ctrl).attachEvent) {
        document.getElementById(ctrl).attachEvent('onkeydown', HideCtrl);
        document.getElementById(ctrl).attachEvent('onchange', HideCtrl);
        document.getElementById(ctrl).attachEvent('onfocusout', HideCtrl);

    }
    if(!document.getElementById(ctrl).disabled=="disabled")
        document.getElementById(ctrl).focus();
}
function HideCtrl() {
    divTag.style.visibility = "hidden";
    imgTag.style.visibility = "hidden";
}
function DetectBrowser()
{
    var val = navigator.userAgent.toLowerCase();
    if(val.indexOf("firefox") > -1)
    {
        return "firefox";
    } 
    else if(val.indexOf("opera") > -1)
    {
        return "opera";
    }
    else if(val.indexOf("msie") > -1)
    {
        return "msie";
    }
    else if (val.indexOf("chrome") > -1) 
    {
        return "chrome";
    }
    else if(val.indexOf("safari") > -1)
    {
        return "safari";
    }
    
}

function autoResize(ctl) {
    try {
        if (ctl) {
            var browser = DetectBrowser();
            var newheight = 450;
            if (document.getElementById) {
                if (browser == "chrome" || browser == "safari")
                    newheight = ctl.contentDocument.documentElement.scrollHeight;
                else
                    newheight = ctl.contentWindow.document.body.scrollHeight;
            }
            if (newheight < 450)
                newheight = 450;
            ctl.style.height = (newheight) + "px";
        }
    }
    catch (ex) { 
    }
}
function autoResizeF(ctl) {
    try {
        if (ctl) {
            var browser = DetectBrowser();
            var newheight = 100;
            if (document.getElementById) {
                if (browser == "chrome" || browser == "safari")
                    newheight = ctl.contentDocument.documentElement.scrollHeight;
                else
                    newheight = ctl.contentWindow.document.body.scrollHeight;
            }
            if (newheight < 100)
                newheight = 100;
            ctl.style.height = (newheight) + "px";
        }
    }
    catch (ex) {
    }
}


//function checkNumber(ctrl, digitBeforePoint, digitAfterPoint) {
//    var arrVal = ctrl.value.split(".")
//    if (digitAfterPoint == 0) {
//        if (ctrl.value.length >= digitBeforePoint) {
//            window.event.keyCode = 0
//            return false;
//        }
//    }
//    else if (digitAfterPoint > 0) {
//        if (arrVal.length == 1) {
//            if (arrVal[0].length == digitBeforePoint && window.event.keyCode != 46) {
//                window.event.keyCode = 0
//                return false;
//            }
//        }
//        else if (arrVal.length == 2) {
//            if (window.event.keyCode == 46) {
//                window.event.keyCode = 0
//                return false;
//            }
//            if (arrVal[1].length > digitAfterPoint) {
//                window.event.keyCode = 0
//                return false;
//            }
//        }
//    }
//    var curLoc = (GetCursorLocation(ctrl))
//    var ch = window.event.keyCode
//    if (ch == 46 && digitAfterPoint == 0) {
//        CallDiv(ctrl.id, "Dot(.) not allowed. Please enter numiric value only");
//        window.event.keyCode = 0
//        return false;
//    }
//    else if (ch == 46) {
//        return true;
//    }
//    else if (ch == 45 && curLoc == 0 && ctrl.value.search("-") < 0) {
//        return true;
//    }
//    else if (isNaN(String.fromCharCode(ch)) || ch == 32)//.toString()+String.fromCharCode(ch)
//    {
//        CallDiv(ctrl.id, "Not a number. Please enter numiric value only");
//        window.event.keyCode = 0
//        return false;
//    }
//}




function checkNumber(ctrl, digitBeforePoint, digitAfterPoint, evt) {

    if (evt == null)
        evt = window.event;
    var arrVal = ctrl.value.split(".")
    var keyCode;


    if (evt.keyCode > 0) {

        keyCode = evt.keyCode;
    }

    else if (typeof (evt.charCode) != "undefined") {

        keyCode = evt.charCode;
    }

    if (digitAfterPoint == 0) {
        if (ctrl.value.length >= digitBeforePoint) {
     
            keyCode = 0
            return false;
        }
    }


    else if (digitAfterPoint > 0) {
        if (arrVal.length == 1) {
            if (arrVal[0].length == digitBeforePoint && keyCode != 46) {
                keyCode = 0
                return false;
            }
        }
        else if (arrVal.length == 2) {
            if (keyCode == 46) {
                keyCode = 0
                return false;
            }
            if (arrVal[1].length > digitAfterPoint) {
                keyCode = 0
                return false;
            }
        }
    }


    var curLoc = (GetCursorLocation(ctrl))
    var ch = keyCode
    if (ch == 46 && digitAfterPoint == 0) {
        CallDiv(ctrl.id, "Dot(.) not allowed. Please enter numeric value only");
        keyCode = 0;
        return false;
    }
    else if (ch == 46) {
        return true;
    }
    else if (ch == 45 && curLoc == 0 && ctrl.value.search("-") < 0) {
        return true;
    }
    else if (isNaN(String.fromCharCode(ch)) || ch == 32)//.toString()+String.fromCharCode(ch)
    {
        keyCode = 0;
        CallDiv(ctrl.id, "Not a number. Please enter numeric value only");        
        return false;
    }
}

function GetCursorLocation(CurrentTextBox) {
    var CurrentSelection, FullRange, SelectedRange, LocationIndex = -1;
    if (typeof CurrentTextBox.selectionStart == "number") {
        LocationIndex = CurrentTextBox.selectionStart;
    }
    else if (document.selection && CurrentTextBox.createTextRange) {
        CurrentSelection = document.selection;
        if (CurrentSelection) {
            SelectedRange = CurrentSelection.createRange();
            FullRange = CurrentTextBox.createTextRange();
            FullRange.setEndPoint("EndToStart", SelectedRange);
            LocationIndex = FullRange.text.length;
        }
    }
    return LocationIndex;
}
function checkDot(ctl) {
    if (ctl.value == "")
        return false;
    if (ctl.value == ".")
        ctl.value = "";
    else
        ctl.value = eval(ctl.value);
}

function formatNumber(ct) {
    if (ctl.value != "") {
        ctl.value = parseFloat(ctl.value).toFixed(2);
    }
}

function isValidEmail(ctl, msg) {
    if (document.getElementById(ctl)) {
        var emailCtl = document.getElementById(ctl);
        emailCtl.value = trim(emailCtl.value, ' ');
        var email = emailCtl.value;
        var response = true;
        if (email == null) {
            response = true;
        }
        if (email.length == 0) {
            response = true;
        }
        if (!allValidChars(email)) {  // check to make sure all characters are valid
            response = false;
        }
        else if (email.indexOf("@") < 1) { //  must contain @, and it must not be the first character
            response = false;
        }
        else if (email.lastIndexOf(".") <= email.indexOf("@")) {  // last dot must be after the @
            response = false;
        }
        else if (email.indexOf("@") == email.length) {  // @ must not be the last character
            response = false;
        }
        else if (email.indexOf("..") >= 0) { // two periods in a row is not valid
            response = false;
        }
        else if (email.indexOf(".") == email.length || (email.length - email.lastIndexOf(".")) < 3) {  // . must not be the last character
            response = false;
        }
        if (response == false) {
            CallDiv(ctl, msg);
            return false;
        }
        else
            return true;
    }
    return true;
}

function allValidChars(email) {
    var parsed = true;
    var validchars = "abcdefghijklmnopqrstuvwxyz0123456789@.-_";
    for (var i = 0; i < email.length; i++) {
        var letter = email.charAt(i).toLowerCase();
        if (validchars.indexOf(letter) != -1)
            continue;
        parsed = false;
        break;
    }
    return parsed;
}



//This function can be used for GridView control only. Id of both checkboxes should be same.
function CheckUncheckAll(GridControlId, Sender, CheckBoxName) {
    //cphContents_GridView1
    //cphContents_GridView1_chk_0
    //chk
    //alert(GridControlId + "\n" + Sender.id + "\n" + CheckBoxName)
    var allCheckBox = document.getElementById(GridControlId + "_" + CheckBoxName)
    var index = 0; var action = false;
    if (Sender == allCheckBox) {
        if (Sender.checked == true)
            action = true
        for (index = 0; index < 10000; index++) 
        {
            if (document.getElementById(GridControlId + "_" + CheckBoxName + "_" + index))
                document.getElementById(GridControlId + "_" + CheckBoxName + "_" + index).checked = action
            else
                break;
        }
    }
    else {
        action = true;
        for (index = 0; index < 10000; index++) 
        {
            if (document.getElementById(GridControlId + "_" + CheckBoxName + "_" + index)) 
            {
                if (document.getElementById(GridControlId + "_" + CheckBoxName + "_" + index).checked == false) 
                {
                    action = false;
                    break;
                }
            } 
            else
                break;
        }
        allCheckBox.checked = action;
    }
}

var CurrentObj = null;
var popupCtlID = null;
function ShowHideMenu(obj, tableid) { 
    popupCtlID = tableid;
    if (CurrentObj == null) {
        CurrentObj = obj
        ShowHideMenu(obj, tableid)
    }
    document.getElementById(tableid).style.backgroundColor = "WhiteSmoke";
    if (document.getElementById(tableid).style.visibility == 'hidden') {
        var arrPos = findPos(obj)
        obj.src = '../Images/fleche_up_sel.gif'
        document.getElementById(tableid).style.visibility = 'visible'
        document.getElementById(tableid).style.top = (arrPos[1] - (document.getElementById(tableid).style.height).replace(/px/i, '')) + 22  + "px"
        document.getElementById(tableid).style.left = (arrPos[0] - (document.getElementById(tableid).style.width).replace(/px/i, ''))  + "px"

    }
    else {
        obj.src = '../Images/fleche_down_sel.gif'
        document.getElementById(tableid).style.visibility = 'hidden'

        var arrLoc = CurrentObj.src.split("/")
        if (arrLoc[arrLoc.length - 1] == 'fleche_up_sel.gif') {
            CurrentObj.src = '../Images/fleche_down_sel.gif'
            var arrPos = findPos(obj)
            obj.src = '../Images/fleche_up_sel.gif'
            document.getElementById(tableid).style.visibility = 'visible'
            document.getElementById(tableid).style.top = arrPos[1] + "px";
            document.getElementById(tableid).style.left = (arrPos[0] - (document.getElementById(tableid).style.width).replace(/px/i, '')) + "px"
        }
    }
    CurrentObj = obj;
}

function ConfirmAction(msg) {
    if (confirm(msg))
        return true;
    else
        return false;
}

//function document.documentElement.onclick() {
//    if (document.getElementById(popupCtlID)) {
//        if (document.getElementById(popupCtlID).style.visibility == 'visible' && event.srcElement != CurrentObj) {
//            document.getElementById(popupCtlID).style.visibility = 'hidden'
//            CurrentObj.src = '../Images/fleche_down_sel.gif'
//        }
//    }
//}

function isBlankDate(ctrl, msg, defaultText) {
    if (document.getElementById(ctrl)) {
        if (defaultText == "")
            defaultText = "dd-MMM-yyyy";
        if (document.getElementById(ctrl)) {
            if (trim(document.getElementById(ctrl).value, " ") == defaultText || trim(document.getElementById(ctrl).value, " ") == "") {
                CallDiv(ctrl, msg + " can not be left blank")
                document.getElementById(ctrl).focus();
                return false;
            }
        }
    }
    return true;

}

arr_month = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)
arr_days = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31)
var mm1, mm,y,year;
var minYear = 1900;
var maxYear = 2100;
var success = false;
function isDate(ctrl, msg, defaultText) {
    if (document.getElementById(ctrl)) {
        if (!(trim(document.getElementById(ctrl).value, " ") == defaultText || trim(document.getElementById(ctrl).value, " ") == "")) {
            dt = document.getElementById(ctrl).value;
            arr_mon = new Array("", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec")
            dd = dt.substring(0, dt.indexOf("-"))
            mm1 = dt.substring(dt.indexOf("-") + 1, dt.lastIndexOf("-"))
            y = dt.substring(dt.lastIndexOf("-") + 1)
            if (isNaN(dd)) {
               CallDiv(document.getElementById(ctrl).id, msg);
              return false; 
            }
            if (isNaN(y)) {
              CallDiv(document.getElementById(ctrl).id, msg);
              return false;
            }
            var c = 1;
            for (var i = 1; i < arr_mon.length; i++) {
                if (arr_mon[i].toUpperCase() == mm1.toUpperCase()) 
                {
                    success = true;
                    break;
                }
                c++;
                if (c == arr_mon.length) {
                    CallDiv(document.getElementById(ctrl).id, msg);
                    return false;
                }
            }
            year = parseInt(y)
            if (y.length != 4 || year < minYear || year > maxYear) 
            {
                CallDiv(document.getElementById(ctrl).id, msg);
                return false;
               
            }
            for (i = 1; i < 13; i++) 
            {
                if (mm1.toUpperCase().indexOf(arr_mon[i].toUpperCase(), 0) >= 0) 
                {
                    mm = i;
                    break;
                }
            }
            yy = dt.substring(dt.lastIndexOf("-") + 1, dt.length)
            if (dd < 10)
                dd = "0" + dd
            if (mm < 10)
                mm = "0" + mm
            dt = dd + "." + mm + "." + yy
            arr_dt = dt.split(".")
           
            if (parseInt(arr_dt[2], 10))
                checkLeap(arr_dt[2])

            if ((arr_dt[1] < 1) || (arr_dt[1] > 12)) {
                CallDiv(document.getElementById(ctrl).id, msg);
                return false;
            }
            for (i = 0; i < 12; i++) {
                if (arr_month[i] == arr_dt[1]) {
                    if ((arr_dt[0] > arr_days[i]) || (arr_dt[0] < 1)) {

                        CallDiv(document.getElementById(ctrl).id, msg);
                        return false;
                    }
                }
            }
        }
        if (success == true) {
            return true;
        }
        else if (success == false)
            return false;
    }
    return true;
}

//---------Check Leap Year----------

function checkLeap(year) {
    if ((eval(year) % eval(4)) == 0) {
        if ((eval(year) % eval(100)) == 0) {
            if ((eval(year) % eval(400)) == 0) {
                arr_days[1] = 29
            }
            else {
                arr_days[1] = 28
            }
        }
        else {
            arr_days[1] = 29
        }
    }
    else {
        arr_days[1] = 28
    }
}

function IsValidMinMaxLenght(ctrl, minLenght, maxLength, msg) 
{
    if (document.getElementById(ctrl)) {
        ctl = document.getElementById(ctrl);
        var numLenght = ctl.value.length;
        if (minLenght > 0 || maxLength > 0) 
        {
            if (!(numLenght >= minLenght && numLenght <= maxLength)) {
                CallDiv(ctl.id, msg);
                return false;
            }
        }
    }
    return true;
}

function CompareDates(minDate, maxDate, errMsg, allowEqual) {
    if (allowEqual == null)
        allowEqual = true;
    //date format = 31/Jan/2009 dd/Mon/yyyy
    minDate = getDateString(minDate)
    maxDate = getDateString(maxDate)
    date1 = minDate.split("/")
    minDt = new Date(date1[2], date1[1] - 1, date1[0])
    date1 = maxDate.split("/")
    maxDt = new Date(date1[2], date1[1] - 1, date1[0])
    if (allowEqual == true) {
        if (minDt > maxDt) {
            alert(errMsg)
            return false;
        }
        else
            return true;
    }
    else {
        if (minDt >= maxDt) {
            alert(errMsg)
            return false;
        }
        else
            return true;
    }
    return false
}

function getDateString(dt) {
    //converting from dd-Mon-yyyy format to dd/mm/yyyyy

    var arrDt = dt.split("-")
    if (arrDt[1].length > 2) {
        return arrDt[0] + "/" + getMonthNumber(arrDt[1]) + "/" + arrDt[2]
    }
    else {
        return arrDt[0] + "/" + arrDt[1] + "/" + arrDt[2]
    }
}

function getMonthNumber(mn) {
    if (mn == "Jan")
        return "01"
    else if (mn == "Feb")
        return "02"
    else if (mn == "Mar")
        return "03"
    else if (mn == "Apr")
        return "04"
    else if (mn == "May")
        return "05"
    else if (mn == "Jun")
        return "06"
    else if (mn == "Jul")
        return "07"
    else if (mn == "Aug")
        return "08"
    else if (mn == "Sep")
        return "09"
    else if (mn == "Oct")
        return "10"
    else if (mn == "Nov")
        return "11"
    else if (mn == "Dec")
        return "12"
}

function compareDatesWithTime(minDate, maxDate, errMsg) {
    //date format = 31/01/2009 24:00
    //if Date format is 31-Jan-2009 23:59
    if (minDate.indexOf("-") > 0) {
        var dt = minDate.split("-")
        minDate = dt[0] + "/" + getMonthNumber(dt[1]) + "/" + dt[2]
        var dt = maxDate.split("-")
        maxDate = dt[0] + "/" + getMonthNumber(dt[1]) + "/" + dt[2]
    }
    dtTime = minDate.split(" ")
    date1 = dtTime[0].split("/")
    time1 = dtTime[1].split(":")
    minDt = new Date(date1[2], date1[1] - 1, date1[0], time1[0], time1[1])

    dtTime = maxDate.split(" ")
    date1 = dtTime[0].split("/")
    time1 = dtTime[1].split(":")
    maxDt = new Date(date1[2], date1[1] - 1, date1[0], time1[0], time1[1])

    if (minDt > maxDt) {
        alert(errMsg)
        return false;
    }
    else
        return true;
    return false
}

function isSpecialCharacter(ctrl, msg) 
{
    if (document.getElementById(ctrl)) 
    {
        var iChars = "!`@#$%^&*()+=[]\\\';,/{}|\":<>?~_";
        var data = document.getElementById(ctrl).value;
        for (var i = 0; i < data.length; i++) 
        {
            if (iChars.indexOf(data.charAt(i)) != -1) 
            {
                CallDiv(ctrl, msg)
                document.getElementById(ctrl).value = "";
                return false;
            }
        }
    }
    return true;
}



//Added
function isSpecialCharacterAffidavit(ctrl, msg) {
    if (document.getElementById(ctrl)) {
        var iChars = "!`@#$%^&*()+=[]\\\';,{}|\":<>?~_";
        var data = document.getElementById(ctrl).value;
        for (var i = 0; i < data.length; i++) {
            if (iChars.indexOf(data.charAt(i)) != -1) {
                CallDiv(ctrl, msg)
                document.getElementById(ctrl).value = "";
                return false;
            }
        }
    }
    return true;
}



function CheckNumberPresent(ctrl, msg) {
    if (document.getElementById(ctrl)) {
        var iChars = "0123456789";
        var data = document.getElementById(ctrl).value;
        for (var i = 0; i < data.length; i++) {
            if (iChars.indexOf(data.charAt(i)) != -1) {
                CallDiv(ctrl, msg)
                document.getElementById(ctrl).value = "";
                return false;
            }
        }
    }
    return true;
}
// deep add on 10 march 2021
function isSelectedDrp(ctrl, errMsg) {
    if (document.getElementById(ctrl) && document.getElementById(ctrl).value == "0") {        
        alert(errMsg)
        return false;
    }
    return true;
}
//deep add end on 10 march 2021

function DateTime() {
    var today = new Date();
    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var month = monthNames[today.getMonth()];
    var Day = today.getDate();
    var Year = today.getFullYear();
    var time = today.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', second: 'numeric', hour12: true });

    document.getElementById('txt').innerHTML =
    month + " " + Day + ", " + Year + "<br/>" + time;
    var t = setTimeout(startTime, 500);
}
