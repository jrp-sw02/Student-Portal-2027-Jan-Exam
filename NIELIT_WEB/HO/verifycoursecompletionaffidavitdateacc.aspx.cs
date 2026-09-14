using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class verifycoursecompletionaffidavitdateacc : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
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
            Int32 CourseId = 0;
            using (EConnectContext vContext = new EConnectContext())
            {
                CourseId = vContext.Courses.Where(s => s.Code == "ACC").Select(s => s.ID).FirstOrDefault();
            }
            BindAccCentre(CourseId);
            FillFilterRegnYear();
            //BindRegistrationCycle(CourseId);
            if (gbapplicant.Rows.Count <= 0)
            {
                lblError.Text = "Please Select Filter Criteria to View Application Records";
                lblError.Visible = true;
            }
        }
    }
    private void BindRegistrationCycle(Int32 CourseId, Int32 Year)
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var RegnCyleList = vContext.Exams.Where(s => s.CourseID == CourseId && s.ExamYear == Year )
                    //&& s.ExamStartDate < DateTime.Now
                                    .Select(s => new { ValueField = s.ID, TextField = s.Name })
                                    .OrderByDescending(s => s.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRegnCycle, RegnCyleList, lst);
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    private void BindAccCentre(Int32 CourseId)
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);

                var InstituteList = vContext.AccCourseCompletionDates.Where(s => s.CourseId == CourseId)
                                    .Select(r => new { ValueField = r.InstituteId, TextField = r.InstituteId }).Distinct()
                                    .OrderByDescending(t => t.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlInstitute, InstituteList, lst);
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    private void BindGridView()
    {
        Int64 InstituteId = Convert.ToInt64(ddlInstitute.SelectedValue);
        Int32 ExamId = Convert.ToInt32(ddlRegnCycle.SelectedValue);
        string VerificationStatus = ddlVerified.SelectedValue;
        string CertificateIssued = ddlCertificateIssued.SelectedValue;

        using (EConnectContext vContext = new EConnectContext())
        {
            var institute = vContext.Institutes.Find(InstituteId);
            LabelDetail.Text = institute.ID + "-" + institute.Name + "<br/>Registration Cycle: " + ddlRegnCycle.SelectedItem.Text + "<br/>Verification Status: " + ddlVerified.SelectedItem.Text + "<br/>Certificate Status: " + ddlCertificateIssued.SelectedItem.Text;

            var applicants = vContext.AccCourseCompletionDates.Where(s => s.InstituteId == InstituteId && s.RegnCycleId == ExamId)
                                .OrderByDescending(s => s.RegistrationNo).ThenBy(s => s.CertificateIssued)
                                .Select(s => new
                                {
                                    ID = s.ID,
                                    RegistrationNo = s.RegistrationNo,
                                    CourseId = s.CourseId,
                                    Name = s.Name,
                                    SessionStart = s.StartDate,
                                    SessionEnd = s.EndDate,
                                    IsVerified = (s.IsVerified.HasValue) ? s.IsVerified.Value : false,
                                    CertificateIssued = (s.CertificateIssued.HasValue) ? s.CertificateIssued.Value : false
                                }).ToList();

            var capplicants = applicants.Select(s => new
            {
                RegistrationNo = s.RegistrationNo,
                CourseId = s.CourseId,
                Name = s.Name,
                SessionStart = s.SessionStart.ToString("dd-MMM-yyyy"),
                SessionEnd = s.SessionEnd.ToString("dd-MMM-yyyy"),
                IsVerified = s.IsVerified,
                CertificateIssued = s.CertificateIssued
            });

            if (VerificationStatus != "0")
            { capplicants = (VerificationStatus == "Y") ? capplicants.Where(s => s.IsVerified == true) : capplicants.Where(s => s.IsVerified == false); }
            if (CertificateIssued != "0")
            { capplicants = (CertificateIssued == "Y") ? capplicants.Where(s => s.CertificateIssued == true) : capplicants.Where(s => s.CertificateIssued == false); }
                        
            PagingBar1.Bind(capplicants, ref gbapplicant);
            uPnlGrid.Update();
            uPnlNavigation.Update();

            if (gbapplicant.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
                btnsubmit.Visible = false;
            }
            else
            {
                if (VerificationStatus == "Y" || CertificateIssued == "Y")
                { btnsubmit.Visible = false; }
                else { btnsubmit.Visible = true; }
                lblError.Visible = false;
                lblError.Text = "";
            }
        };
    }
    protected void gbapplicant_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();
            }
            if (Convert.ToBoolean(gbapplicant.DataKeys[e.Row.RowIndex][2].ToString()) == true)
            {
                e.Row.Cells[5].Enabled = false; e.Row.Cells[5].Text = "Verified";
                e.Row.Cells[6].Enabled = false; e.Row.Cells[6].Text = "";
            }
            if (Convert.ToBoolean(gbapplicant.DataKeys[e.Row.RowIndex][3].ToString()) == true)
            {
                e.Row.Cells[5].Enabled = false; e.Row.Cells[5].Text = "Certified";
                e.Row.Cells[6].Enabled = false; e.Row.Cells[6].Text = "";
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected void gbapplicant_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gbapplicant.EditIndex = e.NewEditIndex;
        BindGridView();
    }
    protected void gbapplicant_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Int64 RegistrationNo = Convert.ToInt32(gbapplicant.DataKeys[e.RowIndex][0].ToString());
        Int32 CourseId = Convert.ToInt32(gbapplicant.DataKeys[e.RowIndex][1].ToString());

        GridViewRow row = (GridViewRow)gbapplicant.Rows[e.RowIndex];

        TextBox txtADStart = (TextBox)row.FindControl("txtAfdStart");
        TextBox txtADEnd = (TextBox)row.FindControl("txtAfdEnd");
        
        DateTime StartDate = Convert.ToDateTime(txtADStart.Text.ToString());
        DateTime EndDate = Convert.ToDateTime(txtADEnd.Text.ToString());

        gbapplicant.EditIndex = -1;

        using (var vContext = new EConnectContext())
        {
            try
            {
                if (DateValidate(RegistrationNo, StartDate, EndDate))
                {
                    var app = vContext.AccCourseCompletionDates.Find(RegistrationNo, CourseId);
                    String sqlI = "INSERT INTO Acc_Course_Completion_Dates_History(Acc_Course_Completion_Dates_Id, Registration_No, Name, Course_Id, Exam_Id, Institute_Id, Old_StartDate, Old_EndDate, Phase_No, PhaseDate, Certificate_Issued, Created_by, Created_On)" +
                          "(select ID, Registration_No, Name, Course_Id, Exam_Id, Institute_Id, StartDate, EndDate, Phase_No, PhaseDate, Certificate_Issued, '" + Convert.ToInt32(Session["UserID"]) + "', '" + DateTime.Now + "' from Acc_Course_Completion_Dates Where Registration_No = " + RegistrationNo + " and Course_Id = " + CourseId + " )";
                    vContext.Database.ExecuteSqlCommand(sqlI);
                    vContext.SaveChanges();

                    app.StartDate = Convert.ToDateTime(txtADStart.Text.ToString());
                    app.EndDate = Convert.ToDateTime(txtADEnd.Text.ToString());
                    app.LastModificatationDate = DateTime.Now;
                    vContext.Entry(app).State = System.Data.Entity.EntityState.Modified;
                    vContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        BindGridView();
    }
    protected void gbapplicant_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gbapplicant.EditIndex = -1;
        BindGridView();
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if (ddlRegnCycle.SelectedValue != "0" && ddlInstitute.SelectedValue != "0")
            {
                BindGridView();
            }
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
            ddlRegnCycle.SelectedValue = "0";
            ddlInstitute.SelectedValue = "0";
            ddlCertificateIssued.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gbapplicant.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            int CourseId = vContext.Courses.Where(s => s.Code == "ACC").Select(s => s.ID).FirstOrDefault();

            int Year = Convert.ToInt32(ddlYear.SelectedItem.Text);
            BindRegistrationCycle(CourseId, Year);
        }
    }
    protected void FillFilterRegnYear()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {

                int CourseId = vContext.Courses.Where(s => s.Code == "ACC").Select(s => s.ID).FirstOrDefault();

                ListItem lst = new ListItem("--Select One--", "0");

                var Year = (from p in vContext.Exams
                            where p.CourseID == CourseId
                            select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct().OrderByDescending(s => s.ValueField).Take(6);


                EConnect.Utils.Common.ControlUtility.BindListObject(ddlYear, Year, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Int64 RegistrationNo;
        Int32 CourseId;
        using (EConnectContext vContext = new EConnectContext())
        {
            for (int i = 0; i < gbapplicant.Rows.Count; i++)
            {
                CheckBox cbx = (CheckBox)gbapplicant.Rows[i].FindControl("chkchild");
                if (cbx != null)
                {
                    if (cbx.Checked)
                    {
                        RegistrationNo = Convert.ToInt32(gbapplicant.DataKeys[i][0].ToString());
                        CourseId = Convert.ToInt32(gbapplicant.DataKeys[i][1].ToString());

                        var applicant = vContext.AccCourseCompletionDates.Find(RegistrationNo, CourseId);

                        applicant.IsVerified = true;

                        vContext.Entry(applicant).State = System.Data.Entity.EntityState.Modified;

                    }
                }
            }
            vContext.SaveChanges();
        };
        BindGridView();
    }

    protected Boolean DateValidate(Int64 RegistrationNo, DateTime StartDate, DateTime EndDate)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                //var RegDate = context.RegistrationDetails.Where(p => p.RegistrationNo == RegistrationNo && p.CourseID == 102).Select(p => p.RegistrationDate).Single();
                TimeSpan Timediffrence = EndDate - StartDate;
                //if (StartDate < Convert.ToDateTime(RegDate))
                //    throw new Exception("For Registration Number: '" + RegistrationNo + "', Start-Date can not be less than Registration Date: '" + RegDate + "' ");
                if (EndDate > DateTime.Now)
                    throw new Exception("For Registration Number: '" + RegistrationNo + "', End-Date can not be greater than Current-Date");
                if (EndDate < StartDate)
                    throw new Exception("For Registration Number: '" + RegistrationNo + "', End-Date can not be less than Start-Date");
                if (Timediffrence.Days < 4)
                    throw new Exception("For Registration Number: '" + RegistrationNo + "', Minimum gap between Start-date and End-Date should be Five Days.");

            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}