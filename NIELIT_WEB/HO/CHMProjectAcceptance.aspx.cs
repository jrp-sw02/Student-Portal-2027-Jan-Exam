using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Globalization;
using EConnect.URM;

public partial class HO_CHMProjectAcceptance : BasePage
{

    string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //lblError.Visible = false;
        //lblError.Text = "";
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
        if (!UserManager.HasRight(currentRoleId, enmRight.View))
        {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
        }
        else
        {
            if (!IsPostBack)
            {
                BindInstitute();
            }

            Int64 instituteId = Convert.ToInt64(ddlInstitute.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                var reportVisibility = (from p in context.CourseProjectApplications
                                        where p.InstituteID == instituteId && p.ApplicationStatusID == 10
                                        select p).Count();

                if (reportVisibility >= 0)
                {
                    btnReport.Visible = true;
                   
                }
                else
                {
                    btnReport.Visible = false;
                }
            }
       }
    }

    protected void BindInstitute()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var instituteList = from p in context.CourseProjectApplications
                                    join  i in  context.Institutes on  p.InstituteID equals i.ID
                                    where  p.ApplicationStatusID == 8 
                                 orderby (i.Name)
                                 select new { ValueField = i.ID, TextField = i.Name };
                instituteList = instituteList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlInstitute, instituteList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable GetCandidates()
    {

        string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection sqlCon = new SqlConnection(conString);
        try
        {
            int instituteId = Convert.ToInt32(ddlInstitute.SelectedValue);
           
            DataTable dt = new DataTable();


            using (SqlCommand cmd = new SqlCommand("CHMT_Project_Candidates_Institutewise", sqlCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@instituteID", instituteId);
              
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            DataColumn srNoColumn = new DataColumn("SrNo", typeof(int));
            dt.Columns.Add(srNoColumn);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["SrNo"] = i + 1;
            }

            return dt;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            return null;
        }
    }
   
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvGrid.PageIndex = PagingBar1.CurrentPageIndex;
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
            PagingBar1.CurrentPageIndex = 0;
            gvGrid.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void BindGridView()
    {

           int instituteId = Convert.ToInt32(ddlInstitute.SelectedValue);
           using (EConnectContext context = new EConnectContext())
            {

                btnVerify.Visible = true;
                btnFinalize.Visible = false;

                Int32 statusID = 0;
                Int32 statusID1 = 0;
                Int32 statusID2 = 0;

             
                    statusID = Convert.ToInt32(Request.QueryString["Status"]);

                enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID;
                var fillCourseProjectData = (from p in context.CourseProjectApplications
                                             join  d in  context.CourseProjectApplicationDetails on  p.ID  equals d.CourseExamApplicationID
                                             where p.InstituteID == instituteId && (p.ApplicationStatusID == 8 ) &&
                                             context.CourseProjectApplicationDetails.Any(cpad =>
                                                 cpad.CourseExamApplicationID == p.ID && cpad.ModuleID == 1159 && cpad.ResultGradeID == null)
                                             select new
                                             {
                                                 ID = p.ID,
                                                 Registration_Number = p.RegistrationNumber,
                                                 Project_Title =  d.ProjectTitle,
                                                 Candidate_Name = (from candidateDetail in context.RegistrationDetails
                                                                   join candidate in context.Candidates on candidateDetail.CandidateID equals candidate.ID
                                                                   where candidateDetail.RegistrationNo == p.RegistrationNumber
                                                                   select candidate.Name).FirstOrDefault(),
                                                                   ApplicationStatusID = p.ApplicationStatusID
                                             }).ToList() // Execute the query to retrieve the results
                                .Select((p, index) => new
                                {
                                    SrNo = index + 1, // Add sequential number
                                    p.ID,
                                    p.Registration_Number,
                                    p.Candidate_Name, 
                                    p.ApplicationStatusID,
                                    p.Project_Title,
                                    //ProjectReceiptDate = p.ProjectReceiptDate, // Assuming this field exists
                                    //ProjectTitle = p.ProjectTitle 
                                });
               PagingBar1.Bind(fillCourseProjectData, ref gvGrid);     
               
                        //                          };
               if (applStatus ==  enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)
               {
                   gvGrid.Columns[7].Visible = false;
                   gvGrid.Columns[6].Visible = false;
                   btnFinalize.Visible = true;
                   //btnReject.Visible = true;
               }
         }
    }
    
    protected void btnShow_Click(object sender, EventArgs e)
    {
        BindGridView();       
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.Render();
            Int16 verifiedCount = 0;         
            Int32 Applstatusid1 = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);

               
            #region Project Exam---------------
           
                //commented on  05-04-2024
                if (gvGrid.Rows.Count > 0)
                {
                    for (int i = 0; i < gvGrid.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvGrid.Rows[i].FindControl("chk");
                        TextBox txtF = (TextBox)gvGrid.Rows[i].FindControl("txtDate");
                        

                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                
                                 if (txtF.Text == "")
                                {
                                    ShowAlert("Please enter Project Submission Date  ", true);
                                    txtF.Focus();
                                    return;
                                }                                                                                                                         
                            }
                        }
                    }
                }
                Int64 applID = 0;
                Int32 ApplicationStatusId = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);

                using (EConnectContext context = new EConnectContext())
                {
                    //Int32 paidStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    Int32 instituteID = Convert.ToInt32(ddlInstitute.SelectedValue);
                    for (int i = 0; i < gvGrid.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvGrid.Rows[i].FindControl("chk");
                        TextBox txtF = (TextBox)gvGrid.Rows[i].FindControl("txtDate");
                       
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                verifiedCount += 1;
                               // string registrationNumber = gvGrid.DataKeys[i]["Registration_Number"].ToString();
                                //Int64  registrationNumber =Convert.ToInt64(gvGrid.Rows[i].Cells[2].Text);
                               // applID = Convert.ToInt64(gvGrid.DataKeys[i].Values[0]);
                                //CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                  applID = Convert.ToInt64(gvGrid.DataKeys[i].Values[0]);
                                
                                SqlConnection sqlCon = new SqlConnection(conString);
                                SqlCommand cmd = new SqlCommand("CHMT_Project_Finalization", sqlCon);

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@instituteID", instituteID);
                                cmd.Parameters.AddWithValue("@appId", applID);


                                //SqlParameter outP = new SqlParameter("@result", SqlDbType.NVarChar, 500);
                                //outP.Direction = ParameterDirection.Output;
                                //cmd.Parameters.Add(outP);

                                sqlCon.Open();
                                cmd.ExecuteNonQuery();
                                sqlCon.Close();


                                DateTime dtFrom = Convert.ToDateTime(txtF.Text);
                               

                                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                                var moduleDetails = (from p in context.CourseProjectApplicationDetails
                                                     where p.CourseExamApplicationID == applID
                                                     select p).FirstOrDefault();
                               // System.Web.UI.WebControls.TextBox txtProjectTitle = (System.Web.UI.WebControls.TextBox)gv.FindControl("txtProjectTitle");
                                TextBox txtProjectTitle = (TextBox)gvGrid.Rows[i].FindControl("txtProjectTitle");
                               txtProjectTitle.Text = moduleDetails.ProjectTitle;
                               moduleDetails.ProjectReceiptDate = dtFrom;
                               // var appl = context.CourseProjectApplications.Where(a => a.RegistrationNumber == registrationNumber && a.InstituteID == instituteID).FirstOrDefault();
                               // CourseProjectApplication appl = context.CourseProjectApplications.Find(registrationNumber);
                                if (appl != null)
                                {
                                    //appl.CourseDurationFrom = drF.SelectedItem.Text + "-" + txtF.Text;
                                    //appl.CourseDurationTo = drT.SelectedItem.Text + "-" + txtT.Text;
                                    //appl.DateOfProjectSubmissionOfPreviousLevel = dtFrom;                               
                                    appl.IsVerifiedByInstitute = true;
                                    appl.DateOfVerificationByInstitute = DateTime.Now;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }


                                //foreach (GridViewRow gv in gvGrid.Rows)
                                //{
                                //    int rowIndex = gv.RowIndex;
                                //    System.Web.UI.WebControls.TextBox txtProjectTitle = (System.Web.UI.WebControls.TextBox)gv.FindControl("txtProjectTitle");
                                    
                                //  //CourseProjectApplicationDetail module = new CourseProjectApplicationDetail();
                                // txtProjectTitle.Text =    moduleDetails.ProjectTitle ;
                                //  //  context.Entry(moduleDetails).State = System.Data.Entity.EntityState.Modified;
                                //    ///context.CourseProjectApplicationDetails.Add(moduleDetails);                                    
                                //}
                            }
                        }
                    }
                    context.SaveChanges();
                };

                ShowAlert(verifiedCount.ToString() + " application(s) verified and finalized successfully.", true);


                //if (applStatus == enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)
                //{
                //    fillCourseExamApplication = fillCourseExamApplication.Where(a => a.PaymentStatusID == sts);
                //}


               
                BindGridView();
            }
            #endregion
       // }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {           
            Response.Redirect("~/HO/CHMT_Project_Report.aspx");
        }
            
        catch (Exception ex)
        {
        }
    }
}