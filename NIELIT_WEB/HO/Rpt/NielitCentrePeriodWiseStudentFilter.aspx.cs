using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using EConnect;
using EConnect.Utils.Common;

public partial class NielitCentrePeriodWiseStudentFilter : BasePage
{
     UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {        
        lblerror.Text = "";
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            loginUserType = (UserType)Session["UserType"];

            if (!IsPostBack)
            {
                bindCentre();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre PeriodWise Students", "HO/Rpt/NielitCentrePeriodWiseStudentFilter.aspx", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }    
    }
   
    #region vCode

    #region Old
    //protected void btnView_Click(object sender, EventArgs e)
    //{
    //    if (ddlCentreName.SelectedValue.ToString().Equals("0"))
    //    {
    //        ShowAlert("Select Centre Name");
    //        return;
    //    }
    //    if (ddlReportType.SelectedValue.ToString().Equals("0"))
    //    {
    //        ShowAlert("Select Year Type");
    //        return;
    //    }
    //    if (txtBatchFrom.Text == "")
    //    {
    //        ShowAlert("Select Batch From Date");
    //        return;
    //    }
    //    if (txtBatchto.Text == "")
    //    {
    //        ShowAlert("Select Batch To Date");
    //        return;
    //    }

    //    try
    //    {
    //        string centre = "", yearType = "", batchFromDate = "", batchToDate = "", RdsearchbyRadio = "";

    //        if (Rdsearchby.SelectedValue == "N")
    //        {
    //            RdsearchbyRadio = Rdsearchby.SelectedValue;  // in case of All students
    //            centre = ddlCentreName.SelectedItem.Value;
    //            yearType = RdsearchbyRadio;
    //            batchFromDate = txtBatchFrom.Text;
    //            batchToDate = txtBatchto.Text;
    //            string category = "0", gender = "0";

    //            //if (centre != "0" && yearType != "" && batchFromDate != "" && batchToDate != "")
    //            //{
    //                string centreID = centre;
    //                string TypeYear = yearType;
    //                string FromDate = batchFromDate;
    //                string ToDate = batchToDate;
    //                string categoryId = category;
    //                string genderId = gender;

    //                // Display Report on New Tap
    //                string url = "NielitCentrePeriodWiseStudentRep.aspx?centreID=" + centreID + "&TypeYear=" + TypeYear + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&categoryId=" + categoryId + "&genderId=" + genderId;
    //            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url + "' ,'_blank');", true);

    //            // Display Report on same page
    //            //    Response.Redirect(string.Format("../Rpt/NielitCentrePeriodWiseStudentRep.aspx?centreID={0}&TypeYear={1}&FromDate={2}&ToDate={3}&categoryId={4}&genderId={5}", centreID, TypeYear, FromDate, ToDate, categoryId, genderId));
    //        }
    //        //if (Rdsearchby.SelectedValue == "N")
    //        //{
    //        //    RdsearchbyRadio = "N";  // in case of All students
    //        //    centre = ddlCentreName.SelectedItem.Value;
    //        //    yearType = ddlReportType.SelectedValue;
    //        //    batchFromDate = txtBatchFrom.Text;
    //        //    batchToDate = txtBatchto.Text;
    //        //    string category = "0", gender = "0";
    //        //    //if (centre != "0" && yearType != "" && batchFromDate != "" && batchToDate != "")
    //        //    //{
    //        //    string centreID = HttpUtility.UrlEncode(Encrypt(centre));
    //        //    string TypeYear = HttpUtility.UrlEncode(Encrypt(yearType));
    //        //    string FromDate = HttpUtility.UrlEncode(Encrypt(batchFromDate));
    //        //    string ToDate = HttpUtility.UrlEncode(Encrypt(batchToDate));
    //        //    string categoryId = HttpUtility.UrlEncode(Encrypt(category));
    //        //    string genderId = HttpUtility.UrlEncode(Encrypt(gender));
    //        //    Response.Redirect(string.Format("../Rpt/NielitCentrePeriodWiseStudentRep.aspx?centreID={0}&TypeYear={1}&FromDate={2}&ToDate={3}&categoryId{4}&genderId{5}", centreID, TypeYear, FromDate, ToDate, categoryId, genderId));
    //        //    //}
    //        //    //else
    //        //    //{
    //        //    //    ShowAlert("Select All Filters..", true);
    //        //    //    return;
    //        //    //}
    //        //}

    //       else if (Rdsearchby.SelectedValue == "C")
    //        {
    //            RdsearchbyRadio = "C";
    //            if (ddlCastCategory.SelectedValue.ToString().Equals("0"))
    //            {
    //                ShowAlert("Select Category");
    //                return;
    //            }

    //            //string centre = "", yearType = "", batchFromDate = "", batchToDate = "", category = "";
    //            string category = "", gender="0";

    //            RdsearchbyRadio = Rdsearchby.SelectedValue;
    //            centre = ddlCentreName.SelectedItem.Value;
    //            yearType = RdsearchbyRadio;
    //            batchFromDate = txtBatchFrom.Text;
    //            batchToDate = txtBatchto.Text;
    //            category = ddlCastCategory.SelectedValue;

    //            //if (centre != "0" && yearType != "" && batchFromDate != "" && batchToDate != "" && category !="")
    //            //{
    //            //string centreID = HttpUtility.UrlEncode(Encrypt(centre));
    //            //string TypeYear = HttpUtility.UrlEncode(Encrypt(yearType));
    //            //string FromDate = HttpUtility.UrlEncode(Encrypt(batchFromDate));
    //            //string ToDate = HttpUtility.UrlEncode(Encrypt(batchToDate));
    //            //string categoryId = HttpUtility.UrlEncode(Encrypt(category));
    //            //string genderId = HttpUtility.UrlEncode(Encrypt(gender));

    //            string centreID = centre;
    //            string TypeYear = yearType;
    //            string FromDate = batchFromDate;
    //            string ToDate = batchToDate;
    //            string categoryId = category;
    //            string genderId = gender;

    //            string url = "NielitCentrePeriodWiseStudentRep.aspx?centreID=" + centreID + "&TypeYear=" + TypeYear + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&categoryId=" + categoryId + "&genderId=" + genderId;
    //            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url + "' ,'_blank');", true);


    //            //Response.Redirect(string.Format("../Rpt/NielitCentrePeriodWiseStudentRep.aspx?centreID={0}&TypeYear={1}&FromDate={2}&ToDate={3}&categoryId={4}&genderId={5}", centreID, TypeYear, FromDate, ToDate, categoryId, genderId));



    //            //}
    //            //else
    //            //{
    //            //    ShowAlert("Select All Filters..", true);
    //            //    return;
    //            //}
    //        }

    //       else if (Rdsearchby.SelectedValue == "G")
    //        {
    //            if (ddlGender.SelectedValue.ToString().Equals("0"))
    //            {
    //                ShowAlert("Select Gender");
    //                return;
    //            }

    //            string gender = "";
    //            RdsearchbyRadio = Rdsearchby.SelectedValue;
    //            centre = ddlCentreName.SelectedItem.Value;
    //            yearType = RdsearchbyRadio;
    //            batchFromDate = txtBatchFrom.Text;
    //            batchToDate = txtBatchto.Text;
    //            gender = ddlGender.SelectedItem.Value;
    //            string category = "0";

    //            //if (centre != "0" && yearType != "" && batchFromDate != "" && batchToDate != "" && gender != "")
    //            //{
    //            //string centreID = HttpUtility.UrlEncode(Encrypt(centre));
    //            //string TypeYear = HttpUtility.UrlEncode(Encrypt(yearType));
    //            //string FromDate = HttpUtility.UrlEncode(Encrypt(batchFromDate));
    //            //string ToDate = HttpUtility.UrlEncode(Encrypt(batchToDate));
    //            //string categoryId = HttpUtility.UrlEncode(Encrypt(category));
    //            //string genderId = HttpUtility.UrlEncode(Encrypt(gender));

    //            string centreID = centre;
    //            string TypeYear = yearType;
    //            string FromDate = batchFromDate;
    //            string ToDate = batchToDate;
    //            string categoryId = category;
    //            string genderId = gender;

    //            string url = "NielitCentrePeriodWiseStudentRep.aspx?centreID=" + centreID + "&TypeYear=" + TypeYear + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&categoryId=" + categoryId + "&genderId=" + genderId;
    //            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url + "' ,'_blank');", true);

    //            //Response.Redirect(string.Format("../Rpt/NielitCentrePeriodWiseStudentRep.aspx?centreID={0}&TypeYear={1}&FromDate={2}&ToDate={3}&categoryId={4}&genderId={5}", centreID, TypeYear, FromDate, ToDate, categoryId, genderId));


    //            //}
    //            //else
    //            //{
    //            //    ShowAlert("Select All Filters..", true);
    //            //    return;
    //            //}
    //        }
    //        //    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    //        //{
    //        //    GridViewRow row = gvMain.SelectedRow;
    //        //    //Find label id from gridview cell data
    //        //    Label lblRegno = row.FindControl("lblRegno") as Label;
    //        //    Label lblExamid = row.FindControl("lblExamid") as Label;
    //        //    Int64 Examid = 0;
    //        //    Examid = Convert.ToInt64(lblExamid.Text);
    //        //    Int64 RegistrationNo = Convert.ToInt64(lblRegno.Text);
    //        //    string url = "PuraskarAppDocsVerAndDecByInstt.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo;
    //        //    EConnect.Utils.Security.QuertStringModule.Encrypt(url);
    //        //    string url2 = EConnect.Utils.Security.QuertStringModule.Encrypt(url);
    //        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url2 + "' ,'_blank');", true);
    //        //    //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarAppDocsVerAndDecByInstt.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo), true); 
    //        //    lblMessage.Text = "";
    //        //} 
    //        //string url = "PuraskarAppModulesVerificationByExam.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo + "&VF=" + ViewFor;
    //        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url + "' ,'_blank');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
#endregion

    protected void bindCentre()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (UserTypeId == 6)
                {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var centreName1 = from s in context.NielitCentres
                                      select new { ValueField = s.ID, TextField = s.Name };
                    if (centreName1 != null)
                    {
                        var centreName = centreName1.OrderBy(i => i.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                        ddlCentreName.Enabled = true;
                    }
                }
                else
                {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var centreName = from s in context.NielitCentres
                                     where s.ID == entityID
                                     select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                    ddlCentreName.SelectedValue = Convert.ToInt32(entityID).ToString();
                    ddlCentreName.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCentreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlReportType.Items.Clear();
        ddlReportType.Items.Insert(0, new ListItem("Financial Year", "F"));
        ddlReportType.Items.Insert(0, new ListItem("Calendar Year", "C"));
        ddlReportType.Items.Insert(0, new ListItem("--Select One--", "0"));
    }    
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCentreName.SelectedValue.ToString().Equals("0"))
        {
            ShowAlert("Select Centre Name");
            return;
        }

        if (ddlReportType.SelectedValue == "C")
        {

            trBatchdate.Visible = true;
            trbatchfrom.Visible = true;
            txtBatchFrom.Text = "";
            txtBatchto.Text = "";
            txtBatchFrom.Enabled=true;
            txtBatchto.Enabled = false;
        }
        else if (ddlReportType.SelectedValue == "F")
        {
            trBatchdate.Visible = true;
            trbatchfrom.Visible = true;
            txtBatchFrom.Text = "";
            txtBatchto.Text = "";
            txtBatchFrom.Enabled = true;
            txtBatchto.Enabled = false;
        }
        if (txtBatchFrom.Text.Trim().Length != 0)
            txtBatchFrom_TextChanged(sender, e);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/HO/Rpt/NielitCentrePeriodWiseStudentFilter.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

        //try
        //{
        //    BreadCrumb1.Render();
        //    ddlCentreName.SelectedValue = "0";
        //    ddlReportType.SelectedValue = "0";            
        //    txtBatchFrom.Text = "";
        //    txtBatchto.Text = "";
        //}
        //catch (Exception ex)
        //{
        //    // ShowAlert(ex.Message, true);
        //}
    }
    protected void BindGender()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                //ddlGender.Items.Clear();
                //ListItem lst = new ListItem("--Select One--", "0");
                //var Gender = from s in vContext.tblGender
                //             select new { ValueField = s.ID, TextField = s.name};
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlGender, Gender, lst);

                ddlGender.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                var Gender = from s in vContext.tblGender
                             select new { ValueField = s.genderCode, TextField = s.name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlGender, Gender, lst);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = (ex.Message);
            lblerror.Visible = true;
        }
    }
    protected void BindCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {

                ddlCastCategory.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");

                var castcategory = from p in context.CastCategories
                                   orderby (p.DisplayOrder)
                                   select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCastCategory, castcategory, lst);
            };
        }
        catch (Exception ex)
        {
            lblerror.Text = (ex.Message);
            lblerror.Visible = true;
        }
    }
    protected void txtBatchFrom_TextChanged(object sender, EventArgs e)
    {
        DateTime batchFromDate = Convert.ToDateTime(txtBatchFrom.Text);
        txtBatchto.Enabled = false;
        if (ddlReportType.SelectedValue == "C" && (batchFromDate.Day.ToString() != "1" || batchFromDate.Month.ToString() != "1"))
        {
            lblerror.Text = "Invalid Batch from date chosen for Calendar Year";
            lblerror.ForeColor = System.Drawing.Color.Red;
            lblerror.Visible = true;
            return;
        }
        if (ddlReportType.SelectedValue == "C" && batchFromDate.Day.ToString() == "1" && batchFromDate.Month.ToString() == "1")
        {
            txtBatchto.Text = batchFromDate.AddYears(1).AddDays(-1).ToString("dd-MMM-yyyy");
            return;
        }
        if (ddlReportType.SelectedValue == "F" && (batchFromDate.Day.ToString() != "1" || batchFromDate.Month.ToString() != "4"))
        {
            lblerror.Text = "Invalid Batch from date chosen for Financial Year";
            lblerror.Visible = true;
            lblerror.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (ddlReportType.SelectedValue == "F" && batchFromDate.Day.ToString() == "1" && batchFromDate.Month.ToString() == "4")
        {
            txtBatchto.Text = batchFromDate.AddYears(1).AddDays(-1).ToString("dd-MMM-yyyy");
            return;
        }
    }
    protected void Rdsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rdsearchby.SelectedValue == "N")
        {
            trCategory.Visible = false;
            trGender.Visible = false;
        }
        if (Rdsearchby.SelectedValue == "C")
        {
            trCategory.Visible = true;
            trGender.Visible = false;
            BindCategory();
        }
        if (Rdsearchby.SelectedValue == "G")
        {
            trCategory.Visible = false;
            trGender.Visible = true;
            BindGender();
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
    #endregion
}
