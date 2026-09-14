using System;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;


public partial class ExamCentreChoiceDetails : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCategories();
                    ShowEditMode();
                }
                else
                {

                    FillFilterCourses();
                    //if (Request.QueryString["Success"] == "Y")
                    //    lblError.Text = "Centre CHoice generated successfully. Select Filter Criteria For View Result";
                    //else if (Request.QueryString["Success"] == "N")
                    //    lblError.Text = "No Record found";

                    if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
                    {
                        ddlflCourse.SelectedValue = Request.QueryString["CourseID"];
                        ddlflCourse_SelectedIndexChanged(ddlflCourse, EventArgs.Empty);
                        ddlflExamCycle.SelectedValue = Request.QueryString["ExamCycleID"];
                        ddlflExamCycle_SelectedIndexChanged(ddlflExamCycle, EventArgs.Empty);
                        ddlflExamYear.SelectedValue = Request.QueryString["ExamYear"];
                        ddlflExamYear_SelectedIndexChanged(ddlflExamYear, EventArgs.Empty);
                        ddlflExam.SelectedValue = Request.QueryString["ExamID"];
                        ddlflExam_SelectedIndexChanged(ddlflExam.SelectedValue, EventArgs.Empty);
                        ddlRegionalCentre.SelectedValue = Request.QueryString["RegCentreID"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Admit Card Data: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/AdmitCardUpload.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Generate/View Exam Centre Choice", "Exam/ExamCentreChoiceDetails.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria For View Result";
                        lblError.Visible = true;
                    }
                }
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

    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");

                var Category = (from s in context.CourseCategories
                                join c in context.Courses on s.ID equals c.CourseCategoryID
                                where c.CourseTypeID == CourseType
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillRegionalCenter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                var rc = (from p in context.CertificateExamApplications
                          where p.ExamID == examID
                          join s in context.RegionalCenters on p.RegionalCenterID equals s.ID
                          select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                ListItem lst1 = new ListItem("--Select One--", "0");
                if (rc != null)
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        ddlRc.SelectedValue = entityID.ToString();
                        ddlRc.Enabled = false;
                    }
                }
                else
                {
                    ddlRc.Items.Add(lst1);
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterRegionalCenter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = Convert.ToInt32(ddlflExam.SelectedValue);
                var rc = (from p in context.ExamCentreChoiceStats
                          where p.ExamId == examID
                          join s in context.RegionalCenters on p.RegionalCentreId equals s.ID
                          select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                ListItem lst1 = new ListItem("--Select One--", "0");
                if (rc != null)
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, rc.OrderBy(c => c.TextField), lst1);
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        ddlRegionalCentre.SelectedValue = entityID.ToString();
                        ddlRegionalCentre.Enabled = false;
                    }
                }
                else
                {
                    ddlRegionalCentre.Items.Add(lst1);
                }
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillExamYear()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamYear = (from p in context.Exams
                                where p.ExaminationCycleID == ExamCycleID
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(3);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterExamYear()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlflExamCycle.SelectedValue);
                var ExamYear = (from p in context.Exams
                                where p.ExaminationCycleID == ExamCycleID
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(3);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamYear, ExamYear, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillExamCycle()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                int CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                var ExamCycleList = from p in context.ExaminationCycles
                                    where p.CourseID == CourseID
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, ExamCycleList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterExamCycle()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                int CourseID = Convert.ToInt32(ddlflCourse.SelectedValue);
                var ExamCycleList = from p in context.ExaminationCycles
                                    where p.CourseID == CourseID
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamCycle, ExamCycleList, lst);
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
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id && p.CourseTypeID == CourseType
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseTypeID = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseTypeID == CourseTypeID
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflCourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillExamName()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamName = from p in context.Exams
                               where p.ExamYear == ExamYear && p.CourseID == CourseID && p.ExaminationCycleID == ExamCycleID
                               && p.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                               select new { ValueField = p.ID, TextField = p.Name };
                ExamName = ExamName.Distinct().OrderByDescending(s => s.ValueField);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterExam()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 ExamYear = Convert.ToInt32(ddlflExamYear.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlflCourse.SelectedValue);
                Int32 examcycleid = Convert.ToInt32(ddlflExamCycle.SelectedValue);

                var ExamName = from p in context.Exams
                               where p.ExamYear == ExamYear && p.CourseID == CourseID && p.ExaminationCycleID == examcycleid
                               && p.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                               select new { ValueField = p.ID, TextField = p.Name };
                ExamName = ExamName.OrderByDescending(s => s.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExam, ExamName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            lblHeading.Text = "Generate/View Exam Centre Choice";
            tbls2.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            Int32 cid = 0;
            lblError.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                cid = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            Int64 CourseID = 0;
            Int64 ExamID = 0;
            Int64 ExamYear = 0;
            Int64 ExamCycleID = 0;
            Int64 regcentreID = 0;
            if (ddlRegionalCentre.SelectedValue != "0")
                regcentreID = Convert.ToInt64(ddlRegionalCentre.SelectedValue);
            if (ddlflCourse.SelectedValue != "0")
                CourseID = Convert.ToInt64(ddlflCourse.SelectedValue);
            if (ddlflExamCycle.SelectedValue != "0")
                ExamCycleID = Convert.ToInt64(ddlflExamCycle.SelectedValue);
            if (ddlflExamYear.SelectedValue != "0")
                ExamYear = Convert.ToInt64(ddlflExamYear.SelectedValue);
            if (ddlflExam.SelectedValue != "0")
                ExamID = Convert.ToInt64(ddlflExam.SelectedValue);

            var CentreChoices = from s in context.ExamCentreChoiceStats.AsNoTracking()
                                where s.RegionalCentreId == regcentreID && s.ExamId == ExamID
                                select new
                                {
                                    ID = s.ID,
                                    StateName = s.ExamCenter.State.Name,
                                    CityName = s.ExamCenter.Name,
                                    CityCode = s.ExamCenter.Code,
                                    FirstChoice = s.FirstChoiceTotal,
                                    SecondChoice = s.SecondChoiceTotal
                                };
            if (CentreChoices.Count() > 0)
                btnDownload.Visible = true;

            PagingBar1.Bind(CentreChoices, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No Record Found";
                lblError.Visible = true;
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            if (ddlflCourse.SelectedValue != "0" && ddlflExam.SelectedValue != "0")
            {
                BindGridView();
                btnDownload.Visible = true;
            }

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

            FillCategories();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Generate/View Exam Centre Choice";
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Exam Centre Choice", "", ""));
        }
        else
        {
            if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
            {
                Response.Redirect("ExamCentreChoiceDetails.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&RegCentreID=" + Request.QueryString["RegCentreID"], true);
            }
            else
            {
                Response.Redirect("ExamCentreChoiceDetails.aspx", true);
            }
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if (ddlflCourse.SelectedValue != "0" && ddlflExam.SelectedValue != "0")
            {
                //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Certificate Exam Result: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/ResultImports.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue, ""));
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Generate/View Exam Centre Choice: " + ddlflCourse.SelectedItem.Text + "-" + "(" + ddlflExam.SelectedItem.Text + ")", "Admin/AdmitCardUpload.aspx?CourseID=" + ddlflCourse.SelectedValue + "&ExamCycleID=" + ddlflExamCycle.SelectedValue + "&ExamYear=" + ddlflExamYear.SelectedValue + "&ExamID=" + ddlflExam.SelectedValue + "&RegCentreID=" + ddlRegionalCentre.SelectedValue, ""));
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
            ddlflCourse.SelectedValue = "0";
            ddlflExamCycle.SelectedValue = "0";
            ddlflExamYear.SelectedValue = "0";
            ddlflExam.SelectedValue = "0";
            ddlRegionalCentre.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
        ddlExamCycle.Items.Clear();
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlRc.Items.Clear();
        ddlExamCycle.Items.Insert(0, "--Select One--");
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Insert(0, "--Select One--");
        FillCourses();
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamCycle.Items.Clear();
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlRc.Items.Clear();
        ddlExamYear.Items.Insert(0, "--Select One--");
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Insert(0, "--Select One--");
        FillExamCycle();
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        ddlExamName.Items.Insert(0, "--Select One--");
        ddlRc.Items.Clear();
        ddlRc.Items.Insert(0, "--Select One--");
        FillExamYear();
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamName.Items.Clear();
        ddlRc.Items.Clear();
        ddlRc.Items.Insert(0, "--Select One--");
        FillExamName();
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRc.Items.Clear();
        FillRegionalCenter();
    }

    protected void ddlflCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExamCycle.Items.Clear();
        ddlflExamYear.Items.Clear();
        ddlflExam.Items.Clear();
        ddlflExamYear.Items.Insert(0, "--Select One--");
        ddlflExam.Items.Insert(0, "--Select One--");
        ddlRegionalCentre.Items.Clear();
        ddlRegionalCentre.Items.Insert(0, "--Select One--");
        FillFilterExamCycle();
    }
    protected void ddlflExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExamYear.Items.Clear();
        ddlflExam.Items.Clear();
        ddlflExam.Items.Insert(0, "--Select One--");
        ddlRegionalCentre.Items.Clear();
        ddlRegionalCentre.Items.Insert(0, "--Select One--");
        FillFilterExamYear();
    }
    protected void ddlflExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlflExam.Items.Clear();
        ddlRegionalCentre.Items.Clear();
        ddlRegionalCentre.Items.Insert(0, "--Select One--");
        FillFilterExam();
    }
    protected void ddlflExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRegionalCentre.Items.Clear();
        FillFilterRegionalCenter();
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            int Success = 0;

            using (var context = new EConnectContext())
            {
                int ExamId = Convert.ToInt32(ddlExamName.SelectedValue);
                int RCId = Convert.ToInt32(ddlRc.SelectedValue);

                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == ExamId && c.ApplicantTypeID == 2
                                     select c.EfferctiveDate).FirstOrDefault();

                Int32 NeftExtensionPeriod = context.Exams.Find(ExamId).NeftExtPeriod;

                //if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                //{
                //    throw new Exception("You cannot generate before the last date of Exam :" + lastDate);
                //}

                var Generated = (from p in context.ExamCentreChoiceStats
                                 where p.RegionalCentreId == RCId
                                 && p.ExamId == ExamId
                                 select p).Count();
                if (Generated > 0)
                { ShowAlert("Already Generated"); return; }

                var List1 = (from p in context.CertificateExamApplications
                             where p.ExamID == ExamId
                             && p.RegionalCenterID == RCId
                             && p.FinalSubmitted == true
                             && (p.PaymentStatusID == 2 || p.PaymentStatusID == 4)
                             group p by p.ExamCenter1ID into c
                             select new
                             {
                                 CityId = c.Key,
                                 ChoiceCount = c.Count()
                             });
                var List2 = (from p in context.CertificateExamApplications
                             where p.ExamID == ExamId
                             && p.RegionalCenterID == RCId
                             && p.FinalSubmitted == true
                             && (p.PaymentStatusID == 2 || p.PaymentStatusID == 4)
                             group p by p.ExamCenter2ID into c
                             select new
                             {
                                 CityId = c.Key,
                                 ChoiceCount = c.Count()
                             });
                var ListLeft = from p in List1
                               join q in List2 on p.CityId equals q.CityId into temp
                               join r in context.ExamCenters on p.CityId equals r.ID
                               from q in temp.DefaultIfEmpty(new { p.CityId, ChoiceCount = default(int) })
                               select new
                               {
                                   CityId = p.CityId,
                                   CityCode = r.Code,
                                   FirstChoiceCount = p.ChoiceCount,
                                   SecondChoiceCount = q.ChoiceCount
                               };
                var ListRight = from q in List2
                                join p in List1 on q.CityId equals p.CityId into temp
                                join r in context.ExamCenters on q.CityId equals r.ID
                                from p in temp.DefaultIfEmpty(new { q.CityId, ChoiceCount = default(int) })
                                select new
                                {
                                    CityId = p.CityId,
                                    CityCode = r.Code,
                                    FirstChoiceCount = p.ChoiceCount,
                                    SecondChoiceCount = q.ChoiceCount
                                };
                var ListFinal = ListLeft.Union(ListRight);


                if (ListFinal.Count() > 0)
                {
                    ExamCentreChoiceStat ChoiceObj;
                    foreach (var item in ListFinal)
                    {
                        ChoiceObj = new ExamCentreChoiceStat();
                        ChoiceObj.ExamId = ExamId;
                        ChoiceObj.RegionalCentreId = RCId;
                        ChoiceObj.CityId = item.CityId;
                        ChoiceObj.CityCode = item.CityCode;
                        ChoiceObj.FirstChoiceTotal = Convert.ToInt16(item.FirstChoiceCount);
                        ChoiceObj.SecondChoiceTotal = Convert.ToInt16(item.SecondChoiceCount);
                        ChoiceObj.CreatedByID = entityID;
                        ChoiceObj.CreatedOn = DateTime.Now;
                        context.ExamCentreChoiceStats.Add(ChoiceObj);
                    }
                    context.SaveChanges();
                    Success = 1;
                }
            }
            if (Success == 1)
                Response.Redirect("ExamCentreChoiceDetails.aspx?Success=Y");
            else
                Response.Redirect("ExamCentreChoiceDetails.aspx?Success=N");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        var context = new EConnectContext();
        int ExamId = Convert.ToInt32(ddlflExam.SelectedValue);
        int RCId = Convert.ToInt32(ddlRegionalCentre.SelectedValue);
        OleDbConnection connection = new OleDbConnection();
        try
        {
            var ChoiceList = (from p in context.ExamCentreChoiceStats
                              where p.ExamId == ExamId
                              && p.RegionalCentreId == RCId
                              select new
                              {
                                  Course = p.Exam.Course.Code,
                                  Exam = p.Exam.Name,
                                  RegionalCenter = p.RegionalCenter.Name,
                                  StateName = p.ExamCenter.State.Name,
                                  CityName = p.ExamCenter.Name,
                                  CityCode = p.ExamCenter.Code,
                                  FirstChoice = p.FirstChoiceTotal,
                                  SecondChoice = p.SecondChoiceTotal
                              }).ToList();
            if (ChoiceList.Count() == 0)
            { lblError.Text = "Data Not generated Yet."; }
            else
            {
                //OleDbConnection connection = new OleDbConnection();
                OleDbCommand command = new OleDbCommand();
                string mdbFilePath = "";
                String filename = "";

                DataTable dtTbl = new DataTable();

                string regCentreCode = context.RegionalCenters.Find(RCId).Code;
                string ExamCode = context.Exams.Find(ExamId).Name;
                filename = "Exam_Centre" + "_" + regCentreCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";

                mdbFilePath = Server.MapPath("~/Download/" + filename);
                if (System.IO.File.Exists(mdbFilePath))
                    System.IO.File.Delete(mdbFilePath);

                System.IO.File.Copy(Server.MapPath("~/Download/Exam_Centre_Database.accdb"), mdbFilePath);

                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                //string connect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                connection.ConnectionString = connect;
                connection.Open();
                //command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[Exam_Centre_Choice]", connection);
                //December_2024
                command = new OleDbCommand("delete from  [MS Access;Database=@mdbFilePath].[Exam_Centre_Choice]", connection);
                command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath);
                command.ExecuteNonQuery();
                String sqlStr = "";

                for (int i = 0; i < ChoiceList.Count(); i++)
                {
                    sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[Exam_Centre_Choice] (Course, Exam, RegionalCentre, " +
                                    " StateName, CityName, CityCode, FirstChoice,SecondChoice";
                    sqlStr += "      ) " +
                                    " values( '" + ChoiceList[i].Course + "', '" + ChoiceList[i].Exam + "', '" + ChoiceList[i].RegionalCenter + "',  '" + ChoiceList[i].StateName + "', '" + ChoiceList[i].CityName + "', '" + ChoiceList[i].CityCode + "', '" + ChoiceList[i].FirstChoice + "', '" + ChoiceList[i].SecondChoice + "'";
                    sqlStr += ")";

                    command = new OleDbCommand(sqlStr, connection);
                    command.ExecuteNonQuery();
                }
                command.Dispose();
                connection.Close();
                connection.Dispose();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.ContentType = "application/octet-stream";
                Response.Charset = "UTF-8";
                Response.WriteFile(mdbFilePath);
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose();
        connection.Close();
        }
    }
}