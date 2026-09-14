<%@ Page Title="Course Registration Form" Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="FrmPreview.aspx.cs"
    Inherits="FrmPreview" Debug="True" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .DivImage
        {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 168px;
        }
        .InnerTable
        {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 100%;
            height: 100%;
        }
        .PhotoImage
        {
            position: relative;
            right: 3px;
            top: 5px;
            z-index: -1;
        }
        .BarCodeImage
        {
            position: relative;
            right: 5px;
            top: -1px;
            z-index: 0;
        }
    </style>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" id="Tblpreview"
        runat="server">
        <tr>
            <td>
                <uc1:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="Lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr style="height: 45px;">
            <td align="center" style="border-bottom: 1px solid #000000;" valign="middle">
                <asp:Label ID="Lblhead" runat="server" Style="font-size: 22px;"></asp:Label>
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 10px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        
        <tr>
            <td align="center">
                <table style="width: 100%;" class="preview" cellpadding="1" cellspacing="0">
                    <tr class="normal" style="font: bold 18px arial;">
                        <td align="center" width="25%">
                            Application Number
                        </td>
                        <td align="center" width="25%">
                            Application Date &amp; Time
                        </td>
                        <td align="center">
                            For Office Use Only&nbsp;
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-right: 1px solid #000000;" align="center"
                            valign="middle" rowspan="2" width="18%">
                            <div class="DivImage">
                                <%-- <table>
                                <tr>
                                    <td style="border-bottom: 0px; border-left: 0px; border-top: 0px;" align="center">--%>
                                <%-- <img src="../images/photo.jpg" id="ImgApplicantPhoto" style="height: 111px; width: 113px"  runat="server" CssClass="PhotoImage"/>--%>
                                <asp:Image ID="ImgApplicantPhoto" CssClass="PhotoImage" ImageUrl="../images/photo.jpg"
                                    runat="server" Height="100px" Width="97px" />
                                <%-- </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: 0px; border-left: 0px; border-top: 0px;" align="center">--%>
                                <img id="imgPhotoBarcode" runat="server" class="BarCodeImage" />
                                <%-- </td>
                                </tr>
                            </table>--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblAppNumber" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblAppDataTime" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            valign="middle">
                            <table cellpadding="0" cellspacing="0" style="height: 100%;" width="100%">
                                <tr id="TrDemandnote" runat="server">
                                    <td align="left" style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 1px solid #000000;"
                                        valign="middle">
                                        &nbsp;
                                        <asp:Label ID="Label95" runat="server" Text="Demand Note Number"></asp:Label>
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; border-left: 0px; border-right: 0px;
                                        width: 50%" align="left">
                                        &nbsp;
                                        <asp:Label ID="LblDemandNoteID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="TrDemandnote1" runat="server">
                                    <td align="left" style="border-bottom: 1px solid #000000; width: 50%; border-left: 0px;
                                        border-right: 1px solid #000000;" valign="middle">
                                        &nbsp;
                                        <asp:Label ID="Label102" runat="server" Text="Demand Note Date"></asp:Label>
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; width: 50%;" align="left">
                                        &nbsp;<asp:Label ID="LblDemandNoteDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: 1px solid #000000; width: 50%; border-left: 0px;
                                        border-right: 1px solid #000000;" valign="middle">
                                        &nbsp; Registration No.
                                    </td>
                                    <td style="border-bottom: 1px solid #000000; width: 50%;" align="left">
                                        &nbsp;<asp:Label ID="lblRegistrationNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: 0px; border-left: 0px; border-right: 1px solid #000000;"
                                        valign="middle">
                                        &nbsp;&nbsp;Batch No.
                                    </td>
                                    <td style="border-bottom: 0px; width: 50%;" align="left">
                                        &nbsp;<asp:Label ID="lblBatchNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td align="left" valign="top">
                <table style="width: 100%;" class="preview" border="0" cellspacing="0" cellpadding="2">
                    <tr class="head1">
                        <td align="left" width="35%">
                            1. Registration Details / पंजीयन का विवरण
                        </td>
                        <td align="left" width="15%">
                        </td>
                        <td align="left" width="25%">
                            &nbsp;
                        </td>
                        <td align="left" width="25%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Registration for Course / पाठ्यक्रम के लिए पंजीयन"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td rowspan="2" id="Tdundergoing" runat="server">
                            <asp:Label ID="Label89" runat="server" Text="Applied As / किसके रूप में आवेदन किया"></asp:Label>
                        </td>
                        <td rowspan="2" id="Tdpreundergoing" runat="server">
                            <asp:Label ID="Lblundergoing" Text="" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblAcc" runat="server" Text=" Accreditation no. of the institute / प्रत्यायन संख्या"></asp:Label>
                            <asp:Label ID="LblExp" runat="server" Text="Experience in years / वर्षों का अनुभव"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblAccNo" Text="" runat="server"></asp:Label><br />
                            <asp:Label ID="LblExperience" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrLastCenterInstiName" runat="server">
                        <td>
                            <asp:Label ID="Label62" runat="server" Text="Name of the institute / संस्थान का नाम"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblInstitute" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPreRegLevel2" runat="server">
                        <td rowspan="2" id="TdPreRegLevel2" runat="server">
                            <asp:Label ID="Label58" runat="server" Text="Whether already registered with DOEACC. / क्या पहले से ही डीओईएसीसी के साथ पंजीकृत है"></asp:Label>
                        </td>
                        <td rowspan="2" id="Tdpreregister" runat="server">
                            <asp:Label ID="Lblpreregister" Text="" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Lblprecourse" runat="server" Text=" Course /  पाठ्यक्रम"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPreDoeaccCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPreRegLevel1" runat="server">
                        <td>
                            <asp:Label ID="Label68" runat="server" Text="Registration Number / पंजीयन संख्या"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPreRegno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            2.
                            <asp:Label ID="Label69" runat="server" Text="Applicant's Details / आवेदक का विवरण"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td class="rightBorder">
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label70" runat="server" Text="Applicant's full name / आवेदक का पूरा नाम"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblAppName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrFatherName" runat="server">
                        <td>
                            <asp:Label ID="Label26" runat="server" Text="Father's Name / पिता का नाम "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblFName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrMotherName" runat="server">
                        <td>
                            <asp:Label ID="Label27" runat="server" Text="Mother's Name / माता का नाम "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblMName" runat="server"></asp:Label>
                        </td>
                    </tr>
 <%--Added_UP_Project_New_01_01_2025_Start Amit Added --class="auto-style2"--auto-style1--%>

                        <tr class="normal" id="TrProject" runat="server" Visible="true">
                            <td >
                                <asp:Label ID="lblProjectId" runat="server" Text="Project" Visible="false"></asp:Label>
                            </td>
                            <td  colspan="1">
                                <asp:Label ID="lblProjectIdContent" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td >
                                <asp:Label ID="lblField1" runat="server" Text="Field 1" Visible="false"></asp:Label>
                            </td>
                            <td colspan="1" class="rightBorder">
                                <asp:Label ID="lblField1Content" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrField23" runat="server" Visible="true">
                            <td>
                                <asp:Label ID="lblField2" runat="server" Text="Field 2" Visible="false"></asp:Label>
                            </td>
                            <td colspan="1">
                                <asp:Label ID="lblField2Content" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblField3" runat="server" Text="Field 3" Visible="false"></asp:Label>
                            </td>
                            <td colspan="1" class="rightBorder">
                                <asp:Label ID="lblField3Content" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal" id="TrField45" runat="server" Visible="true">
                            <td>
                                <asp:Label ID="lblField4" runat="server" Text="Field 4" Visible="false"></asp:Label>
                            </td>
                            <td colspan="1">
                                <asp:Label ID="lblField4Content" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblField5" runat="server" Text="Field 5" Visible="false"></asp:Label>
                            </td>
                            <td colspan="1" class="rightBorder">
                                <asp:Label ID="lblField5Content" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>

                        <%--Added_UP_Project_New_01_01_2025_End Amit added--%>
                    <tr class="normal" id="TrGardianName" runat="server">
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Guardian's Name / संरक्षक का नाम "></asp:Label>
                        </td>
                        <td class="rightBorder" >
                            <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
                        </td>
                  
                  
                       <%-- Added 17 Mar 2019 --%>
                          <td class="rightBorder">
                                <asp:Label ID="Label3" runat="server" Text="Affdavit No. / शपथ पत्र संख्या ">
                                </asp:Label>
                            </td>
                        <td class="rightBorder">
                            <asp:Label ID="lblAffidavitNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrAffidavit" runat="server" >
                        <td>
                                <asp:Label ID="Label8" runat="server" Text="Affidavit Date / शपथ पत्र दिनांक">
                                </asp:Label>
                                
                            </td>
                        <td class="rightBorder" colspan ="3">
                            <asp:Label ID="lblAffidavitDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <%-- Till here --%>

                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Gender / लिंग"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblGender" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Marital Status / वैवाहिक स्थिति"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblMaritalStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Date of Birth / जन्म दिनांक  (dd/mm/yyyy)"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblDob" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Category / वर्ग"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblCategory" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label71" runat="server" Text="Handicapped / विकलांग"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblHandicapped" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label72" runat="server" Text="Ex-Serviceman / पूर्व सेवाकर्मी"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblExService" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label73" runat="server" Text="Religion / धर्म"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblReligion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            3. Contact Details / संपर्क विवरण
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label18" runat="server" Text="Phone with STD code / दूरभाष एस टी डी कोड सहित "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblLandLine" runat="server"></asp:Label>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="Label40" runat="server" Text="Mobile Number / मोबाइल नंबर"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="Email Address / ईमेल पता "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblEmail" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            4. Permanent Address Details / स्थायी पता विवरण
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label83" runat="server" Text="Address Lin1/पता पंक्ति 1 "></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblPerAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td class="normal">
                            <asp:Label ID="Label96" runat="server" Text="Address Line2/पता पंक्ति 2 "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblPerAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label85" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblPerAddressLine3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label97" runat="server" Text="City Name/शहर का नाम"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPerCity" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label78" runat="server" Text="District / जिला  "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPerDistrict" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label79" runat="server" Text="State / राज्य "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblPerState" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Pin Code / पिन  कोड  "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPerPinCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            5.
                            <asp:Label ID="Label74" runat="server" Text="Correspondence Details / पत्राचार की सूचना"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label98" runat="server" Text="Address Lin1/पता पंक्ति 1 "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblCorAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label99" runat="server" Text="Address Line2/पता पंक्ति 2 "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblCorAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label100" runat="server" Text="Address Line3/पता पंक्ति 3"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblCorAddressLine3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label101" runat="server" Text="City Name/शहर का नाम"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCorCity" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label34" runat="server" Text="District / जिला  "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblDistrict" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label33" runat="server" Text="State / राज्य "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblState" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label39" runat="server" Text="Pin Code / पिन  कोड  "></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPincode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            6. Educational / Qualification Details / शैक्षिक / योग्यता का विवरण
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr id="Tr1" class="normal" runat="server">
                        <td valign="top" rowspan="3" id="Tdqualified" runat="server">
                            <asp:Label ID="Label84" runat="server" Text="DOEACC Qualification / डीओईएसीसी योग्यता"></asp:Label>
                        </td>
                        <td align="left" rowspan="3" valign="top" id="Tdlblqualified" runat="server">
                            <asp:Label ID="Lblqualified" Text="" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="Lbldcourse" runat="server" Text=" Course /  पाठ्यक्रम"></asp:Label>
                        </td>
                        <td align="left" class="rightBorder">
                            <asp:Label ID="LblDoeaccCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Trregno">
                        <td align="left">
                            <asp:Label ID="Label64" runat="server" Text="Registration Number / पंजीयन संख्या"></asp:Label>
                        </td>
                        <td align="left" class="rightBorder">
                            <asp:Label ID="LblDoeaccRegno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="Tryp">
                        <td align="left">
                            <asp:Label ID="Label65" runat="server" Text="Year of  Passing / उत्तीर्ण वर्ष"></asp:Label>
                        </td>
                        <td align="left" class="rightBorder">
                            <asp:Label ID="LblDoeaccYopass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr2" class="normal" runat="server">
                        <td>
                            <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification / उच्चतम शैक्षिक योग्यता"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblHeighEducation" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr3" class="normal" runat="server">
                        <td>
                            <asp:Label ID="Label82" runat="server" Text="Year of Passing / उत्तीर्ण वर्ष"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblHeighestEduYrofPass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr4" class="head1" runat="server" valign="top">
                        <td>
                            7. Identification Detail
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label30" runat="server" Text="Visible Distinguishing Mark / स्पष्ट पहचान चिन्ह"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblBodyMark" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Labeladh" runat="server" text="Aadhar Card Number / आधार कार्ड संख्या">
                            </asp:label>
                        </td>
                        <td class="rightBorder">
                            <asp:label id="lblaadhar" runat="server"></asp:label>
                        </td>
			 <td>
                            <asp:label id="Labelapaar" runat="server" text="Apaar ID / अपार आईडी">
                            </asp:label>
                        </td>
                        <td class="rightBorder" >
                            <asp:label id="lblApaar" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="head1" id="TrPaymentDetail" runat="server">
                        <td align="left" valign="top" colspan="4">
                            8. Payment Detail
                        </td>
                    </tr>
                    <tr class="normal" id="TrPaymentMode" runat="server">
                        <td align="left" valign="top">
                            Payment Mode:
                        </td>
                        <td>
                            <asp:Label ID="LblPaymentMode" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblFeeType" runat="server"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblFeeAmount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPaymentModeInstruction" runat="server">
                        <td align="left" valign="top" colspan="4" class="rightBorder">
                            <asp:Label ID="LblPaymentDescription" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPaymentModeTransactionNo" runat="server">
                        <td align="left" valign="top" colspan="3">
                            Please enter the received Transaction no.<asp:Label ID="LblPaymentSrc" runat="server"></asp:Label>
                            प्राप्त ट्रांजेक्शन संख्या दर्ज करें
                        </td>
                        <td class="rightBorder">
                             Transaction No.
                             <asp:Label ID="lblTransactionNumber" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrPayment_not" runat="server">
                        <td align="left" valign="top" colspan="4" class="rightBorder">
                            &nbsp;<strong>Note</strong> *&nbsp; <%--Please don&#39;t send this form without making
                            payment./कृपया इस फार्म को भुगतान के बिना नहीं भेजें--%>
                            Please pay the registration fee online after online submission of registration application form/पंजीकरण आवेदन फार्म ऑनलाइन जमा करने के बाद ऑनलाइन पंजीकरण शुल्क का भुगतान करें
                        </td>
                    </tr>
                    <tr class="normal" id="TrInstitute" runat="server">
                        <td align="left" valign="top" colspan="4" class="rightBorder" id="TdInstituteInfo"
                            runat="server" style="padding-left: 6px;">
                        </td>
                    </tr>
                    <%--<tr class="head1">
                        <td colspan="2">
                            9.Enclosures / भेजें
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td colspan="4" class="rightBorder">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                            Attested copies of educational qualifications of the candidate (अभ्यर्थी की शैक्षिक
                            योग्यता प्रमाणपत्र की सत्यापित प्रतिलिपि )
                        </td>
                    </tr>
                    <tr class="normal">
                        <td colspan="4" class="rightBorder">
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                            Attested copy of age proof certificate of the candidate ( अभ्यर्थी के आयु प्रमाणपत्र
                            की सत्यापित प्रतिलिपि )
                        </td>
                    </tr>
                    <tr class="normal" id="TrExperience" runat="server">
                        <td colspan="4" class="rightBorder">
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                            Attested copy of&nbsp; experience certificate obtained by the candidate ( अभ्यर्थी
                            द्वारा प्राप्त अनुभव प्रमाण पत्र की सत्यापित प्रतिलिपि )
                        </td>
                    </tr>
                    <tr class="normal" id="TrDD" runat="server">
                        <td colspan="4" class="rightBorder">
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                            Please attach Demand Draft with the application form and write Application Number,
                            Name, Course applied for at the reverse side of the draft.(कृपया आवेदन पत्र के साथ
                            डिमांड ड्राफ्ट संलग्न करें, इसके पीछे की ओर निम्नलिखित विवरण लिखें: आवेदन संख्या,
                            अभ्यर्थी का नाम और पाठ्यक्रम जिसके लिए आवेदन किया )
                        </td>
                    </tr>--%>
                   
                    <tr class="head1">
                        <td colspan="2">
                            9. Declaration / घोषणा
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td colspan="4" class="rightBorder"  style="text-align: justify;">
                            <span id="tddeclaration1" runat="server" visible="false">
                            Certified that, the information furnished above are true to the best of my knowledge
                            and belief. / मैं एतद द्वारा घोषणा करता/करती हूँ कि मेरे द्वारा प्रस्तुत विवरण /सूचनायें
                            सत्य हैं | 
                            </span>
                            <span id="tddeclaration2" runat="server" visible="false">
                            I,<asp:label id="lblname" runat="server" text=""></asp:label>
                            have read and understood the eligibility criteria of the course
                            <asp:label runat="server" text="" id="lbldeccoursecode" style="font-weight: bold;"></asp:label>
                            as mentioned in the course syllabus and I understand that I am eligible to register
                            myself for this course. If it is discovered at any later stage that I was not eligible
                            to register myself in the
                            <asp:label runat="server" text="" id="lbldeccoursecode1" style="font-weight: bold;">
                            </asp:label>, my registration of the course will automatically be treated as null
                            and void and I will have no claim whatsoever. I further undertake that I have carefully
                            read all the relevant rules/instructions etc. of the NIELIT and I undertake to abide
                            by the same in all respects. I solemnly declare that the particulars filled in by
                            me are correct and nothing has been concealed. In case of any discrepancy found
                            therein, I shall be responsible for the consequences.
                            <br />
                            </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; padding-top: 10px;" cellpadding="1" cellspacing="0">
                    <tr>
                        <td align="center" width="40%">
                            <img src="../images/thumb.jpg" id="imgThumbImpression" style="height: 50px; width: 168px"
                                runat="server" />
                        </td>
                        <td align="center" colspan="2" valign="middle" width="20%">
                            <img id="imgBarCode" runat="server" />
                        </td>
                        <td align="center" width="40%">
                            <img src="../images/sign.jpg" id="imgSignature" style="height: 35px; width: 168px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Left hand thumb impression / बाएं हाथ के अंगूठे का निशान
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center">
                            Signature of Applicant/ हस्ताक्षर
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trnote" visible="false" runat="server" class="gdrow1">
            <td colspan="4">
                <strong style="font-size: 12px; padding-left:2px;">
                <br />
                Note: Registration does not guarantee admission. It will depend on the availability of the seats, minimum batch size and availability
                of slot/resources with the concerned Centre.
                </strong>
            </td>
        </tr>
    </table>
    <div style="text-align: center; margin-top: 10px;" id="divfooter" runat="server">
        <asp:Button ID="Btnsubmit" runat="server" Text="Final Submit" Width="100px" OnClick="Btnsubmit_Click" />
        <asp:Button ID="Btnback" runat="server" Text="Back" Width="50px" OnClick="Btnback_Click" />
    </div>
    <br />
    <asp:HiddenField ID="courseid" runat="server" />
    <asp:HiddenField ID="appid" runat="server" />
    </form>
</body>
</html>
