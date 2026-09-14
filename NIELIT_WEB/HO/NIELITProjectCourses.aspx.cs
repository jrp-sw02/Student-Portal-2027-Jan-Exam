using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

public partial class HO_NIELITProjectCourses : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    NIELITMISContext context1;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    ArrayList TempDataTable = new ArrayList();

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
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            entityID = Convert.ToInt32(Session["EntityID"]);


            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    BindProjects();

                    ShowEditMode();

                }
                else
                {
                    BindGridView();
                    //BindGridCourses();
                    BindProjects();
                    FillFilter();
                }
            }
            BindGridView();
            if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());
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


                var projList = from p in context.NielitProjCoursess
                               join c in context.NielitProjectss on p.projID equals c.ID
                               where p.projID == c.ID
                               select new { ValueField = c.ID, TextField = c.ProjectName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectFilter, projList.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillProjects()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var projList = from p in context.NielitProjectss
                               where System.DateTime.Today >= p.projectFromDate
                               && System.DateTime.Today <= p.projectTodate
                               select new { ValueField = p.ID, TextField = p.ProjectName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjects, projList, lst);
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
            lblHeading.Text = "ProjectWise Courses";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Project Courses :New", "#", ""));
        }
        else
        {
            //txtInstituteID.Enabled = false;
            Response.Redirect("NIELITProjectCourses.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gridProjectCourses.PageIndex = PagingBar1.CurrentPageIndex;
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
            gridProjectCourses.PageIndex = PagingBar1.CurrentPageIndex;
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
            gridProjectCourses.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
            gridProjectCourses.PageIndex = PagingBar1.CurrentPageIndex;
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
            ddlProjectFilter.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gridProjectCourses.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    //protected void gvMain_Sorting(object sender, GridViewSortEventArgs e) 
    //{
    //    try
    //    {
    //        ViewState["SortField"] = e.SortExpression;
    //        if (ViewState["SortOrder"].ToString() == "DESC")
    //            ViewState["SortOrder"] = "ASC";
    //        else
    //            ViewState["SortOrder"] = "DESC";
    //        BindGridView();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        NIELITMISContext context1 = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var projects = from s in context1.NielitProjectss
                           select new { Name = s.ProjectName };
            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            projects = projects.OrderBy(s => s.Name).Distinct();
            foreach (var project in projects)
            {
                items.Add(project.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context1.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NIELITProjectCourses.aspx", true);
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        int Countskip = 0;
        try
        {
            BreadCrumb1.Render();
            Int32 isactivecheck = Convert.ToInt32(ddlActive.SelectedValue);
            if (isactivecheck == -1)
            {
                throw new Exception("Please select IsActive ");
                return;
            }
            Int32 userid = Convert.ToInt32(Session["UserId"]);
            Int64 projID = Convert.ToInt64(ddlProjects.SelectedValue);
            Int32 projID1 = Convert.ToInt32(ddlProjects.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    foreach (ListItem li in cbCourses.Items)
                    {
                        if (li.Selected)
                        {
                            Int64 courseID = Convert.ToInt64(li.Value);


                            NielitProjCourses projCourses;
                            projCourses = new NielitProjCourses();

                            projCourses = new NielitProjCourses();

                            if (context.NielitProjCoursess.Where(s => s.projID == projID && s.courseID == courseID).Count() == 0)
                            {
                                projCourses.courseID = courseID;
                                projCourses.projID = projID;

                                if (ddlActive.SelectedValue == "1")
                                    projCourses.IsActive = true;
                                else
                                {
                                    projCourses.IsActive = false;
                                }
                                projCourses.enterDate = System.DateTime.Today;
                                projCourses.enterBy = userid;

                                context.NielitProjCoursess.Add(projCourses);
                                context.SaveChanges();
                                strMessage = "New record saved";

                            }
                            else
                            {
                                Countskip = Countskip + 1;
                                //Response.Redirect("NIELITProjectCourses.aspx?msg=" + li.Text + " Course is already allocated with the Project " + ddlProjects.SelectedItem.ToString().ToUpper(), true);

                            }

                        }
                    }

                }
                else
                {
                    Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
                    NielitProjCourses projCourses;

                    bool chkupdte = validateupdate();
                    if (chkupdte == true)
                    {
                        projCourses = context.NielitProjCoursess.Find(Convert.ToInt32(Request.QueryString["Key"]));


                        foreach (ListItem li in cbCourses.Items)
                        {
                            Int64 courseID = Convert.ToInt64(li.Value);
                            if (li.Selected)
                            {
                                //if (context.NielitCentreBatchs.Where(s => s.CourseDurationID == projCourses.courseID).Count() == 0)
                                //{
                                    //if (context.NielitProjCoursess.Where(s => s.projID == projID && s.courseID == courseID).Count() == 0)
                                    //{
                                    projCourses.courseID = courseID;
                                    projCourses.projID = projID;
                                    if (ddlActive.SelectedValue == "1")
                                        projCourses.IsActive = true;
                                    else
                                    {
                                        projCourses.IsActive = false;
                                    }
                                    projCourses.enterDate = System.DateTime.Today;
                                    projCourses.enterBy = userid;

                                    context.SaveChanges();
                                    strMessage = "Record updated";

                                    //}

                                    //else
                                    //{
                                    //    strMessage = "Selected courses is already exists. Please select another to update";
                                    //}
                               // }
                               // else
                                //{
                                 //   strMessage = "You cannot update because Batch is created with this course.";
                                //}
                            }

                        }


                    }

                    //}
                    //else
                    //{
                    //    Response.Redirect("NIELITProjectCourses.aspx?msg=" + "Batch is already created Not able to update the course");
                    //}



                    else
                    {
                        strMessage = "You have selected multiple courses to update. Please select single course to update.";
                    }
                }

            }
            if (Countskip == 0)
            {
                Response.Redirect("NIELITProjectCourses.aspx?msg=" + strMessage);
            }
            else
            {
                Response.Redirect("NIELITProjectCourses.aspx?msg=" + strMessage + "Already allocated courses not saved. ");
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected bool validateupdate()
    {
        int count = 0;
        bool update = false;
        foreach (ListItem li in cbCourses.Items)
        {
            if (li.Selected)
            {
                count = count + 1;
                if (count > 1)
                {
                    update = false;
                    break;
                }
                else
                {
                    update = true;
                }
            }
        }
        return update;

    }

    protected void ShowEditMode()
    {

        try
        {
            NIELITMISContext context = new NIELITMISContext();
            context = new NIELITMISContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            //mltvTab.ActiveViewIndex = 1;
            //pnlFilter.Visible = false;
            //ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Project Courses Details";
            NielitProjCourses ProjCourses = context.NielitProjCoursess.Find(Convert.ToInt64(Request.QueryString["Key"]));
            ddlProjects.SelectedValue = ProjCourses.projID.ToString();
            ddlProjects_SelectedIndexChanged(ddlProjects, EventArgs.Empty);
            if (ProjCourses.IsActive == true)
                ddlActive.SelectedValue = "1";
            else
            {
                ddlActive.SelectedValue = "0";
            }
            foreach (ListItem li in cbCourses.Items)
            {
                if (li.Value == ProjCourses.courseID.ToString())
                {
                    li.Selected = true;
                }
            }


            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(ProjCourses..ToString(), "", ""));
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
            //context.Dispose();
        }
    }

    protected void gridProjectCourses_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void gridProjectCourses_RowDataBound(object sender, GridViewRowEventArgs e)
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
                imgAction.ID = "imgAction_" + gridProjectCourses.DataKeys[e.Row.RowIndex].Values[0].ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gridProjectCourses.DataKeys[e.Row.RowIndex].Values[0].ToString();
            }
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
            lblError.Visible = false;

            context = new EConnectContext();

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlProjectFilter.SelectedValue != "0")
                stateID = Convert.ToInt64(ddlProjectFilter.SelectedValue);

            DataTable DT = new DataTable();

            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("BindGrid", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);


                Sda.Fill(DT);
            }
            con.Close();

            if (!String.IsNullOrEmpty(searchString))
            {
                string expression = "[ProjectName] like '%" + searchString + "%'";

                if (DT.Select(expression).Count() > 0)
                    DT = DT.Select(expression).CopyToDataTable();
                else
                {
                    ShowAlert("No record found", true);

                }

            }

            if (ddlProjectFilter.SelectedValue.ToString() != "0")
            {
                string expression = "[PID]=" + Convert.ToInt32(ddlProjectFilter.SelectedValue);
                if (DT.Select(expression).Count() > 0)
                    DT = DT.Select(expression).CopyToDataTable();
                else
                {
                    ShowAlert("No record found", true);

                }
            }

            PagingBar1.Bind(DT, ref gridProjectCourses);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gridProjectCourses.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            context.Dispose();
        }
    }

    protected void BindProjects()
    {
        try
        {
            NonAffInstitute objCentre = null;
            context1 = new NIELITMISContext();
            ListItem lst = new ListItem("--Select One--", "0");
            var state = from s in context1.NielitProjectss

                        select new { ValueField = s.ID, TextField = s.ProjectName };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjects, state, lst);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            context1.Dispose();
        }
    }

    protected void BindGridCourses()
    {
        try
        {
            lblError.Visible = false;
            //context1 = new NIELITMISContext();

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            DataTable DT = new DataTable();

            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("BindCourse", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
            con.Close();

            //checkboxSelectCombo = DT;

            //lstFruits.DataSource = DT;
            //lstFruits.DataTextField = "Name";
            //lstFruits.DataValueField = "ID";
            //lstFruits.DataBind();

            //multiselect.DataSource = DT;
            //multiselect.DataTextField = "Name";
            //multiselect.DataValueField = "ID";
            //multiselect.DataBind();

            cbCourses.DataSource = DT;
            cbCourses.DataTextField = "Name";
            cbCourses.DataValueField = "ID";


            cbCourses.DataBind();


            uPnlGrid.Update();
            uPnlNavigation.Update();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            //context.Dispose();
        }
    }

    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }

    protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (ddlProjects.SelectedIndex == 0)
            {
                cbCourses.Visible = false;
                allChkBox.Visible = false;
            }
            else
            {
                BindGridCourses();
                cbCourses.Visible = true;
                allChkBox.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void allChkBox_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem chkitem in cbCourses.Items)
        {
            if (allChkBox.Checked == true)
            {
                chkitem.Selected = true;
            }
            else
            {
                chkitem.Selected = false;
            }
        }
    }


}