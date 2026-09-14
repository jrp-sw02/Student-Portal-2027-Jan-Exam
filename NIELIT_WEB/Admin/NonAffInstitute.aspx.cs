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

public partial class Admin_NonAffInstitute : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    NIELITMISContext context1;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    

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
            entityID = Convert.ToInt32(Session["EntityID"]);

            

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
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Non Accredited", "Admin/NonAffInstitute.aspx", ""));
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
        //Added 19-Dec-2018
        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
        {
            NonAffInstitute objCentre = null;
            context1 = new NIELITMISContext();
            using (var context = new EConnectContext())
            {
                //Added 19-Dec-2018
                if (objCentre == null)
                    objCentre = context1.NonAffInstitutes.Find(Convert.ToInt64(Request.QueryString["key"]));
                //
                ListItem lst = new ListItem("--Select One--", "0");
                var state = from s in context.Locations
                            where s.LocationTypeID == 2
                            
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlstate, state, lst);

            }
        }
        else
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

    }
    public void BindDistrict(int id)
    {
        //Added 19-Dec-2018
        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
        {
            NonAffInstitute objCentre = null;
            context1 = new NIELITMISContext();

            using (var context = new EConnectContext())
            {
                //Added 19-Dec-2018
                if (objCentre == null)
                    objCentre = context1.NonAffInstitutes.Find(Convert.ToInt32(Request.QueryString["key"]));
                //
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
        else
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
    }
    protected void FillFilter()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var statelist = from p in context.NonAffInstitutes
                                where p.enterBy == loginUserNo
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
    protected void ShowEditMode()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            btnCancel.Text = "Back";
            lblHeading.Text = "Non Accredited Institute";
            tblNavLinks.Visible = true;

            using (NIELITMISContext context1 = new NIELITMISContext())
            {
                Int32 centreId = Convert.ToInt32(Request.QueryString["Key"]);
                txtInstituteID.Text = centreId.ToString();
                txtInstituteID.Enabled = false;
                var centre = (from p in context1.NonAffInstitutes
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


                txtInstituteID.Enabled = false;
                txtaccentre.Enabled = false;
                txtcontactperson.Enabled = false;
                txtdesignation.Enabled = false;
                txtmobile.Enabled = false;
                txtfaxno.Enabled = false;
                txtstdno.Enabled = false;
                txtphone1.Enabled = false;
                txtphone2.Enabled = false;
                txtemail1.Enabled = false;
                txtemail2.Enabled = false;
                txtwebaddress.Enabled = false;
                txtadd1.Enabled = false;
                txtadd2.Enabled = false;
                txtadd3.Enabled = false;
                txtcity.Enabled = false;
                ddlCityType.Enabled = false;
                ddlstate.Enabled = false;
                ddldistrict.Enabled = false;
                txtpinno.Enabled = false;

                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(centre.Name, "Admin/NonAffInstitute.aspx?" + Request.QueryString.ToString(), ""));
                //hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("accrediationdetails.aspx?key1=" + Request.QueryString["Key"] + "&name=" + Request.QueryString["Name"]);
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("NonAffInstitute.aspx?key1=" + Request.QueryString["Key"]);


                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("NonAffiliatedInstituteCourses.aspx?key1=" + Request.QueryString["Key"]);     
                
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
            con.Close();
            // context.Dispose();
        }
    }
    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            lblError.Visible = false;
            context1 = new NIELITMISContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlAccentre.SelectedValue != "0")
                stateID = Convert.ToInt64(ddlAccentre.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var centre = (from s in context1.NonAffInstitutes
                          where s.enterBy == loginUserNo
                          select new
                          {
                              ID = s.ID,
                              Location = s.CityName.ToUpper() ,//+ ", " + s.State.Name.ToUpper(),
                              City1 = s.CityName,
                              StateID = s.StateID,
                              Name = s.Name,
                              ContactPersonName = s.ContactPersonName,
                              MobileNumber = s.MobileNumber.HasValue ? s.MobileNumber : 0
                              //AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault() //s.AccreditationDetails.Select(a => a.AccreditationNumber).Aggregate((a, x) => a + ", " + x)
                          }).Distinct();

            DataTable DT = new DataTable();

     
            con.Open();
            SqlParameter param;
            using (SqlCommand Cmm = new SqlCommand("BindGridNonACC", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                param = new SqlParameter("@loginUserNo", loginUserNo);
                Cmm.Parameters.Add(param);
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);


                Sda.Fill(DT);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                string expression = "[Name] like '%" + searchString + "%'";

                if (DT.Select(expression).Count() > 0)
                    DT = DT.Select(expression).CopyToDataTable();
                else
                {
                    ShowAlert("No record found", true);

                }

            }

            if (ddlAccentre.SelectedValue.ToString() != "0")
            {
                string expression = "[ID]=" + Convert.ToInt32(ddlAccentre.SelectedValue);
                if (DT.Select(expression).Count() > 0)
                    DT = DT.Select(expression).CopyToDataTable();
                else
                {
                    ShowAlert("No record found", true);

                }
            }

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
            //}
            //if (stateID != 0)
            //{
            //    centre = centre.Where(s => s.StateID == stateID);
            //}
            //if (!string.IsNullOrEmpty(sortOrder))
            //{
            //    switch (sortField)
            //    {
            //        case "ID":
            //            if (sortOrder == "DESC")
            //                centre = centre.OrderByDescending(s => s.ID);
            //            else
            //                centre = centre.OrderBy(s => s.ID);
            //            break;
            //        case "Name":
            //            if (sortOrder == "DESC")
            //                centre = centre.OrderByDescending(s => s.Name);
            //            else
            //                centre = centre.OrderBy(s => s.Name);
            //            break;
            //        case "Location":
            //            if (sortOrder == "DESC")
            //                centre = centre.OrderByDescending(s => s.Location);
            //            else
            //                centre = centre.OrderBy(s => s.Location);
            //            break;
            //        //case "MobileNumber":
            //        //    if (sortOrder == "DESC")
            //        //        centre = centre.OrderByDescending(s => s.AccreditationNumber);
            //        //    else
            //        //        centre = centre.OrderBy(s => s.AccreditationNumber);
            //        //    break;

            //        default:
            //            centre = centre.OrderBy(s => s.ID);
            //            break;
            //    }

            //}
            //if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            //{
            //    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
            //    centre = (from c in centre
            //              join r in context.AccreditationDetails
            //                  on c.ID equals r.InstituteID
            //              where roleCourses.Contains(r.CourseID)
            //              select new
            //              {
            //                  ID = c.ID,
            //                  Location = c.Location,
            //                  City1 = c.City1,
            //                  StateID = c.StateID,
            //                  Name = c.Name,
            //                  ContactPersonName = c.ContactPersonName,
            //                  MobileNumber = c.MobileNumber.HasValue ? c.MobileNumber : 0
            //              }).Distinct();

            //    var centre1 = (from s in context.Institutes
            //                   where s.AccreditationDetails.Count <= 0
            //                   select new
            //                   {
            //                       ID = s.ID,
            //                       Location = s.CityName.ToUpper() + ", " + s.State.Name.ToUpper(),
            //                       City1 = s.CityName,
            //                       StateID = s.StateID,
            //                       Name = s.Name,
            //                       ContactPersonName = s.ContactPersonName,
            //                       MobileNumber = s.MobileNumber.HasValue ? s.MobileNumber : 0
            //                       //AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault()
            //                   }).Distinct();
            //    centre = centre.Union(centre1);
            //}
            PagingBar1.Bind(DT, ref gvMain);
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
            //txtInstituteID.Enabled = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Non Accredited Institute";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Non Accredited Institute", "#", ""));
        }
        else
        {
            //txtInstituteID.Enabled = false;
            Response.Redirect("NonAffInstitute.aspx", true);
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
        //context = new NIELITMISContext();
        //NonAffInstitute application;

        try
        {
            //if (System.Text.RegularExpressions.Regex.IsMatch(txtaccentre.Text, "^[a-zA-Z ]"))
				 if (!System.Text.RegularExpressions.Regex.IsMatch(txtaccentre.Text, @"^[a-zA-Z ]+$"))
            {
                txtaccentre.Focus();
                txtaccentre.Text = "";
                throw new Exception("Institute name accepts only alphabetical characters");
            }

            BreadCrumb1.Render();
            //using (TransactionScope scope = new TransactionScope())
            //{
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NonAffInstitute objCentre;
                    NIELITMIS lnkMIS = new NIELITMIS();
                    Int64 instituteid = 0; //Convert.ToInt64(txtInstituteID.Text);
                    Int32 lnkID = 0;
                    lnkID = entityID;
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        //if (!context.NonAffInstitutes.Any(s => s.ID == instituteid))
                        //{
                            objCentre = new NonAffInstitute();
                            //objCentre.ID = Convert.ToInt64(txtInstituteID.Text);
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

                            objCentre.linkedToCentre = lnkID;
                            objCentre.enterDate = DateTime.Now;
                            objCentre.enterBy = loginUserNo;

                            context.NonAffInstitutes.Add(objCentre);
                            context.SaveChanges();

                            string uid = objCentre.ID.ToString();

                            /////////////////////// User Create Account ///////////////////

                            User objUser;

                            using (EConnectContext context1 = new EConnectContext())
                            {
                                if (context1.Users.Any(u => u.LoginID == uid) == true)
                                    throw new Exception("UserID is alreay exist. Please give another UserID");

                                objUser = new EConnect.URM.User();
                                objUser.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                objUser.CreatedOn = DateTime.Now;
                                objUser.HasLoginAccess = true;
                                objUser.LoginID = uid;
                                objUser.OrganizationID = Convert.ToInt32(Session["OrgID"]);
                                string password = EConnect.Utils.Security.RandomPassword.Generate(6, 8);
                                objUser.Password = UserManager.ComputeSha256Hash(password).ToUpper(); //.HashPasswordForStoringInConfigFile(password, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                                objUser.PasswordExpiryDays = 10;
                                objUser.LastPasswordChangedOn = DateTime.Now;
                                objUser.FailedLoginAttempts = 0;
                                objUser.UserName = txtaccentre.Text;
                                //objUser.EmailID = txtEmail.Text;
                                objUser.EmailID = txtemail1.Text;
                                objUser.MobileNumber = Convert.ToInt64(txtmobile.Text);

                                objUser.UserTypeID = 11 ; //NonAccredited Ins Type ID 
                                objUser.UserRefNumber = Convert.ToInt64(uid);
                                objUser.DefaultRoleID = 26;
                                context1.Users.Add(objUser);
                                context1.SaveChanges();

                                //User Roles
                                EConnect.URM.UserRole userRole = new EConnect.URM.UserRole();
                                userRole.UserID = objUser.UserID;
                                userRole.RoleID = 26;
                                userRole.CreatedOn = DateTime.Now;
                                userRole.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                context1.UserRoles.Add(userRole);
                                context1.SaveChanges();

                                CommonFunctions.SendAccountActivationEmail(objUser.UserID, false, password);
                            }
                            ////////////////////////////////////////////////////
                            
                            strMessage = "New record saved.";
                        //}
                        //else
                        //{
                        //    ShowAlert("Institute Already exists with this Instiute-ID:-" + Convert.ToInt64(txtInstituteID.Text));
                        //    return;
                        //}
                    }
                    else
                    {
                        objCentre = context.NonAffInstitutes.Find(Convert.ToInt32(Request.QueryString["key"]));

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

                        objCentre.linkedToCentre = lnkID;
                        objCentre.enterDate = DateTime.Now;
                        objCentre.enterBy = loginUserNo;

                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                };
                //Call save method
                //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
                //Redirect it to list mode
                Response.Redirect("NonAffInstitute.aspx?msg=" + strMessage);
           // };
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
        NIELITMISContext context1 = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var centre = from s in context1.NonAffInstitutes
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
        finally { context1.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NonAffInstitute.aspx", true);
    }
    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id1 = Convert.ToInt32(ddlstate.SelectedValue);
        ddldistrict.Items.Clear();
        BindDistrict(id1);
    }
}