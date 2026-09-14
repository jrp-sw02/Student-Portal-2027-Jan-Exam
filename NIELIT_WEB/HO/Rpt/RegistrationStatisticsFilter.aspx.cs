using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class HO_Registration_StatisticsFilter : BasePage
{
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Statistics", "HO/Rpt/RegistrationStatisticsFilter.aspx", ""));
            HfApplicantType.Value = "Dir";
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!IsPostBack)
            {
                bindCourse();
                BindRegstatus();
                //BindRegstype();
                ChkRegStatusAll.Checked = true;
                if (ChkRegStatusAll.Checked == true)
                HfRegStatusId.Value = "0";
                ChkRegStatusAll_CheckedChanged(CheckBoxRegStatusList, EventArgs.Empty);
                // Reg-Type
                //chkRegTypeAll.Checked = true;
                //if (chkRegTypeAll.Checked == true)
                //    HfRegTypeId.Value = "0";
                //chkRegTypeAll_CheckedChanged(CheckBoxRegTypeList, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    public void bindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                Int32 coursetypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--ALL--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == coursetypeID
                              select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )"};

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses.Distinct(), lst);
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlDisplayCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HfApplicantType.Value = "Dir";
            LblSubCriteria.Visible = true;
            ddlSubCriteria.Visible = true;
            string str = ddlDisplayCriteria.SelectedValue;
            switch (str)
            {
                case "0":
                    LblSubCriteria.Visible = false;
                    ddlSubCriteria.Visible = false;  
                    TrIns1.Visible = false;
                    TrIns2.Visible = false;
                    break;
                case "G":                    
                    LblSubCriteria.Text = "Gender";
                    BindGender();
                     TrIns1.Visible = false;
                    TrIns2.Visible = false;
                    break;
                case "CC":                   
                    LblSubCriteria.Text = "Cast Category";
                    BindCastCategory();
                     TrIns1.Visible = false;
                    TrIns2.Visible = false;
                    break;
                case "S":
                    LblSubCriteria.Text = "State";
                    BindState();
                     TrIns1.Visible = false;
                    TrIns2.Visible = false;
                    break;
                case "AT":
                    LblSubCriteria.Text = "Application Type";
                    BindApplicantType();
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        
        }
    }
    public void BindGender()
    {
        try
        {
            ddlSubCriteria.Items.Clear();       
            ddlSubCriteria.Items.Insert(0, "--ALL--");
            ddlSubCriteria.Items[0].Value = "0";
            ddlSubCriteria.Items.Insert(1,"Male");
            ddlSubCriteria.Items[1].Value = "Male";
            ddlSubCriteria.Items.Insert(2,"Female");
            ddlSubCriteria.Items[2].Value = "Female";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void BindCastCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--ALL--", "0");
                var category = from p in context.CastCategories
                               orderby p.DisplayOrder
                               select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubCriteria, category, lst);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void BindRegstatus()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
               
                var Status = from p in context.RegistrationStatus
                               orderby p.ID
                             select new { ValueField = p.ID, TextField = p.Name + "&nbsp;&nbsp;" };
                CheckBoxRegStatusList.DataSource = Status.ToList();
                CheckBoxRegStatusList.DataTextField = "TextField";
                CheckBoxRegStatusList.DataValueField = "ValueField";
                CheckBoxRegStatusList.DataBind(); 
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }  
    public void BindState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--ALL--", "0");
                var state = from s in context.Locations
                            orderby (s.Name)
                            where s.LocationTypeID == 2
                            select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubCriteria, state, lst);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void BindApplicantType()
    {
        try
        {
         
            ListItem lst1 = new ListItem("--ALL--", "0");
            EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlSubCriteria, typeof(enmApplicantType), lst1);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try 
        {
            ddlCourseName.SelectedValue = "0";
            ddlDisplayCriteria.SelectedValue = "0";
            LblSubCriteria.Visible = false;
            ddlSubCriteria.Visible = false;
            TxtInstituteName.Text = "";
            TrIns1.Visible = false;
            TrIns2.Visible = false;
            ChkRegStatusAll.Checked = true;
            ChkRegStatusAll_CheckedChanged(CheckBoxRegStatusList, EventArgs.Empty);
            //chkRegTypeAll.Checked = true;
            //chkRegTypeAll_CheckedChanged(CheckBoxRegTypeList, EventArgs.Empty);
            txtDateFrom.Text = "";
            txtToDate.Text = "";
        
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }   
    protected void ChkRegStatusAll_CheckedChanged(object sender, EventArgs e)
    {
        try 
        {

            for (int i = 0; i < CheckBoxRegStatusList.Items.Count; i++)
            {
                if (ChkRegStatusAll.Checked == true)
                {
                    HfRegStatusId.Value = "0";
                    CheckBoxRegStatusList.Items[i].Selected = true;
                }
                else
                {
                    CheckBoxRegStatusList.Items[i].Selected = false;
                    HfRegStatusId.Value = "";
                }
            }
        
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void CheckBoxRegStatusList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string str ="";
            int AllCheckCount = 0;
            int AllUncheckCount = 0;
            Boolean IsAllCheck = false;
            for (int i = 0; i < CheckBoxRegStatusList.Items.Count; i++)
            {
                if (CheckBoxRegStatusList.Items[i].Selected == true)
                {
                    str += CheckBoxRegStatusList.Items[i].Value + ",";
                    AllCheckCount++;
                    if (AllCheckCount == CheckBoxRegStatusList.Items.Count)
                        IsAllCheck = true;
                }
                else
                {
                    if (AllUncheckCount == CheckBoxRegStatusList.Items.Count)
                        IsAllCheck = false;
                    AllUncheckCount++;
                }
                
            }
            if (IsAllCheck == true)
            {
                HfRegStatusId.Value = "0";
                ChkRegStatusAll.Checked = true;
            }
            else
            {
                HfRegStatusId.Value = str;
                ChkRegStatusAll.Checked = false;
            }
        
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ChkAllInstitute_CheckedChanged(object sender, EventArgs e)
    {
        try 
        {

            if (ChkAllInstitute.Checked == true)
            {
                TxtInstituteName.Text = "";
                TxtInstituteName.Enabled = false;
                
            }
            else
                TxtInstituteName.Enabled = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            aceSearch.ContextKey = ddlCourseName.SelectedValue;
        
        }
        catch (Exception ex)
        { 
            ShowAlert(ex.Message); 
        }
    }
    protected void ddlSubCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            HfApplicantType.Value = "Dir";
            TrIns1.Visible = false;
            TrIns2.Visible = false;
            if (ddlSubCriteria.SelectedValue != "0")
            {
               
                if (ddlDisplayCriteria.SelectedValue == "AT")
                {
                    HfApplicantType.Value = "Dir";
                    if (Convert.ToInt32(ddlSubCriteria.SelectedValue) == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        TrIns1.Visible = true;
                        TrIns2.Visible = true;
                        HfApplicantType.Value = "Ins";
                        //UpdatePanel5.Visible = true;
                        //UpdatePanel5.Update();
                    }

                }
            }
               
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }   
}