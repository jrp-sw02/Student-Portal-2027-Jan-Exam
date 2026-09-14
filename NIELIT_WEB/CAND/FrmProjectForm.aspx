<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmProjectForm.aspx.cs" Inherits="CAND_FrmProjectForm" Debug="true" %>

<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        document.onkeydown = function (e) {
            if (event.keyCode == 123) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
                return false;
            }
            if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
            }
            function showForm(url) {
                window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
                return false;
            }
            function validate() {
                if (!ischecked("chkdisclamier", "Declaration"))
                    return false;            
                if (parseInt(document.getElementById("selectedModules").value) == 0) {
                    alert("Please select at least one Project Module to apply.");                
                    return false;
                    if (!confirm("Are you sure you want to apply for selected Project Module. Kindly make sure that your project is ready to dispatch to NIELIT within 3 months...!"))
                        return false;
                }            
                return true;
            }
    </script>
    <style type="text/css">
        .auto-style3
        {
            width: 25%;
            height: 27px;
        }
        .auto-style5
        {
            width: 40%;
        }
        .auto-style8
        {
            width: 16%;
        }
        .auto-style9
        {
            width: 16%;
            height: 27px;
        }
        .auto-style11
        {
            width: 43%;
        }
        .WrapText
        {
            width: 100%;
            word-break: break-all;
        }
        .auto-style12
        {
            height: 25px;
        }
        .auto-style13
        {
            width: 40%;
            height: 25px;
        }
        </style>
