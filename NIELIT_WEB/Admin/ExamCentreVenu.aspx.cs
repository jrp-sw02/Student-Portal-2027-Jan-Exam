using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;

public partial class Admin_ExamCentreVenu : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Admin/ExamCentres.aspx"))
            {
                Response.Write("Sorry! You don't have rights to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    //FillState();
                    //FillExamCentres();
                    ShowEditMode();
                }
                else
                {
                    //FillState();
                    //FillExamCentres();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    //FillFilterState();
                    BindGridView();
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No Record Found";
                        lblError.Visible = true;
                        //btnMode.Visible = false;
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

    protected void FillFilterState()
    {
        using (EConnectContext context = new EConnectContext())
        {
            ListItem lst = new ListItem("--All--", "0");
            var state = (from s in context.ExamVenues
                         select new { ValueField = s.State.ID, TextField = s.State.Name }).Distinct();
            // EConnect.Utils.Common.ControlUtility.BindListObject(ddlflstates, state, lst);
        };
    }
    protected void FillFilterCity(Int32 StateID)
    {
        using (EConnectContext context = new EConnectContext())
        {
            ListItem lst = new ListItem("--All--", "0");
            var City = (from s in context.ExamVenues
                        where s.StateID == StateID
                        select new { ValueField = s.City, TextField = s.City }).Distinct();
            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlflCity, City, lst);
        };
    }
   
    protected void FillCourses(DropDownList ddl, Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
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
            //FillState();
            //FillExamCentres();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Exam Centre Venue";
            //tblNavLinks.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ExamVenuID = Convert.ToInt32(Request.QueryString["Key"]);
                //var ExamVenue = (from s in context.ExamVenues where s.ID == ExamVenuID select s).First();
                //var datates = (from d in context.Exams where d.ID==6279 select d).FirstOrDefault();
                var ExamVenue1 = from s in context.ExamVenues
                                 where s.ID == ExamVenuID
                                 select new
                                {
                                    ID = s.ID,
                                    Name = s.Name,
                                    StateID = s.StateID,
                                    StateName = s.State.Name,
                                    //City = s.City.ToUpper() + ", " + s.State.Name.ToUpper(),
                                    City = s.City.ToUpper(),
                                    //IsActive = s.IsActive ? "Active" : "InActive",
                                    IsActive = s.IsActive,
                                    ExamCentreID = s.ExamCentreID,
                                    ExamCentre = s.ExamCenter.Name,
                                    Code = s.Code,
                                    AddressLine1=s.AddressLine1,
                                    AddressLine2=s.AddressLine2,
                                    StdCode=s.StdCode,
                                    PhoneNumber=s.PhoneNumber,
                                    FaxNumber=s.FaxNumber,
                                    MobileNumber = s.MobileNumber,
                                    EmailAddress=s.EmailAddress,
                                    PinCode=s.PinCode
                                }; 
                var ExamVenue = ExamVenue1.Where(s=>s.ID==ExamVenuID).FirstOrDefault();
                txtExamCentre.Text = ExamVenue.Name;
                txtExamCentre.Enabled = false;
                txtState.Text = ExamVenue.StateName;
                txtState.Enabled = false;
                hfExamCentreID.Value = ExamVenue.ExamCentreID.ToString();
                hfStateID.Value = ExamVenue.StateID.ToString();
                txtCity.Text = ExamVenue.City.ToString().ToUpper();
                Txtvenuecode.Text = ExamVenue.Code.ToString().ToUpper();
                Txtvenuecode.Enabled = false;
                txtVenueName.Text = ExamVenue.Name.ToString().ToUpper();
                txtAddress1.Text = ExamVenue.AddressLine1.ToString().ToUpper();
                if (ExamVenue.AddressLine2 != null && ExamVenue.AddressLine2 != "")
                    txtAddress2.Text = Convert.ToString(ExamVenue.AddressLine2.ToUpper());
                if (ExamVenue.StdCode != null && ExamVenue.StdCode !=0)
                    txtStdCode.Text ="0" + ExamVenue.StdCode.ToString().ToUpper();
                txtPhoneNumber.Text = ExamVenue.PhoneNumber.ToString().ToUpper();
                txtFaxNumber.Text = ExamVenue.FaxNumber.ToString().ToUpper();
                txtMobileNumber.Text = ExamVenue.MobileNumber.ToString().ToUpper();
                if (ExamVenue.EmailAddress != null && ExamVenue.EmailAddress != "")
                txtEmail.Text = ExamVenue.EmailAddress.ToString();
                if (ExamVenue.IsActive == true)
                {
                    ddlActiveStatus.SelectedValue = "1";
                }
                else if (ExamVenue.IsActive == false)
                {
                    ddlActiveStatus.SelectedValue = "2";
                }
                txtPinCode.Text = ExamVenue.PinCode.ToString();
                hlExamMenu.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentreVenu.aspx?ExamCentreId=" + Request.QueryString["Key"]);
                //BreadCrumb1.Render();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Venue", "#", ""));
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("ExamVenue", "#", ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
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
            lblError.Visible = false;
            Int32 ExamCentreID= Convert.ToInt32(Request.QueryString["ExamCentreId"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["CourseId"]);
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                ucSearchBar.AutoCompleteContextKey = ExamCentreID.ToString();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                string Activestatus="";
                if (ddlflActiveStatus.SelectedValue != "0")
                    Activestatus =Convert.ToString(ddlflActiveStatus.SelectedItem.Text) ;
                var ExamVenue = from s in context.ExamVenues
                                where s.ExamCentreID == ExamCentreID && s.CourseID == courseid
                                 select new
                                 {
                                     ID = s.ID,
                                     Name = s.Name,
                                     StateID = s.StateID,
                                     StateName=s.State.Name,
                                     City=s.City.ToUpper() + ", " + s.State.Name.ToUpper(),
                                     IsActive = s.IsActive ? "Active" : "InActive",
                                     ExamCentreID=s.ExamCentreID,
                                     ExamCentre=s.ExamCenter.Name,
                                     Code = s.Code
                                 };
                if (!String.IsNullOrEmpty(searchString))
                {
                    ExamVenue = ExamVenue.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                if (Activestatus != "")
                {
                    ExamVenue = ExamVenue.Where(s => s.IsActive == Activestatus);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                ExamVenue = ExamVenue.OrderByDescending(s => s.ID);
                            else
                                ExamVenue = ExamVenue.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                ExamVenue = ExamVenue.OrderByDescending(s => s.Name);
                            else
                                ExamVenue = ExamVenue.OrderBy(s => s.Name);
                            break;
                        case "City":
                            if (sortOrder == "DESC")
                                ExamVenue = ExamVenue.OrderByDescending(s => s.City);
                            else
                                ExamVenue = ExamVenue.OrderBy(s => s.City);
                            break;
                        case "IsActive":
                            if (sortOrder == "DESC")
                                ExamVenue = ExamVenue.OrderByDescending(s => s.IsActive);
                            else
                                ExamVenue = ExamVenue.OrderBy(s => s.IsActive);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                ExamVenue = ExamVenue.OrderByDescending(s => s.Code);
                            else
                                ExamVenue = ExamVenue.OrderBy(s => s.Code);
                            break;
                        default:
                            ExamVenue = ExamVenue.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(ExamVenue, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Centre Venue", "Admin/ExamCentreVenu.aspx?ExamCentreId=" + Request.QueryString["ExamCentreId"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Centre Venue", "Admin/ExamCentreVenu.aspx?" + Request.QueryString.ToString(), ""));
                }
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No Record Found";
                    lblError.Visible = true;
                }
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
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ExamCentreID = Convert.ToInt32(Request.QueryString["ExamCentreId"]);
                var ExamCentre = (from s in context.ExamCenters
                                 where s.ID == ExamCentreID
                                 select s).FirstOrDefault();
                txtExamCentre.Text = ExamCentre.Name.ToUpper();
                txtExamCentre.Enabled = false;
                txtState.Text = ExamCentre.State.Name;
                txtState.Enabled = false;
                hfExamCentreID.Value = ExamCentre.ID.ToString();
                hfStateID.Value = ExamCentre.StateID.ToString();
                
            };
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Exam Centres Venue";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre Venue", "#", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ExamCentreId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentreVenu.aspx?ExamCentreId=" + Request.QueryString["ExamCentreId"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString()), true);
            }
            else
            {
                Response.Redirect("ExamCentreVenu.aspx", true);
            }
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
           Int32 courseid = Convert.ToInt32(Request.QueryString["CourseId"]);
           if (courseid == 0)
           {
               strMessage = "Wrong Course Id !! Please Refresh This Page";
               Response.Redirect("ExamCentreVenu.aspx?msg=" + strMessage + "&ExamCentreId=" + Request.QueryString["ExamCentreId"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString());
           }
            using (EConnectContext context = new EConnectContext())
            {
               // ExamVenue objExCentreVenue;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    // commneted by abhi singh Dated on 26062023
                    //objExCentreVenue = new EConnect.NIELIT.ExamVenue();
                    //objExCentreVenue.ExamCentreID = Convert.ToInt32(hfExamCentreID.Value);
                    //objExCentreVenue.CourseID = courseid;
                    //objExCentreVenue.Code = Txtvenuecode.Text.ToString().ToUpper();
                    //objExCentreVenue.StateID = Convert.ToInt32(hfStateID.Value);
                    //objExCentreVenue.City = txtCity.Text.ToString().ToUpper();
                    //objExCentreVenue.Name = txtVenueName.Text.ToString().ToUpper();
                    //objExCentreVenue.AddressLine1 = txtAddress1.Text.ToString().ToUpper();
                    //objExCentreVenue.AddressLine2 = txtAddress2.Text.ToString().ToUpper();

                    Int32 StdCode = Convert.ToInt32(null);
                     if (!string.IsNullOrEmpty(txtStdCode.Text))
                      StdCode = Convert.ToInt32(txtStdCode.Text);

                     Int32 PhoneNumber = Convert.ToInt32(null); 
                     if (!string.IsNullOrEmpty(txtPhoneNumber.Text))
                     PhoneNumber = Convert.ToInt32(txtPhoneNumber.Text);

                     Int32 FaxNumber = Convert.ToInt32(null); 
                     if (!string.IsNullOrEmpty(txtFaxNumber.Text))
                      FaxNumber = Convert.ToInt32(txtFaxNumber.Text);

                     Int64 MobileNumber = Convert.ToInt32(null); 
                     if (!string.IsNullOrEmpty(txtMobileNumber.Text))
                      MobileNumber = Convert.ToInt64(txtMobileNumber.Text);

                    string EmailAddress="";
                    if (!string.IsNullOrEmpty(txtEmail.Text))
                     EmailAddress = txtEmail.Text.ToString();

                    // commneted by abhi singh Dated on 26062023
                    //objExCentreVenue.CreatedByID = Convert.ToInt32(Session["UserID"]);
                    //objExCentreVenue.CreatedOn = Convert.ToDateTime(DateTime.Now.ToString());

                    bool IsActive=false;
                     if (ddlActiveStatus.SelectedValue == "1")
                     {
                       IsActive = true;
                     }
                     else if (ddlActiveStatus.SelectedValue == "2")
                     {
                        IsActive = false;
                     }
                     Int32 PinCode = Convert.ToInt32(null);
                     if (!string.IsNullOrEmpty(txtPinCode.Text))
                      PinCode = Convert.ToInt32(txtPinCode.Text);
                    //--
                    //context.ExamVenues.Add(objExCentreVenue);
                    //context.SaveChanges();

                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);
                    string sqlQuery = "INSERT INTO exam_venue (Name,Exam_Centre_ID,Address1,Address2,City,Pin_Code,State_ID,Std_Code,Phone_Nunber,Fax_Number,Mobile,Email,Is_Active,Created_By,Created_On,Course_id,Code) VALUES ('" + txtVenueName.Text.ToString().ToUpper() + "'," + hfExamCentreID.Value + ",'" + txtAddress1.Text.ToString().ToUpper() + "','" + txtAddress2.Text.ToString().ToUpper() + "','" + txtCity.Text.ToString().ToUpper() + "'," + PinCode + "," + Convert.ToInt32(hfStateID.Value) + "," + StdCode + "," + PhoneNumber + "," + FaxNumber + "," + MobileNumber + ",'" + EmailAddress + "','" + IsActive + "'," + Convert.ToInt32(Session["UserID"]) + ",'" + Convert.ToDateTime(DateTime.Now.ToString()) + "'," + courseid + ",'" + Txtvenuecode.Text.ToString().ToUpper() + "')";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    strMessage = "New record saved.";
                }
                else
                {
                    //commneted by abhi singh Dated on 26062023
                    //objExCentreVenue = context.ExamVenues.Find(Convert.ToInt32(Request.QueryString["key"]));
                    ////Int32 data=Convert.ToInt32(Request.QueryString["Key"]);
                    ////objExCentreVenue = ExamVenue1.Where(s=>s.ID==data).FirstOrDefault();
                    //objExCentreVenue.ExamCentreID = Convert.ToInt32(hfExamCentreID.Value);
                    //objExCentreVenue.CourseID = courseid;
                    //objExCentreVenue.Code = Txtvenuecode.Text.ToString().ToUpper();
                    //objExCentreVenue.StateID = Convert.ToInt32(hfStateID.Value);
                    //objExCentreVenue.City = txtCity.Text.ToString().ToUpper();
                    //objExCentreVenue.Name = txtVenueName.Text.ToString().ToUpper();
                    //objExCentreVenue.AddressLine1 = txtAddress1.Text.ToString().ToUpper();
                    //objExCentreVenue.AddressLine2 = txtAddress2.Text.ToString().ToUpper();
                    Int32 StdCode = Convert.ToInt32(null);
                    if (!string.IsNullOrEmpty(txtStdCode.Text))
                        StdCode = Convert.ToInt32(txtStdCode.Text);

                    Int32 PhoneNumber = Convert.ToInt32(null);
                    if (!string.IsNullOrEmpty(txtPhoneNumber.Text))
                        PhoneNumber = Convert.ToInt32(txtPhoneNumber.Text);

                    Int32 FaxNumber = Convert.ToInt32(null);
                    if (!string.IsNullOrEmpty(txtFaxNumber.Text))
                        FaxNumber = Convert.ToInt32(txtFaxNumber.Text);

                    Int64 MobileNumber = Convert.ToInt32(null);
                    if (!string.IsNullOrEmpty(txtMobileNumber.Text))
                        MobileNumber = Convert.ToInt64(txtMobileNumber.Text);

                    string EmailAddress = "";
                    if (!string.IsNullOrEmpty(txtEmail.Text))
                        EmailAddress = txtEmail.Text.ToString();

                    //objExCentreVenue.CreatedByID = Convert.ToInt32(Session["UserID"]);
                    //objExCentreVenue.CreatedOn = Convert.ToDateTime(DateTime.Now.ToString());
                    bool IsActive = false;
                    if (ddlActiveStatus.SelectedValue == "1")
                    {
                        IsActive = true;
                    }
                    else if (ddlActiveStatus.SelectedValue == "2")
                    {
                        IsActive = false;
                    }
                    Int32 PinCode = Convert.ToInt32(null);
                    if (!string.IsNullOrEmpty(txtPinCode.Text))
                        PinCode = Convert.ToInt32(txtPinCode.Text);

                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);
                    string sqlQuery = "update exam_venue set Name='" + txtVenueName.Text.ToString().ToUpper() + "',Exam_Centre_ID=" + hfExamCentreID.Value + ",Address1='" + txtAddress1.Text.ToString().ToUpper() + "',Address2='" + txtAddress2.Text.ToString().ToUpper() + "',City='" + txtCity.Text.ToString().ToUpper() + "',Pin_Code=" + PinCode + ",State_ID=" + Convert.ToInt32(hfStateID.Value) + ",Std_Code=" + StdCode + ",Phone_Nunber=" + PhoneNumber + ",Fax_Number=" + FaxNumber + ",Mobile=" + MobileNumber + ",Email='" + EmailAddress + "',Is_Active='" + IsActive + "',Created_By=" + Convert.ToInt32(Session["UserID"]) + ",Created_On='" + Convert.ToDateTime(DateTime.Now.ToString()) + "',Course_id=" + courseid + ",Code='" + Txtvenuecode.Text.ToString().ToUpper() + "' where Id= " + Convert.ToInt32(Request.QueryString["key"]) + "";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    strMessage = "Record updated.";
                    //context.SaveChanges();
                }
                Response.Redirect("ExamCentreVenu.aspx?msg=" + strMessage + "&ExamCentreId=" + Request.QueryString["ExamCentreId"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString(), true);
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
            //ddlflstates.SelectedValue = "0";
            //ddlficoursecategory.SelectedValue = "0";
            //ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
            //ddlflexcentretype.SelectedValue = "0";
            ddlflActiveStatus.SelectedValue = "0";
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
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if ((!String.IsNullOrEmpty(Request.QueryString["ExamCentreId"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseId"])))
                {
                    href += "&ExamCentreId=" + Request.QueryString["ExamCentreId"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
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
            Int32 ExamCentreID = Convert.ToInt32(contextKey);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var ExamCentreVenu = from s in context.ExamVenues
                                 where s.ExamCentreID == ExamCentreID
                                 select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                ExamCentreVenu = ExamCentreVenu.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            ExamCentreVenu = ExamCentreVenu.OrderBy(s => s.Name);
            foreach (var c in ExamCentreVenu)
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
        if (!String.IsNullOrEmpty(Request.QueryString["ExamCentreId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentreVenu.aspx?ExamCentreId=" + Request.QueryString["ExamCentreId"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("ExamCentreVenu.aspx", true);
        }

    }
    protected void ddlflstates_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlflCity.Items.Clear();
        //Int32 StateID = Convert.ToInt32(ddlflstates.SelectedValue);
        // FillFilterCity(StateID);
    }
}