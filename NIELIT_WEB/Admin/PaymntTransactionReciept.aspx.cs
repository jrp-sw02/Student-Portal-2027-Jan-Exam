using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
public partial class HO_HOPaymntRecieptReport : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
               // Response.Write("Sorry! You don't have rights  to view this page");
              //  Response.End();
            }

            if (!Page.IsPostBack)
            {
                ddlregcenter.Enabled = false;
                trsettled.Visible = false;
                trsettled1.Visible = false;
                txttDateFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtDateto.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                FillPaymentMode();
                FillCategories();
                FillRegionalcentre();
                FillGateways();
                //FillPaymentStatus();
                FillTransactionType();
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Pay Transaction Report", "#", ""));
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
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // added by amit end

    protected void BindApplicationType(Int32 ccatid)
    {

        try
        {
            using (var context = new EConnectContext())
            {
                Int32 courseTypeID = context.Courses.Where(a => a.CourseCategoryID == ccatid).FirstOrDefault().CourseTypeID;
                ListItem lst = new ListItem("--Select One--", "0");
                var application = from s in context.ApplicationTypes
                                  where s.CourseTypeID == courseTypeID
                                  select new { ValueField = s.ID, TextField = s.Name };
                var vafacfindropdown = Convert.ToInt32(enmApplicationType.CourseVafAcfApplication);
                var ModuleCertificateRequest = Convert.ToInt32(enmApplicationType.ModuleCertificateRequest);
                application = application.Where(a => a.ValueField != ModuleCertificateRequest);
                if (Convert.ToInt32(ddlCourseCategry.SelectedValue) != 1)
                {
                    application = application.Where(a => a.ValueField != vafacfindropdown);
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, application, lst);

            }
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

                if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillRegionalcentre()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("All", "0");
                var regional = from p in context.RegionalCenters
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlregcenter, regional, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourses()
    {
        try
        {
            //using (EConnectContext context = new EConnectContext())
            //{
            //    //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            //    ListItem lst = new ListItem("--All--", "0");
            //Int32 id = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            //    var CourseList = from p in context.Courses
            //                     where p.CourseCategoryID == id
            //                      && p.ShowOnWeb
            //                     orderby p.Name
            //                     select new { ValueField = p.ID, TextField = p.Name + " ( " + p.Code + " )" };

            //    if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
            //    {
            //        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
            //        CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
            //    }
            //    CourseList = CourseList.Distinct();
            //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
            //};
            //Added by Deep on 12 May 2022 for Short term course(6) view all courses start
            Int32 id = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            if (id == 6)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                    ddlCourseName.BackColor = System.Drawing.Color.LightYellow;
                    ListItem lst = new ListItem("--All--", "0");
                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == id
                                     orderby p.Name
                                     select new { ValueField = p.ID, TextField = p.Name + " ( " + p.Code + " )" };

                    if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    CourseList = CourseList.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
                };

            }
            else
            {
                using (EConnectContext context = new EConnectContext())
                {
                    ListItem lst = new ListItem("--All--", "0");

                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == id
                                      && p.ShowOnWeb
                                     orderby p.Name
                                     select new { ValueField = p.ID, TextField = p.Name + " ( " + p.Code + " )" };

                    if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    CourseList = CourseList.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
                };
            }

            //Added by Deep on 12 May 2022 for Short term course(6) view all courses End

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
                Int32 CSCSPV = Convert.ToInt32(enmPaymentMode.CSCSPV);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID != multicheque && p.ID != cash
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                string roleName = context.Roles.Find(currentRoleId).Name.ToUpper();
                if (roleName.Contains("CSC-SPV"))
                {
                    Category = Category.Where(s => s.ValueField == CSCSPV);
                }
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
    //protected void FillPaymentStatus()
    //{
    //    try
    //    {

    //        EnumUtility.BindListObject(ref ddlpaymentstatus, typeof(EConnect.enmPaymentStatus), new ListItem("--All--", "0"));

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}
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
        Int32 ccatID = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        if (ccatID > 0)
        {
            if (ccatID == 2 || ccatID == 8)
            {
                FillRegionalcentre();
                ddlregcenter.Enabled = true;
            }
            else
            {
                ddlregcenter.Items.Clear();
                ListItem lst = new ListItem("All", "0");
                ddlregcenter.Items.Add(lst);
                ddlregcenter.Enabled = false;
            }
            ddlAppType.Items.Clear();
            BindApplicationType(ccatID);
        }
        if (ddlCourseCategry.SelectedValue == "0")
        {
            ListItem lst2 = new ListItem("All", "0");
            ListItem lst3 = new ListItem("--Select One--", "0");
            ddlexamname.Items.Clear();
            ddlexamname.Items.Add(lst2);
            ddlAppType.Items.Clear();
            ddlAppType.Items.Add(lst3);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 coursecatid = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            ListItem lst = new ListItem("All", "0");
            using (var context = new EConnectContext())
            {
                if (AppTypeID > 0)
                {
                    if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        var courses = from s in context.Exams
                                      join c in context.CourseRegistrationApplications
                                       on s.ID equals c.ApplicableExamID
                                      where c.CourseCategoryID == coursecatid
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.Name,
                                          TextField = s.Name,
                                      };

                        courses = courses.Distinct().OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, courses, lst);
                    }
                    else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        var courses = from s in context.Exams
                                      join c in context.CourseExamApplications
                                       on s.ID equals c.ExamID
                                      where c.CourseCategoryID == coursecatid
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.Name,
                                          TextField = s.Name
                                      };

                        courses = courses.Distinct().OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, courses, lst);
                    }
                    else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                    {
                        var courses = from s in context.Exams
                                      join c in context.CourseExamVafAcfs
                                       on s.ID equals c.ExamID
                                      where c.CourseCategoryID == coursecatid
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.Name,
                                          TextField = s.Name
                                      };

                        courses = courses.Distinct().OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, courses, lst);
                    }
                    //---------vaf acf end----------------------------------------------------------------

                    else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        var courses = from s in context.Exams
                                      join c in context.CertificateExamApplications
                                       on s.ID equals c.ExamID
                                      where c.CourseCategoryID == coursecatid
                                      select new
                                      {
                                          Month = s.ExamMonth,
                                          Year = s.ExamYear,
                                          ValueField = s.Name,
                                          TextField = s.Name,
                                      };

                        courses = courses.Distinct().OrderByDescending(a => a.Year).ThenBy(k => k.Month);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, courses, lst);
                    }
                }
            };

            if (ddlAppType.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("All", "0");
                ddlexamname.Items.Clear();
                ddlexamname.Items.Add(lst1);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnReset_Click1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            txtDateto.Text = "";
            txttDateFrom.Text = "";
            ddlAppType.SelectedValue = "0";
            ddlCourseCategry.SelectedValue = "0";
            ddlpaymentmode.SelectedValue = "0";
            ddlpaymentstatus.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlexamname.SelectedValue = "0";
            ddltytype.SelectedValue = "0";
            Txttransno.Text = "";
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
        try
        {
            Int32 paymentmode = Convert.ToInt32(ddlpaymentmode.SelectedValue);
            string paymentstatus = Convert.ToString(ddlpaymentstatus.SelectedValue);

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

            if ((paymentmode == Convert.ToInt32(enmPaymentMode.Online)) && (paymentstatus == "S"))
            {
                trsettled.Visible = true;
                trsettled1.Visible = true;
            }
            else
            {
                trsettled.Visible = false;
                trsettled1.Visible = false;
            }
            if ((paymentmode == Convert.ToInt32(enmPaymentMode.DemandDraft)) || (paymentmode == Convert.ToInt32(enmPaymentMode.CSCSPV)))
            {
                Lbldatetype.Visible = false;
                ddldatetype.Visible = false;
            }
            else
            {
                Lbldatetype.Visible = true;
                ddldatetype.Visible = true;
            }
            ddldatetype.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
    protected void ddlpaymentstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 paymentmode = Convert.ToInt32(ddlpaymentmode.SelectedValue);
            string paymentstatus = Convert.ToString(ddlpaymentstatus.SelectedValue);
            if ((paymentstatus == "S") && (paymentmode == Convert.ToInt32(enmPaymentMode.Online)))
            {
                trsettled.Visible = true;
                trsettled1.Visible = true;
            }
            else
            {
                trsettled.Visible = false;
                trsettled1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }

    //protected void ddlGateway_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlgateway.SelectedItem.Value == "2")
    //        {
    //            ddldatetype.SelectedValue = "P";
    //            ddldatetype.Enabled = false;
    //        }
    //        else
    //        {
    //            ddldatetype.SelectedIndex = 0;
    //            ddldatetype.Enabled = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}