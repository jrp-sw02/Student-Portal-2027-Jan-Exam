using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class admfrmodule : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Admin/CertificateCourse.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    // BindState();
                    FillModuleType();
                    FillFilterModuleType();
                    FillFilterSelectionType();
                    FillFilterRevisionNumber();
                    //FillCourses();
                    FillSelectionType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillModuleType();
                    FillFilterModuleType();
                    FillFilterSelectionType();
                    FillFilterRevisionNumber();
                    FillSelectionType();
                    BindGridView();
                    if (!string.IsNullOrEmpty(Request.QueryString["src"]))
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Modules", "Admin/admfrmodule.aspx?" + Request.QueryString.ToString(), ""));
                      
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Modules", "Admin/admfrmodule.aspx?" + Request.QueryString.ToString(), ""));
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
    protected void FillFilterModuleType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.ModuleTypes
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlfiltermoduletype, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterSelectionType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusList = from p in context.SelectionTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlfilterselection, StatusList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterRevisionNumber()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                Course currentcourse = context.Courses.Find(courseID);
                if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    var RevisionNoList = (from p in context.Modules
                                          where p.CourseID == courseID
                                          select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber }).Distinct().ToList();
                    if (RevisionNoList.Count != 0)
                    {
                        ddlfilterRevisionNo.SelectedValue = RevisionNoList.Max(p => p.TextField.ToString());
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlfilterRevisionNo, RevisionNoList, null);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillModuleType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.ModuleTypes
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlmoduletype, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillSelectionType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusList = from p in context.SelectionTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlselectiontype, StatusList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillStates()
    {
        //try
        //{
        //    ListItem lst = new ListItem("--Select One--", "0");
        //    var statelist = from p in context.Locations
        //                    where p.LocationTypeID == 2
        //                    select new { ValueField = p.ID, TextField = p.Name };


        //    //var mylist = string.Concat(statelist,CourseList);
        //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlstates, statelist, lst);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    protected void FillFilterCategory()
    {
        //try
        //{
        //    ListItem lst = new ListItem("--Select One--", "0");
        //    var statelist = from p in context.CourseCategories
        //                    select new { ValueField = p.ID, TextField = p.Name };

        //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategry, statelist, lst);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    protected void FillFilterCourse()
    {
        //try
        //{
        //    ListItem lst = new ListItem("--Select One--", "0");
        //    var statelist = from p in context.Courses
        //                    select new { ValueField = p.ID, TextField = p.Name };

        //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcour, statelist, lst);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    protected void FillFilterStatus()
    {
    //    try
    //    {
    //        ListItem lst = new ListItem("--Select One--", "0");
    //        var statelist = from p in context.AccreditationStatus
    //                        select new { ValueField = p.ID, TextField = p.Name };

    //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlsts, statelist, lst);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
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
            lblHeading.Text = "Modules";
            using (EConnectContext context = new EConnectContext())
            {
                Course Cou;
                Cou = context.Courses.Find(Convert.ToInt32(Request.QueryString["CourseID"]));
                Int32 PractivalMId = Convert.ToInt32(Request.QueryString["Key"]);
                var Module = (from p in context.Modules
                              where p.ID == PractivalMId
                                 select new
                                 {
                                     ID = p.ID,
                                     Name=p.Name,
                                     RevisionNumber=p.RevisionNumber,
                                     ShortName=p.ShortName,
                                     ModuleTypeID=p.ModuleTypeID,
                                     ProjectNo=p.ProjectNumber,
                                     Synopsis=p.SynopsisRequired,
                                     ModuleNUmber=p.ModuleNUmber,
                                     ModuleSubNUmber=p.ModuleSubNUmber,
                                     SelectionTypeID=p.SelectionTypeID,
                                     ElectiveGroup=p.ElectiveGroup,
                                     ModuleSubCode=p.ModuleSubCode,
                                     Code=p.Code,
                                     ExamMode=p.ExamModeId,
                                     EffectiveFromDate=p.EffectiveFromDate,
                                     NoofModulesAllowed = p.NumberOfElectiveModulesAllowed
                                 }).FirstOrDefault();
                ddlrevisionno.SelectedItem.Text =Module.RevisionNumber.ToString();
                txtname.Text = Module.Name.ToString().ToUpper();
                txtsubname.Text = Module.ShortName.ToString().ToUpper();
                ddlmoduletype.SelectedValue = Module.ModuleTypeID.ToString();
                ddlmoduletype.Enabled = false;
                ddlProjectNo.SelectedValue = Module.ProjectNo.ToString();
                ddlSynopsis.SelectedValue = Module.Synopsis.ToString();
                ddlExamMode.SelectedValue = Module.ExamMode.ToString();
                txtmoduleno.Text = Module.ModuleNUmber.ToString();
                txtsubmoduleno.Text = Module.ModuleSubNUmber.ToString();
                //txtsubmoduleno.Text = Module.ModuleSubNUmber.ToString();
                ddlselectiontype.SelectedValue = Module.SelectionTypeID.ToString();
                ddlselectiontype_SelectedIndexChanged(ddlselectiontype, EventArgs.Empty);
                txtElectiveGroup.Text = Module.ElectiveGroup.ToString();
                txtmodulecode.Text = Module.Code.ToString().ToUpper();
                txteffectivefrom.Text = Module.EffectiveFromDate.ToString("dd-MMM-yyyy");
                txtSubCodeNumber.Text = Module.ModuleSubCode.ToString();
                txtallowedmodules.Text = Module.NoofModulesAllowed.ToString();
                //Updating breadscrumb
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Module.Code, "Admin/admfrmodule.aspx?" + Request.QueryString.ToString(), ""));
                BreadCrumb1.Render();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Module.Code.Value.ToString(), "Admin/admfrmodule.aspx?" + Request.QueryString.ToString(), ""));
                //accrediationdetails.aspx?key1=<%=  hfAccID.Value %> &name=<%=  hfName.Value  %>
                ViewState["LastModifiedOn"] = DateTime.Now;
                Int32 PracticalModuleID=Convert.ToInt32(txtmoduleno.Text);
                Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                Int32 RevisionNo = Convert.ToInt32(ddlrevisionno.SelectedItem.Text);
                Int32 ModuleTypeID = Convert.ToInt32(ddlmoduletype.SelectedValue);
                Int32 SelectionTypeID=Convert.ToInt32(ddlselectiontype.SelectedValue);
                Int32 ModuleTypePr = Convert.ToInt32(enmModuleType.Practical);
                Int32 MoudleTypePrj = Convert.ToInt32(enmModuleType.Project);
                Int32 SelectionType = Convert.ToInt32(enmSelectionType.Compulsory);
                Int32 SelectionTypeElec = Convert.ToInt32(enmSelectionType.Elective);
                if (ModuleTypeID == Convert.ToInt32(enmModuleType.Practical))
                {
                    tblPrEligibility.Visible = true;
                    //Binding CheckBox List
                    var ModuleList = from s in context.Modules
                                     where s.CourseID == CourseID && s.RevisionNumber == RevisionNo && s.ModuleTypeID != ModuleTypePr && s.ModuleTypeID != MoudleTypePrj && s.SelectionTypeID==SelectionType
                                     orderby (s.Code)
                                     select new { ValueField = s.ID, TextField = s.ShortName + "    " + s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(chkModulelist, ModuleList);
                    //Binding TreeView
                    var electiveGroup =( from s in context.Modules
                                             where s.CourseID == CourseID && s.RevisionNumber == RevisionNo && s.ModuleTypeID != ModuleTypePr && s.ModuleTypeID != MoudleTypePrj && s.SelectionTypeID == SelectionTypeElec
                                             orderby (s.ElectiveGroup)
                                             select s.ElectiveGroup).Distinct();
                    trvElective.Nodes.Clear();
                    foreach (Int32 grp in electiveGroup)
                    {
                        TreeNode tn = new TreeNode("Elective Group: "+grp.ToString(), grp.ToString());
                        trvElective.Nodes.Add(tn);
                        BindChildNodee(ref tn, CourseID, RevisionNo, grp);
                    }
                    trvElective.CollapseAll();
                }
                //------------------------------Fill Data-----------------------------------------------------------------
                var filldata = (from f in context.ModulePracticalEligibilities
                                where f.PracticalModuleID == PractivalMId && f.ElectiveGroup == null
                                select new { ModuleID = f.TheoryModuleID }).ToList();
                if (filldata.Count > 0)
                {
                    foreach (var s in filldata)
                    {
                        foreach (ListItem sli in chkModulelist.Items)
                        {
                            if (s.ModuleID == Convert.ToInt64(sli.Value))
                            {
                                sli.Selected = true;
                            }
                        }

                    }
                }
                var fillTreeView = (from f in context.ModulePracticalEligibilities
                                    where f.PracticalModuleID == PractivalMId && f.ElectiveGroup !=null
                                    select new { ElectiveGroupID = f.ElectiveGroup }).ToList();
                if (fillTreeView.Count > 0)
                {
                    foreach (var s in fillTreeView)
                    {
                        foreach (TreeNode n in trvElective.Nodes)
                        {
                            if (s.ElectiveGroupID == Convert.ToInt64(n.Value))
                            {
                                n.Checked = true;
                            }
                        }

                    }
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
    private void BindChildNodee(ref TreeNode ParentMenuItem, Int32 CourseID, Int32 RevisionNo,  Int32 electiveGroupID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var ModuleListElective = from s in context.Modules
                                         where s.CourseID == CourseID && s.RevisionNumber == RevisionNo && s.ElectiveGroup == electiveGroupID
                                         orderby (s.Code)
                                         select s;
                foreach (Module module in ModuleListElective)
                {
                    TreeNode child = new TreeNode(module.ShortName + "    " + module.Name, module.ID.ToString());
                    child.ShowCheckBox = false;
                    child.SelectAction = TreeNodeSelectAction.None;
                    ParentMenuItem.ChildNodes.Add(child);
                }
            };
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
                Course cou;
                cou = context.Courses.Find(Convert.ToInt64(Request.QueryString["CourseID"]));
               
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 ModuleTId = 0;
                Int32 SelectionId = 0;
                Int32 RevisionId = 0;

                if (ddlfiltermoduletype.SelectedValue != "0")
                    ModuleTId = Convert.ToInt32(ddlfiltermoduletype.SelectedValue);
                if (ddlfilterselection.SelectedValue != "0")
                    SelectionId = Convert.ToInt32(ddlfilterselection.SelectedValue);
                if (ddlfilterRevisionNo.SelectedValue != "0")
                    RevisionId = Convert.ToInt32(ddlfilterRevisionNo.SelectedValue);
                var modules = from s in context.Modules
                              where s.CourseID == cou.ID
                              select new { CourseID = s.CourseID, ID = s.ID, SeletionTID = s.SelectionTypeID, RevNo = s.RevisionNumber, Name = s.Name, SName = s.ShortName, Code = s.Code, ModuleTypeID = s.ModuleTypeID, MType = s.ModuleType.Name, ExamMode = s.ExamModeId == 1 ? "Online" : "Offline", EC = (s.ElectiveGroup.HasValue) ? "Yes" : "No" };

                if (!String.IsNullOrEmpty(searchString))
                {
                    modules = modules.Where(s => s.Name.ToUpper().Contains(searchString));
                }

                if (ModuleTId != 0)
                    modules = modules.Where(s => s.ModuleTypeID == ModuleTId);
                if (SelectionId != 0)
                {
                    modules = modules.Where(s => s.SeletionTID == SelectionId);

                }
                modules = modules.Where(s => s.RevNo == RevisionId);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                modules = modules.OrderByDescending(s => s.ID);
                            else
                                modules = modules.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                modules = modules.OrderByDescending(s => s.Name);
                            else
                                modules = modules.OrderBy(s => s.Name);
                            break;
                        case "SName":
                            if (sortOrder == "DESC")
                                modules = modules.OrderByDescending(s => s.SName);
                            else
                                modules = modules.OrderBy(s => s.SName);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                modules = modules.OrderByDescending(s => s.Code);
                            else
                                modules = modules.OrderBy(s => s.Code);
                            break;
                        case "MType":
                            if (sortOrder == "DESC")
                                modules = modules.OrderByDescending(s => s.MType);
                            else
                                modules = modules.OrderBy(s => s.MType);
                            break;
                        case "EC":
                            if (sortOrder == "DESC")
                                modules = modules.OrderByDescending(s => s.EC);
                            else
                                modules = modules.OrderBy(s => s.EC);
                            break;

                        default:
                            modules = modules.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(modules, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            FillModuleType();
            //FillCourses();
            FillSelectionType();
            ddlselectiontype.Enabled = false;
         
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                var MaxRevisionNo = (from p in context.Modules
                                     where p.CourseID == courseID
                                     select new { ValueField = p.RevisionNumber+1, TextField = p.RevisionNumber+1 }).Distinct().OrderByDescending(p => p.ValueField).Take(2);

                //ddlrevisionno.SelectedItem.Text = Convert.ToString(Convert.ToInt32(rv) + 1);
                if (MaxRevisionNo.Count()!=0)
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlrevisionno, MaxRevisionNo, null);
                }
                else
                {
                    ddlrevisionno.Items.Clear();
                    ddlrevisionno.Items.Insert(0, "1");
                }

                if (MaxRevisionNo.Count() == 1)
                {
                    ddlrevisionno.Items.Clear();
                    ddlrevisionno.Items.Insert(0, "1");
                }

                if (ddlrevisionno.Items.Count >= 1)
                    ddlrevisionno.SelectedIndex = 0;
            };
                btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Module Details";
            //Updating breadscrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Module", "#", ""));
        }
        else
        {
            Response.Redirect("admfrmodule.aspx?CourseId=" + Request.QueryString["CourseId"], true);
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
            Course cou;
            cou = context.Courses.Find(Convert.ToInt64(Request.QueryString["CourseID"]));
            Module objModule;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objModule = new EConnect.NIELIT.Module();
                objModule.CourseCategoryID = cou.CourseCategoryID;
                objModule.CourseID = cou.ID;
                objModule.ModuleTypeID = Convert.ToInt32(ddlmoduletype.SelectedValue);
                objModule.Name = txtname.Text.ToString().ToUpper();
                objModule.ShortName = txtsubname.Text.ToString().ToUpper();
                if (ddlProjectNo.Enabled == true)
                {
                    objModule.ProjectNumber = Convert.ToInt32(ddlProjectNo.SelectedValue);
                }
                if (ddlSynopsis.Enabled == true)
                {
                    objModule.SynopsisRequired = Convert.ToBoolean(ddlSynopsis.SelectedValue);
                }
                objModule.ModuleNUmber = Convert.ToInt32(txtmoduleno.Text);
                if (txtsubmoduleno.Enabled == true)
                {
                    objModule.ModuleSubNUmber = Convert.ToInt32(txtsubmoduleno.Text);
                }
                objModule.SelectionTypeID = Convert.ToInt32(ddlselectiontype.SelectedValue);
                if (txtElectiveGroup.Enabled == true)
                {
                    objModule.ElectiveGroup = Convert.ToInt32(txtElectiveGroup.Text);
                }
                objModule.Code =Convert.ToInt32(txtmodulecode.Text);
                objModule.EffectiveFromDate = Convert.ToDateTime(txteffectivefrom.Text);
                objModule.RevisionNumber = Convert.ToInt32(ddlrevisionno.SelectedValue);
                objModule.ModuleSubCode = Convert.ToInt32(txtSubCodeNumber.Text);
                objModule.ExamModeId = Convert.ToInt32(ddlExamMode.SelectedValue);
                if (txtallowedmodules.Text == "0")
                {
                    ShowAlert("Zero not allowed", true);
                    return;
                }
                if (txtallowedmodules.Enabled == true)
                {
                    objModule.NumberOfElectiveModulesAllowed = Convert.ToInt32(txtallowedmodules.Text);
                }
                context.Modules.Add(objModule);
                //context.Courses.Add(cou);
                context.SaveChanges();
                strMessage = "New record saved.";
            }
            else
            {
                objModule = context.Modules.Find(Convert.ToInt32(Request.QueryString["key"]));
                objModule.CourseCategoryID = cou.CourseCategoryID;
                objModule.CourseID = cou.ID;
                objModule.ModuleTypeID = Convert.ToInt32(ddlmoduletype.SelectedValue);
                objModule.Name = txtname.Text.ToString().ToUpper();
                objModule.ShortName = txtsubname.Text.ToString().ToUpper();
                if (ddlProjectNo.Enabled == true)
                {
                    objModule.ProjectNumber = Convert.ToInt32(ddlProjectNo.SelectedValue);
                }
                if (ddlSynopsis.Enabled == true)
                {
                    objModule.SynopsisRequired = Convert.ToBoolean(ddlSynopsis.SelectedValue);
                }
                objModule.ModuleNUmber = Convert.ToInt32(txtmoduleno.Text);
                if (txtsubmoduleno.Enabled == true)
                {
                    objModule.ModuleSubNUmber = Convert.ToInt32(txtsubmoduleno.Text);
                }
                objModule.SelectionTypeID = Convert.ToInt32(ddlselectiontype.SelectedValue);
                if (txtElectiveGroup.Enabled == true)
                {
                    objModule.ElectiveGroup = Convert.ToInt32(txtElectiveGroup.Text);
                }
                objModule.Code = Convert.ToInt32(txtmodulecode.Text);
                objModule.EffectiveFromDate = Convert.ToDateTime(txteffectivefrom.Text);
                objModule.ModuleSubCode = Convert.ToInt32(txtSubCodeNumber.Text);
                objModule.ExamModeId = Convert.ToInt32(ddlExamMode.SelectedValue);
                if (txtallowedmodules.Text == "0")
                {
                    ShowAlert("Zero not allowed", true);
                    return;
                }
                if (txtallowedmodules.Enabled == true)
                {
                    objModule.NumberOfElectiveModulesAllowed = Convert.ToInt32(txtallowedmodules.Text);
                }
                context.Entry(objModule).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record updated.";
               
            }
            Response.Redirect("admfrmodule.aspx?CourseId=" + Request.QueryString["CourseID"], true);
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
            ddlfiltermoduletype.SelectedValue = "0";
            ddlfilterselection.SelectedValue = "0";
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = hl.NavigateUrl;

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = hl.NavigateUrl;

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = hl.NavigateUrl;
               
                
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
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
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var centre = (from s in context.Modules
                         select new { Name = s.Name }).Distinct();
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.Name);
            foreach (var course in centre)
            {
                items.Add(course.Name);
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
        //BreadCrumb1.Render();
         
        Response.Redirect( "admfrmodule.aspx?CourseId=" + Request.QueryString["CourseID"] , true);
    }
    protected void ddlselectiontype_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtElectiveGroup.Text = "";
        txtsubmoduleno.Text = "";
        txtallowedmodules.Text = "";
        txtElectiveGroup.Enabled = false;
        txtsubmoduleno.Enabled = false;
        txtallowedmodules.Enabled = false;
        if(ddlselectiontype.SelectedItem.Text==Convert.ToString(enmSelectionType.Elective))
        {
            //txtsubmoduleno.Text = "";
            txtElectiveGroup.Enabled = true;
            txtsubmoduleno.Enabled = true;
            txtallowedmodules.Enabled = true;
            int revisionNumber = Convert.ToInt32(ddlrevisionno.SelectedValue);
            txtElectiveGroup.Text = txtmoduleno.Text;
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 SelectionTypeId = Convert.ToInt32(ddlselectiontype.SelectedValue);
            Int32 ElectiveGroupID = Convert.ToInt32(txtElectiveGroup.Text);
            Int32 ModuleNumber = Convert.ToInt32(txtmoduleno.Text);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 moduleSubNo = (from c in context.Modules
                                  where c.CourseID == CourseId && c.SelectionTypeID == SelectionTypeId && c.ElectiveGroup == ElectiveGroupID && c.ModuleNUmber == ModuleNumber && c.RevisionNumber == revisionNumber
                                  select c).Count();
                txtsubmoduleno.Text = Convert.ToString(Convert.ToInt32(moduleSubNo+1) );

                txtSubCodeNumber.Text = (revisionNumber +1).ToString() + txtmoduleno.Text + txtsubmoduleno.Text;
                txtallowedmodules.Text = "1";
            };

        }
    }
    protected void txtprefix_TextChanged(object sender, EventArgs e)
    {
        //AddModuleNo();
    }
    protected void ddlrevisionno_TextChanged(object sender, EventArgs e)
    {
        //AddModuleNo();
    }
    protected void ddlmoduletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 ModuleTypeID=Convert.ToInt32(enmModuleType.Project);
        ddlProjectNo.SelectedIndex = 0;
        ddlSynopsis.SelectedIndex = 0;
        ddlProjectNo.Enabled = false;
        ddlSynopsis.Enabled = false;
        if (Convert.ToInt32(ddlmoduletype.SelectedValue) == ModuleTypeID)
        { 
            ddlProjectNo.Enabled = true;
            ddlSynopsis.Enabled = true;
        }
    }
    protected void txtElectiveGroup_TextChanged(object sender, EventArgs e)
    {
        //Int32 CourseId=Convert.ToInt32(Request.QueryString["CourseID"]);
        //Int32 SelectionTypeId=Convert.ToInt32(ddlselectiontype.SelectedValue);
        //Int32 ElectiveGroupID=Convert.ToInt32(txtElectiveGroup.Text);
        //Int32 ModuleNumber=Convert.ToInt32(txtmoduleno.Text);
        //using (EConnectContext context = new EConnectContext())
        //{
        //    var moduleSubNo = from c in context.Modules
        //                      where c.CourseID == CourseId && c.SelectionTypeID == SelectionTypeId && c.ElectiveGroup == ElectiveGroupID && c.ModuleNUmber == ModuleNumber
        //                      select new { c.ModuleSubNUmber };
        //};
    }
    protected void btnPrList_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 CourseID= Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 PracticalModleID = Convert.ToInt32(Request.QueryString["Key"]);
            using (EConnectContext context = new EConnectContext())
            {
               string x="Delete from Module_Practical_Eligibility Where Practical_Module_ID =" + PracticalModleID;
                context.Database.ExecuteSqlCommand(x);
                
                ModulePracticalEligibility objprModule;
                Module objModule=new Module();
                foreach (ListItem l in chkModulelist.Items)
                {
                    if (l.Selected)
                    {
                        Int32 moduleID = Convert.ToInt32(l.Value);
                        objprModule = new EConnect.NIELIT.ModulePracticalEligibility();
                        objprModule.PracticalModuleID = PracticalModleID;
                        objprModule.TheoryModuleID =moduleID;
                        
                        objModule = context.Modules.Find(moduleID);
                        objprModule.SelectionTypeID =objModule.SelectionTypeID;
                       // objprModule.ElectiveGroup = objModule.ElectiveGroup;
                        objprModule.CreatedBy = Convert.ToInt16(Session["UserID"]);
                        objprModule.CreatedOn = Convert.ToDateTime(DateTime.Now.ToString());
                        context.ModulePracticalEligibilities.Add(objprModule);
                    }
                }
                foreach(TreeNode n in trvElective.Nodes)
                {
                    Module prModule = context.Modules.Find(PracticalModleID);
                    if (n.Checked)
                    {
                        Int32 groupID = Convert.ToInt32(n.Value);
                        var ModuleListElective = from s in context.Modules
                                                 where s.CourseID == CourseID && s.RevisionNumber == prModule.RevisionNumber && s.ElectiveGroup == groupID
                                                 orderby (s.Code)
                                                 select s;
                        foreach (Module module in ModuleListElective)
                        {
                            
                            objprModule = new EConnect.NIELIT.ModulePracticalEligibility();
                            objprModule.PracticalModuleID = PracticalModleID;
                            objprModule.TheoryModuleID = module.ID;

                            
                            objprModule.SelectionTypeID = module.SelectionTypeID;
                            objprModule.ElectiveGroup = module.ElectiveGroup;
                            objprModule.CreatedBy = Convert.ToInt16(Session["UserID"]);
                            objprModule.CreatedOn = Convert.ToDateTime(DateTime.Now.ToString());
                            context.ModulePracticalEligibilities.Add(objprModule);
                        }
                    }
                }
  
                context.SaveChanges();
            };
            ShowAlert("List updated", true);
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}