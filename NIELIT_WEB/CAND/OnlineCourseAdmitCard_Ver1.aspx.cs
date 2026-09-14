using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class CAND_OnlineCourseAdmitCard_Ver1 : BasePage
{
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && (!String.IsNullOrEmpty(Request.QueryString["Appid"])))
            {
              RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
            }

            if (!Page.IsPostBack)
            {
                showofficeaddress();
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
                //Lblctype.Text = currentCourse.Code;
                Int64 appid = Convert.ToInt64(Request.QueryString["Appid"]);
                //int appid = 5914069;
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    var download = (from c in context.CourseExamApplications
                                    join d in context.Candidates on c.CandidateID equals d.ID
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select new
                                    {
                                        Level = c.CourseID == 1 ? "O Level" : c.CourseID == 2 ? "A Level" : c.CourseID == 3 ? "B Level" :c.CourseID == 1213?"CHM-T O Level":"C Level",
                                        RegistrationNo = c.RegistrationNumber,
                                        RollNumber = c.RollNumber,
                                        CanName = d.Name,
                                        CanFatherName = d.FatherName,
                                        Dob = d.DateOfBirth,
                                        ExamName=c.Exam.Name.ToUpper()
                                         }).FirstOrDefault();

                    if (download != null)
                    {
                        Lblevel.Text = download.Level;
                        LbRegNo.Text = download.RegistrationNo.ToString();
                        LbRollNo.Text = download.RollNumber.ToString();
                        LbCname.Text = download.CanName;
                        Lbfgname.Text = download.CanFatherName;
                        Lbldob.Text = download.Dob.ToString("dd-MMM-yyyy");
                        imgPhotoBarcode.Src = "../Handlers/BarcodeHandler.ashx?Code=" +download.RegistrationNo+", "+download.RollNumber;
                        //imgPhotoBarcode.Src = "../Handlers/BarcodeHandler.ashx?Code="+"ghfhgfhfhghfjhgjhgjg";
                        Lbename.Text = download.ExamName;
                    }
                     var download1 = (from c in context.CourseExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select c).FirstOrDefault();
                    if(download1!=null)
                          ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download1.Candidate.Photo.BlobFile);

                    var data1 = (from m in context.CourseExamApplicationDetails where m.CourseExamApplicationID == appid select new { VenueCityName = m.VenueCityName }).FirstOrDefault();
                    if (data1 != null)
                        LblECenter.Text = data1.VenueCityName;
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

                //var examdate = (from f in context.CourseExamApplicationDetails
                //                where f.Module.ModuleTypeID != 3 && f.CourseExamApplicationID == applicationID && f.Module.ExamModeId==1
                //            orderby f.PracticalExamDate ascending, f.PracticalExamReportingTime descending
                //            select new
                //            {
                //                M_ShortName = f.Module.ShortName,
                //                ExamDate = f.PracticalExamDate,
                //                ReportingTime = f.PracticalExamReportingTime,
                //                ExamStartTime = f.PracticalExamBatchNumber,
                //                LoginId = f.OnlineExamLoginID,
                //                Vcode = f.VenueCode,
                //                Vname = f.VenueName,
                //                VAdrress = f.VenueAddress,

                //            }).ToList();


                //November_2024
                //string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                //SqlConnection con = new SqlConnection(constr);
                ////SqlConnection con = new SqlConnection("Data Source=10.246.112.177;Initial Catalog=NIELIT;Persist Security Info=True;User ID=shaukat; Password=Db4PareekshaUAT@9211$; MultipleActiveResultSets=True;Timeout = 120; Max Pool Size=1000");
                //string sqlQuery = "select d1.Online_Exam_Login_ID,d1.Venue_Code,d1.Venue_Name,d1.Venue_Address,d1.Exam_Date,d1.Rept_Time,d1.Start_Time,d2.Short_Name from Course_Exam_Schedule as d1 join module as d2 on d1.Module_ID=d2.ID where d1.Registration_Number='" + appl.RegistrationNumber + "' and d1.Exam_ID='" + appl.ExamID + "' and d2.Exam_Mode_ID=1 and d2.Module_Type_ID=1";
                ////string sqlQuery1 = "INSERT INTO Course_Exam_Application_Detail(Candidate_ID ,Registration_Number ,Module_Id,Fee_Amount,Is_UMC,Result_Grade_Id,Is_Canceled, Module_Ccde,Exam_Month,Exam_Year, Course_Id, Exam_Id,Absent_flag) VALUES ('" + Candidate_ID + "','" + Registration_Number + "','" + moduleid + "','0','0','" + IdGradeY + "','0','" + ModuleCode + "','" + exam.ExamMonth + "','" + exam.ExamYear + "','" + currentCourseID + "','" + exam.ID + "','N')";
                //con.Open();
                //SqlDataAdapter da = new SqlDataAdapter(sqlQuery, con);
                //DataTable dt = new DataTable();
                //da.Fill(dt);


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
                {
                    string sqlQuery = "select d1.Online_Exam_Login_ID,d1.Venue_Code,d1.Venue_Name,d1.Venue_Address,d1.Exam_Date,d1.Rept_Time,d1.Start_Time,d2.Short_Name from Course_Exam_Schedule as d1 join module as d2 on d1.Module_ID=d2.ID where d1.Registration_Number=@RegistrationNumber and d1.Exam_ID=@ExamID and d2.Exam_Mode_ID=1 and d2.Module_Type_ID=1";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        //November_2024
                        cmd.Parameters.AddWithValue("@RegistrationNumber", appl.RegistrationNumber);
                        cmd.Parameters.AddWithValue("@ExamID", appl.ExamID);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        showtableheader();
                        // int i = 1;

                        //foreach (var edate in examdate)
                        //{
                        //    TableRow tr = new TableRow();
                        //    tr.Font.Size = FontUnit.Medium;

                        //    TableCell tdRow1 = new TableCell();
                        //    tdRow1.Width = Unit.Percentage(10);
                        //    DateTime dt = Convert.ToDateTime(edate.ExamDate);
                        //    tdRow1.Text = dt.ToString("dd-MMM-yyyy");
                        //    tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        //    tdRow1.BorderWidth = Unit.Pixel(1);
                        //    tdRow1.Font.Bold = false;
                        //    tr.Cells.Add(tdRow1);

                        //    TableCell tdRow2 = new TableCell();
                        //    tdRow2.Width = Unit.Percentage(10);
                        //    tdRow2.Text = edate.ReportingTime;
                        //    tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        //    tdRow2.BorderWidth = Unit.Pixel(1);
                        //    tdRow2.Font.Bold = false;
                        //    tr.Cells.Add(tdRow2);

                        //    TableCell tdRow3 = new TableCell();
                        //    tdRow3.Width = Unit.Percentage(10);
                        //    tdRow3.Text = edate.ExamStartTime;
                        //    tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        //    tdRow3.BorderWidth = Unit.Pixel(1);
                        //    tdRow3.Font.Bold = false;
                        //    tr.Cells.Add(tdRow3);


                        //    TableCell tdRow4 = new TableCell();
                        //    tdRow4.Width = Unit.Percentage(10);
                        //    tdRow4.Text = edate.M_ShortName;
                        //    tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        //    tdRow4.BorderWidth = Unit.Pixel(1);
                        //    tdRow4.Font.Bold = false;
                        //    tr.Cells.Add(tdRow4);

                        //    TableCell tdRow5 = new TableCell();
                        //    tdRow5.Width = Unit.Percentage(10);
                        //    tdRow5.Text = edate.LoginId;
                        //    tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        //    tdRow5.BorderWidth = Unit.Pixel(1);
                        //    tdRow5.Font.Bold = false;
                        //    tr.Cells.Add(tdRow5);

                        //    TableCell tdRow6 = new TableCell();
                        //    tdRow6.Width = Unit.Percentage(60);
                        //    tdRow6.Style.Add("Padding-left","5px");
                        //    tdRow6.Text = "<b>Venue Code</b>: " + edate.Vcode + "<br/> <b>Name</b>: " + edate.Vname + "<br/> <b>Adrress </b>: " + edate.VAdrress;
                        //    tdRow6.HorizontalAlign = HorizontalAlign.Left;
                        //    tdRow6.BorderWidth = Unit.Pixel(1);
                        //    tdRow6.Font.Bold = false;
                        //    tr.Cells.Add(tdRow6);

                        //    tbl.Rows.Add(tr);
                        //    i++;
                        //}
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow tr = new TableRow();
                            tr.Font.Size = FontUnit.Medium;

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(10);
                            DateTime dt1 = Convert.ToDateTime(dt.Rows[i]["Exam_Date"].ToString());
                            tdRow1.Text = dt1.ToString("dd-MMM-yyyy");
                            tdRow1.HorizontalAlign = HorizontalAlign.Center;
                            tdRow1.BorderWidth = Unit.Pixel(1);
                            tdRow1.Font.Bold = false;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            tdRow2.Text = dt.Rows[i]["Rept_Time"].ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tdRow2.BorderWidth = Unit.Pixel(1);
                            tdRow2.Font.Bold = false;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(10);
                            tdRow3.Text = dt.Rows[i]["Start_Time"].ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Center;
                            tdRow3.BorderWidth = Unit.Pixel(1);
                            tdRow3.Font.Bold = false;
                            tr.Cells.Add(tdRow3);


                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(10);
                            tdRow4.Text = dt.Rows[i]["Short_Name"].ToString();
                            tdRow4.HorizontalAlign = HorizontalAlign.Center;
                            tdRow4.BorderWidth = Unit.Pixel(1);
                            tdRow4.Font.Bold = false;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = dt.Rows[i]["Online_Exam_Login_ID"].ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tdRow5.BorderWidth = Unit.Pixel(1);
                            tdRow5.Font.Bold = false;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(60);
                            tdRow6.Style.Add("Padding-left", "5px");
                            tdRow6.Text = "<b>Venue Code</b>: " + dt.Rows[i]["Venue_Code"].ToString() + "<br/> <b>Name</b>: " + dt.Rows[i]["Venue_Name"].ToString() + "<br/> <b>Adrress </b>: " + dt.Rows[i]["Venue_Address"].ToString();
                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                            tdRow6.BorderWidth = Unit.Pixel(1);
                            tdRow6.Font.Bold = false;
                            tr.Cells.Add(tdRow6);

                            tbl.Rows.Add(tr);
                            // i++;
                        }
                    }
                }
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
            tcCol.Width = Unit.Percentage(10);
            tcCol.Text = "Exam Date";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            tcCol.BorderWidth = Unit.Pixel(1);
            tcCol.Font.Bold = true;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(10);
            tcCol1.Text = "Reporting Time";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.BorderWidth = Unit.Pixel(1);
            tcCol1.Font.Bold = true;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            tcCol2.Text = " Exam Start Time";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.BorderWidth = Unit.Pixel(1);
            tcCol2.Font.Bold = true;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Module";
            tcCol3.BorderWidth = Unit.Pixel(1);
            tcCol3.Font.Bold = true;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            tcCol4.Text = "Login ID";
            tcCol4.BorderWidth = Unit.Pixel(1);
            tcCol4.Font.Bold = true;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(60);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Venue Detail";
            tcCol5.BorderWidth = Unit.Pixel(1);
            tcCol5.Font.Bold = true;
            th.Cells.Add(tcCol5);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}