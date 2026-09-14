using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

public partial class NielitCentreBatchWiseStudentsRep : BasePage
{
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0, centreID = 0;
    Int32 currentRoleId = 0, UserTypeId;

    string courseCategoryName = "";
    string courseName = "";
    string centreName = "";
    string batchCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            
            currentRoleId = Convert.ToInt32(Session["RoleID"]);

                if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreBatchWiseStudentsFilter.aspx"))
                    {
                        Response.Write("Sorry! You don't have rights  to view this page");
                        Response.End();
                    }

                loginUserType = (UserType)Session["UserType"];
                entityID = Convert.ToInt64(Session["EntityID"]);  // centreId
                UserTypeId = Convert.ToInt32(Session["UserTypeId"]);                

                if (UserTypeId != 6)  // For Non Ho Users
                {
                    centreID = Convert.ToInt64(Request.QueryString["centreID"]);    
                    if (entityID != centreID)
                    {
                        Response.Write("Sorry! You don't have rights  to view this page");
                        Response.End();
                    }
                }


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

    #region vCode
    public DataTable BatchWiseStudentsDetailsReport(Int64 centreID, Int64 courseCat, Int64 courseID, Int64 batchId)
        {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand scCommand = new SqlCommand("RepBatchWiseStudentsDetails", con))
                {
                scCommand.CommandType = CommandType.StoredProcedure;

                scCommand.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                scCommand.Parameters["@centreId"].Value = centreID;
                scCommand.Parameters.Add(new SqlParameter("@courseCategoryID", SqlDbType.BigInt));
                scCommand.Parameters["@courseCategoryID"].Value = courseCat;
                scCommand.Parameters.Add(new SqlParameter("@courseId", SqlDbType.BigInt));
                scCommand.Parameters["@courseId"].Value = courseID;
                scCommand.Parameters.Add(new SqlParameter("@batchId", SqlDbType.BigInt));
                scCommand.Parameters["@batchId"].Value = batchId;

                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(scCommand))
                    {
                    sda.Fill(myDt);
            }}}
        return myDt;
        }
    protected void ShowData()
    {
        try
        {
            Int64 courseCat=0, courseID=0, batchId=0;

            centreID = Convert.ToInt64(Request.QueryString["centreID"]);
            courseCat = Convert.ToInt64(Request.QueryString["courseCat"]);
            courseID = Convert.ToInt64(Request.QueryString["courseID"]);
            batchId = Convert.ToInt64(Request.QueryString["batchId"]);


            lbldatefromto.Text = ""; // +startDate.ToString("dd-MMM-yyyy") + " to " + endDate.ToString("dd-MMM-yyyy");
            //lbldatefromto.Text = "Report Period : " + startDate.ToString("dd-MMM-yyyy") + " to " + endDate.ToString("dd-MMM-yyyy");


            using (DataTable dt = BatchWiseStudentsDetailsReport(centreID, courseCat, courseID, batchId))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var application = (from a in dt.AsEnumerable()
                                           select new
                                           {
                                               CentreNa = a.Field<string>("CentreName"),
                                               CoCatName = a.Field<string>("CoCatName"),
                                               CourseName = a.Field<string>("CourseName"),
                                               batchCode = a.Field<string>("batchCode"),
                                               batchName = a.Field<string>("batchName"),

                                               Number = a.Field<string>("Number"),
                                               Name = a.Field<string>("Name"),
                                               F_Name = a.Field<string>("F_Name"),
                                               M_Name = a.Field<string>("M_Name"),
                                               Locality = a.Field<string>("Locality"),
                                               DistrictName = a.Field<string>("DistrictName"),
                                               City = a.Field<string>("City"),
                                               StateName = a.Field<string>("StateName"),
                                               Pin = a.Field<int>("Pin"),
                                               Email = a.Field<string>("Email"),
                                               MobileNo = a.Field<Int64>("MobileNo"),
                                               AadharNo = a.Field<Int64>("AadharNo"),
                                               Gender = a.Field<string>("Gender"),
                                               DOB = a.Field<DateTime>("DOB"),
                                               CastCat = a.Field<string>("CastCat"),
                                               RegistrationNo = a.Field<int>("RegistrationNo"),
                                               AdmDate = a.Field<DateTime>("AdmDate"),
                                               CourseStateDate = a.Field<DateTime>("CourseStateDate"),
                                               CourseEndDate = a.Field<DateTime>("CourseEndDate"),
                                               ResultStatus = a.Field<string>("ResultStatus"),
                                               EmpStatus = a.Field<string>("EmpStatus"),
                                               Remark = a.Field<string>("Remark"),
                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            var hh1 = application.FirstOrDefault();
                            centreName = hh1.CentreNa;
                            courseCategoryName = hh1.CoCatName;
                            courseName = hh1.CourseName;
                            batchCode = hh1.batchCode;

                            string checkRD = string.Empty;
                            ShowTableHeader(centreName, courseCategoryName, courseName, batchCode);

                            int i = 1;
                            foreach (var app in application)
                            {
                                string SkipOneRowData = app.Number.ToString();
                                if (SkipOneRowData != checkRD)
                                {
                                    checkRD = SkipOneRowData;
                                    TableRow tr = new TableRow();
                                    if (i % 2 == 0)
                                        tr.CssClass = "gdalternate1";
                                    else
                                        tr.CssClass = "gdrow1";

                                    TableCell tdRow = new TableCell();
                                    tdRow.Width = Unit.Percentage(1);
                                    tdRow.Text = i.ToString();
                                    tdRow.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(tdRow);

                                    //TableCell tdRow0 = new TableCell();
                                    //tdRow0.Width = Unit.Percentage(18);
                                    //tdRow0.Text = app.Number.ToString();
                                    //tdRow0.HorizontalAlign = HorizontalAlign.Left;
                                    //tr.Cells.Add(tdRow0);

                                    TableCell tdRow2 = new TableCell();
                                    tdRow2.Width = Unit.Percentage(18);
                                    tdRow2.Text = app.Name.ToString();
                                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow2);

                                    TableCell tdRow3 = new TableCell();
                                    tdRow3.Width = Unit.Percentage(5);
                                    tdRow3.Text = app.F_Name.ToString();
                                    tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow3);

                                    TableCell tdRow5 = new TableCell();
                                    tdRow5.Width = Unit.Percentage(5);
                                    tdRow5.Text = app.M_Name.ToString();
                                    tdRow5.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow5);

                                    TableCell tdRow13 = new TableCell();
                                    tdRow13.Width = Unit.Percentage(6);
                                    tdRow13.Text = app.Locality.ToString();
                                    tdRow13.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow13);

                                    TableCell tdRow14 = new TableCell();
                                    tdRow14.Width = Unit.Percentage(6);
                                    tdRow14.Text = app.DistrictName.ToString();
                                    tdRow14.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow14);

                                    TableCell tdRow15 = new TableCell();
                                    tdRow15.Width = Unit.Percentage(5);
                                    tdRow15.Text = app.City.ToString();
                                    tdRow15.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow15);

                                    TableCell tdRow16 = new TableCell();
                                    tdRow16.Width = Unit.Percentage(6);
                                    tdRow16.Text = app.StateName.ToString();
                                    tdRow16.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow16);

                                    TableCell tdRow17 = new TableCell();
                                    tdRow17.Width = Unit.Percentage(8);
                                    tdRow17.Text = app.Pin.ToString();
                                    tdRow17.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow17);


                                    TableCell tdRow18 = new TableCell();
                                    tdRow18.Width = Unit.Percentage(6);
                                    //tdRow18.Text = app.Email.ToString();
                                    if (app.Email != "")
                                        {
                                        string email1 = "";
                                        email1 = Convert.ToString(app.Email);
                                        tdRow18.Text = EmailConvertIntoTextForm(email1);
                                        }
                                    tdRow18.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow18);

                                    //TableCell tdRow18 = new TableCell();
                                    //tdRow18.Width = Unit.Percentage(6);
                                    //tdRow18.Text = app.Email.ToString();
                                    //tdRow18.HorizontalAlign = HorizontalAlign.Left;
                                    ////tdRow18.Wrap = true;
                                    //tr.Cells.Add(tdRow18);
                                    ////TableCell tdRow18 = new TableCell();
                                    ////tdRow18.Width = Unit.Percentage(4);
                                    ////tdRow18.Text = app.Email.ToString();
                                    ////tdRow18.HorizontalAlign = HorizontalAlign.Left;
                                    //////tdRow18.Wrap = false;
                                    ////tr.Cells.Add(tdRow18);

                                    TableCell tdRow19 = new TableCell();
                                    tdRow19.Width = Unit.Percentage(8);
                                    tdRow19.Text = app.MobileNo.ToString();
                                    tdRow19.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow19);

                                    TableCell tdRow20 = new TableCell();
                                    tdRow20.Width = Unit.Percentage(8);
                                    string aadharnumber = app.AadharNo.ToString();
                                    //tdRow20.Text = app.AadharNo.ToString();
                                    if (aadharnumber != "0")
                                    {
                                    //-----------  XXXX XXXX 1234
                                    var lastDigits = aadharnumber.Substring(aadharnumber.Length - 4, 4);
                                    var marked = new String('X', aadharnumber.Length - lastDigits.Length);
                                    var FinalAadharDecrypted = string.Concat(marked, lastDigits);
                                    var lastDigitsWd = FinalAadharDecrypted.Substring(FinalAadharDecrypted.Length - 4, 4);
                                    var Total_Marked = new String('X', FinalAadharDecrypted.Length - lastDigitsWd.Length);
                                    var FinalAadharDecryptedWithSpace = string.Concat(Total_Marked, " - " + lastDigitsWd);
                                    tdRow20.Text = FinalAadharDecryptedWithSpace;
                                        
                                        }
                                    else { tdRow20.Text = "NA"; }
                                    tdRow20.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(tdRow20);

                                    //TableCell tdRow21 = new TableCell();
                                    //tdRow21.Width = Unit.Percentage(8);
                                    //tdRow21.Text = app.Email.ToString(); //Any Other Details
                                    //tdRow21.HorizontalAlign = HorizontalAlign.Left;
                                    //tr.Cells.Add(tdRow21);

                                    TableCell tdRow22 = new TableCell();
                                    tdRow22.Width = Unit.Percentage(8);
                                    tdRow22.Text = app.Gender.ToString();
                                    tdRow22.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(tdRow22);

                                    TableCell tdRow23 = new TableCell();
                                    tdRow23.Width = Unit.Percentage(6);
                                    tdRow23.Text = app.DOB.ToString("dd-MMM-yyyy");
                                    tdRow23.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow23);

                                    TableCell tdRow24 = new TableCell();
                                    tdRow24.Width = Unit.Percentage(8);
                                    tdRow24.Text = app.CastCat.ToString();
                                    tdRow24.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow24);

                                    TableCell tdRow25 = new TableCell();
                                    tdRow25.Width = Unit.Percentage(6);
                                    //tdRow25.Text = app.RegistrationNo.ToString();
                                    if (app.RegistrationNo != 0)
                                    { tdRow25.Text = app.RegistrationNo.ToString(); }
                                    else { tdRow25.Text = "NA"; }
                                    //tcCol16.Text = String.IsNullOrEmpty(dtRow["SettledDate"].ToString()) ? "NA" : Convert.ToDateTime(dtRow["SettledDate"]).ToString("dd-MMM-yyyy");
                                    tdRow25.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(tdRow25);

                                    TableCell tdRow26 = new TableCell();
                                    tdRow26.Width = Unit.Percentage(4);
                                    tdRow26.Text = app.AdmDate.ToString("dd-MMM-yyyy");
                                    tdRow26.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow26);

                                    TableCell tdRow27 = new TableCell();
                                    tdRow27.Width = Unit.Percentage(8);
                                    tdRow27.Text = app.CourseStateDate.ToString("dd-MMM-yyyy");
                                    tdRow27.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow27);

                                    TableCell tdRow28 = new TableCell();
                                    tdRow28.Width = Unit.Percentage(8);
                                    tdRow28.Text = app.CourseEndDate.ToString("dd-MMM-yyyy");
                                    tdRow28.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow28);

                                    TableCell tdRow29 = new TableCell();
                                    tdRow29.Width = Unit.Percentage(6);
                                    tdRow29.Text = app.ResultStatus.ToString();
                                    tdRow29.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow29);

                                    TableCell tdRow30 = new TableCell();
                                    tdRow30.Width = Unit.Percentage(4);
                                    tdRow30.Text = app.EmpStatus.ToString();
                                    tdRow30.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(tdRow30);

                                    TableCell tdRow31 = new TableCell();
                                    tdRow31.Width = Unit.Percentage(8);
                                    tdRow31.Text = app.Remark.ToString();
                                    tdRow31.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow31);

                                    tbl.Rows.Add(tr);
                                    i++;
                                }
                            }
                        }

                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }
                    }

                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader(string centreName, string coursecat, string courseName, string batchCode)
    {
        try
        {
        int NumberOf_ColumnSpan = 22;
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            tc1.ColumnSpan = NumberOf_ColumnSpan;
            tc1.Text = "NIELIT Center BatchWise Students Details Report For - " + centreName;
            //tc1.Text = "NIELIT Center CourseWise Students Details Report For " + datefrom.ToString("dd-MMM-yyyy") + " To " + dateto.ToString("dd-MMM-yyyy") + "";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);
            tbl.Rows.Add(th1);


            //TableHeaderRow th101 = new TableHeaderRow();
            //th101.CssClass = "sample3";
            //TableHeaderCell tc1011 = new TableHeaderCell();
            //tc1011.Width = Unit.Percentage(100);
            //tc1011.ColumnSpan = NumberOf_ColumnSpan;
            //tc1011.Text = "<b>Centre Name : " + centreName +"<b>";
            //tc1011.HorizontalAlign = HorizontalAlign.Left;
            //th101.Cells.Add(tc1011);
            //tbl.Rows.Add(th101);

            TableHeaderRow th102 = new TableHeaderRow();
            th102.CssClass = "sample3";
            TableHeaderCell tc1012 = new TableHeaderCell();
            tc1012.Width = Unit.Percentage(100);
            tc1012.ColumnSpan = NumberOf_ColumnSpan;
            tc1012.Text = "<b>Course Catogory : " + coursecat + "<b>";
            tc1012.HorizontalAlign = HorizontalAlign.Left;
            th102.Cells.Add(tc1012);
            tbl.Rows.Add(th102);

            TableHeaderRow th103 = new TableHeaderRow();
            th103.CssClass = "sample3";
            TableHeaderCell tc1013 = new TableHeaderCell();
            tc1013.Width = Unit.Percentage(100);
            tc1013.ColumnSpan = NumberOf_ColumnSpan;
            tc1013.Text = "<b>Course : " + courseName + "<b>";
            tc1013.HorizontalAlign = HorizontalAlign.Left;
            th103.Cells.Add(tc1013);
            tbl.Rows.Add(th103);

            TableHeaderRow th104 = new TableHeaderRow();
            th104.CssClass = "sample3";
            TableHeaderCell tc1014 = new TableHeaderCell();
            tc1014.Width = Unit.Percentage(100);
            tc1014.ColumnSpan = NumberOf_ColumnSpan;
            tc1014.Text = "<b>Batch Code : " + batchCode;
            tc1014.HorizontalAlign = HorizontalAlign.Left;
            th104.Cells.Add(tc1014);
            tbl.Rows.Add(th104);

            //TableHeaderRow th105 = new TableHeaderRow();
            //th105.CssClass = "sample3";
            //TableHeaderCell tc1016 = new TableHeaderCell();
            //tc1016.Width = Unit.Percentage(100);
            //tc1016.ColumnSpan = NumberOf_ColumnSpan;
            //tc1016.Text = "End Date : " +dateto.ToString("dd-MMM-yyyy");
            //tc1016.HorizontalAlign = HorizontalAlign.Left;
            //th105.Cells.Add(tc1016);
            //tbl.Rows.Add(th105);

            //TableHeaderRow th2 = new TableHeaderRow();
            ////th2.CssClass = "head1";
            //th2.CssClass = "sample3";

            //TableHeaderCell tc2 = new TableHeaderCell();
            //tc2.Width = Unit.Percentage(100);
            //tc2.ColumnSpan = NumberOf_ColumnSpan;
            //tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
            //tc2.HorizontalAlign = HorizontalAlign.Right;
            //th2.Cells.Add(tc2);
            //tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tc = new TableHeaderCell();
            tc.Width = Unit.Percentage(1);
            tc.Text = "<b>#</b>";
            tc.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tc);

            //TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.Width = Unit.Percentage(8);
            //tcCol1.Text = "<b>Student Number</b>";
            //tcCol1.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "<b>Student Name</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(5);
            tcCol3.Text = "<b>Father Name</b>";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            //tcCol4.Text = "<b>Father/Guardian Name</b>";
            tcCol4.Text = "<b>Mother Name</b>";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(6);
            tcCol5.Text = "<b>Locality /Block</b>";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(6);
            tcCol6.Text = "<b>District</b>";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "<b>City</b>";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(6);
            tcCol8.Text = "<b>State</b>";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            //tcCol8.Wrap = false;
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(8);
            tcCol9.Text = "<b>Pin</b>";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(6);
            tcCol10.Text = "<b>Email ID</b>";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            //tcCol10.Wrap = true;
            th.Cells.Add(tcCol10);
            //TableHeaderCell tcCol10 = new TableHeaderCell();
            //tcCol10.Width = Unit.Percentage(4);
            //tcCol10.Text = "<b>Email ID</b>";
            //tcCol10.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol10);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(8);
            tcCol11.Text = "<b>Mobile No.</b>";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);

            TableHeaderCell tcCol12 = new TableHeaderCell();
            tcCol12.Width = Unit.Percentage(8);
            tcCol12.Text = "<b>Aadhar No.</b>";
            tcCol12.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol12);

            //TableHeaderCell tcCol13 = new TableHeaderCell();
            //tcCol13.Width = Unit.Percentage(8);
            //tcCol13.Text = "<b>Any Other Details</b>";
            //tcCol13.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol13);

            TableHeaderCell tcCol51 = new TableHeaderCell();
            tcCol51.Width = Unit.Percentage(8);
            tcCol51.Text = "<b>Gender</b>";
            tcCol51.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol51);

            TableHeaderCell tcCol61 = new TableHeaderCell();
            tcCol61.Width = Unit.Percentage(6);
            tcCol61.Text = "<b>DOB</b>";
            tcCol61.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol61);

            TableHeaderCell tcCol91 = new TableHeaderCell();
            tcCol91.Width = Unit.Percentage(8);
            tcCol91.Text = "<b>Category</b>";
            tcCol91.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol91);

            TableHeaderCell tcCol101 = new TableHeaderCell();
            tcCol101.Width = Unit.Percentage(6);
            tcCol101.Text = "<b>Regis tration No.</b>";
            tcCol101.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol101);

            TableHeaderCell tcCol16 = new TableHeaderCell();
            tcCol16.Width = Unit.Percentage(4);
            tcCol16.Text = "<b>Admi ssion Date </b>";
            tcCol16.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol16);

            TableHeaderCell tcCol71 = new TableHeaderCell();
            tcCol71.Width = Unit.Percentage(8);
            tcCol71.Text = "<b>Course Start Date</b>";
            tcCol71.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol71);

            TableHeaderCell tcCol81 = new TableHeaderCell();
            tcCol81.Width = Unit.Percentage(8);
            tcCol81.Text = "<b>Course End Date</b>";
            tcCol81.HorizontalAlign = HorizontalAlign.Center;
            //tcCol8.Wrap = false;
            th.Cells.Add(tcCol81);

            TableHeaderCell tcCol14 = new TableHeaderCell();
            tcCol14.Width = Unit.Percentage(6);
            tcCol14.Text = "<b>Result Status</b>";
            tcCol14.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol14);

            TableHeaderCell tcCol15 = new TableHeaderCell();
            tcCol15.Width = Unit.Percentage(4);
            tcCol15.Text = "<b>Employ ment Status</b>";
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol15);

            TableHeaderCell tcCol111 = new TableHeaderCell();
            tcCol111.Width = Unit.Percentage(8);
            tcCol111.Text = "<b>Remark</b>";
            tcCol111.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol111);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "8");

            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 1;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();

            divReportData.Controls.Add(tbl);
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NielitCentreBatchWiseStudentsDetails.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
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
            Response.AddHeader("content-disposition", "attachment;filename=NielitCentreBatchWiseStudentsDetails.xls");
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
    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    private string EmailConvertIntoTextForm(string email)
        {
        try
            {
            string emailFinal = "";
            for (int e = 0; e < email.Length; e++)
                {
                string e1 = email.Substring(e, 1);
                string e2 = "";
                if (e1 == ".")
                    {
                    e1 = "[dot]";
                    }
                else if (e1 == "@")
                    {
                    e1 = "[at]";
                    }
                e2 = e1;
                emailFinal += e2;
                }
            return emailFinal;
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    #endregion
}