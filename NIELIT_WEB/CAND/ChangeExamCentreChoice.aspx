<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" 
    CodeFile="ChangeExamCentreChoice.aspx.cs" Inherits="ReportPgae"  Debug="false"%>

<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    
    function printwindow() {
        window.print();
        return false;
    }    
    function userValid() {
        var id = document.getElementById("<%=appId.ClientID%>").value.trim();
        var dob = document.getElementById("<%=txtDob.ClientID%>").value.trim();
        

        if (id == '') {
            alert("Please enter Application Number");
            document.getElementById("<%=appId.ClientID%>").focus();
            return false;           
        }

        if (dob == '') {
            alert("Please enter Date of Birth");
            document.getElementById("<%=txtDob.ClientID%>").focus();
            return false;            
        }        
    }
</script>
    <style type="text/css">
        .error {}
        .auto-style2 {
            height: 26px;
        }
        .auto-style3
        {
            height: 23px;
        }
        .auto-style5
        {
            height: 30px;
        }
        .auto-style6
        {
            color: #FF0000;
            font-size: small;
        }
        .auto-style7
        {
            text-decoration: underline;
            font-size: medium;
        }
        .auto-style9
        {
            font-size: medium;
            text-align: justify;
        }
    p.MsoNormal
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:8.0pt;
	margin-left:0cm;
	line-height:107%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	}
        .auto-style10
        {
            height: 44px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content3"  ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;--%>
  <%--  <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" 
        ToolTip="Export to PDF file" ID="ibExport" ImageUrl="~/images/pdf.jpg" 
        runat="server" onclick="imgPDF_Click" Visible="True" height="30%" width="15%"/>--%>
    <%--<asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="30%" width="15%" />--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
        <%--<asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>--%>
&nbsp;
</asp:Content>
<asp:Content ID="Content5"  ContentPlaceHolderID="cpReportDate" runat="server">
   Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
 <div id="div1" runat="server" style="width:100%;">    
 <table style="width:100%;" id="Table1" runat="server"> 
        <tr>
            <td width="100%" colspan="2" align="center" class="auto-style7" >
                <strong style="font-size: large; text-align: justify;">DLC (CCC/BCC/CCCP/ECC) examinations of April, May, June, August and September 2020 exam cycles are tentatively scheduled from 29th September 2020 onwards.
            </strong>
            </td>

        </tr>
        <tr>
            <td width="100%" colspan="2" align="center" class="auto-style7" >
            </td>

        </tr>
     <tr>
            <td width="100%" colspan="2" class="auto-style9" >
                In view of the ongoing COVID-19 pandemic and in the interest of well-being of candidates who have applied for appearing in the examinations of DLC (CCC/BCC/CCCP/ECC) for the examination cycles viz. April, May, June, August and September 2020, it has been decided by NIELIT to open the facility for candidates to submit their declaration to opt-out from the September 2020 examination or change examination centre preference for the examination commencing from September 29, 2020. Such candidates may change their examination centre preference or retain their original examination centre preference submitted in their OEAF. The request and declaration must be submitted by the candidates up to September 15, 2020.
                <br />
                <br />
                <strong>Candidates are advised to exercise this option with utmost care</strong>, as option once exercised by the candidate shall not be rolled back under any circumstances. The other terms and conditions for conduct of examination shall be the same as mentioned in the OEAF of candidate.
                <br />
                <br />
                The allocation of the examination center shall however be subject to availability of seats and other feasibility conditions. Although all possible efforts shall be made by NIELIT to allocate examination center as per the preference opted by the candidate, however NIELIT reserves the right to allocate any other examination center.In case of any dispute arising out of the situation the decision of DG NIELIT shall be final and binding.
            </td>
        </tr>

     <tr>
            <td width="100%" colspan="2" class="auto-style9" >
                &nbsp;</td>
        </tr>

     <tr id="tr_consent_row3" runat="server">
            <td width="100%" colspan="2" class="auto-style9" >
                Submit your choice by clicking on the appropriate button :-</td>
        </tr>

     <tr id="tr_consent_row" runat="server" >
                     <td align="left" width="70%" style="text-align:justify; border:1px solid black" class="auto-style10"> 
                         <strong>1.)</strong> I Request for change examination center for the examination commencing from September 29, 2020.
                         <br />
