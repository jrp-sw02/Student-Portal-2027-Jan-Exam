using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class Admin_BCCCandidatePersonalDetails : BasePage
{
  
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        //Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Common/SearchBCCCCCandidate.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                bindOccupation();
                bindCastCategory();
                bindGender();
                if (!String.IsNullOrEmpty(Request.QueryString["key"]))
                {
                    ShowEditMode();                  
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();     
                    
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public void bindOccupation()
    {

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var occupation = from p in context.Occupations
                                orderby (p.DisplayOrder)
                                select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddloccupation, occupation, lst);
        };

    }
    public void bindGender()
    {
        //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlGender, typeof(EConnect.Gender), new ListItem("--Select One--", "0"));
        //Added for gender
        using (EConnectContext vContext = new EConnectContext())
        {
            var Gender = from s in
                             vContext.tblGender
                         select new { ValueField = s.genderCode, TextField = s.name };

            EConnect.Utils.Common.ControlUtility.BindListObject(ddlGender, Gender, new ListItem("--Select Gender--", "0"));

        }

    }
    public void bindCastCategory()
    {

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var castcategory = from p in context.CastCategories
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
        };


    }
    protected void ShowEditMode()
    {

        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Personal Detail:Update", "", ""));
            BreadCrumb1.Render();
            context = new EConnectContext();
            string name = Request.QueryString["Name"].ToLower().Substring(3);
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            var student = (from s in context.CertificateExamApplications 
                           where s.ID == Appid
                           select new
                           {
                               name = s.Name,
                               fname= s.FatherName,
                               mname= s.MotherName,
                               dob= s.DateOfBirth,
                               gender = s.Gender,
                               occupation= (s.OccupationID!=0)?s.OccupationID:0,
                               category = (s.CastCategoryID!=0)?s.CastCategoryID : 0,
                               salution=s.Salutation,
                               Gname=s.GuardianName,
                           }).FirstOrDefault();
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Update Personal Detail";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields
            if (student.salution.ToString().ToUpper() == "MR.")
                ddlSalutation.SelectedValue = "1";
            else
            {
                if (student.salution.ToString().ToUpper() == "MS.")
                    ddlSalutation.SelectedValue = "2";
                else
                    ddlSalutation.SelectedValue = "3";
            }

            //if (student.salution.ToString().ToUpper() == "MR.")
            //    ddlSalutation.SelectedValue = "1";
            //else
            //    ddlSalutation.SelectedValue = "2";
            txtName.Text = student.name.ToUpper();

            //Modified for gender
            if (student.gender.ToString().ToUpper() == "FEMALE")
                ddlGender.SelectedValue = "Female";
            if (student.gender.ToString().ToUpper() == "MALE")
                ddlGender.SelectedValue = "Male";
            else
                ddlGender.SelectedValue = "Trans";



            //if(student.gender.ToString().ToUpper()=="FEMALE")
            //  ddlGender.SelectedValue = "1";
            //else
            //    ddlGender.SelectedValue = "2";
            if (string.IsNullOrEmpty(student.Gname) == true && string.IsNullOrWhiteSpace(student.Gname) == true)
            {
                if (string.IsNullOrEmpty(student.fname) == false && !string.IsNullOrWhiteSpace(student.fname))
                    Txt_Fname.Text = GetInitCap(student.fname);
                else
                    Txt_Fname.Text = " ";
                if (string.IsNullOrEmpty(student.mname) == false && string.IsNullOrWhiteSpace(student.mname) == false)
                    Txt_Mname.Text =GetInitCap(student.mname);
                else
                    Txt_Mname.Text = " ";
            }
            else
            {
                Txt_GName.Text = string.IsNullOrEmpty(student.Gname) == false && string.IsNullOrWhiteSpace(student.Gname) == false ? GetInitCap(student.Gname) : " ";
            }
            
            Txt_Dob.Text = student.dob.ToString("dd-MMM-yyyy");
            ddloccupation.SelectedValue = student.occupation.ToString();
            ddlCategory.SelectedValue = student.category.ToString();

            EnableDisableColumn();

            if (!UserManager.HasRight(currentRoleId, enmRight.Edit, "Common/SearchBCCCCCandidate.aspx"))
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
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int64 Appid = Convert.ToInt64(Request.QueryString["key1"]);
            var student = (from s in context.CertificateExamApplications
                          where s.ID == Appid
                          select new
                          {
                              ID = s.ID,
                              appid = Appid,
                              Name = s.Salutation + "" + s.Name.ToUpper(),
                              Fname = ((s.FatherName!=null && s.FatherName!=" ")? "Mr." + s.FatherName.ToUpper():"NA"),
                              Mname = ((s.MotherName!=null && s.MotherName!=" ")?"Mrs." + s.MotherName.ToUpper():"NA"),
                              gender = s.Gender.ToUpper(),
                          });

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.ID);
                        else
                            student = student.OrderBy(s => s.ID);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Name);
                        else
                            student = student.OrderBy(s => s.Name);
                        break;
                    case "Fname":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Fname);
                        else
                            student = student.OrderBy(s => s.Fname);
                        break;
                    case "Mname":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Mname);
                        else
                            student = student.OrderBy(s => s.Mname);
                        break;
                    case "gender":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.gender);
                        else
                            student = student.OrderBy(s => s.gender);
                        break;
                    default:
                        student = student.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(student, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Personal Detail", "Admin/BCCCandidatePersonalDetails.aspx?key1=" + Request.QueryString["key1"], ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Personal Detail", "Admin/BCCCandidatePersonalDetails.aspx?" + Request.QueryString.ToString(), ""));
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New Personal Details";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Personal Details", "#", ""));
            //Change the heading text as required
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BCCCandidatePersonalDetails.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("BCCCandidatePersonalDetails.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            string name = Request.QueryString["Name"].ToLower().Substring(3);
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                var student = (from s in context.CertificateExamApplications
                               where s.ID == Appid
                               select s).FirstOrDefault();

                if (ddlSalutation.SelectedValue == "1")
                    student.Salutation = "Mr.";
                else if (ddlSalutation.SelectedValue == "2")
                    student.Salutation = "Ms.";
                else
                    student.Salutation = "Others";
                student.Name = txtName.Text;
                //Added for gender
                student.Gender = ddlGender.SelectedValue;

                //if(ddlGender.SelectedValue=="1")
                //    student.Gender = "Female";
                //else
                //    student.Gender = "Male";

                if (Txt_GName.Enabled == true && Txt_Fname.Enabled == true && Txt_Mname.Enabled == true)
                {
                    if ((isBlank(Txt_GName) && isBlank(Txt_Fname) && isBlank(Txt_Mname)) || (!isBlank(Txt_GName) && !isBlank(Txt_Fname) && !isBlank(Txt_Mname)))
                    {
                        ShowAlert("Please enter either (Father Name and Mother Name) OR  Guardian Name.");
                        return;
                    }
                    else if (!isBlank(Txt_Fname) && isBlank(Txt_Mname))
                    {
                        ShowAlert("Please enter Father Name.");
                        return;
                    }
                    else if (!isBlank(Txt_Mname) && isBlank(Txt_Fname))
                    {
                        ShowAlert("Please enter Mother Name.");
                        return;
                    }
                }
                if (string.IsNullOrEmpty(Txt_GName.Text) == true && string.IsNullOrWhiteSpace(Txt_GName.Text) == true)
                {
                    student.FatherName = Txt_Fname.Text.Trim();
                    student.MotherName = Txt_Mname.Text.Trim();
                    student.GuardianName = null;
                }
                else
                {
                    student.FatherName = null;
                    student.MotherName = null;
                    student.GuardianName = Txt_GName.Text.Trim();
                }
             
                //student.FatherName = Txt_Fname.Text;
                //student.MotherName = Txt_Mname.Text;
                student.DateOfBirth = Convert.ToDateTime(Txt_Dob.Text.ToString());
                student.OccupationID = Convert.ToInt32(ddloccupation.SelectedValue);
                student.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record Updated";
            }

            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BCCCandidatePersonalDetails.aspx?key1=" + Request.QueryString["key1"] + "&msg=" + strMessage));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        //try
        //{
        //    //ddlSearchUserType.SelectedValue = "0";
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
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
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    //public static String[] GetSearchText(String prefixText, Int32 count)
    //{
    //    EConnectContext context = new EConnectContext();
    //    try
    //    {
    //        if (count <= 0)
    //            count = 10;
    //        List<String> items = new List<String>();
    //        string searchString = prefixText.Trim().ToUpper();
    //        var student = from s in context.CourseRegistrationApplications
    //                    select new { Name = s.UserName };
    //        if (!String.IsNullOrEmpty(searchString))
    //        {
    //            student = student.Where(s => s.Name.ToUpper().Contains(searchString));
    //        }
    //        student = student.OrderBy(s => s.Name);

    //        var student1 = from s in context.CourseRegistrationApplications
    //                     select new { Name = s.LoginID };
    //        if (!String.IsNullOrEmpty(searchString))
    //        {
    //            student1 = student1.Where(s => s.Name.ToUpper().Contains(searchString));
    //        }
    //        student = student.Union(student1).Take(count);
    //        foreach (var user in student)
    //        {
    //            items.Add(user.Name);
    //        }
    //        return items.ToArray();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally { context.Dispose(); }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BCCCandidatePersonalDetails.aspx?key1=" + Request.QueryString["key1"]));
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
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
    protected void ddlSalutation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Changed for gender


            if (ddlSalutation.SelectedValue == "1")
                ddlGender.SelectedValue = "Male";
            //ddlGender.SelectedValue = "1";
            if (ddlSalutation.SelectedValue == "2")
                ddlGender.SelectedValue = "Female";
            //ddlGender.SelectedValue = "2";
            if (ddlSalutation.SelectedValue == "3")
                ddlGender.SelectedValue = "Trans";            

            
                //if (ddlSalutation.SelectedValue == "2")
                //    ddlGender.SelectedValue = "1";
                //if (ddlSalutation.SelectedValue == "1")
                //    ddlGender.SelectedValue = "2";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void EnableDisableColumn()
    {
        try
        {
            //configurable update of columns
            var name = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Name));
            if (name.IsUpdate == true)
                txtName.Enabled = true;
            else
                txtName.Enabled = false;

            var dob = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.DOB));
            if (dob.IsUpdate == true)
            {
                Txt_Dob.Enabled = true;
                ceDOB.Enabled = true;
            }
            else
            {
                Txt_Dob.Enabled = false;
                ceDOB.Enabled = false;
            }

            var gender = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Gender));
            if (gender.IsUpdate == true)
                ddlGender.Enabled = true;
            else
                ddlGender.Enabled = false;

            var ocupation = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Occupation));
            if (ocupation.IsUpdate == true)
                ddloccupation.Enabled = true;
            else
                ddloccupation.Enabled = false;

            var catcategory = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.CasteCategory));
            if (catcategory.IsUpdate == true)
                ddlCategory.Enabled = true;
            else
                ddlCategory.Enabled = false;

            var fname = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.FatherName));
            if (fname.IsUpdate == true)
                Txt_Fname.Enabled = true;
            else
                Txt_Fname.Enabled = false;

            var mname = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.MotherName));
            if (mname.IsUpdate == true)
                Txt_Mname.Enabled = true;
            else
                Txt_Mname.Enabled = false;

            var guardian = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Guardian));
            if (guardian.IsUpdate == true)
                Txt_GName.Enabled = true;
            else
                Txt_GName.Enabled = false;

            var saltation = context.UpdateFields.Find(Convert.ToInt32(enmUpdateFields.Salutation));
            if (saltation.IsUpdate == true)
                ddlSalutation.Enabled = true;
            else
                ddlSalutation.Enabled = false;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }

    }
}