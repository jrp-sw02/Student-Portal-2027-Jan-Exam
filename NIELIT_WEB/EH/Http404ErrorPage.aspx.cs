using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.Utils.Common;

public partial class Http404ErrorPage : System.Web.UI.Page
{
    protected HttpException ex = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Log the exception and notify system operators
        Response.StatusCode = 404;
        Response.StatusDescription = "Not Found";
        Response.TrySkipIisCustomErrors = true;
        ex = new HttpException("HTTP 404");
        ExceptionUtility.LogException(ex, "Caught in Http404ErrorPage");
        ExceptionUtility.NotifySystemOps(ex);
    }
}