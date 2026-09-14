using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class Admin_AdminRegstude : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    String currentRoleName = string.Empty;
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
            currentRoleName = (string)Session["RoleName"];
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/SearchCandidate.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                Session["level"] = null;
                if (!String.IsNullOrEmpty(Request.QueryString["ApplID"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    Filltype();
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
    protected void ShowEditMode()
    {

        try
        {
            showsidelink();
            context = new EConnectContext();
            Int64 Regno = Convert.ToInt64(Request.QueryString["Regno"]);
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            var cr = (from s in context.RegistrationDetails
                      where s.RegistrationNo == Regno && s.CourseID == CourseId
                      orderby s.RegistrationDate descending
                     select s).FirstOrDefault();
            if (cr != null)
            {
                LblAppName.Text = cr.Candidate.Salutation + " " + GetInitCap(cr.Candidate.Name);
                if (string.IsNullOrEmpty(cr.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(cr.Candidate.GuardianName) == true)
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    if (string.IsNullOrEmpty(cr.Candidate.FatherName) == false && !string.IsNullOrWhiteSpace(cr.Candidate.FatherName))
                        LblFatherName.Text = "Mr. " + GetInitCap(cr.Candidate.FatherName);
                    else
                        LblFatherName.Text = "NA";
                    if (string.IsNullOrEmpty(cr.Candidate.MotherName) == false && string.IsNullOrWhiteSpace(cr.Candidate.MotherName) == false)
                        LblMotherName.Text = "Mrs. " + GetInitCap(cr.Candidate.MotherName);
                    else
                        LblMotherName.Text = "NA";
                }
                else
                {
                    LblGuardianName.Text = string.IsNullOrEmpty(cr.Candidate.GuardianName) == false && string.IsNullOrWhiteSpace(cr.Candidate.GuardianName) == false ? GetInitCap(cr.Candidate.GuardianName) : "NA";
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    TrName.Attributes.Add("class", "gdalternate1");
                }
                LblGender.Text = string.IsNullOrEmpty(cr.Candidate.Gender) == false && !string.IsNullOrWhiteSpace(cr.Candidate.Gender) ? GetInitCap(cr.Candidate.Gender) : "NA";
                LblMaritalStatus.Text = (cr.Candidate.MaritalStatusID.HasValue) ? GetInitCap(cr.Candidate.MaritalStatus.Name) : "-";
                LblDob.Text = cr.Candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                LblCategory.Text = (cr.Candidate.CastCategoryID.HasValue) ? GetInitCap(cr.Candidate.CastCategory.Name) : "-";
                LblBodyMark.Text = string.IsNullOrEmpty(cr.Candidate.BodyMark) == false && !string.IsNullOrWhiteSpace(cr.Candidate.BodyMark) ? GetInitCap(cr.Candidate.BodyMark) : "NA";
                LblOccuption.Text = cr.Candidate.OccupationID.HasValue && cr.Candidate.OccupationID != 0 ? GetInitCap(cr.Candidate.Occupation.Name) : "NA";
                LblIsHandicaped.Text = cr.Candidate.IsHandicaped == false ? "No" : "Yes";
                LblIsExServicemane.Text = cr.Candidate.IsExServicemane == false ? "No" : "Yes";

                if (cr.Candidate.Photo != null)
                    ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Candidate.Photo.BlobFile);
                foreach (CandidateContactDetail contact in cr.Candidate.ContactDetails)
                {
                    LblMobile.Text = (contact.MobileNumber.HasValue ? contact.MobileNumber.Value.ToString() : "NA");
                    LblPhone.Text = contact.PhoneNumber.HasValue && contact.PhoneNumber != 0 ? "0" + contact.StdNumber.ToString() + "-" + contact.PhoneNumber.ToString() : "NA";
                    LblEmail.Text = string.IsNullOrEmpty(contact.EmailAddress) == false && !string.IsNullOrWhiteSpace(contact.EmailAddress) ? contact.EmailAddress.ToString() : "NA";
                }
                foreach (Address add in cr.Candidate.Addresses)
                {
                    if (add.AddressTypeID == Convert.ToInt32(enmAddressType.PermanentAddress))
                    {
                        LblPerAddressLine1.Text = string.IsNullOrEmpty(add.AddressLine1) == false && !string.IsNullOrWhiteSpace(add.AddressLine1) ? GetInitCap(add.AddressLine1) : "NA";
                        LblPerAddressLine2.Text = string.IsNullOrEmpty(add.AddressLine2) == false && !string.IsNullOrWhiteSpace(add.AddressLine2) ? GetInitCap(add.AddressLine2) : "NA";
                        LblPerAddressLine3.Text = string.IsNullOrEmpty(add.AddressLine3) == false && !string.IsNullOrWhiteSpace(add.AddressLine3) ? GetInitCap(add.AddressLine3) : "NA";
                        LblPerCityName.Text = string.IsNullOrEmpty(add.CityName) == false && !string.IsNullOrWhiteSpace(add.CityName) ? GetInitCap(add.CityName) : "NA";
                        LblPerStateName.Text = add.StateID.HasValue && add.StateID != 0 ? add.State.Name : "NA";
                        if (add.DistrictID.HasValue)
                            LblPerDistrictName.Text = GetInitCap(add.District.Name);
                        else
                            LblPerDistrictName.Text  = "NA";
                        LblPerPinCodeNumber.Text = add.PinCode.HasValue ? add.PinCode.Value.ToString() : "NA";
                    }

                    if (add.AddressTypeID == Convert.ToInt32(enmAddressType.CorrespondenceAddress))
                    {
                        LblCorAddressLine1.Text = string.IsNullOrEmpty(add.AddressLine1) == false && !string.IsNullOrWhiteSpace(add.AddressLine1) ? GetInitCap(add.AddressLine1) : "NA";
                        LbCorlAddressLine2.Text = string.IsNullOrEmpty(add.AddressLine2) == false && !string.IsNullOrWhiteSpace(add.AddressLine2) ? GetInitCap(add.AddressLine2) : "NA";
                        LblCorAddressLine3.Text = string.IsNullOrEmpty(add.AddressLine3) == false && !string.IsNullOrWhiteSpace(add.AddressLine3) ? GetInitCap(add.AddressLine3) : "NA";
                        LblCityName.Text = string.IsNullOrEmpty(add.CityName) == false && !string.IsNullOrWhiteSpace(add.CityName) ? GetInitCap(add.CityName) : "NA";
                        lblStateName.Text = add.StateID.HasValue && add.StateID != 0 ? add.State.Name : "NA";
                        if (add.DistrictID.HasValue)
                            lblDistrictName.Text = GetInitCap(add.District.Name);
                        else
                            lblDistrictName.Text = "NA";
                        lblPinCodeNumber.Text = add.PinCode.HasValue ? add.PinCode.Value.ToString() : "NA";

                    }
                }

                foreach (CandidateQualificationDetail Qualification in cr.Candidate.EducationalQualifications)
                {
                    LblHeighestEducation.Text =  string.IsNullOrEmpty(Qualification.EducationalQualification.Name) == false && !string.IsNullOrWhiteSpace(Qualification.EducationalQualification.Name) ? GetInitCap(Qualification.EducationalQualification.Name):"NA";
                    LblYrOfPassing.Text = (Qualification.PassingYear.HasValue ? Qualification.PassingYear.Value.ToString() : "NA");

                }
                LblRegNo.Text = cr.RegistrationNo.ToString();
                LblRegDate.Text = cr.RegistrationDate.ToString("dd-MMM-yyyy");
                LblComDate.Text = cr.CommencementFromDate.ToString("dd-MMM-yyyy");
                LblValidUpto.Text = cr.ValidUptoDate.ToString("dd-MMM-yyyy");
                LblCourse.Text = GetInitCap(cr.Course.Name);
                if (cr.ApplicantTypeID != 0)
                {
                    LblCandidateType.Text = cr.ApplicantType.Name.ToString();
                    if (cr.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        TrAccCentre.Visible = true;
                        LblAccCentre.Text = GetInitCap(cr.Institute.Name) + "(" + cr.Institute.AccreditationDetails.Where (c=>c.CourseID == cr.CourseID).FirstOrDefault().AccreditationNumber.ToString() + ")";
                        trstatus.Attributes.Add("class", "gdalternate1");
                    }
                    else
                    {
                        TrAccCentre.Visible = false;
                        trstatus.Attributes.Add("class", "gdrow1");
                    }
                }
                else
                {
                    LblCandidateType.Text = "-";
                    TrAccCentre.Visible = false;
                    trstatus.Attributes.Add("class", "gdrow1");
                }
                Lbregstatus.Text = cr.RegistrationStatusID.HasValue ? cr.RegistrationStatus.Name : "NA";
            }
            if (Request.UrlReferrer.ToString().ToLower().Contains("changecandidatephoto.aspx"))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.RemoveLastBreadCrumbItem();
            }
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Candidate.Salutation + cr.Candidate.Name.ToUpper(), "Admin/AdminRegstud.aspx?" + Request.QueryString.ToString(), ""));
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "Candidate Details";
            //Get last modified date of current record and save it in ViewState object.
           
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
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int32 AppTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 status = Convert.ToInt32(Request.QueryString["Status"]);
            ucSearchBar.AutoCompleteContextKey = CourseId.ToString() + "," + status.ToString();
            Int32 apptypeID = 0;
            if (ddlapptype.SelectedValue!="0")
                apptypeID = Convert.ToInt32(ddlapptype.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var registration = from c in context.RegistrationDetails.AsNoTracking()
                               where c.CourseID == CourseId && c.RegistrationStatusID == status
                               orderby c.CourseID
                               select new
                                   {
                                       ID = c.ID,
                                       ApplID = c.CandidateID,
                                       Regno = c.RegistrationNo,
                                       courseID = CourseId,
                                       Status = status,
                                       date = c.RegistrationDate,
                                       StudentName = c.Candidate.Salutation + c.Candidate.Name.ToUpper(),
                                       appname = c.Candidate.Name.ToLower(),
                                       FatherName = "Mr." + c.Candidate.FatherName.ToUpper(),
                                       CourseName = c.Course.Name.ToUpper(),
                                       ApplTypeID = AppTypeID,
                                       applicanttypeID = c.ApplicantTypeID,
                                       regname = (c.RegistrationStatusID.HasValue?c.RegistrationStatus.Name:"NA")
                                   };
                if (apptypeID != 0)
                    registration = registration.Where(q => q.applicanttypeID == apptypeID);
                if (!string.IsNullOrEmpty(searchString))
                {
                    registration = registration.Where((s => s.appname.ToUpper().Contains(searchString)));
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
                        default:
                            registration = registration.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(registration, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (registration.Count()==0)
                {
                    Course cs = context.Courses.Find(CourseId);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Name.ToUpper()+ ":-"+EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmRegistrationStatus) status).ToUpper(), "Admin/AdminRegstud.aspx?" + Request.QueryString.ToString(), ""));
                    BreadCrumb1.Render();
                    Lblerror.Visible = true;
                    Lblerror.Text = "No Record Found..";
                }
                else
                {
                    Course cs = context.Courses.Find(CourseId);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cs.Name.ToUpper() + ":-" + registration.FirstOrDefault().regname.ToUpper(), "Admin/AdminRegstud.aspx?" + Request.QueryString.ToString(), ""));
                    BreadCrumb1.Render();
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
            lblHeading.Text = "Registered Students";
        }
        else
        {
            BreadCrumb1.Render();
            if (!string.IsNullOrEmpty(Request.QueryString["Src"]))
            {
                Response.Redirect("../Common/SearchCandidate.aspx?regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&Status=" + Request.QueryString["Status"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"]);
            }
            else
            {
                Response.Redirect("AdminRegstud.aspx?CourseID=" + Request.QueryString["courseID"] + "&Status=" + Request.QueryString["Status"]);
            }
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
            //context = new EConnectContext();
            //User objUser;
            //if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            //{
            //    objUser = new EConnect.URM.User();
            //    objUser.CreatedBy = Convert.ToInt32(Session["UserID"]);
            //    objUser.CreatedOn = DateTime.Now;
            //    objUser.EmailID = txtEmail.Text;
            //    objUser.HasLoginAccess = Convert.ToBoolean(Convert.ToInt16(ddlStatus.SelectedValue));
            //    objUser.LoginID = txtUserId.Text.ToUpper();
            //    objUser.OrganizationID = Convert.ToInt32(hfDeptId.Value.Split('$')[0]);
            //    objUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtUserId.Text.ToLower(), System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
            //    objUser.DefaultModuleID = 5;
            //    objUser.PasswordExpiryDays = Convert.ToInt32(ddlExpiryDays.SelectedValue);
            //    objUser.LastPasswordChangedOn = DateTime.Now;
            //    objUser.FailedLoginAttempts = 0;
            //    objUser.UserName = txtUserName.Text.ToUpper();
            //    objUser.UserTypeID = Convert.ToInt32(ddlUserType.SelectedValue);
            //    objUser.UserRefNumber = Convert.ToInt32(ddlEntity.SelectedValue);

            //    context.Users.Add(objUser);
            //    context.SaveChanges();
            //    strMessage = "New record saved.";
            //}
            //else
            //{
            //    ////Initialize current object by loading it and get its current modified date
            //    //DateTime modifiedOn = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //    ////Caompare last modified date saved in viewstate object with current modified date.
            //    //if (Convert.ToDateTime(ViewState["LastModifiedOn"]) == modifiedOn)
            //    //{
            //    //Set modifiedBy and ModifiedOn property of current record
            //    //objMenuObject.ModifiedBy = Convert.ToInt64(Session["UserNo"]);
            //    //objMenuObject.ModifiedOn = DateTime.Now;

            //    objUser = context.Users.Find(Convert.ToInt32(Request.QueryString["key"]));
            //    objUser.EmailID = txtEmail.Text;
            //    objUser.HasLoginAccess = Convert.ToBoolean(Convert.ToInt16(ddlStatus.SelectedValue));
            //    objUser.PasswordExpiryDays = Convert.ToInt32(ddlExpiryDays.SelectedValue);
            //    objUser.UserName = txtUserName.Text.ToUpper();
            //    strMessage = "Record updated.";
            //    context.SaveChanges();
            //    //}
            //    //else
            //    //    throw new Exception("Record changed by some other person. Please reopen the record in edit mode and update again.");

            //}

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("AdminRegstud.aspx?msg=" + strMessage);
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
    protected void LnkBtnPhotoDetail_Click(object sender, EventArgs e)
    {
        if (currentRoleName == "Technical Support Query")
         {
             Response.Write("Sorry! You don't have rights  to view this page");
             Response.End();
               
         }
            else
            {
                try
                {
                    Int64 Regno = Convert.ToInt64(Request.QueryString["Regno"]);
                    Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
                    using (EConnectContext context = new EConnectContext())
                    {
                        var candidate = (from r in context.RegistrationDetails
                                         where r.RegistrationNo == Regno && r.CourseID == CourseId
                                         select r).FirstOrDefault();
                        if (candidate != null)
                        {
                            Response.Redirect("ChangeCandidatePhoto.aspx?CandidateID=" + candidate.CandidateID + "&Regno=" + Request.QueryString["Regno"] + "&CourseId=" + Request.QueryString["CourseId"] + "&regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&Status=" + Request.QueryString["Status"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"], true);
                        }
                    };
                }
                catch (Exception ex)
                {
                    ShowAlert(ex.Message);
                }
            }        
       
    }
    protected void ToggleImage(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            string arg = btn.CommandArgument.ToLower();
            Int64 Regno = Convert.ToInt64(Request.QueryString["Regno"]);
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
            using (EConnectContext context = new EConnectContext())
            {

                var reg = (from s in context.RegistrationDetails
                                 where s.RegistrationNo == Regno && s.CourseID == CourseId
                                 select s).FirstOrDefault();
                if (arg == "p")
                {
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        LblPhotoCaption.Text = "Photograph";
                        ImgBtnPrevious.Enabled = false;
                        ImgBtnNext.Enabled = true;
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        LblPhotoCaption.Text = "Signature";
                        ImgBtnPrevious.Enabled = true;
                        ImgBtnNext.Enabled = true;
                    }
                }
                else
                {
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        LblPhotoCaption.Text = "Signature";
                        ImgBtnPrevious.Enabled = true;
                        ImgBtnNext.Enabled = true;
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        LblPhotoCaption.Text = "Thumb";
                        ImgBtnNext.Enabled = false;
                        ImgBtnPrevious.Enabled = true;
                    }
                }
                ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                ImgCandidatePhoto.Height = 130;
                ImgCandidatePhoto.Width = 112;
                if (LblPhotoCaption.Text.ToLower() == "photograph")
                {
                    ImgBtnPrevious.ToolTip = "";
                    ImgBtnNext.ToolTip = "Click to view signature.";
                    if (reg.Candidate.PhotoFileID.HasValue)
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])reg.Candidate.Photo.BlobFile);
                }
                if (LblPhotoCaption.Text.ToLower() == "signature")
                {
                    ImgBtnPrevious.ToolTip = "Click to view photograph";
                    ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                    if (reg.Candidate.SignatureFileID.HasValue)
                    {
                        ImgCandidatePhoto.Height = 50;
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])reg.Candidate.Signature.BlobFile);
                    }
                }
                else if (LblPhotoCaption.Text.ToLower() == "thumb")
                {
                    ImgBtnPrevious.ToolTip = "Click to view signature";
                    ImgBtnNext.ToolTip = "";
                    if (reg.Candidate.LeftThumbImpressionFileID.HasValue)
                    {
                        ImgCandidatePhoto.Height = 60;
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])reg.Candidate.LeftThumbImpression.BlobFile);
                    }
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlapptype.SelectedValue = "0";
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
    protected void showsidelink()
    {

        SideLink1.Items.Add(new SideLinkItem("Personal Detail", "AdminCandidatePersonaldetail.aspx?key1=" + Request.QueryString["ApplID"], "", ""));
        SideLink1.Items.Add(new SideLinkItem("Address Detail", "AdminCandidateAddressdetail.aspx?key1=" + Request.QueryString["ApplID"], "", ""));
        SideLink1.Items.Add(new SideLinkItem("Contact Detail", "AdminCandidateContactDetail.aspx?key1=" + Request.QueryString["ApplID"], "", ""));
        SideLink1.Items.Add(new SideLinkItem("Qualification Detail", "AdminCandidateQualificationDetail.aspx?key1=" + Request.QueryString["ApplID"], "", ""));
        SideLink1.Items.Add(new SideLinkItem("Courses", "AdminCourses.aspx?key1=" + Request.QueryString["ApplID"], "", ""));
        SideLink1.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
        SideLink1.Render();

    }
    protected void Filltype()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var app = from p in context.ApplicantTypes
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlapptype , app, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        if (!string.IsNullOrEmpty(Request.QueryString["Src"]))
        {
            Response.Redirect("../Common/SearchCandidate.aspx?regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&Status=" + Request.QueryString["Status"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"]+"&index="+Request.QueryString["index"]);
        }
        else
        {
           Response.Redirect("AdminRegstud.aspx?CourseID=" + Request.QueryString["courseID"] + "&Status=" + Request.QueryString["Status"]);
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count,String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            string[] array = contextKey.Split(',');
            Int32 courseId = Convert.ToInt32(array[0]);
            Int32 statusId = Convert.ToInt32(array[1]);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var users = (from s in context.RegistrationDetails
                        where s.CourseID == courseId && s.RegistrationStatusID == statusId
                        select new { Name = s.Candidate.Name.ToLower() }).Distinct().Take(count);
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);
            foreach (var user in users)
            {
                items.Add(user.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    
}