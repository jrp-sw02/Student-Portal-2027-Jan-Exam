using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;
using Ionic.Zip;

public partial class Common_CertigicateImageDownload : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                BindCourseLevel();
            }

        }
        catch (Exception ex)
        { lblerror.Text = ex.Message; }

    }

    protected void BindCourseLevel()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var level = from p in context.CertificateOABCPhotos
                            orderby (p.LevelCode)
                            select new { ValueField = p.LevelCode, TextField = p.LevelCode };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, level.Distinct(), lst);
            };
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
            using (EConnectContext context = new EConnectContext())
            {
                string level = ddlCourseName.SelectedValue;
                ListItem lst = new ListItem("--Select One--", "0");
                var Phase = from p in context.CertificateOABCPhotos
                            where p.LevelCode == level
                            orderby p.CertificatePhaseNo descending
                            select new { ValueField = p.CertificatePhaseNo, TextField = p.CertificatePhaseNo };
                Phase = Phase.Distinct().OrderByDescending(p => p.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPhaseNumber, Phase, lst);

                lblTotalCand.Text = "Total Number of Candidate :";
                lblPhotoCand.Text = "Available Photo :";
                lblNoPhotoCand.Text = "Missing Photo : ";
                lblMissingCand.Visible = false;
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnphoto_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string level = ddlCourseName.SelectedValue;
                Int32 phase = Convert.ToInt32(ddlPhaseNumber.SelectedValue);

                //Main query.
                var images = (from a in context.CertificateOABCPhotos.AsNoTracking()
                              join c in context.UploadedFiles.AsNoTracking() on a.PhotofileId equals c.ID
                              where (a.LevelCode == level && a.CertificatePhaseNo == phase)
                              select new
                              {
                                  Photo = c.BlobFile,
                                  Regno = a.RegistrationNo,
                                  level = a.LevelCode
                              }).ToList();

                String directoryName = level.Trim() + "_" + ddlPhaseNumber.SelectedItem.Text.Replace(" ", "").Replace(",", "");
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName));
                string photoName = "";
                foreach (var photo in images)
                {
                    if (level == "ACC")
                        photoName = photo.Regno.ToString();
                    else
                        photoName = level.Trim() + photo.Regno.ToString();
                    FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + ".jpg"));
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(photo.Photo);
                    bw.Close();
                    fs.Close();
                }
                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
                    zipFile.Save(Response.OutputStream);
                }
                System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName), true);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlPhaseNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string level = ddlCourseName.SelectedValue;
                Int32 phase = Convert.ToInt32(ddlPhaseNumber.SelectedValue);

                //Main query.
                int totalCand = context.CertificateOABCPhotos.Where(s => s.LevelCode == level && s.CertificatePhaseNo == phase).Count();
                lblTotalCand.Text = "Total Number of Candidate :" + totalCand;

                int Availableimages = context.CertificateOABCPhotos.Where(s => s.LevelCode == level && s.CertificatePhaseNo == phase && s.PhotofileId != null).Count();

                lblPhotoCand.Text = "Available Photo : " + Availableimages;

                lblNoPhotoCand.Text = "Missing Photo : " + (totalCand - Availableimages);

                if ((totalCand - Availableimages) > 0)
                {
                    lblMissingCand.Text = "";
                    lblMissingCand.Visible = true;
                    var missingPhotoRegn = context.CertificateOABCPhotos.Where(s => s.LevelCode == level && s.CertificatePhaseNo == phase && s.PhotofileId == null).ToList();
                    for (int i = 0; i < missingPhotoRegn.Count(); i++)
                    { lblMissingCand.Text += missingPhotoRegn[i].RegistrationNo + ", "; }
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}