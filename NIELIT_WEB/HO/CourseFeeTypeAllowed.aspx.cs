using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

public partial class HO_CourseFeeTypeAllowed : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    NIELITMISContext context1;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    ArrayList TempDataTable = new ArrayList();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;


        try
        {
            ArrayList CheckBoxArray, TempDataTable;
            if (ViewState["CheckBoxArray"] != null)
            {
                CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            }
            else
            {
                CheckBoxArray = new ArrayList();
            }

            if (ViewState["TempDataTable"] != null)
            {
                TempDataTable = (ArrayList)ViewState["TempDataTable"];
            }
            else
            {
                TempDataTable = new ArrayList();
            }

            if (IsPostBack && String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                int CheckBoxIndex;
                bool CheckAllWasChecked = false;
                CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
                string checkAllIndex = "chkAll-" + gvMain.PageIndex;
                if (chkAll.Checked)
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
                    {
                        CheckBoxArray.Add(checkAllIndex);
                    }
                }
                else
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBoxArray.Remove(checkAllIndex);
                        CheckAllWasChecked = true;
                    }
                }
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkCourse");
                        Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
                       // CheckBoxIndex = Convert.ToInt32(lblID.Text); 
                        CheckBoxIndex = gvMain.PageSize * PagingBar1.CurrentPageIndex +(i + 1);
                        if (chk.Checked)
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
                            {
                                CheckBoxArray.Add(CheckBoxIndex);
                                TempDataTable.Add(Convert.ToInt32(lblID.Text));
                            }
                        }
                        else
                        {
                            if(TempDataTable.Contains(Convert.ToInt32(lblID.Text)))
                            {
                                TempDataTable.Remove(Convert.ToInt32(lblID.Text));
                            }
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
                            {
                                CheckBoxArray.Remove(CheckBoxIndex);
                            }
                        }
                    }
                }
                

            }

            ViewState["CheckBoxArray"] = CheckBoxArray;
            ViewState["TempDataTable"] = TempDataTable;
            //ViewState["SortField"] = "";
            //ViewState["SortOrder"] = "";
            //BindGridView();

            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Accredited", "Admin/AffInstitute.aspx", ""));
            
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            entityID = Convert.ToInt32(Session["EntityID"]);




            

            if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                //BindState();
                //Added 13 feb 2019
                //BindCityType();
                
               
                ShowEditMode(sender ,e);
               // BindGridView();

            }
            //else
            //{
            //    ViewState["SortField"] = "";
            //    ViewState["SortOrder"] = "";
               
            //    //BindCity();
            //    //Added 13 feb 2019
            //    //BindCityType();
            //    //BindState();
            //    pnlEdit.Visible = false;
             
            //  //  BindGridView();
            //    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Fee Type Allowed", "HO/CourseFeeTypeAllowed.aspx", ""));
            //}
            if (!Page.IsPostBack && String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                FillFilter();
                FillCategories();
                BindGridView();
                BindGridCoursesFee();
                gvMain.Visible = false;
                PagingBar1.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
            }
            //    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
            //        ShowAlert(Request.QueryString["msg"].ToString());


            //}
            //BreadCrumb1.Render();
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
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               join c in context.Courses on p.ID equals c.CourseCategoryID
                              // where c.CourseTypeID == CourseType
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                Category = Category.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
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
                ListItem lst = new ListItem("--Select One--", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id
                                 && p.ShowOnWeb 
                                 select new { ValueField = p.ID, TextField = p.Name + " ("+p.Code +")" };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
        
        FillCourses();
    }
    protected void FillFilter()
    {
        try
        {
            using (EConnectContext  context = new EConnectContext ())
            {
                ListItem lst = new ListItem("--All--", "0");
                var courselist = from p in context.Courses  
                               
                                select new { ValueField = p.ID, TextField = p.Name +" ("+p.Code+")"   };


                //var mylist = string.Concat(statelist,CourseList);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseFilter, courselist, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFeeType()
    {
        try
        {
            using (NIELITMISContext  context = new NIELITMISContext ())
            {
                ListItem lst = new ListItem("--All--", "0");
                var feeType = from p in context.feeTypeMas 

                                 select new { ValueField = p.ID, TextField = p.feeType  };


                //var mylist = string.Concat(statelist,CourseList);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFeeType , feeType , lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
            //txtInstituteID.Enabled = true;
            btnMode.ViewMode = ToggleView.Mode.List;
          //  mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            pnlEdit.Visible = false;
            pnlNew.Visible = true;
            //Change the heading text as required
            lblHeading.Text = "Course fee Type Allowed";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Fee Type Allowed :New", "#", ""));
        }
        else
        {
            //txtInstituteID.Enabled = false;
            Response.Redirect("CourseFeeTypeAllowed.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar2.CurrentPageIndex = 0;
            gridCourseFeeType.PageIndex = PagingBar2.CurrentPageIndex;
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
            PagingBar2.CurrentPageIndex = 0;
            gridCourseFeeType.PageIndex = PagingBar2.CurrentPageIndex;
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
            PagingBar2.CurrentPageIndex = 0;
            gridCourseFeeType.PageIndex = PagingBar2.CurrentPageIndex;
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
           ddlCourseFilter .SelectedValue = "0";
            PagingBar2.CurrentPageIndex = 0;
            gridCourseFeeType.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridCoursesFee ();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext  context1 = new EConnectContext ();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var courses = from s in context1.Courses  
                         //  where s.IsActive 
                           select new { Name = s.Name   };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name).Distinct();
            foreach (var course in courses)
            {
                items.Add(course.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context1.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("CourseFeeTypeAllowed.aspx", true);
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    CourseFeetype CourseFeeType;
                    CourseFeeType = context2.CourseFeetypes.Find(Convert.ToInt32(Request.QueryString["Key"]));
                    if (ddlIsActive.SelectedValue.ToString() == "1")
                        CourseFeeType.isActive = true;
                    if (ddlIsActive.SelectedValue.ToString() == "2")
                        CourseFeeType.isActive = false;
                    context2.SaveChanges();       
                }

               

                gvMain.AllowPaging = true;
                gvMain.PageSize = 10;
                strMessage = "Record updated";


                Response.Redirect("CourseFeeTypeAllowed.aspx?msg=" + strMessage);

                return;
            }
            


            if (ViewState["CheckBoxArray"] != null)
            {
                ArrayList TempDataTable1 = (ArrayList)ViewState["TempDataTable"];
                  Int32 userid = Convert.ToInt32(Session["UserId"]);
            foreach (Int32 c in TempDataTable1)
            {

               
                    Int32 feeTypeID = c;// Convert.ToInt32(gvMain.DataKeys[gvrow.RowIndex].Value);

                    BreadCrumb1.Render();
                    using (NIELITMISContext context2 = new NIELITMISContext())
                    {
                        CourseFeetype CourseFeeType;
                        CourseFeeType =new CourseFeetype ();
                        int courseID = Convert.ToInt32(ddlcourse.SelectedValue);

                        var CourseFeeType1=(from s in context2.CourseFeetypes 
                                        where s.courseID.ToString ().Trim () == courseID.ToString ()
                                        && s.feeTypeID.ToString ().Trim()==c.ToString ()
                                            select s).Count();

                        CourseFeeType = new CourseFeetype();
                     

                       

                        if (CourseFeeType1 > 0)
                        {
                            ShowAlert("Course and Fee Type already Exist");
                           continue;
                        }
                        

                        CourseFeeType.courseID =courseID ;
                        CourseFeeType.feeTypeID  = c;
                        if (ddlIsActive.SelectedValue.ToString() == "1")
                            CourseFeeType.isActive = true;
                        if (ddlIsActive.SelectedValue.ToString() == "2")
                            CourseFeeType.isActive = false;

                       // projCourses .projID =Convert.ToInt32 (ddlProjects .SelectedValue );
                        CourseFeeType.enterDate = System.DateTime.Today;
                        CourseFeeType.enterBy = userid;


                        context2.CourseFeetypes .Add(CourseFeeType);
                             context2.SaveChanges();
                        }

                       

                        gvMain.AllowPaging = true;
                        gvMain.PageSize = 10;
                        strMessage = "New record saved.";
                    
                }
            }
            
            Response.Redirect("CourseFeeTypeAllowed.aspx?msg=" + strMessage);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            lblError.Visible = false;
            context = new EConnectContext();
            
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlCourseFilter .SelectedValue != "0")
                stateID = Convert.ToInt64(ddlCourseFilter.SelectedValue);

            DataTable DT = new DataTable();

           
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("getFeeTypeMas", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }


           PagingBar1.Bind(DT, ref gvMain);
           uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;

            }
            else
            {
                PagingBar1.Visible = true;
                uPnlGrid.Visible = true;
                uPnlNavigation.Visible = true;
            }

            //if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            //{
            //    gvMain.Columns[4].Visible = false;
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
            context.Dispose();
        }
    }

    protected void BindGridCoursesFee()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            lblError.Visible = false;
            context1 = new NIELITMISContext();

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            DataTable DT = new DataTable();

          
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("getCourseFeeType", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                 SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                string expression = "[course] like '%" + searchString + "%'";
                
                    DT = DT.Select(expression ).CopyToDataTable ();

            }

            if (ddlCourseFilter.SelectedValue.ToString() != "0" )
            {
                string expression = "[courseID]=" +Convert.ToInt32 ( ddlCourseFilter.SelectedValue)  ;
                if (DT.Select(expression).Count() > 0)
                    DT = DT.Select(expression).CopyToDataTable();
                else
                {
                    ShowAlert("No record found", true);
                    
                }
            }
            PagingBar2.Bind(DT, ref gridCourseFeeType   );
            UpdatePanel1.Update();
            uPnlNavigation2.Update();
            if (gridCourseFeeType.Rows.Count > 0)
            {
                PagingBar2.Visible = true;
                UpdatePanel1.Visible = true;
                uPnlNavigation2.Visible = true;
            }
            else
            {
                PagingBar2.Visible = false;
                UpdatePanel1.Visible = false;
                uPnlNavigation2.Visible = false;
            }
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
            context1.Dispose();
        }
    }

    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }
    protected void PageIndexChanged2(Int32 NewPageIndex)
    {
        try
        {
           gridCourseFeeType.PageIndex = PagingBar2.CurrentPageIndex;
            int NewIndex = NewPageIndex;
         
            BindGridCoursesFee();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            int NewIndex = NewPageIndex;
            BindGridView();
            
            if (ViewState["CheckBoxArray"] != null)
            {
                ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                string checkAllIndex = "chkAll-" + gvMain.PageIndex;

                if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                {
                    CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
                    chkAll.Checked = true;
                }
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {

                    if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                        {
                            CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkCourse");
                            chk.Checked = true;
                            gvMain.Rows[i].Attributes.Add("style", "background-color:aqua");
                        }
                        else
                        {

                            int CheckBoxIndex = gvMain.PageSize * (NewIndex) + (i + 1);
                            Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
                            int ID = Convert.ToInt32(lblID.Text);
                            //if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                            {
                                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkCourse");
                                chk.Checked = true;
                                gvMain.Rows[i].Attributes.Add("style", "background-color:aqua");
                            }
                        }
                    }
                }
            }
            BindGridCoursesFee();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowEditMode(object sender, EventArgs e)
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            FillCategories();
            //FillCourses();
            FillFeeType();
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Course Fee Type Allowed";
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 courseFeeTypeAllowedID = 0;
                //Int32 NielitCentreCourseId = Convert.ToInt32(Request.QueryString["Id"]);
                courseFeeTypeAllowedID = Convert.ToInt32(Request.QueryString["Key"]);
                CourseFeetype courseFeeType = context.CourseFeetypes.Find(courseFeeTypeAllowedID);
                var courseFeeTypeRec = (from p in context.CourseFeetypes
                                        where p.ID == courseFeeTypeAllowedID 
                              select new
                              {
                                  ID = p.ID,
                                  feeTypeID = p.feeTypeID ,
                                  courseID = p.courseID ,
                                  IsActive = p.isActive ,
                                  
                              }).FirstOrDefault();
                EConnectContext context1 = new EConnectContext();
                var courseCategory = (from c in context1.Courses
                                      where c.ID == courseFeeTypeRec.courseID
                                      select new
                                      {
                                          categoryID = c.CourseCategoryID
                                      }).FirstOrDefault();

                //int courseIdExists = (from b in context.NielitCentreBatchs
                //              where b.CourseID == courseId
                //              select b).Count();   
                ddlcoursecategory.SelectedValue = courseCategory.categoryID.ToString ();
                ddlcoursecategory_SelectedIndexChanged(sender, e);

                ddlcourse.SelectedValue = courseFeeTypeRec.courseID.ToString ();
                ddlFeeType.SelectedValue = courseFeeTypeRec.feeTypeID.ToString();
                
                     if (courseFeeTypeRec.IsActive == true)
                {
                    ddlIsActive.SelectedValue = "1";
                }
                else
                {
                    ddlIsActive.SelectedValue = "2";
                }
                ddlIsActive.Enabled = true;
                 ddlcourse.Enabled = false;
                 ddlFeeType.Enabled = false;
                 ddlcoursecategory.Enabled = false;

                 uPnlNavigation.Visible = true;
                 pnlNew.Visible = true;
                 pnlEdit.Visible = true;


                 uPnlGrid.Visible = true;
                 btnCancel.Visible = true;
                 btnSave.Visible = true;
                 divNavigation.Visible = true;
                 PagingBar1.Visible = false;
                 PagingBar2.Visible = false;
               
                }
            
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridView();
        gvMain.Visible = true;
        PagingBar1.Visible = true;
        btnSave.Visible = true;
        btnCancel.Visible = true;

        //gridCourseFeeType.Visible = false;
        //divNavigation2.Visible = false;
        //PagingBar2.Visible = false; 
        //uPnlNavigation2.Visible = false;
    }
}