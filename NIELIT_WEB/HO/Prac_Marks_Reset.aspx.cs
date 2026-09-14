using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data.Objects;
using EConnect;

public partial class HO_Prac_Marks_Reset : BasePage
{
    Int32 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    string Returnmessage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //loginUserNo = Convert.ToInt32(Session["UserID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            //lblError.Visible = false;
            //entityID = Convert.ToInt32(Session["EntityID"]);

            if (!IsPostBack)
            { 
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
    protected bool IsValidForm()
    {
        try
        {

            if (!isSelected(ddlMarksType))
            {
                lblError.Visible = true;
                lblError.Text = "Please Select Marks Type";
                return false;
            }
            if (!isBlank(txtRegNo))
            {
                lblError.Visible = true;
                lblError.Text = "Regn No. can not be left blank";
                return false;
            }
            if (!isNumber(txtRegNo))
            {
                lblError.Visible = true;
                lblError.Text = "Invalid  Regn Number";
                return false;
            }
            if (!isBlank(txtBatchNo))
            {
                lblError.Visible = true;
                lblError.Text = "Batch No. can not be left blank";
                return false;
            }
            //if (!isNumber(txtBatchNo))
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Invalid  Regn Number";
            //    return false;
            //}

            if (!isBlank(txtRemarks))
            {
                lblError.Visible = true;
                lblError.Text = "Remarks can not be left blank";
                return false;
            }
            return true;    
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                using (SqlConnection Conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_practical_marks_reset", Conn))
                    {
                        Conn.Open();
                  
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add(new SqlParameter("@reg_no", SqlDbType.BigInt));
                        cmd.Parameters["@reg_no"].Value = txtRegNo.Text;

                        cmd.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 3));
                        cmd.Parameters["@Batch"].Value = txtBatchNo.Text;

                        cmd.Parameters.Add(new SqlParameter("@type_of_marks_reset_required", SqlDbType.Int));
                        cmd.Parameters["@type_of_marks_reset_required"].Value = ddlMarksType.SelectedValue;

                        cmd.Parameters.Add(new SqlParameter("@input_remarks", SqlDbType.VarChar,255));
                        cmd.Parameters["@input_remarks"].Value = txtRemarks.Text;

                        cmd.Parameters.Add("@success_msg", SqlDbType.VarChar, 255);
                        cmd.Parameters["@success_msg"].Direction = ParameterDirection.Output;
                      

                        cmd.Parameters.Add("@success", SqlDbType.Int);
                        cmd.Parameters["@success"].Direction = ParameterDirection.Output;
                       
                        int data= cmd.ExecuteNonQuery();                       
                        Int32 Success = 0;
                        Success = (Int32)cmd.Parameters["@success"].Value;
                        Returnmessage = (string)cmd.Parameters["@success_msg"].Value;                       
                        if (data != 0)
                        {
                            if (Success == 1)
                            {
                                lblError.Visible = true;                             
                                lblError.Text = Returnmessage;
                            }
                        }
                        else 
                        {
                            if (Success == 0)
                            {
                                
                                lblError.Visible = true;
                                lblError.Text = Returnmessage;                                
                            }
                            
                        }
                       
                        Conn.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
}