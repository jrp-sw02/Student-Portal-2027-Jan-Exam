using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;

public partial class Admin_DataDownloadSeqGenerationNSQF : BasePage
{
    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");

        if (!IsPostBack)
        {
            BindDataDownloadSeqNo();
            GenerateNewCaptchaImage();
        }
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception) { }
    }
    private void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Arial");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        { 
            ShowAlert(ex.Message); 
        }

    }
    private bool IsValidForm()
    {
        try
        {
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                Lblerror.Visible = true;
                Lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsValidForm())
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("NSQF_Phase_Generation_Process", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandTimeout = 120;

                    SqlParameter outP = new SqlParameter("@PFLAG_NSQF_Phase_Generation_Process_Completed", SqlDbType.Int);
                    outP.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outP);


                    SqlParameter outP1 = new SqlParameter("@rowInserted", SqlDbType.Int);
                    outP1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outP1);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        Int32 resultFlag =  Convert.ToInt32(cmd.Parameters["@PFLAG_NSQF_Phase_Generation_Process_Completed"].Value) ;
                        Int32 certificateCount = Convert.ToInt32(cmd.Parameters["@rowInserted"].Value);
                        //Int32 certificateCount = Convert.ToInt32(cmd.Parameters["@rowInserted"].Value ?? 0);
                      //  var outputValue = cmd.Parameters["@PFLAG_NSQF_Phase_Generation_Process_Completed"].Value;
                       // Int32 resultFlag = 0  ;

                       // if (outputValue != DBNull.Value)
                      //  {
                         //   resultFlag = Convert.ToInt32(outputValue);
                       //}

                        if (resultFlag == 0)
                        {
                            Lblerror.Visible = true;
                            Lblerror.Text = "Phase Generation for this Data Download Sequence No. has already  been done.";
                        }
                        else
                        {                            

                            Lblerror.Visible = true;
                            Lblerror.Text = "NSQF  phase generation and freeze process is completed. Certificate Count :" + certificateCount;
                        }
                       
                    }
                    catch (Exception ex)
                    {

                        ShowAlert("An error occurred: " + ex.Message);
                    }
                }
            }
        }

    }
    protected void BindDataDownloadSeqNo()
    {
        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("getDataDownloadSeqNoAndDate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())  // Check if there is any data
                    {
                        // Assuming the stored procedure returns a column named "data_downloaded_sequence"
                        // Adjust the column name as necessary
                        txtSeqNo.Text = reader["data_downloaded_sequence"].ToString();
                    }

                    reader.Close();


                   // txtSeqNo.Text = 

                    //ddlSeqNo.DataSource = reader;
                    //ddlSeqNo.DataTextField = "data_downloaded_sequence"; // The column you want to display
                    //ddlSeqNo.DataValueField = "data_downloaded_sequence"; // The column you want to use as the value
                    //ddlSeqNo.DataBind();

                  
                    //ddlSeqNo.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
                catch (Exception ex)
                {
                  
                   ShowAlert("An error occurred: " + ex.Message);
                }
            }
        }
    }

    //protected void ddlSeqNo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Int16 pMode = 2;
    //    Int64 dataDownloadedSeq = Convert.ToInt64(ddlSeqNo.SelectedValue);
    //    using (SqlConnection conn = new SqlConnection(constr))
    //    {
    //        using (SqlCommand cmd = new SqlCommand("getDataDownloadSeqNoAndDate", conn))
    //        {
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@pMode", pMode);
    //            cmd.Parameters.AddWithValue("@dataDownloadedSeq", dataDownloadedSeq);

    //            try
    //            {
    //                conn.Open();
    //                SqlDataReader reader = cmd.ExecuteReader();

    //                ddlDataDownloadedDate.DataSource = reader;
    //                ddlDataDownloadedDate.DataTextField = "data_download_date"; // The column you want to display
    //                ddlDataDownloadedDate.DataValueField = "data_download_date"; // The column you want to use as the value
    //                ddlDataDownloadedDate.DataBind();


    //                ddlDataDownloadedDate.Items.Insert(0, new ListItem("--Select One--", "0"));
    //            }
    //            catch (Exception ex)
    //            {

    //                ShowAlert("An error occurred: " + ex.Message);
    //            }
    //        }
    //    }
    //}
}