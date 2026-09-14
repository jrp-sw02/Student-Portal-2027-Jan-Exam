using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Common_SearchCandidate : BasePage
{
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
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Candidate", "/Common/SearchCandidate.aspx", ""));
                BindDdl();
                if ((!string.IsNullOrEmpty(Request.QueryString["regdate"])) || (!string.IsNullOrEmpty(Request.QueryString["regtodate"])) || (!string.IsNullOrEmpty(Request.QueryString["filcriteria"])) || (!string.IsNullOrEmpty(Request.QueryString["dob"])))
                {
                    divGrid.Visible = true;
                    BindBackGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindDdl()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                Int32 coursetypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--All--", "0");
                var sts = from p in context.RegistrationStatus
                          orderby (p.Name)
                          select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlregstatus, sts, lst);

                ListItem lst1 = new ListItem("--All--", "0");
                var apappTypep = from p in context.ApplicantTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlapptype, apappTypep, lst1);

                ListItem lst2 = new ListItem("--All--", "0");
                var crs = from p in context.Courses
                          where p.CourseTypeID == coursetypeID
                          orderby (p.DisplayOrder)
                          select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    crs = crs.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcname, crs.Distinct(), lst2);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            BreadCrumb1.Render();
            divGrid.Visible = true;
            BindGridView();
            // BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Candidate", "/Common/SearchCandidate.aspx?regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&Status=" + Request.QueryString["Status"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"], ""));
            //BreadCrumb1.Render();
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
            Int32 cid = 0;
            Int32 applicanttypeID = 0;
            Int32 regstatusid = 0;
            Int64 instituteID = 0;
            string filter = "";
            Int64 filter1 = 0;
            DateTime? regdatefrom = null;
            DateTime? regdateto = null;
            DateTime? dateofbirth = null;
            if (ddlcname.SelectedValue != "0")
                cid = Convert.ToInt32(ddlcname.SelectedValue);
            if (ddlapptype.SelectedValue != "0")
                applicanttypeID = Convert.ToInt32(ddlapptype.SelectedValue);
            if (ddlregstatus.SelectedValue != "0")
                regstatusid = Convert.ToInt32(ddlregstatus.SelectedValue);
            if (!IsNumeric(Txtfilter.Text))
            {
                filter = Txtfilter.Text.ToUpper();
            }
            else
                filter1 = Convert.ToInt64(Txtfilter.Text);
            if (applicanttypeID == Convert.ToInt32(enmApplicantType.Institute))
            {
                if (HfInstitute.Value != "0" && HfInstitute.Value != "")
                {
                    instituteID = Convert.ToInt64(HfInstitute.Value);
                }
            }
            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                regdatefrom = Convert.ToDateTime(txtDateFrom.Text).Date;
            }
            if (!string.IsNullOrEmpty(txtDateto.Text))
            {
                regdateto = Convert.ToDateTime(txtDateto.Text).Date;
            }
            if (!string.IsNullOrEmpty(txtdate.Text))
            {
                dateofbirth = Convert.ToDateTime(txtdate.Text).Date;
            }
            context = new EConnectContext();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var registration = from c in context.RegistrationDetails.AsNoTracking()
                               select new
                               {
                                   ID = c.ID,
                                   ApplID = c.CandidateID,
                                   Regno = c.RegistrationNo,
                                   CourseID = c.CourseID,
                                   couID = cid,
                                   Status = regstatusid,
                                   regstatus = c.RegistrationStatusID,
                                   canddob = c.Candidate.DateOfBirth,
                                   regname = c.RegistrationStatusID.HasValue ? c.RegistrationStatus.Name : "NA",
                                   date = System.Data.Entity.DbFunctions.TruncateTime(c.RegistrationDate),
                                   studname = c.Candidate.Name,
                                   fathname = c.Candidate.FatherName,
                                   mothername = c.Candidate.MotherName,
                                   StudentName = c.Candidate.Salutation + c.Candidate.Name.ToUpper(),
                                   FatherName = (!string.IsNullOrEmpty(c.Candidate.FatherName)) ? "Mr." + c.Candidate.FatherName.ToUpper() : "NA",
                                   CourseName = c.Course.Name.ToUpper(),
                                   guardian = c.Candidate.GuardianName,
                                   AptpID = applicanttypeID,
                                   ApptypeID = c.ApplicantTypeID,
                                   InstituteID=c.InstituteID ,
                                   regdate = regdatefrom,
                                   regtodate = regdateto,
                                   dob = dateofbirth,
                                   filcriteria = Txtfilter.Text,
                                   index = PagingBar1.CurrentPageIndex
                               };
            if (cid != 0)
            {
                registration = registration.Where(s => s.CourseID == cid);
            }
            if (regstatusid != 0)
            {
                registration = registration.Where(s => s.regstatus == regstatusid);
            }
            if (applicanttypeID != 0)
            {
                registration = registration.Where(s => s.ApptypeID == applicanttypeID);
            }
            if (instituteID != 0)
            {
                registration = registration.Where(s => s.InstituteID == instituteID);
            }
            if (regdatefrom.HasValue && regdateto.HasValue == false)
            {
                registration = registration.Where(s => s.date == regdatefrom.Value);
            }
           
            if (regdatefrom.HasValue && regdateto.HasValue)
            {
                registration = registration.Where(s => s.date >= regdatefrom.Value && s.date <= regdateto.Value);
            }
            if (dateofbirth.HasValue)
            {
                registration = registration.Where(s => s.canddob == dateofbirth);
            }
            if (Rdsearchby.SelectedValue == "0")
            {
                if (Txtfilter.Text.Trim() != "" && Txtfilter.Text != null)
                {
                    if (!IsNumeric(Txtfilter.Text))
                    {
                        ShowAlert("Enter Valid Registration No. only", true);
                        Txtfilter.Focus();
                        return;
                    }
                    else if (Txtfilter.Text == "0")
                    {
                        ShowAlert("Invalid Registration No.", true);
                        Txtfilter.Focus();
                        return;
                    }
                    else
                    {
                        filter1 = Convert.ToInt64(Txtfilter.Text);
                    }

                    if (filter1 != 0)
                    {
                        registration = registration.Where(s => s.Regno == filter1);
                    }
                }
            }
            else if (Rdsearchby.SelectedValue == "1")
            {
                registration = registration.Where(s => s.studname.ToUpper().Contains(filter));
            }
            else if (Rdsearchby.SelectedValue == "2")
            {
                registration = registration.Where(s => s.fathname.ToUpper().Contains(filter));
            }
            else if (Rdsearchby.SelectedValue == "3")
            {
                registration = registration.Where(s => s.mothername.ToUpper().Contains(filter));
            }
            else if (Rdsearchby.SelectedValue == "4")
            {
                registration = registration.Where(s => s.guardian.ToUpper().Contains(filter));
            }          
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.ID);
                        else
                            registration = registration.OrderBy(s => s.ID);
                        break;
                    case "Regno":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.Regno);
                        else
                            registration = registration.OrderBy(s => s.Regno);
                        break;
                    case "date":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.date);
                        else
                            registration = registration.OrderBy(s => s.date);
                        break;
                    case "StudentName":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.StudentName);
                        else
                            registration = registration.OrderBy(s => s.StudentName);
                        break;
                    case "FatherName":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.FatherName);
                        else
                            registration = registration.OrderBy(s => s.FatherName);
                        break;
                    case "CourseName":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.CourseName);
                        else
                            registration = registration.OrderBy(s => s.CourseName);
                        break;
                    case "regname":
                        if (sortOrder == "DESC")
                            registration = registration.OrderByDescending(s => s.CourseName);
                        else
                            registration = registration.OrderBy(s => s.CourseName);
                        break;
                    default:
                        registration = registration.OrderBy(s => s.ID);
                        break;
                }
            }

            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                registration = registration.Where(a => roleCourses.Contains(a.CourseID));
            }
            PagingBar1.Bind(registration, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (registration.Count() == 0)
            {
                Lblerror.Visible = true;
                Lblerror.Text = "No Record Found";
            }
            else
            {
                Lblerror.Visible = false;
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
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        ddlcname.SelectedValue = "0";
        ddlapptype.SelectedValue = "0";
        ddlregstatus.SelectedValue = "0";
        txtDateFrom.Text = "";
        txtDateto.Text = "";
        Txtfilter.Text = "";
        Lblerror.Visible = false;
        divGrid.Visible = false;
        TrIns1.Visible = false;
        TrIns2.Visible = false;
        HfInstitute.Value = "0";
        hfInstituteName.Value = "";
        TxtInstituteName.Text = "";

    }
    protected void BindBackGrid()
    {
        try
        {
            if (Request.QueryString["couID"] != null)
                ddlcname.SelectedValue = Request.QueryString["couID"];
            if (Request.QueryString["AptpID"] != null)
                ddlapptype.SelectedValue = Request.QueryString["AptpID"];
            if (Request.QueryString["Status"] != null)
                ddlregstatus.SelectedValue = Request.QueryString["Status"];
            if (Request.QueryString["regdate"] != null)
                txtDateFrom.Text = Request.QueryString["regdate"];
            if (Request.QueryString["regtodate"] != null)
                txtDateto.Text = Request.QueryString["regtodate"];
            if (Request.QueryString["dob"] != null)
                txtdate.Text = Request.QueryString["dob"];
            if (Request.QueryString["filcriteria"] != null)
                Txtfilter.Text = Request.QueryString["filcriteria"];
            if (Request.QueryString["index"] != null)
                PagingBar1.CurrentPageIndex = Convert.ToInt32(Request.QueryString["index"]);
            BindGridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Rdsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Txtfilter.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            aceSearch.ContextKey = ddlcname.SelectedValue ;
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ChkAllInstitute_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (ChkAllInstitute.Checked == true)
            {
                TxtInstituteName.Text = "";
                TxtInstituteName.Enabled = false;
                HfInstitute.Value = "0";
            }
            else
                TxtInstituteName.Enabled = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlapptype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            TrIns1.Visible = false;
            TrIns2.Visible = false;
            if (Convert.ToInt32(ddlapptype.SelectedValue) == Convert.ToInt32(enmApplicantType.Institute))
            {
                TrIns1.Visible = true;
                TrIns2.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}