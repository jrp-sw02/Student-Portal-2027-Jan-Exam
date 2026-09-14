using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class myprofile : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Profile", "CAND/myprofile.aspx", ""));
                Sidelink.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
                Sidelink.Items.Add(new SideLinkItem("Print Form", "Application.aspx", "", "_blank"));
                Sidelink.Render();
                showdata();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected Boolean UpdateLock()
    {
        try
        {
            int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
            using (EConnectContext context = new EConnectContext())
            {
                var IsNotNullCand = (from a in context.Candidates
                                     where a.ID == entityID && (
                                     (a.FatherName != null && a.FatherName.Trim() != "" &&
                                     a.MotherName != null && a.MotherName.Trim() != "") || (a.GuardianName != null && a.GuardianName.Trim() != "")) &&
                                     a.Gender != null && a.Gender.Trim() != "" &&
                                     a.PhotoFileID != null && a.PhotoFileID != 0 &&
                                     a.SignatureFileID != null && a.SignatureFileID != 0 &&
                                     a.LeftThumbImpressionFileID != null && a.LeftThumbImpressionFileID != 0
                                     select a);

                var IsNotNullCorAddress = (from a in context.Addresses
                                           where a.CandidateID == entityID && a.AddressTypeID == CorAddTypeId &&
                                           a.AddressLine1 != null && a.AddressLine1.Trim() != "" &&
                                           a.CityName != null && a.CityName.Trim() != "" &&
                                           a.DistrictID != null && a.DistrictID != 0 &&
                                           a.StateID != null && a.StateID != 0 &&
                                           a.PinCode != null && a.PinCode != 0
                                           select a);

                var IsNotNullContact = (from a in context.CandidateContactDetails
                                        where a.CandidateID == entityID &&
                                        a.EmailAddress != null && a.EmailAddress.Trim() != "" &&
                                        a.MobileNumber != null && a.MobileNumber != 0 &&
                                        a.IsEmailVerified == true
                                        select a);

                if (IsNotNullCand.Count() > 0 && IsNotNullContact.Count() > 0 && IsNotNullCorAddress.Count() > 0)
                { return true; }
                else
                { return false; }
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = context.Candidates.Find(entityID);

                LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                if (string.IsNullOrEmpty(candidate.GuardianName) == true)
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    if (string.IsNullOrEmpty(candidate.FatherName) == false)
                    { LblFatherName.Text = "Mr. " + GetInitCap(candidate.FatherName); }
                    else
                    { LblFatherName.Text = "<font color='red'> (Not Completed) </font>"; }
                    if (string.IsNullOrEmpty(candidate.MotherName) == false)
                    { LblMotherName.Text = "Mrs. " + GetInitCap(candidate.MotherName); }
                    else
                    { LblMotherName.Text = "<font color='red'> (Not Completed) </font>"; }
                }
                else
                {
                    LblGuardianName.Text = string.IsNullOrEmpty(candidate.GuardianName) == false ? GetInitCap(candidate.GuardianName) : "<font color='red'> (Not Completed) </font>";
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    TrName.Attributes.Add("class", "gdrow1");
                }

                LblGender.Text = string.IsNullOrEmpty(candidate.Gender) == false ? GetInitCap(candidate.Gender) : "<font color='red'> (Not Completed) </font>";
                LblMaritalStatus.Text = GetInitCap((candidate.MaritalStatusID.HasValue) && (candidate.MaritalStatusID != 0) ? candidate.MaritalStatus.Name : "NA");
                LblDob.Text = string.IsNullOrEmpty(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) == false ? candidate.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";
                LblCategory.Text = GetInitCap(candidate.CastCategoryID.HasValue ? candidate.CastCategory.Name : "NA");

                if (candidate.Photo != null)
                { ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile); }
                LblIsHandicaped.Text = candidate.IsHandicaped == false ? "No" : "Yes";
                LblIsExServicemane.Text = candidate.IsExServicemane == false ? "No" : "Yes";

                int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                var contact = candidate.ContactDetails.FirstOrDefault();
                string Mstatus = "";
                string Estatus = "";
                if (candidate.IsVerified == false)
                {
                    FldStSideVrifiedMessage.Visible = false;
                    LnkBtnVerifyMobile.Visible = false;
                    LnkBtnVerifyEmail.Visible = false;
                    if (contact != null)
                    {
                        if (contact.MobileNumber.HasValue && contact.MobileNumber != 0)
                        {
                            if (contact.IsMobileNumberVerified == false)
                            {
                                LnkBtnVerifyMobile.Visible = true;
                                Mstatus = "&nbsp;&nbsp;(<font color=Red> Not Verified</font>)";
                            }
                            else
                            {
                                Mstatus = "";
                                LnkBtnVerifyMobile.Visible = false;
                            }
                        }
                        else
                            LnkBtnVerifyMobile.Visible = false;

                        if (string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress))
                        {
                            if (contact.IsEmailVerified == false)
                            {
                                Estatus = "&nbsp;&nbsp;(<font color=Red> Not Verified</font>)";
                                LnkBtnVerifyEmail.Visible = true;
                            }
                            else
                            {
                                Estatus = "";
                                LnkBtnVerifyEmail.Visible = false;
                            }
                        }
                        else
                            LnkBtnVerifyEmail.Visible = false;
                    }
                    if ((!candidate.PhotoFileID.HasValue || candidate.PhotoFileID == 0) && (!candidate.SignatureFileID.HasValue || candidate.SignatureFileID == 0) && (!candidate.LeftThumbImpressionFileID.HasValue || candidate.LeftThumbImpressionFileID == 0))
                    { LnkBtnPhotoDetail.Visible = true; }
                    else
                    { LnkBtnPhotoDetail.Visible = false; }
                    if (UpdateLock() && candidate.IsLocked == false)
                    {
                        LnkBtnLockDetail.Visible = true;
                        FldStSideLockMessage.Visible = false;
                        FldStSideNotLockMessage.Visible = true;
                        FldStSideincompleteMessage.Visible = false;

                        Boolean IsMobileNumberVerified = (from a in candidate.ContactDetails
                                                          select a.IsMobileNumberVerified).FirstOrDefault();
                        if (IsMobileNumberVerified == false)
                        {

                        }
                    }
                    else if (!UpdateLock() && candidate.IsLocked == false)
                    {
                        LnkBtnLockDetail.Visible = false;
                        LnkBtnPersonalDetail.Visible = true;
                        FldStSideLockMessage.Visible = false;
                        FldStSideNotLockMessage.Visible = false;
                        FldStSideincompleteMessage.Visible = true;
                    }
                    else if (UpdateLock() && candidate.IsLocked == true)
                    {
                        LnkBtnLockDetail.Visible = false;
                        LnkBtnPersonalDetail.Visible = false;
                        LnkBtnContactDetail.Visible = false;
                        LnkBtnCorAddressDetail.Visible = false;
                        FldStSideLockMessage.Visible = true;
                        LblLockedOn.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                        FldStSideNotLockMessage.Visible = false;
                        FldStSideincompleteMessage.Visible = false;
                    }
                    else
                    { LnkBtnLockDetail.Visible = false; }
                }
                else if (candidate.IsVerified == true)
                {
                    LnkBtnLockDetail.Visible = false;
                    LnkBtnPersonalDetail.Visible = false;
                    LnkBtnContactDetail.Visible = false;
                    LnkBtnCorAddressDetail.Visible = false;
                    FldStSideLockMessage.Visible = false;
                    FldStSideNotLockMessage.Visible = false;
                    FldStSideincompleteMessage.Visible = false;
                    Sidelink.Visible = false;
                    LnkBtnVerifyEmail.Visible = false;
                    LnkBtnVerifyMobile.Visible = false;
                    LnkBtnPhotoDetail.Visible = false;
                    FldStSideVrifiedMessage.Visible = true;
                    lblVerifiedOn.Text = candidate.VerifiedOn.Value.ToString("dd-MMM-yyyy");
                }
                if (contact != null)
                {
                    LblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() + "" + Mstatus : "<font color='red'> (Not Completed) </font>";
                    LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";
                    LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ? contact.EmailAddress.ToString() + "" + Estatus : "<font color='red'> (Not Completed) </font>";
                }
                var CorAdd = (from cad in candidate.Addresses
                              where cad.AddressTypeID == CorAddTypeId
                              orderby cad.EffectiveDateFrom descending
                              select cad).FirstOrDefault();

                if (CorAdd != null)
                {
                    //Corespondance Address
                    LblCorAddressLine1.Text = string.IsNullOrEmpty(CorAdd.AddressLine1) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine1) ? GetInitCap(CorAdd.AddressLine1) : "<font color='red'> (Not Completed) </font>";
                    LbCorlAddressLine2.Text = string.IsNullOrEmpty(CorAdd.AddressLine2) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine2) ? GetInitCap(CorAdd.AddressLine2) : "NA";
                    LblCorAddressLine3.Text = string.IsNullOrEmpty(CorAdd.AddressLine3) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine3) ? GetInitCap(CorAdd.AddressLine3) : "NA";
                    LblCityName.Text = string.IsNullOrEmpty(CorAdd.CityName) == false && !string.IsNullOrWhiteSpace(CorAdd.CityName) ? GetInitCap(CorAdd.CityName) : "<font color='red'> (Not Completed) </font>";
                    LblState.Text = CorAdd.StateID.HasValue && CorAdd.StateID != 0 ? CorAdd.State.Name : "<font color='red'> (Not Completed) </font>";
                    if (CorAdd.DistrictID.HasValue)
                    { LblDistrict.Text = GetInitCap(CorAdd.District.Name); }
                    else
                    { LblDistrict.Text = "<font color='red'> (Not Completed) </font>"; }
                    LblPincode.Text = CorAdd.PinCode.HasValue ? CorAdd.PinCode.Value.ToString() : "<font color='red'> (Not Completed) </font>";
                }
                int PerAddTypeId = Convert.ToInt32(enmAddressType.PermanentAddress);
                var PerAdd = context.Addresses.Where(a => a.CandidateID == candidate.ID && a.AddressTypeID == PerAddTypeId).FirstOrDefault();
                //Permanent Address
                LblPerAddressLine1.Text = string.IsNullOrEmpty(PerAdd.AddressLine1) == false && !string.IsNullOrWhiteSpace(PerAdd.AddressLine1) ? GetInitCap(PerAdd.AddressLine1) : "NA";
                LblPerAddressLine2.Text = string.IsNullOrEmpty(PerAdd.AddressLine2) == false && !string.IsNullOrWhiteSpace(PerAdd.AddressLine2) ? GetInitCap(PerAdd.AddressLine2) : "NA";
                LblPerAddressLine3.Text = string.IsNullOrEmpty(PerAdd.AddressLine3) == false && !string.IsNullOrWhiteSpace(PerAdd.AddressLine3) ? GetInitCap(PerAdd.AddressLine3) : "NA";
                LblPerCityName.Text = string.IsNullOrEmpty(PerAdd.CityName) == false && !string.IsNullOrWhiteSpace(PerAdd.CityName) ? GetInitCap(PerAdd.CityName) : "NA";
                LblPerState.Text = PerAdd.StateID.HasValue && PerAdd.StateID != 0 ? GetInitCap(PerAdd.State.Name) : "NA";
                if (PerAdd.DistrictID.HasValue)
                { LblPerDistrict.Text = GetInitCap(PerAdd.District.Name); }
                else
                { LblPerDistrict.Text = "NA"; }
                LblPerPinCode.Text = PerAdd.PinCode.HasValue ? PerAdd.PinCode.Value.ToString() : "NA";

                DateTime EduMaxEffectiveDate = (from c in candidate.EducationalQualifications
                                                select c.EffectiveFromDate).Max();

                var Qualification = context.CandidateQualificationDetails.Where(a => a.CandidateID == candidate.ID && a.EffectiveFromDate == EduMaxEffectiveDate).FirstOrDefault();
                LblHeighestEducation.Text = Qualification.EducationalQualificationID != 0 ? GetInitCap(Qualification.EducationalQualification.Name) : "NA";
                LblYrOfPassing.Text = string.IsNullOrEmpty(Qualification.PassingYear.ToString()) == false ? Qualification.PassingYear.ToString() : "NA";

                var reg = candidate.RegistrationDetails.OrderByDescending(q => q.RegistrationDate).FirstOrDefault();
                if (reg != null)
                {
                    LblRegNo.Text = reg.RegistrationNo.ToString();
                    LblRegDate.Text = reg.RegistrationDate.ToString("dd-MMM-yyyy");
                    LblValidUpto.Text = reg.ValidUptoDate.ToString("dd-MMM-yyyy");
                    LblCourse.Text = GetInitCap(reg.Course.Name);
                    LblCandidateType.Text = reg.ApplicantTypeID != 0 ? GetInitCap(reg.ApplicantType.Name) : "NA";
                    if (reg.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        TrAccCentre.Visible = true;
                        LblAccCentre.Text = reg.Institute.AccreditationDetails.Where(c => c.CourseID == reg.CourseID).FirstOrDefault().AccreditationNumber.ToString() + " - " + GetInitCap(reg.Institute.Name);
                    }
                    else
                    {
                        TrAccCentre.Visible = false;
                    }
                }
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void LnkBtnPersonalDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=1");
    }
    protected void LnkBtnCorAddressDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=3");
    }
    protected void LnkBtnContactDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=2");
    }
    protected void LnkBtnPerAddressDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=4");
    }
    protected void LnkBtnEducatiion_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=5");
    }
    protected void LnkBtnAccCentreDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=6");
    }
    protected void LnkBtnVerifyMobile_Click(object sender, EventArgs e)
    {
        try
        {
            CommonFunctions.GenerateMobileOTP(UserType.Candidate, entityID);
            Response.Redirect("OTPprocess.aspx?verify=mobile");
        }
        catch (Exception ex)
        {
            ShowAlert("Mail could not be sent due to some technical reason.");
        }
    }
    protected void LnkBtnVerifyEmail_Click(object sender, EventArgs e)
    {
        try
        {
            CommonFunctions.GenerateEmailOTP(UserType.Candidate, entityID);
            Response.Redirect("OTPprocess.aspx?verify=email");
        }
        catch (Exception ex) { throw ex; }
    }
    protected void LnkBtnPhotoDetail_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = context.Candidates.Find(entityID);
                candidate.TempPhoto = null;
                candidate.PhotoFileName = null;
                candidate.TempSignature = null;
                candidate.SignatureFileName = null;
                candidate.TempLeftThumb = null;
                candidate.LeftThumbFileName = null;
                context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            };
            Response.Redirect("EditUploadPhoto.aspx");
        }
        catch (Exception ex) { throw ex; }
    }
    protected void ToggleImage(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            string arg = btn.CommandArgument.ToLower();
            using (EConnectContext context = new EConnectContext())
            {

                var candidate = context.Candidates.Find(entityID);
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
                    if (candidate.PhotoFileID.HasValue)
                    { ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile); }
                }
                if (LblPhotoCaption.Text.ToLower() == "signature")
                {
                    ImgBtnPrevious.ToolTip = "Click to view photograph";
                    ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                    if (candidate.SignatureFileID.HasValue)
                    {
                        ImgCandidatePhoto.Height = 50;
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Signature.BlobFile);
                    }
                }
                else if (LblPhotoCaption.Text.ToLower() == "thumb")
                {
                    ImgBtnPrevious.ToolTip = "Click to view signature";
                    ImgBtnNext.ToolTip = "";
                    if (candidate.LeftThumbImpressionFileID.HasValue)
                    {
                        ImgCandidatePhoto.Height = 60;
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.LeftThumbImpression.BlobFile);
                    }
                }
            };

        }
        catch (Exception ex) { throw ex; }
    }
    protected void LnkBtnLockDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConfirmLock.aspx");
    }
}