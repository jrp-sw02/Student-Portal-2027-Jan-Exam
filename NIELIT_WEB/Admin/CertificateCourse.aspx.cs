using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;

public partial class Admin_CertificateCourse : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
               //Response.Write("Sorry! You don't have rights  to view this page");
               //Response.End();
            }
            //Response.Write(UserManager.HasRight(currentRoleId, enmRight.Delete).ToString());
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    FillCourseCategory();
                    FillCourseType();
					FillSectorID();
                    FillApplicantType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCourseCategory();
                    FillApplicantType();
                    FillCourseType();
					FillSectorID();
                    FillFilter();
                    FillFilterCourseType();
                    BindGridView();

                    //if (!string.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    //{
                    //    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "Admin/CertificateCourse.aspx?CourseId=" + Request.QueryString["CourseId"], ""));
                    //}
                    //else
                    //{
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "Admin/CertificateCourse.aspx", ""));
                    //}
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
    protected void FillFilter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourses, CourseList, lst);
            };
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
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, CourseList, lst);
            };
        }
        catch (Exception ex) 
        {
            throw ex;
        }
    }
    protected void FillApplicantType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("Both", "0");
                var Applicantype = from p in context.ApplicantTypes
                                   orderby (p.Name)
                                   select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlapplicanttype, Applicantype, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourseType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.Course.CourseTypeID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlctype, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCourseType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.CourseTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.Course.CourseTypeID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseType, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Filllanguage()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var language = from p in context.Languages
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddllanguage, language, lst);
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
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Course Details";
            tblNavLinks.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseId = Convert.ToInt32(Request.QueryString["CourseId"]);
                Course editCourse = context.Courses.Find(courseId);
                filleditpreviouscourses(editCourse.CourseCategoryID, editCourse.ID);
                var course = (from p in context.Courses join c in context.CourseRevisions
                              on p.ID equals c.CourseID
                             where p.ID == courseId 
                             orderby c.EffectiveFromDate descending
                             select new
                             {
                                 ID = p.ID,
								 sector=p.NielitSectorID,
                                 trgSplecialization=p.NIELITrgSplnID,
								 Name = p.Name,
                                 Code = p.Code,
                                 CourseCategoryID = p.CourseCategoryID,
                                 CourseTypeID = p.CourseTypeID,
                                 displayorder= p.DisplayOrder,
                                 CurrentRevisionNumber = c.RevisionNumber,
                                 EffectiveFromDate = c.EffectiveFromDate,
                                 showOnWeb = p.ShowOnWeb,
								 isfuture=p.IsFuture,
                                 lowerCourseID = p.LowerCourseID,
                                 appltypeID= p.ApplicantTypeID.HasValue ? p.ApplicantTypeID : 0,
                                 examserviceID = p.ExaminationServiceID,
                                 regserviceid = p.RegistrationServiceID 
                             }).FirstOrDefault();

				ddlNielitSectorID.SelectedValue = course.sector.ToString ();
                FillNielitTrgSpecialization(Convert.ToInt32(ddlNielitSectorID.SelectedValue));
                ddlNielitTrgSpecialization.SelectedValue = course.trgSplecialization.ToString ();
                txtdisplay.Text = course.displayorder.ToString();
                txtdisplay.Enabled = false;
                Txtrevisionno.Text = course.CurrentRevisionNumber.ToString();
                Txtrevisionno.Enabled = false;
                txtcoursename.Text = course.Name;
                txtcoursecode.Text = course.Code;
                txtexamserviceID.Text = course.examserviceID;
                txtregserviceID.Text =  course.regserviceid;
                ddlcoursecategory.SelectedValue = course.CourseCategoryID.ToString();
                ddlcoursecategory.Enabled = false;
                ddlctype.SelectedValue = course.CourseTypeID.ToString();
                ddlctype.Enabled = false;
                txteffectivedate.Text = course.EffectiveFromDate.ToString("dd-MMM-yyyy");
                //txteffectivedate.Enabled = false;
                //txteffectivedate_CalendarExtender.Enabled = false;
                ddlapplicanttype.SelectedValue = course.appltypeID.ToString();
                //ddlapplicanttype.Enabled = false;
                if (course.showOnWeb == true)
                {
                   ddlShowOnWeb.SelectedValue = "1";
                }
                else
                {
                    ddlShowOnWeb.SelectedValue = "2";
                }
				 if (course.isfuture  == true)
                {
                    ddlWhetherFuture.SelectedValue = "1";
                }
                else
                {
                    ddlWhetherFuture.SelectedValue = "2";
                }
                //ddlShowOnWeb.Enabled = false;
                if (course.lowerCourseID.HasValue)
                {
                    ddlcourse.SelectedValue = course.lowerCourseID.Value.ToString();
                }
                else
                {
                    //ddlcourse.Items.Clear();
                    //ddlcourse.Items.Insert(0, "NA");
                    ddlcourse.SelectedValue = "0";
                }
                //ddlcourse.Enabled = false;
                var reregistration = (from c in context.CourseRegistrationPolicies
                                      where c.CourseID == courseId
                                      orderby c.EffectiveFromDate descending
                                      select c).FirstOrDefault();
             
                if (editCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    if (reregistration != null)
                    {

                        Filllanguage();
                        ddllanguage.Items.Insert(3, "Both");
                        tbreregistration.Visible = true;
                        Lbprevcoursename.Visible = true;
                        tbreregistration.Visible = true;
                        txtregperiod.Text = reregistration.RegistrationValidity.ToString();
                        if (reregistration.AllowedLanguage.HasValue)
                        {

                            ddllanguage.SelectedValue = Convert.ToInt32(reregistration.AllowedLanguage.Value).ToString();
                        }
                        else
                        {
                            ddllanguage.SelectedIndex = 3;
                        }
                        if (reregistration.ReRegistrationChance == true)
                        {
                            ddlregchances.SelectedValue = "1";
                        }
                        else
                        {
                            ddlregchances.SelectedValue = "2";
                        }
                        if (reregistration.ReRegistrationValidity.HasValue)
                        {
                            txtvalidity.Text = reregistration.ReRegistrationValidity.Value.ToString();
                        }
                        else
                        {
                            txtvalidity.Text = "NA";
                        }
                        if (reregistration.ReRegistrationGapInMonths.HasValue)
                        {
                            txtreggape.Text = reregistration.ReRegistrationGapInMonths.Value.ToString();
                        }
                        else
                        {
                            txtreggape.Text = "NA";
                        }

                        Txtregpolicyeffectivedate.Text = reregistration.EffectiveFromDate.ToString("dd-MMM-yyyy");
                        //ddlregchances.Enabled = false;
                        //txtregperiod.Enabled = false;
                        //txtvalidity.Enabled = false;
                        //txtreggape.Enabled = false;
                        //Txtregpolicyeffectivedate.Enabled = false;
                        //Txtregpolicyeffectivedate_CalendarExtender.Enabled = false;
                        //ddllanguage.Enabled = false;
                    }
                }
                else
                {
                    ddlcourse.Visible = false;
                    Lbprevcoursename.Visible = false;
                    tbreregistration.Visible = false;
                    txtregserviceID.Visible = false;
                    lblreg.Visible = false;
                }
                hfAccID.Value = Request.QueryString["CourseId"];
                hfName.Value = Request.QueryString["Name"];
				hlCourseLevelDuration.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCourseLevelDuration.aspx?CourseId=" + Request.QueryString["CourseId"]);
                hlmodule.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("admfrmodule.aspx?CourseId=" + Request.QueryString["CourseId"]);
                h1cexam.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentres.aspx?CourseId=" + Request.QueryString["CourseId"] + "&CategoryID=" + course.CourseCategoryID);
                hlModuleParity.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("ModuleParity.aspx?CourseId=" + Request.QueryString["CourseId"] + "&CategoryID=" + course.CourseCategoryID);
                hlModuleExemption.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("ModuleExemption.aspx?CourseId=" + Request.QueryString["CourseId"] + "&RevisionNo=" + course.CurrentRevisionNumber);
                h2Exams.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CourseId=" + Request.QueryString["CourseId"] + "&CatID=" + course.CourseCategoryID);
                hlDownload.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("DownLoad.aspx?CourseId=" + Request.QueryString["CourseId"] + "&CategoryID=" + course.CourseCategoryID);
                hlQualification.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?CourseId=" + Request.QueryString["CourseId"]);
                h1FeeDetail.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("FeeDetail.aspx?CourseId=" + Request.QueryString["CourseId"]);
                hlrevision.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("Revisionlist.aspx?CourseId=" + Request.QueryString["CourseId"]);
                hlregpolicy.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["CourseId"]);
                hlTimeTablePattern.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("TimeTablePattern.aspx?CourseID=" + Request.QueryString["CourseId"]);
                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(course.Name, "Admin/CertificateCourse.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
           // context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            int courseType = 0;
            int courseCategory = 0;
            int courseName = 0;
            if (ddlCourses.SelectedValue != "0")
                courseCategory = Convert.ToInt32(ddlCourses.SelectedValue);
            if (ddlCourseName.SelectedValue != "0")
                courseName = Convert.ToInt32(ddlCourseName.SelectedValue);
            if (ddlCourseType.SelectedValue != "0")
                courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var courses = (from s in context.Courses 
                          select new 
                          { 
                            ID = s.ID,
                            Name = s.Name,
                            Code = s.Code,
                            CategoryName = s.CourseCategory.Code, 
                            CourseCategoryID = s.CourseCategoryID ,
                            courseType=s.CourseType.Name,
                            courseTypeID=s.CourseTypeID,
                            revno = s.CourseRevisions.Max(p=>p.RevisionNumber)
                          });
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            if (courseCategory != 0)
                courses = courses.Where(s => s.CourseCategoryID == courseCategory);
            if (courseName != 0)
                courses = courses.Where(s => s.ID == courseName);
            if (courseType != 0)
                courses = courses.Where(s => s.courseTypeID == courseType);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Name":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.Name);
                        else
                            courses = courses.OrderBy(s => s.Name);
                        break;
                    case "CategoryName":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.CategoryName);
                        else
                            courses = courses.OrderBy(s => s.CategoryName);
                        break;
                    case "Code":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.Code);
                        else
                            courses = courses.OrderBy(s => s.Code);
                        break;
                    case "courseType":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.courseType);
                        else
                            courses = courses.OrderBy(s => s.courseType);
                        break;
                    case "revno":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.revno);
                        else
                            courses = courses.OrderBy(s => s.revno);
                        break;
                    default:
                        courses = courses.OrderBy(s => s.Name);
                        break;
                }
            }

            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                courses = courses.Where(a => roleCourses.Contains(a.ID));
            }
           
            PagingBar1.Bind(courses, ref gvMain);        
            uPnlGrid.Update();
            uPnlNavigation.Update();
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
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
            FillCourseCategory();
            FillCourseType();
		    FillSectorID();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Course";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Course", "#", ""));
        }
        else
        {
            Response.Redirect("CertificateCourse.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        try
        {
            context = new EConnectContext();
            //create and object 
            Course currentCourse;
            CourseRevision courserev;
            Boolean chance = false;
            Boolean showonweb = false;
			Boolean futureSkill = false;
            CourseRegistrationPolicy courseregpolicy;
            if (String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
               Int32 coursetypeID = Convert.ToInt32(ddlctype.SelectedValue);
               var coursetype = (from c in context.Courses
                                     where c.CourseTypeID == coursetypeID
                                     select c).FirstOrDefault();
               if (coursetype.enmCourseType == enmCourseType.CertificationCourse)
               {
                   currentCourse = new EConnect.NIELIT.Course();
                   currentCourse.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                   currentCourse.CourseTypeID = Convert.ToInt32(ddlctype.SelectedValue);
                   currentCourse.Name = txtcoursename.Text;
                   currentCourse.Code = txtcoursecode.Text;
                   currentCourse.RegistrationServiceID = txtregserviceID.Text;
                   currentCourse.ExaminationServiceID = txtexamserviceID.Text;
                   currentCourse.DisplayOrder = Convert.ToInt32(txtdisplay.Text);
				    //Added for sectors
                   currentCourse.NielitSectorID = Convert.ToInt32(ddlNielitSectorID.SelectedValue);
                   currentCourse.NIELITrgSplnID = Convert.ToInt32(ddlNielitTrgSpecialization .SelectedValue );
                   //
                   if (ddlShowOnWeb.SelectedValue == "1")
                   {
                       showonweb = true;
                   }
                   if (ddlShowOnWeb.SelectedValue == "2")
                   {
                       showonweb = false; ;
                   }
                   currentCourse.ShowOnWeb = showonweb;
				   currentCourse.IsActive = showonweb;
                   if (ddlWhetherFuture.SelectedValue == "1")
                   {
                       futureSkill  = true;
                   }
                   if (ddlWhetherFuture.SelectedValue == "2")
                   {
                       futureSkill  = false; ;
                   }
                   currentCourse.IsFuture = futureSkill;
                   if (ddlcourse.Visible == true)
                   {
                       if (ddlcourse.SelectedValue != "0")
                           currentCourse.LowerCourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                       else
                           currentCourse.LowerCourseID = null;
                   }
                   if (ddlapplicanttype.SelectedValue == "0")
                       currentCourse.ApplicantTypeID = null;
                   else
                       currentCourse.ApplicantTypeID = Convert.ToInt32(ddlapplicanttype.SelectedValue);
                   context.Courses.Add(currentCourse);
                   context.SaveChanges();
                   courserev = new EConnect.NIELIT.CourseRevision();
                   courserev.CourseID = currentCourse.ID;
                   courserev.CourseCategoryID = currentCourse.CourseCategoryID;
                   courserev.RevisionNumber = Convert.ToInt32(Txtrevisionno.Text);
                   courserev.Name = currentCourse.Name;
                   courserev.Code = currentCourse.Code;
                   courserev.EffectiveFromDate = Convert.ToDateTime(txteffectivedate.Text);
                   context.CourseRevisions.Add(courserev);
                   context.SaveChanges();

                   //Course Registration Policy
                   courseregpolicy = new EConnect.NIELIT.CourseRegistrationPolicy();
                   courseregpolicy.CourseID = currentCourse.ID;
                   courseregpolicy.CourseCategoryID = currentCourse.CourseCategoryID;
                   courseregpolicy.EffectiveFromDate = Convert.ToDateTime(Txtregpolicyeffectivedate.Text);
                   courseregpolicy.RegistrationValidity = Convert.ToInt32(txtregperiod.Text);
                   if (ddllanguage.SelectedIndex != 3)
                   {
                       courseregpolicy.AllowedLanguage = Convert.ToInt32(ddllanguage.SelectedValue);
                   }
                   if (ddlregchances.SelectedValue == "1")
                   {
                       chance = true;
                   }
                   else if (ddlregchances.SelectedValue == "2")
                   {
                       chance = false;
                   }
                   courseregpolicy.ReRegistrationChance = chance;
                   if (txtreggape.Visible == true)
                   {
                       courseregpolicy.ReRegistrationGapInMonths = Convert.ToInt32(txtreggape.Text);
                   }
                   if (txtvalidity.Visible == true)
                   {
                       courseregpolicy.ReRegistrationValidity = Convert.ToInt32(txtvalidity.Text);
                   }
                   context.CourseRegistrationPolicies.Add(courseregpolicy);
                   context.SaveChanges();

                   //Course Exam Session Table Forenoon
                   CourseExamSession courseexamForeNoon = new EConnect.NIELIT.CourseExamSession();
                   courseexamForeNoon.CourseID = currentCourse.ID;
                   courseexamForeNoon.CourseCategoryID = currentCourse.CourseCategoryID;
                   courseexamForeNoon.ExamSessionID = Convert.ToInt32(enmExamSession.Forenoon);
                   context.CourseExamSessions.Add(courseexamForeNoon);
                   context.SaveChanges();

                   //Course Exam Session Table Aternoon
                   CourseExamSession courseexamAfterNoon = new EConnect.NIELIT.CourseExamSession();
                   courseexamAfterNoon.CourseID = currentCourse.ID;
                   courseexamAfterNoon.CourseCategoryID = currentCourse.CourseCategoryID;
                   courseexamAfterNoon.ExamSessionID = Convert.ToInt32(enmExamSession.Afternoon);
                   context.CourseExamSessions.Add(courseexamAfterNoon);
                   context.SaveChanges();
                 
                   strMessage = "New Record Saved";
               }
               else if(coursetype.enmCourseType == enmCourseType.CertificationExam)
               {
                   currentCourse = new EConnect.NIELIT.Course();
                   currentCourse.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                   currentCourse.CourseTypeID = Convert.ToInt32(ddlctype.SelectedValue);
                   currentCourse.Name = txtcoursename.Text;
                   currentCourse.Code = txtcoursecode.Text;
                   currentCourse.ExaminationServiceID = txtexamserviceID.Text;
                   currentCourse.DisplayOrder = Convert.ToInt32(txtdisplay.Text);
                   if (ddlapplicanttype.SelectedValue == "0")
                       currentCourse.ApplicantTypeID = null;
                   else
                       currentCourse.ApplicantTypeID = Convert.ToInt32(ddlapplicanttype.SelectedValue);

                   if (ddlShowOnWeb.SelectedValue == "1")
                   {
                       showonweb = true;
                   }
                   if (ddlShowOnWeb.SelectedValue == "2")
                   {
                       showonweb = false; ;
                   }
                   currentCourse.ShowOnWeb = showonweb;
				   if (ddlWhetherFuture.SelectedValue == "1")
                   {
                       futureSkill = true;
                   }
                   if (ddlWhetherFuture.SelectedValue == "2")
                   {
                       futureSkill = false; ;
                   }
                   currentCourse.IsFuture = futureSkill;
                   context.Courses.Add(currentCourse);
                   context.SaveChanges();
                   courserev = new EConnect.NIELIT.CourseRevision();
                   courserev.CourseID = currentCourse.ID;
                   courserev.CourseCategoryID = currentCourse.CourseCategoryID;
                   courserev.RevisionNumber = Convert.ToInt32(Txtrevisionno.Text);
                   courserev.Name = currentCourse.Name;
                   courserev.Code = currentCourse.Code;
                   courserev.EffectiveFromDate = Convert.ToDateTime(txteffectivedate.Text);
                   context.CourseRevisions.Add(courserev);
                   context.SaveChanges();
                   strMessage = "New Record Saved";
                 }
                
            }
            else
            {
                currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["CourseId"]));
                currentCourse.Name = txtcoursename.Text;
                currentCourse.Code = txtcoursecode.Text;
				//Added for sectors
                currentCourse.NielitSectorID = Convert.ToInt32(ddlNielitSectorID.SelectedValue);
                currentCourse.NIELITrgSplnID = Convert.ToInt32(ddlNielitTrgSpecialization.SelectedValue);
                //
                if (ddlapplicanttype.SelectedValue == "0")
                    currentCourse.ApplicantTypeID = null;
                else
                    currentCourse.ApplicantTypeID = Convert.ToInt32(ddlapplicanttype.SelectedValue);
                if (ddlShowOnWeb.SelectedValue == "1")
                {
                    showonweb = true;
                }
                if (ddlShowOnWeb.SelectedValue == "2")
                {
                    showonweb = false; ;
                }
                currentCourse.ShowOnWeb = showonweb;
				currentCourse.IsActive = showonweb;
                if (ddlWhetherFuture.SelectedValue == "1")
                {
                    futureSkill = true;
                }
                if (ddlWhetherFuture.SelectedValue == "2")
                {
                    futureSkill = false; ;
                }
                currentCourse.IsFuture = futureSkill;

                currentCourse.DisplayOrder = Convert.ToInt32(txtdisplay.Text);
                if (ddlcourse.Visible == true)
                {
                    if (ddlcourse.SelectedValue != "0")
                        currentCourse.LowerCourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                    else
                        currentCourse.LowerCourseID = null;
                }

                if (txtregserviceID.Visible == true)
                {
                    if(!String.IsNullOrEmpty(txtregserviceID.Text))
                       currentCourse.RegistrationServiceID = txtregserviceID.Text;
                }

                if (!String.IsNullOrEmpty(txtexamserviceID.Text))
                   currentCourse.ExaminationServiceID = txtexamserviceID.Text;

                context.SaveChanges();

                //course revision
                var coursere = context.CourseRevisions.Where(s => s.CourseID == currentCourse.ID).OrderByDescending(s=> s.RevisionNumber).FirstOrDefault();
                coursere.Name = currentCourse.Name;
                coursere.Code = currentCourse.Code;
                coursere.EffectiveFromDate = Convert.ToDateTime(txteffectivedate.Text);
                context.Entry(coursere).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                if(currentCourse.CourseTypeID == Convert.ToInt32(enmCourseType.CertificationCourse))
                {
                    var couregpolicy = context.CourseRegistrationPolicies.Where(s => s.CourseID == currentCourse.ID).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault();
                    couregpolicy.CourseID = currentCourse.ID;
                    couregpolicy.CourseCategoryID = currentCourse.CourseCategoryID;
                    couregpolicy.EffectiveFromDate = Convert.ToDateTime(Txtregpolicyeffectivedate.Text);
                    couregpolicy.RegistrationValidity = Convert.ToInt32(txtregperiod.Text);
                    if (ddllanguage.SelectedIndex != 3)
                    {
                        couregpolicy.AllowedLanguage = Convert.ToInt32(ddllanguage.SelectedValue);
                    }
                    if (ddlregchances.SelectedValue == "1")
                    {
                        chance = true;
                    }
                    else if (ddlregchances.SelectedValue == "2")
                    {
                        chance = false;
                    }
                    couregpolicy.ReRegistrationChance = chance;
                    if (txtreggape.Visible == true)
                    {
                        couregpolicy.ReRegistrationGapInMonths = Convert.ToInt32(txtreggape.Text);
                    }
                    if (txtvalidity.Visible == true)
                    {
                        couregpolicy.ReRegistrationValidity = Convert.ToInt32(txtvalidity.Text);
                    }
                    context.Entry(couregpolicy).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                strMessage = "Record Updated";
                 
            }
            Response.Redirect("CertificateCourse.aspx?msg=" + strMessage, true);
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
            ddlCourseType.SelectedValue = "0";
            ddlCourses.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BindGridView();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 courseid = Convert.ToInt32(hfActionID.Value);
                    EConnect.NIELIT.Course course = context.Courses.Find(courseid);
                    var revisions = from r in context.CourseRevisions
                                    where r.CourseID == courseid
                                    select r;
                    revisions.ToList().ForEach(d => context.CourseRevisions.Remove(d));
                    context.SaveChanges();

                    context.Courses.Remove(course);
                    context.SaveChanges();
                    scope.Complete();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                };
            }
            BindGridView();
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("CertificateCourse.aspx", true);
    }
    protected void ddlCourses_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id2 = Convert.ToInt32(ddlCourses.SelectedValue);
        ddlCourseName.Items.Clear();
        FillCourseNames(id2);
    }
    protected void fillpreviouscourses(Int32 coursecatID)
    {
        try
        {
            ddlcourse.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                {
                    Int32 ccatid = coursecatID;
                    Int32 coursetypeid = Convert.ToInt32(enmCourseType.CertificationCourse);
                    if (coursetypeid == Convert.ToInt32(enmCourseType.CertificationCourse))
                    {
                        var courseid = (from p in context.Courses 
                                          where p.CourseCategoryID == ccatid && p.CourseRegistrationPolicies.FirstOrDefault().RegistrationValidity!=null
                                          orderby p.LowerCourseID descending
                                          select new
                                          {
                                              ID = p.ID
                                          }).FirstOrDefault();


                        if (courseid != null)
                        {
                            ddlcourse.Visible = true;
                            Lbprevcoursename.Visible = true;
                            var CourseName = from p in context.Courses
                                             where p.CourseTypeID == coursetypeid && p.CourseCategoryID == ccatid && p.ID == courseid.ID
                                             orderby (p.DisplayOrder)
                                             select new { ValueField = p.ID, TextField = p.Name + " ("+ p.Code +")" };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseName, lst);
                        }
                        else
                        {
                            ddlcourse.Visible = false;
                            Lbprevcoursename.Visible = false;
                        }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void filleditpreviouscourses(Int32 coursecatID, Int32 editcourseid)
    {
        try
        {
            ddlcourse.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                {
                    Int32 ccatid = coursecatID;
                    Int32 editcourse = editcourseid;
                    Int32 coursetypeid = Convert.ToInt32(enmCourseType.CertificationCourse);
                    if (coursetypeid == Convert.ToInt32(enmCourseType.CertificationCourse))
                    {
                        var CourseName = from p in context.Courses
                                         where p.CourseTypeID == coursetypeid && p.CourseCategoryID == ccatid && p.ID != editcourse
                                         orderby (p.DisplayOrder)
                                         select new { ValueField = p.ID, TextField = p.Name+ " ("+ p.Code +")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseName, lst);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillCourseNames(int catID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (catID != null || catID != 0)
                {
                    var CourseName = from p in context.Courses
                                     where p.CourseCategoryID == catID
                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name+ " ("+ p.Code +")" };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseName, lst);
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlctype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursetypeID = Convert.ToInt32(ddlctype.SelectedValue);
            Int32 ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                var currentcourse = (from c in context.Courses
                                     where c.CourseTypeID == coursetypeID
                                     select c).FirstOrDefault();
                if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Filllanguage();
                    ddllanguage.Items.Insert(3, "Both");
                    fillpreviouscourses(ccatid);
                    tbreregistration.Visible = true;
                    txtregserviceID.Visible = true;
                    lblreg.Visible = true;
                }
                else if(currentcourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    tbreregistration.Visible = false;
                    ddlcourse.Visible = false;
                    Lbprevcoursename.Visible = false;
                    txtregserviceID.Visible = false;
                    lblreg.Visible = false;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            fillpreviouscourses(coursecatID);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlregchances_SelectedIndexChanged(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        if (ddlregchances.SelectedValue == "1")
        {
            Lbreregperiod.Visible = true;
            Lbrereggap.Visible = true;
            txtvalidity.Visible = true;
            txtreggape.Visible = true;
        }
        else if (ddlregchances.SelectedValue == "2")
        {
            Lbreregperiod.Visible = false;
            Lbrereggap.Visible = false;
            txtvalidity.Visible = false;
            txtreggape.Visible = false;
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
            Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var courses = from s in context.Courses
                          where s.CourseTypeID == courseType
                          select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);

            //var users1 = from s in context.Users
            //            select new { Name = s.LoginID };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            //}
            //courses = courses.Union(users1).Take(count);
            foreach (var course in courses)
            {
                items.Add(course.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
		}
    protected void FillSectorID()
    {
        try
        {
            using (NIELITMISContext  context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusName = from p in context.NielitSectorss
                                 orderby (p.sectorName)
                                 select new { ValueField = p.ID, TextField = p.sectorName + "(" + p.sectorShortCode + ")" };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitSectorID, StatusName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlNIELITSector_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillNielitTrgSpecialization(Convert.ToInt32 (ddlNielitSectorID .SelectedValue ));
    }
    protected void FillNielitTrgSpecialization(Int32 vSectorID)
    {
        try
        {
            using (NIELITMISContext   context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var NielitTrgSpecializationList = from p in context.NielitTrgSpecializations
                                                  where p.NielitSectorID==vSectorID
                                                  orderby (p.specializationName)
                                                  select new { ValueField = p.ID, TextField = p.specializationName + "  ( " + p.specializationCode + " ) " };
                NielitTrgSpecializationList = NielitTrgSpecializationList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitTrgSpecialization, NielitTrgSpecializationList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}