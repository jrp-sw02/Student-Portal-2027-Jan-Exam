using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

public partial class Admin_NielitSectors : BasePage
{
    NIELITMISContext  context;
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    //BindGridView();
                    //BindEditNewModeData();
                    ShowEditMode();
                }
                else
                {
                    //BindListData();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillStatusName();

                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Sectors Master", "Admin/NielitSectors.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Sectors Master", "Admin/NielitSectors.aspx", ""));
                    }

                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
           // BreadCrumb1.Render();
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

            context = new NIELITMISContext ();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Sectors :Update", "", ""));
            BreadCrumb1.Render();
            Int32 NielitSecid = Convert.ToInt32(Request.QueryString["key"]);

                var NielitSec = (from c in context.NielitSectorss
                              where c.ID == NielitSecid
                              select new
                              {
                                  sectorShortCode = c.sectorShortCode,
                                  sectorName = c.sectorName,
                                  sectorDescription = c.sectorDescription

                              }).FirstOrDefault();
                txtSectorShortCode.Enabled = false;
                btnMode.Visible = true;
                btnSubmit.Text = "Update";
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "NIELIT Sectors Details";
                //Get last modified date of current record and save it in ViewState object.
                //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                txtSectorName.Text = NielitSec.sectorName;
                txtSectorShortCode.Text = NielitSec.sectorShortCode;
                txtDesc.Text = NielitSec.sectorDescription;
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

    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BindGridView();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (NIELITMISContext  context = new NIELITMISContext ())
                {
                    Int32 id = Convert.ToInt32(hfActionID.Value);
                    NielitSectors Sectors = context.NielitSectorss.Find(id);

                    context.NielitSectorss.Remove(Sectors);
                    context.SaveChanges();
                    scope.Complete();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                };
            }
            BindGridView();
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
        }
    }

    protected void BindGridView()
    {
        try
        {
            using (NIELITMISContext  context = new NIELITMISContext ())
            {
                Int32 StatusID = 0;
                Int32 Code = 0;
                if (ddlSectorsName.SelectedValue != "0")
                    StatusID = Convert.ToInt32(ddlSectorsName.SelectedValue);
                if (ddlCode.SelectedValue != "0")
                    Code = Convert.ToInt32(ddlCode.SelectedValue);
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                string ddlStatusNames = "";
                string ddlCodes = "";

                if (ddlSectorsName.SelectedValue != "0")
                    ddlStatusNames = ddlSectorsName.SelectedItem.ToString();
                if (ddlCode.SelectedValue != "0")
                    ddlCodes = ddlCode.SelectedItem.ToString();

                var feeGroupMass = from s in context.NielitSectorss
                                   orderby s.sectorName
                                   select new
                                   {
                                       ID = s.ID,
                                       sectorShortCode = s.sectorShortCode,
                                       sectorName = s.sectorName,
                                       sectorDescription = s.sectorDescription

                                   };

                if (StatusID != 0)
                {
                    feeGroupMass = feeGroupMass.Where(s => s.ID == StatusID);
                }

                //if (!String.IsNullOrEmpty(searchString))
                //{
                //    ReqStatusmas = ReqStatusmas.Where(s => s.StatusName.ToUpper().Contains(searchString));
                //}
                if (!String.IsNullOrEmpty(searchString))
                {
                    feeGroupMass = feeGroupMass.Where(s => s.sectorName.ToUpper().Contains(searchString) || s.sectorShortCode.ToUpper().Contains(searchString));
                }
                feeGroupMass = feeGroupMass.OrderBy(s => s.sectorName);
                //if (!String.IsNullOrEmpty(ddlStatusNames))
                //{
                //    NielitSectors = NielitSectors.Where(s => s.StatusDesc == ddlStatusNames);
                //}

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.ID);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.ID);
                            break;
                        case "sectorName":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.sectorName);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.sectorName);
                            break;
                        case "sectorShortCode":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.sectorShortCode);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.sectorShortCode);
                            break;

                        case "sectorDescription":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.sectorDescription);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.sectorDescription);
                            break;

                        default:
                            feeGroupMass = feeGroupMass.OrderBy(s => s.sectorName);
                            break;
                    }
                }

                PagingBar1.Bind(feeGroupMass, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();

                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[7].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillStatusName()
    {
        try
        {
            using (NIELITMISContext  context = new NIELITMISContext ())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusName = from p in context.NielitSectorss
                                 orderby (p.sectorName)
                                 select new { ValueField = p.ID, TextField = p.sectorName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSectorsName, StatusName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
            //BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New NIELIT Sectors Master";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New NIELIT Sectors Master", "#", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitSectors.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitSectors.aspx", true);
            }
        }
    }

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlSectorsName.SelectedValue = "0";

            //ddlflexcentretype.SelectedValue = "0";
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

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
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
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    href += "&ID=" + Request.QueryString["ID"].ToString();
                }
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                //HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //h2.NavigateUrl = hl.NavigateUrl;
                //HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //h3.NavigateUrl = hl.NavigateUrl;
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitSectors.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() ), true);
        }
        else
        {
            Response.Redirect("NielitSectors.aspx", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            
            if (String.IsNullOrEmpty(txtSectorShortCode.Text.Trim()))
            {
                lbl1.Text = "Please Enter Sector Short Code.";
            }
            else if (String.IsNullOrEmpty(txtSectorName.Text))
            {
                lbl1.Text = "Please Enter Sector Name.";

            }
            //else if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
            //{
            //    lbl1.Text = "Please Enter Sector Description.";
            //}

            else
            {
                try
                {
                    context = new NIELITMISContext ();
                    NielitSectors application;

                    //Nullable<string> Todate = null;
                    Int32 userid = Convert.ToInt32(Session["UserID"]);
                    DateTime entryDate = DateTime.Now;
                    String SectorShortCode = txtSectorShortCode.Text;
                    String SectorName = txtSectorName.Text;
                    String SectorDesc = txtDesc.Text;
                    //Int32 amt = Convert.ToInt32(txtAmt.Text.ToString());
                    //DateTime Efrm = Convert.ToDateTime(txtEFrm.Text.ToString());

                    //var FilterCode = (from s in context.NielitSectorss
                    //                   where s.sectorShortCode == SectorShortCode.Trim()
                    //                   select new
                    //                   {
                    //                       ID = s.ID,
                    //                       sectorShortCode = s.sectorShortCode,
                    //                       sectorName = s.sectorName,
                    //                       sectorDescription = s.sectorDescription
                    //                   }).FirstOrDefault();

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        if (context.NielitSectorss.Count((s) => s.sectorShortCode == SectorShortCode.Trim()) == 0)
                        {
                            application = new NielitSectors();

                            application.sectorShortCode = SectorShortCode;
                            application.sectorName = SectorName;
                            if (SectorDesc == "")
                            {
                                application.sectorDescription = " ";
                            }
                            else
                            {
                                application.sectorDescription = SectorDesc;
                            }
                            application.enterDate = entryDate;
                            application.enterBy = userid;
                            context.NielitSectorss.Add(application);
                            context.SaveChanges();
                            strMessage = "New Record Saved";

                        }
                        else
                        {
                            //strMessage = "Person is already active in this period. Please select valid date  ";
                            //Response.Redirect("NielitSectors.aspx?msg=" + "Sector Short Code already exists.", true);
                            ShowAlert("Sector Short Code already exists",true );
                            lbl1.Text = "Sector Short Code already exists";
                            return;
                        }
                    }
                    else
                    {
                        Int32 id = Convert.ToInt32(Request.QueryString["key"]);
                        application = context.NielitSectorss.Find(Convert.ToInt32(Request.QueryString["Key"]));

                        application.sectorShortCode = SectorShortCode;
                        application.sectorName = SectorName;

                        if (SectorDesc == "")
                        {
                            application.sectorDescription = " ";
                        }
                        else
                        {
                            application.sectorDescription = SectorDesc;
                        }

                        application.enterDate = entryDate;
                        application.enterBy = userid;

                        context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                        //context.NielitSectorss.Add(application);
                        context.SaveChanges();
                        strMessage = "Record updated.";
                    }

                    Response.Redirect("NielitSectors.aspx?msg=" + strMessage, true);


                }
                catch (Exception ex)
                {
                    throw ex;

                }
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
        NIELITMISContext  context = new NIELITMISContext ();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var category = from s in context.NielitSectorss
                           select new { Name = s.sectorName };
            if (!String.IsNullOrEmpty(searchString))
            {
                category = category.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            category = category.OrderBy(s => s.Name);
            var category1 = from s in context.NielitSectorss
                            select new { Name = s.sectorShortCode };
            if (!String.IsNullOrEmpty(searchString))
            {
                category1 = category1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            category = category.Union(category1).Take(count);
            foreach (var linkName in category)
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

}