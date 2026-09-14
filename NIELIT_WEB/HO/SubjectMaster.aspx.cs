using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
public partial class SubjectMaster : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;    
    Int32 loginUserNo = 0;
    NIELITMISContext ncontext;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
             if (IsSessionAlive() == false)
                 Response.Redirect("../Index.aspx");
            
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
                    loginUserNo = Convert.ToInt32(Session["UserID"]);
                    FillSubject();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Subject Master", "HO/SubjectMaster.aspx", ""));
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillSubject()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var subject = from p in context.SubjectMaster
                              orderby (p.Name)
                              select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlsubject, subject, lst);
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
            ncontext = new NIELITMISContext();
            EConnect.NIELIT.SubjectMaster subject;
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Subject Details";
            //tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.

            subject = new EConnect.NIELIT.SubjectMaster();

            subject = ncontext.SubjectMaster.Find(Convert.ToInt32(Request.QueryString["Key"]));


            txtName.Text = subject.Name;


            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(subject.Name, "HO/SubjectMaster.aspx?Key=" + Request.QueryString["Key"], ""));
            //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            //{
            //    btnSave.Visible = false;
            //}
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

            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            ncontext = new NIELITMISContext();
            int subjecttype = 0;
            if (ddlsubject.SelectedValue != "0")
                subjecttype = Convert.ToInt32(ddlsubject.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var query = from s in ncontext.SubjectMaster
                      //  orderby s.Name
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.ToUpper().Contains(searchString)
                                       );
            }
            if (subjecttype != 0)

                query = query.Where(s => s.ID == subjecttype);

            query = query.OrderBy(s => s.Name);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Name":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.Name);
                        else
                            query = query.OrderBy(s => s.Name);
                        break;
                    default:
                        query = query.OrderBy(s => s.Name);
                        break;
                }
            }
            PagingBar1.Bind(query, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
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
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Subject";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Subject", "#", ""));
        }
        else
        {
            Response.Redirect("SubjectMaster.aspx", true);
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
            BindGridView();
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
            ncontext = new NIELITMISContext();
            EConnect.NIELIT.SubjectMaster subject;
            EConnect.NIELIT.SemesterSubjectMaster semestersubject;
            context = new EConnectContext();
            //create and object 
            string name = txtName.Text.ToUpper().Trim();
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                subject = new EConnect.NIELIT.SubjectMaster();

                if (ncontext.SubjectMaster.Any(s => s.Name.ToUpper() == name))
                {
                    throw new Exception("Subject with this name already exists.");
                }
                else
                {
                    subject.Name = txtName.Text.Trim();
                    subject.CreatedByID =Convert.ToInt32(Session["UserId"]);
                    subject.CreatedOn = DateTime.Now;
                    ncontext.SubjectMaster.Add(subject);
                    ncontext.SaveChanges();
                    strMessage = "New record saved.";
                }
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);

                if (!(ncontext.SubjectMaster.Any(s => s.Name.ToUpper() == name.ToUpper() && s.ID != KeyID)))
                {
                    subject = ncontext.SubjectMaster.Find(Convert.ToInt32(Request.QueryString["Key"]));
                    semestersubject = new EConnect.NIELIT.SemesterSubjectMaster();
                    var semestersubjects = (from s in ncontext.SemesterSubjectMaster
                                           where s.SubId == KeyID 
                                           select new
                                           {
                                               ID = s.Id,                                              
                                               SubId = s.SubId
                                           }).FirstOrDefault();
                    if (semestersubjects == null)
                    {
                        //subject = new EConnect.NIELIT.SubjectMaster();
                        subject.Name = txtName.Text.Trim();
                        subject.CreatedByID = Convert.ToInt32(Session["UserId"]);
                        subject.CreatedOn = DateTime.Now;

                        ncontext.SaveChanges();
                        strMessage = "Record updated.";
                    }
                    else
                    {
                        strMessage = "Updation will be not allowed when subject is used in semester.";
                    }
                }
                else
                {
                    throw new Exception("Record with this name already exist.");
                }
            }
           
            Response.Redirect("SubjectMaster.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
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
            ddlsubject.SelectedValue = "0";
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
                
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var query = from s in context.SubjectMaster
                        select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            query = query.OrderBy(s => s.Name);

            
            foreach (var user in query)
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
        try
        {
            Response.Redirect("SubjectMaster.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

}