</head>
<body style="background-color: #ffffff" oncontextmenu="return false;">
    <form id="form2" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="WrapText">
            <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
                <tr>
                    <td align="center">
                        <h3>
                            <strong style="text-align: center">
                                <asp:Label ID="lbllevel" runat="server" Text=""></asp:Label>
                                ONLINE PROJECT FORM 
                            </strong>
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%" Visible="false"> </asp:Label>
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
                                <td style="text-align: left; vertical-align: top" class="auto-style5"></td>
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
                                <td class="auto-style5">
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
                                <td class="auto-style5">Registration Number
                                </td>
                                <td>
                                    <asp:Label ID="lblRegNumber" runat="server" Text="NA"></asp:Label>
                                </td>
                            </tr>
                           
                            <tr class="gdrow1">
                                <td class="auto-style12">
                                    <asp:Label ID="lbl4p1" runat="server" Text="1.3"></asp:Label>
                                </td>
                                <td class="auto-style13">Previous Applied Project Details
                                </td>
                                <td align="left" id="tdformstatus" runat="server" class="auto-style12" >
                                    <%--<asp:Label ID="lblPreviousExam" runat="server" Text="NA"></asp:Label>--%>
                                </td>
                            </tr>                            
                            <tr class="gdrow1">
                                <td>1.4
                                </td>
                                <td class="auto-style5">Candidate Type (Registered As)
                                </td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:Label ID="lblCandidateType1" runat="server" Text="Direct Candidate"></asp:Label>
                                </td>
                            </tr>   
                            
                             <tr class="gdalternate1" id="chmExam" runat="server"  visible="false">
                                <td>1.5</td>
                                <td>Applying This Exam</td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:DropDownList ID="ddlCandidateType" enabled ="true"  runat="server" Width="537px"
                                        TabIndex="34" AutoPostBack="True" >                                                                             
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr class="gdrow1" id="chmPay" runat="server"    visible="false">
                                <td>1.6
                                </td>
                                <td>Applicable Examination Fee Will Be Paid To?
                                </td>
                                <td style="padding-left: 0px;" align="left">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPaymentOption" runat="server" Width="537px" TabIndex="34" Enabled="true">
                                               
                                                <asp:ListItem Value="2" Text="Accredited Centre"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCandidateType" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                                                
                            
                            <%--<tr class="head1" id="trModuleHead" runat="server" visible="true">
                                <td colspan="3" class="auto-style4">
                                    <asp:Label ID="lblSeven" runat="server" Text="2."></asp:Label>
                                    Project Appearing For (
                                <asp:Label ID="lblRevision" runat="server" Text="1<sup>th</sup>"></asp:Label>

                                </td>
                            </tr>--%> 
                            
                            <%--<tr id="tr1" runat="server" visible="true" class="gdalternate1">
                                <td colspan="2" width="30%">
                                    <asp:Label ID="Label5" runat="server" Text="2.1 "></asp:Label>Project Modules Passed  
                                </td>
                                <td width="70%">
                                     
                                </td>
                            </tr>   
                                                      
                            <tr id="tr2" runat="server" visible="true" align="left" class="gdrow1">
                                <td colspan="3">
                                    <asp:GridView ID="GridView_Passedproject" runat="server" AutoGenerateColumns="False"  
                                        EmptyDataText="No Project Modules has been passed." DataKeyNames="ID" width="100%" OnRowDataBound="GridView_Passedproject_RowDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="#"></asp:BoundField>
                                            <asp:BoundField DataField="code" HeaderText="Code"></asp:BoundField>
                                            <asp:BoundField DataField="name" HeaderText="Module Name"></asp:BoundField>
                                            <asp:BoundField DataField="Result" HeaderText="Result"></asp:BoundField>
                                            <asp:BoundField DataField="Grade" HeaderText="Grade"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>

                                </td>
                            </tr>   --%>                              
                            <%--<tr class="head1" id="tr3" runat="server" visible="true">
                                <td colspan="3">
                                    <asp:Label ID="Label7" runat="server" Text="2.2 "></asp:Label>
                                    List of Project Modules Remaining to Pass                        

                                </td>
                            </tr> --%>
                                                    
                            <%--<tr id="trModuleOption" runat="server" visible="true">
                                <td colspan="3" style="vertical-align: top; padding: 0;">
                                    <table class="sample3" style="width: 100%; text-align: left;" border="0" cellpadding="3"
                                        cellspacing="1">
                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; width: 3%;">
                                                <asp:Label ID="lblserve1" runat="server" Text="2.3"></asp:Label>
                                            </td>
                                            <td style="width: 70%">

                                                <asp:Label ID="lblCoutMessage" runat="server" Text=""></asp:Label>
                                            </td>
                                            
                                        </tr>

                                        <tr id="trModuleOption1" class="gdrow1" runat="server" visible="true">
                                            <td style="vertical-align: top; width: 3%;"></td>
                                            <td style="vertical-align: top;" align="left">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                <asp:CheckBoxList ID="chklistProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklistProject_SelectedIndexChanged" >
                                                </asp:CheckBoxList>
                                                 </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chklistProject" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            </td>                                            
                                        </tr>                                      


                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; height: 5px;" colspan="3"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>   --%>
                                                    
                            <tr class="head1">
                                <td colspan="3">
                                    <asp:Label ID="Label1" runat="server" Text="2."></asp:Label>
                                    Fee Details<font color='RED'>*</font>
                                </td>
                            </tr>
                            <tr class="gdalternate1" valign="top">
                                <td >
                                    <asp:Label ID="Label8" runat="server" Text="2.1"></asp:Label>
                                </td>
                                <td colspan="3" valign="top">
                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                     <ContentTemplate>
                                    <asp:GridView ID="GridView_project" runat="server" AutoGenerateColumns="False" AutoPostBack="true"  
                                        EmptyDataText="NO project." DataKeyNames="ID" width="100%" OnRowDataBound="GridView_project_RowDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="#" ItemStyle-Width="10px" HeaderStyle-Width="10px"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Select Project Module">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_selection" runat="server" AutoPostBack="true" OnCheckedChanged="cb_selection_CheckedChanged"   />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <HeaderStyle Width="100px" />
                
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Project Module Name" DataField="name" ItemStyle-Width="120px" HeaderStyle-Width="120px"></asp:BoundField>
                                            <asp:BoundField HeaderText="Fee/Module (Rs.)" DataField="fees" ItemStyle-Width="100px" HeaderStyle-Width="100px"></asp:BoundField>                                            
                                            <asp:TemplateField HeaderText="Project Title">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProjectTitle" runat="server" Width="350px" Visible="true" style="text-align: left"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                                <HeaderStyle Width="150px" />
                                            </asp:TemplateField>                                           

                                        </Columns>
                                    </asp:GridView>
                                         </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="GridView_project" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                </td>
                            </tr>

                           <%--<tr class="gdalternate1">
                                            <td  colspan="2" style="vertical-align: top; text-align:right" class="auto-style6">
                                                &nbsp;&nbsp;&nbsp;&nbsp; Total Fees (a)
                                            </td>
                                            <td style="text-align: right;"  class="auto-style7">
                                                <asp:Label ID="Label10" runat="server" Text="0.00"></asp:Label>
                                                (<asp:Label ID="Label9" runat="server" Text="Zero Rupees Only"></asp:Label>)
                                            </td>                                            
                                        </tr>--%>

                            <tr class="gdalternate1" valign="top">
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="2.2"></asp:Label>
                                </td>
                                <td colspan="2" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                    
                                    <table class="sample3" style="width: 100%; text-align: left;" border="1" cellpadding="3"
                                        cellspacing="0">
                                        <tr style="font-weight: bold; text-align: center" class="gdrow1">
                                            <td style="vertical-align: top; " class="auto-style9">Module Type
                                            </td>
                                            <td class="auto-style9">Modules Selected
                                            </td>
                                            <td class="auto-style3" colspan="2">Total Fee Amount (Rs.)
                                            </td>
                                            <%--<td class="auto-style3">Total Fee Amount (Rs.)
                                            </td>--%>
                                        </tr>

                                        <tr class="gdalternate1">
                                            <td style="vertical-align: top; " class="auto-style8">Project Modules
                                            </td>
                                            <td style="text-align: center" class="auto-style8">
                                                <asp:Label ID="lblProjectCount" runat="server" Text="0"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="auto-style11" >
                                                <%--<asp:Label ID="lblProjectFee" runat="server" Text="0.00"></asp:Label>--%>
                                                <asp:Label ID="lblAmountHindi" runat="server" Text="Zero Rupees Only"></asp:Label>
                                            </td>
                                            <td style="width: 20%; text-align: right">
                                                <%--<asp:Label ID="lblTotalProjectFee" runat="server" Text="0.00"></asp:Label>--%>
                                                <asp:Label ID="lblTotalFee" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>

                                        <%--<tr class="gdalternate1">
                                            <td style="vertical-align: top; width: 30%;">&nbsp;&nbsp;&nbsp;&nbsp; Total Fees (a) </td>
                                            <td style="text-align: right" colspan="2">
                                                <asp:Label ID="lblAmountHindi" runat="server" Text="Zero Rupees Only"></asp:Label>
                                            </td>
                                            <td style="width: 25%; text-align: right">
                                                <%--<asp:Label ID="lblTotalFee" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>--%>
                                    </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="GridView_project" EventName="SelectedIndexChanged" />                                       
                                    </Triggers>
                                </asp:UpdatePanel>
                                </td>
                                <%--<td></td>--%>
                            </tr>

                            <tr class="head1">
                                <td colspan="3">
                                    <asp:Label ID="lblDeclarationNumber" runat="server" Text="3."></asp:Label>
                                    Declaration / घोषणा
                                </td>
                            </tr>
                            <tr class="gdrow1">
                                <td id="Td1" runat="server" style="text-align: justify; padding-right: 20px; padding-left: 20px;" colspan="3">
                                    <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="41" Text="<font color='RED'>*</font>" />
                                    I,
                                    <asp:Label ID="lblName" Font-Bold="true" runat="server" Text=""></asp:Label>
                                    <span id="header1" runat="server">registered
                                        <asp:Label ID="lblCandidateType" Font-Bold="true" Text="NA" runat="server"></asp:Label>
                                        hereby declare that, all the particular stated in the application, are true to the best of my knowledge and belief. I hereby certify that I have applied the aforesaid checks before submitting the online Project Form. I have read and understood all the instructions available on the site. I agree to abide by the rules and regulations of the NIELIT and also to the decision of the Examination Authority, on any issue related to my admission to the Examination. I have noted that the Examination Authority has the right to withhold my result ever after appearing in the Project in addition to any other action as may be deemed fit in the event of any of the statements made above being found incorrect or my candidature being found ineligible at a later date. I have specially gone through the eligibility criteria laid down by NIELIT for appearing in different projects and 
                                I confirm that I fullfill the eligibility for the Project modules, I have applied for.</span>
                                    <span id="header2" runat="server">I, have read and understood the relevant rules/instructions etc. of the NIELIT and I undertake to abide by the same in all respects. I solemnly declare that the particulars filled in by me are correct and nothing has been concealed. In case of any discrepancy found therein,I shall be responsible for the consequences. 
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <tr>
                        <td align="center" colspan="3">
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Proceed" TabIndex="42" OnClick="btnSave_Click"
                                OnClientClick="return validate();" />
                            <input runat="server" type="button" id="btnback" onclick="window.history.back();" value="Back" class="btnNormal" />
                            <asp:Button ID="btnpayment" runat="server" Text="Pay Fee" TabIndex="43" OnClick="btnpayment_Click" Visible="false"
                                OnClientClick="return validate();" />
                        </td>
                    </tr>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
