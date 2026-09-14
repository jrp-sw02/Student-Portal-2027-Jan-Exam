<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master"
    CodeFile="IDcard.aspx.cs" Inherits="IDcard" %>

<%@ Register Src="UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function ValidateFormFields() {
            if (document.getElementById("<%=ddlCourseLevel.ClientID %>").value == 0)
                return false;
        }
        function printStatus() {
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Student ID Card"></asp:Label>
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server">
    </asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContents" runat="Server">
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div id="divGrid" runat="server">
                <table width="99%" cellspacing="0" cellpadding="0" id="tblheader">
                    <tr>
                        <td align="center" style="font-weight: bold; font-size: 12px;" width="40%">
                            <asp:Label ID="lblName" runat="server" Text="Course Name"></asp:Label>
                        </td>
                        <td style="font-size: 12px;" width="60%">
                            <asp:DropDownList ID="ddlCourseLevel" runat="server" Width="100%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right; margin-top: 10px; margin-right: 8px;">
                    <asp:Button ID="btnView" runat="server" Text="View ID Card" OnClick="btnView_Click"
                        OnClientClick="return ValidateFormFields();" />
                </div>
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div id="divIDcard" runat="server" style="width: 450px; height: 548px; margin: 0px auto auto auto;">
                <div style="width: 420px; height: 250px; text-align: center; margin: auto; margin-top: 10px;
                    border: thin solid black;" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" style="padding-top: 10px;">
                        <tr>
                            <td style="vertical-align: top; width: 15%;" rowspan="3">
                                <img id="NielitLogo" src="images/Nielitlogo.jpg" height="50px" width="70px" />
                            </td>
                            <td style="font-size: 10px; width: 85%; vertical-align: top;">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="font-size: 12px; vertical-align: top; font-weight: bold;">
                                            National Institute of Electronics and Information Technology
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 10px; vertical-align: top;">
                                            (An Autonomous Scientific Society Under Department of Electronics & I.T.<br />
                                            Ministry of Communications & Information Technology, Government of India)
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="text-align: left; font-weight: bold; padding-left: 5px; padding-right: 0px;
                                font-size: 11px">
                                Registration Allocation-cum-Identity Card
                            </td>
                            <td style="text-align: right; font-size: 11px; padding-left: 0px; padding-right: 5px;
                                font-weight: bold;">
                                Validity:<asp:Label ID="RegnValidity" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="vertical-align: top; width: 30%;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center" style="vertical-align: top; padding-top: 10px; padding-left: 10px;">
                                            <asp:Image ID="ImgApplicantPhoto" CssClass="DivImage" ImageUrl="images/photologo.jpg"
                                                runat="server" Height="80px" Width="90px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="vertical-align: top; padding-top: 10px; padding-left: 10px">
                                            <asp:Image ID="imgSignature" CssClass="DivImage" ImageUrl="images/signlogo.jpg" runat="server"
                                                Height="30px" Width="90px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top; width: 70%; padding-top: 5px;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Registration Number :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblRegnNumber" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Level :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblCourse" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Name :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblAppName" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Date of Birth :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblDob" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="TrFatherName" runat="server">
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Father's Name :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblFName" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="TrMotherName" runat="server">
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Mother's Name :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblMName" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="TrGardianName" runat="server">
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Guardian's Name :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; text-align: left;">
                                            <asp:Label ID="LblGuardianName" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; width: 35%; font-size: 10px; text-align: right">
                                            Address :&nbsp;
                                        </td>
                                        <td style="font-size: 10px; min-height: 40px; text-align: left;">
                                            <asp:Label ID="LblAddress" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="35%">
                                        </td>
                                        <td align="right" style="padding-right: 0px; min-height:35px; padding-bottom: 0px;">
                                            <asp:Image ID="imgHeadSignature" CssClass="PhotoImage" ImageUrl="images/HeadSignature.jpg"
                                                runat="server" Height="30px" Width="90px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="35%">
                                        </td>
                                        <td style="vertical-align: bottom; text-align: right; padding-right: 5px; font-size: 10px">
                                            Authorized Signatory
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 420px; text-align: center; margin: auto; border: thin solid black;">
                    <table width="100%">
                        <tr>
                            <td style="height: 15px;">
                                <img id="crop" src="images/cropping.jpg" height="10px" width="200px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 420px; height: 250px; text-align: center; margin: auto; margin-bottom: 10px;
                    border: thin solid black;">
                    <table width="100%">
                        <tr>
                            <td style="text-align: center; font-size: 12px; text-decoration: underline; padding-bottom: 0px;
                                font-weight: bold;">
                                Instructions
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: justify; vertical-align: top; font-size: 9px; padding-right: 5px;
                                padding-top: 0px; padding-bottom: 0px; padding-left: 0px;">
                                <ol style="padding-bottom: 0px;">
                                    <li>This Registration-cum-Identity card is for the limited purpose for proving Candidate's
                                        Identity at NIELIT Examination center only.</li>
                                    <li>This Card is valid for the particular level and the period specified overleaf, unless
                                        specifically extended.</li>
                                    <li>This registration number is to be quoted in all correspondence with the Society.</li>
                                    <li>Please go through the details of Name, Date of Birth, Father's Name, Mother's Name
                                        or Guardian's Name, Level, Address in case of find any inaccuracy report back the
                                        same immediately</li>
                                    <li>Re-registration is to be applied within one year after expiry of validity period
                                        of registration number.</li>
                                    <li>Loss of this card should be reported to the nearest Police Station and to the NIELIT.</li>
                                </ol>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; padding-top: 0px; font-size: 9px; font-weight: bold;">
                                For any further information please visit to our website http://www.nielit.gov.in<br />
                                Online Student Portal: https://student.nielit.gov.in<br />
                                National Institute of Electronics and Information Technology
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; padding-top: 0px; font-size: 9px;">
                                Electronics Niketan<br />
                                6, CGO Complex, Lodhi Road, New Delhi-110003<br />
                                Tel:-011-24363330,31,32,011-24366577,79,80,Fax:-011-24363335,E-mail: regn@nielit.gov.in
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 450px; text-align: right; margin: auto; margin-top: 50px; border: none;">
                <asp:ImageButton OnClientClick="return printStatus();" ClientIDMode="Static" AlternateText="Print"
                    ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
