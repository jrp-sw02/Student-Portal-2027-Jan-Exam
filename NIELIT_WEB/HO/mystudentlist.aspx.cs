using System;

public partial class MystudentList : BasePage
{
   string abc;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        string status = Request.QueryString["status"].ToString();
        lblHeading.Text = "List Of Candidates  " + status;
        string exam = Convert.ToString(Request.QueryString["exam"]);
        string st = Convert.ToString(Request.QueryString["st"]);
       abc=st;
        
        if (st == null)
        {
            btnok.Visible = false;
            btncancel.Text = "Back";
        }
        if (st == "1")
        {
            btnok.Text = "Pay Now";
            btncancel.Text = "Back";
            status1.InnerText = "Not Paid & Verified";
            status2.InnerText = "Not Paid & Verified";
            status3.InnerText = "Not Paid & Verified";
            status4.InnerText = "Not Paid & Verified";
            status5.InnerText = "Not Paid & Verified";

        }
        if (st == "2")
        {
            btnok.Visible = false;
            chk.Visible = false;
            ch1.Visible = false;
            ch2.Visible = false;
            ch3.Visible = false;
            ch4.Visible = false;
            ch5.Visible = false;
            rno1.HRef = "#";
            rno2.HRef = "#";
            rno3.HRef = "#";
            rno4.HRef = "#";
            status1.InnerText = "Pending For Approval";
            status2.InnerText = "Pending For Approval";
            status3.InnerText = "Pending For Approval";
            status4.InnerText = "Pending For Approval";
            status5.InnerText = "Pending For Approval";
            //btnok.Text = "Forword";
            btncancel.Text = "Back";
        }
        if (st == "3")
        {
            btnok.Visible = true;
            btnok.Text = "Send Alert";
              
            rno1.HRef = "#";
            rno2.HRef = "#";
            rno3.HRef = "#";
            rno4.HRef = "#";
            status1.InnerText = "Verified But Not Paid";
            status2.InnerText = "Verified But Not Paid";
            status3.InnerText = "Verified But Not Paid";
            status4.InnerText = "Verified But Not Paid";
            status5.InnerText = "Verified But Not Paid";
            //btnok.Text = "Forword";
            btncancel.Text = "Back";
        }
        if (st == "0")
        {
            btnok.Visible = false;
            btncancel.Text = "Back";
        }
        if (exam != null)
        {
            Div_Course.Visible = false;
            Div_Exam.Visible = true;
            a1.InnerText = "Exam Registration Status";
            a1.HRef = "acc_exam_info.aspx";
        }
        else
        {
            
            Div_Course.Visible = true;
            Div_Exam.Visible = false;
            a1.HRef = "acc_reg_info.aspx";
            a1.InnerText = "Course Registration Status";
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/acc_exam_info.aspx");
    }
    protected void btnok_Click(object sender, EventArgs e)
    {
        if (abc == "1")
        {
            Response.Redirect("~/frmconfirm.aspx");
        }
    }
}