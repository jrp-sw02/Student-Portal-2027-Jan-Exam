using AjaxControlToolkit;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class SamplePageHO : BasePage
{
    string strMessage = string.Empty;
    EConnectContext context;
    String currentRoleName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        //Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleName = (string)Session["RoleName"];
            if (currentRoleName == "Technical Support Query")
            {
                //Response.Write("Sorry! You don't have rights  to view this page");
                btnSave.Visible = false;
                //Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();

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
    protected void ShowEditMode()
    {

        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Contact Detail:Update", "", ""));
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            Int32 phoneNumber = Convert.ToInt32(Request.QueryString["PhoneNo"]);
            var student = (from s in context.Candidates
                           join ccd in context.CandidateContactDetails on s.ID equals ccd.CandidateID
                           where (ccd.PhoneNumber.HasValue ? ccd.PhoneNumber.Value : 0) == phoneNumber && s.ID == Appid
                           select new
                           {
                               phoneno = (ccd.PhoneNumber.HasValue) ? ccd.PhoneNumber.Value : 0,
                               mobileno = (ccd.MobileNumber.HasValue) ? ccd.MobileNumber.Value : 0,
                               email = ccd.EmailAddress,
                               effdate = ccd.EffectiveFromDate
                           }).FirstOrDefault();
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Update Contact Detail";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            txtPhoneno.Text = student.phoneno.ToString();
            Txt_Mobno.Text = student.mobileno.ToString();
            Txt_MailId.Text = student.email;
            Txt_EffDate.Text = student.effdate.ToString("dd-MMM-yyyy");
            Txt_EffDate.Enabled = false;
            ViewState["SortOrder1"] = "";
            ViewState["SortField1"] = "";
            Bindoldgridview();
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
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            ucSearchBar.AutoCompleteContextKey = Appid.ToString();
            var student = from c in context.CandidateContactDetails
                          where c.CandidateID == Appid
                          orderby c.EffectiveFromDate descending
                          select new
                          {
                              ID = c.ID,
                              PhoneNo = (c.PhoneNumber.HasValue) ? c.PhoneNumber.Value : 0,
                              Mobno = (c.MobileNumber.HasValue) ? c.MobileNumber.Value : 0,
                              appid = Appid,
                              email = c.EmailAddress,
                              effdate = c.EffectiveFromDate
                          };

            if (!string.IsNullOrEmpty(searchString))
            {
                student = student.Where((s => s.email.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.ID);
                        else
                            student = student.OrderBy(s => s.ID);
                        break;
                    case "PhoneNo":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.PhoneNo);
                        else
                            student = student.OrderBy(s => s.PhoneNo);
                        break;
                    case "Mobno":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Mobno);
                        else
                            student = student.OrderBy(s => s.Mobno);
                        break;
                    case "email":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.email);
                        else
                            student = student.OrderBy(s => s.email);
                        break;
                    case "effdate":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.effdate);
                        else
                            student = student.OrderBy(s => s.effdate);
                        break;
                    default:
                        student = student.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(student, ref gvMain);
            uPnlGrid.Update();
            if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Contact Detail", "Admin/AdminCandidateContactDetail.aspx?key1=" + Request.QueryString["key1"], ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Contact Detail", "Admin/AdminCandidateContactDetail.aspx?" + Request.QueryString.ToString(), ""));
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

            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Contact";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Contact", "#", ""));
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateContactDetail.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("AdminCandidateContactDetail.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }


    protected int checkCandEmailMobileforExamID(Int32 CandidateID, Int64 MobileNumber, String EmailAddress)
    {

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("checkMobileAndEmailInExamId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // Parameters   
                cmd.Parameters.Add("@candidate_id", SqlDbType.Int).Value = CandidateID;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = EmailAddress;
                cmd.Parameters.Add("@Mobile", SqlDbType.BigInt).Value = MobileNumber;

                SqlParameter outParam = new SqlParameter("@is_valid", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outParam);

                con.Open();
                cmd.ExecuteNonQuery();


                int is_valid = (outParam.Value != DBNull.Value) ? Convert.ToInt32(outParam.Value) : 0;

                return is_valid;
            }
        }
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();

            Int32 phoneNumber = Convert.ToInt32(Request.QueryString["PhoneNo"]);
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);





            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {

                var student = (from s in context.Candidates
                               join ccd in context.CandidateContactDetails on s.ID equals ccd.CandidateID
                               where (ccd.PhoneNumber.HasValue ? ccd.PhoneNumber.Value : 0) == phoneNumber && s.ID == Appid
                               select ccd).FirstOrDefault();


                // CHECK DUPLICATE IN EXAM ID
                Int32 CandidateID = Convert.ToInt32(Request.QueryString["key1"]);
                Int64 MobileNumber = Convert.ToString(student.MobileNumber) == Txt_Mobno.Text ? -99 : Convert.ToInt64(Txt_Mobno.Text);
                String EmailAddress = student.EmailAddress == Txt_MailId.Text ? "-99" : Txt_MailId.Text;

                Int32 is_valid = checkCandEmailMobileforExamID(CandidateID, MobileNumber, EmailAddress);

                switch (is_valid)
                {
                    case 3:
                        ShowAlert("Email and Mobile exist 3 times or more in the most recent Exam Cycle.");
                        return;
                    case 2:
                        ShowAlert("Mobile exists 3 times or more in the most recent Exam Cycle.");
                        return;
                    case 1:
                        ShowAlert("Email exists 3 times or more in the most recent Exam Cycle.");
                        return;
                    case 0:
                        // Success 
                        break;
                    default:
                        // Handle -1 or other errors from the DB
                        ShowAlert("Error Occurred.");
                        return;
                }


                // CHECK DUPLICATE IN EXAM ID FINISH


                if (txtPhoneno.Text != null)
                    student.PhoneNumber = Convert.ToInt32(txtPhoneno.Text);

                student.MobileNumber = Convert.ToInt64(Txt_Mobno.Text);
                student.EmailAddress = Txt_MailId.Text;
                student.EffectiveFromDate = DateTime.Now;

                context.Entry(student).State = System.Data.Entity.EntityState.Modified;

                context.SaveChanges();

                Candidate_Contact_Update_temp();

                var student1 = (from s in context.Candidates
                                join ccd in context.CandidateContactHistoryDetails on s.ID equals ccd.CandidateID
                                where (ccd.PhoneNumber.HasValue ? ccd.PhoneNumber.Value : 0) == phoneNumber && s.ID == Appid
                                select ccd).FirstOrDefault();


                // updating required student fields

                if (txtPhoneno.Text != null)
                    student1.PhoneNumber = Convert.ToInt32(txtPhoneno.Text);


                student1.MobileNumber = Convert.ToInt64(Txt_Mobno.Text);
                student1.EmailAddress = Txt_MailId.Text;
                student1.CreatedOn = DateTime.Now;
                student1.CandidateID = Appid;
                student1.CreatedByID = Convert.ToInt32(Session["UserID"]);

                context.Entry(student1).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record updated.";
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateContactDetail.aspx?key1=" + Request.QueryString["key1"] + "&msg=" + strMessage));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            context.Dispose();
        }

    }
    public void Candidate_Contact_Update_temp()//this function use of temporary teble insert recored for online to offline updation purpose.
    {
        //////string ipaddress;
        //////ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //////if (ipaddress == "" || ipaddress == null)
        //////    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
        //////string clientMachineName;
        //////clientMachineName = (System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);


        Int32 phoneNumber = Convert.ToInt32(Request.QueryString["PhoneNo"]);
        Int64 Appid = Convert.ToInt32(Request.QueryString["key1"]);
        using (EConnectContext context = new EConnectContext())
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                Candidate_Personal_UpdateTemp add1 = new Candidate_Personal_UpdateTemp();

                add1.Details_Type_ID = 2;
                add1.Candidate_ID = Convert.ToInt64(Appid);
                add1.Mobile = Convert.ToInt64(Txt_Mobno.Text);
                add1.Phone = Convert.ToInt32(txtPhoneno.Text);
                add1.Email = Txt_MailId.Text;
                add1.Dob = DateTime.Now;
                add1.Update_DateTime = DateTime.Now;
                add1.Client_IPAddress = "TEST"; //ipaddress.ToString();
                add1.Client_UserId = "";
                add1.Client_HostName = "TEST";// clientMachineName.ToString();
                context.Candidate_Personal_UpdateTemps.Add(add1);
                context.SaveChanges();

            }
        }
        ;

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        //try
        //{
        //    //ddlSearchUserType.SelectedValue = "0";
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, string contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim();
            Int32 appid = Convert.ToInt32(contextKey);
            var users = from c in context.Candidates
                        join ccd in context.CandidateContactDetails on c.ID equals ccd.CandidateID
                        where c.ID == appid
                        select new { Name = ccd.EmailAddress };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);
            foreach (var user in users)
            {
                items.Add(user.Name);
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
        BreadCrumb1.Render();
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateContactDetail.aspx?key1=" + Request.QueryString["key1"]));
    }
    protected void Bindoldgridview()
    {

        try
        {
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim();
            string sortOrder = ViewState["SortOrder1"].ToString();
            string sortField = ViewState["SortField1"].ToString();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            ucSearchBar.AutoCompleteContextKey = Appid.ToString();

            var student = (from c in context.CandidateContactHistoryDetails
                           where c.CandidateID == Appid
                           orderby c.CreatedOn descending
                           select new
                           {
                               ID = c.ID,
                               PhoneNo = (c.PhoneNumber.HasValue) ? c.PhoneNumber.Value : 0,
                               Mobno = (c.MobileNumber.HasValue) ? c.MobileNumber.Value : 0,
                               appid = Appid,
                               email = c.EmailAddress,
                               effdate = c.CreatedOn
                           });
            if (!string.IsNullOrEmpty(searchString))
            {
                student = student.Where((s => s.email.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.ID);
                        else
                            student = student.OrderBy(s => s.ID);
                        break;
                    case "PhoneNo":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.PhoneNo);
                        else
                            student = student.OrderBy(s => s.PhoneNo);
                        break;
                    case "Mobno":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Mobno);
                        else
                            student = student.OrderBy(s => s.Mobno);
                        break;
                    case "email":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.email);
                        else
                            student = student.OrderBy(s => s.email);
                        break;
                    case "effdate":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.effdate);
                        else
                            student = student.OrderBy(s => s.effdate);
                        break;
                    default:
                        student = student.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar2.Bind(student, ref GridViewOld);
            uPnlGrid.Update();
            if (GridViewOld.Rows.Count <= 0)
            {
                lblError2.Visible = true;
                lblError2.Text = "No history found for candidate contact detail..";
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
    protected void PageIndexChangedOld(Int32 NewPageIndex)
    {
        try
        {
            GridViewOld.PageIndex = PagingBar2.CurrentPageIndex;
            Bindoldgridview();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void GridViewOld_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField1"] = e.SortExpression;
            if (ViewState["SortOrder1"].ToString() == "DESC")
                ViewState["SortOrder1"] = "ASC";
            else
                ViewState["SortOrder1"] = "DESC";
            Bindoldgridview();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void GridViewOld_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}