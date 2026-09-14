using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_BookletGenerate : System.Web.UI.Page

{
    private string message = string.Empty;
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        
    }

    protected DataTable BindDatatable()
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext1"].ConnectionString);
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        cnn.Open(); 
       // string connetionString;
       // SqlConnection cnn;
      //  connetionString = @"Data Source=localhost;Initial Catalog=doeacc_accr;User ID=sa;Password=nielit@12";
       // cnn = new SqlConnection(connetionString);
        using (SqlCommand cmd = new SqlCommand("select Name,Address1,Address2,Address3,Pin,City,State,Std,Phone1,Phone2,Email,Fax,O_Status,O_Accr,convert(varchar(10),O_Validity,103) as O_Validity,A_Status,A_Accr,convert(varchar(12),A_Validity,103) as A_Validity,B_Status,B_Accr,convert(varchar(12),B_Validity,103) as B_Validity,C_Status,C_Accr,convert(varchar(12),C_Validity,105) as C_Validity,convert(varchar(10),AsOnDate,105) as AsOnDate from [doeacc_accr].dbo.approved_accr"))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = cnn;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }  
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext1"].ConnectionString);
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        cnn.Open(); 
       // string connetionString;
       // SqlConnection cnn;
      //  connetionString = @"Data Source=localhost;Initial Catalog=doeacc_accr;User ID=sa;Password=nielit@12";
       // cnn = new SqlConnection(connetionString);
       // cnn.Open();    
        try
        {
            SqlCommand cmd = new SqlCommand("BookletGeneration", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@pError", SqlDbType.Int, 1);
            cmd.Parameters["@pError"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
           //Convert.ToInt16(message)=cmd.Parameters["@pError"].Value;
            cnn.Close();
                     
           
        DataTable dt = BindDatatable();
        GridView excel = new GridView();
        excel.DataSource = dt;
        excel.DataBind();  
        
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "BookletOABC_" + DateTime.Now.ToString("ddMMyyyy") + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        excel.GridLines = GridLines.Both;
        excel.HeaderStyle.Font.Bold = true;
        excel.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());      
        Response.End();  
        Lblmessage.Text = "Successful data porting in table website_a_institute_details!!";
        Lblmessage.ForeColor = System.Drawing.Color.Red;
        //Response.End();
    }
                   
        catch (Exception ex)
        {
            Lblmessage.Text = ex.Message;
            Lblmessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            // context.Dispose();
        }
    }


}