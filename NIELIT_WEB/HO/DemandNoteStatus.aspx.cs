using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class DemandNoteStatus : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;

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
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillApplicationTypes();
                    FillFilterPaymentMode();
                    FillFilterPaymentStatus();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillApplicationTypes();
                    FillFilterPaymentMode();
                    FillFilterPaymentStatus();
                    if (Request.QueryString["ApplTypeID"] != null || Request.QueryString["PModeID"] != null ||  Request.QueryString["PstsID"] != null)
                    {
                        ddlAppType.SelectedValue = Request.QueryString["ApplTypeID"];
                        ddlPaymentMode.SelectedValue = Request.QueryString["PModeID"];                       
                        ddlPaymentStatus.SelectedValue = Request.QueryString["PstsID"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text + "-" + "(" + ddlPaymentMode.SelectedItem.Text + ")" + "-" + "(" + ddlPaymentStatus.SelectedItem.Text + ")", "Admin/DemandNoteStatus.aspx?ApplTypeID=" + ddlAppType.SelectedValue + "&PModeID=" + ddlPaymentMode.SelectedValue + "&PstsID=" + ddlPaymentStatus.SelectedValue, ""));
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text , "Admin/DemandNoteStatus.aspx?ApplTypeID=" + ddlAppType.SelectedValue, ""));
                        BindGridView();

                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status", "Admin/DemandNoteStatus.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria For View Records";
                        lblError.Visible = true;
                    }
                    //BindGridView();
                    //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status", "Admin/DemandNoteStatus.aspx?"+Request.QueryString["Key"], ""));
                }
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
    protected void FillApplicationTypes()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var ApplicationList = from p in context.ApplicationTypes
                                      select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterPaymentMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                Int32 cheque = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
                Int32 cash = Convert.ToInt32(enmPaymentMode.Cash);
                var PaymentMode = from p in context.PaymentModes
                                  where p.ID != cheque && p.ID != cash
                                  select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPaymentMode, PaymentMode, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterPaymentStatus()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var PaymentStatus = from p in context.PaymentStatuss
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPaymentStatus, PaymentStatus, lst);
            };
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
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            btnMode.ViewMode = ToggleView.Mode.List;
            btnMode.Visible = true;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Demand Note Status";
            btnPrint.Visible = true;
            //tblNavLinks.Visible = true;
            //ddlEntity.SelectedIndex = 4;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 DemandNoteID = Convert.ToInt32(Request.QueryString["Key"]);
                hfDemandNoteID.Value = Request.QueryString["Key"].ToString();

                var DemandNote = (from p in context.DemandNotes
                                  where p.ID == DemandNoteID
                                  select new
                                  {
                                      ID = p.ID,
                                      DemandNo = p.ID,
                                      DemandDate = p.ApplicationDate,
                                      PaymentMode = p.PaymentMode.Name,
                                      PaymentStatus = p.PaymentStatus.Name,
                                      Amount=p.Amount,
                                      FeeType=p.FeeTypeID,
                                      ApplicationTypeID =p.ApplicationTypeID,
                                      DemandNoteTypeID=p.DemandNoteTypeID
                                  }).FirstOrDefault();
                lblDemandNoteNo.Text = DemandNote.DemandNo.ToString();
                lblDemandNoteDate.Text = DemandNote.DemandDate.ToString("dd-MMM-yyyy");
                lblPaymentMode.Text = DemandNote.PaymentMode.ToString();
                lblPaymentStatus.Text = DemandNote.PaymentStatus.ToString();
                lblAmount.Text = DemandNote.Amount.ToString();
                string PaymentStatus = Convert.ToString(enmPaymentStatus.Pending);
                if (DemandNote.PaymentStatus == PaymentStatus)
                {
                    btnpaynow.Visible = true;
                }
                else
                {
                    btnpaynow.Visible = false;
                }
                //string Pmode = Convert.ToString(enmPaymentMode.DemandDraft);
                if (DemandNote.PaymentMode == "DEMAND DRAFT")
                {

                    var DD =(from p in context.DemandNotes join dt in context.DemandDraftTransactions on p.ID equals dt.DemandNoteID
                             where p.ID== DemandNoteID
                             select new {ID=dt.DemandDraftNumber, Date=dt.DemandDraftDate , Bank= dt.IssuingBankName,VerifiedOn = dt.VerificatonDate}).FirstOrDefault();

                    //DemandDraftTransaction dt = context.DemandDraftTransactions.Find(ID);
                    lbltd.Visible = true;
                    lbldno.Visible = true;
                    lblddate.Visible = true;
                    lblbname.Visible = true;
                    lblVdate.Visible = true;
                    lblddno.Text = DD.ID.ToString();
                    lbldddate.Text = DD.Date.ToString("dd-MMM-yyyy");
                    lblBank.Text = DD.Bank.ToString();
                    LblVarificationDate.Text =DD.VerifiedOn.HasValue? DD.VerifiedOn.Value.ToString("dd-MMM-yyyy"):"Not Yet Verified";
                }
                else if (DemandNote.PaymentMode == "NEFT/RTGS")
                {
                    var NEFT = (from p in context.DemandNotes
                              join dt in context.NEFTTransactions on p.ID equals dt.DemandNoteID
                              where p.ID == DemandNoteID
                              select new { ID = dt.TransactionNumber, Date = dt.TransactionDate, Bank = dt.TransactionBank, VerifiedOn = dt.VerificatonDate }).FirstOrDefault();

                    //DemandDraftTransaction dt = context.DemandDraftTransactions.Find(ID);
                    lbltd.Visible = true;
                    lbldno.Visible = true;
                    lblddate.Visible = true;
                    lblbname.Visible = true;
                    lblVdate.Visible = true;
                    lblddno.Text = NEFT.ID.ToString();
                    lbldddate.Text = NEFT.Date.ToString("dd-MMM-yyyy");
                    lblBank.Text = NEFT.Bank.ToString();
                    LblVarificationDate.Text = NEFT.VerifiedOn.HasValue ? NEFT.VerifiedOn.Value.ToString("dd-MMM-yyyy") : "Not Yet Verified";
                }
                else
                {
                    lbltd.Visible = false;
                    lbldno.Visible = false;
                    lblddate.Visible = false;
                    lblbname.Visible = false;
                    lblVdate.Visible = false;
                }
                enmFeeType feetype = (enmFeeType)DemandNote.FeeType;
                lblFeeType.Text = EConnect.Utils.Common.EnumUtility.GetDescription(feetype);
                lblApplType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)DemandNote.ApplicationTypeID);
                lblDemandNoteType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmDemandNoteType)DemandNote.DemandNoteTypeID);
                //EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)p.ApplicationTypeID)
                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("DemandNote", "Admin/DemandNoteStatus.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
               
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            lblError.Visible = false;
            gvMain.Visible = true;
            Int32 ApplTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
            Int32 DemandNodeTypeID=Convert.ToInt32(enmDemandNoteType.Multiple);
            Int32 PaymentStatus = 0;
            Int32 PaymentMode = 0;
            
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            if (ddlPaymentStatus.SelectedValue != "0")
                PaymentStatus = Convert.ToInt32(ddlPaymentStatus.SelectedValue);
            if (ddlPaymentMode.SelectedValue != "0")
                PaymentMode = Convert.ToInt32(ddlPaymentMode.SelectedValue);
            //if (ddlCourseType.SelectedValue != "0")
            //    courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                var DemadNote = from s in context.DemandNotes
                                join c in context.CourseRegistrationApplications on s.ID equals c.DemandNoteID
                                where s.DemandNoteTypeID == DemandNodeTypeID 
                                select new
                                {
                                    ID = s.ID,
                                    DemandNo = s.ID,
                                    DemandDate = s.ApplicationDate,
                                    PaymentMode = s.PaymentMode.Name,
                                    PaymentModeID=s.PaymentModeID,
                                    PaymentStatus = s.PaymentStatus.Name,
                                    PaymentStatusID=s.PaymentStatusID,
                                    ApplicationTypeID = s.ApplicationTypeID,
                                    Amount=s.Amount,
                                    instituteID=c.InstituteID
                                    // ApplicationTypeID =EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)s.ApplicationTypeID)
                                };
                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (PaymentStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PaymentStatus);
                if (PaymentMode != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentModeID == PaymentMode);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                var DemadNote = (from s in context.DemandNotes
                                join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID
                                where s.DemandNoteTypeID == DemandNodeTypeID 
                                select new
                                {
                                    ID = s.ID,
                                    DemandNo = s.ID,
                                    DemandDate = s.ApplicationDate,
                                    PaymentMode = s.PaymentMode.Name,
                                    PaymentModeID = s.PaymentModeID,
                                    PaymentStatus = s.PaymentStatus.Name,
                                    PaymentStatusID = s.PaymentStatusID,
                                    ApplicationTypeID = s.ApplicationTypeID,
                                    Amount=s.Amount,
                                    instituteID = c.InstituteID
                                    // ApplicationTypeID =EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)s.ApplicationTypeID)
                                }).Distinct();
                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (PaymentStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PaymentStatus);
                if (PaymentMode != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentModeID == PaymentMode);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                var DemadNote = (from s in context.DemandNotes
                                join c in context.CourseExamApplications on s.ID equals c.DemandNoteID
                                where s.DemandNoteTypeID == DemandNodeTypeID 
                                select new
                                {
                                    ID = s.ID,
                                    DemandNo = s.ID,
                                    DemandDate = s.ApplicationDate,
                                    PaymentMode = s.PaymentMode.Name,
                                    PaymentModeID = s.PaymentModeID,
                                    PaymentStatus = s.PaymentStatus.Name,
                                    PaymentStatusID = s.PaymentStatusID,
                                    ApplicationTypeID = s.ApplicationTypeID,
                                    Amount=s.Amount,
                                    instituteID = c.InstituteID
                                    // ApplicationTypeID =EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)s.ApplicationTypeID)
                                }).Distinct();
                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (PaymentStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PaymentStatus);
                if (PaymentMode != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentModeID == PaymentMode);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //FillCourseCategory();
            //FillCourseType();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
        }
        else
        {
            Response.Redirect("DemandNoteStatus.aspx?ApplTypeID=" + Request.QueryString["TypeID"] + "&PModeID=" + Request.QueryString["PModeID"] + "&PstsID=" + Request.QueryString["PstsID"], true);
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
        //try
        //{
        //    context = new EConnectContext();
        //    //create and object 
        //    User objUser;
        //    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
        //    {

        //        strMessage = "New record saved.";
        //    }
        //    else
        //    {
        //        ////Initialize current object by loading it and get its current modified date
        //        strMessage = "Record updated.";
        //    }

        //    //Call save method
        //    //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
        //    //Redirect it to list mode
        //    Response.Redirect("CertificateCourse.aspx?msg=" + strMessage);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {

        try
        {
            if (ddlAppType.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text , "Admin/DemandNoteStatus.aspx?ApplTypeID=" + ddlAppType.SelectedValue , ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
            else if (ddlPaymentMode.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text + "-" + "-" + ddlPaymentMode.SelectedItem.Text , "Admin/DemandNoteStatus.aspx?ApplTypeID=" + ddlAppType.SelectedValue + "&PModeID=" + ddlPaymentMode.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
            else if(ddlPaymentStatus.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text + "-" + "-" + ddlPaymentMode.SelectedItem.Text + "-" + ddlPaymentStatus.SelectedItem.Text, "Admin/DemandNoteStatus.aspx?ApplTypeID=" + ddlAppType.SelectedValue + "&PModeID=" + ddlPaymentMode.SelectedValue + "&PStsID=" + ddlPaymentStatus.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
            ddlAppType.SelectedValue = "0";
            ddlPaymentMode.SelectedValue = "0";
            ddlPaymentStatus.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            //BindGridView();
            gvMain.Visible = false;
            lblError.Text = "Please Select Filter Criteria For View Records";
            lblError.Visible = true;
            uPnlGrid.Update();
            BreadCrumb1.RemoveLastBreadCrumbItem();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status", "Admin/DemandNoteStatus.aspx", ""));
            upBread.Update();
            BreadCrumb1.Render();
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
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);

                HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);

                HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var courses = from s in context.Courses
                          where s.CourseTypeID == courseType
                          select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);

            //var users1 = from s in context.Users
            //            select new { Name = s.LoginID };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            //}
            //courses = courses.Union(users1).Take(count);
            foreach (var course in courses)
            {
                items.Add(course.Name);
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
        Response.Redirect("DemandNoteStatus.aspx", true);
    }
    protected void btnpaynow_Click(object sender, EventArgs e)
    {
        Int32 ApplID = 0;
        Response.Redirect("../CAND/FrmConfirm.aspx?Appid=" + ApplID + "&TypeID=" + Request.QueryString["TypeID"] + "&DemandID=" + Request.QueryString["Key"] );
    }
    
}