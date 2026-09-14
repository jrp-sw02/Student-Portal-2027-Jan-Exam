using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_UploadCourseDetails : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_upload_course_file_Click(object sender, EventArgs e)
    {
        Page.Validate("s");
        if (Page.IsValid)
        {
            try
            {
                DateTime fr_date, to_date;
                if (!string.IsNullOrEmpty(txt_fr_date.Text))
                    fr_date = DateTime.ParseExact(txt_fr_date.Text, "dd-MMM-yyyy", new DateTimeFormatInfo()).Date;
                else fr_date = Convert.ToDateTime("01-Jan-1900").Date;

                if (!string.IsNullOrEmpty(txt_to_date.Text))
                    to_date = DateTime.ParseExact(txt_to_date.Text, "dd-MMM-yyyy", new DateTimeFormatInfo()).Date;
                else to_date = Convert.ToDateTime("01-Jan-1900").Date;

                string login_user;
                try { login_user = Session["UserID"].ToString(); }
                catch { login_user = "Null"; }

                // limitation of maximum file size(5 MB)
                int intFileSizeLimit = 5242880;

                // get the full path of your computer               
                string strFileNameWithPath = Path.GetFullPath(fu_course_file.FileName.Trim());
                //string strFileNameWithPath = FileUpload1.PostedFile.FileName;

                // get the extension name of the file
                string strExtensionName = System.IO.Path.GetExtension(strFileNameWithPath).ToLower();

                // get the filename of user file
                string strFileName = System.IO.Path.GetFileName(strFileNameWithPath);

                // get the file size
                int intFileSize = fu_course_file.PostedFile.ContentLength;

                // Restrict the user to upload only following file types ...
                string filetypes = ".pdf";

                if (!string.IsNullOrEmpty(strFileNameWithPath))
                {
                    bool check_file_name = ClassJKS.check_upload_file_name(strFileName);

                    if (check_file_name == true)
                    {
                        if (filetypes.IndexOf(strExtensionName) >= 0 && ClassJKS.check_pdf_file(strFileName))
                        {
                            // Restrict the File Size 
                            if (intFileSize <= intFileSizeLimit)
                            {
                                lbl_fu_msg.Text = "";
                                //FileStream fs = new FileStream(strFileNameWithPath, FileMode.Open, FileAccess.Read);
                                Stream fs = fu_course_file.PostedFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                string Base64bytes = Convert.ToBase64String(bytes, 0, bytes.Length);
                                br.Close();
                                fs.Close();
                                int course_id = string.IsNullOrEmpty(ddl_course_name.SelectedValue) ? -1 : int.Parse(ddl_course_name.SelectedValue);

                                //insert the file into database
                                strFileName = "Course_File_" + ddl_course_category.SelectedValue + "-" + ddl_course_name.SelectedValue + strExtensionName;
                                SqlCommand cmd = new SqlCommand("JKS_InsertCourseFile");
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@course_id", SqlDbType.Int).Value = course_id;
                                cmd.Parameters.Add("@course_eligibility_doc", SqlDbType.VarChar).Value = Base64bytes;
                                cmd.Parameters.Add("@from_dt", SqlDbType.Date).Value = fr_date;
                                cmd.Parameters.Add("@to_dt", SqlDbType.Date).Value = to_date;
                                cmd.Parameters.Add("@entered_by", SqlDbType.VarChar, 50).Value = login_user;


                                if (ClassJKS.InsertUpdateData(cmd))
                                {
                                    lbl_fu_msg.Text = "Course file for the selected category has been uploaded successfully (File Name: " + strFileName + ")"; ;
                                    bind_grd_course_file();
                                }
                                else
                                {
                                    lbl_fu_msg.Text = "Error uploading file ...";
                                }
                            }
                            else
                            {
                                lbl_fu_msg.Text = "File to be uploaded cannot be more than 5 MB...";
                            }
                        }
                        else
                        {
                            lbl_fu_msg.Text = "Source file type not allowed for uploading [ " + strExtensionName + " ] ... only PDF files allowed";
                        }
                    }
                    else
                    {
                        lbl_fu_msg.Text = "Invalid file name [ " + strFileName + " ]";
                    }
                }
                else
                {
                    lbl_fu_msg.Text = "Select file to be uploaded ...";
                }
            }
            catch (Exception ex)
            {
                lbl_fu_msg.Text = "Error Uploading File : " + ex.Message;
                //or may be redirected to a separate error page
            }
        }
        else
        {
            lbl_fu_msg.Text = "Please check input values and try again ...";
        }
    }

    private void bind_grd_course_file()
    {
        if (cdd_course_category.SelectedValue != "" && cdd_course_name.SelectedValue != "")
        {
            //DataSet ds = ClassJKS.JKS_GetCourseIDFiles(int.Parse(cdd_course_name.SelectedValue));

            int course_id = string.IsNullOrEmpty(ddl_course_name.SelectedValue) ? -1 : int.Parse(ddl_course_name.SelectedValue);

            SqlCommand cmd = new SqlCommand("JKS_GetCourseIDFiles");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@course_id", SqlDbType.Int).Value = course_id;
            DataSet ds = ClassJKS.ReturnDataset(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_course_file.DataSource = ds;
                grd_course_file.DataBind();
            }
            else
            {
                grd_course_file.DataSource = null;
                grd_course_file.DataBind();
            }
            ds.Dispose();
        }
    }

    protected void ddl_course_name_SelectedIndexChanged(object sender, EventArgs e)
    {
        bind_grd_course_file();
    }
}