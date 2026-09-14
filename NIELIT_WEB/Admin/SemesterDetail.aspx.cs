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

public partial class Admin_SemesterDetail : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;
    Int64 aflInstId = 0;
    Int64 NonaflInstId = 0;
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
            UserTypeId = Convert.ToInt32(Session["UserType"]);

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
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                RdoAffInstOrNonAffInst.SelectedValue = "2";

                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                RdoAffInstOrNonAffInst.SelectedValue = "2";

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
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(0);

                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                        }
                        else if (UserTypeId == 4)
                        {
                            var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          where s.instituteID == loginUser.UserRefNumber
                                                          select new { ID = s.ID, linkedToCentre = s.linkedToCentre, instid = s.instituteID };
                            if (intituteslinkedToCentre.Count() == 0)
                            {
                                ShowAlert("Menu is not available for the institute");
                                return;
                            }
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                            AFLINSTID.Value = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().instid).ToString();
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);

                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();
                        ShowEditMode();
                    }
                    else
                    {
                        BindListData();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Batch Details ", "Admin/SemesterDetail.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Batch Details", "Admin/SemesterDetail.aspx", ""));
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
            Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
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

                ListItem lst1 = new ListItem("--Select One--", "0");
                var Session = from s in context.SessionMass
                              orderby (s.ID)
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchSession, Session, lst1);

                //Added for learning Mode
                ListItem lstLearn = new ListItem("--Select One--", "0");
                var learnMode = from a in context.LearningModeMass
                                orderby (a.Name)
                                select new { ValueField = a.ID, TextField = a.Name + "(" + a.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLearningMode, learnMode, lstLearn);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetCoursesForSemesterMaster()
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
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
                    cmd.Parameters["@centerid"].Value = nielitcentreid;
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

    protected void BindListData()
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

    public DataTable GetCoursesForSemesterMasterFilter()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCoursesForBatchfilter", con))
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


    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

            Int64 Course = 0;

            Course = Convert.ToInt64(ddlCourse.SelectedValue);



            ddlBatchCode.ClearSelection();
            ddlBatchCode.Items.Clear();
            txtName.Text = "";
            ddlSemNo.ClearSelection();
            ddlSemNo.Items.Clear();


            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var BatchCode = from s in context.NielitCentreBatchs
                                where s.IsVerified == true && s.centreID == nielitcentreid && s.IsSemBased == true
                                        && s.CourseDurationID == Course // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.BatchCode };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchCode, BatchCode, lst);


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSemester()
    {
        Int64 batchid = Convert.ToInt64(ddlBatchCode.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@batchid", SqlDbType.BigInt);
                    cmd.Parameters["@batchid"].Value = batchid;
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

    protected void ddlBatchCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillBatchName();

        try
        {
            ddlSemNo.Items.Clear();

            using (NIELITMISContext context = new NIELITMISContext())
            {
                // ListItem lst = new ListItem("--All--", "0");
                using (DataTable dt = GetSemester())
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i = Convert.ToInt32(dt.Rows[0]["NoOfSems"]);
                        int n = i;
                        for (i = 0; i <= n; i++)
                        {
                            if (i == 0)
                            {
                                ddlSemNo.Items.Add(new ListItem("Select", "0"));
                            }
                            else
                            {
                                ddlSemNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                ddlSemNo.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillBatchName()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 Batchid = Convert.ToInt32(ddlBatchCode.SelectedValue);


            using (NIELITMISContext context = new NIELITMISContext())
            {
                txtName.Text = "";
                var BatchName = (from p in context.NielitCentreBatchs
                                 where p.CourseDurationID == courseid && p.ID == Batchid
                                 select new
                                 {
                                     ID = p.ID,
                                     Name = p.Name,
                                     //
                                 }).FirstOrDefault();
                txtName.Text = BatchName.Name.ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void txtstartDate_TextChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
        using (NIELITMISContext context = new NIELITMISContext())
        {
            int CourseIdExistinDurationList = (from b in context.NielitCourseDurations
                                               where b.ID == courseid
                                               select b).Count();
            if (CourseIdExistinDurationList > 0)
            {
                var courseRunningDays = (from p in context.NielitCourseDurations
                                         where p.ID == courseid
                                         select new
                                         {
                                             CourseRunningDays = p.courseDurationDays,
                                             CourseCode = p.courseID
                                         }).FirstOrDefault();
                Int32 coursDays = courseRunningDays.CourseRunningDays;
                DateTime EndDate = Convert.ToDateTime(txtstartDate.Text);
                EndDate = EndDate.AddDays(-1);
                txtendDate.Text = EndDate.AddDays(coursDays).ToString("dd-MMM-yyyy"); ;
            }
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
            lblHeading.Text = "Semester Detail";
            tblNavLinks.Visible = false;
            lblShowOnWeb.Visible = true;
            lblIsVerifieds.Visible = true;
            lblIsActive.Visible = true;
            ddlVerifieds.Visible = true;
            ddlShowOnWeb.Visible = true;
            ddlActive.Visible = true;
            ddlShowOnWeb.Enabled = false;
            ddlActive.Enabled = false;
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                if (context.SemesterDetail.Any(s => (s.IsVerified == true) && s.ID == Id))
                {
                    //Added for learningMode
                    ddlLearningMode.Enabled = false;
                    txtRemarks.Enabled = false;
                    //
                    ddlCourse.Enabled = false;
                    ddlBatchCode.Enabled = false;
                    ddlSemNo.Enabled = false;
                    ddlbatchSession.Enabled = false;
                    ddlActive.Enabled = false;
                    ddlVerifieds.Enabled = false;
                    ddlShowOnWeb.Enabled = false;
                    txtName.Enabled = false;
                    txtstartDate.Enabled = false;
                    txtendDate.Enabled = false;
                    txtbatchDurationPracticalHours.Enabled = false;
                    txtbatchDurationTheoryHours.Enabled = false;
                    txtFName.Enabled = false;
                    txtFEmail.Enabled = false;
                    ddlShowOnWeb.Enabled = false;
                    ddlActive.Enabled = false;
                    if (UserTypeId == 10)
                    {
                        this.Rview.Visible = true;
                        this.Rview1.Visible = true;
                    }

                    var Batchcentre = (from p in context.SemesterDetail
                                       join d in context.NielitCentreBatchs on p.batchID equals d.ID
                                       where p.ID == Id
                                       select new
                                       {
                                           ID = p.ID,
                                           BatchName = d.Name,
                                           BatchId = p.batchID,
                                           BatchCode = d.BatchCode,
                                           learningModeID = p.learningModeID,
                                           remarks = p.Remarks,
                                           CourseID = p.CourseDurationID,
                                           CentreId = p.centreID,
                                           SubCentreId = d.subCentreID,                                          
                                           startDate = p.semStartDate,
                                           endDate = p.semEndDate,                                          
                                           enterBy = p.enterBy,
                                           BatchSession = p.semSession,
                                           SemNo = p.semesterNo,
                                           batchDurationPracticalHours = p.batchDurationPracticalHours,
                                           batchDurationTheoryHours= p.batchDurationTheoryHours,
                                           facultyName=p.facultyName,
                                           facultyEmail=p.facultyEmail,
                                           IsActive = p.IsActive ,
                                           IsVerified = p.IsVerified ,
                                           Show_On_Web= p.Show_On_Web
                                       }).FirstOrDefault();
                    NIELITMISContext context2 = new NIELITMISContext();

                    //Added for learning Mode
                    
                    ddlCourse.SelectedValue = Batchcentre.CourseID.ToString();
                    ddlCourse.SelectedValue = Batchcentre.CourseID != null && Batchcentre.CourseID != 0 ? Batchcentre.CourseID.ToString() : "0";
                    int id3 = Convert.ToInt32(ddlCourse.SelectedValue);
                   ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                   if (Batchcentre.BatchId != null)
                       ddlBatchCode.SelectedValue = Batchcentre.BatchId.ToString();
                   ddlBatchCode_SelectedIndexChanged(ddlBatchCode, EventArgs.Empty);
                   if (Batchcentre.SemNo != null)
                       ddlSemNo.SelectedValue = Batchcentre.SemNo.ToString();
                  
                    ddlLearningMode.SelectedValue = Batchcentre.learningModeID.ToString();
                    txtRemarks.Text = Batchcentre.remarks.ToString();
                    ddlCourse.SelectedValue = Batchcentre.CourseID.ToString();
                    txtName.Text = Batchcentre.BatchName.ToString();
                    txtstartDate.Text = Batchcentre.startDate.ToString();
                    txtendDate.Text = Batchcentre.endDate.ToString();
                    ddlbatchSession.SelectedValue = Batchcentre.BatchSession.ToString();

                    if (Batchcentre.IsActive == true)
                    {
                        ddlActive.SelectedValue = "1";
                    }
                    else
                    {
                        ddlActive.SelectedValue = "0";
                    }

                    if (Batchcentre.IsVerified == true)
                    {
                        ddlVerifieds.SelectedValue = "1";
                    }
                    else
                    {
                        ddlVerifieds.SelectedValue = "0";
                    }

                    if (Batchcentre.Show_On_Web == true)
                    {
                        ddlShowOnWeb.SelectedValue = "1";
                    }
                    else
                    {
                        ddlShowOnWeb.SelectedValue = "0";
                    }
                    txtbatchDurationPracticalHours.Text = Batchcentre.batchDurationPracticalHours.ToString();
                    txtbatchDurationTheoryHours.Text = Batchcentre.batchDurationTheoryHours.ToString();
                    txtFName.Text = Batchcentre.facultyName.ToString();
                    txtFEmail.Text = Batchcentre.facultyEmail.ToString();                   
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnBack.Visible = true;
                    throw new Exception("This Semester Detail is Already Verifed.");
                }
                else
                {
                    //Added for learning Mode
                    ddlBatchCode.Enabled = false;
                    ddlLearningMode.Enabled = true;
                    txtRemarks.Enabled = true;
                    ddlCourse.Enabled = false;
                    ddlbatchSession.Enabled = true;
                    ddlVerifieds.Enabled = true;
                    ddlSemNo.Enabled = false;
                    txtName.Enabled = false;
                    txtstartDate.Enabled = true;
                    txtendDate.Enabled = true;
                    txtbatchDurationPracticalHours.Enabled = true;
                    txtbatchDurationTheoryHours.Enabled = true;
                    txtFName.Enabled = true;
                    txtFEmail.Enabled = true;
                    this.Rview.Visible = false;
                    this.Rview1.Visible = false;
                    var Batchcentre = (from p in context.SemesterDetail
                                       join d in context.NielitCentreBatchs on p.batchID equals d.ID
                                       where p.ID == Id
                                       select new
                                       {
                                           ID = p.ID,
                                           BatchName = d.Name,
                                           BatchCode = d.BatchCode,
                                           BatchId= p.batchID,
                                           learningModeID = p.learningModeID,
                                           remarks = p.Remarks,
                                           CourseID = p.CourseDurationID,
                                           CentreId = p.centreID,
                                           SubCentreId = d.subCentreID,
                                           BatchDurationTheoryHours = p.batchDurationTheoryHours,
                                           BatchDurationPracticalHours = p.batchDurationPracticalHours,
                                           startDate = p.semStartDate,
                                           endDate = p.semEndDate,
                                           FacultyName = p.facultyName,
                                           FacultyEmail = p.facultyEmail,
                                           enterBy = p.enterBy,
                                           BatchSession = p.semSession,
                                           SemNo = p.semesterNo,                                                                                   
                                           IsActive = p.IsActive,
                                           IsVerified = p.IsVerified,
                                           Show_On_Web = p.Show_On_Web
                                       }).FirstOrDefault();

                    ddlLearningMode.SelectedValue = Batchcentre.learningModeID.ToString();
                    txtRemarks.Text = Batchcentre.remarks.ToString();
                    ddlCourse.SelectedValue = Batchcentre.CourseID.ToString();
                    ddlCourse.SelectedValue = Batchcentre.CourseID != null && Batchcentre.CourseID != 0 ? Batchcentre.CourseID.ToString() : "0";
                    int id3 = Convert.ToInt32(ddlCourse.SelectedValue);
                    ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                    if (Batchcentre.BatchId != null)
                        ddlBatchCode.SelectedValue = Batchcentre.BatchId.ToString();
                    ddlBatchCode_SelectedIndexChanged(ddlBatchCode, EventArgs.Empty);
                    if (Batchcentre.SemNo != null)
                        ddlSemNo.SelectedValue = Batchcentre.SemNo.ToString();
                  
                    ddlbatchSession.SelectedValue = Batchcentre.BatchSession.ToString();
                    NIELITMISContext context2 = new NIELITMISContext();
                    txtName.Text = Batchcentre.BatchName.ToString();
                    txtstartDate.Text = Batchcentre.startDate.ToString("dd-MMM-yyyy");
                    txtendDate.Text = Batchcentre.endDate.ToString("dd-MMM-yyyy");
                    if (UserTypeId == 10)
                    {
                        this.Rview.Visible = true;
                        this.Rview1.Visible = true;
                    }

                    if (Batchcentre.IsActive == true)
                    {
                        ddlActive.SelectedValue = "1";
                    }
                    else
                    {
                        ddlActive.SelectedValue = "0";
                    }

                    if (Batchcentre.IsVerified == true)
                    {
                        ddlVerifieds.SelectedValue = "1";
                    }
                    else
                    {
                        ddlVerifieds.SelectedValue = "0";
                    }

                    if (Batchcentre.Show_On_Web == true)
                    {
                        ddlShowOnWeb.SelectedValue = "1";
                    }
                    else
                    {
                        ddlShowOnWeb.SelectedValue = "0";
                    }
                    ddlSemNo.SelectedValue = Batchcentre.SemNo.ToString();
                    txtbatchDurationPracticalHours.Text = Batchcentre.BatchDurationPracticalHours.ToString();
                    txtbatchDurationTheoryHours.Text = Batchcentre.BatchDurationTheoryHours.ToString();
                    txtFName.Text = Batchcentre.FacultyName.ToString();
                    txtFEmail.Text = Batchcentre.FacultyEmail.ToString();
                    //txtOrgTrained.Text = Batchcentre.orgTrained.ToString();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Batchcentre.BatchName, "Admin/SemesterDetail.aspx?" + Request.QueryString.ToString(), ""));
                    //Get last modified date of current record and save it in ViewState object.
                    ViewState["LastModifiedOn"] = DateTime.Now;
                }
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
    public DataTable FillGridViewNIELITMISCourseNielitCourseRecord()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                Int64 ent = Convert.ToInt64(Session["EntityID"]);
                using (SqlCommand cmd = new SqlCommand("FillGridViewNIELITMISSemesterCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreID"].Value = Convert.ToInt64(Session["EntityID"]);
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
                Int32 VerifiedStatus = 0;
                Int32 NielitCentreIdRefAfflorNonAffl = 0, subcenteridLoginRef = 0;
                if (ddlCourseName.SelectedValue != "0")
                    CourseName = Convert.ToInt64(ddlCourseName.SelectedValue);
                if (ddlbatchname.SelectedValue != "0")
                    batchname = Convert.ToInt32(ddlbatchname.SelectedValue);
                //if (ddlBatchVerifiedStatus.SelectedValue != "0")
                VerifiedStatus = Convert.ToInt32(ddlBatchVerifiedStatus.SelectedValue);
                //if (ddlbatchcode.SelectedValue != "0") 
                //    Batchcodeid = Convert.ToInt32(ddlbatchcode.SelectedValue);              
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
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            NielitCentreIdFilter = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    else if (UserTypeId == 4)
                    {
                        var intituteslinkedToCentre = from s in context.AffInstitutes
                                                      where s.instituteID == loginUser.UserRefNumber
                                                      select new { ID = s.ID, linkedToCentre = s.linkedToCentre };

                        // NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.ToList().FirstOrDefault().linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
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
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            NielitCentreIdRefAfflorNonAffl = NelitCentreLinkId;
                            NielitCentreIdFilter = NielitCentreId;
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            NIELITCentreId.Value = NelitCentreLinkId.ToString();
                            NielitCentreIdRefAfflorNonAffl = NelitCentreLinkId;
                            NielitCentreIdFilter = NielitCentreId;
                        }
                    }
                }
               
                using (DataTable dt = FillGridViewNIELITMISCourseNielitCourseRecord())
                {
                    if (dt.Rows.Count > 0)
                    {
                        var CentreBatchs = (from p in dt.AsEnumerable()
                                            select new
                                            {
                                                ID = p.Field<Int64>("ID"),
                                                Name = p.Field<string>("Name"),
                                                BatchId = p.Field<Int64>("batchID"),
                                                CourseName = p.Field<string>("CourseName"),
                                                BatchCode = p.Field<string>("BatchCode"),
                                                SemNo = p.Field<int>("SemNo"),
                                                subcenterid = p.Field<Int64>("subcenterid"),
                                                startDate = p.Field<DateTime>("startDate"),
                                                endDate = p.Field<DateTime>("endDate"),
                                                CourseID = p.Field<Int64>("CourseID"),
                                                CentreID = p.Field<Int64>("CentreID"),
                                                enterBy = p.Field<int>("enterBy"),
                                                IsVerified = p.Field<string>("IsVerified"),
                                                IsVerifiedid = p.Field<int>("IsVerifiedid"),
                                                learningModeName = p.Field<string>("learningModeName")
                                            });
                        if (NielitCentreIdRefAfflorNonAffl != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.CentreID == NielitCentreIdRefAfflorNonAffl);
                        }
                        if (UserTypeId != 10)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.enterBy == loginUserNo || s.subcenterid == subcenteridLoginRef);
                        }
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.Name.ToUpper().Contains(searchString) || s.BatchCode.ToUpper().Contains(searchString));
                        }
                        if (CourseName != 0 && batchname != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseName && s.BatchId == batchname);
                        }
                        else if (CourseName != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseName);
                        }
                        else if (batchname != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.ID == batchname);
                        }
                        if (VerifiedStatus != 99)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.IsVerifiedid == VerifiedStatus);
                        }
                                    

                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.ID);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.ID);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.Name);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                                    break;
                                case "BatchCode":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.BatchCode);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.BatchCode);
                                    break;
                                case "startDate":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.startDate);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.startDate);
                                    break;
                                case "endDate":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.endDate);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.endDate);
                                    break;
                                default:
                                    CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                                    break;
                            }
                        }

                        PagingBar1.Bind(CentreBatchs, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        // gvMain.Columns[5].Visible = false;
                        if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        {
                            gvMain.Columns[7].Visible = false;
                        }
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
        //ShowAlert("Inside Code");
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            this.Rview.Visible = false;
            this.Rview1.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Semester Detail";
            //Updating Breadcrumb         
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Semester Detail", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("SemesterDetail.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("SemesterDetail.aspx", true);
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
            BreadCrumb1.Render();
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            Int64 CourseId = Convert.ToInt64(ddlCourse.SelectedValue.Trim());
            Int64 batchid = Convert.ToInt64(ddlBatchCode.SelectedValue.Trim());
            DateTime semStartDate = Convert.ToDateTime(txtstartDate.Text);
             Int32 Number = Convert.ToInt32(ddlSemNo.SelectedItem.Text);
             Int32 semno = Number - 1;
            DateTime semEndDate = Convert.ToDateTime(txtendDate.Text);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                SemesterDetail objBatchCentre = new SemesterDetail();


                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {                    
                    var application = (from a in context.SemesterDetail

                                       where a.semesterNo == Number && a.batchID == batchid && a.CourseDurationID==CourseId
                                       select new
                                       {
                                           ID = a.ID,
                                           CourseId = a.CourseDurationID

                                       }).FirstOrDefault();

                    if (application != null)
                    {                        
                       ShowAlert("Record already exist for this semester No. Please select another semester No.");
                        return;
                    }
                    if (Number > 1)
                    {
                        var applicationcheck = (from a in context.SemesterDetail

                                                where a.semesterNo == semno && a.batchID == batchid && a.CourseDurationID == CourseId
                                                select new
                                                {
                                                    ID = a.ID,
                                                    CourseId = a.CourseDurationID

                                                }).FirstOrDefault();

                        if (applicationcheck == null)
                        {
                            ShowAlert("Record not exist for previous semester No. Please select Previous semester No.");
                            return;
                        }
                    }

                    if (Number == 1)
                    {
                        var BatchStartDates = (from x in context.NielitCentreBatchs where x.CourseDurationID == CourseId && x.ID == batchid  select x).First();
                        DateTime BatchStartDate = Convert.ToDateTime(BatchStartDates.startDate);
                        DateTime BatchEndDate = Convert.ToDateTime(BatchStartDates.endDate);

                        if (semStartDate < BatchStartDate)
                        {
                            ShowAlert("Semester Start date should be greater than or equal to Batch Start date.");
                            return;
                        }
                        if (semStartDate > BatchEndDate)
                        {
                            ShowAlert("Semester Start date should be Less than or equal to Batch End date.");
                            return;
                        }
                    }

                    if (Number > 1)
                    {
                        var BatchStartDates = (from x in context.SemesterDetail where x.CourseDurationID == CourseId && x.batchID == batchid && x.semesterNo == semno select x).First();
                        DateTime BatchStartDate = Convert.ToDateTime(BatchStartDates.semEndDate);
                        if (semStartDate < BatchStartDate)
                        {
                            ShowAlert("Semester Start date should be greater than previous Semester End Date.");
                            return;
                        }

                        var BatchStartDatess = (from x in context.NielitCentreBatchs where x.CourseDurationID == CourseId && x.ID == batchid select x).First();
                       
                        DateTime BatchEndDate = Convert.ToDateTime(BatchStartDatess.endDate);                       
                        if (semStartDate > BatchEndDate)
                        {
                            ShowAlert("Semester Start date should be Less than or equal to Batch End date.");
                            return;
                        }
                    }
                  
                    //learningmode
                    objBatchCentre.learningModeID = Convert.ToInt32(ddlLearningMode.SelectedValue);
                    objBatchCentre.Remarks = txtRemarks.Text;
                    //                      
                    objBatchCentre.CourseDurationID = Convert.ToInt32(ddlCourse.SelectedValue);
                    objBatchCentre.batchID = Convert.ToInt32(ddlBatchCode.SelectedValue);
                    objBatchCentre.semesterNo = Convert.ToInt32(ddlSemNo.SelectedItem.Text);
                    objBatchCentre.semStartDate = Convert.ToDateTime(txtstartDate.Text);
                    objBatchCentre.semEndDate = Convert.ToDateTime(txtendDate.Text);
                    objBatchCentre.semSession = Convert.ToInt32(ddlbatchSession.SelectedValue);
                    objBatchCentre.batchDurationTheoryHours = Convert.ToDecimal(txtbatchDurationTheoryHours.Text);
                    objBatchCentre.batchDurationPracticalHours = Convert.ToDecimal(txtbatchDurationPracticalHours.Text);
                    objBatchCentre.centreID = Convert.ToInt64(Session["EntityID"]);
                    objBatchCentre.facultyName = txtFName.Text;
                    objBatchCentre.facultyEmail = txtFEmail.Text;
                    objBatchCentre.enterDate = DateTime.Now;
                    objBatchCentre.enterBy = Convert.ToInt32(Session["UserID"]);
                    objBatchCentre.IsActive = true;
                    objBatchCentre.IsVerified = false;
                    objBatchCentre.Show_On_Web = true;
                    context.SemesterDetail.Add(objBatchCentre);
                    context.SaveChanges();
                    strMessage = "New record saved.";

                }
                else
                {
                    objBatchCentre = context.SemesterDetail.Find(Convert.ToInt32(Request.QueryString["key"]));

                    if (Number > 1)
                    {
                        var BatchStartDates = (from x in context.SemesterDetail where x.CourseDurationID == CourseId && x.batchID == batchid && x.semesterNo == semno select x).First();
                        DateTime BatchStartDate = Convert.ToDateTime(BatchStartDates.semEndDate);
                        if (semStartDate < BatchStartDate)
                        {
                            ShowAlert("Semester Start date should be greater than previous Semester End Date.");
                            return;
                        }
                    }
                    
                    //Added learning mode
                    objBatchCentre.learningModeID = Convert.ToInt32(ddlLearningMode.SelectedValue);
                    objBatchCentre.Remarks = txtRemarks.Text;
                    //
                    objBatchCentre.CourseDurationID = Convert.ToInt32(ddlCourse.SelectedValue);
                    objBatchCentre.semStartDate = Convert.ToDateTime(txtstartDate.Text);
                    objBatchCentre.semEndDate = Convert.ToDateTime(txtendDate.Text);
                    objBatchCentre.semSession = Convert.ToInt32(ddlbatchSession.SelectedValue);
                    objBatchCentre.batchDurationTheoryHours = Convert.ToDecimal(txtbatchDurationTheoryHours.Text);
                    objBatchCentre.batchDurationPracticalHours = Convert.ToDecimal(txtbatchDurationPracticalHours.Text);

                    objBatchCentre.facultyName = txtFName.Text;
                    objBatchCentre.facultyEmail = txtFEmail.Text;
                    objBatchCentre.enterDate = DateTime.Now;
                    if (ddlActive.SelectedValue == "1")
                        objBatchCentre.IsActive = true;
                    else
                    {
                        objBatchCentre.IsActive = false;
                    }

                    if (ddlVerifieds.SelectedValue == "1")
                        objBatchCentre.IsVerified = true;
                    else
                    {
                        objBatchCentre.IsVerified = false;
                    }

                    if (ddlShowOnWeb.SelectedValue == "1")
                        objBatchCentre.Show_On_Web = true;
                    else
                    {
                        objBatchCentre.Show_On_Web = false;
                    }
                    strMessage = "Record updated.";
                    context.SaveChanges();
                }
            }
            Response.Redirect("SemesterDetail.aspx?msg=" + strMessage, true);
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
            ddlBatchVerifiedStatus.SelectedValue = "99";
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
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;

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
            //var BatchName =  from s in context.NielitCentreBatchs
            //                         join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID 
            //                         join p in context.NielitCentreCourses on c.courseID equals p.ID
            //                 where s.centreID == NielitCentreIdSearch && c.isVerified==true && c.isVerified==true
            //                 select new { Name = s.Name };

            var BatchName = from s in context.NielitCentreBatchs
                            join b in context.SemesterDetail on s.ID equals b.batchID 
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true
                            select new { Name = s.Name };

            if (!String.IsNullOrEmpty(searchString))
            {
                BatchName = BatchName.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            BatchName = BatchName.OrderBy(s => s.Name);

            //var Batchcode = from s in context.NielitCentreBatchs
            //               join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID
            //               join p in context.NielitCentreCourses on c.courseID equals p.ID
            //                where s.centreID == NielitCentreIdSearch && c.isVerified == true && c.isVerified == true
            //               select new { Name = s.BatchCode };
            var Batchcode = from s in context.NielitCentreBatchs
                            join b in context.SemesterDetail on s.ID equals b.batchID 
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true
                            select new { Name = s.BatchCode };

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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SemesterDetail.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("SemesterDetail.aspx", true);
    }

    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            //using (NIELITMISContext context = new NIELITMISContext())
            //{
            //    Int64 batchnameid = Convert.ToInt64(ddlbatchname.SelectedValue);
            //    //Int32 coursecategoryid = Convert.ToInt32(ddlCourseCategory.SelectedValue);
            //    if (batchnameid != 0)
            //    {
            //        var BatchName = from p in context.NielitCentreCourses
            //                        join k in context.NielitCourseDurations on p.ID equals k.courseID
            //                        join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
            //                        where p.IsVerified == true && k.isVerified == true && k.ID == batchnameid
            //                        orderby (d.Name)
            //                        select new { ValueField = d.ID, TextField = d.BatchCode };

            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchcode, BatchName.Distinct(), lst);
            // }
            // }
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
                            join i in context.SemesterDetail
                                on s.ID equals i.batchID
                            where s.IsVerified == true && s.centreID == nielitcentreid 
                                    && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, Batch.Distinct(), lst);

            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lbdisable_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to edit the records.", true);
                    return;
                }
                ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                if (examcenter.IsEnabled == true)
                    examcenter.IsEnabled = false;
                else
                    examcenter.IsEnabled = true;
                context.Entry(examcenter).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                BindGridView();
                ShowAlert("You have successfully changed the status of the Exam Center.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be edited!", true);
        }
    }
}