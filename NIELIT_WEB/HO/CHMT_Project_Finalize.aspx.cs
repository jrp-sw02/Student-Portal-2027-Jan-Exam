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
using EConnect.URM;

public partial class HO_CHMT_Project_Finalize : BasePage
{
    string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
       // lblError.Visible = false;
       // lblError.Text = "";
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
       }

      //  Int64 instituteId = Convert.ToInt64(ddlInstitute.SelectedValue);
       
    }

    protected void BindInstitute()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var instituteList = from p in context.CourseProjectApplications
                                    join i in context.Institutes on p.InstituteID equals i.ID
                                    where p.ApplicationStatusID == 10
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

   
  
    protected void BindGridView()
    {

        int instituteId = Convert.ToInt32(ddlInstitute.SelectedValue);
        using (EConnectContext context = new EConnectContext())
        {
            
            btnFinalize.Visible = true;

            Int32 statusID = 0;
            Int32 statusID1 = 0;
            Int32 statusID2 = 0;

      
            statusID = Convert.ToInt32(Request.QueryString["Status"]);

            enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID;
            var fillCourseProjectData = (from p in context.CourseProjectApplications
                                         join d in context.CourseProjectApplicationDetails on p.ID equals d.CourseExamApplicationID
                                         where p.InstituteID == instituteId && (p.ApplicationStatusID == 10) 
                                         //&&
                                         //context.CourseProjectApplicationDetails.Any(cpad =>
                                         //    cpad.CourseExamApplicationID == p.ID && cpad.ModuleID == 1159 && cpad.ResultGradeID == null)
                                         select new
                                         {
                                             ID = p.ID,
                                             Registration_Number = p.RegistrationNumber,
                                             Project_Title = d.ProjectTitle,
                                             Project_Receipt_Date = d.ProjectReceiptDate,
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
                                p.Project_Receipt_Date 
                                // Project_Receipt_Date = p.Project_Receipt_Date.ToString("yyyy-MM-dd")
                                //ProjectTitle = p.ProjectTitle 
                            });
            PagingBar1.Bind(fillCourseProjectData, ref gvGrid);

            //                          };
           
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvGrid.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        BindGridView();
    }

    protected void btnFinalize_Click(object sender, EventArgs e)
    {

        try
        {
            Int64 applID = 0;
            Int16 finalizedCount = 0;
            Int32 ApplicationStatusId = Convert.ToInt32(enmCourseExamApplicationStatus.CHMTProjectfinalizedbyNIELIT);
            using (EConnectContext context = new EConnectContext())
            {
                finalizedCount += 1;

                Int32 instituteID = Convert.ToInt32(ddlInstitute.SelectedValue);
                for (int i = 0; i < gvGrid.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvGrid.Rows[i].FindControl("chk");
                    TextBox txtF = (TextBox)gvGrid.Rows[i].FindControl("txtDate");

                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {

                            applID = Convert.ToInt64(gvGrid.DataKeys[i].Values[0]);
                            //Int64 registrationNumber =  Convert.ToInt64(gvGrid.DataKeys[i].Values["Registration_Number"]);
                            Int64 registrationNumber = Convert.ToInt64(gvGrid.Rows[i].Cells[1].Text);


                            SqlConnection sqlCon = new SqlConnection(conString);
                            SqlCommand cmd = new SqlCommand("CHMT_Project_Finalization", sqlCon);

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@InstituteId", instituteID);
                            cmd.Parameters.AddWithValue("@regNo", registrationNumber);

                            SqlParameter msgOut = new SqlParameter("@msgOut", SqlDbType.VarChar, 1000);
                            msgOut.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(msgOut);

                            sqlCon.Open();
                            cmd.ExecuteNonQuery();

                            string msg = cmd.Parameters["@msgOut"].Value as string;

                            if (!string.IsNullOrEmpty(msg))
                            {
                                lblErrorMsg.Visible = true;
                                lblErrorMsg.Text = msg;
                            }

                            else
                            {

                                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                                var moduleDetails = (from p in context.CourseProjectApplicationDetails
                                                     where p.CourseExamApplicationID == applID
                                                     select p).FirstOrDefault();

                                if (appl != null)
                                {

                                    //appl.DateOfProjectSubmissionOfPreviousLevel = dtFrom;                               
                                    // appl.IsVerifiedByInstitute = true;
                                    //appl.DateOfVerificationByInstitute = DateTime.Now;
                                    appl.ApplicationStatusID = ApplicationStatusId;
                                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                }

                                ShowAlert(finalizedCount.ToString() + " application(s)  finalized successfully.", true);
                                sqlCon.Close();

                            }
                        }
                    }
                }
                context.SaveChanges();
            };

            BindGridView();


        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            //sqlCon.Close();

        }
    }

   
}