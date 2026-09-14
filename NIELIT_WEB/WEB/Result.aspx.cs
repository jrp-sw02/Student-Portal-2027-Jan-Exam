using System;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Result : BasePage
{

    Table tbl = new Table();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            Lblerror.Visible = false;
			//if (Request.UrlReferrer == null)
            //if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "http://nielit.gov.in"))
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //    Response.End();
            //    return;
            //}
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);

                    RenderPage(courseID);
                    showsidelink(courseID);
                }
                GenerateNewCaptchaImage();
                bindExamYear();
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }

    protected void RenderPage(int courseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(courseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Rdserachby.Items[2].Text = "Search By Registration Number";
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("View Result", "WEB/Result.aspx?ID=" + Request.QueryString["id"], "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("View Result", "WEB/Result.aspx?ID=" + Request.QueryString["id"], "")); }
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Accredited Centre", "FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
                else
                {
                    Rdserachby.Items[2].Text = "Search By Application Number";
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("View Result", "WEB/Result.aspx?" + Request.QueryString, "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("View Result", "WEB/Result.aspx?" + Request.QueryString, "")); }

                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString, "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Form", "FilledForm.aspx?" + Request.QueryString, "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Form Status", "ApplicationStatus.aspx?" + Request.QueryString, "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?" + Request.QueryString, "../images/Print_Admit_Card.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Centre", "FrmAccredetedCentre.aspx?" + Request.QueryString, "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            txtcode.Text = "";
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Arial");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showsidelink(int courseID)
    {
        try
        {
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

                IQueryable<Downloadable> files = context.Downloadables;
                var downloadables = files.Where(s => s.CourseID == courseID).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName })
                                     .Union(files.Where(s => s.CourseCategoryID == ccatId && s.CourseID == null).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName }))
                                     .Union(files.Where(s => s.CourseID == null && s.CourseCategoryID == null).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName }))
                                     .Distinct();

                foreach (var file in downloadables)
                {
                    Sidelink1.Items.Add(new SideLinkItem(file.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + file.FileID.ToString(), "", "_blank"));
                }
                Sidelink1.Render();
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void bindExamYear()
    {
        using (var context = new EConnectContext())
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            ListItem lst = new ListItem("--Select One--", "0");
            var ExamYear = context.Exams.Where(s => s.CourseID == courseID && s.ExamYear > 2012).Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear });
            ExamYear = ExamYear.Distinct().OrderByDescending(p => p.ValueField);
            EConnect.Utils.Common.ControlUtility.BindListObject(Ddlexamyear, ExamYear, lst);
        }
    }
    protected void bindexams()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                int examyear = Convert.ToInt32(Ddlexamyear.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                IQueryable<Exam> exam = context.Exams.Where(s => s.CourseID == courseID && s.ExamYear == examyear && s.DateOfPublishingOfResult != null && s.DateOfPublishingOfResult <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));
                var examname = exam.OrderByDescending(s => s.ExamYear).ThenByDescending(s => s.ExamMonth).Select(s => new { ValueField = s.ID, TextField = s.Name });
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlexamcycle, examname, lst);
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void Btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Lblerror.Visible = false;
            Txtrollno.Text = "";
            TxtDOB.Text = "";

            Ddlexamcycle.SelectedValue = "0";
            GenerateNewCaptchaImage();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Btnback_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../WEB/Result.aspx?" + Request.QueryString);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private DataTable GetData(Int64 currentCourseID, Int64 registrationNumber, Int64 examId)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

      /*  string sql =   " select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
                       " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
                       " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' "+
                       " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  "+
                       " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade, "+
                       " case  when  (cad.Module_ID  not in (929,930,931,932,938,939,940,941,1151,1152,1153,1154)  and Module_Type_ID =1)  then Marks_Total else cad.Theory_marks_out_of_100_new_pattern end as TheoryMarks," +
                       " case  when (cad.Module_ID not in (933,934,935,936,956,957,958,959,1155,1156,1157,1158)  and Module_Type_ID = 3 ) then Marks_Total else cad.practical_marks_out_of_100_new_pattern end as  PracticalMarks,"+
                       " cad.Marks_Total as WeightedMarks from " +
                       " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
                       " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
                       " and rg.Code <>'$'and cad.Module_ID not in (933,934,935,936,956,957,958,959,1155,1156,1157,1158)   and cad.Registration_Number ='" + registrationNumber + "'" +
                       "  and cad.Exam_ID = '" + examId + "'  and cad.Course_ID ='" + currentCourseID + "' " +
                       " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";*/

 string sql =
                        " SELECT cad.Course_ID CourseID " +
                        " ,m.id AS ID " +
                        " ,upper(m.Name) AS name " +
                        " ,m.Short_Name AS Code " +
                        " ,m.Module_Type_ID AS ModuleTypeID " +
                        " ,m.Selection_Type_ID AS SelectionTypeID " +
                        " ,m.Elective_Group AS ElectiveGroup " +
                        " ,mt.Name + ( " +
                        " CASE " +
                        " WHEN m.Module_Type_ID = 1 " +
                        " AND m.Selection_Type_ID = 1 " +
                        " AND Sub_Number = 0 " +
                        " THEN '(Comp.)' " +
                        " WHEN m.Module_Type_ID = 1 " +
                        " AND m.Selection_Type_ID > 1 " +
                        " AND Sub_Number > 0 " +
                        " THEN '(Elect.)' " +
                        " ELSE '' " +
                        " END " +
                        " ) AS MType " +
                        " ,isnull(e.Name, 'NA') AS doexam " +
                        " ,rg.Description AS Result " +
                        " ,rg.Code AS Grade " +
                        " ,CASE " +
                        " WHEN ( isnull(m.TheoryWithPractical,0) =0 " +
                        " AND " +
                        " Module_Type_ID = 1 " +
                        " ) " +
                        " THEN cad.Marks_Total " +
                        " ELSE cad.Theory_marks_out_of_100_new_pattern " +
                        " END AS TheoryMarks " +
                        " ,CASE " +
                        " WHEN ( " +
                        " isnull(m.TheoryWithPractical,0) =0 " +
                        " AND Module_Type_ID = 3 " +
                        " ) " +
                        " THEN cad.Marks_Total " +
                        " ELSE cad.practical_marks_out_of_100_new_pattern " +
                        " END AS PracticalMarks " +
                        " ,cad.Marks_Total AS WeightedMarks " +
                        " FROM Module_Type mt " +
                        " ,module m " +
                        " ,Result_Grading rg " +
                        " ,Course_Exam_Application_Detail cad " +
                        " LEFT OUTER JOIN exam e ON cad.Exam_ID = e.id " +
                        " WHERE cad.Module_ID = m.id " +
                        " AND cad.Result_Grade_ID = rg.id " +
                        " AND m.Module_Type_ID = mt.ID " +
                        " AND rg.Code <> '$' " +
                        " and  ( iif " +
                        " (isnull(m.TheoryWithPractical,0)<>0 " +
                        " and m.Module_Type_ID =3 " +
                        " ,0,1)=1 " +
                        " ) " +
                        " " +
                        " AND cad.Registration_Number = '" + registrationNumber + "' " +
                        " AND cad.Exam_ID = '" + examId + "' " +
                        " AND cad.Course_ID = '" + currentCourseID + "' " +
                        " ORDER BY m.Module_Type_ID " +
                        " ,m.Selection_Type_ID " +
                        " ,m.ID ";




        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }

    protected void Btnview_Click(object sender, EventArgs e)
    {
        var context = new EConnectContext();
        try
        {
            BreadCrumb1.Render();
            Int32 searchBy = 0;
            if (Ddlexamcycle.SelectedValue == "0")
                throw new Exception("Please select Examination Name");
            if (Rdserachby.SelectedValue == "1")
            {
                if (String.IsNullOrWhiteSpace(Txtrollno.Text))
                    throw new Exception("Please enter Roll Number.");
                searchBy = 1;//Search by roll number.
            }
            else if (Rdserachby.SelectedValue == "2")
            {
                if (String.IsNullOrWhiteSpace(Txtrollno.Text))
                    throw new Exception("Please enter Candidate's Name.");
                searchBy = 2;//Search by cnadidate name.
            }
            else
            {

                Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                if (context.Courses.Find(courseID).enmCourseType == enmCourseType.CertificationCourse)
                {
                    if (String.IsNullOrWhiteSpace(Txtrollno.Text))
                        throw new Exception("Please enter Registration Number.");
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(Txtrollno.Text))
                        throw new Exception("Please enter Application Number.");
                }

                searchBy = 3;//Search by cnadidate name.
            }
            if (String.IsNullOrWhiteSpace(TxtDOB.Text.Trim()))
                throw new Exception("Please enter Candidate's Date of Birth.");
            if (!IsDate(TxtDOB.Text))
                throw new Exception("Invalid Date of Birth of the candidate");
            if (String.IsNullOrWhiteSpace(txtcode.Text.Trim()))
                throw new Exception("Please enter Captcha Code shown in image below.");
            string appType = Convert.ToString(enmApplicantType.Direct);
            if (txtcode.Text.Trim() == ViewState["CaptchCode"].ToString())
            {

                Int32 CourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                Int32 examID = Convert.ToInt32(Ddlexamcycle.SelectedValue);
                DateTime DOB = Convert.ToDateTime(TxtDOB.Text.ToString());
                Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"].ToString()));

                Lblcourse.Text = GetInitCap("RESULT CARD FOR") + " " + GetInitCap(currentCourse.Name);

                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    string searchText = Txtrollno.Text.Trim();

                    //Added code for message
                    var stuDet = (from a in context.CertificateExamApplications
                                  where (searchBy == 1 ? a.RollNumber.Equals(searchText, StringComparison.OrdinalIgnoreCase) : (searchBy == 2 ? a.Name.Equals(searchText, StringComparison.OrdinalIgnoreCase) : a.Number.Equals(searchText, StringComparison.OrdinalIgnoreCase)))
                                  && a.ExamID == examID && a.DateOfBirth == DOB
                                  select new
                                  {
                                      name = a.Name,

                                  }).FirstOrDefault();

                    if (stuDet == null)
                        throw new Exception("Invalid Exam Name, Roll Number, Date of Birth.");
                    //

                    var result = (from a in context.CertificateExamApplications
                                  where (searchBy == 1 ? a.RollNumber.Equals(searchText, StringComparison.OrdinalIgnoreCase) : (searchBy == 2 ? a.Name.Equals(searchText, StringComparison.OrdinalIgnoreCase) : a.Number.Equals(searchText, StringComparison.OrdinalIgnoreCase)))
                                  && a.ExamID == examID && a.DateOfBirth == DOB && a.ResultGradeID != null
                                  select new
                                  {
                                      name = a.Name,
                                      Appid = a.ID,
                                      fname = a.FatherName,
                                      mname = a.MotherName,
                                      gname = a.GuardianName,
                                      doe = a.DateOfExam,
                                      rollno = a.RollNumber,
                                      examname = a.Exam.Name,
                                      studresult = a.ResultGrade,
                                      resultDate = a.ResultUpdatedOn,
                                      apptypeId = a.ApplicantTypeID,
                                      instid = a.InstituteID,
                                      instname = a.Institute.Name,
                                      examcentre = a.ExamCentreName,
                                      examcyclename = a.Exam.ExaminationCycle.Name,
                                      courcecode = a.Course.Code,
                                      courcecateId = a.CourseCategoryID,
                                      courseid = a.CourseID,
                                      examID = a.ExamID
                                  }).FirstOrDefault();
                    if (result != null)
                    {
                        if (result.studresult != null)
                        {
                            divfilter.Visible = false;
                            divresult.Visible = true;
                            divfooter.Visible = true;
                            tbsearch.Visible = false;
                            Lblexamdate.Text = result.doe.HasValue ? result.doe.Value.ToString("dd-MMM-yyyy") : " NA ";
                            LblRollno.Text = (!String.IsNullOrEmpty(result.rollno)) ? result.rollno.ToString() : " NA ";
                            Lblname.Text = GetInitCap(result.name);
                            if (string.IsNullOrEmpty(result.gname) == true && string.IsNullOrWhiteSpace(result.gname) == true)
                            {
                                trfathername.Visible = true;
                                trmothername.Visible = true;
                                trgname.Visible = false;
                                if (string.IsNullOrEmpty(result.fname) == false && !string.IsNullOrWhiteSpace(result.fname))
                                    Lbfname.Text = "Mr. " + GetInitCap(result.fname);
                                else
                                    Lbfname.Text = "NA";
                                if (string.IsNullOrEmpty(result.mname) == false && string.IsNullOrWhiteSpace(result.mname) == false)
                                    Lbmname.Text = "Mrs. " + GetInitCap(result.mname);
                                else
                                    Lbmname.Text = "NA";
                            }
                            else
                            {
                                Lgname.Text = String.IsNullOrWhiteSpace(result.gname) == false ? GetInitCap(result.gname) : "NA";
                                trfathername.Visible = false;
                                trmothername.Visible = false;
                                trgname.Visible = true;
                            }
                            if (result.apptypeId == Convert.ToInt32(enmApplicantType.Institute))
                            {
                                if (result.instid.HasValue)
                                {
                                    string AccreditationNumber = context.AccreditationDetails.Where(s => s.InstituteID == result.instid && s.CourseID == currentCourse.ID).FirstOrDefault().AccreditationNumber;
                                    if (AccreditationNumber != null)
                                    {
                                        Lblcc.Text = AccreditationNumber.ToUpper();
                                        Lbliname.Text = GetInitCap(result.instname);
                                    }
                                    else
                                    {
                                        Lblcc.Text = "NA";
                                        Lbliname.Text = "NA";
                                    }
                                }
                                else
                                {
                                    Lblcc.Text = "NA";
                                    Lbliname.Text = "NA";
                                }
                            }
                            else
                            {
                                Lblcc.Text = appType;
                                Lbliname.Text = "--";
                            }

                            Lblexam.Text = GetInitCap(result.examname) + "(" + result.courcecode.ToUpper() + " - " + GetInitCap(result.examcyclename) + ")";
                            var grade = result.studresult;
                             if (result.courcecateId != 1 || result.courcecateId != 6)
                            {
                                trResultGrade.Visible = true;
                                if (grade.IsPassed.ToString() == "True")
                                    Lblresult.Text = grade.Code + (grade.IsPassed.ToString() == "True" ? " (Pass) " : "(" + grade.Description + ")");
                                else
                                    Lblresult.Text = grade.Code + (grade.IsPassed.ToString() == "True" ? " (Pass) " : "(" + grade.Description + ")");
                            }
                            LblResultDate.Text = result.resultDate.GetValueOrDefault().ToString("dd-MMM-yyyy");

                            Lblexamcentre.Text = (!String.IsNullOrEmpty(result.examcentre)) ? result.examcentre : "NA";
                            Btnprint.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ViewResult.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + result.Appid + "&ExamId=" + result.examID) + "','Form'); return false;");
                            Rdserachby.Visible = false;
                            trlegends.Visible = true;
                            trlegends1.Visible = true;
                            trcoursename.Visible = false;
                            ShowLegendsCertificate(CourseID, currentCourse.CourseCategoryID, examID);
                        }
                        else
                           // throw new Exception("Result not found. Invalid Exam Name, Roll Number, Date of Birth.");
                            //Modified
                            //throw new Exception("Result not found. Invalid Exam Name, Roll Number, Date of Birth.");
                            throw new Exception("Result not uploaded as yet.");


                    }
                    else
                       // throw new Exception("Result not found. Invalid Exam Name, Roll Number, Date of Birth.");
                        //Modified
                        // throw new Exception("Result not found. Invalid Exam Name, Roll Number, Date of Birth.");
                        throw new Exception("Result not uploaded as yet.");
                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Int64 rollNo = 0;
                    string name = "";
                    Int64 registrationNumber = 0;
                    if (searchBy == 1)
                    {
                        if (!IsNumeric(Txtrollno.Text.Trim()))
                        {
                            throw new Exception("Invalid Roll Number. Please enter numeric value only");
                        }
                        rollNo = Convert.ToInt64(Txtrollno.Text);
                    }
                    else if (searchBy == 2)
                    {
                        searchBy = 2;//Search by cnadidate name.
                        name = Txtrollno.Text.Trim().ToUpper();
                    }
                    else
                    {
                        if (!IsNumeric(Txtrollno.Text.Trim()))
                        {
                            throw new Exception("Invalid Registration Number. Please enter numeric value only");
                        }
                        registrationNumber = Convert.ToInt64(Txtrollno.Text);
                    }

                    //Added code for message
                    var stuDet = (from c in context.CourseExamApplicationDetails
                                  where (searchBy == 1 ? c.RollNumber == rollNo : (searchBy == 2 ? c.Candidate.Name.ToUpper() == name : c.RegistrationNumber == registrationNumber))
                                   && c.CourseID == CourseID && c.ExamID == examID && c.Candidate.DateOfBirth == DOB
                                  select new
                                  {
                                      name = c.Candidate.Name,

                                  }).FirstOrDefault();


                    if (stuDet == null)
                        throw new Exception("Invalid Exam Name, Roll Number, Date of Birth.");
                    //


                    var result = (from c in context.CourseExamApplicationDetails
                                  where (searchBy == 1 ? c.RollNumber == rollNo : (searchBy == 2 ? c.Candidate.Name.ToUpper() == name : c.RegistrationNumber == registrationNumber))
                                  && c.ResultGradeID != null && c.CourseID == CourseID && c.ExamID == examID && c.Candidate.DateOfBirth == DOB
                                  select new
                                  {
                                      name = c.Candidate.Name,
                                      Appid = c.ID,
                                      RegistrationNumber = c.RegistrationNumber,
                                      fname = c.Candidate.FatherName,
                                      mname = c.Candidate.MotherName,
                                      gname = c.Candidate.GuardianName,
                                      ResultDeclaredOn = c.Exam.DateOfPublishingOfResult,
                                      rollno = c.RollNumber,
                                      examname = c.Exam.Name,
                                      salutation = c.Candidate.Salutation,
                                      remark = (!string.IsNullOrEmpty(c.carryforwardremarks)) ? c.carryforwardremarks : "N/A"
                                  }).FirstOrDefault();

                    if (result != null)
                    {
                        divfilter.Visible = false;
                        divresult.Visible = true;
                        divfooter.Visible = true;
                        trExamDate.Visible = false;
                        trCentreName.Visible = false;
                        trCCCName.Visible = false;
                        trInstituteName.Visible = false;
                        LblRollno.Text = result.rollno.ToString();
                        registrationNumber = result.RegistrationNumber;
                        Lblname.Text = result.salutation + " " + GetInitCap(result.name.ToUpper());
                        if (string.IsNullOrEmpty(result.gname) == true && string.IsNullOrWhiteSpace(result.gname) == true)
                        {
                            trfathername.Visible = true;
                            trmothername.Visible = true;
                            trgname.Visible = false;
                            if (string.IsNullOrEmpty(result.fname) == false && !string.IsNullOrWhiteSpace(result.fname))
                                Lbfname.Text = "Mr. " + GetInitCap(result.fname);
                            else
                                Lbfname.Text = "NA";
                            if (string.IsNullOrEmpty(result.mname) == false && string.IsNullOrWhiteSpace(result.mname) == false)
                                Lbmname.Text = "Mrs. " + GetInitCap(result.mname);
                            else
                                Lbmname.Text = "NA";
                        }
                        else
                        {
                            Lgname.Text = string.IsNullOrWhiteSpace(result.gname) == false ? GetInitCap(result.gname) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trgname.Visible = true;
                        }
                        Lblexam.Text = result.examname.ToUpper();
                        //Lblresult.Text = "Declared on " + result.ResultDeclaredOn.Value.ToString("dd-MMM-yyyy");
                        LblResultDate.Text = "Declared on " + result.ResultDeclaredOn.Value.ToString("dd-MMM-yyyy");
                        lblRemark.Text = result.remark;
                        Btnprint.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ViewCourseResult.aspx?ID=" + Request.QueryString["id"].ToString() + "&RegNo=" + result.RegistrationNumber + "&ExamId=" + examID) + "','Form'); return false;");
                        Rdserachby.Visible = false;
                        trlegends.Visible = false;
                        trlegends1.Visible = false;
                        trcoursename.Visible = true;
                        Lblccname.Text = GetInitCap(currentCourse.Name) + " (" + currentCourse.CourseCategory.Name + ")";
                        //var modulesresult = (from c in context.CourseExamApplicationDetails
                        //                     where c.RegistrationNumber == registrationNumber && c.ResultGradeID != null && c.CourseID == CourseID && c.ExamID == examID
                        //                     && c.ModuleID != 933 && c.ModuleID != 934 && c.ModuleID != 935 && c.ModuleID != 936 && c.ModuleID != 956 && c.ModuleID != 957 && c.ModuleID != 958 && c.ModuleID != 959
                        //                     //not in (933, 934, 935, 936, 956, 957, 958, 959)
                        //                     orderby c.ModuleID
                        //                     select new
                        //                     {
                        //                         ID = c.ID,
                        //                         name = c.Module.Name,
                        //                         Code = c.Module.ShortName,
                        //                         MType = c.Module.ModuleType.Name + (c.Module.ModuleTypeID == 1 ? (c.Module.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                        //                         Result = c.Grade.Description,
                        //                         Grade = c.Grade.Code
                        //                     });
                        var modulesresult = GetData(CourseID, registrationNumber, examID);
                        gvMain.DataSource = modulesresult;
                        gvMain.DataBind();
                        gvMain.Visible = true;
                        tblResult.Visible = true;
                    }
                    else
                       // throw new Exception("Result not found. Invalid Exam Name, Roll Number, Date of Birth.");
                        //Modified
                        //  throw new Exception("Result not found. Invalid Exam Name, Roll Number, Date of Birth.");
                        throw new Exception("Result not uploaded as yet.");
                }

            }
            else
                throw new Exception("Invalid Captcha Code");
        }
        catch (Exception ex)
        {
            gvMain.Visible = false;
            GenerateNewCaptchaImage();
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
        finally { context.Dispose(); }

    }
    protected void Rdserachby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdserachby.SelectedValue == "1")
            {
                tdfilter.InnerText = "Enter Roll Number";
                spfilter.InnerText = "(Roll No. printed on your admit card)";
                Txtrollno.Text = "";

                TxtDOB.Text = "";
                GenerateNewCaptchaImage();
            }
            else if (Rdserachby.SelectedValue == "2")
            {
                tdfilter.InnerText = "Enter Candidate Name";
                spfilter.InnerText = "(Name of the candidate)";
                Txtrollno.Text = "";

                TxtDOB.Text = "";
                GenerateNewCaptchaImage();
            }
            else
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                using (EConnectContext context = new EConnectContext())
                {
                    if (context.Courses.Find(courseID).enmCourseType == enmCourseType.CertificationCourse)
                    {
                        tdfilter.InnerText = "Enter Registration Number";
                        spfilter.InnerText = "(Registration Number of Selected Course)";
                    }
                    else
                    {
                        tdfilter.InnerText = "Enter Application Number";
                        spfilter.InnerText = "(Application Number of Exam Form)";
                    }
                }
                Txtrollno.Text = "";

                TxtDOB.Text = "";
                GenerateNewCaptchaImage();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowLegendsCertificate(Int32 courseID, Int32 CourseCategoryID, Int32 ResultExamID)
    {

        try
        {
            tbl.Width = Unit.Percentage(100);
            tbl.CellSpacing = 0;
            tbl.BorderColor = System.Drawing.Color.Black;
            tbl.BorderWidth = Unit.Pixel(1);
            tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;

            string[] heading = { "Grade", "Marks Range (in %)", "Remarks" };
            using (EConnectContext context = new EConnectContext())
            {
                Int32[] notInGrades = { 24 };
                Int32 ResultVersionID = context.Exams.Where(s => s.ID == ResultExamID).FirstOrDefault().ResultGradeVersionID.Value;

                IQueryable<ResultGrade> grade = context.ResultGrades.Where(s => s.CourseCategoryID == CourseCategoryID && s.VersionID == ResultVersionID && !notInGrades.Contains(s.ID)).OrderBy (s=>s.Code );
                if (courseID != 5 && CourseCategoryID == 2) //BCC
                { grade = grade.Where(s => s.Code != "E"); }

                if (grade.Count() >= 0)
                {
                    int rowCounter = 0;
                    for (rowCounter = 0; rowCounter < 3; rowCounter++)
                    {
                        TableRow tr = new TableRow();

                        tbl.Rows.Add(tr);

                        TableCell tdHeading = new TableCell();
                        tdHeading.Text = heading[rowCounter].ToString();
                        tdHeading.HorizontalAlign = HorizontalAlign.Left;
                        tdHeading.Font.Bold = true;
                        tdHeading.Width = Unit.Percentage(20);
                        tdHeading.BorderColor = System.Drawing.Color.Black;
                        tdHeading.BorderWidth = Unit.Pixel(1);
                        tdHeading.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(tdHeading);
                        foreach (var result in grade)
                        {
                            TableCell tdGrade = new TableCell();
                            tdGrade.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdGrade);
                            tdGrade.Font.Bold = false;
                            tdGrade.BorderColor = System.Drawing.Color.Black;
                            tdGrade.BorderWidth = Unit.Pixel(1);
                            if (rowCounter == 0)
                            {

                                tdGrade.Font.Bold = true;
                                tdGrade.Text = result.Code.ToUpper();
                            }
                            else if (rowCounter == 1)
                            {
                                if (result.PercentageFrom > 0 && result.PercentageTo > 0)
                                    tdGrade.Text = result.PercentageFrom + " to " + result.PercentageTo;
                                else
                                    tdGrade.Text = result.Code;
                            }
                            else
                                tdGrade.Text = (result.Description);
                            tdGrade.VerticalAlign = VerticalAlign.Top;
                        }
                    }
                }
                tdLegends.Controls.Add(tbl);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showTableheader()
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(20);
            tcCol.Text = "Grade";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(30);
            tcCol2.Text = "Marks(%)";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(50);
            tcCol1.Text = "Result";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            tbl.Rows.Add(th);
        }
        catch (Exception ex) { ShowAlert(ex.Message); }

    }
    protected void Ddlexamyear_SelectedIndexChanged(object sender, EventArgs e)
    { bindexams(); }

    //protected void ShowLegendsCourse()
    //{
    //    try
    //    {
    //        tbl.Width = Unit.Percentage(100);
    //        tbl.CellSpacing = 0;
    //        tbl.BorderColor = System.Drawing.Color.Black;
    //        tbl.BorderWidth = Unit.Pixel(1);
    //        tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
    //        //showTableheader();
    //        int i = 1;
    //        string[] heading = { "Grade", "Marks Range (in %)", "Remarks" };
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32[] notInGrades = { 8, 10 };
    //            var grade = (from g in context.ResultGrades
    //                         where g.CourseCategoryID == 1 && !notInGrades.Contains(g.ID)
    //                         orderby g.Code
    //                         select new
    //                         {
    //                             grade = g.Code,
    //                             description = g.Description,
    //                             legend1 = g.PercentageFrom,
    //                             legend2 = g.PercentageTo
    //                         }).ToList();
    //            if (grade.Count() >= 0)
    //            {
    //                int rowCounter = 0;
    //                for (rowCounter = 0; rowCounter < 3; rowCounter++)
    //                {
    //                    TableRow tr = new TableRow();
    //                    //if (i % 2 == 0)
    //                    //    tr.CssClass = "normal";
    //                    //else
    //                    //    tr.CssClass = "normal";
    //                    //i++;
    //                    tbl.Rows.Add(tr);

    //                    TableCell tdHeading = new TableCell();
    //                    tdHeading.Text = heading[rowCounter].ToString();
    //                    tdHeading.HorizontalAlign = HorizontalAlign.Left;
    //                    tdHeading.Font.Bold = true;
    //                    tdHeading.Width = Unit.Percentage(20);
    //                    tdHeading.BorderColor = System.Drawing.Color.Black;
    //                    tdHeading.BorderWidth = Unit.Pixel(1);
    //                    tr.Cells.Add(tdHeading);

    //                    foreach (var result in grade)
    //                    {
    //                        TableCell tdGrade = new TableCell();
    //                        tdGrade.HorizontalAlign = HorizontalAlign.Center;
    //                        tr.Cells.Add(tdGrade);
    //                        tdGrade.Font.Bold = false;
    //                        tdGrade.BorderColor = System.Drawing.Color.Black;
    //                        tdGrade.BorderWidth = Unit.Pixel(1);
    //                        if (rowCounter == 0)
    //                        {

    //                            tdGrade.Font.Bold = true;
    //                            tdGrade.Text = result.grade.ToUpper();
    //                        }
    //                        else if (rowCounter == 1)
    //                        {
    //                            if (result.legend1 != null && result.legend1 > 0 && result.legend2 != null && result.legend2 > 0)
    //                                tdGrade.Text = result.legend1 + " to " + result.legend2;
    //                            else
    //                                tdGrade.Text = "-";
    //                        }
    //                        else
    //                            tdGrade.Text = (result.description);

    //                    }

    //                }
    //            }
    //            tdLegends.Controls.Add(tbl);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //protected Int32 GetCurrentExamID(Int32 applicantTypeID, Int32 courseID)
    //{
    //    try
    //    {
    //        Int32 ExamID = 0;
    //        Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
    //        Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
    //        if (courseID != 0)
    //        {
    //            using (EConnectContext context = new EConnectContext())
    //            {

    //                var LateFeeExam = (from e in context.CutOffDates
    //                                   join i in context.Exams on e.ExamID equals i.ID
    //                                   where e.CourseID == courseID
    //                                   && e.ApplicantTypeID == applicantTypeID
    //                                   && e.ActivityID == LateFeeActivityId
    //                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                                   orderby e.EfferctiveDate ascending
    //                                   select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
    //                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
    //                {

    //                    if (LateFeeExam != null)
    //                    {
    //                        ExamID = LateFeeExam.FirstOrDefault().ExamID;
    //                    }
    //                }
    //                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
    //                {

    //                    var NormalFeeExam = (from e in context.CutOffDates
    //                                         join i in context.Exams on e.ExamID equals i.ID
    //                                         where e.CourseID == courseID
    //                                         && e.ApplicantTypeID == applicantTypeID
    //                                         && e.ActivityID == NormalactivityId
    //                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                                         orderby e.EfferctiveDate ascending
    //                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
    //                    if (NormalFeeExam.Count() > 0)
    //                    {
    //                        ExamID = NormalFeeExam.FirstOrDefault().ExamID;
    //                    }
    //                }

    //            };
    //        }
    //        return ExamID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //protected Int32 GetNextExamID(Int32 applicantTypeID, Int32 courseID, Int32 examCycleID)
    //{
    //    try
    //    {
    //        Int32 examID = 0;
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            int StartDateofFormFilling = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
    //            var examsAll = (from f in context.Exams
    //                            where f.CourseID == courseID && f.ExaminationCycleID == examCycleID &&
    //                            f.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && f.DateOfPublishingOfRollNumber == null
    //                            select f);

    //            examsAll = examsAll.Where(a => a.CutOffDates.Where(k => k.ActivityID == StartDateofFormFilling && k.ApplicantTypeID == applicantTypeID).FirstOrDefault().EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));

    //            if (examsAll.Count() > 0)
    //            {
    //                int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
    //                int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
    //                var exmasWithNormalLastDate = (from t in context.CutOffDates
    //                                               where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
    //                                               t.ActivityID == NormalFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                                               orderby t.Exam.ExamStartDate
    //                                               select t.Exam).Distinct();
    //                if (exmasWithNormalLastDate.Count() == 0)
    //                {
    //                    var exmasWithLateFeeLastDate = (from t in context.CutOffDates
    //                                                    where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
    //                                                    t.ActivityID == LateFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                                                    orderby t.Exam.ExamStartDate
    //                                                    select t.Exam).Distinct();
    //                    examID = exmasWithLateFeeLastDate.Select(c => c.ID).FirstOrDefault();
    //                }
    //                else
    //                {
    //                    examID = exmasWithNormalLastDate.Select(c => c.ID).FirstOrDefault();
    //                }
    //            }
    //            else
    //                examID = examsAll.Select(c => c.ID).FirstOrDefault();
    //        };
    //        return examID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}