using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public partial class Admin_NSQFFreeAccrRequest : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int32 NielitAccreditedInstituteId = 0;
    Int32 UserTypeId = 0;
    Int32 previousAppliedCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);//4
            loginUserNo = Convert.ToInt32(Session["UserID"]);//19291
            entityID = Convert.ToInt64(Session["EntityID"]);//589
            UserTypeId = Convert.ToInt32(Session["UserType"]);//4

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
                    if (UserTypeId == 4)
                    {
                        NielitAccreditedInstituteId = Convert.ToInt32(loginUser.UserRefNumber);
                        Institute institutesName = context.Institutes.Where(s => s.ID == NielitAccreditedInstituteId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name + " ( " + institutesName.AddressLine1 + ", " + institutesName.CityName + " )";
                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    { }
                    else
                    {
                        BindAllAccrDetails();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();
                        FillCoursesName();

                        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NSQF Free Accreditation Request", "Admin/NSQFFreeAccrRequest.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NSQF Free Accreditation Request", "Admin/NSQFFreeAccrRequest.aspx", ""));
                        }
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

    protected void BindAllAccrDetails()
    {
        try
        {
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("-- Select One --", "0");
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitAccreditedInstitutesId = Convert.ToInt32(loginUser.UserRefNumber);
                if (NielitAccreditedInstitutesId != 0)
                {
                    var AllAccrDetails = from i in context.Institutes
                                         join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                         where new[] { 1, 2, 3, 4, 9 }.Contains(a.AccreditationStatusID) && i.ID == NielitAccreditedInstitutesId
                                         && new[] { 1, 2, 3, 4 }.Contains(a.CourseID)
					 && a.EffectiveToDate.Value  >= System.DateTime .Today 
                                         orderby a.EffectiveFromDate descending 
                                         select new { ValueField = a.CourseID, TextField = a.AccreditationNumber };
			AllAccrDetails = AllAccrDetails.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAllAccrDetails, AllAccrDetails, lst);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlAllAccrDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbNielitFreeCourses.Items.Clear();
            Int32 courseid = Convert.ToInt32(ddlAllAccrDetails.SelectedValue);
            lblError.Visible = false;
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            DataTable DT = new DataTable();

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString; //NielitAccreditedInstituteId
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            //using (SqlCommand Cmm = new SqlCommand("SELECT [bucketCourseID] ID ,c.Name  FROM [NIELIT].[dbo].[NSQFfreeCourseMapping] m  , [NIELIT].[dbo].[Course] c where c.ID=m.bucketCourseID and m.courseID <=" + courseid, con))
            using (SqlCommand Cmm = new SqlCommand("GetNSQFfreeCourseMappingData", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                Cmm.Parameters.Add("@courseId", SqlDbType.Int);
                Cmm.Parameters["@courseId"].Value = courseid;
                //Cmm.Parameters.Add("@InstitueId", SqlDbType.BigInt);
                //Cmm.Parameters["@InstitueId"].Value = NielitAccreditedInstituteId;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);
                Sda.Fill(DT);
            }
            con.Close();
            cbNielitFreeCourses.DataSource = DT;
            cbNielitFreeCourses.DataTextField = "Name";
            cbNielitFreeCourses.DataValueField = "ID";
            cbNielitFreeCourses.DataBind();
            uPnlGrid.Update();
            uPnlNavigation.Update();
            cbNielitFreeCourses.Visible = true;
            allChkBox.Visible = false;
            EConnectContext context = new EConnectContext();
            User objUser;
            objUser = new EConnect.URM.User();
            User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            NielitAccreditedInstituteId = Convert.ToInt32(loginUser.UserRefNumber);
            Institute institutesName = context.Institutes.Where(s => s.ID == NielitAccreditedInstituteId).FirstOrDefault();
            if (institutesName != null)
            {
                txtInstitute.Text = institutesName.Name + " ( " + institutesName.AddressLine1 + ", " + institutesName.CityName + " )";
            }
            //NielitAccreditedInstituteId = 4587;
            var AlreadyAppliedCourses = (from c in context.NSQFFreeCourseGrants
                                         where c.InstituteID == NielitAccreditedInstituteId
                                         select new
                                         {
                                             ID = c.ID,
                                             CourseID = c.mappedCourseID,
                                             CentreId = c.InstituteID,

                                         }).ToList();

            if (AlreadyAppliedCourses.Count > 0)
            {
                foreach (var Ci in AlreadyAppliedCourses)
                {
                    foreach (ListItem li in cbNielitFreeCourses.Items)
                    {
                        if (li.Value == Ci.CourseID.ToString())
                        {
                            li.Selected = true;
                            li.Enabled = false;
                            li.Attributes.Add("style", "background-color: PaleTurquoise");
                            //cbNielitFreeCourses.SelectedItem.Attributes["style"] = "BackGround-color: LightGreen";
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridNielitFreeCourse()
    {
        try
        {
            cbNielitFreeCourses.Items.Clear();
            Int32 courseid = Convert.ToInt32(ddlAllAccrDetails.SelectedValue);
            lblError.Visible = false;
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            DataTable DT = new DataTable();

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("SELECT [bucketCourseID] ID ,c.Name  FROM [NIELIT].[dbo].[NSQFfreeCourseMapping] m  , [NIELIT].[dbo].[Course] c where c.ID=m.bucketCourseID and m.courseID <=" + courseid, con))
            {
                Cmm.CommandType = CommandType.Text;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
            con.Close();

            cbNielitFreeCourses.DataSource = DT;
            cbNielitFreeCourses.DataTextField = "Name";
            cbNielitFreeCourses.DataValueField = "ID";
            cbNielitFreeCourses.DataBind();
            uPnlGrid.Update();
            uPnlNavigation.Update();
            cbNielitFreeCourses.Visible = true;
            allChkBox.Visible = false;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
        }
    }
    protected void allChkBox_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem chkitem in cbNielitFreeCourses.Items)
        {
            if (allChkBox.Checked == true)
            {
                chkitem.Selected = true;
                cbNielitFreeCourses.BackColor = System.Drawing.Color.LightGreen;

            }
            else
            {
                chkitem.Selected = false;
                cbNielitFreeCourses.BackColor = System.Drawing.Color.White;
            }
        }
    }
    protected bool IsValidForm()
    {
        try
        {
            if (!chkUnderTakingByInstitute.Checked)
            {
                ShowAlert("Please select undertaking");
                return false;
            }
            //if(txtName .Text =="")
            //{
            //    ShowAlert("Please enter Undertaking Signatory Name");
            //    return false;
            //}
            //if (txtFatherName.Text == "")
            //{
            //    ShowAlert("Please enter Undertaking Signatory Father Name");
            //    return false;
            //}
            //if (txtDesignation.Text == "")
            //{
            //    ShowAlert("Please enter Undertaking Signatory Designation");
            //    return false;
            //}
            //if (txtAddress.Text == "")
            //{
            //    ShowAlert("Please enter Undertaking Signatory Adrress");
            //    return false;
            //}

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlandTextBox(TextBox txtOrgTrained)
    {
        try
        {

            if (txtOrgTrained.Text == "")
            {
                return false;
            }
            else
                return true;
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
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 CourseName = 0;
                if (ddlCourseName.SelectedValue != "0")
                    CourseName = Convert.ToInt64(ddlCourseName.SelectedValue);
                User objUser;
                objUser = new EConnect.URM.User();

                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                NielitAccreditedInstituteId = Convert.ToInt32(loginUser.UserRefNumber);
              /*  var Courses = from s in context.NSQFFreeCourseGrants
                              join i in context.AccreditationDetails  on s.InstituteID equals i.InstituteID 
                              join c in context.Courses on s.mappedCourseID equals c.ID
                              join l in context.CourseLevelDurationss on s.mappedCourseID equals l.CourseID into gj
                              from subquery in gj.DefaultIfEmpty ()
                              where s.InstituteID == NielitAccreditedInstituteId
                              && s.accrCourseID==i.CourseID 
                              select new
                              {
                                  ID = s.ID,
                                  instituteID = s.InstituteID,
                                  CourseId = s.mappedCourseID ,
                                  CourseName = c.Name + " (" + c.Code +" )",
                                  IsActive = s.isActive ? "YES" : "NO",
                                  GrantDate = s.grantDate,
                                  Remarks = s.Remarks,
                                  Effs=subquery.Effective_To_Date  ,
                                  accrNo=i.AccreditationNumber ,
                                  accrUpto=i.EffectiveToDate ,

                              };*/
				var Courses = from s in context.NSQFFreeCourseGrants
                              join i in context.AccreditationDetails on new { Id = s.InstituteID, cid = s.mappedCourseID.ToString() }  equals new { Id = i.InstituteID, cid = i.CourseID.ToString () } into xy
                              from subquery1 in xy.DefaultIfEmpty ()
                              join c in context.Courses on s.mappedCourseID equals c.ID
                              join l in context.CourseLevelDurationss on s.mappedCourseID equals l.CourseID into gj
                              from subquery in gj.DefaultIfEmpty()
                              where s.InstituteID == NielitAccreditedInstituteId
				// && subquery1.EffectiveToDate == context.AccreditationDetails.Where(w=>w.InstituteID==subquery1.InstituteID).Max(m=>m.EffectiveToDate) 
				  && subquery.Effective_To_Date  == context.CourseLevelDurationss .Where(w=> w.CourseID   == subquery.CourseID).Max(m => m.Effective_To_Date) 
                              //  && subquery.CourseID ==i.CourseID 
                              select new
                              {
                                  ID = s.ID,
                                  instituteID = s.InstituteID,
                                  CourseId = s.mappedCourseID,
                                  CourseName = c.Name + " (" + c.Code + " )",
                                  IsActive = s.isActive ? "YES" : "NO",
                                  GrantDate = s.grantDate,
                                  Remarks = s.Remarks,
                                  Effs = subquery.Effective_To_Date,
                                  accrNo = subquery1.AccreditationNumber,
                                  accrUpto = subquery1.EffectiveToDate,

                              };
		Courses = Courses.Distinct();
                if (!String.IsNullOrEmpty(searchString))
                {
                    Courses = Courses.Where(s => s.CourseName.ToUpper().Contains(searchString));
                }

                if (CourseName != 0)
                {
                    Courses = Courses.Where(s => s.CourseId == CourseName);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                Courses = Courses.OrderByDescending(s => s.ID);
                            else
                                Courses = Courses.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                Courses = Courses.OrderByDescending(s => s.CourseName);
                            else
                                Courses = Courses.OrderBy(s => s.CourseName);
                            break;

                        default:
                            Courses = Courses.OrderBy(s => s.CourseName);
                            break;
                    }
                }

                PagingBar1.Bind(Courses, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                divNavigation.Visible = true;
               // gvMain.Columns[5].Visible = false;
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[7].Visible = false;
                }
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    divNavigation.Visible = false;
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
            lblHeading.Text = "NSQF Free Accreditation Request";
            BreadCrumb1.Render();
            //Updating Breadcrumb         
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New NSQF Free Accreditation Request", "Admin/NSQFFreeAccrRequest.aspx", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NSQFFreeAccrRequest.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NSQFFreeAccrRequest.aspx", true);
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
    protected void FillCoursesName()
    {
        try
        {
            ddlCourseName.Items.Clear();
            ListItem lst1 = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                ddlCourseName.ClearSelection();

                var courses = from t in context.NSQFFreeCourseGrants
                              join c in context.Courses on t.mappedCourseID equals c.ID
                              orderby (c.Name)
                              select new { ValueField = c.ID, TextField = c.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst1);
            }

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
            Int64 NielitCentresID = 0, i = 0, AtLeastChecked = 0;
            Int64 bucketCourseID = 0, AlbucketCourseID = 0;
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                EConnectContext context1 = new EConnectContext();
                User objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int64 NielitCentreId = Convert.ToInt64(loginUser.UserRefNumber);
                Int64 courseid = Convert.ToInt64(ddlAllAccrDetails.SelectedValue); // course id 1,2,3,4 based on Accr Number
                foreach (ListItem lii in cbNielitFreeCourses.Items)
                {
                    if (lii.Selected )
                    {
                        
                        AtLeastChecked = AtLeastChecked + 1;
                        var alreadyAccr = from c in context1.AccreditationDetails
                                          where c.InstituteID.ToString() == NielitCentreId.ToString()
                                          && c.CourseID.ToString() == lii.Value.ToString()
                                          && (c.AccreditationStatusID.ToString ()  != enmAccreditationStatus.Deferred.ToString ()
                                          && c.AccreditationStatusID.ToString() != enmAccreditationStatus.Acknowledged.ToString()
                                              && c.AccreditationStatusID.ToString() != enmAccreditationStatus.Withdrawal.ToString()
                                              && c.AccreditationStatusID.ToString() != enmAccreditationStatus.Rejected.ToString())
                                          select c;
                        var vNSQFGrant = from c in context1.NSQFFreeCourseGrants
                                         where c.InstituteID.ToString() == NielitCentreId.ToString()
                                            && c.mappedCourseID.ToString() == lii.Value.ToString()
                                         select c;

                        if (alreadyAccr.Count() > 0 && vNSQFGrant .Count() ==0)
                        {
                            ShowAlert("Accreditaion already exist for " + lii.Text+" Please deselect it");
                            return;
                        }

                        NielitCentresID = Convert.ToInt32(lii.Value);
                       
                        i = i + 1;
                        if (i > 5)
                        {
                            ShowAlert("Maximum 5 Courses Allowed !!");
                            return;
                        }
                    }
                }
                if (AtLeastChecked == 0)
                {
                    ShowAlert("You must check at least one checkbox. !!");
                    return;
                }
                if (chkUnderTakingByInstitute.Checked == false)
                {
                    ShowAlert("Please Accept the Undertaking. !!");
                    return;
                }
                using (EConnectContext context = new EConnectContext())
                {
                    NSQFFreeCourseGrant objNSQFFreeCourseGrant;
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        objNSQFFreeCourseGrant = new NSQFFreeCourseGrant();

                        foreach (ListItem li in cbNielitFreeCourses.Items)
                        {
                            if (li.Selected && li.Enabled == true)
                            {
                                AlbucketCourseID = 1;
                                bucketCourseID = Convert.ToInt64(li.Value);
                                var courseCheck = (from p in context.NSQFFreeCourseGrants
                                                   where p.mappedCourseID == bucketCourseID && p.InstituteID == NielitCentreId

                                                   select p).ToList();
                                if (courseCheck.Count != 0)
                                {
                                    throw new Exception("Course already requested or granted.");
                                    //return;
                                }
                                else
                                {
                                    objNSQFFreeCourseGrant.InstituteID = NielitCentreId;
                                    objNSQFFreeCourseGrant.accrCourseID = courseid;
                                    objNSQFFreeCourseGrant.mappedCourseID = bucketCourseID;
                                    //objNSQFFreeCourseGrant.undertakingSignatory = txtName.Text;
                                    //objNSQFFreeCourseGrant.undertakingSigDesig = txtFatherName.Text;
                                    //objNSQFFreeCourseGrant.undertakingSigAddress = txtAddress.Text;
                                    //objNSQFFreeCourseGrant.isActive = false;
                                    objNSQFFreeCourseGrant.Remarks = txtRemarks.Text;
                                    objNSQFFreeCourseGrant.enterDate = DateTime.Now;
                                    objNSQFFreeCourseGrant.enterBy = Convert.ToInt32(Session["UserID"]);



                                    context.NSQFFreeCourseGrants.Add(objNSQFFreeCourseGrant);
                                    context.SaveChanges();

                                    strMessage = "Successfully Applied !!";
                                }
                            }
                        }
                        if (AlbucketCourseID == 0)
                        {
                            ShowAlert("You must check at least one checkbox. !!");
                            return;
                        }
                    }
                    
                };
                Response.Redirect("NSQFFreeAccrRequest.aspx?msg=" + strMessage, true);
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
            ddlCourseName.SelectedValue = "0";
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                NielitCentreBatch Batchcenter = context.NielitCentreBatchs.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.NielitCentreBatchs.Remove(Batchcenter);
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
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }


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
        try
        {
            EConnectContext context = new EConnectContext();
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();

            string searchString = prefixText.Trim().ToUpper();
            var courses = from c in context.Courses
                          join w in context.NSQFFreeCourseGrants on c.ID equals w.mappedCourseID
                          select new { Name = c.Name };

            courses = courses.Distinct();

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);
            foreach (var course in courses)
            {
                items.Add(course.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NSQFFreeAccrRequest.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("NSQFFreeAccrRequest.aspx", true);
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
                if (coursenameid != 0)
                {
                }

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
                ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                if (examcenter.IsEnabled == true)
                    examcenter.IsEnabled = false;
                else
                    examcenter.IsEnabled = true;
                context.Entry(examcenter).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                BindGridView();
                ShowAlert("You have successfully changed the status of the Exam Center.", true);
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
}