&nbsp;&nbsp;
                    </td>
                    <td  align="left" width="30%" style="border:1px solid black" class="auto-style10">               
            
            <asp:Button ID="btn_center_change" runat="server"  Text="I agree to proceed for center change" Font-Bold="True" Height="25px" Width="275px" OnClick="btn_center_change_Click" />
                    </td>

        </tr>

     <tr id="tr_consent_row1" runat="server">
                     <td align="left" width="70%" style="text-align:justify; border:1px solid black" > 
                         <strong>2.)</strong> To opt out from the examination commencing from September 29, 2020 and select from examination cycle October 2020 or November 2020.</td>
                    <td  align="left" width="30%" style="border:1px solid black">               
            
            <asp:Button ID="btn_opt_out" runat="server" Text="I agree to proceed for opt-out"  Width="275px" Font-Bold="True" Height="25px" OnClick="btn_opt_out_Click" />
                    </td>

        </tr>

     <tr id="tr_consent_row2" runat="server">
                     <td align="left" width="50%" style="text-align:right"> 
                         &nbsp;</td>
                    <td  align="left" width="50%">               
            
                        &nbsp;</td>

        </tr>
        </table>
        </div>

<div id="divReportData" runat="server" style="width:100%;" visible="false">
    
    <table style="width:100%;" id="tbl_full_info" runat="server">   

        <tr>
            <td width="100%" colspan="2" align="center" class="auto-style3" >
                <asp:label id="Lblerror" runat="server" align ="centre" enabletheming="False" CssClass="error" Width="100%" visible="False" style="color: #FF0000; text-align: center"></asp:label>
            </td>

        </tr>
        <tr>
            <td width="40%" >
                    Application No.  
            </td>
            <td >
                 <asp:TextBox ID="appId" runat="server" Width="160px" MaxLength="30" ></asp:TextBox>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;"></span>
            </td>
            <%--<td>&nbsp;</td>--%>

        </tr>
        <tr>
           <td width="40%" class="auto-style5">
                    Date of Birth   
                </td>
            <td class="auto-style5">
                <asp:TextBox MaxLength="30" ID="txtDob" onkeydown="return false" runat="server" SkinID="txtDate" Width="140px"
                                    TabIndex="11" Height="16px"></asp:TextBox>
                 <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                              
                                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>   </td>
        </tr>
        <tr>
            <td class="auto-style5"></td>
            <td class="auto-style5">
                <asp:Button ID="btnSubmit" runat="server"  Text="Submit" style ="width : 100px" OnClick="btnSubmit_Click" OnClientClick="return userValid();"  />
                
            </td>          
        </tr>
        <%--<br />--%>

        <tr id="tr_candidate_name" runat="server" visible="False" class="gdalternate1">
            <td class="auto-style3">Candidate Name</td>
            <td class="auto-style3">
                <asp:label id="lbl_candidate_name" runat="server" align ="centre" style="font-weight: 700" 
        ></asp:label>
                
            </td>          
        </tr>

        <tr id="tr_exam_id" runat="server" visible="False">
            <td class="auto-style3">Exam Applied For</td>
            <td class="auto-style3">
                <asp:label id="lbl_examid" runat="server" align ="centre" style="font-weight: 700" 
        ></asp:label>
                
            </td>          
        </tr>

        <tr id="tr_centre1" runat="server" visible="False" class="gdalternate1">
            <td class="auto-style3">Previous Selected Centre 1</td>
            <td class="auto-style3">
                <asp:label id="lblcentre1" runat="server" align ="centre" style="font-weight: 700" 
        ></asp:label>
                
            </td>          
        </tr>
        <tr id="tr_centre2" runat="server" visible="False">
            <td class="auto-style3">Previous Selected Centre 2</td>
            <td class="auto-style3">
                <asp:label id="lblcentre2" runat="server" align =" centre" style="font-weight: 700" 
        ></asp:label>
                
            </td>          
        </tr>

        <tr>
            <td colspan ="2">
                  <br />      
            </td>
        </tr>
        <tr id="tr_change_centre" runat="server" visible="false">
            <td class="auto-style2">                
                <asp:label id="lbl_message" runat="server" align ="centre" enabletheming="False"></asp:label>
            </td>
            <td class="auto-style2">
            <asp:Button ID="btn_yes" runat="server" Text="Agreed" style ="width : 100px" OnClick="btn_yes_Click"  />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_no" runat="server"  Text="Disagreed" style ="width : 100px" OnClick="btn_no_Click"   />
            </td>
        </tr>
        <tr id="tr_examcycle" runat="server" visible="false">
            <td class="auto-style2">                
                Select Exam Cycle
            </td>
            <td class="auto-style2">
                <asp:DropDownList ID="ddl_examcycle" runat="server" Width="185px" AutoPostBack="True">
                                            <asp:ListItem Value="0">--Select ExamCycle--</asp:ListItem>
                                        </asp:DropDownList>
            </td>
        </tr>

        <tr id="tr_new_loc" runat="server" visible="false">
            <td class="auto-style2">                
                Select                
                New Exam Location
            </td>
            <td class="auto-style2">
                <asp:DropDownList ID="DdlExamCentreState1" runat="server" Width="185px" AutoPostBack="True"
                                             OnSelectedIndexChanged="DdlExamCentreState1_SelectedIndexChanged">
                                            <asp:ListItem Value="0">--Select State--</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DdlExamCentre1" runat="server" Width="185px" AutoPostBack="True">
                                            <asp:ListItem Value="0">--Select Location--</asp:ListItem>
                                        </asp:DropDownList>
            </td>
        </tr>

        <tr id="tr_new_loc0" runat="server" visible="false">
            <td class="auto-style2">                
                &nbsp;</td>
            <td class="auto-style2">
                &nbsp;</td>
        </tr>

        <tr id="tr11" runat="server" visible="false">
        <td colspan="2">
              Declaration
            </td>        
            </tr>

        <tr id="tr12" runat="server" visible="false">        
        <td colspan="2" style="text-align: justify">
            <asp:CheckBox ID="chk" runat="server" OnCheckedChanged="chk_CheckedChanged" TabIndex="42" Text="&lt;font color='RED'&gt;*&lt;/font&gt;" />
            I
                <asp:label id="lbl_candidate_name0" runat="server" align ="centre" style="font-weight: 700" 
        ></asp:label>
                
            , hereby undertake that I have read and understood the above-mentioned information and hereby submit my willingness with respect to examination cycle and the examination center preference submitted as above. I further undertake that I will abide by the decision of NIELIT for my rescheduled examination cycle. </td>
            </tr>

        <tr id="tr_final_save" runat="server" visible="false">
        <td class="auto-style5">
        </td>
        <td class="auto-style5">
            <asp:Button ID="btnSave" runat="server"  Text="Proceed" style ="width : 100px" OnClick="btnSave_Click"  />
        </td>
            </tr>

        <tr id="tr_enter_otp" runat="server" visible="false">
        <td>
            Enter OTP number Sent on registered Mobile No. &amp; Email Id</td>
        <td>
                 <asp:TextBox ID="txt_otp" runat="server" Width="139px" MaxLength="30" ></asp:TextBox>
                 <span class="auto-style6">(Please check junk/spam mail if you not received this OTP mail in your inbox)</span></td>
            </tr>

        <tr id="tr_validate" runat="server" visible="false">
        <td>
            &nbsp;</td>
        <td>
            <asp:Button ID="btn_validate" runat="server"  Text="Validate OTP" style ="width : 100px" OnClick="btn_validate_Click" />
        </td>
            </tr>

        <tr id="tr_validate_otp" runat="server" visible="false">
        <td>
                <asp:label id="lbl_save_option" runat="server" align ="centre" enabletheming="False" visible="False"></asp:label>
            </td>
        <td>
                <asp:label id="lbl_validate_msg" runat="server" align ="centre" enabletheming="False" visible="False" style="color: #FF0000"></asp:label>
        </td>
            </tr>
    </table>
      



</div>
</asp:Content>


