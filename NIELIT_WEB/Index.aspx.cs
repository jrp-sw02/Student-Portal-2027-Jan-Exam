using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.URM;

public partial class Index : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
            Response.Redirect("Mainpage.aspx");

        try
        {
            if (!Page.IsPostBack)
            {                   
                Session["OrgID"] = "1";
                showheader();
             
                //Request.QueryString["p1"] = UserID (Send always 0)
                //Request.QueryString["p2"] = InstituteCode   (2/3/4/5)
                //Request.QueryString["p3"] = LoginID  (abc/syz)
                //Request.QueryString["p4"] = Date (Current Date e.g. 08-Apr-2013)
                //http://115.249.179.239:815/Index.aspx/Index.aspx?p1=0&p2=4193&p3=TRMXAkc&p4=18-Feb-2013
            
                if (!string.IsNullOrEmpty(Request.QueryString["p1"]) && !string.IsNullOrEmpty(Request.QueryString["p2"]) && !string.IsNullOrEmpty(Request.QueryString["p3"]) && !string.IsNullOrEmpty(Request.QueryString["p4"]))
                {
                    DateTime? qryDate = null;
                    Int32 UserID = 0;
                    Int64 InstituteID = 0;
                    String UserName = "";

                    if (IsDate(Request.QueryString["p4"]))
                    {
                        qryDate = Convert.ToDateTime(Request.QueryString["p4"]);
                        if (qryDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                        {
                            if (IsNumeric(Request.QueryString["p1"]))
                            {
                                UserID = Convert.ToInt32(Request.QueryString["p1"]);
                                if (IsNumeric(Request.QueryString["p2"]))
                                {
                                    InstituteID = Convert.ToInt64(Request.QueryString["p2"]);
                                    UserName = Convert.ToString(Request.QueryString["p3"]);
                                    using (EConnectContext context = new EConnectContext())
                                    {
                                        int UserType = Convert.ToInt16(EConnect.URM.UserType.Institute);
                                        if (context.Users.Any(s => s.UserTypeID == UserType && s.UserRefNumber == InstituteID && s.LoginID.ToUpper() == UserName.ToUpper()))
                                        {
                                            User loginUser = context.Users.Where(s => s.UserTypeID == UserType && s.UserRefNumber == InstituteID && s.LoginID.ToUpper() == UserName.ToUpper()).FirstOrDefault();
                                            if (loginUser != null)
                                            {
                                                UserManager.LogLoginDetails(loginUser.LoginID, loginUser.UserTypeID, EConnect.URM.AuthenticationResponse.Successfull, context, 1);
                                                context.SaveChanges();
                                                Session["OrgId"] = loginUser.OrganizationID.ToString();
                                                //Session["loginUser"] = loginUser;
                                                Session["UserID"] = loginUser.UserID.ToString();
                                                Session["UserName"] = loginUser.UserName.ToString();
                                                Session["LogID"] = UserManager.GetCerrentLogID(loginUser.LoginID, context, 1);
                                                Session["ModuleId"] = loginUser.UserType.ModuleID.ToString();
                                                Session["RoleID"] = loginUser.DefaultRoleID;
                                                Session["ProjectID"] = "2";
                                                Session["UserType"] = loginUser.enmUserType;
                                                Session["EntityID"] = loginUser.UserRefNumber;

                                                Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
                                                LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];
                                                Application.Lock();

                                                if (LoginUsersList.ContainsKey(loginUser.UserID))
                                                {
                                                    LoginUsersList.Remove(loginUser.UserID);
                                                    this.ShowAlert("Duplicate login not allowed. You have been logged out from another location");
                                                }
                                                LoginUsersList.Add(loginUser.UserID, Convert.ToInt32(Session["LogID"]));
                                                Application["LoginUsersList"] = LoginUsersList;
                                                Application.UnLock();
                                                Response.Redirect("Mainpage.aspx");
                                            }
                                        }
                                    };
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showheader()
    {
        try
        {
            using (EConnectContext ctx = new EConnectContext())
            {
                Int32 orgID = Convert.ToInt32(Session["OrgID"]);
                var users1 = (from u in ctx.Organizations
                              where u.ID == orgID
                              select u).Single();
                tdHeaderBig.InnerText = users1.Name;
                string m = users1.MainHeading;
                string s = users1.SubHeading;
                tdHeaderSmall.InnerText = string.Concat(m, s);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}