
//---------------------------------------------------------------------------------------

function compare_join_dates(source, args) {
    //alert("t0");
    var fromDate = new Date();
    var txtFromDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_emp_fr_date").value;
    //'<%= TextBox1.ClientID %>'
    var aFromDate = txtFromDate.split("/");

    /*Start 'Date to String' conversion block, this block is required because javascript do not provide any direct function to convert 'String to Date' */

    var fdd = aFromDate[0]; //get the day part
    var fmm = aFromDate[1]; //get the month part
    var fyyyy = aFromDate[2]; //get the year part

    fromDate.setUTCDate(fdd);
    fromDate.setUTCMonth(fmm - 1);
    fromDate.setUTCFullYear(fyyyy);

    parseInt(fyyyy + fmm + fdd);

    //alert("t1: "+fdd+","+fmm+","+fyyyy);

    var toDate = new Date();
    var txtToDate = document.getElementById("ctl00_ContentPlaceHolder1_txt_emp_to_date").value;

    //ctl00_ContentPlaceHolder1_txt_to_date
    var aToDate = txtToDate.split("/");
    var tdd = aToDate[0]; //get the day part
    var tmm = aToDate[1]; //get the month part
    var tyyyy = aToDate[2]; //get the year part

    toDate.setUTCDate(tdd);
    toDate.setUTCMonth(tmm - 1);
    toDate.setUTCFullYear(tyyyy);

    var td = parseInt(tyyyy + tmm + tdd);

    if (toDate != '' && fromDate > toDate) {
        //alert("Invalid");
        args.IsValid = false;
    }
    else {
        //alert("valid");
        args.IsValid = true;
    }
}

//---------------------------------------------------------------------------------------
