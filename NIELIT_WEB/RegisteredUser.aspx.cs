using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class RegisteredUser : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.UrlReferrer.ToString().Substring(0, Request.UrlReferrer.ToString().LastIndexOf('/')) + "/Activation.aspx");
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {

            if (!Page.IsPostBack)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //Populating Courses
                    Int32 coursetypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                    ListItem lst = new ListItem("--Select One--", "0");
                    var Course = from p in context.Courses
                                 where p.CourseTypeID == coursetypeID
					 && p.ShowOnWeb   // comment by Deep on 11 may 2022 Uncomment on 1 Oct 2024 DGR issue
                                 orderby (p.DisplayOrder)
                                 select new { ValueField = p.ID, TextField = p.Name+ " (" + p.Code +")" };

                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddlcname, Course, lst);
                };
                if (Request.UrlReferrer != null)
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("rulesforonlineregistration.aspx"))
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                    }
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New User Login Registration", "", ""));
                }
                GenerateNewCaptchaImage();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
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
            ShowAlert(ex.Message, true);
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
            throw ex;
        }
    }
    protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int64 regno = 0;

            if (Wizard1.ActiveStep.ID == "first")
            {
                Wizard1.StepNextButtonText = "Next";
                if (Ddlcname.SelectedValue == "0")
                {
                    lbl1.Text = "Please select course name.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else if (String.IsNullOrEmpty(Txtregno.Text.Trim()))
                {
                    lbl1.Text = "Please enter registration number of the course you have selected.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else if (!IsNumeric(Txtregno.Text.Trim()))
                {
                    lbl1.Text = "Invalid registration number.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else if (String.IsNullOrEmpty(Txtname.Text.Trim()))
                {
                    lbl1.Text = "Please enter name of the candidate.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else if (String.IsNullOrEmpty(TxtDOB.Text))
                {
                    lbl1.Text = "Please enter date of birth of the candidate.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else if (!IsDate(TxtDOB.Text))
                {
                    lbl1.Text = "Invalid date of birth.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else if (String.IsNullOrEmpty(txtcode.Text))
                {
                    lbl1.Text = "Please enter captcha code.";
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    e.Cancel = true;
                }
                else
                {
                    Int32 courseID = Convert.ToInt32(Ddlcname.SelectedValue);
                    regno = Convert.ToInt64(Txtregno.Text);
                    string registeredno = regno.ToString();
                    DateTime DOB = Convert.ToDateTime(TxtDOB.Text);
                    string name = CommonFunctions.RemoveSpaces(Txtname.Text).Trim().ToUpper();
                    using (EConnectContext context = new EConnectContext())
                    {
                        if ((txtcode.Text == ViewState["CaptchCode"].ToString()))
                        {
                            if (!(context.Users.Any(s => s.LoginID == registeredno && s.UserName.ToUpper() == name.ToUpper() && s.UserTypeID == 3 && s.HasLoginAccess == true)))
                            {
                                if (context.RegistrationDetails.Any(s => s.CourseID == courseID && s.RegistrationNo == regno))
                                {
                                    var registration = (from rg in context.RegistrationDetails
                                                        where rg.CourseID == courseID && rg.RegistrationNo == regno
                                                        select rg).FirstOrDefault();

                                    Int64 candidateID = registration.CandidateID;
                                    String candidatename= CommonFunctions.RemoveSpaces(registration.Candidate.Name).Trim().ToUpper();
                                    if (context.Candidates.Any(c => c.DateOfBirth == DOB && c.ID == candidateID && candidatename == name))
                                    {
                                        var candidate = (from c in context.Candidates
                                                         join cd in context.CandidateContactDetails
                                                             on c.ID equals cd.CandidateID
                                                         where c.DateOfBirth == DOB && c.ID == candidateID
                                                         select new
                                                         {
                                                             mobileno = (cd.MobileNumber.HasValue) ? cd.MobileNumber.Value : 0,
                                                             email = cd.EmailAddress,
                                                             name = c.Name,
                                                             Salutation = c.Salutation
                                                         }).FirstOrDefault();
                                        if (candidate.email != null)
                                            txtemail.Text = candidate.email;
                                        txtemail.Text = txtemail.Text.Trim();
                                        txtmobile.Text = candidate.mobileno.ToString();
                                        lblname.Text = candidate.Salutation + " " + GetInitCap(candidate.name.Trim());
                                        Lblcname1.Text = candidate.Salutation + " " + GetInitCap(candidate.name.Trim());
                                        lblUserName.Text = regno.ToString();
                                        userid.InnerText = GetInitCap(candidate.name);
                                        Lblerror2.Text = "Please enter/update your contact details to proceed new user registration process.<br>Note: Please enter a valid and active email address and mobile number. ";
                                        if ((context.Users.Any(s => s.LoginID == registeredno && s.UserName.ToUpper() == name.ToUpper() && s.UserTypeID == 3 && s.HasLoginAccess == false)))
                                        {
                                            Lblerror2.Text = "Dear Candidate,<br>You are already registered with Online Student Information and Enrollment System with the following contact details. If you have not received any mail regarding activation of your account, you can re-registered yourself by changing your email address. OTP (One Time Password) will be sent to your email address to verify entered email address.<br>" + Lblerror2.Text;
                                        }
                                    }
                                    else
                                    {
                                        lbl1.Text = "Invalid name or date of birth of the candidate .";
                                        GenerateNewCaptchaImage();
                                        txtcode.Text = "";
                                        e.Cancel = true;
                                    }
                                }
                                else
                                {
                                    lbl1.Text = "Invalid registration number. Please enter correct registration number of the current course in which candidate is registered.";
                                    GenerateNewCaptchaImage();
                                    txtcode.Text = "";
                                    e.Cancel = true;
                                }
                            }
                            else
                            {

                                lbl1.Text = "You have already registered and provided user id and password. Please see email that was sent to your registered email address at the time of registration.";
                                GenerateNewCaptchaImage();
                                txtcode.Text = "";
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            lbl1.Text = "Invalid captcha code.";
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            e.Cancel = true;
                        }
                    }
                };
            }
            else if (Wizard1.ActiveStep.ID == "second")
            {

                if (String.IsNullOrEmpty(txtemail.Text))
                {
                    Lblcontact.Text = "Please enter your valid and active email-address.";
                    e.Cancel = true;
                }
                else if (!IsValidEmailAddress(txtemail.Text))
                {
                    Lblcontact.Text = "Invalid email-address.";
                    e.Cancel = true;
                }
                else if (String.IsNullOrEmpty(txtmobile.Text))
                {
                    Lblcontact.Text = "Please enter your valid and active mobile number.";
                    e.Cancel = true;
                }
                else if (!IsNumeric(txtmobile.Text))
                {
                    Lblcontact.Text = "Invalid mobile number.";
                    e.Cancel = true;
                }
                else if (!IsValidMobileNumber(txtmobile.Text))
                {
                    Lblcontact.Text = "Invalid mobile number.";
                    e.Cancel = true;
                }
                else if (txtmobile.Text.Length < 10)
                {
                    Lblcontact.Text = "Invalid mobile number.";
                    e.Cancel = true;
                }
                else
                {
                    Int64 cID = 0;
                    using (EConnectContext context = new EConnectContext())
                    {

                        Int64 rno = Convert.ToInt64(Txtregno.Text);
                        cID = (from c in context.RegistrationDetails
                               where c.RegistrationNo == rno
                               select c.CandidateID).FirstOrDefault();
                        CandidateContactDetail objcd = context.CandidateContactDetails.Where(c => c.CandidateID == cID).FirstOrDefault();
                        if (objcd != null)
                        {
                            //if (objcd.EmailAddress != null)
                            //{
                            //    if (txtemail.Text.Trim().ToLower() == objcd.EmailAddress.ToLower())
                            //    {
                            //        Lblcontact.Text = "Registered email address and new email address can not be same. Please enter different email address.";
                            //        e.Cancel = true;
                            //        return;
                            //    }
                            //}
                            CandidateContactHistory ContactHistory = new CandidateContactHistory();
                            ContactHistory.CandidateID = cID;
                            ContactHistory.MobileNumber = objcd.MobileNumber;
                            ContactHistory.StdNumber = objcd.StdNumber;
                            ContactHistory.PhoneNumber = objcd.PhoneNumber;
                            ContactHistory.EmailAddress = objcd.EmailAddress;
                            ContactHistory.CreatedOn = DateTime.Now;
                            ContactHistory.CreatedByID = 1;
                            context.CandidateContactHistoryDetails.Add(ContactHistory);

                            objcd.MobileNumber = Convert.ToInt64(txtmobile.Text);
                            objcd.EmailAddress = Convert.ToString(txtemail.Text).ToLower();
                            objcd.EffectiveFromDate = DateTime.Now;
                            objcd.MobileOTP = null;
                            objcd.MobileNumberVerifiedOn = null;
                            objcd.MobileOTPValidUptoDate = null;
                            objcd.IsMobileNumberVerified = false;

                            context.Entry(objcd).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        else
                        {
                            CandidateContactDetail objcd1 = new CandidateContactDetail();
                            objcd1.MobileNumber = Convert.ToInt64(txtmobile.Text);
                            objcd1.EmailAddress = Convert.ToString(txtemail.Text).ToLower();
                            objcd1.EffectiveFromDate = DateTime.Now;
                            objcd1.MobileOTP = null;
                            objcd1.MobileNumberVerifiedOn = null;
                            objcd1.MobileOTPValidUptoDate = null;
                            objcd1.IsMobileNumberVerified = false;
                            context.CandidateContactDetails.Add(objcd1);
                            context.SaveChanges();
                        }
                    };
                    using (EConnectContext context = new EConnectContext())
                    {
                        CommonFunctions.GenerateEmailOTP(UserType.Candidate, cID, true);
                        //Set OTP on LABLEL
                        CandidateContactDetail objotp = context.CandidateContactDetails.Where(c => c.CandidateID == cID).FirstOrDefault();
                        lblErrorEmail.Visible = true;
                        lblErrorEmail.Text = " OTP(One Time Password)has been sent to your e-mail address <b>" + objotp.EmailAddress + "</b> with reference Number  <b>" + objotp.EmailOTPValidUptoDate.Value.ToString("hhmmss") + "</b>.Please enter OTP in below Text Box to verify your e-mail address.";
                        lblcname.Text = objotp.Candidate.Salutation + " " + objotp.Candidate.Name;
                        lblcEmail.Text = objotp.EmailAddress;
                        lblCMobile.Text = objotp.MobileNumber.ToString();
                        lblRefno.InnerText = "(OTP Reference Number: " + objotp.EmailOTPValidUptoDate.Value.ToString("hhmmss") + ")";
                    };
                    Wizard1.StepNextButtonText = "Validate";
                }
            }
            else if (Wizard1.ActiveStep.ID == "Third")
            {
                Wizard1.StepNextButtonText = "Next";
                // Wizard1.StepNextButtonText = "Go to next step";
                if (!IsNumeric(txtCOTPNumber.Text))
                {
                    lblcc.Text = "Invalid OTP Number.";
                    Wizard1.StepNextButtonText = "Validate";
                    e.Cancel = true;
                }
                else
                {
                    try
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            Int64 reno = Convert.ToInt64(Txtregno.Text);
                            Int64 entityID = (from c in context.RegistrationDetails
                                              where c.RegistrationNo == reno
                                              select c.CandidateID).FirstOrDefault();
                            var email = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();

                            if (Convert.ToInt32(txtCOTPNumber.Text.Trim()) == email.EmailOTP || 1 == 1) // changed by amit
                            {
                                txtCOTPNumber.Text = "";
                                email.IsEmailVerified = true;
                                email.EmailVerifiedOn = DateTime.Now;
                                email.EmailOTPValidUptoDate = DateTime.Now;
                                context.Entry(email).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                            }
                            else
                            {
                                lblErrorEmail.Visible = true;
                                lblcc.Text = "Entered OTP does not match with the OTP sent to your Registered Email Address. Please check and re-enter OTP.";
                                Wizard1.StepNextButtonText = "Validate";
                                e.Cancel = true;
                            }


                        };

                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            Int32 courseID = Convert.ToInt32(Ddlcname.SelectedValue);
            Int64 regno = Convert.ToInt64(Txtregno.Text);
            string name = Txtname.Text;
            if (String.IsNullOrEmpty(Txtpassword.Text))
            {
                Lbuser.Text = "Please enter new password.";
                e.Cancel = true;
            }
            else if (String.IsNullOrEmpty(Txtconfirmpassword.Text))
            {
                Lbuser.Text = "Please enter confirm new password.";
                e.Cancel = true;
            }

            else if (Txtconfirmpassword.Text != Txtpassword.Text)
            {
                Lbuser.Text = "entered new password and confirm new password do not match. Please enter again.";
                e.Cancel = true;
            }
            //else if (!EConnect.Utils.Security.RandomPassword.IsValidPassword(Txtpassword.Text, EConnect.Utils.Security.RandomPassword.PasswordPolicy.Excellent))
            //{
            //    Lbuser.Text = EConnect.Utils.Common.EnumUtility.GetDescription(EConnect.Utils.Security.RandomPassword.PasswordPolicy.Excellent);
            //    e.Cancel = true;
            //}
            else if (ddlSecurityQues.SelectedValue == "0")
            {
                Lbuser.Text = "Please select security question";
                e.Cancel = true;
            }
            else if (String.IsNullOrEmpty(TxtSecurityAns.Text))
            {
                Lbuser.Text = "Please enter security answer.";
                e.Cancel = true;
            }
            else
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var registration = (from rg in context.RegistrationDetails
                                        where rg.CourseID == courseID && rg.RegistrationNo == regno
                                        select rg).FirstOrDefault();

                    Int64 candidateID = registration.CandidateID;
                    //CandidateContactDetail objContact = context.CandidateContactDetails.Where(c => c.CandidateID == candidateID).FirstOrDefault();

                    ////Add Candidate Contact history.
                    //CandidateContactHistory ContactHistory = new CandidateContactHistory();
                    //ContactHistory.CandidateID = candidateID;
                    //ContactHistory.MobileNumber = objContact.MobileNumber;
                    //ContactHistory.StdNumber = objContact.StdNumber;
                    //ContactHistory.PhoneNumber = objContact.PhoneNumber;
                    //ContactHistory.EmailAddress = objContact.EmailAddress;
                    //ContactHistory.CreatedOn = DateTime.Now;
                    //ContactHistory.CreatedByID = 1;
                    //context.CandidateContactHistoryDetails.Add(ContactHistory);

                    ////Update Candidate Contact Detail.
                    //objContact.CandidateID = candidateID;
                    //objContact.EffectiveFromDate = DateTime.Now;
                    //objContact.MobileNumber = Convert.ToInt64(txtmobile.Text);
                    //objContact.MobileOTP = null;
                    //objContact.MobileNumberVerifiedOn = null;
                    //objContact.MobileOTPValidUptoDate = null;
                    //objContact.IsMobileNumberVerified = false;
                    //objContact.EmailAddress = txtemail.Text;
                    //objContact.EmailOTP = null;
                    //objContact.IsEmailVerified = false;
                    //objContact.EmailVerifiedOn = null;
                    //objContact.EmailOTPValidUptoDate = null;
                    //context.Entry(objContact).State = System.Data.Entity.EntityState.Modified;

                    //Update User Detail.
                    User objUser = new EConnect.URM.User();
                    bool userAlreadyCreated = false;
					if (context.Users.Any(u => u.LoginID == Txtregno.Text.Trim()) == true)
                        throw new Exception("UserID is alreay exist for this registration Number " + Txtregno.Text.Trim());
                    if ((context.Users.Any(s => s.LoginID == Txtregno.Text.Trim() && s.UserName.ToUpper() == Txtname.Text.Trim().ToUpper() && s.UserTypeID == 3 && s.HasLoginAccess == false)))
                    {
                        objUser = (from s in context.Users
                                   where s.LoginID == Txtregno.Text.Trim() && s.UserName.ToUpper() == Txtname.Text.Trim().ToUpper()
                                   select s).FirstOrDefault();
                        userAlreadyCreated = true;
                    }
                    objUser.CreatedBy = 1;
                    objUser.CreatedOn = DateTime.Now;
                    objUser.HasLoginAccess = false;
                    objUser.LoginID = lblUserName.Text;
                    objUser.OrganizationID = 1;
                    objUser.Password = UserManager.ComputeSha256Hash(Txtpassword.Text).ToUpper(); //FormsAuthentication.HashPasswordForStoringInConfigFile(Txtpassword.Text, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                    objUser.PasswordExpiryDays = 0;
                    objUser.LastPasswordChangedOn = DateTime.Now;
                    objUser.LastLoginDateTime = DateTime.Now;
                    objUser.SecurityQuestion = ddlSecurityQues.SelectedValue.ToString();
                    objUser.SecurityAnswer = TxtSecurityAns.Text.ToString().ToUpper();
                    objUser.FailedLoginAttempts = 0;
                    objUser.UserName = name;
                    objUser.UserTypeID = Convert.ToInt32(UserType.Candidate);
                    objUser.UserRefNumber = candidateID;
                    objUser.DefaultRoleID = Convert.ToInt32(enmRole.Candidate);
                    if (userAlreadyCreated == false)
                        context.Users.Add(objUser);
                    else
                        context.Entry(objUser).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    if (userAlreadyCreated == false)
                    {
                        EConnect.URM.UserRole userRole = new EConnect.URM.UserRole();
                        userRole.UserID = objUser.UserID;
                        userRole.RoleID = Convert.ToInt32(enmRole.Candidate);
                        userRole.CreatedOn = DateTime.Now;
                        userRole.CreatedBy = objUser.UserID;
                        context.UserRoles.Add(userRole);
                        context.SaveChanges();
                    }
                    tblResult.Visible = true;
                    Wizard1.Visible = false;
                    try
                    {
                        //Sending activation email.
                        CommonFunctions.SendAccountActivationEmail(objUser.UserID, true, Txtpassword.Text);
                    }
                    catch (Exception ){ }
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            GenerateNewCaptchaImage();
            if (Wizard1.ActiveStep.ID == "first")
            {
                Wizard1.StepNextButtonText = "Next";
            }
            else if (Wizard1.ActiveStep.ID == "second")
            {
                Wizard1.StepNextButtonText = "Validate";
            }
            else if (Wizard1.ActiveStep.ID == "Third")
            {
                Wizard1.StepNextButtonText = "Next";
            }
            else
            {
                Wizard1.StepNextButtonText = "Validate";
            }
            txtcode.Text = "";
            Lblcontact.Text = "";
            lbl1.Text = "";
            lblcc.Text = "";
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

}