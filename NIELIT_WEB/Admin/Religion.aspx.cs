using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;

public partial class Admin_Religion : BasePage
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
                    FillFilterReligion();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Religion", "Admin/Religion.aspx", ""));
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
    protected void FillFilterReligion()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var ReligionList = from p in context.Religions
                                             orderby p.ID
                                             select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReligion,ReligionList, lst);
            }
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Religion";
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ReligionID = Convert.ToInt32(Request.QueryString["Key"]);
                var objReligion = (from s in context.Religions
                                   where s.ID == ReligionID
                                              select s).FirstOrDefault();
                txtCode.Text = objReligion.Code.ToString();
                txtReligion.Text = objReligion.Name.ToString();
                txtDisplayOrder.Text = objReligion.DisplayOrder.ToString();
                txtDisplayOrder.Enabled = false;
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objReligion.Name, "#", ""));
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("ExamVenue", "#", ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                }
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
    protected void BindGridView()
    {
        try
        {
            //Int64 instituteId = Convert.ToInt64(hfAccreID.Value);
            //lblError.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 ReligionID = 0;
                if (ddlReligion.SelectedValue != "0")
                    ReligionID = Convert.ToInt32(ddlReligion.SelectedValue);
                var objReligion = from s in context.Religions
                                     orderby s.DisplayOrder
                                     select new
                                     {
                                         ID = s.ID,
                                         Name = s.Name,
                                         Code = s.Code

                                     };
                if (!String.IsNullOrEmpty(searchString))
                {
                    objReligion = objReligion.Where(s => s.Name.ToUpper().Contains(searchString)
                                                    || s.Code.ToUpper().Contains(searchString));
                }

                if (ReligionID != 0)
                {
                    objReligion = objReligion.Where(s => s.ID == ReligionID);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                objReligion = objReligion.OrderByDescending(s => s.ID);
                            else
                                objReligion = objReligion.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                objReligion = objReligion.OrderByDescending(s => s.Name);
                            else
                                objReligion = objReligion.OrderBy(s => s.Name);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                objReligion = objReligion.OrderByDescending(s => s.Code);
                            else
                                objReligion = objReligion.OrderBy(s => s.Code);
                            break;
                        default:
                            objReligion = objReligion.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(objReligion, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                //if (gvMain.Rows.Count <= 0)
                //{
                //    lblError.Text = "No Record Found";
                //    lblError.Visible = true;
                //}
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
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                //Change the heading text as required
                lblHeading.Text = "Religion";
                //Updating Breadcrumb
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Religion", "#", ""));
            }
            else
            {
                Response.Redirect("Religion.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            using (EConnectContext context = new EConnectContext())
            {
                Religion objReligion;
                Int32 DispalyOrder = Convert.ToInt32(txtDisplayOrder.Text);
                string religionname = txtReligion.Text;
                string religioncode = txtCode.Text;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {

                    if (context.Religions.Any(s => s.Name.ToUpper() == religionname.ToUpper()))
                    {
                        throw new Exception("This religion name already exists.");
                    }
                    else if (context.Religions.Any(s => s.Code.ToUpper() == religioncode.ToUpper()))
                    {
                        throw new Exception("Religion name with this code already exists.");
                    }
                    else if (context.Religions.Any(s => s.DisplayOrder == DispalyOrder))
                    {
                        throw new Exception("Religion name with this display order already exists.");
                    }
                    else
                    {
                        objReligion = new EConnect.Religion();
                        objReligion.Name = Convert.ToString(txtReligion.Text.Trim());
                        objReligion.Code = txtCode.Text.ToString().ToUpper().Trim();
                        objReligion.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                        context.Religions.Add(objReligion);
                        context.SaveChanges();
                        strMessage = "New record saved";
                    }
                }
                else
                {
                    Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
                    if (!(context.Religions.Any(s => s.Name.ToUpper() == religionname.ToUpper() && s.Code.ToUpper() == religioncode.ToUpper() && s.ID!=KeyID)))
                    {
                        objReligion = context.Religions.Find(Convert.ToInt32(Request.QueryString["key"]));
                        objReligion.Name = Convert.ToString(txtReligion.Text.Trim());
                        objReligion.Code = txtCode.Text.ToString().ToUpper().Trim();
                        strMessage = "Record updated";
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("This Religion Already Exist");
                    }

                }

                Response.Redirect("Religion.aspx?msg="+strMessage);
                //Response.Redirect("ExamCentreVenu.aspx?msg=" + strMessage + "&key1=" + Request.QueryString["CourseID"]);    
            }
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
            ddlReligion.SelectedValue = "0";
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
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                Religion religion = context.Religions.Find(Convert.ToInt32(hfActionID.Value));
                context.Religions.Remove(religion);
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl);
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
            var RelgionList = from s in context.Religions
                           select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                RelgionList = RelgionList.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            RelgionList = RelgionList.OrderBy(s => s.Name);

            var RelgionList1 = from s in context.Religions
                              select new { Name = s.Code };
            if (!String.IsNullOrEmpty(searchString))
            {
                RelgionList1 = RelgionList1.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            RelgionList1 = RelgionList1.OrderBy(s => s.Name);

            RelgionList = RelgionList.Union(RelgionList1).Take(count);
            foreach (var c in RelgionList)
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
        try
        {
            Response.Redirect("Religion.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
}