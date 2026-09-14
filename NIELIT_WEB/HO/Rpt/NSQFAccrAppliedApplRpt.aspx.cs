using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data .SqlClient ;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;
using System.Collections.Generic;

using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;

public partial class NSQFAccrAppliedApplRpt : BasePage
{
    Table tbl = new Table();
    
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/index.aspx");
            //}
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                FillCategories();
                ddlcoursecategory.SelectedValue = "1";
                FillCourses();
                BindNSQFCourse();
                //tbl.CssClass = "sample3";
                //tbl.BorderStyle  = BorderStyle .Solid ;  
                //tbl.CellPadding = 2;
                //tbl.CellSpacing = 1;
                //tbl.Width = Unit.Percentage(100);
                //divReportData.Controls.Add(tbl);                
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderForStatisticsReport()
    {
        //if (ddlcourse.SelectedValue.ToString().Equals("0"))
        //    return;

        //if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
        //    return;       
      
        try
        {
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
        
            
            tc1.ColumnSpan = 8;
            if (RdoNSQFCourseRptChoice.SelectedValue == "5")
            {
                tc1.Text = " NSQF Accredidation Courses  Applied   From " + txtPaymentFromDate.Text + " To " + txPaymentToDate.Text + "";
            }
            else
            {
                tc1.Text = RdoNSQFCourseRptChoice.SelectedItem.Text.ToUpper() + "  Report of NSQF Accredidation Courses  Applied";

            }
  
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);
            
            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            //TableHeaderCell tc2 = new TableHeaderCell();
            //tc2.Width = Unit.Percentage(100);
                     
            //tc2.ColumnSpan = 8;
            //tc2.Text = "Report Generated on: " + System .DateTime .Now.ToLongDateString ();
            //tc2.HorizontalAlign = HorizontalAlign.Right;
           // th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

           
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);
        
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            tcCol2.Text = "Accr No.";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableCell tdRow113 = new TableCell();
            tdRow113.Width = Unit.Percentage(15);
            tdRow113.Text = "City Name";
            tdRow113.HorizontalAlign = HorizontalAlign.Center;

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(15);
            tcCol6.Text = "Institute Name";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            th.Cells.Add(tdRow113);
            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.Text = "State Name ";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);                       

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.Text = "ApplicationDate";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "Contact Details";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(40);
            tcCol7.Text = "NSQF Course(s) Applied";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            //tcCol8.Width = Unit.Percentage(5);
            //tcCol8.Text = "Caste";
            //tcCol8.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol8);

            //TableHeaderCell tcCol9 = new TableHeaderCell();
            //tcCol9.Width = Unit.Percentage(5);
            //tcCol9.Text = "Physically Handicapped";
            //tcCol9.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol9);

            //TableHeaderCell tcCol10 = new TableHeaderCell();
            //tcCol10.Width = Unit.Percentage(10);
            //tcCol10.Text = "Exams Appeared";
            //tcCol10.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol10);

            //TableHeaderCell tcCol11 = new TableHeaderCell();
            //tcCol11.Width = Unit.Percentage(10);
            //tcCol11.Text = "Exams Passed";
            //tcCol11.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol11);

            //TableHeaderCell tcCol12 = new TableHeaderCell();
            //tcCol12.Width = Unit.Percentage(5);
            //tcCol12.Text = "Verifcation Status";
            //tcCol12.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol12);

            //TableHeaderCell tcCol13 = new TableHeaderCell();
            //tcCol13.Width = Unit.Percentage(5);
            //tcCol13.Text = "Rejection Reason(if rejected)";
            //tcCol13.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol13);

            //TableHeaderCell tcCol14 = new TableHeaderCell();
            //tcCol14.Width = Unit.Percentage(5);
            //tcCol14.Text = "Application Status";
            //tcCol14.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol14);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void RdoNSQFCourseRptChoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblheading.Visible = false;
            lblError.Visible = false;
             if (RdoNSQFCourseRptChoice.SelectedValue == "4")
             {
                 ddlCourseName.Enabled = true;
                // ddlcourse.SelectedValue = "0";
                 txtPaymentFromDate.Enabled = false;
                 txPaymentToDate.Enabled = false;
                 txtPaymentFromDate.Text = "";
                 txPaymentToDate.Text = "";
             }
             else if (RdoNSQFCourseRptChoice.SelectedValue == "5")
             {
                 ddlCourseName.Enabled = false;
                 BindNSQFCourse();        
                 txtPaymentFromDate.Enabled = true;
                 txPaymentToDate.Enabled = true;               
             }
             else
             {
                 ddlCourseName.Enabled = false;
                 BindNSQFCourse(); 
                 txtPaymentFromDate.Enabled = false;
                 txPaymentToDate.Enabled = false;
                 txtPaymentFromDate.Text = "";
                 txPaymentToDate.Text = "";
             }                              
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void BindNSQFCourse()
    {
        try
        {
           
            int courseCatId = 6;
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--ALL--", "0");
                if (courseCatId > 0)
                {
                    var courses = from s in context.Courses
                                  join m in context.NSQFFreeCourseMapping on s.ID equals m.mappedCourseID
                                  where s.CourseCategoryID == courseCatId && ( m.effectiveTo == null || m.effectiveTo >= System.DateTime.Now)
                                  select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
                   
                    courses = courses.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);

                }
                else
                {
                    ddlCourseName.Items.Clear();
                    ddlCourseName.Items.Insert(0, lst);
                }
                ddlCourseName.SelectedValue = "0";
               
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowData()
    {      
        try
        {
            
            if (RdoNSQFCourseRptChoice.SelectedValue.ToString()=="")
            {
                ShowAlert("Choose one of the options");
                return;
            }
            if (RdoNSQFCourseRptChoice.SelectedValue.ToString() == "5")
            {
                if (txtPaymentFromDate.Text.Length == 0)
                {
                    ShowAlert(" From Date.");
                    return;
                }
                if (txPaymentToDate.Text.Length == 0)
                {
                    ShowAlert(" To Date");
                    return;
                }
            }

            if ((txPaymentToDate.Text.Length == 0) && (txtPaymentFromDate.Text.Length == 0))
            {
                txtPaymentFromDate.Text = "01-01-1900";
                txPaymentToDate.Text = "01-01-1900";
            }

            DateTime FromDate = Convert.ToDateTime(txtPaymentFromDate.Text);
            DateTime ToDate = Convert.ToDateTime(txPaymentToDate.Text);
            if (FromDate > ToDate)
            {
                ShowAlert("Incorrect dates");
                return;
            }
            Int64 gtot = 0;
            int choice = Convert.ToInt32(RdoNSQFCourseRptChoice.SelectedValue.ToString());
           // EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("getGridNSQFFreeCoursesAppliedReport", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@Choice", SqlDbType.Int).Value = choice;
            scCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = Convert.ToInt32(ddlCourseName.SelectedValue);           
            scCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(txtPaymentFromDate.Text);
            scCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = Convert.ToDateTime(txPaymentToDate.Text);
            scCommand.CommandTimeout = 50000;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            if ((txPaymentToDate.Text == "01-01-1900") && (txtPaymentFromDate.Text == "01-01-1900"))
            {
                txtPaymentFromDate.Text = "";
                txPaymentToDate.Text = "";
            }
            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();

            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gtot = Convert.ToInt64( ds.Tables[0].Rows.Count);
                ShowTableHeaderForStatisticsReport();
                int i;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";
         
                    TableCell tdRow = new TableCell();
                    tdRow.Width = Unit.Percentage(1);
                    tdRow.Text = (i + 1).ToString();
                    tdRow.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow);


                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(4);
                    tdRow2.Text = ds.Tables[0].Rows[i]["Accreditation_Number"].ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2);


                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(5);
                    tdRow3.Text = ds.Tables[0].Rows[i]["InstituteName"].ToString();// Convert.ToDateTime( .ToString("dd/MMM/yyyy");
                    tdRow3.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow3);

                    TableCell tdRow16 = new TableCell();
                    tdRow16.Width = Unit.Percentage(5);
                    tdRow16.Text = ds.Tables[0].Rows[i]["City_Name"].ToString();
                    tdRow16.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow16);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(20);
                    tdRow4.Text = ds.Tables[0].Rows[i]["StateName"].ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(10);
                    tdRow5.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["ApplicationDate"]).ToString("dd/MMM/yyyy");
                    tdRow5.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow5);

                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(5);
                    tdRow7.Text = ds.Tables[0].Rows[i]["ContactDetails"].ToString();
                    tdRow7.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow7);

                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(25);
                    tdRow8.Text = ds.Tables[0].Rows[i]["NSQFCourseNameApllied"].ToString();
                    tdRow8.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow8);
                   
                    tbl.Rows.Add(tr);

                }


                TableRow trGrand = new TableRow();
                TableCell tCell1 = new TableCell();
                tCell1.Width = Unit.Percentage(1);
                tCell1.HorizontalAlign = HorizontalAlign.Center;
                trGrand.Cells.Add(tCell1);

                TableCell tCell2 = new TableCell();
                tCell2.Width = Unit.Percentage(10);
                tCell2.Text = "Total Record -";
                tCell2.Font.Bold = true;
                tCell2.HorizontalAlign = HorizontalAlign.Center;
                trGrand.Cells.Add(tCell2);



                TableCell tCell3 = new TableCell();
                tCell3.ColumnSpan = 6;
                tCell3.Width = Unit.Percentage(10);
                tCell3.Text = gtot.ToString().Trim();
                tCell3.Font.Bold = true;
                tCell3.HorizontalAlign = HorizontalAlign.Left;

                trGrand.Cells.Add(tCell3);

                tbl.Rows.Add(trGrand);
                //lblError.Visible = false;
                //lblheading.Visible = true;
                //lblheadingCandDetails.Visible = false;
                //hddbtnR.Value = "1";
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
                lblheading.Visible = true;
                lblheadingCandDetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
                      
   }       

   protected void ShowTableHeaderForCandData()
   {
       if (ddlcourse.SelectedValue.ToString().Equals("0"))
           return;

       if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
           return;

       try
       {
           TableHeaderRow th1 = new TableHeaderRow();
           th1.CssClass = "head1";

           TableHeaderCell tc1 = new TableHeaderCell();
           tc1.Width = Unit.Percentage(100);


           tc1.ColumnSpan = 9;
           tc1.Text = "Report of Protsahan Puraskar Candidates Details (Finance)  From " + txtPaymentFromDate.Text + " To " + txPaymentToDate.Text + "";

           tc1.HorizontalAlign = HorizontalAlign.Center;
           th1.Cells.Add(tc1);

           tbl.Rows.Add(th1);

           TableHeaderRow th2 = new TableHeaderRow();
           th2.CssClass = "head1";

           TableHeaderCell tc2 = new TableHeaderCell();
           tc2.Width = Unit.Percentage(100);

           tc2.ColumnSpan = 9;
           tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
           tc2.HorizontalAlign = HorizontalAlign.Right;
           th2.Cells.Add(tc2);

           tbl.Rows.Add(th2);


           TableHeaderRow th = new TableHeaderRow();
           th.CssClass = "head1";

           TableHeaderCell tcCol = new TableHeaderCell();
           tcCol.Width = Unit.Percentage(1);
           tcCol.Text = "#";
           tcCol.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol);


           TableHeaderCell tcCol2 = new TableHeaderCell();
           tcCol2.Width = Unit.Percentage(10);
           tcCol2.Text = "Name";
           tcCol2.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol2);

           TableCell tdRow113 = new TableCell();
           tdRow113.Width = Unit.Percentage(15);
           tdRow113.Text = "Registration No.";
           tdRow113.HorizontalAlign = HorizontalAlign.Center;

           TableHeaderCell tcCol6 = new TableHeaderCell();
           tcCol6.Width = Unit.Percentage(15);
           tcCol6.Text = "Father's Name";
           tcCol6.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol6);

           th.Cells.Add(tdRow113);
           TableHeaderCell tcCol3 = new TableHeaderCell();
           tcCol3.Width = Unit.Percentage(15);
           tcCol3.Text = "Exam Appeared ";
           tcCol3.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol3);

           TableHeaderCell tcCol4 = new TableHeaderCell();
           tcCol4.Width = Unit.Percentage(15);
           tcCol4.Text = "Exam Passed";
           tcCol4.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol4);

           TableHeaderCell tcCol5 = new TableHeaderCell();
           tcCol5.Width = Unit.Percentage(20);
           tcCol5.Text = "Bank Details";
           tcCol5.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol5);

           TableHeaderCell tcCol15 = new TableHeaderCell();
           tcCol15.Width = Unit.Percentage(20);
           tcCol15.Text = "Payment Status";
           tcCol15.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol15);

           TableHeaderCell tcCol7 = new TableHeaderCell();
           tcCol7.Width = Unit.Percentage(5);
           tcCol7.Text = "Amount To Be Released";
           tcCol7.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol7);

           

           tbl.Rows.Add(th);
       }
       catch (Exception ex)
       {
           ShowAlert(ex.Message);
       }
   }
   protected void ShowCandidatesDetailsData()
   {
       try
       {    
           if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
           {
               ShowAlert("Select Course Category");
               return;
           }

           if (ddlcourse.SelectedValue.ToString().Equals("0"))
           {
               ShowAlert("Select Course");
               return;
           }
           if (txtPaymentFromDate.Text.Length == 0)
           {
               ShowAlert("Payment From Date.");
               return;
           }
           if (txPaymentToDate.Text.Length == 0)
           {
               ShowAlert("Payment To Date");
               return;
           }

           DateTime PaymentFromDate = Convert.ToDateTime(txtPaymentFromDate.Text);
           DateTime PaymentToDate = Convert.ToDateTime(txPaymentToDate.Text);
           if (PaymentFromDate > PaymentToDate)
           {
               ShowAlert("Incorrect dates");
               return;
           }


           Int64 gtot = 0;
           Int64 totAmountToBeReleased = 0, totCandidates = 0;

           EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
           SqlCommand scCommand = new SqlCommand("getFinanceStatisticsAndCandidateDetailsReport", new SqlConnection(con.ConnectionString));
           scCommand.CommandType = CommandType.StoredProcedure;
           scCommand.Parameters.Add("@View", SqlDbType.Int).Value = 2;
           scCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = Convert.ToInt32(ddlcourse.SelectedValue);
           scCommand.Parameters.Add("@PaymentStatus", SqlDbType.Int).Value = Convert.ToInt32(ddlPaymentStatus.SelectedValue);
           scCommand.Parameters.Add("@PaymentFromDate", SqlDbType.Date).Value = Convert.ToDateTime(txtPaymentFromDate.Text);
           scCommand.Parameters.Add("@PaymentToDate", SqlDbType.Date).Value = Convert.ToDateTime(txPaymentToDate.Text);
           scCommand.CommandTimeout = 50000;
           if (scCommand.Connection.State == ConnectionState.Closed)
           {
               scCommand.Connection.Open();
           }

           SqlDataAdapter da = new SqlDataAdapter(scCommand);
           DataSet ds = new DataSet();

           da.Fill(ds);

           if (ds.Tables[0].Rows.Count > 0)
           {
               ShowTableHeaderForCandData();
               int i;
               for (i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   TableRow tr = new TableRow();
                   if (i % 2 == 0)
                       tr.CssClass = "gdalternate1";
                   else
                       tr.CssClass = "gdrow1";
                  
                   TableCell tdRow = new TableCell();
                   tdRow.Width = Unit.Percentage(1);
                   tdRow.Text = (i + 1).ToString();
                   tdRow.HorizontalAlign = HorizontalAlign.Center;
                   tr.Cells.Add(tdRow);


                   TableCell tdRow2 = new TableCell();
                   tdRow2.Width = Unit.Percentage(4);
                   tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                   tdRow2.HorizontalAlign = HorizontalAlign.Left;

                   tr.Cells.Add(tdRow2);


                   TableCell tdRow3 = new TableCell();
                   tdRow3.Width = Unit.Percentage(5);
                   tdRow3.Text = ds.Tables[0].Rows[i]["FathersName"].ToString();
                   tdRow3.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow3);
                   

                   TableCell tdRow16 = new TableCell();
                   tdRow16.Width = Unit.Percentage(5);
                   tdRow16.Text = ds.Tables[0].Rows[i]["RegnNo"].ToString();
                   tdRow16.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow16);

                  
                   totCandidates = totCandidates +1;

                   TableCell tdRow4 = new TableCell();
                   tdRow4.Width = Unit.Percentage(20);
                   tdRow4.Text = ds.Tables[0].Rows[i]["ExamAppeared"].ToString();
                   tdRow4.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow4);

                   TableCell tdRow5 = new TableCell();
                   tdRow5.Width = Unit.Percentage(15);
                   tdRow5.Text = ds.Tables[0].Rows[i]["ExamPassed"].ToString();
                   tdRow5.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow5);


                   TableCell tdRow7 = new TableCell();
                   tdRow7.Width = Unit.Percentage(5);
                   tdRow7.Text = ds.Tables[0].Rows[i]["BankDetails"].ToString();
                   tdRow7.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow7);

                   TableCell tdRow17 = new TableCell();
                   tdRow17.Width = Unit.Percentage(5);
                   tdRow17.Text = ds.Tables[0].Rows[i]["PaymentStatus"].ToString();
                   tdRow17.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow17);

                   TableCell tdRow8 = new TableCell();
                   tdRow8.Width = Unit.Percentage(5);
                   tdRow8.Text = ds.Tables[0].Rows[i]["AmountReleased"].ToString();
                   tdRow8.HorizontalAlign = HorizontalAlign.Center;

                   tr.Cells.Add(tdRow8);

                  Int64 AmountReleased = Convert.ToInt64(ds.Tables[0].Rows[i]["AmountReleased"].ToString());
                  totAmountToBeReleased = totAmountToBeReleased + AmountReleased;

                  

                   tbl.Rows.Add(tr);

               }
               TableRow trNew = new TableRow();
               if (i % 2 == 0)
                   trNew.CssClass = "gdalternate1";
               else
                   trNew.CssClass = "gdrow1";
               TableCell tdNewRow1 = new TableCell();
               //tdNewRow1.Width = Unit.Percentage(1);
               //tdNewRow1.Height = Unit.Percentage(15);
               //tdNewRow1.Text = "";
               //tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
               //tdNewRow1.BorderWidth = 1;
               //tdNewRow1.BorderColor = System.Drawing.Color.White;
               //trNew.Cells.Add(tdNewRow1);               

               TableCell tdNewRow21 = new TableCell();
               tdNewRow21.Width = Unit.Percentage(8);
               tdNewRow21.Height = Unit.Percentage(15);
               tdNewRow21.Text = "<b>Total Candidates :</b> &nbsp;&nbsp;" + totCandidates.ToString();
               tdNewRow21.ColumnSpan = 4;
               tdNewRow21.HorizontalAlign = HorizontalAlign.Right;
               tdNewRow21.BorderWidth = 1;
               tdNewRow21.BorderColor = System.Drawing.Color.White;
               trNew.Cells.Add(tdNewRow21);

               TableCell tdNewRow22 = new TableCell();
               tdNewRow22.Width = Unit.Percentage(8);
               tdNewRow22.Height = Unit.Percentage(15);
               tdNewRow22.Text = "<b>Total Amount To Be Released :</b> &nbsp;&nbsp;" + totAmountToBeReleased.ToString() + " /-";
               tdNewRow22.ColumnSpan = 5;
               tdNewRow22.HorizontalAlign = HorizontalAlign.Right;
               tdNewRow22.BorderWidth = 1;
               tdNewRow22.BorderColor = System.Drawing.Color.White;
               trNew.Cells.Add(tdNewRow22);

               tbl.Rows.Add(trNew);
               lblError.Visible = false;
               lblheading.Visible = false;
               lblheadingCandDetails.Visible = true;
               hddbtnR.Value = "2";
           }
           else
           {
               lblError.Visible = true;
               lblError.Text = "No Record Found !";
               lblheading.Visible = false;
               lblheadingCandDetails.Visible = true; 
           }
       }
       catch (Exception ex)
       {
           ShowAlert(ex.Message);
       }

   }
   protected void btnReportChoice_Click(object sender, EventArgs e)
   {
       lblheading.Visible = true;
       tbl.CssClass = "sample3";
       tbl.CellPadding = 2;
       tbl.CellSpacing = 1;
       tbl.Width = Unit.Percentage(100);
       ShowData();
       divReportData.Controls.Add(tbl);
       divReportData.Visible = true;
   }  
   protected void btnShowStatistics_Click(object sender, EventArgs e)
   {
       tbl.CssClass = "sample3";
       tbl.CellPadding = 2;
       tbl.CellSpacing = 1;
       tbl.Width = Unit.Percentage(100);
       ShowData();
       divReportData.Controls.Add(tbl);
       divReportData.Visible = true;
   }
   protected void btnShowCandidateDetails_Click(object sender, EventArgs e)
    {   
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);
        ShowCandidatesDetailsData();
        divReportData.Controls.Add(tbl);
        divReportData.Visible = true;       
    }
    
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        //if (ddlcourse.SelectedValue.ToString().Equals("0"))
        //    return;

        //if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
        //    return;

        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
           
                divReportData.Visible = true;
                //getData();
                ShowData();
                divReportData.Controls.Add(tbl);
                Response.Clear();

                Response.AddHeader("content-disposition", "attachment;filename=NSQFAccrAppliedReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            htmlWrite.AddAttribute("border", "2");
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        //if (ddlcourse .SelectedValue.ToString ().Equals ("0"))
        //    return;

        //if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
        //    return;
            
          try
        {        
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
              hw.RenderBeginTag(HtmlTextWriterTag.Font );
              hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize , "9");

            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle .Solid ;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);
                    ShowData();
           
            divReportData.Controls.Add(tbl);
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
           // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
              
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NSQFAccrAppliedReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            Response.Write(pdfDoc);
            Response.End();           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse );
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

                var Category = (from s in context.CourseCategories
                                join c in context.Courses on s.ID equals c.CourseCategoryID
                                where c.CourseTypeID == CourseType && s.ID==1
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id                                 
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }  
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
             
        FillCourses();
        divReportData.Visible = false;       
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        divReportData.Visible = false;
        lblheading.Visible = false;
        lblheadingCandDetails.Visible = false;
        lblError.Text = "";
        lblError.Visible = false;
    }
        
    
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Verifies that the control is rendered */
    //}    
}