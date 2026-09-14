using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
public partial class insttAccrStatusByInstWise : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                Response.Redirect("../../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/insttAccrStatusByInstWise.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                hfAccreID.Value = Request.QueryString["key1"];
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
			  BindGridView();
                    FillCategories();
                    bindState();
                }
                else
                {                    
                    FillCategories();
                    bindState();                  
                   // BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Accrediation Detail", "Admin/accrediationdetails.aspx?" + Request.QueryString.ToString(), ""));
                }
                //if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                //    ShowAlert(Request.QueryString["msg"].ToString());
            }
           // BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };                
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategry, Category.Distinct(), lst);
                ddlcategry.SelectedValue = "9"; // Actual 2 or 9 category is to be considered
                ddlcategry.Enabled = false;
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
        
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();           
            Int32 instId = 0;


            if (DdlAccCentre.SelectedValue != "0")
                instId = Convert.ToInt32(DdlAccCentre.SelectedValue);
            string vAccrNo = txtAccrNo.Text;
            var inst = from a in context.AccreditationDetails
                       where a.InstituteID == instId
                       select new
                       {
                           ID = a.ID,
                           AccreditationNumber = a.AccreditationNumber,
                           EffectiveFromDate = a.EffectiveFromDate,
                           EffectiveToDate = a.EffectiveToDate,
                           Name = a.Course.Name,                         
                           SNAME = a.AccreditationStatus.Name,                         
                           Withdrawldate=a.WithdrawlDate                           
                       };

            var instMaster = (from b in context.Institutes                              
                              join c in context.Locations
                              on b.StateID equals c.ID
                              where b.ID == instId                             
                              select new
                              {
                                  IName = b.Name,
                                  Address = b.AddressLine1 + " " + b.AddressLine2 + " " + b.AddressLine3 + " " + b.CityName + " (" + c.Name + ")",                                 
                              }).FirstOrDefault();

            txtName.Text = instMaster.IName;
            txtAddress.Text = instMaster.Address;

            lblName.Visible = true;
            lblAddress.Visible = true;
            txtName.Visible = true;
            txtAddress.Visible = true;           
          
            PagingBar1.Bind(inst, ref gvMain);
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
    //protected void PageIndexChanged(Int32 NewPageIndex)
    //{
    //    try
    //    {
    //        gvMain.PageIndex = PagingBar1.CurrentPageIndex;
    //        BindGridView();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}   
   
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
        //try

        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
               // string href = hl.NavigateUrl;

                if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
                {                
                   // href += "&key1=" + Request.QueryString["key1"].ToString();                 
                }
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&key1=" + Request.QueryString["key1"].ToString());
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                //HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //h2.NavigateUrl = hl.NavigateUrl;
                //HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //h3.NavigateUrl = hl.NavigateUrl;
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = hl.NavigateUrl;
                //HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                //h5.NavigateUrl = hl.NavigateUrl;
             //   e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, string contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            Int32 instID = Convert.ToInt32(contextKey.ToString());
            string searchString = prefixText.Trim().ToUpper();
            var centre = from s in context.AccreditationDetails
                         where s.InstituteID == instID
                         orderby s.AccreditationNumber
                         select new { AccreditationNumber = s.AccreditationNumber };
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.AccreditationNumber.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.AccreditationNumber);
            foreach (var course in centre)
            {
                items.Add(course.AccreditationNumber);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 stateid = Convert.ToInt32(DdlAccState.SelectedValue);           
            BindAccCentre(stateid);
        }
        catch (Exception ex)
        {           
        }
    }
    protected void BindAccCentre(int stateid)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");            
            using (var context = new EConnectContext())
            {
                var AccCentre = (from i in context.Institutes
                                 //join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                 orderby i.Name
                                 where i.StateID == stateid
                                 select new { ValueField = i.ID, TextField = i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindState()
    {
        Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
        Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var AccState = (from s in context.Locations
                                join i in context.Institutes on s.ID equals i.StateID
                                join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct().ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, AccState, lst);

                
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            DdlAccState.SelectedValue = "0";
            DdlAccCentre.SelectedValue = "0";
            //if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
            //{
            //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("insttAccrStatusByInstWise.aspx?key1=" + Request.QueryString["key1"].ToString()), true);
            //}
            //else
            //{
            //    Response.Redirect("insttAccrStatusByInstWise.aspx", true);
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
    public AccreditationDetail objAccreDetail 
    { 
        get; set; 
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (DdlAccState.SelectedValue == "0")
        {
            ShowAlert("Select State required.", true);
            return;
        }
        if (DdlAccCentre.SelectedValue == "0")
        {
            ShowAlert("Select Centre Name of Accredited Institute required.", true);
            return;
        }
        EConnectContext context=new EConnectContext ();        
        string vAccrNo=txtAccrNo.Text ;
        var inst = (from a in context.AccreditationDetails
                   where a.AccreditationNumber == vAccrNo
                   && (a.CourseCategoryID == 2
                   || a.CourseCategoryID == 9)
                   select a).Count ();               
        BindGridView();
        gvMain.Visible = true;
        PagingBar1.Visible = true;
    }
}