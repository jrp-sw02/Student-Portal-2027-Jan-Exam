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
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data.Objects;
using EConnect;


public partial class Admin_StudentResultDetails : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;

    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserRefNumber = 0;
    Int32 NielitCentreId = 0;
    Int32 UserTypeid = 0;
    Int32 lnkID = 0;
    Int64 subcentreid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            //if (IsSessionAlive() == false)
            // Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeid = Convert.ToInt32(Session["UserTypeId"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                User objUsers;
                using (EConnectContext context = new EConnectContext())
                {
                    objUsers = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeid == 10)
                        {
                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            NielitCentreIdFilter = NielitCentreId;
                            HNonAfflAfflInst.Value = "99";
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (NielitCentrelinkedToCentreId != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                ///Login for all  todayyy
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();

                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();

                            }
                        }
                        else if (UserTypeid == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }
                        else if (UserTypeid == 4)
                        {
                            var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }

                    }

                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        //  BindEditNewModeData();
                    }
                    else
                    {
                        BindCourse();
                        BindListData();
                        FillResult();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        //    BindGridView();
                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Result Details", "Admin/StudentResultDetails.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Result Details", "Admin/StudentResultDetails.aspx", ""));
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

    protected void BindCourse()
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

        try
        {
            // Int32 subid = Convert.ToInt32(ddlSubjects.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlCourse.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var semestercourse = from p in context.SemesterMaster
                                     join c in context.NielitCourseDurations on p.CourseId equals c.ID
                                     join s in context.NielitCentreCourses on c.courseID equals s.ID
                                     join b in context.NielitCentreBatchs on c.ID equals b.CourseDurationID
                                     where b.centreID == nielitcentreid && s.CourseCategoryID == 101
                                     select new { ValueField = c.ID, TextField = s.Name + " (" + s.Code + ")" + " (" + c.courseDurationDays + "Days" + ")" + " (" + c.courseDurationHrs + "Hours" + ")" };
                semestercourse = semestercourse.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, semestercourse.Distinct(), lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void BindListData()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                using (DataTable dt = GetCoursesForSemesterMaster())       //bind course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlCourseName.DataSource = dt;
                        ddlCourseName.DataTextField = "Name";
                        ddlCourseName.DataValueField = "ID";
                        ddlCourseName.DataBind();
                        ddlCourseName.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

        Int64 Course = 0;

        ddlbatchSession.ClearSelection();
        ddlbatchSession.Items.Clear();
        ddlsemester.ClearSelection();
        ddlbatchSession.Items.Clear();
        ddlStudentNumber.ClearSelection();
        gvMain.Visible = false;

        try
        {

            Course = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true && s.centreID == nielitcentreid
                                    && s.CourseDurationID == Course // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
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

    public DataTable GetCoursesForSemesterMaster()
    {
        subcentreid = Convert.ToInt64(Session["EntityID"]);
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
                    cmd.Parameters["@centerid"].Value = subcentreid;
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
    public DataTable GetBatchesForSemesterMaster()
    {
        subcentreid = Convert.ToInt64(Session["EntityID"]);

        Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchesForSemesterMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@courseDurationId", SqlDbType.Int));
                    cmd.Parameters["@courseDurationId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.Int));
                    cmd.Parameters["@centreId"].Value = subcentreid;
                    cmd.Parameters.Add(new SqlParameter("@centreType", SqlDbType.Char));

                    cmd.Parameters["@centreType"].Value = 'm'; //need to apply condition for main or sub centre


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

    protected void ddlbatchSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem lst = new ListItem("--All--", "0");
        using (NIELITMISContext context = new NIELITMISContext())
        {

            try
            {

                FillStudentId();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlsemester.ClearSelection();

        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
                Int64 StudentId = Convert.ToInt64(ddlStudentNumber.SelectedValue);
                Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                if (coursenameid != 0)
                {
                    var Batch = from s in context.NielitCentreStudent
                                where s.InstituteID == nielitcentreid && s.CourseID == coursenameid && s.batch_ID == batchid
                                        && s.ID == StudentId
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.semesterid };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlsemester, Batch, lst);
                }

            }
            FillStudentName();
        }


        catch (Exception ex)
        {
            throw ex;
        }
        // }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubjectName.ClearSelection();
        try
        {
            Int32 Batchid = Convert.ToInt32(ddlbatchSession.SelectedValue);
            Int32 Courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int64 semno = Convert.ToInt64(ddlsemester.SelectedItem.Text);

            using (DataTable dt = GetSubject())
            {
                if (dt.Rows.Count > 0)
                {
                    ddlSubjectName.DataSource = dt;
                    ddlSubjectName.DataTextField = "SubjectName";
                    ddlSubjectName.DataValueField = "Id";
                    ddlSubjectName.DataBind();
                }
                else
                {
                    ddlSubjectName.Items.Clear();
                    ddlSubjectName.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSemester()
    {
        Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
        Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterforFormalCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                    cmd.Parameters["@courseId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.Int));
                    cmd.Parameters["@BatchId"].Value = batchid;


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
    public DataTable GetSubject()
    {

        Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
        Int64 semno = Convert.ToInt64(ddlsemester.SelectedItem.Text);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getSubjectNameSubjectWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                    cmd.Parameters["@BatchId"].Value = batchid;
                    cmd.Parameters.Add(new SqlParameter("@semno", SqlDbType.BigInt));
                    cmd.Parameters["@semno"].Value = semno;
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

    public DataTable GetSemesterfilter()
    {
        Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
        Int64 batchid = Convert.ToInt64(ddlbatchname.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterforFormalCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                    cmd.Parameters["@courseId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.Int));
                    cmd.Parameters["@BatchId"].Value = batchid;

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
    protected void FillStudentName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
                Int64 StudentId = Convert.ToInt64(ddlStudentNumber.SelectedValue);
                Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                var applicationcheck = (from s in context.NielitCentreStudent
                                        where s.InstituteID == nielitcentreid && s.CourseID == coursenameid && s.batch_ID == batchid
                                                && s.ID == StudentId
                                        select new
                                        {
                                            ID = s.ID,
                                            Name = s.Name

                                        }).FirstOrDefault();

                if (applicationcheck != null)
                {

                    txtStudent.Text = applicationcheck.Name.ToString();
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillStudentId()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 Batchid = Convert.ToInt32(ddlbatchSession.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlStudentNumber.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var StudentNumber = from p in context.NielitCentreStudent

                                    where p.CourseID == courseid && p.batch_ID == Batchid && p.semesterid != null
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Number };
                StudentNumber = StudentNumber.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlStudentNumber, StudentNumber.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillStudentIdFilter()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 Batchid = Convert.ToInt32(ddlbatchname.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlstudent.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var StudentFilter = from p in context.NielitCentreStudent

                                    where p.CourseID == courseid && p.batch_ID == Batchid && p.semesterid != null
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Number };
                StudentFilter = StudentFilter.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlstudent, StudentFilter.Distinct(), lst);
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillResult()
    {
        try
        {
            Int32 result = Convert.ToInt32(ddlResult.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlStudentNumber.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var resultdata = from p in context.semesterResultMaster

                                 //   orderby (p.Result)
                                 select new { ValueField = p.Id, TextField = p.Result };
                resultdata = resultdata.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlResult, resultdata.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        using (NIELITMISContext context = new NIELITMISContext())
        {
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int64 BatchIDFilter = 0, CourseIDFilter = 0, studentid = 0, subjectid = 0;


            if (ddlCourseName.SelectedValue != "0")
                CourseIDFilter = Convert.ToInt64(ddlCourseName.SelectedValue);
            if (ddlbatchname.SelectedValue != "0")
                BatchIDFilter = Convert.ToInt64(ddlbatchname.SelectedValue);

            if (ddlstudent.SelectedValue != "0")
                studentid = Convert.ToInt64(ddlstudent.SelectedValue);

            con.Open();

            using (DataTable dt = GetStudentResultFilterForGrid())
            {
                if (dt.Rows.Count > 0)
                {
                    var StudentResults = (from p in dt.AsEnumerable()
                                          select new
                                          {
                                              ID = p.Field<int>("ID"),
                                              batchid = p.Field<Int64>("batchid"),
                                              studentID = p.Field<Int64>("studentID"),
                                              SubjectID = p.Field<int>("SubjectID"),
                                              Name = p.Field<string>("Name"),
                                              father_name = p.Field<string>("father_name"),
                                              dob = p.Field<string>("dob"),
                                              CourseName = p.Field<string>("CourseName"),
                                              courseID = p.Field<Int64>("courseID"),
                                              BatchCode = p.Field<string>("BatchCode"),
                                              BatchName = p.Field<string>("BatchName"),
                                              SubjectName = p.Field<string>("SubjectName"),
                                              Number = p.Field<string>("Number"),
                                              Result = p.Field<string>("Result"),
                                              semno = p.Field<int>("semno"),
                                              enterBy = p.Field<Int64>("enterBy")
                                          });

                    if (UserTypeid != 10)
                    {
                        StudentResults = StudentResults.Where(s => s.enterBy == loginUserNo);
                    }
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        StudentResults = StudentResults.Where(s => s.BatchName.ToUpper().Contains(searchString) || s.BatchCode.ToUpper().Contains(searchString));

                    }
                    if (CourseIDFilter != 0 && BatchIDFilter != 0 && studentid != 0)
                    {
                        StudentResults = StudentResults.Where(s => s.courseID == CourseIDFilter && s.batchid == BatchIDFilter && s.studentID == studentid);
                    }
                    else if (CourseIDFilter != 0 && BatchIDFilter != 0)
                    {
                        StudentResults = StudentResults.Where(s => s.courseID == CourseIDFilter && s.batchid == BatchIDFilter);
                    }
                    else if (CourseIDFilter != 0)
                    {
                        StudentResults = StudentResults.Where(s => s.courseID == CourseIDFilter);
                    }



                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    StudentResults = StudentResults.OrderByDescending(s => s.ID);
                                else
                                    StudentResults = StudentResults.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    StudentResults = StudentResults.OrderByDescending(s => s.Name);
                                else
                                    StudentResults = StudentResults.OrderBy(s => s.Name);
                                break;
                            case "BatchName":
                                if (sortOrder == "DESC")
                                    StudentResults = StudentResults.OrderByDescending(s => s.BatchName);
                                else
                                    StudentResults = StudentResults.OrderBy(s => s.BatchName);
                                break;
                            case "CourseName":
                                if (sortOrder == "DESC")
                                    StudentResults = StudentResults.OrderByDescending(s => s.CourseName);
                                else
                                    StudentResults = StudentResults.OrderBy(s => s.CourseName);
                                break;
                            case "SubjectName":
                                if (sortOrder == "DESC")
                                    StudentResults = StudentResults.OrderByDescending(s => s.SubjectName);
                                else
                                    StudentResults = StudentResults.OrderBy(s => s.SubjectName);
                                break;
                            default:
                                StudentResults = StudentResults.OrderBy(s => s.Name);
                                break;
                        }
                    }

                    PagingBar1.Bind(StudentResults, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;
                    gvMain.Visible = true;
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                        gvMain.Visible = false;
                    }
                }
            }
        }
    }

    public DataTable GetStudentResultFilterForGrid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        subcentreid = Convert.ToInt64(Session["EntityID"]);
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentResultSearchForGrid", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = subcentreid;
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
        try
        {
            BreadCrumb1.Render();
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();

            Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
            Int64 StudentID = Convert.ToInt64(ddlStudentNumber.SelectedValue);
            Int64 semno = Convert.ToInt64(ddlsemester.SelectedItem.Text);
            Int64 subjectid = Convert.ToInt64(ddlSubjectName.SelectedValue);
            // string result = ddlResult.Text;
            using (NIELITMISContext context = new NIELITMISContext())
            {

                StudentSemesterAcademicDetails studentresult;

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    studentresult = new StudentSemesterAcademicDetails();

                    // StudentID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Value);
                    //objStudentFeePaid = context.NIELITStudentFeePaids.Find(StudentID);
                    int StudentIDExists = (from b in context.StudentSemesterAcademicDetails
                                           where b.StudentId == StudentID && b.SemSubId == subjectid
                                           select b).Count();


                    if (StudentIDExists == 0)
                    {
                        studentresult.StudentId = Convert.ToInt64(StudentID);
                        studentresult.SemSubId = Convert.ToInt64(subjectid);
                        if (ddlResult.SelectedItem.Text == "Pass")
                        {
                            studentresult.Result = "P";
                        }
                        else if (ddlResult.SelectedItem.Text == "Fail")
                        {
                            studentresult.Result = "F";
                        }
                        studentresult.enterBy = Convert.ToInt32(Session["UserID"]);
                        studentresult.enterDate = DateTime.Now;
                        context.StudentSemesterAcademicDetails.Add(studentresult);
                        context.SaveChanges();
                        strMessage = "Data Saved successfully.";
                        lblMessage.Text = "Data Saved successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        StudentSemesterAcademicDetails studentresultUpdate;
                        studentresultUpdate = new StudentSemesterAcademicDetails();
                        StudentID = Convert.ToInt64(ddlStudentNumber.SelectedValue);


                        studentresultUpdate = context.StudentSemesterAcademicDetails.Single(k => k.StudentId == StudentID && k.SemSubId == subjectid);
                        studentresult.SemSubId = Convert.ToInt64(subjectid);
                        if (ddlResult.SelectedItem.Text == "Pass")
                        {
                            studentresultUpdate.Result = "P";
                        }
                        else if (ddlResult.SelectedItem.Text == "Fail")
                        {
                            studentresultUpdate.Result = "F";
                        }
                        studentresultUpdate.enterBy = Convert.ToInt32(Session["UserID"]);
                        studentresultUpdate.enterDate = DateTime.Now;
                        StudentResultHistory(StudentID);
                        context.SaveChanges();
                        strMessage = "Data updated successfully!";
                        // lblMessage.ForeColor = System.Drawing.Color.Green;
                    }

                }
            }
            Response.Redirect("StudentResultDetails.aspx?msg=" + strMessage, true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public void StudentResultHistory(Int64 id)
    {
        try
        {
            //string constr = ConfigurationManager.ConnectionStrings["NIELITMISContextt"].ConnectionString;
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("InsertStudentResultHistory", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ApllyFilter(object sender, EventArgs e)
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
            ddlsem.SelectedValue = "0";
            ddlstudent.SelectedValue = "0";

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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentResultDetails.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentResultDetails.aspx", true);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        User objUser;
        int loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        using (EConnectContext context1 = new EConnectContext())
        {
            objUser = new EConnect.URM.User();

            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            int UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

            NIELITMISContext context = new NIELITMISContext();
            try
            {
                if (count <= 0)
                    count = 10;
                List<String> items = new List<String>();
                string searchString = prefixText.Trim().ToUpper();

                var BatchName = from s in context.NielitCentreStudent
                                join r in context.StudentSemesterAcademicDetails on s.ID equals r.StudentId
                                join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                where s.InstituteID == UserRefNumber && b.IsVerified == true
                                select new { Name = b.Name };

                if (!String.IsNullOrEmpty(searchString))
                {
                    BatchName = BatchName.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                BatchName = BatchName.OrderBy(s => s.Name);

                var Batchcode = from s in context.NielitCentreStudent
                                join r in context.StudentSemesterAcademicDetails on s.ID equals r.StudentId
                                join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                where s.InstituteID == UserRefNumber && b.IsVerified == true
                                select new { Name = b.BatchCode };

                if (!String.IsNullOrEmpty(searchString))
                {
                    Batchcode = Batchcode.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                BatchName = BatchName.Union(Batchcode).Take(count);

                foreach (var c in BatchName)
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
    }

    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
        Int64 Courseid = 0;
        ddlbatchname.ClearSelection();
        ddlbatchname.Items.Clear();
        ddlstudent.ClearSelection();
        ddlsem.ClearSelection();
        try
        {
            Courseid = Convert.ToInt64(ddlCourseName.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true && s.centreID == nielitcentreid
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

    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStudentIdFilter();
    }

    protected void ddlstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
                Int64 batchid = Convert.ToInt64(ddlbatchname.SelectedValue);
                Int64 StudentId = Convert.ToInt64(ddlstudent.SelectedValue);
                Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                if (coursenameid != 0)
                {
                    var Batch = from s in context.NielitCentreStudent
                                where s.InstituteID == nielitcentreid && s.CourseID == coursenameid && s.batch_ID == batchid
                                        && s.ID == StudentId
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.semesterid };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlsem, Batch, lst);
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

}

