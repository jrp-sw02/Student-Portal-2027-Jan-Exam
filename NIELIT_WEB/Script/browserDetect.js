
// Checks the browser and adds classes to the body to reflect it.

//$(document).ready(function(){

//    console.log("browserdetect loaded");
//    var userAgent = navigator.userAgent.toLowerCase();

//    $.browser.chrome = /chrome/.test(navigator.userAgent.toLowerCase());

//    // Is this a version of IE?
//    if($.browser.msie){
//        $('body').addClass('browserIE');

//        // Add the version number
//        $('body').addClass('browserIE' + $.browser.version.substring(0,1));
//    }


//    // Is this a version of Chrome?
//    if($.browser.chrome){

//        console.log("chrome detected"); // amit test
//        $('body').addClass('browserChrome');

//        //Add the version number
//        userAgent = userAgent.substring(userAgent.indexOf('chrome/') +7);
//        userAgent = userAgent.substring(0,1);
//        $('body').addClass('browserChrome' + userAgent);

//        // If it is chrome then jQuery thinks it's safari so we have to tell it it isn't
//        $.browser.safari = false;
//    }

//    // Is this a version of Safari?
//    if($.browser.safari){
//        $('body').addClass('browserSafari');

//        // Add the version number
//        userAgent = userAgent.substring(userAgent.indexOf('version/') +8);
//        userAgent = userAgent.substring(0,1);
//        $('body').addClass('browserSafari' + userAgent);
//    }

//    // Is this a version of Mozilla?
//    if($.browser.mozilla){

//        //Is it Firefox?
//        if(navigator.userAgent.toLowerCase().indexOf('firefox') != -1){
//            $('body').addClass('browserFirefox');

//            // Add the version number
//            userAgent = userAgent.substring(userAgent.indexOf('firefox/') +8);
//            userAgent = userAgent.substring(0,1);
//            $('body').addClass('browserFirefox' + userAgent);
//        }
//        // If not then it must be another Mozilla
//        else{
//            $('body').addClass('browserMozilla');
//        }
//    }

//    // Is this a version of Opera?
//    if($.browser.opera){
//        $('body').addClass('browserOpera');
//    }


//});





// test amit new code

document.addEventListener("DOMContentLoaded", function () {

    var userAgent = navigator.userAgent.toLowerCase();
    var body = document.body;

    var isIE = /msie|trident/.test(userAgent);
    var isChrome = /chrome/.test(userAgent) && !/edge|edg|opr/.test(userAgent);
    var isSafari = /safari/.test(userAgent) && !/chrome|edge|edg|opr/.test(userAgent);
    var isFirefox = /firefox/.test(userAgent);
    var isOpera = /opera|opr/.test(userAgent);

    // IE
    if (isIE) {
        body.classList.add("browserIE");

        var match = userAgent.match(/(msie\s|rv:)(\d+)/);
        if (match && match[2]) {
            body.classList.add("browserIE" + match[2].substring(0, 1));
        }
    }

    // Chrome
    if (isChrome) {

        console.log("chrome detected");
        
        body.classList.add("browserChrome");

        var chromeIndex = userAgent.indexOf("chrome/");
        if (chromeIndex !== -1) {
            var version = userAgent.substring(chromeIndex + 7, chromeIndex + 8);
            body.classList.add("browserChrome" + version);
        }
    }

    // Safari
    if (isSafari) {
        body.classList.add("browserSafari");

        var safariIndex = userAgent.indexOf("version/");
        if (safariIndex !== -1) {
            var version = userAgent.substring(safariIndex + 8, safariIndex + 9);
            body.classList.add("browserSafari" + version);
        }
    }

    // Firefox (Mozilla)
    if (isFirefox) {
        body.classList.add("browserFirefox");

        var firefoxIndex = userAgent.indexOf("firefox/");
        if (firefoxIndex !== -1) {
            var version = userAgent.substring(firefoxIndex + 8, firefoxIndex + 9);
            body.classList.add("browserFirefox" + version);
        }
    }
    else if (/mozilla/.test(userAgent) && !isIE && !isChrome && !isSafari) {
        body.classList.add("browserMozilla");
    }

    // Opera
    if (isOpera) {
        body.classList.add("browserOpera");
    }

});