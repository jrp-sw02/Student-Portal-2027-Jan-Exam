 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;

public partial class HO_DownloadApplications_course : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationCourse);
    Int32 currentRoleId = 0;
    Int32 paymentPendingStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
    //Int32 applInstituteStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
    Int32 applInstituteStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);   
    //Int32 applDirectStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
    Int32 applDirectStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
    EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
    SqlDataAdapter da = new SqlDataAdapter();   
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            ddlRc.Enabled = false;
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
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindCourseCategory();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Applications", "#", ""));
                }
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Applications", "#", ""));
                btnView.Visible = false;
                btnReset.Visible = false;
                Lblerror.Text = "You can not download Candidate Applications.";
                Lblerror.Visible = true;
            }
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message, true);
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                //var courses = from s in context.CourseCategories
                //              join c in context.Courses
                //                  on s.ID equals c.CourseCategoryID
                //              where c.CourseTypeID == courseTypeCertificateExam
                //              select new { ValueField = s.ID, TextField = s.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);


                //---------------- edit by chhavi
                //var courses = from s in context.CourseCategories
                //              join c in context.Courses
                //                  on s.ID equals c.CourseCategoryID
                //              //--where (c.CourseTypeID == courseTypeCertificateExam || s.ID == 6 || s.ID==1)
                //              where !(s.ID == 2 || s.ID == 6 || s.ID == 8 || s.ID == 9)
                //              select new { ValueField = s.ID, TextField = s.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

                //-------------------


                var courses = from s in context.Courses
                              join m in context.CourseCategories on s.CourseCategoryID equals m.ID
                              where s.CourseTypeID == courseTypeCertificateExam && m.ID != 8 && m.ID != 6 && m.ID != 9
                              select new { ValueField = m.ID, TextField = m.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

                                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                ListItem lst1 = new ListItem("--ALL--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                if (loginUserType == UserType.RegionalCenter)
                {
                    ddlRc.SelectedValue = entityID.ToString();
                    ddlRc.Enabled = false;
                }

            }
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
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            //mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "Download Applications";
            using (EConnectContext context = new EConnectContext())
            {
                Int32 AppID = Convert.ToInt32(Request.QueryString["Key"]);
                //var applications = (from p in context.CourseExamApplications
                //                    where p.ID == AppID
                //                    select p).FirstOrDefault();
                var applications = (from p in context.CertificateExamApplications
                                    where p.ID == AppID
                                    select p).FirstOrDefault();



                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(applications.Number + "-" + "(" + GetInitCap(applications.Name) + ")", "HO/DownloadApplications_course.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            //PagingBar1.CurrentPageIndex = 0;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            //PagingBar1.CurrentPageIndex = 0;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {


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

            //PagingBar1.CurrentPageIndex = 0;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            //BindGridView();
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            //mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
        }
        else
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
            if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null && Request.QueryString["examcycle"] != null && Request.QueryString["examyear"] != null && Request.QueryString["coursecategory"] != null)
            {
                Response.Redirect("DownloadApplications_course.aspx?cname=" + Request.QueryString["CourseID"] + "&exam=" + Request.QueryString["ExamID"] + "&examcycle=" + Request.QueryString["examcycle"] + "&examyear=" + Request.QueryString["examyear"] + "&coursecategory=" + Request.QueryString["coursecategory"], true);
            }
            else
                Response.Redirect("DownloadApplications_course.aspx", true);

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
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            //var applications = from s in context.CertificateExamApplications

            var applications = from s in context.Candidates
                               select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.OrderBy(s => s.Name).Distinct();
            //var applications1 = from s in context.CertificateExamApplications
            var applications1 = from s in context.Candidates
                                select new { Name = s.FatherName };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications1 = applications1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.Union(applications1).Take(count);
            foreach (var c in applications)
            {
                items.Add(c.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.RemoveLastBreadCrumbItem();
        if (Request.QueryString["CourseID"] != null && Request.QueryString["ExamID"] != null && Request.QueryString["examcycle"] != null && Request.QueryString["examyear"] != null && Request.QueryString["coursecategory"] != null)
        {
            Response.Redirect("DownloadApplications_course.aspx?cname=" + Request.QueryString["CourseID"] + "&exam=" + Request.QueryString["ExamID"] + "&examcycle=" + Request.QueryString["examcycle"] + "&examyear=" + Request.QueryString["examyear"] + "&coursecategory=" + Request.QueryString["coursecategory"], true);
        }
        else
            Response.Redirect("DownloadApplications_course.aspx", true);
    }
    protected void imgAccess_Click(object sender, ImageClickEventArgs e)
    {
        OleDbConnection connection = new OleDbConnection(); ;
        OleDbCommand command = new OleDbCommand();
        //int CourseId = Convert.ToInt32(ddlcourse.SelectedValue);
        //Course currentCourse = context.Courses.Find(CourseId);
        //int ExamId = Convert.ToInt32(ddlexamname.SelectedValue);
        //RegionalCenter regional = context.RegionalCenters.Find(entityID);
        //string Access = "";
        //var filename=  regional.Name + "- " + "Applications_"+ currentCourse.Code+ "_" +DateTime.Now.ToString("ddMMyyyyhhmmss")+".mdb";
        ////RC-CHA_Apllications_CCC_Jan2013.mdb
        try
        {
            //    if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
            //    {
            //        var result = (from c in context.CertificateExamApplications
            //                      where c.CourseID == CourseId && c.ExamID == ExamId
            //                      && c.RegionalCenterID == entityID
            //                      select new
            //                      {
            //                          Folder_no = "",
            //                          batch_no = "",
            //                          sr_no = c.ID,
            //                          exam_year = c.Exam.Name,
            //                          NAME = c.Name,
            //                          F_NAME = c.FatherName,
            //                          M_NAME = c.MotherName,
            //                          D_O_B = c.DateOfBirth,
            //                          SEX = c.Gender,
            //                          H_Qual = c.EducationalQualification.Code,
            //                          ADD1 = c.CorAddressLine1,
            //                          ADD2 = c.CorAddressLine2,
            //                          ADD3 = c.CorAddressLine3,
            //                          CITY = c.CorDistrict.Name,
            //                          STATE = c.CorState.Name,
            //                          PINCODE = c.CorPinCode,
            //                          PH_NO_C = (c.PhoneNumber.HasValue ? c.PhoneNumber.Value : 0),
            //                          EMAIL_C = c.EmailAddress,
            //                          CCC_NO = (c.InstituteID.HasValue ? c.InstituteID.Value : 0),
            //                          INST_NAME = c.Institute.Name,
            //                          courseID = c.CourseID,
            //                          examID = c.ExamID,
            //                          INST_ADD = c.Institute.AddressLine1 + "  " + c.Institute.AddressLine2 + "   " + c.Institute.State.Name,
            //                          INST_STAT = c.Institute.AccreditationDetails.FirstOrDefault().AccreditationStatus.Code,
            //                          TH_CENT_CH = c.ExamCenter1.Code,
            //                          SEC_TH_CEN = c.ExamCenter2.Code,
            //                          OCCUPATION = c.Occupation.Name,
            //                          CATEGORY = c.CastCategory.Code,
            //                          PREV_APP = (c.AlreadyApplied == true) ? "Y" : "N",
            //                          PREV_M = c.PreviousExam.Name,
            //                          PREV_YEAR = "",//need to work on this
            //                          PREV_ROLL = (c.PreviousRollNumber.Trim()!="" && c.PreviousRollNumber!=null) ? c.PreviousRollNumber.ToUpper() : "NA",
            //                          Rollno = "",
            //                          cent_allot = "",
            //                          cent_add = "",
            //                          examDate = c.Exam.ExamStartDate,
            //                          batch = "",
            //                          rep_time = "",
            //                      });
            //           Access = Server.MapPath("~/Download/" + filename);
            //           if(CourseId!=0 && ExamId!=0)
            //           result = result.Where(p => p.courseID == CourseId && p.examID == ExamId);
            //           if(System.IO.File.Exists(Access))
            //              System.IO.File.Delete(Access);
            //           System.IO.File.Copy(Server.MapPath("~/Download/Database_format_BCC.mdb"), Access);
            //           string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Access + ";Persist Security Info=False;";
            //           connection.ConnectionString = connect;
            //           connection.Open();
            //           command = new OleDbCommand("delete from  [MS Access;Database=" + Access + "].[Exam_Database]", connection);
            //           command.ExecuteNonQuery();
            //           foreach (var certificate in result)
            //           {
            //            command = new OleDbCommand("insert into [MS Access;Database=" + Access + "].[Exam_Database](Folder_no, batch_no,sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time)values('" + certificate.Folder_no + "','" + certificate.batch_no + "','" + certificate.sr_no + "','" + certificate.exam_year + "','" + certificate.NAME + "','" + certificate.F_NAME + "','" + certificate.M_NAME + "','" + certificate.D_O_B + "','" + certificate.SEX + "','" + certificate.H_Qual + "','" + certificate.ADD1 + "','" + certificate.ADD2 + "','" + certificate.ADD3 + "','" + certificate.CITY + "','" + certificate.STATE + "','" + certificate.PINCODE + "','" + certificate.PH_NO_C + "','" + certificate.EMAIL_C + "','" + certificate.CCC_NO + "','" + certificate.INST_NAME + "','" + certificate.INST_ADD + "','" + certificate.INST_STAT + "','" + certificate.TH_CENT_CH + "','" + certificate.SEC_TH_CEN + "','" + certificate.OCCUPATION + "','" + certificate.CATEGORY + "','" + certificate.PREV_APP + "','" + certificate.PREV_M + "','" + certificate.PREV_YEAR + "','" + certificate.PREV_ROLL + "','" + certificate.Rollno + "','" + certificate.cent_allot + "','" + certificate.cent_add + "','" + certificate.examDate + "','" + certificate.batch + "','" + certificate.rep_time + "')", connection);
            //            command.ExecuteNonQuery();
            //           }
            //           Response.Clear();
            //           Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //           Response.Charset = "";
            //           Response.ContentType = "Application/vnd.mdb";
            //           //Response.TransmitFile(Access);
            //           Response.WriteFile(Access);
            //           Response.End();
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            //context.Dispose();
            command.Dispose();
            connection.Dispose();
            connection.Close();
        }
    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddl_course_type.Enabled = true;
        //try
        //{
        //    Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
        //    ListItem lst = new ListItem("--Select One--", "0");
        //    using (EConnectContext context = new EConnectContext())
        //    {
        //        Course cr = context.Courses.Find(cid);
        //        if (cr != null)
        //        {
        //            var ApplicationList = from p in context.ApplicationTypes
        //                                  where p.CourseTypeID == cr.CourseTypeID
        //                                  select new { ValueField = p.ID, TextField = p.Name };
        //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
        //        }
        //        else
        //        {
        //            ddlAppType.Items.Clear();
        //            ddlAppType.Items.Insert(0, lst);
        //        }
        //    };
        //    ddlAppType.SelectedValue = "0";
        //    ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}


        try
        {
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID && p.Code=="LE"
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            ddlAppType.SelectedValue = "0";
            ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    
    //protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ListItem lst = new ListItem("--Select One--", "0");
    //        Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
    //        if (AppTypeId > 0)
    //        {
    //            using (EConnectContext context = new EConnectContext())
    //            {
    //                Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
    //                ListItem lst1 = new ListItem("--Select One--", "0");
    //                var courses = from s in context.ExaminationCycles
    //                              join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
    //                              where s.CourseID == CourseId
    //                              select new { ValueField = s.ID, TextField = s.Name };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);

    //            };
    //        }
    //        else
    //        {
    //            ddlExamCycle.Items.Clear();
    //            ddlExamCycle.Items.Insert(0, lst);
    //            ddlExamCycle.SelectedValue = "0";
    //            ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}


    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)                 //edit by chhavi   
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (courseCatId == 2)
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

                       
                        ListItem lst1 = new ListItem("--Select One--", "0");
                        var courses = from s in context.Exams
                                      join a in context.ExaminationCycles on s.ExaminationCycleID equals a.ID
                                      where a.CourseID == CourseId
                                      select new { ValueField = s.ExaminationCycleID, TextField = a.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                    }

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
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    var courses = (from s in context.Exams
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                   select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
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
            }
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
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                   && s.DateOfPublishingOfTimeTable != null
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                    ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (ddlExamName.SelectedValue != "0")
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //Int32 applicationStatus = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
                    Int32 applicationStatus = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing);
                    Int32 applInstituteStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                    Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
                    Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                    Int32 courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                    Int32 regionalCentreID = Convert.ToInt32(ddlRc.SelectedValue);
                    divBatchDetails.Visible = true;
                    if (context.Exams.Find(examID).IsBatchProcessable)
                    {
                        BindGridView();
                        lblApplicationsDetails.Visible = false;
                        tblGrid.Visible = true;
                    }
                    else
                    {
                        tblGrid.Visible = false;
                        var appls = context.CourseExamApplications.Where(a => a.ExamID == examID && (a.ApplicationStatusID == applDirectStatusID || a.ApplicationStatusID == applInstituteStatusID ) && a.PaymentStatusID !=paymentPendingStatusID && a.FinalSubmitted == true && a.DemandNoteID != null
                                     && context.DemandNotes.Any(s => s.ID == a.DemandNoteID && (s.PaymentStatusID == 2 || s.PaymentStatusID == 4) && (s.OnlineTransactionID != null || s.NEFTTransactionID != null || s.CSCTransaction_ID != null)));

                        //var appls = context.CourseExamApplications.Where(application => application.PaymentStatusID == 1).Select(application => application.ID).Take(100).ToList();



                        var dataFromDetails = (from m in context.CourseExamApplicationDetails join n in appls on m.CourseExamApplicationID equals n.ID where m.ResultGradeID != 191 || m.ResultGradeID != 12 || m.ResultGradeID != 7 select m);

                        var dataFromDetails_Theory = (from m in context.CourseExamApplicationDetails join n in appls on m.CourseExamApplicationID equals n.ID where (m.ResultGradeID != 191 || m.ResultGradeID != 12 || m.ResultGradeID != 7) && (m.Module.ModuleTypeID == 1 || m.Module.ModuleTypeID == 5) select m);

                        var dataFromDetails_Practical = (from m in context.CourseExamApplicationDetails join n in appls on m.CourseExamApplicationID equals n.ID where m.Module.ModuleTypeID == 3 && (m.ResultGradeID != 191 || m.ResultGradeID != 12 || m.ResultGradeID != 7) select m);
                        //if (ddlRc.SelectedValue != "0")
                        //{
                        //    appls = appls.Where(b => b.RegionalCenterID == regionalCentreID);
                        //}
                        if (appls != null && dataFromDetails != null)
                        {
                            // lblApplicationsDetails.Text = "Batch Processing is not applicable for selected exam. Total Number of Applications: " + appls.Count(); Commented on 22 Sep 2021 because Batch Processing is no more applicable.

                            //string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                            //SqlConnection con = new SqlConnection(constr);
                            //con.Open();

                            ////string sql = "select  count(*)  Pending_Count  from Certificate_Exam_Application d where exam_id = '" + examID + "' and d.Regional_Center_ID = '" + regionalCentreID + "' and Course_ID = '" + courseId + "'   and (Roll_Number is null or   d.Result_Grade_ID = 19 ) and Payment_Status_ID in (2,4) and Application_Status_ID =17  group by d.Exam_ID  , d.Course_ID, d.Regional_Center_ID order by Pending_Count ";
                            ////using (SqlConnection conn = new SqlConnection(constr))
                            ////{
                            ////    using (SqlCommand cmd = new SqlCommand(sql))
                            ////    {
                            ////        cmd.Connection = conn;
                            ////        SqlDataReader sdr = new SqlDataReader();

                            ////            //sda.Fill(dt1);
                            ////            //Int64 pC = sda.
                            ////            sdr = cmd.ExecuteReader();

                            ////            Int64 pCC =  Convert.ToInt64( sdr);
                            ////    }
                            ////}

                            //Int64 pC = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteReader("select count(*) Pending_Count from Certificate_Exam_Application d where exam_id = '" + examID + "' and d.Regional_Center_ID = '" + regionalCentreID  + "' and Course_ID = '" + courseId + "'   and (Roll_Number is null or   d.Result_Grade_ID = 19 ) and Payment_Status_ID in (2,4) and Application_Status_ID =17  group by d.Exam_ID ,d.Course_ID, d.Regional_Center_ID order by Pending_Count ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                            dataCountdiv.Visible = true;
                            lblApplicationsDetails.Text = " Total Number of Applications: " + appls.Count();
                            lblApplicationsDetails.Visible = true;

                            lblApplicationsDetails_1.Text = " Total Number of Modules Applied: " + dataFromDetails.Count();
                            lblApplicationsDetails_1.Visible = true;
                            lblApplicationsDetails_3.Visible = false;
                            lblApplicationsDetails_2.Visible = false;
                            if (ddl_course_type.SelectedValue == "1" && dataFromDetails_Theory!=null)
                            {
                                lblApplicationsDetails_2.Text = "Total Theory Modules Applied: " + dataFromDetails_Theory.Count();
                                lblApplicationsDetails_2.Visible = true;
                                lblApplicationsDetails.Style.Add("Font-Size","15px");
                                lblApplicationsDetails_1.Style.Add("Font-Size", "15px");
                                lblApplicationsDetails_2.Style.Add("Font-Size", "15px");
                                lblApplicationsDetails_3.Visible = false;
                            }
                            if (ddl_course_type.SelectedValue == "2" && dataFromDetails_Practical!=null)
                            {
                                lblApplicationsDetails_3.Text = "Total Practical Modules Applied: " + dataFromDetails_Practical.Count();
                                lblApplicationsDetails_3.Visible = true;
                                lblApplicationsDetails.Style.Add("Font-Size", "15px");
                                lblApplicationsDetails_1.Style.Add("Font-Size", "15px");
                                lblApplicationsDetails_3.Style.Add("Font-Size", "15px");
                                lblApplicationsDetails_2.Visible = false;
                            }
                            if (ddl_course_type.SelectedValue == "3" && dataFromDetails_Theory != null && dataFromDetails_Practical!=null)
                            {
                                lblApplicationsDetails_2.Text = "Total Theory Modules Applied: " + dataFromDetails_Theory.Count();
                                lblApplicationsDetails_2.Visible = true;

                                lblApplicationsDetails_3.Text = "Total Practical Modules Applied: " + dataFromDetails_Practical.Count();
                                lblApplicationsDetails_3.Visible = true;
                                lblApplicationsDetails.Style.Add("Font-Size", "11px");
                                lblApplicationsDetails_1.Style.Add("Font-Size", "11px");
                                lblApplicationsDetails_2.Style.Add("Font-Size", "11px");
                                lblApplicationsDetails_3.Style.Add("Font-Size", "11px");
                            }
                            btnPendingCount.Visible = true;
                            //gvPendingCount.Visible = true;

                            // con.Close();
                        }

                    }
                };
            }
            else
                divBatchDetails.Visible = false;

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
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 couCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                Int32 couID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                if (applType != 0 && examID != 0 && couID != 0 && couCatId != 0)
                {
                    var batches = from b in context.Batchs
                                  where (b.ApplicationTypeID == applType && b.ExamID == examID &&
                                         b.CourseCategoryID == couCatId && b.CourseID == couID)
                                  select new
                                  {
                                      ID = b.ID,
                                      BatchNumber = b.Number,
                                      RegionalCenterID = b.RegionalCenterID,
                                      applCount = (from p in context.BatchItems
                                                   where p.BatchID == b.ID
                                                   select p).Count(),
                                      Status = b.StatusID
                                  };

                    if (ddlRc.SelectedValue != "0")
                    {
                        Int32 regionalCentreID = Convert.ToInt32(ddlRc.SelectedValue);
                        batches = batches.Where(b => b.RegionalCenterID == regionalCentreID);
                    }

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    batches = batches.OrderByDescending(s => s.ID);
                                else
                                    batches = batches.OrderBy(s => s.ID);
                                break;
                            case "BatchNumber":
                                if (sortOrder == "DESC")
                                    batches = batches.OrderByDescending(s => s.BatchNumber);
                                else
                                    batches = batches.OrderBy(s => s.BatchNumber);
                                break;
                            case "Status":
                                if (sortOrder == "DESC")
                                    batches = batches.OrderByDescending(s => s.Status);
                                else
                                    batches = batches.OrderBy(s => s.Status);
                                break;
                            case "applCount":
                                if (sortOrder == "DESC")
                                    batches = batches.OrderByDescending(s => s.applCount);
                                else
                                    batches = batches.OrderBy(s => s.applCount);
                                break;
                            default:
                                batches = batches.OrderBy(s => s.ID);
                                break;
                        }
                    }

                    PagingBar1.Bind(batches, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblErrorGrid.Text = "No record found.";
                        lblErrorGrid.Visible = true;
                    }
                    else
                    {
                        lblErrorGrid.Visible = false;
                    }
                }
            };
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        //finally
        //{
        //    context.Dispose();
        //}

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
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
            ddlExamYear.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlRc.SelectedValue = "0";
            divBatchDetails.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        string mdbFilePath = "";
        String filename = "";
        //String directoryName = "";

        try
        {
            context = new EConnectContext();
            //DataTable dtTbl = new DataTable();

            DataTable dtTbl = bindcandidaterecords_for_CHMO();
            Int32 completed = Convert.ToInt32(enmBatchStatus.Completed);
            //Int32 applicationStatus = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
            Int32 applicationStatus = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            Course currentCourse = context.Courses.Find(CourseId);
            Int32 examId = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 regCentreId = Convert.ToInt32(ddlRc.SelectedValue);
            string courseCode = currentCourse.Code.ToString();


            if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
            {
               
                //string regCentreCode = context.RegionalCenters.Find(regCentreId).Code;
                //string courseCode = currentCourse.Code.ToString();


                //----------------------Edit by chhavi start
                  //if (ddlRc.SelectedValue != "0" && ddlCourseCategry.SelectedValue == "3")
                      if (ddlRc.SelectedValue != "0")
                    {
                        filename = ddlRc.SelectedItem + "_" + courseCode.Replace("/", "_") + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".mdb";

                    }
                    else
                    {
                        //filename = "CHM_O_LEVEL_" + "_" + courseCode.Replace("/", "_") + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".mdb";
                        filename =   courseCode.Replace("/", "_") + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".mdb";


                    }

                    //mdbFilePath = Server.MapPath("~/Download/" + filename);
                    //if (System.IO.File.Exists(mdbFilePath))
                    //    System.IO.File.Delete(mdbFilePath);

                    //System.IO.File.Copy(Server.MapPath("~/Download/ABCDE.accdb"), mdbFilePath);

                    //string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                    //connection.ConnectionString = connect;
                    //connection.Open();
                    //command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[course_exam_app_detail_jul23]", connection);
                    //command.ExecuteNonQuery();
                    //String sqlStr = "";
                  mdbFilePath = Server.MapPath("~/Download/" + filename);
                  if (System.IO.File.Exists(mdbFilePath))
                      System.IO.File.Delete(mdbFilePath);
                  System.IO.File.Copy(Server.MapPath("~/Download/Course_Exam_DownloadApplication.mdb"), mdbFilePath);
                  string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                  connection.ConnectionString = connect;
                  connection.Open();
                //command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[Exam_Database]", connection);

                //December_2024
                command = new OleDbCommand("delete from  [MS Access;Database=@mdbFilePath].[Exam_Database]", connection);
                command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath);
                command.ExecuteNonQuery();
                  String sqlStr = "";



                  if (ddlRc.SelectedValue == "0")
                      for (int i = 0; i < dtTbl.Rows.Count; i++)
                      {
                        sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[Exam_Database] (Course_name, Exam_Paper_Name, Module_Code, Module_Short_Name," +
                                " Previous_applied, Candidate_registration_no, registration_no_Valid_Upto_Date, Center_First_Choice_code, Center_First_Choice_City_Name, Center_Second_Choice_code, Center_Second_Choice_City_Name, Candidate_Name, Candidate_Gender, Father_Name, Mother_Name, Guardian_Name," +
                                " Candidate_Birth_date, Candidate_Email, Candidate_Mobile, Candidate_Corr_Address, Candidate_Perm_Address, Institute_Name, Roll_Number, Is_Handicaped, Course_ID, level_code, Exam_ID, applicationId, detailId, Module_ID, Candidate_Corr_Address_City, Candidate_Corr_Address_State, Candidate_Perm_Address_City, Candidate_Perm_Address_State, PercentageOfDisability, aadhaarNo )" +
                                " values('" + dtTbl.Rows[i]["Course_name"] + "','" + dtTbl.Rows[i]["Exam_Paper_Name"] + "','" + dtTbl.Rows[i]["Module_Code"] + "','" + dtTbl.Rows[i]["Module_Short_Name"] + "','" + dtTbl.Rows[i]["Previous_applied"].ToString() + "','" + dtTbl.Rows[i]["Candidate_registration_no"] + "','" + dtTbl.Rows[i]["registration_no_Valid_Upto_Date"].ToString().Replace("'", "") + "','"
                                + dtTbl.Rows[i]["Center_First_Choice_code"].ToString() + "','" + dtTbl.Rows[i]["Center_First_Choice_City_Name"].ToString() + "','" + dtTbl.Rows[i]["Center_Second_Choice_code"].ToString() + "','" + dtTbl.Rows[i]["Center_Second_Choice_City_Name"].ToString() + "','" + dtTbl.Rows[i]["Candidate_Name"] + "','" + dtTbl.Rows[i]["Candidate_Gender"] + "','" + dtTbl.Rows[i]["Father_Name"].ToString().Replace("'", "''") + "','"
                                + dtTbl.Rows[i]["Mother_Name"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Guardian_Name"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Candidate_Birth_date"].ToString().Replace("'", "") + "','" + dtTbl.Rows[i]["Candidate_Email"] + "','" + dtTbl.Rows[i]["Candidate_Mobile"] + "','"
                                + dtTbl.Rows[i]["Candidate_Corr_Address"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Candidate_Perm_Address"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Institute_Name"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Roll_Number"] + "','" + dtTbl.Rows[i]["Is_Handicaped"] + "','"
                                + dtTbl.Rows[i]["Course_ID"] + "','" + dtTbl.Rows[i]["level_code"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Exam_ID"] + "','" + dtTbl.Rows[i]["applicationId"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["detailId"] + "','" + dtTbl.Rows[i]["Module_ID"] + "','" + dtTbl.Rows[i]["Candidate_Corr_Address_City"].ToString().Replace("'", "''") + "','"
                                + dtTbl.Rows[i]["Candidate_Corr_Address_State"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Candidate_Perm_Address_City"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Candidate_Perm_Address_State"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["PercentageOfDisability"] + "','" + dtTbl.Rows[i]["aadhaarNo"] + "' )";


                        command = new OleDbCommand(sqlStr, connection);
                          command.ExecuteNonQuery();
                      }
                  else
                  {
                      ShowAlert("Data must be download after Last Date of Application!!", true);
                  }

                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.ContentType = "application/octet-stream";
                    Response.Charset = "UTF-8";
                    Response.WriteFile(mdbFilePath);

                //filename = regCentreCode + "_" + courseCode + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";
                //}
                //---------------------------for course exam  start
                //else
                //{
                //    if ((ddlRc.SelectedValue != "0"))
                //    {
                //        filename = ddlRc.SelectedItem + "_" + courseCode + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";

                //    }
                //    else
                //    {
                //        filename = "NIELIT_Regional_ALL_" + "_" + courseCode + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";
                //    }
                //    mdbFilePath = Server.MapPath("~/Download/" + filename);
                //    if (System.IO.File.Exists(mdbFilePath))
                //        System.IO.File.Delete(mdbFilePath);

                //    System.IO.File.Copy(Server.MapPath("~/Download/Database_format_BCC.accdb"), mdbFilePath);
                //    string connect1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                //    connection.ConnectionString = connect1;
                //    connection.Open();
                //    command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[Exam_Database]", connection);
                //    command.ExecuteNonQuery();
                //    String sqlStr = "";
                //    String sql = "";

                //    if (context.Exams.Find(examId).IsBatchProcessable)
                //    {

                //        sql = " SELECT DISTINCT C.ID AS APPL_ID ,C.DATE AS APPL_DATE ,C.NUMBER AS APPL_NUMBER ,'NA' AS FOLDER_NO,BAT.BATCH_NO AS BATCH_NO ,C.ID AS SR_NO," +
                //                " UPPER(CYCLE.NAME) AS EXAM_CYCLE_NAME,UPPER(DATENAME(M, STR(EXM.EXAM_MONTH) + '/1/2011')) AS EXAM_MONTH ,EXM.EXAM_YEAR AS EXAM_YEAR," +
                //                " UPPER(C.NAME) AS NAME , UPPER(C.FATHER_NAME) AS FNAME , UPPER(C.MOTHER_NAME) AS MNAME ,   UPPER(C.GUARDIAN_NAME) AS GNAME , REPLACE(CONVERT(VARCHAR,C.DOB,106),' ','-') AS DOB," +
                //                " CASE C.GENDER WHEN 'MALE' THEN 'M' ELSE 'F' END AS GENDER, EDU.CODE   AS H_Q, UPPER(C.COR_ADDRESS1) AS ADDRESS_LINE1 ,UPPER(C.COR_ADDRESS2) AS ADDRESS_LINE2 ,UPPER(C.COR_ADDRESS3) AS ADDRESS_LINE3 , UPPER(C.COR_CITY_NAME) AS CITY ,UPPER(LOC1.NAME)  AS STATE," +
                //                " UPPER(LOC2.NAME) AS DISTRICT,  C.COR_PIN_CODE AS PINCODE ,C.MOBILE AS MOBILE_NO , C.EMAIL AS EMAIL ,CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE  ACC.ACCREDITATION_NUMBER END AS CCC_NO, CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE  UPPER(INS.NAME) END AS INSTITUTE_NAME ," +
                //                " CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE UPPER(INS.ADDRESS1) + ', ' + ISNULL(UPPER(INS.ADDRESS2),'') + ' ' +ISNULL(UPPER(INS.ADDRESS3),'')+ ' '+ UPPER(INS.CITY_NAME) +',' + UPPER(LOC3.NAME) END AS INSTITUTE_ADDR ," +
                //                " CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'OTH' ELSE 'AIC' END AS INS_STAT ,CENTER1.CODE AS EXAM_CENTER1 ,CENTER2.CODE AS EXAM_CENTER2 ,OCC.CODE AS OCCUPATION,CATG.CODE AS CATEGORY," +
                //                " CASE C.ALREADY_APPLIED WHEN 'TRUE' THEN 'Y' ELSE 'N'  END  AS ALREADY_APPLIED , " +
                //                " UPPER(PREVEXMCYCLE.NAME) AS PREV_EXAM_CYCLE ,PREVEXM.EXAM_MONTH AS PREV_EXM_MONTH ,PREVEXM.EXAM_YEAR AS PREV_EXM_YEAR, C.PREVIOUS_ROLL_NUMBER AS PREV_ROLL_NUMBER, " +
                //                " C.ROLL_NUMBER AS ROLL_NUMBER,C.EXAM_CENTRE_NAME AS EXAM_CENTRE_NAME ,C.EXAM_BATCH_NUMBER AS EXAM_BATCH_NUMBER ," +
                //                " C.EXAM_CENTRE_ADDRESS AS EXAM_CENTRE_ADDRESS,C.REPORTING_TIME AS REPORTING_TIME ,C.DATE_OF_EXAM  AS DATE_OF_EXAM ,RES.CODE  AS RESULT, " +
                //                " C.IS_DISABILITY AS DISABILITY, DS.NAME AS DISABILITYTYPE, C.DISABILITY_PERCENTAGE AS DISABILITYPERCENT " +
                //                " FROM CERTIFICATE_EXAM_APPLICATION C LEFT OUTER JOIN EXAM PREVEXM ON C.PREVIOUS_EXAM_ID = PREVEXM.ID" +
                //                " LEFT OUTER JOIN DISABILITY_TYPE DS ON C.DISABILITY_TYPE_ID = DS.ID " +
                //                " LEFT OUTER JOIN   EXAM_CYCLE PREVEXMCYCLE ON PREVEXM.EXAMN_CYCLE_ID = PREVEXMCYCLE.ID LEFT OUTER JOIN INSTITUTE INS ON C.INSTITUTE_ID = INS.ID" +
                //                " LEFT OUTER JOIN INTITUTE_ACCREDITATION_DETAIL ACC ON C.COURSE_ID = ACC.COURSE_ID AND  C.INSTITUTE_ID = ACC.INSTITUTE_ID" +
                //                " LEFT OUTER JOIN  LOCATION LOC3  ON INS.STATE_ID = LOC3.ID LEFT OUTER JOIN  RESULT_GRADING RES ON C.RESULT_GRADE_ID = RES.ID ,  " +
                //                " BATCH_ITEM B,BATCH BAT, EXAM EXM , EXAM_CYCLE CYCLE,EDUCATIONAL_QUALIFICATION EDU," +
                //                " LOCATION LOC2 ,  EXAM_CENTER CENTER1 ,EXAM_CENTER CENTER2," +
                //                " OCCUPATION OCC ,CAST_CATEGORY CATG,LOCATION LOC1 " +
                //                " WHERE C.BATCH_ITEM_ID = B.ID  AND  B.BATCH_ID = BAT.ID AND C.EXAM_ID = EXM.ID AND EXM.EXAMN_CYCLE_ID = CYCLE.ID " +
                //                " AND C.EDUCATIONAL_QUALIFICATION_ID =EDU.ID  AND  C.COR_DISTRICT_ID =LOC2.ID" +
                //                " AND C.EXAM_ID =" + examId + " AND C.REGIONAL_CENTER_ID = " + regCentreId +
                //                " AND C.APPLICATION_STATUS_ID =" + applicationStatus + " AND C.EXAM_CENTER1_ID = CENTER1.ID AND C.EXAM_CENTER2_ID = CENTER2.ID " +
                //                " AND C.OCCUPATION_ID = OCC.ID AND C.CAST_CATEGORY_ID =CATG.ID  AND C.COR_STATE_ID =LOC1.ID " +
                //                " AND C.DEMAND_NOTE_ID IS NOT NULL AND EXISTS ( SELECT ID FROM DEMAND_NOTE WHERE ID = DEMAND_NOTE_ID AND STATUS_ID IN ( 2 ,4) AND (ONLINE_TRANSACTION_ID IS NOT NULL OR NEFT_TRANSACTION_ID IS NOT NULL OR CSC_TRANSACTION_ID IS NOT NULL))";
                //    }
                //    else
                //    {


                //        if (ddlRc.SelectedValue != "0")
                //        {

                //            string sqlUpdateStatus = "UPDATE Certificate_Exam_Application SET Application_Status_ID=" + applicationStatus + " WHERE Course_ID=" + CourseId + " and Application_Status_ID in(" + applInstituteStatusID + "," + applDirectStatusID + ") and Exam_ID=" + examId + " and Regional_Center_ID=" + regCentreId + " and Final_Submitted=1 and Payment_Status_ID !=" + paymentPendingStatusID;
                //            context.Database.ExecuteSqlCommand(sqlUpdateStatus);
                //            context.SaveChanges();

                //            sql = " SELECT DISTINCT C.ID AS APPL_ID ,C.DATE AS APPL_DATE ,C.NUMBER AS APPL_NUMBER ,'NA' AS FOLDER_NO,'NA' AS BATCH_NO ,C.ID AS SR_NO," +
                //                  " UPPER(CYCLE.NAME) AS EXAM_CYCLE_NAME,UPPER(DATENAME(M, STR(EXM.EXAM_MONTH) + '/1/2011')) AS EXAM_MONTH ,EXM.EXAM_YEAR AS EXAM_YEAR," +
                //                  "  UPPER(C.NAME) AS NAME , UPPER(C.FATHER_NAME) AS FNAME , UPPER(C.MOTHER_NAME) AS MNAME ,   UPPER(C.GUARDIAN_NAME) AS GNAME , REPLACE(CONVERT(VARCHAR,C.DOB,106),' ','-') AS DOB," +
                //                  " CASE C.GENDER WHEN 'MALE' THEN 'M'  WHEN 'TRANS'  THEN 'T' ELSE 'F' END AS GENDER, EDU.CODE   AS H_Q, UPPER(C.COR_ADDRESS1) AS ADDRESS_LINE1 ,UPPER(C.COR_ADDRESS2) AS ADDRESS_LINE2 ,UPPER(C.COR_ADDRESS3) AS ADDRESS_LINE3 , UPPER(C.COR_CITY_NAME) AS CITY ,UPPER(LOC1.NAME)  AS STATE," +
                //                  " UPPER(LOC2.NAME) AS DISTRICT,  C.COR_PIN_CODE AS PINCODE ,C.MOBILE AS MOBILE_NO , C.EMAIL AS EMAIL ,CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE  ACC.ACCREDITATION_NUMBER END AS CCC_NO, CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE  UPPER(INS.NAME) END AS INSTITUTE_NAME ," +
                //                  " CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE UPPER(INS.ADDRESS1) + ', ' + ISNULL(UPPER(INS.ADDRESS2),'') + ' ' +ISNULL(UPPER(INS.ADDRESS3),'')+ ' '+ UPPER(INS.CITY_NAME) +',' + UPPER(LOC3.NAME) END AS INSTITUTE_ADDR ," +
                //                  " CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'OTH' ELSE 'AIC' END AS INS_STAT ,CENTER1.CODE AS EXAM_CENTER1 ,CENTER2.CODE AS EXAM_CENTER2 ,OCC.CODE AS OCCUPATION,CATG.CODE AS CATEGORY," +
                //                  " CASE C.ALREADY_APPLIED WHEN 'TRUE' THEN 'Y' ELSE 'N'  END  AS ALREADY_APPLIED , " +
                //                  " UPPER(PREVEXMCYCLE.NAME) AS PREV_EXAM_CYCLE ,PREVEXM.EXAM_MONTH AS PREV_EXM_MONTH ,PREVEXM.EXAM_YEAR AS PREV_EXM_YEAR, C.PREVIOUS_ROLL_NUMBER AS PREV_ROLL_NUMBER, " +
                //                  " C.ROLL_NUMBER AS ROLL_NUMBER,C.EXAM_CENTRE_NAME AS EXAM_CENTRE_NAME ,C.EXAM_BATCH_NUMBER AS EXAM_BATCH_NUMBER ," +
                //                  " C.EXAM_CENTRE_ADDRESS AS EXAM_CENTRE_ADDRESS,C.REPORTING_TIME AS REPORTING_TIME ,C.DATE_OF_EXAM  AS DATE_OF_EXAM ,RES.CODE  AS RESULT, " +
                //                  " C.IS_DISABILITY AS DISABILITY, DS.NAME AS DISABILITYTYPE, C.DISABILITY_PERCENTAGE AS DISABILITYPERCENT " +
                //                  " FROM CERTIFICATE_EXAM_APPLICATION C LEFT OUTER JOIN EXAM PREVEXM ON C.PREVIOUS_EXAM_ID = PREVEXM.ID " +
                //                  " LEFT OUTER JOIN DISABILITY_TYPE DS ON C.DISABILITY_TYPE_ID = DS.ID " +
                //                  " LEFT OUTER JOIN   EXAM_CYCLE PREVEXMCYCLE ON PREVEXM.EXAMN_CYCLE_ID = PREVEXMCYCLE.ID LEFT OUTER JOIN INSTITUTE INS ON C.INSTITUTE_ID = INS.ID" +
                //                  " LEFT OUTER JOIN INTITUTE_ACCREDITATION_DETAIL ACC ON C.COURSE_ID = ACC.COURSE_ID AND  C.INSTITUTE_ID = ACC.INSTITUTE_ID" +
                //                  " LEFT OUTER JOIN  LOCATION LOC3  ON INS.STATE_ID = LOC3.ID LEFT OUTER JOIN  RESULT_GRADING RES ON C.RESULT_GRADE_ID = RES.ID ,  " +
                //                  " EXAM EXM , EXAM_CYCLE CYCLE,EDUCATIONAL_QUALIFICATION EDU," +
                //                  " LOCATION LOC2 ,  EXAM_CENTER CENTER1 ,EXAM_CENTER CENTER2," +
                //                  " OCCUPATION OCC ,CAST_CATEGORY CATG,LOCATION LOC1 " +
                //                  " WHERE C.EXAM_ID = EXM.ID AND EXM.EXAMN_CYCLE_ID = CYCLE.ID " +
                //                  " AND C.EDUCATIONAL_QUALIFICATION_ID =EDU.ID  AND  C.COR_DISTRICT_ID =LOC2.ID" +
                //                  " AND c.Exam_ID =" + examId + " AND C.REGIONAL_CENTER_ID = " + regCentreId +
                //                  " AND C.APPLICATION_STATUS_ID =" + applicationStatus + " AND C.EXAM_CENTER1_ID = CENTER1.ID AND C.EXAM_CENTER2_ID = CENTER2.ID " +
                //                  " AND C.OCCUPATION_ID = OCC.ID AND C.CAST_CATEGORY_ID =CATG.ID  AND C.COR_STATE_ID =LOC1.ID  " +
                //                  " AND C.DEMAND_NOTE_ID IS NOT NULL AND EXISTS ( SELECT ID FROM DEMAND_NOTE WHERE ID = DEMAND_NOTE_ID AND STATUS_ID IN ( 2 ,4) AND (ONLINE_TRANSACTION_ID IS NOT NULL OR NEFT_TRANSACTION_ID IS NOT NULL OR CSC_TRANSACTION_ID IS NOT NULL))";
                //        }



                //        else
                //        {

                //            string sqlUpdateStatus = "UPDATE Certificate_Exam_Application SET Application_Status_ID=" + applicationStatus + " WHERE Course_ID=" + CourseId + " and Application_Status_ID in(" + applInstituteStatusID + "," + applDirectStatusID + ") and Exam_ID=" + examId + "  and Final_Submitted=1 and Payment_Status_ID !=" + paymentPendingStatusID;
                //            context.Database.ExecuteSqlCommand(sqlUpdateStatus);
                //            context.SaveChanges();

                //            sql = " SELECT DISTINCT C.ID AS APPL_ID ,C.DATE AS APPL_DATE ,C.NUMBER AS APPL_NUMBER ,'NA' AS FOLDER_NO,'NA' AS BATCH_NO ,C.ID AS SR_NO," +
                //                  " UPPER(CYCLE.NAME) AS EXAM_CYCLE_NAME,UPPER(DATENAME(M, STR(EXM.EXAM_MONTH) + '/1/2011')) AS EXAM_MONTH ,EXM.EXAM_YEAR AS EXAM_YEAR," +
                //                  "  UPPER(C.NAME) AS NAME , UPPER(C.FATHER_NAME) AS FNAME , UPPER(C.MOTHER_NAME) AS MNAME ,   UPPER(C.GUARDIAN_NAME) AS GNAME , REPLACE(CONVERT(VARCHAR,C.DOB,106),' ','-') AS DOB," +
                //                  " CASE C.GENDER WHEN 'MALE' THEN 'M'  WHEN 'TRANS'  THEN 'T' ELSE 'F' END AS GENDER, EDU.CODE   AS H_Q, UPPER(C.COR_ADDRESS1) AS ADDRESS_LINE1 ,UPPER(C.COR_ADDRESS2) AS ADDRESS_LINE2 ,UPPER(C.COR_ADDRESS3) AS ADDRESS_LINE3 , UPPER(C.COR_CITY_NAME) AS CITY ,UPPER(LOC1.NAME)  AS STATE," +
                //                  " UPPER(LOC2.NAME) AS DISTRICT,  C.COR_PIN_CODE AS PINCODE ,C.MOBILE AS MOBILE_NO , C.EMAIL AS EMAIL ,CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE  ACC.ACCREDITATION_NUMBER END AS CCC_NO, CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE  UPPER(INS.NAME) END AS INSTITUTE_NAME ," +
                //                  " CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'DIRECT' ELSE UPPER(INS.ADDRESS1) + ', ' + ISNULL(UPPER(INS.ADDRESS2),'') + ' ' +ISNULL(UPPER(INS.ADDRESS3),'')+ ' '+ UPPER(INS.CITY_NAME) +',' + UPPER(LOC3.NAME) END AS INSTITUTE_ADDR ," +
                //                  " CASE WHEN C.APPLICANT_TYPE_ID = 1 THEN 'OTH' ELSE 'AIC' END AS INS_STAT ,CENTER1.CODE AS EXAM_CENTER1 ,CENTER2.CODE AS EXAM_CENTER2 ,OCC.CODE AS OCCUPATION,CATG.CODE AS CATEGORY," +
                //                  " CASE C.ALREADY_APPLIED WHEN 'TRUE' THEN 'Y' ELSE 'N'  END  AS ALREADY_APPLIED , " +
                //                  " UPPER(PREVEXMCYCLE.NAME) AS PREV_EXAM_CYCLE ,PREVEXM.EXAM_MONTH AS PREV_EXM_MONTH ,PREVEXM.EXAM_YEAR AS PREV_EXM_YEAR, C.PREVIOUS_ROLL_NUMBER AS PREV_ROLL_NUMBER, " +
                //                  " C.ROLL_NUMBER AS ROLL_NUMBER,C.EXAM_CENTRE_NAME AS EXAM_CENTRE_NAME ,C.EXAM_BATCH_NUMBER AS EXAM_BATCH_NUMBER ," +
                //                  " C.EXAM_CENTRE_ADDRESS AS EXAM_CENTRE_ADDRESS,C.REPORTING_TIME AS REPORTING_TIME ,C.DATE_OF_EXAM  AS DATE_OF_EXAM ,RES.CODE  AS RESULT, " +
                //                  " C.IS_DISABILITY AS DISABILITY, DS.NAME AS DISABILITYTYPE, C.DISABILITY_PERCENTAGE AS DISABILITYPERCENT " +
                //                  " FROM CERTIFICATE_EXAM_APPLICATION C LEFT OUTER JOIN EXAM PREVEXM ON C.PREVIOUS_EXAM_ID = PREVEXM.ID " +
                //                  " LEFT OUTER JOIN DISABILITY_TYPE DS ON C.DISABILITY_TYPE_ID = DS.ID " +
                //                  " LEFT OUTER JOIN   EXAM_CYCLE PREVEXMCYCLE ON PREVEXM.EXAMN_CYCLE_ID = PREVEXMCYCLE.ID LEFT OUTER JOIN INSTITUTE INS ON C.INSTITUTE_ID = INS.ID" +
                //                  " LEFT OUTER JOIN INTITUTE_ACCREDITATION_DETAIL ACC ON C.COURSE_ID = ACC.COURSE_ID AND  C.INSTITUTE_ID = ACC.INSTITUTE_ID" +
                //                  " LEFT OUTER JOIN  LOCATION LOC3  ON INS.STATE_ID = LOC3.ID LEFT OUTER JOIN  RESULT_GRADING RES ON C.RESULT_GRADE_ID = RES.ID ,  " +
                //                  " EXAM EXM , EXAM_CYCLE CYCLE,EDUCATIONAL_QUALIFICATION EDU," +
                //                  " LOCATION LOC2 ,  EXAM_CENTER CENTER1 ,EXAM_CENTER CENTER2," +
                //                  " OCCUPATION OCC ,CAST_CATEGORY CATG,LOCATION LOC1 " +
                //                  " WHERE C.EXAM_ID = EXM.ID AND EXM.EXAMN_CYCLE_ID = CYCLE.ID " +
                //                  " AND C.EDUCATIONAL_QUALIFICATION_ID =EDU.ID  AND  C.COR_DISTRICT_ID =LOC2.ID" +
                //                  " AND c.Exam_ID =" + examId +
                //                  " AND C.APPLICATION_STATUS_ID =" + applicationStatus + " AND C.EXAM_CENTER1_ID = CENTER1.ID AND C.EXAM_CENTER2_ID = CENTER2.ID " +
                //                  " AND C.OCCUPATION_ID = OCC.ID AND C.CAST_CATEGORY_ID =CATG.ID  AND C.COR_STATE_ID =LOC1.ID  " +
                //                  " AND C.DEMAND_NOTE_ID IS NOT NULL AND EXISTS ( SELECT ID FROM DEMAND_NOTE WHERE ID = DEMAND_NOTE_ID AND STATUS_ID IN ( 2 ,4) AND (ONLINE_TRANSACTION_ID IS NOT NULL OR NEFT_TRANSACTION_ID IS NOT NULL OR CSC_TRANSACTION_ID IS NOT NULL))";
                //        }

                //    } //----------------------Edit by chhavi end
                //    dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                //    if (dtTbl.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dtTbl.Rows.Count; i++)
                //        {
                //            sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[Exam_Database] (Application_ID, Application_Date, Application_Number, " +
                //                            " Folder_no, batch_Number, sr_no, exam_cycle_name,exam_month,exam_year, NAME, F_NAME, M_NAME, G_Name, D_O_B, SEX, H_Qual, ADD1 ,ADD2,  ADD3," +
                //                            " CITY, STATE, District, PINCODE, PH_NO_C, EMAIL_C, CCC_NO, INST_NAME, INST_ADD, INST_STAT, TH_CENT_CH," +
                //                            " SEC_TH_CEN,  OCCUPATION,  CATEGORY, Disability, DisabilityType, DisabilityPercent, PREV_APP,prev_exam_cycle_name,  PREV_M,  PREV_YEAR,  PREV_ROLL, " +
                //                            " Rollno,cent_allot,cent_add,batch,rep_time,Result ";
                //            if (!String.IsNullOrEmpty(dtTbl.Rows[i]["Date_of_Exam"].ToString()))
                //                sqlStr += ",examDate ";
                //            sqlStr += "      ) " +
                //                            " values(val('" + dtTbl.Rows[i]["appl_id"] + "')," +
                //                            " #" + dtTbl.Rows[i]["appl_date"] + "#, '" + dtTbl.Rows[i]["appl_number"] + "','NA','" + dtTbl.Rows[i]["batch_no"].ToString() + "', val('" + dtTbl.Rows[i]["sr_no"] + "'),'" + dtTbl.Rows[i]["exam_cycle_name"] + "','"
                //                            + dtTbl.Rows[i]["exam_month"].ToString() + "','" + dtTbl.Rows[i]["exam_year"].ToString() + "', '" + dtTbl.Rows[i]["name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["fname"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["mname"].ToString().Replace("'", "''") + "','"
                //                            + dtTbl.Rows[i]["gname"].ToString().Replace("'", "''") + "', #" + dtTbl.Rows[i]["DOB"].ToString() + "#, '" + dtTbl.Rows[i]["gender"] + "', '" + dtTbl.Rows[i]["H_Q"] + "', '" + dtTbl.Rows[i]["address_line1"].ToString().Replace("'", "''")
                //                            + "', '" + dtTbl.Rows[i]["address_line2"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["address_line3"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["city"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["state"]
                //                            + "', '" + dtTbl.Rows[i]["district"] + "', val('" + dtTbl.Rows[i]["pincode"] + "'), '" + dtTbl.Rows[i]["mobile_no"].ToString() + "', '"
                //                            + dtTbl.Rows[i]["email"] + "','" + dtTbl.Rows[i]["ccc_no"] + "','" + dtTbl.Rows[i]["institute_name"].ToString().Replace("'", "''") + "','"
                //                            + dtTbl.Rows[i]["institute_addr"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["ins_stat"] + "', '" + dtTbl.Rows[i]["exam_center1"].ToString().Trim() + "','" + dtTbl.Rows[i]["exam_center2"].ToString().Trim()
                //                            + "', '" + dtTbl.Rows[i]["occupation"] + "', '" + dtTbl.Rows[i]["category"] + "', '" + dtTbl.Rows[i]["Disability"] + "', '" + dtTbl.Rows[i]["DisabilityType"] + "', '" + dtTbl.Rows[i]["DisabilityPercent"] + "', '" + dtTbl.Rows[i]["already_applied"]
                //                            + "', '" + dtTbl.Rows[i]["prev_exam_cycle"] + "','" + dtTbl.Rows[i]["prev_exm_month"] + "', '" +
                //                            dtTbl.Rows[i]["prev_exm_year"] + "','" + dtTbl.Rows[i]["prev_roll_number"] + "','" + dtTbl.Rows[i]["Roll_Number"] + "',' " +
                //                            dtTbl.Rows[i]["Exam_Centre_Name"].ToString().Replace("'", "''").Trim() + "','" + dtTbl.Rows[i]["Exam_Centre_Address"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Exam_Batch_Number"].ToString().Replace("'", "''") + "','" +
                //                            dtTbl.Rows[i]["Reporting_Time"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Result"].ToString().Replace("'", "''") + "'";
                //            if (!String.IsNullOrEmpty(dtTbl.Rows[i]["Date_of_Exam"].ToString()))
                //                sqlStr += ",#" + dtTbl.Rows[i]["Date_of_Exam"] + "# ";
                //            sqlStr += ")";

                //            command = new OleDbCommand(sqlStr, connection);
                //            command.ExecuteNonQuery();
                //        }
                  

            //    }


            //    command.Dispose();
            //    connection.Close();
            //    connection.Dispose();
            //    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //    Response.ContentType = "application/octet-stream";
            //    Response.Charset = "UTF-8";
            //    Response.WriteFile(mdbFilePath);
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
        finally { context.Dispose(); }
    }






    DataTable bindcandidaterecords_for_CHMO()
    {
        DataSet ds = new DataSet();
        string course_name = ddlCourseName.SelectedItem.Text.Trim();
        Int32 courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
        int course_type = Convert.ToInt32(ddl_course_type.SelectedValue.Trim());
        Int32 examId = Convert.ToInt32(ddlExamName.SelectedValue);
        Int32 PExam_id = Convert.ToInt32(ddlExamName.SelectedValue);

        SqlCommand scCommand = new SqlCommand("Course_Exam_Data_Download", new SqlConnection(con.ConnectionString));
        scCommand.CommandType = CommandType.StoredProcedure;
        scCommand.Parameters.Add("@PExam_id", SqlDbType.Int).Value = PExam_id;
        scCommand.Parameters.Add("@PCourse_id", SqlDbType.Int).Value = courseId;
        scCommand.Parameters.Add("@Option", SqlDbType.VarChar).Value = course_type;
        scCommand.Parameters.Add("@PFLAG_download_Completed", SqlDbType.Int).Value = 0;
        scCommand.Parameters.Add("@PFLAG_Roll_no_Gen_Completed", SqlDbType.Int).Value = 0;   
        scCommand.CommandTimeout = 500000;
        if (scCommand.Connection.State == ConnectionState.Closed)
        {
            scCommand.Connection.Open();
        }
        da = new SqlDataAdapter(scCommand);
        da.Fill(ds);
        scCommand.Connection.Close();
        
        return ds.Tables[0];
      
    }





    protected void btnPendingCount_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 regionalCentreID = Convert.ToInt32(ddlRc.SelectedValue);

            //Response.Redirect("DownloadApplicationsPendingCount_Course.aspx?courseID=" + courseId + "&examID=" + examID + "&regionalCentreID=" + regionalCentreID);
            Response.Redirect("DownloadApplicationsPendingCount_Course.aspx?courseID=" + courseId + "&examID=" + examID);

        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
}