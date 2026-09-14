using System;
using System.Data.Objects;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Common_SearchBCCCCCandidate : BasePage
{
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
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search DLC/IRDA-Exam Candidate", "/Common/SearchBCCCCCandidate.aspx", ""));
                //BindDdl();
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

                ListItem lst1 = new ListItem("--All--", "A");
                var apappTypep = from p in context.ApplicantTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlapptype, apappTypep, lst1);
                ddlapptype.Items.Insert(0, new ListItem("--Select--", "0"));

                ListItem lst2 = new ListItem("--All--", "A");
                var crs = from p in context.Courses
                          where p.CourseTypeID == 2
                          orderby (p.DisplayOrder)
                          select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcname, crs, lst2);
                ddlcname.Items.Insert(0, new ListItem("--Select--", "0"));
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
            //BindGridView(); //commented on 14/10/2015 by vivek
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
            string gender = "";
            string filter = "";
            DateTime? regdatefrom = null;
            DateTime? regdateto = null;
            DateTime? dateofbirth = null;
            if (ddlcname.SelectedValue != "0" && ddlcname.SelectedValue != "A" && ddlcname.SelectedValue != "")
                cid = Convert.ToInt32(ddlcname.SelectedValue);
            if (ddlapptype.SelectedValue != "0" && ddlapptype.SelectedValue != "A" && ddlcname.SelectedValue != "")
                applicanttypeID = Convert.ToInt32(ddlapptype.SelectedValue);
            filter = Convert.ToString(Txtfilter.Text.Trim().ToUpper());
            if (applicanttypeID == Convert.ToInt32(enmApplicantType.Institute))
            {
                if (HfInstitute.Value != "0" && HfInstitute.Value != "")
                {
                    instituteID = Convert.ToInt64(HfInstitute.Value);
                }
            }
            if (ddlGender.SelectedValue != "0" && ddlGender.SelectedValue != "A")
            {
                gender = ddlGender.SelectedItem.Text;
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
            gvMain.DataSource = null;
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            #region //Search by Student Application Number-
            if (Rdsearchby.SelectedValue == "0")
            {
                if (Txtfilter.Text.Trim() != "" && Txtfilter.Text != null)
                {
                    //...Search by Application Number..........
                    var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                       where  c.Number == filter
                                       select new
                                       {
                                           ID = c.ID,
                                           ApplID = c.ID,
                                           Applno = c.Number,
                                           CourseID = c.CourseID,
                                           couID = cid,
                                           Status = regstatusid,
                                           canddob = c.DateOfBirth,
                                           cangender = c.Gender,
                                           date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                           studname = c.Name,
                                           fathname = c.FatherName,
                                           mothername = c.MotherName,
                                           StudentName = c.Salutation + c.Name.ToUpper(),
                                           FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                           CourseName = c.Course.Code.ToUpper(),
                                           guardian = c.GuardianName,
                                           AptpID = applicanttypeID,
                                           ApptypeID = c.ApplicantTypeID,
                                           InstituteID = c.InstituteID,
                                           regdate = regdatefrom,
                                           regtodate = regdateto,
                                           dob = dateofbirth,
                                           filcriteria = Txtfilter.Text,
                                           index = PagingBar1.CurrentPageIndex
                                       };


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
                                    registration = registration.OrderByDescending(s => s.Applno);
                                else
                                    registration = registration.OrderBy(s => s.Applno);
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
                    PagingBar1.Bind(registration, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvMain.Rows.Count == 0)
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No Record Found";
                    }
                    else
                    {
                        Lblerror.Visible = false;
                    }

                }
            }
            #endregion
            #region //Search by Student name Start With, DOB-
            else if (Rdsearchby.SelectedValue == "1")
            {
                //......Search by Candidate Name, DOB ....
                if (dateofbirth.HasValue)
                {
                    var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                       where c.CourseID == cid  && c.Name.Contains(filter)
                                       select new
                                       {
                                           ID = c.ID,
                                           ApplID = c.ID,
                                           Applno = c.Number,
                                           CourseID = c.CourseID,
                                           couID = cid,
                                           Status = regstatusid,
                                           canddob = c.DateOfBirth,
                                           cangender = c.Gender,
                                           date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                           studname = c.Name,
                                           fathname = c.FatherName,
                                           mothername = c.MotherName,
                                           StudentName = c.Salutation + c.Name.ToUpper(),
                                           FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                           CourseName = c.Course.Code.ToUpper(),
                                           guardian = c.GuardianName,
                                           AptpID = applicanttypeID,
                                           ApptypeID = c.ApplicantTypeID,
                                           InstituteID = c.InstituteID,
                                           regdate = regdatefrom,
                                           regtodate = regdateto,
                                           dob = dateofbirth,
                                           filcriteria = Txtfilter.Text,
                                           index = PagingBar1.CurrentPageIndex
                                       };

                    if (cid >= 1)
                    {
                        registration = registration.Where(s => s.CourseID == cid);
                    }
                    if (applicanttypeID >= 1)
                    {
                        registration = registration.Where(s => s.ApptypeID == applicanttypeID);
                    }
                    if (instituteID != 0)
                    {
                        registration = registration.Where(s => s.InstituteID == instituteID);
                    }
                    if (gender != "")
                    {
                        registration = registration.Where(s => s.cangender == gender);
                    }
                    if (regdatefrom.HasValue && regdateto.HasValue == false)
                    {
                        registration = registration.Where(s => s.date == regdatefrom.Value);
                    }
                    if (regdatefrom.HasValue && regdateto.HasValue)
                    {
                        registration = registration.Where(s => s.date >= regdatefrom.Value && s.date <= regdateto.Value);
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
                                    registration = registration.OrderByDescending(s => s.Applno);
                                else
                                    registration = registration.OrderBy(s => s.Applno);
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
                    PagingBar1.Bind(registration, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvMain.Rows.Count == 0)
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No Record Found";
                    }
                    else
                    {
                        Lblerror.Visible = false;
                    }
                }
                //......Search by Candidate Start Name only 
                else
                {
                    var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                       where c.CourseID == cid && c.Name.Contains(filter.ToUpper())
                                       select new
                                       {
                                           ID = c.ID,
                                           ApplID = c.ID,
                                           Applno = c.Number,
                                           CourseID = c.CourseID,
                                           couID = cid,
                                           Status = regstatusid,
                                           canddob = c.DateOfBirth,
                                           cangender = c.Gender,
                                           date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                           studname = c.Name,
                                           fathname = c.FatherName,
                                           mothername = c.MotherName,
                                           StudentName = c.Salutation + c.Name.ToUpper(),
                                           FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                           CourseName = c.Course.Code.ToUpper(),
                                           guardian = c.GuardianName,
                                           AptpID = applicanttypeID,
                                           ApptypeID = c.ApplicantTypeID,
                                           InstituteID = c.InstituteID,
                                           regdate = regdatefrom,
                                           regtodate = regdateto,
                                           dob = dateofbirth,
                                           filcriteria = Txtfilter.Text,
                                           index = PagingBar1.CurrentPageIndex
                                       };

                    if (cid >= 1)
                    {
                        registration = registration.Where(s => s.CourseID == cid);
                    }
                    if (applicanttypeID >= 1)
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
                    if (gender != "")
                    {
                        registration = registration.Where(s => s.cangender == gender);
                    }
                    if (dateofbirth.HasValue)
                    {
                        registration = registration.Where(s => s.canddob == dateofbirth);
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
                                    registration = registration.OrderByDescending(s => s.Applno);
                                else
                                    registration = registration.OrderBy(s => s.Applno);
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
                    PagingBar1.Bind(registration, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    //if (registration.Count() == 0)
                    if (gvMain.Rows.Count == 0)
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No Record Found";
                    }
                    else
                    {
                        Lblerror.Visible = false;
                    }
                }
            }
            #endregion
            #region //Search by Father start Name-
            else if (Rdsearchby.SelectedValue == "2")
            {
                var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                   where c.CourseID == cid && c.FatherName.Contains(filter)
                                   select new
                                   {
                                       ID = c.ID,
                                       ApplID = c.ID,
                                       Applno = c.Number,
                                       CourseID = c.CourseID,
                                       couID = cid,
                                       Status = regstatusid,
                                       canddob = c.DateOfBirth,
                                       cangender = c.Gender,
                                       date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                       studname = c.Name,
                                       fathname = c.FatherName,
                                       mothername = c.MotherName,
                                       StudentName = c.Salutation + c.Name.ToUpper(),
                                       FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                       CourseName = c.Course.Code.ToUpper(),
                                       guardian = c.GuardianName,
                                       AptpID = applicanttypeID,
                                       ApptypeID = c.ApplicantTypeID,
                                       InstituteID = c.InstituteID,
                                       regdate = regdatefrom,
                                       regtodate = regdateto,
                                       dob = dateofbirth,
                                       filcriteria = Txtfilter.Text,
                                       index = PagingBar1.CurrentPageIndex
                                   };

                if (dateofbirth.HasValue)
                {
                    registration = registration.Where(s => s.canddob == dateofbirth);
                }
                if (cid >= 1)
                {
                    registration = registration.Where(s => s.CourseID == cid);
                }
                if (applicanttypeID >= 1)
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
                if (gender != "")
                {
                    registration = registration.Where(s => s.cangender == gender);
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
                                registration = registration.OrderByDescending(s => s.Applno);
                            else
                                registration = registration.OrderBy(s => s.Applno);
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
                PagingBar1.Bind(registration, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count == 0)
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "No Record Found";
                }
                else
                {
                    Lblerror.Visible = false;
                }
            }
            #endregion
            #region //Search by Mother start Name-
            else if (Rdsearchby.SelectedValue == "3")
            {
                var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                   where c.CourseID == cid && c.MotherName.Contains(filter)
                                   select new
                                   {
                                       ID = c.ID,
                                       ApplID = c.ID,
                                       Applno = c.Number,
                                       CourseID = c.CourseID,
                                       couID = cid,
                                       Status = regstatusid,
                                       canddob = c.DateOfBirth,
                                       cangender = c.Gender,
                                       date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                       studname = c.Name,
                                       fathname = c.FatherName,
                                       mothername = c.MotherName,
                                       StudentName = c.Salutation + c.Name.ToUpper(),
                                       FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                       CourseName = c.Course.Code.ToUpper(),
                                       guardian = c.GuardianName,
                                       AptpID = applicanttypeID,
                                       ApptypeID = c.ApplicantTypeID,
                                       InstituteID = c.InstituteID,
                                       regdate = regdatefrom,
                                       regtodate = regdateto,
                                       dob = dateofbirth,
                                       filcriteria = Txtfilter.Text,
                                       index = PagingBar1.CurrentPageIndex
                                   };

                if (dateofbirth.HasValue)
                {
                    registration = registration.Where(s => s.canddob == dateofbirth);
                }
                if (cid >= 1)
                {
                    registration = registration.Where(s => s.CourseID == cid);
                }
                if (applicanttypeID >= 1)
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
                if (gender != "")
                {
                    registration = registration.Where(s => s.cangender == gender);
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
                                registration = registration.OrderByDescending(s => s.Applno);
                            else
                                registration = registration.OrderBy(s => s.Applno);
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
                PagingBar1.Bind(registration, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                //if (registration.Count() == 0)
                if (gvMain.Rows.Count == 0)
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "No Record Found";
                }
                else
                {
                    Lblerror.Visible = false;
                }

            }
            #endregion
            #region //Search by Guardian start Name
            else if (Rdsearchby.SelectedValue == "4")
            {
                var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                   where c.CourseID == cid && c.GuardianName.Contains(filter)
                                   select new
                                   {
                                       ID = c.ID,
                                       ApplID = c.ID,
                                       Applno = c.Number,
                                       CourseID = c.CourseID,
                                       couID = cid,
                                       Status = regstatusid,
                                       canddob = c.DateOfBirth,
                                       cangender = c.Gender,
                                       date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                       studname = c.Name,
                                       fathname = c.FatherName,
                                       mothername = c.MotherName,
                                       StudentName = c.Salutation + c.Name.ToUpper(),
                                       FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                       CourseName = c.Course.Code.ToUpper(),
                                       guardian = c.GuardianName,
                                       AptpID = applicanttypeID,
                                       ApptypeID = c.ApplicantTypeID,
                                       InstituteID = c.InstituteID,
                                       regdate = regdatefrom,
                                       regtodate = regdateto,
                                       dob = dateofbirth,
                                       filcriteria = Txtfilter.Text,
                                       index = PagingBar1.CurrentPageIndex
                                   };

                if (dateofbirth.HasValue)
                {
                    registration = registration.Where(s => s.canddob == dateofbirth);
                }
                if (cid >= 1)
                {
                    registration = registration.Where(s => s.CourseID == cid);
                }
                if (applicanttypeID >= 1)
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
                if (gender != "")
                {
                    registration = registration.Where(s => s.cangender == gender);
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
                                registration = registration.OrderByDescending(s => s.Applno);
                            else
                                registration = registration.OrderBy(s => s.Applno);
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
                PagingBar1.Bind(registration, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count == 0)
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "No Record Found";
                }
                else
                {
                    Lblerror.Visible = false;
                }

            }
            #endregion
            #region //Search by Student Roll Number-
            if (Rdsearchby.SelectedValue == "5")
            {
                if (Txtfilter.Text.Trim() != "" && Txtfilter.Text != null)
                {
                    //...Search by Application Number..........
                    var registration = from c in context.CertificateExamApplications.AsNoTracking()
                                       where c.RollNumber == filter
                                       select new
                                       {
                                           ID = c.ID,
                                           ApplID = c.ID,
                                           Applno = c.Number,
                                           CourseID = c.CourseID,
                                           couID = cid,
                                           Status = regstatusid,
                                           canddob = c.DateOfBirth,
                                           cangender = c.Gender,
                                           date = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                           studname = c.Name,
                                           fathname = c.FatherName,
                                           mothername = c.MotherName,
                                           StudentName = c.Salutation + c.Name.ToUpper(),
                                           FatherName = (!string.IsNullOrEmpty(c.FatherName)) ? "Mr." + c.FatherName.ToUpper() : "NA",
                                           CourseName = c.Course.Code.ToUpper(),
                                           guardian = c.GuardianName,
                                           AptpID = applicanttypeID,
                                           ApptypeID = c.ApplicantTypeID,
                                           InstituteID = c.InstituteID,
                                           regdate = regdatefrom,
                                           regtodate = regdateto,
                                           dob = dateofbirth,
                                           filcriteria = Txtfilter.Text,
                                           index = PagingBar1.CurrentPageIndex
                                       };


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
                                    registration = registration.OrderByDescending(s => s.Applno);
                                else
                                    registration = registration.OrderBy(s => s.Applno);
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
                    PagingBar1.Bind(registration, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvMain.Rows.Count == 0)
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No Record Found";
                    }
                    else
                    {
                        Lblerror.Visible = false;
                    }

                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        { context.Dispose(); }
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
            TblOptions.Visible = false;
            if (Rdsearchby.SelectedValue != "0" && Rdsearchby.SelectedValue != "5")
            {
                TblOptions.Visible = true;
                BindDdl();
            }
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
            aceSearch.ContextKey = ddlcname.SelectedValue;
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