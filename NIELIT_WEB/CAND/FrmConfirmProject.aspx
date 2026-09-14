<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmConfirmProject.aspx.cs" Inherits="FrmConfirmProject"  Debug="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script src="../Script/GlobalFunction.js" type="text/javascript"></script>--%>
    <%--   <script src="../Script/shortcut.js" type="text/javascript"></script>--%>
    <script language="javascript" type="text/javascript">
        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
        function ValidateFormFields() {
            if (!isBlankNumber("<%=TxtDDnumber.ClientID %>", "Demand Draft No."))
                return false;
            if (!isNumber("<%=TxtDDnumber.ClientID %>", "Not Valid Demand Draft No."))
                return false;
            if (!isBlank("<%=txtDDdate.ClientID %>", "Demand Draft Date"))
                return false;
            if (!isDate("<%=txtDDdate.ClientID %>", "Invalid Demand Draft date", "dd-MMM-yyyy"))
                return false;
            if (!isBlank("<%=TxtBank.ClientID %>", "Bank Name."))
                return false;
            return true;
        }
        function ValidateFormFields1() {
            if (!isBlank("<%=Txtnefttransno.ClientID %>", "Transaction No."))
                return false;
            if (!isBlank("<%=txtneftdate.ClientID %>", "Transaction Date"))
                return false;
            if (!isDate("<%=txtneftdate.ClientID %>", "Invalid Transaction date", "dd-MMM-yyyy"))
                return false;
            if (!isBlank("<%=Txtneftbank.ClientID %>", "Bank Name."))
                return false;
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <center>
        <asp:Label ID="LblError" runat="server" ForeColor="#FF3300"></asp:Label>
        <table class="sample3" width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr align="left">
                <td align="right">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:LinkButton ID="LnkBtnBack" runat="server" Visible="false" Text="Back" OnClick="LnkBtnBack_Click"
                                    Style="font-weight: 700">
                                </asp:LinkButton>
                            </td>
                            <td align="right" width="95%" class="style2">
                                <asp:Label ID="LblPrintForm" runat="server" Text="Click here to Print the Form&nbsp;"></asp:Label>
                                <strong>
                                    <asp:LinkButton ID="LnkBtnPrintForm" runat="server" Height="26px" Text="Print Form"
                                        Style="vertical-align: top;" />
                                </strong>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="head1" align="left">
                <td>
                    <strong>Application Details: </strong>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td class="box" id="TDApplicationHead" runat="server" style="text-align: left; font-size: 10pt;
                    font-style: normal;">
                    <font color='#000099'>Dear Applicant,<br />
                        Your online application for
                        <asp:Label ID="LblECourseName" runat="server"></asp:Label>
                        &nbsp;has been successfully submitted.
                        <br />
                        Application No is :<asp:Label ID="LblAppID1" Font-Bold="true" runat="server"></asp:Label>
                        .
                        <br />
                        Please note your Application Number which will be used further for future correspondence.<br />
                        <asp:Label ID="LblCentreAddressE" runat="server"></asp:Label>
                        <br />
                        प्रिय आवेदक,<br />
                        आप
                        <asp:Label ID="LblHCourseName" runat="server"></asp:Label>
                        <asp:Label ID="LblAppType" runat="server"></asp:Label>
                        के लिए सफलतापूर्वक ऑनलाइन आवेदन कर चुके हैं|<br />
                        आपकी आवेदन संख्या
                        <asp:Label ID="LblAppID2" Font-Bold="true" runat="server"></asp:Label>
                        है|
                        <br />
                        कृपया ध्यान दें कि आपकी आवेदन संख्या, जो भविष्य में पत्राचार के लिए आगे उपयोग की
                        जाएगी, उसे कृपया नोट करें|</font>
                    <br />
                    <font color='#000099'>
                        <asp:Label ID="LblCentreAddressH" runat="server"></asp:Label>
                    </font>
                </td>
            </tr>
            <tr id="Tr1" runat="server">
                <td height="25">
                </td>
            </tr>
            <tr class="head1" id="trpymtdetail" runat="server">
                <td style="text-align: left;">
                    <strong>Payment Details:</strong>
                </td>
            </tr>
            <tr class="gdalternate1" id="trnpaydetails" runat="server" visible="false">
                <td class="box" id="Td1" runat="server" style="text-align: left; font-size: 10pt;
                    font-style: normal;">
                    <asp:label id="lbldemanderror" runat="server" text=""></asp:label>
                    <br />
                </td>
            </tr>
            <tr class="gdalternate1" id="trpaydetails" runat="server" visible="false">
                <td class="box" id="TdPaymentDetail" runat="server" style="text-align: left; font-size: 10pt;
                    font-style: normal;">
                    <font color='#000099'>&nbsp;Demand Note Number is:&nbsp;&nbsp;
                        <asp:Label ID="LblEDemandNoteID" runat="server" Text=""></asp:Label>
                        <br />
                        &nbsp;Demand Date is:&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="LblEDemandDate" runat="server" Text=""></asp:Label>
                        <br />
                        &nbsp;Last Date of Payment is:&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblELastDate" runat="server" Text=""></asp:Label>
                        <br />
                        &nbsp;<asp:Label ID="LblPayAppTypeE2" runat="server"></asp:Label>
                        &nbsp;Fee is:&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="LblEExamAmount" runat="server" Text=""></asp:Label>
                        <br />
                        &nbsp;Please pay your
                        <asp:Label ID="LblPayAppTypeE1" runat="server"></asp:Label>
                        &nbsp;Fee by selecting any of the following payment options:-<br />
                        <br />
                        डिमांड नोट संख्या:&nbsp;&nbsp;
                        <asp:Label ID="LblHDemandNoteID" runat="server" Text=""></asp:Label>
                        &nbsp;है
                        <br />
                        डिमांड तिथि:&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="LblHDemandDate" runat="server" Text=""></asp:Label>
                        है
                        <br />
                        भुगतान की अंतिम तिथि:&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblLastDate" runat="server" Text=""></asp:Label>
                        है
                        <br />
                        <asp:Label ID="LblPayAppTypeH1" runat="server"></asp:Label>
                        &nbsp;शुल्क :
                        <asp:Label ID="LblHExamAmount" runat="server" Text=""></asp:Label>
                        &nbsp;है|<br />
                        कृपया निम्नलिखित भुगतान विकल्पों में से किसी एक का चयन करके अपने
                        <asp:Label ID="LblPayAppTypeH2" runat="server"></asp:Label>
                        &nbsp;शुल्क का भुगतान करें:- </font>
                    <br />
                </td>
            </tr>
            <tr>
                <td height="25">
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <table class="box" width="100%" cellpadding="0" cellspacing="0" id="tblpaymentmode"
                            runat="server">
                            <tr class="head1">
                                <td align="left">
                                    <b>Payment Options:</b>
                                </td>
                            </tr>
                            <tr class="gdalternate1">
                                <td align="left">
                                    <asp:RadioButtonList ID="RdoPaymentMode" runat="server" Width="267px" AutoPostBack="True"
                                        OnSelectedIndexChanged="RdoPaymentMode_SelectedIndexChanged">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <%-- <td>
                </td>--%>
            </tr>
            <tr>
                <td height="25">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="box">
                                <tr class="head1">
                                    <td style="text-align: left;">
                                        <strong>Payment By CSC </strong>:
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="text-align: left;" class="style1">
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="text-align: left;">
                                        Please Print Form and go to nearest Common Service Center and make your Payment 
                                        / कृपया अपने फार्म को प्रिंट करें और अपने निकटतम कॉमन सर्विस सेंटर पर जाए और अपना 
                                        भुगतान करें| <br />
                                        To find the nearest common service center in your area go to <a href= "https://www.csc.gov.in" target="_blank" >
                                        https://www.csc.gov.in</a>
                                        and use VLE Locator /  अपने क्षेत्र में निकटतम सामान्य सेवा केन्द्र खोजने के
                                        लिए इस लिंक पर क्लिक करें 
                                        <a href="https://www.csc.gov.in" target="_blank" > https://www.csc.gov.in </a> &nbsp;और 
                                        VLE लोकेटर का उपयोग करें|
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="text-align: left;">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="box">
                                <tr class="head1">
                                    <td style="text-align: left;">
                                        <strong>Payment By Online:</strong>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="text-align: left; height: 24px;">
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="text-align: left;">
                                        Please make your Payment by Clicking on this link / कृपया इस लिंक पर क्लिक करके
                                        अपना भुगतान करें
                                        <asp:HyperLink ToolTip="Click to pay online" Style="cursor: pointer; color: Blue;"
                                            ID="nPayNow" runat="server">Pay Online</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td style="text-align: left; height: 24px;">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <table width="100%" cellpadding="0" cellspacing="1">
                                <tr class="head1">
                                    <td colspan="2" style="text-align: left;">
                                        <strong>Payment By Demand Draft: </strong>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" width="20%" colspan="2">
                                        <asp:Label ID="LblDD" runat="server" Text="Please fill below mentioned Demand Draft details to complete your application./ <br/> कृपया आपके आवेदन को पूरा करने के लिए नीचे उल्लेख डिमांड ड्राफ्ट का पूरा विवरण भरें"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trdd" runat="server">
                                    <td align="left" width="20%" colspan="2">
                                        <asp:Label ID="LblDDerror" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td align="left" width="30%">
                                        Demand Draft In Favor Of
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="LblAccountName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" width="30%">
                                        Demand Draft Payable At
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="LblPayableAt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td align="left" width="30%">
                                        Demand Draft Number <b class='mandatory'>*</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtDDnumber" runat="server" MaxLength="6" onkeypress="checkNumber(this,6,0,event);"
                                            Width="200px"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr class="trgdalternate1calendar">
                                    <td align="left" width="30%">
                                        Demand Draft Date <b class='mandatory'>*</b>
                                    </td>
                                    <td valign="bottom" align="left">
                                        &nbsp;<asp:TextBox MaxLength="11" ID="txtDDdate" runat="server" Width="160px"></asp:TextBox>
                                        <img id="imgDDDate" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                        <span style="font-size:12px;vertical-align:middle;">(Demand Draft should be of 30 (Thirty) Days Validity)</span>
                                        <asp:CalendarExtender ID="calendar1" TargetControlID="txtDDdate" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgDDDate" runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trddamt" runat="server">
                                    <td align="left" width="30%">
                                        Amount <b class='mandatory'>*</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtAmount" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="trddbname" runat="server">
                                    <td align="left" width="30%">
                                        Bank Name <b class='mandatory'>*</b>
                                    </td>
                                    <td align="left">
                                        <%--<asp:TextBox ID="TxtBank" runat="server" Width="200px" MaxLength="50"></asp:TextBox>--%>
                                        <asp:TextBox ID="TxtBank" MaxLength="50" runat="server"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="aceSearch" ServiceMethod="GetBankNames" FirstRowSelected="true"
                                            ServicePath="~/WS/Common.asmx" TargetControlID="TxtBank" runat="server" MinimumPrefixLength="1"
                                            CompletionInterval="0" EnableCaching="false" CompletionSetCount="10">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="trddsubmit" runat="server">
                                    <td align="left" width="20%">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="BtnSave" runat="server" Text="Submit" OnClick="BtnSave_Click" OnClientClick="return ValidateFormFields();" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View4" runat="server">
                            <table width="100%" cellpadding="0" cellspacing="1">
                                <tr class="head1">
                                    <td colspan="2" style="text-align: left;">
                                        <strong>Payment By NEFT RTGS: </strong>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                <td align="left" width="20%" colspan="2">
                                    <asp:Label ID="Label5" runat="server" Text=" NEFT/RTGS Electronic Transfer Details. / एनईएफटी / आरटीजीएस इलेक्ट्रॉनिक ट्रांसफर विवरण"></asp:Label>
                                </td>
                                </tr>
								 
                                <tr class="gdrow1">
                                    <td align="left" width="30%">
                                        Account Name
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label6" runat="server" Text="NIELIT"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" width="30%">
                                         Bank Name
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label7" runat="server" Text="BANK OF INDIA"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td align="left" width="30%">
                                        Bank Account Number
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label8" runat="server" Text="604820100000012"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" width="30%">
                                        Name of the Branch
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label9" runat="server" Text="CGO COMPLEX BRANCH"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td align="left" width="30%">
                                       Branch Code
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label10" runat="server" Text="6048"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" width="30%">
                                        MICR Code
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label11" runat="server" Text="110013052"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1">
                                    <td align="left" width="30%">
                                        IFSC Code
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label12" runat="server" Text="BKID0006048"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdalternate1">
                                    <td align="left" width="30%" valign="top">
                                        Address of the Bank
                                    </td>
                                    <td align="left">
                                    ELECTRONICS NIKETAN <br/>
                                    6, CGO COMPLEX, LODHI ROAD <br />
                                        NEW DELHI- 110 003.
                                    </td>
                                </tr>

                               
                                <tr class="gdrow1">
                                    <td align="left" width="20%" colspan="2">
                                        <asp:Label ID="Label1" runat="server" Text="Please fill below mentioned NEFT/RTGS details to complete your application./ <br/> कृपया आपके आवेदन को पूरा करने के लिए नीचे उल्लेख  एनईएफटी / आरटीजीएस का पूरा विवरण भरें"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="tr2" runat="server">
                                    <td align="left" width="20%" colspan="2">
                                        <asp:Label ID="Lbnefterror" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                               <%-- <tr class="gdrow1">
                                    <td align="left" width="30%">
                                        Demand Draft In Favor Of
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                                <%--<tr class="gdalternate1">
                                    <td align="left" width="30%">
                                        Demand Draft Payable At
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr class="gdalternate1">
                                    <td align="left" width="30%">
                                        Transaction Number <b class='mandatory'>*</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Txtnefttransno" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr class="trgdrow1calendar">
                                    <td align="left" width="30%">
                                        Transaction Date <b class='mandatory'>*</b>
                                    </td>
                                    <td valign="bottom" align="left">
                                        &nbsp;<asp:TextBox MaxLength="11" ID="txtneftdate" runat="server" Width="160px"></asp:TextBox>
                                        <img id="imgneftdate" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                                            vertical-align: top;" />
                                        <%--(Demand Draft should be of 3 (three) months validity)--%>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtneftdate" PopupPosition="BottomLeft"
                                            Format="dd-MMM-yyyy" PopupButtonID="imgneftdate" runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="tr3" runat="server">
                                    <td align="left" width="30%">
                                        Amount <b class='mandatory'>*</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Txtneftamount" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="gdrow1" id="tr4" runat="server">
                                    <td align="left" width="30%">
                                        Bank Name <b class='mandatory'>*</b>
                                    </td>
                                    <td align="left">
                                        <%--<asp:TextBox ID="TxtBank" runat="server" Width="200px" MaxLength="50"></asp:TextBox>--%>
                                        <asp:TextBox ID="Txtneftbank" MaxLength="50" runat="server"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="GetBankNames"
                                            FirstRowSelected="true" ServicePath="~/WS/Common.asmx" TargetControlID="Txtneftbank"
                                            runat="server" MinimumPrefixLength="1" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr class="gdalternate1" id="tr5" runat="server">
                                    <td align="left" width="20%">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="Btnneftsubmit" runat="server" Text="Submit"  
                                            OnClientClick="return ValidateFormFields1();" onclick="Btnneftsubmit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <%--  <asp:Button ID="BtnCancel" runat="server" Text="Cancel" />--%>
        </div>
    </center>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
