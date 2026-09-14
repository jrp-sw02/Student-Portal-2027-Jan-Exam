using System;

public partial class frmmodulelistforexam : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string status = Request.QueryString["status"].ToString();
        //lblHeading.Text = "List Of Candidates  " + status;
        string sts = Convert.ToString(Request.QueryString["v"]);
        if (sts == "p")
        {
            btnapproved.Visible = false;
            btnmodify.Visible = false;
            btnreject.Visible = false;
            a3.HRef = "MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p";
        }
        if (sts == "f")
        {
            btnapproved.Visible = false;
            btnmodify.Visible = false;
            btnreject.Visible = false;
            a3.HRef = "MystudentList.aspx?status=For Pending Forword&course=Pending For Forword&exam=July 2012&st=2&sts=f";
        }
        if (sts == "j")
        {
            a3.HRef = "MystudentList.aspx?status=for Examination&course=New Applications&exam=July 2012&st=0&sts=a&sts=j";
        }

        
        string exam = Convert.ToString(Request.QueryString["exam"]);
        if (exam != null)
        {
            //Div_Course.Visible = false;
            //Div_Exam.Visible = true;
            a1.InnerText = "Exam Application Status";
            a1.HRef = "acc_exam_info.aspx";
           
        }
        else
        {
            //Div_Course.Visible = true;
            //Div_Exam.Visible = false;
            //a1.HRef = "acc_reg_info.aspx";
            //a1.InnerText = "Course Registration Status";
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
         string sts = Convert.ToString(Request.QueryString["v"]);
        if(sts=="j")
        {
            Response.Redirect("~/MystudentList.aspx?status=for Examination&course=Pending For Verification&exam=July 2012&st=0&sts=j");
        }
        if(sts=="p")
        {
            Response.Redirect("MystudentList.aspx?status=For Pending Payment&course=Panding For Payment&exam=July 2012&st=1&sts=p");
        }
    }
}