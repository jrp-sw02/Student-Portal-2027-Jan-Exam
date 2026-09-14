using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Data.Objects;
using System.Text.RegularExpressions;
using EConnect;
using System.Data.OleDb;
using System.Net;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Admin_NielitCentreStudentUpload : BasePage
{
    int stateid = 0;
    String strMessage = string.Empty, XlsmfileName = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    Int32 lnkID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    Int64 NielitCentrelinkedToCentreId = 0;

    StringBuilder appIdList4 = new StringBuilder();
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
            entityID = Convert.ToInt32(Session["EntityID"]);
            UserTypeid = Convert.ToInt32(Session["UserType"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            User objUser1;
            using (EConnectContext context = new EConnectContext())
            { 
                objUser1 = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);
            }
            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {
                        if (UserTypeid == 10)
                        {

                            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                            var Center = from t in context1.NielitCentres
                                         where t.ID == UserRefNumber
                                         orderby (t.Name)
                                         select new { ValueField = t.ID, TextField = t.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);

                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (lnkID != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                //        txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                // ddlSubcentreName.Enabled = false;
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                //      txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                            }
                        }
                        else if (UserTypeid == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                            if (institutesName != null)
                            {
                                // txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                            ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                        }
                        else if (UserTypeid == 4)
                        {
                            var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          where s.instituteID == loginUser.UserRefNumber
                                                          select new { ID = s.ID, linkedToCentre = s.linkedToCentre };
                            if (intituteslinkedToCentre.Count() == 0)
                            {
                                ShowAlert("Menu is not available for the institute");
                                return;
                            }
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                            //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            // NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                            if (institutesName != null)
                            {
                                // txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                //hcentreID.Value = NelitCentreLinkId.ToString();
                                //Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);


                        }
                    }

                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        NewEntryPage();
                    }
                    else
                    {
                        NewEntryPage();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";

                        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student Upload", "Admin/NielitCentreStudentUpload.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student Upload", "Admin/NielitCentreStudentUpload.aspx", ""));
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

    protected void FillddlcentreName()
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "99");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);


                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                    // txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    ddlCenter.ClearSelection();
                    if (UserTypeid == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.instituteID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCourse.ClearSelection();
            ddlBatch.ClearSelection();
            ddlCourse.Items.Clear();
            ddlBatch.Items.Clear();
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (lnkID != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                        //  txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeid == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    //ddlCenter.Enabled = false;
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    ddlCenter.Enabled = true;
                                }
                                if (UserTypeid == 4)//AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    //ddlCenter.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = Afflcentre.instituteID.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    ddlCenter.Enabled = true;
                                }
                            }
                            else
                            {
                                ddlCenter.Items.Add(new System.Web.UI.WebControls.ListItem("--Select One--", "99"));
                                ddlCenter.SelectedValue = "99";
                                //ddlCenter.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        //    txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                ddlCenter.ClearSelection();
                                ddlCenter.Items.Clear();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                ddlCenter.Enabled = true;
                            }

                            else
                            {
                                ddlCenter.ClearSelection();

                                var Center = from t in context.NielitCentres
                                             where t.ID == UserRefNumber
                                             orderby (t.Name)
                                             select new { ValueField = t.ID, TextField = t.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst1);

                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);

                                //ddlCenter.Items.Add(new ListItem("--Select One--", "99"));
                                //ddlCenter.SelectedValue = "99";
                                //ddlCenter.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }

    protected void FillCourses(DropDownList ddl, Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddl.Items.Clear();
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
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
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            // BindEditNewModeData();
            //ddlExamCentreType.SelectedValue = "3";
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Nielit Centre Student";

            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Nielit Centre Student Upload", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreStudent.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCentreStudent.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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
            ddlName.SelectedValue = "0";
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
                NielitCentreStudent CenterStudent = context.NielitCentreStudent.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.NielitCentreStudent.Remove(CenterStudent);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            }
            ;
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //string href = hl.NavigateUrl;
                //if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                //{
                //    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                //}
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                //HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //h2.NavigateUrl = hl.NavigateUrl;
                //HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //h3.NavigateUrl = hl.NavigateUrl;
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = hl.NavigateUrl;

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

        User objUser;
        int loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        using (EConnectContext context1 = new EConnectContext())
        {
            objUser = new EConnect.URM.User();


            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            int UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

            NIELITMISContext context = new NIELITMISContext();
            try
            {
                if (count <= 0)
                    count = 10;
                List<String> items = new List<String>();
                string searchString = prefixText.Trim().ToUpper();

                var Examcentre = from s in context.NielitCentreBatchs
                                 where s.centreID == UserRefNumber
                                 select new { Name = s.Name };


                if (!String.IsNullOrEmpty(searchString))
                {
                    Examcentre = Examcentre.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                Examcentre = Examcentre.OrderBy(s => s.Name);

                foreach (var c in Examcentre)
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
    }



    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divValidateData.Visible = false;
            btnCancel.Visible = false;
            btnUpload.Visible = true;
            btnback.Visible = true;
            r1.Visible = true;
            r2.Visible = true;
            r3.Visible = true;
            r4.Visible = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
        Int64 NelitCentreLinkId = 0;
        Int64 NielitCentrelinkedToCentreId = 0;
        Int64 subcentreid = 0;
        string Seleted = "";
        User objUser;

        ddlBatch.ClearSelection();
        ddlCourse.ClearSelection();

        ddlBatch.Items.Clear();
        ddlCourse.Items.Clear();
        try
        {
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Seleted = RdoAffInstOrNonAffInst.SelectedValue;
                    subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }

                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    }

                    if (Seleted == "2")
                        BindCourses(Convert.ToInt32(NielitCentreId), Convert.ToInt16(Seleted));
                    else
                        BindCourses(Convert.ToInt32(subcentreid), Convert.ToInt16(Seleted));

                    ddlCourse.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error:" + ex.Message);
        }

    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 subcentreid = 0;
        Int64 Courseid = 0;

        ddlBatch.ClearSelection();

        ddlBatch.Items.Clear();

        try
        {
            subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        //  bindProject();
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid //&& (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        //bindProject();

                    }
                }
                else
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid
                                    //(s.startDate <= System.DateTime.Now && 
                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        //bindProject();
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid
                                    //(s.startDate <= System.DateTime.Now && 

                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        //bindProject();

                    }
                }
            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindBatch()
    {
        try
        {
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();

                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                            && p.subCentreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();

                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        }
                    }
                    else if (UserTypeid == 11)
                    {
                        var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);
                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {

                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        // FillddlSubcentreName();
                        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                    }
                    else if (UserTypeid == 4)
                    {
                        var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);

                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {

                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        // FillddlSubcentreName();
                        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindCourses(Int32 CentreId, int CentreType)
    {
        // 2- Centre, 0--Non Aff Instt 1--Aff Instt

        using (NIELITMISContext context = new NIELITMISContext())
        {
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            using (DataTable dt = GetBatchCourseRecord(CentreId, CentreType))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
                }
            }
        }
    }
    protected DataTable GetBatchCourseRecord(Int32 CentreId, int CentreType)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt).Value = CentreId;
                    cmd.Parameters.Add("@pCentreType", SqlDbType.Int).Value = CentreType;
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
    public DataTable FillGridViewNIELITCentreStudentUploadRemarks()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                Int64 ent = Convert.ToInt64(Session["EntityID"]);
                using (SqlCommand cmd = new SqlCommand("TempData_NielitCentreStudentUploadRemarksView", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@centreID", SqlDbType.BigInt);
                    cmd.Parameters["@centreID"].Value = Convert.ToInt64(Session["EntityID"]);
                    cmd.Parameters.Add("@view1", SqlDbType.Int);
                    cmd.Parameters["@view1"].Value = 11;
                    cmd.Parameters.Add("@ExcelFileName", SqlDbType.VarChar, 150);
                    cmd.Parameters["@ExcelFileName"].Value = hfFileName.Value;// XlsmfileName;
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
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            using (DataTable dt = FillGridViewNIELITCentreStudentUploadRemarks())
            {
                if (dt.Rows.Count > 0)
                {
                    var UplaoadRemarks = (from p in dt.AsEnumerable()
                                          select new
                                          {
                                              ID = p.Field<string>("SlNo"),
                                              Name = p.Field<string>("Name"),
                                              Remarks = p.Field<string>("Remarks"),
                                          });

                    PagingBar1.Bind(UplaoadRemarks, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    // gvMain.Columns[5].Visible = false;
                    //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                    //{
                    //    gvMain.Columns[7].Visible = false;
                    //}
                }
            }


        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally
        {
            con.Close();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (ddlCenter.SelectedIndex == 0)
        {
            ShowAlert("Please select relevant Center");
            return;
        }
        if (ddlCourse.SelectedIndex == 0)
        {
            ShowAlert("Please select relevant Course");
            return;
        }
        if (ddlBatch.SelectedIndex == 0)
        {
            ShowAlert("Please select relevant Batch");
            return;
        }
        if (!fileUpload.HasFile)
        {
            ShowAlert("Please select file to upload.");
            return;
        }
        string filepath = Server.MapPath("../");
        fileUpload.SaveAs(filepath + System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fileUpload.FileName));
        string path = filepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fileUpload.FileName);
        string fname = System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fileUpload.FileName);
        hfFileName.Value = System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName);
        XlsmfileName = System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName);
        string ext = System.IO.Path.GetExtension(this.fileUpload.PostedFile.FileName);
        string excelConnectionString = "";
        if (ext.ToUpper() == ".XLSM")
            excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", path);
        else
        {
            ShowAlert("Please Choose .XLM Extension File", true);
            return;
        }
        OleDbConnection connection = new OleDbConnection();
        connection.ConnectionString = excelConnectionString;
        connection.Open();
        OleDbCommand command = new OleDbCommand("select * from [StuData$]", connection);
        OleDbDataReader dr = command.ExecuteReader();

        try
        {
            string ErrorMessageP = string.Empty;
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            StringBuilder FaildRecords = new StringBuilder();
            FaildRecords.Append("Failed IDs:-");
            int failedRecordCount = 0;
            int ValidateRecords = 0;
            int TotalRecords = 0;
            int userType = Convert.ToInt32(EConnect.URM.UserType.Institute);
            string ErrorMessage = string.Empty;
            string slnos = "0";

            while (dr.Read())
            {
                //temp
                // update remarks
                string consStringg = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                using (SqlConnection connn = new SqlConnection(consStringg))
                {
                    string query = "update [NIELITMIS].[dbo].[TempData_NielitCentreStudentUpload] set Remarks = " + "'" + ErrorMessageP + "' where SlNo=" + "'" + slnos + "'";
                    SqlCommand cmddd = new SqlCommand(query, connn);
                    cmddd.CommandType = CommandType.Text;
                    cmddd.Connection = connn;
                    connn.Open();
                    cmddd.ExecuteNonQuery();
                    connn.Close();
                }
                // update remarks end

                //Header row
                if (dr[0].ToString() == "Sr. No.")
                {
                    continue;
                }
                //Header row

                if (dr[0].ToString() == "")
                {
                    continue;
                }

                if (dr[5].ToString() == "")
                {
                    break;
                }
                string CandidateNames = dr[5].ToString();
                if (CandidateNames != null && CandidateNames != "Name")
                {
                    string consString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(consString))
                    {
                        using (SqlCommand cmdd = new SqlCommand("Insert_TempData_NielitCentreStudentUpload", conn))
                        {
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.Connection = conn;
                            string a = dr[0].ToString();
                            cmdd.Parameters.AddWithValue("@SLN", dr[0].ToString());
                            cmdd.Parameters.AddWithValue("@AlreadyRegistered", dr[1].ToString());
                            cmdd.Parameters.AddWithValue("@RegisteredCourse", dr[2].ToString());
                            cmdd.Parameters.AddWithValue("@RegisteredCourseRegistrationNo", dr[3].ToString());
                            cmdd.Parameters.AddWithValue("@Salutaion", dr[4].ToString());
                            cmdd.Parameters.AddWithValue("@Name", dr[5].ToString());
                            cmdd.Parameters.AddWithValue("@FatherName", dr[6].ToString());
                            cmdd.Parameters.AddWithValue("@MotherName", dr[7].ToString());
                            cmdd.Parameters.AddWithValue("@GuardianName", dr[8].ToString());
                            cmdd.Parameters.AddWithValue("@Gender", dr[9].ToString());
                            cmdd.Parameters.AddWithValue("@MaritalStatus", dr[10].ToString());
                            cmdd.Parameters.AddWithValue("@Dob", dr[11].ToString());
                            cmdd.Parameters.AddWithValue("@Castcategory", dr[12].ToString());
                            cmdd.Parameters.AddWithValue("@Religion", dr[13].ToString());
                            cmdd.Parameters.AddWithValue("@Handicapped", dr[14].ToString());
                            cmdd.Parameters.AddWithValue("@ExServiceMan", dr[15].ToString());
                            cmdd.Parameters.AddWithValue("@BodyMark", dr[16].ToString());
                            cmdd.Parameters.AddWithValue("@Mobile", dr[17].ToString());
                            cmdd.Parameters.AddWithValue("@Std", dr[18].ToString());
                            cmdd.Parameters.AddWithValue("@Phone", dr[19].ToString());
                            cmdd.Parameters.AddWithValue("@Email", dr[20].ToString());
                            cmdd.Parameters.AddWithValue("@CorAddress1", dr[21].ToString());
                            cmdd.Parameters.AddWithValue("@CorAddress2", dr[22].ToString());
                            cmdd.Parameters.AddWithValue("@CorAddress3", dr[23].ToString());
                            cmdd.Parameters.AddWithValue("@CorState", dr[24].ToString());
                            cmdd.Parameters.AddWithValue("@CorDistrict", dr[25].ToString());
                            cmdd.Parameters.AddWithValue("@CorCityName", dr[26].ToString());
                            cmdd.Parameters.AddWithValue("@CorPinCode", dr[27].ToString());
                            cmdd.Parameters.AddWithValue("@PerAddress1", dr[28].ToString());
                            cmdd.Parameters.AddWithValue("@PerAddress2", dr[29].ToString());
                            cmdd.Parameters.AddWithValue("@PerAddress3", dr[30].ToString());
                            cmdd.Parameters.AddWithValue("@PerState", dr[31].ToString());
                            cmdd.Parameters.AddWithValue("@PerDistrict", dr[32].ToString());
                            cmdd.Parameters.AddWithValue("@PerCityName", dr[33].ToString());
                            cmdd.Parameters.AddWithValue("@PerPinCode", dr[34].ToString());
                            cmdd.Parameters.AddWithValue("@DocumentType", dr[35].ToString());
                            cmdd.Parameters.AddWithValue("@DocumentNumber", dr[36].ToString());
                            cmdd.Parameters.AddWithValue("@whetherProjectStudent", dr[37].ToString());
                            cmdd.Parameters.AddWithValue("@projectName", dr[38].ToString());
                            cmdd.Parameters.AddWithValue("@EWS", dr[39].ToString());
                            cmdd.Parameters.AddWithValue("@ExcelFileName", System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName));
                            cmdd.Parameters.AddWithValue("@centreID", ddlCenter.SelectedValue.ToString());

                            conn.Open();
                            cmdd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                //temp

                string cons2 = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                using (SqlConnection connections = new SqlConnection(cons2))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertRecord_NielitCentreStudentUpload", connections))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = connections;
                        if (ddlCenter.SelectedValue != "0")
                        {
                            cmd.Parameters.Add(new SqlParameter("@centreID", SqlDbType.BigInt));
                            cmd.Parameters["@centreID"].Value = Convert.ToInt64(ddlCenter.SelectedValue);
                        }
                        else
                        {
                            throw new Exception("Centre Name required!!");
                        }

                        try
                        {
                            int fieldNo = 0;
                            //Header row
                            if (dr[0].ToString() == "Sr. No.")
                            {
                                continue;
                            }
                            //Header row

                            if (dr[0].ToString() == "")
                            {
                                continue;
                            }

                            if (dr[5].ToString() == "")
                            {
                                break;
                            }

                            string CandidateName = dr[5].ToString();
                            if (CandidateName != null && CandidateName != "Name")
                            {
                                TotalRecords = TotalRecords + 1; // total record

                                fieldNo = 1;
                                if (dr[1].ToString() == "Y")
                                {
                                    cmd.Parameters.Add(new SqlParameter("@Already_Registered", SqlDbType.Int));
                                    cmd.Parameters["@Already_Registered"].Value = 1;

                                }

                                else if (dr[1].ToString() == "N")
                                {
                                    cmd.Parameters.Add(new SqlParameter("@Already_Registered", SqlDbType.Int));
                                    cmd.Parameters["@Already_Registered"].Value = 0;
                                }
                                else
                                {
                                    ErrorMessageP = "Already Registered (Y/N) required!!";
                                    throw new Exception("Already Registered (Y/N) required!!");
                                }
                                fieldNo = 2;
                                if (dr[2].ToString() != "")
                                {

                                    //if (System.Text.RegularExpressions.Regex.IsMatch(dr[8].ToString(), "^[a-zA-Z\u00FC\u00DC ]*$"))
                                    //{
                                    //cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                    //cmd.Parameters["@Registered_Course_ID"].Value = Convert.ToInt32(dr[2].ToString());
                                    //cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                    //cmd.Parameters["@Registered_Course_ID"].Value = 1; // for asked to be changes in excel file when file this.
                                    //}
                                    if (dr[2].ToString() != "O Level")
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                        cmd.Parameters["@Registered_Course_ID"].Value = 1;
                                    }
                                    if (dr[2].ToString() != "A Level")
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                        cmd.Parameters["@Registered_Course_ID"].Value = 2;
                                    }
                                    if (dr[2].ToString() != "B Level")
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                        cmd.Parameters["@Registered_Course_ID"].Value = 3;
                                    }
                                    if (dr[2].ToString() != "C Level")
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                        cmd.Parameters["@Registered_Course_ID"].Value = 4;
                                    }
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@Registered_Course_ID", SqlDbType.Int));
                                    cmd.Parameters["@Registered_Course_ID"].Value = DBNull.Value;
                                }

                                fieldNo = 3;
                                if (dr[3].ToString() != "" && dr[2].ToString() != "")
                                {
                                    if (isNumber(dr[3].ToString().Trim()) && dr[3].ToString().Length <= 9)
                                    {
                                        cmd.Parameters.Add("@Registered_Course_Registration_No", SqlDbType.Int);
                                        cmd.Parameters["@Registered_Course_Registration_No"].Value = Convert.ToInt32(dr[3].ToString());
                                    }
                                    else
                                    {
                                        ErrorMessageP = "RegisteredCourseRegistrationNo Not Valid !!";
                                        throw new Exception("RegisteredCourseRegistrationNo Not Valid !!");
                                    }
                                }
                                else
                                {
                                    cmd.Parameters.Add("@Registered_Course_Registration_No", SqlDbType.Int);
                                    cmd.Parameters["@Registered_Course_Registration_No"].Value = DBNull.Value;
                                }


                                fieldNo = 4;
                                // Personal Details of candidates start
                                if (dr[4].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Salutaion", SqlDbType.VarChar, 10);
                                    cmd.Parameters["@Salutaion"].Value = dr[4].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "Salutation Not Selected!";
                                    throw new Exception("Salutation required!!");
                                }
                                fieldNo = 5;
                                if (dr[5].ToString() != "" && dr[5].ToString().Length <= 60 && System.Text.RegularExpressions.Regex.IsMatch(dr[5].ToString(), "^[a-zA-Z\u00FC\u00DC ]*$"))
                                {
                                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 60);
                                    cmd.Parameters["@Name"].Value = dr[5].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "Name Invalid!";
                                    throw new Exception("Name required!!");
                                }
                                fieldNo = 6;
                                if (dr[6].ToString() != "" && dr[7].ToString() != "" && dr[8].ToString() == "")
                                {
                                    if (dr[6].ToString().Length <= 60 && System.Text.RegularExpressions.Regex.IsMatch(dr[6].ToString(), "^[a-zA-Z\u00FC\u00DC ]*$"))
                                    {
                                        cmd.Parameters.Add("@Father_Name", SqlDbType.VarChar, 60);
                                        cmd.Parameters["@Father_Name"].Value = dr[6].ToString();
                                    }
                                    else
                                    {
                                        ErrorMessageP = "FatherName Invalid!";
                                        throw new Exception("FatherName required!!");
                                    }
                                    if (dr[7].ToString().Length <= 60 && System.Text.RegularExpressions.Regex.IsMatch(dr[7].ToString(), "^[a-zA-Z\u00FC\u00DC ]*$"))
                                    {
                                        cmd.Parameters.Add("@Mother_Name", SqlDbType.VarChar, 60);
                                        cmd.Parameters["@Mother_Name"].Value = dr[7].ToString();

                                        cmd.Parameters.Add("@Guardian_Name", SqlDbType.VarChar, 60);
                                        cmd.Parameters["@Guardian_Name"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        ErrorMessageP = "MotherName Invalid!";
                                        throw new Exception("MotherName required!!");
                                    }
                                }
                                else if (dr[8].ToString() != "")
                                {
                                    if (dr[8].ToString().Length <= 60 && System.Text.RegularExpressions.Regex.IsMatch(dr[8].ToString(), "^[a-zA-Z\u00FC\u00DC ]*$"))
                                    {
                                        cmd.Parameters.Add("@Guardian_Name", SqlDbType.VarChar, 60);
                                        cmd.Parameters["@Guardian_Name"].Value = dr[8].ToString();
                                        cmd.Parameters.Add("@Father_Name", SqlDbType.VarChar, 60);
                                        cmd.Parameters["@Father_Name"].Value = DBNull.Value;
                                        cmd.Parameters.Add("@Mother_Name", SqlDbType.VarChar, 60);
                                        cmd.Parameters["@Mother_Name"].Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        ErrorMessageP = "GuardianName Invalid!";
                                        throw new Exception("GuardianName required!!");
                                    }
                                }
                                else
                                {
                                    ErrorMessageP = "Father Name and Mother Name OR GuardianName required!!";
                                    throw new Exception("Father Name and Mother Name OR GuardianName required!!");
                                }
                                fieldNo = 9;
                                if (dr[9].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 6);
                                    cmd.Parameters["@Gender"].Value = dr[9].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "Gender Not Selected!";
                                    throw new Exception("Gender required!!");
                                }
                                fieldNo = 10;
                                if (dr[10].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Marital_Status] WHERE Name= '" + dr[10].ToString() + "'";
                                    string MartialStatusID = GetDataByValue(getValue);

                                    cmd.Parameters.Add("@Marital_Status_ID", SqlDbType.Int);
                                    cmd.Parameters["@Marital_Status_ID"].Value = Convert.ToInt32(MartialStatusID);
                                }
                                else
                                {
                                    ErrorMessageP = "Marital Status Not Selected!";
                                    throw new Exception("Marital Status required!!");
                                }
                                fieldNo = 11;
                                if (dr[11].ToString() != "")
                                {
                                    if (isValidDob(dr[11].ToString().Trim()))
                                    {
                                        cmd.Parameters.Add("@Dob", SqlDbType.Date);
                                        cmd.Parameters["@Dob"].Value = Convert.ToDateTime(dr[11].ToString());
                                    }
                                    else
                                    {
                                        ErrorMessageP = "DateOfBirth Invalid!!";
                                        throw new Exception("DateOfBirth required!!");
                                    }
                                }
                                else
                                {
                                    ErrorMessageP = "DateOfBirth required!!";
                                    throw new Exception("DateOfBirth required!!");
                                }
                                fieldNo = 12;
                                if (dr[12].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Cast_Category] WHERE Name= '" + dr[12].ToString() + "'";
                                    string CastCategoryID = GetDataByValue(getValue);

                                    cmd.Parameters.Add("@Cast_Category_ID", SqlDbType.Int);
                                    cmd.Parameters["@Cast_Category_ID"].Value = Convert.ToInt32(CastCategoryID);
                                }
                                else
                                {
                                    ErrorMessageP = "Cast Category Not Selected!";
                                    throw new Exception("Cast Category required!!");
                                }
                                fieldNo = 13;
                                if (dr[13].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Religion] WHERE Name= '" + dr[13].ToString() + "'";
                                    string ReligionID = GetDataByValue(getValue);

                                    cmd.Parameters.Add("@Religion_ID", SqlDbType.Int);
                                    cmd.Parameters["@Religion_ID"].Value = Convert.ToInt32(ReligionID);

                                }
                                else
                                {
                                    ErrorMessageP = "Religion Not Selected!";
                                    throw new Exception("Religion required!!");
                                }
                                fieldNo = 14;
                                if (dr[14].ToString() == "Y")
                                {
                                    cmd.Parameters.Add("@Is_Handicaped", SqlDbType.Int);
                                    cmd.Parameters["@Is_Handicaped"].Value = 1;
                                }

                                else if (dr[14].ToString() == "N")
                                {
                                    cmd.Parameters.Add("@Is_Handicaped", SqlDbType.Int);
                                    cmd.Parameters["@Is_Handicaped"].Value = 0;
                                }
                                else
                                {
                                    ErrorMessageP = "IsHandicaped (Y/N) Not Selected!";
                                    throw new Exception("IsHandicaped (Y/N) required!!");
                                }
                                fieldNo = 15;
                                if (dr[15].ToString() == "Y")
                                {
                                    cmd.Parameters.Add("@Is_Ex_Servicemane", SqlDbType.Int);
                                    cmd.Parameters["@Is_Ex_Servicemane"].Value = 1;
                                }

                                else if (dr[15].ToString() == "N")
                                {
                                    cmd.Parameters.Add("@Is_Ex_Servicemane", SqlDbType.Int);
                                    cmd.Parameters["@Is_Ex_Servicemane"].Value = 0;
                                }
                                else
                                {
                                    ErrorMessageP = "IsExServicemane (Y/N) Not Selected!";
                                    throw new Exception("IsExServicemane (Y/N) required!!");
                                }

                                if (dr[16].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Body_Mark", SqlDbType.VarChar, 50);
                                    cmd.Parameters["@Body_Mark"].Value = dr[16].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@Body_Mark", SqlDbType.VarChar, 50);
                                    cmd.Parameters["@Body_Mark"].Value = DBNull.Value;
                                }

                                if (dr[39].ToString() == "Y")
                                {
                                    cmd.Parameters.Add("@Is_EWS", SqlDbType.Int);
                                    cmd.Parameters["@Is_EWS"].Value = 1;
                                }

                                else if (dr[39].ToString() == "N")
                                {
                                    cmd.Parameters.Add("@Is_EWS", SqlDbType.Int);
                                    cmd.Parameters["@Is_EWS"].Value = 0;
                                }
                                else
                                {
                                    ErrorMessageP = "Is_EWS (Y/N) Not Selected!";
                                    throw new Exception("Is_EWS (Y/N) required!!");
                                }

                                // Personal Details of candidates End



                                // Contact Details of candidates start
                                if (dr[17].ToString() != "")
                                {
                                    string mob = dr[17].ToString();
                                    if (dr[17].ToString().Length == 10 && isNumber(dr[17].ToString().Trim()))
                                    {
                                        cmd.Parameters.Add("@Mobile", SqlDbType.BigInt);
                                        cmd.Parameters["@Mobile"].Value = Convert.ToInt64(dr[17].ToString());
                                    }
                                    else
                                    {
                                        ErrorMessageP = "MobileNumber Not Valid!";
                                        throw new Exception("MobileNumber Not Valid!!");
                                    }
                                }
                                else
                                {
                                    ErrorMessageP = "MobileNumber required!!";
                                    throw new Exception("MobileNumber required!!");
                                }

                                if (dr[18].ToString() != "")
                                {
                                    if (dr[19].ToString() != "")
                                    {
                                        if (isNumber(dr[18].ToString().Trim()))
                                        {
                                            cmd.Parameters.Add("@Std", SqlDbType.Int);
                                            cmd.Parameters["@Std"].Value = Convert.ToInt32(dr[18].ToString());
                                        }
                                        else
                                        {
                                            ErrorMessageP = "StdNumber Not Valid!";
                                            throw new Exception("StdNumber Not Valid!!");
                                        }
                                        if (isNumber(dr[19].ToString().Trim()))
                                        {
                                            cmd.Parameters.Add("@Phone", SqlDbType.Int);
                                            cmd.Parameters["@Phone"].Value = Convert.ToInt32(dr[19].ToString());
                                        }
                                        else
                                        {
                                            ErrorMessageP = "PhoneNumber Not Valid!";
                                            throw new Exception("PhoneNumber Not Valid!!");
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessageP = "STD and Phone Number required!!";
                                        throw new Exception("STD and Phone Number required!!");
                                    }
                                }
                                else
                                {
                                    cmd.Parameters.Add("@Std", SqlDbType.Int);
                                    cmd.Parameters["@Std"].Value = DBNull.Value;
                                    cmd.Parameters.Add("@Phone", SqlDbType.Int);
                                    cmd.Parameters["@Phone"].Value = DBNull.Value;
                                }

                                if (dr[20].ToString() != "")
                                {
                                    if (Regex.IsMatch(dr[20].ToString().Trim(), @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z"))
                                    {
                                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100);
                                        cmd.Parameters["@Email"].Value = dr[20].ToString();
                                    }
                                    else
                                    {
                                        ErrorMessageP = "EmailAddress Not Valid!";
                                        throw new Exception("EmailAddress Not Valid!!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("EmailAddress required!!");
                                }
                                // Contact Details of candidates End


                                // Correspondenc Address start
                                if (dr[21].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Cor_Address1", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Cor_Address1"].Value = dr[21].ToString();
                                }
                                else
                                {
                                    throw new Exception("CorAddressLine1 required!!");
                                }

                                if (dr[22].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Cor_Address2", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Cor_Address2"].Value = dr[22].ToString();
                                }
                                else
                                {
                                    throw new Exception("CorAddressLine2 required!!");
                                }

                                if (dr[23].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Cor_Address3", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Cor_Address3"].Value = dr[23].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@Cor_Address3", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Cor_Address3"].Value = DBNull.Value;
                                }

                                Int64 corstateid = 0;
                                if (dr[24].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Location] WHERE Parent_ID=1 and  Name= '" + dr[24].ToString() + "'";
                                    string CorStateID = GetDataByValue(getValue);
                                    corstateid = Convert.ToInt64(CorStateID);

                                    cmd.Parameters.Add("@Cor_State_ID", SqlDbType.BigInt);
                                    cmd.Parameters["@Cor_State_ID"].Value = Convert.ToInt64(CorStateID);
                                }
                                else
                                {
                                    ErrorMessageP = "CorState Invalid!";
                                    throw new Exception("CorState required!!");
                                }

                                if (dr[25].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Location] WHERE Parent_ID=" + corstateid + " and Name= '" + dr[25].ToString() + "'";
                                    string CorDistrictID = GetDataByValue(getValue);

                                    cmd.Parameters.Add("@Cor_District_ID", SqlDbType.BigInt);
                                    cmd.Parameters["@Cor_District_ID"].Value = Convert.ToInt64(CorDistrictID);
                                }
                                else
                                {
                                    ErrorMessageP = "CorDistrict Invalid!";
                                    throw new Exception("CorDistrict required!!");
                                }

                                if (dr[26].ToString() != "" && dr[26].ToString().Length <= 100)
                                {
                                    cmd.Parameters.Add("@Cor_City_Name", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Cor_City_Name"].Value = dr[26].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "CorCityName required!!";
                                    throw new Exception("CorCityName required!!");
                                }

                                if (dr[27].ToString() != "" && dr[27].ToString().Length == 6 && isNumber(dr[27].ToString().Trim()))
                                {
                                    cmd.Parameters.Add("@Cor_Pin_Code", SqlDbType.Int);
                                    cmd.Parameters["@Cor_Pin_Code"].Value = Convert.ToInt32(dr[27].ToString());
                                }
                                else
                                {
                                    ErrorMessageP = "CorPinCode Invalid!";
                                    throw new Exception("CorPinCode required!!");
                                }
                                // Correspondenc Address End



                                // Permanent Address start
                                if (dr[28].ToString() != "" && dr[28].ToString().Length <= 100)
                                {
                                    cmd.Parameters.Add("@Per_Address1", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Per_Address1"].Value = dr[28].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "PerAddressLine1 Invalid!";
                                    throw new Exception("PerAddressLine1 required!!");
                                }

                                if (dr[29].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Per_Address2", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Per_Address2"].Value = dr[29].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "PerAddressLine2 Invalid!";
                                    throw new Exception("PerAddressLine2 required!!");
                                }

                                if (dr[30].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Per_Address3", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Per_Address3"].Value = dr[30].ToString();
                                }
                                else
                                {
                                    cmd.Parameters.Add("@Per_Address3", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Per_Address3"].Value = DBNull.Value;

                                }

                                Int64 perstateid = 0;
                                if (dr[31].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Location] WHERE Parent_ID=1 and  Name= '" + dr[31].ToString() + "'";
                                    string PerStateID = GetDataByValue(getValue);
                                    perstateid = Convert.ToInt64(PerStateID);

                                    cmd.Parameters.Add("@Per_State_ID", SqlDbType.BigInt);
                                    cmd.Parameters["@Per_State_ID"].Value = Convert.ToInt64(PerStateID);
                                }
                                else
                                {
                                    ErrorMessageP = "PerState Invalid!";
                                    throw new Exception("PerState required!!");
                                }

                                if (dr[32].ToString() != "")
                                {
                                    string getValue = " SELECT ID  FROM [NIELIT].[dbo].[Location] WHERE Parent_ID=" + perstateid + " and Name= '" + dr[32].ToString() + "'";
                                    string PerDistrictID = GetDataByValue(getValue);

                                    cmd.Parameters.Add("@Per_District_ID", SqlDbType.BigInt);
                                    cmd.Parameters["@Per_District_ID"].Value = Convert.ToInt64(PerDistrictID);
                                }
                                else
                                {
                                    ErrorMessageP = "PerDistrict Invalid!";
                                    throw new Exception("PerDistrict required!!");
                                }

                                if (dr[33].ToString() != "")
                                {
                                    cmd.Parameters.Add("@Per_City_Name", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@Per_City_Name"].Value = dr[33].ToString();
                                }
                                else
                                {
                                    ErrorMessageP = "PerCityName required!!";
                                    throw new Exception("PerCityName required!!");
                                }

                                if (dr[34].ToString() != "" && dr[34].ToString().Length == 6 && isNumber(dr[34].ToString().Trim()))
                                {
                                    cmd.Parameters.Add("@Per_Pin_Code", SqlDbType.Int);
                                    cmd.Parameters["@Per_Pin_Code"].Value = Convert.ToInt32(dr[34].ToString());
                                }
                                else
                                {
                                    ErrorMessageP = "PerPinCode Invalid!";
                                    throw new Exception("PerPinCode required!!");
                                }
                                // Permanent Address End

                                //Identification Details start
                                if (dr[35].ToString() != "")
                                {
                                    if (dr[35].ToString() == "Aadhaar")
                                    {

                                        cmd.Parameters.Add("@UID_Type", SqlDbType.Int);
                                        cmd.Parameters["@UID_Type"].Value = 1;
                                        string AadharEncryptedUidNumber = EncryptDecrypt.EncryptString(dr[36].ToString());
                                        // ncs.UIDNumber=AadharEncryptedUidNumber;
                                        if (dr[36].ToString().Length == 12 && isNumber(dr[36].ToString().Trim()))
                                        {
                                            cmd.Parameters.Add("@Aadhar_Number", SqlDbType.BigInt);
                                            cmd.Parameters["@Aadhar_Number"].Value = Convert.ToInt64(dr[36].ToString());
                                            cmd.Parameters.Add("@UID_Number", SqlDbType.VarChar, 50);
                                            cmd.Parameters["@UID_Number"].Value = AadharEncryptedUidNumber;
                                        }
                                        else
                                        {
                                            ErrorMessageP = "Aadhaar Invalid!";
                                            throw new Exception("Aadhaar required!!");
                                        }
                                    }
                                    if (dr[36].ToString() == "PAN")
                                    {
                                        cmd.Parameters.Add("@UID_Type", SqlDbType.Int);
                                        cmd.Parameters["@UID_Type"].Value = 2;
                                        if (dr[36].ToString().Length == 10)
                                        {
                                            cmd.Parameters.Add("@UID_Number", SqlDbType.VarChar, 50);
                                            cmd.Parameters["@UID_Number"].Value = dr[36].ToString();
                                            cmd.Parameters.Add("@Aadhar_Number", SqlDbType.BigInt);
                                            cmd.Parameters["@Aadhar_Number"].Value = DBNull.Value;
                                        }
                                        else
                                        {
                                            ErrorMessageP = "PAN Invalid!";
                                            throw new Exception("PAN required!!");
                                        }
                                    }
                                }
                                else
                                {
                                    cmd.Parameters.Add("@UID_Type", SqlDbType.Int);
                                    cmd.Parameters["@UID_Type"].Value = DBNull.Value;
                                    cmd.Parameters.Add("@UID_Number", SqlDbType.VarChar, 50);
                                    cmd.Parameters["@UID_Number"].Value = DBNull.Value;
                                    cmd.Parameters.Add("@Aadhar_Number", SqlDbType.BigInt);
                                    cmd.Parameters["@Aadhar_Number"].Value = DBNull.Value;
                                }
                                //Identification Details End

                                //Whether project details or not start
                                if (dr[37].ToString() == "Y")
                                {
                                    if (dr[38].ToString() != "")
                                    {
                                        string getValue = " SELECT ID  FROM [NIELIT].[dbo].[projMaster] WHERE projName= '" + dr[38].ToString() + "'";
                                        string projectId = GetDataByValue(getValue);
                                        if (Convert.ToInt32(projectId) != 0)
                                        {

                                            cmd.Parameters.Add("@projectId", SqlDbType.BigInt);
                                            cmd.Parameters["@projectId"].Value = Convert.ToInt64(projectId);
                                            cmd.Parameters.Add("@whetherProjectStudent", SqlDbType.Int);
                                            cmd.Parameters["@whetherProjectStudent"].Value = 1;
                                        }
                                        else
                                        {
                                            throw new Exception("Project Name required!!");
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessageP = "Project Name required!!";
                                        throw new Exception("Project Name required!!");
                                    }
                                }
                                else if (dr[37].ToString() == "N")
                                {
                                    cmd.Parameters.Add("@projectId", SqlDbType.BigInt);
                                    cmd.Parameters["@projectId"].Value = DBNull.Value;
                                    cmd.Parameters.Add("@whetherProjectStudent", SqlDbType.Int);
                                    cmd.Parameters["@whetherProjectStudent"].Value = 0;
                                }
                                else
                                {
                                    ErrorMessageP = "whetherProjectStudent (Y/N) Not Selected !!";
                                    throw new Exception("whetherProjectStudent (Y/N) required!!");
                                }
                                cmd.Parameters.Add("@enterBy", SqlDbType.Int);
                                cmd.Parameters["@enterBy"].Value = Convert.ToInt32(Session["UserID"]);

                                cmd.Parameters.Add("@batchID", SqlDbType.BigInt);
                                cmd.Parameters["@batchID"].Value = Convert.ToInt64(ddlBatch.SelectedValue);

                                cmd.Parameters.Add("@CourseID", SqlDbType.BigInt);
                                cmd.Parameters["@CourseID"].Value = Convert.ToInt64(ddlCourse.SelectedValue);

                                string whetherAffiliated = "O";
                                if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                                {
                                    whetherAffiliated = "Y";
                                }
                                if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                                {
                                    whetherAffiliated = "N";
                                }
                                cmd.Parameters.Add("@whetherAffiliated", SqlDbType.Char, 1);
                                cmd.Parameters["@whetherAffiliated"].Value = whetherAffiliated;

                                cmd.Parameters.Add("@ReturnStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                                connections.Open();
                                cmd.ExecuteNonQuery();
                                int pReturnStatus = Convert.ToInt32(cmd.Parameters["@ReturnStatus"].Value);
                                if (pReturnStatus != 200)
                                {
                                    ErrorMessageP = "Records Already Exits !!";
                                    throw new Exception("Records Already Exits !!");
                                }
                                connections.Close();
                                ValidateRecords = ValidateRecords + 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (dr[5].ToString() != null)
                            {
                                ErrorMessage = "";
                                slnos = "0";
                                ErrorMessage = ex.Message.ToString();
                                slnos = dr[0].ToString();
                                FaildRecords.Append(dr[0].ToString() + "- " + dr[5].ToString() + "-" + ex.Message + ",");
                                FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                                failedRecordCount++;
                                if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                    FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                            }
                        }
                    }
                }
            }
            divValidateData.Visible = true;
            btnCancel.Visible = true;
            btnUpload.Visible = false;
            btnback.Visible = false;
            r1.Visible = false;
            r2.Visible = false;
            r3.Visible = false;
            r4.Visible = false;
            failedRecordCount = TotalRecords - ValidateRecords;
            lblTotalRecords.Text = TotalRecords.ToString();
            lblValidateRecords.Text = ValidateRecords.ToString();
            //lblFailedRecords.Text = failedRecordCount.ToString() + WebUtility.HtmlDecode("<br/>") + FaildRecords.ToString().TrimEnd(',');
            lblFailedRecords.Text = failedRecordCount.ToString();
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

        finally
        {
            dr.Close();
            dr.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            System.IO.File.Delete(path);
        }
    }
    public void ExportToPDFFile(DataTable objDataTable)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DetailsOfRemarks.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Document document = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(document);
            PdfWriter.GetInstance(document, Response.OutputStream);

            document.Open();
            PdfPTable table = new PdfPTable(objDataTable.Columns.Count);
            table.WidthPercentage = 90;

            //Set columns names in the pdf file
            for (int k = 0; k < objDataTable.Columns.Count; k++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(objDataTable.Columns[k].ColumnName));

                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                //cell.BackgroundColor = new iTextSharp.text.Color(160, 160, 160);
                // cell.BackgroundColor = new iTextSharp.text.co(160, 160, 160);
                cell.Padding = 1;

                table.AddCell(cell);
            }

            //Add values of DataTable in pdf file
            for (int i = 0; i < objDataTable.Rows.Count; i++)
            {
                for (int j = 0; j < objDataTable.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(objDataTable.Rows[i][j].ToString()));

                    //Align the cell in the center
                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT;

                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();
            Response.Write(document);
            Response.End();
        }
        catch (Exception ex)
        {

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = FillGridViewNIELITCentreStudentUploadRemarks();
        if (dt.Rows.Count > 0)
        {
            ExportToPDFFile(dt);
        }
    }

    //private void ExportGridToText()
    //{
    //    BindGridView();
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", "attachment;filename=Vithal_Wadje.txt");
    //    Response.Charset = "";
    //    Response.ContentType = "application/text";
    //    gvMain.AllowPaging = false;
    //    gvMain.DataBind();
    //    StringBuilder Rowbind = new StringBuilder();
    //    for (int k = 0; k < gvMain.Columns.Count; k++)
    //    {
    //        Rowbind.Append(gvMain.Columns[k].HeaderText + ' ');
    //    }
    //    Rowbind.Append("\r\n");
    //    for (int i = 0; i < gvMain.Rows.Count; i++)
    //    {
    //        for (int k = 0; k < gvMain.Columns.Count; k++)
    //        {
    //            Rowbind.Append(gvMain.Rows[i].Cells[k].Text + ' ');
    //        }
    //        Rowbind.Append("\r\n");
    //    }
    //    Response.Output.Write(Rowbind.ToString());
    //    Response.Flush();
    //    Response.End();
    //}



    protected bool isNumber(String strvalue)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (strvalue.Trim() != "")
            {
                if (!regex.IsMatch(strvalue.Trim()))
                {
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isValidDob(string strvaluedob)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(strvaluedob);

            int result1 = DateTime.Compare(todaydate, Inputdate);
            int result2 = DateTime.Compare(todaydate.AddYears(-10), Inputdate);

            if (result2 == -1)
            {
                lblerror.Text = "Invalid date of birth";
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();

            divValidateData.Visible = false;
            btnCancel.Visible = false;
            btnUpload.Visible = true;
            btnback.Visible = true;
            r1.Visible = true;
            r2.Visible = true;
            r3.Visible = true;
            r4.Visible = true;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public string GetDataByValue(string myQuery)
    {
        string result = "0";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        SqlCommand cmd = new SqlCommand(myQuery, conn);
        conn.Open();
        string getValue = cmd.ExecuteScalar().ToString();
        if (getValue != null)
        {
            result = getValue.ToString();
        }
        conn.Close();
        return result;
    }

    protected void NewEntryPage()
    {
        btnMode.ViewMode = ToggleView.Mode.List;
        mltvTab.ActiveViewIndex = 1;
        pnlFilter.Visible = false;
        ucSearchBar.Visible = false;
        //Change the heading text as required
        lblHeading.Text = "Nielit Centre Student";
        //Updating Breadcrumb       
        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Nielit Centre Student Upload", "", ""));
    }
    // deep add code on 29 june 2022 end    
}