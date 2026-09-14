using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class CAND_CandidateScholarship : BasePage
{
    //EConnectContext context = new EConnectContext();
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                ShowName();
                BindCasteCategory();
                BindIncomeCategory();
                GenerateNewCaptchaImage();
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Feedback/Suggestions", "", ""));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            HfCaptcha.Value = ViewState["CaptchCode"].ToString();
            //imgcap.Src = "~/Handlers/CaptchaHandler.ashx?num=" + ViewState["CaptchCode"].ToString();
            EConnect.CaptchaImage captcha = new CaptchaImage(HfCaptcha.Value, 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void ShowName()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 institutetype = Convert.ToInt32(enmApplicantType.Institute);
                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 orderby a.EffectiveFromDate descending
                                 select new
                                 {
                                     name = a.Name,
                                     dob = a.DateOfBirth,
                                     email = a.ContactDetails.OrderByDescending(t => t.EffectiveFromDate).FirstOrDefault().EmailAddress,
                                     mobile = a.ContactDetails.OrderByDescending(t => t.EffectiveFromDate).FirstOrDefault().MobileNumber,
                                     gender = a.Gender
                                 }).FirstOrDefault();
                if (candidate != null)
                {
                    lblcandname.Text = GetInitCap(candidate.name);
                    lblcanddob.Text = candidate.dob.ToString("dd-MMM-yyyy");
                    lblgender.Text = candidate.gender;

                    if (!String.IsNullOrEmpty(candidate.email))
                        lblcandemail.Text = candidate.email;
                    else
                        lblcandemail.Text = "NA";
                    if (candidate.mobile.HasValue == true)
                        lblcandmobno.Text = candidate.mobile.Value.ToString();
                    else
                        lblcandmobno.Text = "NA";

                    var institute = (from r in context.RegistrationDetails
                                     where r.CandidateID == entityID
                                     && r.ApplicantTypeID == institutetype
                                     orderby r.ValidUptoDate descending
                                     select new
                                     {
                                         instname = r.InstituteID.HasValue ? r.Institute.Name : "NA",
                                         instituteid = r.InstituteID.HasValue ? r.InstituteID : 0,
                                         courseid = r.CourseID
                                     }).FirstOrDefault();

                    if (institute.instituteid != 0)
                        lblcandinsdetails.Text = institute.instname + " ( " + context.AccreditationDetails.Where(s => s.InstituteID == institute.instituteid && s.CourseID == institute.courseid).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault().AccreditationNumber + " ) ";
                    else
                        lblcandinsdetails.Text = "NA";
                }
                else
                {

                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.Render();
            Response.Redirect("../frmDashBoard.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    CandidateScholarship scholarship = new CandidateScholarship();
                    if (!context.CandidateScholarships.Any(s => s.CandidateID == entityID))
                    {
                        scholarship.AnnualIncome = Convert.ToInt32(ddlannualincome.SelectedValue);
                        if (!String.IsNullOrEmpty(Txtaadharnumber.Text))
                            scholarship.AadharNumber = Convert.ToInt64(Txtaadharnumber.Text.Trim());
                        else
                            scholarship.AadharNumber = null;
                        if (!String.IsNullOrEmpty(txtaccholdername.Text))
                            scholarship.AccountHolderName = txtaccholdername.Text.Trim();
                        else
                            scholarship.AccountHolderName = null;

                        if (Rdhandicapped.SelectedValue == "Y")
                            scholarship.IsHandicaped = true;
                        else
                            scholarship.IsHandicaped = false;

                        scholarship.AccountNumber = txtaccountnumber.Text.Trim();
                        scholarship.BankAddress = txtbaddress.Text.Trim();
                        scholarship.BankName = txtbname.Text.Trim();
                        scholarship.CasteCategory = Convert.ToInt32(ddlcategory.SelectedValue);
                        scholarship.TypeOfAccount = txtacctype.Text;
                        scholarship.IFSCCode = txtifsccode.Text;
                        scholarship.Date = DateTime.Now;
                        scholarship.CandidateID = entityID;

                        context.CandidateScholarships.Add(scholarship);
                        context.SaveChanges();

                        //Save Documents File.

                        UploadedFile Aadharfile = new EConnect.NIELIT.UploadedFile();
                        if (fuaadhar.HasFile)
                        {
                            Aadharfile.Name = "SC-AD-" + scholarship.CandidateID;
                            Aadharfile.OriginalName = fuaadhar.FileName.ToString();
                            Aadharfile.Extension = System.IO.Path.GetExtension(fuaadhar.FileName).ToLower();
                            Aadharfile.BlobFile = fuaadhar.FileBytes;
                            Aadharfile.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(Aadharfile);
                            context.SaveChanges();
                        }

                        UploadedFile IncomeFile = new EConnect.NIELIT.UploadedFile();
                        IncomeFile.Name = "SC-IN-" + scholarship.CandidateID;
                        IncomeFile.OriginalName = fuincomecertificate.FileName.ToString();
                        IncomeFile.Extension = System.IO.Path.GetExtension(fuincomecertificate.FileName).ToLower();
                        IncomeFile.BlobFile = fuincomecertificate.FileBytes;
                        IncomeFile.UploadedOn = DateTime.Now;
                        context.UploadedFiles.Add(IncomeFile);
                        context.SaveChanges();

                        UploadedFile CasteCertificateFile = new EConnect.NIELIT.UploadedFile();
                        CasteCertificateFile.Name = "SC-CC-" + scholarship.CandidateID;
                        CasteCertificateFile.OriginalName = fucastecertificate.FileName.ToString();
                        CasteCertificateFile.Extension = System.IO.Path.GetExtension(fucastecertificate.FileName).ToLower();
                        CasteCertificateFile.BlobFile = fucastecertificate.FileBytes;
                        CasteCertificateFile.UploadedOn = DateTime.Now;
                        context.UploadedFiles.Add(CasteCertificateFile);
                        context.SaveChanges();

                        UploadedFile PhysicalHandicapFile = new EConnect.NIELIT.UploadedFile();
                        if (fuhandicapped.HasFile)
                        {
                            PhysicalHandicapFile.Name = "SC-PH-" + scholarship.CandidateID;
                            PhysicalHandicapFile.OriginalName = fuhandicapped.FileName.ToString();
                            PhysicalHandicapFile.Extension = System.IO.Path.GetExtension(fuhandicapped.FileName).ToLower();
                            PhysicalHandicapFile.BlobFile = fuhandicapped.FileBytes;
                            PhysicalHandicapFile.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(PhysicalHandicapFile);
                            context.SaveChanges();
                        }

                        UploadedFile BankAccountfile = new EConnect.NIELIT.UploadedFile();
                        BankAccountfile.Name = "SC-BA-" + scholarship.CandidateID;
                        BankAccountfile.OriginalName = fuaccount.FileName.ToString();
                        BankAccountfile.Extension = System.IO.Path.GetExtension(fuaccount.FileName).ToLower();
                        BankAccountfile.BlobFile = fuaccount.FileBytes;
                        BankAccountfile.UploadedOn = DateTime.Now;
                        context.UploadedFiles.Add(BankAccountfile);
                        context.SaveChanges();

                        //Update Scholarship table with file_id from uploaded_file table.
                        if (fuaadhar.HasFile)
                            scholarship.AaadharFileID = Aadharfile.ID;
                        scholarship.BankAccountFileID = BankAccountfile.ID;
                        scholarship.CasteCertificateFileID = CasteCertificateFile.ID;
                        if (fuhandicapped.HasFile)
                            scholarship.PhysicalHandicapFileID = PhysicalHandicapFile.ID;
                        scholarship.IncomeProofFileID = IncomeFile.ID;
                        context.Entry(scholarship).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        String mobileMsg = "";
                        String msg = "";
                        String name = context.Candidates.Where(s => s.ID == entityID).FirstOrDefault().Name;
                        String salutation = context.Candidates.Where(s => s.ID == entityID).FirstOrDefault().Salutation;

                        mobileMsg = "You have successfully submitted your Aadhaar And Bank Account Details for availing the scholarship on " + scholarship.Date.ToString("dd-MMM-yyyy hh:mm:ss");

                        msg = "Dear " + GetInitCap(salutation + " " + name) + "<br/><br/>" + " You have successfully submitted your Aadhaar & Bank Account Details for availing the scholarship through Information and Enrollment System of NIELIT on " + scholarship.Date.ToString("dd-MMM-yyyy hh:mm:ss");

                        try
                        {
                            //sending Email 
                            if (lblcandemail.Text.Trim().Length > 0)
                            {
                                EConnect.NIELIT.Email mail = new Email("Aadhaar And Bank Account Details Form : NIELIT ", msg, lblcandemail.Text.Trim());
                                mail.Send();
                            }
                        }
                        catch (Exception) { }

                        try
                        {
                            //Sending Mobile Message
                            if (lblcandmobno.Text != "0")
                            {
                                EConnect.NIELIT.SMS message = new SMS(mobileMsg, lblcandmobno.Text.ToString(),"1307161053009306519", SmsServiceType.SignleSMS);
                                int sentMessageCount;
                                message.sendSingleSMS(out sentMessageCount);
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                    }
                };

                Response.Redirect("../frmDashBoard.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public bool IsValidForm()
    {
        try
        {
            if (ddlannualincome.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Annual Income of Parents.");
            }


            if (ddlcategory.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Category.");
            }

            if (!string.IsNullOrEmpty(Txtaadharnumber.Text))
            {
                if (!IsNumeric(Txtaadharnumber.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Incorrect Aadhar Number.");
                }
            }

            if ((String.IsNullOrWhiteSpace(txtaccountnumber.Text)))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Account Number cannot be left blank.");
            }

            if (!IsNumeric(txtaccountnumber.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Incorrect Account Number.");
            }

            if (!string.IsNullOrEmpty(txtaccholdername.Text))
            {
                if (!Char.IsLetter(txtaccholdername.Text, 0))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Account Holder name must start with an alphabet.");
                }
            }

            if ((String.IsNullOrWhiteSpace(txtacctype.Text)))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Account Type cannot be left blank.");
            }

            if (IsNumeric(txtacctype.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Incorrect Account Type.");
            }

            if ((String.IsNullOrWhiteSpace(txtifsccode.Text)))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("IFSC Code cannot be left blank.");
            }

            if ((String.IsNullOrWhiteSpace(txtbname.Text)))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Bank Name cannot be left blank.");
            }

            if ((String.IsNullOrWhiteSpace(txtbaddress.Text)))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Bank Address cannot be left blank.");
            }


            if (!String.IsNullOrEmpty(Txtaadharnumber.Text))
            {
                if (fuaadhar.HasFile == false)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Aadhar Card file cannot be left blank.");
                }
            }

            if (fuaadhar.HasFile && !isvalidFileExtension(fuaadhar))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Aadhar Card file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
            }
            if (fuaadhar.HasFile && !isvalidFileSize(fuaadhar, 256000))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Aadhar Card file size should be of 250 KB or less.");
            }

            if (fuincomecertificate.HasFile == false)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Income Certificate file cannot be left blank.");
            }
            if (fuincomecertificate.HasFile && !isvalidFileExtension(fuincomecertificate))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Income Certificate file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
            }
            if (fuincomecertificate.HasFile && !isvalidFileSize(fuincomecertificate, 256000))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception(" Income Certificate file size should be of 250 KB or less.");
            }

            if (Rdhandicapped.SelectedValue == "Y")
            {
                if (fuhandicapped.HasFile == false)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Physically Handicapped file cannot be left blank.");
                }
            }

            if (fuhandicapped.HasFile && !isvalidFileExtension(fuhandicapped))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Physically Handicapped file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
            }
            if (fuhandicapped.HasFile && !isvalidFileSize(fuhandicapped, 256000))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Physically Handicapped file size should be of 250 KB or less.");
            }


            if (fucastecertificate.HasFile == false)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Caste Certificate file cannot be left blank.");
            }

            if (fucastecertificate.HasFile && !isvalidFileExtension(fucastecertificate))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Caste Certificate file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
            }
            if (fucastecertificate.HasFile && !isvalidFileSize(fucastecertificate, 256000))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Caste Certificate file size should be of 250 KB or less.");
            }

            if (fuaccount.HasFile == false)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Bank Account Pass Book file cannot be left blank.");
            }

            if (fuaccount.HasFile && !isvalidFileExtension(fuaccount))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Bank Account Pass Book file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
            }
            if (fuaccount.HasFile && !isvalidFileSize(fuaccount, 256000))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Bank Account Pass Book file size should be of 250 KB or less.");
            }

            if (String.IsNullOrWhiteSpace(txtcode.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Captcha Code can not be left blank");
            }
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Captcha Code does not match with the code shown in image above.");
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindCasteCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var castecategory = from p in context.CastCategories
                                    orderby p.DisplayOrder
                                    select new { ValueField = p.ID, TextField = p.Name + "/" + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategory, castecategory, new ListItem("--Select One--", "0"));
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindIncomeCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                var incategory = from p in context.IncomeCategories
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlannualincome, incategory, new ListItem("--Select One--", "0"));
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}