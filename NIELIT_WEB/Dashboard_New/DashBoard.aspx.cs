using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

public partial class DashBoard1_HomePage : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
       getCounts();
       // lbl1.Text = "Training Partners";       
       // lbl1.Font.Size = new FontUnit(13);
       // lbl1.ForeColor = Color.White;
       //// lbl1.Controls.Add("deep");
       // lbl2.Text = "Registered Candidates";    
       // lbl2.Font.Size = new FontUnit(13);
       // lbl2.ForeColor = Color.White;
       // lbl3.Text = "Training Completed";      
       // lbl3.Font.Size = new FontUnit(13);
       // lbl3.ForeColor = Color.White;
       // lbl4.Text = "Certified Students"; 
       // lbl4.Font.Size = new FontUnit(13);
       // lbl4.ForeColor = Color.White;
       // //lbnc.Text = "Nielit Centers";
       // //lbnc.Font.Size = new FontUnit(13);
       // //lbnc.ForeColor = Color.White;
       // lbscst.Text = "SC/ST Candidates";
       // lbscst.Font.Size = new FontUnit(13);
       // lbscst.ForeColor = Color.White;
       // if (!IsPostBack)
       // {
          //  GetTrainingParteners(null);
            
     //   }
       
    }
    private void getCounts()
    {
        // Count of NIELIT Centres
        parameters  vPara=new parameters  ();
        DataSet ds=new DataSet ();
        ds = utility.executeProcedure("getNIELITCentresCount", vPara);
        if (ds.Tables[0].Rows.Count > 0)
            lblNielitPresenceCount.Text = ds.Tables[0].Rows[0][0].ToString();

        //Count of Training Partners
        ds = utility.executeProcedure("getTrainingPartnersCount", vPara);
        if (ds.Tables[0].Rows.Count > 0)
            lblTrainingCount.Text = ds.Tables[0].Rows[0][0].ToString();

        //Count of IECT candidates
        ds = utility.executeProcedure("getCandidatesITSkill", vPara);
        if (ds.Tables[0].Rows.Count > 0)
            lblITSkillingCount.Text = ds.Tables[0].Rows[0][0].ToString();

        //Count of NSQF candidates
        vPara.count = 1;
        vPara.pID = 0;
        ds = utility.executeProcedure("getCandidatesNSQF", vPara);
        if (ds.Tables[0].Rows.Count > 0)
            lblNSQFCoursesCount.Text = ds.Tables[0].Rows[0][0].ToString();

        //ds = utility.executeProcedure("Nielit_Reg_CandList_all", vPara);
        //if (ds.Tables[0].Rows.Count > 0)
        //    Label2.Text = ds.Tables[0].Rows[0][0].ToString();



        //Nielit_Reg_CandList_all
        //Count of DigiLocker Certificates
        //vPara.count = 1;
        //vPara.pGrand = 1;
        //vPara.pID = null;
        //ds = utility.executeProcedure("getDigiLockerCounts", vPara);
        //if (ds.Tables[0].Rows.Count > 0)
        //    lblDigiLockerCount.Text = ds.Tables[0].Rows[0][0].ToString();
        //else
        //    lblDigiLockerCount.Text = "0";

        ////Count of placements
        //vPara.count = 1;
        //vPara.pGrand = 1;
        //vPara.pID = null;
        //ds = utility.executeProcedure("getPlacementCounts", vPara);
        //if (ds.Tables[0].Rows.Count > 0)
        //    lblPlacementsCount.Text = ds.Tables[0].Rows[0][0].ToString();
        //else
        //    lblPlacementsCount.Text = "0";
        //lblPlacementsCount.Text = lblPlacementsCount.Text + "<br/>";

        //Count of future Skills
        //vPara.count = 0;
        //vPara.pGrand = null;
        //vPara.pID = null;
        //ds = utility.executeProcedure("getNIELITFutureSkillsCount", vPara);
        //if (ds.Tables[0].Rows.Count > 0)
        //    lblEmergingTechCount.Text = ds.Tables[0].Rows[0][0].ToString();
        //else
        //    lblEmergingTechCount.Text = "0";
    }

    //private void GetData1(string SortExpression)
    //{
    //    string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    //    using (SqlConnection con = new SqlConnection(CS))
    //    {
    //        SqlCommand cmd = new SqlCommand("Select COUNT( Name) course from NielitCentreCourse", con);
    //        con.Open();
    //        //SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        //DataSet ds = new DataSet();
    //        //da.Fill(ds);           
    //       int result = Convert.ToInt32(cmd.ExecuteScalar());
    //     // GetData objApplicant = new GetData( string sql);
            
    //    }
    //}
    private void GetTrainingParteners(string SortExpression)
    {
        //string sql = "select  COUNT (distinct nb.InstituteID)Center from [NIELIT].[dbo].[NielitCentreBatch] nb,[NIELIT].[dbo].[Institute] inst," +
        //                "[NIELIT].[dbo].[Location] st where nb.InstituteID=inst.ID  and st.ID=inst.State_ID and st.Parent_ID=1";
        string sql = "select  COUNT (distinct inst.ID)Center from [NIELIT].[dbo].[Institute] inst,[NIELIT].[dbo].[Location] st where  st.ID=inst.State_ID and st.Parent_ID=1 and  sinst.ID in ( Select Institute_ID from " +
                       " [NIELIT].[dbo].[Intitute_Accreditation_Detail] WHERE Accreditation_Status_ID IN(1,2,3,4)) ";
       int a = GetRecord(sql);
       
       //lblTrPtn.Font.Size = new FontUnit(18);      
       //lblTrPtn.Text = a.ToString();
       //lblRegCan.Font.Size = new FontUnit(18);
       //lblRegCan.Text = "854252";
       //lblTrCom.Font.Size = new FontUnit(18);
       //lblTrCom.Text = "25470";
       //lblCerStu.Font.Size = new FontUnit(18);
       //lblCerStu.Text = "4125";
       string sqlnc = "select distinct COUNT(*) from [NIELIT].[dbo].[NielitCentres] nc,[NIELIT].[dbo].[Location] st " +
                       " where st.ID=nc.State_ID and st.Parent_ID=1and nc.instituteid in (select centreid from NIELIT.dbo.centrewisecourses)";
       int nc = GetRecord(sqlnc);
       lblNielitPresenceCount.Text = nc.ToString();
       lblNielitPresenceCount.Visible = true;
       //lbncData.Font.Size = new FontUnit(18);
       //lbncData.Text = nc.ToString();
       string sqlscstcr = "select COUNT(*) FROM [NIELIT].[dbo].[Course_Registration_Application]  " +
       " where Cast_Category_ID in (2,3) and Final_Submitted=1";
       int scstcr = GetRecord(sqlscstcr);
       string sqlscstce = "select COUNT(*) FROM [NIELIT].[dbo].[Certificate_Exam_Application]  " +
       " where Cast_Category_ID in (2,3) and Final_Submitted=1";
       int scstce = GetRecord(sqlscstce);
       int totalSCST = scstcr + scstce;
       //lbscstdata.Font.Size = new FontUnit(18);
       //lbscstdata.Text = totalSCST.ToString();
    }

    public int GetRecord(string sqlquery)
    {
        int result;
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(CS))
        {
            using (SqlCommand cmd = new SqlCommand(sqlquery, con))
            {
                cmd.CommandType = CommandType.Text;
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        return result;
    }

    protected void btntrainingparteners_Click(object sender, System.EventArgs e)
    {

        Response.Redirect("NielitCentreInst.aspx");
    }
    protected void Button1_Click(object sender, System.EventArgs e)
    {
        Response.Redirect("RegisteredCandList.aspx");
    }
    protected void Button2_Click(object sender, System.EventArgs e)
    {
        Response.Redirect("RegisteredCandList.aspx");
    }
    protected void Button3_Click(object sender, System.EventArgs e)
    {
        Response.Redirect("RegisteredCandList.aspx");
    }
    protected void btnnielitregionalcenter_Click(object sender, System.EventArgs e)
    {

        Response.Redirect("NielitRegionalCentre.aspx");
    }

    protected void btnNielitPresence_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentres.aspx");
    }
    protected void btnTraining_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrainingPartners.aspx");
    }
    protected void btnITSkilling_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegisteredCandList.aspx");
    }
    protected void btnlblNSQFCourses_Click(object sender, EventArgs e)
    {
        Response.Redirect("NSQFCounts.aspx");
    }
    protected void btnlblPlacements_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlacementDetails.aspx");

    }
    protected void btnDigiLocker_Click(object sender, EventArgs e)
    {
        Response.Redirect("DigiLocker.aspx");
    }
    protected void btnlblEmergingTech_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmergingTrends.aspx");
    }
    protected void btnNorthEast_Click(object sender, EventArgs e)
    {

    }
    //protected void btnCandListYear_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("NielitCandListYearWise.aspx");
    //}
    protected void btnCandListYear_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegisteredCandList1.aspx");
    }
}