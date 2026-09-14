using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using System.Data;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public partial class HO_PuraskarDocumentUpdation :  BasePage
{
    String strMessage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           // Lblerror.Text = "";
            if (Request.UrlReferrer == null)
            
            {
                Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                Response.End();
                return;
            }
		string x = Session["UserID"].ToString();
            if (x != "349790")
            {
                ShowAlert("Menu not available for logged in user");
                return;
            }
            if (!Page.IsPostBack)
            {
                ExamName();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ViewRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 paymentPending = Convert.ToInt32(enmPaymentStatus.Pending);
            StringBuilder Paymentname = new StringBuilder();
            StringBuilder deficiencyname = new StringBuilder();
          
                using (EConnectContext context = new EConnectContext())
                {
                    //string applID = txtRegNo.Text.ToString();
                    Int64 Regnno = Convert.ToInt64(txtRegNo.Text.ToString().Trim());
                    Int64 ExamIds = Convert.ToInt64(ddlflExam.SelectedValue);
                   // DateTime DOB = Convert.ToDateTime(TxtDOB.Text.ToString());
                    PuraskarApplicationForm app = new PuraskarApplicationForm();
                    var application = (from a in context.PuraskarApplicationForms
                                       join b in context.Exams on a.ExamID equals b.ID
                                       join c in context.CourseRegistrationApplications on a.CandidateID equals c.CandidateID
                                       where a.RegnNo == Regnno && a.ExamID == ExamIds && a.finalSubmit == true
                                       select new
                                       {
                                           appdate = a.Final_Submission_Date,
                                           appno = a.ID,
                                           Appnumber = a.OnlineRefNo,
                                           name = a.Name,
                                           fname = c.FatherName,
                                           mname = c.MotherName,
                                           gname = c.GuardianName,
                                           photo = c.Photo,
                                           salutation = c.Salutation,
                                           status = a.applicationStatusID,
                                           institutename = c.Institute.Name + "(" + c.Institute.AccreditationDetails.FirstOrDefault().AccreditationNumber + ")",
                                           instituteVerifiedOn = a.VerifiedInsttOn,
                                           examVerifiedOn = a.VerifiedExamOn,
                                           financeVerifiedOn = a.VerifiedByFinanceOn,
                                           insttRejectionReason = a.InsttRejectionReason,
                                           examRejectionReason = a.examRejectionReason,
                                           financeRejectionreason = a.financeRejectionReason,
                                           course = c.CourseID,
                                           examCycle = b.Name


                                       }).FirstOrDefault();

                    if (application != null)
                    {
                        //divfilter.Visible = false;
                        divStatus.Visible = true;

                        Course coursename = context.Courses.Find(Convert.ToInt32(application.course));
                        if (coursename != null)
                            Lblcourse.Text = GetInitCap("STATUS OF PURASKAR APPLICATION FOR") + " " + GetInitCap(coursename.Name);

                        LblAppdate.Text = application.appdate.Value.ToString("dd/MMM/yyyy");
                        LblAppno.Text = application.Appnumber.ToString();
                       // LblDOB.Text = TxtDOB.Text;
                        Lblname.Text = application.salutation + " " + GetInitCap(application.name);
                        if (string.IsNullOrEmpty(application.gname) == true && string.IsNullOrWhiteSpace(application.gname) == true)
                        {
                            trfathername.Visible = true;
                            trmothername.Visible = true;
                            trgname.Visible = false;
                            if (string.IsNullOrEmpty(application.fname) == false && !string.IsNullOrWhiteSpace(application.fname))
                                Lbfname.Text = "Mr. " + GetInitCap(application.fname);
                            else
                                Lbfname.Text = "NA";
                            if (string.IsNullOrEmpty(application.mname) == false && string.IsNullOrWhiteSpace(application.mname) == false)
                                Lbmname.Text = "Mrs. " + GetInitCap(application.mname);
                            else
                                Lbmname.Text = "NA";
                        }
                        else
                        {
                            Lgname.Text = string.IsNullOrEmpty(application.gname) == false && string.IsNullOrWhiteSpace(application.gname) == false ? GetInitCap(application.gname) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trgname.Visible = true;
                        }
                        imgcandphoto.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.photo);
                        Lblecycle.Text = application.examCycle;
                        enmPuraskarApplicationStatus applStatus = (enmPuraskarApplicationStatus)application.status;
                        if (applStatus == enmPuraskarApplicationStatus.AppliedByCandidate || applStatus == enmPuraskarApplicationStatus.PaymentTransferredToBankAccount || applStatus == enmPuraskarApplicationStatus.PaymentReleasedToBank || applStatus == enmPuraskarApplicationStatus.PaymentRejectedByBank)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);

                        }
                        else if (applStatus == enmPuraskarApplicationStatus.VerifiedByInstituteButNIELITVerificationPending)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            Lblss.Text += ". Verfication Date :" + application.instituteVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        }
                        else if (applStatus == enmPuraskarApplicationStatus.VerifiedByExamWingButFinanceWingVerificationPending)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            Lblss.Text += ". Verfication Date :" + application.examVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        }
                        else if (applStatus == enmPuraskarApplicationStatus.VerifiedByFinanceWingButPaymentToBeProcessed)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            Lblss.Text += ". Verfication Date :" + application.financeVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        }

                        else if (applStatus == enmPuraskarApplicationStatus.RejectedByInstitute || applStatus == enmPuraskarApplicationStatus.RejectedByExamWing || applStatus == enmPuraskarApplicationStatus.RejectedByFinanceWing)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            if (application.examRejectionReason != null)
                                Lblss.Text += " Reason :" + application.examRejectionReason;
                            if (application.insttRejectionReason != null)
                                Lblss.Text += " Reason :" + application.insttRejectionReason;
                            if (application.financeRejectionreason != null)
                                Lblss.Text += " Reason :" + application.financeRejectionreason;
                        }

                        trexcycle.Visible = true;
                        trcstatus.BgColor = "#E6F0F0";
                        Lblerror.Visible = false;
                    }
                    else
                    {
                        Lblerror.Text = "Invalid Application No. or Exam Name or not final submitted";
                        divStatus.Visible = false;
                    }


                    if (!String.IsNullOrEmpty(Lblerror.Text))
                        Lblerror.Visible = true;
                }
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void EncryptAadhar(object sender, EventArgs e)
    {
        try
        {
           // string adhvalue = "907737549941";

            string aadhar = EncryptDecrypt.EncryptString(TxtaadharEnc.Text.Trim());
            lblaadhar.Text = aadhar;
            //string decadh = EncryptDecrypt.DecryptString(aadhar);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }


    public void ExamName()
    {
        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--All--", "0");
        using (System.Data.DataTable dt = getExamNameForDocumentUpdation())
        {
            if (dt.Rows.Count > 0)
            {
                int k;
                for (k = 0; k < 1; k++)
                {
                    ddlflExam.DataSource = dt;
                    ddlflExam.DataTextField = "ExamName";
                    ddlflExam.DataValueField = "ID";
                    ddlflExam.DataBind();
                    ddlflExam.Items.Insert(0, new ListItem("--Select One--", "0"));

                }
            }
            else
            {
                ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
            }
        }

    }

    public DataTable getExamNameForDocumentUpdation()
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getExamNameForDocumentUpdation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }
    protected void BtnUpdate_Click(object sender, System.EventArgs e)
    {
         byte[] CasteCertUpload = null;//System.IO.File.ReadAllBytes(inputCasteCertUpload.Value);
            byte[] IncomeCertUpload = null;// System.IO.File.ReadAllBytes(inputIncomeCertUpload.Value);
            byte[] PHCertUploadF = null;                      
            int CountPapersAppeared = 0,CountPapersPassed = 0;            
            string IncomeCertUploadFileName = "", CasteCertUploadFileName = "", PHCertUploadFileName = "";

        
            Int64 Regnno = Convert.ToInt64(txtRegNo.Text.ToString().Trim());
            Int64 ExamIds = Convert.ToInt64(ddlflExam.SelectedValue);

        using (EConnectContext context = new EConnectContext())
        {
            var objpuraskarApplication = (from s in context.PuraskarApplicationForms
                                          where s.RegnNo == Regnno && s.ExamID == ExamIds
                                          select s).FirstOrDefault();
            
            if (IncomeCerFileUpload.HasFile)
            {
                if (IncomeCerFileUpload.FileName.Length > 50)
                {
                    IncomeCertUploadFileName = IncomeCerFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(IncomeCerFileUpload.FileName).ToLower();
                }
                else
                {
                    IncomeCertUploadFileName = IncomeCerFileUpload.FileName;
                    IncomeCertUpload = IncomeCerFileUpload.FileBytes;
                }
                //objpuraskarApplication.IncomeCertNo = txtIncomeCertNo.Text;
                //objpuraskarApplication.IncomeCertDate = Convert.ToDateTime(txtIncomeCertDate.Text);
                objpuraskarApplication.IncomeCertUpload = IncomeCertUpload;
                objpuraskarApplication.IncomeCertFile = IncomeCertUploadFileName;
                objpuraskarApplication.IncomeCertUploadedOn = Convert.ToDateTime(DateTime.Now);
            }
            if (CasteCertFileUpload.HasFile)
            {
                if (CasteCertFileUpload.FileName.Length > 50)
                {
                    CasteCertUploadFileName = CasteCertFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(CasteCertFileUpload.FileName).ToLower();
                }
                else
                {
                    CasteCertUploadFileName = CasteCertFileUpload.FileName;
                    CasteCertUpload = CasteCertFileUpload.FileBytes;
                }

                //objpuraskarApplication.CasteCertNo = txtCasteCertNo.Text;
                //objpuraskarApplication.CasteCertDate = Convert.ToDateTime(txtCasteCertDate.Text);
                objpuraskarApplication.CasteCertUpload = CasteCertUpload;
                objpuraskarApplication.CasteCertFile = CasteCertUploadFileName;
                objpuraskarApplication.CasteCertUploadedOn = Convert.ToDateTime(DateTime.Now);
            }
            if (PHCertUpload.HasFile)
            {
                if (PHCertUpload.FileName.Length > 50)
                {
                    PHCertUploadFileName = PHCertUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(PHCertUpload.FileName).ToLower();
                }
                else
                {
                    PHCertUploadFileName = PHCertUpload.FileName;
                    PHCertUploadF = PHCertUpload.FileBytes;
                }
                //objpuraskarApplication.PHCertNo = txtPHCertNo.Text;
                //objpuraskarApplication.PHCertDate = Convert.ToDateTime(txtPHCertDate.Text);
                objpuraskarApplication.PHCertUpload = PHCertUploadF;
                objpuraskarApplication.PHCertFile = PHCertUploadFileName;
                objpuraskarApplication.PHCertUploadedOn = Convert.ToDateTime(DateTime.Now);
            }
            objpuraskarApplication.enterBy = Convert.ToInt32(Session["UserID"]);
            objpuraskarApplication.enterDate = DateTime.Now;
            context.Entry(objpuraskarApplication).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            strMessage = "Record updated.";
            ShowAlert(strMessage);
        }

    }
}