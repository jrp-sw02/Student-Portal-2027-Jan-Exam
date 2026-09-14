using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

public partial class HO_NielitCentreStudentTrainedFilter : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0; 
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student Trained", "HO/Rpt/NielitCentreStudentTrainedFilter.aspx", ""));
           
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
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                BindCentres();
            }
            BreadCrumb1.Render();           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
        protected void BindCentres()
    {
        try
        {
            ddlCentreName.ClearSelection();
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var NielitList = from p in context.NielitCentres
                                  orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName , NielitList.Distinct(), lst);
            };
            ddlSubcentreName.Enabled = false;
            ddlCentreName.Enabled = false;
         
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
            ddlCentreName.SelectedValue = "0";
            ddlSubcentreName.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtDateto.Text = "";        
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            User objUser;

            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeId == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeId == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre); 
                    }
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        //txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NIELITCentreId.Value = NelitCentreLinkId.ToString();
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "0");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                ddlSubcentreName.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeId == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = false;
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                                    ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = true;
                                    ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();
                                }
                                if (UserTypeId == 4)//AffInstitutes by user refNumber
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.instituteID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlSubcentreName.SelectedValue = Afflcentre.ID.ToString();
                                    ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = true;
                                    ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();
                                }
                            }
                            else
                            {
                                ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                                ddlSubcentreName.SelectedValue = "0";
                                ddlSubcentreName.Enabled = false;
                            }
                        }
                    }
                    
                    // Deep add code for Ho or Admin on 10 march 2021
                    if (UserTypeId == 6)
                    {                        
                            BindCentres();
                            ddlCentreName.Enabled = true;
                            ddlSubcentreName.SelectedValue = "0";                   

                    }
                    // Deep add code for Ho or Admin on 10 march 2021

                    else
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                       // txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "0");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                ddlSubcentreName.Enabled = true;
                                ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();
                                //txtName.Text = "";
                                //txtBatchCode.Text = "";
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                ddlSubcentreName.Enabled = true;
                                ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();
                                //txtName.Text = "";
                                //txtBatchCode.Text = "";
                            }
                            else
                            {
                                ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                                ddlSubcentreName.SelectedValue = "0";
                                ddlSubcentreName.Enabled = false;

                                NIELITCentreId.Value = NielitCentreId.ToString();


                                //deep add on 9 march 2021                                  
                                     institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        //txtInstitute.Text = institutesName.Name;
                         NelitCentreLinkId = Convert.ToInt32(institutesName.ID);

                         if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                         {
                             ddlCentreName.ClearSelection();
                             using (NIELITMISContext context3 = new NIELITMISContext())
                             {
                                 ListItem lst = new ListItem("--Select One--", "0");
                                 var NielitList = from p in context3.NielitCentres
                                                  orderby (p.Name)
                                                  select new { ValueField = p.ID, TextField = p.Name };
                                 EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, NielitList.Distinct(), lst);
                             };
                             ddlSubcentreName.SelectedValue = "0";
                             ddlCentreName.Enabled = false;
                             ddlSubcentreName.Enabled = false;
                         }
                         ddlCentreName.SelectedValue = NelitCentreLinkId.ToString();

                                //deep add end on 9 march 2021
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }

    protected void ddlCentreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 ddlcentreId = Convert.ToInt32(ddlCentreName.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst1 = new ListItem("--Select One--", "0");
                if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                {
                    ddlSubcentreName.ClearSelection();
                    var centreName = from s in context.AffInstitutes
                                     where s.linkedToCentre == ddlcentreId
                                     select new { ValueField = s.instituteID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                    ddlSubcentreName.Enabled = true;
                }
                else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                {
                    if (UserTypeId == 11)//Non AffInstitutes by user refNumber
                    {
                        ddlSubcentreName.ClearSelection();
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == ddlcentreId 
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == ddlcentreId 
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                    }
                    else //NonAffInstitutes for Nielit Centres
                    {
                        ddlSubcentreName.ClearSelection();
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == ddlcentreId
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = true;
                    }
                    if (UserTypeId == 4)//AffInstitutes by user refNumber
                    {
                        ddlSubcentreName.ClearSelection();
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == ddlcentreId 
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = false;
                        var Afflcentre = (from p in context.AffInstitutes
                                          where p.linkedToCentre == ddlcentreId 
                                          select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = Afflcentre.ID.ToString();
                    }                   
                }
                else
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {
                        ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                        ddlSubcentreName.SelectedValue = "0";
                        ddlSubcentreName.Enabled = false;
                    }
                }
            }
        }

        catch (Exception ex)
        {            
            ShowAlert(ex.Message, true);
        }
    }   
}