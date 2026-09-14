<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmExamForm_Edit.aspx.cs"
    Inherits="FrmExamForm_Edit" Debug="true" %>

<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
 
<head id="Head1" runat="server">
    <title></title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/Date.js" type="text/javascript"></script>
   
     <script language="javascript" type="text/javascript">
        
     
document.onkeydown = function(e) {
if(event.keyCode == 123) {
return false;
}
if(e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)){
return false;
}
if(e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)){
return false;
}
if(e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)){
return false;
}
}
        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
        function validate() {
            var value12 = document.getElementById("levelNameHidden").value;
           // alert(value12);
            if (value12 == '1') {
                if (!confirm("Ensure your Medium of Exam. It can not be changed later. "))
                    return false;
            }
            else {
                if (!confirm("Are you sure, You want to Proceed/Update"))
                    return false;
            }
            if (!isSelected("ddlStateFirst", "Exam centre location of first choice")) {
                alert("Please select Exam centre location of first choice");
                return false;
            }
            if (!isSelected("DdlExamCentre1", "Exam centre: First choice")) {
                alert("Please select Exam centre: First choice");
                return false;
            }
            if (!isSelected("ddlStateSecond", "Exam centre location of second choice")) {
                alert("Please select Exam centre location of second choice");
                return false;
            }
            if (!isSelected("DdlExamCentre2", "Exam centre: Second choice")) {
                alert("Please select Exam centre: Second choice");
                return false;
            }
            if (!$("#trImprovementOption").is(":visible")) {
                if (parseInt(document.getElementById("selectedModules").value) == 0) {
                    alert("Please select at least one module to apply for exam");
                    return false;
                }
            }
            if (!ischecked("chkdisclamier", "Declaration"))
                return false;
            if (!confirm("Are you sure you want to apply for selected modules!"))
                return false;
            return true;
        }
         function CnfrmationBox() {
             
             var value = document.getElementById("levelNameHidden").value;
             if (value == '3') {
                 if (!confirm("Are you sure you want to avail Exemption for selected modules! After Availing Exemption you will not be allowed to apply through old revision!"))
                     return false;
                 return true;
             } else
             {
                 if (!confirm("Are you sure you want to avail Exemption for selected modules!"))
                     return false;
                 return true;
             }
        }
       
    </script>

    <style type="text/css">
        .auto-style1
        {
            width: 30%;
            height: 27px;
        }
        .auto-style2
        {
            width: 20%;
            height: 27px;
        }
        .auto-style3
        {
            width: 25%;
            height: 27px;
        }
        .auto-style4
        {
            height: 34px;
        }
        .auto-style5
        {
            width: 3%;
            height: 34px;
        }
        .auto-style6 {
            height: 29px;
        }
        .auto-style7 {
            width: 3%;
            height: 29px;
        }
        .auto-style8 {
            width: 3%;
            height: 26px;
        }
        .auto-style9 {
            width: 70%;
            height: 26px;
        }
        .auto-style10 {
            width: 27%;
            height: 26px;
        }
        .auto-style11 {
            width: 10px;
        }
    </style>
     
  
