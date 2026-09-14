using System;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class FrmdashBoard : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                // BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Profile", "CAND/myprofile.aspx", ""));
                showdata();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                // fetch candidate record according to login
                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();


                // if one of the (father + mother) or guardian name entirely is missing ,show personal details update option
                if ((candidate.FatherName == null || candidate.FatherName == "") && (candidate.MotherName == null || candidate.MotherName == ""))
                {
                    divpersonal.Visible = true;
                }

                if (candidate.ContactDetails.Count > 0)
                {
                    foreach (CandidateContactDetail contact in candidate.ContactDetails)
                    {
                        if ((contact.MobileNumber == null) && (contact.EmailAddress == "" || contact.EmailAddress == null))
                        {
                            divcontact.Visible = true;
                        }

                    }
                }
                //foreach (Address add in candidate.Addresses)
                //{
                //    if (add.AddressTypeID == Convert.ToInt32(enmAddressType.PermanentAddress))
                //    {
                //        //LblPerHouseNo.Text = add.AddressLine1;
                //        //LblPerCity.Text = add.AddressLine2;
                //        //LblPerTehsil.Text = add.AddressLine3;
                //        //LblPerState.Text = add.State.Name;
                //        //if (add.DistrictID.HasValue)
                //        //    LblPerDistrict.Text = add.District.Name;
                //        //else
                //        //    LblPerDistrict.Text = "-";
                //        //LblPerPinCode.Text = (add.PinCode.HasValue ? add.PinCode.Value : 0).ToString();
                //    }

                //    if (add.AddressTypeID == Convert.ToInt32(enmAddressType.CorrespondenceAddress))
                //    {
                //        //LblHouseNo.Text = add.AddressLine1;
                //        //LblCity.Text = add.AddressLine2;
                //        //LblTehsil.Text = add.AddressLine3;
                //        //LblState.Text = add.State.Name;
                //        //if (add.DistrictID.HasValue)
                //        //    LblDistrict.Text = add.District.Name;
                //        //else
                //        //    LblDistrict.Text = "-";
                //        //LblPincode.Text = (add.PinCode.HasValue ? add.PinCode.Value : 0).ToString();

                //    }
                //}

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void Lnlcontact_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEditCandidateDetail.aspx?SrcType=1");
    }
    //protected void Lnladdress_Click(object sender, EventArgs e)
    //{

    //}
    //protected void Lnkpersonal_Click(object sender, EventArgs e)
    //{

    //}
}