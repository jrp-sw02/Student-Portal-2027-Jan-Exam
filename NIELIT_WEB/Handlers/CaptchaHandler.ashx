<%@ WebHandler Language="C#" Class="CaptchaHandler" %>

using System;
using System.Web;
using System.Drawing;
using EConnect;

public class CaptchaHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
    {
        try
        {
            CaptchaImage ci;
            if (context.Request["num"] != null)
                ci = new CaptchaImage(context.Request["num"].ToString(), 200, 50, "Century Schoolbook");
            else
                ci = new CaptchaImage(context.Session["num"].ToString(), 200, 50, "Century Schoolbook");
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            ci.Image.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            ci.Dispose();
        }
        catch (Exception ex)
        {
            context.Response.Write("Invalid requst parameters");
        }
    }
    public bool IsReusable 
    {
        get 
        {
            return false;
        }
    }

}