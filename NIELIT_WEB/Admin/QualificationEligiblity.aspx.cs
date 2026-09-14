using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class QualificationEligiblityForm : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    fillApplicantType();
                    fillQLevels();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    fillFilterApplicantType();
                    fillFilterQLevels();
                    //fillEffectiveFromDt();
                    fillApplicantType();
                    fillQLevels();
                    BindGridView();
                   
                    //if (ddlEffDtFrom.Items.Count > 0)
                    //{
                    //    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Eligibility (WEF: " + ddlEffDtFrom.SelectedValue + ")", "", ""));
                    //}
                    //else
                    //{
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Eligibility (WEF: Not Defined)", "", ""));
                    //}
                    
                    //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Eligibility", "Admin/QualificationEligiblity.aspx", ""));
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
    protected void fillFilterApplicantType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillApplicant = (from p in context.ApplicantTypes
                                     orderby p.Name
                                     select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAtype, fillApplicant, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillApplicantType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillApplicant = (from p in context.ApplicantTypes
                                     orderby p.Name
                                     select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, fillApplicant, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillFilterQLevels()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillQualification = (from p in context.QualificationLevels
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
    protected void fillQLevels()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillQualification = (from p in context.QualificationLevels
                                         orderby p.DisplayOrder
                                         select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlQualLevel , fillQualification, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void fillEffectiveFromDt()
    //{
    //    try
    //    {
    //        Int32 CouID = 0;
    //        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
    //        {
    //            CouID = Convert.ToInt32(Request.QueryString["CourseId"]);
    //        }
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //           var fillDt = (from p in context.QualificationEligibility
    //                         where p.CourseID == CouID
    //                         select new { ValueField = p.EffectiveDateFrom, TextField = p.EffectiveDateFrom }).Distinct().OrderByDescending(d => d.TextField);
    //           EConnect.Utils.Common.ControlUtility.BindListObject(ddlEffDtFrom, fillDt, null);
    //           foreach (ListItem lst1 in ddlEffDtFrom.Items)
    //           {
    //               if (lst1.Value != "0")
    //               {
    //                   lst1.Value = Convert.ToDateTime(lst1.Value).ToString("dd-MMM-yyyy");
    //                   lst1.Text = lst1.Value;
    //               }
    //           }
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected void ShowEditMode()
    {

        try
        {
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Form Header Detail";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
             
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
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 CouID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                CouID = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            Int32 applicantID = 0;
            if (ddlAtype.SelectedValue != "0")
                applicantID = Convert.ToInt32(ddlAtype.SelectedValue);
            Int32 qID = 0;
            if (ddlQlevels.SelectedValue != "0")
                qID = Convert.ToInt32(ddlQlevels.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var query = from s in context.QualificationEligibility
                        select new
                        {
                            ID=s.ID,
                            applicantTypeID=s.ApplicantTypeID,
                            ApplicantType=s.ApplicantType.Name,
                            qLevelId=s.QualificationLevelID,
                            qualificationLevel=s.QualificationLevel.Name,
                            experience=s.Experience ,
                            courseID=s.CourseID
                        };
            if (CouID != 0)
            {
                query = query.Where(s => s.courseID == CouID);
            }
            if (applicantID != 0)
            {
                query = query.Where(s => s.applicantTypeID == applicantID);
            }
            if (qID != 0)
            {
                query = query.Where(s => s.qLevelId == qID);
            }
            //if (ddlEffDtFrom.Items.Count > 0)
            //{
            //    DateTime effectiveDateFrom = Convert.ToDateTime(ddlEffDtFrom.SelectedValue);
            //    query = query.Where(d => d.EffectiveDateFrom == effectiveDateFrom);
            //}
            //else
            //    query = query.Where(d => d.EffectiveDateFrom == null);
           
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ApplicantType":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.ApplicantType);
                        else
                            query = query.OrderBy(s => s.ApplicantType);
                        break;
                    case "qualificationLevel":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.qualificationLevel);
                        else
                            query = query.OrderBy(s => s.qualificationLevel);
                        break;
                    case "experience":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.experience);
                        else
                            query = query.OrderBy(s => s.experience);
                        break;
                    default:
                        query = query.OrderBy(s => s.ApplicantType);
                        break;
                }
            }
            //if (ddlEffDtFrom.SelectedIndex > 0)
            //{
            //    //gvMain.Columns[gvMain.Columns.Count - 1].Visible = false;
            //    gvMain.Columns[gvMain.Columns.Count - 2].Visible = false;
            //    gvMain.ShowFooter = false;
            //}
            //else
            //{
            //    //gvMain.Columns[gvMain.Columns.Count - 1].Visible = true;
            //    gvMain.Columns[gvMain.Columns.Count - 2].Visible = true;
            //    gvMain.ShowFooter = true;
            //}
            PagingBar1.Bind(query, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            //if (gvMain.Rows.Count >= 2)
            //{
            //    btnMode.Visible = false;
            //    gvMain.FooterRow.Visible = false;
            //}
            //else
            //{
            //    btnMode.Visible = true;
            //    gvMain.FooterRow.Visible = true;
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
            fillApplicantType();
            fillQLevels();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Qualification Eligibility Details";
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Qualification Eligibility", "", ""));
            //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Qualification Eligibility", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
            }
            else
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect("QualificationEligiblity.aspx", true);
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
            context = new EConnectContext();
            //create and object 
            QualificationEligibility ObjQeligibility ;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                Course objCourse = new EConnect.NIELIT.Course();
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Int32 applicantTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 qualificationLevelID = Convert.ToInt32(ddlQualLevel.SelectedValue);
                objCourse = context.Courses.Find(couID);

                //finding duplicate data
                if(context.QualificationEligibility.Any(n=> (n.CourseID == couID && n.ApplicantTypeID == applicantTypeID && n.QualificationLevelID == qualificationLevelID)))
                {
                    throw new Exception("Record already exist for selected Applicant Type and Qualification Level.");
                }

                ObjQeligibility = new QualificationEligibility();
                ObjQeligibility.CourseID = objCourse.ID;
                ObjQeligibility.CourseCategoryID = objCourse.CourseCategoryID;
                ObjQeligibility.ApplicantTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                ObjQeligibility.QualificationLevelID = Convert.ToInt32(ddlQualLevel.SelectedValue);
                ObjQeligibility.Experience = Convert.ToDecimal(txtExperienceYrs.Text);
                ObjQeligibility.EffectiveDateFrom = new DateTime(1990, 1, 1);
                context.QualificationEligibility.Add(ObjQeligibility);
                context.SaveChanges();
                 
                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                strMessage = "Record updated.";
            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&msg=" ), true);
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?msg=" ), true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Qualification Eligibility", "", ""));
            upBreadCrumb.Update();
            PagingBar1.CurrentPageIndex = 0;
            BindGridView();
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
            ddlAtype.SelectedValue = "0";
            ddlQlevels.SelectedValue = "0";
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
                if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call delete function
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
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
                //Encryption url of hypelink field
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                if (gvMain.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddlApplicant = (DropDownList)e.Row.FindControl("ddlApplicantType");
                    using (EConnectContext context = new EConnectContext())
                    {
                       var fillApplicant = (from p in context.ApplicantTypes
                                            orderby p.DisplayOrder 
                                            select new { ValueField = p.ID, TextField = p.Name });
                        ListItem lst = new ListItem("--Select One--", "0");
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlApplicant, fillApplicant,lst);

                        ddlApplicant.Items.FindByText((e.Row.FindControl("lblAType") as Label).Text).Selected = true;
                    };
                    DropDownList ddlQualification = (DropDownList)e.Row.FindControl("ddlQLevel");
                    using (EConnectContext context = new EConnectContext())
                    {
                        var fillQualification = (from p in context.QualificationLevels
                                                 orderby p.DisplayOrder
                                                 select new { ValueField = p.ID, TextField = p.Name });
                        ListItem lst = new ListItem("--Select One--", "0");
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlQualification, fillQualification, lst);

                        ddlQualification.Items.FindByText((e.Row.FindControl("lbQLevel") as Label).Text).Selected = true;
                    };
                }
              
               
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlApplicantFooter = (DropDownList)e.Row.FindControl("ddlApplicantType1");
                using (EConnectContext context = new EConnectContext())
                {
                    var fillApplicant = (from p in context.ApplicantTypes
                                         orderby p.Name
                                         select new { ValueField = p.ID, TextField = p.Name });
                    ListItem lst = new ListItem("--Select One--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlApplicantFooter, fillApplicant, lst);
                };
                DropDownList ddlQlevelFooter = (DropDownList)e.Row.FindControl("ddlQLevel1");
                using (EConnectContext context = new EConnectContext())
                {
                    var fillQualification = (from p in context.QualificationLevels
                                             orderby p.Name
                                             select new { ValueField = p.ID, TextField = p.Name });
                    ListItem lst = new ListItem("--Select One--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlQlevelFooter, fillQualification, lst);
                };
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
            BreadCrumb1.RemoveLastBreadCrumbItem();
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("QualificationEligiblity.aspx", true);
        }
    }
    protected void gvMain_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvMain.EditIndex = e.NewEditIndex;
            gvMain.Columns[gvMain.Columns.Count - 1].Visible = false;
            BindGridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }
    protected void gvMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvMain.EditIndex = -1;
            gvMain.Columns[gvMain.Columns.Count - 1].Visible = true;
            BindGridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }       
    }
    protected void gvMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            gvMain.Columns[gvMain.Columns.Count - 1].Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                GridViewRow row = (GridViewRow)gvMain.Rows[e.RowIndex];
                Int32 id = Int32.Parse(gvMain.DataKeys[e.RowIndex].Value.ToString());
                TextBox txtEditTemplate = (TextBox)row.FindControl("txtExperience");
                TextBox tExperience = (TextBox)row.FindControl("txtExperience");
                DropDownList dApplicant = (DropDownList)row.FindControl("ddlApplicantType");
                DropDownList dQlevel = (DropDownList)row.FindControl("ddlQLevel");
                Int32 applicantTypeID = Convert.ToInt32(dApplicant.SelectedValue);
                Int32 qualificationLevelID = Convert.ToInt32(dQlevel.SelectedValue);
                QualificationEligibility objQEligibility = new QualificationEligibility();
                objQEligibility = context.QualificationEligibility.Find(id);
                if (context.QualificationEligibility.Any(n => (n.CourseID == objQEligibility.CourseID && n.ApplicantTypeID == applicantTypeID && n.QualificationLevelID == qualificationLevelID && n.ID != objQEligibility.ID)))
                {
                    BindGridView();
                    throw new Exception("Record already exist for selected Applicant Type and Qualification Level.");
                }
                
                objQEligibility.ApplicantTypeID = Convert.ToInt32(dApplicant.SelectedValue);
                objQEligibility.QualificationLevelID = Convert.ToInt32(dQlevel.SelectedValue);
                objQEligibility.Experience = Convert.ToDecimal(tExperience.Text);
                context.SaveChanges();
            };

            //strMessage = "Record updated.";
            String msg = "Record updated.";
            ShowAlert(msg, true);
            //Reset the edit index.
            gvMain.EditIndex = -1;
            BindGridView();
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            BindGridView();
            ShowAlert(ex.Message, true);
        }      
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                //DropDownList ddlAppType = (DropDownList)gvMain.FooterRow.FindControl("ddlApplicantType1");
                //DropDownList ddlQualLevel = (DropDownList)gvMain.FooterRow.FindControl("ddlQLevel1");
                //TextBox tExp = (TextBox)gvMain.FooterRow.FindControl("txtExp");
                //Int32 AppType= Convert.ToInt32(ddlAppType.SelectedValue);
                //Int32 QualLevel=Convert.ToInt32(ddlQualLevel.SelectedValue);
                //using (EConnectContext context = new EConnectContext())
                //{
                //    Course objCourse = new EConnect.NIELIT.Course();
                //    Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                //    objCourse = context.Courses.Find(couID);
                //    var q = (from n in context.QualificationEligibility
                //             where n.CourseID == couID
                //             group n by n.ID into g
                //             select new { ID = g.Key, Date = g.Max(t => t.EffectiveDateFrom) }).FirstOrDefault(); 
                //    QualificationEligibility objQEligibility = new EConnect.NIELIT.QualificationEligibility();
                //    if (context.QualificationEligibility.Any(s => s.ApplicantTypeID == AppType
                //                                             && s.QualificationLevelID == QualLevel
                //                                             && s.EffectiveDateFrom == q.Date))
                //    {
                //        throw new Exception("Data already exixts");
                //    }
                //    else
                //    {
                //        objQEligibility.ApplicantTypeID = AppType;
                //        objQEligibility.QualificationLevelID = QualLevel;
                //         objQEligibility.Experience = Convert.ToInt32(tExp.Text);
                //         objQEligibility.EffectiveDateFrom = q.Date;
                //         objQEligibility.CourseID = objCourse.ID;
                //         objQEligibility.CourseCategoryID = objCourse.CourseCategoryID;
                //         context.QualificationEligibility.Add(objQEligibility);
                //         context.SaveChanges();
                //         String msg = "Record Saved.";
                //         ShowAlert(msg, true);
                //    }

                //};
                //gvMain.EditIndex = -1;
                //BindGridView();
                //if (gvMain.Rows.Count >= 2)
                //{
                //    btnMode.Visible = false;
                //    gvMain.FooterRow.Visible = false;
                //}
                //else
                //{
                //    btnMode.Visible = true;
                //    gvMain.FooterRow.Visible = true;
                //}
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void gvMain_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            
            Int32 ID =  Convert.ToInt32(gvMain.DataKeys[e.RowIndex].Values[0].ToString());
            context = new EConnectContext();
          
            QualificationEligibility Q = context.QualificationEligibility.Find(ID);
            context.QualificationEligibility.Remove(Q);
          
            context.SaveChanges();
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
                gvMain.FooterRow.Visible = true;
            }
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}