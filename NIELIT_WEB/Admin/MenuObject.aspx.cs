using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using System.Data.Entity.Infrastructure;
public partial class Admin_MenuObject : BasePage, ICallbackEventHandler
{
    EConnectContext context;
    String strSql = null;
    String strMessage = null;
    String result = null;
    Int32 currentRoleId = 0;
    #region ICallbackEventHandler Members
    public void RaiseCallbackEvent(string eventArgument)
    {
        String strPad = "";
        String[] args = eventArgument.Split('$');
        if (args[0] == "GetDetail")
        {
            //strSql = " SELECT id, name, type_id FROM urm_menu_object CONNECT BY PRIOR parent_id = ID START WITH ID = '" + args[1] + "' ORDER BY LEVEL DESC ";
            //strSql = " SELECT id, name, type_id FROM urm_menu_object where ID = '" + args[1] + "'";
            ////DataTable dt = EConnect.Utils.Data.DbUtility.GetDataTable(strSql, new EConnect.Connections.OracleCon(), null, CommandType.Text, false);
            //DataTable dt = EConnect.Utils.Data.DbUtility.GetDataTable(strSql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            //foreach (DataRow dtRow in dt.Rows)
            //{
            //    strPad = "";
            //    strPad = strPad.PadLeft((Convert.ToInt16(dtRow["type_id"]) - 1) * 4, '.');
            //    result += strPad + dtRow["name"].ToString() + "<br>";

            //}
            List<string> list1=new List<string>(); 
            using (EConnectContext context = new EConnectContext())
            {
                MenuObject objMenu = context.MenuObjects.Find(Convert.ToInt64(args[1]));
                if (objMenu != null)
                {
                    MenuObject parent1 = objMenu.ParentMenuObject;
                    if (parent1 != null)
                    {
                        list1.Add(parent1.Name);
                        MenuObject parent2 = parent1.ParentMenuObject;
                        if (parent2 != null)
                        {
                            list1.Add(parent2.Name);
                            MenuObject parent3 = parent2.ParentMenuObject;
                            if (parent3 != null)
                            {
                                list1.Add(parent3.Name);
                            }
                        }

                    }
                }
                list1.Reverse();
            };
           
            for (int i = 0; i < list1.Count ; i++)
            {
                strPad = "";
                //strPad = strPad.PadLeft((list1.Count - 1) * 4, '.');
                strPad = strPad.PadLeft(i * 4, '.');
                result += strPad + list1[i] + "<br>";

            }
        }
         
    }
    public string GetCallbackResult()
    {
        return result;
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (Session["ModuleId"] == null)
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
                    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlSearchObjectType, typeof(EConnect.URM.enmMenuObjectType), new ListItem("--All--", "0"));
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Menu Objects", "Admin/MenuObject.aspx", ""));
                    
                }
                if(!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
            ////Writin these 2 lines in all forms is compulsory.
            //if (Context.Request.QueryString.AllKeys.Length > 0 && Context.Request.QueryString.AllKeys.Contains("?value") == false)
            //    Form.Action = EConnect.Utils.Security.QuertStringModule.Encrypt(Context.Request.Url.ToString());
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "Menu Object Details";
            EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlObjectType, typeof(EConnect.URM.enmMenuObjectType), new ListItem("--Select One--", "0"));
            MenuObject objMenuObject = context.MenuObjects.Find(Convert.ToInt32(Request.QueryString["Key"]));
            ddlObjectType.SelectedValue = objMenuObject.MenuObjectTypeID.ToString();
            ddlObjectType_SelectedIndexChanged(ddlObjectType, EventArgs.Empty);
            ddlIsParent.SelectedValue = Convert.ToInt16(objMenuObject.IsParent).ToString() ;
            ddlIsParent_SelectedIndexChanged(ddlIsParent, EventArgs.Empty);
            if (objMenuObject.ParentMenuObjectID.HasValue)
            {
                txtParent.Text = objMenuObject.ParentMenuObject.Name;
                hfParentId.Value = objMenuObject.ParentMenuObjectID.Value.ToString();
            }
            txtObjectName.Text = objMenuObject.Name;
            txtAbr.Text = objMenuObject.Abbreviation;
            txtOrderNo.Text = objMenuObject.DisplayOrder.ToString();
            txtURL.Text = objMenuObject.FormURL;
            ddlIsNewWindow.SelectedValue =Convert.ToInt16(objMenuObject.OpenInNewWindow).ToString();
            txtParameters.Text = objMenuObject.NewWindowParamenters;
            txtIconPath.Text = objMenuObject.IconPath;
            txtThumbPath.Text = objMenuObject.ThumbNailPath;
            //ViewState["LastModifiedOn"] = objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Updating BreadScrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objMenuObject.Name, "Admin/MenuObject.aspx?Key=" + Request.QueryString["Key"], ""));
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            int objectType = 0;
            if (ddlSearchObjectType.SelectedValue != "0")
                objectType = Convert.ToInt32(ddlSearchObjectType.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var menuObjects = from s in context.MenuObjects
                              select new { ID = s.ID, OrganizationID = s.OrganizationID, s.Name,
                                MenuObjectType = s.MenuObjectType.Name, ParentMenuObject= s.ParentMenuObject.Name, 
                                ParentMenuObjectType= s.ParentMenuObject.MenuObjectType.Name, MenuObjectTypeID = s.MenuObjectTypeID  };
            if (!String.IsNullOrEmpty(searchString))
            {
                menuObjects = menuObjects.Where(s => s.Name.ToUpper().Contains(searchString)
                                       || s.ParentMenuObject.ToUpper().Contains(searchString));
            }
            
            if (objectType != 0)
                menuObjects = menuObjects.Where(s => s.MenuObjectTypeID == objectType);
             
            menuObjects = menuObjects.OrderBy(s => s.Name);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Name":
                        if (sortOrder == "DESC")
                            menuObjects = menuObjects.OrderByDescending(s => s.Name);
                        else
                            menuObjects = menuObjects.OrderBy(s => s.Name);
                        break;
                    case "MenuObjectType":
                        if (sortOrder == "DESC")
                            menuObjects = menuObjects.OrderByDescending(s => s.MenuObjectType);
                        else
                            menuObjects = menuObjects.OrderBy(s => s.MenuObjectType);
                        break;
                    case "ParentMenuObject":
                        if (sortOrder == "DESC")
                            menuObjects = menuObjects.OrderByDescending(s => s.ParentMenuObject);
                        else
                            menuObjects = menuObjects.OrderBy(s => s.ParentMenuObject);
                        break;
                    case "ParentMenuObjectType":
                        if (sortOrder == "DESC")
                            menuObjects = menuObjects.OrderByDescending(s => s.ParentMenuObjectType);
                        else
                            menuObjects = menuObjects.OrderBy(s => s.ParentMenuObjectType);
                        break;
                    default:
                        menuObjects = menuObjects.OrderBy(s => s.Name);
                        break;
                }
            }
            PagingBar1.Bind(menuObjects, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
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
           ShowAlert(ex.Message);
        }
    }
    protected void BtnMode_Click(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}s
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New Menu Object";
            EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlObjectType , typeof(EConnect.URM.enmMenuObjectType), new ListItem("--Select One--", "0"));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Menu Object", "#", ""));
        }
        else
        {
            Response.Redirect("MenuObject.aspx", true);
        }
    }
    protected void LnkBtnGO(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
           ShowAlert(ex.Message);
        }
    }
    protected void LnkBtnResetSearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
           ShowAlert(ex.Message);
        }
    }
    protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtParent.Text = "";
            hfParentId.Value = "";
            trForm.Visible = false;
            trForm1.Visible = false;
            ddlIsParent.SelectedValue = "1";
            context = new EConnectContext();
            if (ddlObjectType.SelectedValue != "0")
            {
                MenuObjectType objectType = context.MenuObjectTypes.Find(Convert.ToInt32(ddlObjectType.SelectedValue));
                if (objectType.HasSameParent == true)
                {
                    ddlIsParent.SelectedValue = "1";
                    ddlIsParent.Enabled = true;
                }
                else
                {
                    ddlIsParent.SelectedValue = "1";
                    ddlIsParent.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.Render();
            context = new EConnectContext();
            MenuObject objMenuObject;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objMenuObject = new MenuObject();
                objMenuObject.OrganizationID = Convert.ToInt32(Session["OrgId"]);
                objMenuObject.CreatedBy = Convert.ToInt32(Session["UserID"]);
                objMenuObject.CreatedOn = DateTime.Now;
                strMessage = "New record saved.";
            }
            else
            {
                objMenuObject = context.MenuObjects.Find(Convert.ToInt32(Request.QueryString["Key"]));
                //DateTime modifiedOn = objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //if (Convert.ToDateTime(ViewState["LastModifiedOn"]) == modifiedOn)
                //{
                objMenuObject.ModifiedBy = Convert.ToInt32(Session["UserID"]);
                objMenuObject.ModifiedOn = DateTime.Now;
                strMessage = "Record updated.";
                //}
                //else
                //    throw new Exception("Record changed by some other person. Please reopen the record in edit mode and update again.");

            }
            objMenuObject.MenuObjectTypeID = Convert.ToInt32(ddlObjectType.SelectedValue);
            objMenuObject.IsParent = Convert.ToBoolean(Convert.ToInt32(ddlIsParent.SelectedValue));
            if (!String.IsNullOrWhiteSpace(hfParentId.Value))
            {
                objMenuObject.ParentMenuObjectID = Convert.ToInt32(hfParentId.Value);
            }
            objMenuObject.Name = txtObjectName.Text;
            objMenuObject.Abbreviation = txtAbr.Text;
            objMenuObject.DisplayOrder = txtOrderNo.Text == "" ? 0 : Convert.ToInt32(txtOrderNo.Text);
            if (objMenuObject.IsParent == false)
            {
                objMenuObject.FormURL = txtURL.Text;
                objMenuObject.OpenInNewWindow = Convert.ToBoolean(Convert.ToInt32(ddlIsNewWindow.SelectedValue));
                objMenuObject.NewWindowParamenters = txtParameters.Text;
            }
            else
            {
                objMenuObject.FormURL = "";
                objMenuObject.OpenInNewWindow = false;
                objMenuObject.NewWindowParamenters = "";
            }
            objMenuObject.IconPath = txtIconPath.Text;
            objMenuObject.ThumbNailPath = txtThumbPath.Text;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                context.MenuObjects.Add(objMenuObject);
            else
            {
                context.Entry(objMenuObject).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
            Response.Redirect("MenuObject.aspx?msg=" + strMessage);
        }
        catch (DbUpdateConcurrencyException et)
        {
            ShowAlert(et.Message);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
           ShowAlert(ex.Message);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlSearchObjectType.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
           ShowAlert(ex.Message);
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
           ShowAlert(ex.Message);
        }
    }
    protected void imgSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            context = new EConnectContext();
            tvParents.Nodes.Clear();
            MenuObjectType objMenuObjectType = context.MenuObjectTypes.Find(Convert.ToInt32(ddlObjectType.SelectedValue));
            if (objMenuObjectType.ParentID.HasValue)
            {
                var parents = from p in context.MenuObjects
                              where p.MenuObjectTypeID == (int)enmMenuObjectType.Project
                              select p;
                foreach (MenuObject  menuObject in parents)
                {
                    TreeNode tn = new TreeNode(menuObject.Name, menuObject.ID.ToString());

                    if ((enmMenuObjectType)objMenuObjectType.ParentID.Value == menuObject.enmMenuObjectType)
                        tn.SelectAction = TreeNodeSelectAction.Select;
                    else
                        tn.SelectAction = TreeNodeSelectAction.Expand;
                    tvParents.Nodes.Add(tn);
                    tvParents.ExpandAll();
                    BindTreeView(ref tn, (enmMenuObjectType)objMenuObjectType.ParentID.Value, menuObject.enmMenuObjectType, menuObject.ChildMenuObjects);
                }

                ModalPopupExtender1.Show();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tt", "alert('Menu object type Project does not have any parent type')", true);
                        
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    private void BindTreeView(ref TreeNode ParentMenuItem, enmMenuObjectType pParentOfSelectedObjectType, enmMenuObjectType pCurrentParentType, ICollection<MenuObject> menuObjects)
    {
        try
        {
            foreach (MenuObject menuObject in menuObjects)
            {
                TreeNode tn = new TreeNode(menuObject.Name, menuObject.ID.ToString());
                ParentMenuItem.ChildNodes.Add(tn);
                if (pParentOfSelectedObjectType == menuObject.enmMenuObjectType)
                    tn.SelectAction = TreeNodeSelectAction.Select;
                else
                {
                    if(menuObject.IsParent == true)
                        tn.SelectAction = TreeNodeSelectAction.Select;
                    else
                        tn.SelectAction = TreeNodeSelectAction.Expand;
                }
                tvParents.ExpandAll();
                BindTreeView(ref tn, pParentOfSelectedObjectType, menuObject.enmMenuObjectType, menuObject.ChildMenuObjects);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void tvParents_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            txtParent.Text = tvParents.SelectedNode.Text;
            hfParentId.Value = tvParents.SelectedNode.Value;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlIsParent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            MenuObjectType objectType = context.MenuObjectTypes.Find(Convert.ToInt32(ddlObjectType.SelectedValue));
            if (objectType.IsWindowType && ddlIsParent.SelectedValue == "0")
            {
                trForm.Visible = true;
                trForm1.Visible = true;
            }
            else
            {
                trForm.Visible = false;
                trForm1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void lbDelteteOne_Click(object sender, EventArgs e)
    {
        try
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            context = new EConnectContext();
            context.MenuObjects.Remove(context.MenuObjects.Find(Convert.ToInt32(hfActionID.Value.Split('$')[0].ToString())));
            context.SaveChanges();
            BindGridView();
            ShowAlert("Record deleted successfully.");
            hfActionID.Value = "";
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be delted!", true);
        }
        finally { context.Dispose(); }
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
            var menuObjects = from m in context.MenuObjects
                        select new { Name = m.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                menuObjects = menuObjects.Where(m => m.Name.ToUpper().Contains(searchString));
            }
            menuObjects = menuObjects.OrderBy(m => m.Name).Distinct().Take(count);

            foreach (var user in menuObjects)
            {
                items.Add(user.Name);
            }
            context.Dispose();
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
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

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString() + "$" + gvMain.DataKeys[e.Row.RowIndex].Values[1].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuObject.aspx", true);
    }
}