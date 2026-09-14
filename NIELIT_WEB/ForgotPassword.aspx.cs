using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class ForgotPassword : BasePage
{
   protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";// FormsAuthentication.HashPasswordForStoringInConfigFile("nitesh", System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "function window.onload(){IsValidated();}", true);
        try
        {
            if (!Page.IsPostBack)
            {
                Session.Abandon();
                if (!String.IsNullOrEmpty(Request.QueryString["Uname"]))
                    Txtname.Text = Request.QueryString["Uname"];
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Forgot Password", "", ""));
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
   protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        BreadCrumb1.Render();
        if (Wizard1.ActiveStep.ID == "first")
        {
            lbl1.Text = "";
            if (String.IsNullOrEmpty(Txtname.Text.Trim()))
            {
                lbl1.Text = "Please enter your user name. It can't be left blank.";
                e.Cancel = true;
            }
            else
            {
                lblname.Text = Txtname.Text.ToUpper();
                using (EConnectContext context = new EConnectContext())
                {
                    User currentUser = UserManager.GetUserByLoginID(Txtname.Text.Trim(), int.Parse(ddlUserType.SelectedValue), context, 1);
                    if (currentUser == null)
                    {
                        lbl1.Text = "Invalid User Name. Please enter a valid user name provided to you.";
                        e.Cancel = true;
                    }
                };
                
            }
        }
        else if (Wizard1.ActiveStep.ID == "second")
        {
            Label2.Text = "";
            lblname1.Text = Txtname.Text.ToUpper();
            lblemail.Text = txtemail.Text;

            if (String.IsNullOrEmpty(txtemail.Text.Trim()))
            {
                Label2.Text = "Please enter your registered email id. It can't be left blank.";
                e.Cancel = true;
            }
            else
            {
                string emailAddress = "";
                string email = txtemail.Text.Trim().ToUpper();
                using (EConnectContext context = new EConnectContext())
                {
                    User currentUser = UserManager.GetUserByLoginID(Txtname.Text.Trim(), int.Parse(ddlUserType.SelectedValue), context, 1);
                    if (currentUser.enmUserType == EConnect.URM.UserType.Candidate)
                    {
                        emailAddress = (from c in context.CandidateContactDetails
                                        where c.CandidateID == currentUser.UserRefNumber
                                        select new
                                        {
                                            email = c.EmailAddress
                                        }).FirstOrDefault().email;
                       
                    }
                    else if (currentUser.enmUserType == EConnect.URM.UserType.Admin || currentUser.enmUserType == EConnect.URM.UserType.ExternalAdmin || currentUser.enmUserType == EConnect.URM.UserType.HeadOffice)
                    {
                        emailAddress = (from c in context.ExternalEntities
                                        where c.ID == currentUser.UserRefNumber
                                        select new
                                        {
                                            email = c.Email
                                        }).FirstOrDefault().email;
                    }
                    else if (currentUser.enmUserType == EConnect.URM.UserType.Institute)
                    {
                        emailAddress = (from c in context.Institutes
                                        where c.ID == currentUser.UserRefNumber
                                        select new
                                        {
                                            email = c.EmailAddress1
                                        }).FirstOrDefault().email;
                    }
                    else if (currentUser.enmUserType == EConnect.URM.UserType.RegionalCenter)
                    {
                        emailAddress = (from c in context.RegionalCenters
                                        where c.ID == currentUser.UserRefNumber
                                        select new
                                        {
                                            email = c.RegisteredEmailAddress
                                        }).FirstOrDefault().email;
                    }
					
					 //Added 26 Jun 2020
                    else if (currentUser.enmUserType == EConnect.URM.UserType.projectNIELITCentre)
                    {
                        emailAddress = (from c in context.Users 
                                        where c.UserID   == currentUser.UserID
                                        select new
                                        {
                                            email = c.EmailID
                                        }).FirstOrDefault().email;
                    }
                    //
                    if (emailAddress.ToUpper() != txtemail.Text.ToUpper())
                    {
                        Label2.Text = "Invalid E-mail Id. Please enter a valid email id.";
                        e.Cancel = true;
                    }
                    else
                    {
                        txtques.Text = currentUser.SecurityQuestion;
                    }
                };
            }
        }
   }
   protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
   {
       BreadCrumb1.Render();
       Label2.Text = "";
       Label3.Text = "";
       if (String.IsNullOrEmpty(txtsec.Text.Trim()))
       {
           Label3.Text = "Please enter your security answer. It can't be left blank.";
           e.Cancel = true;
       }
       else
       {
           using (EConnectContext context = new EConnectContext())
           {
               User currentUser = UserManager.GetUserByLoginID(Txtname.Text.Trim(), int.Parse(ddlUserType.SelectedValue), context, 1);
               if (currentUser.SecurityAnswer.ToUpper() != txtsec.Text.ToUpper())
               {
                   Label3.Text = "Invalid security answer. Please enter correct security answer.";
                   e.Cancel = true;
               }
               else
               {
                   //Send a email link to the registered email address of user.
                   tblResult.Visible = true;
                   Wizard1.Visible = false;
                   Session["UserID"] = currentUser.UserID.ToString();
                   string password = "";
                   password = EConnect.Utils.Security.RandomPassword.Generate();
                   currentUser.FailedLoginAttempts = 0;
                   currentUser.Password = UserManager.ComputeSha256Hash(password).ToUpper(); // FormsAuthentication.HashPasswordForStoringInConfigFile(password, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                   currentUser.LastPasswordChangedOn = DateTime.Now;
                   context.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;
                   context.SaveChanges();
                   CommonFunctions.ForgotPasswordMail(currentUser.UserID, password);
               }
           };
       }
   }
   protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
   {
       BreadCrumb1.Render();
       lblError.Text = "";
       Label2.Text = "";
       Label3.Text = "";
   }
   protected void lnkresend_Click(object sender, EventArgs e)
   {
       try
       {
           lblError.Text = "";
           if (Session["UserID"] != null)
           {
               Int32 UserID = Convert.ToInt32(Session["UserID"]);
               Int32 userTypeId = Convert.ToInt32(Session["UserTypeId"]);
               string password = "";
               password = EConnect.Utils.Security.RandomPassword.Generate();
               using (EConnectContext context = new EConnectContext())
               {
                   User currentUser = UserManager.GetUserByUserID(UserID, context);
                   currentUser.Password = UserManager.ComputeSha256Hash(password).ToUpper();  //FormsAuthentication.HashPasswordForStoringInConfigFile(password, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                   context.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;
                   context.SaveChanges();
                   CommonFunctions.ForgotPasswordMail(Convert.ToInt32(Session["UserID"]), password);
                   Session["UserID"] = null;
               };
           }
       }
       catch (Exception ex)
       {
           ShowAlert(ex.Message);
       }
   }
   protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
   {
       if (ddlUserType.SelectedIndex != 0)
       {
           Txtname.Enabled = true;           
       }
       else
       {
           Txtname.Enabled = false;           
       }
   }
}