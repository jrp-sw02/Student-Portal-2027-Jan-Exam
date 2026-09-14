using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_Revisionlist : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Revision Numbers", "", ""));
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Form Header Detail";
            //tblNavLinks.Visible = true;
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
            context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 CouID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                CouID = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var query = from s in context.CourseRevisions
                        where s.CourseID == CouID
                        select new
                        {
                            ID=s.ID,
                            revno=s.RevisionNumber,
                            cname =s.Course.Name +"(" + s.Course.CourseCategory.Name + ")",
                            EffectiveDateFrom = s.EffectiveFromDate,
                            courseID=s.CourseID
                        };
            if (CouID != 0)
            {
                query = query.Where(s => s.courseID == CouID);
            }
           
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "revno":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.revno);
                        else
                            query = query.OrderBy(s => s.revno);
                        break;
                    case "cname":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.cname);
                        else
                            query = query.OrderBy(s => s.cname);
                        break;
                    case "EffectiveDateFrom":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.EffectiveDateFrom);
                        else
                            query = query.OrderBy(s => s.EffectiveDateFrom);
                        break;
                    default:
                        query = query.OrderBy(s => s.ID);
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Revision Number Details";
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Revision Number", "", ""));
            //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Qualification Eligibility", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Revisionlist.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
            }
            else
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect("Revisionlist.aspx", true);
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
            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Revisionlist.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&msg=" + strMessage), true);
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Revisionlist.aspx?msg=" + strMessage), true);
            }
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
        try
        {
            context = new EConnectContext();
            if (hfActionID.Value != "")
            {
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call delete function
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                TextBox revno = (TextBox)e.Row.FindControl("Txtrevno");
                TextBox coursename = (TextBox)e.Row.FindControl("Txtcname");
                Course objCourse = new EConnect.NIELIT.Course();
                using (EConnectContext context = new EConnectContext())
                {
                    objCourse = context.Courses.Find(couID);
                    var currentrev = (from c in context.CourseRevisions
                                      where c.CourseID == couID
                                      orderby c.EffectiveFromDate descending
                                      select new
                                      {
                                          revisionno = c.RevisionNumber,
                                      }).FirstOrDefault();
                    revno.Text = (currentrev.revisionno + 1).ToString();
                    coursename.Text = GetInitCap(objCourse.Name);

                };
                revno.Enabled = false;
                coursename.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Revisionlist.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("Revisionlist.aspx", true);
        }
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                TextBox revno = (TextBox)gvMain.FooterRow.FindControl("Txtrevno");
                TextBox coursename = (TextBox)gvMain.FooterRow.FindControl("Txtcname");
                TextBox date = (TextBox)gvMain.FooterRow.FindControl("txteffdate");
                DateTime doe = Convert.ToDateTime(date.Text);
                Int32 revisionNo = Convert.ToInt32(revno.Text);
                if (!IsDate(date.Text))
                {
                    ShowAlert("Invalid Date", true);
                    date.Focus();
                    return;
                }
                else if (doe < DateTime.Today)
                {
                    ShowAlert("Date of Introduction should be greater than today's date", true);
                    date.Focus();
                    return;
                }
                using (EConnectContext context = new EConnectContext())
                {
                    Course objCourse = new EConnect.NIELIT.Course();
                    Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                    objCourse = context.Courses.Find(couID);
                   
                    CourseRevision courserev= new CourseRevision();
                    if (context.CourseRevisions.Any(s =>s.EffectiveFromDate == doe))
                    {
                        throw new Exception("Data already exixts");
                    }
                    else
                    {
                        courserev.EffectiveFromDate = doe;
                        courserev.RevisionNumber = revisionNo;
                        courserev.CourseID = objCourse.ID;
                        courserev.CourseCategoryID = objCourse.CourseCategoryID;
                        courserev.Name = objCourse.Name;
                        courserev.Code = objCourse.Code;
                        context.CourseRevisions.Add(courserev);
                        context.SaveChanges();
                        String msg = "Record Saved.";
                        ShowAlert(msg, true);
                    }

                };
              
                gvMain.EditIndex = -1;
                BindGridView();
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
   
   
}