using DocumentFormat.OpenXml.Bibliography;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_PhysicalProgressReportFilter: System.Web.UI.Page
{

     UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 NielitCentreId = 0;
    Int32 NielitCentreIdFilter = 0;
    Int32 UserTypeId = 0;
    string centre = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        lblerror.Text = "";
        try
        {

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            //UserTypeId = 10;
            //currentRoleId = 21;
            //loginUserNo = 285716;
            //loginUserType = 10;

            if (!IsPostBack)
            {
                bindCentre();
              }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }

           
    }
    
    public void bindCentre()
    {
        ddlCentreName.Items.Clear();

        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                User objUser;
                using (EConnectContext context1 = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo && s.UserTypeID == 10).FirstOrDefault();
                    if (loginUser != null)
                        NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    if (loginUser == null)
                        NielitCentreId = 99;
                }


                ListItem lst = new ListItem("--All--", "99");
                string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //centre = "Select * from NielitCentres where id= '" + NielitCentreId + "'";
                    centre = "Select * from NielitCentres where id= @pNielitCentreId";
                    if (NielitCentreId == 99)
                        centre = "Select * from NielitCentres where 99= @pNielitCentreId";


                    SqlCommand cmd = new SqlCommand();

                    SqlParameter param = new SqlParameter();
                    param = new SqlParameter("@pNielitCentreId", NielitCentreId);
                    //param[1] = new SqlParameter("@pwd", txtPwd.Text);
                    cmd.CommandText = centre;
                    cmd.Parameters.Add(param);
                    cmd.Connection = con;

                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);

             

                    ddlCentreName.Items.Add("--All--");
                    ddlCentreName.Items[0].Value = "99";
                    ddlCentreName.AppendDataBoundItems = true;
                    ddlCentreName.DataSource = dt;
                    //ddlCentreName.DataBind();
                    ddlCentreName.DataTextField = "Name";
                    ddlCentreName.DataValueField = "ID";
                    ddlCentreName.DataBind();

                    if (NielitCentreId == 99)
                    {
                        ddlCentreName.Enabled = true;

                    }
                    else
                        ddlCentreName.Enabled = false;

                }
                     
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCentreName.SelectedValue = "0";
            
        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message, true);
        }
    }

      
    }

