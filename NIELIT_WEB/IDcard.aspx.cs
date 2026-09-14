using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;

public partial class IDcard : BasePage
{
    String strMessage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("Index.aspx");

            lblError.Text = "";
            mltvTab.ActiveViewIndex = 0;

            if (!Page.IsPostBack)
            {
                BindCourse();
            }
        }
        catch (Exception) { }
    }
    private void BindCourse()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Int64 RegnNo = Convert.ToInt64(Request.QueryString["RegnNo"]);
            Int64 CandidateID = Convert.ToInt64(Request.QueryString["CandidateID"]);
            ListItem lst = new ListItem("--Select One--", "0");
            var course = (from s in context.RegistrationDetails
                          join c in context.Courses on s.CourseID equals c.ID
                          where s.RegistrationNo == RegnNo && s.RegistrationStatusID < 4
                          orderby (c.ID)
                          select new { ValueField = s.CourseID, TextField = c.Name }).ToList();
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseLevel, course, lst);
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        showdata();
    }
    protected void showdata()
    {
        Int64 RegnNo = Convert.ToInt64(Request.QueryString["RegnNo"]);
        Int64 CandidateID = Convert.ToInt64(Request.QueryString["CandidateID"]);
        lblError.Visible = false;
        lblError.Text = "";
        Int32 LevelId = Convert.ToInt32(ddlCourseLevel.SelectedValue);
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(CS);
        try
        {

            //string sqlphoto = "SELECT top 1 Uploaded_File as CPhoto  FROM Uploaded_File WHERE ID = (Select top 1 Photo_uploaded_file_Id  FROM Regn_I_Card_Candidate_Photos WHERE Candidate_Id ='" + CandidateID + "') Update Regn_I_Card_Candidate_Photos set Last_Download_Date =GETDATE (), I_Card_Download_Count =(isnull(a.I_Card_Download_Count,0) +1)from Regn_I_Card_Candidate_Photos a where a.Candidate_Id ='" + CandidateID + "' ";
            //string sqlsign = "SELECT top 1 Uploaded_File as CSign FROM Uploaded_File WHERE ID = (Select top 1 Sign_uploaded_file_Id  FROM Regn_I_Card_Candidate_Photos WHERE Candidate_Id ='" + CandidateID + "')";


            //November_2024
            string sqlphoto = "SELECT top 1 Uploaded_File as CPhoto  FROM Uploaded_File WHERE ID = (Select top 1 Photo_uploaded_file_Id  FROM Regn_I_Card_Candidate_Photos WHERE Candidate_Id =@candidateID ) Update Regn_I_Card_Candidate_Photos set Last_Download_Date =GETDATE (), I_Card_Download_Count =(isnull(a.I_Card_Download_Count,0) +1)from Regn_I_Card_Candidate_Photos a where a.Candidate_Id =@candidateID ";
            string sqlsign = "SELECT top 1 Uploaded_File as CSign FROM Uploaded_File WHERE ID = (Select top 1 Sign_uploaded_file_Id  FROM Regn_I_Card_Candidate_Photos WHERE Candidate_Id =@candidateID )";

            SqlCommand cmd = new SqlCommand(sqlphoto, con);
            //November_2024
            cmd.Parameters.AddWithValue("@candidateID", CandidateID);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            con.Open();
            SqlCommand cmd1 = new SqlCommand(sqlsign, con);
            //November_2024
            cmd1.Parameters.AddWithValue("@candidateID", CandidateID);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            con.Close();

            using (EConnectContext context = new EConnectContext())
            {
                var cr = (from s in context.RegistrationDetails
                          where s.RegistrationNo == RegnNo && s.CourseID == LevelId
                          orderby s.RegistrationDate descending
                          select s).FirstOrDefault();
                if (cr != null)
                {
                    var add = cr.Candidate.Addresses.Where(s => s.AddressTypeID == 1).OrderByDescending(s => s.EffectiveDateFrom).FirstOrDefault();
                    string corDistrict = add.DistrictID.HasValue && add.DistrictID != 0 ? GetInitCap(add.District.Name) : "";
                    string corState = add.StateID.HasValue && add.StateID != 0 ? GetInitCap(add.State.Name) : "";
                    string corAddress = GetInitCap(add.AddressLine1 + " " + add.AddressLine2 + " " + add.AddressLine3 + " " + add.CityName + " " + corDistrict + " " + corState + " " + add.PinCode);

                    mltvTab.ActiveViewIndex = 1;
                    LblRegnNumber.Text = cr.RegistrationNo.ToString();
                    LblAppName.Text = cr.Candidate.Salutation + " " + GetInitCap(cr.Candidate.Name);
                    LblDob.Text = cr.Candidate.DateOfBirth.ToString("dd/MMM/yyyy").Replace('-', '/');
                    LblCourse.Text = GetInitCap(cr.Course.Code);
                    RegnValidity.Text = cr.CommencementFromDate.ToString("MMM/yyyy").Replace('-', '/') + '-' + cr.ValidUptoDate.ToString("MMM/yyyy").Replace('-', '/');
                    if (string.IsNullOrEmpty(cr.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(cr.Candidate.GuardianName) == true)
                    {
                        TrFatherName.Visible = true;
                        TrMotherName.Visible = true;
                        TrGardianName.Visible = false;
                        if (string.IsNullOrEmpty(cr.Candidate.FatherName) == false && !string.IsNullOrWhiteSpace(cr.Candidate.FatherName))
                            LblFName.Text = "Mr. " + GetInitCap(cr.Candidate.FatherName);
                        else
                            LblFName.Text = "NA";
                        if (string.IsNullOrEmpty(cr.Candidate.MotherName) == false && string.IsNullOrWhiteSpace(cr.Candidate.MotherName) == false)
                            LblMName.Text = "Mrs. " + GetInitCap(cr.Candidate.MotherName);
                        else
                            LblMName.Text = "NA";
                    }
                    else
                    {
                        TrFatherName.Visible = false;
                        TrMotherName.Visible = false;
                        TrGardianName.Visible = true;
                        LblGuardianName.Text = string.IsNullOrEmpty(cr.Candidate.GuardianName) == false && string.IsNullOrWhiteSpace(cr.Candidate.GuardianName) == false ? GetInitCap(cr.Candidate.GuardianName) : "NA";
                    }
                    LblAddress.Text = corAddress;

                    ImgApplicantPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dt.Rows[0]["CPhoto"]);
                    imgSignature.ImageUrl = "data:image/jpgg;base64," + Convert.ToBase64String((byte[])dt1.Rows[0]["CSign"]);

                    //if (cr.CourseRegistrationApplication.Photo != null)
                    //{
                    //    ImgApplicantPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.CourseRegistrationApplication.Photo);
                    //    imgSignature.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.CourseRegistrationApplication.Signature);
                    //}
                    //else
                    //{
                    //    lblError.Visible = true;
                    //    lblError.Text = "Your Photo is not available for print of ID Card";
                    //}
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Your have not a valid registration status for " + ddlCourseLevel.SelectedItem.ToString() + ". Contact NIELIT Head Office";
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
            //Response.Redirect("Error.aspx");
        }
        finally
        {
            con.Close();
        }

    }
}