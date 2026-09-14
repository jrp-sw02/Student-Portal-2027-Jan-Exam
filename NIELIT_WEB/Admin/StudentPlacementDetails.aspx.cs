using EConnect.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;
using EConnect.NIELIT;

public partial class Admin_StudentPlacementDetails : BasePage
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
		if(Session["StudentID"]==null)
		{
			 ShowAlert("Please use Student form to enter placement details");
	                   Response.End();
	
		}
                if (!String.IsNullOrEmpty(Session["StudentID"].ToString()))
                {
                    txtStuID.Text = Session["StudentID"].ToString();
                }
                else
                {
                    ShowAlert("Please use Student form to enter placement details");
                    return;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    bindCompanyName();
                    //BindCourseCategory(ddlCourseCategory, "--ALL--");
                    if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                    {
                        using (var context = new EConnectContext())
                        {
                            Int32 myCourseId1 = Convert.ToInt32(Request.QueryString["CourseId"]);
                            var courses = context.Courses.Find(myCourseId1);

                            //ddlCourseCategory.SelectedValue = courses.CourseCategoryID.ToString();
                            ddlFilterCompany.Enabled = false;
                        };
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Placement Detail", "Admin/StudentPlacementDetails.aspx?CourseId=" + Request.QueryString["CourseId"], ""));
                    }
                    else
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Placement Detail", "Admin/StudentPlacementDetails.aspx", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
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

    protected void bindCompanyName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                ListItem llst = new ListItem("--ALL--", "0");
                var companyMas = from t in context.companyMasters
                                 orderby (t.company_Name)
                                 select new { ValueField = t.ID, TextField = t.company_Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCompanyName, companyMas, lst);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFilterCompany, companyMas, llst);
                
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

            using (NIELITMISContext context = new NIELITMISContext())
            {
                bindCompanyName();
                
                Int64 Id = Convert.ToInt64(Request.QueryString["Key"]);
                var obj = context.studentPlacementDetails.Find(Id);

                //var studentPlacement = (from t in context.studentPlacementDetails
                //           join f in context.NielitCentreStudent on t.studentID equals f.ID
                //           where t.ID == obj.studentID
                //           select new
                //           {
                //               ID = t.ID,
                //               studentID = f.Number,
                //           });
                var studentPlacement = context.NielitCentreStudent.Find(obj.studentID); 
                txtStuID.Text = studentPlacement.Number.ToString();
                ddlCompanyName.SelectedValue = obj.companyID.ToString();
                txtDesgn.Text = obj.designation.ToString();

                string EDate = obj.EffectiveFromDate.ToString();
                if (EDate == "")
                {
                    txtEffectiveFromDate.Text = obj.EffectiveFromDate.ToString();
                }
                else
                {
                    txtEffectiveFromDate.Text = Convert.ToDateTime(EDate).ToString("dd-MMM-yyyy");
                }

                string TDate = obj.EffectiveToDate.ToString();
                if (TDate == "")
                {
                    txtEffectiveToDate.Text = obj.EffectiveToDate.ToString();
                }
                else
                {
                    txtEffectiveToDate.Text = Convert.ToDateTime(TDate).ToString("dd-MMM-yyyy");
                }

                txtStuID.Enabled = false;
                ddlCompanyName.Enabled = false;
                txtDesgn.Enabled = false;
                txtEffectiveFromDate.Enabled = false;
                txtEffectiveToDate.Enabled = true;

                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;

                btnSave.Text = "Update";
                lblHeading.Text = "Student Placement Details";

                
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

    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            using (NIELITMISContext context = new NIELITMISContext())
            {
                int CourseCategoryId = Convert.ToInt32(ddlFilterCompany.SelectedValue);
                

                //Added
                EConnectContext context1 = new EConnectContext();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                
               NielitCentres  nielitcentre = context.NielitCentres.Find(loginUser.UserRefNumber);
               string  NameCenter = nielitcentre.Name;


                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 Id = Convert.ToInt64(Request.QueryString["Key"]);

                var studentPlacement = (from t in context.studentPlacementDetails
                           join f in context.NielitCentreStudent on t.studentID equals f.ID
                           join q in context.companyMasters on t.companyID equals q.ID
                                        where (NameCenter+"-"+t.studentID.ToString()) == txtStuID.Text

                           select new
                           {
                               ID = t.ID,
                               studentID = f.Number,
                               companyID = t.companyID,
                               companyName = q.company_Name,
                               designation = t.designation,
                               FromDate = t.EffectiveFromDate,
                               ToDate = t.EffectiveToDate,
                           });

                if (CourseCategoryId != 0)
                    studentPlacement = studentPlacement.Where(s => s.companyID == CourseCategoryId);
               

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "studentID":
                            if (sortOrder == "DESC")
                                studentPlacement = studentPlacement.OrderByDescending(s => s.studentID);
                            else
                                studentPlacement = studentPlacement.OrderBy(s => s.studentID);
                            break;
                        case "companyName":
                            if (sortOrder == "DESC")
                                studentPlacement = studentPlacement.OrderByDescending(s => s.companyName);
                            else
                                studentPlacement = studentPlacement.OrderBy(s => s.companyName);
                            break;
                       
                        default:
                            studentPlacement = studentPlacement.OrderBy(s => s.FromDate);
                            break;
                    }
                }

                PagingBar1.Bind(studentPlacement, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
                //DisableFilter();
                //BindCourseCategory(ddlCourseCategoryE, "--Select One--");
                PagingBar1.Visible = false;
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                //BindGridView();
                //Change the heading text as required
                lblHeading.Text = "New Placement Detail";
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                {
                    using (var context = new EConnectContext())
                    {
                        Int32 myCourseId1 = Convert.ToInt32(Request.QueryString["CourseId"]);
                        
                        var courses = (from s in context.Courses
                                       where s.ID == myCourseId1
                                       select s).FirstOrDefault();
                        //ddlCourseCategoryE.SelectedValue = courses.CourseCategoryID.ToString();
                        //ddlCourseCategoryE_SelectedIndexChanged(ddlCourseNameE, EventArgs.Empty);
                        //ddlCourseNameE.SelectedValue = courses.ID.ToString();
                        //ddlCourseNameE.Enabled = false;
                        //ddlCourseCategoryE.Enabled = false;
                        //BindFeeType(ddlFeeTypeE, "-Select One--", true, Convert.ToInt32(ddlCourseNameE.SelectedValue));
                    };
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Placement Detail", "", ""));
                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Placement Detail", "", ""));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                    Response.Redirect("StudentPlacementDetails.aspx?CourseId=" + Convert.ToString(Request.QueryString["CourseId"]));
                else
                    Response.Redirect("StudentPlacementDetails.aspx");
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

    protected string Save()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
                {

                   

                        var stuPlacement = new EConnect.NIELIT.studentPlacementDetail();

                        string[] words = txtStuID.Text.Split('-');

                        stuPlacement.studentID = Convert.ToInt64(words[1]);
                        stuPlacement.companyID = Convert.ToInt64(ddlCompanyName.SelectedValue);
                        stuPlacement.designation = txtDesgn.Text;
                        stuPlacement.EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text);

                        context.studentPlacementDetails.Add(stuPlacement);

                        context.SaveChanges();
                        strMessage = "New record saved.";
                        return strMessage;
                    

                };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                //create and object 

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    DateTime EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text.ToString());
                    Int64 batchID = Convert.ToInt64(Session["Batch_ID"]);

                    if (context.NielitCentreBatchs.Where(s => s.ID == batchID && (s.endDate <= EffectiveFromDate)).Count() > 0)
                    {
                        strMessage = Save();
                    }
                    else
                    {
                        var CenterBatch = (from s in context.NielitCentreBatchs
                                           where s.ID == batchID
                                           select s).FirstOrDefault();

                        LblError.Visible = true;
                        LblError.Text = "Effective From date greater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy");
                        txtEffectiveFromDate.Focus();
                        return;
                    }

                }
                else
                {
                    if (txtEffectiveToDate.Text != "")
                    {
                        DateTime FromDate = Convert.ToDateTime(txtEffectiveFromDate.Text.Trim());
                        DateTime ToDate = Convert.ToDateTime(txtEffectiveToDate.Text.Trim());
                        if (ToDate > FromDate)
                        {
                            Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);
                            var stuPlacement = context.studentPlacementDetails.Find(applID);

                            stuPlacement.EffectiveToDate = Convert.ToDateTime(txtEffectiveToDate.Text);

                            context.studentPlacementDetails.Add(stuPlacement);

                            context.Entry(stuPlacement).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            strMessage = "Record Updated.";
                        }
                        else
                        {
                            LblError.Visible = true;
                            LblError.Text = "To date should be greater than From date.";
                            txtEffectiveToDate.Focus();
                            return;

                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Start date should be greate than End date')", true);
                        }

                    }
                    else
                    {
                        LblError.Visible = true;
                        LblError.Text = "Please Enter Effective To Date.";
                        txtEffectiveToDate.Focus();
                        return;

                    }
                }
            };
            Response.Redirect("StudentPlacementDetails.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected void ApplyFilter(object sender, EventArgs e)
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
            {
                ddlFilterCompany.SelectedValue = "0";
            }
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
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    hl.NavigateUrl += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("StudentPlacementDetails.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("StudentPlacementDetails.aspx", true);
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
                EConnect.NIELIT.FeeDetail feedetail = context.FeeDetails.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.FeeDetails.Remove(feedetail);
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

}