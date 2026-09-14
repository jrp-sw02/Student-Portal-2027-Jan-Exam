using System;
using System.Data.Objects;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class ViewResult : BasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["Appid"]) && !String.IsNullOrEmpty(Request.QueryString["ExamId"]))
                {
                    RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                    Response.End();
                    return;
                }
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
                string appType = Convert.ToString(enmApplicantType.Direct);
                Course currentCourse = context.Courses.Find(currentCourseID);
                Int64 appid = Convert.ToInt64(Request.QueryString["Appid"]);
                Int32 examID = Convert.ToInt32(Request.QueryString["ExamId"]);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    Lblcourse.Text = "RESULT CARD FOR" + " " + currentCourse.Name.ToUpper();
                   
                    var result = (from a in context.CertificateExamApplications
                                  where a.ID == appid 
                                  select new
                                  {
                                      name = a.Name,
                                      Appid = a.ID,
                                      fname = a.FatherName,
                                      mname = a.MotherName,
                                      gname= a.GuardianName,
                                      doe = a.DateOfExam,
                                      rollno = a.RollNumber,
                                      examname = a.Exam.Name,
                                      studresult = a.ResultGrade,
                                      resultDate = a.ResultUpdatedOn,
                                      apptypeId = a.ApplicantTypeID,
                                      instid = a.InstituteID,
                                      instname = a.Institute.Name,
                                      examcentre = a.ExamCentreName,
                                      salutation = a.Candidate.Salutation,
                                      courseid = a.CourseID,
                                      examcyclename= a.Exam.ExaminationCycle.Name
                                  }).FirstOrDefault();

                    if (result != null)
                    {
                        if (result.studresult != null)
                        {
                            if (result.doe.HasValue)
                                Lblexamdate.Text = result.doe.Value.ToString("dd-MMM-yyyy");
                            else
                                Lblexamdate.Text = "NA";
                            LblRollno.Text = (!String.IsNullOrEmpty(result.rollno)) ? result.rollno.ToString() : " NA ";
                            Lblname.Text = result.salutation + " " + GetInitCap(result.name);
                            if (string.IsNullOrEmpty(result.gname) == true && string.IsNullOrWhiteSpace(result.gname) == true)
                            {
                                trfathername.Visible = true;
                                trmothername.Visible = true;
                                trgname.Visible = false;
                                if (string.IsNullOrEmpty(result.fname) == false && !string.IsNullOrWhiteSpace(result.fname))
                                    Lbfname.Text = "Mr. " + GetInitCap(result.fname);
                                else
                                    Lbfname.Text = "NA";
                                if (string.IsNullOrEmpty(result.mname) == false && string.IsNullOrWhiteSpace(result.mname) == false)
                                    Lbmname.Text = "Mrs. " + GetInitCap(result.mname);
                                else
                                    Lbmname.Text = "NA";
                            }
                            else
                            {
                                Lgname.Text = string.IsNullOrEmpty(result.gname) == false && string.IsNullOrWhiteSpace(result.gname) == false ? GetInitCap(result.gname) : "NA";
                                trfathername.Visible = false;
                                trmothername.Visible = false;
                                trgname.Visible = true;
                            }
                            if (result.apptypeId == Convert.ToInt32(enmApplicantType.Institute))
                            {
                                if (result.instid.HasValue)
                                {
                                    var accredidationdetails = (from c in context.AccreditationDetails
                                                               where c.InstituteID == result.instid && c.CourseID == currentCourse.ID
                                                               select c).FirstOrDefault();
                                    if (accredidationdetails != null)
                                    {
                                        Lblcc.Text = accredidationdetails.AccreditationNumber.ToUpper();
                                        Lbliname.Text = GetInitCap(result.instname);
                                    }
                                    else
                                    {
                                        Lblcc.Text = "NA";
                                        Lbliname.Text = "NA";
                                    }
                                }
                                else
                                {
                                    Lblcc.Text = "NA";
                                    Lbliname.Text = "NA";
                                }
                            }
                            else
                            {
                                Lblcc.Text = appType;
                                Lbliname.Text = "--";
                            }
                            Int32 occurance = 0;
                            string occuranceno = "";
                            if (result.doe.HasValue)
                            {
                                occurance = (int)Math.Ceiling(result.doe.Value.Day / 7.0);
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
                            }
                      
                            Lblexam.Text = GetInitCap(result.examname)+"(" + currentCourse.Code + " - " + GetInitCap(result.examcyclename) + ")"  ;
                            //Lblexam.Text = "(" + currentCourse.Code + "  " + GetInitCap(result.examcyclename) + ")" + "-" + occuranceno + " Sat, " + GetInitCap(result.examname);
                            var grade = result.studresult;
                            if (grade.IsPassed.ToString() == "True")
                            { Lblresult.Text = "Grade:- " + grade.Code + (grade.IsPassed.ToString() == "True" ? " (Pass) " : "(" + grade.Description + ")"); }
                            else
                            { Lblresult.Text = grade.Code + (grade.IsPassed.ToString() == "True" ? " (Pass) " : "(" + grade.Description + ")"); }
                            LblResultDate.Text = result.resultDate.GetValueOrDefault().ToString("dd-MMM-yyyy");

                            Lblexamcentre.Text = (!String.IsNullOrEmpty(result.examcentre)) ? result.examcentre : "NA";
                            ShowLegends(result.apptypeId, currentCourseID, currentCourse.CourseCategoryID, examID);
                        }
                    }

                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowLegends(Int32 appTypeID, Int32 courseID, Int32 CourseCategoryID, Int32 ResultExamID)
    {
        try
        {
            Table tbl = new Table();       
            tbl.Width = Unit.Percentage(100);
            tbl.CellSpacing = 0;
            tbl.BorderColor = System.Drawing.Color.Black;
            tbl.BorderWidth = Unit.Pixel(1);
            tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;          
          
            string[] heading = { "Grade", "Marks Range (in %)", "Remarks" };
            using (EConnectContext context = new EConnectContext())
            {
                Int32[] notInGrades = {24};
                Int32 ResultVersionID = context.Exams.Where(s => s.ID == ResultExamID).FirstOrDefault().ResultGradeVersionID.Value;
                var grade = (from g in context.ResultGrades
                             where g.CourseCategoryID == CourseCategoryID && !notInGrades.Contains(g.ID) && g.VersionID == ResultVersionID
                             orderby g.Code
                             select new
                             {
                                 grade = g.Code,
                                 description = g.Description,
                                 legend1 = g.PercentageFrom,
                                 legend2 = g.PercentageTo
                             }).ToList();

                if (courseID != 5 && CourseCategoryID == 2) //BCC
                { grade = grade.Where(s => s.grade != "E").ToList(); }
                if (grade.Count() >= 0)
                {
                    int rowCounter = 0;
                    for (rowCounter = 0; rowCounter < 3; rowCounter++)
                    {
                        TableRow tr = new TableRow();

                        tbl.Rows.Add(tr);

                        TableCell tdHeading = new TableCell();
                        tdHeading.Text = heading[rowCounter].ToString();
                        tdHeading.HorizontalAlign = HorizontalAlign.Left;
                        tdHeading.Font.Bold = true;
                        tdHeading.Width = Unit.Percentage(20);
                        tdHeading.BorderColor = System.Drawing.Color.Black;
                        tdHeading.BorderWidth = Unit.Pixel(1);
                        tdHeading.VerticalAlign = VerticalAlign.Top;
                        tr.Cells.Add(tdHeading);
                        foreach (var result in grade)
                        {
                            TableCell tdGrade = new TableCell();
                            tdGrade.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdGrade);
                            tdGrade.Font.Bold = false;
                            tdGrade.BorderColor = System.Drawing.Color.Black;
                            tdGrade.BorderWidth = Unit.Pixel(1);
                            if (rowCounter == 0)
                            {
                                tdGrade.Font.Bold = true;
                                tdGrade.Text = result.grade.ToUpper();
                            }
                            else if (rowCounter == 1)
                            {
                                if ( result.legend1 > 0 && result.legend2 > 0)
                                { tdGrade.Text = result.legend1 + " to " + result.legend2; }
                                else
                                { tdGrade.Text = result.grade; }
                            }
                            else
                            { tdGrade.Text = (result.description); }
                            tdGrade.VerticalAlign = VerticalAlign.Top;
                        }
                    }
                }
                tdLegends.Controls.Add(tbl);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }    
}