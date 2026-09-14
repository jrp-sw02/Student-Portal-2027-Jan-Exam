using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Data;
using System.Data;
using EConnect.NIELIT;
using System.Collections.Generic;                           //November_2024
using System.Data.SqlClient;                                //November_2024


public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    public Int32 CourseId;
    public DateTime DateFrom;
    public DateTime DateTo;
    public String DisplayCriteria;
    public String SubCriteria;
    public String RegStatusIds;
    public String RegtypeIds;
    public Int64 instituteID;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"HO/Rpt/RegistrationStatisticsFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            //if (Request.UrlReferrer == null)
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", "../MainPage.aspx"));
            //    Response.End();
            //    return;
            //}
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
    protected void ShowTableHeader(string ColumnHead)
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol0 = new TableHeaderCell();
            tcCol0.Width = Unit.Percentage(3);
            tcCol0.Text = "#";
            tcCol0.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol0);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(10);
            tcCol1.Text = "Level";
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            th.Cells.Add(tcCol1);

            if (ColumnHead != "")
            {
                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(20);
                tcCol2.Text = ColumnHead;
                tcCol2.HorizontalAlign = HorizontalAlign.Left;
                th.Cells.Add(tcCol2);
            }
            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.Text = "No. Of Candidates";
            tcCol3.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol3);
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void getAllParameters()
    {
        try
        {
            CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            DateFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
            DisplayCriteria = Convert.ToString(Request.QueryString["DisplayCriteria"]);
            SubCriteria = Convert.ToString(Request.QueryString["SubCriteria"]);
            instituteID = Convert.ToInt64(Request.QueryString["instituteID"]);
            RegStatusIds = HttpUtility.UrlDecode(Request.QueryString["RegStatusId"]);
            RegtypeIds = HttpUtility.UrlDecode(Request.QueryString["RegTypeId"]);
            if (RegStatusIds != "0")
            RegStatusIds = RegStatusIds.Remove(RegStatusIds.LastIndexOf(','));
            //if (RegtypeIds != "0")
            //    RegtypeIds = RegtypeIds.Remove(RegtypeIds.LastIndexOf(','));
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
            int i = 0;
            String strSql = "";
            string strHead = "";
            getAllParameters();
            string StrColumnHead = "";
            
            //Dynamic Array for Paramters_01_11_2024
            List<SqlParameter> parameters = new List<SqlParameter>();                 //November_2024
            int AddressTypeId = Convert.ToInt32(enmAddressType.PermanentAddress);
            strHead += " <b>Date From  :</b> " + DateFrom.ToString("dd-MMM-yyyy") + "  and  <b> Date To : </b>" + DateTo.ToString("dd-MMM-yyyy");
            using (EConnectContext context = new EConnectContext())
            {
                if (CourseId != 0)
                {
                    var course = context.Courses.Find(CourseId);
                    strHead += "</br> <b>Course Name :</b> " + course.Name;
                }
                else
                    strHead += "</br> <b>Course Name :</b> ALL";
                if (instituteID != 0)
                {
                    var Institute = context.Institutes.Find(instituteID);
                    strHead += "</br> <b>Institute Name :</b> " + Institute.Name;
                }
                else
                    strHead += "</br> <b>Institute Name :</b> ALL";
                if (RegStatusIds != "0")
                {
                    //string sql = " select Name from Registration_Status where ID IN(" + RegStatusIds + ")";
                    //DataTable dt1 = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);

                    //November_2024
                    SqlParameter[] para1 = { new SqlParameter("@regStatusIds", RegStatusIds) };
                    
                    //November_2024
                    string sql = " select Name from Registration_Status where ID IN(@regStatusIds)";                             
                    DataTable dt1 = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), para1, CommandType.Text, false);
                    string RegStatus="";
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dtRow1 in dt1.Rows)
                        {
                            RegStatus += dtRow1["Name"].ToString() + " ,";
                        }
                        strHead += "</br> <b>Registration Status : </b>" + RegStatus.Remove(RegStatus.LastIndexOf(',')) ;
                    }
                }
                else
                    strHead += "</br> <b>Registration Status : </b> ALL";

                //if (RegtypeIds != "0")
                //{
                //    string sql = " select Name from Registration_Type where ID IN(" + RegtypeIds + ")";
                //    DataTable dt1 = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                //    string RegType = "";
                //    if (dt1.Rows.Count > 0)
                //    {
                //        foreach (DataRow dtRow1 in dt1.Rows)
                //        {
                //            RegType += dtRow1["Name"].ToString() + " ,";
                //        }
                //        strHead += "</br> <b>Registration Type : </b>" + RegType.Remove(RegType.LastIndexOf(','));
                //    }
                //}
                //else
                //    strHead += "</br> <b>Registration Type : </b> ALL";

                if (DisplayCriteria != "0")
                {
                    switch (DisplayCriteria)
                    {
                        case "G":
                            //strSql = " select CR.Display_Order, CR.Name as CourseName, C.Gender as ColumnName,  COUNT(*) as Count " +
                            //        " from Registration_Detail RD, Candidate C, Course CR " +
                            //        " Where RD.Candidate_ID = C.ID  and  CAST(Registration_Date as DATE) >= '" + DateFrom + "' and CAST(Registration_Date as DATE) <= '" + DateTo + "' and ";

                            strSql = " select CR.Display_Order, CR.Name as CourseName, C.Gender as ColumnName,  COUNT(*) as Count " +
                                    " from Registration_Detail RD, Candidate C, Course CR " +
                                    " Where RD.Candidate_ID = C.ID  and  CAST(Registration_Date as DATE) >= @dateFrom and CAST(Registration_Date as DATE) <= @dateTo and ";                //November_2024

                            parameters.Add(new SqlParameter("@dateFrom", DateFrom));             //November_2024
                            parameters.Add(new SqlParameter("@dateTo", DateTo));


                            if (RegStatusIds != "0")
                            {
                                //strSql += " Registration_Status_ID IN(" + RegStatusIds + ") and ";

                                strSql += " Registration_Status_ID IN(@regStatusIds) and ";              //November_2024
                                parameters.Add(new SqlParameter("@regStatusIds", RegStatusIds));
                            }
                            //if (RegtypeIds != "0")
                            //{
                            //    strSql += " Reg_Type_ID IN(" + RegtypeIds + ") and ";
                            //}
                            if (SubCriteria != "0")
                            {
                                //strSql += " C.Gender = '" + SubCriteria + "' and ";

                                strSql += " C.Gender = @subCriteria and ";                              //November_2024
                                parameters.Add(new SqlParameter("@subCriteria", SubCriteria));

                                strHead += "</br> <b>Gender :</b> " + SubCriteria;
                            }
                            else
                                strHead += "</br> <b>Gender :</b> ALL";
                            if (CourseId != 0)
                            { 
                                //strSql += " RD.Course_ID = " + CourseId + " and ";

                                strSql += " RD.Course_ID = @courseId and ";                             //November_2024
                                parameters.Add(new SqlParameter("@courseId", CourseId));
                            }
                            //if (instituteID != 0)
                            //{
                            //    strSql += " RD.Institute_ID = " + instituteID + " and ";
                            //}
                            strSql += " RD.Course_ID = CR.ID " +
                                    " group by CR.Display_Order,CR.Name, C.Gender " +
                                    " Order by 1,3";
                            StrColumnHead = "Gender";
                            break;
                        case "CC":
                            //strSql = "select CR.Display_Order, CR.Name as CourseName, CC.Name as ColumnName,  COUNT(*) as Count" +
                            //     " from Registration_Detail RD, Cast_Category CC, Course CR,Candidate C " +
                            //     " Where RD.Candidate_ID = C.ID  and CAST(Registration_Date as DATE) >= '" + DateFrom + "' and CAST(Registration_Date as DATE) <= '" + DateTo + "' and " +
                            //     " C.Cast_Category_ID = CC.ID and ";

                            strSql = "select CR.Display_Order, CR.Name as CourseName, CC.Name as ColumnName,  COUNT(*) as Count" +
                                 " from Registration_Detail RD, Cast_Category CC, Course CR,Candidate C " +
                                 " Where RD.Candidate_ID = C.ID  and CAST(Registration_Date as DATE) >= @dateFrom and CAST(Registration_Date as DATE) <= @dateTo and " +
                                 " C.Cast_Category_ID = CC.ID and ";                            //November_2024
                            parameters.Add(new SqlParameter("@dateFrom", DateFrom));
                            parameters.Add(new SqlParameter("@dateTo", DateTo));

                            if (RegStatusIds != "0")
                            {
                                //strSql += " Registration_Status_ID IN(" + RegStatusIds + ") and ";

                                strSql += " Registration_Status_ID IN(@regStatusIds) and ";             //November_2024
                                parameters.Add(new SqlParameter("@regStatusIds", RegStatusIds));
                            }
                            //if (RegtypeIds != "0")
                            //{
                            //    strSql += " Reg_Type_ID IN(" + RegtypeIds + ") and ";
                            //}
                            if (SubCriteria != "0")
                            {
                                int CastCategoryId = Convert.ToInt32(SubCriteria);
                                //strSql += " C.Cast_Category_ID = " + CastCategoryId + " and ";

                                strSql += " C.Cast_Category_ID = @castCategoryId and ";                             //November_2024
                                parameters.Add(new SqlParameter("@castCategoryId", CastCategoryId));

                                var cc = context.CastCategories.Find(CastCategoryId);
                                strHead += "</br> <b>Cast Category :</b> " + cc.Name;
                            }
                            else
                                strHead += "</br> <b>Cast Category :</b> ALL";
                            if (CourseId != 0)
                            { 
                                //strSql += " RD.Course_ID = " + CourseId + " and ";

                                strSql += " RD.Course_ID = @courseId and ";                                         //November_2024
                                parameters.Add(new SqlParameter("@courseId", CourseId));
                            }
                            //if (instituteID != 0)
                            //{
                            //    strSql += " RD.Institute_ID = " + instituteID + " and ";
                            //}
                            strSql += " RD.Course_ID = CR.ID " +
                                 " group by CR.Display_Order,CR.Name,CC.Name" +
                                 " Order by 1,3";

                            StrColumnHead = "Cast Category";
                            break;
                        case "S":
                            //strSql = "select CR.Display_Order, CR.Name as CourseName, L.Name as ColumnName,  COUNT(*) as Count" +
                            //        " from Registration_Detail RD, Address Adr, Course CR,Candidate C,Location L " +
                            //        " Where RD.Candidate_ID = C.ID  and  CAST(Registration_Date as DATE) >= '" + DateFrom + "' and CAST(Registration_Date as DATE) <= '" + DateTo + "' and " +
                            //         " C.ID = Adr.Candidate_ID and Adr.State_ID = L.ID  and Address_Type_ID = " + AddressTypeId + " and ";


                            strSql = "select CR.Display_Order, CR.Name as CourseName, L.Name as ColumnName,  COUNT(*) as Count" +
                                   " from Registration_Detail RD, Address Adr, Course CR,Candidate C,Location L " +
                                   " Where RD.Candidate_ID = C.ID  and  CAST(Registration_Date as DATE) >= @dateFrom and CAST(Registration_Date as DATE) <= @dateTo and " +
                                    " C.ID = Adr.Candidate_ID and Adr.State_ID = L.ID  and Address_Type_ID = @addressTypeId and ";                          //November_2024
                            parameters.Add(new SqlParameter("@dateFrom", DateFrom));                                                    //November_2024
                            parameters.Add(new SqlParameter("@dateTo", DateTo));
                            parameters.Add(new SqlParameter("@addressTypeId", AddressTypeId));
                            if (RegStatusIds != "0")
                            {
                                //strSql += " Registration_Status_ID IN(" + RegStatusIds + ") and ";

                                strSql += " Registration_Status_ID IN(@regStatusIds) and ";                 //November_2024
                                parameters.Add(new SqlParameter("@regStatusIds", RegStatusIds));
                            }
                            //if (RegtypeIds != "0")
                            //{
                            //    strSql += " Reg_Type_ID IN(" + RegtypeIds + ") and ";
                            //}
                            if (SubCriteria != "0")
                            {
                                int StateId = Convert.ToInt32(SubCriteria);
                                //strSql += " Adr.State_ID = " + StateId + " and ";

                                strSql += " Adr.State_ID = @stateId and ";                              //November_2024
                                parameters.Add(new SqlParameter("@stateId", StateId));
                                var S = context.Locations.Find(StateId);
                                strHead += "</br> <b>State Name:</b> " + S.Name;
                            }
                            else
                                strHead += "</br> <b>State Name :</b> ALL";
                            if (CourseId != 0)
                            {
                                //strSql += " RD.Course_ID = " + CourseId + " and ";

                                strSql += " RD.Course_ID = @courseId and ";                                 //November_2024
                                parameters.Add(new SqlParameter("@courseId", CourseId));
                            }
                            //if (instituteID != 0)
                            //{
                            //    strSql += " RD.Institute_ID = " + instituteID + " and ";
                            //}
                            strSql += " RD.Course_ID = CR.ID " +
                                    " group by CR.Display_Order,CR.Name,L.Name" +
                                    " Order by 1,3";
                            StrColumnHead = "State";
                            break;
                        case "AT":
                           

                            //strSql = " select CR.Display_Order, CR.Name as CourseName, AP.Name as ColumnName,  COUNT(*) as Count" +
                            //        " from Registration_Detail RD, Applicant_Type AP, Course CR " +
                            //        " Where RD.Applicant_Type_ID = AP.ID and CAST(Registration_Date as DATE) >= '" + DateFrom + "' and CAST(Registration_Date as DATE) <= '" + DateTo + "' and ";


                            strSql = " select CR.Display_Order, CR.Name as CourseName, AP.Name as ColumnName,  COUNT(*) as Count" +
                                    " from Registration_Detail RD, Applicant_Type AP, Course CR " +
                                    " Where RD.Applicant_Type_ID = AP.ID and CAST(Registration_Date as DATE) >= @dateFrom and CAST(Registration_Date as DATE) <= @dateTo and ";      //November_2024
                            parameters.Add(new SqlParameter("@dateFrom", DateFrom));
                            parameters.Add(new SqlParameter("@dateTo", DateTo));
                         
                            if (RegStatusIds != "0")
                            {
                                //strSql += " Registration_Status_ID IN(" + RegStatusIds + ") and ";

                                strSql += " Registration_Status_ID IN(@regStatusIds) and ";                             //November_2024
                                parameters.Add(new SqlParameter("@regStatusIds", RegStatusIds));
                            }
                            //if (RegtypeIds != "0")
                            //{
                            //    strSql += " Reg_Type_ID IN(" + RegtypeIds + ") and ";
                            //}
                            if (SubCriteria != "0")
                            {
                                int ApplicantTypeId = Convert.ToInt32(SubCriteria);
                                //strSql += " RD.Applicant_Type_ID = " + ApplicantTypeId + " and ";

                                strSql += " RD.Applicant_Type_ID = @applicantTypeId and ";                                  //November_2024
                                parameters.Add(new SqlParameter("@applicantTypeId", ApplicantTypeId));
                                var Ap = context.ApplicantTypes.Find(ApplicantTypeId);
                                strHead += "</br> <b>Applicant Type:</b> " + Ap.Name;
                            }
                            else
                                strHead += "</br> <b>Applicant Type:</b> ALL";
                            if (CourseId != 0)
                            {
                                //strSql += " RD.Course_ID = " + CourseId + " and ";

                                strSql += " RD.Course_ID = @courseId and ";                                             //November_2024
                                parameters.Add(new SqlParameter("@courseId", CourseId));
                            }
                            if (instituteID != 0)
                            {
                                //strSql += " RD.Institute_ID = " + instituteID + " and ";

                                strSql += " RD.Institute_ID = @instituteID and ";                                       //November_2024
                                parameters.Add(new SqlParameter("@instituteID", instituteID));
                            }
                            strSql += " RD.Course_ID = CR.ID " +
                                    " group by CR.Display_Order,CR.Name,AP.Name" +
                                    " Order by 1,3";

                            StrColumnHead = "Applicant Type";
                            break;
                    }
                }
                else
                {
                    //strSql += "select CR.Display_Order, CR.Name as CourseName,  COUNT(*) as Count" +
                    //            " from Registration_Detail RD, Course CR " +
                    //            " Where RD.Course_ID = CR.ID   and CAST(Registration_Date as DATE) >= '" + DateFrom + "' and CAST(Registration_Date as DATE) <= '" + DateTo + "' ";

                    strSql += "select CR.Display_Order, CR.Name as CourseName,  COUNT(*) as Count" +
                                " from Registration_Detail RD, Course CR " +
                                " Where RD.Course_ID = CR.ID   and CAST(Registration_Date as DATE) >= @dateFrom and CAST(Registration_Date as DATE) <= @dateTo ";                 //November_2024
                    parameters.Add(new SqlParameter("@dateFrom", DateFrom));
                    parameters.Add(new SqlParameter("@dateTo", DateTo));
                    if (RegStatusIds != "0")
                    {
                        //strSql += " and Registration_Status_ID IN(" + RegStatusIds + ")  ";

                        strSql += " and Registration_Status_ID IN(@regStatusIds)  ";                            //November_2024
                        parameters.Add(new SqlParameter("@regStatusIds", RegStatusIds));
                    }
                    //if (RegtypeIds != "0")
                    //{
                    //    strSql += " and Reg_Type_ID IN(" + RegtypeIds + ") ";
                    //}
                    if (CourseId != 0)
                    {
                        //strSql += " and RD.Course_ID = " + CourseId + "";

                        strSql += " and RD.Course_ID = @courseId";                                             //November_2024
                        parameters.Add(new SqlParameter("@courseId", CourseId));
                    }
                    //if (instituteID != 0)
                    //{
                    //    strSql += " and RD.Institute_ID = " + instituteID + "  ";
                    //}
                    strSql += " group by CR.Display_Order,CR.Name" +
                                " Order by 1,3 ";
                    StrColumnHead = "";
                }
            };
            LblRptSubHeader.Text = strHead;
            //DataTable dt = DbUtility.GetDataTable(strSql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            DataTable dt = DbUtility.GetDataTable(strSql, new EConnect.Connections.SqlCon(), parameters.ToArray(), CommandType.Text, false);
            string TempRowData = "";
            if (dt.Rows.Count > 0)
                TempRowData = Convert.ToString(dt.Rows[0]["CourseName"]);
            else
                LblRptSubHeader.Text = "<font color='Red'> No Record found !</font>";
            string TempCourseName = "";
            Int64 Counter = 0;
            Int64 Total = 0;
            Int64 GrandTotal = 0;
            string TempCount = "";
            string TempColName = "";
            if (dt.Rows.Count > 0)
            {
                ShowTableHeader(StrColumnHead);
                foreach (DataRow dtRow in dt.Rows)
                {
                    TempCount = dtRow["Count"].ToString();
                    GrandTotal += Convert.ToInt64(dtRow["Count"]);
                    TempCourseName = dtRow["CourseName"].ToString();
                    if (DisplayCriteria == "0")
                        TempColName = null;
                    else
                    {
                        TempColName = dtRow["ColumnName"].ToString();
                        if (TempRowData == dtRow["CourseName"].ToString())
                        {
                            Total += Convert.ToInt64(dtRow["Count"]);
                            if (Counter == 0)
                                TempCourseName = dtRow["CourseName"].ToString();
                            else
                                TempCourseName = "";
                            Counter++;
                        }
                        else
                        {
                            createRow("", "<b>Total", "", "" + Total.ToString());
                            TempRowData = dtRow["CourseName"].ToString();
                            Total = Convert.ToInt64(dtRow["Count"]);
                        }
                    }
                    createRow((i + 1).ToString(), TempCourseName, TempColName, TempCount);
                    i++;
                }
                if (DisplayCriteria != "0")
                {
                    createRow("", "<b>Total", "", "" + Total.ToString());
                    if(CourseId == 0)
                    createRow("", "<b>Grand Total", "", "" + GrandTotal.ToString());
                }
                else
                    createRow("", "<b>Grand Total", TempColName,  GrandTotal.ToString());
            }

        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void createRow(string i, string TempCourseName, string TempColName, string TempCount)
    {
        try
        {

            TableRow tr = new TableRow();
            if (i != "")
            {
                if ((Convert.ToInt32(i) % 2) == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";
            }
            else
            {
                tr.Attributes.Add("style", "background: #A4B7C6;");
            }
            //#95ABBD" #A4B7C6 #B9C7D2
            TableCell tdRow0 = new TableCell();
            tdRow0.Width = Unit.Percentage(3);
            tdRow0.Text = i.ToString();
            tdRow0.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tdRow0);

            TableCell tdRow1 = new TableCell();
            tdRow1.Width = Unit.Percentage(10);
            tdRow1.Text = TempCourseName;
            //if (i == "")
            //{
            //    tdRow1.ColumnSpan = 2;
            //}
            tr.Cells.Add(tdRow1);
            if (TempColName != null)
            {
                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(20);
                tdRow2.Text = TempColName;
                tr.Cells.Add(tdRow2);
            }
            TableCell tdRow3 = new TableCell();
            tdRow3.Width = Unit.Percentage(10);
            tdRow3.Text = TempCount;
            tdRow3.HorizontalAlign = HorizontalAlign.Right;
            tdRow3.Attributes.Add("style", "padding-right:20px;");
            tr.Cells.Add(tdRow3);
            tbl.Rows.Add(tr);

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
}