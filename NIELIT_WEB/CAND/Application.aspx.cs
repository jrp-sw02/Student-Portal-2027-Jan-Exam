using System;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;

public partial class ChangeApplication : BasePage
{
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                //Show all the filled Information as a Preview 
                if (!string.IsNullOrEmpty(Session["EntityID"].ToString()))
                {
                    ShowData();
                }
                else
                {
                    Response.Write("Invalid Request Paramaters");
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    //Show all the filled Information as a Preview
    protected void ShowData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Lblhead.Text = "[Registration Wing]" ;
                //lblHead2.Text = "Form for capturing details of Parent's Name & Communication Details";
                var addNielit = (from s in context.Organizations
                                 select new
                                 {
                                     address = s.Name + "<br/>" + s.AddressLine1 + "," + s.AddressLine2 + "," + s.CityName + "-" + s.PinCode
                                 }).FirstOrDefault();

                LblAddNielit.Text = addNielit.address;

                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();
                if (candidate.LockedOn.HasValue)
                {
                    lblDate.Text = candidate.LockedOn.Value.ToString("dd-MMM-yyyy");
                }
                else
                    lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //Applicant's Personal Details 
                if (candidate.IsLocked == false)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Your profile is not locked yet. Please lock your profile after completion of all uncompleted details before printing.');window.close();", true);
                    tblMain.Visible = false;
                    return;
                }
                LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
               

                LblDob.Text = string.IsNullOrEmpty(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) == false &&
                              !string.IsNullOrWhiteSpace(candidate.DateOfBirth.ToString("dd-MMM-yyyy")) ? candidate.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                if (string.IsNullOrEmpty(candidate.GuardianName) == true && string.IsNullOrWhiteSpace(candidate.GuardianName) == true)
                {
                    if (string.IsNullOrEmpty(candidate.FatherName) == false && !string.IsNullOrWhiteSpace(candidate.FatherName))
                        LblFName.Text = "Mr. " + GetInitCap(candidate.FatherName);
                    else
                        LblFName.Text = "NA";
                    if (string.IsNullOrEmpty(candidate.MotherName) == false && string.IsNullOrWhiteSpace(candidate.MotherName) == false)
                        LblMName.Text = "Mrs. " + GetInitCap(candidate.MotherName);
                    else
                        LblMName.Text = "NA";
                    trMother.Visible = true;
                    trFather.Visible = true;
                    trGuardian.Visible = false;
                }
                else
                {
                    trMother.Visible = false;
                    trFather.Visible = false;
                    trGuardian.Visible = true;
                    LblGName.Text = GetInitCap(candidate.GuardianName);

                }



                //Telephone Numbers(with STD Code) 
                DateTime ContactMaxEffectiveDate = (from c in context.Candidates
                                                    join cad in context.CandidateContactDetails on c.ID equals cad.CandidateID
                                                    where c.ID == candidate.ID
                                                    select cad.EffectiveFromDate).Max();
                var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.ID &&
                              a.EffectiveFromDate == ContactMaxEffectiveDate).FirstOrDefault();
                LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ?
                               contact.EmailAddress.ToString() : "NA";
                lblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString() : "NA";
                LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";

                //Address For Communication 
                int CorrespondenceAddressTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                var CorAdd = context.Addresses.Where(a => a.CandidateID == candidate.ID && a.AddressTypeID == CorrespondenceAddressTypeId).OrderByDescending(c=>c.EffectiveDateFrom).FirstOrDefault();
                LblAdd1.Text = string.IsNullOrEmpty(CorAdd.AddressLine1) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine1) ? GetInitCap(CorAdd.AddressLine1) : "NA";
                LblAdd2.Text = string.IsNullOrEmpty(CorAdd.AddressLine2) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine2) ? GetInitCap(CorAdd.AddressLine2) : "NA";
                lblAdd3.Text = string.IsNullOrEmpty(CorAdd.AddressLine3) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine3) ? GetInitCap(CorAdd.AddressLine3) : "NA";
                LblCity.Text = string.IsNullOrEmpty(CorAdd.CityName) == false && !string.IsNullOrWhiteSpace(CorAdd.CityName) ? GetInitCap(CorAdd.CityName) : "NA";
                LblState.Text = CorAdd.StateID.HasValue && CorAdd.StateID != 0 ? GetInitCap(CorAdd.State.Name) : "NA";
                if (CorAdd.DistrictID.HasValue)
                    LblDistrict.Text = GetInitCap(CorAdd.District.Name);
                else
                    LblDistrict.Text = "NA";
                LblPinCode.Text = CorAdd.PinCode.HasValue ? CorAdd.PinCode.Value.ToString() : "NA";

                //Declaration
                if (candidate.PhotoFileID.HasValue)
                    ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile);
                if (candidate.SignatureFileID.HasValue)
                {
                    imgSignature.Height = 50;
                    imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Signature.BlobFile);
                }
                if (candidate.LeftThumbImpressionFileID.HasValue)
                {
                    imgThumbImpression.Height = 70;
                    imgThumbImpression.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.LeftThumbImpression.BlobFile);
                }
                
                //Registration Details
                var reg = context.RegistrationDetails.Where(q => q.CandidateID == entityID).OrderByDescending(q => q.CommencementFromDate).FirstOrDefault();
                lblRegNum.Text = reg.RegistrationNo.ToString();
                lblcurrentLevel.Text = GetInitCap(reg.Course.Name);
                Page.Title = "Application_Form_of_" + reg.RegistrationNo.ToString();
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}