<%@ WebHandler Language="C#" Class="ViewMercyDoc" Debug="True" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

public class ViewMercyDoc : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            long id;
            if (long.TryParse(context.Request.QueryString["id"], out id))
            {
                string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string sql = "SELECT Original_File_Name, Extension, Uploaded_File FROM MercyCase_UploadedDocs WHERE [candidate_id] = @ID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string fileName = dr["Original_File_Name"] != DBNull.Value ? dr["Original_File_Name"].ToString() : "Document.pdf";
                                byte[] fileBytes = dr["Uploaded_File"] != DBNull.Value ? (byte[])dr["Uploaded_File"] : null;
                                
                                if (fileBytes != null && fileBytes.Length > 0)
                                {
                                    context.Response.Clear();
                                    context.Response.ContentType = "application/pdf";
                                    context.Response.AddHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
                                    context.Response.BinaryWrite(fileBytes);
                                    context.Response.Flush();
                                    context.Response.End();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        context.Response.ContentType = "text/plain";
        context.Response.Write("Document not found or empty.");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}
