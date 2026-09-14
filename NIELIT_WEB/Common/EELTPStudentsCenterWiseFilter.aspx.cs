using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.Objects;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Security.Cryptography;



public partial class Common_EELTPStudentsCenterWiseFilter : BasePage
{
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0; 
    Int32 UserTypeId = 0;
    UserType loginUserType;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
           
            loginUserType = (UserType)Session["UserType"];
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
           
            if (!IsPostBack)
            {
                FillProjectName();
				FillCentre();
                ddlProjectName.SelectedValue = "10020";
                ddlProjectName.Enabled = false;
               // BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Report Details Of Students  ProjectWise CenterWise Filter", "#", ""));

            }
            if (UserTypeId.ToString() == "28" || UserTypeId.ToString() == "10" || UserTypeId .ToString()=="6")
            {
                ddlProjectName.SelectedValue = "10020";
                ddlProjectName.Enabled = false;
                 //MISHO or NIELIT Jammu can see all centres from drop down
                 currentRoleId = Convert.ToInt32(Session["RoleID"]);
                if (currentRoleId.ToString() == "40")
                {
                    ddlCentre.SelectedValue = "0";
                    ddlCentre.Enabled = true;
                }
                else
                { //Other Centres can see data of their centre and its TP
                    User objUser;
                    using (EConnectContext context = new EConnectContext())
                    {
                        objUser = new EConnect.URM.User();

                        User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {

                            if (UserTypeId == 10)
                            {
                                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                                ddlCentre.SelectedValue = NielitCentreId.ToString();
                            }
                        }


                    }
                    ddlCentre.Enabled = false;
                }
            }
            else
            {
                ShowAlert("View for the page is not available, Contact portal administrator");
                return;
            }
           
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
 
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlProjectName.SelectedValue = "26";
            ddlProjectName_SelectedIndexChanged(sender, e);
            ddlCentre.SelectedValue = "0";
            txtDateto.Text = "";
            txttDateFrom.Text = "";
            ddlStatus.SelectedValue = "0";
            // BreadCrumb1.Render();          

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime datefromC, datetoC;
            datefromC = Convert.ToDateTime(txttDateFrom.Text.Trim());
            datetoC = Convert.ToDateTime(txtDateto.Text.Trim());
            if (datefromC < datetoC)
            {

                string startDate = HttpUtility.UrlEncode(Encrypt(txttDateFrom.Text.Trim()));
                string endDate = HttpUtility.UrlEncode(Encrypt(txtDateto.Text.Trim()));
                string ProjID = HttpUtility.UrlEncode(Encrypt(ddlProjectName.SelectedItem.Value));
                string centreID = HttpUtility.UrlEncode(Encrypt(ddlCentre.SelectedItem.Value));
                string status = HttpUtility.UrlEncode(Encrypt(ddlStatus.SelectedItem.Value));

                Response.Redirect(string.Format("../HO/Rpt/EELTPStudentsCenterWise.aspx?startDate={0}&endDate={1}&ProjID={2}&centreID={3}&status={4}", startDate, endDate, ProjID, centreID, status));
               
               
            }
            else
            {
                ShowAlert("DateFrom should not be greater than DateTo.", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    protected void FillProjectName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--ALL--", "0");
                var ProjectList = from p in context.NielitProjectss
                                  orderby (p.ProjectName)
                                  select new { ValueField = p.ID, TextField = p.ProjectName };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectName, ProjectList, lst);

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCentre()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--ALL--", "0");

                var CourseList = from p in context.NielitCentres
                                 join q in context.projectMainCentres 
                                 on p.ID equals q.centreID 
                                 where q.projectID ==10020
                                 orderby p.Name
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentre, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCentre.Items.Clear();
        FillCentre();
        ddlStatus.SelectedValue = "99";
        
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddlCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStatus.SelectedValue = "99";
       
    }
    protected void txtDateto_TextChanged(object sender, EventArgs e)
    {
        DateTime DateFrom = Convert.ToDateTime(txttDateFrom.Text);
        DateTime DateTo = Convert.ToDateTime(txtDateto.Text);
        DateFrom = DateFrom.AddDays(-1);
        DateTime EndDate = DateFrom.AddYears(1);
        if (DateTo > EndDate)
        {
            ShowAlert("DateFrom and DateTo range should be maximum 1 year", true);
            return;
        }
    }
}