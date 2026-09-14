using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
public partial class Admin_AdminAccrediatedAsprDcenter : BasePage
{
    String strMessage = string.Empty;
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
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    BindState();
					//Added 13 feb 2019
                  
                    BindCityType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilter();
                    //BindCity();
					
                    //Added 13 feb 2019
                    BindCityType();
                  
                    BindState();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Institute Accredited For Aspirational District", "Admin/AdminAccrediatedAsprDcenter.aspx", ""));
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
	
	 //Added 13 feb 2019
    public void BindCityType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var cityType = from p in context.cityTypeMas
                               orderby (p.ID)
                               select new { ValueField = p.ID, TextField = p.description };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCityType, cityType, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
	
    public void BindState()
    {
        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var state = from s in context.Locations
                        where s.LocationTypeID == 2
                        select new { ValueField = s.ID, TextField = s.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlstate, state, lst);

        }
    }
    public void BindDistrict(int id)
    {
        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select All--", "0");
            if (id != null || id != 0)
            {
                var district = from s in context.Locations
                               where s.LocationTypeID == 4 && s.ParentLocationID == id
                               select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddldistrict, district, lst);
            }

        }
    }
    protected void FillFilter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var statelist = from p in context.Locations
                                where p.LocationTypeID == 2
                                select new { ValueField = p.ID, TextField = p.Name };


                //var mylist = string.Concat(statelist,CourseList);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAccentre, statelist, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //deep RdoYesNo_SelectedIndexChanged 
    protected void RdoYesNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Int32 Check = Convert.ToInt32(RdoYesNo.SelectedItem.Value);
            if (Check == 1)
            {
                YesNo.Visible = true;
                //btnSave.Visible = false;
                //btnCancel.Visible = false;
                DisplayRow(false);
            }
            if (Check == 0)
            {
                YesNo.Visible = false;
                DisplayRow(true);
                ClearRecords();
                RowError19.Visible = false;
                GenerateInstiteID();
                btnview.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void BindCourseCategory()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var coursecategory = from s in context.CourseCategories
    //                                 select new { ValueField = s.ID, TextField = s.Name };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, coursecategory, lst);
    //        };
    //        ddlcoursecategory.SelectedValue = "6";
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //protected void BindCourse()
    //{
    //    try
    //    {
    //        Int32 catg = Convert.ToInt32(ddlcoursecategory.SelectedItem.Value);
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var course = from s in context.Courses
    //                         where s.CourseCategoryID == catg
    //                             //Added 17 Jan 2019
    //                         && s.ShowOnWeb
    //                         select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, course, lst);
    //        };
    //        ddlcoursecategory.SelectedValue = "6";
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 catg = Convert.ToInt32(ddlcoursecategory.SelectedItem.Value);
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var course = from s in context.Courses
    //                         where s.CourseCategoryID == catg
    //                             //Added 17 Jan 2019
    //                         && s.ShowOnWeb
    //                         select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, course, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
   
    protected void DisplayRow(Boolean value)
    {       
        row1.Visible = value;
        row2.Visible = value;
        row3.Visible = value;
        row4.Visible = value;
        row5.Visible = value;
        row6.Visible = value;
        row7.Visible = value;
        row8.Visible = value;
        row9.Visible = value;
        row10.Visible = value;
        row11.Visible = value;
        row12.Visible = value;
        row13.Visible = value;
        row14.Visible = value;
        row15.Visible = value;
        row16.Visible = value;
        row17.Visible = value;
        row18.Visible = value;
        btnSave.Visible = value;
        btnCancel.Visible = value;
        

    }
    protected void ClearRecords()
    {
        txtInstituteID.Text="";
        txtInstituteID.Enabled = false;
        txtaccentre.Text = "";
        txtcontactperson.Text = "";
        txtdesignation.Text = "";
        txtstdno.Text = "";
        txtphone1.Text = "";
        txtphone2.Text = "";
        txtfaxno.Text = "";
        txtmobile.Text = "";
        txtemail1.Text = "";
        txtemail2.Text = "";
        txtwebaddress.Text = "";
        txtadd1.Text = "";
        txtadd2.Text = "";
        txtadd3.Text = "";
        txtcity.Text = "";
        txtpinno.Text = "";
        ddlCityType.SelectedValue = "0";
        ddlstate.SelectedValue = "0";
        ddldistrict.SelectedValue = "0";
        //ddlcoursecategory.SelectedValue = "6";
        //ddlcourseName.SelectedValue = "0";
    }

    protected void GenerateInstiteID()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Int64 AutoInstituteId = 62100001;
           
            //var InstituteIId1 = (from c in context.Institutes
            //                     join a in context.AccreditationDetails on c.ID equals a.InstituteID
            //                     where a.CourseCategoryID == 6 && c.ID > 62100000 && c.ID.ToString().StartsWith("621")
            //                     select new
            //                     {
            //                         ID = c.ID
            //                     }).ToList();

            var InstituteIId1 = (from c in context.Institutes                                
                                 where  c.ID > 62100000 && c.ID.ToString().StartsWith("621")
                                 select new
                                 {
                                     ID = c.ID
                                 }).ToList();

            //var InstituteIId = InstituteIId1.OrderByDescending(x => x.ID).First();


            if (InstituteIId1.Count != 0)
            {
                var InstituteIId = InstituteIId1.OrderByDescending(x => x.ID).First();
                Int64 MaxInstituteID = Convert.ToInt64(InstituteIId.ID);
                AutoInstituteId = MaxInstituteID + 1;
            }
            else
            {
                AutoInstituteId = 62100001;

                //var InstituteIId2 = (from c in context.Institutes                                    
                //                     where  c.ID > 62100000 && c.ID.ToString().StartsWith("621")
                //                     select new
                //                     {
                //                         ID = c.ID
                //                     }).ToList();
                //if (InstituteIId2.Count != 0)
                //{
                //    var InstituteIId = InstituteIId1.OrderByDescending(x => x.ID).First();
                //    Int64 MaxInstituteID = Convert.ToInt64(InstituteIId.ID);
                //    AutoInstituteId = MaxInstituteID + 1;

                //}
            }
            txtInstituteID.Text = AutoInstituteId.ToString();
           
        }
    }
     protected void SearchRecord(object sender, EventArgs e)
    {
        try
        {
            btnSave.Visible = false;
            btnview.Visible = false;
            DisplayRow(true);
            string accrNo = txtAccrNo.Text.Trim();

            using (EConnectContext context = new EConnectContext())
            {
                var InstitueAccrDetails = (from p in context.AccreditationDetails
                                           where p.AccreditationNumber == accrNo //&& p.AccreditationStatusID <=4
                              select p).FirstOrDefault();
                if (InstitueAccrDetails != null)
                {


                    string instId = InstitueAccrDetails.InstituteID.ToString();
                    Int64 centreId = Convert.ToInt64(InstitueAccrDetails.InstituteID);
                    txtInstituteID.Text = centreId.ToString();
                    txtInstituteID.Enabled = false;
                    var centre = (from p in context.Institutes
                                  where p.ID == centreId
                                  select p).FirstOrDefault();

                    txtaccentre.Text = centre.Name.ToString();
                    if (centre.ContactPersonName != null)
                        txtcontactperson.Text = centre.ContactPersonName;
                    if (centre.ContactPersonPost != null)
                        txtdesignation.Text = centre.ContactPersonPost;
                    if (centre.MobileNumber != null)
                        txtmobile.Text = centre.MobileNumber.ToString();
                    if (centre.FaxNumber != null)
                        txtfaxno.Text = centre.FaxNumber.ToString();
                    if (centre.StdNumber != null)
                        txtstdno.Text = "0" + centre.StdNumber.ToString();
                    if (centre.PhoneNumber1 != null)
                        txtphone1.Text = centre.PhoneNumber1.ToString();
                    if (centre.PhoneNumber2 != null)
                        txtphone2.Text = centre.PhoneNumber2.ToString();
                    if (centre.EmailAddress1 != null)
                        txtemail1.Text = centre.EmailAddress1.ToString();
                    if (centre.EmailAddress2 != null)
                        txtemail2.Text = centre.EmailAddress2.ToString();
                    if (centre.WebAddress != null)
                        txtwebaddress.Text = centre.WebAddress.ToString();
                    hfAccID.Value = Request.QueryString["Key"];
                    hfName.Value = Request.QueryString["Name"];

                    txtadd1.Text = centre.AddressLine1;
                    if (centre.AddressLine2 != null)
                        txtadd2.Text = centre.AddressLine2;
                    if (centre.AddressLine3 != null)
                        txtadd3.Text = centre.AddressLine3;
                    if (centre.CityName != null)
                        txtcity.Text = centre.CityName;

                    //Added 13 Feb 2019
                    if (centre.cityTypeID != null)
                        ddlCityType.SelectedValue = centre.cityTypeID.ToString();

                    if (centre.StateID != null)
                    {
                        ddlstate.SelectedValue = centre.StateID.ToString();
                        ddlstate_SelectedIndexChanged(ddlstate, EventArgs.Empty);
                    }
                    if (centre.DistrictID.HasValue)
                        ddldistrict.SelectedValue = centre.DistrictID.ToString();
                    if (centre.PinCode != null)
                        txtpinno.Text = centre.PinCode.ToString();
                    RowError19.Visible = false;
               

                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(centre.Name, "Admin/AdminAccrediatedAsprDcenter.aspx?" + Request.QueryString.ToString(), ""));
                //hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("accrediationdetails.aspx?key1=" + Request.QueryString["Key"] + "&name=" + Request.QueryString["Name"]);
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("AccrediationDetailsAsprD.aspx?key1=" + Request.QueryString["Key"]);
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                    btnSave.Visible = false;
                }
                }
                else
                {
                    RowError19.Visible = true;
                    lblerrorAccr.Text = "Record not found / Invalid Accreditation Number";
                    DisplayRow(false);

                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    //deep
    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            btnSave.Enabled = false;
            btnSave.Visible = false;
            lblHeading.Text = "Institute Accredited For Aspirational District";
            tblNavLinks.Visible = true;

            using (EConnectContext context = new EConnectContext())
            {
                Int32 centreId = Convert.ToInt32(Request.QueryString["Key"]);
                txtInstituteID.Text = centreId.ToString();
                txtInstituteID.Enabled = false;
                var centre = (from p in context.Institutes
                              where p.ID == centreId
                              select p).FirstOrDefault();

                txtaccentre.Text = centre.Name.ToString();
                if (centre.ContactPersonName != null)
                    txtcontactperson.Text = centre.ContactPersonName;
                if (centre.ContactPersonPost != null)
                    txtdesignation.Text = centre.ContactPersonPost;
                if (centre.MobileNumber != null)
                    txtmobile.Text = centre.MobileNumber.ToString();
                if (centre.FaxNumber != null)
                    txtfaxno.Text = centre.FaxNumber.ToString();
                if (centre.StdNumber != null)
                    txtstdno.Text = "0" + centre.StdNumber.ToString();
                if (centre.PhoneNumber1 != null)
                    txtphone1.Text = centre.PhoneNumber1.ToString();
                if (centre.PhoneNumber2 != null)
                    txtphone2.Text = centre.PhoneNumber2.ToString();
                if (centre.EmailAddress1 != null)
                    txtemail1.Text = centre.EmailAddress1.ToString();
                if (centre.EmailAddress2 != null)
                    txtemail2.Text = centre.EmailAddress2.ToString();
                if (centre.WebAddress != null)
                    txtwebaddress.Text = centre.WebAddress.ToString();
                hfAccID.Value = Request.QueryString["Key"];
                hfName.Value = Request.QueryString["Name"];

                txtadd1.Text = centre.AddressLine1;
                if (centre.AddressLine2 != null)
                    txtadd2.Text = centre.AddressLine2;
                if (centre.AddressLine3 != null)
                    txtadd3.Text = centre.AddressLine3;
                if (centre.CityName != null)
                    txtcity.Text = centre.CityName;
				
				 //Added 13 Feb 2019
                if (centre.cityTypeID != null)
                    ddlCityType.SelectedValue = centre.cityTypeID.ToString();
				
                if (centre.StateID != null)
                {
                    ddlstate.SelectedValue = centre.StateID.ToString();
                    ddlstate_SelectedIndexChanged(ddlstate, EventArgs.Empty);
                }
                if (centre.DistrictID.HasValue)
                    ddldistrict.SelectedValue = centre.DistrictID.ToString();
                if (centre.PinCode != null)
                    txtpinno.Text = centre.PinCode.ToString();

                btnSave.Enabled = false;

                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(centre.Name, "Admin/AdminAccrediatedAsprDcenter.aspx?" + Request.QueryString.ToString(), ""));
                //hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("accrediationdetails.aspx?key1=" + Request.QueryString["Key"] + "&name=" + Request.QueryString["Name"]);
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("AccrediationDetailsAsprD.aspx?key1=" + Request.QueryString["Key"]);
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
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
    public DataTable FillGridViewNIELITMISInstituteAccrGeneratedWithNIELITINSTRecord()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("FillGridViewNIELITMISInstituteAccrGeneratedWithNIELITINSTRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreID"].Value = Convert.ToInt64(Session["EntityID"]);
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
        try
        {
            lblError.Visible = false;
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlAccentre.SelectedValue != "0")
                stateID = Convert.ToInt64(ddlAccentre.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            using (DataTable dt = FillGridViewNIELITMISInstituteAccrGeneratedWithNIELITINSTRecord())
            {
                if (dt.Rows.Count > 0)
                {
                    var centre = (from p in dt.AsEnumerable()
                                  select new
                                  {
                                      ID = p.Field<Int64>("ID"),
                                      Name = p.Field<string>("INSTITUTENAME"), //INSTITUTE NAME
                                      Location = p.Field<string>("City_Name"),// + ", " + p.Field<string>("STATENAME"),
                                      City1 = p.Field<string>("City_Name"),
                                      StateID = p.Field<Int64>("State_ID"),
                                     // enterBy = p.Field<int>("enterBy"),
                                      ContactPersonName = p.Field<string>("Contact_Person_Name"),
                                      AccreditationNumber = p.Field<string>("Accreditation_Number")
                                  });


                    //var centre = (from s in context.Institutes

                    //              where s.StateID != null 
                    //              select new
                    //               {
                    //                   ID = s.ID,
                    //                   Location = s.CityName.ToUpper() + ", " + s.State.Name.ToUpper(),
                    //                   City1 = s.CityName,
                    //                   StateID = s.StateID,
                    //                   Name = s.Name,
                    //                   ContactPersonName = s.ContactPersonName,
                    //                   //  MobileNumber = s.MobileNumber.HasValue ? s.MobileNumber : 0
                    //                   AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault() //s.AccreditationDetails.Select(a => a.AccreditationNumber).Aggregate((a, x) => a + ", " + x)
                    //               }).Distinct();

                   
                    NIELITMISContext context1 = new NIELITMISContext();




                    if (!String.IsNullOrEmpty(searchString))
                    {
                        centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
                    }
                    if (stateID != 0)
                    {
                        centre = centre.Where(s => s.StateID == stateID);
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
                            case "Name":
                                if (sortOrder == "DESC")
                                    centre = centre.OrderByDescending(s => s.Name);
                                else
                                    centre = centre.OrderBy(s => s.Name);
                                break;
                            case "Location":
                                if (sortOrder == "DESC")
                                    centre = centre.OrderByDescending(s => s.Location);
                                else
                                    centre = centre.OrderBy(s => s.Location);
                                break;
                            case "MobileNumber":
                                if (sortOrder == "DESC")
                                    centre = centre.OrderByDescending(s => s.AccreditationNumber);
                                else
                                    centre = centre.OrderBy(s => s.AccreditationNumber);
                                break;

                            default:
                                centre = centre.OrderBy(s => s.ID);
                                break;
                        }

                    }
                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        //centre = (from c in centre
                        //          join r in context.AccreditationDetails
                        //              on c.ID equals r.InstituteID
                        //          where roleCourses.Contains(r.CourseID)
                        //          select new

                        //           {
                        //               ID = c.ID,
                        //               Location = c.Location,
                        //               City1 = c.City1,
                        //               StateID = c.StateID,
                        //               Name = c.Name,
                        //               ContactPersonName = c.ContactPersonName,
                        //               AccreditationNumber = c.AccreditationNumber
                        //           });

                        //var centre1 = (from s in context.Institutes
                        //               where s.AccreditationDetails.Count <= 0
                        //               select new
                        //               {
                        //                   ID = s.ID,
                        //                   Location = s.CityName.ToUpper() + ", " + s.State.Name.ToUpper(),
                        //                   City1 = s.CityName,
                        //                   StateID = s.StateID,
                        //                   Name = s.Name,
                        //                   ContactPersonName = s.ContactPersonName,
                        //                   AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault()
                        //               }).Distinct();
                       // centre = centre.Union(centre1);
                    }
                    PagingBar1.Bind(centre, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }

                    if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                    {
                        gvMain.Columns[4].Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            DisplayRow(false);
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            txtInstituteID.Enabled = false;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            RdoYesNo.Enabled = true;
            //Change the heading text as required
            lblHeading.Text = "Institute Accredited For Aspirational District";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Accredited Centre", "#", ""));
        }
        else
        {
            txtInstituteID.Enabled = false;
            Response.Redirect("AdminAccrediatedAsprDcenter.aspx", true);
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
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                NIELITMIS CheckDistrict = new NIELITMIS();
                int districtId = Convert.ToInt32(ddldistrict.SelectedValue);
                using (DataTable dt = CheckDistrict.DistrictsExists(districtId))
                {
                    if (dt.Rows.Count == 0)
                    {
                        ShowAlert(ddldistrict.SelectedItem + "  does not fall in the category of Aispirational District.");
                        return;
                    }
                }             

                Institute objCentre;
                Int64 instituteid = Convert.ToInt64(txtInstituteID.Text);
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (!context.Institutes.Any(s => s.ID == instituteid))
                    {
                        objCentre = new Institute();
                        
                        objCentre.ID = Convert.ToInt64(txtInstituteID.Text);                      

                        objCentre.Name = txtaccentre.Text.ToUpper();
                        objCentre.ContactPersonName = txtcontactperson.Text.ToUpper();
                        objCentre.ContactPersonPost = txtdesignation.Text.ToUpper();
                        objCentre.MobileNumber = Convert.ToInt64(txtmobile.Text);
                        if (txtfaxno.Text.Trim() != "")
                            objCentre.FaxNumber = Convert.ToInt32(txtfaxno.Text);
                        else
                            objCentre.FaxNumber = null;
                        if (txtstdno.Text.Trim() != "")
                            objCentre.StdNumber = Convert.ToInt32(txtstdno.Text);
                        else
                            objCentre.StdNumber = null;
                        if (txtphone1.Text.Trim() != "")
                            objCentre.PhoneNumber1 = Convert.ToInt32(txtphone1.Text);
                        else
                            objCentre.PhoneNumber1 = null;
                        if (txtphone2.Text.Trim() != "")
                            objCentre.PhoneNumber2 = Convert.ToInt32(txtphone2.Text);
                        else
                            objCentre.PhoneNumber2 = null;
                        if (txtemail1.Text.Trim() != "")
                            objCentre.EmailAddress1 = txtemail1.Text.ToString();
                        else
                            objCentre.EmailAddress1 = null;
                        if (txtemail2.Text.Trim() != "")
                            objCentre.EmailAddress2 = txtemail2.Text.ToString();
                        else
                            objCentre.EmailAddress2 = null;
                        if (txtwebaddress.Text.Trim() != "")
                            objCentre.WebAddress = txtwebaddress.Text.ToString();
                        else
                            objCentre.WebAddress = null;
                        objCentre.WebAddress = txtwebaddress.Text;

                        objCentre.AddressLine1 = txtadd1.Text;
                        objCentre.AddressLine2 = txtadd2.Text;
                        if (txtadd3.Text.Trim() != "")
                            objCentre.AddressLine3 = txtadd3.Text.ToString();
                        else
                            objCentre.AddressLine3 = null;
                        if (txtcity.Text.Trim() != "")
                            objCentre.CityName = txtcity.Text.ToString();
                        else
                            objCentre.CityName = null;
						
						 //Added 13 Feb 2019
                        objCentre.cityTypeID = Convert.ToInt32(ddlCityType.SelectedValue);
                        objCentre.StateID = Convert.ToInt64(ddlstate.SelectedValue);
                        objCentre.DistrictID = Convert.ToInt64(ddldistrict.SelectedValue);
                        objCentre.PinCode = Convert.ToInt32(txtpinno.Text);
                        context.Institutes.Add(objCentre);
                        context.SaveChanges();
                        strMessage = "New record saved.";

                        //Create User
                        int userType = 4;
                        Int64 INSTID= Convert.ToInt64(txtInstituteID.Text);
                        if (context.Users.Where(a => a.UserTypeID == userType && a.UserRefNumber == INSTID).Count() <= 0)
                        {
                            User user = new User();
                            user.OrganizationID = 1;
                            //if (ddlCourseCategory.SelectedIndex == 6)
                            //{
                            //    user.LoginID = accNumber.Substring(0, 9);
                            //}
                            //else
                            //{
                            //    user.LoginID = accNumber;
                            //}
                            string accNumber = "AD-" + txtInstituteID.Text.Trim();
                            user.LoginID = accNumber;
                            user.UserName = txtaccentre.Text.ToUpper();
                            user.Password = UserManager.ComputeSha256Hash(accNumber).ToUpper();//FormsAuthentication.HashPasswordForStoringInConfigFile(accNumber, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                            user.UserTypeID = userType;
                            user.UserRefNumber = Convert.ToInt64(txtInstituteID.Text);
                            user.EmailID = txtemail1.Text.ToString(); 
                            user.PasswordExpiryDays = 0;
                            user.LastPasswordChangedOn = DateTime.Now;
                            user.FailedLoginAttempts = 0;
                            user.CreatedBy = Convert.ToInt32(Session["UserID"]);
                            user.CreatedOn = DateTime.Now;
                            user.HasLoginAccess = true;
                            user.DefaultRoleID = Convert.ToInt32(enmRole.AdminInstitute);
                            context.Users.Add(user);
                            context.SaveChanges();


                            String EmailMsg = "Dear " + txtaccentre.Text.ToUpper() + ",  The userid and password for login to Student.nielit.gov.in is Userid : " + accNumber + "  and  Password:.  " + accNumber + ". Please change your password after first Login.";
                           

                            //sending Email 
                            if (txtemail1.Text.ToString().Length > 0)
                            {
                                try
                                {
                                    EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, txtemail1.Text.ToString());
                                    mail.Send();
                                }
                                catch { ShowAlert("Login Id and Password are sent on E-mail."); }
                            }

                          
                        }
                        //context.SaveChanges();

                    }
                    else
                    {
                        ShowAlert("Institute Already exists with this Instiute-ID:-" + Convert.ToInt64(txtInstituteID.Text));
                        return;
                    }
                }
                else
                {
                    objCentre = context.Institutes.Find(Convert.ToInt32(Request.QueryString["key"]));

                    //btnSave.Enabled = false;

                    objCentre.Name = txtaccentre.Text.ToUpper();
                    objCentre.ContactPersonName = txtcontactperson.Text.ToUpper();
                    objCentre.ContactPersonPost = txtdesignation.Text.ToUpper();
                    objCentre.MobileNumber = Convert.ToInt64(txtmobile.Text);
                    if (txtfaxno.Text.Trim() != "")
                        objCentre.FaxNumber = Convert.ToInt32(txtfaxno.Text);
                    else
                        objCentre.FaxNumber = null;
                    if (txtstdno.Text.Trim() != "")
                        objCentre.StdNumber = Convert.ToInt32(txtstdno.Text);
                    else
                        objCentre.StdNumber = null;
                    if (txtphone1.Text.Trim() != "")
                        objCentre.PhoneNumber1 = Convert.ToInt32(txtphone1.Text);
                    else
                        objCentre.PhoneNumber1 = null;
                    if (txtphone2.Text.Trim() != "")
                        objCentre.PhoneNumber2 = Convert.ToInt32(txtphone2.Text);
                    else
                        objCentre.PhoneNumber2 = null;
                    if (txtemail1.Text.Trim() != "")
                        objCentre.EmailAddress1 = txtemail1.Text.ToString();
                    else
                        objCentre.EmailAddress1 = null;
                    if (txtemail2.Text.Trim() != "")
                        objCentre.EmailAddress2 = txtemail2.Text.ToString();
                    else
                        objCentre.EmailAddress2 = null;
                    if (txtwebaddress.Text.Trim() != "")
                        objCentre.WebAddress = txtwebaddress.Text.ToString();
                    else
                        objCentre.WebAddress = null;
                    objCentre.AddressLine1 = txtadd1.Text;
                    objCentre.AddressLine2 = txtadd2.Text;
                    if (txtadd3.Text.Trim() != "")
                        objCentre.AddressLine3 = txtadd3.Text.ToString();
                    else
                        objCentre.AddressLine3 = null;
                    if (txtcity.Text.Trim() != "")
                        objCentre.CityName = txtcity.Text.ToString();
                    else
                        objCentre.CityName = null;
					
					 //Added 13 Feb 2019
                        objCentre.cityTypeID = Convert.ToInt32(ddlCityType.SelectedValue);
                    objCentre.StateID = Convert.ToInt64(ddlstate.SelectedValue);
                    objCentre.DistrictID = Convert.ToInt64(ddldistrict.SelectedValue);
                    objCentre.PinCode = Convert.ToInt32(txtpinno.Text);
                    strMessage = "Record updated.";
                    context.SaveChanges();
                }
            };
            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("AdminAccrediatedAsprDcenter.aspx?msg=" + strMessage);

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
            ddlAccentre.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
                Institute institute = context.Institutes.Find(Convert.ToInt32(hfActionID.Value));
                context.Institutes.Remove(institute);
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
            ShowAlert("Record can not be delted!");
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
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var centre = from s in context.Institutes
                         select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.Name).Distinct();
            foreach (var course in centre)
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
        Response.Redirect("AdminAccrediatedAsprDcenter.aspx", true);
    }
    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id1 = Convert.ToInt32(ddlstate.SelectedValue);
        ddldistrict.Items.Clear();
        BindDistrict(id1);
    }

}