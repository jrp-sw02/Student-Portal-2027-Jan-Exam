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
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

public partial class HO_NielitCentreBatchFee : BasePage
{
    NIELITMISContext context;
    String strMessage = string.Empty;    
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    Int32 lnkID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0; 
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
            UserTypeid = Convert.ToInt32(Session["UserType"]);
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                objUser = new EConnect.URM.User();

                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);
                Session["UserRefNumber"] = UserRefNumber.ToString();

            }
            NIELITMIS lnkMIS = new NIELITMIS();
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
			
            if (!Page.IsPostBack)
            {
                User objUsers;
                using (EConnectContext context = new EConnectContext())
                {
                    objUsers = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeid == 10)
                        {
                            ListItem lst = new ListItem("--Select One--", "0");
                            var Center = from t in context1.NielitCentres
                                         where t.ID == UserRefNumber
                                         orderby (t.Name)
                                         select new { ValueField = t.ID, TextField = t.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);
                           
                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (lnkID != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";                                
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);								
                            }
                        }
                        else if (UserTypeid == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                            ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);

                            btnMode.Visible = false;
                        }
                        else if (UserTypeid == 4)
                        {                           
						   var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          where s.instituteID == loginUser.UserRefNumber
                                                          select new { ID = s.ID, linkedToCentre = s.linkedToCentre };
                            if (intituteslinkedToCentre.Count() == 0)
                            {
                                ShowAlert("Menu is not available for the institute");
                                return;
                            }   
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault ().linkedToCentre );
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();                           
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);

                            btnMode.Visible = false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    //BindGridView();
                    //BindEditNewModeData();
                    //FillCourse();                   
                    //FillBatch();
                   // FillFees();
                    ShowEditMode();
                }
                else
                {					
                    BindListData();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCourse(e);
                    //FillBatch();
                   // FillFees();
                    BindGridView();
                    RdoAffInstOrNonAffInst.Enabled = true;                    
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Batch Fee", "HO/NielitCentreBatchFee.aspx?BatchID=" + Request.QueryString["BatchId"].ToString() + "&feeTypeID=" + Request.QueryString["feeTypeID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Batch Fee", "HO/NielitCentreBatchFee.aspx", ""));
                    }

                }
				
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            else
            {
                ;
                //FillCourse();
                //FillBatch();
                //FillFees();
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
//if (RdoAffInstOrNonAffInst.SelectedValue.ToString () == "2")
  //          FillCourse(e);
    }

    protected void FillddlcentreName()
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
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    ddlCenter.ClearSelection();
                    if (UserTypeid == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        //ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.instituteID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                        //ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void BindListData()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {

                ListItem lst1 = new ListItem("--All--", "0");

                var batchFee = (from s in context.NielitCentreBatchFees
                                join k in context.feeTypeMas on s.feeTypeID equals k.ID
                                select new { ValueField = k.ID, TextField = k.feeType });

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlStatusName, batchFee, lst1);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowEditMode()
    {

        try
        {

          NIELITMISContext   context = new NIELITMISContext ();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Centre Batch Fee :Update", "", ""));
            BreadCrumb1.Render();
            Int32 Feeid = Convert.ToInt32(Request.QueryString["key"]);
            if (context.NielitCentreBatchFees .Where(s => s.ID == Feeid && s.effectiveToDate == null).Count() > 0)
            {
                var batchFee = (from c in context.NielitCentreBatchFees 
                                
                              where c.ID == Feeid
                              select new
                              { 
                                 
                                  BatchID=c.batchID ,
                                  feeType=c.feeTypeID ,
                                  feeAmt = c.feeAmount,
                                  Efrm = c.effectiveFromDate

                              }).FirstOrDefault();
                btnMode.Visible = true;
                btnSubmit.Text = "Update";
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "Centre Batch Fee Details";
                Int32 centre=Convert.ToInt32 (Session ["EntityID"]);
                
                ddlCenter .Enabled =false;
                RdoAffInstOrNonAffInst.Enabled = false;
                ddlCourse.Enabled = false;
                ddlBatch.Enabled = false;
                ddlCenter.Enabled = false;
                ddlFees.Enabled = false;
                //ddlCourse.SelectedValue = batchFee.CourseDurationID.ToString();
                //Get last modified date of current record and save it in ViewState object.
                //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;

                var course = (from b in context.NielitCentreBatchs
                             where b.ID == batchFee.BatchID
                             select b).FirstOrDefault ();

                if (course.whetherAffiliated.ToString() == "Y")
                {
                    RdoAffInstOrNonAffInst.SelectedValue = "1";
                    RdoAffInstOrNonAffInst_SelectedIndexChanged(RdoAffInstOrNonAffInst, EventArgs.Empty);
                }
                else if (course.whetherAffiliated.ToString() == "N")
                {
                    RdoAffInstOrNonAffInst.SelectedValue = "0";
                    RdoAffInstOrNonAffInst_SelectedIndexChanged(RdoAffInstOrNonAffInst, EventArgs.Empty);
                }
                else
                {
                    RdoAffInstOrNonAffInst.SelectedValue = "2";
                    RdoAffInstOrNonAffInst_SelectedIndexChanged(RdoAffInstOrNonAffInst, EventArgs.Empty);
                }

                ddlCenter.SelectedValue = centre.ToString();
                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);

                ddlCourse.SelectedValue = course.CourseDurationID .ToString ();
                ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);

                ddlBatch.SelectedValue = batchFee.BatchID.ToString ();
                ddlFees . SelectedValue = batchFee.feeType .ToString();
                txtAmt.Text = batchFee.feeAmt.ToString();
                txtEFrm.Text = batchFee.Efrm.ToString("dd-MMM-yyyy");
            }
            else
            {
                Response.Redirect("NielitCentreBatchFee.aspx?msg=" + "Record not available for updation. ", true);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
          //  context.Dispose();
        }
    }

    protected void BindGridView()
    {
        try
        {
            Int32 userRefNo =Convert.ToInt32 ( Session["EntityID"]);
            using (NIELITMISContext context = new NIELITMISContext())
            {

                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                var batchFee = from s in context.NielitCentreBatchFees
                               join p in context.NielitCentreBatchs on s.batchID.ToString().Trim() equals p.ID.ToString().Trim()
                               join q in context.feeTypeMas on s.feeTypeID.ToString().Trim() equals q.ID.ToString().Trim()
                               where (p.centreID.ToString().Trim() == userRefNo.ToString().Trim() || p.subCentreID.ToString().Trim() == userRefNo.ToString().Trim())
                               orderby s.batchID, s.effectiveToDate, s.feeTypeID
                               select new
                               {
                                   ID = s.ID,
                                   batchID = p.BatchCode,
                                   feeTypeID = q.feeType,
                                   feeAmt = s.feeAmount,
                                   FeeEfrm = s.effectiveFromDate,                                   
                                   FeeTo = s.effectiveToDate
                               };

              

                if (!String.IsNullOrEmpty(searchString))
                {
                    batchFee = batchFee.Where(s => s.batchID .ToUpper().Contains(searchString) || s.feeTypeID .ToUpper().Contains (searchString ));
                }

                PagingBar1.Bind(batchFee, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();

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

    protected void FillCourse(EventArgs e)
    {
        try
        {

            ddlCenter_SelectedIndexChanged(ddlCenter, e);
        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillBatch()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var NielitBatch = from p in context.NielitCentreBatchs 
                                  where (p.centreID.ToString ().Trim () ==UserRefNumber.ToString ().Trim () || p.subCentreID.ToString().Trim() == UserRefNumber.ToString().Trim())
                                  
                               orderby (p.ID)
                               select new { ValueField = p.ID, TextField = p.BatchCode  };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch , NielitBatch, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   protected void FillFees()
    {
        try
        {
            Int64 courseid = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var feetype = from a in context.CourseFeetypes
                              where a.courseID == courseid
                              select a;
                int cnt = feetype.Count();

                if (cnt > 0)
                {
                    var feeTypes = from p in context.CourseFeetypes
                                   join q in context.feeTypeMas
                                   on p.feeTypeID equals q.ID
                                   where p.courseID == courseid
                                   orderby (p.ID)
                                   select new { ValueField = q.ID, TextField = q.feeType };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlFees, feeTypes, lst);
                }
                else
                {
                    var feeTypes = from q in context.feeTypeMas

                                   orderby (q.ID)
                                   select new { ValueField = q.ID, TextField = q.feeType };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlFees, feeTypes.Distinct(), lst);
                }                
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {		
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
			
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }			
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Centre Batch Fees";
            //Updating Breadcrumb           
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Centre Batch Fees", "#", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["BatchID"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreBatchFee.aspx?BatchID=" + Request.QueryString["BatchID"].ToString() + "&FeeTypeID=" + Request.QueryString["FeeTypeID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCentreBatchFee.aspx", true);
            }
        }
    }

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlStatusName.SelectedValue = "0";         
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    href += "&ID=" + Request.QueryString["ID"].ToString();
                }
               
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreBatchFee.aspx", true);

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (ddlCenter.SelectedIndex == 0)
            {
                lbl1.Text = "Please Select Center.";
            }

            else if (ddlCourse.SelectedIndex == 0)
            {
                lbl1.Text = "Please Select Course.";
            }

            else if (ddlBatch .SelectedIndex ==0)
            {
                lbl1.Text = "Please Select Batch.";
            }
            else if (ddlFees.SelectedIndex == 0)
            {
                lbl1.Text = "Please Select Fee Type.";
            }
            else if (String.IsNullOrEmpty(txtAmt.Text.Trim()))
            {
                lbl1.Text = "Please Enter Fee Amount.";
            }
            else if (String.IsNullOrEmpty(txtEFrm.Text))
            {
                lbl1.Text = "Please Enter Effective From date.";

            }

            else
            {
                try
                {
                    NIELITMISContext context = new NIELITMISContext();
                    NielitCentreBatchFee  batchFee;

                    Nullable<DateTime> Todate = null;
                    Int32 userid = Convert.ToInt32(Session["UserId"]);
                    DateTime entryDate = DateTime.Now;
                    Int64 batchID = Convert.ToInt64(ddlBatch.SelectedValue);
                    Int64 feeTypeID = Convert.ToInt64(ddlFees.SelectedValue);
                    Int32 amt = Convert.ToInt32(txtAmt.Text.ToString());
                    DateTime Efrm = Convert.ToDateTime(txtEFrm.Text.ToString());

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {


                        batchFee = new NielitCentreBatchFee();


                        batchFee.batchID = batchID;
                        batchFee.feeTypeID = feeTypeID;

                        var batchFeeExist = from batchFee1 in context.NielitCentreBatchFees
                                            where batchFee1.batchID.ToString().Trim() == batchID.ToString().Trim()
                                            && batchFee1.feeTypeID.ToString().Trim() == feeTypeID.ToString().Trim()
                                            && batchFee1.effectiveToDate.ToString().Trim().Length == 0

                                            select batchFee1;
                        if (batchFeeExist.Count ()>0)
                        {
                            ShowAlert("Batch fees of this category already exist, to modify fees update record");
                            return;
                        }

                        
                        batchFee.feeAmount = amt;
                        batchFee.effectiveFromDate = Efrm;
                        batchFee.effectiveToDate = Todate;
                        batchFee.enterDate = entryDate;
                        batchFee.enterBy = userid;
                        context.NielitCentreBatchFees.Add(batchFee);
                        context.SaveChanges();
                        strMessage = "New Record Saved";


                    }
                    else
                    {
                        Int32 id = Convert.ToInt32(Request.QueryString["key"]);
                        batchFee = context.NielitCentreBatchFees.Find(Convert.ToInt32(Request.QueryString["Key"]));

                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                batchFee = context.NielitCentreBatchFees.Single(reg => reg.ID == id && reg.effectiveToDate == null);
                                if (batchFee == null)
                                {
                                    ShowAlert("Already Updated record,Not allowed to change");
                                    return;
                                }
                                if (batchFee.effectiveFromDate >= Efrm)
                                {
                                    ShowAlert("Effective From cannot be less than or equal to existng date:");
                                    return;
                                }
                                if (batchFee.effectiveFromDate < Efrm && batchFee.feeAmount ==amt )
                                {
                                    ShowAlert("Amount cannot be same as existing amount");
                                    return;
                                }

                                batchFee.effectiveToDate = Efrm.AddDays(-1);
                                context.SaveChanges();



                                batchFee.feeAmount = amt;
                                batchFee.effectiveFromDate = Efrm;
                                batchFee.effectiveToDate = Todate;
                                batchFee.enterDate = entryDate;
                                batchFee.enterBy = userid;

                                context.NielitCentreBatchFees.Add(batchFee);
                                context.SaveChanges();
                                strMessage = "Record updated.";

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();  
                            }
                        }
                    }
                    Response.Redirect("NielitCentreBatchFee.aspx?msg=" + strMessage, true);
                    

                }
                catch (Exception ex)
                {
                    throw ex;

                }
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
        Int32 userRefNo = Convert.ToInt32(HttpContext.Current.Session["EntityID"]);
        Int32 userRefNo1 = Convert.ToInt32(HttpContext.Current.Session["UserRefNumber"]);
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();

            var FeeType=from s in context.feeTypeMas 
                        join f in context.NielitCentreBatchFees on s.ID equals f.feeTypeID
                       join  b in context.NielitCentreBatchs on f.batchID equals b.ID
                        where b.centreID.ToString().Trim() == userRefNo.ToString().Trim() || b.subCentreID.ToString().Trim() == userRefNo1.ToString().Trim()
                        select new { Name = s.feeType, ID=s.ID  };          
            if (!String.IsNullOrEmpty(searchString))
            {
                FeeType = FeeType.Where(s => s.Name.ToUpper().Contains(searchString));
            }    
            var batch =from b in context.NielitCentreBatchs
                       join f in context.NielitCentreBatchFees on b.ID equals f.batchID
                       where b.centreID.ToString().Trim() == userRefNo.ToString().Trim() || b.subCentreID.ToString().Trim() == userRefNo1.ToString().Trim()
                       select new { Name = b.BatchCode , ID = b.ID };
            if (!String.IsNullOrEmpty(searchString))
            {
                batch = batch.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            foreach (var c in FeeType )
            {
                items.Add(c.Name);
            }
            foreach (var c in batch.Distinct())
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

    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCourse.ClearSelection();
            ddlBatch.ClearSelection();
            ddlCourse.Items.Clear();
            ddlBatch.Items.Clear();

            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }

                        objUser = new EConnect.URM.User();
                         loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                         NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
 
                            if (UserTypeid == 10)
                            {
                                var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                                lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                            }
                            if (UserTypeid == 11)
                            {
                                var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                                lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                            }
                            if (lnkID != 0)
                            {
                                NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                //txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                using (NIELITMISContext context = new NIELITMISContext())
                                {
                                    ListItem lst1 = new ListItem("--Select One--", "99");
                                    if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                                    {
                                        ddlCenter.ClearSelection();
                                        var centreName = from s in context.AffInstitutes
                                                         where s.linkedToCentre == NelitCentreLinkId
                                                         select new { ValueField = s.instituteID, TextField = s.Name };
                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                        ddlCenter.Enabled = true;
                                    }
                                    else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                                    {
                                        if (UserTypeid == 11)//Non AffInstitutes by user refNumber
                                        {
                                            ddlCenter.ClearSelection();
                                            var centreName = from s in context.NonAffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                             select new { ValueField = s.ID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                            //ddlCenter.Enabled = false;
                                            var NonAfflcentre = (from p in context.NonAffInstitutes
                                                                 where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                                 select p).FirstOrDefault();
                                            ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                                        }
                                        else //NonAffInstitutes for Nielit Centres
                                        {
                                            ddlCourse.ClearSelection();
                                            var centreName = from s in context.NonAffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId
                                                             select new { ValueField = s.ID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                            ddlCourse.Enabled = true;
                                        }
                                        if (UserTypeid == 4)//AffInstitutes by user refNumber
                                        {
                                            ddlCenter.ClearSelection();
                                            var centreName = from s in context.AffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                             select new { ValueField = s.instituteID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                            //ddlCenter.Enabled = false;
                                            var Afflcentre = (from p in context.AffInstitutes
                                                              where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                                              select p).FirstOrDefault();
                                            ddlCenter.SelectedValue = Afflcentre.ID.ToString();
                                        }
                                        else // AffInstitutes for Nielit Centres
                                        {
                                            ddlCenter.ClearSelection();
                                            var centreName = from s in context.AffInstitutes
                                                             where s.linkedToCentre == NelitCentreLinkId
                                                             select new { ValueField = s.instituteID, TextField = s.Name };
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                            ddlCenter.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        ddlCenter.ClearSelection();
                                        ddlCenter.Items.Add(new ListItem("--Select One--", "99"));
                                        ddlCenter.SelectedValue = "99";
                                        //ddlCenter.Enabled = false;
                                    }
                                }
                            }
                            else
                            {
                                NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                //txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                using (NIELITMISContext context = new NIELITMISContext())
                                {
                                    ListItem lst1 = new ListItem("--Select One--", "99");
                                    if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                                    {
                                        ddlCourse.ClearSelection();
                                        var centreName = from s in context.AffInstitutes
                                                         where s.linkedToCentre == NelitCentreLinkId
                                                         select new { ValueField = s.instituteID, TextField = s.Name };
                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                        ddlCenter.Enabled = true;
                                    }
                                    else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                                    {
                                        ddlCenter.ClearSelection();
                                        var centreName = from s in context.NonAffInstitutes
                                                         where s.linkedToCentre == NelitCentreLinkId
                                                         select new { ValueField = s.ID, TextField = s.Name };
                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                        ddlCenter.Enabled = true;
                                    }
                                    else
                                    {
                                        ddlCenter.ClearSelection();
                                        
                                        var Center = from t in context.NielitCentres
                                                     where t.ID == UserRefNumber
                                                     orderby (t.Name)
                                                     select new { ValueField = t.ID, TextField = t.Name };
                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst1);

                                        ddlCenter.SelectedValue = NielitCentreId.ToString();
                                        ddlCenter.Enabled = false;
                                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);                                        
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

    protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Comment      
        #endregion 

        ddlCourse.ClearSelection();
        ddlBatch.ClearSelection();
        ddlCourse.Items.Clear();
        ddlBatch.Items.Clear();

        Int64 centerID = Convert.ToInt64(ddlCenter.SelectedValue);
        Int64 subcenterID = Convert.ToInt64(ddlCenter.SelectedValue);

        if (RdoAffInstOrNonAffInst.SelectedValue == "2")
        {
            string toggle = "1";
            centerID = Convert.ToInt64(ddlCenter.SelectedValue);;
            subcenterID = 0;

            using (DataTable dt = GetCourseNielitCourseRecordFee(toggle, centerID, subcenterID))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }


        }
        else
        {
            string toggle = "0";
            centerID = Convert.ToInt64(UserRefNumber);
            subcenterID = Convert.ToInt64(ddlCenter.SelectedValue);

            using (DataTable dt = GetCourseNielitCourseRecordFee(toggle, centerID, subcenterID))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }

    }

    public DataTable GetCourseNielitCourseRecordFee(string toggle, Int64 centerID, Int64 subcenterID)
    {
        //SqlParameter param;
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            
            using (SqlCommand cmd = new SqlCommand("GetNIELITMISCourseNielitCourseRecordFee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@toggle", toggle));
                cmd.Parameters.Add(new SqlParameter("@centreID", centerID));
                cmd.Parameters.Add(new SqlParameter("@subCentreID", subcenterID));
                //param = new SqlParameter("@toggle", toggle);
                //param = new SqlParameter("@centreID", centerID); 
                //cmd.Parameters.Add();
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
                con.Close();
            }
        }
        return myDt;
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {      
        Int64 subcentreid = 0;
        Int64 Courseid = 0;

        ddlBatch.ClearSelection();

        ddlBatch.Items.Clear();

        try
        {
            subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);                       
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid //&& (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                    }
                }
                else
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid 
                                            //&& (s.startDate <= System.DateTime.Now )
                                            //&& (s.endDate >= System.DateTime.Now) // comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);                       
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid  
                                            //(s.startDate <= System.DateTime.Now && 
                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst); 
                    }
                }
            };
	
        }
        catch (Exception ex)
        {
            throw ex;
        }
	FillFees();
    }
}