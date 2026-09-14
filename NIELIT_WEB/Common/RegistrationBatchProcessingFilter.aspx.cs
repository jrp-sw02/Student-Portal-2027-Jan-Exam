using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;
using Ionic.Zip;
using System.Data.Objects;

public partial class Common_RegistrationBatchProcessingFilter : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                Ddlinstitutes.Enabled = false;
                BindCourseCategory();
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlGender, typeof(EConnect.Gender), new ListItem("--All--", "0"));
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCastCategory, typeof(EConnect.enmCastCategory), new ListItem("--All--", "0"));
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlregtype, typeof(EConnect.NIELIT.enmRegistrationType), new ListItem("--All--", "0"));
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Processing Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    protected void BindCourseCategory()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               join c in context.Courses on p.ID equals c.CourseCategoryID
                               where c.CourseTypeID == CourseType
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void bindinst()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                var institutes = from c in context.Institutes
                                 where c.ID == entityID
                                 select new
                                 {
                                     ValueField = c.ID,
                                     TextField = c.Name
                                 };
                if (institutes.Count() > 0)
                {
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddlinstitutes, institutes.Distinct(), lst);
                    Ddlinstitutes.SelectedValue = entityID.ToString();
                    Ddlinstitutes.Enabled = false;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseCatId > 0)
                {
                    var courses = from s in context.Courses
                                  where s.CourseCategoryID == courseCatId
                                  select new { ValueField = s.ID, TextField = s.Name };

                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    courses = courses.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                }
                else
                {
                    ddlCourseName.Items.Clear();
                    ddlCourseName.Items.Insert(0, lst);
                }
                ddlCourseName.SelectedValue = "0";
                ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
            };
            if (ddlCourseCategry.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlCandidateState.Items.Clear();
                ddlCandidateState.Items.Add(lst1);
                ddlGender.SelectedValue = "0";
                ddlCastCategory.SelectedValue = "0";
                ddlregtype.SelectedValue = "0";
                ddlapplicationtype.SelectedValue = "0";
                ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                txtDateFrom.Text = "";
                txtDateto.Text = "";
            }
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
            BreadCrumb1.Render();
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                          orderby p.ID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList.Take(1), lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
                ddlAppType.SelectedValue = "0";
                ddlAppType_SelectedIndexChanged(ddlapplicationtype, EventArgs.Empty);
            };
            if (ddlCourseName.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlCandidateState.Items.Clear();
                ddlCandidateState.Items.Add(lst1);
                ddlapplicationtype.SelectedValue = "0";
                ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                ddlGender.SelectedValue = "0";
                ddlCastCategory.SelectedValue = "0";
                ddlregtype.SelectedValue = "0";
                txtDateFrom.Text = "";
                txtDateto.Text = "";
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
            BreadCrumb1.Render();
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0 && ExamCycleId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                    {
                        Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                        if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        {
                            var courses = (from s in context.Exams
                                           join c in context.CourseRegistrationApplications
                                               on s.ID equals c.ApplicableExamID
                                           where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                           select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        }
                    }
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                }
                ddlExamName.SelectedValue = "0";
                ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
            }
            if (ddlExamCycle.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                ddlCandidateState.Items.Clear();
                ddlCandidateState.Items.Add(lst1);
                ddlapplicationtype.SelectedValue = "0";
                ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                ddlGender.SelectedValue = "0";
                ddlCastCategory.SelectedValue = "0";
                ddlregtype.SelectedValue = "0";
                txtDateFrom.Text = "";
                txtDateto.Text = "";
            }
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
            ddlregtype.SelectedValue = "0";
            ddlapplicationtype.SelectedValue = "0";
            ddlCandidateState.SelectedValue = "0";
            ddlApplStatus.SelectedValue = "V";
            if (ddlApplStatus.SelectedValue == "V")
                btnphoto.Visible = true;
            else
                btnphoto.Visible = false;
            txtDateFrom.Text = "";
            txtDateto.Text = "";
            //trinstitute.Visible = false;
            // trinstitute1.Visible = false;
            Ddlinstitutes.SelectedValue = "0";
            ddlbatchnumber.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlapplicationtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ListItem lst1 = new ListItem("--All--", "0");
            if (Convert.ToInt32(ddlapplicationtype.SelectedValue) == Convert.ToInt32(enmApplicantType.Direct))
            {
                Ddlinstitutes.Enabled = false;
                Ddlinstitutes.Items.Clear();
                Ddlinstitutes.Items.Add(lst1);
            }
            else if (Convert.ToInt32(ddlapplicationtype.SelectedValue) == Convert.ToInt32(enmApplicantType.Institute))
            {
                Ddlinstitutes.Enabled = true;
                Int32 ApplicationTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 examid = Convert.ToInt32(ddlExamName.SelectedValue);
                BindInstitutes(ApplicationTypeID, courseID, examid);
            }
            else
            {
                Ddlinstitutes.Items.Clear();
                Ddlinstitutes.Items.Add(lst1);
                Ddlinstitutes.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindInstitutes(Int32 ApplicationTypeID, Int32 courseID, Int32 examid)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            using (EConnectContext context = new EConnectContext())
            {

                if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {

                    var institutes = from c in context.CourseRegistrationApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     join r in context.RegistrationDetails
                                        on c.ID equals r.CourseRegistrationApplicationID
                                     where a.CourseID == courseID && c.ApplicableExamID == examid
                                     select new
                                     {
                                         ValueField = i.ID,
                                         TextField = i.Name + "(" + a.AccreditationNumber + ")"
                                     };
                    if (institutes.Count() > 0)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(Ddlinstitutes, institutes.Distinct(), lst);
                    }
                    else
                    {
                        ddlapplicationtype.SelectedValue = "1";
                        ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 stid = Convert.ToInt32(enmLocationType.State);
            Int32 batchstatus = Convert.ToInt32(enmBatchStatus.Completed);
            Int32 ApplicanttypeID = Convert.ToInt32(ddlapplicationtype.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    var capp = from cr in context.CourseRegistrationApplications
                               join lo in context.Locations on cr.CorStateID equals lo.ID
                               join r in context.RegistrationDetails on cr.ID equals r.CourseRegistrationApplicationID
                               where cr.ApplicableExamID == ExamID && cr.CourseID == CourseID && lo.LocationTypeID == stid && cr.FinalSubmitted == true
                               orderby lo.Name
                               select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst = new ListItem("--All--", "0");


                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCandidateState, capp.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst);
                }
            };
            ddlCandidateState.SelectedValue = "0";
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
            BreadCrumb1.Render();
            ListItem lst = new ListItem("--All--", "0");
            ListItem lst2 = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                if (AppTypeId > 0 && CourseId > 0)
                {

                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                }
                else
                {
                    ddlExamCycle.Items.Clear();
                    ddlExamCycle.Items.Insert(0, lst2);
                }
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
                if (ddlAppType.SelectedValue == "0")
                {
                    ListItem lst12 = new ListItem("--All--", "0");
                    ddlCandidateState.Items.Clear();
                    ddlCandidateState.Items.Add(lst12);
                    ddlapplicationtype.SelectedValue = "0";
                    ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                    ddlGender.SelectedValue = "0";
                    ddlCastCategory.SelectedValue = "0";
                    ddlregtype.SelectedValue = "0";
                    txtDateFrom.Text = "";
                    txtDateto.Text = "";
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnphoto_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 instid = 0;
                DateTime regFromDate = Convert.ToDateTime(txtDateFrom.Text);
                DateTime regToDate = Convert.ToDateTime(txtDateto.Text);
                Int32 courseid = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 examid = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 gender = Convert.ToInt32(ddlGender.SelectedValue);
                Int32 stateid = Convert.ToInt32(ddlCandidateState.SelectedValue);
                Int32 apptypeID = Convert.ToInt32(ddlapplicationtype.SelectedValue);
                Int32 category = Convert.ToInt32(ddlCastCategory.SelectedValue);
                if (apptypeID == Convert.ToInt32(enmApplicantType.Institute))
                {
                    instid = Convert.ToInt32(Ddlinstitutes.SelectedValue);
                }
                Int32 regtypeid = Convert.ToInt32(ddlregtype.SelectedValue);

                //Main query.
                var images = (from a in context.RegistrationDetails
                              join c in context.CourseRegistrationApplications
                              on a.CourseRegistrationApplicationID equals c.ID
                              join b in context.BatchItems
                                  on c.BatchItemID equals b.ID
                              where c.ApplicableExamID == examid && c.CourseID == courseid && c.FinalSubmitted == true
                              select new
                              {

                                  Photo = c.Photo,
                                  Signature = c.Signature,
                                  Regdate = System.Data.Entity.DbFunctions.TruncateTime(a.RegistrationDate),
                                  ReRegdate = a.RegistrationTypeID == 4 ? System.Data.Entity.DbFunctions.TruncateTime(a.ReRegistrationDate) : null,
                                  Regno = a.RegistrationNo,
                                  ApplicantTypeID = c.ApplicantTypeID,
                                  InstituteID = c.ApplicantTypeID == 2 ? c.InstituteID : 0,
                                  Gender = c.Gender,
                                  CastCategoryID = c.CastCategoryID,
                                  CorStateID = c.CorStateID,
                                  Regtypeid = a.RegistrationTypeID != null ? a.RegistrationTypeID.Value : 0
                              }).ToList();

                //Filter Criteria
                if (apptypeID == 1)
                {
                    images = images.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                }
                else if (apptypeID == 2)
                {
                    images = images.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                    if (instid != 0)
                    {
                        images = images.Where(s => s.InstituteID == instid).ToList();
                    }
                }
                if (gender == 1)
                {
                    string gendername = "Female";
                    images = images.Where(s => s.Gender.ToUpper().Trim() == gendername.ToUpper().Trim()).ToList();
                }
                else if (gender == 2)
                {
                    string gendername = "Male";
                    images = images.Where(s => s.Gender.ToUpper().Trim() == gendername.ToUpper().Trim()).ToList();
                }
                if (category != 0)
                {
                    images = images.Where(s => s.CastCategoryID == category).ToList();
                }
                if (stateid != 0)
                {
                    images = images.Where(s => s.CorStateID == stateid).ToList();
                }
                if (regtypeid == 4)
                {
                    images = images.Where(s => s.ReRegdate >= regFromDate && s.ReRegdate <= regToDate && s.Regtypeid == regtypeid).ToList();
                }
                else
                {
                    images = images.Where(s => s.Regdate >= regFromDate && s.Regdate <= regToDate && s.Regtypeid == regtypeid).ToList();
                }

                String directoryName = ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "");
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName));
                string photoName = "";
                foreach (var photo in images)
                {
                    photoName = photo.Regno.ToString();
                    FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_photo.jpeg"));
                    FileStream fs1 = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_sign.jpeg"));
                    BinaryWriter bw = new BinaryWriter(fs);
                    BinaryWriter bw1 = new BinaryWriter(fs1);
                    bw.Write(photo.Photo);
                    bw1.Write(photo.Signature);
                    bw.Close();
                    fs.Close();
                    bw1.Close();
                    fs1.Close();
                }
                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
                    zipFile.Save(Response.OutputStream);
                }
                System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName), true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlApplStatus_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            string appstatus = Convert.ToString(ddlApplStatus.SelectedValue);
            if (appstatus.Trim().ToUpper() == "V")
            {
                btnphoto.Visible = true;
            }
            else
            {
                btnphoto.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void txtDateFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                BreadCrumb1.Render();
                DateTime regFromDate = Convert.ToDateTime(txtDateFrom.Text);
                DateTime regToDate = Convert.ToDateTime(txtDateto.Text);
                Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 stid = Convert.ToInt32(enmLocationType.State);
                Int32 batchstatus = Convert.ToInt32(enmBatchStatus.Completed);
                Int32 ApplicanttypeID = Convert.ToInt32(ddlapplicationtype.SelectedValue);
                Int32 regtypeID = Convert.ToInt32(ddlregtype.SelectedValue);
                Int32[] BatchID;

                if (regtypeID == 4)
                {
                    BatchID = (from r in context.RegistrationDetails
                               join c in context.CourseRegistrationApplications
                                   on r.CourseRegistrationApplicationID equals c.ID
                               join b in context.BatchItems
                                  on c.BatchItemID equals b.ID
                               where System.Data.Entity.DbFunctions.TruncateTime(r.ReRegistrationDate.Value) >= regFromDate && System.Data.Entity.DbFunctions.TruncateTime(r.ReRegistrationDate.Value) <= regToDate
                               select b.BatchID).ToArray();
                }
                else
                {
                    BatchID = (from r in context.RegistrationDetails
                               join c in context.CourseRegistrationApplications
                                   on r.CourseRegistrationApplicationID equals c.ID
                               join b in context.BatchItems
                                  on c.BatchItemID equals b.ID
                               where System.Data.Entity.DbFunctions.TruncateTime(r.RegistrationDate) >= regFromDate && System.Data.Entity.DbFunctions.TruncateTime(r.RegistrationDate) <= regToDate
                               select b.BatchID).ToArray();
                }

                ListItem lst1 = new ListItem("All", "0");
                if (ApplicanttypeID != 0)
                {
                    var batchno = from b in context.Batchs
                                  where b.CourseID == CourseID && b.ExamID == ExamID && b.ApplicationTypeID == AppTypeID && b.StatusID == batchstatus && b.ApplicantTypeID == ApplicanttypeID && BatchID.Contains(b.ID)
                                  select new { ValueField = b.ID, TextField = b.Number };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchnumber, batchno.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst1);
                }
                else
                {
                    var batchno = from b in context.Batchs
                                  where b.CourseID == CourseID && b.ExamID == ExamID && b.ApplicationTypeID == AppTypeID && b.StatusID == batchstatus && BatchID.Contains(b.ID)
                                  select new { ValueField = b.ID, TextField = b.Number };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchnumber, batchno.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst1);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void txtDateto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                BreadCrumb1.Render();
                DateTime regFromDate = Convert.ToDateTime(txtDateFrom.Text);
                DateTime regToDate = Convert.ToDateTime(txtDateto.Text);
                Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 stid = Convert.ToInt32(enmLocationType.State);
                Int32 batchstatus = Convert.ToInt32(enmBatchStatus.Completed);
                Int32 ApplicanttypeID = Convert.ToInt32(ddlapplicationtype.SelectedValue);
                Int32 regtypeID = Convert.ToInt32(ddlregtype.SelectedValue);
                Int32[] BatchID;

                if (regtypeID == 4)
                {
                    BatchID = (from r in context.RegistrationDetails
                               join c in context.CourseRegistrationApplications
                                   on r.CourseRegistrationApplicationID equals c.ID
                               join b in context.BatchItems
                                  on c.BatchItemID equals b.ID
                               where System.Data.Entity.DbFunctions.TruncateTime(r.ReRegistrationDate.Value) >= regFromDate && System.Data.Entity.DbFunctions.TruncateTime(r.ReRegistrationDate.Value) <= regToDate
                               select b.BatchID).ToArray();
                }
                else
                {
                    BatchID = (from r in context.RegistrationDetails
                               join c in context.CourseRegistrationApplications
                                   on r.CourseRegistrationApplicationID equals c.ID
                               join b in context.BatchItems
                                  on c.BatchItemID equals b.ID
                               where System.Data.Entity.DbFunctions.TruncateTime(r.RegistrationDate) >= regFromDate && System.Data.Entity.DbFunctions.TruncateTime(r.RegistrationDate) <= regToDate
                               select b.BatchID).ToArray();
                }
                ListItem lst1 = new ListItem("All", "0");
                if (ApplicanttypeID != 0)
                {
                    var batchno = from b in context.Batchs
                                  where b.CourseID == CourseID && b.ExamID == ExamID && b.ApplicationTypeID == AppTypeID && b.StatusID == batchstatus && b.ApplicantTypeID == ApplicanttypeID && BatchID.Contains(b.ID)
                                  select new { ValueField = b.ID, TextField = b.Number };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchnumber, batchno.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst1);
                }
                else
                {
                    var batchno = from b in context.Batchs
                                  where b.CourseID == CourseID && b.ExamID == ExamID && b.ApplicationTypeID == AppTypeID && b.StatusID == batchstatus && BatchID.Contains(b.ID)
                                  select new { ValueField = b.ID, TextField = b.Number };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchnumber, batchno.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst1);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}