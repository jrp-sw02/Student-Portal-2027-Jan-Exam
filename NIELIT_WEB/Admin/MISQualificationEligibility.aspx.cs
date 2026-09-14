using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
public partial class Admin_MISQualificationEligibility : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        cbQualifications.Attributes.Add("onclick", "checkBoxList1OnCheck(this);");
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {

                    //fillQLevels();
                    BindGridQualificationLevels();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    fillFilterQLevels();
                    FillCategories();
                    //fillQLevels();
                    BindGridQualificationLevels();
                    BindGridView();
                    FillCategoriesF();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Qualification Eligibility (WEF: Not Defined)", "", ""));

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

    protected void FillCategoriesF()
    {
        try
        {
            using (DataTable dt = FillCourseCategoryFilter())
            {
                if (dt.Rows.Count > 0)
                {
                    ddlcoursecategoryF.DataSource = dt;
                    ddlcoursecategoryF.DataTextField = "Name";
                    ddlcoursecategoryF.DataValueField = "ID";
                    ddlcoursecategoryF.DataBind();
                    ddlcoursecategoryF.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FillCourseCategoryFilter()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryForMISQualification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void ddlcoursecategoryF_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCourseNameF.Items.Clear();
        FillCourseFilter(Convert.ToInt32(ddlcoursecategoryF.SelectedValue));
    }
    protected void FillCourseFilter(int pCourseCategory)
    {
        try
        {
            using (DataTable dt = FillCourseRecordsFilter(pCourseCategory))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourseNameF.DataSource = dt;
                    ddlCourseNameF.DataTextField = "Name";
                    ddlCourseNameF.DataValueField = "ID";
                    ddlCourseNameF.DataBind();
                    ddlCourseNameF.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
                BreadCrumb1.Render();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable FillCourseRecordsFilter(int pCourseCategory)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getMISQualificationElbFilter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }
    protected void fillFilterQLevels()
    {
        try
        {
            NIELITMISContext context1 = new NIELITMISContext();
            using (EConnectContext context = new EConnectContext())
            {
                var fillQualification = (from p in context.QualificationLevels
                                        // join v in context1.MISQualification_Eligibility on p.ID equals v.QualificationLevelID
                                         orderby p.DisplayOrder
                                         select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlQlevels, fillQualification, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void fillQLevels()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var fillQualification = (from p in context.QualificationLevels
    //                                     orderby p.DisplayOrder
    //                                     select new { ValueField = p.ID, TextField = p.Name });
    //            ListItem lst = new ListItem("--All--", "0");
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlQualLevel, fillQualification, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }
    protected void BindGridQualificationLevels()
    {
        try
        {
            lblError.Visible = false; 
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            DataTable DT = new DataTable();
          


            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("select ID, Name FROM [NIELIT].[dbo].[Qualification_Level] ORDER BY  Display_Order", con))
            {
                Cmm.CommandType = CommandType.Text;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
            con.Close();

            cbQualifications.DataSource = DT;
            cbQualifications.DataTextField = "Name";
            cbQualifications.DataValueField = "ID";
            cbQualifications.DataBind();
            uPnlGrid.Update();
            uPnlNavigation.Update();

            cbQualifications.Visible = true;
            allChkBox.Visible = false;

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
    protected void allChkBox_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem chkitem in cbQualifications.Items)
        {
            if (allChkBox.Checked == true)
            {
                chkitem.Selected = true;
                cbQualifications.BackColor = System.Drawing.Color.LightGreen;
                
            }
            else
            {
                chkitem.Selected = false;
                cbQualifications.BackColor = System.Drawing.Color.White;
            }
        }
    }

    //protected void cbQualifications_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Control chk = ((Control)sender).FindControl("chk");
    //    CheckBoxList ch = (CheckBoxList)chk;
    //    if (ch.Items[ch.SelectedIndex].Selected)
    //        ch.Items[ch.SelectedIndex].Attributes.Add("Style", "background-color: red;");

    //}
    protected void FillCategories()
    {
        try
        {
            NIELITMISContext context1 = new NIELITMISContext();
            ddlcoursecategory.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context1.NielitCentreCourseCategorys
                               where p.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            using (NIELITMISContext context1 = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context1.NielitCentreCourses
                                 join d in context1.NielitCourseDurations on p.ID equals d.courseID
                                 where p.CourseCategoryID == coursecatID && p.IsActive == true
                                 orderby (p.Name) ascending
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" + " (" + d.courseDurationDays + "Days" + ")" + " (" + d.courseDurationHrs + "Hours" + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList.Distinct(), lst);

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool validateupdate()
    {
        int count = 0;
        bool update = false;
        foreach (ListItem li in cbQualifications.Items)
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
            using (NIELITMISContext context1 = new NIELITMISContext())
            {
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                btnSave.Text = "Update";
                lblHeading.Text = "MIS Qualification Eligibility Detail";
                FillCategories();
               // fillQLevels();
                BindGridQualificationLevels();
                FillCategories();
                Int32 ID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ID = Convert.ToInt32(Request.QueryString["Key"]);
                }
                //var query = (from s in context1.MISQualification_Eligibility
                //             where s.ID == ID
                //             select new
                //             {
                //                 ID = s.ID,
                //                 CourseCat = s.CourseCategoryID,
                //                 CourseID = s.CourseID,
                //                 Experience = s.Experience,
                //                 EffectiveFromDate = s.EffectiveDateFrom,
                //                 EffectiveToDate = s.EffectiveDateTo,
                //                 QualificationId = s.QualificationLevelID
                                 
                //             }).ToList();
                //if (query != null)
                //{
                MISQualification_Eligibility query = context1.MISQualification_Eligibility.Find(Convert.ToInt32(Request.QueryString["Key"]));

                    ddlcoursecategory.SelectedValue = Convert.ToInt32(query.CourseCategoryID).ToString();
                    ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);
                    ddlcourseName.SelectedValue = query.CourseID.ToString();

                    foreach (ListItem li in cbQualifications.Items)
                    {
                        if (li.Value == query.QualificationLevelID.ToString())
                        {
                            li.Selected = true;
                           //cbQualifications.SelectedItem.Attributes["style"] = "color:red";
                            cbQualifications.SelectedItem.Attributes["style"] = "BackGround-color: LightGreen";                            
                        }                       
                    }
                    txtExperienceYrs.Text = query.Experience.ToString();
                   // ddlQualLevel.SelectedValue = Convert.ToInt32(query.QualificationId).ToString();
                  
                    txtEffectiveFromDt.Text = query.EffectiveDateFrom.ToString("dd-MMM-yyyy");
                    txtEffectiveToDt.Text = query.EffectiveDateTo.ToString();
                    ddlcoursecategory.Enabled = false;
                    ddlcourseName.Enabled = false;
                 //   txtEffectiveToDt.Text = query.EffectiveToDate.ToString("dd-MMM-yyyy");
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Qualification Eligibility", "Admin/MISQualificationEligibility.aspx?" + Request.QueryString.ToString(), ""));  
                   
                    ViewState["LastModifiedOn"] = DateTime.Now;
               // }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

     public DataTable FillGridViewMISQualificationElibibility()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getMISQualificationEligibilityDetails", con))
                {                   
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            NIELITMISContext context1 = new NIELITMISContext();
            Int32 CouID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                CouID = Convert.ToInt32(Request.QueryString["Key"]);
            }
            Int32 qID = 0;
            Int32 CourseCatId = 0;
            Int32 CourseId = 0;
            if (ddlcoursecategoryF.SelectedValue != "0")
                CourseCatId = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);
            if (ddlCourseNameF.SelectedValue != "0")
                CourseId = Convert.ToInt32(ddlCourseNameF.SelectedValue);
            if (ddlQlevels.SelectedValue != "0")
                qID = Convert.ToInt32(ddlQlevels.SelectedValue);

            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

             using (DataTable dt = FillGridViewMISQualificationElibibility())

                 if (dt.Rows.Count > 0)
                 {
                     var courses = (from p in dt.AsEnumerable()
                                    select new
                                    {
                                        ID = p.Field<int>("ID"),
                                        coursecatName = p.Field<string>("coursecatName"),
                                        Name = p.Field<string>("Name"),
                                        Experience = p.Field<decimal>("Experience"),
                                        CourseCatId = p.Field<int>("CourseCatId"),
                                        CourseId = p.Field<int>("CourseId"),
                                        EffFromDate = p.Field<string>("EffFromDate"),
                                        EffToDate = p.Field<string>("EffToDate"),
                                        QualLEvelId = p.Field<int>("QualLEvelId"),
                                        QualificationLevelName = p.Field<string>("QualificationLevelName")
                                    });
                     
                     if (CouID != 0)
                     {
                         courses = courses.Where(s => s.ID == CouID);
                     }

                     if (CourseCatId != 0)
                     {
                         courses = courses.Where(s => s.CourseCatId == CourseCatId);
                     }
                     if (CourseId != 0)
                     {
                         courses = courses.Where(s => s.CourseId == CourseId);
                     }
                     if (qID != 0)
                     {
                         courses = courses.Where(s => s.QualLEvelId == qID);
                     }
                     

                     if (!string.IsNullOrEmpty(sortOrder))
                     {
                         switch (sortField)
                         {
                             case "course":
                                 if (sortOrder == "DESC")
                                     courses = courses.OrderByDescending(s => s.CourseId);
                                 else
                                     courses = courses.OrderBy(s => s.CourseId);
                                 break;

                             case "qualificationLevel":
                                 if (sortOrder == "DESC")
                                     courses = courses.OrderByDescending(s => s.QualLEvelId);
                                 else
                                     courses = courses.OrderBy(s => s.QualLEvelId);
                                 break;
                             case "experience":
                                 if (sortOrder == "DESC")
                                     courses = courses.OrderByDescending(s => s.Experience);
                                 else
                                     courses = courses.OrderBy(s => s.Experience);
                                 break;

                             default:
                                 courses = courses.OrderBy(s => s.CourseCatId);
                                 break;
                         }
                     }

                     PagingBar1.Bind(courses, ref gvMain);
                     uPnlGrid.Update();
                     uPnlNavigation.Update();
                     if (gvMain.Rows.Count <= 0)
                     {
                         lblError.Text = "No record found.";
                         lblError.Visible = true;
                         gvMain.Visible = false;
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

            //fillQLevels();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New MIS Qualification Eligibility Details";
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New MIS Qualification Eligibility", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("MISQualificationEligibility.aspx?ID=" + Request.QueryString["Key"].ToString()), true);
            }
            else
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect("MISQualificationEligibility.aspx", true);
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
            int Countskip = 0;
            BreadCrumb1.Render();
            NIELITMISContext context1 = new NIELITMISContext();
            MISQualification_Eligibility ObjQeligibility = new MISQualification_Eligibility();

            Int32 courseID = Convert.ToInt32(ddlcourseName.SelectedValue.Trim());


            DateTime QulaificationEffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDt.Text);
            DateTime extdate = System.DateTime.Now;
            if (QulaificationEffectiveFromDate > extdate)
            {
                ShowAlert("Effective Date From  should be  current date or before date.");
                return;
            }       

            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                foreach (ListItem li in cbQualifications.Items)
                {
                    if (li.Selected)
                                  {
                        Int32 QulaificationLevelID = Convert.ToInt32(li.Value);

                        if (context1.MISQualification_Eligibility.Where(s => s.QualificationLevelID == QulaificationLevelID && s.CourseID == courseID).Count() == 0)
                        {   
                            ObjQeligibility.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue.Trim());
                            ObjQeligibility.CourseID = Convert.ToInt32(ddlcourseName.SelectedValue.Trim());

                            ObjQeligibility.QualificationLevelID = QulaificationLevelID;
                            ObjQeligibility.Experience = Convert.ToDecimal(txtExperienceYrs.Text);
                            ObjQeligibility.EffectiveDateFrom = Convert.ToDateTime(txtEffectiveFromDt.Text);
                            ObjQeligibility.EffectiveDateTo = Convert.ToDateTime(txtEffectiveToDt.Text);
                            ObjQeligibility.enterDate = DateTime.Now;
                            ObjQeligibility.enterByID = Convert.ToInt64(Session["UserID"]);
                            context1.MISQualification_Eligibility.Add(ObjQeligibility);
                            context1.SaveChanges();
                        }
                        else
                        {
                            Countskip = Countskip + 1;
                        }

                    }

                }

                strMessage = "New record saved.";
            }
            else
            {
                ObjQeligibility = context1.MISQualification_Eligibility.Find(Convert.ToInt32(Request.QueryString["Key"]));

                foreach (ListItem li in cbQualifications.Items)
                {
                    if (li.Selected)
                    {                       
                        Int32 QulaificationLevelID = Convert.ToInt32(li.Value);

                        if (context1.MISQualification_Eligibility.Where(s => s.QualificationLevelID == QulaificationLevelID && s.CourseID == courseID).Count() == 0)
                        {
                            ObjQeligibility.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue.Trim());
                            ObjQeligibility.QualificationLevelID = QulaificationLevelID;
                            ObjQeligibility.Experience = Convert.ToDecimal(txtExperienceYrs.Text);
                            ObjQeligibility.EffectiveDateFrom = Convert.ToDateTime(txtEffectiveFromDt.Text);
                            ObjQeligibility.EffectiveDateTo = Convert.ToDateTime(txtEffectiveToDt.Text);
                            ObjQeligibility.enterDate = DateTime.Now;
                            ObjQeligibility.enterByID = Convert.ToInt64(Session["UserID"]);
                            context1.MISQualification_Eligibility.Add(ObjQeligibility);
                            context1.SaveChanges();
                        }
                        else
                        {
                            //ObjQeligibility.QualificationLevelID = Convert.ToInt32(ddlQualLevel.SelectedValue);
                            ObjQeligibility.QualificationLevelID = QulaificationLevelID;
                            ObjQeligibility.Experience = Convert.ToDecimal(txtExperienceYrs.Text);
                            ObjQeligibility.EffectiveDateFrom = Convert.ToDateTime(txtEffectiveFromDt.Text);
                            ObjQeligibility.EffectiveDateTo = Convert.ToDateTime(txtEffectiveToDt.Text);
                            ObjQeligibility.enterDate = DateTime.Now;
                            ObjQeligibility.enterByID = Convert.ToInt64(Session["UserID"]);
                            context1.SaveChanges();
                        }
                    }
                }
                strMessage = "Record Updated";
            }

            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("MISQualificationEligibility.aspx?msg="), false);
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
            BindGridView();

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
            ddlcoursecategoryF.SelectedValue = "0";
            ddlCourseNameF.SelectedValue = "0";
            ddlQlevels.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
                if (btnAction.CommandName == "Delete")
                {                    
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Action")
                {                  
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {       
            Response.Redirect("MISQualificationEligibility.aspx", true);       
    }
    
    protected void gvMain_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            Int32 ID = Convert.ToInt32(gvMain.DataKeys[e.RowIndex].Values[0].ToString());
            NIELITMISContext context1 = new NIELITMISContext();

            MISQualification_Eligibility Q = context1.MISQualification_Eligibility.Find(ID);
            context1.MISQualification_Eligibility.Remove(Q);

            context1.SaveChanges();
            String msg = "Record deleted successfully";
            ShowAlert(msg, true);
            BindGridView();
            if (gvMain.Rows.Count >= 2)
            {
                btnMode.Visible = false;
                gvMain.FooterRow.Visible = false;

            }
            else
            {
                btnMode.Visible = true;
                gvMain.FooterRow.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}
