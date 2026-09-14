using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;

public partial class Admin_ChangeBCCCCCCandidatePhoto : BasePage
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
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Common/SearchBCCCCCandidate.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!IsPostBack)
            {
                lblError.Visible = false;
            }
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Change Candidate Images", "", ""));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlphoto_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            using (EConnectContext context = new EConnectContext())
            {
                var certificate = (from a in context.CertificateExamApplications
                                 where a.ID == ApplicationID && a.CourseID == CourseID
                                 select a).FirstOrDefault();
                if (ddlphoto.SelectedValue == "1")
                {
                    if (certificate.Photo != null && certificate.Photo[0] != 1 && certificate.Photo[1] != 35)
                    {
                        ImgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])certificate.Photo);
                        Lbname.Text = "Current View of Candidate Photograph";
                        Lbbrowse.Text = "Browse new Candidate Photograph Image";
                        ImgPhoto.Height = Unit.Pixel(130);
                        ImgPhoto.Width = Unit.Pixel(112);
                    }
                    else
                    {
                        ImgPhoto.ImageUrl = "../images/photo.jpg";
                        Lbbrowse.Text = "Browse new Candidate Photograph Image";
                        ShowAlert("Candidate Photo not available", true);
                        return;
                    }
                }
                else if (ddlphoto.SelectedValue == "2")
                {
                    if (certificate.Signature != null && certificate.Signature[0] != 1 && certificate.Signature[1] != 35)
                    {
                        ImgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])certificate.Signature);
                        Lbname.Text = "Current View of Candidate Signature";
                        Lbbrowse.Text = "Browse new Candidate Signature Image";
                        ImgPhoto.Width = Unit.Pixel(112);
                        ImgPhoto.Height = Unit.Pixel(50);
                    }
                    else
                    {
                        ImgPhoto.ImageUrl = "../images/photo.jpg";
                        Lbbrowse.Text = "Browse new Candidate Signature Image";
                        ShowAlert("Candidate Signature not available", true);
                        return;
                    }
                }
                else if (ddlphoto.SelectedValue == "3")
                {
                    if (certificate.LeftThumb != null && certificate.LeftThumb[0] != 1 && certificate.LeftThumb[1] != 35)
                    {
                        ImgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])certificate.LeftThumb);
                        Lbname.Text = "Current View of Candidate Thumb";
                        Lbbrowse.Text = "Browse new Candidate Thumb Impression Image";
                        ImgPhoto.Width = Unit.Pixel(112);
                        ImgPhoto.Height = Unit.Pixel(60);
                    }
                    else
                    {
                        ImgPhoto.ImageUrl = "../images/photo.jpg";
                        Lbbrowse.Text = "Browse new Candidate Thumb Impression Image";
                        ShowAlert("Candidate Thumb Impression not available", true);
                        return;
                    }
                }
                else
                {
                    ImgPhoto.ImageUrl = "../images/photo.jpg";
                    Lbname.Text = "Current View";
                    Lbbrowse.Text = "Browse new image";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
                Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
                using (EConnectContext context = new EConnectContext())
                {
                    var certificate = (from a in context.CertificateExamApplications
                                       where a.ID == ApplicationID && a.CourseID == CourseID
                                       select a).FirstOrDefault();

                    if (ddlphoto.SelectedValue == "1")
                    {
                        certificate.PhotoFileName = ImgUpload.FileName.ToString();
                        certificate.Photo = ImgUpload.FileBytes;
                        context.Entry(certificate).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        Lbfinal.Text = "You have successfully changed the candidate photo image.";
                    }
                    else if (ddlphoto.SelectedValue == "2")
                    {
                        certificate.SignatureFileName = ImgUpload.FileName.ToString();
                        certificate.Signature = ImgUpload.FileBytes;
                        context.Entry(certificate).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        Lbfinal.Text = "You have successfully changed the candidate signature image.";

                    }
                    else if (ddlphoto.SelectedValue == "3")
                    {
                        certificate.LeftThumbFileName = ImgUpload.FileName.ToString();
                        certificate.LeftThumb = ImgUpload.FileBytes;
                        context.Entry(certificate).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        Lbfinal.Text = "You have successfully changed the candidate thumb impression image.";
                    }
                    tbfilter.Visible = false;
                    divfinal.Visible = true;
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminRegstudbcc.aspx?ApplID=" + Request.QueryString["ApplID"] + "&CourseId=" + Request.QueryString["CourseId"] + "&regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"]+ "&couID=" + Request.QueryString["couID"] + "&Src=Search"), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void LnkBtnBacktoProfile1_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int64 ApplicationID = Convert.ToInt64(Request.QueryString["ApplID"]);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminRegstudbcc.aspx?ApplID=" + Request.QueryString["ApplID"] + "&CourseId=" + Request.QueryString["CourseId"] + "&regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"] + "&couID=" + Request.QueryString["couID"] + "&Src=Search"), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public bool IsValidForm()
    {
        try
        {
            
                if (ImgUpload.FileName.ToString() == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Photo can not be left blank";
                    return false;
                }
                if (!isvalidFileExtension(ImgUpload))
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Photo .Only jpg, gif, jpeg ,png extensions are allowed.";
                    return false;
                }
                if (!isvalidFileSize(ImgUpload, 51200))
                {
                    lblError.Visible = true;
                    lblError.Text = "Photo File size should be of 50 KB or less.";
                    return false;
                }
               if (ImgUpload.FileName.Length > 50)
                {
                    lblError.Visible = true;
                    lblError.Text = "Photo File name size should be less than 50 characters.";
                    return false;
                }
            
            lblError.Visible = false;
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}