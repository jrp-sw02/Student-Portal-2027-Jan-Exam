using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;
using System.Data.Entity.SqlServer;

public partial class Admin_Message : BasePage
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
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();

                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    lblError1.Visible = true;
                    lblError1.Text = "Please select filter criteria for display SMS/Email records.";
                    if (!string.IsNullOrEmpty(Request.QueryString["stype"]) && !string.IsNullOrEmpty(Request.QueryString["Datefrom"]) && !string.IsNullOrEmpty(Request.QueryString["Dateto"]))
                    {
                        lblError1.Visible = false;
                        lblError.Visible = false;
                        ddlflserviceType.SelectedValue = Request.QueryString["stype"];
                        //&& !string.IsNullOrEmpty(Request.QueryString["Datefrom"]) && !string.IsNullOrEmpty(Request.QueryString["Dateto"])
                        txtflFromDate.Text = Request.QueryString["Datefrom"];
                        txtToDate.Text = Request.QueryString["Dateto"];
                        BindGridView();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        //UpdatePanel1.Update();
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("SMS/Email Service", "Admin/Message.aspx?stype=" + Request.QueryString["stype"] + "&Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"], ""));
                        BreadCrumb1.Render();



                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        //UpdatePanel1.Update();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("SMS/Email Service", "Admin/Message.aspx", ""));
                        BreadCrumb1.Render();
                        //UpdatePanel1.Update();

                    }
                    if (rblServiceType.SelectedValue == "1")
                    {
                        tblsms.Visible = true;
                        tblEmail.Visible = false;
                        btnSendSms.Visible = true;
                        btnEmail.Visible = false;
                        btnCancel.Visible = true;
                        //BindGridView();
                    }
                    else if (rblServiceType.SelectedValue == "2")
                    {
                        tblsms.Visible = false;
                        tblEmail.Visible = true;
                        btnSendSms.Visible = false;
                        btnEmail.Visible = true;
                        btnCancel.Visible = true;
                    }
                    //Get last modified date of current record and save it in ViewState object.
                    ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                    //Create an object of record to be modified and assign properties to relevant fields.
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 smsbox = txtSMSMessage.Text.Length;
            if (smsbox > 160)
            {
                ShowAlert("Message length can not be greater than 160 characters.", true);
                txtSMSMessage.Focus();
                return;
            }
            else
            {
                string mobileMsg = txtSMSMessage.Text.ToString();
                EConnect.NIELIT.SMS message = new SMS(mobileMsg, txtSMSMobiles.Text.ToString(),"test", SmsServiceType.BulkSMS, true);
                int sentMessageCount;
                message.sendSingleSMS(out sentMessageCount);
                //string msg = "";
                //message.Send(out msg);
                lblError.Visible = true;
                lblError.Text = sentMessageCount.ToString() + " sms sent successfully";
                Clearcontrols();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    protected void rblServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblServiceType.Text = "Service Type: " + rblServiceType.SelectedItem.Text;
        BreadCrumb1.Render();
        //SMS
        if (rblServiceType.SelectedValue == "1")
        {
            lblError.Visible = false;
            Clearcontrols();
            tblEmail.Visible = false;
            tblsms.Visible = true;
            btnSendSms.Visible = true;
            btnEmail.Visible = false;
            btnCancel.Visible = true;
        }
        else
        {
            lblError.Visible = false;
            Clearcontrols();
            tblEmail.Visible = true;
            tblsms.Visible = false;
            btnSendSms.Visible = false;
            btnEmail.Visible = true;
            btnCancel.Visible = true;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Message.aspx");
    }
    protected void Clearcontrols()
    {
        txtSMSMessage.Text = "";
        txtSMSMobiles.Text = "";
        txtEmailToAddress.Text = "";
        txtSubject.Text = "";
        txtMsg.Text = "";
    }
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        string emailAddress = null;
        string msg = txtMsg.Text.ToString();
        try
        {
            string[] EmailArr = null;
            int count = 0;
            emailAddress = txtEmailToAddress.Text.ToString();
            EmailArr = emailAddress.Split(',');
            for (count = 0; count <= EmailArr.Length - 1; count++)
            {
                EConnect.NIELIT.Email mail = new Email(txtSubject.Text.ToString(), msg, EmailArr[count].ToString().Trim(), true);
                mail.Send();
            }
            lblError.Visible = true;
            lblError.Text = (count).ToString() + " email(s) send successfully.";
            Clearcontrols();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    protected void BindGridView()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
                DateTime todate = Convert.ToDateTime(txtToDate.Text);
                HiddenField1.Value = ddlflserviceType.SelectedValue;
                ucSearchBar.AutoCompleteContextKey = HiddenField1.Value;
                if (ddlflserviceType.SelectedValue == "1")
                {

                    var SmsData = from s in context.SentSMS
                                  where System.Data.Entity.DbFunctions.TruncateTime(s.SentOn) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate) && System.Data.Entity.DbFunctions.TruncateTime(s.SentOn) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                  select new
                                  {
                                      ID = s.ID,
                                      Mobile = SqlFunctions.StringConvert((double)s.MobileNumber),
                                      Message = s.Message.Substring(0, 55) + ".........",
                                      Date = s.SentOn,
                                      Status = s.ResponseID,
                                      stype = 1
                                  };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        SmsData = SmsData.Where(s => s.Mobile.Contains(searchString));
                    }
                    if (SmsData.Count() > 0)
                    {
                        gvMain.Visible = true;
                        uPnlGrid.Update();
                        gvEmail.Visible = false;
                        uPnlGrid.Update();
                        lblError1.Visible = false;
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        SmsData = SmsData.OrderByDescending(s => s.ID);
                                    else
                                        SmsData = SmsData.OrderBy(s => s.ID);
                                    break;
                                case "Mobile":
                                    if (sortOrder == "DESC")
                                        SmsData = SmsData.OrderByDescending(s => s.Mobile);
                                    else
                                        SmsData = SmsData.OrderBy(s => s.Mobile);
                                    break;
                                case "Message":
                                    if (sortOrder == "DESC")
                                        SmsData = SmsData.OrderByDescending(s => s.Message);
                                    else
                                        SmsData = SmsData.OrderBy(s => s.Message);
                                    break;
                                case "Date":
                                    if (sortOrder == "DESC")
                                        SmsData = SmsData.OrderByDescending(s => s.Date);
                                    else
                                        SmsData = SmsData.OrderBy(s => s.Date);
                                    break;
                                case "Status":
                                    if (sortOrder == "DESC")
                                        SmsData = SmsData.OrderByDescending(s => s.Status);
                                    else
                                        SmsData = SmsData.OrderBy(s => s.Status);
                                    break;
                                default:
                                    SmsData = SmsData.OrderBy(s => s.Mobile);
                                    break;
                            }
                        }
                        PagingBar1.Bind(SmsData, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                    }
                    else
                    {
                        gvMain.Visible = false;
                        uPnlGrid.Update();
                        gvEmail.Visible = false;
                        uPnlGrid.Update();
                        lblError1.Visible = true;
                        lblError1.Text = "No record found";


                    }
                }
                else if (ddlflserviceType.SelectedValue == "2")
                {

                    gvMain.Visible = false;
                    gvEmail.Visible = true;
                    var EmailData = from s in context.SentEmails
                                    where System.Data.Entity.DbFunctions.TruncateTime(s.SentOn) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate) && System.Data.Entity.DbFunctions.TruncateTime(s.SentOn) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                    select new
                                    {
                                        ID = s.ID,
                                        Subject = s.Subject,
                                        Email = s.EmailAddress,
                                        Date = s.SentOn,
                                        stype = 2
                                    };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        EmailData = EmailData.Where(s => s.Email.ToUpper().Contains(searchString));
                    }
                    if (EmailData.Count() > 0)
                    {
                        gvMain.Visible = false;
                        uPnlGrid.Update();
                        gvEmail.Visible = true;
                        uPnlGrid.Update();
                        lblError1.Visible = false;
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        EmailData = EmailData.OrderByDescending(s => s.ID);
                                    else
                                        EmailData = EmailData.OrderBy(s => s.ID);
                                    break;
                                case "Subject":
                                    if (sortOrder == "DESC")
                                        EmailData = EmailData.OrderByDescending(s => s.Subject);
                                    else
                                        EmailData = EmailData.OrderBy(s => s.Subject);
                                    break;
                                case "Email":
                                    if (sortOrder == "DESC")
                                        EmailData = EmailData.OrderByDescending(s => s.Email);
                                    else
                                        EmailData = EmailData.OrderBy(s => s.Email);
                                    break;
                                case "Date":
                                    if (sortOrder == "DESC")
                                        EmailData = EmailData.OrderByDescending(s => s.Date);
                                    else
                                        EmailData = EmailData.OrderBy(s => s.Date);
                                    break;
                                default:
                                    EmailData = EmailData.OrderBy(s => s.Email);
                                    break;
                            }
                        }
                        PagingBar1.Bind(EmailData, ref gvEmail);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                    }
                    else
                    {
                        gvMain.Visible = false;
                        uPnlGrid.Update();
                        gvEmail.Visible = false;
                        uPnlGrid.Update();
                        lblError1.Visible = true;
                        lblError1.Text = "No record found";
                    }
                }

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
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            //BindEditNewModeData();
            // ddlExamCentreType.SelectedValue = "3";
            rblServiceType.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "SMS/Email Service";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New SMS/Email", "", ""));
        }
        else
        {
            Response.Redirect("Message.aspx?stype=" + Request.QueryString["stype"] + "&Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"], true);
            //Response.Redirect("Message.aspx", true);
        }

    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
            ddlflserviceType.SelectedValue = "0";
            txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            gvMain.Visible = false;
            //uPnlGrid.Update();
            //lblError1.Visible = true;
            //lblError1.Text = "Please select filter criteria for display SMS/Email records.";
            //uPnlGrid.Update();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("Message.aspx");
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
    protected void gvEmail_Sorting(object sender, GridViewSortEventArgs e)
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
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.ExamCenters.Remove(examcenter);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h4.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                //HyperLink h5 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h4.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
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
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            if (CommonFunctions.IsNumeric(searchString))
            {
                //Int64 mobilen = Convert.ToInt64(searchString);
                var mobilesms = from s in context.SentSMS
                                select new { Name = SqlFunctions.StringConvert((double)s.MobileNumber) };
                if (!String.IsNullOrEmpty(searchString))
                {
                    mobilesms = mobilesms.Where(s => s.Name.Contains(searchString));
                }
                mobilesms = mobilesms.OrderBy(s => s.Name).Distinct();
                foreach (var c in mobilesms)
                {
                    items.Add(c.Name.ToString());
                }
            }
            else
            {
                var emailmsg = from s in context.SentEmails
                               select new { Name = s.EmailAddress };
                if (!String.IsNullOrEmpty(searchString))
                {
                    emailmsg = emailmsg.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                emailmsg = emailmsg.OrderBy(s => s.Name).Distinct();
                foreach (var c in emailmsg)
                {
                    items.Add(c.Name.ToString());
                }
            }

            return items.ToArray();

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void bbtnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        Response.Redirect("Message.aspx?stype=" + Request.QueryString["stype"] + "&Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"], true);
    }
    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            Int32 serviceType = Convert.ToInt32(Request.QueryString["stype"]);
            if (serviceType == 1)
            {
                tblsms.Visible = true;
                tblEmail.Visible = false;
                btnResendSMS.Visible = true;
                btnResendEmail.Visible = false;
                lblServiceType.Text = "<b>Service Type: SMS</b>";
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 msgId = Convert.ToInt32(Request.QueryString["Key"]);
                    var mobileMsg = (from p in context.SentSMS
                                     where p.ID == msgId
                                     select p).FirstOrDefault();
                    txtSMSMessage.Text = mobileMsg.Message.ToString();
                    txtSMSMobiles.Text = mobileMsg.MobileNumber.ToString();
                    lblDate.Text = "<b>SMS sent on :</b>" + mobileMsg.SentOn.ToString("dd-MMM-yyyy") + " <b>at </b> :" + mobileMsg.SentOn.ToLongTimeString();
                    if (mobileMsg.ResponseID == 402)
                    {
                        lblstatus.Text = "<b>Status : </b> Successfully sent ";
                    }
                    else
                    {
                        lblstatus.Text = "<b>Status : </b> Failed";
                    }
                };
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("SMS:Detail", "", ""));
                BreadCrumb1.Render();
            }
            else if (serviceType == 2)
            {
                btnResendSMS.Visible = false;
                btnResendEmail.Visible = true;
                tblsms.Visible = false;
                tblEmail.Visible = true;
                lblServiceType.Text = "<b>Service Type: EMAIL</b>";
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 emailId = Convert.ToInt32(Request.QueryString["Key"]);
                    var emailMsg = (from p in context.SentEmails
                                    where p.ID == emailId
                                    select p).FirstOrDefault();
                    tdEmailToAddress.InnerHtml = emailMsg.EmailAddress.ToString();
                    tdSubject.InnerHtml = emailMsg.Subject.ToString();
                    tdMsg.InnerHtml = emailMsg.Message.ToString().Replace("<br>", "");
                    lblDateEmail.Text = "<b>EMAIL sent on :</b>" + emailMsg.SentOn.ToString("dd-MMM-yyyy") + " <b>at </b> :" + emailMsg.SentOn.ToLongTimeString();
                    lblTotalEmail.Text = "Total Email will be sent: 1";
                };
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Email:Detail", "", ""));
                BreadCrumb1.Render();
            }

            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.

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

    protected void btnResendSMS_Click(object sender, EventArgs e)
    {

        try
        {
            BreadCrumb1.Render();
            Int32 smsbox = txtSMSMessage.Text.Length;
            if (smsbox > 160)
            {
                ShowAlert("Message length can not be greater than 160 characters.", true);
                txtSMSMessage.Focus();
                return;
            }
            else
            {
                string mobileMsg = txtSMSMessage.Text.ToString();
                EConnect.NIELIT.SMS message = new SMS(mobileMsg, txtSMSMobiles.Text.ToString(),"test", SmsServiceType.BulkSMS, true);
                int sentMessageCount;
                message.sendSingleSMS(out sentMessageCount);
                //lblError.Visible = true;
                //lblError.Text = sentMessageCount.ToString() + " sms sent successfully";
                ShowAlert("SMS resent successfully", true);
                btnResendSMS.Visible = false;
                //Response.Redirect("Message.aspx?stype="+ Request.QueryString["stype"] + "&Key="+Request.QueryString["Key"]);
                //Clearcontrols();
            }
        }
        catch (Exception ex)
        {
            //lblError.Text = ex.Message;
            //lblError.Visible = true;
            ShowAlert(ex.Message);
        }
    }
    protected void btnResendEmail_Click(object sender, EventArgs e)
    {
        string emailAddress = null;
        string msg = null;
        string Subj = null;
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 emailId = Convert.ToInt32(Request.QueryString["Key"]);
                var emailMsg = (from p in context.SentEmails
                                where p.ID == emailId
                                select p).FirstOrDefault();
                emailAddress = emailMsg.EmailAddress;
                msg = emailMsg.Message;
                Subj = emailMsg.Subject;

            };

            EConnect.NIELIT.Email mail = new Email(Subj, msg, emailAddress.ToString().Trim(), true);
            mail.Send();
            ShowAlert("Email resent successfully", true);
            btnResendEmail.Visible = false;

            //Response.Redirect("Message.aspx?stype=" + Request.QueryString["stype"] + "&Key=" + Request.QueryString["Key"]);

        }
        catch (Exception ex)
        {
            //lblError.Text = ex.Message;
            //lblError.Visible = true;
            ShowAlert(ex.Message);
        }
    }
}