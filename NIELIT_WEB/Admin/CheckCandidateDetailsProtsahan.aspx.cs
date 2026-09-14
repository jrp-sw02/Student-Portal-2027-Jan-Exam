using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class Admin_CheckCandidateDetailsProtsahan : BasePage
{
    EConnectContext context;
    Int32 currentRoleId = 0;  
    Int32 loginUserNo = 0;
    String strMessage = string.Empty;
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
                lblRegh.Visible = false;
                lblcand.Visible = false;
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                divfilter.Visible = true;
                if ((!String.IsNullOrEmpty(Request.QueryString["Regno"])) && (!String.IsNullOrEmpty(Request.QueryString["courseID"])))
                {
                    bindCastCategory();
                    BindGender();
                    ShowEditMode();                   
                }
                else
                {
                    if ((!String.IsNullOrEmpty(Request.QueryString["RegNoQ"])) && (!String.IsNullOrEmpty(Request.QueryString["LevelQ"])))
                    {
                        if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                            ShowAlert(Request.QueryString["msg"].ToString());

                        Int64 RegNoT = Convert.ToInt64(Request.QueryString["RegNoQ"].ToString());
                        Int32 LevelT = Convert.ToInt32(Request.QueryString["LevelQ"].ToString());
                        BindGridView();
                    }
                    

                }
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Check Candidate Details for Protsahan", "/Admin/CheckCandidateDetailsProtsahan.aspx", ""));
               
            }            
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGender()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                var Gender = from s in
                                 vContext.tblGender
                             select new { ValueField = s.genderCode, TextField = s.name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlGender, Gender, new ListItem("--Select Gender--", "0"));

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindCastCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var castcategory = from p in context.CastCategories
                                   orderby (p.DisplayOrder)
                                   select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCastCategory, castcategory, lst);
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
            if (!IsNumeric(TxtRegNo.Text))
            {
                throw new Exception("Not Valid Registration Number !!");
                return;
            }
            PagingBar1.CurrentPageIndex = 0;
            BreadCrumb1.Render();
            divGrid.Visible = true;
            BindGridView();
           
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
            lblRegh.Visible = true;
            lblcand.Visible = true;
            Int32 Level = 0;
            Int64 RegNo = 0;

            if ((!String.IsNullOrEmpty(Request.QueryString["RegNoQ"])) && (!String.IsNullOrEmpty(Request.QueryString["LevelQ"])))
            {
                RegNo = Convert.ToInt64(Request.QueryString["RegNoQ"].ToString());
                Level = Convert.ToInt32(Request.QueryString["LevelQ"].ToString());
            }
            else
            {
                if (IsNumeric(TxtRegNo.Text))
                {
                    RegNo = Convert.ToInt64(TxtRegNo.Text);
                }
                if (ddlLevel.SelectedValue != "0")
                {
                    Level = Convert.ToInt32(ddlLevel.SelectedValue);
                }
                else
                {
                    throw new Exception("Please select Level");
                    return;
                }
            }

            context = new EConnectContext();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var registration = from c in context.RegistrationDetails.AsNoTracking()
                               join d in context.CourseRegistrationApplications on c.CandidateID equals d.CandidateID
                               where //c.RegistrationNo==d.RegisteredCourseRegistrationNo && 
                               c.CourseID==d.CourseID
                               && d.FinalSubmitted==true && c.RegistrationNo == RegNo && c.CourseID == Level
                               select new
                               {
                                   
                                   ID = d.ID,
                                   ApplID = c.CandidateID,
                                   Regno = c.RegistrationNo,
                                   CourseID = c.CourseID,                                  
                                   canddob = c.Candidate.DateOfBirth,
                                   regname = c.RegistrationStatusID.HasValue ? c.RegistrationStatus.Name : "NA",
                                   date = System.Data.Entity.DbFunctions.TruncateTime(c.RegistrationDate),
                                   studname = c.Candidate.Name, 
                                   StudentName = c.Candidate.Salutation + c.Candidate.Name.ToUpper(),
                                   FatherName = (!string.IsNullOrEmpty(c.Candidate.FatherName)) ? "Mr." + c.Candidate.FatherName.ToUpper() : "NA",
                                   CourseName = c.Course.Name.ToUpper(),
                                   guardian = c.Candidate.GuardianName,
                                   Castname=d.CastCategory.Name.ToString(),
                                   CastID=d.CastCategoryID,
                                   phId = d.IsHandicaped.ToString() == "True" ? "1" : "0",
                                   phName = d.IsHandicaped.ToString() == "True" ? "YES" : "NO",
                                   GenderName=d.Gender,
                                   index = PagingBar1.CurrentPageIndex
                               };

            var candidatesDetails = from s in context.Candidates
                                join r in context.RegistrationDetails on s.ID equals r.CandidateID
                                    where r.RegistrationNo == RegNo && r.CourseID == Level
                                select new
                                {
                                    SLNO = "1",
                                    ID = r.ID,
                                    ApplID = r.CandidateID,
                                    Regno = r.RegistrationNo,
                                    CourseID = r.CourseID,
                                    canddob = r.Candidate.DateOfBirth,
                                    regname = r.RegistrationStatusID.HasValue ? r.RegistrationStatus.Name : "NA",
                                    date = System.Data.Entity.DbFunctions.TruncateTime(r.RegistrationDate),
                                    studname = r.Candidate.Name,
                                    StudentName = r.Candidate.Salutation + r.Candidate.Name.ToUpper(),
                                    FatherName = (!string.IsNullOrEmpty(r.Candidate.FatherName)) ? "Mr." + r.Candidate.FatherName.ToUpper() : "NA",
                                    CourseName = r.Course.Name.ToUpper(),
                                    guardian = r.Candidate.GuardianName,
                                    Castname = s.CastCategory.Name.ToString(),
                                    CastID = s.CastCategoryID,
                                    phId = s.IsHandicaped.ToString() == "True" ? "1" : "0",
                                    phName = s.IsHandicaped.ToString() == "True" ? "YES" : "NO",
                                    GenderName = s.Gender,
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
            PagingBar1.Visible = false;
            if (registration.Count() == 0)
            {
                Lblerror.Visible = true;
                Lblerror.Text = "No Record Found";
            }
            else
            {
                Lblerror.Visible = false;
            }

            PagingBar2.Bind(candidatesDetails, ref gvMainCandidates);
            uPnlGrid1.Update();
            uPnlNavigation1.Update();
            gvMainCandidates.Visible = true;
            lblErrMsg1.Visible = false;
            PagingBar2.Visible = false;
            if (gvMainCandidates.Rows.Count <= 0)
            {
                lblErrMsg1.Text = "No record found.";
                lblErrMsg1.Visible = true;
                gvMainCandidates.Visible = false;
                PagingBar2.Visible = false;
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
    protected void ShowEditMode()
    {
        try
        {
            divfilter.Visible = false;
            //btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;           
            btnSave.Text = "Update";
            lblHeading.Text = "Check Candidates Details for Protsahan Puruskar";
           
            using (EConnectContext context = new EConnectContext())
            {
                Int32 Id = 0;
                Int64 RegId = Convert.ToInt32(Request.QueryString["Regno"]);
                Int32 CourseId = Convert.ToInt32(Request.QueryString["courseID"]);
                if (RegId != 0 && CourseId != 0)
                {
                    //Int64 ent = Convert.ToInt64(Session["EntityID"]);
                    REGID.Value = RegId.ToString();
                    CsId.Value = CourseId.ToString();
                                       
                    var registration = (from c in context.RegistrationDetails.AsNoTracking()
                                        join d in context.CourseRegistrationApplications on c.CandidateID equals d.CandidateID
                                        where //c.RegistrationNo == d.RegisteredCourseRegistrationNo && 
                                        c.CourseID == d.CourseID
                                        && d.FinalSubmitted == true && c.RegistrationNo == RegId && c.CourseID == CourseId                                     
                                       select d).FirstOrDefault();
                    txtCandNamesV.Text = registration.Name.ToString().ToUpper();
                    //TxtRegV.Text = registration.RegisteredCourseRegistrationNo.ToString();
                    TxtRegV.Text = RegId.ToString();                    
                    txtLevelV.Text = registration.Course.Name.ToString();

                    ddlCastCategory.SelectedValue = registration.CastCategoryID.ToString();
                    if (registration.IsHandicaped.ToString() == "True")
                    {
                        ddlPHCategory.SelectedValue = "1";
                    }
                    else
                    {
                        ddlPHCategory.SelectedValue = "0";
                    }
                    ddlGender.SelectedItem.Text = registration.Gender.ToString();


                }
                else
                {
                    throw new Exception("Something is wrong. Please contact administrator.");                    
                }

                
            };
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
            // context.Dispose();
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            if ((txtRemarks.Text.Trim() == "")||(txtRemarks.Text.Trim() == "0"))
            {              
                throw new Exception("Remarks can not be blank.");
                return;
            }
             BreadCrumb1.Render();
             using (EConnectContext context = new EConnectContext())
                {
                    Int32 castidForUpdation =0 ;
                    int phidForUpdation =99 ;
                    string GenderForUpdation ="NA";
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        Int64 registrationNo =Convert.ToInt64(TxtRegV.Text);
                      
                       // CourseRegistrationApplication cra = new CourseRegistrationApplication();
                        Int64 CID = Convert.ToInt64(Request.QueryString["key"].ToString());
                        Int32 Level = Convert.ToInt32(Request.QueryString["courseID"].ToString());

                        var registration = (from c in context.RegistrationDetails.AsNoTracking()
                                           join d in context.CourseRegistrationApplications on c.CandidateID equals d.CandidateID
                                           where //c.RegistrationNo == d.RegisteredCourseRegistrationNo && 
                                           c.CourseID == d.CourseID
                                           && d.FinalSubmitted == true && c.RegistrationNo == registrationNo && c.CourseID == Level
                                           select new
                                           {
                                               ID = d.ID,
                                               ApplID = c.CandidateID,
                                               Regno = c.RegistrationNo,
                                               CourseID = c.CourseID,
                                               CastID = d.CastCategoryID,
                                               phId = d.IsHandicaped.ToString() == "True" ? "1" : "0",
                                               GenderName = d.Gender
                                           }).FirstOrDefault();

                        Int32 castidDb = Convert.ToInt32(registration.CastID.ToString());
                        int phidDb = Convert.ToInt32(registration.phId.ToString());
                        string GenderDb = registration.GenderName.ToString().Trim();

                        Int32 castidForU =Convert.ToInt32(ddlCastCategory.SelectedValue);
                        int phidForU = Convert.ToInt32(ddlPHCategory.SelectedValue);
                        string GenderForU = ddlGender.SelectedItem.Text.Trim();

                        Int64 CandidateID = Convert.ToInt64(registration.ApplID.ToString());
                        Int32 courseIds = Convert.ToInt32(registration.CourseID.ToString());
                        string candName = txtCandNamesV.Text.Trim().ToUpper();

                        string Remarks = txtRemarks.Text.Trim().ToUpper();
                        if (castidDb!=castidForU)
                        {
                            castidForUpdation = castidForU;
                        }
                        if(phidDb!=phidForU)
                        {
                            phidForUpdation = phidForU;
                        }
                        if (GenderDb != GenderForU)
                        {
                            GenderForUpdation = GenderForU;
                        }
                        if (castidForUpdation == 0 && phidForUpdation == 99 && GenderForUpdation == "NA")
                        {
                            throw new Exception("NO CHANGES MADE [ Cast Category OR Handicapped OR Gender ]");
                            return;
                        }
                        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                        using (SqlConnection Conn = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("UpdateCandCrRegDetailsProtsahanPuruskar", Conn))
                            {
                                Conn.Open();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CID", CID);
                                cmd.Parameters.AddWithValue("@RegNo", Convert.ToInt64(registrationNo));
                                cmd.Parameters.AddWithValue("@CandidateID", CandidateID);
                                cmd.Parameters.AddWithValue("@courseIds", courseIds);
                                cmd.Parameters.AddWithValue("@candName", candName);
                                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                                cmd.Parameters.AddWithValue("@updatedBy", loginUserNo);                              
                                cmd.Parameters.AddWithValue("@castid", castidForUpdation);                                
                                cmd.Parameters.AddWithValue("@PHid", phidForUpdation);                                
                                cmd.Parameters.AddWithValue("@GenderName", GenderForUpdation);

                                cmd.Parameters.AddWithValue("@castidDb", castidDb);
                                cmd.Parameters.AddWithValue("@phidDb", phidDb);
                                cmd.Parameters.AddWithValue("@GenderDb", GenderDb);
  
                                cmd.ExecuteNonQuery();
                                strMessage = "Record updated.";
                            }
                        }
                       
                    }

                } 
            
             Int64 RegNo = Convert.ToInt64(Request.QueryString["Regno"].ToString());
             Int32 Levell = Convert.ToInt32(Request.QueryString["courseID"].ToString());

             Response.Redirect("CheckCandidateDetailsProtsahan.aspx?RegNoQ=" + RegNo + "&LevelQ=" + Levell + "&msg=" + strMessage, true);
               //Response.Redirect("NielitProjects.aspx?msg=" + strMessage, true);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
          protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Session["RegIdViewEdit"] = RegId.ToString();
        //Session["CourseIdViewEdit"] = RegId.ToString();  /Admin/CheckCandidateDetailsProtsahan.aspx
        //PagingBar1.CurrentPageIndex = 0; 
    Int64    RegNo = Convert.ToInt64(REGID.Value.ToString());
      Int32  Level = Convert.ToInt32(CsId.Value.ToString());
        //BreadCrumb1.Render();
        //divGrid.Visible = true;
        //BindGridView();
      Response.Redirect("CheckCandidateDetailsProtsahan.aspx?RegNoQ=" + RegNo + "&LevelQ=" + Level , true);
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

    protected void gvMainCandidates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    //Encryption url of hypelink field
            //    HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
            //    string href = hl.NavigateUrl;
            //    if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            //    {
            //        href += "&Id=" + Request.QueryString["Id"].ToString();
            //    }
            //    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
              

            //    e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();

            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMainCandidates_Sorting(object sender, GridViewSortEventArgs e)
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
        ddlLevel.SelectedValue = "0";
        TxtRegNo.Text = "";      
        Lblerror.Visible = false;
        divGrid.Visible = false;
      

    }
  
    protected void Rdsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            //Txtfilter.Text = "";
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
           // aceSearch.ContextKey = ddlcname.SelectedValue ;
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
  
}