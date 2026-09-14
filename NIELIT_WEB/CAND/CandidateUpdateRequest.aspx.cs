using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

//
using System.Data;
using System.Net;
using System.IO;
using System.Web;

public partial class CAND_CandidateUpdateRequest : BasePage
{   

    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentCourseID = 0;
    Int64 RegApplID = 0; //2754656;//1951796;
    Course currentcourse;
    Int64 requestNo = 0; 
    String updateFieldID = string.Empty;
    String strMessage = string.Empty;

    #region[Page_Load_Event]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Text = "";
            lblerror.Visible = false;
            NormalHeader1.Visible = true;
            
            Int64 CandiID = Convert.ToInt64(Request.QueryString["candidateID"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["ID"]);

            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["candidateID"]))
            {
                entityID = Convert.ToInt64(Request.QueryString["candidateID"]);
            }
            else
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
            }

            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;

             //RegApplID  = Convert.ToInt64(Request.QueryString["RegApplID"]);
            using (EConnectContext context = new EConnectContext())
            {

                RegApplID = Convert.ToInt64(context.RegistrationDetails.Where(rd => rd.CandidateID == entityID).Select(rd => rd.ID).SingleOrDefault());
                currentcourse = context.RegistrationDetails.Find(RegApplID).Course;
                currentCourseID = currentcourse.ID;
                LblCourseLevel.Text = currentcourse.Name;
               
                if (entityID == 0)
                {
                    entityID = context.RegistrationDetails.Where(s => s.ID == RegApplID && s.CourseCategoryID == 1).Select(s => s.CandidateID).SingleOrDefault();
                }
            }
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["entityID"])))
                {

                    ////testing 
                    using (EConnectContext context = new EConnectContext())
                    {
                        try
                        {
                            if (context.candidateUpdateRequest.Any(o => o.candID == entityID && o.courseID == currentCourseID && o.paymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && o.requestStatusID == 3))
                            {

                                var ApplUpdateReq_Exist = (from p in context.candidateUpdateRequest
                                                               // orderby (p.DisplayOrder)
                                                           where p.candID == entityID && p.courseID == currentCourseID && p.paymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && p.requestStatusID == 3

                                                           select new { DID1 = p.demandNoteID, Request_Number1 = p.requestNo, candidateID1 = p.candID }).LastOrDefault();
                                Int64 DID = Convert.ToInt64(ApplUpdateReq_Exist.DID1);
                                Int64 Request_Number = Convert.ToInt64(ApplUpdateReq_Exist.Request_Number1);
                                Int64 candidateID = Convert.ToInt64(ApplUpdateReq_Exist.candidateID1);

                                Response.Redirect("~/CAND/CandidateUpdateConfirm.aspx?DID=" + DID + "&Request_Number=" + Request_Number + "&candidateID=" + candidateID);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblerror.Text = ex.Message;
                        }
                    }
                    //end testing

                }
                else
                { 
                    DisableAll();
                    DisplayAll();
                    //Lbregno.Text = Reg.RegistrationNo.ToString();
                   // bindCourse();
                   // bindDescription();
                    bindMaritalStatus();
                    bindCastCategory();
                    bindState();
                    bindEducationalQualification();

                    //if (!String.IsNullOrEmpty(Request.QueryString["RegApplID"]))
                    if (!String.IsNullOrEmpty(Request.QueryString["Request_Number"]))//new
                    {                       
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]); //new                      
                        Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
                        //Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
                       // UpdateSelection();
                        showEditRequestData();
                        Label2.Visible = false;
                        trEditHead.Visible = true;
                        btnSave.Text = "Update"; 
                        btnback.Visible = false;
                        //UpdateRequestData();
                    }
                    else
                    {
                        UpdateSelection();
                       trEditHead.Visible = false;
                        btnSave.Text = "Save/Preview";
                        btnback.Visible = true;
                        //InsertRequestData();
                    }
                   
                }
            }

            UpdateSelection();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    #endregion
    #region[Bind_All_DDL]
    protected void bindMaritalStatus()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var maritalStatus = from p in context.MaritalStatus
                                    orderby (p.DisplayOrder)
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlMStatus, maritalStatus, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
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
                                   select new { ValueField = p.ID, TextField = p.Name };//+ " / " + p.NameRegional 
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void bindState()
    {
        Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
        //Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");


                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, CorState, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
            bindDistrict(id2, ref ddldistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district = from s in context.Locations
                               orderby (s.Name)
                               where s.LocationTypeID == locationTypeID
                               && s.ParentLocationID == stateID
                               select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);
                if (ddl.Items.Count == 0)
                    ddl.Items.Add(lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void bindEducationalQualification()
    {
        //int courseID = 1;
       // int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
        using (EConnectContext vContext = new EConnectContext())
        {
            var EducationalQualification = vContext.EducationalQualifications.OrderBy(s => s.DisplayOrder).
                                            Join(vContext.QualificationEligibility.Where
                                            (q => q.CourseID == currentCourseID),
                                            e => e.QualificationLevelID, q => q.QualificationLevelID,
                                            (e, q) => new { ValueField = e.ID, TextField = e.Name }
                                            ).Distinct();
            

            EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, EducationalQualification, new ListItem("--Select One--", "0"));
        }
    }
    #endregion

    //protected void bindEducational()
    //{
    //    try
    //    {
    //        Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
    //        int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
    //        using (var context = new EConnectContext())
    //        {
    //            List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
    //            ListItem lst = new ListItem("--Select One--", "0");

    //            var education = from p in context.EducationalQualifications
    //                            join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
    //                            where q.CourseID == courseID
    //                            && q.ApplicantTypeID == ApplicantTypeId
    //                            select new { ValueField = p.ID, TextField = p.Name };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}

    //protected void bindDescription() 
    //{
    //    try
    //    {
    //        using (var context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var Desc = from p in context.reqStatusMass
    //                               orderby (p.reqStatusDesc)
    //                               select new { ValueField = p.ID, TextField = p.reqStatusDesc};
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlDescription, Desc, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    #region[Disable_Selection_Senerio_Request]
    protected void DisableAll()
    {
        Rdhandicapped.Enabled = false;
        ddlMStatus.Enabled = false;
       // ddlMStatus.SelectedValue = "0";

        ddlCategory.Enabled = false;
       // ddlCategory.SelectedValue = "0";

        txtCorMobileNo.Enabled = false;
        txtCorMobileNo.Text = "";
       
        txtEmailId.Enabled = false;
        txtEmailId.Text = "";
        TxtCorAddressLine1.Enabled = false;
        TxtCorAddressLine1.Text = "";
        TxtCorAddressLine2.Enabled = false;
        TxtCorAddressLine2.Text = "";
        TxtCorAddressLine3.Enabled = false;
        TxtCorAddressLine3.Text = "";
        TxtCorCity.Enabled = false;
        TxtCorCity.Text = "";

        ddlCorState.Enabled = false;
        //ddlCorState.SelectedValue = "0";

        ddldistrict.Enabled = false;
        //ddldistrict.SelectedValue = "0";

        txtCorPinCode.Enabled = false;
        txtCorPinCode.Text = "";
        DDLeducode.Enabled = false;

        //DDLeducode.SelectedValue = "0";
        TxtEduIfOther.Enabled = false;

        TxtEduIfOther.Text = "";
       // btnupdate.Visible = false;
        btnSave.Visible = true;

        ChkHandicapped.Checked = false;
        ChkMaritalStatus.Checked = false;
        ChkCategory.Checked = false;

        chkMob.Checked = false;
        ChkEmail.Checked = false;

        ChkCorAddress.Checked = false;

        //ChkAdd1.Checked = false;
        //ChkAdd2.Checked = false;
        //ChkAdd3.Checked = false;
        //ChkCity.Checked = false;
        //ChkState.Checked = false;
        //ChkDistrict.Checked = false;
        //ChkPin.Checked = false;

        ChkEducation.Checked = false;               
    }
    #endregion

    #region[Display_Candidate_Previous_MainData]
    protected void DisplayAll()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                RegistrationDetail RegID = context.RegistrationDetails.Find(RegApplID);


                    LblCourseLevel.Text = RegID.Course.Name;
                    Lbregno.Text = RegID.RegistrationNo.ToString();
                    Candidate candidate = RegID.Candidate;
                    if (RegID.Candidate.IsHandicaped)
                        LblHandicapped.Text = "Yes";
                    else
                        LblHandicapped.Text = "No";
                    //LblHandicapped.Text = RegID.Candidate.IsHandicaped.ToString();
                    LblMaritalStatus.Text = candidate.MaritalStatus.Name;
                    LblCategory.Text = candidate.CastCategory.Name;
                    //currentCourseID = currentcourse.ID;
                    currentCourseID = RegID.CourseID;
                    var registered = (from rg in context.RegistrationDetails
                                      where rg.CandidateID == entityID && rg.CourseID == currentCourseID && rg.RegistrationNo == RegID.RegistrationNo
                                      orderby rg.CommencementFromDate descending
                                      select rg).FirstOrDefault();
                    ICollection<CandidateContactDetail> contactDetails = candidate.ContactDetails;
                    if (contactDetails != null)
                    {
                        CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();

                        LblMobile.Text = (cd.MobileNumber.HasValue ? cd.MobileNumber.Value : 0).ToString();
                        LblEmail.Text = cd.EmailAddress;
                    }
                    Int32 corresspondence = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    Address add = candidate.Addresses.Where(d => d.AddressTypeID == corresspondence).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();

                    //if (add != null)
                    //{
                    //    Lblfulladd.Text = FullCorAddress(add);
                    //}
                    if (add.AddressTypeID == 1)
                   {
                    LblCorAddressLine1.Text = add.AddressLine1;
                    LblCorAddressLine2.Text = add.AddressLine2;
                    LblCorAddressLine3.Text = add.AddressLine3;
                    LblCorCity.Text = add.CityName;
                    LblCorState.Text = add.State.Name;
                    if (add.DistrictID != null)
                    {
                        Lbldistrict.Text = add.District.Name.ToString();
                    }
                    else { Lbldistrict.Text = "NA"; }
                    

                      LblCorPinCode.Text = add.PinCode.ToString();                               
                    
                   }

                    //CandidateQualificationDetail CandidateQualificationDetails = context.CandidateQualificationDetails.Find(RegApplID);
                    //LblLeducode.Text = CandidateQualificationDetails.EducationalQualification.Name;
                    Int32 a = Convert.ToInt32(RegID.CandidateID);
                   // EducationalQualification edu;
                   // var applicationName = context.CandidateQualificationDetails.Find(a);
                   // var application = context.CandidateQualificationDetails.Where(p => p.CandidateID == a && p.EducationalQualificationID == CurrentCourse.CourseCategoryID).FirstOrDefault();
                   //// Int32 a = Convert.ToInt32(RegID.CandidateID);

                    //var educationName = (from e in context.EducationalQualifications                              
                    //           join c in context.CandidateQualificationDetails on e.ID equals c.EducationalQualificationID 
                    //           join q in context.QualificationEligibility on  currentcourse equals q.CourseID 
                    //           where e.CandidateID == entityID
                    //           orderby q.CourseID
                    //           select new { e.Name});

                //
                    var CandidateQualificationDetails = (from cq in context.CandidateQualificationDetails
                                     where cq.CandidateID == a //37068
                                     select new { ValueField = cq.ID, TextField = cq.EducationalQualificationID }).FirstOrDefault();

                    if (CandidateQualificationDetails != null)
                    {

                        Int64 CandQuID = Convert.ToInt64(CandidateQualificationDetails.TextField);

                        //CandidateQualificationDetail CandidateQualificationDetails = context.CandidateQualificationDetails.Find(a);
                        var eq = context.QualificationEligibility.Find(CandQuID);
                        var eq1 = context.EducationalQualifications.Find(CandQuID);
                        LblLeducode.Text = Convert.ToString(eq1.Name);
                        //Educational_Qualification
                    }
                    



                // where n.ID == centreId && sf.courseid == courseId && sf.batchID == batchId
                    //CandidateQualificationDetail CandidateQualificationDetails = context.CandidateQualificationDetails.Find(a);
                    
                    //LblLeducode.Text = CandidateQualificationDetails.EducationalQualification.Name;


              

               

            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    #endregion

    private string FullCorAddress(Address crs)
    {
        string str = crs.AddressLine1;
        if (crs.AddressLine2 != null)
            if (crs.AddressLine2.Trim().Length > 1)
                str += ", " + crs.AddressLine2.Trim().Trim(',');
        if (crs.AddressLine3 != null)
            if (crs.AddressLine3.Trim().Length > 1)
                str += ", " + crs.AddressLine3.Trim().Trim(',');
        if (crs.CityName != null)
            str += "<br>" + crs.CityName;
        if (crs.DistrictID.HasValue)
            str += "<br/> District:- " + crs.District.Name + ", ";
        if (crs.State.Name != null)
            str += "<br/> State:- " + crs.State.Name;
        if (crs.PinCode.HasValue)
            str += ",&nbsp; Pin:- " + crs.PinCode.Value.ToString();
        return str;
    }

    #region[Check_Candidate_Request]
    protected void UpdateSelection()
    {
       // btnupdate.Visible = true;
       // btnSave.Visible = false;
        if (ChkHandicapped.Checked == true)
        {
            Rdhandicapped.Enabled = true;
        }
        else { Rdhandicapped.Enabled = false; }

        if (ChkMaritalStatus.Checked == true)
        {           
            ddlMStatus.Enabled = true;
        }
        else { ddlMStatus.Enabled = false; }

        if (ChkCategory.Checked == true)
        {
            ddlCategory.Enabled = true;
        }
        else { ddlCategory.Enabled = false; }


        if (chkMob.Checked == true)
        {
            txtCorMobileCode.Visible = true;
            txtCorMobileNo.Enabled = true;
        }
        else { txtCorMobileNo.Enabled = false; }
        if (ChkEmail.Checked == true)
        { txtEmailId.Enabled = true; }
        else { txtEmailId.Enabled = false; }

        if (ChkCorAddress.Checked == true)
        {
            TxtCorAddressLine1.Enabled = true;
            TxtCorAddressLine2.Enabled = true;
            TxtCorAddressLine3.Enabled = true;
            TxtCorCity.Enabled = true;
            ddlCorState.Enabled = true;
            ddldistrict.Enabled = true;
            txtCorPinCode.Enabled = true;
        }
        else {
            TxtCorAddressLine1.Enabled = false;
            TxtCorAddressLine2.Enabled = false;
            TxtCorAddressLine3.Enabled = false;
            TxtCorCity.Enabled = false;
            ddlCorState.Enabled = false;
            ddldistrict.Enabled = false;
            txtCorPinCode.Enabled = false;
        }

        //if (ChkAdd1.Checked == true)
        //{ TxtCorAddressLine1.Enabled = true; }
        //else { TxtCorAddressLine1.Enabled = false; }
        //if (ChkAdd2.Checked == true)
        //{ TxtCorAddressLine2.Enabled = true; }
        //else { TxtCorAddressLine2.Enabled = false; }
        //if (ChkAdd3.Checked == true)
        //{ TxtCorAddressLine3.Enabled = true; }
        //else { TxtCorAddressLine3.Enabled = false; }
        //if (ChkCity.Checked == true)
        //{ TxtCorCity.Enabled = true; }
        //else { TxtCorCity.Enabled = false; }
        //if (ChkState.Checked == true)
        //{ ddlCorState.Enabled = true; }
        //else { ddlCorState.Enabled = false; }
        //if (ChkDistrict.Checked == true) 
        //{ ddldistrict.Enabled = true; }
        //else { ddldistrict.Enabled = false; }
        //if (ChkPin.Checked == true)
        //{ txtCorPinCode.Enabled = true; }
        //else { txtCorPinCode.Enabled = false; }

        if (ChkEducation.Checked == true)
        { DDLeducode.Enabled = true; }
        else { DDLeducode.Enabled = false; }


    }
    #endregion

    #region[Update_Candidate_Request_old]
    //protected void UpdateAll()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Candidate objUpdateCandi = context.Candidates.Find(entityID);
    //            if (ChkHandicapped.Checked == true) //update Disabilty
    //            {
    //                #region old
    //                //if (Rdhandicapped.SelectedValue != null)
    //                //{

    //                //    if (Rdhandicapped.SelectedItem.Value == "1")
    //                //    //if (Rdhandicapped.SelectedValue =="Y")
    //                //    {

    //                //        //objUpdateCandi.IsHandicaped = Convert.ToBoolean(Rdhandicapped.SelectedItem.Value.ToString());
    //                //        //objUpdateCandi.IsHandicaped = Convert.ToBoolean(Rdhandicapped.SelectedItem.Text);
    //                //        objUpdateCandi.IsHandicaped = Convert.ToBoolean(Rdhandicapped.SelectedItem.Value.ToString());
    //                //    }

    //                //    else
    //                //    { objUpdateCandi.IsHandicaped = Convert.ToBoolean(Rdhandicapped.SelectedItem.Value.ToString()); }
    //                //    //context.Candidates.Add(objUpdateCandi);
    //                //    //context.SaveChanges();
    //                //}

    //                //Rdhandicapped.SelectedValue = "";
    //                //throw new Exception("Handicapped (Disability)");
    //                #endregion
    //                if (!String.IsNullOrWhiteSpace(Rdhandicapped.SelectedItem.Value))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var candiDeatails = db.Candidates.SingleOrDefault(c => c.ID == entityID);

    //                        if (candiDeatails != null)
    //                        {
    //                            candiDeatails.IsHandicaped = Convert.ToBoolean(Rdhandicapped.SelectedItem.Value);
    //                            db.SaveChanges();
                                
    //                        }
    //                    }

    //                }
    //                else
    //                { // throw new Exception("Mobile No. can not be left blank");
    //                    strMessage = "Mobile No. can not be left blank";
    //                }

    //            }
    //            if (ChkMaritalStatus.Checked == true)//Marital Status
    //            {
    //                if (ddlMStatus.SelectedValue != "0")
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var candiDeatails = db.Candidates.SingleOrDefault(c => c.ID == entityID);

    //                        if (candiDeatails != null)
    //                        {
    //                            // candiDeatails.MaritalStatus = Convert.ToInt32(ddlMStatus.SelectedValue.ToString());
    //                            db.SaveChanges();
    //                            ChkMaritalStatus.Enabled = false;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strMessage = "Select Marital Status can not be left blank";
    //                }
    //            }
    //            if (ChkCategory.Checked == true) //Update Category
    //            {
    //                if (ddlCategory.SelectedValue != "0")
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var candiDeatails = db.Candidates.SingleOrDefault(c => c.ID == entityID);

    //                        if (candiDeatails != null)
    //                        {

    //                            candiDeatails.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue.ToString());
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strMessage = "Select Marital Status can not be left blank";
    //                }
    //            }
    //            if (chkMob.Checked == true) //Update Mobile No.
    //            {
    //                if (!String.IsNullOrWhiteSpace(txtCorMobileNo.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var ContactDetail = db.CandidateContactDetails.SingleOrDefault(c => c.CandidateID == entityID);

    //                        if (ContactDetail != null)
    //                        {

    //                            ContactDetail.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strMessage = "Mobile no. can not be left blank";
    //                }
    //            }
    //            if (ChkEmail.Checked == true) //Update Email ID
    //            {
    //                if (!String.IsNullOrWhiteSpace(txtEmailId.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var ContactDetail = db.CandidateContactDetails.SingleOrDefault(c => c.CandidateID == entityID);

    //                        if (ContactDetail != null)
    //                        {

    //                            ContactDetail.EmailAddress = txtEmailId.Text;
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                   // strMessage = "Email ID can not be left blank";
    //                    ShowAlert("You check Email ID  for Update, Email ID can not be left blank.");
    //                    DisplayAll();
    //                    return;
    //                }
    //            }
    //            if (ChkAdd1.Checked == true) //Update Add Line 1
    //            {
    //                if (!String.IsNullOrWhiteSpace(TxtCorAddressLine1.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.AddressLine1 = TxtCorAddressLine1.Text;
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strMessage = "If you check Address Line 1 ?Add Line 1 can not be left blank";
    //                }

    //            }
    //            if (ChkAdd2.Checked == true) //Update Add Line 2
    //            {
    //                if (!String.IsNullOrWhiteSpace(TxtCorAddressLine2.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.AddressLine2 = TxtCorAddressLine2.Text;
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strMessage = "If you check Address Line 2 ?Add Line 2 can not be left blank";
    //                }

    //            }
    //            if (ChkAdd3.Checked == true) //Update Add Line 3
    //            {
    //                if (!String.IsNullOrWhiteSpace(TxtCorAddressLine3.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.AddressLine3 = TxtCorAddressLine3.Text;
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    strMessage = "If you check Address Line 3 ?Add Line 3 can not be left blank";
    //                }

    //            }
    //            if (ChkCity.Checked == true) //Update City Name
    //            {
    //                if (!String.IsNullOrWhiteSpace(TxtCorCity.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.CityName = TxtCorCity.Text;
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //strMessage = "If you check City Name ?City Name can not be left blank";
    //                    ShowAlert("If you check City Name ?City Name can not be left blank.");
    //                    DisplayAll();
    //                    return;
    //                }

    //            }
    //            if (ChkState.Checked == true) //Update State 
    //            {
    //                if (ddlCorState.SelectedValue != "0")
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.StateID = Convert.ToInt32(ddlCorState.SelectedValue.ToString());
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //strMessage = "If you check State? State Name can not be left blank";
    //                    ShowAlert("If you check State? State Name can not be left blank.");
    //                    DisplayAll();
    //                    return;

    //                }

    //            }
    //            if (ChkDistrict.Checked == true) //Update District
    //            {
    //                if (ddldistrict.SelectedValue != "0")
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.DistrictID = Convert.ToInt32(ddldistrict.SelectedValue.ToString());
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //strMessage = "If you check District? District Name can not be left blank";
    //                    ShowAlert("If you check District? District Name can not be left blank.");
    //                    DisplayAll();
    //                    return;
    //                }

    //            }
    //            if (ChkPin.Checked == true) //Update PinCode
    //            {
    //                if (!String.IsNullOrWhiteSpace(txtCorPinCode.Text))
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var CorrespondenceAddress = db.Addresses.SingleOrDefault(c => c.CandidateID == entityID && c.AddressTypeID == 1);

    //                        if (CorrespondenceAddress != null)
    //                        {

    //                            CorrespondenceAddress.PinCode = Convert.ToInt32(txtCorPinCode.Text.ToString());
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //strMessage = "If you check PinCode? PinCode can not be left blank";
    //                    ShowAlert("If you check PinCode? PinCode can not be left blank.");
    //                    DisplayAll();
    //                    return;
    //                }

    //            }
    //            if (ChkEducation.Checked == true)//Update Highest Candidate Qualification
    //            {
    //                if (DDLeducode.SelectedValue != "0")
    //                {
    //                    using (var db = new EConnectContext())
    //                    {
    //                        var HighestCandidateQualification = db.CandidateQualificationDetails.SingleOrDefault(c => c.CandidateID == entityID);

    //                        if (HighestCandidateQualification != null)
    //                        {

    //                            HighestCandidateQualification.EducationalQualificationID = Convert.ToInt32(DDLeducode.SelectedValue.ToString());
    //                            db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    // strMessage = "If you check Highest Qualification? Highest Qualification can not be left blank";
    //                    ShowAlert("If you check Highest Qualification? Highest Qualification can not be left blank.");
    //                    DisplayAll();
    //                    return;
    //                }
    //            }

    //            //context.Candidates.Add(objUpdateCandi);
    //            //context.SaveChanges();

    //            // CandidateContactDetail objUpdateCandi1 = context.CandidateContactDetails.Find(entityID);
    //            //Candidate objUpdateCandi = context.Candidates.Find(entityID);
    //            //if (chkMob.Checked == true)
    //            //{ objUpdateCandi1.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text.ToString()); }
    //            //if( (ChkEmail.Checked == true)|| (chkMob.Checked == true))
    //            //{
    //            //    using (var db = new EConnectContext())
    //            //    {
    //            //        var ContactDetail = db.CandidateContactDetails.SingleOrDefault(c => c.CandidateID == entityID);

    //            //        if (ContactDetail != null)
    //            //        {
    //            //            if (String.IsNullOrWhiteSpace(txtCorMobileNo.Text))
    //            //            {

    //            //               // throw new Exception("Mobile No. can not be left blank");
    //            //                lblerror.Text = "Mobile No. can not be left blank";
    //            //            }
    //            //            else { ContactDetail.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text); }
    //            //            if (String.IsNullOrWhiteSpace(txtEmailId.Text))
    //            //            {

    //            //                throw new Exception("Email can not be left blank");
    //            //            }
    //            //            else
    //            //            { ContactDetail.EmailAddress = txtEmailId.Text; }

    //            //            db.SaveChanges();
    //            //        }
    //            //    }

    //            //    //context.CandidateContactDetails.Add(objUpdateCandi1);
    //            //    //context.SaveChanges();


    //            //}
    //        }
    //        strMessage = "Data Updated Successfully";

    //        DisplayAll();
    //        DisableAll();
    //    }
    //    catch (Exception ex) { ShowAlert(ex.Message); }
    //}
    #endregion

    #region[Insert_Candidate_Request]
    protected void InsertRequestData()
    {
        try
        {
            Boolean success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    candUpdateRequest ObjcandidateRequest;
                    ObjcandidateRequest = new candUpdateRequest();

                    #region[Req for Disability Change]
                    if (ChkHandicapped.Checked == true)
                    {
                        //if (!Rdhandicapped.Checked == true)
                        //{
                        //    ShowAlert("Select Marital Status");

                        //    return;
                        //}

                        if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                        {
                            ObjcandidateRequest.requestNo = requestNo + 1;
                            ObjcandidateRequest.candID = entityID;
                            ObjcandidateRequest.courseID = currentCourseID;
                            ObjcandidateRequest.regnNo = Lbregno.Text;
                            ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);//1;
                            ObjcandidateRequest.changedValue = Rdhandicapped.SelectedItem.Text;
                            ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                            ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                            ObjcandidateRequest.enterdate = DateTime.Now;
                            ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;
                            context.candidateUpdateRequest.Add(ObjcandidateRequest);
                            ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                            context.SaveChanges();
                            ChkHandicapped.Checked = false;
                            ChkHandicapped.Enabled = false;
                            Rdhandicapped.Enabled = false;
                        }
                        else
                        {

                            Int32 Upd_Field_Handi = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);
                            Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                            Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                            Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);
                            //if (context.candidateUpdateRequest.Any(o => o.candID == entityID & o.updateFieldID == Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped) &
                            //    o.requestStatusID == Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived)))
                            if (context.candidateUpdateRequest.Any(o => o.candID == entityID & o.updateFieldID == Upd_Field_Handi &
                               o.requestStatusID == App_Status_RequestReceived))
                            {
                                var Request_Nu = (from a in context.candidateUpdateRequest
                                                  where a.candID == entityID && a.courseID == currentCourseID &&
                                                  a.updateFieldID == Upd_Field_Handi
                                                  && a.requestStatusID == App_Status_RequestReceived
                                                  orderby a.requestNo descending
                                                  select new
                                                  {
                                                      RqNo = a.requestNo

                                                  }).FirstOrDefault();
                               
                                //ShowAlert("You have already requested for the field.Please Upload required Documents");
                                ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                return;
                            }
                            else
                            {
                                if (context.candidateUpdateRequest.All(u => u.candID == entityID & u.requestStatusID != App_Status_RequestReceived || u.requestStatusID != App_Status_DocumentsUploaded)) //3.check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                {
                                    Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        .Max(u => u == null ? 0 : u.requestNo);

                                    ObjcandidateRequest.requestNo = requestNo_old;
                                    ObjcandidateRequest.candID = entityID;
                                    ObjcandidateRequest.courseID = currentCourseID;
                                    ObjcandidateRequest.regnNo = Lbregno.Text;
                                    ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);//1
                                    ObjcandidateRequest.changedValue = Rdhandicapped.SelectedItem.Text;
                                    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                    ObjcandidateRequest.enterdate = DateTime.Now;
                                    ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;
                                    ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                    context.SaveChanges();
                                    ChkHandicapped.Checked = false;
                                    ChkHandicapped.Enabled = false;
                                    Rdhandicapped.Enabled = false;
                                }

                                else if (
                                    
                                    (context.candidateUpdateRequest.Any
                                            (u => u.candID == entityID && 
                                                     u.requestStatusID!= App_Status_RequestReceived && u.requestStatusID != App_Status_DocumentsUploaded
                                            )
                                     )
                                    
                                    || 
                                    
                                    (!context.candidateUpdateRequest.Any(u => u.candID == entityID)))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As new Req.)
                                {
                                    //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                    //       .Max(u => u == null ? 0 : u.requestNo);
                                    Int64 requestNo_New = context.candidateUpdateRequest
                                          .Max(u => u == null ? 0 : u.requestNo);

                                    ObjcandidateRequest.requestNo = requestNo_New + 1;
                                    ObjcandidateRequest.candID = entityID;
                                    ObjcandidateRequest.courseID = currentCourseID;
                                    ObjcandidateRequest.regnNo = Lbregno.Text;
                                    ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);// 1;
                                    ObjcandidateRequest.changedValue = Rdhandicapped.SelectedItem.Text;
                                    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                    ObjcandidateRequest.enterdate = DateTime.Now;
                                    ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;
                                    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                    ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                    context.SaveChanges();
                                    ChkHandicapped.Checked = false;
                                    ChkHandicapped.Enabled = false;
                                    Rdhandicapped.Enabled = false;

                                }
                                //else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with any request then request id start with 1 (New Request)
                                //{
                                //    ObjcandidateRequest.requestNo = requestNo + 1;

                                //    ObjcandidateRequest.candID = entityID;
                                //    ObjcandidateRequest.courseID = currentCourseID;
                                //    ObjcandidateRequest.regnNo = Lbregno.Text;
                                //    ObjcandidateRequest.updateFieldID = 1;
                                //    ObjcandidateRequest.changedValue = Rdhandicapped.SelectedItem.Text;
                                //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                //    ObjcandidateRequest.enterdate = DateTime.Now;
                                //    ObjcandidateRequest.requestStatusID = 1;//request Received
                                //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                //    context.SaveChanges();
                                //    ChkHandicapped.Checked = false;
                                //    ChkHandicapped.Enabled = false;
                                //    Rdhandicapped.Enabled = false;
                                //}

                            }
                        }
                    }
                    #endregion

                    #region[Req for Marital change]
                    if (ChkMaritalStatus.Checked == true)
                    {
                        if (ddlMStatus.SelectedValue == "0")
                        {
                            ShowAlert("Select Marital Status");

                            return;
                        }
                        else
                        {
                            // if (ObjcandidateRequest.requestNo == 0)//check first time tble is null genrate request no.
                            if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                            {
                                ObjcandidateRequest.requestNo = requestNo + 1;
                                ObjcandidateRequest.candID = entityID;
                                ObjcandidateRequest.courseID = currentCourseID;
                                ObjcandidateRequest.regnNo = Lbregno.Text;
                                ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);//2;
                                ObjcandidateRequest.changedValue = ddlMStatus.SelectedItem.Text;
                                ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                ObjcandidateRequest.enterdate = DateTime.Now;
                                ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;
                                context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                context.SaveChanges();
                                ChkMaritalStatus.Checked = false;
                                ChkMaritalStatus.Enabled = false;
                                ddlMStatus.Enabled = false;
                            }

                            else
                            {
                                Int32 Upd_Field_Marital = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);
                                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                                if (context.candidateUpdateRequest.Any(o => o.candID == entityID & o.updateFieldID == Upd_Field_Marital &
                                    o.requestStatusID == App_Status_RequestReceived))
                                {
                                    var Request_Nu = (from a in context.candidateUpdateRequest
                                                      where a.candID == entityID && a.courseID == currentCourseID &&
                                                      a.updateFieldID == Upd_Field_Marital &&
                                                      a.requestStatusID == App_Status_RequestReceived
                                                      orderby a.requestNo descending
                                                      select new
                                                      {
                                                          RqNo = a.requestNo

                                                      }).FirstOrDefault();

                                    //var req = context.candidateUpdateRequest.Where(o => o.candID == entityID & o.updateFieldID == 2 & o.requestStatusID == 1);
                                    ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                    return;

                                }
                                else
                                {
                                    if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_RequestReceived || u.requestStatusID == App_Status_DocumentsUploaded)) //check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                    {
                                        Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_old;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);//2;
                                        ObjcandidateRequest.changedValue = ddlMStatus.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkMaritalStatus.Checked = false;
                                        ChkMaritalStatus.Enabled = false;
                                        ddlMStatus.Enabled = false;
                                    }
                                    else if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_submittedbutfeesnotpaid))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As New Req.)
                                    {
                                        //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        //       .Max(u => u == null ? 0 : u.requestNo);
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);//2;
                                        ObjcandidateRequest.changedValue = ddlMStatus.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkMaritalStatus.Checked = false;
                                        ChkMaritalStatus.Enabled = false;
                                        ddlMStatus.Enabled = false;
                                    }
                                    else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with any request then request id start with 1 (New Request)
                                    {

                                        // ObjcandidateRequest.requestNo = requestNo + 1;  
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;
                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);//2;
                                        ObjcandidateRequest.changedValue = ddlMStatus.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1.request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkMaritalStatus.Checked = false;
                                        ChkMaritalStatus.Enabled = false;
                                        ddlMStatus.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region[Req for category change]
                    if (ChkCategory.Checked == true)
                    {
                        if (ddlCategory.SelectedValue == "0")
                        {
                            ShowAlert("Select Category");

                            return;
                        }
                        else
                        {
                            if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                            {
                                ObjcandidateRequest.requestNo = requestNo + 1;
                                ObjcandidateRequest.candID = entityID;
                                ObjcandidateRequest.courseID = currentCourseID;
                                ObjcandidateRequest.regnNo = Lbregno.Text;
                                ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);//3;
                                ObjcandidateRequest.changedValue = ddlCategory.SelectedItem.Text;
                                ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                ObjcandidateRequest.enterdate = DateTime.Now;
                                ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                context.SaveChanges();
                                ChkCategory.Checked = false;
                                ChkCategory.Enabled = false;
                                ddlCategory.Enabled = false;
                            }


                            else
                            {
                                Int32 Upd_Field_CasteCategory = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);
                                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                                if (context.candidateUpdateRequest.Any(o => o.candID == entityID & o.updateFieldID == Upd_Field_CasteCategory &
                                    o.requestStatusID == App_Status_RequestReceived))
                                {
                                    var Request_Nu = (from a in context.candidateUpdateRequest
                                                      where a.candID == entityID && a.courseID == currentCourseID &&
                                                      a.updateFieldID == Upd_Field_CasteCategory &&
                                                      a.requestStatusID == App_Status_RequestReceived
                                                      orderby a.requestNo descending
                                                      select new
                                                      {
                                                          RqNo = a.requestNo

                                                      }).FirstOrDefault();

                                  
                                    ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                    return;
                                    //ShowAlert("You have already requested for the field.Please Upload required Documents");
                                    //return;

                                }
                                else
                                {
                                    if (context.candidateUpdateRequest.Any(u => u.candID == entityID &
                                        u.requestStatusID == App_Status_RequestReceived || u.requestStatusID == App_Status_DocumentsUploaded)) //check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                    {
                                        Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_old;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);//3;
                                        ObjcandidateRequest.changedValue = ddlCategory.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkCategory.Checked = false;
                                        ChkCategory.Enabled = false;
                                        ddlCategory.Enabled = false;

                                    }
                                    else if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_submittedbutfeesnotpaid))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As New Req.)
                                    {
                                        //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        //       .Max(u => u == null ? 0 : u.requestNo);
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                        .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);//3;
                                        ObjcandidateRequest.changedValue = ddlCategory.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkCategory.Checked = false;
                                        ChkCategory.Enabled = false;
                                        ddlCategory.Enabled = false;
                                    }
                                    else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with any request then request id start with 1 (New Request)
                                    {

                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory); //3;
                                        ObjcandidateRequest.changedValue = ddlCategory.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkCategory.Checked = false;
                                        ChkCategory.Enabled = false;
                                        ddlCategory.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region[Request Mob change]
                    if (chkMob.Checked == true)
                    {

                        //if (txtCorMobileNo.Text == "")
                        //{
                        //    ShowAlert("Enater Mobile No");

                        //    return;
                        //}
                        if (String.IsNullOrWhiteSpace(txtCorMobileNo.Text))
                        {

                            throw new Exception("Mobile Number can not be left blank");
                        }
                        else if (txtCorMobileNo.Text.Contains('.'))
                        {

                            throw new Exception("Dot(.) not allowed in Mobile Number");
                        }
                        else if (!IsNumeric(txtCorMobileNo.Text))
                        {

                            throw new Exception("Invalid Mobile Number");
                        }
                        
                        
                        else
                        {
                            if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                            {
                                ObjcandidateRequest.requestNo = requestNo + 1;
                                ObjcandidateRequest.candID = entityID;
                                ObjcandidateRequest.courseID = currentCourseID;
                                ObjcandidateRequest.regnNo = Lbregno.Text;
                                ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);//4;
                                ObjcandidateRequest.changedValue = txtCorMobileNo.Text;
                                ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                ObjcandidateRequest.enterdate = DateTime.Now;
                                ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                context.SaveChanges();

                                chkMob.Checked = false;
                                chkMob.Enabled = false;
                                txtCorMobileNo.Enabled = false;
                            }

                            else
                            {
                                Int32 Upd_Field_Mobile = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);
                                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                                if (context.candidateUpdateRequest.Any(o => o.candID == entityID &
                                    o.updateFieldID == Upd_Field_Mobile & o.requestStatusID == App_Status_RequestReceived))
                                {
                                    var Request_Nu = (from a in context.candidateUpdateRequest
                                                      where a.candID == entityID && a.courseID == currentCourseID &&
                                                      a.updateFieldID == Upd_Field_Mobile &&
                                                      a.requestStatusID == App_Status_RequestReceived
                                                      orderby a.requestNo descending
                                                      select new
                                                      {
                                                          RqNo = a.requestNo

                                                      }).FirstOrDefault();


                                    ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                    return;
                                    //ShowAlert("You have already requested for the field.Please Upload required Documents");
                                    //return;
                                   
                                }
                                else
                                {
                                    if (context.candidateUpdateRequest.Any(u => u.candID == entityID 
                                        & u.requestStatusID == App_Status_RequestReceived || u.requestStatusID == App_Status_DocumentsUploaded)) //check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                    {
                                        Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_old;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);//4;
                                        ObjcandidateRequest.changedValue = txtCorMobileNo.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        context.SaveChanges();

                                        chkMob.Checked = false;
                                        chkMob.Enabled = false;
                                        txtCorMobileNo.Enabled = false;
                                    }
                                    else if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_submittedbutfeesnotpaid))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As New Req.)
                                    {
                                        //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        //       .Max(u => u == null ? 0 : u.requestNo);

                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);//4;
                                        ObjcandidateRequest.changedValue = txtCorMobileNo.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        context.SaveChanges();

                                        chkMob.Checked = false;
                                        chkMob.Enabled = false;
                                        txtCorMobileNo.Enabled = false;
                                    }
                                    else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with no any request then request No. start with 1 (New Request)
                                    {
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);//4;
                                        ObjcandidateRequest.changedValue = txtCorMobileNo.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);//1;//request Received
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        context.SaveChanges();

                                        chkMob.Checked = false;
                                        chkMob.Enabled = false;
                                        txtCorMobileNo.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region[Request Email change ]

                    if (ChkEmail.Checked == true)
                    {

                        //if (txtEmailId.Text == "")
                        //{
                        //    ShowAlert("Enter Email");

                        //    return;
                        //}
                        if (String.IsNullOrWhiteSpace(txtEmailId.Text))
                        {

                            throw new Exception("Email Address can not be left blank");
                        }
                        else if (!IsValidEmailAddress(txtEmailId.Text.Trim()))
                        {

                            throw new Exception("Invalid Email Address");
                        }
                        else
                        {
                            if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                            {
                                ObjcandidateRequest.requestNo = requestNo + 1;
                                ObjcandidateRequest.candID = entityID;
                                ObjcandidateRequest.courseID = currentCourseID;
                                ObjcandidateRequest.regnNo = Lbregno.Text;
                                ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);//5;
                                ObjcandidateRequest.changedValue = txtEmailId.Text;
                                ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                ObjcandidateRequest.enterdate = DateTime.Now;
                                ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                context.SaveChanges();

                                ChkEmail.Checked = false;
                                ChkEmail.Enabled = false;
                                txtEmailId.Enabled = false;
                            }


                            else
                            {
                                Int32 Upd_Field_Email = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);
                                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                                if (context.candidateUpdateRequest.Any(o => o.candID == entityID & o.updateFieldID == Upd_Field_Email &
                                    o.requestStatusID == App_Status_RequestReceived)) //cheking for dublicate request
                                {
                                    var Request_Nu = (from a in context.candidateUpdateRequest
                                                      where a.candID == entityID && a.courseID == currentCourseID &&
                                                      a.updateFieldID == Upd_Field_Email &&
                                                      a.requestStatusID == App_Status_RequestReceived
                                                      orderby a.requestNo descending
                                                      select new
                                                      {
                                                          RqNo = a.requestNo

                                                      }).FirstOrDefault();


                                    ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                    return;
                                    //ShowAlert("You have already requested for the field.Please Upload required Documents");
                                    //return;

                                }
                                else
                                {
                                    if (context.candidateUpdateRequest.Any(u => u.candID == entityID & 
                                        u.requestStatusID == App_Status_RequestReceived || u.requestStatusID == App_Status_DocumentsUploaded)) //check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                    {
                                        Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_old;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);//5;
                                        ObjcandidateRequest.changedValue = txtEmailId.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();

                                        ChkEmail.Checked = false;
                                        ChkEmail.Enabled = false;
                                        txtEmailId.Enabled = false;
                                    }
                                    else if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_submittedbutfeesnotpaid))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As New Req.)
                                    {
                                        //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        //       .Max(u => u == null ? 0 : u.requestNo);

                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);//5;
                                        ObjcandidateRequest.changedValue = txtEmailId.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();

                                        ChkEmail.Checked = false;
                                        ChkEmail.Enabled = false;
                                        txtEmailId.Enabled = false;
                                    }

                                    else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with no any request then request No. start with 1 (New Request)
                                    {
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;


                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);//5;
                                        ObjcandidateRequest.changedValue = txtEmailId.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();

                                        ChkEmail.Checked = false;
                                        ChkEmail.Enabled = false;
                                        txtEmailId.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region[Request for change Correspondence Address ]
                    if (ChkCorAddress.Checked == true)
                    {

                        if (TxtCorAddressLine1.Text == "" || TxtCorAddressLine2.Text == "" || TxtCorAddressLine3.Text == "" || TxtCorCity.Text == "" || ddlCorState.SelectedValue == "0" || ddldistrict.SelectedValue == "0" || txtCorPinCode.Text == "")
                        {
                            ShowAlert("Correspondence Address cant not be blank");

                            return;
                        }
                        //else if (!CheckNumberPresent("TxtCorCity", "Numeric characters are not allowed"))
                        //    return false;
                        //else if (!isSpecialCharacter("TxtCorCity", "Special characters are not allowed"))
                        //    return false;


                        else
                        {
                            if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                            {
                                ObjcandidateRequest.requestNo = requestNo + 1;
                                ObjcandidateRequest.candID = entityID;
                                ObjcandidateRequest.courseID = currentCourseID;
                                ObjcandidateRequest.regnNo = Lbregno.Text;
                                ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);//6;
                                ObjcandidateRequest.changedValue = TxtCorAddressLine1.Text + ";" + TxtCorAddressLine2.Text + ";" + TxtCorAddressLine3.Text + ";" + TxtCorCity.Text + ";" + ddlCorState.SelectedItem.Text + ";" + ddldistrict.SelectedItem.Text + ";" + txtCorPinCode.Text;
                                ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                ObjcandidateRequest.enterdate = DateTime.Now;
                                ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                context.SaveChanges();

                                ChkCorAddress.Checked = false;
                                ChkCorAddress.Enabled = false;

                                TxtCorAddressLine1.Enabled = false;
                                TxtCorAddressLine2.Enabled = false;
                                TxtCorAddressLine3.Enabled = false;
                                TxtCorCity.Enabled = false;
                                ddlCorState.Enabled = false;
                                ddldistrict.Enabled = false;
                                txtCorPinCode.Enabled = false;
                            }


                            else
                            {
                                Int32 Upd_Field_Address = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);
                                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                                if (context.candidateUpdateRequest.Any(o => o.candID == entityID & o.updateFieldID == Upd_Field_Address &
                                    o.requestStatusID == App_Status_RequestReceived))
                                {
                                    var Request_Nu = (from a in context.candidateUpdateRequest
                                                      where a.candID == entityID && a.courseID == currentCourseID &&
                                                      a.updateFieldID == Upd_Field_Address &&
                                                      a.requestStatusID == App_Status_RequestReceived
                                                      orderby a.requestNo descending
                                                      select new
                                                      {
                                                          RqNo = a.requestNo

                                                      }).FirstOrDefault();


                                    ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                    return;

                                }
                                else
                                {
                                    if (context.candidateUpdateRequest.Any(u => u.candID == entityID &
                                        u.requestStatusID == App_Status_RequestReceived || u.requestStatusID == App_Status_DocumentsUploaded)) //check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                    {
                                        Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_old;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);// 6;
                                        ObjcandidateRequest.changedValue = TxtCorAddressLine1.Text + ";" + TxtCorAddressLine2.Text + ";" + TxtCorAddressLine3.Text + ";" + TxtCorCity.Text + ";" + ddlCorState.SelectedItem.Text + ";" + ddldistrict.SelectedItem.Text + ";" + txtCorPinCode.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();

                                        ChkCorAddress.Checked = false;
                                        ChkCorAddress.Enabled = false;

                                        TxtCorAddressLine1.Enabled = false;
                                        TxtCorAddressLine2.Enabled = false;
                                        TxtCorAddressLine3.Enabled = false;
                                        TxtCorCity.Enabled = false;
                                        ddlCorState.Enabled = false;
                                        ddldistrict.Enabled = false;
                                        txtCorPinCode.Enabled = false;
                                    }
                                    else if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_submittedbutfeesnotpaid))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As New Req.)
                                    {
                                        //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        //       .Max(u => u == null ? 0 : u.requestNo);

                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);// 6;
                                        ObjcandidateRequest.changedValue = TxtCorAddressLine1.Text + ";" + TxtCorAddressLine2.Text + ";" + TxtCorAddressLine3.Text + ";" + TxtCorCity.Text + ";" + ddlCorState.SelectedItem.Text + ";" + ddldistrict.SelectedItem.Text + ";" + txtCorPinCode.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();

                                        ChkCorAddress.Checked = false;
                                        ChkCorAddress.Enabled = false;

                                        TxtCorAddressLine1.Enabled = false;
                                        TxtCorAddressLine2.Enabled = false;
                                        TxtCorAddressLine3.Enabled = false;
                                        TxtCorCity.Enabled = false;
                                        ddlCorState.Enabled = false;
                                        ddldistrict.Enabled = false;
                                        txtCorPinCode.Enabled = false;
                                    }
                                    else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with no any request then request No. start with 1 (New Request)
                                    {
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);// 6;
                                        ObjcandidateRequest.changedValue = TxtCorAddressLine1.Text + ";" + TxtCorAddressLine2.Text + ";" + TxtCorAddressLine3.Text + ";" + TxtCorCity.Text + ";" + ddlCorState.SelectedItem.Text + ";" + ddldistrict.SelectedItem.Text + ";" + txtCorPinCode.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();

                                        ChkCorAddress.Checked = false;
                                        ChkCorAddress.Enabled = false;

                                        TxtCorAddressLine1.Enabled = false;
                                        TxtCorAddressLine2.Enabled = false;
                                        TxtCorAddressLine3.Enabled = false;
                                        TxtCorCity.Enabled = false;
                                        ddlCorState.Enabled = false;
                                        ddldistrict.Enabled = false;
                                        txtCorPinCode.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region[old Address chage Indivisually]
                    //if (ChkAdd1.Checked == true) //Request Add 1 change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {

                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 6;
                    //    ObjcandidateRequest.changedValue = TxtCorAddressLine1.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkAdd1.Enabled = false;
                    //    TxtCorAddressLine1.Enabled = false;
                    //}
                    //if (ChkAdd1.Checked == true) //Request Add 2 change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {

                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 7;
                    //    ObjcandidateRequest.changedValue = TxtCorAddressLine2.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkAdd2.Enabled = false;
                    //    TxtCorAddressLine2.Enabled = false;
                    //}
                    //if (ChkAdd3.Checked == true) //Request Add 3 change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {
                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 8;
                    //    ObjcandidateRequest.changedValue = TxtCorAddressLine3.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkAdd3.Enabled = false;
                    //    TxtCorAddressLine3.Enabled = false;
                    //}
                    //if (ChkCity.Checked == true) //Request City Name change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {
                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 9;
                    //    ObjcandidateRequest.changedValue = TxtCorCity.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkCity.Enabled = false;
                    //    TxtCorCity.Enabled = false;
                    //}
                    //if (ChkState.Checked == true) //Request State change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {
                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 10;
                    //    ObjcandidateRequest.changedValue = ddlCorState.SelectedItem.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkState.Enabled = false;
                    //    ddlCorState.Enabled = false;
                    //}
                    //if (ChkDistrict.Checked == true) //Request District change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {
                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 11;
                    //    ObjcandidateRequest.changedValue = ddldistrict.SelectedItem.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkDistrict.Enabled = false;
                    //    ddldistrict.Enabled = false;
                    //}
                    //if (ChkPin.Checked == true) //Request Pin change 
                    //{
                    //    Int64 requestNo1 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //         .Max(u => u == null ? 0 : u.requestNo);

                    //    if (requestNo1 == 0)
                    //    {
                    //        ObjcandidateRequest.requestNo = requestNo1 + 1;
                    //    }

                    //    else
                    //    {
                    //        Int64 requestNo2 = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                    //            .Max(u => u == null ? 0 : u.requestNo);
                    //        ObjcandidateRequest.requestNo = requestNo2 + 1;
                    //    }

                    //    ObjcandidateRequest.candID = entityID;
                    //    ObjcandidateRequest.courseID = currentCourseID;
                    //    ObjcandidateRequest.regnNo = Lbregno.Text;
                    //    ObjcandidateRequest.updateFieldID = 12;
                    //    ObjcandidateRequest.changedValue = txtCorPinCode.Text;
                    //    ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                    //    ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                    //    ObjcandidateRequest.enterdate = DateTime.Now;
                    //    context.candidateUpdateRequest.Add(ObjcandidateRequest);
                    //    context.SaveChanges();
                    //    ChkPin.Enabled = false;
                    //    txtCorPinCode.Enabled = false;
                    //}
                    #endregion

                    #region[Request Highest Education change]
                    if (ChkEducation.Checked == true) //Request Highest Education change 
                    {

                        if (DDLeducode.SelectedValue == "0")
                        {
                            ShowAlert("Highest Qualification cant not be blank");

                            return;
                        }
                        else
                        {
                           
                            if (!context.candidateUpdateRequest.Any())//check first time tble is null genrate request no.
                            {
                                ObjcandidateRequest.requestNo = requestNo + 1;
                                ObjcandidateRequest.candID = entityID;
                                ObjcandidateRequest.courseID = currentCourseID;
                                ObjcandidateRequest.regnNo = Lbregno.Text;
                                ObjcandidateRequest.updateFieldID =  Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);//7; //12
                                ObjcandidateRequest.changedValue = DDLeducode.SelectedItem.Text;
                                ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                ObjcandidateRequest.enterdate = DateTime.Now;
                                ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                context.SaveChanges();
                                ChkEducation.Enabled = false;
                                DDLeducode.Enabled = false;
                            }

                            else
                            {
                                Int32 Upd_Field_HighEdu = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);
                                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                                if (context.candidateUpdateRequest.Any(o => o.candID == entityID &
                                    o.updateFieldID == Upd_Field_HighEdu & o.requestStatusID == App_Status_RequestReceived))
                                    
                                {
                                    var Request_Nu = (from a in context.candidateUpdateRequest
                                                      where a.candID == entityID && a.courseID == currentCourseID &&
                                                      a.updateFieldID == Upd_Field_HighEdu &&
                                                      a.requestStatusID == App_Status_RequestReceived
                                                      orderby a.requestNo descending
                                                      select new
                                                      {
                                                          RqNo = a.requestNo

                                                      }).FirstOrDefault();


                                    ShowAlert("You have already requested for the field against Request No. =" + Request_Nu.RqNo.ToString());
                                    return;

                                }
                                else
                                {
                                    if (context.candidateUpdateRequest.Any(u => u.candID == entityID &
                                           u.requestStatusID == App_Status_RequestReceived || u.requestStatusID == App_Status_DocumentsUploaded)) //check if exists req no. with no fee paid status for this candidate, then req no. should be same as old req No. 
                                    {
                                        Int64 requestNo_old = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                            .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_old;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);// 7; //12
                                        ObjcandidateRequest.changedValue = DDLeducode.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkEducation.Enabled = false;
                                        DDLeducode.Enabled = false;
                                    }
                                    else if (context.candidateUpdateRequest.Any(u => u.candID == entityID & u.requestStatusID == App_Status_submittedbutfeesnotpaid))//check exists req no.& fee paid for this candidate,if exists then req no.(old req no.+1)(As New Req.)
                                    {
                                        //Int64 requestNo_New = context.candidateUpdateRequest.Where(u => u.candID == entityID)
                                        //       .Max(u => u == null ? 0 : u.requestNo);

                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);// 7; //12
                                        ObjcandidateRequest.changedValue = DDLeducode.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkEducation.Enabled = false;
                                        DDLeducode.Enabled = false;
                                    }
                                    else if (!context.candidateUpdateRequest.Any(u => u.candID == entityID))//check if candidate does'nt exist with no any request then request No. start with 1 (New Request)
                                    {
                                        Int64 requestNo_New = context.candidateUpdateRequest
                                         .Max(u => u == null ? 0 : u.requestNo);

                                        ObjcandidateRequest.requestNo = requestNo_New + 1;

                                        ObjcandidateRequest.candID = entityID;
                                        ObjcandidateRequest.courseID = currentCourseID;
                                        ObjcandidateRequest.regnNo = Lbregno.Text;
                                        ObjcandidateRequest.updateFieldID = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);// 7; //12
                                        ObjcandidateRequest.changedValue = DDLeducode.SelectedItem.Text;
                                        ObjcandidateRequest.enterBy = Convert.ToInt32(entityID);
                                        ObjcandidateRequest.feesVerified = Convert.ToBoolean(0);
                                        ObjcandidateRequest.enterdate = DateTime.Now;
                                        ObjcandidateRequest.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);// 1;//request Received
                                        context.candidateUpdateRequest.Add(ObjcandidateRequest);
                                        ObjcandidateRequest.requestFinalised = true; //add for Flaging 21-02-23
                                        context.SaveChanges();
                                        ChkEducation.Enabled = false;
                                        DDLeducode.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    long CandidateID = entityID;//ObjcandidateRequest.candID;
                    // Int64 Request_Number = ObjcandidateRequest.requestNo;
                    Int64 Request_Nu_Last = ObjcandidateRequest.requestNo;

                    success = true;
                    if (success == true)
                        scope.Complete();
                    lblerror.Visible = true;
                    lblerror.Text = "New Request saved.";
                    // if (Request_Nu_Last == 0)
                    // {
                    //     Request_Nu_Last = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.courseID == currentCourseID && s.requestStatusID != 3)
                    //         .Select(s => s.requestNo).SingleOrDefault();



                    // }

                    // Int64 Request_Number = Request_Nu_Last;
                    //// Request_Number = Convert.ToInt64(Session["Request_Number"]);
                    // if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    // {
                    //     Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
                    // }
                    // else
                    // {
                    //     Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID +"&Request_Number=" + Request_Number));
                    // }
                    //if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    //{
                    //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestDocUpload.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID));
                    //}
                    //else
                    //{
                    //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestDocUpload.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID));
                    //}
                }
            }

            lblerror.Text = "Something Wrong..Plaese Select any Request or Login after Some time";
            using (EConnectContext context1 = new EConnectContext())
            {
                 candUpdateRequest ObjcandidateRequest;
                    ObjcandidateRequest = new candUpdateRequest();
                    Int64 Request_Nu_Last = 0;
                    long CandidateID = entityID;
                //if (Request_Nu_Last == 0)
                //{
                //    Request_Nu_Last = context1.candidateUpdateRequest.Where(s => s.candID == entityID && s.courseID == currentCourseID && s.requestStatusID != 3)
                //        .Select(s => s.requestNo).SingleOrDefault();

                //}
                    if (Request_Nu_Last == 0)
                    {
                        Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);
                        var Request_Nu = (from a in context1.candidateUpdateRequest
                                          where a.candID == entityID && a.courseID == currentCourseID && a.requestStatusID != App_Status_submittedbutfeesnotpaid //3
                                          orderby a.requestNo descending
                                          select new
                                          {
                                              RqNo = a.requestNo

                                          }).FirstOrDefault();


                        Int64 Request_Number = Request_Nu.RqNo;
                       
                        
                            //Int64 Request_Number = Convert.ToInt64(Request_Number1);
                            // Request_Number = Convert.ToInt64(Session["Request_Number"]);
                            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                            {
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
                            }
                            else
                            {
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
                            }
                        
                    }
                   

            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    #endregion

    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
    protected bool isBlankNumber(TextBox txtBox)
    {
        try
        {
            int zero = 0;

            if (txtBox.Text.Trim() == "" || txtBox.Text.Trim() == zero.ToString() || txtBox.Text.Trim() == ".")
            {
                txtBox.Text = "";
                txtBox.Focus();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidForm()
    {
        try
        {
             if (ChkHandicapped.Checked == true) 
             {
                // if(Rdhandicapped.SelectedItem == null)
                     if (Rdhandicapped.SelectedValue =="")
                   {
                     lblerror.Text = "Please Select Disability";
                      // ShowAlert("Please Select Disability");
                     return false;
                     }                 
                   
             }
             if (ChkMaritalStatus.Checked == true)
             {

                 if (!isSelected(ddlMStatus)) //on 13-01-23
                 {
                     lblerror.Visible = true;
                     lblerror.Text = "Please Select Marital Status";
                     return false;
                 }
                // if(ddlMStatus.SelectedIndex == 0)
                //{ ShowAlert("Please Select Marital Status");}          
                //     return false;
             }
             if (ChkCategory.Checked == true)
             {
                 if (!isSelected(ddlCategory))
                 {
                     lblerror.Visible = true;
                     lblerror.Text = "Please Select Cast Category";
                     return false;
                 }
                 //if (ddlCategory.SelectedIndex == 0)
                 //     { ShowAlert("Please Select Category");}     
                 //    return false;
             }
             if (chkMob.Checked == true)
             {
                 if (!isBlank(txtCorMobileNo))
                 {
                     lblerror.Visible = true;                    
                     lblerror.Text = "Mobile Number can not be left blank";
                     return false;
                 }
                 if (!isNumber(txtCorMobileNo))
                 {
                     lblerror.Visible = true;                    
                     lblerror.Text = "Invalid Mobile Number";
                     return false;
                 }
                 

                 //if (txtCorMobileNo.Text == "")
                 //  { ShowAlert("Please Enter Mobile Number");}     
                 //    return false;
    
                 //if (!isNumber("txtCorMobileNo"))
                 //    return false;
                 //if (!chekMobNo("txtCorMobileNo"))
                 //    return false;
             }
             if (ChkEmail.Checked == true)
             {
                 if (!isBlank(txtEmailId))
                 {
                     lblerror.Visible = true;
                     lblerror.Text = "Email Id can not be left blank";
                     return false;
                 }
                 
                 //if (txtEmailId.Text =="")
                 //     { ShowAlert("Please Enter Email Id");}     
                 //    return false;

                 //if (!isValidEmail("txtEmailId", "Invalid E-Mail ID"))
                 //    return false;
             }
             
             
             if (ChkCorAddress.Checked == true)
             {
                 if (TxtCorAddressLine1.Text == "" || TxtCorAddressLine2.Text == "" || TxtCorAddressLine3.Text == "" || TxtCorCity.Text == "" || ddlCorState.SelectedValue == "0" || ddldistrict.SelectedValue == "0" || txtCorPinCode.Text == "")
                 {
                     ShowAlert("Correspondence Address cant not be blank");

                     return false;
                 }
                 if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
                 {
                     TxtCorAddressLine1.Text = "";
                     TxtCorAddressLine1.Focus();
                     throw new Exception("Correspondence Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
                 }

                 if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
                 {
                     TxtCorAddressLine2.Text = "";
                     TxtCorAddressLine2.Focus();
                     throw new Exception("Correspondence Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
                 }

                 if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
                 {
                     TxtCorAddressLine3.Text = "";
                     TxtCorAddressLine3.Focus();
                     throw new Exception("Correspondence Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
                 }
                 if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorCity.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                 {
                     TxtCorCity.Text = "";
                     TxtCorCity.Focus();
                     throw new Exception("Correspondence City Name should be with an English Alphabets(e.g - a-zA-Z)");
                 }
                 if (!isSelected(ddlCorState))
                 {
                     lblerror.Visible = true;                     
                     lblerror.Text = "Please Select correspondence State";
                     return false;
                 }
                 if (!isSelected(ddldistrict))
                 {
                     lblerror.Visible = true;                    
                     lblerror.Text = "Please Select correspondence District";
                     return false;
                 }
                 if (!isBlank(txtCorPinCode))
                 {
                     lblerror.Visible = true;
                     lblerror.Text = "Pin Code can not be left blank";
                     return false;
                 }
                 if (!isNumber(txtCorPinCode))
                 {
                     lblerror.Visible = true;                    
                     lblerror.Text = "Invalid  Pin Code Number";
                     return false;
                 }
                 if (txtCorPinCode.Text.Length != 6)
                 {
                     lblerror.Text = "Invalid Pin Code Number";
                     return false;
                 }


             }
                
             if (ChkEducation.Checked == true) {
                 if (!isSelected(DDLeducode))
                 {
                     lblerror.Visible = true;                    
                     lblerror.Text = "Please Select Highest Education";
                     return false;
                 }
                 
             }
             return true;         
             }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region[Update_Candidate_Request]    
    protected void UpdateRequestData()
    {
        try
        {
            
            //if (IsValidForm())
            //{
                Boolean success = false;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var db = new EConnectContext())
                    {
                        candUpdateRequest ObjcandidateRequest; //added on 16-12-22
                        ObjcandidateRequest = new candUpdateRequest();

                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                        Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                        Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                        #region[Req Update for Disability Change]
                        if (ChkHandicapped.Checked == true)
                        {
                            Int32 Upd_Field_Handi = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);                            

                            var objCandUpdate_Handicapped = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_Handi &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_Handicapped != null)
                            {
                                //objCandUpdate_Handicapped.changedValue = Rdhandicapped.SelectedItem.Text;
                                if (LblHandicapped.Text == Rdhandicapped.SelectedItem.Text)
                                {
                                    objCandUpdate_Handicapped.requestFinalised = false;                                 
                                    
                                }
                                else
                                objCandUpdate_Handicapped.changedValue = Rdhandicapped.SelectedItem.Text;
                                db.SaveChanges();
                                ShowAlert("Request Change Successfully");
                            }
                            else
                            {
                                ShowAlert("You are not genrate request for that filed.Kindly Update your requested fields only");
                                return;
                            }
                        }
                        #endregion

                        #region[Req Update for Marital change]
                        if (ChkMaritalStatus.Checked == true)
                        {
                            Int32 Upd_Field_Matrial = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);  
                            var objCandUpdate_Matrital = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_Matrial &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_Matrital != null)
                            {
                                if (LblMaritalStatus.Text == ddlMStatus.SelectedItem.Text)
                                {
                                    objCandUpdate_Matrital.requestFinalised = false;                                   
                                    
                                }
                                else
                                objCandUpdate_Matrital.changedValue = ddlMStatus.SelectedItem.Text;
                                db.SaveChanges();
                                ShowAlert("Request Change Sucessfully");
                            }
                            else
                            {
                                ShowAlert("You are not genrate request for that filed.Kindly Update your requested fields only");
                                return;
                            }
                        }
                        #endregion

                        #region[Req Update for Caste Change]
                        if (ChkCategory.Checked == true)
                        {
                            Int32 Upd_Field_Caste = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);  
                            var objCandUpdate_Caste = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_Caste &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_Caste != null)
                            {
                                if (LblCategory.Text == ddlCategory.SelectedItem.Text) 
                                {
                                    objCandUpdate_Caste.requestFinalised = false;

                                }
                                else
                                objCandUpdate_Caste.changedValue = ddlCategory.SelectedItem.Text;
                                db.SaveChanges();
                                ShowAlert("Request Change Sucessfully");
                            }
                            else
                            {
                                ShowAlert("You are not genrate request for that filed.Kindly Update your requested fields only");
                                return;
                            }
                        }
                        #endregion

                        #region[Req Update for Mobile Change]
                        if (chkMob.Checked == true)
                        {
                            Int32 Upd_Field_Mobile = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);  
                            var objCandUpdate_Mob = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_Mobile &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_Mob != null)
                            {
                                if (LblMobile.Text == txtCorMobileNo.Text) //Logic to check if candidate edit same value as it in exist data then we update reqfinal 'No'
                                {
                                    //var ReqFlag = db.candidateUpdateRequest.Find(1);
                                    //ReqFlag.requestFinalised = false;
                                    objCandUpdate_Mob.requestFinalised = false;
                                   // ObjcandidateRequest.requestFinalised = false;                               
                                                                         
                                }
                                else
                                {
                                    objCandUpdate_Mob.changedValue = txtCorMobileNo.Text;
                                }
                                db.SaveChanges();
                                ShowAlert("Request Change Sucessfully");
                            }
                            else
                            {
                                ShowAlert("Final Submit Your data.You Can not Update.Please do Payment if you done pleae ignore");
                            }
                        }
                        #endregion

                        #region[Req Update for Email Change]
                        if (ChkEmail.Checked == true)
                        {
                            Int32 Upd_Field_Email = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress); 
                            var objCandUpdate_Email = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_Email &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_Email != null)
                            {
                                if (LblEmail.Text == txtEmailId.Text)
                                {
                                    objCandUpdate_Email.requestFinalised = false; 
                                    db.SaveChanges();
                                    //context.Database.ExecuteSqlCommand(" Update candUpdateRequest set  paymentStatusID = " + Convert.ToInt32(enmPaymentStatus.Paid) + "," +
                                    //                                        "requestStatusID=" + statusID + "," + "feesVerified=" + 1 + "," + "feesVerifiedOn=" + DateTime.Now +
                                    //                                        "," + "feesVerifiedBy=" + Convert.ToInt32(Session["RoleID"]) + " Where demandNoteID = " + demandNote.ID);
                                }
                                else
                                objCandUpdate_Email.changedValue = txtEmailId.Text;
                                db.SaveChanges();
                                ShowAlert("Request Change Sucessfully");
                            }
                            else
                            {
                                ShowAlert("You are not genrate request for that filed.Kindly Update your requested fields only");
                                return;
                            }
                        }
                        #endregion

                        #region[Req Update for CorAddress Change]
                        if (ChkCorAddress.Checked == true)
                        {
                            Int32 Upd_Field_Address = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress); 
                            var objCandUpdate_CorAddress = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_Address &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_CorAddress != null)
                            {
                                if ((LblCorAddressLine1.Text == TxtCorAddressLine1.Text && LblCorAddressLine2.Text == TxtCorAddressLine2.Text &&
                                    LblCorAddressLine3.Text == TxtCorAddressLine3.Text && LblCity.Text == TxtCorCity.Text &&
                                    LblCorState.Text == ddlCorState.SelectedItem.Text && Lbldistrict.Text == ddldistrict.SelectedItem.Text && LblCorPinCode.Text == txtCorPinCode.Text))
                                {
                                    objCandUpdate_CorAddress.requestFinalised = false;

                                }
                                else
                                objCandUpdate_CorAddress.changedValue = TxtCorAddressLine1.Text + ";" + TxtCorAddressLine2.Text + ";" + TxtCorAddressLine3.Text + ";" + TxtCorCity.Text + ";" + ddlCorState.SelectedItem.Text + ";" + ddldistrict.SelectedItem.Text + ";" + txtCorPinCode.Text;
                                db.SaveChanges();
                                ShowAlert("Request Change Sucessfully");
                            }
                            else
                            {
                                ShowAlert("You are not genrate request for that filed.Kindly Update your requested fields only");
                                return;
                            }
                        }
                        #endregion

                        #region[Req Update for HighEducation Change]
                        if (ChkEducation.Checked == true)
                        {
                            Int32 Upd_Field_HighEdu = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification); 
                            var objCandUpdate_HighEducation = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID &
                                c.updateFieldID == Upd_Field_HighEdu &
                                c.requestStatusID == App_Status_RequestReceived);

                            if (objCandUpdate_HighEducation != null)
                            {
                                if (LblLeducode.Text == DDLeducode.SelectedItem.Text) 
                                {
                                    objCandUpdate_HighEducation.requestFinalised = false;
                                    db.SaveChanges();
                                    //context.Database.ExecuteSqlCommand(" Update candUpdateRequest set  paymentStatusID = " + Convert.ToInt32(enmPaymentStatus.Paid) + "," +
                                    //                                        "requestStatusID=" + statusID + "," + "feesVerified=" + 1 + "," + "feesVerifiedOn=" + DateTime.Now +
                                    //                                        "," + "feesVerifiedBy=" + Convert.ToInt32(Session["RoleID"]) + " Where demandNoteID = " + demandNote.ID);
                                }
                                else
                                objCandUpdate_HighEducation.changedValue = DDLeducode.SelectedItem.Text;
                                db.SaveChanges();
                                ShowAlert("Request Change Sucessfully");
                            }
                            else
                            { ShowAlert("You are not genrate request for that filed.Kindly Update your requested fields only"); }

                        }
                        #endregion


                        //    long CandidateID = entityID;//ObjcandidateRequest.candID;
                        //    scope.Complete();
                        //    lblerror.Visible = true;
                        //    lblerror.Text = "Request Updated.";
                        //    if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                        //    {
                        //        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID));
                        //    }
                        //    else
                        //    {
                        //        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID));
                        //    }

                        long CandidateID = entityID;//ObjcandidateRequest.candID;                   
                        Int64 Request_Nu_Last = ObjcandidateRequest.requestNo;

                        success = true;
                        if (success == true)
                            scope.Complete();
                        lblerror.Visible = true;
                        lblerror.Text = "Update Request Successfully.";

                    }

                }
                using (EConnectContext context1 = new EConnectContext())
                {
                    candUpdateRequest ObjcandidateRequest;
                    ObjcandidateRequest = new candUpdateRequest();
                    Int64 Request_Nu_Last = 0;

                    long CandidateID = entityID;

                    if (Request_Nu_Last == 0)
                    {
                         Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);
                        var Request_Nu = (from a in context1.candidateUpdateRequest
                                          where a.candID == entityID && a.courseID == currentCourseID && a.requestStatusID != App_Status_submittedbutfeesnotpaid  //3
                                          orderby a.requestNo descending
                                          select new
                                          {
                                              RqNo = a.requestNo

                                          }).FirstOrDefault();

                        //var Request_Nu = (from a in context1.candidateUpdateRequest
                        //                  where a.candID == entityID && a.courseID == currentCourseID && a.requestStatusID != 3
                        //                  orderby a.requestNo descending
                        //                  select new
                        //                  {
                        //                      RqNo = a.requestNo

                        //                  }).LastOrDefault(); //on 6-02-23 on the bais of status check


                        Int64 Request_Number = Request_Nu.RqNo;
                        

                        if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                        {
                            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
                        }
                        else
                        {
                            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
                        }
                    }

                }

           // }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    #endregion


    protected void btnupdate_Click(object sender, EventArgs e)
    {
       // UpdateAll();//old 
       
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
         try
        {
          if (IsValidForm())
          {

        if (!String.IsNullOrEmpty(Request.QueryString["RegApplID"]))
        {
            btnSave.Text = "Update";
            btnback.Visible = false;
            UpdateRequestData();
        }
        else
        {
            if ((ChkHandicapped.Checked == false) && (ChkMaritalStatus.Checked == false) && (ChkCategory.Checked == false) && (chkMob.Checked == false) && (ChkEmail.Checked == false) && (ChkCorAddress.Checked == false) && (ChkEducation.Checked == false))
            { ShowAlert("Please Check Some fields for Updation Request"); }
            else
            {
                btnSave.Text = "Save/Preview";
                btnback.Visible = true;
                InsertRequestData();
            }
        }
       }
      }
      catch (Exception ex)
      {
        ShowAlert(ex.Message);
      }
    }

    
    #region[Display_Candidate_Previous_RequestUpdate_Data]
    protected void  showEditRequestData()
    {
        //ViewState["LastModifiedOn"]
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
                Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
                Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

                #region[Handicapped]
                Int32 Upd_Field_Handi = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);   
                var Handicapped = (from s in context.candidateUpdateRequest
                                   orderby (s.updateFieldID)
                                   where s.updateFieldID == Upd_Field_Handi //1
                                && s.candID == candidateID && s.requestNo == Request_Number && s.requestStatusID == App_Status_RequestReceived //1
                                   select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();


                if (Handicapped != null)
                {                   
                    //LblHandicapped.Text = Handicapped.ValueField;
                    Rdhandicapped.Enabled = true;
                    //Rdhandicapped.Text = Handicapped.ValueField;
                    if (Handicapped.ValueField == "Yes")
                    {
                        Rdhandicapped.SelectedValue = "1";
                    }
                    else { Rdhandicapped.SelectedValue = "0"; }
                  
                }
                else { Rdhandicapped.Enabled = false;
                ChkHandicapped.Visible = false;
                //ChkHandicapped.Enabled = false;
                }
                #endregion

                #region[Marital]
                Int32 Upd_Field_Matrial = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus); 
                var marital = (from s in context.candidateUpdateRequest
                               orderby (s.updateFieldID)
                               where s.updateFieldID == Upd_Field_Matrial //&& s.demandNoteID == null
                                && s.candID == candidateID && s.requestNo == Request_Number && s.requestStatusID == App_Status_RequestReceived
                               select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (marital != null)
                {
                    //LblMaritalStatus.Text = marital.ValueField;
                    ddlMStatus.Enabled = true;
                    ddlMStatus.SelectedItem.Text = marital.ValueField;
                }
                else { ddlMStatus.Enabled = false;
                ChkMaritalStatus.Visible = false;
                }
                #endregion

                #region[Category]
                Int32 Upd_Field_category = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory); 
                var category = (from s in context.candidateUpdateRequest
                                orderby (s.updateFieldID)
                                where s.updateFieldID == Upd_Field_category &&
                                s.requestNo == Request_Number && s.requestStatusID == App_Status_RequestReceived
                                && s.candID == candidateID
                                select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (category != null)
                {                   
                    ddlCategory.Enabled = true;
                    ddlCategory.SelectedItem.Text = category.ValueField;
                }
                else { ddlCategory.Enabled = false;
                ChkCategory.Visible = false;
                }
                #endregion

                #region[Mobile]
                Int32 Upd_Field_Mobile = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber); 
                var Mobile = (from s in context.candidateUpdateRequest
                                orderby (s.updateFieldID)
                              where s.updateFieldID == Upd_Field_Mobile && s.requestNo == Request_Number &&
                                s.requestStatusID == App_Status_RequestReceived && s.candID == candidateID 
                                select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (Mobile != null)
                {
                    txtCorMobileNo.Enabled = true;
                    txtCorMobileNo.Text = Mobile.ValueField;
                }
                else { txtCorMobileNo.Enabled = false;
                chkMob.Visible = false;
                }
               #endregion

                #region[Email]
                Int32 Upd_Field_Email = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress); 
                var Email = (from s in context.candidateUpdateRequest
                             orderby (s.updateFieldID)
                             where s.updateFieldID == Upd_Field_Email && s.requestNo == Request_Number &&
                             s.requestStatusID == App_Status_RequestReceived && s.candID == candidateID
                             select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (Email != null)
                {
                    txtEmailId.Enabled = true;
                    txtEmailId.Text = Email.ValueField;
                }
                else { txtEmailId.Enabled = false;
                ChkEmail.Visible = false;
                }
                #endregion

                #region[CorrespondenceAddress]
                Int32 Upd_Field_Address = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress); 
                var CorrespondenceAddress = (from s in context.candidateUpdateRequest
                                             orderby (s.updateFieldID)
                                             where s.updateFieldID == Upd_Field_Address && s.requestNo == Request_Number &&
                                             s.requestStatusID == App_Status_RequestReceived
                                             && s.candID == candidateID
                                             select new { changevalue = s.changedValue, Date = s.enterdate }).FirstOrDefault();
                if (CorrespondenceAddress != null)
                {
                    string complete_add = CorrespondenceAddress.changevalue;
                    string[] valuesList = complete_add.Split(';');
                    TxtCorAddressLine1.Enabled = true;
                    TxtCorAddressLine1.Text = valuesList[0].ToString();

                    TxtCorAddressLine2.Enabled = true;
                    TxtCorAddressLine2.Text = valuesList[1].ToString();

                    TxtCorAddressLine3.Enabled = true;
                    TxtCorAddressLine3.Text = valuesList[2].ToString();

                    TxtCorCity.Enabled = true;
                    TxtCorCity.Text = valuesList[3].ToString();

                    ddlCorState.Enabled = true;
                    ddlCorState.SelectedItem.Text = valuesList[4].ToString();

                    ddldistrict.Enabled = true;
                    ddldistrict.SelectedItem.Text = valuesList[5].ToString();

                    txtCorPinCode.Enabled = true;
                    txtCorPinCode.Text = valuesList[6].ToString();
                }
                else { TxtCorAddressLine1.Enabled = false;
                TxtCorAddressLine2.Enabled = false;
                TxtCorAddressLine3.Enabled = false;
                TxtCorCity.Enabled = false;
                ddlCorState.Enabled = false;
                ddldistrict.Enabled = false;
                txtCorPinCode.Enabled = false;

                ChkCorAddress.Visible = false;
                }
              
                #endregion

                #region[HighEducation]
                Int32 Upd_Field_HighEdu = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification); 
                var HighEducation = (from s in context.candidateUpdateRequest
                                     orderby (s.updateFieldID)
                                     where s.updateFieldID == Upd_Field_HighEdu && s.requestNo == Request_Number &&
                                     s.requestStatusID == App_Status_RequestReceived
                                     && s.candID == candidateID
                                     select new { HighEdu = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (HighEducation != null)
                {
                    DDLeducode.Enabled = true;
                    DDLeducode.SelectedItem.Text = GetInitCap(HighEducation.HighEdu);                  
                }
                else { DDLeducode.Enabled = false;
                ChkEducation.Visible = false;
                }
                #endregion
            }
            
        }
        catch (Exception ex)
        {
            //catch (Exception ex) { throw ex; }
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        } 
    }
#endregion
}