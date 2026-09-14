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



public partial class Admin_MisReportProjectWiseCenterWiseFilter : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;   
    Int32 currentRoleId = 0;

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
            entityID = Convert.ToInt64(Session["EntityID"]);

           
            if (!IsPostBack)
            {
                FillProjectName();
                FillCentre(0);
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Report Details Of Students  ProjectWise CenterWise Filter", "#", ""));
                BreadCrumb1.Render();
            }
			 if (Session["RoleID"].ToString() == "28")
            {
                ddlProjectName.SelectedValue = "2";
                ddlProjectName.Enabled = false;
            }
            else
            {
               // ddlProjectName.SelectedValue = "0";
                ddlProjectName.Enabled = true;
            }


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
			ddlProjectName.SelectedValue ="0";
			ddlCentre.SelectedValue ="0";
			txtDateto.Text="";
			txttDateFrom.Text="";
			ddlStatus.SelectedValue="0";
          //  BreadCrumb1.Render();          

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

                Response.Redirect(string.Format("../HO/Rpt/MisReportProjectWiseCenterWise.aspx?startDate={0}&endDate={1}&ProjID={2}&centreID={3}&status={4}", startDate, endDate, ProjID, centreID, status));
               
               
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

    protected void FillCentre(Int64 projectID)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--ALL--", "0");

            
                
                var centrename = from p in context.NielitCentres
                                 join pmc in context.projectMainCentres  on p.ID equals pmc.centreID
                                 where pmc.projectID == projectID || projectID == 0
                                 orderby p.Name
                                 select new { ValueField = p.ID, TextField = p.Name };


                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentre, centrename, lst);
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
        
        FillCentre(Convert.ToInt64(ddlProjectName.SelectedValue));
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