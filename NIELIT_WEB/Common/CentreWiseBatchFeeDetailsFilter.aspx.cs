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

public partial class Common_CentreWiseBatchFeeDetailsFilter : System.Web.UI.Page
{

     UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 NielitCentreId = 0;
    Int32 NielitCentreIdFilter = 0;
    Int32 UserTypeId = 0;
    string centre = "";
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
            //UserTypeId = 10;
            //currentRoleId = 21;
            //loginUserNo = 285716;
            //loginUserType = 10;

            if (!IsPostBack)
            {
                bindCentre();
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }

      

      //  bindCentre();
    }


    public void bindCentre()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                User objUser;
                using (EConnectContext context1 = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                }
                
                   
               
                    string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                    // centre = "Select * from NielitCentres where id= '" + NielitCentreId + "'";
		    centre = "Select * from NielitCentres where id= @NielitCentreId";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = centre;
                    cmd.Parameters.AddWithValue("@NielitCentreId", Convert.ToInt32(NielitCentreId));
                        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                        adpt.Fill(dt);
                        ddlCentreName.DataSource = dt;
                        ddlCentreName.DataBind();
                        ddlCentreName.DataTextField = "Name";
                        ddlCentreName.DataValueField = "ID";
                        ddlCentreName.DataBind();
                        ddlCentreName.Enabled = false;

                    }
                    if (dt.Rows.Count==0)
                {
                    ListItem lst = new ListItem("--Select--", "0");
                    var CentreName = from c in context.NielitCentres
                                     select new { ValueField = c.ID, TextField = c.Name };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, CentreName.Distinct(), lst);
                }

                    if (UserTypeId == 1 || UserTypeId == 6)
                    {
                        ddlCentreName.Enabled = true;

                    }
                    else
                    {
                        ddlCentreName.Enabled = false;
                    }
            };
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


    public DataTable FillBatchNIELITMISCourse()
    {
        User objUser;
        using (EConnectContext context1 = new EConnectContext())
        {
            objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
        }
        NielitCentreIdFilter = Convert.ToInt32(NielitCentreId);
        Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("FillBatchNIELITMISCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@NielitCentreIdFilter", SqlDbType.Int));
                    cmd.Parameters["@NielitCentreIdFilter"].Value = NielitCentreIdFilter;
                    cmd.Parameters.Add(new SqlParameter("@coursenameid", SqlDbType.Int));
                    cmd.Parameters["@coursenameid"].Value = coursenameid;
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


    protected void BindListData()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                using (DataTable dt = FillBatchNIELITMISCourse())
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlBatch.DataSource = dt;
                        ddlBatch.DataTextField = "Name";
                        ddlBatch.DataValueField = "ID";
                        ddlBatch.DataBind();
                        ddlBatch.Items.Insert(0, new ListItem("--All--", "99999"));
                    }
                }

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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCentreName.SelectedValue = "0";
            ddlReportType.SelectedValue = "0";
            ddlCourse.SelectedValue = "0";
            ddlBatch.SelectedValue = "0";
            txtBatchFrom.Text = "";
            txtBatchto.Text = "";

        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message, true);
        }
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue == "C")
        {
            Trcourse.Visible = true;
            TrcourseInput.Visible = true;
            trBatchdate.Visible = false;
            trbatchfrom.Visible = false;
            BindListData();
        }
        else if (ddlReportType.SelectedValue == "D")
        {
            trBatchdate.Visible = true;
            trbatchfrom.Visible = true;
            Trcourse.Visible = false;
            TrcourseInput.Visible = false;
        }
    }



    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            }
            NielitCentreIdFilter = Convert.ToInt32(NielitCentreId);
            ListItem lst = new ListItem("--All--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
                if (coursenameid != 0)
                {

                    using (DataTable dt = FillBatchNIELITMISCourse())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ddlBatch.DataSource = dt;
                            ddlBatch.DataTextField = "Name";
                            ddlBatch.DataValueField = "ID";
                            ddlBatch.DataBind();
                            ddlBatch.Items.Insert(0, new ListItem("--All--", "99999"));
                        }
                        else
                        {
                            ddlBatch.Items.Clear();
                            ddlBatch.Items.Insert(0, new ListItem("--All--", "99999"));
                        }
                    }
                }
                else
                {
                    using (DataTable dt = FillBatchNIELITMISCourse())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ddlBatch.DataSource = dt;
                            ddlBatch.DataTextField = "Name";
                            ddlBatch.DataValueField = "ID";
                            ddlBatch.DataBind();
                            ddlBatch.Items.Insert(0, new ListItem("--Select One--", "0"));
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message, true);
        }
    }
}
