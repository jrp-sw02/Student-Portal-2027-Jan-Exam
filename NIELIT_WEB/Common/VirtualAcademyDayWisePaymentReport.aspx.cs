using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.URM;
using EConnect.Utils.Common;
using System.Collections.Generic;
using System.Data;
using EConnect.NIELIT;
using System.Web;
using System.Data.Objects;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Common_VirtualAcademyDayWisePaymentReport : BasePage
{
    String strMessage = string.Empty;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
            }
            if (!Page.IsPostBack)
            {
                txttDateFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtDateto.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                FillCategories();
                FillPaymentMode();
                ddlpaymentmode.Enabled = false;
                FillTransactionType();
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Day Wise Payment Report ", "#", ""));
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected void BindApplicationType()
    {
        try
        {
        Int32 ccID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        if (ccID < 101) // only for NIELIT DB
            {
            using (EConnectContext context = new EConnectContext())
                {
                Int32 courseTypeID = 0;
                Int32 courseIDForNIELIT = 0;
                Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                if (CourseId > 0)
                    {                 
                    using (NIELITMISContext contextmis = new NIELITMISContext())
                        {
                        courseIDForNIELIT = contextmis.NielitCourseDurations.Find(CourseId).courseID;
                        };
                    if (courseIDForNIELIT != 0)
                        {
                        courseTypeID = context.Courses.Where(a => a.ID == courseIDForNIELIT).FirstOrDefault().CourseTypeID;
                        }
                    var application = from s in context.ApplicationTypes
                                      //where s.CourseTypeID == courseTypeID
                                      where s.CourseTypeID == 1 && s.ID == 1   // only for registration
                                      select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, application, lst);
                    ddlAppType.SelectedValue = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication).ToString();
                    //ddlAppType.Enabled = false;
                    }
                };
            }
        else  // only for NIELITMIS DB
            {
            using (EConnectContext context = new EConnectContext())
                {
                Int32 courseTypeID = 0;
                Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                if (CourseId > 0)
                    {
                    //courseTypeID = context.Courses.Where(a => a.ID == CourseId).FirstOrDefault().CourseTypeID;                    
                    var application = from s in context.ApplicationTypes
                                      //where s.CourseTypeID == courseTypeID

                                      // fixed for formal and non formal courses of NIELITMIN DB
                                      where s.CourseTypeID == 1 && s.ID == 1   // only for registration
                                      select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, application, lst);
                    ddlAppType.SelectedValue = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication).ToString();
                    //ddlAppType.Enabled = false;
                    }
                };

            //using (EConnectContext context = new EConnectContext())
            //    {
            //    using (NIELITMISContext contextmis = new NIELITMISContext())
            //        {
            //        Int32 courseTypeID1 = 0;
            //        Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            //        ListItem lst = new ListItem("--Select One--", "0");
            //        if (CourseId > 0)
            //            {
            //            var courseTypeID11 = from nc in contextmis.NielitCentreCourses
            //                                 join nd in contextmis.NielitCourseDurations on nc.ID equals nd.courseID
            //                                 where nd.courseID == CourseId
            //                                 select new { ValueField = nd.ID, TextField = nc.Name};
            //            //select new { ValueField = nd.ID, TextField = nc.Name + '(' + nd.courseDurationDays.ToString() + "Days-" + nd.courseDurationHrs.ToString() + "Hrs)" };
            //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, courseTypeID11, lst);
            //            //ddlAppType.SelectedValue = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication).ToString();   // to select registration type
            //            }
            //        };
            //    };
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void FillPaymentMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 multicheque = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
                Int32 cash = Convert.ToInt32(enmPaymentMode.Cash);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID != multicheque && p.ID != cash
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlpaymentmode, Category, lst);
                ddlpaymentmode.SelectedValue = Convert.ToInt32(enmPaymentMode.Online).ToString();  // To select the online payment option
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillTransactionType()
    {
        try
        {
            EnumUtility.BindListObject(ref ddltytype, typeof(EConnect.enmDemandNoteType), new ListItem("--All--", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {

        //Report rpt = new Report();
        ////rpt.Draftno = txtDraftno.Text;
        //rpt.DateFrom = TxtDateFrom.Text;
        ////rpt.Dateto = TxtDateto.Text;
        ////rpt.TransactionNo = TxtTransno.Text;
        //rpt.Coursecategory = ddlCourseCategry.SelectedValue.ToString();
        //rpt.CourseName = TxtCourseName.Text;
        //rpt.FeeType = ddlFeeType.SelectedValue.ToString();

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        ddlAppType.Items.Clear();
        ddlAppType.Items.Insert(0, "--Select One--");

        ddlCourseName.Items.Clear();
        ddlCourseName.Items.Insert(0, "--Select One--");
        Int32 ccID = 0;
        ccID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        FillCourses(ccID);

    }
    protected void btnReset_Click1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            txttDateFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtDateto.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //ddlCourseCategry.SelectedValue = "0";
            FillCategories();
            //ddlCourseCategry_SelectedIndexChanged(ddlCourseCategry.SelectedValue, EventArgs.Empty);            
            //FillPaymentStatus();
            FillTransactionType();
            //ddlpaymentmode.SelectedValue = "0";
            FillPaymentMode();
            ddlpaymentmode.Enabled = false;
            ddlpaymentstatus.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddltytype.SelectedValue = "0";
            if (ddldatetype.Visible == true)
                ddldatetype.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    Int32 paymentmode = Convert.ToInt32(ddlpaymentmode.SelectedValue);
        //    if ((paymentmode == Convert.ToInt32(enmPaymentMode.Online)) || (paymentmode == Convert.ToInt32(enmPaymentMode.NEFTRTGS)))
        //    {
        //        Lbdatetype.Visible = true;
        //        ddldatetype.Visible = true;
        //    }
        //    else
        //    {
        //        Lbdatetype.Visible = false;
        //        ddldatetype.Visible = false;
        //    }
        //    ddldatetype.SelectedValue = "0";
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message);
        //}

    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAppType.Items.Clear();
        ddlAppType.Items.Insert(0, "--Select One--");
        BindApplicationType();      
    }
    protected void btnView_Click1(object sender, EventArgs e)
        {

        }

    #region Vishal    
    //Course
    protected void FillCourses(int pCourseCategory)
        {
        try
            {
            using (DataTable dt = FillCourseRecords(pCourseCategory))
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
    public DataTable FillCourseRecords(int pCourseCategory)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseForDayWisePaymentReport", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
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
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseCategoryForDayWisePaymentReport", con))
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
    #endregion

}
