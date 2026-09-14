using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_InstituteDetails : BasePage
{
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/frmCourseRegistrationApplication.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                Bind();
            }
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Institute Detail", "", ""));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void Bind()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                StringBuilder accrstr = new StringBuilder();
                Int32 InstituteId = Convert.ToInt32(Request.QueryString["InstituteId"]);

                var institute = context.Institutes.Find(InstituteId);

                lblName.Text          = GetInitCap(institute.Name);
                lblAddress.Text       = GetInitCap((institute.AddressLine1 + ", " + institute.AddressLine2 + ", " +
                                        institute.AddressLine3 + ", " + institute.CityName + ", " + institute.State.Name + ", " + institute.PinCode.ToString())).Replace(",,", ",");
                lblEmail.Text         = (institute.EmailAddress1 + ",   " + institute.EmailAddress2).ToLower();
                lblMobile.Text        = institute.MobileNumber.ToString();
                lblLandline.Text      = "STD.No. - "+(!string.IsNullOrEmpty(institute.StdNumber.ToString()) ? institute.StdNumber.ToString() : "N/A") +",   "+ 
                                         (!string.IsNullOrEmpty(institute.PhoneNumber1.ToString()) ? institute.PhoneNumber1.ToString() : "N/A") + ",   " +
                                         (!string.IsNullOrEmpty(institute.PhoneNumber2.ToString()) ? institute.PhoneNumber2.ToString() : "N/A") ;
                LblContactPerson.Text = GetInitCap("Name - " + (!string.IsNullOrEmpty(institute.ContactPersonName) ? institute.ContactPersonName : "N/A") +
                                        "<br/>Designation - " + (!string.IsNullOrEmpty(institute.ContactPersonPost) ? institute.ContactPersonPost : "N/A"));
                LblFax.Text           = (!string.IsNullOrEmpty(institute.FaxNumber.ToString()) ? institute.FaxNumber.ToString() : "N/A");

                var Accreditation = context.AccreditationDetails.Where(p => p.InstituteID == InstituteId);
                foreach (var accr in Accreditation)
                {
                    accrstr.Append("<b>"+accr.Course.Code + "</b> - " + accr.AccreditationNumber +",  " +accr.AccreditationStatus.Code +",  "+
                                          ((accr.EffectiveFromDate != null) ? accr.EffectiveFromDate.Value.ToString("dd/MMM/yyyy") : "N/A") + " to " +
                                          ((accr.EffectiveToDate != null) ? accr.EffectiveToDate.Value.ToString("dd/MMM/yyyy") : "N/A") + "\n"); 
                }
                LblAccr.Text = accrstr.ToString().Replace("\n", "<Br />");

            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}