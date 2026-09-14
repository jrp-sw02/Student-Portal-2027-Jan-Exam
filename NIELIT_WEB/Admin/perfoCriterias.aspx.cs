using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Globalization;
public partial class perfoCriterias : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                    FillCourseCat();
                    FillCityType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCityType();
                    FillFilterCourseCat();
                    FillCourseCat();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Performance Criteria", "Admin/perfoCriterias.aspx", ""));
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
    protected void FillCourseCat()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select--", "0");
                //var locType = from p in context.LocationTypes
                //              where (p.ID != city  && p.ID!= zone && p.ID != tehsil )
                //              orderby (p.ID)
                //              select new { ValueField = p.ID, TextField = p.Name };
                var courseCat = (from p in context.CourseCategories 
                             // where p.ShowOnWeb
                              orderby p.ID
                              select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, courseCat, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillFilterCourseCat()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                
                ListItem lst = new ListItem("--All--", "0");
                
                var courseCat=(from p in context.CourseCategories  
                            //where p.ShowOnWeb 
                            orderby p.ID
                            select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategoryFilter, courseCat, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCityType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                             
                ListItem lst = new ListItem("--Select One--", "0");
                var cityType = from p in context.cityTypeMas 
                              orderby (p.ID)
                              select new { ValueField = p.ID, TextField = p.description   };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCityType , cityType , lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowEditMode()
    {
         EConnectContext context = new EConnectContext();
        try
        {
            EConnect.NIELIT.perfoCriteriaMas objPerfoCriteria = context.perfoCriteriaMas.Find(Convert.ToInt32(Request.QueryString["Key"]));

            if (objPerfoCriteria != null && objPerfoCriteria.effectiveToDate != null)
            {
                ShowAlert("Updation not allowed for selected record");
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                FillCityType();
                FillFilterCourseCat();
                FillCourseCat();
                BindGridView();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Performance Criteria", "Admin/perfoCriterias.aspx", ""));
                return;
            }
           
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Performance Criteria Details";
            //tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.

           


            ddlCityType.SelectedValue = objPerfoCriteria.cityTypeID.ToString();
            ddlCourseCategory.SelectedValue = objPerfoCriteria.courseCatID .ToString();
            txtMinCand.Text = objPerfoCriteria.minCand.ToString();
            txtPassPercent.Text = objPerfoCriteria.passPercent.ToString();
            txtExamCount.Text = objPerfoCriteria.examCount.ToString();

           txtEffectiveFrom.Text = objPerfoCriteria.effectiveFromDate.ToString("dd-MMM-yyyy");
              
          
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objPerfoCriteria.ID.ToString () , "Admin/perfoCriterias.aspx?Key=" + Request.QueryString["Key"], ""));
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
            
            if (objPerfoCriteria.effectiveToDate  == null)
            {
                txtEffectiveFrom.Enabled = true;
                ddlCourseCategory.Enabled = false;
                ddlCityType.Enabled = false;
                txtMinCand.Enabled = true;
                txtExamCount.Enabled = true;
                txtPassPercent.Enabled = true;

                btnSave.Enabled = true;
                
            }
            else
            {
                txtEffectiveFrom.Enabled = false;
                ddlCourseCategory.Enabled = false;
                ddlCityType.Enabled = false;
                txtMinCand.Enabled = false;
                txtExamCount.Enabled = false;
                txtPassPercent.Enabled = false;
                btnSave.Enabled = false;
                
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
    protected void BindGridView()
    { EConnectContext context = new EConnectContext();
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
           
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
          
            var query = from s in context.perfoCriteriaMas 
                       
                        select new
                        {
                            ID = s.ID,
                            courseCat=s.parentCourseCat.Name ,
                            cityType=s.parentCityType.description   ,
                            minCand=s.minCand ,
                            passPercent=s.passPercent ,
                            examCount=s.examCount ,
                            effFrom=s.effectiveFromDate ,
                            effTo=s.effectiveToDate
                                     
                        };
            if (!String.IsNullOrEmpty(searchString) && query !=null)
            {
                query = query.Where(s => s.courseCat.ToUpper().Contains(searchString)
                                       || s.cityType.ToUpper().Contains(searchString));
            }
            
            query = query.OrderBy(s => s.courseCat);
            if (!string.IsNullOrEmpty(sortOrder) && query!=null)
            {
                switch (sortField)
                {
                    case "courseCat":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.courseCat);
                        else
                            query = query.OrderBy(s => s.courseCat);
                        break;
                    case "cityType":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.cityType);
                        else
                            query = query.OrderBy(s => s.cityType);
                        break;
                    
                    default:
                        query = query.OrderBy(s => s.courseCat);
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
            lblHeading.Text = "New Performance Criteria";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Performance Criteria", "#", ""));
        }
        else
        {
            Response.Redirect("perfoCriterias.aspx", true);
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
         EConnectContext context = new EConnectContext();
        //create an object 
          perfoCriteriaMas objPerfoCriteria=new perfoCriteriaMas ();
        try
        {
            DateTime edate = DateTime.ParseExact(txtEffectiveFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                if( edate < System .DateTime .Today )
                {
                    ShowAlert ("Effective from date cannot be before today's date");
                    return;
                }
            BreadCrumb1.Render();
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                int cId=Convert.ToInt32(ddlCourseCategory.SelectedValue);
                int cityTypeId = Convert.ToInt32(ddlCityType.SelectedValue);
                //Added 12 Feb 2019
                var query = (from c in context.perfoCriteriaMas
                             where c.courseCatID.Equals(cId)
                                 && c.cityTypeID.Equals(cityTypeId)
                                 && c.effectiveToDate.ToString().Trim().Length.Equals(0)
                             select new { id = c.ID });
                //List < int> p= query.ToList();
                int count =query.Count();
                    if(count >0)
                    {
                        ShowAlert ("Criteria already exist for specified Course Category and City type, please update that");
                        return;
                    }


                objPerfoCriteria.courseCatID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                objPerfoCriteria.cityTypeID = Convert.ToInt32(ddlCityType.SelectedValue);
                objPerfoCriteria.passPercent = Convert.ToInt32(txtPassPercent.Text);
                objPerfoCriteria.examCount = Convert.ToInt32(txtExamCount.Text);
                objPerfoCriteria.minCand = Convert.ToInt32(txtMinCand.Text);

                if (txtEffectiveFrom.Text.Trim().Length == 0)
                    objPerfoCriteria.effectiveFromDate = System.DateTime.Today;
                else
                    objPerfoCriteria.effectiveFromDate = DateTime.ParseExact(txtEffectiveFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

                objPerfoCriteria.enterBy =Convert.ToInt32 ( Session["userID"]);
                objPerfoCriteria.enterDate = System.DateTime.Today;

                context.perfoCriteriaMas.Add(objPerfoCriteria);
                context.SaveChanges();
                strMessage = "New record saved.";
                context.Dispose();
            }

            else
            {

                ////Initialize current object by loading it and get its current modified date
                Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
                DateTime eDate = DateTime.ParseExact(txtEffectiveFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                objPerfoCriteria = context.perfoCriteriaMas.Find(KeyID);
                if (objPerfoCriteria.effectiveFromDate >= edate)
                {
                    ShowAlert("Please check effective From Date, Record not updated");
                    return;
                }



                if (objPerfoCriteria != null)
                {

                    if (objPerfoCriteria.effectiveToDate == null)
                    {
                        objPerfoCriteria.effectiveToDate = DateTime.ParseExact(txtEffectiveFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture).AddDays(-1);
                    }
                    context.SaveChanges();
                    objPerfoCriteria = new perfoCriteriaMas();
                    objPerfoCriteria.courseCatID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                    objPerfoCriteria.cityTypeID = Convert.ToInt32(ddlCityType.SelectedValue);
                    objPerfoCriteria.passPercent = Convert.ToInt32(txtPassPercent.Text);
                    objPerfoCriteria.examCount = Convert.ToInt32(txtExamCount.Text);
                    objPerfoCriteria.minCand = Convert.ToInt32(txtMinCand.Text);
                    objPerfoCriteria.effectiveToDate = null;

                    if (txtEffectiveFrom.Text.Trim().Length == 0)
                        objPerfoCriteria.effectiveFromDate = System.DateTime.Today;
                    else
                        objPerfoCriteria.effectiveFromDate = DateTime.ParseExact(txtEffectiveFrom.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    objPerfoCriteria.enterBy = Convert.ToInt32(Session["userID"]);
                    objPerfoCriteria.enterDate = System.DateTime.Today;

                    context.perfoCriteriaMas.Add(objPerfoCriteria);
                    context.SaveChanges();
                    strMessage = "Record updated.";
                }
            }
            
            Response.Redirect("perfoCriterias.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        { //context.Dispose(); }
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
            
            ddlCourseCategoryFilter.SelectedValue = "0";
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
                if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                    {
                        BreadCrumb1.Render();
                        ShowAlert("Sorry! You don't have rights to delete the records.", true);
                        return;
                    }
                    Location  location = context.Locations.Find(Convert.ToInt32(hfActionID.Value));
                    context.Locations.Remove(location);
                    context.SaveChanges();
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
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
            var query  = from s in context.perfoCriteriaMas 
                         join p in context.cityTypeMas 
                         on s.cityTypeID equals p.ID 
                         join q in context.CourseCategories 
                         on s.courseCatID  equals q.ID 
                        select new {  courseCat = q.Name   ,
                                       city=p.cityTypeName };

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.city .ToUpper().Contains(searchString) || s.courseCat .ToUpper ().Contains (searchString ));
            }
            query = query.OrderBy(s => s.courseCat );

           
            foreach (var user in query)
            {
                items.Add(user.courseCat );
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
            Response.Redirect("perfoCriterias.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
   
   
    
}