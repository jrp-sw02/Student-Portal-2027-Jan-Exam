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
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminCandidatePersonaldetail : BasePage
{
    string apaarID = "";
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    String currentRoleName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        //Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            currentRoleName = (string)Session["RoleName"];
            if (currentRoleName == "Technical Support Query")
            {
                //Response.Write("Sorry! You don't have rights  to view this page");
                btnSave.Visible = false;
                //Response.End();
            }

            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/SearchCandidate.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            if (!Page.IsPostBack)
            {
                bindMaritalStatus();
                bindCastCategory();
                bindGender();
                if (!String.IsNullOrEmpty(Request.QueryString["key"]))
                {

                    ShowEditMode();
                   
                    txtDob_TextChanged(null, null); // amit_apaar_api_changes_may_2026
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();

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
    public void bindMaritalStatus()
    {

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var maritalStatus = from p in context.MaritalStatus
                                orderby (p.DisplayOrder)
                                select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlMaritalStatus, maritalStatus, lst);
        };

    }
    public void bindGender()
    {
        //  EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlGender, typeof(EConnect.Gender), new ListItem("--Select One--", "0"));
        //Added for gender
        using (EConnectContext vContext = new EConnectContext())
        {
            var Gender = from s in
                             vContext.tblGender
                         select new { ValueField = s.genderCode, TextField = s.name };

            EConnect.Utils.Common.ControlUtility.BindListObject(ddlGender, Gender, new ListItem("--Select Gender--", "0"));

        }

    }
    public void bindCastCategory()
    {

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var castcategory = from p in context.CastCategories
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
        };


    }
    protected void ShowEditMode()
    {
        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Personal Detail:Update", "", ""));
            BreadCrumb1.Render();
            context = new EConnectContext();
            string name = "";
            if (Request.QueryString["Name"].StartsWith("OTHERS"))
                name = Request.QueryString["Name"].ToLower().Substring(6);
            else
                name = Request.QueryString["Name"].ToLower().Substring(3);
            // string name = Request.QueryString["Name"].ToLower().Substring(3);
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            var student = (from s in context.Candidates
                           where s.Name == name && s.ID == Appid
                           select new
                           {
                               name = s.Name,
                               fname = s.FatherName,
                               mname = s.MotherName,
                               dob = s.DateOfBirth,
                               gender = s.Gender,
                               maritalstatus = (s.MaritalStatusID.HasValue) ? s.MaritalStatusID.Value : 0,
                               category = (s.CastCategoryID.HasValue) ? s.CastCategoryID.Value : 0,
                               effdate = s.EffectiveFromDate,
                               religion = (s.ReligionID.HasValue) ? s.ReligionID.Value : 0,
                               salution = s.Salutation,
                               Gname = s.GuardianName,
                               IsHandi = s.IsHandicaped,
                               IsExSerMan = s.IsExServicemane
                           }).FirstOrDefault();
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Update Personal Detail";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields
            if (student.salution.ToString().ToUpper() == "MR.")
                ddlSalutation.SelectedValue = "1";
            else
            {
                if (student.salution.ToString().ToUpper() == "MS.")
                    ddlSalutation.SelectedValue = "2";
                else
                    ddlSalutation.SelectedValue = "3";
            }
            txtName.Text = student.name.ToUpper();
            //Modified for gender
            if (student.gender.ToString().ToUpper() == "FEMALE")
                ddlGender.SelectedValue = "Female";
            else
            {
                if (student.gender.ToString().ToUpper() == "MALE")
                    ddlGender.SelectedValue = "Male";
                else
                    ddlGender.SelectedValue = "Trans";
            }
            if (string.IsNullOrEmpty(student.Gname) == true && string.IsNullOrWhiteSpace(student.Gname) == true)
            {
                if (string.IsNullOrEmpty(student.fname) == false && !string.IsNullOrWhiteSpace(student.fname))
                    Txt_Fname.Text = GetInitCap(student.fname);
                else
                    Txt_Fname.Text = " ";
                if (string.IsNullOrEmpty(student.mname) == false && string.IsNullOrWhiteSpace(student.mname) == false)
                    Txt_Mname.Text = GetInitCap(student.mname);
                else
                    Txt_Mname.Text = " ";
            }
            else
            {
                Txt_GName.Text = string.IsNullOrEmpty(student.Gname) == false && string.IsNullOrWhiteSpace(student.Gname) == false ? GetInitCap(student.Gname) : " ";
            }
            if (student.IsHandi == true)
                ddlIsHandi.SelectedValue = "2";
            else
                ddlIsHandi.SelectedValue = "1";
            if (student.IsExSerMan == true)
                ddlIsExService.SelectedValue = "2";
            else
                ddlIsExService.SelectedValue = "1";
            Txt_Dob.Text = student.dob.ToString("dd-MMM-yyyy");
            ddlMaritalStatus.SelectedValue = student.maritalstatus.ToString();
            ddlCategory.SelectedValue = student.category.ToString();
            Txt_EffDate.Text = student.effdate.ToString("dd-MMM-yyyy");
            Txt_EffDate.Enabled = false;

            //vishal 25-03-2022
            if (Appid != 0)
            {
                int PaymentStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                var cra_AppDetails = context.CourseRegistrationApplications.Where(i => i.CandidateID == Appid && i.Name == name && i.FinalSubmitted == true && i.PaymentStatusID == PaymentStatus).FirstOrDefault();
                if (cra_AppDetails == null)
                {
                    LabelAadharNo.Visible = false;
                    TextBoxAadharNo.Visible = false;
                    return;
                }
                if (cra_AppDetails.AadharNumber != null)
                {
                    TextBoxAadharNo.Text = Convert.ToString(cra_AppDetails.AadharNumber);
                    LabelAadharNo.Visible = false;
                    TextBoxAadharNo.Visible = false;
                    TextBoxAadharNo.Enabled = false;
                }
                else
                {
                    LabelAadharNo.Visible = true;
                    TextBoxAadharNo.Visible = true;
                    TextBoxAadharNo.Text = "";
                    TextBoxAadharNo.Enabled = true;
                }
            }
            //vishal 25-03-2022
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
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            var student = (from s in context.Candidates
                           where s.ID == Appid
                           select new
                           {
                               ID = s.ID,
                               appid = Appid,
                               Name = s.Salutation.ToUpper() + s.Name.ToUpper(),
                               Fname = ((s.FatherName != null && s.FatherName != " ") ? "Mr." + s.FatherName.ToUpper() : "NA"),
                               Mname = ((s.MotherName != null && s.MotherName != " ") ? "Mrs." + s.MotherName.ToUpper() : "NA"),
                               gender = s.Gender.ToUpper(),
                               effdate = s.EffectiveFromDate
                           });

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.ID);
                        else
                            student = student.OrderBy(s => s.ID);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Name);
                        else
                            student = student.OrderBy(s => s.Name);
                        break;
                    case "Fname":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Fname);
                        else
                            student = student.OrderBy(s => s.Fname);
                        break;
                    case "Mname":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Mname);
                        else
                            student = student.OrderBy(s => s.Mname);
                        break;
                    case "gender":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.gender);
                        else
                            student = student.OrderBy(s => s.gender);
                        break;
                    case "effdate":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.effdate);
                        else
                            student = student.OrderBy(s => s.effdate);
                        break;
                    default:
                        student = student.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(student, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Personal Detail", "Admin/AdminCandidatePersonaldetail.aspx?key1=" + Request.QueryString["key1"], ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Personal Detail", "Admin/AdminCandidatePersonaldetail.aspx?" + Request.QueryString.ToString(), ""));
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
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New Personal Details";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Personal Details", "#", ""));
            //Change the heading text as required
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidatePersonaldetail.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("AdminCandidatePersonaldetail.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }

    // // amit_apaar_api_changes_may_2026_start
    protected bool isValidForm()
    {
        try
        {
            
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

            if (string.IsNullOrEmpty(ddlConsentRelation.SelectedValue))
            {
                throw new Exception("Please select Consent Relation");


            }

                if (string.IsNullOrEmpty(ddlAuthMode.SelectedValue) || ddlAuthMode.SelectedValue == "0")
                {
                ddlAuthMode.Focus();
                    throw new Exception("Please select Authentication Mode");
                }
       
            if (string.IsNullOrWhiteSpace(txtAuthenticationIdNo.Text.Trim()))
            {
                txtAuthenticationIdNo.Focus();
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

            if (!Regex.IsMatch(
                    txtConsentPlace.Text.Trim(),
                    @"^(?=.{5,30}$)[A-Za-z]+(?:'[A-Za-z]+)*(?: [A-Za-z]+(?:'[A-Za-z]+)*)?$"))
            {
                throw new Exception("Consent Place must contain one or two words, using only letters and apostrophes.");
            }

            if (chkApaarDeclaration.Checked != true)
            {
                throw new Exception("Please check the Apaar Declaration Statement");
            }

            if (chkDeclaration2.Checked != true)
            {
                throw new Exception("Please check the Final Declaration Statement");
            }

            return true;

        }catch(Exception ex)
        {
            throw ex;
        }
    }

    // amit_apaar_api_changes_may_2026_end

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            if (isValidForm())
            {
                BreadCrumb1.Render();
                context = new EConnectContext();
                //string name = Request.QueryString["Name"].ToLower().Substring(3);
                string name = "";
                if (Request.QueryString["Name"].StartsWith("OTHERS"))
                    name = Request.QueryString["Name"].ToLower().Substring(6);
                else
                    name = Request.QueryString["Name"].ToLower().Substring(3);
                Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);


                // apaar validation start
                string gender = ddlGender.SelectedValue.Substring(0, 1).ToUpper();
                long apaarRequestId = 0;

                //try
                //{
                string validatedApaarData = validateApaar.
                    ConvertApaarDatatoJSONandEncrypt(txtapaar.Text.Trim(),
                    txtName.Text.Trim(),
                    Txt_Dob.Text, gender,
                    txtproviderName.Text.Trim(), ddlAuthMode.SelectedItem.Text,
                    txtAuthenticationIdNo.Text, ddlConsentRelation.SelectedItem.Text,
                    txtConsentPlace.Text.Trim(), lblApaarDeclaration.Text,
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
                        DateTime dob = Convert.ToDateTime(Txt_Dob.Text);
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
                // amit_apaar_may_2026_end


                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(900)))  // vishal
                    {
                        //vishal 28-03-2022
                        int PaymentStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                        if (isBlank(TextBoxAadharNo))
                        {
                            string state1 = "True";
                            string state2 = Convert.ToString(TextBoxAadharNo.Enabled);
                            if (state1 == state2)
                            {
                                if (!IsNumeric(TextBoxAadharNo.Text.Trim()))
                                {
                                    ShowAlert("Please enter only numeric value for Aadhar Card No.");
                                    return;
                                }
                                if (!IsValidAAdharNo(TextBoxAadharNo.Text.Trim()))
                                {
                                    ShowAlert("Please enter a valid Aadhar Card No. of 12 digites");
                                    return;
                                }

                                var cra1 = (from a in context.CourseRegistrationApplications
                                            where a.CandidateID == Appid && a.Name.ToLower().Equals(name) && a.FinalSubmitted == true && a.PaymentStatusID == PaymentStatus
                                            orderby a.ID descending
                                            select a).FirstOrDefault();

                                if (cra1.AadharNumber == null)
                                {
                                    cra1.AadharNumber = Convert.ToInt64(TextBoxAadharNo.Text.Trim());
                                    context.Entry(cra1).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }

                            }

                            //}
                        }
                        var cra2 = (from a in context.CourseRegistrationApplications
                                    where a.CandidateID == Appid && a.Name.ToLower().Equals(name) && a.FinalSubmitted == true && a.PaymentStatusID == PaymentStatus
                                    orderby a.ID descending
                                    select a).FirstOrDefault();
                        //vishal 28-03-2022
                       // if (cra2.apaarID == null)
                       if(cra2.apaarID == null)
                        {
                            ShowAlert("Candidate data not found in our records properly. Please contact HO");
                            return;
                             
                            //ShowAlert("ApaarId of Candidate is missing, Cannot update");
                            //return;
                        }
                        else
                        {
                            if(cra2.apaarID!=null)
                                apaarID = EncryptDecrypt.DecryptString(cra2.apaarID);
                        }

                        var student = (from s in context.Candidates
                                       where s.Name == name && s.ID == Appid
                                       select s).FirstOrDefault();

                        if (ddlSalutation.SelectedValue == "1")
                            student.Salutation = "Mr.";
                        else
                        {
                            if (ddlSalutation.SelectedValue == "2")
                                student.Salutation = "Ms.";
                            else
                                student.Salutation = "Others";
                        }

                        //Commented due to Apaar
                        student.Name = txtName.Text;

                        //Added for gender
                  //       ddlGender.SelectedValue = student.Gender;

                        //Commented due to Apaar
                        student.Gender = ddlGender.SelectedValue.Trim();

                       // if(ddlGender.SelectedValue=="1")
                       //     student.Gender = "Female";
                       //else
                       //   student.Gender = "Male";

                        if ((isBlank(Txt_GName) && isBlank(Txt_Fname) && isBlank(Txt_Mname)) || (!isBlank(Txt_GName) && !isBlank(Txt_Fname) && !isBlank(Txt_Mname)))
                        {
                            ShowAlert("Please enter either (Father Name and Mother Name) OR  Guardian Name.");
                            return;
                        }
                        else if (!isBlank(Txt_Fname) && isBlank(Txt_Mname))
                        {
                            ShowAlert("Please enter Father Name.");
                            return;
                        }
                        else if (!isBlank(Txt_Mname) && isBlank(Txt_Fname))
                        {
                            ShowAlert("Please enter Mother Name.");
                            return;
                        }
                        else if (string.IsNullOrEmpty(Txt_GName.Text) == true && string.IsNullOrWhiteSpace(Txt_GName.Text) == true)
                        {
                            student.FatherName = Txt_Fname.Text.Trim();
                            student.MotherName = Txt_Mname.Text.Trim();
                            student.GuardianName = null;
                        }
                        else
                        {
                            student.FatherName = null;
                            student.MotherName = null;
                            student.GuardianName = Txt_GName.Text.Trim();
                        }


                        if (ddlIsHandi.SelectedValue == "1")
                            student.IsHandicaped = false;
                        else
                            student.IsHandicaped = true;

                        if (ddlIsExService.SelectedValue == "1")
                            student.IsExServicemane = false;
                        else
                            student.IsExServicemane = true;

                        //student.FatherName = Txt_Fname.Text;
                        //student.MotherName = Txt_Mname.Text;

                        //Commented due to apaar
                        student.DateOfBirth = Convert.ToDateTime(Txt_Dob.Text.ToString());
                        student.MaritalStatusID = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
                        student.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                        student.EffectiveFromDate = DateTime.Now;

                        context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        // Commented due to apaar
                        strMessage = "Record Updated";
                        //strMessage = "Record Updated but name, date of birth and gender cannot be updated ";
                        Candidate_Personal_Update_temp();

                        scope.Complete();
                    }
                    ; //vishal
                }
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidatePersonaldetail.aspx?key1=" + Request.QueryString["key1"] + "&msg=" + strMessage), false);
            }
        
        }

        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message, true);

        }
        //finally { context.Dispose(); }

    }
    public void Candidate_Personal_Update_temp()//this function use of temporary teble insert recored for online to offline updation purpose.
    {
        /////////// string ipaddress;
        ////////ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        ////////if (ipaddress == "" || ipaddress == null)
        ////////    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
        /////////string clientMachineName;
        //////////clientMachineName = (System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);


        string name = Request.QueryString["Name"].ToLower().Substring(3);
        Int64 Appid = Convert.ToInt32(Request.QueryString["key1"]);
        using (EConnectContext context = new EConnectContext())
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                var student1 = (from s in context.Candidates
                                where
                                    //s.Name == name && 
                s.ID == Appid
                                select s).FirstOrDefault();

                Candidate_Personal_UpdateTemp add1 = new Candidate_Personal_UpdateTemp();

                add1.Details_Type_ID = 1;
                add1.Candidate_ID = Convert.ToInt64(Appid);
                add1.Name = txtName.Text;
                add1.Father_Name = Txt_Fname.Text.Trim();
                add1.Mother_Name = Txt_Mname.Text.Trim();
                //Added for gender
               // student1.Gender = ddlGender.SelectedValue;
                add1.Gender = ddlGender.SelectedValue;
                //if (ddlGender.SelectedValue == "1")
                //    student1.Gender = "Female";
                //else
                //    student1.Gender = "Male";
                // add1.Gender = student1.Gender;
                add1.Marital_Status_ID = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
                add1.Dob = Convert.ToDateTime(Txt_Dob.Text.ToString());
                add1.Cast_Category_ID = Convert.ToInt32(ddlCategory.SelectedValue);
                if (ddlIsHandi.SelectedValue == "1")
                    add1.Is_Handicaped = false;
                else
                    add1.Is_Handicaped = true;
                //                add1.Is_Handicaped = Convert.ToBoolean(student1.IsHandicaped);
                if (ddlIsExService.SelectedValue == "1")
                    add1.Is_Ex_Servicemane = false;
                else
                    add1.Is_Ex_Servicemane = true;
                //add1.Is_Ex_Servicemane = Convert.ToBoolean(student1.IsExServicemane);
                add1.Is_Verified = Convert.ToBoolean(student1.IsVerified);
                add1.Verified_By = Convert.ToInt32(student1.VerifiedBy);
                add1.Update_DateTime = DateTime.Now;
                add1.Client_IPAddress = "TEST";//ipaddress.ToString();
                add1.Client_UserId = "";
                add1.Client_HostName = "TEST";//clientMachineName.ToString();
                context.Candidate_Personal_UpdateTemps.Add(add1);
                context.SaveChanges();
                //ShowAlert("Record Inserted Successfully");

            }
        };

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        //try
        //{
        //    //ddlSearchUserType.SelectedValue = "0";
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    //public static String[] GetSearchText(String prefixText, Int32 count)
    //{
    //    EConnectContext context = new EConnectContext();
    //    try
    //    {
    //        if (count <= 0)
    //            count = 10;
    //        List<String> items = new List<String>();
    //        string searchString = prefixText.Trim().ToUpper();
    //        var student = from s in context.CourseRegistrationApplications
    //                    select new { Name = s.UserName };
    //        if (!String.IsNullOrEmpty(searchString))
    //        {
    //            student = student.Where(s => s.Name.ToUpper().Contains(searchString));
    //        }
    //        student = student.OrderBy(s => s.Name);

    //        var student1 = from s in context.CourseRegistrationApplications
    //                     select new { Name = s.LoginID };
    //        if (!String.IsNullOrEmpty(searchString))
    //        {
    //            student1 = student1.Where(s => s.Name.ToUpper().Contains(searchString));
    //        }
    //        student = student.Union(student1).Take(count);
    //        foreach (var user in student)
    //        {
    //            items.Add(user.Name);
    //        }
    //        return items.ToArray();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally { context.Dispose(); }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidatePersonaldetail.aspx?key1=" + Request.QueryString["key1"]));
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidAAdharNo(string AadharNo)
    {
        try
        {
            if (AadharNo.Length == 12)
            {
                return true;
            }
            else
                return false;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    //amit_apaar_api_changes_may_2026_start
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
            //DateTime Inputdate = Convert.ToDateTime(Txt_Dob.Text);
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
            Txt_Dob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            throw new Exception("AP01, Error related to Applicant Name. Contact NIELIT HO");
        }
    }
    protected void txtapaar_TextChanged(object sender, EventArgs e)
    {
        txtDob_TextChanged(sender, e);

    }
    protected void txtDob_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txtAuthenticationIdNo.Text = "";
            ddlAuthMode.SelectedIndex = 0;
            ddlConsentRelation.SelectedIndex = 0;
            txtproviderName.Text = "";
            //txtConsentDate.Text = "";
            //txtConsentTime.Text = "";
            txtConsentPlace.Text = "";

            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = DateTime.ParseExact(Txt_Dob.Text, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);

            txtapaar.Enabled = true;
            //added for Apaar Api validation check ashutosh start
            if (countAge >= 0)
            {
                txtIsProviderPresent.Text = "True";
                txtIsProviderPresent.Enabled = false;

                ddlConsentRelation.Items.Clear();
                ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                ddlConsentRelation.Items.Add(new ListItem("Self", "1"));

                ddlConsentRelation.SelectedIndex = 1;
                ddlConsentRelation.Enabled = false;
                ddlConsentRelation_SelectedIndexChanged(sender, e);
                txtproviderName.Text = txtName.Text;
                txtproviderName.Enabled = false;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedValue = "1";

                ddlAuthMode.Enabled = false;
                //ddlAuthMode.Enabled = false;
                txtAuthenticationIdNo.Text = txtapaar.Text;
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
                if (String.IsNullOrEmpty(Txt_GName.Text))
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
                    ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));
                }
                else
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
                }
                ddlConsentRelation.SelectedIndex = 0;
                txtproviderName.Text = "";
                // txtproviderName.Text = txtproviderName.Text;
                // txtproviderName.ReadOnly = true;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedIndex = 0;
                //ddlAuthMode.Enabled =true;
                //txtproviderName.Enabled=true;
                //txtAuthenticationIdNo.Enabled = true;
                // ddlConsentRelation.Enabled=true;
            }
            txtConsentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtConsentTime.Text = DateTime.Now.ToString("HH:mm");
            //added for Apaar Api validation check ashutosh end 
            //BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = "Please enter proper Date of Birth e.g. 10-Jan-2001";
            ShowAlert("Please enter proper Date of Birth");
            //throw new Exception("Please enter proper Date of Birth");

        }
    }
    protected void txtFatherName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Txt_Dob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";
            Txt_Mname.Focus();

        }
        catch (Exception ex)
        {
            string err = "Enter Father name properly";
            ShowAlert(err, true);
            lblerror.Text = err;
        }
    }
    protected void txtMotherName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Txt_Dob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            string err = "Enter Mother name properly";
            ShowAlert(err, true);
            lblerror.Text = err;
        }
    }
    protected void txtGuardianName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Txt_Dob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            string err = "Enter Guardian name properly";
            ShowAlert(err, true);
            lblerror.Text = err;
        }
    }
    protected void ddlConsentRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txtproviderName.Text = "";

            switch (ddlConsentRelation.SelectedValue)
            {
                case "1":
                    txtproviderName.Text = txtName.Text;
                    break;

                case "2":
                    txtproviderName.Text = Txt_GName.Text;
                    break;

                case "3":
                    txtproviderName.Text = Txt_Fname.Text;
                    break;

                case "4":
                    txtproviderName.Text = Txt_Mname.Text;
                    break;

                default:
                    txtproviderName.Text = "";
                    break;
            }
            txtproviderName.Enabled = false;
            DateTime todaydate = DateTime.Now;
            DateTime inputdate = Convert.ToDateTime(Txt_Dob.Text);

            int countAge = DateTime.Compare(todaydate.AddYears(-18), inputdate);

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            throw new Exception("Enter Proper Date of Birth , 01-Jan-2005");
        }
    }
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
                                  + txtName.Text +
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

    // amit_apaar_api_changes_may_2026_end

    // apaar end
    protected void ddlSalutation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Changed for gender

            if (ddlSalutation.SelectedValue == "1")
                ddlGender.SelectedValue = "Male";
            //ddlGender.SelectedValue = "1";
            if (ddlSalutation.SelectedValue == "2")
                ddlGender.SelectedValue = "Female";
            //ddlGender.SelectedValue = "2";
            if (ddlSalutation.SelectedValue == "3")
                ddlGender.SelectedValue = "Trans";


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}