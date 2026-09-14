using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;

public partial class Admin_ChangeCandidatePhoto : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Common/SearchCandidate.aspx"))
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
            Int64 candidateid = Convert.ToInt64(Request.QueryString["CandidateID"]);
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from a in context.Candidates
                                 where a.ID == candidateid
                                 select a).FirstOrDefault();
                if (ddlphoto.SelectedValue == "1")
                {
                    if (candidate.PhotoFileID == null || candidate.PhotoFileID==0)
                    {
                        Lbbrowse.Text = "Browse new Candidate Photograph Image";
                        ShowAlert("Candidate Photo not available", true);
                        return;
                    }
                    else
                    {
                        ImgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile);
                        Lbname.Text = "Current View of Candidate Photograph";
                        Lbbrowse.Text = "Browse new Candidate Photograph Image";
                        ImgPhoto.Height = Unit.Pixel(130);
                        ImgPhoto.Width = Unit.Pixel(112);
                    }
                }
                else if (ddlphoto.SelectedValue == "2")
                {
                    if (candidate.SignatureFileID == null || candidate.SignatureFileID == 0)
                    {
                        Lbbrowse.Text = "Browse new Candidate Signature Image";
                        ShowAlert("Candidate Signature not available", true);
                        return;
                    }
                    else
                    {
                        ImgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Signature.BlobFile);
                        Lbname.Text = "Current View of Candidate Signature";
                        Lbbrowse.Text = "Browse new Candidate Signature Image";
                        ImgPhoto.Width = Unit.Pixel(112);
                        ImgPhoto.Height = Unit.Pixel(50);
                        
                    }
                }
                else if (ddlphoto.SelectedValue == "3")
                {
                    if (candidate.LeftThumbImpressionFileID == null || candidate.LeftThumbImpressionFileID ==0)
                    {
                        Lbbrowse.Text = "Browse new Candidate Thumb Impression Image";
                        ShowAlert("Candidate Thumb Impression not available", true);
                        return;
                    }
                    else
                    {
                        ImgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.LeftThumbImpression.BlobFile);
                        Lbname.Text = "Current View of Candidate Thumb";
                        Lbbrowse.Text = "Browse new Candidate Thumb Impression Image";
                        ImgPhoto.Width = Unit.Pixel(112);
                        ImgPhoto.Height = Unit.Pixel(60);
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
                Int64 candidateid = Convert.ToInt64(Request.QueryString["CandidateID"]);
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        var candidate = (from a in context.Candidates
                                         where a.ID == candidateid
                                         select a).FirstOrDefault();
                        if (ddlphoto.SelectedValue == "1")
                        {
                            if (candidate.PhotoFileID == null || candidate.PhotoFileID == 0)
                                candidate.TempPhoto = null;
                            else
                                candidate.TempPhoto = candidate.Photo.BlobFile;
                            UploadedFile objPhoto = new EConnect.NIELIT.UploadedFile();
                            objPhoto.Name = "LR-P-" + candidate.ID; //Photo File Name Legend Indicate Particular Candidate's Photo
                            objPhoto.OriginalName = ImgUpload.FileName.ToString();
                            objPhoto.BlobFile = ImgUpload.FileBytes;
                            objPhoto.Extension = System.IO.Path.GetExtension(ImgUpload.FileName.ToString());
                            objPhoto.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(objPhoto);
                            context.SaveChanges();
                            candidate.PhotoFileID = objPhoto.ID;
                            Lbfinal.Text = "You have successfully changed the candidate photo image.";
                        }
                        else if (ddlphoto.SelectedValue == "2")
                        {
                            if (candidate.SignatureFileID == null || candidate.SignatureFileID == 0)
                                candidate.TempSignature = null;
                            else
                                candidate.TempSignature = candidate.Signature.BlobFile;
                            UploadedFile objSignature = new EConnect.NIELIT.UploadedFile();
                            objSignature.Name = "LR-S-" + candidate.ID;//Photo File Name Legend Indicate Particular Candidate's Signature
                            objSignature.OriginalName = ImgUpload.FileName.ToString();
                            objSignature.BlobFile = ImgUpload.FileBytes;
                            objSignature.Extension = System.IO.Path.GetExtension(ImgUpload.FileName.ToString());
                            objSignature.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(objSignature);
                            context.SaveChanges();
                            candidate.SignatureFileID = objSignature.ID;
                            Lbfinal.Text = "You have successfully changed the candidate signature image.";
                            
                        }
                        else if (ddlphoto.SelectedValue == "3")
                        {
                            if (candidate.LeftThumbImpressionFileID == null || candidate.LeftThumbImpressionFileID == 0)
                                candidate.TempLeftThumb = null;
                            else
                                candidate.TempLeftThumb = candidate.LeftThumbImpression.BlobFile;
                            UploadedFile objThumb = new EConnect.NIELIT.UploadedFile();
                            objThumb.Name = "LR-T-" + candidate.ID;//Photo File Name Legend Indicate Particular Candidate's Left Thumb Impression
                            objThumb.OriginalName = ImgUpload.FileName.ToString();
                            objThumb.BlobFile = ImgUpload.FileBytes;
                            objThumb.Extension = System.IO.Path.GetExtension(ImgUpload.FileName.ToString());
                            objThumb.UploadedOn = DateTime.Now;
                            context.UploadedFiles.Add(objThumb);
                            context.SaveChanges();
                            candidate.LeftThumbImpressionFileID = objThumb.ID;
                            Lbfinal.Text = "You have successfully changed the candidate thumb impression image.";
                            
                        }
                        context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        scope.Complete(); // Transaction Process finally Complete    
                        tbfilter.Visible = false;
                        divfinal.Visible = true;
                    };
                }
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
            Int64 candidateid = Convert.ToInt64(Request.QueryString["CandidateID"]);
            Int64 Regno = Convert.ToInt64(Request.QueryString["Regno"]);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminRegstud.aspx?Regno=" + Request.QueryString["Regno"] + "&CourseId=" + Request.QueryString["CourseId"] + "&ApplID=" + Request.QueryString["candidateid"] + "&regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&Status=" + Request.QueryString["Status"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"] + "&Src=Search"), true);
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
            Int64 candidateid = Convert.ToInt64(Request.QueryString["CandidateID"]);
            Int64 Regno = Convert.ToInt64(Request.QueryString["Regno"]);
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminRegstud.aspx?Regno=" + Request.QueryString["Regno"] + "&CourseId=" + Request.QueryString["CourseId"] + "&ApplID=" + Request.QueryString["candidateid"] + "&regdate=" + Request.QueryString["regdate"] + "&regtodate=" + Request.QueryString["regtodate"] + "&filcriteria=" + Request.QueryString["filcriteria"] + "&Status=" + Request.QueryString["Status"] + "&couID=" + Request.QueryString["couID"] + "&AptpID=" + Request.QueryString["AptpID"] + "&dob=" + Request.QueryString["dob"] + "&index=" + Request.QueryString["index"] + "&Src=Search"), true);
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
            
            lblError.Visible = false;
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}