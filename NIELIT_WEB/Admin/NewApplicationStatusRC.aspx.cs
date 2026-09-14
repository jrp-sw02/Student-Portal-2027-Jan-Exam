using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class Admin_NewApplicationStatusRC : BasePage
{
    String strMessage = string.Empty;
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ExamId"]) && !String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    ShowData();
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                    Response.End();
                    return;
                }
            }
        }
        catch (Exception ex) { }

    }
    protected void ShowData()
    {
        using (EConnectContext context = new EConnectContext())
        {
            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            LblRptSubHeader.Text = "<b>Course : </b>" + context.Courses.Find(CourseId).Code + "</br><b>Exam : </b>" + context.Exams.Find(ExamId).Name;

            var DlcNewApplicationData = context.CertificateExamApplications.AsNoTracking()
                                              .Where(s => s.ExamID == ExamId)
                                              .GroupBy(s => new { s.RegionalCenterID, s.RegionalCenter.Name })
                                              .Select(s => new
                                              {
                                                  CourseId = CourseId,
                                                  ExamId = ExamId,
                                                  RcId = s.Key.RegionalCenterID,
                                                  RcName = s.Key.Name,
                                                  FinalSubmitted = s.Count(p => p.FinalSubmitted == true),
                                                  Direct = s.Count(p => p.FinalSubmitted == true && p.ApplicantTypeID == 1),
                                                  InstituteNotVerified = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == false),
                                                  InstituteVerified = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == true),                                                  
                                                  InstitutePaid = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == true && p.PaymentStatusID > 1),
                                                  FeePaid = s.Count(p => p.FinalSubmitted == true && p.PaymentStatusID > 1),
                                                  Disability = s.Count(p => p.FinalSubmitted == true && p.IsDisability == true && p.PaymentStatusID > 1)
                                              }).ToList();

            if (DlcNewApplicationData.Count() > 0)
            {
                const string format = "{0}  |  {1}  |  {2}";
                var DisplayData = DlcNewApplicationData
                                  .Select(s => new
                                  {
                                      CourseId = s.CourseId,
                                      ExamId = s.ExamId,
                                      RcId = s.RcId,
                                      RcName = s.RcName,
                                      FinalSubmitted = s.FinalSubmitted,
                                      Direct = s.Direct,
                                      ViaInstitute = string.Format(format, s.InstituteNotVerified, s.InstituteVerified, s.InstitutePaid),
                                      FeePaid = s.FeePaid,
                                      Disability = s.Disability
                                  }).ToList();

                DlcNewApplicationGrid.DataSource = DisplayData;
                DlcNewApplicationGrid.DataBind();
                UpdatePanel1.Update();
            }
        }
    }
    protected void DlcNewApplicationGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
        }
    }
}