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
using System.Collections.Generic;
using System.Web.UI;
using System.Web;



public partial class Common_Projects_TPFilter : BasePage
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
              //  ddlProjectName.SelectedValue = "26";
              //  ddlProjectName.Enabled = false;
               // BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Report Details Of Students  ProjectWise CenterWise Filter", "#", ""));

            }
            //For headoffice, NIELIT Centres, NIELIT Jammu
            if (UserTypeId.ToString() == "28" || UserTypeId.ToString() == "10" || UserTypeId .ToString() =="6")
            {
                User objUser;
                string defaultRole;
                 using (EConnectContext context = new EConnectContext())
                 {
                     objUser = new EConnect.URM.User();

                     User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                      defaultRole = loginUser.DefaultRoleID.ToString();
                 }
                 if (defaultRole == "40")
                 {
                     ddlProjectName.SelectedValue = "10020";
                     ddlProjectName.Enabled = false;
                 }

                 //MISHO or NIELIT Jammu can see all centres from drop down
                if (UserTypeId.ToString() == "28")
                {
                    //ddlProjectName.SelectedValue = "10020";
                   // ddlProjectName.Enabled = false;
                    ddlCentre.SelectedValue = "0";
                    ddlCentre.Enabled = true;
                }
                else
                { //Other Centres can see data of their centre and its TP
                   
                    using (EConnectContext context = new EConnectContext())
                    {
                        objUser = new EConnect.URM.User();

                        User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                         defaultRole = loginUser.DefaultRoleID.ToString();

                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {

                            if (UserTypeId == 10)
                            {
                                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                                ddlCentre.SelectedValue = NielitCentreId.ToString();
				 ddlCentre.Enabled = false;
                            }
                        }


                    }
                   
                  
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
            ddlProjectName.SelectedValue = "10020";
            ddlProjectName_SelectedIndexChanged(sender, e);
            ddlCentre.SelectedValue = "0";
            txtDateto.Text = "";
            txttDateFrom.Text = "";
           // ddlStatus.SelectedValue = "0";
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
                

                Response.Redirect(string.Format("../HO/Rpt/Projects_TP.aspx?startDate={0}&endDate={1}&ProjID={2}&centreID={3}", startDate, endDate, ProjID, centreID));
               
               
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
                string projectID = ddlProjectName.SelectedValue.ToString();
                var CourseList = from p in context.NielitCentres
                                 join q in context.projectMainCentres 
                                 on p.ID equals q.centreID 
                                 where (q.projectID.ToString () ==projectID || "0"==projectID )
                                 orderby p.Name
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentre, CourseList.Distinct (), lst);
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
       
        
    }

    
    protected void txtDateto_TextChanged(object sender, EventArgs e)
    {
        DateTime DateFrom = Convert.ToDateTime(txttDateFrom.Text);
        DateTime DateTo = Convert.ToDateTime(txtDateto.Text);
        DateFrom = DateFrom.AddDays(-1);
        DateTime EndDate = DateFrom.AddYears(5);
        if (DateTo > EndDate)
        {
            ShowAlert("DateFrom and DateTo range should be maximum 5 years", true);
            return;
        }
    }
    protected void ddlCentre_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}