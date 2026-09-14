using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;

public partial class Admin_CastCategory : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindCategories();
                    BindGridView();  
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Caste Categories", "Admin/CastCategory.aspx", ""));
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
    protected void BindCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CastCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCastCategory, Category, lst);
            };
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
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 CouCatID = 0;
            if (ddlCastCategory.SelectedValue != "0")
                CouCatID = Convert.ToInt32(ddlCastCategory.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var categories = from s in context.CastCategories
                             orderby s.DisplayOrder
                             select new
                             {
                                 ID = s.ID,
                                 categoryname = s.Name,
                                 categorycode = s.Code,
                                
                             };


            if (CouCatID != 0)
            {
                categories = categories.Where(s => s.ID == CouCatID);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.categoryname.ToUpper().Contains(searchString) || s.categorycode.ToUpper().Contains(searchString));
            }

            categories = categories.OrderBy(s => s.categoryname);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "categoryname":
                        if (sortOrder == "DESC")
                            categories = categories.OrderByDescending(s => s.categoryname);
                        else
                            categories = categories.OrderBy(s => s.categoryname);
                        break;
                    case "categorycode":
                        if (sortOrder == "DESC")
                            categories = categories.OrderByDescending(s => s.categorycode);
                        else
                            categories = categories.OrderBy(s => s.categorycode);
                        break;
                    default:
                        categories = categories.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(categories, ref gvMain);
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
            lblHeading.Text = "Caste Category Details";
            var category = context.CastCategories.Find(Convert.ToInt32(Request.QueryString["Key"]));
            Txtcatcode.Text = category.Code;
            Txtcatname.Text = category.Name;
            Txtdisplayorder.Text = category.DisplayOrder.ToString();
            Txtdisplayorder.Enabled = false;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(category.Name.ToString(), "Admin/CastCategory.aspx?" + Request.QueryString.ToString(), ""));
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
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
            lblHeading.Text = "New Caste Category";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Caste Category", "", ""));
        }
        else
        {
            Response.Redirect("CastCategory.aspx", true);
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
            Int32 displayorder = Convert.ToInt32(Txtdisplayorder.Text);
            string castname = Txtcatname.Text;
            string castcode = Txtcatcode.Text;
            CastCategory category;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                if (context.CastCategories.Any(s => s.Name.ToUpper() == castname.ToUpper()))
                {
                    throw new Exception("This caste category  name already exists.");
                }
                else if (context.CastCategories.Any(s => s.Code.ToUpper() == castcode.ToUpper()))
                {
                    throw new Exception("Caste category name with this code already exists.");
                }
                else if (context.CastCategories.Any(s => s.DisplayOrder == displayorder))
                {
                    throw new Exception("Caste category name with this display order already exists.");
                }
                else
                {
                    category = new EConnect.CastCategory();
                    category.Name = Txtcatname.Text.Trim();
                    category.Code = Txtcatcode.Text.Trim();
                    category.DisplayOrder = Convert.ToInt32(Txtdisplayorder.Text.Trim());
                    context.CastCategories.Add(category);
                    context.SaveChanges();
                    strMessage = "New record saved.";
                }
            }
            else
            {
                Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
                if (!(context.CastCategories.Any(s => s.Code.ToUpper() == castcode.ToUpper() && s.Name.ToUpper() == castname.ToUpper()  && s.ID!=KeyID)))
                {
                    category = context.CastCategories.Find(Convert.ToInt32(Request.QueryString["Key"]));
                    category.Name = Txtcatname.Text.Trim();
                    category.Code = Txtcatcode.Text.Trim();
                    category.DisplayOrder = Convert.ToInt32(Txtdisplayorder.Text.Trim());
                    context.SaveChanges();
                    strMessage = "Record updated.";
                }
                else
                {
                    throw new Exception(" This Caste Category already exists.");
                }
            }
            Response.Redirect("CastCategory.aspx?msg="+strMessage);
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
            ddlCastCategory.SelectedValue = "0";
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
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
                CastCategory castcategory = context.CastCategories.Find(Convert.ToInt32(hfActionID.Value));
                context.CastCategories.Remove(castcategory);
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
            ShowAlert("Record can not be delted!", true);
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
            var caste = from s in context.CastCategories
                           select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                caste = caste.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            caste = caste.OrderBy(s => s.Name);
            var caste1 = from s in context.CastCategories
                        select new { Name = s.Code };
            if (!String.IsNullOrEmpty(searchString))
            {
                caste1 = caste1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            caste1 = caste1.OrderBy(s => s.Name);
            caste = caste.Union(caste1).Take(count);
            foreach (var linkName in caste)
            {
                items.Add(linkName.Name);
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
        Response.Redirect("CastCategory.aspx", true);
    }
}