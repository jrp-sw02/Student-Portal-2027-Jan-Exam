using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_DemandNoteCancellation : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int64 demandnoteNo = 0;
    Int64 Amount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Cancellation", "HO/DemandNoteCancellation.aspx", ""));
                BreadCrumb1.Render();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            demandnoteNo = Convert.ToInt64(txtdemandnoteno.Text);
            DateTime createdDate = DateTime.Now;
            Int32 createdByID = Convert.ToInt32(Session["UserID"]);
            //Int32 CourseExamApplStatus = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
            //Int32 CourseRegApplStatus = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
            //Int32 CertificateExamApplStatus = Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
            var demand = context.DemandNotes.Where(s => s.ID == demandnoteNo).FirstOrDefault();
            Int32 OnlineSuccesscount = (from r in context.OnlineTransaction
                                        where r.DemandNoteID == demand.ID && r.ResponseStatusCode == "0300"
                                        select r).Count();
            Int32 CSCSuccessCount = (from r in context.CSCTransactions
                                     where r.DemandNoteID == demand.ID && (r.ResponseStatus == 0 || r.ResponseStatus == 100)
                                     select r).Count();
            if (demand != null)
            {
                if (demand.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                {
                    if (demand.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        Int32 demandnotcount = (from r in context.CourseExamApplications
                                                where r.DemandNoteID == demand.ID
                                                select r).Count();

                        //Int32 Applicationstatuscount = (from r in context.CourseExamApplications
                        //                                where r.ApplicationStatusID == CourseExamApplStatus && r.DemandNoteID == demand.ID
                        //                                select r).Count();

                        //if (demandnotcount != 0 && Applicationstatuscount != 0 && demandnotcount == Applicationstatuscount && OnlineSuccesscount == 0 && CSCSuccessCount == 0)
                        if (demandnotcount != 0 && OnlineSuccesscount == 0 && CSCSuccessCount == 0)
                        {
                            if (context.CourseExamApplications.Any(s => s.DemandNoteID == demand.ID))
                            {
                                //creating history for demand note cancelled
                                //context.Database.ExecuteSqlCommand("insert into Demand_Note_Cancel_History (Date,Fee_Type_ID,Payment_Mode_ID,Demand_Note_Type_ID,Amount,Application_Type_ID,Online_Transaction_ID,CSC_Transaction_ID,DD_Transaction_ID,Status_ID,Created_By,NEFT_Transaction_ID,Cancel_Reason,History_Created_By,History_Created_On,Demand_Note_ID,Request_Person_Email_ID)(select s.Date,s.Fee_Type_ID,s.Payment_Mode_ID,s.Demand_Note_Type_ID,s.Amount,s.Application_Type_ID,s.Online_Transaction_ID,s.CSC_Transaction_ID,s.DD_Transaction_ID,s.Status_ID,s.Created_By,s.NEFT_Transaction_ID, '" + txtreason.Text + "'," + createdByID + " , '" + createdDate + "', s.ID, '" + txtemailid.Text.Trim() + "' from Demand_Note s where s.ID = " + demand.ID + ")");

                                //December_2024
                                SqlParameter[] param1 = { new SqlParameter("@reason", txtreason.Text),
                                                            new SqlParameter("@createdByID", createdByID),
                                                              new SqlParameter("@createdDate",  createdDate),
                                                                new SqlParameter("@emailId",  txtemailid.Text.Trim() ),
                                                                    new SqlParameter("@demandNoteID",  demand.ID)
                                                                            };
                                context.Database.ExecuteSqlCommand("insert into Demand_Note_Cancel_History (Date,Fee_Type_ID,Payment_Mode_ID,Demand_Note_Type_ID,Amount,Application_Type_ID,Online_Transaction_ID,CSC_Transaction_ID,DD_Transaction_ID,Status_ID,Created_By,NEFT_Transaction_ID,Cancel_Reason,History_Created_By,History_Created_On,Demand_Note_ID,Request_Person_Email_ID)(select s.Date,s.Fee_Type_ID,s.Payment_Mode_ID,s.Demand_Note_Type_ID,s.Amount,s.Application_Type_ID,s.Online_Transaction_ID,s.CSC_Transaction_ID,s.DD_Transaction_ID,s.Status_ID,s.Created_By,s.NEFT_Transaction_ID, @reason,@createdByID , @createdDate, s.ID, @emailId from Demand_Note s where s.ID = @demandNoteID)", param1);
                                context.SaveChanges();

                                //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute) + " , Demand_Note_ID = null Where  Demand_Note_ID= " + demand.ID);

                                //December_2024
                                SqlParameter[] param2 = { new SqlParameter("@applicationStatusID",  Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)),
                                                                    new SqlParameter("@demandNoteID",  demand.ID)
                                                                            };
                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatusID , Demand_Note_ID = null Where  Demand_Note_ID= @demandNoteID ",param2);
                                context.SaveChanges();
                                strMessage = "DemandNote with DemandNoteID:- " + demandnoteNo + " cancelled successfully.";
                            }
                            else
                            {
                                strMessage = "No record found for this DemandNoteID :- " + demandnoteNo;
                            }
                        }
                        else
                        {
                            strMessage = " DemandNote with DemandNoteID :- " + demandnoteNo + " cannot be cancelled because records does not match / Already Cancelled.";
                        }
                    }
                    else if (demand.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        Int32 demandnotcount = (from r in context.CourseRegistrationApplications
                                                where r.DemandNoteID == demand.ID
                                                select r).Count();

                        //Int32 Applicationstatuscount = (from r in context.CourseRegistrationApplications
                        //                                where r.ApplicationStatusID == CourseRegApplStatus && r.DemandNoteID == demand.ID
                        //                                select r).Count();
                        //if (demandnotcount != 0 && Applicationstatuscount != 0 && demandnotcount == Applicationstatuscount && OnlineSuccesscount == 0 && CSCSuccessCount == 0)
                        if (demandnotcount != 0 && OnlineSuccesscount == 0 && CSCSuccessCount == 0)
                        {
                            if (context.CourseRegistrationApplications.Any(s => s.DemandNoteID == demand.ID))
                            {
                                //creating history for demand note cancelled
                                //context.Database.ExecuteSqlCommand("insert into Demand_Note_Cancel_History (Date,Fee_Type_ID,Payment_Mode_ID,Demand_Note_Type_ID,Amount,Application_Type_ID,Online_Transaction_ID,CSC_Transaction_ID,DD_Transaction_ID,Status_ID,Created_By,NEFT_Transaction_ID,Cancel_Reason,History_Created_By,History_Created_On,Demand_Note_ID,Request_Person_Email_ID)(select s.Date,s.Fee_Type_ID,s.Payment_Mode_ID,s.Demand_Note_Type_ID,s.Amount,s.Application_Type_ID,s.Online_Transaction_ID,s.CSC_Transaction_ID,s.DD_Transaction_ID,s.Status_ID,s.Created_By,s.NEFT_Transaction_ID, '" + txtreason.Text + "'," + createdByID + " , '" + createdDate + "', s.ID , '" + txtemailid.Text.Trim() + "' from Demand_Note s where s.ID = " + demand.ID + ")");

                                //December_2024
                                SqlParameter[] param3 = { new SqlParameter("@reason", txtreason.Text),
                                                            new SqlParameter("@createdByID", createdByID),
                                                              new SqlParameter("@createdDate",  createdDate),
                                                                new SqlParameter("@emailId",  txtemailid.Text.Trim()),
                                                                    new SqlParameter("@demandNoteID",  demand.ID)
                                                                            };
                                context.Database.ExecuteSqlCommand("insert into Demand_Note_Cancel_History (Date,Fee_Type_ID,Payment_Mode_ID,Demand_Note_Type_ID,Amount,Application_Type_ID,Online_Transaction_ID,CSC_Transaction_ID,DD_Transaction_ID,Status_ID,Created_By,NEFT_Transaction_ID,Cancel_Reason,History_Created_By,History_Created_On,Demand_Note_ID,Request_Person_Email_ID)(select s.Date,s.Fee_Type_ID,s.Payment_Mode_ID,s.Demand_Note_Type_ID,s.Amount,s.Application_Type_ID,s.Online_Transaction_ID,s.CSC_Transaction_ID,s.DD_Transaction_ID,s.Status_ID,s.Created_By,s.NEFT_Transaction_ID, @reason,@createdByID, @createdDate, s.ID , @emailId from Demand_Note s where s.ID = @demandNoteID)", param3);
                                context.SaveChanges();

                                //context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByInstitute) + " , Demand_Note_ID = null Where  Demand_Note_ID= " + demand.ID);

                                //December_2024
                                SqlParameter[] param4 = { new SqlParameter("@applicationStatusID",  Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByInstitute)),
                                                                    new SqlParameter("@demandNoteID",  demand.ID)
                                                                            };
                                context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatusID , Demand_Note_ID = null Where  Demand_Note_ID=@demandNoteID", param4);
                                context.SaveChanges();
                                strMessage = "DemandNote with DemandNoteID:- " + demandnoteNo + " cancelled successfully.";
                            }
                            else
                            {
                                strMessage = "No record found for this DemandNoteID :- " + demandnoteNo;
                            }
                        }
                        else
                        {
                            strMessage = " DemandNote with DemandNoteID :- " + demandnoteNo + " cannot be cancelled because records does not match / Already Cancelled.";
                        }
                    }
                    else if (demand.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        Int32 demandnotcount = (from r in context.CertificateExamApplications
                                                where r.DemandNoteID == demand.ID
                                                select r).Count();

                        //Int32 Applicationstatuscount = (from r in context.CertificateExamApplications
                        //                                where r.ApplicationStatusID == CertificateExamApplStatus && r.DemandNoteID == demand.ID
                        //                                select r).Count();

                        //if (demandnotcount != 0 && Applicationstatuscount != 0 && demandnotcount == Applicationstatuscount && OnlineSuccesscount == 0 && CSCSuccessCount == 0)
                        if (demandnotcount != 0 && OnlineSuccesscount == 0 && CSCSuccessCount == 0)
                        {
                            if (context.CertificateExamApplications.Any(s => s.DemandNoteID == demand.ID))
                            {
                                //creating history for demand note cancelled
                                //context.Database.ExecuteSqlCommand("insert into Demand_Note_Cancel_History (Date,Fee_Type_ID,Payment_Mode_ID,Demand_Note_Type_ID,Amount,Application_Type_ID,Online_Transaction_ID,CSC_Transaction_ID,DD_Transaction_ID,Status_ID,Created_By,NEFT_Transaction_ID,Cancel_Reason,History_Created_By,History_Created_On,Demand_Note_ID,Request_Person_Email_ID)(select s.Date,s.Fee_Type_ID,s.Payment_Mode_ID,s.Demand_Note_Type_ID,s.Amount,s.Application_Type_ID,s.Online_Transaction_ID,s.CSC_Transaction_ID,s.DD_Transaction_ID,s.Status_ID,s.Created_By,s.NEFT_Transaction_ID, '" + txtreason.Text + "'," + createdByID + " , '" + createdDate + "', s.ID,'" + txtemailid.Text.Trim() + "' from Demand_Note s where s.ID = " + demand.ID + ")");

                                //December_2024
                                SqlParameter[] param5 = { new SqlParameter("@reason", txtreason.Text),
                                                            new SqlParameter("@createdByID", createdByID),
                                                              new SqlParameter("@createdDate",  createdDate),
                                                                new SqlParameter("@emailId",  txtemailid.Text.Trim()),
                                                                    new SqlParameter("@demandNoteID",  demand.ID )
                                                                            };
                                context.Database.ExecuteSqlCommand("insert into Demand_Note_Cancel_History (Date,Fee_Type_ID,Payment_Mode_ID,Demand_Note_Type_ID,Amount,Application_Type_ID,Online_Transaction_ID,CSC_Transaction_ID,DD_Transaction_ID,Status_ID,Created_By,NEFT_Transaction_ID,Cancel_Reason,History_Created_By,History_Created_On,Demand_Note_ID,Request_Person_Email_ID)(select s.Date,s.Fee_Type_ID,s.Payment_Mode_ID,s.Demand_Note_Type_ID,s.Amount,s.Application_Type_ID,s.Online_Transaction_ID,s.CSC_Transaction_ID,s.DD_Transaction_ID,s.Status_ID,s.Created_By,s.NEFT_Transaction_ID, @reason,@createdByID, @createdDate, s.ID,@emailId from Demand_Note s where s.ID = @demandNoteID)", param5);
                                context.SaveChanges();

                                //context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute) + " , Demand_Note_ID = null Where  Demand_Note_ID= " + demand.ID);

                                //December_2024
                                SqlParameter[] param6 = { new SqlParameter("@applicationStatusID",  Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute)),
                                                                    new SqlParameter("@demandNoteID",  demand.ID)
                                                                            };
                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatusID , Demand_Note_ID = null Where  Demand_Note_ID= @demandNoteID ", param6);
                                context.SaveChanges();
                                strMessage = "DemandNote with DemandNoteID:- " + demandnoteNo + " cancelled successfully.";
                            }
                            else
                            {
                                strMessage = "No record found for this DemandNoteID :- " + demandnoteNo;
                            }
                        }
                        else
                        {
                            strMessage = " DemandNote with DemandNoteID :- " + demandnoteNo + " cannot be cancelled because records does not match / Already Cancelled.";
                        }
                    }
                }
                else
                {
                    strMessage = "Demand note with DemandNoteID:- " + demandnoteNo + " cannot be cancelled because it has already being paid.";
                }
            }
            else
            {
                strMessage = " No DemandNote record found. ";
            }

            divdetails.Visible = false;
            txtdemandnoteno.Text = "";
            txtamount.Text = "";
            txtreason.Text = "";
            txtemailid.Text = "";
            throw new Exception(strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        divdetails.Visible = false;
        Response.Redirect("DemandNoteCancellation.aspx");
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            demandnoteNo = Convert.ToInt64(txtdemandnoteno.Text);
            Amount = Convert.ToInt64(txtamount.Text);
            Int32 pending = Convert.ToInt32(enmPaymentStatus.Pending);
            Int32 DemandType = Convert.ToInt32(enmDemandNoteType.Multiple);
            using (EConnectContext context = new EConnectContext())
            {
                if (context.DemandNotes.Any(s => s.ID == demandnoteNo && s.Amount == Amount && s.PaymentStatusID == pending))
                {
                    divdetails.Visible = true;
                    var DemandNote = (from p in context.DemandNotes
                                      where p.ID == demandnoteNo
                                      select new
                                      {
                                          DemandNo = p.ID,
                                          DemandDate = p.ApplicationDate,
                                          PaymentMode = p.PaymentMode.Name,
                                          PaymentStatus = p.PaymentStatus.Name,
                                          Amount = p.Amount,
                                          FeeType = p.FeeTypeID,
                                          ApplicationTypeID = p.ApplicationTypeID,
                                          DemandNoteTypeID = p.DemandNoteTypeID,
                                          createdBy = p.CreatedBy
                                      }).FirstOrDefault();
                    lblDemandNoteNo.Text = DemandNote.DemandNo.ToString();
                    lblDemandNoteDate.Text = DemandNote.DemandDate.ToString("dd-MMM-yyyy");
                    lblPaymentMode.Text = DemandNote.PaymentMode.ToString();
                    lblPaymentStatus.Text = DemandNote.PaymentStatus.ToString();
                    lblAmount.Text = DemandNote.Amount.ToString();
                    enmFeeType feetype = (enmFeeType)DemandNote.FeeType;
                    lblFeeType.Text = EConnect.Utils.Common.EnumUtility.GetDescription(feetype);
                    lblApplType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)DemandNote.ApplicationTypeID);
                    lblDemandNoteType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmDemandNoteType)DemandNote.DemandNoteTypeID);
                    if (DemandNote.createdBy.HasValue)
                        lblcreatedby.Text = context.Users.Where(s => s.UserID == DemandNote.createdBy).FirstOrDefault().UserName;
                    else
                        lblcreatedby.Text = "NA";
                }
                else
                {
                    divdetails.Visible = false;
                    throw new Exception("No Record Found / Demand Note Already Paid.");
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
}