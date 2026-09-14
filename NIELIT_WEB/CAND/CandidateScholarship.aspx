<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateScholarship.aspx.cs" Inherits="CAND_CandidateScholarship" Debug="false" %>

<%@ Register Src="~/UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scholarship Form</title>
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
    <script src="../Script/Date.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateForm() {

            if (!isSelected("ddlannualincome", "Annual Income"))
                return false;

            if (!isSelected("ddlcategory", "Category"))
                return false;

            if (!isBlank("txtaccountnumber", "Account Number"))
                return false;

            if (!isNumber("txtaccountnumber", "Invalid Account Number"))
                return false;

            if (!isSpecialCharacter("txtaccountnumber", "Special characters are not allowed"))
                return false;

            if (!isBlank("txtacctype", "Account Type"))
                return false;

            if (!isSpecialCharacter("txtacctype", "Special characters are not allowed"))
                return false;

            if (!isBlank("txtifsccode", "IFSC Code"))
                return false;

            if (!isSpecialCharacter("txtifsccode", "Special characters are not allowed"))
                return false;

            if (!isBlank("txtbname", "Bank Name"))
                return false;

            if (!isSpecialCharacter("txtbname", "Special characters are not allowed"))
                return false;

            if (!isBlank("txtbaddress", "Bank Address"))
                return false;

            if (!isBlank("txtcode", "Captcha Code"))
                return false;

            if (!ischecked("chkdisclamier", "Declaration"))
                return false;

            return true;
        }
    </script>
