using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class HO_PaymentStatus : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            if (!IsPostBack)
            {

                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem("Payment Status","",""));
                BindGridView();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGridView()
    {
        try
        {
            
            Int32 PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
            Int32 paidButPending = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
            Int32 Pending = Convert.ToInt32(enmPaymentStatus.Pending);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 AppcourseRegistration = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                var CourseRegistration = (from s in  context.DemandNotes
                                          where s.ApplicationTypeID == AppcourseRegistration
                                          group s by new { s.PaymentModeID, s.PaymentMode.Name } into c
                                          select new
                                          {
                                              ID = c.Key.PaymentModeID,
                                              Name = c.Key.Name,
                                              Pending = c.Count(p => p.PaymentStatusID == Pending),
                                              Paidnotverified = c.Count(p => p.PaymentStatusID == paidButPending),
                                              Paid = c.Count(p => p.PaymentStatusID == PaymentPaid),
                                          });

                if (CourseRegistration.Count() == 0)
                {
                    lblCourseMsg.Text = "Data Not Found";
                }
                else
                {
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    CourseRegistration = CourseRegistration.OrderByDescending(s => s.ID);
                                else
                                    CourseRegistration = CourseRegistration.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    CourseRegistration = CourseRegistration.OrderByDescending(s => s.Name);
                                else
                                    CourseRegistration = CourseRegistration.OrderBy(s => s.Name);
                                break;
                            case "Pending":
                                if (sortOrder == "DESC")
                                    CourseRegistration = CourseRegistration.OrderByDescending(s => s.Pending);
                                else
                                    CourseRegistration = CourseRegistration.OrderBy(s => s.Pending);
                                break;
                            case "Paidnotverified":
                                if (sortOrder == "DESC")
                                    CourseRegistration = CourseRegistration.OrderByDescending(s => s.Paidnotverified);
                                else
                                    CourseRegistration = CourseRegistration.OrderBy(s => s.Paidnotverified);
                                break;
                            case "Paid":
                                if (sortOrder == "DESC")
                                    CourseRegistration = CourseRegistration.OrderByDescending(s => s.Paid);
                                else
                                    CourseRegistration = CourseRegistration.OrderBy(s => s.Paid);
                                break;
                            default:
                                CourseRegistration = CourseRegistration.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    gvCourse.DataSource = CourseRegistration.ToList();
                    gvCourse.DataBind();
                }

                //---------------------------------------Certificate Exam----------------------------------------------  
                Int32 AppcerificateExam = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                var CertificateExam = (from s in context.DemandNotes
                                       where s.ApplicationTypeID == AppcerificateExam
                                       group s by new { s.PaymentModeID, s.PaymentMode.Name } into c
                                       select new
                                       {
                                           ID = c.Key.PaymentModeID,
                                           Name = c.Key.Name,
                                           Pending = c.Count(p => p.PaymentStatusID == Pending),
                                           Paidnotverified = c.Count(p => p.PaymentStatusID == paidButPending),
                                           Paid = c.Count(p => p.PaymentStatusID == PaymentPaid),
                                       });
                if (CertificateExam.Count() == 0)
                {
                    lblCertificateMsg.Text = "Data Not Found";
                }
                else
                {
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    CertificateExam = CertificateExam.OrderByDescending(s => s.ID);
                                else
                                    CertificateExam = CertificateExam.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    CertificateExam = CertificateExam.OrderByDescending(s => s.Name);
                                else
                                    CertificateExam = CertificateExam.OrderBy(s => s.Name);
                                break;
                            case "Pending":
                                if (sortOrder == "DESC")
                                    CertificateExam = CertificateExam.OrderByDescending(s => s.Pending);
                                else
                                    CertificateExam = CertificateExam.OrderBy(s => s.Pending);
                                break;
                            case "Paidnotverified":
                                if (sortOrder == "DESC")
                                    CertificateExam = CertificateExam.OrderByDescending(s => s.Paidnotverified);
                                else
                                    CertificateExam = CertificateExam.OrderBy(s => s.Paidnotverified);
                                break;
                            case "Paid":
                                if (sortOrder == "DESC")
                                    CertificateExam = CertificateExam.OrderByDescending(s => s.Paid);
                                else
                                    CertificateExam = CertificateExam.OrderBy(s => s.Paid);
                                break;
                            default:
                                CertificateExam = CertificateExam.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    gvCertificate.DataSource = CertificateExam.ToList();
                    gvCertificate.DataBind();
                }
                //---------------------------------------Course Exam----------------------------------------------
                Int32 AppcourseExam = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                var CourseExam = (from s in context.DemandNotes
                                  where s.ApplicationTypeID == AppcourseExam
                                  group s by new { s.PaymentModeID, s.PaymentMode.Name } into c
                                  select new
                                  {
                                      ID = c.Key.PaymentModeID,
                                      Name = c.Key.Name,
                                      Pending = c.Count(p => p.PaymentStatusID == Pending),
                                      Paidnotverified = c.Count(p => p.PaymentStatusID == paidButPending),
                                      Paid = c.Count(p => p.PaymentStatusID == PaymentPaid),
                                  });
                if (CourseExam.Count() == 0)
                {
                    lblCourseExam.Text = "Data Not Found";
                }
                else
                {

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    CourseExam = CourseExam.OrderByDescending(s => s.ID);
                                else
                                    CourseExam = CourseExam.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    CourseExam = CourseExam.OrderByDescending(s => s.Name);
                                else
                                    CourseExam = CourseExam.OrderBy(s => s.Name);
                                break;
                            case "Pending":
                                if (sortOrder == "DESC")
                                    CourseExam = CourseExam.OrderByDescending(s => s.Pending);
                                else
                                    CourseExam = CourseExam.OrderBy(s => s.Pending);
                                break;
                            case "Paidnotverified":
                                if (sortOrder == "DESC")
                                    CourseExam = CourseExam.OrderByDescending(s => s.Paidnotverified);
                                else
                                    CourseExam = CourseExam.OrderBy(s => s.Paidnotverified);
                                break;
                            case "Paid":
                                if (sortOrder == "DESC")
                                    CourseExam = CourseExam.OrderByDescending(s => s.Paid);
                                else
                                    CourseExam = CourseExam.OrderBy(s => s.Paid);
                                break;
                            default:
                                CourseExam = CourseExam.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    gvCourseExam.DataSource = CourseExam.ToList();
                    gvCourseExam.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void gvCourse_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            //BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvCourse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
               // HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
               // hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification).ToString());

               // HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
               // hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

               // HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
               // hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

               // HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
               // hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre).ToString());

               // HyperLink hl6 = (HyperLink)e.Row.Cells[6].Controls[0];
               // hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre).ToString());

               //// e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCertificate_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            //BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvCourseExam_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            //BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvCourseExam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////Encryption url of hypelink field
                //HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification).ToString());

                //HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                //HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                //HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                //hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString() + "," + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                //HyperLink hl6 = (HyperLink)e.Row.Cells[6].Controls[0];
                //hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT).ToString());

                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCertificate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////Encryption url of hypelink field
                //HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification).ToString());

                //HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                //HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                //HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                //hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre).ToString());

                //HyperLink hl6 = (HyperLink)e.Row.Cells[6].Controls[0];
                //hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre).ToString());

                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}