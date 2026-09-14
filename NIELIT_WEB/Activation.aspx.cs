using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Activation : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                //Response.Write(Request.QueryString.ToString());
                //Response.End();
                //string PagePath = "Activation.aspx?p1=" + 10 + "&p2=" + "kuldeep" + "&p3=" + 34;
               // Response.Write(Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf('/')) + "/" + PagePath);
                if (!String.IsNullOrEmpty(Request.QueryString["p1"]) && !String.IsNullOrEmpty(Request.QueryString["p2"]) && !String.IsNullOrEmpty(Request.QueryString["p3"]))
                {
                    Int32 UserID = Convert.ToInt32(Request.QueryString["p1"]);
                    string LoginID = Convert.ToString(Request.QueryString["p2"]);
                    Int64 CandidateID = Convert.ToInt64(Request.QueryString["p3"]);

                    using (EConnectContext context = new EConnectContext())
                    {
                        if (context.Users.Any(s => s.UserID == UserID && s.LoginID == LoginID && s.UserRefNumber == CandidateID))
                        {

                            User objUser = context.Users.Where(c => c.UserID == UserID && c.LoginID == LoginID && c.UserRefNumber == CandidateID).FirstOrDefault();
                            if (objUser.HasLoginAccess == true)
                            {
                                lblError.Text = "Your Account Is Already Activated.";
                            }
                            else
                            {
                                objUser.HasLoginAccess = true;

                                context.Entry(objUser).State = System.Data.Entity.EntityState.Modified;

                                if (objUser.UserType.ID  == Convert.ToInt32(EConnect.URM.UserType.Candidate))
                                {
                                    CandidateContactDetail objContact = context.CandidateContactDetails.Where(c => c.CandidateID == CandidateID).FirstOrDefault();
                                    objContact.IsEmailVerified = true;
                                    objContact.EmailVerifiedOn = DateTime.Now;
                                    context.Entry(objContact).State = System.Data.Entity.EntityState.Modified;
                                }

                                context.SaveChanges();
                                lblError.Text = "Dear " + GetInitCap(objUser.UserName) + ",<br/><br/>Your account has been activated successfully.<br/> Now you can login from" + "<a href='Index.aspx'> login page</a>";
                            }
                        }
                    }

                }
                else
                {
                    lblError.Text = GeInvalidRequestMessage("Go to login page", "index.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { }
    }
}