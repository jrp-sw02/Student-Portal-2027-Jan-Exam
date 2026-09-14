using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.Objects;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

public partial class FrmdashBoard : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    string requesturl = "";
   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {     
                if (IsSessionAlive() == false)
                    Response.Redirect("Index.aspx");
                loginUserType = (UserType)Session["UserType"];
                entityID = Convert.ToInt64(Session["EntityID"]);
                if (!IsPostBack)
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    if (loginUserType == UserType.Admin)
                    {
                        //Get Data From External_Entity Table URM.ExternalEntity.cs
                        //lblHeading.Text = "Admin Dashboard";
                    }
					
					 if (loginUserType == UserType.Examiner)
                     {
                        
                        using (EConnectContext context = new EConnectContext())
                        {
                            String UserId = context.Users.Where(c => c.UserRefNumber == entityID ).Select(s => s.LoginID).FirstOrDefault();
                            String CentreCode = UserId.Substring(UserId.Length - 9);
                            
                            Response.Redirect("~/Admin/PracExaminerDashboard.aspx?CentreCode=" + CentreCode, false);
                         }
                     }
                    if (loginUserType == UserType.Observer )
                     {
                       
                        using (EConnectContext context = new EConnectContext())
                        {
                            String UserId = context.Users.Where(c => c.UserRefNumber == entityID).Select(s => s.LoginID).FirstOrDefault();
                            String CentreCode = UserId.Substring(UserId.Length - 9);
                           
                            Response.Redirect("~/Admin/PracObserverDashboard.aspx?CentreCode=" + CentreCode, false);
                         
                        }
                     }
					
					
                    #region--Regional Centre--------------------------------------------
                    else if (loginUserType == UserType.RegionalCenter)
                    {
                        //Get User Data From NIELIT.REGIONALCENTRE.cs
                        //lblHeading.Text = "Regional Centre Dashboard";                    
                        regCentre.Visible = true;
                        givInstitute.Visible = false;
                        Coursediv.Visible = false;
                        CourseExamdiv.Visible = false;
						divProject.Visible = false;
                        divHO.Visible = false;
                        divDLC.Visible = false;
                        BindRegCentreData();
                    }
                    #endregion ----------------------------------------------------------------------------------------------------------
                    #region--Institute-----------------------------------------------------
                    else if (loginUserType == UserType.Institute)
                    {
                        givInstitute.Visible = true;
                        divInstitute.Visible = true;
                        regCentre.Visible = false;
                        CourseExamdiv.Visible = true;
						divProject.Visible = true;
                        Coursediv.Visible = true;
                        divHO.Visible = false;
                        divDLC.Visible = false;
                        using (EConnectContext context = new EConnectContext())
                        {
                            string sortOrder = ViewState["SortOrder"].ToString();
                            string sortField = ViewState["SortField"].ToString();
                            divInstitute.Visible = true;
                            Institute loginInsitute = context.Institutes.Find(entityID);
                            if (loginInsitute != null)
                            {
                                lblHeading.Text = "Welcome " + GetInitCap(loginInsitute.Name);

                                BindCoursesData();
                                BindCertificateExamData();
                                BindCourseExamData();
								BindProjectData();
                            }
                        };
                    }
                    #endregion-----------------------------------------------------------------------------
                    #region--Candidate-----------------------------------------------------
                    else if (loginUserType == UserType.Candidate)
                    {
                        givInstitute.Visible = false;
                        divInstitute.Visible = false;
                        regCentre.Visible = false;
                        CourseExamdiv.Visible = false;
						divProject.Visible = false;
                        Coursediv.Visible = false;
                        divHO.Visible = false;
                        divDLC.Visible = false;

                        Boolean personaldetailupdate = false;
                        Boolean contactdetailupdate = false;
                        Boolean addressdetailupdate = false;
                        Boolean photodetailupdate = false;
                        Int32 counter = 0;
                        StringBuilder Paymentname = new StringBuilder();
						StringBuilder appendexamcycle = new StringBuilder();
                        StringBuilder deficiencyname = new StringBuilder();
                        int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        using (EConnectContext context = new EConnectContext())
                        {

                            var candidatePersonal = (from a in context.Candidates
                                                     where a.ID == entityID
                                                     select new
                                                     {
                                                         guardianname = a.GuardianName,
                                                         FatherName = a.FatherName,
                                                         MotherName = a.MotherName,
                                                         gender = a.Gender,
                                                         photo = a.PhotoFileID,
                                                         signature = a.SignatureFileID,
                                                         thumb = a.LeftThumbImpressionFileID,
                                                         locked = a.IsLocked,
                                                         verified = a.IsVerified,
                                                         verifieddate = a.VerifiedOn,
                                                         lockeddate = a.LockedOn
                                                     }).FirstOrDefault();

                            var candidatecontact = (from a in context.CandidateContactDetails
                                                    where a.CandidateID == entityID
                                                    orderby a.EffectiveFromDate descending
                                                    select new
                                                    {
                                                        MobileNumber = a.MobileNumber,
                                                        EmailAddress = a.EmailAddress,
                                                        IsMobileVerified = a.IsMobileNumberVerified,
                                                        IsEmailVerified = a.IsEmailVerified,
                                                    }).FirstOrDefault();

                            var candidateaddress = (from a in context.Addresses
                                                    where a.CandidateID == entityID && a.AddressTypeID == CorAddTypeId
                                                    orderby a.EffectiveDateFrom descending
                                                    select new
                                                    {
                                                        Addline1 = a.AddressLine1,
                                                        Addline2 = a.AddressLine2,
                                                        Addline3 = a.AddressLine3,
                                                        StateID = a.StateID,
                                                        DistrictID = a.DistrictID,
                                                        CityName = a.CityName,
                                                    }).FirstOrDefault();

                            //commentted date 15032023
                            //var latestcourse = (from c in context.RegistrationDetails
                            //                    where c.CandidateID == entityID && (c.RegistrationStatusID == 1 || c.RegistrationStatusID == 2 || c.RegistrationStatusID == 3 || c.RegistrationStatusID == 4)
                            //                    orderby c.CommencementFromDate descending
                            //                    select new
                            //                    {
                            //                        courseid = c.CourseID,
                            //                        coursecatId = c.CourseCategoryID,
                            //                        courseCatName = c.CourseCategory.Name,
                            //                        apptypeid = c.ApplicantTypeID,
                            //                        regno = c.RegistrationNo,
                            //                        regstatusid = c.RegistrationStatusID,
                            //                        validuptodate = c.ValidUptoDate,
                            //                        CommencementFromDate = c.CommencementFromDate
                            //                    }).FirstOrDefault();

                            var latestcourse = (from c in context.RegistrationDetails
                                                where c.CandidateID == entityID && (c.RegistrationStatusID == 1 || c.RegistrationStatusID == 2 || c.RegistrationStatusID == 3 || c.RegistrationStatusID == 4)
                                                orderby c.CommencementFromDate descending,c.CourseID descending
                                                select new
                                                {
                                                    courseid = c.CourseID,
                                                    coursecatId = c.CourseCategoryID,
                                                    courseCatName = c.CourseCategory.Name,
                                                    apptypeid = c.ApplicantTypeID,
                                                    regno = c.RegistrationNo,
                                                    regstatusid = c.RegistrationStatusID,
                                                    validuptodate = c.ValidUptoDate,
                                                    CommencementFromDate = c.CommencementFromDate
                                                }).FirstOrDefault();

                            if (latestcourse.coursecatId == 1)
                            {
                                LinkReusltSheetDownload.Visible = true;
                            }


                            if (!context.Feedbacks.Any(s => s.UserID == entityID))
                            {
                                SideLink1.Items.Add(new SideLinkItem("Give Feedback/Suggestions", "CAND/CandidateFeedBack.aspx", "images/Get_Filled_Form.jpg"));
                                SideLink1.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                                SideLink1.Render();
                            }

                            // deep code add on 12 March 2020 for NSQF Candidates Result View button 
                            if (latestcourse.coursecatId == 6)
                            {
                                SideLink1.Items.Add(new SideLinkItem("NSQF Result View", "CAND/NSQFCandidatesResultView.aspx", "images/View_Result.jpg"));
                                SideLink1.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                                SideLink1.Render();
                            }
                            // deep cod add END on 12 March 2020 for NSQF Candidates Result View button 

                            //if (!context.CandidateScholarships.Any(s => s.CandidateID == entityID))
                            //{
                            //    if (latestcourse.apptypeid == Convert.ToInt32(enmApplicantType.Institute))
                            //    {
                            //        SideLink1.Items.Add(new SideLinkItem("Aadhaar & Bank Acc. Details", "CAND/CandidateScholarship.aspx", "images/Apply_Online.jpg"));
                            //        SideLink1.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                            //        SideLink1.Render();
                            //    }
                            //}  
                    
                            if (latestcourse.courseCatName == "IRDA") //------ IRDA Exam -------------
                            {
                                SideLink1.Items.Add(new SideLinkItem("Exam Application (IRDA)", "WEB/allCoursesExam.aspx", "images/Get_Filled_Form.jpg"));
                                SideLink1.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                                SideLink1.Render();
                            }
                            if (string.IsNullOrEmpty(candidatePersonal.guardianname) == true && string.IsNullOrWhiteSpace(candidatePersonal.guardianname) == true)
                            {
                                if ((candidatePersonal.FatherName == null || candidatePersonal.FatherName.Trim() == "") || (candidatePersonal.MotherName == null || candidatePersonal.MotherName.Trim() == "") || (candidatePersonal.gender == null || candidatePersonal.gender.Trim() == ""))
                                {
                                    divprofile.Visible = true;
                                    lipersonal.Visible = true;
                                    Lnkpersonal.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmEditCandidateDetail.aspx?SrcType=1");
                                }
                            }
                            else
                            {
                                if (candidatePersonal.guardianname == null || candidatePersonal.guardianname.Trim() == "")
                                {
                                    divprofile.Visible = true;
                                    lipersonal.Visible = true;
                                    Lnkpersonal.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmEditCandidateDetail.aspx?SrcType=1");
                                }
                            }
                            if (string.IsNullOrEmpty(candidatePersonal.guardianname) == true && string.IsNullOrWhiteSpace(candidatePersonal.guardianname) == true)
                            {
                                if ((candidatePersonal.FatherName != null && candidatePersonal.FatherName.Trim() != "") && (candidatePersonal.MotherName != null && candidatePersonal.MotherName.Trim() != "") && (candidatePersonal.gender != null && candidatePersonal.gender.Trim() != ""))
                                {
                                    personaldetailupdate = true;
                                }
                            }
                            else
                            {
                                if (candidatePersonal.guardianname != null || candidatePersonal.guardianname.Trim() != "")
                                {
                                    personaldetailupdate = true;
                                }
                            }
                            if ((!candidatePersonal.photo.HasValue || candidatePersonal.photo == 0) || (!candidatePersonal.signature.HasValue || candidatePersonal.signature == 0) || (!candidatePersonal.thumb.HasValue || candidatePersonal.thumb == 0))
                            {
                                divprofile.Visible = true;
                                liphoto.Visible = true;
                            }
                            if ((candidatePersonal.photo.HasValue && candidatePersonal.photo != 0) && (candidatePersonal.signature.HasValue && candidatePersonal.signature != 0) && (candidatePersonal.thumb.HasValue && candidatePersonal.thumb != 0))
                            {
                                photodetailupdate = true;
                            }
                            if (candidatecontact != null)
                            {
                                if ((!candidatecontact.MobileNumber.HasValue || candidatecontact.MobileNumber == 0) || (candidatecontact.EmailAddress.Trim() == "" || candidatecontact.EmailAddress == null))
                                {
                                    divprofile.Visible = true;
                                    licontact.Visible = true;
                                    Lnkcontact.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmEditCandidateDetail.aspx?SrcType=2");
                                }
                                if (candidatecontact.MobileNumber.HasValue && candidatecontact.MobileNumber != 0)
                                {
                                    if (candidatecontact.IsMobileVerified == false)
                                    {
                                        divprofile.Visible = true;
                                        limobno.Visible = true;
                                        var reg = context.RegistrationDetails.Where(q => q.CandidateID == entityID).OrderByDescending(q => q.CourseID).FirstOrDefault();
                                        if (reg != null)
                                            lbmessage.Text = "<br/>(<b>Note:-</b> If you are not able to verify your mobile number through OTP process, you can verify your mobile number by <br/> sending sms <b>NIELIT VM " + reg.RegistrationNo + " to 51969</b>)";
                                    }
                                }
                                if (string.IsNullOrEmpty(candidatecontact.EmailAddress) == false && !string.IsNullOrWhiteSpace(candidatecontact.EmailAddress))
                                {
                                    if (candidatecontact.IsEmailVerified == false)
                                    {
                                        divprofile.Visible = true;
                                        liemail.Visible = true;
                                    }
                                }
                                if ((candidatecontact.MobileNumber.HasValue && candidatecontact.MobileNumber != 0) && (string.IsNullOrEmpty(candidatecontact.EmailAddress) == false && candidatecontact.EmailAddress != null) && (candidatecontact.IsEmailVerified == true && candidatecontact.IsMobileVerified == true))
                                {
                                    contactdetailupdate = true;
                                }
                            }
                            else
                            {
                                divprofile.Visible = true;
                                licontact.Visible = true;
                                Lnkcontact.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmEditCandidateDetail.aspx?SrcType=2");
                            }
                            if (candidateaddress != null)
                            {
                                if ((string.IsNullOrWhiteSpace(candidateaddress.Addline1) || candidateaddress.Addline1 == null) || (candidateaddress.StateID == null) || (candidateaddress.DistrictID == null) || (string.IsNullOrWhiteSpace(candidateaddress.CityName) || candidateaddress.CityName == null))
                                {
                                    divprofile.Visible = true;
                                    liaddress.Visible = true;
                                    Lnkaddress.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmEditCandidateDetail.aspx?SrcType=3");
                                }
                                if ((string.IsNullOrWhiteSpace(candidateaddress.Addline1) == false && candidateaddress.Addline1 != null) && (candidateaddress.StateID != null) && (candidateaddress.DistrictID != null) && (string.IsNullOrWhiteSpace(candidateaddress.CityName) == false && candidateaddress.CityName != null))
                                {
                                    addressdetailupdate = true;
                                }
                            }
                            else
                            {
                                divprofile.Visible = true;
                                liaddress.Visible = true;
                                Lnkaddress.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmEditCandidateDetail.aspx?SrcType=3");
                            }
                            if ((addressdetailupdate == true) && (contactdetailupdate == true) && (photodetailupdate == true) && (personaldetailupdate == true))
                            {
                                if (candidatePersonal.locked == false)
                                {
                                    divprofile.Visible = true;
                                    linotlocked.Visible = true;
                                    Lnknotlocked.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/myprofile.aspx");
                                }
                            }
                            if (candidatePersonal.locked == true && candidatePersonal.verified == false)
                            {
                                divprofile.Visible = true;
                                lilocked.Visible = true;
                                Lbllockeddate.Text = candidatePersonal.lockeddate.Value.ToString("dd-MMM-yyyy");
                              //Lnklocked.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/Application.aspx") + "','Form'); return false;");
                            }
                            if (candidatePersonal.locked == true && candidatePersonal.verified == true)
                            {
                                divprofile.Visible = true;
                                liverified.Visible = true;
                                liverified.InnerText = "Your profile details have been verified successfully on " + candidatePersonal.verifieddate.Value.ToString("dd-MMM-yyyy");
                            }
                            //Exam Notification
                            var nextExam = CourseManager.GetNextExam(context, latestcourse.courseid, latestcourse.apptypeid);
                           // if (nextExam == null)// exist before 
                            int courseId = (from m in context.RegistrationDetails where m.CandidateID == entityID && (m.RegistrationStatusID == 1 || m.RegistrationStatusID == 2) orderby m.CourseID descending select m.CourseID).FirstOrDefault();
                            //int ExamId=(from E in context.Exams where E.CourseID==courseId orderby E.ID descending select E.ID).FirstOrDefault();
                            int ExamId = (from E in context.CourseExamApplications where E.CourseID == courseId && E.CandidateID == entityID && (E.ApplicationStatusID == 10 || E.ApplicationStatusID == 8) && (E.PaymentStatusID == 2 || E.PaymentStatusID == 4) orderby E.ExamID descending select E.ExamID).FirstOrDefault();
                          // added code by abhi singh dated on 15122023 for specisal case for this course
                            // if (courseId == 1213)
                                // ExamId = 7512;
                            if (nextExam == null )
                            { 
                                divcourse.Visible = true;
                                var examinformation = (from r in context.CourseExamApplications
                                                       //join d in context.Exams on r.ExamID equals d.ID
                                                       where r.CandidateID == entityID && r.FinalSubmitted == true && (r.RollNumber != null || r.NSQFRollNo!=null) && r.CourseID == courseId && r.ExamID == ExamId
                                    && (r.ApplicationStatusID == 10 || r.ApplicationStatusID == 8) && (r.PaymentStatusID == 2 || r.PaymentStatusID == 4) 
                                                       orderby r.ID descending
                                                       select new
                                                       {
                                                           appno = r.ID,
                                                           RegistrationNumber = r.RegistrationNumber,
                                                           dateofdownloadadmitcard = r.Exam.DateOfPublishingOfRollNumber,
                                                           dateofresultdeclaration = r.Exam.DateOfPublishingOfResult,
                                                           dateoftimetablepublishing = r.Exam.DateOfPublishingOfTimeTable,
                                                           dateofpracticaladmitcard = r.Exam.DateofPublishingPracticalAdmitCard,
                                                           dateonlineAdmitcard = r.Exam.Online_Admit_Card_Publish_Date,
                                                           nofpractmodule = r.NumberOfPracticalModulesApplied,
                                                           nooftheorymodule=r.NumberOfTheoryModulesApplied,
                                                           rollno = r.RollNumber.HasValue ? r.RollNumber.Value : 0,
                                                           pr_officerefno = r.PracticalOfficeRefNumber,
                                                           courseid = r.CourseID,
                                                           course_cate_id=r.CourseCategoryID,
                                                           examid = r.ExamID,
                                                           examname = r.Exam.Name,
                                                           coursename = r.Course.Name,
                                                           nsqfRollNo= r.NSQFRollNo
                                                       }).FirstOrDefault();

                                // added code for validate online/offline admit card with module, date on dated 16082023 
                                //int courseId=(from m in context.RegistrationDetails where m.CandidateID==entityID && (m.RegistrationStatusID==1||m.RegistrationStatusID==2) orderby m.CourseID descending select m.CourseID).FirstOrDefault();
                                //int ExamId=(from E in context.Exams where E.CourseID==courseId orderby E.CourseID descending select E.CourseID).FirstOrDefault();
                                //var download = (from a in context.CourseExamApplications
                                //                //where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.RegistrationNumber == registrationNumber) && (a.ExamID == examID || a.ExamID == previousExamId) && a.CourseID == CourseID && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true 
                                //                where a.CandidateID == entityID && a.ExamID == ExamId && a.CourseID == courseId  && a.FinalSubmitted == true
                                //                && (a.RollNumber != null)
                                //                && a.ApplicationStatusID == 10 && (a.PaymentStatusID == 2 || a.PaymentStatusID == 4)
                                //                orderby a.ExamID descending
                                //                //where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.ID == registrationNumber) && a.ExamID == examID && a.CourseID == CourseID && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true && a.RollNumber != null
                                //                select new
                                //                {
                                //                    name = a.Candidate.Salutation + a.Candidate.Name,
                                //                    Appid = a.ID,
                                //                    fname = a.Candidate.FatherName,
                                //                    mname = a.Candidate.MotherName,
                                //                    gname = a.Candidate.GuardianName,
                                //                    doe = a.Exam.ExamStartDate,
                                //                    dob = a.Candidate.DateOfBirth,
                                //                    rollno = a.RollNumber,
                                //                    examname = a.Exam.Name,
                                //                    examcentre = a.AllottedExamCentre.Name,
                                //                    pracexamcentre = a.PracticalInstituteName,
                                //                    noofpractmodule = a.NumberOfPracticalModulesApplied,
                                //                    nooftheorymodule = a.NumberOfTheoryModulesApplied,
                                //                    rollnumber = a.RollNumber.HasValue ? a.RollNumber.Value : 0,
                                //                    profficerefno = a.PracticalOfficeRefNumber,
                                //                    examId = a.ExamID
                                //                }).FirstOrDefault();
                                
                                if (examinformation != null)
                                {
                                    if (examinformation.dateoftimetablepublishing.HasValue)
                                    {
                                        liexamtimetable.Visible = true;
                                        Lbtimetable.Text = examinformation.examname + " (" + GetInitCap(examinformation.coursename) + ") :- <b> Time Table declared on " + examinformation.dateoftimetablepublishing.Value.ToString("dd-MMM-yyyy") + ".</b>";
                                        lnktimetable.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./Admin/ExamTimeTableReport.aspx?CourseId=" + examinformation.courseid.ToString() + "&ExamId=" + examinformation.examid.ToString()) + "');");
                                    }

                                    List<int> data = new List<int> { 942, 943, 944, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955 };
                                    List<int> data1 = (from m in context.CourseExamApplicationDetails where m.CourseID == 2 && m.ExamID == examinformation.examid && m.CandidateID == entityID select m.ModuleID).ToList();
                                    int count = data1.Intersect(data).Count();
                                    if (examinformation.nooftheorymodule != 0 && examinformation.dateofdownloadadmitcard.HasValue && examinformation.rollno != 0 && examinformation.courseid != 1 && examinformation.courseid != 1213 && ((examinformation.courseid == 2 && count != 0) || examinformation.courseid != 2))
                                    {
                                        //divcourse.Visible = true;
                                         lidownloadadmitcard.Visible = true; //Uncomment below line after Covid -19  
                                         Lbdownload.Visible = true;
                                         Lbdownload.Text = examinformation.examname + " (" + GetInitCap(examinformation.coursename) + ") :- <b> Offline Admit Card Published on " + examinformation.dateofdownloadadmitcard.Value.ToString("dd-MMM-yyyy") + ".</b>";
                                        //Lbdownload.Text = "Due to Covid - 19 pandemic, it is recommended that candidate should  download Admit Card from https://student.nielit.gov.in at  Download Admit Card section after reading the declaration thoroughly.";
                                        Lnkdownload.Visible = true;
                                        Lnkdownload.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/CourseAdmitCard_Ver8.aspx?id=" + examinformation.courseid.ToString() + "&Appid=" + examinformation.appno) + "');");
                                    }

                                     List<int> data2 = new List<int> { 938, 939, 940, 941 };
                                    //List<int> data2 = new List<int> { 942, 943, 944, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955 };
                                    List<int> data3 = (from m in context.CourseExamApplicationDetails where m.CourseID == 2 && m.ExamID == examinformation.examid && m.CandidateID == entityID select m.ModuleID).ToList();
                                    int count1 = data3.Intersect(data2).Count();
                                    if (examinformation.nooftheorymodule != 0 && examinformation.dateonlineAdmitcard.HasValue && (examinformation.courseid == 1 || examinformation.course_cate_id == 3 || examinformation.course_cate_id == 6 || (examinformation.courseid == 2 && count1 != 0)))
                                    {
                                        //divcourse.Visible = true;
                                         Onlinelidownloadadmitcard.Visible = true; //Uncomment below line after Covid -19 
                                         OnlineLbdownload.Visible = true;
                                         OnlineLbdownload.Text = examinformation.examname + " (" + GetInitCap(examinformation.coursename) + ") :- <b> Online Admit Card Published on " + examinformation.dateonlineAdmitcard.Value.ToString("dd-MMM-yyyy") + ".</b>";
                                        //Lbdownload.Text = "Due to Covid - 19 pandemic, it is recommended that candidate should  download Admit Card from https://student.nielit.gov.in at  Download Admit Card section after reading the declaration thoroughly.";
                                        OnlineLnkadmitcard.Visible = true;
                                        OnlineLnkadmitcard.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/OnlineCourseAdmitCard_Ver1.aspx?id=" + examinformation.courseid.ToString() + "&Appid=" + examinformation.appno) + "');");
                                    }
                                    if (examinformation.dateofpracticaladmitcard.HasValue && examinformation.nofpractmodule != 0 )
                                    {
                                        lipracadmitcard.Visible = true;
                                        Lbpractical.Text = examinformation.examname + " (" + GetInitCap(examinformation.coursename) + ") :- <b> Practical Examination Admit Card declared on " + examinformation.dateofpracticaladmitcard.Value.ToString("dd-MMM-yyyy") + ".</b>";
                                        Lnkpracadmitcard.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/CoursePracticalAdmitCard_Ver5.aspx?id=" + examinformation.courseid.ToString() + "&Appid=" + examinformation.appno) + "');");
                                    }
                                    if (examinformation.dateofresultdeclaration.HasValue)
                                    {
                                        //divcourse.Visible = true;
                                        liviewresult.Visible = true;
                                        Lbresult.Text = examinformation.examname + " (" + GetInitCap(examinformation.coursename) + ") :- <b> Result declared on " + examinformation.dateofresultdeclaration.Value.ToString("dd-MMM-yyyy") + ".</b>";
                                        Lnkresult.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./WEB/ViewCourseResult.aspx?id=" + examinformation.courseid.ToString() + "&RegNo=" + examinformation.RegistrationNumber.ToString() + "&ExamId=" + examinformation.examid) + "');");
                                    }
                                    // code added by Abhi singh on dated 04122023 for give a provision for Edit Exam city Center of specific student.
                                    DateTime examstartdate = (from m in context.Exams where m.ID == ExamId select m.ExamStartDate).FirstOrDefault();
                                    var examstatus12 = (from r in context.CourseExamApplications
                                                        where r.ExamID == ExamId && r.CandidateID == entityID && r.FinalSubmitted == true
                                                        select new
                                                        {
                                                            RegistrationNumber = r.RegistrationNumber,
                                                            courseid = r.CourseID,
                                                            nsqfRollNo = r.NSQFRollNo
                                                        }).FirstOrDefault();
                                   // List<Int64> regListdata = new List<Int64>() { 1, 1014318, 998379, 1430340, 1166255, 1419449, 1125666, 1271187, 1170687, 1470772, 1485745, 1355663, 1254043, 1334353, 1378878, 1569472, 1275098, 1435976, 1037072, 1376818, 1336718, 1227668, 941205, 1159256, 1505690, 1280455, 1505689, 1389506, 1448958, 1292208, 1505688, 1087768, 1237582, 1204594, 1505687, 1505683, 1275632, 891046, 1417739, 1090277, 1340175, 1054566, 1446233, 1059386, 1273248, 1270725, 1344034, 1294885, 1393365, 1312991, 1296548, 1187980, 1325075, 1295700, 1487440, 1458531, 1402523, 1408165, 1139252, 1154947, 1565461, 1461252, 1586272, 1510082, 1510154 };
                                    //List<Int64> compdata = new List<Int64>() { 1, Convert.ToInt64(examstatus12.RegistrationNumber) };
                                   // List<Int64> stser = regListdata.Intersect(compdata).ToList();
                                   // if (stser.Count > 1 && examstatus12.nsqfRollNo != "Y")
                                  //  {
                                       // linotexamEdit.Visible = true;
                                      //  lnknotexamEdit.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmExamForm_Edit.aspx?CourseID=" + examstatus12.courseid + "&RegNo=" + examstatus12.RegistrationNumber + "&examStartDate=" + examstartdate);
                                  //  }
                                }
                                else
                                {
                                    // code added by Abhi singh on dated 04122023 for give a provision for Edit Exam city Center of specific student.
                                    //DateTime examstartdate = (from m in context.Exams where m.ID == ExamId select m.ExamStartDate).FirstOrDefault();
                                    //var examstatus = (from r in context.CourseExamApplications
                                    //                   where r.ExamID == ExamId && r.CandidateID == entityID && r.FinalSubmitted == true
                                    //              select new
                                    //              {
                                    //               RegistrationNumber = r.RegistrationNumber,
                                    //               courseid = r.CourseID,
                                    //                nsqfRollNo=r.NSQFRollNo
                                    //              }).FirstOrDefault();
                                    //List<Int64> regListdata = new List<Int64>() { 1, 1014318, 998379, 1430340, 1166255, 1419449, 1125666, 1271187, 1170687, 1470772, 1485745, 1355663, 1254043, 1334353, 1378878, 1569472, 1275098, 1435976, 1037072, 1376818, 1336718, 1227668, 941205, 1159256, 1505690, 1280455, 1505689, 1389506, 1448958, 1292208, 1505688, 1087768, 1237582, 1204594, 1505687, 1505683, 1275632, 891046, 1417739, 1090277, 1340175, 1054566, 1446233, 1059386, 1273248, 1270725, 1344034, 1294885, 1393365, 1312991, 1296548, 1187980, 1325075, 1295700, 1487440, 1458531, 1402523, 1408165, 1139252, 1154947, 1565461, 1461252, 1586272, 1510082, 1510154 };
                                    //List<Int64> compdata = new List<Int64>() { 1, Convert.ToInt64(examstatus.RegistrationNumber) };
                                    //List<Int64> stser = regListdata.Intersect(compdata).ToList();
                                    //if (stser.Count > 1 && examstatus.nsqfRollNo != "Y")
                                    //{
                                    //    linotexamEdit.Visible = true;
                                    //    lnknotexamEdit.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmExamForm_Edit.aspx?CourseID=" + examstatus.courseid + "&RegNo=" + examstatus.RegistrationNumber + "&examStartDate=" + examstartdate);
                                    //}
                                    linotexamfound.Visible = true;
                                    linotexamfound.InnerText = "No notification.";
                                }
                            }
                            else
                            {
                                var cutdates = from r in context.CutOffDates
                                               where r.ExamID == nextExam.ID && r.ApplicantTypeID == latestcourse.apptypeid && r.CourseID == latestcourse.courseid
                                               select new
                                               {
                                                   activity = r.ActivityID,
                                                   date = r.EfferctiveDate,
                                                   examname = r.Exam.Name,
                                                   coursename = r.Course.Name
                                               };
                                var examTimeTable = (from r in context.ExamTimeTables
                                                     join ex in context.Exams on r.ExamID equals ex.ID
                                                     where r.ExamID == nextExam.ID && r.CourseID == latestcourse.courseid
                                                     select new
                                                     {
                                                         examname = r.Exam.Name,
                                                         coursename = r.Course.Name,
                                                         dateofpublishing = ex.DateOfPublishingOfTimeTable,
                                                         courseid = r.CourseID,
                                                         examid = r.ExamID
                                                     }).FirstOrDefault();

                                var examstatus = (from r in context.CourseExamApplications
                                                  where r.ExamID == nextExam.ID && r.CandidateID == entityID && r.FinalSubmitted == true
                                                  select new
                                                  {
                                                      status = r.ApplicationStatusID,
                                                      verifiedon = r.DateOfVerificationByInstitute,
                                                      Demandid = r.DemandNoteID.HasValue ? r.DemandNoteID.Value : 0,
                                                      appno = r.ID,
                                                      RegistrationNumber = r.RegistrationNumber,
                                                      examname = r.Exam.Name,
                                                      coursename = r.Course.Name,
                                                      dateofdownloadadmitcard = r.Exam.DateOfPublishingOfRollNumber,
                                                      dateofresultdeclaration = r.Exam.DateOfPublishingOfResult,
                                                      courseid = r.CourseID,
                                                      rollno = r.RollNumber,
                                                      latefee = r.LateFeeImposed,
                                                      paymentsourceid = r.PaymentSourceID,
                                                      examid = r.ExamID,
                                                      applicanttypeID = r.ApplicantTypeID,
                                                      applicationstatusID = r.ApplicationStatusID,
                                                      paymentsourcechanged = r.PaymentSourceChanged,
                                                      paymentsourcechangeddate = r.PaymentSourceChangedOn,
                                                      candidateid = r.CandidateID,
                                                      paymentstatus = r.PaymentStatusID,
                                                      nsqfRollNo=r.NSQFRollNo
                                                  }).FirstOrDefault();

                                //Displaying only active payment modes
                                var paymentTypes = (from p in context.PaymentModes
                                                    where p.Visible == true
                                                    orderby p.Name
                                                    select p).ToList();

                                foreach (var names in paymentTypes)
                                {
                                    Paymentname.Append(names.Name + ",");
                                }
                                if (examstatus != null)
                                {
                                    divcourse.Visible = true;
                                    liexam.Visible = true;
                                    if (examstatus.dateofdownloadadmitcard.HasValue)
                                    {
                                        divcourse.Visible = true;
                                        lidownloadadmitcard.Visible = true;
                                        Lbdownload.Text = examTimeTable.examname + "(" + GetInitCap(examTimeTable.coursename) + "):-<b>Admit Card.</b>";
                                        // Lbdownload.Text = "Due to Covid - 19 Pendemic, it is recommended that candidate should  download Admit Card from https://student.nielit.gov.in at  Download Admit Card section after thouroghly reading declaration.";
                                        // Lnkdownload.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/CourseAdmitCard_Ver4.aspx?id=" + examstatus.courseid.ToString() + "&Appid=" + examstatus.appno) + "');");
                                    }
                                    if (examstatus.dateofresultdeclaration.HasValue)
                                    {
                                        divcourse.Visible = true;
                                        liviewresult.Visible = true;
                                        Lbresult.Text = examTimeTable.examname + " (" + GetInitCap(examTimeTable.coursename) + "):-<b>Result declared.</b>";
                                        Lnkresult.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./WEB/ViewCourseResult.aspx?id=" + examstatus.courseid.ToString() + "&RegNo=" + examstatus.RegistrationNumber.ToString() + "&ExamId=" + examstatus.examid) + "');");
                                    }
                                    enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)examstatus.status;
                                    if (applStatus == enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                                    {
                                        lbstatus.Text = examstatus.examname + " (" + GetInitCap(examstatus.coursename) + " " + ")" + "<b>Current Status:-</b> " + EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                        lbstatus.Text += " On :" + examstatus.verifiedon.Value.ToString("dd-MMM-yyyy");
                                        LnkBtnPrintForm.Visible = true;
                                        LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/ExamFormPreview.aspx?Appid=" + examstatus.appno.ToString()) + "');");
                                    }
                                    else if (applStatus == enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate && examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Candidate))
                                    {
                                        DateTime latefeedate = DateTime.MinValue;
                                        DateTime normalfeedate = DateTime.MinValue;
                                        lbstatus.Text = examstatus.examname + " (" + GetInitCap(examstatus.coursename) + " " + ") " + "<b>Current Status:-</b> " + EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                        LnkBtnPrintForm.Visible = true;
                                        LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/ExamFormPreview.aspx?Appid=" + examstatus.appno.ToString()) + "');");
                                        Lblexam.Visible = true;
                                        if (cutdates != null)
                                        {
                                            foreach (var ct in cutdates)
                                            {
                                                enmActivity activitystatus = (enmActivity)ct.activity;
                                                if (activitystatus == enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm)
                                                {
                                                    normalfeedate = ct.date;
                                                }
                                                if (activitystatus == enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee)
                                                {
                                                    latefeedate = ct.date;
                                                }
                                            }
                                        }
                                        if (examstatus.latefee == true)
                                        {
                                            if (DateTime.Today.Date <= latefeedate)
                                            {
                                                Lblexam.InnerHtml = "Note:-Please pay your fee either through <b> " + Paymentname.ToString().TrimEnd(',') + " </b> by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseExamApplication) + "&Appid=" + examstatus.appno.ToString() + "&DemandID=" + examstatus.Demandid.ToString()) + "'>Pay Fee </a>";
                                                TimeSpan datediff = latefeedate.Subtract(DateTime.Today);
                                                int days = datediff.Days;
                                                if (days <= 3)
                                                {
                                                    Lblfeenotification.Visible = true;
                                                    if (days > 1)
                                                        Lblfeenotification.Text = "<i style='color:red'>Only less than " + days.ToString() + " day(s) left to pay your examination fee. Please pay your examination fee before " + latefeedate.ToString("dd-MMM-yyyy");
                                                    else
                                                        Lblfeenotification.Text = "<i style='color:red'>Today is last date to pay your examination fee. Please pay your examination fee today.";
                                                }
                                            }
                                            else
                                            {
                                                linopaymentfee.Visible = true;
                                                linopaymentfee.InnerHtml = "<i style='color:red'>We have not received your examination fee. Your examination form/demand note has been expired on " + latefeedate.ToString("dd-MMM-yyyy");
                                            }
                                        }
                                        else if (examstatus.latefee == false)
                                        {
                                            if (DateTime.Today <= normalfeedate)
                                            {
                                                Lblexam.InnerHtml = "Note:-Please pay your fee either through <b> " + Paymentname.ToString().TrimEnd(',') + " </b> by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseExamApplication) + "&Appid=" + examstatus.appno.ToString() + "&DemandID=" + examstatus.Demandid.ToString()) + "'>Pay Fee </a>";
                                                TimeSpan datediff = normalfeedate.Subtract(DateTime.Today);
                                                int days = datediff.Days;
                                                if (days <= 3)
                                                {
                                                    Lblfeenotification.Visible = true;
                                                    Lblfeenotification.Text = "<i style='color:red'>Only less than 3 Days are left to deposit your fee.If you will not deposit the fee within the 3 Days your registration will be cancelled.";
                                                }
                                            }
                                            else
                                            {
                                                linopaymentfee.Visible = true;
                                                linopaymentfee.InnerHtml = "<i style='color:red'>Your time to pay the fee has expired.So you cannot apply for the exam.";
                                            }
                                        }
                                    }
                                    else if (applStatus == enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate && examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Institute))
                                    {
                                        enmCourseExamApplicationStatus appstatus = enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification;
                                        lbstatus.Text = examstatus.examname + " (" + GetInitCap(examstatus.coursename) + " " + ") " + "<b>Current Status:-</b> " + EConnect.Utils.Common.EnumUtility.GetDescription(appstatus);
                                        LnkBtnPrintForm.Visible = true;
                                        LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/ExamFormPreview.aspx?Appid=" + examstatus.appno.ToString()) + "');");
                                    }
                                    else
                                    {
                                        lbstatus.Text = examstatus.examname + " (" + GetInitCap(examstatus.coursename) + " " + ") " + "<b>Current Status:-</b> " + EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                        LnkBtnPrintForm.Visible = true;
                                        LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/ExamFormPreview.aspx?Appid=" + examstatus.appno.ToString()) + "');");
                                    }
                                    
                                    // code to change payment option
                                    if (examstatus.applicanttypeID == Convert.ToInt32(enmApplicantType.Institute) && (examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && examstatus.paymentstatus == Convert.ToInt32(enmPaymentStatus.Pending))
                                    {
                                        Lbpaymentsource.Visible = true;
                                        Lnkpaymentsourcechange.Visible = true;
                                        Lbappsource.Visible = true;
                                        Lnkappchange.Visible = true;
                                        if (examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Candidate))
                                        {
                                            if (CommonFunctions.IsDemandNoteCancellable(examstatus.Demandid))
                                            {
                                                Lbpaymentsource.Text = "Your applicable examination fee will be paid by you. Click on below link if you want to  change the payment option from candidate to institute. <br/>";
                                                Lnkpaymentsourcechange.CommandArgument = "CourseID=" + examstatus.courseid.ToString() + "&ExamId=" + examstatus.examid.ToString() + "&CandidateID=" + examstatus.candidateid.ToString();
                                                if (examstatus.paymentsourcechanged.ToString() == "True" && examstatus.paymentsourcechangeddate.HasValue == true)
                                                {
                                                    lbstatus.Text += "<br/>(Payment option changed from institute to candidate on " + examstatus.paymentsourcechangeddate.Value.ToString("dd-MMM-yyyy") + " )";
                                                    Lbpaymentsource.Visible = false;
                                                    Lnkpaymentsourcechange.Visible = false;
                                                }
                                                Lbappsource.Text = "<br/>If you want to cancel or edit your application please click on below link <br/>";
                                                Lnkappchange.CommandArgument = "CourseID=" + examstatus.courseid.ToString() + "&ExamId=" + examstatus.examid.ToString() + "&CandidateID=" + examstatus.candidateid.ToString();
                                            }
                                            else
                                            {
                                                Lbpaymentsource.Visible = false;
                                                Lnkpaymentsourcechange.Visible = false;
                                                Lbappsource.Visible = false;
                                                Lnkappchange.Visible = false;
                                            }
                                        }
                                        else if (examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Institute))
                                        {
                                            Lbpaymentsource.Text = " Your applicable examination fee will be paid by the accredited institute. Click on below link if you want to change the payment option from institute to candidate. <br/>";
                                            Lnkpaymentsourcechange.CommandArgument = "CourseID=" + examstatus.courseid.ToString() + "&ExamId=" + examstatus.examid.ToString() + "&CandidateID=" + examstatus.candidateid.ToString();
                                            if (examstatus.paymentsourcechanged.ToString() == "True" && examstatus.paymentsourcechangeddate.HasValue == true)
                                            {
                                                lbstatus.Text += "<br/>(Payment option changed from candidate to institute on " + examstatus.paymentsourcechangeddate.Value.ToString("dd-MMM-yyyy") + " )";
                                                Lbpaymentsource.Visible = false;
                                                Lnkpaymentsourcechange.Visible = false;
                                            }
                                            Lbappsource.Text = "<br/>If you want to cancel or edit your application please click on below link <br/>";
                                            Lnkappchange.CommandArgument = "CourseID=" + examstatus.courseid.ToString() + "&ExamId=" + examstatus.examid.ToString() + "&CandidateID=" + examstatus.candidateid.ToString();
                                        }
                                    }
                                    else if (examstatus.applicanttypeID == Convert.ToInt32(enmApplicantType.Direct) && examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && CommonFunctions.IsDemandNoteCancellable(examstatus.Demandid))
                                    {
                                        Lbpaymentsource.Visible = false;
                                        Lnkpaymentsourcechange.Visible = false;
                                        Lbappsource.Visible = true;
                                        Lnkappchange.Visible = true;
                                        Lbappsource.Text = "If you want to cancel or edit your application please click on below link <br/>";
                                        Lnkappchange.CommandArgument = "CourseID=" + examstatus.courseid.ToString() + "&ExamId=" + examstatus.examid.ToString() + "&CandidateID=" + examstatus.candidateid.ToString();
                                    }
                                }
                                if (examstatus == null)
                                {
                                    divcourse.Visible = true;
                                    Int32 registeredid = Convert.ToInt32(enmRegistrationStatus.Registered);
                                    Int32 reregisteredid = Convert.ToInt32(enmRegistrationStatus.ReRegistered);
                                    //string test=nextExam.ExamStartDate.AddDays(-(double)(nextExam.ExamStartDate.Day - 1)).ToString();
                                    //ShowAlert(test + " - " + latestcourse.CommencementFromDate);
                                    if ((latestcourse.regstatusid == registeredid || latestcourse.regstatusid == reregisteredid) && (latestcourse.validuptodate > DateTime.Today)
                                        //Added Due to restrict July-22 CommencementFromDate candidate To apply in Jan-22 exam
                                        //&& latestcourse.CommencementFromDate < DateTime.Today )
                                        // dateadd(day,((-(datepart(day,Exam_Start_Date)))+1),Exam_Start_Date ) 
                                         && latestcourse.CommencementFromDate <= nextExam.ExamStartDate.AddDays(-(double)(nextExam.ExamStartDate.Day - 1)))
                                        //&& latestcourse.regno != 1248376  )
                                    {
                                        linotexam.Visible = true;
                                        lnknotexam.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmExamForm.aspx?CourseID=" + latestcourse.courseid + "&RegNo=" + latestcourse.regno + "&examStartDate=" + nextExam.ExamStartDate);
                                    }
                                }
                                if (cutdates != null)
                                {
                                    foreach (var cr in cutdates)
                                    {
                                        enmActivity activitystatus = (enmActivity)cr.activity;
                                        if (activitystatus == enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm)
                                        {
                                            licutoffdates1.Visible = true;
                                            licutoffdates1.InnerText = cr.examname + " (" + GetInitCap(cr.coursename) + " " + ")" + ":- " + EConnect.Utils.Common.EnumUtility.GetDescription(activitystatus) + ":-" + cr.date.ToString("dd-MMM-yyyy");
                                        }
                                        if (activitystatus == enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm)
                                        {
                                            licutoffdates2.Visible = true;
                                            licutoffdates2.InnerText = cr.examname + " (" + GetInitCap(cr.coursename) + " " + ")" + ":-  " + EConnect.Utils.Common.EnumUtility.GetDescription(activitystatus) + ":-" + cr.date.ToString("dd-MMM-yyyy");
                                        }
                                        if (activitystatus == enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee)
                                        {
                                            licutoffdates3.Visible = true;
                                            licutoffdates3.InnerText = cr.examname + " (" + GetInitCap(cr.coursename) + " " + ")" + ":-  " + EConnect.Utils.Common.EnumUtility.GetDescription(activitystatus) + ":-" + cr.date.ToString("dd-MMM-yyyy");
                                        }
                                    }
                                }
                                if (cutdates == null)
                                {
                                    divcourse.Visible = true;
                                    linocutoffdates.Visible = true;
                                    linocutoffdates.InnerText = "Cut-Off Dates for the  " + nextExam.Name + " exam not declared";
                                }
                                if (examTimeTable != null)
                                {
                                    DateTime maxexamdate = (from r in context.ExamTimeTables
                                                            where r.ExamID == nextExam.ID && r.CourseID == latestcourse.courseid
                                                            orderby r.ExamFomDate descending
                                                            select new
                                                            {
                                                                maxexamdate = r.ExamFomDate
                                                            }).FirstOrDefault().maxexamdate;
                                    divcourse.Visible = true;
                                    if (DateTime.Today >= examTimeTable.dateofpublishing && DateTime.Today <= maxexamdate)
                                    {
                                        liexamtimetable.Visible = true;
                                        Lbtimetable.Text = examTimeTable.examname + " (" + GetInitCap(examTimeTable.coursename) + "):- <b>Time Table has been declared</b>.";
                                        lnktimetable.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./Admin/ExamTimeTableReport.aspx?CourseId=" + examTimeTable.courseid.ToString() + "&ExamId=" + examTimeTable.examid.ToString()) + "');");
                                    }
                                    else
                                    {
                                        divcourse.Visible = true;
                                        linoexamtimetable.Visible = true;
                                        linoexamtimetable.InnerText = "Time Table for the " + nextExam.Name + " exam not declared.";
                                    }
                                }
                                if (examTimeTable == null)
                                {
                                    divcourse.Visible = true;
                                    linoexamtimetable.Visible = true;
                                    linoexamtimetable.InnerText = "Time Table for the " + nextExam.Name + " exam  not declared.";
                                }
                            }
                            //Registration Notification
                            var registered = (from r in context.CourseRegistrationApplications
                                              where r.CandidateID == entityID && r.FinalSubmitted == true
                                              orderby r.ApplicationDate descending
                                              select new
                                              {
                                                  status = r.ApplicationStatusID,
                                                  coursename = r.Course.Name,
                                                  applicationID = r.ID,
                                                  demandnoteID = r.DemandNoteID,
                                                  Batchitemid = r.BatchItemID.HasValue ? r.BatchItemID.Value : 0
                                              }).FirstOrDefault();                       

                            //To Display Registration ID Link
                            if (candidatePersonal.locked == true)
                            {
                                // added to disable apply module certificate visibility 
                                var RegIDstatus = (from r in context.RegistrationDetails
                                                   where r.CandidateID == entityID 
                                                   //&& r.RegistrationStatusID < 4
                                                   //orderby r.CourseID descending
					 orderby r.CommencementFromDate descending
                                                   select new { RegNo = r.RegistrationNo, CandidateID = r.CandidateID, CourseCatId = r.CourseCategoryID ,courseId = r.CourseID }).FirstOrDefault();

                                liPrintRegID.Visible = true;
                                lnkPrintRegID.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("IDcard.aspx?RegnNo=" + RegIDstatus.RegNo + "&CandidateID=" + RegIDstatus.CandidateID);


                                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                                DataTable myDt = new DataTable();
                                using (SqlConnection con = new SqlConnection(constr))
                                {
                                    try
                                    {
                                        using (SqlCommand cmd = new SqlCommand("checklevelclearfrom_NIELITDB", con))
                                        {
                                           
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@pRegNo", SqlDbType.Int);
                                            cmd.Parameters["@pRegNo"].Value = Convert.ToInt64(RegIDstatus.RegNo);

                                            cmd.Parameters.Add("@pcourseId", SqlDbType.SmallInt);
                                            cmd.Parameters["@pcourseId"].Value = Convert.ToInt64(RegIDstatus.courseId);

                                            cmd.Parameters.Add("@is_cleared", SqlDbType.Char, 500);
                                            cmd.Parameters["@is_cleared"].Direction = ParameterDirection.Output;

                                            con.Open();
                                            
                                            cmd.ExecuteNonQuery();
                                            Int16 isCleared;
                                            isCleared = Convert.ToInt16(cmd.Parameters["@is_cleared"].Value);

                                            con.Close();
                                           
                                            var cerficateCheck = (from r in context.RegistrationDetails
                                                                 where r.CandidateID == entityID
                                                                 && r.RegistrationStatusID < 4
                                                                 //orderby r.CourseID descending
							                                   orderby r.CommencementFromDate descending
                                                                 select new { RegNo = r.RegistrationNo, CandidateID = r.CandidateID, CourseCatId = r.CourseCategoryID }).FirstOrDefault();

                                            var guardianCheckCount = (from a in context.CourseRegistrationApplications
                                                                      where a.CandidateID == entityID && ( a.GuardianName != null && a.GuardianName.Trim().TrimEnd().TrimStart().Length > 0 )
                                                                      select new { guardianName = a.GuardianName }).Count();
                                            if (guardianCheckCount > 0)
                                            {

                                                var guardianCheck = (from a in context.CourseRegistrationApplications
                                                                     where a.CandidateID == entityID && a.GuardianName != null
                                                                     select new { guardianName = a.GuardianName }).FirstOrDefault();

                                                //}
                                                //int x = guardianCheck.guardianName.ToString().Length;
                                                // if (guardianCheck != null && x >= 1)                                           
                                                //if (string.IsNullOrEmpty(guardianCheck.guardianName) == false)
                                                //{

                                                    var affidavitCheck = (from cr in context.CourseRegistrationApplications
                                                                          where cr.CandidateID == entityID && cr.affidavitNo != null && cr.affidavitVerified
                                                                          select new { affadavitNo = cr.affidavitNo }).FirstOrDefault();
                                                    if (affidavitCheck != null)
                                                    {
                                                        if (isCleared == 0 || cerficateCheck != null)
                                                        {                                                            
                                                            if (RegIDstatus.CourseCatId == 1)
                                                            {
                                                                SideLink1.Items.Add(new SideLinkItem("Module-Wise Certificate", "CAND/ApplyModuleCertificate.aspx?RegnNo=" + RegIDstatus.RegNo + "&CandidateID=" + RegIDstatus.CandidateID, "images/Apply_Online.jpg"));
                                                                SideLink1.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                                                                SideLink1.Render();
                                                            }
                                                        }
                                                    }
                                               // }
                                            }
                                             if (guardianCheckCount == 0)
                                            {
                                                if (isCleared == 0 || cerficateCheck != null) 
                                                {
                                                    liPrintRegID.Visible = true;
                                                    lnkPrintRegID.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("IDcard.aspx?RegnNo=" + RegIDstatus.RegNo + "&CandidateID=" + RegIDstatus.CandidateID);

                                                    if (RegIDstatus.CourseCatId == 1)
                                                    {
                                                        SideLink1.Items.Add(new SideLinkItem("Module-Wise Certificate", "CAND/ApplyModuleCertificate.aspx?RegnNo=" + RegIDstatus.RegNo + "&CandidateID=" + RegIDstatus.CandidateID, "images/Apply_Online.jpg"));
                                                        SideLink1.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                                                        SideLink1.Render();
                                                    }
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
                                        con.Close();
                                    }
                                }                               
                            }
                            if (registered != null)
                            {
                                divregistration.Visible = true;
                                liregfound.Visible = true;
                                enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)registered.status;
                                lbtext.Text = " Status of " + GetInitCap(registered.coursename) + " Registration is :-" + EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                liPrintCourseReg.Visible = true;
                                lnkPrntCourseReg.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/FrmPreview.aspx?Appid=" + registered.applicationID) + "');");
                                if (applStatus == enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate)
                                {
                                    liPaymentNote.Visible = true;
                                    Lblnote.Visible = true;
                                    requesturl = Request.Url.ToString();
                                    var paymentTypesreg = (from p in context.PaymentModes
                                                           where p.Visible == true
                                                           orderby p.Name
                                                           select p).ToList();

                                    foreach (var names in paymentTypesreg)
                                    {
                                        Paymentname.Append(names.Name + ",");
                                    }
                                    Lblnote.Text = "Note:-Please pay your fee either through <b>" + Paymentname.ToString().TrimEnd(',') + " </b> by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseRegistrationApplication) + "&Appid=" + registered.applicationID.ToString() + "&DemandID=" + registered.demandnoteID.Value.ToString() + "&RU=" + requesturl) + "'>Pay Fee </a>";

                                }
                                else if (applStatus == enmCourseApplicationStatus.KeptInAbeyance && registered.Batchitemid != 0)
                                {
                                    liPaymentNote.Visible = true;
                                    Lblnote.Visible = true;
                                    var deficiencydetails = (from p in context.BatchItemDeficiencyDetails
                                                             join t in context.DeficiencyCodes on p.DeficiencyID equals t.ID
                                                             join c in context.BatchItems on p.BatchItemID equals c.ID
                                                             where p.BatchItemID == registered.Batchitemid && p.IsFullFilled == false
                                                             orderby p.DeficiencyID
                                                             select new
                                                             {
                                                                 deficiencydesc = c.KeptInAbeyanceReason,
                                                                 defficiencydetail = t.Description
                                                             }).ToList();
                                    if (deficiencydetails.Count() > 0)
                                    {
                                        foreach (var deficiency in deficiencydetails)
                                        {
                                            counter = counter + 1;
                                            deficiencyname.Append(counter + ". " + deficiency.defficiencydetail + "<br/>");
                                        }
                                        Lblnote.Text = "Note:-Your Application has been Kept In Abeyance due to the following deficiencies found in your application form :- <br/> " + deficiencyname.ToString() + " Remarks:- <br/>" + GetInitCap(deficiencydetails.FirstOrDefault().deficiencydesc.ToString()) + " <br/> Please fulfill the deficiencies found in your application and resend your application form back to the NIELIT alongwith the suitable documents in support of this within the 15 days , failing which your application will be rejected without prior notice.";
                                    }
                                }
                            }
                            if (registered == null)
                            {
                                divregistration.Visible = true;
                                linotregfound.Visible = true;
                                linotregfound.InnerText = "No notification.";
                            }

                           if (!CourseManager.IsCandidateDebarred(latestcourse.courseid, latestcourse.regno))
                            {
                                string msg = null;
                                var examcycle = (from ee in context.Exams
                                                join d in context.DebarredCandidates on ee.ID equals d.debarred_for_examcycle
                                                where d.registration_number == latestcourse.regno && d.course_id == latestcourse.courseid
                                                select new
                                                 {
                                                     exammonth = ee.ExamMonth,
                                                     examyear = ee.ExamYear,
                                                     examcount = d.debarred_upto_examcycle_count,
                                                     examcyclename = ee.Name
                                                 }).FirstOrDefault();

                                var course_name = (from c in context.Courses                                                
                                                    where  c.ID == latestcourse.courseid
                                                    select c.Name).FirstOrDefault();

                                int month = examcycle.exammonth;
                                int year = examcycle.examyear;
                                for (int i = 0; i < examcycle.examcount+1; i++)
                                {
                                    if (month > 12)
                                    {
                                        month = 1;
                                        year += 1;
                                    }
                                    
                                    int Quotient = month / 6;                                    
                                    if (Quotient == 0)
                                    {
                                        month+= 6;                                        
                                        msg = "January-" + year + ";";
                                    }
                                    else
                                    {                                        
                                        month += 6;
                                        //year += 1;
                                        msg = "July-" + year + ";";
                                    }
									if(i>0)
									{
                                    appendexamcycle.Append(msg);
									}
                                }

                                //var remarks_msg = (from d in context.DebarredCandidates 
                                //                 where d.registration_number == latestcourse.regno && d.course_id == latestcourse.courseid
                                //                 select d.course_id).FirstOrDefault();
                                linotexam.Style.Add("color","red");
                                linotexam.InnerText = "Due to adoption of unfair means by you in the examination of " + course_name + " - " + examcycle.examcyclename +
                                " session, your current examination has been cancelled and you have been debarred from examinations of NIELIT for " + examcycle.examcount +
                                " sessions i.e. (" + appendexamcycle.ToString() + ").";
                                
                                lnknotexam.Visible = false;
                            }
                            else
                            {
                                //context.Database.ExecuteSqlCommand("update tblDebarredCandidate set whether_active ='N' where registration_number = " + latestcourse.regno + " and course_id=" + latestcourse.courseid);

                                //December_2024
                                SqlParameter[] param1 = { new SqlParameter("@latestcourseRegNo", latestcourse.regno),
                                                            new SqlParameter("@latestcourseCourseId", latestcourse.courseid)
                                                            };
                                context.Database.ExecuteSqlCommand("update tblDebarredCandidate set whether_active ='N' where registration_number = @latestcourseRegNo and course_id=@latestcourseCourseId ", param1);
                                context.SaveChanges();
                                //linotexam.Visible = true;
                                //lnknotexam.Visible = true;
                            }
                             lnkprojectfee.PostBackUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmProjectForm.aspx?CourseID=" + latestcourse.courseid + "&RegNo=" + latestcourse.regno);
                            //scholarship notification
                            //var scholar = (from r in context.CandidateScholarships
                            //               where r.CandidateID == entityID
                            //               orderby r.Date descending
                            //               select new
                            //               {
                            //                   date = r.Date,
                            //                   AppID = r.ID
                            //               }).FirstOrDefault();
                            //if (scholar != null)
                            //{
                            //    divscholarship.Visible = true;
                            //    lischolar.Visible = true;
                            //    Lblscholarshipdate.Text = scholar.date.ToString("dd-MMM-yyyy");
                            //    lnkscholar.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("./CAND/ScholarshipPreview.aspx?Appid=" + scholar.AppID) + "');");
                            //}
                            //else
                            //{
                            //    divscholarship.Visible = false;
                            //    lischolar.Visible = false;
                            //}
                        };
                    }
                    #endregion-----------------------------------------------------------------------------
                    #region--HeadOffice----------------------------------------------
                    else if (loginUserType == UserType.HeadOffice)
                    {
                        divHO.Visible = true;
                        divDLC.Visible = true;
                        regCentre.Visible = false;
                        givInstitute.Visible = false;
                        Coursediv.Visible = false;
                        CourseExamdiv.Visible = false;
						divProject.Visible = false;
                        divProject.Visible = false;
                        divInstitute.Visible = false;
                        //BindNewDLCApplicationData();
                    }
                    #endregion-----------------------------------------------------------------------------
                    else
                    {

                    }
                }
            
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    }

    #region Click Event--
    protected void Lnkemail_Click(object sender, EventArgs e)
    {
        try
        {
            CommonFunctions.GenerateEmailOTP(UserType.Candidate, entityID);
            Response.Redirect("~/CAND/OTPprocess.aspx?verify=email");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Lnkmobno_Click(object sender, EventArgs e)
    {
        try
        {
            CommonFunctions.GenerateMobileOTP(UserType.Candidate, entityID);
            Response.Redirect("~/CAND/OTPprocess.aspx?verify=mobile");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Lnkphoto_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {


                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();
                candidate.TempPhoto = null;
                candidate.PhotoFileName = null;
                candidate.TempSignature = null;
                candidate.SignatureFileName = null;
                candidate.TempLeftThumb = null;
                candidate.LeftThumbFileName = null;
                context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            };
            Response.Redirect("~/CAND/EditUploadPhoto.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void lnkCourse_Click(object sender, EventArgs e)
    {
        Int32 applicantType = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
        Response.Redirect("Admin/acc_reg_info.aspx?ApplTypeID=" + applicantType);
    }
    protected void Lnkappchange_Click(object sender, EventArgs e)
    {
        Response.Redirect("CAND/frmexamhistory.aspx?" + ((LinkButton)sender).CommandArgument);
    }
    protected void lnkCourseExam_Click(object sender, EventArgs e)
    {
        Int32 applicantType = Convert.ToInt32(enmApplicationType.CourseExamApplication);
        Response.Redirect("Admin/acc_reg_info.aspx?ApplTypeID=" + applicantType);
    }
	    protected void lnkProjectExam_Click(object sender, EventArgs e)
    {
        Int32 applicantType = Convert.ToInt32(enmApplicationType.CourseExamApplication);
        Response.Redirect("Admin/acc_reg_info.aspx?ApplTypeID=" + applicantType);
    }
    protected void lnkCertificate_Click(object sender, EventArgs e)
    {
        Int32 applicantType = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
        Response.Redirect("Admin/acc_reg_info.aspx?ApplTypeID=" + applicantType);
    }
    protected void Lnkpaymentsourcechange_Click(object sender, EventArgs e)
    {
        Response.Redirect("CAND/frmexamhistory.aspx?" + ((LinkButton)sender).CommandArgument);
    }
    protected void LinkReusltSheetDownload_Click(object sender, EventArgs e)
    {

        string UserID;
        UserID = (Session["studentReg"]).ToString();
        try
        {
            Response.Redirect("~/CAND/ResultSheet_Download.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    #endregion

    #region PageIndexChange Event
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvRegCentre.PageIndex = PagingBar1.CurrentPageIndex;
            BindRegCentreData();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged2(Int32 NewPageIndex)
    {
        try
        {
            gvCourse.PageIndex = PagingBar2.CurrentPageIndex;
            BindCoursesData();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged3(Int32 NewPageIndex)
    {
        try
        {
            gvCourseExam.PageIndex = PagingBar3.CurrentPageIndex;
            BindCourseExamData();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged4(Int32 NewPageIndex)
    {
        try
        {
            gvCertificate.PageIndex = PagingBar4.CurrentPageIndex;
            BindCertificateExamData();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    #endregion

    #region Sorting Event
    protected void gvRegCentre_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindRegCentreData();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvCourse_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvCertificate_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvCourseExam_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
	 protected void gvCourseProject_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    #endregion

    #region RowDataBound Event
    protected void gvCourseExam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute).ToString());

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification).ToString());

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString());

                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString() + "," + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl6 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT).ToString());

                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
	 protected void gvCourseProject_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute).ToString());

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification).ToString());

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString());

                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString() + "," + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl6 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT).ToString());

                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCertificate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                //HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                //hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationRejectedbyInstitute).ToString());

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationRejectedbyInstitute).ToString());

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification).ToString());

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre).ToString());

                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre).ToString());

                HyperLink hl6 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre).ToString());

                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvRegCentre_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCourse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl + Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute).ToString());

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification).ToString() + "," + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT).ToString());

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl + Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl + Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString());

                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl + Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT).ToString() + "," + Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByInstitute).ToString());

                HyperLink hl6 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl + Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT).ToString());

                // e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

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
    protected void DlcGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
        }
    }
    #endregion

    #region Private Methods
    protected void BindRegCentreData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                divInstitute.Visible = true;
                RegionalCenter regionalcentre = context.RegionalCenters.Find(entityID);
                Int32 sts = Convert.ToInt32(enmPaymentStatus.Pending);
                Int32 PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                Int32 Dispetched = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                Int32 Received = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                Int32 DDverifyPending = Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending);
                Int32 KeptInAby = Convert.ToInt32(enmCertificateExamApplicationStatus.KeptInAbeyance);
                Int32 VerifiedBYRegCen = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
                Int32 PaidCandidate = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                if (regionalcentre != null)
                {
                    var CertificateRegExam = (from s in context.CertificateExamApplications.AsNoTracking()
                                              where s.RegionalCenterID == entityID && s.ExamID != 0 &&  (s.Exam.ExamYear == DateTime.Now.Year || s.Exam.ExamYear == DateTime.Now.Year+1 || s.Exam.ExamYear == DateTime.Now.Year-1) 
                                              group s by new { s.CourseID, s.Course.Code, s.ExamID, s.Exam.Name, s.Exam.ExamYear } into c
                                              select new
                                              {
                                                  ID = c.Key.CourseID,
                                                  Name = c.Key.Code,
                                                  ExamName = c.Key.Name,
                                                  ExamId = c.Key.ExamID,
                                                  ExamYear = c.Key.ExamYear,
                                                  PBC = c.Count(p => p.ApplicationStatusID == PaidCandidate),
                                                  Dipetched = c.Count(p => p.ApplicationStatusID == Dispetched),
                                                  Received = c.Count(p => p.ApplicationStatusID == Received),
                                                  DDpending = c.Count(p => p.ApplicationStatusID == DDverifyPending),
                                                  KIA = c.Count(p => p.ApplicationStatusID == KeptInAby),
                                                  Verify = c.Count(p => p.ApplicationStatusID == VerifiedBYRegCen)
                                              });
                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.ExamYear).ThenByDescending(s => s.ExamId).ThenBy(s => s.ID);
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.ID);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.ID);
                                break;
                            case "Name":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.Name);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.Name);
                                break;
                            case "ExamName":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.ExamName);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.ExamName);
                                break;
                            case "PBC":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.PBC);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.PBC);
                                break;
                            case "Dipetched":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.Dipetched);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.Dipetched);
                                break;
                            case "Received":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.Received);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.Received);
                                break;
                            case "DDpending":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.DDpending);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.DDpending);
                                break;
                            case "KIA":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.KIA);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.KIA);
                                break;
                            case "Verify":
                                if (sortOrder == "DESC")
                                    CertificateRegExam = CertificateRegExam.OrderByDescending(s => s.Verify);
                                else
                                    CertificateRegExam = CertificateRegExam.OrderBy(s => s.Verify);
                                break;
                            default:
                                CertificateRegExam = CertificateRegExam.OrderBy(s => s.ExamYear);
                                break;
                        }
                    }

                    PagingBar1.Bind(CertificateRegExam, ref gvRegCentre);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (gvRegCentre.Rows.Count <= 0)
                    {
                        lnkCertificate.Visible = true;
                        lblCertificateMsg.Text = "Data Not Found";
                    }
                    else
                    {
                        lnkCertificate.Visible = false;
                        lblCertificateMsg.Text = "";
                    }
                }
                else
                {

                    lnkCertificate.Visible = false;
                    lblCertificateMsg.Text = "";
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindCoursesData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                divInstitute.Visible = true;
                Institute loginInsitute = context.Institutes.Find(entityID);
                Int32 sts = Convert.ToInt32(enmPaymentStatus.Pending);
                Int32 PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 paidButPending = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                Int32 AppliedButPendingForInstituteVerification = Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification);
                Int32 ApplicationVerifiedByInstitute = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByInstitute);
                // For Course Exam Applications
                Int32 ApplicationRejectedbyInstitutecoursereg = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute);
                //New Copied From Acc_Reg_Info
                Int32 FeePaidByInstituteButApplicationPendingToDispatchToNIELIT = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                Int32 ApplicationDispatchedByTheInstituteToNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                Int32 finalcourseregstatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                Int32 feedepositedbycandidatebutappliedthroughinstitute = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);

                if (loginInsitute != null)
                {
                    lblHeading.Text = "Welcome " + GetInitCap(loginInsitute.Name);
                    //---------------------------------------Course Registration----------------------------------------------
                    var CourseRegistration = (from s in context.CourseRegistrationApplications.AsNoTracking()
                                              where  s.ApplicantTypeID == applicantType && s.InstituteID == entityID && s.ApplicableExamID != 0 && s.ApplicationStatusID != finalcourseregstatus && (s.ApplicableExam.ExamYear== DateTime.Now.Year || s.ApplicableExam.ExamYear== DateTime.Now.Year+1 || s.ApplicableExam.ExamYear== DateTime.Now.Year -1)  // && (s.ApplicableExamID ==2675 || s.ApplicableExamID ==2677 || s.ApplicableExamID ==2679 || s.ApplicableExamID ==2681 )
                                              group s by new { s.CourseID, s.CourseCategoryID, s.Course.RegistrationServiceID, s.Course.Code, s.ApplicableExamID, s.ApplicableExam.Name, s.ApplicableExam.ExamYear, s.ApplicableExam.ExamMonth } into c
                                              select new
                                              {
                                                  ID = c.Key.CourseID,
                                                  CourseCategoryID = c.Key.CourseCategoryID,
                                                  ServiceID = c.Key.RegistrationServiceID,
                                                  Examid = c.Key.ApplicableExamID,
                                                  Examyear = c.Key.ExamYear,
                                                  Exammonth = c.Key.ExamMonth,
                                                  Name = c.Key.Code + " Level \n(" + c.Key.Name + ")",
                                                  AppTypeID = applicantType,
                                                  Pverification = c.Count(p => p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && (p.ApplicationStatusID == AppliedButPendingForInstituteVerification || p.ApplicationStatusID == feedepositedbycandidatebutappliedthroughinstitute)),
                                                  Prejected = c.Count(p => (p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && p.ApplicationStatusID == ApplicationRejectedbyInstitutecoursereg)),
                                                  PPstatus = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationVerifiedByInstitute && p.DemandNoteID == null),
                                                  PNotPaid = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToNIELIT && p.DemandNoteID != null),
                                                  PSubmit = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && (p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToNIELIT || p.ApplicationStatusID == ApplicationVerifiedByInstitute)),
                                                  PWaiting = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationDispatchedByTheInstituteToNIELIT),
                                                  ApplType = 1
                                              });
                    CourseRegistration = CourseRegistration.OrderByDescending(s => s.Examyear).ThenByDescending(s => s.Exammonth);
                    if (CourseRegistration.Count() == 0)
                    {
                        lnkCourse.Visible = false;
                        lblCourseMsg.Text = "Data Not Found";
                        PagingBar2.Visible = false;
                    }
                    else
                    {
                        PagingBar2.Visible = true;
                        lnkCourse.Visible = true;
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        CourseRegistration = CourseRegistration.OrderByDescending(s => s.ID);
                                    else
                                        CourseRegistration = CourseRegistration.OrderBy(s => s.ID);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        CourseRegistration = CourseRegistration.OrderByDescending(s => s.Name);
                                    else
                                        CourseRegistration = CourseRegistration.OrderBy(s => s.Name);
                                    break;
                                case "Pverification":
                                    if (sortOrder == "DESC")
                                        CourseRegistration = CourseRegistration.OrderByDescending(s => s.Pverification);
                                    else
                                        CourseRegistration = CourseRegistration.OrderBy(s => s.Pverification);
                                    break;
                                case "PPstatus":
                                    if (sortOrder == "DESC")
                                        CourseRegistration = CourseRegistration.OrderByDescending(s => s.PPstatus);
                                    else
                                        CourseRegistration = CourseRegistration.OrderBy(s => s.PPstatus);
                                    break;
                                case "PSubmit":
                                    if (sortOrder == "DESC")
                                        CourseRegistration = CourseRegistration.OrderByDescending(s => s.PSubmit);
                                    else
                                        CourseRegistration = CourseRegistration.OrderBy(s => s.PSubmit);
                                    break;

                                case "PWaiting":
                                    if (sortOrder == "DESC")
                                        CourseRegistration = CourseRegistration.OrderByDescending(s => s.PWaiting);
                                    else
                                        CourseRegistration = CourseRegistration.OrderBy(s => s.PWaiting);
                                    break;

                                default:
                                    CourseRegistration = CourseRegistration.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar2.Bind(CourseRegistration, ref gvCourse);
                        UpdatePanelcoursereg.Update();
                        UpdatePanelcoursereg1.Update();
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindCourseExamData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                divInstitute.Visible = true;
                Institute loginInsitute = context.Institutes.Find(entityID);
                Int32 sts = Convert.ToInt32(enmPaymentStatus.Pending);
                Int32 ApplTypeCou = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                Int32 ApplTypeCer = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                Int32 PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 paidButPending = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                // For Course Exam Applications
                Int32 ApplTypeCourseExam = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                Int32 AppliedButPendingForInstituteVerificationForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                Int32 FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                Int32 ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                Int32 ApplicationVerifiedByInstituteForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute);
                Int32 ApplicationRejectedbyInstitutecourseexam = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute);
                //New Copied From Acc_Reg_Info
                Int32 finalcourseexamstatus = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);

                if (loginInsitute != null)
                {
                    var CourseExam = (from s in context.CourseExamApplications.AsNoTracking()
                                      where s.ApplicantTypeID == applicantType && s.InstituteID == entityID && s.ExamID != 0 && s.ApplicationStatusID != finalcourseexamstatus && (s.Exam.ExamYear == DateTime.Now.Year || s.Exam.ExamYear == DateTime.Now.Year+1 || s.Exam.ExamYear == DateTime.Now.Year -1)
                                      group s by new { s.CourseID, s.CourseCategoryID, s.Course.ExaminationServiceID, s.Course.Code, s.ExamID, s.Exam.Name, s.Exam.ExamYear, s.Exam.ExamMonth } into c
                                      select new
                                      {
                                          ID = c.Key.CourseID,
                                          CourseCategoryID = c.Key.CourseCategoryID,
                                          ServiceID = c.Key.ExaminationServiceID,
                                          Examid = c.Key.ExamID,
                                          Examyear = c.Key.ExamYear,
                                          Exammonth = c.Key.ExamMonth,
                                          Name = c.Key.Code + " Level \n(" + c.Key.Name + ")",
                                          AppTypeID = applicantType,
                                          Pverification = c.Count(p => (p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && p.ApplicationStatusID == AppliedButPendingForInstituteVerificationForCourseExamApplication)),
                                          Prejected = c.Count(p => (p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && p.ApplicationStatusID == ApplicationRejectedbyInstitutecourseexam)),
                                          PPstatus = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationVerifiedByInstituteForCourseExamApplication && p.DemandNoteID == null),
                                          PNotPaid = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication && p.DemandNoteID != null),
                                          PSubmit = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && (p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication || p.ApplicationStatusID == ApplicationVerifiedByInstituteForCourseExamApplication)),
                                          PWaiting = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplication),
                                          ApplType = ApplTypeCourseExam
                                      });
                    CourseExam = CourseExam.OrderByDescending(s => s.Examyear).ThenByDescending(s => s.Exammonth);
                    if (CourseExam.Count() == 0)
                    {
                        lnkCourseExam.Visible = false;
                        lblCourseExam.Text = "Data Not Found";
                        PagingBar3.Visible = false;
                    }
                    else
                    {
                        PagingBar3.Visible = true;
                        lnkCourseExam.Visible = true;
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.ID);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.ID);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.Name);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.Name);
                                    break;
                                case "Pverification":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.Pverification);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.Pverification);
                                    break;
                                case "Prejected":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.Prejected);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.Prejected);
                                    break;
                                case "PPstatus":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.PPstatus);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.PPstatus);
                                    break;
                                case "PSubmit":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.PSubmit);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.PSubmit);
                                    break;

                                case "PWaiting":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.PWaiting);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.PWaiting);
                                    break;
                                case "PNotPaid":
                                    if (sortOrder == "DESC")
                                        CourseExam = CourseExam.OrderByDescending(s => s.PNotPaid);
                                    else
                                        CourseExam = CourseExam.OrderBy(s => s.PNotPaid);
                                    break;
                                default:
                                    CourseExam = CourseExam.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar3.Bind(CourseExam, ref gvCourseExam);
                        UpdatePanelcourseexam.Update();
                        UpdatePanelcourseexam1.Update();
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
	    protected void BindProjectData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                divInstitute.Visible = true;
                Institute loginInsitute = context.Institutes.Find(entityID);
                Int32 sts = Convert.ToInt32(enmPaymentStatus.Pending);
                Int32 ApplTypeCou = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                Int32 ApplTypeCer = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                Int32 PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 paidButPending = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                // For Project Exam Applications
                Int32 ApplTypeProjectExam = Convert.ToInt32(enmApplicationType.CourseProjectApplication);
                Int32 AppliedButPendingForInstituteVerificationForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                Int32 FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                Int32 ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                Int32 ApplicationVerifiedByInstituteForCourseExamApplication = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute);
                Int32 ApplicationRejectedbyInstitutecourseexam = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute);
                //New Copied From Acc_Reg_Info
                Int32 finalcourseexamstatus = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);

                if (loginInsitute != null)
                {
                    var CourseProject = (from s in context.CourseProjectApplications.AsNoTracking()
                                      where s.ApplicantTypeID == applicantType && s.InstituteID == entityID && s.ExamID != 0 && s.ApplicationStatusID != finalcourseexamstatus && (s.Exam.ExamYear == DateTime.Now.Year || s.Exam.ExamYear == DateTime.Now.Year + 1 || s.Exam.ExamYear == DateTime.Now.Year - 1)
                                         //where s.ApplicantTypeID == applicantType && s.InstituteID == entityID && s.ApplicationStatusID != finalcourseexamstatus// && (s.Exam.ExamYear == DateTime.Now.Year || s.Exam.ExamYear == DateTime.Now.Year + 1 || s.Exam.ExamYear == DateTime.Now.Year - 1)
                                      group s by new { s.CourseID, s.CourseCategoryID, s.Course.ProjectServiceID, s.Course.Code ,s.ExamID,s.Exam.ExamYear, s.Exam.ExamMonth  } into c
                                      select new
                                      {
                                          ID = c.Key.CourseID,
                                          CourseCategoryID = c.Key.CourseCategoryID,
                                          ServiceID = c.Key.ProjectServiceID,
                                          Examid = c.Key.ExamID,
                                          Examyear = c.Key.ExamYear,
                                          Exammonth = c.Key.ExamMonth,                                         
                                          Name = c.Key.Code + " Level \n(" + c.Key.Code + ")",
                                          AppTypeID = applicantType,
                                          Pverification = c.Count(p => (p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && p.ApplicationStatusID == AppliedButPendingForInstituteVerificationForCourseExamApplication)),
                                          Prejected = c.Count(p => (p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && p.ApplicationStatusID == ApplicationRejectedbyInstitutecourseexam)),
                                          PPstatus = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationVerifiedByInstituteForCourseExamApplication && p.DemandNoteID == null),
                                          PNotPaid = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication && p.DemandNoteID != null),
                                          PSubmit = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && (p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToNIELITForCourseExamApplication || p.ApplicationStatusID == ApplicationVerifiedByInstituteForCourseExamApplication)),
                                          PWaiting = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationDispatchedByTheInstituteToNIELITForCourseExamApplication),
                                          ApplType = ApplTypeProjectExam
                                      });
                   // CourseExam = CourseExam.OrderByDescending(s => s.Examyear).ThenByDescending(s => s.Exammonth);
                    CourseProject = CourseProject.OrderByDescending(s => s.Examyear).ThenByDescending(s => s.Exammonth);
                    if (CourseProject.Count() == 0)
                    {
                        lnkCourseExam.Visible = false;
                        lblCourseProject.Text = "Data Not Found";
                        PagingBar3.Visible = false;
                    }
                    else
                    {
                        PagingBar3.Visible = true;
                        lnkCourseExam.Visible = true;
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.ID);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.ID);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.Name);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.Name);
                                    break;
                                case "Pverification":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.Pverification);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.Pverification);
                                    break;
                                case "Prejected":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.Prejected);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.Prejected);
                                    break;
                                case "PPstatus":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.PPstatus);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.PPstatus);
                                    break;
                                case "PSubmit":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.PSubmit);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.PSubmit);
                                    break;

                                case "PWaiting":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.PWaiting);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.PWaiting);
                                    break;
                                case "PNotPaid":
                                    if (sortOrder == "DESC")
                                        CourseProject = CourseProject.OrderByDescending(s => s.PNotPaid);
                                    else
                                        CourseProject = CourseProject.OrderBy(s => s.PNotPaid);
                                    break;
                                default:
                                    CourseProject = CourseProject.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar3.Bind(CourseProject, ref gvCourseProject);
                        UpdatePanelcourseexam.Update();
                        UpdatePanelcourseexam1.Update();
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindCertificateExamData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                divInstitute.Visible = true;
                Institute loginInsitute = context.Institutes.Find(entityID);
                Int32 sts = Convert.ToInt32(enmPaymentStatus.Pending);
                Int32 ApplTypeCer = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                Int32 PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 paidButPending = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                Int32 applicantType = Convert.ToInt32(enmApplicantType.Institute);
                Int32 AppliedButPendingForInstituteVerification = Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification);
                Int32 FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
                Int32 ApplicationDispatchedByTheInstituteToRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                Int32 ApplicationVerifiedByInstitute = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute);
                Int32 ApplicationRejectedbyInstitutecertificateeexam = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationRejectedbyInstitute);
                Int32 certificatefinalstatus = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);

                if (loginInsitute != null)
                {
                    var CertificateExam = (from s in context.CertificateExamApplications.AsNoTracking()
                                           where s.ApplicantTypeID == applicantType && s.InstituteID == entityID && s.ExamID != 0 && (s.Exam.ExamYear ==DateTime.Now.Year  || s.Exam.ExamYear ==DateTime.Now.Year-1 || s.Exam.ExamYear ==DateTime.Now.Year+1)
                                           && s.ApplicationStatusID != certificatefinalstatus
                                           group s by new { s.CourseID, s.CourseCategoryID, s.Course.ExaminationServiceID, s.Course.Code, s.ExamID, s.Exam.Name, s.Exam.ExamYear, s.Exam.ExamMonth } into c
                                           select new
                                           {
                                               ID = c.Key.CourseID,
                                               CourseCategoryID = c.Key.CourseCategoryID,
                                               ServiceID = c.Key.ExaminationServiceID,
                                               Examid = c.Key.ExamID,
                                               Examyear = c.Key.ExamYear,
                                               Exammonth = c.Key.ExamMonth,
                                               Name = c.Key.Code + "\n(" + c.Key.Name + ")",
                                               AppTypeID = applicantType,
                                               //PnotSubmitted = c.Count(p => p.FinalSubmitted == false),
                                               Pverification = c.Count(p => p.IsVerifiedByInstitute == false && p.FinalSubmitted == true && p.ApplicationStatusID == AppliedButPendingForInstituteVerification),
                                               Prejected = c.Count(p => (p.IsVerifiedByInstitute == false && p.ApplicationStatusID == ApplicationRejectedbyInstitutecertificateeexam)),
                                               PPstatus = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationVerifiedByInstitute && p.DemandNoteID == null),
                                               PNotPaid = c.Count(p => p.PaymentStatusID == sts && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre && p.DemandNoteID != null),
                                               PSubmit = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre),
                                               PWaiting = c.Count(p => (p.PaymentStatusID == PaymentPaid || p.PaymentStatusID == paidButPending) && p.IsVerifiedByInstitute == true && p.ApplicationStatusID == ApplicationDispatchedByTheInstituteToRegionalCentre),
                                               ApplType = ApplTypeCer,
                                               count = c.Count()
                                           });
                    CertificateExam = CertificateExam.OrderByDescending(s => s.Examyear).ThenByDescending(s => s.Exammonth);
                    if (CertificateExam.Count() == 0)
                    {
                        lnkCertificate.Visible = false;
                        lblCertificateMsg.Text = "Data Not Found";
                        PagingBar4.Visible = false;
                    }
                    else
                    {
                        PagingBar4.Visible = true;
                        lnkCertificate.Visible = true;
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "ID":
                                    if (sortOrder == "DESC")
                                        CertificateExam = CertificateExam.OrderByDescending(s => s.ID);
                                    else
                                        CertificateExam = CertificateExam.OrderBy(s => s.ID);
                                    break;
                                case "Name":
                                    if (sortOrder == "DESC")
                                        CertificateExam = CertificateExam.OrderByDescending(s => s.Name);
                                    else
                                        CertificateExam = CertificateExam.OrderBy(s => s.Name);
                                    break;
                                case "Pverification":
                                    if (sortOrder == "DESC")
                                        CertificateExam = CertificateExam.OrderByDescending(s => s.Pverification);
                                    else
                                        CertificateExam = CertificateExam.OrderBy(s => s.Pverification);
                                    break;
                                case "PPstatus":
                                    if (sortOrder == "DESC")
                                        CertificateExam = CertificateExam.OrderByDescending(s => s.PPstatus);
                                    else
                                        CertificateExam = CertificateExam.OrderBy(s => s.PPstatus);
                                    break;
                                case "PSubmit":
                                    if (sortOrder == "DESC")
                                        CertificateExam = CertificateExam.OrderByDescending(s => s.PSubmit);
                                    else
                                        CertificateExam = CertificateExam.OrderBy(s => s.PSubmit);
                                    break;

                                case "PWaiting":
                                    if (sortOrder == "DESC")
                                        CertificateExam = CertificateExam.OrderByDescending(s => s.PWaiting);
                                    else
                                        CertificateExam = CertificateExam.OrderBy(s => s.PWaiting);
                                    break;

                                default:
                                    CertificateExam = CertificateExam.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar4.Bind(CertificateExam, ref gvCertificate);
                        UpdatePanelcertificateexam.Update();
                        UpdatePanelcertificateexam1.Update();
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindNewDLCApplicationData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                const string format = "{0}  |  {1}  |  {2}";
                var courses = context.Courses.Where(s => (s.CourseCategory.Code == "ITL"))
                    .Select(s => new { CourseId = s.ID, ApplicantTypeId = (s.ApplicantTypeID != null) ? s.ApplicantTypeID.Value : 1 }).ToList();
                List<Int32> CourseIds = new List<Int32>();
                List<Int32> exams = new List<Int32>();
                foreach (var course in courses)
                {
                    CourseIds.Add(course.CourseId);
                    exams.Add(GetCurrentExam(course.CourseId, course.ApplicantTypeId));
                }
                var DlcNewApplicationData = context.CertificateExamApplications.AsNoTracking()
                                               .Where(s => exams.Contains(s.ExamID))
                                               .GroupBy(s => new { s.ExamID, s.Exam.Name, s.CourseID, s.Course.Code })
                                               .Select(s => new
                                               {
                                                   CourseId = s.Key.CourseID,
                                                   ExamId = s.Key.ExamID,
                                                   CourseName = s.Key.Code,
                                                   ExamName = s.Key.Name,
                                                   FinalSubmitted = s.Count(p => p.FinalSubmitted == true),
                                                   Direct = s.Count(p => p.FinalSubmitted == true && p.ApplicantTypeID == 1),                                                   
                                                   InstituteNotVerified = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == false),
                                                   InstituteVerified = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == true),
                                                   InstitutePaid = s.Count(p => p.ApplicantTypeID == 2 && p.FinalSubmitted == true && p.IsVerifiedByInstitute == true && p.PaymentStatusID > 1),                                                  
                                                   FeePaid = s.Count(p => p.FinalSubmitted == true && p.PaymentStatusID > 1),
                                                   Disability = s.Count(p => p.FinalSubmitted == true && p.IsDisability == true && p.PaymentStatusID > 1)
                                               }).ToList();
                
                if (DlcNewApplicationData.Count() > 0)
                {
                    var DisplayData = DlcNewApplicationData
                                      .Select(s => new
                                         {
                                             CourseId = s.CourseId,
                                             ExamId = s.ExamId,
                                             CourseName = s.CourseName,
                                             ExamName = s.ExamName,
                                             FinalSubmitted = s.FinalSubmitted,
                                             Direct = s.Direct,
                                             ViaInstitute = string.Format(format, s.InstituteNotVerified, s.InstituteVerified, s.InstitutePaid),
                                             FeePaid = s.FeePaid,
                                             Disability = s.Disability
                                         });
                    DisplayData = DisplayData.OrderBy(s => s.CourseName).ToList();;
                    DlcNewApplicationGrid.DataSource = DisplayData;
                    DlcNewApplicationGrid.DataBind();
                    UpdatePanel1.Update();
                }
            }
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected Int32 GetCurrentExam(Int32 courseID, Int32 ApplicantTypeId)
    {
        try
        {
            Int32 exams;

            using (EConnectContext context = new EConnectContext())
            {
                int StartDateofFormFilling = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
                var examsAll = (from f in context.Exams
                                where f.CourseID == courseID
                                && f.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && f.DateOfPublishingOfRollNumber == null
                                select f);

                examsAll = examsAll.Where(a => a.CutOffDates.Where(k => k.ActivityID == StartDateofFormFilling && k.ApplicantTypeID == ApplicantTypeId).FirstOrDefault().EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));

                if (examsAll.Count() > 0)
                {
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    var exmasWithNormalLastDate = (from t in context.CutOffDates
                                                   where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == ApplicantTypeId &&
                                                   t.ActivityID == NormalFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                   orderby t.Exam.ExamStartDate
                                                   select t.Exam).Distinct();
                    if (exmasWithNormalLastDate.Count() == 0)
                    {
                        var exmasWithLateFeeLastDate = (from t in context.CutOffDates
                                                        where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == ApplicantTypeId &&
                                                        t.ActivityID == LateFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                        orderby t.Exam.ExamStartDate
                                                        select t.Exam).Distinct();

                        exams = exmasWithLateFeeLastDate.Select(c => c.ID).FirstOrDefault();
                    }
                    else
                    {
                        exams = exmasWithNormalLastDate.Select(c => c.ID).FirstOrDefault();
                    }
                }
                else
                    exams = examsAll.Select(c => c.ID).FirstOrDefault();
            };
            return exams;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion



   
}