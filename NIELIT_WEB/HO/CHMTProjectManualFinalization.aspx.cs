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

public partial class HO_CHMTProjectManualFinalization : BasePage
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
        //currentRoleId = Convert.ToInt32(Session["RoleID"]);
        //loginUserNo = Convert.ToInt32(Session["UserID"]);
        //if (!UserManager.HasRight(currentRoleId, enmRight.View))
        //{
        //    Response.Write("Sorry! You don't have rights  to view this page");
        //    Response.End();
        //}
        //else
        //{
        if (!IsPostBack)
        {
          //  BindInstitute();
        }
        //  }

        //  Int64 instituteId = Convert.ToInt64(ddlInstitute.SelectedValue);

    }

   
    protected void BindGridView()
    {

       // int instituteId = Convert.ToInt32(ddlInstitute.SelectedValue);
        using (EConnectContext context = new EConnectContext())
        {

            btnFinalize.Visible = true;

            Int32 statusID = 0;
            Int32 statusID1 = 0;
            Int32 statusID2 = 0;

            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime toDate = Convert.ToDateTime(txtToDate.Text);
            statusID = Convert.ToInt32(Request.QueryString["Status"]);

            enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)statusID;
            var fillCourseProjectData = (from p in context.CHMTProjectManualEntry
                                         join r in context.RegistrationDetails on  p.RegistrationNo equals  r.RegistrationNo
                                        // join d in  context.CourseProjectApplicationDetails on  r.CandidateID equals d.CandidateID  
                                        // join a  in  context.CourseProjectApplications  on  d.CourseExamApplicationID equals a.ID
                                         where
                                         //r.RegistrationNo == d.RegistrationNumber && p.RegistrationNo == d.RegistrationNumber && r.CourseID == d.CourseID 
                                         //&&  a.ApplicationStatusID ==  10
                                       //  && 
                                         r.CourseID ==  1213 && 
                                         p.EntryDate >= fromDate && p.EntryDate <= toDate 
                                         &&  p.WhetherFinalized  == null 
                                         && p.FinalizedDate  ==  null 
                                                                             
                                         select new
                                         {
                                             ID = p.ID,
                                             Registration_Number = p.RegistrationNo,
                                            // Project_Title = d.ProjectTitle,
                                             Project_Receipt_Date = p.ProjectReceiptDate  ,
                                             Institute_Name = r.Institute.Name,
                                             Candidate_Name = p.Name,
                                            
                                         }).ToList() // Execute the query to retrieve the results
                            .Select((p, index) => new
                            {
                                SrNo = index + 1, // Add sequential number
                                p.ID,
                                p.Registration_Number,
                                p.Candidate_Name,
                                p.Institute_Name,
                              //  p.Project_Title,
                                Project_Receipt_Date = p.Project_Receipt_Date.ToString("yyyy-MM-dd")
                                //ProjectReceiptDate = p.ProjectReceiptDate, // Assuming this field exists
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
        Int64 applID = 0;
        Int16 finalizedCount = 0;
        Int32 ApplicationStatusId = Convert.ToInt32(enmCourseExamApplicationStatus.CHMTProjectfinalizedbyNIELIT);
        using (EConnectContext context = new EConnectContext())
        {
            finalizedCount += 1;
 

           // Int32 instituteID = Convert.ToInt32(ddlInstitute.SelectedValue);
            for (int i = 0; i < gvGrid.Rows.Count; i++)
            {
                CheckBox cbx = (CheckBox)gvGrid.Rows[i].FindControl("chk");
                TextBox txtF = (TextBox)gvGrid.Rows[i].FindControl("txtDate");

                if (cbx != null)
                {
                    if (cbx.Checked)
                    {

                        applID = Convert.ToInt64(gvGrid.DataKeys[i].Values[0]);
                        Int64 registrationNumber = Convert.ToInt64(gvGrid.Rows[i].Cells[1].Text);

                       // RegistrationDetail rd = context.RegistrationDetails.Find(registrationNumber);

                        var rd = (from r in context.RegistrationDetails
                                  where r.RegistrationNo == registrationNumber
                                  select r).FirstOrDefault();

                        Int64? instituteID = rd.InstituteID;

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
                           ShowAlert(finalizedCount.ToString() + " application(s)  finalized successfully.", true);
                        }

                        sqlCon.Close();

                        CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                      
                        if (appl != null)
                        {

                            //appl.DateOfProjectSubmissionOfPreviousLevel = dtFrom;                               
                            // appl.IsVerifiedByInstitute = true;
                            //appl.DateOfVerificationByInstitute = DateTime.Now;
                            appl.ApplicationStatusID = ApplicationStatusId;
                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                        }

                    }
                }
            }

          //  ShowAlert(finalizedCount.ToString() + " application(s)  finalized successfully.", true);

            context.SaveChanges();
        };

       

    }
}