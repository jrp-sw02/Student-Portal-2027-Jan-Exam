using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.Services.Protocols;
using System.Web.Services;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.HRMS;
using System.Web.Security;
using EConnect.Utils.Common;

public partial class UserControl_Address : System.Web.UI.UserControl
{
#region Public Properties
    public Int32 AddressTypeID
    {
        set
        {
            ddlAddrType.SelectedValue =  value.ToString();
            ddlAddrType.Enabled = false;
        }
        get { return Convert.ToInt32(ddlAddrType.SelectedValue.ToString()); }
    }
    public enmAddressType AddressType
    {
        get { return (enmAddressType)Convert.ToInt32(ddlAddrType.SelectedValue.ToString()); }
    }
    public Int64 DistrictID
    {
        set
        {
            ddlDistrict.SelectedValue = value.ToString();
        }
        get { return Convert.ToInt64(ddlDistrict.SelectedValue.ToString()); }
    }
    public Int64 StateID
    {
      
        get { return Convert.ToInt64(ddlState.SelectedValue.ToString()); }
        set
        {
            ddlState.SelectedValue = value.ToString();
            ddlState_SelectedIndexChanged(ddlState, EventArgs.Empty);
        }
    }
    public String CityName
    {
        set { txtCity.Text  = value.ToString(); }
        get { return txtCity.Text; }
    }
    public String Address1
    {
        set { txtAddress1.Text = value.ToString(); }
        get { return txtAddress1.Text; }
        
    }
    public String Address2
    {
        set { txtAddress2.Text = value.ToString(); }
        get { return txtAddress2.Text; }
    }
    public String Address3
    {
        set { txtAddress3.Text = value.ToString(); }
        get { return txtAddress3.Text; }
    }
    public String Pin
    {
        set { txtPin.Text = value.ToString(); }
        get { return txtPin.Text; }
    }
#endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            EnumUtility.BindListObject(ref ddlAddrType, typeof(EConnect.enmAddressType), new ListItem("--Select One--", "0"));
            FillState();
        }
    }
    public void FillState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillState = (from p in context.Locations
                                 where p.LocationTypeID == 2
                                 orderby p.Name ascending
                                 select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, fillState, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDistrict(Convert.ToInt64(ddlState.SelectedValue));
    }
    private void FillDistrict(Int64 stateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district2 = from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 4 && s.ParentLocationID == stateID
                                select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlDistrict, district2, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}