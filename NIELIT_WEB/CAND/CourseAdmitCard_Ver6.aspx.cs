using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;


public partial class CourseAdmitCard : BasePage
{
    Table tbl = new Table();
    Int32 practical = Convert.ToInt32(enmModuleType.Practical);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && (!String.IsNullOrEmpty(Request.QueryString["Appid"])))
            {
                RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
            }
            else
            {
                //Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
                //Response.End();
                //return;
            }

            if (!Page.IsPostBack)
            {
                showofficeaddress();
                //tbl.BorderWidth = Unit.Pixel(1);
                //tbl.BorderColor = System.Drawing.Color.Black;
                tbl.CellPadding = 0;
                tbl.CellSpacing = 0;
                tbl.Width = Unit.Percentage(100);
                tbl.HorizontalAlign = HorizontalAlign.Left;
                ShowData();
                divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void RenderPage(Int32 currentCourseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                Course currentCourse = context.Courses.Find(currentCourseID);
                Int64 appid = Convert.ToInt64(Request.QueryString["Appid"]);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    var download = (from c in context.CourseExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select c).FirstOrDefault();

                    if (download != null)
                    {
                        Lblrno.Text = download.RollNumber.Value.ToString();
                        Lbcname.Text = download.Candidate.Salutation + GetInitCap(download.Candidate.Name);
                        Lbregno.Text = download.RegistrationNumber.ToString();
                        Lblevel.Text = download.CourseCategory.Code + " - " + download.Course.Name.ToUpper();
                        //Lbexamyear.Text = download.Exam.ExamYear.ToString();
                        //lboffrefno.Text = download.BatchItemID.HasValue ? download.BatchItemID.Value.ToString() : "NA";
                        Int32 correspondenceaddress = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        var CorAdd = download.Candidate.Addresses.Where(s => s.AddressTypeID == correspondenceaddress).OrderByDescending(s => s.EffectiveDateFrom).FirstOrDefault();
                        {
                            lbstudaddress.Text = string.IsNullOrEmpty(CorAdd.AddressLine1) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine1) ? GetInitCap(CorAdd.AddressLine1) : "";
                            lbstudaddress.Text += string.IsNullOrEmpty(CorAdd.AddressLine2) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine2) ? WebUtility.HtmlDecode("<br/>") + GetInitCap(CorAdd.AddressLine2) : "";
                            lbstudaddress.Text += string.IsNullOrEmpty(CorAdd.AddressLine3) == false && !string.IsNullOrWhiteSpace(CorAdd.AddressLine3) ? WebUtility.HtmlDecode("<br/>") + GetInitCap(CorAdd.AddressLine3) : "";
                            lbstudaddress.Text += string.IsNullOrEmpty(CorAdd.CityName) == false && !string.IsNullOrWhiteSpace(CorAdd.CityName) ? WebUtility.HtmlDecode("<br/>") + GetInitCap(CorAdd.CityName) : "";
                            if (CorAdd.DistrictID.HasValue)
                                lbstudaddress.Text += WebUtility.HtmlDecode("<br/>") + " Dist:- " + GetInitCap(CorAdd.District.Name) + WebUtility.HtmlDecode(", ");
                            else
                                lbstudaddress.Text += "";
                            lbstudaddress.Text += (CorAdd.StateID.HasValue && CorAdd.StateID != 0) ? CorAdd.State.Name : "";
                            lbstudaddress.Text += CorAdd.PinCode.HasValue ? WebUtility.HtmlDecode("<br/>") + " Pin:- " + CorAdd.PinCode.Value.ToString() : "";
                        }
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download.Candidate.Photo.BlobFile);
                        ImgCandidatesignature.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download.Candidate.Signature.BlobFile);
                        //ImgCandidatethumb.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download.Candidate.LeftThumbImpression.BlobFile);
                        Lblmedium.Text = (!string.IsNullOrEmpty(download.MediumOfExam.Name)) ? GetInitCap(download.MediumOfExam.Name) : "";


                        //if (download.ExamID == 4246 || download.ExamID == 4248 || download.ExamID == 4250 || download.ExamID == 4252)
                        //{
                        //    Int64 courseid = Convert.ToInt64(Request.QueryString["id"]);
                        //    var newExam = (from m in context.Exams
                        //                   where m.CourseID == courseid && m.ExamYear == 2021 && m.ExamMonth == 1
                        //                   select new
                        //                   {
                        //                       examId = m.ID,
                        //                       examname = m.Name
                        //                   }).FirstOrDefault();

                        //    Lbename.Text = (!string.IsNullOrEmpty(newExam.examname.ToUpper())) ? "CANDIDATE ADMIT CARD <br /> VALID FOR " + newExam.examname.ToUpper() + " EXAMINATION ONLY" : "";
                        //}
                        //else
                        //{
                            Lbename.Text = (!string.IsNullOrEmpty(download.Exam.Name.ToUpper())) ? "CANDIDATE ADMIT CARD <br /> VALID FOR " + download.Exam.Name.ToUpper() + "  EXAMINATION ONLY RESCHEDULE IN MARCH 2022" : "";
                        //}
                        //lbename1.Text = (!string.IsNullOrEmpty(download.Exam.Name)) ? download.Exam.Name.ToUpper() : "";
                        if (download.Exam.DateOfPublishingOfRollNumber.HasValue)
                        {
                            lbpubdate.Text = download.Exam.DateOfPublishingOfRollNumber.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                           // lbpubdate.Text = "NA";

                            lbpubdate.Text = "";
                        }
                        Lbccode.Text = (!string.IsNullOrEmpty(download.AllottedExamCentre.Code)) ? download.AllottedExamCentre.Code.ToUpper() : "";
                        var examcentreaddress = (from c in context.ExamVenues
                                                 join d in context.ExamCenters
                                                     on c.ExamCentreID equals d.ID
                                                 where c.ID == download.AllottedExamVenueID && d.ID == download.AllottedExamCentreID
                                                 select new
                                                 {
                                                     venuename = c.Name,
                                                     addline1 = c.AddressLine1,
                                                     addline2 = c.AddressLine2,
                                                     city = c.City,
                                                     state = c.State.Name,
                                                     pincode = c.PinCode
                                                 }).FirstOrDefault();
                        if (examcentreaddress != null)
                        {
                            lblexamaddress.Text = examcentreaddress.venuename + WebUtility.HtmlDecode("<br/>");
                            lblexamaddress.Text += examcentreaddress.addline1 + WebUtility.HtmlDecode("<br/>");
                            lblexamaddress.Text += string.IsNullOrEmpty(examcentreaddress.addline2) == false && !string.IsNullOrWhiteSpace(examcentreaddress.addline2) ? examcentreaddress.addline2 + WebUtility.HtmlDecode("<br/>") : "";
                            lblexamaddress.Text += examcentreaddress.city + WebUtility.HtmlDecode("<br/>");
                            lblexamaddress.Text += GetInitCap(examcentreaddress.state) + WebUtility.HtmlDecode(", ");
                            //lblexamaddress.Text += examcentreaddress.pincode.HasValue ? "Pin:- " + examcentreaddress.pincode.Value.ToString() : "NA";
                            lblexamaddress.Text += examcentreaddress.pincode.HasValue ? "Pin:- " + examcentreaddress.pincode.Value.ToString() : "";
                        }
                    }
                    try
                    {
                        String sql = "Select 'Batch: '+cast(batch as varchar)+'&nbsp;&nbsp;Sr No: '+Cast(s_no as varchar) as ref_no from e_fm where form_no = " + appid.ToString();
                        lboffrefno.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller(sql, new EConnect.Connections.SqlCon(), null, System.Data.CommandType.Text, false).ToString();
                    }
                    catch (Exception)
                    {
                        lboffrefno.Text = "Form Number: " + appid.ToString();
                    }

                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected String GetModuleName(EConnectContext context, Int64 courseExamApplicationID, DateTime examDate, Int32 examSessionID)
       {
        try
        {
            CourseExamApplication appl = context.CourseExamApplications.Find(courseExamApplicationID);
           
            //var x = (from k in context.ExamTimeTables where k.ExamID == newExam.examId && k.ExamFomDate == examDate && k.ExamSessiionID == examSessionID select  k.ModuleID);
            //var moduleName = (from d in context.CourseExamApplicationDetails
            //                  where (d.CourseExamApplicationID == courseExamApplicationID || d.CourseExamApplicationID == applmerge.CourseExamApplicationID) &&
            //                // d.ModuleID == (from k in context.ExamTimeTables where k.ExamID == newExam.examId && k.ExamFomDate == examDate && k.ModuleID == d.ModuleID && k.ExamSessiionID == examSessionID select k.ModuleID).FirstOrDefault()
            //                 //&& d.ModuleID
            //                 (from k in context.ExamTimeTables where k.ExamID == appl.ExamID && k.ModuleID.ToStr && k.ExamFomDate == examDate && k.ExamSessiionID == examSessionID select k.ModuleID).FirstOrDefault()                   
            //                  &&  d.Module.ModuleTypeID != practical &&
            //                 ( d.ResultGradeID != 183 || d.ResultGradeID !=7)
            //                  && (d.ExamID == 4903 || d.ExamID == 4246) 
            //                  //t.ExamID == 4247 
            //                  select d.Module.ShortName).FirstOrDefault();

            var moduleName = (from d in context.CourseExamApplicationDetails
                              join t in context.ExamTimeTables
                              on d.ExamID equals t.ExamID
                              where (d.CourseExamApplicationID == courseExamApplicationID) &&
                              d.ModuleID == t.ModuleID && t.Module.ModuleTypeID != practical &&
                              t.ExamFomDate == examDate &&
                              t.ExamSessiionID == examSessionID
                              && d.IsCanceled == null && d.ResultGradeID == null
                              select t.Module.ShortName).FirstOrDefault();

            if (moduleName != null)
                return moduleName.ToString();
            else 
                return "***";     
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowData()
    {
        try
        {
            tbl.Font.Bold = true;
            tbl.Font.Size = FontUnit.Point(10);
            Int64 applicationID = Convert.ToInt64(Request.QueryString["Appid"]);
            using (EConnectContext context = new EConnectContext())
            {
                //---------------------------------------------------------Exam Session-----------------------------------------------------------------
                var examSessions = (from s in context.ExamSessions
                                    orderby s.StartTime
                                    select s).ToList();
                CourseExamApplication appl = context.CourseExamApplications.Find(applicationID);

                          
                var examdate = (from r in context.ExamTimeTables
                                where r.ExamID == appl.ExamID//2676 
                                && r.Module.ModuleTypeID != practical
                                orderby r.ExamFomDate
                                select r.ExamFomDate).Distinct();

                TableRow tr = new TableRow();
                TableCell thc1 = new TableCell();
                thc1.Text = "Date";
                thc1.Style.Add("vertical-align", "middle");
                thc1.Width = Unit.Percentage(20);
                thc1.BorderWidth = Unit.Pixel(1);
                thc1.BorderColor = System.Drawing.Color.Black;
                thc1.Height = Unit.Pixel(41);
                thc1.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(thc1);
                int dateCount = examdate.Count();
                foreach (var edate in examdate.Distinct())
                {
                    TableCell tdRow1 = new TableCell();
                    tdRow1.Width = Unit.Percentage(80 / dateCount);
                    tdRow1.Style.Add("vertical-align", "middle");
                    //tdRow1.Style.Add("border-top", "0px");
                    tdRow1.BorderWidth = Unit.Pixel(1);
                    tdRow1.BorderColor = System.Drawing.Color.Black;
                    tdRow1.Text = edate.ToString("dd-MMM-yy");
                    tdRow1.Height = Unit.Pixel(41);
                    tdRow1.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow1);
                }
                tbl.Rows.Add(tr);
                foreach (ExamSession examSession in examSessions)
                {
                    TableRow trSession = new TableRow();

                    TableCell tdSession = new TableCell();
                    tdSession.Text = "<u><b>" + examSession.Name.ToUpper() + "</b></u>" + WebUtility.HtmlDecode("<br/>") + examSession.StartTime.ToString("HHmm") + "-" + examSession.EndTime.ToString("HHmm") + " hrs";
                    tdSession.Width = Unit.Percentage(15);
                    tdSession.Style.Add("vertical-align", "middle");
                    tdSession.Font.Bold = false;
                    tdSession.HorizontalAlign = HorizontalAlign.Center;
                    tdSession.BorderWidth = Unit.Pixel(1);
                    tdSession.BorderColor = System.Drawing.Color.Black;
                    tdSession.Style.Add("padding", "2px");
                    tdSession.Height = Unit.Pixel(41);
                    trSession.Cells.Add(tdSession);
                    tbl.Rows.Add(trSession);

                    //TableRow trSignature = new TableRow();
                    //TableCell tdSignature = new TableCell();
                    //tdSignature.Text = "Signature";
                    //tdSignature.Style.Add("vertical-align", "middle");
                    //tdSignature.Width = Unit.Percentage(20);
                    //tdSignature.HorizontalAlign = HorizontalAlign.Left;
                    //tdSignature.Height = Unit.Pixel(15);
                    //tdSignature.Style.Add("padding", "2px");
                    //trSignature.Cells.Add(tdSignature);
                    //tbl.Rows.Add(trSignature);

                     foreach (var edate in examdate.Distinct())
                    {
                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(80 / dateCount);
                        tdRow1.Style.Add("vertical-align", "middle");
                        tdRow1.Text = GetModuleName(context, applicationID, edate, examSession.ID);
                        tdRow1.Height = Unit.Pixel(41);
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tdRow1.BorderWidth = Unit.Pixel(1);
                        tdRow1.BorderColor = System.Drawing.Color.Black;
                        trSession.Cells.Add(tdRow1);

                        //TableCell tdSign = new TableCell();
                        //tdSign.Width = Unit.Percentage(80 / dateCount);
                        //tdSign.Style.Add("vertical-align", "middle");
                        //tdSign.Text = "";
                        //tdSign.Height = Unit.Pixel(15);
                        //tdSign.HorizontalAlign = HorizontalAlign.Center;
                        //trSignature.Cells.Add(tdSign);
                    }
                }
            };
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showofficeaddress()
    {
        try
        {
            using (EConnectContext ctx = new EConnectContext())
            {
                Session["OrgID"] = "1";
                Int32 orgID = Convert.ToInt32(Session["OrgID"]);
                var organisation = (from u in ctx.Organizations
                                    where u.ID == orgID
                                    select u).Single();
                tdHeaderBig.InnerText = organisation.Name;
                string m = organisation.MainHeading;
                string s = organisation.SubHeading;
                tdHeaderSmall.InnerText = string.Concat(m, s);
                tdheaderaddress.InnerText = organisation.AddressLine1 + " " + organisation.AddressLine2 + ", " + organisation.CityName + " - " + organisation.PinCode.ToString();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}