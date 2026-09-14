using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect;

public partial class HO_ProjectMainCentre : BasePage
{
    String strMessage = string.Empty;
    NIELITMISContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeid = 0;
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
            UserTypeid = Convert.ToInt32(Session["UserType"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }


            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);

                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        FillCategoriesupdate();
                        //FillCategories();
                        FillFilter();
                        ShowEditMode();
                    }
                    else
                    {
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        FillCategories();
                        FillFilter();
                        BindGridView();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Main Centre", "HO/ProjectMainCentre.aspx", ""));
                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void FillFilter()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var ProjectName = from p in context.projectMainCentres
                                join c in context.NielitProjectss
                                on p.projectID equals c.ID
                                select new { ValueField = p.ID, TextField = c.ProjectName };


                //var mylist = string.Concat(statelist,CourseList);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlfillProcname, ProjectName.Distinct(), lst);
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
            //txtInstituteID.Enabled = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Main Centre";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Main Centre", "#", ""));
        }
        else
        {
            //txtInstituteID.Enabled = false;
            Response.Redirect("ProjectMainCentre.aspx", true);
        }
    }

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlfillProcname.SelectedValue = "0";
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
                hl1.NavigateUrl = hl.NavigateUrl;

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
        try
        {
            Response.Redirect("ProjectMainCentre.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void FillCategories()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Proc = from t in context.NielitProjectss
                              orderby (t.ProjectName)
                           where (DateTime.Now >=  t.projectFromDate && t.projectTodate >= DateTime.Now)
                              select new { ValueField = t.ID, TextField = t.ProjectName };

                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlfillProcname, Taxtype, lst);

                //var Proc = from t in context.NielitProjectss
                //           join k in context.projectMainCentres on t.ID equals k.projectID
                //           orderby (t.ProjectName)
                //           select new { ValueField = t.ID, TextField = t.ProjectName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);


                var Center = from t in context.NielitCentres
                             orderby (t.Name)
                             select new { ValueField = t.ID, TextField = t.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitcenter, Center, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCategoriesupdate()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Proc = from t in context.NielitProjectss
                           orderby (t.ProjectName)
                           select new { ValueField = t.ID, TextField = t.ProjectName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);


                var Center = from t in context.NielitCentres
                             orderby (t.Name)
                             select new { ValueField = t.ID, TextField = t.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitcenter, Center, lst);

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
            context = new NIELITMISContext();
            Int32 PID = 0;
            if (ddlfillProcname.SelectedValue != "0")
                PID = Convert.ToInt32(ddlfillProcname.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var taxmaster = from m in context.projectMainCentres
                            join r in context.NielitProjectss on m.projectID equals r.ID
                            join q in context.NielitCentres on m.centreID equals q.ID
                            orderby m.ID
                            //where m.enterBy == loginUserNo 
                            select new
                            {
                                ID = m.ID,
                                ProjectName = r.ProjectName,//context.NielitProjectss.Where(s => s.ID == m.projectID).Select(k => k.ProjectName),
                                Centre = q.Name, //context.NielitCentres.Where(s => s.ID == m.centreID).Select(k => k.Name),
                                FromDate = m.allocatedFromDate,
                                Budget = m.budgetAllocated,
                                Adhaar = m.isAadharAuthenticationReqd
                            };
            if (PID != 0)
            {
                taxmaster = taxmaster.Where(s => s.ID == PID);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                taxmaster = taxmaster.Where(s => s.ProjectName.ToString().Contains(searchString)
                                                || s.Centre.ToUpper().Contains(searchString)); ;
            }
            taxmaster = taxmaster.OrderBy(s => s.ID);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ProjectName":
                        if (sortOrder == "DESC")
                            taxmaster = taxmaster.OrderByDescending(s => s.ProjectName);
                        else
                            taxmaster = taxmaster.OrderBy(s => s.ProjectName);
                        break;
                    case "Centre":
                        if (sortOrder == "DESC")
                            taxmaster = taxmaster.OrderByDescending(s => s.Centre);
                        else
                            taxmaster = taxmaster.OrderBy(s => s.Centre);
                        break;

                    case "FromDate":
                        if (sortOrder == "DESC")
                            taxmaster = taxmaster.OrderByDescending(s => s.FromDate);
                        else
                            taxmaster = taxmaster.OrderBy(s => s.FromDate);
                        break;

                    default:
                        taxmaster = taxmaster.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(taxmaster, ref gvMain);
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
            context = new NIELITMISContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Main Centre Details";
            projectMainCentre MainCentre = context.projectMainCentres.Find(Convert.ToInt32(Request.QueryString["Key"]));

            ddlProcname.Enabled = false;
            ddlNielitcenter.Enabled = false;
 
            ddlProcname.SelectedValue = MainCentre.projectID.ToString();
            ddlNielitcenter.SelectedValue = MainCentre.centreID.ToString();
            txtEffectiveFromDate.Text = MainCentre.allocatedFromDate.ToShortDateString();

            txtEffectiveToDate.Text = MainCentre.allocatedTodate.ToShortDateString();

            txtBudget.Text = MainCentre.budgetAllocated.ToString();

            if (MainCentre.isAadharAuthenticationReqd == true)
            {
                ddlAdhaar.SelectedValue = "1";
                ddlAdhaar.Enabled = false;
            }
            else
            {
                ddlAdhaar.SelectedValue = "2";
            }

            Int64 Pid = Convert.ToInt64(MainCentre.projectID.ToString());
            Int64 cid = Convert.ToInt64(MainCentre.centreID.ToString());

            //var StudentID = (from c in context.NielitCentreStudent where c.projectId == Pid && c.InstituteID == cid select c).Count();
            if ((from c in context.NielitCentreStudent where c.projectId == Pid && c.InstituteID == cid select c).Count() > 0)
            {
                //txtEffectiveFromDate.Enabled = false;
                //txtEffectiveToDate.Enabled = false;
                //txtBudget.Enabled = false;
                //ddlAdhaar.Enabled = false;
                //btnSave.Visible=false;
                txtEffectiveFromDate.Enabled = false;
                txtEffectiveToDate.Enabled = true;
                txtBudget.Enabled = false;
                ddlAdhaar.Enabled = false;
                btnSave.Visible = true;
            }
			else
			{
				btnSave.Visible=true;
			}

            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Update Main Centre", "", ""));
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
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

    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                projectMainCentre MainCentre = context.projectMainCentres.Find(Convert.ToInt32(hfActionID.Value));
                context.projectMainCentres.Remove(MainCentre);
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
            ShowAlert("Record can not be delted!");
        }
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (NIELITMISContext context = new NIELITMISContext())
            {

                projectMainCentre NielitProject;

                if (isvalidForm())
                {
                    Int64 PID = Convert.ToInt64(ddlProcname.SelectedValue);
                    DateTime Efrm = Convert.ToDateTime(txtEffectiveFromDate.Text.ToString());
                    DateTime ETo = Convert.ToDateTime(txtEffectiveToDate.Text.ToString());

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {

                        NielitProject = new projectMainCentre();

                        //Check Validation 

                        var ProcID = (from c in context.NielitProjectss
                                      where c.ID == PID
                                      select c).FirstOrDefault();

                        if (ProcID.ID != 0)
                        {           //   27 aug <= 30 aug               29 aug <= 29 aug 
                            if (Efrm <= ETo)
                            {
                                if ((ProcID.projectFromDate <= Efrm && Efrm <= ProcID.projectTodate) && (ProcID.projectFromDate <= ETo && ETo <= ProcID.projectTodate))
                                {
                                    if (ProcID.budgetAllocated < Convert.ToInt64(txtBudget.Text))
                                    {
                                        ShowAlert("Budget cannot be more than total budget of project: " + ProcID.budgetAllocated.ToString());
                                        return;
                                    }
                                    NielitProject.allocatedFromDate = Efrm;
                                    NielitProject.projectID = PID;
                                    NielitProject.centreID = Convert.ToInt64(ddlNielitcenter.SelectedValue);
                                    NielitProject.allocatedTodate = ETo;
                                    NielitProject.budgetAllocated = Convert.ToInt64(txtBudget.Text);

                                    if (ddlAdhaar.SelectedItem.Text == "Yes")
                                    {
                                        NielitProject.isAadharAuthenticationReqd = true;
                                    }
                                    else
                                    {
                                        NielitProject.isAadharAuthenticationReqd = false;
                                    }

                                    NielitProject.enterDate = DateTime.Now;
                                    NielitProject.enterBy = Convert.ToInt32(Session["UserID"]);

                                    context.projectMainCentres.Add(NielitProject);
                                    context.SaveChanges();
                                    strMessage = "New record saved";
                                }
                                else
                                {
                                    strMessage = "Project date is out of range. Please select Date From : " + ProcID.projectFromDate.ToShortDateString() + " To : " + ProcID.projectTodate.ToShortDateString();
                                    //Response.Redirect("ProjectMainCentre.aspx?msg=" + strMessage);
                                    ShowAlert(strMessage);
                                    return;
                                    //Response.Redirect("RegAuthSign.aspx?msg=" + "Person is already active in this period. Please select valid date  ", true);
                                }

                            }
                            else
                            {
                                strMessage = "From Date should be greater than To Date";
                                ShowAlert(strMessage);
                                // Response.Redirect("ProjectMainCentre.aspx?msg=" + strMessage);
                                return;
                            }
                        }
                    }

                    else
                    {
                        Int64 KeyID = Convert.ToInt32(Request.QueryString["key"]);

                        //Check Validation 

                        var ProcID = (from c in context.NielitProjectss
                                      where c.ID == PID
                                      select c).FirstOrDefault();

                        if (ProcID.ID != 0)
                        {
                            if (ProcID.projectFromDate <= Efrm && ProcID.projectTodate >= ETo)
                            {
                                if (ProcID.budgetAllocated < Convert.ToInt64(txtBudget.Text))
                                {
                                    ShowAlert("Budget cannot be more than total budget of project: " + ProcID.budgetAllocated.ToString());
                                    return;
                                }
                                NielitProject = context.projectMainCentres.Find(Convert.ToInt32(Request.QueryString["Key"]));
                                NielitProject.allocatedTodate = DateTime.Today.AddDays(-1);
                                context.Entry(NielitProject).State = System.Data.Entity.EntityState.Modified;

                                NielitProject.allocatedFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text);
                                NielitProject.allocatedTodate = Convert.ToDateTime(txtEffectiveToDate.Text);
                                NielitProject.projectID = Convert.ToInt64(ddlProcname.SelectedValue);
                                NielitProject.centreID = Convert.ToInt64(ddlNielitcenter.SelectedValue);
                                //NielitProject.allocatedTodate = null;
                                NielitProject.budgetAllocated = Convert.ToInt64(txtBudget.Text);

                                if (ddlAdhaar.SelectedItem.Text == "Yes")
                                {
                                    NielitProject.isAadharAuthenticationReqd = true;
                                }
                                else
                                {
                                    NielitProject.isAadharAuthenticationReqd = false;
                                }

                                NielitProject.enterDate = DateTime.Now;
                                NielitProject.enterBy = Convert.ToInt32(Session["UserID"]);

                                //  context.projectMainCentres.Add(NielitProject);
                                context.SaveChanges();
                                strMessage = "New record updated";
                            }
                            else
                            {
                                strMessage = "Project date is out of range. Please select Date From : " + ProcID.projectFromDate.ToShortDateString() + " To : " + ProcID.projectTodate.ToShortDateString();
                                //Response.Redirect("ProjectMainCentre.aspx?msg=" + strMessage);
                                ShowAlert(strMessage);
                                return;

                                //Response.Redirect("RegAuthSign.aspx?msg=" + "Person is already active in this period. Please select valid date  ", true);
                            }
                        }
                        //}
                        //else
                        //{
                        //    strMessage = "This record is use in today so it can be updated in next day.";
                        //    ShowAlert(strMessage, true);
                        //}

                    }
                }
                Response.Redirect("ProjectMainCentre.aspx?msg=" + strMessage);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
            var ProcName = from s in context.NielitProjectss
                           join k in context.projectMainCentres on s.ID equals k.projectID
                          select new { Name = s.ProjectName };
            if (!String.IsNullOrEmpty(searchString))
            {
                ProcName = ProcName.Where(s => s.Name.Contains(searchString));
            }
            ProcName = ProcName.OrderBy(s => s.Name);

            var ProcName1 = from s in context.projectMainCentres
                            join k in context.NielitCentres on s.centreID equals k.ID
                            select new { Name = k.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                ProcName1 = ProcName1.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            ProcName1 = ProcName1.OrderBy(s => s.Name);

            ProcName = ProcName.Union(ProcName1).Take(count);

            foreach (var linkName in ProcName)
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

    protected Boolean isvalidForm()
    {
        try
        {
            if (ddlProcname.SelectedValue == "0")
            {
                strMessage = "Please select Project Name.";
                ShowAlert(strMessage, true);
                return false;
            }

            if (ddlNielitcenter.SelectedValue == "0")
            {
                strMessage = "Please select Nielit Center Name.";
                ShowAlert(strMessage, true);
                return false;
            }


            if (string.IsNullOrEmpty(txtEffectiveFromDate.Text.Trim()))
            {
                strMessage = "Please Effective From Date ";
                ShowAlert(strMessage, true);
                return false;
            }

            if (ddlAdhaar.SelectedValue == "0")
            {
                strMessage = "Please select Nielit Center Name.";
                ShowAlert(strMessage, true);
                return false;
            }


            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlProcname_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            NIELITMISContext context1 = new NIELITMISContext();
            Int32 projectID = 0;
            projectID =  Convert.ToInt32(ddlProcname.SelectedValue);
            var Proj = from p in context1.NielitProjectss
                       where p.ID == projectID
                       select new { Adhaar =  p.isAadharAuthenticationReqd  };

            var application = context1.NielitProjectss.Find(projectID);

            if (application.isAadharAuthenticationReqd == true)
            {
                ddlAdhaar.SelectedValue = "1";
                ddlAdhaar.Enabled = false;
            }
            else
            {
                ddlAdhaar.Enabled = true;
            }

                        

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}