using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.IO;
using System.Text;

public partial class HO_AffidavitEntry : BasePage
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
            //BtnBack.Visible = false;
            if (!IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Affidavit Entry Process (Old cases)", "#", ""));
                //bindCourse();
                BindCourseType();

                //Added for candidates
                if (currentRoleId == 3)
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        var candidate = (from a in context.CourseRegistrationApplications
                                         join g in context.Users on  a.CandidateID.Value   equals g.UserRefNumber 

                                         where (
                                         a.FinalSubmitted == true
                                        && a.GuardianName != null
                                        && a.GuardianName.Trim().Length != 0
                                        && a.affidavitDate == null
                                        && a.affidavitNo == null
                                        && g.UserRefNumber  ==entityID
                                        )
                                         select a).FirstOrDefault();
                        if (candidate == null)
                        {
                            //Added for old cases updates
                            var candidate1 = (from a in context.Candidates 
                                             join g in context.Users on a.ID  equals g.UserRefNumber
                                             join p in context.CourseRegistrationApplications on a.ID equals p.CandidateID 
                                             where (
                                             p.FinalSubmitted == true &&
                                            a.GuardianName != null
                                            && a.GuardianName.Trim().Length != 0
                                            && p.affidavitDate == null
                                            && p.affidavitNo == null
                                            && g.UserRefNumber == entityID
                                            )
                                             select  a).FirstOrDefault ();
                            if (candidate1 == null)
                            {
                                ShowAlert("Affidavit already uploaded or form not completely submitted with guardian name");
                                DivSearch.Visible = false;
                                return;
                            }
                        }
                        Int32 courseType = candidate.CourseCategoryID;
                        Int32 courseID = candidate.CourseID;
                        var regn = (from b in context.RegistrationDetails
                                    where b.CandidateID.ToString().Trim() == candidate.CandidateID.ToString().Trim()
                                    
                                    select b).FirstOrDefault();
                        string regNo = regn.RegistrationNo.ToString();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration No. " + regNo.ToString(), "#", ""));
                        ddlCourseType.SelectedValue = courseType.ToString();
                        ddlCourseType_SelectedIndexChanged(sender, e);
                        ddlCourseName.SelectedValue = courseID.ToString();
                        txtRegNo.Text = regNo.ToString();
                        showData(courseType, courseID, regNo);
                        ddlCourseName.SelectedValue = courseID.ToString();
                        //lblCourse.Text = courseID.ToString();
                        DivSearch.Visible  = false;
                        ImgBtnReset.Visible = false;
                        ShowAlert("Please send the hard copy of affidavt to NIELIT HQ Regn. section by post for verification");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseTypeID"])))
                    {
                        Int32 courseType = Convert.ToInt32(Request.QueryString["CourseTypeID"]);
                        Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                        string regNo = Convert.ToString(Request.QueryString["RegNo"]);
                       // BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration/Roll No. " + regNo.ToString(), "#", ""));

                        showData(courseType, courseID, regNo);
                        ddlCourseName.SelectedValue = courseID.ToString();
                        DivSearch.Visible = false;
                        // BtnBack.Visible = true;
                    }
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

            if (ddlCourseType.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select course Type";
                return false;
            }

            if (ddlCourseName.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select course.";
                return false;
            }
            if (string.IsNullOrEmpty(txtRegNo.Text.Trim()) && string.IsNullOrEmpty(txtRegNo.Text.Trim()))
            {
                lblError.Visible = true;
                if (ddlCourseType.SelectedValue == "1")
                    lblError.Text = "Please enter Registration Number";
                else
                    lblError.Text = "Please enter Roll Number";
                return false;
            }
            if (ddlCourseType.SelectedValue == "1")
            {
                if (!IsNumeric(txtRegNo.Text))
                {
                    lblError.Visible = true;
                    lblError.Text = "Registration Number should be numeric";
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlCourseType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseType
                              select new { ValueField = s.ID, TextField = s.Name +" ("+s.Code +")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            };
            ddlCourseName.SelectedValue = "0";

            if (courseType.ToString() == "1")
                lblRegNo.Text = "Registration No.";

            if (courseType.ToString() == "2")
                lblRegNo.Text = "Roll No.";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void BindCourseType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courseType = from s in context.CourseTypes

                                 select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseType, courseType.Distinct(), lst);


            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void showData(Int32 courseTypeID, Int32 courseID, string regNo)
    {
        try
        {
            Resetcontrols();
            using (EConnectContext context = new EConnectContext())
            {
                if (ddlCourseType.SelectedValue == "1")
                {
                    Label4.Text = "Registration Number:";
                    /*var candidate = (from a in context.Candidates
                                     join g in context.RegistrationDetails on a.ID equals g.CandidateID
                                     where a.IsLocked == true && g.CourseID == courseID && g.RegistrationNo == regNo
                                     && a.GuardianName !=null
                                     select a).FirstOrDefault();*/
                    var candidate = (from a in context.CourseRegistrationApplications
                                     join g in context.RegistrationDetails on a.CandidateID equals g.CandidateID
                                     where (
                                     a.FinalSubmitted == true && a.CourseID == courseID && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                    && a.GuardianName != null
                                    && a.GuardianName.Trim().Length != 0
                                    && a.affidavitDate == null
                                    && a.affidavitNo == null
								&& g.CourseRegistrationApplicationID ==a.ID 
                                    )
                                     select a).FirstOrDefault();
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
                        lblRegNum.Text = regNo.ToString();
                        //LblLockedOn.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                        // to get the current course
                        lblcurrentLevel.Text = context.RegistrationDetails.Where(s => s.RegistrationNo.ToString() == regNo).OrderByDescending(s => s.RegistrationDate).Select(t => t.Course.Name).FirstOrDefault();
                        LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);

                        //if (string.IsNullOrEmpty(candidate.FatherName) == false && !string.IsNullOrWhiteSpace(candidate.FatherName))
                        //    LblFName.Text = "Mr. " + GetInitCap(candidate.FatherName);
                        //else
                        //    LblFName.Text = "NA";
                        //if (string.IsNullOrEmpty(candidate.MotherName) == false && string.IsNullOrWhiteSpace(candidate.MotherName) == false)
                        //    LblMName.Text = "Mrs. " + GetInitCap(candidate.MotherName);
                        //else
                        //    LblMName.Text = "NA";

                        LblDob.Text = string.IsNullOrEmpty(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                                      !string.IsNullOrWhiteSpace(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                        //    if (((string.IsNullOrEmpty(candidate.FatherName)) || (string.IsNullOrWhiteSpace(candidate.FatherName))) && ((string.IsNullOrEmpty(candidate.MotherName)) || (string.IsNullOrWhiteSpace(candidate.MotherName))))
                        //  {
                        trGuardian.Visible = true;
                        if ((!string.IsNullOrEmpty(candidate.GuardianName)) && (!string.IsNullOrWhiteSpace(candidate.GuardianName)))
                            LblGName.Text = GetInitCap(candidate.GuardianName);
                        else
                            LblGName.Text = "NA";

                        // trParent.Visible = false;
                        trAffidavit.Visible = true;
                        trShowAffidavit.Visible = true;
                        // lblAffidavitDate.Text = candidate.affidavitDate.Value.ToString("dd-MMM-yyyy");
                        // lblAffidavitNo.Text = candidate.affidavitNo;
                        //if (candidate.affidavitVerifiedOn != null)
                        //    lblAffidavitVerifiedDate.Text = candidate.affidavitVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        //if (candidate.affidavitVerified)
                        //    lnkAffidavitVerified.Visible = false;
                        //else
                        //{
                        //    lnkAffidavitVerified.Visible = true;
                        //}
                        //  }
                        /*          else
                                  {
                                      trParent.Visible = true;
                        
                                      trGuardian.Visible = false;
                                      trAffidavit.Visible = false;
                                      trShowAffidavit.Visible = false;
                                      Trdob.Attributes.Add("class", "gdalternate1");


                                  }*/

                        //contact details
                        var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.CandidateID).FirstOrDefault();
                        LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                                       contact.EmailAddress.ToString() : "NA";
                        lblMobile.Text = (contact.MobileNumber != null && contact.MobileNumber != 0) ? contact.MobileNumber.ToString() : "NA";
                        LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";

                        //Address For Communication
                        int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        var address = (from c in context.Addresses
                                       where c.AddressTypeID == CorAddTypeId && c.CandidateID == candidate.CandidateID
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

                        //Added for old cases updates
                        var candidate1 = (from a in context.Candidates
                                          join g in context.RegistrationDetails on a.ID equals g.CandidateID
                                          join p in context.CourseRegistrationApplications on a.ID equals p.CandidateID
                                          where (
                                          p.FinalSubmitted == true &&
                                         a.GuardianName != null
                                         && a.GuardianName.Trim().Length != 0
                                         && p.affidavitDate == null
                                         && p.affidavitNo == null
                                         && g.CandidateID  == p.CandidateID 
                                         &&g.CourseID ==p.CourseID
                                       && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                         )
                                          select a).FirstOrDefault();
                        if (candidate1 == null)
                        {
                            lblError.Visible = true;
                            lblError.Text = "Sorry ! No such Record found who submitted Guardian Details. or affidavit already uploaded";
                            PnlCandidate.Visible = false;
                            ImgBtnReset.Visible = true;
                            return;
                        }

                        var candidate2= (from a in context.CourseRegistrationApplications
                                         join g in context.RegistrationDetails on a.CandidateID equals g.CandidateID
                                         where (
                                         a.FinalSubmitted == true && a.CourseID == courseID 
                                         && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                        && a.affidavitDate == null
                                        && a.affidavitNo == null
                                    && g.CourseRegistrationApplicationID == a.ID
                                        )
                                         select a).FirstOrDefault();
                            tabphoto.Visible = true;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate2.Photo);
                            imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate2.Signature);
                            imgThumb.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate2.LeftThumb);
                            PnlCandidate.Visible = true;
                            lblRegNum.Text = regNo.ToString();
                            //LblLockedOn.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                            // to get the current course
                            lblcurrentLevel.Text = context.RegistrationDetails.Where(s => s.RegistrationNo.ToString() == regNo).OrderByDescending(s => s.RegistrationDate).Select(t => t.Course.Name).FirstOrDefault();
                            LblAppName.Text = candidate2.Salutation + " " + GetInitCap(candidate2.Name);

                            //if (string.IsNullOrEmpty(candidate.FatherName) == false && !string.IsNullOrWhiteSpace(candidate.FatherName))
                            //    LblFName.Text = "Mr. " + GetInitCap(candidate.FatherName);
                            //else
                            //    LblFName.Text = "NA";
                            //if (string.IsNullOrEmpty(candidate.MotherName) == false && string.IsNullOrWhiteSpace(candidate.MotherName) == false)
                            //    LblMName.Text = "Mrs. " + GetInitCap(candidate.MotherName);
                            //else
                            //    LblMName.Text = "NA";

                            LblDob.Text = string.IsNullOrEmpty(candidate2.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                                          !string.IsNullOrWhiteSpace(candidate2.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate2.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                            //    if (((string.IsNullOrEmpty(candidate.FatherName)) || (string.IsNullOrWhiteSpace(candidate.FatherName))) && ((string.IsNullOrEmpty(candidate.MotherName)) || (string.IsNullOrWhiteSpace(candidate.MotherName))))
                            //  {
                            trGuardian.Visible = true;
                            if ((!string.IsNullOrEmpty(candidate1.GuardianName)) && (!string.IsNullOrWhiteSpace(candidate1.GuardianName)))
                                LblGName.Text = GetInitCap(candidate1.GuardianName);
                            else
                                LblGName.Text = "NA";

                            // trParent.Visible = false;
                            trAffidavit.Visible = true;
                            trShowAffidavit.Visible = true;
                            // lblAffidavitDate.Text = candidate.affidavitDate.Value.ToString("dd-MMM-yyyy");
                            // lblAffidavitNo.Text = candidate.affidavitNo;
                            //if (candidate.affidavitVerifiedOn != null)
                            //    lblAffidavitVerifiedDate.Text = candidate.affidavitVerifiedOn.Value.ToString("dd-MMM-yyyy");

                            //if (candidate.affidavitVerified)
                            //    lnkAffidavitVerified.Visible = false;
                            //else
                            //{
                            //    lnkAffidavitVerified.Visible = true;
                            //}
                            //  }
                            /*          else
                                      {
                                          trParent.Visible = true;

                                          trGuardian.Visible = false;
                                          trAffidavit.Visible = false;
                                          trShowAffidavit.Visible = false;
                                          Trdob.Attributes.Add("class", "gdalternate1");


                                      }*/

                            //contact details
                            var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate2.CandidateID).FirstOrDefault();
                            LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                                           contact.EmailAddress.ToString() : "NA";
                            lblMobile.Text = (contact.MobileNumber != null && contact.MobileNumber != 0) ? contact.MobileNumber.ToString() : "NA";
                            LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";

                            //Address For Communication
                            int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                            var address = (from c in context.Addresses
                                           where c.AddressTypeID == CorAddTypeId && c.CandidateID == candidate2.CandidateID
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

                            }
                            ImgBtnReset.Visible = true;




                        }
                    
                }
                   if(ddlCourseType .SelectedValue .ToString ()=="2")
                    {
                        Label4.Text = "Roll Number:";
                        var candidate1 = (from a in context.CertificateExamApplications

                                          where (
                                          a.FinalSubmitted == true && a.CourseID == courseID && a.RollNumber.Trim() == regNo.Trim()
                                         && a.GuardianName != null
                                         && a.GuardianName.Trim().Length != 0)
                                       //   && a.affidavitDate == null
                                       //     && a.affidavitNo == null)
                                          select a).FirstOrDefault();
                        if (candidate1 != null)
                        {
                            //Form of candidate belonging to institute but yet not verified
                            if (candidate1.ApplicantTypeID == 2 && candidate1.IsVerifiedByInstitute == false)
                            {
                                ShowAlert("Candidate not verified by institute");
                                return;
                            }


                            tabphoto.Visible = true;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate1.Photo);
                            imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate1.Signature);
                            imgThumb.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate1.LeftThumb);
                            PnlCandidate.Visible = true;
                            lblRegNum.Text = regNo.ToString();
                            //LblLockedOn.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                            // to get the current course
                            // lblcurrentLevel.Text = context.RegistrationDetails.Where(s => s.RegistrationNo == regNo).OrderByDescending(s => s.RegistrationDate).Select(t => t.Course.Name).FirstOrDefault();
                            LblAppName.Text = candidate1.Salutation + " " + GetInitCap(candidate1.Name);

                            //if (string.IsNullOrEmpty(candidate1.FatherName) == false && !string.IsNullOrWhiteSpace(candidate1.FatherName))
                            //    LblFName.Text = "Mr. " + GetInitCap(candidate1.FatherName);
                            //else
                            //    LblFName.Text = "NA";
                            //if (string.IsNullOrEmpty(candidate1.MotherName) == false && string.IsNullOrWhiteSpace(candidate1.MotherName) == false)
                            //    LblMName.Text = "Mrs. " + GetInitCap(candidate1.MotherName);
                            //else
                            //    LblMName.Text = "NA";

                            LblDob.Text = string.IsNullOrEmpty(candidate1.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                                          !string.IsNullOrWhiteSpace(candidate1.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate1.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                            //    if (((string.IsNullOrEmpty(candidate.FatherName)) || (string.IsNullOrWhiteSpace(candidate.FatherName))) && ((string.IsNullOrEmpty(candidate.MotherName)) || (string.IsNullOrWhiteSpace(candidate.MotherName))))
                            //  {
                            trGuardian.Visible = true;
                            if ((!string.IsNullOrEmpty(candidate1.GuardianName)) && (!string.IsNullOrWhiteSpace(candidate1.GuardianName)))
                                LblGName.Text = GetInitCap(candidate1.GuardianName);
                            else
                                LblGName.Text = "NA";

                            //  trParent.Visible = false;
                            trAffidavit.Visible = true;
                            trShowAffidavit.Visible = true;
                            // lblAffidavitDate.Text = candidate1.affidavitDate.Value.ToString("dd-MMM-yyyy");
                            //  lblAffidavitNo.Text = candidate1.affidavitNo;
                            //if (candidate.affidavitVerifiedOn != null)
                            //    lblAffidavitVerifiedDate.Text = candidate1.affidavitVerifiedOn.Value.ToString("dd-MMM-yyyy");

                            //if (candidate.affidavitVerified)
                            //    lnkAffidavitVerified.Visible = false;
                            //else
                            //{
                            //    lnkAffidavitVerified.Visible = true;
                            //}
                            //  }
                            /*          else
                                      {
                                          trParent.Visible = true;
                        
                                          trGuardian.Visible = false;
                                          trAffidavit.Visible = false;
                                          trShowAffidavit.Visible = false;
                                          Trdob.Attributes.Add("class", "gdalternate1");


                                      }*/

                            //contact details
                           // var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate1.ID).FirstOrDefault();
                            LblEmail.Text = string.IsNullOrEmpty(candidate1.EmailAddress) == false && !string.IsNullOrWhiteSpace(candidate1.EmailAddress) ?
                                           candidate1.EmailAddress.ToString() : "NA";
                            lblMobile.Text = candidate1.MobileNumber!=null && candidate1.MobileNumber != 0 ? candidate1.MobileNumber.ToString() : "NA";
                            LblPhone.Text = candidate1.PhoneNumber.HasValue && candidate1.PhoneNumber != 0 ? "0" + candidate1.StdNumber.ToString() + "-" + candidate1.PhoneNumber.ToString() : "NA";

                            ////Address For Communication
                            //int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                            //var address = (from c in context.Addresses
                            //               where c.AddressTypeID == CorAddTypeId && c.CandidateID == candidate1.ID
                            //               orderby c.EffectiveDateFrom descending
                            //               select c).Take(2).ToList();
                            //Int32 count = 0;
                            //foreach (var currentaddress in address)
                            //{
                            //    if (count == 0) //current address
                            //    {
                                    LblAdd1.Text = string.IsNullOrEmpty(candidate1.CorAddressLine1 ) == false && !string.IsNullOrWhiteSpace(candidate1.CorAddressLine1) ? GetInitCap(candidate1.CorAddressLine1) : "NA";
                                    LblAdd2.Text = string.IsNullOrEmpty(candidate1.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(candidate1.CorAddressLine2) ? GetInitCap(candidate1.CorAddressLine2) : "NA";
                                    lblAdd3.Text = string.IsNullOrEmpty(candidate1.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(candidate1.CorAddressLine3) ? GetInitCap(candidate1.CorAddressLine3) : "NA";
                                    LblCity.Text = string.IsNullOrEmpty(candidate1.CorCityName) == false && !string.IsNullOrWhiteSpace(candidate1.CorCityName) ? GetInitCap(candidate1.CorCityName) : "NA";
                                    LblState.Text = candidate1.CorStateID!=null && candidate1.CorStateID != 0 ? GetInitCap(candidate1.CorState.Name) : "NA";
                                    if (candidate1.CorDistrictID.HasValue)
                                    { LblDistrict.Text = GetInitCap(candidate1.CorDistrict.Name); }
                                    else
                                    { LblDistrict.Text = "NA"; }
                                    LblPinCode.Text = candidate1.CorPinCode!=null ? candidate1.CorPinCode.ToString() : "NA";
                                //    count = count + 1;
                                //}


                                ImgBtnReset.Visible = true;

                            }
                        
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Sorry ! No such Record found who submitted Guardian Details or affidavit already uploaded.";
                            PnlCandidate.Visible = false;
                            ImgBtnReset.Visible = true;
                        }
                    };
                }
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
                Int32 courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                string regNo = txtRegNo.Text;
                showData(courseType, courseID, regNo);
                //lblCourse.Text = courseID.ToString();
                ImgBtnSearch.Visible = false;
                ImgBtnReset.Visible = true;
                txtRegNo.Enabled = false;
                ddlCourseName.Enabled = false;
                
                ddlCourseType.Enabled = false;
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
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseTypeID
                              select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )" };

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
            txtRegNo.Enabled = true;
            ddlCourseName.Enabled = true;
            ddlCourseType.Enabled = true;
            txtRegNo.Text = "";
            ddlCourseName.SelectedValue = "0";
            ddlCourseType.SelectedValue = "0";
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
            //LblFName.Text = "NA";
            // LblMName.Text = "NA";
            lblMobile.Text = "NA";
            LblGName.Text = "NA";
            LblState.Text = "NA";
            // LblSyncedOn.Text = "NA";
            lblRegNum.Text = "NA";
            lblcurrentLevel.Text = "NA";
            // LblLockedOn.Text = "NA";
            // LblVerifiedOn.Text = "NA";
            // lblAffidavitNo.Text = "NA";
            //lblAffidavitDate.Text = "NA";

            ImgCandidatePhoto.ImageUrl = "";
            imgSignature.Src = "";
            imgThumb.Src = "";
            txtAffidavitDate.Text = "";
            txtAffidavitNo.Text = "";
            //txtRegNo.Text = "";
            //ddlCourseName.SelectedIndex = -1;
            //ddlCourseType.SelectedIndex = -1;
            //bindCourse();
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
  
   
   
    protected void cmdSaveAffidavit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (IsValidAffidavit(ddlCourseType.SelectedValue))
            {
                Int32 courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                string regNo = txtRegNo.Text;
                saveData(courseType, courseID, regNo);
                if (currentRoleId == 3)
                {
                    txtAffidavitDate.Enabled = false;
                    txtAffidavitNo.Enabled = false;
                    cmdSaveAffidavit.Enabled = false;
                    cmdSaveAffidavit.Visible = false;
                    fileAffidavit.Visible = false;
                    lblAffidavitFile.Visible = false;
                }
                else
                {
                    Resetcontrols();
                    PnlCandidate.Visible = false;
                }
                ImgBtnSearch.Visible = false;
                ImgBtnReset.Visible = true;
                txtRegNo.Enabled = false;
                ddlCourseName.Enabled = false;
                //Added 8 May 2019
                //PnlCandidate.Visible = false;
                 if (currentRoleId != 3)
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration No. " + regNo.ToString(), "#", ""));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidAffidavit(string CatgName)
    {
        try
        {
            lblError.Visible = true;

            if (String.IsNullOrWhiteSpace(txtAffidavitNo.Text))
            {

                lblError.Text = ("Please enter Affidavit No.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtAffidavitDate.Text))
            {

                lblError.Text = ("Please enter Affidavit Date.");
                return false;
            }

            DateTime affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
            
           if(affidavitDate > System .DateTime .Today  )
           {
                lblError.Text = (" Affidavit Date cannot be more than today's date");
                return false;
                }
            //Added 18 june 2019
           if (affidavitDate < System.DateTime.Today.AddYears(-1))
           {
               lblError.Text = (" Affidavit Date should not be older than 1 year");
               return false;
           }
            //

            if (fileAffidavit.FileName.ToString() == "")
            {

                lblError.Text = ("Affidavit File name can not be left blank");
                return false;

            }
            if (fileAffidavit.HasFile && fileAffidavit.FileName.Length > 50)
            {

                lblError.Text = ("Affidavit file name should be less than 50 characters.");
               return false;
            }
            
            String fileExtension = System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
            if (fileExtension != ".pdf")
            {
               lblError .Text=("Invalid Affidavit file .Only pdf extensions are allowed.");
               return false;

            }
            if (!isvalidFileSize(fileAffidavit, 102400))
            {
                lblError.Text = ("Affidavit File size should be of 100 KB or less.");
               return false;

            }
            lblError.Text = "";
            lblError.Visible = false;
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void saveData(Int32 courseTypeID, Int32 courseID, string regNo)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (courseTypeID == 1)
                {
                    var candidate = (from a in context.CourseRegistrationApplications
                                     join g in context.RegistrationDetails on a.CandidateID equals g.CandidateID
                                     where (
                                     a.FinalSubmitted == true && a.CourseID == courseID && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                    && a.GuardianName != null
                                    && a.GuardianName.Trim().Length != 0
                                    && a.affidavitDate == null
                                    && a.affidavitNo == null
									&& g.CourseRegistrationApplicationID ==a.ID 
                                    )
                                     select a).FirstOrDefault();
                    if (candidate == null)
                    {
                        //Added for old cases updates
                        //Added for old cases updates
                        var candidate1 = (from a in context.Candidates
                                          join g in context.RegistrationDetails on a.ID equals g.CandidateID
                                          join p in context.CourseRegistrationApplications on a.ID equals p.CandidateID
                                          where (
                                          p.FinalSubmitted == true &&
                                         a.GuardianName != null
                                         && a.GuardianName.Trim().Length != 0
                                         && p.affidavitDate == null
                                         && p.affidavitNo == null
                                         && g.CandidateID == p.CandidateID
                                         && g.CourseID == p.CourseID
                                       && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                         )
                                          select p).FirstOrDefault();

                        if (candidate1 != null)
                        {
                            CourseRegistrationApplication application = context.CourseRegistrationApplications.Find(candidate1.ID);
                            if (application != null)
                            {
                                application.affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
                                application.affidavitNo = txtAffidavitNo.Text;
                                if (fileAffidavit.FileName.Length > 50)
                                {
                                    application.affidavitFile = fileAffidavit.FileName.Substring(0, 45) + System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
                                }
                                else
                                    application.affidavitFile = fileAffidavit.FileName;

                                application.affidavitUpload = fileAffidavit.FileBytes;
                                application.affidavitVerified = false;

                                application.affidavitUploadedOn = DateTime.Now;
                                application.affidavitUploadedBy = Convert.ToInt32(Session["UserID"]);

                                context.SaveChanges();
                                if (currentRoleId == 3)
                                    ShowAlert("Affidavit details updated:, Please send hardcopy of affidavit to NIELIT HQ Regn section");
                                else
                                    ShowAlert("Affidavit details updated:");
                                if (currentRoleId != 3)
                                    Resetcontrols();
                            }

                        }
                    }
                    else
                    {
                        CourseRegistrationApplication application = context.CourseRegistrationApplications.Find(candidate.ID);
                        if (application != null)
                        {
                            application.affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
                            application.affidavitNo = txtAffidavitNo.Text;
                            if (fileAffidavit.FileName.Length > 50)
                            {
                                application.affidavitFile = fileAffidavit.FileName.Substring(0, 45) + System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
                            }
                            else
                                application.affidavitFile = fileAffidavit.FileName;

                            application.affidavitUpload = fileAffidavit.FileBytes;
                            application.affidavitVerified = false;

                            application.affidavitUploadedOn = DateTime.Now;
                            application.affidavitUploadedBy = Convert.ToInt32(Session["UserID"]);

                            context.SaveChanges();
                            if (currentRoleId == 3)
                                ShowAlert("Affidavit details updated:, Please send hardcopy of affidavit to NIELIT HQ Regn section");
                            else
                                ShowAlert("Affidavit details updated:");
                            if (currentRoleId != 3)
                                Resetcontrols();
                        }
                }
		}

                if (courseTypeID == 2)
                {
                    var candidate1 = (from a in context.CertificateExamApplications

                                      where (
                                      a.FinalSubmitted == true && a.CourseID == courseID && a.RollNumber.Trim() == regNo.Trim()
                                     && a.GuardianName != null
                                     && a.GuardianName.Trim().Length != 0)
                                      select a).FirstOrDefault();
                    if (candidate1 != null)
                    {
                        CertificateExamApplication application = context.CertificateExamApplications.Find(candidate1.ID);
                        if (application != null)
                        {
                          //  application.affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
                          //  application.affidavitNo = txtAffidavitNo.Text;
                            if (fileAffidavit.FileName.Length > 50)
                            {
                          //      application.affidavitFile = fileAffidavit.FileName.Substring(0, 45) + System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
                            }
                            else
                           //     application.affidavitFile = fileAffidavit.FileName;

                          //  application.affidavitUpload = fileAffidavit.FileBytes;
                         //   application.affidavitVerified = false;
                          //  application.affidavitUploadedOn = DateTime.Now;
                          //  application.affidavitUploadedBy = Convert.ToInt32(Session["UserID"]);

                            context.SaveChanges();
                            ShowAlert("Affidavit details updated:");
                            Resetcontrols();

                        }


                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Data not updated, Contact Administrator");
        }
    }
}