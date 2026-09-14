using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class Admin_NewApplicationStatusInstitute : BasePage
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
                if (!String.IsNullOrEmpty(Request.QueryString["ExamId"]) && !String.IsNullOrEmpty(Request.QueryString["CourseId"]) && !String.IsNullOrEmpty(Request.QueryString["RcId"]))
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
            int RcId = Convert.ToInt32(Request.QueryString["RcId"]);
            LblRptSubHeader.Text = "<b>Course : </b>" + context.Courses.Find(CourseId).Code + "</br><b>Exam : </b>" + context.Exams.Find(ExamId).Name + "</br><b>RC Name : </b>" + context.RegionalCenters.Find(RcId).Name;

            var DlcNewApplicationData = context.CertificateExamApplications.AsNoTracking()
                                              .Where(s => s.ExamID == ExamId && s.RegionalCenterID == RcId)
                                              .GroupBy(s => new { s.InstituteID, s.Institute.Name })
                                              .Select(s => new
                                              {
                                                  InstituteId = s.Key.InstituteID,
                                                  InstituteName = s.Key.Name + " (" + s.Key.InstituteID + ")",
                                                  FinalSubmitted = s.Count(p => p.FinalSubmitted == true),
                                                  InstituteNotVerified = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == false),
                                                  InstituteVerified = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == true),
                                                  InstitutePaid = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == true && p.PaymentStatusID > 1),
                                                  Disability = s.Count(p => p.FinalSubmitted == true && p.IsDisability == true && p.PaymentStatusID > 1)
                                              }).ToList();

            DlcNewApplicationGrid.DataSource = DlcNewApplicationData;
            DlcNewApplicationGrid.DataBind();
            UpdatePanel1.Update();
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