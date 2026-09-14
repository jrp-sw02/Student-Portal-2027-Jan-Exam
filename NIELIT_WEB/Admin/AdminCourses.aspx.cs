using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using System.Text;
using EConnect.NIELIT;
using System.Transactions;
public partial class Admin_AdminCourses : BasePage
{
    string strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    String currentRoleName = string.Empty;
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
            currentRoleName = (string)Session["RoleName"];
            if (currentRoleName == "Technical Support Query")
            {
                //Response.Write("Sorry! You don't have rights  to view this page");
                btnSave.Visible = false;
                btnapplicanttype.Visible = false;
                btninstitute.Visible = false;
                //Response.End();
            }
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Common/SearchCandidate.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    Filltype();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    Bindcourses();
                    BindGridView();
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
    protected void Filltype()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var app = from p in context.ApplicantTypes
                          orderby (p.Name)
                          select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlapptype, app, lst);
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
            showsidelink();
            context = new EConnectContext();
            btnSave.Visible = false;
            btnCancel.Visible = true;
            Btnback.Visible = false;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses Detail:Update", "Admin/AdminCourses.aspx?" + Request.QueryString.ToString(), ""));
            BreadCrumb1.Render();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 statusid = Convert.ToInt32(Request.QueryString["StatusId"]);
            var student = (from c in context.Candidates
                          join rg in context.RegistrationDetails
                          on c.ID equals rg.CandidateID
                           where c.ID == Appid && rg.CourseID == courseID && rg.RegistrationStatusID == statusid
                          select new
                          {
                              Course = rg.Course.Name.ToUpper(),
                              regdate = rg.RegistrationDate,
                              validdate = rg.ValidUptoDate,
                              status = (rg.RegistrationStatusID.HasValue ? rg.RegistrationStatusID.Value : 0),
                              CandtypeID = (rg.ApplicantTypeID!=0?rg.ApplicantTypeID:0),
                              CommencementDate = rg.CommencementFromDate,
                              courseID = rg.CourseID,
                              instituteid = rg.ApplicantTypeID == 2 ? rg.InstituteID.Value : 0,
                              name= c.Name,
                              reregistrationdate = rg.ReRegistrationDate,
                              expiryDate = rg.ExpiryDate,
                              reregno = rg.ReRegistrationNumber,
                              regno = rg.RegistrationNo,
                              cancelleddate = rg.CancelledOn
                          }).FirstOrDefault();
            btnMode.Visible = true;
            btnSave.Text = "Update";
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "Courses Details";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            lbtxtcname.Text = student.Course;
            Lbname.Text = GetInitCap(student.name);
            EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapptype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--Select One--", "0"));
            EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlstatus, typeof(EConnect.NIELIT.enmRegistrationStatus), new ListItem("--Select One--", "0"));
            ddlstatus.SelectedValue = student.status.ToString();
            ddlstatus.Enabled = false; // need to change in future
            ddlapptype.SelectedValue = student.CandtypeID.ToString();
            ddlapptype.Enabled = false; // need to change in future
            lbtxtregdate.Enabled = false;
            lbcommencementDate.Enabled = false;
            lbregno.Enabled = false;
            lbtxtvaliddate.Enabled = false;
            if (ddlapptype.SelectedValue == "2")
            {
                btninstitute.Visible = true;
                btnapplicanttype.Visible = true;
                ddlinstitute.Enabled = false;
                var institute = (from i in context.Institutes
                                join d in context.AccreditationDetails
                                    on i.ID equals d.InstituteID
                                where d.CourseID == student.courseID
                                orderby i.Name
                                select new
                                {
                                    TextField = i.Name + ", " + i.CityName + " ( " + d.AccreditationNumber + " ) ",
                                    ValueField = i.ID
                                }).ToList();
                ListItem lst = new ListItem("--Select One--");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitute, institute.Distinct(), lst);
                ddlinstitute.SelectedValue = student.instituteid.ToString();
            }
            else if (ddlapptype.SelectedValue == "1")
            {
                ListItem lst1 = new ListItem("--Select One--");
                ddlinstitute.Items.Add(lst1);
                ddlinstitute.Enabled = false;
                btninstitute.Visible = false;
                //btnapplicanttype.Visible = false;
            }
            // Registration Status
            if (ddlstatus.SelectedValue == "2")
            {
                Lblregdate.Text = "Re-Registration Date";
                Lbl_effDate.Text = "Valid Upto ";
                lbregno.Visible = true;
                lbcommencementDate.Visible = false;
                Lbcommendate.Text = "Re-Registration Number";
                if(student.reregistrationdate.HasValue)
                   lbtxtregdate.Text =  student.reregistrationdate.Value.ToString("dd-MMM-yyyy");
                else
                   lbtxtregdate.Text =  "";
                if (student.reregno.HasValue)
                    lbregno.Text = student.reregno.Value.ToString();
                else
                    lbregno.Text = "";
                lbtxtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
            }
            else if (ddlstatus.SelectedValue == "5")
            {
                Lblregdate.Text = "Expiry Date";
                Lbl_effDate.Text = "Valid Upto ";
                lbregno.Visible = true;
                lbcommencementDate.Visible = false;
                Lbcommendate.Text = "Registration Number";
                if (student.expiryDate.HasValue)
                    lbtxtregdate.Text = student.expiryDate.Value.ToString("dd-MMM-yyyy");
                else
                    lbtxtregdate.Text = "";
                lbregno.Text = student.regno.ToString();
                lbtxtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
            }
            else if (ddlstatus.SelectedValue == "6")
            {
                lbcommencementDate.Visible = true;
                lbregno.Visible = false;
                Lbcommendate.Text = "Commencement Date";
                Lblregdate.Text = "Registration Date";
                Lbl_effDate.Text = "Cancelled Date";
                lbtxtregdate.Text = student.regdate.ToString("dd-MMM-yyyy");
                if (student.cancelleddate.HasValue)
                    lbtxtvaliddate.Text = student.cancelleddate.Value.ToString("dd-MMM-yyyy");
                else
                    lbtxtvaliddate.Text = "";
                lbcommencementDate.Text = student.CommencementDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                lbcommencementDate.Visible = true;
                lbregno.Visible = false;
                Lbcommendate.Text = "Commencement Date";
                Lblregdate.Text = "Registration Date";
                Lbl_effDate.Text = "Valid Upto ";
                lbtxtregdate.Text = student.regdate.ToString("dd-MMM-yyyy");
                lbtxtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
                lbcommencementDate.Text = student.CommencementDate.ToString("dd-MMM-yyyy");
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
    protected void Bindcourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var Course = from p in context.Courses
                             where p.CourseCategoryID == 1
                             orderby (p.DisplayOrder)
                             select new { ValueField = p.ID, TextField = p.Name.ToUpper() };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcname, Course, lst);
            };
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
            int courseid = 0;
            if (ddlcname.SelectedValue != "0")
                courseid = Convert.ToInt32(ddlcname.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            var student = from c in context.Candidates
                        join rg in context.RegistrationDetails
                        on c.ID equals rg.CandidateID
                        where c.ID == Appid
                        orderby rg.Course.DisplayOrder
                        select new
                        {
                            ID = c.ID,
                            Course = rg.Course.Name.ToUpper(),
                            regdate = rg.RegistrationDate,
                            validdate = rg.ValidUptoDate,
                            status = (rg.RegistrationStatusID.HasValue ? rg.RegistrationStatus.Name : "NA"),
                            appid = Appid,
                            CourseID = rg.CourseID,
                            CandId = rg.CandidateID,
                            StatusId= rg.RegistrationStatusID.HasValue ? rg.RegistrationStatusID.Value : 0,
                            RegID = rg.ID 
                        };
            if (courseid != 0)
                student = student.Where(p => p.CourseID == courseid);
            if (!string.IsNullOrEmpty(searchString))
            {
                student = student.Where((s => s.Course.Contains(searchString)));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.ID);
                        else
                            student = student.OrderBy(s => s.ID);
                        break;
                    case "Course":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Course);
                        else
                            student = student.OrderBy(s => s.Course);
                        break;
                    case "regdate":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.regdate);
                        else
                            student = student.OrderBy(s => s.regdate);
                        break;
                    case "validdate":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.validdate);
                        else
                            student = student.OrderBy(s => s.validdate);
                        break;
                    case "status":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.status);
                        else
                            student = student.OrderBy(s => s.status);
                        break;
                    default:
                        student = student.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(student, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Detail", "Admin/AdminCourses.aspx?key1=" + Request.QueryString["key1"], ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Detail", "Admin/AdminCourses.aspx?" + Request.QueryString.ToString(), ""));
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
            //lblHeading.Text = "New Course";
            
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCourses.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("AdminCourses.aspx", true);
            } 
        }
        
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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
    protected void SearchBar_Reset(object sender, EventArgs e)
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            btnSave.Visible = true;
            context = new EConnectContext();
            StringBuilder mySql = new StringBuilder();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 statusid = Convert.ToInt32(Request.QueryString["StatusId"]);
            DateTime createdDate = DateTime.Now;
            Int32 createdByID = Convert.ToInt32(Session["UserID"]);
            strMessage = " Record updated ";
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                // Inserting in Registration history Table
                mySql.Append("insert into Registration_Detail_History (Candidate_ID,Registration_No, Course_Category_ID, Course_ID , Registration_Month  , Registration_Year  ,Registration_Date ,Commencement_From_Date  ,Valid_Upto_Date  ,whether_extension  ,registration_status  ,Reg_Type_ID  ,Registration_Status_ID  ,REG_TYPE  ,Applicant_Type_ID  ,Institute_ID  ,Experience_In_Years  ,Re_Registration_No  ,Expiry_Date  ,Re_Registration_Date  ,Completion_Month ,Completion_Year  ,Cancelled_On  ,Cancelled_By  ,Courser_Reg_Appl_ID ,Current_Reg_Status_ID , Created_On, Created_By)  (select rd.Candidate_ID, rd.Registration_No, rd.Course_Category_ID, rd.Course_ID  ,rd.Registration_Month  ,rd.Registration_Year  ,rd.Registration_Date ,rd.Commencement_From_Date , rd.Valid_Upto_Date  ,rd.whether_extension  , rd.registration_status  , rd.Reg_Type_ID  , rd.Registration_Status_ID ,rd.REG_TYPE  , rd.Applicant_Type_ID  ,rd.Institute_ID  ,rd.Experience_In_Years  , rd.Re_Registration_No  ,rd.Expiry_Date , rd.Re_Registration_Date  , rd.Completion_Month , rd.Completion_Year  ,rd.Cancelled_On  , rd.Cancelled_By  , rd.Courser_Reg_Appl_ID ,rd.Current_Reg_Status_ID , '" + createdDate + "' , " + createdByID + " from  Registration_Detail rd where rd.Candidate_ID = " + Appid + " and rd.Course_ID = " + courseID + "  and rd.Registration_Status_ID = " + statusid + ")");
                EConnect.Utils.Data.DbUtility.ExecuteNonQuery(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);

                // Updating Registration Details
                using (TransactionScope scope = new TransactionScope())
                {
                    var student = (from c in context.Candidates
                                   join rg in context.RegistrationDetails
                                   on c.ID equals rg.CandidateID
                                   where c.ID == Appid && rg.CourseID == courseID && rg.RegistrationStatusID == statusid
                                   orderby rg.RegistrationStatus.Name ascending
                                   select rg).FirstOrDefault();

                    student.Course.Name = lbtxtcname.Text;
                    student.RegistrationStatusID = Convert.ToInt32(ddlstatus.SelectedValue);
                    if (ddlapptype.SelectedValue == "2")
                    {
                        student.ApplicantTypeID = Convert.ToInt32(ddlapptype.SelectedValue);
                        if (ddlinstitute.SelectedValue.ToUpper().Trim() == "--Select One--".ToUpper().Trim())
                        {
                            ShowAlert("Please select institute name.", true);
                            return;
                        }
                        student.InstituteID = Convert.ToInt64(ddlinstitute.SelectedValue);
                    }
                    else if (ddlapptype.SelectedValue == "1")
                    {
                        student.ApplicantTypeID = Convert.ToInt32(ddlapptype.SelectedValue);
                        student.InstituteID = null;
                    }
                    context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    strMessage += " in candidate registration details of " + student.Course.Name + " course with registration number:-" + student.RegistrationNo;
                    if (context.CourseExamApplications.Any(s => s.CandidateID == Appid && s.CourseID == courseID && s.RegistrationNumber == student.RegistrationNo))
                    {
                        // Updating Course Exam Application
                        CourseExamApplication cexam = context.CourseExamApplications.Where(s => s.CourseID == courseID && s.CandidateID == Appid && s.RegistrationNumber == student.RegistrationNo).OrderByDescending(s=>s.ID).FirstOrDefault();
                        if (cexam != null)
                        {
                            if (ddlapptype.SelectedValue == "2")
                            {
                                if (ddlinstitute.Enabled == true && ddlapptype.Enabled == false)
                                {
                                    if ((cexam.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)) && ((cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)) || (cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute))))
                                    {
                                        if (ddlinstitute.SelectedValue.ToUpper().Trim() == "--Select One--".ToUpper().Trim())
                                        {
                                            ShowAlert("Please select institute name.", true);
                                            return;
                                        }
                                        cexam.InstituteID = Convert.ToInt64(ddlinstitute.SelectedValue);
                                        strMessage += " , and in " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " course exam application with application number:-" + cexam.Number;
                                    }
                                    else if (cexam.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                                    {
                                        if (ddlinstitute.SelectedValue.ToUpper().Trim() == "--Select One--".ToUpper().Trim())
                                        {
                                            ShowAlert("Please select institute name.", true);
                                            return;
                                        }
                                        cexam.InstituteID = Convert.ToInt64(ddlinstitute.SelectedValue);
                                        strMessage += " , and in " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " course exam application with application number:-" + cexam.Number;
                                    }
                                }
                                else if (ddlinstitute.Enabled == true && ddlapptype.Enabled == true)
                                {
                                    if ((cexam.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)) && (cexam.ApplicationStatusID ==Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate)) && CommonFunctions.IsDemandNoteCancellable(cexam.DemandNoteID.HasValue ? cexam.DemandNoteID.Value : 0))
                                    {
                                        cexam.ApplicantTypeID = Convert.ToInt32(ddlapptype.SelectedValue);
                                        if (ddlinstitute.SelectedValue.ToUpper().Trim() == "--Select One--".ToUpper().Trim())
                                        {
                                            ShowAlert("Please select institute name.", true);
                                            return;
                                        }
                                        cexam.InstituteID = Convert.ToInt64(ddlinstitute.SelectedValue);
                                        cexam.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                        cexam.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Institute);
                                        if (CommonFunctions.IsDemandNoteCancellable(cexam.DemandNoteID.HasValue ? cexam.DemandNoteID.Value : 0))
                                        {
                                            cexam.DemandNoteID = null;
                                        }
                                        strMessage += " , and in " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " course exam application with application number:-" + cexam.Number;
                                    }
                                }
                            }
                            else if (ddlapptype.SelectedValue == "1")
                            {
                                if (cexam.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && (cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate)))
                                {
                                    cexam.ApplicantTypeID = Convert.ToInt32(ddlapptype.SelectedValue);
                                    cexam.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Candidate);
                                    cexam.InstituteID = null;
                                    cexam.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                                    strMessage += " , and in " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " course exam application with application number:-" + cexam.Number;
                                }
                            }
                            context.Entry(cexam).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                       
                            // Updating Course Exam Application Detail
                            if (context.CourseExamApplicationDetails.Any(s => s.CourseExamApplicationID == cexam.ID))
                            {
                                if (ddlapptype.SelectedValue == "2")
                                {
                                    if ((cexam.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)) && ((cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)) || (cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute))))
                                    {
                                        if (ddlinstitute.SelectedValue.ToUpper().Trim() == "--Select One--".ToUpper().Trim())
                                        {
                                            ShowAlert("Please select institute name.", true);
                                            return;
                                        }
                                        context.Database.ExecuteSqlCommand("Update Course_Exam_Application_Detail set Institute_ID = " + Convert.ToInt64(ddlinstitute.SelectedValue) + " Where Course_Exam_Appl_ID = " + cexam.ID);
                                        strMessage += " , and in course exam application details of " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " examination with course exam application ID:-"+ cexam.ID;
                                    }
                                    else if (cexam.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                                    {
                                        if (ddlinstitute.SelectedValue.ToUpper().Trim() == "--Select One--".ToUpper().Trim())
                                        {
                                            ShowAlert("Please select institute name.", true);
                                            return;
                                        }
                                        context.Database.ExecuteSqlCommand("Update Course_Exam_Application_Detail set Institute_ID = " + Convert.ToInt64(ddlinstitute.SelectedValue) + " Where Course_Exam_Appl_ID = " + cexam.ID);
                                        strMessage += " , and in course exam application details of " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " examination with course exam application ID:-" + cexam.ID;
                                    }
                                }
                                else if (ddlapptype.SelectedValue == "1")
                                {
                                    if (cexam.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && (cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cexam.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate)))
                                    {
                                        context.Database.ExecuteSqlCommand("Update Course_Exam_Application_Detail set Institute_ID = null where Course_Exam_Appl_ID =" + cexam.ID);
                                        strMessage += " , and in course exam application details of " + cexam.Exam.Name + " (" + cexam.Course.Name + " )" + " examination with course exam application ID:-" + cexam.ID;
                                    }
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                   scope.Complete();
                };
            }
            GetInitCap(strMessage);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCourses.aspx?key1=" + Request.QueryString["key1"]+"&msg="+strMessage)); 
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
            ddlcname.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
            var courses = from s in context.Courses
                                where s.CourseCategoryID == 1
                                select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);
            foreach (var user in courses)
            {
                items.Add(user.Name);
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
        BreadCrumb1.Render();
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCourses.aspx?key1=" + Request.QueryString["key1"]));
    }
    protected void showsidelink()
    {
        try
        {
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 RegistrationID = Convert.ToInt32(Request.QueryString["RegID"]);
            context = new EConnectContext();
            var student = (from c in context.Candidates
                           join rg in context.RegistrationDetails
                           on c.ID equals rg.CandidateID
                           where c.ID == Appid && rg.CourseID == courseID && rg.ID == RegistrationID
                           orderby rg.RegistrationDate descending
                           select new
                           {
                               CourseID = rg.CourseID,
                               CandidateID = rg.CandidateID,
                               RegNo = rg.RegistrationNo,
                           }).FirstOrDefault();
            SideLink1.Items.Add(new SideLinkItem("Modules Status", "../CAND/FrmViewDetail.aspx?CourseID=" +student.CourseID+"&RegNo="+student.RegNo+"&CandidateID="+student.CandidateID, "", ""));
            SideLink1.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
            SideLink1.Render();
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
    protected void ddlapptype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 statusid = Convert.ToInt32(Request.QueryString["StatusId"]);
            using (EConnectContext context = new EConnectContext())
            {
                var student = (from c in context.Candidates
                               join rg in context.RegistrationDetails
                               on c.ID equals rg.CandidateID
                               where c.ID == Appid && rg.CourseID == courseid && rg.RegistrationStatusID == statusid
                               select new
                               {
                                   courseID = rg.CourseID,
                                   instituteid = rg.ApplicantTypeID == 2 ? rg.InstituteID.Value : 0,
                               }).FirstOrDefault();
                if (ddlapptype.SelectedValue == "1")
                {
                    ListItem lst1 = new ListItem("--Select One--");
                    ddlinstitute.Items.Clear();
                    ddlinstitute.Items.Add(lst1);
                }
                else if (ddlapptype.SelectedValue == "2")
                {

                    var institute = (from i in context.Institutes
                                     join d in context.AccreditationDetails
                                         on i.ID equals d.InstituteID
                                     where d.CourseID == student.courseID
                                     orderby i.Name
                                     select new
                                     {
                                         TextField = i.Name + ", " + i.CityName + " ( " + d.AccreditationNumber + " ) ",
                                         ValueField = i.ID
                                     }).ToList();
                    ListItem lst = new ListItem("--Select One--");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitute, institute.Distinct(), lst);
                    ddlinstitute.SelectedValue = student.instituteid.ToString();
                }
                else
                {
                    ListItem lst2 = new ListItem("--Select One--");
                    ddlinstitute.Items.Clear();
                    ddlinstitute.Items.Add(lst2);
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 statusid = Convert.ToInt32(Request.QueryString["StatusId"]);
            using (EConnectContext context = new EConnectContext())
            {
                var student = (from c in context.Candidates
                               join rg in context.RegistrationDetails
                               on c.ID equals rg.CandidateID
                               where c.ID == Appid && rg.CourseID == courseid && rg.RegistrationStatusID == statusid
                               select new
                               {
                                   Course = rg.Course.Name.ToUpper(),
                                   regdate = rg.RegistrationDate,
                                   validdate = rg.ValidUptoDate,
                                   status = (rg.RegistrationStatusID.HasValue ? rg.RegistrationStatusID.Value : 0),
                                   CandtypeID = (rg.ApplicantTypeID != 0 ? rg.ApplicantTypeID : 0),
                                   CommencementDate = rg.CommencementFromDate,
                                   courseID = rg.CourseID,
                                   instituteid = rg.ApplicantTypeID == 2 ? rg.InstituteID.Value : 0,
                                   name = c.Name,
                                   reregistrationdate = rg.ReRegistrationDate,
                                   expiryDate = rg.ExpiryDate,
                                   reregno = rg.ReRegistrationNumber,
                                   regno = rg.RegistrationNo,
                                   cancelleddate = rg.CancelledOn
                               }).FirstOrDefault();

                // Registration Status
                if (ddlstatus.SelectedValue == "2")
                {
                    Lblregdate.Text = "Re-Registration Date";
                    Lbl_effDate.Text = "Valid Upto ";
                    lbregno.Visible = true;
                    lbcommencementDate.Visible = false;
                    Lbcommendate.Text = "Re-Registration Number";
                    if (student.reregistrationdate.HasValue)
                        lbtxtregdate.Text = student.reregistrationdate.Value.ToString("dd-MMM-yyyy");
                    else
                        lbtxtregdate.Text = "";
                    if (student.reregno.HasValue)
                        lbregno.Text = student.reregno.Value.ToString();
                    else
                        lbregno.Text = "";
                    lbtxtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
                }
                else if (ddlstatus.SelectedValue == "5")
                {
                    Lblregdate.Text = "Expiry Date";
                    Lbl_effDate.Text = "Valid Upto ";
                    lbregno.Visible = true;
                    lbcommencementDate.Visible = false;
                    Lbcommendate.Text = "Registration Number";
                    if (student.expiryDate.HasValue)
                        lbtxtregdate.Text = student.expiryDate.Value.ToString("dd-MMM-yyyy");
                    else
                        lbtxtregdate.Text = "";
                    lbregno.Text = student.regno.ToString();
                    lbtxtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
                }
                else if (ddlstatus.SelectedValue == "6")
                {
                    lbcommencementDate.Visible = true;
                    lbregno.Visible = false;
                    Lbcommendate.Text = "Commencement Date";
                    Lblregdate.Text = "Registration Date";
                    Lbl_effDate.Text = "Cancelled Date";
                    lbtxtregdate.Text = student.regdate.ToString("dd-MMM-yyyy");
                    if (student.cancelleddate.HasValue)
                        lbtxtvaliddate.Text = student.cancelleddate.Value.ToString("dd-MMM-yyyy");
                    else
                        lbtxtvaliddate.Text = "";
                    lbcommencementDate.Text = student.CommencementDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    lbcommencementDate.Visible = true;
                    lbregno.Visible = false;
                    Lbcommendate.Text = "Commencement Date";
                    Lblregdate.Text = "Registration Date";
                    Lbl_effDate.Text = "Valid Upto ";
                    lbtxtregdate.Text = student.regdate.ToString("dd-MMM-yyyy");
                    lbtxtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
                    lbcommencementDate.Text = student.CommencementDate.ToString("dd-MMM-yyyy");
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnapplicanttype_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlapptype.Enabled = true;
            ddlinstitute.Enabled = true;
            btninstitute.Visible = false;
            btnapplicanttype.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            Btnback.Visible = true;
            divinfo.Visible = true;
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            using (EConnectContext context = new EConnectContext())
            {
                var courseexam = (from c in context.CourseExamApplications
                                  where c.CandidateID == Appid && c.CourseID == courseID
                                  orderby c.ID descending
                                  select c).FirstOrDefault();
                if (courseexam != null)
                {
                    lbfilter.Text = "Current Course Exam Application Details of Candidate are as followed:- <br>";
                    lbfilter.Text += "Application Number:-" + courseexam.Number + "<br/>";
                    lbfilter.Text += "Application Date:-" + courseexam.ApplicationDate.ToString("dd-MMM-yyyy") + "<br/>";
                    lbfilter.Text += "Application Status:-" + GetInitCap(courseexam.ApplicationStatus.Name) + "<br/>";
                    lbfilter.Text += "Exam Name:-" + courseexam.Exam.Name + "<br/>";
                    lbfilter.Text += "Course Name:-" + courseexam.Course.Name + "<br/>";
                    lbfilter.Text += "Applicant Type:-" + courseexam.ApplicantType.Name + "<br/>";
                    lbfilter.Text += "Institute Name:-" + (courseexam.InstituteID.HasValue ? courseexam.Institute.Name : "NA") + "<br/>";
                    lbfilter.Text += "To change the applicant type of candidate please select the candidate type as Direct and click on the update button.<br/>";
                    lbfilter.Text += "If you do not want to change the applicant type of the candidate click on the cancel button.";
                }
                else
                {
                    lbfilter.Text = "No Current Course Exam Application Details found for the candidate.<br/>";
                    lbfilter.Text += "To change the applicant type of candidate please select the candidate type as Direct and click on the update button.<br/>";
                    lbfilter.Text += "If you do not want to change the applicant type of the candidate click on the cancel button.";
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btninstitute_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlapptype.Enabled = false;
            ddlinstitute.Enabled = true;
            btnapplicanttype.Visible = false;
            btninstitute.Visible = false;
            btnSave.Visible = true;
            divinfo.Visible = true;
            btnCancel.Visible = false;
            Btnback.Visible = true;
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            using (EConnectContext context = new EConnectContext())
            {
                var courseexam = (from c in context.CourseExamApplications
                                  where c.CandidateID == Appid && c.CourseID == courseID 
                                  orderby c.ID descending
                                  select c).FirstOrDefault();
                if (courseexam != null)
                {
                    lbfilter.Text = "Current Course Exam Application Details of Candidate are as followed:- <br>";
                    lbfilter.Text += "Application Number:-" + courseexam.Number + "<br/>";
                    lbfilter.Text += "Application Date:-" + courseexam.ApplicationDate.ToString("dd-MMM-yyyy") + "<br/>";
                    lbfilter.Text += "Application Status:-" + GetInitCap(courseexam.ApplicationStatus.Name) + "<br/>";
                    lbfilter.Text += "Exam Name:-" + courseexam.Exam.Name + "<br>";
                    lbfilter.Text += "Course Name:-" + courseexam.Course.Name + "<br>";
                    lbfilter.Text += "Applicant Type:-" + courseexam.ApplicantType.Name + "<br>";
                    lbfilter.Text += "Institute Name:-" + (courseexam.InstituteID.HasValue ? courseexam.Institute.Name : "NA") + "<br/>";
                    lbfilter.Text += "To change the current institute name of the candidate please select the new institute name and click on the update button.<br/>";
                    lbfilter.Text += "If you do not want to change the current institute name of the candidate click on the cancel button.";
                }
                else
                {
                    lbfilter.Text  = "No Current Course Exam Application details found for the candidate.<br/>";
                    lbfilter.Text += "To change the current institute name of the candidate please select the new institute name and click on the update button.<br/>";
                    lbfilter.Text += "If you do not want to change the current institute name of the candidate click on the cancel button.";
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Btnback_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("AdminCourses.aspx?"+Request.QueryString.ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}