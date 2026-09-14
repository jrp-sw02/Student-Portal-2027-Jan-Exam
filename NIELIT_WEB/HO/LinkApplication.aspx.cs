using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_LinkApplication : BasePage
{
    Int32 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 batchID = 0;
    Int32 status = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            batchID = Convert.ToInt64(Request.QueryString["BatchID"]);
            status = Convert.ToInt32(Request.QueryString["status"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/hoCoursesRegStatus.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            lblError.Visible = false;
            entityID = Convert.ToInt32(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                bindCourse();
                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlregtype , typeof(EConnect.NIELIT.enmRegistrationType), new ListItem("--Select One--", "0"));
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected Boolean isvalidForm()
    {
        try
        {
            if (ddlcrname.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please Select Registered Course Name.";
                return false;
            }
            if (String.IsNullOrEmpty(txtregistrationno.Text.Trim()))
            {
                lblError.Visible = true;
                lblError.Text = "Please Enter Registered Registration Number";
                return false;
            }
            if (!IsNumeric(txtregistrationno.Text))
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Registered Registration Number.";
                return false;
            }
            if (ddlregtype.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please Select Registration Type.";
                return false;
            }
            if (ddlpcrname.SelectedValue != "0")
            {
                if (String.IsNullOrEmpty(txtpregistrationno.Text.Trim()))
                {
                    lblError.Visible = true;
                    lblError.Text = "Please Enter Qualified Registration Number";
                    return false;
                }
                if (!IsNumeric(txtregistrationno.Text))
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Qualified Registration Number.";
                    return false;
                }
                if (String.IsNullOrEmpty(txtpregistrationyear.Text.Trim()))
                {
                    lblError.Visible = true;
                    lblError.Text = "Please Enter Qualified Year ";
                    return false;
                }
               
                if (!IsNumeric(txtpregistrationyear.Text))
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Qualified Year.";
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void bindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseType
                              select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcrname, courses, lst);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlpcrname, courses, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("BatchItems.aspx?BatchID=" + batchID + "&status=" + status +"&src=link", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        String strMessage = "";
        try
        {
            CourseRegistrationApplication coursereg;
            using (EConnectContext context = new EConnectContext())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["batchItemID"]))
                {
                    Int64 BatchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);
                    Int64 courseregappID = context.BatchItems.Where(s => s.ID == BatchItemID && s.CourseRegistrationApplicationID != null).FirstOrDefault().CourseRegistrationApplicationID.Value;
                    if (courseregappID != 0)
                    {
                        if (isvalidForm())
                        {
                            Int64 Regnumber = Convert.ToInt64(txtregistrationno.Text.Trim());
                            Int32 Reg_Cou_ID = Convert.ToInt32(ddlcrname.SelectedValue);
                            var candidate = (from p in context.RegistrationDetails
                                             where p.RegistrationNo == Regnumber && p.CourseID == Reg_Cou_ID
                                             orderby p.CommencementFromDate descending
                                             select new
                                             {
                                                 candidateid = p.CandidateID
                                             }).FirstOrDefault();
                            if (candidate !=null)
                            {
                                Boolean islinkable = false;
                                var candidatedetails = context.Candidates.Where(s => s.ID == candidate.candidateid).FirstOrDefault();
                                if (candidatedetails != null)
                                {
                                    coursereg = context.CourseRegistrationApplications.Find(courseregappID);
                                    if (!String.IsNullOrEmpty(coursereg.GuardianName))
                                    {
                                        if (candidatedetails.Name.Trim().ToUpper() == coursereg.Name.Trim().ToUpper() && candidatedetails.DateOfBirth == coursereg.DateOfBirth && candidatedetails.GuardianName.Trim().ToUpper() == coursereg.GuardianName.Trim().ToUpper())
                                        {
                                            islinkable = true;
                                        }
                                    }
                                    else
                                    {
                                        if (candidatedetails.Name.Trim().ToUpper() == coursereg.Name.Trim().ToUpper() && candidatedetails.DateOfBirth == coursereg.DateOfBirth && candidatedetails.FatherName.Trim().ToUpper() == coursereg.FatherName.Trim().ToUpper() && candidatedetails.MotherName.Trim().ToUpper() == coursereg.MotherName.Trim().ToUpper())
                                        {
                                            islinkable = true;
                                        }
                                    }
                                    if (islinkable)
                                    {
                                        coursereg.AlreadyRegistered = Convert.ToBoolean(true);
                                        coursereg.RegisteredCourseID = Reg_Cou_ID;
                                        coursereg.RegistrationTypeID = Convert.ToInt32(ddlregtype.SelectedValue);
                                        coursereg.RegisteredCourseRegistrationNo = Convert.ToInt32(txtregistrationno.Text);
                                        coursereg.CandidateID = candidate.candidateid;
                                        if (ddlpcrname.SelectedValue != "0")
                                        {
                                            coursereg.QualifiedCourseID = Convert.ToInt32(ddlpcrname.SelectedValue);
                                            coursereg.AlreadyQualified = Convert.ToBoolean(true);
                                        }
                                        if (!String.IsNullOrEmpty(txtpregistrationno.Text))
                                            coursereg.QualifiedCourseRegistrationNo = Convert.ToInt32(txtpregistrationno.Text);
                                        if (!String.IsNullOrEmpty(txtpregistrationyear.Text))
                                            coursereg.QualifiedCoursePassingYear = Convert.ToInt32(txtpregistrationyear.Text);

                                        coursereg.IsLinked = Convert.ToBoolean(true);
                                        context.Entry(coursereg).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                        strMessage = "Record Updated";
                                        Response.Redirect("BatchItems.aspx?BatchID=" + batchID + "&status=" + status + "&src=link", true);
                                    }
                                    else
                                    {
                                        ShowAlert("Incorrect Candidate Details..!", true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                ShowAlert("Please Enter Correct Registration Details..!", true);
                                return;
                            }
                        }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}