<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using EConnect.NIELIT;
using EConnect;
using EConnect.DAL;
using System.Linq;
using System.Drawing;
using System.IO;
public class ImageHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            Int64 applID = 0;
            enmApplicationType applType = enmApplicationType.CourseRegistrationApplication;
            //img = "P", "t", "s"
            if (context.Request["ID"] != null && context.Request["ApplType"] != "" && context.Request["imgType"] !="")
            {
                applID = Convert.ToInt64(context.Request.QueryString["ID"]);
                applType = (enmApplicationType)Convert.ToInt32(context.Request["ApplType"]);
                string imageType = context.Request["imgType"].ToString().ToLower();
                using (EConnectContext eContext = new EConnectContext())
                {
                    if (applType == enmApplicationType.CourseRegistrationApplication)
                    {
                        CourseRegistrationApplication appl = eContext.CourseRegistrationApplications.Find(applID);
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        if (imageType == "p")
                        {
                            context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", appl.PhotoFileName));
                            context.Response.AddHeader("content-length", appl.Photo.Length.ToString());
                            context.Response.BinaryWrite(appl.Photo);
                        }
                        else if (imageType == "t")
                        {
                            context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", appl.LeftThumbFileName));
                            context.Response.AddHeader("content-length", appl.LeftThumb.Length.ToString());
                            context.Response.BinaryWrite(appl.LeftThumb);
                        }
                        else if (imageType == "s")
                        {
                            context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", appl.SignatureFileName));
                            context.Response.AddHeader("content-length", appl.Signature.Length.ToString());
                            context.Response.BinaryWrite(appl.Signature);
                        }
                        
                        //context.Response.ContentType = "application/pdf";
                        
                    }
                    else if (applType == enmApplicationType.CertificateExamApplication)
                    {
                        CertificateExamApplication appl = eContext.CertificateExamApplications.Find(applID);
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        if (imageType == "p")
                        {
                            context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", appl.PhotoFileName));
                            context.Response.AddHeader("content-length", appl.Photo.Length.ToString());
                            context.Response.BinaryWrite(appl.Photo);
                        }
                        else if (imageType == "t")
                        {
                            context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", appl.LeftThumbFileName));
                            context.Response.AddHeader("content-length", appl.LeftThumb.Length.ToString());
                            context.Response.BinaryWrite(appl.LeftThumb);
                        }
                        else if (imageType == "s")
                        {
                            context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", appl.SignatureFileName));
                            context.Response.AddHeader("content-length", appl.Signature.Length.ToString());
                            context.Response.BinaryWrite(appl.Signature);
                        }
                        
                    }
                    
                    //}
                };

               
                
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("Invalid requst parameters");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}