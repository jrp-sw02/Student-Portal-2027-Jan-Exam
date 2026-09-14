using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_Occupation : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.QueryString.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                FillCourses();
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilterOccupation();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Occupation", "Admin/Occupation.aspx", ""));
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
    protected void FillFilterOccupation()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var OccupationList = from p in context.Occupations
                                   orderby p.ID
                                   select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlOccupation, OccupationList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CertifcationExam = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("All", "0");
                var courses = from p in context.Courses
                                     where p.CourseTypeID == CertifcationExam
                                     orderby p.ID
                                     select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(chkcourses, courses);
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
            lblHeading.Text = "Occupation";
            //tblNavLinks.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 OccupationID = Convert.ToInt32(Request.QueryString["Key"]);
                var objOccupation = (from s in context.Occupations
                                     where s.ID == OccupationID
                                   select s).FirstOrDefault();
                txtCode.Text = objOccupation.Code.ToString();
                txtOccupation.Text = objOccupation.Name.ToString();
                txtDisplayOrder.Text = objOccupation.DisplayOrder.ToString();
                txtDisplayOrder.Enabled = false;
                //if (objOccupation.CourseID.HasValue == true)
                //    ddlcourse.SelectedValue = objOccupation.CourseID.Value.ToString();
                //else
                //    ddlcourse.SelectedValue = "0";

                //displaying selected courses
                btnUpdate.CommandArgument = OccupationID.ToString();
                var filldata = (from f in context.CourseWiseOccupationMappings
                                where f.OccupationID == OccupationID
                                select new { CourseID = f.CourseID }).ToList();
                if (filldata.Count > 0)
                {
                    foreach (var s in filldata)
                    {
                        foreach (ListItem sli in chkcourses.Items)
                        {
                            if (s.CourseID == Convert.ToInt64(sli.Value))
                            {
                                sli.Selected = true;
                            }
                        }

                    }
                }

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objOccupation.Name, "#", ""));
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
                Int32 OccupationID = 0;
                if (ddlOccupation.SelectedValue != "0")
                    OccupationID = Convert.ToInt32(ddlOccupation.SelectedValue);
                var objOccupation = from s in context.Occupations
                                  orderby s.DisplayOrder
                                  select new
                                  {
                                      ID = s.ID,
                                      Name = s.Name,
                                      Code = s.Code,
                                      Active = s.IsEnabled ? "Active" : "InActive",

                                  };
                if (!String.IsNullOrEmpty(searchString))
                {
                    objOccupation = objOccupation.Where(s => s.Name.ToUpper().Contains(searchString)
                                                        || s.Code.ToUpper().Contains(searchString));
                }
                if (OccupationID != 0)
                {
                    objOccupation = objOccupation.Where(s => s.ID == OccupationID);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                objOccupation = objOccupation.OrderByDescending(s => s.ID);
                            else
                                objOccupation = objOccupation.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                objOccupation = objOccupation.OrderByDescending(s => s.Name);
                            else
                                objOccupation = objOccupation.OrderBy(s => s.Name);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                objOccupation = objOccupation.OrderByDescending(s => s.Code);
                            else
                                objOccupation = objOccupation.OrderBy(s => s.Code);
                            break;
                        case "Active":
                            if (sortOrder == "DESC")
                                objOccupation = objOccupation.OrderByDescending(s => s.Active);
                            else
                                objOccupation = objOccupation.OrderBy(s => s.Active);
                            break;
                        default:
                            objOccupation = objOccupation.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(objOccupation, ref gvMain);
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
                lblHeading.Text = "Occupation";
                //Updating Breadcrumb
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Occupation", "#", ""));
            }
            else
            {
                Response.Redirect("Occupation.aspx", true);
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
                Occupation objOccupation;
                Int32 DispalyOrder = Convert.ToInt32(txtDisplayOrder.Text);
                string occupationname = txtOccupation.Text;
                string occupationcode = txtCode.Text;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (context.Occupations.Any(s => s.Name.ToUpper() == occupationname.ToUpper()))
                    {
                        throw new Exception("This occupation name already exists.");
                    }
                    else if (context.Occupations.Any(s => s.Code.ToUpper() == occupationcode.ToUpper()))
                    {
                        throw new Exception("This occupation code already exists. ");
                    }
                    else if (context.Occupations.Any(s => s.DisplayOrder == DispalyOrder))
                    {
                        throw new Exception("This occupation display order already exists.");
                    }
                    else
                    {
                        objOccupation = new EConnect.Occupation();
                        objOccupation.Name = Convert.ToString(txtOccupation.Text.Trim());
                        objOccupation.Code = txtCode.Text.ToString().ToUpper().Trim();
                        objOccupation.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                        //objOccupation.CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                        context.Occupations.Add(objOccupation);
                        context.SaveChanges();
                        strMessage = "New record saved";
                    }

                }
                else
                {
                    Int32 keyID = Convert.ToInt32(Request.QueryString["Key"]);
                    if (!(context.Occupations.Any(s => s.Name.ToUpper() == occupationname.ToUpper() && s.Code.ToUpper() == occupationcode.ToUpper() && s.ID!=keyID )))
                    {
                        objOccupation = context.Occupations.Find(Convert.ToInt32(Request.QueryString["key"]));
                        objOccupation.Name = Convert.ToString(txtOccupation.Text.Trim());
                        objOccupation.Code = txtCode.Text.ToString().ToUpper().Trim();
                        //objOccupation.CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                        //objOccupation.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                        context.SaveChanges();
                        strMessage = "Record updated";
                    }
                    else
                    {
                        throw new Exception("This Occupation Already Exist");
                    }

                }
                Response.Redirect("Occupation.aspx?msg="+strMessage);
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
            ddlOccupation.SelectedValue = "0";
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
                Occupation occupation = context.Occupations.Find(Convert.ToInt32(hfActionID.Value));
                context.Occupations.Remove(occupation);
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
            var OccupationList = from s in context.Occupations
                              select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                OccupationList = OccupationList.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            OccupationList = OccupationList.OrderBy(s => s.Name);

            var OccupationList1 = from s in context.Occupations
                               select new { Name = s.Code };
            if (!String.IsNullOrEmpty(searchString))
            {
                OccupationList1 = OccupationList1.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            OccupationList1 = OccupationList1.OrderBy(s => s.Name);

            OccupationList = OccupationList.Union(OccupationList1).Take(count);
            foreach (var c in OccupationList)
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
            Response.Redirect("Occupation.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);       
        }
    }
    protected void lbdisable_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to edit the records.", true);
                    return;
                }
                Occupation occupation = context.Occupations.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                if (occupation.IsEnabled == true)
                    occupation.IsEnabled = false;
                else
                    occupation.IsEnabled = true;
                context.Entry(occupation).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                BindGridView();
                ShowAlert("You have successfully changed the status of the Occupation.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be edited!", true);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 Occupation = Convert.ToInt32(btnUpdate.CommandArgument);
            using (EConnectContext context = new EConnectContext())
            {
                context.Database.ExecuteSqlCommand("Delete from CourseWiseOccupation Where Occupation_ID =" + Occupation);
                CourseWiseOccupationMapping Mapping;
                foreach (ListItem l in chkcourses.Items)
                {
                    if (l.Selected)
                    {
                        Mapping = new EConnect.NIELIT.CourseWiseOccupationMapping();
                        Mapping.OccupationID = Occupation;
                        Mapping.CourseID = Convert.ToInt32(l.Value);
                        Mapping.CourseCategoryID = context.Courses.Find(Convert.ToInt32(l.Value)).CourseCategoryID;
                        context.CourseWiseOccupationMappings.Add(Mapping);
                    }
                } 
                context.SaveChanges();
            };
            ShowAlert("List updated", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}