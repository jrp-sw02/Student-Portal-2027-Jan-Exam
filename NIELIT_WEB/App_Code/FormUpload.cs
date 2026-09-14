using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Generic;

/// <summary>
/// Summary description for FormUpload
/// </summary>
public static class FormUpload
{
    private static readonly Encoding encoding = Encoding.UTF8;
    public static HttpWebResponse MultipartFormPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string headerkey, string headervalue)
    {
        string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
        string contentType = "multipart/form-data; boundary=" + formDataBoundary;
        byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
        return PostForm(postUrl, userAgent, contentType, formData, headerkey, headervalue);
    }
    private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string headerkey, string headervalue)
    {
        HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
        if (request == null)
        {
            throw new NullReferenceException("request is not a http request");
        }

        // Set up the request properties.  
        request.Method = "POST";
        request.ContentType = contentType;
        request.UserAgent = userAgent;
        request.CookieContainer = new CookieContainer();
        request.ContentLength = formData.Length;

        //Add header if needed  
        request.Headers.Add(headerkey, headervalue);
        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(formData, 0, formData.Length);
            requestStream.Close();
        }

        return request.GetResponse() as HttpWebResponse;
    }

    private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
    {
        Stream formDataStream = new System.IO.MemoryStream();
        bool needsCLRF = false;
        foreach (var param in postParameters)
        {
            if (needsCLRF)
                formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
            needsCLRF = true;
            if (param.Value is FileParameter)
            {
                FileParameter fileToUpload = (FileParameter)param.Value;
                string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                    boundary,
                    param.Key,
                    fileToUpload.FileName ?? param.Key,
                    fileToUpload.ContentType ?? "application/octet-stream");
                formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
            }
            else
            {
                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
            }
        }
        string footer = "\r\n--" + boundary + "--\r\n";
        formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
        formDataStream.Position = 0;
        byte[] formData = new byte[formDataStream.Length];
        formDataStream.Read(formData, 0, formData.Length);
        formDataStream.Close();
        return formData;
    }

    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }

    // Convert Json to Table
    public static DataTable JSONToDataTable(string JSONData)
    {
        DataTable dt = new DataTable();
        string[] jsonStringArray = Regex.Split(JSONData.Replace("/", "").Replace("/", ""), "},{");
        List<string> ColumnsName = new List<string>();
        foreach (string strJSONarr in jsonStringArray)
        {
            string[] jsonStringData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
            foreach (string ColumnsNameData in jsonStringData)
            {
                try
                {
                    int idx = ColumnsNameData.IndexOf(":");
                    string ColumnsNameString = ColumnsNameData.Substring(0, idx).Replace("\"", "").Trim();
                    if (!ColumnsName.Contains(ColumnsNameString))
                    {
                        ColumnsName.Add(ColumnsNameString);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                }
            }
            break;
        }
        foreach (string AddColumnName in ColumnsName)
        {
            dt.Columns.Add(AddColumnName);
        }
        foreach (string strJSONarr in jsonStringArray)
        {
            string[] RowData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
            DataRow nr = dt.NewRow();
            foreach (string rowData in RowData)
            {
                try
                {
                    int idx = rowData.IndexOf(":");
                    string RowColumns = rowData.Substring(0, idx).Replace("\"", "").Trim();
                    string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                    nr[RowColumns] = RowDataString;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            dt.Rows.Add(nr);
        }
        return dt;
    }
}