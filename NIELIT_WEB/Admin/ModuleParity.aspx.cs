using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class ModuleParity : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;

    IEnumerable<Object> modules;
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
                    fillOldRevNo();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Modules Parity", "Admin/ModuleParity.aspx", ""));
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
    protected void fillOldRevNo()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 catID = Convert.ToInt32(Request.QueryString["CategoryID"]);
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                ListItem lst = new ListItem("--Select One--", "-1");
                var query = (from p in context.CourseRevisions
                            where (p.CourseCategoryID== catID && p.CourseID == couID)
                            orderby (p.RevisionNumber)
                            select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber}).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlOldRevNo, query, lst);
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Form Header Detail";
            tblNavLinks.Visible = true;
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
            Int32 CourseID = 0;
            Int32 couCatID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            if (!String.IsNullOrEmpty(Request.QueryString["CategoryID"]))
            {
                couCatID = Convert.ToInt32(Request.QueryString["CategoryID"]);
            }
            int[] moduleTypes = { Convert.ToInt32(enmModuleType.Theory), Convert.ToInt32(enmModuleType.Bridge) };
            Int32 revNo = Convert.ToInt32(ddlOldRevNo.SelectedValue);
            Int32 newrevNo = Convert.ToInt32(ddlNewRevNo.SelectedValue);
            //var newQuery =( from s in context.Modules
            //            where (s.CourseCategoryID == couCatID && s.CourseID == CourseID && s.RevisionNumber == newrevNo
            //                   && moduleTypes.Contains(s.ModuleTypeID))
            //            orderby s.Code
            //            select new
            //            {
            //                ValueField = s.ID,
            //                TextField = s.ShortName + "-" + s.Name
            //            }).ToList();

            //modules = newQuery;
            var query = from s in context.Modules
                        where (s.CourseCategoryID == couCatID && s.CourseID == CourseID && s.RevisionNumber == revNo)
                        orderby s.Code
                        select new
                        {
                            ID=s.ID,
                            oldModuleName=s.ShortName + "-" + s.Name
                        };
                gvMain.Columns[1].HeaderText = "Module Name:Revision " + ddlOldRevNo.SelectedValue.ToString();
                gvMain.Columns[2].HeaderText = "Module Name:Revision " + ddlNewRevNo.SelectedValue.ToString();
                if (query.Count() != 0)
                {
                    PagingBar1.Bind(query, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                }
                else
                {
                    lberror.Visible = true;
                    lberror.Text = "No Record Found";
                    btnReset.Visible = false;
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            Response.Redirect("ModuleParity.aspx", true);
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
            context = new EConnectContext();
            //create and object 
            Parity objParity;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objParity = new Parity();
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Int32 couCatID = Convert.ToInt32(Request.QueryString["CategoryID"]);
                objParity.CourseID = couID;
                objParity.CourseCategoryID = couCatID;
                objParity.OldRevisionNumber = Convert.ToInt32(ddlOldRevNo.SelectedValue);
                objParity.NewRevisionNumber = Convert.ToInt32(ddlNewRevNo.SelectedValue);
                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                strMessage = "Record updated.";
            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("ModuleParity.aspx?msg=" + strMessage);
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
            //ddlSearchUserType.SelectedValue = "0";
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
                using (EConnectContext context = new EConnectContext())
                {
                     int[] moduleTypesBridgeTheory = { Convert.ToInt32(enmModuleType.Theory), Convert.ToInt32(enmModuleType.Bridge) };
                     int[] moduleTypePractical = { Convert.ToInt32(enmModuleType.Practical) };
                     int[] moduleTypeProject = { Convert.ToInt32(enmModuleType.Project) };
                     Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                     Int32 couCatID = Convert.ToInt32(Request.QueryString["CategoryID"]);
                     Int32 oldModuleID = Convert.ToInt32(gvMain.DataKeys[e.Row.RowIndex].Values[0]);
                     Int32 newrevNo = Convert.ToInt32(ddlNewRevNo.SelectedValue);
                     Module oldModuleTypeID = context.Modules.Find(oldModuleID);
                     if (oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Theory) || oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Bridge))
                     {
                         var newQuery = (from s in context.Modules
                                         where (s.CourseCategoryID == couCatID && s.CourseID == couID && s.RevisionNumber == newrevNo
                                                && moduleTypesBridgeTheory.Contains(s.ModuleTypeID))
                                         orderby s.Code
                                         select new
                                         {
                                             ValueField = s.ID,
                                             TextField = s.ShortName + "-" + s.Name
                                         }).ToList();

                         modules = newQuery;
                         DropDownList ddlNewRevNumber = (DropDownList)e.Row.FindControl("ddlNew");
                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNumber, modules, new ListItem("--Select One--", "0"));
                         var query = (from s in context.Parities
                                      where (s.CourseID == couID && s.CourseCategoryID == couCatID && s.OldModuleID == oldModuleID)
                                      select new { newmoduleID = s.NewModuleID }).FirstOrDefault();
                         if (query != null)
                         {
                             ddlNewRevNumber.SelectedValue = query.newmoduleID.ToString();
                         }
                     }
                     else if (oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Practical))
                     {
                         var newQuery = (from s in context.Modules
                                         where (s.CourseCategoryID == couCatID && s.CourseID == couID && s.RevisionNumber == newrevNo
                                                && moduleTypePractical.Contains(s.ModuleTypeID))
                                         orderby s.Code
                                         select new
                                         {
                                             ValueField = s.ID,
                                             TextField = s.ShortName + "-" + s.Name
                                         }).ToList();

                         modules = newQuery;
                         DropDownList ddlNewRevNumber = (DropDownList)e.Row.FindControl("ddlNew");
                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNumber, modules, new ListItem("--Select One--", "0"));
                         var query = (from s in context.Parities
                                      where (s.CourseID == couID && s.CourseCategoryID == couCatID && s.OldModuleID == oldModuleID)
                                      select new { newmoduleID = s.NewModuleID }).FirstOrDefault();
                         if (query != null)
                         {
                             ddlNewRevNumber.SelectedValue = query.newmoduleID.ToString();
                         }
                     }
                     else if (oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Project))
                     {
                         var newQuery = (from s in context.Modules
                                         where (s.CourseCategoryID == couCatID && s.CourseID == couID && s.RevisionNumber == newrevNo
                                                && moduleTypeProject.Contains(s.ModuleTypeID))
                                         orderby s.Code
                                         select new
                                         {
                                             ValueField = s.ID,
                                             TextField = s.ShortName + "-" + s.Name
                                         }).ToList();

                         modules = newQuery;
                         DropDownList ddlNewRevNumber = (DropDownList)e.Row.FindControl("ddlNew");
                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNumber, modules, new ListItem("--Select One--", "0"));
                         var query = (from s in context.Parities
                                      where (s.CourseID == couID && s.CourseCategoryID == couCatID && s.OldModuleID == oldModuleID)
                                      select new { newmoduleID = s.NewModuleID }).FirstOrDefault();
                         if (query != null)
                         {
                             ddlNewRevNumber.SelectedValue = query.newmoduleID.ToString();
                         }
                     }
                };
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
            var users = from s in context.Users
                        select new { Name = s.UserName };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);

            var users1 = from s in context.Users
                         select new { Name = s.LoginID };
            if (!String.IsNullOrEmpty(searchString))
            {
                users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.Union(users1).Take(count);
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
        if (!String.IsNullOrEmpty(Request.QueryString["CategoryID"]))
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ModuleParity.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"]), true);
            }
        }
        else
        {
            Response.Redirect("ModuleParity.aspx", true);
        }
       
    }
    protected void ddlOldRevNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlNewRevNo.Items.Clear();
        fillNewRevNo();
    }
    protected void fillNewRevNo()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 catID = Convert.ToInt32(Request.QueryString["CategoryID"]);
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Int32 oldRevNo = Convert.ToInt32(ddlOldRevNo.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                var query = (from p in context.CourseRevisions
                             where (p.CourseCategoryID == catID && p.CourseID==couID &&  p.RevisionNumber > oldRevNo )
                             orderby p.RevisionNumber ascending 
                             select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber }).Take(1);
                
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNo, query, lst);
            };
   
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (ddlOldRevNo.SelectedValue == "-1")
            {
                throw new Exception("Please select Old Revision No.");
            }
            if (ddlNewRevNo.SelectedValue == "0")
            {
                throw new Exception("Please select New Revision No.");
            }
           
            
            divGrid.Visible = true;
            divNavigation.Visible = true;
           
            PagingBar1.CurrentPageSize = 0; 
            BindGridView();

            PagingBar1.Visible = true;
            divSave.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlOldRevNo.SelectedValue = "-1";
            ddlNewRevNo.SelectedValue = "0";
            divGrid.Visible = false;
            divNavigation.Visible = false;
            divSave.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            Boolean isNotSelectedCurrentModule = false;
            Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Int32 couCatID = Convert.ToInt32(Request.QueryString["CategoryID"]);
            Int32 oldRevNum = Convert.ToInt32(ddlOldRevNo.SelectedValue);
            Int32 newRevNum=Convert.ToInt32(ddlNewRevNo.SelectedValue);
            foreach (GridViewRow row in gvMain.Rows)
            {
                Int32 newModuleID = Convert.ToInt32(((DropDownList)row.FindControl("ddlNew")).SelectedValue);
                if (newModuleID != 0)
                {
                    isNotSelectedCurrentModule = true;
                }
            }
            if (isNotSelectedCurrentModule == false)
            {
                throw new Exception("Please Select at least one Module Name");
            }
            context.Database.ExecuteSqlCommand("delete from Parity where Course_ID='" + couID.ToString() + "'" +
                                               " and Course_Category_ID='" + couCatID.ToString() + "'" +
                                               " and Old_Revision_Number='" + oldRevNum.ToString() + "'" +
                                               " and New_Revision_Number='" + newRevNum.ToString() + "'");
            if (btnSave.Text == "Save")
            {
                foreach (GridViewRow row in gvMain.Rows)
                {
                    Parity objParity = new Parity();
                   
                    objParity.CourseID = couID;
                    objParity.CourseCategoryID = couCatID;
                    objParity.OldRevisionNumber = oldRevNum;
                    objParity.NewRevisionNumber = newRevNum;
                    Int32 newModuleID = Convert.ToInt32(((DropDownList)row.FindControl("ddlNew")).SelectedValue);
                    objParity.NewModuleID = newModuleID;
                    Int32 oldModuleID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                    objParity.OldModuleID = oldModuleID;
                    if (newModuleID != 0)
                    {
                        context.Parities.Add(objParity);
                    }
                }
                context.SaveChanges();
                strMessage = "Record saved";
            }
            Response.Redirect("ModuleParity.aspx?" + Request.QueryString.ToString() + "&msg=" + strMessage);
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
}