using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;

public partial class CAND_ScholarshipPreview : BasePage 
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
                var addNielit = (from s in context.Organizations
                                 select new
                                 {
                                     address = s.Name + "<br/>" + s.AddressLine1 + "," + s.AddressLine2 + "," + s.CityName + "-" + s.PinCode
                                 }).FirstOrDefault();

                LblAddNielit.Text = addNielit.address;

                Int64 ScholarID = Convert.ToInt64(Request.QueryString["Appid"]);
                var scholarship = (from s in context.CandidateScholarships
                                   where s.ID == ScholarID
                                   select new
                                   {
                                       aacno = s.AccountNumber,
                                       bname = s.BankName,
                                       baddresss = s.BankAddress,
                                       ifscocde = s.IFSCCode,
                                       aadharcard = s.AadharNumber.HasValue ? s.AadharNumber : 0,
                                       aadharfileid = s.AaadharFileID.HasValue ? s.AaadharFileID : 0,
                                       icomeid = s.IncomeProofFileID.HasValue ? s.IncomeProofFileID : 0,
                                       phid = s.PhysicalHandicapFileID.HasValue ? s.PhysicalHandicapFileID : 0,
                                       cid = s.CasteCertificateFileID.HasValue ? s.CasteCertificateFileID : 0,
                                       bid = s.BankAccountFileID.HasValue ? s.BankAccountFileID : 0,
                                       isphyhandicap = s.IsHandicaped,
                                       anualincome = s.IncomeCategory.Name,
                                       categoryid = s.CasteCategory,
                                       date = s.Date,
                                       acctype =s.TypeOfAccount
                                   }).FirstOrDefault();
                if (scholarship != null)
                {

                    lblDate.Text = scholarship.date.ToString("dd-MMM-yyyy");
                    lblanincome.Text = scholarship.anualincome;
                    lblcastcategory.Text = context.CastCategories.Where(s => s.ID == scholarship.categoryid).FirstOrDefault().Name;
                    Lblbname.Text = scholarship.bname.ToString();
                    Lblbkaddress.Text = scholarship.baddresss.ToString();
                    Lblifsccode.Text = scholarship.ifscocde.ToString();
                    lblacctype.Text = scholarship.acctype.ToString();
                    Lblaccno.Text = scholarship.aacno.ToString();
                    if (scholarship.aadharcard != 0)
                        lbladhar.Text = scholarship.aadharcard.Value.ToString();
                    else
                        lbladhar.Text = "NA";

                    if (scholarship.isphyhandicap.ToString() == "True")
                        lblphcap.Text = "Yes";
                    else
                        lblphcap.Text = "No";

                    //doc details
                    if (scholarship.aadharfileid != 0)
                    {
                        HyperLink hlaadhar = new HyperLink();
                        hlaadhar.Text = "<img src='../images/rightMark2.jpg'></img> Please send Attested Copy of Aadhar Card.";
                        hlaadhar.Style.Add("text-decoration", "none");
                        hlaadhar.ForeColor = System.Drawing.Color.Black;
                        //hlaadhar.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.aadharfileid;
                        tddoc.Controls.Add(hlaadhar);
                    }
                    if (scholarship.icomeid != 0)
                    {
                        HyperLink hlincome = new HyperLink();
                        if(scholarship.aadharfileid != 0)
                           hlincome.Text = "<br/><img src='../images/rightMark2.jpg'></img> Please send Attested Copy of Income Certificate of Parents.";
                        else
                            hlincome.Text = "<img src='../images/rightMark2.jpg'></img> Please send Attested Copy of Income Certificate of Parents.";
                        hlincome.Style.Add("text-decoration", "none");
                        hlincome.ForeColor = System.Drawing.Color.Black;
                        //hlincome.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.icomeid;
                        tddoc.Controls.Add(hlincome);
                    }
                    if (scholarship.cid != 0)
                    {
                        HyperLink hlcaste = new HyperLink();
                        hlcaste.Text = "<br/><img src='../images/rightMark2.jpg'></img> Please send  Attested Copy of Cast Certificate.";
                        hlcaste.Style.Add("text-decoration", "none");
                        hlcaste.ForeColor = System.Drawing.Color.Black;
                        //hlcaste.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.cid;
                        tddoc.Controls.Add(hlcaste);
                    }

                    if (scholarship.bid != 0)
                    {
                        HyperLink hlbank = new HyperLink();
                        hlbank.Text = "<br/><img src='../images/rightMark2.jpg'></img> Please send Attested Copy of Bank Account Pass Book. ";
                        hlbank.Style.Add("text-decoration", "none");
                        hlbank.ForeColor = System.Drawing.Color.Black;
                        //hlbank.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.bid;
                        tddoc.Controls.Add(hlbank);
                    }

                    if (scholarship.phid != 0)
                    {
                        HyperLink hlhandicap = new HyperLink();
                        hlhandicap.Text = "<br/><img src='../images/rightMark2.jpg'></img> Please send Attested Copy of Physically Handicapped Certificate.";
                        hlhandicap.Style.Add("text-decoration", "none");
                        hlhandicap.ForeColor = System.Drawing.Color.Black;
                        //hlhandicap.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.phid;
                        tddoc.Controls.Add(hlhandicap);
                    }
                }

                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();
               
                LblAppName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                lblgender.Text = candidate.Gender;

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

                //Address For Communication 
                int CorrespondenceAddressTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                var CorAdd = context.Addresses.Where(a => a.CandidateID == candidate.ID && a.AddressTypeID == CorrespondenceAddressTypeId).OrderByDescending(c => c.EffectiveDateFrom).FirstOrDefault();
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
                Page.Title = "Scholarship_Form_of_" + reg.RegistrationNo.ToString();
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}