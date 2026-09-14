using System;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class CertificateAdmitCardVersion6 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && (!String.IsNullOrEmpty(Request.QueryString["Appid"])))
            {
                RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
            }


            //int appid = 5914069;
            //int id = 5;

            //if (appid == 5914069 && id == 5)
            //{
            //    RenderPage(5);
            //}
            else
            {
                //Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                //Response.End();
                //return;
            }
            if (!Page.IsPostBack)
            {
                showofficeaddress();
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
                Lblctype.Text = currentCourse.Code;
                Int64 appid = Convert.ToInt64(Request.QueryString["Appid"]);
                //int appid = 5914069;
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var download = (from c in context.CertificateExamApplications
                                    where c.ID == appid && c.FinalSubmitted == true && c.RollNumber != null
                                    select c).FirstOrDefault();

                    if (download != null)
                    {

                        Lblrno.Text = download.RollNumber;
                        //Lbrollno.Text = download.RollNumber;
                        Lbcname.Text = GetInitCap(download.Name);
                        LblGender.Text = download.Gender;
                        //Lbcname1.Text = GetInitCap(download.Name);
                        if (string.IsNullOrEmpty(download.GuardianName) == true && string.IsNullOrWhiteSpace(download.GuardianName) == true)
                        {
                            trfathername.Visible = true;
                            trmothername.Visible = true;
                            trguardian.Visible = false;
                            if (string.IsNullOrEmpty(download.FatherName) == false && !string.IsNullOrWhiteSpace(download.FatherName))
                                Lbfname.Text = GetInitCap(download.FatherName);
                            else
                                Lbfname.Text = "NA";
                            if (string.IsNullOrEmpty(download.MotherName) == false && string.IsNullOrWhiteSpace(download.MotherName) == false)
                                Lbmname.Text = GetInitCap(download.MotherName);
                            else
                                Lbmname.Text = "NA";
                        }
                        else
                        {
                            Lbgname.Text = string.IsNullOrEmpty(download.GuardianName) == false && string.IsNullOrWhiteSpace(download.GuardianName) == false ? GetInitCap(download.GuardianName) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trguardian.Visible = true;
                        }
                        imgcandphoto.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])download.Photo);
                        Lblexamcode.Text = (!string.IsNullOrEmpty(download.ExamCentreName)) ? download.ExamCentreName.ToUpper() : "";
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
                            Lbename.Text = " ( " + currentCourse.Code + "  " + GetInitCap(download.Exam.ExaminationCycle.Name) + " ) " + GetInitCap(download.Exam.Name);
                        }
                        else
                        {
                            Lbename.Text = "";
                        }
                        Lbedate.Text = (download.DateOfExam.HasValue) ? download.DateOfExam.Value.ToString("dd-MMM-yyyy") : "";
                        Lblbatchno.Text = (!string.IsNullOrEmpty(download.ExamBatchNumber)) ? GetInitCap(download.ExamBatchNumber) : "";
                        Lbreptime.Text = (!string.IsNullOrEmpty(download.ReportingTime)) ? download.ReportingTime.ToUpper() : "";
                        Lbccode.Text = (!string.IsNullOrEmpty(download.ExamCentreName)) ? download.ExamCentreName.ToUpper() : "";
                        lblexamaddress.Text = (!string.IsNullOrEmpty(download.ExamCentreAddress)) ? download.ExamCentreAddress.ToUpper() : "";
                        if (download.CourseID == 5 || download.CourseID == 75 || download.CourseID == 175 || download.CourseID == 1226)
                        {
                            lbminutes.Text = "60 MIN.";
                            //lblmintime.Text = "45";
                        }
                        else
                        {
                            lbminutes.Text = "90 MIN.";
                            //lblmintime.Text = "60";
                        }
                        DateTime repTime = Convert.ToDateTime(Lbreptime.Text);
                        

                        if (repTime != DateTime.MinValue)
                        {
                           
                            string count = Convert.ToString(download.RollNumber.Substring(9));
                            Int64 countTime = Convert.ToInt64(count) % 45;

                            TimeSpan ts = TimeSpan.FromMinutes(countTime);
                                                    
                            Lbclosing.Text = repTime.AddMinutes(-15).ToShortTimeString();
                            //Lbreporting.Text = repTime.AddMinutes(-60).ToShortTimeString();
                            Lbreporting.Text = repTime.AddHours(-1).Add(ts).ToShortTimeString();
                        }

                        //{ Lbreporting.Text = reporting; Lbclosing.Text = closing; }

                        //if (download.ExamBatchNumber == "1")
                        //{ Lbreporting.Text = reporting; Lbclosing.Text = closing; }
                        //else if (download.ExamBatchNumber == "2")
                        //{ Lbreporting.Text = reporting; Lbclosing.Text = closing; }
                        //else if (download.ExamBatchNumber == "3")
                        //{ Lbreporting.Text = reporting; Lbclosing.Text = closing; }
                        //else if (download.ExamBatchNumber == "4")
                        //{ Lbreporting.Text = reporting; Lbclosing.Text = closing; }
                        //else if (download.ExamBatchNumber == "5")
                        //{ Lbreporting.Text = reporting; Lbclosing.Text = closing; }
                        else
                        { Lbreporting.Text = ""; Lbclosing.Text = ""; }


                        //if (download.ExamBatchNumber == "1")
                        //{ Lbreporting.Text = "08:30 AM"; Lbclosing.Text = "08:45 AM"; }
                        //else if (download.ExamBatchNumber == "2")
                        //{ Lbreporting.Text = "11:00 AM"; Lbclosing.Text = "11:15 AM"; }
                        //else if (download.ExamBatchNumber == "3")
                        //{ Lbreporting.Text = "01:30 PM"; Lbclosing.Text = "01:45 PM"; }
                        //else if (download.ExamBatchNumber == "4")
                        //{ Lbreporting.Text = "03:30 PM"; Lbclosing.Text = "03:45 PM"; }
                        //else if (download.ExamBatchNumber == "5")
                        //{ Lbreporting.Text = "05:00 PM"; Lbclosing.Text = "05:15 PM"; }
                        //else
                        //{ Lbreporting.Text = ""; Lbclosing.Text = ""; }

                        //if (download.ExamBatchNumber == "1")
                        //{ Lbreporting.Text = "08:45 AM"; Lbclosing.Text = "09:00 AM"; }
                        //else if (download.ExamBatchNumber == "2")
                        //{ Lbreporting.Text = "10:30 AM"; Lbclosing.Text = "10:45 AM"; }
                        //else if (download.ExamBatchNumber == "3")
                        //{ Lbreporting.Text = "12:45 PM"; Lbclosing.Text = "01:00 PM"; }
                        //else if (download.ExamBatchNumber == "4")
                        //{ Lbreporting.Text = "02:30 PM"; Lbclosing.Text = "02:45 PM"; }
                        //else if (download.ExamBatchNumber == "5")
                        //{ Lbreporting.Text = "04:30 PM"; Lbclosing.Text = "04:45 PM"; }
                        //else
                        //{ Lbreporting.Text = ""; Lbclosing.Text = ""; }

                        LblDisability.Text = (download.IsDisability.HasValue && download.IsDisability == true) ? download.DisabilityType.Name : "No";
 
                        //if (download.BatchItemID.HasValue)
                        //    Lblbatchcode.Text = context.BatchItems.Find(download.BatchItemID.Value).Batch.Number + "/" + download.BatchItemID.Value;
                        //else
                        //    Lblbatchcode.Text = "NA";
                        //Lbexcode.Text = (!string.IsNullOrEmpty(download.ExamCentreName)) ? download.ExamCentreName.ToUpper() : "";
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