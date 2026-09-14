<%@ WebHandler Language="C#" Class="BarcodeHandler" %>

using System;
using System.Web;
using EConnect.NIELIT;
using EConnect;
using EConnect.DAL;
using System.Linq;
using System.Drawing;
using System.IO;
public class BarcodeHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
    {
            if (context.Request.QueryString["Code"] != null)
            {
                String barCode = context.Request.QueryString["Code"];
                iTextSharp.text.pdf.Barcode128 barCodeImage = new iTextSharp.text.pdf.Barcode128();
                barCodeImage.CodeType = iTextSharp.text.pdf.Barcode.CODABAR;
                barCodeImage.ChecksumText = true;
                barCodeImage.GenerateChecksum = true;
                barCodeImage.StartStopText = true;
                barCodeImage.Code = barCode;

                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                barCodeImage.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White).Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
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