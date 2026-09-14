using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using System.Data.Objects;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class HO_ConsolidatedPaymentApplicationFilter : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
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
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCastCategory, typeof(EConnect.enmCastCategory), new ListItem("--All--", "0"));
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlapplicationtype, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--All--", "0"));
                OccupationFill();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Consolidated Payment Application Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }

    #region Private Methods--
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
    protected void binRegCentre()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var RegCentre = from c in context.RegionalCenters
                                where c.ID == entityID
                                select new
                                {
                                    ValueField = c.ID,
                                    TextField = c.Name
                                };
                if (RegCentre.Count() > 0)
                {
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, RegCentre.Distinct(), lst);
                    ddlRegionalCentre.SelectedValue = entityID.ToString();
                    ddlRegionalCentre.Enabled = false;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void OccupationFill()
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var Occ = from p in context.Occupations
                          select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlOccupation, Occ, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (loginUserType == UserType.RegionalCenter)
                {

                    var courses = from s in context.CourseCategories
                                  join c in context.Courses
                                      on s.ID equals c.CourseCategoryID
                                  where c.CourseTypeID == courseTypeCertificateExam
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);
                }
                else
                {
                    var courses = from s in context.CourseCategories
                                  select new { ValueField = s.ID, TextField = s.Name  };


                    if ((currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                        courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses, lst);
                }
                if (loginUserType == UserType.Institute)
                {                  
                    ddlapplicationtype.SelectedValue = "2";
                    ddlapplicationtype.Enabled = false;
                    bindinst();
                }
                else if (loginUserType == UserType.RegionalCenter)
                {
                    binRegCentre();
                    ddlRegionalCentre.SelectedValue = entityID.ToString();
                    ddlRegionalCentre.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindInstitutes(Int32 ApplicationTypeID, Int32 courseID, Int32 examid)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var examid1 = from s in context.Exams
                              where s.Name == ddlExamName.SelectedItem.Text
                              select s.ID;
                if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    var institutes = from c in context.CourseExamApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     where examid1.Contains(c.ExamID) && a.CourseCategoryID == courseID
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
                else if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    var examid2 = from s in context.Exams
                                  where s.Name == ddlExamName.SelectedItem.Text
                                  select s.ID.ToString();
                    var institutes = from c in context.CourseRegistrationApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     where examid2.Contains(c.ApplicableExamID.ToString()) && a.CourseID == courseID  // c.ApplicableExamID == examid
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
                else if (ApplicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var institutes = from c in context.CertificateExamApplications
                                     join i in context.Institutes
                                         on c.InstituteID equals i.ID
                                     join a in context.AccreditationDetails
                                         on i.ID equals a.InstituteID
                                     where a.CourseCategoryID == courseID && examid1.Contains(c.ExamID)
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
    #endregion

    #region Events--
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {               
                Course cr = context.Courses.Where(s => s.CourseCategoryID == courseCatId).FirstOrDefault();
                ListItem lst = new ListItem("--Select One--", "0");
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                    if (loginUserType == UserType.Institute)
                    {                       
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
                ddlAppType.SelectedValue = "0";
                ddlAppType_SelectedIndexChanged(ddlapplicationtype, EventArgs.Empty);              
            };
            if (ddlCourseCategry.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
                if (loginUserType != UserType.RegionalCenter)
                {
                    ddlRegionalCentre.Items.Clear();
                    ddlRegionalCentre.Items.Add(lst1);
                }            
                ddlCastCategory.SelectedValue = "0";
                ddlOccupation.SelectedValue = "0";
                if (loginUserType != UserType.Institute)
                {
                    ddlapplicationtype.SelectedValue = "0";
                    ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                }
            }
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
            ListItem lst2 = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 RegionalCenterId = Convert.ToInt32(ddlRegionalCentre.SelectedValue);
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        
            using (var context = new EConnectContext())
            {
                Course currentcourse = context.Courses.Where(s => s.CourseCategoryID == courseCatId).FirstOrDefault();
                ListItem lst = new ListItem("--Select One--", "0");
                if (AppTypeId > 0)
                {                   
                    if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                    {
                        Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                        if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        {                            
                                //var courses = (from s in context.Exams
                                //               join c in context.CourseRegistrationApplications
                                //                   on s.ID equals c.ApplicableExamID
                                //               where s.CourseCategoryID == courseCatId //&& s.ExaminationCycleID == ExamCycleId
                                //               select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                                //courses = courses.OrderByDescending(s => s.ValueField);
                                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                            using (DataTable dt = GetExamNameList())
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    ddlExamName.DataSource = dt;
                                    ddlExamName.DataTextField = "Name";
                                    ddlExamName.DataValueField = "IDs";
                                    ddlExamName.DataBind();
                                    ddlExamName.Items.Insert(0, new ListItem("--Select One--", "0"));
                                }
                            }
                            if (loginUserType == UserType.Institute)
                            {
                                ddlapplicationtype.SelectedValue = "2";
                                ddlapplicationtype.Enabled = false;
                                bindinst();
                            }
                        }
                        else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {                           
                            //var courses = (from s in context.Exams
                            //               join c in context.CourseExamApplications
                            //                   on s.ID equals c.ExamID
                            //               where s.CourseCategoryID == courseCatId //&& s.ExaminationCycleID == ExamCycleId
                            //               select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                            //courses = courses.OrderByDescending(s => s.ValueField);
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                            using (DataTable dt = GetExamNameList())
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    ddlExamName.DataSource = dt;
                                    ddlExamName.DataTextField = "Name";
                                    ddlExamName.DataValueField = "IDs";
                                    ddlExamName.DataBind();
                                    ddlExamName.Items.Insert(0, new ListItem("--Select One--", "0"));
                                }
                            }
                            if (loginUserType == UserType.Institute)
                            {
                                ddlapplicationtype.SelectedValue = "2";
                                ddlapplicationtype.Enabled = false;
                                bindinst();
                            }
                        }
                    }
                    else if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
                    {                           
                            //var courses = (from s in context.Exams
                            //               where s.CourseCategoryID == courseCatId //&& s.ExaminationCycleID == ExamCycleId 
                            //               && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                            //               orderby s.ExamYear descending, s.ExamMonth descending
                            //               select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                            //courses = courses.OrderByDescending(s => s.ValueField); 
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                            using (DataTable dt = GetExamNameList())
                            { 
                                if (dt.Rows.Count > 0)
                                {
                                    ddlExamName.DataSource = dt;
                                    ddlExamName.DataTextField = "Name";
                                    ddlExamName.DataValueField = "IDs";
                                    ddlExamName.DataBind();
                                    ddlExamName.Items.Insert(0, new ListItem("--Select One--", "0"));
                                }
                            }
                        }

                        if (loginUserType == UserType.Institute)
                        {
                            ddlapplicationtype.SelectedValue = "2";
                            ddlapplicationtype.Enabled = false;
                            bindinst();
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
            if (ddlAppType.SelectedValue == "0")
            {
                ListItem lst1 = new ListItem("--All--", "0");
               
                if (loginUserType != UserType.RegionalCenter)
                {
                    ddlRegionalCentre.Items.Clear();
                    ddlRegionalCentre.Items.Add(lst1);
                }              
                ddlCastCategory.SelectedValue = "0";
                ddlOccupation.SelectedValue = "0";
              
                if (loginUserType != UserType.Institute)
                {
                    ddlapplicationtype.SelectedValue = "0";
                    ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
                }
            }         
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public DataTable GetExamNameList()
    {
        int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetExamNameDistinct", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@courseCatId", SqlDbType.Int));
                    cmd.Parameters["@courseCatId"].Value = courseCatId;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }
     protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";         
            ddlAppType.SelectedValue = "0";        
            ddlExamName.SelectedValue = "0";          
            ddlapplicationtype.SelectedValue = "0";
            Ddlinstitutes.SelectedValue = "0";
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
                int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                Int32 examid = Convert.ToInt32(ddlExamName.SelectedValue);
                BindInstitutes(ApplicationTypeID, courseCatId, examid);
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
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 AppTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);           
            Int32 CourseID = 0;
            Int32 stid = Convert.ToInt32(enmLocationType.State);
            using (EConnectContext context = new EConnectContext())
            {
                if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    var capp = from cr in context.CourseRegistrationApplications
                               join lo in context.Locations on cr.CorStateID equals lo.ID
                               where cr.ApplicableExamID == ExamID && cr.CourseID == CourseID && lo.LocationTypeID == stid && cr.FinalSubmitted == true
                               orderby lo.Name
                               select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst = new ListItem("--All--", "0");

                   
                    ddlRegionalCentre.Enabled = false;
                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }                  
                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    Int32 correspondenceAddress = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                   
                    ddlRegionalCentre.Enabled = false;
                    var capp1 = from cr in context.CourseExamApplications
                                join lo in context.ExamCenters on cr.ExamCenter1ID equals lo.ID
                                orderby lo.Name
                                where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst1 = new ListItem("--All--", "0");

                    var capp2 = from cr in context.CourseExamApplications
                                join lo in context.ExamCenters on cr.ExamCenter2ID equals lo.ID
                                orderby lo.Name
                                where cr.ExamID == ExamID && cr.CourseID == CourseID && cr.FinalSubmitted == true
                                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst2 = new ListItem("--All--", "0");

                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }                
                }
                else if (AppTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    
                    ddlRegionalCentre.Enabled = true;                   

                    var examid = from s in context.Exams
                                 where s.Name == ddlExamName.SelectedItem.Text
                                 select s.ID;
                    var RegCentre = from cr in context.CertificateExamApplications
                                    join lo in context.RegionalCenters on cr.RegionalCenterID equals lo.ID
                                    where examid.Contains(cr.ExamID) && cr.CourseCategoryID == courseCatId
                                    && cr.FinalSubmitted == true
                                    orderby lo.Name
                                    select new { ValueField = lo.ID, TextField = lo.Name };
                    //var RegCentre = from cr in context.CertificateExamApplications
                    //                join lo in context.RegionalCenters on cr.RegionalCenterID equals lo.ID
                    //                where cr.ExamID == ExamID && cr.CourseCategoryID == courseCatId
                    //                && cr.FinalSubmitted == true
                    //                orderby lo.Name
                    //                select new { ValueField = lo.ID, TextField = lo.Name };
                    ListItem lst3 = new ListItem("--All--", "0");
                    if (loginUserType == UserType.Institute)
                    {
                        ddlapplicationtype.SelectedValue = "2";
                        ddlapplicationtype.Enabled = false;
                        bindinst();
                    }
                    else if (loginUserType == UserType.RegionalCenter)
                    {
                        binRegCentre();
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegionalCentre, RegCentre.Distinct().OrderBy(t => t.TextField).ThenBy(r => r.ValueField), lst3);
                }
            };

            if (loginUserType != UserType.Institute)
            {
                ddlapplicationtype.SelectedValue = "0";
                ddlapplicationtype_SelectedIndexChanged(ddlapplicationtype.SelectedValue, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    #endregion
}