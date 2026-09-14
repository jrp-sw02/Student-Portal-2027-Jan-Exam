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



public partial class Admin_StatisticsReportFilter : BasePage
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

            if (Session["RoleID"].ToString() == "28")
            {
                ddlProjectName.SelectedValue = "2";
                ddlProjectName.Enabled = false;
            }
            else
            {
                ddlProjectName.SelectedValue = "0";
                ddlProjectName.Enabled = true;
            }
            //ddlProjectName.SelectedValue = "2";
            //ddlProjectName.Enabled = false;
            if (!IsPostBack)
            {
                FillProjectName();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Statistics Report", "#", ""));

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
            BreadCrumb1.Render();          

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

                string DateFrom = HttpUtility.UrlEncode(Encrypt(txttDateFrom.Text.Trim()));
                string DateTo = HttpUtility.UrlEncode(Encrypt(txtDateto.Text.Trim()));
                string projectID = HttpUtility.UrlEncode(Encrypt(ddlProjectName.SelectedItem.Value));
                string projectIDD = ddlProjectName.SelectedItem.Value;
                if (projectIDD == "0")
                {
                    Response.Redirect(string.Format("../HO/Rpt/StatisticsReport.aspx?DateFrom={0}&DateTo={1}", DateFrom, DateTo));
                }
                else
                {
                    Response.Redirect(string.Format("../HO/Rpt/StatisticsReportProjectWise.aspx?DateFrom={0}&DateTo={1}&projectID={2}", DateFrom, DateTo, projectID));
                }
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

    protected void FillProjectName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
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
}