using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_Targetmas : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");


            currentRoleId = Convert.ToInt32(Session["RoleID"]);


            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
                fillfrequency();
            }


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected bool isValidForm()
    {
        if (String.IsNullOrEmpty(txttargetname.Text))
        {
            ShowAlert("Enter Target name");
            return false;
        }

        if (ddlfrequency.SelectedIndex == 0)
        {
            ShowAlert("Please Choose Frequency");
            return false;
        }

        return true;
    }


    protected void fillfrequency()
    {
        try
        {
            ListItem lst1 = new ListItem("-Select One-", "0");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                conn.Open();
                string sql = @"select * from nielitmis.dbo.frequency order by id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        ddlfrequency.DataSource = ds.Tables[0];
                        ddlfrequency.DataTextField = "Name";
                        ddlfrequency.DataValueField = "ID";
                        ddlfrequency.DataBind();
                        ddlfrequency.Items.Insert(0, lst1);
                        ddlfrequency.SelectedIndex = 0;


                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (!isValidForm())
            {
                return;
            }

            string targetName = txttargetname.Text.Trim();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                conn.Open();
                
                string sql = "select count(1) from targetMas where targetname=@targetName";

                using (var cmdcheck = new SqlCommand(sql, conn))
                {
                    cmdcheck.Parameters.AddWithValue("@targetName", targetName);
                    int existingCount = (int)cmdcheck.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        // The target name already exists. Show error and stop execution.
                        ShowAlert("Error: A target with the name " + targetName + " already exists.");
                        txttargetname.Text = "";
                        return; 
                    }
                }

                using (var cmd = new SqlCommand("insert_update_targetMas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@targetID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@targetName", txttargetname.Text.Trim());
                    cmd.Parameters.AddWithValue("@targetFrequency", ddlfrequency.SelectedValue);
                    cmd.Parameters.AddWithValue("@fileuploadreqd", radflupload.SelectedValue == "1");
                    cmd.Parameters.AddWithValue("@actionID", "save");
                    cmd.Parameters.AddWithValue("@userId", Convert.ToInt64(Session["UserID"]));

                    cmd.ExecuteNonQuery();

                    ShowAlert("Target created successfully!");

                }

            }


            btnreset_Click(null, e);

        }
        catch (Exception ex)
        {
            ShowAlert("Error Occurred");
        }
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txttargetname.Text = "";
            ddlfrequency.SelectedIndex = 0;
            radflupload.SelectedValue = "0";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("../frmDashBoard.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}