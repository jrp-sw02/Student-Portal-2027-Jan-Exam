using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class ConfigurationManagement : BasePage
{
    String strMessage = string.Empty;   
    Int32 currentRoleId = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    btnSave.Visible = false;
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Configuration Management", "ConfigurationManagement.aspx", ""));
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    
    protected void ShowDetailPaymentMode()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                Int64 PaymentModeID = 0;
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chkShow");
                    if (cbx != null)
                    {
                        PaymentModeID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                        PaymentMode pMode = context.PaymentModes.Find(PaymentModeID);
                        if (pMode.Visible == true)
                        {
                            cbx.Checked = true;
                        }
                        
                    }
                }
               
            };
            gvMain.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowDetailEvents()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                Int64 EventID = 0;
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbxsms = (CheckBox)gvMain.Rows[i].FindControl("chksms");
                    CheckBox cbxEmail = (CheckBox)gvMain.Rows[i].FindControl("chkEmail");
                    if (cbxsms != null)
                    {
                        EventID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                        NotificationEvent eMode = context.NotificationEvent.Find(EventID);
                        if (eMode.SendSms)
                        {
                            cbxsms.Checked = true;
                        }
                    }
                    if (cbxEmail != null)
                    {
                        EventID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                        NotificationEvent eMode = context.NotificationEvent.Find(EventID);
                        if (eMode.SendEmail)
                        {
                            cbxEmail.Checked = true;
                        }
                    }
                }

            };
            gvMain.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ShowDetailFileds()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                Int64 FieldID = 0;
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbxupdate = (CheckBox)gvMain.Rows[i].FindControl("chkupdate");
                    if (cbxupdate != null)
                    {
                        FieldID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                        UpdateFields eMode = context.UpdateFields.Find(FieldID);
                        if (eMode.IsUpdate)
                        {
                            cbxupdate.Checked = true;
                        }
                    }
                }

            };
            gvMain.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGridView()
    {
        try
        {
            Int32 PaymentMode = Convert.ToInt32(ConfigurationType.PaymentMode);
            Int32 MultiCity = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
            Int32 Cash = Convert.ToInt32(enmPaymentMode.Cash);
            if (Convert.ToInt32(rblConfigurationtype.SelectedValue) == 1)
            {
                btnSave.Visible = true;
                gvMain.Columns[1].HeaderText = "Payment Mode";
                gvMain.Columns[2].Visible = false;
                gvMain.Columns[3].Visible = true;
                gvMain.Columns[4].Visible = false;
                gvMain.Columns[5].Visible = false;
                using (EConnectContext context = new EConnectContext())
                {
                    string sortOrder = ViewState["SortOrder"].ToString();
                    string sortField = ViewState["SortField"].ToString();
                    var filldata = from s in context.PaymentModes
                                   where s.ID != MultiCity && s.ID != Cash
                                   select new
                                   {
                                       ID = s.ID,
                                       Name = s.Name,
                                       ShowData = s.Visible,
                                   };
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    filldata = filldata.OrderByDescending(s => s.ID);
                                else
                                    filldata = filldata.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    filldata = filldata.OrderByDescending(s => s.Name);
                                else
                                    filldata = filldata.OrderBy(s => s.Name);
                                break;
                            default:
                                filldata = filldata.OrderBy(s => s.Name);
                                break;
                        }
                    }
                    PagingBar1.Bind(filldata, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    ShowDetailPaymentMode();
                };
            }
            else if (Convert.ToInt32(rblConfigurationtype.SelectedValue) == 2)
            {
                if (gvMain.Rows.Count != 0)
                {
                    btnSave.Visible = true;
                }
                gvMain.Columns[1].HeaderText = "Event Name";
                gvMain.Columns[2].Visible = false;
                gvMain.Columns[3].Visible = false;
                gvMain.Columns[4].Visible = true;
                gvMain.Columns[5].Visible = true;
                using (EConnectContext context = new EConnectContext())
                {
                    string sortOrder = ViewState["SortOrder"].ToString();
                    string sortField = ViewState["SortField"].ToString();
                    var filldata = from s in context.NotificationEvent
                                   where s.ID >4
                                   select new
                                   {
                                       ID = s.ID,
                                       Name = s.EventName,
                                       SendSMS = s.SendSms,
                                       SendEmail=s.SendEmail
                                   };
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    filldata = filldata.OrderByDescending(s => s.ID);
                                else
                                    filldata = filldata.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    filldata = filldata.OrderByDescending(s => s.Name);
                                else
                                    filldata = filldata.OrderBy(s => s.Name);
                                break;
                            default:
                                filldata = filldata.OrderBy(s => s.Name);
                                break;
                        }
                    }
                    PagingBar1.Bind(filldata, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    ShowDetailEvents();
                };
            }
            else if (Convert.ToInt32(rblConfigurationtype.SelectedValue) == 3)
            {
                if (gvMain.Rows.Count != 0)
                {
                    btnSave.Visible = true;
                }
                gvMain.Columns[1].HeaderText = "Field Name";
                gvMain.Columns[2].Visible = true;
                gvMain.Columns[3].Visible = false;
                gvMain.Columns[4].Visible = false;
                gvMain.Columns[5].Visible = false;
                using (EConnectContext context = new EConnectContext())
                {
                    string sortOrder = ViewState["SortOrder"].ToString();
                    string sortField = ViewState["SortField"].ToString();
                    var filldata = from s in context.UpdateFields
                                   select new
                                   {
                                       ID = s.ID,
                                       Name = s.FieldName,
                                       IsUpdate = s.IsUpdate
                                   };
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    filldata = filldata.OrderByDescending(s => s.ID);
                                else
                                    filldata = filldata.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    filldata = filldata.OrderByDescending(s => s.Name);
                                else
                                    filldata = filldata.OrderBy(s => s.Name);
                                break;
                            default:
                                filldata = filldata.OrderBy(s => s.Name);
                                break;
                        }
                    }
                    PagingBar1.Bind(filldata, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    ShowDetailFileds();
                };
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        //if (btnMode.ViewMode == ToggleView.Mode.New)
        //{
        //    btnMode.ViewMode = ToggleView.Mode.List;
        //    mltvTab.ActiveViewIndex = 1;
        //    pnlFilter.Visible = false;
        //    ucSearchBar.Visible = false;
        //    //Change the heading text as required
        //    lblHeading.Text = "Exam Centres";
        //    //Updating Breadcrumb
        //    //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
        //    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "", ""));
        //}
        //else
        //{
        //    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        //    {
        //        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentres.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString()), true);
        //    }
        //    else
        //    {
        //        Response.Redirect("ExamCentres.aspx", true);
        //    }
        //}
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
    protected void ResetFilterPanel(object sender, EventArgs e)
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
            //    function PerformAction(obj, tableid) {
            //        document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            //        ShowHideMenu(obj, tableid);
            //    }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
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
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var Examcentre = from s in context.ExamCenters
                             select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                Examcentre = Examcentre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            Examcentre = Examcentre.OrderBy(s => s.Name);
            var ExamCode = from c in context.ExamCenters
                           select new { Name = c.Code };
            if (!String.IsNullOrEmpty(searchString))
            {
                ExamCode = ExamCode.Where(c => c.Name.ToUpper().Contains(searchString));
            }
            Examcentre = Examcentre.Union(ExamCode).Take(count);
            foreach (var c in Examcentre)
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
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 PaymentMode = Convert.ToInt32(ConfigurationType.PaymentMode);
            if (Convert.ToInt32(rblConfigurationtype.SelectedValue) == PaymentMode)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 PaymentModeID = 0;
                    for (int i = 0; i < gvMain.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chkShow");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                PaymentModeID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                                PaymentMode pMode = context.PaymentModes.Find(PaymentModeID);
                                if (pMode != null)
                                {
                                    pMode.Visible = true;
                                    context.Entry(pMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                PaymentModeID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                                PaymentMode pMode = context.PaymentModes.Find(PaymentModeID);
                                if (pMode != null)
                                {
                                    pMode.Visible = false;
                                    context.Entry(pMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    ShowAlert("Data Updated successfully.", true);
                };
            }
            else if(rblConfigurationtype.SelectedValue == "2")
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 EventID = 0;
                    for (int i = 0; i < gvMain.Rows.Count; i++)
                    {
                        CheckBox cbxsms = (CheckBox)gvMain.Rows[i].FindControl("chksms");
                        CheckBox cbxEmail = (CheckBox)gvMain.Rows[i].FindControl("chkEmail");
                        if (cbxsms != null)
                        {
                            if (cbxsms.Checked)
                            {
                                EventID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                                NotificationEvent nMode = context.NotificationEvent.Find(EventID);
                                if (nMode != null)
                                {

                                    nMode.SendSms = true;
                                    context.Entry(nMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                EventID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                                NotificationEvent nMode = context.NotificationEvent.Find(EventID);
                                if (nMode != null)
                                {
                                    nMode.SendSms = false;
                                    context.Entry(nMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                        if (cbxEmail != null)
                        {
                            if (cbxEmail.Checked)
                            {
                                EventID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                                NotificationEvent nMode = context.NotificationEvent.Find(EventID);
                                if (nMode != null)
                                {

                                    nMode.SendEmail = true;
                                    context.Entry(nMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                EventID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                                NotificationEvent nMode = context.NotificationEvent.Find(EventID);
                                if (nMode != null)
                                {
                                    nMode.SendEmail = false;
                                    context.Entry(nMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    ShowAlert("Data Updated successfully.", true);
                };
            }
            else if (rblConfigurationtype.SelectedValue == "3")
            {

                using (EConnectContext context = new EConnectContext())
                {
                    Int64 FieldID = 0;
                    for (int i = 0; i < gvMain.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chkupdate");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                FieldID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                                UpdateFields uMode = context.UpdateFields.Find(FieldID);
                                if (uMode != null)
                                {
                                    uMode.IsUpdate = true;
                                    context.Entry(uMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                FieldID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                                UpdateFields uMode = context.UpdateFields.Find(FieldID);
                                if (uMode != null)
                                {
                                    uMode.IsUpdate = false;
                                    context.Entry(uMode).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    ShowAlert("Data Updated successfully.", true);
                };

            }
            BindGridView();
            gvMain.Visible = true;
        }
       
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void rblConfigurationtype_SelectedIndexChanged(object sender, EventArgs e)
       {
           BindGridView();
       }
       
}