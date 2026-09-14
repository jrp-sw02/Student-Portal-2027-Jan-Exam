<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScholarshipPreview.aspx.cs" Inherits="CAND_ScholarshipPreview" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scholarship Form</title>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
    <table id="tblMain" runat="server" align="center" border="0" cellpadding="0" cellspacing="0"
        width="977px">
        <tr>
            <td colspan="3">
                <uc2:normalheader id="NormalHeader1" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle">
                <%--<asp:label id="Lblhead" runat="server" style="font-size: 20px;" text="[Registration Wing]">
                </asp:label><br />--%>
                <asp:label id="lblHead2" runat="server" style="font-size: 20px;" text="Aadhaar & Bank Account Details Form to avail Scholarship">
                </asp:label>
                <asp:imagebutton id="BtnPrint" runat="server" style="float: right; padding-bottom: 20px;"
                    onclientclick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    imageurl="~/images/print.gif" tooltip="Print Form" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:label id="lblerror" runat="server" enabletheming="false" forecolor="Red"></asp:label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <table style="width: 100%;" class="preview" border="0" cellspacing="0" cellpadding="2">
                    <tr class="head1">
                        <td align="left" width="35%">
                            1. Registration Details
                        </td>
                        <td align="left" width="25%">
                        </td>
                        <td align="left" width="15%">
                            &nbsp;
                        </td>
                        <td align="right" width="25%">
                            <%-- Date: &nbsp;<%= DateTime.Now.ToString("dd-MMM-yyyy") %>--%>
                            Date: &nbsp;<asp:label id="lblDate" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label4" runat="server" text="Registration Number "></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblRegNum" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label5" runat="server" text="Current Level"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblcurrentLevel" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            2. Address For Communication
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label75" runat="server" text="Address1"></asp:label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:label id="LblAdd1" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label77" runat="server" text="Address2"></asp:label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:label id="LblAdd2" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label3" runat="server" text="Address3"></asp:label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:label id="lblAdd3" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td class="normal">
                            <asp:label id="Label76" runat="server" text="City"></asp:label>
                        </td>
                        <td>
                            <asp:label id="LblCity" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label id="Label78" runat="server" text="District"></asp:label>
                        </td>
                        <td class="rightBorder">
                            <asp:label id="LblDistrict" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label79" runat="server" text="State"></asp:label>
                        </td>
                        <td>
                            <asp:label id="LblState" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label id="Label38" runat="server" text="Pin Code"></asp:label>
                        </td>
                        <td class="rightBorder">
                            <asp:label id="LblPinCode" runat="server"></asp:label>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td align="center" valign="middle" colspan="4" style="text-decoration: underline;
                            line-height: 1.3pc; padding-top: 5px;">
                            DATA TO BE CAPTURED<br />
                            Please provide the following information
                        </td>
                    </tr>--%>
                    <tr class="head1">
                        <td colspan="2">
                            3.
                            <asp:label id="Label24" runat="server" text="Applicant's Personal Details "></asp:label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label25" runat="server" text="Name of Candidate "></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="LblAppName" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="trFather">
                        <td>
                            <asp:label id="Label26" runat="server" text="Father's Name "></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="LblFName" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="trMother">
                        <td>
                            <asp:label id="Label27" runat="server" text="Mother's Name"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="LblMName" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="trGuardian" visible="false">
                        <td>
                            <asp:label id="Label28" runat="server" text="Guardian's Name"></asp:label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:label id="LblGName" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label30" runat="server" text="Date of Birth (dd-MMM-yyyy)"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="LblDob" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label32" runat="server" text="Gender"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblgender" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label33" runat="server" text="Physically Handicapped"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblphcap" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label34" runat="server" text="Category"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblcastcategory" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label35" runat="server" text="Annual Income of Parents"></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblanincome" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            4. Contact Details
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label1" runat="server" text="Mobile"></asp:label>
                            &nbsp;Number
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="lblMobile" runat="server"></asp:label>
                            <br />
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label id="Label37" runat="server" text="Email Address "></asp:label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:label id="LblEmail" runat="server"></asp:label>
                            <br />
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            4. Bank Details
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label runat="server" text="Bank Name"></asp:label>
                        </td>
                        <td>
                            <asp:label id="Lblbname" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label  runat="server" text="Bank Address"></asp:label>
                        </td>
                        <td class="rightBorder">
                            <asp:label id="Lblbkaddress" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label runat="server" text="IFSC Code"></asp:label>
                        </td>
                        <td>
                            <asp:label id="Lblifsccode" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label runat="server" text="Account Type"></asp:label>
                        </td>
                        <td class="rightBorder">
                            <asp:label id="lblacctype" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:label runat="server" text="Account Number"></asp:label>
                        </td>
                        <td>
                            <asp:label id="Lblaccno" runat="server"></asp:label>
                        </td>
                        <td>
                            <asp:label runat="server" text="Aadhar Number"></asp:label>
                        </td>
                        <td class="rightBorder">
                            <asp:label id="lbladhar" runat="server"></asp:label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td>
                            5. Attachment
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td colspan="4" class="rightBorder" id="tddoc" runat="server">
                        </td>
                    </tr>
                    <tr class="head1">
                        <td>
                            6. Declaration
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td align="left" valign="top" colspan="4" class="rightBorder">
                            I hereby declare that the above statement and information are correct to the best
                            of my knowledge and belief and I agree and abide by the information above.
                          <%--  <br />--%>
                           <%-- I have attached self attested certificates in support of Father/Mother's or Guardian's
                            Name.--%>
                            <br />
                            <%--<br />--%>
                            <span style="float: right">................................................</span><br />
                            <span style="float: right">[Signature of the Applicant]</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table style="width: 100%; padding-top: 10px;" cellpadding="1" cellspacing="0">
                    <tr>
                        <td align="center" width="33%" valign="bottom">
                            <asp:image id="ImgCandidatePhoto" cssclass="PhotoImage" imageurl="../images/photo.jpg"
                                runat="server" height="110px" width="98px" />
                        </td>
                        <td align="center" width="33%" valign="bottom">
                            <%--<img id="imgBarCode" runat="server" />--%>
                            <img src="../images/sign.jpg" id="imgSignature" style="height: 35px; width: 168px"
                                runat="server" />
                        </td>
                        <td align="center" width="33%" valign="bottom">
                            <img src="../images/thumb.jpg" id="imgThumbImpression" style="height: 45px; width: 168px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Photograph of the Applicant
                        </td>
                        <td align="center">
                            Signature of the Applicant
                        </td>
                        <td align="center">
                            Left hand thumb impression
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <ul>
                                <li>Father's and mother's name must be according to 10th qualifying certificate or any
                                    certificate issued by any Government agency.</li>
                                <li>The scholarship will not be credited unless and until this filled in form is received
                                    whether by post or submitted personally at our following address.</li>
                            </ul>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td colspan="3" align="center">
                            <asp:label id="LblAddNielit" runat="server" text=""></asp:label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    </form>
</body>
</html>
