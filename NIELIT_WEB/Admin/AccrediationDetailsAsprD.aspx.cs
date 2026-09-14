using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
public partial class AccrediationDetailsAsprD : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
        Int64 NielitCentrelinkedToCentreId = 0;
        Int32 NielitCentreIdFilter = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/AdminAccrediatedAsprDcenter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                hfAccreID.Value = Request.QueryString["key1"];
            }
            if (!Page.IsPostBack)

            {
                User objUser;
                NIELITMISContext context1 = new NIELITMISContext();
                EConnectContext context = new EConnectContext();
                objUser = new EConnect.URM.User();

                //User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                //var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                //Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                //NielitCentreIdFilter = NielitCentreId;               
                //NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                   // BindState();
                    FillCategories();
                    //FillCourses();
                    FillStatus();
                    FillNielitCentre();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCategories();
                    FillFilterCategory();
                    FillFilterStatus();
                    //FillCourses();
                    FillStatus();
                    FillNielitCentre();
                    //BindCity();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Accrediation Detail", "Admin/AccrediationDetailsAsprD.aspx?" + Request.QueryString.ToString(), ""));
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
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category.Distinct(), lst);
            };

            ddlcoursecategory.SelectedValue = "6";
            FillCourses();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");

                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);

                var CourseList = from p in context.Courses
 where p.CourseCategoryID == id && (p.ID == 1215|| p.ID == 1216|| p.ID == 133 || p.ID == 107 )
                                // where p.CourseCategoryID == id && (p.ID == 115 || p.ID == 149 || p.ID == 133 || p.ID == 107 )
				 //Added 8 Feb 2019
                                && p.ShowOnWeb
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };

                                // select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlaccfor, CourseList, lst);
            };
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillNielitCentre()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
               
                ListItem lst = new ListItem("--Select One--", "0");

                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);

                var NielitCentreList = from p in context.NielitCentres
                                // where p.CourseCategoryID == id
                                     
                                
                                 select new { ValueField = p.ID, TextField = p.Name  };




                NielitCentreList = NielitCentreList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitCentre, NielitCentreList, lst);
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillStatus()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusList = from p in context.AccreditationStatus
                                 where p.ID == 4 || p.ID == 5
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlstatus, StatusList, lst);
            };
            ddlstatus.SelectedValue = "4";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var category = from p in context.CourseCategories
                               select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    category = category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategry, category.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterCourse(Int32 CategoryId)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var statelist = from p in context.Courses
                                where p.CourseCategoryID == CategoryId
                                orderby p.DisplayOrder
                                select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    statelist = statelist.Where(a => roleCourses.Contains(a.ValueField));
                }
                statelist = statelist.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcour, statelist, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterStatus()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var statelist = from p in context.AccreditationStatus
                                select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlsts, statelist, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //Added 20 May 2020 for blocking instt

    protected void chkBlocked_CheckedChanged(object sender, EventArgs e)
    {
        Session["blockFlag"] = true;
        if (chkBlocked.Checked == false)
            txtBlockDate.Text = "";
    }
    public DataTable FillnielitCenreddlID(Int64 instid, Int64 ccid)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select top 1 linkedToCentre  from InstituteAccrGenerated where Institute_ID=" + instid + " and CourseID= " + ccid, con))
                {
                    cmd.CommandType = CommandType.Text; 
                    //cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    //cmd.Parameters["@pCentreID"].Value = Convert.ToInt64(Session["EntityID"]);
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

    protected void ShowEditMode()
    {
        try
        {
           
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Accredited Centres";
            tblNavLinks.Visible = true;

            //Added 20 May 2020 for blocking instt
            Session["blockFlag"] = false;
            //
            Int64 linkcentreID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                //Institute ins;
                //ins = context.Institutes.Find(Convert.ToInt64(Request.QueryString["CId"]));
                Int32 AccId = Convert.ToInt32(Request.QueryString["Key"]);
                var Acccentre = (from p in context.AccreditationDetails
                              where p.ID == AccId
                              select new
                              {
                                  ID = p.ID,
                                  CourseCategoryID=p.CourseCategory.ID,
                                  CourseID=p.CourseID,
                                  AccreditationNumber=p.AccreditationNumber,
                                  EffectiveFromDate=p.EffectiveFromDate,
                                  EffectiveToDate=p.EffectiveToDate,
                                  AccreditationStatusID=p.AccreditationStatus.ID,
								   //Added 20 June 2019
                                  WithdrawlDate=p.WithdrawlDate ,
                                  instituteID=p.InstituteID,
								  //
                                  //Added 20 May 2020 for blocking instt
	                			    whetherBlock=p.tempBlocked,
				                    BlockedFromDate=p.BlockedFromDate
//
                              }).FirstOrDefault();

                Int64 ccid = Convert.ToInt64( Acccentre.CourseID.ToString());
                Int64 instid = Convert.ToInt64(Acccentre.instituteID.ToString());
                using (DataTable dt = FillnielitCenreddlID(instid, ccid))
                {
                    if (dt.Rows.Count > 0)
                    {                        
                         linkcentreID =Convert.ToInt64( dt.Rows[0]["linkedToCentre"].ToString());
                    }
                }
                ddlNielitCentre.SelectedValue = linkcentreID.ToString();

                ddlcoursecategory.SelectedValue = Acccentre.CourseCategoryID.ToString();
                ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);  
                ddlcoursecategory.Enabled = false;
                ddlaccfor.SelectedValue = Acccentre.CourseID.ToString();
                ddlaccfor.Enabled = false;
                ddlstatus.SelectedValue = Acccentre.AccreditationStatusID.ToString();
                //ddlstatus.Enabled = false;
                txtaccno.Text = Acccentre.AccreditationNumber.ToString();
                if(Acccentre.EffectiveFromDate.HasValue)
                    txteffectivefrom.Text = Acccentre.EffectiveFromDate.Value.ToString("dd-MMM-yyyy");
                if(Acccentre.EffectiveToDate.HasValue)
                    txteffectiveto.Text = Acccentre.EffectiveToDate.Value.ToString("dd-MMM-yyyy");
					
					 //Added 20 June 2019
                if (Acccentre.WithdrawlDate .HasValue)
                    txtWithdrawldate .Text  = Acccentre.WithdrawlDate .Value.ToString("dd-MMM-yyyy");
               
                    if (ddlstatus.SelectedValue.ToString() == "5")
                    {
						//Added 6 June 2020
                withdrawal.Visible = true;
                withdrawal1.Visible = true;
                blocking.Visible = false;
                chkBlocked.Visible = false;
                lblBlockDate.Visible = false;
                txtBlockDate.Visible = false;
                //
                        txtWithdrawldate.Visible = true;
                        lblWithdrawlDate.Visible = true;
                    }
                    else
                    {
						//Added 6 June 2020
                withdrawal.Visible = false;
                withdrawal1.Visible = false;
                blocking.Visible = true;
                chkBlocked.Visible = true;
                chkBlocked.Checked = true;
                lblBlockDate.Visible = true;
                txtBlockDate.Visible = true;
                //
                        txtWithdrawldate.Visible = false;
                        lblWithdrawlDate.Visible = false;
                        txtWithdrawldate.Text = "";
                    }
                //

                    //Added 20 May 2020 for blocking instt
                    if (Acccentre.AccreditationStatusID!=5)
                    {

                        if (Acccentre.whetherBlock)
                        {
                            withdrawal.Visible = false;
                            withdrawal1.Visible = false;
                            blocking.Visible = true;
                            chkBlocked.Visible = true;
                            chkBlocked.Checked = true;
                            lblBlockDate.Visible = true;
                            txtBlockDate.Visible = true;
                            txtBlockDate.Text = Acccentre.BlockedFromDate.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            withdrawal.Visible = false;
                            withdrawal1.Visible = false;
                            blocking.Visible = true;
                            chkBlocked.Checked = false;
                            chkBlocked.Visible = true;
                            lblBlockDate.Visible = true;
                            txtBlockDate.Visible = true;
                            txtBlockDate.Text = "";
                        }

                    }
                    else
                    {
                        //If institute withdrawn , cannot be blocked.
                        withdrawal.Visible = true;
                        withdrawal1.Visible = true;
                        blocking.Visible = false;
                        chkBlocked.Visible = false;
                        lblBlockDate.Visible = false;
                        txtBlockDate.Visible = false;

                    }

                    //


                //Updating breadscrumb
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Acccentre.AccreditationNumber, "Admin/AccrediationDetailsAsprD.aspx?" + Request.QueryString.ToString(), ""));
                ViewState["LastModifiedOn"] = DateTime.Now;

                if (!UserManager.HasRight(currentRoleId, enmRight.Edit, "Admin/AdminAccrediatedAsprDcenter.aspx"))
                {
                    btnSave.Visible = false;
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
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            Int64 instituteId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                instituteId = Convert.ToInt64(Request.QueryString["key1"]);
            }
            Int32 CourseCategoryID = 0;
            Int32 courseID = 0;
            Int32 StatusID = 0;
            if (ddlcategry.SelectedValue != "0")
                CourseCategoryID = Convert.ToInt32(ddlcategry.SelectedValue);
            if (ddlcour.SelectedValue != "0")
                courseID = Convert.ToInt32(ddlcour.SelectedValue);
            if (ddlsts.SelectedValue != "0")
                StatusID = Convert.ToInt32(ddlsts.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            ucSearchBar.AutoCompleteContextKey = instituteId.ToString();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var centre = from s in context.AccreditationDetails
                         where s.InstituteID == instituteId
                         select new { 
                          ID = s.ID, 
                          AccreditationNumber = s.AccreditationNumber, 
                          EffectiveFromDate = s.EffectiveFromDate, 
                          EffectiveToDate = s.EffectiveToDate, 
                          Name = s.Course.Name, 
                          CourseID = s.CourseID, 
                          AccreditationStatusID=s.AccreditationStatusID,
                          SNAME=s.AccreditationStatus.Name,
                          CourseCategoryID = s.CourseCategoryID,
                          //Added 22May 2020 for institute blocking
                          Temp_Blocked=s.tempBlocked
                          //
                         };
            if (CourseCategoryID != 0)
            {
                centre = centre.Where(s => s.CourseCategoryID == CourseCategoryID);
            }
            if (courseID != 0)
            {
                centre = centre.Where(s => s.CourseID == courseID);
            }
            if (StatusID != 0)
            {
                centre = centre.Where(s => s.AccreditationStatusID == StatusID);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.AccreditationNumber.ToUpper().Contains(searchString));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.ID);
                        else
                            centre = centre.OrderBy(s => s.ID);
                        break;
                    case "AccreditationNumber":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.AccreditationNumber);
                        else
                            centre = centre.OrderBy(s => s.AccreditationNumber);
                        break;
                    case "EffectiveFromDate":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.EffectiveFromDate);
                        else
                            centre = centre.OrderBy(s => s.EffectiveFromDate);
                        break;
                     case "EffectiveToDate":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.EffectiveToDate);
                        else
                            centre = centre.OrderBy(s => s.EffectiveToDate);
                        break;
                   
                    default:
                        centre = centre.OrderBy(s => s.ID);
                        break;
                }
            }
            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                centre = centre.Where(a => roleCourses.Contains(a.CourseID));
            }
            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                centre = centre.Where(a => roleCourses.Contains(a.CourseCategoryID));
            }
            PagingBar1.Bind(centre, ref gvMain);
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
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.New, "Admin/AdminAccrediatedAsprDcenter.aspx"))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to add new record.", true);
                    return;
                }
                FillCategories();
                //FillCourses();
                FillStatus();
               // GenerateAccrNumber();
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                //Change the heading text as required
                lblHeading.Text = "Accredited Centre";
                
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Accredition", "", ""));
                //BreadCrumb1.Items.Add(new BreadCrumbItem("Accredited Centres", "adminaccrediatedcenter.aspx", ""));
                //BreadCrumb1.Items.Add(new BreadCrumbItem(hfAccName.Value, "adminaccrediatedcenter.aspx?qs=Nt75N5U5FCNFdeQ7non68XjyxPvnC/MuGmQC2f8I9NI=", ""));
                //BreadCrumb1.Items.Add(new BreadCrumbItem("Accrediatin Detail", "accrediationdetails.aspx", ""));
                //BreadCrumb1.Items.Add(new BreadCrumbItem("New Accredition", "#", ""));
                //BreadCrumb1.Render();

            }
            else
            {
                if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
                {
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AccrediationDetailsAsprD.aspx?key1=" + Request.QueryString["key1"].ToString()), true);
                }
                else
                {
                    Response.Redirect("accrediationdetails.aspx", true);
                }
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
    protected void ddlaccfor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //int id1 = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            //ddlaccfor.Items.Clear();
            //FillCourses();
            GenerateAccrNumber();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GenerateAccrNumber()
    {
       
        using (EConnectContext context = new EConnectContext())
        {
            Institute ins;
            ins = context.Institutes.Find(Convert.ToInt64(Request.QueryString["key1"]));
            Int64 instId=ins.ID;
            Int64 AutoAccrID = 60001;
            string FinalAccNumber = "";
            string statecode = "AA";            
            string newTP = "N", accrSeries="6";
            
            var InstituteAccrLvl = (from c in context.AccreditationDetails
                                    where c.CourseCategoryID == 1 && c.ID == instId
                                    select new
                                    {
                                        ID = c.ID

                                    }).FirstOrDefault();

            if (InstituteAccrLvl != null)
            {
                newTP = "E";
            }
            else
            {
                newTP = "N";
            }

            var InstituteIId2 = (from c in context.Institutes                                
                                 join s in context.Locations on c.StateID equals s.ID

                                 where  c.ID == instId  //&& c.ID > 62100000 && c.ID.ToString().StartsWith("621")
                                 select new
                                 {
                                     ID = c.ID,
                                     StateCode = s.Code
                                 }).FirstOrDefault();
          
            if (InstituteIId2 != null)
            {
                statecode = InstituteIId2.StateCode.ToString();
            }
                        
            var InstituteIId1 = (from c in context.Institutes
                                 join a in context.AccreditationDetails on c.ID equals a.InstituteID
                                 join s in context.Locations on c.StateID equals s.ID

                                 where a.CourseCategoryID == 6 && c.ID == instId 
                                 select new
                                 {
                                     ID = c.ID,
                                     StateCode= s.Code
                                 }).FirstOrDefault();

            
            if (InstituteIId1 != null)
            {
                statecode = InstituteIId1.StateCode.ToString();
            }
          
            Int32 courseid = Convert.ToInt32(ddlaccfor.SelectedValue);
            Int32 ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            string FullAccr = "";

            var AccrNumber = (from c in context.AccreditationDetails
                              //where c.CourseCategoryID == 6 && c.AccreditationNumber.ToString().StartsWith(statecode) && c.InstituteID == instId                               
                              where c.CourseCategoryID == 6  && c.AccreditationNumber.ToString().Substring(4, 1).StartsWith(accrSeries) && c.InstituteID == instId
                                 select new
                                 {
                                     ID = c.ID,
                                     AccrNum = c.AccreditationNumber
                                 }).ToList();
            if (AccrNumber.Count > 0)
            {
                var AccrNumber1 = AccrNumber.OrderByDescending(x => x.ID).First();// UPN-00001-109
                FullAccr = AccrNumber1.AccrNum.ToString();

                int firsthypen = FullAccr.IndexOf('-');
                int secondhypen = FullAccr.LastIndexOf('-');

                string InstituteIdSeries = FullAccr.Substring(firsthypen + 1, secondhypen - 4);  
                FinalAccNumber = statecode + newTP + "-" + InstituteIdSeries.ToString() + "-" + courseid.ToString();
            }
            else
            {               
                var AccrNumberAuto = (from c in context.AccreditationDetails
                                      join i in context.Institutes on c.InstituteID equals i.ID 
                                      join l in context.Locations on i.StateID equals l.ID 
                                      //where c.CourseCategoryID == 6 && c.AccreditationNumber.ToString().StartsWith(statecode) 
                                      where c.CourseCategoryID == 6 && c.AccreditationNumber.ToString().Substring(3, 1).StartsWith("-")  && c.AccreditationNumber.ToString().Substring(4, 1).StartsWith(accrSeries) && l.Code == statecode
                                  select new
                                  {
                                      ID = c.ID,
                                      AccrNum = c.AccreditationNumber
                                  }).ToList();
                if (AccrNumberAuto.Count>0)
                {
                    var AccrNumber1 = AccrNumberAuto.OrderByDescending(x => x.AccrNum ).First();// UPN-00001-109
                    FullAccr = AccrNumber1.AccrNum.ToString();

                    int firsthypen = FullAccr.IndexOf('-');
                    int secondhypen = FullAccr.LastIndexOf('-');

                    string InstituteIdSeries = FullAccr.Substring(firsthypen + 1, secondhypen - 4);
                    AutoAccrID = Convert.ToInt64(InstituteIdSeries) + 1;

                    FinalAccNumber = statecode + newTP + "-" + AutoAccrID.ToString() + "-" + courseid.ToString();
                }
                else
                {
                    FinalAccNumber = statecode + newTP + "-" + AutoAccrID.ToString() + "-" + courseid.ToString();
                }               
            }    
            txtaccno.Text = FinalAccNumber;       
        }
    }
    public void UpdateInstituteAccrGenerated(Int64 id, Int64 COURSEID, Int64 linktoCentre)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Ins_InstituteAccrGenerated", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@institueid", id);
                    //cmd.Parameters.AddWithValue("@projectid", Convert.ToInt64(Session["Candidate_ID"].ToString()));
                    //cmd.Parameters.AddWithValue("@courseid", Convert.ToInt64(Session["Exam_ID"].ToString()));
                    //cmd.Parameters.AddWithValue("@linkToCenreid", Convert.ToInt64(Session["Exam_ID"].ToString()));
                    cmd.Parameters.AddWithValue("@projectid", 1);
                    cmd.Parameters.AddWithValue("@courseid", COURSEID);
                    cmd.Parameters.AddWithValue("@linkToCenreid", linktoCentre);
                    cmd.Parameters.AddWithValue("@enterby", Convert.ToInt32(Session["UserID"]));                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        //Added for blocking instt
        context = new EConnectContext();
        //
        try
        {
		
              //Added 20 June 2019
            if (ddlstatus.SelectedValue == "5")
            {
                if (txtWithdrawldate.Text.Trim().Length == 0)
                {
                    ShowAlert("Please enter withdrawl date");
                    return;
                }
				if(txteffectivefrom.Text.Trim().Length!=0)
				{
                if (Convert.ToDateTime(txtWithdrawldate.Text) < Convert.ToDateTime(txteffectivefrom.Text))
                {
                    ShowAlert("Withdrawl date cannot be less than effectivefrom");
                    return;
                }
				}
            }
            //

            //Added 20 June 2019
			if(txteffectivefrom.Text.Trim().Length!=0  && txteffectiveto.Text.Trim().Length!=0 )
			{
             if (Convert.ToDateTime(txteffectiveto .Text ) < Convert.ToDateTime(txteffectivefrom.Text))
             {
                ShowAlert("Effective To date cannot be less than effectivefrom");
                return;
             }
			 }

            //
            //Added 20 May 2020 for instt blocking
            if (chkBlocked.Checked)
            {
                if (txtBlockDate.Text.Trim().Length == 0)
                {
                    ShowAlert("Please enter Block From date");
                    return;
                }

                if (Convert.ToDateTime(txtBlockDate.Text) < System.DateTime.Today)
                {
                    ShowAlert("Block From date cannot be less than current date");
                    return;
                }
            }

            //		

            BreadCrumb1.Render();
           // context = new EConnectContext();
            Institute ins;
            ins = context.Institutes.Find(Convert.ToInt64(Request.QueryString["key1"]));
            Int32 ccid = Convert.ToInt32(ddlaccfor.SelectedValue);
            AccreditationDetail objAccreDetail;
        //Added 20 May 2020 for instt blocking
            int districtid = 0;
            string districtname = "";
            var DistrictDetails = (from s in context.Institutes
                                   join l in context.Locations on s.DistrictID equals l.ID

                                   where s.ID == ins.ID
                                   select new
                                   {
                                       District = l.Name,
                                       DistricID = s.DistrictID
                                   }).FirstOrDefault();
            if (DistrictDetails != null)
            {
                districtid = Convert.ToInt32(DistrictDetails.DistricID);
                districtname = DistrictDetails.District.ToString();
            }
            NIELITMIS CheckDistrict = new NIELITMIS();

            using (DataTable dt = CheckDistrict.DistrictsExists(districtid))
            {
                if (dt.Rows.Count == 0)
                {
                    ShowAlert(districtname + "  does not fall in the category of Aispirational District.");
                    return;
                }
            }
            using (TransactionScope scope = new TransactionScope())
            {
                //
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    var existCourse = (from r in context.AccreditationDetails
                                       where r.InstituteID == ins.ID && r.CourseID == ccid //&& (r.AccreditationStatusID == 4 || r.AccreditationStatusID == 3 || r.AccreditationStatusID == 2)
                              select new
                               {
                                   ID = r.ID,
                                   Course = r.CourseID                                   
                               }).ToList();
                    if (existCourse.Count > 0)
                    {
                        //strMessage = "Course is already Exists.";
                        //Response.Redirect("AccrediationDetailsAsprD.aspx?msg=" + strMessage, true);
                        lblerror.Text = "**Accreditation already granted for the course.";
                        return;
                    }
                    else 
                    {
                        lblerror.Text = "";
                        objAccreDetail = new EConnect.NIELIT.AccreditationDetail();
                        objAccreDetail.InstituteID = ins.ID;
                        objAccreDetail.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                        objAccreDetail.CourseID = Convert.ToInt32(ddlaccfor.SelectedValue);
                        objAccreDetail.AccreditationStatusID = Convert.ToInt32(ddlstatus.SelectedValue);
                        objAccreDetail.AccreditationNumber = txtaccno.Text.ToString();
                        objAccreDetail.EffectiveFromDate = Convert.ToDateTime(txteffectivefrom.Text);
                        objAccreDetail.EffectiveToDate = Convert.ToDateTime(txteffectiveto.Text);
                        context.AccreditationDetails.Add(objAccreDetail);
                        //Added 20 June 2019
                        if (txtWithdrawldate.Text.Trim().Length != 0)
                            objAccreDetail.WithdrawlDate = Convert.ToDateTime(txtWithdrawldate.Text);
                        //
                        //Added 20 May 2020 for blocking instt
                        if (chkBlocked.Checked)
                        {
                            objAccreDetail.tempBlocked = true;

                            if (txtBlockDate.Text.Trim().Length != 0)
                                objAccreDetail.BlockedFromDate = Convert.ToDateTime(txtBlockDate.Text);
                        }
                        else
                            objAccreDetail.tempBlocked = false;


                        NIELITMISContext context1 = new NIELITMISContext();

                        objAccreDetail.whetherProjectBased = true;
                        //
                        context.SaveChanges();
                       strMessage = "New record saved.";
                        string CentreName = "";
                        string EmailAddress = "";
                        string accrno = "";
                        string courseName = "";
                        EConnectContext context2 = new EConnectContext(); 
                     var    EmailAddressAccrGenration = (from c in context2.AccreditationDetails join 
                                i in context2.Institutes on c.InstituteID equals i.ID
                                where i.ID==ins.ID                                 
                                  select new

                                   {
                                       ID = c.ID,
                                       EmailAddress = i.EmailAddress1,
                                       CentreName=i.Name
                                      
                                   }).ToList().FirstOrDefault();
                     if (EmailAddressAccrGenration != null)
                     {
                       CentreName = EmailAddressAccrGenration.CentreName.ToString();
                          EmailAddress = EmailAddressAccrGenration.EmailAddress.ToString();
                          accrno = txtaccno.Text.ToString();
                          courseName = ddlaccfor.SelectedItem.Text;
                     }
                   //  String EmailMsg = "Dear " + CentreName.ToUpper() + ",  Course Name  : " + courseName + "  for accreditation number is :.  " + accrno + ". Please note for future reference.";
					 String EmailMsg = "Dear " + CentreName.ToUpper() + ",  Accreditation number for Course : " + courseName + "  is :  " + accrno + ". Please note this for future reference.";


                        //sending Email 
                     if (EmailAddress.ToString().Length > 0)
                        {
                            try
                            {
                                EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, EmailAddress.ToString());
                                mail.Send();
                                //strMessage = "New record saved.";
                            }
                            catch { ShowAlert("Accreditation Details are sent on E-mail."); }
                        }

                    }

                    //Int64 COURSEID= Convert.ToInt64(ddlaccfor.SelectedValue);
                    //Int64 Centreid=Convert.ToInt64(Session["EntityID"]);
                    //UpdateInstituteAccrGenerated(ins.ID, COURSEID, Centreid);
                }
                else
                {
                    objAccreDetail = context.AccreditationDetails.Find(Convert.ToInt32(Request.QueryString["key"]));
                    //Added 20 May 2020 for instt blocking
                    bool x = Convert.ToBoolean(Session["blockFlag"]);
                    //Added 20 May 2020
                    if (Convert.ToBoolean(Session["blockFlag"]))
                    {
                        //Add record in history table keeping transaction on
                        AccreditationDetailHistory accrediationdetailhistory = new AccreditationDetailHistory();
                        accrediationdetailhistory.InsttAccrDetailID = Convert.ToInt32(Request.QueryString["key"]);
                        accrediationdetailhistory.InstituteID = ins.ID;
                        accrediationdetailhistory.CourseCategoryID = Convert.ToInt32(objAccreDetail.CourseCategoryID);
                        accrediationdetailhistory.CourseID = objAccreDetail.CourseID;
                        accrediationdetailhistory.AccreditationStatusID = objAccreDetail.AccreditationStatusID;
                        accrediationdetailhistory.AccreditationNumber = objAccreDetail.AccreditationNumber;
                        accrediationdetailhistory.EffectiveFromDate = Convert.ToDateTime(objAccreDetail.EffectiveFromDate);
                        accrediationdetailhistory.EffectiveToDate = Convert.ToDateTime(objAccreDetail.EffectiveToDate);
                        if (objAccreDetail.WithdrawlDate != null)
                            accrediationdetailhistory.WithdrawlDate = objAccreDetail.WithdrawlDate;
                        accrediationdetailhistory.tempBlocked = objAccreDetail.tempBlocked;
                        //if (chkBlocked.Checked == true)
                        accrediationdetailhistory.BlockedFromDate = objAccreDetail.BlockedFromDate;
                        if (objAccreDetail.tempBlocked == true && Convert.ToBoolean(Session["blockFlag"]) == true)
                        {
                            if (objAccreDetail.BlockedFromDate > System.DateTime.Now)
                                accrediationdetailhistory.BlockedToDate = objAccreDetail.BlockedFromDate;
                            else
                                accrediationdetailhistory.BlockedToDate = System.DateTime.Now;
                        }
                        accrediationdetailhistory.updatedBy = Convert.ToInt32(Session["UserID"]);
                        context.AccreditationDetailHistory.Add(accrediationdetailhistory);
                        context.SaveChanges();
                    }

                    objAccreDetail.InstituteID = ins.ID;
                    objAccreDetail.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                    objAccreDetail.CourseID = Convert.ToInt32(ddlaccfor.SelectedValue);
                    objAccreDetail.AccreditationStatusID = Convert.ToInt32(ddlstatus.SelectedValue);
                    objAccreDetail.AccreditationNumber = txtaccno.Text.ToString();
                    objAccreDetail.EffectiveFromDate = Convert.ToDateTime(txteffectivefrom.Text);
                    objAccreDetail.EffectiveToDate = Convert.ToDateTime(txteffectiveto.Text);
                    //Added 20 June 2019
                    if (txtWithdrawldate.Text.Trim().Length != 0)
                        objAccreDetail.WithdrawlDate = Convert.ToDateTime(txtWithdrawldate.Text);
                    //

                    //Added 20 May 2020 for instt blocking
                    if (chkBlocked.Checked)
                    {
                        objAccreDetail.tempBlocked = true;

                        if (txtBlockDate.Text.Trim().Length != 0)
                            objAccreDetail.BlockedFromDate = Convert.ToDateTime(txtBlockDate.Text);
                    }
                    else
                    {
                        objAccreDetail.tempBlocked = false;
                        objAccreDetail.BlockedFromDate = null;
                    }


                    //
                   objAccreDetail.whetherProjectBased= true;
                    strMessage = "Record updated.";
                    context.SaveChanges();
                }
               
                scope.Complete();
            }

            Int64 COURSEID = Convert.ToInt64(ddlaccfor.SelectedValue);
            Int64 Centreid = Convert.ToInt64(Session["EntityID"]);
            Int64 linktoCentre = Convert.ToInt64(ddlNielitCentre.SelectedValue);
            UpdateInstituteAccrGenerated(ins.ID, COURSEID, linktoCentre);
            if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AccrediationDetailsAsprD.aspx?key1=" + Request.QueryString["key1"].ToString()), true);              
            }
            else
            {
                Response.Redirect("AccrediationDetailsAsprD.aspx?msg=" + strMessage, true);
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
            ddlcategry.SelectedValue = "0";
            ddlcour.SelectedValue = "0";
            ddlsts.SelectedValue = "0";
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
                string href = hl.NavigateUrl;

                if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
                {                
                    href += "&key1=" + Request.QueryString["key1"].ToString();                 
                }
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&key1=" + Request.QueryString["key1"].ToString());
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;
                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = hl.NavigateUrl;
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
    public static String[] GetSearchText(String prefixText, Int32 count, string contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            Int32 instID = Convert.ToInt32(contextKey.ToString());
            string searchString = prefixText.Trim().ToUpper();
            var centre = from s in context.AccreditationDetails
                         where s.InstituteID == instID
                         orderby s.AccreditationNumber
                         select new { AccreditationNumber = s.AccreditationNumber };
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.AccreditationNumber.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.AccreditationNumber);
            foreach (var course in centre)
            {
                items.Add(course.AccreditationNumber);
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
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AccrediationDetailsAsprD.aspx?key1=" + Request.QueryString["key1"].ToString()), true);
            }
            else
            {
                Response.Redirect("accrediationdetails.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //int id1 = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            ddlaccfor.Items.Clear();
            FillCourses();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public AccreditationDetail objAccreDetail 
    { 
        get; set; 
    }
    protected void ddlcategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCategoryID = Convert.ToInt32(ddlcategry.SelectedValue);
            FillFilterCourse(courseCategoryID);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
//Added again on 7 Aug 2019
 protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlstatus.SelectedValue.ToString() == "5")
            {
				 //Added 6 June 2020
                withdrawal.Visible = true;
                withdrawal1.Visible = true;
                blocking.Visible = false;
                chkBlocked.Visible = false;
                lblBlockDate.Visible = false;
                txtBlockDate.Visible = false;
                //
                lblWithdrawlDate.Visible = true;
                txtWithdrawldate.Visible = true;
            }
            else
            {
				//Added 6 June 2020
                withdrawal.Visible = false;
                withdrawal1.Visible = false;
                blocking.Visible = true;
                chkBlocked.Visible = true;
                chkBlocked.Checked = true;
                lblBlockDate.Visible = true;
                txtBlockDate.Visible = true;
                //
                lblWithdrawlDate.Visible = false;
                txtWithdrawldate.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}