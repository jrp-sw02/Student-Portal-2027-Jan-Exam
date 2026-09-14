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
using System.Globalization;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
public partial class UserControl_CourseApplication : System.Web.UI.UserControl
{
    Int64 batchItemID = 0;
    public Int64 BatchItemID
    {
        set
        {
            batchItemID = value;
            ViewState["batchItemID"] = value;
        }
    }
    Int64 applicationId = 0;
    public Int64 ApplicationId
    {
        set
        {
            applicationId = value;
            ViewState["applicationId"] = value;
        }
    }
    Int32 applicationTypeID = 0;
    public Int32 ApplicationTypeID
    {
        set
        {
            applicationTypeID = value;
            ViewState["applicationTypeID"] = value;
        }
    }
    public void Bind()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {   
                BatchItem batchItem=new BatchItem();
                CertificateExamApplication cr=new CertificateExamApplication();
                CourseRegistrationApplication crs = new CourseRegistrationApplication();
                CourseExamApplication cea = new CourseExamApplication();
                CourseProjectApplication cpa = new CourseProjectApplication();
                RegistrationDetail rd = new RegistrationDetail();
                if(applicationTypeID==Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        cr = context.CertificateExamApplications.Find(batchItem.CertificateExamApplicationID);
                    }
                    else
                    {
                        cr = context.CertificateExamApplications.Find(applicationId);
                    }
                    if (cr != null)
                    {
                        trRegVal.Visible = false;

                        lblAppno.Text = cr.Number;
                        lblDreceive.Text = cr.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(cr.Course.Name) + "-" + Convert.ToString(cr.CourseCategory.Name);
                        Lblname.Text = CommonFunctions.GetInitCap(Convert.ToString(cr.Name));
                        lblExamName.Text = cr.Exam.Name;
                        lblFeeAmt.Text = cr.FeeAmount.Value.ToString("F");
                        

                        if (string.IsNullOrEmpty(cr.GuardianName) == true && string.IsNullOrWhiteSpace(cr.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(cr.FatherName) == false && !string.IsNullOrWhiteSpace(cr.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(cr.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(cr.MotherName) == false && string.IsNullOrWhiteSpace(cr.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(cr.MotherName);
                            else
                                LblMotherName.Text = "NA";
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(cr.GuardianName) == false && string.IsNullOrWhiteSpace(cr.GuardianName) == false ? CommonFunctions.GetInitCap(cr.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                            
                        }
                        trperaddress.Visible = false;
                        trperaddressname.Visible = false;
                        trmaritalstatus.Visible = false;
                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(cr.Gender));
                        enmCertificateExamApplicationStatus applStatus = (enmCertificateExamApplicationStatus)cr.ApplicationStatusID;
                        if ((applStatus == enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre) && (cr.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Visible = false;
                        trgender.Attributes.Add("class", "gdalternate1");
                        trdob.Attributes.Add("class", "gdrow1");
                        lblDob.Text = cr.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Photo);
                        lblCaddress.Text = FullCoAddressExam(cr);
                        lblCcontact.Text = Convert.ToString(cr.StdNumber) + "-" + Convert.ToString(cr.PhoneNumber);
                        lblCmobile.Text = Convert.ToString(cr.MobileNumber);
                        lblCemail.Text = Convert.ToString(cr.EmailAddress);
                        lblHqualification.Text = CommonFunctions.GetInitCap(Convert.ToString(cr.EducationalQualification.Name));
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?ID=" + cr.CourseID + "&Appid=" + cr.ID + "&Dob=" + cr.DateOfBirth + "&Type=Print");
                        hlklink.Target = "_blank";
                    }
                   
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        crs = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                    }
                    else
                    {
                        crs = context.CourseRegistrationApplications.Find(applicationId);
                    }
                    if (crs != null)
                    {

                        trRegVal.Visible = false;
                        lblAppno.Text = crs.Number;
                        lblDreceive.Text = crs.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(crs.Course.Name) + "-" + Convert.ToString(crs.CourseCategory.Name);
                        Lblname.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.Name));
                        lblExamName.Text = crs.ApplicableExam.Name;
                        lblFeeAmt.Text = crs.FeeAmount.Value.ToString("F");

                        if (string.IsNullOrEmpty(crs.GuardianName) == true && string.IsNullOrWhiteSpace(crs.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(crs.FatherName) == false && !string.IsNullOrWhiteSpace(crs.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(crs.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(crs.MotherName) == false && string.IsNullOrWhiteSpace(crs.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(crs.MotherName);
                            else
                                LblMotherName.Text = "NA";
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(crs.GuardianName) == false && string.IsNullOrWhiteSpace(crs.GuardianName) == false ? CommonFunctions.GetInitCap(crs.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                        }

                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.Gender));
                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)crs.ApplicationStatusID;
                        if ((applStatus == enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (crs.PaymentStatusID.Value == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.MaritalStatus.Name));
                        lblDob.Text = crs.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.Photo);
                        lblCaddress.Text = FullCoAddress(crs);
                        lblPaddress.Text = FullPerAddress(crs);
                        lblCcontact.Text = Convert.ToString(crs.StdNumber) + "-" + Convert.ToString(crs.PhoneNumber);
                        lblCmobile.Text = Convert.ToString(crs.MobileNumber);
                        lblCemail.Text = Convert.ToString(crs.EmailAddress);

                        //Added_UP_Project_08_01_2025_Start

                        if (crs.projectID != null)
                        {
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ToString()))
                            {
                                SqlCommand scCommand = new SqlCommand("select projectName from NielitProjects where id=@projID", new SqlConnection(con.ConnectionString));

                                scCommand.Parameters.AddWithValue("@projID", crs.projectID.ToString());

                                if (scCommand.Connection.State == ConnectionState.Closed)
                                {
                                    scCommand.Connection.Open();
                                }
                                trProject.Visible = true;
                                lblProject.Text = scCommand.ExecuteScalar().ToString();
                            }

                            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
                            {

                                SqlCommand scCommand1 = new SqlCommand("SELECT [ID],[field1],[field2],[field3],[field4],[field5]  FROM [NIELIT].[dbo].[projCriteriaMaster] where projID=@projID", new SqlConnection(conn.ConnectionString));

                                scCommand1.Parameters.AddWithValue("@projID", crs.projectID.ToString());
                                if (scCommand1.Connection.State == ConnectionState.Closed)
                                {
                                    scCommand1.Connection.Open();
                                }

                                DataTable dt = new DataTable();
                                SqlDataAdapter da = new SqlDataAdapter(scCommand1);
                                //DataSet ds = new DataSet();
                                da.Fill(dt);

                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["field1"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field1"].ToString()))
                                    {
                                        trField1.Visible = true;
                                        headField1.Text = row["field1"].ToString();
                                        lblField1.Text = string.IsNullOrWhiteSpace(crs.field1.ToString()) ? " NA " : crs.field1.ToString();
                                    }
                                    else
                                    {
                                        trField1.Visible = false;
                                        headField1.Text = "Field 1";
                                        lblField1.Text = "";
                                    }
                                    if (row["field2"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field2"].ToString()))
                                    {

                                        trField2.Visible = true;
                                        headField2.Text = row["field2"].ToString();
                                        lblField2.Text = string.IsNullOrWhiteSpace(crs.field2.ToString()) ? " NA " : crs.field2.ToString();
                                    }
                                    else
                                    {
                                        trField2.Visible = false;
                                        headField2.Text = "Field 2";
                                        lblField2.Text = "";
                                    }
                                    if (row["field3"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field3"].ToString()))
                                    {
                                        trField3.Visible = true;
                                        headField3.Text = row["field3"].ToString();
                                        lblField3.Text = string.IsNullOrWhiteSpace(crs.field3.ToString()) ? " NA " : crs.field3.ToString();
                                    }
                                    else
                                    {
                                        trField3.Visible = false;
                                        headField3.Text = "Field 3";
                                        lblField3.Text = "";
                                    }
                                    if (row["field4"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field4"].ToString()))
                                    {
                                        trField4.Visible = true;
                                        headField4.Text = row["field4"].ToString();
                                        lblField4.Text = string.IsNullOrWhiteSpace(crs.field4.ToString()) ? " NA " : crs.field4.ToString();
                                    }
                                    else
                                    {
                                        trField4.Visible = false;
                                        headField4.Text = "Field 4";
                                        lblField4.Text = "";
                                    }
                                    if (row["field5"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field5"].ToString()))
                                    {
                                        trField5.Visible = true;
                                        headField5.Text = row["field5"].ToString();
                                        lblField5.Text = string.IsNullOrWhiteSpace(crs.field5.ToString()) ? " NA " : crs.field5.ToString();
                                    }
                                    else
                                    {
                                        trField5.Visible = false;
                                        headField5.Text = "Field 5";
                                        lblField5.Text = "";
                                    }
                                }
                            }
                        }

                        //Added_UP_Project_08_01_2025_End

                        lblHqualification.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.EducationalQualification.Name));
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?ID=" + crs.CourseID + "&Appid=" + crs.ID + "&Dob=" + crs.DateOfBirth + "&Type=Print");
                        hlklink.Target = "_blank";
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                         cea  = context.CourseExamApplications.Find(batchItem.CourseExamApplicationID);
                    }
                    else
                    {
                        cea = context.CourseExamApplications.Find(applicationId);
                    }
                    if (cea != null)
                    {
                        trRegVal.Visible = false;

                        lblAppno.Text = cea.Number;
                        lblDreceive.Text = cea.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(cea.Course.Name) + "-" + Convert.ToString(cea.CourseCategory.Name);
                        Lblname.Text = CommonFunctions.GetInitCap(Convert.ToString(cea.Candidate.Name));
                        lblExamName.Text = cea.Exam.Name;
                        lblFeeAmt.Text = cea.FeeAmount.ToString("F");
                        if (string.IsNullOrEmpty(cea.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(cea.Candidate.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(cea.Candidate.FatherName) == false && !string.IsNullOrWhiteSpace(cea.Candidate.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(cea.Candidate.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(cea.Candidate.MotherName) == false && string.IsNullOrWhiteSpace(cea.Candidate.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(cea.Candidate.MotherName);
                            else
                                LblMotherName.Text = "NA";
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(cea.Candidate.GuardianName) == false && string.IsNullOrWhiteSpace(cea.Candidate.GuardianName) == false ? CommonFunctions.GetInitCap(cea.Candidate.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                        }
                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(cea.Candidate.Gender));
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)cea.ApplicationStatusID;
                        if ((applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)
                        {
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                            if (cea.FormForwardedByInstituteOn.HasValue)
                                lblStatus.Text += "<br/> On :- " + cea.FormForwardedByInstituteOn.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Text = CommonFunctions.GetInitCap(Convert.ToString(cea.Candidate.MaritalStatus.Name));
                        lblDob.Text = cea.Candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cea.Candidate.Photo.BlobFile);
                        Int32 corresspondence = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        Address corrAdd =cea.Candidate.Addresses.Where(d => d.AddressTypeID == corresspondence).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                        if (corrAdd != null)
                        {
                            lblPaddress.Text = FullCorAddressCourseExamApplication(corrAdd);
                        }
                        Int32 permanent= Convert.ToInt32(enmAddressType.PermanentAddress);
                        Address perAdd = cea.Candidate.Addresses.Where(d => d.AddressTypeID == permanent).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                        if (perAdd != null)
                        {
                            lblCaddress.Text = FullPermanentAddressCourseExamApplication(perAdd);
                        }
                        ICollection<CandidateContactDetail> contactDetails = cea.Candidate.ContactDetails;
                        if (contactDetails != null)
                        {
                            CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                            lblCcontact.Text = (cd.StdNumber.HasValue ? cd.StdNumber.Value : 0).ToString() + "-" + (cd.PhoneNumber.HasValue ? cd.PhoneNumber.Value : 0).ToString();
                            lblCemail.Text = cd.EmailAddress;
                            lblCmobile.Text = (cd.MobileNumber.HasValue ? cd.MobileNumber.Value : 0).ToString();
                        }
                        CandidateQualificationDetail qd = context.CandidateQualificationDetails.Where(s => s.CandidateID == cea.CandidateID).OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                        lblHqualification.Text = (qd.EducationalQualificationID != null && qd.EducationalQualificationID != 0) ? CommonFunctions.GetInitCap(qd.EducationalQualification.Name) : "NA";
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?ID=" + cea.CourseID + "&Appid=" + cea.ID + "&Dob=" + cea.Candidate.DateOfBirth + "&candidateID=" + cea.CandidateID+ "&Type=Print");
                        hlklink.Target = "_blank";
                    }
                 
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                {
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        cpa = context.CourseProjectApplications.Find(batchItem.CourseExamApplicationID);
                    }
                    else
                    {
                        cpa = context.CourseProjectApplications.Find(applicationId);
                    }
                    if (cpa != null)
                    {
                        lblAppno.Text = cpa.Number;
                        lblDreceive.Text = cpa.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(cpa.Course.Name) + "-" + Convert.ToString(cpa.CourseCategory.Name);
                        Lblname.Text = CommonFunctions.GetInitCap(Convert.ToString(cpa.Candidate.Name));
                       // lblExamName.Text = cpa.Exam.Name;

                        trRegVal.Visible = true;
                        Int64? candidateId = cpa.CandidateID;
                       // rd = context.RegistrationDetails.Find(candidateId);

                        var regDetail = (from r in context.RegistrationDetails
                                         where r.CandidateID == candidateId
                                         select new {  validity= r.ValidUptoDate}).FirstOrDefault();
                        lblRegValidity.Text =  regDetail.validity.ToString("dd-MMM-yyyy");



                        int? examId = cpa.ExamID;
                        var examName = context.Exams.Where(x => x.ID == examId).FirstOrDefault();
                        lblExamName.Text = examName.Name;

                        lblFeeAmt.Text = cpa.FeeAmount.ToString("F");
                        if (string.IsNullOrEmpty(cpa.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(cpa.Candidate.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(cpa.Candidate.FatherName) == false && !string.IsNullOrWhiteSpace(cpa.Candidate.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(cpa.Candidate.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(cpa.Candidate.MotherName) == false && string.IsNullOrWhiteSpace(cpa.Candidate.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(cpa.Candidate.MotherName);
                            else
                                LblMotherName.Text = "NA";
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(cpa.Candidate.GuardianName) == false && string.IsNullOrWhiteSpace(cpa.Candidate.GuardianName) == false ? CommonFunctions.GetInitCap(cpa.Candidate.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                        }
                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(cpa.Candidate.Gender));
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)cpa.ApplicationStatusID;
                        if ((applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (cpa.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)
                        {
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                            if (cea.FormForwardedByInstituteOn.HasValue)
                                lblStatus.Text += "<br/> On :- " + cpa.FormForwardedByInstituteOn.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Text = CommonFunctions.GetInitCap(Convert.ToString(cpa.Candidate.MaritalStatus.Name));
                        lblDob.Text = cpa.Candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cpa.Candidate.Photo.BlobFile);
                        Int32 corresspondence = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        Address corrAdd = cpa.Candidate.Addresses.Where(d => d.AddressTypeID == corresspondence).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                        if (corrAdd != null)
                        {
                            lblPaddress.Text = FullCorAddressCourseExamApplication(corrAdd);
                        }
                        Int32 permanent = Convert.ToInt32(enmAddressType.PermanentAddress);
                        Address perAdd = cpa.Candidate.Addresses.Where(d => d.AddressTypeID == permanent).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                        if (perAdd != null)
                        {
                            lblCaddress.Text = FullPermanentAddressCourseExamApplication(perAdd);
                        }
                        ICollection<CandidateContactDetail> contactDetails = cpa.Candidate.ContactDetails;
                        if (contactDetails != null)
                        {
                            CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                            lblCcontact.Text = (cd.StdNumber.HasValue ? cd.StdNumber.Value : 0).ToString() + "-" + (cd.PhoneNumber.HasValue ? cd.PhoneNumber.Value : 0).ToString();
                            lblCemail.Text = cd.EmailAddress;
                            lblCmobile.Text = (cd.MobileNumber.HasValue ? cd.MobileNumber.Value : 0).ToString();
                        }
                        CandidateQualificationDetail qd = context.CandidateQualificationDetails.Where(s => s.CandidateID == cpa.CandidateID).OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                        lblHqualification.Text = (qd.EducationalQualificationID != null && qd.EducationalQualificationID != 0) ? CommonFunctions.GetInitCap(qd.EducationalQualification.Name) : "NA";
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?ID=" + cea.CourseID + "&Appid=" + cea.ID + "&Dob=" + cea.Candidate.DateOfBirth + "&candidateID=" + cea.CandidateID + "&Type=Print");
                        hlklink.Target = "_blank";
                    }

                }
               
               
            };

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    private string FullCorAddressCourseExamApplication(Address add)
    {
        string str = add.AddressLine1;
        if (add.AddressLine2 != null)
            if (add.AddressLine2.Trim().Length > 1)
                str += ", " + add.AddressLine2.Trim().Trim(',');
        if (add.AddressLine3 != null)
            if (add.AddressLine3.Trim().Length > 1)
                str += ", " + add.AddressLine3.Trim().Trim(',');
        if (add.CityName != null)
            str += "<br>" + add.CityName;
        if (add.DistrictID.HasValue)
            str += "<br/> District:- " + add.District.Name + ", ";
        if (add.State.Name != null)
            str += "<br/> State:- " + add.State.Name;
        if (add.PinCode.HasValue)
            str += ",&nbsp; Pin:- " + add.PinCode.Value.ToString();
        return str;
    }
    private string FullPermanentAddressCourseExamApplication(Address add)
    {
        string str = add.AddressLine1;
        if (add.AddressLine2 != null)
            if (add.AddressLine2.Trim().Length > 1)
                str += ", " + add.AddressLine2.Trim().Trim(',');
        if (add.AddressLine3 != null)
            if (add.AddressLine3.Trim().Length > 1)
                str += ", " + add.AddressLine3.Trim().Trim(',');
        if (add.CityName != null)
            str += "<br>" + add.CityName;
        if (add.DistrictID.HasValue)
            str += "<br/> District:- " + add.District.Name + ", ";
        if (add.State.Name != null)
            str += "<br/> State:- " + add.State.Name;
        if (add.PinCode.HasValue)
            str += ",&nbsp; Pin:- " + add.PinCode.Value.ToString();
        return str;
    }
    private String FullCoAddressExam(CertificateExamApplication cr)
    {
        string str = CommonFunctions.GetInitCap(cr.CorAddressLine1);
        if (cr.CorAddressLine2 != null)
            str += "<br>" + CommonFunctions.GetInitCap(cr.CorAddressLine2);
        if (cr.CorAddressLine3 != null)
            str += "<br>" + CommonFunctions.GetInitCap(cr.CorAddressLine3);
        if (cr.CorCityName != null)
            str += "<br>" + CommonFunctions.GetInitCap(cr.CorCityName);
        if (cr.CorDistrictID.HasValue && cr.CorStateID != 0)
            str += "<br>District: " + CommonFunctions.GetInitCap(cr.CorDistrict.Name) + ", " + CommonFunctions.GetInitCap(cr.CorState.Name);
        if (cr.CorPinCode != 0)
            str += "<br>Pin: " + cr.CorPinCode.ToString();
        return str;
    }
    private String FullPerAddress(CourseRegistrationApplication crs)
    {
        string str = CommonFunctions.GetInitCap(crs.PerAddressLine1);
        if (crs.PerAddressLine2 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.PerAddressLine2);
        if (crs.PerAddressLine3 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.PerAddressLine3);
        if (crs.PerCityName != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.PerCityName);
        if (crs.PerDistrictID.HasValue && crs.PerStateID != 0)
            str += "<br>District: " + CommonFunctions.GetInitCap(crs.PerDistrict.Name) + ", " + CommonFunctions.GetInitCap(crs.PerState.Name);
        if (crs.PerPinCode != 0)
            str += "<br>Pin: " + crs.PerPinCode.ToString();
        return str;
    }
    private String FullCoAddress(CourseRegistrationApplication crs)
    {
        string str = CommonFunctions.GetInitCap(crs.CorAddressLine1);
        if (crs.CorAddressLine2 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.CorAddressLine2);
        if (crs.CorAddressLine3 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.CorAddressLine3);
        if (crs.CorCityName != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.CorCityName);
        if (crs.CorDistrictID.HasValue && crs.CorStateID != 0)
            str += "<br>District: " + CommonFunctions.GetInitCap(crs.CorDistrict.Name) + ", " + CommonFunctions.GetInitCap(crs.CorState.Name);
        if (crs.CorPinCode != 0)
            str += "<br>Pin: " + crs.PerPinCode.ToString();
        return str;
    }
    protected void ToggleImage(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            string arg = btn.CommandArgument.ToLower();
            using (EConnectContext context = new EConnectContext())
            {
                BatchItem batchItem = new BatchItem();
                CertificateExamApplication cr = new CertificateExamApplication();
                CourseRegistrationApplication crs = new CourseRegistrationApplication();
                CourseExamApplication cea = new CourseExamApplication();
                applicationTypeID = Convert.ToInt32(ViewState["applicationTypeID"]);
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    batchItemID = Convert.ToInt64(ViewState["batchItemID"]);
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        cr = context.CertificateExamApplications.Find(batchItem.CertificateExamApplicationID);
                    }
                    else
                    {
                        applicationId = Convert.ToInt64(ViewState["applicationId"]);
                        cr = context.CertificateExamApplications.Find(applicationId);
                    }
                    if (arg == "p")
                    {
                        if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Photograph";
                            ImgBtnPrevious.Enabled = false;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "thumb")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (LblPhotoCaption.Text.ToLower() == "photograph")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Thumb";
                            ImgBtnNext.Enabled = false;
                            ImgBtnPrevious.Enabled = true;
                        }
                    }
                    ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                    ImgCandidatePhoto.Height = 130;
                    ImgCandidatePhoto.Width = 112;
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        ImgBtnPrevious.ToolTip = "";
                        ImgBtnNext.ToolTip = "Click to view signature.";
                        if(!string.IsNullOrEmpty(cr.PhotoFileName))
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Photo);
                    }
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view photograph";
                        ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                        if(!string.IsNullOrEmpty(cr.SignatureFileName))
                        {
                            ImgCandidatePhoto.Height = 50;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Signature);
                        }
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view signature";
                        ImgBtnNext.ToolTip = "";
                        if(!string.IsNullOrEmpty(cr.LeftThumbFileName))
                        {
                            ImgCandidatePhoto.Height = 60;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.LeftThumb);
                        }
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    batchItemID = Convert.ToInt64(ViewState["batchItemID"]);
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        crs = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                    }
                    else
                    {
                        applicationId = Convert.ToInt64(ViewState["applicationId"]);
                        crs = context.CourseRegistrationApplications.Find(applicationId);
                    }
                    if (arg == "p")
                    {
                        if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Photograph";
                            ImgBtnPrevious.Enabled = false;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "thumb")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (LblPhotoCaption.Text.ToLower() == "photograph")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Thumb";
                            ImgBtnNext.Enabled = false;
                            ImgBtnPrevious.Enabled = true;
                        }
                    }
                    ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                    ImgCandidatePhoto.Height = 130;
                    ImgCandidatePhoto.Width = 112;
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        ImgBtnPrevious.ToolTip = "";
                        ImgBtnNext.ToolTip = "Click to view signature.";
                        if (!string.IsNullOrEmpty(crs.PhotoFileName))
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.Photo);
                    }
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view photograph";
                        ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                        if (!string.IsNullOrEmpty(crs.SignatureFileName))
                        {
                            ImgCandidatePhoto.Height = 50;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.Signature);
                        }
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view signature";
                        ImgBtnNext.ToolTip = "";
                        if (!string.IsNullOrEmpty(crs.LeftThumbFileName))
                        {
                            ImgCandidatePhoto.Height = 60;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.LeftThumb);
                        }
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {

                    batchItemID = Convert.ToInt64(ViewState["batchItemID"]);
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        cea = context.CourseExamApplications.Find(batchItem.CourseExamApplicationID);
                    }
                    else
                    {
                        applicationId = Convert.ToInt64(ViewState["applicationId"]);
                        cea = context.CourseExamApplications.Find(applicationId);
                    }
                    Candidate cand = cea.Candidate;
                    if (arg == "p")
                    {
                        if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Photograph";
                            ImgBtnPrevious.Enabled = false;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "thumb")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (LblPhotoCaption.Text.ToLower() == "photograph")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Thumb";
                            ImgBtnNext.Enabled = false;
                            ImgBtnPrevious.Enabled = true;
                        }
                    }
                    ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                    ImgCandidatePhoto.Height = 130;
                    ImgCandidatePhoto.Width = 112;
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        ImgBtnPrevious.ToolTip = "";
                        ImgBtnNext.ToolTip = "Click to view signature.";
                        if (cand.Photo != null)
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cand.Photo.BlobFile);
                    }
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view photograph";
                        ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                        if (cand.Signature != null)
                        {
                            ImgCandidatePhoto.Height = 50;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cand.Signature.BlobFile);
                        }
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view signature";
                        ImgBtnNext.ToolTip = "";
                        if (cand.LeftThumbImpression != null)
                        {
                            ImgCandidatePhoto.Height = 60;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cand.LeftThumbImpression.BlobFile);
                        }
                    }
                }

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}