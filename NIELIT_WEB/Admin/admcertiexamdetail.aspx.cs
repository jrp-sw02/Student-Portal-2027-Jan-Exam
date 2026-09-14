using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Common;

public partial class admcertiexamdetail : BasePage
{
	String strMessage = string.Empty;
	EConnectContext context;
	Int32 currentRoleId = 0;
	Table tbl = new Table();

	protected void Page_Load(object sender, EventArgs e)
	{
		Response.CacheControl = "no-cache";
		Response.AddHeader("Progra", "no-cache");
		Response.Expires = -1500;
		try
		{
			lblerror.Visible = false;
			lblerror.Text = "";
			if (IsSessionAlive() == false)
				Response.Redirect("../Index.aspx");
			currentRoleId = Convert.ToInt32(Session["RoleID"]);
			if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/Examination_Cycle.aspx"))
			{
				Response.Write("Sorry! You don't have rights  to view this page");
				Response.End();
			}
			if (!Page.IsPostBack)
			{
				ucSearchBar.AutoCompleteContextKey = Request.QueryString["CourseId"];
				FillResulGradeVersions(Convert.ToInt32(Request.QueryString["CourseId"].ToString()));
				if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
				{
                    showUIOnCond();
					FillExamCycles();
					ShowEditMode();
				}
				else
				{
					ViewState["SortField"] = "";
					ViewState["SortOrder"] = "";
					FillFilterCategories();

					FillFilterExamCycles();
					BindGridView();
					BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Details", "Admin/admcertiexamdetail.aspx?" + Request.QueryString.ToString(), ""));
				}
				if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
					ShowAlert(Request.QueryString["msg"].ToString());
			}
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}

    void showUIOnCond()
    {
        Int32 CourseID = 0;
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
        }
        if (CourseID != 1 && CourseID!=2 && CourseID!=1213)
        {
            Label2.Text = "Roll Number Publishing Date";
            rpubdatelabel.Visible = false;
            rpubdatetext.Visible = false;
        }
       
    }
	protected void ShowEditMode()
	{
		try
		{
		
            strMessage = "";
			Int32 CourseID = 0;
			if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
			{
				CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
			}
			using (EConnectContext context = new EConnectContext())
			{
				Course objCourse = context.Courses.Find(CourseID);
				ddlOccurance.Items.Clear();
				ddlOccurance.Items.Add(new ListItem("--Select One--", "0"));
				ddlOccurance.Items.Add(new ListItem("First", "1"));
				ddlOccurance.Items.Add(new ListItem("Second", "2"));
				ddlOccurance.Items.Add(new ListItem("Third", "3"));
				ddlOccurance.Items.Add(new ListItem("Fourth", "4"));
				ddlOccurance.Items.Add(new ListItem("Fifth", "5"));
				btnMode.ViewMode = ToggleView.Mode.List;
				mltvTab.ActiveViewIndex = 1;
				pnlFilter.Visible = false;
				ucSearchBar.Visible = false;
				btnSave.Visible = true;
				btnCancel.Visible = true;
				btnSave.Text = "Update";
				lblHeading.Text = "Exam Details";
				btnCreate.Visible = false;
				Exam objExm = context.Exams.Find(Convert.ToInt32(Request.QueryString["key"]));
				ddlExamcycle.SelectedValue = objExm.ExaminationCycleID.ToString();
				ddlExamcycle.Enabled = false;
				ddlOccurance.SelectedValue = objExm.DayOccurance.ToString();
				ddlWeek.SelectedValue = objExm.WeekNumber.ToString();
				DateTimeFormatInfo monthName = new DateTimeFormatInfo();
				lbStartMonth.Text = monthName.GetMonthName(objExm.ExamMonth).ToString();
				lbSchedule.Text = EnumUtility.GetDescription(objExm.ExaminationCycle.enmExamSchedule);
				txtYear.Text = objExm.ExamYear.ToString();
				txtYear.Enabled = false;
				trShow.Visible = true;
				trShow1.Visible = true;
				//tr1.Visible = true;
				//tr2.Visible = true;
				trResultpubdate.Visible = false;
				//rpubdatetext.Visible = false;
				txtExamName.Text = objExm.Name.ToString();
				txtExamDate.Text = objExm.ExamStartDate.ToString("dd-MMM-yyyy");
				hfSendDate.Value = txtExamDate.Text;
				DateTime todayDate = DateTime.Now;
				//--txtneftperiod.Text = objExm.NeftExtPeriod.ToString();
				//deep add code 2
				txtfeesubmissionInstituteExtPeriod.Text = objExm.FeeSubmissioInstituteExtPeriod.ToString();
				//deep end add code 2
                if (CourseID == 1)
                    txtOfflinePubdate.Enabled = false;
				ddlversion.SelectedValue = objExm.ResultGradeVersionID.Value.ToString();
				ddlversion_SelectedIndexChanged(ddlversion.SelectedValue, EventArgs.Empty);
				if (objCourse.enmCourseType == enmCourseType.CertificationCourse)
				{
					if (objExm.DateOfPublishingOfTimeTable.HasValue)
					{
						txtpubdate.Text = objExm.DateOfPublishingOfTimeTable.Value.ToString("dd-MMM-yyyy");
						txtpubdate.Visible = true;
						imgPublishDate.Visible = true;
						if (DateTime.Now >= objExm.ExamStartDate || DateTime.Now >= objExm.DateOfPublishingOfTimeTable)
						{
							imgPublishDate.Visible = false;
							txtpubdate.Enabled = false;
						}
					}
					Int32 latefee = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
					Int32 fee = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
					var mxeffectivedate = (from c in context.CutOffDates
										   where c.ExamID == objExm.ID && (c.ActivityID == latefee || c.ActivityID == fee)
										   orderby c.EfferctiveDate descending
										   select new{ effectivedate = c.EfferctiveDate }).FirstOrDefault();
					if (mxeffectivedate != null)
					{
						if (objExm.DateOfPublishingOfTimeTable.HasValue && mxeffectivedate.effectivedate <= DateTime.Now)
						{
							tr1.Visible = true;
							tr2.Visible = true;
							//--tdpract.Visible = false;
							//--tdpract2.Visible = false;
							if (objExm.DateOfPublishingOfRollNumber.HasValue)
							{
                                txtOfflinePubdate.Text = objExm.DateOfPublishingOfRollNumber.Value.ToString("dd-MMM-yyyy");
								if (DateTime.Now >= objExm.DateOfPublishingOfRollNumber.Value)
								{
									txtOfflinePubdate.Enabled = false;
									Calendarextender5.Enabled = false;
								}
							}
						}
					}
                    //Practical AdmitCard Publishing Date.
                    
                        tr1.Visible = true;
                        tr2.Visible = true;
                        tdpract.Visible = true;
                        tdpract2.Visible = true;
                        if (objExm.Online_Admit_Card_Publish_Date.HasValue)
                        {
                            txtOnlinePubdate.Text = objExm.Online_Admit_Card_Publish_Date.Value.ToString("dd-MMM-yyyy");
                            if (DateTime.Now >= objExm.Online_Admit_Card_Publish_Date.Value)
                            {
                                txtOnlinePubdate.Enabled = false;
                                Calendarextender3.Enabled = false;
                            }
                        }
                    
					//Practical AdmitCard Publishing Date.
					
						tr1.Visible = true;
						tr2.Visible = true;
						tdpract.Visible = true;
						tdpract2.Visible = true;
						if (objExm.DateofPublishingPracticalAdmitCard.HasValue)
						{
							txtpracAdmitCard.Text = objExm.DateofPublishingPracticalAdmitCard.Value.ToString("dd-MMM-yyyy");
							if (DateTime.Now >= objExm.DateofPublishingPracticalAdmitCard.Value)
							{
								txtpracAdmitCard.Enabled = false;
								Calendarextender4.Enabled = false;
							}
						}
					
					var maxexamdate = (from c in context.ExamTimeTables
									   where c.ExamID == objExm.ID
									   orderby c.ExamFomDate descending
									   select new
									   {
										   maxdate = c.ExamFomDate
									   }).FirstOrDefault();
					if (maxexamdate != null)
					{
                        if ((objExm.Online_Admit_Card_Publish_Date.HasValue && objExm.DateofPublishingPracticalAdmitCard.HasValue && maxexamdate.maxdate <= DateTime.Now && CourseID==1) || (objExm.DateOfPublishingOfRollNumber.HasValue && objExm.DateofPublishingPracticalAdmitCard.HasValue && maxexamdate.maxdate <= DateTime.Now && CourseID!=1))
						{
							trResultpubdate.Visible = true;
							//rpubdatetext.Visible = true;
							if (objExm.DateOfPublishingOfResult.HasValue)
							{
								txtResultPubDate.Text = objExm.DateOfPublishingOfResult.Value.ToString("dd-MMM-yyyy");
								if (DateTime.Now >= objExm.DateOfPublishingOfResult.Value)
								{
									txtResultPubDate.Enabled = false;
									Calendarextender3.Enabled = false;
								}
							}
						}
					}
					ddlOccurance.Enabled = false;
					//   ddlWeek.Enabled = false;
					showsidelink();
				}
				else if (objCourse.enmCourseType == enmCourseType.CertificationExam)
				{
					tddispatch.Visible = tddispatch1.Visible = tddispatch2.Visible = tddispatch3.Visible = true;
					if (objExm.IsDispatchable == true)
					{
						ddldispatch.SelectedValue = "1";
					}
					else
					{
						ddldispatch.SelectedValue = "2";
					}

					if (objExm.IsBatchProcessable == true)
					{
						ddlbatchprocessing.SelectedValue = "1";
					}
					else
					{
						ddlbatchprocessing.SelectedValue = "2";
					}

					Int32 regcentreReceicveStatus= Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
					Int32 regcentreReceicveStatus1 = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
					if (context.CertificateExamApplications.Where(s => s.ExamID == objExm.ID && (s.ApplicationStatusID == regcentreReceicveStatus || s.ApplicationStatusID == regcentreReceicveStatus1)).Count() > 0)
						ddlbatchprocessing.Enabled = false;
					else
						ddlbatchprocessing.Enabled = true;

					if (objExm.DateOfPublishingOfTimeTable.HasValue)
					{
						txtpubdate.Text = objExm.DateOfPublishingOfTimeTable.Value.ToString("dd-MMM-yyyy");
						txtpubdate.Visible = true;
						imgPublishDate.Visible = true;
						if (DateTime.Now >= objExm.DateOfPublishingOfTimeTable.Value)
						{
							txtpubdate.Enabled = false;
							Calendarextender2.Enabled = false;
						}
					}
					else
					{
						txtpubdate.Visible = true;
						imgPublishDate.Visible = true;
						txtpubdate.Enabled = true;
					}
					Int32 latefee = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
					Int32 fee = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
					var mxeffectivedate = (from c in context.CutOffDates
										   where c.ExamID == objExm.ID && (c.ActivityID == latefee || c.ActivityID == fee)
										   orderby c.EfferctiveDate descending
										   select new
										   {
											   effectivedate = c.EfferctiveDate
										   }).FirstOrDefault();
					if (mxeffectivedate != null)
					{
						if (objExm.DateOfPublishingOfTimeTable.HasValue && mxeffectivedate.effectivedate <= DateTime.Now)
						{
							tr1.Visible = true;
							tr2.Visible = true;
                            
							//--tdpract.Visible = false;
							//--tdpract2.Visible = false;
							if (objExm.DateOfPublishingOfRollNumber.HasValue)
							{
								txtOfflinePubdate.Text = objExm.DateOfPublishingOfRollNumber.Value.ToString("dd-MMM-yyyy");
								if (DateTime.Now >= objExm.DateOfPublishingOfRollNumber.Value)
								{
									txtOfflinePubdate.Enabled = false;
									Calendarextender5.Enabled = false;
								}
							}
						}

					}
					if (mxeffectivedate != null)
					{
						if (objExm.DateOfPublishingOfRollNumber.HasValue && mxeffectivedate.effectivedate <= DateTime.Now)
						{
							trResultpubdate.Visible = true;
							//rpubdatetext.Visible = true;
							if (objExm.DateOfPublishingOfResult.HasValue)
							{
								txtResultPubDate.Text = objExm.DateOfPublishingOfResult.Value.ToString("dd-MMM-yyyy");
								if (DateTime.Now >= objExm.DateOfPublishingOfResult.Value)
								{
									txtResultPubDate.Enabled = false;
									Calendarextender3.Enabled = false;
								}
							}
						}
					}
					ddlOccurance.Enabled = false;
					//     ddlWeek.Enabled = false;
					showsidelink();
				}

				//Get last modified date of current record and save it in ViewState object.
				ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
				//Create an object of record to be modified and assign properties to relevant fields.
				BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objExm.Name, "Admin/admcertiexamdetail.aspx?" + Request.QueryString.ToString(), ""));
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	protected void BindGridView()
	{
		try
		{
			//this is the sample code how to bind the grid control
			context = new EConnectContext();
			Int32 cid1 = 0;
			Int32 CycleID1 = 0;
			if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
			{
				cid1 = Convert.ToInt32(Request.QueryString["CourseId"]);
			}
			if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{
				CycleID1 = Convert.ToInt32(Request.QueryString["CycleID"]);
			}
			// Int32 CouCatID = 0;
			// Int32 CouID = 0;
			Int32 CycleID = 0;
			Int32 ExamYear = 0;
			DateTime dt = DateTime.MaxValue;
			// if (ddlficoursecategory.SelectedValue != "0")
			//    CouCatID = Convert.ToInt32(ddlficoursecategory.SelectedValue);
			// if (ddlflcourse.SelectedValue != "0")
			//     CouID = Convert.ToInt32(ddlflcourse.SelectedValue);
			if (ddlCycle.SelectedValue != "0")
				CycleID = Convert.ToInt32(ddlCycle.SelectedValue);
			if (ddlFilterYear.SelectedValue != "0")
				ExamYear = Convert.ToInt32(ddlFilterYear.SelectedValue);
			string searchString = ucSearchBar.SearchText.Trim().ToUpper();
			string sortOrder = ViewState["SortOrder"].ToString();
			string sortField = ViewState["SortField"].ToString();
			var exams = from s in context.Exams
						select new
						{
							ID = s.ID,
							name = s.Name,
							examCycleID = s.ExaminationCycleID,
							examCycle = s.ExaminationCycle.Name,
							year = s.ExamYear,
							examDate = s.ExamStartDate,
							couID = s.CourseID,
							//courseName= s.CourseCategory.Code + "-" +s.Course.Code ,
							CourseCategoryID = s.CourseCategoryID,
							examMonth = System.Data.Entity.DbFunctions.CreateDateTime(s.ExamYear, s.ExamMonth, 1, 0, 0, 0),
							ExamYear = s.ExamYear,
							PubDate = s.DateOfPublishingOfTimeTable,
							OfflinePubDate = s.DateOfPublishingOfRollNumber,
                            OnlinePubDate = s.Online_Admit_Card_Publish_Date,
                            PracPubDate = s.DateofPublishingPracticalAdmitCard,
							ResultDate = s.DateOfPublishingOfResult


						};
			if (cid1 != 0)
			{
				exams = exams.Where(s => s.couID == cid1);
			}
			if (CycleID1 != 0)
			{
				exams = exams.Where(s => s.examCycleID == CycleID1);
			}
			if (!String.IsNullOrEmpty(searchString))
			{
				exams = exams.Where(s => s.name.ToUpper().Contains(searchString));
			}
			//if (CouCatID != 0)
			//{
			//exams = exams.Where(s => s.CourseCategoryID == CouCatID);
			//}
			// if (CouID != 0)
			// {
			// exams = exams.Where(s => s.couID == CouID);
			//}
			if (CycleID != 0)
			{
				exams = exams.Where(s => s.examCycleID == CycleID);
			}
			if (ExamYear != 0)
			{
				exams = exams.Where(s => s.ExamYear == ExamYear);
			}
			exams = exams.OrderByDescending(s => s.ExamYear);
			if (!string.IsNullOrEmpty(sortOrder))
			{
				switch (sortField)
				{
					case "name":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.name);
						else
							exams = exams.OrderBy(s => s.name);
						break;
					case "examCycle":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.examCycle);
						else
							exams = exams.OrderBy(s => s.examCycle);
						break;
					case "year":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.year);
						else
							exams = exams.OrderBy(s => s.year);
						break;
					case "examDate":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.examDate);
						else
							exams = exams.OrderBy(s => s.examDate);
						break;
					//case "courseName":
					//    if (sortOrder == "DESC")
					//        exams = exams.OrderByDescending(s => s.courseName);
					//    else
					//        exams = exams.OrderBy(s => s.courseName);
					//    break;
					case "examMonth":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.examMonth);
						else
							exams = exams.OrderBy(s => s.examMonth);
						break;
					case "PubDate":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.PubDate);
						else
							exams = exams.OrderBy(s => s.PubDate);
						break;
                    case "OfflinePubDate":
						if (sortOrder == "DESC")
                            exams = exams.OrderByDescending(s => s.OfflinePubDate);
						else
                            exams = exams.OrderBy(s => s.OfflinePubDate);
						break;
                    case "PracPubDate":
                        if (sortOrder == "DESC")
                            exams = exams.OrderByDescending(s => s.PracPubDate);
                        else
                            exams = exams.OrderBy(s => s.PracPubDate);
                        break;
                    case "OnlinePubDate":
                        if (sortOrder == "DESC")
                            exams = exams.OrderByDescending(s => s.OnlinePubDate);
                        else
                            exams = exams.OrderBy(s => s.OnlinePubDate);
                        break;
					case "ResultDate":
						if (sortOrder == "DESC")
							exams = exams.OrderByDescending(s => s.ResultDate);
						else
							exams = exams.OrderBy(s => s.ResultDate);
						break;
					default:
						exams = exams.OrderByDescending(s => s.year);
						break;
				}
			}
			else
				exams = exams.OrderByDescending(s => s.ExamYear);
			PagingBar1.Bind(exams, ref gvMain);
			uPnlGrid.Update();
			uPnlNavigation.Update();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			context.Dispose();
		}
	}
	protected void PageIndexChanged(Int32 NewPageIndex)
	{
		try
		{
			gvMain.PageIndex = PagingBar1.CurrentPageIndex;
			BindGridView();
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}
	protected void ToggleViewMode_Changed(object sender, EventArgs e)
	{
		if (btnMode.ViewMode == ToggleView.Mode.New)
		{
			FillExamCycles();
			btnMode.ViewMode = ToggleView.Mode.List;
			mltvTab.ActiveViewIndex = 1;
			pnlFilter.Visible = false;
			ucSearchBar.Visible = false;
			//Change the heading text as required
			lblHeading.Text = "New Exam Detail";
            trResultpubdate.Visible = false;
			BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Detail", "", ""));
			if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{

				//using (var context = new EConnectContext())
				//{
				//    Int32 cycleID = Convert.ToInt32(Request.QueryString["CycleID"]);
				//    ddlExamcycle.Enabled = false;
				//    ExaminationCycle examCycle = context.ExaminationCycles.Find(cycleID);
				//    ddlExamcycle.SelectedValue = examCycle.ID.ToString();
				//    Int32 Occurance = examCycle.Occurance;
				//    if (Occurance == 1)
				//        lbOccurance.Text = "First";
				//    else if (Occurance == 2)
				//        lbOccurance.Text = "Second";
				//    else if (Occurance == 3)
				//        lbOccurance.Text = "Third";
				//    else if (Occurance == 4)
				//        lbOccurance.Text = "Fourth";
				//    else if (Occurance == 5)
				//        lbOccurance.Text = "Fifth";

				//    DateTimeFormatInfo monthName = new DateTimeFormatInfo();

				//    lblDay.Text = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[examCycle.WeekNumber];
				//    lbStartMonth.Text = monthName.GetMonthName(examCycle.StartingMonth).ToString();
				//    lbSchedule.Text = EnumUtility.GetDescription((enmExamSchedule)examCycle.ExamScheduleID);
				//};
			}
		}
		else
		{
			if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
			{
				if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
				{
					Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CatID=" + Request.QueryString["CatID"]), true);
				}
			}
			else if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{
				Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CycleID=" + Request.QueryString["CycleID"].ToString()), true);
			}
			else
			{
				Response.Redirect("admcertiexamdetail.aspx", true);
			}
		}
	}
	protected void SearchBar_ApplySearch(object sender, EventArgs e)
	{
		try
		{
			BreadCrumb1.Render();
			PagingBar1.CurrentPageIndex = 0;
			gvMain.PageIndex = PagingBar1.CurrentPageIndex;
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}
	protected void SearchBar_Reset(object sender, EventArgs e)
	{
		try
		{
			BreadCrumb1.Render();
			PagingBar1.CurrentPageIndex = 0;
			gvMain.PageIndex = PagingBar1.CurrentPageIndex;
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}
	protected void SaveRecord(object sender, EventArgs e)
	{
		try
		{
			BreadCrumb1.Render();
			if (String.IsNullOrEmpty(Request.QueryString["Key"]))
			{
				using (EConnectContext context = new EConnectContext())
				{
					ExaminationCycle objExamCycle = new EConnect.NIELIT.ExaminationCycle();
					int CycleId = 0;
					if (ddlExamcycle.SelectedValue != "0")
						CycleId = Convert.ToInt32(ddlExamcycle.SelectedValue);
					objExamCycle = context.ExaminationCycles.Find(Convert.ToInt32(CycleId));
					DateTimeFormatInfo monthName = new DateTimeFormatInfo();
					DateTime exmDate;

					int occurance = 0;
					string occuranceNo = "";
					int year = Convert.ToInt32(txtYear.Text);
					int dayOccurance = Convert.ToInt32(ddlOccurance.SelectedValue);
					DayOfWeek weekNumber = (DayOfWeek)Convert.ToInt32(ddlWeek.SelectedValue);
					Exam objExam;
					if (objExamCycle.enmExamSchedule == enmExamSchedule.Monthly)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month++)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
							{
								objExam = new Exam();
								objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
								objExam.CourseID = objExamCycle.CourseID;
								objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
								objExam.CreatedOn = DateTime.Now;
								objExam.ExamStartDate = exmDate;
								objExam.ExaminationCycleID = objExamCycle.ID;
								objExam.ExamYear = year;
								objExam.DayOccurance = dayOccurance;
								objExam.WeekNumber = (Int32)weekNumber;
								objExam.ExamMonth = month > 12 ? month % 12 : month;
								objExam.Name = monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
								objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
								objExamCycle.Exams.Add(objExam);
							}
						}
					}
					else if (objExamCycle.enmExamSchedule == enmExamSchedule.Periodic)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 4)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
							{
								objExam = new Exam();
								objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
								objExam.CourseID = objExamCycle.CourseID;
								objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
								objExam.CreatedOn = DateTime.Now;
								objExam.ExamStartDate = exmDate;
								objExam.ExaminationCycleID = objExamCycle.ID;
								objExam.ExamYear = year;
								objExam.DayOccurance = dayOccurance;
								objExam.WeekNumber = (Int32)weekNumber;
								objExam.ExamMonth = month > 12 ? month % 12 : month;
								objExam.Name = monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
								objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
								objExamCycle.Exams.Add(objExam);
							}
						}
					}
					else if (objExamCycle.enmExamSchedule == enmExamSchedule.Quarterly)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 3)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
							{
								objExam = new Exam();
								objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
								objExam.CourseID = objExamCycle.CourseID;
								objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
								objExam.CreatedOn = DateTime.Now;
								objExam.ExamStartDate = exmDate;
								objExam.ExaminationCycleID = objExamCycle.ID;
								objExam.ExamYear = year;
								objExam.DayOccurance = dayOccurance;
								objExam.WeekNumber = (Int32)weekNumber;
								objExam.ExamMonth = month > 12 ? month % 12 : month;
								objExam.Name = monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
								objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
								objExamCycle.Exams.Add(objExam);
							}
						}
					}
					else if (objExamCycle.enmExamSchedule == enmExamSchedule.HalfYearly)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 6)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
							{
								objExam = new Exam();
								objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
								objExam.CourseID = objExamCycle.CourseID;
								objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
								objExam.CreatedOn = DateTime.Now;
								objExam.ExamStartDate = exmDate;
								objExam.ExaminationCycleID = objExamCycle.ID;
								objExam.ExamYear = year;
								objExam.DayOccurance = dayOccurance;
								objExam.WeekNumber = (Int32)weekNumber;
								objExam.ExamMonth = month > 12 ? month % 12 : month;
								objExam.Name = monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
								objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
								objExamCycle.Exams.Add(objExam);
							}
						}
					}
					else if (objExamCycle.enmExamSchedule == enmExamSchedule.Yearly)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 12)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
							{
								objExam = new Exam();
								objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
								objExam.CourseID = objExamCycle.CourseID;
								objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
								objExam.CreatedOn = DateTime.Now;
								objExam.ExamStartDate = exmDate;
								objExam.ExaminationCycleID = objExamCycle.ID;
								objExam.ExamYear = year;
								objExam.DayOccurance = dayOccurance;
								objExam.WeekNumber = (Int32)weekNumber;
								objExam.ExamMonth = month > 12 ? month % 12 : month;
								objExam.Name = monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
								objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
								objExamCycle.Exams.Add(objExam);
							}
						}
					}
					else if (objExamCycle.enmExamSchedule == enmExamSchedule.Fortnightly)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 1)
						{
							occurance = 0;
							dayOccurance = Convert.ToInt32(ddlOccurance.SelectedValue);
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							DateTime lastDate = exmDate.AddMonths(1);
							lastDate = new DateTime(lastDate.Year, lastDate.Month, 1).AddDays(-1);

							for (DateTime newDate = exmDate; newDate <= lastDate; newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance && dayOccurance <= 4)
									{
										exmDate = newDate;
										dayOccurance = dayOccurance + 2;
										Int32 mod = occurance % 10;
										if (mod == 1)
											occuranceNo = occurance.ToString() +"st ";
										else if (mod == 2)
											occuranceNo = occurance.ToString() + "nd ";
										else if (mod == 3)
											occuranceNo = occurance.ToString() + "rd ";
										else if (mod > 3)
											occuranceNo = occurance.ToString() + "th ";
										exmDate = newDate;

										if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
										{
											objExam = new Exam();
											objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
											objExam.CourseID = objExamCycle.CourseID;
											objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
											objExam.CreatedOn = DateTime.Now;
											objExam.ExamStartDate = exmDate;
											objExam.ExaminationCycleID = objExamCycle.ID;
											objExam.ExamYear = year;
											objExam.DayOccurance = dayOccurance;
											objExam.WeekNumber = (Int32)weekNumber;
											objExam.ExamMonth = month > 12 ? month % 12 : month;
											objExam.Name = occuranceNo.ToString() + " " + weekNumber.ToString() + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
											objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
											objExamCycle.Exams.Add(objExam);
										}
									}

								}
							}
						}
					}
					else if (objExamCycle.enmExamSchedule == enmExamSchedule.Weekly)
					{
						for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 1)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							DateTime lastDate = exmDate.AddMonths(1);
							lastDate = new DateTime(lastDate.Year, lastDate.Month, 1).AddDays(-1);
							for (DateTime newDate = exmDate; newDate <= lastDate; newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									Int32 mod = occurance % 10;
									 if (mod == 1)
											occuranceNo = occurance.ToString() +"st ";
										else if (mod == 2)
											occuranceNo = occurance.ToString() + "nd ";
										else if (mod == 3)
											occuranceNo = occurance.ToString() + "rd ";
										else if (mod > 3)
											occuranceNo = occurance.ToString() + "th ";
									
									if (occurance == dayOccurance)
									{
										exmDate = newDate;

									}
									if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == year && s.ExamStartDate == exmDate) == false)
									{
										objExam = new Exam();
										objExam.CourseCategoryID = objExamCycle.CourseCategoryID;
										objExam.CourseID = objExamCycle.CourseID;
										objExam.CreatedByID = Convert.ToInt32(Session["UserID"]);
										objExam.CreatedOn = DateTime.Now;
										objExam.ExamStartDate = exmDate;
										objExam.ExaminationCycleID = objExamCycle.ID;
										objExam.ExamYear = year;
										objExam.DayOccurance = dayOccurance;
										objExam.WeekNumber = (Int32)weekNumber;
										objExam.ExamMonth = month > 12 ? month % 12 : month;
										objExam.Name = occuranceNo.ToString() + " " + weekNumber.ToString() + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString();
										objExam.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
										objExamCycle.Exams.Add(objExam);
									}
								}
							}


						}
					}
					context.SaveChanges();
					strMessage = "Record saved";
				};
			}

			else
			{
				//context = new EConnectContext();
				using (EConnectContext context = new EConnectContext())
				{
					Int32 moduletypeID = Convert.ToInt32(enmModuleType.Practical);
                    //Int32 moduletypeID1 = Convert.ToInt32(enmModuleType.Theory);
					Exam objExm = context.Exams.Find(Convert.ToInt32(Request.QueryString["key"]));
					objExm.Name = txtExamName.Text;
					objExm.ExamStartDate = Convert.ToDateTime(txtExamDate.Text);
					//Practical Exam-Date
					DateTime pracexamdate = new DateTime();
                    DateTime onlineexamdate = new DateTime();
					Int32 courseid = Convert.ToInt32(Request.QueryString["CourseId"]);
					Course currentcourse = context.Courses.Find(courseid);
					if (currentcourse.enmCourseType == enmCourseType.CertificationCourse)
					{
						var practicaldate = (from r in context.ExamTimeTables
											 where r.ExamID == objExm.ID && r.Module.ModuleTypeID == moduletypeID
											 orderby r.ExamFomDate ascending
											 select r).FirstOrDefault();
                        var onlinedate = (from r in context.ExamTimeTables
                                          where r.ExamID == objExm.ID && (r.ModuleID == 929 || r.ModuleID == 930 || r.ModuleID == 931 || r.ModuleID == 932 || r.ModuleID == 938 || r.ModuleID == 939 || r.ModuleID == 940 || r.ModuleID == 941) && r.ExamFomDate !=null
                                             orderby r.ExamFomDate ascending
                                             select r).FirstOrDefault();
						if (practicaldate != null)
						{
							pracexamdate = practicaldate.ExamFomDate;
						}
                        if (onlinedate != null)
                        {
                            onlineexamdate = onlinedate.ExamFomDate;
                        }
					}

					// to update is_dispatchable and Is_batchprocessing field
					Boolean is_dispatchable = false;
					Boolean is_batchprocessing = false;
					if (currentcourse.enmCourseType == enmCourseType.CertificationExam)
					{
						if (ddldispatch.SelectedValue == "1")
						{
							is_dispatchable = true;
						}
						if (ddldispatch.SelectedValue == "2")
						{
							is_dispatchable = false; ;
						}
						if (ddlbatchprocessing.SelectedValue == "1")
						{
							is_batchprocessing = true;
						}
						if (ddlbatchprocessing.SelectedValue == "2")
                        {
							is_batchprocessing = false; ;
						}
						objExm.IsDispatchable = is_dispatchable;
						objExm.IsBatchProcessable = is_batchprocessing;
					}

                     // commented by abhi singh on dated 28112023
                    //if (!IsNumeric(txtneftperiod.Text))
                    //{
                    //    ShowAlert("Not correct Neft Extension Period.");
                    //    return;
                    //}
                    //else
                    //{
                    //    objExm.NeftExtPeriod = Convert.ToInt32(txtneftperiod.Text);
                    //}
                    objExm.NeftExtPeriod = 0;  // Line added dated on 28112023
					//deep add code 1
					if (!IsNumeric(txtfeesubmissionInstituteExtPeriod.Text))
					{
						ShowAlert("Not correct Fee Submission of Institute Extension Period");
						return;
					}
					else
					{
						objExm.FeeSubmissioInstituteExtPeriod = Convert.ToInt32(txtfeesubmissionInstituteExtPeriod.Text);
					}
					//deep end add code 1
					objExm.ResultGradeVersionID = Convert.ToInt32(ddlversion.SelectedValue);
					if (IsDate(txtpubdate.Text))
						if (Convert.ToDateTime(txtpubdate.Text) >= Convert.ToDateTime(txtExamDate.Text))
						{
							ShowAlert("Publishing date should be less than exam start date");
							return;
						}
						else
						{
							objExm.DateOfPublishingOfTimeTable = Convert.ToDateTime(txtpubdate.Text);
						}
					else
						objExm.DateOfPublishingOfTimeTable = null;
					if (IsDate(txtOfflinePubdate.Text))
						if (Convert.ToDateTime(txtOfflinePubdate.Text) > Convert.ToDateTime(txtExamDate.Text) || Convert.ToDateTime(txtOfflinePubdate.Text) <= Convert.ToDateTime(txtpubdate.Text))
                        //--if ( Convert.ToDateTime(txtOfflinePubdate.Text) <= Convert.ToDateTime(txtpubdate.Text))
						{
							ShowAlert("Roll Number Publishing date should be less then or equal to Start Exam Date and greater then Time-Table publishing date.");
							return;
						}
						else
						{
							objExm.DateOfPublishingOfRollNumber = Convert.ToDateTime(txtOfflinePubdate.Text);
						}

					else
						objExm.DateOfPublishingOfRollNumber = null;
                    // added new feature for Online Publishing date on dated 04082023
                    if (IsDate(txtOnlinePubdate.Text))
                        if (Convert.ToDateTime(txtOnlinePubdate.Text) >= onlineexamdate || Convert.ToDateTime(txtOnlinePubdate.Text) <=Convert.ToDateTime(txtpubdate.Text))
                        //if (Convert.ToDateTime(txtOnlinePubdate.Text) <= Convert.ToDateTime(txtpubdate.Text))
                        {
                            ShowAlert("Online Admit Card Publishing date should be less than Online exam start date and greater then Time-Table publishing date");
                            return;
                        }
                        else
                        {
                            objExm.Online_Admit_Card_Publish_Date = Convert.ToDateTime(txtOnlinePubdate.Text);
                        }

                    else
                        objExm.Online_Admit_Card_Publish_Date = null;

					if (IsDate(txtpracAdmitCard.Text))
						
                        if (Convert.ToDateTime(txtpracAdmitCard.Text) >= pracexamdate || Convert.ToDateTime(txtpracAdmitCard.Text) <= Convert.ToDateTime(txtpubdate.Text))
						{
							ShowAlert("Practical Admit Card Publishing date should be less than practical exam start date and greater then Time-Table publishing date");
							return;
						}
						else
						{
							objExm.DateofPublishingPracticalAdmitCard = Convert.ToDateTime(txtpracAdmitCard.Text);
						}
					else
						objExm.DateofPublishingPracticalAdmitCard = null;
					if (IsDate(txtResultPubDate.Text))
                        if (Convert.ToDateTime(txtResultPubDate.Text) <= Convert.ToDateTime(txtExamDate.Text))
                        {
                            ShowAlert("Result Publishing date should be greater than exam start date");
                            return;
                        }
                        else
                        {
                            objExm.DateOfPublishingOfResult = Convert.ToDateTime(txtResultPubDate.Text);
                        }
                        //objExm.DateOfPublishingOfResult = Convert.ToDateTime(txtResultPubDate.Text);
					else
						objExm.DateOfPublishingOfResult = null;
					objExm.WeekNumber = Convert.ToInt32(ddlWeek.SelectedValue);
					context.SaveChanges();
					strMessage = "Record updated.";
				};
			}

			if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
			{
				if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
				{
					Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CatID=" + Request.QueryString["CatID"]), true);
				}
			}
			else if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{
				Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CycleID=" + Request.QueryString["CycleID"].ToString()), true);
			}
			else
			{
				Response.Redirect("admcertiexamdetail.aspx?msg=" + strMessage);
			}
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}
	protected void AllyFilter(object sender, EventArgs e)
	{
		try
		{
			PagingBar1.CurrentPageIndex = 0;
			gvMain.PageIndex = PagingBar1.CurrentPageIndex;
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}
	protected void ResetFilterPanel(object sender, EventArgs e)
	{
		try
		{
			if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
			{
				ddlCycle.SelectedValue = "0";
			}
			else if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{
				//ddlficoursecategory.SelectedValue = "0";
				//ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
			}
			ddlFilterYear.SelectedValue = "0";
			PagingBar1.CurrentPageIndex = 0;
			gvMain.PageIndex = PagingBar1.CurrentPageIndex;
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}

	protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
	{
		try
		{
			ViewState["SortField"] = e.SortExpression;
			if (ViewState["SortOrder"].ToString() == "DESC")
				ViewState["SortOrder"] = "ASC";
			else
				ViewState["SortOrder"] = "DESC";
			BindGridView();
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}

	protected void PerformPopupAction(object sender, EventArgs e)
	{
		//try
		//{
		//    context = new EConnectContext();
		//    if (hfActionID.Value != "")
		//    {
		//        String recordID = hfActionID.Value.Split('$')[0].ToString();
		//        LinkButton btnAction = (LinkButton)sender;
		//        if (btnAction.CommandName == "Delete")
		//        {
		//            //Load the object and apply validateion if required
		//            //call delete function
		//            //bind the grid again
		//            BindGridView();
		//            ShowAlert("Record deleted successfully.", true);
		//            hfActionID.Value = "";
		//        }
		//        else if (btnAction.CommandName == "Action")
		//        {
		//            //Load the object and apply validateion if required
		//            //call function to perform required action
		//            //bind the grid again
		//            BindGridView();
		//            ShowAlert("Record Action1 successfully.", true);
		//            hfActionID.Value = "";
		//        }
		//        uPnlGrid.Update();
		//    }
		//}
		//catch (Exception ex)
		//{
		//    hfActionID.Value = "";
		//    ShowAlert(ex.Message, true);
		//}
		//finally { context.Dispose(); }
	}

	protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				//Encryption url of hypelink field
				HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
				string href = hl.NavigateUrl;

				if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
				{
					if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
					{
						href += "&CatID=" + Request.QueryString["CatID"].ToString() + "&CourseId=" + Request.QueryString["CourseId"].ToString();
					}
				}
				else if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
				{
					href += "&CycleID=" + Request.QueryString["CycleID"].ToString();
				}
				//e.Row.Cells[4].Text =String.Format("{0:dd-MMM-yyyy}", dt)
				hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
				HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
				HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
				HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
				HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
				// HyperLink h6 = (HyperLink)e.Row.Cells[6].Controls[0];
				//hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&" +Request.QueryString.ToString());
				h2.NavigateUrl = hl.NavigateUrl;
				h3.NavigateUrl = hl.NavigateUrl;
				h4.NavigateUrl = hl.NavigateUrl;
				h5.NavigateUrl = hl.NavigateUrl;
				// h6.NavigateUrl = hl.NavigateUrl;
				e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
				//h3.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(h3.Text));
				//Image imgAction = (Image)e.Row.FindControl("imgAction");
				//imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

				//CheckBox chk = (CheckBox)e.Row.FindControl("chk");
				//imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}   
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
		{
			if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
			{
				Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CatID=" + Request.QueryString["CatID"]), true);
			}
		}
		else if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
		{
			Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CycleID=" + Request.QueryString["CycleID"].ToString()), true);
		}
		else
		{
			Response.Redirect("admcertiexamdetail.aspx", true);
		}
	}
	protected void ddlExamcycle_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
			{
				if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
				{
					using (var context = new EConnectContext())
					{
						Int32 categoryId = Convert.ToInt32(Request.QueryString["CatID"]);
						Int32 cid = Convert.ToInt32(Request.QueryString["CourseId"]);
						Int32 CycleId = Convert.ToInt32(ddlExamcycle.SelectedValue);
						ExaminationCycle examCycle = context.ExaminationCycles.Find(CycleId);
						DateTimeFormatInfo monthName = new DateTimeFormatInfo();

						//lblDay.Text = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[examCycle.WeekNumber];
						lbStartMonth.Text = monthName.GetMonthName(examCycle.StartingMonth).ToString();
						lbSchedule.Text = EnumUtility.GetDescription((enmExamSchedule)examCycle.ExamScheduleID);
						hfScheduleID.Value = examCycle.ExamScheduleID.ToString();
						if (Convert.ToInt32(hfScheduleID.Value) == (Int32)enmExamSchedule.Weekly)
						{
							ddlOccurance.Enabled = false;
						}
						else if (Convert.ToInt32(hfScheduleID.Value) == (Int32)enmExamSchedule.Fortnightly)
						{
							ddlOccurance.Enabled = true;
							ddlOccurance.Items.Clear();
							ddlOccurance.Items.Add(new ListItem("--Select One--", "0"));
							ddlOccurance.Items.Add(new ListItem("First", "1"));
							ddlOccurance.Items.Add(new ListItem("Second", "2"));
						}
						else
						{
							ddlOccurance.Enabled = true;
							ddlOccurance.Items.Clear();
							ddlOccurance.Items.Add(new ListItem("--Select One--", "0"));
							ddlOccurance.Items.Add(new ListItem("First", "1"));
							ddlOccurance.Items.Add(new ListItem("Second", "2"));
							ddlOccurance.Items.Add(new ListItem("Third", "3"));
							ddlOccurance.Items.Add(new ListItem("Fourth", "4"));
							ddlOccurance.Items.Add(new ListItem("Fifth", "5"));
						}
					};
				}
			}
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message, true);
		}
	}
	protected void btnCreate_Click(object sender, EventArgs e)
	{
		try
		{
			BreadCrumb1.Render();
			if (txtYear.Text.Trim() == "")
			{
				throw new Exception("Enter Year of Exam");
			}
			if (Convert.ToInt32(hfScheduleID.Value) != (Int32)enmExamSchedule.Weekly)
			{
				if (ddlOccurance.SelectedValue == "0")
				{
					throw new Exception("Please select starting day of exam");
				}
			}
			ddlOccurance.Enabled = false;
			ddlWeek.Enabled = false;
			if (btnCreate.Text == "Create Exams")
			{
				ddlExamcycle.Enabled = false;
				txtYear.Enabled = false;
				btnCreate.Text = "Reset Exams";

				using (var context = new EConnectContext())
				{
					int CycleId = 0;
					if (ddlExamcycle.SelectedValue != "0")
						CycleId = Convert.ToInt32(ddlExamcycle.SelectedValue);
					ExaminationCycle objExam = context.ExaminationCycles.Find(CycleId);
					Int32 yearName = Convert.ToInt32(txtYear.Text);
					trShowTbl.Visible = true;
					StringBuilder tablestring = new StringBuilder();
					tablestring.Append("<table width='100%' class='sample3' cellspacing='0' cellpadding='3' border='1' id='tblExam'><tr class='head1'><th>#</th><th>Exam Name</th><th>Exam Month</th><th>Exam Year</th><th>Exam Start Date</th></tr>");
					//tablestring.Append("<table width='100%' class='sample3' cellspacing='0' cellpadding='3' border='1' id='tblExam'><tr class='head1'><th>#</th><th>Exam Cycle Name</th><th>Exam Start Date</th><th></th></tr>");
					int rowNumber = 0;
					DateTimeFormatInfo monthName = new DateTimeFormatInfo();
					DateTime exmDate;
					int occurance = 0;
					int counter = 0;
					string occuranceNo = "";
					String styleClass = "";
					int year = Convert.ToInt32(txtYear.Text);
					int dayOccurance = Convert.ToInt32(ddlOccurance.SelectedValue);
					DayOfWeek weekNumber = (DayOfWeek)Convert.ToInt32(ddlWeek.SelectedValue);
					if (objExam.enmExamSchedule == enmExamSchedule.Monthly)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month++)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							counter++;
							if (counter % 2 == 0)
								styleClass = "gdalternate1";
							else
								styleClass = "gdrow1";
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
							{
								rowNumber++;
								tablestring.Append("<tr class=" + styleClass + " ><td align='center'>" + (rowNumber).ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td align='center'>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
								//tablestring.Append("<tr class=" + styleClass + " ><td align='center'>" + (rowNumber).ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td align='center'>" + exmDate.ToString("dd-MMM-yyyy") + "</td><td><input checked=true onclick='alert(this.id);' type='checkbox' id='chk_" + (rowNumber).ToString() + "'/></td></tr>");
							}

						}

					}
					else if (objExam.enmExamSchedule == enmExamSchedule.Periodic)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month = month + 4)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							counter++;
							if (counter % 2 == 0)
								styleClass = "gdalternate1";
							else
								styleClass = "gdrow1";
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
							{
								rowNumber++;
								tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
								//tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + objExam.Name + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td><td><input checked=true onclick='alert(this.id);' type='checkbox' id='chk_" + (rowNumber).ToString() + "' /></td></tr>");
							}

						}
					}
					else if (objExam.enmExamSchedule == enmExamSchedule.HalfYearly)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month = month + 6)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							counter++;
							if (counter % 2 == 0)
								styleClass = "gdalternate1";
							else
								styleClass = "gdrow1";
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
							{
								rowNumber++;
								tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
								//tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + objExam.Name + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td><td><input checked=true onclick='alert(this.id);' type='checkbox' id='chk_" + (rowNumber).ToString() + "' /></td></tr>");
							}

						}
					}
					else if (objExam.enmExamSchedule == enmExamSchedule.Yearly)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month = month + 12)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
							{
								rowNumber++;
								tablestring.Append("<tr><td>" + (rowNumber).ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
								//tablestring.Append("<tr><td>" + (rowNumber).ToString() + "</td><td>" + objExam.Name + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td><td><input checked=true onclick='alert(this.id);' type='checkbox' id='chk_" + (rowNumber).ToString() + "' /></td></tr>");
							}
						}
					}
					else if (objExam.enmExamSchedule == enmExamSchedule.Quarterly)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month = month + 3)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
							{
								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance)
									{
										exmDate = newDate;
										break;
									}
								}
							}
							counter++;
							if (counter % 2 == 0)
								styleClass = "gdalternate1";
							else
								styleClass = "gdrow1";
							if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
							{
								rowNumber++;
								tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
								//tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + objExam.Name + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td><td><input checked=true onclick='alert(this.id);' type='checkbox' id='chk_" + (rowNumber).ToString() + "' /></td></tr>");
							}

						}
					}
					else if (objExam.enmExamSchedule == enmExamSchedule.Fortnightly)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month = month + 1)
						{
							occurance = 0;
							dayOccurance = Convert.ToInt32(ddlOccurance.SelectedValue);
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							DateTime lastDate = exmDate.AddMonths(1);
							lastDate = new DateTime(lastDate.Year, lastDate.Month, 1).AddDays(-1);
							for (DateTime newDate = exmDate; newDate <= lastDate; newDate = newDate.AddDays(1))
							{

								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									if (occurance == dayOccurance && dayOccurance <= 4)
									{
										exmDate = newDate;
										dayOccurance = dayOccurance + 2;
										Int32 mod = occurance % 10;
										 if (mod == 1)
											occuranceNo = occurance.ToString() +"st ";
										else if (mod == 2)
											occuranceNo = occurance.ToString() + "nd ";
										else if (mod == 3)
											occuranceNo = occurance.ToString() + "rd ";
										else if (mod > 3)
											occuranceNo = occurance.ToString() + "th ";
										exmDate = newDate;

										if (counter % 2 == 0)
											styleClass = "gdalternate1";
										else
											styleClass = "gdrow1";
										if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
										{
											rowNumber++;
											tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + occuranceNo.ToString() + weekNumber.ToString() + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
										}
										counter++;
									}
								}

							}
						}
					}
					else if (objExam.enmExamSchedule == enmExamSchedule.Weekly)
					{
						for (int month = objExam.StartingMonth; month < objExam.StartingMonth + 12; month = month + 1)
						{
							occurance = 0;
							if (month > 12 && year == Convert.ToInt32(txtYear.Text))
								year = year + 1;
							exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
							DateTime lastDate = exmDate.AddMonths(1);
							lastDate = new DateTime(lastDate.Year, lastDate.Month, 1).AddDays(-1);
							for (DateTime newDate = exmDate; newDate <= lastDate; newDate = newDate.AddDays(1))
							{

								if (newDate.DayOfWeek == weekNumber)
								{
									occurance++;
									Int32 mod = occurance % 10;
									 if (mod == 1)
											occuranceNo = occurance.ToString() +"st ";
										else if (mod == 2)
											occuranceNo = occurance.ToString() + "nd ";
										else if (mod == 3)
											occuranceNo = occurance.ToString() + "rd ";
										else if (mod > 3)
											occuranceNo = occurance.ToString() + "th ";
									exmDate = newDate;

									if (counter % 2 == 0)
										styleClass = "gdalternate1";
									else
										styleClass = "gdrow1";
									if (context.Exams.Any(s => s.ExaminationCycleID == CycleId && s.ExamYear == yearName && s.ExamStartDate == exmDate) == false)
									{
										rowNumber++;
										tablestring.Append("<tr class=" + styleClass + " ><td>" + (rowNumber).ToString() + "</td><td>" + occuranceNo.ToString() + weekNumber.ToString() + " " + monthName.GetMonthName(exmDate.Month).ToString() + ", " + year.ToString() + "</td><td>" + monthName.GetMonthName(exmDate.Month).ToString() + "</td><td>" + year.ToString() + "</td><td>" + exmDate.ToString("dd-MMM-yyyy") + "</td></tr>");
									}

								}
								counter++;
							}

						}
					}
					//tablestring = tablestring + "<tr><td>" + objExam.Name.ToString()+  "</td></tr>";

					tablestring.Append("</table>");
					divdetail.InnerHtml = tablestring.ToString();
					if (rowNumber == 0)
					{
						lblerror.Text = "Exam can not be created for the selected year. Reason: All possible exams have been already created.";
						lblerror.Visible = true;
						btnSave.Visible = false;
						btnCancel.Visible = btnSave.Visible = false;
					}
					else
					{
						btnSave.Visible = true;
						btnCancel.Visible = true;
					}
					//}
				};
			}
			else
			{
				btnCreate.Text = "Create Exams";
				ddlExamcycle.Enabled = true;
				txtYear.Enabled = true;
				ddlOccurance.Enabled = true;
				ddlWeek.Enabled = true;
				ddlExamcycle.SelectedValue = "0";

				ddlOccurance.Items.Clear();
				ddlOccurance.Items.Add(new ListItem("--Select One--", "0"));
				ddlOccurance.SelectedValue = "0";
				lbStartMonth.Text = "";
				lbSchedule.Text = "";
				txtYear.Text = "";
				trShowTbl.Visible = false;
				divdetail.InnerHtml = "";
				btnSave.Visible = false;
				btnCancel.Visible = false;
				lblerror.Text = "";
			}
		}
		catch (Exception ex)
		{
			lblerror.Text = ex.Message;
			lblerror.Visible = true;
		}

	}
	protected void ddlficoursecategory_SelectedIndexChanged(object sender, EventArgs e)
	{
		//ddlflcourse.Items.Clear();

		//FillFilterCourses();
	}
	protected void FillFilterCategories()
	{
		//try
		//{
		//    using (EConnectContext context = new EConnectContext())
		//    {
		//        ListItem lst = new ListItem("--All--", "0");
		//        var Category = from p in context.CourseCategories
		//                       orderby (p.Name)
		//                       select new { ValueField = p.ID, TextField = p.Name };

		//        EConnect.Utils.Common.ControlUtility.BindListObject(ddlficoursecategory, Category, lst);
		//    };
		//}
		//catch (Exception ex)
		//{
		//    throw ex;
		//}
	}
	protected void FillFilterCourses()
	{
		//try
		//{
		//    using (EConnectContext context = new EConnectContext())
		//    {
		//        ListItem lst = new ListItem("--All--", "0");

		//       // int id = Convert.ToInt32(ddlficoursecategory.SelectedValue);

		//        var CourseList = from p in context.Courses
		//                         where p.CourseCategoryID == id

		//                         select new { ValueField = p.ID, TextField = p.Name };
		//       // EConnect.Utils.Common.ControlUtility.BindListObject(ddlflcourse, CourseList, lst);
		//    };

		//}
		//catch (Exception ex)
		//{
		//    throw ex;
		//}
	}
	protected void FillExamCycles()
	{
		try
		{
			if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
			{
				if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
				{
					using (var context = new EConnectContext())
					{
						Int32 categoryId = Convert.ToInt32(Request.QueryString["CatID"]);
						Int32 cid = Convert.ToInt32(Request.QueryString["CourseId"]);
						ListItem lst = new ListItem("--Select One--", "0");
						var examCycle = from p in context.ExaminationCycles
										where (p.CourseID == cid && p.CourseCategoryID == categoryId)
										orderby (p.Name)
										select new { ValueField = p.ID, TextField = p.Name };
						EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamcycle, examCycle, lst);
					};
				}
			}
			else if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{
				using (var context = new EConnectContext())
				{
					Int32 cycleID = Convert.ToInt32(Request.QueryString["CycleID"]);
					ListItem lst = new ListItem("--Select One--", "0");
					var examCycle = from p in context.ExaminationCycles
									where (p.ID == cycleID)
									orderby (p.Name)
									select new { ValueField = p.ID, TextField = p.Name };
					EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamcycle, examCycle, lst);
				};

			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	protected void showsidelink()
	{
		DateTime ExamSDate = DateTime.MaxValue;
		using (EConnectContext context = new EConnectContext())
		{
			Exam objExm = context.Exams.Find(Convert.ToInt32(Request.QueryString["key"]));
			ExamSDate = objExm.ExamStartDate;
		}
		SideLink1.Items.Add(new SideLinkItem("Exam Time Table", "admexamdetail.aspx?ExamID=" + Request.QueryString["Key"] + "&CourseID=" + Request.QueryString["CourseId"] + "&ExamStartDate=" + txtExamDate.Text.ToString(), "_self"));
		SideLink1.Items.Add(new SideLinkItem("Cut-Off Dates", "CutoffDates.aspx?ExamID=" + Request.QueryString["Key"] + "&CourseID=" + Request.QueryString["CourseId"], "", "_self"));
		SideLink1.Items.Add(new SideLinkItem("Exam Center Allotment", "ExamCenterAlloted.aspx?ExamID=" + Request.QueryString["Key"] + "&CourseID=" + Request.QueryString["CourseId"], "", "_self"));
		//SideLink1.Items.Add(new SideLinkItem("Time Table Pattern", "TimeTablePattern.aspx?CourseID=" + Request.QueryString["CourseId"], "", "_self"));
		SideLink1.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
		SideLink1.Render();
	}
	protected void FillFilterExamCycles()
	{
		try
		{
			if (!String.IsNullOrEmpty(Request.QueryString["CatID"]))
			{
				if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
				{
					using (var context = new EConnectContext())
					{
						Int32 categoryId = Convert.ToInt32(Request.QueryString["CatID"]);
						Int32 cid = Convert.ToInt32(Request.QueryString["CourseId"]);
						ListItem lst = new ListItem("--All--", "0");
						var examCycle = (from p in context.ExaminationCycles
										 join c in context.Exams on p.ID equals c.ExaminationCycleID
										 where (p.CourseID == cid)
										 orderby (p.Name)
										 select new { ValueField = p.ID, TextField = p.Name }).Distinct();
						EConnect.Utils.Common.ControlUtility.BindListObject(ddlCycle, examCycle, lst);

						ListItem lst1 = new ListItem("--All--", "0");
						var examYear = (from p in context.Exams
										where (p.CourseID == cid)                                       
										select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
						examYear = examYear.OrderByDescending(s => s.ValueField).Take(6);
						EConnect.Utils.Common.ControlUtility.BindListObject(ddlFilterYear, examYear, lst1);
					};
				}
			}
			if (!String.IsNullOrEmpty(Request.QueryString["CycleID"]))
			{
				using (var context = new EConnectContext())
				{
					Int32 cycleID = Convert.ToInt32(Request.QueryString["CycleID"]);
					ListItem lst = new ListItem("--Select One--", "0");
					var examCycle = from p in context.ExaminationCycles
									where (p.ID == cycleID)
									orderby (p.Name)
									select new { ValueField = p.ID, TextField = p.Name };
					EConnect.Utils.Common.ControlUtility.BindListObject(ddlCycle, examCycle, lst);

					var examCycle1 = context.ExaminationCycles.Find(cycleID);
					ddlCycle.SelectedValue = examCycle1.ID.ToString();
					ddlCycle.Enabled = false;

					ListItem lst1 = new ListItem("--All--", "0");
					var examYear = (from p in context.Exams
									where p.ExaminationCycleID == cycleID                                    
									select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
					examYear = examYear.OrderByDescending(s => s.ValueField).Take(6);
					EConnect.Utils.Common.ControlUtility.BindListObject(ddlFilterYear, examYear, lst1);
				};
			}

		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	protected void FillResulGradeVersions(Int32 courseID)
	{
		try
		{
			using (EConnectContext context = new EConnectContext())
			{
				Course course = context.Courses.Find(courseID);
				var version = (from p in context.ResultGrades
								 where p.CourseCategoryID == course.CourseCategoryID
								 select new { ValueField = p.VersionID, TextField = p.VersionID }).Distinct().OrderByDescending(p => p.ValueField);
				EConnect.Utils.Common.ControlUtility.BindListObject(ddlversion, version,null);
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	protected void ddlWeek_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			using (EConnectContext context = new EConnectContext())
			{
				ExaminationCycle objExamCycle = new EConnect.NIELIT.ExaminationCycle();
				int CycleId = 0;
				if (ddlExamcycle.SelectedValue != "0")
					CycleId = Convert.ToInt32(ddlExamcycle.SelectedValue);
				objExamCycle = context.ExaminationCycles.Find(Convert.ToInt32(CycleId));
				DateTimeFormatInfo monthName = new DateTimeFormatInfo();
				DateTime exmDate = new DateTime();

				int occurance = 0;
				string occuranceNo = "";
				int year = Convert.ToInt32(txtYear.Text);
				int dayOccurance = Convert.ToInt32(ddlOccurance.SelectedValue);
				DayOfWeek weekNumber = (DayOfWeek)Convert.ToInt32(ddlWeek.SelectedValue);
				if (objExamCycle.enmExamSchedule == enmExamSchedule.HalfYearly)
				{
					for (int month = objExamCycle.StartingMonth; month < objExamCycle.StartingMonth + 12; month = month + 6)
					{
						occurance = 0;
						if (month > 12 && year == Convert.ToInt32(txtYear.Text))
							year = year + 1;
						exmDate = new DateTime(year, (month % 12) == 0 ? 12 : (month % 12), 1);
						for (DateTime newDate = exmDate; exmDate <= exmDate.AddMonths(1); newDate = newDate.AddDays(1))
						{
							if (newDate.DayOfWeek == weekNumber)
							{
								occurance++;
								if (occurance == dayOccurance)
								{
									exmDate = newDate;
									break;
								}
							}
						}
					}
				}
				txtExamDate.Text = exmDate.ToString("dd-MMM-yyyy");
			};
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message);
		}
	}

	protected void Popup(Int32 courseID)
	{
		try
		{
			tbl.CssClass = "sample3";
			tbl.CellPadding = 2;
			tbl.CellSpacing = 1;
			tbl.Width = Unit.Percentage(100);
			showTableheader();
			int i = 1;
			Int32 VersionID = 0;
			ImgBtnPopupFee.Enabled = true;
			ImgBtnPopupFee.ImageUrl = "~/images/popup1.jpg";
			ImgBtnPopupFee.ToolTip = "Click here to view Grade Legends.";

		   
			using (EConnectContext context = new EConnectContext())
			{
				if (ddlversion.SelectedValue == "")
					VersionID = 1;
				else
					VersionID = Convert.ToInt32(ddlversion.SelectedValue);
				Int32[] notInGrades = { 8, 10};
				Course course = context.Courses.Find(courseID);
				var grade = (from g in context.ResultGrades
							 where g.CourseCategoryID == course.CourseCategoryID && !notInGrades.Contains(g.ID) && g.VersionID == VersionID
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
					foreach (var result in grade)
					{
						TableRow tr = new TableRow();
						if (i % 2 == 0)
							tr.CssClass = "gdalternate1";
						else
							tr.CssClass = "gdrow1";

						TableCell tdRow = new TableCell();

						tdRow.Text = result.grade.ToUpper();
						tdRow.HorizontalAlign = HorizontalAlign.Center;
						tr.Cells.Add(tdRow);

						TableCell tdRow2 = new TableCell();

						if (result.legend1 != null && result.legend1 > 0 && result.legend2 != null && result.legend2 > 0)
							tdRow2.Text = result.legend1 + " to " + result.legend2;
						else
							tdRow2.Text = "-";
						tdRow2.HorizontalAlign = HorizontalAlign.Center;
						tr.Cells.Add(tdRow2);

						TableCell tdRow1 = new TableCell();

						tdRow1.Text = GetInitCap(result.description).ToString();
						if (result.description.Length >= 18)
						{
							tdRow1.Text = result.description.ToString().Substring(0, 18) + "...";
							tdRow1.ToolTip = result.description;
						}
						tdRow1.HorizontalAlign = HorizontalAlign.Left;
						tr.Cells.Add(tdRow1);


						tbl.Rows.Add(tr);
						i++;
					}
				}
			};
			divreport.Controls.Clear();
			divreport.Controls.Add(tbl);
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message);
		}
	}
	protected void showTableheader()
	{

		try
		{
			TableHeaderRow th = new TableHeaderRow();
			th.CssClass = "head1";

			TableHeaderCell tcCol = new TableHeaderCell();
			tcCol.Width = Unit.Percentage(18);
			tcCol.Text = "Grade";
			tcCol.HorizontalAlign = HorizontalAlign.Center;
			th.Cells.Add(tcCol);

			TableHeaderCell tcCol2 = new TableHeaderCell();
			tcCol2.Width = Unit.Percentage(44);
			tcCol2.Text = "Marks Range (in %)";
			tcCol2.HorizontalAlign = HorizontalAlign.Center;
			th.Cells.Add(tcCol2);

			TableHeaderCell tcCol1 = new TableHeaderCell();
			tcCol1.Width = Unit.Percentage(38);
			tcCol1.Text = "Remarks";
			tcCol1.HorizontalAlign = HorizontalAlign.Center;
			th.Cells.Add(tcCol1);



			tbl.Rows.Add(th);
		}
		catch (Exception ex)
		{
			ShowAlert(ex.Message);
		}

	}
	protected void ddlversion_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			BreadCrumb1.Render();
			Popup(Convert.ToInt32(Request.QueryString["CourseId"].ToString()));
		}
		 catch (Exception ex)
		{
			ShowAlert(ex.Message);
		}
	}

	[System.Web.Services.WebMethod(EnableSession = true)]
	public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
	{
		EConnectContext context = new EConnectContext();
		try
		{
			if (count <= 0)
				count = 10;
			Int32 courseID = 0;
			if (!String.IsNullOrEmpty(contextKey))
				courseID = Convert.ToInt32(contextKey);
			List<String> items = new List<String>();
			string searchString = prefixText.Trim().ToUpper();
			var examNames = (from s in context.Exams
							 select new { courseID = s.CourseID, Name = s.Name }).Distinct();
			if (courseID != 0)
				examNames = examNames.Where(c => c.courseID == courseID);
			if (!String.IsNullOrEmpty(searchString))
			{
				examNames = examNames.Where(s => s.Name.ToUpper().Contains(searchString));
			}
			examNames = examNames.OrderBy(s => s.Name).Take(count);
			foreach (var names in examNames)
			{
				items.Add(names.Name);
			}
			return items.ToArray();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally { context.Dispose(); }
	}
}