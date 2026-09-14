using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.URM;
using EConnect.Utils.Common;
using System.Data;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

public partial class Common_VirtualAcademyPaymentStatusReportBatchWise : BasePage
    {
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int32 UserTypeId = 0;
    Int32 CourseCategory = 0;
    Int32 CourseId = 0;
    Int32 CentreID = 0;

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
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserType"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
                {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
                }
            if (!Page.IsPostBack)
                {
                FillCentreName();
                FillCategories();

                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(" Virtual Academy Batch Wise Payment Status Report ", "#", ""));
                }
            BreadCrumb1.Render();

            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }

        }
    protected void NielitCente_SelectedIndexChanged(object sender, EventArgs e)
        {
        CentreID = Convert.ToInt32(ddlNielitCente.SelectedValue);
        ddlCourseCategry.Items.Clear();
        ddlCourseCategry.Items.Insert(0, new ListItem("--Select One--", "0"));
        FillCategories();
        ddlCourseName.Items.Clear();
        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
        ddlBatchName.Items.Clear();
        ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
        }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
        {
        ddlCourseName.Items.Clear();
        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
        CentreID = Convert.ToInt32(ddlNielitCente.SelectedValue);
        CourseCategory = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        FillCourses(CourseCategory, CentreID);
        ddlBatchName.Items.Clear();
        ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
        }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
        {
        ddlBatchName.Items.Clear();
        ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
        CentreID = Convert.ToInt32(ddlNielitCente.SelectedValue);
        CourseCategory = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
        FillBatch(CentreID, CourseCategory, CourseId);
        }
    protected void btnReset_Click1(object sender, EventArgs e)
        {
        try
            {
            BreadCrumb1.Render();
            ddlNielitCente.Items.Clear();
            ddlNielitCente.Items.Insert(0, new ListItem("--Select One--", "0"));
            FillCentreName();
            ddlCourseCategry.Items.Clear();
            ddlCourseCategry.Items.Insert(0, new ListItem("--Select One--", "0"));
            FillCategories();
            ddlCourseName.Items.Clear();
           // ddlCourseName.Items.Insert(0, "--Select One--");
            ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
            ddlBatchName.Items.Clear();
            ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }

    #region New
    //Batch
    protected void FillBatch(int pCentreId, int pCourseCategory, int pCourseId)
        {
        try
            {
            using (DataTable dt = FillBatchRecords(pCentreId, pCourseCategory, pCourseId))
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlBatchName.DataSource = dt;
                    ddlBatchName.DataTextField = "Name";
                    ddlBatchName.DataValueField = "ID";
                    ddlBatchName.DataBind();
                    ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                BreadCrumb1.Render();
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    public DataTable FillBatchRecords(int pCentreId, int pCourseCategory, int pCourseId)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetVirtualAcademyBatchRecordsForBatchWisePaymentStatusReport", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd.Parameters.Add("@pUserId", SqlDbType.BigInt);
                    cmd.Parameters["@pUserId"].Value = UserTypeId;
                    cmd.Parameters.Add("pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["pCentreId"].Value = pCentreId;
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;
                    cmd.Parameters.Add("@pCourseId", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseId"].Value = pCourseId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }
    //Course
    protected void FillCourses(int pCourseCategory, int pCentreId)
        {
        try
            {
            using (DataTable dt = FillCourseRecords(pCourseCategory, pCentreId))
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlCourseName.DataSource = dt;
                    ddlCourseName.DataTextField = "Name";
                    ddlCourseName.DataValueField = "ID";
                    ddlCourseName.DataBind();
                    ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                BreadCrumb1.Render();
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    public DataTable FillCourseRecords(int pCourseCategory, int pCentreId)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseForBatchWisePaymentStatusReport", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd.Parameters.Add("@pUserId", SqlDbType.BigInt);
                    cmd.Parameters["@pUserId"].Value = UserTypeId;
                    cmd.Parameters.Add("pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["pCentreId"].Value = pCentreId;
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }
    //Course Category 
    protected void FillCategories()
        {
        try
            {
            using (DataTable dt = FillCourseCategoryRecords())
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlCourseCategry.DataSource = dt;
                    ddlCourseCategry.DataTextField = "Name";
                    ddlCourseCategry.DataValueField = "ID";
                    ddlCourseCategry.DataBind();
                    ddlCourseCategry.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    public DataTable FillCourseCategoryRecords()
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetVirtualAcademyCourseCategoryForBatchWisePaymentStatustReport", con))
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
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }
    //centreName
    protected void FillCentreName()
        {
        try
            {
            using (NIELITMISContext context = new NIELITMISContext())
                {
                if (UserTypeId == 6)  // ho user
                    {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var centreName1 = from s in context.NielitCentres
                                      //where s.ID == entityID
                                      select new { ValueField = s.ID, TextField = s.Name };
                    if (centreName1 != null)
                        {
                        var centreName = centreName1.OrderBy(i => i.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitCente, centreName.Distinct(), lst1);
                        }
                    }
                else if (UserTypeId == 10)  // centre user
                    {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var centreName = from s in context.NielitCentres
                                     where s.ID == entityID
                                     select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitCente, centreName.Distinct(), lst1);
                    ddlNielitCente.SelectedValue = Convert.ToInt32(entityID).ToString();
                    ddlNielitCente.Enabled = false;
                    }
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    #endregion
    }
