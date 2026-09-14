using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using System.Web;

public partial class HO_ResultMaster : BasePage
{
    String strMessage = string.Empty;  
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    Int32 UserTypeId = 0;
    NIELITMISContext ncontext;
    EConnectContext context= new EConnectContext();
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
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
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
                    FillResult();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Result Master", "HO/ResultMaster.aspx", ""));
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
    protected void FillResult()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var Result = from p in context.semesterResultMaster
                             orderby (p.Result)
                             select new { ValueField = p.Id, TextField = p.Result };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlResult, Result, lst);
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
            EConnect.NIELIT.semesterResultMaster result;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Result Master";

            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : 
            result = new EConnect.NIELIT.semesterResultMaster();
            result = ncontext.semesterResultMaster.Find(Convert.ToInt32(Request.QueryString["Key"]));
            txtResultName.Text = result.Result;
            txtCode.Text = result.Code;

            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(result.Result, "HO/ResultMaster.aspx?Key=" + Request.QueryString["Key"], ""));

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
            ncontext = new NIELITMISContext();
            int resultid = 0;
            if (ddlResult.SelectedValue != "0")
                resultid = Convert.ToInt32(ddlResult.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var query = from s in ncontext.semesterResultMaster
                        //  orderby s.Name
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Result.ToUpper().Contains(searchString)
                                       );
            }
            if (resultid != 0)

                query = query.Where(s => s.Id == resultid);

            query = query.OrderBy(s => s.Result);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Name":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.Result);
                        else
                            query = query.OrderBy(s => s.Result);
                        break;
                    case "Code":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.Code);
                        else
                            query = query.OrderBy(s => s.Code);
                        break;
                    default:
                        query = query.OrderBy(s => s.Result);
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
            lblHeading.Text = "New Result";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Result", "#", ""));
        }
        else
        {
            Response.Redirect("ResultMaster.aspx", true);
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
            EConnect.NIELIT.semesterResultMaster objResult;

            //create and object 
            string resultname = txtResultName.Text.ToUpper().Trim();
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objResult = new EConnect.NIELIT.semesterResultMaster();

                if (ncontext.semesterResultMaster.Any(s => s.Result.ToUpper() == resultname))
                {
                    throw new Exception("Result with this name already exists.");
                }
                else
                {
                    objResult.Result = txtResultName.Text.Trim();
                    objResult.Code = txtCode.Text;
                    objResult.enterBy = Convert.ToInt32(Session["UserId"]);
                    objResult.enterDate = DateTime.Now;
                    ncontext.semesterResultMaster.Add(objResult);
                    ncontext.SaveChanges();
                    strMessage = "New record saved.";
                }
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);

                if (!(ncontext.semesterResultMaster.Any(s => s.Result.ToUpper() == resultname.ToUpper() && s.Id != KeyID)))
                {
                    objResult = ncontext.semesterResultMaster.Find(Convert.ToInt32(Request.QueryString["Key"]));
                    objResult.Result = txtResultName.Text.Trim();
                    objResult.Code = txtCode.Text;
                    objResult.enterBy = Convert.ToInt32(Session["UserId"]);
                    objResult.enterDate = DateTime.Now;
                    ncontext.SaveChanges();
                    strMessage = "Record updated.";
                }
                else
                {
                    throw new Exception("Record with this name already exist.");
                }
            }

            Response.Redirect("ResultMaster.aspx?msg=" + strMessage);
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
            ddlResult.SelectedValue = "0";
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
            var query = from s in context.semesterResultMaster
                        select new { Name = s.Result };
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
            Response.Redirect("ResultMaster.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
 
}