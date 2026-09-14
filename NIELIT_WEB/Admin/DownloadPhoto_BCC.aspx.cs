using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;
using Ionic.Zip;

public partial class Admin_DownloadPhoto_BCC : BasePage
{

    UserType loginUserType;
    Int64 entityID = 0;
    // Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
      //  photoDownload();
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
        if (!UserManager.HasRight(currentRoleId, enmRight.View))
        {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
        }
    }

    private DataTable photoDownload()
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("Temp_exam_database_photo_download"))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;                

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }

    public void ZippedFile(string directoryName)
    {
        string p = Server.MapPath("~/download/" + directoryName);
        if (Directory.Exists(p))
        {
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                Response.Clear();
                zipFile.CompressionMethod = CompressionMethod.None;
                zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
                zipFile.Save(Response.OutputStream);
            };

        }
        else
        {
            ShowAlert("Data not available for this category.", true);
            return;
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        //FileStream fs = new FileStream();
        //BinaryWriter bw = new BinaryWriter();
        try
        {
            String directoryName = "Temp_exam_database_photo_download";

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            DataTable DT = photoDownload();
            if (DT.Rows.Count > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {

                    //  var Ids = ( from c in context.CertificateExamApplications
                    //              select new { id = c.ID}).ToList();
                    //var photos = (from a in context.CertificateExamApplications.AsNoTracking()
                    //              // join b in context.BatchItems on a.BatchItemID equals b.ID
                    //              where a.Number.Contains(DT.Columns[1].ToString())
                    //              //Batches.Contains(b.BatchID)
                    //              select new { a.Photo, a.Number, a.RollNumber }).ToList();
                }
                foreach (DataRow dr in DT.Rows)
                {

                    Int32 iD = Convert.ToInt32(dr["ID"].ToString());
                    using (EConnectContext context = new EConnectContext())
                    {
                        var photos = (from c in context.CertificateExamApplications
                                      where c.ID == iD
                                      select new { photo = c.Photo, number = c.Number }).FirstOrDefault();

                        //var photos = (from a in context.CertificateExamApplications.AsNoTracking()
                        //             // join b in context.BatchItems on a.BatchItemID equals b.ID
                        //              where a.ID.Contains(iD)
                        //              //Batches.Contains(b.BatchID)
                        //              select new { a.Photo, a.Number, a.RollNumber }).ToList();
                        string photoName = photos.number;

                        FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpeg"));
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(photos.photo);
                        bw.Flush();
                        fs.Flush();
                        bw.Close();
                        fs.Close();
                    }
                    ZippedFile(directoryName);

                }

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

        finally
        {
            throw new Exception();
        }
    }
}