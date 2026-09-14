using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Collections.Generic;

public partial class verifycoursecompletionacc : BasePage
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
            using (EConnectContext context = new EConnectContext())
            {

                Institute loginInsitute = context.Institutes.Find(entityID);
                if (loginInsitute != null)
                {
                    BindACCCoursesData();
                }
            };
        }

    }
    protected void BindACCCoursesData()
    {
        //string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        //string sqlData = " select b.Name, Registration_No as RegistrationNo, Candidate_ID as CandidateId, Course_ID as CourseId, Institute_ID as InstituteId, " +
        //                 " convert(varchar(11),A.Registration_Date, 106) as RegistrationDate from Registration_Detail a, candidate b  " +
        //                 " where Course_ID =102 and Institute_ID='" + entityID + "' and a.Candidate_ID =b.ID " +
        //                 " except " +
        //                 " select Name, registration_no as RegistrationNo, Candidate_ID as CandidateId, Course_ID as CourseId, Institute_ID as InstituteId , " +
        //                 " (SELECT CONVERT(varchar(11), Registration_Date, 106) as RegistrationDate  from Registration_Detail " +
        //                 " WHERE Course_ID =102 AND Registration_No =AA.Registration_No) " +
        //                 " from Acc_Course_Completion_Dates AA  where Course_ID =102  and Institute_ID='" + entityID + "' order by RegistrationNo";
        //using (SqlConnection con = new SqlConnection(CS))
        //{
        //    using (SqlCommand cmd = new SqlCommand(sqlData))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            cmd.Connection = con;
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);

        //                gbapplicant.DataSource = dt;
        //                gbapplicant.DataBind();
        //            }
        //        }
        //    }
        //}
        using (EConnectContext vContext = new EConnectContext())
        {
            //List<Int64> RegsteredCand = new List<Int64>();
            //RegsteredCand = vContext.AccCourseCompletionDates.Where(s => s.CourseId == 102 && s.InstituteId == entityID).Select(s => s.RegistrationNo).ToList();

            var NewCand = (from p in vContext.RegistrationDetails
                          where p.CourseID == 102 
                          && p.InstituteID == entityID
                          && !(vContext.AccCourseCompletionDates.Where(s => s.CourseId == 102 && s.InstituteId == entityID).Select(s => s.RegistrationNo)).Contains(p.RegistrationNo)
                          select new
                          {
                              RegistrationNo = p.RegistrationNo,
                              Name = p.Candidate.Name,                             
                              ExamId = p.CourseRegistrationApplication.ApplicableExamID,
                              CourseId = p.CourseID,
                              InstituteId = p.InstituteID,
                              RegistrationDate = p.RegistrationDate
                          }).ToList();
            var FNewCand = NewCand.Select(p => new
            {
                RegistrationNo = p.RegistrationNo,
                Name = p.Name,
                ExamId = p.ExamId,
                CourseId = p.CourseId,
                InstituteId = p.InstituteId,
                RegistrationDate = p.RegistrationDate.ToString("dd-MMM-yyyy")
            });

            gbapplicant.DataSource = FNewCand.ToList();
            gbapplicant.DataBind();
            uPnlGrid1.Update();
        }
        if (gbapplicant.Rows.Count == 0)
            btnProcess.Visible = false;

         
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {

        try
        {
            using (var vContext = new EConnectContext())
            {
                Int32 CourseId = 102;
                Int32 SuccessCount = 0;
                Int64 RegistrationNo;
                Int64 ExamId;
 
                for (int i = 0; i < gbapplicant.Rows.Count; i++)
                {
                    TextBox Sdate = (TextBox)gbapplicant.Rows[i].FindControl("txtDoStart");
                    TextBox Edate = (TextBox)gbapplicant.Rows[i].FindControl("txtDoEnd");

                    if (!string.IsNullOrEmpty(Sdate.Text) && !string.IsNullOrEmpty(Edate.Text))
                    {
                        DateTime StartDate = DateTime.Parse(Sdate.Text);
                        DateTime EndDate = DateTime.Parse(Edate.Text);
                        //Int64 RegistrationNo = Convert.ToInt64(gbapplicant.DataKeys[i].Value);

                        RegistrationNo = Convert.ToInt32(gbapplicant.DataKeys[i][0].ToString());
                        ExamId = Convert.ToInt64(gbapplicant.DataKeys[i][1].ToString());

                        //Int32 RegnCycleId = vContext.CourseRegistrationApplications.Where(s => s.CandidateID == CandidateId).FirstOrDefault().ApplicableExamID.Value;

                        string Name = gbapplicant.Rows[i].Cells[3].Text.ToString();

                        if (DateValidate(RegistrationNo, StartDate, EndDate))
                        {
                        String SqlInsert = " insert into  Acc_Course_Completion_Dates(Registration_No, Name, Course_Id, Exam_Id, Institute_Id, StartDate, EndDate, Certificate_Issued) " +
                                               " values('" + RegistrationNo + "', '" + Name + "', '" + CourseId + "', '" + ExamId + "', '" + entityID + "', '" + StartDate + "', '" + EndDate + "', 0 )";
                        vContext.Database.ExecuteSqlCommand(SqlInsert);
                        vContext.SaveChanges();
                        SuccessCount += 1;
                        }
                    }
                } ShowAlert("Total " + SuccessCount + "application is submitted");
                Response.Redirect("verifycoursecompletionacc.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gbapplicant_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
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
                  //  throw new Exception("For Registration Number: '" + RegistrationNo + "', Start-Date can not be less than Registration Date: '" + RegDate + "' ");
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
            lblerror.Visible = true;
            lblerror.Text = ex.Message;
            throw ex;
        }
    }
}