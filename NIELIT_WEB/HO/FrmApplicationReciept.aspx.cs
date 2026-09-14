using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using Independentsoft.Office.Word.Fields;

public partial class FrmApplicationReciept : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ucSearchBar.AutoCompleteContextKey = Request.QueryString["examID"] + "," + Request.QueryString["Key"];
                    ShowEditMode();
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["Handicapped"]) || !String.IsNullOrEmpty(Request.QueryString["SingleGuardian"]))
                {

                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilterCourseCategory();
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria to View Application Records";
                        lblError.Visible = true;
                    }
                    if (Request.QueryString["courseid"] != null || Request.QueryString["examID"] != null || Request.QueryString["examcycle"] != null || Request.QueryString["examyear"] != null || Request.QueryString["categoryid"] != null)
                    {

                        ddlcoursecategory.SelectedValue = Request.QueryString["categoryid"];
                        ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory.SelectedValue, EventArgs.Empty);
                        ddlcourse.SelectedValue = Request.QueryString["courseid"];
                        ddlcourse_SelectedIndexChanged(ddlcourse.SelectedValue, EventArgs.Empty);
                        ddlexamcycle.SelectedValue = Request.QueryString["examcycle"];
                        ddlexamcycle_SelectedIndexChanged(ddlexamcycle.SelectedValue, EventArgs.Empty);
                        ddlexamyear.SelectedValue = Request.QueryString["examyear"];
                        ddlexamyear_SelectedIndexChanged(ddlexamyear.SelectedValue, EventArgs.Empty);
                        ddlexamname.SelectedValue = Request.QueryString["examID"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Applications:-" + ddlcourse.SelectedItem.Text + "-" + ddlexamname.SelectedItem.Text, "HO/FrmApplicationReciept.aspx?courseid=" + ddlcourse.SelectedValue + "&examID=" + ddlexamname.SelectedValue + "&examcycle=" + ddlexamcycle.SelectedValue + "&examyear=" + ddlexamyear.SelectedValue + "&categoryid=" + ddlcoursecategory.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Applications", "HO/FrmApplicationReciept.aspx", ""));
                        BreadCrumb1.Render();
                    }
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void FillFilterCourse(Int32 ccatid)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 coutrsetypeid = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from p in context.Courses
                              where p.CourseTypeID == coutrsetypeid && p.CourseCategoryID == ccatid
                              orderby p.DisplayOrder
                              select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, courses, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 coutrsetypeid = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = (from p in context.Courses
                               join c in context.CourseCategories
                                   on p.CourseCategoryID equals c.ID
                               where p.CourseTypeID == coutrsetypeid
                               orderby p.DisplayOrder
                               select new { ValueField = c.ID, TextField = c.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, courses, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void BindExamCycle(int CourseId)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = context.ExaminationCycles
                               .Where(s => s.CourseID == CourseId)
                               .Select(s => new { ValueField = s.ID, TextField = s.Name });

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamcycle, courses, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindExamYear(Int32 cid, Int32 examcycleid)
    {

        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var examyear = (from s in context.Exams
                                    where s.CourseID == cid && s.ExaminationCycleID == examcycleid
                                    && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                    select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    examyear = examyear.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamyear, examyear, lst);
                }
                else
                {
                    ddlexamname.Items.Insert(0, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void BindExamname(Int32 cid, Int32 examcycleid, Int32 examyear)
    {

        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var courses = context.Exams.Where(s => s.CourseID == cid && s.ExaminationCycleID == examcycleid && s.ExamYear == examyear && s.DateOfPublishingOfTimeTable != null)
                                   .Select(s => new { ValueField = s.ID, TextField = s.Name });

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamname, courses, lst);
                }
                else
                {
                    ddlexamname.Items.Insert(0, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowEditMode()
    {

        try
        {
            context = new EConnectContext();
            Int32 applicationstatusid = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 examid = Convert.ToInt32(Request.QueryString["examID"]);
            Int32 categoryid = Convert.ToInt32(Request.QueryString["categoryid"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["courseid"]);
            Int32 examyear = Convert.ToInt32(Request.QueryString["examyear"]);
            Int32 examcycle = Convert.ToInt32(Request.QueryString["examcycle"]);
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = true;

            lblHeading.Text = "Certificate Exam Applications";
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Applications", "HO/FrmApplicationReciept.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&&courseid=" + Request.QueryString["courseid"] + "&examyear=" + Request.QueryString["examyear"] + "&categoryid=" + Request.QueryString["categoryid"] + "&examcycle=" + Request.QueryString["examcycle"] + "&paymentStatus=" + Request.QueryString["paymentStatus"], ""));
            //Create an object of record to be modified and assign properties to relevant fields.
            ViewState["SortOrder1"] = "";
            ViewState["SortField1"] = "";
            BindApplicationDetails();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 examid = 0;
            Int32 courseid = 0;
            Int32 ccatid = 0;
            Int32 examyear = 0;
            Int32 examcycleid = 0;
            if (ddlcoursecategory.SelectedValue != "0")
            {
                ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["categoryid"]))
                    ccatid = Convert.ToInt32(Request.QueryString["categoryid"]);
            }
            if (ddlcourse.SelectedValue != "0")
            {
                courseid = Convert.ToInt32(ddlcourse.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["courseid"]))
                    courseid = Convert.ToInt32(Request.QueryString["courseid"]);
            }
            if (ddlexamyear.SelectedValue != "0")
            {
                examyear = Convert.ToInt32(ddlexamyear.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examyear"]))
                    examyear = Convert.ToInt32(Request.QueryString["examyear"]);
            }
            if (ddlexamname.SelectedValue != "0")
            {
                examid = Convert.ToInt32(ddlexamname.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examID"]))
                    examid = Convert.ToInt32(Request.QueryString["examID"]);
            }
            if (ddlexamcycle.SelectedValue != "0")
            {
                examcycleid = Convert.ToInt32(ddlexamcycle.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examcycle"]))
                    examcycleid = Convert.ToInt32(Request.QueryString["examcycle"]);
            }
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 defaultid = Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
            if (courseid != 0 && examid != 0)
            {
                int paymentIDPending = Convert.ToInt32(enmPaymentStatus.Pending);
                int applIDMarked = Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
                var data = (from s in context.ApplicationStatuses
                            join p in context.CertificateExamApplications
                            on s.ID equals p.ApplicationStatusID
                            where (p.CourseID == courseid && p.Exam.ID == examid)
                            select new
                                   {
                                       Id = p.ApplicationStatusID,
                                       examName = p.Course.Code + "-" + p.Exam.Name,
                                       description = (p.PaymentStatusID == paymentIDPending && p.ApplicationStatusID == applIDMarked) ? "Fee Pending to be Paid by Institute" : s.Description,
                                       examID = p.ExamID,
                                       categoryid = p.CourseCategoryID,
                                       courseid = p.CourseID,
                                       examyear = p.Exam.ExamYear,
                                       examcycle = p.Exam.ExaminationCycleID,
                                       paymentStatusID = p.PaymentStatusID
                                   }).AsNoTracking();


                var application = from p in data
                                  group p by new { p.description, p.Id } into c
                                  select new
                                  {
                                      Id = c.Key.Id,
                                      examName = c.FirstOrDefault().examName,
                                      description = c.Key.description,
                                      examID = c.FirstOrDefault().examID,
                                      NumberOfApp = c.Count(),
                                      categoryid = c.FirstOrDefault().categoryid,
                                      courseid = c.FirstOrDefault().courseid,
                                      examyear = c.FirstOrDefault().examyear,
                                      examcycle = c.FirstOrDefault().examcycle,
                                      paymentStatus = c.FirstOrDefault().paymentStatusID
                                  };

                application = application.OrderBy(s => s.examName).ThenBy(s => s.description);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "examName":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.examName);
                            else
                                application = application.OrderBy(s => s.examName);
                            break;
                        case "description":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.description);
                            else
                                application = application.OrderBy(s => s.description);
                            break;
                        case "NumberOfApp":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.NumberOfApp);
                            else
                                application = application.OrderBy(s => s.NumberOfApp);
                            break;
                        default:
                            application = application.OrderBy(s => s.examName).ThenBy(s => s.description);
                            break;
                    }
                }
                PagingBar1.Bind(application, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();

                var application1 = context.CertificateExamApplications.AsNoTracking()
                                    .Where(s => s.Exam.ID == examid).GroupBy(s => s.ExamID)
                                    .Select(s => new
                                    {
                                        CourseId = courseid,
                                        ExamId = examid,
                                        examName = s.FirstOrDefault().Course.Code + "-" + s.FirstOrDefault().Exam.Name,
                                        Handicapped = s.Count(p => p.FinalSubmitted == true && p.IsDisability == true && p.PaymentStatusID > 1),
                                        SingleGuardian = s.Count(p => p.FinalSubmitted == true && p.GuardianName != null && p.PaymentStatusID > 1)
                                    });
                gvMain1.DataSource = application1.ToList();
                gvMain1.DataBind();
                UpdatePanel1.Update();

                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                    lblError.Text = "";
                }
            }
            else
            {
                Response.Redirect("FrmApplicationReciept.aspx", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            Response.Redirect("FrmApplicationReciept.aspx", true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            //create and object 
            //User objUser;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {

                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                strMessage = "Record updated.";
            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("FrmApplicationReciept.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if (ddlcourse.SelectedValue != "0" || ddlexamname.SelectedValue != "0" || ddlexamcycle.SelectedValue != "0" || ddlexamyear.SelectedValue != "0" || ddlcoursecategory.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Exam Applications:-" + ddlcourse.SelectedItem.Text + "-" + ddlexamname.SelectedItem.Text, "HO/FrmApplicationReciept.aspx?courseid=" + ddlcourse.SelectedValue + "&examID=" + ddlexamname.SelectedValue + "&examcycle=" + ddlexamcycle.SelectedValue + "&examyear=" + ddlexamyear.SelectedValue + "&categoryid=" + ddlcoursecategory.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlcourse.SelectedValue = "0";
            ddlexamname.SelectedValue = "0";
            ddlexamcycle.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("FrmApplicationReciept.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmApplicationReciept.aspx?categoryid=" + Request.QueryString["categoryid"] + "&courseid=" + Request.QueryString["courseid"] + "&examyear=" + Request.QueryString["examyear"] + "&examID=" + Request.QueryString["examID"] + "&examcycle=" + Request.QueryString["examcycle"]), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 cid = Convert.ToInt32(ddlcourse.SelectedValue);
        BindExamCycle(cid);
    }
    protected void ddlexamcycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlcourse.SelectedValue);
        Int32 examcycleid = Convert.ToInt32(ddlexamcycle.SelectedValue);
        BindExamYear(courseid, examcycleid);

    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 ccategoryid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        FillFilterCourse(ccategoryid);
    }
    protected void ddlexamyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlcourse.SelectedValue);
        Int32 examcycleid = Convert.ToInt32(ddlexamcycle.SelectedValue);
        Int32 examyear = Convert.ToInt32(ddlexamyear.SelectedValue);
        BindExamname(courseid, examcycleid, examyear);
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                hl2.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hl2.Text.ToLower());

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                hl3.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hl3.Text.ToLower());

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);

                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvdetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField1"] = e.SortExpression;
            if (ViewState["SortOrder1"].ToString() == "DESC")
                ViewState["SortOrder1"] = "ASC";
            else
                ViewState["SortOrder1"] = "DESC";
            BindApplicationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChangedOld(Int32 NewPageIndex)
    {
        try
        {
            gvdetail.PageIndex = PagingBar2.CurrentPageIndex;
            BindApplicationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindApplicationDetails()
    {
        try
        {
            context = new EConnectContext();
            Int32 applicationstatusid = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 paymentStatusId = Convert.ToInt32(Request.QueryString["paymentStatus"]);
            string Handicapped = Request.QueryString["Handicapped"];
            string SingleGuardian = Request.QueryString["SingleGuardian"];
            Int32[] paymentstatus = { Convert.ToInt32(enmPaymentStatus.Paid), Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), Convert.ToInt32(enmPaymentStatus.Failed) };
            Int32 examid = Convert.ToInt32(Request.QueryString["examID"]);
            string sortOrder = ViewState["SortOrder1"].ToString();
            string sortField = ViewState["SortField1"].ToString();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int32 apptypeID = Convert.ToInt32(enmApplicationType.CertificateExamApplication);

            IEnumerable<MyType> application = (from p in context.CertificateExamApplications.AsNoTracking()
                                               where p.ExamID == examid
                                               select new MyType
                                               {
                                                   Id = p.ID,
                                                   Appno = p.Number,
                                                   Appdate = p.ApplicationDate,
                                                   apptypeID = apptypeID,
                                                   Name = p.Name,
                                                   GuardianName = p.GuardianName,
                                                   InstituteId = p.InstituteID != null ? p.InstituteID : 0,
                                                   Institute = p.InstituteID != null ? p.Institute.Name : "N/A",
                                                   RegionalCenter = p.RegionalCenter.Name,
                                                   Contact = (!string.IsNullOrEmpty(p.EmailAddress) ? p.EmailAddress.ToLower() : "N/A") + "<br/>" + SqlFunctions.StringConvert((double)p.MobileNumber),
                                                   key = p.ApplicationStatusID,
                                                   examID = p.ExamID,
                                                   pStatusID = p.PaymentStatusID,
                                                   disability = p.IsDisability != null ? p.IsDisability : false
                                               });
            if (applicationstatusid > 0)
            { application = application.Where(s => s.key == applicationstatusid); }
            if (Handicapped == "true")
            { application = application.Where(s => s.disability == true && s.pStatusID > 1); }
            if (SingleGuardian == "true")
            { application = application.Where(s => s.GuardianName != null && s.pStatusID > 1); }

            application = application.OrderBy(s => s.RegionalCenter).ThenBy(s => s.apptypeID).ThenBy(s => s.InstituteId);

            if ((applicationstatusid == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
            {
                application = application.Where(s => s.pStatusID == paymentStatusId);
            }
            else if ((applicationstatusid == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Paid) || paymentStatusId == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
            {
                application = application.Where(s => paymentstatus.Contains(s.pStatusID));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                application = application.Where(s => s.Appno.ToUpper().Contains(searchString)
                                       || s.Name.ToUpper().Contains(searchString));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Id":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Id);
                        else
                            application = application.OrderBy(s => s.Id);
                        break;
                    case "Appno":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Appno);
                        else
                            application = application.OrderBy(s => s.Appno);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Name);
                        else
                            application = application.OrderBy(s => s.Name);
                        break;
                    case "Institute":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Institute);
                        else
                            application = application.OrderBy(s => s.Institute);
                        break;                   
                    case "Appdate":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Appdate);
                        else
                            application = application.OrderBy(s => s.Appdate);
                        break;
                    default:
                        application = application.OrderBy(s => s.Appno);
                        break;
                }
            }
            PagingBar2.Bind(application, ref gvdetail);
            Updatepanellist.Update();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Btnprint_Click(object sender, EventArgs e)
    {
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        try
        {
            Int32 applicationstatusid = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 examid = Convert.ToInt32(Request.QueryString["examID"]);
            Int32 paymentStatusId = Convert.ToInt32(Request.QueryString["paymentStatus"]);
            string examname = string.Empty;
            string strQuery = string.Empty;
            string paymentstatus = string.Empty;
            paymentstatus += Convert.ToInt32(enmPaymentStatus.Paid) + ",";
            paymentstatus += Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + ",";
            paymentstatus += Convert.ToInt32(enmPaymentStatus.Failed);

            using (EConnectContext context = new EConnectContext())
            {
                Exam currentexam = context.Exams.Find(examid);
                ApplicationStatus appstatus = context.ApplicationStatuses.Find(applicationstatusid);
                if (currentexam != null)
                {
                    if ((applicationstatusid == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
                    {
                        examname = currentexam.Course.Code.ToUpper() + "_ " + CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(currentexam.ExamMonth).Substring(0, 3) + currentexam.ExamYear + "_" + "Fee Pending to be Paid by Institute";
                    }
                    else
                    {
                        examname = currentexam.Course.Code.ToUpper() + "_ " + CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(currentexam.ExamMonth).Substring(0, 3) + currentexam.ExamYear + "_" + GetInitCap(appstatus.Description.Replace(",", ""));
                    }
                }
            };
            BreadCrumb1.Render();

            //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            con.Open();
            DataTable dt = new DataTable();

            //December_2024
            List<SqlParameter> parameters = new List<SqlParameter>();       //Dynamic_Array
                                                                            
            if ((applicationstatusid == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
            {
                //strQuery = "select ce.Number, REPLACE(CONVERT(VARCHAR(11), ce.Date, 106), ' ', '-') as Application_Date , ce.Salutaion + '' + ce.Name as Candidate_Name , isnull('Mr.'+ ce.Father_Name,'N/A') as Father_Name , lower(ce.Email) as Email_Address , ce.Mobile as Mobile_Number, 'Fee Pending to be Paid by Institute' as Application_Status from Certificate_Exam_Application ce " +
                //                 ", Application_Status a where ce.Application_Status_ID = a.ID and ce.Application_Status_ID = " + applicationstatusid + " and  ce.Exam_ID = " + examid + " and ce.Payment_Status_ID = " + paymentStatusId + " order by ce.Number asc ";

                //December_2024
                parameters.Add(new SqlParameter("@applicationStatusId", applicationstatusid));             
                parameters.Add(new SqlParameter("@examId", examid));
                parameters.Add(new SqlParameter("@paymentStatusId", paymentStatusId));
                strQuery = "select ce.Number, REPLACE(CONVERT(VARCHAR(11), ce.Date, 106), ' ', '-') as Application_Date , ce.Salutaion + '' + ce.Name as Candidate_Name , isnull('Mr.'+ ce.Father_Name,'N/A') as Father_Name , lower(ce.Email) as Email_Address , ce.Mobile as Mobile_Number, 'Fee Pending to be Paid by Institute' as Application_Status from Certificate_Exam_Application ce " +
                                  ", Application_Status a where ce.Application_Status_ID = a.ID and ce.Application_Status_ID = @applicationStatusId and  ce.Exam_ID = @examId and ce.Payment_Status_ID = @paymentStatusId order by ce.Number asc ";
            }
            else if ((applicationstatusid == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Paid) || paymentStatusId == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
            {
                //strQuery = "select ce.Number, REPLACE(CONVERT(VARCHAR(11), ce.Date, 106), ' ', '-') as Application_Date , ce.Salutaion + '' + ce.Name as Candidate_Name , isnull('Mr.'+ ce.Father_Name,'N/A') as Father_Name , lower(ce.Email) as Email_Address , ce.Mobile as Mobile_Number, a.Description as Application_Status from Certificate_Exam_Application ce " +
                //                  ", Application_Status a where ce.Application_Status_ID = a.ID and ce.Application_Status_ID = " + applicationstatusid + " and  ce.Exam_ID = " + examid + " and ce.Payment_Status_ID in (" + paymentstatus.Trim().TrimEnd(',') + ") order by ce.Number asc";

                //December_2024
                parameters.Add(new SqlParameter("@applicationStatusId", applicationstatusid));
                parameters.Add(new SqlParameter("@examId", examid));
                parameters.Add(new SqlParameter("@paymentStatus", paymentstatus.Trim().TrimEnd(',')));
                strQuery = "select ce.Number, REPLACE(CONVERT(VARCHAR(11), ce.Date, 106), ' ', '-') as Application_Date , ce.Salutaion + '' + ce.Name as Candidate_Name , isnull('Mr.'+ ce.Father_Name,'N/A') as Father_Name , lower(ce.Email) as Email_Address , ce.Mobile as Mobile_Number, a.Description as Application_Status from Certificate_Exam_Application ce " +
                                  ", Application_Status a where ce.Application_Status_ID = a.ID and ce.Application_Status_ID = @applicationStatusId and  ce.Exam_ID = @examId and ce.Payment_Status_ID in (@paymentStatus) order by ce.Number asc";
            }
            else
            {
                //strQuery = "select ce.Number, REPLACE(CONVERT(VARCHAR(11), ce.Date, 106), ' ', '-') as Application_Date , ce.Salutaion + '' + ce.Name as Candidate_Name , isnull('Mr.'+ ce.Father_Name,'N/A') as Father_Name , lower(ce.Email) as Email_Address , ce.Mobile as Mobile_Number, a.Description as Application_Status from Certificate_Exam_Application ce " +
                //                  ", Application_Status a where ce.Application_Status_ID = a.ID and ce.Application_Status_ID = " + applicationstatusid + " and  ce.Exam_ID = " + examid + " order by ce.Number asc";

                //December_2024
                parameters.Add(new SqlParameter("@applicationStatusId", applicationstatusid));
                parameters.Add(new SqlParameter("@examId", examid));
                strQuery = "select ce.Number, REPLACE(CONVERT(VARCHAR(11), ce.Date, 106), ' ', '-') as Application_Date , ce.Salutaion + '' + ce.Name as Candidate_Name , isnull('Mr.'+ ce.Father_Name,'N/A') as Father_Name , lower(ce.Email) as Email_Address , ce.Mobile as Mobile_Number, a.Description as Application_Status from Certificate_Exam_Application ce " +
                                 ", Application_Status a where ce.Application_Status_ID = a.ID and ce.Application_Status_ID = @applicationStatusId and  ce.Exam_ID = @examId order by ce.Number asc";
            }

            string sheetname = examname.Replace(",", "_") + "_Candidate_Details";
            dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, parameters.ToArray(), CommandType.Text, true);

            foreach (DataRow dr in dt.Rows)
            {
                dr["Candidate_Name"] = GetInitCap(dr["Candidate_Name"].ToString());
                dr["Father_Name"] = GetInitCap(dr["Father_Name"].ToString());
            }

            if (dt.Rows.Count > 0)
            {
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                con.Close();
            }
            else
            {
                ShowAlert("No record found.");
                con.Close();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            con.Close();
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {

    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar2.CurrentPageIndex = 0;
            gvdetail.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar2.CurrentPageIndex = 0;
            gvdetail.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int32 examid = 0;
            Int32 applicationstatusID = 0;
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                examid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                applicationstatusID = Convert.ToInt32(keys[1]);
            string searchString = prefixText.Trim().ToUpper();

            var applications = context.CertificateExamApplications.Where(p => p.ExamID == examid && p.ApplicationStatusID == applicationstatusID)
                                   .Select(p => new[] { p.Name, p.Number }).SelectMany(s => s).AsNoTracking();
            applications = applications.Where(s => s.ToUpper().Contains(searchString));

            foreach (var user in applications)
            {
                items.Add(user.ToUpper());
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
}
public partial class MyType
{
    public long Id { get; set; }
    public string Appno { get; set; }
    public DateTime Appdate { get; set; }
    public int apptypeID { get; set; }
    public string Name { get; set; }
    public string GuardianName { get; set; }
    public long? InstituteId { get; set; }
    public string Institute { get; set; }
    public string RegionalCenter { get; set; }
    public string Contact { get; set; }   
    public int key { get; set; }
    public int examID { get; set; }
    public int pStatusID { get; set; }
    public bool? disability { get; set; }

}