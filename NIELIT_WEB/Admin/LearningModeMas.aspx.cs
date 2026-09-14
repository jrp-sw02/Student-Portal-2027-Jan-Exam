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

public partial class Admin_LearningModeMas : BasePage
{
    NIELITMISContext context;
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0 ; 
    Int64 loginUserNo = 0 ;  
    Int64 entityID = 0 ; 

    Int32 UserTypeId = 0 ; 

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

           // currentRoleId = 21;// Convert.ToInt32(Session["RoleID"]);
           // loginUserNo = 349792;// Convert.ToInt32(Session["UserID"]);
            //entityID = 5023;// Convert.ToInt64(Session["EntityID"]);
            //UserTypeId = 10;// Convert.ToInt32(Session["UserTypeId"]);      

            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
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

                    // Unable to Get It 
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Learning Mode Master", "Admin/LearningModeMas.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Learning Mode Master", "Admin/LearningModeMas.aspx", "")) ; 
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

            context = new NIELITMISContext();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Learning Mode Master :Update", "", ""));
            BreadCrumb1.Render();

            Int64 NielitLearnid = Convert.ToInt64(Request.QueryString["key"]);
            ////Int32 NielitSecid = Convert.ToInt32(Request.QueryString["key"]);

            var NielitSec = (from c in context.LearningModeMass
                             where c.ID == NielitLearnid
                             select new
                             {
                                 LearningCode = c.Code,
                                 LearningName = c.Name,
                                 LearningDescription = c.Description

                             }).FirstOrDefault();



            ////var NielitSec = (from c in context.NielitSectorss
            ////                 where c.ID == NielitSecid
            ////                 select new
            ////                 {
            ////                     sectorShortCode = c.sectorShortCode,
            ////                     sectorName = c.sectorName,
            ////                     sectorDescription = c.sectorDescription

            ////                 }).FirstOrDefault() ; 

            txtLearningCode.Enabled = false ; 
            btnMode.Visible = true ; 
            btnSubmit.Text = "Update" ; 
            btnMode.ViewMode = ToggleView.Mode.List ; 
            mltvTab.ActiveViewIndex = 1 ; 
            pnlFilter.Visible = false ; 
            ucSearchBar.Visible = false ; 
            lblHeading.Text = "Learning Mode Master Details" ; 
            //Get last modified date of current record and save it in ViewState object.
            //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            txtLearningName.Text = NielitSec.LearningName;
            txtLearningCode.Text = NielitSec.LearningCode;
            txtDesc.Text = NielitSec.LearningDescription;
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
                //BindGridView();
                //uPnlGrid.Update();
                //BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int64 id = Convert.ToInt64(hfActionID.Value);

                    LearningModeMas LearningModes = context.LearningModeMass.Find(id);
                    ////NielitSectors Sectors = context.NielitSectorss.Find(id);

                    context.LearningModeMass.Remove(LearningModes);
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 StatusID = 0;
                Int64 Code = 0;
                if (ddlLearningName.SelectedValue != "0")
                    StatusID = Convert.ToInt64(ddlLearningName.SelectedValue);
                if (ddlCode.SelectedValue != "0")
                    Code = Convert.ToInt64(ddlCode.SelectedValue);
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                string ddlStatusNames = "";
                string ddlCodes = "";

                if (ddlLearningName.SelectedValue != "0")
                    ddlStatusNames = ddlLearningName.SelectedItem.ToString();
                if (ddlCode.SelectedValue != "0")
                    ddlCodes = ddlCode.SelectedItem.ToString();

