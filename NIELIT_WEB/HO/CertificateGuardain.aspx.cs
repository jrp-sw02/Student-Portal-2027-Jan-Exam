using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_CertificateGuardain : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
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
            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
                if (!IsPostBack)
                {
                    BindCourseCategory();
                    bindGender();
                    bindOccupation();
                    bindCastCategory();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    if (!String.IsNullOrEmpty(Request.QueryString["ApplID"]))
                    {
                        ShowEditMode();
                    }
                    else
                    {
                        if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null)
                        {
                            ddlRc.SelectedValue = Request.QueryString["RegCentreID"];
                            ddlCourseCategry.SelectedValue = Request.QueryString["CouCategory"];
                            ddlCourseCategry_SelectedIndexChanged(ddlCourseCategry.SelectedValue, EventArgs.Empty);
                            ddlCourseName.SelectedValue = Request.QueryString["CourseID"];
                            ddlCourseName_SelectedIndexChanged(ddlCourseName.SelectedValue, EventArgs.Empty);
                            ddlAppType.SelectedValue = Request.QueryString["AppltypeID"];
                            ddlAppType_SelectedIndexChanged(ddlAppType.SelectedValue, EventArgs.Empty);
                            ddlExamCycle.SelectedValue = Request.QueryString["ExamCycleID"];
                            ddlExamCycle_SelectedIndexChanged(ddlExamCycle.SelectedValue, EventArgs.Empty);
                            ddlExamYear.SelectedValue = Request.QueryString["ExamYear"];
                            ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                            ddlExamName.SelectedValue = Request.QueryString["ExamID"];
                            ddlDownloadType.SelectedValue = Request.QueryString["filter"];
                            ddlDownloadType_SelectedIndexChanged(ddlDownloadType.SelectedValue, EventArgs.Empty);
                            ddlResultgradeType.SelectedValue = Request.QueryString["ResultType"];
                            if (Request.QueryString["index"] != null)
                                PagingBar1.CurrentPageIndex = Convert.ToInt32(Request.QueryString["index"]);
                            if (Request.QueryString["search"] != null)
                                TxtSearch.Text = Convert.ToString(Request.QueryString["search"]);
                            if (Request.QueryString["Searchtype"] != null)
                                Rdsearchby.SelectedValue = Request.QueryString["Searchtype"];
                            BreadCrumb1.RemoveLastBreadCrumbItem();
                            BreadCrumb1.RemoveLastBreadCrumbItem();
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Guardian Data", "HO/CertificateGuardain.aspx?CourseID=" + ddlCourseName.SelectedValue + "&ExamID=" + ddlExamName.SelectedValue + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&index=" + PagingBar1.CurrentPageIndex + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue, ""));
                            BreadCrumb1.Render();
                            BindGridView();
                        }
                        else
                        {
                            BreadCrumb1.RemoveLastBreadCrumbItem();
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Guardian Data", "HO/CertificateGuardain.aspx", ""));
                            BreadCrumb1.Render();
                        }
                    }
                }
                BreadCrumb1.Render();
            }
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    public void bindOccupation()
    {

        using (EConnectContext context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var occupation = from p in context.Occupations
                             orderby (p.DisplayOrder)
                             select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddloccupation, occupation, lst);
        };

    }
    public void bindGender()
    {
        EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlGender, typeof(EConnect.Gender), new ListItem("--Select One--", "0"));
    }
    public void bindCastCategory()
    {

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var castcategory = from p in context.CastCategories
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
        };
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.CourseCategories
                              join c in context.Courses
                                  on s.ID equals c.CourseCategoryID
                              where c.CourseTypeID == courseTypeCertificateExam
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                ListItem lst1 = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                if (loginUserType == UserType.RegionalCenter)
                {
                    ddlRc.SelectedValue = entityID.ToString();
                    ddlRc.Enabled = false;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlExamCycle.Items.Clear();
            ddlExamYear.Items.Clear();
            ddlExamName.Items.Clear();
            ddlExamCycle.Items.Insert(0, "--Select One--");
            ddlExamYear.Items.Insert(0, "--Select One--");
            ddlExamName.Items.Insert(0, "--Select One--");
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            };
          
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlExamCycle.Items.Clear();
            ddlExamYear.Items.Clear();
            ddlExamName.Items.Clear();
            ddlExamCycle.Items.Insert(0, "--Select One--");
            ddlExamYear.Items.Insert(0, "--Select One--");
            ddlExamName.Items.Insert(0, "--Select One--");
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            //ddlAppType.SelectedValue = "0";
            //ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlExamCycle.Items.Clear();
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);

                };
            }
            else
            {
                ddlExamCycle.Items.Clear();
                ddlExamCycle.Items.Insert(0, lst);
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    var courses = (from s in context.Exams
                                   join c in context.CertificateExamApplications
                                       on s.ID equals c.ExamID
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                   select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                    ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                }
                else
                {
                    ddlExamYear.Items.Clear();
                    ddlExamYear.Items.Insert(0, lst);
                    ddlExamYear.SelectedValue = "0";
                    ddlExamYear_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    var courses = (from s in context.Exams
                                   join c in context.CertificateExamApplications
                                       on s.ID equals c.ExamID
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    //ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                    // ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlDownloadType.SelectedValue = "0";
            TxtSearch.Text = "";
            if ((loginUserType != UserType.RegionalCenter) && (loginUserType != UserType.ExternalRegionalCenter))
            {
                ddlRc.SelectedValue = "0";
            }
            gvMain.DataSource = null;
            gvMain.DataBind();
            Response.Redirect("CertificateGuardain.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlDownloadType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            if (ddlDownloadType.SelectedValue == "1")
            {
                tdresultgrade.Visible = false;
                tdresultgradeheader.Visible = false;
            }
            else
            {
                tdresultgrade.Visible = true;
                tdresultgradeheader.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGridView()
    {
        try
        {
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 ccatid = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 applTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 regionalCenterID = Convert.ToInt32(ddlRc.SelectedValue);
            Int32 filterID = Convert.ToInt32(ddlDownloadType.SelectedValue);
            Int32 appltypeid = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 resulttype = Convert.ToInt32(ddlResultgradeType.SelectedValue);
            string filter = "";
            filter = Convert.ToString(TxtSearch.Text);
            context = new EConnectContext();
            String[] IncorrectCharacter = { " ", "'", "-", "--", "---", "----", ".", "...", "....", "_", "0", "1", "`", "..", "/", @",", @"\", "-----", "------", ".....", "......" };
            Int32[] passedresultgrade = context.ResultGrades.Where(s => s.CourseCategoryID == ccatid && s.IsPassed == true).Select(t => t.ID).ToArray();
            Int32[] notpassedresultgrade = context.ResultGrades.Where(s => s.CourseCategoryID == ccatid && s.IsPassed == false).Select(t => t.ID).ToArray();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var registration = from c in context.CertificateExamApplications
                               where (c.GuardianName != null || IncorrectCharacter.Contains(c.FatherName) || IncorrectCharacter.Contains(c.MotherName))
                               select new
                               {
                                   ID = c.ID,
                                   ApplID = c.ID,
                                   Applno = c.Number,
                                   Rollno = c.RollNumber,
                                   ExamCycleID = c.Exam.ExaminationCycleID,
                                   ExamYear = c.Exam.ExamYear,
                                   date = c.ApplicationDate,
                                   CourseID = c.CourseID,
                                   couID = courseID,
                                   StudentName = c.Salutation + c.Name.ToUpper(),
                                   CourseName = c.Course.Code.ToUpper(),
                                   guardian = (!string.IsNullOrEmpty(c.GuardianName)) ? c.GuardianName : "NA",
                                   fname = c.FatherName,
                                   mname = c.MotherName,
                                   ExamID = c.ExamID,
                                   filcriteria = filterID,
                                   index = PagingBar1.CurrentPageIndex,
                                   Regcentreid = c.RegionalCenterID,
                                   resultgradeid = c.ResultGradeID,
                                   coucategoryid = c.CourseCategoryID,
                                   appltypeid = appltypeid,
                                   resulttype = resulttype,
                                   grade = c.ResultGradeID.HasValue ? c.ResultGrade.Code : "N/A"
                               };
            if (courseID != 0)
            {
                registration = registration.Where(s => s.CourseID == courseID);
            }
            if (examID != 0)
            {
                registration = registration.Where(s => s.ExamID == examID);
            }
            if (regionalCenterID != 0)
            {
                registration = registration.Where(s => s.Regcentreid == regionalCenterID);
            }
            if (filterID == 0)
            {
                registration = registration.Where(s => s.resultgradeid != null);
                Int32 resultgradetype = Convert.ToInt32(ddlResultgradeType.SelectedValue);
                if (resultgradetype == 1)
                    registration = registration.Where(s => passedresultgrade.Contains(s.resultgradeid.Value));
                else
                    registration = registration.Where(s => notpassedresultgrade.Contains(s.resultgradeid.Value));
            }
            if (filterID == 1)
            {
                registration = registration.Where(s => s.resultgradeid == null);
            }
            if (Rdsearchby.SelectedValue == "0")
            {
                if (TxtSearch.Text.Trim() != "" && TxtSearch.Text != null)
                {
                    registration = registration.Where(s => s.Rollno.ToUpper() == filter.ToUpper());
                }
            }
            else if (Rdsearchby.SelectedValue == "1")
            {
                registration = registration.Where(s => s.StudentName.ToUpper().Contains(filter));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.ID);
                        else
                            registration = registration.OrderBy(s => s.ID);
                        break;
                    case "Applno":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.Applno);
                        else
                            registration = registration.OrderBy(s => s.Applno);
                        break;
                    case "date":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.date);
                        else
                            registration = registration.OrderBy(s => s.date);
                        break;
                    case "StudentName":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.StudentName);
                        else
                            registration = registration.OrderBy(s => s.StudentName);
                        break;
                    case "guardian":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.guardian);
                        else
                            registration = registration.OrderBy(s => s.guardian);
                        break;
                    case "fname":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.fname);
                        else
                            registration = registration.OrderBy(s => s.fname);
                        break;
                    case "mname":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.mname);
                        else
                            registration = registration.OrderBy(s => s.mname);
                        break;
                    case "CourseName":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.CourseName);
                        else
                            registration = registration.OrderBy(s => s.CourseName);
                        break;
                    case "grade":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.grade);
                        else
                            registration = registration.OrderBy(s => s.grade);
                        break;
                    case "Rollno":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.Rollno);
                        else
                            registration = registration.OrderBy(s => s.Rollno);
                        break;
                    default:
                        registration = registration.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(registration, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (registration.Count() == 0)
            {
                Lblerror.Visible = true;
                Lblerror.Text = "No Record Found";
                divGrid.Visible = false;
            }
            else
            {
                Lblerror.Visible = false;
                divGrid.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            context.Dispose();
        }

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            BindGridView();
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
            divedit.Visible = true;
            divfilter.Visible = false;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certificate Exam Guardian Data:Update", "", ""));
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int32 Appid = Convert.ToInt32(Request.QueryString["ApplID"]);
            var student = (from s in context.CertificateExamApplications
                           where s.ID == Appid
                           select new
                           {
                               name = s.Name,
                               fname = s.FatherName,
                               mname = s.MotherName,
                               dob = s.DateOfBirth,
                               gender = s.Gender,
                               occupation = (s.OccupationID != 0) ? s.OccupationID : 0,
                               category = (s.CastCategoryID != 0) ? s.CastCategoryID : 0,
                               salution = s.Salutation,
                               Gname = s.GuardianName,
                           }).FirstOrDefault();

            btnSave.Text = "Update";

            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields
            if (student.salution.ToString().ToUpper() == "MR.")
                ddlSalutation.SelectedValue = "1";
            else
                ddlSalutation.SelectedValue = "2";

            txtName.Text = student.name.ToUpper();
            if (student.gender.ToString().ToUpper() == "FEMALE")
                ddlGender.SelectedValue = "1";
            else
                ddlGender.SelectedValue = "2";
            if (string.IsNullOrEmpty(student.Gname) == true && string.IsNullOrWhiteSpace(student.Gname) == true)
            {
                if (string.IsNullOrEmpty(student.fname) == false && !string.IsNullOrWhiteSpace(student.fname))
                    Txt_Fname.Text = GetInitCap(student.fname);
                else
                    Txt_Fname.Text = " ";
                if (string.IsNullOrEmpty(student.mname) == false && string.IsNullOrWhiteSpace(student.mname) == false)
                    Txt_Mname.Text = GetInitCap(student.mname);
                else
                    Txt_Mname.Text = " ";
            }
            else
            {
                Txt_GName.Text = string.IsNullOrEmpty(student.Gname) == false && string.IsNullOrWhiteSpace(student.Gname) == false ? GetInitCap(student.Gname) : " ";
            }

            Txt_Dob.Text = student.dob.ToString("dd-MMM-yyyy");
            ddloccupation.SelectedValue = student.occupation.ToString();
            ddlCategory.SelectedValue = student.category.ToString();

            EnableDisableColumn();

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
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl6 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl7 = (HyperLink)e.Row.Cells[8].Controls[0];
                hl7.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl7.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                HyperLink hl8 = (HyperLink)e.Row.Cells[9].Controls[0];
                hl8.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl8.NavigateUrl + "&ExamCycleID=" + ddlExamCycle.SelectedValue + "&ExamYear=" + ddlExamYear.SelectedValue + "&filter=" + ddlDownloadType.SelectedValue + "&RegCentreID=" + ddlRc.SelectedValue + "&CouCategory=" + ddlCourseCategry.SelectedValue + "&AppltypeID=" + ddlAppType.SelectedValue + "&ResultType=" + ddlResultgradeType.SelectedValue + "&search=" + TxtSearch.Text + "&Searchtype=" + Rdsearchby.SelectedValue);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 Appid = Convert.ToInt32(Request.QueryString["ApplID"]);
                if (!String.IsNullOrEmpty(Request.QueryString["ApplID"]))
                {
                    var student = (from s in context.CertificateExamApplications
                                   where s.ID == Appid
                                   select s).FirstOrDefault();

                    if (ddlSalutation.SelectedValue == "1")
                        student.Salutation = "Mr.";
                    else
                        student.Salutation = "Ms.";
                    student.Name = txtName.Text;
                    if (ddlGender.SelectedValue == "1")
                        student.Gender = "Female";
                    else
                        student.Gender = "Male";

                    if (!isBlank(txtName))
                    {
                        ShowAlert("Please enter Student Name.");
                        return;
                    }

                    if (Txt_GName.Enabled == true && Txt_Fname.Enabled == true && Txt_Mname.Enabled == true)
                    {

                        if ((isBlank(Txt_GName) && isBlank(Txt_Fname) && isBlank(Txt_Mname)) || (!isBlank(Txt_GName) && !isBlank(Txt_Fname) && !isBlank(Txt_Mname)))
                        {
                            ShowAlert("Please enter either (Father Name and Mother Name) OR  Guardian Name.");
                            return;
                        }
                        else if (!isBlank(Txt_Fname) && isBlank(Txt_Mname))
                        {
                            ShowAlert("Please enter Father Name.");
                            return;
                        }
                        else if (!isBlank(Txt_Mname) && isBlank(Txt_Fname))
                        {
                            ShowAlert("Please enter Mother Name.");
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(Txt_GName.Text) == true && string.IsNullOrWhiteSpace(Txt_GName.Text) == true)
                    {
                        student.FatherName = Txt_Fname.Text.Trim();
                        student.MotherName = Txt_Mname.Text.Trim();
                        student.GuardianName = null;
                    }
                    else
                    {
                        student.FatherName = null;
                        student.MotherName = null;
                        student.GuardianName = Txt_GName.Text.Trim();
                    }

                    //student.FatherName = Txt_Fname.Text;
                    //student.MotherName = Txt_Mname.Text;
                    student.DateOfBirth = Convert.ToDateTime(Txt_Dob.Text.ToString());
                    student.OccupationID = Convert.ToInt32(ddloccupation.SelectedValue);
                    student.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                    context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    //strMessage = "Record Updated";
                }

                //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BCCCandidatePersonalDetails.aspx?key1=" + Request.QueryString["key1"] + "&msg=" + strMessage));
                //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CertificateGuardain.aspx"));
                divedit.Visible = false;
                divfilter.Visible = true;
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CertificateGuardain.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&filter=" + Request.QueryString["filter"] + "&RegCentreID=" + Request.QueryString["RegCentreID"] + "&CouCategory=" + Request.QueryString["CouCategory"] + "&AppltypeID=" + Request.QueryString["AppltypeID"] + "&ResultType=" + Request.QueryString["ResultType"] + "&index=" + Request.QueryString["index"] + "&search=" + Request.QueryString["search"] + "&Searchtype=" + Request.QueryString["Searchtype"]), true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlSalutation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlSalutation.SelectedValue == "2")
                ddlGender.SelectedValue = "1";
            if (ddlSalutation.SelectedValue == "1")
                ddlGender.SelectedValue = "2";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            divedit.Visible = false;
            divfilter.Visible = true;
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CertificateGuardain.aspx?CourseID=" + Request.QueryString["CourseID"] + "&ExamID=" + Request.QueryString["ExamID"] + "&ExamCycleID=" + Request.QueryString["ExamCycleID"] + "&ExamYear=" + Request.QueryString["ExamYear"] + "&filter=" + Request.QueryString["filter"] + "&RegCentreID=" + Request.QueryString["RegCentreID"] + "&CouCategory=" + Request.QueryString["CouCategory"] + "&AppltypeID=" + Request.QueryString["AppltypeID"] + "&ResultType=" + Request.QueryString["ResultType"] + "&index=" + Request.QueryString["index"] + "&search=" + Request.QueryString["search"] + "&Searchtype=" + Request.QueryString["Searchtype"]), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Rdsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            TxtSearch.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void EnableDisableColumn()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //configurable update of columns
                var name = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Name));
                if (name.IsUpdate == true)
                    txtName.Enabled = true;
                else
                    txtName.Enabled = false;

                var dob = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.DOB));
                if (dob.IsUpdate == true)
                {
                    Txt_Dob.Enabled = true;
                    ceDOB.Enabled = true;
                }
                else
                {
                    Txt_Dob.Enabled = false;
                    ceDOB.Enabled = false;
                }

                var gender = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Gender));
                if (gender.IsUpdate == true)
                    ddlGender.Enabled = true;
                else
                    ddlGender.Enabled = false;

                var ocupation = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Occupation));
                if (ocupation.IsUpdate == true)
                    ddloccupation.Enabled = true;
                else
                    ddloccupation.Enabled = false;

                var catcategory = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.CasteCategory));
                if (catcategory.IsUpdate == true)
                    ddlCategory.Enabled = true;
                else
                    ddlCategory.Enabled = false;

                var fname = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.FatherName));
                if (fname.IsUpdate == true)
                    Txt_Fname.Enabled = true;
                else
                    Txt_Fname.Enabled = false;

                var mname = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.MotherName));
                if (mname.IsUpdate == true)
                    Txt_Mname.Enabled = true;
                else
                    Txt_Mname.Enabled = false;

                var guardian = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Guardian));
                if (guardian.IsUpdate == true)
                    Txt_GName.Enabled = true;
                else
                    Txt_GName.Enabled = false;

                var saltation = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Salutation));
                if (saltation.IsUpdate == true)
                    ddlSalutation.Enabled = true;
                else
                    ddlSalutation.Enabled = false;
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }

    }
}