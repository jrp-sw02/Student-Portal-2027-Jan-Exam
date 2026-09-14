using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data.Objects;
using EConnect;

public partial class Admin_NIELITStudentFeePaid : BasePage
{
    String strMessage = string.Empty;   
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;    
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 UserTypeId = 0;
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
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserType"]);        

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();                   
                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {                        
                        if (UserTypeId == 10)
                        {
                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                          NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                          if (NielitCentrelinkedToCentreId != 0)
                          {
                              NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                              txtInstitute.Text = intitutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                              hcentreID.Value = Convert.ToString( NelitCentreLinkId);
                              RdoAffInstOrNonAffInst.SelectedValue = "2";
                              ddlSubcentreName.Enabled = false;
                          }
                          else
                          {
                              NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                              txtInstitute.Text = intitutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                              hcentreID.Value = Convert.ToString(NelitCentreLinkId);
                              RdoAffInstOrNonAffInst.SelectedValue = "2";
                              ddlSubcentreName.Enabled = false;
                          }
                        }
                        else if (UserTypeId == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                hcentreID.Value = Convert.ToString(NelitCentreLinkId);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                            FillddlSubcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                        }
                        else if (UserTypeId == 4)

                        {
                            //

                            var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          where s.instituteID == loginUser.UserRefNumber
                                                          select new { ID = s.ID, linkedToCentre = s.linkedToCentre };
                            if (intituteslinkedToCentre.Count() == 0)
                            {
                                ShowAlert("Menu is not available for the institute");
                                return;
                            }
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                            //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                hcentreID.Value = NelitCentreLinkId.ToString();
                                //NielitCentreIdFilter = NelitCentreLinkId;
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlSubcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);     
                            //
                            //var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                            //NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            //NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            //if (institutesName != null)
                            //{
                            //    txtInstitute.Text = institutesName.Name;
                            //    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            //    hcentreID.Value = Convert.ToString(NelitCentreLinkId);
                            //}
                            //RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            //FillddlSubcentreName();
                            //RdoAffInstOrNonAffInst.Items.RemoveAt(1);                         
                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();                      
                    }
                    else
                    {                       
                        BindEditNewModeData();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();
                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Student Fee Paid", "Admin/NIELITStudentFeePaid.aspx?Id=" + Request.QueryString["Id"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Student Fee Paid", "Admin/NIELITStudentFeePaid.aspx", ""));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void BindEditNewModeData()
    {
        try
        {
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    if (UserTypeId == 10)
                    {
                        var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlSubcentreName.Enabled = false;
                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true  
                                            //(p.startDate <= System.DateTime.Now && 
                                                //  && (p.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                            && p.subCentreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlSubcentreName.Enabled = false;
                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true  
                                            //(p.startDate <= System.DateTime.Now &&
                                            //  && (p.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                    }
                    else if (UserTypeId == 11)
                    {
                        var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);
                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }                      
                        FillddlSubcentreName();                       
                        ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true  
                                        //(p.startDate <= System.DateTime.Now && 
                                            //  && (p.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                    }
                    else if (UserTypeId == 4)
                    {
                        Int32 subcentreId = 0;
                        var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                      where s.instituteID == loginUser.UserRefNumber
                                                      select new { ID = s.ID, linkedToCentre = s.linkedToCentre,s.instituteID };
                        if (intituteslinkedToCentre.Count() == 0)
                        {
                            ShowAlert("Menu is not available for the institute");
                            return;
                        }
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                        //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            hcentreID.Value = NelitCentreLinkId.ToString();
                            subcentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().instituteID);
                            HSubcentreID.Value = subcentreId.ToString();
                        }

                       
                        
                        
                        //
                        //var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                        //NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        //Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);

                        //NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        //if (institutesName != null)
                        //{
                        //    txtInstitute.Text = institutesName.Name;
                        //    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        //}                       
                        FillddlSubcentreName();                       
                        ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true 
                                        //(p.startDate <= System.DateTime.Now && 
                                            //  && (p.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                    }
                }
            }               
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFeeTypeMasId(DropDownList ddl, Int32 batchid)
    {
        try
        {
           // Int32 batchid = Convert.ToInt32(ddlBatch.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                var FeeTypeMas = from p in context.NielitCentreBatchs
                                 join s in context.NielitCentreBatchFees on p.ID equals s.batchID
                                 join f in context.feeTypeMas on s.feeTypeID equals f.ID
                                 where p.IsVerified == true && p.ID == batchid
                                 orderby (p.Name)
                                 select new { ValueField = s.feeTypeID, TextField = f.feeType };
                FeeTypeMas = FeeTypeMas.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, FeeTypeMas.Distinct(), lst);
                btnSave.Visible = false;
                btnCancel.Visible = false;
                lblMessage.Text = "";               
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 batchid = Convert.ToInt32(ddlBatch.SelectedValue);
            FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlBatch.SelectedValue));
            ddlbatchname.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 batchidfilter = Convert.ToInt32(ddlbatchname.SelectedValue);
            FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlBatch.SelectedValue));
            ddlBatch.SelectedValue = "0";
            ddlfeetypemasid.SelectedValue = "0";           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlSubcentreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 batchid = Convert.ToInt32(ddlBatch.SelectedValue);
            Int32 batchidFilter = Convert.ToInt32(ddlbatchname.SelectedValue);
            Int32 subcenterId = Convert.ToInt32(ddlSubcentreName.SelectedValue); 
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (subcenterId != 99)
                {
                    var BatchName = from p in context.NielitCentreBatchs
                                    where p.IsVerified == true && 
                                    //(p.startDate <= System.DateTime.Now && 
                                    (p.endDate >= System.DateTime.Now)
                                    && p.subCentreID == subcenterId
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                    FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlBatch.SelectedValue));                 
                }               
                btnSave.Visible = false;
                btnCancel.Visible = false;
                lblMessage.Text = "";               
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowRecord(object sender, EventArgs e)
    {
        BindGridView();
        btnCancel.Visible = false;
        btnSave.Visible = false;
        lblMessage.Text = "";
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
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                using (NIELITMISContext context = new NIELITMISContext())
                                {
                                    ListItem lst1 = new ListItem("--Select One--", "99");
                                    if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                                    {
                                        ddlSubcentreName.ClearSelection();
                                        var centreName = from s in context.AffInstitutes
                                                         where s.linkedToCentre == NelitCentreLinkId
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
                                                             where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                             select new { ValueField = s.ID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                            ddlSubcentreName.Enabled = false;
                                            var NonAfflcentre = (from p in context.NonAffInstitutes
                                                                 where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                               select p).FirstOrDefault();
                                            ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                                        }
                                        else //NonAffInstitutes for Nielit Centres
                                        {
                                            ddlSubcentreName.ClearSelection();
                                            var centreName = from s in context.NonAffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId
                                                             select new { ValueField = s.ID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                            ddlSubcentreName.Enabled = true;
                                        }
                                        if (UserTypeId == 4)//AffInstitutes by user refNumber
                                        {
                                            ddlSubcentreName.ClearSelection();
                                            var centreName = from s in context.AffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                             select new { ValueField = s.instituteID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                            ddlSubcentreName.Enabled = false;
                                            var Afflcentre = (from p in context.AffInstitutes
                                                                 where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                                                 select p).FirstOrDefault();
                                            ddlSubcentreName.SelectedValue = Afflcentre.ID.ToString();
                                        }
                                        else // AffInstitutes for Nielit Centres
                                        {
                                            ddlSubcentreName.ClearSelection();
                                            var centreName = from s in context.AffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId
                                                             select new { ValueField = s.instituteID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                            ddlSubcentreName.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        ddlSubcentreName.Items.Add(new ListItem("--Select One--", "99"));
                                        ddlSubcentreName.SelectedValue = "99";
                                        ddlSubcentreName.Enabled = false;
                                    }
                                }
                            }
                        else
                          {
                              NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                              txtInstitute.Text = institutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                              using (NIELITMISContext context = new NIELITMISContext())
                              {
                                  ListItem lst1 = new ListItem("--Select One--", "99");
                                  if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                                  {
                                      ddlSubcentreName.ClearSelection();
                                      var centreName = from s in context.AffInstitutes
                                                       where s.linkedToCentre == NelitCentreLinkId
                                                       select new { ValueField = s.instituteID, TextField = s.Name };
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                      ddlSubcentreName.Enabled = true;
                                      ListItem lst = new ListItem("--Select One--", "0");
                                    
                                      var BatchName = from p in context.NielitCentreBatchs
                                                      where p.IsVerified == true  
                                                      //(p.startDate <= System.DateTime.Now && 
                                                   // && (p.endDate >= System.DateTime.Now) // comment this line for previous date entry batch code and student records on 15 march 2021
                                                      && p.subCentreID == NielitCentreId
                                                      orderby (p.Name)
                                                      select new { ValueField = p.ID, TextField = p.Name };
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                                      FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlBatch.SelectedValue));                                     
                                  }
                                  else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                                  {
                                      ddlSubcentreName.ClearSelection();
                                      var centreName = from s in context.NonAffInstitutes
                                                       where s.linkedToCentre == NelitCentreLinkId
                                                       select new { ValueField = s.ID, TextField = s.Name };
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                      ddlSubcentreName.Enabled = true;

                                      ListItem lst = new ListItem("--Select One--", "0");
                                      var BatchName = from p in context.NielitCentreBatchs
                                                      where p.IsVerified == true  
                                                      //(p.startDate <= System.DateTime.Now && 
                                                    // && (p.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                                      && p.subCentreID == NielitCentreId
                                                      orderby (p.Name)
                                                      select new { ValueField = p.ID, TextField = p.Name };
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                                      FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlBatch.SelectedValue));                                  
                                  }
                                  else
                                  {
                                      ddlSubcentreName.Items.Add(new ListItem("--Select One--", "99"));
                                      ddlSubcentreName.SelectedValue = "99";
                                      ddlSubcentreName.Enabled = false;
                                      ListItem lst = new ListItem("--Select One--", "0");
                                      var BatchName = from p in context.NielitCentreBatchs
                                                      where p.IsVerified == true 
                                                      //(p.startDate <= System.DateTime.Now && 
                                                          //&&(p.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                                      && p.centreID == NielitCentreId
                                                      orderby (p.Name)
                                                      select new { ValueField = p.ID, TextField = p.Name };
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                                      EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                                      FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlBatch.SelectedValue));                                     
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
    protected void FillddlSubcentreName()
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                ListItem lst1 = new ListItem("--Select One--", "99");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    ddlSubcentreName.ClearSelection();
                    if (UserTypeId == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.instituteID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";                        
                    }
                }
            }           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    protected void FillCourses(DropDownList ddl, Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 
    protected void BindGridView()
    {
        try
        {           
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 BatchID = 0, BatchIDFilter=0;
                Int32 FeeTypeMasID = 0;
                Int32 courseID = 0;
                Int64 subcenterId=99;
                Int32 NielitCentreLinkid = 0;
                NielitCentreLinkid = Convert.ToInt32( hcentreID.Value);
                if (ddlBatch.SelectedValue != "0")
                    BatchID = Convert.ToInt64(ddlBatch.SelectedValue);
                if (ddlbatchname.SelectedValue != "0")
                    BatchIDFilter = Convert.ToInt64(ddlbatchname.SelectedValue);
                if (ddlfeetypemasid.SelectedValue != "0")
                    FeeTypeMasID = Convert.ToInt32(ddlfeetypemasid.SelectedValue);

                if (ddlSubcentreName.SelectedValue != "99")
                {
                    subcenterId = Convert.ToInt32(ddlSubcentreName.SelectedValue);
                    NielitCentreLinkid = 9999999;
                }
                //else
                //{
                //    if (UserTypeId == 4)
                //    {
                //        NielitCentreLinkid = Convert.ToInt32(HSubcentreID.Value);
                //    }
                //}
                
                var CentreBatchs = from s in context.NielitCentreStudent  
                                   join c in context.NielitCentreBatchFees on s.batch_ID equals c.batchID 
                                   from  p in context.NIELITStudentFeePaids.Where(x => s.ID==x.studentID && x.feeTypeID==c.feeTypeID).DefaultIfEmpty()
                                   where s.batch_ID == c.batchID && s.batch_ID == BatchID && c.feeTypeID == FeeTypeMasID && s.InstituteID == NielitCentreLinkid || s.InstituteID == subcenterId                               
                                   //orderby s.ID descending
                                   select new
                                   {                                      
                                       ID = s.ID,
                                       Name = s.Name,
                                       CentreId = s.InstituteID,                                       
                                       BatchCode = s.batch_ID,
                                       FeetypeMasIDD=c.feeTypeID,
                                       FatherName = s.FatherName, 
                                       DoB = s.DateOfBirth,                                      
                                       AmountPaid =(int?)p.AmtPaid  ,
                                       PaymentDate = (DateTime?)p.paymentDate,
                                       Remark_s =(String) p.Remarks,                                      
                                       enterBy = s.enter_By,                                       
                                   };
                var CentreBatchsFilter = from s in context.NielitCentreStudent
                                         join c in context.NielitCentreBatchFees on s.batch_ID equals c.batchID
                                         join f in context.feeTypeMas on c.feeTypeID equals f.ID
                                         from p in context.NIELITStudentFeePaids.Where(x => s.ID == x.studentID && x.feeTypeID == c.feeTypeID).DefaultIfEmpty()
                                         where s.batch_ID == c.batchID && s.batch_ID == BatchIDFilter && s.InstituteID == NielitCentreLinkid || s.InstituteID == subcenterId                                         
                                         //orderby s.ID descending
                                         select new
                                         {
                                             ID = s.ID,
                                             Name = s.Name,
                                             CentreId = s.InstituteID,
                                             BatchCode = s.batch_ID,
                                            FeetypeMasIDD = f.feeType,
                                             FatherName = s.FatherName,
                                             DoB = s.DateOfBirth,
                                             AmountPaid = (int?)p.AmtPaid,
                                             PaymentDate = (DateTime?)p.paymentDate,
                                             Remark_s = (String)p.Remarks,
                                             enterBy = s.enter_By,
                                         };                 
                if (BatchIDFilter != 0)
                {
                    CentreBatchsFilter = CentreBatchsFilter.Where(s => s.BatchCode == BatchIDFilter);
                }

                if (BatchID != 0)
                {
                    CentreBatchs = CentreBatchs.Where(s => s.BatchCode == BatchID);
                }
                if (UserTypeId != 10)
                {
                    CentreBatchs = CentreBatchs.Where(s => s.enterBy == loginUserNo);                  
                }

                if (FeeTypeMasID != 0)
                {
                    CentreBatchs = CentreBatchs.Where(s => s.FeetypeMasIDD == FeeTypeMasID);
                }
              
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.ID);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.Name);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                            break; 
                        default:
                            CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                            break;
                    }
                }
                CentreBatchs = CentreBatchs.OrderBy(s => s.AmountPaid );
                if (!String.IsNullOrEmpty(searchString))
                {
                    CentreBatchs = CentreBatchs.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                CentreBatchs = CentreBatchs.OrderByDescending(s=>s.ID);               
                CentreBatchsFilter = CentreBatchsFilter.OrderByDescending(s => s.ID);
                if (BatchIDFilter != 0)
                {
                    PagingBar1.Bind(CentreBatchsFilter, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;
                    PagingBar1.Visible = true;
                    gvMain.Visible = true;
                    divGrid.Visible = true;  
                    if (gvMain.Rows.Count <= 0)
                    {
                        if (BatchIDFilter == 0)
                        {
                            lblError.Visible = false;
                            lblError.Text = "No record found.";
                        }
                        else
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                        }
                        lblError.Visible = true;
                        gvMain.Visible = false;
                        PagingBar1.Visible = false;
                    }
                }
                else
                {
                    PagingBar1.Bind(CentreBatchs, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;
                    PagingBar1.Visible = true;
                    gvMain.Visible = true;
                    divGrid.Visible = true; 
                    if (gvMain.Rows.Count <= 0)
                    {
                        if (ddlfeetypemasid.SelectedValue == "0")
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = false;
                        }
                        else
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                        }                     
                        gvMain.Visible = false;
                        PagingBar1.Visible = false;
                    }
                }
                if (FeeTypeMasID == 0 && BatchID == 0)
                {
                    gvMain.Visible = false;
                    PagingBar1.Visible = false;
                } 
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[7].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
      
    }
    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        bool isUpdateVisible = false;
       lblMessage.Text = string.Empty;       
        //Loop through all rows in GridView
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[8].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                    row.RowState = DataControlRowState.Edit;
                for (int i = 3; i < row.Cells.Count; i++)
                {
                   // row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                    if (row.Cells[i].Controls.OfType<TextBox>().ToList().Count > 0)
                    {
                        row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;                        
                    }
                    if (isChecked && !isUpdateVisible)
                    {                       
                        isUpdateVisible = true;
                        btnCancel.Visible = true;
                    }
                }
                String AmountPaid = row.Cells[5].Controls.OfType<Label>().FirstOrDefault().Text;
                if (AmountPaid != "")
                {
                    //row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                }
                String PaymentDate = row.Cells[6].Controls.OfType<Label>().FirstOrDefault().Text;
                if (PaymentDate != "")
                {
                   // row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                }
            }
        }
         btnSave.Visible = isUpdateVisible;
         btnCancel.Visible = isUpdateVisible;
         lblMessage.Text = "";
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render(); 
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {        
        try
        {
            BreadCrumb1.Render();
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            Int64 feetypemasid = Convert.ToInt64(ddlfeetypemasid.SelectedValue);
            Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue);
            Int64 batchidFilter = Convert.ToInt64(ddlbatchname.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                NIELITStudentFeePaid objStudentFeePaid;

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    objStudentFeePaid = new NIELITStudentFeePaid();

                    foreach (GridViewRow row in gvMain.Rows)
                    {
                        bool isChecked = row.Cells[8].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            int StudentID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Value);
                            //objStudentFeePaid = context.NIELITStudentFeePaids.Find(StudentID);
                            int StudentIDExists = (from b in context.NIELITStudentFeePaids
                                                   where b.studentID == StudentID && b.feeTypeID == feetypemasid
                                                   select b).Count();
                           
                            var item = (from x in context.NielitCentreBatchFees where x.feeTypeID == feetypemasid && x.batchID == batchid select x).First();
                            var BatchStartDates = (from x in context.NielitCentreBatchs where x.ID == batchid select x).First();
                            Int32 feeamount = item.feeAmount;
                            DateTime BatchStartDate = Convert.ToDateTime(BatchStartDates.startDate);
                            DateTime paymentDate = Convert.ToDateTime(row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text);
                            Int32 AmountPaid = Convert.ToInt32(row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Text);
                            //if (AmountPaid == feeamount)
                            if (AmountPaid <= feeamount && AmountPaid >= 0)
                            {
                                //DateTime test = BatchStartDate.AddMonths(-6);
                                if (paymentDate >= BatchStartDate.AddMonths(-6) && paymentDate <= System.DateTime.Now)
                                {
                                    if (StudentIDExists == 0)
                                    {
                                        objStudentFeePaid.AmtPaid = Convert.ToInt32(row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Text);

                                        objStudentFeePaid.paymentDate = Convert.ToDateTime(row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                        objStudentFeePaid.Remarks = row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text;
                                        objStudentFeePaid.feeTypeID = feetypemasid;
                                        objStudentFeePaid.studentID = Convert.ToInt64(StudentID);
                                        objStudentFeePaid.enterBy = Convert.ToInt32(Session["UserID"]);
                                        objStudentFeePaid.enterDate = DateTime.Now;
                                        context.NIELITStudentFeePaids.Add(objStudentFeePaid);
                                        context.SaveChanges();
                                        strMessage = "Record updated.";
                                        lblMessage.Text = "Data updated successfully!";
                                        lblMessage.ForeColor = System.Drawing.Color.Green;
                                    }
                                    else
                                    {
                                        var previousAmtPaid = (from x in context.NIELITStudentFeePaids where x.feeTypeID == feetypemasid && x.studentID == StudentID select x).First();
                                        Int32 PreviousAmtPaid = previousAmtPaid.AmtPaid;
									
                                        if (AmountPaid > PreviousAmtPaid && AmountPaid >= 0)
                                        {
                                            NIELITStudentFeePaid objStudentFeePaidUpdate;
                                            objStudentFeePaidUpdate = new NIELITStudentFeePaid();
                                            StudentID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Value);
                                            objStudentFeePaidUpdate = context.NIELITStudentFeePaids.Single(k => k.studentID == StudentID && k.feeTypeID == feetypemasid);
                                            //objStudentFeePaidUpdate = context.NIELITStudentFeePaids.Find(StudentID);
                                            objStudentFeePaidUpdate.AmtPaid = Convert.ToInt32(row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                            objStudentFeePaidUpdate.paymentDate = Convert.ToDateTime(row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                            objStudentFeePaidUpdate.Remarks = row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text;
                                            int amountpaid = Convert.ToInt32(row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                            objStudentFeePaidUpdate.enterBy = Convert.ToInt32(Session["UserID"]);
                                            objStudentFeePaidUpdate.enterDate = DateTime.Now;
                                            NIELITStudentFeePaidHistory(StudentID, amountpaid, feetypemasid);
                                            context.SaveChanges();
                                            lblMessage.Text = "Data updated successfully!";
                                            lblMessage.ForeColor = System.Drawing.Color.Green;
                                        }
                                        else
                                        {
                                            lblMessage.Text = "Amount can not be less than previous amount!";
                                            lblMessage.ForeColor = System.Drawing.Color.Red;
                                        }
                                        BindGridView();
                                        //lblMessage.Text = "Data updated successfully!";
                                        //lblMessage.ForeColor = System.Drawing.Color.Green;
                                        btnSave.Visible = false;
                                        btnCancel.Visible = false;
                                    }

                                }
                                else
                                {
                                    lblMessage.Text = "Payment Date should be less or current date, not more than Batch Start Date!";
                                    lblMessage.ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Fee Amount should be positive and less than or equal to " + feeamount + "/- !";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
                }           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public void NIELITStudentFeePaidHistory(Int64 id, int amountpaid, Int64 feetypeid)
    {
        try
        {
            //string constr = ConfigurationManager.ConnectionStrings["NIELITMISContextt"].ConnectionString;
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("InsertNIELITStudentFeePaidHistory", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@amountpaid", amountpaid);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@feetypeid", feetypeid);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            BindGridView();                  
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {           
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            ddlbatchname.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
  
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                   href += "&Id=" + Request.QueryString["Id"].ToString();
                }               
                if (ddlbatchname.SelectedValue != "0")
                {
                    foreach (DataControlField col in gvMain.Columns)
                    {
                        if (col.HeaderText == "FeeDescription")
                        {
                            col.Visible = true;
                        }
                        if (col.HeaderText == "")
                        {
                            col.Visible = false;
                        }
                        if (col.HeaderText == "lblIdVisibleFalse")
                        {
                            col.Visible = false;
                        }
                    }
                }
                else
                {
                    foreach (DataControlField col in gvMain.Columns)
                    {
                        if (col.HeaderText == "FeeDescription")
                        {
                            col.Visible = false;
                        }
                        if (col.HeaderText == "")
                        {
                            col.Visible = true;
                        }
                    }
                } 
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        Int32 loginUserNo = 0, UserTypeId = 0, NielitSubCentreIdd=0, NielitCentreIdSearch = 0, NielitCentrelinkedToCentreId = 0;
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();

                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                if (UserTypeId == 10)
                {
                    var intituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                    Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitCentreIdSearch = NielitCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        NielitCentreIdSearch = NielitCentreId;
                    }
                    else
                    {
                        NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        NielitCentreIdSearch = NielitCentreId;
                    }
                }
                else if (UserTypeId == 11)
                {
                    var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                    Int32 NielitSubCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitSubCentreIdd = NielitSubCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    if (institutesName != null)
                    {                       
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NielitCentreIdSearch = NielitSubCentreId;
                    }                   
                }
                else if (UserTypeId == 4)
                {
                    var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                    Int32 NielitSubCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitSubCentreIdd = NielitSubCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    if (institutesName != null)
                    {                       
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NielitCentreIdSearch = NielitSubCentreId;
                    }                   
                }
            }      
            if (count <= 0)
                count = 10;
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();

            var candidatesName = from s in context.NielitCentreStudent
                                 join b in context.NielitCentreBatchs on s.batch_ID equals b.ID
                                 select new { Name = s.Name, CentreID = s.InstituteID, Enterby = s.enter_By, subcentreId= b.subCentreID };
            if (UserTypeId != 10)
            {
                candidatesName = candidatesName.Where(s => s.Enterby == loginUserNo || s.subcentreId == NielitSubCentreIdd);
            }
            else
            {
                candidatesName = candidatesName.Where(s => s.CentreID == NielitCentreIdSearch);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                candidatesName = candidatesName.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            candidatesName = candidatesName.OrderBy(s => s.Name);
            foreach (var c in candidatesName)
            {
                items.Add(c.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {          
        BindGridView();
        btnSave.Visible = false;
        btnCancel.Visible = false;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("NIELITStudentFeePaid.aspx", true);
    }   
}