using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.Objects;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EConnect;
using EConnect.Utils.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class NielitCentreCourseNameWiseFilter : BasePage
{
    UserType loginUserType;
    Int32 currentRoleId = 0;
           Int32 loginUserNo = 0;
           Int64 entityID = 0;
          Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        try
        {
            if (IsSessionAlive() == false)
                //Response.Redirect("../Index.aspx");
                Response.Redirect("~/index.aspx");

            currentRoleId = Convert.ToInt32(Session["RoleID"]);            

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            loginUserType = (UserType)Session["UserType"];

            if (!Page.IsPostBack)
                {
                FillCentre();
                FillCourseCategory();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Center Course Name Wise Students", "#", ""));
                }

            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }

    #region vCode
    protected void btnReset_Click(object sender, EventArgs e)
        {
        try
            {
            Response.Redirect("~/HO/Rpt/NielitCentreCourseNameWiseFilter.aspx");
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    
    protected void FillCentre()
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
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                    ddlCentreName.Enabled = true;
                    }
                }
            else //if (UserTypeId == 10)  // centre user
                {
                ListItem lst1 = new ListItem("--Select One--", "0");
                var centreName = from s in context.NielitCentres
                                 where s.ID == entityID
                                 select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                ddlCentreName.SelectedValue = Convert.ToInt32(entityID).ToString();
                ddlCentreName.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    protected void FillCourseCategory()
        {
        try
            {
            using (DataTable dt = FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord())
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlCourseCat.DataSource = dt;
                    ddlCourseCat.DataTextField = "Name";
                    ddlCourseCat.DataValueField = "ID";
                    ddlCourseCat.DataBind();
                    ddlCourseCat.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }

            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    public DataTable FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord()
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryNielitMisAndNielitForMisReports", con))
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

    protected void FillCourse(int pCourseCategory)
        {
        try
            {
            using (DataTable dt = FillCourseNIELITMISCourseNielitCourseRecord(pCourseCategory))
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

    public DataTable FillCourseNIELITMISCourseNielitCourseRecord(int coursecatID)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("[GetCourseNielitMisAndNielitForMisReports]", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CourseCat", SqlDbType.Int));
                    cmd.Parameters["@CourseCat"].Value = coursecatID;
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
    
    protected void ddlCentreName_SelectedIndexChanged(object sender, EventArgs e)
        {
        ddlCourseCat.Items.Clear();
        ddlCourseCat.Items.Insert(0, new ListItem("--Select One--", "0"));
        FillCourseCategory();

        ddlCourseName.Items.Clear();
        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
        int Ccat = 0;
        Ccat = Convert.ToInt32(ddlCourseCat.SelectedValue);
        FillCourse(Ccat);
        }

    protected void ddlCourseCat_SelectedIndexChanged(object sender, EventArgs e)
        {
        ddlCourseName.Items.Clear();
        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
        int Ccat = 0;
        Ccat = Convert.ToInt32(ddlCourseCat.SelectedValue);
        FillCourse(Ccat);
        }
    
#endregion

    }