                var feeGroupMass = from s in context.LearningModeMass
                                   orderby s.Name
                                   select new
                                   { 
                                       ID = s.ID, 
                                       LearningCode = s.Code, 
                                       LearningName = s.Name,
                                       LearningDescription = s.Description 
                                   } ; 

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
                    feeGroupMass = feeGroupMass.Where(s => s.LearningName.ToUpper().Contains(searchString) || s.LearningCode.ToUpper().Contains(searchString));
                }
                feeGroupMass = feeGroupMass.OrderBy(s => s.LearningName);
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
                        case "LearningName":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.LearningName);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.LearningName);
                            break;
                        case "LearningCode":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.LearningCode);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.LearningCode);
                            break;

                        case "LearningDescription":
                            if (sortOrder == "DESC")
                                feeGroupMass = feeGroupMass.OrderByDescending(s => s.LearningDescription);
                            else
                                feeGroupMass = feeGroupMass.OrderBy(s => s.LearningDescription);
                            break;

                        default:
                            feeGroupMass = feeGroupMass.OrderBy(s => s.LearningName);
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusName = from p in context.LearningModeMass 
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = (p.Name +"-"+p.Description) } ; 

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLearningName, StatusName, lst);
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
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            //BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Learning Mode Master";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Learning Mode Master", "#", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("LearningModeMas.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString()), true);
            }
            else
            {
                Response.Redirect("LearningModeMas.aspx", true);
            }
        }
    }

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlLearningName.SelectedValue = "0";

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
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("LearningModeMas.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("LearningModeMas.aspx", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();


            if (String.IsNullOrEmpty(txtLearningCode.Text.Trim()))
            {
                lbl1.Text = "Please Enter Learning Mode Code.";
            }
            else if (String.IsNullOrEmpty(txtLearningName.Text))
            {
                lbl1.Text = "Please Enter Learning Mode Name.";

            }
            //else if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
            //{
            //    lbl1.Text = "Please Enter Sector Description.";
            //}

            else
            {
                try
                {
                    context = new NIELITMISContext();
                    LearningModeMas application;

                    //Nullable<string> Todate = null;
                    Int64 userid = Convert.ToInt64(Session["UserId"]);
                    DateTime entryDate = DateTime.Now;
                    String LearningCode = txtLearningCode.Text;
                    String LearningName = txtLearningName.Text;
                    String LearningDesc = txtDesc.Text;
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
                        if (context.LearningModeMass.Count((s) => s.Code == LearningCode.Trim()) == 0)
                        {
                            application = new LearningModeMas() ; 

                            application.Code = LearningCode; 
                            application.Name = LearningName;
                            if (LearningDesc == "")
                            {
                                application.Description = " ";
                            }
                            else
                            {
                                application.Description = LearningDesc ; 
                            }
                            application.enterDate = entryDate;
                            application.enterBy = userid;
                            context.LearningModeMass.Add(application);
                            context.SaveChanges();
                            strMessage = "New Record Saved";

                        }
                        else
                        {
                            //strMessage = "Person is already active in this period. Please select valid date  ";
                            //Response.Redirect("NielitSectors.aspx?msg=" + "Sector Short Code already exists.", true);
                            ShowAlert("Learning Mode Code already exists", true);
                            lbl1.Text = "Learning Mode Code already exists";
                            return;
                        }
                    }
                    else
                    {
                        Int64 id = Convert.ToInt64(Request.QueryString["key"]);
                        application = context.LearningModeMass.Find(Convert.ToInt64(Request.QueryString["Key"]));

                        application.Code = LearningCode;
                        application.Name = LearningName;

                        if (LearningDesc == "")
                        {
                            application.Description = " ";
                        }
                        else
                        {
                            application.Description = LearningDesc;
                        }

                        application.enterDate = entryDate;
                        application.enterBy = userid;

                        context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                        //context.NielitSectorss.Add(application);
                        context.SaveChanges();
                        strMessage = "Record updated.";
                    }

                    Response.Redirect("LearningModeMas.aspx?msg=" + strMessage, true);


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
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var category = from s in context.LearningModeMass
                           select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                category = category.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            category = category.OrderBy(s => s.Name);
            var category1 = from s in context.LearningModeMass
                            select new { Name = s.Code };
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