using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;

public partial class Admin_CandidateFeedbackView : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
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
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddluserType, typeof(UserType), lst);
                    ddluserType.SelectedValue = "3";
                    ddluserType.Enabled = false;
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    if (!string.IsNullOrEmpty(Request.QueryString["UserTypeID"]) && !string.IsNullOrEmpty(Request.QueryString["Datefrom"]) && !string.IsNullOrEmpty(Request.QueryString["Dateto"]) && !string.IsNullOrEmpty(Request.QueryString["markRead"]))
                    {
                        ddluserType.Enabled = true;

                        ddluserType.SelectedValue = Request.QueryString["UserTypeID"];
                        ddlreadonstatus.SelectedValue = Request.QueryString["markRead"];
                        txtflFromDate.Text = Request.QueryString["Datefrom"];
                        txtToDate.Text = Request.QueryString["Dateto"];
                        BindGridView();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Feedback/Suggestions:" + txtflFromDate.Text + " To " + txtToDate.Text, "Admin/CandidateFeedbackView.aspx?UserTypeID=" + Request.QueryString["UserTypeID"] + "&Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"] + "&markRead=" + Request.QueryString["markRead"], ""));
                        BreadCrumb1.Render();
                    }
                    else
                    {
                        BindGridView();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Feedback/Suggestions:" + txtflFromDate.Text + " To " + txtToDate.Text, "Admin/CandidateFeedbackView.aspx", ""));
                        BreadCrumb1.Render();
                    }
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                Int32 usertypeID = 0;
                string readonstatus = "";
                if (ddluserType.SelectedValue != "0")
                    usertypeID = Convert.ToInt32(ddluserType.SelectedValue);
                if (ddlreadonstatus.SelectedValue != "0")
                    readonstatus = Convert.ToString(ddlreadonstatus.SelectedItem.Text);
                var feedback = from s in context.Feedbacks
                               join u in context.Users
                               on s.UserID equals u.UserRefNumber
                               where System.Data.Entity.DbFunctions.TruncateTime(s.Date) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate) && System.Data.Entity.DbFunctions.TruncateTime(s.Date) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                && s.UserTypeID == usertypeID
                               select new
                               {
                                   ID = s.ID,
                                   userName = u.UserName,
                                   usertype = u.UserType.Name,
                                   feedback = (!string.IsNullOrEmpty(s.FeedbackDescription)) ? "Yes" : "No",
                                   suggestions = (!string.IsNullOrEmpty(s.Suggestions)) ? "Yes" : "No",
                                   Date = s.Date,
                                   Isreadon = s.IsMarkedRead ? "Yes" : "No",
                                   userTypeID = u.UserTypeID
                               };
                if (readonstatus.ToString() != "")
                {
                    feedback = feedback.Where(s => s.Isreadon.ToUpper() == readonstatus.ToUpper());
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "userName":
                            if (sortOrder == "DESC")
                                feedback = feedback.OrderByDescending(s => s.userName);
                            else
                                feedback = feedback.OrderBy(s => s.userName);
                            break;
                        case "usertype":
                            if (sortOrder == "DESC")
                                feedback = feedback.OrderByDescending(s => s.usertype);
                            else
                                feedback = feedback.OrderBy(s => s.usertype);
                            break;
                        case "feedback":
                            if (sortOrder == "DESC")
                                feedback = feedback.OrderByDescending(s => s.feedback);
                            else
                                feedback = feedback.OrderBy(s => s.feedback);
                            break;
                        case "suggestions":
                            if (sortOrder == "DESC")
                                feedback = feedback.OrderByDescending(s => s.suggestions);
                            else
                                feedback = feedback.OrderBy(s => s.suggestions);
                            break;
                        case "Date":
                            if (sortOrder == "DESC")
                                feedback = feedback.OrderByDescending(s => s.Date);
                            else
                                feedback = feedback.OrderBy(s => s.Date);
                            break;
                        default:
                            feedback = feedback.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(feedback, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                ShowDetailFeedback();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError1.Visible = true;
                    btnPrint.Visible = false;
                    BtnMark.Visible = false;
                    lblError1.Text = "No record found. Please select filter criteria to  display users feedback/suggestions records";
                }
                else
                {
                    lblError1.Visible = false;
                    lblError1.Text = "";
                    btnPrint.Visible = true;
                    BtnMark.Visible = true;

                }
                btnPrint.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("CandidateFeedbackViewReport.aspx?UserTypeID=" + ddluserType.SelectedValue + "&Datefrom=" + txtflFromDate.Text + "&Dateto=" + txtToDate.Text + "&Type=Print") + "');");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnCancel.Visible = true;
            lblHeading.Text = "Feedback/Suggestions Details";
            using (EConnectContext context = new EConnectContext())
            {
                Int32 fid = Convert.ToInt32(Request.QueryString["Key"]);
                var feedback = (from s in context.Feedbacks
                                join u in context.Users
                                on s.UserID equals u.UserRefNumber
                                where s.ID == fid
                                select new
                                {
                                    userName = u.UserName,
                                    usertype = u.UserType.Name,
                                    feedback = (!string.IsNullOrEmpty(s.FeedbackDescription)) ? s.FeedbackDescription : "",
                                    suggestions = (!string.IsNullOrEmpty(s.Suggestions)) ? s.Suggestions : "",
                                    Date = s.Date,
                                    ismark = s.IsMarkedRead
                                }).FirstOrDefault();
                tdUserName.InnerHtml = GetInitCap(feedback.userName);
                tdUserType.InnerHtml = GetInitCap(feedback.usertype);
                tdSuggDate.InnerHtml = feedback.Date.ToString("dd-MMM-yyyy");
                tdFeedback.InnerHtml = GetInitCap(feedback.feedback);
                tdsuggestion.InnerHtml = GetInitCap(feedback.suggestions);
                if (feedback.ismark == true)
                {
                    lblMark.Visible = true;
                    lblMark.Text = "This record is marked as read ";
                    btnMarkOnUpdate.Text = "Mark As Unread";
                }
                else
                {
                    lblMark.Visible = false;
                    btnMarkOnUpdate.Text = "Mark As Read";
                }

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Feedback/Suggestions Detail", "", ""));
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
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.New))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to add new record.", true);
                    return;
                }

            }
            else
            {
                // Response.Redirect("Message.aspx", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Feedback/Suggestions:" + txtflFromDate.Text + " To " + txtToDate.Text, "Admin/CandidateFeedbackView.aspx?UserTypeID=" + Request.QueryString["UserTypeID"] + "&Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"] + "&markRead=" + Request.QueryString["markRead"], ""));
            BreadCrumb1.Render();
            upBread.Update();
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
            ddluserType.SelectedValue = "0";
            txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ddlreadonstatus.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("CandidateFeedbackView.aspx");
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
    //protected void PerformPopupAction(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
    //            {
    //                BreadCrumb1.Render();
    //                ShowAlert("Sorry! You don't have rights to delete the records.", true);
    //                return;
    //            }
    //            ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
    //            context.ExamCenters.Remove(examcenter);
    //            context.SaveChanges();
    //            BindGridView();
    //            ShowAlert("Record deleted successfully.", true);
    //            hfActionID.Value = "";
    //        };
    //        uPnlGrid.Update();
    //    }
    //    catch (Exception ex)
    //    {
    //        BindGridView();
    //        uPnlGrid.Update();
    //        ShowAlert("Record can not be deleted!", true);
    //    }
    //}
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text + "&markRead=" + ddlreadonstatus.SelectedValue);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text + "&markRead=" + ddlreadonstatus.SelectedValue);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text + "&markRead=" + ddlreadonstatus.SelectedValue);
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h4.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text + "&markRead=" + ddlreadonstatus.SelectedValue);
                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h5.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text + "&markRead=" + ddlreadonstatus.SelectedValue);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("CandidateFeedbackView.aspx?UserTypeID=" + Request.QueryString["UserTypeID"] + "&Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"] + "&markRead=" + ddlreadonstatus.SelectedValue, true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BtnMark_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 FeebbackID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            FeebbackID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                            Feedback feedback = context.Feedbacks.Find(FeebbackID);
                            if (feedback != null)
                            {
                                feedback.IsMarkedRead = true;
                                context.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            FeebbackID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                            Feedback feedback = context.Feedbacks.Find(FeebbackID);
                            if (feedback != null)
                            {
                                feedback.IsMarkedRead = false;
                                context.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }
                ShowAlert("Data Updated successfully.", true);
            };
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowDetailFeedback()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                Int64 FeedBackID = 0;
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        FeedBackID = Convert.ToInt32(gvMain.DataKeys[i].Values[0]);
                        Feedback feedback = context.Feedbacks.Find(FeedBackID);
                        if (feedback.IsMarkedRead)
                        {
                            cbx.Checked = true;
                        }
                    }
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnMarkOnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 FeebbackID = Convert.ToInt64(Request.QueryString["Key"]);
            if (btnMarkOnUpdate.Text == "Mark As Unread")
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Feedback feedback = context.Feedbacks.Find(FeebbackID);
                    if (feedback != null)
                    {
                        feedback.IsMarkedRead = false;
                        context.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    ShowAlert("Data Unmarked successfully.", true);
                };
            }
            else
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Feedback feedback = context.Feedbacks.Find(FeebbackID);
                    if (feedback != null)
                    {
                        feedback.IsMarkedRead = true;
                        context.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    ShowAlert("Data Marked successfully.", true);
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}