</head>
<body style="background-color: #ffffff; margin-bottom: 0;">
    <form id="form2" runat="server">
    <asp:scriptmanager id="ScriptManager1" runat="server">
    </asp:scriptmanager>
    <div>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
            <tr>
                <td>
                    <uc1:NormalHeader ID="NormalHeader1" Visible="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center">
                <br />
                    <h3>
                        <strong style="text-align: center" class="headfont">Aadhaar & Bank Account Details Form
                            to avail Scholarship
                        </strong>
                        <br />
                        <span style="font-size: 14px;">
                        (Filling up of this form does not guarantee you for availing the scholarship facility. You have to fulfil the
                         Eligibility Criteria and apply for the Scholarship separately before submission of the below mentioned details.) 
                         </span>
                    </h3>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblmandatory0" runat="server" text="<font color='red'>*</font> अनिवार्य फिल्डस (Mandatory fields)">
                    </asp:label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:updatepanel id="UpdatePanel3" runat="server">
                        <contenttemplate>
                            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                Width="99%"></asp:Label>
                        </contenttemplate>
                    </asp:updatepanel>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
                        cellspacing="1">
                        <tr class="head1">
                            <td colspan="3">
                                1. Candidate Details / उम्मीदवार का विवरण
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                1.1
                            </td>
                            <td width="40%" >
                               Name / नाम
                            </td>
                            <td width="60%" align="left">
                                <asp:label runat="server" text=""  id="lblcandname"></asp:label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td width="3%">
                              1.2 
                            </td>
                            <td width="40%" >
                                Date of Birth / जन्म तिथि
                            </td>
                            <td width="60%" align="left">
                                <asp:label runat="server" text="" id="lblcanddob"></asp:label>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                1.3
                            </td>
                            <td width="40%">
                                Gender / लिंग
                            </td>
                            <td width="60%" align="left">
                                <asp:label runat="server" text="" id="lblgender"></asp:label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td width="3%">
                                1.4
                            </td>
                            <td>
                                Institute Details / संस्थान का विवरण
                            </td>
                            <td>
                                <asp:label runat="server" text="" id="lblcandinsdetails"></asp:label>
                            </td>
                        </tr>
                        <tr class="gdalternate1" runat="server">
                            <td width="3%" valign="middle">
                                1.5
                            </td>
                            <td width="40%">
                                E-Mail / ईमेल
                            </td>
                            <td width="60%" align="left">
                                <asp:label runat="server" text="" id="lblcandemail"></asp:label>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td width="3%">
                                1.6
                            </td>
                            <td>
                                Mobile Number / मोबाइल नंबर
                            </td>
                            <td>
                                <asp:label runat="server" text="" id="lblcandmobno"></asp:label>
                            </td>
                        </tr>
                        <tr class="gdalternate1" runat="server">
                            <td width="3%" valign="middle">
                                1.7
                            </td>
                            <td width="40%">
                               Annual Income of Parents / माता पिता की वार्षिक आय <font color='RED'>*</font>
                            </td>
                            <td width="60%" align="left">
                               <%-- <asp:textbox id="Txtincome" runat="server" maxlength="4" tabindex="1" width="256px"
                                    autocomplete="off" onkeypress="checkNumber(this,1,2,event);"></asp:textbox>
                                <span style="color: red;">(In Lakhs)</span>--%>
                                <asp:dropdownlist runat="server" id="ddlannualincome" runat="server" tabindex="1" width="256px">
                                    <asp:listitem value="0">--Select One--</asp:listitem>
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr class="gdrow1" runat="server">
                            <td width="3%" valign="middle">
                                1.8
                            </td>
                            <td width="40%">
                                Category / श्रेणी <font color='RED'>*</font>
                            </td>
                            <td width="60%" align="left">
                                <asp:dropdownlist id="ddlcategory" runat="server" width="256px" tabindex="2">
                                    <asp:listitem value="0">--Select One--</asp:listitem>
                                </asp:dropdownlist>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                1.9
                            </td>
                            <td width="40%">
                                <asp:label id="Label71" runat="server" text="Handicapped / विकलांग &lt;font color='RED'&gt;*&lt;/font&gt;">
                                </asp:label>
                            </td>
                            <td width="60%" align="left">
                               <asp:radiobuttonlist id="Rdhandicapped" runat="server" repeatdirection="Horizontal"
                                tabindex="3">
                                <asp:listitem value="N" selected="True">No / नहीं </asp:listitem>
                                <asp:listitem value="Y">Yes / हाँ  </asp:listitem>
                                </asp:radiobuttonlist>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                2.
                                <asp:label id="Label69" runat="server" text="Bank Details / बैंक विवरण ">
                                </asp:label>
                                <span style="font-size:12px;">(Your Account Number needs to be linked with your Aadhaar Number (If Available)
                                    for availing the Scholarship. You need to visit your Bank to link Aadhaar Number
                                    with the Bank Account. In case of Non-Aadhaar Enrolled Students, they need to submit
                                    their Bank Account Details only.) 
                               </span>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                2.1
                            </td>
                            <td>
                                <asp:label id="Label82" runat="server" text="Aadhar Number / आधार संख्या "></asp:label>
                            </td>
                            <td>
                                <asp:textbox id="Txtaadharnumber" runat="server" maxlength="12" tabindex="4" width="256px"
                                    autocomplete="off" onkeypress="checkNumber(this,12,0,event);" onpaste="return false;"
                                    oncopy="return false;" oncut="return false;">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td width="3%">
                                2.2
                            </td>
                            <td>
                                <asp:label id="Label2" runat="server" text="Account Number / खाता संख्या <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td>
                                <asp:textbox id="txtaccountnumber" runat="server" maxlength="16" tabindex="5" width="256px"
                                    autocomplete="off" onpaste="return false;" oncopy="return false;" oncut="return false;"
                                    onkeypress="checkNumber(this,16,0,event);">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="gdalternate1" id="traccholdername" runat="server">
                            <td width="3%">
                                2.3
                            </td>
                            <td>
                                <asp:label id="Label3" runat="server" text="Name of Account Holder / खाता धारक का नाम ">
                                </asp:label>
                            </td>
                            <td>
                                <asp:textbox id="txtaccholdername" runat="server" maxlength="45" width="256px" autocomplete="off"
                                    tabindex="6" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="gdrow1" id="tracctype" runat="server">
                            <td width="3%">
                                2.4
                            </td>
                            <td>
                                <asp:label id="Label4" runat="server" text="Account Type / खाते का प्रकार <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td align="left">
                                <asp:textbox id="txtacctype" runat="server" maxlength="15" width="256px" autocomplete="off"
                                    tabindex="7" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                2.5
                            </td>
                            <td>
                                <asp:label id="Label6" runat="server" text="IFSC code / आईएफएससी कोड <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td>
                                <asp:textbox maxlength="16" id="txtifsccode" runat="server" width="256px" tabindex="8"
                                    autocomplete="off" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td width="3%">
                                2.6
                            </td>
                            <td>
                                <asp:label id="Label7" runat="server" text="Bank Name / बैंक का नाम <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td>
                                <asp:textbox maxlength="50" id="txtbname" runat="server" width="256px" tabindex="9"
                                    autocomplete="off" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td width="3%">
                                2.7
                            </td>
                            <td>
                                <asp:label id="Label5" runat="server" text="Bank Address / बैंक का पता <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td>
                                <asp:textbox maxlength="60" id="txtbaddress" runat="server" width="256px" tabindex="10"
                                    autocomplete="off" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:textbox>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3">
                                3. Document Details / दस्तावेज का विवरण &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" style="width: 3%" valign="top">
                                3.1
                            </td>
                            <td align="left" valign="top">
                                <asp:label id="lblaadhar" runat="server" text="Upload Scanned Copy of Aadhaar Card / आधार कार्ड की स्कैन की हुई कॉपी">
                                </asp:label>
                            </td>
                            <td align="left" valign="top">
                                <asp:fileupload id="fuaadhar" runat="server" onkeypress="return false;" tabindex="11"
                                    width="360px"  />
                                <br />
                                (JPG,JPEG,GIF,PNG image with size upto 250 KB)
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" style="width: 3%" valign="top">
                                3.2
                            </td>
                            <td align="left" valign="top">
                                <asp:label id="lblsign" runat="server" text="Upload Scanned Copy of Income Certificate of Parents (To be issued by SDM/Tehsildar) / माता पिता की आय प्रमाण पत्र की स्कैन की हुई कॉपी (एसडीएम / तहसीलदार द्वारा जारी किए जाने वाले)  <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td align="left" valign="top">
                                <asp:fileupload id="fuincomecertificate" runat="server" onkeypress="return false;"
                                    tabindex="12" width="360px"  />
                                <br />
                                (JPG,JPEG,GIF,PNG image with size upto 250 KB)
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" style="width: 3%" valign="top">
                                3.3
                            </td>
                            <td align="left" valign="top">
                                <asp:label id="lblThumb" runat="server" text="Upload Scanned Copy of PH Certificate / शारीरिक रूप से विकलांग प्रमाणपत्र की स्कैन की हुई कॉपी ">
                                </asp:label>
                            </td>
                            <td align="left" valign="top">
                                <asp:fileupload id="fuhandicapped" runat="server" onkeypress="return false;" tabindex="13"
                                    width="360px" />
                                <br />
                                (JPG,JPEG,GIF,PNG image with size upto 250 KB)
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" style="width: 3%" valign="top">
                                3.4
                            </td>
                            <td align="left" valign="top">
                                <asp:label id="lblcaste" runat="server" text="Upload Scanned copy of Caste Certificate / जाति प्रमाण पत्र की स्कैन की हुई कॉपी <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td align="left" valign="top">
                                <asp:fileupload id="fucastecertificate" runat="server" onkeypress="return false;" tabindex="14"
                                    width="360px" />
                                <br />
                                (JPG,JPEG,GIF,PNG image with size upto 250 KB)
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" style="width: 3%" valign="top">
                                3.5
                            </td>
                            <td align="left" valign="top">
                                <asp:label id="lblaccount" runat="server" text="Upload Scanned Copy of Cancelled Cheque OR Scanned Copy of Bank Account Pass Book  / रद्द चैक या बैंक खाता पास बुक की स्कैन की हुई कॉपी <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td align="left" valign="top">
                                <asp:fileupload id="fuaccount" runat="server" onkeypress="return false;" tabindex="15"
                                    width="360px" />
                                <br />
                                (JPG,JPEG,GIF,PNG image with size upto 250 KB)
                            </td>
                        </tr>
                        <tr class="gdalternate1">
                            <td align="left" style="width: 3%" valign="top">
                                3.6
                            </td>
                            <td align="left" valign="top">
                                <asp:label id="Label31" runat="server" text="Code / कोड <font color='RED'>*</font>">
                                </asp:label>
                            </td>
                            <td align="left">
                                <asp:updatepanel id="UpdatePanel5" runat="server" updatemode="Conditional">
                                    <contenttemplate>
                                        <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td align="left" style="width: 90%" valign="middle">
                                                    <img id="imgcap" runat="server" alt="Capture Code" width="150" height="55" src="" />
                                                    <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                                        Width="30px" OnClick="ImgBtnRefresh_Click" />
                                                </td>
                                                <td align="left" style="width: 10%">
                                                    <asp:HiddenField ID="HfCaptcha" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtcode" runat="server" MaxLength="6" TabIndex="16" Width="139px"
                                                        autocomplete="off" onpaste="return false;" oncopy="return false;"
                                                        oncut="return false;">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </contenttemplate>
                                </asp:updatepanel>
                            </td>
                        </tr>
                        <tr class="head1">
                            <td colspan="3" id="tddeclarartion" runat="server">
                                4. Declaration / घोषणा
                            </td>
                        </tr>
                        <tr class="gdrow1">
                            <td align="left" valign="top" colspan="3">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="style107">
                                        </td>
                                        <td style="text-align: justify;">
                                            &nbsp;<asp:checkbox id="chkdisclamier" runat="server" tabindex="17" text="<font color='RED'>*</font>" />
                                            I,hereby declare that the particulars submitted by me in the Aadhaar & Bank Account
                                            details form to avail scholarship are true to the best of my knowledge and belief. I agree to abide by the rules and
                                            regulations of NIELIT.
                                            <br />
                                             मैं,यह घोषणा करता/ करती हूँ कि छात्रवृत्ति हेतु मेरे आधार और बैंक खाते कि वर्णित समस्त जानकारी मेरे ज्ञान
                                            और विश्वास से सत्य है। मैं रा.इ.सू.प्रौ.सं. के निर्णयों से भी सहमत हूँ। 
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <br />
                                <asp:button id="btnSave" runat="server" text="Submit" tabindex="18"
                                    onclientclick="return ValidateForm();" onclick="btnSave_Click" />
                                <asp:button id="btnback" runat="server" text="Cancel" tabindex="19" 
                                    onclick="btnback_Click"  />
                            </td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
