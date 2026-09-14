using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public partial class SemesterMaster : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;
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
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            

            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    ///Login for all  today
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeId == 10)
                        {
                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            NielitCentreIdFilter = NielitCentreId;
                            HNonAfflAfflInst.Value = "99";
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (NielitCentrelinkedToCentreId != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                                //txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                ///Login for all  todayyy
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();

                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                //txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();

                            }
                        }
                        else if (UserTypeId == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                //txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }
                        else if (UserTypeId == 4)
                        {
                            var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                //txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();
                        ShowEditMode();
                    }
                    else
                    {
                        BindCourse();
                        BindCourseForFilter();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {                          
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Details", "HO/SemesterMaster.aspx?Id=" + Request.QueryString["Id"].ToString() + ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Details", "HO/SemesterMaster.aspx", ""));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
                  
    }

    protected void BindEditNewModeData()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                int id = -1;
                int courseid = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    id = int.Parse(Request.QueryString["Key"].ToString());
                }
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    courseid = int.Parse(Request.QueryString["CourseId"].ToString());
                }
                ListItem lst = new ListItem("--Select One--", "0");
                using (DataTable dt = GetSemesterMasterDataForUpdate(id))    //get course for course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlCourse.DataSource = dt;
                        ddlCourse.DataTextField = "Name";
                        ddlCourse.DataValueField = "Id";
                        ddlCourse.DataBind();
                        ddlCourse.Items.Insert(0, new ListItem(dt.Rows[0]["Name"].ToString(), dt.Rows[0]["Id"].ToString()));
                        ddlbatchSession.Items.Insert(0, new ListItem(dt.Rows[0]["BatchName"].ToString(), dt.Rows[0]["Id"].ToString()));
                        txtSems.Text = dt.Rows[0]["NoOfSems"].ToString();

                    }
                }

                ListItem lstLearn = new ListItem("--Select One--", "0");


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindCourse()
    {
        try
        {           
            using (NIELITMISContext context = new NIELITMISContext())
            {             
                using (DataTable dt = GetCoursesForSemesterMaster())       //bind course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlCourse.DataSource = dt;
                        ddlCourse.DataTextField = "Name";
                        ddlCourse.DataValueField = "ID";
                        ddlCourse.DataBind();
                        ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));                      
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindCourseForFilter()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                using (DataTable dt = GetCoursesForSemesterMasterFilter())       //bind course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {                       
                        ddlCourseName.DataSource = dt;
                        ddlCourseName.DataTextField = "Name";
                        ddlCourseName.DataValueField = "ID";
                        ddlCourseName.DataBind();
                        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidForm()
    {
        try
        {
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
      
        Int64 Courseid = 0;

        ddlbatchSession.ClearSelection();
        ddlbatchSession.Items.Clear();

        try
        {
           
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");               
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == nielitcentreid && s.IsSemBased==true
                                            && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchSession, Batch, lst);                       
                    }                               
        }
        catch (Exception ex)
        {
            throw ex;
        }   
    }

    //delete
    protected void fillCourseWithNielitCourse()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            if (courseid != 0)
            {
                lblerrorddlcouse.Text = "";
                Int32 ddlsubcentreId = 507; //Convert.ToInt32(ddlSubcentreName.SelectedValue);
                if (ddlsubcentreId == 0)
                {
                    Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        EConnectContext context1 = new EConnectContext();
                        int courseIdExists = (from b in context.NielitCentreBatchs
                                              where b.centreID == nielitcentreid && b.CourseDurationID == courseid
                                              select b).Count();
                        if (courseIdExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.centreID == nielitcentreid && p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCourse = (from p in context1.Courses
                                                where p.ID == courseid
                                                select new
                                                {
                                                    CourseName = p.Name,
                                                    CourseCode = p.Code
                                                }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string batchname = nielitCentreBatch.BatchName;
                            string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                            Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                            string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();

                            //txtName.Text = batchNameFormate;
                            // txtBatchCode.Text = batchcodeFormate;
                        }
                        else
                        {
                            int BatchNameExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid //&& b.centreID==nielitcentreid
                                                   select b).Count();
                            if (BatchNameExists == 0)
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                //txtName.Text = batchNameFormate;
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                //txtName.Text = batchNameFormate;
                            }

                        }// Batch name autro fill end code

                        int BatchCodeExists = (from b in context.NielitCentreBatchs
                                               where b.CourseDurationID == courseid
                                               select b).Count();
                        if (BatchCodeExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            //string batchname = nielitCentreBatch.BatchName;
                            string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);

                            Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                            string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                            //txtName.Text = batchNameFormate;
                            //txtBatchCode.Text = batchCodeFormate;

                        }
                        else
                        {
                            var nielitCourse = (from p in context1.Courses
                                                where p.ID == courseid
                                                select new
                                                {
                                                    CourseName = p.Name,
                                                    CourseCode = p.Code
                                                }).FirstOrDefault();
                            string batchCodeFormate = nielitCourse.CourseCode.ToString() + "/" + 1;
                            //txtName.Text = batchNameFormate;
                            ///txtBatchCode.Text = batchCodeFormate;
                        }
                    }
                }
                else
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        EConnectContext context1 = new EConnectContext();
                        int courseIdExists = (from b in context.NielitCentreBatchs
                                              where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                              select b).Count();
                        if (courseIdExists > 0)//Exists subcentreid and courseid                        
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.subCentreID == ddlsubcentreId && p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string batchname = nielitCentreBatch.BatchName;
                            string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                            Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                            string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();

                            // txtName.Text = batchNameFormate;
                            // txtBatchCode.Text = batchcodeFormate;
                        }
                        else
                        {
                            int BatchNameExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchNameExists > 0)
                            {
                                int BatchNameAndSubCentreIDExists = (from b in context.NielitCentreBatchs
                                                                     where b.centreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                                     select b).Count();
                                if (BatchNameAndSubCentreIDExists == 0)
                                {
                                    var nielitCentreCourse = (from p in context1.Courses
                                                              where p.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code
                                                              }).FirstOrDefault();

                                    string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    //txtName.Text = batchNameFormate;
                                }
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                //txtName.Text = batchNameFormate;
                            }
                        }
                        int BatchCodeExists = (from b in context.NielitCentreBatchs
                                               where b.CourseDurationID == courseid
                                               select b).Count();
                        if (BatchCodeExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);
                            Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                            string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                            // txtBatchCode.Text = batchCodeFormate;
                        }
                        else
                        {
                            var nielitCourse = (from p in context1.Courses
                                                where p.ID == courseid
                                                select new
                                                {
                                                    CourseName = p.Name,
                                                    CourseCode = p.Code
                                                }).FirstOrDefault();
                            string batchCodeFormate = nielitCourse.CourseCode.ToString() + "/" + 1;
                            //txtBatchCode.Text = batchCodeFormate;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //txtName.Text = "";
            //txtBatchCode.Text = "";
            ShowAlert(ex.Message, true);
        }
    }
    // Deep Modified function on 17 Aug 2020    delete
    protected void fillCourseWithDuration()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 CourseType = courseid.ToString().Length;
            if (CourseType > 3)
            {
                if (courseid != 0)
                {
                    lblerrorddlcouse.Text = "";
                    Int32 ddlsubcentreId = 506;// Convert.ToInt32(ddlSubcentreName.SelectedValue);
                    if (ddlsubcentreId == 0)
                    {
                        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {

                            int courseIdExists = (from b in context.NielitCentreBatchs
                                                  where b.centreID == nielitcentreid && b.CourseDurationID == courseid
                                                  select b).Count();
                            if (courseIdExists > 0)
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.centreID == nielitcentreid && p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID

                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                string batchname = nielitCentreBatch.BatchName;
                                string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                                Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                                //string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();
                                string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + batchNameSrNum.ToString();
                                // txtName.Text = batchNameFormate;
                                // txtBatchCode.Text = batchcodeFormate;
                            }
                            else
                            {
                                int BatchNameExists = (from b in context.NielitCentreBatchs
                                                       where b.CourseDurationID == courseid //&& b.centreID==nielitcentreid
                                                       select b).Count();
                                if (BatchNameExists == 0)
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                    // txtName.Text = batchNameFormate;
                                }
                                else
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                    //txtName.Text = batchNameFormate;
                                }

                            }// Batch name autro fill end code

                            int BatchCodeExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchCodeExists > 0)
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                //string batchname = nielitCentreBatch.BatchName;
                                string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);

                                Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                                //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + batchCodeSrNum.ToString();
                                //txtName.Text = batchNameFormate;
                                //txtBatchCode.Text = batchCodeFormate;

                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + 1;
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                //txtName.Text = batchNameFormate;
                                // txtBatchCode.Text = batchCodeFormate;
                            }
                        }
                    }
                    else
                    {
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            int courseIdExists = (from b in context.NielitCentreBatchs
                                                  where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                  select b).Count();
                            if (courseIdExists > 0)//Exists subcentreid and courseid                        
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.subCentreID == ddlsubcentreId && p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                string batchname = nielitCentreBatch.BatchName;
                                string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                                Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                                //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();
                                string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + batchNameSrNum.ToString();
                                //txtName.Text = batchNameFormate;
                                // txtBatchCode.Text = batchcodeFormate;
                            }
                            else
                            {
                                int BatchNameExists = (from b in context.NielitCentreBatchs
                                                       where b.CourseDurationID == courseid
                                                       select b).Count();
                                if (BatchNameExists > 0)
                                {
                                    int BatchNameAndSubCentreIDExists = (from b in context.NielitCentreBatchs
                                                                         where b.centreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                                         select b).Count();
                                    if (BatchNameAndSubCentreIDExists == 0)
                                    {
                                        var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                                  join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                                  where k.ID == courseid
                                                                  select new
                                                                  {
                                                                      CourseName = p.Name,
                                                                      CourseCode = p.Code,
                                                                      CourseDurationID = k.ID
                                                                  }).FirstOrDefault();

                                        //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                        string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                        // txtName.Text = batchNameFormate;
                                    }
                                }
                                else
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                    // txtName.Text = batchNameFormate;
                                }
                            }
                            int BatchCodeExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchCodeExists > 0)
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);
                                Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                                //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + batchCodeSrNum.ToString();
                                //txtBatchCode.Text = batchCodeFormate;
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                // string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + 1;
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                //txtBatchCode.Text = batchCodeFormate;
                            }
                        }
                    }
                }
            }

            else if (CourseType <= 3)
            {
                fillCourseWithNielitCourse();
            }
            else
            {
                ShowAlert("Please select one of  the Course.");
                //txtName.Text = "";
                //txtBatchCode.Text = "";
                lblerrorddlcouse.Text = "Please select one of  the Course.";
            }
        }
        catch (Exception ex)
        {
            //txtName.Text = "";
            //txtBatchCode.Text = "";
            ShowAlert(ex.Message, true);
        }
    }

    protected void FillBatchNameAndBatchCode(Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };
            }
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
            lblHeading.Text = "Semester Details";
            tblNavLinks.Visible = true;

            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                ddlCourse.Enabled = false;
                ddlbatchSession.Enabled = false;

            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = true;
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();

                Int64 CourseName = 0;
                Int32 batchname = 0;
                Int32 NielitCentreIdRefAfflorNonAffl = 0, subcenteridLoginRef = 0;
                if (ddlCourseName.SelectedValue != "0")
                    CourseName = Convert.ToInt32(ddlCourseName.SelectedValue);
                if (ddlbatchname.SelectedValue != "0")
                    batchname = Convert.ToInt32(ddlbatchname.SelectedValue);
               

                User objUser;
                using (EConnectContext context1 = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();

                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    if (UserTypeId == 11)
                    {
                        var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            //txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            NielitCentreIdFilter = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    else if (UserTypeId == 4)
                    {
                        var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            //txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            NielitCentreIdFilter = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    if (UserTypeId == 10)
                    {
                        var intituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentreIdRefAfflorNonAffl = NielitCentreId;
                        NielitCentreIdFilter = NielitCentreId;
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            //txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            NielitCentreIdRefAfflorNonAffl = NelitCentreLinkId;
                            NielitCentreIdFilter = NielitCentreId;
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            //txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            NIELITCentreId.Value = NelitCentreLinkId.ToString();
                            NielitCentreIdRefAfflorNonAffl = NelitCentreLinkId;
                            NielitCentreIdFilter = NielitCentreId;
                        }
                    }
                }

                using (DataTable dt = GetSemesterMasterDetailsForGrid())
                {
                    if (dt.Rows.Count > 0)
                    {
                        var SemesterDetails = (from p in dt.AsEnumerable()
                                               select new
                                               {
                                                   Id = p.Field<int>("Id"),
                                                   CourseId = p.Field<Int64>("CourseId"),
                                                   BatchId = p.Field<Int64>("BatchId"),
                                                   NoOfSems = p.Field<int>("NoOfSems"),
                                                   CourseName = p.Field<string>("CourseName"),
                                                   BatchCode = p.Field<string>("BatchCode"),
                                                   BatchName = p.Field<string>("BatchName"),
                                               });

                        if (!String.IsNullOrEmpty(searchString))
                        {
                            SemesterDetails = SemesterDetails.Where(s => s.BatchCode.ToUpper().Contains(searchString));
                        }
                        if (CourseName != 0 && batchname != 0)
                        {
                            SemesterDetails = SemesterDetails.Where(s => s.CourseId == CourseName && s.BatchId == batchname);
                        }
                        else if (CourseName != 0)
                        {
                            SemesterDetails = SemesterDetails.Where(s => s.CourseId == CourseName);
                        }
                        else if (batchname != 0)
                        {
                            SemesterDetails = SemesterDetails.Where(s => s.BatchId == batchname);
                        }
                      

                        if (!string.IsNullOrEmpty(sortOrder))
                        {                   
                        switch (sortField)
                        {
                            case "Id":
                                if (sortOrder == "DESC")
                                    SemesterDetails = SemesterDetails.OrderByDescending(s => s.Id);
                                else
                                    SemesterDetails = SemesterDetails.OrderBy(s => s.Id);
                                break;
                            case "CourseId":
                                if (sortOrder == "DESC")
                                    SemesterDetails = SemesterDetails.OrderByDescending(s => s.CourseId);
                                else
                                    SemesterDetails = SemesterDetails.OrderBy(s => s.CourseId);
                                break;
                            case "BatchId":
                                if (sortOrder == "DESC")
                                    SemesterDetails = SemesterDetails.OrderByDescending(s => s.BatchId);
                                else
                                    SemesterDetails = SemesterDetails.OrderBy(s => s.BatchId);
                                break;
                            case "NoOfSems":
                                if (sortOrder == "DESC")
                                    SemesterDetails = SemesterDetails.OrderByDescending(s => s.NoOfSems);
                                else
                                    SemesterDetails = SemesterDetails.OrderBy(s => s.NoOfSems);
                                break;

                            default:
                                SemesterDetails = SemesterDetails.OrderBy(s => s.CourseId);
                                break;
                        }

                        }
                   
                        PagingBar1.Bind(SemesterDetails, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        // gvMain.Columns[5].Visible = false;
                        //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        //{
                        //    gvMain.Columns[7].Visible = false;
                        //}
                    }
                }
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
          /*
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }

          */
            BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //this.Rview.Visible = false;
            //this.Rview1.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Semester Master";
            //Updating Breadcrumb         
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Semester Master", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("SemesterMaster.aspx?ID=" + Request.QueryString["Id"].ToString()), true);
            }
            else
            {
                Response.Redirect("SemesterMaster.aspx", true);
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
            lblerrorddlcouse.Text = "";
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                EConnectContext context1 = new EConnectContext();
                User objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                Int32 semno = Convert.ToInt32(txtSems.Text);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentreBatch objBatchCentre;
                    string sBatchCode = "";// txtBatchCode.Text;
                    if (ddlCourse.SelectedValue == "0")
                    {
                        lblerrorddlcouse.Visible = true;
                        lblerrorddlcouse.Text = "Please Select Course";
                        ddlCourse.Focus();
                        return;

                    }
                    if (ddlbatchSession.SelectedValue == "0")
                    {
                        lblerrorddlbatch.Visible = true;
                        lblerrorddlbatch.Text = "Please Select Batch";
                        ddlbatchSession.Focus();
                        return;

                    }
                    if (semno == 0 || semno <= 0)
                    {
                        trsem.Visible = true;
                        lblerrorsem.Visible = true;
                        lblerrorsem.Text = "Semester No. can not be zero or negative";
                        txtSems.Focus();
                        return;
                    }
                    else
                    {
                        trsem.Visible = false;
                        lblerrorsem.Visible = false;
                    }
                    //Insert if
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                               
                        objBatchCentre = new NielitCentreBatch();
                        HNANFL.Value = "O";
                        if (context.NielitCentreBatchs.Where(s => s.BatchCode == sBatchCode).Count() != 0)
                        {
                            fillCourseWithDuration();
                        }

                        if (UserTypeId == 4) //AffInstitutes
                        {
                            var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                //txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            HNANFL.Value = "Y";
                        }
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            //txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            objBatchCentre.centreID = NelitCentreLinkId;
                            // HNANFL.Value = "O";
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();                           
                        }
                        // objBatchCentre.centreID = NelitCentreLinkId;
                        objBatchCentre.CourseDurationID = Convert.ToInt32(ddlCourse.SelectedValue);

                        objBatchCentre.batchSession = Convert.ToInt32(ddlbatchSession.SelectedValue);

                        objBatchCentre.enterDate = DateTime.Now;
                        objBatchCentre.enterBy = Convert.ToInt32(Session["UserID"]);
                        objBatchCentre.IsActive = true;
                        objBatchCentre.IsVerified = false;
                        objBatchCentre.Show_On_Web = true;
                        context.NielitCentreBatchs.Add(objBatchCentre);
                        //Save Here
                        int val = SaveRecord();
                        //context.SaveChanges();
                        if (val == 0)
                        {
                            strMessage = "New record saved.";
                        }
                        else
                        {
                            strMessage = "This record already exists.";
                        }
                    }
                    else
                    {
                        int var = RecordUpdate();
                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                }
                Response.Redirect("SemesterMaster.aspx?msg=" + strMessage, true);
            }

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
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
            ddlCourseName.SelectedValue = "0";
            ddlbatchname.SelectedValue = "0";
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
               /*
                                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                                {
                                    BreadCrumb1.Render();
                                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                                    return;
                                }
               */
               
                NielitCentreBatch Batchcenter = context.NielitCentreBatchs.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.NielitCentreBatchs.Remove(Batchcenter);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
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
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                if (!String.IsNullOrEmpty(Request.QueryString["BatchId"]))
                {
                    href += "&BatchId=" + Request.QueryString["BatchId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = hl.NavigateUrl;

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
        Int32 loginUserNo = 0, UserTypeId = 0, NielitCentreIdSearch = 0, NielitCentrelinkedToCentreId = 0;
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                if (UserTypeId == 10)
                {
                    var intituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                    Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitCentreIdSearch = NielitCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        NielitCentreIdSearch = NielitCentreId;
                    }
                    else
                    {
                        NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        NielitCentreIdSearch = NielitCentreId;
                    }
                }
            }
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            EConnectContext context2 = new EConnectContext();
        

           
            var Batchcode = from s in context.NielitCentreBatchs
                           join i in context.SemesterMaster
                              on s.ID equals i.BatchId
                            where s.centreID == NielitCentreIdSearch //&& s.IsVerified == true
                            select new { Name = s.BatchCode };

            if (!String.IsNullOrEmpty(searchString))
            {
                Batchcode = Batchcode.Where(s => s.Name.ToUpper().Contains(searchString));
            }
           // BatchName = BatchName.Union(Batchcode).Take(count);

            foreach (var c in Batchcode)
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
        Response.Redirect("SemesterMaster.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("SemesterMaster.aspx", true);
    }

    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

        Int64 Courseid = 0;

        ddlbatchname.ClearSelection();
        ddlbatchname.Items.Clear();

        try
        {

            Courseid = Convert.ToInt64(ddlCourseName.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");



                var Batch = from s in context.NielitCentreBatchs
                            join i in context.SemesterMaster
                                on s.ID equals i.BatchId
                            where s.IsVerified == true && s.centreID == nielitcentreid && s.IsSemBased==true
                                    && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, Batch, lst);

            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetCoursesForSemesterMaster()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCoursesForSemester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@centerid", SqlDbType.Int));
                    cmd.Parameters["@centerid"].Value = NielitCentreIdFilter;
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

    public DataTable GetCoursesForSemesterMasterFilter()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCoursesForSemesterfilter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@centerid", SqlDbType.Int));
                    cmd.Parameters["@centerid"].Value = NielitCentreIdFilter;
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
    public int SaveRecord()
    {              
            int rVal = -1;
            Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            DataTable myDt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SaveSemesterInfo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CourseId", SqlDbType.BigInt);
                        cmd.Parameters["@CourseId"].Value = int.Parse(ddlCourse.SelectedValue);// Convert.ToInt64(Session["EntityID"]);
                        cmd.Parameters.Add("@BatchId", SqlDbType.BigInt);
                        cmd.Parameters["@BatchId"].Value = int.Parse(ddlbatchSession.SelectedValue);
                        cmd.Parameters.Add("@NoOfSems", SqlDbType.BigInt);
                        cmd.Parameters["@NoOfSems"].Value = int.Parse(txtSems.Text);

                        cmd.Parameters.Add("@enterBy", SqlDbType.BigInt);
                        cmd.Parameters["@enterBy"].Value = Convert.ToInt32(Session["UserID"]);

                        cmd.Parameters.Add("@enterDate", SqlDbType.Date);
                        cmd.Parameters["@enterDate"].Value = DateTime.Now;


                        cmd.Parameters.Add("@Exists", SqlDbType.Int);
                        cmd.Parameters["@Exists"].Direction = ParameterDirection.ReturnValue;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        rVal = Convert.ToInt32(cmd.Parameters["@Exists"].Value);

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
            return rVal;
        
    }

    public int RecordUpdate()
    {
        int rVal = -1;
        Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
        Int64 NoOfSem = Convert.ToInt64(txtSems.Text);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetOneSemesterDetailsForUpdate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.BigInt);
                    cmd.Parameters["@Id"].Value = KeyID;// Convert.ToInt64(Session["EntityID"]);
                    cmd.Parameters.Add("@NoOfSems", SqlDbType.BigInt);
                    cmd.Parameters["@NoOfSems"].Value = int.Parse(txtSems.Text);

                    cmd.Parameters.Add("@enterBy", SqlDbType.BigInt);
                    cmd.Parameters["@enterBy"].Value = Convert.ToInt32(Session["UserID"]);

                    cmd.Parameters.Add("@enterDate", SqlDbType.Date);
                    cmd.Parameters["@enterDate"].Value = DateTime.Now;

                    con.Open();
                    cmd.ExecuteNonQuery();

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
        return rVal;

    }

    public DataTable GetSemesterMasterDetailsForGrid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterMasterDetailsForGrid", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                  cmd.Parameters["@pCentreID"].Value =Convert.ToInt64 (Session["EntityID"]);
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


    public DataTable GetSemesterMasterDataForUpdate(int id)
    {
        int courseid = 0;
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
          courseid = int.Parse(Request.QueryString["CourseId"].ToString());
        }
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterMasterDataForUpdate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                    cmd.Parameters["@Id"].Value = id;
                    cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.Int));
                    cmd.Parameters["@CourseId"].Value = courseid;
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                    cmd.Parameters["@centreId"].Value = nielitcentreid;
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

}