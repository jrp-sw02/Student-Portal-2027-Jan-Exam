using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Collections.Generic;
using System.Web.UI;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

public partial class VANielitCentrePaymentReportForGst : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                //Testing
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                BindCentres();
                txtDateFrom.Text = "";
                txtDateto.Text = "";
                txtDateto.Enabled = false;
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Payment Report For Virtual Academy Course", "HO/Rpt/VANielitCentrePaymentReportForGst.aspx", ""));
            }
            BreadCrumb1.Render();           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }    

    #region vCode
    protected void BindCentres()
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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/HO/Rpt/VANielitCentrePaymentReportForGst.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        //try 
        //{
        //    ddlCentreName.SelectedValue = "0";
        //    ddlSubcentreName.SelectedValue = "0";
        //    txtDateFrom.Text = "";
        //    txtDateto.Text = "";        
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message);
        //}
    }    
    protected void ddlCentreName_SelectedIndexChanged(object sender, EventArgs e)
    {

            txtDateFrom.Text = "";
            txtDateto.Text = "";
            //return;
        //try
        //{
        //    txtDateFrom.Text = "";
        //    txtDateto.Text = "";
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }    
    protected void txtDateFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDateFrom.Text != "")
            {
                if (!IsDate(txtDateFrom.Text))
                {
                    txtDateFrom.Text = "";
                    txtDateto.Text = "";

                    ShowAlert("Invalid payment settled from date.");
                    return;
                }

                DateTime TrFromDate = Convert.ToDateTime(txtDateFrom.Text);

                if (TrFromDate.Day.ToString() != "1")
                {
                    lblerror.Text = "Invalid payment settled from date. Please, select First day of any month.";
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    lblerror.Visible = true;
                    return;
                }

                if (TrFromDate.Day.ToString() == "1")
                {
                    txtDateto.Text = TrFromDate.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");
                    lblerror.Visible = false;
                    return;
                }
                txtDateto.Enabled = false;
            }
            else
            {
                txtDateFrom.Text = "";
                txtDateto.Text = "";

            }
            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }    
    #endregion
    
}