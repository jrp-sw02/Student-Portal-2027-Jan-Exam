using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class CAND_CourseModuleList : BasePage
{
    Table tbl = new Table();
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("../Home.aspx");
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
            base.ReWriteAction(this.Form);
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
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(2);
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.Text = "Short Name";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(48);
            tcCol3.ColumnSpan = 2;
            tcCol3.Text = "Module Name";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            //TableHeaderCell tcCol4 = new TableHeaderCell();
            //tcCol4.Width = Unit.Percentage(15);
            //tcCol4.HorizontalAlign = HorizontalAlign.Center;
            //tcCol4.Text = "Module Type";
            //th.Cells.Add(tcCol4);

            //TableHeaderCell tcCol5 = new TableHeaderCell();
            //tcCol5.Width = Unit.Percentage(30);
            //tcCol5.Text = "Selection Type";
            //tcCol5.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol5 = new TableHeaderCell();
            //tcCol5.Width = Unit.Percentage(10);
            //tcCol5.Text = "Mother Name";
            //tcCol5.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol6 = new TableHeaderCell();
            //tcCol6.Width = Unit.Percentage(5);
            //tcCol6.Text = "Date of Birth";
            //tcCol6.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol6);

            //TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(13);
            //tcCol7.Text = "Payment Mode";
            //tcCol7.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol7);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            //tcCol8.Width = Unit.Percentage(19);
            //tcCol8.Text = "Payment status";
            //tcCol8.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol8);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowCourseExamData(IEnumerable<Module> application)
    {
        try
        {
            context = new EConnectContext();
            int i = 0;
            foreach (Module app in application)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";

                TableCell tdRow0 = new TableCell();
                tdRow0.Width = Unit.Percentage(2);
                tdRow0.Text = (i + 1).ToString();
                tdRow0.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow0);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(15);
                tdRow1.Text = app.ShortName.ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow1);
                if (app.ModuleTypeID == Convert.ToInt32(enmModuleType.Practical))
                {
                    var filldata = (from f in context.ModulePracticalEligibilities
                                    join m in context.Modules on f.TheoryModuleID equals m.ID
                                    where f.PracticalModuleID == app.ID
                                    select new
                                    {
                                        ModuleID = f.TheoryModuleID,
                                        sname = m.ShortName
                                    }).ToList();
                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(48);
                    tdRow2.Text = app.Name;
                    //+" (";
                    //foreach(var moduleslist in filldata)
                    //{

                    //    tdRow2.Text += moduleslist.sname;

                    //}
                    //tdRow2.Text +=")";
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(48);
                    tdRow3.Text = " (";
                    foreach (var moduleslist in filldata)
                    {

                        tdRow3.Text += moduleslist.sname + ",";

                    }
                    tdRow3.Text = tdRow3.Text.TrimEnd(',') + ")";
                    tdRow3.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow3);
                }
                else
                {
                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(48);
                    tdRow2.ColumnSpan = 2;
                    tdRow2.Text = app.Name;
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);
                }
                //TableCell tdRow3 = new TableCell();
                //tdRow3.Width = Unit.Percentage(15);
                //tdRow3.Text = app.ModuleType.Name.Trim().ToString();
                //tdRow3.HorizontalAlign = HorizontalAlign.Left;
                //tr.Cells.Add(tdRow3);

                //TableCell tdRow4 = new TableCell();
                //tdRow4.Width = Unit.Percentage(30);
                //tdRow4.Text = app.SelectionType.Name.ToString();
                //tdRow4.HorizontalAlign = HorizontalAlign.Left;
                //tr.Cells.Add(tdRow4);

                //TableCell tdRow5 = new TableCell();
                //tdRow5.Width = Unit.Percentage(15);
                //if (app.Candidate.MotherName != null)
                //    tdRow5.Text = app.Candidate.MotherName.ToString();
                //tdRow5.HorizontalAlign = HorizontalAlign.Left;
                //tr.Cells.Add(tdRow5);




                //TableCell tdRow6 = new TableCell();
                //tdRow6.Width = Unit.Percentage(10);
                //if (app.Candidate.DateOfBirth != null)
                //    tdRow6.Text = app.Candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                //tdRow6.HorizontalAlign = HorizontalAlign.Center;
                //tr.Cells.Add(tdRow6);

                //TableCell tdRow7 = new TableCell();
                //tdRow7.Width = Unit.Percentage(10);
                //if (app.DemandNote != null)
                //    tdRow7.Text = app.DemandNote.PaymentMode.Name.ToString();
                //tdRow7.HorizontalAlign = HorizontalAlign.Left;
                //tr.Cells.Add(tdRow7);

                //TableCell tdRow8 = new TableCell();
                //tdRow8.Width = Unit.Percentage(10);
                //tdRow8.HorizontalAlign = HorizontalAlign.Left;
                //tdRow8.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentStatus)(app.PaymentStatusID)).ToString();
                //tr.Cells.Add(tdRow8);

                tbl.Rows.Add(tr);
                i++;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }

    }
    protected void ShowData()
    {
        try
        {
            
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int RevisionId = Convert.ToInt32(Request.QueryString["RevisionId"]);
            Int32 practical = Convert.ToInt32(enmModuleType.Practical);
            Int32 project = Convert.ToInt32(enmModuleType.Project);
            Int32 theory = Convert.ToInt32(enmModuleType.Theory);
            Int32 elective = Convert.ToInt32(enmSelectionType.Elective);
            Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
            string strHead = "";
            //strHead += "</br> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
            if (CourseId != 0)
            {
                    using (EConnectContext context = new EConnectContext())
                    {
                        //var modules = from s in context.Modules
                        // where s.CourseID == CourseId && s.RevisionNumber==RevisionId
                        // select new { CourseID = s.CourseID, ID = s.ID, SeletionTID=s.SelectionTypeID , RevNo=s.RevisionNumber  ,Name = s.Name, SName=s.ShortName, Code = s.Code, ModuleTypeID=s.ModuleTypeID ,MType=s.ModuleType.Name, EC=(s.ElectiveGroup.HasValue)?"Yes":"No"};
                        
                        var thmodules = (from s in context.Modules
                                         where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID == theory && s.SelectionTypeID!=elective
                         select s).ToList();

                        if (thmodules.Count() > 0)
                        {
                            ShowTableHeader();
                            //strHead += ", <b>Course Category :</b> " + thmodules.FirstOrDefault().CourseCategory.Name;
                            //strHead += " , <b>Course : </b>" + thmodules.FirstOrDefault().Course.Name + "(" + thmodules.FirstOrDefault().Course.Code + ")";
                            if (RevisionId > 3)
                            {
                                if (RevisionId == 6)
                                {
                                    string Rv = "5.1";
                                    strHead += "List of Modules of <b>Course : </b>" + thmodules.FirstOrDefault().Course.Name + " <b>Revision Number :</b> " + Rv + "<sup>th</sup>";
                                }
                                else
                                strHead += "List of Modules of <b>Course : </b>" + thmodules.FirstOrDefault().Course.Name + " <b>Revision Number :</b> " + thmodules.FirstOrDefault().RevisionNumber + "<sup>th</sup>";
                                //strHead += "List of Theory And Practical/Project Papers <b>Revision Number :</b> " + thmodules.FirstOrDefault().RevisionNumber + "<sup>th</sup>";
                            }
                            else
                            {
                                strHead += "List of Modules of <b>Course : </b>" + thmodules.FirstOrDefault().Course.Name + " <b>Revision Number :</b> " + thmodules.FirstOrDefault().RevisionNumber;
                            }
                            TableRow trTh = new TableRow();
                            trTh.CssClass = "sample2";
                            TableCell tcTh = new TableCell();
                            tcTh.Width = Unit.Percentage(35);
                            tcTh.ColumnSpan = 4;
                            tcTh.Font.Bold = true;
                            tcTh.Text = "Theory Papers";
                            trTh.Cells.Add(tcTh);
                            tbl.Rows.Add(trTh);

                            ShowCourseExamData(thmodules);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }

                        //var electivemodules = (from s in context.Modules
                        //                 where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID == theory && s.SelectionTypeID==elective
                        //                 select s).ToList();
                        //int gp = from s in context.Modules
                        //         where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID == theory && s.SelectionTypeID == elective
                        //         select new { ElectiveGroup = s.ElectiveGroup };

                        var gp = (from s in context.Modules
                                    where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID == theory && s.SelectionTypeID == elective
                                    select new { ElectiveGp = s.ElectiveGroup }).Distinct();
                        foreach (var c in gp)
                        {

                            var electivemodules = (from s in context.Modules
                                                   where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID == theory && s.SelectionTypeID == elective && s.ElectiveGroup==c.ElectiveGp
                                                   select s).ToList();

                            if (electivemodules.Count() > 0)
                            {

                                TableRow trTh = new TableRow();
                                trTh.CssClass = "sample2";
                                TableCell tcTh = new TableCell();
                                tcTh.Width = Unit.Percentage(35);
                                tcTh.ColumnSpan = 4;
                                tcTh.Font.Bold = true;
                                tcTh.Text = "Elective Theory Papers Elective Group No " + electivemodules.FirstOrDefault().ElectiveGroup.ToString() + ",  Select Papers: " + electivemodules.FirstOrDefault().NumberOfElectiveModulesAllowed.ToString() ;
                                trTh.Cells.Add(tcTh);
                                tbl.Rows.Add(trTh);
                                ShowCourseExamData(electivemodules);
                            }
                        }
                        var prmodules = (from s in context.Modules
                                         where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID !=theory && s.ModuleTypeID !=bridge
                                         select s).ToList();
                        if (prmodules.Count() > 0)
                        {
                            TableRow trTh = new TableRow();
                            trTh.CssClass = "sample2";
                            TableCell tcTh = new TableCell();
                            tcTh.Width = Unit.Percentage(35);
                            tcTh.ColumnSpan = 3;
                            tcTh.Font.Bold = true;
                            tcTh.Text = "Practical/Project Papers";
                            trTh.Cells.Add(tcTh);

                            TableCell tcTh1 = new TableCell();
                            tcTh1.Width = Unit.Percentage(35);                            
                            tcTh1.Font.Bold = true;
                            tcTh1.Text = "Practical Module Eligibility";
                            trTh.Cells.Add(tcTh1);
                            tbl.Rows.Add(trTh);
                            ShowCourseExamData(prmodules);
                        }

                        var bridgeCourse = (from s in context.Modules
                                         where s.CourseID == CourseId && s.RevisionNumber == RevisionId && s.ModuleTypeID == bridge
                                         select s).ToList();
                        if (bridgeCourse.Count() > 0)
                        {
                            TableRow trTh = new TableRow();
                            trTh.CssClass = "sample2";
                            TableCell tcTh = new TableCell();
                            tcTh.Width = Unit.Percentage(35);
                            tcTh.ColumnSpan = 4;
                            tcTh.Font.Bold = true;
                            tcTh.Text = "Bridge Course Papers";
                            trTh.Cells.Add(tcTh);
                            tbl.Rows.Add(trTh);

                            ShowCourseExamData(bridgeCourse);
                        }
                        //else
                        //{
                        //    lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}

                    };
               
            }

            LblRptSubHeader.Text = strHead;
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