using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class WEB_ViewCourseResult : BasePage
{

    Int64 entityID;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            entityID = Convert.ToInt64(Request.QueryString["CandidateID"]);

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["RegNo"]) && !String.IsNullOrEmpty(Request.QueryString["ExamId"]))
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

    private DataTable GetData(Int64 currentCourseID, Int64 registrationNumber, Int64 examID )
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

      /*  string sql = " select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
                       " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
                       " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
                       " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
                       " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade, " +
                       " case  when  (cad.Module_ID  not in (929,930,931,932,938,939,940,941,1151,1152,1153,1154)  and Module_Type_ID =1)  then Marks_Total else cad.Theory_marks_out_of_100_new_pattern end as TheoryMarks," +
                       " case  when (cad.Module_ID not in (933,934,935,936,956,957,958,959,1155,1156,1157,1158)  and Module_Type_ID = 3 ) then Marks_Total else cad.practical_marks_out_of_100_new_pattern end as  PracticalMarks," +
                       " cad.Marks_Total as WeightedMarks from " +
                       " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
                       " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
                       " and rg.Code <>'$'and cad.Module_ID not in (933,934,935,936,956,957,958,959,1155,1156,1157,1158)   and cad.Registration_Number ='" + registrationNumber + "'" +
                       "  and cad.Exam_ID = '" + examID + "'  and cad.Course_ID ='" + currentCourseID + "' " +
                       " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";
*/

 string sql =
                        " SELECT cad.Course_ID CourseID " +
                        " ,m.id AS ID " +
                        " ,upper(m.Name) AS name " +
                        " ,m.Short_Name AS Code " +
                        " ,m.Module_Type_ID AS ModuleTypeID " +
                        " ,m.Selection_Type_ID AS SelectionTypeID " +
                        " ,m.Elective_Group AS ElectiveGroup " +
                        " ,mt.Name + ( " +
                        " CASE " +
                        " WHEN m.Module_Type_ID = 1 " +
                        " AND m.Selection_Type_ID = 1 " +
                        " AND Sub_Number = 0 " +
                        " THEN '(Comp.)' " +
                        " WHEN m.Module_Type_ID = 1 " +
                        " AND m.Selection_Type_ID > 1 " +
                        " AND Sub_Number > 0 " +
                        " THEN '(Elect.)' " +
                        " ELSE '' " +
                        " END " +
                        " ) AS MType " +
                        " ,isnull(e.Name, 'NA') AS doexam " +
                        " ,rg.Description AS Result " +
                        " ,rg.Code AS Grade " +
                        " ,CASE " +
                        " WHEN ( isnull(m.TheoryWithPractical,0) =0 " +
                        " AND " +
                        " Module_Type_ID = 1 " +
                        " ) " +
                        " THEN cad.Marks_Total " +
                        " ELSE cad.Theory_marks_out_of_100_new_pattern " +
                        " END AS TheoryMarks " +
                        " ,CASE " +
                        " WHEN ( " +
                        " isnull(m.TheoryWithPractical,0) =0 " +
                        " AND Module_Type_ID = 3 " +
                        " ) " +
                        " THEN cad.Marks_Total " +
                        " ELSE cad.practical_marks_out_of_100_new_pattern " +
                        " END AS PracticalMarks " +
                        " ,cad.Marks_Total AS WeightedMarks " +
                        " FROM Module_Type mt " +
                        " ,module m " +
                        " ,Result_Grading rg " +
                        " ,Course_Exam_Application_Detail cad " +
                        " LEFT OUTER JOIN exam e ON cad.Exam_ID = e.id " +
                        " WHERE cad.Module_ID = m.id " +
                        " AND cad.Result_Grade_ID = rg.id " +
                        " AND m.Module_Type_ID = mt.ID " +
                        " AND rg.Code <> '$' " +
                        " and  ( iif " +
                        " (isnull(m.TheoryWithPractical,0)<>0 " +
                        " and m.Module_Type_ID =3 " +
                        " ,0,1)=1 " +
                        " ) " +
                        " " +
                        " AND cad.Registration_Number = '" + registrationNumber + "' " +
                        " AND cad.Exam_ID = '" + examID + "' " +
                        " AND cad.Course_ID = '" + currentCourseID + "' " +
                        " ORDER BY m.Module_Type_ID " +
                        " ,m.Selection_Type_ID " +
                        " ,m.ID ";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }


    protected void RenderPage(Int32 currentCourseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string appType = Convert.ToString(enmApplicantType.Direct);
                StringBuilder sb= new StringBuilder();
                Course currentCourse = context.Courses.Find(currentCourseID);
                Int64 registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
                Int32 examID = Convert.ToInt32(Request.QueryString["ExamId"]);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Lblcourse.Text = "RESULT CARD FOR" + " " + currentCourse.Name.ToUpper();
                    Lblccname.Text = GetInitCap(currentCourse.Name) + " (" + currentCourse.CourseCategory.Name + ")";
                    var result = (from c in context.CourseExamApplicationDetails
                                  where c.RegistrationNumber == registrationNumber && c.ResultGradeID != null && c.CourseID == currentCourseID && c.ExamID == examID
                                  select new
                                  {
                                      name = c.Candidate.Name,
                                      Appid = c.ID,
                                      fname = c.Candidate.FatherName,
                                      mname = c.Candidate.MotherName,
                                      gname = c.Candidate.GuardianName,
                                      rollno = c.RollNumber,
                                      examname = c.Exam.Name,
                                      ResultDeclaredOn = c.Exam.DateOfPublishingOfResult,
                                      salutation = c.Candidate.Salutation,
                                      remark = (!string.IsNullOrEmpty(c.carryforwardremarks)) ? c.carryforwardremarks : "N/A"
                                  }).FirstOrDefault();
                    if (result != null)
                    {
                        LblRollno.Text = result.rollno.ToString();
                        Lblname.Text = result.salutation + " " + GetInitCap(result.name);
                        LblResultDate.Text = "Declared on " + result.ResultDeclaredOn.Value.ToString("dd-MMM-yyyy");
                        if (string.IsNullOrEmpty(result.fname) == false && string.IsNullOrWhiteSpace(result.fname) == false)
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
                            Lgname.Text = string.IsNullOrWhiteSpace(result.gname) == false ? GetInitCap(result.gname) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trgname.Visible = true;
                        }
                        Lblexam.Text = GetInitCap(result.examname);
                        lblRemark.Text = result.remark;
                        //var modulesresult = (from c in context.CourseExamApplicationDetails
                        //                     where c.RegistrationNumber == registrationNumber && c.ResultGradeID != null && c.CourseID == currentCourseID && c.ExamID == examID
                        //                     orderby c.ModuleID
                        //                     select new
                        //                     {
                        //                         ID = c.ID,
                        //                         name = c.Module.Name,
                        //                         Code = c.Module.ShortName,
                        //                         MType = c.Module.ModuleType.Name + (c.Module.ModuleTypeID == 1 ? (c.Module.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                        //                         Result = c.Grade.Description,
                        //                         Grade = c.Grade.Code
                        //                     });

                        var moduleresult = GetData(currentCourseID, registrationNumber, examID);
                        gvMain.DataSource = moduleresult;
                        gvMain.DataBind();
                        ShowLegends(currentCourse.CourseCategoryID, examID);
                    }
                    else
                    {
                        Response.Write("No record Found");
                        Response.End();
                        return;
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowLegends(Int32 CourseCategoryID, Int32 ResultExamID)
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
                Int32[] notInGrades = { 8, 10, 11 };
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
                                if (result.legend1 > 0 && result.legend2 > 0)
                                    tdGrade.Text = result.legend1 + " to " + result.legend2;
                                else
                                    tdGrade.Text = "-";
                            }
                            else
                                tdGrade.Text = (result.description);                            
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