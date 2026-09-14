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

public partial class HO_AffidavitVerification : BasePage
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Affidavit Verification Process (Old Cases)", "#", ""));
                // bindCourse();
                BindCourseType();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])))
                {
                    Int32 courseType = Convert.ToInt32(Request.QueryString["CourseTypeID"]);
                    Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    string regNo = Convert.ToString(Request.QueryString["RegNo"]);
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration No. " + regNo.ToString(), "#", ""));
                    showData(courseType, courseID, regNo);
                    //showData(courseID, regNo);
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

    protected void BindCourseType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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
    protected void ddlCourseType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == courseType
                              select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
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


    protected Boolean isvalidForm()
    {
        try
        {

            if (ddlCourseType.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select course Type.";
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
                lblError.Text = "Please enter Registration Number";
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
                 && a.ID == g.CourseRegistrationApplicationID
                )
                                     select a).FirstOrDefault();
                    if (candidate != null)
                    {
                        if (candidate.affidavitUpload == null)
                        {
                            ShowAlert("Affidavit has not been uploaded");
                            return;
                        }

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
                            lblAffidavitVerifiedDate.Text = "";
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

                        //contact details
                        var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.ID).FirstOrDefault();
                        LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                                       contact.EmailAddress.ToString() : "NA";
                        lblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() : "NA";
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
                                         && p.affidavitDate != null
                                         && p.affidavitNo != null
                                         && g.CandidateID == p.CandidateID
                                         && g.CourseID == p.CourseID
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

                        var candidate2 = (from a in context.CourseRegistrationApplications
                                          join g in context.RegistrationDetails on a.CandidateID equals g.CandidateID
                                          where (
                                          a.FinalSubmitted == true && a.CourseID == courseID
                                          && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                         && a.affidavitDate != null
                                         && a.affidavitNo != null
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
                        // lblcurrentLevel.Text = context.RegistrationDetails.Where(s => s.RegistrationNo == regNo).OrderByDescending(s => s.RegistrationDate).Select(t => t.Course.Name).FirstOrDefault();
                        LblAppName.Text = candidate2.Salutation + " " + GetInitCap(candidate2.Name);

                        if (string.IsNullOrEmpty(candidate1.FatherName) == false && !string.IsNullOrWhiteSpace(candidate1.FatherName))
                            LblFName.Text = "Mr. " + GetInitCap(candidate1.FatherName);
                        else
                            LblFName.Text = "NA";
                        if (string.IsNullOrEmpty(candidate1.MotherName) == false && string.IsNullOrWhiteSpace(candidate1.MotherName) == false)
                            LblMName.Text = "Mrs. " + GetInitCap(candidate1.MotherName);
                        else
                            LblMName.Text = "NA";

                        LblDob.Text = string.IsNullOrEmpty(candidate2.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                                      !string.IsNullOrWhiteSpace(candidate2.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate2.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                        //    if (((string.IsNullOrEmpty(candidate.FatherName)) || (string.IsNullOrWhiteSpace(candidate.FatherName))) && ((string.IsNullOrEmpty(candidate.MotherName)) || (string.IsNullOrWhiteSpace(candidate.MotherName))))
                        //  {
                        trGuardian.Visible = true;
                        if ((!string.IsNullOrEmpty(candidate1.GuardianName)) && (!string.IsNullOrWhiteSpace(candidate1.GuardianName)))
                            LblGName.Text = GetInitCap(candidate1.GuardianName);
                        else
                            LblGName.Text = "NA";

                        trParent.Visible = false;
                        trAffidavit.Visible = true;
                        trShowAffidavit.Visible = true;
                        lblAffidavitDate.Text = candidate2.affidavitDate.Value.ToString("dd-MMM-yyyy");
                        lblAffidavitNo.Text = candidate2.affidavitNo;
                        if (candidate2.affidavitVerifiedOn != null)
                            lblAffidavitVerifiedDate.Text = candidate2.affidavitVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        if (candidate2.affidavitVerified)
                            lnkAffidavitVerified.Visible = false;
                        else
                        {
                            lnkAffidavitVerified.Visible = true;
                            lblAffidavitVerifiedDate.Text = "";
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

                        //contact details
                        var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate1.ID).FirstOrDefault();
                        LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                                       contact.EmailAddress.ToString() : "NA";
                        lblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() : "NA";
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
                if (ddlCourseType.SelectedValue.ToString() == "2")
                {
                    Label4.Text = "Roll Number:";
                    var candidate1 = (from a in context.CertificateExamApplications

                                      where (
                                      a.FinalSubmitted == true && a.CourseID == courseID && a.RollNumber.Trim() == regNo.Trim()
                                     && a.GuardianName != null
                                     && a.GuardianName.Trim().Length != 0)
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
                        ////                           lblAffidavitDate.Text = candidate1.affidavitDate.Value.ToString("dd-MMM-yyyy");
                        //            //            lblAffidavitNo.Text = candidate1.affidavitNo;
                        //           //             if (candidate1.affidavitVerifiedOn != null)
                        //                 //           lblAffidavitVerifiedDate.Text = candidate1.affidavitVerifiedOn.Value.ToString("dd-MMM-yyyy");
                        ////
                        //                   //     if (candidate1.affidavitVerified)
                        //                            lnkAffidavitVerified.Visible = false;
                        //                        else
                        //                        {
                        //                            lnkAffidavitVerified.Visible = true;
                        //                            lblAffidavitVerifiedDate.Text = "";
                        //                        }



                        //contact details
                        // var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate1.ID).FirstOrDefault();
                        LblEmail.Text = string.IsNullOrEmpty(candidate1.EmailAddress) == false && !string.IsNullOrWhiteSpace(candidate1.EmailAddress) ?
                                       candidate1.EmailAddress.ToString() : "NA";
                        lblMobile.Text = candidate1.MobileNumber != null && candidate1.MobileNumber != 0 ? candidate1.MobileNumber.ToString() : "NA";
                        LblPhone.Text = candidate1.PhoneNumber.HasValue && candidate1.PhoneNumber != 0 ? "0" + candidate1.StdNumber.ToString() + "-" + candidate1.PhoneNumber.ToString() : "NA";


                        LblAdd1.Text = string.IsNullOrEmpty(candidate1.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(candidate1.CorAddressLine1) ? GetInitCap(candidate1.CorAddressLine1) : "NA";
                        LblAdd2.Text = string.IsNullOrEmpty(candidate1.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(candidate1.CorAddressLine2) ? GetInitCap(candidate1.CorAddressLine2) : "NA";
                        lblAdd3.Text = string.IsNullOrEmpty(candidate1.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(candidate1.CorAddressLine3) ? GetInitCap(candidate1.CorAddressLine3) : "NA";
                        LblCity.Text = string.IsNullOrEmpty(candidate1.CorCityName) == false && !string.IsNullOrWhiteSpace(candidate1.CorCityName) ? GetInitCap(candidate1.CorCityName) : "NA";
                        LblState.Text = candidate1.CorStateID != null && candidate1.CorStateID != 0 ? GetInitCap(candidate1.CorState.Name) : "NA";
                        if (candidate1.CorDistrictID.HasValue)
                        { LblDistrict.Text = GetInitCap(candidate1.CorDistrict.Name); }
                        else
                        { LblDistrict.Text = "NA"; }
                        LblPinCode.Text = candidate1.CorPinCode != null ? candidate1.CorPinCode.ToString() : "NA";


                        ImgBtnReset.Visible = true;

                    }

                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Sorry ! No such Record found who submitted Guardian Details.";
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
                Int32 courseType = Convert.ToInt32(Request.QueryString["CourseTypeID"]);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                string regNo = txtRegNo.Text;
                showData(courseType, courseID, regNo);
                ImgBtnSearch.Visible = false;
                ImgBtnReset.Visible = true;
                txtRegNo.Enabled = false;
                ddlCourseName.Enabled = false;
                ddlCourseType.Enabled = false;

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Label4.Text + regNo.ToString(), "#", ""));
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
            LblFName.Text = "NA";
            LblMName.Text = "NA";
            lblMobile.Text = "NA";
            LblGName.Text = "NA";
            LblState.Text = "NA";
            // LblSyncedOn.Text = "NA";
            lblRegNum.Text = "NA";
            lblcurrentLevel.Text = "NA";
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
                if (ddlCourseType.SelectedValue == "1")
                {
                    Int32 courseID = 0;
                    string regNo = "";
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])))
                    {
                        courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                        regNo = Request.QueryString["RegNo"];
                    }
                    else
                    {
                        Int32 courseType = Convert.ToInt32(Request.QueryString["CourseTypeID"]);
                        courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                        regNo = txtRegNo.Text;

                        var candidate = (from a in context.CourseRegistrationApplications
                                         join g in context.RegistrationDetails on a.ID equals g.CourseRegistrationApplicationID
                                         where
                                          a.CourseID == courseID && g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                         && a.affidavitDate != null && a.affidavitNo != null
                                         && a.affidavitUpload != null
                     && a.ID == g.CourseRegistrationApplicationID
                                         select a).FirstOrDefault();
                        if (candidate != null)
                        {
                            if (candidate.IsVerifiedByInstitute == false && candidate.ApplicantType.ToString().Trim() == "2")
                            {
                                ShowAlert("Candidate not verified by institute");
                                return;
                            }
                            candidate.affidavitVerified = true;
                            candidate.affidavitVerifiedOn = DateTime.Now;
                            candidate.affidavitVerifiedBy = Convert.ToInt32(Session["UserID"]);
                            candidate.GuardianFlagStatusID = 3;
                            context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            string msg = "Candiate's affidavit details have been verified successfully.";
                            ShowAlert(msg, true);
                            showData(courseType, courseID, regNo);
                        }
                        else
                        {
                            string msg = "Candiate's affidavit details have not been verified successfully.";
                            ShowAlert(msg, true);
                        }
                    }
                }


                if (ddlCourseType.SelectedValue == "2")
                {
                    Int32 courseID = 0;
                    string rollNo = "";
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseID"])) && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["RegNo"])))
                    {
                        courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                        rollNo = Request.QueryString["RegNo"];
                    }
                    else
                    {
                        Int32 courseType = Convert.ToInt32(Request.QueryString["CourseTypeID"]);
                        courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                        rollNo = txtRegNo.Text;

                        var candidate = (from a in context.CertificateExamApplications

                                         where (
                                         a.FinalSubmitted == true && a.CourseID == courseID && a.RollNumber.Trim() == rollNo.Trim()
                                        && a.GuardianName != null
                                        && a.GuardianName.Trim().Length != 0
                                             //   && a.affidavitDate != null && a.affidavitNo != null
                                             //   && a.affidavitUpload != null
                                        )
                                         select a).FirstOrDefault();



                        if (candidate != null)
                        {
                            if (candidate.IsVerifiedByInstitute == false && candidate.ApplicantType.ToString().Trim() == "2")
                            {
                                ShowAlert("Candidate not verified by institute");
                                return;
                            }
                            //candidate.affidavitVerified = true;
                            //candidate.affidavitVerifiedOn = DateTime.Now;
                            //candidate.affidavitVerifiedBy = Convert.ToInt32(Session["UserID"]);
                            context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            string msg = "Candiate's affidavit details have been verified successfully.";
                            ShowAlert(msg, true);
                            showData(courseType, courseID, rollNo);
                        }
                        else
                        {
                            string msg = "Candiate's affidavit details have not been verified successfully.";
                            ShowAlert(msg, true);
                        }
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
            Response.Redirect("AffidavitVerification.aspx?" + Request.QueryString.ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void lnkAffidavit_Click(object sender, EventArgs e)
    {
        lnkAffidavit.Attributes.Add("target", "blank");
        string regNo = "";


        regNo = txtRegNo.Text;

        using (EConnectContext context = new EConnectContext())
        {
            if (ddlCourseType.SelectedValue == "1")
            {
                var candidate = (from a in context.CourseRegistrationApplications
                                 join g in context.RegistrationDetails on a.CandidateID equals g.CandidateID
                                 where (
                                 g.RegistrationNo.ToString().Trim() == regNo.Trim()
                                     // && a.GuardianName != null
                                && a.affidavitDate != null && a.affidavitNo != null
                               && a.affidavitUpload != null
                                 && a.ID == g.CourseRegistrationApplicationID
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
                        Response.Redirect("showAffidavit.aspx?num=" + txtRegNo.Text + "&cat=1");
                    else
                    {
                        ShowAlert("Affidavit not uploaded");
                        return;
                    }
                }
            }

            if (ddlCourseType.SelectedValue == "2")
            {
                var candidate = (from a in context.CertificateExamApplications

                                 where (
                                 a.FinalSubmitted == true && a.RollNumber.Trim() == regNo.Trim()
                                && a.GuardianName != null
                                && a.GuardianName.Trim().Length != 0
                                     //&& a.affidavitDate != null && a.affidavitNo != null
                                     //&& a.affidavitUpload != null
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


                    //if (candidate.affidavitUpload != null)
                    //    Response.Redirect("showAffidavit.aspx?num=" + txtRegNo.Text + "&cat=2");
                    //else
                    //{
                    //    ShowAlert("Affidavit not uploaded");
                    //    return;
                    //}
                }

            }
        }
    }
}
