  <%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
        Application["LoginUsersList"] = LoginUsersList;

        Dictionary<String, HttpSessionState> sessionData =
               new Dictionary<String, HttpSessionState>();
        Application["s"] = sessionData;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    //void Application_Error(object sender, EventArgs e) 
    //{
    //    //// Code that runs when an unhandled error occurs

    //    // Get the exception object.
    //    Exception exc = Server.GetLastError();

    //    // Handle HTTP errors
    //    if (exc.GetType() == typeof(HttpException))
    //    {
    //        // The Complete Error Handling Example generates
    //        // some errors using URLs with "NoCatch" in them;
    //        // ignore these here to simulate what would happen
    //        // if a global.asax handler were not implemented.
    //        if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
    //            return;

    //        //Redirect HTTP errors to HttpError page
    //        //Server.Transfer("EH/HttpErrorPage.aspx");
    //    }

    //    // For other kinds of errors give the user some information
    //    // but stay on the default page
    //    Response.Write("<h2>Global Page Error</h2>\n");
    //    Response.Write(
    //        "<p>" + exc.Message + "</p>\n");
    //    Response.Write("Return to the <a href='Home.aspx'>" +
    //        "Home Page</a>\n");
    //    // Log the exception and notify system operators
    //    ExceptionUtility.LogException(exc, "DefaultPage");
    //    ExceptionUtility.NotifySystemOps(exc);

    //    // Clear the error from the server
    //    Server.ClearError();
    //}

    void Session_Start(object sender, EventArgs e)
    {
        Session["LogID"] = "0";
        try
        {
            Dictionary<String, HttpSessionState> sessionData = (Dictionary<String, HttpSessionState>)Application["s"];

            if (sessionData.ContainsKey(HttpContext.Current.Session.SessionID))
            {
                sessionData.Remove(HttpContext.Current.Session.SessionID);
                sessionData.Add(HttpContext.Current.Session.SessionID,
                                HttpContext.Current.Session);
            }
            else
            {
                sessionData.Add(HttpContext.Current.Session.SessionID,
                                HttpContext.Current.Session);
            }
            Application["s"] = sessionData;

            using (EConnect.DAL.EConnectContext context = new EConnect.DAL.EConnectContext())
            {
                EConnect.HRMS.Organization org = context.Organizations.Find(1);
                org.VisitorCounter = org.VisitorCounter + 1;
                HttpContext.Current.Session["counter"] = org.VisitorCounter;
                //context.SaveChanges();
            };
        }
        catch (Exception)
        {

        }
        //// Code that runs when a new session is started
        HttpCookie mycookie;
        mycookie = new HttpCookie(HttpContext.Current.Response.Cookies["NET_SessionId"].Name, HttpContext.Current.Response.Cookies["NET_SessionId"].Value);
        HttpContext.Current.Response.Cookies.Remove("NET_SessionId");
        mycookie.Path = "/student.nielit.gov.in/";
        HttpContext.Current.Response.Cookies.Add(mycookie);
    }

    void Session_End(object sender, EventArgs e)
    {
        try
        {
            Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
            Dictionary<string, HttpSessionState> sessionData = (Dictionary<string, HttpSessionState>)Application["s"];
            if (sessionData.ContainsKey(Session.SessionID))
                sessionData.Remove(Session.SessionID);
            Application["s"] = sessionData;
            LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];
            int logID = Convert.ToInt32(Session["logID"]);
            int userID = Convert.ToInt32(Session["UserID"]);
            if (LoginUsersList.ContainsValue(logID))
                LoginUsersList.Remove(userID);
            EConnect.DAL.EConnectContext context = new EConnect.DAL.EConnectContext();
            EConnect.URM.LoginLog log = context.LoginLogs.Find(logID);
            log.LogoutTime = DateTime.Now;
            context.Entry(log).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            context.Dispose();
        }
        catch (Exception)
        {

        }
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.


    }

    protected void Application_EndRequest(object sender, EventArgs e)
    {
        if (Response.StatusCode == 403)
        {
            Response.Clear();
            Response.TrySkipIisCustomErrors = false;
            Response.StatusCode = 404;
            Response.StatusDescription = "Not Found";
            Context.ApplicationInstance.CompleteRequest();

        }
    }

</script>
