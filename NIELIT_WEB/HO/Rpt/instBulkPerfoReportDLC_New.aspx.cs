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

public partial class instBulkPerfoReportDLC_New : BasePage
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
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                tbl.CssClass = "sample3";
                tbl.BorderStyle  = BorderStyle .Solid ;
                
                
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
        if (txtValidUptoStart.Text.Length == 0)
            return;

        if (txtValidUptoEnd.Text.Length == 0)
            return;

        //if (txtYears.Text.Length == 0 )
        //{
            
        //    return;
        //}
        //else
        //{
        //    Int16 vYears = Convert.ToInt16(txtYears.Text);
        //    if( vYears < 0)
        //    {
        //        ShowAlert("Check Years");
        //        return;
        //    }
        //}

        DateTime vstartDate=Convert.ToDateTime(txtValidUptoStart .Text);
        DateTime vEndDate=Convert.ToDateTime (txtValidUptoEnd .Text);
        if (vstartDate  > vEndDate )
        {
            ShowAlert("Incorrect dates");
            return;
        }
        try
        {
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
        
            
            tc1.ColumnSpan = 17;
           // tc1.Text = "Performance Report of DLC (BCC,CCC,CCC+, ECC) . <br/> Institute validity expiring from: " + txtValidUptoStart .Text +" to " + txtValidUptoEnd .Text + "<br/> Report generated for: "+ txtYears .Text +" years";
            tc1.Text = "Performance Report of DLC (BCC,CCC,CCC+, ECC) . <br/> Institute validity expiring from: " + txtValidUptoStart.Text + " to " + txtValidUptoEnd.Text + "<br/> Report generated for: 2.5 years";
         
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);
            //Added 8 May 2019
            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
                     
            tc2.ColumnSpan = 17;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString ();
            tc2.HorizontalAlign = HorizontalAlign.Right;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            //
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);


            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "E.Prov.No.";
            // tcCol4.Text = "Accr No.";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            //TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.Width = Unit.Percentage(1);
            //tcCol1.Text = "Institute ID";
            //tcCol1.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(20);
            tcCol2.Text = "Name";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            //Added for Address
            TableHeaderCell tcCol2f = new TableHeaderCell();
            tcCol2f.Width = Unit.Percentage(3);
            tcCol2f.Text = "Address";
            tcCol2f.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2f);

            //Added for city Name
            TableHeaderCell tcCol2b = new TableHeaderCell();
            tcCol2b.Width = Unit.Percentage(3);
            tcCol2b.Text = "City Name";
            tcCol2b.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2b);

            //TableHeaderCell tcCol3 = new TableHeaderCell();
            //tcCol3.Width = Unit.Percentage(3);
            //tcCol3.Text = "City Type";
            //tcCol3.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol3);

            ////Added for email 
            //TableHeaderCell tcCol2c = new TableHeaderCell();
            //tcCol2c.Width = Unit.Percentage(1);
            //tcCol2c.Text = "Email";
            //tcCol2c.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol2c);

            //Added for State
            TableHeaderCell tcCol2e = new TableHeaderCell();
            tcCol2e.Width = Unit.Percentage(5);
            tcCol2e.Text = "State Name";
            tcCol2e.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2e);
            //Added for Pin Code
            TableHeaderCell tcCol2g = new TableHeaderCell();
            tcCol2g.Width = Unit.Percentage(3);
            tcCol2g.Text = "Pin Code";
            tcCol2g.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2g);
 //Added for email 
            TableHeaderCell tcCol2c = new TableHeaderCell();
            tcCol2c.Width = Unit.Percentage(1);
            tcCol2c.Text = "Email";
            tcCol2c.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2c);
           
           

            //Added for Mobile No. 
            TableHeaderCell tcCol2d = new TableHeaderCell();
            tcCol2d.Width = Unit.Percentage(1);
            tcCol2d.Text = "Mobile No.";
            tcCol2d.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2d);

            ////Added for State
            //TableHeaderCell tcCol2e = new TableHeaderCell();
            //tcCol2e.Width = Unit.Percentage(5);
            //tcCol2e.Text = "State Name";
            //tcCol2e.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol2e);

            //Added for course count 
            TableHeaderCell tcCol2a = new TableHeaderCell();
            tcCol2a.Width = Unit.Percentage(4);
            tcCol2a.Text = "Course Count";
            tcCol2a.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2a);

            ////Added for city Name
            //TableHeaderCell tcCol2b = new TableHeaderCell();
            //tcCol2b.Width = Unit.Percentage(3);
            //tcCol2b.Text = "City Name";
            //tcCol2b.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol2b);

            //TableHeaderCell tcCol3 = new TableHeaderCell();
            //tcCol3.Width = Unit.Percentage(3);
            //tcCol3.Text = "City Type";
            //tcCol3.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol3);

           // TableHeaderCell tcCol4 = new TableHeaderCell();
           // tcCol4.Width = Unit.Percentage(5);
           // tcCol4.Text = "E.Prov.No.";
           //// tcCol4.Text = "Accr No.";
           // tcCol4.HorizontalAlign = HorizontalAlign.Center;
           // th.Cells.Add(tcCol4);

           
                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(5);
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                tcCol5.Text = "Valid From";
                th.Cells.Add(tcCol5);
           

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "Valid Upto";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            //TableHeaderCell tcCol6a = new TableHeaderCell();
            //tcCol6a.Width = Unit.Percentage(2);
            //tcCol6a.Text = "Accr Years";
            //tcCol6a.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol6a);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(2);
            tcCol7.Text = "Candidates Fielded";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(2);
            tcCol8.Text = "Candidates Appeared";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(2);
            tcCol9.Text = "Candidates Passed";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(1);
            tcCol10.Text = "Pass %";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(1);
            tcCol11.Text = "Whether Eligible";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);

            tbl.Rows.Add(th);           
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
            ShowTableHeader();

            if (txtValidUptoStart.Text.Length == 0)
                return;

            if (txtValidUptoEnd.Text.Length == 0)
                return;

            //if (txtYears.Text.Length == 0)
            //    return;
            //else
            //{
            //    Int16 vYears = Convert.ToInt16(txtYears.Text);
            //    if (vYears < 0)
            //    {
            //        ShowAlert("Check Years");
            //        return;
            //    }
            //}

            DateTime vstartDate = Convert.ToDateTime(txtValidUptoStart.Text);
            DateTime vEndDate = Convert.ToDateTime(txtValidUptoEnd.Text);
            if (vstartDate > vEndDate)
            {
                ShowAlert("Incorrect dates");
                return;
            }
            txtYears.Text = "2.5";
            float a = float.Parse(txtYears.Text);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

            SqlCommand scCommand = new SqlCommand("genBulkInstitutePerfoReport_New", new SqlConnection(con.ConnectionString));
             scCommand.CommandType = CommandType.StoredProcedure;
             scCommand.Parameters.Add("@pValidUptoStartDate", SqlDbType.Date ).Value =Convert.ToDateTime (txtValidUptoStart .Text  ) ;
             scCommand.Parameters.Add("@pValidUptoEndDate", SqlDbType.Date).Value = Convert.ToDateTime(txtValidUptoEnd.Text);
            // scCommand.Parameters.Add("@pYears", SqlDbType.Int ).Value = Convert.ToInt32(txtYears.Text);
             scCommand.Parameters.Add("@pYears", SqlDbType.Float).Value = float.Parse(txtYears.Text);
            //Added 8 May 2019
            if (txtCourseCount .Text.Trim().Length ==0)
                scCommand.Parameters.Add("@pCourseCount", SqlDbType.Int).Value =0;
            else
             scCommand.Parameters.Add("@pCourseCount", SqlDbType.Int).Value = Convert.ToInt32(txtCourseCount.Text);
            scCommand.Parameters.Add("@pEnterBy", SqlDbType.Int  ).Value =Convert.ToInt32 (Session ["UserID"]) ;
            scCommand.CommandTimeout = 50000;
                   
                        if (scCommand.Connection.State == ConnectionState.Closed)
                        {
                            scCommand.Connection.Open();
                        }

            SqlDataAdapter da=new SqlDataAdapter (scCommand );
            DataSet ds=new DataSet ();

            da.Fill(ds);

            if(ds.Tables [0].Rows .Count >0)
            {
                 int i ;
           for(i=0;i<ds.Tables [0].Rows .Count ;i++)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";

                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(1);
                tdRow.Text = (i+1).ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow);

               //Commented 8 May 2019
                //TableCell tdRow1 = new TableCell();
                //tdRow1.Width = Unit.Percentage(1);
                //tdRow1.Text = ds.Tables [0].Rows [i]["instituteID"].ToString();
                //tdRow1.HorizontalAlign = HorizontalAlign.Left;
                //tr.Cells.Add(tdRow1);
                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(5);
                tdRow4.Text = ds.Tables[0].Rows[i]["accrNo"].ToString();
                tdRow4.HorizontalAlign = HorizontalAlign.Left;

                tr.Cells.Add(tdRow4);
               
                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(40);
                tdRow2.Text = ds.Tables [0].Rows [i]["Name"].ToString();
                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                
                tr.Cells.Add(tdRow2);

                //Added for Address
                TableCell tdRow2f = new TableCell();
                tdRow2f.Width = Unit.Percentage(3);
                tdRow2f.Text = ds.Tables[0].Rows[i]["address"].ToString();
                tdRow2f.HorizontalAlign = HorizontalAlign.Left;

                tr.Cells.Add(tdRow2f);


                //Added for city Name
                TableCell tdRow2b = new TableCell();
                tdRow2b.Width = Unit.Percentage(3);
                tdRow2b.Text = ds.Tables[0].Rows[i]["cityName"].ToString();
                tdRow2b.HorizontalAlign = HorizontalAlign.Left;

                tr.Cells.Add(tdRow2b);
               
                //TableCell tdRow3 = new TableCell();
                //tdRow3.Width = Unit.Percentage(5);
                //tdRow3.Text = ds.Tables[0].Rows[i]["Description"].ToString();
                //tdRow3.HorizontalAlign = HorizontalAlign.Left;

                //tr.Cells.Add(tdRow3);

                //Added for state
                TableCell tdRow2e = new TableCell();
                tdRow2e.Width = Unit.Percentage(3);
                tdRow2e.Text = ds.Tables[0].Rows[i]["state"].ToString();
                tdRow2e.HorizontalAlign = HorizontalAlign.Center;

                tr.Cells.Add(tdRow2e);
                //Added for Pincode
                TableCell tdRow2g = new TableCell();
                tdRow2g.Width = Unit.Percentage(3);
                tdRow2g.Text = ds.Tables[0].Rows[i]["pinCode"].ToString();
                tdRow2g.HorizontalAlign = HorizontalAlign.Left;

                tr.Cells.Add(tdRow2g);

                //Added for email
                TableCell tdRow2c = new TableCell();
                tdRow2c.Width = Unit.Percentage(1);
                tdRow2c.Text = ds.Tables[0].Rows[i]["email1"].ToString();
                tdRow2c.HorizontalAlign = HorizontalAlign.Center;

                tr.Cells.Add(tdRow2c);

                //Added for mobile
                TableCell tdRow2d = new TableCell();
                tdRow2d.Width = Unit.Percentage(1);
                tdRow2d.Text = ds.Tables[0].Rows[i]["mobileNo"].ToString();
                tdRow2d.HorizontalAlign = HorizontalAlign.Center;

                tr.Cells.Add(tdRow2d);

                ////Added for state
                //TableCell tdRow2e = new TableCell();
                //tdRow2e.Width = Unit.Percentage(3);
                //tdRow2e.Text = ds.Tables[0].Rows[i]["state"].ToString();
                //tdRow2e.HorizontalAlign = HorizontalAlign.Center;

                //tr.Cells.Add(tdRow2e);

                //Added for course Count
                TableCell tdRow2a = new TableCell();
                tdRow2a.Width = Unit.Percentage(3);
                tdRow2a.Text = ds.Tables[0].Rows[i]["courseCount"].ToString();
                tdRow2a.HorizontalAlign = HorizontalAlign.Center;

                tr.Cells.Add(tdRow2a);

               ////Added for city Name
               // TableCell tdRow2b = new TableCell();
               // tdRow2b.Width = Unit.Percentage(3);
               // tdRow2b.Text = ds.Tables[0].Rows[i]["cityName"].ToString();
               // tdRow2b.HorizontalAlign = HorizontalAlign.Left;

               // tr.Cells.Add(tdRow2b);

                
               // TableCell tdRow3 = new TableCell();
               // tdRow3.Width = Unit.Percentage(5);
               // tdRow3.Text = ds.Tables [0].Rows [i]["Description"].ToString();
               // tdRow3.HorizontalAlign = HorizontalAlign.Left;
                
               // tr.Cells.Add(tdRow3);

                // TableCell tdRow4 = new TableCell();
                //tdRow4.Width = Unit.Percentage(5);
                //tdRow4.Text = ds.Tables [0].Rows [i]["accrNo"].ToString();
                //tdRow4.HorizontalAlign = HorizontalAlign.Left;
                
                //tr.Cells.Add(tdRow4);

                TableCell tdRow5 = new TableCell();
                tdRow5.Width = Unit.Percentage(6);
                if (ds.Tables[0].Rows[i]["ValidFrom"] != null)
                {
                    DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ValidFrom"]);
                    tdRow5.Text = dt1.ToString("dd-MMM-yyyy");
                }
                tdRow5.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow5);

                TableCell tdRow6 = new TableCell();
                tdRow6.Width = Unit.Percentage(6);
                if (ds.Tables[0].Rows[i]["ValidUpTo"] != null)
                {
                    DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ValidUpTo"]);
                    tdRow6.Text = dt1.ToString("dd-MMM-yyyy");
                }
                tdRow6.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow6); 
               ////Added 6 May 2019
               // TableCell tdRow6a = new TableCell();
               // tdRow6a.Width = Unit.Percentage(2);
               // if (ds.Tables[0].Rows[i]["accrYears"] != null)
               // {
               //     decimal  dt1 = Convert.ToDecimal (ds.Tables[0].Rows[i]["accrYears"]);
               //     //Int32  dt1 = Convert.ToInt32 (ds.Tables[0].Rows[i]["accrYears"]);
               //     tdRow6a.Text = dt1.ToString();
               // }
               // tdRow6a.HorizontalAlign = HorizontalAlign.Center;
               // tr.Cells.Add(tdRow6a);
               //
               TableCell tdRow7 = new TableCell();
                tdRow7.Width = Unit.Percentage(2);
                tdRow7.Text =ds.Tables [0].Rows [i]["candFielded"].ToString();
                tdRow7.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow7);

                TableCell tdRow8 = new TableCell();
                tdRow8.Width = Unit.Percentage(2);
                tdRow8.Text =ds.Tables [0].Rows [i]["candAppeared"].ToString();
                tdRow8.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow8);

                TableCell tdRow9 = new TableCell();
                tdRow9.Width = Unit.Percentage(2);
                tdRow9.Text =ds.Tables [0].Rows [i]["candPassed"].ToString();
                tdRow9.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow9);

               
                TableCell tdRow10 = new TableCell();
                tdRow10.Width = Unit.Percentage(1);
                tdRow10.Text =ds.Tables [0].Rows [i]["passPercent"].ToString();
                tdRow10.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow10);

               
                TableCell tdRow11 = new TableCell();
                tdRow11.Width = Unit.Percentage(1);
                tdRow11.Text =ds.Tables [0].Rows [i]["whetherEligible"].ToString();
                tdRow11.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow11);
                
                tbl.Rows.Add(tr);
                scCommand.Connection.Close();
            }
          }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
                      
   }

    protected void getData()
    {
        try
        {
            ShowTableHeader();

            if (txtValidUptoStart.Text.Length == 0)
                return;

            if (txtValidUptoEnd.Text.Length == 0)
                return;

            //if (txtYears.Text.Length == 0)
            //    return;
            txtYears.Text = "2.5";
            float a = float.Parse(txtYears.Text);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

            SqlCommand scCommand = new SqlCommand("getBulkInstitutePerfoReport_New", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@pValidUptoStartDate", SqlDbType.Date).Value = Convert.ToDateTime(txtValidUptoStart.Text);
            scCommand.Parameters.Add("@pValidUptoEndDate", SqlDbType.Date).Value = Convert.ToDateTime(txtValidUptoEnd.Text);
           // scCommand.Parameters.Add("@pYears", SqlDbType.Int).Value = Convert.ToInt32(txtYears.Text);
            scCommand.Parameters.Add("@pYears", SqlDbType.Float).Value = float.Parse(txtYears.Text);
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

                    //Commented 8 May 2019
                    //TableCell tdRow1 = new TableCell();
                    //tdRow1.Width = Unit.Percentage(1);
                    //tdRow1.Text = ds.Tables [0].Rows [i]["instituteID"].ToString();
                    //tdRow1.HorizontalAlign = HorizontalAlign.Left;
                    //tr.Cells.Add(tdRow1);
                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(5);
                    tdRow4.Text = ds.Tables[0].Rows[i]["accrNo"].ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow4);

                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(40);
                    tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2);
                    //Added for Address
                    TableCell tdRow2f = new TableCell();
                    tdRow2f.Width = Unit.Percentage(3);
                    tdRow2f.Text = ds.Tables[0].Rows[i]["address"].ToString();
                    tdRow2f.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2f);


                    //Added for city Name
                    TableCell tdRow2b = new TableCell();
                    tdRow2b.Width = Unit.Percentage(3);
                    tdRow2b.Text = ds.Tables[0].Rows[i]["cityName"].ToString();
                    tdRow2b.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2b);


                    //TableCell tdRow3 = new TableCell();
                    //tdRow3.Width = Unit.Percentage(5);
                    //tdRow3.Text = ds.Tables[0].Rows[i]["Description"].ToString();
                    //tdRow3.HorizontalAlign = HorizontalAlign.Left;

                    //tr.Cells.Add(tdRow3);

                    //Added for state
                    TableCell tdRow2e = new TableCell();
                    tdRow2e.Width = Unit.Percentage(3);
                    tdRow2e.Text = ds.Tables[0].Rows[i]["state"].ToString();
                    tdRow2e.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow2e);

                   
                    //Added for Pincode
                    TableCell tdRow2g = new TableCell();
                    tdRow2g.Width = Unit.Percentage(3);
                    tdRow2g.Text = ds.Tables[0].Rows[i]["pinCode"].ToString();
                    tdRow2g.HorizontalAlign = HorizontalAlign.Left;
                    //Added for email
                    TableCell tdRow2c = new TableCell();
                    tdRow2c.Width = Unit.Percentage(1);
                    tdRow2c.Text = ds.Tables[0].Rows[i]["email1"].ToString();
                    tdRow2c.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow2c);

                    tr.Cells.Add(tdRow2g);
                    //Added for mobile
                    TableCell tdRow2d = new TableCell();
                    tdRow2d.Width = Unit.Percentage(1);
                    tdRow2d.Text = ds.Tables[0].Rows[i]["mobileNo"].ToString();
                    tdRow2d.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow2d);

                    ////Added for state
                    //TableCell tdRow2e = new TableCell();
                    //tdRow2e.Width = Unit.Percentage(3);
                    //tdRow2e.Text = ds.Tables[0].Rows[i]["state"].ToString();
                    //tdRow2e.HorizontalAlign = HorizontalAlign.Center;

                    //tr.Cells.Add(tdRow2e);

                    //Added for course Count
                    TableCell tdRow2a = new TableCell();
                    tdRow2a.Width = Unit.Percentage(3);
                    tdRow2a.Text = ds.Tables[0].Rows[i]["courseCount"].ToString();
                    tdRow2a.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow2a);

                    ////Added for city Name
                    // TableCell tdRow2b = new TableCell();
                    // tdRow2b.Width = Unit.Percentage(3);
                    // tdRow2b.Text = ds.Tables[0].Rows[i]["cityName"].ToString();
                    // tdRow2b.HorizontalAlign = HorizontalAlign.Left;

                    // tr.Cells.Add(tdRow2b);


                    // TableCell tdRow3 = new TableCell();
                    // tdRow3.Width = Unit.Percentage(5);
                    // tdRow3.Text = ds.Tables [0].Rows [i]["Description"].ToString();
                    // tdRow3.HorizontalAlign = HorizontalAlign.Left;

                    // tr.Cells.Add(tdRow3);

                    // TableCell tdRow4 = new TableCell();
                    //tdRow4.Width = Unit.Percentage(5);
                    //tdRow4.Text = ds.Tables [0].Rows [i]["accrNo"].ToString();
                    //tdRow4.HorizontalAlign = HorizontalAlign.Left;

                    //tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(6);
                    if (ds.Tables[0].Rows[i]["ValidFrom"] != null)
                    {
                        DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ValidFrom"]);
                        tdRow5.Text = dt1.ToString("dd-MMM-yyyy");
                    }
                    tdRow5.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow5);

                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(6);
                    if (ds.Tables[0].Rows[i]["ValidUpTo"] != null)
                    {
                        DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ValidUpTo"]);
                        tdRow6.Text = dt1.ToString("dd-MMM-yyyy");
                    }
                    tdRow6.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow6);
                    ////Added 6 May 2019
                    // TableCell tdRow6a = new TableCell();
                    // tdRow6a.Width = Unit.Percentage(2);
                    // if (ds.Tables[0].Rows[i]["accrYears"] != null)
                    // {
                    //     decimal  dt1 = Convert.ToDecimal (ds.Tables[0].Rows[i]["accrYears"]);
                    //     //Int32  dt1 = Convert.ToInt32 (ds.Tables[0].Rows[i]["accrYears"]);
                    //     tdRow6a.Text = dt1.ToString();
                    // }
                    // tdRow6a.HorizontalAlign = HorizontalAlign.Center;
                    // tr.Cells.Add(tdRow6a);
                    //
                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(4);
                    tdRow7.Text = ds.Tables[0].Rows[i]["candFielded"].ToString();
                    tdRow7.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow7);

                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(4);
                    tdRow8.Text = ds.Tables[0].Rows[i]["candAppeared"].ToString();
                    tdRow8.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow8);

                    TableCell tdRow9 = new TableCell();
                    tdRow9.Width = Unit.Percentage(2);
                    tdRow9.Text = ds.Tables[0].Rows[i]["candPassed"].ToString();
                    tdRow9.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow9);


                    TableCell tdRow10 = new TableCell();
                    tdRow10.Width = Unit.Percentage(1);
                    tdRow10.Text = ds.Tables[0].Rows[i]["passPercent"].ToString();
                    tdRow10.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow10);


                    TableCell tdRow11 = new TableCell();
                    tdRow11.Width = Unit.Percentage(1);
                    tdRow11.Text = ds.Tables[0].Rows[i]["whetherEligible"].ToString();
                    tdRow11.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow11);

                    tbl.Rows.Add(tr);
                    scCommand.Connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);
        ShowData();
        divReportData.Controls.Add(tbl);
    }

   
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        if (txtValidUptoStart.Text.Length == 0)
            return;

        if (txtValidUptoEnd.Text.Length == 0)
            return;

        if (txtYears.Text.Length == 0)
            return;

        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
           
                divReportData.Visible = true;
                getData();
                divReportData.Controls.Add(tbl);
                Response.Clear();
            
            Response.AddHeader("content-disposition", "attachment;filename=PerformanceReport.xls");
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
        if (txtValidUptoStart.Text.Length == 0)
            return;

        if (txtValidUptoEnd.Text.Length == 0)
            return;

        if (txtYears.Text.Length == 0)
            return;

          try
        {
           
   
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "2");
              hw.RenderBeginTag(HtmlTextWriterTag.Font );
              hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize , "9");

            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle .Solid ;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);
            
                getData();
                divReportData.Controls.Add(tbl);
            
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4.Rotate (), 5f, 5f, 5f, 0f);
              
              
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=BulkPerformanceReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            Response.Write(pdfDoc);
            Response.End();            

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
   
}