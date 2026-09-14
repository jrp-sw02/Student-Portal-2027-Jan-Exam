using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
public partial class AdminCandidateQualificationDetail : BasePage
{
    string strMessage = string.Empty;
    EConnectContext context;
    
    String currentRoleName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");       
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleName = (string)Session["RoleName"];
            if (currentRoleName == "Technical Support Query")
            {               
                btnSave.Visible = false;               
            }
            if (!Page.IsPostBack)
            {
                bindEducational();
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
    public void bindEducational()
    {

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var education = from p in context.EducationalQualifications where p.ID <=11
                            orderby (p.DisplayOrder)
                            select new { ValueField = p.ID, TextField = p.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddl_Degree, education, lst);
        };

    }
    protected void ShowEditMode()
    {

        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Detail:Update", "", ""));
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int32 qualificationID = Convert.ToInt32(Request.QueryString["qualificationID"]);
            Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
            var student = (from  c in context.Candidates
                           join cq in context.CandidateQualificationDetails on c.ID equals cq.CandidateID
                           where c.ID == appid && cq.EducationalQualificationID == qualificationID
                           select new 
                           {
                               qualificationID = cq.EducationalQualificationID,
                               Passingyear = cq.PassingYear.HasValue?cq.PassingYear.Value:0,
                               effdate = cq.EffectiveFromDate
                           }).FirstOrDefault();

            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Update Qualification Detail";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            Txt_PassYr.Text = student.Passingyear.ToString();
            ddl_Degree.SelectedValue = student.qualificationID.ToString();
            Txt_EffDate.Text = student.effdate.ToString("dd-MMM-yyyy");
            Txt_EffDate.Enabled = false;
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
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
            ucSearchBar.AutoCompleteContextKey = appid.ToString();
            var student = from c in context.Candidates
                          join cq in context.CandidateQualificationDetails on c.ID equals cq.CandidateID
                          where c.ID == appid
                          select new
                          {
                              ID = c.ID,
                              Appid = appid,
                              EduQualification = cq.EducationalQualification.Name.ToUpper(),
                              qualificationID = cq.EducationalQualificationID,
                              Passingyear = cq.PassingYear.HasValue ? cq.PassingYear.Value : 0,
                              effdate = c.EffectiveFromDate
                          };
           if (!string.IsNullOrEmpty(searchString))
            {
                student = student.Where((s => s.EduQualification.Contains(searchString)));
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
                    case "EduQualification":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.EduQualification);
                        else
                            student = student.OrderBy(s => s.EduQualification);
                        break;
                    case "Passingyear":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Passingyear);
                        else
                            student = student.OrderBy(s => s.Passingyear);
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
            uPnlNavigation.Update();
            if(!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Detail", "Admin/AdminCandidateQualificationDetail.aspx?key1=" + Request.QueryString["key1"], ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Detail", "Admin/AdminCandidateQualificationDetail.aspx?" + Request.QueryString.ToString(), "")); 
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
            lblHeading.Text = "New Qualification Detail";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Qualification Detail", "#", ""));
            Lbl_Year.Visible = true;
            Txt_PassYr.Visible = true;
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateQualificationDetail.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("AdminCandidateQualificationDetail.aspx", true);
            }
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
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int32 qualificationID = Convert.ToInt32(Request.QueryString["qualificationID"]);
            Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                var student = (from  c in context.Candidates
                              join cq in context.CandidateQualificationDetails on c.ID equals cq.CandidateID
                           where c.ID == appid && cq.EducationalQualificationID == qualificationID
                           select cq).FirstOrDefault();

                student.EducationalQualificationID = Convert.ToInt32(ddl_Degree.SelectedValue);
                student.PassingYear = Convert.ToInt32(Txt_PassYr.Text.ToString());
                student.EffectiveFromDate = DateTime.Now;
                context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record updated.";
                Candidate_Qualifications_Update_temp();
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateQualificationDetail.aspx?key1=" + Request.QueryString["key1"] + "&msg=" + strMessage));
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
    public void Candidate_Qualifications_Update_temp()//this function use of temporary teble insert recored for online to offline updation purpose.
    {
        ////////string ipaddress;
        ////////ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        ////////if (ipaddress == "" || ipaddress == null)
        ////////    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
        ////////string clientMachineName;
        ////////clientMachineName = (System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);

        Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
        //Int32 phoneNumber = Convert.ToInt32(Request.QueryString["PhoneNo"]);
        //Int64 Appid = Convert.ToInt32(Request.QueryString["key1"]);
        using (EConnectContext context = new EConnectContext())
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                Candidate_Qualifications_UpdateTemp add1 = new Candidate_Qualifications_UpdateTemp();
                add1.Candidate_ID = Convert.ToInt64(appid);
                add1.Educational_Qualification_ID = Convert.ToInt32(ddl_Degree.SelectedValue);
                add1.Passing_Year = Convert.ToInt32(Txt_PassYr.Text.ToString());
                add1.Other_Qualification = null;
                add1.Effective_From_Date = DateTime.Now;
                add1.Update_DateTime = DateTime.Now;
                add1.Client_IPAddress = "TEST"; //ipaddress.ToString();
                add1.Client_UserId = "";
                add1.Client_HostName = "TEST"; //clientMachineName.ToString();
                context.Candidate_Qualifications_UpdateTemps.Add(add1);
                context.SaveChanges();

            }
        };

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
            string searchString = prefixText.Trim().ToUpper();
            Int32 appid = Convert.ToInt32(contextKey);
            var qualification = from c in context.Candidates
                                join cq in context.CandidateQualificationDetails on c.ID equals cq.CandidateID
                                where c.ID == appid
                                select new { Name = cq.EducationalQualification.Name};
            if (!String.IsNullOrEmpty(searchString))
            {
                qualification = qualification.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            qualification = qualification.OrderBy(s => s.Name);
            foreach (var user in qualification)
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
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateQualificationDetail.aspx?key1="+Request.QueryString["key1"]));
    }
  
}
