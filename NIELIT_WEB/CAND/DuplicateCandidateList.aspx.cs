using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;

public partial class CAND_DuplicateCandidateList : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["candidateID"]))
                {
                    showdata();
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
                    Response.End();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 candidateID = Convert.ToInt64(Request.QueryString["candidateID"]);
                var candidate = (from a in context.Candidates
                                 where a.ID == candidateID
                                 select a).FirstOrDefault();

                LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                if (string.IsNullOrEmpty(candidate.GuardianName) == true && string.IsNullOrWhiteSpace(candidate.GuardianName) == true)
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    if (string.IsNullOrEmpty(candidate.FatherName) == false && !string.IsNullOrWhiteSpace(candidate.FatherName))
                        LblFatherName.Text = "Mr. " + GetInitCap(candidate.FatherName);
                    else
                        LblFatherName.Text = "NA";
                    if (string.IsNullOrEmpty(candidate.MotherName) == false && string.IsNullOrWhiteSpace(candidate.MotherName) == false)
                        LblMotherName.Text = "Mrs. " + GetInitCap(candidate.MotherName);
                    else
                        LblMotherName.Text = "NA";
                }
                else
                {
                    LblGuardianName.Text = string.IsNullOrEmpty(candidate.GuardianName) == false && string.IsNullOrWhiteSpace(candidate.GuardianName) == false ? GetInitCap(candidate.GuardianName) : "NA";
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    TrName.Attributes.Add("class", "gdrow1");
                }

                LblGender.Text = string.IsNullOrEmpty(candidate.Gender) == false && string.IsNullOrWhiteSpace(candidate.Gender) == false ? GetInitCap(candidate.Gender) : "NA";
                LblMaritalStatus.Text = string.IsNullOrEmpty(candidate.MaritalStatus.Name) == false && !string.IsNullOrWhiteSpace(candidate.MaritalStatus.Name) ? GetInitCap(candidate.MaritalStatus.Name) : "NA";
                LblDob.Text = string.IsNullOrEmpty(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) == false && !string.IsNullOrWhiteSpace(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";
                LblCategory.Text = string.IsNullOrEmpty(candidate.CastCategory.Name) == false && !string.IsNullOrWhiteSpace(candidate.CastCategory.Name) ? GetInitCap(candidate.CastCategory.Name) : "NA";

                if (candidate.Photo != null)
                    ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile);
                LblIsHandicaped.Text = candidate.IsHandicaped == false ? "No" : "Yes";
                LblIsExServicemane.Text = candidate.IsExServicemane == false ? "No" : "Yes";
                LblOccuption.Text = candidate.OccupationID.HasValue && candidate.OccupationID != 0 ? GetInitCap(candidate.Occupation.Name) : "NA";
                int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);

                var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.ID).FirstOrDefault();
                
                if (contact != null)
                {
                    LblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() : "NA";
                    LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";
                    LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ? contact.EmailAddress.ToString() : "NA";
                }
                var CorAdd = (from cad in context.Addresses
                              where cad.CandidateID == candidateID && cad.AddressTypeID == CorAddTypeId
                              orderby cad.EffectiveDateFrom descending
                              select cad).FirstOrDefault();

                if (CorAdd != null)
                {

                    LblCorAddressLine1.Text = string.IsNullOrEmpty(CorAdd.AddressLine1) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine1) ? GetInitCap(CorAdd.AddressLine1) : "";
                    LbCorlAddressLine2.Text = string.IsNullOrEmpty(CorAdd.AddressLine2) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine2) ? GetInitCap(CorAdd.AddressLine2) : "NA";
                    LblCorAddressLine3.Text = string.IsNullOrEmpty(CorAdd.AddressLine3) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine3) ? GetInitCap(CorAdd.AddressLine3) : "NA";
                    LblCityName.Text = string.IsNullOrEmpty(CorAdd.CityName) == false && !string.IsNullOrWhiteSpace(CorAdd.CityName) ? GetInitCap(CorAdd.CityName) : "";
                    LblState.Text = CorAdd.StateID.HasValue && CorAdd.StateID != 0 ? CorAdd.State.Name : "";
                    if (CorAdd.DistrictID.HasValue)
                        LblDistrict.Text = GetInitCap(CorAdd.District.Name);
                    else
                        LblDistrict.Text = "";
                    LblPincode.Text = CorAdd.PinCode.HasValue ? CorAdd.PinCode.Value.ToString() : "";
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
                    LblPerDistrict.Text = GetInitCap(PerAdd.District.Name);
                else
                    LblPerDistrict.Text = "NA";
                LblPerPinCode.Text = PerAdd.PinCode.HasValue ? PerAdd.PinCode.Value.ToString() : "NA";


                DateTime EduMaxEffectiveDate = (from c in context.Candidates
                                                join cad in context.CandidateQualificationDetails on c.ID equals cad.CandidateID
                                                where c.ID == candidate.ID
                                                select cad.EffectiveFromDate).Max();

                var Qualification = context.CandidateQualificationDetails.Where(a => a.CandidateID == candidate.ID && a.EffectiveFromDate == EduMaxEffectiveDate).FirstOrDefault();
                LblHeighestEducation.Text = Qualification.EducationalQualificationID != null && Qualification.EducationalQualificationID != 0 ? GetInitCap(Qualification.EducationalQualification.Name) : "NA";
                LblYrOfPassing.Text = string.IsNullOrEmpty(Qualification.PassingYear.ToString()) == false ? Qualification.PassingYear.ToString() : "NA";



                var reg = (from r in context.RegistrationDetails
                          where r.CandidateID == candidateID
                          orderby r.CommencementFromDate descending
                          select new 
                          {
                             RegNo = r.RegistrationStatusID.Value == 4 ? r.ReRegistrationNumber.Value : r.RegistrationNo,
                             ValidUpto = r.ValidUptoDate,
                             RegDate = r.RegistrationStatusID.Value == 4 ? r.ReRegistrationDate : r.RegistrationDate,
                             course =  r.Course.Name,
                             CandidateType = r.ApplicantTypeID == 1 ? "Direct" : "Institute",
                             Status = r.RegistrationStatusID.HasValue ? r.RegistrationStatus.Name : ""
                          }).ToList();
                
                    if (reg != null)
                    {
                        GridView1.DataSource = reg;
                        GridView1.DataBind();
                    }
            }
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
            Int64 candidateID = Convert.ToInt64(Request.QueryString["candidateID"]);
            using (EConnectContext context = new EConnectContext())
            {

                var candidate = (from a in context.Candidates
                                 where a.ID == candidateID
                                 select a).FirstOrDefault();
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
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile);
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
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}