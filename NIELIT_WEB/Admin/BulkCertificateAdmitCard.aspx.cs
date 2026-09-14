using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Admin_BulkCertificateAdmitCard : BasePage
{
    Int64 EntityID = 0;
    Int32 currentRoleId = 0;
    string mintime = "";
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);

    protected void Page_Load(object sender, EventArgs e)
    {
        //Lblerror.Text = "This facility is temporarily closed due to Covid-19 pandemic it is mandatory for the candidate to click on the declaration related to Covid-19  before downloading the admit card, so you are requested to ask all of your candidates for downloading the admit card by itself through visiting the student portal .";
        //Lblerror.Visible = true;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            EntityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                BindCourseCategory();
                showofficeaddress();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Admit Cards", "#", ""));
                lblcount.Text = "";
                lblcount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = from s in context.CourseCategories
                              join c in context.Courses
                                  on s.ID equals c.CourseCategoryID
                              where c.CourseTypeID == courseTypeCertificateExam
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void CreateAdmidCards(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select c).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;FOR OFFICE USE ONLY : </td>";
                        if (download.BatchItemID.HasValue)
                            html += "<td valign='top' align='left'>&nbsp;" + context.BatchItems.Find(download.BatchItemID.Value).Batch.Number + "/" + download.BatchItemID.Value + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; NA </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.Exam.Name) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + " ) " + GetInitCap(download.Exam.Name) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='3' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        //html += "<td rowspan='2' valign='top'>";
                        //if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                        //    html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        //else
                        //    html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;REPORTING TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        if (download.CourseID == 5 || download.CourseID == 75)
                            html += "60 MIN. </td>";
                        else
                            html += "90 MIN.</td>";
                        html += "</tr>";

                        //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
                        //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a) Candidates shall be admitted to the Examination hall only 15 minutes before the commencement of Examination.<br />";
                        html += " b) Before the commencement of the Examination, it is essential and mandatory for all candidates to sign and put Left Thumb Impression on the attendance sheet.<br />";
                        html += " c) Candidates must carry their Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination.The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadharcard, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " d) No candidate will be permitted to leave the hall before 30 minutes, after the commencement of Examination.<br />";
                        html += " e) Candidates should carry only their admit card issued by NIELIT, a photo identity card and pen at the examination centre.<br />";
                        html += " f) No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " g) No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " h) Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br />";
                        html += " i) No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br />";
                        html += " j) No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br />";
                        html += " k) Candidate should not use abusive/derogatory language orally against the Exam Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators or Exam Supdt.<br />";
                        html += " l) No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br />";
                        html += " m) In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision of the Examination Board in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.<br />";
                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CreateAdmidCardsVersion6(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        ID = c.ID,
                                        Number = c.Number,
                                        RollNumber = c.RollNumber,
                                        Name = c.Name,
                                        FatherName = c.FatherName,
                                        MotherName = c.MotherName,
                                        Gender = c.Gender,
                                        Photo = c.Photo,
                                        GuardianName = c.GuardianName,
                                        IsDisability = c.IsDisability.HasValue ? c.IsDisability : false,
                                        DisabilityName = c.DisabilityType.Name,
                                        ExamCentreName = c.ExamCentreName,
                                        BatchItemID = c.BatchItemID,
                                        CourseID = c.CourseID,
                                        ExamName = c.Exam.Name,
                                        DateOfExam = c.DateOfExam,
                                        ExaminationCycleName = c.Exam.ExaminationCycle.Name,
                                        ExamCentreAddress = c.ExamCentreAddress,
                                        ExamBatchNumber = c.ExamBatchNumber,
                                        ReportingTime = c.ReportingTime

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        //code added by abhi singh dated on 15052024
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;GENDER </td>";
                        html += "<td  align='left'>&nbsp;" + download.Gender + " </td>";
                        html += "</tr>";
                        // End

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;PWD : </td>";
                        if (download.IsDisability == true)
                            html += "<td valign='top' align='left'>&nbsp;" + download.DisabilityName + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; No </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.ExamName) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.ExaminationCycleName) + " ) " + GetInitCap(download.ExamName) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='5' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;REPORTING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                //Lbclosing.Text = repTime.AddMinutes(-15).ToShortTimeString();
                                ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string ReportTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ ReportTime = "08:45 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ ReportTime = "10:30 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ ReportTime = "12:45 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ ReportTime = "02:30 PM"; }
                            //else
                            //{ ReportTime = ""; }
                            html += "" + ReportTime + "  </td>";
                        }
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<b><td colspan='2' align='left'>&nbsp;GATE CLOSING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string GateCloseTime = "";
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                GateCloseTime = repTime.AddMinutes(-15).ToShortTimeString();
                                //ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string GateCloseTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ GateCloseTime = "09:00 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ GateCloseTime = "10:45 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ GateCloseTime = "01:00 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ GateCloseTime = "02:45 PM"; }
                            //else
                            //{ GateCloseTime = ""; }
                            html += "" + GateCloseTime + "<br />No candidate will be allowed to enter the examination center after the gate closing time.</td>";
                        }
                        else
                            html += "  </td></b>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;EXAM START TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        //  if (download.CourseID == 5 || download.CourseID == 75 || download.CourseID==1226 )  // code commented by abhi singh dated on 15052024
                        if (download.CourseID == 5 || download.CourseID == 75 || download.CourseID == 175 || download.CourseID == 1226) // code added by abhi singh dated on 15052024
                        {
                            html += "60 MIN. </td>";
                            mintime = "45";
                        }
                        else
                        {
                            html += "90 MIN.</td>";
                            mintime = "60";
                        }
                        html += "</tr>";
                        //code added by abhi singh dated on 15042024 for give important information on admit card.
                        html += "<tr>";
                        html += "<td colspan='3' align='center'> <b><span>IMPORTANT:CANDIDATES ARE ADVISED TO PUT THEIR SIGNATURE AND LEFT THUMB IMPRESSION(LTI) ON THE ATTENDANCE SHEET, FAILING WHICH, THEIR CANDIDATURE SHALL NOT BE CONSIDERED ADMIT CARD ISSUED BY NIELIT FAILING WHICH THE CANDIDATES WILL NOT BE ALLOWED TO APPEAR IN THE EXAMINATION.</span><br /><span>महत्वपूर्ण: अभ्यर्थियों को उपस्थिति-पत्र में अपने हस्ताक्षर व बाएँ हाथ के अंगूठे  की छाप दिये जाने की सलाह दी जाती है। ऐसा न करने पर, परीक्षा के लिए उनकी अभ्यर्थिता पर विचार नहीं किया जाएगा। अभ्यर्थी को रा.इ.सू.प्रौ.सं. द्धारा जारी प्रवेश-पत्र के साथ एक मूल वैध फोटो पहचान पत्र लाना अनिवार्य है अन्यथा उन्हें परीक्षा में बैठने की अनुमति नहीं दी जायेगी। </span></td></tr>";
                        //end
                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions-3.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a)  Candidates are required to follow the rules, regulations and guidelines of NIELIT; Local Authorities, State Government and Government of India especially related to COVID-19 as issued from time to time.<br />";
                        html += " b)  Candidates are required to report at the examination centre strictly as per the reporting time allotted to the candidate to maintain staggered entry.<br />";
                        html += " c)  Candidates shall be admitted to the Examination Centre maximum 60 minutes before the commencement of Examination.<br />";
                        html += " d)  Candidates shall not be permitted to enter the Examination Centre after Gate Closing Time.<br />";
                        html += " e)  Candidates are expected to take seat in the Examination Hall as soon as possible after entering the examination centre and not move around to maintain social distancing. <br />";                       
                        html += " f)  Candidates entry will not be permitted inside the examination hall after the time of commencement of examination.<br />";                                            
                        html += " g)  Before the commencement of the Examination, it is essential and mandatory for all candidates to sign on the attendance sheet.<br />";
                        html += " h)  Candidate must ensure that s/he appends her/his signature on the attendance sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while submitting Online Examination Application Form for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.<br />";
                        html += " i)  Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination. The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadhar card, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic, Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " j)  Candidates will be allowed to leave the examination hall only after completion of sixty minutes from the time of commencement of examination.<br />";
                        html += " k)  Candidates should carry admit card issued by NIELIT, an original photo identity card, pen, soap/hand-sanitiser of up to 50 ml in transparent bottle, face mask, gloves and water in transparent bottle for personal use. No other item other than specifically mentioned here shall be permissible inside the examination centre.<br />";
                        html += " l)  Candidates are advised not to bring any valuable items to the examination centre as arrangement for safe keeping of such items cannot be assured.<br />";
                        html += " m)  No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br /> ";
                        html += " n)  No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br /> ";
                        html += " o)  Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br /> ";
                        html += " p)  No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br /> ";
                        html += " q)  No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br /> ";
                        html += " r)  Candidate should not use abusive/derogatory language orally or otherwise/ threatening/ using violence towards/against anyone including Examination Staff.<br /> ";
                        html += " s)  No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br /> ";
                        html += " t)  Smoking or Chewing Tobacco or use of Alcohol/Intoxicating Substance is strictly prohibited at the Examination Centre and inside Examination Hall. Candidates found doing so during the course of the Examination, shall be liable, to be expelled from the Examination Centre by the Examination Superintendent. A candidate, if found smoking or chewing tobacco or under the influence of intoxicated drinks/drugs/alcohol shall not be allowed to enter the examination hall and if found appearing in the examination shall be expelled from the examination hall immediately by the Examination Superintendent.<br /> ";
                        html += " u)  In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, candidate shall be expelled from the examination centre and his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision as per SOP of the Examination in imposing penalty for the offence committed by the candidate shall be final and binding on him/her. <br /> ";
                        html += " v)  <strong> Candidates are advised to strictly adhere to the time schedule and instructions. </strong>"; 
                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CreateAdmidCardsVersion5(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        ID = c.ID,
                                        Number = c.Number,
                                        RollNumber = c.RollNumber,
                                        Name = c.Name,
                                        FatherName = c.FatherName,
                                        MotherName = c.MotherName,
                                        Photo = c.Photo,
                                        GuardianName = c.GuardianName,
                                        IsDisability = c.IsDisability.HasValue ? c.IsDisability : false,
                                        DisabilityName = c.DisabilityType.Name,
                                        ExamCentreName = c.ExamCentreName,
                                        BatchItemID = c.BatchItemID,
                                        CourseID = c.CourseID,
                                        ExamName = c.Exam.Name,
                                        DateOfExam = c.DateOfExam,
                                        ExaminationCycleName = c.Exam.ExaminationCycle.Name,
                                        ExamCentreAddress = c.ExamCentreAddress,
                                        ExamBatchNumber = c.ExamBatchNumber,
                                        ReportingTime = c.ReportingTime

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;PWD : </td>";
                        if (download.IsDisability == true)
                            html += "<td valign='top' align='left'>&nbsp;" + download.DisabilityName + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; No </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.ExamName) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.ExaminationCycleName) + " ) " + GetInitCap(download.ExamName) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='5' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;REPORTING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                //Lbclosing.Text = repTime.AddMinutes(-15).ToShortTimeString();
                                ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string ReportTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ ReportTime = "08:45 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ ReportTime = "10:30 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ ReportTime = "12:45 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ ReportTime = "02:30 PM"; }
                            //else
                            //{ ReportTime = ""; }
                            html += "" + ReportTime + "  </td>";
                        }
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<b><td colspan='2' align='left'>&nbsp;GATE CLOSING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string GateCloseTime = "";
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                GateCloseTime = repTime.AddMinutes(-15).ToShortTimeString();
                                //ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string GateCloseTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ GateCloseTime = "09:00 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ GateCloseTime = "10:45 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ GateCloseTime = "01:00 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ GateCloseTime = "02:45 PM"; }
                            //else
                            //{ GateCloseTime = ""; }
                            html += "" + GateCloseTime + "<br />No candidate will be allowed to enter the examination center after the gate closing time.</td>";
                        }
                        else
                            html += "  </td></b>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;EXAM START TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        if (download.CourseID == 5 || download.CourseID == 75)
                        {
                            html += "60 MIN. </td>";
                            mintime = "45";
                        }
                        else
                        {
                            html += "90 MIN.</td>";
                            mintime = "60";
                        }
                        html += "</tr>";



                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions-3.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a) Candidates are required to report at the examination centre half hour prior to the scheduled examination time alloted to the candidate.<br />";
                        html += " b) Candidates shall be admitted to the Examination hall only 15 minutes before the commencement of Examination and candidates will not be permitted to enter the examination hall after expiry of 30 minutes from the time of commencement of examination.<br />";
                        html += " c) Before the commencement of the Examination, it is essential and mandatory for all candidates to sign and put Left Thumb Impression on the attendance sheet.<br />";
                        html += " d) Candidate must ensure that s/he appends her/his signature on the attendance sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while submitting Online Examination Application Form for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.<br />";
                        html += " e) Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination.The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadharcard, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " f) Candidates will be allowed to leave the examination hall only after completion of " + mintime + " minutes from the time of commencement of examination.<br />";
                        html += " g) Candidates should carry only their admit card issued by NIELIT, an original photo identity card and pen at the examination centre.<br />";
                        html += " h) No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " i) No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " j) Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br />";
                        html += " k) No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br />";
                        html += " l) No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br />";
                        html += " m) Candidate should not use abusive/derogatory language orally against the Exam Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators or Exam Supdt.<br />";
                        html += " n) No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br />";
                        html += " o) In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision of the Examination Board in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.<br />";
                        html += " p) <strong>Candidates are advised to strictly adhere to the time schedule and instructions.</strong><br />";

                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CreateAdmidCardsVersion4(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        ID = c.ID,
                                        Number = c.Number,
                                        RollNumber = c.RollNumber,
                                        Name = c.Name,
                                        FatherName = c.FatherName,
                                        MotherName = c.MotherName,
                                        Photo = c.Photo,
                                        GuardianName = c.GuardianName,
                                        IsDisability = c.IsDisability.HasValue ? c.IsDisability : false,
                                        DisabilityName = c.DisabilityType.Name,
                                        ExamCentreName = c.ExamCentreName,
                                        BatchItemID = c.BatchItemID,
                                        CourseID = c.CourseID,
                                        ExamName = c.Exam.Name,
                                        DateOfExam = c.DateOfExam,
                                        ExaminationCycleName = c.Exam.ExaminationCycle.Name,
                                        ExamCentreAddress = c.ExamCentreAddress,
                                        ExamBatchNumber = c.ExamBatchNumber,
                                        ReportingTime = c.ReportingTime

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;PWD : </td>";
                        if (download.IsDisability == true)
                            html += "<td valign='top' align='left'>&nbsp;" +  download.DisabilityName + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; No </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.ExamName) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.ExaminationCycleName) + " ) " + GetInitCap(download.ExamName) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='5' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;REPORTING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                //Lbclosing.Text = repTime.AddMinutes(-15).ToShortTimeString();
                                ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string ReportTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ ReportTime = "08:45 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ ReportTime = "10:30 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ ReportTime = "12:45 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ ReportTime = "02:30 PM"; }
                            //else
                            //{ ReportTime = ""; }
                            html += "" + ReportTime + "  </td>";
                        }
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<b><td colspan='2' align='left'>&nbsp;GATE CLOSING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string GateCloseTime = "";

                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                GateCloseTime = repTime.AddMinutes(-15).ToShortTimeString();
                                //ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //if (download.ExamBatchNumber == "1")
                            //{ GateCloseTime = "09:00 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ GateCloseTime = "10:45 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ GateCloseTime = "01:00 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ GateCloseTime = "02:45 PM"; }
                            //else
                            //{ GateCloseTime = ""; }
                            html += "" + GateCloseTime + "<br />No candidate will be allowed to enter the examination center after the gate closing time.</td>";
                        }
                        else
                            html += "  </td></b>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;EXAM START TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        if (download.CourseID == 5 || download.CourseID == 75)
                        {
                            html += "60 MIN. </td>";
                            mintime = "45";
                        }
                        else
                        {
                            html += "90 MIN.</td>";
                            mintime = "60";
                        }
                        html += "</tr>";



                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions-3.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a) Candidates are required to report at the examination centre half hour prior to the scheduled examination time alloted to the candidate.<br />";
                        html += " b) Candidates shall be admitted to the Examination hall only 15 minutes before the commencement of Examination and candidates will not be permitted to enter the examination hall after expiry of 30 minutes from the time of commencement of examination.<br />";
                        html += " c) Before the commencement of the Examination, it is essential and mandatory for all candidates to sign and put Left Thumb Impression on the attendance sheet.<br />";
                        html += " d) Candidate must ensure that s/he appends her/his signature on the attendance sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while submitting Online Examination Application Form for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.<br />";
                        html += " e) Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination.The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadharcard, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " f) Candidates will be allowed to leave the examination hall only after completion of " + mintime + " minutes from the time of commencement of examination.<br />";
                        html += " g) Candidates should carry only their admit card issued by NIELIT, an original photo identity card and pen at the examination centre.<br />";
                        html += " h) No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " i) No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " j) Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br />";
                        html += " k) No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br />";
                        html += " l) No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br />";
                        html += " m) Candidate should not use abusive/derogatory language orally against the Exam Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators or Exam Supdt.<br />";
                        html += " n) No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br />";
                        html += " o) In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision of the Examination Board in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.<br />";
                        html += " p) <strong>Candidates are advised to strictly adhere to the time schedule and instructions.</strong><br />";

                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CreateAdmidCardsVersion3(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        ID = c.ID,
                                        Number = c.Number,
                                        RollNumber = c.RollNumber,
                                        Name = c.Name,
                                        FatherName = c.FatherName,
                                        MotherName = c.MotherName,
                                        Photo = c.Photo,
                                        GuardianName = c.GuardianName,
                                        ExamCentreName = c.ExamCentreName,
                                        BatchItemID = c.BatchItemID,
                                        CourseID = c.CourseID,
                                        ExamName = c.Exam.Name,
                                        DateOfExam = c.DateOfExam,
                                        ExaminationCycleName = c.Exam.ExaminationCycle.Name,
                                        ExamCentreAddress = c.ExamCentreAddress,
                                        ExamBatchNumber = c.ExamBatchNumber,
                                        ReportingTime = c.ReportingTime

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;FOR OFFICE USE ONLY : </td>";
                        if (download.BatchItemID.HasValue)
                            html += "<td valign='top' align='left'>&nbsp;" + context.BatchItems.Find(download.BatchItemID.Value).Batch.Number + "/" + download.BatchItemID.Value + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; NA </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.ExamName) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.ExaminationCycleName) + " ) " + GetInitCap(download.ExamName) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='5' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;REPORTING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                //Lbclosing.Text = repTime.AddMinutes(-15).ToShortTimeString();
                                ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string ReportTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ ReportTime = "08:45 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ ReportTime = "10:30 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ ReportTime = "12:45 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ ReportTime = "02:30 PM"; }
                            //else
                            //{ ReportTime = ""; }
                            html += "" + ReportTime + "  </td>";
                        }
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<b><td colspan='2' align='left'>&nbsp;GATE CLOSING TIME : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                        {
                            string GateCloseTime = "";
                            string ReportTime = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";

                            DateTime repTime = Convert.ToDateTime(ReportTime);


                            if (repTime != DateTime.MinValue)
                            {
                                GateCloseTime = repTime.AddMinutes(-15).ToShortTimeString();
                                //ReportTime = repTime.AddMinutes(-30).ToShortTimeString();
                            }

                            //string GateCloseTime = "";
                            //if (download.ExamBatchNumber == "1")
                            //{ GateCloseTime = "09:00 AM"; }
                            //else if (download.ExamBatchNumber == "2")
                            //{ GateCloseTime = "10:45 AM"; }
                            //else if (download.ExamBatchNumber == "3")
                            //{ GateCloseTime = "01:00 PM"; }
                            //else if (download.ExamBatchNumber == "4")
                            //{ GateCloseTime = "02:45 PM"; }
                            //else
                            //{ GateCloseTime = ""; }
                            html += "" + GateCloseTime + "<br />No candidate will be allowed to enter the examination center after the gate closing time.</td>";
                        }
                        else
                            html += "  </td></b>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left'>&nbsp;EXAM START TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        if (download.CourseID == 5 || download.CourseID == 75)
                        {
                            html += "60 MIN. </td>";
                            mintime = "45";
                        }
                        else
                        {
                            html += "90 MIN.</td>";
                            mintime = "60";
                        }
                        html += "</tr>";

                       

                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions-3.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a) Candidates are required to report at the examination centre half hour prior to the scheduled examination time alloted to the candidate.<br />";
                        html += " b) Candidates shall be admitted to the Examination hall only 15 minutes before the commencement of Examination and candidates will not be permitted to enter the examination hall after expiry of 30 minutes from the time of commencement of examination.<br />";
                        html += " c) Before the commencement of the Examination, it is essential and mandatory for all candidates to sign and put Left Thumb Impression on the attendance sheet.<br />";
                        html += " d) Candidate must ensure that s/he appends her/his signature on the attendance sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while submitting Online Examination Application Form for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.<br />";
                        html += " e) Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination.The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadharcard, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " f) Candidates will be allowed to leave the examination hall only after completion of " + mintime + " minutes from the time of commencement of examination.<br />";
                        html += " g) Candidates should carry only their admit card issued by NIELIT, an original photo identity card and pen at the examination centre.<br />";
                        html += " h) No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " i) No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " j) Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br />";
                        html += " k) No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br />";
                        html += " l) No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br />";
                        html += " m) Candidate should not use abusive/derogatory language orally against the Exam Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators or Exam Supdt.<br />";
                        html += " n) No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br />";
                        html += " o) In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision of the Examination Board in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.<br />";
                        html += " p) <strong>Candidates are advised to strictly adhere to the time schedule and instructions.</strong><br />";

                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CreateAdmidCardsVersion2(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        ID = c.ID,
                                        Number = c.Number,
                                        RollNumber = c.RollNumber,
                                        Name = c.Name,
                                        FatherName = c.FatherName,
                                        MotherName = c.MotherName,
                                        Photo = c.Photo,
                                        GuardianName = c.GuardianName,
                                        ExamCentreName = c.ExamCentreName,
                                        BatchItemID = c.BatchItemID,
                                        CourseID = c.CourseID,
                                        ExamName = c.Exam.Name,
                                        DateOfExam = c.DateOfExam,
                                        ExaminationCycleName = c.Exam.ExaminationCycle.Name,
                                        ExamCentreAddress = c.ExamCentreAddress,
                                        ExamBatchNumber = c.ExamBatchNumber,
                                        ReportingTime = c.ReportingTime

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;FOR OFFICE USE ONLY : </td>";
                        if (download.BatchItemID.HasValue)
                            html += "<td valign='top' align='left'>&nbsp;" + context.BatchItems.Find(download.BatchItemID.Value).Batch.Number + "/" + download.BatchItemID.Value + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; NA </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.ExamName) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.ExaminationCycleName) + " ) " + GetInitCap(download.ExamName) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='3' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        //html += "<td rowspan='2' valign='top'>";
                        //if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                        //    html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        //else
                        //    html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;EXAM TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        if (download.CourseID == 5 || download.CourseID == 75)
                        {
                            html += "60 MIN. </td>";
                            mintime = "45";
                        }
                        else
                        {
                            html += "90 MIN.</td>";
                            mintime = "60";
                        }
                        html += "</tr>";

                        //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
                        //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a) Candidates are required to report at the examination centre one hour prior to the scheduled examination time alloted to the candidate.<br />";
                        html += " b) Candidates shall be admitted to the Examination hall only 15 minutes before the commencement of Examination and candidates will not be permitted to enter the examination hall after expiry of 30 minutes from the time of commencement of examination.<br />";
                        html += " c) Before the commencement of the Examination, it is essential and mandatory for all candidates to sign and put Left Thumb Impression on the attendance sheet.<br />";
                        html += " d) Candidate must ensure that s/he appends her/his signature on the attendance sheet at the examination centre in the same manner as uploaded by them at the Student Online Portal while submitting Online Examination Application Form for the particular course. NIELIT at its own discretion may check record of any/all candidate(s) and if any mismatch in the signatures of the candidate on the above said two documents is found, there is a possibility of cancellation of examination of such candidates.<br />";
                        html += " e) Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination.The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadharcard, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " f) Candidates will be allowed to leave the examination hall only after completion of " + mintime + " minutes from the time of commencement of examination.<br />";
                        html += " g) Candidates should carry only their admit card issued by NIELIT, an original photo identity card and pen at the examination centre.<br />";
                        html += " h) No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " i) No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " j) Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br />";
                        html += " k) No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br />";
                        html += " l) No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br />";
                        html += " m) Candidate should not use abusive/derogatory language orally against the Exam Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators or Exam Supdt.<br />";
                        html += " n) No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br />";
                        html += " o) In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision of the Examination Board in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.<br />";
                        html += " p) <strong>Candidates are advised to strictly adhere to the time schedule and instructions.</strong><br />";

                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CreateAdmidCardsVersion1(Int64 appid, Int32 currentCourseID, String DirectoryName)
    {
        try
        {

            String html = "<table align='center' border='0' cellpadding='1' cellspacing='1' style='width:956px;'>" +
                           "<tr><td style='border: 0;'></td><td style='border: 0;'></td><td style='border: 0;'></td></tr><tr><td >" +
                           "<img src= '" + Server.MapPath("~/App_Themes/Blue/Images/Logo.jpg") + "' alt='NIELIT' /></td><td colspan='2' align='center' style='background:#8fbcdb; font:bold 15px verdana; color:#000066; vertical-align:top; padding:12px 0 0 5px;'>";

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        ID = c.ID,
                                        Number = c.Number,
                                        RollNumber = c.RollNumber,
                                        Name = c.Name,
                                        FatherName = c.FatherName,
                                        MotherName = c.MotherName,
                                        Photo = c.Photo,
                                        GuardianName = c.GuardianName,
                                        ExamCentreName = c.ExamCentreName,
                                        BatchItemID = c.BatchItemID,
                                        CourseID = c.CourseID,
                                        ExamName = c.Exam.Name,
                                        DateOfExam = c.DateOfExam,
                                        ExaminationCycleName = c.Exam.ExaminationCycle.Name,
                                        ExamCentreAddress = c.ExamCentreAddress,
                                        ExamBatchNumber = c.ExamBatchNumber,
                                        ReportingTime = c.ReportingTime

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        html += showofficeaddress();
                        html += "</td></tr>";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='padding: 5px; font-weight: bold;'>";
                        html += "<span>NIELIT '" + currentCourse.Code + "' EXAMINATION </span><br/><span>CANDIDATE ADMIT CARD </span></td></tr>";
                        html += "<tr class='normal'><td colspan='3' align='center' style='font-weight: bold;'> Name of the Candidate<i> ( AS FILLED BY THE CANDIDATE IN OEAF)</i></td></tr> ";
                        html += "<tr style='height:40px;'><td colspan='3'></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td align='left' style='font-weight:bold;'>&nbsp;ROLL NO </td>";
                        html += "<td align='left' style='font-weight:bold;padding-left: 5px;' >&nbsp;" + download.RollNumber + " </td>";

                        if (download.Photo != null)
                        {
                            FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg"), FileMode.Create, FileAccess.Write);
                            fs.Write(download.Photo, 0, download.Photo.Length);
                            fs.Close();
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'><img src='" + Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + download.Number + ".jpg") + "' height='98%' width='95%' /></td>";
                        }
                        else
                        {
                            html += "<td style='padding:8px 8px 8px 8px;width:10%;' align='center' rowspan='6'> Photo Not Available </td>";
                        }
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td  align='left'>&nbsp;NAME </td>";
                        html += "<td  align='left'>&nbsp;" + GetInitCap(download.Name) + " </td>";
                        html += "</tr>";
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;MOTHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.MotherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";

                            //father name
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;FATHER'S NAME </td>";
                            if (string.IsNullOrEmpty(download.FatherName) == false && string.IsNullOrWhiteSpace(download.FatherName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.FatherName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA </td>";
                            html += "</tr>";
                        }
                        else
                        {
                            html += "<tr border='1'>";
                            html += "<td align='left'>&nbsp;GUARDIAN NAME </td>";
                            if (string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false)
                                html += "<td align='left'>&nbsp;" + GetInitCap(download.GuardianName) + " </td>";
                            else
                                html += "<td align='left'>&nbsp;NA</td>";
                            html += "</tr>";
                        }

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTER CODE </td>";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "<td align='left'>&nbsp;" + download.ExamCentreName.ToUpper() + " </td>";
                        else
                            html += "<td align='left'> </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' valign='top'>&nbsp;FOR OFFICE USE ONLY : </td>";
                        if (download.BatchItemID.HasValue)
                            html += "<td valign='top' align='left'>&nbsp;" + context.BatchItems.Find(download.BatchItemID.Value).Batch.Number + "/" + download.BatchItemID.Value + "</td>";
                        else
                            html += "<td valign='top' align='left'>&nbsp; NA </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' colspan='3' style='font-weight:bold;'> VALID FOR ";
                        if (!string.IsNullOrEmpty(download.ExamName) && download.DateOfExam.HasValue)
                        {
                            Int32 occurance = 0;
                            string occuranceno = "";
                            occurance = (int)Math.Ceiling(download.DateOfExam.Value.Day / 7.0);
                            if (occurance == 1)
                                occuranceno = occurance.ToString() + "<sup>st</sup> ";
                            else if (occurance == 2)
                                occuranceno = occurance.ToString() + "<sup>nd</sup> ";
                            else if (occurance == 3)
                                occuranceno = occurance.ToString() + "<sup>rd</sup> ";
                            else if (occurance == 4)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            else if (occurance == 5)
                                occuranceno = occurance.ToString() + "<sup>th</sup> ";
                            //Lbename.Text = "(" + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(download.Exam.Name);
                            html += "" + " ( " + currentCourse.Code + "  " + GetInitCap(download.ExaminationCycleName) + " ) " + GetInitCap(download.ExamName) + " EXAMINATION ONLY</td>";
                        }
                        else
                        {
                            html += "NA </td>";
                        }
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='center' valign='top' colspan='3'><b><u>BATCH SCHEDULE</u></b></td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left'>&nbsp;EXAM CENTRE CODE : ";
                        if (!string.IsNullOrEmpty(download.ExamCentreName))
                            html += "" + download.ExamCentreName.ToUpper() + "</td>";
                        else
                            html += " </td>";

                        html += "<td colspan='2' align='left'>&nbsp;EXAM DATE : ";
                        if (download.DateOfExam.HasValue)
                            html += "" + download.DateOfExam.Value.ToString("dd-MMM-yyyy") + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td align='left' rowspan='3' style='padding-left:2px;'>&nbsp;EXAM CENTRE ADDRESS : " + "<br/>";
                        if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                            html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        else
                            html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;BATCH : ";
                        if (!string.IsNullOrEmpty(download.ExamBatchNumber))
                            html += "" + GetInitCap(download.ExamBatchNumber) + "</td>";
                        else
                            html += " </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        //html += "<td rowspan='2' valign='top'>";
                        //if (!string.IsNullOrEmpty(download.ExamCentreAddress))
                        //    html += "" + download.ExamCentreAddress.ToUpper() + "</td>";
                        //else
                        //    html += "</td>";
                        html += "<td colspan='2' align='left'>&nbsp;REPORTING TIME : ";
                        if (!string.IsNullOrEmpty(download.ReportingTime))
                            html += "" + download.ReportingTime.ToUpper() + "  </td>";
                        else
                            html += "  </td>";
                        html += "</tr>";

                        html += "<tr border='1'>";
                        html += "<td colspan='2' align='left' valign='top'>&nbsp;EXAM DURATION : ";
                        if (download.CourseID == 5 || download.CourseID == 75)
                        {
                            html += "60 MIN. </td>";
                            mintime = "45";
                        }
                        else
                        {
                            html += "90 MIN.</td>";
                            mintime = "60";
                        }
                        html += "</tr>";

                        //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
                        //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                        html += "<tr border='1' >";
                        html += "<td colspan='3'  align='center' valign='top'  >";
                        html += " <table border='0' valign='top' style=' height:100%; width:100%; margin-top:0px;' cellpadding='0' cellspacing='0' > <tr><td><img border='0' vspace='0' src= '" + Server.MapPath("~/App_Themes/Blue/Images/Instructions.JPG") + "' alt='Instructions' /> </td>";
                        html += "</tr></table></td></tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' align='center'><strong>INSTRUCTIONS TO BE FOLLOWED BY CANDIDATES AT NIELIT EXAMINATION</strong></td>";
                        html += "</tr>";
                        html += "<tr border='1'>";
                        html += "<td colspan='3' style='text-align:justify;margin-left:10px;margin-right:10px;'><div style='pading : 3px 3px 3px 3px;'>";
                        html += " a) Candidates are required to report at the examination centre one hour prior to the scheduled examination time alloted to the candidate.<br />";
                        html += " b) Candidates shall be admitted to the Examination hall only 15 minutes before the commencement of Examination and candidates will not be permitted to enter the examination hall after expiry of 30 minutes from the time of commencement of examination.<br />";
                        html += " c) Before the commencement of the Examination, it is essential and mandatory for all candidates to sign and put Left Thumb Impression on the attendance sheet.<br />";
                        html += " d) Candidates must carry their Original Photo Identity Proof failing which; the candidates will not be allowed to appear in the examination.The valid photo Identity proofs are Voter ID card, Passport, PAN card, Permanent laminated driving licence, Aadharcard, Student identity card with photograph issued by recognised School/College/ITI/Polytechnic,Photo identity card having serial number issued by Central/State Government, Photo identity card issued by PSU/Autonomous bodies, Nationalised bank passbook with photograph with attested customer photograph and signature by bank official/manager, Credit cards issued by banks with laminated photograph, Certificate of Identity having photo issued by Gazetted Officer or Tehsildar on letterhead. <br />";
                        html += " e) Candidates will be allowed to leave the examination hall only after completion of " + mintime + " minutes from the time of commencement of examination.<br />";
                        html += " f) Candidates should carry only their admit card issued by NIELIT, an original photo identity card and pen at the examination centre.<br />";
                        html += " g) No cell phones or any electronic device will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " h) No pocketbooks, handbags, books, notes, written or printed material, CDs or data of any kind will be allowed in the examination hall. If any such items are found in possession of the candidate, they will be confiscated.<br />";
                        html += " i) Candidate will not be allowed to make any notes from memory once he/she enter the examination hall and prior to the start of an examination session. If any candidate is found with any such items, they will be confiscated.<br />";
                        html += " j) No candidate can bring any knife (including any pocketknife or multi-tool with blades), or any weapon of any kind into the examination centre.<br />";
                        html += " k) No candidate shall create a continuing distraction by sound or movement which tends to disrupt the concentration of another candidate nor shall any candidate engage in any activity which reasonably may be considered to be a breach of the peace.<br />";
                        html += " l) Candidate should not use abusive/derogatory language orally against the Exam Supdt./Technical Coordinator/Invigilator or threatening/using violence towards Invigilators or Exam Supdt.<br />";
                        html += " m) No candidate shall give or receive aid from any other applicant or source during the administration of the examination.<br />";
                        html += " n) In case any candidate is found breaching of the hall discipline and / or found having used unfair means which directly / indirectly disturbs the sanctity of the examination, his/her examination result shall be withheld, and action as deemed fit under the SOP will be taken. The decision of the Examination Board in imposing penalty for the offence committed by the candidate shall be final and binding on him/her.<br />";
                        html += " o) <strong>Candidates are advised to strictly adhere to the time schedule and instructions.</strong><br />";

                        html += "</div></td>";
                        html += "</tr>";
                        html += "</table>";
                        HTMLToPdf(html, download.Number, DirectoryName);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected string showofficeaddress()
    {
        string html = string.Empty;
        try
        {
            using (EConnectContext ctx = new EConnectContext())
            {
                Session["OrgID"] = "1";
                Int32 orgID = Convert.ToInt32(Session["OrgID"]);
                var organisation = (from u in ctx.Organizations
                                    where u.ID == orgID
                                    select u).Single();

                string m = organisation.MainHeading;
                string s = organisation.SubHeading;

                html = "<div>" + organisation.Name + "</div><div style='font:normal 11px verdana;color:#000000;padding:2px 0 0 2px;width:100%;'>" + string.Concat(m, s) + "</div><div style='font:normal 11px verdana;color:#000000;padding: 2px 0 0 2px;width:100%;text-align:center;margin-right:45px;'>" + organisation.AddressLine1 + " " + organisation.AddressLine2 + ", " + organisation.CityName + " - " + organisation.PinCode.ToString() + "</div>";

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return html;
    }
    // public void ZippedFile(string directoryName)
    //{
    //    string p = Server.MapPath("~/Temp_Docs/" + directoryName);
    //    if (Directory.Exists(p))
    //    {
    //        using (ZipFile zipFile = new ZipFile())
    //        {
    //            zipFile.AddDirectory(Server.MapPath("~/Temp_Docs/" + directoryName));
    //            Response.Clear();
    //            zipFile.CompressionMethod = CompressionMethod.None;
    //            zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
    //            Response.ContentType = "application/zip";
    //            Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
    //            zipFile.Save(Response.OutputStream);
    //        };
            
    //    }
    //    else
    //    {
    //        ShowAlert("Data not available for this category.", true);
    //        return;
    //    }
    //}
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Lblerror.Visible == true)
            //{
            //    Lblerror.Text = "This facility is temporarily closed due to Covid-19 pandemic it is mandatory for the candidate to click on the declaration related to Covid-19  before downloading the admit card, so you are requested to ask all of your candidates for downloading the admit card by itself through visiting the student portal .";
            //}

            //else
            //{
                BreadCrumb1.Render();
                lblcount.Visible = true;
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 applicatTypeID = Convert.ToInt32(enmApplicantType.Institute);
                using (EConnectContext context = new EConnectContext())
                {
                    String directoryName = context.Institutes.Find(EntityID).Name + "_" + context.Courses.Find(courseID).Code.ToUpper() + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "");
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Temp_Docs/" + directoryName));
                    var applications = (from c in context.CertificateExamApplications.AsNoTracking()
                                        where c.InstituteID == EntityID && c.ApplicantTypeID == applicatTypeID && c.FinalSubmitted == true && c.RollNumber != null && c.CourseID == courseID && c.ExamID == examID
                                        select new
                                        {
                                            ID = c.ID,
                                            CourseID = c.CourseID
                                        }).ToList();

                    lblcount.Text = "Total no of Admit Cards:- " + applications.Count().ToString();
                    if (applications.Count() <= 0)
                        return;

                    int applicationsCount = applications.Count();

                    // changes should be done same as DownloadAdmitCard Dynamic version5 

                    // added newly to enable the bulk admit card Download after covid

                    if ((examID >= 5055 && courseID == 7) || (examID >= 5067 && courseID == 5) || (examID >= 5079 && courseID == 98) || (examID >= 5091 && courseID == 99) || (examID >= 5103 && courseID == 174) || (examID >= 5115 && courseID == 175) || (examID >= 7783 && courseID == 1226)) //|| (examID >= 5107 && CourseID == 175))
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion6(applications[i].ID, applications[i].CourseID, directoryName);
                        }

                         //Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion7.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); 
                    }

                    else if ((examID >= 4371 && courseID == 7) || (examID >= 4383 && courseID == 5) || (examID >= 4395 && courseID == 98) || (examID >= 4407 && courseID == 99) || (examID >= 4431 && courseID == 174) || (examID >= 4455 && courseID == 175)) //|| (examID >= 5107 && CourseID == 175))
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion5(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }

                    //till here 

                    else  if ((examID >= 3778 && examID < 4371 && courseID == 7) || (examID >= 3790 && examID < 4383 && courseID == 5) || (examID >= 3802 && examID < 4395 && courseID == 98) || (examID >= 3814 && examID < 4407 && courseID == 99))
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion5(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }

                    else   if ((examID >= 2708 && courseID == 7) || (examID >= 2696 && courseID == 5) || (examID >= 2720 && courseID == 98) || (examID >= 2732 && courseID == 99))
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion4(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }
                    else if ((examID >= 1944 && courseID == 7) || (examID >= 1932 && courseID == 5) || (examID >= 1968 && courseID == 99) || (examID >= 1956 && courseID == 98) || (examID >= 2640 && courseID == 101) || (examID >= 2628 && courseID == 100))
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion3(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }
                    else if ((examID >= 1177 && courseID == 7) || (examID >= 1189 && courseID == 5) || (examID >= 1201 && courseID == 99) || (examID >= 1213 && courseID == 98) || (examID >= 1225 && courseID == 101) || (examID >= 1237 && courseID == 100))
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion2(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }
                    else if ((examID >= 1004 && examID < 1011) || (examID >= 992 && examID < 999) || (examID >= 1089 && examID < 1124) || examID >= 1170)
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCardsVersion1(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < applicationsCount; i++)
                        {
                            CreateAdmidCards(applications[i].ID, applications[i].CourseID, directoryName);
                        }
                    }

                    //deleting images before ZIP
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Temp_Docs/" + directoryName));
                    FileInfo[] files = dir.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
                    foreach (FileInfo file in files)
                    {
                        if (file.Extension == ".jpg")
                        {
                            file.Delete();
                        }
                    }

                
                    try
                    {
                        //using (ZipFile zipFile = new ZipFile())
                        //{
                        //    zipFile.AddDirectory(Server.MapPath("~/Temp_Docs/" + directoryName));
                        //    Response.Clear();
                        //    zipFile.CompressionMethod = CompressionMethod.None;
                        //    zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                        //    Response.ContentType = "application/zip";
                        //    Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
                        //    zipFile.Save(Response.OutputStream);
                        //    //  Response.Flush();
                        //    // Response.End();
                        //};


                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                            zip.AddDirectory(Server.MapPath("~/Temp_Docs/" + directoryName));
                            //zip.AddDirectoryByName("Files");
                            //foreach (GridViewRow row in GridView1.Rows)
                            //{
                            //    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                            //    {
                            //        string filePath = (row.FindControl("lblFilePath") as Label).Text;
                            //        zip.AddFile(filePath, "Files");
                            //    }
                            //}
                            Response.Clear();
                            Response.BufferOutput = false;
                            string zipName = String.Format("AdmitCards.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                            zip.Save(Response.OutputStream);
                            Response.End();
                        }




                    }
                    catch (Exception ex)
                    {
                        ShowAlert(ex.Message);
                    }

                  //  Response.Close();

                    //deleting directories one day old
                    //DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Temp_Docs/"));
                    //if (d.Exists)
                    //{
                    //    if (d.CreationTime < DateTime.Now.AddDays(-1))
                    //        d.Delete(true);
                    //}

                    //System.IO.Directory.Delete(Server.MapPath("~/Temp_Docs/" + directoryName), true);
                };
            }
       // }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == courseTypeCertificateExam
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            ddlAppType.SelectedValue = "0";
            ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                    System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);

                };
            }
            else
            {
                ddlExamCycle.Items.Clear();
                ddlExamCycle.Items.Insert(0, lst);
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    var courses = (from s in context.Exams
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                   select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    courses = courses.OrderByDescending(s => s.TextField).Take(2);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                    ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                }
                else
                {
                    ddlExamYear.Items.Clear();
                    ddlExamYear.Items.Insert(0, lst);
                    ddlExamYear.SelectedValue = "0";
                    ddlExamYear_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    var courses = (from s in context.Exams
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear && s.DateOfPublishingOfTimeTable.HasValue
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public void HTMLToPdf(String HTML, String ApplicationNumber, String DirectoryName)
    {
        try
        {
            //var htmlContent = String.Format(HTML);

            //using PdfGenerator.dll
            //var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent);

            ////using PCheckin.dll
            //var pdfBytes = Factory.Create(new GlobalConfig()).Convert(htmlContent);

            //FileStream fs = new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + ApplicationNumber + ".pdf"), FileMode.Create, FileAccess.Write);
            //fs.Write(pdfBytes, 0, pdfBytes.Length);
            //fs.Close();

            Document pdfDoc = new Document(PageSize.A3);
            StyleSheet ss = new iTextSharp.text.html.simpleparser.StyleSheet();
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Style = ss;
            PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("~/Temp_Docs/" + DirectoryName + "/AppNo-" + ApplicationNumber + ".pdf"), FileMode.Create));
            pdfDoc.Open();
            StringWriter sw = new StringWriter();
            Html32TextWriter hw = new Html32TextWriter(sw);
            pnladmitcard.Controls.Add(new LiteralControl(HTML));
            pnladmitcard.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            htmlparser.Parse(sr);
            pdfDoc.Close();
            pnladmitcard.Controls.Clear();
        }
        catch (Exception ex)
        {
            //throw ex;
            ShowAlert(ex.Message.ToString(), true);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            lblcount.Text = "";
            lblcount.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblcount.Visible = true;
            Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
            Int32 applicatTypeID = Convert.ToInt32(enmApplicantType.Institute);
            using (EConnectContext context = new EConnectContext())
            {
                var applicationsCount = (from c in context.CertificateExamApplications
                                         where c.InstituteID == EntityID && c.ApplicantTypeID == applicatTypeID && c.FinalSubmitted == true && c.RollNumber != null && c.CourseID == courseID && c.ExamID == examID
                                         select c.Number).Count();

                lblcount.Text = "Total no of Admit Cards:- " + applicationsCount.ToString();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString(), true);
        }

    }
}