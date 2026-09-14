using System;
using System.Linq;
using System.Web.Security;
using EConnect.DAL;
using EConnect.URM;

public partial class Admin_AdminChangePasswd : BasePage
{
    UserType loginUserType;
    int userTypeId;    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../index.aspx");
            try
            {
                loginUserType = (UserType)Session["UserType"];
                userTypeId = (int)Session["UserTypeId"];                 
            }
            catch (Exception){}
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["src"].ToString()))
                {
                    string src = Request.QueryString["src"].ToString();
                    HfSrc.Value = src;
                    using (var context = new EConnectContext())
                    {
                        User objUser = UserManager.GetUserByUserID(Convert.ToInt32(Session["UserID"]), context);
                        TxtUserName.Text = objUser.LoginID.ToUpper();
                        TxtUserName.ReadOnly = true;
                        LblWelcome.Text = "Dear " + objUser.LoginID.ToLower() + ",";
                        switch (src)
                        {
                            case "login":
                                div1.Visible = false;
                                div2.Visible = true;
                                LblHead.Text = "Change password on : First Login";
                                TrQues.Visible = true;
                                TrAns.Visible = true;
                                LblTitle.Visible = true;
                                LblTitle.Text = "Hi ! " + objUser.LoginID.ToLower() + "  please change your password on first login for security purpose";
                                btnClose.Text = "Cancel";
                                LblMsgHead.Text = "Password change process successfully completed on first login. ";
                                break;
                            case "profile":
                                div1.Visible = false;
                                div2.Visible = true;
                                LblHead.Text = "Change Password";
                                TrQues.Visible = false;
                                TrAns.Visible = false;
                                LblTitle.Visible = false;
                                btnClose.Text = "Close";
                                btnClose.Attributes.Add("onclick", "window.close();return false;");
                                break;
                            case "expire":
                                div1.Visible = false;
                                div2.Visible = true;
                                LblHead.Text = "Change password on : Password Expiration";
                                TrQues.Visible = false;
                                TrAns.Visible = false;
                                LblTitle.Visible = true;
                                LblTitle.Text = "Hi ! " + objUser.LoginID.ToLower() + "Your password is expired. Please change your password.";
                                btnClose.Text = "Cancel";
                                LblMsgHead.Text = "Password change process successfully completed on password expiration. ";
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            Response.Write("Invalid Request Parameters");
            Response.End();
        }
    }
    new public bool IsValid()
    {
        string src = Request.QueryString["src"].ToString();
        if (Txt_OldPsswd.Text == "")
        {
            lblMsg.Text = " Current Password should not be blank";
            return false;
        }
        if (Txt_NewPsswd.Text == "")
        {
            lblMsg.Text = " New Password should not be blank";
            return false;
        }
        if (Txt_OldPsswd.Text.Trim() == Txt_NewPsswd.Text.Trim())
        {
            lblMsg.Text = "Current Password and New Password should not be same";
            return false;
        }
        if (Txt_ConfirmPsswd.Text == "")
        {
            lblMsg.Text = "Confirm New Password should not be blank";
            return false;
        }
        //if (!EConnect.Utils.Security.RandomPassword.IsValidPassword(Txt_NewPsswd.Text, EConnect.Utils.Security.RandomPassword.PasswordPolicy.Excellent))
        //{
        //    lblMsg.Text = EConnect.Utils.Common.EnumUtility.GetDescription(EConnect.Utils.Security.RandomPassword.PasswordPolicy.Excellent);
        //    return false;
        //}
        if (src == "login")
        {

            if (ddlSecurityQues.SelectedValue == "0")
            {
                lblMsg.Text = "should select Security Question";
                return false;

            }
            if (TxtSecurityAns.Text == "")
            {
                lblMsg.Text = "Security answer should not be blank";
                return false;

            }
        }
        return true;
    }
    protected void btnChangePsswd_Click(object sender, EventArgs e)
    {
        try
        {
            string src = Request.QueryString["src"].ToString();

            if (IsValid())
            {
                Int32 orgId = 1;
                string pwd = "";
                
                pwd = tuntCurr.Value; //UserManager.ComputeSha256Hash(Txt_OldPsswd.Text).ToUpper(); //FormsAuthentication.HashPasswordForStoringInConfigFile(Txt_OldPsswd.Text, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                string Newpwd = tuntNew.Value; //UserManager.ComputeSha256Hash(Txt_NewPsswd.Text).ToUpper();  //FormsAuthentication.HashPasswordForStoringInConfigFile(Txt_NewPsswd.Text, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                string Confirmpwd = tuntConfirm.Value; //UserManager.ComputeSha256Hash(Txt_ConfirmPsswd.Text).ToUpper();  //FormsAuthentication.HashPasswordForStoringInConfigFile(Txt_ConfirmPsswd.Text, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());

                using (var context = new EConnectContext())
                {                   
                    var userQuery = from u in context.Users
                                    where u.LoginID.ToUpper() == TxtUserName.Text.Trim().ToUpper() &&
                                    u.OrganizationID == orgId
                                    select u;
                    if (userTypeId == 3 || userTypeId == 4)
                    {
                        userQuery = from u in context.Users
                                    where u.LoginID.ToUpper() == TxtUserName.Text.Trim().ToUpper() && u.UserTypeID == userTypeId &&
                                    u.OrganizationID == orgId
                                    select u;
                    }
                    if (userQuery.Count() > 0)
                    {

                        User loginUser = userQuery.Single();
                        // Utility to movedd all users frm MD5 to SHA256 Encryption
                        if (loginUser.Password.Length == 32)
                        {
                            pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(MD5Curr.Value, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                        }

                        if (loginUser.Password == pwd.ToUpper())
                        {

                            if (Newpwd == Confirmpwd)
                            {
                                loginUser.LastLoginDateTime = DateTime.Now;
                                loginUser.Password = Newpwd;
                                loginUser.FailedLoginAttempts = 0;
                                loginUser.LastPasswordChangedOn = DateTime.Now;
                                if (src == "login")
                                {
                                    loginUser.SecurityQuestion = ddlSecurityQues.SelectedItem.Text;
                                    loginUser.SecurityAnswer = TxtSecurityAns.Text.ToUpper().Trim();
                                }

                                context.Entry(loginUser).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                if (src == "login" || src == "expire")
                                {
                                    div1.Visible = true;
                                    div2.Visible = false;
                                }
                                else
                                    lblMsg.Text = "Password has been successfully changed";
                            }
                            else
                            {
                                lblMsg.Text = "New Password and confirm Password should be same..";
                                if (src == "login" || src == "expire")
                                {
                                    div1.Visible = false;
                                    div2.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Invalid Current Password..";
                            if (src == "login" || src == "expire")
                            {
                                div1.Visible = false;
                                div2.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnClose.Text.ToLower() == "cancel")
            {
                Session.Abandon();
                Response.Redirect("../Home.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}