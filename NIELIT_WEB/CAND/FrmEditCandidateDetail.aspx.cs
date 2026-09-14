using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI.WebControls;





public partial class CAND_FrmEditCandidateDetail : BasePage
{
    Int32 changeRequestTypeID = 0;
    UserType loginUserType;
    Int64 entityID = 0;


    // amit_apaar_api_changes_may_2026_start
    public int IsAgeGreaterthan18
    {
        get { return ViewState["IsAgeGreaterthan18"] == null ? 0 : (int)ViewState["IsAgeGreaterthan18"]; }
        set { ViewState["IsAgeGreaterthan18"] = value; }
    }

    public string candidate_name
    {
        get { return ViewState["CandidateName"] == null ? "" : ViewState["CandidateName"].ToString(); }
        set { ViewState["CandidateName"] = value; }
    }
    // amit_apaar_api_changes_may_2026_end
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            lblError.Visible = false; 
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["SrcType"])))
                {
                    changeRequestTypeID = Convert.ToInt32(Request.QueryString["SrcType"]);
                    DdlAllDetail.SelectedValue = changeRequestTypeID.ToString();
                    BtnAllContinue_Click(BtnAllContinue, EventArgs.Empty);
                    getBreadCrumb(false);
                }
            }
            else
                getBreadCrumb(true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GetDetail(int Src)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();
                MultiView1.ActiveViewIndex = Src;
                switch (Src)
                {
                    case 0:
                        mvAction.Visible = false;
                        LblReqType.Text = "Change Request";
                        break;
                    case 1:
                        showPersonalDetail(candidate);
                        HfRequestTypeID.Value = "1";
                        LblReqType.Text = "Personal ";
                        break;
                    case 2:
                        showContactDetail(candidate);
                        HfRequestTypeID.Value = "2";
                        LblReqType.Text = "Contact ";
                        if(candidate.IsLocked == true)
                        DivChangeConfirm.InnerHtml = "<table class='box' style='margin-top: 20px' cellpadding='3' cellspacing='0'><tr class='gdalternate1'><td>Your Profile has been Locked. If you change your contact detail,changes will not be reflected and call will be received on the above given number only." +
                         "If you still wish to continue. Please click on Continue to update else on Cancel to go back to profile.</td></tr></table>";        
                        break;
                    case 3:
                        showCorespondenceAddressDetail(candidate);
                        HfRequestTypeID.Value = "3";
                        LblReqType.Text = "correspondence Address ";
                        
                        break;
                   
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void getBreadCrumb(Boolean update)
    {
        try
        {
            string Title = "";
            int Src = Convert.ToInt32(MultiView1.ActiveViewIndex);
            switch (Src)
            {
                case 0:
                    Title = "Change Request Details";
                    break;
                case 1:
                    Title = "Change Personal Detail";
                    break;
                case 2:
                    Title = "Change Contact Detail";
                    break;
                case 3:
                    Title = "Change correspondence  Address Detail";
                    break;

            }
            if (update == true)
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem(Title, "CAND/FrmEditCandidateDetail.aspx?SrcType=" + MultiView1.ActiveViewIndex.ToString(), ""));
            else
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Title, "CAND/FrmEditCandidateDetail.aspx?SrcType=" + MultiView1.ActiveViewIndex.ToString(), ""));                        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void bindCorrState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var state = (from s in context.Locations
                             orderby (s.Name)
                             where s.LocationTypeID == 2 && s.ParentLocationID == 1
                             select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, state, lst);
            };    
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (stateID != null & stateID != 0)
                {
                    var district = from s in context.Locations
                                   orderby (s.Name)
                                   where s.LocationTypeID == 4 && s.ParentLocationID == stateID
                                   select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);
                }

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showPersonalDetail(Candidate candidateP)
    { 
        try
        {
            LblHeading.Text = "Change Personal Detail";
            using (EConnectContext context = new EConnectContext())
            {
                //// amit_apaar_api_changes_may_2026_start
                candidate_name = candidateP.Name; // for other functions
                // amit_apaar_api_changes_may_2026_start

                LblCandNameE.Text = candidateP.Salutation + "  " + GetInitCap(candidateP.Name);
                rdbtnlstgender.SelectedValue = candidateP.Gender.ToLower();
                LblAppName.Text = candidateP.Salutation +" "+ GetInitCap(candidateP.Name);
                if (string.IsNullOrEmpty(candidateP.GuardianName) == true && string.IsNullOrWhiteSpace(candidateP.GuardianName) == true)
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    //TrFatherName1.Visible = true;
                    //TrMotherName1.Visible = true;
                    //TrGardianName1.Visible = false;
                    if (string.IsNullOrEmpty(candidateP.FatherName) == false && !string.IsNullOrWhiteSpace(candidateP.FatherName))
                        LblFatherName.Text = "Mr. " + GetInitCap(candidateP.FatherName);
                    else
                        LblFatherName.Text = "NA";
                    if (string.IsNullOrEmpty(candidateP.MotherName) == false && string.IsNullOrWhiteSpace(candidateP.MotherName) == false)
                        LblMotherName.Text = "Mrs. " + GetInitCap(candidateP.MotherName);
                    else
                        LblMotherName.Text = "NA";
                    TxtFatherName.Text = candidateP.FatherName;
                    TxtMotherName.Text = candidateP.MotherName;
                }
                else
                {
                    LblGuardianName.Text = string.IsNullOrEmpty(candidateP.GuardianName) == false && string.IsNullOrWhiteSpace(candidateP.GuardianName) == false ? GetInitCap(candidateP.GuardianName) : "NA";
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    //TrFatherName1.Visible = false;
                    //TrMotherName1.Visible = false;
                    //TrGardianName1.Visible = true;
                    TxtGuardianName.Text = candidateP.GuardianName;
                    TrName.Attributes.Add("class", "gdrow1");
                    //TrName1.Attributes.Add("class", "gdrow1");
                }
                LblGender.Text = string.IsNullOrEmpty(candidateP.Gender) == false && !string.IsNullOrWhiteSpace(candidateP.Gender) ? GetInitCap(candidateP.Gender) : "NA";

                // amit_apaar_api_changes_may_2026_start
                string candDob = candidateP.DateOfBirth.ToString("dd-MMM-yyyy");
                lblDob.Text = candDob; 
                DateTime todaydate = DateTime.Now;
                DateTime Inputdate = Convert.ToDateTime(candDob);
                int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);

                IsAgeGreaterthan18 = countAge; // added to persist age value across multiple functions

                if (countAge >= 0)
                {
                    txtIsProviderPresent.Text = "True";
                    txtIsProviderPresent.Enabled = false;

                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Self", "1"));

                    ddlConsentRelation.SelectedIndex = 1;
                    ddlConsentRelation.Enabled = false;
                    ddlConsentRelation_SelectedIndexChanged(null, null);
                    txtproviderName.Text = candidate_name; // changed to global variable
                    txtproviderName.Enabled = false;

                    BindAuthMode(countAge);
                    ddlAuthMode.SelectedValue = "1";
                    ddlAuthMode.Enabled = false;
                    //txtAuthenticationIdNo.Text = txtapaar.Text;
                    txtAuthenticationIdNo.Enabled = false;
                    ddlAuthMode_SelectedIndexChanged(null, null);

                }
                else
                {
                    txtIsProviderPresent.Text = "True";
                    txtIsProviderPresent.Enabled = false;

                    ddlConsentRelation.Enabled = true;
                    txtproviderName.Enabled = true;
                    ddlAuthMode.Enabled = true;
                    txtAuthenticationIdNo.Enabled = true;
                    ddlConsentRelation.SelectedIndex = 0;
                    txtproviderName.Text = "";
                    // txtproviderName.Text = txtproviderName.Text;
                    // txtproviderName.ReadOnly = true;

                    BindAuthMode(countAge);
                    ddlAuthMode.SelectedValue = "0";
                    //ddlAuthMode.Enabled =true;
                    //txtproviderName.Enabled=true;
                    //txtAuthenticationIdNo.Enabled = true;
                    // ddlConsentRelation.Enabled=true;
                }
                txtConsentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtConsentTime.Text = DateTime.Now.ToString("HH:mm");

                BindApaarDeclaration(countAge); // based on user age
                //BindAuthMode(countAge); // based on user age
                ddlConsentRelation_SelectedIndexChanged(null, null); // based on user age
                // amit_apaar_api_changes_may_2026_start

                //LblCategory.Text = string.IsNullOrEmpty(candidateP.CastCategory.Name) == false && !string.IsNullOrWhiteSpace(candidateP.CastCategory.Name) ? GetInitCap(candidateP.CastCategory.Name) : "NA";
                //LblMaritalStatus.Text = string.IsNullOrEmpty(candidateP.MaritalStatus.Name) == false && !string.IsNullOrWhiteSpace(candidateP.MaritalStatus.Name) ? GetInitCap(candidateP.MaritalStatus.Name) : "NA";
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showContactDetail(Candidate candidateC)
    {
        try
        {
            LblHeading.Text = "Change  Contact Detail";              
            using (EConnectContext context = new EConnectContext())
            {
                foreach (CandidateContactDetail contact in candidateC.ContactDetails)
                {
               
                    if (contact.StdNumber.HasValue && contact.StdNumber != 0)
                    {
                        LblPhoneNo.Text = "0" + contact.StdNumber.ToString() + " - " + contact.PhoneNumber.ToString();
                        TxtSTDcode.Text = "0" + contact.StdNumber.ToString();
                    }
                    if (contact.MobileNumber.HasValue && contact.MobileNumber != 0)
                    {
                        LblMobile.Text = contact.MobileNumber.ToString();
                        TxtMobile.Text = contact.MobileNumber.ToString();
                    }
                    if (String.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress))
                    {
                        TxtEmail.Text = contact.EmailAddress.ToString();
                        LblEmail.Text = contact.EmailAddress.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showCorespondenceAddressDetail(Candidate candidateCA)
    {
        try
        {
            LblHeading.Text = "Change correspondence Address Detail";
            bindCorrState();
            using (EConnectContext context = new EConnectContext())
            {
                int AddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                var add = (from cad in context.Addresses
                              where cad.CandidateID == entityID && cad.AddressTypeID == AddTypeId
                              orderby cad.EffectiveDateFrom descending
                              select cad).FirstOrDefault();
                if(add !=null)
                {
                    TxtAddressLine1.Text = add.AddressLine1;
                    TxtAddressLine2.Text = add.AddressLine2;
                    TxtAddressLine3.Text = add.AddressLine3;
                    TxtCity.Text = add.CityName;
                    ddlCorState.SelectedValue = add.StateID.HasValue && add.StateID != 0? add.StateID.ToString() : "0";
                    if (ddlCorState.SelectedValue != "0")
                    {
                        long id1 = Convert.ToInt64(ddlCorState.SelectedValue);
                        bindDistrict(id1, ref ddldistrict);
                    }
                    ddldistrict.SelectedValue = add.DistrictID.HasValue? add.DistrictID.Value.ToString():"0";
                    TxtPincode.Text = add.PinCode.ToString();
                    LblAddressLine1.Text = GetInitCap(add.AddressLine1);
                    LblAddressLine2.Text = string.IsNullOrEmpty(add.AddressLine2) == false && !string.IsNullOrWhiteSpace(add.AddressLine2)? GetInitCap(add.AddressLine2) : "NA";
                    LblAddressLine3.Text = string.IsNullOrEmpty(add.AddressLine3) == false && !string.IsNullOrWhiteSpace(add.AddressLine3)? GetInitCap(add.AddressLine3) : "NA";
                    LblCity.Text = string.IsNullOrEmpty(add.CityName) == false && !string.IsNullOrWhiteSpace(add.CityName)? GetInitCap(add.CityName) : "NA";
                    LblState.Text = add.StateID.HasValue && add.StateID != 0 ? GetInitCap(add.State.Name) : "NA"; 
                    LblDistrict.Text = add.DistrictID.HasValue?GetInitCap(add.District.Name):"NA";
                    LblPincode.Text = add.PinCode.HasValue && add.PinCode != 0? add.PinCode.Value.ToString():"NA";
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddldistrict.Items.Clear();
            bindDistrict(Convert.ToInt32(ddlCorState.SelectedValue), ref ddldistrict);
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
            if (MultiView1.ActiveViewIndex == 1)
            {
                if ((string.IsNullOrEmpty(TxtGuardianName.Text.Trim()) == false && string.IsNullOrEmpty(TxtGuardianName.Text.Trim()) == false &&
                    string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false && string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false
                    && string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false && string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false) ||
                    (!string.IsNullOrEmpty(TxtGuardianName.Text.Trim()) == false && !string.IsNullOrEmpty(TxtGuardianName.Text.Trim()) == false
                    && !string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false && !string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false
                    && !string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false && !string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false))
                {
                    lblError.Visible = true;
                    lblError.Text = "Please enter either Guardian Name OR Father Name and Mother Name.";
                    return false;
                }
                else if (!string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false && !string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false
                    && string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false && string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Please enter Father Name.";
                    return false;
                }
                else if (string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false && string.IsNullOrEmpty(TxtFatherName.Text.Trim()) == false
                    && !string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false && !string.IsNullOrEmpty(TxtMotherName.Text.Trim()) == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Please enter Mother Name.";
                    return false;
                }


                // amit_apaar_api_changes_may_2026_start

                if (String.IsNullOrEmpty(txtapaar.Text))
                {
                 
                    throw new Exception("Apaar ID  should not be Blank.");

                }

                if (txtapaar.Text.Length != 12)
                {
                    txtapaar.Text = "";
                    txtapaar.Focus();
                    throw new Exception("Invalid ApaarID.");
                }
            
                if (ddlConsentRelation.SelectedValue == "0")
                {
                    throw new Exception("Please select Consent Relation");

                }
             

                    if (string.IsNullOrEmpty(ddlAuthMode.SelectedValue) || ddlAuthMode.SelectedValue == "0")
                    {
                        throw new Exception("Please select Authentication Mode");
                    }
             

                if (string.IsNullOrWhiteSpace(txtAuthenticationIdNo.Text.Trim()))
                {
                    throw new Exception("Authentication ID cannot be blank");
                }
                else
                {
                    bool isValid = false;
                    switch (ddlAuthMode.SelectedValue)
                    {
                        case "3": // PAN 
                            isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9]{10}$");
                            if (!isValid) throw new Exception("Enter Valid 10-digit PAN card number only. No space allowed");
                            break;
                        case "4": // DL
                            isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9/-]{10,15}$");
                            if (!isValid) throw new Exception("Enter Valid DL card number only.No space allowed");
                            break;
                        case "5": // Passport
                            isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9]{8}$");
                            if (!isValid) throw new Exception("Enter Valid 8-digit Passport ID number only. No space allowed");
                            break;
                        case "6": // EPIC
                            isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9]{10}$");
                            if (!isValid) throw new Exception("Enter Valid EPIC card number only. Only numbers and alphabets are allowed. No space allowed");
                            break;
                    }
                }


                if (string.IsNullOrWhiteSpace(txtConsentPlace.Text))
                {
                    txtConsentPlace.Focus();
                    throw new Exception("Consent Place cannot be blank");
                }

                if (string.IsNullOrWhiteSpace(txtproviderName.Text) || string.IsNullOrWhiteSpace(txtproviderName.Text))
                {
                    txtConsentPlace.Focus();
                    throw new Exception("Consent Place cannot be blank");
                }

                if (!Regex.IsMatch(
                        txtConsentPlace.Text.Trim(),
                        @"^(?=.{5,30}$)[A-Za-z]+(?:'[A-Za-z]+)*(?: [A-Za-z]+(?:'[A-Za-z]+)*)?$"))
                {
                    throw new Exception("Consent Place must contain one or two words, using only letters and apostrophes.");
                }

                if (chkApaarDeclaration.Checked != true)
                {
                   
                    throw new Exception("Please tick the Apaar Declaration Statement");
                }

                if (chkWarning.Checked != true)
                {

                    throw new Exception("Please tick the Final Declaration");
                }


                // amit_apaar_api_changes_may_2026_end




            }
            if (MultiView1.ActiveViewIndex == 2) // contanct details
            {
                if (string.IsNullOrEmpty(TxtSTDcode.Text.Trim()) == false && string.IsNullOrEmpty(TxtPhoneNo.Text.Trim()) == false)
                {
                    if (IsNumeric(TxtSTDcode.Text.Trim()) == false)
                    {
                        lblError.Visible = true;
                        lblError.Text = "Only Numbers are allowed in  STD Code!";
                        return false;
                    }
                    if (IsNumeric(TxtPhoneNo.Text.Trim()) == false)
                    {
                        lblError.Visible = true;
                        lblError.Text = "Only Numbers are allowed in Phone Number!";
                        return false;
                    }                                       
                }
                if (IsValidEmailAddress(TxtEmail.Text.Trim()) == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Email Address !";
                    return false;
                }
                if (string.IsNullOrEmpty(TxtMobile.Text.Trim()) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "Mobile Number cannot be left blank...!";
                    return false;
                }
                if (IsNumeric(TxtMobile.Text.Trim()) == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Only Numbers are allowed in  Mobile Number!";
                    return false;
                }
                if (string.IsNullOrEmpty(TxtEmail.Text.Trim()) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "Email Address cannot be left blank...!";
                    return false;
                }
                if (IsValidEmailAddress(TxtEmail.Text.Trim()) == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Email Address !";
                    return false;
                }
            }
            if (MultiView1.ActiveViewIndex == 3) // adddress detalis
            {
                if (string.IsNullOrEmpty(TxtAddressLine1.Text.Trim()) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "Address Line1 cannot be left blank...!";
                    return false;
                }
                if (string.IsNullOrEmpty(TxtCity.Text.Trim()) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "City Name cannot be left blank...!";
                    return false;
                }
                if (string.IsNullOrEmpty(TxtPincode.Text.Trim()) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "Pincode cannot be left blank...!";
                    return false;
                }
                if (IsNumeric(TxtPincode.Text.Trim()) == false)
                {
                    lblError.Visible = true;
                    lblError.Text = "Only Numbers are allowed in Pincode!";
                    return false;
                }
                if (ddlCorState.SelectedValue == "0")
                {
                    lblError.Visible = true;
                    lblError.Text = "Please select State !";
                    return false;
                }
                if (ddldistrict.SelectedValue == "0")
                {
                    lblError.Visible = true;
                    lblError.Text = "Please select District !";
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
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["SrcType"])))
            {
                if (isvalidForm())
                {
                    string userName = "";
                    string Src = MultiView1.ActiveViewIndex.ToString();
                    using (EConnectContext context = new EConnectContext())
                    {
                        // amit_apaar_api_changes_may_2026_start

                        string gender = LblGender.Text.Substring(0, 1).ToUpper();
                        long apaarRequestId = 0;

                   
                        string validatedApaarData = validateApaar.
                            ConvertApaarDatatoJSONandEncrypt
                            (txtapaar.Text.Trim(),
                            candidate_name.Trim(),
                            lblDob.Text,
                            gender,
                            txtproviderName.Text.Trim(),
                            ddlAuthMode.SelectedItem.Text,
                            txtAuthenticationIdNo.Text,
                            ddlConsentRelation.SelectedItem.Text,
                            txtConsentPlace.Text.Trim(),
                            lblApaarDeclaration.Text,
                            out apaarRequestId);

                        if (String.IsNullOrWhiteSpace(validatedApaarData))
                        {
                            throw new Exception("Apaar could not be validated.");
                        }

                        JObject apaarObj = JObject.Parse(validatedApaarData);


                        string status =
                            apaarObj["status"] == null
                            ? ""
                            : apaarObj["status"].ToString().Trim().ToLower();

                        string statusCode =
                            apaarObj["status_code"] == null
                            ? ""
                            : apaarObj["status_code"].ToString().Trim();

                        string messageCode =
                            apaarObj["message_code"] == null
                            ? ""
                            : apaarObj["message_code"].ToString().Trim();

                        string message =
                            apaarObj["message"] == null
                            ? ""
                            : apaarObj["message"].ToString().Trim();

                        using (EConnectContext db = new EConnectContext())
                        {
                            apaarResponseRecd responseObj = new apaarResponseRecd();

                            // ApaarID
                            responseObj.abc_account_id = txtapaar.Text;

                            if (apaarObj["message"].ToString() != "Records not found")
                            {
                                responseObj.abc_account_id = txtapaar.Text;
                                // cname

                                if (apaarObj["CNAME"] != null)
                                    responseObj.cname = apaarObj["CNAME"].ToString();


                                // gender
                                if (apaarObj["GENDER"] != null)
                                {
                                    string g =
                                        apaarObj["GENDER"]
                                        .ToString()
                                        .Trim()
                                        .ToUpper();

                                    if (g == "M")
                                        responseObj.genderID = 1;

                                    else if (g == "F")
                                        responseObj.genderID = 2;

                                    else if (g == "T")
                                        responseObj.genderID = 3;
                                }

                                // dob
                                if (apaarObj["DOB"] != null)
                                {
                                    DateTime parsedDob;

                                    if (DateTime.TryParseExact(apaarObj["DOB"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDob))
                                    {
                                        responseObj.dob = parsedDob;

                                    }
                                }
                            }
                            else
                            {
                                responseObj.status = false;
                                //bit values insert data according to api rsponse
                                responseObj.nameMatch = false;
                                responseObj.birthYearMatch = false;
                                responseObj.genderMatch = false;
                            }

                            // status insert , db takes bit value
                            string apiStatus =
                                apaarObj["status"] != null
                                ? apaarObj["status"].ToString().Trim().ToLower()
                                : "";

                            if (apiStatus == "success")
                            {
                                DateTime dob = Convert.ToDateTime(lblDob.Text);
                                string dateofbirth = dob.ToString("dd/MM/yyyy");

                                if (dateofbirth.Contains('-'))
                                {
                                    string[] vdob = dateofbirth.Split('-');
                                    dateofbirth = vdob[0] + "/" + vdob[1] + "/" + vdob[2];
                                }

                                string dateofbirth1 = apaarObj["DOB"].ToString();

                                if (dateofbirth1.Length < 10)
                                {
                                    string[] s = dateofbirth1.Split('/');
                                    if (s[0].Length < 2)
                                        s[0] = "0" + s[0];

                                    if (s[1].Length < 2)
                                        s[1] = "0" + s[1];



                                    dateofbirth1 = s[0] + "/" + s[1] + "/" + s[2];
                                }


                                if (dateofbirth1 != dateofbirth)
                                {
       
                                    throw new Exception("Invalid Apaar or Date of birth Mismatch, if apaar not generated, please generate or correct and enter");
                                }


                                responseObj.status = true;
                                //bit values insert data according to api rsponse
                                responseObj.nameMatch = true;
                                responseObj.birthYearMatch = true;
                                responseObj.genderMatch = true;
                            }
                            else
                            {
                                responseObj.status = false;
                                if (apaarObj["message"].ToString() == "Records not found")
                                {
                                    responseObj.status = false;
                                    //bit values insert data according to api rsponse
                                    responseObj.nameMatch = false;
                                    responseObj.birthYearMatch = false;
                                    responseObj.genderMatch = false;
                                }
                                else
                                {
                                    //bit values insert data according to api rsponse
                                    if (apaarObj["match_data_status"] != null)
                                    {
                                        JObject matchObj =
                                            (JObject)apaarObj["match_data_status"];

                                        if (matchObj["student_name_match"] != null)
                                        {
                                            responseObj.nameMatch =
                                                Convert.ToBoolean(
                                                    matchObj["student_name_match"].ToString()
                                                );
                                        }

                                        if (matchObj["year_of_birth"] != null)
                                        {
                                            responseObj.birthYearMatch =
                                                Convert.ToBoolean(
                                                    matchObj["year_of_birth"].ToString()
                                                );
                                        }

                                        if (matchObj["gender_match"] != null)
                                        {
                                            responseObj.genderMatch =
                                                Convert.ToBoolean(
                                                    matchObj["gender_match"].ToString()
                                                );
                                        }
                                    }
                                }
                            }

                            // status code 
                            responseObj.statuscode =
                                apaarObj["status_code"] != null
                                ? apaarObj["status_code"].ToString()
                                : "";

                            // status code 
                            responseObj.status_code =
                                apaarObj["status_code"] != null
                                ? apaarObj["status_code"].ToString()
                                : "";

                            // message
                            responseObj.message =
                                apaarObj["message"] != null
                                ? apaarObj["message"].ToString()
                                : "";

                            // full json response
                            responseObj.responseContent =
                                validatedApaarData;

                            // ENTER BY
                            responseObj.enterByID = 99;

                            // enter date
                            responseObj.enterDate =
                                DateTime.Now;


                            // messageCode
                            responseObj.messageCode =
                                apaarObj["message_code"] != null
                                ? apaarObj["message_code"].ToString()
                                : "";


                            // apaar requestID
                            responseObj.apaarReqID =
                                apaarRequestId;
                            db.apaarResponseRecd.Add(responseObj);
                            db.SaveChanges();
                        }

                        if (message == "Records not found")
                            throw new Exception(message);

                        if (status == "fail")
                        {
         
                            // mismatch validations

                            JObject matchObj =
                                (JObject)apaarObj["match_data_status"];

                            bool nameMatch =
                                matchObj["student_name_match"] != null
                                ? Convert.ToBoolean(
                                    matchObj["student_name_match"].ToString()
                                  )
                                : false;

                            bool dobMatch =
                                matchObj["year_of_birth"] != null
                                ? Convert.ToBoolean(
                                    matchObj["year_of_birth"].ToString()
                                  )
                                : false;

                            bool genderMatch =
                                matchObj["gender_match"] != null
                                ? Convert.ToBoolean(
                                    matchObj["gender_match"].ToString()
                                  )
                                : false;

                            if (!nameMatch)
                            {
                                throw new Exception(
                                    "APAAR validation failed : Name does not match."
                                );
                            }

                            if (!dobMatch)
                            {
                                throw new Exception(
                                    "APAAR validation failed : Date of Birth does not match."
                                );
                            }

                            if (!genderMatch)
                            {
                                throw new Exception(
                                    "APAAR validation failed : Gender does not match."
                                );
                            }

                            throw new Exception(message);
                        }
              
                        // amit_apaar_api_changes_may_2026_end



                        Int32 candidateid = Convert.ToInt32(Session["EntityID"]);
                       
                            User objUser = UserManager.GetUserByUserID(Convert.ToInt32(Session["UserID"]), context);
                            userName = objUser.UserName.ToString();
                            Int64 reqTypeID = Convert.ToInt32(HfRequestTypeID.Value);
                            CandidateRequestType req = context.CandidateRequestTypes.Where(p => p.ID == reqTypeID).FirstOrDefault();


                            switch (Src)
                            {
                                case "1":
                                    var candidate = context.Candidates.Find(candidateid);
                                    if (candidate.IsLocked == false)
                                        UpdatePersonalDetail();
                                    else
                                    {
                                        lblError.Visible = true;
                                        lblError.Text = "Record is Locked..You cannot change it any more.";
                                    }
                                    
                                    break;
                                case "2":
                                    UpdateContactDetail();
                                    break;
                                case "3":
                                    UpdateCorAddressDetail();
                                    break;
                            }
                            LblWelcome.Text = "Dear " + userName.ToLower() + ",\n";
                            LblDetail.Text = req.Name;
                            LblUpdateHeading.Text = req.Name;

                            MultiView1.Visible = false;
                            if (Src == "2")
                                TdContactUpdateMsg.Visible = true;
                            else
                                TdContactUpdateMsg.Visible = false;
                            mvAction.ActiveViewIndex = 2;
                            
                    };
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void UpdateCorAddressDetail()
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    string Src = Request.QueryString["SrcType"].ToString();
                    Int32 corrAddressID = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    Int32 userID = Convert.ToInt32(Session["UserID"]);
                    //if address is already saved by the candidate
                    if (context.Addresses.Where(a => a.AddressTypeID == corrAddressID && a.CandidateID == entityID && a.CreatedByID == userID).Count() > 0)
                    {
                        //updating the existing record
                        Address address1 = context.Addresses.Where(a => a.AddressTypeID == corrAddressID && a.CandidateID == entityID && a.CreatedByID == userID).OrderByDescending(a => a.EffectiveDateFrom).FirstOrDefault();

                        address1.AddressLine1 = TxtAddressLine1.Text;
                        if (String.IsNullOrEmpty(TxtAddressLine2.Text) == false)
                            address1.AddressLine2 = TxtAddressLine2.Text;
                        if (String.IsNullOrEmpty(TxtAddressLine3.Text) == false)
                            address1.AddressLine3 = TxtAddressLine3.Text;
                        address1.StateID = Convert.ToInt32(ddlCorState.SelectedValue);
                        address1.DistrictID = Convert.ToInt32(ddldistrict.SelectedValue);
                        address1.PinCode = Convert.ToInt32(TxtPincode.Text);
                        address1.AddressTypeID = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        address1.CandidateID = entityID;

                        var cid = context.Locations.Find(Convert.ToInt32(ddlCorState.SelectedValue));
                        address1.CountryID = cid.ParentLocationID.Value;
                        address1.IsVerified = false;
                        address1.EffectiveDateFrom = DateTime.Now;
                        address1.CityName = TxtCity.Text;
                        context.Entry(address1).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    else
                    {
                        Address objAddress2 = new Address();
                        objAddress2.AddressLine1 = TxtAddressLine1.Text;
                        if (String.IsNullOrEmpty(TxtAddressLine2.Text) == false)
                            objAddress2.AddressLine2 = TxtAddressLine2.Text;
                        if (String.IsNullOrEmpty(TxtAddressLine3.Text) == false)
                            objAddress2.AddressLine3 = TxtAddressLine3.Text;
                        objAddress2.StateID = Convert.ToInt32(ddlCorState.SelectedValue);
                        objAddress2.DistrictID = Convert.ToInt32(ddldistrict.SelectedValue);
                        objAddress2.PinCode = Convert.ToInt32(TxtPincode.Text);
                        objAddress2.AddressTypeID = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        objAddress2.CandidateID = entityID;

                        var cid = context.Locations.Find(Convert.ToInt32(ddlCorState.SelectedValue));
                        objAddress2.CountryID = cid.ParentLocationID.Value;
                        objAddress2.IsVerified = false;
                        objAddress2.EffectiveDateFrom = DateTime.Now;
                        objAddress2.CreatedOn = DateTime.Now;

                        objAddress2.CreatedByID = Convert.ToInt32(Session["UserID"]);
                        objAddress2.CityName = TxtCity.Text;
                        context.Addresses.Add(objAddress2);
                        context.SaveChanges();
                    }
                    scope.Complete();
                };
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void GenerateMobileOTP()
    //{
    //    try { 
        
    //        Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var mobile = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();
    //            if (mobile.MobileOTP == null)
    //            {
    //                mobile.MobileOTP = OTP;
    //                mobile.MobileOTPValidUptoDate = DateTime.Now.Date.AddDays(1);
    //                context.Entry(mobile).State = System.Data.Entity.EntityState.Modified;
    //                EConnect.NIELIT.SMS message = new SMS("OTP for mobile number verification is " + OTP.ToString() + ". Use this OTP to complete verification process", mobile.MobileNumber.ToString(), SmsServiceType.SignleSMS);
    //                int sentMessageCount;
    //                message.Send(out sentMessageCount);
    //                if (sentMessageCount == 1)
    //                    context.SaveChanges();
    //                else
    //                    throw new Exception("OTP could not be sent due to some technical problem. Please try again later");                    
    //            }
    //        };
        
    //    }
    //    catch(Exception ex)
    //    { throw ex; }
    //}
    //protected void GenerateEmailOTP()
    //{
    //    try
    //    {
    //        Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var email = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();
    //            if (email.EmailOTP == null)
    //            {
    //                email.EmailOTP = OTP;
    //                email.EmailOTPValidUptoDate = DateTime.Now.Date.AddDays(1);
    //                context.Entry(email).State = System.Data.Entity.EntityState.Modified;
    //                context.SaveChanges();
    //                string msg = "";
    //                var obj = context.Candidates.Find(entityID);
    //                msg = "Dear " + GetInitCap(obj.Name) + ",<br/><br/>" + "OTP for Email Address verification is " + OTP.ToString() + ". Use this OTP to complete Email Address verification process<br/><br/><br/><br/><br/>Thank You,<br/> NIELIT";
    //                EConnect.NIELIT.Email mail = new Email("OTP for Email Address Verification :NIELIT", msg, email.EmailAddress);
    //                mail.Send();                    
    //            }              
    //        }
    //    }
    //    catch (Exception ex)
    //    { throw ex; }
    //}
    protected void UpdateContactDetail()
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //Save Candidate Contact History Detail.
                    CandidateContactDetail objContact = context.CandidateContactDetails.Where(c => c.CandidateID == entityID).FirstOrDefault();
                    if (objContact != null)
                    {
                        Boolean isChangedPhone = false;
                        Boolean isChangedMobile = false;
                        Boolean isChangedEmail = false;
                        Boolean isBlankEmail = false;

                        if (objContact.MobileNumber != Convert.ToInt64(TxtMobile.Text.Trim()))
                            isChangedMobile = true;
                        if (string.IsNullOrEmpty(objContact.EmailAddress) == false && string.IsNullOrWhiteSpace(objContact.EmailAddress) == false)
                        {
                            if (objContact.EmailAddress.ToUpper() != TxtEmail.Text.ToUpper())
                                isChangedEmail = true;
                        }
                        else
                            isBlankEmail = true;

                        if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(TxtPhoneNo.Text) == false && objContact.PhoneNumber != Convert.ToInt64(TxtPhoneNo.Text.Trim()))
                            isChangedPhone = true;

                        if (context.CandidateContactHistoryDetails.Where(c => c.CandidateID == entityID).Count() == 0)
                        {
                            CandidateContactHistory ContactHistory = new CandidateContactHistory();
                            ContactHistory.CandidateID = entityID;
                            ContactHistory.MobileNumber = objContact.MobileNumber;
                            ContactHistory.StdNumber = objContact.StdNumber;
                            ContactHistory.PhoneNumber = objContact.PhoneNumber;
                            ContactHistory.EmailAddress = objContact.EmailAddress;
                            ContactHistory.CreatedOn = DateTime.Now;
                            ContactHistory.CreatedByID = Convert.ToInt32(Session["UserID"]);
                            context.CandidateContactHistoryDetails.Add(ContactHistory);
                            context.SaveChanges();
                           
                        }

                        //Update Candidate Contact Detail.
                        objContact.CandidateID = entityID;
                        objContact.EffectiveFromDate = DateTime.Now;
                        if (isChangedPhone)
                        {
                            objContact.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                            objContact.PhoneNumber = Convert.ToInt32(TxtPhoneNo.Text);
                        }
                        if (isChangedMobile)
                        {
                            objContact.MobileNumber = Convert.ToInt64(TxtMobile.Text);
                            objContact.MobileNumberVerifiedOn = null;
                            objContact.IsMobileNumberVerified = false;
                        }
                        if (isChangedEmail || isBlankEmail)
                        {
                            objContact.EmailAddress = TxtEmail.Text;
                            objContact.IsEmailVerified = false;
                            objContact.EmailVerifiedOn = null;
                        }
                        context.Entry(objContact).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        if (isChangedEmail == true)
                        {
                            CommonFunctions.GenerateEmailOTP(UserType.Candidate, entityID,true);
                        }
                        if (isChangedMobile == true)
                        {
                            CommonFunctions.GenerateMobileOTP(UserType.Candidate, entityID,true);
                        }
                    }
                    else
                    {
                        CandidateContactHistory ContactHistory = new CandidateContactHistory();
                        ContactHistory.CandidateID = entityID;
                        ContactHistory.CreatedOn = DateTime.Now;
                        ContactHistory.CreatedByID = Convert.ToInt32(Session["UserID"]);
                        context.CandidateContactHistoryDetails.Add(ContactHistory);
                        context.SaveChanges();

                        CandidateContactDetail objContact1 = new CandidateContactDetail();
                        objContact1.CandidateID = entityID;
                        objContact1.EffectiveFromDate = DateTime.Now;
                        if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(TxtPhoneNo.Text) == false)
                        {
                            objContact1.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                            objContact1.PhoneNumber = Convert.ToInt32(TxtPhoneNo.Text);
                        }
                        objContact1.MobileNumber = Convert.ToInt64(TxtMobile.Text);
                        objContact1.MobileNumberVerifiedOn = null;
                        objContact1.IsMobileNumberVerified = false;
                        objContact1.EmailAddress = TxtEmail.Text;
                        objContact1.IsEmailVerified = false;
                        objContact1.EmailVerifiedOn = null;
                        context.CandidateContactDetails.Add(objContact1);
                        context.SaveChanges();
                        //CommonFunctions.GenerateEmailOTP(UserType.Candidate,entityID);
                        //CommonFunctions.GenerateMobileOTP(UserType.Candidate, entityID);
                    }
                };
                scope.Complete();           
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    // amit_apaar_api_changes_may_2026_start
    private void BindApaarDeclaration(int countAge)
    {
        string relationText = "";
        string wardText = "";
        string authDoc = "";

        if (countAge >= 0)
        {
            relationText = txtproviderName.Text + "(" + ddlConsentRelation.SelectedItem.Text + ")";
            wardText = "self";
        }
        else
        {
            relationText = txtproviderName.Text + "(" + ddlConsentRelation.SelectedItem.Text + ")";
            wardText = "ward";
        }

        authDoc = txtAuthenticationIdNo.Text.Trim();

        lblApaarDeclaration.Text =
                                  "I "
                                  + relationText +
                                  ", hereby voluntarily give my consent to NIELIT to use APAAR ID of "
                                  + LblAppName.Text +
                                  " (" + wardText + ") with APAAR ID as "
                                  + txtapaar.Text +
                                  " for validation of personal details."
                                  + "I understand that the APAAR ID may be used and shared only for limited, authorized purposes, "
                                  + "and that the information provided by me shall be kept confidential."
                                  + "The information w.r.t authentication document no. "
                                  + authDoc +
                                  " provided by me, is correct and valid to the best of my knowledge.";
    }
    protected void BindAuthMode(int whether18)
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand("BIND_AUTH_MODE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@whether18", whether18);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            ddlAuthMode.DataSource = dt;
            ddlAuthMode.DataTextField = "AuthModeName";
            ddlAuthMode.DataValueField = "AuthModeValue";
            ddlAuthMode.DataBind();

            ddlAuthMode.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlAuthMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlAuthMode.SelectedItem.Text.ToLower() == "self")
            {
                txtAuthenticationIdNo.Text = txtapaar.Text.ToString();
                txtAuthenticationIdNo.Enabled = false;
                authidrule.InnerText = "Apaar ID";
            }
            else
            {
                txtAuthenticationIdNo.Text = "";
                lblauthidrules.Visible = true;
                txtAuthenticationIdNo.Enabled = true;

                switch (ddlAuthMode.SelectedValue)
                {
                    case "3": // PAN
                        authidrule.InnerText = "10 characters only. Alphabets and numbers allowed. No spaces.";
                        break;

                    case "4": // DL
                        authidrule.InnerText = "10 to 15 characters only. Alphabets, numbers, hyphen (-). No spaces.";
                        break;

                    case "5": // Passport
                        authidrule.InnerText = "8 characters only. Alphabets and numbers allowed. No spaces.";
                        break;

                    case "6": // EPIC
                        authidrule.InnerText = "10 characters only. Alphabets and numbers allowed. No spaces.";
                        break;

                    default:
                        authidrule.InnerText = string.Empty;
                        break;
                }
            }

            //DateTime todaydate = DateTime.Now;
            //DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
            //int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
            //BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            throw new Exception("ER101 , Date Of Birth is not valid.");
        }
    }
    protected void txtAppName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("AP01, Error related to Applicant Name. Contact NIELIT HO");
        }
    }
    protected void ddlConsentRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (Rdoownertype.SelectedValue == "P")
            //{
            //    if (String.IsNullOrEmpty(txtAppName.Text) ||
            //   String.IsNullOrEmpty(txtFatherName.Text) ||
            //   String.IsNullOrEmpty(txtMotherName.Text))
            //    {
            //        lblError.Visible = true;
            //        lblError.Text = "Empty Name fields are not allowed. Check All Name fields properly";
             
            //        ShowAlert(lblError.Text, true);
            //        return;
            //    }
            //}
            //else
            //{
            //    if (String.IsNullOrEmpty(TxtGuardianName.Text))
            //    {
            //        TxtGuardianName.Focus();
            //        lblError.Visible = true;
            //        lblError.Text = "Guardian Name is Empty";
            //        ShowAlert(lblError.Text, true);
            //        return;

            //    }
            //}

            txtproviderName.Text = "";

            switch (ddlConsentRelation.SelectedValue)
            {
                case "1":
                    txtproviderName.Text = candidate_name;
                    break;

                case "2":
                    txtproviderName.Text =  TxtGuardianName.Text ;
                    break;

                case "3":
                    txtproviderName.Text = TxtFatherName.Text;
                    break;

                case "4":
                    txtproviderName.Text = TxtMotherName.Text ;
                    break;

                default:
                    txtproviderName.Text = "";
                    break;
            }
            txtproviderName.Enabled = false;
            DateTime todaydate = DateTime.Now;
            DateTime inputdate = Convert.ToDateTime(lblDob.Text);

            int countAge = DateTime.Compare(todaydate.AddYears(-18), inputdate);

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            throw new Exception("Enter Proper Date of Birth , 01-Jan-2005");
        }
    }
    protected void txtFatherName_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            string err = "Enter Father name properly";
            ShowAlert(err, true);
            lblError.Text = err;
        }
    }
    protected void txtMotherName_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            string err = "Enter Mother name properly";
            ShowAlert(err, true);
            lblError.Text = err;
        }
    }
    protected void txtGuardianName_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            string err = "Enter Guardian name properly";
            ShowAlert(err, true);
            lblError.Text = err;
        }
    }
    protected void txtapaar_TextChanged(object sender, EventArgs e)
    {
        try
        {

            // for the case of less than 18 year  and then have to choose consent relation provider.

            int countAge = IsAgeGreaterthan18;

            if (countAge <= 0) // only in case when candidate is less than 18
            {

                if (!String.IsNullOrEmpty(TxtGuardianName.Text))
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
                }
                else
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
                    ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));
                }
            }
            else
            {
                    txtAuthenticationIdNo.Text = txtapaar.Text;
                }
        }
        catch(Exception ex)
        {
            ShowAlert("Apaar not found");
        }
    }
    // amit_apaar_api_changes_may_2026_end

    protected void UpdatePersonalDetail()
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 userID  = Convert.ToInt32(Session["UserID"]);
                    string Src = Request.QueryString["SrcType"].ToString();
                    var candidateP = (from a in context.Candidates
                                        where a.ID == entityID
                                        select a).FirstOrDefault();
                    //Save Candidate Personal History Detail.
                    if (context.CandidateHistory.Where(h => h.CandidateID == entityID && h.CreatedByID == userID).Count() == 0)
                    {
                        CandidateHistory CandHistory = new CandidateHistory();
                        CandHistory.CandidateID = entityID;
                        CandHistory.FatherName = candidateP.FatherName;
                        CandHistory.MotherName = candidateP.MotherName;
                        CandHistory.GuardianName = candidateP.GuardianName;
                        CandHistory.Gender = candidateP.Gender;
                        CandHistory.CreatedOn = DateTime.Now;
                        CandHistory.CreatedByID = userID;
                        context.CandidateHistory.Add(CandHistory);
                        context.SaveChanges();
                    }
                    //Update Candidate Personal Detail...
                    if (string.IsNullOrEmpty(TxtGuardianName.Text.Trim()) == true && string.IsNullOrWhiteSpace(TxtGuardianName.Text.Trim()) == true)
                    {
                        candidateP.FatherName = TxtFatherName.Text.Trim().ToString();
                        candidateP.MotherName = TxtMotherName.Text.Trim().ToString();
                        candidateP.GuardianName = null;
                    }
                    else
                    {
                        candidateP.GuardianName = TxtGuardianName.Text.Trim().ToString();
                        candidateP.FatherName = null;
                        candidateP.MotherName = null;
                    }
                    string gender = rdbtnlstgender.SelectedItem.Text.ToString();
                    gender = gender.Substring(0, gender.IndexOf('/'));
                    candidateP.Gender = gender.Trim();
                    candidateP.Salutation = (gender.Trim() == "Male") ? "Mr." : "Ms.";
                    candidateP.EffectiveFromDate = DateTime.Now;
                    context.Entry(candidateP).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    scope.Complete();
                };
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void LnkBtnNo_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CAND/myprofile.aspx");
    }
    protected void BtnAllContinue_Click(object sender, EventArgs e)
    {
        try
        {
            if (changeRequestTypeID == 0)
            {
                changeRequestTypeID = Convert.ToInt32(DdlAllDetail.SelectedValue);
                HfRequestTypeID.Value = DdlAllDetail.SelectedValue;
                GetDetail(changeRequestTypeID);
            }
            else
                GetDetail(changeRequestTypeID);
            if (changeRequestTypeID != 0)
            {
                mvAction.ActiveViewIndex = 0;
                mvAction.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnCancelUpdate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CAND/myprofile.aspx");
    }
    protected void LnkBtnBacktoProfile2(object sender, EventArgs e)
    {
        Response.Redirect("../FrmDashBoard.aspx");
    }
    protected void LnkBtnBacktoProfile1_Click(object sender, EventArgs e)
    {
        Response.Redirect("../FrmDashBoard.aspx");
    }
    protected void BtnOTPcontinue_Click(object sender, EventArgs e)
    {
        try
        {
            changeRequestTypeID = MultiView1.ActiveViewIndex;
            View activeView = MultiView1.Views[MultiView1.ActiveViewIndex];
            activeView.Controls[1].Visible = false;
            activeView.Controls[3].Visible = true;
            mvAction.ActiveViewIndex = 1;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }    
    protected void BtnAllCancel_Click(object sender, EventArgs e)
    {
        //Response.Write(Request.UrlReferrer.AbsoluteUri.ToString());
        Response.Redirect("../FrmDashBoard.aspx");
    }
}