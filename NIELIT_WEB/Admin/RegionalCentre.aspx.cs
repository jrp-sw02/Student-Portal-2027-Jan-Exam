using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.Services.Protocols;
using System.Web.Services;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.HRMS;
using System.Web.Security;
using EConnect.Utils.Common;
using EConnect.NIELIT;
public partial class RegionalCentre : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillFilterRegional();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    //BindCity();
                    FillFilterRegional();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Regional Centres", "Admin/RegionalCentre.aspx", ""));

                }
                // BindState();
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
    public void FillFilterRegional()
    {
        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--All--", "0");
            var state = from s in context.Locations
                        where s.LocationTypeID == 2
                        select new { ValueField = s.ID, TextField = s.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlflregcen, state, lst);

        }
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
            lblHeading.Text = "Regional Centres";
            tblNavLinks.Visible = false;
            tblShow.Visible = true;


            using (EConnectContext context = new EConnectContext())
            {
                Int32 RecentreId = Convert.ToInt32(Request.QueryString["Key"]);
                var Regcentre = (from p in context.RegionalCenters
                                 where p.ID == RecentreId
                                 select p).FirstOrDefault();
                txtname.Text = Regcentre.Name.ToString().ToUpper();
                txtcode.Text = Regcentre.Code.ToString().ToUpper();
                txtcontact.Text = Regcentre.ContactNumbers.ToString();
                txtemail.Text = Regcentre.EmailAddresses.ToString();
                txtweb.Text = Regcentre.Website.ToString();
                txtaddress.Text = Regcentre.Address.ToString();
                txtregmobileno.Text = Regcentre.RegisteredMobileNumber.ToString();
                txtregemail.Text = Regcentre.RegisteredEmailAddress.ToString();
                txtBankAccount.Text = Regcentre.BankAccountName;
                txtBankBranch.Text = Regcentre.BankBranchName;
                Int32 courseTypeID = Convert.ToInt32(EConnect.NIELIT.enmCourseType.CertificationExam);
                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Request.QueryString["Name"], "#", ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;

                MappedCourseState();

            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {

        }
    }
    protected void BindGridView()
    {
        try
        {
            //Int64 instituteId = Convert.ToInt64(hfAccreID.Value);
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            int RegID = 0;

            if (ddlflregcen.SelectedValue != "0")
                RegID = Convert.ToInt16(ddlflregcen.SelectedValue);

            var RCentre = from s in context.RegionalCenters
                          select new { ID = s.ID, Name = s.Name, Code = s.Code, email = s.RegisteredEmailAddress, contactno = s.RegisteredMobileNumber.HasValue ? s.RegisteredMobileNumber.Value : 0 };
            if (!String.IsNullOrEmpty(searchString))
            {
                RCentre = RCentre.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            if (RegID != 0)
            {

                RCentre = RCentre.Where(s => s.ID == RegID);

            }
            //if (districtID != 0)
            //{
            //    RegionalCentre = RegionalCentre.Where(s => s.DistrictID == districtID);
            //}
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            RCentre = RCentre.OrderByDescending(s => s.ID);
                        else
                            RCentre = RCentre.OrderBy(s => s.ID);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            RCentre = RCentre.OrderByDescending(s => s.Name);
                        else
                            RCentre = RCentre.OrderBy(s => s.Name);
                        break;
                    case "Code":
                        if (sortOrder == "DESC")
                            RCentre = RCentre.OrderByDescending(s => s.Code);
                        else
                            RCentre = RCentre.OrderBy(s => s.Code);
                        break;
                    case "email":
                        if (sortOrder == "DESC")
                            RCentre = RCentre.OrderByDescending(s => s.email);
                        else
                            RCentre = RCentre.OrderBy(s => s.email);
                        break;
                    case "contatcno":
                        if (sortOrder == "DESC")
                            RCentre = RCentre.OrderByDescending(s => s.contactno);
                        else
                            RCentre = RCentre.OrderBy(s => s.contactno);
                        break;
                    default:
                        RCentre = RCentre.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(RCentre, ref gvMain);
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
            lblHeading.Text = "Regional Centres";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Regional Centre", "#", ""));
        }
        else
        {
            Response.Redirect("RegionalCentre.aspx", true);
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
            context = new EConnectContext();
            //if (Context.Menores.Any(s => s.Solicitud.fiExpEmpleado == someValue))

            RegionalCenter objRCentre;

            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {

                objRCentre = new EConnect.NIELIT.RegionalCenter();
                objRCentre.Name = txtname.Text.ToString().ToUpper();
                objRCentre.Code = txtcode.Text.ToString().ToUpper();
                objRCentre.ContactNumbers = txtcontact.Text.ToString();
                objRCentre.EmailAddresses = txtemail.Text.ToString();
                objRCentre.Address = txtaddress.Text.ToString();
                objRCentre.Website = txtweb.Text.ToString();
                objRCentre.BankAccountName = txtBankAccount.Text;
                objRCentre.BankBranchName = txtBankBranch.Text;
                objRCentre.RegisteredEmailAddress = txtregemail.Text;
                if (!string.IsNullOrEmpty(txtregmobileno.Text))
                    objRCentre.RegisteredMobileNumber = Convert.ToInt64(txtregmobileno.Text);
                context.RegionalCenters.Add(objRCentre);
                context.SaveChanges();
                Int32 RegionalCentreID = objRCentre.ID;

                // Inserting the records for new regional centre creation in Regional Centre_Course wise count
                context.Database.ExecuteSqlCommand("insert into Regional_Centre_Course_Wise_Count values ( " + RegionalCentreID + ", 5, 0)");
                context.Database.ExecuteSqlCommand("insert into Regional_Centre_Course_Wise_Count values ( " + RegionalCentreID + ", 7, 0)");
                context.SaveChanges();

                strMessage = "New record saved.";
            }
            else
            {
                objRCentre = context.RegionalCenters.Find(Convert.ToInt16(Request.QueryString["key"]));
                objRCentre.Name = txtname.Text.ToString().ToUpper();
                objRCentre.Code = txtcode.Text.ToString().ToUpper();
                objRCentre.ContactNumbers = txtcontact.Text.ToString();
                objRCentre.EmailAddresses = txtemail.Text.ToString();
                objRCentre.Address = txtaddress.Text.ToString();
                objRCentre.Website = txtweb.Text.ToString();
                objRCentre.BankAccountName = txtBankAccount.Text;
                objRCentre.BankBranchName = txtBankBranch.Text;
                objRCentre.RegisteredEmailAddress = txtregemail.Text;
                if (!string.IsNullOrEmpty(txtregmobileno.Text))
                    objRCentre.RegisteredMobileNumber = Convert.ToInt64(txtregmobileno.Text);
                strMessage = "Record updated.";
                context.SaveChanges();
            }


            Response.Redirect("RegionalCentre.aspx?msg=" + strMessage);
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
            ddlflregcen.SelectedValue = "0";
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
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                RegionalCenter regcentre = context.RegionalCenters.Find(Convert.ToInt32(hfActionID.Value));
                context.RegionalCenters.Remove(regcentre);
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

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
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var Regcentre = from s in context.RegionalCenters
                            //            where s.CourseTypeID == courseType
                            select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                Regcentre = Regcentre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            Regcentre = Regcentre.OrderBy(s => s.Name);

            //var ExamCode = from c in context.ExamCenters
            //               //            where s.CourseTypeID == courseType
            //               select new { Code = c.Code };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    ExamCode = ExamCode.Where(c => c.Code.ToUpper().Contains(searchString));
            //}

            //ExamCenter =
            //Examcentre = Examcentre.Union(ExamCode).Take(count);
            foreach (var course in Regcentre)
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
        Response.Redirect("RegionalCentre.aspx", true);
    }
    protected void btnCourseUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 rCenterID = Convert.ToInt32(Request.QueryString["key"]);
            Int32 courseID = Convert.ToInt32(RblAllCourse.SelectedValue.ToString());
            using (EConnectContext context = new EConnectContext())
            {
                context.Database.ExecuteSqlCommand("Delete from Course_Wise_State Where Regional_Center_ID =" + rCenterID.ToString() + " and Course_ID =" + courseID.ToString());
                Int32 stateID = 0;
                CourseWiseState objCwise;


                foreach (ListItem j in chkStatelist.Items)
                {
                    if (j.Selected && j.Enabled)
                    {
                        objCwise = new EConnect.NIELIT.CourseWiseState();
                        stateID = Convert.ToInt32(j.Value);
                        objCwise.RegionalCenterID = rCenterID;
                        objCwise.CourseID = courseID;
                        objCwise.StateID = stateID;
                        context.CourseWiseStates.Add(objCwise);
                    }
                }

                context.SaveChanges();
            };
            ShowAlert("List updated", true);            
            MappedCourseState();
            RblAllCourse.SelectedValue = courseID.ToString();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void RblAllCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (EConnectContext context = new EConnectContext())
        {

            Int32 rCenterID = Convert.ToInt32(Request.QueryString["key"]);
            Int32 onecourseID = Convert.ToInt32(RblAllCourse.SelectedValue.ToString());

            var state = from s in context.Locations
                        orderby (s.Name)
                        where s.LocationTypeID == 2 && s.ParentLocationID == 1
                        select new { ValueField = s.ID, TextField = s.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(chkStatelist, state);

            var fillstate = (from f in context.CourseWiseStates
                             where f.CourseID == onecourseID
                             select new { StateID = f.StateID, CourseID = f.CourseID, RegionalCenterID = f.RegionalCenterID }).ToList();
            var mappedstate = fillstate.Where(k => k.RegionalCenterID == rCenterID).Select(k => k.StateID).Distinct().ToList();
            if (mappedstate.Count > 0)
            {
                foreach (var s in mappedstate)
                {
                    foreach (ListItem sli in chkStatelist.Items)
                    {
                        if (s == Convert.ToInt64(sli.Value))
                        {
                            sli.Selected = true;
                        }
                    }

                }
            }

            var disablestate = fillstate.Where(k => k.RegionalCenterID != rCenterID).Select(k => k.StateID).Distinct().ToList();
            if (disablestate.Count > 0)
            {
                foreach (var s in disablestate)
                {
                    foreach (ListItem sli in chkStatelist.Items)
                    {
                        if (s == Convert.ToInt64(sli.Value))
                        {
                            sli.Enabled = false;
                        }
                    }

                }
            }


        };
        btnCourseUpdate.Enabled = true;
    }
    protected void MappedCourseState()
    {
        Int32 courseTypeID = Convert.ToInt32(EConnect.NIELIT.enmCourseType.CertificationExam);
        Int32 rCenterID = Convert.ToInt32(Request.QueryString["key"]);

        using (EConnectContext context = new EConnectContext())
        {
            var shorttermcourses = (from c in context.Courses
                                    orderby c.ID
                                    where c.CourseTypeID == courseTypeID
                                    select new { ValueField = c.ID, TextField = c.Name }).ToList();
            EConnect.Utils.Common.ControlUtility.BindListObject(RblAllCourse, shorttermcourses);


            var fillcourse = (from c in context.CourseWiseStates
                              join d in context.Locations on c.StateID equals d.ID
                              where c.RegionalCenterID == rCenterID && d.LocationTypeID == 2
                              orderby d.Name
                              select new { StateID = c.StateID, StateName = d.Name, CourseID = c.CourseID });

            List<string> mappedstatename = new List<string>();
            Int32 mappedcourseid = 0;

            foreach (ListItem sli in RblAllCourse.Items)
            {
                mappedcourseid = Convert.ToInt32(sli.Value);
                fillcourse = fillcourse.Where(s => s.CourseID == mappedcourseid);
                foreach (var l in fillcourse)
                {
                    mappedstatename.Add(l.StateName);
                }
                if (mappedstatename.Count() > 0)
                {
                    foreach (var k in mappedstatename)
                    {

                        sli.Text = sli + " - " + k.ToString();
                    }

                }
                mappedstatename.Clear();
            }
        };
    }
}