using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class FrmAccredetedCentre : BasePage
{
    Int32 CourseID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            //if (Request.UrlReferrer == null)   //COMMETED ON 20-09-2018 ON Request of Sh. Vikas Mittal DD(T)
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //    Response.End();
            //    return;
            //}
            if (!Page.IsPostBack)
            {
                bindState();
                bindstatus();
                bindcoursecategories();
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    bindcourse();
                    //RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
                    //RenderPage();
                    //showsidelink();
                }
                else
                {
                    lblHeading.Text = "Search Accredited Institutes";
                    //showaccredited();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Accredited Institutes", "WEB/FrmAccredetedCentre.aspx", ""));
                }
            }
            if (!String.IsNullOrEmpty(Lblerror.Text))
                Lblerror.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void RenderPage()
    {
        try
        {
            string tt = Convert.ToString(Session["ModuleID"]);
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse && (Request.QueryString["type"] == Convert.ToString(3)))
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Accredited Centre", "WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"], ""));
                    else
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Accredited Centre", "WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"], ""));
                    bindcoursecategory();
                    lblHeading.Text = "Search Accredited Centre";
                    //Sidelink.Items.Add(new SideLinkItem("Apply Online", "../CAND/FrmExamForm.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/Apply_Online.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Course Status", "CCStatus.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Accredited Centre", "WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"], ""));
                    else
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Accredited Centre", "WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"], ""));
                    bindcoursecategory();
                    lblHeading.Text = "Search Accredited Centre";
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Result.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
                else
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Centre", "WEB/FrmAccredetedCentre.aspx?" + Request.QueryString, ""));
                    //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Centre", "WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"], ""));
                    else
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Centre", "WEB/FrmAccredetedCentre.aspx?" + Request.QueryString, ""));
                    //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Centre", "WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"], ""));
                    bindcoursecategory();
                    lblHeading.Text = "Search Centre";
                    //Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Application_Status.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void bindState()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var state = from s in context.Locations
                            where s.LocationTypeID == 2
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlstate, state, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindDistrict(int id)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("All", "0");
                if (id != null || id != 0)
                {
                    var district = from s in context.Locations
                                   where s.LocationTypeID == 4 && s.ParentLocationID == id
                                   select new { ValueField = s.ID, TextField = s.Name };

                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddldistrict, district, lst);
                }

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id1 = Convert.ToInt32(Ddlstate.SelectedValue);
        Ddldistrict.Items.Clear();
        bindDistrict(id1);


    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            int[] approvedstatus = { 1, 2, 3, 4, 9 };

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Accredited Centre:Result", "", ""));
                BreadCrumb1.Render();
                CourseID = Convert.ToInt32(Request.QueryString["id"].ToString());

            }
            else
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Search Accredited Institute :Result", "", ""));
                BreadCrumb1.Render();
                CourseID = Convert.ToInt32(DdlcourseName.SelectedValue);
            }
            Int32 CourseCatId = Convert.ToInt32(DdlcourseCategory.SelectedValue);
            Int32 StateID = Convert.ToInt32(Ddlstate.SelectedValue);
            Int32 DistrictID = Convert.ToInt32(Ddldistrict.SelectedValue);
            Int32 StatusID = Convert.ToInt32(Ddlstatus.SelectedValue);
            Int32 accStatusId = Convert.ToInt32(enmAccreditationStatus.Full);
            Int32 accStatusId1 = Convert.ToInt32(enmAccreditationStatus.Provisional);
            String State = Ddlstate.SelectedItem.Text;


            var institute = (from i in context.Institutes
                             join a in context.AccreditationDetails
                                 on i.ID equals a.InstituteID
                             where approvedstatus.Contains(a.AccreditationStatusID)
				&& a.EffectiveToDate == context.AccreditationDetails.Where(w=>w.InstituteID==a.InstituteID  && w.CourseID ==a.CourseID).Max(m=>m.EffectiveToDate)
                             select new
                             {
                                 ID = i.ID,
                                 Name = i.Name,
                                 StateID = i.StateID,
                                 DistrictID = (i.DistrictID.HasValue) ? i.DistrictID.Value : 0,
                                 AccreditationStatusID = a.AccreditationStatusID,
                                 CourseID = a.CourseID,
                                 CategoryId = a.CourseCategoryID,
                                 StdNumber = (i.StdNumber.HasValue) ? i.StdNumber.Value : 0,
                                 PhoneNumber1 = (i.PhoneNumber1.HasValue) ? i.PhoneNumber1.Value : 0,
                                 EmailAddress1 = i.EmailAddress1,
                                 FaxNumber = (i.FaxNumber.HasValue) ? i.FaxNumber.Value : 0,
                                 MobileNumber = (i.MobileNumber.HasValue) ? i.MobileNumber.Value : 0,
                                 AddressLine1 = i.ID,
                                 AccNo = a.AccreditationNumber,
                                 Category = a.CourseCategory.Code,
                                 status = a.AccreditationStatus.Name,
                                 cname = a.Course.Name,
                                 validity = a.EffectiveToDate
                             });
            if (CourseCatId != 0)
                institute = institute.Where(q => q.CategoryId == CourseCatId);
            if (StateID != 0)
                institute = institute.Where(q => q.StateID == StateID);
            if (DistrictID != 0)
                institute = institute.Where(q => q.DistrictID == DistrictID);
            if (CourseID != 0)
                institute = institute.Where(q => q.CourseID == CourseID);
            if (StatusID != 0)
                institute = institute.Where(q => q.AccreditationStatusID == StatusID);
            if (institute.Count() > 0)
            {
                divfilter.Visible = false;
                divresult.Visible = true;
                Lblerror.Visible = false;
                Repeater1.Visible = true;
                Repeater1.DataSource = institute.ToList();
                Repeater1.DataBind();

            }
            else
            {
                Lblerror.Text = " No Record Found ";
            }


            if (!String.IsNullOrEmpty(Lblerror.Text))
                Lblerror.Visible = true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected String FullAddress(Institute inst)
    {

        string str = "";
        if (inst.AddressLine1 != null)
            str += inst.AddressLine1;
        if (inst.AddressLine2 != null)
            str += "<br/>" + inst.AddressLine2;
        if (inst.AddressLine3 != null)
            str += "<br/>" + inst.AddressLine3;
        if (inst.CityName != null)
            str += "<br/>" + inst.CityName + ", ";
        if (inst.DistrictID.HasValue)
            str += "<br/> District: " + inst.District.Name + ", ";
        if (inst.State.Name != null)
            str += inst.State.Name;
        if (inst.PinCode.HasValue)
            str += "<br/>Pin: " + inst.PinCode.Value.ToString();
        return str;
    }
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            String State = Ddlstate.SelectedItem.Text;
            if (e.Item.ItemType == ListItemType.Header)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //AccreditationDetail course = context.AccreditationDetails.Find(CourseID);
                    Label Lblhead = (Label)e.Item.FindControl("Lblhead");
                    if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    {
                        CourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                        Course currentCourse = context.Courses.Find(CourseID);
                        Lblhead.Text = "Course Category: " + currentCourse.CourseCategory.Name + ", Course Name: " + currentCourse.Name + ", State:" + State;
                    }
                    else
                    {
                        CourseID = Convert.ToInt32(DdlcourseName.SelectedValue);
                        Lblhead.Text = "Course Category: " + DdlcourseCategory.SelectedItem.Text + ",Course Name:" + DdlcourseName.SelectedItem.Text + ",State:" + State;
                    }
                }
            }
            if (e.Item.ItemIndex >= 0)
            {
                Label lblAddress = (Label)e.Item.FindControl("lblAddress");
                if (lblAddress != null)
                {
                    Int32 Id = Convert.ToInt32(lblAddress.Text);
                    //lblAddress.Text = "-";
                    if (Id != 0)
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            var address = (from c in context.Institutes
                                           where c.ID == Id
                                           select c).FirstOrDefault();
                            lblAddress.Text = FullAddress(address);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindstatus()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                int[] approvedstatus = { 1, 2, 3, 4, 9 };
              
                ListItem lst = new ListItem("--All--", "0");
                var status = from s in context.AccreditationStatus
                             where approvedstatus.Contains(s.ID)
                             select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlstatus, status, lst);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindcoursecategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var ccat = from s in context.CourseCategories
                           select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlcourseCategory, ccat, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindcourse()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlcourseName, courses, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindcourse(int id)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
              /*  var courses = from s in context.Courses
                              where s.CourseCategoryID == id
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlcourseName, courses, lst);*/
				 //Modified for NSQF courses
                if (id == 6)
                {
                    var courses = from s in context.Courses
                                  where s.CourseCategoryID == id
                                  && s.ID > 102
                                  select new { ValueField = s.ID, TextField = s.Name+ " ("+ s.Code+")" };

                    EConnect.Utils.Common.ControlUtility.BindListObject(DdlcourseName, courses, lst);
                }
                else
                {
                    var courses = from s in context.Courses
                                  where s.CourseCategoryID == id
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(DdlcourseName, courses, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindcoursecategory()
    {
        try
        {
            Int32 Courseid = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (var context = new EConnectContext())
            {

                Course currentcourse = context.Courses.Find(Courseid);
                if (currentcourse != null)
                {
                    var ccat = (from c in context.Courses
                                join cc in context.CourseCategories
                                on c.CourseCategoryID equals cc.ID
                                where c.ID == Courseid
                                select new { CourseCatname = cc.Name, Coursename = c.Name }).FirstOrDefault();

                    DdlcourseCategory.SelectedItem.Text = ccat.CourseCatname;
                    DdlcourseCategory.Enabled = false;
                    DdlcourseName.SelectedItem.Text = ccat.Coursename;
                    DdlcourseName.Enabled = false;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        Lblerror.Visible = false;
        DdlcourseCategory.SelectedValue = "0";
        DdlcourseName.SelectedValue = "0";
        Ddlstate.SelectedValue = "0";
        Ddldistrict.SelectedValue = "0";
        Ddlstatus.SelectedValue = "0";

    }
    protected void DdlcourseCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id1 = Convert.ToInt32(DdlcourseCategory.SelectedValue);
        DdlcourseName.Items.Clear();
        bindcourse(id1);
        if (id1 == 1)
        {
            Ddlstatus.Enabled = true;
        }
        else
        {
            Ddlstatus.SelectedIndex = 0;
            Ddlstatus.Enabled = false;
        }

    }
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../WEB/FrmAccredetedCentre.aspx?" + Request.QueryString));
            }
            else
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect("../WEB/FrmAccredetedCentre.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void showsidelink()
    //{
    //    try
    //    {
    //        Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
    //        Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var crs = context.Courses.Find(courseID);
    //            Int32 ccatId = crs.CourseCategoryID;

    //            var dl = from d in context.Downloadables
    //                     where d.CourseID == courseID
    //                     select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };
    //            var d2 = (from d in context.Downloadables
    //                      where d.CourseID == null && d.CourseCategoryID == ccatId
    //                      select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(dl);
    //            var d3 = (from d in context.Downloadables
    //                      where d.CourseID == null && d.CourseCategoryID == null
    //                      select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(d2);
    //            foreach (var dnbl in d3.Distinct())
    //            {
    //                Sidelink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
    //            }
    //            Sidelink1.Render();
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    protected void showaccredited()
    {
        try
        {
            lblHeading.Text = "Search Accredited Institutes";
            Sidelink.Items.Add(new SideLinkItem("Apply Online", "allCourses.aspx?query=apply", "../images/Apply_Online.jpg"));
            Sidelink.Items.Add(new SideLinkItem("Certifications/Courses", "allCourses.aspx", "../images/Get_Filled_Form.jpg"));
            Sidelink.Items.Add(new SideLinkItem("Regional Centres", "../abt_centers.aspx", "../images/View_Application_Status.jpg"));
            Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
            Sidelink.Render();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Search Accredited Institutes", "WEB/FrmAccredetedCentre.aspx", ""));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}