</head>
<body style="background-color: #ffffff">
    <form id="form2" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

       <div id="pageloaddiv"></div>
        <div>
    
       <%-- <asp:Label ID="lblImp" runat="server" EnableTheming="false" CssClass="error" Width="99%" Visible="true"> <strong> Attention – O/A/B/C Level Candidates/Institutes:</strong>  The candidate whose result of July, 2021, O/A/B/C Level Exam is awaited, may also apply in January, 2022 Exam for same papers. If such candidates are found passed on declaring result of July, 2021 Exam., the examination fee, in respect of passed papers, will be refunded. </asp:Label>--%>

            <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
                <tr>
                    <td align="center">
                        <h3>
                            <strong style="text-align: center">
                                <asp:Label ID="lbllevel" runat="server" Text=""></asp:Label>
                                <asp:hiddenfield id="levelNameHidden" runat="server"></asp:hiddenfield>
                                ONLINE EXAMINATION FORM -&nbsp;
                            <asp:Label ID="lblExamName" runat="server"></asp:Label>
                            </strong>
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%" Visible="false" style="font-size:larger;font-weight:bold"> </asp:Label>
                        <asp:LinkButton ID="LnkBtnPrintForm" runat="server" Height="26px" Text="Print Form" Visible="false"
                            Style="vertical-align: top; float: right" />
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <table id="tblMain" runat="server" class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
                            cellspacing="1">
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:Label ID="lblmandatory0" runat="server" Text="<font color='red'>*</font> अनिवार्य फिल्डस (Mandatory fields)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3%; text-align: left; vertical-align: top"></td>
                                <td style="width: 40%; text-align: left; vertical-align: top"></td>
                                <td style="width: 57%; text-align: left; vertical-align: top"></td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">
                                    <asp:Label ID="lblthree" runat="server" Text="1."></asp:Label>
                                    Exam Details
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="1.1"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Name of Candidate"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCandidateName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="1.2"></asp:Label>
                                </td>
                                <td>Registration Number
                                </td>
                                <td>
                                    <asp:Label ID="lblRegNumber" runat="server" Text="NA"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>
                                    <asp:Label ID="lbl3p1" runat="server" Text="1.3"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Name of Exam Applying For"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblExamName1" runat="server" Text="Not Found"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>
                                    <asp:Label ID="lbl4p1" runat="server" Text="1.4"></asp:Label>
                                </td>
                                <td>Previous Appearance (Name of Exam)
                                </td>
                                <td>
                                    <asp:Label ID="lblPreviousExam" runat="server" Text="NA"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="lblMediumrow" runat="server">
                                <td>
                                    <asp:Label ID="lbl3p2" runat="server" Text="1.5"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="Medium of Exam &lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:RadioButtonList ID="rblMedium" Enabled="false"  runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="1">English</asp:ListItem>
                                        <asp:ListItem Value="2">Hindi</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>1.6
                                </td>
                                <td>Candidate Type (Registered As)
                                </td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:Label ID="lblCandidateType1" runat="server" Text="Direct Candidate"></asp:Label>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td>1.7</td>
                                <td>Applying This Exam</td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:DropDownList ID="ddlCandidateType" disabled="true" runat="server" Width="537px"
                                        TabIndex="34" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCandidateType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td>1.8
                                </td>
                                <td>Applicable Examination Fee Will Be Paid To?
                                </td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPaymentOption" disabled="true" runat="server" Width="537px" TabIndex="34">
                                                <asp:ListItem Value="1" Text="NIELIT (Using Available Payment Methods)"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Accredited Centre"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCandidateType" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            
                             <%--Added By Reena --%>
                             <tr class="gdalternate1" id="TrOnlTheoryExamCenter1" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="LblOnlSI1" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblOnlExm1" runat="server" Text="Online Exam Centre: First Choice&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlOnlTheoryExamStateFirst" runat="server" Width="267px" AutoPostBack="True"
                                                TabIndex="32" OnSelectedIndexChanged="ddlOnlTheoryExamStateFirst_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlOnlTheoryExamCentre1" runat="server" Width="267px" AutoPostBack="True"
                                                TabIndex="33" OnSelectedIndexChanged="DdlOnlTheoryExamCentre1_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select Exam Centre--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlOnlTheoryExamCentre2" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrOnlTheoryExamCenter2" runat="server" visible="false">
                                <td class="auto-style6">
                                    <asp:Label ID="LblOnlSI2" runat="server"></asp:Label>
                                </td>
                                <td class="auto-style6">
                                    <asp:Label ID="LblOnlExm2" runat="server" Text="Online Exam Centre: Second Choice&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left" valign="top" width="30%" class="auto-style7">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlOnlTheoryExamStateSecond" runat="server" AutoPostBack="True" Width="267px"
                                                TabIndex="34" OnSelectedIndexChanged="ddlOnlTheoryExamStateSecond_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlOnlTheoryExamCentre2" runat="server" Width="267px"
                                                TabIndex="35">
                                                <asp:ListItem Text="--Select Exam Centre--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlOnlTheoryExamCentre1" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr class="gdalternate1" id="TrPracExamCenter1" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="LblPracSI1" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="LblPrac1" runat="server" Text="Practical Exam Centre: First Choice&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPracStateFirst" runat="server" Width="267px" AutoPostBack="True"
                                                TabIndex="32" OnSelectedIndexChanged="ddlPracStateFirst_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlPracExamCentre1" runat="server" Width="267px" AutoPostBack="True"
                                                TabIndex="33" OnSelectedIndexChanged="DdlPracExamCentre1_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select Exam Centre--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlPracExamCentre2" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrPracExamCenter2" runat="server" visible="false">
                                <td class="auto-style6">
                                    <asp:Label ID="LblPracSI2" runat="server"></asp:Label>
                                </td>
                                <td class="auto-style6">
                                    <asp:Label ID="LblPrac2" runat="server" Text="Practical Exam Centre: Second Choice&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left" valign="top" width="30%" class="auto-style7">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPracStateSecond" runat="server" AutoPostBack="True" Width="267px"
                                                TabIndex="34" OnSelectedIndexChanged="ddlPracStateSecond_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlPracExamCentre2" runat="server" Width="267px"
                                                TabIndex="35">
                                                <asp:ListItem Text="--Select Exam Centre--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlPracExamCentre1" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                           <%-- End Added By Reena --%>
                            <tr class="gdrow1" id="tr_select_module" runat="server" visible="false">
                                <td class="auto-style4"></td>
                                <td class="auto-style4">Select Modules Appearing For :
                                </td>
                                <td align="left" valign="top" width="30%" class="auto-style5" style="color: #FF0000">
                                    <asp:RadioButtonList ID="rbtn_module_selection" runat="server" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rbtn_module_selection_SelectedIndexChanged" AutoPostBack="true" ForeColor="Black">
                                        <asp:ListItem Value="pre"></asp:ListItem>
                                        <asp:ListItem Value="current"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    (Before selecting any module kindly read carefully the course syllabus)
                                    <asp:HyperLink ID="hpl_syllabus" runat="server" ForeColor="Blue" NavigateUrl="https://nielit.gov.in/content/computer-course-0" Target="_blank">Click here to check</asp:HyperLink>
                                </td>
                            </tr>

                            <%-- Start-----------------------for previous revision-------------------%>
                            <tr class="head1" id="trModuleHead" runat="server" visible="true">
                                <td colspan="3">
                                    <asp:Label ID="lblSeven" runat="server" Text="2."></asp:Label>
                                    Modules Appearing For (
                                <asp:Label ID="lblRevision" runat="server" Text="1<sup>th</sup>"></asp:Label>

                                </td>
                            </tr>
   
                            <%--Start added code date 27032023 for Any N Module Exmption option--%>

                            <tr id="trModuleOptionExmpt" runat="server" visible="false">
                                <td colspan="3" style="vertical-align: top; padding: 0;">
                                    <table class="sample3" style="width: 100%; text-align: left;" border="0" cellpadding="3"
                                        cellspacing="1">
                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; " class="auto-style8">
                                                <asp:Label ID="lblserveExpt" runat="server"></asp:Label>
                                            </td>
                                            <td class="auto-style9">
                                              <asp:Label ID="lblCoutMessageExmpt" runat="server" Text="2."></asp:Label> &nbsp; &nbsp;<br/>
                                                <asp:HiddenField ID="lblCoutMessageExmpthidden" runat="server" />
                                                <span style="color:maroon;font-weight:bold;font-size:10px"> Note - Please Check the module for which you want to avail exemption against the module listed in the right panel</span>
                                            </td>
                                            <td class="auto-style10" style="color:red">  <asp:Label ID="Label5" runat="server" Text="Conditional Module List passed by candidate"></asp:Label>&nbsp; &nbsp;<br/>
                                                <span style="color:maroon;font-weight:bold;font-size:10px"> Note - Exemption will be marked in the below order upon the selection</span>
                                            </td>
                                        </tr>

                                        <tr id="trModuleOptionExmpt1" class="gdrow1" runat="server" visible="false">
                                            <td style="vertical-align: top; width: 3%;"></td>
                                            <td style="vertical-align: top;" align="left">
                                                <asp:UpdatePanel ID="UpdatePanelExmpt" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBoxList ID="chklistModulesExmpt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklistModulesExmpt_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chklistModulesExmpt" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="vertical-align: top;" align="left">

                                               <%-- <asp:UpdatePanel ID="UpdatePanelExmpt1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBoxList AutoPostBack="true" ID="chklistPracticalsExmpt" runat="server"
                                                            OnSelectedIndexChanged="chklistPracticalsExmpt_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        
                                                    </Triggers>
                                                </asp:UpdatePanel>--%>

                                                <asp:listbox id="listboxexmpt" style="border-style:none;background-color:#c9d7e2;word-break:break-all" runat="server"></asp:listbox>
                                                
                                            </td>
                                        </tr>
                                        <%-------------------------for previous revision -----------------END--%>
                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; height: 5px;" colspan="3"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%-- End --%>
                            <tr id="trModuleOption" runat="server" visible="false">
                                <td colspan="3" style="vertical-align: top; padding: 0;">
                                    <table class="sample3" style="width: 100%; text-align: left;" border="0" cellpadding="3"
                                        cellspacing="1">
                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; width: 3%;">
                                                <asp:Label ID="lblserve1" runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 70%">
                                                <asp:Label ID="lblCoutMessage" runat="server" Text="2."></asp:Label>
                                            </td>
                                            <td style="width: 27%">Practicals
                                            </td>
                                        </tr>

                                        <tr id="trModuleOption1" class="gdrow1" runat="server" visible="false">
                                            <td style="vertical-align: top; width: 3%;"></td>
                                            <td style="vertical-align: top;" align="left">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBoxList ID="chklistModules" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklistModules_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chklistPracticals" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="vertical-align: top;" align="left">
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBoxList AutoPostBack="true" ID="chklistPracticals" runat="server"
                                                            OnSelectedIndexChanged="chklistPracticals_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chklistModules" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <%-------------------------for previous revision -----------------END--%>
                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; height: 5px;" colspan="3"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="head1">
                                <td colspan="3">
                                    <asp:Label ID="lblsix" runat="server" Text="3."></asp:Label>
                                    Exam Center City Choice (These choice will be applicable to all theory and practical modules)</td>
                            </tr>
                            <tr class="gdalternate1" id="TrExamCentre1" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="LblSiOff1" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="Exam Centre: First Choice&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlStateFirst" runat="server" Width="267px" AutoPostBack="True"
                                                TabIndex="32" OnSelectedIndexChanged="ddlStateFirst_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlExamCentre1" runat="server" Width="267px" AutoPostBack="True"
                                                TabIndex="33" OnSelectedIndexChanged="DdlExamCentre1_SelectedIndexChanged1">
                                                <asp:ListItem Text="--Select Exam Centre--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlExamCentre2" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="gdrow1" id="TrExamCentre2" runat="server" visible="false">
                                <td class="auto-style6">
                                    <asp:Label ID="LblSiOff2" runat="server"></asp:Label>
                                </td>
                                <td class="auto-style6">
                                    <asp:Label ID="Label13" runat="server" Text="Exam Centre: Second Choice&lt;font color='RED'&gt;*&lt;/font&gt;"></asp:Label>
                                </td>
                                <td align="left" valign="top" width="30%" class="auto-style7">
                                    <asp:UpdatePanel ID="Update" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlStateSecond" runat="server" AutoPostBack="True" Width="267px"
                                                TabIndex="34" OnSelectedIndexChanged="ddlStateSecond_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DdlExamCentre2" runat="server" Width="267px"
                                                TabIndex="35">
                                                <asp:ListItem Text="--Select Exam Centre--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DdlExamCentre1" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr class="head1" id="trImprovementHead">
                                <td colspan="3">
                                    <asp:Label ID="lblFive" runat="server" Text="4"></asp:Label>
                                    Improvement (One module only)
                                </td>
                            </tr>
                            <tr class="gdalternate1" valign="top" id="trImprovementOption" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="lbl5p2" runat="server" Text="4.1"></asp:Label>
                                </td>
                                <td> Module Name for Improvement
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlModules" AutoPostBack="true" runat="server" Width="537px"
                                        TabIndex="34" OnSelectedIndexChanged="ddlModules_SelectedIndexChanged">
                                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="FeedtlsId" class="head1">
                                <td colspan="3">
                                    <asp:Label ID="Label7" runat="server" Text="5."></asp:Label>
                                    Fee Details
                                </td>
                            </tr>
                            <tr id="panelfeedetails" class="gdalternate1" valign="top"  visible="true">
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="5.1"></asp:Label>
                                </td>
                                <td colspan="2" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="selectedModules" runat="server" Value="0" />

                                            <table class="sample3" style="width: 100%; text-align: left;" border="1" cellpadding="3"
                                                cellspacing="0">
                                                <tr style="font-weight: bold; text-align: center" class="gdrow1">
                                                    <td style="vertical-align: top;" class="auto-style1">Module Type
                                                    </td>
                                                    <td class="auto-style2">Modules Selected
                                                    </td>
                                                    <td class="auto-style3">Fee/Module (Rs.)
                                                    </td>
                                                    <td class="auto-style3">Total Fee Amount (Rs.)
                                                    </td>
                                                </tr>
                                                <tr class="gdalternate1">
                                                    <td style="vertical-align: top; width: 30%;">(a) Theory Modules
                                                    </td>
                                                    <td style="width: 20%; text-align: center">
                                                        <asp:Label ID="lblTheoryCount" runat="server" Text="0"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblTheoryFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblTotalTheoryFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="gdalternate1">
                                                    <td style="vertical-align: top; width: 30%;">(b) Practical Modules
                                                    </td>
                                                    <td style="width: 20%; text-align: center">
                                                        <asp:Label ID="lblPracticalCount" runat="server" Text="0"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblPracticalFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblTotalPracticalFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="gdalternate1">
                                                    <td style="vertical-align: top; width: 30%;">(c) Exam Form Processing Fee</td>
                                                    <td style="width: 20%; text-align: center">(Per Exam Form)
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblProcessingFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblTotalProcessingFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="gdalternate1">
                                                    <td style="vertical-align: top; width: 30%;">(d) Late Fee
                                                    </td>
                                                    <td style="width: 20%; text-align: center">(Per Exam Form)
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblLateFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblTotalLateFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="gdalternate1">
                                                    <td style="vertical-align: top; width: 30%;">&nbsp;&nbsp;&nbsp;&nbsp; Total Fees (a + b + c + d)
                                                    </td>
                                                    <td colspan="2" style="text-align: right">
                                                        <asp:Label ID="lblAmountHindi" runat="server" Text="Zero Rupees Only"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%; text-align: right">
                                                        <asp:Label ID="lblTotalFee" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chklistModules" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chklistPracticals" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlModules" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="auto-style11"></td>
                            </tr>

                            <tr id="disclamrdivid" class="head1">
                                <td colspan="3">
                                    <asp:Label ID="lblDeclarationNumber" runat="server" Text="5."></asp:Label>
                                    Declaration / घोषणा
                                </td>
                            </tr>
                            <tr id="disclmrPanel" class="gdrow1">
                                <td runat="server" style="text-align: justify; padding-right: 20px; padding-left: 20px;" colspan="3">
                                    <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="41" Text="<font color='RED'>*</font>" />
                                    I,
                                    <asp:Label ID="lblName" Font-Bold="true" runat="server" Text=""></asp:Label>
                                    <span id="header1" runat="server">registered
                                        <asp:Label ID="lblCandidateType" Font-Bold="true" Text="NA" runat="server"></asp:Label>
                                        hereby declare that, all the particular stated in the application,
                                are true to the best of my knowledge and belief. 
                                I hereby certify that I have applied the aforesaid checks before submitting the Online Examination Form.
                                I have read and understood all the instructions available on the site.
                                I agree to abide by the rules and regulations of the NIELIT and also to the decision of the Examination Authority, on any issue related to
                                my admission to the Examination. I have noted that the Examination Authority has
                                the right to withhold my result ever after appearing in the Examination in addition
                                to any other action as may be deemed fit in the event of any of the statements made
                                above being found incorrect or my candidature being found ineligible at a later date.
                                I have specially gone through the eligibility criteria laid down by NIELIT for appearing in different examinations and 
                                I confirm that I fullfill the eligibility for the Theory & Practical modules, I have applied for.</span>
                                    <span id="header2" runat="server">, have read and understood the relevant rules/instructions etc. of the NIELIT and I undertake to abide by the
                                same in all respects. I solemnly declare that the particulars filled in by me are correct and nothing has been 
                                concealed. In case of any discrepancy found therein,I shall be responsible for the consequences.I further understand 
                                that that I may have to appear for the examination at an examination centre not opted by me in my examination
                                application form. In this regard, I will abide by the decision of NIELIT. 
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                 </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <br />
                            <%--<asp:button id="btnExmpSubmit" runat="server" text="Submit Exemption" OnClientClick="return CnfrmationBox();" Visible="false" OnClick="btnExmpSubmit_Click"/>--%>
                            <asp:Button ID="btnSave" runat="server" Text="Proceed" TabIndex="42" OnClick="btnSave_Click"
                                OnClientClick="return validate();" />
                           <%-- <input runat="server" type="button" id="btnback" onclick="window.history.back();" value="Back" class="btnNormal" />--%>
                        </td>
                    </tr>
               
            </table>
        </div>
    </form>
</body>
</html>
