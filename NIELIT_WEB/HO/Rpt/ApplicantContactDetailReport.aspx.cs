using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.SqlClient;

public partial class HO_Rpt_ApplicantContactDetailReport : BasePage
{
    Table tbl = new Table();    
    //Int32 StatusId = 0;
    UserType loginUserType;
    Int64 entityID = 0;
    //Int32 applicantTypeID = 0;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/ApplicantContactDetailsFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {

                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                ShowData();
                divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader()
    {
        try
        {
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            int moduletype = Convert.ToInt32(Request.QueryString["moduletypeID"]);
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "ApplicationNo.";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol20 = new TableHeaderCell();
                tcCol20.Width = Unit.Percentage(5);
                tcCol20.HorizontalAlign = HorizontalAlign.Center;
                tcCol20.Text = "Reg.No.";
                th.Cells.Add(tcCol20);
            }

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(10);
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            tcCol10.Text = "Course Name";
            th.Cells.Add(tcCol10);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(19);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Candidate Name";
            th.Cells.Add(tcCol3);


            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(5);
            tcCol5.Text = "Email";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(10);
            tcCol6.Text = "Mobile Number";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                if (moduletype == 0)
                {
                    TableHeaderCell tcCol7 = new TableHeaderCell();
                    tcCol7.Width = Unit.Percentage(9);
                    tcCol7.Text = "No of Theory Modules";
                    tcCol7.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol7);


                    TableHeaderCell tcCol8 = new TableHeaderCell();
                    tcCol8.Width = Unit.Percentage(9);
                    tcCol8.Text = "No of Practical Modules";
                    tcCol8.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol8);
                }
                else if (moduletype == 1)
                {
                    TableHeaderCell tcCol7 = new TableHeaderCell();
                    tcCol7.Width = Unit.Percentage(9);
                    tcCol7.Text = "No of Theory Modules";
                    tcCol7.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol7);
                }
                else if (moduletype == 2)
                {
                    TableHeaderCell tcCol8 = new TableHeaderCell();
                    tcCol8.Width = Unit.Percentage(9);
                    tcCol8.Text = "No of Practical Modules";
                    tcCol8.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol8);
                }
            }
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowData()
    {
        EConnectContext context= new EConnectContext();
        try
        {            
            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            int moduletypeID = Convert.ToInt32(Request.QueryString["moduletypeID"]);
            StringBuilder mySql = new StringBuilder();
            DataTable dt;
            int i = 0;
            int InstituteID = 0;
            int RegionalcentreID = 0;
            if (loginUserType == UserType.Institute)
                InstituteID = Convert.ToInt32(entityID);
            if (loginUserType == UserType.RegionalCenter)
                RegionalcentreID = Convert.ToInt32(entityID);
            Exam exam = context.Exams.Find(ExamId);
            string strHead = "";
            if (exam != null)
            {
                strHead += "</br> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                strHead += ", <b>Course Category :</b> " + exam.CourseCategory.Name;
                strHead += " , <b>Course : </b>" + exam.Course.Name;
                strHead += "<br/> <b> Exam Cycle : </b>" + exam.ExaminationCycle.Name;
                strHead += ", <b> Exam Name : </b>" + exam.Name;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                if (moduletypeID == 0)
                {
                    strHead += "<br/> <b> Module Type : </b> All";
                }
                else if (moduletypeID == 1)
                {
                    strHead += "<br/> <b> Module Type : </b> Theory";
                }
                else if (moduletypeID == 2)
                {
                    strHead += "<br/> <b> Module Type : </b> Practical";
                }
            }
            if (loginUserType == UserType.Institute)
            {
                var ins = context.Institutes.AsNoTracking().Where(s => s.ID == InstituteID).FirstOrDefault();
                if (ins != null)
                    strHead += "<br/> <b> Institute Name : </b>" + GetInitCap(ins.Name);
            }
            if (loginUserType == UserType.RegionalCenter)
            {
                var reg = context.RegionalCenters.Where(s => s.ID == RegionalcentreID).FirstOrDefault();
                if (reg != null)
                    strHead += "<br/> <b> Regional Centre Name : </b>" + GetInitCap(reg.Name);
            }
            if (ExamId != 0 && CourseId != 0 && TypeId != 0)
            {
                if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    if (loginUserType == UserType.Institute)
                    {
                       /* mySql.Append("select c.Code as coursename, a.Number as appno , A.Name as name, A.Mobile as mobile, A.Email as email from Certificate_Exam_Application A, Course c " +
                                     " Where c.ID = A.Course_ID and  A.Exam_ID = " + ExamId + " and  A.Course_ID = " + CourseId + "  and  A.Institute_ID = " + InstituteID + " " +
                                     " order by A.ID");*/
                        mySql.Append("select c.Code as coursename, a.Number as appno , A.Name as name, A.Mobile as mobile, A.Email as email from Certificate_Exam_Application A, Course c " +
                                    " Where c.ID = A.Course_ID and  A.Exam_ID = @ExamId  and  A.Course_ID = @CourseId   and  A.Institute_ID = @InstituteID " +
                                    " order by A.ID");

                       
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId),
                                     new SqlParameter("@InstituteID",InstituteID),
                                       };
                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(5);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                    else if (loginUserType == UserType.HeadOffice)
                    {
                        mySql.Append("select c.Code as coursename, a.Number as appno , A.Name as name, A.Mobile as mobile, A.Email as email from Certificate_Exam_Application A, Course c " +
                                     " Where c.ID = A.Course_ID and  A.Exam_ID @ ExamId  and  A.Course_ID = @CourseId " +
                                     " order by A.ID");
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId)
                                       };
                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(5);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                    else if (loginUserType == UserType.RegionalCenter)
                    {
                        mySql.Append("select c.Code as coursename, a.Number as appno , A.Name as name, A.Mobile as mobile, A.Email as email from Certificate_Exam_Application A, Course c " +
                                     " Where c.ID = A.Course_ID and  A.Exam_ID = @ExamId  and  A.Course_ID = @CourseId  and A.Regional_Center_ID = @RegionalcentreID  " +
                                     " order by A.ID");
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId),
                                     new SqlParameter("@RegionalcentreID",RegionalcentreID),
                                       };
                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(5);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                }
                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    if (loginUserType == UserType.Institute)
                    {
                        mySql.Append("select c.Name as coursename, a.Number as appno , A.Name as name, A.Mobile as mobile, A.Email as email from Course_Registration_Application A, Course c " +
                                     " Where c.ID = A.Course_ID and  A.Exam_ID = @ExamId  and  A.Course_ID = @CourseId   and  A.Institute_ID = @InstituteID  " +
                                     " order by A.ID");
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId),
                                     new SqlParameter("@InstituteID",InstituteID),
                                       };
                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(5);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                    else if (loginUserType == UserType.HeadOffice)
                    {
                        mySql.Append("select c.Name as coursename, a.Number as appno , A.Name as name, A.Mobile as mobile, A.Email as email from Course_Registration_Application A, Course c " +
                                     " Where c.ID = A.Course_ID and A.Exam_ID = @ExamId  and  A.Course_ID = @CourseId  order by A.ID");
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId)
                                    
                                       };
                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(5);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                }
                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    if (loginUserType == UserType.Institute)
                    {
                        if (moduletypeID == 0)
                        {
                            mySql.Append("select c.Name as coursename, a.Appl_Number as appno , a.Registration_Number as regno, n.Name as name, t.Mobile as mobile, t.Email as email, a.Number_of_th_Modules as th, a.Number_of_Pr_Modules as prac from Course_Exam_Application A, Course c, Candidate n, Candidate_Contact t " +
                                       " Where c.ID = A.Course_ID and a.Candidate_ID = n.ID and n.ID = t.Candidate_ID  and  A.Exam_ID= @ExamId  and  A.Course_ID= @CourseId   and  A.Institute_ID = @InstituteID  " +
                                       " order by c.ID, a.Registration_Number ");
                           
                        }
                        else if (moduletypeID == 1)
                        {
                            mySql.Append("select c.Name  as coursename,a.Appl_Number as appno , a.Registration_Number as regno, n.Name as name, t.Mobile as mobile, t.Email as email, a.Number_of_th_Modules as th from Course_Exam_Application A, Course c, Candidate n, Candidate_Contact t " +
                                     " Where c.ID = A.Course_ID and a.Candidate_ID = n.ID and n.ID = t.Candidate_ID  and  A.Exam_ID = @ExamId  and  A.Course_ID= @CourseId   and  A.Institute_ID = @InstituteID   and a.Number_of_th_Modules > 0 " +
                                     " order by c.ID, a.Registration_Number ");
                           

                        }
                        else if (moduletypeID == 2)
                        {
                            mySql.Append("select c.Name  as coursename, a.Appl_Number as appno , a.Registration_Number as regno, n.Name as name, t.Mobile as mobile, t.Email as email, a.Number_of_Pr_Modules as prac  from Course_Exam_Application A, Course c, Candidate n, Candidate_Contact t " +
                                     " Where c.ID = A.Course_ID and a.Candidate_ID = n.ID and n.ID = t.Candidate_ID  and  A.Exam_ID= @ExamId  and  A.Course_ID = @CourseId   and  A.Institute_ID = @InstituteID  and a.Number_of_Pr_Modules > 0 " +
                                     " order by c.ID, a.Registration_Number ");
                           
                        }
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId),
                                     new SqlParameter("@InstituteID",InstituteID),
                                       };

                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol2 = new TableCell();
                                tcCol2.Width = Unit.Percentage(4);
                                tcCol2.HorizontalAlign = HorizontalAlign.Left;
                                tcCol2.Text = Convert.ToInt64(dtRow["regno"]).ToString();
                                tr.Cells.Add(tcCol2);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(5);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                                {
                                    if (moduletypeID == 0)
                                    {
                                        TableCell tcCol8 = new TableCell();
                                        tcCol8.Width = Unit.Percentage(5);
                                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol8.Text = Convert.ToInt32(dtRow["th"]).ToString();
                                        tr.Cells.Add(tcCol8);

                                        TableCell tcCol9 = new TableCell();
                                        tcCol9.Width = Unit.Percentage(5);
                                        tcCol9.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol9.Text = Convert.ToInt32(dtRow["prac"]).ToString();
                                        tr.Cells.Add(tcCol9);
                                    }
                                    if (moduletypeID == 1)
                                    {
                                        TableCell tcCol8 = new TableCell();
                                        tcCol8.Width = Unit.Percentage(5);
                                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol8.Text = Convert.ToInt32(dtRow["th"]).ToString();
                                        tr.Cells.Add(tcCol8);
                                    }
                                    if (moduletypeID == 2)
                                    {
                                        TableCell tcCol9 = new TableCell();
                                        tcCol9.Width = Unit.Percentage(5);
                                        tcCol9.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol9.Text = Convert.ToInt32(dtRow["prac"]).ToString();
                                        tr.Cells.Add(tcCol9);
                                    }
                                }
                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                    else if (loginUserType == UserType.HeadOffice)
                    {

                        if (moduletypeID == 0)
                        {
                            mySql.Append("select c.Name as coursename, a.Appl_Number as appno , a.Registration_Number as regno, n.Name as name, t.Mobile as mobile, t.Email as email, a.Number_of_th_Modules as th, a.Number_of_Pr_Modules as prac from Course_Exam_Application A, Course c, Candidate n, Candidate_Contact t " +
                                       " Where c.ID = A.Course_ID and a.Candidate_ID = n.ID and n.ID = t.Candidate_ID  and  A.Exam_ID= @ExamId  and  A.Course_ID =  @CourseId  " +
                                       " order by c.ID, a.Registration_Number ");

                        }
                        else if (moduletypeID == 1)
                        {
                            mySql.Append("select c.Name  as coursename,a.Appl_Number as appno , a.Registration_Number as regno, n.Name as name, t.Mobile as mobile, t.Email as email, a.Number_of_th_Modules as th from Course_Exam_Application A, Course c, Candidate n, Candidate_Contact t " +
                                     " Where c.ID = A.Course_ID and a.Candidate_ID = n.ID and n.ID = t.Candidate_ID  and  A.Exam_ID= @ExamId  and  A.Course_ID = @CourseId  and a.Number_of_th_Modules > 0 " +
                                     " order by c.ID, a.Registration_Number ");

                        }
                        else if (moduletypeID == 2)
                        {
                            mySql.Append("select c.Name  as coursename, a.Appl_Number as appno , a.Registration_Number as regno, n.Name as name, t.Mobile as mobile, t.Email as email, a.Number_of_Pr_Modules as prac  from Course_Exam_Application A, Course c, Candidate n, Candidate_Contact t " +
                                     " Where c.ID = A.Course_ID and a.Candidate_ID = n.ID and n.ID = t.Candidate_ID  and  A.Exam_ID= @ExamId  and  A.Course_ID = @CourseId  and a.Number_of_Pr_Modules > 0 " +
                                     " order by c.ID, a.Registration_Number ");

                        }
                        SqlParameter[] p ={
				                    new SqlParameter("@ExamId",ExamId),
                                     new SqlParameter("@CourseId",CourseId)
                                       };
                        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeader();
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableHeaderCell tcCol1 = new TableHeaderCell();
                                tcCol1.Width = Unit.Percentage(2);
                                tcCol1.HorizontalAlign = HorizontalAlign.Left;
                                tcCol1.Text = i.ToString();
                                tr.Cells.Add(tcCol1);

                                TableCell tcCol = new TableCell();
                                tcCol.Width = Unit.Percentage(3);
                                tcCol.HorizontalAlign = HorizontalAlign.Left;
                                tcCol.Text = dtRow["appno"].ToString();
                                tr.Cells.Add(tcCol);

                                TableCell tcCol2 = new TableCell();
                                tcCol2.Width = Unit.Percentage(3);
                                tcCol2.HorizontalAlign = HorizontalAlign.Left;
                                tcCol2.Text = Convert.ToInt64(dtRow["regno"]).ToString();
                                tr.Cells.Add(tcCol2);

                                TableCell tcCol3 = new TableCell();
                                tcCol3.HorizontalAlign = HorizontalAlign.Left;
                                tcCol3.Width = Unit.Percentage(5);
                                tcCol3.Text = dtRow["coursename"].ToString();
                                tr.Cells.Add(tcCol3);

                                TableCell tcCol4 = new TableCell();
                                tcCol4.HorizontalAlign = HorizontalAlign.Left;
                                tcCol4.Width = Unit.Percentage(25);
                                tcCol4.Text = dtRow["name"].ToString();
                                tr.Cells.Add(tcCol4);

                                TableCell tcCol6 = new TableCell();
                                tcCol6.Width = Unit.Percentage(25);
                                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                                tcCol6.Text = dtRow["Email"].ToString();
                                tr.Cells.Add(tcCol6);

                                TableCell tcCol7 = new TableCell();
                                tcCol7.Width = Unit.Percentage(4);
                                tcCol7.HorizontalAlign = HorizontalAlign.Right;
                                tcCol7.Text = Convert.ToInt64(dtRow["mobile"]).ToString();
                                tr.Cells.Add(tcCol7);

                                if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                                {
                                    if (moduletypeID == 0)
                                    {
                                        TableCell tcCol8 = new TableCell();
                                        tcCol8.Width = Unit.Percentage(5);
                                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol8.Text = Convert.ToInt32(dtRow["th"]).ToString();
                                        tr.Cells.Add(tcCol8);

                                        TableCell tcCol9 = new TableCell();
                                        tcCol9.Width = Unit.Percentage(5);
                                        tcCol9.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol9.Text = Convert.ToInt32(dtRow["prac"]).ToString();
                                        tr.Cells.Add(tcCol9);
                                    }
                                    if (moduletypeID == 1)
                                    {
                                        TableCell tcCol8 = new TableCell();
                                        tcCol8.Width = Unit.Percentage(5);
                                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol8.Text = Convert.ToInt32(dtRow["th"]).ToString();
                                        tr.Cells.Add(tcCol8);
                                    }
                                    if (moduletypeID == 2)
                                    {
                                        TableCell tcCol9 = new TableCell();
                                        tcCol9.Width = Unit.Percentage(5);
                                        tcCol9.HorizontalAlign = HorizontalAlign.Right;
                                        tcCol9.Text = Convert.ToInt32(dtRow["prac"]).ToString();
                                        tr.Cells.Add(tcCol9);
                                    }
                                }
                                tbl.Rows.Add(tr);
                            }
                            lblCount.Visible = true;
                            lblCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found !";
                        }
                    }
                }
            }
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }
    }
}