using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

public partial class HO_AffidavitVerificationRegn : BasePage
{
    Int32 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            lblError.Visible = false;
            entityID = Convert.ToInt32(Session["EntityID"]);
            BtnBack.Visible = false;
            if (!IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Affidavit Verification Process", "#", ""));
                bindCourse();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["AppNo"])))
                {
                    Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    string appNo = Convert.ToString(Request.QueryString["AppNo"]);
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application No. " + appNo.ToString(), "#", ""));

                    showData(courseID, appNo);
                    DivSearch.Visible = false;
                    BtnBack.Visible = true;
                }
            }
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
            if (ddlCourseName.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select course.";
                return false;
            }
            if (string.IsNullOrEmpty(txtAppNo.Text.Trim()) && string.IsNullOrEmpty(txtAppNo.Text.Trim()))
            {
                lblError.Visible = true;
                lblError.Text = "Please enter Application Number";
                return false;
            }
           

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showData(Int32 courseID, string appNo)
    {
        try
        {
            Resetcontrols();
            using (EConnectContext context = new EConnectContext())
            {

                /*var candidate = (from a in context.Candidates
                                 join g in context.RegistrationDetails on a.ID equals g.CandidateID
                                 where a.IsLocked == true && g.CourseID == courseID && g.RegistrationNo == regNo
                                 && a.GuardianName !=null
                                 select a).FirstOrDefault();*/
                var candidate = (from a in context.CourseRegistrationApplications 
                                 
                                 where a.FinalSubmitted  == true  && a.Number .Trim ()  == appNo 
                                 && a.GuardianName != null && a.GuardianName.Trim().Length != 0
                                 && a.affidavitNo !=null && a.affidavitUpload !=null
                                 && a.CourseID ==courseID 
                                 select a).FirstOrDefault();
                                 
                                // where (
                                // a.FinalSubmitted == true && a.CourseID == courseID && a.Number .Trim()  == appNo.Trim()
                                //&& a.GuardianName != null 
                                //&& a.GuardianName .Trim().Length!=0)
                                // select a).FirstOrDefault();
                if (candidate != null)
                {
                    //Form of candidate belonging to institute but yet not verified
                    if (candidate.ApplicantTypeID == 2 && candidate.IsVerifiedByInstitute == false)
                    {
                        ShowAlert("Candidate not verified by institute");
                        return;
                    }


                    tabphoto.Visible = true;
                    ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo);
                    imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Signature);
                    imgThumb.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.LeftThumb);
                    PnlCandidate.Visible = true;
                   // lblRegNum.Text = regNo.ToString();
                    //LblLockedOn.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                    // to get the current course
                    // lblcurrentLevel.Text = context.RegistrationDetails.Where(s => s.RegistrationNo == regNo).OrderByDescending(s => s.RegistrationDate).Select(t => t.Course.Name).FirstOrDefault();
                    LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);

                    if (string.IsNullOrEmpty(candidate.FatherName) == false && !string.IsNullOrWhiteSpace(candidate.FatherName))
                        LblFName.Text = "Mr. " + GetInitCap(candidate.FatherName);
                    else
                        LblFName.Text = "NA";
                    if (string.IsNullOrEmpty(candidate.MotherName) == false && string.IsNullOrWhiteSpace(candidate.MotherName) == false)
                        LblMName.Text = "Mrs. " + GetInitCap(candidate.MotherName);
                    else
                        LblMName.Text = "NA";

                    LblDob.Text = string.IsNullOrEmpty(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                                  !string.IsNullOrWhiteSpace(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                    //    if (((string.IsNullOrEmpty(candidate.FatherName)) || (string.IsNullOrWhiteSpace(candidate.FatherName))) && ((string.IsNullOrEmpty(candidate.MotherName)) || (string.IsNullOrWhiteSpace(candidate.MotherName))))
                    //  {
                    trGuardian.Visible = true;
                    if ((!string.IsNullOrEmpty(candidate.GuardianName)) && (!string.IsNullOrWhiteSpace(candidate.GuardianName)))
                        LblGName.Text = GetInitCap(candidate.GuardianName);
                    else
                        LblGName.Text = "NA";

                    trParent.Visible = false;
                    trAffidavit.Visible = true;
                    trShowAffidavit.Visible = true;
                    lblAffidavitDate.Text = candidate.affidavitDate.Value.ToString("dd-MMM-yyyy");
                    lblAffidavitNo.Text = candidate.affidavitNo;
                    if (candidate.affidavitVerifiedOn != null)
                        lblAffidavitVerifiedDate.Text = candidate.affidavitVerifiedOn.Value.ToString("dd-MMM-yyyy");

                    if (candidate.affidavitVerified)
                        lnkAffidavitVerified.Visible = false;
                    else
                    {
                        lnkAffidavitVerified.Visible = true;
                    }
                    //  }
                    /*          else
                              {
                                  trParent.Visible = true;
                        
                                  trGuardian.Visible = false;
                                  trAffidavit.Visible = false;
                                  trShowAffidavit.Visible = false;
                                  Trdob.Attributes.Add("class", "gdalternate1");


                              }*/
                    //Exam details
                    lblExamMonthV.Text =new System.DateTime(2010, candidate.ApplicableExam.ExamMonth, 1).ToString("MMM"); 
                   lblExamYearV.Text = candidate.ApplicableExam.ExamYear .ToString();



                    //contact details
                    var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.CandidateID ).FirstOrDefault();
                    if (contact != null)
                    {
                        LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                                       contact.EmailAddress.ToString() : "NA";
                        lblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() : "NA";
                        LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";
                    }
                    else
                    {
                        LblEmail.Text = string.IsNullOrEmpty(candidate.EmailAddress) == false && !string.IsNullOrWhiteSpace(candidate.EmailAddress) ?
                                    candidate.EmailAddress.ToString() : "NA";
                        lblMobile.Text = candidate.MobileNumber !=null && candidate.MobileNumber != 0 ? candidate.MobileNumber.ToString() : "NA";
                        LblPhone.Text = candidate.PhoneNumber.HasValue && candidate.PhoneNumber != 0 ? "0" + candidate.StdNumber.ToString() + "-" + candidate.PhoneNumber.ToString() : "NA";
                    }
                    //Address For Communication
                    int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    var address = (from c in context.Addresses
                                   where c.AddressTypeID == CorAddTypeId && c.CandidateID == candidate.CandidateID 
                                   orderby c.EffectiveDateFrom descending
                                   select c).Take(2).ToList();
                    if (address.Count  != 0)
                    {
                        Int32 count = 0;
                        foreach (var currentaddress in address)
                        {
                            if (count == 0) //current address
                            {
                                LblAdd1.Text = string.IsNullOrEmpty(currentaddress.AddressLine1) == false && !string.IsNullOrWhiteSpace(currentaddress.AddressLine1) ? GetInitCap(currentaddress.AddressLine1) : "NA";
                                LblAdd2.Text = string.IsNullOrEmpty(currentaddress.AddressLine2) == false && !string.IsNullOrWhiteSpace(currentaddress.AddressLine2) ? GetInitCap(currentaddress.AddressLine2) : "NA";
                                lblAdd3.Text = string.IsNullOrEmpty(currentaddress.AddressLine3) == false && !string.IsNullOrWhiteSpace(currentaddress.AddressLine3) ? GetInitCap(currentaddress.AddressLine3) : "NA";
                                LblCity.Text = string.IsNullOrEmpty(currentaddress.CityName) == false && !string.IsNullOrWhiteSpace(currentaddress.CityName) ? GetInitCap(currentaddress.CityName) : "NA";
                                LblState.Text = currentaddress.StateID.HasValue && currentaddress.StateID != 0 ? GetInitCap(currentaddress.State.Name) : "NA";
                                if (currentaddress.DistrictID.HasValue)
                                { LblDistrict.Text = GetInitCap(currentaddress.District.Name); }
                                else
                                { LblDistrict.Text = "NA"; }
                                LblPinCode.Text = currentaddress.PinCode.HasValue ? currentaddress.PinCode.Value.ToString() : "NA";
                                count = count + 1;
                            }

                        }
                    }
                    else
                    {
                        LblAdd1.Text = string.IsNullOrEmpty(candidate.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(candidate.CorAddressLine1) ? GetInitCap(candidate.CorAddressLine1) : "NA";
                        LblAdd2.Text = string.IsNullOrEmpty(candidate.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(candidate.CorAddressLine2) ? GetInitCap(candidate.CorAddressLine2) : "NA";
                        lblAdd3.Text = string.IsNullOrEmpty(candidate.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(candidate.CorAddressLine3) ? GetInitCap(candidate.CorAddressLine3) : "NA";
                        LblCity.Text = string.IsNullOrEmpty(candidate.CorCityName) == false && !string.IsNullOrWhiteSpace(candidate.CorCityName) ? GetInitCap(candidate.CorCityName) : "NA";
                        LblState.Text = candidate.CorStateID != null && candidate.CorStateID != 0 ? GetInitCap(candidate.CorState.Name) : "NA";
                        if (candidate.CorDistrictID.HasValue)
                        { LblDistrict.Text = GetInitCap(candidate.CorDistrict.Name); }
                        else
                        { LblDistrict.Text = "NA"; }
                        LblPinCode.Text = candidate.CorPinCode!=null ? candidate.CorPinCode.ToString() : "NA";
                    }
                    ImgBtnReset.Visible = true;
                    //if (candidate.IsVerifiedByInstitute  == false)
                    //{
                    //    LblVerifiedOn.Text = "Not Verified";
                    //    btnVerify.Visible = true;
                    //}
                    //else
                    //{
                    //    btnVerify.Visible = false;
                    //    //LblVerifiedOn.Text = candidate.VerifiedOn.Value.ToString("dd-MMM-yyyy");
                    //}
                    //if (candidate.IsSynced == false)
                    //{
                    //    LblSyncedOn.Text = "Not Synced";
                    //}
                    //else
                    //{
                    //    LblSyncedOn.Text = candidate.SyncedOn.Value.ToString("dd-MMM-yyyy");
                    //}


                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Sorry ! No such Record found who submitted Guardian Details and uploaded affidavit.";
                    PnlCandidate.Visible = false;
                    ImgBtnReset.Visible = true;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (isvalidForm())
            {
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                string appNo = txtAppNo.Text;
                showData(courseID, appNo);
                ImgBtnSearch.Visible = false;
                ImgBtnReset.Visible = true;
                txtAppNo.Enabled = false;
                ddlCourseName.Enabled = false;

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application No. " + appNo.ToString(), "#", ""));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void bindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                Int32 courseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseTypeID
                              select new { ValueField = s.ID, TextField = s.Name +" ( "+ s.Code +" )"};

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ImgBtnReset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
            BreadCrumb1.Render();
            Resetcontrols();
            PnlCandidate.Visible = false;
            // btnVerify.Visible = false;
            tabphoto.Visible = false;
            ImgBtnSearch.Visible = true;
            ImgBtnReset.Visible = false;
            txtAppNo.Enabled = true;
            ddlCourseName.Enabled = true;
            txtAppNo.Text = "";
            ddlCourseName.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Resetcontrols()
    {
        try
        {
            LblAppName.Text = "NA";
            LblAdd1.Text = "NA";
            LblAdd2.Text = "NA";
            lblAdd3.Text = "NA";
            LblDistrict.Text = "NA";
            LblDob.Text = "NA";
            LblEmail.Text = "NA";
            LblPhone.Text = "NA";
            LblPinCode.Text = "NA";
            LblCity.Text = "NA";
            LblFName.Text = "NA";
            LblMName.Text = "NA";
            lblMobile.Text = "NA";
            LblGName.Text = "NA";
            LblState.Text = "NA";
            // LblSyncedOn.Text = "NA";
           //lblRollNum .Text = "NA";
           // lblCourse.Text = "NA";
            // LblLockedOn.Text = "NA";
            // LblVerifiedOn.Text = "NA";
            lblAffidavitNo.Text = "NA";
            lblAffidavitDate.Text = "NA";

            ImgCandidatePhoto.ImageUrl = "";
            imgSignature.Src = "";
            imgThumb.Src = "";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    /*  protected void btnVerify_Click(object sender, EventArgs e)
      {
          try
          {
              BreadCrumb1.Render();
              using (EConnectContext context = new EConnectContext())
              {
                  Int32 courseID = 0;
                  string  regNo = "";
                  if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])))
                  {
                      courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                      regNo = Request.QueryString["RegNo"];
                  }
                  else
                  {
                      courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                      regNo = txtRegNo.Text;
                  }
                  var candidate = (from a in context.Candidates
                                   join g in context.RegistrationDetails on a.ID equals g.CandidateID
                                   where a.IsLocked == true && g.CourseID == courseID && g.RegistrationNo == regNo
                                   select a).FirstOrDefault();
                  candidate.IsVerified = true;
                  candidate.VerifiedOn = DateTime.Now;
                  candidate.VerifiedBy = Convert.ToInt32(Session["UserID"]);
                  context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                  context.SaveChanges();
                  string msg = "Candiate's profile details have been verified successfully.";
                  ShowAlert(msg, true);
                  showData(courseID, regNo);               
              };
          }
          catch (Exception ex)
          {
              ShowAlert(ex.Message);
          }
      }
      */
    protected void btnVerifyAffidavit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = 0;
                string appNo = "";
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["AppNo"])))
                {
                    courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    appNo = Request.QueryString["AppNo"];
                }
                else
                {
                    courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                    appNo = txtAppNo.Text;

                    var candidate = (from a in context.CourseRegistrationApplications
                                   //  join g in context.RegistrationDetails on a.ID equals g.CourseRegistrationApplicationID 
                                     where 
                                      a.CourseID == courseID && a.Number  .ToString ().Trim() == appNo.Trim()
                                     && a.affidavitDate != null && a.affidavitNo != null
                                     && a.affidavitUpload != null
                                     select a).FirstOrDefault();
                    if (candidate != null)
                    {
                        if (candidate.IsVerifiedByInstitute == false && candidate.ApplicantType.ToString ().Trim () == "2")
                        {
                            ShowAlert("Candidate not verified by institute");
                            return;
                        }
                        candidate.affidavitVerified = true;
                        candidate.affidavitVerifiedOn = DateTime.Now;
                        candidate.GuardianFlagStatusID = 3;
                        candidate.affidavitVerifiedBy = Convert.ToInt32(Session["UserID"]);
                        //Added 19 July 2019
                        
                            
                       Int64  batchItemID =Convert.ToInt64 (candidate .BatchItemID );
                        BatchItem batchItem = context.BatchItems.Find(batchItemID);
                        if (batchItem != null && batchItem.IsKeptInAbeyance )
                        {
                            //candidate.ApplicationStatusID  = Convert.ToInt32 (enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing  );
                            candidate.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT );
                            batchItem.KeptInAbeyanceReason = "";
                            batchItem.IsKeptInAbeyance = false;
                            batchItem.StatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                            context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                        }
                        //
                        context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        string msg = "Candiate's affidavit details have been verified successfully.";
                        ShowAlert(msg, true);

                        showData(courseID, appNo);

                    }
                    else
                    {
                        string msg = "Candiate's affidavit details have not been verified successfully.";
                        ShowAlert(msg, true);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AffidavitVerificationRegn.aspx?" + Request.QueryString.ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void lnkAffidavit_Click(object sender, EventArgs e)
    {
        lnkAffidavit.Attributes.Add("target", "blank");
        string appNo = "";
        

            appNo = txtAppNo .Text ;
        
        using (EConnectContext context = new EConnectContext())
        {

            var candidate = (from a in context.CourseRegistrationApplications
                            // join g in context.RegistrationDetails on a.ID equals g.CourseRegistrationApplicationID 
                             where (
                             a.Number  .ToString ().Trim () == appNo.Trim()
                            && a.GuardianName != null)
                             select a).FirstOrDefault();
            if (candidate != null)
            {
                //Form of candidate belonging to institute but yet not verified
                if (candidate.ApplicantTypeID == 2 && candidate.IsVerifiedByInstitute == false)
                {
                    ShowAlert("Candidate not verified by institute");
                    return;
                }


               // byte[] bytes = (byte[])candidate.affidavitUpload;

               // //Response.Clear();
               // //Response.ContentType = "application/pdf";
               // //Response.AddHeader("content-length", bytes.Length.ToString());
               // //Response.BinaryWrite(bytes);


               // Response.ClearContent();
               // Response.ClearHeaders();
               // Response.ContentType = "application/pdf";
               // Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now);

               // Response.BinaryWrite(bytes);
               //// Response.End();
               // Response.Flush();
               // Response.Clear();


                //Document myDocument = new Document(PageSize.LETTER);
                //PdfWriter.GetInstance(myDocument, new FileStream("D:\\mydocument.pdf", FileMode.Create));
                //myDocument.Open();
                //myDocument.Add(new Paragraph(Encoding.UTF8.GetString(bytes)));
                //myDocument.Close();
                if (candidate.affidavitUpload != null)
                    Response.Redirect("showAffidavitNew.aspx?num=" + txtAppNo.Text+ "&cat=1");
                else
                {
                    ShowAlert("Affidavit not uploaded");
                    return;
                }
            }
        }
   }
}
