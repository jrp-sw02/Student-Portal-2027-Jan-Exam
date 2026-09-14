using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Web.Security;
using EConnect.Utils.Common;


public partial class UserControl_FormStatus : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (ViewState["CourseID"] == null)
                ViewState["CourseID"] = "0";
            if (ViewState["AppID"] == null)
                ViewState["AppID"] = "0";
            if (ViewState["DOB"] == null)
                ViewState["DOB"] = "";

        }

        Lbldate.Text = "Date:-" + DateTime.Now.ToString("dd-MMM-yyyy");
    }
          

    public Int32 CourseID
    {
        set
        {
            ViewState["CourseID"] = value.ToString();
        }
        get
        {
            return Convert.ToInt32(ViewState["CourseID"]);
        }
    }
    public Int64 AppId
    {
        set
        {
            ViewState["AppId"] = value.ToString();
        }
        get
        {
            return Convert.ToInt64(ViewState["AppId"]);
        }
    }
    public DateTime DOB
    {
        set
        {
            ViewState["DOB"] = value.ToString();
        }
        get
        {
            return Convert.ToDateTime(ViewState["DOB"]);
        }
    }
    public void RenderPage()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //string tt = Convert.ToString(Session["ModuleID"]);
                Int32 currentCourseID = CourseID;
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
              
                    Lblcourse.Text = "STATUS OF ONLINE REGISTRATION APPLICATION FOR" + " " + currentCourse.Name.ToUpper();
                    Lblccname.Text = currentCourse.CourseCategory.Name + "-" + "(" + currentCourse.Name + ")";
                    CourseRegistrationApplication app = new CourseRegistrationApplication();
                    var application = (from a in context.CourseRegistrationApplications join c in context.Candidates
                                       on a.CandidateID equals c.ID
                                       where a.ID == AppId && c.DateOfBirth == DOB && a.CourseID == CourseID
                                       select new {
                                        appdate = a.ApplicationDate,
                                        appno = a.ID,
                                        dob = c.DateOfBirth,
                                        name = c.Name,
                                        fname= c.FatherName,
                                        mname= c.MotherName,
                                        photo = c.Photo,
                                        salutation= c.Salutation,
                                        status = a.PaymentStatus.Name
                                       }).FirstOrDefault();
                   
                    if (application!=null)
                    {
                        //divdata.Visible = false;
                        divfull.Visible = true;
                        divStatus.Visible = true;
                        LblAppdate.Text = application.appdate.ToString("dd-MMM-yyyy");
                        LblAppno.Text = application.appno.ToString();
                        LblDOB.Text = application.dob.ToString("dd-MMM-yyyy");
                        Lblname.Text = application.salutation + " " + application.name.ToUpper();
                        Lbfname.Text = "Mr. " + application.fname.ToUpper();
                        Lbmname.Text = "Mrs." + application.mname.ToUpper();
                        imgcandphoto.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.photo.BlobFile);
                        Lblss.Text = application.status;
                        trexcycle.Visible = false;
                        
                    }
                    else
                    {
                        //Lblerror.Text = "Invalid Application No. or Date of Birth";
                        divStatus.Visible = false;
                        divfull.Visible = true;
                       
                        
                    }
    
                   
                }

                else if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    Lblcourse.Text = "STATUS OF ONLINE EXAMINATION APPLICATION FOR " + " " + currentCourse.Code ;
                    Lblccname.Text =  currentCourse.CourseCategory.Name + ":-" + currentCourse.Name;
                    var application = (from a in context.CertificateExamApplications
                                       join c in context.Candidates on a.CandidateID equals c.ID
                                       join ec in context.Exams on a.ExamID equals ec.ID
                                       where a.ID == AppId && c.DateOfBirth == DOB && a.CourseID == CourseID
                                       select new
                                       {
                                           appdate = a.ApplicationDate,
                                           appno = a.ID,
                                           dob = c.DateOfBirth,
                                           name = c.Name,
                                           fname = c.FatherName,
                                           mname = c.MotherName,
                                           photo = c.Photo,
                                           salutation = c.Salutation,
                                           examcycle = ec.Name,
                                           status  = a.PaymentStatus.Name,
                                           cid = a.CourseID
                                       }).FirstOrDefault();

                    if (application != null)
                    {
                        //divdata.Visible = false;
                        divfull.Visible = true;
                        divStatus.Visible = true;
                        LblAppdate.Text = application.appdate.ToString("dd-MMM-yyyy");
                        LblAppno.Text = application.appno.ToString();
                        LblDOB.Text = application.dob.ToString("dd-MMM-yyyy");
                        Lblname.Text = application.salutation + " " + application.name.ToUpper();
                        Lbfname.Text = "Mr. " + application.fname.ToUpper();
                        Lbmname.Text = "Mrs." + application.mname.ToUpper();
                        imgcandphoto.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.photo.BlobFile);
                        Lblss.Text = application.status;
                        trexcycle.Visible = false;

                    }
                    else
                    {
                        //Lblerror.Text = "Invalid Application No. or Date of Birth";
                        divStatus.Visible = false;
                        divfull.Visible = true;


                    }

                }
            };

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("../WEB/ApplicationStatus.aspx?ID=" + Request.QueryString["id"]);
    }
}
    
