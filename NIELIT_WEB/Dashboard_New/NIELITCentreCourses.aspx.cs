using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class NIELITCentreCourses : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        getNIELITCentreCoursesByID();
           

            }


    protected void getNIELITCentreCoursesByID()
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
            //string path = HttpContext.Current.Request.Url.PathAndQuery;
            //string Id = Request.QueryString["pInstituteID"].ToString();
            //  string Id = Convert.ToString(EncryptDecryptD.Decrypt(path));
            // parameters vPara = new parameters();
            // DataSet ds = new DataSet();
            Int64 ID = Convert.ToInt64(Request.QueryString["pInstituteID"]);

            using (SqlCommand cmd = new SqlCommand("getNIELITCentreCoursesByID", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pID", ID);
                con.Open();

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                {
                    // lblCentre.Text = ds.Tables[0].Rows[0][0].ToString();
                    lblCentre.Text = dt.Rows[0][0].ToString();
                    lblCentre.Text = lblCentre.Text;
                    lblCentre.Visible = true;
                    lblCentre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00008B");


                }
                con.Close();

                using (SqlCommand cmd2 = new SqlCommand("getNIELITCentreCourses", con))
                {
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@pInstituteId", ID);
                    con.Open();

                    DataTable dt2 = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd2))
                    {
                        da.Fill(dt2);
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        grdCentreCourse.DataSource = dt2;
                        grdCentreCourse.DataBind();
                    }
                   
                }
            }
        }
    }
   
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrainingPartners.aspx");
    }
    protected void grdCentreCourse_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCentreCourse.PageIndex = e.NewPageIndex;
        getNIELITCentreCoursesByID ();
       // GridView1.Visible = false;
        btnBack.Visible = true;
        grdCentreCourse.DataBind();

    }
}