using System;

public partial class ExamDetail : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        display();
        string id =  Convert.ToString(Request.QueryString["ID"]);
        string tt = Convert.ToString(Session["ModuleID"]);
        Sidelink.Items.Add(new SideLinkItem("Apply Online", "../CAND/FrmExamForm.aspx?ID="+id,"../images/Apply_Online.jpg"));
        Sidelink.Items.Add(new SideLinkItem("View Filled Application", "../WEB/FilledForm.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
        Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "../WEB/DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
        Sidelink.Items.Add(new SideLinkItem("View Result", "../WEB/Result.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/View_Result.jpg"));
        Sidelink.Items.Add(new SideLinkItem("View Course Status", "../WEB/CCStatus.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
        Sidelink.Items.Add(new SideLinkItem("Check Accredited Centre", "../WEB/FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
        Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
        Sidelink.Render();
        Sidelink1.Items.Add(new SideLinkItem("Download Notification", "../Download/Notification-BCC.pdf", "", "_blank"));
        Sidelink1.Items.Add(new SideLinkItem("Download Brochure", "../Download/Notification-BCC.pdf", "", "_blank"));
        Sidelink1.Items.Add(new SideLinkItem("Download Exam Schedule", "../Download/Notification-BCC.pdf", "", "_blank"));
        Sidelink1.Items.Add(new SideLinkItem("Download Syllabus", "../Download/syllabusbcc.pdf", "", "_blank"));
        Sidelink1.Items.Add(new SideLinkItem("Download Date Sheet", "../Download/Notification-BCC.pdf", "", "_blank"));
        Sidelink1.Items.Add(new SideLinkItem("Download FAQ", "../Download/Notification-BCC.pdf", "", "_blank"));
        Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
        Sidelink1.Render();

    }
    protected void LnkApply4Exam_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/FrmExamform.aspx");
    }
    protected void LnkApply4Exam2_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/FrmExamform.aspx");
    }
    protected void LnkNextExamDate_Click(object sender, EventArgs e)
    {
        string lvl = "1";
        Response.Redirect("~/FrmNextExamDate.aspx?ID=" + lvl);
    }
    protected void LnkNextExamDate0_Click(object sender, EventArgs e)
    {
        string lvl = "2";
        Response.Redirect("~/FrmNextExamDate.aspx?ID=" + lvl);
    }
    protected void LnkExam1_Click(object sender, EventArgs e)
    {
         string level = Convert.ToString(Request.QueryString["level"]);
         Session["exam"] = LnkExam1.Text;
             Response.Redirect("~/CAND/frmexamhistory.aspx?level=" + level + "&exam=" + Session["exam"]);
                          
   }
    public void display()
    {
        string level = Convert.ToString(Session["level1"]);
        if (level == "1")
        {
            //div_O.Visible = true;
            //div_A.Visible = false;
            Span1.InnerText = "Exam Details of O Level";
            l1.Visible = true;
            //spn2.InnerText = "MyCourses: O Level";

        }
        else if (level == "A")
        {
            Span1.InnerText = "Exam Details of A Level";
            l1.Visible = true;
           // spn2.InnerText = "MyCourses: A Level";
            //div_O.Visible = false;
            //div_A.Visible = true;
        }
    }
    protected void LnkExam2_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Request.QueryString["level"]);
        Session["exam"] = LnkExam2.Text;
        Response.Redirect("~/CAND/frmexamhistory.aspx?level=" + level + "&exam=" + Session["exam"]);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        string tt = Convert.ToString(Session["ModuleID"]);
        Response.Redirect("~/FilledForm.aspx?Name= " + level1 + " &Cat=Course&type=" + tt);
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        string tt = Convert.ToString(Session["ModuleID"]);
        Response.Redirect("~/DownloadAdmitCard.aspx?Name="+level1+ "&Cat=Course&type="+tt);
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        string tt = Convert.ToString(Session["ModuleID"]);
        Response.Redirect("~/ApplicationStatus.aspx?Name=" + level1 + "&Cat=Course&type=" + tt);

    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        string tt = Convert.ToString(Session["ModuleID"]);
        Response.Redirect("~/Result.aspx?Name=" + level1 + "&Cat=Course&type=" + tt);

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
       // Response.Redirect("~/NielitRegistration.aspx");
        //string level = Convert.ToString(Session["level1"]);
        //string level1 = level + "Level";
        //Response.Redirect("~/RulesForOnlineRegistration.aspx?Name=" + level1);
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        Response.Redirect("~/FrmExamForm.aspx?Name="+level1);
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        string tt = Convert.ToString(Session["ModuleID"]);
        Response.Redirect("~/CCStatus.aspx?Name=" + level1 + "&Cat=Course&type="+tt);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string level = Convert.ToString(Session["level1"]);
        string level1 = level + "Level";
        string tt = Convert.ToString(Session["ModuleID"]);
        Response.Redirect("~/FrmAccredetedCentre.aspx?Name=" + level1 + "&Cat=Course&type=" + tt);
    }
}