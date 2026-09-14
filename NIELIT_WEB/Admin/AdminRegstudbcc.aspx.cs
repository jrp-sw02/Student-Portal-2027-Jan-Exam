using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;

public partial class Admin_AdminRegstudbcc : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/SearchBCCCCCandidate.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                Session["level"] = null;
                if (!String.IsNullOrEmpty(Request.QueryString["ApplID"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                }   
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowEditMode()
    {

        try
        {
            showsidelink();
            context = new EConnectContext();
            Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
            //Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            //Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            //var cr = (from s in context.CertificateExamApplications
            //          where s.ID == ApplicationID && s.CourseID == CourseId
            //          orderby s.ApplicationDate descending
            //          select s).FirstOrDefault();
            var cr = context.CertificateExamApplications.Find(ApplicationID);
            
            if (cr != null)
            {
                LblAppName.Text = cr.Salutation + " " + GetInitCap(cr.Name);
                if (string.IsNullOrEmpty(cr.GuardianName) == true && string.IsNullOrWhiteSpace(cr.GuardianName) == true)
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    if (string.IsNullOrEmpty(cr.FatherName) == false && !string.IsNullOrWhiteSpace(cr.FatherName))
                        LblFatherName.Text = "Mr. " + GetInitCap(cr.FatherName);
                    else
                        LblFatherName.Text = "NA";
                    if (string.IsNullOrEmpty(cr.MotherName) == false && string.IsNullOrWhiteSpace(cr.MotherName) == false)
                        LblMotherName.Text = "Mrs. " + GetInitCap(cr.MotherName);
                    else
                        LblMotherName.Text = "NA";
                }
                else
                {
                    LblGuardianName.Text = string.IsNullOrEmpty(cr.GuardianName) == false && string.IsNullOrWhiteSpace(cr.GuardianName) == false ? GetInitCap(cr.GuardianName) : "NA";
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    TrName.Attributes.Add("class", "gdalternate1");
                }
                LblGender.Text = string.IsNullOrEmpty(cr.Gender) == false && !string.IsNullOrWhiteSpace(cr.Gender) ? GetInitCap(cr.Gender) : "NA";
                LblDob.Text = cr.DateOfBirth.ToString("dd-MMM-yyyy");
                LblCategory.Text = (cr.CastCategoryID!=0) ? GetInitCap(cr.CastCategory.Name) : "-";
                LblOccuption.Text = cr.OccupationID != 0 ? GetInitCap(cr.Occupation.Name) : "NA";

                if (cr.Photo != null && cr.Photo[0] != 1 && cr.Photo[1] != 35)
                {
                    ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Photo);
                   
                    String photoName =  cr.Number.ToString();
                    FileStream fs = File.Create(Server.MapPath("~/TempPhoto/" + photoName + ".jpg"));
                   
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(cr.Photo);
                    bw.Close();
                    fs.Close();
                    CandPhotoApp.HRef = "~/TempPhoto/" + photoName + ".jpg";
                    if (cr.RollNumber != null)
                    {
                        String photoName1 = cr.RollNumber.ToString();
                        FileStream fs1 = File.Create(Server.MapPath("~/TempPhoto/" + photoName1 + ".jpg"));
                        BinaryWriter bw1 = new BinaryWriter(fs1);
                        bw1.Write(cr.Photo);
                        bw1.Close();
                        fs1.Close();
                        CandPhotoRoll.HRef = "~/TempPhoto/" + photoName1 + ".jpg";
                    }
                }

                LblMobile.Text = cr.MobileNumber!=0 ? cr.MobileNumber.ToString() : "NA";
                LblPhone.Text = cr.PhoneNumber.HasValue && cr.PhoneNumber != 0 && cr.StdNumber.HasValue ? "0" + cr.StdNumber.Value.ToString() + "-" + cr.PhoneNumber.Value.ToString() : "NA";
                LblEmail.Text = string.IsNullOrEmpty(cr.EmailAddress) == false && !string.IsNullOrWhiteSpace(cr.EmailAddress) ? cr.EmailAddress.ToString() : "NA";

                //Address Details
                
                LblPerAddressLine1.Text = string.IsNullOrEmpty(cr.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(cr.CorAddressLine1) ? GetInitCap(cr.CorAddressLine1) : "NA";
                LblPerAddressLine2.Text = string.IsNullOrEmpty(cr.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(cr.CorAddressLine2) ? GetInitCap(cr.CorAddressLine2) : "NA";
                LblPerAddressLine3.Text = string.IsNullOrEmpty(cr.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(cr.CorAddressLine3) ? GetInitCap(cr.CorAddressLine3) : "NA";
                LblPerCityName.Text = string.IsNullOrEmpty(cr.CorCityName) == false && !string.IsNullOrWhiteSpace(cr.CorCityName) ? GetInitCap(cr.CorCityName) : "NA";
                LblPerStateName.Text = cr.CorStateID != 0 && cr.CorStateID != 1 ? cr.CorState.Name : "NA";
                if (cr.CorDistrictID.HasValue)
                    LblPerDistrictName.Text = GetInitCap(cr.CorDistrict.Name);
                else
                    LblPerDistrictName.Text = "NA";
                LblPerPinCodeNumber.Text = cr.CorPinCode!= 0  ? cr.CorPinCode.ToString() : "NA";
                    
                //Qualification Details
                LblHeighestEducation.Text = string.IsNullOrEmpty(cr.EducationalQualification.Name) == false && !string.IsNullOrWhiteSpace(cr.EducationalQualification.Name) ? GetInitCap(cr.EducationalQualification.Name) : "NA";
                LblYrOfPassing.Text = cr.PassingYear.HasValue ? cr.PassingYear.Value.ToString() : "NA";

                
                
                LblRegNo.Text = cr.Number.ToString();
                LblRegDate.Text = cr.ApplicationDate.ToString("dd-MMM-yyyy");
                Lblresultgrade.Text = cr.ResultGradeID.HasValue ? cr.ResultGrade.Code + "(" + cr.ResultGrade.Description + ")" : " Result Not Declared";
                Lblresultdate.Text = cr.ResultUpdatedOn.HasValue ? cr.ResultUpdatedOn.GetValueOrDefault().ToString("dd-MMM-yyyy") : " Result Not Published";
                Lblregcentre.Text = cr.RegionalCenterID.HasValue ? cr.RegionalCenter.Name : "NA";
                LblCourse.Text = GetInitCap(cr.Course.Name);
                Lblexamname.Text = cr.ExamID != 0 ? GetInitCap(cr.Exam.Name) : "NA";
                lblcoursecategory.Text = GetInitCap(cr.CourseCategory.Name);
                if (cr.ApplicantTypeID != 0)
                {
                    LblCandidateType.Text = cr.ApplicantType.Name.ToString();
                    if (cr.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        TrAccCentre.Visible = true;
                        if (cr.InstituteID.HasValue)
                            LblAccCentre.Text = GetInitCap(cr.Institute.Name) + "(" + cr.Institute.AccreditationDetails.Where(c => c.CourseID == cr.CourseID).FirstOrDefault().AccreditationNumber.ToString() + ")";
                        else
                            LblAccCentre.Text = "NA";
                    }
                    else
                    {
                        TrAccCentre.Visible = false;
                    }
                }
                else
                {
                    LblCandidateType.Text = "-";
                    TrAccCentre.Visible = false;
                }
            }
            if (Request.UrlReferrer.ToString().ToLower().Contains("changebccccccandidatephoto.aspx"))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.RemoveLastBreadCrumbItem();
            }
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Salutation + cr.Name.ToUpper(), "Admin/AdminRegstudbcc.aspx?" + Request.QueryString.ToString(), ""));
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            pnlFilter.Visible = false;
            lblHeading.Text = "Candidate Details";
            //Get last modified date of current record and save it in ViewState object.
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit, "Common/SearchBCCCCCandidate.aspx"))
            {
                LnkBtnPhotoDetail.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            pnlFilter.Visible = false;
            lblHeading.Text = "Certificate Students";
        }
        else
        {
            BreadCrumb1.Render();
            if (!string.IsNullOrEmpty(Request.QueryString["Src"]))
            {
                Response.Redirect("../Common/SearchBCCCCCandidate.aspx?regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"]);
            }
            else
            {
                Response.Redirect("AdminRegstudbcc.aspx?CourseID=" + Request.QueryString["courseID"]);
            }
        }
    }
    protected void LnkBtnPhotoDetail_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            using (EConnectContext context = new EConnectContext())
            {
                //var certficate = (from r in context.CertificateExamApplications
                //                 where r.ID == ApplicationID && r.CourseID == CourseId
                //                 select r).FirstOrDefault();
                var certficate = context.CertificateExamApplications.Find(ApplicationID);

                if (certficate != null)
                {
                    Response.Redirect("ChangeBCCCCCCandidatePhoto.aspx?ApplID=" + certficate.ID + "&CourseId=" + Request.QueryString["CourseId"] + "&regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"] + "&couID=" + Request.QueryString["couID"]);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ToggleImage(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            string arg = btn.CommandArgument.ToLower();
            Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
            //Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            //Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            using (EConnectContext context = new EConnectContext())
            {

                //var reg = (from s in context.CertificateExamApplications
                //           where s.ID == ApplicationID && s.CourseID == CourseId
                //           select s).FirstOrDefault();
                var reg = context.CertificateExamApplications.Find(ApplicationID);

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
                    if (reg.Photo != null && reg.Photo[0] != 1 && reg.Photo[1] != 35)
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])reg.Photo);
                }
                if (LblPhotoCaption.Text.ToLower() == "signature")
                {
                    ImgBtnPrevious.ToolTip = "Click to view photograph";
                    ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                    if (reg.Signature != null && reg.Signature[0] != 1 && reg.Signature[1] != 35)
                    {
                        ImgCandidatePhoto.Height = 50;
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])reg.Signature);
                    }
                }
                else if (LblPhotoCaption.Text.ToLower() == "thumb")
                {
                    ImgBtnPrevious.ToolTip = "Click to view signature";
                    ImgBtnNext.ToolTip = "";
                    if (reg.LeftThumb != null && reg.LeftThumb[0] != 1 && reg.LeftThumb[1] != 35)
                    {
                        ImgCandidatePhoto.Height = 60;
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])reg.LeftThumb);
                    }
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        if (!string.IsNullOrEmpty(Request.QueryString["Src"]))
        {
            Response.Redirect("../Common/SearchBCCCCCandidate.aspx?regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"]);
        }
        else
        {
            Response.Redirect("AdminRegstudbcc.aspx?CourseID=" + Request.QueryString["courseID"]);
        }
    }
    protected void showsidelink()
    {

        SideLink1.Items.Add(new SideLinkItem("Personal Detail", "BCCCandidatePersonalDetails.aspx?key1=" + Request.QueryString["ApplID"], "", ""));
        SideLink1.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
        SideLink1.Render();
    }
}