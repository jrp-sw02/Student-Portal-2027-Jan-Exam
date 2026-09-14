using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
public partial class Download : BasePage
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
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCategories();

                    EnumUtility.BindListObject(ref ddlDownloadType, typeof(EConnect.NIELIT.enmDownloadableType), new ListItem("--Select One--", "0"));
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCategories();
                    FillFilterCategories();
                    EnumUtility.BindListObject(ref ddlDownloadType, typeof(EConnect.NIELIT.enmDownloadableType), new ListItem("--Select One--", "0"));
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]) || !String.IsNullOrEmpty(Request.QueryString["CategoryID"]))
                    {

                        using (var context = new EConnectContext())
                        {
                            Int32 myCourseId1 = Convert.ToInt32(Request.QueryString["CourseId"]);
                            Int32 CategoryID = Convert.ToInt32(Request.QueryString["CategoryId"]);
                            var query = (from s in context.Courses
                                         where s.ID == myCourseId1
                                         select s).FirstOrDefault();
                            ddlficoursecategory.SelectedValue = query.CourseCategoryID.ToString();
                            ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
                            ddlFiCourse.SelectedValue = query.ID.ToString();
                            ddlficoursecategory.Enabled = false;
                            ddlFiCourse.Enabled = false;
                        };
                    }
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Uploaded Documents", "Admin/Download.aspx?" + Request.QueryString.ToString(), ""));
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
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                ListItem lst1 = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, Category.Distinct(), lst1);
                }
                else
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, Category.Distinct(), lst);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlficoursecategory, Category.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlficoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFiCourse.Items.Clear();
        FillFilterCourses();
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCourseName.Items.Clear();
        FillCourses();
    }
    protected void FillFilterCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                int id = Convert.ToInt32(ddlficoursecategory.SelectedValue);

                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFiCourse, CourseList, lst);
            };
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
                int catId = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == catId
                                 select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            imgDownload.Visible = true;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            trMsg.Visible = true;
            btnSave.Text = "Update";
            lblHeading.Text = "Uploaded Document Details";
            lblDownloadFile.Visible = true;
            Downloadable objDownload = context.Downloadables.Find(Convert.ToInt32(Request.QueryString["Key"]));
            ddlDownloadType.SelectedValue = objDownload.DownloadableTypeID.ToString();
            txtLinkName.Text = objDownload.LinkName.ToString();
            txtEffectiveDtFrom.Text = objDownload.EffectiveFromDate.ToString("dd-MMM-yyyy");
            ddlShow.SelectedValue = (Convert.ToInt32(objDownload.ShowOnWeb)).ToString();
            ddlCourseCategory.SelectedValue = objDownload.CourseCategoryID.ToString();
            ddlCourseCategory.Enabled = false;
            ddlcoursecategory_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
            ddlCourseName.SelectedValue = objDownload.CourseID.ToString();
            ddlCourseName.Enabled = false;

            hfFileID.Value = objDownload.DownloadableFileID.ToString();
            imgDownload.Attributes.Add("OnClick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../Handlers/UploadedFileHandler.ashx?ID=" + hfFileID.Value) + "'); return false;");
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objDownload.DownloadableType.ToString(), "Admin/Download.aspx?" + Request.QueryString.ToString(), ""));
            // Button1.Attributes.Add("Onclick", "window.open('ViewApplicationStatus.aspx?Type="+level+"','Form'); return false;");
            //tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
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
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 CouCatID = 0;
            Int32 CouID = 0;
            Int32 cid1 = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                cid1 = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            if (ddlficoursecategory.SelectedValue != "0")
                CouCatID = Convert.ToInt32(ddlficoursecategory.SelectedValue);
            if (ddlFiCourse.SelectedValue != "0")
                CouID = Convert.ToInt32(ddlFiCourse.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var download = from s in context.Downloadables
                           select new
                                  {
                                      ID = s.ID,
                                      downloadType = s.DownloadableTypeID,
                                      linkName = s.LinkName,
                                      CourseID = s.CourseID,
                                      effectiveDtFrom = s.EffectiveFromDate,
                                      CourseCategoryID = s.CourseCategoryID,
                                      CategoryName = (!string.IsNullOrEmpty(s.CourseCategory.Code) ? s.CourseCategory.Code : "All") + "-" + (!string.IsNullOrEmpty(s.Course.Code) ? s.Course.Code : "All"),
                                      fileID = s.DownloadableFileID
                                  };

            if (cid1 != 0)
            {
                download = download.Where(s => s.CourseID == cid1);
            }
            if (download.Count() > 0)
            {
                if (CouCatID != 0)
                {
                    download = download.Where(s => s.CourseCategoryID == CouCatID);
                }
                if (CouID != 0)
                {
                    download = download.Where(s => s.CourseID == CouID);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    download = download.Where(s => s.linkName.ToUpper().Contains(searchString));
                }
                //if (userType != 0)
                //    download = download.Where(s => s.UserTypeID == userType);
                download = download.OrderBy(s => s.linkName);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "downloadType":
                            if (sortOrder == "DESC")
                                download = download.OrderByDescending(s => s.downloadType);
                            else
                                download = download.OrderBy(s => s.downloadType);
                            break;
                        case "linkName":
                            if (sortOrder == "DESC")
                                download = download.OrderByDescending(s => s.linkName);
                            else
                                download = download.OrderBy(s => s.linkName);
                            break;
                        case "effectiveDtFrom":
                            if (sortOrder == "DESC")
                                download = download.OrderByDescending(s => s.effectiveDtFrom);
                            else
                                download = download.OrderBy(s => s.effectiveDtFrom);
                            break;
                        case "CategoryName":
                            if (sortOrder == "DESC")
                                download = download.OrderByDescending(s => s.CategoryName);
                            else
                                download = download.OrderBy(s => s.CategoryName);
                            break;
                        default:
                            download = download.OrderBy(s => s.downloadType);
                            break;
                    }
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    download = download.Where(a => roleCourses.Contains(a.CourseCategoryID.Value));
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    download = download.Where(a => roleCourses.Contains(a.CourseID.Value));
                }
                PagingBar1.Bind(download, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            else
            {
                Lblerror.Visible = true;
                Lblerror.Text = "No documents Uploaded";
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
            FillCategories();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            txtEffectiveDtFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //Change the heading text as required
            lblHeading.Text = "Upload New Document";
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                using (var context = new EConnectContext())
                {
                    Int32 myCourseId1 = Convert.ToInt32(Request.QueryString["CourseId"]);
                    var query = (from s in context.Courses
                                 where s.ID == myCourseId1
                                 select s).FirstOrDefault();
                    ddlCourseCategory.SelectedValue = query.CourseCategoryID.ToString();
                    ddlcoursecategory_SelectedIndexChanged(ddlCourseCategory, EventArgs.Empty);
                    ddlCourseName.SelectedValue = query.ID.ToString();
                    ddlCourseCategory.Enabled = false;
                    ddlCourseName.Enabled = false;
                };
            }
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload New Document", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Download.aspx?CourseId=" + Request.QueryString["CourseId"]), true);
            }
            else
            {
                Response.Redirect("Download.aspx", true);
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
            Boolean success = false;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                if (FileUpload1.HasFile)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            //Course objCourse = new EConnect.NIELIT.Course();
                            //int CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
                            //objCourse = context.Courses.Find(CourseID);

                            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                            {
                                if (ddlCourseCategory.SelectedValue == "0")
                                {
                                    ShowAlert("Please Select Course Category.", true);
                                    return;
                                }
                            }
                            //Save Downloadable Details.
                            Downloadable objDownload = new EConnect.NIELIT.Downloadable();
                            if (ddlCourseName.SelectedValue != "0")
                                objDownload.CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                            //objDownload.CourseCategoryID = objCourse.CourseCategoryID;
                            if (ddlCourseCategory.SelectedValue != "0")
                                objDownload.CourseCategoryID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                            objDownload.LinkName = txtLinkName.Text;
                            objDownload.EffectiveFromDate = Convert.ToDateTime(txtEffectiveDtFrom.Text);
                            objDownload.DownloadableTypeID = Convert.ToInt32(ddlDownloadType.SelectedValue);
                            objDownload.ShowOnWeb = Convert.ToBoolean(Convert.ToInt32(ddlShow.SelectedValue));
                            context.Downloadables.Add(objDownload);
                            context.SaveChanges();

                            //Save Downloadable File.
                            UploadedFile objFile = new EConnect.NIELIT.UploadedFile();
                            objFile.Name = "DN-M-" + objDownload.ID;
                            objFile.OriginalName = FileUpload1.FileName.ToString();
                            objFile.Extension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                            objFile.BlobFile = FileUpload1.FileBytes;
                            objFile.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(objFile);
                            context.SaveChanges();

                            //Update Downloadable table with uploaded file_id from uploaded_file table.
                            objDownload.DownloadableFileID = objFile.ID;
                            context.Entry(objDownload).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();

                            success = true;
                            if (success == true)
                                scope.Complete(); // Transaction Process finally Complete

                        };
                    };
                }
                else
                {
                    throw new Exception("Please select document to be uploaded");
                }

                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                using (EConnectContext context = new EConnectContext())
                {
                    if (FileUpload1.HasFile)
                    {
                        Downloadable objDownload = context.Downloadables.Find(Convert.ToInt32(Request.QueryString["Key"]));
                        objDownload.LinkName = txtLinkName.Text;
                        objDownload.EffectiveFromDate = Convert.ToDateTime(txtEffectiveDtFrom.Text);
                        objDownload.DownloadableTypeID = Convert.ToInt32(ddlDownloadType.SelectedValue);
                        objDownload.ShowOnWeb = Convert.ToBoolean(Convert.ToInt32(ddlShow.SelectedValue));

                        UploadedFile objFile = context.UploadedFiles.Find(objDownload.DownloadableFileID);
                        objFile.OriginalName = FileUpload1.FileName.ToString();
                        objFile.Extension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                        objFile.BlobFile = FileUpload1.FileBytes;
                        objFile.UploadedOn = DateTime.Now;

                        context.SaveChanges();
                    }
                    else
                    {
                        Downloadable objDownload = context.Downloadables.Find(Convert.ToInt32(Request.QueryString["Key"]));
                        objDownload.LinkName = txtLinkName.Text;
                        objDownload.EffectiveFromDate = Convert.ToDateTime(txtEffectiveDtFrom.Text);
                        objDownload.DownloadableTypeID = Convert.ToInt32(ddlDownloadType.SelectedValue);
                        objDownload.ShowOnWeb = Convert.ToBoolean(Convert.ToInt32(ddlShow.SelectedValue));

                        context.SaveChanges();
                    }
                };
                strMessage = "Record updated.";
            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Download.aspx?CourseId=" + Request.QueryString["CourseId"] + "&msg=" + strMessage), true);
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Download.aspx?msg=" + strMessage), true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            //context.Dispose(); 
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
            ddlFiCourse.SelectedValue = "0";
            ddlficoursecategory.SelectedValue = "0";
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
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            context = new EConnectContext();
            Downloadable d = context.Downloadables.Find(Convert.ToInt32(hfActionID.Value.ToString()));
            UploadedFile u = d.DownloadableFile;
            context.Downloadables.Remove(d);
            context.UploadedFiles.Remove(u);
            context.SaveChanges();
            BindGridView();
            ShowAlert("Record deleted successfully.");
            hfActionID.Value = "";
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
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);

                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;
                //HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                //h5.NavigateUrl = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();


                Image imgupload = (Image)e.Row.FindControl("imgDownload");
                if (imgupload != null)
                {
                    imgupload.Attributes.Add("onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../Handlers/UploadedFileHandler.ashx?ID=" + gvMain.DataKeys[e.Row.RowIndex].Values[1].ToString()) + "'); return false;");
                }
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
            var download = from s in context.Downloadables
                           select new { Name = s.LinkName };
            if (!String.IsNullOrEmpty(searchString))
            {
                download = download.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            download = download.OrderBy(s => s.Name);

            //var users1 = from s in context.Users
            //             select new { Name = s.LoginID };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            //}
            //download = download.Union(users1).Take(count);
            foreach (var linkName in download)
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("Download.aspx?CourseId=" + Request.QueryString["CourseId"]), true);
        }
        else
        {
            Response.Redirect("Download.aspx", true);
        }
    }
}