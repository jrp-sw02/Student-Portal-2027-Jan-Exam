using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
//Added for biometric
using System.Net;
using System.Net.Sockets;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


public partial class Admin_NielitCentreFormalStudent_Verify_Info : BasePage
{
    public string finalResponse;


    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0, NielitCentreIdFilter = 0;
    Int32 UserTypeId = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0, BatchIDs = 0, IsverifyStatus = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError.Visible = false;
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
            BatchIDs = Convert.ToInt32(Request.QueryString["BatchID"]);
            IsverifyStatus = Convert.ToInt32(Request.QueryString["VerifyStatus"]);
            Int32 IsverifyStatus1 = Convert.ToInt32(Session["status"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

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

                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (NielitCentrelinkedToCentreId != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                            }
                        }
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "Please Select Filter Criteria For View Records";
                            lblError.Visible = true;
                        }
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            ShowEditMode();
                        }
                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NielitCentreFormalStudent: Application Status", "Admin/NielitCentreFormalStudent_Verify_Info.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                        }
                        else
                        {
                            if (BatchIDs != 0)
                            {
                                BindGridView();
                            }
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NielitCentreFormalStudent: Application Status", "NielitCentreFormalStudent_Verify_Info.aspx", ""));
                        }
                    }
                }
            }

            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    public DataTable FillCourseNIELITMISCourseNielitCourse()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("FillFormalCourseNIELITMISCourseNielitCourseVerify", con))
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


    protected void ddlCourseCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        ddlcourseName.Items.Clear();
        FillCourses();
    }
    protected void FillCourses()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (ddlCourseCategoryName.SelectedValue.Trim()!="0" )
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    //var CourseList = from p in context.NielitCentreCourses
                    //                 where p.IsVerified == true && p.CourseCategoryID == catID 
                    //                 orderby (p.Name)
                    //                 select new { ValueField = p.ID, TextField = p.Name };
                    //EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);            
                    using (DataTable dt = FillCourseNIELITMISCourseNielitCourse())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ddlcourseName.DataSource = dt;
                            ddlcourseName.DataTextField = "Name";
                            ddlcourseName.DataValueField = "ID";
                            ddlcourseName.DataBind();
                            ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                        }
                        else
                        {
                            ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
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
    protected void ddlcourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
            ListItem lst = new ListItem("--Select One--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlcourseName.SelectedValue);
                Int64 CCatID = Convert.ToInt32(ddlCourseCategoryName.SelectedValue);

                var BatchName = from p in context.NielitCentreBatchs
                                join k in context.NielitCourseDurations on p.CourseDurationID equals k.ID
                                //where p.IsVerified == true && k.isVerified == true && k.courseID == coursenameid
                                where k.ID == coursenameid
                                        && p.centreID == NielitCentreIdFilter
                                orderby (p.Name)
                                select new { ValueField = p.ID, TextField = p.Name };
                ddlbatchname.Items.Clear();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName.Distinct(), lst);


            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseVerifiedStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 verifyStatush = 0;
            if (HVerifystatus.Value == null || HVerifystatus.Value != "0")
            {
                if (ddlCourseVerifiedStatus.SelectedValue != "0")
                {
                    HVerifystatus.Value = ddlCourseVerifiedStatus.SelectedValue;
                    HVerifystatus1.Value = ddlCourseVerifiedStatus.SelectedValue;
                    verifyStatush = Convert.ToInt32(ddlCourseVerifiedStatus.SelectedValue);
                }
            }
            Session["status"] = ddlCourseVerifiedStatus.SelectedValue;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlIsverifiedStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 IsverifyStatus = Convert.ToInt32(ddlIsverifiedStatus.SelectedValue);//HCourseID.Value
            Int32 courseid = 0, bathcid = 0, studentid = 0;
            lblmsg.Visible = false;
            courseid = Convert.ToInt32(HCourseID.Value.ToString());
            bathcid = Convert.ToInt32(HBatchId.Value.ToString());
            studentid = Convert.ToInt32(HStudentID.Value.ToString());
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (IsverifyStatus == 1)
                {
                    var CheckAadharAuthRequred = (from p in context.NielitProjCoursess
                                                  join d in context.NielitCourseDurations on p.courseID equals d.ID
                                                  join c in context.NielitCentreCourses on d.courseID equals c.ID
                                                  join k in context.NielitProjectss on p.projID equals k.ID
                                                  where d.ID == courseid && k.isAadharAuthenticationReqd == true
                                                  select p).Count();
                    if (CheckAadharAuthRequred != 0)
                    {
                        //var CheckAadharAuthStudent = (from p in context.NielitCentreStudent
                        //                              where p.CourseID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == false
                        //                              select p).Count();

                        var CheckAadharAuthStudent = (from p in context.NielitCentreStudent
                                                      join d in context.NielitCourseDurations on p.CourseID equals d.ID
                                                      where d.ID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == false
                                                      select p).Count();

                        //Label4.Visible = true;
                        if (CheckAadharAuthStudent != 0)
                        {
                            btnCaptureBiometric.Visible = true;
                            lblmsg.Visible = false;
                            btnVerify.Visible = false;
                        }
                        else
                        {
                            btnCaptureBiometric.Visible = false;
                            btnVerify.Visible = false;
                        }
                    }
                    else
                    {
                        btnCaptureBiometric.Visible = false;
                        btnVerify.Visible = true;
                    }
                }
                else
                {
                    btnCaptureBiometric.Visible = false;
                    btnVerify.Visible = true;
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void VerifyRecord(object sender, EventArgs e) //SaveRecordNielitCourse
    {
        try
        {
            BreadCrumb1.Render();
            using (NIELITMISContext context = new NIELITMISContext())
            {
                //create and object 
                NielitCentreStudent student;
                Boolean isverified = false;
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    student = new NielitCentreStudent();
                    student = context.NielitCentreStudent.Find(Convert.ToInt32(Request.QueryString["key"]));
                    if (ddlIsverifiedStatus.SelectedValue == "1")
                    {
                        isverified = true;
                    }
                    if (ddlIsverifiedStatus.SelectedValue == "2")
                    {
                        isverified = false; ;
                    }
                    student.IsVerifiedByInstitute = isverified;
                    student.DateOfVerificationByInstitute = DateTime.Now; ;
                    context.SaveChanges();

                    //strMessage = "Verified Successfully !.";
                    lblmsg.Text = "Verification Status recorded Successfully !.";
                    lblmsg.ForeColor = System.Drawing.Color.Green;

                    lblmsg.Visible = true;
                }
            }
            // Response.Redirect("NielitCentreStudent_Verify_Info.aspx?msg=" + strMessage, true);
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
            // Int32 BatchID = 0;
            lblError.Visible = false;
            gvMain.Visible = true;
            NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
            Int64 CourseId = Convert.ToInt64(ddlcourseName.SelectedValue);
            Int32 BatchID = Convert.ToInt32(ddlbatchname.SelectedValue);
            Int32 BatchIDh = 0, verifiedid = 0;
            verifiedid = Convert.ToInt32(ddlCourseVerifiedStatus.SelectedValue);
            if (!String.IsNullOrEmpty(HBatchId.Value))
            {
                BatchIDh = Convert.ToInt32(HBatchId.Value.ToString());
            }
            if (!String.IsNullOrEmpty(HBatchID1.Value))
            {
                BatchIDh = Convert.ToInt32(HBatchID1.Value.ToString());
            }
            if (BatchIDs != 0)
            {
                BatchIDh = Convert.ToInt32(Request.QueryString["BatchID"]);
            }
            if (BatchID != 0)
            {
                BatchIDh = BatchID;
            }
            Int32 IsverifyStatus1 = Convert.ToInt32(Session["status"]);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
            using (NIELITMISContext context = new NIELITMISContext())
            {
                var courseStudentData = (from s in context.NielitCentreStudent
                                         join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                         where b.centreID == NielitCentreIdFilter && s.semesterid != null && (s.batch_ID == BatchID || s.batch_ID == BatchIDh || s.CourseID == CourseId)
                                       
                                         select new
                                          {
                                              ID = s.ID,
                                              BatchID = s.batch_ID,
                                              Appno = s.Number,
                                              Appdate = s.ApplicationDate,
                                              Name = s.Name.ToUpper(),
                                              FatherName = s.FatherName.ToUpper(),
                                              DOB = s.DateOfBirth,
                                              VerifiedStatusID = s.IsVerifiedByInstitute == true ? 1 : 2,
                                              VerifiedStatus = s.IsVerifiedByInstitute == true ? "Verified" : "Not Verified",
                                              projectId = s.projectId,
                                              AadharVerfied = s.AadharVerfied,
                                              Genderr = s.Gender,
                                              MotherName = s.MotherName,
                                              Mobilenumber = s.MobileNumber,
                                              EmailId = s.EmailAddress,
                                              SemNo=s.semesterid

                                          });
                if (verifiedid != 0)
                {
                    if (verifiedid != 99)
                    {
                        courseStudentData = courseStudentData.Where(s => s.VerifiedStatusID == verifiedid);
                    }
                    else
                    { }
                }
               
                if (IsverifyStatus1 != 0)
                {
                    if (IsverifyStatus1 != 99)
                    {
                        courseStudentData = courseStudentData.Where(s => s.VerifiedStatusID == IsverifyStatus1);
                    }
                    else { }
                }
                courseStudentData = courseStudentData.OrderByDescending(s => s.ID);

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                courseStudentData = courseStudentData.OrderByDescending(s => s.ID);
                            else
                                courseStudentData = courseStudentData.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                courseStudentData = courseStudentData.OrderByDescending(s => s.Name);
                            else
                                courseStudentData = courseStudentData.OrderBy(s => s.Name);
                            break;

                        default:
                            courseStudentData = courseStudentData.OrderByDescending(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(courseStudentData, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();

            };
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
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
            // ucSearchBar.Visible = false;          
            btnVerify.Text = "Verify";
            lblHeading.Text = "NIELIT  Centre Formal Student";
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 NielitStudentId = 0;
                NielitStudentId = Convert.ToInt32(Request.QueryString["Key"]);
                NielitCentreStudent editCourse = context.NielitCentreStudent.Find(NielitStudentId);
                //var CheckAadharAuthStudent = (from p in context.NielitCentreStudent
                //                                     join d in context.NielitCourseDurations on p.CourseID equals d.ID
                //                                     where d.ID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == true
                //                                     select p).Count();

                var RecordExistscourseStudentData = (from s in context.NielitCentreStudent
                                                     join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                                     join c in context.NielitCourseDurations on b.CourseDurationID equals c.ID
                                                     join k in context.NielitCentreCourses on c.courseID equals k.ID
                                                     where b.centreID == NielitCentreIdFilter && s.ID == NielitStudentId
                                                     select s).Count();
                if (RecordExistscourseStudentData != 0)
                {
                    var courseStudentData = (from s in context.NielitCentreStudent
                                             join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                             join c in context.NielitCourseDurations on b.CourseDurationID equals c.ID
                                             join k in context.NielitCentreCourses on c.courseID equals k.ID
                                             where b.centreID == NielitCentreIdFilter && s.ID == NielitStudentId
                                             //orderby s.ID  descending
                                             select new
                                             {
                                                 ID = s.ID,
                                                 BatchID = s.batch_ID,
                                                 Appno = s.Number,
                                                 Appdate = s.ApplicationDate,
                                                 Name = s.Name.ToUpper(),
                                                 FatherName = s.FatherName.ToUpper(),
                                                 DOB = s.DateOfBirth,
                                                 VerifiedStatus = s.IsVerifiedByInstitute == true ? "Verified" : "Not Verified",
                                                 VerifiedStatusID = s.IsVerifiedByInstitute == true ? 1 : 2,
                                                 projectId = s.projectId,
                                                 CourseName = k.Name,
                                                 AadharVerfied = s.AadharVerfied,
                                                 Genderr = s.Gender,
                                                 MotherName = s.MotherName,
                                                 Mobilenumber = s.MobileNumber,
                                                 EmailId = s.EmailAddress,
                                                 CourseId = s.CourseID,
                                                 batchIdH = s.batch_ID,
                                                 aadhaar = s.UIDNumber,
                                                 EWS = s.Is_EWS

                                             }).FirstOrDefault();
                    lblID.Text = courseStudentData.ID.ToString().Trim();
                    lblAppno.Text = courseStudentData.Appno.ToString();
                    lblApplicationDate.Text = courseStudentData.Appdate.ToString("dd-MMM-yyyy");
                    lblCourse.Text = courseStudentData.CourseName.ToString();
                    Lblname.Text = courseStudentData.Name.ToString();
                    LblFatherName.Text = string.IsNullOrEmpty(courseStudentData.FatherName) == false && !string.IsNullOrWhiteSpace(courseStudentData.FatherName) ? GetInitCap(courseStudentData.FatherName) : "";
                    LblMotherName.Text = string.IsNullOrEmpty(courseStudentData.MotherName) == false && !string.IsNullOrWhiteSpace(courseStudentData.MotherName) ? GetInitCap(courseStudentData.MotherName) : "";
                    HBatchId.Value = courseStudentData.batchIdH.ToString();
                    HCourseID.Value = courseStudentData.CourseId.ToString();
                    HStudentID.Value = courseStudentData.ID.ToString();
                    Label2.Text = courseStudentData.DOB.ToString("dd-MMM-yyyy");
                    Label1.Text = courseStudentData.Genderr.ToString();
                    lblmobile.Text = courseStudentData.Mobilenumber.ToString();
                    lblemail.Text = courseStudentData.EmailId.ToString();
                    lblStatus.Text = courseStudentData.VerifiedStatus.ToString();
                    lblAadhaar.Text = courseStudentData.aadhaar.ToString();
                    if (courseStudentData.EWS)
                        lblEWS.Text = "Yes";
                    else
                        lblEWS.Text = "No";

                    Int64 courseid = Convert.ToInt64(courseStudentData.CourseId.ToString());
                    Int64 bathcid = Convert.ToInt64(courseStudentData.batchIdH.ToString());
                    Int64 studentid = Convert.ToInt64(courseStudentData.ID.ToString());
                    var CheckAadharAuthStudent = (from p in context.NielitCentreStudent
                                                  join d in context.NielitCourseDurations on p.CourseID equals d.ID
                                                  where d.ID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == true
                                                  select p).Count();

                    //Label4.Visible = true;
                    if (CheckAadharAuthStudent != 0)
                    {
                        ddlIsverifiedStatus.Enabled = false;
                        ddlIsverifiedStatus.SelectedValue = "1";
                    }

                    else if (CheckAadharAuthStudent == 0)
                    {
                        var CheckStudent = (from p in context.NielitCentreStudent
                                                      join d in context.NielitCourseDurations on p.CourseID equals d.ID
                                                      where d.ID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.IsVerifiedByInstitute==true
                                                      select p).Count();
                        if (CheckStudent != 0)
                        {
                            ddlIsverifiedStatus.Enabled = false;
                            ddlIsverifiedStatus.SelectedValue = "1";
                        }
                        else if (CheckStudent == 0)
                        {
                            ddlIsverifiedStatus.Enabled = true;
                           // ddlIsverifiedStatus.SelectedValue = "1";
                        }

                       // ddlIsverifiedStatus.SelectedValue = "1";
                    }


                }
                else // nielit db course table data  only batch id with fectch student data for verify
                {
                    var courseStudentData = (from s in context.NielitCentreStudent
                                             join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                             //join c in context.NielitCourseDurations on b.CourseDurationID equals c.ID
                                             // join k in context.NielitCentreCourses on c.courseID equals k.ID

                                             where b.centreID == NielitCentreIdFilter && s.ID == NielitStudentId
                                             //orderby s.ID  descending
                                             select new
                                             {
                                                 ID = s.ID,
                                                 BatchID = s.batch_ID,
                                                 Appno = s.Number,
                                                 Appdate = s.ApplicationDate,
                                                 Name = s.Name.ToUpper(),
                                                 FatherName = s.FatherName.ToUpper(),
                                                 DOB = s.DateOfBirth,
                                                 VerifiedStatus = s.IsVerifiedByInstitute == true ? "Verified" : "Not Verified",
                                                 VerifiedStatusID = s.IsVerifiedByInstitute == true ? 1 : 2,
                                                 projectId = s.projectId,
                                                 // CourseName = k.Name,
                                                 AadharVerfied = s.AadharVerfied,
                                                 Genderr = s.Gender,
                                                 MotherName = s.MotherName,
                                                 Mobilenumber = s.MobileNumber,
                                                 EmailId = s.EmailAddress,
                                                 CourseId = s.CourseID,
                                                 batchIdH = s.batch_ID,
                                                 aadhaar = s.UIDNumber,
                                                 EWS = s.Is_EWS

                                             }).FirstOrDefault();
                    lblID.Text = courseStudentData.ID.ToString().Trim();
                    lblAppno.Text = courseStudentData.Appno.ToString();
                    lblApplicationDate.Text = courseStudentData.Appdate.ToString("dd-MMM-yyyy");
                    // lblCourse.Text = courseStudentData.CourseName.ToString();
                    Lblname.Text = courseStudentData.Name.ToString();
                    LblFatherName.Text = string.IsNullOrEmpty(courseStudentData.FatherName) == false && !string.IsNullOrWhiteSpace(courseStudentData.FatherName) ? GetInitCap(courseStudentData.FatherName) : "";
                    LblMotherName.Text = string.IsNullOrEmpty(courseStudentData.MotherName) == false && !string.IsNullOrWhiteSpace(courseStudentData.MotherName) ? GetInitCap(courseStudentData.MotherName) : "";
                   // LblFatherName.Text = courseStudentData.FatherName.ToString();
                  //  LblMotherName.Text = courseStudentData.MotherName.ToString();
                    HBatchId.Value = courseStudentData.batchIdH.ToString();
                    HCourseID.Value = courseStudentData.CourseId.ToString();
                    HStudentID.Value = courseStudentData.ID.ToString();
                    Label2.Text = courseStudentData.DOB.ToString("dd-MMM-yyyy");
                    Label1.Text = courseStudentData.Genderr.ToString();
                    lblmobile.Text = courseStudentData.Mobilenumber.ToString();
                    lblemail.Text = courseStudentData.EmailId.ToString();
                    lblStatus.Text = courseStudentData.VerifiedStatus.ToString();
                    lblAadhaar.Text = courseStudentData.aadhaar.ToString();
                    // deep add line on 21 June 2021
                    String courseIds = courseStudentData.CourseId.ToString();
                    Int32 ccid = Convert.ToInt32(courseIds);
                    EConnectContext contextt2 = new EConnectContext();
                    var courseNames = (from c in contextt2.Courses
                                       where c.ID == ccid
                                       select new
                                       {
                                           cID = c.ID,
                                           CourseName = c.Name
                                       }).FirstOrDefault();
                    lblCourse.Text = courseNames.CourseName.ToString();
                    //deep add end code line on 21 June 2021

                    if (courseStudentData.EWS)
                        lblEWS.Text = "Yes";
                    else
                        lblEWS.Text = "No";

                    Int64 courseid = Convert.ToInt64(courseStudentData.CourseId.ToString());
                    Int64 bathcid = Convert.ToInt64(courseStudentData.batchIdH.ToString());
                    Int64 studentid = Convert.ToInt64(courseStudentData.ID.ToString());
                    var CheckAadharAuthStudent = (from p in context.NielitCentreStudent
                                                  //join d in context.NielitCourseDurations on p.CourseID equals d.ID
                                                  //where d.ID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == true
                                                  where p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == true
                                                  select p).Count();

                    //Label4.Visible = true;
                    if (CheckAadharAuthStudent != 0)
                    {
                        ddlIsverifiedStatus.Enabled = false;
                        ddlIsverifiedStatus.SelectedValue = "1";
                    }


                }

                //var courseStudentData = (from s in context.NielitCentreStudent                                         
                //                         join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                //                         join c in context.NielitCourseDurations on b.CourseDurationID equals c.ID
                //                         join k in context.NielitCentreCourses on c.courseID equals k.ID
                //                         where b.centreID == NielitCentreIdFilter && s.ID == NielitStudentId
                //                         //orderby s.ID  descending
                //                         select new
                //                         {
                //                             ID = s.ID,
                //                             BatchID=s.batch_ID,
                //                             Appno = s.Number,
                //                             Appdate = s.ApplicationDate,
                //                             Name = s.Name.ToUpper(),
                //                             FatherName = s.FatherName.ToUpper(),
                //                             DOB = s.DateOfBirth,
                //                             VerifiedStatus = s.IsVerifiedByInstitute == true ? "Verified" : "Not Verified",
                //                             VerifiedStatusID = s.IsVerifiedByInstitute == true ? 1 : 2,
                //                             projectId = s.projectId,
                //                             CourseName=k.Name,                                            
                //                             AadharVerfied = s.AadharVerfied,
                //                             Genderr = s.Gender,
                //                             MotherName = s.MotherName,
                //                             Mobilenumber = s.MobileNumber,
                //                             EmailId = s.EmailAddress,
                //                             CourseId=s.CourseID,
                //                             batchIdH=s.batch_ID,
                //                             aadhaar=s.UIDNumber ,
                //                             EWS=s.Is_EWS 

                //                         }).FirstOrDefault();
                //lblID.Text = courseStudentData.ID.ToString().Trim();
                //lblAppno.Text = courseStudentData.Appno.ToString();
                //lblApplicationDate.Text = courseStudentData.Appdate.ToString("dd-MMM-yyyy");
                //lblCourse.Text = courseStudentData.CourseName.ToString();
                //Lblname.Text = courseStudentData.Name.ToString();
                //LblFatherName.Text = courseStudentData.FatherName.ToString();
                //LblMotherName.Text = courseStudentData.MotherName.ToString();
                //HBatchId.Value = courseStudentData.batchIdH.ToString();
                //HCourseID.Value = courseStudentData.CourseId.ToString();
                //HStudentID.Value = courseStudentData.ID.ToString();
                //Label2.Text = courseStudentData.DOB.ToString("dd-MMM-yyyy");
                //Label1.Text = courseStudentData.Genderr.ToString();
                //lblmobile.Text = courseStudentData.Mobilenumber.ToString();
                //lblemail.Text = courseStudentData.EmailId.ToString();
                //lblStatus.Text = courseStudentData.VerifiedStatus.ToString();
                //lblAadhaar.Text = courseStudentData.aadhaar.ToString ();
                //if (courseStudentData.EWS)
                //    lblEWS.Text = "Yes";
                //else
                //    lblEWS.Text = "No";

                //Int64 courseid = Convert.ToInt64(courseStudentData.CourseId.ToString());
                //Int64 bathcid = Convert.ToInt64(courseStudentData.batchIdH.ToString());
                //Int64 studentid = Convert.ToInt64(courseStudentData.ID.ToString());
                // var CheckAadharAuthStudent = (from p in context.NielitCentreStudent
                //                                      join d in context.NielitCourseDurations on p.CourseID equals d.ID
                //                                      where d.ID == courseid && p.batch_ID == bathcid && p.ID == studentid && p.AadharVerfied == true
                //                                      select p).Count();

                //        //Label4.Visible = true;
                // if (CheckAadharAuthStudent != 0)
                // {
                //     ddlIsverifiedStatus.Enabled = false;
                //     ddlIsverifiedStatus.SelectedValue = "1";
                // }

            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnVerify.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //if (UserTypeId == 10 || UserTypeId==6 || UserTypeId==1)
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.");
            //    return;
            //}
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            // ucSearchBar.Visible = false;

            if (UserTypeId == 10)
            {
                //ddlIsVerified.Enabled = false;
            }
            //Change the heading text as required
            lblHeading.Text = "New NielitCentreFormalStudent";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NielitCentreFormalStudent: Application Status", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreFormalStudent_Verify_Info.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCentreFormalStudent_Verify_Info.aspx", true);
            }
        }
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        Int32 BatchIDh = 0, verifyStatush = 0;
        BatchIDh = Convert.ToInt32(HBatchId.Value.ToString()); //HVerifystatus

        if (this.ViewState["BatchID"] == null)
        {
            // Load data Every  time click
            if (BatchIDh != 0)
            {
                this.ViewState["BatchID"] = BatchIDh;
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreFormalStudent_Verify_Info.aspx?BatchID=" + BatchIDh), true);
            }
        }

        // Response.Redirect("NielitCentreStudent_Verify_Info.aspx", true);
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
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if (HBatchID1.Value != null || HBatchID1.Value != "0")
            {
                if (ddlbatchname.SelectedValue != "0")
                {
                    HBatchID1.Value = ddlbatchname.SelectedValue;
                    HVerifystatus.Value = ddlCourseVerifiedStatus.SelectedValue;
                }
            }
            if (this.ViewState["HVerifystatusID"] == null)
            {
                // Load data Every  time click

                this.ViewState["HVerifystatusID"] = ddlCourseVerifiedStatus.SelectedValue;
            }
            BindGridView();
            BreadCrumb1.Render();
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
            BreadCrumb1.Render();
            ddlCourseCategoryName.SelectedValue = "0";
            ddlcourseName.SelectedValue = "0";
            ddlbatchname.SelectedValue = "0";
            ddlCourseVerifiedStatus.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            gvMain.Visible = false;
            lblError.Text = "Please Select Filter Criteria For View Records";
            lblError.Visible = true;
            uPnlGrid.Update();
            BreadCrumb1.RemoveLastBreadCrumbItem();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application Status:", "Admin/NielitCentreFormalStudent_Verify_Info.aspx", ""));
            BreadCrumb1.Render();
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
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    href += "&Id=" + Request.QueryString["Id"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
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
    protected void btnCaptureBiometric_Click(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsValid)
                return;
            if (Page.IsValid)
            {
                //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreStudent_Verify_Info.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../aadharForm.aspx?para1=" + Lblname.Text + "&para2=" + Label2.Text + "&para3=" + Label1.Text.Substring(0, 1) + "&para4=" + lblAadhaar.Text + "&para5=" + lblID.Text), false);
                ////    getData();
                ////    // string x = Session["Name"].ToString();
                ////    //TextBox t = (TextBox)form1.FindControl("TextBox1");
                ////    //if (t != null)
                ////    //    finalResponse = Request.Form["TextBox1"].ToString();
                ////    //HiddenField  l = (HiddenField )form1.FindControl("hidPID");
                ////    //finalResponse = l.Value ;
                ////    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(finalResponse);
                ////    finalResponse = System.Convert.ToBase64String(plainTextBytes);
                ////    ShowAlert(finalResponse);
                ////    // ShowAlert(finalResponse);


                ////    //string jsonString = people.ToJSON();
                ////    //       body:
                ////    //           {"name":"Abc",
                ////    //           "dob":"11/11/1997",
                ////    //           "gender":"F",
                ////    //           "biometricdeviceResponse":finalResponse ,
                ////    //           "aadhaarNumber":"123456789012"
                ////    //               };

                ////    var body = new
                ////    {
                ////        name = Lblname.Text,
                ////        //dob = "27/11/1974",
                ////        dob = Convert.ToDateTime(Label2.Text).ToString("yyyy-MM-dd"),
                ////        gender = Label1.Text.Substring (1,1),
                ////        biometricDeviceResponse = finalResponse,
                ////        aadhaarNumber = lblAadhaar.Text
                ////        //name = "Santosh Bhardwaj",
                ////        ////dob = "27/11/1974",
                ////        //dob = "1974-11-27",
                ////        //     gender="F",
                ////        //     biometricDeviceResponse=finalResponse ,
                ////        //aadhaarNumber = "206927461266"
                ////    };

                ////    string json_data = JsonConvert.SerializeObject(body);

                ////    //Print the Json object
                ////    // ShowAlert(json_data);
                ////    //string result = GetJSON ("Santosh Bhardwaj","27/11/1974","F",finalResponse ,"206927461266");
                ////    var client = new RestClient("https://sp.epramaan.in:4003/nielitwebservice/validate");
                ////    var request1 = new RestRequest();

                ////    request1.Method = Method.POST;
                ////    //    request1.RequestFormat = RestSharp .DataFormat .Json ;
                ////    request1.AddHeader("Accept", "application/json");

                ////    request1.Parameters.Clear();
                ////    request1.AddParameter("application/json", json_data, ParameterType.RequestBody);

                ////    var response1 = client.Execute(request1);
                ////    var content = response1.Content; // raw content as string  
                ////    ShowAlert(content);
                ////    //var content = "";
                ////    bioReturn biometricReturn = JsonConvert.DeserializeObject<bioReturn>(content);
                ////    ShowAlert(biometricReturn.status);
                ////    bool statusId = false;
                ////    string errorCode = "";
                ////    if (biometricReturn.status == "True")
                ////    {
                ////        statusId = true;
                ////        errorCode = "Null";
                ////    }
                ////    if (biometricReturn.status == "False")
                ////    {
                ////        statusId = false;
                ////        errorCode = biometricReturn.errorCode;
                ////    }

                ////    int result = saveResponse(statusId, biometricReturn.transactionID, errorCode);

                ////}
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Exception :-" + ex.Message);
        }
    }
    void getData()
    {
        //using System.Net.Sockets
        string hostName = "127.0.0.1";
        int hostPort = 11100;
        int response = 0;
        string page = "";

        //For Service Discovery

        //string request = "RDSERVICE / HTTP/1.1\r\n" +
        //                 "Host: 127.0.0.1:11100\r\n" +
        //                 "Content-Length: 0\r\n" +
        //                 "Accept: text/xml\r\n" +
        //                 "\r\n";



        //For Device Info

        /*string request = "DEVICEINFO /getDeviceInfo HTTP/1.1\r\n" +
                      "Host: 127.0.0.1:11100\r\n" +
                      "Content-Length: 0\r\n" +
                      "Accept: text/xml\r\n" +
                      "\r\n";
       */

        //For Capture, Content Length will vary

        string request = "CAPTURE /capture HTTP/1.1\r\n" +
                          "Host: 127.0.0.1:11100\r\n" +
                          "Content-Length: 190\r\n" +
                          "Accept: text/xml\r\n" +
                          "\r\n" +
                          "<PidOptions ver=\"1.0\"><Opts fCount=\"1\" fType=\"0\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"10000\" otp=\"\" wadh=\"\" posh=\"\"/></PidOptions>";


        IPAddress host = IPAddress.Parse(hostName);
        IPEndPoint hostep = new IPEndPoint(host, hostPort);
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        sock.Connect(hostep);

        //string request_url = "http://127.0.0.1/getDeviceInfo";
        response = sock.Send(Encoding.UTF8.GetBytes(request));
        //response = sock.Send(Encoding.UTF8.GetBytes("\r\n"));

        byte[] bytesReceived = new byte[4096];

        int bytes = sock.Receive(bytesReceived, bytesReceived.Length, 0);
        page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
        int index1 = page.IndexOf("<Pid");//171
        int chars = page.Length - index1; // 3411
        finalResponse = page.Substring(index1, chars);
        // Console.WriteLine(page);
        sock.Close();

    }
    public class bioReturn
    {

        public string status { get; set; }
        public string transactionID { get; set; }
        public string errorCode { get; set; }

    };
    protected int saveResponse(bool status, string transID, string errCode)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                aadhaarResponse aadharResp = new aadhaarResponse();
                aadharResp.applicationNo = "Test";
                aadharResp.courseCategoryID = 1;
                aadharResp.courseID = 1;
                aadharResp.status = status;
                aadharResp.transactionID = transID;
                if (errCode != "Null")
                    aadharResp.errorCode = errCode;
                else
                {
                    if (status)
                    {
                        aadharResp.Remarks = "Success";
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            Int32 NielitStudentId = 0;
                            NielitStudentId = Convert.ToInt32(Request.QueryString["Key"]);
                            NielitCentreStudent editCourse = context.NielitCentreStudent.Find(NielitStudentId);
                            editCourse.AadharVerfied = true;
                            context1.SaveChanges();
                        }

                    }
                }
                if (errCode.ToString().StartsWith("REQ") || errCode.ToString().StartsWith("100"))
                    aadharResp.Remarks = "Invalid Name, Date of birth or Gender";
                if (errCode.ToString().StartsWith("SYS"))
                    aadharResp.Remarks = "System Error";
                if (errCode.ToString().StartsWith("998"))
                    aadharResp.Remarks = "Invalid Aadhaar number";

                aadharResp.enterByID = 99;
                aadharResp.enterDate = System.DateTime.Today;

                context.aadhaarResponses.Add(aadharResp);
                context.SaveChanges();
                return (1);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            return (0);
        }
    }
}
