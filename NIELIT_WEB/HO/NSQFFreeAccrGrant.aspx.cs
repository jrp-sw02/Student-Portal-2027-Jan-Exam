using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using System.Text.RegularExpressions;
using EConnect.NIELIT;
using System.Web;
using System.Transactions;
using System.Data.Objects;
using EConnect;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;

public partial class HO_NSQFFreeAccrGrant : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;
    Int32 entityID = 0;
    Int32 insttID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        //cbNielitCentres.Attributes.Add("onclick", "checkBoxList1OnCheck(this);");
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            entityID = Convert.ToInt32(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                ShowAlert("Sorry! You don't have rights to view this page");
                return;
            }

            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    Institute instt = context.Institutes.Find(loginUser.UserRefNumber);
                    insttID =Convert.ToInt32 ( instt.ID);
                    txtDateto.Text = System.DateTime.Today.AddDays(-1).ToString("dd-MMM-yyyy");
                    BindGridAccrCourses();
                    // RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                       
                        FillCategoriesF(6);
                       
                        FillCourseFilter();
                        FillCourseBucket();
                        BindGridAccrCourses();
                        ShowEditMode();
                        //FillddlcentreName();
                       
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                          
                          
                            FillCategoriesF(6);
                           
                            FillCourseFilter();
                            FillCourseBucket();
                            BindGridAccrCourses();
                            
                           
                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Free NSQF Course Accr Grant", "HO/NSQFFreeAccrGrant.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                            }
                            else
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Free NSQF Course Accr Grant", "HO/NSQFFreeAccrGrant.aspx", ""));
                            }
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //if (e.Row.Cells[3].Text.Contains("01-Jan-1900"))
                //    e.Row.Cells[3].Text = "-";
                HyperLink h1 = ((HyperLink)e.Row.Cells[3].Controls[0]);
                if (h1 != null)
                    if (h1.Text == "01-Jan-1900")
                        h1.Text = "-";

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   


    protected void FillCategoriesF(int category)
    {
        try
        {
         //   ddlcoursecategory.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.IsActive == true
                                && p.ID == category 
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategoryF , Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   


   

    protected void FillCourseFilter()
    {
        try
        {
            ddlCourseNameF.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             join q in context.NSQFFreeCourseGrants 
                             on p.ID equals q.mappedCourseID
                             where p.IsActive == true
                              && p.CourseCategoryID == 6 
                             orderby (p.DisplayOrder)
                             select new { ValueField = p.ID, TextField = p.Name + " ( "+p.Code +")" };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseNameF, course.Distinct (), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourseBucket()
    {
        try
        {
           // ddlCourseMapped.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             where p.IsActive == true
                              && p.CourseCategoryID == 6
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name +"( "+p.Code +" )" };

              //  EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseMapped, course, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCourseReplaced()
    {
        try
        {
            //ddlCourseReplaced.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             where 
                             //p.IsActive == true
                             // &&
                             p.CourseCategoryID == 6
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name + "( " + p.Code + " )" };

               // EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseReplaced, course, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    ////protected void FillddlcentreName()
    ////{
    ////    try
    ////    {
    ////        ListItem lst1 = new ListItem("--Select One--", "0");
    ////            using (NIELITMISContext context = new NIELITMISContext())
    ////            {                    
    ////                ddlCenter.ClearSelection();

    ////                var Center = from t in context.NielitCentres                                 
    ////                             orderby (t.Name)
    ////                             select new { ValueField = t.ID, TextField = t.Name };
    ////                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst1);
    ////            }
            
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowAlert(ex.Message, true);
    ////    }
    ////}
  /*  protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }*/
    protected void BindGridAccrCourses()
    {
        try
        {
            lblError.Visible = false;
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string CourseName = "NA";
            string CCat = "NA";
            if (ddlcoursecategoryF.SelectedValue != "0")
                CCat = ddlcoursecategoryF.SelectedItem.Text;

            if (ddlCourseNameF.SelectedValue != "0")
                CourseName = ddlCourseNameF.SelectedItem.Text;
            
            DataTable dt = new DataTable();
            
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("getGridNSQFFreeCoursesGrant", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(dt);
            }
            con.Close();

            if (dt.Rows.Count > 0)
            {
                var AccrCourses = (from p in dt.AsEnumerable()
                                   select new
                                   {
                                       id = p.Field<Int64>("ID"),
                                       AccrNo = p.Field<string>("Accreditation_Number"),
                                       Status = p.Field<string>("Status"),
                                       InstName = p.Field<string>("InstName"),
                                       InstAddress = p.Field<string>("InstAddress"),
                                       EffectiveFrom = p.Field<DateTime?>("EffectiveFrom"),
                                       EffectiveTo = p.Field<DateTime?>("EffectiveTo"),
                                       grantDate = p.Field<DateTime?>("grantDate"),
                                       course = p.Field<string>("course"),
                                       validity=p.Field <DateTime>("CourseValidity")
                                   });

                if (!String.IsNullOrEmpty(searchString))
                {
                    AccrCourses = AccrCourses.Where(s => s.InstName .ToUpper().Contains(searchString));
                }

                if (CCat != "NA" && CourseName != "NA")
                {
                    AccrCourses = AccrCourses.Where(s => s.course == CourseName);
                }

                PagingBar1.Bind(AccrCourses, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                divNavigation.Visible = true;
                PagingBar1.Visible = true;
                lblError.Visible = false;
                gvMain.Visible = true;
                uPnlNavigation.Visible = true;
		 		PagingBar1.Visible = true;
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    gvMain.Visible = false;
                    //divNavigation.Visible = false;
			 		PagingBar1.Visible = false;
                }

            }
            else
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
                gvMain.Visible = false;
                uPnlNavigation.Visible = false;
                //divNavigation.Visible = false;
		 		PagingBar1.Visible = false;
            }

            //gvMain.DataSource = dt;
            //gvMain.DataBind();
            //uPnlGrid.Update();
            //uPnlNavigation.Update();

            

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

        protected void grdNSQFCoursesForAccr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
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

           //  BindGridAccrCoursesGrant();

                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;

             
        }
        catch (Exception ex)
        {
            BreadCrumb1.Render();
            throw ex;
        }
    }


    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("NSQFFreeAccrGrant.aspx", true);
    }

   

    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridAccrCourses ();
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
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
         //   BindGridAccrCoursesGrant();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
          
            //Change the heading text as required
            lblHeading.Text = "NSQF Course Accr Grant";
            
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NSQFFreeAccrGrant.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NSQFFreeAccrGrant.aspx", true);
            }
        }

    }

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
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
            BreadCrumb1.Render();
            ddlCourseNameF.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public string CheckExistsRecordCount(string myQuery)
    {
        string result = "0";
        try
        {
            // string result = "0";          
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
        catch (Exception exx)
        {
            return result;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext  context = new EConnectContext())
            {
           
                int vSelected = grdNSQFCoursesForAccr.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("chkGrant")).Checked);
                if (vSelected  == 0)
                {
                    ShowAlert("Please select at least one for grant!");
                    return;
                }
                if (txtEFileNo.Text == "")
                {
                    ShowAlert("Please enter EFile Number!");
                    return;

                }
                if (txtApprovalDate.Text == "")
                {
                    ShowAlert("Please enter Approval Date!");
                    return;
                }
                
                foreach (GridViewRow row in grdNSQFCoursesForAccr.Rows)
                {

                    
                    bool isChecked = row.Cells[9].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                         //Int32 id = 51; 
                        string instituteID = "NA";
                        Int64 instId = 0; 
                        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                            SqlConnection con = new SqlConnection(constr);
                            con.Open();
                            SqlTransaction trans=con.BeginTransaction ();
                       // Update grant table and grant in intitute_accreditation_detail table
                        try{
                            Label l1=(Label) row.FindControl ("lblID");
                            if(l1==null)
                            {
                                ShowAlert("Not found");
                                return;
                            }
                            Int32 id = Convert.ToInt32(l1.Text );
                            instId = id;
                            
                          //  con.Open();

                           var course = (from p in context.NSQFFreeCourseGrants 
                              where p.ID == id
                              select new
                              {
                                  ID = p.ID,
                                  //Name = c.Name,
                                  instituteID=p.InstituteID ,
                                  CourseID = p.mappedCourseID ,
                                
                              }).FirstOrDefault();
                          instituteID = course.instituteID.ToString();
                            string accrNo = GenerateAccrNumber(Convert.ToInt64 (course.instituteID),Convert.ToInt32 ( course.CourseID));
                           // trans=con.BeginTransaction ();

                            string GetCount = " SELECT Count(*)  FROM [NIELIT].[dbo].[NSQFFreeCourseGrant] WHERE instituteID= '" + instituteID + "'" +
                           " and grantDate is not null and isActive=1";
                            
                            Int32 ExistsRecordCount = Convert.ToInt32(CheckExistsRecordCount(GetCount));

                            if (ExistsRecordCount <5)
                            {  
                             using (SqlCommand Cmm = new SqlCommand("saveFreeNSQFCoursesAccr", con))
                                 {
                                     Cmm.Transaction = trans;
                                     Cmm.CommandType = CommandType.StoredProcedure;
                                     Cmm.Parameters.Add("@pID",SqlDbType.BigInt ).Value =id;
                                     Cmm.Parameters.Add("@pAccrNo", SqlDbType.VarChar, 20).Value = accrNo;
                                     Cmm.Parameters.Add("@pEFileNo", SqlDbType.VarChar, 100).Value = txtEFileNo.Text;
                                     Cmm.Parameters.Add("@pApprovalDate", SqlDbType.Date).Value = Convert.ToDateTime(txtApprovalDate.Text);
                                     Cmm.Parameters.Add("@pUserID",SqlDbType.BigInt ).Value =Convert.ToInt32 (Session["UserID"]);
                                     Cmm.ExecuteNonQuery();
                                 }
                                trans .Commit ();
                                con.Close();
                                strMessage = "New Record Saved";
                            }
                            else
                            {
                                ShowAlert("The Institute can be granted only 5 NSQF Free Courses.");
                                return;
                            }
                            
           }
           catch(Exception ex)
            {
               trans.Rollback ();
               con.Close();
           }

                        // EMAILHERE Deep add on 28 July 2022
                        var InstDetail = (from p in context.NSQFFreeCourseGrants
                                          join i in context.Institutes on p.InstituteID equals i.ID
                                          join l in context.Locations on i.StateID equals l.ID
                                          join c in context.Courses on p.mappedCourseID equals c.ID
                                          join d in context.Courses on p.accrCourseID equals d.ID
                                          join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                          where a.CourseID==p.accrCourseID  && p.ID == instId
                                      select new
                                      {
                                          ID = p.ID,
                                          //Name = c.Name,
                                          instituteID = p.InstituteID,
                                          CourseID = p.mappedCourseID,
                                          InstituteName=i.Name,
                                          //instAdd = i.AddressLine1 + ", " + !string.IsNullOrEmpty( i.AddressLine2),
                                          //instAdd = i.AddressLine1  + !string.IsNullOrEmpty(i.AddressLine2) ??  ", "+ i.AddressLine2.ToString() ?? "" ,
                                          instAdd = i.AddressLine1 + (!string.IsNullOrEmpty(i.AddressLine2) ? ", " + i.AddressLine2 : ""),
                                          InstCityPin=i.CityName + " - " + i.PinCode,
                                          InstState= l.Name ,
                                          CourseName=c.Name,
                                          Instemail=i.EmailAddress1,
                                          InstAccrLvl=d.Name,
                                          InstAccNumber=a.AccreditationNumber,                                          
                                      }).FirstOrDefault();


                        var InstDetail1 = (from p in context.NSQFFreeCourseGrants
                                          join i in context.Institutes on p.InstituteID equals i.ID
                                          join l in context.Locations on i.StateID equals l.ID
                                          join c in context.Courses on p.mappedCourseID equals c.ID
                                          join d in context.Courses on p.accrCourseID equals d.ID
                                          join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                          where  a.CourseID == p.mappedCourseID && p.ID == instId
                                          select new
                                          {
                                              ID = p.ID,
                                             
                                              InstAccrNSQF = a.AccreditationNumber,
                                          }).FirstOrDefault();

                        string subject = "Permission to conduct NIELIT NSQF Course(s) - " + InstDetail.CourseName + " based on " + InstDetail.InstAccrLvl + " accreditation – Reg.";
                        String EmailMsg = "<pre>" + InstDetail.InstituteName + ",<br/>" + InstDetail.instAdd + ",<br/>" + InstDetail.InstCityPin + ",<br/>" + InstDetail.InstState + "<br/>" + "<br/> <br/> Sir/Madam, <br/><br/> This is in reference to your application for grant of permission to conduct NSQF Aligned course(s) <b> " + InstDetail.CourseName + "</b>. <br/><br/>   NIELIT is please to inform you that your institute has been permitted to conduct NSQF Aligned course(s) based on <b> " + InstDetail.InstAccrLvl + "</b>. Accreditation vide Accreditation No. <b>" + InstDetail.InstAccNumber + "</b>. as per the “Terms and Conditions/undertaking” that has been agreed upon by you. <br/><br/> 2.	Your institute has been allotted Accr No. <b>" + InstDetail1.InstAccrNSQF + "</b>, which will remain valid till your institute holds valid accreditation of NIELIT’S O/A/B/C levels of courses/up to the validity of NSQF Aligned course(s) applied for, whichever is earlier. You are, accordingly allowed to field the candidates for the examination of NSQF Aligned course(s). This number should always be quoted for all correspondence with regard to NSQF courses.   <br/><br/> 3.	Please note that the permission is subject to the following in addition to the Terms and Conditions and any amendment /modifications thereto, as may be issued by NIELIT from time to time:<br/> <p><pre>  3.1	3.1	The permission granted to your institute to conduct NSQF will automatically be withdrawn on withdrawal of accreditation of O/A/B/C levels of courses, even if, the validity with respect to NSQF Aligned course(s) is not expired. <br/> 3.2	You will ensure availability of qualified faculty, hardware,  software etc. required for conducting the NSQF course(s);<br/> 3.3	3.3	Your facilities shall be open for inspection by the officials / experts deputed by NIELIT at any point of time; <br/> 3.4	Appearing in the Examination for NSQF Aligned course(s) by your students shall be subject to the receipt of duly completed Examination Forms along with the requisite Examination Fee as may be prescribed by the NIELIT from time to time in respect of each candidate; <br/> 3.5	Your institute shall strictly adhere to the cut off dates indicated in the calendar of events prescribed or otherwise fixed by the NIELIT and shall not seek any relaxation on this account. <br/> 3.6	Your institute shall not over charge examination fee from the candidates for NSQF Aligned course(s);   </b> <br/> 3.7	Your institute <b> ( Accr No.  " + InstDetail1.InstAccrNSQF + " )</b>  will not charge additional fee from candidates for distribution of any document forwarded by NIELIT to the institute for onward disbursement to the candidate’s viz. admit card/result/certificates. <br/> 3.8	3.8	Your institute will not, in anyway, misuse this permission and shall not indulge in unfair marketing practices by exaggerating the facilities available at its institute, which, in the opinion of NIELIT, amounts to misleading the public.<br/> 3.9	3.9	This permission in any case does not confer any right on your institute to enroll students for other Courses offered by NIELIT, unless specific accreditation/ permission is obtained for the purpose; <br/> 3.10  3.10	You will provide PCs with required hardware and software, space, support, manpower, faculty and other amenities including all cooperation at no cost to the candidates as well as external examiners(s) and any other person(s) appointed by NIELIT for conduct of examinations of NSQF Aligned course(s).</pre> </p> 4. 	The institute will ensure that the NSQF course(s) in which the institute is taking admission is NSQF aligned and the course is valid at the time of admission. Qualification Files consisting of validity (Date of planned review) of a NSQF course is available at National Qualification Register (https://nqr.gov.in/). NIELIT will not be responsible for any liability, if an institute takes admission in a course which is not valid at the time of admission. <br/> 5.    NIELIT reserves the right to discontinue/amend this permission anytime without assigning any reason. In all matters related to this permission, the decision of Competent Authority, NIELIT will be final and binding. <br/> 6. 	<b>For fielding of students for forthcoming NSQF Examination, visit https://student.nielit.in. Initially  the user-id and password to log into the portal will be the Accr number; which will be activated within a weeks’ time from the date of issue of this letter. It is advised to change the password immediately after the first login.</b><br/> <br/> 7. 	For any further query regarding issues in login, examinations, fielding of candidates etc., please mail to accr-1@nielit.gov.in.  Please also visit our website https://nielit.gov.in/ for more details.<br/><br/><br/> This is a Computer generated letter does not need signature. </pre><br/><br/>FROM NIELIT HQ ";
			 if (InstDetail.Instemail == null)
                        {
                            ShowAlert("Email cannot be sent as emailid is not present");
                           continue;
                        }

                        
                        if (InstDetail.Instemail.Length > 0 && isChecked)
                        {
                            try
                            {
                               
                                EConnect.NIELIT.Email mail = new Email(subject, EmailMsg, InstDetail.Instemail);
                                mail.Send();
                               
                            }
                            catch { ShowAlert("Something is wrong!! please contact administrtor!!"); }
                        } 
                      
                        // add code end on 10 aug 2022 for email
				
                        }
 				else   //UnChecked
                        {
                        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                        SqlConnection con = new SqlConnection(constr);
                        con.Open();
                        SqlTransaction trans = con.BeginTransaction();
                        try
                            {
                            #region UnChecked
                            //string instituteID = "NA";
                            //Int64 instId = 0;

                            TextBox textboxRemaked = (TextBox)row.FindControl("txtRemarks");
                            Label l1 = (Label)row.FindControl("lblID");
                            if (l1 == null)
                                {
                                ShowAlert("Not found");
                                return;
                                }
                            Int32 id = Convert.ToInt32(l1.Text);

                            if (textboxRemaked.Text == "")
                                {
                               // ShowAlert("Please, Enter the Remarks For Request ID=" + id);
                               // return;
				 con.Close();
				continue;
                                }

                            string remarksEntered = "";
                            remarksEntered = Convert.ToString(textboxRemaked.Text);
                            int enterBy = 0;
                            enterBy = Convert.ToInt32(Session["UserID"]);
                            DateTime enterDate = DateTime.Now;

                            //var course = (from p in context.NSQFFreeCourseGrants
                            //              where p.ID == id
                            //              select new
                            //              {
                            //                  ID = p.ID,
                            //                  //Name = c.Name,
                            //                  instituteID = p.InstituteID,
                            //                  CourseID = p.mappedCourseID,

                            //              }).FirstOrDefault();
                            //instituteID = course.instituteID.ToString();
                            //string accrNo = GenerateAccrNumber(Convert.ToInt64(course.instituteID), Convert.ToInt32(course.CourseID));
                            // trans=con.BeginTransaction ();

                            using (SqlCommand Cmm = new SqlCommand("holdFreeNSQFCoursesAccr", con))
                                {
                                Cmm.Transaction = trans;
                                Cmm.CommandType = CommandType.StoredProcedure;
                                Cmm.Parameters.Add(new SqlParameter("@pID", SqlDbType.BigInt));
                                Cmm.Parameters["@pID"].Value = id;
                                //Cmm.Parameters.Add(new SqlParameter("@pInstID", SqlDbType.BigInt));
                                //Cmm.Parameters["@pInstID"].Value = instId;
                                Cmm.Parameters.Add(new SqlParameter("@pRemarks", SqlDbType.NVarChar));
                                Cmm.Parameters["@pRemarks"].Value = remarksEntered;
                                Cmm.Parameters.Add(new SqlParameter("@pUserID", SqlDbType.Int));
                                Cmm.Parameters["@pUserID"].Value = enterBy;
                                //Cmm.Parameters.Add(new SqlParameter("@enterDate", SqlDbType.DateTime));
                                //Cmm.Parameters["@enterDate"].Value = enterDate;
                                Cmm.ExecuteNonQuery();
                                }
                            trans.Commit();
                            con.Close();
                            strMessage = "New Record Saved in NSQFFreeCourseOnHold";
                            }
                        catch (Exception ex)
                            {
                            trans.Rollback();
                            con.Close();
                            ShowAlert(ex.Message);                            
                            }
                        #endregion
                    }
                }
            
        
                Response.Redirect("NSQFFreeAccrGrant.aspx?msg=" + strMessage, true);
            }
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
            BindGridAccrCourses();
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
                BindGridAccrCourses();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
          
            BindGridAccrCourses();
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            // BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
        }
    }
   
 

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NSQFFreeAccrGrant.aspx", true);
    }
    protected string GenerateAccrNumber(Int64 pInstID, Int32 courseid)
    {

        using (EConnectContext context = new EConnectContext())
        {
            Institute ins;
            ins = context.Institutes.Find(pInstID );
            Int64 instId = ins.ID;
            Int64 AutoAccrID = 00001;
            string FinalAccNumber = "";
            string statecode = "AA";
            string newTP = "F";

            var InstituteIId2 = (from c in context.Institutes
                                 join s in context.Locations on c.StateID equals s.ID

                                 where c.ID == instId  //&& c.ID > 62100000 && c.ID.ToString().StartsWith("621")
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
                                     StateCode = s.Code
                                 }).FirstOrDefault();


            if (InstituteIId1 != null)
            {
                statecode = InstituteIId1.StateCode.ToString();
            }

            string accrStart=statecode +"F";
          //Case 1To check if already accrediated for any free course
            string FullAccr = "";
            var AccrNumber = (from c in context.AccreditationDetails
                             where c.CourseCategoryID == 6 && c.AccreditationNumber.StartsWith(accrStart )  && c.InstituteID.ToString ().Trim () == pInstID.ToString ().Trim ()
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
                string accr = "";
                for (int i = InstituteIdSeries.Length; i < 5; i++)
                    accr = accr + "0";
                accr = accr + InstituteIdSeries ;

                FinalAccNumber = statecode + newTP + "-" + accr + "-" + courseid.ToString();
            }
            else
            {
               //Case 2 if any other institute for free course in that state
                var AccrNumberAuto = (from c in context.AccreditationDetails
                                      join i in context.Institutes on c.InstituteID equals i.ID
                                      join l in context.Locations on i.StateID equals l.ID
                                       where c.CourseCategoryID == 6 && c.AccreditationNumber.StartsWith(accrStart ) && l.Code == statecode
                                      select new
                                      {
                                          ID = c.ID,
                                          AccrNum = c.AccreditationNumber
                                      }).ToList();
                if (AccrNumberAuto.Count > 0)
                {
                    var AccrNumber1 = AccrNumberAuto.OrderByDescending(x => x.AccrNum).First();// UPN-00001-109
                    FullAccr = AccrNumber1.AccrNum.ToString();

                    int firsthypen = FullAccr.IndexOf('-');
                    int secondhypen = FullAccr.LastIndexOf('-');

                    string InstituteIdSeries = FullAccr.Substring(firsthypen + 1, secondhypen - 4);
                    AutoAccrID = Convert.ToInt64(InstituteIdSeries) + 1;

                    string accr = "";
                    for (int i = AutoAccrID.ToString().Trim().Length; i < 5; i++)
                        accr = accr + "0";
                    accr = accr + AutoAccrID.ToString().Trim();

                    FinalAccNumber = statecode + newTP + "-" + accr + "-" + courseid.ToString();
                }
                else
                {
                    string accr="";
                    for (int i = AutoAccrID.ToString().Trim ().Length; i < 5; i++)
                        accr = accr + "0";
                    accr = accr + AutoAccrID.ToString().Trim ();
                    // Case 3 if no institute for that state
                    FinalAccNumber = statecode + newTP + "-" + accr + "-" + courseid.ToString();
                }
            }
             return FinalAccNumber;
        }
    }
   
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        Int32 loginUserNo = 0, UserTypeId = 0;
        EConnectContext context = new EConnectContext();
        NIELITMISContext context1 = new NIELITMISContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();

            string searchString = prefixText.Trim().ToUpper();
            var instGrant = from c in context.NSQFFreeCourseGrants                         
                          join w in context.Institutes  on c.InstituteID equals w.ID
                          join x in context.AccreditationDetails on w.ID equals x.InstituteID
                          where c.grantDate !=null && c.isActive
                          && x.CourseID ==c.accrCourseID
                          select new { Name = w.Name + " ("+x.AccreditationNumber +" )" };
                        
            instGrant  = instGrant.Distinct();

            if (!String.IsNullOrEmpty(searchString))
            {
                instGrant  = instGrant.Where(s => s.Name.ToUpper().Contains(searchString) );
            }
           
           
            foreach (var inst in instGrant)
            {
                items.Add(inst.Name );              
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    { //vishal
    try
        {

        if (txttDateFrom.Text == "")
            {
            ShowAlert("Please Enter Request From Date!");
            return;
            }
        if (txtDateto.Text == "")
            {
            ShowAlert("Please Enter Request To Date!");
            return;
            }

        DateTime toDateShow = Convert.ToDateTime(txtDateto.Text);
        DateTime toDayDate = System.DateTime.Now.AddDays(-1);

        if (toDateShow < toDayDate)
            {
            BindGridAccrCoursesGrant();
            //txtDateto.Text = toDate.ToString("dd-MMM-yyyy");
            }
        else
            {
            ShowAlert("Please Enter Request To Date Less Than Today Date!");
            return;
            }
        }
    catch (Exception ex)
        {
        ShowAlert(ex.Message);
        }
    //vishal
       // BindGridAccrCoursesGrant();
       //if (grdNSQFCoursesForAccr.Rows.Count > 0)
        //{
        //    grdNSQFCoursesForAccr.Visible = true;
        //    FileBlock.Visible = true;
        //    FileLabelBlock.Visible = true;
        //    saveBlock.Visible = true;
        //}
        //else
        //{
        //    grdNSQFCoursesForAccr.Visible = false;
        //    FileBlock.Visible = false;
        //    FileLabelBlock.Visible = false;
        //    saveBlock.Visible = false;
        //}
    }
protected void BindGridAccrCoursesGrant()
    {
        try
        {
            lblError.Visible = false;
           

            DataTable dt = new DataTable();

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("getGridNSQFFreeCoursesApplied", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                Cmm.Parameters.Add("@vRequestDateFrom", SqlDbType.Date).Value = Convert.ToDateTime(txttDateFrom.Text);
                Cmm.Parameters.Add("@vRequestDateTo", SqlDbType.Date).Value = Convert.ToDateTime(txtDateto.Text );
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(dt);
            }
            con.Close();

            if (dt.Rows.Count > 0)
            {
                var AccrCourses = (from p in dt.AsEnumerable()
                                   select new
                                   {
                                       Id = p.Field<Int64>("ID"),
                                       InstName = p.Field<string>("InstName"),
                                       InstAddress = p.Field<string>("InstAddress"),
                                       Name = p.Field<string>("Name"),
                                       AppDate = p.Field<DateTime?>("AppDate"),
									   Accreditation_Number = p.Field<string>("Accreditation_Number")
                                   });



                PagingBar1.Bind(AccrCourses, ref grdNSQFCoursesForAccr);
                uPnlGrid.Update();
                //uPnlNavigation.Update();
                //divNavigation.Visible = true;
                //PagingBar1.Visible = true;
                lblError.Visible = false;
              //  gvMain.Visible = true;
              //  uPnlNavigation.Visible = true;

                grdNSQFCoursesForAccr.Visible = true;
                FileBlock.Visible = true;
                FileLabelBlock.Visible = true;
                saveBlock.Visible = true;
            }
            else
            {
                ShowAlert("No record found", true);
                lblError.Text = "No record found.";
                lblError.Visible = true;
                grdNSQFCoursesForAccr.Visible = false;
                uPnlNavigation.Visible = false;
                FileBlock.Visible = false;
                FileLabelBlock.Visible = false;
                saveBlock.Visible = false;
                //divNavigation.Visible = false;
            }

            //gvMain.DataSource = dt;
            //gvMain.DataBind();
            //uPnlGrid.Update();
            //uPnlNavigation.Update();



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
}