using System;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class CAND_CoursePracticalAdmitCard : BasePage
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
                Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
                Response.End();
                return;
            }
           
            if (!Page.IsPostBack)
            {
                showofficeaddress();
                tbl.BorderWidth = Unit.Pixel(1);
                tbl.BorderColor = System.Drawing.Color.Black;
                tbl.CellPadding = 2;
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
                                    where c.ID == appid && c.FinalSubmitted == true && c.PracticalOfficeRefNumber != null && c.NumberOfPracticalModulesApplied !=0
                                    select c).FirstOrDefault();

                    if (download != null)
                    {

                       // Lbename.Text = (!string.IsNullOrEmpty(download.Exam.Name.ToUpper())) ? "CANDIDATE ADMIT CARD <br /> VALID FOR " + download.Exam.Name.ToUpper() + "  EXAMINATION ONLY RESCHEDULE IN SEPTEMBER 2021" : "";
                        Lbcname.Text = download.Candidate.Salutation + GetInitCap(download.Candidate.Name);
                        Lbcname1.Text = download.Candidate.Salutation + GetInitCap(download.Candidate.Name);
                        Lbregno.Text = download.RegistrationNumber.ToString();
                        lboffrefno.Text = !String.IsNullOrEmpty(download.PracticalOfficeRefNumber) ? download.PracticalOfficeRefNumber : "";
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
                        Lbinstname.Text = download.PracticalInstituteName;
                        Lbinstaddress.Text = download.PracticalInstituteAddress;
                        // Code No 0001 Start:  Photo dispay Code has been added on 15/01/2020 as per request of Sh. G Bhasker JD(Admin)
                        ImgCandidatePhoto_prac.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download.Candidate.Photo.BlobFile);
                        ImgCandidatesignature_prac.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download.Candidate.Signature.BlobFile);
                        // Code No 0001 End: Photo dispay Code has been added on 15/01/2020 as per request of Sh. G Bhasker JD(Admin)

                        // to check that only paricular course papers are displayed.
                        //if (download.CourseID == 1)
                        //{
                        //    trolevel.Visible = true;
                        //}
                        //else if (download.CourseID == 2)
                        //{
                        //    tralevel.Visible = true;
                        //}
                        //else if (download.CourseID == 3)
                        //{
                        //    trblevel.Visible = true;
                        //}
                        //else if (download.CourseID == 4)
                        //{
                        //    trclevel1.Visible = true;
                        //    trclevel2.Visible = true;
                        //}
                        //else
                        //{
                        //    trolevel.Visible = false;
                        //    tralevel.Visible = false;
                        //    trblevel.Visible = false;
                        //    trclevel1.Visible = false;
                        //    trclevel2.Visible = false;
                        //}
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
                tdheaderaddress.InnerText = organisation.AddressLine1 + ", " + organisation.AddressLine2 + ", " + organisation.CityName + " - " + organisation.PinCode.ToString();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                CourseExamApplication appl = context.CourseExamApplications.Find(applicationID);

                //var applmerge = (from m in context.CourseExamApplications
                //                 join c in context.CourseExamApplicationDetails on
                //                 new { m.RegistrationNumber, m.ExamID } equals new { c.RegistrationNumber, c.ExamID }
                //                 where m.RegistrationNumber == appl.RegistrationNumber && c.ExamYear == 2020 && c.ExamMonth == 7
                //                 select c).FirstOrDefault();

                //if (applmerge == null)
                //{
                //    var examdate1 = (from c in context.CourseExamApplications
                //                    join f in context.CourseExamApplicationDetails
                //                    on c.ID equals f.CourseExamApplicationID
                //                    where f.Module.ModuleTypeID == practical && (f.CourseExamApplicationID == applicationID ) &&
                //                    (f.ResultGradeID != 183 || f.ResultGradeID != 7)
                //                    orderby f.PracticalExamDate ascending, f.PracticalExamReportingTime descending
                //                    select new
                //                    {
                //                        modulename = f.Module.ShortName,
                //                        level = c.Course.Code,
                //                        peracticaldate = f.PracticalExamDate,
                //                        batch = f.PracticalExamBatchNumber,
                //                        reptime = f.PracticalExamReportingTime
                //                    }).ToList();

                var examdate = (from c in context.CourseExamApplications
                                join f in context.CourseExamApplicationDetails
                                on c.ID equals f.CourseExamApplicationID
                                where f.Module.ModuleTypeID == practical && f.CourseExamApplicationID == applicationID
                                orderby f.PracticalExamDate ascending ,f.PracticalExamReportingTime descending
                                select new
                                {
                                    modulename= f.Module.ShortName,
                                    level = c.Course.Code,
                                    peracticaldate = f.PracticalExamDate,
                                    batch = f.PracticalExamBatchNumber,
                                    reptime = f.PracticalExamReportingTime
                                }).ToList();

                    showtableheader();
                    int i = 1;

                    foreach (var edate in examdate)
                    {
                        TableRow tr = new TableRow();
                        tr.Font.Size = FontUnit.Medium;

                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(15);
                        tdRow1.Text = edate.level + " / " + edate.modulename;
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tdRow1.BorderWidth = Unit.Pixel(1);
                        tdRow1.Font.Bold = false;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(15);
                        tdRow2.Text = edate.batch;
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tdRow2.BorderWidth = Unit.Pixel(1);
                        tdRow2.Font.Bold = false;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(15);
                        if (edate.peracticaldate.HasValue)
                        tdRow4.Text = edate.peracticaldate.Value.ToString("dd-MMM-yyyy");
                        // tdRow4.Text = "2018-Jan-31";
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tdRow4.BorderWidth = Unit.Pixel(1);
                        tdRow4.Font.Bold = false;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(15);
                        tdRow3.Text = edate.reptime;
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tdRow3.BorderWidth = Unit.Pixel(1);
                        tdRow3.Font.Bold = false;
                        tr.Cells.Add(tdRow3);

                        tbl.Rows.Add(tr);
                        i++;
                    }             
              //  }
                //else
                //{
                    
                //    var applmerge1 = (from m in context.CourseExamApplications
                //                     join c in context.CourseExamApplicationDetails on
                //                     new { m.RegistrationNumber, m.ExamID } equals new { c.RegistrationNumber, c.ExamID }
                //                     where m.RegistrationNumber == appl.RegistrationNumber && c.ExamYear == 2021 && c.ExamMonth == 1 && c.PracticalExamBatchNumber != null 
                //                     select c).FirstOrDefault();

                //    Int64? addtinalAppId;
                //    if (applmerge1 != null)
                //    {
                //         addtinalAppId = applmerge1.CourseExamApplicationID;
                //    }
                //    else
                //    {
                //        addtinalAppId = applmerge.CourseExamApplicationID;
                //    }
                //        var examdate2 = (from c in context.CourseExamApplications
                //                         join f in context.CourseExamApplicationDetails
                //                         on c.ID equals f.CourseExamApplicationID
                //                         where f.Module.ModuleTypeID == practical && (f.CourseExamApplicationID == applicationID || f.CourseExamApplicationID == addtinalAppId) &&
                //                         (f.ResultGradeID != 183 || f.ResultGradeID != 7)
                //                         orderby f.PracticalExamDate ascending, f.PracticalExamReportingTime descending
                //                         select new
                //                         {
                //                             modulename = f.Module.ShortName,
                //                             level = c.Course.Code,
                //                             peracticaldate = f.PracticalExamDate,
                //                             batch = f.PracticalExamBatchNumber,
                //                             reptime = f.PracticalExamReportingTime
                //                         }).ToList();
                    
                //    showtableheader();
                //    int i = 1;

                //    foreach (var edate in examdate2)
                //    {
                //        TableRow tr = new TableRow();
                //        tr.Font.Size = FontUnit.Medium;

                //        TableCell tdRow1 = new TableCell();
                //        tdRow1.Width = Unit.Percentage(15);
                //        tdRow1.Text = edate.level + " / " + edate.modulename;
                //        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                //        tdRow1.BorderWidth = Unit.Pixel(1);
                //        tdRow1.Font.Bold = false;
                //        tr.Cells.Add(tdRow1);

                //        TableCell tdRow2 = new TableCell();
                //        tdRow2.Width = Unit.Percentage(15);
                //        tdRow2.Text = edate.batch;
                //        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                //        tdRow2.BorderWidth = Unit.Pixel(1);
                //        tdRow2.Font.Bold = false;
                //        tr.Cells.Add(tdRow2);

                //        TableCell tdRow4 = new TableCell();
                //        tdRow4.Width = Unit.Percentage(15);
                //        if (edate.peracticaldate.HasValue)
                //        tdRow4.Text = edate.peracticaldate.Value.ToString("dd-MMM-yyyy");
                //        // tdRow4.Text = "2018-Jan-31";
                //        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                //        tdRow4.BorderWidth = Unit.Pixel(1);
                //        tdRow4.Font.Bold = false;
                //        tr.Cells.Add(tdRow4);

                //        TableCell tdRow3 = new TableCell();
                //        tdRow3.Width = Unit.Percentage(15);
                //        tdRow3.Text = edate.reptime;
                //        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                //        tdRow3.BorderWidth = Unit.Pixel(1);
                //        tdRow3.Font.Bold = false;
                //        tr.Cells.Add(tdRow3);

                //        tbl.Rows.Add(tr);
                //        i++;
                //    }
                //}
                               
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showtableheader()
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.Font.Size = FontUnit.Point(10);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(15);
            tcCol.Text = "स्तर / पीआर-कोड / LEVEL / PR-CODE";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            tcCol.BorderWidth = Unit.Pixel(1);
            tcCol.Font.Bold = true;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(15);
            tcCol1.Text = "बैच / BATCH ";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.BorderWidth = Unit.Pixel(1);
            tcCol1.Font.Bold = true;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(12);
            tcCol2.Text = "दिनांक / DATE";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.BorderWidth = Unit.Pixel(1);
            tcCol2.Font.Bold = true;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(12);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "समय-स्लॉट / TIME-SLOT";
            tcCol3.BorderWidth = Unit.Pixel(1);
            tcCol3.Font.Bold = true;
            th.Cells.Add(tcCol3);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}