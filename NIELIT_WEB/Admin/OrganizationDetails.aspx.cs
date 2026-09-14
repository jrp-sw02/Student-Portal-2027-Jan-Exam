using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.HRMS;
public partial class Admin_OrganizationDetails : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write("Count=" + BreadCrumb1.Items.Count.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {

            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!Page.IsPostBack)
            {
               
                    BindState(); 
                    DataExist();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Organization Detail", "Admin/OrganizationDetails.aspx", ""));
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public void BindState()
    {
        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var state = from s in context.Locations
                        where s.LocationTypeID == 2 && s.ParentLocationID==1
                        select new { ValueField = s.ID, TextField = s.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, state, lst);

        };
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        Response.Redirect("OrganizationDetails.aspx", true);
        
    }
    protected void DataExist()
    {
        using(EConnectContext context=new EConnectContext())
        {
            var objOrg=(from p in context.Organizations
                       select p).FirstOrDefault();
            if(objOrg!=null)
            {
               
                    btnSave.Text = "Update";
                    hMainID.Value = objOrg.ID.ToString();
                    if (objOrg.Name != null)
                        txtOrgName.Text = objOrg.Name;
                    if (objOrg.MainHeading != null)
                        txtMainHeading.Text = objOrg.MainHeading;
                    if (objOrg.SubHeading != null)
                        txtSubHeading.Text = objOrg.SubHeading;
                    if (objOrg.AddressLine1 != null)
                        txtAddress1.Text = objOrg.AddressLine1;
                    if (objOrg.AddressLine2 != null)
                        txtAddress2.Text = objOrg.AddressLine2;
                    if (objOrg.StateID != null)
                        ddlState.SelectedValue = objOrg.StateID.ToString();
                    if (objOrg.CityName != null)
                        txtCityName.Text = objOrg.CityName;
                    if (objOrg.PinCode != null)
                        txtPinNumber.Text = objOrg.PinCode;
                    if (objOrg.PhoneNumber1 != null)
                        txtPhoneNumber1.Text = objOrg.PhoneNumber1;
                    if (objOrg.PhoneNumber2 != null)
                        txtPhoneNumber2.Text = objOrg.PhoneNumber2;
                    if (objOrg.PhoneNumber3 != null)
                        txtPhoneNumber3.Text = objOrg.PhoneNumber3;
                    if (objOrg.PhoneNumber4 != null)
                        txtPhoneNumber4.Text = objOrg.PhoneNumber4;
                    if (objOrg.FaxNumber != null)
                        txtFaxNumber.Text = objOrg.FaxNumber;
                    if (objOrg.EmailAddress != null)
                        txtEmail.Text = objOrg.EmailAddress;
                    if (objOrg.WebSite != null)
                        txtWebSite.Text = objOrg.WebSite;
                    if (objOrg.BankAccountName != null)
                        txtBankAcName.Text = objOrg.BankAccountName;
                    if (objOrg.BankBranchName != null)
                        txtBranchName.Text = objOrg.BankBranchName;
                    if(objOrg.EmailTechnicalPerson!=null)
                        txtTechEmail.Text = objOrg.EmailTechnicalPerson.ToString();
                    if (objOrg.MobileNumberTechnicalPerson!=null)
                        txtTechMobile.Text = objOrg.MobileNumberTechnicalPerson.ToString();
                    if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                    {
                        btnSave.Visible = false;
                    }
            }
            else
            {
                
                btnSave.Text = "Save";
            }
        };
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Organization objOrg;
                if (String.IsNullOrEmpty(hMainID.Value))
                {
                    objOrg = new EConnect.HRMS.Organization();
                    if (txtOrgName.Text.Trim() != "")
                        objOrg.Name = txtOrgName.Text.ToString();
                    else
                        objOrg.Name = null;
                    if (txtMainHeading.Text.Trim() != "")
                        objOrg.MainHeading = txtMainHeading.Text.ToString();
                    else
                        objOrg.MainHeading = null;
                    if (txtSubHeading.Text.Trim() != "")
                        objOrg.SubHeading = txtSubHeading.Text.ToString();
                    else
                        objOrg.SubHeading = null;
                    if (txtAddress1.Text.Trim() != "")
                        objOrg.AddressLine1 = txtAddress1.Text.ToString();
                    else
                        objOrg.AddressLine1 = null;
                    if (txtAddress2.Text.Trim() != "")
                        objOrg.AddressLine2 = txtAddress2.Text.ToString();
                    else
                        objOrg.AddressLine2 = null;
                    if (ddlState.SelectedValue != "0")
                        objOrg.StateID = Convert.ToInt64(ddlState.SelectedValue);
                    else
                        objOrg.StateID = 0;
                    if (txtCityName.Text.Trim() != "")
                        objOrg.CityName = txtCityName.Text.ToString();
                    else
                        objOrg.CityName = null;
                    if (txtPinNumber.Text.Trim() != "")
                        objOrg.PinCode = txtPinNumber.Text.ToString();
                    else
                        objOrg.PinCode = null;
                    if (txtPhoneNumber1.Text.Trim() != "")
                        objOrg.PhoneNumber1 = txtPhoneNumber1.Text.ToString();
                    else
                        objOrg.PhoneNumber1 = null;
                    if (txtPhoneNumber2.Text.Trim() != "")
                        objOrg.PhoneNumber2 = txtPhoneNumber2.Text.ToString();
                    else
                        objOrg.PhoneNumber2 = null;
                    if (txtPhoneNumber3.Text.Trim() != "")
                        objOrg.PhoneNumber3 = txtPhoneNumber3.Text.ToString();
                    else
                        objOrg.PhoneNumber3 = null;
                    if (txtPhoneNumber4.Text.Trim() != null)
                        objOrg.PhoneNumber4 = txtPhoneNumber4.Text.ToString();
                    else
                        objOrg.PhoneNumber4 = null;
                    if (txtFaxNumber.Text.Trim() != "")
                        objOrg.FaxNumber = txtFaxNumber.Text.ToString();
                    else
                        objOrg.FaxNumber = null;
                    if (txtEmail.Text.Trim() != "")
                        objOrg.EmailAddress = txtEmail.Text.ToString();
                    else
                        objOrg.EmailAddress = null;
                    if (txtWebSite.Text.Trim() != "")
                        objOrg.WebSite = txtWebSite.Text.ToString();
                    else
                        objOrg.WebSite = null;
                    if (txtBankAcName.Text.Trim() != null)
                        objOrg.BankAccountName = txtBankAcName.Text.ToString();
                    else
                        objOrg.BankAccountName = null;
                    if (txtBranchName.Text.Trim() != "")
                        objOrg.BankBranchName = txtBranchName.Text.ToString();
                    else
                        objOrg.BankBranchName = null;
                    if (txtTechEmail.Text.Trim() != "")
                        objOrg.EmailTechnicalPerson = txtTechEmail.Text.ToString();
                    else
                        objOrg.EmailTechnicalPerson = null;
                    if (txtTechMobile.Text.Trim() != "")
                        objOrg.MobileNumberTechnicalPerson = Convert.ToInt64(txtTechMobile.Text);
                    else
                        objOrg.MobileNumberTechnicalPerson = null;
                    context.Organizations.Add(objOrg);
                    context.SaveChanges();
                    strMessage = "New record saved.";
                }
                else
                {
                    objOrg = context.Organizations.Find(Convert.ToInt16(hMainID.Value));
                    if (txtOrgName.Text.Trim() != "")
                        objOrg.Name = txtOrgName.Text.ToString();
                    else
                        objOrg.Name =null;
                    if (txtMainHeading.Text.Trim() != "")
                        objOrg.MainHeading = txtMainHeading.Text.ToString();
                    else
                        objOrg.MainHeading = null;
                    if (txtSubHeading.Text.Trim() != "")
                        objOrg.SubHeading = txtSubHeading.Text.ToString();
                    else
                        objOrg.SubHeading = null;
                    if (txtAddress1.Text.Trim() != "")
                        objOrg.AddressLine1 = txtAddress1.Text.ToString();
                    else
                        objOrg.AddressLine1 = null;
                    if (txtAddress2.Text.Trim() != "")
                        objOrg.AddressLine2 = txtAddress2.Text.ToString();
                    else
                        objOrg.AddressLine2 = null;
                    if(ddlState.SelectedValue!="0")
                        objOrg.StateID = Convert.ToInt64(ddlState.SelectedValue);
                    else
                        objOrg.StateID = 0;
                    if (txtCityName.Text.Trim() != "")
                        objOrg.CityName = txtCityName.Text.ToString();
                    else
                        objOrg.CityName = null;
                    if (txtPinNumber.Text.Trim() != "")
                        objOrg.PinCode = txtPinNumber.Text.ToString();
                    else
                        objOrg.PinCode = null;
                    if (txtPhoneNumber1.Text.Trim() != "")
                        objOrg.PhoneNumber1 = txtPhoneNumber1.Text.ToString();
                    else
                        objOrg.PhoneNumber1 = null;
                    if (txtPhoneNumber2.Text.Trim() != "")
                        objOrg.PhoneNumber2 = txtPhoneNumber2.Text.ToString();
                    else
                        objOrg.PhoneNumber2 = null;
                    if (txtPhoneNumber3.Text.Trim() != "")
                        objOrg.PhoneNumber3 = txtPhoneNumber3.Text.ToString();
                    else
                        objOrg.PhoneNumber3 = null;
                    if (txtPhoneNumber4.Text.Trim() != null)
                        objOrg.PhoneNumber4 = txtPhoneNumber4.Text.ToString();
                    else
                        objOrg.PhoneNumber4 = null;
                    if (txtFaxNumber.Text.Trim() != "")
                        objOrg.FaxNumber = txtFaxNumber.Text.ToString();
                    else
                        objOrg.FaxNumber = null;
                    if (txtEmail.Text.Trim() != "")
                        objOrg.EmailAddress = txtEmail.Text.ToString();
                    else
                        objOrg.EmailAddress = null;
                    if (txtWebSite.Text.Trim() != "")
                        objOrg.WebSite = txtWebSite.Text.ToString();
                    else
                        objOrg.WebSite = null;
                    if (txtBankAcName.Text.Trim() != null)
                        objOrg.BankAccountName = txtBankAcName.Text.ToString();
                    else
                        objOrg.BankAccountName = null;
                    if (txtBranchName.Text.Trim() != "")
                        objOrg.BankBranchName = txtBranchName.Text.ToString();
                    else
                        objOrg.BankBranchName = null;
                    if (txtTechEmail.Text.Trim() != "")
                        objOrg.EmailTechnicalPerson = txtTechEmail.Text.ToString();
                    else
                        objOrg.EmailTechnicalPerson = null;
                    if (txtTechMobile.Text.Trim() != "")
                        objOrg.MobileNumberTechnicalPerson = Convert.ToInt64(txtTechMobile.Text);
                    else
                        objOrg.MobileNumberTechnicalPerson = null;
                    strMessage = "Record updated.";
                    context.SaveChanges();
                }
            };
            Response.Redirect("OrganizationDetails.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        
    }
}