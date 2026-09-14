using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Spreadsheet;
using EConnect;
using EConnect.DAL;
using EConnect.URM;
using EConnect.Utils.Common;

public partial class Common_DayWisePaymentReport : BasePage
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
                FillGateways(); // added by amit 
                //FillPaymentStatus();
                FillTransactionType();
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Day Wise Payment Report ", "#", ""));
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

    #region-----Private Methods--------------------

    protected void BindApplicationType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseTypeID = 0;
                Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                if (CourseId > 0)
                {
                    courseTypeID = context.Courses.Where(a => a.ID == CourseId).FirstOrDefault().CourseTypeID;
                    var application = from s in context.ApplicationTypes
                                      where s.CourseTypeID == courseTypeID
                                      select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, application, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // added by amit start
    protected void FillGateways()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("-- ALL Gateways --", "0");
                var gateways = from p in context.Paymentgateways
                               orderby (p.Description)
                               select new { ValueField = p.ID, TextField = p.Description };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlgateway, gateways, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // added by amit end

    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 Catid = Convert.ToInt32(ddlCourseCategry.SelectedValue);

               var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == Catid
                                 //Added for code
                                  && p.ShowOnWeb
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
								 
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
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

    #endregion-----Private Methods--------------------

    #region-----Events--------------------

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
        ddlCourseName.Items.Clear();
        FillCourses();
    }
    protected void btnReset_Click1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            txtDateto.Text = "";
            txttDateFrom.Text = "";
            ddlCourseCategry.SelectedValue = "0";
            //ddlCourseCategry_SelectedIndexChanged(ddlCourseCategry.SelectedValue, EventArgs.Empty);
            ddlpaymentmode.SelectedValue = "0";
            ddlpaymentstatus.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddltytype.SelectedValue = "0";
            if (ddldatetype.Visible == true)
                ddldatetype.SelectedValue = "0";

            ddlgateway.Visible = false; //added by amit 
            ddlgateway.SelectedValue = "0";//added by amit 

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 paymentmode = Convert.ToInt32(ddlpaymentmode.SelectedValue);

            // added by amit start
            if (paymentmode == Convert.ToInt32(enmPaymentMode.Online))
            {
                ddlgateway.Visible = true;   
            }
            else
            {
                ddlgateway.Visible = false;
            }
            // added by amit end


            if ((paymentmode == Convert.ToInt32(enmPaymentMode.Online)) || (paymentmode == Convert.ToInt32(enmPaymentMode.NEFTRTGS)))
            {
                Lbdatetype.Visible = true;
                ddldatetype.Visible = true;
            }
            else
            {
                Lbdatetype.Visible = false;
                ddldatetype.Visible = false;
             
            }
            ddldatetype.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAppType.Items.Clear();
        ddlAppType.Items.Insert(0, "--Select One--");
        BindApplicationType();      
    }

    #endregion-----Events--------------------


    protected void ddlGateway_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if(ddlgateway.SelectedItem.Value == "2" )
            {
                ddldatetype.SelectedValue = "P";
                ddldatetype.Enabled = false;
            }
            else
            {
                ddldatetype.SelectedIndex = 0;
                ddldatetype.Enabled = true;
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}
