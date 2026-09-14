using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_TrainingCalendarReportFilter : BasePage
{

    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;   
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        try
        {
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);           

            if (!IsPostBack)
            {
                
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }        
    }

    public void bindCentre()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                var CentreName = from c in context.NielitCentres
                                 select new { ValueField = c.ID, TextField = c.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentre, CentreName.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindCourseData()
    {
        try
        {
            using (DataTable dt = GetNielitCourse())
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetNielitCourse()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetNIELITMISCourseName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void Rdsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rdsearchby.SelectedValue == "C")
        {
            TrCentre.Visible = true;
            TrcourseInput.Visible = false;
            txtdateFrom.Text = "";
            txtdateto.Text = "";
            bindCentre();
        }
        else if (Rdsearchby.SelectedValue == "Co")
        {
            TrCentre.Visible = false;
            TrcourseInput.Visible = true;
            txtdateFrom.Text = "";
            txtdateto.Text = "";
            BindCourseData();
        }
        else if (Rdsearchby.SelectedValue == "A")
        {
            TrCentre.Visible = false;
            TrcourseInput.Visible = false;
            txtdateFrom.Text = "";
            txtdateto.Text = "";
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCentre.SelectedValue = "0";
            ddlCourse.SelectedValue = "0";          
            txtdateFrom.Text = "";
            txtdateto.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

}