<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="ApplicationDetails.aspx.cs" Inherits="HO_ApplicationDetails" Debug="False" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Application Detail"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
        cellspacing="1">
        <tr class="head1">
            <td align="left" colspan="3">
                Application Details
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Application No.
            </td>
            <td width="44%">
                <asp:HyperLink ID="hlklink" runat="server"> <asp:Label ID="lblAppno" runat="server"></asp:Label></asp:HyperLink>
            </td>
            <td style="width: 20%; padding-top: 25px; padding-bottom: 25px;" align="center" rowspan="9"
                valign="top" id="tdphoto" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="90%" cellspacing="3">
                            <tr>
                                <td class="box" align="center" valign="middle" style="background-color: #c9d7e2;
                                    height: 140px;">
                                    <asp:Image ID="ImgCandidatePhoto" ImageUrl="../images/photo.jpg" Width="112" Height="130"
                                        runat="server" Style="text-align: center;" ImageAlign="Middle" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="TdPhotoCaption" class="box" style="background-color: #c9d7e2;">
                                    <asp:ImageButton ID="ImgBtnPrevious" runat="server" Style="float: left; width: 20px;"
                                        ImageUrl="~/images/previous.gif" CommandArgument="P" Enabled="False" OnClick="ToggleImage" />
                                    <asp:Label ID="LblPhotoCaption" runat="server" Text="Photograph"></asp:Label>
                                    <asp:ImageButton ID="ImgBtnNext" ToolTip="Click to view signature" runat="server"
                                        ImageUrl="~/images/next.gif" Style="float: right;" CommandArgument="N" OnClick="ToggleImage" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Application Date
            </td>
            <td width="44%">
                <asp:Label ID="lblDreceive" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Course
            </td>
            <td width="44%">
                <asp:Label ID="lblCourse" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Exam Name
            </td>
            <td width="44%">
                <asp:Label ID="lblExamName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Fee Amount
            </td>
            <td width="44%">
                <asp:Label ID="lblFeeAmt" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                Application Status
            </td>
            <td width="44%">
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Applicant Name
            </td>
            <td width="44%">
                <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrFatherName" runat="server">
            <td width="36%">
                Father's Name
            </td>
            <td width="44%">
                <asp:Label ID="LblFatherName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="TrMotherName" runat="server">
            <td width="36%">
                Mother's Name
            </td>
            <td width="44%">
                <asp:Label ID="LblMotherName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrGardianName" runat="server">
            <td width="36%">
                Guardian Name
            </td>
            <td width="44%">
                <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="trgender" runat="server">
            <td width="36%">
                <asp:Label ID="Label8" runat="server" Text="Gender"></asp:Label>
            </td>
            <td width="44%" colspan="2">
                <asp:Label ID="lblGender" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="trmaritalstatus" runat="server">
            <td width="36%">
                <asp:Label ID="Label9" runat="server" Text="Marital Status"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblMstatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="trdob" runat="server">
            <td width="36%">
                <asp:Label ID="Label12" runat="server" Text="Date of Birth "></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblDob" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label74" runat="server" Text="Correspondence Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                Address
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCaddress" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                <asp:Label ID="Label18" runat="server" Text="Phone with STD Code"></asp:Label>
            </td>
            <td width="44%" colspan="2">
                <asp:Label ID="lblCcontact" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="36%">
                <asp:Label ID="Label40" runat="server" Text="Mobile"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCmobile" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td width="36%">
                <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email "></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCemail" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="head1" id="trperaddress" runat="server">
            <td colspan="3">
                <asp:Label ID="Label2" runat="server" Text="Permanent Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="trperaddressname" runat="server">
            <td width="36%">
                Address
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblPaddress" runat="server">N.A.</asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                Educational/Qualification Details
            </td>
        </tr>
        <tr valign="top" class="gdalternate1">
            <td width="36%">
                <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblHqualification" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="head1" id="trpayment" runat="server" visible="false">
            <td colspan="3">
                Payment Details
            </td>
        </tr>
        <tr valign="top" class="gdalternate1" id="trpaymentmode" runat="server" visible="false">
            <td width="36%">
                <asp:Label ID="Label3" runat="server" Text="Payment Mode"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lbpaymtmode" runat="server"></asp:Label>
                <asp:Button ID="btnDemadNote" runat="server" onclick="btnDemadNote_Click" 
                    Text="Generate Demand Note" Visible="False" Width="160px" />
            </td>
        </tr>
        <tr valign="top" class="gdrow1" id="trappliedas" runat="server" visible="false">
            <td width="36%">
                <asp:Label ID="Label5" runat="server" Text="Applied As"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lbappliedas" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top" class="gdalternate1" id="trpaymentsource" runat="server" visible="false">
            <td width="36%">
                <asp:Label ID="Label4" runat="server" Text="Payment Source/Option"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="10%" style="padding-left: 0px;">
                            <asp:Label ID="lbpaymtsource" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%--<asp:LinkButton ID="lnkchangeapptype" runat="server" Visible="false" OnClick="lnkchangeapptype_Click">Change Applicant Type</asp:LinkButton>--%>
            </td>
        </tr>
        <tr runat="server" visible="false" id="tdradio" class="gdrow1">
            <td colspan="3" align="left">
                <asp:RadioButtonList ID="Rdserachby" runat="server" RepeatDirection="Horizontal"
                    Width="80%" AutoPostBack="True" Style="text-align: left;" Visible="false" OnSelectedIndexChanged="Rdserachby_SelectedIndexChanged">
                    <asp:ListItem Value="1">Change Payment Option</asp:ListItem>
                    <asp:ListItem Value="2">Change Applicant Type</asp:ListItem>
                    <asp:ListItem Value="3">Cancel / Mark Application as Editable</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr class="head1" id="trreg1" runat="server" visible="false">
            <td colspan="3">
                Application Processing Details:-</td>
        </tr>
        <tr class="gdrow1" id="trreg2" runat="server" visible="false">
            <td>
                <asp:Label ID="Label6" runat="server" Text="Exam Name"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblexamname1" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="trreg3" runat="server" visible="false">
            <td>
                <asp:Label ID="Lbreg" runat="server" Text="Registration Number"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCurrntRegNo" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="trreg4" runat="server" visible="false">
            <td >
                <asp:Label ID="Lbregdate" runat="server" Text="Registration Date"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCurrntRegDate" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="trreg5" runat="server" visible="false">
            <td>
                <asp:Label ID="Label14" runat="server" Text="Current Course"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCurrntRegCourse" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="trreg6" runat="server" visible="false">
            <td>
                <asp:Label ID="Label11" runat="server" Text="Commencement From Date"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblCommncmntFrmDate" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="trreg7" runat="server" visible="false">
            <td>
                <asp:Label ID="Lbvaliddate" runat="server" Text="Valid Upto Date"></asp:Label>
            </td>
            <td colspan="2" width="44%">
                <asp:Label ID="lblValidUpToDate" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1" id="trreg8" runat="server" visible="false">
            <td valign="top" colspan="3">
                <asp:Label ID="Lbabeyance" runat="server" Text="NA" style="color:Red; font-size:14px;"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="trreg9" runat="server" visible="false">
            <td colspan="3">
                <asp:Label ID="lbrejected" runat="server" Text="NA" Style="color: Red; font-size: 14px;"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="margin-top: 10px" id="divradio" visible="false" runat="server" class="error">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr class="gdalternate1">
                <td valign="top">
                    <asp:Label ID="lbfilter" runat="server" Text="" Style="text-align: right;"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="right">
                    <asp:Button ID="Btnyes" runat="server" Text="Yes" OnClick="Btnyes_Click" />
                    <asp:Button ID="Btnno" runat="server" Text="No" OnClick="Btnno_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 20px" width="100%" id="divoutput" runat="server" visible="false"
        class="error">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr class="gdalternate1">
                <td valign="top">
                    <asp:Label ID="lboutput" runat="server" Text="" Style="text-align: right;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 10px; text-align: right;" id="div1" runat="server">
        <asp:Button ID="BtnBack" runat="server" Text="Back" OnClick="Btnback_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
