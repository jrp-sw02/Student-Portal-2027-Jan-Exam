using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_ChangeApplicationVerification : BasePage
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application Verification Process", "#", ""));
                bindCourse();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])))
                {
                    Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    Int64 regNo = Convert.ToInt64(Request.QueryString["RegNo"]);
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration No. " + regNo.ToString(), "#", ""));

                    showData(courseID, regNo);
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
            if (string.IsNullOrEmpty(txtRegNo.Text.Trim()) && string.IsNullOrEmpty(txtRegNo.Text.Trim()))
            {
                lblError.Visible = true;
                lblError.Text = "Please enter Registration Number";
                return false;
            }
            if (!IsNumeric(txtRegNo.Text))
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Number.";
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showData(Int32 courseID, Int64 regNo)
    {
        try
        {
            Resetcontrols();
            using (EConnectContext context = new EConnectContext())
            {

                var candidate = (from a in context.Candidates
                                 join g in context.RegistrationDetails on a.ID equals g.CandidateID
                                 where a.IsLocked == true && g.CourseID == courseID && g.RegistrationNo == regNo
                                 select a).FirstOrDefault();
                if (candidate != null)
                {
                    tabphoto.Visible = true;
                    ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile);
                    imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Signature.BlobFile);
                    imgThumb.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.LeftThumbImpression.BlobFile);
                    PnlCandidate.Visible = true;
                    lblRegNum.Text = regNo.ToString();
                    LblLockedOn.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                    // to get the current course
                    lblcurrentLevel.Text = context.RegistrationDetails.Where(s => s.RegistrationNo == regNo).OrderByDescending(s => s.RegistrationDate).Select(t => t.Course.Name).FirstOrDefault();
                    LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                    LbloldAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
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
                    LbloldDob.Text = string.IsNullOrEmpty(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                                  !string.IsNullOrWhiteSpace(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";
                    if (((string.IsNullOrEmpty(candidate.FatherName)) || (string.IsNullOrWhiteSpace(candidate.FatherName))) && ((string.IsNullOrEmpty(candidate.MotherName)) || (string.IsNullOrWhiteSpace(candidate.MotherName))))
                    {
                        trGuardian.Visible = true;
                        if ((!string.IsNullOrEmpty(candidate.GuardianName)) && (!string.IsNullOrWhiteSpace(candidate.GuardianName)))
                            LblGName.Text = GetInitCap(candidate.GuardianName);
                        else
                            LblGName.Text = "NA";
                        trMother.Visible = false;
                        trFather.Visible = false;
                    }
                    else
                    {
                        trMother.Visible = true;
                        trFather.Visible = true;
                        trGuardian.Visible = false;
                        Trdob.Attributes.Add("class", "gdalternate1");
                    }
                    //old personal details
                    var oldpersonal = (from c in context.CandidateHistory
                                       where c.CandidateID == candidate.ID && c.CreatedByID == candidate.ID
                                       orderby c.CreatedOn descending
                                       select c).FirstOrDefault();
                    if (oldpersonal != null)
                    {
                        if (string.IsNullOrEmpty(oldpersonal.FatherName) == false && !string.IsNullOrWhiteSpace(oldpersonal.FatherName))
                            LbloldFName.Text = "Mr. " + GetInitCap(oldpersonal.FatherName);
                        else
                            LbloldFName.Text = "NA";
                        if (string.IsNullOrEmpty(oldpersonal.MotherName) == false && string.IsNullOrWhiteSpace(oldpersonal.MotherName) == false)
                            LbloldMName.Text = "Mrs. " + GetInitCap(oldpersonal.MotherName);
                        else
                            LbloldMName.Text = "NA";
                        if (((string.IsNullOrEmpty(oldpersonal.FatherName)) || (string.IsNullOrWhiteSpace(oldpersonal.FatherName))) && ((string.IsNullOrEmpty(oldpersonal.MotherName)) || (string.IsNullOrWhiteSpace(oldpersonal.MotherName))))
                        {
                            trGuardian.Visible = true;
                            if ((!string.IsNullOrEmpty(oldpersonal.GuardianName)) && (!string.IsNullOrWhiteSpace(oldpersonal.GuardianName)))
                                LbloldGName.Text = GetInitCap(oldpersonal.GuardianName);
                            else
                                LbloldGName.Text = "NA";
                        }
                    }
                    //contact details
                    var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.ID).FirstOrDefault();
                    LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                                   contact.EmailAddress.ToString() : "NA";
                    lblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() : "NA";
                    LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";
                    // old contatct details
                    var olcontact = (from c in context.CandidateContactHistoryDetails
                                     where c.CandidateID == candidate.ID && c.CreatedByID == candidate.ID
                                     orderby c.CreatedOn descending
                                     select c).FirstOrDefault();
                    if (olcontact != null)
                    {
                        LbloldEmail.Text = string.IsNullOrEmpty(olcontact.EmailAddress) == false && !string.IsNullOrWhiteSpace(olcontact.EmailAddress) ? olcontact.EmailAddress.ToString() : "NA";
                        lbloldMobile.Text = olcontact.MobileNumber.HasValue && olcontact.MobileNumber != 0 ? olcontact.MobileNumber.ToString() : "NA";
                        LbloldPhone.Text = olcontact.PhoneNumber.HasValue && olcontact.PhoneNumber != 0 ? "0" + olcontact.StdNumber.ToString() + "-" + olcontact.PhoneNumber.ToString() : "NA";
                    }
                    //Address For Communication
                    int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    var address = (from c in context.Addresses
                                   where c.AddressTypeID == CorAddTypeId && c.CandidateID == candidate.ID
                                   orderby c.EffectiveDateFrom descending
                                   select c).Take(2).ToList();
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
                        else if (count == 1) //old address
                        {
                            var oldaddress = currentaddress;
                            LbloldAdd1.Text = string.IsNullOrEmpty(oldaddress.AddressLine1) == false && !string.IsNullOrWhiteSpace(oldaddress.AddressLine1) ? GetInitCap(oldaddress.AddressLine1) : "NA";
                            LbloldAdd2.Text = string.IsNullOrEmpty(oldaddress.AddressLine2) == false && !string.IsNullOrWhiteSpace(oldaddress.AddressLine2) ? GetInitCap(oldaddress.AddressLine2) : "NA";
                            lbloldAdd3.Text = string.IsNullOrEmpty(oldaddress.AddressLine3) == false && !string.IsNullOrWhiteSpace(oldaddress.AddressLine3) ? GetInitCap(oldaddress.AddressLine3) : "NA";
                            LbloldCity.Text = string.IsNullOrEmpty(oldaddress.CityName) == false && !string.IsNullOrWhiteSpace(oldaddress.CityName) ? GetInitCap(oldaddress.CityName) : "NA";
                            LbloldState.Text = oldaddress.StateID.HasValue && oldaddress.StateID != 0 ? GetInitCap(oldaddress.State.Name) : "NA";
                            if (oldaddress.DistrictID.HasValue)
                            { LbloldDistrict.Text = GetInitCap(oldaddress.District.Name); }
                            else
                            { LbloldDistrict.Text = "NA"; }
                            LbloldPinCode.Text = oldaddress.PinCode.HasValue ? oldaddress.PinCode.Value.ToString() : "NA";
                        }
                    }
                    ImgBtnReset.Visible = true;
                    if (candidate.IsVerified == false)
                    {
                        LblVerifiedOn.Text = "Not Verified";
                        btnVerify.Visible = true;
                    }
                    else
                    {
                        btnVerify.Visible = false;
                        LblVerifiedOn.Text = candidate.VerifiedOn.Value.ToString("dd-MMM-yyyy");
                    }
                    if (candidate.IsSynced == false)
                    {
                        LblSyncedOn.Text = "Not Synced";
                    }
                    else
                    {
                        LblSyncedOn.Text = candidate.SyncedOn.Value.ToString("dd-MMM-yyyy");
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Sorry ! No such Record found.";
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
                Int64 regNo = Convert.ToInt64(txtRegNo.Text);
                showData(courseID, regNo);
                ImgBtnSearch.Visible = false;
                ImgBtnReset.Visible = true;
                txtRegNo.Enabled = false;
                ddlCourseName.Enabled = false;
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration No. " + regNo.ToString(), "#", ""));
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
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseTypeID
                              select new { ValueField = s.ID, TextField = s.Name };

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
            btnVerify.Visible = false;
            tabphoto.Visible = false;
            ImgBtnSearch.Visible = true;
            ImgBtnReset.Visible = false;
            txtRegNo.Enabled = true;
            ddlCourseName.Enabled = true;
            txtRegNo.Text = "";
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
            LblSyncedOn.Text = "NA";
            lblRegNum.Text = "NA";
            lblcurrentLevel.Text = "NA";
            LblLockedOn.Text = "NA";
            LblVerifiedOn.Text = "NA";
            LbloldAppName.Text = "NA";
            LbloldAdd1.Text = "NA";
            LbloldAdd2.Text = "NA";
            lbloldAdd3.Text = "NA";
            LbloldDistrict.Text = "NA";
            LbloldDob.Text = "NA";
            LbloldEmail.Text = "NA";
            LbloldPhone.Text = "NA";
            LbloldPinCode.Text = "NA";
            LbloldCity.Text = "NA";
            LbloldFName.Text = "NA";
            LbloldMName.Text = "NA";
            lbloldMobile.Text = "NA";
            LbloldGName.Text = "NA";
            LbloldState.Text = "NA";
            ImgCandidatePhoto.ImageUrl = "";
            imgSignature.Src = "";
            imgThumb.Src = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = 0;
                Int64 regNo = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])))
                {
                    courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    regNo = Convert.ToInt64(Request.QueryString["RegNo"]);
                }
                else
                {
                    courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                    regNo = Convert.ToInt64(txtRegNo.Text);
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
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("ChangeAppStatus.aspx?" + Request.QueryString.ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}