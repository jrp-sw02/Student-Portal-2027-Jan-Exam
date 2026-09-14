<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="Application.aspx.cs"
    Inherits="ChangeApplication" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Application Form</title>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
    <table id="tblMain" runat="server" align="center" border="0" cellpadding="0" cellspacing="0" width="977px">
        <tr>
            <td colspan="3">
                <uc2:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle">
                <asp:Label ID="Lblhead" runat="server" Style="font-size: 20px;" Text="[Registration Wing]"></asp:Label><br />
                <asp:Label ID="lblHead2" runat="server" Style="font-size: 20px;" Text="Form for capturing details of Parent's Name & Communication Details"></asp:Label>
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 20px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
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
                           Date: &nbsp;<asp:Label ID="lblDate" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Registration Number "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblRegNum" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Current Level"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblcurrentLevel" runat="server"></asp:Label>
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
                            <asp:Label ID="Label75" runat="server" Text="Address1"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblAdd1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label77" runat="server" Text="Address2"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblAdd2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Address3"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="lblAdd3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td class="normal">
                            <asp:Label ID="Label76" runat="server" Text="City"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblCity" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label78" runat="server" Text="District"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblDistrict" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label79" runat="server" Text="State"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblState" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label38" runat="server" Text="Pin Code"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblPinCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="middle" colspan="4" style="text-decoration:underline;line-height:1.3pc;padding-top:5px;">
                         
                           DATA TO BE CAPTURED<br />
                           Please provide the following information
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="2">
                            3.
                            <asp:Label ID="Label24" runat="server" Text="Applicant's Personal Details "></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td >
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label25" runat="server" Text="Name of Candidate "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblAppName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="trFather">
                        <td>
                            <asp:Label ID="Label26" runat="server" Text="Father's Name "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblFName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="trMother">
                        <td>
                            <asp:Label ID="Label27" runat="server" Text="Mother's Name"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblMName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" runat="server" id="trGuardian" visible="false">
                        <td>
                            <asp:Label ID="Label28" runat="server" Text="Guardian's Name"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblGName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label30" runat="server" Text="Date of Birth (dd-MMM-yyyy)"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblDob" runat="server"></asp:Label>
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
                            <asp:Label ID="Label35" runat="server" Text="Phone"></asp:Label>
                            &nbsp;Number&nbsp; with STD
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblPhone" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Mobile"></asp:Label>
                            &nbsp;Number
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="lblMobile" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr class="normal">
                        <td>
                            <asp:Label ID="Label37" runat="server" Text="Email Address "></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblEmail" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                     <tr>
                        <td  valign="middle" colspan="4" style="padding-top:2px; font-style:italic;">
                         (Please provide your active e-mail ID and mobile number. Kindly note that important data/information pertaining to 
                           registration form will be provided to candidates through e-mail/SMS.)<br />
                          
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
                        <td colspan="4" class="rightBorder">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/rightMark2.jpg" />
                            Attested copies of certificates in support of Father/Mother's or Guardian's Name
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
                            <br />
                            I have attached self attested certificates in support of Father/Mother's or Guardian's
                            Name.
                            <br />
                            <br />
                            <span style="float:right">................................................</span><br />
                            <span style="float:right">[Signature of the Applicant]</span>
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
                            <asp:Image ID="ImgCandidatePhoto" CssClass="PhotoImage" ImageUrl="../images/photo.jpg"
                                runat="server" Height="110px" Width="98px" />
                        </td>
                        <td align="center"  width="33%" valign="bottom">
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
                        <td  align="center">
                            Signature of the Applicant
                        </td>
                        <td align="center">
                            Left hand thumb impression
                        </td>
                    </tr>
                  
                    <tr>
                        <td colspan="3" align="left">
                            <ul>
                                <li>This form can also be downloaded from our website www.nielit.in  <span style="text-decoration:underline;">www.nielit.in</span></li>
                                <li>Father's and mother's name must be according to 10th qualifying certificate or any certificate issued by any Government agency.</li>
                               <%-- Commented on 13 Feb 2019 <li>The certificate will not be issued unless and until this filled in form is received whether by post or submitted personally at our following address.</li>--%>
                            </ul>
                        </td>
                    </tr>
                      <tr class="normal">
                        <td colspan="3" align="center" >
                            <asp:Label ID="LblAddNielit" runat="server" Text=""></asp:Label>
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
