using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;


public partial class frmexamhistory : BasePage
{
    Int32 examID = 0;
    Int32 courseID = 0;
    UserType loginUserType;
    Int64 entityID;
    string examName1 ;
    string courseName;
    
   

    protected void Page_Load(object sender, EventArgs e)
    {
        examName1 = "NA";
        entityID = 0;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            examID = Convert.ToInt32(Request.QueryString["ExamId"]);
            loginUserType = (UserType)Session["UserType"];
            if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                entityID = Convert.ToInt64(Request.QueryString["CandidateID"]);
            else
                entityID = Convert.ToInt64(Session["EntityID"]);
            using (EConnectContext context = new EConnectContext())
            {
                courseName = context.Courses.Find(courseID).Name;
                Exam exam = context.Exams.Find(examID);
                if (exam != null)
                    examName1 = exam.Name;
                lblHeading.Text = "Exam Details: " + examName1 + " (" + courseName + ")";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Details: " + examName1, "CAND/frmexamhistory.aspx?" + Request.QueryString.ToString(), ""));
                BreadCrumb1.Render();
            };
            examDetails();
            BindGridView();
            if (loginUserType == UserType.Candidate)
            {
                showsidelink();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private DataTable GetData(Int64 currentCourseID, Int64 entityID, Int64 examID)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

       /* string sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
                    " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
                    " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
                    " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
                    " isnull(e.Name ,'NA') as doexam,case when (select Exam_Year  from exam where id = e.id ) <=2020 then rg.Description else case when exists ( select top 1 id from exam where (( Result_Publish_Date is not null and Result_Publish_Date <=getdate() and id=e.id  ) )) then rg.Description else '' end end  as Result  , case when (select Exam_Year  from exam where id =e.id   ) <=2020 then rg.Code else case when exists ( select top 1 id from exam where (( Result_Publish_Date is not null and Result_Publish_Date <=getdate() and id=e.id  ) )) then rg.Code else '' end end   as Grade  from " +
                    " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
                    " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
                    " and rg.Code <>'$'  and cad.Module_ID not in (933,934,935,936,956,957,958,959) and cad.Candidate_ID ='" + entityID + "' and cad.Course_ID ='" + currentCourseID + "' and cad.Exam_ID = '" + examID + "'  " +
                    " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";*/
					 
		//string sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
  //                   " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
  //                   " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
  //                   " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
  //                   " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
  //                   " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
  //                   " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
  //                   " and rg.Code <>'$'  and cad.Module_ID not in (933,934,935,936,956,957,958,959) and cad.Candidate_ID ='" + entityID + "' and cad.Course_ID ='" + currentCourseID + "' and cad.Exam_ID = '" + examID + "'  " +
  //                   " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";

        //November_2024
        string sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
                     " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
                     " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
                     " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
                     " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
                     " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
                     " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
                     " and rg.Code <>'$'  and cad.Module_ID not in (933,934,935,936,956,957,958,959) and cad.Candidate_ID =@entityID and cad.Course_ID =@currentCourseID and cad.Exam_ID =@examID " +
                     " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                //November_2024
                cmd.Parameters.AddWithValue("@entityID", entityID);
                cmd.Parameters.AddWithValue("@currentCourseID", currentCourseID);
                cmd.Parameters.AddWithValue("@examID", examID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }


    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                Int32 couID = Convert.ToInt32(Request.QueryString["CourseID"]);
                Int32 examID = Convert.ToInt32(Request.QueryString["ExamId"]);
                //var listOfAppearedModules = (from d in context.CourseExamApplicationDetails
                //                             join m in context.Modules on d.ModuleID equals m.ID
                //                             where d.CourseID == courseID && d.CandidateID == entityID
                //                             && d.ExamID == examID
                //                             orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                             select new
                //                             {
                //                                 CourseID = m.CourseID,
                //                                 ID = m.ID,
                //                                 name = m.Name,
                //                                 Code = m.ShortName,
                //                                 ModuleTypeID = m.ModuleTypeID,
                //                                 MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                //                                 Result = d.Grade.Description == null ? "NA" : d.Grade.Description,
                //                                 Grade = d.Grade.Code == null ? "NA" : d.Grade.Code
                //                             }).Distinct();
               // 
                var listOfPassedModules = GetData(couID, entityID, examID);
                gvMain.DataSource = listOfPassedModules;
                gvMain.DataBind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
                if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&CandidateID=" + Request.QueryString["CandidateID"]);
                else
                    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = hl.NavigateUrl;

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = hl.NavigateUrl;

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void showsidelink()
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = (from c in context.Courses
                                where c.ID == courseID && c.ShowOnWeb == true
                                select new
                                {
                                    courcatID = c.CourseCategoryID
                                }).FirstOrDefault().courcatID;
                var dl = from d in context.Downloadables
                         where d.CourseID == courseID && d.ShowOnWeb == true
                         select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };
                var d2 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == ccatId && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(dl);
                var d3 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == null && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(d2);
                foreach (var dnbl in d3.Distinct())
                {
                    Sidelink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                }
                Sidelink1.Render();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void examDetails()
    {
        try
        {

            StringBuilder Paymentname = new StringBuilder();
            using (EConnectContext context = new EConnectContext())
            {
                var examstatus = (from r in context.CourseExamApplications
                                  join d in context.Exams
                                      on r.ExamID equals d.ID
                                  where r.ExamID == examID && r.CandidateID == entityID && r.CourseID == courseID && r.FinalSubmitted == true
                                  select new
                                  {
                                      status = r.ApplicationStatus.Description,
                                      verifiedon = r.DateOfVerificationByInstitute,
                                      Demandid = r.DemandNoteID.HasValue ? r.DemandNoteID.Value : 0,
                                      appno = r.ID,
                                      formfillingdate = r.ApplicationDate,
                                      applicationnumber = r.Number,
                                      examname = r.Exam.Name,
                                      coursename = r.Course.Name,
                                      dateofdownloadadmitcard = r.Exam.DateOfPublishingOfRollNumber,
                                      dateofresultdeclaration = r.Exam.DateOfPublishingOfResult,
                                      dateoftimetabledeclaration = r.Exam.DateOfPublishingOfTimeTable,
                                      dateofpracticaladmitcard = r.Exam.DateofPublishingPracticalAdmitCard,
                                      nofpractmodule = r.NumberOfPracticalModulesApplied,
                                      courseid = r.CourseID,
                                      examid = r.ExamID,
                                      RegistrationNumber = r.RegistrationNumber,
                                      candidateid = r.CandidateID,
                                      rollno = r.RollNumber.HasValue ? r.RollNumber.Value : 0,
                                      pr_officerefno = r.PracticalOfficeRefNumber,
                                      latefee = r.LateFeeImposed,
                                      paymentsourceid = r.PaymentSourceID,
                                      paymentstatus = r.PaymentStatusID,
                                      applicanttypeID = r.ApplicantTypeID,
                                      applicationstatusID = r.ApplicationStatusID,
                                      paymentsourcechanged = r.PaymentSourceChanged,
                                      paymentsourcechangeddate = r.PaymentSourceChangedOn,
                                  }).FirstOrDefault();

                if (examstatus != null)
                {
                    Exam exam = CourseManager.GetNextExam(context, courseID, examstatus.applicanttypeID);
                    if (!string.IsNullOrEmpty(examstatus.status) && (!string.IsNullOrEmpty(examstatus.formfillingdate.ToString())))
                        tdexamformstatus.InnerHtml = "Application submitted by you on " + examstatus.formfillingdate.ToString("dd-MMM-yyyy") + " and your application-no is <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("ExamFormPreview.aspx?candidateID=" + examstatus.candidateid + "&Appid=" + examstatus.appno.ToString()) + "' target='_blank' >" + examstatus.applicationnumber + "</a>" + "<br/> and  your application status is " + examstatus.status;
                    else
                        tdexamformstatus.InnerText = "Not Available";
                    if (!string.IsNullOrEmpty(examstatus.paymentstatus.ToString()))
                    {
                        enmPaymentStatus paymentstatus = (enmPaymentStatus)examstatus.paymentstatus;
                        if (paymentstatus == enmPaymentStatus.Pending && examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Candidate))
                        {
                            var paymentTypes = (from p in context.PaymentModes
                                                where p.Visible == true
                                                orderby p.Name
                                                select p).ToList();

                            foreach (var names in paymentTypes)
                            {
                                Paymentname.Append(names.Name + ",");
                            }
                            tdpaymentstatus.InnerHtml = paymentstatus + "" + ".Please pay your fee either through <b>" + Paymentname.ToString().TrimEnd(',') + " </b> by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseExamApplication) + "&Appid=" + examstatus.appno.ToString() + "&DemandID=" + examstatus.Demandid.ToString()) + "'>Pay Fee </a>";
                        }
                        else if (paymentstatus == enmPaymentStatus.Pending && examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Institute))
                            tdpaymentstatus.InnerHtml = paymentstatus + "" + " and your fee will be deposited by the accredited institute";
                        else if (paymentstatus == enmPaymentStatus.Paid)
                        {
                            tdpaymentstatus.InnerHtml = paymentstatus.ToString();
                            DemandNote demandNote = context.DemandNotes.Find(examstatus.Demandid);
                            if (demandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name) + " ) " + "and your transaction No.is:- " + demandNote.CSCTransaction.ResponseTransactionNumber;
                            }
                            if (demandNote.enmPaymentMode == enmPaymentMode.DemandDraft)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name) + " ) " + "and your DD No.is:- " + demandNote.DemandDraftTransaction.DemandDraftNumber;
                            }
                            if (demandNote.enmPaymentMode == enmPaymentMode.Online)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name) + " ) " + "and your transaction No.is:- " + demandNote.OnlineTransaction.ReferenceNumber;
                            }
                            if (demandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name).ToUpper() + " ) " + "and your NEFT transaction No.is:- " + demandNote.NEFTTransaction.TransactionNumber;
                            }
                        }
                        else if (paymentstatus == enmPaymentStatus.PaidButNotVerified)
                        {
                            tdpaymentstatus.InnerHtml = paymentstatus.ToString();
                            DemandNote demandNote = context.DemandNotes.Find(examstatus.Demandid);
                            if (demandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name) + " ) " + "and your transaction No.is:- " + demandNote.CSCTransaction.ResponseTransactionNumber;
                            }
                            if (demandNote.enmPaymentMode == enmPaymentMode.DemandDraft)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name) + " ) " + "and your DD No.is:- " + demandNote.DemandDraftTransaction.DemandDraftNumber;
                            }
                            if (demandNote.enmPaymentMode == enmPaymentMode.Online)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name) + " ) " + "and your transaction No.is:- " + demandNote.OnlineTransaction.ReferenceNumber;
                            }
                            if (demandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                            {
                                tdpaymentstatus.InnerHtml += " ( " + GetInitCap(demandNote.PaymentMode.Name).ToUpper() + " ) " + "and your NEFT transaction No.is:- " + demandNote.NEFTTransaction.TransactionNumber;
                            }
                        }
                        else
                        {
                            tdpaymentstatus.InnerHtml = paymentstatus.ToString();
                        }
                    }
                    if (examstatus.dateofdownloadadmitcard.HasValue && examstatus.rollno != 0)
                    {
                       // tdadmitcard.InnerHtml = "Admit Card for the exam declared on " + examstatus.dateofdownloadadmitcard.Value.ToString("dd-MMM-yyyy") + " <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("CourseAdmitCard_Ver4.aspx?id=" + examstatus.courseid.ToString() + "&Appid=" + examstatus.appno.ToString()) + "' target='_blank'>Click here to view/Print the Admit Card</a>";
                        tdadmitcard.InnerHtml = "Due to Covid - 19 Pendemic, it is recommended that candidate should  download Admit Card from https://student.nielit.gov.in at  Download Admit Card section after thouroghly reading Declaration.";
                            //+ examstatus.dateofdownloadadmitcard.Value.ToString("dd-MMM-yyyy") + " <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("CourseAdmitCard_Ver4.aspx?id=" + examstatus.courseid.ToString() + "&Appid=" + examstatus.appno.ToString()) + "' target='_blank'>Click here to view/Print the Admit Card</a>";
                        //lbldownload.Visible = true;
                        //lbldownload.Text = "Due to Covid - 19 Pendemic, it is recommended that candidate should  download Admit Card from Front Page after reading decalaration.";
                    }
                    else
                    {
                        tdadmitcard.InnerHtml = "Not Available";
                    }
                    if (examstatus.dateofpracticaladmitcard.HasValue && examstatus.nofpractmodule != 0 && examstatus.pr_officerefno != null)
                    {
                       // tdpracticaladmitcard.InnerHtml = "Practical Examination Admit Card for the exam declared on " + examstatus.dateofpracticaladmitcard.Value.ToString("dd-MMM-yyyy") + " <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("CoursePracticalAdmitCard.aspx?id=" + examstatus.courseid.ToString() + "&Appid=" + examstatus.appno.ToString()) + "' target='_blank'>Click here to view/Print the Practical Examination Admit Card</a>";
                        tdpracticaladmitcard.InnerHtml = "Due to Covid - 19 Pendemic, it is recommended that candidate should  download Practical Admit Card from https://student.nielit.gov.in at  Download Admit Card section after thouroghly reading Declaration.";
                    }
                    else
                    {
                        tdpracticaladmitcard.InnerHtml = "Not Available / You have not applied for any practical exam.";
                    }
                    if (examstatus.dateofresultdeclaration.HasValue)
                    {
                        tdResult.InnerHtml = "Result for the exam declared on " + examstatus.dateofresultdeclaration.Value.ToString("dd-MMM-yyyy") + " <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../WEB/ViewCourseResult.aspx?id=" + examstatus.courseid.ToString() + "&RegNo=" + examstatus.RegistrationNumber.ToString() + "&ExamId=" + examstatus.examid.ToString()) + "' target='_blank'>Click here to view/Print the Result</a>";
                    }
                    else
                    {
                        tdResult.InnerHtml = "Not Available";
                    }
                    if (examstatus.dateoftimetabledeclaration.HasValue)
                    {
                        tdtimetable.InnerHtml = "Time-Table for the exam  declared on " + examstatus.dateoftimetabledeclaration.Value.ToString("dd-MMM-yyyy") + " <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../Admin/ExamTimeTableReport.aspx?CourseId=" + examstatus.courseid.ToString() + "&ExamId=" + examstatus.examid.ToString()) + "' target='_blank'>Click here to view/Print the Time-Table</a>";
                    }
                    else
                    {
                        tdtimetable.InnerHtml = "Not Available";
                    }
                    if (examstatus.applicanttypeID == Convert.ToInt32(enmApplicantType.Institute) && (examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && examstatus.paymentstatus == Convert.ToInt32(enmPaymentStatus.Pending))
                    {
                        trpayment.Visible = true;
                        if (exam != null)
                        {
                            if (exam.ID == examstatus.examid)
                            {
                                if (examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Candidate))
                                {
                                    if (CommonFunctions.IsDemandNoteCancellable(examstatus.Demandid))
                                    {
                                        Lbpaymentsource.Visible = true;
                                        Lnkpaymentsourcechange.Visible = true;

                                        Lbpaymentsource.Text = "Your applicable examination fee will be paid by you. Click on below link if you want to  change the payment option from candidate to institute.";
                                        lbfilter.Text = "Are you sure you want to change the payment option from candidate to institute ?<br/> Payment option once changed you cannot revert back the payment option again.";
                                        if (examstatus.paymentsourcechanged.ToString() == "True" && examstatus.paymentsourcechangeddate.HasValue == true)
                                        {
                                            tdpaymentstatus.InnerHtml += "<br/>(Payment option changed from institute to candidate on " + examstatus.paymentsourcechangeddate.Value.ToString("dd-MMM-yyyy") + " )";
                                            //trpayment.Visible = false;
                                            Lnkpaymentsourcechange.Visible = false;
                                            Lbpaymentsource.Visible = false;
                                        }

                                        Lnkappchange.Visible = true;
                                        Lbappchange.Visible = true;
                                        Lbappchange.Text = "If you want to cancel or edit your application please click on below link <br/>";
                                        lbfilter1.Text = "Are you sure you want to cancel or edit your application?";
                                    }
                                    else
                                    {
                                        Lbpaymentsource.Visible = false;
                                        Lnkpaymentsourcechange.Visible = false;
                                        Lnkappchange.Visible = false;
                                        Lbappchange.Visible = false;
                                    }
                                }
                                else if (examstatus.paymentsourceid == Convert.ToInt32(enmPaymentSource.Institute))
                                {
                                    Lbpaymentsource.Visible = true;
                                    Lnkpaymentsourcechange.Visible = true;
                                    Lbpaymentsource.Text = " Your applicable examination fee will be paid by the accredited institute. Click on below link if you want to change the payment option from institute to candidate.";
                                    lbfilter.Text = "Are you sure you want to change the payment option from institute to candidate?<br/> Payment option once changed you cannot revert back the payment option again.";
                                    if (examstatus.paymentsourcechanged.ToString() == "True" && examstatus.paymentsourcechangeddate.HasValue == true)
                                    {
                                        tdpaymentstatus.InnerHtml += "<br/>(Payment option changed from candidate to institute on " + examstatus.paymentsourcechangeddate.Value.ToString("dd-MMM-yyyy") + " )";
                                        //trpayment.Visible = false;
                                        Lnkpaymentsourcechange.Visible = false;
                                        Lbpaymentsource.Visible = false;
                                    }

                                    Lnkappchange.Visible = true;
                                    Lbappchange.Visible = true;
                                    Lbappchange.Text = "If you want to cancel or edit your application please click on below link <br/>";
                                    lbfilter1.Text = "Are you sure you want to cancel or edit your application?";

                                }
                            }
                        }
                    }
                    else if (examstatus.applicanttypeID == Convert.ToInt32(enmApplicantType.Direct) && examstatus.applicationstatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && CommonFunctions.IsDemandNoteCancellable(examstatus.Demandid))
                    {
                        if (exam != null)
                            if (exam.ID == examstatus.examid)
                            {
                                trpayment.Visible = true;
                                Lnkappchange.Visible = true;
                                Lbappchange.Visible = true;
                                Lbappchange.Text = "If you want to cancel or edit your application please click on below link <br/>";
                                lbfilter1.Text = "Are you sure you want to cancel or edit your application?";
                            }
                        Lbpaymentsource.Visible = false;
                    }
                }
                else
                {
                    tdexamformstatus.InnerText = "Not Available";
                    tdadmitcard.InnerText = "Not Available";
                    tdpracticaladmitcard.InnerText = "Not Available";
                    tdpaymentstatus.InnerText = "Not Available";
                    tdResult.InnerText = "Not Available";
                    tdtimetable.InnerText = "Not Available";
                    trpayment.Visible = false;
                    //Show Result Status

                    if (context.CourseExamApplicationDetails.Any(c => (c.ExamID == examID && c.CandidateID == entityID && c.CourseID == courseID)))
                    {
                        Int64 RegistrationNumber = (from c in context.CourseExamApplicationDetails
                                                    where c.ExamID == examID && c.CandidateID == entityID && c.CourseID == courseID && c.ResultGradeID != null
                                                    select c.RegistrationNumber).FirstOrDefault();
                        if (RegistrationNumber != 0)
                            tdResult.InnerHtml = "Result available. <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../WEB/ViewCourseResult.aspx?id=" + courseID.ToString() + "&RegNo=" + RegistrationNumber.ToString() + "&ExamId=" + examID.ToString()) + "' target='_new'>Click here to view/Print the Result</a>";
                        else
                            tdtimetable.InnerText = "Not Available";
                    }
                    else
                        tdtimetable.InnerText = "Not Available";
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Lnkpaymentsourcechange_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divoutput.Visible = false;
            divradio.Visible = true;
            divradio1.Visible = false;
            divoutput1.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Btnno_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divradio.Visible = false;
            divradio1.Visible = false;
            divoutput1.Visible = false;
            divoutput.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            string Msg = "";
            string emailAddress = "";
            //Int64 demandID = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    CourseExamApplication cea = new CourseExamApplication();
                    cea = context.CourseExamApplications.Where(r => r.ExamID == examID && r.CandidateID == entityID && r.CourseID == courseID && r.FinalSubmitted == true).FirstOrDefault();
                    ICollection<CandidateContactDetail> contactDetails = cea.Candidate.ContactDetails;
                    if (contactDetails != null)
                    {
                        CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                        emailAddress = cd.EmailAddress;
                    }
                    DemandNote demand = new DemandNote();
                    // code when applicant type is institute and payment source is Institute
                    if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                    {

                        if (cea.PaymentSourceChanged == false)
                        {
                            demand.ApplicationDate = DateTime.Now;
                            demand.FeeTypeID = cea.FeeTypeID.Value;
                            demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                            demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                            demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                            demand.Amount = cea.FeeAmount;
                            demand.CreatedBy = 1;
                            demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                            demand.CourseCategoryID = cea.CourseCategoryID;
                            demand.CourseID = cea.CourseID;
                            demand.ServiceID = cea.Course.ExaminationServiceID;

                            context.DemandNotes.Add(demand);
                            context.SaveChanges();

                            cea.DemandNoteID = demand.ID;
                            cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                            cea.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Candidate);
                            cea.PaymentSourceChanged = true;
                            cea.PaymentSourceChangedBy = Convert.ToInt32(Session["UserID"]);
                            cea.PaymentSourceChangedOn = DateTime.Now;

                            context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();

                            scope.Complete();

                            Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + "<br><br>Your payment option has been changed successfully on your request from institute to candidate on " + cea.PaymentSourceChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + ". and  Your application no is." + " " + cea.Number + "Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is:- " + cea.DemandNote.ID.ToString();
                            try
                            {
                                if (emailAddress.Trim().Length > 0)
                                {
                                    //sending Email 
                                    EConnect.NIELIT.Email mail = new Email("Change Payment Option:NIELIT", Msg, emailAddress);
                                    mail.Send();
                                }
                            }
                            catch (Exception) { }
                            lboutput.Text = "Payment option has been successfully changed by you...";
                            divoutput.Visible = true;
                        }
                        else
                        {
                            ShowAlert("Payment option for this application:- " + cea.Number + " has already been changed", true);
                            return;
                        }

                    }
                    // code when applicant type is institute and payment source is candidate
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
                    {
                        if (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                        {
                            if (cea.PaymentSourceChanged == false)
                            {
                                demand = context.DemandNotes.Find(cea.DemandNoteID);
                                if (demand != null)
                                {                                   
                                    cea.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Institute);
                                    cea.PaymentSourceChanged = true;
                                    cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                    cea.PaymentSourceChangedBy = Convert.ToInt32(Session["UserID"]);
                                    cea.PaymentSourceChangedOn = DateTime.Now;
                                    if (CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.Value))
                                    {
                                        cea.DemandNoteID = null;
                                    }
                                    context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();                                   
                                    scope.Complete();

                                    Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + "<br><br>Your payment option has been changed successfully on your request from candidate to institute on " + cea.PaymentSourceChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + ". and  Your application no is." + " " + cea.Number;
                                    try
                                    {
                                        if (emailAddress.Trim().Length > 0)
                                        {
                                            //sending Email 
                                            EConnect.NIELIT.Email mail = new Email("Change Payment Option:NIELIT", Msg, emailAddress);
                                            mail.Send();
                                        }
                                    }
                                    catch (Exception) { }
                                    lboutput.Text = "Payment option has been successfully changed by you...";
                                    divoutput.Visible = true;
                                }
                                else
                                {
                                    ShowAlert("DemandNote not found for this application:-" + cea.Number + ", payment option cannot be changed", true);
                                    return;
                                }
                            }
                            else
                            {
                                ShowAlert("Payment option for this application:- " + cea.Number + " has already been changed", true);
                                return;
                            }

                        }
                    }
                };
            };
            divradio.Visible = false;
            divoutput1.Visible = false;
            divradio.Visible = false;
            examDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Lnkappchange_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divoutput1.Visible = false;
            divradio1.Visible = true;
            divradio.Visible = false;
            divoutput.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Btnappyes_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            string Msg = "";
            string emailAddress = "";
            //Int64 demandID = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    CourseExamApplication cea = new CourseExamApplication();
                    cea = context.CourseExamApplications.Where(r => r.ExamID == examID && r.CandidateID == entityID && r.CourseID == courseID && r.FinalSubmitted == true).FirstOrDefault();
                    ICollection<CandidateContactDetail> contactDetails = cea.Candidate.ContactDetails;
                    if (contactDetails != null)
                    {
                        CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                        emailAddress = cd.EmailAddress;
                    }
                    DemandNote demand = new DemandNote();
                    // code when applicant type is institute and payment source is Institute
                    if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute))
                    {

                        cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                        cea.FinalSubmitted = false;
                        cea.FinalSubmissionDate = null;
                        context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();


                        Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>You have successfully marked your " + cea.Exam.Name +
                       " (" + cea.Course.Name + ") Examination application as editable on " + DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss") + ". Your application number is:" + " " + cea.Number;
                        scope.Complete();
                        try
                        {
                            if (emailAddress.Trim().Length > 0)
                            {
                                //sending Email 
                                EConnect.NIELIT.Email mail = new Email("Mark Application as Editable/ Cancelled :NIELIT", Msg, emailAddress);
                                mail.Send();
                            }
                        }
                        catch (Exception) { }
                        lboutput1.Text = "Your course exam application has been successfully marked as editable/cancelled by you. Please click on 'Apply Online For Exam' to make changes in exam form and submit by last date.";
                        divoutput1.Visible = true;

                    }
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)))
                    {
                        // Mark appplication as editable when applicant type is institute and payment source is candidate
                        if ((cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                        {
                            demand = context.DemandNotes.Find(cea.DemandNoteID);
                            if (demand != null)
                            {                               
                                cea.FinalSubmitted = false;
                                cea.FinalSubmissionDate = null;
                                if (CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.Value))
                                {
                                    cea.DemandNoteID = null;
                                }
                                context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>You have successfully marked your " + cea.Exam.Name +
                            " (" + cea.Course.Name + ") Examination application as editable on " + DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss") + ". Your application number is:" + " " + cea.Number;
                                scope.Complete();
                                try
                                {
                                    if (emailAddress.Trim().Length > 0)
                                    {
                                        //sending Email 
                                        EConnect.NIELIT.Email mail = new Email("Mark Application as Editable/ Cancelled :NIELIT", Msg, emailAddress);
                                        mail.Send();
                                    }
                                }
                                catch (Exception) { }

                                lboutput1.Text = "Your course exam application has been successfully marked as editable/cancelled by you. Please click on 'Apply Online For Exam' to make changes in exam form and submit by last date.";
                                divoutput1.Visible = true;
                            }
                        }
                        else
                        {
                            String transactionNo = "";
                            demand = context.DemandNotes.Find(cea.DemandNoteID);
                            if (demand.enmPaymentMode == enmPaymentMode.DemandDraft)
                                transactionNo = demand.DemandDraftTransaction.DemandDraftNumber.ToString();
                            else if (demand.enmPaymentMode == enmPaymentMode.CSCSPV)
                                transactionNo = demand.CSCTransaction.ResponseTransactionNumber.ToString();
                            else if (demand.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                transactionNo = demand.NEFTTransaction.TransactionNumber.ToString();
                            else if (demand.enmPaymentMode == enmPaymentMode.Online)
                                transactionNo = demand.OnlineTransaction.ReferenceNumber.ToString();

                            ShowAlert("You cannot mark this application as editable/cancelled because payment is already made by you using " + demand.PaymentMode.Name + "  payment mode with transaction number : " + transactionNo, true);
                            return;
                        }
                    }
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate))
                    {
                        // Mark appplication as editable when applicant type is direct
                        if (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                        {
                            demand = context.DemandNotes.Find(cea.DemandNoteID);
                            if (demand != null)
                            {                               
                                cea.FinalSubmitted = false;
                                cea.FinalSubmissionDate = null;
                                if (CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.Value))
                                {
                                    cea.DemandNoteID = null;
                                }
                                context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>You have successfully marked your " + cea.Exam.Name +
                            " (" + cea.Course.Name + ") Examination application as editable on " + DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss") + ". Your application number is:" + " " + cea.Number;
                                scope.Complete();
                                try
                                {
                                    if (emailAddress.Trim().Length > 0)
                                    {
                                        //sending Email 
                                        EConnect.NIELIT.Email mail = new Email("Mark Application as Editable/ Cancelled :NIELIT", Msg, emailAddress);
                                        mail.Send();
                                    }
                                }
                                catch (Exception) { }

                                lboutput1.Text = "Your course exam application has been successfully marked as editable/cancelled by you. Please click on 'Apply Online For Exam' to make changes in exam form and submit by last date.";
                                divoutput1.Visible = true;
                            }
                        }
                        else
                        {
                            String transactionNo = "";
                            demand = context.DemandNotes.Find(cea.DemandNoteID);
                            if (demand.enmPaymentMode == enmPaymentMode.DemandDraft)
                                transactionNo = demand.DemandDraftTransaction.DemandDraftNumber.ToString();
                            else if (demand.enmPaymentMode == enmPaymentMode.CSCSPV)
                                transactionNo = demand.CSCTransaction.ResponseTransactionNumber.ToString();
                            else if (demand.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                transactionNo = demand.NEFTTransaction.TransactionNumber.ToString();
                            else if (demand.enmPaymentMode == enmPaymentMode.Online)
                                transactionNo = demand.OnlineTransaction.ReferenceNumber.ToString();

                            ShowAlert("You cannot mark this application as editable/cancelled because payment is already made by you using " + demand.PaymentMode.Name + "  payment mode with transaction number : " + transactionNo, true);
                            return;
                        }
                    }
                };
            };
            divradio1.Visible = false;
            divoutput.Visible = false;
            divradio.Visible = false;
            examDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Btnappno_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            divradio1.Visible = false;
            divoutput1.Visible = false;
            divoutput.Visible = false;
            divradio.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}