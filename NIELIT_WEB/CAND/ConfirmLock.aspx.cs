using System;
using System.Linq;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class CAND_ConfirmLock : BasePage
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
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Confirm lock Profile", "#", ""));
                BreadCrumb1.Render();       
                using (EConnectContext context = new EConnectContext())
                {
                    Candidate obj = context.Candidates.Where(c => c.ID == entityID).FirstOrDefault();
                    LblCandName.Text = GetInitCap(obj.Name);
                    LblAppName.Text = obj.Salutation + " " + GetInitCap(obj.Name);
                    if (string.IsNullOrEmpty(obj.GuardianName) == true && string.IsNullOrWhiteSpace(obj.GuardianName) == true)
                    {
                        TrFatherName.Visible = true;
                        TrMotherName.Visible = true;
                        TrGardianName.Visible = false;
                        if (string.IsNullOrEmpty(obj.FatherName) == false && !string.IsNullOrWhiteSpace(obj.FatherName))
                            LblFatherName.Text = "Mr. " + GetInitCap(obj.FatherName);
                        else
                            LblFatherName.Text = "NA";
                        if (string.IsNullOrEmpty(obj.MotherName) == false && string.IsNullOrWhiteSpace(obj.MotherName) == false)
                            LblMotherName.Text = "Mrs. " + GetInitCap(obj.MotherName);
                        else
                            LblMotherName.Text = "NA";
                    }
                    else
                    {
                        LblGuardianName.Text = string.IsNullOrEmpty(obj.GuardianName) == false && string.IsNullOrWhiteSpace(obj.GuardianName) == false ? GetInitCap(obj.GuardianName) : "NA";
                        TrFatherName.Visible = false;
                        TrMotherName.Visible = false;
                        TrGardianName.Visible = true;
                        TrName.Attributes.Add("class", "gdrow1");
                    }

                    LblGender.Text = string.IsNullOrEmpty(obj.Gender) == false && string.IsNullOrWhiteSpace(obj.Gender) == false ? GetInitCap(obj.Gender) : "NA";                   
                    LblDob.Text = string.IsNullOrEmpty(obj.DateOfBirth.ToString("dd-MMM-yyyy")) == false && !string.IsNullOrWhiteSpace(obj.DateOfBirth.ToString("dd-MMM-yyyy")) ? obj.DateOfBirth.ToString("dd-MMM-yyyy") : "NA";

                    //Contact Detail

                    DateTime ContactMaxEffectiveDate = (from c in context.Candidates
                                                        join cad in context.CandidateContactDetails on c.ID equals cad.CandidateID
                                                        where c.ID == obj.ID
                                                        select cad.EffectiveFromDate).Max();
                    var contact = context.CandidateContactDetails.Where(a => a.CandidateID == obj.ID && a.EffectiveFromDate == ContactMaxEffectiveDate).FirstOrDefault();
                    LblMobile.Text = contact.MobileNumber.HasValue && contact.MobileNumber != 0 ? contact.MobileNumber.ToString(): "NA";
                    LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";
                    LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ? contact.EmailAddress.ToString(): "NA";
                    
                    //Corespondance Address
                    int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    DateTime effectiveDate = (from c in context.Candidates
                                              join cad in context.Addresses on c.ID equals cad.CandidateID
                                              where c.ID == obj.ID
                                              select cad.EffectiveDateFrom).Max();                   
                    var CorAdd = context.Addresses.Where(a => a.CandidateID == obj.ID && a.EffectiveDateFrom == effectiveDate && a.AddressTypeID == CorAddTypeId).FirstOrDefault();
                                      
                    LblCorAddressLine1.Text = string.IsNullOrEmpty(CorAdd.AddressLine1) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine1) ? GetInitCap(CorAdd.AddressLine1) : "NA";
                    LbCorlAddressLine2.Text = string.IsNullOrEmpty(CorAdd.AddressLine2) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine2) ? GetInitCap(CorAdd.AddressLine2) : "NA";
                    LblCorAddressLine3.Text = string.IsNullOrEmpty(CorAdd.AddressLine3) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine3) ? GetInitCap(CorAdd.AddressLine3) : "NA";
                    LblCityName.Text = string.IsNullOrEmpty(CorAdd.CityName) == false && !string.IsNullOrWhiteSpace(CorAdd.CityName) ? GetInitCap(CorAdd.CityName) : "NA";
                    LblState.Text = CorAdd.StateID.HasValue && CorAdd.StateID != 0 ? CorAdd.State.Name : "NA";
                    if (CorAdd.DistrictID.HasValue)
                        LblDistrict.Text = GetInitCap(CorAdd.District.Name);
                    else
                        LblDistrict.Text = "NA";
                    LblPincode.Text = CorAdd.PinCode.HasValue ? CorAdd.PinCode.Value.ToString() : "NA";
                   
                };
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnLock_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Save Candidate  Detail.
                Candidate obj = context.Candidates.Where(c => c.ID == entityID).FirstOrDefault();
                obj.IsLocked = true;
                obj.LockedOn = DateTime.Now;
                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                //ShowAlert("Your Personal Detail has been Locked.Now you cannot change in future.");
                Response.Redirect("../FrmDashBoard.aspx");
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void LnkBtnNo_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/CAND/myprofile.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
}