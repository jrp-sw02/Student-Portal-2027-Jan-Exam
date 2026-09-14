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

public partial class Admin_NielitTrgSpecialization : BasePage
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
                    FillSectorID();
                    ShowEditMode();
                }
                else
                {
                    //BindListData();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillStatusName();
                    FillSectorID();
                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Training Specialization", "Admin/NielitTrgSpecialization?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Training Specialization", "Admin/NielitTrgSpecialization.aspx", ""));
                    }

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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitTrgSpecialization.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("NielitTrgSpecialization.aspx", true);
        }
    }
    protected void ShowEditMode()
    {

        try
        {

            context = new NIELITMISContext();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Training Specialization :Update", "", ""));
            BreadCrumb1.Render();
            Int32 NielitSecid = Convert.ToInt32(Request.QueryString["key"]);

            var NielitSec = (from c in context.NielitTrgSpecializations
                             where c.ID == NielitSecid
                             select new
                             {
                                 NielitSectorID = c.NielitSectorID,
                                 specializationCode = c.specializationCode,
                                 specializationName = c.specializationName,
                                 specializationDetails = c.specializationDetails

                             }).FirstOrDefault();
            txtspecializationCode.Enabled = false;
            ddlNielitSectorID.Enabled = false;
            btnMode.Visible = true;
            btnSubmit.Text = "Update";
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "NIELIT Training Specializations";
            //Get last modified date of current record and save it in ViewState object.
            //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            ddlNielitSectorID.SelectedValue = NielitSec.NielitSectorID.ToString();
            txtspecializationCode.Text = NielitSec.specializationCode;
            txtspecializationName.Text = NielitSec.specializationName;
            txtspecializationDetails.Text = NielitSec.specializationDetails;
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
                    NielitTrgSpecialization Sectors = context.NielitTrgSpecializations.Find(id);

                    context.NielitTrgSpecializations.Remove(Sectors);
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
                if (ddlSectorsName.SelectedValue != "0")
                    StatusID = Convert.ToInt32(ddlSectorsName.SelectedValue);

                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                string ddlStatusNames = "";

                if (ddlSectorsName.SelectedValue != "0")
                    ddlStatusNames = ddlSectorsName.SelectedItem.ToString();


                var feeGroupMass = from s in context.NielitTrgSpecializations
                                   join c in context.NielitSectorss
                                    on s.NielitSectorID equals c.ID
                                   orderby s.specializationName 
                                   select new
                                   {
                                       ID = s.ID,
                                       NielitSectorID = c.sectorName + "(" + c.sectorShortCode + ")",
                                       specializationCode = s.specializationCode,
                                       specializationName = s.specializationName,
                                       specializationDetails = s.specializationDetails

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
                    feeGroupMass = feeGroupMass.Where(s => s.specializationName.ToUpper().Contains(searchString) || s.specializationCode.ToUpper().Contains(searchString));
                }
                feeGroupMass = feeGroupMass.OrderBy(s => s.specializationName);
                //if (!String.IsNullOrEmpty(ddlStatusNames))
                //{
                //    NielitTrgSpecialization = NielitTrgSpecialization.Where(s => s.StatusDesc == ddlStatusNames);
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
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.specializationName);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.specializationName);
                            break;
                        case "sectorShortCode":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.specializationCode);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.specializationCode);
                            break;

                        case "sectorDescription":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.specializationDetails);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.specializationDetails);
                            break;

                        default:
                            feeGroupMass = feeGroupMass.OrderBy(s => s.specializationName);
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
                var StatusName = from p in context.NielitTrgSpecializations
                                 orderby (p.specializationName)
                                 select new { ValueField = p.ID, TextField = p.specializationName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSectorsName, StatusName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillSectorID()
    {
        try
        {
            using (NIELITMISContext  context = new NIELITMISContext ())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusName = from p in context.NielitSectorss
                                 orderby (p.sectorName)
                                 select new { ValueField = p.ID, TextField = p.sectorName + "(" + p.sectorShortCode + ")" };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitSectorID, StatusName, lst);
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
            lblHeading.Text = "New NIELIT Training Specialization";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New NIELIT Training Specialization", "#", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitTrgSpecialization.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitTrgSpecialization.aspx", true);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            if (ddlNielitSectorID.SelectedValue == "0")
            {
                lbl1.Text = "Please Select Sector.";
            }

            else if (String.IsNullOrEmpty(txtspecializationCode.Text.Trim()))
            {
                lbl1.Text = "Please Enter Specialization Code.";
            }
            else if (String.IsNullOrEmpty(txtspecializationName.Text))
            {
                lbl1.Text = "Please Enter Specialization Name.";

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
                    NielitTrgSpecialization application;

                    //Nullable<string> Todate = null;
                    Int32 userid = Convert.ToInt32(Session["UserID"]);
                    DateTime entryDate = DateTime.Now;
                    int NielitSectorID = Convert.ToInt32(ddlNielitSectorID.SelectedValue);
                    String specializationCode = txtspecializationCode.Text;
                    String specializationName = txtspecializationName.Text;
                    String specializationDetails = txtspecializationDetails.Text;
                    //Int32 amt = Convert.ToInt32(txtAmt.Text.ToString());
                    //DateTime Efrm = Convert.ToDateTime(txtEFrm.Text.ToString());

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        application = new NielitTrgSpecialization();
                        if (context.NielitTrgSpecializations.Count((s) => s.specializationCode == specializationCode.Trim()) == 0)
                        {
                            application.NielitSectorID = NielitSectorID;
                            application.specializationCode = specializationCode;
                            application.specializationName = specializationName;
                            application.specializationDetails = specializationDetails;

                            if (specializationDetails == "")
                            {
                                application.specializationDetails = " ";
                            }
                            else
                            {
                                application.specializationDetails = specializationDetails;
                            }
                            application.enterDate = entryDate;
                            application.enterBy = userid;
                            context.NielitTrgSpecializations.Add(application);
                            context.SaveChanges();
                            strMessage = "New Record Saved";
                        }
                        else
                        {
                            //strMessage = "Person is already active in this period. Please select valid date  ";
                           // Response.Redirect("NielitTrgSpecialization.aspx?msg=" + "Specialization Code already exists.", true);
                            ShowAlert("Specialization Code already exists.", true);
                            lbl1.Text = "Specialization Code already exists.";
                            return;
                        }

                    }
                    else
                    {
                        Int32 id = Convert.ToInt32(Request.QueryString["key"]);
                        application = context.NielitTrgSpecializations.Find(Convert.ToInt32(Request.QueryString["Key"]));

                        application.NielitSectorID = NielitSectorID;
                        application.specializationCode = specializationCode;
                        application.specializationName = specializationName;
                        
                        if (specializationDetails == "")
                        {
                            application.specializationDetails = " ";
                        }
                        else
                        {
                            application.specializationDetails = specializationDetails;
                        }

                        application.enterDate = entryDate;
                        application.enterBy = userid;

                        context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                        //context.NielitTrgSpecializations.Add(application);
                        context.SaveChanges();
                        strMessage = "Record updated.";
                    }

                    Response.Redirect("NielitTrgSpecialization.aspx?msg=" + strMessage, true);


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
        NIELITMISContext  context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var category = from s in context.NielitTrgSpecializations
                           select new { Name = s.specializationName };
            if (!String.IsNullOrEmpty(searchString))
            {
                category = category.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            category = category.OrderBy(s => s.Name);
            var category1 = from s in context.NielitTrgSpecializations
                            select new { Name = s.specializationCode };
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