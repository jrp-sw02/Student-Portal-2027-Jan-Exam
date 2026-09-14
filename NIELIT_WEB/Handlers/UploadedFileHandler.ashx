<%@ WebHandler Language="C#" Class="UploadedFileHandler" %>
using System;
using System.Web;
using EConnect.NIELIT;
using EConnect;
using EConnect.DAL;
using System.Linq;
using System.Drawing;
using System.IO;
public class UploadedFileHandler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            UploadedFile imageFile;
            Int64 fileID = 0;

            //Added 4 Feb 2019
            string prevPage = "";
            prevPage = context.Request.UrlReferrer.ToString();
            if (prevPage == null)
                return;

            if (context.Request["ID"] != null && prevPage != null)
          //  if (context.Request["ID"] != null)
                fileID = Convert.ToInt64(context.Request.QueryString["ID"]);
            using (EConnectContext eContext = new EConnectContext())
            {
                imageFile = eContext.UploadedFiles.Find(fileID);

                //if (imageFile.Extension.ToLower() == ".jpg" || imageFile.Extension.ToLower() == ".jpeg")
                //{
                //    MemoryStream m = new MemoryStream(imageFile.BlobFile);
                //    Bitmap gifImage = new Bitmap(m);
                //    context.Response.Clear();
                //    context.Response.ContentType = "image/jpeg";
                //    gifImage.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //    gifImage.Dispose();
                //    m.Dispose();
                //}
                //else
                //{
                    context.Response.Clear();
                    context.Response.AddHeader("content-disposition", string.Format("inline;filename={0}", imageFile.OriginalName));
                    context.Response.AddHeader("content-length", imageFile.BlobFile.Length.ToString());
                    //context.Response.ContentType  =  "application/octet-stream";
                    context.Response.ContentType ="application/"+ imageFile.Extension.ToLower().Replace(".", "");
                    //context.Response.ContentType = "application/pdf";
                    context.Response.BinaryWrite(imageFile.BlobFile);
                //}
            };
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