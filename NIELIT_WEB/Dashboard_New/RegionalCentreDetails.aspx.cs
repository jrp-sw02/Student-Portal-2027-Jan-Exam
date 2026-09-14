using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class RegionalCentreDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
           // string path = HttpContext.Current.Request.Url.PathAndQuery;

            //string path = HttpContext.Current.Request.QueryString["querystring"];
            // Count of Certificates yearly
          //  parameters vPara = new parameters();
           // vPara.count = 1;
           // vPara.pInstituteId =Convert.ToInt64( EncryptDecryptD.Decrypt(path));
             //if (Request.QueryString["pInstituteID"] !=  null )
           // {
                Int64 paramVal =   Convert.ToInt64(Request.QueryString["pInstituteID"]);
            //}

             using (SqlConnection con = new SqlConnection(CS))
             {
                 using (SqlCommand cmd =  new SqlCommand("getNIELITCentreDetails",con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@pInstituteId", paramVal);
                     con.Open();

                     DataTable dt = new DataTable();
                     using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                     {
                         da.Fill(dt);
                     }
                     grdCentre.DataSource = dt;
                     grdCentre.DataBind();
                 }
             }
           
            //DataSet ds = new DataSet();
            //ds = utility.executeProcedure("getNIELITCentreDetails", vPara);
           
        }

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("NIELITCentres.aspx");
    }
}