using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class FrmMycourse : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    string CourseCatId = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lblError.Text = "";
            lblError.Visible = false;
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                Showdata();
            }
            using (EConnectContext context = new EConnectContext())
            {
                String CourseCatId = context.RegistrationDetails.Where(c => c.CandidateID == entityID).Select(s => s.CourseCategory.Name).FirstOrDefault();
                if (CourseCatId != "IRDA") //IRDA
                {
                    SideLink2.Visible = true;
                    SideLink2.Items.Add(new SideLinkItem("Apply For New Registration", "../CAND/CurrentRegistrationStatus.aspx", "../images/Apply_Online.jpg"));                  
                    SideLink2.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;                    
                    SideLink2.Render();

                    showsidelink();
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Showdata()
    {
        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("My Courses", "CAND/FrmMycourse.aspx", ""));
            BreadCrumb1.Render();

            StringBuilder sb = new StringBuilder();
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from c in context.RegistrationDetails
                                 where c.CandidateID == entityID
                                 orderby c.CommencementFromDate descending
                                 select new
                                 {

                                     Regno = c.RegistrationNo,
                                     RegDate = c.RegistrationDate,
                                     RegComDate = c.CommencementFromDate,
                                     Validity = c.ValidUptoDate,
                                     Cname = c.Course.Name,
                                     RegStatus = c.RegistrationStatus.Name,
                                     candtype = c.ApplicantTypeID != 0 ? c.ApplicantType.Name : "NA",
                                     cid = c.CourseID,
                                     courseCategoryId = c.CourseCategoryID
                                 });

                Repeater1.DataSource = candidate.ToList();
                Repeater1.DataBind();
            };
        }
        catch (Exception ex)
        { throw ex; }
    }   
    protected void showsidelink()
    {
        try
        {
            SideLink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                var courselist = (from rg in context.RegistrationDetails
                                  where rg.CandidateID == entityID
                                  orderby rg.CourseID ascending
                                  select rg.CourseID).ToArray();

                for (int i = 0; i < courselist.Length; i++)
                {
                    Int32 courseId = Convert.ToInt32(courselist[i]);
                    var dl = from d in context.Downloadables
                             where d.CourseID == courseId && d.ShowOnWeb == true
                             select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };

                    foreach (var dnbl in dl.Distinct())
                    {
                        SideLink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                    }
                    SideLink1.Render();
                }
                Int32 ccatId =  context.RegistrationDetails.Where(c => c.CandidateID == entityID).Select(s => s.CourseCategoryID).FirstOrDefault();

                var d2 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == ccatId && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName });
                var d3 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == null && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(d2);

                foreach (var dnbl in d3.Distinct())
                {
                    SideLink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                }
                SideLink1.Render();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool IsEligibleForAutoupgradation(long candidateID)//status on Search Button
    {
        try
        {
            bool isAutoupgraded = false;
            using (EConnectContext context = new EConnectContext())
            {
                var currentCourse = (from c in context.RegistrationDetails
                                     where c.CandidateID == candidateID
                                     orderby c.CommencementFromDate descending
                                     select c).FirstOrDefault();
                if (currentCourse != null && currentCourse.enmApplicantType == enmApplicantType.Institute)
                {
                    Int32 courseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                    var courseid = (from p in context.Courses
                                    where p.CourseCategoryID == currentCourse.CourseCategoryID && p.CourseTypeID == courseTypeID && p.CourseRegistrationPolicies.FirstOrDefault().RegistrationValidity != null
                                    orderby p.LowerCourseID descending
                                    select new
                                    {
                                        ID = p.ID
                                    }).FirstOrDefault();

                    if (currentCourse.CourseID != courseid.ID)
                    {
                        if (currentCourse.enmRegistrationStatus == enmRegistrationStatus.Completed)
                        {
                            Exam previousExams = CourseManager.GetListOfAttemptedExams(context, currentCourse.CourseID, currentCourse.RegistrationNo, candidateID).OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                            int applicantTypeId = Convert.ToInt32(enmApplicantType.Institute);
                            Exam nextExam = CourseManager.GetNextExam(context, currentCourse.CourseID, applicantTypeId);
                            if (previousExams != null && nextExam != null)
                            {
                                if (previousExams.ExamYear == nextExam.ExamYear)
                                {
                                    if (previousExams.ExamMonth < nextExam.ExamMonth)
                                        isAutoupgraded = true;
                                }
                                else if (previousExams.ExamYear < nextExam.ExamYear)
                                    isAutoupgraded = true;
                            }

                        }
                    }
                }
                return isAutoupgraded;
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool ProjectPending(long candidateID)//check whether candidate project is pending or not
    {
        try
        {
            bool isProjectPending = false;
            Int32 projectpending = Convert.ToInt32(enmRegistrationStatus.ProjectPending);
            using (EConnectContext context = new EConnectContext())
            {
                var currentCourse = (from c in context.RegistrationDetails
                                     where c.CandidateID == candidateID
                                     orderby c.CommencementFromDate descending
                                     select c).FirstOrDefault();
                if (currentCourse.RegistrationStatusID == projectpending)
                {
                    isProjectPending = true;
                }
                return isProjectPending;
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemIndex >= 0)
            {
                Label lblCourse = (Label)e.Item.FindControl("Lbcid");
                Label lblRegNo = (Label)e.Item.FindControl("lbRegNo");
                Int32 courseID = Convert.ToInt32(lblCourse.Text);
                Int64 regNo = Convert.ToInt32(lblRegNo.Text);
                if (CourseCatId != "IRDA")
                {
                    LinkButton lb = (LinkButton)e.Item.FindControl("btnViewDetail2");
                    if (lb != null)
                    {
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmViewDetail.aspx?CourseID=" + courseID + "&RegNo=" + regNo), false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}