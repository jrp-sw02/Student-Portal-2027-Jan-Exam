using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.URM;

public partial class DigitalIndiaStudent : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;  
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            lblerror.Visible = false;

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
            if (!IsPostBack)
            {
                bindState();
                InstituteDetail(entityID);
                ddlCardType_SelectedIndexChanged(ddlCardType.SelectedValue, EventArgs.Empty);
                //disableAll();  


            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }

    }
    protected bool isBlankNumber(TextBox txtBox)
    {
        try
        {
            int zero = 0;

            if (txtBox.Text.Trim() == "" || txtBox.Text.Trim() == zero.ToString() || txtBox.Text.Trim() == ".")
            {
                txtBox.Text = "";
                txtBox.Focus();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
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
    public bool IsValidForm()
    {
        try
        {
            if (!isBlank(AccrInstitute))
            {
                lblerror.Visible = true;
                lblerror.Text = "Accredited Institute can not be blank";
                return false;
            }

            if (RdoAppliedAs.SelectedValue == "REG")
            {
                //if (RdoApplicantType.SelectedValue == null)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "Please Select Applicant Type";
                //    return false;
                //}
                if (!isBlank(RegnRollNumber))
                {
                    if (RdoApplicantType.SelectedValue == "OABCMAT")
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Enter your Registration Number";
                        return false;
                    }
                    if (RdoApplicantType.SelectedValue == "CCCBCC")
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Enter your Roll Number";
                        return false;
                    }
                }
            }



            if (!isBlank(txtAppName))
            {
                lblerror.Visible = true;
                lblerror.Text = "Applicant Name can not be left blank";
                return false;
            }

            if (!Char.IsLetter(txtAppName.Text, 0))
            {
                throw new Exception("Applicant Name should start with an alphabet.");
            }

            if (!Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1))
            {
                throw new Exception("Applicant Name should end with an alphabet.");
            }

            if (!isBlank(txtDob))
            {
                lblerror.Visible = true;
                lblerror.Text = "Date Of Birth can not be left blank";
                return false;
            }

            if (!isBlank(txtCorMobileNo))
            {
                lblerror.Visible = true;
                lblerror.Text = "Mobile Number can not be left blank";
                return false;
            }

            if (!isNumber(txtCorMobileNo))
            {
                lblerror.Visible = true;
                lblerror.Text = "Invalid Mobile Number";
                return false;
            }
            if (!isBlank(txtEmailId))
            {
                lblerror.Visible = true;
                lblerror.Text = "Email Id can not be left blank";
                return false;
            }

            if (!isBlank(TxtPerAddressLine1))
            {
                lblerror.Visible = true;
                lblerror.Text = "Address Line1 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine2))
            {
                lblerror.Visible = true;
                lblerror.Text = "Address Line2 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerCity))
            {
                lblerror.Visible = true;
                lblerror.Text = "City can not be left blank";
                return false;
            }

            if (!isSelected(ddlPState))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select State";
                return false;
            }
            if (!isSelected(ddlPdistrict))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select District";
                return false;
            }
            if (!isBlank(TxtPpincode))
            {
                lblerror.Visible = true;
                lblerror.Text = "Pin Code can not be left blank";
                return false;
            }
            if (!isNumber(TxtPpincode))
            {
                lblerror.Visible = true;
                lblerror.Text = "Invalid Pin Code Number";
                return false;
            }

            if (!isBlank(Qualificationtxt))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter Highest Education";
                return false;
            }

            if (!(ddlCardType.SelectedIndex == 0))
            {
                if (!isBlank(CardNumbertxt))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter Card Number";
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                SaveData();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void SaveData()
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
        try
        {
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Insert_DigitalIndiaCandidate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //Passing values...            
            cmd.Parameters.Add("@pApplicantName", SqlDbType.VarChar).Value = txtAppName.Text;
            cmd.Parameters.Add("@pApplicantDOB", SqlDbType.DateTime).Value = txtDob.Text;
            cmd.Parameters.Add("@pApplicantMobile", SqlDbType.BigInt).Value = txtCorMobileNo.Text;
            cmd.Parameters.Add("@pApplicantEmail", SqlDbType.VarChar).Value = txtEmailId.Text;
            cmd.Parameters.Add("@pAddress1", SqlDbType.VarChar).Value = TxtPerAddressLine1.Text;
            cmd.Parameters.Add("@pAddress2", SqlDbType.VarChar).Value = TxtPerAddressLine2.Text;
            cmd.Parameters.Add("@pAddress3", SqlDbType.VarChar).Value = TxtPerAddressLine3.Text;
            cmd.Parameters.Add("@pCity", SqlDbType.VarChar).Value = TxtPerCity.Text;
            cmd.Parameters.Add("@pState", SqlDbType.VarChar).Value = ddlPState.SelectedItem.Text;
            cmd.Parameters.Add("@pDistrict", SqlDbType.VarChar).Value = ddlPdistrict.SelectedItem.Text;
            cmd.Parameters.Add("@pPincode", SqlDbType.BigInt).Value = TxtPpincode.Text;
            cmd.Parameters.Add("@pQualification", SqlDbType.VarChar).Value = Qualificationtxt.Text;
            cmd.Parameters.Add("@pIDcardType", SqlDbType.Char).Value = ddlCardType.SelectedValue;
            cmd.Parameters.Add("@pIDcardnumber", SqlDbType.VarChar).Value = CardNumbertxt.Text;
            cmd.Parameters.Add("@pAppliedAs", SqlDbType.VarChar).Value = RdoAppliedAs.SelectedValue;
            cmd.Parameters.Add("@pApplicantType", SqlDbType.VarChar).Value = RdoApplicantType.SelectedValue;
            cmd.Parameters.Add("@pRegnRollNumber", SqlDbType.VarChar).Value = RegnRollNumber.Text;
            cmd.Parameters.Add("@pInstituteID", SqlDbType.BigInt).Value = lblinstcode.Text;
            cmd.Parameters.Add("@pAccrInstName", SqlDbType.VarChar).Value = AccrInstitute.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("WelcomeDigitalIndia.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
            if (con.State ==ConnectionState .Open )
                con.Close();
        }
    }
    public void InstituteDetail(Int64 instituteid)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var Aboutinstitute = context.Institutes.Find(instituteid);
                AccrInstitute.Text = Aboutinstitute.Name;
                lblinstcode.Text = Convert.ToString(Aboutinstitute.ID);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void bindState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var state = (from s in context.Locations
                             orderby (s.Name)
                             where s.LocationTypeID == 2 && s.ParentLocationID == 1
                             select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPState, state, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            //ddl.Items.Clear();
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district = from s in context.Locations
                               orderby (s.Name)
                               where s.LocationTypeID == locationTypeID && s.ParentLocationID == stateID
                               select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);
                if (ddl.Items.Count == 0)
                    ddl.Items.Add(lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RdoAppliedAs_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RdoAppliedAs.SelectedValue == "REG")
            {
                ApplicantTypeTr.Visible = true;
                RegnRollTr.Visible = true;
                RdoApplicantType.SelectedIndex = 0;
                //btnValidate.Visible = true;
                //btnSave.Visible = false;
                RdoApplicantType_SelectedIndexChanged(RdoApplicantType.SelectedValue, EventArgs.Empty);
            }
            else
            {
                ApplicantTypeTr.Visible = false;
                RegnRollTr.Visible = false;
                //btnValidate.Visible = false;
                //btnSave.Visible = true;
                RdoApplicantType.SelectedValue = null;
                RegnRollNumber.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void RdoApplicantType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RdoApplicantType.SelectedValue == "CCCBCC")
            {
                LblRegnRoll.Text = "Roll Number<font color='RED'>*</font>";
            }
            else
            {
                LblRegnRoll.Text = "Registration Number<font color='RED'>*</font>";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void ddlPState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id2 = Convert.ToInt32(ddlPState.SelectedValue);
            bindDistrict(id2, ref ddlPdistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void ddlCardType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCardType.SelectedValue != "0")
            CardNumbertxt.Enabled = true;
        else
            CardNumbertxt.Enabled = false;
    }
    //protected void btnValidate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (RdoAppliedAs.SelectedValue == "REG")
    //        {
    //            if (RdoApplicantType.SelectedValue == "CCCBCC")
    //            {
    //                //Pass on Roll-Number, Name, DOB


    //            }
    //            else
    //            {
    //                //Pass on Registration-Number, Name, DOB

    //            }
    //            btnSave.Visible = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}