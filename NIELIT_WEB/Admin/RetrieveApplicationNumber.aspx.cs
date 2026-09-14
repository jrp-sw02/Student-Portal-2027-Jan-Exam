using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class Admin_RetrieveApplicationNumber : BasePage
{

    UserType loginUserType;
    Int64 entityID = 0;   
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (IsSessionAlive() == false)
        //    Response.Redirect("../Index.aspx");
        //currentRoleId = Convert.ToInt32(Session["RoleID"]);
        //loginUserNo = Convert.ToInt32(Session["UserID"]);
        //if (!UserManager.HasRight(currentRoleId, enmRight.View))
        //{
        //    Response.Write("Sorry! You don't have rights  to view this page");
        //    Response.End();
        //}

        if (!IsPostBack)
        {
            BindCourse();
            BindExamYear();
            GenerateNewCaptchaImage();
        }       
    }

    protected void BindExamYear()
    {

        var currentYear = DateTime.Today.Year - 3;
        for (int i = 1; i <= 4; i++)
        {
            ddlYear.Items.Add((currentYear + i).ToString());
        }
        //ddl_year.Items.Insert(0, new ListItem("--Select One--", "--Select One--"));
    }

    protected void BindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select One --", "0");

                var courses = (from c in context.Courses
                               where c.CourseCategoryID == 2
                               select new { ValueField = c.ID, TextField = c.Name });
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdoownertype.SelectedValue == "P")
            {
                trfather.Visible = true;
                trmother.Visible = true;
                trguardian.Visible = false;
            }
            if (Rdoownertype.SelectedValue == "G")
            {
                trguardian.Visible = true;
                trfather.Visible = false;
                trmother.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var context = new EConnectContext())
            {               
                    string candidateName = txtName.Text;
                    string fatherName = txtFatherName.Text;
                    string motherName = txtMotherName.Text;
                    string guardianName = txtGuardianName.Text;
                    DateTime dateofBirth = Convert.ToDateTime(txtDOB.Text);
                    Int64 courseName = Convert.ToInt64(ddlCourseName.SelectedValue);
                    //var course = (from c in context.Courses
                    //              where (c.Name == courseName || c.Code == courseName ) 
                    //              select new { courseId = c.ID }).FirstOrDefault();

                    Int64 examMonth = Convert.ToInt64(ddlMonth.SelectedValue);
                    //Int64 examMonth = 

                    // Int64 examYear = Convert.ToInt64(txtExamYear.Text);
                    Int64 examYear = Convert.ToInt64(ddlYear.SelectedValue);

                    var exam = (from b in context.Exams
                                where b.CourseID == courseName && b.ExamMonth == examMonth && b.ExamYear == examYear
                                select new { Id = b.ID }).FirstOrDefault();

                    var applicationNumber = (from a in context.CertificateExamApplications
                                             where a.Name == candidateName && a.FatherName == fatherName && a.MotherName == motherName && a.DateOfBirth == dateofBirth
                                             && a.ExamID == exam.Id
                                             select new { appNum = a.Number }).FirstOrDefault();

                    if (applicationNumber == null)
                    {
                        var historyCheck = (from b in context.CertificateExamApplicationHistory
                                            where b.Name == candidateName && b.FatherName == fatherName && b.MotherName == motherName && b.DateOfBirth == dateofBirth
                                            && b.ExamID == exam.Id
                                            select new { appNum = b.Number }).FirstOrDefault();

                        if (historyCheck == null)
                        {
                            trResult.Visible = true;

                            if (txtCode.Text == ViewState["CaptchCode"].ToString())
                            {
                                lblResult.Text = " Your Application Number is not available . Please contact Administrator.";
                            }
                            else
                            {
                                lblResult.Text = "Invalid Captcha Code";
                                GenerateNewCaptchaImage();
                                txtCode.Text = "";
                            }     
                        }
                        else
                        {
                            trResult.Visible = true;
                            if (txtCode.Text == ViewState["CaptchCode"].ToString())
                            {
                                lblResult.Text = " Your Application Number is :-" + historyCheck.appNum;
                            }
                            else
                            {
                                lblResult.Text = "Invalid Captcha Code";
                                GenerateNewCaptchaImage();
                                txtCode.Text = "";
                            }                            
                        }
                    }
                    else
                    {
                        trResult.Visible = true;
                        if (txtCode.Text == ViewState["CaptchCode"].ToString())
                        {
                            lblResult.Text = " Your Application Number is :-" + applicationNumber.appNum;
                        }
                        else
                        {
                            lblResult.Text = "Invalid Captcha Code";
                            GenerateNewCaptchaImage();
                            txtCode.Text = "";
                        }
                    }
                }                                      
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            EConnect.CaptchaImage captcha = new EConnect.CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Arial");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {           
            GenerateNewCaptchaImage();
            txtCode.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}