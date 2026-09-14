using DocumentFormat.OpenXml.Spreadsheet;
using EConnect;
using EConnect.DAL;
using EConnect.URM;
using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_MISRegistrationLink : BasePage
{
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    UserType loginUserType;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");

            entityID = Convert.ToInt64(Session["EntityID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
    

            //if (!checkUserHO_LKN() || !UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //   Response.End();
            //}


            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!IsPostBack)
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
                // Initialization code here 
                FillInstitutes();
                
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool checkUserHO_LKN()
    {
        bool isHO = false;
        bool isN_LKN = false;

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
        using (var command = new SqlCommand("checkuserlogin", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@entity_id", entityID);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                // First result set → IsHO
                if (reader.Read())
                {
                    isHO = Convert.ToBoolean(reader[0]);
                }

                // Move to next result set → IsN_LKN
                if (reader.NextResult() && reader.Read())
                {
                    isN_LKN = Convert.ToBoolean(reader[0]);
                }
            }
        }

        return (isHO || isN_LKN);
    }
    protected void FillInstitutes()
    {
        try
        {
            int courseid = Convert.ToInt32(Request.QueryString["id"]);
            ListItem lst1 = new ListItem("-- Select One --", "0");

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("fillUPAccCentres", connection))
                {
                    // check  nulls beofre passing
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@course_id", SqlDbType.Int).Value = courseid;

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {

                        da.Fill(ds);
                    }

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitute, ds.Tables[0], lst1);

                    ddlinstitute.DataTextField = "DisplayName";
                    ddlinstitute.DataValueField = "ID";


                    ddlinstitute.DataBind();
                    ddlinstitute.Items.Insert(0, new ListItem("-- Select One --", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool whetherexistsincra()
    {
        try
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select COUNT(*) from Course_Registration_Application where Course_Id = 1 and Apaar_ID= @apaarid", conn);

                string enc_apaar = EncryptDecrypt.EncryptString(txtapaar.Text);
                cmd.Parameters.AddWithValue("@apaarid", enc_apaar);
                int exist = (int)cmd.ExecuteScalar();

                return exist > 0;

            }
        }
        catch (Exception ex)
        {
            throw ex; 
        }
    }

    /*Backend Validations Start */
    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isValidDob(TextBox txtBox)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtBox.Text);

            int result1 = DateTime.Compare(todaydate, Inputdate);
            int result2 = DateTime.Compare(todaydate.AddYears(-10), Inputdate);

            if (result2 == -1)
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isValidForm()
    {
        if (!isSelected(ddlinstitute))
        {
            lblerror.Visible = true;
            lblerror.Text = " Please select School";
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtapaar.Text))
        {
            lblerror.Visible = true;
            lblerror.Text = lblapaar.Text + " cannot be left blank";
            return false;
        }

        // Check if APAAR ID is exactly 12 digits
        if (!System.Text.RegularExpressions.Regex.IsMatch(txtapaar.Text, @"^\d{12}$"))
        {
            lblerror.Visible = true;
            lblerror.Text = lblapaar.Text + " must be exactly 12 digits.";
            return false;
        }

        if (!string.IsNullOrWhiteSpace(txtdob.Text))
        {
            if (!isValidDob(txtdob))
            {
                lblerror.Visible = true;
                lblerror.Text = "Enter Valid DOB";
            }
        }
        

        return true;
    }

    /* Backend Validations End */
    protected void btn_status(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            if (!isValidForm())
            {
                return;
            }
			

                          BindGridView();
                          
                                 btnStatus.Visible = true;
            
        }
        catch(Exception ex)
        {
            ShowAlert("Error : " + ex.Message);
        }
    }
    private void BindGridView()
    {
        try
        {
            // put validation
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getStudentDataforcorrection", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    String accnum = ddlinstitute.SelectedValue;
                    String udisecode = accnum.Split('-')[1];

                    // check null before passing
                    command.Parameters.AddWithValue("@ApaarID", txtapaar.Text);
                    command.Parameters.AddWithValue("@udisecode", udisecode);
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                    gvMain.Visible = true;
                    gvMain.DataSource = dt;
                    gvMain.DataBind();
                            lblerror.Visible = false;
                    //uPnlGrid.Update();
                    //lblMessage.Visible = dt.Rows.Count == 0 ? true : false;
                    lblMessage.Text = "No record found.";

                    if (dt.Rows.Count > 0)
                    {
                        lblMessage.Visible = false;
                        update_table.Visible = true;

                    }
                    else if (dt.Rows.Count == 0)
                    {
                        lblMessage.Visible = true;
                        update_table.Visible = false;

                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "An error occurred: " + ex.Message;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        update_table.Visible = false;
        lblMessage.Visible = false;
        ddlinstitute.SelectedIndex = 0;
        txtapaar.Text = "";
        txtClass.Text = "";
        txtfathername.Text = "";
        txtrollnum.Text = "";
        txtname.Text = "";
        lblsuccess.Visible = false;
        lblMessage.Visible = false;
        lblerror.Visible = false;
        gvMain.Visible = false;
        txtdob.Text = "";
        btnStatus.Visible = true;
        
    }
    
    
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (!isValidForm())
            {
                return;
            }


            if (whetherexistsincra())
            {
                ShowAlert("Registration for this candidate is already completed. No further updates are allowed");
                return;
            }

            int cnt = 0;
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select COUNT(*) from Course_Registration_Log where Apaar_ID = @apaarid", conn);
                cmd.Parameters.AddWithValue("@apaarid", txtapaar.Text);

                cnt = (int)cmd.ExecuteScalar();
            }

            if (cnt >= 2)
            {
                ShowAlert("You have reached the maximum limit of 2 updates.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtdob.Text) &&
                        string.IsNullOrWhiteSpace(txtname.Text) &&
                        string.IsNullOrWhiteSpace(txtClass.Text) &&
                        string.IsNullOrWhiteSpace(txtrollnum.Text) &&
                        string.IsNullOrWhiteSpace(txtfathername.Text))
            {
                lblsuccess.Visible = true;
                lblsuccess.Text = "No Data Updated , All fields are Empty .";
                lblsuccess.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection sqlCon = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand("upmspCorrectionUpdate", sqlCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@apaarid",
                    String.IsNullOrWhiteSpace(txtapaar.Text) ? "-99" : txtapaar.Text);

                cmd.Parameters.AddWithValue("@class",
                    String.IsNullOrWhiteSpace(txtClass.Text) ? "-99" : txtClass.Text);

                cmd.Parameters.AddWithValue("@name",
                    String.IsNullOrWhiteSpace(txtname.Text) ? "-99" : txtname.Text);

                cmd.Parameters.AddWithValue("@rollnumber",
                    String.IsNullOrWhiteSpace(txtrollnum.Text) ? "-99" : txtrollnum.Text);

                cmd.Parameters.AddWithValue("@fathername",
                    String.IsNullOrWhiteSpace(txtfathername.Text) ? "-99" : txtfathername.Text);

                cmd.Parameters.AddWithValue("@dob",
                    String.IsNullOrWhiteSpace(txtdob.Text)
                        ? Convert.ToDateTime("1900-01-01")
                        : Convert.ToDateTime(txtdob.Text));

                cmd.Parameters.AddWithValue("@updatedby", entityID);

                sqlCon.Open();
                cmd.ExecuteNonQuery();
                
                
            }
            lblsuccess.Visible = true;
                    lblerror.Visible = false;
            lblsuccess.Text = "Data Updated Successfully";
            lblsuccess.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            ShowAlert("Error: " + ex.Message);
        }
    }
}