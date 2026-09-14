using System;
using System.Data;
using System.Data.SqlClient ;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class DisplayDocuments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string ID = Request.QueryString["ID"].ToString();
            string TYP = Request.QueryString["TYP"].ToString();
            //TYP 1->course docs
            if (!string.IsNullOrEmpty(ID))
            {
                SqlConnection con = ClassJKS.get_con2();
                SqlCommand cmd = new SqlCommand();

                if (TYP == "1")
                    cmd.CommandText = "JKS_GetCourseIDFileContents";

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(ID);

                using (con)
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())  // or (dr.HasRows)
                    {
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/pdf";
                        Response.Buffer = true;
                        if (Request.QueryString["download"].ToString() == "1")
                            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "Course File Document.pdf" + "\"");
                        var tt = (dr["course_eligibility_doc"]);

                        //byte[] bytes = Convert.FromBase64String(dr["course_eligibility_doc"].ToString());
                       // string viewfor = Convert.FromBase64String(tt.ToString());
                        byte[] bytes = Convert.FromBase64String(tt.ToString());
                        Response.BinaryWrite(bytes);
                        //Response.BinaryWrite((byte[])dr["file_content"]);
                        Response.Flush();
                        Response.End();
                    }
                    dr.Close();
                    dr.Dispose();
                    cmd.Dispose();
                }
            }
        }
        catch (Exception err)
        {
            //Response.Redirect("~/Login.aspx", false);
        }
    }
}